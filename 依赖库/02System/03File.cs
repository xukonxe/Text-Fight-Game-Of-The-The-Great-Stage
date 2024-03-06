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
        ///<summary>����Ŀ¼������Ч����0�������ļ�������1�����մ���ʱ������2�������޸�ʱ������3�������ļ���С����</summary>
        public static List<string> ReadFileList(string Path, int ���� = 0) {
            var �ļ����б� = new List<string>();
            var directoryInfo = new DirectoryInfo(Path);
            var �ļ��б� = directoryInfo.GetFiles();

            if (���� == 0) {
                Array.Sort(�ļ��б�, (x, y) => x.Name.CompareTo(y.Name));
            } else if (���� == 1) {
                Array.Sort(�ļ��б�, (x, y) => x.CreationTime.CompareTo(y.CreationTime));
            } else if (���� == 2) {
                Array.Sort(�ļ��б�, (x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));
            } else if (���� == 3) {
                Array.Sort(�ļ��б�, (x, y) => x.Length.CompareTo(y.Length));
            }

            foreach (var file in �ļ��б�) {
                �ļ����б�.Add(file.Name);
            }

            return �ļ����б�;
        }
        ///<summary>���Ŀ¼</summary>
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
        public static List<string> �ļ�����(string �ļ�����, string ����) {
            var A = new List<string>();
            foreach (var i in GetFileList(�ļ�����)) {
                foreach (var j in File.ReadAllLines(i)) {
                    if (j.Contains(����)) {
                        A.Add(i + "��" + j);
                    }
                }
            }
            return A;
        }
    }
}