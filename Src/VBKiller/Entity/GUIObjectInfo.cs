using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x5D)]
    public class GUIObjectInfo : EntityBase<GUIObjectInfo>
    {
        #region 属性
        private Int32 _Unknown1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown1
        {
            get { return _Unknown1; }
            set { _Unknown1 = value; }
        }

        private Byte _Unknown2;
        /// <summary>属性说明</summary>
        [DataField]
        public Byte Unknown2
        {
            get { return _Unknown2; }
            set { _Unknown2 = value; }
        }

        private Byte[] _guidObjectGUI;
        /// <summary>属性说明</summary>
        [DataField(Size = 16)]
        public Byte[] guidObjectGUI
        {
            get { return _guidObjectGUI; }
            set { _guidObjectGUI = value; }
        }

        private Byte[] _uuidUnknown1;
        /// <summary>属性说明</summary>
        [DataField(Size = 16)]
        public Byte[] uuidUnknown1
        {
            get { return _uuidUnknown1; }
            set { _uuidUnknown1 = value; }
        }

        private Byte[] _guidCOMEventsIID;
        /// <summary>属性说明</summary>
        [DataField(Size = 16)]
        public Byte[] guidCOMEventsIID
        {
            get { return _guidCOMEventsIID; }
            set { _guidCOMEventsIID = value; }
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

        private Int32 _Unknown5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown5
        {
            get { return _Unknown5; }
            set { _Unknown5 = value; }
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

        private Int32 _Unknown8;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown8
        {
            get { return _Unknown8; }
            set { _Unknown8 = value; }
        }

        private Int32 _Unknown9;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown9
        {
            get { return _Unknown9; }
            set { _Unknown9 = value; }
        }

        private Int32 _Unknown10;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown10
        {
            get { return _Unknown10; }
            set { _Unknown10 = value; }
        }

        private Int32 _Unknown11;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Unknown11
        {
            get { return _Unknown11; }
            set { _Unknown11 = value; }
        }

        private Int32 _PropertiesLength;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 PropertiesLength
        {
            get { return _PropertiesLength; }
            set { _PropertiesLength = value; }
        }
        #endregion
    }
}
