using  DayActualManually;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace  DayActualManually
{
    public partial class DayActualManuallyModifyForm : MaterialForm
    {
        string d_dept = "";
        string work_day = "";
        string work_qty = "";
        string note = "";
        string art_no="";
        public DayActualManuallyModifyForm()
        {
            InitializeComponent();           
        }

        public DayActualManuallyModifyForm(string dept, string workDate, string workQty, string strNote,string vart_no)
        {
            InitializeComponent();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.client, "", Program.client.Language);
            d_dept = dept;
            work_day = workDate;
            work_qty = workQty;
            note = strNote;
            art_no = vart_no;
        }

        private void DayActualManuallyModifyForm_Load(object sender, EventArgs e)
        {
            textDept.Text = d_dept;
            textWorkDay.Text = work_day;
            textWorkQty.Text = work_qty;
            textNote.Text = note;
            txtArt_no.Text = art_no;
            textWorkQty.Focus();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textWorkQty.Text.ToString().Trim() == "")
            {
                SJeMES_Control_Library.MessageHelper.ShowErr(this, Parameters.languageTranslation("Please fill in actual output!",this.Name));
                return;
            }
            string qty = textWorkQty.Text.ToString().Trim();
            //DataTable dt = QueryArtList();
            //if(FindText(dt, "art_no", txtArt_no.Text, "art_no")!="")
              UpdateWorkTarget(d_dept, work_day, qty, textNote.Text.ToString(),txtArt_no.Text);
            //else
            //{
            //    SJeMES_Control_Library.MessageHelper.ShowErr(this, Parameters.languageTranslation("Art No not match!",this.Name));
            //}
        }
        public static string FindText(DataTable indt, string findcolname, string findtext, string getcolname)
        {
            var result = indt.AsEnumerable().Where(myRow => myRow.Field<string>(findcolname).ToLower() == findtext.ToLower()).FirstOrDefault();
            try
            {
                if (result != null)
                    return result[getcolname].ToString();
            }
            catch { }
            return "";
        }
        private DataTable QueryArtList()
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("Art_No", "");
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "GetListArt_No", Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                return SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
            }
            else
            {
                return null;
            }
        }
        private void UpdateWorkTarget(string dept, string workDay, string qty, string note,string vart_no)
        {
            Dictionary<string, Object> d = new Dictionary<string, object>();
            d.Add("vDDept", dept);
            d.Add("vWorkDay", workDay);
            d.Add("vWorkQty", qty);
            d.Add("vNote", note);
            d.Add("vArtNo", vart_no);
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.client.APIURL, "KZ_MESAPI", "KZ_MESAPI.Controllers.DayActualManuallyServer", "UpdateWorkActual",
                            Program.client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string tips =Parameters.languageTranslation("Tips",this.Name);
                string msg = Parameters.languageTranslation("Successfully modified!", this.Name);
               // SJeMES_Control_Library.MessageHelper.ShowSuccess(this,msg);
                DialogResult result = MessageBox.Show(msg, tips);
                if (result == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                string tips = Parameters.languageTranslation("Error!",this.Name);
                string msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                MessageBox.Show(msg, tips);
            }
        }

        //控制当日目标只能输入0-9 和 退格
        private void textWorkQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
