using System;
using CMKZ;
using static CMKZ.LocalStorage;
using CMKZ.C02;
using static CMKZ.C02.LocalStorage;
using CMKZ.C02.多人大冒险01;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace CMKZ {
    public interface I指令 {
        public string Invoke();
    }
    public class 注释Attribute : Attribute {
        public string 内容;
        public 注释Attribute(string 内容) {
            this.内容 = 内容;
        }
    }
    public static partial class LocalStorage {
        public static 参数匹配控制台 控制台 = new();
        public static void 控制台执行(string X) {
            控制台.控制台执行(X);
        }
        public static Dictionary<Type, Action<I指令>> OnExec = new();
        public static void SetOnCommandExec<T>(Action<T> X) where T : class, I指令 {
            OnExec[typeof(T)] += t => X(t as T);
        }
    }
    public class 参数匹配控制台 {
        public List<I指令> 所有指令 = new();
        public Dictionary<Type, Func<string, (bool 判断,string 消息,object 实例)>> 转化器 = new();
        public string[] 所有指令名称;
        public bool IsInit;
        public Action<string> LogDebug = t =>   Print(t);
        public Action<string> LogWarning = t => Print("Warning:" + t);
        public Action<string> LogError = t =>   Print("Error:" + t);
        public void 初始化指令() {
            //不能用subclass，因为现在用的是接口。
            //Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(I指令))).ForEach(t => 所有指令.Add((I指令)Activator.CreateInstance(t)));
            //这样就可以了：
            Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(I指令))).ForEach(t => 所有指令.Add((I指令)Activator.CreateInstance(t)));
            所有指令名称 = 所有指令.Select(t => t.GetType().Name.Remove("指令") + " " + t.GetType().GetProperties().Select(t => t.GetCustomAttribute<注释Attribute>() == null ? t.PropertyType.BaseString() : t.GetCustomAttribute<注释Attribute>().内容).Join(" ")).ToArray();
            转化器[typeof(string)] = t => (true, "", t);
            转化器[typeof(int)] = t => (true, "", Convert.ChangeType(t, typeof(int)));
            转化器[typeof(long)] = t => (true, "", Convert.ChangeType(t, typeof(long)));
            转化器[typeof(double)] = t => (true, "", Convert.ChangeType(t, typeof(double)));
            转化器[typeof(uint)] = t => (true, "", Convert.ChangeType(t, typeof(uint)));
            转化器[typeof(short)] = t => (true, "", Convert.ChangeType(t, typeof(short)));
            转化器[typeof(ulong)] = t => (true, "", Convert.ChangeType(t, typeof(ulong)));
            转化器[typeof(float)] = t => (true, "", Convert.ChangeType(t, typeof(float)));
            转化器[typeof(ushort)] = t => (true, "", Convert.ChangeType(t, typeof(ushort)));
            转化器[typeof(char)] = t => (true, "", Convert.ChangeType(t, typeof(char)));
        }
        public I指令 匹配指令(string X) {
            foreach (var a in 所有指令) {
                if (a.GetType().Name.Remove("指令") == X) return a;
            }
            return null;
        }
        public string 匹配参数(string X, I指令 指令) {
            var 整段指令 = X.Split(' ');
            var 参数数量= 整段指令.Length - 1;
            var A = 指令.GetType().GetProperties();
            var 所有字段 = 指令.GetType().GetFields().Where(t => t.Name.StartsWith("D"));
            if (参数数量 != 所有字段.Count()) {
                return $"错误：指令【{指令}】需要{所有字段.Count()}个参数，而非{参数数量}个";
            }
            for (int i = 1; i < 整段指令.Length; i++) {
                var 当前字段 = 所有字段.Find(t => t.Name == "D" + i);
                if (!转化器.ContainsKey(当前字段.FieldType)) {
                    //LogWarning($"并未注册参数类型 {当前字段.FieldType} 的转化器！");
                    return "错误0002，请联系管理员！";
                }
                var value = 转化器[当前字段.FieldType](整段指令[i]);
                if (!value.判断) {
                    return value.消息;
                }
                当前字段.SetValue(指令, value.实例);
            }
            return 指令.Invoke();
        }
        public string 控制台执行(string X) {
            if (!IsInit) {
                IsInit = true;
                初始化指令();
            }
            var A = 匹配指令(X.Split(' ')[0]);
            if (A == null) {
                //LogWarning($"未找到指令");
                return "未找到指令，输入【帮助】查看帮助";
            } else {
                OnExec[A.GetType()]?.Invoke(A);
            }
            return 匹配参数(X, A);
        }
    }
}