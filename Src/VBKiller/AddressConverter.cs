using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace VBKiller
{
    /// <summary>
    /// 地址编辑器
    /// </summary>
    class AddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String) || sourceType == typeof(Int32)) return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String) || destinationType == typeof(Int32)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) return 0;

            if (value is String)
            {
                String str = value as String;
                if (!String.IsNullOrEmpty(str) && str.StartsWith("0x"))
                {
                    str = str.Substring(2);
                    return Convert.ToInt32(str, 16);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Int32)
            {
                return String.Format("0x{0:X}", value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        //public override bool IsValid(ITypeDescriptorContext context, object value)
        //{
        //    return base.IsValid(context, value);
        //}
    }
}
