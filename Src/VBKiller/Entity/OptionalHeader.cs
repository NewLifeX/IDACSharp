using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 可选头
    /// <remarks>详见winnt.h中的IMAGE_OPTIONAL_HEADER32（9440行）</remarks>
    /// </summary>
    [DataObject(Size = 0x60)]
    public class OptionalHeader : EntityBase<OptionalHeader>
    {
        #region 标准字段
        private Int16 _Magic;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Magic
        {
            get { return _Magic; }
            set { _Magic = value; }
        }

        private Byte _MajorLinkerVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte MajorLinkerVersion
        {
            get { return _MajorLinkerVersion; }
            set { _MajorLinkerVersion = value; }
        }

        private Byte _MinorLinkerVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte MinorLinkerVersion
        {
            get { return _MinorLinkerVersion; }
            set { _MinorLinkerVersion = value; }
        }

        private Int32 _SizeOfCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfCode
        {
            get { return _SizeOfCode; }
            set { _SizeOfCode = value; }
        }

        private Int32 _SizeOfInitializedData;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfInitializedData
        {
            get { return _SizeOfInitializedData; }
            set { _SizeOfInitializedData = value; }
        }

        private Int32 _SizeOfUninitializedData;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfUninitializedData
        {
            get { return _SizeOfUninitializedData; }
            set { _SizeOfUninitializedData = value; }
        }

        private Int32 _AddressOfEntryPoint;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 AddressOfEntryPoint
        {
            get { return _AddressOfEntryPoint; }
            set { _AddressOfEntryPoint = value; }
        }

        private Int32 _BaseOfCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 BaseOfCode
        {
            get { return _BaseOfCode; }
            set { _BaseOfCode = value; }
        }

        private Int32 _BaseOfData;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 BaseOfData
        {
            get { return _BaseOfData; }
            set { _BaseOfData = value; }
        }
        #endregion

        #region NT附加字段
        private Int32 _ImageBase;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 ImageBase
        {
            get { return _ImageBase; }
            set { _ImageBase = value; }
        }

        private Int32 _SectionAlignment;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SectionAlignment
        {
            get { return _SectionAlignment; }
            set { _SectionAlignment = value; }
        }

        private Int32 _FileAlignment;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 FileAlignment
        {
            get { return _FileAlignment; }
            set { _FileAlignment = value; }
        }

        private Int16 _MajorOperatingSystemVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MajorOperatingSystemVersion
        {
            get { return _MajorOperatingSystemVersion; }
            set { _MajorOperatingSystemVersion = value; }
        }

        private Int16 _MinorOperatingSystemVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MinorOperatingSystemVersion
        {
            get { return _MinorOperatingSystemVersion; }
            set { _MinorOperatingSystemVersion = value; }
        }

        private Int16 _MajorImageVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MajorImageVersion
        {
            get { return _MajorImageVersion; }
            set { _MajorImageVersion = value; }
        }

        private Int16 _MinorImageVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MinorImageVersion
        {
            get { return _MinorImageVersion; }
            set { _MinorImageVersion = value; }
        }

        private Int16 _MajorSubsystemVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MajorSubsystemVersion
        {
            get { return _MajorSubsystemVersion; }
            set { _MajorSubsystemVersion = value; }
        }

        private Int16 _MinorSubsystemVersion;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 MinorSubsystemVersion
        {
            get { return _MinorSubsystemVersion; }
            set { _MinorSubsystemVersion = value; }
        }

        private Int32 _Win32VersionValue;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Win32VersionValue
        {
            get { return _Win32VersionValue; }
            set { _Win32VersionValue = value; }
        }

        private Int32 _SizeOfImage;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfImage
        {
            get { return _SizeOfImage; }
            set { _SizeOfImage = value; }
        }

        private Int32 _SizeOfHeaders;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfHeaders
        {
            get { return _SizeOfHeaders; }
            set { _SizeOfHeaders = value; }
        }

        private Int32 _CheckSum;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 CheckSum
        {
            get { return _CheckSum; }
            set { _CheckSum = value; }
        }

        private Int16 _Subsystem;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Subsystem
        {
            get { return _Subsystem; }
            set { _Subsystem = value; }
        }

        private Int16 _DllCharacteristics;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 DllCharacteristics
        {
            get { return _DllCharacteristics; }
            set { _DllCharacteristics = value; }
        }

        private Int32 _SizeOfStackReserve;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfStackReserve
        {
            get { return _SizeOfStackReserve; }
            set { _SizeOfStackReserve = value; }
        }

        private Int32 _SizeOfStackCommit;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfStackCommit
        {
            get { return _SizeOfStackCommit; }
            set { _SizeOfStackCommit = value; }
        }

        private Int32 _SizeOfHeapReserve;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfHeapReserve
        {
            get { return _SizeOfHeapReserve; }
            set { _SizeOfHeapReserve = value; }
        }

        private Int32 _SizeOfHeapCommit;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 SizeOfHeapCommit
        {
            get { return _SizeOfHeapCommit; }
            set { _SizeOfHeapCommit = value; }
        }

        private Int32 _LoaderFlags;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 LoaderFlags
        {
            get { return _LoaderFlags; }
            set { _LoaderFlags = value; }
        }

        private Int32 _NumberOfRvaAndSizes;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 NumberOfRvaAndSizes
        {
            get { return _NumberOfRvaAndSizes; }
            set { _NumberOfRvaAndSizes = value; }
        }

        //private Int32 _DataDirectory;
        ///// <summary>属性说明</summary>
        //[DataField]
        //public Int32 DataDirectory
        //{
        //    get { return _DataDirectory; }
        //    set { _DataDirectory = value; }
        //}
        #endregion

        #region 扩展属性
        /// <summary>数据目录</summary>
        public DataDirectory[] DataDirectorys
        {
            get { return GetExtendList<DataDirectory>("DataDirectorys"); }
        }

        private ExportDirectory _Export;
        /// <summary>
        /// 导出表
        /// </summary>
        public ExportDirectory Export
        {
            get
            {
                if (_Export != null) return _Export;

                if (DataDirectorys == null || DataDirectorys.Length <= 0) return null;
                if (DataDirectorys[0].VirtualAddress <= 0) return null;

                _Export = new ExportDirectory();
                _Export.Info = Info;
                Seek(Reader, DataDirectorys[0].VirtualAddress);
                _Export.Read(Reader);

                return _Export;
            }
        }
        #endregion

        #region 方法
        public override void ReadExtend()
        {
            base.ReadExtend();

            // 读取数据目录
            ReadDataDirectorys();
        }

        protected override void TryReadExtend(string name)
        {
            if (name == "DataDirectorys")
                ReadDataDirectorys();
            else
                base.TryReadExtend(name);
        }

        private Boolean hasRead = false;
        void ReadDataDirectorys()
        {
            if (hasRead) return;
            hasRead = true;

            Seek(Reader, Address + OptionalHeader.ObjectSize);
            DataDirectory entity = new DataDirectory();
            entity.Info = Info;
            EntityBase2[] list = entity.ReadExtendList(Reader, 16);

            Extends.Add("DataDirectorys", list);
        }
        #endregion
    }
}
