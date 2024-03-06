using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string NowTime => DateTime.Now.ToLocalTime().ToString("[yyyy.MM.dd_HH:mm:ss]");
        public static string Now => DateTime.Now.ToLocalTime().ToString("HH:mm:ss");
        public static long FileSize(string X) {
            FileInfo fileInfo = new FileInfo(X);
            if (fileInfo.Exists) {
                return fileInfo.Length;
            } else {
                return -1;
            }
        }
        ///<summary>单层目录，参数效果：0→按照文件名排序。1→按照创建时间排序。2→按照修改时间排序。3→按照文件大小排序。</summary>
        public static List<string> ReadFileList(string Path, int 参数 = 0) {
            var 文件名列表 = new List<string>();
            var directoryInfo = new DirectoryInfo(Path);
            var 文件列表 = directoryInfo.GetFiles();

            if (参数 == 0) {
                Array.Sort(文件列表, (x, y) => x.Name.CompareTo(y.Name));
            } else if (参数 == 1) {
                Array.Sort(文件列表, (x, y) => x.CreationTime.CompareTo(y.CreationTime));
            } else if (参数 == 2) {
                Array.Sort(文件列表, (x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));
            } else if (参数 == 3) {
                Array.Sort(文件列表, (x, y) => x.Length.CompareTo(y.Length));
            }

            foreach (var file in 文件列表) {
                文件名列表.Add(file.Name);
            }

            return 文件名列表;
        }
        ///<summary>多层目录</summary>
        public static List<string> GetFileList(string X) {
            var A = new List<string>();
            GetFilesRecursive(X, A);
            var B = A.SortFile();
            for (int i = 0; i < B.Length; i++) {
                A[i] = B[i].Replace(X, X.FileName());
            }
            return A;
        }
        private static void GetFilesRecursive(string X, List<string> Y) {
            foreach (var i in Directory.GetFiles(X)) {
                Y.Add(i.Replace("\\", "/"));
            }
            foreach (var i in Directory.GetDirectories(X)) {
                GetFilesRecursive(i, Y);
            }
        }
        public static string[] SortFile(this List<string> fileNames) {
            var files = fileNames.Select(fileName => new Tuple<string, DateTime>(fileName, File.GetCreationTime(fileName))).ToArray();
            var sortedFiles = files.OrderBy(file => file.Item2);
            return sortedFiles.Select(file => file.Item1).ToArray();
        }
        public static FileSystemInfo[] SortByCreateTime(this FileSystemInfo[] X) {
            Array.Sort(X, (x, y) => y.CreationTime.CompareTo(x.CreationTime));
            return X;
        }
        public static List<string> 文件检索(string 文件夹名, string 内容) {
            var A = new List<string>();
            foreach (var i in GetFileList(文件夹名)) {
                foreach (var j in File.ReadAllLines(i)) {
                    if (j.Contains(内容)) {
                        A.Add(i + "：" + j);
                    }
                }
            }
            return A;
        }
    }
}