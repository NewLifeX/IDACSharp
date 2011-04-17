using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 设计器信息。不定长
    /// </summary>
    [DataObject(Size = 0x0)]
    public class DesignerInfo : EntityBase<DesignerInfo>
    {
        #region 属性
        private Byte[] _uuidDesigner;
        /// <summary>CLSID of the Addin/Designer</summary>
        [DataField(Size = 16)]
        public Byte[] uuidDesigner
        {
            get { return _uuidDesigner; }
            set { _uuidDesigner = value; }
        }

        private Int32 _StructSize;
        /// <summary>Total Size of the next fields</summary>
        [DataField]
        public Int32 StructSize
        {
            get { return _StructSize; }
            set { _StructSize = value; }
        }

        private Int16 _SizeAddinRegKey;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeAddinRegKey
        {
            get { return _SizeAddinRegKey; }
            set { _SizeAddinRegKey = value; }
        }

        private String _AddinRegKey;
        /// <summary>属性说明</summary>
        [DataField(SizeField = "SizeAddinRegKey")]
        public String AddinRegKey
        {
            get { return _AddinRegKey; }
            set { _AddinRegKey = value; }
        }

        private Int16 _SizeAddinName;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeAddinName
        {
            get { return _SizeAddinName; }
            set { _SizeAddinName = value; }
        }

        private String _AddinName;
        /// <summary>属性说明</summary>
        [DataField(SizeField = "SizeAddinName")]
        public String AddinName
        {
            get { return _AddinName; }
            set { _AddinName = value; }
        }

        private Int16 _SizeAddinDescription;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeAddinDescription
        {
            get { return _SizeAddinDescription; }
            set { _SizeAddinDescription = value; }
        }

        private String _AddinDescription;
        /// <summary>属性说明</summary>
        [DataField(SizeField = "SizeAddinDescription")]
        public String AddinDescription
        {
            get { return _AddinDescription; }
            set { _AddinDescription = value; }
        }

        private Int32 _LoadBehaviour;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 LoadBehaviour
        {
            get { return _LoadBehaviour; }
            set { _LoadBehaviour = value; }
        }

        private Int16 _SizeSatelliteDLL;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeSatelliteDLL
        {
            get { return _SizeSatelliteDLL; }
            set { _SizeSatelliteDLL = value; }
        }

        private String _SatelliteDLL;
        /// <summary>属性说明</summary>
        [DataField(SizeField = "SizeSatelliteDLL")]
        public String SatelliteDLL
        {
            get { return _SatelliteDLL; }
            set { _SatelliteDLL = value; }
        }

        private Int16 _SizeAdditionalRegKey;
        /// <summary>属性说明</summary>
        [DataField]
        public Int16 SizeAdditionalRegKey
        {
            get { return _SizeAdditionalRegKey; }
            set { _SizeAdditionalRegKey = value; }
        }

        private String _AdditionalRegKey;
        /// <summary>属性说明</summary>
        [DataField(SizeField = "SizeAdditionalRegKey")]
        public String AdditionalRegKey
        {
            get { return _AdditionalRegKey; }
            set { _AdditionalRegKey = value; }
        }

        private Int32 _CommandLineSafe;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 CommandLineSafe
        {
            get { return _CommandLineSafe; }
            set { _CommandLineSafe = value; }
        }
        #endregion
    }
}
