using System;
using System.Collections.Generic;
using System.Text;
using IDACSharp;
using VBKiller.Entity;

namespace VBKiller
{
    /// <summary>
    /// VB 结构体
    /// </summary>
    class VBStruct
    {
        /// <summary>
        /// 创建并标识结构体
        /// </summary>
        /// <typeparam name="TEntity">结构体实体类型</typeparam>
        /// <param name="entity">结构体数据实体</param>
        /// <param name="address">结构体基地址，引用类型的成员可能需要该地址作为相对地址</param>
        /// <param name="canPostfix">当名称已存在时，是否允许使用后缀</param>
        /// <returns></returns>
        public static Struct Make<TEntity>(TEntity entity, UInt32 address, Boolean canPostfix) where TEntity : EntityBase<TEntity>, new()
        {
            Int32 addr = (Int32)(entity.Address + entity.Info.ImageBase);

            Struct st = VBStruct.Create<TEntity>(entity, address, canPostfix);
            if (st == null) throw new Exception(String.Format("为类型{0}创建结构体失败！", typeof(TEntity)));

            Bytes.MakeNameAnyway((UInt32)addr, typeof(TEntity).Name);

            //KernelWin.WriteLine("MakeStruct: 0x{0:X8} {1:X}h {2}", addr, (Int32)EntityBase<TEntity>.ObjectSize, st.Name);
            //Bytes.MakeStruct(addr, (Int32)EntityBase<TEntity>.ObjectSize, st.Name);
            MakeStruct<TEntity>(addr, st);

            // 处理结构体成员中的字符串
            Dictionary<String, DataFieldItem> dic = DataFieldItem.GetFields(typeof(TEntity));
            foreach (DataFieldItem item in dic.Values)
            {
                if (item.Attribute.RefType != typeof(String)) continue;

                // 先直接取地址
                Int32 temp = Convert.ToInt32(item.Property.GetValue(entity, null));
                if (temp <= 0) continue;

                UInt32 address2 = (UInt32)temp;
                switch (item.Attribute.RefKind)
                {
                    case RefKinds.Virtual:
                        break;
                    case RefKinds.Relative:
                        // 相对，加上前面的结构体基地址
                        address2 += (UInt32)entity.Address;
                        break;
                    case RefKinds.Auto:
                        // 如果小于基址，可能是相对地址
                        if (address2 < entity.Info.ImageBase && address2 > 0)
                            address2 += (UInt32)entity.Address;
                        break;
                    case RefKinds.Absolute:
                        throw new Exception("不支持的类型：" + item.Attribute.RefKind);
                }

                if (address2 <= 0) continue;

                // 标为字符串
                //Bytes.MakeUnknown(address2, 0);
                Bytes.MakeAscii(address2, (Int32)Bytes.BadAddress, StringType.C);
            }

            return st;
        }

        /// <summary>
        /// 创建结构体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="address"></param>
        /// <param name="canPostfix"></param>
        /// <returns></returns>
        public static Struct Create<TEntity>(TEntity entity, UInt32 address, Boolean canPostfix) where TEntity : EntityBase<TEntity>, new()
        {
            Type type = typeof(TEntity);

            // 类名作为结构体名字
            String name = type.Name;

            // 检查是否已存在
            //UInt32 id = Struct.GetStructID(name);
            //if (id != Bytes.BadAddress)
            //{
            //    KernelWin.WriteLine("结构体{0}(ID={1:X}h)已存在！", name, id);
            //    return Struct.FindStructByID(id);
            //}

            Struct st = Struct.FindStructByName(name);
            //if (st != null)
            //{
            //    KernelWin.WriteLine("结构体{0}(ID={1:X}h)已存在！", name, st.ID);
            //    return st;
            //}

            Int32 n = 0;
            String name2 = name;
            while (st == null)
            {
                st = Struct.Create(name2, Bytes.BadAddress, false);

                n++;
                if (st == null)
                {
                    if (n > 10 || !canPostfix) throw new Exception("创建结构体失败！");
                    name2 = name + "_" + n;
                }
            }

            Dictionary<String, DataFieldItem> dic = DataFieldItem.GetFields(type);
            foreach (DataFieldItem item in dic.Values)
            {
                name = item.Property.Name;
                DataType dt;
                UInt32 size = 0;

                if (item.Attribute.RefType == null)
                {
                    #region 普通成员
                    if (item.Property.PropertyType == typeof(String))
                    {
                        dt = DataType.ASCI;
                        if (!String.IsNullOrEmpty(item.Attribute.SizeField)) size = Convert.ToUInt32(entity[item.Attribute.SizeField]);
                        if (size <= 0) size = (UInt32)item.Attribute.Size;
                    }
                    else if (item.Property.PropertyType == typeof(Byte[]))
                    {
                        // 字节数组也可以当作字符串一样处理
                        dt = DataType.ASCI;
                        if (!String.IsNullOrEmpty(item.Attribute.SizeField)) size = Convert.ToUInt32(entity[item.Attribute.SizeField]);
                        if (size <= 0) size = (UInt32)item.Attribute.Size;
                    }
                    else if (item.Property.PropertyType == typeof(Byte))
                    {
                        dt = DataType.BYTE;
                        size = 1;
                    }
                    else if (item.Property.PropertyType == typeof(Int16))
                    {
                        dt = DataType.WORD;
                        size = 2;
                    }
                    else if (item.Property.PropertyType == typeof(Int32))
                    {
                        dt = DataType.DWRD;
                        size = 4;
                    }
                    else
                        throw new Exception("不支持的类型：" + type.Name);

                    //KernelWin.WriteLine("创建普通成员：{0} {1} {2} ", name, dt, size);
                    st.Add(name, dt, size);

                    #endregion
                }
                else
                {
                    #region 引用成员
                    if (item.Property.PropertyType == typeof(Int16))
                    {
                        dt = DataType.WORD | DataType.F0OFF;
                        size = 2;
                    }
                    else if (item.Property.PropertyType == typeof(Int32))
                    {
                        dt = DataType.DWRD | DataType.F0OFF;
                        size = 4;
                    }
                    else
                        throw new Exception("不支持的类型：" + type.Name);

                    UInt32 addr = 0;
                    switch (item.Attribute.RefKind)
                    {
                        case RefKinds.Virtual:
                            addr = 0;
                            break;
                        case RefKinds.Relative:
                            addr = address;
                            break;
                        case RefKinds.Auto:
                            // 先直接取地址
                            addr = Convert.ToUInt32(item.Property.GetValue(entity, null));
                            // 如果小于基址，可能是相对地址
                            if (addr < entity.Info.ImageBase && addr > 0)
                                addr = address;
                            else
                                addr = 0;
                            break;
                        case RefKinds.Absolute:
                            throw new Exception("不支持的类型：" + item.Attribute.RefKind);
                    }

                    //KernelWin.WriteLine("创建引用成员：{0} {1} 0x{2:X} {3}", name, dt, addr, size);
                    st.Add(name, Bytes.BadAddress, dt, addr, size);

                    #endregion
                }
            }

            return st;
        }

        public static Boolean MakeStruct<TEntity>(Int32 address, Struct st) where TEntity : EntityBase<TEntity>, new()
        {
            //KernelWin.WriteLine("MakeStruct: 0x{0:X8} {1:X}h {2} ID={3}", address, (Int32)EntityBase<TEntity>.ObjectSize, st.Name, st.ID);
            Bytes.MakeUnknown((UInt32)address, (UInt32)EntityBase<TEntity>.ObjectSize, 0);
            Boolean ret = Bytes.MakeStruct(address, EntityBase<TEntity>.ObjectSize, (UInt32)st.ID);
            if (!ret) KernelWin.WriteLine("为 地址0x{0:X} 类型{1} 创建结构体 {2} 失败！", address, typeof(TEntity), st.Name);
            return ret;
        }
    }
}
