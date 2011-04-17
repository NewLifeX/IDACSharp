using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace VBKiller.Entity
{
    /// <summary>
    /// 数据对象特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataObjectAttribute : Attribute
    {
        private Int32 _Size;
        /// <summary>大小</summary>
        public Int32 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// 取得指定成员上的数据特性
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static DataObjectAttribute GetAttribute(MemberInfo member)
        {
            return Attribute.GetCustomAttribute(member, typeof(DataObjectAttribute)) as DataObjectAttribute;
        }
    }
}
