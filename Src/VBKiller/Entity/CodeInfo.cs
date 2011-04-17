using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    public class CodeInfo : EntityBase<CodeInfo>
    {
        #region 属性
        private Int32 _ObjectInfo;
        /// <summary>属性说明</summary>
        public Int32 ObjectInfo
        {
            get { return _ObjectInfo; }
            set { _ObjectInfo = value; }
        }

        private Int16 _Flag1;
        /// <summary>属性说明</summary>
        public Int16 Flag1
        {
            get { return _Flag1; }
            set { _Flag1 = value; }
        }

        private Int16 _Flag2;
        /// <summary>属性说明</summary>
        public Int16 Flag2
        {
            get { return _Flag2; }
            set { _Flag2 = value; }
        }

        private Int16 _CodeLength;
        /// <summary>属性说明</summary>
        public Int16 CodeLength
        {
            get { return _CodeLength; }
            set { _CodeLength = value; }
        }

        private Int32 _Flag3;
        /// <summary>属性说明</summary>
        public Int32 Flag3
        {
            get { return _Flag3; }
            set { _Flag3 = value; }
        }

        private Int16 _Flag4;
        /// <summary>属性说明</summary>
        public Int16 Flag4
        {
            get { return _Flag4; }
            set { _Flag4 = value; }
        }

        private Int16 _Null1;
        /// <summary>属性说明</summary>
        public Int16 Null1
        {
            get { return _Null1; }
            set { _Null1 = value; }
        }

        private Int32 _Flag5;
        /// <summary>属性说明</summary>
        public Int32 Flag5
        {
            get { return _Flag5; }
            set { _Flag5 = value; }
        }

        private Int16 _Flag6;
        /// <summary>属性说明</summary>
        public Int16 Flag6
        {
            get { return _Flag6; }
            set { _Flag6 = value; }
        }
        #endregion
    }
}
