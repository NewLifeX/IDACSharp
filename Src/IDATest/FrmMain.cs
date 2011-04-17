using System;
using System.Windows.Forms;
using IDACSharp;
using System.Collections.Generic;

namespace IDATest
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label2.Text = TotalFuncs().ToString();
            Int32 count = Function.TotalCount();
            label2.Text = count.ToString();

            unsafe
            {
                listView1.Items.Clear();
                if (count > 30) count = 30;
                for (int i = 0; i < count; i++)
                {
                    Function function = Function.GetItem(i);
                    String name = function.Name;
                    ListViewItem lv = listView1.Items.Add(name);
                    lv.SubItems.Add("0x" + function.Start.ToString("x8"));
                    lv.SubItems.Add("0x" + function.End.ToString("x8"));
                    lv.SubItems.Add("0x" + function.Size.ToString("x8"));
                    lv.SubItems.Add(function.Name);
                    lv.Tag = i;
                }
            }
        }

        Function GetSelected()
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count <= 0) return null;
            ListViewItem lv = listView1.SelectedItems[0];
            if (lv == null || lv.Tag == null) return null;

            Int32 id = (Int32)lv.Tag;
            return Function.GetItem(id);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Function function = GetSelected();
            if (function == null) return;
            propertyGrid1.SelectedObject = function;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Function function = GetSelected();
            if (function == null) return;
            KernelWin.Jump(function.Start);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<IDCFunction> list = IDCFunction.FindAll();
            if (list == null || list.Count <= 0) return;

            foreach (IDCFunction item in list)
            {
                String args = "";
                if (item.Args != null && item.Args.Count > 0)
                {
                    foreach (IDCValueTypes elm in item.Args)
                    {
                        args += " " + elm;
                    }
                }
                KernelWin.Msg("{0} {1} Flags={2}\n", item.Name, args, item.Flag);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            if (String.IsNullOrEmpty(openFileDialog1.FileName)) return;

            String file = openFileDialog1.FileName;

            try
            {
                if (IDCFunction.DoSysFile(file))
                    MessageBox.Show("执行成功！");
                else
                    MessageBox.Show("执行失败！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败！" + ex.Message);
            }
        }
    }
}
