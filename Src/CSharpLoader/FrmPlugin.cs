using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IDACSharp;

namespace CSharpLoader
{
    public partial class FrmPlugin : Form
    {
        public FrmPlugin()
        {
            InitializeComponent();
        }

        private void FrmPlugin_Load(object sender, EventArgs e)
        {
            ShowPlugins();
        }

        void ShowPlugins()
        {
            listView1.Items.Clear();

            if (Loader.Plugins == null || Loader.Plugins.Count <= 0) return;

            foreach (IPlugin item in Loader.Plugins)
            {
                ListViewItem lv = listView1.Items.Add(item.Name);
                lv.SubItems.Add(item.GetType().Assembly.Location);
                lv.Tag = item;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            IPlugin plugin = GetSelected();
            if (plugin == null) return;

            plugin.Start();
        }

        IPlugin GetSelected()
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count <= 0) return null;
            ListViewItem lv = listView1.SelectedItems[0];
            if (lv == null || lv.Tag == null) return null;

            return (IPlugin)lv.Tag;
        }
    }
}
