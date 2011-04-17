using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    public class TypeApiList : EntityBase<TypeApiList>
    {
        private String _LibraryName;
        /// <summary>属性说明</summary>
        public String LibraryName
        {
            get { return _LibraryName; }
            set { _LibraryName = value; }
        }

        private Int32 _FunctionName;
        /// <summary>属性说明</summary>
        public Int32 FunctionName
        {
            get { return _FunctionName; }
            set { _FunctionName = value; }
        }
    }
}
