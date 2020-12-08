﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_EAP_CUT_MAIN
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.client = obj as SJeMES_Framework.Class.ClientClass;
                MainForm frm = new MainForm();
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
