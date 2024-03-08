using CMKZ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    //自嵌套类，用于表示地图。每个实例本身就是一个地图块，分别包含东南西北四个方向的出口，以及建筑和玩家列表。
    public abstract class 地图类 {
        public int 地图大小 = 1000;
        public abstract string 名称 { get; set; }
        public 地图类 东出口;
        public 地图类 南出口;
        public 地图类 西出口;
        public 地图类 北出口;
        public 坐标系<I实体> 坐标系 = new(1000,1000);
        public List<建筑类> 建筑物 => 坐标系.所有物体<建筑类>();
        /// <summary>
        /// 0是东，1是南，2是西，3是北
        /// </summary>
        /// <returns></returns>
        public List<地图类> 获得所有出口() {
            var 出口 = new List<地图类>();
            if (东出口 != null) 出口.Add(东出口);
            if (南出口 != null) 出口.Add(南出口);
            if (西出口 != null) 出口.Add(西出口);
            if (北出口 != null) 出口.Add(北出口);
            return 出口;
        }
        public void 吸收角色(方向 D, 角色类 角色) {
            Vector2Int 坐标;
            switch (D) {
                case 方向.东:
                坐标 = new(地图大小 / 2, 0);
                break;
                case 方向.南:
                坐标 = new(0, -地图大小 / 2);
                break;
                case 方向.西:
                坐标 = new(-地图大小 / 2, 0);
                break;
                case 方向.北:
                坐标 = new(0, 地图大小 / 2);
                break;
                default:
                坐标 = new(0, 0);
                break;
            };
            坐标系.Add(坐标.X, 坐标.Y, 角色);
        }
        public void 角色离开(角色类 角色) {
            坐标系.Remove(角色);
        }
        public bool 角色移动(角色类 角色,Vector2Int 坐标) {
            //如果坐标过大，那么就不允许移动
            if (Math.Abs(坐标.X) > 地图大小 / 2 || Math.Abs(坐标.Y) > 地图大小 / 2) {
                return false;
            }
            if (坐标系.MoveTo(角色, 坐标.X, 坐标.Y)) {
                return true;
            }
            return false;
        }
         
        public bool Contains(I实体 实体) {
            return 坐标系.物体坐标.Find(t => t.Key == 实体) != null;
        }
    }
    public enum 方向 {
        东,
        南,
        西,
        北
    }
    public interface I实体 { public string 名称 { get; } }
    public class 坐标系<T> {
        public Dictionary<int, Dictionary<int, List<T>>> 数据 = new();
        public KeyValueList<T, Vector2Int> 物体坐标 = new();
        public Vector2Int Find(T Data) => 物体坐标[Data];
        public List<T> 所有物体() => 物体坐标.Select(i => i.Key).ToList();
        public List<T2> 所有物体<T2>() => 物体坐标.Select(i => i.Key).OfType<T2>().ToList();
        public Vector2Int 坐标系大小;
        public 坐标系(int X, int Y) {
            坐标系大小 = new(X, Y);
        }
        public List<T> this[int x, int y] {
            get {
                if (存在(x, y)) {
                    return 数据[x][y];
                } else {
                    throw new Exception("坐标不存在");
                }
            }
            //set {
            //    if (存在(x, y)) {
            //        数据[x][y] = value;
            //    } else {
            //        Set(x, y, value);
            //    }
            //}
        }
        public bool 存在(int x, int y) {
            //这个坐标下是否存在物体
            return 物体坐标.Select(i => i.Value.X == x && i.Value.Y == y).Any();
        }
        public void Add(int x, int y, T Data) {
            //如果Y坐标不存在，那么就创建Y坐标
            添加空集合(x, y);
            数据[x][y].Add(Data);
            物体坐标[Data] = new(x, y);
        }
        //设置一个坐标位置，在设置物体
        public void Set(int x, int y, List<T> Data) {
            添加空集合(x, y);
            //先移除此坐标内的所有物体
            foreach (var i in 数据[x][y]) {
                物体坐标.RemoveKey(i);
            }
            //然后添加并替换新的物体
            数据[x][y]=Data;
            foreach (var i in Data) {
                物体坐标[i] = new(x, y);
            }
        }
        public bool Remove(T Data) {
            if (物体坐标.ContainsKey(Data)) {
                var 坐标 = 物体坐标[Data];
                //先移除此物体
                数据[坐标.X][坐标.Y].Remove(Data);
                删除空集合(坐标.X, 坐标.Y);
                //然后移除物体坐标
                物体坐标.RemoveKey(Data);
                return true;
            }
            return false;
        }
        public bool MoveTo(T Data, int x, int y) {
            if (物体坐标.ContainsKey(Data)) {
                //修改物体坐标：先在新位置添加，然后在旧位置移除
                添加空集合(x, y);
                var 当前坐标 = 物体坐标[Data];
                数据[当前坐标.X][当前坐标.Y].Remove(Data);
                数据[x][y].Add(Data);
                删除空集合(当前坐标.X, 当前坐标.Y);
                //修改物体坐标的get
                物体坐标[Data] = new(x, y);
                return true;
            }
            return false;
        }
        private void 添加空集合(int x, int y) {
            if (!数据.ContainsKey(x)) 数据[x] = new();
            if (!数据[x].ContainsKey(y)) 数据[x][y] = new();
        }
        private void 删除空集合(int x, int y) {
            if (数据[x][y].Count == 0) 数据[x].Remove(y);
            if (数据[x].Count == 0) 数据.Remove(x);
        }
        ////FindAll
        //public List<T> FindAll(Predicate<T> match) {
        //    var 结果 = new List<T>();
        //    foreach (var i in 数据) {
        //        foreach (var j in i.Value) {
        //            foreach (var k in j.Value) {
        //                if (match(k)) {
        //                    结果.Add(k);
        //                }
        //            }
        //        }
        //    }
        //    return 结果;
        //}
        //允许使用foreach
        public IEnumerator<T> GetEnumerator() {
            foreach (var i in 数据) {
                foreach (var j in i.Value) {
                    foreach (var k in j.Value) {
                        yield return k;
                    }
                }
            }
        }
        //IEnumerator IEnumerable.GetEnumerator() {
        //    return GetEnumerator();
        //}
    }
    //public class 地图坐标 {
    //    public int 上限 => 地图类.地图大小;
    //    public int X;
    //    public int Y;
    //    public 地图坐标(int x, int y) {
    //        X = x;
    //        Y = y;
    //    }
    //    public static bool operator ==(地图坐标 a, 地图坐标 b) {
    //        return a.X == b.X && a.Y == b.Y;
    //    }
    //    public static bool operator !=(地图坐标 a, 地图坐标 b) {
    //        return a.X != b.X || a.Y != b.Y;
    //    }
    //    public static 地图坐标 operator +(地图坐标 a, 地图坐标 b) {
    //        return new 地图坐标(a.X + b.X, a.Y + b.Y);
    //    }
    //    public static 地图坐标 operator -(地图坐标 a, 地图坐标 b) {
    //        return new 地图坐标(a.X - b.X, a.Y - b.Y);
    //    }
    //    public static 地图坐标 operator *(地图坐标 a, int b) {
    //        return new 地图坐标(a.X * b, a.Y * b);
    //    }
    //    public static 地图坐标 operator /(地图坐标 a, int b) {
    //        return new 地图坐标(a.X / b, a.Y / b);
    //    }
    //    public override bool Equals(object obj) {
    //        return base.Equals(obj);
    //    }
    //    public override int GetHashCode() {
    //        return base.GetHashCode();
    //    }
    //    public override string ToString() {
    //        return $"({X},{Y})";
    //    }
    //}
}
