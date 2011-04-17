using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// DOS头
    /// <remarks>详见winnt.h中的IMAGE_DOS_HEADER（9209）</remarks>
    /// </summary>
    [DataObject(Size = 0x40)]
    public class DosHeader : EntityBase<DosHeader>
    {
        #region 属性
        private String _Magic;
        /// <summary>属性说明</summary>
        [DataField(Size = 2)]
        public String Magic
        {
            get { return _Magic; }
            set { _Magic = value; }
        }

        private Int16 _LastPageBytes;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 LastPageBytes
        {
            get { return _LastPageBytes; }
            set { _LastPageBytes = value; }
        }

        private Int16 _Pages;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Pages
        {
            get { return _Pages; }
            set { _Pages = value; }
        }

        private Int16 _Relocations;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Relocations
        {
            get { return _Relocations; }
            set { _Relocations = value; }
        }

        private Int16 _ParagraphsSize;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 ParagraphsSize
        {
            get { return _ParagraphsSize; }
            set { _ParagraphsSize = value; }
        }

        private Int16 _Minalloc;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Minalloc
        {
            get { return _Minalloc; }
            set { _Minalloc = value; }
        }

        private Int16 _Maxalloc;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Maxalloc
        {
            get { return _Maxalloc; }
            set { _Maxalloc = value; }
        }

        private Int16 _SS;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SS
        {
            get { return _SS; }
            set { _SS = value; }
        }

        private Int16 _SP;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SP
        {
            get { return _SP; }
            set { _SP = value; }
        }

        private Int16 _Checksum;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Checksum
        {
            get { return _Checksum; }
            set { _Checksum = value; }
        }

        private Int16 _IP;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        private Int16 _CS;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 CS
        {
            get { return _CS; }
            set { _CS = value; }
        }

        private Int16 _RelocationTable;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 RelocationTable
        {
            get { return _RelocationTable; }
            set { _RelocationTable = value; }
        }

        private Int16 _OverlayNumber;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 OverlayNumber
        {
            get { return _OverlayNumber; }
            set { _OverlayNumber = value; }
        }

        private Byte[] _ReservedWords;
        /// <summary>属性说明</summary>
        [DataField(Size = 8)]
        public Byte[] ReservedWords
        {
            get { return _ReservedWords; }
            set { _ReservedWords = value; }
        }

        private Int16 _OEMID;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 OEMID
        {
            get { return _OEMID; }
            set { _OEMID = value; }
        }

        private Int16 _OEMInfo;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 OEMInfo
        {
            get { return _OEMInfo; }
            set { _OEMInfo = value; }
        }

        private Byte[] _ReservedWords2;
        /// <summary>属性说明</summary>
        [DataField(Size = 20)]
        public Byte[] ReservedWords2
        {
            get { return _ReservedWords2; }
            set { _ReservedWords2 = value; }
        }

        private Int32 _NewExeHeader;
        /// <summary>属性说明</summary>
        [DataField(typeof(FileHeader), RefKinds.Absolute)]
        //[DataField]
        public Int32 NewExeHeader
        {
            get { return _NewExeHeader; }
            set { _NewExeHeader = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>新文件头</summary>
        public FileHeader FileHeader
        {
            get { return this["NewExeHeader"] as FileHeader; }
        }

        /// <summary>可选文件头</summary>
        public OptionalHeader OptionalHeader
        {
            get { return this["OptionalHeader"] as OptionalHeader; }
        }
        #endregion

        #region 方法
        public override void ReadExtend()
        {
            base.ReadExtend();

            // 读取可选对象信息
            ReadOptionalHeader();
        }

        protected override void TryReadExtend(string name)
        {
            if (name == "OptionalHeader")
                ReadOptionalHeader();
            else
                base.TryReadExtend(name);
        }

        private Boolean hasRead = false;
        void ReadOptionalHeader()
        {
            if (hasRead) return;
            hasRead = true;

            Seek(Reader, NewExeHeader + FileHeader.ObjectSize);
            OptionalHeader entity = new OptionalHeader();
            entity.Info = Info;
            entity.Read(Reader);

            Extends.Add("OptionalHeader", entity);
        }
        #endregion
    }
}
