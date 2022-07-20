
Partial Class frmVendingMarchines
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
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                Response.Redirect("http://vendingmachines/")
            Case Else
                Response.Redirect("http://vendingmachinesPiri/")
        End Select
    End Sub

    Protected Sub imgRelatorioVM_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRelatorioVM.Click
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                Response.Redirect("http://vendingmachines/frmvendavenmach.aspx")
            Case Else
                Response.Redirect("http://vendingmachinesPiri/frmvendavenmach.aspx")
        End Select
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub
End Class
