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
        //    //��ϣ���㣺���������㡣��һ���������һ������������Ϣ��һ��΢С�仯�����ᵼ������ľ޴�仯���仯�ĺ��޹��ɡ�����ȷ��Ψһ��
        //    byte[] dataBytes = BitConverter.GetBytes(X);
        //    byte[] hashBytes = SHA256.Create().ComputeHash(dataBytes);
        //    return int.Parse(BitConverter.ToString(hashBytes).Replace("-", string.Empty).Substring(0, 8));
        //}
        //public static int RandomBySeed(this int ��׼ֵ, params int[] ����) {
        //    //Ҫ�󣺡���׼ֵ��С���ӡ����ӡ�ȷ��Ψһ���������ӽ���׼ֵ���ӽ��̶���С���������޹ء������������޹ء����׼ֵ�����޹ء�
        //    int A = 0;
        //    foreach (var i in ����) {
        //        A += GetHash(i);
        //    }
        //    return Math.Max(1, ��׼ֵ * Math.Max(2, (A + GetHash(��׼ֵ)) % 100) / 20);
        //}
        ///<summary>0��1�������</summary>
        //public static float RandomFloat() {
        //    return Random(0.0f, 1.0f);
        //}
    }
}