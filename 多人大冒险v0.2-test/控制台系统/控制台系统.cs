using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMKZ;
using static CMKZ.LocalStorage;
using CMKZ.C02;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class 控制台系统 {
        public static string 启动() {
            控制台.转化器[typeof(Vector2Int)]= (X) => {
                //原始数据：(1,1)
                var A = X.Remove("（").Remove("）").Remove("(").Remove(")").Replace("，",",").Split(',');
                //如果A中包含非数字字符，那么报错
                if (A.Any(t => !t.All(char.IsDigit))) {
                    return (false, "请输入正确的坐标格式。例：(1,1)", new());
                }
                return (true, "", new Vector2Int(int.Parse(A[0]), int.Parse(A[1])));
            };
            return "控制台已启动";
        }
        public static string 查询游戏信息(账号信息 K) {
            var 玩家 = 游戏系统.获取玩家(K);
            var A = new 游戏信息();
            A.玩家 = 玩家;
            if (游戏系统.当前位置(A.玩家) == null && 游戏系统.世界.未出生玩家.Contains(玩家)) {
                A.当前位置 = null;
            }
            A.当前位置 = 游戏系统.当前位置(A.玩家);
            A.血量 = 玩家.生命;
            A.金币 = 玩家.金币;
            A.附近玩家 = 玩家.附近玩家();
            A.附近建筑 = 玩家.附近建筑();
            A.附近家具 = 玩家.附近家具();
            if (玩家.正在建筑内) {
                A.可用家具 = 玩家.所在建筑.所有家具;
                A.建筑角色 = 玩家.所在建筑.所有角色;
            }
            return A.ToString();
        }
        public static string 控制台执行(string X) {
           return 控制台.控制台执行(X);
        }
    }
    public class 帮助指令 : I指令 {
        public string D1;
        public string D2;
        public string Invoke() {
            var A = new StringBuilder();
            A.AppendLine("====可用指令列表====");
            A.AppendLine(控制台.所有指令名称.Join("\n"));
            A.AppendLine("====================");
            return A.ToString();
        }
    }
    public class 游戏信息 {
        public 角色类 玩家;
        public 地图类 当前位置;
        public 限数 血量;
        public double 金币;
        public List<角色类> 附近玩家;
        public List<建筑类> 附近建筑;
        public List<家具类> 附近家具;
        public List<家具类> 可用家具;
        public List<角色类> 建筑角色;
        public override string ToString() {
            if (玩家.正在建筑内) return 建筑中信息();
            return 地图中信息();
        }
        public string 地图中信息() {
            var A = new StringBuilder();
            A.AppendLine("=========控制台消息=========");
            if (玩家 == null) {
                A.AppendLine($"玩家：不存在此玩家");
            } else {
                A.AppendLine($"玩家：{玩家.账号数据.账号}");
            }
            if (当前位置 == null) {
                A.AppendLine($"状态：无位置");
            } else {
                A.AppendLine($"位置：{当前位置.名称} {当前位置.坐标系.物体坐标[玩家]}");
            }
            A.AppendLine($"血量：{血量}");
            A.AppendLine($"金币：{金币}");
            if (附近玩家.Any()) {
                A.AppendLine($"----------------------------");
                A.AppendLine($"附近玩家：");
                执行X次(附近玩家.Count, t => {
                    A.Append($"{t + 1}.{附近玩家[t].账号数据.账号}");
                    A.AppendLine($" {当前位置.坐标系.物体坐标[附近玩家[t]]}");
                });
                A.AppendLine($"----------------------------");
            }
            if (附近建筑.Any()) {
                A.AppendLine($"----------------------------");
                A.AppendLine($"附近建筑：");
                执行X次(附近建筑.Count, t => {
                    A.Append($"{t + 1}.{附近建筑[t].名称}");
                    A.AppendLine($" {当前位置.坐标系.物体坐标[附近建筑[t]]}");
                });

                A.AppendLine("指令提示：");
                A.AppendLine($"【进入建筑 建筑名】 ");
                A.AppendLine($"----------------------------");
            }
            if (附近家具.Any()) {
                A.AppendLine($"----------------------------");
                A.AppendLine($"附近家具：");
                执行X次(附近家具.Count, t => {
                    A.Append($"{t + 1}.{附近家具[t].名称}");
                    A.AppendLine($"{当前位置.坐标系.物体坐标[附近家具[t]]}");
                });
                A.AppendLine("指令提示：");
                A.AppendLine($"【使用家具 家具名");
                A.AppendLine($"----------------------------");
            }
            A.AppendLine("============================");
            if (当前位置 == null) {
                A.AppendLine($"请输入【进入主城】以出生。");
            }
            return A.ToString();
        }
        public string 建筑中信息() {
            var A = new StringBuilder();
            A.AppendLine("=========控制台消息=========");
            A.AppendLine($"当前正在建筑 {玩家.所在建筑.名称} 内");
            A.AppendLine($"玩家：{玩家.账号数据.账号}");
            A.AppendLine($"位置：{当前位置.名称} {玩家.所在建筑.名称}");
            A.AppendLine($"血量：{血量}");
            if (可用家具.Any()) {
                A.AppendLine($"----------------------------");
                A.AppendLine($"可用家具：");
                执行X次(可用家具.Count, t => {
                    A.AppendLine($"{t + 1}.{可用家具[t].名称}");
                });
                A.AppendLine($"----------------------------");
                A.AppendLine("指令提示：");
                A.AppendLine($"【使用家具 家具名】");
                A.AppendLine($"【退出建筑】");
            }
            if (附近玩家.Any()) {
                A.AppendLine($"----------------------------");
                A.AppendLine($"当前建筑内的玩家：");
                执行X次(附近玩家.Count, t => {
                    A.AppendLine($"{t + 1}.{附近玩家[t].账号数据.账号}");
                });
                A.AppendLine($"----------------------------");
            }
            A.Append("============================");
            return A.ToString();
        }
    }
}
