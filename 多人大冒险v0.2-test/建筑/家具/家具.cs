using CMKZ.C02.多人大冒险01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMKZ;
using static CMKZ.LocalStorage;
using static CMKZ.C02.多人大冒险01.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
   
    public interface IShop {
        public KeyValueList<IBuyAble, long> 商品列表 { get; set; }
        public abstract bool CanBuy(角色类 buyer);
        public abstract bool CanSell(角色类 seller);
        public abstract string Buy(角色类 buyer, (IBuyAble 商品, long 数量) 商品信息);
        public abstract string Sell(角色类 seller, (IBuyAble 商品, long 数量) 商品信息);
    }
    public interface I可搬运 { }
    public interface IUseable {
        public List<角色类> 正使用的角色 { get; set; }
        public int 最大容纳人数 { get; set; }
        public 系统消息类 CanUse(角色类 user);
        public string 使用(角色类 user);
        public string 离开(角色类 user);
    }
    public abstract class 家具类 : I实体 {
        public string 名称 => this.GetType().Name;
    }
    public abstract class 床 : 家具类, IUseable {
        public List<角色类> 正使用的角色 { get; set; } = new();
        public abstract int 最大容纳人数 { get; set; }
        public 系统消息类 CanUse(角色类 user) {
            if (正使用的角色.Count >= 最大容纳人数) {
                return new 系统消息类("床上人数已满", false);
            }
            return new 系统消息类("", true);
        }
        public string 使用(角色类 user) {
            user.正使用 = this;
            //如果床上有其他人
            var 其他人 = 正使用的角色.FindAll(i => i != user);
            if (其他人.Count != 0) {
                return $"您已上床，要睡觉了。同床还有：{其他人.Select(i => i.名称).Join("、")}，输入【做爱】可和同床的人做爱";
            }
            return "您已上床，要睡觉了";
        }
        public string 离开(角色类 user) {
            user.正使用 = null;
            //如果床上有其他人
            var 其他人 = 正使用的角色.FindAll(i => i != user);
            if (其他人.Count != 0) {
                return "您已下床";
            }
            return $"您已下床，您离开了床上的 {其他人.Select(i => i.名称).Join("、")}！";
        }
        public string 做爱(角色类 user) {
            user.正使用 = this;
            //如果床上人数大于两个人
            if (正使用的角色.Count < 2) {
                return "床上人数不足,无法做爱";
            }
            正使用的角色.ForEach(i => {
                正使用的角色.ForEach(j => {
                    i.添加物品(new 精华(j.名称));
                    i.添加物品(new 精华(j.名称));
                    i.添加物品(new 精华(j.名称));
                });
            });
            var 其他人 = 正使用的角色.FindAll(i => i != user);
            return $"您正在和 {其他人.Select(i => i.名称).Join("、")} 做爱！";
        }
    }
    public abstract class 可坐家具 : 家具类, IUseable{
        public List<角色类> 正使用的角色 { get; set; } = new();
        public abstract int 最大容纳人数 { get; set; }
        public 系统消息类 CanUse(角色类 user) {
            if (正使用的角色.Count >= 最大容纳人数) {
                return new 系统消息类("椅子上人数已满", false);
            }
            return new 系统消息类("", true);
        }
        public string 使用(角色类 user) {
            user.正使用 = this;
            正使用的角色.Add(user);
            return "您坐下了";
        }
        public string 离开(角色类 user) {
            user.正使用 = null;
            正使用的角色.Remove(user);
            return "您站起了";
        }
    }
    public class 商店柜台 : 家具类, IUseable, IShop {
        public KeyValueList<IBuyAble, long> 商品列表 { get; set; } = new();
        public List<角色类> 正使用的角色 { get; set; } = new();
        public int 最大容纳人数 { get; set; } = 100000;
        public 系统消息类 CanUse(角色类 user) {
            if (正使用的角色.Count >= 最大容纳人数) {
                return new 系统消息类("柜台上人数已满", false);
            }
            return new 系统消息类("", true);
        }
        public 商店柜台() {
            商品列表.Add(new 纪念堂建筑蓝图(), 1000);
            商品列表.Add(new 原木(), 1000);
            商品列表.Add(new 石块(), 1000);
            商品列表.Add(new 铁锭(), 1000);
        }
        public string 使用(角色类 user) {
            user.正使用 = this;
            return 角色打开柜台(user);
        }
        public long 获取商品数量(IBuyAble 商品) {
            return 商品列表.ContainsKey(商品) ? 商品列表[商品] : 0;
        }
        public string 角色打开柜台(角色类 user) {
            var A = new StringBuilder();
            A.AppendLine("======控制台信息======");
            A.AppendLine("欢迎光临主城商店！");
            A.AppendLine("您需要什么？");
            A.AppendLine("------商品列表------");

            foreach (var i in 商品列表) {
                A.AppendLine($"{i.Key.名称}：{i.Key.价格}金币 剩余{i.Value}");
            }
            A.AppendLine("--------------------");
            A.AppendLine("指令提示：");
            A.AppendLine("【购买商品 商品名 数量】");
            A.AppendLine("【退出家具】");
            A.AppendLine("======================");
            return A.ToString();
        }
        public string 离开(角色类 user) {
            user.正使用 = null;
            return "您离开了商店柜台\n";
        }
        public string Buy(角色类 buyer, (IBuyAble 商品, long 数量) 商品信息) {
            var 物品数量 = 获取商品数量(商品信息.商品);
            if (物品数量 < 商品信息.数量) {
                return "商品数量不足";
            }
            var 总价 = 商品信息.商品.价格 * 商品信息.数量;
            if (buyer.金币 < 总价) {
                return "金币不足";
            }
            buyer.金币 -= 总价;
            执行X次((int)商品信息.数量, () => {
                buyer.背包.Add(商品信息.商品 as 道具类);
                商品列表[商品信息.商品] -= 商品信息.数量;
            });
            return $"购买 {商品信息.数量} 个 {商品信息.商品.名称} 成功。\n指令提示 【查询背包】";
        }
        public string Sell(角色类 seller, (IBuyAble 商品, long 数量) 商品信息) {
            var 总价 = 商品信息.商品.价格 * 商品信息.数量;
            seller.金币 += 总价;
            执行X次((int)商品信息.数量, () => {
                seller.背包.Remove(商品信息.商品 as 道具类);
                商品列表[商品信息.商品] += 商品信息.数量;
            });
            return $"出售 {商品信息.数量} 个 {商品信息.商品} 成功";
        }
        public bool CanBuy(角色类 buyer) => true;
        public bool CanSell(角色类 seller) => true;
    }
    public class 木框海报 : 家具类, I可搬运 {
        public string? 图片内容;
        public 木框海报() { }
        public 木框海报(string 图片内容) {
            this.图片内容 = 图片内容;
        }
        //public override string 使用消息() {
        //    //如果图片内容为空，就返回默认消息
        //    if (图片内容 == null) {
        //        return "这是一张木框海报，上面什么都没有";
        //    }
        //    return "这是一张木框海报，上面画着：" + 图片内容;
        //}
    }
    public class 大幅相片 : 木框海报 {
        public 大幅相片() { }
        public 大幅相片(string 图片内容) : base(图片内容) { }
    }
    public class 花圈 : 家具类, I可搬运 { }
    public class 沙发椅 : 可坐家具 {
        public override int 最大容纳人数 { get; set; } = 3;
    }
    public class 双人床 :  床 {
        public override int 最大容纳人数 { get; set; } = 2;
    }
    public class 淋浴房 : 家具类, IUseable {
        public List<角色类> 正使用的角色 { get; set; } = new();
        public int 最大容纳人数 { get; set; } = 1;
        public 系统消息类 CanUse(角色类 user) {
            if (正使用的角色.Count >= 最大容纳人数) {
                return new 系统消息类($"淋浴房人数已满,当前正在使用：{正使用的角色.Select(i => i.名称).Join("、")}", false);
            }
            return new 系统消息类("", true);
        }
        public string 使用(角色类 user) {
            user.正使用 = this;
            return "您正在洗澡";
        }
        public string 离开(角色类 user) {
            user.正使用 = null;
            var 污渍数量= user.获取物品数量<I污渍>();
            user.移除所有<I污渍>();
            return $"您洗完澡了，洗掉了{污渍数量}个污渍！";
        }
    }
}
