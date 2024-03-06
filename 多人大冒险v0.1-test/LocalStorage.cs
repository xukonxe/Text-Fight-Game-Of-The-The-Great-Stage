using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public static partial class LocalStorage {
        public static void 打开商店(角色类 角色, 商店 商店) {
            //var 商品列表 = 商店.家具;
        }
        public static List<地图类> 获得所有子地图(this 地图类 上一个地图) {
            List<地图类> output = 上一个地图.获得所有出口();
            var 一阶子地图 = 上一个地图.获得所有出口();
            //如果获得所有出口不为空，那么就递归调用获得所有出口
            if (一阶子地图.Any()) {
                foreach (var 出口 in 一阶子地图) {
                    output.AddRange(出口.获得所有子地图());
                }
            }
            return output;
        }
    }
}
