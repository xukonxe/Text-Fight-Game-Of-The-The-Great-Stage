using System;
using System.Collections.Generic;
using System.Linq;

namespace CMKZ {
    //String
    public static partial class LocalStorage {
        public static string Join(this string[] X, string Y = ",") => string.Join(Y, X);
        public static string ToHex(this string input) => input.Aggregate(string.Empty, (current, c) => current + ((int)c).ToString("X2"));
        public static string Remove(this string X, string Y) => X.Replace(Y, "");
        public static string TrimStart(this string X, string Y) {
            if (X.StartsWith(Y)) {
                X = X[Y.Length..];
            }
            return X;
        }
        public static string Ê¡ÂÔ(this string input) {
            if (input.Length > 100) {
                var firstFiveChars = input[..50];
                var lastFiveChars = input.Substring(input.Length - 50, 50);
                return $"{firstFiveChars}...{lastFiveChars}";
            } else {
                return input;
            }
        }
        public static string Join(this int[] X, char Y) {
            return X.ToString(t => t.ToString(), Y);
        }
    }
}