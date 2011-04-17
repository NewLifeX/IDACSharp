using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 这个是可选的OBJECT_INFO和PEHEADER里的OPTIONAL_HEADER类似，是否有要看每个Object_t里面的ObjectTyper表里的倒数第二个位（详细看上表）
    /// </summary>
    [DataObject(Size = 0x40)]
    public class OptionalObjectInfo : EntityBase<OptionalObjectInfo>
    {
        #region 属性
        private Int32 _ObjectGuids;
        /// <summary>How many GUIDs to Register. 2 = Designer</summary>
        [DataField]
        public Int32 ObjectGuids
        {
            get { return _ObjectGuids; }
            set { _ObjectGuids = value; }
        }

        private Int32 _ObjectCLSID;
        /// <summary>指向CLSID对象</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ObjectCLSID
        {
            get { return _ObjectCLSID; }
            set { _ObjectCLSID = value; }
        }

        private Int32 _Null1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _ObjectTypes;
        /// <summary>Pointer to Array of Object Interface GUIDs</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ObjectTypes
        {
            get { return _ObjectTypes; }
            set { _ObjectTypes = value; }
        }

        private Int32 _ObjectTypeGuids;
        /// <summary>How many GUIDs in the Array above</summary>
        [DataField]
        public Int32 ObjectTypeGuids
        {
            get { return _ObjectTypeGuids; }
            set { _ObjectTypeGuids = value; }
        }

        private Int32 _Controls2;
        /// <summary>指向对象行为IID表 Usually the same as lpControls.</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 Controls2
        {
            get { return _Controls2; }
            set { _Controls2 = value; }
        }

        private Int32 _Null2;
        /// <summary>对象行为IID个数</summary>
        [DataField]
        public Int32 Null2
        {
            get { return _Null2; }
            set { _Null2 = value; }
        }

        private Int32 _ObjectDefaultIIDTable;
        /// <summary>指向默认对象IID表</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ObjectDefaultIIDTable
        {
            get { return _ObjectDefaultIIDTable; }
            set { _ObjectDefaultIIDTable = value; }
        }

        private Int32 _ControlCount;
        /// <summary>控件个数</summary>
        [DataField]
        public Int32 ControlCount
        {
            get { return _ControlCount; }
            set { _ControlCount = value; }
        }

        private Int32 _ControlArray;
        /// <summary>指向控件表</summary>
        [DataField(RefType = typeof(VBControl), RefKind = RefKinds.Virtual, SizeField = "ControlCount")]
        public Int32 ControlArray
        {
            get { return _ControlArray; }
            set { _ControlArray = value; }
        }

        private Int16 _EventCount;
        /// <summary>行为的个数，比较重要，知道有几个行为</summary>
        [DataField]
        public Int16 EventCount
        {
            get { return _EventCount; }
            set { _EventCount = value; }
        }

        private Int16 _PCodeCount;
        /// <summary>PCode个数</summary>
        [DataField]
        public Int16 PCodeCount
        {
            get { return _PCodeCount; }
            set { _PCodeCount = value; }
        }

        private Int16 _InitializeEvent;
        /// <summary>offset从aMethodLinkTable指向初始化行为</summary>
        [DataField(RefType = typeof(Int32))]
        public Int16 InitializeEvent
        {
            get { return _InitializeEvent; }
            set { _InitializeEvent = value; }
        }

        private Int16 _TerminateEvent;
        /// <summary>offset从aMethodLinkTable指向终止行为</summary>
        [DataField(RefType = typeof(Int32))]
        public Int16 TerminateEvent
        {
            get { return _TerminateEvent; }
            set { _TerminateEvent = value; }
        }

        private Int32 _EventLinkArray;
        /// <summary>Pointer to pointers of MethodLink</summary>
        [DataField(RefType = typeof(EventLink2), RefKind = RefKinds.Virtual, SizeField = "EventCount")]
        public Int32 EventLinkArray
        {
            get { return _EventLinkArray; }
            set { _EventLinkArray = value; }
        }

        private Int32 _BasicClassObject;
        /// <summary>Pointer to an in-memory</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 BasicClassObject
        {
            get { return _BasicClassObject; }
            set { _BasicClassObject = value; }
        }

        private Int32 _Null3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null3
        {
            get { return _Null3; }
            set { _Null3 = value; }
        }

        private Int32 _Flag2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Flag2
        {
            get { return _Flag2; }
            set { _Flag2 = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>控件列表</summary>
        public VBControl[] Controls
        {
            get { return GetExtendList<VBControl>("ControlArray"); }
        }

        /// <summary>事件列表</summary>
        public EventLink2[] EventLinks
        {
            get { return GetExtendList<EventLink2>("EventLinkArray"); }
        }
        #endregion

        #region 方法
        protected override void ShowExtendProperty(String name, bool isShowExtend)
        {
            if (name == "EventLinkArray")
            {
                if (EventLinks != null && EventLinks.Length > 0)
                {
                    WriteLine();
                    WriteLine("EventLinks:");
                    foreach (EventLink2 item in EventLinks)
                    {
                        WriteLine("0x{0:X}", item.Jump);
                    }
                }

                return;
            }

            base.ShowExtendProperty(name, isShowExtend);
        }
        #endregion
    }
}
