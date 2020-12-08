using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecondEfficiency_Kanban
{
    public class Interface
    {

		public static string MyArgs;
		public static void RunApp(Object obj)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				SecondEfficiencyForm frm = new SecondEfficiencyForm();
				if (Screen.AllScreens.Count() != 1)
				{
					for (int i = 0; i < Screen.AllScreens.Count(); i++)
					{
						Screen s = Screen.AllScreens[i];
						if (!s.Primary)
						{
							frm.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
							frm.Size = new System.Drawing.Size(Screen.AllScreens[i].Bounds.Width, Screen.AllScreens[i].Bounds.Height);
						}
					}
				}
				frm.StartPosition = FormStartPosition.CenterScreen;
				frm.TopMost = true;
				frm.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public static void RunCustomize(Object obj, Object arg)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				MyArgs = arg as string;
				SecondEfficiencyForm frm = new SecondEfficiencyForm();
				if (Screen.AllScreens.Count() != 1)
				{
					for (int i = 0; i < Screen.AllScreens.Count(); i++)
					{
						Screen s = Screen.AllScreens[i];
						if (!s.Primary)
						{
							frm.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
							frm.Size = new System.Drawing.Size(Screen.AllScreens[i].Bounds.Width, Screen.AllScreens[i].Bounds.Height);
						}
					}
				}
				frm.StartPosition = FormStartPosition.CenterScreen;
				frm.TopMost = true;
				frm.ShowDialog();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
