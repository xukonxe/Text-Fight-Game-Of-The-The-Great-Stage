using System;
using System.Collections.Generic;
//using System.Collections.Generic;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class MyString {
        public string Value;
        public MyString(string X) {
            Value = X;
        }
        public static implicit operator string(MyString X) {
            return X.Value;
        }
        public static implicit operator MyString(string X) {
            return new MyString(X);
        }
    }
    public class MyInt {
        public int Value;
        public MyInt(int X) {
            Value = X;
        }
        public static implicit operator int(MyInt X) {
            return X.Value;
        }
        public static implicit operator MyInt(int X) {
            return new MyInt(X);
        }
    }
    public class MyLong {
        public long Value;
        public MyLong(long X) {
            Value = X;
        }
        public static implicit operator long(MyLong X) {
            return X.Value;
        }
        public static implicit operator MyLong(long X) {
            return new MyLong(X);
        }
    }
    public class Tree : Tree<MyString> {

    }
    public class Tree<T> {
        public string Name;
        public T Data;
        public List<Tree<T>> Children = new();
        public bool IsDir = true;
        public Dictionary<T1, T2> ToDictionary<T1, T2>(Func<T, KeyValue<T1, T2>> X) {
            var A = new Dictionary<T1, T2>();
            foreach (var i in Children) {
                if (i.Data != null) {
                    var B = X(i.Data);
                    A.Add(B.Key, B.Value);
                }
                if (i.IsDir) {
                    foreach (var j in i.ToDictionary(X)) {
                        A.Add(j.Key, j.Value);
                    }
                }
            }
            return A;
        }
        public Dictionary<string, T> ToDictionary(string X = null) {
            X = X == null ? "" : X + "/";
            var A = new Dictionary<string, T>();
            foreach (var i in Children) {
                if (i.IsDir) {
                    A.AddRange(i.ToDictionary(X + i.Name));
                } else {
                    A.Add(X + i.Name, i.Data);
                }
            }
            return A;
        }
        public List<T> ToList() {
            var A = new List<T>();
            foreach (var i in Children) {
                A.Add(i.Data);
                if (i.IsDir) {
                    foreach (var j in i.ToList()) {
                        A.Add(j);
                    }
                }
            }
            return A;
        }
        //"XX"则在子节点添加一项
        public Tree<T> AddDir(string X, T Y) {
            if (X.Contains("/")) {
                return Path(X.DirName()).AddDir(X.FileName(), Y);
            } else {
                var A = new Tree<T> { IsDir = true, Data = Y, Name = X };
                Children.Add(A);
                return A;
            }
        }
        public Tree<T> AddFile(string X, T Y) {
            if (X.Contains("/")) {
                return Path(X.DirName()).AddFile(X.FileName(), Y);
            } else {
                var A = new Tree<T> { IsDir = false, Data = Y, Name = X };
                Children.Add(A);
                return A;
            }
        }
        public Tree<T> Move(string 原路径, string 新路径) {
            var A = Path(原路径);
            Remove(原路径);
            var B = Path(新路径);
            B.Data = A.Data;
            B.IsDir = A.IsDir;
            B.Children.AddRange(A.Children);
            return B;
        }
        //""对应根节点
        //"XX"对应名为XX的子节点
        public Tree<T> Path(string X) {
            if (X == "") return this;
            if (!X.Contains("/")) return Children.Find(t => t.Name == X) ?? AddDir(X, CreateObject<T>());
            var A = this;
            foreach (string i in X.Split('/')) {
                A = A.Children.Find(t => t.Name == i) ?? A.AddDir(i, default);
            }
            return A;
        }
        public void Remove(string 路径) {
            Path(路径.DirName()).Children.RemoveAll(t => t.Name == 路径.FileName());
        }
        public void Remove(Func<Tree<T>, bool> X) {
            Children.RemoveAll(t => X(t));
            foreach (var i in Children) {
                i.Remove(X);
            }
        }
        public Tree<T> Find(Func<Tree<T>, bool> X) {
            foreach (var i in Children) {
                if (X(i)) {
                    return i;
                }
                var A = i.Find(X);
                if (A != null) {
                    return A;
                }
            }
            return null;
        }
        public Tree<T> Clone() {
            return FromDictionary(ToDictionary());
        }
        public static Tree<T> FromDictionary(Dictionary<string, T> X) {
            var A = new Tree<T>();
            foreach (var i in X) {
                A.AddFile(i.Key, i.Value);
            }
            return A;
        }
        public static Tree<T> FromDictionary(System.Collections.Generic.Dictionary<string, T> X) {
            var A = new Tree<T>();
            foreach (var i in X) {
                A.AddFile(i.Key, i.Value);
            }
            return A;
        }
    }
}