using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class List<T> : System.Collections.Generic.List<T> {
       
        public event Action<T, int> OnAdd;
        public event Action<T, int> OnRemove;
        public new void Add(T X) {
            base.Add(X);
            OnAdd?.Invoke(X, Count - 1);
        }
        public new void Remove(T X) {
            var A = IndexOf(X);
            base.Remove(X);
            OnRemove?.Invoke(X, A);
        }
        public void RemoveAll(Func<T, bool> X) {
            var A = FindAll(X);
            foreach (var i in A) {
                Remove(i);
            }
        }
        public new void RemoveAt(int X) {
            var A = this[X];
            base.RemoveAt(X);
            OnRemove?.Invoke(A, X);
        }
        public new void Insert(int X, T Y) {
            base.Insert(X, Y);
            OnAdd?.Invoke(Y, X);
        }
        public new void Clear() {
            foreach (var i in this) {
                OnRemove?.Invoke(i, IndexOf(i));
            }
            base.Clear();
        }

        public static List<T> operator +(List<T> X, T Y) {
            X.Add(Y);
            return X;
        }
        public static List<T> operator -(List<T> X, T Y) {
            X.Remove(Y);
            return X;
        }
        public List() : base() { }
        public List(int X) : base(X) {
            //���Tӵ�й��캯������ô����X��ʵ���������б�
            if (typeof(T).GetConstructors().Length > 0) {
                for (int i = 0; i < X; i++) {
                    Add((T)Activator.CreateInstance(typeof(T)));
                }
            }
        }
        public List(T[] X) : base(X) { }
        //�����ֶΡ���ҳ�����������50��Ԫ�أ���ô��ҳ����1�������101������ô��ҳ����2��ÿ100����һҳ��
        //���庯������ȡ��ҳ(int X)���������ȡ��һҳ����ô����0��99Ԫ����ɵ�list���ڶ�ҳ��100��199���������ֻ��150����ô����100��150����������ֻ��150�������ȡ����ҳ����ô���ؿա�
        public int ��ҳ�� => (int)Math.Ceiling((float)Count / 100);
        public List<T> ��ȡ��ҳ(int X) {
            if (X <= 0 || X > ��ҳ��) {
                return new List<T>();
            }
            X--;
            return GetRange(X * 100, Math.Min(Count - X * 100, 100));
        }
        public new List<T> GetRange(int start, int length) {
            var A = new List<T>();
            for (int i = start; i < start + length; i++) {
                A.Add(this[i]);
            }
            return A;
        }
        public List<T> FindAll(Func<T, bool> X) {
            var A = new List<T>();
            foreach (var i in this) {
                if (X(i)) {
                    A.Add(i);
                }
            }
            return A;
        }
        public List<T2> Parse<T2>(Func<T, T2> X) {
            var A = new List<T2>();
            foreach (var i in this) {
                A.Add(X(i));
            }
            return A;
        }
        public string Join(char X) {
            return this.ToString(t => t.ToString(), X);
        }
        public bool AddWithNotRepeat(T X) {
            if (!Contains(X)) {
                Add(X);
                return true;
            }
            return false;
        }
        public List<T> Clone() => new(ToArray());
        public T AddWithReturn(T Y) {
            Add(Y);
            return Y;
        }
        public List<T> AddWithReturnList(T Y) {
            Insert(0,Y);
            return this;
        }
        public List<T> MoveToLast(Func<T, bool> �ж�) {
            //�����б��е�ÿһ��Ԫ�أ�����жϺ�������true�������ƶ����б����
            for (int i = 0; i < this.Count; i++) {
                if (�ж�(this[i])) {
                    this.Move(i, this.Count - 1);
                }
            }
            return this;
        }
        internal List<T> MoveToFirst(Func<T, bool> value) {
            //�����б��е�ÿһ��Ԫ�أ�����жϺ�������true�������ƶ����б���ǰ��
            for (int i = 0; i < this.Count; i++) {
                if (value(this[i])) {
                    this.Move(i, 0);
                }
            }
            return this;
        }
        public void Move(int fromIndex, int toIndex) {
            if (fromIndex < 0 || fromIndex >= this.Count || toIndex < 0 || toIndex >= this.Count) {
                throw new ArgumentOutOfRangeException("����������Χ");
            }
            T item = this[fromIndex];
            this.RemoveAt(fromIndex);
            OnRemove?.Invoke(item, fromIndex);
            this.Insert(toIndex, item);
            OnAdd?.Invoke(item, toIndex);
        }
        //���ؼӺ������������һ���µ��б����������б������Ԫ��
        public static List<T> operator +(List<T> X, List<T> Y) {
            var A = new List<T>();
            A.AddRange(X);
            A.AddRange(Y);
            return A;
        }
        public IEnumerable<T1> Where<T1>() where T1 : class {
            var A = new List<T1>();
            foreach (var i in this) {
                if (i is T1) {
                    A.Add(i as T1);
                }
            }
            return A;
        }
    }
}