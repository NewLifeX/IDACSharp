using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace VBKiller.Entity
{
    public abstract class EntityBase2
    {
        #region 属性
        private VBInfo _Info = VBInfo.Current;
        /// <summary>VB信息</summary>
        public VBInfo Info
        {
            get { return _Info; }
            set { _Info = value; }
        }

        private long _Address;
        /// <summary>结构基地址</summary>
        public long Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private Dictionary<String, Object> _Extends;
        /// <summary>扩展数据</summary>
        public Dictionary<String, Object> Extends
        {
            get { return _Extends ?? (_Extends = new Dictionary<String, Object>()); }
        }
        #endregion

        #region 读取
        public abstract void Read(BinaryReader reader);

        /// <summary>
        /// 通过目标类型读取扩展，以获得目标类型对象集合
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract EntityBase2[] ReadExtendList(BinaryReader reader, Int32 count);
        #endregion

        #region 显示
        public abstract void Show(Boolean isShowExtend);

        public abstract void ShowExtend(Boolean isShowExtend);
        #endregion
    }
}
