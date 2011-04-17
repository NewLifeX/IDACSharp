using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using IDACSharp;
using System.IO;

namespace VBKiller.Entity
{
    /// <summary>
    /// VB头结构
    /// </summary>
    [DataObject(Size = 0x68)]
    public class VBHeader : EntityBase<VBHeader>
    {
        #region 属性
        private String _Signature;
        /// <summary>四个字节的签名符号，和PEHEADER里的那个signature是类似性质的东西，VB文件都是"VB5!"</summary>
        [DataField(Size = 4)]
        public String Signature
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        private Int16 _RuntimeBuild;
        /// <summary>运行时创立的变量（类似编译的时间）</summary>
        [DataField]
        public Int16 RuntimeBuild
        {
            get { return _RuntimeBuild; }
            set { _RuntimeBuild = value; }
        }

        private String _LanguageDLL;
        /// <summary>语言DLL文件的名字（如果是0x2A的话就代表是空或者是默认的）</summary>
        [DataField(Size = 14)]
        public String LanguageDLL
        {
            get { return _LanguageDLL; }
            set { _LanguageDLL = value; }
        }

        private String _BackupLanguageDLL;
        /// <summary>备份DLL语言文件的名字（如果是0x7F的话就代表是空或者是默认的，改变这个值堆EXE文件的运行没有作用）</summary>
        [DataField(Size = 14)]
        public String BackupLanguageDLL
        {
            get { return _BackupLanguageDLL; }
            set { _BackupLanguageDLL = value; }
        }

        private Int16 _RuntimeDLLVersion;
        /// <summary>运行是DLL文件的版本</summary>
        [DataField]
        public Int16 RuntimeDLLVersion
        {
            get { return _RuntimeDLLVersion; }
            set { _RuntimeDLLVersion = value; }
        }

        private Int32 _LanguageID;
        /// <summary>语言的ID</summary>
        [DataField]
        public Int32 LanguageID
        {
            get { return _LanguageID; }
            set { _LanguageID = value; }
        }

        private Int32 _BackupLanguageID;
        /// <summary>备份语言的ID（只有当语言ID存在时它才存在）</summary>
        [DataField]
        public Int32 BackupLanguageID
        {
            get { return _BackupLanguageID; }
            set { _BackupLanguageID = value; }
        }

        private Int32 _SubMain;
        /// <summary>RVA（实际研究下来是VA） sub main过程的地址指针（3.）（如果时00000000则代表这个EXE时从FORM窗体文件开始运行的）</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 SubMain
        {
            get { return _SubMain; }
            set { _SubMain = value; }
        }

        private Int32 _ProjectInfo;
        /// <summary>VA 工程信息的地址指针，指向一个ProjectInfo_t结构</summary>
        [DataField(typeof(ProjectInfo), RefKinds.Virtual)]
        public Int32 ProjectInfo
        {
            get { return _ProjectInfo; }
            set { _ProjectInfo = value; }
        }

        private Int32 _MDLIntObjs;
        /// <summary>详细见"MDL 内部组建的标志表"</summary>
        [DataField]
        public Int32 MDLIntObjs
        {
            get { return _MDLIntObjs; }
            set { _MDLIntObjs = value; }
        }

        private Int32 _MDLIntObjs2;
        /// <summary>详细见"MDL 内部组建的标志表"</summary>
        [DataField]
        public Int32 MDLIntObjs2
        {
            get { return _MDLIntObjs2; }
            set { _MDLIntObjs2 = value; }
        }

        //* MDL 内部组建的标志表
        //+---------+------------+---------------+
        //|   ID    |    值      | 组建名称      |
        //+---------+------------+---------------+
        //|                           第一个标志 |
        //+---------+------------+---------------+
        //|    0x00 | 0x00000001 | PictureBox    |
        //|    0x01 | 0x00000002 | Label         |
        //|    0x02 | 0x00000004 | TextBox       |
        //|    0x03 | 0x00000008 | Frame         |
        //|    0x04 | 0x00000010 | CommandButton |
        //|    0x05 | 0x00000020 | CheckBox      |
        //|    0x06 | 0x00000040 | OptionButton  |
        //|    0x07 | 0x00000080 | ComboBox      |
        //|    0x08 | 0x00000100 | ListBox       |
        //|    0x09 | 0x00000200 | HScrollBar    |
        //|    0x0A | 0x00000400 | VScrollBar    |
        //|    0x0B | 0x00000800 | Timer         |
        //|    0x0C | 0x00001000 | Print         |
        //|    0x0D | 0x00002000 | Form          |
        //|    0x0E | 0x00004000 | Screen        |
        //|    0x0F | 0x00008000 | Clipboard     |
        //|    0x10 | 0x00010000 | Drive         |
        //|    0x11 | 0x00020000 | Dir           |
        //|    0x12 | 0x00040000 | FileListBox   |
        //|    0x13 | 0x00080000 | Menu          |
        //|    0x14 | 0x00100000 | MDIForm       |
        //|    0x15 | 0x00200000 | App           |
        //|    0x16 | 0x00400000 | Shape         |
        //|    0x17 | 0x00800000 | Line          |
        //|    0x18 | 0x01000000 | Image         |
        //|    0x19 | 0x02000000 | Unsupported   |
        //|    0x1A | 0x04000000 | Unsupported   |
        //|    0x1B | 0x08000000 | Unsupported   |
        //|    0x1C | 0x10000000 | Unsupported   |
        //|    0x1D | 0x20000000 | Unsupported   |
        //|    0x1E | 0x40000000 | Unsupported   |
        //|    0x1F | 0x80000000 | Unsupported   |
        //+---------+------------+---------------+
        //|                          第二个标志  |
        //+---------+------------+---------------+
        //|    0x20 | 0x00000001 | Unsupported   |
        //|    0x21 | 0x00000002 | Unsupported   |
        //|    0x22 | 0x00000004 | Unsupported   |
        //|    0x23 | 0x00000008 | Unsupported   |
        //|    0x24 | 0x00000010 | Unsupported   |
        //|    0x25 | 0x00000020 | DataQuery     |
        //|    0x26 | 0x00000040 | OLE           |
        //|    0x27 | 0x00000080 | Unsupported   |
        //|    0x28 | 0x00000100 | UserControl   |
        //|    0x29 | 0x00000200 | PropertyPage  |
        //|    0x2A | 0x00000400 | Document      |
        //|    0x2B | 0x00000800 | Unsupported   |
        //+---------+------------+---------------+
        //ex: 如果值是0x30F000 (那个被叫做 "静态二进制常量定义在大多数的地方")就是意味着来初始化打印机,窗体,屏幕,剪贴板,组建(0xF000)也有Drive/Dir 组建(0x30000).
        //这是VB工程的一个默认的设置因为这些组建都能从一个模块中获得module (例如,他们是没有图像的除了经常被创造窗体)

        private Int32 _ThreadFlags;
        /// <summary>线程的标志</summary>
        [DataField]
        public Int32 ThreadFlags
        {
            get { return _ThreadFlags; }
            set { _ThreadFlags = value; }
        }

        //* 标记的定义（ThreadFlags数值的含义）
        //+-------+----------------+--------------------------------------------------------+
        //| 值    | 名字           | 描述                                                   |
        //+-------+----------------+--------------------------------------------------------+
        //|  0x01 | ApartmentModel | 特别化的多线程使用一个分开的模型                        |
        //|  0x02 | RequireLicense | 特别化需要进行认证(只对OCX)                             |
        //|  0x04 | Unattended     | 特别化的没有GUI图形界面的元素需要初始化                |
        //|  0x08 | SingleThreaded | 特别化的静态区时单线程的                                |
        //|  0x10 | Retained       | 特别化的将文件保存在内存中(只对Unattended)               |
        //+-------+----------------+--------------------------------------------------------+
        //ex: 如果是0x15就表示是一个既有多线程,内存常驻,并且没有GUI元素要初始化

        private Int32 _ThreadCount;
        /// <summary>线程个数</summary>
        [DataField]
        public Int32 ThreadCount
        {
            get { return _ThreadCount; }
            set { _ThreadCount = value; }
        }

        private Int16 _FormCount;
        /// <summary>窗体个数</summary>
        [DataField]
        public Int16 FormCount
        {
            get { return _FormCount; }
            set { _FormCount = value; }
        }

        private Int16 _ExternalComponentCount;
        /// <summary>VA 外部引用个数例如WINSOCK组件的引用</summary>
        [DataField]
        public Int16 ExternalComponentCount
        {
            get { return _ExternalComponentCount; }
            set { _ExternalComponentCount = value; }
        }

        private Int32 _ThunkCount;
        /// <summary>大概是内存对齐相关的东西</summary>
        [DataField]
        public Int32 ThunkCount
        {
            get { return _ThunkCount; }
            set { _ThunkCount = value; }
        }

        private Int32 _GUITable;
        /// <summary>VA GUI元素表的地址指针（指向一个GUITable_t结构）</summary>
        [DataField(RefType = typeof(GUITable), RefKind = RefKinds.Virtual, SizeField = "FormCount")]
        public Int32 GUITable
        {
            get { return _GUITable; }
            set { _GUITable = value; }
        }

        private Int32 _ExternalComponentTable;
        /// <summary>VA 外部引用表的地址指针</summary>
        [DataField(RefType = typeof(ExternalComponentTable), RefKind = RefKinds.Virtual, SizeField = "ExternalComponentCount")]
        public Int32 ExternalComponentTable
        {
            get { return _ExternalComponentTable; }
            set { _ExternalComponentTable = value; }
        }

        private Int32 _ComRegisterData;
        /// <summary>VA COM注册数据的地址指针</summary>
        [DataField(typeof(ComRegData), RefKinds.Virtual)]
        public Int32 ComRegisterData
        {
            get { return _ComRegisterData; }
            set { _ComRegisterData = value; }
        }

        private Int32 _ProjectExename;
        /// <summary>Offset 指向工程EXE名字的字符串</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 ProjectExename
        {
            get { return _ProjectExename; }
            set { _ProjectExename = value; }
        }

        private Int32 _ProjectTitle;
        /// <summary>Offset 指向工程标题的字符串</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 ProjectTitle
        {
            get { return _ProjectTitle; }
            set { _ProjectTitle = value; }
        }

        private Int32 _HelpFile;
        /// <summary>Offset 指向帮助文件的字符串</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 HelpFile
        {
            get { return _HelpFile; }
            set { _HelpFile = value; }
        }

        private Int32 _ProjectName;
        /// <summary>Offset 指向工程名的字符串</summary>
        [DataField(typeof(String), RefKinds.Relative)]
        public Int32 ProjectName
        {
            get { return _ProjectName; }
            set { _ProjectName = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>工程EXE名字</summary>
        public String ProjectExename2
        {
            get { return (String)this["ProjectExename"]; }
            set { this["ProjectExename"] = value; }
        }

        /// <summary>Offset 指向工程标题的字符串</summary>
        public String ProjectTitle2
        {
            get { return (String)this["ProjectTitle"]; }
            set { this["ProjectTitle"] = value; }
        }

        /// <summary>Offset 指向帮助文件的字符串</summary>
        public String HelpFile2
        {
            get { return (String)this["HelpFile"]; }
            set { this["HelpFile"] = value; }
        }

        /// <summary>Offset 指向工程名的字符串</summary>
        public String ProjectName2
        {
            get { return (String)this["ProjectName"]; }
            set { this["ProjectName"] = value; }
        }

        /// <summary>COM注册数据</summary>
        public ComRegData ComRegisterData2
        {
            get { return this["ComRegisterData"] as ComRegData; }
        }

        /// <summary>工程信息</summary>
        public ProjectInfo ProjectInfo2
        {
            get { return this["ProjectInfo"] as ProjectInfo; }
        }

        /// <summary>
        /// 窗体集合
        /// </summary>
        public GUITable[] GUITables
        {
            get { return GetExtendList<GUITable>("GUITable"); }
        }

        /// <summary>
        /// 扩展组件集合
        /// </summary>
        public ExternalComponentTable[] ExternalComponentTables
        {
            get { return GetExtendList<ExternalComponentTable>("ExternalComponentTable"); }
        }
        #endregion

        #region 方法
        public override void ReadExtendProperty(BinaryReader reader, DataFieldItem dataItem)
        {
            PropertyInfo property = dataItem.Property;
            DataFieldAttribute att = dataItem.Attribute;

            if (property.Name == "ExternalComponentTable")
            {
                long offset = 0;
                if (property.PropertyType == typeof(Int32))
                    offset = (Int32)property.GetValue(this, null);
                else
                    offset = (Int16)property.GetValue(this, null);
                if (offset <= 0) return;

                Int32 count = Convert.ToInt32(this["ExternalComponentCount"]);
                if (count <= 0) return;

                long address = GetRealAddress(offset, dataItem.Attribute.RefKind);
                // 超出文件结尾时，终止读取
                if (address <= 0 || address > reader.BaseStream.Length)
                {
                    Extends.Add(property.Name, null);
                    return;
                }

                // Info.Objects只存单个对象，不存集合，所以这里不用判断

                List<ExternalComponentTable> list = new List<ExternalComponentTable>();
                for (int i = 0; i < count; i++)
                {
                    ExternalComponentTable entity = new ExternalComponentTable();

                    // 读取对象
                    Seek(reader, address);
                    entity.Info = Info;
                    entity.Read(reader);

                    list.Add(entity);

                    address += entity.StructLength;
                }

                Extends.Add(property.Name, list.ToArray());
                return;
            }

            base.ReadExtendProperty(reader, dataItem);
        }

        //public override void ShowExtend(Boolean isShowExtend)
        //{
        //    //base.ShowExtend(false);

        //    //if (ComRegisterData2 != null)
        //    //{
        //    //    WriteLine();
        //    //    ComRegisterData2.Show(isShowExtend);
        //    //}

        //    if (ProjectInfo2 != null)
        //    {
        //        WriteLine();
        //        ProjectInfo2.Show(isShowExtend);
        //    }
        //}

        protected override void ShowExtendProperty(string name, bool isShowExtend)
        {
            if (name == "ExternalComponentTable")
            {
                if (ExternalComponentTables != null && ExternalComponentTables.Length > 0)
                {
                    WriteLine();
                    WriteLine("ExternalComponentTables:");
                    foreach (ExternalComponentTable item in ExternalComponentTables)
                    {
                        WriteLine("{0} {1} {2}", item.FileName2, item.Source2, item.Name2);
                    }
                }

                return;
            }

            base.ShowExtendProperty(name, isShowExtend);
        }
        #endregion
    }
}
