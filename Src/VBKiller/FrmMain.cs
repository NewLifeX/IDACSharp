using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VBKiller.Entity;
using IDACSharp;
using System.IO;
using System.Threading;

namespace VBKiller
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void 创建VB头结构体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VBInfo.Current.ReadBody(VBInfo.Current.Reader);

            KernelWin.WriteLine("分析完成！");
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            打开ToolStripMenuItem.Visible = !IsIDA;
            创建VB头结构体ToolStripMenuItem.Visible = IsIDA;

            if (IsIDA) LoadVBInfo(VBInfo.Current);
        }

        public void LoadVBInfo(VBInfo info)
        {
            treeView1.Nodes.Clear();

            TreeNodeCollection rootNodes = treeView1.Nodes;
            TreeNodeCollection nodes = rootNodes;
            TreeNode node = null;

            VBHeader vbheader = info.HeaderInfo;

            node = rootNodes.Add(typeof(VBHeader).Name);
            node.Tag = vbheader;

            node = rootNodes.Add(typeof(ProjectInfo).Name);
            node.Tag = vbheader.ProjectInfo2;

            if (vbheader.ProjectInfo2.ObjectTable2 != null)
            {
                node = rootNodes.Add(typeof(ObjectTable).Name);
                ObjectTable entity = vbheader.ProjectInfo2.ObjectTable2;
                node.Tag = entity;

                if (entity.ProjectInfo22 != null)
                {
                    node = rootNodes.Add(typeof(ProjectInfo2).Name);
                    node.Tag = entity.ProjectInfo22;
                }

                if (entity.Objects != null && entity.Objects.Length > 0)
                {
                    node = rootNodes.Add("对象");
                    nodes = node.Nodes;

                    foreach (PublicObjectDescriptor item in entity.Objects)
                    {
                        node = nodes.Add(item.Name);
                        node.Tag = item;

                        TreeNode node2 = null;

                        if (item.ObjectInfo2 != null)
                        {
                            node2 = node.Nodes.Add(typeof(ObjectInfo).Name);
                            node2.Tag = item.ObjectInfo2;
                        }

                        if (item.OptionalObjectInfo != null)
                        {
                            node2 = node.Nodes.Add(typeof(OptionalObjectInfo).Name);
                            node2.Tag = item.OptionalObjectInfo;

                            TreeNode node3 = null;
                            if (item.OptionalObjectInfo.EventLinks != null && item.OptionalObjectInfo.EventLinks.Length > 0)
                            {
                                node2 = node.Nodes.Add("事件");

                                Int32 i = 1;
                                foreach (EventLink2 elm in item.OptionalObjectInfo.EventLinks)
                                {
                                    String name = String.Empty;
                                    if (item.ProcNames != null && item.ProcNames.Length > i - 1) name = item.Name + "_" + item.ProcNames[i - 1].FriendName;
                                    if (String.IsNullOrEmpty(name)) name = item.Name + "_" + i.ToString("X2");
                                    i++;

                                    node3 = node2.Nodes.Add(name);
                                    node3.Tag = elm;
                                }
                            }

                            if (item.OptionalObjectInfo.Controls != null && item.OptionalObjectInfo.Controls.Length > 0)
                            {
                                node2 = node.Nodes.Add("控件");

                                foreach (VBControl elm in item.OptionalObjectInfo.Controls)
                                {
                                    node3 = node2.Nodes.Add(elm.Name2);
                                    node3.Tag = elm;
                                }
                            }
                        }

                        //if (item.ProcNames != null && item.ProcNames.Length > 0)
                        //{
                        //    foreach (ProcName elm in item.ProcNames)
                        //    {
                        //        node2 = node.Nodes.Add(elm.Name);
                        //        node2.Tag = elm;
                        //    }
                        //}
                    }
                }
            }

            if (vbheader.ComRegisterData2 != null)
            {
                node = rootNodes.Add(typeof(ComRegData).Name);
                ComRegData entity = vbheader.ComRegisterData2;
                node.Tag = entity;

                if (entity.RegInfo2 != null && entity.RegInfo2.Length > 0)
                {
                    node = rootNodes.Add("COM注册");
                    nodes = node.Nodes;

                    foreach (ComRegInfo item in entity.RegInfo2)
                    {
                        node = nodes.Add(item.Name);
                        node.Tag = item;
                    }
                }
            }

            if (vbheader.ExternalComponentTables != null && vbheader.ExternalComponentTables.Length > 0)
            {
                node = rootNodes.Add("引用组件");
                nodes = node.Nodes;

                foreach (ExternalComponentTable item in vbheader.ExternalComponentTables)
                {
                    node = nodes.Add(item.Name2);
                    node.Tag = item;
                }
            }

            if (vbheader.GUITables != null && vbheader.GUITables.Length > 0)
            {
                node = rootNodes.Add("窗体");
                nodes = node.Nodes;

                foreach (GUITable item in vbheader.GUITables)
                {
                    node = nodes.Add(typeof(GUITable).Name);
                    node.Tag = item;
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null) propertyGrid1.SelectedObject = new CustomProperty(e.Node.Tag);
        }

        private Boolean _IsIDA;
        /// <summary>是否在IDA环境</summary>
        public Boolean IsIDA
        {
            get { return _IsIDA; }
            set { _IsIDA = value; }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (!IsIDA) return;

            if (treeView1.SelectedNode == null) return;

            EntityBase2 entity = treeView1.SelectedNode.Tag as EntityBase2;
            if (entity == null || entity.Address <= 0) return;

            long address = entity.Address + entity.Info.ImageBase;
            KernelWin.WriteLine("跳：0x{0:X}", address);
            if (address > 0) KernelWin.Jump((UInt32)address);
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            //BinaryReader reader = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read));
            Byte[] buffer = File.ReadAllBytes(openFileDialog1.FileName);
            BinaryReader reader = new BinaryReader(new MemoryStream(buffer));

            VBInfo info = VBInfo.Current;
            info.Reader = reader;
            info.ReadInfo(reader);

            reader.BaseStream.Seek(info.Header - info.ImageBase, SeekOrigin.Begin);

            VBHeader header = new VBHeader();
            header.Info = info;
            header.Read(reader);
            info.HeaderInfo = header;

            LoadVBInfo(info);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmAbout().ShowDialog(this);
        }

        private void 修正ASPDLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate(Object state)
            {
                try
                {
                    FixASPToDll();
                }
                catch (Exception ex)
                {
                    KernelWin.WriteLine(ex.ToString());
                }
            });
        }

        void FixASPToDll()
        {
            String str = @"AspToDllLog";

            KernelWin.WriteLine("正在查找{0}", str);

            UInt32 address = Search.FindTextDown(0, str);
            KernelWin.WriteLine("0x{0:X8}", address);

            if (address == Bytes.BadAddress)
            {
                str = @"\AspToDllLog.Log";
                KernelWin.WriteLine("正在查找{0}", str);
                address = Search.FindTextDown(0, str);
                KernelWin.WriteLine("0x{0:X8}", address);

                if (address == Bytes.BadAddress)
                {
                    KernelWin.WriteLine("无法找到！");
                    return;
                }
            }

            KernelWin.WriteLine("0x{0:X8}", address);
            return;

            // 找到第一个引用
            address = Ref.GetFirstDataRefFrom(address);
            KernelWin.WriteLine("0x{0:X8}", address);
            if (address == Bytes.BadAddress) return;

            KernelWin.WriteLine("0x{0:X8}", address);

            // 找到函数
            Function func = Function.FindByAddress(address);
            if (func == null) return;

            address = Ref.GetFirstDataRefFrom(func.Start);
            KernelWin.WriteLine("GetFirstDataRefFrom 0x{0:X8}", address);
            while (address != Bytes.BadAddress)
            {

                KernelWin.WriteLine("0x{0:X8}", address);
                // 开始处理
                Function fun = Function.FindByAddress(address);
                if (fun != null) KernelWin.WriteLine(fun.Name);

                // 下一个
                address = Ref.GetNextDataRefFrom(func.Start, address);
            }
        }
    }
}
