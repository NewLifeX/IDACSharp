using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x28)]
    public class ProjectInfo2 : EntityBase<ProjectInfo2>
    {
        #region 属性
        private Int32 _HeapLink;
        /// <summary>Unused after compilation, always 0</summary>
        [DataField]
        public Int32 HeapLink
        {
            get { return _HeapLink; }
            set { _HeapLink = value; }
        }

        private Int32 _ObjectTable;
        /// <summary>Back-Pointer to the Object Table</summary>
        [DataField(RefType = typeof(ObjectTable), RefKind = RefKinds.Virtual, IsParent = true)]
        public Int32 ObjectTable
        {
            get { return _ObjectTable; }
            set { _ObjectTable = value; }
        }

        private Int32 _Reserved;
        /// <summary>Always set to -1 after compiling. Unused</summary>
        [DataField]
        public Int32 Reserved
        {
            get { return _Reserved; }
            set { _Reserved = value; }
        }

        private Int32 _Null2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null2
        {
            get { return _Null2; }
            set { _Null2 = value; }
        }

        private Int32 _ObjectDescriptorTable;
        /// <summary>属性说明</summary>
        [DataField(typeof(PrivateObjectDescriptor), RefKinds.Virtual)]
        public Int32 ObjectDescriptorTable
        {
            get { return _ObjectDescriptorTable; }
            set { _ObjectDescriptorTable = value; }
        }

        private Int32 _Null3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null3
        {
            get { return _Null3; }
            set { _Null3 = value; }
        }

        private Int32 _NTSPrjDescription;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 NTSPrjDescription
        {
            get { return _NTSPrjDescription; }
            set { _NTSPrjDescription = value; }
        }

        private Int32 _NTSPrjHelpFile;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 NTSPrjHelpFile
        {
            get { return _NTSPrjHelpFile; }
            set { _NTSPrjHelpFile = value; }
        }

        private Int32 _Const2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const2
        {
            get { return _Const2; }
            set { _Const2 = value; }
        }

        private Int32 _HelpContextID;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 HelpContextID
        {
            get { return _HelpContextID; }
            set { _HelpContextID = value; }
        }
        #endregion
    }
}
