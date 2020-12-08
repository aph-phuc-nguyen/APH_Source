using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_MMS_Outsourcing_TrackInfo
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				F_MMS_Outsourcing_TrackInfo frm = new F_MMS_Outsourcing_TrackInfo();
				FormCollection collection = Application.OpenForms;
				frm.Owner = collection["frmMain"];
				frm.StartPosition = FormStartPosition.CenterScreen;
				frm.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
