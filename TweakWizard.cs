using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using PotionCraft;
using PotionCraft.InventorySystem;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.BuildMode;
using PotionCraft.ManagersSystem.Room;
using PotionCraft.ObjectBased;
using PotionCraft.ObjectBased.PhysicalParticle;
using PotionCraft.ObjectBased.Stack;
using PotionCraft.ObjectBased.Stack.StackItem;
using PotionCraft.ObjectBased.Stack.StackItem.GrindedSubstance;
using PotionCraft.ScriptableObjects;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraftAutoGarden.Utilities;
using System;
using System.Linq;
using System.Reflection;
using Stack = PotionCraft.ObjectBased.Stack.Stack;

using UnityEngine;
using System.Collections;
using PotionCraft.ObjectOptimizationSystem;
using PotionCraft.ObjectBased.Garden;
using PotionCraft.InputSystem;
using UnityEngine.InputSystem;
using System.Threading;
using System.Collections.Generic;


namespace Ukersn_s_TweakWizard
{
    [BepInPlugin("com.ukersn.plugin.TweakWizard", "Ukersn's TweakWizard", "1.2.1")]
    public class TweakWizard : BaseUnityPlugin
    {
        private static Harmony harmony = new Harmony("com.ukersn.plugin.TweakWizard");
        private static bool lastBuildMode = false;
        private static bool currentBuildMode = false;
        private static bool isFirstLoaded = false;
        private static ConfigEntry<bool> enableUnrestrictedPlanting;
        private static ConfigEntry<bool> enableOneClickGrinding;
        private static ConfigEntry<bool> disableParticleEffects;
        private static ConfigEntry<bool> disableScratchesEffects;

        void Awake()
        {

            Logger.LogInfo("uk优化插件正在加载..");
            // 获取插件的配置
            var config = Config;
            // 配置项1：开启无限制种植
            enableUnrestrictedPlanting = config.Bind("General",
                                                     "EnableUnrestrictedPlanting",
                                                     true,
                                                     "Enable unrestricted planting\n" +
                                                     "Turn this on, and you can now plant plants and crystals overlapping each other\n" +
                                                     "开启它，你现在可以让植物和水晶重叠在一起种植了");
            // 配置项2：一键研磨
            enableOneClickGrinding = config.Bind("General",
                                                 "EnableOneClickGrinding",
                                                 true,
                                                 "Enable one-click grinding\n" +
                                                 "Turn this on to instantly grind ingredients to their fully ground state when right-clicking them in the inventory while in the laboratory(does not affect crystals)\n" +
                                                 "Hold Shift while right-clicking to send the fully ground ingredient directly into the cauldron\n" +
                                                 "开启后，在实验室中右键点击背包里的原料时，可以将其瞬间研磨至完全研磨状态（对水晶无效）\n" +
                                                 "同时按住Shift键右击可以将完全研磨的原料直接送入坩埚");
            // 配置项3：关闭游戏粒子效果
            disableParticleEffects = config.Bind("Performance",
                                                      "DisableParticleEffects",
                                                      true,
                                                      "Disable particle effects in the game\n" +
                                                      "Turn this on to disable as many shiny effects as possible (such as crystals, decorations, etc.)\n" +
                                                      "This can improve frame rate by about 10%\n" +
                                                      "开启后，将尽可能关闭游戏中那些金光闪闪的效果（比如水晶，装饰品等）\n" +
                                                      "这可以提高约10%的帧数");

            // 配置项4：关闭药水和NPC身上的磨损效果
            disableScratchesEffects = config.Bind("Performance",
                                                  "DisableScratchesEffects",
                                                  false,
                                                  "Disable scratches effects on potions and NPCs\n" +
                                                  "Turn this on to disable Scratches effects on potions and NPCs as much as possible\n" +
                                                  "It might look a bit strange, but it can improve frame rate by about 3%!\n" +
                                                  "开启后，将尽可能关闭游戏中在药水和NPC身上的磨损效果\n" +
                                                  "看起来可能有点怪怪的，但它可以提高约3%的帧数！");



            //用于无限制种植
            if (enableUnrestrictedPlanting.Value)
            {
                ApplyBuildModePatch();
                harmony.Patch(
                    AccessTools.Method(typeof(BuildableItemFromInventory), "UpdateActualColliderInCurrentBuildZoneSet"),
                    postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateActualColliderInCurrentBuildZoneSetPostfix)));
            }

            //harmony.PatchAll();
        }

        void Start()
        {
            LoggerWrapper.Init(Logger);
            LocalizationWrapper.Init();

            //用于无限制种植
            if (enableUnrestrictedPlanting.Value)
            {
                harmony.Patch(AccessTools.Method(typeof(RoomManager), "GoTo"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(GoToPostfix)));
            }



            harmony.Patch(AccessTools.Method(typeof(RoomManager), "OnLoad"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(OnLoadPostfix)));            
            //用于一键研磨
            if (enableOneClickGrinding.Value)
            {
                if (SmashHandler.Instance == null)
                {
                    GameObject smashHandlerObject = new GameObject("SmashHandler");
                    smashHandlerObject.AddComponent<SmashHandler>();
                }
            }








        }

        void Update()
        {
            //if (Keyboard.current.f3Key.wasPressedThisFrame)
            //{
            //    //GameObjectHelper.StopAllParticleSystemsAndDisableSpriteScraches();
            //    GameObjectHelper.StopAllParticleSystems();
            //}

        }
        [HarmonyPatch(typeof(RoomManager), "OnLoad")]
        [HarmonyPostfix]
        public static void OnLoadPostfix()
        {
            LoggerWrapper.LogInfo("加载存档");
            if (isFirstLoaded == false)
            {
                isFirstLoaded = true;

                //用于一键研磨
                if (enableOneClickGrinding.Value)
                {
                    // 应用 IngredientTakeFromInventoryPatch
                    harmony.Patch(
                    AccessTools.Method(typeof(Ingredient), "TakeFromInventory"),
                    prefix: new HarmonyMethod(typeof(IngredientTakeFromInventoryPatch), nameof(IngredientTakeFromInventoryPatch.Prefix)),
                    postfix: new HarmonyMethod(typeof(IngredientTakeFromInventoryPatch), nameof(IngredientTakeFromInventoryPatch.Postfix)));

                    // 应用 SpawnNewItemStackPostfix
                    harmony.Patch(
                        AccessTools.Method(typeof(Stack), "SpawnNewItemStack"),
                        postfix: new HarmonyMethod(typeof(TweakWizard), nameof(SpawnNewItemStackPostfix)));

                    // 应用 SpawnNewItemStackPostfix
                    harmony.Patch(AccessTools.Method(typeof(Stack), "Smash"), prefix: new HarmonyMethod(typeof(TweakWizard), nameof(SmashPrefix)));
                }


                //用于帧数优化
                if (disableParticleEffects.Value)
                {
                    harmony.Patch(AccessTools.Method(typeof(GrowthHandler), "UpdateSprite"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateSpritePostfix)));
                }
                //harmony.Patch(AccessTools.Method(typeof(GrowingSpotController), "OnBuildableItemFromInventoryInit"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(OnBuildableItemFromInventoryInitPostfix)));
                //harmony.Patch(AccessTools.Method(typeof(BuildableItemFromInventoryVisualObjectController), "UpdateVisualState"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateVisualStatePostfix)));

            }
            //用于帧数优化
            if (disableParticleEffects.Value || disableScratchesEffects.Value) {
                GameObjectHelper.StopAllParticleSystems(disableParticleEffects.Value,disableScratchesEffects.Value);
            }

            //GameObjectHelper.SetAdjacentRoomsActiveAndOthersInactive(RoomIndex.Laboratory);
        }





        #region 帧数优化
        //UpdateSprite()

        [HarmonyPatch(typeof(GrowthHandler), "UpdateSprite")]
        [HarmonyPostfix]
        public static void UpdateSpritePostfix(GrowthHandler __instance)
        {
            if (__instance == null) return;
            GrowingSpotController growingSpotController = CommonUtils.GetPropertyValueS<GrowingSpotController>(__instance, "GrowingSpot");
            Transform backgroundTransform = growingSpotController.gameObject.transform.Find("Default GrowingSpot VisualObject/Visual Object/Backround");
            GameObjectHelper.StopGameObjectParticleSystems(backgroundTransform);


            //ParticleAndSpriteManager.StopParticleSystemsAndDisableSprites(targetObject);
        }
        //[HarmonyPatch(typeof(GrowingSpotController), "OnBuildableItemFromInventoryInit")]
        //[HarmonyPostfix]
        //public static void OnBuildableItemFromInventoryInitPostfix(GrowingSpotController __instance)
        //{
        //    if (__instance == null || __instance.gameObject == null) return;
        //    Transform backgroundTransform = __instance.gameObject.transform.Find("Default GrowingSpot VisualObject/Visual Object/Backround");
        //    GameObjectHelper.StopGameObjectParticleSystems(backgroundTransform);
        //}

        //[HarmonyPatch(typeof(BuildableItemFromInventoryVisualObjectController), "UpdateVisualState")]
        //[HarmonyPostfix]
        //public static void UpdateVisualStatePostfix(BuildableItemFromInventoryVisualObjectController __instance, BuildableItemFromInventoryState value)
        //{
        //    if (__instance == null || __instance.gameObject == null) return;
        //    Transform backgroundTransform = __instance.gameObject.transform.Find("Visual Object/Backround");
        //    GameObjectHelper.StopGameObjectParticleSystems(backgroundTransform);
        //}




        #endregion 帧数优化



        #region 一键研磨


        // 非静态上下文类 用于存储TakeFromInventory触发时相关变量
        public class TakeFromInventoryContext
        {
            public Stack stack;
            public bool isShiftKeyDown;
            public bool isStackCrystal; //水晶不进行碾压处理，因为研磨有bug(但支持Shift右键进入坩埚)

            public TakeFromInventoryContext(Stack stack, bool isShiftKeyDown)
            {
                this.stack = stack;
                this.isShiftKeyDown = isShiftKeyDown;
                this.isStackCrystal = GameObjectHelper.IsStackCrystal(SmashHandler.Instance.LastCreatedStack);
            }
        }


        [HarmonyPatch(typeof(Ingredient), "TakeFromInventory")]
        public static class IngredientTakeFromInventoryPatch
        {
            static bool isShiftKeyDown;
            [HarmonyPrefix]
            public static void Prefix(Ingredient __instance, ItemsPanel itemsPanel, int count = 1, bool grab = true)
            {
                SmashHandler.Instance.LastCreatedStack = null;
                isShiftKeyDown = Commands.cursorMassActionModifier.State == State.Downed;
            }
            [HarmonyPostfix]
            public static void Postfix(Ingredient __instance, ItemsPanel itemsPanel, int count = 1, bool grab = true)
            {//当从背包中拿出可以被研磨的成分物品时触发该方法。
                //LoggerWrapper.LogInfo($"从背包中取出了 {count} 个 {__instance.name}");
                if (SmashHandler.Instance.LastCreatedStack != null && Managers.Room.CurrentRoomIndex == RoomIndex.Laboratory)
                {
                    //LoggerWrapper.LogInfo($"当前房间是实验室");
                    if (!grab)
                    {
                        //LoggerWrapper.LogInfo($"这是通过右键召唤的物品: {count} 个 {__instance.name}");
                        if (SmashHandler.Instance != null) 
                        {
                            var context = new TakeFromInventoryContext(SmashHandler.Instance.LastCreatedStack,isShiftKeyDown);
                            SmashHandler.Instance.TrySmashWithDelay(context);
                        }
                        else
                        {
                            LoggerWrapper.LogInfo($"SmashHandler.Instance为null，无法进行一键碾压");
                        }

                    }
                }
            }
        }

        [HarmonyPatch(typeof(Stack), "SpawnNewItemStack")]
        [HarmonyPostfix]
        public static void SpawnNewItemStackPostfix(Stack __result)
        {//用于捕获最近创建的Stack配合TakeFromInventory 补丁完成自动碾压
            // 存储创建的 Stack
            SmashHandler.Instance.LastCreatedStack = __result;
        }





        public class SmashHandler : MonoBehaviour
        {
            public static SmashHandler Instance;
            public Stack LastCreatedStack;

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            public void TrySmashWithDelay(TakeFromInventoryContext context)
            {
                    StartCoroutine(SmashWithDelay(context));
            }

            private IEnumerator SmashWithDelay(TakeFromInventoryContext context)
            {

                float elapsedTime = 0f;

                while (elapsedTime < 1.5f)
                {

                    
                    if (!context.stack.isFallingFromPanel)
                    {
                        //LoggerWrapper.LogInfo($"Stack {stack.name} 准备好被碾压");;
                        StartCoroutine(SmashCom(context)); // 启动通用协程
                        yield break;
                    }
                    elapsedTime += 0.1f;

                    yield return new WaitForSeconds(0.1f);

                }
                LoggerWrapper.LogInfo($"Stack {context.stack.name} 准备被碾压的时间超时，可能被收取，取消碾压。"); ;
            }
            private IEnumerator SmashCom(TakeFromInventoryContext context)
            {

                CustomSmashLogic.BeginCustomLogic();
                try
                {
                    PhysicsOptimizer.EnableRigidbodySimulation(context.stack);//需要加这个才能解决bug，bug多到不如快捷键，烦死了。
                    if (!context.isStackCrystal) yield return ExecuteActionMultipleTimes(() => context.stack.Smash(),12, context.stack); //10
                    if (context.stack != null && context.stack.substanceGrinding != null && !context.isStackCrystal) CustomSmashLogic.SubstanceGrindingTryToGrind(context.stack.substanceGrinding);
                    if (context.isShiftKeyDown) {
                        context.stack.leavesGrindStatus = 1f;
                        context.stack.overallGrindStatus = 1f;
                    }

                }
                finally
                {
                    CustomSmashLogic.EndCustomLogic();
                }


            }
            public IEnumerator ExecuteActionMultipleTimes(Action action, int executionCount, Stack stack = null)
            {
                if (stack == null)
                {
                    //LoggerWrapper.LogInfo($"Stack变为null，停止碾压");
                    yield break;
                }
                for (int i = 0; i < executionCount; i++)
                {
                    action.Invoke();
                    //yield return new WaitForSeconds(1f);
                    yield return null;
                }
            }

        }

        [HarmonyPatch(typeof(Stack), "Smash")]
        [HarmonyPrefix]
        public static bool SmashPrefix(Stack __instance)
        {
            if (!CustomSmashLogic.UseCustomLogic)
            {
                // 如果不使用自定义逻辑，允许原始方法执行
                return true;
            }
            //LoggerWrapper.LogInfo($"自定义的Smash  {__instance.name}  {__instance.itemsFromThisStack.Count}");
            if (__instance == null)
            {
                return false;
            }

            bool flag = false;

            foreach (StackItem stackItem in __instance.itemsFromThisStack.ToList<StackItem>())
            {
                IngredientFromStack ingredientFromStack = stackItem as IngredientFromStack;
                if (ingredientFromStack != null)
                {
                    CustomSmashLogic.CustomIngredientFromStackSmash(ingredientFromStack);
                }
                else if (stackItem is GrindedSubstanceInPlay)
                {
                    flag = true;
                }
            }

            // 保留原始方法的其余部分
            if (__instance.vacuumingTo == Managers.Ingredient.cauldron)
            {
                return false;
            }
            Ingredient ingredient = CommonUtils.GetPropertyValueS<InventoryItem>(__instance, "InventoryItem") as Ingredient;
            if (!flag && ingredient.effectCollision != null)
            {
                //屏蔽原版的成分中被研磨而爆出的粒子效果减少卡顿。
                //__instance.visualEffectsScript.SpawnEffectsExplosion(ingredient.effectCollision, SpriteSortingLayers.BackgroundEffects);
                return false;
            }
            PhysicalParticle.Behaviour behaviour = (__instance.state.Get() == StateMachine.State.InPlayZone) ? PhysicalParticle.Behaviour.PlayZone : PhysicalParticle.Behaviour.Mortar;
            //屏蔽原版的成分中的叶子被研磨而爆出的粒子效果减少卡顿。
            //PhysicalParticle.SpawnExplosion(__instance.transform.position, ingredient.physicalParticleType, behaviour, ingredient.grindedSubstanceColor);

            return false;
        }

        public static class CustomSmashLogic
        {
            public static bool UseCustomLogic { get; private set; }
            private static int activeCustomLogicCount = 0; //协程实例计数器 当其归0时才能确定所有协程实例跑完，关闭自定义方法

            public static void BeginCustomLogic()
            {
                if (Interlocked.Increment(ref activeCustomLogicCount) == 1)
                {
                    UseCustomLogic = true;
                }
            }

            public static void EndCustomLogic()
            {
                if (Interlocked.Decrement(ref activeCustomLogicCount) == 0)
                {
                    UseCustomLogic = false;
                }
            }

            public static void CustomIngredientFromStackSmash(IngredientFromStack ingredientFromStack)
            {
                // 在这里实现您的自定义 Smash 逻辑
                //LoggerWrapper.LogInfo($"自定义 Smash 方法被调用：{ingredientFromStack.name}");

                Ingredient inventoryItem = (Ingredient)CommonUtils.GetPropertyValueS<InventoryItem>(ingredientFromStack.stackScript, "InventoryItem");

                if (!(inventoryItem.canBeDamaged))
                {
                    return; // 不执行原始方法
                }
                // 直接调用 TryToGrind，不检查 currentGrindState
                ingredientFromStack.TryToGrind();
                ingredientFromStack.stackScript.UpdateGrindedSubstance();
                //LoggerWrapper.LogInfo($"完成自定义 Smash 方法被调用：{ingredientFromStack.name}");
            }

            // Token: 0x06001B94 RID: 7060 RVA: 0x000B4D0C File Offset: 0x000B2F0C
            public static void SubstanceGrindingTryToGrind(SubstanceGrinding substanceGrinding)
            {

                substanceGrinding.lastGrindTime = Time.time;
                substanceGrinding.grindTicksPerformed = substanceGrinding.GrindTicksToFullGrind;
                substanceGrinding.CurrentGrindStatus = 1f;
            }

        }

        #endregion 一键研磨


























        #region 无限制种植


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
            if (enableUnrestrictedPlanting.Value)
            {
                GameObjectHelper.GetGardenRoomSeedColliders();
                GameObjectHelper.SetBuildZoneColliders(!currentBuildMode);
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
        static void IsTemporaryEnabledSetterPostfix(BuildModeManager __instance, bool value) { LogBuildModeStatus(__instance); }
        static void IsContinuousEnabledSetterPostfix(BuildModeManager __instance, bool value) { LogBuildModeStatus(__instance); }
        static void LogBuildModeStatus(BuildModeManager instance)
        {
            if (instance.IsBuildModeEnabled != lastBuildMode)
            {
                lastBuildMode = instance.IsBuildModeEnabled;
                onSwitchBuildMode(instance.IsBuildModeEnabled);
            }
        }
        static void onSwitchBuildMode(bool isBuildModeEnable)
        {
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