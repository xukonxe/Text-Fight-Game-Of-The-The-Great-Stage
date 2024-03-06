using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMKZ;
using static CMKZ.LocalStorage;
using CMKZ.C02;
using static CMKZ.C02.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class 存档系统 {
        public static string 存档路径 => "C:/C02/大冒险存档/";
        public static string 保存存档(this 世界类 X) {
            FileWrite(存档路径 + X.存档名 + ".txt", X, 全保存: true);
            return $"存档 {X.存档名} 已保存在：{存档路径}";
        }
        public static 世界类 读取存档(string 存档名) {
            return FileRead<世界类>(存档路径 + 存档名 + ".txt", 全保存: true);
        }
        public static 世界类 Try读取存档() { 
            //获取存档路径下的所有文件
            var 存档文件 = Directory.GetFiles(存档路径);
            //如果存档文件为空，那么就不进行读取
            if (存档文件.Length == 0) {
                Print("当前没有存档");
                return null;
            }
            var A=new StringBuilder();
            A.AppendLine("========可用存档列表========");
            var 存档文件名 = 存档文件.Select(x => x.Replace(存档路径, "").Replace(".txt", ""));
            //在存档文件名前加上创建时间
            存档文件名.ForEach(name => A.AppendLine($"[{获取创建时间(存档路径 + name + ".txt")}] {name}"));
            A.AppendLine("============================");
            return 读取存档(存档文件名.First());
        }
    }
}
