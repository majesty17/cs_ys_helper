using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace cs_ys_helper
{
    class WishSimu
    {
        /// <summary>
        /// 
        /// </summary>
        int times = 0;
        int times_1 = 0;
        int times_10 = 0;
        int xinghui_5 = 0;
        int xingchen_4 = 0;
        int star_3 = 0;
        int star_4 = 0;
        int star_5 = 0;
        int bao5_ct = 0;
        int bao4_ct = 0;
        /// <summary>
        /// {"name":"凝光","type":"角色","rarity":5,"count":10}
        /// </summary>
        public JsonData bag;

        int no_5_ct;
        int no_4_ct;
        Random rand;

        public WishSimu()
        {
            times = 0;
            times_1 = 0;
            times_10 = 0;
            xinghui_5 = 0;
            xingchen_4 = 0;
            star_3 = 0;
            star_4 = 0;
            star_5 = 0;

            no_5_ct = 0;
            no_4_ct = 0;
            bao4_ct = 0;
            bao5_ct = 0;
            bag = new JsonData();
            rand = new Random(DateTime.Now.Millisecond);
            

        }

        //随机一个:约定区间左闭右开
        private JsonData random()
        {
            int rarity;
            //概率修正，参考：https://www.bilibili.com/video/BV16i4y1L7Ne
            double rate_5_adjust = no_5_ct >= 72 ? ((1.0 - Data.rate_5) / (89 - 72) *
                (no_5_ct - 72) + Data.rate_5) : Data.rate_5;

            double rate_4_adjust = no_4_ct >= 6 ? ((1.0 - Data.rate_4) / (9 - 6) *
                (no_4_ct - 6) + Data.rate_4) : Data.rate_4;



            if (no_5_ct >= 89) //保5，同时重置
            {
                no_4_ct = 0;
                no_5_ct = 0;
                rarity = 5;
                bao5_ct++;
            }
            else if (no_4_ct >= 9) //仅小保底，从45里随机
            {
                double r = rand.NextDouble();
                if (r < rate_5_adjust) //不是5，就是4
                {
                    no_4_ct = 0;
                    no_5_ct = 0;
                    rarity = 5;
                }
                else
                {
                    no_4_ct = 0;
                    rarity = 4;
                    no_5_ct++;
                }
                bao4_ct++;
            }
            else //没有任何保底，纯概率
            {
                double r = rand.NextDouble();
                if (r < rate_5_adjust)
                {
                    no_4_ct = 0;
                    no_5_ct = 0;
                    rarity = 5;
                }
                else if (r >= rate_5_adjust && r < rate_5_adjust + rate_4_adjust)
                {
                    no_4_ct = 0;
                    rarity = 4;
                    no_5_ct++;
                }
                else
                {
                    rarity = 3;
                    no_5_ct++;
                    no_4_ct++;
                }
            }



            /*

double r = rand.NextDouble();
int rarity = 3;
if (r < Data.rate_4 + Data.rate_5) rarity = 4; //1划分成三个区间
if (r < Data.rate_5) rarity = 5;

//如果中5，则重置90中5保底和10中4保底
if (rarity == 5)
{
    no_5_ct = 0;
    no_4_ct = 0;
}
//如果中4，则重置10中4保底
if (rarity == 4)
{
    no_4_ct = 0;
    no_5_ct++;
}


//10发保底:
if (no_4_ct >= 10 - 1)
{
    rarity = 4;
    no_4_ct = 0;
}
else
{
    no_4_ct++;
}


//90发大保底，如果连续89次都没有5，则第90发必定5
if (no_5_ct >= 90 - 1)
{
    rarity = 5;
    no_5_ct = 0;
    no_4_ct = 0;
}
else
{
    no_5_ct++;
}
//可以看到，当大小保底赶在一起，则取大保底；
*/

            string[] pool = Data.wish_pool[rarity - 3];
            int len = pool.Length;
            int pos = rand.Next() % len;
            string name = pool[pos].Split(new char[] { '_' })[0];
            string type = pool[pos].Split(new char[] { '_' })[1];

            string j_str = "{\"name\":\"" + name + "\",\"type\":\"" + type + "\",\"rarity\":" + rarity + ",\"count\":1}";
            return JsonMapper.ToObject(j_str);
        }




        //加入背包
        private void addToBag(JsonData newone)
        {
            int rarity = (int)newone["rarity"];
            if (rarity == 5) star_5++;
            if (rarity == 4) star_4++;
            if (rarity == 3) star_3++;

            string type = newone["type"].ToString();
            string name = newone["name"].ToString();
            //int count = (int)newone["count"];


            foreach(JsonData item in bag)
            {
                int count = (int)item["count"];
                //1，如果有了，只加数字
                if (item["name"].ToString() == name)
                {
                    //武器的话，数量+1
                    if (type == "武器")
                    {
                        item["count"] = count + 1;
                        if (rarity == 5) xinghui_5 += 10;
                        if (rarity == 4) xinghui_5 += 2;
                        if (rarity == 3) xingchen_4 += 15;
                    }
                    else //人物的话，小于7，加数字，大于等于7加星辉、星辰
                    {
                        if (count < 7)
                        {
                            item["count"] = count + 1;
                            if (rarity == 5) xinghui_5 += 10;
                            if (rarity == 4) xinghui_5 += 2;
                        }
                        else
                        {
                            if (rarity == 5) xinghui_5 += 25;
                            if (rarity == 4) xinghui_5 += 5;
                        }
                    }
                    return; //处理完有的直接跳出
                }
            }
            //2，没有的话 新增进来
            bag.Add(newone);
            if (type == "武器")
            {
                if (rarity == 5) xinghui_5 += 10;
                if (rarity == 4) xinghui_5 += 2;
                if (rarity == 3) xingchen_4 += 15;
            }
            else
            {
                //新角色不给星x
            }

        }


        //单抽
        public JsonData wish1()
        {
            times += 1;
            times_1 += 1;
            JsonData ret = random();
            addToBag(ret);
            return ret;
        }
        //10连,这里不再考虑保底，由random统一考虑
        public JsonData[] wish10()
        {
            times += 10;
            times_10 += 1;

            JsonData[] wishes = new JsonData[10];

            for (int i = 0; i < 10; i++)
            {
                wishes[i] = random();
                addToBag(wishes[i]);
            }

            return wishes;
        }
        //游戏重置
        public void resetGame() {
            times = 0;
            times_1 = 0;
            times_10 = 0;
            xingchen_4 = 0;
            xinghui_5 = 0;
            star_3 = 0;
            star_4 = 0;
            star_5 = 0;
            no_5_ct = 0;
            no_4_ct = 0;
            bao5_ct = 0;
            bao4_ct = 0;
            bag = JsonMapper.ToObject("[]");
        }
        //生成总结
        public string makeSummary()
        {
            string ret = "";


            ret = ret + string.Format("总次数:        {0:G}\n", times);
            ret = ret + string.Format("单抽数:        {0:G}\n", times_1);
            ret = ret + string.Format("十连数:        {0:G}\n", times_10);
            ret = ret + string.Format("花费估计:      {0:G} RMB\n", (times * 16));
                                                     
            ret = ret + string.Format("5星数(率):     {0:G}({1:P4})\n", star_5, (float)star_5 / (float)times );
            ret = ret + string.Format("4星数(率):     {0:G}({1:P4})\n", star_4, (float)star_4 / (float)times );
            ret = ret + string.Format("保底4星数:     {0:G}\n", bao4_ct);
            ret = ret + string.Format("保底5星数:     {0:G}\n", bao5_ct);
            ret = ret + string.Format("连续没4星:     {0:G}\n", no_4_ct);
            ret = ret + string.Format("连续没5星:     {0:G}\n", no_5_ct);
            ret = ret + "注:综合4星率:13%,综合5星率:1.6%\n";
            ret = ret + string.Format("星辉数:        {0:G}\n", xinghui_5);
            ret = ret + string.Format("星辰数:        {0:G}\n", xingchen_4);

            return ret;
        }
    }

   
}
