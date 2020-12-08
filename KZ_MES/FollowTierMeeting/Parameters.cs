using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FollowTierMeeting
{
    class Parameters
    {
        public static string dateFormat = "yyyy/MM/dd";
        public enum DeptForm : int
        {
            All = 0,
            Line = 1
        }
        public enum QueryDeptType : int
        {
            All = 0,
            Plant = 1,
            Section = 2,
            Line = 3
        }
        public static void LoadDataGridView(DataTable indt, DataGridView dgv)
        {
            dgv.DataSource = indt;           
        }
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
        public static void clearDatagridview(DataGridView dgv)
        {
            DataTable dt = (DataTable)dgv.DataSource;
            if (dt != null)
                dt.Clear();
        }
        public static string TextToCheckbox(CheckBox cbxName, string val,bool isnull=false)
        {
            if (isnull == false)
            {
                if (val.Equals("Y"))
                {
                    cbxName.Size = new Size(50, 50);
                    cbxName.Checked = true;
                    cbxName.BackColor = Color.Green;
                }
                else
                {
                    cbxName.Checked = false;
                    cbxName.BackColor = Color.Red;
                }
            }else
            {
                cbxName.Checked = false;
                cbxName.BackColor = Color.White;
            }
            return "";
        }       
        //Following function will return Distinct records for Name, City and State column.
        public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            try
            {
                DataTable dtUniqRecords = new DataTable();
                dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
                return dtUniqRecords;
            }
            catch { return null; }
        }
        public static object FindText(DataTable indt, string fillter,string sort="code asc")
        {
            var result = indt.Select(fillter,sort);
            try
            {
                if (result != null)
                    return result;
            }
            catch { }
            return null;
        }
        public static DataTable Pivot(DataTable dtJson,string rowvalue,string colvalue)
        {
            try
            {
                string[] uniqueUnits = dtJson.AsEnumerable().Select(x => x.Field<string>(colvalue)).Distinct().ToArray();

                DataTable dt1 = new DataTable();
                dt1.Columns.Add(rowvalue, typeof(string));
                //foreach (string unit in uniqueUnits)
                //{
                //    if(unit!="")
                //     dt1.Columns.Add(unit, typeof(string));
                //}
                for (int i = 1; i <= 31; i++)
                {
                    dt1.Columns.Add(string.Format("{0:00}", i), typeof(string));
                }
                var groups = dtJson.AsEnumerable().GroupBy(x => x.Field<string>(rowvalue));

                foreach (var group in groups)
                {
                    DataRow newRow = dt1.Rows.Add();
                    foreach (DataRow row in group)
                    {
                        newRow[rowvalue] = group.Key;
                        if (row.Field<string>(colvalue) != "")
                            newRow[row.Field<string>(colvalue)] = "Yes";// row.Field<string>(colvalue);
                    }
                }
                return dt1;
            }
            catch { return null; }
        }
    }
}
