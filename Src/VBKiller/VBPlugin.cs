using System;
using System.IO;
using IDACSharp;

namespace VBKiller
{
    public class VBPlugin : IPlugin
    {
        #region IPlugin 成员

        public bool Init()
        {
            try
            {
                //KernelWin.WriteLine("文件 {0}", FileName);

                //FileReader.BaseStream.Seek(0x3c, SeekOrigin.Begin);
                //Int32 n = FileReader.ReadInt32();

                //FileReader.BaseStream.Seek(n + 0x34, SeekOrigin.Begin);
                //n = FileReader.ReadInt32();

                //KernelWin.WriteLine("镜像基址 0x{0:x}", n);

                //Int32 PEentry = IDCFunction.EvalAndReturnLong("GetEntryPoint(GetEntryOrdinal(0))");

                VBInfo info = VBInfo.Current;
                info.Reader = FileReader;
                info.ReadInfo(FileReader);

                KernelWin.WriteLine("镜像基址：0x{0:X}", info.ImageBase);
                KernelWin.WriteLine("    入口：0x{0:X}", info.PEEntry);
                KernelWin.WriteLine("    VB头：0x{0:X}", info.Header);
                KernelWin.WriteLine("  VB签名：0x{0:X}", info.VBSig);

                //info.ReadImportTable(FileReader);
                //info.ReadBody(FileReader);
            }
            catch (Exception ex)
            {
                //KernelWin.Msg(ex.Message + Environment.NewLine);
                KernelWin.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public string Name
        {
            get { return "VB杀手"; }
            set { }
        }

        public void Start()
        {
            FrmMain frm = null;

            if (frm == null)
            {
                frm = new FrmMain();
                frm.IsIDA = true;
                frm.Show();
            }
            else
                frm.BringToFront();
        }

        public void Term()
        {
        }

        #endregion

        #region 属性
        private String _FileName;
        /// <summary>文件</summary>
        public String FileName
        {
            get
            {
                if (String.IsNullOrEmpty(_FileName)) _FileName = IdaInfo.GetInputFilePath();
                return _FileName;
            }
        }

        private BinaryReader _FileReader;
        /// <summary>文件读写器</summary>
        public BinaryReader FileReader
        {
            get
            {
                if (_FileReader == null)
                {
                    MemoryStream stream = new MemoryStream(File.ReadAllBytes(FileName));
                    _FileReader = new BinaryReader(stream);
                }
                return _FileReader;
            }
        }
        #endregion
    }
}
