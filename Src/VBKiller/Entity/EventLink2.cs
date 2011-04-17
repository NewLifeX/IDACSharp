using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x4)]
    public class EventLink2 : EntityBase<EventLink2>
    {
        private Int32 _Jump;
        /// <summary>跳转地址</summary>
        [DataField(RefType = typeof(Int32))]
        public Int32 Jump
        {
            get { return _Jump; }
            set { _Jump = value; }
        }

        //public String Name { get { return (String)Extends["Jump"]; } }

        //public override string ToString()
        //{
        //    if (Jump > 0 && !String.IsNullOrEmpty(Name))
        //        return Name;
        //    else
        //        return base.ToString();
        //}
    }
}
