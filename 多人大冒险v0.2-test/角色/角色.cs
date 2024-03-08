using CMKZ.C02;
using CMKZ.C02.多人大冒险01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public class 角色类: I实体 {
        public bool is管理员 = false;
        public string 名称 => 账号数据.账号;
        public 账号信息 账号数据;
        public List<I可携带> 背包 = new();
        public I可搬运? 正在搬运;
        public 限数 生命 = new(100);
        public double 金币 = 999999999;
        public IUseable? 正使用;
        public 建筑类? 所在建筑;
        public bool 正在建筑内 => 所在建筑 != null;
        public DateTime 注册时间;
        public 角色类() {
            注册时间 = DateTime.Now;
        }
        public long 获取物品数量<T>() {
            return 背包.Count(i => i is T);
        }
        public long 获取物品数量(Type 类型) {
            return 背包.Count(i => i.GetType() == 类型);
        }
        public string 移除所有<T>() {
            var 物品 = 背包.FindAll(i => i is T);
            if (物品.Count != 0) {
                背包.RemoveAll(i => i is T);
                return "成功移除";
            } else {
                return "未找到物品";
            }
        }
        public string 移除物品(Type 类型) {
            var 物品 = 背包.FindAll(i => i.GetType() == 类型);
            if (物品.Count != 0) {
                背包.RemoveAll(i => i.GetType() == 类型);
                return "成功移除";
            } else {
                return "未找到物品";
            }
        }
        public string 移除物品<T>(long 数量) {
            var 物品 = 背包.FindAll(i => i is T);
            if (物品.Count >= 数量) {
                for (int i = 0; i < 数量; i++) {
                    背包.Remove(物品[i]);
                }
                return "成功移除";
            } else {
                return "未找到物品";
            }
        }
        public string 移除物品(Type 类型, long 数量) {
            var 物品 = 背包.FindAll(i => i.GetType() == 类型);
            if (物品.Count >= 数量) {
                for (int i = 0; i < 数量; i++) {
                    背包.Remove(物品[i]);
                }
                return "成功移除";
            } else {
                return "未找到物品";
            }
        }
        public string 添加物品(I可携带 物品) {
            背包.Add(物品);
            return "成功添加";
        }
        public string 搬运(I可搬运 物品) {
            if (正在搬运 == null) {
                正在搬运 = 物品;
                return "成功搬运";
            } else {
                return "已有物品正在搬运";
            }
        }
        public string 放下() {
            if (正在搬运 != null) {
                正在搬运 = null;
                return "成功放下";
            } else {
                return "未搬运物品";
            }
        }
    }
}
