using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace VBKiller.Entity
{
    /// <summary>
    /// 从VBHEADER->ComRegData可以获得COM注册数据的地址指针
    /// </summary>
    /// <remarks>这是COMRegData以及COMRegInfo结构的C语言描述</remarks>
    [DataObject(Size = 0x30)]
    public class ComRegData : EntityBase<ComRegData>
    {
        #region 属性
        private Int32 _RegInfo;
        /// <summary>Offset 指向COM Interfaces Info结构（COM接口信息）</summary>
        [DataField(typeof(ComRegInfo), RefKinds.Relative)]
        public Int32 RegInfo
        {
            get { return _RegInfo; }
            set { _RegInfo = value; }
        }

        private Int32 _NTSProjectName;
        /// <summary>Offset 指向Project/Typelib Name（工程名）</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 NTSProjectName
        {
            get { return _NTSProjectName; }
            set { _NTSProjectName = value; }
        }

        private Int32 _NTSHelpDirectory;
        /// <summary>Offset 指向Help Directory（帮助文件目录）</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 NTSHelpDirectory
        {
            get { return _NTSHelpDirectory; }
            set { _NTSHelpDirectory = value; }
        }

        private Int32 _NTSProjectDescription;
        /// <summary>Offset 指向Project Description（工程描述）</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 NTSProjectDescription
        {
            get { return _NTSProjectDescription; }
            set { _NTSProjectDescription = value; }
        }

        private Byte[] _uuidProjectClsId;
        /// <summary>Project/Typelib的CLSID</summary>
        [DataField(Size = 16)]
        public Byte[] uuidProjectClsId
        {
            get { return _uuidProjectClsId; }
            set { _uuidProjectClsId = value; }
        }

        private Int32 _TlbLcid;
        /// <summary>Type Library的LCID</summary>
        [DataField]
        public Int32 TlbLcid
        {
            get { return _TlbLcid; }
            set { _TlbLcid = value; }
        }

        private Int16 _Padding1;
        /// <summary>没有用的内存对齐空间1</summary>
        [DataField]
        public Int16 Padding1
        {
            get { return _Padding1; }
            set { _Padding1 = value; }
        }

        private Int16 _TlbVerMajor;
        /// <summary>Typelib 主版本</summary>
        [DataField]
        public Int16 TlbVerMajor
        {
            get { return _TlbVerMajor; }
            set { _TlbVerMajor = value; }
        }

        private Int16 _TlbVerMinor;
        /// <summary>Typelib 次版本</summary>
        [DataField]
        public Int16 TlbVerMinor
        {
            get { return _TlbVerMinor; }
            set { _TlbVerMinor = value; }
        }

        private Int16 _Padding2;
        /// <summary>没有用的内存对齐空间</summary>
        [DataField]
        public Int16 Padding2
        {
            get { return _Padding2; }
            set { _Padding2 = value; }
        }

        private Int32 _Padding3;
        /// <summary>没有用的内存对齐空间</summary>
        [DataField]
        public Int32 Padding3
        {
            get { return _Padding3; }
            set { _Padding3 = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>COM接口信息</summary>
        public ComRegInfo[] RegInfo2
        {
            get { return this["RegInfo"] as ComRegInfo[]; }
        }

        /// <summary>名称</summary>
        public String Name
        {
            get { return (String)this["NTSProjectName"]; }
        }
        #endregion

        #region 方法
        public override void ReadExtendProperty(BinaryReader reader, DataFieldItem dataItem)
        {
            PropertyInfo property = dataItem.Property;
            DataFieldAttribute att = dataItem.Attribute;

            if (property.Name == "RegInfo")
            {
                long offset = 0;
                if (property.PropertyType == typeof(Int32))
                    offset = (Int32)property.GetValue(this, null);
                else
                    offset = (Int16)property.GetValue(this, null);
                if (offset <= 0) return;

                long address = GetRealAddress(offset, dataItem.Attribute.RefKind);
                // 超出文件结尾时，终止读取
                if (address <= 0 || address > reader.BaseStream.Length)
                {
                    Extends.Add(property.Name, null);
                    return;
                }

                // Info.Objects只存单个对象，不存集合，所以这里不用判断

                List<ComRegInfo> list = new List<ComRegInfo>();
                for (int i = 0; i < offset; i++)
                {
                    ComRegInfo entity = new ComRegInfo();

                    // 读取对象
                    address = Address + offset;

                    Seek(reader, address);
                    entity.Info = Info;
                    entity.RegData = this;
                    entity.Read(reader);

                    //// ObjectName的偏移量也是相对于ComRegData的
                    //address = Address + entity.ObjectName;
                    //Seek(reader, address);
                    ////entity.Extends["ObjectName"] = ReadString(reader);
                    //entity.Extends.Add("ObjectName", ReadString(reader));

                    list.Add(entity);

                    offset = entity.NextObject;
                }

                //Extends[property.Name] = list.ToArray();
                Extends.Add(property.Name, list.ToArray());
                return;
            }

            base.ReadExtendProperty(reader, dataItem);
        }

        ///// <summary>
        ///// 已重载。改变显示逻辑，不显示COM信息
        ///// </summary>
        ///// <param name="isShowExtend"></param>
        //public override void Show(bool isShowExtend)
        //{
        //    base.Show(false);

        //    //if (RegInfo2 == null || RegInfo2.Length <= 0) return;

        //    //foreach (ComRegInfo item in RegInfo2)
        //    //{
        //    //    WriteLine(item.Name);
        //    //}
        //}

        /// <summary>
        /// 已重载。改变显示逻辑，不显示COM信息
        /// </summary>
        /// <param name="isShowExtend"></param>
        public override void ShowExtend(bool isShowExtend)
        {
            //base.ShowExtend(isShowExtend);

            if (RegInfo2 != null && RegInfo2.Length > 0)
            {
                foreach (ComRegInfo item in RegInfo2)
                {
                    WriteLine("{0}.{1}", Name, item.Name);
                }
            }
        }
        #endregion
    }
}
