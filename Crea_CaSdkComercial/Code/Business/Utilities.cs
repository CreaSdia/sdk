using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Crea_CaSdkComercial;
using Crea_CaSdkComercial.Code.Data;
using SdkCreaComercial.Business.Sdk;

namespace SdkCreaComercial.Code.Business
{
    public class Utilities
    {
        public static void CrearCorreo(Documento documento, string baseDatos, string asunto,
            string plantillaCorreo, StreamWriter sw)
        {
            if (!documento.EsTimbrar || plantillaCorreo == "" || documento.DocumentoId == 0 || documento.CodigoCliente == "") return;

            var sdkContext = new ContpaqiDbContext(baseDatos);
            var mensaje = $"Se generó y timbró el documento, folio y serie: {documento.Folio} {documento.Serie}";

            sw.WriteLine(mensaje);

            string errorEnvio;
            var cliente = sdkContext.Clientes.FirstOrDefault(e => e.CCODIGOCLIENTE == documento.CodigoCliente);
            var destinatarios = new List<string> {cliente.CEMAIL1, cliente.CEMAIL2};
            var pathArchivos = Settings.Default.PathArchivos.Replace("#BASEDATOS#", baseDatos);
            var nombreArchivo = documento.Serie + documento.Folio;

            destinatarios.AddRange(cliente.CEMAIL3.Split(','));

            var attachments = new string[]
            {
                pathArchivos + nombreArchivo + ".xml",
                pathArchivos + nombreArchivo + ".pdf"
            };

            var cuerpoCorreo = "";

            if (File.Exists(plantillaCorreo))
            {
                cuerpoCorreo = File.ReadAllText(plantillaCorreo);
            }

            if (destinatarios.Any(e => e != ""))
            {
                SendMail(destinatarios.ToArray(), new string[0], new string[0], asunto, cuerpoCorreo,
                    true, attachments, out errorEnvio);

                if (errorEnvio != "")
                {
                    mensaje =
                        $"No se pudo envíar el correo para el documento: {documento.Folio} {documento.Serie}, error: {errorEnvio}";
                    sw.WriteLine(mensaje);
                }
            }
        }

        public static bool SendMail(string[] pMails, string[] pBccMails, string[] pCCMails, string pSubject,
            string pBody,
            bool isBodyHtml, string[] attachments, out string messageError,
            Dictionary<string, Stream> stearmAttachments = null)
        {
            messageError = "";
            var response = false;

            if (Settings.Default.EnviarEmail)
            {
                try
                {
                    var smtp = "smtp.sendgrid.net";
                    var user = "apikey";
                    var password = "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";
                    var senderEmail = Settings.Default.SenderEmail;
                    var senderName = Settings.Default.SenderName;
                    var replyTo = Settings.Default.ReplyTo;
                    var nameReplyTo = Settings.Default.NameReplyTo;

                    var objSendEmail = new SmtpClient(smtp)
                    {
                        Credentials = new NetworkCredential(user, password)
                    };
                    objSendEmail.Port = 587;

                    string defaultStyles =
                        " <style type=\"text/css\"> body, p, table, div, ul, li  {font-family:\"Verdana\", \"Arial\"; font-weight:normal; font-size:12px; } </style> ";

                    var message = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = pSubject,
                        IsBodyHtml = isBodyHtml,
                        Body = defaultStyles + pBody,
                    };
                    message.ReplyToList.Add(new MailAddress(replyTo, nameReplyTo));

                    //TO
                    foreach (var email in pMails.Where(email => email != ""))
                    {
                        message.To.Add(email.Trim());
                    }

                    //BCC
                    foreach (var email in pBccMails.Where(email => email != ""))
                    {
                        message.Bcc.Add(email.Trim());
                    }

                    //CC
                    foreach (var email in pCCMails.Where(email => email != ""))
                    {
                        message.CC.Add(email.Trim());
                    }

                    foreach (var attachment in attachments)
                    {
                        if (attachment != "" && File.Exists(attachment))
                        {
                            var attach = new Attachment(attachment);
                            message.Attachments.Add(attach);
                        }
                    }

                    if (stearmAttachments != null)
                    {
                        foreach (var attachment in stearmAttachments)
                        {
                            var attach = new Attachment((Stream) attachment.Value, attachment.Key);
                            message.Attachments.Add(attach);
                        }
                    }

                    objSendEmail.Send(message);
                    response = true;
                }
                catch (Exception ex)
                {
                    messageError = ex.Message;
                }
            }
            else
            {
                messageError = "NO esta configurado el sistema para envio de correos, favor de verificar.";
            }

            return response;
        }
    }
}