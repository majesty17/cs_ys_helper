using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LitJson;

namespace cs_ys_helper
{
    public partial class Form1 : Form
    {
        string app_name = "Genshin Impact Helper";
        string app_version = "0.1";
        string cookie;

        WishSimu wishGame;
        SywScore sywScore;

        public Form1()
        {
            InitializeComponent();
            //初始化圣遗物副属性评分
            initSywScore();
            //初始化抽卡历史类型
            initWishType();
        }
        //https://bbs.nga.cn/read.php?tid=23802190

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = app_name + " ver " + app_version;
            this.Icon = cs_ys_helper.Properties.Resources.main_icon;
            Win32Utility.SetCueText(textBox_cookie, "请输入cookie！");
            Win32Utility.SetCueText(textBox_abyss_uid, "请输入游戏uid！");
            Win32Utility.SetCueText(textBox_userinfo_uid, "请输入游戏uid！");
            Win32Utility.SetCueText(textBox_cookie, "account_id=xxxxxx; cookie_token=xxxxxxx");
            comboBox_wish_type.SelectedIndex = 2;
            //u初始化抽卡对象
            wishGame = new WishSimu();
            wishGame.resetGame();

            //尝试读取cookie
            cookie = Utils.loadCookie();
            textBox_cookie.Text = cookie;
        }





        private void updateSywScore(object sender,EventArgs e)
        {
            sywScore = new SywScore();

            try
            {
                string item1 = comboBox_sywscore1.SelectedItem.ToString();
                string item2 = comboBox_sywscore2.SelectedItem.ToString();
                string item3 = comboBox_sywscore3.SelectedItem.ToString();
                string item4 = comboBox_sywscore4.SelectedItem.ToString();

                HashSet<string> names = new HashSet<string>();
                names.Add(item1);
                names.Add(item2);
                names.Add(item3);
                names.Add(item4);
                Console.WriteLine(names.Count);
                if (names.Count < 4)
                {
                    //MessageBox.Show("");
                    throw new Exception("词条不能重复！");
                    return;
                }

                double value1 = Convert.ToDouble(textBox_sywscore1.Text);
                double value2 = Convert.ToDouble(textBox_sywscore2.Text);
                double value3 = Convert.ToDouble(textBox_sywscore3.Text);
                double value4 = Convert.ToDouble(textBox_sywscore4.Text);

                int ind1 = Data.SywSecondAttrName.ToList().IndexOf(item1);
                int ind2 = Data.SywSecondAttrName.ToList().IndexOf(item2);
                int ind3 = Data.SywSecondAttrName.ToList().IndexOf(item3);
                int ind4 = Data.SywSecondAttrName.ToList().IndexOf(item4);
                Console.WriteLine(ind1 + "," + ind4);
                double max1 = Data.SywSecondAttrValue[ind1, 4];
                double max2 = Data.SywSecondAttrValue[ind2, 4];
                double max3 = Data.SywSecondAttrValue[ind3, 4];
                double max4 = Data.SywSecondAttrValue[ind4, 4];

                progressBar_sywscore1.Maximum = Convert.ToInt32(max1 * 10);
                progressBar_sywscore1.Value = Convert.ToInt32(value1 * 10);



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + ex.Message;
                return;
            }
            toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + "ok";
        }


        private void initSywScore()
        {
            string[] names = Data.SywSecondAttrName;
            for (int i = 0; i < names.Length; i++)
            {
                comboBox_sywscore1.Items.Add(names[i]);
                comboBox_sywscore2.Items.Add(names[i]);
                comboBox_sywscore3.Items.Add(names[i]);
                comboBox_sywscore4.Items.Add(names[i]);
            }
            comboBox_sywscore1.SelectedIndex = 0;
            comboBox_sywscore2.SelectedIndex = 1;
            comboBox_sywscore3.SelectedIndex = 2;
            comboBox_sywscore4.SelectedIndex = 3;
        }


        //查询角色信息
        private void button_userinfo_Click(object sender, EventArgs e)
        {
            string uid = textBox_userinfo_uid.Text.Trim();
            if (uid.Length != 9)
            {
                MessageBox.Show("uid不合法！");
                return;
            }
            listView_rolelist.Tag = uid;
            JsonData userinfo = Utils.getUserInfo(uid,cookie);
            //JsonData abyssInfo = Utils.getUserAbyss(uid, cookie);
            if (userinfo["message"].ToString() == "OK")
            {
                //role信息
                listView_rolelist.Items.Clear();
                JsonData avatars = userinfo["data"]["avatars"];
                foreach(JsonData role in avatars)
                {
                    Console.WriteLine(role["name"].ToString());
                    ListViewItem lvi = new ListViewItem(role["name"].ToString());
                    lvi.SubItems.Add(Utils.getElement(role["element"].ToString()));
                    lvi.SubItems.Add(role["fetter"].ToString());
                    lvi.SubItems.Add(role["level"].ToString());
                    lvi.SubItems.Add(role["rarity"].ToString());
                    //颜色
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems[4].BackColor = Utils.getRarityColor((role["rarity"].ToString()));
                    listView_rolelist.Items.Add(lvi);
                    lvi.Tag = role["id"].ToString();
                }

                JsonData stats = userinfo["data"]["stats"];

                //统计信息
                richTextBox_userinfo.Clear();
                richTextBox_userinfo.AppendText("活跃天数\t" + stats["active_day_number"] + "\n");
                richTextBox_userinfo.AppendText("成就达成数\t" + stats["achievement_number"] + "\n");
                richTextBox_userinfo.AppendText("风神瞳数\t" + stats["anemoculus_number"] + "\n");
                richTextBox_userinfo.AppendText("岩神瞳数\t" + stats["geoculus_number"] + "\n");
                richTextBox_userinfo.AppendText("获得角色数\t" + stats["avatar_number"] + "\n");
                richTextBox_userinfo.AppendText("解锁传送点\t" + stats["way_point_number"] + "\n");
                richTextBox_userinfo.AppendText("解锁秘境\t" + stats["domain_number"] + "\n");
                richTextBox_userinfo.AppendText("深境螺旋\t" + stats["spiral_abyss"] + "\n");
                richTextBox_userinfo.AppendText("华丽宝箱数\t" + stats["luxurious_chest_number"] + "\n");
                richTextBox_userinfo.AppendText("珍贵宝箱数\t" + stats["precious_chest_number"] + "\n");
                richTextBox_userinfo.AppendText("精致宝箱数\t" + stats["exquisite_chest_number"] + "\n");
                richTextBox_userinfo.AppendText("普通宝箱数\t" + stats["common_chest_number"]);

                //城市
                richTextBox_usercity.Clear();
                JsonData cities = userinfo["data"]["world_explorations"];
                foreach ( JsonData city in cities)
                {
                    richTextBox_usercity.AppendText("【"+city["name"].ToString() + "】\n");
                    if(city["type"].ToString()== "Reputation")
                        richTextBox_usercity.AppendText("声望等级\t"+city["level"].ToString() + "\n");
                    else
                        richTextBox_usercity.AppendText("供奉等级\t" + city["level"].ToString() + "\n");
                    double exp_per = Convert.ToDouble(city["exploration_percentage"].ToString()) / 10.0;

                    richTextBox_usercity.AppendText("探索度  \t" + exp_per.ToString() + "%\n");
                }

                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] 查询用户信息OK！";

            }
            else
            {
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + userinfo["message"].ToString();
            }

            
        }

        //点击角色列表，展示详情
        private void listView_rolelist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            string uid = listView_rolelist.Tag.ToString();
            //有选中的话去查选中的
            if (listView_rolelist.SelectedItems.Count > 0)
            {
                ListViewItem lvi_old = listView_rolelist.SelectedItems[0];
                Console.WriteLine(lvi_old.Tag.ToString());
                JsonData role_details = Utils.getRoleDetails(uid, new string[] { lvi_old.Tag.ToString() }, cookie);
                if (role_details["message"].ToString() == "OK")
                {
                    JsonData role = role_details["data"]["avatars"][0];
                    Console.WriteLine(role["name"].ToString());
                    string icon = role["icon"].ToString();
                    pictureBox_roledetails.Load(icon);

                    //圣遗物
                    listView_syw.Items.Clear();
                    foreach(JsonData syw in role["reliquaries"])
                    {
                        ListViewItem lvi = new ListViewItem(syw["pos_name"].ToString());
                        lvi.SubItems.Add(syw["name"].ToString());
                        lvi.SubItems.Add(syw["set"]["name"].ToString());
                        lvi.SubItems.Add(syw["level"].ToString());
                        lvi.SubItems.Add(syw["rarity"].ToString());
                        lvi.UseItemStyleForSubItems = false;
                        lvi.SubItems[4].BackColor = Utils.getRarityColor((syw["rarity"].ToString()));
                        listView_syw.Items.Add(lvi);
                    }

                    //武器
                    listView_userweapon.Items.Clear();
                    JsonData weapon = role["weapon"];
                    ListViewItem lvi_we = new ListViewItem(weapon["name"].ToString());
                    lvi_we.SubItems.Add(weapon["type_name"].ToString());
                    lvi_we.SubItems.Add(weapon["level"].ToString());
                    lvi_we.SubItems.Add(weapon["rarity"].ToString());
                    lvi_we.SubItems.Add(weapon["affix_level"].ToString());
                    lvi_we.UseItemStyleForSubItems = false;
                    lvi_we.SubItems[3].BackColor = Utils.getRarityColor((weapon["rarity"].ToString()));
                    listView_userweapon.Items.Add(lvi_we);

                    //命之座
                    listView_life.Items.Clear();
                    foreach(JsonData li in role["constellations"])
                    {
                        ListViewItem lvi_li = new ListViewItem(li["pos"].ToString());
                        lvi_li.SubItems.Add(li["name"].ToString());
                        Console.WriteLine(li["is_actived"].ToString());
                        if (li["is_actived"].ToString() == "True")
                        {
                            lvi_li.UseItemStyleForSubItems = false;
                            lvi_li.Checked = true;
                            lvi_li.SubItems[1].Font = new Font(Control.DefaultFont, FontStyle.Bold);
                            lvi_li.SubItems[1].BackColor = Color.AliceBlue;
                            lvi_li.Tag = true;
                        }
                        else
                            lvi_li.Tag = false;
                        listView_life.Items.Add(lvi_li);
                    }

                    //全局
                    richTextBox_roledetails.Clear();
                    string details = "";
                    details = details + "角色名\t" + role["name"] + "\n";
                    details = details + "属性\t" + Utils.getElement(role["element"].ToString()) + "\n";
                    details = details + "好感度\t" + role["fetter"] + "\n";
                    details = details + "等级\t" + role["level"] + "\n";
                    details = details + "星级\t" + role["rarity"];

                    richTextBox_roledetails.Text = details;

                    //头像颜色
                    pictureBox_roledetails.BackColor = Utils.getRarityColor(role["rarity"].ToString());
                    toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] 查询角色详情OK！";
                }
                else
                {
                    toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + role_details["message"].ToString();
                }
            }
        }


        //查询个人深渊螺旋信息
        private void button_searchabyss_Click(object sender, EventArgs e)
        {
            string uid = textBox_abyss_uid.Text.Trim();
            if (uid.Length != 9)
            {
                MessageBox.Show("uid不合法！");
                return;
            }
            //listView_rolelist.Tag = uid;
            JsonData abyssInfo = Utils.getUserAbyss(uid, cookie);
            if (abyssInfo["message"].ToString() == "OK")
            {
                JsonData data = abyssInfo["data"];
                //全局信息
                string info = "最深抵达\t" + data["max_floor"].ToString() + "\n";
                info = info + "挑战次数\t" + data["total_battle_times"].ToString() + "\n";
                info = info + "获胜次数\t" + data["total_win_times"].ToString() + "\n";
                info = info + "星星个数\t" + data["total_star"].ToString() + "\n";
                info = info + "开始时间\t" + Utils.ts2Date(data["start_time"].ToString()) + "\n";
                info = info + "结束时间\t" + Utils.ts2Date(data["end_time"].ToString()) + "\n";
                richTextBox_abyss_info.Text = info;


                //每项的top
                listView_rank.Items.Clear();
                ListViewItem[] lvi = new ListViewItem[5];

                lvi[0] = new ListViewItem("最多击破数");
                lvi[0].SubItems.Add(data["defeat_rank"][0]["value"].ToString());
                lvi[0].SubItems.Add(Utils.code2name(data["defeat_rank"][0]["avatar_id"].ToString()));
                lvi[0].Tag = data["defeat_rank"][0]["rarity"].ToString();

                lvi[1] = new ListViewItem("最强一击");
                lvi[1].SubItems.Add(data["damage_rank"][0]["value"].ToString());
                lvi[1].SubItems.Add(Utils.code2name(data["damage_rank"][0]["avatar_id"].ToString()));
                lvi[1].Tag = data["damage_rank"][0]["rarity"].ToString();

                lvi[2] = new ListViewItem("承受最多伤害");
                lvi[2].SubItems.Add(data["take_damage_rank"][0]["value"].ToString());
                lvi[2].SubItems.Add(Utils.code2name(data["take_damage_rank"][0]["avatar_id"].ToString()));
                lvi[2].Tag = data["take_damage_rank"][0]["rarity"].ToString();

                lvi[3] = new ListViewItem("元素爆发次数");
                lvi[3].SubItems.Add(data["defeat_rank"][0]["value"].ToString());
                lvi[3].SubItems.Add(Utils.code2name(data["normal_skill_rank"][0]["avatar_id"].ToString()));
                lvi[3].Tag = data["normal_skill_rank"][0]["rarity"].ToString();

                lvi[4] = new ListViewItem("元素战技次数");
                lvi[4].SubItems.Add(data["energy_skill_rank"][0]["value"].ToString());
                lvi[4].SubItems.Add(Utils.code2name(data["energy_skill_rank"][0]["avatar_id"].ToString()));
                lvi[4].Tag = data["energy_skill_rank"][0]["rarity"].ToString();



                for (int i = 0; i < lvi.Length; i++)
                {
                    lvi[i].UseItemStyleForSubItems = false;
                    lvi[i].SubItems[2].BackColor = Utils.getRarityColor(lvi[i].Tag.ToString());
                    listView_rank.Items.Add(lvi[i]);
                }


                //填写深渊细节
                listView_abyss_details.Items.Clear();
                foreach(JsonData floor in data["floors"])
                {
                    Console.WriteLine(floor["index"].ToString()+"层开始");
                    string floor_idx = floor["index"].ToString();
                    string star_floor = floor["star"].ToString() + "/" + floor["max_star"].ToString();

                    ListViewItem lvi_1 = new ListViewItem(floor_idx);
                    lvi_1.SubItems.Add(star_floor);
                    listView_abyss_details.Items.Add(lvi_1);

                    foreach (JsonData level in floor["levels"])
                    {
                        string lev_idx = floor_idx + "-" + level["index"].ToString();
                        string lev_start = level["star"].ToString() + "/" + level["max_star"].ToString();


                        ListViewItem lvi_2= new ListViewItem(new string[] {"","" ,lev_idx, lev_start, "","" });

                        listView_abyss_details.Items.Add(lvi_2);

                        foreach (JsonData battle in level["battles"])
                        {
                            string battle_idx = battle["index"].ToString();
                            string date = Utils.ts2Datetime(battle["timestamp"].ToString());
                            JsonData role = battle["avatars"];

                            ListViewItem lvi_3 = new ListViewItem(new string[] { "", "", "", "", battle_idx });
                            lvi_3.UseItemStyleForSubItems = false;
                            string[] roles = new string[] { "", "", "", "" };
                            for(int i = 0; i < 4; i++)
                            {
                                if (i >= role.Count)
                                {
                                    lvi_3.SubItems.Add("----");
                                }
                                else
                                {
                                    roles[i] = Utils.code2name(role[i]["id"].ToString()) + "|" + role[i]["level"].ToString();
                                    lvi_3.SubItems.Add(roles[i]);
                                    lvi_3.SubItems[i + 5].BackColor = Utils.getRarityColor(role[i]["rarity"].ToString());
                                }
                            }

                            lvi_3.SubItems.Add(date);

                            listView_abyss_details.Items.Add(lvi_3);

                        }
                    }

                }

                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] 查询深渊信息OK！";
            }
            else
            {
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + abyssInfo["message"].ToString();
            }
        }

        //不让修改命座,如果发生改变，以tag里的值为准
        private void listView_life_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem lvi = e.Item;
            lvi.Checked = (bool)(lvi.Tag);
        }





        /// <summary>
        /// 模拟抽卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        //单抽
        private void button_wish1_Click(object sender, EventArgs e)
        {
            wishGame.wish1();
            sync_wish_data();
        }
        //10
        private void button_wish10_Click(object sender, EventArgs e)
        {
            wishGame.wish10();
            sync_wish_data();
        }
        //100
        private void button_wish100_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                wishGame.wish10();
            sync_wish_data();
        }
        //清空背包，重置状态
        private void button_wish_reset_Click(object sender, EventArgs e)
        {
            wishGame.resetGame();
            sync_wish_data();
        }



        //信息同步到背包、状态
        private void sync_wish_data()
        {
            //根据历史抽的情况，写状态信息；
            string stat = wishGame.makeSummary();
            richTextBox_wish_stat.Text = stat;

            //根据背包内容，同步到背包listview；
            listView_wishbag.Items.Clear();
            //先弄角色
            listView_wishbag.Items.Add(new ListViewItem("角色"));
            foreach (JsonData item in wishGame.bag)
            {
                ListViewItem lvi = new ListViewItem(item["name"].ToString());
                if (item["type"].ToString() == "武器")
                    continue;
                lvi.SubItems.Add("☺");
                lvi.SubItems.Add(item["rarity"].ToString());
                lvi.SubItems.Add(item["count"].ToString());
                lvi.UseItemStyleForSubItems = false;

                lvi.SubItems[2].BackColor = Utils.getRarityColor((item["rarity"].ToString()));

                listView_wishbag.Items.Add(lvi);
            }
            //再弄武器
            listView_wishbag.Items.Add(new ListViewItem("武器"));
            foreach (JsonData item in wishGame.bag)
            {
                ListViewItem lvi = new ListViewItem(item["name"].ToString());
                if (item["type"].ToString() == "角色")
                    continue;
                lvi.SubItems.Add("➳");
                lvi.SubItems.Add(item["rarity"].ToString());
                lvi.SubItems.Add(item["count"].ToString());
                lvi.UseItemStyleForSubItems = false;

                lvi.SubItems[2].BackColor = Utils.getRarityColor((item["rarity"].ToString()));

                listView_wishbag.Items.Add(lvi);
            }
        }




        //内容变化的时候自动更新cookie
        private void textBox_cookie_TextChanged(object sender, EventArgs e)
        {
            cookie = textBox_cookie.Text.Trim();
            Utils.saveCookie(cookie);
        }

        //获取许愿历史记录
        private void button_wishlog_Click(object sender, EventArgs e)
        {
            string auth_key = textBox_authkey.Text.Trim();
            string type = ((ComboxItem)comboBox_wishlogtype.SelectedItem).Value;
            listView_wishlog.Items.Clear();
            try
            {
                int ct = 20;
                int page = 1;
                while (ct == 20) //如果等于20 就持续获取
                {
                    JsonData js = Utils.getWishHis(type, auth_key, page);
                    JsonData datas = js["data"]["list"];
                    ct = datas.Count;
                    foreach (JsonData item in datas)
                    {
                        ListViewItem lvi = new ListViewItem(item["item_type"].ToString());
                        lvi.SubItems.Add(item["name"].ToString());
                        lvi.SubItems.Add(item["rank_type"].ToString());
                        lvi.SubItems.Add(item["time"].ToString());
                        lvi.UseItemStyleForSubItems = false;

                        lvi.SubItems[2].BackColor = Utils.getRarityColor((item["rank_type"].ToString()));
                        listView_wishlog.Items.Add(lvi);
                    }

                    page++;
                }
                int count = listView_wishlog.Items.Count;
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + "查询许愿历史OK! 共查到[" +(count) +"]条记录~";
                richTextBox_wishlog.Text = anaWish();
            }
            catch(Exception ex)
            {
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] ERROR!" + ex.Message;
            }
        }


        //初始化许愿类型
        private void initWishType()
        {
            comboBox_wishlogtype.Items.Clear();
            comboBox_wishlogtype.Items.Add(new ComboxItem("新手祈愿", "100"));
            comboBox_wishlogtype.Items.Add(new ComboxItem("常驻祈愿", "200"));
            comboBox_wishlogtype.Items.Add(new ComboxItem("角色活动祈愿", "301"));
            comboBox_wishlogtype.Items.Add(new ComboxItem("武器活动祈愿", "302"));
            comboBox_wishlogtype.SelectedIndex = 0;
        }

        //分析许愿数据
        private string anaWish()
        {
            List<string> data = new List<string>();
            string data_str = "";
            foreach(ListViewItem lvi in listView_wishlog.Items)
            {
                if (lvi.SubItems.Count > 2)
                {
                    data.Add(lvi.SubItems[2].Text);
                    data_str = data_str + lvi.SubItems[2].Text;
                }
            }
            int count = data.Count;
            int ct_4 = data.Count(s => s == "4");
            int ct_5 = data.Count(s => s == "5");

            string[] spl_5 = data_str.Split(new char[1] { '5' });
            string[] spl_4 = data_str.Replace('5', '4').Split(new char[1] { '4' });


            float avg_4, avg_5;
            int max_4, max_5, min_4, min_5;


            if (spl_4.Length == 1)
            {
                min_4 = -1;
                max_4 = -1;
                avg_4 = -1;
            }
            else
            {
                min_4 = int.MaxValue;
                max_4 = int.MinValue;
                int sum = 0;
                for(int i = 1; i < spl_4.Length; i++)
                {
                    if (spl_4[i].Length < min_4) min_4 = spl_4[i].Length;
                    if (spl_4[i].Length > max_4) max_4 = spl_4[i].Length;
                    sum += spl_4[i].Length;
                }
                avg_4 = (float)sum / (float)(spl_4.Length - 1);
            }

            if (spl_5.Length == 1)
            {
                min_5 = -1;
                max_5 = -1;
                avg_5 = -1;
            }
            else
            {
                min_5 = int.MaxValue;
                max_5 = int.MinValue;
                int sum = 0;
                for (int i = 1; i < spl_5.Length; i++)
                {
                    if (spl_5[i].Length < min_5) min_5 = spl_5[i].Length;
                    if (spl_5[i].Length > max_5) max_5 = spl_5[i].Length;
                    sum += spl_5[i].Length;
                }
                avg_5 = (float)sum / (float)(spl_5.Length - 1);
            }


            Console.WriteLine(data_str);


            string ret = "";
            ret = ret + string.Format("总抽数:               {0}\n", count);
            ret = ret + string.Format("4星数|率:             {0}|{1:P4}\n", ct_4, (double)ct_4  / (double)count);
            ret = ret + string.Format("5星数|率:             {0}|{1:P4}\n", ct_5, (double)ct_5  / (double)count);
            ret = ret + string.Format("4星间隔max|min|avg:   {0}|{1}|{2:f2}\n", max_4, min_4, avg_4);
            ret = ret + string.Format("5星间隔max|min|avg:   {0}|{1}|{2:f2}\n", max_5, min_5, avg_5);
            ret = ret + "" + "\n";
            ret = ret + string.Format("注1:算4星间隔的话5星也按照4星来算;" + "\n");
            ret = ret + string.Format("注2:这里说的是两次中的间隔,而不是第几次中;" + "\n");
            return ret ;
        }

        private void button_wishloghelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此查询需auth_key，获取方式可以咨询本人，也可以参考此git：https://github.com/pcrbot/erinilis-modules/tree/master/genshingachalog", "注意事项",MessageBoxButtons.OK);
        }
    }


    //支持kv的comboxitem
    public class ComboxItem
    {
        public string Text = "";
        public string Value = "";
        public ComboxItem(string _Text, string _Value)
        {
            Text = _Text;
            Value = _Value;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
