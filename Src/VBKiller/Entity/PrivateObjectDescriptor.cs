using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x40)]
    public class PrivateObjectDescriptor : EntityBase<PrivateObjectDescriptor>
    {
        #region 属性
        private Int32 _HeapLink;
        /// <summary>Unused after compilation, always 0.</summary>
        [DataField]
        public Int32 HeapLink
        {
            get { return _HeapLink; }
            set { _HeapLink = value; }
        }

        private Int32 _ObjectInfo;
        /// <summary>Pointer to the Object Info for this Object</summary>
        [DataField(RefType = typeof(ObjectInfo), RefKind = RefKinds.Virtual, IsParent = true)]
        public Int32 ObjectInfo
        {
            get { return _ObjectInfo; }
            set { _ObjectInfo = value; }
        }

        private Int32 _Reserved;
        /// <summary>Always set to -1 after compiling</summary>
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

        private Int32 _Flag1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Flag1
        {
            get { return _Flag1; }
            set { _Flag1 = value; }
        }

        private Int32 _Null3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null3
        {
            get { return _Null3; }
            set { _Null3 = value; }
        }

        private Int32 _ObjectList;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ObjectList
        {
            get { return _ObjectList; }
            set { _ObjectList = value; }
        }

        private Int32 _Null4;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null4
        {
            get { return _Null4; }
            set { _Null4 = value; }
        }

        private Int32 _Unknown2;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 Unknown2
        {
            get { return _Unknown2; }
            set { _Unknown2 = value; }
        }

        private Int32 _Unknown3;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 Unknown3
        {
            get { return _Unknown3; }
            set { _Unknown3 = value; }
        }

        private Int32 _Unknown4;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 Unknown4
        {
            get { return _Unknown4; }
            set { _Unknown4 = value; }
        }

        private Int32 _Null5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null5
        {
            get { return _Null5; }
            set { _Null5 = value; }
        }

        private Int32 _Null6;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null6
        {
            get { return _Null6; }
            set { _Null6 = value; }
        }

        private Int32 _Null7;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null7
        {
            get { return _Null7; }
            set { _Null7 = value; }
        }

        private Int32 _Flag2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Flag2
        {
            get { return _Flag2; }
            set { _Flag2 = value; }
        }

        private Int32 _ObjectType;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 ObjectType
        {
            get { return _ObjectType; }
            set { _ObjectType = value; }
        }
        #endregion
    }
}
