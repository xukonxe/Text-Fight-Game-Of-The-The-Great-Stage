using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMKZ;
using static CMKZ.LocalStorage;
using CMKZ.C02.多人大冒险01_命令行客户端;
using static CMKZ.C02.多人大冒险01_命令行客户端.LocalStorage;

namespace CMKZ.C02.多人大冒险01_命令行客户端 {
    public static partial class LocalStorage {
        public static Dictionary<string, Func<string[],string>> 指令 = new();
        public static 账号信息 账号;
        public static string 本地账号路径 = "C:/C02/大冒险用户信息.txt";
        public static void MainStart() {
            Print("正在连接服务器……");
            Print(通信系统.启动客户端());
            注册指令();
            //先检查是否存在本地文件的路径，如果存在，那么读取。
            if (FileExists(本地账号路径)) {
                账号 = FileRead<账号信息>(本地账号路径);
            }
            //读取之后如果账号和密码合法，即账号非空，密码位数大于等于6，那么读取。
            if (账号 != null && (账号.账号 == "" || 账号.密码.Count() < 6)) {
                账号 = null;
            }
            if (账号 == null) {
                void 注册账号() {
                    Print("第一次登录，请输入账号和密码以注册！");
                    账号 = new 账号信息();
                    Print("请输入账号：");
                    账号.账号 = Console.ReadLine();
                    Print("请设置密码：");
                    账号.密码 = Console.ReadLine();
                    Print($"您输入的账号是：{账号.账号}，密码是：{账号.密码}，您确定吗？是/否");
                    if (Console.ReadLine() == "是") {
                        FileWrite(本地账号路径, 账号);
                    } else {
                        注册账号();
                    }
                }
                注册账号();
                Print("正在请求服务器……");
                通信系统.发送指令($"注册账号", 账号);
            } else {
                Print("X");
                通信系统.发送指令($"查询账户信息", 账号);
                通信系统.发送指令($"帮助", 账号);
            }
            bool 运行 = true;
            while (运行) {
                匹配指令(Console.ReadLine());
            }
        }
        public static string 匹配指令(string X) {
            foreach (var B in 指令) {
                if (X.StartsWith(B.Key)) {
                    return B.Value(X.Split(' '));
                }
            }
            通信系统.发送指令(X, 账号);
            return "";
        }
        public static void 注册指令() {
            //指令["注册账号"]=t=> {
            //    if (t.Length != 3) return "错误，注册账号指令格式错误";
            //    if (t[1] == null) return "错误，账号不能为空";
            //    if (t[2] == null) return "错误，密码不能为空";
            //    账号 = new CMKZ.C02.多人大冒险01.账号信息 { 账号 = t[1], 密码 = t[2] };
            //    通信系统.发送指令($"注册账号 {t[1]} {t[2]}");
            //    return "";
            //};
        }
        public static void 输出(string X) { }
        public static void Print(string message) {
            Console.WriteLine(message);
        }
    }
    public static class 通信系统 {
        public static TcpClient 游戏端 = new TcpClient("127.0.0.1:16299");
        public static string 启动客户端() {
            游戏端.OnConnect += () => Print($"游戏服务器已连接");
            游戏端.OnDisconnect += (e) => Print($"游戏服务器 {e.IP} 已断开");
            游戏端.OnReceive += (e, C) => { };
            try {
                游戏端.Start();
            } catch { 
                return "错误，游戏主服务器未启动";
            }
            return "成功连接";
        }
        public static void 发送指令(string X, 账号信息 账号) {
            游戏端.Send(new Dictionary<string, string> { { "标题","用户指令"}, { "指令", X } ,{ "用户", 账号.JsonSerialize() } }, t => {
                Print(t["消息"]);
            });
        }
    }
    public class 账号信息 {
        public string 账号;
        public string 密码;
    }
}
