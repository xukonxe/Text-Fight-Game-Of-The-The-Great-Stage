using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;
using CMKZ;

namespace CMKZ.C02.多人大冒险01 {
    public class 创建存档指令 : I指令 {
        public string D1;
        public string Invoke() {
            var 新世界 = new 世界类(D1);
            游戏系统.世界 = 新世界;
            var 主城商店 = new 主城商店();
            主城商店.坐标系.Add(1, 1, new 商店柜台());
            主城商店.坐标系.所有物体<商店柜台>()[0].商品列表.Add(new 铁质长剑(), 100);
            游戏系统.世界.主城.坐标系.Add(1, 1, 主城商店);
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
