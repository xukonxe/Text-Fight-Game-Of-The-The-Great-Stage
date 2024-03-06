using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer

namespace CMKZ {
    public static partial class LocalStorage {
        //public static int GetHash(int X) {
        //    //哈希运算：即混沌运算。把一个数变成另一个数。输入信息的一点微小变化，都会导致输出的巨大变化，变化的毫无规律、但又确切唯一。
        //    byte[] dataBytes = BitConverter.GetBytes(X);
        //    byte[] hashBytes = SHA256.Create().ComputeHash(dataBytes);
        //    return int.Parse(BitConverter.ToString(hashBytes).Replace("-", string.Empty).Substring(0, 8));
        //}
        //public static int RandomBySeed(this int 标准值, params int[] 种子) {
        //    //要求：【标准值、小种子、种子】确定唯一结果。结果接近标准值，接近程度与小种子线性无关、与种子线性无关、与标准值线性无关。
        //    int A = 0;
        //    foreach (var i in 种子) {
        //        A += GetHash(i);
        //    }
        //    return Math.Max(1, 标准值 * Math.Max(2, (A + GetHash(标准值)) % 100) / 20);
        //}
        ///<summary>0到1的随机数</summary>
        //public static float RandomFloat() {
        //    return Random(0.0f, 1.0f);
        //}
    }
}