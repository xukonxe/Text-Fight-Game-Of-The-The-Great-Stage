using CMKZ.C02;
using CMKZ.C02.多人大冒险01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public class 角色类: I实体 {
        public string 名称 => 账号数据.账号;
        public 账号信息 账号数据;
        public List<道具类> 背包 = new();
        public 限数 生命 = new(100);
        public double 金币 = 0;
        public ISitable? 正坐在;
        public IUseable? 正使用;
        public 建筑类? 所在建筑;
        public bool 正在建筑内 => 所在建筑 != null;
        public DateTime 注册时间;
        public 角色类() {
            注册时间 = DateTime.Now;
        }
    }
}
