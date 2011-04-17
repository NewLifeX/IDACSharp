using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace VBKiller.Entity
{
    /// <summary>
    /// 数据字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataFieldAttribute : Attribute
    {
        #region 属性
        private Int32 _Size;
        /// <summary>大小</summary>
        public Int32 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        private Type _RefType;
        /// <summary>引用类型</summary>
        public Type RefType
        {
            get { return _RefType; }
            set { _RefType = value; }
        }

        private RefKinds _RefKind;
        /// <summary>引用地址类型</summary>
        public RefKinds RefKind
        {
            get { return _RefKind; }
            set { _RefKind = value; }
        }

        private String _SizeField;
        /// <summary>该数据字段（比如字符串）的长度由另一数据字段决定</summary>
        public String SizeField
        {
            get { return _SizeField; }
            set { _SizeField = value; }
        }

        private Boolean _IsParent;
        /// <summary>是否父级引用。该属性主要用于解决循环引用的问题</summary>
        public Boolean IsParent
        {
            get { return _IsParent; }
            set { _IsParent = value; }
        }
        #endregion

        #region 构造函数
        public DataFieldAttribute() { }

        public DataFieldAttribute(Type type, RefKinds kind)
        {
            RefType = type;
            RefKind = kind;
        }
        #endregion

        /// <summary>
        /// 取得指定成员上的数据字段特性
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static DataFieldAttribute GetAttribute(MemberInfo member)
        {
            return Attribute.GetCustomAttribute(member, typeof(DataFieldAttribute)) as DataFieldAttribute;
        }
    }

    /// <summary>
    /// 引用地址类型
    /// </summary>
    public enum RefKinds
    {
        /// <summary>
        /// 自动判断
        /// </summary>
        Auto,

        /// <summary>
        /// 虚拟绝对
        /// </summary>
        Virtual,

        /// <summary>
        /// 相对
        /// </summary>
        Relative,

        /// <summary>
        /// 绝对
        /// </summary>
        Absolute
    }
}
