using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CMKZ.LocalStorage;
using Newtonsoft.Json;

namespace CMKZ {
    [JsonObject(MemberSerialization.OptOut)]
    public class Dictionary<T1, T2> : IEnumerable<KeyValuePair<T1,T2>>{
        public System.Collections.Generic.Dictionary<T1, T2> Data=new();
        [JsonIgnore]
        public int Count => Data.Count;
        [JsonIgnore]
        public T1[] Keys => Data.Keys.ToArray();
        [JsonIgnore]
        public T2[] Values => Data.Values.ToArray();
        public T2 this[T1 X] {
            get {
                if (X == null) {
                    return default;
                }
                if (Data.ContainsKey(X)) {
                    return Data[X];
                }
                if (typeof(T2).IsClass()) {
                    if(typeof(T2).IsAbstract) {
                        return default;
                    }
                    return CreateObject<T2>();
                }
                return default;
            }
            set {
                Data[X] = value;
            }
        }
        public Dictionary() { }
        public Dictionary(System.Collections.Generic.Dictionary<T1,T2> X) {
            foreach (var i in X) {
                this[i.Key] = i.Value;
            }
        }
        public void AddRange(Dictionary<T1, T2> X, Func<T2, T2, T2> Y = null) {
            Y??=(V1,V2)=>V2;
            foreach (var i in X) {
                this[i.Key] = ContainsKey(i.Key) ? Y(this[i.Key], i.Value) : i.Value;
            }
        }
        public void RemoveAll(Func<KeyValuePair<T1, T2>, bool> X) {
            foreach (var item in this.Where(X).ToList()) {
                Remove(item.Key);
            }
        }
        public Dictionary<T1, T2> ChangeKey(T1 Y, T1 Z) {
            this[Z] = this[Y];
            Remove(Y);
            return this;
        }
        public Dictionary<T1, T2> Clone() {
            var A = new Dictionary<T1, T2>();
            foreach (var i in this) {
                A[i.Key] = i.Value;
            }
            return A;
        }
        public Dictionary<T3, T2> ChangeKey<T3>(Func<T1, T3> Y) {
            var A = new Dictionary<T3, T2>();
            foreach (var i in this) {
                A[Y(i.Key)] = i.Value;
            }
            return A;
        }
        public Dictionary<T1,T3> Turn<T3>(Func<T2,T3> X) {
            var A=new Dictionary<T1, T3>();
            foreach (var i in this) {
                A[i.Key] = X(i.Value);
            }
            return A;
        }
        //补充经典字典
        public void Add(T1 X, T2 Y) {
            if (Data.ContainsKey(X)) {
                //编辑器输出警告
                Console.WriteLine($"警告：字典中有相同的键：{X}");
                Data[X] = Y;
            } else {
                Data.Add(X, Y);
            }
        }
        public void Add(KeyValuePair<T1, T2> X) {
            Data.Add(X.Key, X.Value);
        }
        public void Remove(T1 X) {
            Data.Remove(X);
        }
        public bool ContainsKey(T1 X) {
            return Data.ContainsKey(X);
        }
        public bool ContainsValue(T2 X) {
            return Data.ContainsValue(X);
        }
        public void Clear() {
            Data.Clear();
        }
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() {
            return Data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public static implicit operator Dictionary<T1, T2>(System.Collections.Generic.Dictionary<T1, T2> X) {
            return new Dictionary<T1, T2>(X);
        }
        public static implicit operator System.Collections.Generic.Dictionary<T1, T2>(Dictionary<T1, T2> X) {
            return X.Data;
        }
        public static Dictionary<T1, T2> operator +(Dictionary<T1, T2> X, Dictionary<T1, T2> Y) {
            var A = new Dictionary<T1, T2>();
            A.AddRange(X);
            A.AddRange(Y);
            return A;
        }
        public static Dictionary<T1, T2> operator -(Dictionary<T1, T2> X, Dictionary<T1, T2> Y) {
            var A = new Dictionary<T1, T2>();
            A.AddRange(X);
            A.RemoveAll(x => Y.ContainsKey(x.Key));
            return A;
        }
    }
}
