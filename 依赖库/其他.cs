using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static bool TimesForAllTrue(this int X, Func<int, bool> Y) {
            for (var i = 0; i < X; i++) {
                if (!Y(i)) {
                    return false;
                }
            }
            return true;
        }
        public static bool IsInstanceOfGenericType(this object instance, Type genericType) {
            //if (instance.GetType().IsGenericType) {
            //    return instance.GetType().GetGenericTypeDefinition() == genericType;
            //}
            //return false;
            Type instanceType = instance.GetType();
            return genericType.IsAssignableFrom(instanceType) && instanceType.IsGenericType;
        }
        public static string Join(this IEnumerable<string> X, char Y) {
            return string.Join(Y, X);
        }
        public static string BaseString(this Type X) {
            if (X == typeof(int) || X == typeof(float) || X == typeof(double) || X == typeof(long)) {
                return "数字";
            } else if (X == typeof(string)) {
                return "文本";
            } else {
                return X.Name;
            }
        }
        public static string Join(this IEnumerable<string> X, string Y) {
            return string.Join(Y, X);
        }
        /// <summary>
        /// 向上取整
        /// </summary>
        public static long 取整(this double X) {
            return (long)Math.Ceiling(X);
        }
        public static string 两位小数(this double X) {
            //如果是整数，那么不显示小数点。如果是一位小数，那么显示一位小数。如果大于等于两位小数，那么显示两位小数。
            if (X == (int)X) {
                return ((int)X).ToString();
            } else if (X * 10 == (int)(X * 10)) {
                return X.ToString("0.0");
            } else {
                return X.ToString("0.00");
            }
        }
        public static double 乘方(this double X, double Y) {
            return Math.Pow(X, Y);
        }
        public static double 百分化(this double X) {
            return X / (X + 100);
        }
        public static float 波动(this float X,float Y) {
            return X * (1 + Random(-Y, Y));
        }
        public static double 波动(this double X, double Y) {
            return X * (1 + Random(-Y, Y));
        }
        public static long RandomLong() {
            int left = Random(int.MinValue, int.MaxValue);
            int right = Random(int.MinValue, int.MaxValue);
            return (long)left << 32 | (uint)right;
        }
        public static Guid RandomGuid() {
            return Guid.NewGuid();
        }
        public static float Random(float X=0,float Y=1) {
            return (float)Random((double)X, (double)Y);
        }
        public static double Random(double X, double Y) {
            return new Random().NextDouble() * (Y - X) + X;
        }
        public static int Random(int X, int Y) {
            return new Random().Next(X, Y);
        }
        public static int ShouldBiggerThan(this int X, int Y) {
            return X >= Y ? X : Y;
        }
        public static int ShouldSmallerThan(this int X, int Y) {
            return X <= Y ? X : Y;
        }
        public static bool 辗转属于(this string 装备全名, IEnumerable<string> 掉落范围) {
            foreach (var i in 掉落范围) {
                if (Regex.Match(装备全名, i).Success) {
                    return true;
                }
            }
            return false;
        }
        //public static void OnKeyDown(this KeyCode X, Action Y) {
        //    MainPanel.OnUpdate(() => {
        //        if (Input.GetKeyDown(X)) {
        //            Y();
        //        }
        //    });
        //}
        public static int GetID(string path) {
            var ID = FileRead(path).IntAdd();
            FileWrite(path, ID);
            return ID;
        }
        //public static float 获得两向量间的夹角(Vector3 fromVector, Vector3 toVector) {
        //    float angle = Vector3.Angle(fromVector, toVector); //求出两向量之间的夹角
        //    Vector3 normal = Vector3.Cross(fromVector, toVector);//叉乘求出法线向量
        //    angle *= Mathf.Sign(Vector3.Dot(normal, new Vector3(0, 0, 1)));
        //    return angle;
        //}
    }
    //public class 对象池 {
    //    public Dictionary<string, List<GameObject>> 所有物体 = new();
    //    public Dictionary<string, GameObject> 模版 = new();
    //    public Dictionary<string, Action<GameObject>> 复原函数 = new();
    //    public void 释放(string 模版名, GameObject 物体) {
    //        if (复原函数[模版名] == null) throw new Exception("模版不存在");
    //        复原函数[模版名](物体);
    //    }
    //    public GameObject 生成(string 模版名) {
    //        if (模版[模版名] == null) throw new Exception("模版不存在");
    //        return Instantiate(模版[模版名]); //等价于【先new再赋值】
    //    }
    //    public void 注册模版(string 模版名, GameObject 模版物体, Action<GameObject> _复原函数) {
    //        if (模版[模版名] != null) throw new Exception("已存在此模版！");
    //        模版[模版名] = 模版物体;
    //        复原函数[模版名] = _复原函数;
    //    }
    //}
}
