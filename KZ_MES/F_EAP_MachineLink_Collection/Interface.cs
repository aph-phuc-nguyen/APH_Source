using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EAP_MachineLink_Collection
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_MachineLink_Collection_M frm = new F_EAP_MachineLink_Collection_M();
                FormCollection collection = Application.OpenForms;
                frm.Owner = collection["frmMain"];
                frm.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public static void RunpCutRealParam(Object obj, Object arg0)
        {
            try
            {
                Program.client = obj as SJeMES_Framework.Class.ClientClass;
                F_EAP_CUT_INFO frm = new F_EAP_CUT_INFO(arg0.ToString());
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
