using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 导出表
    /// <remarks>详见winnt.h中的IMAGE_EXPORT_DIRECTORY（10378行）</remarks>
    /// </summary>
    [DataObject(Size = 0x28)]
    public class ExportDirectory : EntityBase<ExportDirectory>
    {
        #region 属性
        private Int32 _Characteristics;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Characteristics
        {
            get { return _Characteristics; }
            set { _Characteristics = value; }
        }

        private Int32 _TimeDateStamp;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 TimeDateStamp
        {
            get { return _TimeDateStamp; }
            set { _TimeDateStamp = value; }
        }

        private Int16 _MajorVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MajorVersion
        {
            get { return _MajorVersion; }
            set { _MajorVersion = value; }
        }

        private Int16 _MinorVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MinorVersion
        {
            get { return _MinorVersion; }
            set { _MinorVersion = value; }
        }

        private Int32 _Name;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private Int32 _Base;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Base
        {
            get { return _Base; }
            set { _Base = value; }
        }

        private Int32 _NumberOfFunctions;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 NumberOfFunctions
        {
            get { return _NumberOfFunctions; }
            set { _NumberOfFunctions = value; }
        }

        private Int32 _NumberOfNames;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 NumberOfNames
        {
            get { return _NumberOfNames; }
            set { _NumberOfNames = value; }
        }

        private Int32 _AddressOfFunctions;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32), SizeField = "NumberOfFunctions")]
        public Int32 AddressOfFunctions
        {
            get { return _AddressOfFunctions; }
            set { _AddressOfFunctions = value; }
        }

        private Int32 _AddressOfNames;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32), SizeField = "NumberOfNames")]
        public Int32 AddressOfNames
        {
            get { return _AddressOfNames; }
            set { _AddressOfNames = value; }
        }

        private Int32 _AddressOfNameOrdinals;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32), SizeField = "NumberOfFunctions")]
        public Int32 AddressOfNameOrdinals
        {
            get { return _AddressOfNameOrdinals; }
            set { _AddressOfNameOrdinals = value; }
        }
        #endregion
    }
}
