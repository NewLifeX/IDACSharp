using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x28)]
    public class VBControl : EntityBase<VBControl>
    {
        #region 属性
        private Int16 _ControlType;
        /// <summary>Type of control.</summary>
        [DataField]
        public Int16 ControlType
        {
            get { return _ControlType; }
            set { _ControlType = value; }
        }

        private Int16 _EventCount;
        /// <summary>Number of Event Handlers supported</summary>
        [DataField]
        public Int16 EventCount
        {
            get { return _EventCount; }
            set { _EventCount = value; }
        }

        private Int32 _EventsOffset;
        /// <summary>Offset in to Memory struct to copy Events</summary>
        [DataField(typeof(Int32), RefKinds.Relative)]
        public Int32 EventsOffset
        {
            get { return _EventsOffset; }
            set { _EventsOffset = value; }
        }

        private Int32 _GUID;
        /// <summary>属性说明</summary>
        [DataField(typeof(Int32), RefKinds.Relative)]
        public Int32 GUID
        {
            get { return _GUID; }
            set { _GUID = value; }
        }

        private Int32 _Index;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        private Int32 _Null1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _Null2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null2
        {
            get { return _Null2; }
            set { _Null2 = value; }
        }

        private Int32 _EventTable;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(EventTable))]
        public Int32 EventTable
        {
            get { return _EventTable; }
            set { _EventTable = value; }
        }

        //private Byte _Flag3;
        ///// <summary>属性说明</summary>
        //[DataField]
        //public Byte Flag3
        //{
        //    get { return _Flag3; }
        //    set { _Flag3 = value; }
        //}
        //private Byte _Const2;
        ///// <summary>属性说明</summary>
        //[DataField]
        //public Byte Const2
        //{
        //    get { return _Const2; }
        //    set { _Const2 = value; }
        //}

        private Int32 _Const3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const3
        {
            get { return _Const3; }
            set { _Const3 = value; }
        }

        private Int32 _Name;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        //private Int32 _Index2;
        ///// <summary>属性说明</summary>
        //[DataField]
        //public Int32 Index2
        //{
        //    get { return _Index2; }
        //    set { _Index2 = value; }
        //}

        private Int32 _Const1Copy;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const1Copy
        {
            get { return _Const1Copy; }
            set { _Const1Copy = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>事件列表</summary>
        public EventTable EventTable2
        {
            get { return this["EventTable"] as EventTable; }
        }

        /// <summary>控件名字</summary>
        public String Name2
        {
            get { return (String)this["Name"]; }
        }
        #endregion

        #region 方法
        public override void ShowExtend(bool isShowExtend)
        {
            //base.ShowExtend(isShowExtend);
            // 不显示EventTable
        }
        #endregion
    }
}
