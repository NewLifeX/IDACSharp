using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VBKiller.Entity
{
    /// <summary>
    /// 这个就是每个OBJECT的结构
    /// </summary>
    [DataObject(Size = 0x30)]
    public class PublicObjectDescriptor : EntityBase<PublicObjectDescriptor>
    {
        #region 属性
        private Int32 _ObjectInfo;
        /// <summary>VA 指向一个ObjectInfo_t类型,来显示这个OBJECT的数据</summary>
        [DataField(typeof(ObjectInfo), RefKinds.Virtual)]
        public Int32 ObjectInfo
        {
            get { return _ObjectInfo; }
            set { _ObjectInfo = value; }
        }

        private Int32 _Const1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const1
        {
            get { return _Const1; }
            set { _Const1 = value; }
        }

        private Int32 _PublicBytes;
        /// <summary>VA 指向公用变量表大小</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 PublicBytes
        {
            get { return _PublicBytes; }
            set { _PublicBytes = value; }
        }

        private Int32 _StaticBytes;
        /// <summary>VA 指向静态变量表地址</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 StaticBytes
        {
            get { return _StaticBytes; }
            set { _StaticBytes = value; }
        }

        private Int32 _ModulePublic;
        /// <summary>VA 指向公用变量表</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ModulePublic
        {
            get { return _ModulePublic; }
            set { _ModulePublic = value; }
        }

        private Int32 _ModuleStatic;
        /// <summary>VA 指向静态变量表</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ModuleStatic
        {
            get { return _ModuleStatic; }
            set { _ModuleStatic = value; }
        }

        private Int32 _ObjectName;
        /// <summary>VA 字符串,这个OBJECT的名字</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 ObjectName
        {
            get { return _ObjectName; }
            set { _ObjectName = value; }
        }

        private Int32 _ProcCount;
        /// <summary>events, funcs, subs(事件\函数\过程)数目</summary>
        [DataField]
        public Int32 ProcCount
        {
            get { return _ProcCount; }
            set { _ProcCount = value; }
        }

        private Int32 _ProcNamesArray;
        /// <summary>VA 一般都是0</summary>
        [DataField(RefType = typeof(ProcName), RefKind = RefKinds.Virtual, SizeField = "ProcCount")]
        public Int32 ProcNamesArray
        {
            get { return _ProcNamesArray; }
            set { _ProcNamesArray = value; }
        }

        private Int32 _StaticVar;
        /// <summary>OFFSET  从aModuleStatic指向的静态变量表偏移</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 StaticVar
        {
            get { return _StaticVar; }
            set { _StaticVar = value; }
        }

        private Int32 _ObjectType;
        /// <summary>比较重要显示了这个OBJECT的实行,具体见下表</summary>
        [DataField]
        public Int32 ObjectType
        {
            get { return _ObjectType; }
            set { _ObjectType = value; }
        }

        private Int32 _Null3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null3
        {
            get { return _Null3; }
            set { _Null3 = value; }
        }
        #endregion

        //Object_t.ObjectTyper 属性...//重要的属性表部分
        //#########################################################
        //form:              0000 0001 1000 0000 1000 0011 --> 18083
        //                   0000 0001 1000 0000 1010 0011 --> 180A3
        //                   0000 0001 1000 0000 1100 0011 --> 180C3
        //module:            0000 0001 1000 0000 0000 0001 --> 18001
        //                   0000 0001 1000 0000 0010 0001 --> 18021
        //class:             0001 0001 1000 0000 0000 0011 --> 118003
        //                   0001 0011 1000 0000 0000 0011 --> 138003
        //                   0000 0001 1000 0000 0010 0011 --> 18023
        //                   0000 0001 1000 1000 0000 0011 --> 18803
        //                   0001 0001 1000 1000 0000 0011 --> 118803
        //usercontrol:       0001 1101 1010 0000 0000 0011 --> 1DA003
        //                   0001 1101 1010 0000 0010 0011 --> 1DA023
        //                   0001 1101 1010 1000 0000 0011 --> 1DA803
        //propertypage:      0001 0101 1000 0000 0000 0011 --> 158003
        //                      | ||     |  |    | |    |
        //[moog]                | ||     |  |    | |    |
        //HasPublicInterface ---+ ||     |  |    | |    |  （有公用的接口）
        //HasPublicEvents --------+|     |  |    | |    |  （有公用的事件）
        //IsCreatable/Visible? ----+     |  |    | |    |  （是否可以创建，可见）
        //Same as "HasPublicEvents" -----+  |    | |    |  
        //[aLfa]                         |  |    | |    |
        //usercontrol (1) ---------------+  |    | |    |  （用户控制）
        //ocx/dll (1) ----------------------+    | |    |  （OCX/DLL）
        //form (1) ------------------------------+ |    |  （是不是FORM是就是1）
        //vb5 (1) ---------------------------------+    |  （是不是VB5是就是1）
        //HasOptInfo (1) -------------------------------+  （有没有额外的信息信息由就是1,决定是不是指向OptionalObjectInfo_t类似与PEHEAD里的Optional信息一样）
        //                                              |
        //module(0) ------------------------------------+   （如果是Module模块就这里是0）

        #region 扩展属性
        /// <summary>对象信息</summary>
        public ObjectInfo ObjectInfo2
        {
            get { return this["ObjectInfo"] as ObjectInfo; }
        }

        public String Type
        {
            get
            {
                switch (ObjectType)
                {
                    case 0x18083:
                    case 0x180A3:
                    case 0x180C3:
                        return "Form";
                    case 0x18001:
                    case 0x18021:
                        return "Module";
                    case 0x118003:
                    case 0x138003:
                    case 0x18023:
                    case 0x18803:
                    case 0x118803:
                        return "Class";
                    case 0x1DA003:
                    case 0x1DA023:
                    case 0x1DA803:
                        return "'UserControl";
                    case 0x158003:
                        return "'PropertyPage";
                    default:
                        return ObjectType.ToString("X") + "h";
                }
            }
        }

        /// <summary>
        /// 是否有扩展
        /// </summary>
        public Boolean HasOptionalInfo
        {
            get
            {
                return (ObjectType & 2) == 2;
            }
        }

        /// <summary>可选对象信息</summary>
        public OptionalObjectInfo OptionalObjectInfo
        {
            get { return this["OptionalObjectInfo"] as OptionalObjectInfo; }
        }

        /// <summary>名称</summary>
        public String Name
        {
            get { return (String)this["ObjectName"]; }
        }

        /// <summary>扩展库</summary>
        public ProcName[] ProcNames
        {
            get { return GetExtendList<ProcName>("ProcNamesArray"); }
        }

        //private Dictionary<Int32, String> _ProcNames;
        ///// <summary>函数名集合</summary>
        //public Dictionary<Int32, String> ProcNames
        //{
        //    get
        //    {
        //        if (_ProcNames == null && ProcNamesArray > 0 && ProcCount > 0)
        //        {
        //            _ProcNames = new Dictionary<int, string>();
        //            long address = GetRealAddress(ProcNamesArray);
        //            for (int i = 0; i < ProcCount; i++)
        //            {
        //                Seek(Reader, address + i * 4);
        //                long p = Reader.ReadInt32();
        //                if (p > 0)
        //                {
        //                    Seek(Reader, GetRealAddress(p));
        //                    _ProcNames.Add(i, ReadString(Reader));
        //                }
        //                else
        //                {
        //                    _ProcNames.Add(i, null);
        //                }
        //            }
        //        }
        //        return _ProcNames;
        //    }
        //}
        #endregion

        #region 方法
        public override void ReadExtend()
        {
            base.ReadExtend();

            // 读取可选对象信息
            if (HasOptionalInfo) ReadOptionalObjectInfo();
        }

        protected override void TryReadExtend(string name)
        {
            if (name == "OptionalObjectInfo" && HasOptionalInfo)
                ReadOptionalObjectInfo();
            else
                base.TryReadExtend(name);
        }

        private Boolean hasRead = false;
        void ReadOptionalObjectInfo()
        {
            if (hasRead) return;
            hasRead = true;

            Seek(Reader, ObjectInfo2.Address + VBKiller.Entity.ObjectInfo.ObjectSize);
            OptionalObjectInfo entity = new OptionalObjectInfo();
            entity.Info = Info;
            entity.Read(Reader);

            Extends.Add("OptionalObjectInfo", entity);
        }

        //public override void ShowExtend(bool isShowExtend)
        //{
        //    //base.ShowExtend(isShowExtend);

        //    if (ObjectInfo2 != null)
        //    {
        //        WriteLine();
        //        ObjectInfo2.Show(isShowExtend);
        //    }

        //    if (OptionalObjectInfo != null)
        //    {
        //        WriteLine();
        //        OptionalObjectInfo.Show(isShowExtend);
        //    }

        //    if (ProcNames != null && ProcNames.Length > 0)
        //    {
        //        for (int i = 0; i < ProcNames.Length; i++)
        //        {
        //            //String name = ProcNames[i].Name;
        //            //if (String.IsNullOrEmpty(name)) name = String.Format("{0}_{1}", Name, i);

        //            WriteLine("{0}_{1}", Name, ProcNames[i].FriendName);
        //        }
        //    }
        //}

        protected override void ShowExtendProperty(String name, bool isShowExtend)
        {
            if (name == "ProcNamesArray")
            {
                if (ProcNames != null && ProcNames.Length > 0)
                {
                    WriteLine();
                    WriteLine("ProcNames:");
                    for (int i = 0; i < ProcNames.Length; i++)
                    {
                        WriteLine("{0}_{1}", Name, ProcNames[i].FriendName);
                    }
                }

                return;
            }

            base.ShowExtendProperty(name, isShowExtend);
        }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Type))
                return String.Format("{0}({1})", Name, Type);
            else
                return Name;
        }
        #endregion
    }
}