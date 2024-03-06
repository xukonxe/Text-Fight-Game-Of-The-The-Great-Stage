using System;
using System.Collections;
using System.Collections.Generic;

namespace CMKZ {
    //public class 坐标类 {
    //    public float X;
    //    public float Y;
    //    public 坐标类() { }
    //    public 坐标类(int A, int B) {
    //        X = A;
    //        Y = B;
    //    }
    //    public static implicit operator Vector2(坐标类 X) {
    //        return new Vector2(X.X, X.Y);
    //    }
    //    public static implicit operator 坐标类(Vector2 X) {
    //        return new 坐标类 { X = X.x, Y = X.y };
    //    }
    //    public static implicit operator Vector3(坐标类 X) {
    //        return new Vector3(X.X, X.Y);
    //    }
    //    public static implicit operator 坐标类(Vector3 X) {
    //        return new 坐标类 { X = X.x, Y = X.y };
    //    }
    //}
    public class Grid<T>:IEnumerable<T> {
        public T[,] Data;
        public int Width;
        public int Height;
        public Grid() { }
        public Grid(int X,int Y) { 
            Init(X, Y);
        }
        public Grid<T> Init(int X, int Y) {
            Width = X;
            Height = Y;
            Data = new T[X, Y];
            return this;
        }
        public T this[int X, int Y] {
            get => Data[X, Y];
            set => Data[X, Y] = value;
        }
        public void Fill(T X) => Fill((i, j) => X);
        public void Fill(Func<int, int, T> X) {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    Data[i, j] = X(i, j);
                }
            }
        }
        public void ForEach(Action<T> X) => ForEach((i, j, k) => X(k));
        public void ForEach(Action<int, int, T> X) {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    X(i, j, Data[i, j]);
                }
            }
        }
        public IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    yield return Data[i, j];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
