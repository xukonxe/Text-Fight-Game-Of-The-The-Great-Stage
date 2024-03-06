using System;
using System.Collections.Generic;
using System.Linq;

namespace CMKZ {
    public static partial class LocalStorage {
        public static int[] ToIntList(this string X) => X.Split(" ").ParseTo(t => t.ToInt());
        public static bool IsInt(this string X) => int.TryParse(X, out _);
        public static void Add(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                X[i] += Y[i];
            }
        }
        public static int[] Multiply(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                X[i] *= Y[i];
            }
            return X;
        }
        public static int[] AddToNew(this int[] X, int[] Y) {
            var A = new int[X.Length];
            for (var i = 0; i < X.Length; i++) {
                A[i] = X[i] + Y[i];
            }
            return A;
        }
        public static double[] AddToNew(this double[] X, double[] Y) {
            var A = new double[X.Length];
            for (var i = 0; i < X.Length; i++) {
                A[i] = X[i] + Y[i];
            }
            return A;
        }
        public static bool BiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] < Y[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool NotBiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] > Y[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool OneBiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] > Y[i]) {
                    return true;
                }
            }
            return false;
        }
        public static int IntAdd(this string X) => X == "" ? 1 : int.Parse(X) + 1;
        public static int ToInt(this string X) {
            X = X.Replace("_", "");
            return int.Parse(X);
        }
        public static long ToLong(this string X) {
            X = X.Replace("_", "");
            return long.Parse(X);
        }
        public static float 百分化(this float X) => X / (X + 100);
        public static double 百分化(this long X) => X / (X + 100d);
        public static string 一位小数(this float X) {
            return Math.Round(X, 1).ToString();
        }
        public static string 一位小数(this double X) {
            return Math.Round(X, 1).ToString();
        }
        //public static float Exp(this float X, float Y) {
        //    return Mathf.Pow(X, Y);
        //}
        public static float ToFloat(this string X) {
            return float.Parse(X);
        }
        public static int Pow(int X, int Y) {
            Y.TimesToDo(i => X *= X);
            return X;
        }
    }
}