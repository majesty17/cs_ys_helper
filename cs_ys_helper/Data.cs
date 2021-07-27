using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_ys_helper
{
    class Data
    {
        public static string VERSION = "2.7.0";
        public static string SALT = "14bmu1mz0yuljprsfgpvjh3ju2ni468r";
        public static string CLIENT_TYPE = "5";
        public static string[] SywSecondAttrName = {
            "数值生命值",
            "数值攻击力",
            "数值防御力",
            "元素精通",
            "元素充能%",
            "百分比防御力%",
            "百分比生命值%",
            "百分比攻击力%",
            "暴击率%",
            "暴击伤害%"
        };/*
        public static float[,] SywSecondAttrValue = {
            { 209.0f, 239.0f, 269.0f, 299.0f, 1495.0f },
            {14.0f,16.0f,18f,19f,95f },
            {16.0f,19f,21f,23f,115f },
            {16.0f,19f,21f,23f,115f },
            {4.5f,5.2f,5.8f,6.5f,32.5f },
            {5.1f,5.8f,6.6f,7.3f,36.5f },
            {4.1f,4.7f,5.3f,5.8f,29f },
            {4.1f,4.7f,5.3f,5.8f,29f },
            {2.7f,3.1f,3.5f,3.9f,19.5f },
            {5.4f,6.2f,7.0f,7.8f,39f }
        };*/
        public static float[] SywSecondAttrValue = {
            4780f,
            311f,
            370f,
            187f,
            51.8f,
            58.3f,
            46.6f,
            46.6f,
            31.1f,
            62.2f
        };


        public static double rate_5 = 0.006d;
        public static double rate_4 = 0.051d;

        //角色池
        /// <summary>
        /// 5 0.6%
        /// 4 5.1%
        /// 3 94.3%
        /// 
        /// </summary>
        public static string[][] wish_pool = {
            new string[]{"弹弓_武器","神射手之誓言_武器","鸦羽弓_武器","翡玉发球_武器","讨龙英杰谭_武器","魔导绪论_武器","黑缨枪_武器",
                "以理服人_武器","沐浴龙血的剑_武器","铁影阔剑_武器","飞天御剑_武器","黎明神剑_武器","冷刃_武器" },
            new string[]{ "辛焱_角色","砂糖_角色","迪奥娜_角色","重云_角色","诺艾尔_角色","班尼特_角色","菲谢尔_角色","凝光_角色",
                "行秋_角色","北斗_角色","香菱_角色","安柏_角色","雷泽_角色","凯亚_角色","芭芭拉_角色","丽莎_角色",
                "弓藏_武器","祭礼弓_武器","绝弦_武器","西风猎弓_武器","昭心_武器","祭礼残章_武器","流浪乐章_武器","西风秘典_武器","西风长枪_武器",
                "匣里灭辰_武器","雨裁_武器","祭礼大剑_武器","钟剑_武器","西风大剑_武器","匣里龙吟_武器","祭礼剑_武器","笛剑_武器","西风剑_武器"},
            new string[]{"刻晴_角色","莫娜_角色","七七_角色","迪卢克_角色","琴_角色","阿莫斯之弓_武器",
                "天空之翼_武器","四风原典_武器","天空之卷_武器","和璞鸢_武器","天空之脊_武器",
                "狼的末路_武器","天空之傲_武器","天空之刃_武器","风鹰剑_武器" }

        };

        //这里写你的cookie，例如：account_id=xxxxx; cookie_token=xxxxxxxxxxxxxxxxxxxx，
        //这里废弃了，从textbox里用户输入


        public static string cookieFile = ".cookie";
    }
}
