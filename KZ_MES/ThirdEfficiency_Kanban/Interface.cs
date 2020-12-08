﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThirdEfficiency_Kanban
{
    public class Interface
    {
		public static string plant;
		public static string date;

		public static void RunApp(Object obj)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				ThirdEfficiencyForm frm = new ThirdEfficiencyForm();
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

		public static void RunCustomize(Object obj,Object arg0,Object arg1)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				plant= arg0 as string;
				date= arg1 as string;
				ThirdEfficiencyForm frm = new ThirdEfficiencyForm();
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
