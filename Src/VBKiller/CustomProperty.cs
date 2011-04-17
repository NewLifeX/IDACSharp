using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using VBKiller.Entity;

namespace VBKiller
{
    /// <summary>
    /// 自定义属性
    /// </summary>
    class CustomProperty : ICustomTypeDescriptor
    {
        //当前选择对象
        private object selectObject;

        public CustomProperty(object pSelectObject)
        {
            selectObject = pSelectObject;
        }

        #region ICustomTypeDescriptor Members
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(selectObject);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(selectObject);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(selectObject);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(selectObject);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(selectObject);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(selectObject);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(selectObject, editorBaseType);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(selectObject, attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(selectObject);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<CustomPropertyDescriptor> list = new List<CustomPropertyDescriptor>();
            PropertyDescriptorCollection list2 = TypeDescriptor.GetProperties(selectObject, attributes);

            if (list2 == null || list2.Count <= 0) return list2;

            Dictionary<String, DataFieldItem> dic = DataFieldItem.GetFields(selectObject.GetType());

            foreach (PropertyDescriptor item in list2)
            {
                CustomPropertyDescriptor entity = new CustomPropertyDescriptor(selectObject, item);
                if (dic.ContainsKey(item.Name))
                {
                    entity.SetCategory("数据");

                    entity.DataField = dic[item.Name];
                }
                else
                    entity.SetCategory("其它");

                list.Add(entity);
            }

            return new PropertyDescriptorCollection(list.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(selectObject);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return selectObject;
        }
        #endregion

        class CustomPropertyDescriptor : PropertyDescriptor
        {
            private PropertyDescriptor mProp;
            private object mComponent;

            private DataFieldItem _DataField;
            /// <summary>数据字段</summary>
            public DataFieldItem DataField
            {
                get { return _DataField; }
                set { _DataField = value; }
            }

            public CustomPropertyDescriptor(object pComponent, PropertyDescriptor pPD)
                : base(pPD)
            {
                mCategory = base.Category;
                mDisplayName = base.DisplayName;
                mProp = pPD;
                mComponent = pComponent;
            }

            private string mCategory;
            public override string Category
            {
                get { return mCategory; }
            }

            private string mDisplayName;
            public override string DisplayName
            {
                get { return mDisplayName; }
            }

            public void SetDisplayName(string pDispalyName)
            {
                mDisplayName = pDispalyName;
            }

            public void SetCategory(string pCategory)
            {
                mCategory = pCategory;
            }

            public override bool CanResetValue(object component)
            {
                return mProp.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return mProp.ComponentType; }
            }

            public override object GetValue(object component)
            {
                return mProp.GetValue(component);
            }

            public override bool IsReadOnly
            {
                get { return mProp.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return mProp.PropertyType; }
            }
            public override void ResetValue(object component) { mProp.ResetValue(component); }

            public override void SetValue(object component, object value) { mProp.SetValue(component, value); }

            public override bool ShouldSerializeValue(object component)
            {
                return mProp.ShouldSerializeValue(component);
            }

            public override TypeConverter Converter
            {
                get
                {
                    if (DataField != null)
                    {
                        if (DataField.Property.PropertyType == typeof(Int32))
                        {
                            if (DataField.Attribute.RefType != null) return new AddressConverter();

                        }
                        else if (DataField.Property.PropertyType == typeof(Byte[]))
                        {
                            if (DataField.Attribute.Size == 16) return new GuidConverter();
                        }
                    }

                    return base.Converter;
                }
            }
        }
    }
}
