using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x8)]
    public class ExternalLibrary : EntityBase<ExternalLibrary>
    {
        private Int32 _LibraryName;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 LibraryName
        {
            get { return _LibraryName; }
            set { _LibraryName = value; }
        }

        private Int32 _LibraryFunction;
        /// <summary>属性说明</summary>
        [DataField(typeof(String), RefKinds.Virtual)]
        public Int32 LibraryFunction
        {
            get { return _LibraryFunction; }
            set { _LibraryFunction = value; }
        }

        #region 扩展属性
        /// <summary>库名</summary>
        public String LibraryName2
        {
            get { return (String)this["LibraryName"]; }
        }

        /// <summary>库函数</summary>
        public String LibraryFunction2
        {
            get { return (String)this["LibraryFunction"]; }
        }
        #endregion
    }
}
