using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;
using static CMKZ.C02.多人大冒险01.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class 游戏系统 {
        public static 世界类? 世界;
        public static string 版本=>"v0.01-Beta";
        public static void 注册(string 账号, string 密码) {
            var A = new 账号信息() { 账号 = 账号, 密码 = 密码 };
            世界.未出生玩家.Add(new 角色类() { 账号数据 = A });
        }
        public static bool 验证(string K) => 世界.所有玩家.Any(i => i.账号数据.账号 == K);
        public static bool 验证(string K, string P) => 世界.所有玩家.Any(i => i.账号数据.账号 == K && i.账号数据.密码 == P);
        public static bool 验证(账号信息 K) => 世界.所有玩家.Any(i => i.账号数据.验证(K));
        public static bool 验证(this 账号信息 A, 账号信息 B) => A.账号 == B.账号 && A.密码 == B.密码;
        public static 角色类 获取玩家(账号信息 K) {
            var A = 世界.所有玩家;
            //Print($"XXXX:{世界.所有玩家.ToString(t => t.账号数据.账号)}");
            if (A == null) {
                return null;
            }
            return A.Find(i => i.账号数据.账号 == K.账号 && i.账号数据.密码 == K.密码);
        }
        public static 地图类 当前位置(角色类 玩家) => 世界.所有地图.Find(i => i.Contains(玩家));
        public static List<角色类> 附近玩家(this 角色类 玩家) => 当前位置(玩家)?.坐标系.所有物体<角色类>().Where(i => i != 玩家).ToList();
        public static List<建筑类> 附近建筑(this 角色类 玩家) => 当前位置(玩家)?.坐标系.所有物体<建筑类>().ToList();
        public static List<家具类> 附近家具(this 角色类 玩家) => 当前位置(玩家)?.坐标系.所有物体<家具类>().ToList();
        public static void 进入地图(账号信息 K, 地图类 目标地图,方向 D=方向.南) {
            if (!验证(K)) return;
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            var 所在地图 = 世界.所有地图.Find(i => i.坐标系.物体坐标.ContainsKey(玩家));
            玩家.正坐在?.站起(玩家);
            玩家.所在建筑?.离开(玩家);
            所在地图?.角色离开(玩家);
            目标地图.吸收角色(D,玩家);
        }
        public static bool 移动(账号信息 K, Vector2Int P) {
            if (!验证(K)) return false;
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            var 所在地图 = 世界.所有地图.Find(i => i.坐标系.物体坐标.ContainsKey(玩家));
            玩家.正坐在?.站起(玩家);
            玩家.所在建筑?.离开(玩家);
            return 所在地图.角色移动(玩家, P);
        }
        public static void 进入建筑(账号信息 K, 建筑类 目标建筑) {
            if (!验证(K)) return;
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            var 所在地图 = 世界.所有地图.Find(i => i.Contains(玩家));
            if (!所在地图.Contains(目标建筑)) {
                return;
            };
            玩家.正坐在?.站起(玩家);
            目标建筑.进入(玩家);
        }
        public static void 坐下(账号信息 K, ISitable 目标家具) {
            if (!验证(K)) return;
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            var 所在地图 = 世界.所有地图.Find(i => i.Contains(玩家));
            if (玩家.所在建筑 != null) {
                var 可坐下的家具 = 玩家.所在建筑.坐标系.所有物体<ISitable>();
                if (可坐下的家具.Contains(目标家具)) {
                    目标家具.坐下(玩家);
                }
            }
        }
        public static void 站起(账号信息 K) {
            if (!验证(K)) return;
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            玩家.正坐在?.站起(玩家);
        }
        public static string 使用(账号信息 K, IUseable 目标家具) {
            if (!验证(K)) return "账号信息错误";
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            return 目标家具.使用(玩家);
        }
        public static 系统消息类 购买(账号信息 K,商店柜台 柜台 ,IBuyAble 目标物品,long 数量) {
            if (!验证(K)) return new 系统消息类("账号信息错误", false);
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            柜台.Buy(玩家, (目标物品, 数量));
            return new 系统消息类("购买成功", true);
        }
        public static 系统消息类 出售(账号信息 K, 商店柜台 柜台, IBuyAble 目标物品, long 数量) {
            if (!验证(K)) return new 系统消息类("账号信息错误", false);
            var 玩家 = 世界.所有玩家.Find(i => i.账号数据.验证(K));
            柜台.Sell(玩家, (目标物品, 数量));
            return new 系统消息类("出售成功", true);
        }
    }
    public partial class 世界类 {
        public string 存档名;
        public 世界类(string name) {
            存档名 = name;
        }
    }
    public partial class 世界类 {
        public List<角色类> 未出生玩家 = new();
        public 地图类 主城 = new 主城类();
        public List<地图类> 所有地图 {
            get {
                var A = new List<地图类>();
                A.Add(主城);
                A.AddRange(主城.获得所有子地图());
                return A;
            }
        }
        public List<角色类> 所有玩家 {
            get {
                var 玩家 = new List<角色类>();
                foreach (var 地图 in 所有地图) {
                    玩家.AddRange(地图.坐标系.所有物体<角色类>());
                }
                玩家.AddRange(未出生玩家);
                return 玩家;
            }
        }

    }
    public class 账号信息 {
        public string 账号;
        public string 密码;
    }
}
