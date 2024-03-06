using System;

namespace CMKZ {
    public static partial class LocalStorage {
        //public static Vector2 BiggerThan(this Vector2 X, int Y) {
        //    return new Vector2(Math.Max(X.x, Y), Math.Max(X.y, Y));
        //}
    }
    public struct Vector4Int {
        public int x;
        public int y;
        public int z;
        public int w;
        public Vector4Int(int X, int Y, int Z, int W) {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
        public Vector4Int(string X) {
            var A = X.Split(" ");
            x = int.Parse(A[0]);
            y = int.Parse(A[1]);
            z = int.Parse(A[2]);
            w = int.Parse(A[3]);
        }
        public static Vector4Int operator +(Vector4Int X, Vector4Int Y) {
            Vector4Int A = new();
            A.x = X.x + Y.x;
            A.y = X.y + Y.y;
            A.z = X.z + Y.z;
            A.w = X.w + Y.w;
            return A;
        }
        public static bool operator >=(Vector4Int X, Vector4Int Y) {
            return X.x >= Y.x && X.y >= Y.y && X.z >= Y.z && X.w >= Y.w;
        }
        public static bool operator <=(Vector4Int X, Vector4Int Y) {
            return X.x <= Y.x && X.y <= Y.y && X.z <= Y.z && X.w <= Y.w;
        }
        public static bool operator ==(Vector4Int X, Vector4Int Y) {
            return default;
        }
        public static bool operator !=(Vector4Int X, Vector4Int Y) {
            return default;
        }
        public static Vector4Int operator *(int X, Vector4Int Y) {
            return default;
        }
        public override bool Equals(object obj) {
            return base.Equals(obj);
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return base.ToString();
        }
    }
}
