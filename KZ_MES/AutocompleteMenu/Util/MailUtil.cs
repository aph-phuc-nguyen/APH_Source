using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace mes_master.Util
{
    class MailUtil
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="userEmailAddress">发件人地址</param>
        /// <param name="userName">发件人姓名(可为空)</param>
        /// <param name="password">密码</param>
        /// <param name="host">邮件服务器地址</param>
        /// <param name="port"></param>
        /// <param name="sendToList">收件人(多个电子邮件地址之间必须用逗号字符（“,”）分隔)</param>
        /// <param name="sendCCList">抄送人(多个电子邮件地址之间必须用逗号字符（“,”）分隔)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="attachmentsPath">附件路径</param>
        /// <param name="errorMessage">错误信息</param>
        public static bool SendMessage(List<string> sendToList, List<string> sendCCList, string subject, string body, string[] attachmentsPath, out string errorMessage)
        {
            string userEmailAddress = "ape-mes@apachefootwear.com";
            string userName = "ape-mes@apachefootwear.com";
            //string userName = "pengtao-xu@apachefootwear.com";
            string password = "Aa123456";
            string host = "10.2.1.186";
            int port = 25;

            errorMessage = string.Empty;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(userEmailAddress, password);//用户名、密码
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = host;//邮件服务器
            client.Port = port;//端口号 非SSL方式，默认端口号为：25
            client.UseDefaultCredentials = false;

            MailMessage msg = new MailMessage();
            //加发件人
            foreach (string send in sendToList)
            {
                msg.To.Add(send);
            }
            //加抄送
            foreach (string cc in sendCCList)
            {
                msg.CC.Add(cc);
            }

            //在有附件的情况下添加附件
            if (attachmentsPath != null && attachmentsPath.Length > 0)
            {
                foreach (string path in attachmentsPath)
                {
                    var attachFile = new Attachment(path);
                    msg.Attachments.Add(attachFile);
                }
            }
            msg.From = new MailAddress(userEmailAddress, userName);//发件人地址
            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   

            try
            {
                client.Send(msg);
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
