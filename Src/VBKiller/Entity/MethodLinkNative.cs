using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x5)]
    public class MethodLinkNative : EntityBase<MethodLinkNative>
    {
        private Byte _JmpOpCode;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte JmpOpCode
        {
            get { return _JmpOpCode; }
            set { _JmpOpCode = value; }
        }

        private Int32 _JmpOffset;
        /// <summary>属性说明</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 JmpOffset
        {
            get { return _JmpOffset; }
            set { _JmpOffset = value; }
        }
    }
}
