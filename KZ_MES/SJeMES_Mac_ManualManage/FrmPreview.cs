
using System;
using System.Windows.Forms;
using AxWMPLib;
using SJEMS_SYS;

namespace SJeMES_Mac_ManualManage
{
    public partial class FrmPreview : Form
    {
        public string path;
        AxWindowsMediaPlayer player;
        public FrmPreview()
        {
            InitializeComponent();
           
        }
        private void Preview()
        {
            try
            {
               
                if (this.InvokeRequired)
                {
                   
                    var hander = new ShowDelegate(Preview);
                    this.Invoke(hander);

                }
               
                //panel1.Controls.Clear();
                //Label lbl = new Label();
                //lbl.Location = new System.Drawing.Point(342, 229);
                //lbl.Size = new System.Drawing.Size(134, 118);
                //lbl.Text = "加载中...";
                //panel1.Controls.Add(lbl);
                string stuff_Name = path.Substring(path.LastIndexOf(".") + 1);
                string filePath = string.Empty;
                switch (stuff_Name.ToLower())
                {
                    case "xlsx":
                        filePath = OfficeToPDF.ExcelToPdf(path, true);
                        if (filePath == "404")
                        {
                            MessageBox.Show("文件已被删除，请重新上传！");
                            this.Close();
                            return;
                        }else if(filePath.Contains("pdf"))
                        {
                            OpenPdf(filePath);
                        }
                        else
                        {
                            MessageBox.Show(filePath);
                            this.Close();
                            return;
                           
                        }
                        break;
                    case "xls":

                        filePath = OfficeToPDF.ExcelToPdf(path, true);
                        if (filePath == "404")
                        {
                            MessageBox.Show("文件已被删除，请重新上传！");
                            this.Close();
                            return;
                        }
                        else if (filePath.Contains("pdf"))
                        {
                            OpenPdf(filePath);
                        }
                        else
                        {
                            MessageBox.Show(filePath);
                            this.Close();
                            return;

                        }
                        break;
                    case "docx":
                        filePath = OfficeToPDF.WordToPdf(path, true);
                        if (filePath == "404")
                        {
                            MessageBox.Show("文件已被删除，请重新上传！");
                            this.Close();
                            return;
                        }
                        else if (filePath.Contains("pdf"))
                        {
                            OpenPdf(filePath);
                        }
                        else
                        {
                            MessageBox.Show(filePath);
                            this.Close();
                            return;

                        }
                        break;
                    case "doc":
                        filePath = OfficeToPDF.WordToPdf(path, true);
                        if (filePath == "404")
                        {
                            MessageBox.Show("文件已被删除，请重新上传！");
                            this.Close();
                            return;
                        }
                        else if (filePath.Contains("pdf"))
                        {
                            OpenPdf(filePath);
                        }
                        else
                        {
                            MessageBox.Show(filePath);
                            this.Close();
                            return;

                        }
                        break;
                    case "pdf":
                     
                        filePath = OfficeToPDF.PDF(path, true);
                        if (filePath == "404")
                        {
                            MessageBox.Show("文件已被删除，请重新上传！");
                            this.Close();
                            return;
                        }
                        else if (filePath.Contains("pdf"))
                        {
                            OpenPdf(filePath);
                        }
                        else
                        {
                            MessageBox.Show(filePath);
                            this.Close();
                            return;

                        }
                        break;
                    case "mp4":
                        OpenVideo(path);
                        break;
                    case "mp3":
                        OpenVideo(path);
                        break;
                    case "png":
                        OpenImg(path);
                        break;
                    case "jpg":
                        OpenImg(path);
                        break;
                    case "jpeg":
                        OpenImg(path);
                        break;
                    case "bmp":
                        OpenImg(path);
                        break;
                    default:
                        //up();
                        MessageBox.Show("文件下载失败，暂不支持" + stuff_Name + "文件");
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
         

        }
        /// <summary>
        /// 打开视频
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenVideo(string filePath)
        {
            panel1.Controls.Clear();
            player = new AxWindowsMediaPlayer();
            player.BeginInit();
            player.Dock = DockStyle.Fill;

            player.PlayStateChange += AxWindowsMediaPlayerStop;
            panel1.Controls.Add(player);
            player.EndInit();
            player.URL = filePath;//调用服务器文件
            player.Ctlcontrols.play();
        }
        public void OpenImg(string filePath)
        {
            panel1.Controls.Clear();
            PictureBox picture = new PictureBox();
            picture.Dock = DockStyle.Fill;
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            panel1.Controls.Add(picture);
            picture.ImageLocation = filePath;
        }
        /// <summary>
        /// 查看PDF
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenPdf(string filePath)
        {
            try
            {
                panel1.Controls.Clear();
                AxAcroPDFLib.AxAcroPDF axAcroPDF = new AxAcroPDFLib.AxAcroPDF();
                axAcroPDF.Dock = DockStyle.Fill;
                panel1.Controls.Add(axAcroPDF);
                axAcroPDF.setShowToolbar(false);
                axAcroPDF.setShowScrollbars(false);

                axAcroPDF.LoadFile(filePath);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }

            

        }

        private delegate void ShowDelegate();
        private void FrmPreview_Load(object sender, EventArgs e)
        {
           
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Async));
            thread.IsBackground = true;
            thread.Start();
            
        }

        private void Async()
        {
            this.BeginInvoke(new ShowDelegate(Preview));
        }
        private void AxWindowsMediaPlayerStop(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            AxWindowsMediaPlayer A = sender as AxWindowsMediaPlayer;
            if (A.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                A.Ctlcontrols.stop();
            }
      
        }

        private void FrmPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(player!=null)
             player.Ctlcontrols.stop();
        }

    }
}
