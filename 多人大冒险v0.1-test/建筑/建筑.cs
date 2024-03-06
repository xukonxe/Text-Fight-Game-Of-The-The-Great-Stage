using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public abstract class 建筑类 : I实体 {
        public string 名称 => this.GetType().Name;
        public 坐标系<I实体> 坐标系 = new();
        public List<家具类> 所有家具 => 坐标系.所有物体<家具类>();
        public List<角色类> 所有角色 => 坐标系.所有物体<角色类>();
        public void 进入(角色类 角色) {
            角色.所在建筑 = this;
        }
        public void 离开(角色类 角色) {
            角色.所在建筑 = null;
        }
    }
    public class 商店 : 建筑类 { }
    public class 主城商店 : 建筑类 { }
}
