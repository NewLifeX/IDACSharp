using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    public class TypeProcedureList : EntityBase<TypeProcedureList>
    {
        private String _Parent;
        /// <summary>属性说明</summary>
        public String Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private String _ProcedureName;
        /// <summary>属性说明</summary>
        public String ProcedureName
        {
            get { return _ProcedureName; }
            set { _ProcedureName = value; }
        }
    }
}
