using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using IDACSharp;
using VBKiller.Entity;
using VBKiller;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
                test2();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            Console.WriteLine("OK");
            Console.ReadKey();
        }

        static void test1()
        {
            //String file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharpLoader.dll");
            //Assembly asm = Assembly.LoadFile(file);
            //Type type = asm.GetType("CSharpLoader.Loader");
            //MethodInfo method = type.GetMethod("Init");
            //Boolean b = (Boolean)method.Invoke(null, null);

            //method = type.GetMethod("Start");
            //method.Invoke(null, null);

        }

        static void test2()
        {
            VBInfo.Test();
        }
    }
}
