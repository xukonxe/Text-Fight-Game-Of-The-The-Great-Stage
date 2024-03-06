using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ {
    public static partial class LocalStorage { 
    
    }
    public class Vector2Int {
        public int X;
        public int Y;
        public Vector2Int(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }
        //重载运算符
        public static Vector2Int operator +(Vector2Int A, Vector2Int B) {
            return new Vector2Int(A.X + B.X, A.Y + B.Y);
        }
        public static Vector2Int operator -(Vector2Int A, Vector2Int B) {
            return new Vector2Int(A.X - B.X, A.Y - B.Y);
        }
        public static Vector2Int operator *(Vector2Int A, int B) {
            return new Vector2Int(A.X * B, A.Y * B);
        }
        public static Vector2Int operator /(Vector2Int A, int B) {
            return new Vector2Int(A.X / B, A.Y / B);
        }
        public override string ToString() {
            return $"({X},{Y})";
        }
    }
    //    public static partial class LocalStorage {
    //    public static string Remove(this string X, string Y) => X.Replace(Y, "");
    //    public static string BaseString(this Type X) {
    //        if (X == typeof(int) || X == typeof(float) || X == typeof(double) || X == typeof(long)) {
    //            return "数字";
    //        } else if (X == typeof(string)) {
    //            return "文本";
    //        } else {
    //            return X.Name;
    //        }
    //    }
    //    public static void ForEach<T>(this IEnumerable<T> X, Action<T> Y) {
    //        foreach (var i in X) Y(i);
    //    }
    //    public static string Join(this IEnumerable<string> X, string Y) {
    //        return string.Join(Y, X);
    //    }
    //    public static T Find<T>(this IEnumerable<T> X, Func<T, bool> Y) {
    //        foreach (var i in X) {
    //            if (Y(i)) {
    //                return i;
    //            }
    //        }
    //        throw new Exception($"错误：找不到指定目标。目前集合：{X.JsonSerialize()}");
    //    }
    //    public static string JsonSerialize(this object X, bool W = false) {
    //        return JsonConvert.SerializeObject(X, new JsonSerializerSettings {
    //            TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,
    //            ContractResolver = new IgnoreActionContractResolver(),
    //            PreserveReferencesHandling = PreserveReferencesHandling.All,

    //            //TypeNameHandling = TypeNameHandling.Auto, // 自动包含类型信息，只有在必要时才添加
    //            Formatting = Formatting.Indented // 美化输出格式，可选
    //            //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        });
    //    }
    //    public static byte[] StringToBytes(this string X) {
    //        return Encoding.UTF8.GetBytes(X);
    //    }
    //    public static string BytesToString(this byte[] X) {
    //        return Encoding.UTF8.GetString(X);
    //    }
    //    public class IgnoreActionContractResolver : DefaultContractResolver {
    //        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
    //            var property = base.CreateProperty(member, memberSerialization);
    //            if (typeof(Delegate).IsAssignableFrom(property.PropertyType) || member.MemberType == MemberTypes.Property) {
    //                property.ShouldSerialize = instance => false;
    //            }
    //            return property;
    //        }
    //    }
    //    public static T JsonDeserialize<T>(this string X, bool W = false) {
    //        return JsonConvert.DeserializeObject<T>(X, new JsonSerializerSettings {

    //            ContractResolver = new IgnoreActionContractResolver(),
    //            TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,
    //            PreserveReferencesHandling = PreserveReferencesHandling.All,
    //            Formatting = Formatting.Indented // 美化输出格式，可选
    //            //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        });
    //    }
    //    public static byte[] Encrypt(this byte[] X) {
    //        for (var i = 0; i < X.Length; i++) {
    //            X[i] ^= 0x78;
    //        }
    //        return X;
    //    }
    //    public static byte[] Decrypt(this byte[] X) {
    //        for (var i = 0; i < X.Length; i++) {
    //            X[i] ^= 0x78;
    //        }
    //        return X;
    //    }
    //    public static void FileWrite(string Path, object Y, bool 加密 = false, bool 全保存 = false) {
    //        var A = System.IO.Path.GetDirectoryName(Path);
    //        if (!Directory.Exists(A)) Directory.CreateDirectory(A);
    //        if (Y is string B) {
    //            File.WriteAllText(Path, B);
    //        } else if (加密) {
    //            File.WriteAllBytes(Path, Y.JsonSerialize(全保存).StringToBytes().Encrypt());
    //        } else {
    //            File.WriteAllText(Path, Y.JsonSerialize(全保存));
    //        }
    //    }
    //    public static string FileRead(string X) {
    //        if (File.Exists(X)) {
    //            return File.ReadAllText(X).Replace("\r", "");
    //        } else {
    //            return "";
    //        }
    //    }
    //    public static T FileRead<T>(string X, bool Y = false, bool 全保存 = false) {
    //        if (File.Exists(X)) {
    //            if (!Y) {
    //                return File.ReadAllText(X).JsonDeserialize<T>(全保存);
    //            }
    //            return File.ReadAllBytes(X).Decrypt().BytesToString().JsonDeserialize<T>(全保存);
    //        } else {
    //            return default;
    //        }
    //    }
    //}
}
