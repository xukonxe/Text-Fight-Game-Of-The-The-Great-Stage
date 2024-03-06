using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;
using static CMKZ.C02.多人大冒险01.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class LocalStorage {
        public static string 欢迎文本() => $"欢迎来到多人大冒险 版本：{游戏系统.版本}\n输入“帮助”来获取帮助";
        public static void MainStart() {
            控制台系统.启动();
            通信系统.启动服务器();
            游戏系统.世界 = 存档系统.Try读取存档();
            if (游戏系统.世界 != null) {
                Print($"自动读取：存档 {游戏系统.世界.存档名} 已读取。");
            }
            bool 运行 = true;
            while (运行) {
                Print(控制台系统.控制台执行(Console.ReadLine()));
            }
        }
    }
}
