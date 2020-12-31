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
        /// <summary>
        /// {"name":"凝光","type":"角色","rarity":5,"count":10}
        /// </summary>
        public JsonData bag;

        int no_5_ct;

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
            bag = new JsonData();
            rand = new Random(DateTime.Now.Millisecond);
            

        }

        //随机一个
        private JsonData random()
        {
            double r = rand.NextDouble();
            int rarity = 3;
            if (r < 0.051d) rarity = 4;
            if (r < 0.004d) rarity = 5;

            //如果已经中了，则重置90发保底
            if (rarity == 5)
                no_5_ct = 0;

            //90发大保底，第90发必定5星
            if (no_5_ct >= 90 - 1)
            {
                rarity = 5;
                no_5_ct = 0;
            }
            else
            {
                no_5_ct++;
            }

            string[] pool = Data.wish_pool[rarity - 3];
            int len = pool.Length;
            int pos = rand.Next() % len;
            string name = pool[pos].Split(new char[] { '_' })[0];
            string type = pool[pos].Split(new char[] { '_' })[1];

            string j_str = "{\"name\":\"" + name + "\",\"type\":\"" + type + "\",\"rarity\":" + rarity + ",\"count\":1}";
            return JsonMapper.ToObject(j_str);
        }

        //随机一个(10连里面的保底)
        private JsonData random10()
        {
            double r = rand.NextDouble();
            //如果落在3星区，就重来；
            while (r > 0.051d)
            {
                r = rand.NextDouble();
            }
            int rarity = 4;
            if (r < 0.004d) rarity = 5;

            //90发大保底，
            if (no_5_ct >= 90)
            {
                rarity = 5;
                no_5_ct = 0;
            }
            else
            {
                no_5_ct++;
            }

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
                    else //人物的话，小于7，加数字，大于7加星辉、星辰
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
        //10连
        public JsonData[] wish10()
        {
            times += 10;
            times_10 += 1;
            int count_3 = 0;
            JsonData[] wishes = new JsonData[10];

            //前9次搞出来，统计下出的三星的次数
            for (int i = 0; i < 9; i++)
            {
                wishes[i] = random();
                if (wishes[i]["rarity"].ToString() == "3")
                    count_3++;
            }

            //前9次都是3星的话，第10个给4、5；否则纯随机
            if (count_3 == 9)
            {
                wishes[9] = random10();
            }
            else
            {
                wishes[9] = random();
            }
            

            for (int i = 0; i < 10; i++)
            {
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
                                                     
            ret = ret + string.Format("5星数|率:      {0:G}|{1:P}\n", star_5, (float)star_5 / (float)times );
            ret = ret + string.Format("4星数|率:      {0:G}|{1:P}\n", star_4, (float)star_4 / (float)times );
            ret = ret + string.Format("连续没5星:     {0:G}\n", no_5_ct);
            ret = ret + "注:综合4星率:13%,综合5星率:1.6%\n";
            ret = ret + string.Format("星辉数:        {0:G}\n", xinghui_5);
            ret = ret + string.Format("星辰数:        {0:G}\n", xingchen_4);

            return ret;
        }
    }

   
}
