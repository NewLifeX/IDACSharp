using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    public class EventLink : EntityBase<EventLink>
    {
        #region 属性
        private Int16 _Const1;
        /// <summary>属性说明</summary>
        public Int16 Const1
        {
            get { return _Const1; }
            set { _Const1 = value; }
        }

        private Byte _CompileType;
        /// <summary>属性说明</summary>
        public Byte CompileType
        {
            get { return _CompileType; }
            set { _CompileType = value; }
        }

        private Int32 _Event;
        /// <summary>属性说明</summary>
        public Int32 Event
        {
            get { return _Event; }
            set { _Event = value; }
        }

        private Byte _PushCmd;
        /// <summary>属性说明</summary>
        public Byte PushCmd
        {
            get { return _PushCmd; }
            set { _PushCmd = value; }
        }

        private Int32 _PushAddress;
        /// <summary>属性说明</summary>
        public Int32 PushAddress
        {
            get { return _PushAddress; }
            set { _PushAddress = value; }
        }

        private Byte _Const2;
        /// <summary>属性说明</summary>
        public Byte Const2
        {
            get { return _Const2; }
            set { _Const2 = value; }
        }
        #endregion
    }
}
