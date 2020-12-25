using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_ys_helper
{
    class Data
    {
        public static string VERSION = "2.2.1";
        public static string[] SywSecondAttrName = { "数值生命值",
                "数值攻击力",
                "数值防御力",
                "元素精通",
                "元素充能%",
                "百分比防御力%",
                "百分比生命值%",
                "百分比攻击力%",
                "暴击率%",
                "暴击伤害%"
        };
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
        };

        //这里写你的cookie，例如：account_id=xxxxx; cookie_token=xxxxxxxxxxxxxxxxxxxx，
        //或者从textbox里输入
        public static string COOKIE = "";


    }
}
