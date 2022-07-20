
Partial Class frmMenuFinanceiro
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
            'RESTRIÇÃO DE ACESSO RETIRADO, POIS TODOS PRECISAM TER ACESSO AO OPINARIO.
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
            'Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            'Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            'If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            '    Response.Redirect("AcessoNegado.aspx")
            '    Return
            'End If
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

    Protected Sub imgCartaoConsumo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCartaoConsumo.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao") Or Grupos.Contains("Turismo Social Financeiro")) Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://turismonetcaldas/acessonegado.aspx")
                    Return
                Case Else
                    Response.Redirect("http://turismonetPiri/acessonegado.aspx")
                    Return
            End Select
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://cartaoconsumocaldas/")
                Case Else
                    Response.Redirect("AcessoNegado.aspx")
                    Return
            End Select
        End If
    End Sub

    Protected Sub imgCaixaFinanceiro_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCaixaFinanceiro.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao") Or Grupos.Contains("Turismo Social Financeiro")) Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://turismonetcaldas/acessonegado.aspx")
                    Return
                Case Else
                    Response.Redirect("http://turismonetPiri/acessonegado.aspx")
                    Return
            End Select
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://caixafinanceirocaldas/visualizacaixa.aspx")
                Case Else
                    Response.Redirect("AcessoNegado.aspx")
                    Return
            End Select
        End If
    End Sub
    Protected Sub imgRetiraCaixa_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRetiraCaixa.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao") Or Grupos.Contains("Turismo Social Financeiro")) Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://turismonetcaldas/acessonegado.aspx")
                    Return
                Case Else
                    Response.Redirect("http://turismonetPiri/acessonegado.aspx")
                    Return
            End Select
        Else
            Response.Redirect("http://turismonetcaldas/Financeiro/frmRetiradaDeCaixa.aspx")
        End If
    End Sub

    Protected Sub imgFechamamentoCxaConsumo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFechamamentoCxaConsumo.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Fechamento de Caixa de Consumo") Or Grupos.Contains("Turismo Social Total")) Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    Response.Redirect("http://turismonetcaldas/acessonegado.aspx")
                    Return
                Case Else
                    Response.Redirect("http://turismonetPiri/acessonegado.aspx")
                    Return
            End Select
        Else
            Response.Redirect("http://consumocaldas/frmFechamentoCaixaConsumo.aspx")
        End If
    End Sub
End Class
