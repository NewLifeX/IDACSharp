using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace VBKiller.Entity
{
    /// <summary>
    /// 数据字段项
    /// </summary>
    public class DataFieldItem
    {
        private PropertyInfo _Property;
        /// <summary>属性</summary>
        public PropertyInfo Property
        {
            get { return _Property; }
            private set { _Property = value; }
        }

        private DataFieldAttribute _Attribute;
        /// <summary>特性</summary>
        public DataFieldAttribute Attribute
        {
            get { return _Attribute; }
            private set { _Attribute = value; }
        }

        DataFieldItem(PropertyInfo property, DataFieldAttribute attribute)
        {
            Property = property;
            Attribute = attribute;
        }

        private static Dictionary<Type, Dictionary<String, DataFieldItem>> _fields = new Dictionary<Type, Dictionary<String, DataFieldItem>>();
        /// <summary>
        /// 取得所有数据字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<String, DataFieldItem> GetFields(Type type)
        {
            if (_fields.ContainsKey(type)) return _fields[type];
            lock (_fields)
            {
                if (_fields.ContainsKey(type)) return _fields[type];

                PropertyInfo[] pis = type.GetProperties();
                if (pis == null || pis.Length <= 0) return null;

                Dictionary<String, DataFieldItem> dic = new Dictionary<String, DataFieldItem>();
                foreach (PropertyInfo item in pis)
                {
                    DataFieldAttribute att = DataFieldAttribute.GetAttribute(item);
                    if (att != null) dic.Add(item.Name, new DataFieldItem(item, att));
                }

                if (dic.Count <= 0) dic = null;

                _fields[type] = dic;

                return dic;
            }
        }

        public override string ToString()
        {
            return Property.Name;
        }
    }
}
