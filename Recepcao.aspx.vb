Imports Microsoft.Reporting.WebForms
Imports System.Configuration
Imports System.Web.Configuration
Imports Turismo
Partial Class Recepcao
    Inherits System.Web.UI.Page
    Private contEntrada As Integer
    Private contEstada As Integer
    Private contSaida As Integer
    Private CompletarEnxoval As Boolean
    Private contTransf As Integer
    Dim objReservaListagemIntegranteDAO As Turismo.ReservaListagemIntegranteDAO
    Dim objReservaListagemIntegranteVO As New Turismo.ReservaListagemIntegranteVO
    Dim objCalculaIdadeIntegranteDAO As CalculaIdadeIntegranteDAO
    Dim objCalculaIdadeIntegranteVO As CalculaIdadeIntegranteVO

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Tipo As String = "Nome"
        If Not IsPostBack Then
            If (Not Session("GrupoRecepcao") And Session("MasterPage").ToString = "~/TurismoSocial.Master") Or _
               (Not Session("GrupoRecepcaoPiri") And Not Session("GrupoPortariaPiri") And Session("MasterPage").ToString = "~/TurismoSocialPiri.Master") Then
                Response.Redirect("AcessoNegado.aspx")
            End If
            btnAtualizar_Click(sender, e)
            'Colorindo os apartamentos flutuantes do Wilton e Bloco Anhanguera
            If txtConsulta.Text.Trim.Length = 0 Then
                ConsultaTemporada(Format(Now.Date, "dd-MM-yyyy"), Format(Now.Date, "dd-MM-yyyy"))
            Else
                ConsultaTemporada(Format(CDate(txtConsulta.Text), "dd-MM-yyyy"), Format(CDate(txtConsulta.Text), "dd-MM-yyyy"))
            End If



            btnConsulta.CommandName = "intNome, convert(datetime, intDataIni, 120), hosStatus"

            txtValor.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this.value)}")
            txtQtde.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this.value)}")
            txtModeloVeiculo.Attributes.Add("onkeyup", "javascript:ConverteMaiusculo(this.value)")


            'Me.txtConsulta.Attributes.Add("OnMouseUp", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='MouseUp';")
            'Me.txtConsulta.Attributes.Add("OnMouseDown", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='MouseDown';")
            Me.txtConsulta.Attributes.Add("OnDrop", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='Drop';")
            Me.txtConsulta.Attributes.Add("OnPaste", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='Paste';")
            Me.txtConsulta.Attributes.Add("OnKeyPress", "javascript:if (this.value.length==1) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo1.value=agora.getTime();} else {if (this.value.length==6) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo2.value=agora.getTime(); ctl00_conPlaHolTurismoSocial_hddOperacao.value = ctl00_conPlaHolTurismoSocial_hddPeriodo2.value - ctl00_conPlaHolTurismoSocial_hddPeriodo1.value > 200;}};")
            Dim lista As New ArrayList
            Dim bd As String
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                bd = "TurismoSocialCaldas"
            Else
                bd = "TurismoSocialPiri"
                rdbCortesiaRestaurante.Visible = False
            End If
            Dim objTipoMovimentoDAO = New Turismo.TipoMovimentoDAO(bd)
            lista = objTipoMovimentoDAO.consultar
            Dim listaAux As New ArrayList
            For Each item As Turismo.TipoMovimentoVO In lista
                If item.tmoManual = "S" Then
                    listaAux.Add(item)
                End If
            Next

            cmbTipo.DataSource = listaAux
            cmbTipo.DataValueField = ("tmoId")
            cmbTipo.DataTextField = ("tmoDescricao")
            cmbTipo.DataBind()

            Dim objBlocoDAO = New Turismo.BlocoDAO(bd)
            lista = objBlocoDAO.consultar()
            If lista.Count > 1 Then
                cmbBloco.DataSource = lista
                cmbBloco.DataValueField = ("bloId")
                cmbBloco.DataTextField = ("bloDescricao")
                cmbBloco.DataBind()
                cmbBloco.Items.Insert(0, New ListItem("", "0"))
                cmbBloco.Items.Insert(1, New ListItem("Emissivo", "E"))
            Else
                lblDestino.Visible = False
                cmbBloco.Visible = False
            End If
            Dim objTestaGrupo As New Uteis.TestaUsuario
            rdbCortesiaCaucao.Enabled = objTestaGrupo.testaGrupo(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""), "Turismo Social Cortesia")
            rdbCortesiaConsumo.Enabled = rdbCortesiaCaucao.Enabled
            rdbCortesiaRestaurante.Enabled = rdbCortesiaCaucao.Enabled
            txtMemorando.Enabled = rdbCortesiaCaucao.Enabled

            Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
            Dim objSituacaoAtualVO As Turismo.SituacaoAtualVO

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")
            Else
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
            End If

            If cmbBloco.Text = "" Then
                objSituacaoAtualVO = objSituacaoAtualDAO.consultar("0", Tipo, txtConsulta.Text, _
                                                                   ckbEntrada.Checked, ckbEstada.Checked, _
                                                                   ckbSaida.Checked, ckbTransferencia.Checked)
            Else
                objSituacaoAtualVO = objSituacaoAtualDAO.consultar(cmbBloco.Text, Tipo, txtConsulta.Text, _
                                                                   ckbEntrada.Checked, ckbEstada.Checked, _
                                                                   ckbSaida.Checked, ckbTransferencia.Checked)
            End If

            CType(Page.Master.FindControl("Label2"), Label).Text = CStr(CInt(objSituacaoAtualVO.sitAptoCheckin.ToString) - CInt(objSituacaoAtualVO.sitAptoJaEstada.ToString)) + " Aptos"
            CType(Page.Master.FindControl("Label3"), Label).Text = objSituacaoAtualVO.sitHospedeCheckin.ToString + " Hóspedes | "
            CType(Page.Master.FindControl("Label5"), Label).Text = objSituacaoAtualVO.sitAptoCheckout.ToString + " Aptos"
            CType(Page.Master.FindControl("Label6"), Label).Text = objSituacaoAtualVO.sitHospedeCheckout.ToString + " Hóspedes | "
            CType(Page.Master.FindControl("Label8"), Label).Text = objSituacaoAtualVO.sitAptoEstada.ToString + " Aptos"
            CType(Page.Master.FindControl("Label9"), Label).Text = objSituacaoAtualVO.sitHospedeEstada.ToString + " Hóspedes | "
            CType(Page.Master.FindControl("Label10"), Label).Text = objSituacaoAtualVO.sitPassante.ToString + " Passantes | "
            CType(Page.Master.FindControl("Label11"), Label).Text = "Total " + CStr(CInt(objSituacaoAtualVO.sitHospedeEstada.ToString) + CInt(objSituacaoAtualVO.sitPassante.ToString))


            CType(Page.Master.FindControl("Label1"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label2"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label3"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label4"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label5"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label6"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label7"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label8"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label9"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label10"), Label).Visible = "True"
            CType(Page.Master.FindControl("Label11"), Label).Visible = "True"
            CType(Page.Master.FindControl("ckbTempoReal"), CheckBox).Visible = "True"

            CType(Page.Master.FindControl("imgAjuda"), Image).Visible = True

            txtConsulta.Focus()


            If Not Page.PreviousPage Is Nothing Then
                Dim placeHolder As Control = PreviousPage.Controls(0).FindControl("conPlaHolTurismoSocial")
                Dim SourceResId As HiddenField = CType(placeHolder.FindControl("hddResId"), HiddenField)
                Dim SourceIntId As HiddenField = CType(placeHolder.FindControl("hddIntId"), HiddenField)
                Dim SourcetxtConsulta As HiddenField = CType(placeHolder.FindControl("hddtxtConsultaRecepcao"), HiddenField)
                Dim SourcecmbBloco As HiddenField = CType(placeHolder.FindControl("hddcmbBloco"), HiddenField)
                Dim SourceckbEntrada As HiddenField = CType(placeHolder.FindControl("hddckbEntrada"), HiddenField)
                Dim SourceckbEstada As HiddenField = CType(placeHolder.FindControl("hddckbEstada"), HiddenField)
                Dim SourceckbSaida As HiddenField = CType(placeHolder.FindControl("hddckbSaida"), HiddenField)
                Dim SourceckbTransferencia As HiddenField = CType(placeHolder.FindControl("hddckbTransferencia"), HiddenField)
                If Not SourceResId Is Nothing Then
                    hddResId.Value = SourceResId.Value
                    hddIntId.Value = SourceIntId.Value
                    txtConsulta.Text = SourcetxtConsulta.Value
                    cmbBloco.Text = SourcecmbBloco.Value
                    ckbEntrada.Checked = SourceckbEntrada.Value
                    ckbEstada.Checked = SourceckbEstada.Value
                    ckbSaida.Checked = SourceckbSaida.Value
                    ckbTransferencia.Checked = SourceckbTransferencia.Value
                    'gdvReserva1_SelectedIndexChanged(sender, e)
                    btnConsulta_Click(sender, e)
                    Try
                        If hddIntId.Value > "" Then
                            For Each linha As GridViewRow In gdvReserva1.Rows
                                If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                                    gdvReserva1.SelectedIndex = linha.RowIndex
                                    Exit For
                                End If
                            Next
                            gdvReserva1.SelectedRow.Cells(0).FindControl("lnkNome").Focus()
                        Else
                            For Each linha As GridViewRow In gdvReserva1.Rows
                                If (gdvReserva1.DataKeys(linha.RowIndex).Item(0).ToString = hddResId.Value) Then
                                    gdvReserva1.SelectedIndex = linha.RowIndex
                                    Exit For
                                End If
                            Next
                            gdvReserva1.SelectedRow.Cells(4).Focus()
                        End If

                    Catch ex As Exception

                    End Try
                End If
            End If

        ElseIf CType(Page.Master.FindControl("ckbTempoReal"), CheckBox).Checked Then
            Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
            Dim objSituacaoAtualVO As Turismo.SituacaoAtualVO

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")
            Else
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
            End If

            If InStr(txtConsulta.Text, "/") > 0 Then 'Data
                Tipo = "Dia"
            Else
                Dim i As Integer
                Dim Numero As Integer = 0
                Dim Caracter As Integer = 0
                For i = 1 To txtConsulta.Text.Trim.Length
                    If IsNumeric(txtConsulta.Text.Substring(i - 1, 1)) Then
                        Numero += 1
                    ElseIf txtConsulta.Text.Substring(i - 1, 1) <> " " Then
                        Caracter += 1
                    End If
                Next
                If Caracter = 3 And Numero = 4 Then 'Placa
                    Tipo = "Placa"
                ElseIf Caracter >= 0 And Numero > 0 Then 'Apartamento
                    Tipo = "Apartamento"
                ElseIf hddCartao.Value > "" Then 'Volta de ação feito após consulta via cartão
                    Tipo = "Cartao"
                Else 'Nome ou placa (se 3 letras)
                    Tipo = "Nome"
                End If
            End If

            If Tipo <> "Cartao" Then
                If cmbBloco.Text = "" Then
                    objSituacaoAtualVO = objSituacaoAtualDAO.consultar("0", Tipo, txtConsulta.Text, _
                                                                       ckbEntrada.Checked, ckbEstada.Checked, _
                                                                       ckbSaida.Checked, ckbTransferencia.Checked)
                Else
                    objSituacaoAtualVO = objSituacaoAtualDAO.consultar(cmbBloco.Text, Tipo, txtConsulta.Text, _
                                                                       ckbEntrada.Checked, ckbEstada.Checked, _
                                                                       ckbSaida.Checked, ckbTransferencia.Checked)
                End If
                'CType(Page.Master.FindControl("Label2"), Label).Text = objSituacaoAtualVO.sitAptoCheckin.ToString + " Aptos"
                Try
                    CType(Page.Master.FindControl("Label2"), Label).Text = CStr(CInt(objSituacaoAtualVO.sitAptoCheckin.ToString) - CInt(objSituacaoAtualVO.sitAptoJaEstada.ToString)) + " Aptos"
                    CType(Page.Master.FindControl("Label3"), Label).Text = objSituacaoAtualVO.sitHospedeCheckin.ToString + " Hóspedes | "
                    CType(Page.Master.FindControl("Label5"), Label).Text = objSituacaoAtualVO.sitAptoCheckout.ToString + " Aptos"
                    CType(Page.Master.FindControl("Label6"), Label).Text = objSituacaoAtualVO.sitHospedeCheckout.ToString + " Hóspedes | "
                    CType(Page.Master.FindControl("Label8"), Label).Text = objSituacaoAtualVO.sitAptoEstada.ToString + " Aptos"
                    CType(Page.Master.FindControl("Label9"), Label).Text = objSituacaoAtualVO.sitHospedeEstada.ToString + " Hóspedes | "
                    CType(Page.Master.FindControl("Label10"), Label).Text = objSituacaoAtualVO.sitPassante.ToString + " Passantes | "
                    CType(Page.Master.FindControl("Label11"), Label).Text = "Total " + CStr(CInt(objSituacaoAtualVO.sitHospedeEstada.ToString) + CInt(objSituacaoAtualVO.sitPassante.ToString))
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub

    Protected Sub DisponibilidadeTroca()
        hddDisponibilidadeTroca.Value = "N"
        If hddApto1.Value > "" And hddApto2.Value > "" Then
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Os apartamentos foram posicionados." + "');", True)
            If hddResId1.Value = hddResId2.Value Then
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId idênticos." + "');", True)
            Else
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId diferentes." + "');", True)
            End If

            If hddAcmId1.Value = hddAcmId2.Value Then
                hddDisponibilidadeTroca.Value = "S"
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Acomodações idênticas." + "');", True)
            Else
                Dim objDisponibilidadeDAO As Turismo.DisponibilidadeDAO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialCaldas")
                Else
                    objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialPiri")
                End If
                If hddDataFim1.Value > "" And hddDataFim2.Value > "" Then
                    If hddDataFim1.Value <> hddDataFim2.Value Then
                        If CDate(hddDataFim1.Value) > CDate(hddDataFim2.Value) Then
                            hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
                        Else
                            hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
                        End If
                        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas diferentes." + "');", True)
                    Else
                        hddDisponibilidadeTroca.Value = "S"
                        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas idênticas." + "');", True)
                    End If
                Else
                    If hddDataFim1.Value = "" Or hddDataFim2.Value = "" Then
                        If hddDataFim1.Value > "" Then
                            hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
                        Else
                            hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
                        End If
                    Else
                        hddDisponibilidadeTroca.Value = "S"
                    End If
                End If
            End If

            'If (hddArrastouIntegrante.Value = "" And (hddCapacidade1.Value < hddQtde2.Value Or hddCapacidade2.Value < hddQtde1.Value)) Or _
            '   (hddArrastouIntegrante.Value = "Sim" And (hddCapacidade1.Value < hddQtde1.Value Or hddCapacidade2.Value < hddQtde2.Value)) Then
            '    hddDisponibilidadeTroca.Value = "N"
            '    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade suportada." + "');", True)
            'Else
            '    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade não suportada." + "');", True)
            'End If
        End If
        btnTroca.Enabled = (hddDisponibilidadeTroca.Value = "S")
        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + hddDisponibilidadeTroca.Value + "');", True)
    End Sub

    Protected Sub SimulaValorTroca()
        Dim objSimuladoDAO As Turismo.SimuladoDAO
        Dim varValor As Decimal = CDec(hddVlrOriginal1.Value) + CDec(hddVlrOriginal2.Value)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objSimuladoDAO = New Turismo.SimuladoDAO("TurismoSocialCaldas")
        Else
            objSimuladoDAO = New Turismo.SimuladoDAO("TurismoSocialPiri")
        End If
        lblVlrOriginal1.Text = ""
        lblVlrTroca1.Text = ""
        lblVlrDiferenca1.Text = ""

        If hddHosp11.Value.StartsWith("-") Or _
            hddHosp12.Value.StartsWith("-") Or _
            hddHosp13.Value.StartsWith("-") Or _
            hddHosp14.Value.StartsWith("-") Or _
            hddHosp15.Value.StartsWith("-") Or _
            hddHosp16.Value.StartsWith("-") Or _
            hddHosp17.Value.StartsWith("-") Or _
            hddHosp18.Value.StartsWith("-") Or _
            hddHosp19.Value.StartsWith("-") Or _
            hddHosp10.Value.StartsWith("-") Or _
            hddHosp21.Value.StartsWith("-") Or _
            hddHosp22.Value.StartsWith("-") Or _
            hddHosp23.Value.StartsWith("-") Or _
            hddHosp24.Value.StartsWith("-") Or _
            hddHosp25.Value.StartsWith("-") Or _
            hddHosp26.Value.StartsWith("-") Or _
            hddHosp27.Value.StartsWith("-") Or _
            hddHosp28.Value.StartsWith("-") Or _
            hddHosp29.Value.StartsWith("-") Or _
            hddHosp20.Value.StartsWith("-") Then
        Else
            hddArrastouIntegrante.Value = ""
        End If

        If hddArrastouIntegrante.Value = "Sim" Then
            If hddHospInfo11.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo11.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo12.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo12.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo13.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo13.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo14.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo14.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo15.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo15.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo16.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo16.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo17.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo17.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo18.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo18.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo19.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo19.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo10.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo10.Value & hddAcmId1.Value & "."
            End If
        Else
            If hddHospInfo11.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo11.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo12.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo12.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo13.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo13.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo14.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo14.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo15.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo15.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo16.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo16.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo17.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo17.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo18.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo18.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo19.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo19.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo10.Value > "" Then
                lblVlrOriginal1.Text += hddHospInfo10.Value & hddAcmId2.Value & "."
            End If
        End If

        Dim objSimuladoVO As Turismo.SimuladoVO = objSimuladoDAO.consultar(lblVlrOriginal1.Text, Math.Max(CInt(hddResId1.Value), CInt(hddResId2.Value)))
        If objSimuladoVO.valor >= 0 And hddAcmId1.Value > "0" Then
            If hddArrastouIntegrante.Value = "Sim" And _
                ((hddResId1.Value = hddResId2.Value) Or hddResId1.Value = 0 Or hddResId2.Value = 0) Then
                lblVlrOriginal1.Text = Format(CDec(objSimuladoVO.valorTroca), "###,##0.00")
            Else
                lblVlrOriginal1.Text = Format(CDec(objSimuladoVO.valor), "###,##0.00")
                hddVlrOriginal1.Value = objSimuladoVO.valor
            End If
            'lblVlrOriginal1.Text = Format(CDec(objSimuladoVO.valor), "###,##0.00")
        Else
            lblVlrOriginal2.Text = ""
        End If

        If objSimuladoVO.valorTroca > 0 Then
            If hddArrastouIntegrante.Value = "Sim" Then
                If varValor >= objSimuladoVO.valor Then
                    varValor -= objSimuladoVO.valor
                Else

                End If
                'lblVlrTroca1.Text = Format(CDec(objSimuladoVO.valor), "###,##0.00")
            Else
                lblVlrTroca1.Text = "- " & Format(CDec(objSimuladoVO.valorTroca), "###,##0.00") & " ="
                If objSimuladoVO.valor - objSimuladoVO.valorTroca >= 0 Then
                    lblVlrDiferenca1.Text = Format(objSimuladoVO.valor - objSimuladoVO.valorTroca, "###,##0.00")
                    lblVlrDiferenca1.ForeColor = Drawing.Color.Blue
                Else
                    lblVlrDiferenca1.Text = "(" & Format(-1 * (objSimuladoVO.valor - objSimuladoVO.valorTroca), "###,##0.00") & ")"
                    lblVlrDiferenca1.ForeColor = Drawing.Color.Red
                End If
            End If
        End If

        lblVlrOriginal2.Text = ""
        lblVlrTroca2.Text = ""
        lblVlrDiferenca2.Text = ""

        If hddArrastouIntegrante.Value = "Sim" Then
            If hddHospInfo21.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo21.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo22.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo22.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo23.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo23.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo24.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo24.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo25.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo25.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo26.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo26.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo27.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo27.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo28.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo28.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo29.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo29.Value & hddAcmId2.Value & "."
            End If
            If hddHospInfo20.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo20.Value & hddAcmId2.Value & "."
            End If
        Else
            If hddHospInfo21.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo21.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo22.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo22.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo23.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo23.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo24.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo24.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo25.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo25.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo26.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo26.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo27.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo27.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo28.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo28.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo29.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo29.Value & hddAcmId1.Value & "."
            End If
            If hddHospInfo20.Value > "" Then
                lblVlrOriginal2.Text += hddHospInfo20.Value & hddAcmId1.Value & "."
            End If
        End If

        objSimuladoVO = objSimuladoDAO.consultar(lblVlrOriginal2.Text, Math.Max(CInt(hddResId1.Value), CInt(hddResId2.Value)))
        If objSimuladoVO.valor >= 0 And hddAcmId2.Value > "0" Then
            If hddArrastouIntegrante.Value = "Sim" And _
            ((hddResId1.Value = hddResId2.Value) Or hddResId1.Value = 0 Or hddResId2.Value = 0) Then
                lblVlrOriginal2.Text = Format(CDec(objSimuladoVO.valorTroca), "###,##0.00")
            Else
                lblVlrOriginal2.Text = Format(CDec(objSimuladoVO.valor), "###,##0.00")
                hddVlrOriginal2.Value = objSimuladoVO.valor
            End If
            'lblVlrOriginal2.Text = Format(CDec(objSimuladoVO.valor), "###,##0.00")
        Else
            lblVlrOriginal2.Text = ""
        End If
        If objSimuladoVO.valorTroca > 0 Then
            If hddArrastouIntegrante.Value = "Sim" Then
                lblVlrOriginal2.Text += " ="
                If varValor - objSimuladoVO.valorTroca >= 0 Then
                    lblVlrDiferenca2.Text = Format(varValor - objSimuladoVO.valorTroca, "###,##0.00")
                    lblVlrDiferenca2.ForeColor = Drawing.Color.Blue
                Else
                    lblVlrDiferenca2.Text = "(" & Format(objSimuladoVO.valorTroca - varValor, "###,##0.00") & ")"
                    lblVlrDiferenca2.ForeColor = Drawing.Color.Red
                End If
            Else
                lblVlrTroca2.Text = "- " & Format(CDec(objSimuladoVO.valorTroca), "###,##0.00") & " ="
                If objSimuladoVO.valor - objSimuladoVO.valorTroca >= 0 Then
                    lblVlrDiferenca2.Text = Format(objSimuladoVO.valor - objSimuladoVO.valorTroca, "###,##0.00")
                    lblVlrDiferenca2.ForeColor = Drawing.Color.Blue
                Else
                    lblVlrDiferenca2.Text = "= (" & Format(-1 * (objSimuladoVO.valor - objSimuladoVO.valorTroca), "###,##0.00") & ")"
                    lblVlrDiferenca2.ForeColor = Drawing.Color.Red
                End If
            End If
        End If

        DisponibilidadeTroca()

    End Sub

    Protected Sub btnConsulta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsulta.Click
        If sender Is btnConsulta And btnConsulta.Text.Trim = "Consultar" Then
            hddResId.Value = 0
        End If
        'If cmbBloco.SelectedIndex > 0 And ckbTransferencia.Checked Then
        ' btnConsulta.CommandName = "intNome,h.hosStatus,Convert(DateTime, intDataIni, 120)"
        ' End If

        'If btnConsulta.Text = "Consultar" Then
        '    hddtxtConsulta.Value = txtConsulta.Text
        'Else
        '    txtConsulta.Text = hddtxtConsulta.Value
        'End If
        btnConsulta.Attributes.Add("resIdEnxoval", Nothing)
        If ckbTransferencia.Checked Then
            gdvReserva1.Columns(5).Visible = False
        Else
            gdvReserva1.Columns(5).Visible = True
        End If
        If (IsDate(txtConsulta.Text) And cmbBloco.SelectedIndex = 0 _
            And ckbEntrada.Checked = False _
            And ckbEstada.Checked = False _
            And ckbSaida.Checked = False _
            And ckbTransferencia.Checked = False) Then
            Mensagem("Será necessário especificar melhor a consulta, aplique mais filtros para prosseguir!")
            Exit Sub
        End If

        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        Dim objCartaoConsumoVO As New Turismo.CartaoConsumoVO
        Dim objCartaoConsumoDAO As Turismo.CartaoConsumoDAO
        Dim lista As New ArrayList
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
            objCartaoConsumoDAO = New Turismo.CartaoConsumoDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
            objCartaoConsumoDAO = New Turismo.CartaoConsumoDAO("TurismoSocialPiri")
        End If

        'Colorindo os apartamentos flutuantes do Wilton e Bloco Anhanguera
        Select Case cmbBloco.SelectedValue
            Case 0, 1, 4 '(Todos,Anhanguera,Wilton e Emissivo)
                If txtConsulta.Text.Trim.Length = 0 Then
                    ConsultaTemporada(Format(Now.Date, "dd-MM-yyyy"), Format(Now.Date, "dd-MM-yyyy"))
                Else  'IndexOf - Quando não encontra a condição retorna -1
                    If (txtConsulta.Text.Length = 10 And txtConsulta.Text.IndexOf("/").ToString <> -1) Then
                        ConsultaTemporada(Format(CDate(txtConsulta.Text), "dd-MM-yyyy"), Format(CDate(txtConsulta.Text), "dd-MM-yyyy"))
                    Else
                        ConsultaTemporada(Format(Now.Date, "dd-MM-yyyy"), Format(Now.Date, "dd-MM-yyyy"))
                    End If
                End If
        End Select

        ckbEntrada.Text = "Entrada"
        ckbEstada.Text = "Estada"
        ckbSaida.Text = "Saída"

        If IsNumeric(txtConsulta.Text) And (txtConsulta.Text.Length = 6) Then 'Cartão de consumo
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), _
            '"alert('" & hddOperacao.Value & "');", True)

            If hddOperacao.Value = "false" Then
                If hddStatusIntegrante.Value <> "3" Then
                    lista = objCheckInOutDAO.consultar("0", "Cartao", txtConsulta.Text, 0, 0, True, True, True, True, btnConsulta.CommandName) 'ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked)
                    hddCartao.Value = txtConsulta.Text
                    If lista.Count = 0 Then
                        hddFinalizaPor.Value = "S"
                        objCartaoConsumoVO = objCartaoConsumoDAO.consultar(txtConsulta.Text.Trim)
                        Select Case objCartaoConsumoVO.carSituacao
                            Case "B"
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), _
                                  "alert('Cartão bloqueado. Retenha-o.');", True)
                            Case "L"
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), _
                                 "alert('Cartão livre.');", True)
                        End Select
                        txtConsulta.Text = ""
                    Else
                        Dim objCheckInVO As New Turismo.CheckInOutVO
                        objCheckInVO = lista.Item(0)
                        If objCheckInVO.intStatus = "S" Or objCheckInVO.estada = "2" Or ckbSaida.Checked Or objCheckInVO.intDataFim < Date.Now.Date Then
                            hddIntId.Value = objCheckInVO.intId
                            If objCheckInVO.estada = "2" Then
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('Deseja finalizar integrante " + objCheckInVO.intNome.Trim + " com check-out " + objCheckInVO.intDataFim + "?')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='C';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
                            Else
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('Deseja finalizar integrante " + objCheckInVO.intNome.Trim + "?')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='C';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
                            End If
                        End If
                        'txtConsulta.Text = objCheckInVO.intNome.Substring(0, objCheckInVO.intNome.IndexOf("("))
                    End If
                ElseIf hddStatusIntegrante.Value = "3" Then
                    'Dim bd As String
                    'If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    'bd = "TurismoSocialCaldas"
                    'Else
                    'bd =  "TurismoSocialPiri"
                    'End If
                    'Dim objCartaoConsumoDAO = New Turismo.CartaoConsumoDAO(bd)
                    Dim Retorno As SByte
                    Retorno = objCartaoConsumoDAO.atribuir(txtConsulta.Text, gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item("intId").ToString, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                    If Retorno = -1 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão em uso pelo integrante." + "');", True)
                    ElseIf Retorno = -2 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão em uso por outro integrante." + "');", True)
                    ElseIf Retorno = -3 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão Bloqueado." + "');", True)
                    ElseIf Retorno = -4 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Integrante já possui cartão." + "');", True)
                    ElseIf Retorno = 0 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
                        'Else
                        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento foi atribuído." + "'); document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnModalPopUp.click();", True)
                    End If
                    txtConsulta.Text = ""
                    lista = objCheckInOutDAO.consultar(0, "", "", hddResId.Value, hddIntId.Value, True, True, True, True, btnConsulta.CommandName)
                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Utilize a leitora de cartão." + "');", True)
                If hddStatusIntegrante.Value = "3" Then
                    lista = objCheckInOutDAO.consultar(0, "", "", hddResId.Value, hddIntId.Value, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
                End If
                txtConsulta.Text = ""
                txtConsulta.Focus()
            End If
            gdvReserva1.DataSource = lista
            gdvReserva1.DataBind()
            gdvReserva1.Enabled = Session("GrupoRecepcaoPiri") Or Session("GrupoRecepcao")
            txtConsulta.Focus()
            txtConsulta.Text = ""
        Else
            If btnConsulta.Text = "  Voltar" Then
                txtConsulta.Text = hddtxtConsulta.Value
            End If
            If hddResId.Value = "" Then
                hddResId.Value = "0"
            End If
            btnAtualizar_Click(sender, e)
            lista = objCheckInOutDAO.consultarTipos(cmbBloco.SelectedValue, txtConsulta.Text, hddResId.Value, 0, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            hddStatusIntegrante.Value = ""

            'If InStr(txtConsulta.Text, "/") > 0 Then 'Data
            '    hddStatusIntegrante.Value = ""
            '    If IsDate(txtConsulta.Text) Then
            '        lista = objCheckInOutDAO.consultar(cmbBloco.SelectedValue, "Dia", Format(CDate(txtConsulta.Text), "yyyy-MM-dd"), 0, 0, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            '    Else
            '        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Data inválida." + "');", True)
            '    End If
            'Else
            '    hddStatusIntegrante.Value = ""
            '    Dim i As Integer
            '    Dim Numero As Integer = 0
            '    Dim Caracter As Integer = 0
            '    For i = 1 To txtConsulta.Text.Trim.Length
            '        If IsNumeric(txtConsulta.Text.Substring(i - 1, 1)) Then
            '            Numero += 1
            '        ElseIf txtConsulta.Text.Substring(i - 1, 1) <> " " Then
            '            Caracter += 1
            '        End If
            '    Next
            '    If Caracter = 3 And Numero = 4 Then 'Placa
            '        lista = objCheckInOutDAO.consultar(cmbBloco.SelectedValue, "Placa", txtConsulta.Text, 0, 0, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            '    ElseIf Caracter >= 0 And Numero > 0 Then 'Apartamento
            '        lista = objCheckInOutDAO.consultar(cmbBloco.SelectedValue, "Apartamento", txtConsulta.Text, 0, 0, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            '    ElseIf hddCartao.Value > "" Then 'Volta de ação feito após consulta via cartão
            '        lista = objCheckInOutDAO.consultar("0", "Cartao", hddCartao.Value, 0, 0, True, True, True, True, btnConsulta.CommandName) 'ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked)
            '        hddCartao.Value = ""
            '    Else 'Nome ou placa (se 3 letras)
            '        lista = objCheckInOutDAO.consultar(cmbBloco.SelectedValue, "Nome", txtConsulta.Text, 0, 0, ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            '    End If
            'End If

            If Not ckbEntrada.Checked And Not ckbEstada.Checked And Not ckbSaida.Checked And Not ckbTransferencia.Checked Then
                gdvReserva1.DataSource = lista
            Else
                Dim TipoData As String = Format(CDate(Date.Now), "dd/MM/yyyy")
                If InStr(txtConsulta.Text, "/") > 0 Then 'Tem Data
                    TipoData = Mid(txtConsulta.Text, txtConsulta.Text.IndexOf("/") - 1, 10) 'Format(CDate(Mid(Valor, Valor.IndexOf("/") - 1, 10)), "yyyy-MM-dd")
                End If

                Dim listaAux As New ArrayList
                For Each item As Turismo.CheckInOutVO In lista
                    'If (ckbEntrada.Checked And (item.estada <> "1" And (item.intStatus = Nothing Or item.intStatus = "P" Or item.intStatus = " "))) Or _
                    If (ckbEntrada.Checked And ((item.estada <> "1" And item.estada <> "4" And item.estada <> "3" And item.estada <> "6" And item.estada <> "7" And item.estada <> "8") _
                                                And (item.intStatus = Nothing Or item.intStatus = "P" Or item.intStatus = Nothing))) Or _
                    (cmbBloco.SelectedValue = "E" And ckbEntrada.Checked And item.estada = "2" And item.intStatus = Nothing) Or _
                    (ckbEstada.Checked And item.estada = "1" And (item.intStatus = "E" Or item.intStatus = "P" Or item.intStatus = "T")) Or _
                    (ckbEstada.Checked And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E" And item.estada <> "6" And item.estada <> "7" And item.estada <> "8")) Or _
                    (ckbSaida.Checked And txtConsulta.Text = "" And CDate(item.intDataFim) <= Date.Now And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                    (ckbSaida.Checked And TipoData > "" And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                    (ckbTransferencia.Checked And (item.estada = "6" Or item.estada = "7" Or item.estada = "8" Or item.estada = "1")) Then
                        '(ckbEstada.Checked And item.intStatus = "T" And item.estada = "0") Then
                        listaAux.Add(item)
                    End If
                    'A Linha abaixo foi retirada pois mostrava apenas os check in do dia e não mostrava as trasferencias
                    '(ckbSaida.Checked And TipoData > "" And CDate(item.intDataFim) = CDate(TipoData) And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                    '<>6 inserido para não mostrar os check in futuros Linha 839 - Estada - Por Washington em 30-12-2013
                    'Eu Washington Inseri item.intStatus = "T" na linha do ckbEstada pois não estava mostrando o integrante quando clicava em estada e consultava

                    '(ckbEstada.Checked And item.intStatus = "T" And item.estada = "0") Then

                    '(ckbEstada.Checked And item.estada <> "0" And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                    '(ckbSaida.Checked And txtConsulta.Text = "" And CDate(item.intDataFim) <= Date.Now And item.estada <> "0" And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                    '(ckbSaida.Checked And TipoData > "" And CDate(item.intDataFim) = CDate(TipoData) And item.estada <> "0" And (item.intStatus = "S" Or item.intStatus = "P" Or item.intStatus = "E")) Or _
                Next
                gdvReserva1.DataSource = listaAux
            End If

            'gdvReserva1.Columns(3).Visible = (cmbBloco.Text <> "E")
            'gdvReserva1.Columns(4).Visible = (cmbBloco.Text <> "E")
            gdvReserva1.DataBind()
            gdvReserva1.Enabled = Session("GrupoRecepcaoPiri") Or Session("GrupoRecepcao")
            gdvReserva1.SelectedIndex = -1
            txtConsulta.Focus()
            If hddStatusIntegrante.Value = "3" And gdvReserva1.Rows.Count > 0 Then
                gdvReserva1.SelectedIndex = 0
            Else
                If btnConsulta.Text = "  Voltar" Then
                    txtConsulta.Text = hddtxtConsulta.Value
                End If
                'pnlTroca.Visible = True
                lblConsulta.Text = "  Consultar"
                hddStatusIntegrante.Value = ""
                btnConsulta.Text = "  Consultar"
                imgCalendario.Visible = True
                imgCalendario_CalendarExtender.Enabled = True
                cmbBloco.Enabled = True
                ckbEntrada.Visible = True
                ckbEstada.Visible = True
                ckbSaida.Visible = True
                ckbTransferencia.Visible = True
                imgBtnReservaNova.Visible = True
                gdvReserva1.Columns(0).Visible = True
            End If
        End If
        If gdvReserva1.Rows.Count > 0 Then
            btnPdf.Visible = True
        Else
            btnPdf.Visible = False
        End If
    End Sub

    Protected Sub gdvReserva1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            contEntrada = 0
            contEstada = 0
            contSaida = 0
            contTransf = 0
            CompletarEnxoval = False
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then

            'Controle da celula de completar enxoval
            If ckbTransferencia.Checked = False Then
                'Se ao percorrer o grid encontrar um apto atribuido, ira setar a variavel com true.
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString.Trim.Length > 0 Then
                    CompletarEnxoval = True
                End If

                If ((IsNothing(btnConsulta.Attributes.Item("resIdEnxoval")) Or _
                   btnConsulta.Attributes.Item("solIdEnxoval") <> gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId")) And _
                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString.Trim.Length > 0) Then
                    'And gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString.Trim.Length > 0
                    btnConsulta.Attributes.Add("resIdEnxoval", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                    btnConsulta.Attributes.Add("solIdEnxoval", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    CType(e.Row.FindControl("divCompletarEnxoval"), HtmlControl).Visible = True
                    'Adicionando ToolTip Personalizado 		
                    If ((gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString.Trim.Length > 0) Or _
                        (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Or _
                        (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S")) Then


                        If ((gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString.Trim.Length = gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length) And _
                        (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString) And _
                        (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString)) Then
                            CType(e.Row.FindControl("imgSalvarEnxoval"), ImageButton).ImageUrl = "~\images\AtendidoEnxovalTrue.png"
                        Else
                            CType(e.Row.FindControl("imgSalvarEnxoval"), ImageButton).ImageUrl = "~\images\AtendidoEnxoval.png"
                        End If


                        'Tratando para exibição no toolTip
                        Dim BercoAtendido As String = "", BanheiraAtendida As String = ""

                        If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S" Then
                                BercoAtendido = "Berço atendido"
                            Else
                                BercoAtendido = "Berçonão Atendido"
                            End If
                        Else
                            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "R" Then
                                BercoAtendido = "Aguardando a retirada do berço"
                            Else
                                BercoAtendido = ""
                            End If
                        End If

                        If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S" Then
                                BanheiraAtendida = "Banheira atendida"
                            Else
                                BanheiraAtendida = "Banheir não Atendida"
                            End If
                        Else
                            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "R" Then
                                BanheiraAtendida = "Aguardando retirada da banheira"
                            Else
                                BanheiraAtendida = ""
                            End If
                        End If

                        'Monta ToolTipo
                        Dim ToolEnxoval As String
                        'ToolEnxoval = "--------Situação--------"
                        If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString.Trim.Length > 0) Then
                            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString = gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString Then
                                ToolEnxoval = ToolEnxoval & "Completado para " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString & " Camas"
                            Else
                                ToolEnxoval = ToolEnxoval & vbNewLine & "Camas ainda não completadas"
                            End If
                        End If

                        If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Then
                            If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "R") Then
                                ToolEnxoval = ToolEnxoval & vbNewLine & BercoAtendido
                            End If
                        End If

                        If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Then
                            If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "R") Then
                                ToolEnxoval = ToolEnxoval & vbNewLine & BanheiraAtendida
                            End If
                        End If

                        If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxMotivoNaoAtendimento").ToString.Trim.Length > 0) Then
                            ToolEnxoval = ToolEnxoval & vbNewLine & "Motivo: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxMotivoNaoAtendimento").ToString
                        End If
                        ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
                        ToolEnxoval = ToolEnxoval & vbNewLine & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
                        ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
                        ToolEnxoval = ToolEnxoval & vbNewLine & "-------Atendente--------"
                        ToolEnxoval = ToolEnxoval & vbNewLine & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioAtendimento").ToString
                        ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataAtendimento").ToString
                        CType(e.Row.FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = ToolEnxoval
                    ElseIf ((gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length = 0) And _
                           (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "N" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "") And _
                           (gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "N" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "")) Then
                        CType(e.Row.FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = "Atendimento não solicitado"
                    Else
                        Dim ToolEnxoval As String = ""
                        ToolEnxoval = "---------Status---------"
                        ToolEnxoval = ToolEnxoval & vbNewLine & "Aguardando atendimento"
                        ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
                        ToolEnxoval = ToolEnxoval & vbNewLine & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
                        ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
                        CType(e.Row.FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = ToolEnxoval
                    End If

                    'Se tiver uma pendencia de retirada de berço ou banheira irá desativar o campo
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "R" Then
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(0).Enabled = False
                    End If

                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "R" Then
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(1).Enabled = False
                    End If

                    'Se for Caldas Novas terá o controle de máximo de camas a ser completada, se for Piri ficará 5 até a Duanny se manifestar
                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Clear()
                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Add(1)
                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Add(2)
                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Add(3)
                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Add(4)
                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Add(5)

                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        Select Case gdvReserva1.DataKeys(e.Row.RowIndex).Item("BloId").ToString
                            Case "1", "3" 'Anhanguera e Kilzer
                                'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(1)
                                'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(2)
                                CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(3)
                                CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(5)
                            Case "2" 'Bambui
                                'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(1)
                                'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(2)
                                CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(3)
                            Case "4" 'Wilton Honorato Normais
                                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString.Contains("WHe") Then
                                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(1)
                                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(2)
                                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(4)
                                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(5)
                                Else
                                    CType(e.Row.FindControl("lblCamaExtra"), Label).Text = ""
                                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(1)
                                    'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(2)
                                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(3)
                                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(4)
                                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(5)
                                End If
                        End Select
                    ElseIf Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" Then
                        'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(1)
                        'CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(2)
                        CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).Items.Remove(3)
                    End If

                    'Exibe os dados já gravados no banco na tela
                    CType(e.Row.FindControl("rdCamaExtra"), CheckBoxList).SelectedValue = gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(0).Selected = True
                    Else
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(0).Selected = False
                    End If

                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(1).Selected = True
                    Else
                        CType(e.Row.FindControl("chkComplemento"), CheckBoxList).Items.Item(1).Selected = False
                    End If
                Else
                    CType(e.Row.FindControl("divCompletarEnxoval"), HtmlControl).Visible = False
                    'btnConsulta.Attributes.Add("resIdEnxoval", "0")
                End If
            End If

            'CType(e.Row.FindControl("lblServidor"), Label).Text += gdvReserva1.DataKeys(e.Row.RowIndex).Item("intUsuario").ToString.Replace("SESC-GO.COM.BR\", "").Replace(".", " ")
            CType(e.Row.FindControl("imgUsuario"), Image).ToolTip += gdvReserva1.DataKeys(e.Row.RowIndex).Item("intUsuario").ToString.Replace("SESC-GO.COM.BR\", "").Replace(".", " ")

            'If gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "T" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" Then
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Visible = False
                CType(e.Row.FindControl("lnkNome"), LinkButton).Enabled = False
                'CType(e.Row.FindControl("lnkNome"), LinkButton).Text += " - Transferir"
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "7" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Visible = False
                CType(e.Row.FindControl("lnkNome"), LinkButton).Enabled = False
                'CType(e.Row.FindControl("lnkNome"), LinkButton).Text += " - Transferir"
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "8" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Visible = False
                CType(e.Row.FindControl("lnkNome"), LinkButton).Enabled = False
                'CType(e.Row.FindControl("lnkNome"), LinkButton).Text += " - Transferir"

            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("intPlacaVeiculo").ToString.Trim > "" Then
                CType(e.Row.FindControl("imgPlaca"), Image).Visible = True
                CType(e.Row.FindControl("lblPlaca"), Label).Visible = True
                CType(e.Row.FindControl("lblPlaca"), Label).Text = gdvReserva1.DataKeys(e.Row.RowIndex).Item("intPlacaVeiculo").ToString & "-" & gdvReserva1.DataKeys(e.Row.RowIndex).Item("IntModeloVeiculo").ToString
            End If
            CType(e.Row.FindControl("lnkResponsavel"), LinkButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("lnkNome"), LinkButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("lnkNome"), LinkButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            CType(e.Row.FindControl("lnkNome"), LinkButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
            CType(e.Row.FindControl("lnkNome"), LinkButton).Attributes.Add("hosId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("hosId"))
            CType(e.Row.FindControl("lnkNome"), LinkButton).Attributes.Add("resCaracteristica", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resCaracteristica"))

            CType(e.Row.FindControl("lblPeriodo"), Label).Text = _
                Mid(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataIni").ToString, 1, 5) _
                + " a " + Mid(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim").ToString, 1, 5) _
                + " " + gdvReserva1.DataKeys(e.Row.RowIndex).Item("resHoraSaida").ToString + "h"

            If ((CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) < Now.Date) Or _
                (CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) = Now.Date And _
                CInt(gdvReserva1.DataKeys(e.Row.RowIndex).Item("resHoraSaida")) < Now.Hour)) _
                And ((gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Isento") Or (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Não")) Then
                'And (gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "1" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "2" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "3" _

                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeCinza.png"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Período vencido"
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                ''

                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "6"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("cartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Finalizar integrante?')"
                'CType(e.Row.FindControl("btnFinalizar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                'CType(e.Row.FindControl("btnFinalizar"), Button).Visible = True
                'If gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim" Then
                'CType(e.Row.FindControl("btnFinalizar"), Button).OnClientClick = "return confirm('Finalizar integrante sem o cartão?')"
                'End If
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus") = "I" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeRoxo.png"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Ir para dados do integrante"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("hosId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("hosId"))
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "5"
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus") = "P" And _
                gdvReserva1.DataKeys(e.Row.RowIndex).Item("devePagto") = "1" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeVermelho.png"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Ir para dados do integrante"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("hosId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("hosId"))
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                ''

                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "4"
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Não" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResRecreandoEscolar") <> "S" Then
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeAmarelo.png"
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                'CType(e.Row.FindControl("btnAtribuirCartao"), Button).Visible = True
                'CType(e.Row.FindControl("btnAtribuirCartao"), Button).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                'CType(e.Row.FindControl("btnAtribuirCartao"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                ''

                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "3"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Clique para atribuir cartão"
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            ElseIf (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Não" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResRecreandoEscolar") = "S") _
                And (gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "E" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "P" _
                Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "S" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "T") Then
                'CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeAzul.png"

                Dim TransferenciaProgramada As String = ""
                'Se houver uma transferencia programada para o hóspede irá aparecer uma imagem "Transferencia com boneco azul"
                If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("Transferencia").ToString = "S" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "T") Then
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeAzulTransf.png"
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "5" Then
                        TransferenciaProgramada = "Existe uma transferência programada para esse integrate.Clique para a entrada do integrante."
                    Else
                        TransferenciaProgramada = "Existe uma transferência programada para esse integrate.Clique para finalizar integrante sem o cartão."
                    End If

                Else
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeAzul.png"
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "5" Then
                        TransferenciaProgramada = "Clique para a entrada do integrante."
                    Else
                        TransferenciaProgramada = "Clique para finalizar integrante sem o cartão."
                    End If
                End If

                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "5" Then
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "2"
                    'CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Clique para a entrada do integrante"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = TransferenciaProgramada
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Confirma a entrada do integrante?')"
                Else
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "1"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Finalizar integrante sem o cartão?')"
                    'CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Clique para finalizar integrante sem o cartão"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = TransferenciaProgramada
                End If

                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intNome", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intNome"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("apaDesc", gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("cartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intCartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intCartao"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intStatus", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intDataFim", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim"))
                CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("acmId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("acmId"))

                'CType(e.Row.FindControl("btnFinalizar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                'CType(e.Row.FindControl("btnFinalizar"), Button).OnClientClick = "return confirm('Finalizar integrante sem o cartão?')"
                'CType(e.Row.FindControl("btnFinalizar"), Button).Visible = True
                'CType(e.Row.FindControl("btnBloquearCartao"), Button).Visible = True
                'CType(e.Row.FindControl("btnBloquearCartao"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                'CType(e.Row.FindControl("btnBloquearCartao"), Button).Attributes.Add("intCartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intCartao"))

                CType(e.Row.FindControl("imgBtnBloquearCartao"), ImageButton).Visible = (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim")
                CType(e.Row.FindControl("imgBtnBloquearCartao"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                CType(e.Row.FindControl("imgBtnBloquearCartao"), ImageButton).Attributes.Add("intCartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intCartao"))

                'CType(e.Row.FindControl("btnEmprestimo"), Button).Visible = True
                'CType(e.Row.FindControl("btnEmprestimo"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("consumo") = "S" Then
                    CType(e.Row.FindControl("imgBtnEmprestimo"), ImageButton).ImageUrl = "~/images/EmprestimoOk.png"
                End If
                CType(e.Row.FindControl("imgBtnEmprestimo"), ImageButton).Visible = (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Isento")
                CType(e.Row.FindControl("imgBtnEmprestimo"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))

            Else
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resCaracteristica") = "P" Then
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeVerde.png"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "7"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Clique para a entrada do passeio"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Confirma a entrada do passeio?')"

                    'CType(e.Row.FindControl("btnEntrar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    'CType(e.Row.FindControl("btnEntrar"), Button).CommandArgument = 7
                    'CType(e.Row.FindControl("btnEntrar"), Button).ToolTip = "Clique para a entrada do passeio"
                    'CType(e.Row.FindControl("btnEntrar"), Button).OnClientClick = "return confirm('Confirma a entrada do passeio?')"
                    'CType(e.Row.FindControl("btnEntrar"), Button).Visible = True

                Else
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeVerde.png"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "2"
                    CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Isento" Then
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                        ''

                    End If
                    If CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) < Date.Now.Date Then
                        'CType(e.Row.FindControl("btnFinalizar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                        'CType(e.Row.FindControl("btnFinalizar"), Button).Visible = True
                        'CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Enabled = False
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CssClass = "opaco"
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Período vencido"
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).CommandArgument = "6"
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Attributes.Add("cartao", gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao"))
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Finalizar integrante?')"
                    Else
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ToolTip = "Clique para a entrada do integrante"
                        CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).OnClientClick = "return confirm('Confirma a entrada do integrante?')"
                        'CType(e.Row.FindControl("btnEntrar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                        'CType(e.Row.FindControl("btnEntrar"), Button).CommandArgument = 2
                        'CType(e.Row.FindControl("btnEntrar"), Button).ToolTip = "Clique para a entrada do integrante"
                        'CType(e.Row.FindControl("btnEntrar"), Button).OnClientClick = "return confirm('Confirma a entrada do integrante?')"
                        'CType(e.Row.FindControl("btnEntrar"), Button).Visible = True
                    End If
                End If
            End If

            If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc") > "") Then
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaStatus") = "L" Then
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoBranco.gif"
                    '0-Sem o check in,1-Integrante em estada,3-Conferencia, 6-Transferencias do dia,7-Transferencias futuras,8-Transferencias atrasadas
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = (gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" Or _
                                                                                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "1" Or _
                                                                                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "3" Or _
                                                                                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Or _
                                                                                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "7" Or _
                                                                                   gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "8")
                    'And (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim")
                    '0-Sem o check in,1-Em Estada,3-Conferencia,4-Encerrado e esta no P.Aquatico,5-Integrante não compareceu,6-Transferencia no dia, 8-Transferencias atrasas
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = (gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "1" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "3" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "4" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "5" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Or _
                                                                                    gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "8") And _
                         (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim")

                    ''

                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaStatus") = "O" Then
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoAzul.gif"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"
                    'Essa parte estava comentada porem não estava sendo permitido liberar apto ocupado com transferencia.
                    'If gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "T" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" Then
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Then
                    ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "7" Then
                    ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "8" Then
                    Else
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = "alert('Apartamento ocupado.'); return false;"
                    End If

                    ''

                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaStatus") = "A" Then
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoAmarelo.gif"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"

                    ''

                Else
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoCinza.gif"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "3"
                    ''
                End If

                'Esse status 5 foi adicionado por mim Washington/tem 3 pessoas em uma reserva e só vem 2, esse terceiro que não veio é o status 5
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "1" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "5" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/ChaveEntregue.gif"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/ChaveEntregue.gif")
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para desocupar o apartamento"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a desocupação do apartamento?')"

                    CType(e.Row.FindControl("imgBtnPermutar"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Isento"
                    CType(e.Row.FindControl("imgBtnPermutar"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))

                    CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Isento"
                    CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                    CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Attributes.Add("acmId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("acmId"))
                    CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Attributes.Add("intDataFim", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim"))

                    'Se a reserva estiver finalizando hoje, não será exibido o botão de transferencia
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim").ToString = Format(Date.Now, "dd/MM/yyyy") And TimeOfDay.Hour > 8 Then
                        CType(e.Row.FindControl("imgBtnTransferir"), ImageButton).Visible = False
                    End If

                    'CType(e.Row.FindControl("btnDesocupar"), Button).Attributes.Add("origem", "~/images/ChaveEntregue.gif")
                    'CType(e.Row.FindControl("btnDesocupar"), Button).Visible = True
                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = ""
                    ''CType(e.Row.FindControl("imgBtnApto_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") <> "T" Then
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Apartamento já ocupado"
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "O"
                    Else
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = "return confirm('Confirma a liberação do apartamento?')"
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para liberar o apartamento"
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "S"
                    End If

                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") <> "T" Then
                    If gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim" Then
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/ChaveNaoEntregue.gif"
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/ChaveNaoEntregue.gif")
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para entregar a chave ao integrante"
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a ocupação do apartamento?')"
                    End If

                    'CType(e.Row.FindControl("btnOcupar"), Button).Attributes.Add("origem", "~/images/ChaveNaoEntregue.gif")
                    'CType(e.Row.FindControl("btnOcupar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    'CType(e.Row.FindControl("btnOcupar"), Button).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = "return confirm('Confirma a liberação do apartamento?')"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para liberar o apartamento"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "S"
                    'CType(e.Row.FindControl("btnLiberar"), Button).Visible = True
                    'CType(e.Row.FindControl("btnLiberar"), Button).AccessKey = "S"

                    'Tratando transferencias (6- do Dia)
                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/transferencia.png"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/transferencia.png")
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para transferir os integrantes"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a transferência do apartamento?')"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))

                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("origem", "~/images/transferencia.png")
                    'CType(e.Row.FindControl("btnTransferir"), Button).Visible = True
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para liberar o apartamento"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "T"
                    'Tratando transferencias (7 - Futuro)
                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "7" Then
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/transferencia.png"
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/transferencia.png")
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para transferir os integrantes"
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a transferência do apartamento?')"
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnEmprestimo"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnBloquearCartao"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).Visible = False

                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("origem", "~/images/transferencia.png")
                    'CType(e.Row.FindControl("btnTransferir"), Button).Visible = True
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para liberar o apartamento"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "T"
                    'Tratando transferencias (8 - Atrasado)
                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "8" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/transferencia.png"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/transferencia.png")
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para transferir os integrantes"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a transferência do apartamento?')"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))

                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("origem", "~/images/transferencia.png")
                    'CType(e.Row.FindControl("btnTransferir"), Button).Visible = True
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
                    'CType(e.Row.FindControl("btnTransferir"), Button).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para liberar o apartamento"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "T"


                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "2" Or gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "4" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/ok.gif"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/ok.gif")
                    If CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) < Date.Now.Date Then
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).CssClass = "opaco"
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Chave devolvida"
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).Enabled = False
                    Else
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Clique para a reentrega da chave"
                        CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "return confirm('Confirma a reentrega da chave?')"
                    End If
                    'CType(e.Row.FindControl("btnReocupar"), Button).Attributes.Add("origem", "~/images/ok.gif")

                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = ""
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = ""
                ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "3" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/Conferencia.png"
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("origem", "~/images/Conferencia.png")
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).ToolTip = "Apartamento aguardando conferência."
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "var Tempo = new Date('" & gdvReserva1.DataKeys(e.Row.RowIndex).Item("dataPedidoConferencia") & "'); var Atual = new Date(); alert('Apartamento aguardando conferência. Tempo decorrido ' + Math.floor((Atual - Tempo) / 60000) + ' minutos.'); return false;"
                    'CType(e.Row.FindControl("imgBtnChave"), ImageButton).OnClientClick = "var Tempo = new Date(); alert('Apartamento aguardando conferência. Tempo decorrido ''' + Tempo + '''); return false;"

                    ''

                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).OnClientClick = ""
                    ''CType(e.Row.FindControl("imgBtnApto_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = ""
                End If

            Else 'Não possui apartamento atribuído
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    'CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/InterrogacaoAzul.png"
                    Select Case gdvReserva1.DataKeys(e.Row.RowIndex).Item("BloId")
                        Case 1
                            CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/AptoA.png"
                        Case 2
                            CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/AptoB.png"
                        Case 3
                            CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/AptoK.png"
                        Case 4
                            CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/AptoW.png"
                        Case Else
                            CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/InterrogacaoAzul.png"
                    End Select
                Else
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/InterrogacaoVerde.png"
                End If
                'CType(e.Row.FindControl("lnkApto"), LinkButton).Enabled = False
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                'Se a houver uma transferencia futura irá esconder o botões abaixo
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "7" Then
                    CType(e.Row.FindControl("imgBtnEmprestimo"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnBloquearCartao"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).Visible = False
                End If

                If CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) < Date.Now.Date Then
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = False
                Else
                    'CType(e.Row.FindControl("btnEscolher"), Button).Visible = (gdvReserva1.DataKeys(e.Row.RowIndex).Item("resCaracteristica") <> "P")
                    'CType(e.Row.FindControl("btnEscolher"), Button).AccessKey = "N"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).ToolTip = "Clique para selecionar o apartamento"
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).AccessKey = "N"
                End If
            End If
            CType(e.Row.FindControl("imgBtnApto"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            CType(e.Row.FindControl("imgBtnApto"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
            'CType(e.Row.FindControl("btnEscolher"), Button).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
            'CType(e.Row.FindControl("btnLiberar"), Button).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
            'CType(e.Row.FindControl("imgBtnChave"), ImageButton).AlternateText = gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId")
            'CType(e.Row.FindControl("imgBtnChave"), ImageButton).CommandArgument = gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId")
            CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("imgBtnChave"), ImageButton).Attributes.Add("apaId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaId"))

            'CType(e.Row.FindControl("btnDesocupar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            'CType(e.Row.FindControl("btnDesocupar"), Button).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
            'CType(e.Row.FindControl("btnDesocupar"), Button).Attributes.Add("apaId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaId"))

            'CType(e.Row.FindControl("btnReocupar"), Button).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            'CType(e.Row.FindControl("btnReocupar"), Button).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
            'CType(e.Row.FindControl("btnReocupar"), Button).Attributes.Add("apaId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaId"))

            CType(e.Row.FindControl("imgBtnAcomodacao"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("resHospede") > "1" And CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) >= Date.Now And gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = Nothing And gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim"
            CType(e.Row.FindControl("imgBtnAcomodacao"), ImageButton).Attributes.Add("solId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("solId"))
            CType(e.Row.FindControl("imgBtnReserva"), ImageButton).Visible = gdvReserva1.DataKeys(e.Row.RowIndex).Item("solHospede") > "1" And CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("intDataFim")) >= Date.Now And gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = Nothing And gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") = "Sim"
            CType(e.Row.FindControl("imgBtnReserva"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))

            If (gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Isento") Then
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).ImageUrl = "~/images/CortesiaAzul.png"
                Else
                    CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).ImageUrl = "~/images/CortesiaVerde.png"
                End If
                CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).Attributes.Add("resId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId"))
                CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).Attributes.Add("intId", gdvReserva1.DataKeys(e.Row.RowIndex).Item("intId"))
            Else
                CType(e.Row.FindControl("imgBtnCortesia"), ImageButton).Visible = False
            End If

            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus") = "I" Then
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                ''

            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus") = "P" Then
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

                ''
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("cartao") <> "Sim" Then
                CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = False

            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus") = "E" Then
                'Se tiver número de apartamento e a não possuir transferencia futura
                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("apaDesc") > "" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "7" Then
                    CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible = True

                    ''

                End If
            End If

            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("checkInLiberado") > "0" Then
                CType(e.Row.FindControl("imgBtnAcomodacao"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgBtnReserva"), ImageButton).Visible = False
                'CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).Visible = (CType(e.Row.FindControl("imgBtnIntegrante"), ImageButton).ImageUrl = "~/images/HospedeVermelho.png")
            End If
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resCaracteristica") = "P" Then
                CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = False
            End If
            'If (CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/ok.gif") And _
            '(CType(e.Row.FindControl("imgBtnChave"), ImageButton).Enabled = True) Then
            'CType(e.Row.FindControl("btnReocupar"), Button).Visible = CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible
            'End If
            'CType(e.Row.FindControl("btnOcupar"), Button).Visible = CType(e.Row.FindControl("imgBtnChave"), ImageButton).Visible _
            'And CType(e.Row.FindControl("imgBtnChave"), ImageButton).ImageUrl = "~/images/ChaveNaoEntregue.gif"
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = Nothing Then
                contEntrada += 1
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "S" Then
                contSaida += 1
                contEstada += 1
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "E" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") <> "0" Then
                contEstada += 1
                'ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("intStatus") = "T" And gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "0" Then
            ElseIf gdvReserva1.DataKeys(e.Row.RowIndex).Item("estada") = "6" Then
                contTransf += 1
            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            'If contEntrada > 0 Then
            'ckbEntrada.Text = contEntrada.ToString & " Entrada"
            'End If
            'If contEstada > 0 And ((ckbEstada.Checked) Or (Not ckbEntrada.Checked And Not ckbEstada.Checked And Not ckbSaida.Checked And Not ckbTransferencia.Checked)) Then
            'ckbEstada.Text = contEstada.ToString & " Estada"
            'End If
            'If contSaida > 0 Then
            'ckbSaida.Text = contSaida.ToString & " Saída"
            'End If

            If CompletarEnxoval = True Then
                gdvReserva1.Columns(5).Visible = True
            Else
                gdvReserva1.Columns(5).Visible = False
            End If

            If contTransf > 0 Then
                ckbTransferencia.Text = contTransf.ToString & " Transferência(s)"
            Else
                ckbTransferencia.Text = "Transferência"
            End If
        End If
    End Sub

    Protected Sub imgBtnApto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnLiberar_Click(sender, e)
    End Sub

    Protected Sub lnkResponsavel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            hddResId.Value = sender.Attributes("resId")
            hddIntId.Value = ""
            hddSolId.Value = ""
            hddHosId.Value = ""
            Server.Transfer("~/Reserva.aspx", True)
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub rblLimpo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblLimpo.SelectedIndexChanged, rblArrumacao.SelectedIndexChanged, rblOcupado.SelectedIndexChanged, rblManutencao.SelectedIndexChanged
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objAptoCheckInOutDAO = New Turismo.AptoCheckInOutDAO(bd)
        Dim Retorno As SByte
        Retorno = objAptoCheckInOutDAO.AtribuirApto(hddSolId.Value, sender.Text, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        If Retorno = -1 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento já foi atribuido para a acomodação." + "');", True)
        ElseIf Retorno = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento foi atribuído." + "'); document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnModalPopUpApto.click();", True)
        End If
        btnConsulta_Click(sender, e)
        Try
            For Each linha As GridViewRow In gdvReserva1.Rows
                If (gdvReserva1.DataKeys(linha.RowIndex).Item(2).ToString = hddSolId.Value) Then
                    gdvReserva1.SelectedIndex = linha.RowIndex
                    Exit For
                End If
            Next
            gdvReserva1.SelectedRow.Cells(3).Focus()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub imgBtnChave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnDesocupar_Click(sender, e)
    End Sub

    Protected Sub btnModalPopUpApto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModalPopUpApto.Click
        Try
            If gdvReserva1.SelectedRow.Cells(3).FindControl("imgBtnChave").Visible Then
                gdvReserva1.SelectedRow.Cells(3).Focus()
            Else
                gdvReserva1.SelectedRow.Cells(2).Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkNome_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            hddResId.Value = sender.Attributes("resId")
            hddIntId.Value = sender.Attributes("intId")
            hddSolId.Value = sender.Attributes("solId")
            hddHosId.Value = sender.Attributes("hosId")
            hddResCaracteristica.Value = sender.Attributes("resCaracteristica")
            'resCaracteristica
            Server.Transfer("~/Reserva.aspx", True)
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub gdvReserva1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva1.SelectedIndexChanged
        Try
            hddResId.Value = gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item("resId").ToString
            hddIntId.Value = gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item("intId").ToString
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub imgBtnIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'System.Web.UI.ImageClickEventArgs)
        hddStatusIntegrante.Value = sender.CommandArgument
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        Dim lista As New ArrayList
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        If sender.CommandArgument = 6 Then
            Try
                hddIntId.Value = sender.Attributes("intId")
                hddCartao.Value = "GRUPO"
                Dim objCheckInVO = New Turismo.CheckInOutVO
                objCheckInVO = objCheckInOutDAO.finalizaHospede(sender.Attributes("intId"), "INTEGRANTE", User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                'hddFinalizaPor.Value = objCheckOutVO.finalizaPor
                If objCheckInVO.msg > "" And objCheckInVO.finalizaPor = "F" Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('" + objCheckInVO.msg + "')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='G';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinalizaPor.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='';}", True)
                ElseIf objCheckInVO.msg > "" Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objCheckInVO.msg + "');", True)
                End If
                btnConsulta_Click(sender, e)
                Try
                    For Each linha As GridViewRow In gdvReserva1.Rows
                        If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                            gdvReserva1.SelectedIndex = linha.RowIndex
                            Exit For
                        End If
                    Next
                    gdvReserva1.SelectedRow.Cells(1).Focus()
                Catch ex As Exception

                End Try
            Catch ex As Exception
            End Try
        ElseIf sender.CommandArgument = 5 Or sender.CommandArgument = 4 Then
            Try
                hddResId.Value = sender.Attributes("resId")
                hddIntId.Value = sender.Attributes("intId")
                hddSolId.Value = sender.Attributes("solId")
                hddHosId.Value = sender.Attributes("hosId")
                Server.Transfer("~/Reserva.aspx", True)
            Catch ex As Exception
            End Try
        ElseIf sender.CommandArgument = 3 Then
            gdvReserva1.Columns(5).Visible = False

            hddResId.Value = sender.Attributes("resId")
            hddIntId.Value = sender.Attributes("intId")
            lista = objCheckInOutDAO.consultar(0, "", "", sender.Attributes("resId"), sender.Attributes("intId"), ckbEntrada.Checked, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
            gdvReserva1.Columns(0).Visible = False
            gdvReserva1.DataSource = lista
            gdvReserva1.DataBind()
            gdvReserva1.Enabled = Session("GrupoRecepcaoPiri") Or Session("GrupoRecepcao")
            gdvReserva1.SelectedIndex = 0
            txtConsulta.Focus()
            lblConsulta.Text = "Aproxime o cartão"
            btnConsulta.Text = "  Voltar"
            pnlTroca.Visible = False
            If txtConsulta.Text > "" Then
                hddtxtConsulta.Value = txtConsulta.Text
                txtConsulta.Text = ""
            End If
            cmbBloco.Enabled = False
            imgCalendario.Visible = False
            imgCalendario_CalendarExtender.Enabled = False
            ckbEntrada.Visible = False
            ckbEstada.Visible = False
            ckbSaida.Visible = False
            ckbTransferencia.Visible = False
            imgBtnReservaNova.Visible = False
        ElseIf sender.CommandArgument = 2 Then
            Dim Retorno As SByte
            Retorno = objCheckInOutDAO.entrada("intId", sender.Attributes("intId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            hddIntId.Value = sender.Attributes("intId")
            btnConsulta_Click(sender, e)
            If Retorno = 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
                Try
                    For Each linha As GridViewRow In gdvReserva1.Rows
                        If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                            gdvReserva1.SelectedIndex = linha.RowIndex
                            Exit For
                        End If
                    Next
                    gdvReserva1.SelectedRow.Cells(1).Focus()
                Catch ex As Exception

                End Try

            End If
        ElseIf sender.CommandArgument = 1 Then
            hddIntId.Value = sender.Attributes("intId")
            hddCartao.Value = sender.Attributes("intCartao")
            hddIntDataFim.Value = sender.Attributes("intDataFim")
            hddAcmId.Value = sender.Attributes("acmId")
            hddResId.Value = sender.Attributes("resId")
            hddSolId.Value = sender.Attributes("solId")
            hddFinalizaPor.Value = "I"
            btnFinaliza_Click(sender, e)

            'lblIntegrante.Text = sender.Attributes("intNome")
            'lblConsumo.Text = "Consumo de Serviços e Empréstimos para " & sender.Attributes("intNome")
            'lblPermuta.Text = "Permutar " & sender.Attributes("intNome") & " da acomodação " & sender.Attributes("apaDesc") & " e todos os acompanhantes com:"
            'lblTransferencia.Text = "Transferir " & sender.Attributes("intNome") & " da acomodação " & sender.Attributes("apaDesc") & " e todos os acompanhantes para:"
            'btnBloquearCartao.Enabled = (sender.Attributes("cartao") = "Sim")

            'btnBloquearCartao.Attributes.Add("intId", hddIntId.Value)
            'btnBloquearCartao.Attributes.Add("intCartao", hddCartao.Value)

            'btnPermutar.Enabled = (sender.Attributes("intStatus") = "E")
            'btnTransferir.Enabled = (sender.Attributes("intStatus") = "E") Or (sender.Attributes("intStatus") = "T")
            'pnlAcao.DataBind() 'Nome do panel onte está contida a tela do modalPopupExtender
            'btnModalPopUpAcao_ModalPopupExtender.Show() 'Nome do modalPopupExtender
            'pnlAcao.Focus()  'Nome do panel onte está contida a tela do modalPopupExtender
        ElseIf sender.CommandArgument = 7 Then
            Try
                Dim Retorno As SByte
                Retorno = objCheckInOutDAO.entrarPasseioPqAquatico(sender.Attributes("resId"), Format(Date.Now, "yyyy-MM-dd HH:mm:ss"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                If Retorno = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
                ElseIf Retorno = -1 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. O passeio não está confirmado para entrar no parque aquático." + "');", True)
                Else
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Entrada do passeio realizado com sucesso." + "');", True)
                End If
                btnConsulta_Click(sender, e)
            Catch ex As Exception
                'Informar erro ao usuário e abortar processo saindo da tela de check-in
            End Try

        End If
    End Sub

    Protected Sub imgBtnAcomodacao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        hddSolId.Value = sender.Attributes("solId")
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Dim Retorno As SByte
        Retorno = objCheckInOutDAO.entrada("solId", sender.Attributes("solId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        btnConsulta_Click(sender, e)
        If Retorno = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
            Try
                For Each linha As GridViewRow In gdvReserva1.Rows
                    If (gdvReserva1.DataKeys(linha.RowIndex).Item(2).ToString = hddSolId.Value) Then
                        gdvReserva1.SelectedIndex = linha.RowIndex
                        Exit For
                    End If
                Next
                gdvReserva1.SelectedRow.Cells(1).Focus()
            Catch ex As Exception

            End Try

        End If
    End Sub

    Protected Sub imgBtnReserva_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        hddResId.Value = sender.Attributes("resId")
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Dim Retorno As SByte
        Retorno = objCheckInOutDAO.entrada("resId", sender.Attributes("resId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        btnConsulta_Click(sender, e)
        If Retorno = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
            Try
                For Each linha As GridViewRow In gdvReserva1.Rows
                    If (gdvReserva1.DataKeys(linha.RowIndex).Item(0).ToString = hddResId.Value) Then
                        gdvReserva1.SelectedIndex = linha.RowIndex
                        Exit For
                    End If
                Next
                gdvReserva1.SelectedRow.Cells(1).Focus()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub imgBtnCortesia_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        hddResId.Value = sender.Attributes("resId")
        hddIntId.Value = sender.Attributes("intId")
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objCheckInOutDAO.consultar(hddResId.Value, hddIntId.Value)
        gdvReserva2.DataSource = lista
        gdvReserva2.DataBind()
        gdvReserva2.SelectedIndex = 0
        txtPlaca.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intPlacaVeiculo"))
        txtModeloVeiculo.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("IntModeloVeiculo"))
        txtMemorando.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intMemorando"))
        rdbCortesiaCaucao.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaCaucao")
        rdbCortesiaConsumo.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaConsumo")
        rdbCortesiaRestaurante.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaRestaurante")
        pnlConsulta.Visible = False
        pnlCortesia.Visible = True
        txtPlaca.Focus()
        If lista.Count = 1 Then
            btnCortesiaColetivo.Visible = False
            btnCortesiaIndividual.Text = "Atribuir"
        End If
    End Sub

    Protected Sub gdvReserva2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva2.SelectedIndexChanged
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        hddResId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(0)
        hddIntId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(1)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objCheckInOutDAO.consultar(hddResId.Value, hddIntId.Value)
        gdvReserva2.DataSource = lista
        gdvReserva2.DataBind()
        gdvReserva2.SelectedIndex = 0
        txtPlaca.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intPlacaVeiculo"))
        txtModeloVeiculo.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("IntModeloVeiculo"))
        txtMemorando.Text = Trim(gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intMemorando"))
        rdbCortesiaCaucao.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaCaucao")
        rdbCortesiaConsumo.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaConsumo")
        rdbCortesiaRestaurante.Text = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item("intCortesiaRestaurante")
        txtPlaca.Focus()
    End Sub

    Protected Sub btnCortesiaVolta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCortesiaVolta.Click
        pnlCortesia.Visible = False
        txtPlaca.Text = ""
        pnlConsulta.Visible = True
        txtConsulta.Focus()
    End Sub

    Protected Sub btnCortesiaIndividual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCortesiaIndividual.Click, btnCortesiaColetivo.Click
        If txtPlaca.Text.Trim.Length > 0 And txtModeloVeiculo.Text.Trim.Length = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Insira o modelo do veículo." + "');", True)
            txtModeloVeiculo.Focus()
            Return
        End If

        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Dim Retorno As SByte
        If sender Is btnCortesiaIndividual Then
            Retorno = objCheckInOutDAO.cortesia(hddIntId.Value, 0, txtMemorando.Text.Trim, rdbCortesiaCaucao.Text, rdbCortesiaConsumo.Text, _
                                                 rdbCortesiaRestaurante.Text, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), txtPlaca.Text.Trim, txtModeloVeiculo.Text.Trim)
        Else
            Retorno = objCheckInOutDAO.cortesia(hddIntId.Value, hddResId.Value, txtMemorando.Text.Trim, rdbCortesiaCaucao.Text, rdbCortesiaConsumo.Text, _
                                     rdbCortesiaRestaurante.Text, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), txtPlaca.Text.Trim, txtModeloVeiculo.Text.Trim)
        End If
        If Retorno = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Informações gravadas com sucesso." + "');", True)
        End If
        gdvReserva2_SelectedIndexChanged(sender, e)
        txtPlaca.Focus()
    End Sub

    Protected Sub btnFinalizaPor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinalizaPor.Click
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        'Dim lista As New ArrayList
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        Try
            Dim objCheckInVO = New Turismo.CheckInOutVO
            objCheckInVO = objCheckInOutDAO.finalizaHospede(hddIntId.Value, hddCartao.Value, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            If objCheckInVO.msg > "" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objCheckInVO.msg + "');", True)
            End If
            btnConsulta_Click(sender, e)
            Try
                For Each linha As GridViewRow In gdvReserva1.Rows
                    If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                        gdvReserva1.SelectedIndex = linha.RowIndex
                        Exit For
                    End If
                Next
                gdvReserva1.SelectedRow.Cells(1).Focus()
            Catch ex As Exception

            End Try
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnFinaliza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinaliza.Click
        Try
            Dim objCheckInOutDAO As Turismo.CheckInOutDAO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
            Else
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
            End If
            Dim objCheckInVO = New Turismo.CheckInOutVO
            If hddFinalizaPor.Value = "S" Then
                objCheckInVO = objCheckInOutDAO.finalizaHospede(0, hddCartao.Value, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                hddFinalizaPor.Value = objCheckInVO.finalizaPor
                hddMsg.Value = objCheckInVO.msg
            ElseIf hddFinalizaPor.Value = "C" Then
                objCheckInVO = objCheckInOutDAO.finalizaHospede(hddIntId.Value, "CARTAO", User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                hddFinalizaPor.Value = objCheckInVO.finalizaPor
                hddMsg.Value = objCheckInVO.msg
            ElseIf hddFinalizaPor.Value = "F" Then
                objCheckInVO = objCheckInOutDAO.finalizaHospede(hddIntId.Value, "FINALIZA", User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                hddFinalizaPor.Value = objCheckInVO.finalizaPor
                hddMsg.Value = objCheckInVO.msg
            ElseIf hddFinalizaPor.Value = "G" Then
                objCheckInVO = objCheckInOutDAO.finalizaHospede(hddIntId.Value, "GRUPO", User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                hddFinalizaPor.Value = objCheckInVO.finalizaPor
                hddMsg.Value = objCheckInVO.msg
            ElseIf hddFinalizaPor.Value = "I" Then
                objCheckInVO = objCheckInOutDAO.finalizaHospede(hddIntId.Value, "INTEGRANTE", User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
                hddFinalizaPor.Value = objCheckInVO.finalizaPor
                hddMsg.Value = objCheckInVO.msg
            End If
            If objCheckInVO.msg > "" And objCheckInVO.finalizaPor = "C" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('" + objCheckInVO.msg + "')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
            ElseIf objCheckInVO.msg > "" And objCheckInVO.finalizaPor = "F" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('" + objCheckInVO.msg + "')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
            ElseIf objCheckInVO.msg > "" And objCheckInVO.finalizaPor = "G" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('" + objCheckInVO.msg + "')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
            ElseIf objCheckInVO.msg > "" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objCheckInVO.msg + "');document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();", True)
                'If objCheckInVO.msg = "Integrante finalizado!" Or objCheckInVO.msg = "Integrante finalizado" Then
                If (objCheckInVO.finalizaPor = "1") Then
                ElseIf objCheckInVO.finalizaPor = "0" Then
                    gdvReserva1.DataSource = Nothing
                    gdvReserva1.DataBind()
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    'Protected Sub btnFinalizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinalizar.Click
    '    hddFinalizaPor.Value = "I"
    '    hddIntId.Value = sender.Attributes("intId")
    '    btnFinaliza_Click(sender, e)
    'End Sub

    Protected Sub btnConsumoVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsumoVoltar.Click
        pnlConsumo.Visible = False
        pnlConsulta.Visible = True
        Try
            For Each linha As GridViewRow In gdvReserva1.Rows
                If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                    gdvReserva1.SelectedIndex = linha.RowIndex
                    Exit For
                End If
            Next
            gdvReserva1.SelectedRow.Cells(0).Focus()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gdvReserva3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvReserva3.DataKeys(e.Row.RowIndex).Item("cseOrigem") = "EG" Then
                CType(e.Row.FindControl("lnkItem"), LinkButton).Text = "Emprestado"
            Else
                CType(e.Row.FindControl("lnkItem"), LinkButton).Text = "Consumido"
            End If
        End If
    End Sub

    Protected Sub gdvReserva3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva3.SelectedIndexChanged
        hddCSeId.Value = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseId")
        txtData.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseData")
        cmbTipo.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("tmoId")
        rdbEmprestimo.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseOrigem")
        txtItem.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseDescricao")
        txtValor.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseValor")
        txtQtde.Text = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("cseQuantidade")
        If gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item("situacaoPgto") = "A Pagar" Then
            btnConsumoExcluir.Enabled = True
        Else
            btnConsumoSalvar.Enabled = False
            btnConsumoExcluir.Enabled = False
        End If
    End Sub

    Protected Sub btnConsumoIncluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsumoIncluir.Click
        btnConsumoSalvar.Enabled = True
        btnConsumoExcluir.Enabled = False
        hddCSeId.Value = "0"
        cmbTipo.SelectedIndex = 0
        txtData.Text = Date.Today
        rdbEmprestimo.Text = "EG"
        txtItem.Text = ""
        txtValor.Text = "1,00"
        txtQtde.Text = "1,00"
        cmbTipo.Focus()
    End Sub

    Protected Sub btnConsumoSalvar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsumoSalvar.Click
        Dim objConsumoServicoDAO As Turismo.ConsumoServicoDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialCaldas")
        Else
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialPiri")
        End If
        Dim objConsumoServicoVO = New Turismo.ConsumoServicoVO
        objConsumoServicoVO.cseId = hddCSeId.Value
        objConsumoServicoVO.intId = hddIntId.Value
        objConsumoServicoVO.tmoId = cmbTipo.SelectedValue
        objConsumoServicoVO.cseData = txtData.Text
        objConsumoServicoVO.cseOrigem = rdbEmprestimo.Text
        objConsumoServicoVO.cseDescricao = txtItem.Text
        objConsumoServicoVO.cseManual = "S"
        objConsumoServicoVO.cseValor = txtValor.Text
        objConsumoServicoVO.cseQuantidade = txtQtde.Text
        objConsumoServicoVO.cseUsuario = User.Identity.Name.Replace("SESC-GO.COM.BR\", "")
        objConsumoServicoVO.cseUsuarioData = Format(Date.Now, "dd/MM/yyyy")
        objConsumoServicoDAO.Acao(objConsumoServicoVO)
        btnConsumoSalvar.Attributes.Add("intId", hddIntId.Value)
        'btnEmprestimo_Click(sender, e)
        imgBtnEmprestimo_Click(sender, e)
        btnConsumoIncluir_Click(sender, e)
    End Sub

    Protected Sub btnConsumoExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsumoExcluir.Click
        Dim objConsumoServicoDAO As Turismo.ConsumoServicoDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialCaldas")
        Else
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialPiri")
        End If
        Dim objConsumoServicoVO = New Turismo.ConsumoServicoVO
        objConsumoServicoVO.cseId = -1 * hddCSeId.Value
        objConsumoServicoVO.intId = hddIntId.Value
        objConsumoServicoVO.tmoId = cmbTipo.SelectedValue
        objConsumoServicoVO.cseData = txtData.Text
        objConsumoServicoVO.cseOrigem = rdbEmprestimo.Text
        objConsumoServicoVO.cseDescricao = txtItem.Text
        objConsumoServicoVO.cseManual = "S"
        objConsumoServicoVO.cseValor = txtValor.Text
        objConsumoServicoVO.cseQuantidade = txtQtde.Text
        objConsumoServicoVO.cseUsuario = User.Identity.Name.Replace("SESC-GO.COM.BR\", "")
        objConsumoServicoVO.cseUsuarioData = Format(Date.Now, "dd/MM/yyyy")
        objConsumoServicoDAO.Acao(objConsumoServicoVO)
        btnConsumoExcluir.Attributes.Add("intId", hddIntId.Value)
        'btnEmprestimo_Click(sender, e)
        imgBtnEmprestimo_Click(sender, e)
        btnConsumoIncluir_Click(sender, e)
    End Sub

    Protected Sub btnCaixa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaixa.Click
        Dim objConsumoServicoDAO As Turismo.ConsumoServicoDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialCaldas")
        Else
            objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialPiri")
        End If
        objConsumoServicoDAO.Caixa(hddIntId.Value)
    End Sub

    Protected Sub btnPermutar_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objPermutaAptoDAO As Turismo.PermutaAptoDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPermutaAptoDAO = New Turismo.PermutaAptoDAO("TurismoSocialCaldas")
        Else
            objPermutaAptoDAO = New Turismo.PermutaAptoDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objPermutaAptoDAO.consultar(hddIntId.Value)
        If lista.Count > 0 Then
            gdvReserva4.DataSource = lista
            gdvReserva4.DataBind()
            gdvReserva4.SelectedIndex = -1
            pnlPermuta.Visible = True
            pnlConsulta.Visible = False
        Else
            gdvReserva4.DataSource = lista
            gdvReserva4.DataBind()
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não existem apartamentos para permutar." + "');", True)
        End If
    End Sub

    Protected Sub btnPermutaVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPermutaVoltar.Click
        pnlPermuta.Visible = False
        pnlConsulta.Visible = True
    End Sub

    Protected Sub gdvReserva4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva4.SelectedIndexChanged
        Try
            Dim objPermutaAptoDAO As Turismo.PermutaAptoDAO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objPermutaAptoDAO = New Turismo.PermutaAptoDAO("TurismoSocialCaldas")
            Else
                objPermutaAptoDAO = New Turismo.PermutaAptoDAO("TurismoSocialPiri")
            End If
            objPermutaAptoDAO.permutar(gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item("solic"), _
                                       gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item("apto"), _
                                       gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item("solId"), _
                                       gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item("apaId"), _
                                       User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            btnPermutaVoltar_Click(sender, e)
            btnConsulta_Click(sender, e)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnTransferirVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransferirVoltar.Click
        pnlTransferencia.Visible = False
        btnConsulta_Click(sender, e)
        pnlConsulta.Visible = True
    End Sub

    Protected Sub gdvReserva5_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva5.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvReserva5.DataKeys(e.Row.RowIndex).Item("apaStatus") = "L" Then
                CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoBranco.gif"
            ElseIf gdvReserva5.DataKeys(e.Row.RowIndex).Item("apaStatus") = "O" Then
                'CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoAzul.gif"
                e.Row.Visible = False
            ElseIf gdvReserva5.DataKeys(e.Row.RowIndex).Item("apaStatus") = "A" Then
                'CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoAmarelo.gif"
                e.Row.Visible = False
            Else
                'CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoCinza.gif"
                e.Row.Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvReserva5_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva5.SelectedIndexChanged
        Dim objTransferenciaDAO As Turismo.TransferenciaDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
        Else
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
        End If
        objTransferenciaDAO.insereSolicitacao(hddResId.Value, hddSolId.Value, _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("acmId"), _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("codId"), _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("dtInicial"), _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("dtFinal"), _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("hrInicial"), _
                                              gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item("hrFinal"), _
                                              User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        imgBtnTransferir_Click(sender, e)
    End Sub

    Protected Sub gdvReserva6_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva6.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvReserva6.DataKeys(e.Row.RowIndex).Item("apaStatus") = "L" Then
                CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoBranco.gif"
            ElseIf gdvReserva6.DataKeys(e.Row.RowIndex).Item("apaStatus") = "O" Then
                CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoAzul.gif"
            ElseIf gdvReserva6.DataKeys(e.Row.RowIndex).Item("apaStatus") = "A" Then
                CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoAmarelo.gif"
            Else
                CType(e.Row.FindControl("imgAptoStatus"), ImageButton).ImageUrl = "~/images/StatusAptoCinza.gif"
            End If
        End If
    End Sub

    Protected Sub btnTransferirCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransferirCancelar.Click
        Dim objTransferenciaDAO As Turismo.TransferenciaDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
        Else
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
        End If
        objTransferenciaDAO.cancelaTransferencia(hddSolId.Value, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        imgBtnTransferir_Click(sender, e)
    End Sub

    Protected Sub btnTransferirComOnus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransferirComOnus.Click, btnTransferirSemOnus.Click
        Dim objTransferenciaDAO As Turismo.TransferenciaDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
        Else
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objTransferenciaDAO.confirmaTransferencia(hddSolId.Value, sender.CommandArgument, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
        Dim objTransferenciaVO = lista.Item(0)
        If objTransferenciaVO.nome.ToString > "" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objTransferenciaVO.nome.ToString + "');", True)
        Else
            btnTransferirVoltar_Click(sender, e)
        End If
    End Sub

    Protected Sub imgBtn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddFinalizaPor.Value = "I"
        hddIntId.Value = sender.Attributes("intId")
        btnFinaliza_Click(sender, e)
    End Sub

    Protected Sub btnDesocupar_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objAptoCheckInOutDAO = New Turismo.AptoCheckInOutDAO(bd)
        Dim Retorno As SByte
        hddIntId.Value = sender.Attributes("intId")
        If sender.Attributes("origem") = "~/images/ChaveNaoEntregue.gif" Then
            Retorno = objAptoCheckInOutDAO.EntregarChaveApto(sender.Attributes("intId"), sender.Attributes("resId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            btnConsulta_Click(sender, e)
            If Retorno > 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Chave entregue com sucesso." + "');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
                Try
                    For Each linha As GridViewRow In gdvReserva1.Rows
                        If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                            gdvReserva1.SelectedIndex = linha.RowIndex
                            Exit For
                        End If
                    Next
                    gdvReserva1.SelectedRow.Cells(3).Focus()
                Catch ex As Exception

                End Try
            End If
        ElseIf sender.Attributes("origem") = "~/images/ChaveEntregue.gif" Then
            Retorno = objAptoCheckInOutDAO.DevolveChaveApto(sender.Attributes("intId"), sender.Attributes("apaId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            btnConsulta_Click(sender, e)
            If Retorno > 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Chave devolvida com sucesso." + "');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
                Try
                    For Each linha As GridViewRow In gdvReserva1.Rows
                        If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                            gdvReserva1.SelectedIndex = linha.RowIndex
                            Exit For
                        End If
                    Next
                    gdvReserva1.SelectedRow.Cells(3).Focus()
                Catch ex As Exception

                End Try
            End If
        ElseIf sender.Attributes("origem") = "~/images/ok.gif" Then
            Retorno = objAptoCheckInOutDAO.ReentregaChaveApto(sender.Attributes("intId"), sender.Attributes("apaId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            btnConsulta_Click(sender, e)
            If Retorno > 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Chave reentregue com sucesso." + "');", True)
            ElseIf Retorno = -1 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível reentregar a chave. Período não permite." + "');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
            End If
            Try
                For Each linha As GridViewRow In gdvReserva1.Rows
                    If (gdvReserva1.DataKeys(linha.RowIndex).Item(1).ToString = hddIntId.Value) Then
                        gdvReserva1.SelectedIndex = linha.RowIndex
                        Exit For
                    End If
                Next
                gdvReserva1.SelectedRow.Cells(3).Focus()
            Catch ex As Exception

            End Try
        ElseIf sender.Attributes("origem") = "~/images/transferencia.png" Then
            Dim objTransferenciaDAO = New Turismo.TransferenciaDAO(bd)
            Dim lista As New ArrayList
            lista = objTransferenciaDAO.confirmaTransferenciaIndividual(sender.Attributes("intId"), sender.Attributes("solId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            Dim objTransferenciaVO = lista.Item(0)
            If objTransferenciaVO.nome.ToString > "" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objTransferenciaVO.nome.ToString + "');", True)
            Else
                btnConsulta_Click(sender, e)
            End If
        End If
    End Sub

    Protected Sub btnLiberar_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hddSolId.Value = sender.Attributes("solId")
        Dim lista As New ArrayList
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objAptoCheckInOutDAO = New Turismo.AptoCheckInOutDAO(bd)
        If sender.AccessKey = "N" Then
            lista = objAptoCheckInOutDAO.consultar(hddSolId.Value)
            If lista.Count > 0 Then
                Dim listaLimpo As New ArrayList
                Dim listaArrumacao As New ArrayList
                Dim listaOcupado As New ArrayList
                Dim listaManutencao As New ArrayList
                For Each item As Turismo.AptoCheckInOutVO In lista
                    If item.apaStatus = "L" Then
                        listaLimpo.Add(item)
                    ElseIf item.apaStatus = "A" Then
                        listaArrumacao.Add(item)
                    ElseIf item.apaStatus = "O" Then
                        listaOcupado.Add(item)
                    ElseIf item.apaStatus = "M" Then
                        listaManutencao.Add(item)
                    End If
                Next
                rblLimpo.DataSource = listaLimpo
                rblLimpo.DataValueField = "apaId"
                rblLimpo.DataTextField = "apaDesc"
                pnlAptoLimpo.Visible = listaLimpo.Count > 0

                rblArrumacao.DataSource = listaArrumacao
                rblArrumacao.DataValueField = "apaId"
                rblArrumacao.DataTextField = "apaDesc"
                pnlAptoArrumacao.Visible = listaArrumacao.Count > 0

                rblOcupado.DataSource = listaOcupado
                rblOcupado.DataValueField = "apaId"
                rblOcupado.DataTextField = "apaDesc"
                pnlAptoOcupado.Visible = listaOcupado.Count > 0

                rblManutencao.DataSource = listaManutencao
                rblManutencao.DataValueField = "apaId"
                rblManutencao.DataTextField = "apaDesc"
                pnlAptoManutencao.Visible = listaManutencao.Count > 0

                pnlListaApto.DataBind() 'Nome do panel onte está contida a tela do modalPopupExtender
                btnModalPopUpApto_ModalPopupExtender.Show() 'Nome do modalPopupExtender
                pnlListaApto.Focus()  'Nome do panel onte está contida a tela do modalPopupExtender
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não existe acomodação disponível." + "');", True)
            End If
        ElseIf (sender.AccessKey = "S") Or (sender.AccessKey = "T") Then
            Dim Retorno As SByte
            Retorno = objAptoCheckInOutDAO.LiberarApto(hddSolId.Value, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            If Retorno = -1 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento não foi desocupado. Já foram entregues as chaves." + "');", True)
            ElseIf Retorno = 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento foi desocupado." + "');", True)
            End If
            btnConsulta_Click(sender, e)
        ElseIf sender.AccessKey = "O" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Apartamento já está ocupado." + "');", True)
        End If
        Try
            For Each linha As GridViewRow In gdvReserva1.Rows
                If (gdvReserva1.DataKeys(linha.RowIndex).Item(2).ToString = sender.Attributes("solId")) Then
                    gdvReserva1.SelectedIndex = linha.RowIndex
                    Exit For
                End If
            Next
            gdvReserva1.SelectedRow.Cells(2).Focus()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAtribuirCartao_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        Dim lista As New ArrayList
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        hddResId.Value = sender.Attributes("resId")
        hddIntId.Value = sender.Attributes("intId")
        lista = objCheckInOutDAO.consultar(0, "", "", sender.Attributes("resId"), sender.Attributes("intId"), True, ckbEstada.Checked, ckbSaida.Checked, ckbTransferencia.Checked, btnConsulta.CommandName)
        gdvReserva1.Columns(0).Visible = False
        gdvReserva1.DataSource = lista
        gdvReserva1.DataBind()
        gdvReserva1.Enabled = Session("GrupoRecepcaoPiri") Or Session("GrupoRecepcao")
        gdvReserva1.SelectedIndex = 0
        txtConsulta.Focus()
        lblConsulta.Text = "Aproxime o cartão"
        btnConsulta.Text = "  Voltar"
        pnlTroca.Visible = False
        If txtConsulta.Text > "" Then
            hddtxtConsulta.Value = txtConsulta.Text
            txtConsulta.Text = ""
        End If
        cmbBloco.Enabled = False
        imgCalendario.Visible = False
        imgCalendario_CalendarExtender.Enabled = False
        ckbEntrada.Visible = False
        ckbEstada.Visible = False
        ckbSaida.Visible = False
        ckbTransferencia.Visible = False
        imgBtnReservaNova.Visible = False
    End Sub

    Protected Sub ckbEntrada_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckbEntrada.PreRender, ckbEstada.PreRender, ckbSaida.PreRender, ckbTransferencia.PreRender
        If sender.Checked Then
            sender.CssClass = "formLabelWebChecked"
        Else
            sender.CssClass = "formLabelWeb"
        End If
    End Sub

    Protected Sub lnkhdrIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdrResponsavel" Or sender.ID = "imgResponsavel" Then
            If btnConsulta.CommandName = "resNome, intNome desc, hosStatus" Then
                btnConsulta.CommandName = "resNome desc, intNome desc, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnConsulta.CommandName = "resNome, intNome desc, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante" Then
            If btnConsulta.CommandName = "intNome, convert(datetime, intDataIni, 120), hosStatus" Then
                btnConsulta.CommandName = "intNome desc, convert(datetime, intDataIni, 120) desc, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnConsulta.CommandName = "intNome, convert(datetime, intDataIni, 120), hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor" Then
            If btnConsulta.CommandName = "intUsuario, intNome, hosStatus" Then
                btnConsulta.CommandName = "intUsuario desc, intNome, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnConsulta.CommandName = "intUsuario, intNome, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrPeriodo" Or sender.ID = "imgPeriodo" Then
            If btnConsulta.CommandName = "convert(datetime, intDataIni, 120), convert(datetime, intDataFim, 120), resHoraSaida, intNome, hosStatus" Then
                btnConsulta.CommandName = "convert(datetime, intDataIni, 120) desc, convert(datetime, intDataFim, 120) desc, resHoraSaida desc, intNome, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnConsulta.CommandName = "convert(datetime, intDataIni, 120), convert(datetime, intDataFim, 120), resHoraSaida, intNome, hosStatus"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrAcao" Or sender.ID = "imgAcao" Then
            If btnConsulta.CommandName = "cast(substring(ApaDesc,1,charindex(' ', ApaDesc)) as integer)" Then
                btnConsulta.CommandName = "cast(substring(ApaDesc,1,charindex(' ', ApaDesc)) as integer) desc"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgAcao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnConsulta.CommandName = "cast(substring(ApaDesc,1,charindex(' ', ApaDesc)) as integer)"
                btnConsulta_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgAcao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If
        CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).Visible = (sender.ID = "lnkhdrResponsavel" Or sender.ID = "imgResponsavel")
        CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante")
        CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).Visible = (sender.ID = "lnkhdrPeriodo" Or sender.ID = "imgPeriodo")
        CType(gdvReserva1.HeaderRow.FindControl("imgAcao"), ImageButton).Visible = (sender.ID = "lnkhdrAcao" Or sender.ID = "imgAcao")
        CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).Visible = (sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor")
    End Sub

    Protected Sub imgIntegrante_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdrIntegrante_Click(sender, e)
    End Sub

    Protected Sub imgBtnEmprestimo_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim objConsumoServicoDAO As Turismo.ConsumoServicoDAO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialCaldas")
            Else
                objConsumoServicoDAO = New Turismo.ConsumoServicoDAO("TurismoSocialPiri")
            End If
            Dim lista As New ArrayList
            hddIntId.Value = sender.Attributes("intId")
            lista = objConsumoServicoDAO.consultarPorIntegrante(sender.Attributes("intId"))
            gdvReserva3.DataSource = lista
            gdvReserva3.DataBind()
            gdvReserva3.SelectedIndex = -1
            pnlConsumo.Visible = True
            pnlConsulta.Visible = False
            If gdvReserva3.Rows.Count > 0 Then
                gdvReserva3_SelectedIndexChanged(sender, e)
            Else
                btnConsumoIncluir_Click(sender, e)
            End If
            cmbTipo.Focus()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub imgBtnBloquearCartao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim objCartaoConsumoDAO As Turismo.CartaoConsumoDAO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objCartaoConsumoDAO = New Turismo.CartaoConsumoDAO("TurismoSocialCaldas")
            Else
                objCartaoConsumoDAO = New Turismo.CartaoConsumoDAO("TurismoSocialPiri")
            End If
            Dim Retorno As SByte
            Retorno = objCartaoConsumoDAO.bloquear(sender.Attributes("intCartao"), sender.Attributes("intId"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))
            If Retorno = 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Por favor, repita esta operação novamente. Caso o problema persista, entre em contato com o suporte técnico." + "');", True)
            ElseIf Retorno = -1 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão livre." + "');", True)
            ElseIf Retorno = -2 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão não pertence mais ao hóspede." + "');", True)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Cartão foi bloqueado com sucesso." + "');", True)
            End If
            btnConsulta_Click(sender, e)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub imgBtnPermutar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddIntId.Value = sender.Attributes("intId")
        btnPermutar_Click(sender, e)
    End Sub

    Protected Sub imgBtnTransferir_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender Is btnTransferirCancelar Or sender Is gdvReserva5 Then
        Else
            hddResId.Value = sender.Attributes("resId")
            hddSolId.Value = sender.Attributes("solId")
            hddAcmId.Value = sender.Attributes("acmId")
            hddIntDataFim.Value = sender.Attributes("intDataFim")
        End If

        Dim objTransferenciaDAO As Turismo.TransferenciaDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
        Else
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objTransferenciaDAO.consultarTransferencia(hddSolId.Value)
        If lista.Count > 0 Then
            gdvReserva6.DataSource = lista
            gdvReserva6.DataBind()
            gdvReserva6.SelectedIndex = -1
            gdvReserva5.Visible = False
            gdvReserva6.Visible = True
            btnTransferirCancelar.Visible = True
            btnTransferirComOnus.Visible = True
            btnTransferirSemOnus.Visible = True
        Else
            lista = objTransferenciaDAO.consultar(Format(Date.Now, "dd/MM/yyyy"), "12", hddIntDataFim.Value, "12", hddAcmId.Value)
            If lista.Count > 0 Then
                gdvReserva5.DataSource = lista
                gdvReserva5.DataBind()
                gdvReserva5.SelectedIndex = -1
                gdvReserva5.Visible = True
                gdvReserva6.Visible = False
                btnTransferirCancelar.Visible = False
                btnTransferirComOnus.Visible = False
                btnTransferirSemOnus.Visible = False
            End If
        End If
        pnlTransferencia.Visible = True
        pnlConsulta.Visible = False
    End Sub

    Public Sub LocalizaCtrl(ByVal controlP As Control, ByVal controlId As String, ByVal statusCor As Drawing.Color)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is LinkButton Then
                If DirectCast(ctl, LinkButton).CommandName = controlId Then
                    DirectCast(ctl, LinkButton).BackColor = statusCor
                End If
            End If
        Next
    End Sub

    Protected Sub btnAtualizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAtualizar.Click
        Dim objApartamentoDAO As Turismo.ApartamentoDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialCaldas")
        Else
            objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        If cmbBloco.Text = "0" Then
            lista = objApartamentoDAO.consultar("", "")
        Else
            lista = objApartamentoDAO.consultar(cmbBloco.Text, "")
        End If
        pnlTroca.Visible = (cmbBloco.Text <> "E")

        '        pnlTrocaAnhanguera.Visible = (pnlTroca.Visible And (cmbBloco.Text = "1"))
        If pnlTroca.Visible Then
            pnlTrocaCaldas.Visible = False
            pnlTrocaAnhanguera.Visible = False
            pnlTrocaBambui.Visible = False
            pnlTrocaKilzer.Visible = False
            pnlTrocaWilton.Visible = False
            pnlTrocaPiri.Visible = False
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                pnlTrocaCaldas.Visible = True
                For Each item As Turismo.ApartamentoVO In lista
                    If item.apaStatus = "O" And InStr(item.apaDesc, "RT") > 0 Then
                        LocalizaCtrl(pnlTrocaAnhanguera, item.apaId, Drawing.Color.PaleTurquoise)
                        pnlTrocaAnhanguera.Visible = True
                    ElseIf item.apaStatus = "L" And InStr(item.apaDesc, "RT") > 0 Then
                        LocalizaCtrl(pnlTrocaAnhanguera, item.apaId, Drawing.Color.White)
                        pnlTrocaAnhanguera.Visible = True
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "RT") > 0 Then
                        LocalizaCtrl(pnlTrocaAnhanguera, item.apaId, Drawing.Color.Gold)
                        pnlTrocaAnhanguera.Visible = True
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "RT") > 0 Then
                        LocalizaCtrl(pnlTrocaAnhanguera, item.apaId, Drawing.Color.LightGray)
                        pnlTrocaAnhanguera.Visible = True
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "RA") > 0 Then
                        LocalizaCtrl(pnlTrocaBambui, item.apaId, Drawing.Color.PaleTurquoise)
                        pnlTrocaBambui.Visible = True
                    ElseIf item.apaStatus = "L" And InStr(item.apaDesc, "RA") > 0 Then
                        LocalizaCtrl(pnlTrocaBambui, item.apaId, Drawing.Color.White)
                        pnlTrocaBambui.Visible = True
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "RA") > 0 Then
                        LocalizaCtrl(pnlTrocaBambui, item.apaId, Drawing.Color.Gold)
                        pnlTrocaBambui.Visible = True
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "RA") > 0 Then
                        LocalizaCtrl(pnlTrocaBambui, item.apaId, Drawing.Color.LightGray)
                        pnlTrocaBambui.Visible = True
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "RV") > 0 Then
                        LocalizaCtrl(pnlTrocaWilton, item.apaId, Drawing.Color.PaleTurquoise)
                        pnlTrocaWilton.Visible = True
                    ElseIf item.apaStatus = "L" And InStr(item.apaDesc, "RV") > 0 Then
                        LocalizaCtrl(pnlTrocaWilton, item.apaId, Drawing.Color.White)
                        pnlTrocaWilton.Visible = True
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "RV") > 0 Then
                        LocalizaCtrl(pnlTrocaWilton, item.apaId, Drawing.Color.Gold)
                        pnlTrocaWilton.Visible = True
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "RV") > 0 Then
                        LocalizaCtrl(pnlTrocaWilton, item.apaId, Drawing.Color.LightGray)
                        pnlTrocaWilton.Visible = True
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "RP") > 0 Then
                        LocalizaCtrl(pnlTrocaKilzer, item.apaId, Drawing.Color.PaleTurquoise)
                        pnlTrocaKilzer.Visible = True
                    ElseIf item.apaStatus = "L" And InStr(item.apaDesc, "RP") > 0 Then
                        LocalizaCtrl(pnlTrocaKilzer, item.apaId, Drawing.Color.White)
                        pnlTrocaKilzer.Visible = True
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "RP") > 0 Then
                        LocalizaCtrl(pnlTrocaKilzer, item.apaId, Drawing.Color.Gold)
                        pnlTrocaKilzer.Visible = True
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "RP") > 0 Then
                        LocalizaCtrl(pnlTrocaKilzer, item.apaId, Drawing.Color.LightGray)
                        pnlTrocaKilzer.Visible = True
                    End If
                Next
            Else
                pnlTrocaPiri.Visible = True
                For Each item As Turismo.ApartamentoVO In lista
                    If item.apaStatus = "O" And InStr(item.apaDesc, "Bloco A") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoA, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco A") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoA, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco A") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoA, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco B") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoB, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco B") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoB, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco B") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoB, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco C") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoC, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco C") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoC, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco C") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoC, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco D") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoD, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco D") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoD, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco D") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoD, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco E") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoE, item.apaId, Drawing.Color.PaleTurquoise)
                        LocalizaCtrl(pnlTrocaPiriBlocoEE, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco E") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoE, item.apaId, Drawing.Color.Gold)
                        LocalizaCtrl(pnlTrocaPiriBlocoEE, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco E") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoE, item.apaId, Drawing.Color.LightGray)
                        LocalizaCtrl(pnlTrocaPiriBlocoEE, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco F") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoF, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco F") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoF, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco F") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoF, item.apaId, Drawing.Color.LightGray)
                    ElseIf item.apaStatus = "O" And InStr(item.apaDesc, "Bloco G") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoG, item.apaId, Drawing.Color.PaleTurquoise)
                    ElseIf item.apaStatus = "A" And InStr(item.apaDesc, "Bloco G") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoG, item.apaId, Drawing.Color.Gold)
                    ElseIf item.apaStatus = "M" And InStr(item.apaDesc, "Bloco G") > 0 Then
                        LocalizaCtrl(pnlTrocaPiriBlocoG, item.apaId, Drawing.Color.LightGray)
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub LocalizaCtrlFonteDisponibilidade(ByVal controlP As Control, ByVal controlId As String)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is LinkButton Then
                If DirectCast(ctl, LinkButton).CommandName = controlId Or controlId = "Todos" Then
                    DirectCast(ctl, LinkButton).Font.Strikeout = False
                End If
            End If
        Next
    End Sub

    Public Sub LocalizaCtrlFonteSemDisponibilidade(ByVal controlP As Control, ByVal controlId As String)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is LinkButton Then
                If DirectCast(ctl, LinkButton).CommandName = controlId Or controlId = "Todos" Then
                    DirectCast(ctl, LinkButton).Font.Strikeout = True
                End If
            End If
        Next
    End Sub

    Protected Sub hddApto1_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles hddApto1.ValueChanged
        If (hddApto1.Value = hddApto2.Value) Or _
            (Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4) = Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4)) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "O apartamento já está selecionado." + "');", True)
            hddApto1.Value = hddApto.Value
        ElseIf hddApto1.Value <> hddApto.Value Then
            Dim objCheckInOutDAO As Turismo.CheckInOutDAO
            Dim objApartamentoDAO As Turismo.ApartamentoDAO
            Dim objAcomodacaoDAO As Turismo.AcomodacaoDAO
            Dim lista As New ArrayList
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
                objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialCaldas")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialCaldas")
            Else
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
                objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialPiri")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialPiri")
            End If
            lista = objCheckInOutDAO.consultar("0", "AptoId", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4), 0, 0, True, True, False, False, "h.HosDataIniReal desc, h.HosDataFimReal, intNome")
            lblAptoHosp11.Text = ""
            lblAptoHosp11.ForeColor = Drawing.Color.Empty
            lblAptoHosp11.Font.Italic = False
            lblAptoHosp11.Font.Underline = False
            lblAptoHosp11.Visible = False
            lnkAptoHosp11.Text = ""
            lnkAptoHosp11.ForeColor = Drawing.Color.Empty
            lnkAptoHosp11.Font.Italic = False
            lnkAptoHosp11.Font.Underline = False
            lnkAptoHosp11.Visible = False
            hddHospNome11.Value = ""
            hddHosp11.Value = "0"
            hddHospInfo11.Value = ""
            lblAptoHosp12.Text = ""
            lblAptoHosp12.ForeColor = Drawing.Color.Empty
            lblAptoHosp12.Font.Italic = False
            lblAptoHosp12.Font.Underline = False
            lblAptoHosp12.Visible = False
            lnkAptoHosp12.Text = ""
            lnkAptoHosp12.ForeColor = Drawing.Color.Empty
            lnkAptoHosp12.Font.Italic = False
            lnkAptoHosp12.Font.Underline = False
            lnkAptoHosp12.Visible = False
            hddHospNome12.Value = ""
            hddHosp12.Value = "0"
            hddHospInfo12.Value = ""
            lblAptoHosp13.Text = ""
            lblAptoHosp13.ForeColor = Drawing.Color.Empty
            lblAptoHosp13.Font.Italic = False
            lblAptoHosp13.Font.Underline = False
            lblAptoHosp13.Visible = False
            lnkAptoHosp13.Text = ""
            lnkAptoHosp13.ForeColor = Drawing.Color.Empty
            lnkAptoHosp13.Font.Italic = False
            lnkAptoHosp13.Font.Underline = False
            lnkAptoHosp13.Visible = False
            hddHospNome13.Value = ""
            hddHosp13.Value = "0"
            hddHospInfo13.Value = ""
            lblAptoHosp14.Text = ""
            lblAptoHosp14.ForeColor = Drawing.Color.Empty
            lblAptoHosp14.Font.Italic = False
            lblAptoHosp14.Font.Underline = False
            lblAptoHosp14.Visible = False
            lnkAptoHosp14.Text = ""
            lnkAptoHosp14.ForeColor = Drawing.Color.Empty
            lnkAptoHosp14.Font.Italic = False
            lnkAptoHosp14.Font.Underline = False
            lnkAptoHosp14.Visible = False
            hddHospNome14.Value = ""
            hddHosp14.Value = "0"
            hddHospInfo14.Value = ""
            lblAptoHosp15.Text = ""
            lblAptoHosp15.ForeColor = Drawing.Color.Empty
            lblAptoHosp15.Font.Italic = False
            lblAptoHosp15.Visible = False
            lnkAptoHosp15.Text = ""
            lnkAptoHosp15.ForeColor = Drawing.Color.Empty
            lnkAptoHosp15.Font.Italic = False
            lnkAptoHosp15.Font.Underline = False
            lnkAptoHosp15.Visible = False
            hddHospNome15.Value = ""
            hddHosp15.Value = "0"
            hddHospInfo15.Value = ""
            lblAptoHosp16.Text = ""
            lblAptoHosp16.ForeColor = Drawing.Color.Empty
            lblAptoHosp16.Font.Italic = False
            lblAptoHosp16.Font.Underline = False
            lblAptoHosp16.Visible = False
            lnkAptoHosp16.Text = ""
            lnkAptoHosp16.ForeColor = Drawing.Color.Empty
            lnkAptoHosp16.Font.Italic = False
            lnkAptoHosp16.Font.Underline = False
            lnkAptoHosp16.Visible = False
            hddHospNome16.Value = ""
            hddHosp16.Value = "0"
            hddHospInfo16.Value = ""
            lblAptoHosp17.Text = ""
            lblAptoHosp17.ForeColor = Drawing.Color.Empty
            lblAptoHosp17.Font.Italic = False
            lblAptoHosp17.Font.Underline = False
            lblAptoHosp17.Visible = False
            lnkAptoHosp17.Text = ""
            lnkAptoHosp17.ForeColor = Drawing.Color.Empty
            lnkAptoHosp17.Font.Italic = False
            lnkAptoHosp17.Font.Underline = False
            lnkAptoHosp17.Visible = False
            hddHospNome17.Value = ""
            hddHosp17.Value = "0"
            hddHospInfo17.Value = ""
            lblAptoHosp18.Text = ""
            lblAptoHosp18.ForeColor = Drawing.Color.Empty
            lblAptoHosp18.Font.Italic = False
            lblAptoHosp18.Font.Underline = False
            lblAptoHosp18.Visible = False
            lnkAptoHosp18.Text = ""
            lnkAptoHosp18.ForeColor = Drawing.Color.Empty
            lnkAptoHosp18.Font.Italic = False
            lnkAptoHosp18.Font.Underline = False
            lnkAptoHosp18.Visible = False
            hddHospNome18.Value = ""
            hddHosp18.Value = "0"
            hddHospInfo18.Value = ""
            lblAptoHosp19.Text = ""
            lblAptoHosp19.ForeColor = Drawing.Color.Empty
            lblAptoHosp19.Font.Italic = False
            lblAptoHosp19.Font.Underline = False
            lblAptoHosp19.Visible = False
            lnkAptoHosp19.Text = ""
            lnkAptoHosp19.ForeColor = Drawing.Color.Empty
            lnkAptoHosp19.Font.Italic = False
            lnkAptoHosp19.Font.Underline = False
            lnkAptoHosp19.Visible = False
            hddHospNome19.Value = ""
            hddHosp19.Value = "0"
            hddHospInfo19.Value = ""
            lblAptoHosp10.Text = ""
            lblAptoHosp10.ForeColor = Drawing.Color.Empty
            lblAptoHosp10.Font.Italic = False
            lblAptoHosp10.Font.Underline = False
            lblAptoHosp10.Visible = False
            lnkAptoHosp10.Text = ""
            lnkAptoHosp10.ForeColor = Drawing.Color.Empty
            lnkAptoHosp10.Font.Italic = False
            lnkAptoHosp10.Font.Underline = False
            lnkAptoHosp10.Visible = False
            hddHospNome10.Value = ""
            hddHosp10.Value = "0"
            hddHospInfo10.Value = ""
            hddResId1.Value = "0"
            hddSolId1.Value = "0"
            If lista.Count > 0 Then
                Dim objCheckInOutVO As Turismo.CheckInOutVO = lista.Item(0)
                lblApto1.Text = objCheckInOutVO.apaDesc & " até " & Mid(objCheckInOutVO.intDataFim, 1, 5)
                hddResId1.Value = objCheckInOutVO.resId
                hddSolId1.Value = objCheckInOutVO.solId
                hddAcmId1.Value = objCheckInOutVO.acmId
                hddDataFim1.Value = objCheckInOutVO.intDataFim
                hddQtde1.Value = objCheckInOutVO.resHospede

                'DisponibilidadeTroca()

                For Each item As Turismo.CheckInOutVO In lista
                    If hddResId1.Value = item.resId Then
                        If lnkAptoHosp11.Text = "" Then
                            lnkAptoHosp11.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp11.Text = lnkAptoHosp11.Text
                            lblAptoHosp11.ForeColor = Drawing.Color.Purple
                            lblAptoHosp11.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp11.Font.Underline = (item.berco = "S")
                            lnkAptoHosp11.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp11.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp11.Font.Underline = (item.berco = "S")
                            hddHospNome11.Value = lnkAptoHosp11.Text
                            hddHosp11.Value = item.intId
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo11.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo11.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp12.Text = "" Then
                            lnkAptoHosp12.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp12.Text = lnkAptoHosp12.Text
                            lblAptoHosp12.ForeColor = Drawing.Color.Purple
                            lblAptoHosp12.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp12.Font.Underline = (item.berco = "S")
                            lnkAptoHosp12.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp12.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp12.Font.Underline = (item.berco = "S")
                            hddHospNome12.Value = lnkAptoHosp12.Text
                            hddHosp12.Value = item.intId
                            lnkAptoHosp12.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo12.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo12.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp13.Text = "" Then
                            lnkAptoHosp13.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp13.Text = lnkAptoHosp13.Text
                            lblAptoHosp13.ForeColor = Drawing.Color.Purple
                            lblAptoHosp13.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp13.Font.Underline = (item.berco = "S")
                            lnkAptoHosp13.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp13.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp13.Font.Underline = (item.berco = "S")
                            hddHospNome13.Value = lnkAptoHosp13.Text
                            hddHosp13.Value = item.intId
                            lnkAptoHosp13.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo13.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo13.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp14.Text = "" Then
                            lnkAptoHosp14.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp14.Text = lnkAptoHosp14.Text
                            lblAptoHosp14.ForeColor = Drawing.Color.Purple
                            lblAptoHosp14.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp14.Font.Underline = (item.berco = "S")
                            lnkAptoHosp14.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp14.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp14.Font.Underline = (item.berco = "S")
                            hddHospNome14.Value = lnkAptoHosp14.Text
                            hddHosp14.Value = item.intId
                            lnkAptoHosp14.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo14.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo14.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp15.Text = "" Then
                            lnkAptoHosp15.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp15.Text = lnkAptoHosp15.Text
                            lblAptoHosp15.ForeColor = Drawing.Color.Purple
                            lblAptoHosp15.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp15.Font.Underline = (item.berco = "S")
                            lnkAptoHosp15.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp15.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp15.Font.Underline = (item.berco = "S")
                            hddHospNome15.Value = lnkAptoHosp15.Text
                            hddHosp15.Value = item.intId
                            lnkAptoHosp15.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo15.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo15.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp16.Text = "" Then
                            lnkAptoHosp16.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp16.Text = lnkAptoHosp16.Text
                            lblAptoHosp16.ForeColor = Drawing.Color.Purple
                            lblAptoHosp16.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp16.Font.Underline = (item.berco = "S")
                            lnkAptoHosp16.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp16.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp16.Font.Underline = (item.berco = "S")
                            hddHospNome16.Value = lnkAptoHosp16.Text
                            hddHosp16.Value = item.intId
                            lnkAptoHosp16.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo16.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo16.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp17.Text = "" Then
                            lnkAptoHosp17.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp17.Text = lnkAptoHosp17.Text
                            lblAptoHosp17.ForeColor = Drawing.Color.Purple
                            lblAptoHosp17.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp17.Font.Underline = (item.berco = "S")
                            lnkAptoHosp17.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp17.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp17.Font.Underline = (item.berco = "S")
                            hddHospNome17.Value = lnkAptoHosp17.Text
                            hddHosp17.Value = item.intId
                            lnkAptoHosp17.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo17.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo17.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp18.Text = "" Then
                            lnkAptoHosp18.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp18.Text = lnkAptoHosp18.Text
                            lblAptoHosp18.ForeColor = Drawing.Color.Purple
                            lblAptoHosp18.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp18.Font.Underline = (item.berco = "S")
                            lnkAptoHosp18.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp18.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp18.Font.Underline = (item.berco = "S")
                            hddHospNome18.Value = lnkAptoHosp18.Text
                            hddHosp18.Value = item.intId
                            lnkAptoHosp18.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo18.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo18.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp19.Text = "" Then
                            lnkAptoHosp19.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp19.Text = lnkAptoHosp19.Text
                            lblAptoHosp19.ForeColor = Drawing.Color.Purple
                            lblAptoHosp19.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp19.Font.Underline = (item.berco = "S")
                            lnkAptoHosp19.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp19.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp19.Font.Underline = (item.berco = "S")
                            hddHospNome19.Value = lnkAptoHosp19.Text
                            hddHosp19.Value = item.intId
                            lnkAptoHosp19.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo19.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo19.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp10.Text = "" Then
                            lnkAptoHosp10.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lblAptoHosp10.Text = lnkAptoHosp10.Text
                            lblAptoHosp10.ForeColor = Drawing.Color.Purple
                            lblAptoHosp10.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp10.Font.Underline = (item.berco = "S")
                            lnkAptoHosp10.ForeColor = Drawing.Color.Purple
                            lnkAptoHosp10.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp10.Font.Underline = (item.berco = "S")
                            hddHospNome10.Value = lnkAptoHosp10.Text
                            hddHosp10.Value = item.intId
                            lnkAptoHosp10.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo10.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo10.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        End If
                    End If
                Next
            Else
                Dim objApartamentoVO As Turismo.ApartamentoVO = objApartamentoDAO.consultarViaApaId(Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
                lblApto1.Text = objApartamentoVO.apaDesc
                hddResId1.Value = "0"
                hddSolId1.Value = "0"
                hddAcmId1.Value = objApartamentoVO.acmId
                hddDataFim1.Value = ""
                hddQtde1.Value = "0"
                lblVlrOriginal1.Text = ""
                lblVlrTroca1.Text = ""
                lblVlrDiferenca1.Text = ""
            End If
            lista = objAcomodacaoDAO.consultarViaAcmId(hddAcmId1.Value)
            hddCapacidade1.Value = (DirectCast(lista, ArrayList).Item(0).acmCC * 2) + DirectCast(lista, ArrayList).Item(0).acmCS
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                LocalizaCtrlFonteDisponibilidade(pnlTrocaAnhanguera, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaBambui, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaWilton, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaKilzer, "Todos")
            Else
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoA, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoB, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoC, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoD, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoE, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoEE, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoF, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoG, "Todos")
            End If
            If lnkAptoHosp11.Text > "" And lnkAptoHosp21.Text = "" Then
                Dim objTransferenciaDAO As Turismo.TransferenciaDAO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
                Else
                    objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
                End If
                lista = objTransferenciaDAO.consultar(Format(Date.Now, "dd/MM/yyyy"), "12", hddDataFim1.Value, "12", 0)
                If lista.Count > 0 Then
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaAnhanguera, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaBambui, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaWilton, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaKilzer, "Todos")
                        For Each item As Turismo.TransferenciaVO In lista
                            If InStr(item.apaDesc, "RT") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaAnhanguera, item.codId)
                            ElseIf InStr(item.apaDesc, "RA") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaBambui, item.codId)
                            ElseIf InStr(item.apaDesc, "RV") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaWilton, item.codId)
                            ElseIf InStr(item.apaDesc, "RP") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaKilzer, item.codId)
                            End If
                        Next
                    Else
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoA, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoB, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoC, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoD, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoE, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoEE, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoF, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoG, "Todos")
                        For Each item As Turismo.TransferenciaVO In lista
                            If InStr(item.apaDesc, "Bloco A") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoA, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco B") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoB, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco C") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoC, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco D") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoD, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco E") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoE, item.codId)
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoEE, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco F") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoF, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco G") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoG, item.codId)
                            End If
                        Next
                    End If
                End If
            End If

            'DisponibilidadeTroca()

            'hddDisponibilidadeTroca.Value = "N"
            'If hddApto1.Value > "" And hddApto2.Value > "" Then
            '    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Os apartamentos foram posicionados." + "');", True)
            '    If hddResId1.Value = hddResId2.Value Then
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId idênticos." + "');", True)
            '    Else
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId diferentes." + "');", True)
            '    End If

            '    If hddAcmId1.Value = hddAcmId2.Value Then
            '        hddDisponibilidadeTroca.Value = "S"
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Acomodações idênticas." + "');", True)
            '    Else
            '        Dim objDisponibilidadeDAO As Turismo.DisponibilidadeDAO
            '        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            '            objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialCaldas")
            '        Else
            '            objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO( "TurismoSocialPiri")
            '        End If
            '        If hddDataFim1.Value > "" And hddDataFim2.Value > "" Then
            '            If hddDataFim1.Value <> hddDataFim2.Value Then
            '                If CDate(hddDataFim1.Value) > CDate(hddDataFim2.Value) Then
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
            '                Else
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
            '                End If
            '                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas diferentes." + "');", True)
            '            Else
            '                hddDisponibilidadeTroca.Value = "S"
            '                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas idênticas." + "');", True)
            '            End If
            '        Else
            '            If hddDataFim1.Value = "" Or hddDataFim2.Value = "" Then
            '                If hddDataFim1.Value > "" Then
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
            '                Else
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
            '                End If
            '            Else
            '                hddDisponibilidadeTroca.Value = "S"
            '            End If
            '        End If
            '    End If

            '    If hddCapacidade1.Value < hddQtde2.Value Or hddCapacidade2.Value < hddQtde1.Value Then
            '        hddDisponibilidadeTroca.Value = "N"
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade suportada." + "');", True)
            '    Else
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade não suportada." + "');", True)
            '    End If
            'End If
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + hddDisponibilidadeTroca.Value + "');", True)

            If hddArrastouIntegrante.Value > "" Then
                hddArrastouIntegrante.Value = ""
                hddResId2.Value = "0"
                hddSolId2.Value = "0"
                lblApto2.Text = "Arraste o apto aqui"
                hddAcmId2.Value = "0"
                hddCapacidade2.Value = ""
                LocalizaCtrlRecomecar(pnlApto2)
                If hddApto2.Value > "" Then
                    hddApto2_ValueChanged(sender, e)
                End If
            Else
                SimulaValorTroca()
            End If
            'SimulaValorTroca()

            lnkAptoHosp11.Visible = (lnkAptoHosp11.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp12.Visible = (lnkAptoHosp12.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp13.Visible = (lnkAptoHosp13.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp14.Visible = (lnkAptoHosp14.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp15.Visible = (lnkAptoHosp15.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp16.Visible = (lnkAptoHosp16.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp17.Visible = (lnkAptoHosp17.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp18.Visible = (lnkAptoHosp18.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp19.Visible = (lnkAptoHosp19.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp10.Visible = (lnkAptoHosp10.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lblAptoHosp11.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp12.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp13.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp14.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp15.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp16.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp17.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp18.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp19.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp10.Visible = Not lnkAptoHosp11.Visible

            lnkAptoHosp21.Visible = (lnkAptoHosp21.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp22.Visible = (lnkAptoHosp22.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp23.Visible = (lnkAptoHosp23.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp24.Visible = (lnkAptoHosp24.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp25.Visible = (lnkAptoHosp25.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp26.Visible = (lnkAptoHosp26.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp27.Visible = (lnkAptoHosp27.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp28.Visible = (lnkAptoHosp28.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp29.Visible = (lnkAptoHosp29.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp20.Visible = (lnkAptoHosp20.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lblAptoHosp21.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp22.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp23.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp24.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp25.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp26.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp27.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp28.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp29.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp20.Visible = Not lnkAptoHosp21.Visible
            'SimulaValorTroca()
        End If
    End Sub

    Protected Sub hddApto2_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles hddApto2.ValueChanged
        If (hddApto2.Value = hddApto1.Value) Or _
            (Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4) = Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4)) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "O apartamento já está selecionado." + "');", True)
            hddApto2.Value = hddApto.Value
        ElseIf hddApto2.Value <> hddApto.Value Then
            Dim objCheckInOutDAO As Turismo.CheckInOutDAO
            Dim objApartamentoDAO As Turismo.ApartamentoDAO
            Dim objAcomodacaoDAO As Turismo.AcomodacaoDAO

            'DisponibilidadeTroca()

            Dim lista As New ArrayList
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
                objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialCaldas")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialCaldas")
            Else
                objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
                objApartamentoDAO = New Turismo.ApartamentoDAO("TurismoSocialPiri")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialPiri")
            End If
            lista = objCheckInOutDAO.consultar("0", "AptoId", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4), 0, 0, True, True, False, False, "h.HosDataIniReal desc, h.HosDataFimReal, intNome")
            lblAptoHosp21.Text = ""
            lblAptoHosp21.ForeColor = Drawing.Color.Empty
            lblAptoHosp21.Font.Italic = False
            lblAptoHosp21.Font.Underline = False
            lblAptoHosp21.Visible = False
            lnkAptoHosp21.Text = ""
            lnkAptoHosp21.ForeColor = Drawing.Color.Empty
            lnkAptoHosp21.Font.Italic = False
            lnkAptoHosp21.Font.Underline = False
            lnkAptoHosp21.Visible = False
            hddHospNome21.Value = ""
            hddHosp21.Value = "0"
            hddHospInfo21.Value = ""

            lblAptoHosp22.Text = ""
            lblAptoHosp22.ForeColor = Drawing.Color.Empty
            lblAptoHosp22.Font.Italic = False
            lblAptoHosp22.Font.Underline = False
            lblAptoHosp22.Visible = False
            lnkAptoHosp22.Text = ""
            lnkAptoHosp22.ForeColor = Drawing.Color.Empty
            lnkAptoHosp22.Font.Italic = False
            lnkAptoHosp22.Font.Underline = False
            lnkAptoHosp22.Visible = False
            hddHospNome22.Value = ""
            hddHosp22.Value = "0"
            hddHospInfo22.Value = ""

            lblAptoHosp23.Text = ""
            lblAptoHosp23.ForeColor = Drawing.Color.Empty
            lblAptoHosp23.Font.Italic = False
            lblAptoHosp23.Font.Underline = False
            lblAptoHosp23.Visible = False
            lnkAptoHosp23.Text = ""
            lnkAptoHosp23.ForeColor = Drawing.Color.Empty
            lnkAptoHosp23.Font.Italic = False
            lnkAptoHosp23.Font.Underline = False
            lnkAptoHosp23.Visible = False
            hddHospNome23.Value = ""
            hddHosp23.Value = "0"
            hddHospInfo23.Value = ""

            lblAptoHosp24.Text = ""
            lblAptoHosp24.ForeColor = Drawing.Color.Empty
            lblAptoHosp24.Font.Italic = False
            lblAptoHosp24.Font.Underline = False
            lblAptoHosp24.Visible = False
            lnkAptoHosp24.Text = ""
            lnkAptoHosp24.ForeColor = Drawing.Color.Empty
            lnkAptoHosp24.Font.Italic = False
            lnkAptoHosp24.Font.Underline = False
            lnkAptoHosp24.Visible = False
            hddHospNome24.Value = ""
            hddHosp24.Value = "0"
            hddHospInfo24.Value = ""

            lblAptoHosp25.Text = ""
            lblAptoHosp25.ForeColor = Drawing.Color.Empty
            lblAptoHosp25.Font.Italic = False
            lblAptoHosp25.Font.Underline = False
            lblAptoHosp25.Visible = False
            lnkAptoHosp25.Text = ""
            lnkAptoHosp25.ForeColor = Drawing.Color.Empty
            lnkAptoHosp25.Font.Italic = False
            lnkAptoHosp25.Font.Underline = False
            lnkAptoHosp25.Visible = False
            hddHospNome25.Value = ""
            hddHosp25.Value = "0"
            hddHospInfo25.Value = ""

            lblAptoHosp26.Text = ""
            lblAptoHosp26.ForeColor = Drawing.Color.Empty
            lblAptoHosp26.Font.Italic = False
            lblAptoHosp26.Font.Underline = False
            lblAptoHosp26.Visible = False
            lnkAptoHosp26.Text = ""
            lnkAptoHosp26.ForeColor = Drawing.Color.Empty
            lnkAptoHosp26.Font.Italic = False
            lnkAptoHosp26.Font.Underline = False
            lnkAptoHosp26.Visible = False
            hddHospNome26.Value = ""
            hddHosp26.Value = "0"
            hddHospInfo26.Value = ""

            lblAptoHosp27.Text = ""
            lblAptoHosp27.ForeColor = Drawing.Color.Empty
            lblAptoHosp27.Font.Italic = False
            lblAptoHosp27.Font.Underline = False
            lblAptoHosp27.Visible = False
            lnkAptoHosp27.Text = ""
            lnkAptoHosp27.ForeColor = Drawing.Color.Empty
            lnkAptoHosp27.Font.Italic = False
            lnkAptoHosp27.Font.Underline = False
            lnkAptoHosp27.Visible = False
            hddHospNome27.Value = ""
            hddHosp27.Value = "0"
            hddHospInfo27.Value = ""

            lblAptoHosp28.Text = ""
            lblAptoHosp28.ForeColor = Drawing.Color.Empty
            lblAptoHosp28.Font.Italic = False
            lblAptoHosp28.Font.Underline = False
            lblAptoHosp28.Visible = False
            lnkAptoHosp28.Text = ""
            lnkAptoHosp28.ForeColor = Drawing.Color.Empty
            lnkAptoHosp28.Font.Italic = False
            lnkAptoHosp28.Font.Underline = False
            lnkAptoHosp28.Visible = False
            hddHospNome28.Value = ""
            hddHosp28.Value = "0"
            hddHospInfo28.Value = ""

            lblAptoHosp29.Text = ""
            lblAptoHosp29.ForeColor = Drawing.Color.Empty
            lblAptoHosp29.Font.Italic = False
            lblAptoHosp29.Font.Underline = False
            lblAptoHosp29.Visible = False
            lnkAptoHosp29.Text = ""
            lnkAptoHosp29.ForeColor = Drawing.Color.Empty
            lnkAptoHosp29.Font.Italic = False
            lnkAptoHosp29.Font.Underline = False
            lnkAptoHosp29.Visible = False
            hddHospNome29.Value = ""
            hddHosp29.Value = "0"
            hddHospInfo29.Value = ""

            lblAptoHosp20.Text = ""
            lblAptoHosp20.ForeColor = Drawing.Color.Empty
            lblAptoHosp20.Font.Italic = False
            lblAptoHosp20.Font.Underline = False
            lblAptoHosp20.Visible = False
            lnkAptoHosp20.Text = ""
            lnkAptoHosp20.ForeColor = Drawing.Color.Empty
            lnkAptoHosp20.Font.Italic = False
            lnkAptoHosp20.Font.Underline = False
            lnkAptoHosp20.Visible = False
            hddHospNome20.Value = ""
            hddHosp20.Value = "0"
            hddHospInfo20.Value = ""
            hddResId2.Value = "0"
            hddSolId2.Value = "0"

            If lista.Count > 0 Then
                Dim objCheckInOutVO As Turismo.CheckInOutVO = lista.Item(0)
                lblApto2.Text = objCheckInOutVO.apaDesc & " até " & Mid(objCheckInOutVO.intDataFim, 1, 5)
                hddResId2.Value = objCheckInOutVO.resId
                hddSolId2.Value = objCheckInOutVO.solId
                hddAcmId2.Value = objCheckInOutVO.acmId
                hddDataFim2.Value = objCheckInOutVO.intDataFim
                hddQtde2.Value = objCheckInOutVO.resHospede
                For Each item As Turismo.CheckInOutVO In lista
                    If hddResId2.Value = item.resId Then
                        If lnkAptoHosp21.Text = "" Then
                            lnkAptoHosp21.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp21.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp21.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp21.Font.Underline = (item.berco = "S")

                            lblAptoHosp21.Text = lnkAptoHosp21.Text
                            lblAptoHosp21.ForeColor = Drawing.Color.Brown
                            lblAptoHosp21.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp21.Font.Underline = (item.berco = "S")

                            hddHospNome21.Value = lnkAptoHosp21.Text
                            hddHosp21.Value = item.intId
                            lnkAptoHosp21.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo21.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo21.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp22.Text = "" Then
                            lnkAptoHosp22.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp22.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp22.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp22.Font.Underline = (item.berco = "S")

                            lblAptoHosp22.Text = lnkAptoHosp22.Text
                            lblAptoHosp22.ForeColor = Drawing.Color.Brown
                            lblAptoHosp22.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp22.Font.Underline = (item.berco = "S")

                            hddHospNome22.Value = lnkAptoHosp22.Text
                            hddHosp22.Value = item.intId
                            lnkAptoHosp22.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo22.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo22.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp23.Text = "" Then
                            lnkAptoHosp23.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp23.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp23.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp23.Font.Underline = (item.berco = "S")

                            lblAptoHosp23.Text = lnkAptoHosp23.Text
                            lblAptoHosp23.ForeColor = Drawing.Color.Brown
                            lblAptoHosp23.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp23.Font.Underline = (item.berco = "S")

                            hddHospNome23.Value = lnkAptoHosp23.Text
                            hddHosp23.Value = item.intId
                            lnkAptoHosp23.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo23.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo23.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp24.Text = "" Then
                            lnkAptoHosp24.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp24.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp24.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp24.Font.Underline = (item.berco = "S")

                            lblAptoHosp24.Text = lnkAptoHosp24.Text
                            lblAptoHosp24.ForeColor = Drawing.Color.Brown
                            lblAptoHosp24.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp24.Font.Underline = (item.berco = "S")

                            hddHospNome24.Value = lnkAptoHosp24.Text
                            hddHosp24.Value = item.intId
                            lnkAptoHosp24.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo24.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo24.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp25.Text = "" Then
                            lnkAptoHosp25.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp25.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp25.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp25.Font.Underline = (item.berco = "S")

                            lblAptoHosp25.Text = lnkAptoHosp25.Text
                            lblAptoHosp25.ForeColor = Drawing.Color.Brown
                            lblAptoHosp25.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp25.Font.Underline = (item.berco = "S")

                            hddHospNome25.Value = lnkAptoHosp25.Text
                            hddHosp25.Value = item.intId
                            lnkAptoHosp25.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo25.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo25.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp26.Text = "" Then
                            lnkAptoHosp26.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp26.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp26.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp26.Font.Underline = (item.berco = "S")

                            lblAptoHosp26.Text = lnkAptoHosp26.Text
                            lblAptoHosp26.ForeColor = Drawing.Color.Brown
                            lblAptoHosp26.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp26.Font.Underline = (item.berco = "S")

                            hddHospNome26.Value = lnkAptoHosp26.Text
                            hddHosp26.Value = item.intId
                            lnkAptoHosp26.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo26.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo26.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp27.Text = "" Then
                            lnkAptoHosp27.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp27.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp27.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp27.Font.Underline = (item.berco = "S")

                            lblAptoHosp27.Text = lnkAptoHosp27.Text
                            lblAptoHosp27.ForeColor = Drawing.Color.Brown
                            lblAptoHosp27.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp27.Font.Underline = (item.berco = "S")

                            hddHospNome27.Value = lnkAptoHosp27.Text
                            hddHosp27.Value = item.intId
                            lnkAptoHosp27.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo27.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo27.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp28.Text = "" Then
                            lnkAptoHosp28.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp28.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp28.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp28.Font.Underline = (item.berco = "S")

                            lblAptoHosp28.Text = lnkAptoHosp28.Text
                            lblAptoHosp28.ForeColor = Drawing.Color.Brown
                            lblAptoHosp28.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp28.Font.Underline = (item.berco = "S")

                            hddHospNome28.Value = lnkAptoHosp28.Text
                            hddHosp28.Value = item.intId
                            lnkAptoHosp28.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo28.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo28.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp29.Text = "" Then
                            lnkAptoHosp29.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp29.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp29.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp29.Font.Underline = (item.berco = "S")

                            lblAptoHosp29.Text = lnkAptoHosp29.Text
                            lblAptoHosp29.ForeColor = Drawing.Color.Brown
                            lblAptoHosp29.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp29.Font.Underline = (item.berco = "S")

                            hddHospNome29.Value = lnkAptoHosp29.Text
                            hddHosp29.Value = item.intId
                            lnkAptoHosp29.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo29.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo29.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        ElseIf lnkAptoHosp20.Text = "" Then
                            lnkAptoHosp20.Text = Mid(item.intNome, 1, item.intNome.IndexOf("("))
                            lnkAptoHosp20.ForeColor = Drawing.Color.Brown
                            lnkAptoHosp20.Font.Italic = (item.cartao = "Isento")
                            lnkAptoHosp20.Font.Underline = (item.berco = "S")

                            lblAptoHosp20.Text = lnkAptoHosp20.Text
                            lblAptoHosp20.ForeColor = Drawing.Color.Brown
                            lblAptoHosp20.Font.Italic = (item.cartao = "Isento")
                            lblAptoHosp20.Font.Underline = (item.berco = "S")

                            hddHospNome20.Value = lnkAptoHosp20.Text
                            hddHosp20.Value = item.intId
                            lnkAptoHosp20.Visible = True
                            If DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) > 0 Then
                                hddHospInfo20.Value = item.catId & "," & item.intCatCobranca & "#" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataIni)) & "*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            Else
                                hddHospInfo20.Value = item.catId & "," & item.intCatCobranca & "#0*" & _
                                  DateDiff(DateInterval.Day, Now.Date, CDate(item.intDataFim)) & "$" & _
                                  item.acmId & ";"
                            End If
                        End If
                    End If
                Next
            Else
                Dim objApartamentoVO As Turismo.ApartamentoVO = objApartamentoDAO.consultarViaApaId(Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
                lblApto2.Text = objApartamentoVO.apaDesc
                hddResId2.Value = "0"
                hddSolId2.Value = "0"
                hddAcmId2.Value = objApartamentoVO.acmId
                hddDataFim2.Value = ""
                hddQtde2.Value = "0"
                lblVlrOriginal2.Text = ""
                lblVlrTroca2.Text = ""
                lblVlrDiferenca2.Text = ""
            End If
            lista = objAcomodacaoDAO.consultarViaAcmId(hddAcmId2.Value)
            hddCapacidade2.Value = (DirectCast(lista, ArrayList).Item(0).acmCC * 2) + DirectCast(lista, ArrayList).Item(0).acmCS
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                LocalizaCtrlFonteDisponibilidade(pnlTrocaAnhanguera, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaBambui, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaWilton, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaKilzer, "Todos")
            Else
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoA, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoB, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoC, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoD, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoE, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoEE, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoF, "Todos")
                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoG, "Todos")
            End If
            If lnkAptoHosp21.Text > "" And lnkAptoHosp11.Text = "" Then
                Dim objTransferenciaDAO As Turismo.TransferenciaDAO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
                Else
                    objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
                End If
                lista = objTransferenciaDAO.consultar(Format(Date.Now, "dd/MM/yyyy"), "12", hddDataFim2.Value, "12", 0)
                If lista.Count > 0 Then
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaAnhanguera, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaBambui, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaWilton, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaKilzer, "Todos")
                        For Each item As Turismo.TransferenciaVO In lista
                            If InStr(item.apaDesc, "RT") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaAnhanguera, item.codId)
                            ElseIf InStr(item.apaDesc, "RA") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaBambui, item.codId)
                            ElseIf InStr(item.apaDesc, "RV") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaWilton, item.codId)
                            ElseIf InStr(item.apaDesc, "RP") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaKilzer, item.codId)
                            End If
                        Next
                    Else
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoA, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoB, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoC, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoD, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoE, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoEE, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoF, "Todos")
                        LocalizaCtrlFonteSemDisponibilidade(pnlTrocaPiriBlocoG, "Todos")
                        For Each item As Turismo.TransferenciaVO In lista
                            If InStr(item.apaDesc, "Bloco A") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoA, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco B") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoB, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco C") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoC, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco D") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoD, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco E") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoE, item.codId)
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoEE, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco F") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoF, item.codId)
                            ElseIf InStr(item.apaDesc, "Bloco G") > 0 Then
                                LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoG, item.codId)
                            End If
                        Next
                    End If
                End If
            End If

            'DisponibilidadeTroca()

            'hddDisponibilidadeTroca.Value = "N"
            'If hddApto1.Value > "" And hddApto2.Value > "" Then
            '    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Os apartamentos foram posicionados." + "');", True)
            '    If hddResId1.Value = hddResId2.Value Then
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId idênticos." + "');", True)
            '    Else
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId diferentes." + "');", True)
            '    End If
            '    If hddAcmId1.Value = hddAcmId2.Value Then
            '        hddDisponibilidadeTroca.Value = "S"
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Acomodações idênticas." + "');", True)
            '    Else
            '        Dim objDisponibilidadeDAO As Turismo.DisponibilidadeDAO
            '        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            '            objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialCaldas")
            '        Else
            '            objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO( "TurismoSocialPiri")
            '        End If
            '        If hddDataFim1.Value > "" And hddDataFim2.Value > "" Then
            '            If hddDataFim1.Value <> hddDataFim2.Value Then
            '                If CDate(hddDataFim1.Value) > CDate(hddDataFim2.Value) Then
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
            '                Else
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
            '                End If
            '                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas diferentes." + "');", True)
            '            Else
            '                hddDisponibilidadeTroca.Value = "S"
            '                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Datas idênticas." + "');", True)
            '            End If
            '        Else
            '            If hddDataFim1.Value = "" Or hddDataFim2.Value = "" Then
            '                If hddDataFim1.Value > "" Then
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim1.Value, "12", Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4))
            '                Else
            '                    hddDisponibilidadeTroca.Value = objDisponibilidadeDAO.consultarDisponibilidadeTroca(CStr(Now.Date), "12", hddDataFim2.Value, "12", Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4))
            '                End If
            '            Else
            '                hddDisponibilidadeTroca.Value = "S"
            '            End If
            '        End If
            '    End If
            '    If hddCapacidade1.Value < hddQtde2.Value Or hddCapacidade2.Value < hddQtde1.Value Then
            '        hddDisponibilidadeTroca.Value = "N"
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade suportada." + "');", True)
            '    Else
            '        'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Capacidade não suportada." + "');", True)
            '    End If
            'End If
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + hddDisponibilidadeTroca.Value + "');", True)

            If hddArrastouIntegrante.Value > "" Then
                hddArrastouIntegrante.Value = ""
                hddResId1.Value = "0"
                hddSolId1.Value = "0"
                lblApto1.Text = "Arraste o apto aqui"
                hddAcmId1.Value = "0"
                hddCapacidade1.Value = ""
                LocalizaCtrlRecomecar(pnlApto1)
                If hddApto1.Value > "" Then
                    hddApto1_ValueChanged(sender, e)
                End If
            Else
                SimulaValorTroca()
            End If
            'SimulaValorTroca()

            lnkAptoHosp11.Visible = (lnkAptoHosp11.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp12.Visible = (lnkAptoHosp12.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp13.Visible = (lnkAptoHosp13.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp14.Visible = (lnkAptoHosp14.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp15.Visible = (lnkAptoHosp15.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp16.Visible = (lnkAptoHosp16.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp17.Visible = (lnkAptoHosp17.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp18.Visible = (lnkAptoHosp18.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp19.Visible = (lnkAptoHosp19.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp10.Visible = (lnkAptoHosp10.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lblAptoHosp11.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp12.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp13.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp14.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp15.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp16.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp17.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp18.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp19.Visible = Not lnkAptoHosp11.Visible
            lblAptoHosp10.Visible = Not lnkAptoHosp11.Visible

            lnkAptoHosp21.Visible = (lnkAptoHosp21.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp22.Visible = (lnkAptoHosp22.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp23.Visible = (lnkAptoHosp23.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp24.Visible = (lnkAptoHosp24.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp25.Visible = (lnkAptoHosp25.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp26.Visible = (lnkAptoHosp26.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp27.Visible = (lnkAptoHosp27.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp28.Visible = (lnkAptoHosp28.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp29.Visible = (lnkAptoHosp29.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lnkAptoHosp20.Visible = (lnkAptoHosp20.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0")))) And _
                ((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
            lblAptoHosp21.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp22.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp23.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp24.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp25.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp26.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp27.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp28.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp29.Visible = Not lnkAptoHosp21.Visible
            lblAptoHosp20.Visible = Not lnkAptoHosp21.Visible
            'SimulaValorTroca()
        End If
    End Sub

    Protected Sub btnRedestribuir1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRedestribuir1.Click
        hddArrastouIntegrante.Value = "Sim"
        If (hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0")) And hddAcmId1.Value > "0") Then
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId OK." + "');", True)
            Dim IntId As String, Hospede As String, Info As String, Italico As Boolean, Sublinhado As Boolean
            If Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 12, 1) = "2" Then

                'DisponibilidadeTroca()

                If Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "1" Then
                    IntId = hddHosp21.Value
                    Hospede = hddHospNome21.Value
                    Info = hddHospInfo21.Value
                    Italico = lnkAptoHosp21.Font.Italic
                    Sublinhado = lnkAptoHosp21.Font.Underline
                    hddHosp21.Value = "0"
                    hddHospNome21.Value = ""
                    hddHospInfo21.Value = ""
                    lnkAptoHosp21.Text = ""
                    lnkAptoHosp21.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp21.Font.Italic = False
                    lnkAptoHosp21.Font.Underline = False
                    lnkAptoHosp21.Visible = False
                    lblAptoHosp21.Text = ""
                    lblAptoHosp21.ForeColor = Drawing.Color.Empty
                    lblAptoHosp21.Font.Italic = False
                    lblAptoHosp21.Font.Underline = False
                    lblAptoHosp21.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "2" Then
                    IntId = hddHosp22.Value
                    Hospede = hddHospNome22.Value
                    Info = hddHospInfo22.Value
                    Italico = lnkAptoHosp22.Font.Italic
                    Sublinhado = lnkAptoHosp22.Font.Underline
                    hddHosp22.Value = "0"
                    hddHospNome22.Value = ""
                    hddHospInfo22.Value = ""
                    lnkAptoHosp22.Text = ""
                    lnkAptoHosp22.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp22.Font.Italic = False
                    lnkAptoHosp22.Font.Underline = False
                    lnkAptoHosp22.Visible = False
                    lblAptoHosp22.Text = ""
                    lblAptoHosp22.ForeColor = Drawing.Color.Empty
                    lblAptoHosp22.Font.Italic = False
                    lblAptoHosp22.Font.Underline = False
                    lblAptoHosp22.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "3" Then
                    IntId = hddHosp23.Value
                    Hospede = hddHospNome23.Value
                    Info = hddHospInfo23.Value
                    Italico = lnkAptoHosp23.Font.Italic
                    Sublinhado = lnkAptoHosp23.Font.Underline
                    hddHosp23.Value = "0"
                    hddHospNome23.Value = ""
                    hddHospInfo23.Value = ""
                    lnkAptoHosp23.Text = ""
                    lnkAptoHosp23.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp23.Font.Italic = False
                    lnkAptoHosp23.Font.Underline = False
                    lnkAptoHosp23.Visible = False
                    lblAptoHosp23.Text = ""
                    lblAptoHosp23.ForeColor = Drawing.Color.Empty
                    lblAptoHosp23.Font.Italic = False
                    lblAptoHosp23.Font.Underline = False
                    lblAptoHosp23.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "4" Then
                    IntId = hddHosp24.Value
                    Hospede = hddHospNome24.Value
                    Info = hddHospInfo24.Value
                    Italico = lnkAptoHosp24.Font.Italic
                    Sublinhado = lnkAptoHosp24.Font.Underline
                    hddHosp24.Value = "0"
                    hddHospNome24.Value = ""
                    hddHospInfo24.Value = ""
                    lnkAptoHosp24.Text = ""
                    lnkAptoHosp24.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp24.Font.Italic = False
                    lnkAptoHosp24.Font.Underline = False
                    lnkAptoHosp24.Visible = False
                    lblAptoHosp24.Text = ""
                    lblAptoHosp24.ForeColor = Drawing.Color.Empty
                    lblAptoHosp24.Font.Italic = False
                    lblAptoHosp24.Font.Underline = False
                    lblAptoHosp24.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "5" Then
                    IntId = hddHosp25.Value
                    Hospede = hddHospNome25.Value
                    Info = hddHospInfo25.Value
                    Italico = lnkAptoHosp25.Font.Italic
                    Sublinhado = lnkAptoHosp25.Font.Underline
                    hddHosp25.Value = "0"
                    hddHospNome25.Value = ""
                    hddHospInfo25.Value = ""
                    lnkAptoHosp25.Text = ""
                    lnkAptoHosp25.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp25.Font.Italic = False
                    lnkAptoHosp25.Font.Underline = False
                    lnkAptoHosp25.Visible = False
                    lblAptoHosp25.Text = ""
                    lblAptoHosp25.ForeColor = Drawing.Color.Empty
                    lblAptoHosp25.Font.Italic = False
                    lblAptoHosp25.Font.Underline = False
                    lblAptoHosp25.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "6" Then
                    IntId = hddHosp26.Value
                    Hospede = hddHospNome26.Value
                    Info = hddHospInfo26.Value
                    Italico = lnkAptoHosp26.Font.Italic
                    Sublinhado = lnkAptoHosp26.Font.Underline
                    hddHosp26.Value = "0"
                    hddHospNome26.Value = ""
                    hddHospInfo26.Value = ""
                    lnkAptoHosp26.Text = ""
                    lnkAptoHosp26.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp26.Font.Italic = False
                    lnkAptoHosp26.Font.Underline = False
                    lnkAptoHosp26.Visible = False
                    lblAptoHosp26.Text = ""
                    lblAptoHosp26.ForeColor = Drawing.Color.Empty
                    lblAptoHosp26.Font.Italic = False
                    lblAptoHosp26.Font.Underline = False
                    lblAptoHosp26.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "7" Then
                    IntId = hddHosp27.Value
                    Hospede = hddHospNome27.Value
                    Info = hddHospInfo27.Value
                    Italico = lnkAptoHosp27.Font.Italic
                    Sublinhado = lnkAptoHosp27.Font.Underline
                    hddHosp27.Value = "0"
                    hddHospNome27.Value = ""
                    hddHospInfo27.Value = ""
                    lnkAptoHosp27.Text = ""
                    lnkAptoHosp27.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp27.Font.Italic = False
                    lnkAptoHosp27.Font.Underline = False
                    lnkAptoHosp27.Visible = False
                    lblAptoHosp27.Text = ""
                    lblAptoHosp27.ForeColor = Drawing.Color.Empty
                    lblAptoHosp27.Font.Italic = False
                    lblAptoHosp27.Font.Underline = False
                    lblAptoHosp27.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "8" Then
                    IntId = hddHosp28.Value
                    Hospede = hddHospNome28.Value
                    Info = hddHospInfo28.Value
                    Italico = lnkAptoHosp28.Font.Italic
                    Sublinhado = lnkAptoHosp28.Font.Underline
                    hddHosp28.Value = "0"
                    hddHospNome28.Value = ""
                    hddHospInfo28.Value = ""
                    lnkAptoHosp28.Text = ""
                    lnkAptoHosp28.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp28.Font.Italic = False
                    lnkAptoHosp28.Font.Underline = False
                    lnkAptoHosp28.Visible = False
                    lblAptoHosp28.Text = ""
                    lblAptoHosp28.ForeColor = Drawing.Color.Empty
                    lblAptoHosp28.Font.Italic = False
                    lblAptoHosp28.Font.Underline = False
                    lblAptoHosp28.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "9" Then
                    IntId = hddHosp29.Value
                    Hospede = hddHospNome29.Value
                    Info = hddHospInfo29.Value
                    Italico = lnkAptoHosp29.Font.Italic
                    Sublinhado = lnkAptoHosp29.Font.Underline
                    hddHosp29.Value = "0"
                    hddHospNome29.Value = ""
                    hddHospInfo29.Value = ""
                    lnkAptoHosp29.Text = ""
                    lnkAptoHosp29.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp29.Font.Italic = False
                    lnkAptoHosp29.Font.Underline = False
                    lnkAptoHosp29.Visible = False
                    lblAptoHosp29.Text = ""
                    lblAptoHosp29.ForeColor = Drawing.Color.Empty
                    lblAptoHosp29.Font.Italic = False
                    lblAptoHosp29.Font.Underline = False
                    lblAptoHosp29.Visible = False

                ElseIf Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 13, 1) = "0" Then
                    IntId = hddHosp20.Value
                    Hospede = hddHospNome20.Value
                    Info = hddHospInfo20.Value
                    Italico = lnkAptoHosp20.Font.Italic
                    Sublinhado = lnkAptoHosp20.Font.Underline
                    hddHosp20.Value = "0"
                    hddHospNome20.Value = ""
                    hddHospInfo20.Value = ""
                    lnkAptoHosp20.Text = ""
                    lnkAptoHosp20.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp20.Font.Italic = False
                    lnkAptoHosp20.Font.Underline = False
                    lnkAptoHosp20.Visible = False
                    lblAptoHosp20.Text = ""
                    lblAptoHosp20.ForeColor = Drawing.Color.Empty
                    lblAptoHosp20.Font.Italic = False
                    lblAptoHosp20.Font.Underline = False
                    lblAptoHosp20.Visible = False
                End If

                If lnkAptoHosp11.Text = "" Then
                    lnkAptoHosp11.Text = Hospede
                    lnkAptoHosp11.Font.Italic = Italico
                    lnkAptoHosp11.Font.Underline = Sublinhado
                    lblAptoHosp11.Text = Hospede
                    lblAptoHosp11.Font.Italic = Italico
                    lblAptoHosp11.Font.Underline = Sublinhado
                    hddHosp11.Value = -1 * IntId
                    If hddHosp11.Value > 0 Then
                        lnkAptoHosp11.ForeColor = Drawing.Color.Purple
                        lblAptoHosp11.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp11.ForeColor = Drawing.Color.Brown
                        lblAptoHosp11.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome11.Value = Hospede
                    hddHospInfo11.Value = Info
                ElseIf lnkAptoHosp12.Text = "" Then
                    lnkAptoHosp12.Text = Hospede
                    'lnkAptoHosp12.Visible = True
                    lnkAptoHosp12.Font.Italic = Italico
                    lnkAptoHosp12.Font.Underline = Sublinhado
                    lblAptoHosp12.Text = Hospede
                    lblAptoHosp12.Font.Italic = Italico
                    lblAptoHosp12.Font.Underline = Sublinhado
                    hddHosp12.Value = -1 * IntId
                    If hddHosp12.Value > 0 Then
                        lnkAptoHosp12.ForeColor = Drawing.Color.Purple
                        lblAptoHosp12.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp12.ForeColor = Drawing.Color.Brown
                        lblAptoHosp12.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome12.Value = Hospede
                    hddHospInfo12.Value = Info
                ElseIf lnkAptoHosp13.Text = "" Then
                    lnkAptoHosp13.Text = Hospede
                    'lnkAptoHosp13.Visible = True
                    lnkAptoHosp13.Font.Italic = Italico
                    lnkAptoHosp13.Font.Underline = Sublinhado
                    lblAptoHosp13.Text = Hospede
                    lblAptoHosp13.Font.Italic = Italico
                    lblAptoHosp13.Font.Underline = Sublinhado
                    hddHosp13.Value = -1 * IntId
                    If hddHosp13.Value > 0 Then
                        lnkAptoHosp13.ForeColor = Drawing.Color.Purple
                        lblAptoHosp13.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp13.ForeColor = Drawing.Color.Brown
                        lblAptoHosp13.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome13.Value = Hospede
                    hddHospInfo13.Value = Info
                ElseIf lnkAptoHosp14.Text = "" Then
                    lnkAptoHosp14.Text = Hospede
                    'lnkAptoHosp14.Visible = True
                    lnkAptoHosp14.Font.Italic = Italico
                    lnkAptoHosp14.Font.Underline = Sublinhado
                    lblAptoHosp14.Text = Hospede
                    lblAptoHosp14.Font.Italic = Italico
                    lblAptoHosp14.Font.Underline = Sublinhado
                    hddHosp14.Value = -1 * IntId
                    If hddHosp14.Value > 0 Then
                        lnkAptoHosp14.ForeColor = Drawing.Color.Purple
                        lblAptoHosp14.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp14.ForeColor = Drawing.Color.Brown
                        lblAptoHosp14.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome14.Value = Hospede
                    hddHospInfo14.Value = Info
                ElseIf lnkAptoHosp15.Text = "" Then
                    lnkAptoHosp15.Text = Hospede
                    'lnkAptoHosp15.Visible = True
                    lnkAptoHosp15.Font.Italic = Italico
                    lnkAptoHosp15.Font.Underline = Sublinhado
                    lblAptoHosp15.Text = Hospede
                    lblAptoHosp15.Font.Italic = Italico
                    lblAptoHosp15.Font.Underline = Sublinhado
                    hddHosp15.Value = -1 * IntId
                    If hddHosp15.Value > 0 Then
                        lnkAptoHosp15.ForeColor = Drawing.Color.Purple
                        lblAptoHosp15.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp15.ForeColor = Drawing.Color.Brown
                        lblAptoHosp15.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome15.Value = Hospede
                    hddHospInfo15.Value = Info
                ElseIf lnkAptoHosp16.Text = "" Then
                    lnkAptoHosp16.Text = Hospede
                    'lnkAptoHosp16.Visible = True
                    lnkAptoHosp16.Font.Italic = Italico
                    lnkAptoHosp16.Font.Underline = Sublinhado
                    lblAptoHosp16.Text = Hospede
                    lblAptoHosp16.Font.Italic = Italico
                    lblAptoHosp16.Font.Underline = Sublinhado
                    hddHosp16.Value = -1 * IntId
                    If hddHosp16.Value > 0 Then
                        lnkAptoHosp16.ForeColor = Drawing.Color.Purple
                        lblAptoHosp16.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp16.ForeColor = Drawing.Color.Brown
                        lblAptoHosp16.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome16.Value = Hospede
                    hddHospInfo16.Value = Info
                ElseIf lnkAptoHosp17.Text = "" Then
                    lnkAptoHosp17.Text = Hospede
                    'lnkAptoHosp17.Visible = True
                    lnkAptoHosp17.Font.Italic = Italico
                    lnkAptoHosp17.Font.Underline = Sublinhado
                    lblAptoHosp17.Text = Hospede
                    lblAptoHosp17.Font.Italic = Italico
                    lblAptoHosp17.Font.Underline = Sublinhado
                    hddHosp17.Value = -1 * IntId
                    If hddHosp17.Value > 0 Then
                        lnkAptoHosp17.ForeColor = Drawing.Color.Purple
                        lblAptoHosp17.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp17.ForeColor = Drawing.Color.Brown
                        lblAptoHosp17.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome17.Value = Hospede
                    hddHospInfo17.Value = Info
                ElseIf lnkAptoHosp18.Text = "" Then
                    lnkAptoHosp18.Text = Hospede
                    'lnkAptoHosp18.Visible = True
                    lnkAptoHosp18.Font.Italic = Italico
                    lnkAptoHosp18.Font.Underline = Sublinhado
                    lblAptoHosp18.Text = Hospede
                    lblAptoHosp18.Font.Italic = Italico
                    lblAptoHosp18.Font.Underline = Sublinhado
                    hddHosp18.Value = -1 * IntId
                    If hddHosp18.Value > 0 Then
                        lnkAptoHosp18.ForeColor = Drawing.Color.Purple
                        lblAptoHosp18.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp18.ForeColor = Drawing.Color.Brown
                        lblAptoHosp18.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome18.Value = Hospede
                    hddHospInfo18.Value = Info
                ElseIf lnkAptoHosp19.Text = "" Then
                    lnkAptoHosp19.Text = Hospede
                    'lnkAptoHosp19.Visible = True
                    lnkAptoHosp19.Font.Italic = Italico
                    lnkAptoHosp19.Font.Underline = Sublinhado
                    lblAptoHosp19.Text = Hospede
                    lblAptoHosp19.Font.Underline = Sublinhado
                    hddHosp19.Value = -1 * IntId
                    If hddHosp19.Value > 0 Then
                        lnkAptoHosp19.ForeColor = Drawing.Color.Purple
                        lblAptoHosp19.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp19.ForeColor = Drawing.Color.Brown
                        lblAptoHosp19.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome19.Value = Hospede
                    hddHospInfo19.Value = Info
                ElseIf lnkAptoHosp10.Text = "" Then
                    lnkAptoHosp10.Text = Hospede
                    'lnkAptoHosp10.Visible = True
                    lnkAptoHosp10.Font.Italic = Italico
                    lnkAptoHosp10.Font.Underline = Sublinhado
                    lblAptoHosp10.Text = Hospede
                    lblAptoHosp10.Font.Italic = Italico
                    lblAptoHosp10.Font.Underline = Sublinhado
                    hddHosp10.Value = -1 * IntId
                    If hddHosp10.Value > 0 Then
                        lnkAptoHosp10.ForeColor = Drawing.Color.Purple
                        lblAptoHosp10.ForeColor = Drawing.Color.Purple
                    Else
                        lnkAptoHosp10.ForeColor = Drawing.Color.Brown
                        lblAptoHosp10.ForeColor = Drawing.Color.Brown
                    End If
                    hddQtde1.Value += 1 And Not Sublinhado
                    hddQtde2.Value -= 1 And Not Sublinhado
                    hddHospNome10.Value = Hospede
                    hddHospInfo10.Value = Info
                End If
                hddHosp1.Value = ""
                If hddHosp21.Value = "0" Then
                    hddHosp21.Value = hddHosp22.Value
                    hddHospNome21.Value = hddHospNome22.Value
                    hddHospInfo21.Value = hddHospInfo22.Value
                    lnkAptoHosp21.Text = lnkAptoHosp22.Text
                    lnkAptoHosp21.ForeColor = lnkAptoHosp22.ForeColor
                    lnkAptoHosp21.Font.Italic = lnkAptoHosp22.Font.Italic
                    lnkAptoHosp21.Font.Underline = lnkAptoHosp22.Font.Underline
                    lnkAptoHosp21.Visible = (lnkAptoHosp21.Text > "")
                    hddHosp22.Value = "0"
                    hddHospNome22.Value = ""
                    hddHospInfo22.Value = ""
                    lnkAptoHosp22.Text = ""
                    lnkAptoHosp22.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp22.Font.Italic = False
                    lnkAptoHosp22.Font.Underline = False
                    lnkAptoHosp22.Visible = False
                    lblAptoHosp22.Text = ""
                    lblAptoHosp22.ForeColor = Drawing.Color.Empty
                    lblAptoHosp22.Font.Italic = False
                    lblAptoHosp22.Font.Underline = False
                End If
                If hddHosp22.Value = "0" Then
                    hddHosp22.Value = hddHosp23.Value
                    hddHospNome22.Value = hddHospNome23.Value
                    hddHospInfo22.Value = hddHospInfo23.Value
                    lnkAptoHosp22.Text = lnkAptoHosp23.Text
                    lnkAptoHosp22.ForeColor = lnkAptoHosp23.ForeColor
                    lnkAptoHosp22.Font.Italic = lnkAptoHosp23.Font.Italic
                    lnkAptoHosp22.Font.Underline = lnkAptoHosp23.Font.Underline
                    lnkAptoHosp22.Visible = (lnkAptoHosp22.Text > "")
                    hddHosp23.Value = "0"
                    hddHospNome23.Value = ""
                    hddHospInfo23.Value = ""
                    lnkAptoHosp23.Text = ""
                    lnkAptoHosp23.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp23.Font.Italic = False
                    lnkAptoHosp23.Font.Underline = False
                    lnkAptoHosp23.Visible = False
                    lblAptoHosp23.Text = ""
                    lblAptoHosp23.ForeColor = Drawing.Color.Empty
                    lblAptoHosp23.Font.Italic = False
                    lblAptoHosp23.Font.Underline = False
                End If
                If hddHosp23.Value = "0" Then
                    hddHosp23.Value = hddHosp24.Value
                    hddHospNome23.Value = hddHospNome24.Value
                    hddHospInfo23.Value = hddHospInfo24.Value
                    lnkAptoHosp23.Text = lnkAptoHosp24.Text
                    lnkAptoHosp23.ForeColor = lnkAptoHosp24.ForeColor
                    lnkAptoHosp23.Font.Italic = lnkAptoHosp24.Font.Italic
                    lnkAptoHosp23.Font.Underline = lnkAptoHosp24.Font.Underline
                    lnkAptoHosp23.Visible = (lnkAptoHosp23.Text > "")
                    hddHosp24.Value = "0"
                    hddHospNome24.Value = ""
                    hddHospInfo24.Value = ""
                    lnkAptoHosp24.Text = ""
                    lnkAptoHosp24.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp24.Font.Italic = False
                    lnkAptoHosp24.Font.Underline = False
                    lnkAptoHosp24.Visible = False
                    lblAptoHosp24.Text = ""
                    lblAptoHosp24.ForeColor = Drawing.Color.Empty
                    lblAptoHosp24.Font.Italic = False
                    lblAptoHosp24.Font.Underline = False
                End If
                If hddHosp24.Value = "0" Then
                    hddHosp24.Value = hddHosp25.Value
                    hddHospNome24.Value = hddHospNome25.Value
                    hddHospInfo24.Value = hddHospInfo25.Value
                    lnkAptoHosp24.Text = lnkAptoHosp25.Text
                    lnkAptoHosp24.ForeColor = lnkAptoHosp25.ForeColor
                    lnkAptoHosp24.Font.Italic = lnkAptoHosp25.Font.Italic
                    lnkAptoHosp24.Font.Underline = lnkAptoHosp25.Font.Underline
                    lnkAptoHosp24.Visible = (lnkAptoHosp24.Text > "")
                    hddHosp25.Value = "0"
                    hddHospNome25.Value = ""
                    hddHospInfo25.Value = ""
                    lnkAptoHosp25.Text = ""
                    lnkAptoHosp25.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp25.Font.Italic = False
                    lnkAptoHosp25.Font.Underline = False
                    lnkAptoHosp25.Visible = False
                    lblAptoHosp25.Text = ""
                    lblAptoHosp25.ForeColor = Drawing.Color.Empty
                    lblAptoHosp25.Font.Italic = False
                    lblAptoHosp25.Font.Underline = False
                End If
                If hddHosp25.Value = "0" Then
                    hddHosp25.Value = hddHosp26.Value
                    hddHospNome25.Value = hddHospNome26.Value
                    hddHospInfo25.Value = hddHospInfo26.Value
                    lnkAptoHosp25.Text = lnkAptoHosp26.Text
                    lnkAptoHosp25.ForeColor = lnkAptoHosp26.ForeColor
                    lnkAptoHosp25.Font.Italic = lnkAptoHosp26.Font.Italic
                    lnkAptoHosp25.Font.Underline = lnkAptoHosp26.Font.Underline
                    lnkAptoHosp25.Visible = (lnkAptoHosp26.Text > "")
                    hddHosp26.Value = "0"
                    hddHospNome26.Value = ""
                    hddHospInfo26.Value = ""
                    lnkAptoHosp26.Text = ""
                    lnkAptoHosp26.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp26.Font.Italic = False
                    lnkAptoHosp26.Font.Underline = False
                    lnkAptoHosp26.Visible = False
                    lblAptoHosp26.Text = ""
                    lblAptoHosp26.ForeColor = Drawing.Color.Empty
                    lblAptoHosp26.Font.Italic = False
                    lblAptoHosp26.Font.Underline = False
                End If
                If hddHosp26.Value = "0" Then
                    hddHosp26.Value = hddHosp27.Value
                    hddHospNome26.Value = hddHospNome27.Value
                    hddHospInfo26.Value = hddHospInfo27.Value
                    lnkAptoHosp26.Text = lnkAptoHosp27.Text
                    lnkAptoHosp26.ForeColor = lnkAptoHosp27.ForeColor
                    lnkAptoHosp26.Font.Italic = lnkAptoHosp27.Font.Italic
                    lnkAptoHosp26.Font.Underline = lnkAptoHosp27.Font.Underline
                    lnkAptoHosp26.Visible = (lnkAptoHosp27.Text > "")
                    hddHosp27.Value = "0"
                    hddHospNome27.Value = ""
                    hddHospInfo27.Value = ""
                    lnkAptoHosp27.Text = ""
                    lnkAptoHosp27.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp27.Font.Italic = False
                    lnkAptoHosp27.Font.Underline = False
                    lnkAptoHosp27.Visible = False
                    lblAptoHosp27.Text = ""
                    lblAptoHosp27.ForeColor = Drawing.Color.Empty
                    lblAptoHosp27.Font.Italic = False
                    lblAptoHosp27.Font.Underline = False
                End If
                If hddHosp27.Value = "0" Then
                    hddHosp27.Value = hddHosp28.Value
                    hddHospNome27.Value = hddHospNome28.Value
                    hddHospInfo27.Value = hddHospInfo28.Value
                    lnkAptoHosp27.Text = lnkAptoHosp28.Text
                    lnkAptoHosp27.ForeColor = lnkAptoHosp28.ForeColor
                    lnkAptoHosp27.Font.Italic = lnkAptoHosp28.Font.Italic
                    lnkAptoHosp27.Font.Underline = lnkAptoHosp28.Font.Underline
                    lnkAptoHosp27.Visible = (lnkAptoHosp28.Text > "")
                    hddHosp28.Value = "0"
                    hddHospNome28.Value = ""
                    hddHospInfo28.Value = ""
                    lnkAptoHosp28.Text = ""
                    lnkAptoHosp28.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp28.Font.Italic = False
                    lnkAptoHosp28.Font.Underline = False
                    lnkAptoHosp28.Visible = False
                    lblAptoHosp28.Text = ""
                    lblAptoHosp28.ForeColor = Drawing.Color.Empty
                    lblAptoHosp28.Font.Italic = False
                    lblAptoHosp28.Font.Underline = False
                End If
                If hddHosp28.Value = "0" Then
                    hddHosp28.Value = hddHosp29.Value
                    hddHospNome28.Value = hddHospNome29.Value
                    hddHospInfo28.Value = hddHospInfo29.Value
                    lnkAptoHosp28.Text = lnkAptoHosp29.Text
                    lnkAptoHosp28.ForeColor = lnkAptoHosp29.ForeColor
                    lnkAptoHosp28.Font.Italic = lnkAptoHosp29.Font.Italic
                    lnkAptoHosp28.Font.Underline = lnkAptoHosp29.Font.Underline
                    lnkAptoHosp28.Visible = (lnkAptoHosp29.Text > "")
                    hddHosp29.Value = "0"
                    hddHospNome29.Value = ""
                    hddHospInfo29.Value = ""
                    lnkAptoHosp29.Text = ""
                    lnkAptoHosp29.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp29.Font.Italic = False
                    lnkAptoHosp29.Font.Underline = False
                    lnkAptoHosp29.Visible = False
                    lblAptoHosp29.Text = ""
                    lblAptoHosp29.ForeColor = Drawing.Color.Empty
                    lblAptoHosp29.Font.Italic = False
                    lblAptoHosp29.Font.Underline = False
                End If
                If hddHosp29.Value = "0" Then
                    hddHosp29.Value = hddHosp20.Value
                    hddHospNome29.Value = hddHospNome20.Value
                    hddHospInfo29.Value = hddHospInfo20.Value
                    lnkAptoHosp29.Text = lnkAptoHosp20.Text
                    lnkAptoHosp29.ForeColor = lnkAptoHosp20.ForeColor
                    lnkAptoHosp29.Font.Italic = lnkAptoHosp20.Font.Italic
                    lnkAptoHosp29.Font.Underline = lnkAptoHosp20.Font.Underline
                    lnkAptoHosp29.Visible = (lnkAptoHosp20.Text > "")
                    hddHosp20.Value = "0"
                    hddHospNome20.Value = ""
                    hddHospInfo20.Value = ""
                    lnkAptoHosp20.Text = ""
                    lnkAptoHosp20.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp20.Font.Italic = False
                    lnkAptoHosp20.Font.Underline = False
                    lnkAptoHosp20.Visible = False
                    lblAptoHosp20.Text = ""
                    lblAptoHosp20.ForeColor = Drawing.Color.Empty
                    lblAptoHosp20.Font.Italic = False
                    lblAptoHosp20.Font.Underline = False
                End If

                SimulaValorTroca()

                lnkAptoHosp11.Visible = (lnkAptoHosp11.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp12.Visible = (lnkAptoHosp12.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp13.Visible = (lnkAptoHosp13.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp14.Visible = (lnkAptoHosp14.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp15.Visible = (lnkAptoHosp15.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp16.Visible = (lnkAptoHosp16.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp17.Visible = (lnkAptoHosp17.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp18.Visible = (lnkAptoHosp18.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp19.Visible = (lnkAptoHosp19.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp10.Visible = (lnkAptoHosp10.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lblAptoHosp11.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp12.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp13.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp14.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp15.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp16.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp17.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp18.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp19.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp10.Visible = Not lnkAptoHosp11.Visible

                lnkAptoHosp21.Visible = (lnkAptoHosp21.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp22.Visible = (lnkAptoHosp22.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp23.Visible = (lnkAptoHosp23.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp24.Visible = (lnkAptoHosp24.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp25.Visible = (lnkAptoHosp25.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp26.Visible = (lnkAptoHosp26.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp27.Visible = (lnkAptoHosp27.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp28.Visible = (lnkAptoHosp28.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp29.Visible = (lnkAptoHosp29.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp20.Visible = (lnkAptoHosp20.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lblAptoHosp21.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp22.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp23.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp24.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp25.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp26.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp27.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp28.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp29.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp20.Visible = Not lnkAptoHosp21.Visible
            End If
        Else
            If Mid(hddHosp1.Value, hddHosp1.Value.IndexOf("lnkAptoHosp") + 12, 1) = "2" Then
                hddArrastouIntegrante.Value = ""
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Intercâmbio de integrantes apenas para acomodações com o mesmo responsável ou desocupada." + "');", True)
            End If
        End If
    End Sub

    Protected Sub btnRedestribuir2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRedestribuir2.Click
        hddArrastouIntegrante.Value = "Sim"
        If (hddResId2.Value = hddResId1.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0")) And hddAcmId2.Value > "0") Then
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "ResId OK." + "');", True)
            Dim IntId As String, Hospede As String, Info As String, Italico As Boolean, Sublinhado As Boolean
            If Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 12, 1) = "1" Then

                'DisponibilidadeTroca()

                If Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "1" Then
                    IntId = hddHosp11.Value
                    Hospede = hddHospNome11.Value
                    Info = hddHospInfo11.Value
                    Italico = lnkAptoHosp11.Font.Italic
                    Sublinhado = lnkAptoHosp11.Font.Underline
                    hddHosp11.Value = "0"
                    hddHospNome11.Value = ""
                    hddHospInfo11.Value = ""
                    lnkAptoHosp11.Text = ""
                    lnkAptoHosp11.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp11.Font.Italic = False
                    lnkAptoHosp11.Font.Underline = False
                    lnkAptoHosp11.Visible = False
                    lblAptoHosp11.Text = ""
                    lblAptoHosp11.ForeColor = Drawing.Color.Empty
                    lblAptoHosp11.Font.Italic = False
                    lblAptoHosp11.Font.Underline = False
                    lblAptoHosp11.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "2" Then
                    IntId = hddHosp12.Value
                    Hospede = hddHospNome12.Value
                    Info = hddHospInfo12.Value
                    Italico = lnkAptoHosp12.Font.Italic
                    Sublinhado = lnkAptoHosp12.Font.Underline
                    hddHosp12.Value = "0"
                    hddHospNome12.Value = ""
                    hddHospInfo12.Value = ""
                    lnkAptoHosp12.Text = ""
                    lnkAptoHosp12.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp12.Font.Italic = False
                    lnkAptoHosp12.Font.Underline = False
                    lnkAptoHosp12.Visible = False
                    lblAptoHosp12.Text = ""
                    lblAptoHosp12.ForeColor = Drawing.Color.Empty
                    lblAptoHosp12.Font.Italic = False
                    lblAptoHosp12.Font.Underline = False
                    lblAptoHosp12.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "3" Then
                    IntId = hddHosp13.Value
                    Hospede = hddHospNome13.Value
                    Info = hddHospInfo13.Value
                    Italico = lnkAptoHosp13.Font.Italic
                    Sublinhado = lnkAptoHosp13.Font.Underline
                    hddHosp13.Value = "0"
                    hddHospNome13.Value = ""
                    hddHospInfo13.Value = ""
                    lnkAptoHosp13.Text = ""
                    lnkAptoHosp13.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp13.Font.Italic = False
                    lnkAptoHosp13.Font.Underline = False
                    lnkAptoHosp13.Visible = False
                    lblAptoHosp13.Text = ""
                    lblAptoHosp13.ForeColor = Drawing.Color.Empty
                    lblAptoHosp13.Font.Italic = False
                    lblAptoHosp13.Font.Underline = False
                    lblAptoHosp13.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "4" Then
                    IntId = hddHosp14.Value
                    Hospede = hddHospNome14.Value
                    Info = hddHospInfo14.Value
                    Italico = lnkAptoHosp14.Font.Italic
                    Sublinhado = lnkAptoHosp14.Font.Underline
                    hddHosp14.Value = "0"
                    hddHospNome14.Value = ""
                    hddHospInfo14.Value = ""
                    lnkAptoHosp14.Text = ""
                    lnkAptoHosp14.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp14.Font.Italic = False
                    lnkAptoHosp14.Font.Underline = False
                    lnkAptoHosp14.Visible = False
                    lblAptoHosp14.Text = ""
                    lblAptoHosp14.ForeColor = Drawing.Color.Empty
                    lblAptoHosp14.Font.Italic = False
                    lblAptoHosp14.Font.Underline = False
                    lblAptoHosp14.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "5" Then
                    IntId = hddHosp15.Value
                    Hospede = hddHospNome15.Value
                    Info = hddHospInfo15.Value
                    Italico = lnkAptoHosp15.Font.Italic
                    Sublinhado = lnkAptoHosp15.Font.Underline
                    hddHosp15.Value = "0"
                    hddHospNome15.Value = ""
                    hddHospInfo15.Value = ""
                    lnkAptoHosp15.Text = ""
                    lnkAptoHosp15.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp15.Font.Italic = False
                    lnkAptoHosp15.Font.Underline = False
                    lnkAptoHosp15.Visible = False
                    lblAptoHosp15.Text = ""
                    lblAptoHosp15.ForeColor = Drawing.Color.Empty
                    lblAptoHosp15.Font.Italic = False
                    lblAptoHosp15.Font.Underline = False
                    lblAptoHosp15.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "6" Then
                    IntId = hddHosp16.Value
                    Hospede = hddHospNome16.Value
                    Info = hddHospInfo16.Value
                    Italico = lnkAptoHosp16.Font.Italic
                    Sublinhado = lnkAptoHosp16.Font.Underline
                    hddHosp16.Value = "0"
                    hddHospNome16.Value = ""
                    hddHospInfo16.Value = ""
                    lnkAptoHosp16.Text = ""
                    lnkAptoHosp16.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp16.Font.Italic = False
                    lnkAptoHosp16.Font.Underline = False
                    lnkAptoHosp16.Visible = False
                    lblAptoHosp16.Text = ""
                    lblAptoHosp16.ForeColor = Drawing.Color.Empty
                    lblAptoHosp16.Font.Italic = False
                    lblAptoHosp16.Font.Underline = False
                    lblAptoHosp16.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "7" Then
                    IntId = hddHosp17.Value
                    Hospede = hddHospNome17.Value
                    Info = hddHospInfo17.Value
                    Italico = lnkAptoHosp17.Font.Italic
                    Sublinhado = lnkAptoHosp17.Font.Underline
                    hddHosp17.Value = "0"
                    hddHospNome17.Value = ""
                    hddHospInfo17.Value = ""
                    lnkAptoHosp17.Text = ""
                    lnkAptoHosp17.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp17.Font.Italic = False
                    lnkAptoHosp17.Font.Underline = False
                    lnkAptoHosp17.Visible = False
                    lblAptoHosp17.Text = ""
                    lblAptoHosp17.ForeColor = Drawing.Color.Empty
                    lblAptoHosp17.Font.Italic = False
                    lblAptoHosp17.Font.Underline = False
                    lblAptoHosp17.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "8" Then
                    IntId = hddHosp18.Value
                    Hospede = hddHospNome18.Value
                    Info = hddHospInfo18.Value
                    Italico = lnkAptoHosp18.Font.Italic
                    Sublinhado = lnkAptoHosp18.Font.Underline
                    hddHosp18.Value = "0"
                    hddHospNome18.Value = ""
                    hddHospInfo18.Value = ""
                    lnkAptoHosp18.Text = ""
                    lnkAptoHosp18.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp18.Font.Italic = False
                    lnkAptoHosp18.Font.Underline = False
                    lnkAptoHosp18.Visible = False
                    lblAptoHosp18.Text = ""
                    lblAptoHosp18.ForeColor = Drawing.Color.Empty
                    lblAptoHosp18.Font.Italic = False
                    lblAptoHosp18.Font.Underline = False
                    lblAptoHosp18.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "9" Then
                    IntId = hddHosp19.Value
                    Hospede = hddHospNome19.Value
                    Info = hddHospInfo19.Value
                    Italico = lnkAptoHosp19.Font.Italic
                    Sublinhado = lnkAptoHosp19.Font.Underline
                    hddHosp19.Value = "0"
                    hddHospNome19.Value = ""
                    hddHospInfo19.Value = ""
                    lnkAptoHosp19.Text = ""
                    lnkAptoHosp19.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp19.Font.Italic = False
                    lnkAptoHosp19.Font.Underline = False
                    lnkAptoHosp19.Visible = False
                    lblAptoHosp19.Text = ""
                    lblAptoHosp19.ForeColor = Drawing.Color.Empty
                    lblAptoHosp19.Font.Italic = False
                    lblAptoHosp19.Font.Underline = False
                    lblAptoHosp19.Visible = False
                ElseIf Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 13, 1) = "0" Then
                    IntId = hddHosp10.Value
                    Hospede = hddHospNome10.Value
                    Info = hddHospInfo10.Value
                    Italico = lnkAptoHosp10.Font.Italic
                    Sublinhado = lnkAptoHosp10.Font.Underline
                    hddHosp10.Value = "0"
                    hddHospNome10.Value = ""
                    hddHospInfo10.Value = ""
                    lnkAptoHosp10.Text = ""
                    lnkAptoHosp10.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp10.Font.Italic = False
                    lnkAptoHosp10.Font.Underline = False
                    lnkAptoHosp10.Visible = False
                    lblAptoHosp10.Text = ""
                    lblAptoHosp10.ForeColor = Drawing.Color.Empty
                    lblAptoHosp10.Font.Italic = False
                    lblAptoHosp10.Font.Underline = False
                    lblAptoHosp10.Visible = False
                End If
                If lnkAptoHosp21.Text = "" Then
                    lnkAptoHosp21.Text = Hospede
                    'lnkAptoHosp21.Visible = True
                    lnkAptoHosp21.Font.Italic = Italico
                    lnkAptoHosp21.Font.Underline = Sublinhado
                    lblAptoHosp21.Text = Hospede
                    lblAptoHosp21.Font.Italic = Italico
                    lblAptoHosp21.Font.Underline = Sublinhado
                    hddHosp21.Value = -1 * IntId
                    If hddHosp21.Value > 0 Then
                        lnkAptoHosp21.ForeColor = Drawing.Color.Brown
                        lblAptoHosp21.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp21.ForeColor = Drawing.Color.Purple
                        lblAptoHosp21.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome21.Value = Hospede
                    hddHospInfo21.Value = Info
                ElseIf lnkAptoHosp22.Text = "" Then
                    lnkAptoHosp22.Text = Hospede
                    'lnkAptoHosp22.Visible = True
                    lnkAptoHosp22.Font.Italic = Italico
                    lnkAptoHosp22.Font.Underline = Sublinhado
                    lblAptoHosp22.Text = Hospede
                    lblAptoHosp22.Font.Italic = Italico
                    lblAptoHosp22.Font.Underline = Sublinhado
                    hddHosp22.Value = -1 * IntId
                    If hddHosp22.Value > 0 Then
                        lnkAptoHosp22.ForeColor = Drawing.Color.Brown
                        lblAptoHosp22.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp22.ForeColor = Drawing.Color.Purple
                        lblAptoHosp22.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome22.Value = Hospede
                    hddHospInfo22.Value = Info
                ElseIf lnkAptoHosp23.Text = "" Then
                    lnkAptoHosp23.Text = Hospede
                    'lnkAptoHosp23.Visible = True
                    lnkAptoHosp23.Font.Italic = Italico
                    lnkAptoHosp23.Font.Underline = Sublinhado
                    lblAptoHosp23.Text = Hospede
                    lblAptoHosp23.Font.Italic = Italico
                    lblAptoHosp23.Font.Underline = Sublinhado
                    hddHosp23.Value = -1 * IntId
                    If hddHosp23.Value > 0 Then
                        lnkAptoHosp23.ForeColor = Drawing.Color.Brown
                        lblAptoHosp23.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp23.ForeColor = Drawing.Color.Purple
                        lblAptoHosp23.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome23.Value = Hospede
                    hddHospInfo23.Value = Info
                ElseIf lnkAptoHosp24.Text = "" Then
                    lnkAptoHosp24.Text = Hospede
                    'lnkAptoHosp24.Visible = True
                    lnkAptoHosp24.Font.Italic = Italico
                    lnkAptoHosp24.Font.Underline = Sublinhado
                    lblAptoHosp24.Text = Hospede
                    lblAptoHosp24.Font.Italic = Italico
                    lblAptoHosp24.Font.Underline = Sublinhado
                    hddHosp24.Value = -1 * IntId
                    If hddHosp24.Value > 0 Then
                        lnkAptoHosp24.ForeColor = Drawing.Color.Brown
                        lblAptoHosp24.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp24.ForeColor = Drawing.Color.Purple
                        lblAptoHosp24.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome24.Value = Hospede
                    hddHospInfo24.Value = Info
                ElseIf lnkAptoHosp25.Text = "" Then
                    lnkAptoHosp25.Text = Hospede
                    'lnkAptoHosp25.Visible = True
                    lnkAptoHosp25.Font.Italic = Italico
                    lnkAptoHosp25.Font.Underline = Sublinhado
                    lblAptoHosp25.Text = Hospede
                    lblAptoHosp25.Font.Italic = Italico
                    lblAptoHosp25.Font.Underline = Sublinhado
                    hddHosp25.Value = -1 * IntId
                    If hddHosp25.Value > 0 Then
                        lnkAptoHosp25.ForeColor = Drawing.Color.Brown
                        lblAptoHosp25.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp25.ForeColor = Drawing.Color.Purple
                        lblAptoHosp25.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome25.Value = Hospede
                    hddHospInfo25.Value = Info
                ElseIf lnkAptoHosp26.Text = "" Then
                    lnkAptoHosp26.Text = Hospede
                    'lnkAptoHosp26.Visible = True
                    lnkAptoHosp26.Font.Italic = Italico
                    lnkAptoHosp26.Font.Underline = Sublinhado
                    lblAptoHosp26.Text = Hospede
                    lblAptoHosp26.Font.Italic = Italico
                    lblAptoHosp26.Font.Underline = Sublinhado
                    hddHosp26.Value = -1 * IntId
                    If hddHosp26.Value > 0 Then
                        lnkAptoHosp26.ForeColor = Drawing.Color.Brown
                        lblAptoHosp26.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp26.ForeColor = Drawing.Color.Purple
                        lblAptoHosp26.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome26.Value = Hospede
                    hddHospInfo26.Value = Info
                ElseIf lnkAptoHosp27.Text = "" Then
                    lnkAptoHosp27.Text = Hospede
                    'lnkAptoHosp27.Visible = True
                    lnkAptoHosp27.Font.Italic = Italico
                    lnkAptoHosp27.Font.Underline = Sublinhado
                    lblAptoHosp27.Text = Hospede
                    lblAptoHosp27.Font.Italic = Italico
                    lblAptoHosp27.Font.Underline = Sublinhado
                    hddHosp27.Value = -1 * IntId
                    If hddHosp27.Value > 0 Then
                        lnkAptoHosp27.ForeColor = Drawing.Color.Brown
                        lblAptoHosp27.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp27.ForeColor = Drawing.Color.Purple
                        lblAptoHosp27.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome27.Value = Hospede
                    hddHospInfo27.Value = Info
                ElseIf lnkAptoHosp28.Text = "" Then
                    lnkAptoHosp28.Text = Hospede
                    'lnkAptoHosp28.Visible = True
                    lnkAptoHosp28.Font.Italic = Italico
                    lnkAptoHosp28.Font.Underline = Sublinhado
                    lblAptoHosp28.Text = Hospede
                    lblAptoHosp28.Font.Italic = Italico
                    lblAptoHosp28.Font.Underline = Sublinhado
                    hddHosp28.Value = -1 * IntId
                    If hddHosp28.Value > 0 Then
                        lnkAptoHosp28.ForeColor = Drawing.Color.Brown
                        lblAptoHosp28.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp28.ForeColor = Drawing.Color.Purple
                        lblAptoHosp28.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome28.Value = Hospede
                    hddHospInfo28.Value = Info
                ElseIf lnkAptoHosp29.Text = "" Then
                    lnkAptoHosp29.Text = Hospede
                    'lnkAptoHosp29.Visible = True
                    lnkAptoHosp29.Font.Italic = Italico
                    lnkAptoHosp29.Font.Underline = Sublinhado
                    lblAptoHosp29.Text = Hospede
                    lblAptoHosp29.Font.Italic = Italico
                    lblAptoHosp29.Font.Underline = Sublinhado
                    hddHosp29.Value = -1 * IntId
                    If hddHosp29.Value > 0 Then
                        lnkAptoHosp29.ForeColor = Drawing.Color.Brown
                        lblAptoHosp29.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp29.ForeColor = Drawing.Color.Purple
                        lblAptoHosp29.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome29.Value = Hospede
                    hddHospInfo29.Value = Info
                ElseIf lnkAptoHosp20.Text = "" Then
                    lnkAptoHosp20.Text = Hospede
                    'lnkAptoHosp20.Visible = True
                    lnkAptoHosp20.Font.Italic = Italico
                    lnkAptoHosp20.Font.Underline = Sublinhado
                    lblAptoHosp20.Text = Hospede
                    lblAptoHosp20.Font.Italic = Italico
                    lblAptoHosp20.Font.Underline = Sublinhado
                    hddHosp20.Value = -1 * IntId
                    If hddHosp20.Value > 0 Then
                        lnkAptoHosp20.ForeColor = Drawing.Color.Brown
                        lblAptoHosp20.ForeColor = Drawing.Color.Brown
                    Else
                        lnkAptoHosp20.ForeColor = Drawing.Color.Purple
                        lblAptoHosp20.ForeColor = Drawing.Color.Purple
                    End If
                    hddQtde2.Value += 1 And Not Sublinhado
                    hddQtde1.Value -= 1 And Not Sublinhado
                    hddHospNome20.Value = Hospede
                    hddHospInfo20.Value = Info
                End If
                hddHosp2.Value = ""
                If hddHosp11.Value = "0" Then
                    hddHosp11.Value = hddHosp12.Value
                    hddHospNome11.Value = hddHospNome12.Value
                    hddHospInfo11.Value = hddHospInfo12.Value
                    lnkAptoHosp11.Text = lnkAptoHosp12.Text
                    lnkAptoHosp11.ForeColor = lnkAptoHosp12.ForeColor
                    lnkAptoHosp11.ForeColor = lnkAptoHosp12.ForeColor
                    lnkAptoHosp11.Font.Italic = lnkAptoHosp12.Font.Italic

                    lblAptoHosp11.Text = lnkAptoHosp12.Text
                    lblAptoHosp11.ForeColor = lnkAptoHosp12.ForeColor
                    lblAptoHosp11.Font.Italic = lnkAptoHosp12.Font.Italic
                    lblAptoHosp11.Font.Underline = lnkAptoHosp12.Font.Underline
                    lblAptoHosp12.Text = ""
                    lblAptoHosp12.ForeColor = Drawing.Color.Empty
                    lblAptoHosp12.Font.Italic = False
                    lblAptoHosp12.Font.Underline = False
                    'lblAptoHosp12.Visible = False

                    hddHosp12.Value = "0"
                    hddHospNome12.Value = ""
                    hddHospInfo12.Value = ""
                    lnkAptoHosp12.Text = ""
                    lnkAptoHosp12.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp12.Font.Italic = False
                    lnkAptoHosp12.Font.Underline = False
                    'lnkAptoHosp12.Visible = False
                End If
                If hddHosp12.Value = "0" Then
                    hddHosp12.Value = hddHosp13.Value
                    hddHospNome12.Value = hddHospNome13.Value
                    hddHospInfo12.Value = hddHospInfo13.Value

                    lnkAptoHosp12.Text = lnkAptoHosp13.Text
                    lnkAptoHosp12.ForeColor = lnkAptoHosp13.ForeColor
                    lnkAptoHosp12.Font.Italic = lnkAptoHosp13.Font.Italic
                    lnkAptoHosp12.Font.Underline = lnkAptoHosp13.Font.Underline
                    'lnkAptoHosp12.Visible = (lnkAptoHosp12.Text > "")
                    lblAptoHosp12.Text = lnkAptoHosp13.Text
                    lblAptoHosp12.ForeColor = lnkAptoHosp13.ForeColor
                    lblAptoHosp12.Font.Italic = lnkAptoHosp13.Font.Italic
                    lblAptoHosp12.Font.Underline = lnkAptoHosp13.Font.Underline
                    'lblAptoHosp12.Visible = (lnkAptoHosp12.Text > "")

                    hddHosp13.Value = "0"
                    hddHospNome13.Value = ""
                    hddHospInfo13.Value = ""

                    lnkAptoHosp13.Text = ""
                    lnkAptoHosp13.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp13.Font.Italic = False
                    lnkAptoHosp13.Font.Underline = False
                    'lnkAptoHosp13.Visible = False
                    lblAptoHosp13.Text = ""
                    lblAptoHosp13.ForeColor = Drawing.Color.Empty
                    lblAptoHosp13.Font.Italic = False
                    lblAptoHosp13.Font.Underline = False
                    'lblAptoHosp13.Visible = False
                End If
                If hddHosp13.Value = "0" Then
                    hddHosp13.Value = hddHosp14.Value
                    hddHospNome13.Value = hddHospNome14.Value
                    hddHospInfo13.Value = hddHospInfo14.Value

                    lnkAptoHosp13.Text = lnkAptoHosp14.Text
                    lnkAptoHosp13.ForeColor = lnkAptoHosp14.ForeColor
                    lnkAptoHosp13.Font.Italic = lnkAptoHosp14.Font.Italic
                    lnkAptoHosp13.Font.Underline = lnkAptoHosp14.Font.Underline
                    'lnkAptoHosp13.Visible = (lnkAptoHosp13.Text > "")
                    lblAptoHosp13.Text = lnkAptoHosp14.Text
                    lblAptoHosp13.ForeColor = lnkAptoHosp14.ForeColor
                    lblAptoHosp13.Font.Italic = lnkAptoHosp14.Font.Italic
                    lblAptoHosp13.Font.Underline = lnkAptoHosp14.Font.Underline
                    'lblAptoHosp13.Visible = (lnkAptoHosp13.Text > "")

                    hddHosp14.Value = "0"
                    hddHospNome14.Value = ""
                    hddHospInfo14.Value = ""

                    lnkAptoHosp14.Text = ""
                    lnkAptoHosp14.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp14.Font.Italic = False
                    lnkAptoHosp14.Font.Underline = False
                    'lnkAptoHosp14.Visible = False
                    lblAptoHosp14.Text = ""
                    lblAptoHosp14.ForeColor = Drawing.Color.Empty
                    lblAptoHosp14.Font.Italic = False
                    lblAptoHosp14.Font.Underline = False
                    'lblAptoHosp14.Visible = False
                End If
                If hddHosp14.Value = "0" Then
                    hddHosp14.Value = hddHosp15.Value
                    hddHospNome14.Value = hddHospNome15.Value
                    hddHospInfo14.Value = hddHospInfo15.Value

                    lnkAptoHosp14.Text = lnkAptoHosp15.Text
                    lnkAptoHosp14.ForeColor = lnkAptoHosp15.ForeColor
                    lnkAptoHosp14.Font.Italic = lnkAptoHosp15.Font.Italic
                    lnkAptoHosp14.Font.Underline = lnkAptoHosp15.Font.Underline
                    'lnkAptoHosp14.Visible = (lnkAptoHosp14.Text > "")
                    lblAptoHosp14.Text = lnkAptoHosp15.Text
                    lblAptoHosp14.ForeColor = lnkAptoHosp15.ForeColor
                    lblAptoHosp14.Font.Italic = lnkAptoHosp15.Font.Italic
                    lblAptoHosp14.Font.Underline = lnkAptoHosp15.Font.Underline
                    'lblAptoHosp14.Visible = (lnkAptoHosp14.Text > "")

                    hddHosp15.Value = "0"
                    hddHospNome15.Value = ""
                    hddHospInfo15.Value = ""

                    lnkAptoHosp15.Text = ""
                    lnkAptoHosp15.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp15.Font.Italic = False
                    lnkAptoHosp15.Font.Underline = False
                    'lnkAptoHosp15.Visible = False
                    lblAptoHosp15.Text = ""
                    lblAptoHosp15.ForeColor = Drawing.Color.Empty
                    lblAptoHosp15.Font.Italic = False
                    lblAptoHosp15.Font.Underline = False
                    'lblAptoHosp15.Visible = False
                End If
                If hddHosp15.Value = "0" Then
                    hddHosp15.Value = hddHosp16.Value
                    hddHospNome15.Value = hddHospNome16.Value
                    hddHospInfo15.Value = hddHospInfo16.Value

                    lnkAptoHosp15.Text = lnkAptoHosp16.Text
                    lnkAptoHosp15.ForeColor = lnkAptoHosp16.ForeColor
                    lnkAptoHosp15.Font.Italic = lnkAptoHosp16.Font.Italic
                    lnkAptoHosp15.Font.Underline = lnkAptoHosp16.Font.Underline
                    'lnkAptoHosp15.Visible = (lnkAptoHosp16.Text > "")
                    lblAptoHosp15.Text = lnkAptoHosp16.Text
                    lblAptoHosp15.ForeColor = lnkAptoHosp16.ForeColor
                    lblAptoHosp15.Font.Italic = lnkAptoHosp16.Font.Italic
                    lblAptoHosp15.Font.Underline = lnkAptoHosp16.Font.Underline
                    'lblAptoHosp15.Visible = (lnkAptoHosp16.Text > "")

                    hddHosp16.Value = "0"
                    hddHospNome16.Value = ""
                    hddHospInfo16.Value = ""

                    lnkAptoHosp16.Text = ""
                    lnkAptoHosp16.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp16.Font.Italic = False
                    lnkAptoHosp16.Font.Underline = False
                    'lnkAptoHosp16.Visible = False
                    lblAptoHosp16.Text = ""
                    lblAptoHosp16.ForeColor = Drawing.Color.Empty
                    lblAptoHosp16.Font.Italic = False
                    lblAptoHosp16.Font.Underline = False
                    'lblAptoHosp16.Visible = False
                End If
                If hddHosp16.Value = "0" Then
                    hddHosp16.Value = hddHosp17.Value
                    hddHospNome16.Value = hddHospNome17.Value
                    hddHospInfo16.Value = hddHospInfo17.Value

                    lnkAptoHosp16.Text = lnkAptoHosp17.Text
                    lnkAptoHosp16.ForeColor = lnkAptoHosp17.ForeColor
                    lnkAptoHosp16.Font.Italic = lnkAptoHosp17.Font.Italic
                    lnkAptoHosp16.Font.Underline = lnkAptoHosp17.Font.Underline
                    'lnkAptoHosp16.Visible = (lnkAptoHosp17.Text > "")
                    lblAptoHosp16.Text = lnkAptoHosp17.Text
                    lblAptoHosp16.ForeColor = lnkAptoHosp17.ForeColor
                    lblAptoHosp16.Font.Italic = lnkAptoHosp17.Font.Italic
                    lblAptoHosp16.Font.Underline = lnkAptoHosp17.Font.Underline
                    'lblAptoHosp16.Visible = (lnkAptoHosp17.Text > "")

                    hddHosp17.Value = "0"
                    hddHospNome17.Value = ""
                    hddHospInfo17.Value = ""

                    lnkAptoHosp17.Text = ""
                    lnkAptoHosp17.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp17.Font.Italic = False
                    lnkAptoHosp17.Font.Underline = False
                    'lnkAptoHosp17.Visible = False
                    lblAptoHosp17.Text = ""
                    lblAptoHosp17.ForeColor = Drawing.Color.Empty
                    lblAptoHosp17.Font.Italic = False
                    lblAptoHosp17.Font.Underline = False
                    'lblAptoHosp17.Visible = False
                End If
                If hddHosp17.Value = "0" Then
                    hddHosp17.Value = hddHosp18.Value
                    hddHospNome17.Value = hddHospNome18.Value
                    hddHospInfo17.Value = hddHospInfo18.Value

                    lnkAptoHosp17.Text = lnkAptoHosp18.Text
                    lnkAptoHosp17.ForeColor = lnkAptoHosp18.ForeColor
                    lnkAptoHosp17.Font.Italic = lnkAptoHosp18.Font.Italic
                    lnkAptoHosp17.Font.Underline = lnkAptoHosp18.Font.Underline
                    'lnkAptoHosp17.Visible = (lnkAptoHosp18.Text > "")
                    lblAptoHosp17.Text = lnkAptoHosp18.Text
                    lblAptoHosp17.ForeColor = lnkAptoHosp18.ForeColor
                    lblAptoHosp17.Font.Italic = lnkAptoHosp18.Font.Italic
                    lblAptoHosp17.Font.Underline = lnkAptoHosp18.Font.Underline
                    'lblAptoHosp17.Visible = (lnkAptoHosp18.Text > "")

                    hddHosp18.Value = "0"
                    hddHospNome18.Value = ""
                    hddHospInfo18.Value = ""

                    lnkAptoHosp18.Text = ""
                    lnkAptoHosp18.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp18.Font.Italic = False
                    lnkAptoHosp18.Font.Underline = False
                    'lnkAptoHosp18.Visible = False
                    lblAptoHosp18.Text = ""
                    lblAptoHosp18.ForeColor = Drawing.Color.Empty
                    lblAptoHosp18.Font.Italic = False
                    lblAptoHosp18.Font.Underline = False
                    'lblAptoHosp18.Visible = False
                End If
                If hddHosp18.Value = "0" Then
                    hddHosp18.Value = hddHosp19.Value
                    hddHospNome18.Value = hddHospNome19.Value
                    hddHospInfo18.Value = hddHospInfo19.Value

                    lnkAptoHosp18.Text = lnkAptoHosp19.Text
                    lnkAptoHosp18.ForeColor = lnkAptoHosp19.ForeColor
                    lnkAptoHosp18.Font.Italic = lnkAptoHosp19.Font.Italic
                    lnkAptoHosp18.Font.Underline = lnkAptoHosp19.Font.Underline
                    'lnkAptoHosp18.Visible = (lnkAptoHosp19.Text > "")
                    lblAptoHosp18.Text = lnkAptoHosp19.Text
                    lblAptoHosp18.ForeColor = lnkAptoHosp19.ForeColor
                    lblAptoHosp18.Font.Italic = lnkAptoHosp19.Font.Italic
                    lblAptoHosp18.Font.Underline = lnkAptoHosp19.Font.Underline
                    'lblAptoHosp18.Visible = (lnkAptoHosp19.Text > "")

                    hddHosp19.Value = "0"
                    hddHospNome19.Value = ""
                    hddHospInfo19.Value = ""

                    lnkAptoHosp19.Text = ""
                    lnkAptoHosp19.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp19.Font.Italic = False
                    lnkAptoHosp19.Font.Underline = False
                    'lnkAptoHosp19.Visible = False
                    lblAptoHosp19.Text = ""
                    lblAptoHosp19.ForeColor = Drawing.Color.Empty
                    lblAptoHosp19.Font.Italic = False
                    lblAptoHosp19.Font.Underline = False
                    'lblAptoHosp19.Visible = False
                End If
                If hddHosp19.Value = "0" Then
                    hddHosp19.Value = hddHosp10.Value
                    hddHospNome19.Value = hddHospNome10.Value
                    hddHospInfo19.Value = hddHospInfo10.Value

                    lnkAptoHosp19.Text = lnkAptoHosp10.Text
                    lnkAptoHosp19.ForeColor = lnkAptoHosp10.ForeColor
                    lnkAptoHosp19.Font.Italic = lnkAptoHosp10.Font.Italic
                    lnkAptoHosp19.Font.Underline = lnkAptoHosp10.Font.Underline
                    'lnkAptoHosp19.Visible = (lnkAptoHosp10.Text > "")
                    lblAptoHosp19.Text = lnkAptoHosp10.Text
                    lblAptoHosp19.ForeColor = lnkAptoHosp10.ForeColor
                    lblAptoHosp19.Font.Italic = lnkAptoHosp10.Font.Italic
                    lblAptoHosp19.Font.Underline = lnkAptoHosp10.Font.Underline
                    'lblAptoHosp19.Visible = (lnkAptoHosp10.Text > "")

                    hddHosp10.Value = "0"
                    hddHospNome10.Value = ""
                    hddHospInfo10.Value = ""

                    lnkAptoHosp10.Text = ""
                    lnkAptoHosp10.ForeColor = Drawing.Color.Empty
                    lnkAptoHosp10.Font.Italic = False
                    lnkAptoHosp10.Font.Underline = False
                    'lnkAptoHosp10.Visible = False
                    lblAptoHosp10.Text = ""
                    lblAptoHosp10.ForeColor = Drawing.Color.Empty
                    lblAptoHosp10.Font.Italic = False
                    lblAptoHosp10.Font.Underline = False
                    'lblAptoHosp10.Visible = False
                End If

                SimulaValorTroca()

                lnkAptoHosp11.Visible = (lnkAptoHosp11.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp12.Visible = (lnkAptoHosp12.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp13.Visible = (lnkAptoHosp13.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp14.Visible = (lnkAptoHosp14.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp15.Visible = (lnkAptoHosp15.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp16.Visible = (lnkAptoHosp16.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp17.Visible = (lnkAptoHosp17.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp18.Visible = (lnkAptoHosp18.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp19.Visible = (lnkAptoHosp19.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp10.Visible = (lnkAptoHosp10.Text > "") And (hddAcmId1.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lblAptoHosp11.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp12.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp13.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp14.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp15.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp16.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp17.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp18.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp19.Visible = Not lnkAptoHosp11.Visible
                lblAptoHosp10.Visible = Not lnkAptoHosp11.Visible

                lnkAptoHosp21.Visible = (lnkAptoHosp21.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp22.Visible = (lnkAptoHosp22.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp23.Visible = (lnkAptoHosp23.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp24.Visible = (lnkAptoHosp24.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp25.Visible = (lnkAptoHosp25.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp26.Visible = (lnkAptoHosp26.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp27.Visible = (lnkAptoHosp27.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp28.Visible = (lnkAptoHosp28.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp29.Visible = (lnkAptoHosp29.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lnkAptoHosp20.Visible = (lnkAptoHosp20.Text > "") And (hddAcmId2.Value > "0") And _
                ((hddDisponibilidadeTroca.Value = "S") Or ((hddDisponibilidadeTroca.Value = "N") And ((hddResId1.Value = "0" And hddAcmId1.Value > "0") Or _
                                                                                                      (hddResId2.Value = "0" And hddAcmId2.Value > "0") Or _
                                                                                                      (hddResId1.Value = hddResId2.Value))))
                '((hddResId1.Value = hddResId2.Value) Or (((hddResId2.Value = "0") Or (hddResId1.Value = "0"))))
                lblAptoHosp21.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp22.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp23.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp24.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp25.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp26.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp27.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp28.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp29.Visible = Not lnkAptoHosp21.Visible
                lblAptoHosp20.Visible = Not lnkAptoHosp21.Visible
            End If
        Else
            If Mid(hddHosp2.Value, hddHosp2.Value.IndexOf("lnkAptoHosp") + 12, 1) = "1" Then
                hddArrastouIntegrante.Value = ""
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Intercâmbio de integrantes apenas para acomodações com o mesmo responsável ou desocupada." + "');", True)
            End If
        End If
    End Sub

    Public Sub LocalizaCtrlRecomecar(ByVal controlP As Control)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is LinkButton Then
                DirectCast(ctl, LinkButton).Text = ""
                DirectCast(ctl, LinkButton).Visible = False
            ElseIf TypeOf ctl Is HiddenField Then
                DirectCast(ctl, HiddenField).Value = ""

            ElseIf TypeOf ctl Is Label Then
                DirectCast(ctl, Label).Text = ""
                'DirectCast(ctl, Label).Visible = False
            End If
        Next
    End Sub

    Protected Sub btnLimpar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpar.Click
        hddResId1.Value = "0"
        hddSolId1.Value = "0"
        lblApto1.Text = "Arraste o apto aqui"
        hddAcmId1.Value = "0"
        hddApto1.Value = ""
        hddCapacidade1.Value = ""
        hddQtde1.Value = "0"
        lblVlrOriginal1.Text = ""
        lblVlrTroca1.Text = ""
        lblVlrDiferenca1.Text = ""
        LocalizaCtrlRecomecar(pnlApto1)

        hddResId2.Value = "0"
        hddSolId2.Value = "0"
        lblApto2.Text = "Arraste o apto aqui"
        hddAcmId2.Value = "0"
        hddApto2.Value = ""
        hddCapacidade2.Value = ""
        hddQtde2.Value = "0"
        lblVlrOriginal2.Text = ""
        lblVlrTroca2.Text = ""
        lblVlrDiferenca2.Text = ""
        LocalizaCtrlRecomecar(pnlApto2)

        hddDisponibilidadeTroca.Value = "N"
        hddArrastouIntegrante.Value = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            LocalizaCtrlFonteDisponibilidade(pnlTrocaAnhanguera, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaBambui, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaWilton, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaKilzer, "Todos")
        Else
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoA, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoB, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoC, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoD, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoE, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoEE, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoF, "Todos")
            LocalizaCtrlFonteDisponibilidade(pnlTrocaPiriBlocoG, "Todos")
        End If
        btnTroca.Enabled = False
    End Sub

    Protected Sub OKlnk1_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles OKlnk1.Click, OKlnk2.Click, OKlnk3.Click, OKlnk4.Click, OKlnk5.Click,
                OKlnk6.Click, OKlnk7.Click, OKlnk8.Click, OKlnk9.Click, OKlnk10.Click,
                OKlnk11.Click, OKlnk12.Click, OKlnk13.Click, OKlnk14.Click, OKlnk15.Click,
                OKlnk16.Click, OKlnk17.Click, OKlnk18.Click, OKlnk19.Click, OKlnk20.Click,
                OKlnk21.Click, OKlnk22.Click, OKlnk23.Click, OKlnk24.Click, OKlnk25.Click,
                OKlnk26.Click, OKlnk27.Click, OKlnk28.Click, OKlnk29.Click, OKlnk30.Click,
                OKlnk31.Click, OKlnk32.Click, OKlnk33.Click, OKlnk34.Click, OKlnk35.Click,
                OKlnk36.Click, OKlnk37.Click, OKlnk38.Click, OKlnk39.Click, OKlnk40.Click,
                OKlnk41.Click, OKlnk42.Click, OKlnk43.Click, OKlnk44.Click, OKlnk45.Click,
                OKlnk46.Click, OKlnk47.Click, OKlnk48.Click, OKlnk49.Click, OKlnk50.Click,
                OKlnk51.Click, OKlnk52.Click, OKlnk53.Click, OKlnk54.Click, OKlnk55.Click,
                OKlnk56.Click, OKlnk1719.Click, OKlnk1720.Click, OKlnk1721.Click, OKlnk1722.Click,
                OKlnk1723.Click, OKlnk1724.Click, OKlnk1725.Click,
                BBlnk91.Click, BBlnk92.Click, BBlnk93.Click, BBlnk94.Click,
                BBlnk95.Click, BBlnk96.Click, BBlnk97.Click, BBlnk98.Click, BBlnk99.Click,
                BBlnk100.Click, BBlnk101.Click, BBlnk102.Click, BBlnk103.Click, BBlnk104.Click,
                BBlnk105.Click, BBlnk106.Click, BBlnk107.Click, BBlnk108.Click, BBlnk109.Click,
                BBlnk110.Click, BBlnk111.Click, BBlnk112.Click, BBlnk113.Click, BBlnk114.Click,
                BBlnk115.Click, BBlnk116.Click, BBlnk117.Click, BBlnk118.Click, BBlnk119.Click,
                BBlnk120.Click, BBlnk121.Click, BBlnk122.Click, BBlnk123.Click, BBlnk124.Click,
                BBlnk125.Click, BBlnk126.Click, BBlnk127.Click, BBlnk128.Click, BBlnk129.Click,
                BBlnk130.Click, BBlnk131.Click, BBlnk132.Click, BBlnk133.Click, BBlnk134.Click,
                BBlnk135.Click, BBlnk136.Click, BBlnk137.Click, BBlnk138.Click, BBlnk139.Click,
                BBlnk140.Click, BBlnk141.Click, BBlnk142.Click, BBlnk143.Click, BBlnk144.Click,
                BBlnk145.Click, BBlnk146.Click, BBlnk147.Click, BBlnk148.Click, BBlnk149.Click,
                BBlnk150.Click, BBlnk151.Click, BBlnk152.Click, BBlnk153.Click, BBlnk154.Click,
                ANlnk155.Click, ANlnk156.Click, ANlnk157.Click, ANlnk158.Click, ANlnk159.Click,
                ANlnk160.Click, ANlnk161.Click, ANlnk162.Click, ANlnk163.Click, ANlnk164.Click,
                ANlnk165.Click, ANlnk166.Click, ANlnk167.Click, ANlnk168.Click, ANlnk169.Click,
                ANlnk170.Click, ANlnk171.Click, ANlnk172.Click, ANlnk173.Click, ANlnk174.Click,
                ANlnk175.Click, ANlnk176.Click, ANlnk177.Click, ANlnk178.Click, ANlnk179.Click,
                ANlnk180.Click, ANlnk181.Click, ANlnk182.Click, ANlnk183.Click, ANlnk184.Click,
                ANlnk185.Click, ANlnk186.Click, ANlnk187.Click, ANlnk188.Click, ANlnk189.Click,
                ANlnk190.Click, ANlnk191.Click, ANlnk192.Click, ANlnk193.Click, ANlnk194.Click,
                ANlnk195.Click, ANlnk196.Click, ANlnk197.Click, ANlnk198.Click, ANlnk199.Click,
                ANlnk200.Click, ANlnk201.Click, ANlnk202.Click, ANlnk203.Click, ANlnk204.Click,
                ANlnk205.Click, ANlnk206.Click, ANlnk207.Click, ANlnk208.Click, ANlnk209.Click,
                ANlnk210.Click, ANlnk211.Click, ANlnk212.Click, ANlnk213.Click, ANlnk214.Click,
                ANlnk215.Click, ANlnk216.Click, ANlnk217.Click, ANlnk218.Click, ANlnk219.Click,
                ANlnk220.Click, WHlnk1101.Click, WHlnk1102.Click, WHlnk1103.Click, WHlnk1104.Click,
                WHlnk1105.Click, WHlnk1106.Click, WHlnk1107.Click, WHlnk1108.Click, WHlnk1109.Click,
                WHlnk1110.Click, WHlnk1111.Click, WHlnk1112.Click, WHlnk1113.Click, WHlnk1114.Click,
                WHlnk1115.Click, WHlnk1116.Click, WHlnk1117.Click, WHlnk1118.Click, WHlnk1201.Click,
                WHlnk1202.Click, WHlnk1203.Click, WHlnk1204.Click, WHlnk1205.Click, WHlnk1206.Click,
                WHlnk1207.Click, WHlnk1208.Click, WHlnk1209.Click, WHlnk1210.Click, WHlnk1211.Click,
                WHlnk1212.Click, WHlnk1213.Click, WHlnk1214.Click, WHlnk1215.Click, WHlnk1216.Click,
                WHlnk1217.Click, WHlnk1301.Click, WHlnk1302.Click, WHlnk1303.Click, WHlnk1304.Click,
                WHlnk1305.Click, WHlnk1306.Click, WHlnk1307.Click, WHlnk1308.Click, WHlnk1309.Click,
                WHlnk1310.Click, WHlnk1311.Click, WHlnk1312.Click, WHlnk1313.Click, WHlnk1314.Click,
                WHlnk1315.Click, WHlnk1316.Click, WHlnk1317.Click, WHlnk1318.Click, WHlnk1401.Click,
                WHlnk1402.Click, WHlnk1403.Click, WHlnk1404.Click, WHlnk1405.Click, WHlnk1406.Click,
                WHlnk1407.Click, WHlnk1408.Click, WHlnk1409.Click, WHlnk1410.Click, WHlnk1411.Click,
                WHlnk1412.Click, WHlnk1413.Click, WHlnk1414.Click, WHlnk1415.Click, WHlnk1416.Click,
                WHlnk1417.Click, WHlnk1501.Click, WHlnk1502.Click, WHlnk1503.Click, WHlnk1504.Click,
                WHlnk1505.Click, WHlnk1506.Click, WHlnk1507.Click, WHlnk1508.Click, WHlnk1509.Click,
                WHlnk1510.Click, WHlnk1511.Click, WHlnk1512.Click, WHlnk1513.Click, WHlnk1514.Click,
                WHlnk1515.Click, WHlnk1516.Click, WHlnk1517.Click, WHlnk1518.Click, WHlnk1601.Click,
                WHlnk1602.Click, WHlnk1603.Click, WHlnk1604.Click, WHlnk1605.Click, WHlnk1606.Click,
                WHlnk1607.Click, WHlnk1608.Click, WHlnk1609.Click, WHlnk1610.Click, WHlnk1611.Click,
                WHlnk1612.Click, WHlnk1613.Click, WHlnk1614.Click, WHlnk1615.Click, WHlnk1616.Click,
                WHlnk1617.Click, WHlnk1701.Click, WHlnk1702.Click, WHlnk1703.Click, WHlnk1704.Click,
                WHlnk1705.Click, WHlnk1706.Click, WHlnk1707.Click, WHlnk1708.Click, WHlnk1709.Click,
                WHlnk1710.Click, WHlnk1711.Click, WHlnk1712.Click, WHlnk1713.Click, WHlnk1714.Click,
                WHlnk1715.Click, WHlnk1716.Click, WHlnk1717.Click, WHlnk1718.Click,
                Pirilnk101.Click, Pirilnk102.Click, Pirilnk103.Click, Pirilnk104.Click, Pirilnk105.Click,
                Pirilnk106.Click, Pirilnk107.Click, Pirilnk108.Click, Pirilnk109.Click, Pirilnk110.Click,
                Pirilnk111.Click, Pirilnk112.Click, Pirilnk113.Click, Pirilnk114.Click, Pirilnk115.Click,
                Pirilnk116.Click, Pirilnk117.Click, Pirilnk118.Click, Pirilnk119.Click, Pirilnk120.Click,
                Pirilnk121.Click, Pirilnk122.Click, Pirilnk123.Click, Pirilnk124.Click, Pirilnk125.Click,
                Pirilnk126.Click, Pirilnk1.Click, Pirilnk2.Click, Pirilnk3.Click, Pirilnk4.Click,
                Pirilnk5.Click, Pirilnk6.Click, Pirilnk7.Click, Pirilnk8.Click, Pirilnk9.Click,
                Pirilnk10.Click, Pirilnk11.Click, Pirilnk12.Click, Pirilnk13.Click, Pirilnk14.Click,
                Pirilnk15.Click, Pirilnk16.Click, Pirilnk17.Click, Pirilnk18.Click, Pirilnk19.Click

        If hddApto1.Value = "" Then
            hddApto1.Value = "lnk" & sender.CommandName
            hddApto1_ValueChanged(sender, e)
        ElseIf hddApto2.Value = "" Then
            hddApto2.Value = "lnk" & sender.CommandName
            hddApto2_ValueChanged(sender, e)
        Else
            'Se estiver cheio os dois campos, irá limpa-los e será adicionado na primeira caixa os integrantes do último clique.
            btnLimpar_Click(sender, e)
            hddApto1.Value = "lnk" & sender.CommandName
            hddApto1_ValueChanged(sender, e)
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar esta operação. Os 2 espaços para receber a acomodação estão preenchidos. Clique em Limpar para liberar nova operação." + "');", True)
        End If
    End Sub

    Protected Sub lnkAptoHosp11_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAptoHosp11.Click, lnkAptoHosp12.Click, lnkAptoHosp13.Click, lnkAptoHosp14.Click, lnkAptoHosp15.Click, lnkAptoHosp16.Click, lnkAptoHosp17.Click, lnkAptoHosp18.Click, lnkAptoHosp19.Click, lnkAptoHosp10.Click
        hddHosp2.Value = sender.ClientID
        btnRedestribuir2_Click(sender, e)
    End Sub

    Protected Sub lnkAptoHosp21_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAptoHosp21.Click, lnkAptoHosp22.Click, lnkAptoHosp23.Click, lnkAptoHosp24.Click, lnkAptoHosp25.Click, lnkAptoHosp26.Click, lnkAptoHosp27.Click, lnkAptoHosp28.Click, lnkAptoHosp29.Click, lnkAptoHosp20.Click
        hddHosp1.Value = sender.ClientID
        btnRedestribuir1_Click(sender, e)
    End Sub

    Protected Sub btnTroca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTroca.Click
        'Permuta (todos integrantes)
        'Intercâmbio (Algum integrante)
        'Desmembrar (Separar integrantes)
        Try
            hddProcessando.Value = ""
            Dim objTransferenciaDAO As Turismo.TransferenciaDAO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
                objCalculaIdadeIntegranteDAO = New CalculaIdadeIntegranteDAO("TurismoSocialCaldas")
            Else
                objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
                objCalculaIdadeIntegranteDAO = New CalculaIdadeIntegranteDAO("TurismoSocialPiri")
            End If

            Dim varTroca As String
            If hddResId1.Value = hddResId2.Value And _
                   (hddHosp10.Value.StartsWith("-") Or _
                   hddHosp11.Value.StartsWith("-") Or _
                   hddHosp12.Value.StartsWith("-") Or _
                   hddHosp13.Value.StartsWith("-") Or _
                   hddHosp14.Value.StartsWith("-") Or _
                   hddHosp15.Value.StartsWith("-") Or _
                   hddHosp16.Value.StartsWith("-") Or _
                   hddHosp17.Value.StartsWith("-") Or _
                   hddHosp18.Value.StartsWith("-") Or _
                   hddHosp19.Value.StartsWith("-") Or _
                   hddHosp20.Value.StartsWith("-") Or _
                   hddHosp21.Value.StartsWith("-") Or _
                   hddHosp22.Value.StartsWith("-") Or _
                   hddHosp23.Value.StartsWith("-") Or _
                   hddHosp24.Value.StartsWith("-") Or _
                   hddHosp25.Value.StartsWith("-") Or _
                   hddHosp26.Value.StartsWith("-") Or _
                   hddHosp27.Value.StartsWith("-") Or _
                   hddHosp28.Value.StartsWith("-") Or _
                   hddHosp29.Value.StartsWith("-")) Then
                varTroca = "Intercambio"
            ElseIf (hddResId1.Value = "0" Or hddResId2.Value = "0") And _
                   (hddHosp10.Value.StartsWith("-") Or _
                   hddHosp11.Value.StartsWith("-") Or _
                   hddHosp12.Value.StartsWith("-") Or _
                   hddHosp13.Value.StartsWith("-") Or _
                   hddHosp14.Value.StartsWith("-") Or _
                   hddHosp15.Value.StartsWith("-") Or _
                   hddHosp16.Value.StartsWith("-") Or _
                   hddHosp17.Value.StartsWith("-") Or _
                   hddHosp18.Value.StartsWith("-") Or _
                   hddHosp19.Value.StartsWith("-") Or _
                   hddHosp20.Value.StartsWith("-") Or _
                   hddHosp21.Value.StartsWith("-") Or _
                   hddHosp22.Value.StartsWith("-") Or _
                   hddHosp23.Value.StartsWith("-") Or _
                   hddHosp24.Value.StartsWith("-") Or _
                   hddHosp25.Value.StartsWith("-") Or _
                   hddHosp26.Value.StartsWith("-") Or _
                   hddHosp27.Value.StartsWith("-") Or _
                   hddHosp28.Value.StartsWith("-") Or _
                   hddHosp29.Value.StartsWith("-")) Then
                varTroca = "Desmembrar"
            Else
                varTroca = "Permuta"
            End If

            Dim SomaAdultoLadoA As Long = 0
            Dim SomaCriancasColoA As Long = 0

            Dim SomaAdultoLadoB As Long = 0
            Dim SomaCriancasColoB As Long = 0

            'Verificando maiores de idade do lado A da transferenca
            '1-Adulto/2-Criança de 2 a 5/3-Colo com menos de 2 anos(Calcula idade pelo intid )
            Dim cont As Integer = 10
            Dim HddAux As New HiddenField
            HddAux.ID = "HddAux"
            'Contando os integrantes por idade do LadoA
            For cont = 10 To 20 Step 1
                Select Case cont
                    Case 10
                        HddAux.Value = hddHosp10.Value.Replace("-", "")
                    Case 11
                        HddAux.Value = hddHosp11.Value.Replace("-", "")
                    Case 12
                        HddAux.Value = hddHosp12.Value.Replace("-", "")
                    Case 13
                        HddAux.Value = hddHosp13.Value.Replace("-", "")
                    Case 14
                        HddAux.Value = hddHosp14.Value.Replace("-", "")
                    Case 15
                        HddAux.Value = hddHosp15.Value.Replace("-", "")
                    Case 16
                        HddAux.Value = hddHosp16.Value.Replace("-", "")
                    Case 17
                        HddAux.Value = hddHosp17.Value.Replace("-", "")
                    Case 18
                        HddAux.Value = hddHosp18.Value.Replace("-", "")
                    Case 19
                        HddAux.Value = hddHosp19.Value.Replace("-", "")
                End Select

                If HddAux.Value > 0 Then
                    Select Case objCalculaIdadeIntegranteDAO.CalculaIdadeIntegrante(HddAux.Value)
                        Case Is = "1", "2"
                            SomaAdultoLadoA += 1
                        Case Is = "3"
                            SomaCriancasColoA += 1
                    End Select
                End If
            Next

            'Contando os integrantes por idade do LadoB
            cont = 20
            For cont = 20 To 29 Step 1
                Select Case cont
                    Case 20
                        HddAux.Value = hddHosp20.Value.Replace("-", "")
                    Case 21
                        HddAux.Value = hddHosp21.Value.Replace("-", "")
                    Case 22
                        HddAux.Value = hddHosp22.Value.Replace("-", "")
                    Case 23
                        HddAux.Value = hddHosp23.Value.Replace("-", "")
                    Case 24
                        HddAux.Value = hddHosp24.Value.Replace("-", "")
                    Case 25
                        HddAux.Value = hddHosp25.Value.Replace("-", "")
                    Case 26
                        HddAux.Value = hddHosp26.Value.Replace("-", "")
                    Case 27
                        HddAux.Value = hddHosp27.Value.Replace("-", "")
                    Case 28
                        HddAux.Value = hddHosp28.Value.Replace("-", "")
                    Case 29
                        HddAux.Value = hddHosp29.Value.Replace("-", "")
                End Select


                If HddAux.Value > 0 Then
                    Select Case objCalculaIdadeIntegranteDAO.CalculaIdadeIntegrante(HddAux.Value)
                        Case Is = "1", "2"
                            SomaAdultoLadoB += 1
                        Case Is = "3"
                            SomaCriancasColoB += 1
                    End Select
                End If
            Next

            'Verificando se total de leitos comporta todos os integrantes
            objReservaListagemIntegranteVO = New ReservaListagemIntegranteVO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If
            'Efetivando a consulta e carregando os objetos
            objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.VerificaIntegranteAcomodacao(hddResId1.Value, hddSolId1.Value, 0)
            Dim QtdMaior1 As Long = objReservaListagemIntegranteVO.QtdMaior
            Dim QtdMenor1 As Long = objReservaListagemIntegranteVO.QtdMenor
            Dim MaximoPermitido1 As Long = objReservaListagemIntegranteVO.MaximoPermitido
            Dim SaidaAntecipada1 As Long = objReservaListagemIntegranteVO.SaidaAntecipada
            Dim resCaracteristica1 As String = objReservaListagemIntegranteVO.resCaracteristica
            Dim orgId1 As String = objReservaListagemIntegranteVO.orgId

            objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.VerificaIntegranteAcomodacao(hddResId2.Value, hddSolId2.Value, hddAcmId2.Value)
            Dim QtdMaior2 As Long = objReservaListagemIntegranteVO.QtdMaior
            Dim QtdMenor2 As Long = objReservaListagemIntegranteVO.QtdMenor
            Dim MaximoPermitido2 As Long = objReservaListagemIntegranteVO.MaximoPermitido
            Dim SaidaAntecipada2 As Long = objReservaListagemIntegranteVO.SaidaAntecipada
            Dim resCaracteristica2 As String = objReservaListagemIntegranteVO.resCaracteristica
            Dim orgId2 As String = objReservaListagemIntegranteVO.orgId

            If (resCaracteristica1 = "I" And orgId1 <> 37) Then  'And orgId1 <> 49)) Then 'Exceção Presidencia
                If varTroca = "Intercambio" Or varTroca = "Desmembrar" Then
                    If (SomaAdultoLadoA > (MaximoPermitido1 + SaidaAntecipada1) Or SomaAdultoLadoB > (MaximoPermitido2 + SaidaAntecipada2)) Then
                        hddDisponibilidadeTroca.Value = "N"
                        hddAcmId1.Value = 0 'Essa ação não deixará mostrar os link's para arrastar e soltar os nomes
                        Mensagem("A acomodação não possui leitos suficientes para acomodar todos os integrantes.\n\nTroca não realizada.")
                        btnLimpar_Click(sender, e)
                        Exit Try
                    End If
                End If

                If varTroca = "Permuta" Then
                    If (SomaAdultoLadoA > (MaximoPermitido2 + SaidaAntecipada2) Or SomaAdultoLadoB > (MaximoPermitido1 + SaidaAntecipada1)) Then
                        hddDisponibilidadeTroca.Value = "N"
                        hddAcmId1.Value = 0 'Essa ação não deixará mostrar os link's para arrastar e soltar os nomes
                        Mensagem("A acomodação não possui leitos suficientes para acomodar todos os integrantes.\n\nTroca não realizada.")
                        btnLimpar_Click(sender, e)
                        Exit Try
                    End If
                End If
            End If

            ''If (resCaracteristica1 = "I" And orgId1 <> 37) Then  'And orgId1 <> 49)) Then 'Exceção Presidencia
            ''    If QtdMaior1 > MaximoPermitido2 Or QtdMaior2 > MaximoPermitido1 Then
            ''        hddDisponibilidadeTroca.Value = "N"
            ''        hddAcmId1.Value = 0 'Essa ação não deixará mostrar os link's para arrastar e soltar os nomes
            ''        Mensagem("A acomodação não possui leitos suficientes para acomodar todos os integrantes.\n\nTroca não realizada.")
            ''        btnLimpar_Click(sender, e)
            ''        Exit Try
            ''    End If
            ''End If

            Dim lista As New ArrayList
            'lista = objTransferenciaDAO.confirmaTransferencia(hddSolId.Value, sender.CommandArgument, User.Identity.Name.Replace("SESC-GO.COM.BR\", ""))

            'Executando a troca
            lista = objTransferenciaDAO.executaTroca( _
              hddSolId1.Value, _
              hddSolId2.Value, _
              Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""), _
              varTroca, _
              hddResId1.Value, _
              hddResId2.Value, _
              hddAcmId1.Value, _
              hddAcmId2.Value, _
              hddAcmId1.Value, _
              hddAcmId2.Value, _
              Mid(hddApto1.Value, hddApto1.Value.IndexOf("lnk") + 4), _
              Mid(hddApto2.Value, hddApto2.Value.IndexOf("lnk") + 4), _
              hddHosp11.Value, _
              hddHosp12.Value, _
              hddHosp13.Value, _
              hddHosp14.Value, _
              hddHosp15.Value, _
              hddHosp16.Value, _
              hddHosp17.Value, _
              hddHosp18.Value, _
              hddHosp19.Value, _
              hddHosp10.Value, _
              hddHosp21.Value, _
              hddHosp22.Value, _
              hddHosp23.Value, _
              hddHosp24.Value, _
              hddHosp25.Value, _
              hddHosp26.Value, _
              hddHosp27.Value, _
              hddHosp28.Value, _
              hddHosp29.Value, _
              hddHosp20.Value)

            Dim objTransferenciaVO = lista.Item(0)
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + objTransferenciaVO.nome.ToString + "');", True)
            If objTransferenciaVO.apaStatus.ToString = "1" Then
                hddApto1_ValueChanged(sender, e)
                hddApto2_ValueChanged(sender, e)
            End If
            btnTroca.Enabled = False
            'O sistema estava duplicando alguns registro após o arrastar e soltar
            'inserimos o limpar os campos aqui para evitar esse problema em 20-11-2014(washington)
            btnLimpar_Click(sender, e)
            'Atualizando o status do apto no painel
            btnAtualizar_Click(sender, e)
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Troca não realizada." + "');", True)
        End Try
    End Sub

    Protected Sub imgBtnReservaNova_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReservaNova.Click
        Try
            hddResId.Value = "-1"
            hddIntId.Value = "" 'sender.Attributes("intId")
            hddSolId.Value = "" 'sender.Attributes("solId")
            hddHosId.Value = "" 'sender.Attributes("hosId")
            Server.Transfer("~/Reserva.aspx", True)
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub imgBtnTransferir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If sender Is btnTransferirCancelar Or sender Is gdvReserva5 Then
        Else
            hddResId.Value = sender.Attributes("resId")
            hddSolId.Value = sender.Attributes("solId")
            hddAcmId.Value = sender.Attributes("acmId")
            hddIntDataFim.Value = sender.Attributes("intDataFim")
        End If

        Dim objTransferenciaDAO As Turismo.TransferenciaDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialCaldas")
        Else
            objTransferenciaDAO = New Turismo.TransferenciaDAO("TurismoSocialPiri")
        End If
        Dim lista As New ArrayList
        lista = objTransferenciaDAO.consultarTransferencia(hddSolId.Value)
        If lista.Count > 0 Then
            gdvReserva6.DataSource = lista
            gdvReserva6.DataBind()
            gdvReserva6.SelectedIndex = -1
            gdvReserva5.Visible = False
            gdvReserva6.Visible = True
            btnTransferirCancelar.Visible = True
            btnTransferirComOnus.Visible = True
            btnTransferirSemOnus.Visible = True
        Else
            lista = objTransferenciaDAO.consultar(Format(Date.Now, "dd/MM/yyyy"), "12", hddIntDataFim.Value, "12", hddAcmId.Value)
            If lista.Count > 0 Then
                gdvReserva5.DataSource = lista
                gdvReserva5.DataBind()
                gdvReserva5.SelectedIndex = -1
                gdvReserva5.Visible = True
                gdvReserva6.Visible = False
                btnTransferirCancelar.Visible = False
                btnTransferirComOnus.Visible = False
                btnTransferirSemOnus.Visible = False
            End If
        End If
        pnlTransferencia.Visible = True
        pnlConsulta.Visible = False
    End Sub

    Protected Sub ConsultaTemporada(ByVal DataIniConsulta As String, DataFimConsulta As String)
        Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")
        Else
            objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
        End If

        'Colorindo os apartamentos flutuantes de acordo com alta e baixa temporada
        Select Case objSituacaoAtualDAO.ConsultaTemporada(DataIniConsulta, DataFimConsulta)
            Case 1 'Irá pintar de roxo
                'Wilton Honorato
                WHlnk1415.ForeColor = Drawing.Color.Purple
                WHlnk1417.ForeColor = Drawing.Color.Purple
                WHlnk1503.ForeColor = Drawing.Color.Purple
                WHlnk1505.ForeColor = Drawing.Color.Purple
                WHlnk1507.ForeColor = Drawing.Color.Purple
                WHlnk1509.ForeColor = Drawing.Color.Purple
                WHlnk1511.ForeColor = Drawing.Color.Purple
                WHlnk1513.ForeColor = Drawing.Color.Purple
                WHlnk1515.ForeColor = Drawing.Color.Purple
                WHlnk1517.ForeColor = Drawing.Color.Purple
                'Bloco Anhanguera
                ANlnk171.ForeColor = Drawing.Color.Purple
                ANlnk172.ForeColor = Drawing.Color.Purple
                ANlnk173.ForeColor = Drawing.Color.Purple
                ANlnk177.ForeColor = Drawing.Color.Purple
                ANlnk178.ForeColor = Drawing.Color.Purple
                ANlnk179.ForeColor = Drawing.Color.Purple
                ANlnk183.ForeColor = Drawing.Color.Purple
                ANlnk184.ForeColor = Drawing.Color.Purple
                ANlnk185.ForeColor = Drawing.Color.Purple
                ANlnk186.ForeColor = Drawing.Color.Purple
            Case Else
                'Wilton Honorato
                WHlnk1415.ForeColor = Drawing.Color.Black
                WHlnk1417.ForeColor = Drawing.Color.Black
                WHlnk1503.ForeColor = Drawing.Color.Black
                WHlnk1505.ForeColor = Drawing.Color.Black
                WHlnk1507.ForeColor = Drawing.Color.Black
                WHlnk1509.ForeColor = Drawing.Color.Black
                WHlnk1511.ForeColor = Drawing.Color.Black
                WHlnk1513.ForeColor = Drawing.Color.Black
                WHlnk1515.ForeColor = Drawing.Color.Black
                WHlnk1517.ForeColor = Drawing.Color.Black
                'Bloco Anhanguera
                ANlnk171.ForeColor = Drawing.Color.Black
                ANlnk172.ForeColor = Drawing.Color.Black
                ANlnk173.ForeColor = Drawing.Color.Black
                ANlnk177.ForeColor = Drawing.Color.Black
                ANlnk178.ForeColor = Drawing.Color.Black
                ANlnk179.ForeColor = Drawing.Color.Black
                ANlnk183.ForeColor = Drawing.Color.Black
                ANlnk184.ForeColor = Drawing.Color.Black
                ANlnk185.ForeColor = Drawing.Color.Black
                ANlnk186.ForeColor = Drawing.Color.Black
        End Select
    End Sub


    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPdf.Click
        'se o grid tiver mais que 65536  linhas não podemos exportar
        'If gdvQMCapacitacao.Rows.Count.ToString + 1 < 65536 Then

        'dgv.AllowPaging = "False"
        'dgv.DataBind()
        Dim Tipo As String = ""
        Dim DataRelatorio As Date
        If ckbEntrada.Checked = True Then
            Tipo = "Entrada"
        ElseIf ckbSaida.Checked = True Then
            Tipo = "Saida"
        ElseIf ckbEstada.Checked = True Then
            Tipo = "Estada"
        ElseIf ckbTransferencia.Checked = True Then
            Tipo = "Transferencia"
        Else
            Tipo = "Geral"
        End If

        If txtConsulta.Text.Length > 0 Then
            If IsDate(txtConsulta.Text) Then
                DataRelatorio = Format(CDate(txtConsulta.Text), "dd/MM/yyyy")
            End If
        Else
            DataRelatorio = Format(Date.Today, "dd/MM/yyyy")
        End If

        Dim tw As New System.IO.StringWriter()
        tw.WriteLine("SERVICO SOCIAL DO COMERCIO<BR>")
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            tw.WriteLine("SESC DE CALDAS NOVAS - Relatorio de " & Tipo & " ")
        Else
            tw.WriteLine("POUSADA SESC PIRENOPOLIS - Relatorio de " & Tipo & " ")
        End If

        tw.WriteLine("<br>Referente ao dia " & DataRelatorio & " ")
        Dim hw As New System.Web.UI.HtmlTextWriter(tw)
        Dim frm As HtmlForm = New HtmlForm()

        Response.ContentType = "application/vnd.ms-word"
        Response.AddHeader("content-disposition", "attachment;filename=Recepcao.Doc")
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Charset = ""
        EnableViewState = False

        If gdvReserva1.HeaderRow.RowType = DataControlRowType.Header Then
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrAcao"), LinkButton).ForeColor = Drawing.Color.Black
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrAcao"), LinkButton).Text = "Acao"
            CType(gdvReserva1.HeaderRow.FindControl("imgAcao"), ImageButton).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgIntegranteInfo"), Image).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), Image).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgReservaInfo"), Image).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).Visible = False
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrServidor"), LinkButton).Text = "Servidor"
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrServidor"), LinkButton).ForeColor = Drawing.Color.Black
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrIntegrante"), LinkButton).Text = "Integrante"
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrIntegrante"), LinkButton).ForeColor = Drawing.Color.Black
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrPeriodo"), LinkButton).Text = "Periodo"
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrPeriodo"), LinkButton).ForeColor = Drawing.Color.Black
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrResponsavel"), LinkButton).Text = "Responsavel"
            CType(gdvReserva1.HeaderRow.FindControl("lnkhdrResponsavel"), LinkButton).ForeColor = Drawing.Color.Black
        End If

        For Each linha As GridViewRow In gdvReserva1.Rows
            CType(linha.FindControl("imgBtnApto"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnChave"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnTransferir"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnPermutar"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnEmprestimo"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnBloquearCartao"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnAcomodacao"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnReserva"), ImageButton).Visible = False
            CType(linha.FindControl("imgBtnCortesia"), ImageButton).Visible = False
            CType(linha.FindControl("lblApto"), Label).ForeColor = Drawing.Color.Black

            CType(linha.FindControl("imgBtnIntegrante"), ImageButton).Visible = False
            CType(linha.FindControl("imgPlaca"), Image).Visible = False
            CType(linha.FindControl("lnkNome"), LinkButton).Text = RemoveAcentos(CType(linha.FindControl("lnkNome"), LinkButton).Text)
            CType(linha.FindControl("lnkNome"), LinkButton).ForeColor = Drawing.Color.Black
            CType(linha.FindControl("lnkNome"), LinkButton).Font.Overline = False
            CType(linha.FindControl("lnkNome"), LinkButton).Font.Bold = False
            CType(linha.FindControl("lnkResponsavel"), LinkButton).Text = RemoveAcentos(CType(linha.FindControl("lnkResponsavel"), LinkButton).Text)
            CType(linha.FindControl("lnkResponsavel"), LinkButton).ForeColor = Drawing.Color.Black
            CType(linha.FindControl("lnkResponsavel"), LinkButton).Font.Overline = False
            CType(linha.FindControl("lnkResponsavel"), LinkButton).Font.Bold = False
        Next

        gdvReserva1.Font.Size = 8
        Controls.Add(frm)
        If gdvReserva1.Visible Then
            frm.Controls.Add(gdvReserva1)
        End If

        frm.RenderControl(hw)
        Response.Write(tw.ToString())
        Response.End()

        'dgv.AllowPaging = "True"
        'dgv.DataBind()

        'Else
        'LblError.Text = " planilha possui muitas linhas, não é possível exportar para o EXcel"
        'End If
    End Sub
    Public Function RemoveAcentos(ByVal texto As String) As String
        Dim charFrom As String = "ŠŒŽšœžŸ¥µÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿ"
        Dim charTo As String = "SOZsozYYuAAAAAAACEEEEIIIIDNOOOOOOUUUUYsaaaaaaaceeeeiiiionoooooouuuuyy"
        For i As Integer = 0 To charFrom.Length - 1
            texto = Replace(texto, charFrom(i), charTo(i))
        Next
        Return texto
    End Function

    Protected Sub imgSalvarEnxoval_Click(sender As Object, e As ImageClickEventArgs)

        Dim Objeto As ImageButton = sender
        Dim row As GridViewRow = Objeto.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim Linha = row.RowIndex
        'Esse processo não deixará enviar uma soliciação em branco ao setor de governança
        If ((CType(gdvReserva1.Rows.Item(Linha).FindControl("rdCamaExtra"), CheckBoxList).SelectedValue.ToString.Trim.Length = "0") And _
             (CType(gdvReserva1.Rows.Item(Linha).FindControl("chkComplemento"), CheckBoxList).Items.Item(0).Selected = False) And _
             (CType(gdvReserva1.Rows.Item(Linha).FindControl("chkComplemento"), CheckBoxList).Items.Item(1).Selected = False) And _
             (CType(gdvReserva1.Rows.Item(Linha).FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = "Atendimento não solicitado")) Then
            Mensagem("Selecione um dos intens à completar antes de salvar.")
            Return
        End If

        'Se for bloco bambuí irá checar se a opção de cama 4 e 5 estarão selecionados, se estiver dará um alerta.
        If gdvReserva1.DataKeys(row.RowIndex).Item("BloId").ToString = 2 Then
            If (CType(gdvReserva1.Rows.Item(Linha).FindControl("rdCamaExtra"), CheckBoxList).Items.Item(0).Selected = True) And _
                (CType(gdvReserva1.Rows.Item(Linha).FindControl("rdCamaExtra"), CheckBoxList).Items.Item(1).Selected = True) Then
                Mensagem("Quantide de camas a completar inválida! Informe apenas uma das opções.")
                Return
            End If
        End If


        'Carrega o Objeto para inserção ou update
        Dim objCheckInOutDAO As Turismo.CheckInOutDAO
        Dim objCheckInOutVO As New Turismo.CheckInOutVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialCaldas")
        Else
            objCheckInOutDAO = New Turismo.CheckInOutDAO("TurismoSocialPiri")
        End If
        With objCheckInOutVO
            .resId = gdvReserva1.DataKeys(row.RowIndex()).Item("resId").ToString
            .solId = gdvReserva1.DataKeys(row.RowIndex()).Item("solId").ToString
            If CType(gdvReserva1.Rows.Item(Linha).FindControl("rdCamaExtra"), CheckBoxList).SelectedValue.ToString = "0" Then
                .enxCamaExtra = ""
            Else
                .enxCamaExtra = CType(gdvReserva1.Rows.Item(Linha).FindControl("rdCamaExtra"), CheckBoxList).SelectedValue.ToString
            End If

            If CType(gdvReserva1.Rows.Item(Linha).FindControl("chkComplemento"), CheckBoxList).Items.Item(0).Selected = True Then
                .enxBerco = "S"
            Else
                'Nesse caso havia uma solicitação e foi retirada
                If ((gdvReserva1.DataKeys(row.RowIndex()).Item("enxBerco").ToString = "S" Or gdvReserva1.DataKeys(row.RowIndex()).Item("enxBerco").ToString = "R") And gdvReserva1.DataKeys(row.RowIndex()).Item("enxBercoAtendido").ToString = "S") Then
                    .enxBerco = "R"
                Else
                    .enxBerco = "N"
                End If
            End If

            If CType(gdvReserva1.Rows.Item(Linha).FindControl("chkComplemento"), CheckBoxList).Items.Item(1).Selected = True Then
                .enxBanheira = "S"
            Else
                'Nesse caso havia uma solicitação e foi retirada
                If ((gdvReserva1.DataKeys(row.RowIndex()).Item("enxBanheira").ToString = "S" Or gdvReserva1.DataKeys(row.RowIndex()).Item("enxBanheira").ToString = "R") And gdvReserva1.DataKeys(row.RowIndex()).Item("enxBanheiraAtendida").ToString = "S") Then
                    .enxBanheira = "R"
                Else
                    .enxBanheira = "N"
                End If
            End If
            .enxUsuarioSolicitante = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
        End With
        'Quando a passagem de parametro for R quer dizer recepção, então irá fazer update na parte da governança
        Select Case objCheckInOutDAO.InserirCompetarEnxoval(objCheckInOutVO, "R")
            Case Is = 0
                Mensagem("Erro ao enviar a solicitação! Tente novamente.")
            Case Is = 1, 2
                CType(gdvReserva1.Rows.Item(Linha).FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = "Aguardando atendimento."
                CType(gdvReserva1.Rows.Item(Linha).FindControl("imgSalvarEnxoval"), ImageButton).ImageUrl = "~/images/AtendidoEnxoval.png"
                Mensagem("Solicitação enviada com sucesso!")
            Case Is = 3
                CType(gdvReserva1.Rows.Item(Linha).FindControl("imgSalvarEnxoval"), ImageButton).ToolTip = "Atendimento não solicitado."
                CType(gdvReserva1.Rows.Item(Linha).FindControl("imgSalvarEnxoval"), ImageButton).ImageUrl = "~/images/AtendidoEnxoval.png"
                Mensagem("Solicitação apagada com sucesso!")
            Case Is = 4
                Mensagem("Não foi possível apagar a solicitação, pois já existe um atendimento para ela.")
        End Select

        CType(gdvReserva1.Rows.Item(Linha).FindControl("chkComplemento"), CheckBoxList).Focus()
        'Dim Linha = gdvReserva1.DataKeys(row.RowIndex()).Item("intPasId").ToString
        btnConsulta_Click(sender, e)
    End Sub
    Protected Sub Mensagem(texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + texto + "');", True)
    End Sub
    Protected Sub ValidaTotaldeIntegrantesPorAcomodacao(resIdLeitos As Integer, solIdLeitos As Integer)
        'Verifica se o número máximo de leitos por acomodação já foi completado (N - Negar inserção e L-Liberar Inserção)
        'If (hddResCaracteristica.Value = "I" And cmbOrgId.SelectedValue <> "37") Then 'Passeio e Grupos não entram na questão - excessão: Presidencia
        'If (hddResCaracteristica.Value = "I") Then 'Passeio e Grupos não entram na questão - excessão: Presidencia
        'If (hddResCaracteristica.Value = "I") Then
        btnConsulta.Attributes.Add("ResultadoAdulto", "")
        btnConsulta.Attributes.Add("ResultadoCrianca", "")
        objReservaListagemIntegranteVO = New ReservaListagemIntegranteVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
        End If
        'Efetivando a consulta e carregando os objetos
        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.VerificaIntegranteAcomodacao(resIdLeitos, solIdLeitos, 0)
        'Esta sendo verificado mais ou menos na linha 461
    End Sub

    Protected Sub tmrAlertaReserva_Tick(sender As Object, e As EventArgs) Handles tmrAlertaReserva.Tick
        If Now.Hour > 21 And Now.Hour < 24 Then
            If Now.Minute < 59 Then
                'Aqui irei chamar a consulta, abaixo é a select
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objCalculaIdadeIntegranteDAO = New CalculaIdadeIntegranteDAO("TurismoSocialCaldas")
                Else
                    objCalculaIdadeIntegranteDAO = New CalculaIdadeIntegranteDAO("TurismoSocialPiri")
                End If
                'objCalculaIdadeIntegranteVO = New CalculaIdadeIntegranteVO
                Dim Lista As New ArrayList
                Lista = objCalculaIdadeIntegranteDAO.LocalizaCheckInSemEntregaChave()

                If Lista.Count = 0 Then
                    Return
                End If

                Dim ListaPendencias As String = ""
                ListaPendencias = "LISTA DE INTEGRANTES COM PENDÊNCIA NA ENTREGA DE CHAVES\n\n"
                ListaPendencias = ListaPendencias & "APARTAMENTO/NOME\n"
                ListaPendencias = ListaPendencias & "".PadRight(47, "=") & "\n"
                For Each Item As CalculaIdadeIntegranteVO In Lista
                    ListaPendencias = ListaPendencias & Item.ApaDesc & " - " & Item.IntNome.ToString.ToUpper.Trim & "\n"
                Next
                Mensagem(ListaPendencias)
            End If
        End If
    End Sub
    
End Class

