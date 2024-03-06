using System;
using System.Collections.Generic;
using System.Linq;

namespace CMKZ {
    //Int
    public static partial class LocalStorage {
        public static bool IsClass(this Type type) => type != typeof(object) && Type.GetTypeCode(type) == TypeCode.Object;
        public static bool IsEnumerableType(this Type type) => type.GetInterface("IEnumerable") != null;
        public static T CreateObject<T>(params object[] X) {
            if (typeof(T).GetConstructor(Type.EmptyTypes) == null) {
                return default;
            } else if (typeof(T).IsClass()) {
                return (T)Activator.CreateInstance(typeof(T), X);
            }
            return default;
        }
        public static bool IsAllTrue(this Func<bool> X) {
            foreach (Func<bool> i in X.GetInvocationList()) {
                if (!i()) {
                    return false;
                }
            }
            return true;
        }
    }
}