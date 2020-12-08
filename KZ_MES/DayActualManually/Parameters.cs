using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayActualManually
{
  public static class Parameters
    {
        public static string dateFormat = "yyyy/MM/dd";
        public static string languageTranslation(string language, string headername = "")
        {
            if (headername != "")
                headername = headername.TrimEnd('_') + "_";
            string msg = SJeMES_Framework.Common.UIHelper.UImsg(headername + language, Program.client, "", Program.client.Language);
            if (headername != "")
                msg = msg.Replace(headername, string.Empty);
            return msg;
        }
        public static string GetSizeDataGridView(DataGridView dgv)
        {
            string size = "";
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].Visible == true)
                    size += dgv.Columns[i].Name + ":" + decimal.Round((decimal)dgv.Columns[i].Width / (decimal)dgv.Width, 2) + ";";
            }
            return size;
        }
        public static void CustomeSizeDataGridView(DataGridView dgv, string customsize)
        {
            // string customsize = "colrownum:0.04;colEmPlant:0.04;colEmSection:0.05;colEmLine:0.06;colEmCode:0.06;colEmName:0.17;colg_shorten_deptcode:0.05;colg_nameen:0.10;colg_namevn:0.10;colg_bonusrate:0.04;colg_position:0.07;colG_AVG_BONUS_DEPT:0.14;colG_CONTRACTDATE:0.09;colG_JOINDATE:0.13";
            string[] slipstr = customsize.Split(';');
            foreach (string item in slipstr)
            {
                if (item != "")
                {
                    string[] tmp = item.Split(':');
                    if (dgv.Columns.Contains(tmp[0]))
                        dgv.Columns[tmp[0]].Width = Convert.ToInt32(decimal.Round((decimal)(Screen.PrimaryScreen.Bounds.Width * 0.95) * decimal.Parse(tmp[1]), 0));
                }
            }
        }
    }
}
