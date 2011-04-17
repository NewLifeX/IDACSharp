using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 这个是显示这个OBJECT信息的结构,每一个OBJECT都有一个
    /// </summary>
    [DataObject(Size = 0x38)]
    public class ObjectInfo : EntityBase<ObjectInfo>
    {
        #region 属性
        private Int16 _RefCount;
        /// <summary>Always 1 after compilation</summary>
        [DataField]
        public Int16 RefCount
        {
            get { return _RefCount; }
            set { _RefCount = value; }
        }

        private Int16 _ObjectIndex;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 ObjectIndex
        {
            get { return _ObjectIndex; }
            set { _ObjectIndex = value; }
        }

        private Int32 _ObjectTable;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(ObjectTable), RefKind = RefKinds.Virtual, IsParent = true)]
        public Int32 ObjectTable
        {
            get { return _ObjectTable; }
            set { _ObjectTable = value; }
        }

        private Int32 _Null1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _PrivateObject;
        /// <summary>Pointer to Private Object Descriptor</summary>
        [DataField(typeof(PrivateObjectDescriptor), RefKinds.Virtual)]
        public Int32 PrivateObject
        {
            get { return _PrivateObject; }
            set { _PrivateObject = value; }
        }

        private Int32 _Const1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const1
        {
            get { return _Const1; }
            set { _Const1 = value; }
        }

        private Int32 _Null2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null2
        {
            get { return _Null2; }
            set { _Null2 = value; }
        }

        private Int32 _Object;
        /// <summary>Back-Pointer to Public Object Descriptor</summary>
        [DataField(RefType = typeof(PublicObjectDescriptor), RefKind = RefKinds.Virtual, IsParent = true)]
        public Int32 Object
        {
            get { return _Object; }
            set { _Object = value; }
        }

        private Int32 _ProjectData;
        /// <summary>Pointer to in-memory Project Object</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ProjectData
        {
            get { return _ProjectData; }
            set { _ProjectData = value; }
        }

        private Int32 _NumberOfProcs;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 NumberOfProcs
        {
            get { return _NumberOfProcs; }
            set { _NumberOfProcs = value; }
        }

        private Int32 _ProcTable;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ProcTable
        {
            get { return _ProcTable; }
            set { _ProcTable = value; }
        }

        private Int16 _ConstantsCount;
        /// <summary>Number of Constants</summary>
        [DataField]
        public Int16 ConstantsCount
        {
            get { return _ConstantsCount; }
            set { _ConstantsCount = value; }
        }

        private Int16 _MaxConstants;
        /// <summary>Maximum Constants to allocate</summary>
        [DataField]
        public Int16 MaxConstants
        {
            get { return _MaxConstants; }
            set { _MaxConstants = value; }
        }

        private Int32 _Flag5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Flag5
        {
            get { return _Flag5; }
            set { _Flag5 = value; }
        }

        private Int16 _Flag6;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Flag6
        {
            get { return _Flag6; }
            set { _Flag6 = value; }
        }

        private Int16 _Flag7;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 Flag7
        {
            get { return _Flag7; }
            set { _Flag7 = value; }
        }

        private Int32 _ConstantPool;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ConstantPool
        {
            get { return _ConstantPool; }
            set { _ConstantPool = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>私有对象信息</summary>
        public PrivateObjectDescriptor PrivateObject2
        {
            get { return this["PrivateObject"] as PrivateObjectDescriptor; }
        }
        #endregion

        #region 方法
        //public override void ReadExtendProperty(BinaryReader reader, DataFieldItem dataItem)
        //{
        //    if (dataItem.Property.Name == "ObjectName")
        //    {
        //        // ObjectName的偏移量也是相对于ComRegData的Address，而不是相对于自己的Address
        //        long address = RegData.Address + ObjectName;
        //        Seek(reader, address);
        //        //entity.Extends["ObjectName"] = ReadString(reader);
        //        Extends.Add("ObjectName", ReadString(reader));

        //        return;
        //    }

        //    base.ReadExtendProperty(reader, dataItem);
        //}

        //public override string ToString()
        //{
        //    return Name;
        //}
        #endregion
    }
}
