using PotionCraft.LocalizationSystem;
using System.Collections.Generic;

namespace PotionCraftAutoGarden.Utilities
{
    public class LocalizationWrapper
    {
        private static bool _isInitialized = false;

        public static void Init()
        {
            if (!_isInitialized)
            {
                LocalizationManager.OnInitialize.AddListener(SetModLocalization);
                _isInitialized = true;
            }
        }

        public static void SetModLocalization()
        {
            //注册的时候不要以#开头，是错的，会导致PotionCraft.LocalizationSystem.Key实例化失败，因为其在构造器中尝试删除#后以删除后的字符串去池子里找名称为删除前字符串的内容。

            //平台后缀 this.platformPostfix =_steam  不知道有什么用就是了  PotionCraft.LocalizationSystem.Key有关的

            //RegisterLoc("#mod_autogarden_value", "Value", "价值"); 
            //RegisterLoc("#mod_autogarden_insufficient_fertilizer_potion", "<color=red>Insufficient potion for fertilization</color>", "<color=red>用于施肥的药水不足</color>");
            //RegisterLoc("#mod_autogarden_auto_harvest_watering_complete", "Auto-harvest and watering completed", "自动收获浇水完成");
            //RegisterLoc("#mod_autogarden_fertilization_completed", "Auto-fertilization completed in this area", "本区域的自动施肥完成");
            //RegisterLoc("#mod_autogarden_no_operable_crops", "No operable crops in this area", "本区域没有可以操作的作物");
            RegisterLoc("mod_ukersn_s_tweakwizard_quick_potion_pick", "Quick Potion Pick", "快速选药");
            RegisterLoc("mod_ukersn_s_tweakwizard_quick_potion_pick_theoretical_max_value", "Quick Potion Pick (Max Offer: [0])", "快速选药（最高报价: [0]）");
            RegisterLoc("mod_ukersn_s_tweakwizard_no_suitable_potion", "No suitable potion in inventory", "背包中没有符合条件的药水");
            //RegisterLoc("mod_ukersn_s_tweakwizard_previous_potion_pick", "Previous", "上一瓶");
            //RegisterLoc("mod_ukersn_s_tweakwizard_next_potion_pick", "Next", "下一瓶");
            //RegisterLoc("#find_potion", "Auto Find Potion", "自动寻找药水");
            //LoggerWrapper.LogInfo("插件语言本地化注册完成");
            //RegisterLoc("#mod_autogarden_cost", "Cost", "成本");
            //RegisterLoc("#mod_autogarden_has", "Has", "已拥有");
            //RegisterLoc("#mod_autogarden_nothas", "<color=red>Items not owned, recommended</color>", "<color=red>未拥有，建议购入</color>");




            //通过下面的方式来在文本中插入变量内容
            //方法1
            //RegisterLoc("#mod_ukersn_s_tweakwizard_auto_find_potion", "Auto Find Potion {0}", "自动寻找药水{0}");
            //调用
            //String.Format(PotionCraft.LocalizationSystem.LocalizationManager.GetText("#mod_ukersn_s_tweakwizard_auto_find_potion"), "变量内容");


            //方法2
            //RegisterLoc("mod_ukersn_s_tweakwizard_auto_find_potion", "Auto Find Potion[0] [1]", "自动寻找药水[0] [1]");  使用[0]作为插入的占位符
            //使用new List<string> { "qwq" ,"owo"}添加参数
            //new PotionCraft.LocalizationSystem.Key("#mod_ukersn_s_tweakwizard_auto_find_potion", new List<string> { "qwq" ,"owo"}, PotionCraft.LocalizationSystem.KeyParametersStyle.Normal, null, null);



        }
        //string localizedText = LocalizationManager.GetText("#your_mod_key_1");


        public static void RegisterLoc(string key, string en, string zh)
        {
            for (int i = 0; i <= (int)LocalizationManager.Locale.cs; i++)
            {
                if ((LocalizationManager.Locale)i == LocalizationManager.Locale.zh)
                {
                    LocalizationManager.localizationData.Add(i, key, zh);
                }
                else
                {
                    LocalizationManager.localizationData.Add(i, key, en);
                }
            }
        }


        public static Key ToKey(string localizationKey)
        {
            // 确保键以#开头
            if (!localizationKey.StartsWith("#"))
            {
                localizationKey = "#" + localizationKey;
            }

            // 创建并返回Key对象
            return new Key(localizationKey, null, KeyParametersStyle.Normal, null, System.Array.Empty<KeyTagParameter>());
        }

        public static Key ToKey(string localizationKey, List<string> parameters)
        {
            // 确保键以#开头
            if (!localizationKey.StartsWith("#"))
            {
                localizationKey = "#" + localizationKey;
            }

            // 创建并返回Key对象，包含参数
            return new Key(localizationKey, parameters, KeyParametersStyle.Normal, null, System.Array.Empty<KeyTagParameter>());
        }
    }

}
