
Partial Class InformacoesGerenciais_frmMenuGerencial
    Inherits System.Web.UI.Page
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub imgHistHospede_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHistHospede.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmHistoricoHospedes.aspx")
        End If
    End Sub

    Protected Sub imgFreqPassante_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgFreqPassante.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("http://passantecaldas/PassanteTurno.aspx")
        End If
    End Sub

    Protected Sub imgHospPorCategoria_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHospPorCategoria.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("http://relatorioestada/")
        End If
    End Sub

    Protected Sub imgHistPassantes_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHistPassantes.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmHistoricoPassantes.aspx")
        End If
    End Sub

    Protected Sub imgOpinario_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgOpinario.Click
        Select Case Session("MasterPage").ToString
            Case Is = "~/TurismoSocial.Master"
                'Direcionando a aplicação para o banco de Caldas Novas
                Response.Redirect("http://opinario")
            Case Else
                'Direcionando a aplicação para o banco de Pirenopolis
                Response.Redirect("http://opinarioPiri")
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            imgAnhanguera.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/HTMLViewers/interactiveViewer.asp?ReportName=C%3A%5CProgram+Files%5CCrystal+Decisions%5CReport+Application+Server+9%5CReports%5CTurismo+Social+%2D+Hospedagem+Anhanguera%2Erpt')")
            imgBambui.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/HTMLViewers/interactiveViewer.asp?ReportName=C%3A%5CProgram+Files%5CCrystal+Decisions%5CReport+Application+Server+9%5CReports%5CTurismo+Social+%2D+Hospedagem+Bambui%2Erpt')")
            imgWilton.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/HTMLViewers/interactiveViewer.asp?ReportName=C%3A%5CProgram+Files%5CCrystal+Decisions%5CReport+Application+Server+9%5CReports%5CTurismo+Social+%2D+Hospedagem+Wilton%2Erpt')")
            imgKilzer.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/HTMLViewers/interactiveViewer.asp?ReportName=C%3A%5CProgram+Files%5CCrystal+Decisions%5CReport+Application+Server+9%5CReports%5CTurismo+Social+%2D+Hospedagem+Kilzer%2Erpt')")
            imgTodos.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/Hospedagem.asp')")
            imgRelatorioHospedagem.Attributes.Add("OnClick", "javascript:window.open('http://server_mail/rassamples/en/asp/Turismo/HospedagemPiri.asp')")
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                lblCaldo.Visible = False
                imgCaldosVendidos.Visible = False
                PnlRelGerencialPiri.Visible = False
                'Depois posso apargar esse painel, por ter feito o novo relatóri de hospedagem
                'pnlRelGerenciaCaldas.Visible = True
                'Só irá mostrar na portal de caldas novas
                lblAnaliseDemanda.Visible = True
                imgDemanda.Visible = True

                'Alterando dinamicamente as cores da pagina
                Dim myHtmlLink As New HtmlLink()
                Page.Header.Controls.Remove(myHtmlLink)
                myHtmlLink.Href = "~/stylesheet.css"
                myHtmlLink.Attributes.Add("rel", "stylesheet")
                myHtmlLink.Attributes.Add("type", "text/css")
                'Adicionando o Css na Sessão Head da Página
                Page.Header.Controls.Add(myHtmlLink)
            Else
                lblCaldo.Visible = True
                imgCaldosVendidos.Visible = True
                PnlRelGerencialPiri.Visible = True
                'Depois posso apargar esse painel, por ter feito o novo relatóri de hospedagem
                'pnlRelGerenciaCaldas.Visible = False
                'Só irá mostrar no portal de Caldas Novas
                lblAnaliseDemanda.Visible = False
                imgDemanda.Visible = False
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

    Protected Sub imgPrevEstada_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgPrevEstada.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmPrevisaoEstada.aspx")
        End If
    End Sub

    Protected Sub imgRelatorioHospedagem_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Response.Redirect("http://server_mail/rassamples/en/asp/Turismo/Hospedagem.asp")
            Else
                Response.Redirect("http://server_mail/rassamples/en/asp/Turismo/HospedagemPiri.asp")
            End If
        End If
    End Sub

    Protected Sub imgCaldosVendidos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCaldosVendidos.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmConsultaCaldo.aspx")
        End If
    End Sub

    Protected Sub imgEmbratur_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgEmbratur.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmMinisterioTurismo.aspx")
        End If
    End Sub

    Protected Sub imgHospedesAniversariantes_Click(sender As Object, e As ImageClickEventArgs) Handles px.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmAniversariantes.aspx")
        End If
    End Sub

    Protected Sub imgDemanda_Click(sender As Object, e As ImageClickEventArgs) Handles imgDemanda.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("~/frmAnaliseDemandaEmissivo.aspx")
        End If
    End Sub

    Protected Sub imgBI_Click(sender As Object, e As ImageClickEventArgs) Handles imgBI.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("http://srv-adm-bi/QvAJAXZfc/opendoc.htm?document=LG%20QVW%2FBI-SESC.qvw&host=QVS%40srv-adm-bi")
        End If
    End Sub

    Protected Sub imgRelHospedagemNew_Click(sender As Object, e As ImageClickEventArgs) Handles imgRelHospedagemNew.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("~/InformacoesGerenciais/frmRelatorioHospedagem.aspx")
        End If
    End Sub

    Protected Sub imgCortesiasHospedagem_Click(sender As Object, e As ImageClickEventArgs) Handles imgCortesiasHospedagem.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("~/InformacoesGerenciais/frmCortesiasHospedagem.aspx")
        End If
    End Sub
    Protected Sub imgOverBooking_Click(sender As Object, e As ImageClickEventArgs) Handles imgOverBooking.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmConsultaOverbook.aspx")
        End If
    End Sub
    Protected Sub imgTransferencia_Click(sender As Object, e As ImageClickEventArgs) Handles imgTransferencia.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("frmConsultaTranferencias.aspx")
        End If
    End Sub
    Protected Sub imgPgtoCartaoCredito_Click(sender As Object, e As ImageClickEventArgs) Handles imgPgtoCartaoCredito.Click
        'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
        If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
            Response.Redirect("AcessoNegado.aspx")
            Return
        Else
            Response.Redirect("~/Financeiro/frmPagametosCartoesCredito.aspx")
        End If
    End Sub
    Protected Sub imgRelReservasCanceladas_Click(sender As Object, e As ImageClickEventArgs) Handles imgRelReservasCanceladas.Click
        'Acesso ao relatório de reservas canceladas no período
        Response.Redirect("~/InformacoesGerenciais/frmReservasCanceladas.aspx")
    End Sub
End Class
