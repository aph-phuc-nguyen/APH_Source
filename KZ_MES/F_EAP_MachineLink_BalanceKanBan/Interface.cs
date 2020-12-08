using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EAP_MachineLink_BalanceKanBan
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_Balance_Kanban frm = new F_EAP_Balance_Kanban();
                FormCollection collection = Application.OpenForms;
                frm.Owner = collection["frmMain"];
                frm.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public static void RunBalanceKanban(Object obj, Object arg0, Object arg1)
        {
            try
            {
                Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_Balance_Kanban frm = new F_EAP_Balance_Kanban(arg0.ToString());
                //if (Screen.AllScreens.Count() != 1)
                //{
                //    for (int i = 0; i < Screen.AllScreens.Count(); i++)
                //    {
                //        Screen s = Screen.AllScreens[i];
                //        if (!s.Primary)
                //        {
                //            frm.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
                //            frm.Size = new System.Drawing.Size(Screen.AllScreens[i].Bounds.Width, Screen.AllScreens[i].Bounds.Height);
                //        }
                //    }
                //}
                frm.StartPosition = FormStartPosition.CenterScreen;
                //frm.TopMost = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RunOvenRealParam(Object obj, Object arg0, Object arg1)
        {
            try
            {
                Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_Oven_RealParam frm = new F_EAP_Oven_RealParam(arg0.ToString());
                frm.StartPosition = FormStartPosition.CenterScreen;
                //frm.TopMost = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RunFreezerRealParam(Object obj, Object arg0, Object arg1)
        {
            try
            {
                Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_Freezer_RealParam frm = new F_EAP_Freezer_RealParam(arg0.ToString());
                frm.StartPosition = FormStartPosition.CenterScreen;
                //frm.TopMost = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RunpRressRealParam(Object obj, Object arg0, Object arg1)
        {
            try
            {
                Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_Press_RealParam frm = new F_EAP_Press_RealParam(arg0.ToString());
                frm.StartPosition = FormStartPosition.CenterScreen;
                //frm.TopMost = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
