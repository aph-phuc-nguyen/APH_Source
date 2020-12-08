﻿using ProductionDashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionDashBoard
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.Client = obj as SJeMES_Framework.Class.ClientClass;
				DashBoardForm frm = new DashBoardForm();
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
				FormCollection collection = Application.OpenForms;
				frm.Owner = collection["frmMain"];
				frm.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
