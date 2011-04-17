using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using IDACSharp;
using System.IO;
using System.Collections.Generic;

namespace CSharpLoader
{
    /// <summary>
    /// 加载器
    /// </summary>
    public class Loader
    {
        #region 主函数
        public static Boolean Init()
        {
            try
            {
                WriteLine("开始加载插件……");

                List<IPlugin> list = LoadPlugins();

                if (list != null && list.Count > 0)
                {
                    Plugins = new List<IPlugin>();

                    foreach (IPlugin item in list)
                    {
                        WriteLine("加载插件 {0}", item.Name);

                        try
                        {
                            if (item.Init()) Plugins.Add(item);
                        }
                        catch (Exception ex)
                        {
                            WriteLine("加载插件{0}失败！{1}", item.Name, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine("加载插件失败！{0}", ex.Message);
            }

            return true;
        }

        public static void Start()
        {
            //KernalWin.Msg("欢迎使用IDACSharp插件！");
            //MessageBox.Show("欢迎使用IDACSharp插件！");
            //ShowIdaInfo();

            //String file = @"CSharp\IDATest.dll";
            //file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            //Assembly asm = Assembly.LoadFile(file);
            //Type type = asm.GetType("IDATest.CPlugin");
            //MethodInfo method = type.GetMethod("StartPlugin");
            //method.Invoke(null, null);

            FrmPlugin frm = new FrmPlugin();
            frm.Show();
        }

        public static void Term()
        {
            try
            {
                WriteLine("开始卸载插件……");

                if (Plugins != null && Plugins.Count > 0)
                {
                    foreach (IPlugin item in Plugins)
                    {
                        try
                        {
                            item.Term();
                        }
                        catch (Exception ex)
                        {
                            WriteLine("加载卸载{0}失败！{1}", item.Name, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine("卸载插件失败！{0}", ex.Message);
            }
        }
        #endregion

        #region 插件加载
        /// <summary>
        /// 插件集合
        /// </summary>
        public static List<IPlugin> Plugins;

        static List<IPlugin> LoadPlugins()
        {
            List<Type> list = LoadTypes();
            if (list == null || list.Count < 1) return null;

            List<IPlugin> plugins = new List<IPlugin>();
            foreach (Type item in list)
            {
                try
                {
                    IPlugin entity = Activator.CreateInstance(item) as IPlugin;
                    if (entity != null) plugins.Add(entity);
                }
                catch (Exception ex)
                {
                    WriteLine("加载插件【{0}】失败！{1}", item.FullName, ex.Message);
                }
            }

            if (plugins.Count <= 0) return null;

            return plugins;
        }

        static List<Type> LoadTypes()
        {
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharp");
            if (!Directory.Exists(path)) return null;

            List<Type> list = new List<Type>();

            String[] files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            foreach (String item in files)
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(item);
                    if (asm == Assembly.GetExecutingAssembly()) continue;

                    WriteLine("加载 {0}", item);

                    List<Type> types = LoadTypes(asm);
                    if (types != null && types.Count > 0) list.AddRange(types);
                }
                catch { }
            }

            if (list.Count < 1)
                return null;
            else
                return list;
        }

        static List<Type> LoadTypes(Assembly asm)
        {
            if (asm == null) return null;

            Type[] types = asm.GetTypes();
            if (types == null || types.Length <= 0) return null;

            List<Type> list = new List<Type>();

            //查找插件类型，所有实现了IPlugin的类
            foreach (Type item in types)
            {
                if (IsPlugin(item)) list.Add(item);
            }

            if (list.Count < 1)
                return null;
            else
                return list;
        }

        /// <summary>
        /// 是否插件类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Boolean IsPlugin(Type type)
        {
            //为空、不是类、抽象类 都不是插件类
            if (type == null || !type.IsClass || type.IsAbstract) return false;

            //递归判断
            Type t = type;
            while (t != null && t != typeof(Object))
            {
                if (Array.IndexOf<Type>(t.GetInterfaces(), typeof(IPlugin)) >= 0) return true;
                t = t.BaseType;
            }

            return false;
        }
        #endregion

        static void ShowIdaInfo()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                FieldInfo[] pis = typeof(IdaInfo).GetFields();
                foreach (FieldInfo item in pis)
                {
                    Object obj = item.GetValue(IdaInfo.Instance);
                    if (item.FieldType == typeof(long))
                        sb.AppendFormat("{0}=0x{1:X}", item.Name, obj);
                    else
                        sb.AppendFormat("{0}={1}", item.Name, obj);
                    sb.AppendLine();
                }

                MessageBox.Show(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static void WriteLine(String msg)
        {
            KernelWin.Msg(msg + Environment.NewLine);
        }

        private static void WriteLine(String format, params Object[] args)
        {
            KernelWin.Msg(format + Environment.NewLine, args);
        }
    }
}
