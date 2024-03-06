using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMKZ.C02.多人大冒险01 {
    public interface IBuyAble {
        public string 名称 { get; }
        public double 价格 { get; set; }
    }
    public abstract class 道具类 {
        public abstract string 名称 { get; }
    }
    public abstract class 长剑类 : 道具类, IBuyAble {
        public double 价格 { get; set; }
        public abstract long 攻击力 { get; }
        public 长剑类() { }
    }
    public class 铁质长剑 : 长剑类 {
        public override string 名称 => "铁质长剑";
        public override long 攻击力 => 2;
        public 铁质长剑() {
            价格 = 10;
        }
    }
}
