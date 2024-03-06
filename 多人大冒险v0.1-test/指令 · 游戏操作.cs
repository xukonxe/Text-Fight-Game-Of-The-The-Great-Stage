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
using CMKZ;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class LocalStorage {
        public static 系统消息类 检查开始() {
            if (游戏系统.世界 == null) {
                return new 系统消息类("游戏尚未开始。请联系管理员", false);
            }
            return new 系统消息类("", true);
        }
        public static 系统消息类 检查账号密码(string 账号, string 密码) {
            if (!游戏系统.验证(账号)) {
                return new 系统消息类("此账号名不存在。", false);
            }
            if (游戏系统.验证(账号, 密码)) {
                return new 系统消息类("", true);
            } else {
                return new 系统消息类($"用户 {账号} 的密码不正确。", false);
            }
        }
        public static 系统消息类 检查出生(string 账号) {
            var 当前地图 = 游戏系统.当前位置(游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号));
            if (当前地图 == null) {
                return new 系统消息类("您还未进入游戏，请先进入游戏主城", false);
            }
            return new 系统消息类("", true);
        }
        public static 系统消息类 查找建筑(地图类 地图, string 建筑名称) {
            if (地图.建筑物.Exists(i => i.名称 == 建筑名称)) {
                return new 系统消息类("", true);
            } else {
                return new 系统消息类($"在 {地图.名称} 中未找到 {建筑名称}", false);
            }
        }
        public class 系统消息类 {
            public string 消息;
            public bool isSuccess;
            public 系统消息类(string 消息, bool isSuccess) {
                this.消息 = 消息;
                this.isSuccess = isSuccess;
            }
        }
    }
    public class 注册账号指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (游戏系统.世界 == null) {
                return "游戏尚未开始。请联系管理员";
            }
            if (游戏系统.验证(账号)) {
                return "此账号名已存在。";
            }
            if (密码.Length < 6) {
                return "密码长度不得小于6。";
            }
            游戏系统.注册(账号, 密码);
            return "注册成功。\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
        }
    }
    public class 查询指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            return 控制台系统.查询游戏信息(new 账号信息() { 账号 = 账号, 密码 = 密码 });
        }
    }
    public class 查询账户信息指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(D1, D2).isSuccess) return 检查账号密码(D1, D2).消息;
            //返回当前登录的账号、游戏时长、角色位置信息
            var 玩家 = 游戏系统.获取玩家(new 账号信息() { 账号 = D1, 密码 = D2 });
            var A = new StringBuilder();
            A.AppendLine("======控制台消息======");
            A.AppendLine($"当前登录账号：{D1}");
            A.AppendLine($"注册时间：{玩家.注册时间}");
            if (游戏系统.当前位置(玩家) == null) {
                A.AppendLine($"角色位置：无位置");
            } else {
                A.AppendLine($"角色位置：{游戏系统.当前位置(玩家).名称}");
            }
            A.AppendLine($"游戏时长：{DateTime.Now - 玩家.注册时间}");
            A.AppendLine("======================");
            return A.ToString();
        }
    }
    public class 进入主城指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 主城 = 游戏系统.世界.主城;
            游戏系统.进入地图(new 账号信息() { 账号 = 账号, 密码 = 密码 }, 主城);
            //在未出生玩家中移除
            游戏系统.世界.未出生玩家.RemoveAll(i => i.账号数据.账号 == 账号);
            //检测是否已经在主城
            if (游戏系统.当前位置(玩家) == 主城) {
                return "您已经进入主城！欢迎来到大冒险！\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
            }
            return "进入主城失败。异常0001错误，请联系管理员";
        }
    }
    public class 移动指令 : I指令 {
        public string D1;
        public string D2;
        public Vector2Int D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 目标坐标 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 当前地图 = 游戏系统.当前位置(游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号));
            if (游戏系统.移动(new 账号信息() { 账号 = 账号, 密码 = 密码 }, 目标坐标)) {
                return $"角色 {账号} 成功移动到 {目标坐标}。";
            } else {
                var 地图大小 = 当前地图.地图大小;
                var 最大坐标 = new Vector2Int(地图大小 / 2, 地图大小 / 2);
                var 最小坐标 = new Vector2Int(-地图大小 / 2, -地图大小 / 2);
                return $"移动失败。 {当前地图.名称} 的坐标范围为 {最大坐标} ~ {最小坐标}";
            }
        }
    }
    public class 进入建筑指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 建筑名称 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 当前地图 = 游戏系统.当前位置(游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号));
            var 判断1 = 查找建筑(当前地图, 建筑名称);
            if (!判断1.isSuccess) return 判断1.消息;
            var 建筑 = 当前地图.建筑物.Find(i => i.名称 == 建筑名称);
            游戏系统.进入建筑(new 账号信息() { 账号 = 账号, 密码 = 密码 }, 建筑);
            return $"进入 {建筑名称} 成功。\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
        }
    }
    public class 使用家具指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 家具名称 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            家具类 家具;
            if (玩家.正在建筑内) {
                var 当前建筑 = 玩家.所在建筑;
                if (当前建筑 == null) {
                    return "您不在建筑内，无法使用家具。";
                }
                家具 = 当前建筑.坐标系.所有物体<家具类>().Find(i => i.名称 == 家具名称);
            } else {
                家具 = 当前地图.坐标系.所有物体<家具类>().Find(i => i.名称 == 家具名称);
            }
            if (家具 == null) {
                return $"未找到 {家具名称}\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
            }
            if (家具 is IUseable) {
                return 游戏系统.使用(new 账号信息() { 账号 = 账号, 密码 = 密码 }, 家具 as IUseable);
            } else {
                return $"家具 {家具名称} 无法使用。";
            }
        }
    }
    public class 购买商品指令 : I指令 {
        public string D1;
        public string D2;
        public 道具类 D3;
        public int D4;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 账号信息 = new 账号信息() { 账号 = 账号, 密码 = 密码 };
            var 玩家 = 游戏系统.获取玩家(账号信息);
            if (玩家.正使用 is 商店柜台 i) {
                if (D3 is IBuyAble 购买的道具) {
                    return 游戏系统.购买(账号信息, i, 购买的道具, D4).消息;
                } else {
                    return "此物品不可购买。";
                }
            } else {
                return "您并未使用柜台，无法购买商品。";
            }
        }
    }
    public class 出售商品指令 : I指令 { 
        public string D1;
        public string D2;
        public 道具类 D3;
        public int D4;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 账号信息 = new 账号信息() { 账号 = 账号, 密码 = 密码 };
            var 玩家 = 游戏系统.获取玩家(账号信息);
            if (玩家.正使用 is 商店柜台 i) {
                if (D3 is IBuyAble 出售的道具) {
                    return 游戏系统.出售(账号信息, i, 出售的道具, D4).消息;
                } else {
                    return "此物品不可出售。";
                }
            } else {
                return "您并未使用柜台，无法出售商品。";
            }
        }
    }
}
