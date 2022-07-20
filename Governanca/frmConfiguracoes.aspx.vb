
Partial Class frmConfiguracoes
    Inherits System.Web.UI.Page
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("CNV_GOVERNANCA") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Governanca")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            mnuConfiguracao_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            mnuConfiguracao_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                'Alterando dinamicamente as cores da pagina
                Dim myHtmlLink As New HtmlLink()
                Page.Header.Controls.Remove(myHtmlLink)
                myHtmlLink.Href = "~/stylesheet.css"
                myHtmlLink.Attributes.Add("rel", "stylesheet")
                myHtmlLink.Attributes.Add("type", "text/css")
                'Adicionando o Css na Sessão Head da Página
                Page.Header.Controls.Add(myHtmlLink)
            Else
                'Alterando dinamicamente as cores da pagina
                Dim myHtmlLink As New HtmlLink()
                Page.Header.Controls.Remove(myHtmlLink)
                myHtmlLink.Href = "~/stylesheetverde.css"
                myHtmlLink.Attributes.Add("rel", "stylesheet")
                myHtmlLink.Attributes.Add("type", "text/css")
                'Adicionando o Css na Sessão Head da Página
                Page.Header.Controls.Add(myHtmlLink)
            End If
        End If
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("Governanca.aspx")
    End Sub
End Class
