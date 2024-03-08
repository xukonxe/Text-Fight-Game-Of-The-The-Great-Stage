using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;
using static CMKZ.C02.多人大冒险01.LoaclStorage;
using CMKZ;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class LoaclStorage {
        public static string 主城商店柜台商品列表(世界类 世界) {
            var 主城商店 = 世界.主城.坐标系.所有物体<主城商店>().FirstOrDefault();
            var 商店柜台 = 主城商店.坐标系.所有物体<商店柜台>().FirstOrDefault();
            var 商品列表 = 商店柜台.商品列表;
            return 商品列表.JsonSerialize();
        }
    } 
    public class 创建存档指令 : I指令 {
        public string D1;
        public string Invoke() {
            var 新世界 = 世界类.默认模版(D1);
            return 存档系统.保存存档(新世界);
        }
    }
    public class 保存存档指令 : I指令 {
        public string Invoke() {
            if (游戏系统.世界 == null) {
                return "当前没有存档";
            }
            return 存档系统.保存存档(游戏系统.世界);
        }
    }
    public class 读取存档指令 : I指令 {
        public string D1;
        public string Invoke() {
            var 存档名 = D1;
            var 存档 = 存档系统.读取存档(存档名);
            if (存档 == null) {
                return "存档不存在";
            }
            游戏系统.世界 = 存档;
            return $"存档 {存档名} 已读取。";
        }
    }
}
