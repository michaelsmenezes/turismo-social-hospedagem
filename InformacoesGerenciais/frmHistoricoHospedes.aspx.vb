Imports Turismo
Partial Class InformacoesGerenciais_frmHistoricoHospedes
    Inherits System.Web.UI.Page
    Dim TotalConsumo As Decimal = 0
    Dim TotalHospedes As Long = 0
    Dim objCheckInOutVO As CheckInOutVO
    Dim objCheckInOutDAO As CheckInOutDAO

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheet.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
            End Select
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial") Or Grupos.Contains("CNV_RECEPCAO") Or Grupos.Contains("PSP_Recepcao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            txtDataIni.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataFim.Text = Format(Date.Today, "dd/MM/yyyy")

            btnConsultar.Attributes.Add("Data1Tipo", "I")
            btnConsultar.Attributes.Add("Data2Tipo", "F")

            'Formatando datas
            txtDataIni.Attributes.Add("OnKeyPress", "javascript:return FormataData(this,event);")
            txtDataIni.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataIni.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataFim.Attributes.Add("OnKeyPress", "javascript:return FormataData(this,event);")
            txtDataFim.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataFim.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
        End If
    End Sub

    'Protected Sub RdFinal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdFinal.CheckedChanged
    '    rdCheckOut.Checked = False
    '    btnConsultar.Attributes.Add("Data2Tipo", "F")
    'End Sub

    'Protected Sub rdCheckOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdCheckOut.CheckedChanged
    '    RdFinal.Checked = False
    '    btnConsultar.Attributes.Add("Data2Tipo", "C")
    'End Sub

    'Protected Sub rdInicial_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdInicial.CheckedChanged
    '    rdCkeckIn.Checked = False
    '    btnConsultar.Attributes.Add("Data1Tipo", "I")
    'End Sub

    'Protected Sub rdCkeckIn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdCkeckIn.CheckedChanged
    '    rdInicial.Checked = False
    '    btnConsultar.Attributes.Add("Data1Tipo", "C")
    'End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        btnConsultar.Attributes.Add("Filtrado", False)
        If Not (IsDate(CDate(txtDataIni.Text)) Or IsDate(CDate(txtDataFim.Text))) Then
            txtDataIni.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataFim.Text = Format(Date.Today, "dd/MM/yyyy")
        End If

        If DateDiff(DateInterval.Day, CDate(txtDataIni.Text), CDate(txtDataFim.Text)) > 0 _
            And (txtHospede.Text.Trim.Length = 0 And txtAptoPlaca.Text.Trim.Length = 0) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção!\n\nSe a consulta não for do dia de hoje, se faz necessário a inserção do nome do hóspede ou Apartamento/placa do veículo a ser consultado.');", True)
            txtHospede.Focus()
            Return
        End If

        'If txtHospede.Text.Trim.Length < 4 And (CDate(txtDataIni.Text) <> CDate(txtDataFim.Text)) Then
        '    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Digite no mínimo três caracteres no Nome do Hóspede para efetivar a busca.');", True)
        '    txtHospede.Focus()
        '    Return
        'End If
        Select Case Session.Item("Ordem")
            Case "Desc"
                Session.Add("Ordem", "Asc")
            Case "Asc"
                Session.Add("Ordem", "Desc")
            Case Else
                Session.Add("Ordem", "Asc")
        End Select

        Select Case Session.Item("NomeColuna")
            Case Is = "IntNome"
                Session.Add("NomeColuna", "i.IntNome " & Session.Item("Ordem"))
            Case Is = "ApaDesc"
                Session.Add("NomeColuna", "a.ApaId " & Session.Item("Ordem"))
            Case Is = "I.IntDataIni"
                Session.Add("NomeColuna", "I.IntDataIni " & Session.Item("Ordem"))
            Case Is = "HoraIni"
                Session.Add("NomeColuna", "HoraIni " & Session.Item("Ordem"))
            Case Is = "I.IntDataIniReal"
                Session.Add("NomeColuna", "I.IntDataIniReal " & Session.Item("Ordem"))
            Case Is = "HoraIniReal"
                Session.Add("NomeColuna", "HoraIniReal " & Session.Item("Ordem"))
            Case Is = "I.IntDataFim"
                Session.Add("NomeColuna", "I.IntDataFim " & Session.Item("Ordem"))
            Case Is = "HoraFim"
                Session.Add("NomeColuna", "HoraFim " & Session.Item("Ordem"))
            Case Is = "I.IntDataFimReal"
                Session.Add("NomeColuna", "I.IntDataFimReal " & Session.Item("Ordem"))
            Case Is = "HoraFimReal"
                Session.Add("NomeColuna", "HoraFimReal " & Session.Item("Ordem"))
            Case Is = "ResNome"
                Session.Add("NomeColuna", "r.ResNome " & Session.Item("Ordem"))
            Case Is = Nothing
                Session.Add("NomeColuna", "i.IntNome " & Session.Item("Ordem"))
        End Select

        lblTotHospedes.Text = "0"
        pnlInformacoes.Visible = False
        pnlGridHistorico.Visible = True
        Dim objHistoricoHospedeVO As New HistoricoHospedesVO
        Dim objHistoricoHospedeDAO As New HistoricoHospedesDAO
        gdvHistorico.DataSource = objHistoricoHospedeDAO.Consultar(objHistoricoHospedeVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, txtDataIni.Text, txtDataFim.Text, txtHospede.Text.Trim.Replace(" ", "%"), txtAptoPlaca.Text, btnConsultar.Attributes.Item("Data1Tipo").ToString, btnConsultar.Attributes.Item("Data2Tipo").ToString, 0, chkServidores.Checked, Session.Item("NomeColuna"))
        gdvHistorico.DataBind()
    End Sub

    Protected Sub gdvHistorico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvHistorico.SelectedIndexChanged
        lblInformacoesConsumo.Text = "Informações de Consumo - R$ 0,00"
        pnlInformacoes.Visible = True
        pnlGridHistorico.Visible = False
        Dim ObjHistHspInformacoesVO As New HistoricoHospedesInformacoesVO
        Dim ObjHistHspInformacoesDAO As New HistoricoHospedesInformacoesDAO
        btnConsultar.Attributes.Add("IntId", gdvHistorico.DataKeys(gdvHistorico.SelectedIndex).Item(0).ToString())
        lblTituloInformacoes.Text = "Informações de " & gdvHistorico.DataKeys(gdvHistorico.SelectedIndex).Item(2).ToString
        'Chamando o primeiro Grid - Cortesias, cartão.
        gdvSituacaoReserva.DataSource = ObjHistHspInformacoesDAO.ConsultaSituacaoCortesia(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvSituacaoReserva.DataBind()
        'Chamando o Segundo Grid - Informações Hospedagem
        gdvInfHospedagem.DataSource = ObjHistHspInformacoesDAO.ConsultaInformacoesHospedagem(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvInfHospedagem.DataBind()
        'Chamando o terceiro Grid - Informações de Consumo
        gdvConsumo.DataSource = ObjHistHspInformacoesDAO.ConsultaConsumoHospedes(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvConsumo.DataBind()
        'Chamando o quarto Grid - Informações Financeiras'
        gdvFinanceiro.DataSource = ObjHistHspInformacoesDAO.ConsultaFinanceiraHospedes(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvFinanceiro.DataBind()
        'Chamando o quarto Grid - Informações de Log'
        gdvLog.DataSource = ObjHistHspInformacoesDAO.ConsultaLogEventosHospedes(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvLog.DataBind()
        'Escondendo o Painel de Consulta'
        pnlConsulta.Visible = False
        'Histórico de enxoval'
        objCheckInOutVO = New CheckInOutVO
        objCheckInOutDAO = New CheckInOutDAO(btnConsultar.Attributes.Item("AliasBancoTurismo").ToString)
        gdvHistoricoEnxoval.DataSource = objCheckInOutDAO.ConsultaEnxovaisHistoricoHospedes(btnConsultar.Attributes.Item("IntId"), btnConsultar.Attributes.Item("AliasBancoTurismo").ToString)
        gdvHistoricoEnxoval.DataBind()
        If gdvHistoricoEnxoval.Rows.Count = 0 Then
            divHistoricoEnxoval.Visible = False
        Else
            divHistoricoEnxoval.Visible = True
        End If
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        pnlInformacoes.Visible = False
        pnlGridHistorico.Visible = True
        pnlConsulta.Visible = True
    End Sub

    Protected Sub gdvConsumo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvConsumo.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            TotalConsumo = TotalConsumo + CDec(gdvConsumo.DataKeys(e.Row.RowIndex).Item("ValorTotal").ToString)
            'gdvReserva1.DataKeys(e.Row.RowIndex).Item("intPlacaVeiculo").ToString()
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            lblInformacoesConsumo.Text = "Informações de Consumo - R$ " & TotalConsumo
        End If
    End Sub

    Protected Sub btnVoltarBaixo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarBaixo.Click
        btnVoltar_Click(sender, e)
    End Sub

    Protected Sub gdvHistorico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvHistorico.RowDataBound
        If e.Row.RowType = DataControlRowType.Footer Then
            lblTotHospedes.Text = gdvHistorico.Rows.Count
        End If
    End Sub

    Protected Sub btnVoltarMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarMenu.Click
        Response.Redirect("frmMenuGerencial.aspx")
    End Sub

    Protected Sub gdvHistorico_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvHistorico.Sorting
        Session.Add("NomeColuna", e.SortExpression)
        btnConsultar_Click(sender, e)
    End Sub

    Protected Sub gdvHistoricoEnxoval_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvHistoricoEnxoval.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Select Case e.Row.Cells(11).Text
                Case "E"
                    e.Row.Cells(11).Text = "Apagado"
                Case "I"
                    e.Row.Cells(11).Text = "Inserção"
                Case "A"
                    e.Row.Cells(11).Text = ""
            End Select
        End If
    End Sub

    Protected Sub imgFiltraReserva_Click(sender As Object, e As ImageClickEventArgs)
        If btnConsultar.Attributes.Item("Filtrado") = False Then
            btnConsultar.Attributes.Add("Filtrado", True)
            Dim imgFiltro As ImageButton = sender
            Dim row As GridViewRow = imgFiltro.NamingContainer 'Dim index As Integer = row.RowIndex
            Dim ResId = CInt(gdvHistorico.DataKeys(row.RowIndex()).Item("ResId").ToString())

            Dim objHistoricoHospedeVO As New HistoricoHospedesVO
            Dim objHistoricoHospedeDAO As New HistoricoHospedesDAO
            gdvHistorico.DataSource = objHistoricoHospedeDAO.Consultar(btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, ResId)
            gdvHistorico.DataBind()
        Else
            btnConsultar.Attributes.Add("Filtrado", False)
            btnConsultar_Click(sender, e)
        End If
    End Sub
End Class
