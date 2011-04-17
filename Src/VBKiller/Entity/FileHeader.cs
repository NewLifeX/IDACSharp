using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 文件头
    /// <remarks>详见winnt.h中的IMAGE_FILE_HEADER（9326行）</remarks>
    /// </summary>
    [DataObject(Size = 0x18)]
    public class FileHeader : EntityBase<FileHeader>
    {
        #region 属性
        private String _Signature;
        /// <summary>属性说明</summary>
        [DataField(Size = 4)]
        public String Signature
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        private Int16 _Machine;
        /// <summary>机器</summary>
        [DataField]
        public Int16 Machine
        {
            get { return _Machine; }
            set { _Machine = value; }
        }

        private Int16 _NumberOfSections;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 NumberOfSections
        {
            get { return _NumberOfSections; }
            set { _NumberOfSections = value; }
        }

        private Int32 _TimeDateStamp;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 TimeDateStamp
        {
            get { return _TimeDateStamp; }
            set { _TimeDateStamp = value; }
        }

        private Int32 _PointerToSymbolTable;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 PointerToSymbolTable
        {
            get { return _PointerToSymbolTable; }
            set { _PointerToSymbolTable = value; }
        }

        private Int32 _NumberOfSymbols;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 NumberOfSymbols
        {
            get { return _NumberOfSymbols; }
            set { _NumberOfSymbols = value; }
        }

        private Int16 _SizeOfOptionalHeader;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeOfOptionalHeader
        {
            get { return _SizeOfOptionalHeader; }
            set { _SizeOfOptionalHeader = value; }
        }

        private Int16 _Characteristics;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Characteristics
        {
            get { return _Characteristics; }
            set { _Characteristics = value; }
        }
        #endregion
    }
}
