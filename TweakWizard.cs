using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

using PotionCraft.ManagersSystem.BuildMode;
using PotionCraft.ManagersSystem.Room;
using PotionCraft.ObjectBased;
using PotionCraft.ScriptableObjects.BuildZone;
using PotionCraftAutoGarden.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Ukersn_s_TweakWizard
{
    [BepInPlugin("com.ukersn.plugin.TweakWizard", "Ukersn's TweakWizard", "1.0.0")]
    public class TweakWizard : BaseUnityPlugin
    {
        Harmony harmony = new Harmony("com.ukersn.plugin.TweakWizard");
        private static bool lastBuildMode = false;
        private static bool currentBuildMode = false;
        private static ConfigEntry<bool> enableUnrestrictedPlanting;
        private static ConfigEntry<bool> enableFrameRateOptimization;
        private static ConfigEntry<bool> enableExtremeFrameRateOptimization;
        void Start()
        {
            Logger.LogInfo("uk优化插件正在加载..");
            LoggerWrapper.Init(Logger);
            LocalizationWrapper.Init();
            harmony.Patch(AccessTools.Method(typeof(RoomManager), "GoTo"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(GoToPostfix)));
            //harmony.Patch(AccessTools.Method(typeof(RoomManager), "OnLoad"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(OnLoadPostfix)));
            // 获取插件的配置
            var config = Config;
            // 配置项1：开启无限制种植
            enableUnrestrictedPlanting = config.Bind("General",
                                                     "EnableUnrestrictedPlanting",
                                                     true,
                                                     "Enable unrestricted planting\n" +
                                                     "Turn this on, and you can now plant plants and crystals overlapping each other\n" +
                                                     "开启它，你现在可以让植物和水晶重叠在一起种植了");

            // 配置项2：帧数优化功能
            enableFrameRateOptimization = config.Bind("Performance",
                                                      "EnableFrameRateOptimization",
                                                      false,
                                                      "Warning: This is an experimental option\n" +
                                                      "Turn this on to reduce rendering of items in non-adjacent rooms, potentially improving game FPS by up to 5%\n" +
                                                      "I haven't fully tested it yet, it might cause save corruption or item loss.\n" +
                                                      "警告：这是一个实验选项\n" +
                                                      "开启它，会减少非相邻房间的物品渲染，换来约5%的游戏FPS提升\n" +
                                                      "我还没完全测试它，它可能会导致存档损坏或物品丢失。");

            // 配置项3：极端帧数优化功能
            enableExtremeFrameRateOptimization = config.Bind("Performance",
                                                             "EnableExtremeFrameRateOptimization",
                                                             false,
                                                             "Note: Only effective when Frame Rate Optimization is enabled\n" +
                                                             "Turn this on to reduce rendering of items in rooms other than the current one, potentially improving game FPS by up to 9%.\n" +
                                                             "However, when switching to other rooms, you'll see items from the previous room appear to 'vanish'.\n" +
                                                             "注意：仅在帧数优化功能开启后才有效\n" +
                                                             "开启它，会减少非本房间的物品渲染，换来约9%的游戏FPS提升。\n" +
                                                             "但在切换其他房间的时候，你会看见切换前房间的物品好像'隐身'了。");

        }
        void Awake()
        {

            ApplyBuildModePatch();

            //Harmony.CreateAndPatchAll(typeof(TweakWizard));
            harmony.Patch(AccessTools.Method(typeof(BuildableItemFromInventory), "UpdateActualColliderInCurrentBuildZoneSet"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateActualColliderInCurrentBuildZoneSetPostfix)));
            harmony.PatchAll();
        }
        void Update()
        {
        }
        private void ApplyBuildModePatch()
        {
            // 获取 BuildModeManager 类型
            Type buildModeManagerType = typeof(BuildModeManager);
            // 补丁 IsTemporaryEnabled IsContinuousEnabled

            PatchProperty(harmony, buildModeManagerType, "IsTemporaryEnabled", nameof(IsTemporaryEnabledSetterPostfix));
            PatchProperty(harmony, buildModeManagerType, "IsContinuousEnabled", nameof(IsContinuousEnabledSetterPostfix));


        }



        [HarmonyPatch(typeof(RoomManager), "GoTo")]
        [HarmonyPostfix]
        public static void GoToPostfix(RoomIndex newTargetRoom, bool forceImmediately = false) //场景：开着建筑模式切换
        {
            //无限制种植相关
            if (enableUnrestrictedPlanting.Value) {
                GameObjectHelper.GetGardenRoomSeedColliders();
                GameObjectHelper.SetBuildZoneColliders(!currentBuildMode);
            }
            //帧数优化相关
            if (enableFrameRateOptimization.Value) {
                GameObjectHelper.GetNonAdjacentRoomsToCurrentRoom(newTargetRoom);
                GameObjectHelper.SetAdjacentRoomsActiveAndOthersInactive(newTargetRoom, enableExtremeFrameRateOptimization.Value);
            }


        }

        [HarmonyPatch(typeof(BuildableItemFromInventory), "UpdateActualColliderInCurrentBuildZoneSet")]
        [HarmonyPostfix]
        public static void UpdateActualColliderInCurrentBuildZoneSetPostfix(bool isItemPlacedOnScene)
        {
            //无限制种植相关
            if (enableUnrestrictedPlanting.Value)
            {
                GameObjectHelper.GetGardenRoomSeedColliders();
                GameObjectHelper.SetBuildZoneColliders(!currentBuildMode);
            }
        }

        //PotionCraft.ManagersSystem.Room.RoomManager.OnLoad() : void @06002BC7
        //[HarmonyPatch(typeof(RoomManager), "OnLoad")]
        //[HarmonyPostfix]
        //public static void OnLoadPostfix()
        //{
        //    LoggerWrapper.LogInfo("加载存档");


        //    //GameObjectHelper.SetAdjacentRoomsActiveAndOthersInactive(RoomIndex.Laboratory);
        //}

        #region 无限制种植
        private void PatchProperty(Harmony harmony, Type type, string propertyName, string postfixMethodName)
        {
            PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                Logger.LogError($"无法找到 {propertyName} 属性");
                return;
            }

            MethodInfo setter = prop.GetSetMethod();
            if (setter == null)
            {
                Logger.LogError($"无法找到 {propertyName} 的 setter 方法");
                return;
            }
            harmony.Patch(setter, postfix: new HarmonyMethod(typeof(TweakWizard), postfixMethodName));
        }
        static void IsTemporaryEnabledSetterPostfix(BuildModeManager __instance, bool value){ LogBuildModeStatus(__instance); }
        static void IsContinuousEnabledSetterPostfix(BuildModeManager __instance, bool value){ LogBuildModeStatus(__instance); }
        static void LogBuildModeStatus(BuildModeManager instance)
        {
            if (instance.IsBuildModeEnabled != lastBuildMode) {
                lastBuildMode = instance.IsBuildModeEnabled;
                onSwitchBuildMode(instance.IsBuildModeEnabled);
            }
        }
        static void onSwitchBuildMode(bool isBuildModeEnable) {
            //LoggerWrapper.LogInfo($"建筑模式切换：当前建筑模式状态: IsBuildModeEnabled = {isBuildModeEnable}");
            if (enableUnrestrictedPlanting.Value)
            {
                GameObjectHelper.SetBuildZoneColliders(!isBuildModeEnable);
            }
            currentBuildMode = isBuildModeEnable;

        }


        #endregion 无限制种植


    }
}