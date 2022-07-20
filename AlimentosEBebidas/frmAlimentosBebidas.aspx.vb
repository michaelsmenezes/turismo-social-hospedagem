Imports Uteis
Partial Class frmAlimentosBebidas
    Inherits System.Web.UI.Page
    Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
    Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("~/AcessoNegado.aspx")
                Return
            End If
        End If
    End Sub

    Protected Sub imgMeiaDiaria_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgMeiaDiaria.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmMeiaDiariaSemPernoite.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgPrevisao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrevisao.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmRestaurantePrevisao.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgPasseios_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPasseios.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("http://passeiopratorapido/")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub ImgRegManual_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgRegManual.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmInsercaoManualRefeicao.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgHistRefeicao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHistRefeicao.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmHistoricoRefIndividual.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgHistRefeicoes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHistRefeicoes.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmHistoricoRefeicoes.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgVM_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgVM.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("frmVendingMarchines.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub

    Protected Sub imgLanchonete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgLanchonete.Click
        Response.Redirect("frmLanchonetes.aspx")
    End Sub

    Protected Sub imbCortesiaConsumo_Click(sender As Object, e As ImageClickEventArgs) Handles imbCortesiaConsumo.Click
        If (Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Restaurante Gerencial") Or Grupos.Contains("Turismo Social Piri Restaurante Gerencial")) Then
            Response.Redirect("http://estoquecaldas/frmRelSaidaProdutos.aspx")
        Else
            Response.Redirect("~/AcessoNegado.aspx")
            Return
        End If
    End Sub
End Class
