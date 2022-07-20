Imports System.Data.DataTable
Partial Class InformacoesGerenciais_frmHistoricoPassantes
    Inherits System.Web.UI.Page
    Dim TotalConsumo As Decimal = 0
    Dim TotalHospedes As Long = 0
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
                    btnConsultar.Attributes.Add("AliasBancoTurismo",  "TurismoSocialPiri")
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

            'txtDataIni_CalendarExtender.StartDate = Format(DateAdd(DateInterval.Day, -366, CDate(txtDataFim.Text)), "dd/MM/yyyy")
            'txtDataFim_CalendarExtender.EndDate = Format(DateAdd(DateInterval.Day, 366, CDate(txtDataIni.Text)), "dd/MM/yyyy")

            btnConsultar.Attributes.Add("Data1Tipo", "I")
            btnConsultar.Attributes.Add("Data2Tipo", "F")

            'Formatando datas
            txtDataIni.Attributes.Add("onKeyPress", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)};")
            txtDataIni.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
            txtDataIni.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtDataFim.Attributes.Add("onKeyPress", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)};")
            txtDataFim.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
            txtDataFim.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        If Not (IsDate(CDate(txtDataIni.Text)) Or IsDate(CDate(txtDataFim.Text))) Then
            txtDataIni.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataFim.Text = Format(Date.Today, "dd/MM/yyyy")
        End If

        If DateDiff(DateInterval.Day, CDate(txtDataIni.Text), CDate(txtDataFim.Text)) > 0 And txtPassante.Text.Trim.Length < 4 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção!\n\nPara as consultas que não forem do dia de hoje, se faz necessário a inserção do nome do passante a ser consultado.');", True)
            txtPassante.Focus()
            Return
        End If

        'If txtPassante.Text.Trim.Length < 3 And (CDate(txtDataIni.Text) <> CDate(txtDataFim.Text)) Then
        '    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Digite no mínimo três caracteres no Nome do Passante para efetivar a busca.');", True)
        '    txtPassante.Focus()
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

        Dim Ordenador As String = ""

        Select Case Session.Item("NomeColuna")
            Case "i.IntNome", Nothing
                Ordenador = "i.IntNome " & Session.Item("Ordem")
            Case "IntDtNascimento"
                Ordenador = "i.IntDtNascimento " & Session.Item("Ordem")
            Case "Idade"
                Ordenador = "Idade " & Session.Item("Ordem")
            Case "Categoria"
                Ordenador = "c.CatDescricao " & Session.Item("Ordem")
            Case "DataIniReal"
                Ordenador = "DataIniReal " & Session.Item("Ordem")
            Case "HoraIniReal"
                Ordenador = "HoraIniReal " & Session.Item("Ordem")
            Case "DataFimReal"
                Ordenador = "DataFimReal " & Session.Item("Ordem")
            Case "HoraFimReal"
                Ordenador = "HoraFimReal " & Session.Item("Ordem")
            Case "Situacao"
                Ordenador = "i.IntStatus " & Session.Item("Ordem")
            Case "Atendente"
                Ordenador = "i.IntUsuario " & Session.Item("Ordem")
            Case Else
                Ordenador = "i.intNome " & Session.Item("Ordem")
        End Select
        lblTotPassantes.Text = "0"
        pnlInformacoes.Visible = False
        pnlGridHistorico.Visible = True
        Dim objHistoricoPassantesVO As New HistoricoPassantesVO
        Dim objHistoricoPassantesDAO As New HistoricoPassantesDAO
        'Utilizado para filtrar pelos comerciário de Caldas Novas ou toros
        Dim ComerciarioCaldas As String = ""
        Dim FuncCaldasNovas As String = ""
        If chkCaldasNovas.Checked = True Then
            ComerciarioCaldas = "S"
        Else
            ComerciarioCaldas = "N"
        End If

        If chkFuncionarios.Checked = True Then
            FuncCaldasNovas = "S"
        Else
            FuncCaldasNovas = "N"
        End If

        gdvHistorico.DataSource = objHistoricoPassantesDAO.ConsultaPassantes(objHistoricoPassantesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, txtDataIni.Text, txtDataFim.Text, txtPassante.Text.Trim.Replace(" ", "%"), drpCategoria.SelectedValue, ComerciarioCaldas, drpIdade.SelectedValue, Ordenador, FuncCaldasNovas)
        gdvHistorico.DataBind()
    End Sub

    Protected Sub gdvHistorico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvHistorico.SelectedIndexChanged
        lblInformacoesConsumo.Text = "Informações de Consumo - R$ 0,00"
        pnlInformacoes.Visible = True
        pnlGridHistorico.Visible = False
        Dim ObjHistHspInformacoesVO As New HistoricoHospedesInformacoesVO
        Dim ObjHistHspInformacoesDAO As New HistoricoHospedesInformacoesDAO
        btnConsultar.Attributes.Add("IntId", gdvHistorico.DataKeys(gdvHistorico.SelectedIndex).Item(0).ToString())
        'btnConsultar.Attributes.Add("IntVinculoId", gdvHistorico.DataKeys(gdvHistorico.SelectedIndex).Item("intVinculoId").ToString())
        lblTituloInformacoes.Text = "Informações de " & gdvHistorico.DataKeys(gdvHistorico.SelectedIndex).Item(1).ToString
        'Chamando o primeiro Grid - Cortesias, cartão.
        gdvDadosPassante.DataSource = ObjHistHspInformacoesDAO.ConsultaInformacoesPassante(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvDadosPassante.DataBind()
        'Chamando o terceiro Grid - Informações de Consumo
        gdvConsumo.DataSource = ObjHistHspInformacoesDAO.ConsultaConsumoHospedes(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvConsumo.DataBind()
        'Chamando o quarto Grid - Informações Financeiras'
        gdvFinanceiro.DataSource = ObjHistHspInformacoesDAO.ConsultaFinanceiraHospedes(ObjHistHspInformacoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, btnConsultar.Attributes.Item("IntId"))
        gdvFinanceiro.DataBind()
        'Escondendo o Painel de Consulta'
        pnlConsulta.Visible = False
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
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (gdvHistorico.DataKeys(e.Row.RowIndex).Item("VinculoRefeicao")) <> "" Then
                CType(e.Row.FindControl("imgRefeicao"), Image).Visible = True
                CType(e.Row.FindControl("imgRefeicao"), Image).ToolTip = gdvHistorico.DataKeys(e.Row.RowIndex).Item("VinculoRefeicao").ToString
            Else
                CType(e.Row.FindControl("imgRefeicao"), Image).Visible = False
            End If
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            lblTotPassantes.Text = gdvHistorico.Rows.Count
        End If
    End Sub

    Protected Sub gdvDadosPassante_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvDadosPassante.SelectedIndexChanged
        'Carregando a ficha do hóspede'
        pnlInformacoes.Visible = False
        pnlInformacoesPassante.Visible = True
        lblNome.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(0).ToString
        lblCategoria.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(1).ToString
        lblSituacao.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(20).ToString
        lblPlaca.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(21).ToString
        lblEntrada.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(5).ToString
        lblHoraEntrada.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(6).ToString
        lblSaida.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(7).ToString
        lblHoraSaida.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(8).ToString
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(9).ToString = "S" Then
            lblComerciaCnv.Visible = True
            lblComerciaCnv.Text = "Comerciário de Caldas Novas"
        Else
            lblComerciaCnv.Visible = False
        End If
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(10).ToString = "S" Then
            lblIsento.Visible = True
            lblIsento.Text = "Isento(abaixo de 5 anos)"
        Else
            lblIsento.Visible = False
        End If
        lblRG.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(3).ToString
        lblCPF.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(4).ToString
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(11).ToString = "S" Then
            lblCortesiaCaucao.Visible = True
            lblCortesiaCaucao.Text = "* Caução Cartão"
        Else
            lblCortesiaCaucao.Visible = False
        End If
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(12).ToString = "S" Then
            lblCortesiaPqAquatico.Visible = True
            lblCortesiaPqAquatico.Text = "* Parque Aquático"
        Else
            lblCortesiaPqAquatico.Visible = False
        End If
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(13).ToString = "S" Then
            lblCortesiaLanchonete.Visible = True
            lblCortesiaLanchonete.Text = "* Lanchonetes"
        Else
            lblCortesiaLanchonete.Visible = False
        End If
        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(14).ToString = "S" Then
            lblCortesiaPermPqAuquatico.Visible = True
            lblCortesiaPermPqAuquatico.Text = "* Permanência Parque Aquático"
        Else
            lblCortesiaPermPqAuquatico.Visible = False
        End If

        If gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(15).ToString = "S" Then
            lblCortesiaRestaurante.Visible = True
            lblCortesiaRestaurante.Text = "* Restaurante"
        Else
            lblCortesiaRestaurante.Visible = False
        End If
        lblResponsavelCortesia.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(16).ToString
        lblCategoriaCobranca.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(17).ToString
        txtDocMemorando.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(18).ToString
        lblMotivoEmissor.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(19).ToString
        lblUF.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(22).ToString
        lblCidade.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(23).ToString
        lblMatricula.Text = gdvDadosPassante.DataKeys(gdvDadosPassante.SelectedIndex).Item(2).ToString
    End Sub

    Protected Sub btnVoltarInformacoes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarInformacoes.Click
        pnlInformacoes.Visible = True
        pnlInformacoesPassante.Visible = False

    End Sub

    Protected Sub btnVoltarMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarMenu.Click
        Response.Redirect("frmMenuGerencial.aspx")
    End Sub
    Protected Sub gdvHistorico_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gdvHistorico.Sorting
        Session.Add("NomeColuna", e.SortExpression)
        btnConsultar_Click(sender, e)
    End Sub

    Protected Sub txtDataIni_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataIni.TextChanged
        txtDataFim_CalendarExtender.EndDate = Format(DateAdd(DateInterval.Day, 366, CDate(txtDataIni.Text)), "dd/MM/yyyy")
    End Sub

    Protected Sub txtDataFim_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataFim.TextChanged
        txtDataIni_CalendarExtender.StartDate = Format(DateAdd(DateInterval.Day, -366, CDate(txtDataFim.Text)), "dd/MM/yyyy")
    End Sub
End Class
