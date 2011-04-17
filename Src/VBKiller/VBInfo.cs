using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IDACSharp;
using VBKiller.Entity;

namespace VBKiller
{
    /// <summary>
    /// VB信息
    /// </summary>
    public class VBInfo
    {
        #region 属性
        private Dictionary<long, Object> _Objects;
        /// <summary>对象集合</summary>
        public Dictionary<long, Object> Objects
        {
            get { return _Objects ?? (_Objects = new Dictionary<long, object>()); }
        }
        #endregion

        #region 构造
        //public VBInfo(BinaryReader reader)
        //{
        //    Reader = reader;
        //}

        //private VBInfo() { }

        private static VBInfo _Current;
        /// <summary>当前VB信息</summary>
        public static VBInfo Current
        {
            get { return _Current ?? (_Current = new VBInfo()); }
            //set { _Current = value; }
        }
        #endregion

        #region 读取器
        private BinaryReader _Reader;
        /// <summary>读取器</summary>
        public BinaryReader Reader
        {
            get { return _Reader; }
            set { _Reader = value; }
        }

        long Seek(BinaryReader reader, long offset)
        {
            //KernelWin.WriteLine("文件移位到 0x{0:X8}", offset);
            return reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        }
        #endregion

        #region 基本信息
        private UInt32 _ImageBase;
        /// <summary>镜像基址</summary>
        public UInt32 ImageBase
        {
            get { return _ImageBase; }
            set { _ImageBase = value; }
        }

        private UInt32 _PEEntry;
        /// <summary>PE入口</summary>
        public UInt32 PEEntry
        {
            get { return _PEEntry; }
            set { _PEEntry = value; }
        }

        private UInt32 _NextSegment;
        /// <summary>下一个段基址</summary>
        public UInt32 NextSegment
        {
            get { return _NextSegment; }
            set { _NextSegment = value; }
        }

        private UInt32 _Header;
        /// <summary>VB头地址</summary>
        public UInt32 Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        private UInt32 _VBSig;
        /// <summary>VB签名</summary>
        public UInt32 VBSig
        {
            get { return _VBSig; }
            set { _VBSig = value; }
        }

        private Int32 _PEoffset;
        /// <summary>PE位移</summary>
        public Int32 PEoffset
        {
            get { return _PEoffset; }
            set { _PEoffset = value; }
        }

        private VBHeader _HeaderInfo;
        /// <summary>VB头信息</summary>
        public VBHeader HeaderInfo
        {
            get { return _HeaderInfo; }
            set { _HeaderInfo = value; }
        }

        /// <summary>
        /// 读取基本信息
        /// </summary>
        public void ReadInfo(BinaryReader reader)
        {
            //Seek(reader, 0x3c);
            //PEoffset = reader.ReadInt32();

            //Seek(reader, PEoffset + 0x34);
            //ImageBase = reader.ReadUInt32();

            //Seek(reader, PEoffset + 0x28);
            //PEEntry = reader.ReadUInt32() + ImageBase;
            //KernelWin.WriteLine("PEEntry:0x{0:X}", PEEntry);

            //PEEntry = Entry.GetEntryPoint(Entry.GetEntryOrdinal(0));
            //KernelWin.WriteLine("EntryOrdinal:0x{0:X}", Entry.GetEntryOrdinal(0));
            //KernelWin.WriteLine("PEEntry:0x{0:X}", PEEntry);

            DosHeader dosHeader = new DosHeader();
            dosHeader.Read(reader);

            PEoffset = dosHeader.NewExeHeader;
            ImageBase = (UInt32)dosHeader.OptionalHeader.ImageBase;

            ExportDirectory export = dosHeader.OptionalHeader.Export;
            Int32 address = 0;
            if (export != null)
            {
                Seek(reader, export.AddressOfFunctions);
                address = reader.ReadInt32();
            }
            else
            {
                address = dosHeader.OptionalHeader.AddressOfEntryPoint;
            }
            PEEntry = (UInt32)address + ImageBase;

            Seek(reader, PEEntry - ImageBase);
            long temp = reader.ReadByte();
            if (temp == 0x68)
                temp = PEEntry + 1 - ImageBase;
            else if (temp == 0x58)
                temp = PEEntry + 2 - ImageBase;
            Seek(reader, temp);

            Header = reader.ReadUInt32();
            //VBSig = IDCFunction.EvalAndReturnLong("Dword(" + VBHeader + ")");
            //VBSig = Bytes.Dword(Header);
            if (Header - ImageBase > reader.BaseStream.Length) throw new Exception("非VB文件格式！");

            Seek(reader, Header - ImageBase);
            VBSig = reader.ReadUInt32();

            if (VBSig != 0x21354256)	//VB5
            {
                throw new Exception(String.Format("错误VB签名：0x{0:X}", VBSig));
            }

            //temp = IDCFunction.EvalAndReturnLong("Word(" + VBHeader + "+0x22)");
            //temp = Bytes.Word((UInt32)Header + 0x22);
            Seek(reader, Header + 0x22 - ImageBase);
            temp = reader.ReadInt16();
            if (temp < 0x0a)
            {
                throw new Exception("不是VB6程序！");
            }

            Seek(reader, Header - ImageBase);
            VBHeader header = new VBHeader();
            header.Info = this;
            header.Read(reader);

            HeaderInfo = header;
        }
        #endregion

        #region 导入表
        public void ReadImportTable(BinaryReader reader)
        {
            Seek(reader, PEoffset + 0xD8);

            UInt32 temp = reader.ReadUInt32() + reader.ReadUInt32() - 1 + ImageBase;

            Imports = new Dictionary<uint, string>();
            for (UInt32 ea = PEEntry - 6; ea <= PEEntry && ea > temp; ea -= 6)
            {
                if ((Bytes.Byte(ea) == 0xFF) && (Bytes.Byte(ea + 1) == 0x25))	//jmp Ds:xx_name
                {
                    //Bytes.MakeCode(ea);
                    //Bytes.MakeLabel(ea, ("j_" + Bytes.GetTrueName(Bytes.Dword(ea + 2))));

                    //KernelWin.WriteLine("MakeCode 0x{0:X}", ea);

                    String name = Bytes.GetTrueName(Bytes.Dword(ea + 2));
                    Imports.Add(ea, name);

                    KernelWin.WriteLine("MakeLabel 0x{0:X} {1}", ea, name);
                }
            }
        }

        private Dictionary<UInt32, String> _Imports;
        /// <summary>导入表</summary>
        public Dictionary<UInt32, String> Imports
        {
            get { return _Imports; }
            set { _Imports = value; }
        }
        #endregion

        #region VB主体
        public void ReadBody(BinaryReader reader)
        {
            ReadHeader(reader);

        }

        void ReadHeader(BinaryReader reader)
        {
            KernelWin.WriteLine("正在处理头部 {0}", typeof(VBHeader).Name);

            //Seek(reader, Header - ImageBase);

            VBHeader header = HeaderInfo;
            //header.Info = this;
            //header.Read(reader);

            //HeaderInfo = header;

            UInt32 address = Header;

            //if (!VBStruct.Make<VBHeader>(header)) throw new Exception("创建结构体失败！");
            VBStruct.Make<VBHeader>(header, address, true);

            ReadProjectInfo(header.ProjectInfo2);
            ReadComRegData(header.ComRegisterData2);
            ReadGUITable(header);
            ReadExternalComponentTable(header);
        }

        void ReadProjectInfo(ProjectInfo entity)
        {
            if (entity == null) return;

            KernelWin.WriteLine("正在处理工程信息 {0}", typeof(ProjectInfo).Name);

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<ProjectInfo>(entity, address, true);

            Bytes.MakeLabelAnyway((UInt32)entity.StartOfCode, "StartOfCode");
            Bytes.MakeLabelAnyway((UInt32)entity.EndOfCode, "EndOfCode");
            Bytes.MakeLabelAnyway((UInt32)entity.VBAExceptionHandler, "VBAExceptionHandler");
            Bytes.MakeLabelAnyway((UInt32)entity.NativeCode, "NativeCode");

            ReadExternalTable(entity);
            ReadObjectTable(entity.ObjectTable2);
        }

        void ReadComRegData(ComRegData entity)
        {
            if (entity == null) return;

            KernelWin.WriteLine("正在处理COM数据 {0}", typeof(ComRegData).Name);

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<ComRegData>(entity, address, true);

            if (entity.RegInfo2 == null || entity.RegInfo2.Length <= 0) return;

            foreach (ComRegInfo item in entity.RegInfo2)
            {
                KernelWin.WriteLine("COM组件 {0}", item.Name);

                Int32 addr = (Int32)(item.Address + ImageBase);

                VBStruct.Make<ComRegInfo>(item, address, true);

                Bytes.MakeNameAnyway((UInt32)addr, "Com_" + item.Name);
            }
        }

        void ReadGUITable(VBHeader header)
        {
            if (header == null || header.GUITables == null || header.GUITables.Length <= 0) return;

            KernelWin.WriteLine("正在处理界面 {0}", typeof(GUITable).Name);

            UInt32 address = (UInt32)header.GUITable;

            for (int i = 0; i < header.GUITables.Length; i++)
            {
                GUITable item = header.GUITables[i];

                String name = "GUITable_" + i.ToString("X2");
                //if(item.FormPointer2!=null&&item.FormPointer2.

                KernelWin.WriteLine("界面 {0}", name);

                UInt32 addr = (UInt32)(item.Address + ImageBase);
                VBStruct.Make<GUITable>(item, address, true);
                Bytes.MakeNameAnyway(addr, name);
            }
        }

        void ReadExternalComponentTable(VBHeader header)
        {
            if (header == null || header.ExternalComponentTables == null || header.ExternalComponentTables.Length <= 0) return;

            KernelWin.WriteLine("正在处理外部组件 {0}", typeof(ExternalComponentTable).Name);

            UInt32 address = (UInt32)header.ExternalComponentTable;

            foreach (ExternalComponentTable item in header.ExternalComponentTables)
            {
                KernelWin.WriteLine("外部组件 {0}", item.Name2);

                UInt32 addr = (UInt32)(item.Address + ImageBase);

                VBStruct.Make<ExternalComponentTable>(item, addr, true);

                Bytes.MakeNameAnyway(addr, "Ext_" + item.Name2);
            }
        }

        void ReadExternalTable(ProjectInfo entity)
        {
            if (entity == null || entity.ExternalTables == null || entity.ExternalTables.Length <= 0) return;

            KernelWin.WriteLine("正在处理 {0}", typeof(ExternalTable).Name);

            UInt32 address = (UInt32)entity.ExternalTable + ImageBase;

            foreach (ExternalTable item in entity.ExternalTables)
            {
                Int32 addr = (Int32)(item.Address + ImageBase);

                VBStruct.Make<ExternalTable>(item, address, true);

                Bytes.MakeNameAnyway((UInt32)addr, String.Format("{0}_{1}", item.ExternalLibrary2.LibraryName2, item.ExternalLibrary2.LibraryFunction2));
            }
            //for (int i = 1; i < entity.ExternalTables.Length; i++)
            //{
            //    Int32 addr = (Int32)(entity.ExternalTables[i].Address + ImageBase);

            //    VBStruct.Make<ExternalTable>(entity.ExternalTables[i], address, true);

            //    Bytes.MakeNameAnyway((UInt32)addr, "GUITable_" + entity.ExternalTables[i].ExternalLibrary2.LibraryName2);
            //}
        }

        void ReadObjectTable(ObjectTable entity)
        {
            if (entity == null) return;

            KernelWin.WriteLine("正在处理 {0}", typeof(ObjectTable).Name);

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<ObjectTable>(entity, address, true);

            ReadProjectInfo2(entity.ProjectInfo22);
            ReadPublicObjectDescriptor(entity);
        }

        void ReadProjectInfo2(ProjectInfo2 entity)
        {
            if (entity == null) return;

            KernelWin.WriteLine("正在处理 {0}", typeof(ProjectInfo2).Name);

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<ProjectInfo2>(entity, address, true);
        }

        void ReadPublicObjectDescriptor(ObjectTable entity)
        {
            if (entity == null || entity.Objects == null || entity.Objects.Length <= 0) return;

            KernelWin.WriteLine("正在处理 {0}", typeof(PublicObjectDescriptor).Name);

            UInt32 address = (UInt32)entity.Object + ImageBase;

            foreach (PublicObjectDescriptor item in entity.Objects)
            {
                KernelWin.WriteLine("对象 {0}", item.Name);

                Int32 addr = (Int32)(item.Address + ImageBase);

                VBStruct.Make<PublicObjectDescriptor>(item, address, true);

                Bytes.MakeNameAnyway((UInt32)addr, item.Name);

                //ReadPublicObjectDescriptor(item);

                ReadObjectInfo(item.ObjectInfo2, item);
                ReadOptionalObjectInfo(item.OptionalObjectInfo, item);
                ReadProcName(item);
            }
        }

        void ReadObjectInfo(ObjectInfo entity, PublicObjectDescriptor parent)
        {
            if (entity == null) return;

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<ObjectInfo>(entity, address, true);
            Bytes.MakeNameAnyway((UInt32)address, "Inf_" + parent.Name);

            if (entity.PrivateObject2 != null)
            {
                address = (UInt32)entity.PrivateObject2.Address + ImageBase;
                VBStruct.Make<PrivateObjectDescriptor>(entity.PrivateObject2, address, true);
                Bytes.MakeNameAnyway((UInt32)address, "FormList_" + parent.Name);
            }
        }

        void ReadOptionalObjectInfo(OptionalObjectInfo entity, PublicObjectDescriptor parent)
        {
            if (entity == null) return;

            UInt32 address = (UInt32)entity.Address + ImageBase;
            VBStruct.Make<OptionalObjectInfo>(entity, address, true);
            Bytes.MakeNameAnyway((UInt32)address, "OptInf_" + parent.Name);

            if (entity.Controls != null && entity.Controls.Length > 0)
            {
                //address = (UInt32)entity.Address + ImageBase;

                if (entity.Controls.Length == 1)
                {
                    address = (UInt32)entity.Controls[0].Address + ImageBase;
                    VBStruct.Make<VBControl>(entity.Controls[0], address, true);
                    Bytes.MakeNameAnyway((UInt32)address, "Control_" + parent.Name);
                }
                else
                {
                    foreach (VBControl item in entity.Controls)
                    {
                        address = (UInt32)item.Address + ImageBase;
                        VBStruct.Make<VBControl>(item, address, true);
                        Bytes.MakeNameAnyway((UInt32)address, "Control_" + parent.Name + "_" + item.Name2);
                    }
                }
            }

            if (entity.EventLinks != null && entity.EventLinks.Length > 0)
            {
                Int32 i = 1;
                foreach (EventLink2 item in entity.EventLinks)
                {
                    address = (UInt32)item.Address + ImageBase;
                    VBStruct.Make<EventLink2>(item, address, true);

                    // 事件列表命名
                    String name = String.Empty;
                    if (parent.ProcNames != null && parent.ProcNames.Length > i - 1) name = parent.Name + "_" + parent.ProcNames[i - 1].FriendName;
                    if (String.IsNullOrEmpty(name)) name = parent.Name + "_" + i.ToString("X2");
                    i++;
                    Bytes.MakeNameAnyway((UInt32)address, "Event_" + name);

                    // 跳转命名
                    address = (UInt32)item.Jump;
                    Bytes.MakeNameAnyway(address, "j" + name);
                    Bytes.MakeCode(address);

                    // 函数命名
                    if (Bytes.Byte(address) == 0xE9)
                    {
                        // Jump语句，下一个字就是函数起始地址
                        address = Bytes.Dword(address + 1) + address + 5;

                        Function func = Function.FindByAddress(address);
                        if (func == null)
                        {
                            // 如果函数不存在，则创建函数
                            Function.Add(address, Bytes.BadAddress);
                            func = Function.FindByAddress(address);
                        }
                        else
                        {
                            // 函数存在，但是函数的起始地址并不是当前行，表明这个函数分析有错，修改地址
                            if (func.Start != address)
                            {
                                //Function.Delete(func.Start);
                                //Function.Add(func.Start, address - 1);
                                func.End = address - 1;

                                Function.Add(address, Bytes.BadAddress);
                                func = Function.FindByAddress(address);
                            }
                        }

                        if (func == null)
                            KernelWin.WriteLine("0x{0:X} 创建函数失败！", address);
                        else
                        {
                            Bytes.MakeLabelAnyway(address, name);
                        }
                    }
                }
            }
        }

        void ReadProcName(PublicObjectDescriptor entity)
        {
            if (entity == null || entity.ProcNames == null || entity.ProcNames.Length <= 0) return;

            foreach (ProcName item in entity.ProcNames)
            {
                UInt32 addr = (UInt32)(item.Address + ImageBase);

                VBStruct.Make<ProcName>(item, addr, true);

                Bytes.MakeNameAnyway(addr, String.Format("{0}_{1}", entity.Name, item.FriendName));
            }
        }
        #endregion

        #region 测试
        public static void Test()
        {
            String filename = @"D:\CrackMe.exe";
            Byte[] buffer = File.ReadAllBytes(filename);
            BinaryReader reader = new BinaryReader(new MemoryStream(buffer));

            VBInfo.Current.ReadInfo(reader);

            //DosHeader dosHeader = new DosHeader();
            //dosHeader.Read(reader);
            //dosHeader.Show(true);
            //Console.WriteLine();

            //FileHeader fileHeader = new FileHeader();
            //fileHeader.Read(reader);
            //fileHeader.Show(false);
            //Console.WriteLine();

            //OptionalHeader optionalHeader = new OptionalHeader();
            //optionalHeader.Read(reader);
            //optionalHeader.Show(false);
            //Console.WriteLine();

            VBInfo info = VBInfo.Current;
            //info.ImageBase = 0x11000000;
            //info.Header = 0x110079A4;
            //info.ImageBase = 0x400000;
            //info.Header = 0x441944;
            info.ReadInfo(reader);

            reader.BaseStream.Seek(info.Header - info.ImageBase, SeekOrigin.Begin);

            VBHeader header = new VBHeader();
            header.Info = info;
            header.Read(reader);
            //header.ReadExtend();
            header.Show(true);

            //ComRegData regdata = header.ComRegisterData2;
            //regdata.ReadExtend();
            //Console.WriteLine();
            //Console.WriteLine("ComRegData:");
            //regdata.Show();

            //ComRegInfo reginfo = regdata.RegInfo2;
            //while (reginfo != null)
            //{
            //    reginfo.ReadExtend();
            //    Console.WriteLine();
            //    Console.WriteLine("ComRegInfo:");
            //    reginfo.Show();

            //    reginfo = reginfo.Next;
            //}

            //ProjectInfo pinfo = header.ProjectInfo2;
            ////pinfo.ReadExtend();
            //Console.WriteLine();
            //Console.WriteLine("ProjectInfo:");
            //pinfo.Show();
        }
        #endregion
    }
}
