using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 从VBHEADER->GUITable可以获得GUI元素表的地址指针,也就是指向WINDOWS图形元素的表
    /// </summary>
    /// <remarks>这个表没有什么重要，主要就是那个pFormPointer比较有用。</remarks>
    [DataObject(Size = 0x50)]
    public class GUITable : EntityBase<GUITable>
    {
        #region 属性
        private Int32 _StructSize;
        /// <summary>这个结构的总大小</summary>
        [DataField]
        public Int32 StructSize
        {
            get { return _StructSize; }
            set { _StructSize = value; }
        }

        private Byte[] _uuidObjectGUI;
        /// <summary>Object GUI的UUID</summary>
        [DataField(Size = 16)]
        public Byte[] uuidObjectGUI
        {
            get { return _uuidObjectGUI; }
            set { _uuidObjectGUI = value; }
        }

        private Int32 _Unknown1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown1
        {
            get { return _Unknown1; }
            set { _Unknown1 = value; }
        }

        private Int32 _Unknown2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown2
        {
            get { return _Unknown2; }
            set { _Unknown2 = value; }
        }

        private Int32 _Unknown3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown3
        {
            get { return _Unknown3; }
            set { _Unknown3 = value; }
        }

        private Int32 _Unknown4;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown4
        {
            get { return _Unknown4; }
            set { _Unknown4 = value; }
        }

        private Int32 _ObjectID;
        /// <summary>当前工程的组件ID</summary>
        [DataField]
        public Int32 ObjectID
        {
            get { return _ObjectID; }
            set { _ObjectID = value; }
        }

        private Int32 _Unknown5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown5
        {
            get { return _Unknown5; }
            set { _Unknown5 = value; }
        }

        private Int32 _OLEMisc;
        /// <summary>OLEMisc标志</summary>
        [DataField]
        public Int32 OLEMisc
        {
            get { return _OLEMisc; }
            set { _OLEMisc = value; }
        }

        private Byte[] _uuidObject;
        /// <summary>组件的UUID</summary>
        [DataField(Size = 16)]
        public Byte[] uuidObject
        {
            get { return _uuidObject; }
            set { _uuidObject = value; }
        }

        private Int32 _Unknown6;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown6
        {
            get { return _Unknown6; }
            set { _Unknown6 = value; }
        }

        private Int32 _Unknown7;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown7
        {
            get { return _Unknown7; }
            set { _Unknown7 = value; }
        }

        private Int32 _FormPointer;
        /// <summary> VA 指向GUI Object Info结构的地址指针</summary>
        [DataField(RefType = typeof(ObjectInfo), IsParent = true)]
        public Int32 FormPointer
        {
            get { return _FormPointer; }
            set { _FormPointer = value; }
        }

        private Int32 _Unknown8;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown8
        {
            get { return _Unknown8; }
            set { _Unknown8 = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>对象信息</summary>
        public ObjectInfo FormPointer2
        {
            get { return this["FormPointer"] as ObjectInfo; }
        }
        #endregion
    }
}
