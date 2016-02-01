// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCache.asmx.cs" company="John Allberg">
//   Copyright ©2001-2016 John Allberg
//   
//   This program is free software; you can redistribute it and/or
//   modify it under the terms of the GNU General Public License
//   as published by the Free Software Foundation; either version 2
//   of the License, or (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WinShooterUpdate
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Web.Services;

    /// <summary>
    /// Summary description for UpdateCache1
    /// </summary>
    [WebService(Namespace = "http://www.winshooter.se/UpdateCache")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class UpdateCache : WebService
    {
        /// <summary>
        /// The from local file.
        /// </summary>
        /// <param name="fileContent">
        /// The file content.
        /// </param>
        [WebMethod]
        public void FromLocalFile(string fileContent)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(fileContent);

                MemoryStream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);

                sendEmail("Body", stream);
            }
            catch (Exception exc)
            {
                sendEmail(exc.ToString(), null);
            }
        }

        /// <summary>
        /// The send email.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        private void sendEmail(string body, MemoryStream stream)
        {
            MailMessage message = new MailMessage(
                "john@allberg.se", 
                "john@allberg.se", 
                "Autoupdate of cache", 
                body);
            if (stream != null)
                message.Attachments.Add(new Attachment(stream, "LocalCache.xml"));

            string emailUsername = ConfigurationManager.AppSettings["emailUsername"];
            string emailPassword = ConfigurationManager.AppSettings["emailUsername"];

            SmtpClient client = new SmtpClient("127.0.0.1", 26);
            client.Credentials = new NetworkCredential(emailUsername, emailPassword);
            client.Send(message);
        }
    }
}
