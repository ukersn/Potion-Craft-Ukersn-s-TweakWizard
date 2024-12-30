using BepInEx;
using PotionCraft.ScriptableObjects.Ingredient;
using UnityEngine;

namespace Ukersn_s_TweakWizard
{
    public class Abandoned : BaseUnityPlugin
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





    }

}