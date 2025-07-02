using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
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
using PotionCraft.ObjectBased.RecipeMap.RecipeMapItem.VortexMapItem;
using PotionCraft.ObjectBased.Cauldron;
using PotionCraft.ManagersSystem.RecipeMap;
using PotionCraft.DialogueSystem.Dialogue.Data;
using PotionCraft.DialogueSystem.Dialogue;
using PotionCraft.ObjectBased.UIElements.Dialogue;
using Key = UnityEngine.InputSystem.Key;
using System.Collections.Generic;
using PotionCraft.Npc.MonoBehaviourScripts;
using PotionCraft.ScriptableObjects.Potion;
using PotionCraft.LocalizationSystem;
using Ukersn_s_TweakWizard.Uilities;
using PotionCraft.ObjectBased.ScalesSystem;
using PotionCraft.QuestSystem;
using DarkScreenSystem;
using PotionCraft.ObjectBased.Potion;
using PotionCraft.ManagersSystem.Trade;
using PotionCraft.Settings;
using PotionCraft.ManagersSystem.Potion.Entities;
using PotionCraft.Utils;
using PotionCraft.Settings.GameDifficultySettings;


namespace Ukersn_s_TweakWizard
{
    [BepInPlugin("com.ukersn.plugin.TweakWizard", "Ukersn's TweakWizard", "1.3.2")]
    public class TweakWizard : BaseUnityPlugin
    {
        private static Harmony harmony = new Harmony("com.ukersn.plugin.TweakWizard");
        private static bool lastBuildMode = false;
        private static bool currentBuildMode = false;
        private static bool isFirstLoaded = false;
        private static ConfigEntry<bool> enableUnrestrictedPlanting;
        private static ConfigEntry<bool> enableOneClickGrinding;
        //private static ConfigEntry<bool> disableParticleEffects;
        //private static ConfigEntry<bool> disableScratchesEffects;

        private static ConfigEntry<bool> enablePotionEdgeSnapping;
        private static ConfigEntry<Key> potionEdgeSnappingHotkey;

        private static ConfigEntry<bool> enableQuickPotionSelection;
        private static ConfigEntry<bool> enableSingleEffectPotionSelection;



        void Awake()
        {

            Logger.LogInfo("uk多功能插件正在加载..");
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
            //// 配置项3：关闭游戏粒子效果(在2.0.2版本中官方自己出了这个功能覆盖了我的mod功能，所以删除此功能)
            //disableParticleEffects = config.Bind("Performance",
            //                                          "DisableParticleEffects",
            //                                          true,
            //                                          "Disable particle effects in the game\n" +
            //                                          "Turn this on to disable as many shiny effects as possible (such as crystals, decorations, etc.)\n" +
            //                                          "This can improve frame rate by about 10%\n" +
            //                                          "开启后，将尽可能关闭游戏中那些金光闪闪的效果（比如水晶，装饰品等）\n" +
            //                                          "这可以提高约10%的帧数");

            //// 配置项4：关闭药水和NPC身上的磨损效果
            //disableScratchesEffects = config.Bind("Performance",
            //                                      "DisableScratchesEffects",
            //                                      false,
            //                                      "Disable scratches effects on potions and NPCs\n" +
            //                                      "Turn this on to disable Scratches effects on potions and NPCs as much as possible\n" +
            //                                      "It might look a bit strange, but it can improve frame rate by about 3%!\n" +
            //                                      "开启后，将尽可能关闭游戏中在药水和NPC身上的磨损效果\n" +
            //                                      "看起来可能有点怪怪的，但它可以提高约3%的帧数！");


            // 新增：启用药水贴边功能的配置
            enablePotionEdgeSnapping = Config.Bind("General",
                                                   "EnablePotionEdgeSnapping",
                                                   false,
                                                   "Enable potion edge snapping feature\n" +
                                                   "This feature is controlled by holding down the assigned key\n" +
                                                   "When active, potions will automatically snap to the edge of the vortex when close\n" +
                                                   "开启此功能后，按住指定按键时药水靠近旋涡边缘会自动贴边\n" +
                                                   "这可以使药水定位更容易和精确");

            // 新增：药水贴边功能的按键配置
            potionEdgeSnappingHotkey = Config.Bind("Hotkeys",
                                                "PotionEdgeSnappingKey",
                                                Key.Q,
                                                "Key for potion edge snapping feature\n" +
                                                "Hold this key to activate the potion edge snapping feature\n" +
                                                "Release to deactivate\n" +
                                                "Default is Q key\n" +
                                                "按住此键来激活药水贴边功能\n" +
                                                "松开键则停用功能\n" +
                                                "默认为Q键");


            // 配置项：快速选药
            enableQuickPotionSelection = Config.Bind("General",
                                                     "EnableQuickPotionSelection",
                                                     true,
                                                     "Enable quick potion selection\n" +
                                                     "When enabled, a new button will appear in the regular customer and trader interface\n" +
                                                     "This button allows you to quickly select a suitable potion from your inventory and place it on the scale\n" +
                                                     "It also displays the theoretical value of the most suitable potion for this customer (may not be 100% accurate in some cases)\n" +
                                                     "开启此功能后，在普通客人和交易者界面会有一个新的按钮，用于一键从背包中拿出符合要求的药水，到天平上\n" +
                                                     "此外点击按钮后还能显示理论上这个客人需要的最符合条件的药水的价值(某些情况下不是非常准确)");

            // 配置项：快速选药切换为单类型药水
            enableSingleEffectPotionSelection = Config.Bind("General",
                                                            "EnableSingleEffectPotionSelection",
                                                            false,
                                                            "Enable single effect potion selection for quick potion feature\n" +
                                                            "When enabled, the quick potion selection feature will only choose the most valuable single-effect potion for the customer\n" +
                                                            "Potions with multiple effects will not be considered\n" +
                                                            "开启此功能后，快速选药的功能只会为客人挑出最有价值的单一效果的药水(复合效果的药水不计入在内)");


            //用于无限制种植
            if (enableUnrestrictedPlanting.Value)
            {
                ApplyBuildModePatch();
                harmony.Patch(
                    AccessTools.Method(typeof(BuildableItemFromInventory), "UpdateActualColliderInCurrentBuildZoneSet"),
                    postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateActualColliderInCurrentBuildZoneSetPostfix)));
            }

            if (enablePotionEdgeSnapping.Value) {

                harmony.Patch(
                    AccessTools.Method(typeof(Cauldron), "UpdateStirringValue"),
                    prefix: new HarmonyMethod(typeof(TweakWizard), nameof(StirDisablePrefix))
                );

                // RecipeMapManager.GetSpeedOfMovingTowardsBase 补丁
                harmony.Patch(
                    AccessTools.Method(typeof(RecipeMapManager), "GetSpeedOfMovingTowardsBase"),
                    prefix: new HarmonyMethod(typeof(TweakWizard), nameof(LadleDisablePrefix))
                );
                harmony.Patch(
                    AccessTools.Method(typeof(VortexMapItemCollider), "OnTriggerStay2D"),
                    postfix: new HarmonyMethod(typeof(TweakWizard), nameof(OnTriggerStay2DPostfix))
                );

                harmony.Patch(
                    AccessTools.Method(typeof(VortexMapItemCollider), "OnTriggerEnter2D"),
                    prefix: new HarmonyMethod(typeof(TweakWizard), nameof(OnTriggerEnter2DPrefix))
                );

            }
            // Cauldron.UpdateStirringValue 补丁
            if (enableQuickPotionSelection.Value) {
                //添加自动寻药按钮
               harmony.Patch(
               AccessTools.Method(typeof(DialogueBox), "SpawnPotionRequestInterface"),
               postfix: new HarmonyMethod(typeof(TweakWizard), nameof(SpawnPotionRequestInterfacePostfix)));

               harmony.Patch(
               AccessTools.Method(typeof(DialogueBox), "SpawnClosenessPotionRequestInterface"),
               postfix: new HarmonyMethod(typeof(TweakWizard), nameof(SpawnClosenessPotionRequestInterfacePostfix)));
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
            if (enablePotionEdgeSnapping.Value)
            {
                if (lastEdgeDistance != 0f && !Keyboard.current[potionEdgeSnappingHotkey.Value].isPressed) lastEdgeDistance = 0f;
                if (stopMovingPotion && !Keyboard.current[potionEdgeSnappingHotkey.Value].isPressed) stopMovingPotion = false;
            }


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

                }


                //用于帧数优化
                //if (disableParticleEffects.Value)
                //{
                //    harmony.Patch(AccessTools.Method(typeof(GrowthHandler), "UpdateSprite"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateSpritePostfix)));
                //}
                //harmony.Patch(AccessTools.Method(typeof(GrowingSpotController), "OnBuildableItemFromInventoryInit"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(OnBuildableItemFromInventoryInitPostfix)));
                //harmony.Patch(AccessTools.Method(typeof(BuildableItemFromInventoryVisualObjectController), "UpdateVisualState"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateVisualStatePostfix)));

            }
            //用于帧数优化
            //if (disableParticleEffects.Value || disableScratchesEffects.Value) {
            //    GameObjectHelper.StopAllParticleSystems(disableParticleEffects.Value,disableScratchesEffects.Value);
            //}


            //GameObjectHelper.SetAdjacentRoomsActiveAndOthersInactive(RoomIndex.Laboratory);
        }








        #region 快速选药

        public static DialogueButton findPotionButton=null;

        [HarmonyPatch(typeof(DialogueBox), "SpawnPotionRequestInterface")]
        [HarmonyPostfix]
        static void SpawnPotionRequestInterfacePostfix(DialogueBox __instance, PotionRequestNodeData potionRequestNodeData, DialogueData dialogueData)
        {//在创建普通npc对话框的按钮后追加一个新按钮

            //如果在教程中，不创建这个按钮
            if (Managers.Tutorial.IsTutorialActive()) return;

            // 获取现有按钮数量
            int currentButtonCount = __instance.dialogueButtons.Length;

            //// 创建新的按钮数组
            DialogueButton[] newButtons = new DialogueButton[currentButtonCount + 1];

            // 计算新按钮的位置（使用倒数第二个按钮的位置）
            Vector2 position = __instance.dialogueButtons[currentButtonCount - 2].transform.localPosition;

            // 创建新按钮
            DialogueButton newButton = DialogueBox.dialogueButtonSpawner.Spawn(__instance.dialogueButtons[currentButtonCount - 1].SubPoolIndex, position);//这里使用之前最后一个按钮(返回或结束按钮)的预制体(也就是会影响按钮外观)
            var autoFindPotionKey = new PotionCraft.LocalizationSystem.Key("#mod_ukersn_s_tweakwizard_quick_potion_pick", null, PotionCraft.LocalizationSystem.KeyParametersStyle.Normal, null, null);
            // 设置按钮内容
            newButton.SetContent(
                autoFindPotionKey,
                null,
                __instance.givePotionIcon,
                () => { AutoFindPotion(); },  // 这里调用你的自定义方法
                currentButtonCount,//这里使用之前最后一个按钮(返回或结束按钮)的索引
                null,
                null,
                1f,
                null
            );
            //使用下面这种方式重新设定按钮的文本 (不需要了，通过RegisterLoc("mod_ukersn_s_tweakwizard_auto_find_potion")解决了,注册的时候不要携带#字符串开头，是错的)
            //newButton.leftText.Text.text = String.Format(PotionCraft.LocalizationSystem.LocalizationManager.GetText("#mod_ukersn_s_tweakwizard_auto_find_potion"),"Value");

            newButton.SetAlpha(0f);
            Array.Copy(__instance.dialogueButtons, newButtons, currentButtonCount);

            //移动按钮到倒数第二个的位置。
            newButtons[currentButtonCount - 1] = newButton;
            newButtons[currentButtonCount] = __instance.dialogueButtons[currentButtonCount - 1];

            // 更新DialogueBox的按钮数组
            __instance.dialogueButtons = newButtons;

            // 更新所有按钮的位置
            for (int i = currentButtonCount - 1; i < newButtons.Length; i++)
            {
                Vector2 newPosition = newButtons[i].transform.localPosition;
                newPosition.y -= newButton.thisCollider.bounds.size.y + __instance.spaceAnswers;
                newButtons[i].transform.localPosition = newPosition;
            }
            // 更新对话框高度
            __instance.dialogueText.minTextBoxY += newButton.thisCollider.bounds.size.y + __instance.spaceAnswers;


            findPotionButton = newButton;

            UpdateFindPotionButtonText(Mathf.FloorToInt(GetMaxPriceForItem(Managers.Npc.CurrentNpcMonoBehaviour)));
            //LoggerWrapper.LogInfo("创建了按钮");
            //if (Managers.Npc.CurrentNpcMonoBehaviour.trading.isTrader == true)
            //{
            //    LoggerWrapper.LogInfo("创建了商人按钮");

            //}
            //else {
            //    LoggerWrapper.LogInfo("创建了普通按钮");
            //}
        }



        [HarmonyPatch(typeof(DialogueBox), "SpawnClosenessPotionRequestInterface")]
        [HarmonyPostfix]
        static void SpawnClosenessPotionRequestInterfacePostfix(DialogueBox __instance)
        {//在创建商人npc对话框的按钮后追加一个新按钮
            SpawnPotionRequestInterfacePostfix(__instance, null, null);
            if (findPotionButton == null) return;//不显示最高报价
            findPotionButton.localizedText.SetText("#mod_ukersn_s_tweakwizard_quick_potion_pick", null, KeyParametersStyle.Normal);

            //LoggerWrapper.LogInfo("商人npc在药水需求创建了按钮");
        }





        static void AutoFindPotion()
        {
            // 在这里实现自动寻找药水的逻辑
            //LoggerWrapper.LogInfo("自动寻找药水(快速选药)");
            //当前npc
            NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;
            //当前玩家背包
            Inventory inventory = GameObjectHelper.GetPlayInventory();
            //NPC药水需求任务期望的效果列表
            List<PotionEffect> list2 = currentNpcMonoBehaviour.currentQuest.desiredEffects.ToList<PotionEffect>();
            //判断是否能使用这个功能
            if (currentNpcMonoBehaviour == null || currentNpcMonoBehaviour.currentQuest == null)
            {
                Tooltis.SpawnMessageText("Error:找不到当前npc或当前npc的需求任务");
                return;
            }
            DialogueState state = Managers.Dialogue.State;
            if (state != DialogueState.PotionRequest && state != DialogueState.ClosenessPotionRequest)
            {
                Tooltis.SpawnMessageText("Error:当前对话框状态不是药水需求任务状态");
                return;
            }
            if (Scales.Instance.CurrentState != ScalesState.PotionRequest)
            {
                Tooltis.SpawnMessageText("Error:当前天平状态不处于药水需求任务状态");
                return;
            }


            ////根据价格对效果排序
            //list2.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));
            ////挑出价值最高的效果
            //PotionEffect potionEffect = list2[0];
            ////用于确认目前背包里拥有的最适合的效果
            //int nowPotionEffectIndex = 0;
            //PotionEffect nowPotionEffect = list2[nowPotionEffectIndex];


            InventoryItemIntDictionary items = inventory.items;
            List<KeyValuePair<InventoryItem, float>> potionList = new List<KeyValuePair<InventoryItem, float>>();
            // 遍历 ItemContainer 的所有子对象
            foreach (KeyValuePair<InventoryItem, int> item in items)
            {
                //item.Key 物体类型
                //item.Value 物体数量
                if (item.Key is Potion potion)
                {
                    PotionReview potionReview = Potion.GetPotionReview(potion.Effects, currentNpcMonoBehaviour.currentQuest.desiredEffects);
                    bool flag = potionReview.maxTier != 0;
                    if (flag)
                    {
                        flag = GeneratedQuestRequirement.AreRequirementsCompleted(potion, currentNpcMonoBehaviour.currentQuest, currentNpcMonoBehaviour.mandatoryQuestRequirements);
                        
                    }
                    if (flag) {
                        //float price = Managers.Trade.GetCurrentPriceForItem(item.Key, Inventory.Owner.Player,1, currentNpcMonoBehaviour);
                        //potionList.Add(new KeyValuePair<InventoryItem, float>(item.Key, price));
                        if (enableSingleEffectPotionSelection.Value) {
                            bool isSingleEffect = IsSingleEffect(potion.Effects);
                            if (isSingleEffect)
                                potionList.Add(new KeyValuePair<InventoryItem, float>(item.Key, potionReview.cost));
                        }else potionList.Add(new KeyValuePair<InventoryItem, float>(item.Key, potionReview.cost));
                        //LoggerWrapper.LogInfo($"符合条件的药水：{item.Key.name} 价值：{potionReview.cost} {IsSingleEffect(potion.Effects)}");
                    }
                }

            }
            if (potionList.Count > 0)
            {

                Potion suitablePotion = GetSuitablePotion(potionList);
                TryToPutPotionOnScales(inventory, suitablePotion);//将适合的药水拿出来放到天平上
            }
            else {
                Tooltis.SpawnMessageText(LocalizationManager.GetText("mod_ukersn_s_tweakwizard_no_suitable_potion"));
            }
            //Potion bestSuitablePotion = CreateSuitablePotion();
            //int bestSuitablePotionPrice = Mathf.FloorToInt(Managers.Trade.GetCurrentPriceForItem(bestSuitablePotion, Inventory.Owner.Player, 1, currentNpcMonoBehaviour));
            //LoggerWrapper.LogInfo($"最符合条件的药水：{bestSuitablePotion.name} 目前价值：{bestSuitablePotionPrice} 计算价值：{Mathf.RoundToInt(GetMaxPriceForItem(currentNpcMonoBehaviour))}");


        }


        public static float GetMaxPriceForItem(NpcMonoBehaviour npc = null)
        {//获取理论上药水最大在任务中的最大价值 从PotionCraft.ManagersSystem.Trade.TradeManager.GetCurrentPriceForItem(InventoryItem, Inventory.Owner, int, NpcMonoBehaviour)改写
            int count = 1;
            TradeManager tradeManager = Managers.Trade;
            //Potion potion = item as Potion;

            Quest quest = npc.currentQuest;
            //Potion potion = CreateSuitablePotion();
            PotionEffect[] desiredEffects = quest.desiredEffects;
            
            float price = GetMaxMatchingPotionPrice(desiredEffects);                                                                     //药水效果与期望效果符合程度的基础价格
            float num  = GetMaxRequirementsCostMultiplier(npc.mandatoryQuestRequirements);                                               //强制性任务需求的价格乘数
            float num2 = GetMaxRequirementsCostMultiplier(npc.optionalQuestRequirements);                                                //可选任务需求的价格乘数
            float num3 = (Managers.Player.popularity.Tier.Settings.potionCostMultiplier - 1f);                                           //玩家声望系数
            float num4 =  1f;                                                                                                            //原本是(交易时候的)药水成本系数
            float num5 =(tradeManager.SoldPotionYesterdayCostMultiplier * tradeManager.CountOfSoldPotionsYesterday);                     //(天赋技能)基于昨日售出药水数量的价格加成
            float num6 =  1f;                                                                                                            //原本是(交易时候的)商人药水价格乘数
            float num7 = tradeManager.TalentTradePotionPrice.GetCurrentBonus();                                                          //(天赋技能)药水交易价格天赋加成
            float num8 = tradeManager.TalentTradePotionPriceEndless.GetCurrentBonus();                                                   //(天赋技能)无尽模式药水交易价格天赋加成
            float num9 = GameDifficultyModifierSimple<GameDifficultyPotionPriceMultiplier, float>.Instance.GetCurrentValue();            //基于游戏难度的药水价格乘数
            float num10 = price * (1f + num3 + num7 + num8 + num5) * num6 * num4 * num * num2 * num9;                                    //中间结果-药水综合价格




            float currentValue = GameDifficultyModifierSimple<GameDifficultyTradersGreedMultiplier, float>.Instance.GetCurrentValue();
            float num11 = Managers.Trade.TraderGreedCoefficient / currentValue;                                                          //交易者贪婪系数调整值
            float num12 =(1f / num11);                                                                                                   //根据交易方向（买/卖）调整的贪婪系数
            float num13 = num10 * num12 * count;                                                                                         //最终结果

            //LoggerWrapper.LogInfo($"0：{price} 1: {num} 2：{num2} 3: {num3} 4：{num4} 5: {num5} 6：{num6} 7: {num7} 8：{num8} 9: {num9} 10：{num10} 11: {num11} 12：{num12} 13：{num13} ");

            //LoggerWrapper.LogInfo($"药水价格：{price} 理论实际价格: {num13}  主要任务系数：{npc.mandatoryQuestRequirements.Count} => {num} 次要任务系数：{npc.optionalQuestRequirements.Count} => {num2}");

            return num13;
        }



        public static float GetMaxRequirementsCostMultiplier( List<GeneratedQuestRequirement> requirements)
        {
            if (requirements.Count == 0)
            {
                return 1f;
            }
            float CostMultiplier = 1f;

            //bool isStrongPotion = false;
            //foreach (var requirement in requirements)
            //{
            //    if (requirement.requirementInQuest.requirement is QuestRequirementPotionQuality && (requirement.requirementInQuest.requirement as QuestRequirementPotionQuality).targetTier == 3) {
            //        isStrongPotion = true;
            //        break;
            //    }
            //}


                // 检查需求
            foreach (var requirement in requirements)
            {
                switch (requirement.requirementInQuest.requirement)
                {
                    case QuestRequirementOnlyParticularBase _: //只要某基底
                        CostMultiplier *= 2f;
                        break;
                    case QuestRequirementNoSalts _: //不能加盐
                        CostMultiplier *= 1.5f;
                        break;

                    case QuestRequirementMaxIngredients req: //以多少种成分来制作药水
                        switch (req.maxIngredientsAmount) {
                            case 3:
                                CostMultiplier *= 1.5f;
                                break;
                            case 2:
                                CostMultiplier *= 2f;
                                break;
                            case 1:
                                CostMultiplier *= 3f;
                                break;
                        }
                        break;
                    case QuestRequirementPotionQuality req: //对药剂质量有要求
                        switch (req.targetTier)
                        {
                            case 3:
                                CostMultiplier *= 1.5f;
                                break;
                            case 1:
                                CostMultiplier *= 3f;
                                break;
                        }
                        break;

                    case QuestRequirementNeedOneParticularIngredient _: //至少有某一种成分
                        CostMultiplier *= 1.5f;
                        break;
                    case QuestRequirementMainIngredient _: //主要以某种成分
                        CostMultiplier *= 2;
                        break;
                    case QuestRequirementsAdditionalEffects _: //额外药效
                         //1.25 1.5 2.5 5 决定额外药效的乘数
                        //在外面单独处理。
                        break;
                    case QuestRequirementNoParticularIngredient _: //不要某种成分
                        CostMultiplier *= 1.5f;
                        break;
                    case QuestRequirementNoParticularBase _: //不要某种基底
                        CostMultiplier *= 1.5f;
                        break;
                }

            }

            return CostMultiplier;

        }

        public static float GetMaxMatchingPotionPrice(PotionEffect[] desiredEffects)
        { //不完全准确 因为 A > 1.1B的公式，在3+1+1 与3+2的效果搭配可能存在这么个临界点价值是不一样的

            // 如果是特殊NPC，直接返回固定价格
            if (Managers.Debug.IsCurrentNpcAlexNichiporchik())return 4000;
            // 如果没有期望效果,那还算个屁
            if (desiredEffects == null || desiredEffects.Length == 0)return 0;

            //构造效果

            //所有任务需求列表
            NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;
            List<GeneratedQuestRequirement> list = new List<GeneratedQuestRequirement>();
            list.AddRange(currentNpcMonoBehaviour.mandatoryQuestRequirements);
            list.AddRange(currentNpcMonoBehaviour.optionalQuestRequirements);


            //有无药水质量需求和额外效果需求。
            bool hasQualityRequirement = false;
            bool hasAdditionalEffectsRequirement = false;
            int qualityRequirement = 0;

            // 检查需求
            foreach (var requirement in list)
            {
                if (requirement.requirementInQuest.requirement is QuestRequirementPotionQuality)
                {
                    hasQualityRequirement = (requirement.requirementInQuest.requirement as QuestRequirementPotionQuality).targetTier == 3;
                    qualityRequirement = (requirement.requirementInQuest.requirement as QuestRequirementPotionQuality).targetTier;
                }
                else if (requirement.requirementInQuest.requirement is QuestRequirementsAdditionalEffects)
                {
                    hasAdditionalEffectsRequirement = true;
                }
            }

            List<float> additionEffectPriceMultiplier =new List<float> { 1f, 1.25f, 1.5f, 2.5f, 5f };
            List<float> qualityMultiplier = new List<float> { 0.4f,0.7f,1f};


            int maxEffects = 5;//药水效果容器上限
            float totalPrice = 0f; //最终价格
            int usedEffectSlots = 0;//目前使用掉的容器数量
            int effectTypes = 0;//药水效果的种类

            //根据价格对效果排序
            List<PotionEffect> desiredEffectList = desiredEffects.ToList<PotionEffect>(); //期望效果(排序后的)
            desiredEffectList.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));

            if (hasAdditionalEffectsRequirement)
            {
                if (qualityRequirement ==3) // 需要强效果
                {
                    // 主效果使用强效果
                    totalPrice += desiredEffectList[0].price * qualityMultiplier[2];
                    usedEffectSlots = 3;
                    effectTypes = 1;

                    List<PotionEffect> compatibleEffects = FilterCompatibleEffects(desiredEffectList);
                    for (int i = 1; i < compatibleEffects.Count && usedEffectSlots < maxEffects; i++)
                    {
                        totalPrice += desiredEffectList[i].price * qualityMultiplier[0];
                        usedEffectSlots++;
                        effectTypes++;
                    }


                    //这里应该有desiredEffects不满2个效果，去lastAvailableEffects补充的效果，不过最终不参与设计计算只提升倍率，直接设定usedEffectSlots=5就好。
                    effectTypes += maxEffects - usedEffectSlots;
                    usedEffectSlots = 5;
                    //LoggerWrapper.LogInfo($"允许额外效果-无效果或强效果 主要药水效果价格：{desiredEffectList[0].price} 效果数量：{effectTypes} 总体药水价格:{totalPrice}");
                }
                else // 不需要强效果，使用5个弱效果
                {

                    effectTypes = 0;

                    List<PotionEffect> compatibleEffects = FilterCompatibleEffects(desiredEffectList);
                    for (int i = 0; i < compatibleEffects.Count && usedEffectSlots < maxEffects; i++)
                    {
                        totalPrice += desiredEffectList[i].price * qualityMultiplier[0];
                        usedEffectSlots++;
                        effectTypes++;
                    }



                    //这里应该有desiredEffects不满5个效果，去lastAvailableEffects补充的效果，不过最终不参与设计计算只提升倍率，直接设定usedEffectSlots=5就好。
                    effectTypes += maxEffects - usedEffectSlots;
                    usedEffectSlots = 5;
                    //LoggerWrapper.LogInfo($"允许额外效果-弱效果 主要药水效果价格：{desiredEffectList[0].price} 效果数量：{effectTypes} 总体药水价格:{totalPrice}");
                }
            }
            else // 不需要额外效果，但仍可以添加多个期望效果
            {
                if (qualityRequirement == 3 || qualityRequirement == 0)  // 需要强效果
                {
                    // 主效果使用强效果
                    totalPrice += desiredEffectList[0].price * qualityMultiplier[2];
                    usedEffectSlots = 3;
                    effectTypes=1;


                    List<PotionEffect> compatibleEffects = FilterCompatibleEffects(desiredEffectList);


                    for (int i = 1; i < compatibleEffects.Count && usedEffectSlots < maxEffects; i++)
                    {
                        int num = (i ==compatibleEffects.Count-1 && usedEffectSlots<5) ? 1 : 0;
                        totalPrice += desiredEffectList[i].price * qualityMultiplier[0 + num];
                        usedEffectSlots = usedEffectSlots + 1 + num;
                        effectTypes++;
                    }

                    //LoggerWrapper.LogInfo($"无额外效果-无效果或强效果 主要药水效果价格：{desiredEffectList[0].price} 效果数量：{effectTypes} 总体药水价格:{totalPrice}");
                }
                else // 使用弱效果
                {
                    effectTypes=0;


                    List<PotionEffect> compatibleEffects = FilterCompatibleEffects(desiredEffectList);


                    for (int i = 0; i < compatibleEffects.Count && usedEffectSlots < maxEffects; i++)
                    {
                        totalPrice += desiredEffectList[i].price * qualityMultiplier[0];
                        usedEffectSlots++;
                        effectTypes++;
                    }



                    //LoggerWrapper.LogInfo($"无额外效果-弱效果 主要药水效果价格：{desiredEffectList[0].price} 效果数量：{effectTypes} 总体药水价格:{totalPrice}");
                }
            }
             //LoggerWrapper.LogInfo($"算前药水价格:{totalPrice} 额外效果价格系数：{additionEffectPriceMultiplier[effectTypes - 1]} ");
            // 应用额外效果价格乘数（基于实际使用的效果数量）
            totalPrice *= hasAdditionalEffectsRequirement?additionEffectPriceMultiplier[effectTypes - 1]:1;

            return totalPrice;

        }


        public static List<PotionEffect> FilterCompatibleEffects(List<PotionEffect> sortedDesiredEffects)
        {
            if (sortedDesiredEffects == null || sortedDesiredEffects.Count == 0)
                return new List<PotionEffect>();

            List<PotionEffect> compatibleEffects = new List<PotionEffect> { sortedDesiredEffects[0] };

            for (int i = 1; i < sortedDesiredEffects.Count; i++)
            {
                bool isCompatible = true;
                for (int j = 0; j < compatibleEffects.Count; j++)
                {
                    if (!PotionEffect.IsEffectsCompatible(sortedDesiredEffects[i], compatibleEffects[j]))
                    {
                        isCompatible = false;
                        break;
                    }
                }

                if (isCompatible)
                {
                    compatibleEffects.Add(sortedDesiredEffects[i]);
                }
            }

            //foreach (var effect in compatibleEffects) {
                //LoggerWrapper.LogInfo($"兼容池中的效果:{effect.name}");
            //}
            compatibleEffects.Sort((a, b) => b.price.CompareTo(a.price));
            return compatibleEffects;
        }

        public static bool IsSingleEffect(PotionEffect[] potionEffects)
        {
            bool flag = true;
            string mainName = potionEffects[0].name;
            foreach (PotionEffect effect in potionEffects) {
                if (effect.name != mainName) {
                    flag = false;
                    break;
                }

            }
            return flag;
        }


        public static Potion GetSuitablePotion(List<KeyValuePair<InventoryItem, float>> potionList)
        {//在符合价值的药水上，挑选最适合的药水部分最为最终提供的药水
            potionList.Sort((a, b) => b.Value.CompareTo(a.Value));
            int suitablePotionIndex;
            if (Managers.Npc.CurrentNpcMonoBehaviour.trading.isTrader == true)
            {//如果是商人npc，则给出最便宜的药水
                suitablePotionIndex = potionList.Count-1;
                float lastValue = (potionList[suitablePotionIndex].Key as Potion).GetPrice();

                for (int i = suitablePotionIndex -1 ; i > 0; i--)
                {
                    Potion potion = potionList[i].Key as Potion;
                    float price = potion.GetPrice();
                    //LoggerWrapper.LogInfo($"{potionList[i].Value} {price} {lastValue}");
                    if (price < lastValue)
                    {
                        suitablePotionIndex = i;
                        lastValue = price;
                    }
                }

            }else
            {//如果是普通npc，则计算目前最高报价下最适合(药水本身价值最便宜的)的药水
                float maxValue = potionList[0].Value;
                float lastValue = (potionList[0].Key as Potion).GetPrice();
                suitablePotionIndex = 0;
                for (int i = 1; i < potionList.Count; i++)
                {
                    //LoggerWrapper.LogInfo($"{potionList[i].Value} {price} {lastValue}");
                    if (potionList[i].Value == maxValue)
                    {
                        Potion potion = potionList[i].Key as Potion;
                        float price = potion.GetPrice();
                        if (IsSingleEffect(potion.effects))
                        {
                            suitablePotionIndex = i;
                            break;
                        }
                        else if (price < lastValue) { 
                            suitablePotionIndex = i; 
                            lastValue = price;
                        }
                    }
                    else break;
                }

            }


 
            return potionList[suitablePotionIndex].Key as Potion;
        }




        public static void UpdateFindPotionButtonText(int bestSuitablePotionPrice) {//更新自动选药按钮文本
            if (findPotionButton == null) return;
            findPotionButton.localizedText.SetText("#mod_ukersn_s_tweakwizard_quick_potion_pick_theoretical_max_value", new List<string>
                    {
                        bestSuitablePotionPrice.ToString(),
                    }, KeyParametersStyle.Normal);
        }

        public static bool TryToPutPotionOnScales(Inventory inventory ,Potion potion)
        {//将背包中的药水放到天平 PotionCraft.ObjectBased.InteractiveItem.InventoryObject.InventoryObject.TryFastPutPotionOnScales()的改写
            DarkScreen.DeactivateAll(DarkScreenDeactivationType.Other, DarkScreenLayer.Lower, true, null);
            if (potion == null || !Managers.Room.IsCurrentOrTargetRoom(RoomIndex.Meeting) || (Managers.Dialogue.State != DialogueState.PotionRequest && Managers.Dialogue.State != DialogueState.ClosenessPotionRequest))
            {
                return false;
            }
            Vector3 zero = Vector3.zero;
            IItemContainer display = Scales.Instance.rightCupScript.display;
            ItemFromInventory itemFromInventory;
            (itemFromInventory = PotionItem.SpawnNewPotion(zero, potion, inventory.itemsPanel)).SoundController.OnGrab(null);
            if (display.ItemCanBeAccepted(itemFromInventory))
            {
                itemFromInventory.sortingOrderSetter.SetupRenderers();
                display.ItemReleasedOver(itemFromInventory);
                itemFromInventory.sortingOrderSetter.setupOnStart = false;
                Managers.Player.Inventory.RemoveItem(potion, 1, true);
                return true;
            }
            UnityEngine.Object.Destroy(itemFromInventory);
            return false;
        }



        public static Potion CreateSuitablePotion()
        {//创建一个虚拟的适合任务的药水 PotionCraft.ManagersSystem.Potion.PotionManager.CreateQuestPotion()的截取  但不是最大价值的药水。

            //当前npc
            NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;

            DarkScreen.DeactivateAll(DarkScreenDeactivationType.Other, DarkScreenLayer.Lower, true, null);

            //当前玩家背包
            Inventory inventory = GameObjectHelper.GetPlayInventory();
            //NPC药水需求任务期望的效果列表
            List<PotionEffect> list2 = currentNpcMonoBehaviour.currentQuest.desiredEffects.ToList<PotionEffect>();

            //所有任务需求列表
            List<GeneratedQuestRequirement> list = new List<GeneratedQuestRequirement>();
            list.AddRange(currentNpcMonoBehaviour.mandatoryQuestRequirements);
            list.AddRange(currentNpcMonoBehaviour.optionalQuestRequirements);

            //根据价格对效果排序
            list2.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));
            //挑出价值最高的效果
            PotionEffect potionEffect = list2[0];


            List<PotionEffect> list3 = new List<PotionEffect>();
            List<PotionEffect> list4 = PotionEffect.allPotionEffects.ToList<PotionEffect>(); //按价值排序的所有药水效果
            list4.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));

            int num = 0;
            bool flag = false;
            for (int l = 0; l < list.Count; l++)//遍历所有任务
            {
                GeneratedQuestRequirement generatedQuestRequirement = list[l];//当前任务
                if (generatedQuestRequirement.requirementInQuest.requirement is QuestRequirementPotionQuality)//如果这个任务是药水质量
                {
                    num = generatedQuestRequirement.intValue1;
                }
                else if (generatedQuestRequirement.requirementInQuest.requirement is QuestRequirementsAdditionalEffects)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                int num2 = 5 - Mathf.Max(num, 1);
                PotionEffect lastEffect = potionEffect;

                for (int j = 0; j < num2; j++)
                {
                    list4.Remove(lastEffect);

                    // 使用 Lambda 表达式直接定义 predicate
                    list4 = list4.Where(e => PotionEffect.IsEffectsCompatible(lastEffect, e)).ToList();

                    if (list4.Count == 0)
                    {
                        break;
                    }

                    lastEffect = list4[0];
                    list3.Add(lastEffect);
                }
            }
            List<PotionEffect> list5 = new List<PotionEffect>();
            int num3 = (num == 0) ? Mathf.Min(3, 5 - list3.Count) : num;
            for (int k = 0; k < num3; k++)
            {
                list5.Add(potionEffect);
            }
            foreach (PotionEffect item in list3)
            {
                list5.Add(item);
            }
            PotionEffect[] array = list5.ToArray();
            PotionBase potionBase = Settings<RecipeMapManagerPotionBasesSettings>.Asset.potionBases[0];
            List<Ingredient> list6 = Ingredient.allIngredients.ToList<Ingredient>();
            Dictionary<Ingredient, int> dictionary = new Dictionary<Ingredient, int>();
            int num4 = -1;
            bool allowSalts = true;
            foreach (GeneratedQuestRequirement generatedQuestRequirement2 in list)
            {
                QuestRequirement requirement = generatedQuestRequirement2.requirementInQuest.requirement;
                if (!(requirement is QuestRequirementNoParticularIngredient))
                {
                    if (!(requirement is QuestRequirementMainIngredient))
                    {
                        if (!(requirement is QuestRequirementNeedOneParticularIngredient))
                        {
                            if (!(requirement is QuestRequirementOnlyParticularBase))
                            {
                                if (!(requirement is QuestRequirementNoParticularBase))
                                {
                                    if (!(requirement is QuestRequirementMaxIngredients))
                                    {
                                        if (requirement is QuestRequirementNoSalts)
                                        {
                                            allowSalts = false;
                                        }
                                    }
                                    else
                                    {
                                        num4 = generatedQuestRequirement2.intValue1;
                                    }
                                }
                                else if (potionBase.name.Equals(generatedQuestRequirement2.stringValue1))
                                {
                                    potionBase = Settings<RecipeMapManagerPotionBasesSettings>.Asset.potionBases[1];
                                }
                            }
                            else
                            {
                                potionBase = PotionBase.GetByName(generatedQuestRequirement2.stringValue1, false, true);
                            }
                        }
                        else
                        {
                            Ingredient byName = Ingredient.GetByName(generatedQuestRequirement2.stringValue1, false, true);
                            dictionary[byName] = 1;
                        }
                    }
                    else
                    {
                        Ingredient byName2 = Ingredient.GetByName(generatedQuestRequirement2.stringValue1, false, true);
                        dictionary[byName2] = 1000;
                    }
                }
                else
                {
                    GeneratedQuestRequirement requirement1 = generatedQuestRequirement2;
                    list6 = (from i in list6
                             where !i.name.Equals(requirement1.stringValue1)
                             select i).ToList<Ingredient>();
                }
            }
            int count = dictionary.Count;
            if (num4 != -1 && count > 0)
            {
                num4 -= Mathf.Min(count, num4);
            }
            SimpleAlchemySubstanceComponents defaultUsedElements = PotionEffect.GetDefaultUsedElements(array, potionBase, list6, num4, allowSalts);
            foreach (KeyValuePair<Ingredient, int> keyValuePair in dictionary)
            {
                defaultUsedElements.Add(new AlchemySubstanceComponent(keyValuePair.Key, keyValuePair.Value), -1);
            }
            PotionEffect[] effects = array;
            ColoredIcon coloredIcon = null;
            Bottle bottle = null;
            Sticker sticker = null;
            PotionBase potionBase2 = potionBase;
            SimpleAlchemySubstanceComponents components = defaultUsedElements;
            Potion potion = PotionGenerator.GeneratePotion(effects, coloredIcon, bottle, sticker, null, potionBase2, components, "", false, "");
            
            Vector3 zero = Vector3.zero;
            IItemContainer display = Scales.Instance.rightCupScript.display;
            ItemFromInventory itemFromInventory = PotionItem.SpawnNewPotion(zero, potion, Managers.Player.Inventory.itemsPanel);
            if (display.ItemCanBeAccepted(itemFromInventory))
            {
                itemFromInventory.sortingOrderSetter.SetupRenderers();
                display.ItemReleasedOver(itemFromInventory);
                itemFromInventory.sortingOrderSetter.setupOnStart = false;
                
            }
            //UnityEngine.Object.Destroy(itemFromInventory);

            return potion;

        }


        #endregion 快速选药












        #region 旋涡贴边




        public static bool stopMovingPotion = false;
        private static float lastEdgeDistance = 0f;

        public static bool IsNearOutEdge(CircleCollider2D vortexCollider,Collider2D other, float threshold = 0.05f)
        {

            CircleCollider2D indicatorCollider = other as CircleCollider2D;
            if (indicatorCollider == null)
            {
                LoggerWrapper.LogError("药水指示器的 CircleCollider2D 未正确初始化");
                return false;
            }
            // 获取旋涡和指示器的位置
            Vector2 vortexCenter = vortexCollider.transform.parent.position;
            Vector2 indicatorPos = indicatorCollider.transform.parent.position;

            // 计算指示器到旋涡中心的距离
            float distance = Vector2.Distance(vortexCenter, indicatorPos);


            // 计算指示器边缘到旋涡边缘的距离
            float currentEdgeDistance = distance - (vortexCollider.radius + indicatorCollider.radius);

            // 判断是否接近边缘
            bool isNearEdge = Mathf.Abs(currentEdgeDistance) < threshold;
            bool isLeaving = currentEdgeDistance > lastEdgeDistance;

            //LoggerWrapper.LogInfo($"结果：{isNearEdge && isLeaving}, 中心距离：{distance}, " +
            //                      $"当前边缘距离：{currentEdgeDistance}, 上次边缘距离：{lastEdgeDistance}, " +
            //                      $"是否接近边缘：{isNearEdge}, 是否正在离开：{isLeaving}");

            // 更新上次的边缘距离
            lastEdgeDistance = currentEdgeDistance;

            return isNearEdge && isLeaving;

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(VortexMapItemCollider), "OnTriggerStay2D")]
        public static void OnTriggerStay2DPostfix(VortexMapItemCollider __instance, Collider2D other)
        {

            // 获取旋涡的 CircleCollider2D
            CircleCollider2D vortexCollider = __instance.GetComponent<CircleCollider2D>();
            if (vortexCollider == null) return;
            //// 检查指示器是否接近边缘
            if (Keyboard.current[potionEdgeSnappingHotkey.Value].isPressed && IsNearOutEdge(vortexCollider, other))
            {
                stopMovingPotion = true;
            }

        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(VortexMapItemCollider), "OnTriggerEnter2D")]
        public static void OnTriggerEnter2DPrefix(VortexMapItemCollider __instance, Collider2D other)
        {
            //LoggerWrapper.LogInfo($"药水进入了旋涡");
           
            if (Keyboard.current[potionEdgeSnappingHotkey.Value].isPressed) stopMovingPotion = true;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(VortexMapItemCollider), "OnTriggerExit2D")]
        //public static void OnTriggerExit2DPostfix(VortexMapItemCollider __instance, Collider2D other)
        //{
        //    LoggerWrapper.LogInfo($"药水离开了旋涡");
        //}



        //空置的搅拌和加水的办法
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Cauldron), "UpdateStirringValue")]
        public static bool StirDisablePrefix(ref float ___StirringValue)
        {
            bool result=true;
            if (stopMovingPotion)
            {
                ___StirringValue = 0f;
                result = false;
            }
            return result;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(RecipeMapManager), "GetSpeedOfMovingTowardsBase")]
        public static bool LadleDisablePrefix(ref float __result)
        {
            bool result = true;
            if (stopMovingPotion)
            {
                __result = 0f;
                result = false;
            }
            return result;
        }

        #endregion 旋涡贴边







        //#region 帧数优化
        //UpdateSprite()

        //[HarmonyPatch(typeof(GrowthHandler), "UpdateSprite")]
        //[HarmonyPostfix]
        //public static void UpdateSpritePostfix(GrowthHandler __instance)
        //{
        //    if (__instance == null) return;
        //    GrowingSpotController growingSpotController = CommonUtils.GetPropertyValueS<GrowingSpotController>(__instance, "GrowingSpot");
        //    Transform backgroundTransform = growingSpotController.gameObject.transform.Find("Default GrowingSpot VisualObject/Visual Object/Backround");
        //    GameObjectHelper.StopGameObjectParticleSystems(backgroundTransform);


        //    //ParticleAndSpriteManager.StopParticleSystemsAndDisableSprites(targetObject);
        //}


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
        //#endregion 帧数优化



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
                    if (!grab && Mouse.current.rightButton.wasPressedThisFrame)
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
                        SubstanceGrinding substanceGrinding = context.stack.substanceGrinding;

                        context.stack.leavesGrindStatus = 1f;
                        context.stack.overallGrindStatus = 1f;
                        substanceGrinding.CurrentGrindStatus = 1f;
                        substanceGrinding.grindTicksPerformed = substanceGrinding.GrindTicksToFullGrind;
                        context.stack.UpdateGrindedSubstance();
                        foreach (StackItem stackItem in context.stack.itemsFromThisStack.ToList<StackItem>())
                        {
                            bool flag2 = stackItem.GetType() == typeof(IngredientFromStack);
                            if (flag2)
                            {
                                IngredientFromStack ingredientFromStack = stackItem as IngredientFromStack;
                                CommonUtils.SetPropertyValueS(ingredientFromStack, "IsDestroyed", true);
                                Destroy(ingredientFromStack.gameObject);
                                ingredientFromStack.DestroyContainerRecursively(ingredientFromStack.transform.parent);
                            }
                        }
                        yield break;
                    }
                    elapsedTime += 0.1f;

                    yield return new WaitForSeconds(0.1f);

                }
                LoggerWrapper.LogInfo($"Stack {context.stack.name} 准备被碾压的时间超时，可能被收取，取消碾压。"); ;
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