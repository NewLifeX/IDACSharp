using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    [DataObject(Size = 0x8)]
    public class ExternalTable : EntityBase<ExternalTable>
    {
        private Int32 _Flag;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }

        private Int32 _ExternalLibrary;
        /// <summary>属性说明</summary>
        [DataField(typeof(ExternalLibrary), RefKinds.Virtual)]
        public Int32 ExternalLibrary
        {
            get { return _ExternalLibrary; }
            set { _ExternalLibrary = value; }
        }

        #region 扩展属性
        /// <summary>扩展库</summary>
        public ExternalLibrary ExternalLibrary2
        {
            get { return this["ExternalLibrary"] as ExternalLibrary; }
        }
        #endregion

        #region 方法
        //public override void Show()
        //{
        //    base.Show();

        //    if (ExternalLibrary2 != null)
        //    {
        //        ExternalLibrary2.ReadExtend();
        //        WriteLine(String.Empty);
        //        ExternalLibrary2.Show();
        //    }
        //}
        #endregion
    }
}
