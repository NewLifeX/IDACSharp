using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace VBKiller
{
    /// <summary>
    /// Guid转换器
    /// </summary>
    class GuidConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Byte[])
            {
                Byte[] bts = (Byte[])value;
                if (bts != null && bts.Length == 16) return new Guid(bts).ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
