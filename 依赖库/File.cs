
using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer

namespace CMKZ {
    public static partial class LocalStorage {
        public static void FileWrite(string Path, object Y, bool ���� = false, bool ȫ���� = false) {
            var A = System.IO.Path.GetDirectoryName(Path);
            if (!Directory.Exists(A)) Directory.CreateDirectory(A);
            if (Y is string B) {
                File.WriteAllText(Path, B);
            } else if (����) {
                File.WriteAllBytes(Path, Y.JsonSerialize(ȫ����).StringToBytes().Encrypt());
            } else {
                File.WriteAllText(Path, Y.JsonSerialize(ȫ����));
            }
        }
        public static string TryFileRead(string X, string Y) {
            if (!FileExists(X)) {
                FileWrite(X, Y);
            }
            return FileRead(X);
        }
        public static T TryFileRead<T>(string X, T Y, bool ���� = false, bool ȫ���� = false) {
            if (!FileExists(X)) {
                FileWrite(X, Y,����,ȫ����);
            }
            return FileRead<T>(X, ����, ȫ����);
        }
        public static string FileRead(string X) {
            if (File.Exists(X)) {
                return File.ReadAllText(X).Replace("\r", "");
            } else {
                return "";
            }
        }
        public static T FileRead<T>(string X, bool Y = false, bool ȫ���� = false) {
            if (File.Exists(X)) {
                if (!Y) {
                    return File.ReadAllText(X).JsonDeserialize<T>(ȫ����);
                }
                return File.ReadAllBytes(X).Decrypt().BytesToString().JsonDeserialize<T>(ȫ����);
            } else {
                return default;
            }
        }
        public static void FileAppend(string X, string Y) {
            FileWrite(X, FileRead(X) + Y);
        }
        public static void TextAppend(string URL, string X) => FileWrite(URL, FileRead(URL) + X + "\n");
        public static void FileRemove(string X) {
            if (File.Exists(X)) {
                File.Delete(X);
            }
        }
        public static void FileRename(string X, string Y) {
            if (File.Exists(X)) {
                File.Move(X, Y);
            }
        }
        public static bool FileExists(string X) {
            return File.Exists(X);
        }
    }
}