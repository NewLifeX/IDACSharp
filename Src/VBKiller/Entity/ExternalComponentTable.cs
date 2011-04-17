using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 从VBHEADER->ExternalComponentTable可以获得外部引用表的地址指针
    /// </summary>
    /// <remarks>这是ExternalTable以及ExternalLibrary的VB结构以及C语言描述</remarks>
    [DataObject(Size = 0x34)]
    public class ExternalComponentTable : EntityBase<ExternalComponentTable>
    {
        #region 属性
        private Int32 _StructLength;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Relative)]
        public Int32 StructLength
        {
            get { return _StructLength; }
            set { _StructLength = value; }
        }

        private Int32 _l1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l1
        {
            get { return _l1; }
            set { _l1 = value; }
        }

        private Int32 _l2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l2
        {
            get { return _l2; }
            set { _l2 = value; }
        }

        private Int32 _l3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l3
        {
            get { return _l3; }
            set { _l3 = value; }
        }

        private Int32 _l4;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l4
        {
            get { return _l4; }
            set { _l4 = value; }
        }

        private Int32 _l5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l5
        {
            get { return _l5; }
            set { _l5 = value; }
        }

        private Int32 _l6;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l6
        {
            get { return _l6; }
            set { _l6 = value; }
        }

        private Int32 _GuidOffset;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Relative)]
        public Int32 GuidOffset
        {
            get { return _GuidOffset; }
            set { _GuidOffset = value; }
        }

        private Int32 _GUIDlength;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 GUIDlength
        {
            get { return _GUIDlength; }
            set { _GUIDlength = value; }
        }

        private Int32 _l7;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 l7
        {
            get { return _l7; }
            set { _l7 = value; }
        }

        private Int32 _FileName;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private Int32 _Source;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private Int32 _Name;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>名称</summary>
        public String FileName2
        {
            get { return (String)this["FileName"]; }
        }

        /// <summary>名称</summary>
        public String Source2
        {
            get { return (String)this["Source"]; }
        }
        
        /// <summary>名称</summary>
        public String Name2
        {
            get { return (String)this["Name"]; }
        }
        #endregion

        #region 方法
        public override string ToString()
        {
            return Name2;
        }
        #endregion
    }
}
