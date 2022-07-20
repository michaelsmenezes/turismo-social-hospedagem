
Partial Class InformacoesGerenciais_frmMenuRecepcao
    Inherits System.Web.UI.Page
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    
    Protected Sub imgHistPassantes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgConsultaSPC.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('https://servicos.spc.org.br/spc/controleacesso/autenticacao/entry.action');", True)
            'Response.Redirect("https://servicos.spc.org.br/spc/controleacesso/autenticacao/entry.action")
        End If
    End Sub

    Protected Sub imgOpinario_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOpinario.Click
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                'Direcionando a aplicação para o banco de Caldas Novas
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://opinario');", True)
            Case Else
                'Direcionando a aplicação para o banco de Pirenopolis
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://opinarioPiri');", True)
        End Select
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
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Caldas recebem passantes no parque aquático
                    lblPortariaCaldas.Visible = True
                    imgPortariaCaldas.Visible = True
                Case Else
                    'Caldas recebem passantes no parque aquático
                    lblPortariaCaldas.Visible = False
                    imgPortariaCaldas.Visible = False
            End Select
        End If
    End Sub

    Protected Sub imgPassantes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPassantes.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Response.Redirect("http://passantecaldas/")
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://passantecaldas/');", True)
                Case Else
                    Response.Redirect("AcessoNegado.aspx")
                    Return
            End Select
        End If
    End Sub

    Protected Sub imgCartaoConsumo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCartaoConsumo.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://cartaoconsumocaldas/');", True)
                    'Response.Redirect("http://cartaoconsumocaldas/")
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
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Response.Redirect("http://caixafinanceirocaldas/caixafinanceiro.aspx")
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://caixafinanceirocaldas/caixafinanceiro.aspx');", True)
                Case Else
                    Response.Redirect("AcessoNegado.aspx")
                    Return
            End Select
        End If
    End Sub

    Protected Sub imgPortariaCaldas_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPortariaCaldas.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Response.Redirect("http://caixafinanceirocaldas/caixafinanceiro.aspx")
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://PortariaCaldas');", True)
                Case Else
                    Response.Redirect("AcessoNegado.aspx")
                    Return
            End Select
        End If
    End Sub

    Protected Sub imgRecepcaoNet_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRecepcaoNet.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('Recepcao.aspx');", True)
            'Response.Redirect("Recepcao.aspx")
        End If
    End Sub

    Protected Sub imgRetiraCaixa_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRetiraCaixa.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Response.Redirect("http://turismonetcaldas/AcessoNegado.aspx")
            Else
                Response.Redirect("http://turismonetpiri/AcessoNegado.aspx")
            End If
            Return
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://turismonetcaldas/Financeiro/frmRetiradaDeCaixa.aspx');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://turismonetpiri/Financeiro/frmRetiradaDeCaixa.aspx');", True)
            End If
        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "redirect", "window.open('http://turismonetcaldas/Reserva.aspx');", True)
            'Response.Redirect("Recepcao.aspx")
        End If
    End Sub
End Class
