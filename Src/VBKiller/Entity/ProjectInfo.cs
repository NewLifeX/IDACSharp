using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 工程信息
    /// </summary>
    [DataObject(Size = 0x23C)]
    public class ProjectInfo : EntityBase<ProjectInfo>
    {
        #region 属性
        private Int32 _Signature;
        /// <summary>结构的签名特性，和魔术字符类似</summary>
        [DataField]
        public Int32 Signature
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        private Int32 _ObjectTable;
        /// <summary>VA 结构指向的组件列表的地址指针（很重要的！（7.））</summary>
        [DataField(typeof(ObjectTable), RefKinds.Virtual)]
        public Int32 ObjectTable
        {
            get { return _ObjectTable; }
            set { _ObjectTable = value; }
        }

        private Int32 _Null1;
        /// <summary>没有用的东西</summary>
        [DataField]
        public Int32 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _StartOfCode;
        /// <summary>VA 代码开始点,类似PEHEAD->EntryPoint这里告诉了VB代码实际的开始点</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 StartOfCode
        {
            get { return _StartOfCode; }
            set { _StartOfCode = value; }
        }

        private Int32 _EndOfCode;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 EndOfCode
        {
            get { return _EndOfCode; }
            set { _EndOfCode = value; }
        }

        private Int32 _DataSize;
        /// <summary>Size of VB Object Structures. Unused</summary>
        [DataField]
        public Int32 DataSize
        {
            get { return _DataSize; }
            set { _DataSize = value; }
        }

        private Int32 _ThreadSpace;
        /// <summary>多线程的空间</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ThreadSpace
        {
            get { return _ThreadSpace; }
            set { _ThreadSpace = value; }
        }

        private Int32 _VBAExceptionHandler;
        /// <summary>VA VBA意外处理机器地址指针</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 VBAExceptionHandler
        {
            get { return _VBAExceptionHandler; }
            set { _VBAExceptionHandler = value; }
        }

        private Int32 _NativeCode;
        /// <summary>VA 本地机器码开始位置的地址指针</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 NativeCode
        {
            get { return _NativeCode; }
            set { _NativeCode = value; }
        }

        private Int16 _ProjectLocation;
        /// <summary>Offset 工程位置</summary>
        [DataField(RefType = typeof(Int32))]
        public Int16 ProjectLocation
        {
            get { return _ProjectLocation; }
            set { _ProjectLocation = value; }
        }

        private Int16 _Flag2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Flag2
        {
            get { return _Flag2; }
            set { _Flag2 = value; }
        }

        private Int16 _Flag3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Flag3
        {
            get { return _Flag3; }
            set { _Flag3 = value; }
        }

        private String _OriginalPathName;
        /// <summary>原文件地址,一个字符串,长度最长为MAX_PATH</summary>
        [DataField(Size = 521)]
        public String OriginalPathName
        {
            get { return _OriginalPathName; }
            set { _OriginalPathName = value; }
        }

        private Byte _NullSpacer;
        /// <summary>无用的东西,用来占位置</summary>
        [DataField]
        public Byte NullSpacer
        {
            get { return _NullSpacer; }
            set { _NullSpacer = value; }
        }

        private Int32 _ExternalTable;
        /// <summary>VA 引用表的指针地址</summary>
        [DataField(RefType = typeof(ExternalTable), RefKind = RefKinds.Virtual, SizeField = "ExternalCount")]
        public Int32 ExternalTable
        {
            get { return _ExternalTable; }
            set { _ExternalTable = value; }
        }

        private Int32 _ExternalCount;
        /// <summary>引用表大小(个数)</summary>
        [DataField]
        public Int32 ExternalCount
        {
            get { return _ExternalCount; }
            set { _ExternalCount = value; }
        }
        #endregion

        #region 扩展属性
        ///// <summary>引用表</summary>
        //public ExternalTable ExternalTable2
        //{
        //    get { return this["ExternalTable"] as ExternalTable; }
        //}

        /// <summary>组件列表</summary>
        public ObjectTable ObjectTable2
        {
            get { return this["ObjectTable"] as ObjectTable; }
        }

        /// <summary>
        /// 引用库集合
        /// </summary>
        public ExternalTable[] ExternalTables
        {
            get { return GetExtendList<ExternalTable>("ExternalTable"); }
        }
        #endregion

        #region 方法
        //public override void ShowExtend(bool isShowExtend)
        //{
        //    //base.ShowExtend(isShowExtend);

        //    if (ObjectTable2 != null)
        //    {
        //        WriteLine();
        //        ObjectTable2.Show(isShowExtend);
        //    }

        //    if (ExternalTables != null && ExternalTables.Length > 0)
        //    {
        //        WriteLine();
        //        foreach (ExternalTable item in ExternalTables)
        //        {
        //            WriteLine("{0}.{1}", item.ExternalLibrary2.LibraryName2, item.ExternalLibrary2.LibraryFunction2);
        //        }
        //    }
        //}

        protected override void ShowExtendProperty(String name, bool isShowExtend)
        {
            if (name == "ExternalTable")
            {
                if (ExternalTables != null && ExternalTables.Length > 0)
                {
                    WriteLine();
                    WriteLine("ExternalTables:");
                    for (int i = 0; i < ExternalTables.Length; i++)
                    {
                        WriteLine("{0}.{1}", ExternalTables[i].ExternalLibrary2.LibraryName2, ExternalTables[i].ExternalLibrary2.LibraryFunction2);
                    }
                }

                return;
            }

            base.ShowExtendProperty(name, isShowExtend);
        }
        #endregion
    }
}
