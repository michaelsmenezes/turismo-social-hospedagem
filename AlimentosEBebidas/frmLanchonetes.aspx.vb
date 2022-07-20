
Partial Class frmLanchonetes
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
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
        End If
    End Sub

    Protected Sub imgConfiguracao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgConfiguracao.Click
        Select Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://consumocaldas/');", True)
            Case Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://consumocaldasPiri/');", True)
        End Select
        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://consumocaldas/');", True)

    End Sub

    Protected Sub imgRelatorioVM_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEntregaR.Click
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://consumocaldas/consumorefeicao.aspx');", True)
            Case Else
                imgEntregaR.Visible = False
                lblEntregaP.Visible = False
        End Select
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub

    Protected Sub imgConsultaPratos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgConsultaPratos.Click
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://pratosrapidos/');", True)
            Case Else
                imgConsultaPratos.Visible = False
                lblConsultaP.Visible = False
        End Select

    End Sub

    Protected Sub imgPonto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPonto.Click
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://server_mail/forponto/fptoweb.exe/pientramarcacao');", True)
    End Sub
End Class
