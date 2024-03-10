using System;
using System.Diagnostics;
using System.Security.Cryptography;
using CMKZ;
using static CMKZ.LocalStorage;
using System.Text;
using System.Linq;

namespace CMKZ.C02 {
    public static partial class LocalStorage {
        static void 示例() {
            //CMKZ.C02.多人大冒险01_命令行客户端.LocalStorage.MainStart();
            var 消息 = new MsgBox().Title("控制台消息");
            消息.AppendMsg("这是玩家信息");
            var 基本信息 = 消息.AddBox(分割线形态.减号);
            基本信息.AppendPointMsg("建筑1", "距离123", "位置xxx");
            基本信息.AppendNumberMsg("建筑2", "距离345", "位置", "距离345", "位置");
            基本信息.AppendNumberMsg("建筑3");
            基本信息.AppendNumberMsg("建筑4", "距离789", "位置xxx");
            var 附近玩家 = 消息.AddBox(分割线形态.减号);
            附近玩家.AppendNumberMsg("玩家1", "距离123", "位置xxx");
            附近玩家.AppendNumberMsg("玩家2", "距离345", "位置xxx");
            附近玩家.AppendNumberMsg("玩家3", "距离567", "位置xxx");
            附近玩家.AppendNumberMsg("玩家4", "距离789", "位置xxx");
            //输出效果：

            //============= 控制台消息 ============
            //·这是玩家信息
            //-------------------------------------
            //·建筑1 距离123  位置xxx
            //1.建筑2  距离345 位置  距离345 位置
            //2.建筑3
            //3.建筑4  距离789 位置xxx
            //-------------------------------------
            //
            //-------------------------
            //1.玩家1  距离123 位置xxx
            //2.玩家2  距离345 位置xxx
            //3.玩家3  距离567 位置xxx
            //4.玩家4  距离789 位置xxx
            //-------------------------
            //
            //=====================================
        }
        public static MsgBox Title(this MsgBox X, string 标题) {
            X.标题 = 标题;
            return X;
        }
        public static MsgBox AddBox(this MsgBox X, 分割线形态 形态) {
            var A = new MsgBox() { 形态 = 形态 };
            X.Children.Add(A);
            return A;
        }
        public static Msg AppendMsg(this MsgBox X, params string[] 内容) {
            var A = new Msg() { 类型 = MsgType.无 };
            A.内容.AddRange(内容);
            X.Children.Add(A);
            return A;
        }
        public static Msg AppendPointMsg(this MsgBox X, params string[] 内容) {
            var A = new Msg() { 类型 = MsgType.无序 };
            A.内容.AddRange(内容);
            X.Children.Add(A);
            return A;
        }
        public static Msg AppendNumberMsg(this MsgBox X, params string[] 内容) {
            var A = new Msg() { 类型 = MsgType.有序 };
            A.内容.AddRange(内容);
            X.Children.Add(A);
            return A;
        }
        public static bool 是全角(char c) {
            // 根据Unicode编码判断是否为全角字符
            // 全角字符（包括中文、全角标点等）的通用判断范围较广
            // 以下是简化的判断逻辑，可能需要根据具体需求调整
            int code = Convert.ToInt32(c);
            if (code >= 0x1100 && code <= 0x11FF // Hangul Jamo
                || code >= 0x2E80 && code <= 0x9FFF // CJK Unified Ideographs等
                || code >= 0xFF00 && code <= 0xFFEF) // Halfwidth and Fullwidth Forms
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取字符串的全半角长度，中文字符算2个长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int 获取全长(this string input) {
            int length = 0;
            foreach (char c in input) {
                if (是全角(c)) {
                    length += 2; // 全角字符增加2
                } else {
                    length += 1; // 半角字符增加1
                }
            }
            return length;
        }

    }
    public interface IMsg {
        public int 宽度 { get; }
    }
    public class MsgBox : IMsg {
        public string 标题 = "";
        public int 宽度 => 获取宽度();
        public List<IMsg> Children = new();
        public 分割线形态 形态 = 分割线形态.等号;
        public override string ToString() {
            var 宽度 = 获取宽度();
            var A = new StringBuilder();
            //计算标题的左右空格
            var 空格长度 = (宽度 - 标题.获取全长()) / 2;
            char 符号;
            if (形态 == 分割线形态.等号) {
                符号 = '=';
            } else if (形态 == 分割线形态.减号) {
                符号 = '-';
            } else {
                符号 = ' ';
            }

            if (空格长度 * 2 + 标题.获取全长() < 宽度) {
                A.AppendLine(new string(符号, 空格长度) + 标题 + new string(符号, 空格长度) + new string(符号, 1));
            } else {
                A.AppendLine(new string(符号, 空格长度) + 标题 + new string(符号, 空格长度));
            }

            int 顺序当前 = 0;
            foreach (var item in Children) {
                if (item is Msg i) {
                    if (i.类型 != MsgType.有序) {
                        顺序当前 = 0;
                        A.AppendLine(item.ToString());
                        continue;
                    }
                    顺序当前++;
                    A.AppendLine($"{顺序当前}.{item.ToString()}");
                    continue;
                }
                A.AppendLine(item.ToString());
            }

            A.AppendLine(new string(符号, 宽度));
            return A.ToString();
        }
        public int 获取宽度() {
            return Children.Select(x => x.宽度).Max();
        }
    }
    public class Msg : IMsg {
        public List<string> 内容 = new();
        public int 宽度 => 获取宽度();
        public MsgType 类型;
        public 分割线形态 分割线形态 = 分割线形态.无;
        public int 获取宽度() {
            //宽度就是【每个内容的长度+（内容数-1）*2】
            var 每个内容的长度 = 内容.Select(x => x.获取全长());
            int 间隔长度 = (内容.Count - 1) * 2;
            int 序号长度;
            if (类型 == MsgType.有序) {
                序号长度 = 2;
            } else if (类型 == MsgType.无序) {
                序号长度 = 1;
            } else {
                序号长度 = 0;
            }
            return 每个内容的长度.Sum() + 间隔长度 + 序号长度;
        }
        public override string ToString() {
            var 宽度 = 获取宽度();
            var A = new StringBuilder();
            //如果类型是无序，那么直接添加点号。
            if (类型 == MsgType.无序) {
                A.Append("·");
                宽度 -= 1;
            }
            if (类型 == MsgType.有序) {
                宽度 -= 2;
            }
            //序号后面添加内容，如果是一个，那么直接添加。
            //如果是多个，那么水平均匀分布，自动计算间隔。
            if (内容.Count == 1) {
                A.Append(内容[0]);
            } else {
                //计算间隔
                var 每个内容的长度 = 内容.Select(x => x.获取全长()).Sum();
                var 间隔 = (宽度 - 每个内容的长度) / (内容.Count - 1);
                for (int i = 0; i < 内容.Count; i++) {
                    A.Append(内容[i]);
                    if (i != 内容.Count - 1) {
                        A.Append(new string(' ', 间隔));
                    }
                }
            }
            return A.ToString();
        }
        public string 分割线(分割线形态 形态, int 宽度) {
            var A = "";
            if (形态 == 分割线形态.等号) {
                A = new string('=', 宽度);
            } else if (形态 == 分割线形态.减号) {
                A = new string('-', 宽度);
            }
            return A;
        }
    }
    public enum MsgType {
        无,
        有序,
        无序
    }
    public class 分割线类 {
        public 分割线形态 形态;
    }
    public enum 分割线形态 {
        无,
        等号,
        减号
    }
}
