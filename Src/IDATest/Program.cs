using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IDACSharp;

namespace IDATest
{
    public class CPlugin : IPlugin
    {
        public static int StartPlugin()
        {
            //Function func=new Function(
            try
            {
                FrmMain f = new FrmMain();
                //f.ShowDialog(new IDAHWND(hwnd));
                //f.ShowDialog();
                f.Show();
                return 0;
            }
            catch (Exception ex)
            {
                //XTrace.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
                return -1;
            }
        }


        #region IPlugin 成员

        public bool Init()
        {
            return true;
        }

        public string Name
        {
            get { return "IDA测试"; }
            set { }
        }

        //FrmMain frm;

        public void Start()
        {
            FrmMain frm = null;

            if (frm == null)
            {
                frm = new FrmMain();
                frm.Show();
            }
            else
                frm.BringToFront();
        }

        public void Term()
        {
        }

        #endregion
    }
}
