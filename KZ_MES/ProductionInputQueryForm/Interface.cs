﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionInputQuery
{
	public class Interface
	{
		public static void RunApp(Object obj)
		{
			try
			{
				Program.client = obj as SJeMES_Framework.Class.ClientClass;
				ProductionInputQueryForm frm = new ProductionInputQueryForm();
				FormCollection collection = Application.OpenForms;
				frm.Owner = collection["frmMain"];
				frm.Show();
			}
			catch (Exception ex)
			{
				//SJeMES_Control_Library.MessageHelper.ShowErr(, ex.Message);
				throw ex;
			}
		}
	}
}
