using BepInEx;
using HarmonyLib;
using PotionCraft.DialogueSystem.Dialogue.Data;
using PotionCraft.DialogueSystem.Dialogue;
using PotionCraft.ObjectBased.RecipeMap.RecipeMapItem.VortexMapItem;
using PotionCraft.ObjectBased.UIElements.Dialogue;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraftAutoGarden.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PotionCraft.InventorySystem;
using PotionCraft.ManagersSystem.Trade;
using PotionCraft.ManagersSystem;
using PotionCraft.Npc.MonoBehaviourScripts;
using PotionCraft.ObjectBased.ScalesSystem;
using PotionCraft.ScriptableObjects;
using PotionCraft.QuestSystem;
using System.Linq;

namespace Ukersn_s_TweakWizard
{
    public class Abandoned
    {

        //private static ConfigEntry<bool> enableFrameRateOptimization;
        //private static ConfigEntry<bool> enableExtremeFrameRateOptimization;
        void Start()
        {


            //harmony.Patch(AccessTools.Method(typeof(SubstanceGrinding), "TryToGrind"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(TryToGrindPostfix)));
            //harmony.Patch(AccessTools.Method(typeof(Stack), "UpdateOverallGrindStatus"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(UpdateOverallGrindStatusPostfix)));
            //harmony.Patch(AccessTools.Method(typeof(Stack), "IncreaseLeavesGrindStatus"), postfix: new HarmonyMethod(typeof(TweakWizard), nameof(IncreaseLeavesGrindStatusPostfix)));



            //// 配置项2：帧数优化功能
            //enableFrameRateOptimization = config.Bind("Performance",
            //                                          "EnableFrameRateOptimization",
            //                                          false,
            //                                          "Warning: This is an experimental option\n" +
            //                                          "Turn this on to reduce rendering of items in non-adjacent rooms, potentially improving game FPS by up to 5%\n" +
            //                                          "I haven't fully tested it yet, it might cause save corruption or item loss.\n" +
            //                                          "警告：这是一个实验选项\n" +
            //                                          "开启它，会减少非相邻房间的物品渲染，换来约5%的游戏FPS提升\n" +
            //                                          "我还没完全测试它，它可能会导致存档损坏或物品丢失。");

            //// 配置项3：极端帧数优化功能
            //enableExtremeFrameRateOptimization = config.Bind("Performance",
            //                                                 "EnableExtremeFrameRateOptimization",
            //                                                 false,
            //                                                 "Note: Only effective when Frame Rate Optimization is enabled\n" +
            //                                                 "Turn this on to reduce rendering of items in rooms other than the current one, potentially improving game FPS by up to 9%.\n" +
            //                                                 "However, when switching to other rooms, you'll see items from the previous room appear to 'vanish'.\n" +
            //                                                 "注意：仅在帧数优化功能开启后才有效\n" +
            //                                                 "开启它，会减少非本房间的物品渲染，换来约9%的游戏FPS提升。\n" +
            //                                                 "但在切换其他房间的时候，你会看见切换前房间的物品好像'隐身'了。");

        }
        void Awake()
        {


            //harmony.Patch(AccessTools.Method(typeof(BuildableItemFromInventory), "FindAndSetCurrentRoomBuildZoneSet"), prefix: new HarmonyMethod(typeof(TweakWizard), nameof(FindAndSetCurrentRoomBuildZoneSetPrefix)));
            //harmony.Patch(AccessTools.Method(typeof(BuildZone), "GetRoomBuildZoneSet"), prefix: new HarmonyMethod(typeof(TweakWizard), nameof(GetRoomBuildZoneSetPrefix)));
            //Harmony.CreateAndPatchAll(typeof(TweakWizard));


            //harmony.PatchAll();
        }
        void Update()
        {


        }


        //[HarmonyPatch(typeof(BuildableItemFromInventory), "FindAndSetCurrentRoomBuildZoneSet")]
        //[HarmonyPrefix]
        //public static bool FindAndSetCurrentRoomBuildZoneSetPrefix(string buildZoneName, RoomIndex roomIndex)
        //{
        //    LoggerWrapper.LogInfo($"从 {roomIndex} 房间设定buildZone：{buildZoneName} ");
        //    return true; // 返回 true 以继续执行原始方法，返回 false 则跳过原始方法
        //}
        //[HarmonyPatch(typeof(BuildZone), "GetRoomBuildZoneSet")]
        //[HarmonyPrefix]

        //public static bool GetRoomBuildZoneSetPrefix(PotionCraft.ScriptableObjects.Room.Room room)
        //{
        //    LoggerWrapper.LogInfo($"房间BuildZoneSet {room} ");
        //    return true; // 返回 true 以继续执行原始方法，返回 false 则跳过原始方法
        //}



        //// 应用 NextGrindStatePrefix
        //harmony.Patch(
        //    AccessTools.Method(typeof(IngredientFromStack), "NextGrindState"),
        //    prefix: new HarmonyMethod(typeof(TweakWizard), nameof(NextGrindStatePrefix))
        //);


        //[HarmonyPatch(typeof(IngredientFromStack), "NextGrindState")]
        //[HarmonyPrefix]
        //public static bool NextGrindStatePrefix(IngredientFromStack __instance)
        //{

        //    float leavesGrindingChange = __instance.ingredient.GetLeavesGrindingChange(__instance.currentGrindState);
        //    if (__instance.NextStagePrefabs != null && __instance.NextStagePrefabs.Any())
        //    {
        //        __instance.TransformToNextGrindState();
        //    }
        //    else
        //    {
        //        // 使用反射设置 IsDestroyed 属性
        //        CommonUtils.SetPropertyValueS(__instance, "IsDestroyed", true);
        //        if (__instance.gameObject != null)
        //        {
        //            UnityEngine.Object.Destroy(__instance.gameObject); //会有bug...
        //        }
        //        if (__instance.transform.parent != null)
        //        {
        //            __instance.DestroyContainerRecursively(__instance.transform.parent);
        //        }
        //    }

        //    __instance.stackScript.IncreaseLeavesGrindStatus(leavesGrindingChange);

        //    return false; // 阻止原方法执行
        //}




        //[HarmonyPatch(typeof(RoomManager), "GoTo")]
        //[HarmonyPostfix]
        //public static void GoToPostfix(RoomIndex newTargetRoom, bool forceImmediately = false) //场景：开着建筑模式切换
        //{

        //    //帧数优化相关
        //    //if (enableFrameRateOptimization.Value) {
        //    //    GameObjectHelper.GetNonAdjacentRoomsToCurrentRoom(newTargetRoom);
        //    //    GameObjectHelper.SetAdjacentRoomsActiveAndOthersInactive(newTargetRoom, enableExtremeFrameRateOptimization.Value);
        //    //}


        //}







        // 假设我们要监控 SubstanceGrinding 类中的 CurrentGrindStatus 属性
        //[HarmonyPatch(typeof(SubstanceGrinding), "CurrentGrindStatus", MethodType.Setter)]
        //class CurrentGrindStatusPatch
        //{
        //    static void Prefix(SubstanceGrinding __instance, ref float value)
        //    {
        //        if (value != lastValue)
        //        {
        //            //Debug.Log($"数值变化 {lastValue} to {value}");
        //            //Debug.Log($"改变数值的方法: {new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().Name}");
        //            //LogStackTrace();
        //           lastValue = value;
        //        }
        //    }
        //}


        //static void LogStackTrace()
        //{
        //    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("UpdateOverallGrindStatus改变。调用堆栈：");

        //    for (int i = 1; i < stackTrace.FrameCount; i++) // 从2开始跳过Harmony和当前方法
        //    {
        //        var frame = stackTrace.GetFrame(i);
        //        var method = frame.GetMethod();

        //        sb.AppendLine($"在 {method.DeclaringType.FullName}.{method.Name}");

        //        if (frame.GetFileName() != null)
        //        {
        //            sb.AppendLine($"  文件: {frame.GetFileName()}");
        //            sb.AppendLine($"  行号: {frame.GetFileLineNumber()}");
        //        }

        //        sb.AppendLine();
        //    }

        //    Debug.Log(sb.ToString());
        //}

        //[HarmonyPatch(typeof(SubstanceGrinding), "TryToGrindPostfix")]
        //[HarmonyPostfix]
        //public static void TryToGrindPostfix(float pestleLinearSpeed, float pestleAngularSpeed)
        //{
        //    LoggerWrapper.LogInfo($"尝试研磨{pestleLinearSpeed},{pestleAngularSpeed}");
        //}

        //[HarmonyPatch(typeof(Stack), "UpdateOverallGrindStatus")]
        //[HarmonyPostfix]
        //public static void UpdateOverallGrindStatusPostfix()
        //{
        //    LoggerWrapper.LogInfo($"UpdateOverallGrindStatus");
        //    LogStackTrace();
        //}


        //[HarmonyPatch(typeof(Stack), "IncreaseLeavesGrindStatus")]
        //[HarmonyPostfix]
        //public static void IncreaseLeavesGrindStatusPostfix(float change)
        //{
        //    LoggerWrapper.LogInfo($"IncreaseLeavesGrindStatus {change}");
        //}

        //[HarmonyPatch(typeof(IngredientFromStack))]
        //[HarmonyPatch("Smash")]
        //public class SmashPatch
        //{
        //    static bool Prefix(IngredientFromStack __instance)
        //    {
        //        // 检查是否可以被损坏（研磨）



        //        Ingredient inventoryItem = (Ingredient)CommonUtils.GetPropertyValueS<InventoryItem>(__instance.stackScript, "InventoryItem");
        //        if (!(inventoryItem.canBeDamaged))
        //        {
        //            return false; // 不执行原始方法
        //        }

        //        // 直接调用 TryToGrind，不检查 currentGrindState
        //        __instance.TryToGrind();
        //        __instance.stackScript.UpdateGrindedSubstance();
        //        // 如果希望一次性研磨到最大，可以在这里添加循环
        //        // while (__instance.currentGrindState < GetMaxGrindState(__instance))
        //        // {
        //        //     __instance.TryToGrind();
        //        // }

        //        return false; // 不执行原始方法
        //    }

        //    // 获取最大研磨状态的辅助方法
        //    static int GetMaxGrindState(IngredientFromStack item)
        //    {
        //        // 假设最大研磨状态存储在一个名为 maxGrindState 的私有字段中
        //        FieldInfo fieldInfo = typeof(IngredientFromStack).GetField("maxGrindState", BindingFlags.Instance | BindingFlags.NonPublic);
        //        return (int)fieldInfo.GetValue(item);
        //    }
        //}













        //不合理的方法：因为所有Stack共用同类型的成分 当房间有多个同类型Stack，总是找到第一个
        //public static Stack fromIngredientFindStack(Ingredient ingredient)
        //{
        //    GameObject itemContainer = GameObject.Find("ItemContainer");

        //    // 遍历 ItemContainer 的所有子对象
        //    foreach (Transform child in itemContainer.transform)
        //    {
        //        Stack stack = child.GetComponent<Stack>();
        //        if (stack != null && stack.Ingredient == ingredient)
        //        {
        //            return stack;
        //        }
        //    }
        //    return null;
        //}

        //private static GameObject edgeIndicator;

        //private static void CreateEdgeIndicator()
        //{
        //    try
        //    {
        //        if (edgeIndicator != null)return;

        //        // 查找 Map Water 场景
        //        Scene mapWaterScene = SceneManager.GetSceneByName("Map Water");
        //        if (!mapWaterScene.isLoaded)
        //        {
        //            LoggerWrapper.LogError("Map Water 场景未加载。");
        //            return;
        //        }

        //        // 查找 MapItemsContainer
        //        GameObject mapItemsContainer = null;
        //        foreach (GameObject rootObj in mapWaterScene.GetRootGameObjects())
        //        {
        //            mapItemsContainer = rootObj.gameObject;
        //            if (mapItemsContainer != null) break;
        //        }
        //        edgeIndicator = new GameObject("VortexEdgeIndicator");
        //        edgeIndicator.transform.SetParent(mapItemsContainer.transform, false);
        //        SpriteRenderer spriteRenderer = edgeIndicator.AddComponent<SpriteRenderer>();

        //        Texture2D ringTexture = CreateRingTexture(128, 128, Color.red, 0.9f, 1.0f);
        //        Sprite edgeSprite = Sprite.Create(ringTexture, new Rect(0, 0, ringTexture.width, ringTexture.height), new Vector2(0.5f, 0.5f));

        //        spriteRenderer.sprite = edgeSprite;
        //        spriteRenderer.sortingOrder = 30000; // 比旋涡的 12317 高 1
        //        edgeIndicator.SetActive(true); // 初始时隐藏

        //        LoggerWrapper.LogInfo("边缘指示器创建成功。");
        //    }
        //    catch (Exception e)
        //    {
        //        LoggerWrapper.LogError($"创建边缘指示器时发生异常: {e.Message}");
        //        if (edgeIndicator != null)
        //        {
        //            Destroy(edgeIndicator);
        //            edgeIndicator = null;
        //        }
        //    }
        //}
        //private static void CreateEdgeIndicator()
        //{
        //    try
        //    {
        //        if (edgeIndicator != null) return;

        //        // 查找 Map Water 场景
        //        Scene mapWaterScene = SceneManager.GetSceneByName("Map Water");
        //        if (!mapWaterScene.isLoaded)
        //        {
        //            LoggerWrapper.LogError("Map Water 场景未加载。");
        //            return;
        //        }

        //        // 查找 MapItemsContainer
        //        GameObject mapItemsContainer = null;
        //        foreach (GameObject rootObj in mapWaterScene.GetRootGameObjects())
        //        {
        //            mapItemsContainer = rootObj.gameObject;
        //            if (mapItemsContainer != null) break;
        //        }

        //        edgeIndicator = new GameObject("VortexEdgeIndicator");
        //        edgeIndicator.layer = 0;

        //        edgeIndicator.transform.SetParent(mapItemsContainer.transform, false);
        //        edgeIndicator.transform.localPosition = Vector3.zero; // 使用localPosition
        //        edgeIndicator.transform.localScale = Vector3.one * 1.5f; // 设置适当的缩放

        //        SpriteRenderer spriteRenderer = edgeIndicator.AddComponent<SpriteRenderer>();

        //        Texture2D ringTexture = CreateRingTexture(128, 128, Color.red, 0.9f, 1.0f);
        //        Sprite edgeSprite = Sprite.Create(ringTexture, new Rect(0, 0, ringTexture.width, ringTexture.height), new Vector2(0.5f, 0.5f));

        //        spriteRenderer.sprite = edgeSprite;
        //        spriteRenderer.sortingLayerName = "Default";
        //        spriteRenderer.sortingOrder = 30000;
        //        spriteRenderer.enabled = true; // 明确启用SpriteRenderer

        //        // 添加BoxCollider2D
        //        BoxCollider2D collider = edgeIndicator.AddComponent<BoxCollider2D>();
        //        collider.size = edgeSprite.bounds.size;
        //        collider.isTrigger = true;

        //        edgeIndicator.SetActive(true);

        //        LoggerWrapper.LogInfo("边缘指示器创建成功。");
        //    }
        //    catch (Exception e)
        //    {
        //        LoggerWrapper.LogError($"创建边缘指示器时发生异常: {e.Message}");
        //        if (edgeIndicator != null)
        //        {
        //            Destroy(edgeIndicator);
        //            edgeIndicator = null;
        //        }
        //    }
        //}

        //private static Texture2D CreateRingTexture(int width, int height, Color color, float innerRadius, float outerRadius)
        //{
        //    Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        //    Color[] colors = new Color[width * height];

        //    Vector2 center = new Vector2(width / 2f, height / 2f);
        //    float maxRadius = Mathf.Min(width, height) / 2f;

        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            Vector2 pixel = new Vector2(x, y);
        //            float distance = Vector2.Distance(pixel, center) / maxRadius;

        //            if (distance >= innerRadius && distance <= outerRadius)
        //            {
        //                colors[y * width + x] = color;
        //            }
        //            else
        //            {
        //                colors[y * width + x] = Color.clear;
        //            }
        //        }
        //    }

        //    texture.SetPixels(colors);
        //    texture.Apply();
        //    return texture;
        //}

        //private static void UpdateEdgeIndicator(VortexMapItemCollider vortex, CircleCollider2D vortexCollider)
        //{
        //    if (TweakWizard.edgeIndicator == null) return;
        //    if (vortex != lastVortex || !TweakWizard.edgeIndicator.activeSelf)
        //    {
        //        // 移动边缘指示器到新的位置
        //        TweakWizard.edgeIndicator.transform.position = vortex.transform.position;

        //        // 调整边缘指示器的大小
        //        float scale = vortexCollider.radius * 2.1f; // 稍微大于旋涡
        //        TweakWizard.edgeIndicator.transform.localScale = new Vector3(scale, scale, 1);

        //        TweakWizard.edgeIndicator.SetActive(true);
        //        lastVortex = vortex;
        //    }
        //    AdjustOrthographicCamera();

        //}
        //private static void UpdateEdgeIndicator(VortexMapItemCollider vortex, CircleCollider2D vortexCollider)
        //{
        //    if (TweakWizard.edgeIndicator == null) return;
        //    if (vortex != lastVortex || !TweakWizard.edgeIndicator.activeSelf)
        //    {
        //        // 移动边缘指示器到新的位置
        //        TweakWizard.edgeIndicator.transform.position = vortex.transform.position;

        //        // 调整边缘指示器的大小
        //        float scale = vortexCollider.radius * 2.1f; // 稍微大于旋涡
        //        TweakWizard.edgeIndicator.transform.localScale = new Vector3(scale, scale, 1);

        //        TweakWizard.edgeIndicator.SetActive(true);
        //        lastVortex = vortex;
        //    }
        //    AdjustOrthographicCamera();
        //}
        //private static void AdjustOrthographicCamera()
        //{
        //    Camera mainCamera = Camera.main;
        //    if (mainCamera == null || !mainCamera.orthographic) return;


        //    //edgeIndicator.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);


        //    // 获取指示器的位置
        //    //var indicatorPosition = edgeIndicator.transform.position;
        //    // 调整相机位置，但保持z轴不变
        //    //mainCamera.transform.position = new Vector3(indicatorPosition.x, indicatorPosition.y, mainCamera.transform.position.z);


        //    //// 适度增加正交相机的大小
        //    //mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 10f);

        //    //LoggerWrapper.LogInfo($"调整后的2D相机位置: {mainCamera.transform.position}, 正交大小: {mainCamera.orthographicSize}");
        //}


        //private static void HideEdgeIndicator()
        //{
        //    if (edgeIndicator != null)
        //    {
        //        edgeIndicator.SetActive(false);
        //    }
        //    lastVortex = null;
        //}



        //private static VortexMapItemCollider lastVortex;

        //[HarmonyPatch(typeof(DialogueBox), "SpawnPotionRequestInterface")]
        //[HarmonyPostfix]
        //static void SpawnPotionRequestInterfacePostfix(DialogueBox __instance, PotionRequestNodeData potionRequestNodeData, DialogueData dialogueData)
        //{
        //    LoggerWrapper.LogInfo("创建药水按钮");
        //    // 获取现有按钮数量
        //    int currentButtonCount = __instance.dialogueButtons.Length;

        //    //// 创建新的按钮数组
        //    DialogueButton[] newButtons = new DialogueButton[currentButtonCount + 1];
        //    Array.Copy(__instance.dialogueButtons, newButtons, currentButtonCount);

        //    // 创建新按钮
        //    Vector2 position = new Vector2(0, -(__instance.spaceAnswers + __instance.dialogueButtons[currentButtonCount - 1].thisCollider.bounds.size.y));
        //    DialogueButton newButton = DialogueBox.dialogueButtonSpawner.Spawn(__instance.dialogueButtons[currentButtonCount - 1].SubPoolIndex, position);//这里使用之前最后一个按钮(返回或结束按钮)的预制体(也就是会影响按钮外观)
        //    var autoFindPotionKey = new PotionCraft.LocalizationSystem.Key("#trading_start_haggle", null, PotionCraft.LocalizationSystem.KeyParametersStyle.Normal, null, null);
        //    //由于未知的语言化问题，这里key不使用注册本地化后的标志来显示(默认使用#dialogue_give_potion代替)(不知道为什么会失败)
        //    // 设置按钮内容
        //    newButton.SetContent(
        //        autoFindPotionKey,
        //        null,
        //        __instance.givePotionIcon,
        //        () => { AutoFindPotion(); },  // 这里调用你的自定义方法
        //        currentButtonCount,//这里使用之前最后一个按钮(返回或结束按钮)的索引
        //        null,
        //        null,
        //        1f,
        //        null
        //    );
        //    //使用下面这种方式重新设定按钮的文本
        //    newButton.leftText.Text.text = PotionCraft.LocalizationSystem.LocalizationManager.GetText("#mod_ukersn_s_tweakwizard_auto_find_potion");

        //    // 添加新按钮到数组
        //    newButtons[currentButtonCount] = newButton;
        //    // 更新DialogueBox的按钮数组
        //    __instance.dialogueButtons = newButtons;
        //    // 更新对话框高度
        //    __instance.dialogueText.minTextBoxY += newButton.thisCollider.bounds.size.y + __instance.spaceAnswers;
        //}

        //static void AutoFindPotion()
        //{
        //    // 在这里实现自动寻找药水的逻辑
        //    LoggerWrapper.LogInfo("自动寻找药水");
        //}




        //[HarmonyPatch(typeof(DialogueBox), "SpawnPotionRequestInterface")]
        //[HarmonyPostfix]
        //static void SpawnPotionRequestInterfacePostfix(DialogueBox __instance, PotionRequestNodeData potionRequestNodeData, DialogueData dialogueData)
        //{//在创建npc对话框的按钮后追加一个新按钮
        //    // 获取现有按钮数量
        //    int currentButtonCount = __instance.dialogueButtons.Length;

        //    //// 创建新的按钮数组
        //    DialogueButton[] newButtons = new DialogueButton[currentButtonCount + 1];

        //    // 计算新按钮的位置（使用倒数第二个按钮的位置）
        //    Vector2 position = __instance.dialogueButtons[currentButtonCount - 2].transform.localPosition;

        //    // 创建新按钮
        //    DialogueButton newButton = DialogueBox.dialogueButtonSpawner.Spawn(__instance.dialogueButtons[currentButtonCount - 1].SubPoolIndex, position);//这里使用之前最后一个按钮(返回或结束按钮)的预制体(也就是会影响按钮外观)
        //    var autoFindPotionKey = new PotionCraft.LocalizationSystem.Key("#mod_ukersn_s_tweakwizard_quick_potion_pick", new List<string> { "qwq", "owo" }, PotionCraft.LocalizationSystem.KeyParametersStyle.Normal, null, null);
        //    // 设置按钮内容
        //    newButton.SetContent(
        //        autoFindPotionKey,
        //        null,
        //        __instance.givePotionIcon,
        //        () => { AutoFindPotion(); },  // 这里调用你的自定义方法
        //        currentButtonCount,//这里使用之前最后一个按钮(返回或结束按钮)的索引
        //        null,
        //        null,
        //        1f,
        //        null
        //    );
        //    //使用下面这种方式重新设定按钮的文本 (不需要了，通过RegisterLoc("mod_ukersn_s_tweakwizard_auto_find_potion")解决了,注册的时候不要携带#字符串开头，是错的)
        //    //newButton.leftText.Text.text = String.Format(PotionCraft.LocalizationSystem.LocalizationManager.GetText("#mod_ukersn_s_tweakwizard_auto_find_potion"),"Value");

        //    newButton.SetAlpha(0f);
        //    Array.Copy(__instance.dialogueButtons, newButtons, currentButtonCount);

        //    //移动按钮到倒数第二个的位置。
        //    newButtons[currentButtonCount - 1] = newButton;
        //    newButtons[currentButtonCount] = __instance.dialogueButtons[currentButtonCount - 1];

        //    // 更新DialogueBox的按钮数组
        //    __instance.dialogueButtons = newButtons;

        //    // 更新所有按钮的位置
        //    for (int i = currentButtonCount - 1; i < newButtons.Length; i++)
        //    {
        //        Vector2 newPosition = newButtons[i].transform.localPosition;
        //        newPosition.y -= newButton.thisCollider.bounds.size.y + __instance.spaceAnswers;
        //        newButtons[i].transform.localPosition = newPosition;
        //    }
        //    // 更新对话框高度
        //    __instance.dialogueText.minTextBoxY += newButton.thisCollider.bounds.size.y + __instance.spaceAnswers;



        //    //我希望将那个按钮的长度缩短为2分之一，然后右侧再提供两个按钮各占4分之一左右切换符合条件药水队列的功能。
        //}











        //[HarmonyPatch(typeof(DialogueBox), "SpawnPotionRequestInterface")]
        //[HarmonyPostfix]
        //static void SpawnPotionRequestInterfacePostfix(DialogueBox __instance, PotionRequestNodeData potionRequestNodeData, DialogueData dialogueData)
        //{//在创建npc对话框的按钮后追加一个新按钮
        //    // 获取现有按钮数量
        //    int currentButtonCount = __instance.dialogueButtons.Length;

        //    //// 创建新的按钮数组
        //    DialogueButton[] newButtons = new DialogueButton[currentButtonCount + 3];

        //    // 计算新按钮的位置（使用倒数第二个按钮的位置）
        //    Vector2 localPosition = __instance.dialogueButtons[currentButtonCount - 2].transform.localPosition;

        //    // 创建主按钮（自动查找药水）
        //    DialogueButton mainButton = CreateButton(__instance, localPosition, "#mod_ukersn_s_tweakwizard_quick_potion_pick", () => { AutoFindPotion(); }, currentButtonCount, 0.5f, __instance.givePotionIcon);

        //    // 创建左侧切换按钮
        //    DialogueButton leftButton = CreateButton(__instance, localPosition, "mod_ukersn_s_tweakwizard_previous_potion_pick", () => { PreviousPotion(); }, currentButtonCount, 0.25f, null);

        //    // 创建右侧切换按钮
        //    DialogueButton rightButton = CreateButton(__instance, localPosition, "mod_ukersn_s_tweakwizard_next_potion_pick", () => { NextPotion(); }, currentButtonCount, 0.25f, null);


        //    //// 设置按钮位置
        //    //mainButton.transform.localPosition = new Vector2(localPosition.x - mainButton.thisCollider.bounds.size.x / 4, localPosition.y);
        //    //leftButton.transform.localPosition = new Vector2(localPosition.x + mainButton.thisCollider.bounds.size.x / 8, localPosition.y);
        //    //rightButton.transform.localPosition = new Vector2(localPosition.x + (mainButton.thisCollider.bounds.size.x / 8*3), localPosition.y);



        //    //使用下面这种方式重新设定按钮的文本 (不需要了，通过RegisterLoc("mod_ukersn_s_tweakwizard_auto_find_potion")解决了,注册的时候不要携带#字符串开头，是错的)
        //    //newButton.leftText.Text.text = String.Format(PotionCraft.LocalizationSystem.LocalizationManager.GetText("#mod_ukersn_s_tweakwizard_auto_find_potion"),"Value");

        //    Array.Copy(__instance.dialogueButtons, newButtons, currentButtonCount);


        //    newButtons[currentButtonCount - 1] = mainButton;
        //    newButtons[currentButtonCount] = leftButton;
        //    newButtons[currentButtonCount + 1] = rightButton;
        //    newButtons[currentButtonCount + 2] = __instance.dialogueButtons[currentButtonCount - 1];


        //    // 更新DialogueBox的按钮数组
        //    __instance.dialogueButtons = newButtons;

        //    // 更新按钮位置
        //    Vector2 newRowPosition = __instance.dialogueButtons[currentButtonCount - 1].transform.localPosition;
        //    newRowPosition.y -= mainButton.thisCollider.bounds.size.y + __instance.spaceAnswers;

        //    // 设置新一排按钮的位置
        //    //mainButton.transform.localPosition = newRowPosition;
        //    //leftButton.transform.localPosition = new Vector2(newRowPosition.x + mainButton.thisCollider.bounds.size.x * 0.5f, newRowPosition.y);
        //    //rightButton.transform.localPosition = new Vector2(newRowPosition.x + mainButton.thisCollider.bounds.size.x * 0.75f, newRowPosition.y);


        //    // 设置按钮位置
        //    mainButton.transform.localPosition = new Vector2(newRowPosition.x - mainButton.thisCollider.bounds.size.x / 4, newRowPosition.y);
        //    leftButton.transform.localPosition = new Vector2(newRowPosition.x + mainButton.thisCollider.bounds.size.x / 8, newRowPosition.y);
        //    rightButton.transform.localPosition = new Vector2(newRowPosition.x + (mainButton.thisCollider.bounds.size.x / 8 * 3), newRowPosition.y);

        //    // 移动原来的最后一个按钮（通常是"返回"或"结束"按钮）
        //    newButtons[currentButtonCount + 2].transform.localPosition = new Vector2(
        //        newButtons[currentButtonCount + 2].transform.localPosition.x,
        //        newRowPosition.y - (mainButton.thisCollider.bounds.size.y + __instance.spaceAnswers)
        //    );

        //    // 更新对话框高度
        //    __instance.dialogueText.minTextBoxY += mainButton.thisCollider.bounds.size.y + __instance.spaceAnswers;


        //    //我希望将那个按钮的长度缩短为2分之一，然后右侧再提供两个按钮各占4分之一左右切换符合条件药水队列的功能。
        //}

        //static DialogueButton CreateButton(DialogueBox __instance, Vector2 position, string localizationKey, Action onClick, int subPoolIndex, float widthScale, DialogueButtonIcon icon)
        //{
        //    DialogueButton button = DialogueBox.dialogueButtonSpawner.Spawn(__instance.dialogueButtons[subPoolIndex - 3].SubPoolIndex, position);
        //    var key = new PotionCraft.LocalizationSystem.Key(localizationKey, new List<string>(), PotionCraft.LocalizationSystem.KeyParametersStyle.Normal, null, null);

        //    button.SetContent(
        //        key,
        //        null,
        //        icon,
        //        onClick,
        //        subPoolIndex,
        //        null,
        //        null,
        //        1f,
        //        null
        //    );

        //    button.SetAlpha(0f);

        //    // 调整按钮宽度
        //    //var rectTransform = button.GetComponent<RectTransform>();
        //    //if (rectTransform != null)
        //    //{
        //    //    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * widthScale, rectTransform.sizeDelta.y);
        //    //}
        //    // 调整按钮大小
        //    AdjustButtonSize(button, widthScale);

        //    return button;
        //}

        //static void AdjustButtonSize(DialogueButton button, float widthScale)
        //{
        //    // 找到 Background 对象
        //    Transform backgroundTransform = button.transform.Find("Background");
        //    if (backgroundTransform == null) return;
        //    Transform leftTextTransform = backgroundTransform.Find("LeftText");
        //    if (leftTextTransform == null) return;
        //    Transform rightTextTransform = backgroundTransform.Find("RightText");
        //    if (rightTextTransform == null) return;
        //    Transform iconTransform = backgroundTransform.Find("Icon");
        //    if (iconTransform == null) return;

        //    // 调整 Background 的缩放
        //    Vector3 originalScale = backgroundTransform.localScale;
        //    backgroundTransform.localScale = new Vector3(widthScale, originalScale.y, originalScale.z);

        //    // LeftText等子组件 受到影响，通过反向缩放来恢复它
        //    leftTextTransform.localScale = new Vector3(1f / widthScale, 1f, 1f);
        //    rightTextTransform.localScale = new Vector3(1f / widthScale, 1f, 1f);
        //    iconTransform.localScale = new Vector3(1f / widthScale, 1f, 1f);

        //    //根据缩放计算调整后的位置
        //    Vector3 originalPosition = leftTextTransform.localPosition;
        //    leftTextTransform.localPosition = new Vector3(originalPosition.x + (float)(((1 / widthScale) - 1) * 0.225 + 0.1), originalPosition.y, originalPosition.z);
        //    originalPosition = rightTextTransform.localPosition;
        //    rightTextTransform.localPosition = new Vector3(originalPosition.x + (float)(((1 / widthScale) - 1) * 0.225), originalPosition.y, originalPosition.z);
        //    originalPosition = iconTransform.localPosition;
        //    iconTransform.localPosition = new Vector3(originalPosition.x + (float)(((1 / widthScale) - 1) * 0.225), originalPosition.y, originalPosition.z);
        //}

        //static void PreviousPotion()
        //{
        //    // 实现切换到上一个符合条件的药水的逻辑
        //}

        //static void NextPotion()
        //{
        //    // 实现切换到下一个符合条件的药水的逻辑
        //}






        // harmony.Patch(
        //AccessTools.Method(typeof(TradeManager), "RecalculateDealCost"),
        //         postfix: new HarmonyMethod(typeof(TweakWizard), nameof(RecalculateDealCostPostfix))
        //     );

        //[HarmonyPatch(typeof(TradeManager), "RecalculateDealCost")]
        //[HarmonyPostfix]
        //static void RecalculateDealCostPostfix(TradeManager __instance, bool updateBargainedValue, bool updateHaggleWindow)
        //{
        //    ScalesCupDisplay display = Scales.Instance.rightCupScript.display;
        //    if (display.currentPotionItem != null)
        //    {
        //        if (display.isCurrentPotionSuitable)
        //        {
        //            NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;
        //            InventoryItem inventoryItem = CommonUtils.GetPropertyValueS<InventoryItem>(display.currentPotionItem, "InventoryItem");
        //            if (Managers.Dialogue.State != DialogueState.ClosenessPotionRequest)
        //            {
        //                int price = Mathf.FloorToInt(__instance.GetCurrentPriceForItem(inventoryItem, Inventory.Owner.Player, 1, currentNpcMonoBehaviour) - __instance.TraderItemsCost);

        //                LoggerWrapper.LogInfo($"符合条件的药水：{inventoryItem.name} 价值：{price}  同一药水？{lastInventoryItem == inventoryItem} {object.ReferenceEquals(lastInventoryItem, inventoryItem)}  {object.ReferenceEquals(currentNpcMonoBehaviour, lastNpcMonoBehaviour)} {__instance.TraderItemsCost} {Managers.Trade.GetCurrentPriceForItem(inventoryItem, Inventory.Owner.Player)}");
        //            }

        //        }
        //    }
        //}




        //public static Potion CreateSuitablePotion()
        //{//创建一个虚拟的适合任务的药水 PotionCraft.ManagersSystem.Potion.PotionManager.CreateQuestPotion()的截取  但不是最大价值的药水。

        //    NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;
        //    DarkScreen.DeactivateAll(DarkScreenDeactivationType.Other, DarkScreenLayer.Lower, true, null);
        //    List<GeneratedQuestRequirement> list = new List<GeneratedQuestRequirement>();
        //    list.AddRange(currentNpcMonoBehaviour.mandatoryQuestRequirements);
        //    list.AddRange(currentNpcMonoBehaviour.optionalQuestRequirements);
        //    List<PotionEffect> list2 = currentNpcMonoBehaviour.currentQuest.desiredEffects.ToList<PotionEffect>();
        //    list2.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));
        //    PotionEffect potionEffect = list2[0];
        //    List<PotionEffect> list3 = new List<PotionEffect>();
        //    List<PotionEffect> list4 = PotionEffect.allPotionEffects.ToList<PotionEffect>();
        //    list4.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));
        //    int num = 0;
        //    bool flag = false;
        //    for (int l = 0; l < list.Count; l++)
        //    {
        //        GeneratedQuestRequirement generatedQuestRequirement = list[l];
        //        if (generatedQuestRequirement.requirementInQuest.requirement is QuestRequirementPotionQuality)
        //        {
        //            num = generatedQuestRequirement.intValue1;
        //        }
        //        else if (generatedQuestRequirement.requirementInQuest.requirement is QuestRequirementsAdditionalEffects)
        //        {
        //            flag = true;
        //        }
        //    }
        //    if (flag)
        //    {
        //        int num2 = 5 - Mathf.Max(num, 1);
        //        PotionEffect lastEffect = potionEffect;

        //        for (int j = 0; j < num2; j++)
        //        {
        //            list4.Remove(lastEffect);

        //            // 使用 Lambda 表达式直接定义 predicate
        //            list4 = list4.Where(e => PotionEffect.IsEffectsCompatible(lastEffect, e)).ToList();

        //            if (list4.Count == 0)
        //            {
        //                break;
        //            }

        //            lastEffect = list4[0];
        //            list3.Add(lastEffect);
        //        }
        //    }
        //    List<PotionEffect> list5 = new List<PotionEffect>();
        //    int num3 = (num == 0) ? Mathf.Min(3, 5 - list3.Count) : num;
        //    for (int k = 0; k < num3; k++)
        //    {
        //        list5.Add(potionEffect);
        //    }
        //    foreach (PotionEffect item in list3)
        //    {
        //        list5.Add(item);
        //    }
        //    PotionEffect[] array = list5.ToArray();
        //    PotionBase potionBase = Settings<RecipeMapManagerPotionBasesSettings>.Asset.potionBases[0];
        //    List<Ingredient> list6 = Ingredient.allIngredients.ToList<Ingredient>();
        //    Dictionary<Ingredient, int> dictionary = new Dictionary<Ingredient, int>();
        //    int num4 = -1;
        //    bool allowSalts = true;
        //    foreach (GeneratedQuestRequirement generatedQuestRequirement2 in list)
        //    {
        //        QuestRequirement requirement = generatedQuestRequirement2.requirementInQuest.requirement;
        //        if (!(requirement is QuestRequirementNoParticularIngredient))
        //        {
        //            if (!(requirement is QuestRequirementMainIngredient))
        //            {
        //                if (!(requirement is QuestRequirementNeedOneParticularIngredient))
        //                {
        //                    if (!(requirement is QuestRequirementOnlyParticularBase))
        //                    {
        //                        if (!(requirement is QuestRequirementNoParticularBase))
        //                        {
        //                            if (!(requirement is QuestRequirementMaxIngredients))
        //                            {
        //                                if (requirement is QuestRequirementNoSalts)
        //                                {
        //                                    allowSalts = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                num4 = generatedQuestRequirement2.intValue1;
        //                            }
        //                        }
        //                        else if (potionBase.name.Equals(generatedQuestRequirement2.stringValue1))
        //                        {
        //                            potionBase = Settings<RecipeMapManagerPotionBasesSettings>.Asset.potionBases[1];
        //                        }
        //                    }
        //                    else
        //                    {
        //                        potionBase = PotionBase.GetByName(generatedQuestRequirement2.stringValue1, false, true);
        //                    }
        //                }
        //                else
        //                {
        //                    Ingredient byName = Ingredient.GetByName(generatedQuestRequirement2.stringValue1, false, true);
        //                    dictionary[byName] = 1;
        //                }
        //            }
        //            else
        //            {
        //                Ingredient byName2 = Ingredient.GetByName(generatedQuestRequirement2.stringValue1, false, true);
        //                dictionary[byName2] = 1000;
        //            }
        //        }
        //        else
        //        {
        //            GeneratedQuestRequirement requirement1 = generatedQuestRequirement2;
        //            list6 = (from i in list6
        //                     where !i.name.Equals(requirement1.stringValue1)
        //                     select i).ToList<Ingredient>();
        //        }
        //    }
        //    int count = dictionary.Count;
        //    if (num4 != -1 && count > 0)
        //    {
        //        num4 -= Mathf.Min(count, num4);
        //    }
        //    SimpleAlchemySubstanceComponents defaultUsedElements = PotionEffect.GetDefaultUsedElements(array, potionBase, list6, num4, allowSalts);
        //    foreach (KeyValuePair<Ingredient, int> keyValuePair in dictionary)
        //    {
        //        defaultUsedElements.Add(new AlchemySubstanceComponent(keyValuePair.Key, keyValuePair.Value), -1);
        //    }
        //    PotionEffect[] effects = array;
        //    ColoredIcon coloredIcon = null;
        //    Bottle bottle = null;
        //    Sticker sticker = null;
        //    PotionBase potionBase2 = potionBase;
        //    SimpleAlchemySubstanceComponents components = defaultUsedElements;
        //    Potion potion = PotionGenerator.GeneratePotion(effects, coloredIcon, bottle, sticker, null, potionBase2, components, "", false, "");
        //    return potion;
        //}



        //public static int GetPotionMaxCost(PotionEffect[] currentEffects, PotionEffect[] desiredEffects)
        //{
        //    int cost = 0;


        //    if (Managers.Debug.IsCurrentNpcAlexNichiporchik() || Managers.Debug.IsCurrentNpcGenerousCustomer())
        //    {
        //        desiredEffects = null;
        //    }

        //    Dictionary<PotionEffect, int> dictionary = new Dictionary<PotionEffect, int>();
        //    foreach (PotionEffect potionEffect in currentEffects)
        //    {
        //        if (!(potionEffect == null) && !dictionary.TryAdd(potionEffect, 1))
        //        {
        //            Dictionary<PotionEffect, int> dictionary2 = dictionary;
        //            PotionEffect key = potionEffect;
        //            int num = dictionary2[key];
        //            dictionary2[key] = num + 1;
        //        }
        //    }
        //    List<PotionEffect> list = dictionary.Keys.ToList<PotionEffect>();
        //    float num2 = 0f;
        //    for (int j = 0; j < list.Count; j++)
        //    {
        //        PotionEffect potionEffect2 = list[j];
        //        if (desiredEffects == null || desiredEffects.Contains(potionEffect2))
        //        {
        //            int num5 = dictionary[potionEffect2];
        //            num2 += Settings<PotionManagerSettings>.Asset.effectTierPriceMultiplier[num5 - 1] * (float)potionEffect2.price;
        //        }
        //    }
        //    PotionReview potionReview = new PotionReview
        //    {
        //        cost = Mathf.FloorToInt(num2),
        //    };

        //    if (Managers.Debug.IsCurrentNpcAlexNichiporchik())
        //    {
        //        cost = 4000;
        //    }
        //    return cost;
        //}

        //public static int GetMaxMatchingPotionPrice(PotionEffect[] desiredEffects)
        //{

        //    // 如果是特殊NPC，直接返回固定价格
        //    if (Managers.Debug.IsCurrentNpcAlexNichiporchik()) return 4000;
        //    // 如果没有期望效果,那还算个屁
        //    if (desiredEffects == null || desiredEffects.Length == 0) return 0;

        //    //构造效果
        //    //根据价格对效果排序
        //    List<PotionEffect> desiredEffectList = desiredEffects.ToList<PotionEffect>();
        //    desiredEffectList.Sort((PotionEffect a, PotionEffect b) => b.price.CompareTo(a.price));

        //    //所有任务需求列表
        //    NpcMonoBehaviour currentNpcMonoBehaviour = Managers.Npc.CurrentNpcMonoBehaviour;
        //    List<GeneratedQuestRequirement> list = new List<GeneratedQuestRequirement>();
        //    list.AddRange(currentNpcMonoBehaviour.mandatoryQuestRequirements);
        //    list.AddRange(currentNpcMonoBehaviour.optionalQuestRequirements);


        //    //有无药水质量需求和额外效果需求。
        //    bool hasQualityRequirement = false;
        //    bool hasAdditionalEffectsRequirement = false;

        //    // 检查需求
        //    foreach (var requirement in list)
        //    {
        //        if (requirement.requirementInQuest.requirement is QuestRequirementPotionQuality)
        //        {
        //            hasQualityRequirement = (requirement.requirementInQuest.requirement as QuestRequirementPotionQuality).targetTier == 3;
        //        }
        //        else if (requirement.requirementInQuest.requirement is QuestRequirementsAdditionalEffects)
        //        {
        //            hasAdditionalEffectsRequirement = true;
        //        }
        //    }

        //    List<float> additionEffectPriceMultiplier = new List<float> { 1f, 1.25f, 1.5f, 2.5f, 5f };
        //    List<float> qualityMultiplier = new List<float> { 0.4f, 0.7f, 1f };

        //    //最终得到的最高价值
        //    float maxValue = 0f;




        //处理有1个期望效果时

        //只有两种情况，假设效果价值为100
        // 100*0.4*5 弱效+4种额外效果
        // 100 * 1 * 1.5 强效+2种额外效果
        //结论是有额外效果的时候直接弱效

        //var effect = desiredEffectList[0];
        //float nowAdditionEffectPriceMultiplie= additionEffectPriceMultiplier[0];
        //if (hasAdditionalEffectsRequirement) { 
        //var allEffects = PotionEffect.allPotionEffects.ToList();
        ////allEffects.Sort((a, b) => b.price.CompareTo(a.price));
        //var lastEffect = SelectCompatibleEffects(allEffects, effect);//就一个期望效果，所以调用一次。


        //var endEffect = SelectCompatibleAndEachEffects(lastEffect, qualityRequirement); //最后得到期望效果以外的部分的所有互相兼容的可用的额外效果(无需排好序的) 4种

        //nowAdditionEffectPriceMultiplie = additionEffectPriceMultiplier[endEffect.Count - Mathf.Max(qualityRequirement, 1)];
        //}


        //float totalPrice = 0f;
        //int tier = Mathf.Max(qualityRequirement, 1);
        //float effectPrice = Settings<PotionManagerSettings>.Asset.effectTierPriceMultiplier[tier - 1] * effect.price * nowAdditionEffectPriceMultiplie;  // 使用药水等级效果乘数计算价格
        //totalPrice += effectPrice;



        //int tier = hasQualityRequirement ? 2 : 0;
        //float totalPrice = 0f;
        //var effect = desiredEffectList[0];
        //float effectPrice = qualityMultiplier[tier] * effect.price;  // 使用药水等级效果乘数计算价格
        //if (hasAdditionalEffectsRequirement) effectPrice *= additionEffectPriceMultiplier[4-tier];
        //totalPrice += effectPrice;


        //maxValue = totalPrice;

        //处理有2个期望效果时

        //只有两种情况，假设效果价值为2000、100
        // 2000*0.4*5 +100*0.4*5弱效+3种额外效果 4000+200
        // 2000* 0.7 * 2.5 +100*0.4*2.5 强效+2种额外效果  3500+175      2000* 0.7 * 1.5 +100*0.7*1.5 强效+1种额外效果

        // 2000* 1 * 1.5强效 + 100*0.4*1.5 + 1种额外效果  100*1.5+100*0.6=150+60=210   214.5  360
        // 2000* 1 * 1.25 + 100*0.7*1.25强效           100*1.25+ 100*0.875= 125+87.5= 212.5  究极极端(比如前后两个效果价格相等)的情况下3+2的组合可能比311的组合高出 0.025的价格，但这要求 216.5 337.5
        //结论是有额外效果的时候直接弱效  不管怎么样后两个系数相乘都不如一开始的弱效大  当有强效的时候就变出两种情况
        //            A1.5 + B0.6 = x
        //A1.25 + B0.875 = y 某些极端(比如A = B)的情况下3 + 2的组合可能比3 + 1 + 1的组合高出 0.025的价格。
        //A1.25 + B0.875 = y 某些极端(比如A = B)的情况下3 + 2的组合可能比3 + 1 + 1的组合高出 0.025的价格。   
        //临界点 A >= 1.1B的时候 x会比y大 正常来说大部分药剂都能大于这个比例，但还是存在某些药剂，比如爆炸跟手巧，小于这个临界点的。还有的比如石肤跟睡眠，就是相等的


        //int tier = hasQualityRequirement ? 2 : 0;
        //float totalPrice = 0f;
        //float effectPrice = 0f;
        //var effect1 = desiredEffectList[0];
        //var effect2 = desiredEffectList[1];
        //if (effect1.price >= effect2.price * 1.1f && hasAdditionalEffectsRequirement)
        //{//使用 3+1（+1)
        //    effectPrice = qualityMultiplier[tier] * effect1.price;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= additionEffectPriceMultiplier[4 - tier];
        //    totalPrice += effectPrice;

        //    effectPrice = qualityMultiplier[0] * effect2.price;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= additionEffectPriceMultiplier[4 - tier];
        //    totalPrice += effectPrice;
        //}
        //else
        //{//使用 3+2

        //    effectPrice = qualityMultiplier[tier] * effect1.price * 1.25f;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= 4;//5=1.25*4
        //    totalPrice += effectPrice;

        //    effectPrice = qualityMultiplier[0] * effect2.price * 1.25f;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= 4;
        //    totalPrice += effectPrice;
        //}




        //处理有3个期望效果时
        //int tier = hasQualityRequirement ? 2 : 0;
        //float totalPrice = 0f;
        //float effectPrice = 0f;
        //var effect1 = desiredEffectList[0];
        //var effect2 = desiredEffectList[1];
        //var effect3 = desiredEffectList[2];
        //if (effect1.price >= effect2.price * 1.1f && hasAdditionalEffectsRequirement)
        //{//使用 3+1（+1)
        //    effectPrice = qualityMultiplier[tier] * effect1.price;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= additionEffectPriceMultiplier[4 - tier];
        //    totalPrice += effectPrice;

        //    effectPrice = qualityMultiplier[0] * effect2.price;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= additionEffectPriceMultiplier[4 - tier];
        //    totalPrice += effectPrice;
        //}
        //else
        //{//使用 3+2

        //    effectPrice = qualityMultiplier[tier] * effect1.price * 1.25f;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= 4;//5=1.25*4
        //    totalPrice += effectPrice;

        //    effectPrice = qualityMultiplier[0] * effect2.price * 1.25f;  // 使用药水等级效果乘数计算价格
        //    if (hasAdditionalEffectsRequirement) effectPrice *= 4;
        //    totalPrice += effectPrice;
        //}









        //处理有4个期望效果时
        //处理有5个期望效果时



        //return Mathf.FloorToInt(totalPrice);



        //List<PotionEffect> additionalEffects = new List<PotionEffect>();
        //List<PotionEffect> availableEffects = PotionEffect.allPotionEffects.ToList();
        //availableEffects.Sort((a, b) => b.price.CompareTo(a.price));






        //if (hasAdditionalEffectsRequirement)
        //{
        //    int maxAdditionalEffects = 5 - Math.Max(qualityRequirement, 1); //最大额外效果数量 最大是4 最小是2
        //    PotionEffect lastEffect = potionEffect;

        //    for (int i = 0; i < maxAdditionalEffects; i++)
        //    {
        //        availableEffects.Remove(lastEffect);
        //        // 筛选兼容的效果
        //        List<PotionEffect> compatibleEffects = new List<PotionEffect>();
        //        foreach (var effect in availableEffects)
        //        {
        //            if (PotionEffect.IsEffectsCompatible(lastEffect, effect))
        //            {
        //                compatibleEffects.Add(effect);
        //            }
        //        }

        //        if (compatibleEffects.Count == 0)
        //        {
        //            break;
        //        }

        //        lastEffect = compatibleEffects[0];
        //        additionalEffects.Add(lastEffect);

        //        // 更新可用效果列表
        //        availableEffects = compatibleEffects;
        //    }
        //}




        //float totalPrice = 0f;
        //int maxTier = 0;
        //foreach (PotionEffect effect in effectCounts.Keys)
        //{
        //    if (desiredEffects.Contains(effect))
        //    {
        //        int tier = effectCounts[effect];
        //        maxTier = Mathf.Max(maxTier, tier);
        //        // 使用效果等级价格乘数计算价格
        //        float effectPrice = Settings<PotionManagerSettings>.Asset.effectTierPriceMultiplier[tier - 1] * effect.price;
        //        totalPrice += effectPrice;
        //    }
        //}
        //return Mathf.FloorToInt(totalPrice);
        //}









        //public static List<PotionEffect> SelectCompatibleEffects(List<PotionEffect> lastAvailableEffects,PotionEffect potionEffect) {

        //    //LoggerWrapper.LogInfo($"======");

        //    //根据目前的药水效果,筛选上一次后还可兼容的药水效果，多次调用最终得到兼容所有期望效果的额外效果。lastAvailableEffects一开始应该是PotionEffect.allPotionEffects.ToList(); 且排序好的
        //    List<PotionEffect> compatibleEffects = new List<PotionEffect>();
        //    foreach (var effect in lastAvailableEffects)
        //    {
        //        if (effect != potionEffect && PotionEffect.IsEffectsCompatible(potionEffect, effect))
        //        {
        //            compatibleEffects.Add(effect);
        //            //LoggerWrapper.LogInfo($"兼容的效果池：当前：{potionEffect.name} 兼容：{effect.name}");
        //        }
        //    }
        //    //LoggerWrapper.LogInfo($"======");
        //    return compatibleEffects;
        //}

        //public static List<PotionEffect> SelectCompatibleAndEachEffects(List<PotionEffect> endAvailableEffects, int qualityRequirement)
        //{//根据目前的药水效果，得到前maxAdditionalEffects个最高价值且互相兼容的药水效果
        //    endAvailableEffects.Sort((a, b) => b.price.CompareTo(a.price));
        //    List<PotionEffect> additionalEffects = new List<PotionEffect>();
        //    int maxAdditionalEffects = 5 - Math.Max(qualityRequirement, 1); //最大额外效果数量 最大是4 最小是2
        //    PotionEffect lastEffect = endAvailableEffects[0];

        //    for (int i = 0; i < maxAdditionalEffects; i++)
        //    {
        //        endAvailableEffects.Remove(lastEffect);
        //        // 筛选兼容的效果
        //        List<PotionEffect> compatibleEffects = new List<PotionEffect>();
        //        foreach (var effect in endAvailableEffects)
        //        {
        //            if (PotionEffect.IsEffectsCompatible(lastEffect, effect))
        //            {
        //                compatibleEffects.Add(effect);
        //            }
        //        }

        //        if (compatibleEffects.Count == 0)//如果一开始就找不到任何可以兼容的药水，直接终止循环
        //        {
        //            break;
        //        }

        //        lastEffect = compatibleEffects[0];
        //        additionalEffects.Add(lastEffect);

        //        // 更新可用效果列表
        //        endAvailableEffects = compatibleEffects;
        //    }
        //    //additionalEffects.Sort((a, b) => b.price.CompareTo(a.price));
        //    return additionalEffects;
        //}



    }

}