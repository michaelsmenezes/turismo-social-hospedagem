Imports Turismo
Imports Geral
Imports FPW

Partial Class Emissivo_frmEmissivo
    Inherits System.Web.UI.Page
    Dim objPasseioVO As PasseioVO
    Dim ObjPasseioDAO As PasseioDAO
    Dim objTestaGrupo As New Uteis.TestaUsuario
    Dim Grupos As String = objTestaGrupo.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
    Dim objReservaListagemSolicitacaoVO As ReservaListagemSolicitacaoVO
    Dim objReservaListagemSolicitacaoDAO As ReservaListagemSolicitacaoDAO
    Dim objReservaListagemAcomodacaoDAO As Turismo.ReservaListagemAcomodacaoDAO
    Dim objReservaListagemAcomodacaoVO As Turismo.ReservaListagemAcomodacaoVO
    Dim objReservaListagemIntegranteDAO As Turismo.ReservaListagemIntegranteDAO
    Dim objReservaListagemIntegranteVO As New Turismo.ReservaListagemIntegranteVO
    Dim objReservaListagemFinanceiroDAO As Turismo.ReservaListagemFinanceiroDAO
    Dim objReservaListagemFinanceiroVO As Turismo.ReservaListagemFinanceiroVO
    Dim objEstadoDAO As Geral.BdProdEstadoDAO
    Dim objMunicipioDAO As Geral.BdProdMunicipioDAO
    Dim objMunicipioVO As Geral.BdProdMunicipioVO
    Dim objPasseioDestinoDAO As Turismo.PasseioDestinoDAO
    Dim objPasseioDestinoVO As Turismo.PasseioDestinoVO
    Dim objFuncionarioDAO As FPW.FuncionarioDAO
    Dim objFuncionarioVO As FPW.FuncionarioVO
    Dim objRefeicaoPratoDAO As Turismo.RefeicaoPratoDAO
    Dim objRefeicaoPratoVO As Turismo.RefeicaoPratoVO
    Dim objOrgaoDAO As OrgaoDAO
    Dim objOrgaoVO As OrgaoVO
    Dim ObjReservaVO As New Turismo.ReservaVO
    Dim ObjReservaDAO As New Turismo.ReservaDAO("")
    Dim lista As New ArrayList
    Dim objCategoriaDAO As Turismo.CategoriaDAO
    Dim objCategoriaVO As Turismo.CategoriaVO
    Dim objUsuarioGrupoDAO As Turismo.UsuarioGrupoDAO
    Dim objUsuarioGrupoVO As Turismo.UsuarioGrupoVO

    Dim objReservaEmissivoVO As ReservaEmissivoVO
    Dim objReservaEmissivoDAO As ReservaEmissivoDAO
    Private totHospede As Short
    Private totApto As Short
    Private totalValor As Decimal
    Private totalPago As Decimal
    Private cont As Integer
    Protected Sub drpConTipoPasseio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpConTipoPasseio.SelectedIndexChanged
        If drpConTipoPasseio.SelectedIndex = 1 Then
            lblStatusPasseio.Text = "Consultar passeios"
            txtConNomePasseio.Focus()
        ElseIf drpConTipoPasseio.SelectedIndex = 2 Then
            lblStatusPasseio.Text = "Consultar excursões"
            txtConNomePasseio.Focus()
        Else
            lblStatusPasseio.Text = "Consultar passeios e excursões"
            txtConNomePasseio.Focus()
        End If
    End Sub

    Protected Sub drpResTipoPasseio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResTipoPasseio.SelectedIndexChanged
        If drpResTipoPasseio.SelectedIndex = 0 Then 'Emisivo Laercio
            lblDadosPasseio.Text = "Dados do passeio"
            lblDadosDataInicialPasseio.Text = "Data"
            lblDadosDataFinalPasseio.Visible = False
            txtResDataFinalPasseio.Visible = False
            drpResNomePasseio.Focus()
            pnlCamposGrupo.Visible = False
            txtResConfirmar.Text = ""
            txtResPagarSinal.Text = ""
            txtResDigitar.Text = ""
            txtResQuitar.Text = ""
            drpResUsuarioInternet.SelectedIndex = 0
            divResDadosHotelSaidaHorasSaida.Visible = True
            divResLocalRefeicao.Visible = True
            drpResLocalRefeicaoPadrao.SelectedIndex = 0
            drpResLocalRefeicaoPadrao_SelectedIndexChanged(sender, e)
        ElseIf drpResTipoPasseio.SelectedIndex = 1 Then 'Excursão Eduardo
            lblDadosPasseio.Text = "Dados da excursão"
            lblDadosDataInicialPasseio.Text = "Data inicial do passeio"
            lblDadosDataFinalPasseio.Text = "Data final do passeio"
            lblDadosDataFinalPasseio.Visible = True
            txtResDataFinalPasseio.Visible = True
            drpResNomePasseio.Focus()
            pnlCamposGrupo.Visible = False
            txtResConfirmar.Text = ""
            txtResPagarSinal.Text = ""
            txtResDigitar.Text = ""
            txtResQuitar.Text = ""
            drpResUsuarioInternet.SelectedIndex = 0
            divResDadosHotelSaidaHorasSaida.Visible = True
            divResLocalRefeicao.Visible = True
            drpResLocalRefeicaoPadrao.SelectedIndex = 0
            drpResLocalRefeicaoPadrao_SelectedIndexChanged(sender, e)
        ElseIf drpResTipoPasseio.SelectedIndex = 2 Then 'Grupo (Luzia)
            lblDadosPasseio.Text = "Dados do grupo"
            lblDadosDataInicialPasseio.Text = "Data inicia do grupo"
            lblDadosDataFinalPasseio.Text = "Data final do grupo"
            lblDadosDataFinalPasseio.Visible = True
            txtResDataFinalPasseio.Visible = True
            pnlCamposGrupo.Visible = True
            divResDadosHotelSaidaHorasSaida.Visible = False
            divResLocalRefeicao.Visible = False
        End If
        drpResNomePasseio.Focus()
    End Sub

    Protected Sub btnConNovo_Click(sender As Object, e As EventArgs) Handles btnConNovo.Click
        hddResId.Value = 0
        pnlConConsultarPasseio.Visible = False
        pnlDadosPasseio.Visible = True
        drpResTipoPasseio.Focus()
        'Dim ObjCheckInOutVO As New CheckInOutVO
        'Dim ObjCheckInOutDAO As New AptoCheckInOutDAO("TurismoSocialCaldas")
        objPasseioVO = New PasseioVO()
        ObjPasseioDAO = New PasseioDAO

        drpResNomePasseio.DataValueField = "pasId"
        drpResNomePasseio.DataTextField = "pasNomePasseio"
        btnAdicionaNomePasseio.Text = "Adiciona"
        divSalvarNovoPasseio.Attributes("class") = ""
        drpResNomePasseio.Items.Clear()
        drpResNomePasseio.DataSource = ObjPasseioDAO.ConsultaNomePasseio("", "TurismoSocialCaldas")
        drpResNomePasseio.DataBind()
        drpResNomePasseio.Items.Insert(0, New ListItem("Selecione...", "0"))

        CarregaEstado()

        'Carrega os servidores da central de reservas no combo
        objFuncionarioDAO = New FPW.FuncionarioDAO()
        drpResResponsavel.DataValueField = "FUMatFunc"
        drpResResponsavel.DataTextField = "FUNomFunc"
        drpResResponsavel.DataSource = objFuncionarioDAO.consultarPorCentroCusto("452','456")
        drpResResponsavel.DataBind()
        drpResResponsavel.Items.Insert(0, New ListItem("Selecione...", "0"))
        txtResResponsavel.Visible = True
        drpResResponsavel.Visible = False

        btnConConsultar.Attributes.Add("Acao", "NovoPasseio")

        'Limpando os campos
        txtResNomePasseio.Text = ""
        txtResDataInicialPasseio.Text = Format(Date.Today, "dd/MM/yyyy")
        txtResDataFinalPasseio.Text = Format(Date.Today, "dd/MM/yyyy")
        chkDadosOrganizadoSesc.Checked = False
        txtResTelComercial.Text = ""
        txtResTelResidencial.Text = ""
        txtResTelCelular.Text = ""
        drpResCategoria.SelectedIndex = 0
        drpResCategoriaCobranca.SelectedIndex = 0
        txtResMemorando.Text = ""
        drpResEmissor.SelectedIndex = 0
        txtResObservacao.Text = ""
        drpResLocalRefeicaoPadrao.SelectedIndex = 0
        txtResHotel.Text = ""
        drpResHoraSaida.SelectedIndex = 0
        drpResTipoRefeicaoPadrao.SelectedIndex = 0
        drpResEstado.SelectedValue = 9
        drpResCidade.SelectedIndex = 0


        'drpResNomePasseio.SelectedItem.Text = txtResNomePasseio.Text
        'drpResNomePasseio.DataSource = ObjCheckInOutDAO.consultar("618265")
        'drpResNomePasseio.DataBind()
    End Sub

    Protected Sub btnAdicionaNomePasseio_Click(sender As Object, e As EventArgs) Handles btnAdicionaNomePasseio.Click

        divBotoesNovoPasseio.Attributes("class") = "col-md-2"
        divSalvarNovoPasseio.Attributes("class") = "col-md-6"
        divCancelaNovoPasseio.Attributes("class") = "col-md-6"
        divCancelaNovoPasseio.Visible = True


        If btnAdicionaNomePasseio.Text = "Salvar" Then
            objPasseioVO = New PasseioVO()
            ObjPasseioDAO = New PasseioDAO
            If txtResNomePasseio.Text.Trim.Length < 3 Then
                Mensagem("Informe o nome do passeio antes de salvar.")
                txtResNomePasseio.Focus()
                Return
            End If



            Select Case ObjPasseioDAO.InsereNovoNomePasseio(0, txtResNomePasseio.Text, "TurismoSocialCaldas")
                Case 1
                    btnAdicionaNomePasseio.Text = "Adiciona"
                    drpResNomePasseio.Items.Clear()
                    drpResNomePasseio.DataSource = ObjPasseioDAO.ConsultaNomePasseio("", "TurismoSocialCaldas")
                    drpResNomePasseio.DataBind()
                    drpResNomePasseio.Items.Insert(0, New ListItem("Selecione...", "0"))
                    'drpResNomePasseio.SelectedItem.Text = txtResNomePasseio.Text
                    txtResNomePasseio.Text = ""
                    drpResNomePasseio.Visible = True
                    txtResNomePasseio.Visible = False
                    divCancelaNovoPasseio.Visible = False
                    divBotoesNovoPasseio.Attributes("class") = "col-md-1"
                    divSalvarNovoPasseio.Attributes("class") = "col-md-06"
                    divCancelaNovoPasseio.Attributes("class") = ""

                    drpResNomePasseio.Focus()
                Case 2
                    btnAdicionaNomePasseio.Text = "Adiciona"
                    drpResNomePasseio.Items.Clear()
                    drpResNomePasseio.DataSource = ObjPasseioDAO.ConsultaNomePasseio("", "TurismoSocialCaldas")
                    drpResNomePasseio.DataBind()
                    drpResNomePasseio.Items.Insert(0, New ListItem("Selecione...", "0"))
                    'drpResNomePasseio.SelectedItem.Text = txtResNomePasseio.Text
                    txtResNomePasseio.Text = ""
                    drpResNomePasseio.Visible = True
                    txtResNomePasseio.Visible = False
                    divCancelaNovoPasseio.Visible = False
                    divBotoesNovoPasseio.Attributes("class") = "col-md-1"
                    divSalvarNovoPasseio.Attributes("class") = "col-md-06"
                    divCancelaNovoPasseio.Attributes("class") = ""
                    drpResNomePasseio.Focus()
                Case 3
                    txtResNomePasseio.Text = ""
                    drpResNomePasseio.Visible = True
                    txtResNomePasseio.Visible = False
                    divCancelaNovoPasseio.Visible = False
                    divBotoesNovoPasseio.Attributes("class") = "col-md-1"
                    divSalvarNovoPasseio.Attributes("class") = "col-md-06"
                    divCancelaNovoPasseio.Attributes("class") = ""
                    drpResNomePasseio.Focus()
            End Select
        ElseIf btnAdicionaNomePasseio.Text = "Adiciona" Then
            drpResNomePasseio.Visible = False
            txtResNomePasseio.Visible = True
            divCancelaNovoPasseio.Visible = True
            txtResNomePasseio.Text = ""
            txtResNomePasseio.Focus()
            btnAdicionaNomePasseio.Text = "Salvar"
        End If


        'pnlCadastroNomePasseio.Visible = True
        'pnlDadosPasseio.CssClass = "AplicaTransparencia"
        'pnlCadastroNomePasseio.CssClass = "PosicionaFinalTelaCentralizado"
        'pnlDadosPasseio.CssClass = "DivFundoTransparencia"
    End Sub

    Protected Sub btnVoltaDadosPasseio_Click(sender As Object, e As EventArgs) Handles btnVoltaDadosPasseio.Click
        pnlConConsultarPasseio.Visible = True
        If btnConConsultar.Attributes.Item("Acao") = "gdvResponsavel" Then
            gdvConReservas.Visible = True
            pnlDadosPasseio.Visible = False
            pnlConConsultarPasseio.Visible = True
        ElseIf btnConConsultar.Attributes.Item("Acao") = "NovoPasseio" Then
            gdvConReservas.Visible = False
            pnlDadosPasseio.Visible = False
        Else
            gdvConReservas.Visible = False
            pnlDadosPasseio.Visible = False
            pnlConConsultarPasseio.Visible = True
        End If
        drpConTipoPasseio.Focus()
        btnConConsultar.Attributes.Remove("Acao")
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Mudando o valor do button ao digitar algo na caixa de texto
            txtResNomePasseio.Attributes.Add("OnKeyUp", "javascript:if(this.value == ''){aspnetForm.ctl00_conPlaHolTurismoSocial_btnAdicionaNomePasseio.value='Cancelar'}else{aspnetForm.ctl00_conPlaHolTurismoSocial_btnAdicionaNomePasseio.value='Salvar'}")

            txtConDataInicial.Text = Format(CDate(Today.Date), "dd/MM/yyyy")
            txtConDataFinal.Text = Format(CDate(Today.Date), "dd/MM/yyyy")

            'Preenchendo as listas de categoria

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objCategoriaDAO = New CategoriaDAO("TurismoSocialCaldas")
                objOrgaoDAO = New OrgaoDAO("TurismoSocialCaldas")
            Else
                objCategoriaDAO = New CategoriaDAO("TurismoSocialPiri")
                objOrgaoDAO = New OrgaoDAO("TurismoSocialPiri")
            End If

            'txtDataFinalSolicitacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            'txtDataFinalSolicitacao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            'txtDataFinalSolicitacao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")


            lista = objCategoriaDAO.consultar("Reserva")
            'cmbCategoriaReserva.DataSource = lista
            'cmbCategoriaReserva.DataValueField = ("catId")
            'cmbCategoriaReserva.DataTextField = ("catDescricao")
            'cmbCategoriaReserva.DataBind()
            'cmbCategoriaReserva.Items.Insert(0, New ListItem("Todas", "0"))
            drpResCategoria.DataSource = lista
            drpResCategoria.DataValueField = ("catId")
            drpResCategoria.DataTextField = ("catDescricao")
            drpResCategoria.DataBind()
            'cmbResCatId.DataSource = lista
            'cmbResCatId.DataValueField = ("catId")
            'cmbResCatId.DataTextField = ("catDescricao")
            'cmbResCatId.DataBind()
            Dim listaAux As New ArrayList
            For Each item As Turismo.CategoriaVO In lista
                If item.catLink = item.catLinkCat Then
                    listaAux.Add(item)
                End If
            Next
            drpResCategoriaCobranca.DataSource = listaAux
            drpResCategoriaCobranca.DataValueField = ("catId")
            drpResCategoriaCobranca.DataTextField = ("catDescricao")
            drpResCategoriaCobranca.DataBind()

            txtResTelCelular.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResTelCelular.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtResTelComercial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResTelComercial.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtResTelResidencial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResTelResidencial.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            'Formatando os campos de data
            txtConDataInicial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtConDataInicial.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtConDataInicial.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtConDataFinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtConDataFinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtConDataFinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDataInicialPasseio.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDataInicialPasseio.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDataInicialPasseio.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDataFinalPasseio.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDataFinalPasseio.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDataFinalPasseio.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResConfirmar.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResConfirmar.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResConfirmar.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResPagarSinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResPagarSinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResPagarSinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDigitar.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDigitar.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDigitar.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResQuitar.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResQuitar.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResQuitar.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            'Carrega lista de destino se terá refeição ou não
            CarregaCmbDestino()

            'Preenchendo 
            lista = objOrgaoDAO.consultar(Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
            drpResLocalSaida.DataSource = lista
            drpResLocalSaida.DataValueField = ("orgId")
            drpResLocalSaida.DataTextField = ("orgDescricao")
            drpResLocalSaida.DataBind()
            drpResLocalSaida.Items.Insert("0", New ListItem("Selecione...", "0"))
            'Carrega lista de refeições
            CarregaCmbRefeicaoPrato()
            'Carrega usuário de internet
            CarregaCmbResIdWeb()
        End If
    End Sub

    Protected Sub btnConConsultar_Click(sender As Object, e As EventArgs) Handles btnConConsultar.Click
        'Try
        Dim Lista As New ArrayList
        'If (sender.CommandArgument = "0") Then
        '    hddResId.Value = 0
        '    gdvReserva2.DataSource = Nothing
        '    gdvReserva2.DataBind()
        '    gdvReserva3.DataSource = Nothing
        '    gdvReserva3.DataBind()
        '    gdvReserva4.DataSource = Nothing
        '    gdvReserva4.DataBind()
        '    pnlPlanilha.Visible = False
        'End If
        'Catch ex As Exception
        'End Try

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If
        If txtConNomePasseio.Text > "" And CDate(txtConDataFinal.Text) = DateAdd(DateInterval.Day, 1, Now.Date) Then
            Lista = objReservaListagemSolicitacaoDAO.consultar("0", drpConTipoPasseio.SelectedValue, _
              "0", drpConSituacao.SelectedValue, Format(CDate(txtConDataInicial.Text), "dd/MM/yyyy"), _
              Format(CDate(DateAdd(DateInterval.Year, 1, CDate(txtConDataFinal.Text)).ToString), "dd/MM/yyyy").Substring(0, 10), _
              txtConNomePasseio.Text, "resNome", "0", "")
        Else
            Lista = objReservaListagemSolicitacaoDAO.consultar("0", drpConTipoPasseio.SelectedValue, _
              "0", drpConSituacao.SelectedValue, Format(CDate(txtConDataInicial.Text), "dd/MM/yyyy"), _
              Format(CDate(txtConDataFinal.Text), "dd/MM/yyyy"), _
              txtConNomePasseio.Text, "resNome", "0", "")
        End If

        Dim listaAux As New ArrayList
        For Each item As Turismo.ReservaListagemSolicitacaoVO In Lista
            If ((item.orgGrupo = "F") And Session("GrupoGP")) Or _
               ((item.orgGrupo = "R") And Session("GrupoDR")) Or _
               ((item.orgGrupo = "C") And Session("GrupoCerec")) Or _
               ((item.orgGrupo = "P") And Session("GrupoCerec")) Or _
               ((item.orgGrupo = "P") And Session("GrupoEmissivo")) Then
                listaAux.Add(item)
            End If
        Next

        'lblReserva.Text = "Reservas"
        'lblIntegrante.Text = "Integrantes"
        'lblAcomodacao.Text = "Acomodações"
        'lblFinanceiro.Text = "Pagamentos"

        gdvConReservas.DataSource = listaAux
        gdvConReservas.DataBind()
        gdvConReservas.Visible = True
        pnlDadosPasseio.Visible = False
        'If Lista.Count = 0 Then
        '    pnlReserva.Focus()
        'Else
        '    gdvReserva1.Focus()
        'End If
        'If imgBtnReservaMaximizar.Attributes.Item("TelaCheia") <> "S" Then
        '    pnlAcomodacao.Visible = pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
        'End If
        'hddConsulta.Value = 1
    End Sub

    Protected Sub gdvConReservas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvConReservas.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            cont = 0
            totalPago = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkResponsavel"), LinkButton).Text = gdvConReservas.DataKeys(e.Row.RowIndex()).Item("resNome").ToString

            'totHospede += CType(e.Row.FindControl("lnkIntegrante"), LinkButton).Text
            'totApto += CType(e.Row.FindControl("lnkApto"), LinkButton).Text
            cont += 1
            totalPago += gdvConReservas.DataKeys(e.Row.RowIndex).Item("resValorPago")
            e.Row.Cells(5).Text = FormatNumber(e.Row.Cells(5).Text, 2)
            CType(e.Row.FindControl("imgUsuario"), Image).ToolTip = gdvConReservas.DataKeys(e.Row.RowIndex).Item("ResUsuario").ToString
            CType(e.Row.FindControl("lblPeriodo"), Label).Text = _
              gdvConReservas.DataKeys(e.Row.RowIndex).Item("resDataIni").ToString + " à " + gdvConReservas.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString
            '"<br>" + gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString

            'Ira inserir no tooltip a data de criação da reserva (Pedido do Marcelo)
            If gdvConReservas.DataKeys(e.Row.RowIndex).Item("resStatus").ToString <> "C" Then
                CType(e.Row.FindControl("lblPeriodo"), Label).ToolTip = "Criada em........: " & gdvConReservas.DataKeys(e.Row.RowIndex).Item("resDtInsercao").ToString _
                    & Chr(10) & "Limite Retorno: " & gdvConReservas.DataKeys(e.Row.RowIndex).Item("resDtLimiteRetorno").ToString
            End If

            'Quando for cancelada irá mostrar a dat de Criação da reserva e data e quem a cancelou (Pedido por Pollyana)
            If gdvConReservas.DataKeys(e.Row.RowIndex).Item("resStatus").ToString = "C" Then
                CType(e.Row.FindControl("lblPeriodo"), Label).ToolTip = "Criada em " & gdvConReservas.DataKeys(e.Row.RowIndex).Item("resDtInsercao").ToString & Chr(10) & "Cancelada por: " & gdvConReservas.DataKeys(e.Row.RowIndex).Item("ResUsuario").ToString & " em " _
                    & gdvConReservas.DataKeys(e.Row.RowIndex).Item("ResUsuarioData").ToString
            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(3).Text = "Total"
            e.Row.Cells(4).Text = "Registros " & cont
            e.Row.Cells(5).Text = "Total R$ " & FormatNumber(totalPago, 2)
            'lblReserva.Text = "Reservas " & cont.ToString
        End If
    End Sub
    Protected Sub Mensagem(Texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + Texto + "');", True)
    End Sub
    Protected Sub CarregaEstado()
        If drpResEstado.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
            Else
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
            End If

            'Session("ListagemEstado") = objEstadoDAO.consultarEstadoPais
            'Session("ListagemEstadoSemCA") = objEstadoDAO.consultar("CA")

            lista = objEstadoDAO.consultarEstadoPais
            drpResEstado.DataSource = lista
            drpResEstado.DataValueField = ("estId")
            drpResEstado.DataTextField = ("estDescricao")
            drpResEstado.DataBind()
            drpResEstado.SelectedIndex = drpResEstado.Items.IndexOf(drpResEstado.Items.FindByText(" GOIÁS"))

            lista = objMunicipioDAO.consultarCidadePorEstado(drpResEstado.SelectedValue)
            drpResCidade.DataSource = lista
            drpResCidade.DataValueField = ("munDescricao")
            drpResCidade.DataTextField = ("munDescricao")
            drpResCidade.DataBind()
            drpResCidade.Text = Mid(drpResCidade.Text.Trim.ToUpper, 1, 40)

            'cmbDestinoCidade.DataSource = lista
            'cmbDestinoCidade.DataValueField = ("munDescricao")
            'cmbDestinoCidade.DataTextField = ("munDescricao")
            'cmbDestinoCidade.DataBind()

        End If
    End Sub

    Protected Sub CarregaCidadePorEstado()
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
            objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
        Else
            objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
            objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
        End If
        lista = objMunicipioDAO.consultarCidadePorEstado(drpResEstado.SelectedValue)
        drpResCidade.DataSource = lista
        drpResCidade.DataValueField = ("munDescricao")
        drpResCidade.DataTextField = ("munDescricao")
        drpResCidade.DataBind()
        drpResCidade.Text = Mid(drpResCidade.Text.Trim.ToUpper, 1, 40)
    End Sub

    Protected Sub drpResEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResEstado.SelectedIndexChanged
        CarregaCidadePorEstado()
        drpResCidade.Focus()
    End Sub
    Protected Sub CarregaCategoria()
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCategoriaDAO = New CategoriaDAO("TurismoSocialCaldas")
        Else
            objCategoriaDAO = New CategoriaDAO("TurismoSocialPiri")
        End If

        lista = objCategoriaDAO.consultar("Reserva")
        'cmbCategoriaReserva.DataSource = lista
        'cmbCategoriaReserva.DataValueField = ("catId")
        'cmbCategoriaReserva.DataTextField = ("catDescricao")
        'cmbCategoriaReserva.DataBind()
        'cmbCategoriaReserva.Items.Insert(0, New ListItem("Todas", "0"))
        If drpResCategoria.Items.Count = 0 Then
            drpResCategoria.DataSource = lista
            drpResCategoria.DataValueField = ("catId")
            drpResCategoria.DataTextField = ("catDescricao")
            drpResCategoria.DataBind()
            drpResCategoria.Items.Insert(0, New ListItem("Todas", "0"))
        End If
        'cmbResCatId.DataSource = lista
        'cmbResCatId.DataValueField = ("catId")
        'cmbResCatId.DataTextField = ("catDescricao")
        'cmbResCatId.DataBind()
        Dim listaAux As New ArrayList
        For Each item As Turismo.CategoriaVO In lista
            If item.catLink = item.catLinkCat Then
                listaAux.Add(item)
            End If
        Next
        drpResCategoriaCobranca.DataSource = listaAux
        drpResCategoriaCobranca.DataValueField = ("catId")
        drpResCategoriaCobranca.DataTextField = ("catDescricao")
        drpResCategoriaCobranca.DataBind()
    End Sub
    Protected Sub CarregaCmbDestino()
        If drpResLocalRefeicaoPadrao.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objPasseioDestinoDAO = New Turismo.PasseioDestinoDAO("TurismoSocialCaldas")
            Else
                objPasseioDestinoDAO = New Turismo.PasseioDestinoDAO("TurismoSocialPiri")
            End If
            lista = objPasseioDestinoDAO.consultar()
            drpResLocalRefeicaoPadrao.DataSource = lista
            drpResLocalRefeicaoPadrao.DataValueField = ("PasseioDestinoRefeicao")
            drpResLocalRefeicaoPadrao.DataTextField = ("PasseioDestinoDescricao")
            drpResLocalRefeicaoPadrao.DataBind()
        End If
    End Sub

    'Protected Sub btnConsultarResponsavel_Click(sender As Object, e As EventArgs) Handles btnConsultarResponsavel.Click
    '    If drpResResponsavel.Visible = True And drpResResponsavel.SelectedIndex = -1 Then
    '        txtResResponsavel.Visible = True
    '        drpResResponsavel.Visible = False
    '        btnConsultarResponsavel.Text = "Consultar"
    '        txtResResponsavel.Focus()
    '    ElseIf drpResResponsavel.SelectedIndex = -1 And btnConsultarResponsavel.Text = "Consultar" Then
    '        objFuncionarioDAO = New FPW.FuncionarioDAO()
    '        drpResResponsavel.DataValueField = "FUMatFunc"
    '        drpResResponsavel.DataTextField = "FUNomFunc"
    '        drpResResponsavel.DataSource = objFuncionarioDAO.consultarPorCentroCusto("452','456")
    '        drpResResponsavel.DataBind()


    '        txtResResponsavel.Visible = False
    '        drpResResponsavel.Visible = True
    '        btnConsultarResponsavel.Text = "Voltar"
    '        drpResResponsavel.Focus()
    '    Else
    '        txtResResponsavel.Visible = False
    '        drpResResponsavel.Visible = True
    '        btnConsultarResponsavel.Text = "Consultar"
    '        drpResResponsavel.Focus()
    '    End If
    'End Sub

    Protected Sub drpResResponsavel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResResponsavel.SelectedIndexChanged
        txtResResponsavel.Visible = True
        drpResResponsavel.Visible = False
        txtResResponsavel.Text = drpResResponsavel.SelectedItem.ToString
        txtResTelComercial.Focus()
    End Sub

    Protected Sub chkDadosOrganizadoSesc_CheckedChanged(sender As Object, e As EventArgs) Handles chkDadosOrganizadoSesc.CheckedChanged
        If chkDadosOrganizadoSesc.Checked Then
            drpResResponsavel.Visible = True
            txtResResponsavel.Visible = False
            drpResResponsavel.Focus()
        Else
            drpResResponsavel.Visible = False
            txtResResponsavel.Visible = True
            txtResResponsavel.Focus()
        End If
    End Sub

    Protected Sub CarregaDadosReserva()
        Try
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
            Else
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
            End If


            'Com com os Estados para uso no carregamento dos dados.
            drpResEstado.Items.Clear()
            drpResEstado.DataTextField = "estDescricao"
            drpResEstado.DataValueField = "estId"
            drpResEstado.DataSource = objEstadoDAO.consultarEstadoPais
            drpResEstado.DataBind()

            'txtDataInicialSolicitacao.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy").Substring(0, 10)
            'hddResDataIni.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy").Substring(0, 10)
            'hddResDataFim.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy").Substring(0, 10)
            'txtDataFinalSolicitacao.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy").Substring(0, 10)
            'hddIntervaloSolicitacao.Value = DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), CDate(txtDataFinalSolicitacao.Text))
            drpResTipoPasseio.SelectedValue = objReservaListagemSolicitacaoVO.resCaracteristica
            txtResNomePasseio.Text = objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "")
            txtResDataInicialPasseio.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy")
            If objReservaListagemSolicitacaoVO.resCaracteristica <> "P" Then
                lblDadosDataFinalPasseio.Visible = True
                txtResDataFinalPasseio.Visible = True
            End If
            txtResDataFinalPasseio.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy")
            If objReservaListagemSolicitacaoVO.resPasseioPromovidoCEREC = "S" Then
                chkDadosOrganizadoSesc.Checked = True
                drpResResponsavel.Visible = False
                txtResResponsavel.Visible = True
            Else
                chkDadosOrganizadoSesc.Checked = False
                drpResResponsavel.Visible = False
                txtResResponsavel.Visible = True
            End If
            txtResResponsavel.Text = objReservaListagemSolicitacaoVO.resContato
            Dim comercial = objReservaListagemSolicitacaoVO.resFoneComercial
            Dim Com As String = ""
            'Formtando CPF e mostrando com a mascara//////////////
            If comercial.ToString.Length = 11 Then
                Com = "(" + Mid(comercial, 1, 2) + ")" + Mid(comercial, 2, 5) + " - " + Mid(comercial, 7, 4)
            Else
                Com = "(" + Mid(comercial, 1, 2) + ")" + Mid(comercial, 2, 4) + "-" + Mid(comercial, 7, 4)
            End If
            txtResTelComercial.Text = Com

            Dim Residencial = objReservaListagemSolicitacaoVO.resFoneResidencial
            Dim Res As String = ""
            If Residencial.ToString.Length = 11 Then
                Res = "(" + Mid(Residencial, 1, 2) + ")" + Mid(Residencial, 2, 5) + " - " + Mid(Residencial, 7, 4)
            Else
                Res = "(" + Mid(Residencial, 1, 2) + ")" + Mid(Residencial, 2, 4) + "-" + Mid(Residencial, 7, 4)
            End If
            txtResTelResidencial.Text = Res

            Dim Celular = objReservaListagemSolicitacaoVO.resCelular
            Dim Cel As String = ""
            If Celular.ToString.Length = 11 Then
                Cel = "(" + Mid(Celular, 1, 2) + ")" + Mid(Celular, 2, 5) + " - " + Mid(Celular, 7, 4)
            Else
                Cel = "(" + Mid(Celular, 1, 2) + ")" + Mid(Celular, 2, 4) + "-" + Mid(Celular, 7, 4)
            End If
            txtResTelCelular.Text = Cel
            '/////////////////////////////

            Try
                drpResCategoria.Text = objReservaListagemSolicitacaoVO.catId
            Catch ex As Exception
                drpResCategoria.Text = "1"
            End Try

            Try
                drpResCategoriaCobranca.Text = objReservaListagemSolicitacaoVO.resCatCobranca
            Catch ex As Exception
                drpResCategoriaCobranca.Text = "1"
            End Try
            txtResMemorando.Text = Mid(objReservaListagemSolicitacaoVO.resMemorando.Trim, 1, 100)
            Try
                drpResEmissor.SelectedValue = objReservaListagemSolicitacaoVO.resEmissor
            Catch ex As Exception
                drpResEmissor.SelectedValue = 0
            End Try

            txtResObservacao.Text = Mid(objReservaListagemSolicitacaoVO.resObs.Trim, 1, 200).Trim.Replace("'", "")
            Try
                drpResEstado.SelectedValue = objReservaListagemSolicitacaoVO.estIdDes
            Catch ex As Exception
                drpResEstado.SelectedValue = objReservaListagemSolicitacaoVO.estId
            End Try
            drpResEstado_SelectedIndexChanged(sender:=Nothing, e:=Nothing)
            Try
                'drpResCidade.SelectedValue = objReservaListagemSolicitacaoVO.resColoniaFeriasDes
                drpResCidade.SelectedValue = objReservaListagemSolicitacaoVO.resCidadeDes
            Catch ex As Exception
                drpResCidade.SelectedIndex = 0
            End Try
            Try
                'drpResLocalRefeicaoPadrao.Text = objReservaListagemSolicitacaoVO.refPratoCod
                drpResLocalRefeicaoPadrao.Text = objReservaListagemSolicitacaoVO.resColoniaFeriasDes
                drpResTipoRefeicaoPadrao.Text = objReservaListagemSolicitacaoVO.refPratoCod
            Catch ex As Exception
            End Try

            If drpResLocalRefeicaoPadrao.SelectedValue = "0" Then
                lblDadosTipoRefeicaoPadrao.Visible = False
                drpResTipoRefeicaoPadrao.Visible = False
            Else
                lblDadosTipoRefeicaoPadrao.Visible = True
                drpResTipoRefeicaoPadrao.Visible = True
            End If


            If objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = "" Then
                'If objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = "01/01/1900" Then
                txtResConfirmar.Text = ""
            Else
                txtResConfirmar.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao), "dd/MM/yyyy")
            End If

            If objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal = "" Then
                txtResPagarSinal.Text = ""
            Else
                txtResPagarSinal.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal), "dd/MM/yyyy")
            End If
            If objReservaListagemSolicitacaoVO.resDtGrupoListagem = "" Then
                txtResDigitar.Text = ""
            Else
                txtResDigitar.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoListagem), "dd/MM/yyyy")
            End If

            If objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal = "" Then
                txtResQuitar.Text = ""
            Else
                txtResQuitar.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal), "dd/MM/yyyy")
            End If
            Try
                drpResUsuarioInternet.SelectedValue = objReservaListagemSolicitacaoVO.resIdWeb
            Catch ex As Exception
                drpResUsuarioInternet.SelectedIndex = 0
            End Try

            'If objReservaListagemSolicitacaoVO.resCaracteristica = "I" Then
            '    pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Hospedagem de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "")
            'ElseIf objReservaListagemSolicitacaoVO.resDataIni = objReservaListagemSolicitacaoVO.resDataFim Then
            '    pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Passeio de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "")
            'Else
            '    pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Excursão de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "")
            'End If
            If objReservaListagemSolicitacaoVO.resCaracteristica = "E" Then
                drpResLocalSaida.SelectedValue = "S"
            Else
                drpResLocalSaida.SelectedValue = objReservaListagemSolicitacaoVO.orgId
            End If

            'hddResCaracteristica.Value = objReservaListagemSolicitacaoVO.resCaracteristica
            'txtResDtLimiteRetorno.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtLimiteRetorno), "dd/MM/yyyy")
            'Try
            '    cmbResHrLimiteRetorno.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtLimiteRetorno).Hour, "00")
            'Catch ex As Exception
            '    cmbResHrLimiteRetorno.Text = "00"
            'End Try
            'Formatando a matricula
            'Dim Mat = objReservaListagemSolicitacaoVO.resMatricula.Trim.Replace("-", "").Replace("/", "").Replace(".", "").Replace("\", "").Replace("_", "").Replace(" ", "")
            'txtResMatricula.Text = Mid(Mat, 1, 4) & " " & Mid(Mat, 5, 6) & " " & Mid(Mat, 11, 1)
            ''txtResMatricula.Text = objReservaListagemSolicitacaoVO.resMatricula.Trim.Replace("-", "").Replace("/", "").Replace(".", "").Replace("\", "").Replace("_", "")

            ''Formatando o cpf
            'Dim cpf = objReservaListagemSolicitacaoVO.resCPF_CGC.Trim.Replace("'", "").Replace(" ", "")
            'txtResCPF.Text = Mid(cpf, 1, 3) & " " & Mid(cpf, 4, 3) & " " & Mid(cpf, 7, 3) & " " & Mid(cpf, 10, 2)
            ''txtResCPF.Text = objReservaListagemSolicitacaoVO.resCPF_CGC.Trim.Replace("'", "")

            'txtResDataInicialPasseio.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy")
            'txtResDataFinalPasseio.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy")


            'txtResEmail.Text = Mid(objReservaListagemSolicitacaoVO.resEmail.Trim, 1, 40)
            'txtResDtNascimento.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtNascimento), "dd/MM/yyyy")
            'cmbResSexo.Text = objReservaListagemSolicitacaoVO.resSexo
            'hddResStatus.Value = objReservaListagemSolicitacaoVO.resStatus
            'txtResFax.Text = objReservaListagemSolicitacaoVO.resFax
            Dim sender As Object = Nothing
            Dim e As System.EventArgs = Nothing
            'If cmbEstId.Text <> objReservaListagemSolicitacaoVO.estId Then
            'drpResEstado.SelectedValue = objReservaListagemSolicitacaoVO.estId
            'drpResEstado_SelectedIndexChanged(sender, e)
            'End If
            'Try
            '    drpResCidade.Text = Mid(objReservaListagemSolicitacaoVO.resCidade.ToUpper, 1, 40)
            '    'cmbResCidade.Visible = True
            '    'txtResCidade.Visible = False
            'Catch ex As Exception
            '    'cmbResCidade.Visible = False
            '    'txtResCidade.Visible = True
            'End Try
            'txtResCidade.Text = Mid(objReservaListagemSolicitacaoVO.resCidade.Trim, 1, 40).Trim.Replace("'", "")
            'txtResLogradouro.Text = Mid(objReservaListagemSolicitacaoVO.resLogradouro.Trim, 1, 40).Trim.Replace("'", "")
            'txtResNumero.Text = Mid(objReservaListagemSolicitacaoVO.resNumero.Trim, 1, 10).Trim.Replace("'", "")
            'txtResQuadra.Text = Mid(objReservaListagemSolicitacaoVO.resQuadra.Trim, 1, 10).Trim.Replace("'", "")
            'txtResLote.Text = Mid(objReservaListagemSolicitacaoVO.resLote.Trim, 1, 10).Trim.Replace("'", "")
            'txtResComplemento.Text = Mid(objReservaListagemSolicitacaoVO.resComplemento.Trim, 1, 40).Trim.Replace("'", "")
            'txtResBairro.Text = Mid(objReservaListagemSolicitacaoVO.resBairro.Trim, 1, 40).Trim.Replace("'", "")
            'cmbResSalario.SelectedValue = objReservaListagemSolicitacaoVO.resSalario
            'cmbResEscolaridade.SelectedValue = objReservaListagemSolicitacaoVO.resEscolaridade
            'cmbResEstadoCivil.SelectedValue = objReservaListagemSolicitacaoVO.resEstadoCivil

            'Select Case Mid(objReservaListagemSolicitacaoVO.resRG, 1, 3).Trim
            '    Case "RG"
            '        cmbResDocIdentificacao.SelectedValue = "RG"
            '        txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
            '    Case "CC"
            '        cmbResDocIdentificacao.SelectedValue = "CC"
            '        txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
            '    Case "CN"
            '        cmbResDocIdentificacao.SelectedValue = "CN"
            '        txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
            '    Case Else
            '        cmbResDocIdentificacao.SelectedValue = "RG"
            '        txtResDocIdentificacao.Text = objReservaListagemSolicitacaoVO.resRG.Trim
            'End Select

            'cmbResDocIdentificacao.SelectedValue = Mid(objReservaListagemSolicitacaoVO.resRG, 1, 3).Trim
            'txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim  '"CN - 2980993"
            'txtResCep.Text = objReservaListagemSolicitacaoVO.resCep.Trim.Replace("'", "")



            txtResHotel.Text = Mid(objReservaListagemSolicitacaoVO.resHotelExcursao.Trim, 1, 40).Trim.Replace("'", "")
            'Esse vou ter que mudar e buscar
            drpResLocalSaida.Text = Mid(objReservaListagemSolicitacaoVO.resLocalSaida.Trim, 1, 200).Trim.Replace("'", "")
            drpResHoraSaida.SelectedValue = objReservaListagemSolicitacaoVO.resHoraSaida
            'Try
            '    cmbReservaHoraSaida.SelectedValue = objReservaListagemSolicitacaoVO.resHoraSaida
            'Catch ex As Exception
            '    cmbReservaHoraSaida.SelectedValue = "10"
            'End Try
            'Try
            '    txtResValorDesconto.Text = objReservaListagemSolicitacaoVO.resValorDesconto
            'Catch ex As Exception
            '    txtResValorDesconto.Text = "0"
            'End Try


            'Try
            '    cmbDestinoCidade.SelectedValue = objReservaListagemSolicitacaoVO.resCidadeDes.Trim
            'Catch ex As Exception
            '    cmbDestinoCidade.SelectedIndex = 0
            'End Try


            'If objReservaListagemSolicitacaoVO.resCaracteristica = "I" Then
            '    pnlGrupo.Visible = False
            '    pnlDestinoGrupo.Visible = False
            '    lblResHoraSaida.Visible = True
            '    cmbReservaHoraSaida.Visible = True
            '    gdvReserva9.Columns(3).Visible = True
            '    gdvReserva9.Columns(4).Visible = True
            '    btnHospedagemNova.Enabled = (InStr("CF", hddResStatus.Value) = 0)
            '    btnEmissivoNova.Enabled = False
            If objReservaListagemSolicitacaoVO.resCaracteristica = "E" Then
                pnlCamposGrupo.Visible = True
                'pnlDestinoGrupo.Visible = False
                'lblResHoraSaida.Visible = True
                'cmbReservaHoraSaida.Visible = True
                'gdvReserva9.Columns(3).Visible = True
                'gdvReserva9.Columns(4).Visible = True
                'btnHospedagemNova.Enabled = (InStr("CF", hddResStatus.Value) = 0)
                'btnEmissivoNova.Enabled = False
            Else
                pnlCamposGrupo.Visible = False
                'pnlDestinoGrupo.Visible = True
                'lblResHoraSaida.Visible = False
                'cmbReservaHoraSaida.Visible = False
                'gdvReserva9.Columns(3).Visible = False
                'gdvReserva9.Columns(4).Visible = False
                'btnHospedagemNova.Enabled = False
                'btnEmissivoNova.Enabled = (InStr("CFE", hddResStatus.Value) = 0)
            End If
            'cmbDestino_SelectedIndexChanged(sender, e)

            'If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
            '    btnReservaGravar.Visible = (InStr("CFE", hddResStatus.Value) = 0)
            '    btnReservaGravar.Enabled = (InStr("CFE", hddResStatus.Value) = 0)
            '    btnReservaCalculo.Visible = True
            '    btnReservaCancelar.Visible = (InStr("CFE", hddResStatus.Value) = 0) And (hddResId.Value <> "0")
            '    btnReservaCancelar.Enabled = (InStr("CFE", hddResStatus.Value) = 0) And (hddResId.Value <> "0")
            '    btnReservaReativar.Visible = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IE", hddResCaracteristica.Value) <> "0")
            '    btnReservaReativar.Enabled = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IE", hddResCaracteristica.Value) <> "0")
            'End If
            'If hddResCaracteristica.Value = "I" Then
            '    lblResCatCobranca.Visible = False
            '    cmbResCatCobranca.Visible = False
            'Else
            '    lblResCatCobranca.Visible = True
            '    cmbResCatCobranca.Visible = True
            'End If

            '            txtResNome.Focus()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkResponsavel_Click(sender As Object, e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
        End If
        Try
            'hddResId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(1).ToString
            'hddSolId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(0).ToString
            'hddSolIdNovo.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(0).ToString
        Catch ex As Exception
        End Try

        divBotoesNovoPasseio.Attributes("class") = "col-md-1"
        divSalvarNovoPasseio.Attributes("class") = "col-md-06"
        divCancelaNovoPasseio.Attributes("class") = ""

        Dim Link As LinkButton = sender
        Dim row As GridViewRow = Link.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim resId = gdvConReservas.DataKeys(row.RowIndex()).Item("resId").ToString
        hddResId.Value = resId
        hddResCaracteristica.Value = gdvConReservas.DataKeys(row.RowIndex()).Item("resCaracteristica").ToString
        hddResStatus.Value = gdvConReservas.DataKeys(row.RowIndex()).Item("resStatus").ToString

        'Saindo do processo de copia/cola do integrante
        'imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")
        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(resId)
        'gdvReserva1.DataSource = lista
        'gdvReserva1.DataBind()
        'gdvReserva1.SelectedIndex = -1

        'Carrega os nomes dos passeios cadastrados no combo
        objPasseioVO = New PasseioVO()
        ObjPasseioDAO = New PasseioDAO

        drpResNomePasseio.DataValueField = "pasId"
        drpResNomePasseio.DataTextField = "pasNomePasseio"
        btnAdicionaNomePasseio.Text = "Adiciona"
        drpResNomePasseio.Items.Clear()
        drpResNomePasseio.DataSource = ObjPasseioDAO.ConsultaNomePasseio("", "TurismoSocialCaldas")
        drpResNomePasseio.DataBind()
        drpResNomePasseio.Items.Insert(0, New ListItem("Selecione...", "0"))
        drpResNomePasseio.Visible = False
        txtResNomePasseio.Visible = True

        CarregaDadosReserva()
        gdvConReservas.Visible = False
        pnlDadosPasseio.Visible = True
        pnlConConsultarPasseio.Visible = False

        If drpResTipoPasseio.SelectedIndex = 0 Then 'Emisivo Laercio
            lblDadosDataFinalPasseio.Visible = False
            txtResDataFinalPasseio.Visible = False
            pnlCamposGrupo.Visible = False
            divResDadosHotelSaidaHorasSaida.Visible = True
            divResLocalRefeicao.Visible = True
            'lblDadosTipoRefeicaoPadrao.Visible = True
            'drpResTipoRefeicaoPadrao.Visible = True
        ElseIf drpResTipoPasseio.SelectedIndex = 1 Then 'Excursão Eduardo
            lblDadosDataFinalPasseio.Visible = True
            txtResDataFinalPasseio.Visible = True
            pnlCamposGrupo.Visible = False
            divResDadosHotelSaidaHorasSaida.Visible = True
            divResLocalRefeicao.Visible = True
            'lblDadosTipoRefeicaoPadrao.Visible = True
            'drpResTipoRefeicaoPadrao.Visible = True
        ElseIf drpResTipoPasseio.SelectedIndex = 2 Then 'Grupo (Luzia)
            lblDadosDataFinalPasseio.Visible = True
            txtResDataFinalPasseio.Visible = True
            pnlCamposGrupo.Visible = True
            divResDadosHotelSaidaHorasSaida.Visible = False
            divResLocalRefeicao.Visible = False
            'lblDadosTipoRefeicaoPadrao.Visible = False
            'drpResTipoRefeicaoPadrao.Visible = false 
        End If
        btnConConsultar.Attributes.Add("Acao", "gdvResponsavel")
        'Irá definir se esta em estada, cancelada
        DefineStatusPasseio()
    End Sub

    Protected Sub drpResLocalRefeicaoPadrao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResLocalRefeicaoPadrao.SelectedIndexChanged
        drpResLocalRefeicaoPadrao.ToolTip = drpResLocalRefeicaoPadrao.SelectedItem.ToString
        If drpResLocalRefeicaoPadrao.SelectedIndex > 0 Then
            CarregaCmbRefeicaoPrato()
            drpResTipoRefeicaoPadrao.Visible = True
            lblDadosTipoRefeicaoPadrao.Visible = True
        Else
            drpResTipoRefeicaoPadrao.Visible = False
            lblDadosTipoRefeicaoPadrao.Visible = False
        End If
        drpResEstado.Focus()
    End Sub
    Protected Sub CarregaCmbRefeicaoPrato()
        If drpResTipoRefeicaoPadrao.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objRefeicaoPratoDAO = New Turismo.RefeicaoPratoDAO("TurismoSocialCaldas")
            Else
                objRefeicaoPratoDAO = New Turismo.RefeicaoPratoDAO("TurismoSocialPiri")
            End If
            lista = objRefeicaoPratoDAO.consultarDisponibilidade(" ")
            drpResTipoRefeicaoPadrao.DataSource = lista
            drpResTipoRefeicaoPadrao.DataValueField = ("RefPratoCod")
            drpResTipoRefeicaoPadrao.DataTextField = ("RefPratoDesc")
            drpResTipoRefeicaoPadrao.DataBind()
            drpResTipoRefeicaoPadrao.DataSource = lista
            drpResTipoRefeicaoPadrao.DataValueField = ("RefPratoCod")
            drpResTipoRefeicaoPadrao.DataTextField = ("RefPratoDesc")
            drpResTipoRefeicaoPadrao.DataBind()
            'If cmbDestino.SelectedValue = "0" Then
            '    'cmbPratoRapido.Items.Insert(0, New ListItem("", "0"))
            '    'cmbPratoRapido0.Items.Insert(0, New ListItem("", "0"))
            'End If
        End If
    End Sub

    Protected Sub btnSalvarDadosPasseio_Click(sender As Object, e As EventArgs) Handles btnSalvarDadosPasseio.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)

        Try
            If txtResMemorando.Text.Trim.Length > 0 And drpResEmissor.SelectedIndex = 0 Then
                Mensagem("O Campo memorando foi preenchido, selecione o Emissor para prosseguir.")
                drpResEmissor.Focus()
                Exit Try
            End If

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
                objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialCaldas")
            Else
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialPiri")
                objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialPiri")
            End If

            'Carregando os valores no componente antes da inserção.
            objReservaEmissivoVO = New ReservaEmissivoVO
            objReservaEmissivoVO.resId = hddResId.Value
            objReservaEmissivoVO.resCaracteristica = drpResTipoPasseio.SelectedValue.ToString
            objReservaEmissivoVO.resNome = txtResNomePasseio.Text
            objReservaEmissivoVO.resDataIni = Format(CDate(txtResDataInicialPasseio.Text), "dd/MM/yyyy")
            If drpResTipoPasseio.SelectedValue = "P" Then
                objReservaEmissivoVO.resDataFim = Format(CDate(txtResDataInicialPasseio.Text), "dd/MM/yyyy")
            Else
                objReservaEmissivoVO.resDataFim = Format(CDate(txtResDataFinalPasseio.Text), "dd/MM/yyyy")
            End If
            If chkDadosOrganizadoSesc.Checked Then
                objReservaEmissivoVO.resPasseioPromovidoCEREC = "S"
            Else
                objReservaEmissivoVO.resPasseioPromovidoCEREC = "N"
            End If

            objReservaEmissivoVO.resContato = txtResResponsavel.Text
            objReservaEmissivoVO.resFoneComercial = txtResTelComercial.Text.Trim.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")
            objReservaEmissivoVO.resFoneResidencial = txtResTelResidencial.Text.Trim.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")
            objReservaEmissivoVO.resCelular = txtResTelCelular.Text.Trim.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "")

            objReservaEmissivoVO.catId = drpResCategoria.SelectedValue
            objReservaEmissivoVO.resCatCobranca = drpResCategoriaCobranca.SelectedValue
            objReservaEmissivoVO.resMemorando = txtResMemorando.Text.Trim
            objReservaEmissivoVO.resEmissor = drpResEmissor.SelectedValue
            objReservaEmissivoVO.resObs = txtResObservacao.Text.Trim

            If drpResLocalRefeicaoPadrao.SelectedValue = "R" Then
                objReservaEmissivoVO.resAlmoco = "S"
                objReservaEmissivoVO.resAlmocoRestaurante = drpResLocalRefeicaoPadrao.SelectedValue
            ElseIf drpResLocalRefeicaoPadrao.SelectedValue = "0" Then
                objReservaEmissivoVO.resAlmoco = "N"
                objReservaEmissivoVO.resAlmocoRestaurante = drpResLocalRefeicaoPadrao.SelectedValue
            Else
                objReservaEmissivoVO.resAlmoco = "N"
                objReservaEmissivoVO.resAlmocoRestaurante = drpResLocalRefeicaoPadrao.SelectedValue
            End If
            'Esse campo server para setar onde irá acontecer a refeição se será no restaurante ou lanchonete
            objReservaEmissivoVO.resColoniaFeriasDes = drpResLocalRefeicaoPadrao.SelectedValue

            objReservaEmissivoVO.estIdDes = drpResEstado.SelectedValue
            objReservaEmissivoVO.resCidadeDes = drpResCidade.SelectedValue
            objReservaEmissivoVO.resHotelExcursao = txtResHotel.Text.Trim
            objReservaEmissivoVO.resLocalSaida = drpResLocalSaida.SelectedValue
            objReservaEmissivoVO.resHoraSaida = drpResHoraSaida.SelectedValue
            objReservaEmissivoVO.resTipo = drpResTipoPasseio.SelectedValue

            'Esses faltaram ////////////////////////////////
            If hddResId.Value = 0 Then
                objReservaEmissivoVO.resStatus = "I"
            End If
            objReservaEmissivoVO.catRefeicaoId = 0
            objReservaEmissivoVO.refPratoCod = drpResTipoRefeicaoPadrao.SelectedValue
            objReservaEmissivoVO.resCafe = "N"
            objReservaEmissivoVO.resJantar = "N"
            objReservaEmissivoVO.resFormaPagamento = "ER"
            objReservaEmissivoVO.resFormaPagtoCafe = "ER"
            objReservaEmissivoVO.resFormaPagtoAlmoco = "ER"
            objReservaEmissivoVO.resFormaPagtoJantar = "ER"
            'Capital de Goiás 9373=Goiânia em TbBdProdMunicipio
            If drpResCidade.SelectedValue = "9373" Then
                objReservaEmissivoVO.resCapitalGoias = "S"
            Else
                objReservaEmissivoVO.resCapitalGoias = "N"
            End If
            objReservaEmissivoVO.resUsuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            objReservaEmissivoVO.resUsuarioData = Format(Date.Today, "yyyy-MM-dd HH:mm:ss")
            '////////////////////////////////////////////////
            'Complemento quando for Grupo - Luzia
            If IsDate(txtResConfirmar.Text) Then
                objReservaEmissivoVO.resDtGrupoConfirmacao = Format(CDate(txtResConfirmar.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaEmissivoVO.resDtGrupoConfirmacao = "Null"
            End If
            If IsDate(txtResPagarSinal.Text) Then
                objReservaEmissivoVO.resDtGrupoPgtoSinal = Format(CDate(txtResPagarSinal.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaEmissivoVO.resDtGrupoPgtoSinal = "Null"
            End If
            If IsDate(txtResDigitar.Text) Then
                objReservaEmissivoVO.resDtGrupoListagem = Format(CDate(txtResDigitar.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaEmissivoVO.resDtGrupoListagem = "Null"
            End If
            If IsDate(txtResQuitar.Text) Then
                objReservaEmissivoVO.resDtGrupoPgtoTotal = Format(CDate(txtResQuitar.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaEmissivoVO.resDtGrupoPgtoTotal = "Null"
            End If

            If drpResTipoPasseio.SelectedValue = "E" Then
                objReservaEmissivoVO.resIdWeb = drpResUsuarioInternet.SelectedValue
            Else
                objReservaEmissivoVO.resIdWeb = "0" 'cmbResIdWeb.SelectedValue
            End If


            '0: Erro  1: Inserido com sucesso   2:Atualizado com sucesso
            Select Case objReservaEmissivoDAO.InsereEmissivo(objReservaEmissivoVO, drpResTipoPasseio.SelectedValue)
                Case 0
                    Mensagem("Erro ao salvar os dados do passeio.")
                Case 1
                    Mensagem("Passeio inserido com sucesso!")
                Case 2
                    Mensagem("Dados atualizados com sucesso!")
            End Select



        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString & _
           "&Erro=Erro ao cancelar a reserva. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnReservaCancelar Resid: " & hddResId.Value)
        End Try
    End Sub
    Public Sub Habilitar(ByVal controlP As Control)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Enabled = True
            ElseIf TypeOf ctl Is DropDownList Then
                DirectCast(ctl, DropDownList).Enabled = True
            ElseIf TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Enabled = True
            ElseIf TypeOf ctl Is CheckBoxList Then
                DirectCast(ctl, CheckBoxList).Enabled = True
            ElseIf ctl.Controls.Count > 0 Then
                Habilitar(ctl)
            End If
        Next
    End Sub

    Protected Sub drpResNomePasseio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResNomePasseio.SelectedIndexChanged
        txtResNomePasseio.Text = drpResNomePasseio.SelectedItem.ToString
        txtResDataInicialPasseio.Focus()
    End Sub

    Protected Sub btnCancelaNovoNomePassio_Click(sender As Object, e As EventArgs) Handles btnCancelaNovoNomePasseio.Click
        divBotoesNovoPasseio.Attributes("class") = "col-md-1"
        divSalvarNovoPasseio.Attributes("class") = "col-md-06"
        divCancelaNovoPasseio.Attributes("class") = ""

        divSalvarNovoPasseio.Visible = True
        divCancelaNovoPasseio.Visible = False

        drpResNomePasseio.Visible = True
        txtResNomePasseio.Visible = False
        drpResNomePasseio.Focus()
        txtResNomePasseio.Text = ""
        btnAdicionaNomePasseio.Text = "Adiciona"
        Return
    End Sub
    Protected Sub CarregaCmbResIdWeb()
        If drpResUsuarioInternet.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objUsuarioGrupoDAO = New Turismo.UsuarioGrupoDAO("TurismoSocialCaldas")
            Else
                objUsuarioGrupoDAO = New Turismo.UsuarioGrupoDAO("TurismoSocialPiri")
            End If
            lista = objUsuarioGrupoDAO.consultar()
            drpResUsuarioInternet.DataSource = lista
            drpResUsuarioInternet.DataValueField = ("resId")
            drpResUsuarioInternet.DataTextField = ("resNome")
            drpResUsuarioInternet.DataBind()
        End If
    End Sub

    Protected Sub drpResCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResCategoria.SelectedIndexChanged
        drpResCategoriaCobranca.Focus()
    End Sub

    Protected Sub drpResEmissor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResEmissor.SelectedIndexChanged
        txtResObservacao.Focus()
    End Sub

    Protected Sub drpResCidade_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResCidade.SelectedIndexChanged
        txtResHotel.Focus()
    End Sub

    Protected Sub drpResLocalSaida_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResLocalSaida.SelectedIndexChanged
        drpResHoraSaida.Focus()
    End Sub

    Protected Sub drpResTipoRefeicaoPadrao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpResTipoRefeicaoPadrao.SelectedIndexChanged
        drpResTipoRefeicaoPadrao.Focus()
    End Sub

    Protected Sub btnCancelarReserva_Click(sender As Object, e As EventArgs) Handles btnCancelarReserva.Click
        objReservaEmissivoVO = New ReservaEmissivoVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialCaldas")
        Else
            objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialPiri")
        End If
        Select Case objReservaEmissivoDAO.CancelaPasseio(hddResId.Value)
            Case 1
                Mensagem("Cancelado com sucesso!")
                pnlDadosPasseio.Visible = False
                pnlConConsultarPasseio.Visible = True
                btnConConsultar_Click(sender, e)
            Case 0
                Mensagem("Erro no cancelamento, informe ao centro de informática.")
        End Select
    End Sub

    Protected Sub btnReativarReserva_Click(sender As Object, e As EventArgs) Handles btnReativarReserva.Click
        objReservaEmissivoVO = New ReservaEmissivoVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialCaldas")
        Else
            objReservaEmissivoDAO = New ReservaEmissivoDAO("TurismoSocialPiri")
        End If
        Select Case objReservaEmissivoDAO.ReativaPasseio(hddResId.Value)
            Case 1
                hddResStatus.Value = "P"
                pnlTodosDadosReserva.Enabled = True
                btnSalvarDadosPasseio.Enabled = True
                btnCancelarReserva.Enabled = True
                Mensagem("Ativado com sucesso!")
                DefineStatusPasseio()
            Case 2
                hddResStatus.Value = "R"
                pnlTodosDadosReserva.Enabled = True
                btnSalvarDadosPasseio.Enabled = True
                btnCancelarReserva.Enabled = True
                Mensagem("Ativado com sucesso!")
                DefineStatusPasseio()
            Case 0
                Mensagem("Erro na ativação, informe ao centro de informática.")
        End Select
    End Sub

    Protected Sub DefineStatusPasseio()
        If hddResStatus.Value = "C" Then
            pnlTodosDadosReserva.Enabled = False
            btnSalvarDadosPasseio.Enabled = False
            btnCancelarReserva.Enabled = False
            btnVoltaDadosPasseio.Enabled = True
            If Format(CDate(txtResDataInicialPasseio.Text), "yyyy-MM-dd") < Format(Now.Date, "yyyy-MM-dd") Then
                btnReativarReserva.Enabled = False
            Else
                btnReativarReserva.Enabled = True
            End If
            lblDadosPasseio.Text = "Dados do Passeio - Status: Cancelado"
        ElseIf hddResStatus.Value = "P" Then
            lblDadosPasseio.Text = "Dados do Passeio - Status: Pendente de Pagamento"
            btnReativarReserva.Enabled = False
        ElseIf hddResStatus.Value = "I" Then
            btnReativarReserva.Enabled = False
            lblDadosPasseio.Text = "Dados do Passeio - Status: Pendente de Integrante"
        ElseIf hddResStatus.Value = "E" Then
            btnReativarReserva.Enabled = False
            lblDadosPasseio.Text = "Dados do Passeio - Status: Em Estada"
        ElseIf hddResStatus.Value = "R" Then
            btnReativarReserva.Enabled = False
            lblDadosPasseio.Text = "Dados do Passeio - Status: Cofirmado"
        Else
            pnlTodosDadosReserva.Enabled = True
            btnSalvarDadosPasseio.Enabled = True
            btnCancelarReserva.Enabled = True
            btnReativarReserva.Enabled = True
            btnVoltaDadosPasseio.Enabled = True
            lblDadosPasseio.Text = "Dados do Passeio"
        End If
    End Sub
End Class

