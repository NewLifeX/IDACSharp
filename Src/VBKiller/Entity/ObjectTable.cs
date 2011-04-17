using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>这些东西十分重要，是提取VB工程元素的重要标志，从这里我们可以得到这个工程里有多少FORM,多少MODULE,以及他们的重要索引数据，
    /// 可以说重要性和SECTION HEAD在EXE程序中重要性相同。
    /// 入口的地址是由ProjectInfo_t结构提供的(semi提供关于这里提供的资料少之又少，所以需要自己摸索）
    /// 从ProjectInfo_t开始指向的是一个ObjectTable_t,由ObjectTable_t提供第一个OBJECT_t结构的地址</remarks>
    [DataObject(Size = 0x54)]
    public class ObjectTable : EntityBase<ObjectTable>
    {
        #region 属性
        private Int32 _HeapLink;
        /// <summary>Unused after compilation, always 0.</summary>
        [DataField]
        public Int32 HeapLink
        {
            get { return _HeapLink; }
            set { _HeapLink = value; }
        }

        private Int32 _ExecProj;
        /// <summary>VA指向一块内存结构 Pointer to VB Project Exec COM Object</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ExecProj
        {
            get { return _ExecProj; }
            set { _ExecProj = value; }
        }

        private Int32 _ProjectInfo2;
        /// <summary>VA指向Project Info 2</summary>
        [DataField(typeof(ProjectInfo2), RefKinds.Virtual)]
        public Int32 ProjectInfo2
        {
            get { return _ProjectInfo2; }
            set { _ProjectInfo2 = value; }
        }

        private Int32 _Const1;
        /// <summary>没有用的填充东西</summary>
        [DataField]
        public Int32 Const1
        {
            get { return _Const1; }
            set { _Const1 = value; }
        }

        private Int32 _Null2;
        /// <summary>没有用的填充东西</summary>
        [DataField]
        public Int32 Null2
        {
            get { return _Null2; }
            set { _Null2 = value; }
        }

        private Int32 _ProjectObject;
        /// <summary>Pointer to in-memory Project Data</summary>
        [DataField(typeof(Int32), RefKinds.Virtual)]
        public Int32 ProjectObject
        {
            get { return _ProjectObject; }
            set { _ProjectObject = value; }
        }

        private Byte[] _uuidObject;
        /// <summary>GUID of the Object Table</summary>
        [DataField(Size = 16)]
        public Byte[] uuidObject
        {
            get { return _uuidObject; }
            set { _uuidObject = value; }
        }

        private Int16 _CompileType;
        /// <summary>Internal flag used during compilation</summary>
        [DataField]
        public Int16 CompileType
        {
            get { return _CompileType; }
            set { _CompileType = value; }
        }

        private Int16 _ObjectCount;
        /// <summary>OBEJCT数量1</summary>
        [DataField]
        public Int16 ObjectCount
        {
            get { return _ObjectCount; }
            set { _ObjectCount = value; }
        }

        private Int16 _CompiledObjects;
        /// <summary>编译后OBJECT数量</summary>
        [DataField]
        public Int16 CompiledObjects
        {
            get { return _CompiledObjects; }
            set { _CompiledObjects = value; }
        }

        private Int16 _ObjectsInUse;
        /// <summary>Updated in the IDE to correspond the total number ' but will go up or down when initializing/unloading modules.</summary>
        [DataField]
        public Int16 ObjectsInUse
        {
            get { return _ObjectsInUse; }
            set { _ObjectsInUse = value; }
        }

        private Int32 _Object;
        /// <summary>VA指向第一个OBJECT_t结构，很重要</summary>
        [DataField(RefType = typeof(PublicObjectDescriptor), RefKind = RefKinds.Virtual, SizeField = "ObjectCount")]
        public Int32 Object
        {
            get { return _Object; }
            set { _Object = value; }
        }

        private Int32 _Null3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null3
        {
            get { return _Null3; }
            set { _Null3 = value; }
        }

        private Int32 _Null4;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null4
        {
            get { return _Null4; }
            set { _Null4 = value; }
        }

        private Int32 _Null5;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null5
        {
            get { return _Null5; }
            set { _Null5 = value; }
        }

        private Int32 _ProjectName;
        /// <summary>执行工程名字的字符串</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 ProjectName
        {
            get { return _ProjectName; }
            set { _ProjectName = value; }
        }

        private Int32 _LangID1;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 LangID1
        {
            get { return _LangID1; }
            set { _LangID1 = value; }
        }

        private Int32 _LangID2;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 LangID2
        {
            get { return _LangID2; }
            set { _LangID2 = value; }
        }

        private Int32 _Null6;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Null6
        {
            get { return _Null6; }
            set { _Null6 = value; }
        }

        private Int32 _Const3;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Const3
        {
            get { return _Const3; }
            set { _Const3 = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>扩展库</summary>
        public ProjectInfo2 ProjectInfo22
        {
            get { return this["ProjectInfo2"] as ProjectInfo2; }
        }

        /// <summary>扩展库</summary>
        public PublicObjectDescriptor[] Objects
        {
            get { return GetExtendList<PublicObjectDescriptor>("Object"); }
        }
        #endregion

        #region 方法
        //public override void ShowExtend(bool isShowExtend)
        //{
        //    base.ShowExtend(isShowExtend);

        //    //if (ProjectInfo22 != null)
        //    //{
        //    //    WriteLine();
        //    //    ProjectInfo22.Show(isShowExtend);
        //    //}

        //    //if (Objects != null && Objects.Length > 0)
        //    //{
        //    //    //WriteLine();
        //    //    //Objects[0].Show(isShowExtend);
        //    //    //WriteLine();
        //    //    //Objects[1].Show(isShowExtend);
        //    //    //WriteLine();
        //    //    //Objects[2].Show(isShowExtend);

        //    //    foreach (PublicObjectDescriptor item in Objects)
        //    //    {
        //    //        //WriteLine("{0} {1:X}h", item, item.ObjectType);
        //    //        //WriteLine(item.ToString());

        //    //        if (item.ProcNamesArray <= 0) continue;

        //    //        //WriteLine();
        //    //        //item.Show(isShowExtend);
        //    //        //foreach (String elm in item.ProcNames.Values)
        //    //        //{
        //    //        //    if (!String.IsNullOrEmpty(elm)) WriteLine(elm);
        //    //        //}

        //    //        if (!item.HasOptionalInfo) continue;
        //    //        WriteLine((item.ProcNames == null).ToString());

        //    //        WriteLine();
        //    //        item.Show(isShowExtend);

        //    //        break;
        //    //    }
        //    //}
        //}
        #endregion
    }
}
