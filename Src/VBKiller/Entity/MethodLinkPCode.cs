using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0xD)]
    public class MethodLinkPCode : EntityBase<MethodLinkPCode>
    {
        #region 属性
        private Int16 _XorOpCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 XorOpCode
        {
            get { return _XorOpCode; }
            set { _XorOpCode = value; }
        }

        private Byte _MovOpCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte MovOpCode
        {
            get { return _MovOpCode; }
            set { _MovOpCode = value; }
        }

        private Int32 _MovAddress;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 MovAddress
        {
            get { return _MovAddress; }
            set { _MovAddress = value; }
        }

        private Byte _PushOpCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte PushOpCode
        {
            get { return _PushOpCode; }
            set { _PushOpCode = value; }
        }

        private Int32 _PushAddress;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 PushAddress
        {
            get { return _PushAddress; }
            set { _PushAddress = value; }
        }

        private Byte _RetOpCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte RetOpCode
        {
            get { return _RetOpCode; }
            set { _RetOpCode = value; }
        }
        #endregion
    }
}
