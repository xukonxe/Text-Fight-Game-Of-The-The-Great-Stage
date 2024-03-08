using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMKZ;
using static CMKZ.LocalStorage;
using static CMKZ.C02.多人大冒险01.LocalStorage;
using CMKZ.C02;
using static CMKZ.C02.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class LocalStorage { }
    public static partial class 通信系统 { 
        public static TcpServer 服务器 = new TcpServer(16299);
        public static string 启动服务器() {
            服务器.Version="v0.02-Test";
            服务器.OnConnect+= (e) =>
                Print($"[{DateTime.Now}]>\n 已连接");
            服务器.OnDisconnect += (e) => {
                Print($"[{DateTime.Now}]>\n 已断开");
            };
            服务器.OnReceive += (e, C) => { };
            服务器.OnRead["用户指令"] = (t, c) => {
                var 账号信息= t["用户"].JsonDeserialize<账号信息>();
                var 用户指令= t["指令"];
                var A = 用户指令.Split(' ').ToList();
                //在[0]之后插入用户名和密码
                A.Insert(1, 账号信息.密码);
                A.Insert(1, 账号信息.账号);
                var str = A.ToString(t => t.ToString() + " ");
                //移除最后一个空格
                str = str.Remove(str.Length - 1);
                Print($"[{DateTime.Now}]>玩家 {账号信息.账号} 输入了【{str}】");
                return new Dictionary<string, string> {
                    { "消息", $"[{DateTime.Now}]>\n"+控制台系统.控制台执行(str) },
                };
            };
            服务器.Start();
            //当控制台退出时，关闭服务器
            Console.CancelKeyPress += (e, c) => { };
            return "服务器已启动";
        }
    }
}
