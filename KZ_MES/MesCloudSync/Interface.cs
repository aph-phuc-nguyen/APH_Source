﻿using MesCloudSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesCloudSync
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.client = obj as SJeMES_Framework.Class.ClientClass;
                MesCloudSyncForm frm = new MesCloudSyncForm();
				//if (Screen.AllScreens.Count() != 1)
				//{
				//	for (int i = 0; i < Screen.AllScreens.Count(); i++)
				//	{
				//		Screen s = Screen.AllScreens[i];
				//		if (!s.Primary)
				//		{
				//			frm.Location = new System.Drawing.Point(s.Bounds.X, s.Bounds.Y);
				//			frm.Size = new System.Drawing.Size(Screen.AllScreens[i].Bounds.Width, Screen.AllScreens[i].Bounds.Height);
				//		}
				//	}
				//}
				//frm.StartPosition = FormStartPosition.CenterScreen;
				//frm.TopMost = true;
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
