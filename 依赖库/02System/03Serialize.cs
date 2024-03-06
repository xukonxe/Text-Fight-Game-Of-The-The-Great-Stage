using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using static CMKZ.LocalStorage;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string JsonSerialize(this object X, bool W = false) {
            return JsonConvert.SerializeObject(X, new JsonSerializerSettings {
                TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,
                ContractResolver = new IgnoreActionContractResolver(),
                PreserveReferencesHandling = PreserveReferencesHandling.All,

                //TypeNameHandling = TypeNameHandling.Auto, // �Զ�����������Ϣ��ֻ���ڱ�Ҫʱ�����
                Formatting = Formatting.Indented // ���������ʽ����ѡ
                //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            });
        }
        public static T JsonDeserialize<T>(this string X, bool W = false) {
            return JsonConvert.DeserializeObject<T>(X, new JsonSerializerSettings {

                ContractResolver = new IgnoreActionContractResolver(),
                TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                Formatting = Formatting.Indented // ���������ʽ����ѡ
                //MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            });
        }
        public static byte[] StringToBytes(this string X) {
            return Encoding.UTF8.GetBytes(X);
        }
        public static string BytesToString(this byte[] X) {
            return Encoding.UTF8.GetString(X);
        }
        public static byte[] Encrypt(this byte[] X) {
            for (var i = 0; i < X.Length; i++) {
                X[i] ^= 0x78;
            }
            return X;
        }
        public static byte[] Decrypt(this byte[] X) {
            for (var i = 0; i < X.Length; i++) {
                X[i] ^= 0x78;
            }
            return X;
        }
        ////���ַ���ת����SHA256��֤��
        //public static string ToSHA256(this string X) {
        //    var S = new SHA256Managed();
        //    var H = S.ComputeHash(Encoding.UTF8.GetBytes(X));
        //    return BitConverter.ToString(H).Replace("-", "").ToLower();
        //}
        ////���ַ���ת����MD5��֤��
        //public static string ToMD5(this string X) {
        //    var S = new MD5CryptoServiceProvider();
        //    var H = S.ComputeHash(Encoding.UTF8.GetBytes(X));
        //    return BitConverter.ToString(H).Replace("-", "").ToLower();
        //}
    }
    public class IgnoreActionContractResolver : DefaultContractResolver {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);
            if (typeof(Delegate).IsAssignableFrom(property.PropertyType) || member.MemberType == MemberTypes.Property) {
                property.ShouldSerialize = instance => false;
            }
            return property;
        }
    }
}