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


        public Form1()
        {
            InitializeComponent();
            //初始化圣遗物副属性评分
            initSywScore();
        }
        //https://bbs.nga.cn/read.php?tid=23802190

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = app_name + " ver " + app_version;
            this.Icon = cs_ys_helper.Properties.Resources.main_icon;
        }



        private void updateSywScore(object sender,EventArgs e)
        {


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
                label_sywscore1.Text = "最大值:";


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
            String cookie = textBox_cookie.Text.Trim();
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
                richTextBox_userinfo.AppendText("岩神瞳数\t" + stats["active_day_number"] + "\n");
                richTextBox_userinfo.AppendText("获得角色数\t" + stats["avatar_number"] + "\n");
                richTextBox_userinfo.AppendText("解锁传送点\t" + stats["way_point_number"] + "\n");
                richTextBox_userinfo.AppendText("解锁秘境\t" + stats["domain_number"] + "\n");
                richTextBox_userinfo.AppendText("深境螺旋\t" + stats["spiral_abyss"] + "\n");
                richTextBox_userinfo.AppendText("华丽宝箱数\t" + stats["precious_chest_number"] + "\n");
                richTextBox_userinfo.AppendText("珍贵宝箱数\t" + stats["luxurious_chest_number"] + "\n");
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
                String cookie = textBox_cookie.Text.Trim();
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
                        }
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
            String cookie = textBox_cookie.Text.Trim();
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
                info = info + "开始时间\t" + Utils.ts2Date(data["end_time"].ToString()) + "\n";
                richTextBox_abyss_info.Text = info;
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] 查询深渊信息OK！";
            }
            else
            {
                toolStripStatusLabel1.Text = "[" + DateTime.Now.ToString() + "] " + abyssInfo["message"].ToString();
            }
        }
    }
}
