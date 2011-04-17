using System;
using System.Collections.Generic;
using System.Text;

namespace VBKiller.Entity
{
    /// <summary>
    /// 数据目录
    /// <remarks>详见winnt.h中的IMAGE_DATA_DIRECTORY（9388）</remarks>
    /// </summary>
    [DataObject(Size = 0x8)]
    public class DataDirectory : EntityBase<DataDirectory>
    {
        #region 属性
        private Int32 _VirtualAddress;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 VirtualAddress
        {
            get { return _VirtualAddress; }
            set { _VirtualAddress = value; }
        }

        private Int32 _Size;
        /// <summary>属性说明</summary>
        [DataField]
        public Int32 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }
        #endregion
    }
}
