using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer

namespace CMKZ {
    public static partial class LocalStorage {
        ///<summary>目录名</summary>
        public static string DirName(this string X) => X.IndexOf("/") == -1 ? "" : X[..X.LastIndexOf("/")];
        ///<summary>文件名</summary>
        public static string FileName(this string X) => X.IndexOf("/") == -1 ? X : X[(X.LastIndexOf("/") + 1)..];
        ///<summary>根目录名</summary>
        public static string RootName(this string X) => X.IndexOf("/") == -1 ? X : X[..X.IndexOf("/")];
        public static string RemoveFirst(this string Y) => Y.Split("/").RemoveFirst().Join("/");
        public static string GetAtSecond(this string X) => X.Split("/")[1];
        public static string RePath(string X) => X.Replace("/", "\\").Replace("\\\\", "\\").TrimEnd('\\');
    }
}