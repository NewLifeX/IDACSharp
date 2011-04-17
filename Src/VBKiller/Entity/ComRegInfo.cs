using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VBKiller.Entity
{
    /// <summary>
    /// 从VBHEADER->ComRegData可以获得COM注册数据的地址指针
    /// </summary>
    /// <remarks>这是COMRegData以及COMRegInfo结构的C语言描述</remarks>
    [DataObject(Size = 0x44)]
    public class ComRegInfo : EntityBase<ComRegInfo>
    {
        #region 属性
        private Int32 _NextObject;
        /// <summary>Offset to COM Interfaces Info</summary>
        /// <remarks>这里本来是下一个ComRegInfo的偏移（相对于ComRegData），但为了避免重读，改为普通指针</remarks>
        [DataField(RefType = typeof(Int32))]
        public Int32 NextObject
        {
            get { return _NextObject; }
            set { _NextObject = value; }
        }

        private Int32 _ObjectName;
        /// <summary>Offset to Object Name</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 ObjectName
        {
            get { return _ObjectName; }
            set { _ObjectName = value; }
        }

        private Int32 _ObjectDescription;
        /// <summary>Offset to Object Description</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 ObjectDescription
        {
            get { return _ObjectDescription; }
            set { _ObjectDescription = value; }
        }

        private Int32 _Instancing;
        /// <summary>Instancing Mode</summary>
        [DataField]
        public Int32 Instancing
        {
            get { return _Instancing; }
            set { _Instancing = value; }
        }

        private Int32 _ObjectID;
        /// <summary>Current Object ID in the Project</summary>
        [DataField]
        public Int32 ObjectID
        {
            get { return _ObjectID; }
            set { _ObjectID = value; }
        }

        private Byte[] _uuidObjectClsID;
        /// <summary>CLSID of Object</summary>
        [DataField(Size = 16)]
        public Byte[] uuidObjectClsID
        {
            get { return _uuidObjectClsID; }
            set { _uuidObjectClsID = value; }
        }

        private Int32 _IsInterface;
        /// <summary>Specifies if the next CLSID is valid</summary>
        [DataField]
        public Int32 IsInterface
        {
            get { return _IsInterface; }
            set { _IsInterface = value; }
        }

        private Int32 _ObjectClsID;
        /// <summary>Offset to CLSID of Object Interface</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 ObjectClsID
        {
            get { return _ObjectClsID; }
            set { _ObjectClsID = value; }
        }

        private Int32 _ControlClsID;
        /// <summary>Offset to CLSID of Control Interface</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 ControlClsID
        {
            get { return _ControlClsID; }
            set { _ControlClsID = value; }
        }

        private Int32 _IsControl;
        /// <summary>Specifies if the CLSID above is valid</summary>
        [DataField]
        public Int32 IsControl
        {
            get { return _IsControl; }
            set { _IsControl = value; }
        }

        private Int32 _MiscStatus;
        /// <summary>OLEMISC Flags (see MSDN docs)</summary>
        [DataField]
        public Int32 MiscStatus
        {
            get { return _MiscStatus; }
            set { _MiscStatus = value; }
        }

        private Byte _ClassType;
        /// <summary>Class Type</summary>
        [DataField]
        public Byte ClassType
        {
            get { return _ClassType; }
            set { _ClassType = value; }
        }

        private Byte _ObjectType;
        /// <summary>Flag identifying the Object Type</summary>
        [DataField]
        public Byte ObjectType
        {
            get { return _ObjectType; }
            set { _ObjectType = value; }
        }

        private Int16 _ToolboxBitmap32;
        /// <summary>Control Bitmap ID in Toolbox</summary>
        [DataField]
        public Int16 ToolboxBitmap32
        {
            get { return _ToolboxBitmap32; }
            set { _ToolboxBitmap32 = value; }
        }

        private Int16 _DefaultIcon;
        /// <summary>Minimized Icon of Control Window</summary>
        [DataField]
        public Int16 DefaultIcon
        {
            get { return _DefaultIcon; }
            set { _DefaultIcon = value; }
        }

        private Int16 _IsDesigner;
        /// <summary>Specifies whether this is a Designer</summary>
        [DataField]
        public Int16 IsDesigner
        {
            get { return _IsDesigner; }
            set { _IsDesigner = value; }
        }

        private Int32 _DesignerData;
        /// <summary>Offset to Designer Data</summary>
        [DataField(RefType = typeof(DesignerInfo))]
        public Int32 DesignerData
        {
            get { return _DesignerData; }
            set { _DesignerData = value; }
        }
        #endregion

        #region 扩展属性
        ///// <summary>下一个对象</summary>
        //public ComRegInfo Next
        //{
        //    get { return this["NextObject"] as ComRegInfo; }
        //}

        /// <summary>Designer Data</summary>
        public DesignerInfo DesignerData2
        {
            get { return this["DesignerData"] as DesignerInfo; }
        }

        private ComRegData _RegData;
        /// <summary>COM数据</summary>
        public ComRegData RegData
        {
            get { return _RegData; }
            set { _RegData = value; }
        }

        /// <summary>名称</summary>
        public String Name
        {
            get { return (String)this["ObjectName"]; }
        }
        #endregion

        #region 方法
        //public override void Show()
        //{
        //    base.Show();

        //    DesignerInfo entity = this.DesignerData2;
        //    entity.ReadExtend();
        //    WriteLine(String.Empty);
        //    entity.Show();
        //}

        public override void ReadExtendProperty(BinaryReader reader, DataFieldItem dataItem)
        {
            if (dataItem.Property.Name == "ObjectName")
            {
                // ObjectName的偏移量也是相对于ComRegData的Address，而不是相对于自己的Address
                long address = RegData.Address + ObjectName;
                Seek(reader, address);
                //entity.Extends["ObjectName"] = ReadString(reader);
                Extends.Add("ObjectName", ReadString(reader));

                return;
            }

            base.ReadExtendProperty(reader, dataItem);
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
