using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace cs_ys_helper
{
    class SywScore
    {
        public SywScore()
        {

        }
        //根据附词条和值，推测up次数和档位；
        public static string getScore(string[] name,double[] value)
        {
            string ret = "";
            int score = 0;
            for(int i = 0; i < 4; i++)
            {

                int idx = Data.SywSecondAttrName.ToList().IndexOf(name[i]);
                double times = idx < 3 ? 16d : 8d;

                double slice = value[i] / (Data.SywSecondAttrValue[idx] / times / 10d);


                ret += string.Format("{0,-14}\t档位数:{1:f4}\t档位数取整:{2}\n", name[i], slice, Math.Round(slice));
                score += (int)Math.Round(slice);
            }

            ret += string.Format("总档位\t{0}\n", score);
            ret += string.Format("评分\t{0:f2}\t(百分制0~100)\n", (score - 56) / (90f - 56f) * 100f);
            ret += "评分依据:\n最差:最低档7*(3初始+5次突破)=56档\n最好:最高档10*(4初始+5次突破)=90档\n";
            ret += "参考算法: https://wiki.biligame.com/ys/圣遗物属性\n";


            return ret;
        }
    }
}
