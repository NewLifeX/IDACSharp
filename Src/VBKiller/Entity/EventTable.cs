using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x18)]
    public class EventTable : EntityBase<EventTable>
    {
        #region 属性
        private Int32 _Null1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _Control;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(VBControl), IsParent = true)]
        public Int32 Control
        {
            get { return _Control; }
            set { _Control = value; }
        }

        private Int32 _ObjectInfo;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(ObjectInfo), IsParent = true)]
        public Int32 ObjectInfo
        {
            get { return _ObjectInfo; }
            set { _ObjectInfo = value; }
        }

        private Int32 _QueryInterface;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 QueryInterface
        {
            get { return _QueryInterface; }
            set { _QueryInterface = value; }
        }

        private Int32 _AddRef;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 AddRef
        {
            get { return _AddRef; }
            set { _AddRef = value; }
        }

        private Int32 _Release;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 Release
        {
            get { return _Release; }
            set { _Release = value; }
        }
        #endregion
    }
}
