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
                return new 系统消息类(
                    $"========Warning 警告========\n" +
                    $"此账号名不存在。\n" +
                    $"可能原因：服务器存档重置\n" +
                    $"您本地储存的账户是\n" +
                    $"【{账号}】 ，\n" +
                    $"密码是\n" +
                    $"【{密码}】。\n" +
                    $"输入【注册】来使用此账户和密码注册账号。" +
                    $"若要修改账户密码，请修改您电脑上的 C:/C02 文件夹下的用户信息。\n" +
                    $"=============================", false);
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
        public static 系统消息类 查找家具(地图类 地图, string 家具名称) {
            if (地图.坐标系.所有物体<家具类>().Exists(i => i.名称 == 家具名称)) {
                return new 系统消息类("", true);
            } else {
                return new 系统消息类($"在 {地图.名称} 中未找到 {家具名称}", false);
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
        public class 玩家列表消息类 {
            public List<角色类> 玩家列表;
            public string 消息;
            public 玩家列表消息类(List<角色类> 玩家列表, string 消息) {
                this.玩家列表 = 玩家列表;
                this.消息 = 消息;
            }
        }
    }
    public class 注册指令 : I指令 {
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
            return $"注册成功。\n当前游戏版本：{游戏系统.版本}\n当前为测试版，请提供游戏建议\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
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
            //获取游戏时长，转换为无小数时间
            var 游戏时长=DateTime.Now - 玩家.注册时间;
            int days = (int)游戏时长.TotalDays;
            int hours = (int)游戏时长.TotalHours % 24;
            int minutes = (int)游戏时长.TotalMinutes % 60;
            A.AppendLine($"游戏时长：{days}天{hours}小时{minutes}分");
            A.AppendLine("======================");
            A.AppendLine("输入【帮助】以查看帮助");
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
            return new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke() + "\n" + 建筑.进入消息();
        }
    }
    public class 退出建筑指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            if (玩家.正在建筑内) {
                var 当前建筑 = 玩家.所在建筑;
                if (当前建筑 == null) {
                    return "您不在建筑内，无法退出。";
                }
                游戏系统.退出建筑(new 账号信息() { 账号 = 账号, 密码 = 密码 });
                return $"成功退出 {当前建筑.名称}。\n" + new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
            } else {
                return "您并未在建筑内，无法退出。";
            }
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
                return $"家具 {家具名称} 不是可使用的家具。";
            }
        }
    }
    public class 退出家具指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            if (玩家.正使用 != null) {
                return 玩家.正使用.离开(玩家)+new 查询指令() { D1 = 账号, D2 = 密码 }.Invoke();
            }
            return "您并未使用家具，无法退出。";
        }
    }
    public class 购买商品指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
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
                var 要购买的商品 = i.商品列表.Find(j => j.Key.名称 == D3);
                if (要购买的商品 != null) {
                    if (i.CanBuy(玩家)) {
                        return i.Buy(玩家, (要购买的商品.Key, D4));
                    } else { 
                        return "您无法购买此商品。";
                    }
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
        public string D3;
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
                var 要出售的商品 = 玩家.背包.OfType<IBuyAble>().Find(j => j.名称 == D3);
                if (要出售的商品 != null) {
                    if (i.CanSell(玩家)) {
                        return i.Sell(玩家, (要出售的商品, D4));
                    } else {
                        return "您无法出售此商品。";
                    }
                } else {
                    return "您的背包中未找到此物品。";
                }
            } else {
                return "您并未使用柜台，无法出售商品。";
            }
        }
    }

    public class 搬起物品指令 : I指令 {
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
            if (玩家.正在搬运 != null) {
                return $"您正在搬搬运物品{玩家.正在搬运}";
            }
            var 当前地图 = 游戏系统.当前位置(玩家);
            var 当前坐标 = 当前地图.坐标系.物体坐标[玩家];
            //检测当前坐标是否有可搬动的物体
            var 可搬运物体 = 当前地图.坐标系[当前坐标.X, 当前坐标.Y].OfType<I可搬运>();
            if (可搬运物体.Count() != 0) {
                return 玩家.搬运(可搬运物体.First());
            } else {
                return "当前位置无可搬运的物品。";
            }
        }
    }
    public class 放下物品指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 账号信息 = new 账号信息() { 账号 = 账号, 密码 = 密码 };
            var 玩家 = 游戏系统.获取玩家(账号信息);
            return 玩家.放下();
        }
    }
    public class 使用蓝图指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 蓝图名称 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            var 玩家坐标 = 当前地图.坐标系.物体坐标[玩家];
            //查找玩家背包里是否有此蓝图
            var 蓝图 = 玩家.背包.Find(i => i.名称 == 蓝图名称);
            if (蓝图 == null) {
                return $"您的背包中未找到 {蓝图名称}";
            }
            if (蓝图 is I蓝图 i) {
                return i.建造(玩家, 当前地图, 玩家坐标);
            } else {
                return "此物品不是蓝图。";
            }
        }
    }
    public class 设置纪念指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string D4;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 纪念内容 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            var 纪念建筑 = 当前地图.建筑物.Find(i => i is 纪念堂);
            if (纪念建筑 == null) {
                return "当前位置不存在纪念堂";
            }
            if (纪念建筑 is 纪念堂 i) {
                return i.设置纪念(D4);
            } else {
                return "此建筑不存在纪念堂";
            }

        }
    }
    //对当前位置的木框海报，可以设置其内容
    public class 修改图片内容指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string D4;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 海报名称 = D3;
            var 海报内容 = D4;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            var 玩家坐标 = 当前地图.坐标系.物体坐标[玩家];
            var 所有海报 = 当前地图.坐标系.所有物体<木框海报>();
            if (所有海报.Count == 0) {
                return "当前位置不存在可设置图片内容的家具";
            }
            if (所有海报.Exists(i => i.名称 == 海报名称)) {
                var 海报 = 所有海报.Find(i => i.名称 == 海报名称);
                海报.图片内容 = 海报内容;
                return "修改成功";
            } else {
                return "未找到此海报";
            }
        }
    }

    public class 查询背包指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var A = new StringBuilder();
            A.AppendLine("======背包物品======");
            A.AppendLine($"当前登录账号：{账号}");
            A.AppendLine($"注册时间：{玩家.注册时间}");
            A.AppendLine($"背包物品：");
            foreach (var i in 玩家.背包) {
                A.AppendLine(i.名称);
            }
            A.AppendLine("======================");
            return A.ToString();
        }
    }
    //public class 修改昵称指令 {  }
    //public class 修改密码指令 { }
    public class 查询角色信息指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;

            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == D3);
            if (玩家 == null) {
                return "未找到此角色";
            }
            var 当前地图 = 游戏系统.当前位置(玩家);
            var 玩家坐标 = 当前地图.坐标系.物体坐标[玩家];
            var A = new StringBuilder();
            A.AppendLine("======角色信息======");
            A.AppendLine($"当前登录账号：{账号}");
            A.AppendLine($"注册时间：{玩家.注册时间}");
            A.AppendLine($"角色位置：{当前地图.名称} {玩家坐标}");
            A.AppendLine($"游戏时长：{DateTime.Now - 玩家.注册时间}");
            A.AppendLine($"当前血量：{玩家.生命}/{玩家.生命.MaxValue}");
            A.AppendLine($"玩家金币：{玩家.金币}");
            A.AppendLine("======================");
            return A.ToString();
        }
    }

    //查询聊天室建筑内的聊天记录
    public class 查询聊天记录指令 : I指令 { 
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            //检查玩家当前是否在聊天室内
            if (玩家.正在建筑内) {
                var 当前建筑 = 玩家.所在建筑;
                if (当前建筑 is IChatRoom i) {
                    return i.聊天记录;
                } else {
                    return "当前建筑不是有聊天记录的建筑，无法查询聊天记录。";
                }
            } else {
                return "您不在有聊天记录的建筑内，无法查询聊天记录。";
            }
        }
    }
    public class 说话指令 : I指令 {
        public string D1;
        public string D2;
        public string D3;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            var 说话内容 = D3;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            var 当前地图 = 游戏系统.当前位置(玩家);
            //检查玩家当前是否在聊天室内
            if (玩家.正在建筑内) {
                var 当前建筑 = 玩家.所在建筑;
                if (当前建筑 is IChatRoom i) {
                    return i.说话(玩家, 说话内容);
                } else {
                    return "当前建筑不是有聊天记录的建筑，无法说话。";
                }
            } else {
                return "您不在有聊天记录的建筑内，无法说话。";
            }
        }
    }
    //如果正在使用床，那么执行床的做爱函数
    public class 做爱指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var 账号 = D1;
            var 密码 = D2;
            if (!检查开始().isSuccess) return 检查开始().消息;
            if (!检查账号密码(账号, 密码).isSuccess) return 检查账号密码(账号, 密码).消息;
            if (!检查出生(账号).isSuccess) return 检查出生(账号).消息;
            var 玩家 = 游戏系统.世界.所有玩家.Find(i => i.账号数据.账号 == 账号);
            if (玩家.正使用 is 床 i) {
                return i.做爱(玩家);
            } else {
                return "您并未使用床，无法做爱。";
            }
        }
    }
}
