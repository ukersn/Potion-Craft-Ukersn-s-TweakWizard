using HarmonyLib;
using PotionCraft.InventorySystem;
using PotionCraft.ManagersSystem.Player;
using PotionCraft.ManagersSystem.Room;
using PotionCraft.ObjectBased;
using PotionCraft.ObjectBased.Garden;
using PotionCraft.ObjectBased.Potion;
using PotionCraft.ObjectBased.Stack;
using PotionCraft.ScriptableObjects.BuildableInventoryItem;
using PotionCraft.ScriptableObjects.BuildZone;
using PotionCraft.ScriptableObjects.Ingredient;
using PotionCraft.ScriptableObjects.WateringPot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using Stack = PotionCraft.ObjectBased.Stack.Stack;

namespace PotionCraftAutoGarden.Utilities
{
    public class GameObjectHelper
    {
        private static List<GameObject> lastUnActivateSeedColliders = new List<GameObject>();
        private static List<GameObject> lastUnActivateItems = new List<GameObject>();
        private static List<GameObject> gardenRoomSeedColliders;
        //private static List<GameObject> allRooms;
        public static GameObject[] GetVisibleSeeds()
        {
            Camera mainCamera = Camera.main; // 获取主摄像机
            if (mainCamera == null)
            {
                LoggerWrapper.LogInfo("Main camera not found");
                return null;
            }
            GameObject[] seeds = GetAllSeeds();
            // 创建一个列表来存储找到的植物的对象
            List<GameObject> visiableSeeds = new List<GameObject>();

            foreach (GameObject seed in seeds)
            {
                if (seed == null)
                {
                    continue;
                }
                // 使用路径格式直接查找 ItemSprite
                Transform itemSpriteTransform = seed.transform.Find("Default GrowingSpot VisualObject/Visual Object/Backround/ItemSprite");
                if (itemSpriteTransform == null)
                {
                    LoggerWrapper.LogError(string.Format("Could not find ItemSprite for {0}", seed.name));
                    return null;
                }
                // 获取 SpriteRenderer 组件
                SpriteRenderer spriteRenderer = itemSpriteTransform.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    LoggerWrapper.LogError("Could not find SpriteRenderer component on ItemSprite");
                    return null;
                }
                // 检查 SpriteRenderer 是否在相机视野内
                if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(mainCamera), spriteRenderer.bounds))
                {
                    visiableSeeds.Add(seed.gameObject);
                }
            }

            return visiableSeeds.ToArray();

        }
        public static GameObject[] GetAllSeeds()
        { //获得所有被种植的种子物体(比如水晶、植物)
            // 查找名为 "ItemContainer" 的对象
            GameObject itemContainer = GameObject.Find("ItemContainer");
            if (itemContainer == null)
            {
                LoggerWrapper.LogInfo("ItemContainer not found in the scene.");
                return new GameObject[0];
            }
            // 创建一个列表来存储找到的植物的对象
            List<GameObject> seeds = new List<GameObject>();

            // 遍历 ItemContainer 的所有子对象
            foreach (Transform child in itemContainer.transform)
            {
                // 检查子物体是否有 GrowingSpotController 组件且名称以 "Seed" 结尾
                if (child.GetComponent<GrowingSpotController>() != null && child.name.EndsWith("Seed"))
                {
                    seeds.Add(child.gameObject);
                }
            }
            return seeds.ToArray();
        }


        public static Inventory GetPlayInventory()
        {
            GameObject managers = GameObject.Find("Managers");
            if (managers == null)
            {
                LoggerWrapper.LogInfo("Managers not found in the scene.");
                return null;
            }
            PlayerManager playerManager = managers.GetComponent<PlayerManager>();
            if (playerManager == null)
            {
                LoggerWrapper.LogInfo("PlayerManager not found in the scene.");
                return null;
            }
            return playerManager.Inventory;
        }

        public static void GetGardenRoomSeedColliders()
        {
            GameObject managers = GameObject.Find("Managers");
            RoomManager roomManager = managers.GetComponent<RoomManager>();
            string roomName = null;
            switch (roomManager.targetRoom)
            {
                case RoomIndex.Garden:
                    roomName = "Room Garden(Clone)";
                    break;
                case RoomIndex.Pond:
                    roomName = "Room Pond(Clone)";
                    break;
                case RoomIndex.Cave:
                    roomName = "Room Cave(Clone)";
                    break;
                case RoomIndex.Grotto:
                    roomName = "Room Grotto(Clone)";
                    break;
            }
            GameObject garden = GameObject.Find(roomName);
            if (garden == null)
            {
                return;
            }
            List<GameObject> buildZoneActualColliders = new List<GameObject>();
            foreach (Transform child in garden.transform)
            {
                if (child.name.StartsWith("BuildZoneActualCollider"))
                {
                    buildZoneActualColliders.Add(child.gameObject);
                }
            }

            //LoggerWrapper.LogInfo($"buildZoneActualColliders:{buildZoneActualColliders.Count()}");
            List<GameObject> seedColliders = new List<GameObject>();
            foreach (GameObject buildZoneActualCollider in buildZoneActualColliders)
            {
                List<GameObject> childColliderAddedBuildableItems = new List<GameObject>();
                foreach (Transform child in buildZoneActualCollider.transform)
                {
                    if (child.name.StartsWith("Child Collider AddedBuildableItem"))
                    {
                        childColliderAddedBuildableItems.Add(child.gameObject);
                    }
                }
                //LoggerWrapper.LogInfo($"childColliderAddedBuildableItems:{childColliderAddedBuildableItems.Count()}");
                seedColliders.AddRange(childColliderAddedBuildableItems);
                buildZoneActualCollider.GetComponent<BuildZoneRoomActualCollider>().UpdateCompositeCollider(); //更新种植区域
            }


            gardenRoomSeedColliders = seedColliders;
        }

        public static void SetBuildZoneColliders(bool activate)
        {
            //LoggerWrapper.LogInfo($"lastUnActivateSeedColliders 数量 = {lastUnActivateSeedColliders.Count()}");
            foreach (GameObject lastUnActivateSeedCollider in lastUnActivateSeedColliders)
            {
                if (lastUnActivateSeedCollider != null)
                {
                    lastUnActivateSeedCollider.SetActive(true);
                }
            }
            lastUnActivateSeedColliders.Clear();

            if (gardenRoomSeedColliders == null) return;
            if (!activate)
            {
                //LoggerWrapper.LogInfo($"seedColliders 数量 = {gardenRoomSeedColliders.Count()} ");
                foreach (GameObject seedCollider in gardenRoomSeedColliders)
                {
                    if (seedCollider != null)
                    {
                        seedCollider.SetActive(false);
                        lastUnActivateSeedColliders.Add(seedCollider);
                    }
                }
            }
        }




        private static RoomIndex?[,] roomLayout = new RoomIndex?[,] //游戏房间投影
        {
        { null  , RoomIndex.Bedroom , null,  null    },
        { RoomIndex.Meeting, RoomIndex.Laboratory, RoomIndex.Garden, RoomIndex.Pond },
        { RoomIndex.Treasury, RoomIndex.Basement, RoomIndex.Cave, RoomIndex.Grotto }
        };

        private static int rows = 3;
        private static int cols = 4;


        // 获取所有存在的房间的坐标和类型
        private static List<(int x, int y, RoomIndex type)> GetExistingRooms()
        {
            List<(int, int, RoomIndex)> rooms = new List<(int, int, RoomIndex)>();
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (roomLayout[y, x].HasValue)
                    {
                        rooms.Add((x, y, roomLayout[y, x].Value));
                    }
                }
            }
            return rooms;
        }

        // 检查两个房间是否相邻
        private static bool AreRoomsAdjacent((int x, int y) room1, (int x, int y) room2)
        {
            return Math.Abs(room1.x - room2.x) + Math.Abs(room1.y - room2.y) == 1;
        }

        // 获取所有不相邻的房间对
        public static List<(RoomIndex, RoomIndex)> GetNonAdjacentRooms()
        {
            List<(int x, int y, RoomIndex type)> rooms = GetExistingRooms();
            List<(RoomIndex, RoomIndex)> nonAdjacentPairs = new List<(RoomIndex, RoomIndex)>();

            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    if (!AreRoomsAdjacent((rooms[i].x, rooms[i].y), (rooms[j].x, rooms[j].y)))
                    {
                        nonAdjacentPairs.Add((rooms[i].type, rooms[j].type));
                    }
                }
            }

            return nonAdjacentPairs;
        }

        // 获取与当前房间不相邻的房间
        public static List<RoomIndex> GetNonAdjacentRoomsToCurrentRoom(RoomIndex currentRoom)
        {
            List<(int x, int y, RoomIndex type)> rooms = GetExistingRooms();
            List<RoomIndex> nonAdjacentRooms = new List<RoomIndex>();

            (int currentX, int currentY) = (-1, -1);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (roomLayout[y, x] == currentRoom)
                    {
                        currentX = x;
                        currentY = y;
                        break;
                    }
                }
                if (currentX != -1) break;
            }

            if (currentX == -1) return nonAdjacentRooms; // Current room not found
            //LoggerWrapper.LogInfo($"计算不相邻房间 ");
            foreach (var room in rooms)
            {
                if (room.type != currentRoom && !AreRoomsAdjacent((currentX, currentY), (room.x, room.y)))
                {
                    nonAdjacentRooms.Add(room.type);
                    //LoggerWrapper.LogInfo($"这些房间不相邻 {room.type} ");
                }
            }

            return nonAdjacentRooms;
        }


        //public static void GetAllRooms() {

        //    List<GameObject> roomObjects = new List<GameObject>();

        //    // 获取场景中的所有根对象
        //    List<GameObject> rootObjects = new List<GameObject>();
        //    UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects(rootObjects);
        //    // 遍历所有根对象
        //    foreach (GameObject obj in rootObjects)
        //    {
        //        // 检查对象名称是否符合条件
        //        if (obj.name.StartsWith("Room") && obj.name.EndsWith("(Clone)"))
        //        {
        //            roomObjects.Add(obj);
        //        }
        //    }
        //    allRooms = roomObjects;
        //}
        public static List<(GameObject item, RoomIndex roomIndex)> GetAllItemsInRooms()
        {
            GameObject itemContainer = GameObject.Find("ItemContainer");
            List<(GameObject item, RoomIndex roomIndex)> items = new List<(GameObject, RoomIndex)>();

            if (itemContainer == null)
            {
                Debug.LogWarning("ItemContainer not found.");
                return items;
            }

            foreach (Transform child in itemContainer.transform)
            {
                if (child == null) continue;

                BuildableItemFromInventory buildableItemFromInventory = child.GetComponent<BuildableItemFromInventory>();
                GrowingSpotController growingSpotController = child.GetComponent<GrowingSpotController>();
                if (buildableItemFromInventory != null && growingSpotController != null)
                {
                    if (buildableItemFromInventory.currentRoomBuildZoneSet != null &&
                        buildableItemFromInventory.currentRoomBuildZoneSet.room != null)
                    {
                        items.Add((child.gameObject, buildableItemFromInventory.currentRoomBuildZoneSet.room.roomIndex));
                    }
                    continue;
                }

                PotionItem potionItem = child.GetComponent<PotionItem>();
                if (potionItem != null)
                {
                    if (potionItem.LedgeAttachedTo != null)
                    {
                        items.Add((child.gameObject, potionItem.LedgeAttachedTo.buildableItem.currentRoomBuildZoneSet.room.roomIndex));
                    }
                    continue;
                }
            }

            return items;
        }
        // 设置不相邻房间为非活跃状态
        public static void SetAdjacentRoomsActiveAndOthersInactive(RoomIndex currentRoom, bool extremeMode)
        {
            //extremeMode为是否开启极端模式，为false时会保留相邻房间的物品，约5%的性能提升
            //为true时只保留本房间的物品,约9%的性能提升(但在切换其他房间的时候，你会看见切换前房间的物品好像"隐身"了)
            foreach (GameObject lastUnActivateItem in lastUnActivateItems)
            {
                if (lastUnActivateItem != null)
                {
                    lastUnActivateItem.SetActive(true);
                }
            }
            lastUnActivateItems.Clear();
            //GetAllRooms();
            List<(GameObject item, RoomIndex roomIndex)> items = GetAllItemsInRooms();
            if (!extremeMode)
            {
                var nonAdjacentRooms = GetNonAdjacentRoomsToCurrentRoom(currentRoom);
                foreach ((GameObject item, RoomIndex roomIndex) item in items)
                {
                    if (item.item != null)
                    {
                        if (nonAdjacentRooms.Contains(item.roomIndex))
                        {
                            item.item.SetActive(false);
                            lastUnActivateItems.Add(item.item);
                        }
                        else
                        {
                            item.item.SetActive(true);
                        }
                    }
                }
            }
            else {

                foreach ((GameObject item, RoomIndex roomIndex) item in items)
                {
                    if (item.item != null)
                    {
                        if (item.roomIndex != currentRoom)
                        {
                            item.item.SetActive(false);
                            lastUnActivateItems.Add(item.item);
                        }
                        else
                        {
                            item.item.SetActive(true);
                        }
                    }
                }
            }
        }



        public static bool IsStackCrystal(Stack stack)
        {
            if (stack == null || stack.gameObject == null) return false;

            foreach (Transform child in stack.gameObject.transform)
            {
                // 检查子物体是否有 GrowingSpotController 组件且名称以 "Seed" 结尾

                if (child.name.EndsWith("01"))
                {
                    if (child.transform.childCount == 5) {
                        return true;
                    }
                }
            }
            // 如果没有找到符合条件的子对象，返回 false
            return false;
        }


        public static void StopAllParticleSystems(bool disableParticleEffects, bool disableScratchesEffects)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            int wateringPotLayer = LayerMask.NameToLayer("WateringPot");
            foreach (GameObject obj in allObjects)
            {
                if (disableParticleEffects) {
                    ParticleSystem[] particleSystems = obj.GetComponents<ParticleSystem>();

                    foreach (ParticleSystem ps in particleSystems)
                    {
                        if (obj.layer != wateringPotLayer) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                        //LoggerWrapper.LogInfo($"Stopped ParticleSystem on {obj.name}");
                    }

                }
                if (disableScratchesEffects) {
                    // 检查并禁用 Sprite Scraches 的渲染
                    if (obj.name == "Sprite Scraches" || obj.name == "Sprite Scratches" || obj.name == "Scratches")
                    {
                        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.enabled = false;
                            //LoggerWrapper.LogInfo($"Disabled SpriteRenderer on {obj.name}");
                        }
                    }
                }

            }
            //LoggerWrapper.LogInfo("All ParticleSystems have been stopped.");
        }

        public static void StopGameObjectParticleSystems(Transform transform) {
            if (transform == null)
            {
                return;
            }
            foreach (Transform child in transform)
            {
                ParticleSystem[] particleSystems = child.GetComponents<ParticleSystem>();
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps.isPlaying)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                }
            }
        }








        //public static void StopAllParticleSystemsAndDisableSpriteScraches()
        //{
        //    GameObject[] rootObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        //    int particleSystemsStopped = 0;
        //    int spriteScrachesDisabled = 0;

        //    foreach (GameObject rootObj in rootObjects)
        //    {
        //        ProcessGameObjectAndChildren(rootObj, ref particleSystemsStopped, ref spriteScrachesDisabled);
        //    }

        //    LoggerWrapper.LogInfo($"Stopped {particleSystemsStopped} ParticleSystems.");
        //    LoggerWrapper.LogInfo($"Disabled {spriteScrachesDisabled} Sprite Scraches renderers.");
        //}

        private static void ProcessGameObjectAndChildren(GameObject obj, ref int particleSystemsStopped, ref int spriteScrachesDisabled)
        {
            // 处理粒子系统
            ParticleSystem[] particleSystems = obj.GetComponents<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                particleSystemsStopped++;
                LoggerWrapper.LogInfo($"Stopped ParticleSystem on {obj.name}");
            }

            // 检查并禁用 Sprite Scraches 的渲染
            if (obj.name == "Sprite Scraches")
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.enabled)
                {
                    spriteRenderer.enabled = false;
                    spriteScrachesDisabled++;
                    LoggerWrapper.LogInfo($"Disabled SpriteRenderer on {GetFullPath(obj)}");
                }
            }

            // 递归处理所有子对象
            foreach (Transform child in obj.transform)
            {
                ProcessGameObjectAndChildren(child.gameObject, ref particleSystemsStopped, ref spriteScrachesDisabled);
            }
        }

        private static string GetFullPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }


        //药水旋涡贴边
        private static CircleCollider2D indicatorCollider;


        public static CircleCollider2D GetIndicatorCollider()
        {
            if (indicatorCollider == null) UpdateIndicatorCollider();
            return indicatorCollider;
        }
        public static void UpdateIndicatorCollider()
        {
            indicatorCollider = GameObject.Find("Room Laboratory(Clone)/RecipeMap In Room/RecipeMapObject/Map Container/IndicatorMetaContainer/IndicatorContainer/Indicator/Collider")?.
            GetComponent<CircleCollider2D>();
        }

    }
}
