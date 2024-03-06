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
    public interface IUseable {
        public abstract event Action<角色类> OnUse;
        public string 使用(角色类 user);
        public string 离开(角色类 user);
    }
    public interface IShop {
        public KeyValueList<IBuyAble, long> 商品列表 { get; set; }
        public abstract bool CanBuy(角色类 buyer);
        public abstract bool CanSell(角色类 seller);
        public abstract string Buy(角色类 buyer, (IBuyAble 商品, long 数量) 商品信息);
        public abstract string Sell(角色类 seller, (IBuyAble 商品, long 数量) 商品信息);
    }
    public interface ISitable {
        public abstract bool CanSit(角色类 user);
        public void 坐下(角色类 user) {
            user.正坐在 = this;
        }
        public void 站起(角色类 user) {
            user.正坐在 = null;
        }
    }
    public abstract class 家具类 : I实体 {
        public abstract string 名称 { get; }
    }
    public class 商店柜台 : 家具类, IUseable, IShop {
        public override string 名称=> "商店柜台";
        public KeyValueList<IBuyAble, long> 商品列表 { get; set; } = new();
        public event Action<角色类> OnUse;
        public 商店柜台() { }
        public string 使用(角色类 user) {
            OnUse?.Invoke(user);
            user.正使用 = this;
            return 角色打开柜台(user);
        }
        public long 获取商品数量(IBuyAble 商品) {
            return 商品列表.ContainsKey(商品) ? 商品列表[商品] : 0;
        }
        public string 角色打开柜台(角色类 user) {
            var A=new StringBuilder();
            A.AppendLine("======控制台信息======");
            A.AppendLine("欢迎光临主城商店！");
            A.AppendLine("您需要什么？");
            A.AppendLine("------商品列表------");
            执行X次(商品列表.Count, i => {
                var 商品 = 商品列表.ElementAt(i);
                A.AppendLine($"{i + 1}.{商品.Key.名称} 价格：{商品.Value}");
            });
            A.AppendLine("--------------------");
            A.AppendLine("请输入【购买商品 商品名】来购买");
            A.AppendLine("======================");
            return A.ToString();
        }
        public string 离开(角色类 user) {
            user.正使用 = null;
            return "您离开了商店";
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
            return $"购买 {商品信息.数量} 个 {商品信息.商品} 成功";
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
}
