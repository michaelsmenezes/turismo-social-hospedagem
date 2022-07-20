Imports System.Net.Mail
Partial Class erro
    Inherits System.Web.UI.Page
    Dim objSmtp As SmtpClient
    'Dim objEmail As MailMessage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objSmtp As SmtpClient

        '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
        'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
        objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
        objSmtp.EnableSsl = True
        objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
        objSmtp.UseDefaultCredentials = False
        objSmtp.Timeout = 5000

        Dim objEmail As New MailMessage()
        objEmail.From = New MailAddress(Replace(User.Identity.Name, "SESC-GO.COM.BR\", "") + "@sescgo.com.br")
        objEmail.To.Add(New MailAddress("elvis.irineu@sescgo.com.br"))
        objEmail.To.Add(New MailAddress("gustavo.cesar@sescgo.com.br"))

        objEmail.Subject = "Erro - " & Request.QueryString("sistema")
        objEmail.IsBodyHtml = True
        objEmail.Body = Request.QueryString("Erro") & "<br/><br/> " & Request.QueryString("excecao") & " <br/><br/> " & Request.QueryString("acao") & "<br/>" & Request.QueryString("Local")
        objEmail.Priority = MailPriority.Normal
        objSmtp.Send(objEmail)
    End Sub
End Class
