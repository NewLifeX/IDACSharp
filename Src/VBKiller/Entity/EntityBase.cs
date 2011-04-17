using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace VBKiller.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class EntityBase<TEntity> : EntityBase2 where TEntity : EntityBase<TEntity>, new()
    {
        #region 属性
        /// <summary>
        /// 结构大小
        /// </summary>
        public static Int32 ObjectSize
        {
            get
            {
                // 取得成员列表
                Dictionary<String, DataFieldItem> dic = GetFields(typeof(TEntity));
                if (dic == null || dic.Count <= 0) return 0;

                Int32 n = 0;
                foreach (DataFieldItem item in dic.Values)
                {
                    DataFieldAttribute att = item.Attribute;
                    if (att.Size > 0)
                        n += att.Size;
                    else if (item.Property.PropertyType == typeof(String) && !String.IsNullOrEmpty(att.SizeField))
                        n += 4;
                    else
                        n += Marshal.SizeOf(item.Property.PropertyType);
                }
                return n;
            }
        }
        #endregion

        #region 构造函数
        static EntityBase()
        {
            Type type = typeof(TEntity);
            DataObjectAttribute att = DataObjectAttribute.GetAttribute(type);
            if (att == null) throw new Exception(String.Format("类{0}缺少{1}特性！", type.Name, typeof(DataObjectAttribute).Name));

            if (att.Size > 0 && ObjectSize != att.Size)
            {
                ShowPosition();

                throw new Exception(String.Format("类{0}指定大小0x{1:X}，实际大小0x{2:X}！", type.Name, att.Size, ObjectSize));
            }
        }

        static void ShowPosition()
        {
            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(typeof(TEntity));
            if (dic == null || dic.Count <= 0) return;

            Int32 n = 0;
            foreach (DataFieldItem item in dic.Values)
            {
                WriteLine("{0:X}h ({0}d)", n);

                DataFieldAttribute att = item.Attribute;
                if (att.Size > 0)
                    n += att.Size;
                else
                    n += Marshal.SizeOf(item.Property.PropertyType);
            }
        }
        #endregion

        #region 读取数据
        protected BinaryReader Reader;
        private Boolean hasRead = false;

        public override void Read(BinaryReader reader)
        {
            if (hasRead) return;

            if (reader == null) throw new ArgumentNullException("reader");

            hasRead = true;
            Reader = reader;
            Address = reader.BaseStream.Position;

            if (!Info.Objects.ContainsKey(Address)) Info.Objects.Add(Address, this);

            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            if (dic == null || dic.Count <= 0) return;

            // 遍历成员，读取数据
            foreach (DataFieldItem item in dic.Values)
            {
                //if (item.Attribute.RefType == null)
                ReadProperty(reader, item);
                //else
                //    ReadRefProperty(reader, item.Property, item.Attribute);
            }
        }

        /// <summary>
        /// 读取指定成员
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dataItem"></param>
        public virtual void ReadProperty(BinaryReader reader, DataFieldItem dataItem)
        {
            PropertyInfo property = dataItem.Property;
            DataFieldAttribute att = dataItem.Attribute;

            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Boolean:
                    break;
                case TypeCode.Byte:
                    property.SetValue(this, reader.ReadByte(), null);
                    return;
                case TypeCode.Char:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.DateTime:
                    break;
                case TypeCode.Decimal:
                    break;
                case TypeCode.Double:
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Int16:
                    property.SetValue(this, reader.ReadInt16(), null);
                    return;
                case TypeCode.Int32:
                    property.SetValue(this, reader.ReadInt32(), null);
                    return;
                case TypeCode.Int64:
                    property.SetValue(this, reader.ReadInt64(), null);
                    return;
                case TypeCode.Object:
                    if (property.PropertyType == typeof(Byte[]))
                    {
                        if (att.Size <= 0) throw new InvalidOperationException(String.Format("字节数组{0}.{1}的大小未设置！", this.GetType().Name, property.Name));
                        property.SetValue(this, reader.ReadBytes(att.Size), null);
                        return;
                    }
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.Single:
                    break;
                case TypeCode.String:
                    Int32 size = att.Size;
                    if (size <= 0)
                    {
                        if (!String.IsNullOrEmpty(att.SizeField)) size = Convert.ToInt32(this[att.SizeField]);

                        if (size <= 0) throw new InvalidOperationException(String.Format("字符串{0}.{1}的长度未设置！", this.GetType().Name, property.Name));
                    }
                    Byte[] buf = reader.ReadBytes(size);
                    List<Byte> list = new List<Byte>();
                    foreach (Byte item in buf)
                    {
                        if (item != 0) list.Add(item);
                    }
                    if (list.Count > 0)
                    {
                        buf = list.ToArray();
                        property.SetValue(this, Encoding.UTF8.GetString(buf), null);
                    }
                    return;
                case TypeCode.UInt16:
                    property.SetValue(this, reader.ReadUInt16(), null);
                    return;
                case TypeCode.UInt32:
                    property.SetValue(this, reader.ReadUInt32(), null);
                    return;
                case TypeCode.UInt64:
                    property.SetValue(this, reader.ReadUInt64(), null);
                    return;
                default:
                    break;
            }

            throw new InvalidOperationException(String.Format("{0}.{1}的类型{2}不受支持！", this.GetType().Name, property.Name, property.PropertyType.Name));
        }

        ///// <summary>
        ///// 读取引用成员
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <param name="property"></param>
        ///// <param name="att"></param>
        //public virtual void ReadRefProperty(BinaryReader reader, PropertyInfo property, DataFieldAttribute att)
        //{
        //    // 对于引用成员，一律读取四个字节作为地址
        //    if (property.PropertyType != typeof(Int32) && property.PropertyType != typeof(Int16))
        //        throw new InvalidOperationException(String.Format("引用成员{0}.{1}的类型{2}不正确，应该是Int32！", this.GetType().Name, property.Name, property.PropertyType.Name));

        //    if (property.PropertyType == typeof(Int32))
        //    {
        //        Int32 offset = reader.ReadInt32();
        //        property.SetValue(this, offset, null);
        //    }
        //    else
        //    {
        //        Int16 offset = reader.ReadInt16();
        //        property.SetValue(this, offset, null);
        //    }
        //}

        private List<String> hasReadExtend = new List<String>();

        public virtual void ReadExtend()
        {
            BinaryReader reader = Reader;
            Read(reader);

            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            if (dic == null || dic.Count <= 0) return;

            // 遍历成员，读取数据
            foreach (DataFieldItem item in dic.Values)
            {
                if (item.Attribute.RefType == null || hasReadExtend.Contains(item.Property.Name)) continue;
                hasReadExtend.Add(item.Property.Name);

                ReadExtendProperty(reader, item);
            }
        }

        /// <summary>
        /// 读取扩展成员
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dataItem"></param>
        public virtual void ReadExtendProperty(BinaryReader reader, DataFieldItem dataItem)
        {
            PropertyInfo property = dataItem.Property;
            DataFieldAttribute att = dataItem.Attribute;

            if (att.RefType == typeof(Int32)) return;

            long offset = 0;
            if (property.PropertyType == typeof(Int32))
                offset = (Int32)property.GetValue(this, null);
            else
                offset = (Int16)property.GetValue(this, null);
            if (offset <= 0) return;

            // 检查位移是否超出文件结尾
            long address = GetRealAddress(offset, att.RefKind);
            // 超出文件结尾时，终止读取
            if (address <= 0 || address > reader.BaseStream.Length)
            {
                Extends.Add(property.Name, null);
                return;
            }

            // 读取扩展数据
            if (att.RefType == typeof(String))
            {
                // 字符串仅仅是偏移量，需要加上基地址
                //Seek(reader, Address + offset);
                Seek(reader, address);
                String str = ReadString(reader);

                //Extends[property.Name] = str;
                Extends.Add(property.Name, str);
            }
            else
            {
                //offset = GetRealAddress(offset, att.RefKind);

                // 简单对象/对象集合
                if (String.IsNullOrEmpty(att.SizeField))
                {
                    #region 简单对象
                    // 首先判断是否在全局缓存里面
                    if (!Info.Objects.ContainsKey(address))
                    {
                        // 创建对象
                        EntityBase2 entity = Activator.CreateInstance(att.RefType) as EntityBase2;
                        if (entity == null) throw new InvalidOperationException(String.Format("引用成员{0}.{1}的引用类型{2}无法识别！", this.GetType().Name, property.Name, att.RefType.Name));

                        Seek(reader, address);
                        entity.Info = Info;

                        // 读取对象
                        entity.Read(reader);

                        //Extends[property.Name] = entity;
                        //Info.Objects[offset] = entity;
                        // 故意使用Add方法，避免重新赋值
                        Extends.Add(property.Name, entity);
                        //Info.Objects.Add(offset, entity);
                    }
                    else
                    {
                        //Extends[property.Name] = Info.Objects[offset];
                        Extends.Add(property.Name, Info.Objects[address]);

                        //#if DEBUG
                        //                    WriteLine("地址0x{0:X}已分析！", offset);
                        //#endif
                    }
                    #endregion
                }
                else
                {
                    // Info.Objects只存单个对象，不存集合，所以这里不用判断

                    // 创建对象
                    EntityBase2 entity = Activator.CreateInstance(att.RefType) as EntityBase2;
                    if (entity == null) throw new InvalidOperationException(String.Format("引用成员{0}.{1}的引用类型{2}无法识别！", this.GetType().Name, property.Name, att.RefType.Name));

                    Int32 size = Convert.ToInt32(this[att.SizeField]);
                    if (size > 0)
                    {
                        // 先移到第一个对象所在位置
                        Seek(reader, address);
                        entity.Info = Info;
                        Object list = entity.ReadExtendList(reader, size);

                        //Extends[property.Name] = list;
                        //Info.Objects[offset] = list;
                        Extends.Add(property.Name, list);
                        //Info.Objects.Add(offset, list);
                    }
                }
            }
        }

        /// <summary>
        /// 通过目标类型读取扩展，以获得目标类型对象集合
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override EntityBase2[] ReadExtendList(BinaryReader reader, Int32 count)
        {
            List<EntityBase2> list = new List<EntityBase2>(count);
            for (int i = 0; i < count; i++)
            {
                EntityBase2 entity = Activator.CreateInstance(this.GetType()) as EntityBase2;

                // 读取对象
                entity.Info = Info;
                entity.Read(reader);

                list.Add(entity);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 试图读取扩展。当被访问字段为引用类型时才读取
        /// </summary>
        /// <param name="name"></param>
        protected virtual void TryReadExtend(String name)
        {
            if (hasReadExtend.Contains(name)) return;

            //Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            //if (dic == null || dic.Count <= 0 || !dic.ContainsKey(name)) return;

            DataFieldItem field = GetField(name);
            if (field == null || field.Attribute.RefType == null || hasReadExtend.Contains(field.Property.Name)) return;
            hasReadExtend.Add(field.Property.Name);

            ReadExtendProperty(Reader, field);
        }

        /// <summary>
        /// 只读字符串扩展
        /// </summary>
        private void ReadStringExtend()
        {
            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            if (dic == null || dic.Count <= 0) return;

            // 遍历成员，读取数据
            foreach (DataFieldItem item in dic.Values)
            {
                if (item.Attribute.RefType != typeof(String) || hasReadExtend.Contains(item.Property.Name)) continue;
                hasReadExtend.Add(item.Property.Name);

                ReadExtendProperty(Reader, item);
            }
        }
        #endregion

        #region 索引器
        public virtual Object this[String name]
        {
            get
            {
                TryReadExtend(name);

                if (Extends != null && Extends.ContainsKey(name)) return Extends[name];
                //{
                //    Object obj = Extends[name];
                //    if (obj == null || !obj.GetType().IsArray) return obj;
                //    Array arr = obj as Array;
                //    if (arr == null || arr.Length <= 0) return obj;

                //    TEntity[] arr2 = new TEntity[arr.Length];
                //    for (int i = 0; i < arr.Length; i++)
                //    {
                //        arr2[i] = arr.GetValue(i) as TEntity;
                //    }

                //    return arr2;
                //}

                DataFieldItem field = GetField(name);
                //if (field == null) throw new Exception("未找到属性" + name + "！");
                if (field == null) return null;
                return field.Property.GetValue(this, null);
            }
            set
            {
                TryReadExtend(name);

                DataFieldItem field = GetField(name);
                //if (field == null) throw new Exception("未找到属性" + name + "！");
                if (field == null) return;
                field.Property.SetValue(this, value, null);
            }
        }

        protected T[] GetExtendList<T>(String name) where T : EntityBase<T>, new()
        {
            TryReadExtend(name);

            if (Extends == null || Extends.Count <= 0 || !Extends.ContainsKey(name)) return null;

            Array arr = Extends[name] as Array;
            if (arr == null || arr.Length <= 0) return null;

            T[] arr2 = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr2[i] = arr.GetValue(i) as T;
            }

            return arr2;
        }
        #endregion

        #region 辅助函数
        static Dictionary<String, DataFieldItem> GetFields(Type type)
        {
            return DataFieldItem.GetFields(type);
        }

        static DataFieldItem GetField(String name)
        {
            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = DataFieldItem.GetFields(typeof(TEntity));
            if (dic == null || dic.Count <= 0 || !dic.ContainsKey(name)) return null;

            return dic[name];
        }

        protected virtual long GetRealAddress(long address, RefKinds kind)
        {
            switch (kind)
            {
                case RefKinds.Auto:
                    if (address >= Info.ImageBase)
                        return address - Info.ImageBase;
                    else
                        return Address + address;
                case RefKinds.Virtual:
                    //if (Info.ImageBase <= 0) throw new Exception("非法镜像基址！");
                    //if (address < Info.ImageBase) throw new Exception(String.Format("非法地址 0x{0:X}", address));
                    if (address < Info.ImageBase) return 0;
                    return address - Info.ImageBase;
                case RefKinds.Relative:
                    return Address + address;
                case RefKinds.Absolute:
                    return address;
                default:
                    break;
            }

            return address;
        }

        protected static long Seek(BinaryReader reader, long offset)
        {
            return reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }

        protected static String ReadString(BinaryReader reader)
        {
            List<Byte> list = new List<byte>();
            while (true)
            {
                Byte b = reader.ReadByte();
                if (b == 0) break;

                list.Add(b);
            }

            if (list.Count <= 0) return null;

            return Encoding.UTF8.GetString(list.ToArray());
        }
        #endregion

        #region 显示
        public override void Show(Boolean isShowExtend)
        {
            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            if (dic == null || dic.Count <= 0) return;

            WriteLine("{0} (0x{1:X}, 0x{2:X}):", this.GetType().Name, Info.ImageBase + Address, ObjectSize);

            // 遍历成员，计算最长成员，便于格式化输出
            Int32 maxlen = 0;
            foreach (String item in dic.Keys)
            {
                if (item.Length > maxlen) maxlen = item.Length;
            }

            // 如果显示扩展，先读一次，把字符串读出来
            //if (isShowExtend) ReadExtend();
            if (isShowExtend) ReadStringExtend();

            #region 遍历成员，读取数据
            String format = "{0," + maxlen + "}:{1}";
            String format2 = "{0," + maxlen + "}:0x{1:X}";
            String format3 = "{0," + maxlen + "}:{1:X}h";
            foreach (DataFieldItem item in dic.Values)
            {
                PropertyInfo property = item.Property;

                if (item.Attribute.RefType != null)
                {
                    if (item.Attribute.RefType == typeof(String) && Extends != null && Extends.ContainsKey(property.Name))
                        WriteLine("{0," + maxlen + "}:({1:X}h){2}", property.Name, property.GetValue(this, null), Extends[property.Name]);
                    else
                        WriteLine(format2, property.Name, property.GetValue(this, null));
                }
                else if (property.PropertyType == typeof(Int16))
                {
                    Int16 v = (Int16)property.GetValue(this, null);
                    if (v == 0)
                        WriteLine(format, property.Name, v);
                    else
                        WriteLine(format3, property.Name, v);
                }
                else if (property.PropertyType == typeof(Int32))
                {
                    Int32 v = (Int32)property.GetValue(this, null);
                    if (v == 0)
                        WriteLine(format, property.Name, v);
                    else
                        WriteLine(format3, property.Name, v);
                }
                else if (property.PropertyType == typeof(Byte[]))
                {
                    Byte[] bts = (Byte[])property.GetValue(this, null);
                    if (item.Attribute.Size == 16)
                        WriteLine(format, property.Name, new Guid(bts));
                    else
                        WriteLine(format, property.Name, BitConverter.ToString(bts));
                }
                else
                    WriteLine(format, property.Name, property.GetValue(this, null));
            }
            #endregion

            #region 显示扩展
            if (!isShowExtend) return;
            ShowExtend(isShowExtend);
            #endregion
        }

        public override void ShowExtend(bool isShowExtend)
        {
            ReadExtend();

            if (Extends == null || Extends.Count <= 0) return;

            // 取得成员列表
            Dictionary<String, DataFieldItem> dic = GetFields(this.GetType());
            if (dic == null || dic.Count <= 0) return;

            foreach (String item in Extends.Keys)
            {
                // 不显示父级引用
                if (dic.ContainsKey(item) && dic[item].Attribute.IsParent) continue;

                ShowExtendProperty(item, isShowExtend);
            }
        }

        protected virtual void ShowExtendProperty(String name, bool isShowExtend)
        {
            Object obj = Extends[name];
            if (obj == null) return;

            Type type = obj.GetType();
            if (obj is EntityBase2)
            {
                WriteLine(String.Empty);

                (obj as EntityBase2).Show(isShowExtend);
            }
            else if (type.IsArray)
            {
                Array arr = obj as Array;
                if (arr != null)
                {
                    Int32 n = 1;
                    foreach (Object elm in arr)
                    {
                        WriteLine(String.Empty);
                        WriteLine("{0} {1}/{2}", elm.GetType().Name, n++, arr.Length);

                        (elm as EntityBase2).Show(isShowExtend);
                    }
                }
            }
        }
        #endregion

        #region 输出
        protected static void WriteLine()
        {
            WriteLine(String.Empty);
        }

        protected static void WriteLine(String msg)
        {
            Console.WriteLine(msg);
        }

        protected static void WriteLine(String format, params Object[] args)
        {
            WriteLine(String.Format(format, args));
        }
        #endregion
    }
}
