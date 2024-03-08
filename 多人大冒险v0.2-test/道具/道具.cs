using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public interface IBuyAble {
        public string 名称 { get; }
        public double 价格 { get; set; }
    }
    public interface I污渍 { }
    public interface I可携带 {
        public string 名称 { get; }
    }
    public interface I所有者 {
        public string 所有者 { get; set; }
    }
    public interface I蓝图 {
        public string 名称 { get; }
        public KeyValueList<IBuyAble, long> 所需材料列表 { get; }
        public string 建造(角色类 user, 地图类 地图, Vector2Int 位置);
    }
    public abstract class 道具类 : I可携带 {
        public string 名称 => this.GetType().Name;
    }
    public abstract class 长剑类 : 道具类, I实体, IBuyAble {
        public double 价格 { get; set; }
        public abstract long 攻击力 { get; }
        public 长剑类() { }
    }
    public class 铁质长剑 : 长剑类 {
        public override long 攻击力 => 2;
        public 铁质长剑() {
            价格 = 10;
        }
    }
    public class 原木 : 道具类, I实体, IBuyAble {
        public double 价格 { get; set; } = 1;
    }
    public class 石块 : 道具类, I实体, IBuyAble {
        public double 价格 { get; set; } = 3;
    }
    public class 铁锭 : 道具类, I实体, IBuyAble {
        public double 价格 { get; set; } = 10;
    }
    public class 纪念堂建筑蓝图 : 道具类, I实体, IBuyAble, I蓝图 {
        public KeyValueList<IBuyAble, long> 所需材料列表 => new() {
            { new 原木(), 6},
            { new 石块(), 15},
            { new 铁锭(), 2}
        };
        public double 价格 { get; set; } = 1000;
        public string 建造(角色类 user, 地图类 地图, Vector2Int 位置) {
            foreach (var i in 所需材料列表) {
                if (user.获取物品数量(i.Key.GetType()) < i.Value) {
                    return $"您缺少{i.Key.名称}，无法建造";
                }
            }
            foreach (var i in 所需材料列表) {
                user.移除物品(i.Key.GetType(), 所需材料列表.Count());
            }
            var 纪念堂 = new 纪念堂(user.名称);
            地图.坐标系.Add(位置.X, 位置.X, 纪念堂);
            //成功在地图上建造了纪念堂
            return $"您在{地图.名称} 的{位置}处建造了{纪念堂.名称}";
        }
    }
    public class 精华 : 道具类, I实体, I所有者, I可携带, I污渍 {
        public string 所有者 { get; set; }
        public 精华(string 所有者) {
            this.所有者 = 所有者;
        }
        public new string 名称 => $"{所有者} 的精华";
    }
}
