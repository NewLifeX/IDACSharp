using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    public class EventPointer : EntityBase<EventPointer>
    {
        #region 属性
        private Byte _Const1;
        /// <summary>属性说明</summary>
        public Byte Const1
        {
            get { return _Const1; }
            set { _Const1 = value; }
        }

        private Int32 _Flag1;
        /// <summary>属性说明</summary>
        public Int32 Flag1
        {
            get { return _Flag1; }
            set { _Flag1 = value; }
        }

        private Int32 _Const2;
        /// <summary>属性说明</summary>
        public Int32 Const2
        {
            get { return _Const2; }
            set { _Const2 = value; }
        }

        private Byte _Const3;
        /// <summary>属性说明</summary>
        public Byte Const3
        {
            get { return _Const3; }
            set { _Const3 = value; }
        }

        private Int32 _Event;
        /// <summary>属性说明</summary>
        public Int32 Event
        {
            get { return _Event; }
            set { _Event = value; }
        }
        #endregion
    }
}
