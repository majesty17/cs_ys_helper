using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using System.Web;


namespace cs_ys_helper
{
    class Utils
    {


        //获取用户信息
        public static JsonData getUserInfo(string uid,string cookie)
        {


//JsonData jd = JsonMapper.ToObject(content);

            string url = "https://api-takumi.mihoyo.com/game_record/genshin/api/index?server=" + getServer(uid) + "&role_id=" + uid;

            Console.WriteLine(url);


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Headers.Add("cookie", cookie);// Data.COOKIE);
            myRequest.Headers.Add("DS", getDS());
            myRequest.Headers.Add("Origin", "https://webstatic.mihoyo.com");
            myRequest.Headers.Add("x-rpc-app_version", Data.VERSION);
            myRequest.Headers.Add("x-rpc-client_type", "4");
            //myRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            //myRequest.Headers.Add("Accept-Language", "zh-CN,en-US;q=0.8");
            myRequest.Headers.Add("X-Requested-With", "com.mihoyo.hyperion");

            myRequest.UserAgent = "Mozilla/5.0 (Linux; Android 9; Unspecified Device) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 miHoYoBBS/2.2.0";
            myRequest.Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";
            myRequest.Accept = "application/json, text/plain, */*";


            myRequest.Method = "GET";

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            string content = reader.ReadToEnd();

            reader.Close();

            Console.WriteLine(content);

            return JsonMapper.ToObject(content);
        }

        //
        //获取深渊
        public static JsonData getUserAbyss(string uid, string cookie)
        {


            //JsonData jd = JsonMapper.ToObject(content);

            string url = "https://api-takumi.mihoyo.com/game_record/genshin/api/spiralAbyss?server=" + getServer(uid) + "&role_id=" + uid + "&schedule_type=1";
            // + getServer(uid) + "&role_id=" + uid;

            Console.WriteLine(url);


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Headers.Add("cookie", cookie);// Data.COOKIE);
            myRequest.Headers.Add("DS", getDS());
            myRequest.Headers.Add("Origin", "https://webstatic.mihoyo.com");
            myRequest.Headers.Add("x-rpc-app_version", Data.VERSION);
            myRequest.Headers.Add("x-rpc-client_type", "4");
            //myRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            //myRequest.Headers.Add("Accept-Language", "zh-CN,en-US;q=0.8");
            myRequest.Headers.Add("X-Requested-With", "com.mihoyo.hyperion");

            myRequest.UserAgent = "Mozilla/5.0 (Linux; Android 9; Unspecified Device) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 miHoYoBBS/2.2.0";
            myRequest.Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";
            myRequest.Accept = "application/json, text/plain, */*";


            myRequest.Method = "GET";

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            string content = reader.ReadToEnd();

            reader.Close();

            Console.WriteLine(content);

            return JsonMapper.ToObject(content);
        }


        //获取详细角色信息
        public static JsonData getRoleDetails(string uid,string[] character_ids,string cookie)
        {
            string url = "https://api-takumi.mihoyo.com/game_record/genshin/api/character";
            string server = getServer(uid);

            string content = "{\"character_ids\":[" + character_ids[0];
            for(int i = 1; i < character_ids.Length; i++)
            {
                content = content + "," + character_ids[i];
            }
            content = content + "],\"role_id\":\"" + uid + "\",\"server\":\"" + server + "\"}";
            byte[] bs = Encoding.UTF8.GetBytes(content);
            Console.WriteLine(url);
            Console.WriteLine(content);

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Headers.Add("cookie", cookie);
            myRequest.Headers.Add("DS", getDS());
            myRequest.Headers.Add("Origin", "https://webstatic.mihoyo.com");
            myRequest.Headers.Add("x-rpc-app_version", Data.VERSION);
            myRequest.Headers.Add("x-rpc-client_type", "4");
            //myRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            //myRequest.Headers.Add("Accept-Language", "zh-CN,en-US;q=0.8");
            myRequest.Headers.Add("X-Requested-With", "com.mihoyo.hyperion");

            myRequest.UserAgent = "Mozilla/5.0 (Linux; Android 9; Unspecified Device) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 miHoYoBBS/2.2.0";
            myRequest.Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";
            myRequest.Accept = "application/json, text/plain, */*";
            myRequest.ContentType = "application/json;charset=UTF-8";
            myRequest.ContentLength = bs.Length;


            myRequest.Method = "POST";
            Stream reqStream = myRequest.GetRequestStream();
            reqStream.Write(bs, 0, bs.Length);
            reqStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string ret = reader.ReadToEnd();
            reader.Close();
            Console.WriteLine(ret);
            return JsonMapper.ToObject(ret);
        }





        //属性英文名到中文名
        public static string getElement(string eng)
        {
            string Character_Type = "";
            if (eng == "None")
                Character_Type = "无属性";
            else if (eng == "Anemo")
                Character_Type = "风属性";
            else if (eng == "Pyro")
                Character_Type = "火属性";
            else if (eng == "Geo")
                Character_Type = "岩属性";
            else if (eng == "Electro")
                Character_Type = "雷属性";
            else if (eng == "Cryo")
                Character_Type = "冰属性";
            else if (eng == "Hydro")
                Character_Type = "水属性";
            else
                Character_Type = "草属性";
            return Character_Type;
        }
        //属性英文名到颜色
        public static Color getElementColor(string ele)
        {
            return Color.Wheat;
        }
        //稀有度转化为颜色


        public static Color getRarityColor(string n)
        {
            if (n == "1") return Color.FromArgb(192, 114, 119, 139);
            else if (n == "2") return Color.FromArgb(192, 41, 144, 114);
            else if (n == "3") return Color.FromArgb(192, 81, 129, 205);
            else if (n == "4") return Color.FromArgb(192, 163, 86, 226);  //Color.Purple;
            else if (n == "5") return Color.FromArgb(192, 188, 106, 50);// Color.Goldenrod;
            else return Color.White;
        }


        //角色code转换为name
        public static string code2name(string code) //感觉跟这个有点像了：https://webstatic.mihoyo.com/hk4e/gacha_info/cn_gf01/items/zh-cn.json
        {
            string code2name_j = "{\"10000030\": \"钟离\",\"10000026\": \"魈\",\"10000035\": \"七七\",\"10000037\": \"甘雨\"," +
                "\"10000038\": \"阿贝多\",\"10000003\": \"琴\",\"10000005\": \"旅行者\",\"10000006\": \"丽莎\",\"10000007\": " +
            "\"旅行者\",\"10000014\": \"芭芭拉\",\"10000015\": \"凯亚\",\"10000016\": \"迪卢克\",\"10000020\": \"雷泽\",\"10000021\": " +
            "\"安柏\",\"10000022\": \"温迪\",\"10000023\": \"香菱\",\"10000024\": \"北斗\",\"10000025\": \"行秋\",\"10000027\": \"凝光\"," +
            "\"10000029\": \"可莉\",\"10000031\": \"菲谢尔\",\"10000032\": \"班尼特\",\"10000033\": \"达达利亚\",\"10000034\": \"诺艾尔\"," +
            "\"10000036\": \"重云\",\"10000039\": \"迪奥娜\",\"10000041\": \"莫娜\",\"10000042\": \"刻晴\",\"10000043\": \"砂糖\",\"注释1\": \"10000005/7分别对应哥哥和妹妹\"}";
            JsonData code2name_obj = JsonMapper.ToObject(code2name_j);
            try
            {
                return code2name_obj[code].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("未识别的角色code！！！");
                return "[未知]";
            }
        }


        //根据uid获取服务器名
        private static string getServer(string uid)
        {
            if (uid.StartsWith("1"))
            {
                return "cn_gf01";
            }else if (uid.StartsWith("5"))
            {
                return "cn_qd01";
            }
            return "";
        }
        //获取签名
        private static string getDS()
        {
            string n = "";
            if (Data.VERSION == "2.1.0")
            {
                n = md5(Data.VERSION);
            }else if (Data.VERSION == "2.2.1")
            {
                n = "cx2y9z9a29tfqvr1qsq6c7yz99b5jsqt";
            }
            else
            {
                Data.VERSION = "2.2.1";
                n = "cx2y9z9a29tfqvr1qsq6c7yz99b5jsqt";
            }

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string i = Convert.ToInt64(ts.TotalSeconds - 28800).ToString();
            Console.WriteLine(i);
            string r = getRandomStr(6);
            string c = md5("salt=" + n + "&t=" + i + "&r=" + r);
            return i + "," + r + "," + c;
        }
        //计算md5
        private static string md5(string a)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(a));
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }
        //获取随机字符串
        private static string getRandomStr(int len)
        {
            byte[] b = new byte[len];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = "", str = "0123456789abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < len; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        //时间戳转日期
        public static string ts2Date(string ts)
        {
            long delta = Convert.ToInt64(ts);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            return startTime.AddSeconds(delta).ToString("yyyy-MM-dd ");
        }
        //时间戳转日期-长
        public static string ts2Datetime(string ts)
        {
            long delta = Convert.ToInt64(ts);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            return startTime.AddSeconds(delta).ToString("yyyy-MM-dd HH:mm:ss");
        }

        //尝试从本地文件读取cookie
        public static string loadCookie()
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/" + Data.cookieFile;
            try
            {
                return File.ReadAllText(path);
            }
            catch(Exception ex)
            {
                Console.WriteLine("find no cookie file!");
                return "";
            }
        }

        //保存用户输入的cookie
        public static void saveCookie(string cookie)
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/" + Data.cookieFile;
            try
            {
                File.WriteAllText(path, cookie);
            }
            catch(Exception ex)
            {
                Console.WriteLine("写入cookie失败！");
            }
        }

        
        public static JsonData getWishType(string auth_key)
        {

            //JsonData jd = JsonMapper.ToObject(content);

            string url = "https://hk4e-api.mihoyo.com/event/gacha_info/api/getConfigList?authkey_ver=1&authkey=" + auth_key+ "&lang=zh-cn";
            // + getServer(uid) + "&role_id=" + uid;

            Console.WriteLine(url);


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);


            myRequest.UserAgent = "Mozilla/5.0 (Linux; Android 9; Unspecified Device) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 miHoYoBBS/2.2.0";
            myRequest.Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";
            myRequest.Accept = "application/json, text/plain, */*";


            myRequest.Method = "GET";

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            string content = reader.ReadToEnd();

            reader.Close();

            Console.WriteLine(content);

            return JsonMapper.ToObject(content);
        }

        //获取抽卡历史
        public static JsonData getWishHis(string type,string auth_key,int page)
        {
            //JsonData jd = JsonMapper.ToObject(content);

            string url = "https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?authkey_ver=1&authkey=" +  HttpUtility.UrlEncode(auth_key) +
                "&page=" + (page) + "&gacha_type=" + type +
                "&lang=zh-cn&size=20";
            // + getServer(uid) + "&role_id=" + uid;

            Console.WriteLine(url);


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);


            myRequest.UserAgent = "Mozilla/5.0 (Linux; Android 9; Unspecified Device) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 miHoYoBBS/2.2.0";
            myRequest.Referer = "https://webstatic.mihoyo.com/app/community-game-records/index.html?v=6";
            myRequest.Accept = "application/json, text/plain, */*";


            myRequest.Method = "GET";

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            string content = reader.ReadToEnd();

            reader.Close();

            Console.WriteLine(content);

            return JsonMapper.ToObject(content);
            return "";
        }
    }
}
