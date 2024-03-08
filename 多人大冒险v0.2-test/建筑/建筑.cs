using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CMKZ.C02.多人大冒险01.LocalStorage;

namespace CMKZ.C02.多人大冒险01 {
    public abstract class 建筑类 : I实体 {
        public string 名称 => this.GetType().Name;
        public string? 所有者;
        public 坐标系<I实体> 坐标系 = new(16, 16);
        public List<家具类> 所有家具 => 坐标系.所有物体<家具类>();
        public List<角色类> 所有角色 => 坐标系.所有物体<角色类>();
        public abstract string 进入消息();
        public void 进入(角色类 角色) {
            角色.所在建筑 = this;
            坐标系.Add(0, -8, 角色);
        }
        public void 离开(角色类 角色) {
            角色.所在建筑 = null;
            坐标系.Remove(角色);
        }
    }
    public abstract class 商店 : 建筑类 {
        public override abstract string 进入消息();
    }
    public class 主城商店 : 建筑类 {
        public override string 进入消息() {
            //显示附近家具和玩家
            var 所有家具 = 坐标系.所有物体<家具类>();
            var 所有玩家 = 坐标系.所有物体<角色类>();
            var A = new StringBuilder();
            A.AppendLine("欢迎来到主城商店");
            return A.ToString();
        }
    }
    public class 纪念堂 : 建筑类 {
        public string? 纪念名;
        public 纪念堂(string 所有者) {
            坐标系.Add(0, 0, new 大幅相片());
            坐标系.Add(-2, 0, new 花圈());
            坐标系.Add(2, 0, new 花圈());
            this.所有者 = 所有者;
        }
        public 纪念堂(string 人名, string 所有者) {
            坐标系.Add(0, 0, new 大幅相片($"{人名} 的大头像"));
            坐标系.Add(-2, 0, new 花圈());
            坐标系.Add(2, 0, new 花圈());
            this.所有者 = 所有者;
        }
        public string 设置纪念(string 纪念名) {
            this.纪念名 = 纪念名;
            //如果地图内包含大幅相片，就设置大幅相片的名称
            var 大幅相片 = 坐标系.所有物体<大幅相片>().FirstOrDefault();
            if (大幅相片 != null) {
                大幅相片.图片内容 = $"{纪念名} 的黑白大头像，容貌庄严，令人尊敬";
                return $"已设置为 {纪念名} 的纪念堂";
            }
            return "地图内无大头照，已设置纪念堂名，但无法设置大头照";
        }
        public override string 进入消息() {
            //如果纪念名为空，就返回默认消息
            if (纪念名 == null) {
                return "欢迎来到纪念堂，这里可以设置纪念堂名。输入【设置纪念 人名】来设置！";
            }
            var 所有家具 = 坐标系.所有物体<家具类>();
            return $"欢迎来到 {纪念名} 的纪念堂。这里庄严而神圣，有着无数人的缅怀。建筑内有{所有家具.Select(t => t.名称).Join("、")}。";
        }
    }
    public interface IChatRoom {
        public string 聊天记录 { get; set; }
        public int 清除消息行数 { get; set; }
        public List<角色类> 所有角色 { get; }
        public string 说话(角色类 说话者, string 消息) {
            var 时间 = DateTime.Now;
            //时间格式：x月x日x时x分
            var 聊天消息 = 时间.ToString("[yyyy/M/d H:m]");
            聊天消息 += $"{说话者.名称}：{消息}" + "\n";
            聊天记录 += 聊天消息;
            //如果\n的数量超过100，就删除前面的
            if (聊天记录.Count(t => t == '\n') > 100) {
                var A = 聊天记录.Split('\n').ToList();
                var 丢弃后的消息 = A.Skip(A.Count - 清除消息行数).Prepend("---超过100行的消息已丢弃---").ToList();
                聊天记录 = string.Join("\n", 丢弃后的消息);
            }
            广播消息(聊天消息);
            return $"您在聊天室里说了{消息}";
        }
        public string 查看聊天记录() {
            return 聊天记录;
        }
        public 玩家列表消息类 广播消息(string 消息) {
            var 聊天消息 = "【聊天室消息】\n";
            聊天消息 += 消息;
            return new 玩家列表消息类(所有角色, 聊天消息);
        }
    }
    public class 聊天室 : 建筑类, IChatRoom {
        public string 聊天记录 { get; set; } = "";
        public int 清除消息行数 { get; set; } = 100;
        public List<角色类> 所有角色 => 坐标系.所有物体<角色类>();
        public 聊天室() {
            坐标系.Add(-7, -7, new 沙发椅());
            坐标系.Add(-7, -5, new 沙发椅());
            坐标系.Add(-7, -3, new 沙发椅());

            坐标系.Add(-5, -7, new 双人床());
            坐标系.Add(-5, -5, new 双人床());
            坐标系.Add(-5, -3, new 双人床());
        }
        public override string 进入消息() {
            return "欢迎来到聊天室，这里可以和其他人聊天。输入【说话 消息】来说话！";
        }
    }
    public class 澡堂 : 建筑类 {
        public 澡堂() {
            坐标系.Add(-7, -7, new 淋浴房());
            坐标系.Add(-5, -7, new 淋浴房());
            坐标系.Add(-3, -7, new 淋浴房());

            坐标系.Add(-7, -5, new 淋浴房());
            坐标系.Add(-5, -5, new 淋浴房());
            坐标系.Add(-3, -5, new 淋浴房());

            坐标系.Add(-7, -3, new 淋浴房());
            坐标系.Add(-5, -3, new 淋浴房());
            坐标系.Add(-3, -3, new 淋浴房());
        }
        public override string 进入消息() {
            return "欢迎来到澡堂，这里可以洗澡。可以洗掉身上的脏东西。输入【使用 淋浴房】来洗澡！";
        }
    }
}
