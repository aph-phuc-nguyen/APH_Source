using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SJEMES_MAC_RPT
{
    /// <summary>
    /// 动态图表
    /// </summary>
   public static class Charts
    {

        private static string sql;
        /// <summary>
        /// 动态加载图表
        /// </summary>
        /// <param name="reportViewer"></param>
        /// <param name="tab">数据源</param>
        /// <param name="chartType">1：条形图 2：折线图 3：扇形图</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="strFiled">显示字段</param>
        public static void DynamicChart(ReportViewer reportViewer, DataTable tab,int chartType,string groupName,string strFiled)
        {

            try
            {
                reportViewer.Clear();
                reportViewer.BackColor = System.Drawing.Color.Black;
                var strFileds = strFiled.Split(',');
                string fields = string.Empty;//数据集字段
                string chartCategoryHierarchy = string.Empty;//分组字段
                string chartSeriesHierarchy = "<ChartMembers>";//y轴内容
                string chartSeriesCollection = string.Empty;//统计数据字符型
               
                foreach (var item in tab.Columns)//循环添加数据集字段
                {
                    fields +=
                            "<Field Name=\"" + item + "\">" +
                            "<DataField>" + item + "</DataField>" +
                            "<rd:TypeName>System.String</rd:TypeName>" +
                            "</Field>";
                    
                }

                chartCategoryHierarchy =
                    "<ChartMembers>" +
                    "<ChartMember>" +
                    "<Group Name=\"Chart1_CategoryGroup\">" +
                    "<GroupExpressions>" +
                    "<GroupExpression>=Fields!" + groupName + ".Value</GroupExpression>" +
                    "</GroupExpressions>" +
                    "</Group>" +
                    "<SortExpressions>" +
                    "<SortExpression>" +
                    "<Value>=Fields!" + groupName + ".Value</Value>" +
                    "</SortExpression>" +
                    "</SortExpressions>" +
                    "<Label>=Fields!" + groupName + ".Value</Label>" +
                    "</ChartMember>" +
                    "</ChartMembers>";

                foreach (var item in strFileds)
                {
                    chartSeriesHierarchy +=
                        "<ChartMember>" +
                        "<Label>" + item + "</Label>" +
                        "</ChartMember>";

               

                    chartSeriesCollection +=
                        "<ChartSeries Name=\"" + item + "\">" +
                        "<ChartDataPoints>" +
                        "<ChartDataPoint>" +
                        "<ChartDataPointValues>";
                    if (chartType == 3)
                    {
                       
                      chartSeriesCollection += "<Y>=Sum(Fields!" + item+".Value)/Sum(Fields!" + item + ".Value,\"DataSet_Chart\")</Y>" +
                     "</ChartDataPointValues>" +
                     "<ChartDataLabel><Style><FontSize>14pt</FontSize><FontWeight>Bold</FontWeight><Format>0.00%</Format><Color>Snow</Color></Style>" +
                     "<UseValueAsLabel>true</UseValueAsLabel>" +
                     "<Visible>true</Visible>" +
                     "</ChartDataLabel><Style />" +
                     "<ChartMarker><Style />" +
                     "</ChartMarker>" +
                     "<DataElementOutput>Output</DataElementOutput>" +
                     "</ChartDataPoint>" +
                     "</ChartDataPoints>";
                    }
                    else
                    {
                        chartSeriesCollection += "<Y>=Sum(Fields!" + item + ".Value)</Y>" +
                       "</ChartDataPointValues>" +
                       "<ChartDataLabel><Style><FontSize>12pt</FontSize><Color>White</Color></Style>" +
                       "<UseValueAsLabel>true</UseValueAsLabel>" +
                       "<Visible>true</Visible>" +
                       "</ChartDataLabel><Style />" +
                       "<ChartMarker><Style />" +
                       "</ChartMarker>" +
                       "<DataElementOutput>Output</DataElementOutput>" +
                       "</ChartDataPoint>" +
                       "</ChartDataPoints>";
                    }
                    if (chartType == 2)
                        chartSeriesCollection += "<Type>Line</Type>";
                    else if (chartType == 3)
                        chartSeriesCollection += "<Type>Shape</Type>";

                    chartSeriesCollection += "<Style /><ChartEmptyPoints><Style /><ChartMarker><Style />" +
                        "</ChartMarker>" +
                        "<ChartDataLabel><Style /></ChartDataLabel>" +
                        "</ChartEmptyPoints>" +
                        "<ValueAxisName>Primary</ValueAxisName>" +
                        "<CategoryAxisName>Primary</CategoryAxisName>" +
                        "<ChartSmartLabel>" +
                        "<CalloutLineColor>Black</CalloutLineColor>" +
                        "<MinMovingDistance>0pt</MinMovingDistance>" +
                        "</ChartSmartLabel>" +
                        "</ChartSeries>";

                }
                chartSeriesHierarchy += "</ChartMembers>";

                string xml_Charts = xml;

                xml_Charts = xml_Charts.Replace("@field", fields).
                    Replace("@ChartCategoryHierarchy", chartCategoryHierarchy).
                    Replace("@ChartSeriesHierarchy", chartSeriesHierarchy).
                    Replace("@ChartSeriesCollection", chartSeriesCollection);

                var doc = new XmlDocument();
                doc.LoadXml(xml_Charts);
                Stream stream = GetRdlcStream(doc);//序列化

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "DataSet_Chart";//数据集名称
                rds.Value = tab;//要和设计报表时指定的名称一致
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.LoadReportDefinition(stream);
                reportViewer.LocalReport.DataSources.Add(rds);
                ReportParameter[] ps1 = new ReportParameter[1];// ("条件", ReturnData[0]);

                ps1[0] = new ReportParameter("标题", groupName + "明细图");
                reportViewer.LocalReport.SetParameters(ps1);
                reportViewer.LocalReport.Refresh();
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {

                return;
            }
               
           
           
        }

        /// <summary>
        /// 加载下拉框数据
        /// </summary>
        /// <param name="cboGroupName">分组名称下拉框</param>
        /// <param name="cboPage">页签下拉框</param>
        /// <param name="txtFiled">显示字段文本框</param>
        /// <param name="ds">数据集</param>
        public static void LoadComboBoxData(ComboBox cboGroupName,ComboBox cboPage,TextBox txtFiled,DataSet ds)
        {
            try
            {
                txtFiled.Text = "";
                cboGroupName.SelectedIndex = -1;
                cboGroupName.Items.Clear();

                sql = string.Empty;//每一次都要清空

                DataTable tab = new DataTable();
                tab = ds.Tables[cboPage.SelectedItem.ToString()];

                foreach (var item in tab.Columns)
                {
                    if (!item.ToString().Contains("数") && !item.ToString().Contains("序号") && !item.ToString().Contains("备注"))
                        cboGroupName.Items.Add(item.ToString());
                    else if (item.ToString().Contains("数") && !item.ToString().Contains("备注"))
                        sql += "select '" + item.ToString() + "'as '显示字段' union ";
                }
                sql = sql.Substring(0, sql.LastIndexOf("union"));
            }
            catch (Exception ex)
            {

                
            }
           
        }
        
        /// <summary>
        /// 序列化到内存流
        /// </summary>
        /// <returns></returns>
        private static Stream GetRdlcStream(XmlDocument xmlDoc)
        {
            
                Stream ms = new MemoryStream();
          
                XmlSerializer serializer = new XmlSerializer(typeof(XmlDocument));
                serializer.Serialize(ms, xmlDoc);
               
                ms.Position = 0;
                
                return ms;
           
            
        }

        /// <summary>
        /// 过滤table字段
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="fileds">显示字段</param>
        /// <param name="groupName">分组名称</param>
        /// <returns></returns>
        public static DataTable FilterFileds(DataTable tab,string fileds,string groupName)
        {
            try
            {
                var strFileds = fileds.Split(',');

                DataTable dt = new DataTable();
                DataColumn dc = null;
                dc = dt.Columns.Add(groupName, Type.GetType("System.String"));
                foreach (var item in strFileds)
                {
                    dc = null;
                    dc = dt.Columns.Add(item, Type.GetType("System.Decimal"));
                }

                for (int i = 0; i < tab.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[groupName] = tab.Rows[i][groupName].ToString();
                    for (int y = 0; y < strFileds.Length; y++)
                    {
                        dr[strFileds[y]] = Convert.ToDecimal(string.IsNullOrEmpty(tab.Rows[i][strFileds[y]].ToString())==true?"0": tab.Rows[i][strFileds[y]]);
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
         
        }
      
        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="reportViewer"></param>
        /// <param name="cboGroupName">分组名称下拉框</param>
        /// <param name="txtFileds">显示字段文本框</param>
        /// <param name="cboType">图表类型下拉框</param>
        /// <param name="cboPage">页签下拉框</param>
        /// <param name="ds">数据集</param>
        public static void ShowCharts(ReportViewer reportViewer,ComboBox cboGroupName,TextBox txtFileds,ComboBox cboType,ComboBox cboPage,DataSet ds)
        {
            int chartType = 0;
            string strFiled = string.Empty;//显示字段
            string groupName = string.Empty;//分组名称

            #region 页签
            if (cboPage.SelectedIndex == -1 || string.IsNullOrEmpty(cboPage.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择页签！");
                return;
            } 
            #endregion

            #region 分组名称
            if (cboGroupName.SelectedIndex == -1 || string.IsNullOrEmpty(cboGroupName.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择分组名称！");
                return;
            }
            groupName = cboGroupName.SelectedItem.ToString(); 
            #endregion

            #region 显示字段
            if (string.IsNullOrEmpty(txtFileds.Text))
            {
                MessageBox.Show("请选择显示字段！");
                return;
            }
            strFiled = txtFileds.Text;
            #endregion

            #region 图表类型
            if (cboType.SelectedIndex == -1 || string.IsNullOrEmpty(cboType.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择图表类型！");
                return;
            }
            switch (cboType.SelectedItem.ToString())
            {
                case "条形图":
                    chartType = 1;
                    break;
                case "折线图":
                    chartType = 2;
                    break;
                case "扇形图":
                    chartType = 3;
                    break;
            }
            #endregion

            var tab = Charts.FilterFileds(ds.Tables[cboPage.SelectedItem.ToString()], strFiled, groupName);
            DynamicChart(reportViewer, tab, chartType, groupName, strFiled);
        }
       
        /// <summary>
        /// 加载显示字段
        /// </summary>
        /// <param name="txtFiled">显示字段文本框</param>
        public static void LoadShowFiled(TextBox txtFiled)
        {
            if (string.IsNullOrEmpty(sql))
            {
                MessageBox.Show("没有显示的字段！");
                return;
            }
            GDSJ_Framework.WinForm.CommonForm.frmSearchData frm = new GDSJ_Framework.WinForm.CommonForm.frmSearchData(Program.DB, sql, false, true);
            frm.ShowDialog();
            if (!string.IsNullOrEmpty(frm.ReturnDataXML))
            {
                var lst = GDSJ_Framework.Common.StringHelper.GetDataFromTag(frm.ReturnDataXML, "<显示字段>", "</显示字段>");
                txtFiled.Text = string.Join(",", lst);
            }
        }
        /// <summary>
        /// 加载页签下拉框数据
        /// </summary>
        /// <param name="cboPage">页签下拉框</param>
        /// <param name="tabControl">选项卡</param>
        public static void LoadPageDate(ComboBox cboPage,TabControl tabControl)
        {
            try
            {
                foreach (TabPage item in tabControl.TabPages)
                {
                    if (!item.Text.Contains("图"))
                        cboPage.Items.Add(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
          
        }

        /// <summary>
        /// 带标题的空XML
        /// </summary>
        private static string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"+
            "<Report xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\">"+
            "<Body>"+
            "<ReportItems>"+
            "<Chart Name=\"Chart1\">"+
            "<ChartCategoryHierarchy>"+
            "@ChartCategoryHierarchy"+
            "</ChartCategoryHierarchy>"+
            "<ChartSeriesHierarchy>"+
            "@ChartSeriesHierarchy"+
            "</ChartSeriesHierarchy>"+
            "<ChartData>"+
            "<ChartSeriesCollection>"+
            "@ChartSeriesCollection"+
            "</ChartSeriesCollection>"+
            "</ChartData>"+
            "<ChartAreas>"+
            "<ChartArea Name=\"Default\">"+
            "<ChartCategoryAxes>"+
            "<ChartAxis Name=\"Primary\">"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "<Color>White</Color>"+
            "</Style>"+
            "<ChartAxisTitle>"+
            "<Caption />"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "</ChartAxisTitle>"+
            "<ChartMajorGridLines>"+
            "<Enabled>False</Enabled>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "</Border>"+
            "</Style>"+
            "</ChartMajorGridLines>"+
            "<ChartMinorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "<Style>Dotted</Style>"+
            "</Border>"+
            "</Style>"+
            "</ChartMinorGridLines>"+
            "<ChartMinorTickMarks>"+
            "<Length>0.5</Length>"+
            "</ChartMinorTickMarks>"+
            "<CrossAt>NaN</CrossAt>"+
            "<Minimum>NaN</Minimum>"+
            "<Maximum>NaN</Maximum>"+
            "<ChartAxisScaleBreak>"+
            "<Style />"+
            "</ChartAxisScaleBreak>"+
            "</ChartAxis>"+
            "<ChartAxis Name=\"Secondary\">"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "<ChartAxisTitle>"+
            "<Caption>轴标题</Caption>"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "</ChartAxisTitle>"+
            "<ChartMajorGridLines>"+
            "<Enabled>False</Enabled>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "</Border>"+
            "</Style>"+
            "</ChartMajorGridLines>"+
            "<ChartMinorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "<Style>Dotted</Style>"+
            "</Border>"+
            "</Style>"+
            "</ChartMinorGridLines>"+
            "<ChartMinorTickMarks>"+
            "<Length>0.5</Length>"+
            "</ChartMinorTickMarks>"+
            "<CrossAt>NaN</CrossAt>"+
            "<Location>Opposite</Location>"+
            "<Minimum>NaN</Minimum>"+
            "<Maximum>NaN</Maximum>"+
            "<ChartAxisScaleBreak>"+
            "<Style />"+
            "</ChartAxisScaleBreak>"+
            "</ChartAxis>"+
            "</ChartCategoryAxes>"+
            "<ChartValueAxes>"+
            "<ChartAxis Name=\"Primary\">"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "<Color>White</Color>"+
            "</Style>"+
            "<ChartAxisTitle>"+
            "<Caption />"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "</ChartAxisTitle>"+
            "<ChartMajorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "</Border>"+
            "</Style>"+
            "</ChartMajorGridLines>"+
            "<ChartMinorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "<Style>Dotted</Style>"+
            "</Border>"+
            "</Style>"+
            "</ChartMinorGridLines>"+
            "<ChartMinorTickMarks>"+
            "<Length>0.5</Length>"+
            "</ChartMinorTickMarks>"+
            "<CrossAt>NaN</CrossAt>"+
            "<Minimum>NaN</Minimum>"+
            "<Maximum>NaN</Maximum>"+
            "<ChartAxisScaleBreak>"+
            "<Style />"+
            "</ChartAxisScaleBreak>"+
            "</ChartAxis>"+
            "<ChartAxis Name=\"Secondary\">"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "<ChartAxisTitle>"+
            "<Caption>轴标题</Caption>"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "</Style>"+
            "</ChartAxisTitle>"+
            "<ChartMajorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "</Border>"+
            "</Style>"+
            "</ChartMajorGridLines>"+
            "<ChartMinorGridLines>"+
            "<Style>"+
            "<Border>"+
            "<Color>Gainsboro</Color>"+
            "<Style>Dotted</Style>"+
            "</Border>"+
            "</Style>"+
            "</ChartMinorGridLines>"+
            "<ChartMinorTickMarks>"+
            "<Length>0.5</Length>"+
            "</ChartMinorTickMarks>"+
            "<CrossAt>NaN</CrossAt>"+
            "<Location>Opposite</Location>"+
            "<Minimum>NaN</Minimum>"+
            "<Maximum>NaN</Maximum>"+
            "<ChartAxisScaleBreak>"+
            "<Style />"+
            "</ChartAxisScaleBreak>"+
            "</ChartAxis>"+
            "</ChartValueAxes>"+
            "<Style>"+
            "<BackgroundColor>Black</BackgroundColor>" +
            "<BackgroundGradientType>None</BackgroundGradientType>"+
            "</Style>"+
            "</ChartArea>"+
            "</ChartAreas>"+
            "<ChartLegends>"+
            "<ChartLegend Name=\"Default\">"+
            "<Style>"+
            "<BackgroundGradientType>None</BackgroundGradientType>"+
            "<FontSize>16pt</FontSize>"+
            "<FontWeight>Bold</FontWeight>"+
            "<Color>White</Color>"+
            "</Style>"+
            "<ChartLegendTitle>"+
            "<Caption />"+
            "<Style>"+
            "<FontSize>8pt</FontSize>"+
            "<FontWeight>Bold</FontWeight>"+
            "<TextAlign>Center</TextAlign>"+
            "</Style>"+
            "</ChartLegendTitle>"+
            "<HeaderSeparatorColor>Black</HeaderSeparatorColor>"+
            "<ColumnSeparatorColor>Black</ColumnSeparatorColor>"+
            "</ChartLegend>"+
            "</ChartLegends>"+
            "<Palette>BrightPastel</Palette>" +//图形颜色
            "<ChartBorderSkin>"+
            "<Style>"+
            "<BackgroundColor>Gray</BackgroundColor>"+
            "<BackgroundGradientType>None</BackgroundGradientType>"+
            "<Color>White</Color>"+
            "</Style>"+
            "</ChartBorderSkin>"+
            "<ChartNoDataMessage Name=\"NoDataMessage\">"+
            "<Caption>没有可用数据</Caption>"+
            "<Style>"+
            "<BackgroundGradientType>None</BackgroundGradientType>"+
            "<TextAlign>General</TextAlign>"+
            "<VerticalAlign>Top</VerticalAlign>"+
            "</Style>"+
            "</ChartNoDataMessage>"+
            "<DataSetName>DataSet_Chart</DataSetName>"+
            "<Top>0.0412cm</Top>"+
            "<Height>17.13026cm</Height>"+
            "<Width>35.26896cm</Width>" +
            "<Style>"+
            "<Border>"+
          //  "<Color>LightGrey</Color>"+
            "<Style>Solid</Style>"+
            "</Border>"+
            "<BackgroundColor>Black</BackgroundColor>"+
            "<BackgroundGradientType>None</BackgroundGradientType>"+
            "</Style>"+
            "</Chart>"+
            "</ReportItems>"+
            "<Height>6.76042in</Height>"+
            "<Style />"+
            "</Body>"+
            "<Width>13.88542in</Width>"+
            "<Page>"+
            "<PageHeader>"+
            "<Height>1.32292cm</Height>"+
            "<PrintOnFirstPage>true</PrintOnFirstPage>"+
            "<PrintOnLastPage>true</PrintOnLastPage>"+
            "<ReportItems>"+
            "<Textbox Name=\"Textbox6\">"+
            "<CanGrow>true</CanGrow>"+
            "<KeepTogether>true</KeepTogether>"+
            "<Paragraphs>"+
            "<Paragraph>"+
            "<TextRuns>"+
            "<TextRun>"+
            "<Value>=Parameters!标题.Value</Value>"+
            "<Style>"+
            "<FontFamily>微软雅黑</FontFamily>"+
            "<FontSize>20pt</FontSize>"+
            "<FontWeight>Bold</FontWeight>"+
            "</Style>"+
            "</TextRun>"+
            "</TextRuns>"+
            "<Style>"+
            "<TextAlign>Center</TextAlign>"+
            "</Style>"+
            "</Paragraph>"+
            "</Paragraphs>"+
            "<rd:DefaultName>Textbox1</rd:DefaultName>"+
            "<Height>1.18773cm</Height>"+
            "<Width>35.26896cm</Width>"+
            "<Style>"+
            "<Border>"+
            "<Style>None</Style>"+
            "</Border>"+
            "<VerticalAlign>Top</VerticalAlign>"+
            "<PaddingLeft>2pt</PaddingLeft>"+
            "<PaddingRight>2pt</PaddingRight>"+
            "<PaddingTop>2pt</PaddingTop>"+
            "<PaddingBottom>2pt</PaddingBottom>"+
            "</Style>"+
            "</Textbox>"+
            "</ReportItems>"+
            "<Style>"+
            "<Border>"+
            "<Style>None</Style>"+
            "</Border>"+
            "</Style>"+
            "</PageHeader>"+
            "<PageHeight>29.7cm</PageHeight>"+
            "<PageWidth>21cm</PageWidth>"+
            "<LeftMargin>2cm</LeftMargin>"+
            "<RightMargin>2cm</RightMargin>"+
            "<TopMargin>2cm</TopMargin>"+
            "<BottomMargin>2cm</BottomMargin>"+
            "<ColumnSpacing>0.13cm</ColumnSpacing>"+
            "<Style />"+
            "</Page>"+
            "<AutoRefresh>0</AutoRefresh>"+
            "<DataSources>"+
            "<DataSource Name=\"DataSetDynamic\">"+
            "<ConnectionProperties>"+
            "<DataProvider>System.Data.DataSet</DataProvider>"+
            "<ConnectString>/* Local Connection */</ConnectString>"+
            "</ConnectionProperties>"+
            "<rd:DataSourceID>e1636472-7557-491b-9190-2088b4f9d890</rd:DataSourceID>"+
            "</DataSource>"+
            "</DataSources>"+
            "<DataSets>"+
            "<DataSet Name=\"DataSet_Chart\">"+
            "<Query>"+
            "<DataSourceName>DataSetDynamic</DataSourceName>"+
            "<CommandText>/* Local Query */</CommandText>"+
            "</Query>"+
            "<Fields>"+
            "@field"+
            "</Fields>"+
            "</DataSet>"+
            "</DataSets>"+
            "<ReportParameters>"+
            "<ReportParameter Name=\"标题\">"+
            "<DataType>String</DataType>"+
            "<Nullable>true</Nullable>"+
            "<AllowBlank>true</AllowBlank>"+
            "<Prompt>ReportParameter1</Prompt>"+
            "</ReportParameter>"+
            "</ReportParameters>"+
            "<rd:ReportUnitType>Cm</rd:ReportUnitType>"+
            "<rd:ReportID>3733bc73-2eaf-4f89-8799-d1d54a20f15c</rd:ReportID>"+
            "</Report>";

    }
}
