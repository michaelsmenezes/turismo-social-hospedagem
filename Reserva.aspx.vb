Imports Microsoft.Reporting.WebForms
Imports System.Net.Mail
Imports dtsRelacaoIntegrantesTableAdapters
Imports System.DirectoryServices
Imports System.Text.RegularExpressions
'Geração do boletoNet
Imports System, System.IO, System.Net, System.Text, System.Web.Script.Serialization, System.Web.UI.HtmlControls
Imports Turismo
Imports ModelosBoletos
Imports Uteis
Imports BoletoNet

Partial Class Reserva
    Inherits System.Web.UI.Page
    Dim Instance As HtmlTable
    Dim objTestaGrupo As New Uteis.TestaUsuario
    Dim Grupos As String = objTestaGrupo.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
    Dim objAcomodacaoDAO As Turismo.AcomodacaoDAO
    Dim objAcomodacaoVO As Turismo.AcomodacaoVO
    Dim objCategoriaDAO As Turismo.CategoriaDAO
    Dim objCategoriaVO As Turismo.CategoriaVO
    Dim objFormaPagtoDAO As Turismo.FormaPagtoDAO
    Dim objFormaPagtoVO As Turismo.FormaPagtoVO
    Dim objOrgaoDAO As Turismo.OrgaoDAO
    Dim objOrgaoVO As Turismo.OrgaoVO
    Dim objCaracteristicaDAO As Turismo.CaracteristicaDAO
    Dim objCaracteristicaVO As Turismo.CaracteristicaVO
    Dim objReservaListagemSolicitacaoDAO As Turismo.ReservaListagemSolicitacaoDAO
    Dim objReservaListagemSolicitacaoVO As New Turismo.ReservaListagemSolicitacaoVO
    Dim objReservaListagemAcomodacaoDAO As Turismo.ReservaListagemAcomodacaoDAO
    Dim objReservaListagemAcomodacaoVO As Turismo.ReservaListagemAcomodacaoVO
    Dim objReservaListagemIntegranteDAO As Turismo.ReservaListagemIntegranteDAO
    Dim objReservaListagemIntegranteVO As New Turismo.ReservaListagemIntegranteVO
    Dim objReservaListagemFinanceiroDAO As Turismo.ReservaListagemFinanceiroDAO
    Dim objReservaListagemFinanceiroVO As Turismo.ReservaListagemFinanceiroVO
    Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
    Dim objSituacaoAtualVO As Turismo.SituacaoAtualVO
    Dim objPlanilhaItemDAO As Turismo.PlanilhaItemDAO
    Dim objPlanilhaItemVO As New Turismo.PlanilhaItemVO

    Dim ObjReservaVO As New Turismo.ReservaVO
    Dim ObjReservaDAO As New Turismo.ReservaDAO("")
    'Usado para ver se a acomodação esta ocupada ou não (Controle de - ou + Adiconar ou diminuir periodo) e impressão do comprovante de de reserva
    Dim ObjReservaConsultasDAO As ReservaConsultasDAO
    Dim ObjReservaConsultasVO As ReservaConsultasVO

    Dim objPlanilhaCustoDAO As Turismo.PlanilhaCustoDAO
    Dim objPlanilhaCustoVO As New Turismo.PlanilhaCustoVO
    Dim objPlanilhaCustoItemDAO As Turismo.PlanilhaCustoItemDAO
    Dim objPlanilhaCustoItemVO As New Turismo.PlanilhaCustoItemVO

    Dim objDefaultDAO As Turismo.DefaultDAO
    Dim objDefaultVO As Turismo.DefaultVO
    Dim objParametroDAO As Turismo.ParametroDAO
    Dim objParametroVO As Turismo.ParametroVO
    Dim objDisponibilidadeDAO As Turismo.DisponibilidadeDAO
    Dim objDisponibilidadeVO As New Turismo.DisponibilidadeVO
    Dim objEstadoDAO As Geral.BdProdEstadoDAO
    Dim objEstadoVO As Geral.BdProdEstadoVO
    Dim objMunicipioDAO As Geral.BdProdMunicipioDAO
    Dim objMunicipioVO As Geral.BdProdMunicipioVO
    Dim objUsuarioGrupoDAO As Turismo.UsuarioGrupoDAO
    Dim objUsuarioGrupoVO As Turismo.UsuarioGrupoVO
    Dim objPasseioDestinoDAO As Turismo.PasseioDestinoDAO
    Dim objPasseioDestinoVO As Turismo.PasseioDestinoVO
    Dim objRefeicaoPratoDAO As Turismo.RefeicaoPratoDAO
    Dim objRefeicaoPratoVO As Turismo.RefeicaoPratoVO
    Dim objClienteDAO As CentralAtendimento.ClienteDAO
    Dim objClienteVO As CentralAtendimento.ClienteVO
    Dim objUopDAO As Turismo.UopDAO
    Dim objUopVO As Turismo.UopVO
    Dim objBoletoDAO As Turismo.BoletoDAO
    Dim objBoletoVO As Turismo.BoletoVO
    'Geração dos boletos no novo padrão da caixa federal
    Dim objBoletoNetDAO As BoletoNetDAO
    Dim objBoletoNetVO As BoletoNetVO
    Dim objBoleto1NetVO As BoletoNetVO
    Dim objBoleto2NetVO As BoletoNetVO
    Dim objNotificacaoTemplateVO As NotificacaoTemplateVO
    Dim objNotificacaoTemplateDAO As NotificacaoTemplateDAO
    'Dim objModeloADAO As SGF.ModeloaDAO
    'Dim objModeloAVO As SGF.ModeloaVO
    Dim lista As New ArrayList
    Dim listaEstadoAuxiliar As New ArrayList
    Dim varBoleto As Boolean = False
    Dim varBoleto50 As Boolean = False
    Dim varPgtoBoleto As Boolean = False
    Dim DeixaVerSelecaoTotalBoleto As Boolean = False
    Dim VerBoletoAVista As Boolean = False, ExisteBoletoGerado As Integer = 0
    Private totHospede As Short
    Private totApto As Short
    Private totalValor As Decimal
    Private totalPago As Decimal
    Private totDiaria As Decimal
    Private cont As Integer

    'Enviando pagamento para o caixa - Classe local
    Dim objEnviaPagamentoCaixaVO As EnviaPagamentoCaixaVO
    Dim ObjEnviaPagamentoCaixaDAO As EnviaPagamentoCaixaDAO
    'Base de consulta do Hospede já
    Dim ObjConsultasHospedeJaVO As ConsultasHospedeJaVO
    Dim ObjConsultasHospedeJaDAO As ConsultasHospedeJaDAO
    '
    Protected Sub enviarEmail()
        Try
            Dim objEmail As New System.Net.Mail.MailMessage()
            objEmail.Subject = "SESC Goiás - Turismo Social"
            objEmail.To.Add(New System.Net.Mail.MailAddress(txtResEmail.Text))
            'objEmail.To.Add(New MailAddress(objUsuarioRedeVO.mail))
            objEmail.IsBodyHtml = True

            Dim objSmtp As SmtpClient

            '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
            'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
            objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
            objSmtp.EnableSsl = True
            objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
            objSmtp.UseDefaultCredentials = False
            objSmtp.Timeout = 5000

            objEmail.From = New System.Net.Mail.MailAddress("reservas@sescgo.com.br")
            'objEmail.Bcc.Add("elvis.irineu@sescgo.com.br")
            'objEmail.Bcc.Add("gustavo.cesar@sescgo.com.br")

            Dim sEmail As New StringBuilder
            If cmbResSexo.Text = "M" Then
                sEmail.Append("Olá Sr. " & txtResNome.Text)
            Else
                sEmail.Append("Olá Sra. " & txtResNome.Text)
            End If
            sEmail.Append("<p />Obrigado por utilizar o Sistema Online de Turismo Social do SESC Goiás.")
            sEmail.Append("<p />Acesse o link <a href=" & "http://turismosocialweb.sescgo.com.br" & ">Turismo Social Web<a> e informe seus dados de acesso.")
            sEmail.Append("<p />Depois de acessar o sistema, confirme o recebimento do seu pedido para liberar o acesso as acomodações e a lista de hóspedes.")
            sEmail.Append("<p /><b>Fique atento à lixeira de seu e-mail. Algumas respostas automáticas podem ser direcionadas a ela.</b>")
            sEmail.Append("<p />À oportunidade agradecemos a preferência.")
            sEmail.Append("<p />Atenciosamente")
            sEmail.Append("<p />SESC Turismo Social")
            sEmail.Append("<p />Não é preciso responder este email.")
            sEmail.Append("<p />Goiânia, " & Format(Now, "HH:mm") & " horas do dia " & Now.Day.ToString & " de " & MonthName(Now.Month) & " de " & Now.Year.ToString)
            sEmail.Append("<p />Esta mensagem pode conter informações confidenciais e/ou privilegiadas.")
            sEmail.Append("<br />Se você não for o destinatário ou a pessoa autorizada a recebê-la, não pode usar, copiar ou divulgar as informações nela contidas ou tomar qualquer ação baseada nelas.")
            sEmail.Append("<br />Se você recebeu esta mensagem por engano, por favor, avise imediatamente o remetente, e em seguida, apague-a.")
            sEmail.Append("<br />Comunicações pela Internet não podem ser garantidas quanto à segurança ou inexistência de erros ou de vírus. O remetente, por esta razão, não aceita responsabilidade por qualquer erro ou omissão no contexto da mensagem decorrente da transmissão via Internet.")
            objEmail.IsBodyHtml = True
            objEmail.Body = sEmail.ToString
            objEmail.Priority = MailPriority.Normal

            objSmtp.Send(objEmail)
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação&excecao=" & ex.StackTrace.ToString & "&acao=SolicitacaoWeb.aspx.vb:btnEnviarSol() - " & Request.Browser.Browser.ToString)
        End Try
    End Sub

    Public Sub Desabilitar(ByVal controlP As Control)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Enabled = False
            ElseIf TypeOf ctl Is DropDownList Then
                DirectCast(ctl, DropDownList).Enabled = False
            ElseIf TypeOf ctl Is CheckBox Then
                DirectCast(ctl, CheckBox).Enabled = False
            ElseIf TypeOf ctl Is CheckBoxList Then
                DirectCast(ctl, CheckBoxList).Enabled = False
            ElseIf ctl.Controls.Count > 0 Then
                Desabilitar(ctl)
            End If
        Next
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

    Public Sub Limpar(ByVal controlP As Control)
        Dim ctl As Control
        For Each ctl In controlP.Controls
            If TypeOf ctl Is TextBox Then
                DirectCast(ctl, TextBox).Text = String.Empty
            ElseIf ctl.Controls.Count > 0 Then
                Limpar(ctl)
            End If
        Next
        Habilitar(pnlResponsavelAcao)
    End Sub

    Protected Sub ProcuraDisponibilidadeSolicitadaeAlternativa()
        If CDate(txtDataInicialSolicitacao.Text) < Now.Date And hddResId.Value = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Data Inicial é menor que a data de hoje." + "');", True)
            txtDataInicialSolicitacao.Focus()
        ElseIf CDate(txtDataInicialSolicitacao.Text) > CDate(txtDataFinalSolicitacao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Data Final menor que que Data Inicial." + "');", True)
            txtDataFinalSolicitacao.Focus()
        Else
            Try
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialCaldas")
                    btnHospedagemNova.Attributes.Add("AliasBanco", "TurismoSocialCaldas")
                Else
                    objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialPiri")
                    btnHospedagemNova.Attributes.Add("AliasBanco", "TurismoSocialPiri")
                End If
                With gdvReserva6
                    If txtDataInicialSolicitacao.Text > "" Then
                        If hddOrgGrupo.Value = "F" Then
                            lista = objDisponibilidadeDAO.consultarDisponibilidadeGP(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                        ElseIf (Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or
                        (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Session("GrupoRecepcaoPiri")) Then

                            If btnHospedagemNova.Attributes.Item("UsuariosCA") = "S" Then 'Irá mostrar os normais e flutuantes em baixa temporada para o grupo da central de atendimento
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "True", "False")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "True", "False")
                                End If
                            ElseIf cmbHospedagem.SelectedValue = "0" Then 'Todos
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                                End If
                            ElseIf cmbHospedagem.SelectedValue = "N" Then 'Normais - SEREC
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "False")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "False")
                                End If
                            ElseIf cmbHospedagem.SelectedValue = "E" Then 'Especiais 
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "False")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "False")
                                End If

                            ElseIf cmbHospedagem.SelectedValue = "D" Then 'Reserva Técnica
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "True")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "True")
                                End If

                            ElseIf cmbHospedagem.SelectedValue = "R" Then 'Reserva Técnica Manutenção
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "True")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "True", "False", "True")
                                End If
                            ElseIf cmbHospedagem.SelectedValue = "S" Then 'Fecomércio/Presidência
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeGP(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                End If
                            ElseIf cmbHospedagem.SelectedValue = "F" Then 'Flutuantes
                                If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                Else
                                    lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                End If
                                'ElseIf cmbHospedagem.SelectedValue = "S" Then 'Era Presidencia - Agora Fecomercio
                                '    'If CDate(txtDataInicialSolicitacao.Text) = Now.Date Then
                                '    '    lista = objDisponibilidadeDAO.consultarDisponibilidadeRecepcao(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                '    'Else
                                '    lista = objDisponibilidadeDAO.consultarDisponibilidadeGP(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), "False", "True", "False")
                                '    'End If
                            End If
                        Else
                            lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                        End If

                        'Ira ler a lista e pegar apenas os aptos que se enquadra na consulta com base no cmbHospedagem
                        'Se for todos irá ignorar essa ação.
                        Dim DataIniConsulta As String, DataFimConsulta As String
                        If txtDataInicialSolicitacao.Text.Trim.Length = 0 Then
                            DataIniConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                        Else
                            DataIniConsulta = (Format(CDate(txtDataInicialSolicitacao.Text), "dd-MM-yyyy"))
                        End If
                        If txtDataFinalSolicitacao.Text.Trim.Length = 0 Then
                            DataFimConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                        Else
                            DataFimConsulta = (Format(CDate(txtDataFinalSolicitacao.Text), "dd-MM-yyyy"))
                        End If

                        objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO(btnHospedagemNova.Attributes.Item("AliasBanco"))
                        Dim SimboloFlutuante As String = ""
                        If btnHospedagemNova.Attributes.Item("AliasBanco") = "TurismoSocialCaldas" Then
                            SimboloFlutuante = "S" 'Caldas Novas
                        Else
                            SimboloFlutuante = "D" 'Pirenopolis'
                        End If
                        If cmbHospedagem.SelectedIndex > 0 Then
                            Dim ListaAuxiliar As New ArrayList
                            For Each item As Turismo.DisponibilidadeVO In lista
                                '1 = Alta Temporada os flutuantes irão aparecer para o fecomércio, 3 - Calendário Federação
                                Select Case objSituacaoAtualDAO.ConsultaTemporada(DataIniConsulta, DataFimConsulta)
                                    'Case Is = 1, 3
                                    Case Is = 3
                                        'Só irá mostrar flutuantes para o fecomércio
                                        If cmbHospedagem.SelectedValue.Trim = SimboloFlutuante Then
                                            ListaAuxiliar.Add(item)
                                        Else
                                            'Obedece o filtro com tem que ser
                                            If item.acmFederacao.Trim = cmbHospedagem.SelectedValue.Trim Or item.acmFederacao.Trim = "F" Then
                                                ListaAuxiliar.Add(item)
                                            End If
                                        End If
                                    Case Is = 0, 1
                                        'Se não for alta Temporada/Federação irá mostrar os flutuantes para Centrais de Atendimentos e recepções com baixa temporada em preto
                                        If item.acmFederacao.Trim = cmbHospedagem.SelectedValue.Trim Or item.acmFederacao.Trim = "F" Then
                                            ListaAuxiliar.Add(item)
                                        End If
                                        'Case Else
                                        '    'Se não for alta temporada obedece o filtro, não irá aparecer flutuantes para o fecomércio
                                        '    If item.acmFederacao.Trim = cmbHospedagem.SelectedValue.Trim Or item.acmFederacao.Trim = "F" Then
                                        '        ListaAuxiliar.Add(item)
                                        '    End If
                                End Select
                            Next
                            lista.Clear()
                            lista = ListaAuxiliar
                        End If
                        .DataSource = lista
                        .DataBind()
                        .SelectedIndex = -1
                    End If
                End With
                If hddOrgGrupo.Value = "F" Then
                    lista = objDisponibilidadeDAO.consultarDisponibilidadeAlternativaGP(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                Else
                    lista = objDisponibilidadeDAO.consultarDisponibilidadeAlternativa(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
                End If
                pnlSolicitacaoSelecionada.Visible = True
                gdvReserva7.DataSource = lista
                gdvReserva7.DataBind()
                gdvReserva7.SelectedIndex = -1
                pnlAcomodacaoTitulo.Visible = True
                pnlDisponibilidadeAlternativaAux.Visible = True
            Catch ex As Exception
                lista = Nothing
            End Try
        End If
    End Sub

    Protected Sub ProcuraDisponibilidade()
        Try
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialCaldas")
            Else
                objDisponibilidadeDAO = New Turismo.DisponibilidadeDAO("TurismoSocialPiri")
            End If
            lista = objDisponibilidadeDAO.consultarDisponibilidade(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy"), Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
            gdvReserva6.DataSource = lista
            gdvReserva6.DataBind()
            gdvReserva6.SelectedIndex = -1
            gdvReserva7.DataSource = Nothing
            gdvReserva7.DataBind()
            gdvReserva7.SelectedIndex = -1
            pnlAcomodacaoTitulo.Visible = True
            pnlSolicitacaoSelecionada.Visible = True
            pnlDisponibilidadeAlternativaAux.Visible = False
        Catch ex As Exception

        End Try
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
            cmbEstadoAux.Items.Clear()
            cmbEstadoAux.DataTextField = "estDescricao"
            cmbEstadoAux.DataValueField = "estId"
            cmbEstadoAux.DataSource = objEstadoDAO.consultarEstadoPais
            cmbEstadoAux.DataBind()

            txtDataInicialSolicitacao.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy").Substring(0, 10)
            hddResDataIni.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "dd/MM/yyyy").Substring(0, 10)
            hddResDataFim.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy").Substring(0, 10)
            'If txtDataFinalSolicitacao.Text.Trim.Replace("/", "").Length = 0 Or _
            '  CDate(txtDataFinalSolicitacao.Text) <= CDate(txtDataInicialSolicitacao.Text) Then
            txtDataFinalSolicitacao.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim), "dd/MM/yyyy").Substring(0, 10)
            'End If
            hddResTipo.Value = objReservaListagemSolicitacaoVO.resTipo
            hddIntervaloSolicitacao.Value = DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), CDate(txtDataFinalSolicitacao.Text))
            txtResNome.Text = objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "")
            If objReservaListagemSolicitacaoVO.resCaracteristica = "I" Then
                pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Hospedagem de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "") & " (" & hddResId.Value & ")"
                pnlResponsavelTitulo.ToolTip = "Reserva " & objReservaListagemSolicitacaoVO.resId
            ElseIf objReservaListagemSolicitacaoVO.resDataIni = objReservaListagemSolicitacaoVO.resDataFim Then
                pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Passeio de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "") & " (" & hddResId.Value & ")"
                pnlResponsavelTitulo.ToolTip = "Reserva " & objReservaListagemSolicitacaoVO.resId
            Else
                pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Excursão de " & objReservaListagemSolicitacaoVO.resNome.Trim.Replace("'", "") & " (" & hddResId.Value & ")"
                pnlResponsavelTitulo.ToolTip = "Reserva " & objReservaListagemSolicitacaoVO.resId
            End If

            pnlResponsavelTitulo_CollapsiblePanelExtender.ExpandedText = "Responsável (" & hddResId.Value & ")"

            If objReservaListagemSolicitacaoVO.resCaracteristica = "E" Or objReservaListagemSolicitacaoVO.resCaracteristica = "T" And
                (objReservaListagemSolicitacaoVO.orgId <> "6" And
                objReservaListagemSolicitacaoVO.orgId <> "3" And
                objReservaListagemSolicitacaoVO.orgId <> "4") Then
                cmbOrgId.SelectedValue = "S"
            Else
                cmbOrgId.SelectedValue = objReservaListagemSolicitacaoVO.orgId
            End If

            hddResCaracteristica.Value = objReservaListagemSolicitacaoVO.resCaracteristica
            txtResDtLimiteRetorno.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtLimiteRetorno), "dd/MM/yyyy")
            Try
                cmbResHrLimiteRetorno.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtLimiteRetorno).Hour, "00")
            Catch ex As Exception
                cmbResHrLimiteRetorno.Text = "00"
            End Try
            'Formatando a matricula
            Dim Mat = objReservaListagemSolicitacaoVO.resMatricula.Trim.Replace("-", "").Replace("/", "").Replace(".", "").Replace("\", "").Replace("_", "").Replace(" ", "")
            txtResMatricula.Text = Mid(Mat, 1, 4) & " " & Mid(Mat, 5, 6) & " " & Mid(Mat, 11, 1)
            'txtResMatricula.Text = objReservaListagemSolicitacaoVO.resMatricula.Trim.Replace("-", "").Replace("/", "").Replace(".", "").Replace("\", "").Replace("_", "")

            'Formatando o cpf
            Dim cpf = objReservaListagemSolicitacaoVO.resCPF_CGC.Trim.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "")
            If cpf.Length = 11 Then 'CPF
                txtResCPF.Text = Mid(cpf, 1, 3) & "." & Mid(cpf, 4, 3) & "." & Mid(cpf, 7, 3) & "-" & Mid(cpf, 10, 2)
            Else 'CNPJ
                txtResCPF.Text = Mid(cpf, 1, 2) & "." & Mid(cpf, 3, 3) & "." & Mid(cpf, 6, 3) & "/" & Mid(cpf, 9, 4) & "-" & Mid(cpf, 13, 2)
            End If
            'txtResCPF.Text = objReservaListagemSolicitacaoVO.resCPF_CGC.Trim.Replace("'", "")

            txtResContato.Text = objReservaListagemSolicitacaoVO.resContato.Trim
            txtResEmail.Text = Mid(objReservaListagemSolicitacaoVO.resEmail.Trim, 1, 40)
            txtResDtNascimento.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtNascimento), "dd/MM/yyyy")

            cmbResSexo.Text = objReservaListagemSolicitacaoVO.resSexo
            Try
                cmbResCatId.Text = objReservaListagemSolicitacaoVO.catId
            Catch ex As Exception
                cmbResCatId.Text = "1"
            End Try
            hddResStatus.Value = objReservaListagemSolicitacaoVO.resStatus
            txtResFoneComercial.Text = objReservaListagemSolicitacaoVO.resFoneComercial
            txtResFoneResidencial.Text = objReservaListagemSolicitacaoVO.resFoneResidencial
            txtResCelular.Text = objReservaListagemSolicitacaoVO.resCelular
            txtResFax.Text = objReservaListagemSolicitacaoVO.resFax
            Dim sender As Object = Nothing
            Dim e As System.EventArgs = Nothing
            'If cmbEstId.Text <> objReservaListagemSolicitacaoVO.estId Then
            cmbEstId.SelectedValue = objReservaListagemSolicitacaoVO.estId
            cmbEstId_SelectedIndexChanged(sender, e)
            'End If
            Try
                cmbResCidade.Text = Mid(objReservaListagemSolicitacaoVO.resCidade.ToUpper, 1, 40)
                cmbResCidade.Visible = True
                txtResCidade.Visible = False
            Catch ex As Exception
                cmbResCidade.Visible = False
                txtResCidade.Visible = True
            End Try
            txtResCidade.Text = Mid(objReservaListagemSolicitacaoVO.resCidade.Trim, 1, 40).Trim.Replace("'", "")
            txtResLogradouro.Text = Mid(objReservaListagemSolicitacaoVO.resLogradouro.Trim, 1, 40).Trim.Replace("'", "")
            txtResNumero.Text = Mid(objReservaListagemSolicitacaoVO.resNumero.Trim, 1, 10).Trim.Replace("'", "")
            txtResQuadra.Text = Mid(objReservaListagemSolicitacaoVO.resQuadra.Trim, 1, 10).Trim.Replace("'", "")
            txtResLote.Text = Mid(objReservaListagemSolicitacaoVO.resLote.Trim, 1, 10).Trim.Replace("'", "")
            txtResComplemento.Text = Mid(objReservaListagemSolicitacaoVO.resComplemento.Trim, 1, 40).Trim.Replace("'", "")
            txtResBairro.Text = Mid(objReservaListagemSolicitacaoVO.resBairro.Trim, 1, 40).Trim.Replace("'", "")
            cmbResSalario.SelectedValue = objReservaListagemSolicitacaoVO.resSalario
            cmbResEscolaridade.SelectedValue = objReservaListagemSolicitacaoVO.resEscolaridade
            cmbResEstadoCivil.SelectedValue = objReservaListagemSolicitacaoVO.resEstadoCivil

            Select Case Mid(objReservaListagemSolicitacaoVO.resRG, 1, 3).Trim
                Case "RG"
                    cmbResDocIdentificacao.SelectedValue = "RG"
                    txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                Case "CC"
                    cmbResDocIdentificacao.SelectedValue = "CC"
                    txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                Case "CN"
                    cmbResDocIdentificacao.SelectedValue = "CN"
                    txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                Case "OU"
                    cmbResDocIdentificacao.SelectedValue = "OU"
                    txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                Case Else
                    cmbResDocIdentificacao.SelectedValue = "RG"
                    txtResDocIdentificacao.Text = objReservaListagemSolicitacaoVO.resRG.Trim
            End Select

            'cmbResDocIdentificacao.SelectedValue = Mid(objReservaListagemSolicitacaoVO.resRG, 1, 3).Trim
            'txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim  '"CN - 2980993"

            txtResCep.Text = objReservaListagemSolicitacaoVO.resCep.Trim.Replace("'", "").ToString.PadRight(8, "0").Insert(5, " ")
            txtResMemorando.Text = Mid(objReservaListagemSolicitacaoVO.resMemorando.Trim, 1, 100)
            Try
                cmbResEmissor.SelectedValue = objReservaListagemSolicitacaoVO.resEmissor
            Catch ex As Exception
                cmbResEmissor.SelectedValue = 0
            End Try
            txtResObs.Text = Mid(objReservaListagemSolicitacaoVO.resObs.Trim, 1, 600).Trim.Replace("'", "")
            If objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = "" Then
                'If objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = "01/01/1900" Then
                txtResDtGrupoConfirmacao.Text = ""
            Else
                txtResDtGrupoConfirmacao.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao), "dd/MM/yyyy")
            End If

            If objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal = "" Then
                txtResDtGrupoPgtoSinal.Text = ""
            Else
                txtResDtGrupoPgtoSinal.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal), "dd/MM/yyyy")
            End If
            If objReservaListagemSolicitacaoVO.resDtGrupoListagem = "" Then
                txtResDtGrupoListagem.Text = ""
            Else
                txtResDtGrupoListagem.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoListagem), "dd/MM/yyyy")
            End If

            If objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal = "" Then
                txtResDtGrupoPgtoTotal.Text = ""
            Else
                txtResDtGrupoPgtoTotal.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal), "dd/MM/yyyy")
            End If

            'txtResDtGrupoPgtoTotal.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal), "dd/MM/yyyy")

            Try
                cmbResIdWeb.SelectedValue = objReservaListagemSolicitacaoVO.resIdWeb
            Catch ex As Exception
                cmbResIdWeb.SelectedIndex = 0
            End Try
            Try
                cmbDestino.SelectedValue = objReservaListagemSolicitacaoVO.resColoniaFeriasDes
                If objReservaListagemSolicitacaoVO.resColoniaFeriasDes = "S" Then
                    btnHospedagemNova.Attributes.Add("resColoniaFeriasDes", "S")
                Else
                    btnHospedagemNova.Attributes.Add("resColoniaFeriasDes", "N")
                End If
            Catch ex As Exception
                cmbDestino.SelectedIndex = 0
            End Try
            Try
                cmbDestinoEstado.SelectedValue = objReservaListagemSolicitacaoVO.estIdDes
            Catch ex As Exception
                cmbDestinoEstado.SelectedValue = objReservaListagemSolicitacaoVO.estId
            End Try
            cmbDestinoEstado_SelectedIndexChanged(sender, e)
            txtResHoteDestino.Text = Mid(objReservaListagemSolicitacaoVO.resHotelExcursao.Trim, 1, 40).Trim.Replace("'", "")
            txtResLocalSaida.Text = Mid(objReservaListagemSolicitacaoVO.resLocalSaida.Trim, 1, 200).Trim.Replace("'", "")
            cmbResHoraSaida.SelectedValue = objReservaListagemSolicitacaoVO.resHoraSaida
            Try
                cmbReservaHoraSaida.SelectedValue = objReservaListagemSolicitacaoVO.resHoraSaida
            Catch ex As Exception
                cmbReservaHoraSaida.SelectedValue = "12"
            End Try
            Try
                txtResValorDesconto.Text = objReservaListagemSolicitacaoVO.resValorDesconto
            Catch ex As Exception
                txtResValorDesconto.Text = "0"
            End Try
            If objReservaListagemSolicitacaoVO.resPasseioPromovidoCEREC = "S" Then
                btnHospedagemNova.Attributes.Add("resPasseioPromovidoCEREC", "S")
                ckbOrganizadoSESC.Checked = True
            Else
                btnHospedagemNova.Attributes.Add("resPasseioPromovidoCEREC", "N")
                ckbOrganizadoSESC.Checked = False
            End If

            If objReservaListagemSolicitacaoVO.resRecreandoEscolar = "S" Then
                chkRecreandoEscolar.Checked = True
            Else
                chkRecreandoEscolar.Checked = False
            End If

            Try
                cmbResCatCobranca.Text = objReservaListagemSolicitacaoVO.resCatCobranca
            Catch ex As Exception
                cmbResCatCobranca.Text = "1"
            End Try
            Try
                cmbDestinoCidade.SelectedValue = objReservaListagemSolicitacaoVO.resCidadeDes.Trim
            Catch ex As Exception
                cmbDestinoCidade.SelectedIndex = 0
            End Try
            Try
                cmbPratoRapido0.Text = objReservaListagemSolicitacaoVO.refPratoCod
            Catch ex As Exception
            End Try

            If objReservaListagemSolicitacaoVO.resCaracteristica = "I" Then
                pnlGrupo.Visible = False
                pnlDestinoGrupo.Visible = False
                lblResHoraSaida.Visible = True
                cmbReservaHoraSaida.Visible = True
                gdvReserva9.Columns(3).Visible = True
                gdvReserva9.Columns(4).Visible = True
                btnHospedagemNova.Enabled = (InStr("CF", hddResStatus.Value) = 0)
                btnEmissivoNova.Enabled = False
                ckbOrganizadoSESC.Visible = False
            ElseIf objReservaListagemSolicitacaoVO.resCaracteristica = "E" Or objReservaListagemSolicitacaoVO.resCaracteristica = "T" Then
                If hddResStatus.Value = "F" Then
                    imgBoletoSinal.Visible = False
                Else
                    imgBoletoSinal.Visible = True  'Definir aqui como true após migrar o boleto com registro.
                End If
                pnlGrupo.Visible = True
                pnlDestinoGrupo.Visible = False
                lblResHoraSaida.Visible = True
                cmbReservaHoraSaida.Visible = True
                gdvReserva9.Columns(3).Visible = True
                gdvReserva9.Columns(4).Visible = True
                btnHospedagemNova.Enabled = (InStr("CF", hddResStatus.Value) = 0)
                btnEmissivoNova.Enabled = False
                ckbOrganizadoSESC.Visible = Not Session("GrupoCentralAtendimento") And hddResCaracteristica.Value <> "I"
            Else
                pnlGrupo.Visible = False
                pnlDestinoGrupo.Visible = True
                lblResHoraSaida.Visible = False
                cmbReservaHoraSaida.Visible = False
                gdvReserva9.Columns(3).Visible = False
                gdvReserva9.Columns(4).Visible = False
                btnHospedagemNova.Enabled = False
                btnEmissivoNova.Enabled = (InStr("CFE", hddResStatus.Value) = 0)
                ckbOrganizadoSESC.Visible = Not Session("GrupoCentralAtendimento") And hddResCaracteristica.Value <> "I"
            End If
            cmbDestino_SelectedIndexChanged(sender, e)

            If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
                btnReservaGravar.Visible = (InStr("CFE", hddResStatus.Value) = 0)
                btnReservaGravar.Enabled = (InStr("CFE", hddResStatus.Value) = 0)
                btnReservaCalculo.Visible = True
                btnReservaCancelar.Visible = (InStr("CFE", hddResStatus.Value) = 0) And (hddResId.Value <> "0")
                btnReservaCancelar.Enabled = (InStr("CFE", hddResStatus.Value) = 0) And (hddResId.Value <> "0")
                btnReservaReativar.Visible = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IET", hddResCaracteristica.Value) <> "0")
                btnReservaReativar.Enabled = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IET", hddResCaracteristica.Value) <> "0")
                btnInformarRestituicao.Visible = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IET", hddResCaracteristica.Value) <> "0")
                btnInformarRestituicao.Enabled = (InStr("C", hddResStatus.Value) = 1) And (hddResId.Value <> "0") And (InStr("IET", hddResCaracteristica.Value) <> "0")
            End If

            If hddResCaracteristica.Value = "I" Then
                lblResCatCobranca.Visible = False
                cmbResCatCobranca.Visible = False
            Else
                lblResCatCobranca.Visible = True
                cmbResCatCobranca.Visible = True
            End If
            'Irá montar a mascara do telefone com 10 ou 11 Digitos 
            MascaraTelefone()

            '            txtResNome.Focus()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub CarregaDadosIntegrante()
        Try
            pnlEdicaoIntegrante.Visible = True
            If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objCategoriaDAO = New CategoriaDAO("TurismoSocialCaldas")
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                    objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                Else
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                    objCategoriaDAO = New CategoriaDAO("TurismoSocialPiri")
                    objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                End If

                'Com com os Estados para uso no carregamento dos dados.
                cmbEstadoAux.Items.Clear()
                cmbEstadoAux.DataTextField = "estDescricao"
                cmbEstadoAux.DataValueField = "estId"
                cmbEstadoAux.DataSource = objEstadoDAO.consultarEstadoPais
                cmbEstadoAux.DataBind()

                pnlIntegranteEmissivo.Visible = True
                pnlIntegranteHospedagem.Visible = False
                lista = objReservaListagemIntegranteDAO.consultarResponsavelViaResIdIntId(hddResId.Value, hddIntId.Value)
                cmbIntVinculoId.DataSource = lista
                cmbIntVinculoId.DataValueField = ("intId")
                cmbIntVinculoId.DataTextField = ("intNome")
                cmbIntVinculoId.DataBind()
                cmbIntVinculoId.Items.Insert(0, New ListItem("", "0"))

                hddSolId.Value = Nothing
                hddSolIdNovo.Value = Nothing
                lblAcomodacaoEscolhida.Text = Nothing


                txtHosDataIniSol.Text = Format(CDate(objReservaListagemIntegranteVO.intDiaIni), "dd/MM/yyyy")
                hddHosDataIniSol.Value = Format(CDate(objReservaListagemIntegranteVO.intDiaIni), "dd/MM/yyyy")
                txtHosDataFimSol.Text = Format(CDate(objReservaListagemIntegranteVO.intDiaFim), "dd/MM/yyyy")
                hddHosDataFimSol.Value = Format(CDate(objReservaListagemIntegranteVO.intDiaFim), "dd/MM/yyyy")
                txtHosHoraIniSol.Text = objReservaListagemIntegranteVO.intHoraIni
                hddHosHoraIniSol.Value = objReservaListagemIntegranteVO.intHoraIni
                txtHosHoraFimSol.Text = objReservaListagemIntegranteVO.intHoraFim
                hddHosHoraFimSol.Value = objReservaListagemIntegranteVO.intHoraFim
                hddIntStatus.Value = objReservaListagemIntegranteVO.intStatus.ToString.Trim

                Try
                    cmbAcomodacaoCobranca.Text = objReservaListagemIntegranteVO.acmIdCobranca
                    hddAcomodacaoCobranca.Value = objReservaListagemIntegranteVO.acmIdCobranca
                Catch ex As Exception
                    cmbAcomodacaoCobranca.Text = Nothing
                    hddAcomodacaoCobranca.Value = Nothing
                End Try
                hddIntCPFAntes.Value = objReservaListagemIntegranteVO.intCPF
                btnIntegranteGravar.Attributes.Add("NomeAntes", objReservaListagemIntegranteVO.IntNome.Trim)
                btnIntegranteGravar.Attributes.Add("NascimentoAntes", objReservaListagemIntegranteVO.intDtNascimento.Trim)
                btnIntegranteGravar.Attributes.Add("SexoAntes", objReservaListagemIntegranteVO.intSexo)
                btnIntegranteGravar.Attributes.Add("intAlmoco", objReservaListagemIntegranteVO.intAlmoco)
                btnIntegranteGravar.Attributes.Add("intJantar", objReservaListagemIntegranteVO.intJantar)

                Habilitar(pnlIntegranteEmissivo)
                ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                'If cmbDestino.Text <> 0 Then
                'lblPratoRapido.Visible = True
                'cmbPratoRapido.Visible = True
                'Else
                'lblPratoRapido.Visible = False
                'cmbPratoRapido.Visible = False
                'End If
            Else 'Hospedagem
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
                    objCategoriaDAO = New CategoriaDAO("TurismoSocialCaldas")
                    objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                Else
                    objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
                    objCategoriaDAO = New CategoriaDAO("TurismoSocialPiri")
                    objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                End If

                'Com com os Estados para uso no carregamento dos dados.
                cmbEstadoAux.Items.Clear()
                cmbEstadoAux.DataTextField = "estDescricao"
                cmbEstadoAux.DataValueField = "estId"
                cmbEstadoAux.DataSource = objEstadoDAO.consultarEstadoPais
                cmbEstadoAux.DataBind()

                pnlIntegranteEmissivo.Visible = False
                Desabilitar(pnlIntegranteHospedagem)
                pnlIntegranteHospedagem.Visible = True
                cmbIntVinculoId.Text = Nothing
                Try
                    If objReservaListagemIntegranteVO.apaDesc.Trim > "" Then
                        lblAcomodacaoEscolhida.Text = "Acomodação " & objReservaListagemIntegranteVO.acmDescricao &
                          " Apto " & objReservaListagemIntegranteVO.apaDesc
                    Else
                        lblAcomodacaoEscolhida.Text = "Acomodação " & objReservaListagemIntegranteVO.acmDescricao
                    End If
                Catch ex As Exception
                    lblAcomodacaoEscolhida.Text = "Acomodação "
                End Try

                hddSolId.Value = objReservaListagemIntegranteVO.solId
                hddSolIdNovo.Value = objReservaListagemIntegranteVO.solId
                If hddSolId.Value > 0 Then
                    txtHosDataIniSol.Text = Format(CDate(objReservaListagemIntegranteVO.hosDataIniSol), "dd/MM/yyyy")
                    hddHosDataIniSol.Value = Format(CDate(objReservaListagemIntegranteVO.hosDataIniSol), "dd/MM/yyyy")
                    txtHosDataFimSol.Text = Format(CDate(objReservaListagemIntegranteVO.hosDataFimSol), "dd/MM/yyyy")
                    hddHosDataFimSol.Value = Format(CDate(objReservaListagemIntegranteVO.hosDataFimSol), "dd/MM/yyyy")
                    txtHosHoraIniSol.Text = objReservaListagemIntegranteVO.hosHoraIniSol
                    hddHosHoraIniSol.Value = objReservaListagemIntegranteVO.hosHoraIniSol
                    txtHosHoraFimSol.Text = objReservaListagemIntegranteVO.hosHoraFimSol
                    hddHosHoraFimSol.Value = objReservaListagemIntegranteVO.hosHoraFimSol
                    hddIntCPFAntes.Value = objReservaListagemIntegranteVO.intCPF
                    hddIntStatus.Value = objReservaListagemIntegranteVO.intStatus.ToString.Trim
                    btnIntegranteGravar.Attributes.Add("NomeAntes", objReservaListagemIntegranteVO.IntNome.Trim)
                    btnIntegranteGravar.Attributes.Add("NascimentoAntes", objReservaListagemIntegranteVO.intDtNascimento.Trim)
                    btnIntegranteGravar.Attributes.Add("SexoAntes", objReservaListagemIntegranteVO.intSexo)
                    btnIntegranteGravar.Attributes.Add("intAlmoco", objReservaListagemIntegranteVO.intAlmoco)
                    btnIntegranteGravar.Attributes.Add("intJantar", objReservaListagemIntegranteVO.intJantar)
                Else
                    txtHosDataIniSol.Text = ""
                    hddHosDataIniSol.Value = ""
                    txtHosDataFimSol.Text = ""
                    hddHosDataFimSol.Value = ""
                    txtHosHoraIniSol.Text = ""
                    hddHosHoraIniSol.Value = ""
                    txtHosHoraFimSol.Text = ""
                    hddHosHoraFimSol.Value = ""
                    hddIntCPFAntes.Value = ""
                    hddIntStatus.Value = ""
                    btnIntegranteGravar.Attributes.Remove("NomeAntes")
                    btnIntegranteGravar.Attributes.Remove("NascimentoAntes")
                    btnIntegranteGravar.Attributes.Remove("SexoAntes")
                    btnIntegranteGravar.Attributes.Remove("intAlmoco")
                    btnIntegranteGravar.Attributes.Remove("intJantar")
                End If
                Try
                    If (CDate(txtHosDataIniSol.Text) > Now.Date) Then 'And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value)) Then
                        imgBtnDtCheck_InMenos.Visible = True And (InStr("CFE", hddResStatus.Value) = 0) And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value))
                    Else
                        imgBtnDtCheck_InMenos.Visible = False
                    End If
                    If (DateDiff(DateInterval.Day, CDate(txtHosDataIniSol.Text), CDate(txtHosDataFimSol.Text)) > 1) Then
                        imgBtnDtCheck_InMais.Visible = True And (InStr("CFE", hddResStatus.Value) = 0) And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value))
                        imgBtnDtCheck_OutMenos.Visible = True And (InStr("CF", hddResStatus.Value) = 0) And (CDate(txtHosDataFimSol.Text) = CDate(hddResDataFim.Value))
                    Else
                        imgBtnDtCheck_InMais.Visible = False
                        imgBtnDtCheck_OutMenos.Visible = False
                    End If
                    imgBtnDtCheck_OutMais.Visible = True And (InStr("CF", hddResStatus.Value) = 0) And (CDate(txtHosDataFimSol.Text) = CDate(hddResDataFim.Value))
                Catch ex As Exception

                End Try
                Try
                    cmbAcomodacaoCobranca.Text = objReservaListagemIntegranteVO.acmIdCobranca
                    hddAcomodacaoCobranca.Value = objReservaListagemIntegranteVO.acmIdCobranca
                Catch ex As Exception
                    cmbAcomodacaoCobranca.Text = Nothing
                    hddAcomodacaoCobranca.Value = Nothing
                End Try
                lista = objReservaListagemAcomodacaoDAO.consultarHospedagemSelecionada(hddResId.Value, hddIntId.Value)
                If lista.Count > 1 Then
                    gdvReserva12.DataSource = lista
                    gdvReserva12.DataBind()
                Else
                    gdvReserva12.DataSource = Nothing
                    gdvReserva12.DataBind()
                    gdvReserva12.SelectedIndex = -1
                End If
                lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, hddIntId.Value)
                gdvReserva11.DataSource = lista
                gdvReserva11.DataBind()
                gdvReserva11.SelectedIndex = -1

                lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivelAlteracao(hddResId.Value, hddSolId.Value)
                radAcomodacao.DataSource = lista
                radAcomodacao.DataValueField = ("SolUsuario") '("solId")
                radAcomodacao.DataTextField = ("acmDescricao")
                radAcomodacao.DataBind()
                radAcomodacao.Visible = True
                radAcomodacao.Enabled = True
            End If

            btnIntegranteGravar.Visible = (InStr("CF", hddResStatus.Value) = 0) And
               (Session("GrupoCerec") Or Session("GrupoGP") Or Session("GrupoDR") Or (Session("GrupoEmissivo") And ((Format(CDec(objReservaListagemIntegranteVO.hosValorDevido), "#,##0.00") >
               Format(CDec(objReservaListagemIntegranteVO.hosValorPago), "#,##0.00") Or Format(CDec(objReservaListagemIntegranteVO.hosValorDevido), "#,##0.00") = "0,00"))))
            btnIntegranteExcluir.Visible = (InStr("CF", hddResStatus.Value) = 0) And
               (Session("GrupoCerec") Or Session("GrupoGP") Or Session("GrupoDR") Or (Session("GrupoEmissivo") And ((Format(CDec(objReservaListagemIntegranteVO.hosValorDevido), "#,##0.00") >
               Format(CDec(objReservaListagemIntegranteVO.hosValorPago), "#,##0.00") Or Format(CDec(objReservaListagemIntegranteVO.hosValorDevido), "#,##0.00") = "0,00"))))
            imgBtnAlterarCategoria.Visible = btnIntegranteExcluir.Visible And gdvReserva9.Rows.Count > 1
            imgBtnAlterarMemorando.Visible = btnIntegranteExcluir.Visible And gdvReserva9.Rows.Count > 1
            imgBtnAlterarPagamento.Visible = btnIntegranteExcluir.Visible And gdvReserva9.Rows.Count > 1
            imgBtnAlterarRefeicao.Visible = btnIntegranteExcluir.Visible And gdvReserva9.Rows.Count > 1

            If btnIntegranteGravar.Visible Then
                If (objReservaListagemIntegranteVO.estId = 9 And objReservaListagemIntegranteVO.catId <> 4 And objReservaListagemIntegranteVO.intMatricula.Trim > "") Then

                    'Se o integrantes já estiverem em Estada ou Parque aquático.
                    'If (hddIntStatus.Value = "E" Or hddIntStatus.Value = "P") Then
                    '    cmbIntFormaPagamento.Enabled = False
                    '    txtIntMemorando.Enabled = False
                    '    cmbIntEmissor.Enabled = False
                    '    cmbIntCatCobranca.Enabled = False
                    '    imgBtnAlterarCategoria.Enabled = False
                    '    imgBtnAlterarPagamento.Enabled = False
                    '    imgBtnAlterarMemorando.Enabled = False
                    'End If

                    txtIntMatricula.Enabled = False
                    txtIntCPF.Enabled = False
                    txtIntNome.Enabled = False
                    cmbIntSexo.Enabled = False
                    txtIntNascimento.Enabled = False
                    cmbIntCatId.Enabled = False
                    cmbIntEstId.Enabled = False
                    cmbIntCidade.Enabled = False
                    txtIntCidade.Enabled = False
                    cmbIntSalario.Enabled = False
                    cmbIntEscolaridade.Enabled = False
                    cmbIntEstadoCivil.Enabled = False
                    cmbIntRG.Enabled = False
                    txtIntRG.Enabled = False
                    'Se o integrantes já estiverem em Estada ou Parque aquático.
                ElseIf (hddIntStatus.Value = "E" Or hddIntStatus.Value = "P") Then
                    cmbIntFormaPagamento.Enabled = False
                    txtIntMemorando.Enabled = False
                    cmbIntEmissor.Enabled = False
                    cmbIntCatCobranca.Enabled = False
                    imgBtnAlterarCategoria.Enabled = False
                    imgBtnAlterarPagamento.Enabled = False
                    imgBtnAlterarMemorando.Enabled = False
                    txtIntMatricula.Enabled = False

                    'Se o grupo de passeio já estiver no parque aquatico não irá permitir alterar mais nada 
                    If (hddResCaracteristica.Value = "P" And hddIntStatus.Value = "P") Then
                        pnlIntegranteEmissivo.Enabled = False
                        cmbIntRG.Enabled = False
                        txtIntRG.Enabled = False
                    End If

                    'Free poderá alterar o cpf e nome após estarem em estada/ResCaracteristica "E" = Grupos de Hospedagem
                    If ((hddResCaracteristica.Value = "I" Or hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T") And hddResStatus.Value <> "F") Then 'Se for reserva individual e estiver em estada ou parque irá deixar alterar o cpf e nascimento
                        'A recepção poderá alterar os campos abaixo, Guias vem sem nome e precisam ter seus dados atualizados no sistema

                        txtIntCPF.Enabled = True
                        txtIntNascimento.Enabled = True
                        cmbIntRG.Enabled = True
                        txtIntRG.Enabled = True

                        If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then
                            cmbIntFormaPagamento.Enabled = True
                            txtIntMemorando.Enabled = True
                            cmbIntEmissor.Enabled = True
                        End If

                        'Se for um grupo de hospedagem
                        If ((hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "I" Or hddResCaracteristica.Value = "T") And (objReservaListagemIntegranteVO.intFormaPagamento.Trim <> "F" And objReservaListagemIntegranteVO.intFormaPagamento.Trim <> "C")) Then
                            txtIntNome.Enabled = False
                            cmbIntSexo.Enabled = False
                        Else
                            txtIntNome.Enabled = True
                            cmbIntSexo.Enabled = True
                        End If
                    Else
                        txtIntCPF.Enabled = False
                        txtIntNome.Enabled = False
                        cmbIntSexo.Enabled = False
                        txtIntNascimento.Enabled = False
                    End If

                    'txtIntNascimento.Enabled = False
                    cmbIntCatId.Enabled = False
                    cmbIntEstId.Enabled = False
                    cmbIntCidade.Enabled = False
                    txtIntCidade.Enabled = False
                    cmbIntSalario.Enabled = False
                    cmbIntEscolaridade.Enabled = False
                    cmbIntEstadoCivil.Enabled = False
                    'cmbIntRG.Enabled = False
                Else
                    txtIntMatricula.Enabled = True
                    txtIntCPF.Enabled = True
                    txtIntNome.Enabled = True
                    txtIntNascimento.Enabled = True
                    cmbIntSexo.Enabled = True
                    cmbIntCatId.Enabled = True
                    'Se for de outro estado e possuir matrícula cadastrada, não poderá mais alterar o estado
                    If objReservaListagemIntegranteVO.intMatricula.Trim.Length > 0 Then
                        cmbIntEstId.Enabled = False
                    Else
                        cmbIntEstId.Enabled = True
                    End If
                    cmbIntCidade.Enabled = True
                    txtIntCidade.Enabled = True
                    cmbIntSalario.Enabled = True
                    cmbIntEscolaridade.Enabled = True
                    cmbIntEstadoCivil.Enabled = True
                    cmbIntRG.Enabled = True
                    txtIntRG.Enabled = True
                End If
                'Ocultando e exibindo o botão excluir o integrante.
                If (objReservaListagemIntegranteVO.hosStatus = "F" Or
                   objReservaListagemIntegranteVO.hosStatus = " " Or
                   (objReservaListagemIntegranteVO.intStatus <> "E" And objReservaListagemIntegranteVO.intStatus <> "P")) Then
                    btnIntegranteExcluir.Visible = True
                Else
                    btnIntegranteExcluir.Visible = False
                End If

                'Usado para encurtar o periodo de um integrante dentro de uma reserva
                imgBtnDtCheck_InMais.Attributes.Add("DataIniSolOld", objReservaListagemIntegranteVO.solDiaIni)
                imgBtnDtCheck_InMais.Attributes.Add("DataFimSolOld", objReservaListagemIntegranteVO.solDiaFim)

                txtHosDataIniSol.Enabled = cmbAcomodacao.Text <> ""
                txtHosDataFimSol.Enabled = cmbAcomodacao.Text <> ""

                If (hddIntStatus.Value <> "E" And hddIntStatus.Value <> "P") Then
                    cmbIntFormaPagamento.Enabled = True
                    txtIntMemorando.Enabled = True
                    cmbIntEmissor.Enabled = True
                    cmbIntCatCobranca.Enabled = True
                    ckbRefeicao.Enabled = True
                    cmbAcomodacaoCobranca.Enabled = True
                    cmbIntRG.Enabled = True
                    txtIntRG.Enabled = True
                Else
                    'cmbIntFormaPagamento.Enabled = False

                    'Se for cortesias ou Free em grupos, poderá cadastrar o memorando
                    If ((objReservaListagemIntegranteVO.intFormaPagamento.Trim = "F" _
                    Or objReservaListagemIntegranteVO.intFormaPagamento.Trim = "C") _
                    And (hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T")) Then
                        cmbIntFormaPagamento.Enabled = True
                        txtIntMemorando.Enabled = True
                        cmbIntEmissor.Enabled = True
                    Else
                        cmbIntFormaPagamento.Enabled = False
                        txtIntMemorando.Enabled = False
                        cmbIntEmissor.Enabled = False
                    End If

                    cmbIntCatCobranca.Enabled = False
                    imgBtnAlterarCategoria.Enabled = False
                    imgBtnAlterarPagamento.Enabled = False
                    imgBtnAlterarMemorando.Enabled = False
                    'Se pertencer ao grupo abaixo poderá alterar a categoria de cobrança de uma reserva quando houver troca de apto por suite...
                    'Objetivo: Não gerar ônus ao hóspede (P - Pendente de pgto  E-Estada)
                    If (Grupos.Contains("Turismo Social Reserva Categoria de Cobranca") And
                       (hddResStatus.Value = "P" Or hddResStatus.Value = "E")) Then
                        cmbAcomodacaoCobranca.Enabled = True
                    End If
                End If
                ckbRefeicao.Enabled = True
                imgBtnAlterarRefeicao.Enabled = True
            Else
                Desabilitar(pnlEdicaoIntegrante)
            End If
            'txtIntMatricula.Text = objReservaListagemIntegranteVO.intMatricula
            If objReservaListagemIntegranteVO.intMatricula.Trim.Length > 0 Then
                txtIntMatricula.Text = CLng(objReservaListagemIntegranteVO.intMatricula.Trim.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("\", "").Replace("/", "").Replace("_", "")).ToString("0000 000000 0")
            Else
                txtIntMatricula.Text = objReservaListagemIntegranteVO.intMatricula.Trim
            End If
            txtIntNome.Text = objReservaListagemIntegranteVO.IntNome.Trim.Trim.Replace("'", "")
            txtIntNascimento.Text = Format(CDate(objReservaListagemIntegranteVO.intDtNascimento), "dd/MM/yyyy")
            'Calculando a idade do integrante
            If Len(txtIntNascimento.Text.Trim) = 10 And Len(txtDataInicialSolicitacao.Text.Trim) = 10 Then
                hddIdade.Value = calculaIdade(CDate(txtIntNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
            End If

            cmbIntSexo.Text = objReservaListagemIntegranteVO.intSexo
            cmbIntFormaPagamento.Text = objReservaListagemIntegranteVO.intFormaPagamento
            txtIntMemorando.Text = objReservaListagemIntegranteVO.intMemorando.Trim.Trim.Replace("'", "")
            cmbIntEmissor.Text = objReservaListagemIntegranteVO.intEmissor.Trim.Replace("'", "")

            If (hddIntStatus.Value = "E" Or hddIntStatus.Value = "P") Then
                cmbIntCatId.Items.Clear()
                If (objReservaListagemIntegranteVO.estId = 9) Then
                    If objReservaListagemIntegranteVO.catId = "1" Then
                        cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                    ElseIf objReservaListagemIntegranteVO.catId = "2" Then
                        cmbIntCatId.Items.Insert(0, New ListItem("Dependente", "2"))
                    ElseIf objReservaListagemIntegranteVO.catId = "3" Then
                        cmbIntCatId.Items.Insert(0, New ListItem("Conveniado", "3"))
                    Else
                        cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                    End If
                Else
                    cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                    cmbIntCatId.Items.Insert(1, New ListItem("Dependente", "2"))
                    cmbIntCatId.Items.Insert(2, New ListItem("Usuário", "4"))
                End If
            Else
                'Se não estiver em estada ou parque aquatico irá carregar a lista completa de categorias
                cmbIntCatId.Items.Clear()
                lista = objCategoriaDAO.consultar("Reserva")
                cmbIntCatId.DataSource = lista
                cmbIntCatId.DataValueField = ("catId")
                cmbIntCatId.DataTextField = ("catDescricao")
                cmbIntCatId.DataBind()
            End If

            cmbIntCatId.Text = objReservaListagemIntegranteVO.catId
            Try
                cmbIntCatCobranca.Text = objReservaListagemIntegranteVO.intCatCobranca
            Catch ex As Exception
                cmbIntCatCobranca.Text = objReservaListagemIntegranteVO.catId
            End Try
            Dim sender As Object = Nothing
            Dim e As System.EventArgs = Nothing


            CarregaTodosEstadosIntegrante("I")

            cmbIntEstId.SelectedValue = objReservaListagemIntegranteVO.estId
            cmbIntEstId_SelectedIndexChanged(sender, e)

            Try
                cmbIntCidade.Text = objReservaListagemIntegranteVO.intCidade.Trim.ToUpper
                cmbIntCidade.Visible = True
                cmbIntEstId.Enabled = cmbIntCidade.Enabled
                txtIntCidade.Visible = False
            Catch ex As Exception
                cmbIntCidade.Visible = False
                txtIntCidade.Visible = True
            End Try
            txtIntCidade.Text = objReservaListagemIntegranteVO.intCidade.Trim.ToUpper.Trim.Replace("'", "")
            cmbIntSalario.Text = objReservaListagemIntegranteVO.intSalario
            cmbIntEscolaridade.Text = objReservaListagemIntegranteVO.intEscolaridade
            cmbIntEstadoCivil.Text = objReservaListagemIntegranteVO.intEstadoCivil

            If objReservaListagemIntegranteVO.intAlmoco = "I" Then
                ckbRefeicao.Items.Item(0).Selected = True
                ckbRefeicao.Items.Item(2).Selected = False
            ElseIf objReservaListagemIntegranteVO.intAlmoco = "O" Then
                ckbRefeicao.Items.Item(0).Selected = False
                ckbRefeicao.Items.Item(2).Selected = True
            ElseIf objReservaListagemIntegranteVO.intAlmoco = "S" Then
                ckbRefeicao.Items.Item(0).Selected = True
                ckbRefeicao.Items.Item(2).Selected = True
            Else
                ckbRefeicao.Items.Item(0).Selected = False
                ckbRefeicao.Items.Item(2).Selected = False
            End If

            If objReservaListagemIntegranteVO.intJantar = "I" Then
                ckbRefeicao.Items.Item(1).Selected = True
                ckbRefeicao.Items.Item(3).Selected = False
            ElseIf objReservaListagemIntegranteVO.intJantar = "O" Then
                ckbRefeicao.Items.Item(1).Selected = False
                ckbRefeicao.Items.Item(3).Selected = True
            ElseIf objReservaListagemIntegranteVO.intJantar = "S" Then
                ckbRefeicao.Items.Item(1).Selected = True
                ckbRefeicao.Items.Item(3).Selected = True
            Else
                ckbRefeicao.Items.Item(1).Selected = False
                ckbRefeicao.Items.Item(3).Selected = False
            End If

            If objReservaListagemIntegranteVO.intCPF.Trim.Length > 0 Then
                txtIntCPF.Text = CLng(objReservaListagemIntegranteVO.intCPF.Trim.Replace(".", "").Replace("-", "").Replace(" ", "").Replace(".", "").Replace("\", "").Replace("/", "").Replace("_", "")).ToString("000 000 000 00")
            Else
                txtIntCPF.Text = objReservaListagemIntegranteVO.intCPF.Trim
            End If

            If objReservaListagemIntegranteVO.intRG.IndexOf(" - ") = 2 Then
                cmbIntRG.Text = objReservaListagemIntegranteVO.intRG.Substring(0, 2)
                txtIntRG.Text = objReservaListagemIntegranteVO.intRG.Substring(5).Trim.Replace("'", "")
            Else
                cmbIntRG.Text = "RG"
                txtIntRG.Text = objReservaListagemIntegranteVO.intRG.Trim.Trim.Replace("'", "")
            End If

            Try
                cmbIntVinculoId.Text = objReservaListagemIntegranteVO.intVinculoId
            Catch ex As Exception
                cmbIntVinculoId.Text = Nothing
            End Try
            txtIntFoneResponsavelExc.Text = objReservaListagemIntegranteVO.intFoneResponsavelExc
            txtIntLocalTrabalhoResponsavelExc.Text = objReservaListagemIntegranteVO.intLocalTrabalhoResponsavelExc.Trim.Trim.Replace("'", "")
            txtIntEnderecoResponsavelExc.Text = objReservaListagemIntegranteVO.intEnderecoResponsavelExc.Trim.Trim.Replace("'", "")
            txtIntBairroResponsavelExc.Text = objReservaListagemIntegranteVO.intBairroResponsavelExc.Trim.Trim.Replace("'", "")
            txtIntValorUnitarioExc.Text = objReservaListagemIntegranteVO.intValorUnitarioExc
            txtIntPoltronaExc.Text = objReservaListagemIntegranteVO.intPoltronaExc.Trim
            txtIntApartamentoExc.Text = objReservaListagemIntegranteVO.intApartamentoExc.Trim

            'Quando se tratar de grupo de passeio não mostrará as opções de alterar todos os dados de uma só vez no integrante
            If hddResCaracteristica.Value.Trim = "P" Then
                imgBtnAlterarCategoria.Visible = False
                imgBtnAlterarMemorando.Visible = False
                imgBtnAlterarPagamento.Visible = False
            Else
                imgBtnAlterarCategoria.Visible = True
                imgBtnAlterarMemorando.Visible = True
                imgBtnAlterarPagamento.Visible = True
            End If

            Try
                cmbPratoRapido.Text = objReservaListagemIntegranteVO.intPratoRefeicao
            Catch ex As Exception
                cmbPratoRapido.Text = "1"
            End Try
            If objReservaListagemIntegranteVO.intCriancaColoExc = "S" Then
                ckbColo.Checked = True
            Else
                ckbColo.Checked = False
            End If
            If txtIntNome.Enabled Then
                txtIntNome.Focus()
            Else
                cmbIntFormaPagamento.Focus()
            End If


        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
                    "&Erro=Erro ao carregar os dados do integrante. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=CarregaDadosIntegrante IntId: " & hddIntId.Value)
        End Try
    End Sub

    Protected Sub ListaIntegranteViaResId()
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
        End If
        lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", btnCaixa.CommandName)
        If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then
            Dim Linha As String
            Dim listaAux As New ArrayList
            For Each item As Turismo.ReservaListagemIntegranteVO In lista
                Linha = "<br>"
                For Each items As Turismo.ReservaListagemIntegranteVO In lista
                    If item.solId = items.solId And items.hosStatus <> "T" And item.IntNome <> items.IntNome Then
                        If items.catLink = "1" Or items.catLink = "2" Then
                            item.acmDescricao += Linha & "<br><font color='Green'>" & items.IntNome & "</font>"
                        ElseIf item.catLink = "3" Then
                            item.acmDescricao += Linha & "<br><font color='Chocolate'>" & items.IntNome & "</font>"
                        Else
                            item.acmDescricao += Linha & "<br><font color='Red'>" & items.IntNome & "</font>"
                        End If
                        Linha = ""
                    End If
                Next
                listaAux.Add(item)
            Next
            gdvReserva9.DataSource = listaAux
        Else
            gdvReserva9.DataSource = lista
        End If
        gdvReserva9.DataBind()
        gdvReserva9.SelectedIndex = -1
    End Sub

    Protected Sub ListaFinanceiroViaResId()
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
        Else
            objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialPiri")
        End If
        lista = objReservaListagemFinanceiroDAO.consultarViaResId(hddResId.Value, "")
        gdvReserva10.DataSource = lista
        gdvReserva10.DataBind()

        'Se a reserva estiver finalizada ou cancelada não irá permitir alterar as questões financeiras
        If (hddResStatus.Value = "F" Or hddResStatus.Value = "C") Then
            gdvReserva10.Enabled = False
        End If
    End Sub

    Protected Sub CarregaCmbDestino()
        If cmbDestino.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objPasseioDestinoDAO = New Turismo.PasseioDestinoDAO("TurismoSocialCaldas")
            Else
                objPasseioDestinoDAO = New Turismo.PasseioDestinoDAO("TurismoSocialPiri")
            End If
            lista = objPasseioDestinoDAO.consultar()
            cmbDestino.DataSource = lista
            cmbDestino.DataValueField = ("PasseioDestinoRefeicao")
            cmbDestino.DataTextField = ("PasseioDestinoDescricao")
            cmbDestino.DataBind()
        End If
    End Sub

    Protected Sub CarregaEstado()
        If cmbEstId.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
            Else
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
            End If

            lista = objEstadoDAO.consultarEstadoPais
            cmbEstId.DataSource = lista
            cmbEstId.DataValueField = ("estId")
            cmbEstId.DataTextField = ("estDescricao")
            cmbEstId.DataBind()
            cmbEstId.SelectedIndex = cmbEstId.Items.IndexOf(cmbEstId.Items.FindByText(" GOIÁS"))

            cmbIntEstId.DataSource = lista
            cmbIntEstId.DataValueField = ("estId")
            cmbIntEstId.DataTextField = ("estDescricao")
            cmbIntEstId.DataBind()
            cmbIntEstId.SelectedIndex = cmbIntEstId.Items.IndexOf(cmbIntEstId.Items.FindByText(" GOIÁS"))

            Dim It As ListItem
            For Each It In cmbEstId.Items
                If It.Value < 1000 Then
                    cmbDestinoEstado.Items.Add(New ListItem(It.Text, It.Value))
                End If
            Next

            cmbDestinoEstado.SelectedIndex = cmbDestinoEstado.Items.IndexOf(cmbDestinoEstado.Items.FindByText(" GOIÁS"))

            lista = objEstadoDAO.consultarEstadoPaisSemCA
            cmbIntEstIdSemCA.DataSource = lista
            cmbIntEstIdSemCA.DataValueField = ("estId")
            cmbIntEstIdSemCA.DataTextField = ("estDescricao")
            cmbIntEstIdSemCA.DataBind()

            lista = objMunicipioDAO.consultarCidadePorEstado(cmbEstId.SelectedValue)
            cmbResCidade.DataSource = lista
            cmbResCidade.DataValueField = ("munDescricao")
            cmbResCidade.DataTextField = ("munDescricao")
            cmbResCidade.DataBind()
            txtResCidade.Text = Mid(cmbResCidade.Text.Trim.ToUpper, 1, 40)

            cmbDestinoCidade.DataSource = lista
            cmbDestinoCidade.DataValueField = ("munDescricao")
            cmbDestinoCidade.DataTextField = ("munDescricao")
            cmbDestinoCidade.DataBind()

            cmbIntCidade.DataSource = lista
            cmbIntCidade.DataValueField = ("munDescricao")
            cmbIntCidade.DataTextField = ("munDescricao")
            cmbIntCidade.DataBind()
        End If
    End Sub

    Protected Sub CarregaCmbResIdWeb()
        If cmbResIdWeb.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objUsuarioGrupoDAO = New Turismo.UsuarioGrupoDAO("TurismoSocialCaldas")
            Else
                objUsuarioGrupoDAO = New Turismo.UsuarioGrupoDAO("TurismoSocialPiri")
            End If
            lista = objUsuarioGrupoDAO.consultar()
            cmbResIdWeb.DataSource = lista
            cmbResIdWeb.DataValueField = ("resId")
            cmbResIdWeb.DataTextField = ("resNome")
            cmbResIdWeb.DataBind()
        End If
    End Sub

    Protected Sub CarregaFormaPagto()
        If cmbIntFormaPagamento.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objFormaPagtoDAO = New Turismo.FormaPagtoDAO("TurismoSocialCaldas")
            Else
                objFormaPagtoDAO = New Turismo.FormaPagtoDAO("TurismoSocialPiri")
            End If
            lista = objFormaPagtoDAO.consultar()
            cmbIntFormaPagamento.DataSource = lista
            cmbIntFormaPagamento.DataValueField = ("formaPagtoId")
            cmbIntFormaPagamento.DataTextField = ("formaPagto")
            cmbIntFormaPagamento.DataBind()
        End If
    End Sub

    Protected Sub CarregaCmbRefeicaoPrato()
        If cmbPratoRapido.Items.Count = 0 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objRefeicaoPratoDAO = New Turismo.RefeicaoPratoDAO("TurismoSocialCaldas")
            Else
                objRefeicaoPratoDAO = New Turismo.RefeicaoPratoDAO("TurismoSocialPiri")
            End If
            lista = objRefeicaoPratoDAO.consultarDisponibilidade(" ")
            cmbPratoRapido.DataSource = lista
            cmbPratoRapido.DataValueField = ("RefPratoCod")
            cmbPratoRapido.DataTextField = ("RefPratoDesc")
            cmbPratoRapido.DataBind()
            cmbPratoRapido0.DataSource = lista
            cmbPratoRapido0.DataValueField = ("RefPratoCod")
            cmbPratoRapido0.DataTextField = ("RefPratoDesc")
            cmbPratoRapido0.DataBind()

        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not Session("GrupoCerec") And Not Session("GrupoGP") And Not Session("GrupoDR") And Not Session("GrupoEmissivo") Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                btnHospedagemNova.Attributes.Add("Conexao", "TurismoSocialCaldas")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialCaldas")
                objCategoriaDAO = New Turismo.CategoriaDAO("TurismoSocialCaldas")
                objOrgaoDAO = New Turismo.OrgaoDAO("TurismoSocialCaldas")
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")
                objCaracteristicaDAO = New Turismo.CaracteristicaDAO("TurismoSocialCaldas")
                objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialCaldas")
                objParametroDAO = New Turismo.ParametroDAO("TurismoSocialCaldas")
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                pnlCabecalho_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlCabecalho_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue

                pnlSolicitacaoSelecionada_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlSolicitacaoSelecionada_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
                pnlIntegranteGeral_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlIntegranteGeral_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
                pnlFinanceiroAcao_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlFinanceiroAcao_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue

                pnlPlanilhaCusto_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlPlanilhaCusto_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
                pnlPlanilhaItem_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
                pnlPlanilhaItem_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue

                imgBtnReservaNova.ImageUrl = "~/images/Reserva_add_azul.png"
                imgBtnNovaReserva.ImageUrl = "~/images/Reserva_add_azul.png"
                imgBtnPlanilhaItemVoltar.ImageUrl = "~/images/VoltarAzul.png"
                imgBtnReservaAcaoVoltar.ImageUrl = "~/images/VoltarAzul.png"
                imgBtnReservaAcaoVoltarRecepcao.ImageUrl = "~/images/VoltarAzul.png"
            Else
                btnHospedagemNova.Attributes.Add("Conexao", "TurismoSocialPiri")
                objAcomodacaoDAO = New Turismo.AcomodacaoDAO("TurismoSocialPiri")
                objCategoriaDAO = New Turismo.CategoriaDAO("TurismoSocialPiri")
                objOrgaoDAO = New Turismo.OrgaoDAO("TurismoSocialPiri")
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
                objCaracteristicaDAO = New Turismo.CaracteristicaDAO("TurismoSocialPiri")
                objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialPiri")
                objParametroDAO = New Turismo.ParametroDAO("TurismoSocialPiri")
                objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                pnlCabecalho_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlCabecalho_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen

                pnlSolicitacaoSelecionada_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlSolicitacaoSelecionada_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen
                pnlIntegranteGeral_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlIntegranteGeral_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen
                pnlFinanceiroAcao_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlFinanceiroAcao_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen

                pnlPlanilhaCusto_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlPlanilhaCusto_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen
                pnlPlanilhaItem_RoundedCornersExtender.Color = Drawing.Color.DarkSeaGreen
                pnlPlanilhaItem_RoundedCornersExtender.BorderColor = Drawing.Color.DarkSeaGreen

                imgBtnReservaNova.ImageUrl = "~/images/Reserva_add.png"
                imgBtnNovaReserva.ImageUrl = "~/images/Reserva_add.png"
                imgBtnPlanilhaItemVoltar.ImageUrl = "~/images/VoltarVerde.png"
                imgBtnReservaAcaoVoltar.ImageUrl = "~/images/VoltarVerde.png"
                imgBtnReservaAcaoVoltarRecepcao.ImageUrl = "~/images/VoltarVerde.png"
            End If
            txtDataInicialReserva.Text = Format(Date.Now.AddDays(0), "dd/MM/yyyy")
            txtDataFinalReserva.Text = Format(Date.Now.AddDays(1), "dd/MM/yyyy")

            txtDataInicialSolicitacao.Text = Format(Date.Now.AddDays(0), "dd/MM/yyyy")
            txtDataFinalSolicitacao.Text = Format(Date.Now.AddDays(1), "dd/MM/yyyy")
            hddIntervaloSolicitacao.Value = DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), CDate(txtDataFinalSolicitacao.Text))

            lista = objOrgaoDAO.consultar(Session("GrupoCerec"), Session("GrupoGP"), Session("GrupoDR"))
            cmbOrgao.DataSource = lista
            cmbOrgao.DataValueField = ("orgId")
            cmbOrgao.DataTextField = ("orgDescricao")
            cmbOrgao.DataBind()

            cmbOrgId.DataSource = lista
            cmbOrgId.DataValueField = ("orgId")
            cmbOrgId.DataTextField = ("orgDescricao")
            cmbOrgId.DataBind()
            If lista.Count > 2 Then
                cmbOrgao.Items.Insert(0, New ListItem("Todos", "0"))
                cmbOrgId.Items.Insert(cmbOrgId.Items.Count, New ListItem("CEREC Grupos", "S"))

                'Ativando os botões de salvar,cancelar e exportar o titular como integrante
                btnReservaGravar.Visible = True
                ckbResponsavel.Visible = True
                btnReservaCancelar.Visible = True
            ElseIf lista.Count = 1 Then
                lblOrgao.Visible = False
                cmbOrgao.Visible = False
                lblOrgId.Visible = False
                Dim objOrgaoVO As Turismo.OrgaoVO = lista.Item(0)
                hddOrgGrupo.Value = objOrgaoVO.orgGrupo
                hddOrgLotacao.Value = objOrgaoVO.orgLotacao

            ElseIf lista.Count = 0 Then ' Grupo Emissivo não vê Hospedagem
                cmbOrgao.Items.Insert(0, New ListItem("Todos", "0"))
                cmbOrgao.Visible = False
                lblOrgao.Visible = False
                imgBtnReservaNova.Visible = False
                lblNovaReserva.Visible = False

                btnHospedagemNova.Visible = False
                btnEmissivoNova.Visible = False
                imgBtnNovaReserva.Visible = False
                lblNovaReservaAcao.Visible = False
                btnReservaGravar.Visible = False
                btnReservaCalculo.Visible = False
                btnReservaCancelar.Visible = False
                btnReservaReativar.Visible = False
                btnInformarRestituicao.Visible = False
                lblOrgId.Visible = False
                cmbOrgId.Visible = False
            End If

            lista = objCategoriaDAO.consultar("Reserva")

            cmbIntCatId.DataSource = lista
            cmbIntCatId.DataValueField = ("catId")
            cmbIntCatId.DataTextField = ("catDescricao")
            cmbIntCatId.DataBind()
            cmbResCatId.DataSource = lista
            cmbResCatId.DataValueField = ("catId")
            cmbResCatId.DataTextField = ("catDescricao")
            cmbResCatId.DataBind()
            Dim listaAux As New ArrayList

            For Each item As Turismo.CategoriaVO In lista
                If item.catLink = item.catLinkCat Then
                    listaAux.Add(item)
                End If
            Next
            cmbIntCatCobranca.DataSource = listaAux
            cmbIntCatCobranca.DataValueField = ("catId")
            cmbIntCatCobranca.DataTextField = ("catDescricao")
            cmbIntCatCobranca.DataBind()

            lista = objCategoriaDAO.consultar()
            cmbCategoriaIntegrante.DataSource = lista
            cmbCategoriaIntegrante.DataValueField = ("catId")
            cmbCategoriaIntegrante.DataTextField = ("catDescricao")
            cmbCategoriaIntegrante.DataBind()
            cmbCategoriaIntegrante.Items.Insert(0, New ListItem("Todas", "0"))

            lista = objAcomodacaoDAO.consultar(Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"), Format(CDate(txtDataFinalReserva.Text), "dd/MM/yyyy"))
            listaAux.Clear()
            For Each item As Turismo.AcomodacaoVO In lista
                If ((item.acmTipo = "F") And Session("GrupoGP")) Or
                   ((item.acmTipo = "R") And Session("GrupoDR")) Or
                   ((item.acmTipo = "N") And Session("GrupoCerec")) Then
                    listaAux.Add(item)
                End If
            Next
            cmbAcomodacao.DataSource = listaAux
            cmbAcomodacao.DataValueField = ("acmId")
            cmbAcomodacao.DataTextField = ("acmDescricao")
            cmbAcomodacao.DataBind()
            cmbAcomodacao.Items.Insert(0, New ListItem("Todas", "0"))
            cmbAcomodacaoCobranca.DataSource = lista
            cmbAcomodacaoCobranca.DataValueField = ("acmId")
            cmbAcomodacaoCobranca.DataTextField = ("acmDescricao")
            cmbAcomodacaoCobranca.DataBind()

            objDefaultVO = objDefaultDAO.consultar()
            hddHoraInicioPernoite.Value = objDefaultVO.horaInicioPernoite.ToString
            hddHoraFimPernoite.Value = objDefaultVO.horaCheckOutRes.ToString

            hddIdadeAdulto.Value = objDefaultVO.faixaEtariaAdulto
            hddIdadeCrianca.Value = objDefaultVO.faixaEtariaCrianca
            hddIdadeIsento.Value = objDefaultVO.faixaEtariaIsento
            hddDataFaixaEtaria.Value = objDefaultVO.FaixaEtariaData

            objParametroVO = objParametroDAO.consultar()
            hddIdadeColo.Value = "1000"
            hddAutorizaConveniado.Value = "S"
            hddAutorizaUsuario.Value = "S"

            lblTipo.Visible = Session("GrupoCerec")
            If Not lblTipo.Visible Then
                If Session("GrupoEmissivo") Then
                    cmbTipo.SelectedValue = "P"
                    pnlAcomodacao.Visible = False
                    pnlPlanilha.Visible = True
                Else
                    cmbTipo.SelectedValue = "I"
                End If
            End If
            'Data da solicitação da reserva
            txtDataInicialSolicitacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataInicialSolicitacao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataInicialSolicitacao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataFinalSolicitacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataFinalSolicitacao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataFinalSolicitacao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDtLimiteRetorno.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtLimiteRetorno.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtLimiteRetorno.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataInicialReserva.Attributes.Add("OnKeyPress", "javascript:return FormataData(this,event);")
            txtDataInicialReserva.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtDataInicialReserva.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataFinalReserva.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataFinalReserva.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataFinalReserva.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")


            txtHosDataIniSol.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtHosDataIniSol.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
            txtHosDataIniSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtHosDataFimSol.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtHosDataFimSol.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
            txtHosDataFimSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtResDtGrupoConfirmacao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtGrupoConfirmacao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDtGrupoPgtoSinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtGrupoPgtoSinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDtGrupoListagem.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtGrupoListagem.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtResDtGrupoListagem.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtGrupoListagem.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            'Manipulando a matricula, aceitando a entrada tanto pelo teclado quando pelo slot reader
            txtResMatricula.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResMatricula.Attributes.Add("onKeyPress", "javascript:this.value=FormataMatricula(this.value,event);return SoNumero(event);")
            txtResMatricula.Attributes.Add("OnPaste", "javascript:return naoColarTexto();")
            txtResMatricula.Attributes.Add("OnChange", "javascript: if (this.value != ''){ctl00_conPlaHolTurismoSocial_imgBtnResMatricula.click()}; ")

            txtResMatricula.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResNome.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbOrgId.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtLimiteRetorno.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResHrLimiteRetorno.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            ckbResponsavel.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResCPF.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtConCPF.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResContato.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResEmail.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtNascimento.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResSexo.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResCatId.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResFoneComercial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResFoneResidencial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResCelular.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResFax.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResLogradouro.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResNumero.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResQuadra.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResLote.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResComplemento.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResBairro.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResCep.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResCep.Attributes.Add("onKeyPress", "javascript:this.value=mascaracep(this.value,event);return SoNumero(event);")

            cmbEstId.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResCidade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResCidade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResSalario.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResEscolaridade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResEstadoCivil.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResMemorando.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResEmissor.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResObs.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbReservaHoraSaida.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtGrupoConfirmacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtGrupoPgtoSinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtGrupoListagem.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResDtGrupoPgtoTotal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResIdWeb.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbDestino.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbDestinoEstado.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbDestinoCidade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResHoteDestino.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResLocalSaida.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResHoraSaida.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbResCatCobranca.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtResValorDesconto.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            'Movendo o foco para o ResNome da reserva quando clicar no painal da imagem para abrir a tela de digitação
            pnlResponsavelTitulo.Attributes.Add("onClick", "javascript:document.aspnetForm.ctl00$conPlaHolTurismoSocial$btnMoveFocoResNome.click();")

            'Desabilita enter no Integrante
            txtIntMatricula.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntCPF.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntNome.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntNascimento.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntSexo.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntFormaPagamento.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntMemorando.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntEmissor.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntCatId.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntCatCobranca.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntEstId.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntCidade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntCidade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntEstIdSemCA.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntSalario.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntEscolaridade.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntEstadoCivil.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbIntRG.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntRG.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            ckbRefeicao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            'Desativando o enter no PnlIntegranteHospedagem
            txtHosDataIniSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtHosDataFimSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtHosHoraIniSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtHosHoraFimSol.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            cmbAcomodacaoCobranca.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtIntMatricula.Attributes.Add("OnChange", "javascript:if(this.value != ''){ctl00_conPlaHolTurismoSocial_btnIntMatricula.click();};")
            txtIntMatricula.Attributes.Add("onKeyPress", "javascript:this.value=FormataMatricula(this.value,event);return SoNumero(event);")
            txtIntMatricula.Attributes.Add("OnPaste", "javascript:return naoColarTexto();")

            txtIntCPF.Attributes.Add("onBlur", "javascript:if(verificaCPF(this.value)==false){if(this.value!=''){this.value='';this.focus();}};")
            txtIntCPF.Attributes.Add("onKeyPress", "javascript:this.value=FormataCPF(this.value,event);return SoNumero(event);")

            txtConCPF.Attributes.Add("onBlur", "javascript:if (this.value.length > 8){verifica_cpf_cnpj(this.value);if(valida_cpf_cnpj(this.value)==false){if(this.value.length==11){alert('CPF/CNPJ inválido!')}else{alert('CPF/CNPJ inválido!')};this.value='';this.focus();}else{this.value=formata_cpf_cnpj(this.value);};}")

            txtResCPF.Attributes.Add("onBlur", "javascript:if (this.value.length > 0){verifica_cpf_cnpj(this.value);if(valida_cpf_cnpj(this.value)==false){if(this.value.length==11){alert('CPF/CNPJ inválido!')}else{alert('CPF/CNPJ inválido!')};this.value='';this.focus();}else{this.value=formata_cpf_cnpj(this.value);};}")

            'Data de Nascimento dos integrantes
            txtIntNascimento.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtIntNascimento.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")
            txtResDtNascimento.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtResDtNascimento.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            'Esse procedimento obriga o foco a sair de dentro da caixa de texto, fazendo com que o change dela seja executada.
            btnIntegranteGravar.Attributes.Add("onMouseOver", "javascript:this.focus();")

            txtPlcQtde.Attributes.Add("OnChange", "javascript:if (this.value=='') {this.value='1';}")
            txtPlcQtde.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
            txtPlcCapacidade.Attributes.Add("OnChange", "javascript:if (this.value=='') {this.value='1';}")
            txtPlcCapacidade.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
            txtPlcGuia.Attributes.Add("OnChange", "javascript:if (this.value=='') {this.value='1';}")
            txtPlcGuia.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
            txtPlcMotorista.Attributes.Add("OnChange", "javascript:if (this.value=='') {this.value='1';}")
            txtPlcMotorista.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
            TxtPlcIsento.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
            TxtPlcCrianca.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")

            txtIntValorUnitarioExc.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this.value)}")
            txtPlcPercentualConveniado.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            txtPlcPercentualUsuario.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            txtPlcMargem.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            txtPlcColo.Attributes.Add("OnChange", "javascript:if (this.value=='') {this.value='1';}")
            txtPlcColo.Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")


            btnReserva.CommandName = "resNomeComplemento"
            btnAcomodacao.CommandName = "AcmDescricao"
            btnIntegrante.CommandName = "intNomeTitular, i.intVinculoId, intNome, i.intDataIni, i.intDataFim"
            btnCaixa.CommandName = "intNomeTitular, i.intVinculoId, i.intNome, i.intDataIni, i.intDataFim"
            btnFinanceiro.CommandName = "BolImpNossoNumero" '"8 desc, 4 desc"

            'Formatando os números de telefone
            txtResFax.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtResFoneComercial.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtResFoneResidencial.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtResCelular.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")
            txtIntFoneResponsavelExc.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)")

            If Not Page.PreviousPage Is Nothing Then
                Dim placeHolder As Control = PreviousPage.Controls(0).FindControl("conPlaHolTurismoSocial")
                Dim SourceResId As HiddenField = CType(placeHolder.FindControl("hddResId"), HiddenField)
                Dim SourceIntId As HiddenField = CType(placeHolder.FindControl("hddIntId"), HiddenField)
                Dim SourceSolId As HiddenField = CType(placeHolder.FindControl("hddSolId"), HiddenField)
                Dim SourceHosId As HiddenField = CType(placeHolder.FindControl("hddHosId"), HiddenField)
                Dim SourceresCaracteristica As HiddenField = CType(placeHolder.FindControl("hddResCaracteristica"), HiddenField)
                Dim SourcetxtConsulta As TextBox = CType(placeHolder.FindControl("txtConsulta"), TextBox)
                Dim SourcecmbBloco As DropDownList = CType(placeHolder.FindControl("cmbBloco"), DropDownList)
                Dim SourceckbEntrada As CheckBox = CType(placeHolder.FindControl("ckbEntrada"), CheckBox)
                Dim SourceckbEstada As CheckBox = CType(placeHolder.FindControl("ckbEstada"), CheckBox)
                Dim SourceckbSaida As CheckBox = CType(placeHolder.FindControl("ckbSaida"), CheckBox)
                Dim SourceckbTransferencia As CheckBox = CType(placeHolder.FindControl("ckbTransferencia"), CheckBox)
                If Not SourceResId Is Nothing Then
                    hddResId.Value = SourceResId.Value
                    hddIntId.Value = SourceIntId.Value
                    hddSolId.Value = SourceSolId.Value
                    hddHosId.Value = SourceHosId.Value
                    hddResCaracteristica.Value = SourceresCaracteristica.Value
                    hddtxtConsultaRecepcao.Value = SourcetxtConsulta.Text
                    hddcmbBloco.Value = SourcecmbBloco.Text
                    hddckbEntrada.Value = SourceckbEntrada.Checked
                    hddckbEstada.Value = SourceckbEstada.Checked
                    hddckbSaida.Value = SourceckbSaida.Checked
                    hddckbTransferencia.Value = SourceckbTransferencia.Checked
                    If hddResId.Value = "-1" Then
                        imgBtnReservaNova_Click(sender:=Nothing, e:=Nothing)
                    ElseIf hddResId.Value > "" Then

                        If hddHosId.Value > "" Then
                            lnkIntNome_Click(sender, e)
                            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                            Else
                                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                            End If
                            If hddHosId.Value > 0 Then
                                objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaHosId(hddHosId.Value)
                            Else
                                objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaIntId(hddIntId.Value)
                            End If
                            hddIntId.Value = objReservaListagemIntegranteVO.intId
                            CarregaCmbRefeicaoPrato()
                            CarregaDadosIntegrante()
                        ElseIf hddIntId.Value > "" Then

                            lnkIntNome_Click(sender, e)
                        Else
                            lnkResponsavel_Click(sender, e)
                        End If
                        gdvReserva1_SelectedIndexChanged(sender, e)
                    End If
                    imgBtnNovaReserva.Visible = False
                    lblNovaReservaAcao.Visible = False
                    imgBtnReservaAcaoVoltar.Visible = False
                    imgBtnReservaAcaoVoltarRecepcao.Visible = True

                End If
            End If
            cmbHospedagem.Visible = (Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or
               (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Session("GrupoRecepcaoPiri"))

            'Carregando a lista dos Estados na variável para uso futuro.
            cmbEstadoAux.Items.Clear()
            cmbEstadoAux.DataTextField = "estDescricao"
            cmbEstadoAux.DataValueField = "estId"
            cmbEstadoAux.DataSource = objEstadoDAO.consultarEstadoPais
            cmbEstadoAux.DataBind()

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                imgBtnNovaReserva.Attributes.Add("Conexao", "TurismoSocialCaldas")
            Else
                imgBtnNovaReserva.Attributes.Add("Conexao", "TurismoSocialPiri")
            End If

            If Session("GrupoCentralAtendimento") Then
                'Não permitira somar mais um ou diminuir mais um dia
                imgBtnDtCheck_InMenos.Visible = False
                imgBtnDtCheck_InMenos.Visible = False
                imgBtnDtCheck_InMenos.Visible = False
                imgBtnDtCheck_InMenos.Visible = False
                cmbHospedagem.SelectedValue = "N"
                cmbHospedagem.Enabled = False
                btnEmissivoNova.Visible = False
                btnHospedagemNova.Attributes.Add("HospedeJa", "S")
            Else
                btnHospedagemNova.Attributes.Add("HospedeJa", "N")
                cmbHospedagem.Enabled = True
                btnEmissivoNova.Visible = True
            End If

            If Session("GrupoCEREC") Then
                btnRelFinanceiro.Visible = True
            Else
                btnRelFinanceiro.Visible = False
            End If


        End If

    End Sub

    Protected Sub btnReserva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReserva.Click
        Try
            If (sender.CommandArgument = "0") Then
                hddResId.Value = 0
                gdvReserva2.DataSource = Nothing
                gdvReserva2.DataBind()
                gdvReserva3.DataSource = Nothing
                gdvReserva3.DataBind()
                gdvReserva4.DataSource = Nothing
                gdvReserva4.DataBind()
                pnlPlanilha.Visible = False
            End If
        Catch ex As Exception

        End Try
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If
        If (txtResponsavel.Text > "" Or txtConCPF.Text > "") And CDate(txtDataFinalReserva.Text) = DateAdd(DateInterval.Day, 1, Now.Date) Then
            lista = objReservaListagemSolicitacaoDAO.consultar(cmbOrgao.SelectedValue, cmbTipo.SelectedValue,
              cmbCategoriaIntegrante.SelectedValue, cmbSituacao.SelectedValue, Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"),
              Format(CDate(DateAdd(DateInterval.Year, 1, CDate(txtDataFinalReserva.Text)).ToString), "dd/MM/yyyy").Substring(0, 10),
              txtResponsavel.Text, btnReserva.CommandName, hddResId.Value, txtConCPF.Text)
        Else
            lista = objReservaListagemSolicitacaoDAO.consultar(cmbOrgao.SelectedValue, cmbTipo.SelectedValue,
              cmbCategoriaIntegrante.SelectedValue, cmbSituacao.SelectedValue, Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"),
              Format(CDate(txtDataFinalReserva.Text), "dd/MM/yyyy"),
              txtResponsavel.Text, btnReserva.CommandName, hddResId.Value, txtConCPF.Text)
        End If

        Dim listaAux As New ArrayList
        For Each item As Turismo.ReservaListagemSolicitacaoVO In lista
            If ((item.orgGrupo = "F") And Session("GrupoGP")) Or
               ((item.orgGrupo = "R") And Session("GrupoDR")) Or
               ((item.orgGrupo = "C") And Session("GrupoCerec")) Or
               ((item.orgGrupo = "P") And Session("GrupoCerec")) Or
               ((item.orgGrupo = "P") And Session("GrupoEmissivo")) Then
                listaAux.Add(item)
            End If
        Next

        lblReserva.Text = "Reservas"
        lblIntegrante.Text = "Integrantes"
        lblAcomodacao.Text = "Acomodações"
        lblFinanceiro.Text = "Pagamentos"

        gdvReserva1.DataSource = listaAux
        gdvReserva1.DataBind()
        If lista.Count = 0 Then
            pnlReserva.Focus()
        Else
            gdvReserva1.Focus()
        End If
        If imgBtnReservaMaximizar.Attributes.Item("TelaCheia") <> "S" Then
            pnlAcomodacao.Visible = pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
        End If
        hddConsulta.Value = 1
    End Sub

    Protected Sub gdvReserva1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva1.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            cont = 0
            totalPago = 0
            totDiaria = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            totHospede += CType(e.Row.FindControl("lnkIntegrante"), LinkButton).Text
            totApto += CType(e.Row.FindControl("lnkApto"), LinkButton).Text
            cont += 1
            totalPago += gdvReserva1.DataKeys(e.Row.RowIndex).Item("resValorPago")
            'Ira inserir no tooltip a data de criação da reserva (Pedido do Marcelo)
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus").ToString <> "C" Then
                CType(e.Row.FindControl("lnkDisponibilidade"), LinkButton).ToolTip = "Reserva " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId").ToString & " Criada em " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDtInsercao").ToString _
                    & Chr(10) & "Limite Retorno: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDtLimiteRetorno").ToString & Chr(10) & "Servidor: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResUsuario").ToString '& vbNewLine & vbNewLine & "Observações: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString

                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString.Trim.Replace(" ", "").Length > 0 Then
                    CType(e.Row.FindControl("lnkDisponibilidade"), LinkButton).ToolTip += vbNewLine & vbNewLine & "Observações: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString
                End If
            End If

            'Quando for cancelada irá mostrar a dat de Criação da reserva e data e quem a cancelou (Pedido por Pollyana)
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resStatus").ToString = "C" Then
                CType(e.Row.FindControl("lnkDisponibilidade"), LinkButton).ToolTip = "Criada em " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDtInsercao").ToString & Chr(10) & "Cancelada por: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResUsuario").ToString & " em " _
                    & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResUsuarioData").ToString & Chr(10) & "Servidor: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResUsuario").ToString & vbNewLine & vbNewLine & "Observações: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString

                If gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString.Trim.Replace(" ", "").Length > 0 Then
                    CType(e.Row.FindControl("lnkDisponibilidade"), LinkButton).ToolTip += vbNewLine & vbNewLine & "Observações: " & gdvReserva1.DataKeys(e.Row.RowIndex).Item("ResObs").ToString
                End If
            End If

            CType(e.Row.FindControl("lnkServidor"), LinkButton).Text += gdvReserva1.DataKeys(e.Row.RowIndex).Item("resUsuario").ToString.Replace("SESC-GO.COM.BR\", "").Replace(".", " ")
            CType(e.Row.FindControl("lnkDisponibilidade"), LinkButton).Text +=
              " à " + gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString
            '"<br>" + gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString
            If gdvReserva1.DataKeys(e.Row.RowIndex).Item("resValorPagoAntecipado") > 0 Then
                CType(e.Row.FindControl("lnkValores"), LinkButton).Text +=
                  "<br><font size=1>(" + FormatNumber(gdvReserva1.DataKeys(e.Row.RowIndex).Item("resValorPagoAntecipado").ToString) + ")</font>"
            End If
            CType(e.Row.FindControl("lnkResponsavel"), LinkButton).CommandArgument =
            gdvReserva1.DataKeys(e.Row.RowIndex).Item(0).ToString()

            CType(e.Row.FindControl("lnkResponsavel"), LinkButton).ToolTip = "Ir para responsável (" & gdvReserva1.DataKeys(e.Row.RowIndex).Item("resId").ToString & ")"
            'Diárias
            e.Row.Cells(4).Text = DateDiff(DateInterval.Day, CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDataIni").ToString), CDate(gdvReserva1.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString)) * CType(e.Row.FindControl("lnkIntegrante"), LinkButton).Text
            totDiaria += CInt(e.Row.Cells(4).Text)
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(2).Text = "Registros: " & cont
            e.Row.Cells(3).Text = totHospede
            e.Row.Cells(4).Text = totDiaria
            e.Row.Cells(5).Text = totApto
            e.Row.Cells(6).Text = Format(CDec(totalPago), "#,##0.00")
            lblReserva.Text = "Reservas " & cont.ToString
        End If
    End Sub

    Protected Sub gdvReserva1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva1.SelectedIndexChanged
        Try
            If Not pnlReservaAcao.Visible Then
                gdvReserva1.SelectedRow.Cells(0).Focus()
            End If
        Catch ex As Exception
        End Try
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
        End If
        Try
            hddResId.Value = gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(0).ToString
            hddResDataIni.Value = Format(CDate(gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(6).ToString), "dd/MM/yyyy")
            hddResDataFim.Value = Format(CDate(gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(2).ToString), "dd/MM/yyyy")
            hddResDtLimiteRetorno.Value = Format(CDate(gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item("resDtLimiteRetorno").ToString), "dd/MM/yyyy")
            hddResStatus.Value = gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(1).ToString
        Catch ex As Exception
        End Try

        'Carregando o grid com as informações financeiras, Ressarcimento.
        If hddResId.Value > 0 Then
            gdvRessarcimento.DataSource = objReservaListagemFinanceiroDAO.ConsultaRessarcimento(hddResId.Value)
            gdvRessarcimento.DataBind()
        End If

        'Saindo do processo de copia/cola do integrante
        imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")

        'Try
        If hddResCaracteristica.Value Is Nothing Or hddResCaracteristica.Value = "" Then
            hddResCaracteristica.Value = gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(5).ToString
        End If
        '    Else
        '        'Irei mostrar o botão que permite definir a data da inserção da reserva como 01/09/2018 para que pegue a tabela atual de valores.
        '        If Session("GrupoMudarDataInsercao") And (Format(CDate(gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item("resDtInsercao").ToString), "yyyy/MM/dd") < CDate("2018-09-01")) Then
        '            BtnNovaTabela.Visible = True
        '        Else
        '            BtnNovaTabela.Visible = False
        '        End If
        '    End If
        'Catch ex As Exception
        '    hddResCaracteristica.Value = ""
        'End Try

        Dim listaAux As New ArrayList
        objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
        If objPlanilhaCustoVO.resId > "0" Then
            hddCapacidade.Value = objPlanilhaCustoVO.plcCapacidade.ToString
            hddMultiValorado.Value = objPlanilhaCustoVO.plcValorado.ToString
            hddIdadeColo.Value = objPlanilhaCustoVO.plcColo.ToString
            hddAutorizaConveniado.Value = objPlanilhaCustoVO.plcAutorizaConveniado.ToString
            hddAutorizaUsuario.Value = objPlanilhaCustoVO.plcAutorizaUsuario.ToString
            lblComAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            lblComAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = True And pnlReserva.Visible
            'Se for emissivo irá pegar o valor da tabela
            If hddResCaracteristica.Value = "P" Then
                hddIdadeColo.Value = objPlanilhaCustoVO.plcColo
                hddIdadeAdulto.Value = objPlanilhaCustoVO.PlcIdadeCrianca
                hddIdadeCrianca.Value = objPlanilhaCustoVO.PlcIdadeIsento
            End If

            'Emissivo  irá mostrar as acomodações
            If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then  'And ckbOrganizadoSESC.Checked = False Then
                lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
                gdvReserva2.DataSource = lista
                gdvReserva2.DataBind()
                gdvReserva2.SelectedIndex = -1
                For Each item As Turismo.ReservaListagemAcomodacaoVO In lista
                    If item.solExcluido = "N" Then
                        listaAux.Add(item)
                    End If
                Next
                gdvReserva8.DataSource = listaAux
                gdvReserva8.DataBind()
                gdvReserva8.SelectedIndex = -1
                pnlAcomodacao.Visible = True And pnlReserva.Visible
                pnlPlanilha.Visible = False
            End If
        Else
            hddCapacidade.Value = "45"
            hddMultiValorado.Value = "S"
            hddIdadeColo.Value = "1000"
            hddAutorizaConveniado.Value = "S"
            hddAutorizaUsuario.Value = "S"
            lblComAdulto.Text = "0,00"
            lblComCrianca.Text = "0,00"
            lblComIsento.Text = "0,00"
            lblConvAdulto.Text = "0,00"
            lblConvCrianca.Text = "0,00"
            lblConvIsento.Text = "0,00"
            lblUsuAdulto.Text = "0,00"
            lblUsuCrianca.Text = "0,00"
            lblUsuIsento.Text = "0,00"
            lblComAdultoVlr.Text = "0,00"
            lblComCriancaVlr.Text = "0,00"
            lblComIsentoVlr.Text = "0,00"
            lblConvAdultoVlr.Text = "0,00"
            lblConvCriancaVlr.Text = "0,00"
            lblConvIsentoVlr.Text = "0,00"
            lblUsuAdultoVlr.Text = "0,00"
            lblUsuCriancaVlr.Text = "0,00"
            lblUsuIsentoVlr.Text = "0,00"
            lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
            gdvReserva2.DataSource = lista
            gdvReserva2.DataBind()
            gdvReserva2.SelectedIndex = -1
            For Each item As Turismo.ReservaListagemAcomodacaoVO In lista
                If item.solExcluido = "N" Then
                    listaAux.Add(item)
                End If
            Next
            gdvReserva8.DataSource = listaAux
            gdvReserva8.DataBind()
            gdvReserva8.SelectedIndex = -1
            pnlAcomodacao.Visible = True And pnlReserva.Visible
            pnlPlanilha.Visible = False
        End If

        lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", btnCaixa.CommandName)
        gdvReserva3.DataSource = lista
        gdvReserva3.DataBind()
        gdvReserva3.SelectedIndex = -1

        lista = objReservaListagemFinanceiroDAO.consultarViaResId(hddResId.Value, "")
        listaAux.Clear()
        If InStr("ERCF", hddResStatus.Value) > 0 Then
            For Each item As Turismo.ReservaListagemFinanceiroVO In lista
                If item.venData <> "" Then
                    listaAux.Add(item)
                End If
            Next
        Else
            listaAux = lista
        End If

        If listaAux.Count = 0 Then
            Try
                gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados. <br><br> Bloqueado até " & gdvReserva1.DataKeys(gdvReserva1.SelectedIndex).Item(9).ToString
            Catch ex As Exception
                gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados."
            End Try
        Else
            gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados."
        End If
        gdvReserva4.DataSource = listaAux
        gdvReserva4.DataBind()
        gdvReserva4.SelectedIndex = -1
        btnImprimeComprovante.Visible = hddResStatus.Value = "R"



    End Sub

    Protected Sub btnAcomodacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcomodacao.Click
        If (sender.CommandArgument = "0") Then
            hddResId.Value = 0
            gdvReserva1.DataSource = Nothing
            gdvReserva1.DataBind()
            gdvReserva3.DataSource = Nothing
            gdvReserva3.DataBind()
            gdvReserva4.DataSource = Nothing
            gdvReserva4.DataBind()
            pnlPlanilha.Visible = False
        End If
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
        End If
        lista = objReservaListagemAcomodacaoDAO.consultar(Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"),
                                                          Format(CDate(txtDataFinalReserva.Text), "dd/MM/yyyy"),
          cmbAcomodacao.Text, btnAcomodacao.CommandName, hddResId.Value)
        Dim listaAux As New ArrayList
        For Each item As Turismo.ReservaListagemAcomodacaoVO In lista
            If ((item.acmTipo = "F") And Session("GrupoGP")) Or
               ((item.acmTipo = "R") And Session("GrupoDR")) Or
               ((item.acmTipo = "N") And Session("GrupoCerec")) Then
                listaAux.Add(item)
            End If
        Next

        lblReserva.Text = "Reservas"
        lblIntegrante.Text = "Integrantes"
        lblAcomodacao.Text = "Acomodações"
        lblFinanceiro.Text = "Pagamentos"

        gdvReserva2.DataSource = listaAux
        gdvReserva2.DataBind()
        If imgBtnReservaMaximizar.Attributes.Item("TelaCheia") <> "S" Then
            pnlAcomodacao.Visible = pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
        End If
        If listaAux.Count = 0 Then
        Else
            gdvReserva2.Focus()
        End If
        hddConsulta.Value = 2
    End Sub

    Protected Sub gdvReserva2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva2.SelectedIndexChanged
        If Not pnlReservaAcao.Visible Then
            gdvReserva2.SelectedRow.Cells(0).Focus()
        End If
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
            hddResId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(1).ToString
            hddSolId.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(0).ToString
            hddSolIdNovo.Value = gdvReserva2.DataKeys(gdvReserva2.SelectedIndex).Item(0).ToString
        Catch ex As Exception

        End Try
        'Saindo do processo de copia/cola do integrante
        imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")
        lista = objReservaListagemSolicitacaoDAO.consultarViaResId(hddResId.Value)
        gdvReserva1.DataSource = lista
        gdvReserva1.DataBind()
        gdvReserva1.SelectedIndex = -1

        objReservaListagemSolicitacaoVO = lista.Item(0)
        hddResDataIni.Value = objReservaListagemSolicitacaoVO.resDataIni.ToString
        hddResDataFim.Value = objReservaListagemSolicitacaoVO.resDataFim.ToString
        hddResStatus.Value = objReservaListagemSolicitacaoVO.resStatus.ToString

        lista = objReservaListagemIntegranteDAO.consultarViaSolId(hddSolId.Value)
        gdvReserva3.DataSource = lista
        gdvReserva3.DataBind()
        gdvReserva3.SelectedIndex = -1
        lista = objReservaListagemFinanceiroDAO.consultarViaResId(hddResId.Value, "")
        Dim listaAux As New ArrayList
        If InStr("ERCF", hddResStatus.Value) > 0 Then
            For Each item As Turismo.ReservaListagemFinanceiroVO In lista
                If item.venData <> "" Then
                    listaAux.Add(item)
                End If
            Next
        Else
            listaAux = lista
        End If

        If listaAux.Count = 0 Then
            gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados. <br><br> Bloqueado até " & hddResDtLimiteRetorno.Value
        Else
            gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados."
        End If
        hddResDtLimiteRetorno.Value = ""
        gdvReserva4.DataSource = listaAux
        gdvReserva4.DataBind()
        gdvReserva4.SelectedIndex = -1
        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
        gdvReserva8.DataSource = lista
        gdvReserva8.DataBind()
        gdvReserva8.SelectedIndex = -1

        'Grupo de passeio só terá permissão para alterar os dados quem pertencer ao grupo "Turismo Social Acao Passeio"
        If (cmbTipo.SelectedValue = "P" And
         Not Grupos.Contains("Turismo Social Acao Passeio")) Then
            pnlResponsavelTitulo.Visible = False
            pnlResponsavelAcao.Visible = False
        Else
            pnlResponsavelAcao.Visible = True
            pnlResponsavelTitulo.Visible = True
        End If
        btnImprimeComprovante.Visible = hddResStatus.Value = "R"
    End Sub

    Protected Sub btnIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIntegrante.Click
        If (sender.CommandArgument = "0") Then
            hddResId.Value = 0
            gdvReserva1.DataSource = Nothing
            gdvReserva1.DataBind()
            gdvReserva2.DataSource = Nothing
            gdvReserva2.DataBind()
            gdvReserva4.DataSource = Nothing
            gdvReserva4.DataBind()
            pnlPlanilha.Visible = False
        End If
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
        End If

        'If txtNomeIntegrante.Text > "" And CDate(txtDataFinalIntegrante.Text) = DateAdd(DateInterval.Day, 1, Now.Date) Then
        'lista = objReservaListagemIntegranteDAO.consultar(txtDataInicialIntegrante.Text, DateAdd(DateInterval.Year, 1, CDate(txtDataFinalIntegrante.Text)).ToString.Substring(0, 10), _
        'cmbCategoriaIntegrante.Text, txtNomeIntegrante.Text, "")
        'Else
        lista = objReservaListagemIntegranteDAO.consultar(Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"),
                                                              Format(CDate(txtDataFinalReserva.Text), "dd/MM/yyyy"),
                  cmbCategoriaIntegrante.Text, txtResponsavel.Text, "", cmbSituacao.Text, btnIntegrante.CommandName, hddResId.Value, txtConCPF.Text)
        'End If
        Dim listaAux As New ArrayList
        For Each item As Turismo.ReservaListagemIntegranteVO In lista
            If ((item.acmTipo = "F") And Session("GrupoGP")) Or
               ((item.acmTipo = "R") And Session("GrupoDR")) Or
               ((item.acmTipo = "N") And Session("GrupoCerec")) Or
               ((item.acmTipo = "P") And Session("GrupoCerec")) Or
               ((item.acmTipo = "P") And Session("GrupoEmissivo")) Then
                listaAux.Add(item)
            End If
        Next

        lblReserva.Text = "Reservas"
        lblIntegrante.Text = "Integrantes"
        lblAcomodacao.Text = "Acomodações"
        lblFinanceiro.Text = "Pagamentos"

        gdvReserva3.DataSource = listaAux
        If cmbCategoriaIntegrante.Text <> "0" Then
            gdvReserva3.Columns(1).Visible = False
        Else
            gdvReserva3.Columns(1).Visible = True
        End If
        gdvReserva3.DataBind()
        If listaAux.Count = 0 Then
            pnlIntegrante.Focus()
        Else
            gdvReserva3.Focus()
        End If
        If imgBtnReservaMaximizar.Attributes.Item("TelaCheia") <> "S" Then
            pnlAcomodacao.Visible = pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
        End If
        hddConsulta.Value = 3
    End Sub

    Protected Sub gdvReserva3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva3.SelectedIndexChanged
        Try
            If Not pnlReservaAcao.Visible Then
                gdvReserva3.SelectedRow.Cells(0).Focus()
            End If
        Catch ex As Exception

        End Try
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
        End If
        Try
            hddResId.Value = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item(1).ToString
            hddIntId.Value = gdvReserva3.DataKeys(gdvReserva3.SelectedIndex).Item(0).ToString
        Catch ex As Exception

        End Try
        'Saindo do processo de copia/cola do integrante
        imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")
        lista = objReservaListagemSolicitacaoDAO.consultarViaResId(hddResId.Value)
        gdvReserva1.DataSource = lista
        gdvReserva1.DataBind()
        gdvReserva1.SelectedIndex = -1

        objReservaListagemSolicitacaoVO = lista.Item(0)
        hddResDataIni.Value = objReservaListagemSolicitacaoVO.resDataIni.ToString
        hddResDataFim.Value = objReservaListagemSolicitacaoVO.resDataFim.ToString
        hddResStatus.Value = objReservaListagemSolicitacaoVO.resStatus.ToString

        Dim listaAux As New ArrayList
        objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
        If objPlanilhaCustoVO.resId > "0" Then
            hddCapacidade.Value = objPlanilhaCustoVO.plcCapacidade.ToString
            hddMultiValorado.Value = objPlanilhaCustoVO.plcValorado.ToString
            hddIdadeColo.Value = objPlanilhaCustoVO.plcColo.ToString
            hddAutorizaConveniado.Value = objPlanilhaCustoVO.plcAutorizaConveniado.ToString
            hddAutorizaUsuario.Value = objPlanilhaCustoVO.plcAutorizaUsuario.ToString
            lblComAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            lblComAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = True And pnlReserva.Visible
            'Se for emissivo irá pegar o valor da tabela
            If hddResCaracteristica.Value = "P" Then
                hddIdadeColo.Value = objPlanilhaCustoVO.plcColo
                hddIdadeAdulto.Value = objPlanilhaCustoVO.PlcIdadeCrianca
                hddIdadeCrianca.Value = objPlanilhaCustoVO.PlcIdadeIsento
            End If
        Else
            hddCapacidade.Value = "45"
            hddMultiValorado.Value = "S"
            hddIdadeColo.Value = "1000"
            hddAutorizaConveniado.Value = "S"
            hddAutorizaUsuario.Value = "S"
            lblComAdulto.Text = "0,00"
            lblComCrianca.Text = "0,00"
            lblComIsento.Text = "0,00"
            lblConvAdulto.Text = "0,00"
            lblConvCrianca.Text = "0,00"
            lblConvIsento.Text = "0,00"
            lblUsuAdulto.Text = "0,00"
            lblUsuCrianca.Text = "0,00"
            lblUsuIsento.Text = "0,00"
            lblComAdultoVlr.Text = "0,00"
            lblComCriancaVlr.Text = "0,00"
            lblComIsentoVlr.Text = "0,00"
            lblConvAdultoVlr.Text = "0,00"
            lblConvCriancaVlr.Text = "0,00"
            lblConvIsentoVlr.Text = "0,00"
            lblUsuAdultoVlr.Text = "0,00"
            lblUsuCriancaVlr.Text = "0,00"
            lblUsuIsentoVlr.Text = "0,00"
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = True And pnlReserva.Visible
            lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
            gdvReserva2.DataSource = lista
            gdvReserva2.DataBind()
            gdvReserva2.SelectedIndex = -1
            For Each item As Turismo.ReservaListagemAcomodacaoVO In lista
                If item.solExcluido = "N" Then
                    listaAux.Add(item)
                End If
            Next
            gdvReserva8.DataSource = listaAux
            gdvReserva8.DataBind()
            gdvReserva8.SelectedIndex = -1
            pnlAcomodacao.Visible = True And pnlReserva.Visible
            pnlPlanilha.Visible = False
        End If
        lista = objReservaListagemFinanceiroDAO.consultarViaIntId(hddIntId.Value)
        listaAux.Clear()
        If InStr("ERCF", hddResStatus.Value) > 0 Then
            For Each item As Turismo.ReservaListagemFinanceiroVO In lista
                If item.venData <> "" Then
                    listaAux.Add(item)
                End If
            Next
        Else
            listaAux = lista
        End If

        If listaAux.Count = 0 Then
            gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados. <br><br> Bloqueado até " & hddResDtLimiteRetorno.Value
        Else
            gdvReserva4.EmptyDataText = "Atenção! Não existem pagamentos registrados."
        End If
        hddResDtLimiteRetorno.Value = ""
        gdvReserva4.DataSource = listaAux
        gdvReserva4.DataBind()
        gdvReserva4.SelectedIndex = -1

        'Grupo de passeio só terá permissão para alterar os dados quem pertencer ao grupo "Turismo Social Acao Passeio"
        If (cmbTipo.SelectedValue = "P" And
         Not Grupos.Contains("Turismo Social Acao Passeio")) Then
            pnlResponsavelAcao.Visible = False
            pnlResponsavelTitulo.Visible = False
        Else
            pnlResponsavelAcao.Visible = True
            pnlResponsavelTitulo.Visible = True
        End If
        btnImprimeComprovante.Visible = hddResStatus.Value = "R"
    End Sub

    Protected Sub btnFinanceiro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinanceiro.Click
        If (sender.CommandArgument = "0") Then
            hddResId.Value = 0
            gdvReserva1.DataSource = Nothing
            gdvReserva1.DataBind()
            gdvReserva2.DataSource = Nothing
            gdvReserva2.DataBind()
            gdvReserva3.DataSource = Nothing
            gdvReserva3.DataBind()
            pnlPlanilha.Visible = False
        End If
        Dim listaAux As New ArrayList
        'If txtDataPgtoFinanceiro.Text > "" Or txtBoleto.Text > "" Then
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
        Else
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
        End If
        lista = objReservaListagemFinanceiroDAO.consultar(Format(CDate(txtDataInicialReserva.Text), "dd/MM/yyyy"),
          Format(CDate(txtDataFinalReserva.Text), "dd/MM/yyyy"), txtBoleto.Text,
          "0.00", cmbTipoPgto.Text, btnFinanceiro.CommandName, hddResId.Value)
        For Each item As Turismo.ReservaListagemFinanceiroVO In lista
            If ((item.orgGrupo = "F") And Session("GrupoGP")) Or
               ((item.orgGrupo = "R") And Session("GrupoDR")) Or
               ((item.resCaracteristica = "P") And Session("GrupoEmissivo")) Or
               ((item.orgGrupo = "C") And Session("GrupoCerec")) Then
                listaAux.Add(item)
            End If
        Next

        lblReserva.Text = "Reservas"
        lblIntegrante.Text = "Integrantes"
        lblAcomodacao.Text = "Acomodações"
        lblFinanceiro.Text = "Pagamentos"

        gdvReserva4.DataSource = listaAux
        gdvReserva4.DataBind()
        gdvReserva4.Visible = True
        'Else
        'gdvReserva4.Visible = False
        'End If
        If listaAux.Count = 0 Then
            pnlFinanceiro.Focus()
        Else
            gdvReserva4.Focus()
        End If
        If imgBtnReservaMaximizar.Attributes.Item("TelaCheia") <> "S" Then
            pnlAcomodacao.Visible = pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
        End If
        hddConsulta.Value = 4
    End Sub

    Protected Sub gdvReserva4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva4.SelectedIndexChanged
        If Not pnlReservaAcao.Visible Then
            gdvReserva4.SelectedRow.Cells(0).Focus()
        End If
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
        End If
        Try
            hddResId.Value = gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item(0).ToString
            hddBolImpId.Value = Mid(gdvReserva4.DataKeys(gdvReserva4.SelectedIndex).Item(4).ToString, 1, 10)
        Catch ex As Exception

        End Try
        'Saindo do processo de copia/cola do integrante
        imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")
        lista = objReservaListagemSolicitacaoDAO.consultarViaResId(hddResId.Value)
        gdvReserva1.DataSource = lista
        gdvReserva1.DataBind()
        gdvReserva1.SelectedIndex = -1

        objReservaListagemSolicitacaoVO = lista.Item(0)
        hddResDataIni.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataIni.ToString), "dd/MM/yyyy")
        hddResDataFim.Value = Format(CDate(objReservaListagemSolicitacaoVO.resDataFim.ToString), "dd/MM/yyyy")
        Dim listaAux As New ArrayList
        objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
        If objPlanilhaCustoVO.resId > "0" Then
            hddCapacidade.Value = objPlanilhaCustoVO.plcCapacidade.ToString
            hddMultiValorado.Value = objPlanilhaCustoVO.plcValorado.ToString
            hddIdadeColo.Value = objPlanilhaCustoVO.plcColo.ToString
            hddAutorizaConveniado.Value = objPlanilhaCustoVO.plcAutorizaConveniado.ToString
            hddAutorizaUsuario.Value = objPlanilhaCustoVO.plcAutorizaUsuario.ToString
            lblComAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            lblComAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
            lblComCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
            lblComIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
            lblConvAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
            lblConvCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
            lblConvIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
            lblUsuAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
            lblUsuCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
            lblUsuIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = True And pnlReserva.Visible
            'Se for emissivo irá pegar o valor da tabela
            If hddResCaracteristica.Value = "P" Then
                hddIdadeColo.Value = objPlanilhaCustoVO.plcColo
                hddIdadeAdulto.Value = objPlanilhaCustoVO.PlcIdadeCrianca
                hddIdadeCrianca.Value = objPlanilhaCustoVO.PlcIdadeIsento
            End If
        Else
            hddCapacidade.Value = "45"
            hddMultiValorado.Value = "S"
            hddIdadeColo.Value = "1000"
            hddAutorizaConveniado.Value = "S"
            hddAutorizaUsuario.Value = "S"
            lblComAdulto.Text = "0,00"
            lblComCrianca.Text = "0,00"
            lblComIsento.Text = "0,00"
            lblConvAdulto.Text = "0,00"
            lblConvCrianca.Text = "0,00"
            lblConvIsento.Text = "0,00"
            lblUsuAdulto.Text = "0,00"
            lblUsuCrianca.Text = "0,00"
            lblUsuIsento.Text = "0,00"
            lblComAdultoVlr.Text = "0,00"
            lblComCriancaVlr.Text = "0,00"
            lblComIsentoVlr.Text = "0,00"
            lblConvAdultoVlr.Text = "0,00"
            lblConvCriancaVlr.Text = "0,00"
            lblConvIsentoVlr.Text = "0,00"
            lblUsuAdultoVlr.Text = "0,00"
            lblUsuCriancaVlr.Text = "0,00"
            lblUsuIsentoVlr.Text = "0,00"
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = True And pnlReserva.Visible
            lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
            gdvReserva2.DataSource = lista
            gdvReserva2.DataBind()
            gdvReserva2.SelectedIndex = -1
            For Each item As Turismo.ReservaListagemAcomodacaoVO In lista
                If item.solExcluido = "N" Then
                    listaAux.Add(item)
                End If
            Next
            gdvReserva8.DataSource = listaAux
            gdvReserva8.DataBind()
            gdvReserva8.SelectedIndex = -1
            pnlAcomodacao.Visible = True And pnlReserva.Visible 'pnlReserva.Visible And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P")
            pnlPlanilha.Visible = False 'pnlReserva.Visible And Not pnlAcomodacao.Visible
        End If
        If hddBolImpId.Value > "" Then
            lista = objReservaListagemIntegranteDAO.consultarViaBolImpId(hddBolImpId.Value)
        Else
            lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", btnCaixa.CommandName)
        End If
        gdvReserva3.DataSource = lista
        gdvReserva3.DataBind()
        gdvReserva3.SelectedIndex = -1

        'Grupo de passeio só terá permissão para alterar os dados quem pertencer ao grupo "Turismo Social Acao Passeio"
        If (cmbTipo.SelectedValue = "P" And
         Not Grupos.Contains("Turismo Social Acao Passeio")) Then
            pnlResponsavelAcao.Visible = False
            pnlResponsavelTitulo.Visible = False
        Else
            pnlResponsavelAcao.Visible = True
            pnlResponsavelTitulo.Visible = True
        End If
        btnImprimeComprovante.Visible = hddResStatus.Value = "R"
    End Sub

    Protected Sub gdvReserva4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva4.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            cont = 0
            totalValor = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).CommandArgument =
                gdvReserva4.DataKeys(e.Row.RowIndex).Item(0).ToString()

            If gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor") > 0 Then
                CType(e.Row.FindControl("lnkVlrPago"), LinkButton).Text = FormatNumber(gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor").ToString)
            End If
            If gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "B" Then
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Boleto"
                cont += 1
                totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
            ElseIf gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "T" Then
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Cartão de Crédito"
                cont += 1
                totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
            ElseIf gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "A" Then
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Central Atendimento"
                cont += 1
                totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
            ElseIf gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "M" Then
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Manual - " & gdvReserva4.DataKeys(e.Row.RowIndex).Item("venUsuario")
                cont += 1
                totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
            ElseIf gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "C" Then
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Caixa - " & gdvReserva4.DataKeys(e.Row.RowIndex).Item("venUsuario")
                cont += 1
                totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
            ElseIf gdvReserva4.DataKeys(e.Row.RowIndex).Item("venStatus") = "E" Then
                CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDtVencimento"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDtVencimento"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDtPagamento"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDtPagamento"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkVlrPago"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkVlrPago"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = "Estornado " &
                gdvReserva4.DataKeys(e.Row.RowIndex).Item("venUsuario") & " " &
                gdvReserva4.DataKeys(e.Row.RowIndex).Item("venUsuarioData")
            Else
                CType(e.Row.FindControl("lnkFormaPgto"), LinkButton).Text = ""
            End If
            'cont += 1
            'totalValor += gdvReserva4.DataKeys(e.Row.RowIndex).Item("venValor")
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(3).Text = Format(CDec(totalValor), "#,##0.00")
            e.Row.Cells(4).Text = cont
            lblFinanceiro.Text = "Pagamentos " & cont.ToString
        End If
    End Sub

    Protected Sub imgBtnReservaMaximizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReservaMaximizar.Click
        TdReserva.Width = "100%"
        imgBtnReservaMaximizar.Attributes.Add("TelaCheia", "S")
        btnReserva.Visible = True
        pnlCabecalho.Visible = True
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False
        pnlReserva.Height = pnlReserva.Height.Value * 2
        imgBtnReservaMaximizar.Visible = False
        imgBtnReservaMinimizar.Visible = True
    End Sub

    Protected Sub imgBtnReservaMinimizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReservaMinimizar.Click
        imgBtnReservaMaximizar.Attributes.Remove("TelaCheia")
        TdReserva.Width = "60%"
        btnReserva.Visible = True
        pnlCabecalho.Visible = True
        pnlAcomodacao.Visible = True And (Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR"))
        pnlPlanilha.Visible = Not pnlAcomodacao.Visible
        pnlIntegrante.Visible = True
        pnlFinanceiro.Visible = True
        pnlReserva.Height = pnlReserva.Height.Value / 2
        imgBtnReservaMaximizar.Visible = True
        imgBtnReservaMinimizar.Visible = False
    End Sub

    Protected Sub imgBtnAcomodacaoMaximizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnAcomodacaoMaximizar.Click
        imgBtnReservaMaximizar.Attributes.Add("TelaCheia", "S")
        tdAcomodacao.Width = "100%"
        btnAcomodacao.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False
        pnlAcomodacao.Height = pnlAcomodacao.Height.Value * 2
        imgBtnAcomodacaoMaximizar.Visible = False
        imgBtnAcomodacaoMinimizar.Visible = True
    End Sub

    Protected Sub imgBtnAcomodacaoMinimizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnAcomodacaoMinimizar.Click
        imgBtnReservaMaximizar.Attributes.Remove("TelaCheia")
        TdIntegrantes.Width = "40%"
        btnAcomodacao.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = True
        pnlIntegrante.Visible = True
        pnlFinanceiro.Visible = True
        pnlAcomodacao.Height = pnlAcomodacao.Height.Value / 2
        imgBtnAcomodacaoMaximizar.Visible = True
        imgBtnAcomodacaoMinimizar.Visible = False
    End Sub

    Protected Sub imgBtnIntegranteMaximizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnIntegranteMaximizar.Click
        TdIntegrantes.Width = "100%"
        imgBtnReservaMaximizar.Attributes.Add("TelaCheia", "S")
        btnIntegrante.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlFinanceiro.Visible = False
        pnlIntegrante.Height = pnlIntegrante.Height.Value * 2
        imgBtnIntegranteMaximizar.Visible = False
        imgBtnIntegranteMinimizar.Visible = True
    End Sub

    Protected Sub imgBtnIntegranteMinimizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnIntegranteMinimizar.Click
        imgBtnReservaMaximizar.Attributes.Remove("TelaCheia")
        TdIntegrantes.Width = "60%"
        btnIntegrante.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = True
        pnlAcomodacao.Visible = True And (Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR"))
        pnlPlanilha.Visible = Not pnlAcomodacao.Visible
        pnlFinanceiro.Visible = True
        pnlIntegrante.Height = pnlIntegrante.Height.Value / 2
        imgBtnIntegranteMaximizar.Visible = True
        imgBtnIntegranteMinimizar.Visible = False
    End Sub

    Protected Sub imgBtnPagamentoMaximizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnPagamentoMaximizar.Click
        imgBtnReservaMaximizar.Attributes.Add("TelaCheia", "S")
        tdFinanceiro.Width = "100%"
        btnFinanceiro.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Height = pnlFinanceiro.Height.Value * 2
        imgBtnPagamentoMaximizar.Visible = False
        imgBtnPagamentoMinimizar.Visible = True
    End Sub

    Protected Sub imgBtnPagamentoMinimizar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnPagamentoMinimizar.Click
        imgBtnReservaMaximizar.Attributes.Remove("TelaCheia")
        tdFinanceiro.Width = "40%"
        btnFinanceiro.Visible = True
        pnlCabecalho.Visible = True
        pnlReserva.Visible = True
        pnlAcomodacao.Visible = True And (Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR"))
        pnlPlanilha.Visible = Not pnlAcomodacao.Visible
        pnlIntegrante.Visible = True
        pnlFinanceiro.Height = pnlFinanceiro.Height.Value / 2
        imgBtnPagamentoMaximizar.Visible = True
        imgBtnPagamentoMinimizar.Visible = False
    End Sub

    Protected Sub gdvReserva5_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva5.SelectedIndexChanged
        txtPliId.Text = gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item(0).ToString
        txtPliDescricao.Text = gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item(1).ToString
        'cmbModeloA.Text = gdvReserva5.DataKeys(gdvReserva5.SelectedIndex).Item(2).ToString
        btnPlanilhaItemNovo.Enabled = True
        btnPlanilhaItemExcluir.Enabled = True
        txtPliDescricao.Focus()
    End Sub

    Protected Sub btnPlanilhaCustoNovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlanilhaItemNovo.Click
        btnPlanilhaItemNovo.Enabled = False
        btnPlanilhaItemExcluir.Enabled = False
        txtPliDescricao.Text = ""
        txtPliDescricao.Focus()
        txtPliId.Text = 0
        gdvReserva5.SelectedIndex = -1
    End Sub

    Protected Sub btnPlanilhaCustoExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlanilhaItemExcluir.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialCaldas")
        Else
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialPiri")
        End If

        objPlanilhaItemVO.pliId = -1 * txtPliId.Text
        objPlanilhaItemVO.pliDescricao = txtPliDescricao.Text
        'objPlanilhaCustoVO.modA = cmbModeloA.SelectedValue
        objPlanilhaItemDAO.Acao(objPlanilhaItemVO)
        lista = objPlanilhaItemDAO.consultar
        gdvReserva5.DataSource = lista
        gdvReserva5.DataBind()
        gdvReserva5.SelectedIndex = -1
        txtPliDescricao.Focus()
        btnPlanilhaItemNovo.Enabled = False
        btnPlanilhaItemExcluir.Enabled = False
        txtPliId.Text = 0
        txtPliDescricao.Text = ""
    End Sub

    Protected Sub gdvReserva5_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva5.RowDataBound
        If e.Row.RowType = DataControlRowType.Footer Then
            If gdvReserva5.Rows.Count > 1 Then
                e.Row.Cells(0).Text = gdvReserva5.Rows.Count.ToString & " Registros"
            Else
                e.Row.Cells(0).Text = gdvReserva5.Rows.Count.ToString & " Registro"
            End If
        End If
    End Sub

    Protected Sub imgBtnReservaNova_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReservaNova.Click, imgBtnNovaReserva.Click
        pnlSolicitacaoSelecionada_RoundedCornersExtender.Enabled = False
        pnlIntegranteGeral_RoundedCornersExtender.Enabled = False
        'pnlReservaAcao_RoundedCornersExtender.Enabled = False

        Me.Limpar(pnlResponsavelAcao)
        pnlCabecalho.Visible = False
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False
        pnlAcomodacaoTitulo.Visible = False
        pnlDisponibilidadeAlternativaAux.Visible = False

        CarregaCmbResIdWeb()
        CarregaEstado()
        CarregaFormaPagto()
        CarregaCmbDestino()
        CarregaCmbRefeicaoPrato()
        pnlReservaAcao.Visible = True

        gdvReserva6.DataSource = Nothing
        gdvReserva6.DataBind()
        gdvReserva7.DataSource = Nothing
        gdvReserva7.DataBind()
        gdvReserva8.DataSource = Nothing
        gdvReserva8.DataBind()
        gdvReserva9.DataSource = Nothing
        gdvReserva9.DataBind()
        gdvReserva9.SelectedIndex = -1
        gdvReserva10.DataSource = Nothing
        gdvReserva10.DataBind()
        hddResId.Value = 0
        hddResCaracteristica.Value = " "
        hddResStatus.Value = " "
        cmbResCidade.Visible = True
        txtResCidade.Visible = False
        pnlResponsavelTitulo_CollapsiblePanelExtender.CollapsedText = "Responsável"
        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlResponsavelTitulo.Visible = False
        pnlResponsavelAcao.Visible = False
        pnlIntegranteTitulo.Visible = False
        pnlIntegranteAcao.Visible = False
        pnlEdicaoIntegrante.Visible = False
        btnIntegranteGravar.Visible = False
        btnIntUltimoRegistro.Visible = False
        btnIntegranteExcluir.Visible = False
        imgBtnAlterarCategoria.Visible = False
        imgBtnAlterarMemorando.Visible = False
        imgBtnAlterarPagamento.Visible = False
        imgBtnAlterarRefeicao.Visible = False
        pnlIntegranteEmissivo.Visible = False
        pnlIntegranteHospedagem.Visible = False
        pnlFinanceiroTitulo.Visible = False
        pnlFinanceiroAcao.Visible = False

        txtDataInicialSolicitacao.Focus()
        btnHospedagemNova.Enabled = True
        btnEmissivoNova.Enabled = Session("GrupoEmissivo")
        'Limpando os atrributos que faz repetir a acomodação para o proximo integrante
        With lblAcomodacaoEscolhida.Attributes
            .Remove("lblAcomodacaoEscolhida")
            .Remove("txtHosDataIniSol")
            .Remove("hddHosDataIniSol")
            .Remove("txtHosDataFimSol")
            .Remove("hddHosDataFimSol")
            .Remove("txtHosHoraIniSol")
            .Remove("hddHosHoraIniSol")
            .Remove("txtHosHoraFimSol")
            .Remove("hddHosHoraFimSol")
            .Remove("cmbAcomodacaoCobranca")
            .Remove("hddAcomodacaoCobranca")
        End With
    End Sub

    Protected Sub imgBtnReservaAcaoVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgBtnReservaAcaoVoltar.Click
        If imgBtnNovaReserva.Visible Or Session("GrupoEmissivo") Then
            If hddConsulta.Value = 1 Then
                btnReserva_Click(sender, e)
                gdvReserva1_SelectedIndexChanged(sender, e)
            ElseIf hddConsulta.Value = 2 Then
                btnAcomodacao_Click(sender, e)
                gdvReserva2_SelectedIndexChanged(sender, e)
            ElseIf hddConsulta.Value = 3 Then
                btnIntegrante_Click(sender, e)
                gdvReserva3_SelectedIndexChanged(sender, e)
            ElseIf hddConsulta.Value = 4 Then
                btnFinanceiro_Click(sender, e)
                gdvReserva4_SelectedIndexChanged(sender, e)
            End If

            pnlCabecalho.Visible = True
            pnlReserva.Visible = False
            pnlAcomodacao.Visible = False
            pnlPlanilha.Visible = False
            pnlIntegrante.Visible = False
            pnlFinanceiro.Visible = False

            If pnlReserva.Height.Value = 500 Then
                pnlReserva.Visible = True
            ElseIf pnlAcomodacao.Height.Value = 500 Then
                pnlAcomodacao.Visible = True And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P") And (Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR"))
                pnlPlanilha.Visible = Not pnlAcomodacao.Visible And pnlReserva.Visible
            ElseIf pnlIntegrante.Height.Value = 500 Then
                pnlIntegrante.Visible = True
            ElseIf pnlFinanceiro.Height.Value = 500 Then
                pnlFinanceiro.Visible = True
            Else
                pnlCabecalho.Visible = True
                pnlReserva.Visible = True
                pnlAcomodacao.Visible = True And (cmbTipo.SelectedValue <> "P") And (hddResCaracteristica.Value <> "P") And (Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR"))
                pnlPlanilha.Visible = Not pnlAcomodacao.Visible And pnlReserva.Visible
                pnlIntegrante.Visible = True
                pnlFinanceiro.Visible = True
            End If

            If gdvReserva1.SelectedIndex <> -1 Then
                Try
                    gdvReserva1.SelectedRow.Cells(0).Focus()
                Catch ex As Exception
                    gdvReserva1.Focus()
                End Try
            End If

            If gdvReserva2.SelectedIndex <> -1 Then
                Try
                    gdvReserva2.SelectedRow.Cells(0).Focus()
                Catch ex As Exception
                    gdvReserva2.Focus()
                End Try
            End If
            If gdvReserva3.SelectedIndex <> -1 Then
                Try
                    gdvReserva3.SelectedRow.Cells(0).Focus()
                Catch ex As Exception
                    gdvReserva3.Focus()
                End Try
            End If
            If gdvReserva4.SelectedIndex <> -1 Then
                Try
                    gdvReserva4.SelectedRow.Cells(0).Focus()
                Catch ex As Exception
                    gdvReserva4.Focus()
                End Try
            End If
            pnlReservaAcao.Visible = False
            pnlEdicaoIntegrante.Visible = False
            btnIntegranteGravar.Visible = False
            btnIntUltimoRegistro.Visible = False
            btnIntegranteExcluir.Visible = False
            imgBtnAlterarCategoria.Visible = False
            imgBtnAlterarMemorando.Visible = False
            imgBtnAlterarPagamento.Visible = False
            imgBtnAlterarRefeicao.Visible = False
            pnlIntegranteEmissivo.Visible = False
            pnlIntegranteHospedagem.Visible = False

            'Limpando os atrributos que faz repetir a acomodação para o proximo integrante
            With lblAcomodacaoEscolhida.Attributes
                .Remove("lblAcomodacaoEscolhida")
                .Remove("txtHosDataIniSol")
                .Remove("hddHosDataIniSol")
                .Remove("txtHosDataFimSol")
                .Remove("hddHosDataFimSol")
                .Remove("txtHosHoraIniSol")
                .Remove("hddHosHoraIniSol")
                .Remove("txtHosHoraFimSol")
                .Remove("hddHosHoraFimSol")
                .Remove("cmbAcomodacaoCobranca")
                .Remove("hddAcomodacaoCobranca")
            End With
        Else
            Server.Transfer("~/Recepcao.aspx")
        End If
    End Sub

    Protected Sub txtDataInicialSolicitacao_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataInicialSolicitacao.TextChanged
        If CDate(txtDataFinalSolicitacao.Text) <= CDate(txtDataInicialSolicitacao.Text) Then
            txtDataFinalSolicitacao.Text = Format(CDate(DateAdd(DateInterval.Day, System.Math.Abs(CInt(hddIntervaloSolicitacao.Value)), CDate(txtDataInicialSolicitacao.Text))), "dd/MM/yyyy")
        End If
        hddIntervaloSolicitacao.Value = DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), CDate(txtDataFinalSolicitacao.Text))
        txtDataFinalSolicitacao.Focus()
    End Sub

    Protected Sub txtDataFinalSolicitacao_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataFinalSolicitacao.TextChanged
        If IsDate(txtDataFinalSolicitacao.Text) Then
            hddIntervaloSolicitacao.Value = DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), CDate(txtDataFinalSolicitacao.Text))
        End If
        'If btnHospedagemNova.Enabled Then
        'btnHospedagemNova.Focus()
        'Else
        'btnEmissivoNova.Focus()
        'End If
    End Sub

    Protected Sub btnHospedagemNova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHospedagemNova.Click
        Try
            btnReservaGravar.Attributes.Add("RT", "") 'Limpando o RT - Na reserva que possue apto RT ele nunca pederá ser HospedeJá
            If txtDataInicialSolicitacao.Text.Length = 0 Or txtDataFinalSolicitacao.Text.Length = 0 Then
                txtDataInicialSolicitacao.Text = Format(Date.Now, "dd/MM/yyyy")
                txtDataFinalSolicitacao.Text = DateAdd(DateInterval.Day, 2, CDate(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy")))
            End If

            'Validações referente ao HospedeJá
            If ValidaDataHospedeJa(txtDataInicialSolicitacao.Text, txtDataFinalSolicitacao.Text) = False Then
                gdvReserva6.DataSource = ""
                gdvReserva6.DataBind()
                pnlSolicitacaoSelecionada.Visible = False
                pnlAcomodacaoTitulo.Visible = False
                pnlDisponibilidadeAlternativa.Visible = False
                lblDisponibilidadeAlternativa.Visible = False
                pnlDisponibilidadeAlternativaAux.Visible = False
                Exit Try
            End If

            ProcuraDisponibilidadeSolicitadaeAlternativa()
            hddResCaracteristica.Value = "I"
            pnlSolicitacaoSelecionada_RoundedCornersExtender.Enabled = True
            pnlIntegranteGeral_RoundedCornersExtender.Enabled = True
            pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "False"
            'txtResMatricula.Text = ""
            btnEmissivoNova.Enabled = False

            'Se for Hospedejá não deixará ver Disponibilidades alternativas./ em 15-01-2018 Juliano pediu para liberar a lista de alternativos para o hospedeja
            'If btnHospedagemNova.Attributes.Item("UsuariosCA") = "S" Then
            '    pnlDisponibilidadeAlternativa.Visible = False
            '    lblDisponibilidadeAlternativa.Visible = False
            '    pnlIntegranteGeral_RoundedCornersExtender.Enabled = False
            'End If

            'Saindo do processo de copia/cola do integrante
            imgBtnIntegranteNovoAcao.Attributes.Remove("IntCopiado")

            'Desconsiderei o código abaixo, data de Bloqueio será por padrão sempre o dia de hoje
            txtResDtLimiteRetorno.Text = Format(Now, "dd/MM/yyyy")

            'Calculando a Base para geração do boleto
            'Dim DiferencaEntreInicioReservaEHoje As Integer
            'DiferencaEntreInicioReservaEHoje = DateDiff(DateInterval.Day, CDate(Format(Today, "dd/MM/yyyy")), CDate(txtDataInicialSolicitacao.Text))
            'Select Case DiferencaEntreInicioReservaEHoje
            '    Case Is > 3
            '        txtResDtLimiteRetorno.Text = Format(Now, "dd/MM/yyyy")
            '    Case Is = 2
            '        txtResDtLimiteRetorno.Text = DateAdd(DateInterval.Day, -1, CDate(Format(Now, "dd/MM/yyyy")))
            '    Case Is = 1
            '        txtResDtLimiteRetorno.Text = DateAdd(DateInterval.Day, -2, CDate(Format(Now, "dd/MM/yyyy")))
            '    Case Is = 0
            '        txtResDtLimiteRetorno.Text = DateAdd(DateInterval.Day, -3, CDate(Format(Now, "dd/MM/yyyy")))
            '    Case Else
            '        'Esse caso é que será menor que 1, não poderia gerar Boleto
            '        txtResDtLimiteRetorno.Text = Format(Now, "dd/MM/yyyy")
            'End Select
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
                    "&Erro=Criar uma nova reserva" & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnHospedagemNova")
        End Try
    End Sub

    Protected Sub gdvReserva6_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva7.RowDataBound, gdvReserva8.RowDataBound, gdvReserva6.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            'Na lista alternativa irá esconder o botão de bloquear todos quando não pertencer ao grupo mencionado.
            If sender Is gdvReserva7 And
                     Not Grupos.Contains("Turismo Social Central de Reservas Bloqueio Apartamentos") Then
                CType(e.Row.FindControl("imgSobeTodos"), ImageButton).Visible = False
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            ''Colorindo Apartamentos antido
            'If sender.DataKeys(e.Row.RowIndex).Item("acmStatus") = "G" Then
            '    CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkDiaInicial"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkDtInicial"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkDiaFinal"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkDtFinal"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkHospedes"), LinkButton).ForeColor = Drawing.Color.Green
            '    CType(e.Row.FindControl("lnkDiaria"), LinkButton).ForeColor = Drawing.Color.Green
            'ElseIf sender.DataKeys(e.Row.RowIndex).Item("acmStatus") = "O" Then
            '    CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkDiaInicial"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkDtInicial"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkDiaFinal"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkDtFinal"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkHospedes"), LinkButton).ForeColor = Drawing.Color.Chocolate
            '    CType(e.Row.FindControl("lnkDiaria"), LinkButton).ForeColor = Drawing.Color.Chocolate
            'Else
            '    CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkDiaInicial"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkDtInicial"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkDiaFinal"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkDtFinal"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkHospedes"), LinkButton).ForeColor = Drawing.Color.Red
            '    CType(e.Row.FindControl("lnkDiaria"), LinkButton).ForeColor = Drawing.Color.Red
            'End If

            Dim CorDefinida As System.Drawing.Color
            Dim TipoDefinido As String = ""
            Select Case sender.DataKeys(e.Row.RowIndex).Item("acmFederacao")
                Case "N"
                    CorDefinida = Drawing.Color.Black
                    TipoDefinido = "N"
                Case "E"
                    CorDefinida = Drawing.Color.HotPink
                    TipoDefinido = "E"
                Case "D"
                    'Em Pirenópolis os Flutuantes são do tipo D diferente de Caldas que são do tipo F
                    'Definindo formato da data inicial para efetuar a consulta
                    Dim DataIniConsulta As String, DataFimConsulta As String
                    If txtDataInicialSolicitacao.Text.Trim.Length = 0 Then
                        DataIniConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                    Else
                        DataIniConsulta = (Format(CDate(txtDataInicialSolicitacao.Text), "dd-MM-yyyy"))
                    End If
                    If txtDataFinalSolicitacao.Text.Trim.Length = 0 Then
                        DataFimConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                    Else
                        DataFimConsulta = (Format(CDate(txtDataFinalSolicitacao.Text), "dd-MM-yyyy"))
                    End If

                    'Definindo banco de dados/Tratanto Reserva técnica de Caldas Novas
                    Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")

                        CorDefinida = Drawing.Color.Green
                    Else
                        'Aqui estou tratanto flutuante de Piri
                        objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
                        'Aplicando as cores de acordo com a situação de alta ou baixa temporada
                        Select Case objSituacaoAtualDAO.ConsultaTemporada(DataIniConsulta, DataFimConsulta)
                            Case 1
                                'CorDefinida = Drawing.Color.Purple
                                CorDefinida = Drawing.Color.Black
                            Case 3
                                CorDefinida = Drawing.Color.Red
                            Case Else
                                CorDefinida = Drawing.Color.Black
                        End Select
                    End If
                    TipoDefinido = "D"
                Case "R"
                    CorDefinida = Drawing.Color.Blue
                    TipoDefinido = "R"
                Case "S"
                    CorDefinida = Drawing.Color.Red
                    TipoDefinido = "S"
                Case "F"
                    'Colorindo os apartamentos flutuantes do Wilton e Bloco Anhanguera
                    'Definindo formato da data inicial para efetuar a consulta
                    Dim DataIniConsulta As String, DataFimConsulta As String
                    If txtDataInicialSolicitacao.Text.Trim.Length = 0 Then
                        DataIniConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                    Else
                        DataIniConsulta = (Format(CDate(txtDataInicialSolicitacao.Text), "dd-MM-yyyy"))
                    End If
                    If txtDataFinalSolicitacao.Text.Trim.Length = 0 Then
                        DataFimConsulta = (Format(Now.Date, "dd-MM-yyyy"))
                    Else
                        DataFimConsulta = (Format(CDate(txtDataFinalSolicitacao.Text), "dd-MM-yyyy"))
                    End If

                    'Definindo banco de dados
                    Dim objSituacaoAtualDAO As Turismo.SituacaoAtualDAO
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialCaldas")
                    Else
                        objSituacaoAtualDAO = New Turismo.SituacaoAtualDAO("TurismoSocialPiri")
                    End If

                    'Aplicando as cores de acordo com a situação de alta ou baixa temporada
                    Select Case objSituacaoAtualDAO.ConsultaTemporada(DataIniConsulta, DataFimConsulta)
                        Case 1 'Pacotes mas não da federação
                            'CorDefinida = Drawing.Color.Purple
                            CorDefinida = Drawing.Color.Black
                            TipoDefinido = "F"
                            'Comentado, pois a Pollyana solicitou em 05-06-2017.
                            'CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text = CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text.Replace("( * )", "").Replace("*", "")
                        Case 3 'Pacotes da Federação
                            CorDefinida = Drawing.Color.Red
                            TipoDefinido = "F"
                        Case Else 'Baixa temporada, ou seja, sem pacotes
                            CorDefinida = Drawing.Color.Black
                            TipoDefinido = "F"
                            'CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text = CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text.Replace("( * )", "").Replace("*", "")
                    End Select
            End Select

            'Colorindo definitivamente as linhas do grid com base no case acima
            If sender.DataKeys(e.Row.RowIndex).Item("acmFederacao") = TipoDefinido Then
                CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkDiaInicial"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkDtInicial"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkDiaFinal"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkDtFinal"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkHospedes"), LinkButton).ForeColor = CorDefinida
                CType(e.Row.FindControl("lnkDiaria"), LinkButton).ForeColor = CorDefinida
            End If

            'Quando estiverem no grupo Turismo Social Reserva Central de Atendimentos visualizarão somente os aptos normais ou flutuantes
            If btnHospedagemNova.Attributes.Item("UsuariosCA") = "S" Then
                'If (CorDefinida = Drawing.Color.Purple Or CorDefinida = Drawing.Color.Red) Then 'Alta temporada, ver somente os normais
                If (CorDefinida = Drawing.Color.Red) Then 'Ocutando os flutuantes com TbPacotes Federação
                    If (TipoDefinido <> "N") Then
                        e.Row.Visible = False
                    End If
                Else
                    If (TipoDefinido <> "N" And TipoDefinido <> "F") Then 'baixa temporada, ver os normais e flutuantes
                        e.Row.Visible = False
                    End If
                End If
            End If

            Try
                'A pensar... quando o apto já estivere em estada, não deixar mudar o periodo na acomodação.
                If sender Is gdvReserva8 Then
                    'Consultando para ver se a acomodação esta ocupada ou não, 1 - ocupada 0- Não esta ocupada
                    ObjReservaConsultasDAO = New ReservaConsultasDAO(btnHospedagemNova.Attributes.Item("Conexao"))
                    Dim AptoEmEstada = ObjReservaConsultasDAO.VerificaAptoOcupado(hddResId.Value, sender.DataKeys(e.Row.RowIndex).Item("solId"))

                    CType(e.Row.FindControl("lnkServidor"), LinkButton).Text = sender.DataKeys(e.Row.RowIndex).Item("solUsuario").ToString.Replace("SESC-GO.COM.BR\", "")
                    CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = CType(e.Row.FindControl("lnkDtFinal"), LinkButton).ForeColor
                    If (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) > Now.Date) And (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) = CDate(hddResDataIni.Value)) Then
                        'If (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) > CDate(hddResDataIni.Value)) And AptoEmEstada = 0 Then
                        CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0)
                    Else
                        CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).Visible = False
                    End If
                    If (DateDiff(DateInterval.Day, CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")), CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim"))) > 1) Then
                        CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0) And
                            AptoEmEstada = 0 And
                            ((CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) = CDate(hddResDataIni.Value)) Or
                             (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) < CDate(hddResDataFim.Value))) 'Não tinha essa linha adicionei em 05/08/2015

                        CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0) And
                        (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim")) > CDate(CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni"))) And
                        (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim")) > CDate(hddResDataIni.Value)))

                        'Substitui essas linhas em 05/08/2015
                        '((CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim")) = CDate(hddResDataFim.Value)) Or _
                        ' (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) = CDate(hddResDataIni.Value)))
                    Else
                        CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).Visible = False
                        CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).Visible = False
                    End If
                    'Essa era o valor dar segunda linha abaixo, acresentei o < antes era apenas o =
                    ' ((CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim")) = CDate(hddResDataFim.Value)) Or _
                    CType(e.Row.FindControl("imgBtnDtCheckOutMais"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0) And
                      ((CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataFim")) <= CDate(hddResDataFim.Value)) Or
                       (CDate(sender.DataKeys(e.Row.RowIndex).Item("solDataIni")) = CDate(hddResDataIni.Value)))

                    CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).CommandArgument =
                      gdvReserva8.DataKeys(e.Row.RowIndex).Item(1).ToString() & "#" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(3).ToString() & "$" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(4).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).CommandArgument =
                      gdvReserva8.DataKeys(e.Row.RowIndex).Item(1).ToString() & "#" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(3).ToString() & "$" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(4).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).CommandArgument =
                      gdvReserva8.DataKeys(e.Row.RowIndex).Item(1).ToString() & "#" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(3).ToString() & "$" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(4).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckOutMais"), ImageButton).CommandArgument =
                      gdvReserva8.DataKeys(e.Row.RowIndex).Item(1).ToString() & "#" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(3).ToString() & "$" &
                          gdvReserva8.DataKeys(e.Row.RowIndex).Item(4).ToString()

                    If hddHosId.Value = gdvReserva8.DataKeys(e.Row.RowIndex).Item(1).ToString() Then
                        hddDataInDepois.Value = Format(CDate(gdvReserva8.DataKeys(e.Row.RowIndex).Item(3).ToString()), "dd/MM/yyyy")
                        hddDataOutDepois.Value = Format(CDate(gdvReserva8.DataKeys(e.Row.RowIndex).Item(4).ToString()), "dd/MM/yyyy")
                    End If

                    ''Restrito para usuários da reserva central de atendimentos - HospedeJá
                    'If btnHospedagemNova.Attributes.Item("UsuariosCA") = "S" Then
                    '    'Irá esconder + e - para não deixar adicionar manualmente um dia ou retirar
                    '    e.Row.FindControl("imgBtnDtCheckInMenos").Visible = False
                    '    e.Row.FindControl("imgBtnDtCheckInMais").Visible = False
                    '    e.Row.FindControl("imgBtnDtCheckOutMenos").Visible = False
                    '    e.Row.FindControl("imgBtnDtCheckOutMais").Visible = False
                    'End If

                    'Alternativa
                ElseIf sender Is gdvReserva7 And
                   Not Grupos.Contains("Turismo Social Central de Reservas Bloqueio Apartamentos") Then
                    CType(e.Row.FindControl("chkSobeApto"), CheckBox).Visible = False
                    CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
                    CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).Attributes.Add("onBlur", "javascript:if ((this.value=='') || (this.value > " & CLng(sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")) & ")) {this.value='1';alert('O valor digitado não pode ser maior que \na quantidade de apartamentos livres.');this.focus();}")

                ElseIf hddOrgGrupo.Value <> "F" And
                    Grupos.Contains("Turismo Social Central de Reservas Bloqueio Apartamentos") Then
                    'ElseIf (Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or _
                    ' (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Not Session("GrupoRecepcaoPiri")) Then
                    'Turismo Social Central de Reservas Bloqueio Apartamentos
                    CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).OnClientClick =
                    "return ValidaQtdeAcm(this, " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & ", '" & sender.DataKeys(e.Row.RowIndex).Item("acmDescricao").ToString.Trim & "')"
                    CType(e.Row.FindControl("lnkDiaInicial"), LinkButton).OnClientClick =
                    "return ValidaQtdeAcm(this, " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & ", '" & sender.DataKeys(e.Row.RowIndex).Item("acmDescricao").ToString.Trim & "')"
                    CType(e.Row.FindControl("lnkDtInicial"), LinkButton).OnClientClick =
                    "return ValidaQtdeAcm(this, " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & ", '" & sender.DataKeys(e.Row.RowIndex).Item("acmDescricao").ToString.Trim & "')"
                    CType(e.Row.FindControl("lnkDiaFinal"), LinkButton).OnClientClick =
                     "return ValidaQtdeAcm(this, " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & ", '" & sender.DataKeys(e.Row.RowIndex).Item("acmDescricao").ToString.Trim & "')"
                    CType(e.Row.FindControl("lnkDtFinal"), LinkButton).OnClientClick =
                    "return ValidaQtdeAcm(this, " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & ", '" & sender.DataKeys(e.Row.RowIndex).Item("acmDescricao").ToString.Trim & "')"

                    If CInt(sender.DataKeys(e.Row.RowIndex).Item("acmLimpo")) > CInt(sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")) Then
                        CType(e.Row.FindControl("lblLimpoAcomodacao"), Label).Text = " / " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")
                    Else
                        CType(e.Row.FindControl("lblLimpoAcomodacao"), Label).Text = " / " & sender.DataKeys(e.Row.RowIndex).Item("acmLimpo")
                    End If

                    CType(e.Row.FindControl("lblQtdeAcomodacao"), Label).Text = " / " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")

                    'CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).Attributes.Add("onKeyPress", "javascript:return SoNumero(event);if ((this.value=='') || (this.value > " & CLng(sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")) & ")) {this.value='1';}")
                    CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).Attributes.Add("onKeyPress", "javascript:return SoNumero(event);")
                    CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).Attributes.Add("onBlur", "javascript:if ((this.value=='') || (this.value > " & CLng(sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")) & ")) {this.value='1';alert('O valor digitado não pode ser maior que \na quantidade de apartamentos livres.');this.focus();}")

                    If (sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") > 1) Then
                        'CType(e.Row.FindControl("lnkAcomodacao_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).ConfirmText = "Bloquear a quantidade de acomodação livre da linha?"
                        'CType(e.Row.FindControl("lnkDiaInicial_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).ConfirmText = "Bloquear a quantidade de acomodação livre da linha?"
                        'CType(e.Row.FindControl("lnkDtInicial_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).ConfirmText = "Bloquear a quantidade de acomodação livre da linha?"
                        'CType(e.Row.FindControl("lnkDiaFinal_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).ConfirmText = "Bloquear a quantidade de acomodação livre da linha?"
                        'CType(e.Row.FindControl("lnkDtFinal_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).ConfirmText = "Bloquear a quantidade de acomodação livre da linha?"
                    Else
                        'CType(e.Row.FindControl("lnkAcomodacao_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                        'CType(e.Row.FindControl("lnkDiaInicial_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                        'CType(e.Row.FindControl("lnkDtInicial_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                        'CType(e.Row.FindControl("lnkDiaFinal_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                        'CType(e.Row.FindControl("lnkDtFinal_ConfirmButtonExtender"), AjaxControlToolkit.ConfirmButtonExtender).Enabled = False
                        CType(e.Row.FindControl("txtQtdeAcomodacao"), TextBox).ReadOnly = True
                    End If
                Else
                    CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = True
                    If sender.DataKeys(e.Row.RowIndex).Item("acmLimpo") = "L" Then
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoBranco.gif"
                    ElseIf sender.DataKeys(e.Row.RowIndex).Item("acmLimpo") = "O" Then
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoAzul.gif"
                    ElseIf sender.DataKeys(e.Row.RowIndex).Item("acmLimpo") = "A" Then
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoAmarelo.gif"
                    ElseIf sender.DataKeys(e.Row.RowIndex).Item("acmLimpo") = "M" Then
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).ImageUrl = "~/images/StatusAptoCinza.gif"
                    Else
                        CType(e.Row.FindControl("imgBtnApto"), ImageButton).Visible = False
                    End If
                    sender.Columns(1).Visible = False

                    If sender.DataKeys(e.Row.RowIndex).Item("apaDesc") = "" Then
                        CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text += " " & sender.DataKeys(e.Row.RowIndex).Item("apaDesc") & "(" & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre") & " Disp.)"
                    Else
                        CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text += " " & sender.DataKeys(e.Row.RowIndex).Item("apaDesc")
                    End If
                    'CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Text += " " & sender.DataKeys(e.Row.RowIndex).Item("aptosLivre")
                End If
            Catch ex As Exception

            End Try
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            objReservaListagemIntegranteVO = New ReservaListagemIntegranteVO

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If
            'Totalizando as acomodações
            If sender Is gdvReserva8 Then
                gdvReserva8.ShowFooter = (gdvReserva8.Rows.Count > 3)
                If gdvReserva8.Rows.Count > 3 Then
                    objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.SomaAcomodacaoReserva(hddResId.Value)
                    e.Row.Cells(0).Text = objReservaListagemIntegranteVO.acmDescricao
                End If
            End If
        End If
    End Sub

    Protected Sub gdvReserva8_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva8.SelectedIndexChanged
        Dim objDeletaSolicitacaoHospedagemDAO As Turismo.DeletaSolicitacaoHospedagemDAO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objDeletaSolicitacaoHospedagemDAO = New Turismo.DeletaSolicitacaoHospedagemDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objDeletaSolicitacaoHospedagemDAO = New Turismo.DeletaSolicitacaoHospedagemDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If

        Dim DataSolIniAnt As String = "", DataSolFimAnt As String = ""
        If objDeletaSolicitacaoHospedagemDAO.deletaSolicitacaoHospedagem(
          sender.DataKeys(sender.SelectedIndex).Item(1).ToString,
          "N", User.Identity.Name.Replace("SESC-GO.COM.BR\", "")) <> "0" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível excluir a acomodação. Verifique se não existe integrantes vinculados a ela." + "');", True)
        Else
            objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
            CarregaCmbRefeicaoPrato()
            DataSolIniAnt = Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy")
            DataSolFimAnt = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")
            CarregaDadosReserva()
            txtDataInicialSolicitacao.Text = DataSolIniAnt
            txtDataFinalSolicitacao.Text = DataSolFimAnt
        End If
        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
        If lista.Count = 0 Then
            imgBtnReservaNova_Click(sender:=Nothing, e:=Nothing)
        Else
            gdvReserva8.DataSource = lista
            gdvReserva8.DataBind()
            gdvReserva8.SelectedIndex = -1
            'btnHospedagemNova_Click(sender, e) - Comentado: Uma reserva que era Caracteristica "E" por exemplo, com esse evento se tornava I de individual
        End If
        If hddIntId.Value <> "" Then
            lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, hddIntId.Value)
            gdvReserva11.DataSource = lista
            gdvReserva11.DataBind()
            gdvReserva11.SelectedIndex = -1
            'Atualiza a lista de disponibilidade Grid6
            ProcuraDisponibilidade()
            'Atualiza a lista de disponibilidade Grid7
            ProcuraDisponibilidadeSolicitadaeAlternativa()
        End If
    End Sub

    Protected Sub gdvReserva6_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva6.SelectedIndexChanged, gdvReserva7.SelectedIndexChanged
        Dim objLotacaoDAO As New FPW.LotacaoDAO
        Dim objLotacaoVO As New FPW.LotacaoVO
        Dim objTestaGrupo As New Uteis.TestaUsuario

        Dim objInsereSolicitacaoHospedagemDAO As Turismo.InsereSolicitacaoHospedagemDAO
        Dim Banco As String = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            Banco = "DbGeralCaldas"
            'objReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
        Else
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            Banco = "DbGeralPiri"
            'objReservaDAO = New Turismo.ReservaDAO( "TurismoSocialPiri")
        End If

        If hddOrgGrupo.Value = "F" Then
            objLotacaoVO.loCodLot = hddOrgLotacao.Value
        Else
            lista = objLotacaoDAO.consultarReserva(Banco, objTestaGrupo.listaInitials(User.Identity.Name.Replace("SESC-GO.COM.BR\", "")))
            objLotacaoVO = lista.Item(0)
        End If

        Dim varResId As String
        'sender.DataKeys(sender.SelectedIndex).Item(2).ToString, _
        'sender.DataKeys(sender.SelectedIndex).Item(4).ToString, _

        'Antes estava SelectedValue = 'P' só que presidencia agora é "S" de fecomércio
        If (hddOrgGrupo.Value = "F") Or cmbHospedagem.SelectedValue = "S" Or
            (((Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or
            (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Session("GrupoRecepcaoPiri"))) And
            (CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString) = Now.Date)) Then
            varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
              hddResId.Value.ToString, sender.DataKeys(sender.SelectedIndex).Item(0).ToString,
              sender.DataKeys(sender.SelectedIndex).Item("apaId").ToString,
              Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString), "dd/MM/yyyy"),
              Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(3).ToString), "dd/MM/yyyy"), "12", "12",
              "A",
              User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
              objLotacaoVO.loCodLot.ToString)
        Else
            varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
              hddResId.Value.ToString, sender.DataKeys(sender.SelectedIndex).Item(0).ToString,
              sender.DataKeys(sender.SelectedIndex).Item("apaId").ToString,
              Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString), "dd/MM/yyyy"),
              Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(3).ToString), "dd/MM/yyyy"), "12", "12",
              "C",
              User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
              objLotacaoVO.loCodLot.ToString)
        End If

        Dim DataSolIniAnt As String = "", DataSolFimAnt As String = ""

        If varResId = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar a operação." + "');", True)
        Else
            If hddResId.Value <> varResId Then
                hddResId.Value = varResId
                hddIntId.Value = "0"
                objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                CarregaCmbRefeicaoPrato()
                DataSolIniAnt = Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy")
                DataSolFimAnt = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")
                CarregaDadosReserva()
                txtDataInicialSolicitacao.Text = DataSolIniAnt
                txtDataFinalSolicitacao.Text = DataSolFimAnt
                pnlResponsavelTitulo.Visible = True
                pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
                pnlResponsavelAcao.Visible = True
            ElseIf hddResId.Value = varResId Then
                objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                CarregaCmbRefeicaoPrato()
                DataSolIniAnt = Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy")
                DataSolFimAnt = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")
                CarregaDadosReserva()
                txtDataInicialSolicitacao.Text = DataSolIniAnt
                txtDataFinalSolicitacao.Text = DataSolFimAnt
            End If
            While CType(sender.SelectedRow.FindControl("txtQtdeAcomodacao"), TextBox).Text > 1
                varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                  hddResId.Value.ToString, sender.DataKeys(sender.SelectedIndex).Item(0).ToString, "",
                  Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString), "dd/MM/yyyy"),
                  Format(CDate(sender.DataKeys(sender.SelectedIndex).Item(3).ToString), "dd/MM/yyyy"), "12", "12",
                  "C",
                  User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                  objLotacaoVO.loCodLot.ToString)
                CType(sender.SelectedRow.FindControl("txtQtdeAcomodacao"), TextBox).Text -= 1
            End While
        End If

        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
        hddSolId.Value = DirectCast(lista.Item(0), Turismo.ReservaListagemAcomodacaoVO).solId
        gdvReserva8.DataSource = lista
        gdvReserva8.DataBind()
        gdvReserva8.SelectedIndex = -1
        btnHospedagemNova_Click(sender, e)
        pnlSolicitacaoSelecionada.Visible = True
        pnlAcomodacaoTitulo.Visible = True
        lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, 0) 'hddIntId.Value)
        gdvReserva11.DataSource = lista
        gdvReserva11.DataBind()
        gdvReserva11.SelectedIndex = -1
    End Sub

    Protected Sub lnkResponsavel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If
        Try
            hddResId.Value = sender.CommandArgument.ToString
        Catch ex As Exception

        End Try

        pnlSolicitacaoSelecionada_RoundedCornersExtender.Enabled = True
        pnlIntegranteGeral_RoundedCornersExtender.Enabled = True

        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
        CarregaCmbResIdWeb()
        CarregaEstado()
        CarregaFormaPagto()
        CarregaCmbDestino()
        CarregaCmbRefeicaoPrato()
        CarregaDadosReserva()
        ListaIntegranteViaResId()
        ListaFinanceiroViaResId()
        pnlResponsavelTitulo.Visible = True
        pnlResponsavelAcao.Visible = True

        If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("CF", hddResStatus.Value) = 0) Then 'Hospedagem
            ProcuraDisponibilidade()
            'pnlSolicitacaoSelecionada.Visible = True
            btnReservaGravar.Enabled = True
            btnReservaGravar.Visible = True
            btnReservaReativar.Enabled = False
            btnReservaReativar.Visible = False
            btnInformarRestituicao.Enabled = False
            btnInformarRestituicao.Visible = False
            btnReservaCancelar.Enabled = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaCancelar.Visible = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            Habilitar(pnlResponsavelAcao)
            pnlGrupo.Visible = (InStr("ET", hddResCaracteristica.Value) <> 0)
            'Inserido para atender pedido da Pollyana.
            '
            lblResCatCobranca.Visible = hddResCaracteristica.Value <> "I"
            cmbResCatCobranca.Visible = hddResCaracteristica.Value <> "I"

            pnlDestinoGrupo.Visible = False
            lblResHoraSaida.Visible = True
            cmbReservaHoraSaida.Visible = True
            'txtResNome.Focus()
            txtResMatricula.Focus()
        ElseIf (InStr("IET", hddResCaracteristica.Value) = 0) And (InStr("CFE", hddResStatus.Value) = 0) Then 'Emissivo
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
                btnReservaGravar.Enabled = True
                btnReservaGravar.Visible = True
                btnReservaCalculo.Visible = True
                btnReservaReativar.Enabled = False
                btnReservaReativar.Visible = False
                btnInformarRestituicao.Enabled = False
                btnInformarRestituicao.Visible = False
                btnReservaCancelar.Enabled = (hddResId.Value <> "0")
                btnReservaCancelar.Visible = (hddResId.Value <> "0")
                Habilitar(pnlResponsavelAcao)
            End If

            'Usuários das centrais de atendimentos não poderão salvar os dados do responsável de reserva, Emissivo ou Excursão            
            If Session("GrupoCentralAtendimento") And (hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "P" Or hddResCaracteristica.Value = "T") Then
                btnReservaGravar.Enabled = False
                btnReservaCancelar.Enabled = False
                btnReservaReativar.Enabled = False
                btnInformarRestituicao.Enabled = False
                btnReservaCalculo.Enabled = False
                'Inibindo as ações de integrantes
                btnIntegranteGravar.Enabled = False
                imgBtnIntegranteNovoAcao.Enabled = False
                btnIntegranteExcluir.Enabled = False
            End If

            pnlGrupo.Visible = False
            pnlDestinoGrupo.Visible = True
            lblResHoraSaida.Visible = False
            cmbReservaHoraSaida.Visible = False
            'txtResNome.Focus()
            txtResMatricula.Focus()
        Else
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            btnReservaGravar.Enabled = False
            btnReservaGravar.Visible = False
            btnReservaCalculo.Visible = False
            btnReservaCancelar.Enabled = False
            btnReservaCancelar.Visible = False
            If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("C", hddResStatus.Value) = 1) Then 'Hospedagem Cancelada
                btnReservaReativar.Enabled = True
                btnReservaReativar.Visible = True
                btnInformarRestituicao.Enabled = True
                btnInformarRestituicao.Visible = True
            End If
            Desabilitar(pnlResponsavelAcao)
            imgBtnReservaAcaoVoltar.Focus()
        End If
        pnlReservaAcao.Visible = True
        pnlIntegranteTitulo.Visible = True
        pnlIntegranteAcao.Visible = True
        pnlFinanceiroTitulo.Visible = True
        pnlFinanceiroAcao.Visible = True

        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "False"
        pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlFinanceiroTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlCabecalho.Visible = False
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False

        'Grupo de passeio só terá permissão para alterar os dados quem pertencer ao grupo "Turismo Social Acao Passeio"
        If (cmbTipo.SelectedValue = "P" And
         Not Grupos.Contains("Turismo Social Acao Passeio")) Then
            pnlResponsavelAcao.Visible = False
            pnlResponsavelTitulo.Visible = False
        Else
            pnlResponsavelAcao.Visible = True
            pnlResponsavelTitulo.Visible = True
        End If
    End Sub

    Protected Sub lnkAcomodacao_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            'objReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            'objReservaDAO = New Turismo.ReservaDAO( "TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If
        Try
            'hddResId.Value = sender.CommandArgument.ToString
        Catch ex As Exception

        End Try
        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)

        CarregaCmbResIdWeb()
        CarregaEstado()
        CarregaFormaPagto()
        CarregaCmbDestino()
        CarregaCmbRefeicaoPrato()
        CarregaDadosReserva()
        ListaIntegranteViaResId()
        ListaFinanceiroViaResId()
        pnlResponsavelTitulo.Visible = True
        pnlResponsavelAcao.Visible = True

        If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("CF", hddResStatus.Value) = 0) Then 'Hospedagem
            ProcuraDisponibilidade()
            'pnlSolicitacaoSelecionada.Visible = True
            btnReservaGravar.Enabled = True
            btnReservaGravar.Visible = True
            btnReservaCancelar.Enabled = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaCancelar.Visible = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaReativar.Enabled = False
            btnReservaReativar.Visible = False
            btnInformarRestituicao.Enabled = False
            btnInformarRestituicao.Visible = False
            Habilitar(pnlResponsavelAcao)
            'txtResNome.Focus()
            txtResMatricula.Focus()
        ElseIf (InStr("IET", hddResCaracteristica.Value) = 0) And (InStr("CFE", hddResStatus.Value) = 0) Then 'Emissivo
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
                btnReservaGravar.Enabled = True
                btnReservaGravar.Visible = True
                btnReservaCalculo.Visible = True
                btnReservaCancelar.Enabled = (hddResId.Value <> "0")
                btnReservaCancelar.Visible = (hddResId.Value <> "0")
                btnReservaReativar.Enabled = False
                btnReservaReativar.Visible = False
                btnInformarRestituicao.Enabled = False
                btnInformarRestituicao.Visible = False
                Habilitar(pnlResponsavelAcao)
            End If
            'txtResNome.Focus()
            txtResMatricula.Focus()
        Else
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            btnReservaGravar.Enabled = False
            btnReservaGravar.Visible = False
            btnReservaCalculo.Visible = False
            btnReservaCancelar.Enabled = False
            btnReservaCancelar.Visible = False
            If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("C", hddResStatus.Value) = 1) Then 'Hospedagem Cancelada
                btnReservaReativar.Enabled = True
                btnReservaReativar.Visible = True
                btnInformarRestituicao.Enabled = True
                btnInformarRestituicao.Visible = True
            End If
            Desabilitar(pnlResponsavelAcao)
            imgBtnReservaAcaoVoltar.Focus()
        End If

        pnlSolicitacaoSelecionada.Visible = True
        pnlAcomodacaoTitulo.Visible = True

        pnlReservaAcao.Visible = True
        pnlIntegranteTitulo.Visible = True
        pnlIntegranteAcao.Visible = True
        pnlFinanceiroTitulo.Visible = True
        pnlFinanceiroAcao.Visible = True
        If sender Is Nothing Then
        Else
            pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "False"
            pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
            pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "True"
            pnlFinanceiroTitulo_CollapsiblePanelExtender.ClientState = "True"
        End If
        pnlCabecalho.Visible = False
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False
    End Sub

    Protected Sub gdvReserva2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva2.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            cont = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).CommandArgument =
              gdvReserva2.DataKeys(e.Row.RowIndex).Item(1).ToString()
            CType(e.Row.FindControl("lnkServidor"), LinkButton).Text =
              gdvReserva2.DataKeys(e.Row.RowIndex).Item("solUsuario").ToString.Replace("SESC-GO.COM.BR\", "").Replace(".", " ")
            If gdvReserva2.DataKeys(e.Row.RowIndex).Item("solExcluido") = "S" Then
                CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkAcomodacao"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDiaCheckInAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDiaCheckInAcomodacao"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDataCheckInAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDataCheckInAcomodacao"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDiaCheckOutAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDiaCheckOutAcomodacao"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkDataCheckOutAcomodacao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkDataCheckOutAcomodacao"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkServidor"), LinkButton).Font.Strikeout = True
            End If
            cont += 1
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(4).Text = cont
            lblAcomodacao.Text = "Acomodações " & cont.ToString
        End If
    End Sub

    Protected Sub cmbEstId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEstId.SelectedIndexChanged
        If cmbEstId.SelectedValue < 1000 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
            Else
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
            End If
            'Só será permitido inserir um conveniado se ele for do estado de Goiás.
            If cmbEstId.SelectedValue <> 9 And cmbResCatId.SelectedValue = 3 Then
                cmbResCatId.SelectedIndex = 0
                cmbEstId.SelectedIndex = 0
                cmbResCidade.SelectedIndex = 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A categoria de conveniado só poderá estar vinculada as reservas do estado de Goiás.\n\nVerifique por favor o Estado/Categoria.');", True)
                cmbResCatId.Focus()
                Return
            End If


            lista = objMunicipioDAO.consultarCidadePorEstado(cmbEstId.SelectedValue)
            cmbResCidade.DataSource = lista
            cmbResCidade.DataBind()
            cmbResCidade.Visible = True
            cmbResCidade.Focus()
            txtResCidade.Visible = False
            txtResCidade.Text = Mid(cmbResCidade.SelectedItem.Text.Trim, 1, 40)
        Else
            cmbResCidade.Visible = False
            txtResCidade.Visible = True
            txtResCidade.Text = ""
            txtResCidade.Focus()
            txtResCep.Text = "00000 000"
        End If
    End Sub

    Protected Sub cmbResCidade_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbResCidade.TextChanged
        txtResCidade.Text = Mid(cmbResCidade.SelectedItem.Text.Trim, 1, 40)
        cmbResCidade.Focus()
    End Sub

    Protected Sub cmbDestino_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDestino.SelectedIndexChanged
        If cmbDestino.SelectedValue = "0" Then
            lblDestinoEstado.Visible = True
            cmbDestinoEstado.Visible = True
            cmbDestinoEstado.Focus()
            lblDestinoCidade.Visible = True
            cmbDestinoCidade.Visible = True
            lblDestinoHotel.Visible = True
            txtResHoteDestino.Visible = True
        Else
            lblDestinoEstado.Visible = False
            cmbDestinoEstado.Visible = False
            lblDestinoCidade.Visible = False
            cmbDestinoCidade.Visible = False
            lblDestinoHotel.Visible = False
            txtResHoteDestino.Visible = False
            cmbDestino.Focus()
        End If
        If cmbDestino.SelectedValue = "S" And hddResCaracteristica.Value = "P" Then
            lblPratoRapido.Visible = True
            cmbPratoRapido.Visible = True
            lblPratoRapido0.Visible = True
            cmbPratoRapido0.Visible = True
            'gdvReserva9.Columns(8).Visible = True
            gdvReserva9.Columns(9).Visible = True
        Else
            lblPratoRapido.Visible = False
            cmbPratoRapido.Visible = False
            'cmbPratoRapido.Text = "0"
            lblPratoRapido0.Visible = False
            cmbPratoRapido0.Visible = False
            'cmbPratoRapido0.Text = "0"
            'gdvReserva9.Columns(8).Visible = False
            gdvReserva9.Columns(9).Visible = False
        End If
    End Sub

    Protected Sub cmbDestinoEstado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDestinoEstado.SelectedIndexChanged
        If cmbDestinoEstado.SelectedValue < 1000 Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
            Else
                objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
            End If
            lista = objMunicipioDAO.consultarCidadePorEstado(cmbDestinoEstado.SelectedValue)
            cmbDestinoCidade.DataSource = lista
            cmbDestinoCidade.DataBind()
            cmbDestinoCidade.Focus()
        End If
    End Sub
    Protected Sub btnReservaGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReservaGravar.Click, btnReservaCancelar.Click, btnReservaReativar.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""
        Try
            If txtResDtNascimento.Text = "" Or txtResDtNascimento.Text = "__/__/____" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar a data de nascimento.');", True)
                Exit Try
            End If

            If txtResDtLimiteRetorno.Text = "" Or txtResDtLimiteRetorno.Text = "__/__/____" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar a data de bloqueio.');", True)
                Exit Try
            ElseIf ((DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), DateAdd(DateInterval.Day, 3, CDate(txtResDtLimiteRetorno.Text))) > 0) _
                And txtResDtLimiteRetorno.Text.Length > 0) Then
                If Not (sender Is btnReservaCancelar) = True Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Com a data informada, o boleto de pagamento será gerado com a data vencida.\n\nVerifique por favor.\n\nVencimento do Boleto: " & Format(CDate(DateAdd(DateInterval.Day, 3, CDate(txtResDtLimiteRetorno.Text))), "dd/MM/yyyy") & "\nData Inicial: " & Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy") & "');", True)
                    'txtResDtLimiteRetorno.Focus()
                    'Exit Try
                End If
            End If



            If txtResMemorando.Text.Trim.Length > 0 And cmbResEmissor.SelectedIndex = 0 Then
                Mensagem("O Campo memorando foi preenchido, selecione o Emissor para prosseguir.")
                cmbResEmissor.Focus()
                Exit Try
            End If
            'Verificando se o valores de matrícula e cpf são númericos
            If txtResMatricula.Text.Trim.Length > 0 Then
                If Not IsNumeric(txtResMatricula.Text.Replace(" ", "")) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela deve ser composta de apenas números.');", True)
                    txtResMatricula.Focus()
                    Exit Try
                ElseIf txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length < 11 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela precisa conter no mínimo 11 caracteres.');", True)
                    txtResMatricula.Focus()
                    Exit Try
                End If
            End If

            If txtResCPF.Text.Trim.Length > 0 Then
                If Not IsNumeric(txtResCPF.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "")) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O CPF informado não é válido, ele deve ser composto de apenas números.');", True)
                    txtResCPF.Focus()
                    Exit Try
                ElseIf ckbResponsavel.Checked And
                txtResCPF.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "").Replace("\", "") <= 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Não é possível exportar responsável sem o número do CPF.\n\nFavor informar o CPF para continuar.');", True)
                    txtResCPF.Focus()
                    Exit Try
                End If
            End If

            'CPF  de preenchimento obrigatório para maiores de 18 anos com excessão da Presidência e outros Paises.
            If hddIdade.Value = 0 Then
                hddIdade.Value = calculaIdade(CDate(txtResDtNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
            End If

            Dim Idade As Integer = calculaIdade(CDate(txtResDtNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
            If Idade < 18 Then
                Mensagem("Idade inválida!\n\n O responsável pela reserva tem que ser maior de idade.")
                txtResDtNascimento.Focus()
                Return
            End If

            'Quando for de outros paises, não irá obrigar a digitar o CPF mas terá que informar algum documento como outros.
            If ((hddIdade.Value >= 18 And txtResCPF.Text.Trim.Length = 0 And cmbOrgId.SelectedValue <> "37" And hddResCaracteristica.Value <> "P" And cmbEstId.SelectedValue < 1000) And sender Is btnReservaGravar) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Informe o CPF/CNPJ, pois o integrante possui mais de 18 anos.');", True)
                txtResCPF.Focus()
                Return
            End If
            'Quando for de outros Paises setar o CEP como 00000 000
            If cmbEstId.SelectedValue > 1000 Then
                txtResCep.Text = "00000 000"
            End If

            'Só será permitido inserir um conveniado se ele for do estado de Goiás.
            If cmbEstId.SelectedValue <> 9 And cmbResCatId.SelectedValue = 3 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A categoria de conveniado só poderá estar vinculada as reservas do estado de Goiás.\n\nVerifique por favor o Estado/Categoria.');", True)
                cmbResCatId.Focus()
                Exit Try
            End If
            'Faixa de comerciário/Dependente ou Conveniado somente com matrícula
            If (txtResMatricula.Text.Trim.Length = 0 _
                    And cmbResCatId.SelectedValue <> 4 _
                    And hddResCaracteristica.Value <> "P" _
                    And (Not sender Is btnReservaCancelar) _
                    And cmbOrgId.SelectedValue <> "S" And cmbOrgId.SelectedValue <> "6" And cmbOrgId.SelectedValue <> "3" And cmbOrgId.SelectedValue <> "4" _
                    And (Not sender Is btnReservaReativar
                ) And (Not sender Is btnInformarRestituicao)) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! É obrigatório inserir o número da matrícula do comerciário para a categoria selecionada.');", True)
                txtResMatricula.Focus()
                Exit Try
            End If

            If (sender Is btnReservaGravar) = True Then
                'Preenchimento obrigatório para impressão dos boletos'
                If txtResEmail.Text.Trim.Length = 0 Then
                    Mensagem("Faltou infomar o E-Mail.")
                    txtResEmail.Focus()
                    Exit Try
                Else
                    If EmailAddressCheck(txtResEmail.Text.Trim) = False Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O E-mail informado não é válido!');", True)
                        txtResEmail.Focus()
                        'Else
                        '    btnConfirmarDados.Enabled = True
                    End If
                End If
                'Obrigando a digitação do endereço completo para emissão do boleto
                If txtResLogradouro.Text.Trim.Length = 0 Then
                    Mensagem("Faltou infomar o Logradouro.")
                    txtResLogradouro.Focus()
                    Exit Try
                End If
                If ((txtResNumero.Text = "0" Or txtResNumero.Text.Trim.Length = 0) _
                    And txtResQuadra.Text.Trim.Length = 0 _
                    And txtResLote.Text.Trim.Length = 0 _
                    And txtResComplemento.Text.Trim.Length = 0) Then
                    Mensagem("Um dos campos (Numero, Quadra, Lote ou complemento) terá que ser informado.")
                    Exit Try
                End If

                If txtResBairro.Text.Trim.Length = 0 Then
                    Mensagem("Faltou infomar o Bairro.")
                    txtResBairro.Focus()
                    Exit Try
                End If
                If txtResCep.Text.Trim.Length = 0 Then
                    Mensagem("Faltou infomar o CEP.")
                    txtResCep.Focus()
                    Exit Try
                ElseIf txtResCep.Text.Trim.Replace(" ", "").Replace(".", "").Replace("-", "").Length < 8 Then
                    Mensagem("CEP informado inválido! Digite novamente.\n\nObs.: Precisa contém 8 caracteres númerico.")
                    txtResCep.Focus()
                    Exit Try
                Else
                    txtResCep.Text = Mid(txtResCep.Text.Trim, 1, 9)
                End If
                If txtResCidade.Text.Trim.Length = 0 Then
                    Mensagem("Faltou infomar a Cidade.")
                    txtResCidade.Focus()
                    Exit Try
                End If
            End If
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
            Else
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialPiri")
            End If
            If (sender Is btnReservaGravar) Or (sender Is btnResEmailGrupo) Then
                objReservaListagemSolicitacaoVO.resId = hddResId.Value
            Else
                objReservaListagemSolicitacaoVO.resId = -hddResId.Value
            End If

            If (sender Is btnReservaCancelar) = True Then
                If txtResObs.Text.Trim = "" Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Para cancelar a reserva será necessário informar o motivo no campo Observação.');", True)
                    txtResObs.Focus()
                    Return
                Else
                    ObjReservaVO = New Turismo.ReservaVO
                    Select Case ObjReservaDAO.InsereObservacaoCancelaReserva(Mid(txtResObs.Text.Trim, 1, 600), hddResId.Value)
                        Case 0
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Erro ao gravar o motivo do cancelamento da reserva.');", True)
                            txtResObs.Focus()
                            Exit Try
                    End Select
                End If
            End If

            'Quando for grupo de passeio, apenas um grupo seleto poderá alterar os dados
            If hddResCaracteristica.Value = "P" And hddResId.Value > 0 Then
                If Not Grupos.Contains("Turismo Social Acao Passeio") Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Você não possui permissão para alterar dados dos responsável pelo passeio.');", True)
                    Exit Try
                End If
            End If

            objReservaListagemSolicitacaoVO.resNome = Mid(txtResNome.Text.Trim, 1, 100).Replace("'", "")
            objReservaListagemSolicitacaoVO.resDataIni = Format(CDate(txtDataInicialSolicitacao.Text.Trim), "dd/MM/yyyy")
            objReservaListagemSolicitacaoVO.resDataFim = Format(CDate(txtDataFinalSolicitacao.Text.Trim), "dd/MM/yyyy")

            If hddResCaracteristica.Value = "P" Then
                objReservaListagemSolicitacaoVO.orgId = 1
                objReservaListagemSolicitacaoVO.resCaracteristica = "P" 'Passeio
            ElseIf cmbOrgId.SelectedValue = "S" Then
                If hddResCaracteristica.Value = "T" Then
                    objReservaListagemSolicitacaoVO.resCaracteristica = "T" 'Pacote
                    objReservaListagemSolicitacaoVO.resTipo = "T"
                Else
                    objReservaListagemSolicitacaoVO.resCaracteristica = "E" 'Excursão
                    objReservaListagemSolicitacaoVO.resTipo = "E"
                End If
            Else
                objReservaListagemSolicitacaoVO.orgId = cmbOrgId.SelectedValue
                objReservaListagemSolicitacaoVO.resCaracteristica = "I" 'Individual
            End If
            hddResCaracteristica.Value = objReservaListagemSolicitacaoVO.resCaracteristica

            If cmbOrgId.SelectedValue <> "S" Then
                objReservaListagemSolicitacaoVO.orgId = cmbOrgId.SelectedValue
            Else
                objReservaListagemSolicitacaoVO.orgId = 1
                cmbOrgId.SelectedValue = "S"
            End If


            objReservaListagemSolicitacaoVO.resDtLimiteRetorno = Format(CDate(txtResDtLimiteRetorno.Text.Trim), "dd/MM/yyyy") &
              " " & cmbResHrLimiteRetorno.Text.Trim & ":00"

            objReservaListagemSolicitacaoVO.resMatricula = txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "")
            objReservaListagemSolicitacaoVO.resCPF_CGC = txtResCPF.Text.Trim.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "")
            objReservaListagemSolicitacaoVO.resRG = Mid(cmbResDocIdentificacao.SelectedValue & " - " & txtResDocIdentificacao.Text.Trim.Replace("'", ""), 1, 25)
            objReservaListagemSolicitacaoVO.resContato = Mid(txtResContato.Text.Trim, 1, 50).Replace("'", "")
            objReservaListagemSolicitacaoVO.resEmail = Mid(txtResEmail.Text.Trim, 1, 40).Replace("'", "")
            objReservaListagemSolicitacaoVO.resDtNascimento = Format(CDate(txtResDtNascimento.Text.Trim), "dd/MM/yyyy")
            objReservaListagemSolicitacaoVO.resSexo = cmbResSexo.Text.Trim
            objReservaListagemSolicitacaoVO.catId = cmbResCatId.SelectedValue
            objReservaListagemSolicitacaoVO.resEstadoCivil = "0"

            objReservaListagemSolicitacaoVO.resFoneComercial = Mid(txtResFoneComercial.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(") ", "").Trim, 1, 11)
            objReservaListagemSolicitacaoVO.resFoneResidencial = Mid(txtResFoneResidencial.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(") ", "").Trim, 1, 11)
            objReservaListagemSolicitacaoVO.resCelular = Mid(txtResCelular.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(") ", "").Trim, 1, 11)
            objReservaListagemSolicitacaoVO.resFax = Mid(txtResFax.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace(") ", "").Trim, 1, 11)
            objReservaListagemSolicitacaoVO.estId = cmbEstId.SelectedValue
            objReservaListagemSolicitacaoVO.resCidade = Mid(txtResCidade.Text.Trim, 1, 40).Replace("'", "")
            If txtResCidade.Text Like "GOI?NIA" Then
                objReservaListagemSolicitacaoVO.resCapitalGoias = "S"
            Else
                objReservaListagemSolicitacaoVO.resCapitalGoias = "N"
            End If
            objReservaListagemSolicitacaoVO.resLogradouro = Mid(txtResLogradouro.Text.Trim, 1, 40).Replace("'", "")
            objReservaListagemSolicitacaoVO.resNumero = Mid(txtResNumero.Text.Trim, 1, 10)
            objReservaListagemSolicitacaoVO.resQuadra = Mid(txtResQuadra.Text.Trim, 1, 10).Replace("'", "")
            objReservaListagemSolicitacaoVO.resLote = Mid(txtResLote.Text.Trim, 1, 10).Replace("'", "")
            objReservaListagemSolicitacaoVO.resComplemento = Mid(txtResComplemento.Text.Trim, 1, 40).Replace("'", "")
            objReservaListagemSolicitacaoVO.resBairro = Mid(txtResBairro.Text.Trim, 1, 40).Replace("'", "")
            objReservaListagemSolicitacaoVO.resSalario = cmbResSalario.SelectedValue
            objReservaListagemSolicitacaoVO.resEscolaridade = cmbResEscolaridade.SelectedValue
            objReservaListagemSolicitacaoVO.resEstadoCivil = cmbResEstadoCivil.SelectedValue
            objReservaListagemSolicitacaoVO.resCep = Mid(txtResCep.Text.Trim.Replace(" ", ""), 1, 8)
            objReservaListagemSolicitacaoVO.resMemorando = Mid(txtResMemorando.Text.Trim, 1, 100).Replace("'", "")
            objReservaListagemSolicitacaoVO.resEmissor = cmbResEmissor.SelectedValue
            objReservaListagemSolicitacaoVO.resObs = Mid(txtResObs.Text.Trim, 1, 600).Replace("'", "")
            If IsDate(txtResDtGrupoConfirmacao.Text) Then
                objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = Format(CDate(txtResDtGrupoConfirmacao.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaListagemSolicitacaoVO.resDtGrupoConfirmacao = "Null"
            End If
            If IsDate(txtResDtGrupoPgtoSinal.Text) Then
                objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal = Format(CDate(txtResDtGrupoPgtoSinal.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaListagemSolicitacaoVO.resDtGrupoPgtoSinal = "Null"
            End If
            If IsDate(txtResDtGrupoListagem.Text) Then
                objReservaListagemSolicitacaoVO.resDtGrupoListagem = Format(CDate(txtResDtGrupoListagem.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaListagemSolicitacaoVO.resDtGrupoListagem = "Null"
            End If
            If IsDate(txtResDtGrupoPgtoTotal.Text) Then
                objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal = Format(CDate(txtResDtGrupoPgtoTotal.Text.Trim), "dd/MM/yyyy")
            Else
                objReservaListagemSolicitacaoVO.resDtGrupoPgtoTotal = "Null"
            End If
            objReservaListagemSolicitacaoVO.resIdWeb = cmbResIdWeb.SelectedValue
            objReservaListagemSolicitacaoVO.resCafe = "S"
            objReservaListagemSolicitacaoVO.resAlmoco = cmbDestino.SelectedValue
            objReservaListagemSolicitacaoVO.resJantar = "N"
            objReservaListagemSolicitacaoVO.resFormaPagtoCafe = "ER"
            objReservaListagemSolicitacaoVO.resFormaPagtoAlmoco = "ER"
            objReservaListagemSolicitacaoVO.resFormaPagtoJantar = "ER"
            objReservaListagemSolicitacaoVO.resCatCobranca = cmbResCatCobranca.SelectedValue
            objReservaListagemSolicitacaoVO.resValorDesconto = "0" + txtResValorDesconto.Text.Trim
            objReservaListagemSolicitacaoVO.estIdDes = cmbDestinoEstado.SelectedValue
            objReservaListagemSolicitacaoVO.resHotelExcursao = Mid(txtResHoteDestino.Text.Trim, 1, 40)
            objReservaListagemSolicitacaoVO.resUsuario = Mid(Replace(User.Identity.Name.ToString.Trim, "SESC-GO.COM.BR\", ""), 1, 60)
            objReservaListagemSolicitacaoVO.resUsuarioData = Format(CDate(Now.ToString), "dd/MM/yyyy")
            If ckbOrganizadoSESC.Checked Then
                objReservaListagemSolicitacaoVO.resPasseioPromovidoCEREC = "S"
            Else
                objReservaListagemSolicitacaoVO.resPasseioPromovidoCEREC = "N"
            End If
            objReservaListagemSolicitacaoVO.resCidadeDes = Mid(cmbDestinoCidade.SelectedValue.Trim, 1, 40).Replace("'", "")
            objReservaListagemSolicitacaoVO.resLocalSaida = Mid(txtResLocalSaida.Text.Trim, 1, 200).Replace("'", "")
            If cmbReservaHoraSaida.Visible Then
                objReservaListagemSolicitacaoVO.resHoraSaida = cmbReservaHoraSaida.Text.Trim
            Else
                objReservaListagemSolicitacaoVO.resHoraSaida = cmbResHoraSaida.Text.Trim
            End If
            objReservaListagemSolicitacaoVO.resFormaPagamento = "ER"
            objReservaListagemSolicitacaoVO.resColoniaFeriasDes = cmbDestino.SelectedValue

            'Recreando escolar é feito passeio feito pelo setor social
            If chkRecreandoEscolar.Checked = True Then
                objReservaListagemSolicitacaoVO.resRecreandoEscolar = "S"
            Else
                objReservaListagemSolicitacaoVO.resRecreandoEscolar = "N"
            End If

            objReservaListagemSolicitacaoVO.resCapitalGoiasDes = "N"
            objReservaListagemSolicitacaoVO.resStatus = hddResStatus.Value
            objReservaListagemSolicitacaoVO.refPratoCod = cmbPratoRapido0.Text

            'Com pagamento no boleto e vencimento no dia da reserva
            Dim DataInicialCa, DataFinalCa, DataLimiteFinalCa As DateTime
            DataInicialCa = CDate(txtDataInicialSolicitacao.Text)
            DataFinalCa = CDate(txtDataFinalSolicitacao.Text)
            DataLimiteFinalCa = Func_Ultimo_Dia_Mes(DataInicialCa)

            'Limita a data da saída ser apenas um dia a mais caso o final da reserva caia no sábado
            If (DataFinalCa > DataLimiteFinalCa) Then 'se a data final for maior que o último dia do mês ira testar se a saida é diferente de sabado
                If DataFinalCa.DayOfWeek <> DayOfWeek.Saturday Then
                    DataFinalCa = DateAdd(DateInterval.Day, -1, DataFinalCa)
                End If
            End If

            ObjConsultasHospedeJaVO = New ConsultasHospedeJaVO
            ObjConsultasHospedeJaDAO = New ConsultasHospedeJaDAO(imgBtnNovaReserva.Attributes.Item("Conexao"))
            ObjConsultasHospedeJaVO = ObjConsultasHospedeJaDAO.ConsultaReservaPorResId(hddResId.Value)

            'Se não trouxer resultado irá setar os valores com os valores da tela
            If ObjConsultasHospedeJaVO Is Nothing Then
                ObjConsultasHospedeJaVO = New ConsultasHospedeJaVO
                ObjConsultasHospedeJaVO.resDataIni = DataInicialCa
                ObjConsultasHospedeJaVO.resDataFim = DataFinalCa
                ObjConsultasHospedeJaVO.resDtInsercao = Date.Now
                ObjConsultasHospedeJaVO.ResDtLimiteRetorno = Format(CDate(txtResDtLimiteRetorno.Text.Trim), "dd/MM/yyyy") & " " & cmbResHrLimiteRetorno.Text & ":00"
            End If

            Dim TestaSeReservaFoiFeitoInternet As Long
            '1-Feito pela internet/0-Não foi feito pela internt
            TestaSeReservaFoiFeitoInternet = ObjConsultasHospedeJaDAO.TestaSeReservaFoiPelaInternet(hddResId.Value)

            'Verificando se existe algum apto do tipo RT para essa reserva, caso afirmativo ela será do tipo Balcão.
            If hddResCaracteristica.Value = "I" Then
                For Each linha As GridViewRow In gdvReserva8.Rows
                    If gdvReserva8.DataKeys(linha.RowIndex).Item("acmId").ToString = "9" Then
                        btnReservaGravar.Attributes.Add("RT", "S")
                    End If
                Next
            End If

            'Restipo = H: Hospede Já; B: Balção, F: Federação; I:Individual; P:Passeio; E: Excursão;Null (internet); 
            If cmbOrgId.SelectedValue = "37" Then 'Presidencia = Federação
                objReservaListagemSolicitacaoVO.resTipo = "F"
            ElseIf (hddResCaracteristica.Value = "I" And TestaSeReservaFoiFeitoInternet = 1) Then
                objReservaListagemSolicitacaoVO.resTipo = "N" 'Internet
            ElseIf (hddResCaracteristica.Value = "I" And TestaSeReservaFoiFeitoInternet = 0) Then
                objReservaListagemSolicitacaoVO.resTipo = "B" 'Se não atender nenhuma das alternativas acima, a reserva será considerada como barcão.
            End If

            'Irá exigir a inserção do Contato 
            If (sender Is btnReservaGravar) Or (sender Is btnReservaReativar) Or (sender Is btnInformarRestituicao) Or (sender Is btnResEmailGrupo) Then
                Dim objReservaListagemSolicitacaoVOValidacao = New ReservaListagemSolicitacaoVO
                objReservaListagemSolicitacaoVOValidacao = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)

                'Irá verificar se é passeio primeiro, se não for irá verificar a reserva individual, ela sempre tera a data de inserção o passeio não.
                If txtResContato.Text.Trim.Length = 0 And (hddResCaracteristica.Value = "P" Or hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T") Then
                    Mensagem("Preencha o campo Contato de emergência para prosseguir.")
                    txtResContato.Focus()
                    Exit Try
                ElseIf txtResContato.Text.Trim.Length = 0 Then
                    If (Format(CDate(objReservaListagemSolicitacaoVOValidacao.resDtInsercao), "yyyy-MM-dd") = Format(Date.Today, "yyyy-MM-dd")) Then
                        Mensagem("Preencha o campo Contato de emergência para prosseguir.")
                        txtResContato.Focus()
                        Exit Try
                    End If
                End If
                If (txtResFoneComercial.Text.Trim.Length < 8 And
                    txtResCelular.Text.Trim.Length < 8 And
                    txtResFax.Text.Trim.Length < 8 And
                    txtResFoneResidencial.Text.Trim.Length < 8) Then
                    Mensagem("Informe um número de telefone antes de salvar.")
                    txtResFoneComercial.Focus()
                    Exit Try
                End If
            End If

            '...Salva/Cancela a reserva caso não tenha nenhum impedimento acima
            If hddResCaracteristica.Value = "P" Or hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then
                hddResId.Value = objReservaListagemSolicitacaoDAO.AcaoEmissivo(objReservaListagemSolicitacaoVO)
                objReservaListagemSolicitacaoVO.resId = hddResId.Value
                objReservaListagemSolicitacaoDAO.UpdateReservaGrupoCerec(objReservaListagemSolicitacaoVO)
            Else
                objReservaListagemSolicitacaoDAO.Acao(objReservaListagemSolicitacaoVO)
            End If

            If (sender Is btnReservaGravar) Or (sender Is btnReservaReativar) Or (sender Is btnInformarRestituicao) Or (sender Is btnResEmailGrupo) Then
                objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                'Para evitar cancelamento indevido o bloquei nunca poderá ser menor que a data de inserção
                If CDate(Format(CDate(objReservaListagemSolicitacaoVO.resDtInsercao), "yyyy/MM/dd")) > CDate(Format(CDate(objReservaListagemSolicitacaoVO.resDtLimiteRetorno), "yyyy/MM/dd")) Then
                    Mensagem("A data de Bloqueio esta incorreta, ela não poderá ser menor que a data de inserção da reserva que é dia: " & Format(CDate(objReservaListagemSolicitacaoVO.resDtInsercao), "dd/MM/yyyy"))
                    Exit Try
                End If

                CarregaDadosReserva()
                Habilitar(pnlResponsavelAcao)
                If sender Is btnReservaGravar And ckbResponsavel.Checked Then
                    objReservaListagemIntegranteVO = New Turismo.ReservaListagemIntegranteVO
                    objReservaListagemIntegranteVO.intId = 0
                    objReservaListagemIntegranteVO.resId = hddResId.Value
                    objReservaListagemIntegranteVO.intMatricula = txtResMatricula.Text.Replace(" ", "")
                    If txtHosDataIniSol.Text > "" Then
                        objReservaListagemIntegranteVO.intDiaIni = Format(CDate(txtHosDataIniSol.Text), "dd/MM/yyyy")
                    Else
                        objReservaListagemIntegranteVO.intDiaIni = Format(CDate(hddResDataIni.Value), "dd/MM/yyyy")
                    End If
                    If txtHosHoraIniSol.Text.Trim = "" Then
                        objReservaListagemIntegranteVO.intHoraIni = "12"
                    Else
                        objReservaListagemIntegranteVO.intHoraIni = txtHosHoraIniSol.Text
                    End If
                    objReservaListagemIntegranteVO.intDiaFim = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")

                    objReservaListagemIntegranteVO.intHoraFim = "12"

                    objReservaListagemIntegranteVO.catId = cmbResCatId.Text
                    objReservaListagemIntegranteVO.IntNome = Mid(txtResNome.Text.Trim.Replace("'", ""), 1, 80).Replace("'", "")
                    'objReservaListagemIntegranteVO.intRG = cmbIntRG.Text + " - " + txtIntRG.Text
                    objReservaListagemIntegranteVO.intRG = Mid(cmbResDocIdentificacao.SelectedValue & " - " & txtResDocIdentificacao.Text.Trim.Replace("'", ""), 1, 25)
                    objReservaListagemIntegranteVO.intCPF = txtResCPF.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Replace("/", "")
                    objReservaListagemIntegranteVO.intDtNascimento = Format(CDate(txtResDtNascimento.Text), "dd/MM/yyyy")
                    objReservaListagemIntegranteVO.intSexo = cmbResSexo.Text
                    Try
                        objReservaListagemIntegranteVO.solId = hddSolId.Value
                    Catch ex As Exception
                        objReservaListagemIntegranteVO.solId = Nothing
                    End Try
                    objReservaListagemIntegranteVO.intFormaPagamento = "ER"
                    objReservaListagemIntegranteVO.intCatCobranca = cmbResCatCobranca.Text
                    objReservaListagemIntegranteVO.intMemorando = Mid(txtResMemorando.Text.Trim, 1, 100).Replace("'", "")
                    '@Acao char(1), -- funções (I - incluir integrante) (D - excluir integrante) (A - alterar integrante) (H - incluir hospedagem) (T - excluir hospedagem) (M - modifica hospedagem)
                    objReservaListagemIntegranteVO.intEmissor = cmbResEmissor.Text
                    If cmbResCatId.Text = "2" Then
                        objReservaListagemIntegranteVO.acmIdCobranca = "1"
                    Else
                        objReservaListagemIntegranteVO.acmIdCobranca = cmbResCatId.Text
                    End If
                    objReservaListagemIntegranteVO.estId = cmbEstId.Text
                    If txtResCidade.Text.ToUpper Like "GOIÂNIA" Then
                        objReservaListagemIntegranteVO.intCapitalGoias = "S"
                    Else
                        objReservaListagemIntegranteVO.intCapitalGoias = "N"
                    End If
                    objReservaListagemIntegranteVO.intCidade = Mid(txtResCidade.Text.Trim, 1, 40).Replace("'", "")
                    '@Transferencia char(1), -- (S)im ou (N)ão
                    If ckbRefeicao.Items.Item(0).Selected = True And
                        ckbRefeicao.Items.Item(2).Selected = False Then
                        objReservaListagemIntegranteVO.intAlmoco = "I0"
                    ElseIf ckbRefeicao.Items.Item(0).Selected = False And
                        ckbRefeicao.Items.Item(2).Selected = True Then
                        objReservaListagemIntegranteVO.intAlmoco = "O0"
                    ElseIf ckbRefeicao.Items.Item(0).Selected = True And
                        ckbRefeicao.Items.Item(2).Selected = True Then
                        objReservaListagemIntegranteVO.intAlmoco = "I1"
                    Else
                        objReservaListagemIntegranteVO.intAlmoco = "I0"
                    End If
                    If ckbRefeicao.Items.Item(1).Selected = True And
                        ckbRefeicao.Items.Item(3).Selected = False Then
                        objReservaListagemIntegranteVO.intJantar = "I0"
                    ElseIf ckbRefeicao.Items.Item(1).Selected = False And
                        ckbRefeicao.Items.Item(3).Selected = True Then
                        objReservaListagemIntegranteVO.intJantar = "O0"
                    ElseIf ckbRefeicao.Items.Item(1).Selected = True And
                        ckbRefeicao.Items.Item(3).Selected = True Then
                        objReservaListagemIntegranteVO.intJantar = "I1"
                    Else
                        objReservaListagemIntegranteVO.intJantar = "I0"
                    End If
                    objReservaListagemIntegranteVO.intUsuario = Mid(Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""), 1, 60)
                    objReservaListagemIntegranteVO.intSalario = cmbResSalario.Text
                    objReservaListagemIntegranteVO.intEscolaridade = cmbResEscolaridade.Text
                    objReservaListagemIntegranteVO.intEstadoCivil = cmbResEstadoCivil.Text
                    Try
                        objReservaListagemIntegranteVO.intVinculoId = cmbIntVinculoId.Text
                    Catch ex As Exception
                        objReservaListagemIntegranteVO.intVinculoId = Nothing
                    End Try
                    objReservaListagemIntegranteVO.intFoneResponsavelExc = txtResFoneComercial.Text.Replace("(", "").Replace(") ", "").Replace(" ", "").Trim
                    objReservaListagemIntegranteVO.intValorUnitarioExc = 0

                    Dim strResultado As Turismo.ReservaListagemIntegranteDAO.Resultado

                    If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                        If sender Is btnIntegranteGravar Then
                            strResultado = objReservaListagemIntegranteDAO.AcaoEmissivo(objReservaListagemIntegranteVO, "N")
                        Else ' btnIntegranteExcluir
                            strResultado = objReservaListagemIntegranteDAO.AcaoEmissivo(objReservaListagemIntegranteVO, "S")
                        End If
                        If strResultado.mensagem.Trim = "OK" Then
                            hddIntId.Value = strResultado.intId.ToString
                            'If sender Is btnIntegranteExcluir Then
                            imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registrado com sucesso.');", True)
                            ckbResponsavel.Checked = False
                            btnReservaCalculo.Visible = True
                            'End If
                        Else
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                        End If

                    Else 'Hospedagem
                        If objReservaListagemIntegranteVO.intId = 0 Then
                            'Se estiver marcado para importar para integrante ira botar categoria de cobrança igual ao CatId
                            If ckbResponsavel.Checked Then
                                objReservaListagemIntegranteVO.intCatCobranca = cmbResCatId.SelectedValue
                            End If

                            strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "I")
                            If strResultado.mensagem.Trim = "" Then
                                imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registrado com sucesso.');", True)
                                ckbResponsavel.Checked = False
                            Else
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                            End If
                        Else
                            If sender Is btnIntegranteGravar Then
                                strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "A")
                                If strResultado.mensagem.Trim = "" Then
                                    If hddSolId.Value = hddSolIdNovo.Value Then
                                        If hddHosDataIniSol.Value <> txtHosDataIniSol.Text Or
                                           hddHosDataFimSol.Value <> txtHosDataFimSol.Text Or
                                           hddHosHoraIniSol.Value <> txtHosHoraIniSol.Text Or
                                           hddHosHoraFimSol.Value <> txtHosHoraFimSol.Text Or
                                           hddIntCPFAntes.Value <> txtIntCPF.Text.Trim.Replace(" ", "") Or
                                           btnIntegranteGravar.Attributes.Item("NomeAntes").ToString <> txtIntNome.Text.Trim Or
                                           btnIntegranteGravar.Attributes.Item("NascimentoAntes").ToString <> txtIntNascimento.Text Or
                                           btnIntegranteGravar.Attributes.Item("SexoAntes").ToString <> cmbIntSexo.SelectedValue Or
                                           hddAcomodacaoCobranca.Value <> cmbAcomodacaoCobranca.Text Then
                                            strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "M")
                                        End If
                                    Else
                                        strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "H")
                                    End If
                                    If strResultado.mensagem.Trim = "" Then
                                        imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                                    Else
                                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                                    End If
                                Else
                                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                                End If
                            End If
                        End If
                    End If
                    ListaIntegranteViaResId()
                    Me.pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "False"
                End If

                'Gravando o Período da reserva para uso futuro.
                imgBtnDtCheck_InMais.Attributes.Add("DataIniSolOld", Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy").Substring(0, 10))
                imgBtnDtCheck_InMais.Attributes.Add("DataFimSolOld", Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy").Substring(0, 10))

                pnlIntegranteTitulo.Visible = True
                pnlIntegranteAcao.Visible = True
                pnlFinanceiroTitulo.Visible = True
                pnlFinanceiroAcao.Visible = True

                pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "False"
                pnlFinanceiroTitulo_CollapsiblePanelExtender.ClientState = "True"
                pnlPlanilha.Visible = False
                pnlIntegrante.Visible = False
                pnlFinanceiro.Visible = False
                If sender Is btnReservaReativar Then
                    lnkResponsavel_Click(sender:=Nothing, e:=Nothing)
                End If
            Else
                imgBtnReservaAcaoVoltar_Click(sender, e)
            End If

            'Grupo de passeio só terá permissão para alterar o nome ou cancelar quem pertencer ao grupo "Turismo Social Acao Passeio"
            If (hddResCaracteristica.Value = "P" And
             Not Grupos.Contains("Turismo Social Acao Passeio")) Then
                btnReservaCancelar.Visible = False
                txtResNome.Enabled = False
            Else
                btnReservaCancelar.Visible = True
                txtResNome.Enabled = True
            End If
            'Carregando a nova data do limite retorno.
            hddResDtLimiteRetorno.Value = Format(CDate(txtResDtLimiteRetorno.Text), "dd/MM/yyyy")
            ListaIntegranteViaResId()
            'txtResNome.Focus()

            txtResMatricula.Focus()
        Catch ex As Exception
            enviarEmailGenerico("Campos da tela em dados do responsável com erro na reserva para análise " &
                "Matricula: " & objReservaListagemSolicitacaoVO.resMatricula &
                "resCPF_CGC: " & objReservaListagemSolicitacaoVO.resCPF_CGC &
                "resDtLimiteRetorno: " & objReservaListagemSolicitacaoVO.resDtLimiteRetorno &
                "resNome: " & objReservaListagemSolicitacaoVO.resNome &
                "resContato: " & objReservaListagemSolicitacaoVO.resContato &
                "resEmail: " & objReservaListagemSolicitacaoVO.resEmail &
                "resDtNascimento: " & objReservaListagemSolicitacaoVO.resDtNascimento &
                "resFoneComercial: " & objReservaListagemSolicitacaoVO.resFoneComercial &
                "resFoneResidencial: " & objReservaListagemSolicitacaoVO.resFoneResidencial &
                "resCelular: " & objReservaListagemSolicitacaoVO.resCelular &
                "resLogradouro: " & objReservaListagemSolicitacaoVO.resLogradouro &
                "resNumero: " & objReservaListagemSolicitacaoVO.resNumero &
                "resQuadra: " & objReservaListagemSolicitacaoVO.resQuadra &
                "resLote: " & objReservaListagemSolicitacaoVO.resLote &
                "resComplemento: " & objReservaListagemSolicitacaoVO.resComplemento &
                "resBairro: " & objReservaListagemSolicitacaoVO.resBairro &
                "resCidade: " & objReservaListagemSolicitacaoVO.resCidade &
                "resRG: " & objReservaListagemSolicitacaoVO.resRG &
                "resMemorando: " & objReservaListagemSolicitacaoVO.resMemorando &
                "resObs: " & objReservaListagemSolicitacaoVO.resObs &
                "Reserva: " & objReservaListagemSolicitacaoVO.resId &
                "Erro Encontrado: " & ex.StackTrace.ToString)
            GravaLog("Reserva: " & objReservaListagemSolicitacaoVO.resId & "Erro Encontrado: " & ex.StackTrace.ToString)
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao cancelar a reserva. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnReservaCancelar Resid: " & hddResId.Value)
        End Try
    End Sub

    Protected Sub lblReservaAcao_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblReservaAcao.PreRender
        If hddResId.Value = "0" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação nova"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação nova"
        ElseIf hddResStatus.Value = "C" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação cancelada"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação cancelada"
        ElseIf hddResStatus.Value = "F" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação finalizada"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação finalizada"
        ElseIf hddResStatus.Value = "I" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação pendente de integrante"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação pendente de integrante"
        ElseIf hddResStatus.Value = "P" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação pendente de pagamento"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação pendente de pagamento"
        ElseIf hddResStatus.Value = "R" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação confirmada"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação confirmada"
        ElseIf hddResStatus.Value = "E" Then
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação em estada"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação em estada"
        Else
            pnlSolicitacaoAcao_CollapsiblePanelExtender.CollapsedText = "Solicitação"
            pnlSolicitacaoAcao_CollapsiblePanelExtender.ExpandedText = "Solicitação"
        End If
        If ckbOrganizadoSESC.Checked Then
            imgBtnIntegranteNovoAcao.Visible = (InStr("CF", hddResStatus.Value) = 0) And ((gdvReserva9.Rows.Count - CInt(hddColo.Value)) < hddCapacidade.Value) And (InStr("IET", hddResCaracteristica.Value) = 0) Or (InStr("IET", hddResCaracteristica.Value) <> 0)
            If imgBtnIntegranteNovoAcao.Visible Then
                lblIntegranteOver.Text = CInt(hddCapacidade.Value) - (gdvReserva9.Rows.Count - CInt(hddColo.Value)) & " vaga(s)."
            Else
                lblIntegranteOver.Text = "ATENÇÃO! Limite de integrantes foi atingido."
            End If
            lblIntegranteOver.Visible = (InStr("IET", hddResCaracteristica.Value) = 0)
        Else
            imgBtnIntegranteNovoAcao.Visible = True And (InStr("CF", hddResStatus.Value) = 0)
            lblIntegranteOver.Visible = False
        End If
        'btnCaixa.Visible = InStr("IEP", hddResStatus.Value) > 0
        'Sera exibido para grupos de passeio quando for de Caldas ou Pirenopolis
        btnCaixa.Visible = (hddResCaracteristica.Value = "I" Or (hddResCaracteristica.Value = "E") Or (hddResCaracteristica.Value = "T") Or (hddResCaracteristica.Value = "P" And (cmbDestino.SelectedIndex > 0 Or cmbDestinoCidade.SelectedValue.Trim = "PIRENOPOLIS")) And hddResStatus.Value = "P")
        'btnCaixa.Visible = (hddResCaracteristica.Value = "I" Or (hddResCaracteristica.Value = "E") Or (hddResCaracteristica.Value = "P" And (cmbDestinoCidade.SelectedValue.Trim = "CALDAS NOVAS" Or cmbDestinoCidade.SelectedValue.Trim = "PIRENOPOLIS")) And hddResStatus.Value = "P")
        'btnCaixa.Visible = (hddResCaracteristica.Value = "I" Or ((hddResCaracteristica.Value = "P" Or hddResCaracteristica.Value = "E") And (cmbDestinoCidade.SelectedValue.Trim = "CALDAS NOVAS" Or cmbDestinoCidade.SelectedValue.Trim = "PIRENOPOLIS")) And hddResStatus.Value = "P")
        ckbResponsavel.Visible = ((gdvReserva9.Rows.Count = 0) And btnReservaGravar.Visible And hddResCaracteristica.Value = "I")
    End Sub

    Protected Sub gdvReserva10_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva10.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If InStr("ABCET", gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus")) = 0 And
              ((Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR")) Or
              (Session("GrupoEmissivo") And (gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuario") = Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")))) Then
                CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).Visible = True
                CType(e.Row.FindControl("lblNossoNumero"), Label).Visible = False
                CType(e.Row.FindControl("lblDtVencimento"), Label).Text = Format(CDate(gdvReserva10.DataKeys(e.Row.RowIndex).Item("bolImpDtVencimento").ToString), "dd/MM/yyyy")
                CType(e.Row.FindControl("lblDtVencimento"), Label).Visible = True
                Try
                    CType(e.Row.FindControl("lblDtPagamento"), Label).Text = Format(CDate(gdvReserva10.DataKeys(e.Row.RowIndex).Item("venData").ToString), "dd/MM/yyyy")
                Catch ex As Exception
                    CType(e.Row.FindControl("lblDtPagamento"), Label).Text = ""
                End Try
                CType(e.Row.FindControl("lblDtPagamento"), Label).Visible = True
                CType(e.Row.FindControl("lblVlrPago"), Label).Visible = True
                CType(e.Row.FindControl("lblFormaPgto"), Label).Visible = True
                If gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "M" Then
                    CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).ToolTip = "Clique para estornar pagamento."
                Else
                    CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkNossoNumeroInclusao"), LinkButton).Visible = True
                End If
                'Esconderá os boletos com os pagamentos ainda não confirmados com menos de 21 minutos de criação, e a reserva que ainda não estiver confirmada
                Try
                    If DateDiff(DateInterval.Minute, CDate(gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolImpDtDocumento").ToString), Date.Now) < 21 And
                        gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo").ToString = "T" And
                        CType(e.Row.FindControl("lblNossoNumero"), Label).Visible = False Then
                        e.Row.Visible = False
                    End If
                Catch ex As Exception
                End Try

                'Se a Reserva já estiver confirmada, irá esconder todos os boletos gerados e não pago
                If (hddResStatus.Value = "R" Or hddResStatus.Value = "E") And
                    CType(e.Row.FindControl("lnkNossoNumeroInclusao"), LinkButton).Visible = True Then
                    e.Row.Visible = False
                End If
            Else
                CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).Visible = False
                CType(e.Row.FindControl("lblNossoNumero"), Label).Text = gdvReserva10.DataKeys(e.Row.RowIndex).Item("bolImpNossoNumero").ToString
                CType(e.Row.FindControl("lblNossoNumero"), Label).Visible = True
                CType(e.Row.FindControl("lblDtVencimento"), Label).Text = Format(CDate(gdvReserva10.DataKeys(e.Row.RowIndex).Item("bolImpDtVencimento").ToString), "dd/MM/yyyy")
                CType(e.Row.FindControl("lblDtVencimento"), Label).Visible = True
                If gdvReserva10.DataKeys(e.Row.RowIndex).Item("venData").ToString > "" Then
                    CType(e.Row.FindControl("lblDtPagamento"), Label).Text = Format(CDate(gdvReserva10.DataKeys(e.Row.RowIndex).Item("venData").ToString), "dd/MM/yyyy")
                Else
                    CType(e.Row.FindControl("lblDtPagamento"), Label).Text = gdvReserva10.DataKeys(e.Row.RowIndex).Item("venData").ToString
                End If
                CType(e.Row.FindControl("lblDtPagamento"), Label).Visible = True
                CType(e.Row.FindControl("lblVlrPago"), Label).Visible = True
                CType(e.Row.FindControl("lblFormaPgto"), Label).Visible = True
            End If
            If gdvReserva10.DataKeys(e.Row.RowIndex).Item("venValor") > 0 Then
                CType(e.Row.FindControl("lblVlrPago"), Label).Text = FormatNumber(gdvReserva10.DataKeys(e.Row.RowIndex).Item("venValor").ToString)
            End If

            'Exibir se o pagamento foi feito no cartão de crédito ou no boleto
            If gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo") = "T" Then
                CType(e.Row.FindControl("imgBoletoCartao"), Image).ImageUrl = "~/images/CartaoCreditoPequeno.png"
                'ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo") = "B" Then
                '    CType(e.Row.FindControl("imgBoletoCartao"), Image).ImageUrl = "~/images/BarCodPequeno.png"
            Else
                CType(e.Row.FindControl("imgBoletoCartao"), Image).Visible = False
            End If


            If gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "B" Then
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Boleto"
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "A" Then
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Central Atendimento"
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "T" Then
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Cartão de Crédito"
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "M" Then
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Manual - " &
                gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuario")
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Manual - " &
                gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuario")
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "C" Then
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Caixa - " &
                gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuario")
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("venStatus") = "E" Then
                CType(e.Row.FindControl("lblNossoNumero"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblNossoNumero"), Label).Font.Strikeout = True
                CType(e.Row.FindControl("lblDtVencimento"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblDtVencimento"), Label).Font.Strikeout = True
                CType(e.Row.FindControl("lblDtPagamento"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblDtPagamento"), Label).Font.Strikeout = True
                CType(e.Row.FindControl("lblVlrPago"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblVlrPago"), Label).Font.Strikeout = True
                CType(e.Row.FindControl("lblFormaPgto"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblFormaPgto"), Label).Font.Strikeout = True
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = "Estornado " &
                gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuario") & " " &
                gdvReserva10.DataKeys(e.Row.RowIndex).Item("venUsuarioData")
            Else
                CType(e.Row.FindControl("lblFormaPgto"), Label).Text = ""
                CType(e.Row.FindControl("lblNossoNumero"), Label).Visible = False
                'O Grupo de recepção de Caldas Novas poderá ver o vencimento de todos os boletos
                If Not Grupos.Contains("CNV_RECEPCAO") Then
                    CType(e.Row.FindControl("lblDtVencimento"), Label).Visible = False
                End If
                CType(e.Row.FindControl("lnkNossoNumero"), LinkButton).Visible = False
                CType(e.Row.FindControl("lnkNossoNumeroInclusao"), LinkButton).Visible = True
                CType(e.Row.FindControl("lnkNossoNumeroInclusao"), LinkButton).ToolTip = "Clique para realizar pagamento." & " Valor R$ " & FormatNumber(gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolImpValor").ToString)
                'Se a Reserva já estiver confirmada, irá esconder todos os boletos gerados e não pago
                If (hddResStatus.Value = "R" Or hddResStatus.Value = "E") And
                     CType(e.Row.FindControl("lnkNossoNumeroInclusao"), LinkButton).Visible = False Then
                    e.Row.Visible = False
                End If
            End If

            'Controle de exibição do status do registro de boleto junto a caixa econômica federal - 08/05/2019 Washington
            If gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo") = "B" Then
                If gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolStatusRemessaCaixa") = "00" Or
                    gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolStatusRemessaCaixa") = "90" Then
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = True
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ToolTip = "Enviado para registro"
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ImageUrl = "images\BoletoAmarelo.png"
                ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolStatusRemessaCaixa") = "02" Then
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = True
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ToolTip = "Boleto registrado." & vbNewLine & "Clique para imprimir."
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ImageUrl = "images\BoletoVerde.png"
                ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolStatusRemessaCaixa") = "06" Then
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = True
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ToolTip = "Pagamento Efetuado"
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).ImageUrl = "images\BoletoAzul.png"
                Else
                    CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = False
                End If
            ElseIf gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo") = "C" Then
                CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = True
                CType(e.Row.FindControl("imgRegistro"), ImageButton).ToolTip = "Tipo: Cupom"
                CType(e.Row.FindControl("imgRegistro"), ImageButton).ImageUrl = "images\CupomVermelho.png"
            Else
                CType(e.Row.FindControl("imgRegistro"), ImageButton).Visible = False
            End If

            'Controlando quem poderá apagar boletos gerados - 08/05/2019 Washington
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                'Mostrá somente boletos não pago
                If (gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolStatusRemessaCaixa") = "06" Or
                       gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolImpTipoParcela") = "W" Or
                       gdvReserva10.DataKeys(e.Row.RowIndex).Item("BolTipo") <> "B") Then
                    CType(e.Row.FindControl("imgApagaBoleto"), ImageButton).Visible = False
                Else
                    CType(e.Row.FindControl("imgApagaBoleto"), ImageButton).Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub lnkIntNome_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialPiri")
        End If
        'Try
        '    hddResId.Value = sender.CommandArgument.ToString
        'Catch ex As Exception

        'End Try
        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)

        'Atualizando os valores referente a faixa etária dos integrantes.
        objDefaultVO = objDefaultDAO.consultar()
        If Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "yyyy-MM-dd") <= Format(CDate(hddDataFaixaEtaria.Value), "yyyy-MM-dd") Then
            hddIdadeIsento.Value = objDefaultVO.faixaEtariaIsento
            hddIdadeCrianca.Value = objDefaultVO.faixaEtariaCrianca
            hddIdadeAdulto.Value = objDefaultVO.faixaEtariaAdulto
        ElseIf Format(CDate(objReservaListagemSolicitacaoVO.resDataIni), "yyyy-MM-dd") > Format(CDate(hddDataFaixaEtaria.Value), "yyyy-MM-dd") Then
            hddIdadeIsento.Value = objDefaultVO.FaixaEtariaIsento1
            hddIdadeCrianca.Value = objDefaultVO.FaixaEtariaCrianca1
            hddIdadeAdulto.Value = objDefaultVO.FaixaEtariaAdulto1
        End If

        CarregaCmbResIdWeb()
        CarregaEstado()
        CarregaFormaPagto()
        CarregaCmbDestino()
        CarregaCmbRefeicaoPrato()
        CarregaDadosReserva()
        ListaFinanceiroViaResId()
        ListaIntegranteViaResId()
        pnlResponsavelTitulo.Visible = True
        pnlResponsavelAcao.Visible = True

        If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("CF", hddResStatus.Value) = 0) Then 'Hospedagem
            ProcuraDisponibilidade()
            'pnlSolicitacaoSelecionada.Visible = True
            btnReservaGravar.Enabled = True
            btnReservaGravar.Visible = True
            btnReservaCancelar.Enabled = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaCancelar.Visible = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaReativar.Enabled = False
            btnReservaReativar.Visible = False
            btnInformarRestituicao.Enabled = False
            btnInformarRestituicao.Visible = False
            Habilitar(pnlResponsavelAcao)
        ElseIf (InStr("IET", hddResCaracteristica.Value) = 0) And (InStr("CFE", hddResStatus.Value) = 0) Then 'Emissivo
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
                btnReservaGravar.Enabled = True
                btnReservaGravar.Visible = True
                btnReservaCalculo.Visible = True
                btnReservaCancelar.Enabled = (hddResId.Value <> "0")
                btnReservaCancelar.Visible = (hddResId.Value <> "0")
                btnReservaReativar.Enabled = False
                btnReservaReativar.Visible = False
                btnInformarRestituicao.Enabled = False
                btnInformarRestituicao.Visible = False
                btnInformarRestituicao.Enabled = False
                btnInformarRestituicao.Visible = False
                Habilitar(pnlResponsavelAcao)
            End If
        Else
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            btnReservaGravar.Enabled = False
            btnReservaGravar.Visible = False
            btnReservaCalculo.Visible = False
            btnReservaCancelar.Enabled = False
            btnReservaCancelar.Visible = False
            If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("C", hddResStatus.Value) = 1) Then 'Hospedagem Cancelada
                btnReservaReativar.Enabled = True
                btnReservaReativar.Visible = True
                btnInformarRestituicao.Enabled = True
                btnInformarRestituicao.Visible = True
            End If
            Desabilitar(pnlResponsavelAcao)
        End If

        pnlReservaAcao.Visible = True
        pnlIntegranteTitulo.Visible = True
        pnlIntegranteAcao.Visible = True
        pnlFinanceiroTitulo.Visible = True
        pnlFinanceiroAcao.Visible = True
        If sender Is Nothing Then
        Else
            pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "False"
            pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
            pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "True"
            pnlFinanceiroTitulo_CollapsiblePanelExtender.ClientState = "True"
        End If
        pnlCabecalho.Visible = False
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False

        pnlFinanceiro.Visible = False
    End Sub

    Protected Sub gdvReserva3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva3.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            cont = 0
            totalValor = 0
            totalPago = 0
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkIntNome"), LinkButton).CommandArgument =
               gdvReserva3.DataKeys(e.Row.RowIndex).Item(1).ToString()
            If gdvReserva3.DataKeys(e.Row.RowIndex).Item("intExcluido") = "S" Then
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).Font.Strikeout = True
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkServidor"), LinkButton).Font.Strikeout = True
            Else
                If gdvReserva3.DataKeys(e.Row.RowIndex).Item("hosValorDevido") > gdvReserva3.DataKeys(e.Row.RowIndex).Item("hosValorPago") Then
                    e.Row.BackColor = Drawing.Color.FromName("#FFEAEA")
                End If
                cont += 1
                totalValor += gdvReserva3.DataKeys(e.Row.RowIndex).Item("hosValorDevido")
                totalPago += gdvReserva3.DataKeys(e.Row.RowIndex).Item("hosValorPago")
            End If

            If sender.DataKeys(e.Row.RowIndex).Item("catLink") = "1" Then
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                'CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Green
            ElseIf sender.DataKeys(e.Row.RowIndex).Item("catLink") = "3" Then
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                'CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Chocolate
            Else
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                'CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Red
            End If
            CType(e.Row.FindControl("lnkServidor"), LinkButton).Text = gdvReserva3.DataKeys(e.Row.RowIndex).Item("intUsuario").ToString.Replace("SESC-GO.COM.BR\", "").Replace(".", " ")
            CType(e.Row.FindControl("lnkIntNome"), LinkButton).ToolTip = "Ir para integrante (" & gdvReserva3.DataKeys(e.Row.RowIndex).Item("intId").ToString & ")"
            CType(e.Row.FindControl("imgDependente"), Image).Visible = gdvReserva3.DataKeys(e.Row.RowIndex).Item("intVinculoId") <> "0" _
              And gdvReserva3.DataKeys(e.Row.RowIndex).Item("intVinculoId") <> gdvReserva3.DataKeys(e.Row.RowIndex).Item("intId")
            'Mostra no tooltip o nome do responsável vinculado ao integrante
            CType(e.Row.FindControl("imgDependente"), Image).ToolTip = "Responsável: " & gdvReserva3.DataKeys(e.Row.RowIndex).Item("NomeResponsavel").ToString.ToUpper

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(3).Text = cont
            e.Row.Cells(4).Text = Format(CDec(totalValor), "#,##0.00")
            e.Row.Cells(5).Text = Format(CDec(totalPago), "#,##0.00")
            lblIntegrante.Text = "Integrantes " & cont.ToString
        End If
    End Sub

    Protected Sub lnkNossoNumero_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If

        hddResId.Value = sender.CommandArgument.ToString
        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)

        CarregaCmbResIdWeb()
        CarregaEstado()
        CarregaFormaPagto()
        CarregaCmbDestino()
        CarregaCmbRefeicaoPrato()
        CarregaDadosReserva()
        ListaIntegranteViaResId()
        ListaFinanceiroViaResId()
        pnlResponsavelTitulo.Visible = True
        pnlResponsavelAcao.Visible = True

        If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("CF", hddResStatus.Value) = 0) Then 'Hospedagem
            ProcuraDisponibilidade()
            'pnlSolicitacaoSelecionada.Visible = True
            btnReservaGravar.Enabled = True
            btnReservaGravar.Visible = True
            btnReservaCancelar.Enabled = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaCancelar.Visible = (hddResId.Value <> "0") And (hddResStatus.Value <> "E")
            btnReservaReativar.Enabled = False
            btnReservaReativar.Visible = False
            btnInformarRestituicao.Enabled = False
            btnInformarRestituicao.Visible = False
            Habilitar(pnlResponsavelAcao)
        ElseIf (InStr("IET", hddResCaracteristica.Value) = 0) And (InStr("CFE", hddResStatus.Value) = 0) Then 'Emissivo
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            If Session("GrupoCEREC") Or Session("GrupoGP") Or Session("GrupoDR") Then
                btnReservaGravar.Enabled = True
                btnReservaGravar.Visible = True
                btnReservaCalculo.Visible = True
                btnReservaCancelar.Enabled = (hddResId.Value <> "0")
                btnReservaCancelar.Visible = (hddResId.Value <> "0")
                btnReservaReativar.Enabled = False
                btnReservaReativar.Visible = False
                btnInformarRestituicao.Enabled = False
                btnInformarRestituicao.Visible = False
                Habilitar(pnlResponsavelAcao)
            End If
        Else
            pnlSolicitacaoSelecionada.Visible = False
            pnlAcomodacaoTitulo.Visible = False
            btnReservaGravar.Enabled = False
            btnReservaGravar.Visible = False
            btnReservaCalculo.Visible = False
            btnReservaCancelar.Enabled = False
            btnReservaCancelar.Visible = False
            If (InStr("IET", hddResCaracteristica.Value) <> 0) And (InStr("C", hddResStatus.Value) = 1) Then 'Hospedagem Cancelada
                btnReservaReativar.Enabled = True
                btnReservaReativar.Visible = True
                btnInformarRestituicao.Enabled = True
                btnInformarRestituicao.Visible = True
            End If
            Desabilitar(pnlResponsavelAcao)
        End If

        pnlReservaAcao.Visible = True
        pnlIntegranteTitulo.Visible = True
        pnlIntegranteAcao.Visible = True
        pnlFinanceiroTitulo.Visible = True
        pnlFinanceiroAcao.Visible = True

        pnlFinanceiroTitulo_CollapsiblePanelExtender.ClientState = "False"
        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlCabecalho.Visible = False
        pnlReserva.Visible = False
        pnlAcomodacao.Visible = False
        pnlPlanilha.Visible = False
        pnlIntegrante.Visible = False
        pnlFinanceiro.Visible = False
    End Sub

    Protected Sub gdvReserva9_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva9.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvReserva9.DataKeys(e.Row.RowIndex).Item("hosStatus") <> "T" Then
                cont += 1
            End If
            totalValor += gdvReserva9.DataKeys(e.Row.RowIndex).Item("hosValorDevido")
            totalPago += gdvReserva9.DataKeys(e.Row.RowIndex).Item("hosValorPago")

            If totalValor > totalPago Then
                DeixaVerSelecaoTotalBoleto = True
                'Deixarei ver a imagem do boleto somente se o inicio da reserva tiver mais que (07) 10 dias com base no dia atual
                VerBoletoAVista = (Format(CDate(DateAdd(DateInterval.Day, 7, Date.Now)), "yyyy-MM-dd") < Format(CDate(gdvReserva9.DataKeys(e.Row.RowIndex).Item("intDiaIni").ToString), "yyyy-MM-dd"))

                'Se já tiver gerado algum boleto para a reserva, a opção de gerar boleto a vista não será mais exibida
                If (gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "00" Or
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "90" Or
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "02" Or
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "06") And ExisteBoletoGerado = 0 Then
                    ExisteBoletoGerado = 1
                End If

            End If

            If varBoleto50 = True And (gdvReserva9.DataKeys(e.Row.RowIndex).Item(12) > 0) Then
                varBoleto50 = False
            End If
            CType(e.Row.FindControl("lnkServidor"), LinkButton).Text =
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("intUsuario").Replace("SESC-GO.COM.BR\", "")
            If (InStr("IET", hddResCaracteristica.Value) = 0) Then 'Emissivo
                If gdvReserva9.DataKeys(e.Row.RowIndex).Item("intCriancaColoExc").ToString = "S" Then
                    hddColo.Value = CInt(hddColo.Value) + 1
                End If
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("lnkServidor"), LinkButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).CssClass =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgBtnDtCheckOutMais"), ImageButton).Visible = False
                CType(e.Row.FindControl("imgDependente"), Image).Visible = gdvReserva9.DataKeys(e.Row.RowIndex).Item("intVinculoId") <> "0" _
                  And gdvReserva9.DataKeys(e.Row.RowIndex).Item("intVinculoId") <> gdvReserva9.DataKeys(e.Row.RowIndex).Item("intId")
                CType(e.Row.FindControl("imgDependente"), Image).ToolTip = "Responsável: " & gdvReserva9.DataKeys(e.Row.RowIndex).Item("NomeResponsavel").ToString.ToUpper

            Else 'Hospedagem
                If gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString() = "0" Then
                    CType(e.Row.FindControl("lnkIntNome"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).CommandArgument =
                          -gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkRefeicao"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("lnkServidor"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).CssClass =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                Else
                    CType(e.Row.FindControl("lnkIntNome"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkRefeicao"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("lnkServidor"), LinkButton).CommandArgument =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(10).ToString()
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).CssClass =
                          gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() & "#" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString() & "$" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(4).ToString() & "æ" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(5).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() & "#" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString() & "$" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(4).ToString() & "æ" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(5).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() & "#" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString() & "$" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(4).ToString() & "æ" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(5).ToString()
                    CType(e.Row.FindControl("imgBtnDtCheckOutMais"), ImageButton).CommandArgument =
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() & "#" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(0).ToString() & "$" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(4).ToString() & "æ" &
                      gdvReserva9.DataKeys(e.Row.RowIndex).Item(5).ToString()

                    If hddHosId.Value = gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() Then
                        hddDataInDepois.Value = Format(CDate(gdvReserva9.DataKeys(e.Row.RowIndex).Item(4).ToString()), "dd/MM/yyyy")
                        hddDataOutDepois.Value = Format(CDate(gdvReserva9.DataKeys(e.Row.RowIndex).Item(5).ToString()), "dd/MM/yyyy")
                    End If
                End If



                'If hddResCaracteristica.Value = "E" And gdvReserva9.DataKeys(e.Row.RowIndex).Item(15) = "" Then
                'CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).Text += " (" & gdvReserva9.DataKeys(e.Row.RowIndex).Item(2).ToString() & ")"
                'End If

                If (CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaIni")) > Now.Date) Then 'And (CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaIni")) = CDate(hddResDataIni.Value)) Then
                    CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).Visible = True And (InStr("CFE", hddResStatus.Value) = 0)
                Else
                    CType(e.Row.FindControl("imgBtnDtCheckInMenos"), ImageButton).Visible = False
                End If
                If (DateDiff(DateInterval.Day, CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaIni")), CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaFim"))) > 1) Then
                    CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).Visible = True And (InStr("CFE", hddResStatus.Value) = 0) 'And (CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaIni")) = CDate(hddResDataIni.Value)) 
                    CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0) 'And (CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaFim")) = CDate(hddResDataFim.Value))
                Else
                    CType(e.Row.FindControl("imgBtnDtCheckInMais"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgBtnDtCheckOutMenos"), ImageButton).Visible = False
                End If
                CType(e.Row.FindControl("imgBtnDtCheckOutMais"), ImageButton).Visible = True And (InStr("CF", hddResStatus.Value) = 0) 'And (CDate(sender.DataKeys(e.Row.RowIndex).Item("intDiaFim")) = CDate(hddResDataFim.Value))
            End If

            'O voucher só será visível quando o integrante tiver pago seu valor devido
            If ((CDec(CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).Text) > CDec(CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).Text)) _
                    Or CDec(CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).Text) = 0) _
                    Or hddResCaracteristica.Value = "P" Then
                CType(e.Row.FindControl("imbVoucher"), ImageButton).Visible = False
            End If

            'Controlando a exibição de boletos - Se tiver um boleto gerado não irá permitir clicar no box
            If (gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraParcela").ToString.Trim.Length > 0 Or
               gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaParcela").ToString.Trim.Length > 0 Or
               gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolParcelaUnica").ToString.Trim.Length > 0) Then
                'CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = False
                'CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Existe boleto gerado para esse integrante"
                'CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Checked = False


                If gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraParcela").ToString = "00" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto gerado, mas ainda será enviado para registro! Aguarde..." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraParcela").ToString = "90" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto enviado para registro." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraParcela").ToString = "02" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto registrado!" & vbNewLine & "Clique para Imprimir."
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True
                End If

                If gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaParcela").ToString = "00" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto gerado, mas ainda será enviado para registro! Aguarde..." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaParcela").ToString = "90" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto enviado para registro." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaParcela").ToString = "02" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto registrado!" & vbNewLine & "Clique para Imprimir."
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True
                End If

                If gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "00" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto gerado, mas ainda será enviado para registro! Aguarde..." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "90" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto enviado para registro." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "02" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto registrado!" & vbNewLine & "Clique para Imprimir."
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "06" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto pago!"
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = False
                End If

                If (gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraParcela").ToString.Trim.Length > 0 And
                            hddResCaracteristica.Value = "I" And
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraStatusRegistro").ToString = "06") Then
                    'DeixaVerSelecaoTotalBoleto = True
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Primeira parcela gerada e paga, será permitido gerar a segunda parcela do boleto."
                End If

            End If

            'Se for da CENTRAL DE ATENDIMENTOS não irá permitir a emissão de cupons/boletos para Emissivo Organizado pelo Sesc
            If gdvReserva9.DataKeys(e.Row.RowIndex).Item(11) = gdvReserva9.DataKeys(e.Row.RowIndex).Item(12) Or
                (InStr("C", hddResStatus.Value) = 0) = False Or gdvReserva9.DataKeys(e.Row.RowIndex).Item(13) <> "ER" Or
                (Session("GrupoCentralAtendimento") And ((hddResTipo.Value = "P" Or hddResTipo.Value = "E" Or hddResTipo.Value = "T") And ckbOrganizadoSESC.Checked = True)) Then 'CFER
                CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Visible = False
                CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Checked = False
            Else
                '                CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Visible = True

                'Se tiver gerado boleto único, não deixarei exibir o check de seleção para geração do boleto
                CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Visible = ((gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = Nothing Or
                    gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "00" Or gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "02") Or
                (gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraStatusRegistro").ToString = Nothing Or
                    gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraStatusRegistro").ToString = "00" Or gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraStatusRegistro").ToString = "02") Or
                ((gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaStatusRegistro").ToString = Nothing Or
                    gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaStatusRegistro").ToString = "00" Or gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolSegundaStatusRegistro").ToString = "02") And
                gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolPrimeiraStatusRegistro").ToString <> Nothing)) And
                (gdvReserva9.DataKeys(e.Row.RowIndex).Item("hosValorDevido") > gdvReserva9.DataKeys(e.Row.RowIndex).Item("hosValorPago"))


                If gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "00" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto gerado, mas ainda será enviado para registro! Aguarde..." ' & vbNewLine & "Clique para reimprimir."

                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "90" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto enviado para registro." ' & vbNewLine & "Clique para reimprimir."
                ElseIf gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolUnicaStatusRegistro").ToString = "02" Then
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).ToolTip = "Boleto registrado!" & vbNewLine & "Clique para Imprimir."
                    CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True
                End If


                gdvReserva9.Columns(0).Visible = DeixaVerSelecaoTotalBoleto = True

                If (varPgtoBoleto = False) Then
                    varBoleto = True
                    varPgtoBoleto = True
                End If
            End If

            CType(e.Row.FindControl("lnkIntNome"), LinkButton).ToolTip = "Ir para integrante (" & gdvReserva9.DataKeys(e.Row.RowIndex).Item("intId").ToString & ")"
            If sender.DataKeys(e.Row.RowIndex).Item("catLink") = "1" Then
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).ForeColor = Drawing.Color.Green
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Green
            ElseIf sender.DataKeys(e.Row.RowIndex).Item("catLink") = "3" Then
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).ForeColor = Drawing.Color.Chocolate
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Chocolate
            Else
                CType(e.Row.FindControl("lnkIntNome"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCategoriaIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckInIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkCheckOutIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkAcomodacaoIntegrante"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorDevido"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkHosValorPago"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lnkServidor"), LinkButton).ForeColor = Drawing.Color.Red
            End If

        ElseIf e.Row.RowType = DataControlRowType.Header Then
            CType(e.Row.FindControl("txtDiasPrazo"), TextBox).Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            CType(e.Row.FindControl("txtDiasPrazo"), TextBox).Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")
            gdvReserva9.Columns(0).Visible = False
            varBoleto = False
            varBoleto50 = (InStr("CFER", hddResStatus.Value) = 0)
            varPgtoBoleto = False
            hddColo.Value = "0"

            'S - Sem Acomodação
            'F - finalizada
            'C - Cancelada
            'E - Estada
            'P - Rescaracteristica Passeio
            If hddResStatus.Value = "S" Or
            hddResStatus.Value = "F" Or
            hddResStatus.Value = "C" Or
            hddResCaracteristica.Value = "P" Or
            hddResStatus.Value = "E" Then
                gdvReserva9.Columns(4).Visible = False
            Else
                gdvReserva9.Columns(4).Visible = True
            End If

            cont = 0
            totalValor = 0
            totalPago = 0

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(5).Text = "Total"
            e.Row.Cells(6).Text = cont
            e.Row.Cells(7).Text = Format(CDec(totalValor), "#,##0.00")
            e.Row.Cells(8).Text = Format(CDec(totalPago), "#,##0.00")

            If hddResCaracteristica.Value = "P" Or totalPago = 0 Then
                gdvReserva9.Columns(11).Visible = False
            Else
                gdvReserva9.Columns(11).Visible = True
            End If

            If (varBoleto50 = True) And ((InStr("I", hddResCaracteristica.Value) = 0) Or (InStr("CFER", hddResStatus.Value) > 1) Or
               (DateDiff(DateInterval.Day, Now.Date, CDate(txtDataInicialSolicitacao.Text)) < 30)) Then
                varBoleto50 = False
            End If

            If varPgtoBoleto = False Or
                (DateDiff(DateInterval.Day, CDate(Format(Date.Now, "dd/MM/yyyy")),
                CDate(Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy"))) < 2) Then
                gdvReserva9.HeaderRow.FindControl("imgBoleto50").Visible = False
                gdvReserva9.HeaderRow.FindControl("imgCupomIndividual").Visible = False
            Else
                gdvReserva9.HeaderRow.FindControl("imgCupomIndividual").Visible = varBoleto And DeixaVerSelecaoTotalBoleto = True
                gdvReserva9.HeaderRow.FindControl("imgBoleto50").Visible = varBoleto50 And DeixaVerSelecaoTotalBoleto = True
            End If

            'Se for HospedeJá também não irei mostrar boleto para impressão
            gdvReserva9.HeaderRow.FindControl("imgBoleto").Visible = gdvReserva9.Columns(0).Visible And DeixaVerSelecaoTotalBoleto = True And VerBoletoAVista = True And
             hddResTipo.Value <> "H" And ExisteBoletoGerado = 0
            gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Visible = gdvReserva9.Columns(0).Visible
            'Segundo o Juliano, quando se tratar de Grupos não precisa exibir a opção de impressão de cupom
            gdvReserva9.HeaderRow.FindControl("imgCupom").Visible = gdvReserva9.Columns(0).Visible
            gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual").Visible = gdvReserva9.Columns(0).Visible And DeixaVerSelecaoTotalBoleto = True And hddResTipo.Value <> "H"
            gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante").Visible = gdvReserva9.Columns(0).Visible

            CType(gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante"), CheckBox).Visible = DeixaVerSelecaoTotalBoleto = True


            'Irá chamar o Relatório com os integrantes de grupo(Washington 05/09/2014)
            Session.Add("ResId", hddResId.Value)
            Session.Add("ResNome", txtResNome.Text.Trim.ToUpper)

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Session.Add("ResData", " no Período de " & Day(CDate(txtDataInicialSolicitacao.Text)) & " a " & txtDataFinalSolicitacao.Text & "(Sesc Caldas Novas)")
                Session.Add("ResDataPasseio", " Para o dia " & txtDataInicialSolicitacao.Text)
            Else
                Session.Add("ResData", " no Período de " & Day(CDate(txtDataInicialSolicitacao.Text)) & " a " & txtDataFinalSolicitacao.Text & "(Sesc Pirenópolis)")
                Session.Add("ResDataPasseio", " Para o dia " & txtDataInicialSolicitacao.Text)
            End If

            ''Não irá permitir imprimir boletos até migramos para boletos com registro (Gera apenas cupom)
            gdvReserva9.HeaderRow.FindControl("imgBoleto50").Visible = False
            gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual").Visible = False


            'Demonstrativo da relação dos  integrantes da excursão do SESC BIRIGUI/SP, no período de 12 a 17/04/2015 (Sesc Caldas Novas).'
            Session.Add("ResStatus", hddResStatus.Value)
            CType(gdvReserva9.HeaderRow.FindControl("imgListaIntegrante"), ImageButton).OnClientClick = "window.open('formRelIntegrantesPasseio.aspx');"
            gdvReserva9.HeaderRow.FindControl("imgListaIntegrante").Visible = (InStr("I", hddResCaracteristica.Value) = 0)

        End If
    End Sub

    Protected Sub imgBtnIntegranteNovoAcao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnIntegranteNovoAcao.Click

        If (txtResNome.Text = "Solicitação Incompleta" And txtResNome.Text.Trim.Length > "3") Then
            Mensagem("Favor preencher os dados do responsável da reserva.")
            pnlIntegranteTitulo.Visible = False
            txtResMatricula.Focus()
            Return
        End If

        'No click do nome do integrante, o postback ficou true para evitar um erro observado ao alterar o cpf e clicar em salvar diretamente
        hddProcessando.Value = ""
        hddIntId.Value = 0
        imgBtnDtCheck_InMenos.Visible = False
        imgBtnDtCheck_InMais.Visible = False
        imgBtnDtCheck_OutMenos.Visible = False
        imgBtnDtCheck_OutMais.Visible = False
        Limpar(pnlEdicaoIntegrante)
        Limpar(pnlIntegranteHospedagem)
        Limpar(pnlIntegranteEmissivo)
        pnlEdicaoIntegrante.Visible = True
        btnIntegranteGravar.Visible = False
        btnIntegranteExcluir.Visible = False
        btnIntUltimoRegistro.Visible = False
        imgBtnAlterarCategoria.Visible = False
        imgBtnAlterarMemorando.Visible = False
        imgBtnAlterarPagamento.Visible = False
        imgBtnAlterarRefeicao.Visible = False
        Desabilitar(pnlEdicaoIntegrante)
        Desabilitar(pnlIntegranteHospedagem)
        Desabilitar(pnlIntegranteEmissivo)
        txtIntMatricula.Enabled = True
        txtIntMatricula.Text = ""
        txtIntCPF.Enabled = True
        txtIntNome.Enabled = True
        txtIntNascimento.Enabled = True
        If (InStr("IET", hddResCaracteristica.Value) = 0) Then 'Emissivo
            pnlIntegranteHospedagem.Visible = False
            pnlIntegranteEmissivo.Visible = True
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If
            lista = objReservaListagemIntegranteDAO.consultarResponsavelViaResIdIntId(hddResId.Value, hddIntId.Value)
            cmbIntVinculoId.DataSource = lista
            cmbIntVinculoId.DataValueField = ("intId")
            cmbIntVinculoId.DataTextField = ("intNome")
            cmbIntVinculoId.DataBind()
            cmbIntVinculoId.Items.Insert(0, New ListItem("", "0"))
            gdvReserva11.DataSource = Nothing
            gdvReserva11.DataBind()
            gdvReserva12.DataSource = Nothing
            gdvReserva12.DataBind()
            hddSolId.Value = Nothing
            hddSolIdNovo.Value = Nothing
            txtIntMemorando.Text = Mid(txtResMemorando.Text.Trim, 1, 100)
            cmbIntEmissor.Text = cmbResEmissor.Text
            Try
                cmbIntCatId.Text = cmbResCatId.Text
            Catch ex As Exception

            End Try
            cmbIntCatCobranca.Text = cmbResCatCobranca.Text

            'Sempre carregará a lista com todos os Estados
            cmbIntEstId.Items.Clear()
            cmbIntEstId.DataValueField = ("estId")
            cmbIntEstId.DataTextField = ("estDescricao")
            CarregaTodosEstadosIntegrante("I")  '  Session.Item("Estados") 'listaEstadoAuxiliar
            cmbIntEstId.DataBind()

            cmbIntEstId.SelectedValue = cmbEstId.SelectedValue
            cmbIntEstId_SelectedIndexChanged(sender, e)
            If cmbEstId.SelectedValue < 1000 Then
                cmbIntCidade.Text = cmbResCidade.SelectedValue.Trim.ToUpper
            End If

            cmbPratoRapido.SelectedValue = cmbPratoRapido0.SelectedValue
        Else 'Hospedagem
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
                objCategoriaDAO = New Turismo.CategoriaDAO("TurismoSocialCaldas")
            Else
                objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
                objCategoriaDAO = New Turismo.CategoriaDAO("TurismoSocialPiri")
            End If

            If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Then
                cmbIntFormaPagamento.SelectedValue = "ER" 'Forma de pagamento Reserva
                cmbIntFormaPagamento.Enabled = False
            Else
                cmbIntFormaPagamento.Enabled = True
            End If

            lista = objCategoriaDAO.consultar("Reserva")
            cmbIntCatId.DataSource = lista
            cmbIntCatId.DataValueField = ("catId")
            cmbIntCatId.DataTextField = ("catDescricao")
            cmbIntCatId.DataBind()

            cmbIntCatId.Text = cmbResCatId.Text

            If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then 'Emissivo importara a Categoria de cobrança Memo e Emissor do responsável
                cmbIntCatCobranca.SelectedValue = cmbResCatCobranca.SelectedValue
            Else
                'Irá definir a categoria de cobrança sempre igual a categoria real
                Select Case cmbIntCatId.SelectedValue
                    Case 1, 2
                        cmbIntCatCobranca.SelectedValue = 1
                    Case 3
                        cmbIntCatCobranca.SelectedValue = 3
                    Case 4
                        cmbIntCatCobranca.SelectedValue = 4
                End Select
            End If

            gdvReserva12.DataSource = Nothing
            gdvReserva12.DataBind()
            gdvReserva12.SelectedIndex = -1
            lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, hddIntId.Value)
            If lista.Count > 1 Then
                Dim IndexAnterior = gdvReserva11.SelectedIndex
                gdvReserva11.DataSource = lista
                gdvReserva11.SelectedIndex = IndexAnterior
                gdvReserva11.DataBind()
            ElseIf lista.Count = 1 Or lista.Count = 0 Then
                gdvReserva11.DataSource = Nothing
                gdvReserva11.SelectedIndex = -1
                gdvReserva11.DataBind()
            End If
            objReservaListagemAcomodacaoVO = lista.Item(0)
            If lblAcomodacaoEscolhida.Attributes.Item("lblAcomodacaoEscolhida") = Nothing Then
                hddSolId.Value = objReservaListagemAcomodacaoVO.solId
            End If
            hddSolIdNovo.Value = objReservaListagemAcomodacaoVO.solId
            'Esse solId será utilizado para verificar se estourou o limite de usuarios de leit por apto
            radAcomodacao.Attributes.Add("solId", hddSolId.Value)
            ValidaTotaldeIntegrantesPorAcomodacao()

            If (imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" _
                And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" _
                And cmbOrgId.SelectedValue <> "37") Then ' And cmbOrgId.SelectedValue <> "49")) Then '(exceção apenas a Presidencia)
                Mensagem("Esgotado o número de vagas para essa acomodação.")
            Else
                If (cmbOrgId.SelectedValue = "37") Then
                    pnlEdicaoIntegrante.Visible = True
                End If
            End If
            'Na hora que inserir a data de nascimento do novo integrante, irei pegar a idade e ver se é adulto ou criança e fazer o tratamento
            'Final da verificação do máximo de pessoas no apto
            If (gdvReserva11.Rows.Count = 1 Or
                gdvReserva11.Rows.Count = 0 Or
                lblAcomodacaoEscolhida.Attributes.Item("lblAcomodacaoEscolhida") = Nothing Or
                sender Is btnIntegranteExcluir) Then
                lblAcomodacaoEscolhida.Text = "Acomodação " & objReservaListagemAcomodacaoVO.acmDescricao
                txtHosDataIniSol.Text = Format(CDate(objReservaListagemAcomodacaoVO.solDataIni), "dd/MM/yyyy")
                hddHosDataIniSol.Value = Format(CDate(objReservaListagemAcomodacaoVO.solDataIni), "dd/MM/yyyy")
                txtHosDataFimSol.Text = Format(CDate(objReservaListagemAcomodacaoVO.solDataFim), "dd/MM/yyyy")
                hddHosDataFimSol.Value = Format(CDate(objReservaListagemAcomodacaoVO.solDataFim), "dd/MM/yyyy")
                txtHosHoraIniSol.Text = objReservaListagemAcomodacaoVO.solHoraIni
                hddHosHoraIniSol.Value = objReservaListagemAcomodacaoVO.solHoraIni
                txtHosHoraFimSol.Text = objReservaListagemAcomodacaoVO.solHoraFim
                hddHosHoraFimSol.Value = objReservaListagemAcomodacaoVO.solHoraFim
                cmbAcomodacaoCobranca.Text = objReservaListagemAcomodacaoVO.acmId
                hddAcomodacaoCobranca.Value = objReservaListagemAcomodacaoVO.acmId
            ElseIf gdvReserva11.Rows.Count > 1 Then
                'Valore copiados no click da acomodação em gdvReserva11
                With lblAcomodacaoEscolhida.Attributes
                    lblAcomodacaoEscolhida.Text = .Item("lblAcomodacaoEscolhida")
                    txtHosDataIniSol.Text = .Item("txtHosDataIniSol")
                    hddHosDataIniSol.Value = .Item("hddHosDataIniSol")
                    txtHosDataFimSol.Text = .Item("txtHosDataFimSol")
                    hddHosDataFimSol.Value = .Item("hddHosDataFimSol")
                    txtHosHoraIniSol.Text = .Item("txtHosHoraIniSol")
                    hddHosHoraIniSol.Value = .Item("hddHosHoraIniSol")
                    txtHosHoraFimSol.Text = .Item("txtHosHoraFimSol")
                    hddHosHoraFimSol.Value = .Item("hddHosHoraFimSol")
                    cmbAcomodacaoCobranca.Text = .Item("cmbAcomodacaoCobranca")
                    hddAcomodacaoCobranca.Value = .Item("hddAcomodacaoCobranca")
                End With
            End If

            hddIntCPFAntes.Value = objReservaListagemIntegranteVO.intCPF

            'Sempre carregará a lista com todos os Estados
            CarregaTodosEstadosIntegrante("I")
            cmbIntEstId.DataBind()
            'Estado e cidade padrão igual ao da reserva
            cmbIntEstId.SelectedValue = cmbEstId.SelectedValue
            cmbIntEstId_SelectedIndexChanged(sender, e)
            If cmbEstId.SelectedValue < 1000 Then
                cmbIntCidade.Text = cmbResCidade.SelectedValue.Trim.ToUpper
            End If

            cmbPratoRapido.SelectedValue = cmbPratoRapido0.SelectedValue
            cmbIntEscolaridade.SelectedIndex = 0


            'Usado para encurtar o periodo de um integrante dentro de uma reserva
            imgBtnDtCheck_InMais.Attributes.Add("DataIniSolOld", hddResDataIni.Value)
            imgBtnDtCheck_InMais.Attributes.Add("DataFimSolOld", hddResDataFim.Value)

            imgBtnDtCheck_InMenos.Visible = False
            imgBtnDtCheck_InMais.Visible = False
            imgBtnDtCheck_OutMenos.Visible = False
            imgBtnDtCheck_OutMais.Visible = False

            radAcomodacao.Visible = False
            radAcomodacao.Enabled = False

            'Importando os dados da reserva para o integrante no caso de memorando.
            If cmbOrgId.SelectedItem.Text.Trim = "Presidência" Then
                ckbRefeicao.Items.Item(0).Selected = False
                ckbRefeicao.Items.Item(1).Selected = True
                ckbRefeicao.Items.Item(2).Selected = True
                ckbRefeicao.Items.Item(3).Selected = False
                txtIntRG.Text = "Não informado"
            Else
                ckbRefeicao.Items.Item(0).Selected = True
                ckbRefeicao.Items.Item(1).Selected = True
                ckbRefeicao.Items.Item(2).Selected = False
                ckbRefeicao.Items.Item(3).Selected = False
            End If

            txtIntMemorando.Text = Mid(txtResMemorando.Text.Trim, 1, 100)
            cmbIntEmissor.Text = cmbResEmissor.Text

            pnlIntegranteHospedagem.Visible = True
            pnlIntegranteEmissivo.Visible = False

            'Descarregando os dados copíados no salvamente anterior no novo integrante.
            If imgBtnIntegranteNovoAcao.Attributes.Item("IntCopiado") = "S" Then
                CopiaDadosIntegrante("D") 'D - Descarregando
            End If

        End If
        CarregaCmbRefeicaoPrato()
        pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "False"
        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
        pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "True"
        txtIntMatricula.Focus()
    End Sub

    Protected Sub lnkIntNome_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialPiri")
        End If
        'Ao clicar no integrante irá minimizar os dados da reserva
        If pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "false" Then
            pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
        End If
        If pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "false" Then
            pnlAcomodacaoTitulo_CollapsiblePanelExtender.ClientState = "True"
        End If
        btnIntUltimoRegistro.Visible = True

        'Coluna acomodação
        If gdvReserva9.Columns(3).Visible And CInt(sender.CommandArgument.ToString) > 0 Then
            hddHosId.Value = sender.CommandArgument.ToString
            objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaHosId(hddHosId.Value)
            hddIntId.Value = objReservaListagemIntegranteVO.intId
        Else
            hddIntId.Value = Math.Abs(CInt(sender.CommandArgument.ToString)).ToString
            objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaIntId(hddIntId.Value)
        End If

        'Atualizando os valores referente as faixas etárias dos integrantes
        objDefaultVO = objDefaultDAO.consultar()
        If Format(CDate(objReservaListagemIntegranteVO.intDiaIni), "yyyy-MM-dd") <= Format(CDate(hddDataFaixaEtaria.Value), "yyyy-MM-dd") Then
            hddIdadeIsento.Value = objDefaultVO.faixaEtariaIsento
            hddIdadeCrianca.Value = objDefaultVO.faixaEtariaCrianca
            hddIdadeAdulto.Value = objDefaultVO.faixaEtariaAdulto
        ElseIf Format(CDate(objReservaListagemIntegranteVO.intDiaIni), "yyyy-MM-dd") > Format(CDate(hddDataFaixaEtaria.Value), "yyyy-MM-dd") Then
            hddIdadeIsento.Value = objDefaultVO.FaixaEtariaIsento1
            hddIdadeCrianca.Value = objDefaultVO.FaixaEtariaCrianca1
            hddIdadeAdulto.Value = objDefaultVO.FaixaEtariaAdulto1
        End If

        'Pegando a data da reserva da acomodação e não somente do integrante em si (washington)
        imgBtnDtCheck_InMais.Attributes.Add("DataIniSolOld", objReservaListagemIntegranteVO.solDiaIni)
        imgBtnDtCheck_InMais.Attributes.Add("DataFimSolOld", objReservaListagemIntegranteVO.solDiaFim)

        'objReservaListagemIntegranteVO.intAlmoco

        CarregaCmbRefeicaoPrato()
        CarregaDadosIntegrante()
        If txtIntNome.Enabled Then
            txtIntNome.Focus()
        Else
            cmbIntFormaPagamento.Focus()
        End If

        If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Then
            lblAcomodacaoCobranca.Visible = False
            cmbAcomodacaoCobranca.Visible = False
            ckbRefeicao.Enabled = False
            imgBtnAlterarRefeicao.Visible = False
            HospedeJaControleCamposIntegrantes()
        End If
        txtIntValorUnitarioExc.Enabled = (hddMultiValorado.Value = "S")
    End Sub

    Protected Sub cmbIntEstId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbIntEstId.SelectedIndexChanged
        Try
            If cmbIntEstId.SelectedValue < 1000 Then
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                Else
                    objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                End If
                lista = objMunicipioDAO.consultarCidadePorEstado(cmbIntEstId.SelectedValue)
                cmbIntCidade.DataSource = Nothing
                cmbIntCidade.DataBind()
                cmbIntCidade.DataSource = lista
                cmbIntCidade.DataBind()
                If hddIntId.Value = 0 Then
                    cmbIntCidade.Visible = cmbIntEstId.Visible
                    cmbIntCidade.Enabled = cmbIntEstId.Enabled
                    cmbIntCidade.Focus()
                    txtIntCidade.Visible = False
                    txtIntCidade.Enabled = False
                End If
                txtIntCidade.Text = cmbIntCidade.SelectedItem.Text.Trim
            Else
                cmbIntCidade.Visible = False
                cmbIntCidade.Enabled = False
                txtIntCidade.Visible = True
                txtIntCidade.Enabled = True
                txtIntCidade.Text = ""
                cmbIntRG.SelectedValue = "OU"
                txtIntCidade.Focus()
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao carregar a lista de Estados. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: cmbIntEstId")
        End Try
    End Sub

    Protected Sub cmbIntCidade_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbIntCidade.TextChanged
        txtIntCidade.Text = cmbIntCidade.SelectedItem.Text.Trim
        cmbIntCidade.Focus()
    End Sub

    Protected Sub gdvReserva12_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva12.SelectedIndexChanged
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
        End If
        hddHosId.Value = gdvReserva12.DataKeys(gdvReserva12.SelectedIndex).Item(0).ToString
        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaHosId(hddHosId.Value)
        CarregaCmbRefeicaoPrato()
        CarregaDadosIntegrante()
    End Sub

    Protected Function calculaIdade(ByVal datanascimento As Date, ByVal DataCheckIn As Date) As Short
        'Dim dtNasc As Date = CDate(datanascimento)
        'Dim hoje As Date = CDate(Format(DataCheckIn, "yyyy-MM-dd"))  'CDate(Format(Date.Today, "yyyy-MM-dd"))
        Dim idade As Short = DateDiff(DateInterval.Year, datanascimento, DataCheckIn)

        If DatePart(DateInterval.Month, datanascimento) > DatePart(DateInterval.Month, DataCheckIn) Then
            idade -= 1
        ElseIf DatePart(DateInterval.Day, datanascimento) > DatePart(DateInterval.Day, DataCheckIn) And
               DatePart(DateInterval.Month, datanascimento) = DatePart(DateInterval.Month, DataCheckIn) Then
            idade -= 1
        End If
        Return idade
    End Function


    Protected Sub btnIntegranteGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIntegranteGravar.Click, btnIntegranteExcluir.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""
        Try
            If (Not sender Is btnIntegranteExcluir) Then
                'Calculando a idade do integrante
                If IsDate(txtIntNascimento.Text.Trim) Then
                    'hddIdade.Value = DateDiff(DateInterval.Year, CDate(txtIntNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
                    hddIdade.Value = calculaIdade(CDate(txtIntNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
                Else
                    txtIntNascimento.Text = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de nascimento informada não é válida.');", True)
                    txtIntNascimento.Focus()
                    Return
                End If

                'Irá verificar se a inserção será permitida, respeitando o limite máximo de leitos por acomodação

                If (hddResCaracteristica.Value = "I" And cmbOrgId.SelectedValue <> "37") Then ' And cmbOrgId.SelectedValue <> "49")) Then 'Passeio e Grupos não entram na questão - exceção: Presidencia
                    If imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" Then
                        Mensagem("Esgotado o número de vagas para essa acomodação.")
                        Exit Try
                    ElseIf (imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And hddIdade.Value >= CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                        If hddIntId.Value = 0 Then 'Se estiver inserindo
                            Mensagem("Vagas esgotadas para essa acomodação. \nObs.: Será possível inserir apenas integrantes com idade de berço, que tenha menos de " & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos.\n\n Inserção não permitida.")
                            imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                            Exit Try
                        End If
                    ElseIf ((imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "L" And
                            imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N") And
                            hddIdade.Value > CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                        Mensagem("A idade do integrante não poderá ser superior a idade de criança de berço (" & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos). \n\n Inserção não permitida.")
                        imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                        Exit Try
                    End If
                End If
                'End If

                'Verificando se esta preenchido o responsável para menos de idade no caso de grupo de passantes

                If hddResCaracteristica.Value = "P" Then
                    If hddIdade.Value < CInt(hddIdadeCrianca.Value) - 1 And cmbIntVinculoId.SelectedIndex = 0 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Isento necessita de responsável.');", True)
                        cmbIntVinculoId.Focus()
                        Return
                    End If
                    If txtIntFoneResponsavelExc.Text.Trim.Length < 8 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Informe o número do telefone do integrante.');", True)
                        txtIntFoneResponsavelExc.Focus()
                        Return
                    End If
                End If
                If Not sender Is btnIntegranteExcluir Then
                    If txtIntNome.Text.Trim.Length = 0 Or txtIntNascimento.Text.Trim.Length = 0 Or txtIntNascimento.Text.Trim.Length < 10 Or txtIntRG.Text.Trim.Length = 0 Then
                        If hddResCaracteristica.Value <> "E" Then
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique:\n\nNome do Integrante.\nData de Nascimento.\nDocumento de Identificação.\n\nOs campos são de preenchimento obrigatório.');", True)
                            txtIntNome.Focus()
                            Return
                        End If
                    End If

                    'Se estiver salvando e não possuir CPF cadastrato, for maior de 18 e for cortesia ou Free enviará a mensagem solicitado informar o cpf
                    'If (txtIntCPF.Text.Trim.Length = 0 And (hddIntStatus.Value.Trim = "E" Or hddIntStatus.Value.Trim = "P") And (cmbIntFormaPagamento.SelectedValue.Trim = "F" Or cmbIntFormaPagamento.SelectedValue.Trim = "C") And hddIdade.Value >= 18) Then

                    'Se for maior de 18 anos, sem CPF informado e não for da Presidência ou estrangeiro, exige-se a inserção do CPF
                    If (hddIdade.Value >= 18 And txtIntCPF.Text.Trim.Length = 0 And cmbIntEmissor.SelectedValue <> "1" And hddResCaracteristica.Value <> "P" And cmbIntEstId.SelectedValue < 1000) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('CPF obrigatório para maiores de 18 anos.');", True)
                        txtIntCPF.Focus()
                        Return
                    End If
                End If

                'Verificando se a matrícula e o cpf são númericos
                If txtIntMatricula.Text.Trim.Length > 0 Then
                    If Not IsNumeric(txtIntMatricula.Text.Replace(" ", "")) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela deve ser composta de apenas números.');", True)
                        txtIntMatricula.Focus()
                        Return
                    ElseIf txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length < 11 Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela precisa conter no mínimo 11 caracteres.');", True)
                        txtIntMatricula.Focus()
                        Return
                    End If
                End If

                If txtIntCPF.Text.Trim.Length > 0 Then
                    If Not IsNumeric(txtIntCPF.Text.Replace(" ", "")) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O CPF informado não é válido, ele deve ser composto de apenas números.');", True)
                        txtIntCPF.Focus()
                        Return
                    End If
                End If
            End If 'Final da verificação se esta excluindo

            'Quando o titular da reserva é inserido sem marcar a opção de levá-lo como integrante, acontece desses campos
            'Abaixo não ficarem preenchidos, para evitar erro no sistema, eles serão checados, Não validará para passante do tipo "P"

            If Not sender Is btnIntegranteExcluir And hddResCaracteristica.Value = "I" Then
                If lblAcomodacao.Text = "Acomodação" Or lblAcomodacao.Text.Trim.Length = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Por favor selecione uma das acomodações para o integrante.');", True)
                    radAcomodacao.Focus()
                    Return
                End If

                If Not IsDate(txtHosDataIniSol.Text.Trim) Then
                    txtHosDataIniSol.Text = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de Inicial da Solicitação informada não é válida,\nA acomodação já foi escolhida para o integrante?');", True)
                    Return
                End If

                If Not IsDate(txtHosDataFimSol.Text.Trim) Then
                    txtHosDataFimSol.Text = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de Final da Solicitação informada não é válida,\nA acomodação já foi escolhida para o integrante?');", True)
                    Return
                End If
            End If


            'Verifica se a data de solicitação foi alterada e se esta dentro do período permitido/Se estiver excluindo não irá checar essa data
            If (pnlIntegranteHospedagem.Visible = True And Not sender Is btnIntegranteExcluir) Then
                If (txtHosDataIniSol.Text.Length < 10 Or
                    txtHosDataFimSol.Text.Length < 10) Or
                    CDate(txtHosDataIniSol.Text) < CDate(imgBtnDtCheck_InMais.Attributes.Item("DataIniSolOld")) Or
                    CDate(txtHosDataFimSol.Text) > CDate(imgBtnDtCheck_InMais.Attributes.Item("DataFimSolOld")) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique: \n\n- Se a data inicial ou final da solicitação estão fora dos limites permitidos;\n- Se as datas digitadas são válidas.');", True)
                    txtHosDataIniSol.Focus()
                    Return
                End If
            End If
            'Se a data inicial for maior que a final... barra
            If Not sender Is btnIntegranteExcluir And pnlIntegranteHospedagem.Visible = True Then
                If (CDate(txtHosDataIniSol.Text) > CDate(txtHosDataFimSol.Text)) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A data inicial não pode ser maior que a data final.');", True)
                    txtHosDataIniSol.Focus()
                    Return
                End If
            End If

            If Not sender Is btnIntegranteExcluir And (cmbIntCatId.Text = 3 And hddAutorizaConveniado.Value = "N" And InStr("IET", hddResCaracteristica.Value) = 0) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria Conveniado sem disponibilidade.');", True)
                cmbIntCatId.Focus()
            ElseIf Not sender Is btnIntegranteExcluir And (cmbIntCatId.Text = 4 And hddAutorizaUsuario.Value = "N" And InStr("IET", hddResCaracteristica.Value) = 0) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria Usuário sem disponibilidade.');", True)
                cmbIntCatId.Focus()
                'Tratando comerciário/Conveniado - Sempre deverá ter a carteira digitada/Se tiver excluindo não irá checar.

            ElseIf Not sender Is btnIntegranteExcluir _
            And (cmbIntCatId.Text <> 4 And txtIntMatricula.Text.Trim.Length = 0) _
            And InStr("IEP", hddResCaracteristica.Value) <> 0 _
            And (hddResCaracteristica.Value <> "E" And hddResCaracteristica.Value <> "T") _
            And CInt(hddIdade.Value.Replace(".", ",")) >= CInt(hddIdadeCrianca.Value) Then

                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria necessita informar matrícula.');", True)
                cmbIntCatId.Focus()

            ElseIf Not sender Is btnIntegranteExcluir And (txtIntCPF.Text.Trim = "") And (CInt(hddIdade.Value.Replace(".", ",")) >= 18) And
                    Not Session("GrupoRecepcao") And Not Session("GrupoRecepcaoPiri") And
                   (InStr("IET", hddResCaracteristica.Value) = 0 And ckbOrganizadoSESC.Checked) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('CPF obrigatório para maiores de 18 anos.');", True)
                txtIntCPF.Focus()
            ElseIf Not sender Is btnIntegranteExcluir And (txtIntCPF.Text.Trim = "") And (CInt(hddIdade.Value.Replace(".", ",")) >= 18) And
                    Not Session("GrupoRecepcao") And Not Session("GrupoRecepcaoPiri") And
                   (InStr("I", hddResCaracteristica.Value) <> 0) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('CPF obrigatório para maiores de 18 anos.');", True)
                txtIntCPF.Focus()
            ElseIf ((cmbIntCatId.Text = cmbIntCatCobranca.Text) Or (cmbIntCatId.Text = 2 And cmbIntCatCobranca.Text = 1)) Or
              ((cmbIntCatId.Text > cmbIntCatCobranca.Text) And (txtIntMemorando.Text.Trim > "") And cmbIntEmissor.Text > "0") Or
                ((hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T") And txtIntMatricula.Text.Length = 0) Or
                (hddResCaracteristica.Value = "P" And txtIntMatricula.Text.Length = 0) Then


                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                    objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                Else
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                    objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                End If

                objReservaListagemIntegranteVO = New Turismo.ReservaListagemIntegranteVO
                objReservaListagemIntegranteVO.intId = hddIntId.Value
                objReservaListagemIntegranteVO.resId = hddResId.Value
                objReservaListagemIntegranteVO.intMatricula = txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "")
                If txtHosDataIniSol.Text > "" Then
                    objReservaListagemIntegranteVO.intDiaIni = Format(CDate(txtHosDataIniSol.Text), "dd/MM/yyyy")
                Else
                    objReservaListagemIntegranteVO.intDiaIni = Format(CDate(hddResDataIni.Value), "dd/MM/yyyy")
                End If
                objReservaListagemIntegranteVO.intHoraIni = txtHosHoraIniSol.Text

                If btnEmissivoNova.Enabled Then
                    objReservaListagemIntegranteVO.intDiaFim = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")
                Else
                    If txtHosDataFimSol.Text > "" Then
                        objReservaListagemIntegranteVO.intDiaFim = Format(CDate(txtHosDataFimSol.Text), "dd/MM/yyyy")
                    Else
                        If Not sender Is btnIntegranteExcluir Then
                            If pnlIntegranteHospedagem.Visible = True Then
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data final da solicitação precisa ser preenchida.');", True)
                                Return
                            End If
                        End If
                    End If
                End If

                'Modo original a linha abaixo esta ativa
                'objReservaListagemIntegranteVO.intDiaFim = Format(CDate(txtDataFinalSolicitacao.Text), "dd/MM/yyyy")

                'Else
                'If txtHosDataFimSol.Text > "" Then
                'objReservaListagemIntegranteVO.intDiaFim = Format(CDate(txtHosDataFimSol.Text), "dd/MM/yyyy")
                'Else

                'End If
                'End If
                objReservaListagemIntegranteVO.intHoraFim = txtHosHoraFimSol.Text
                objReservaListagemIntegranteVO.catId = cmbIntCatId.Text
                objReservaListagemIntegranteVO.IntNome = txtIntNome.Text.Trim.Replace("'", "")
                objReservaListagemIntegranteVO.intRG = Mid(cmbIntRG.Text + " - " + txtIntRG.Text.Replace("'", ""), 1, 25)
                objReservaListagemIntegranteVO.intCPF = txtIntCPF.Text.Replace(" ", "")
                objReservaListagemIntegranteVO.intDtNascimento = Format(CDate(txtIntNascimento.Text), "dd/MM/yyyy")
                objReservaListagemIntegranteVO.intSexo = cmbIntSexo.Text
                Try
                    objReservaListagemIntegranteVO.solId = hddSolId.Value
                Catch ex As Exception
                    objReservaListagemIntegranteVO.solId = Nothing
                End Try
                objReservaListagemIntegranteVO.intFormaPagamento = cmbIntFormaPagamento.Text.ToString.Trim
                objReservaListagemIntegranteVO.intCatCobranca = cmbIntCatCobranca.Text
                objReservaListagemIntegranteVO.intMemorando = txtIntMemorando.Text.Replace("'", "")
                '@Acao char(1), -- funções (I - incluir integrante) (D - excluir integrante) (A - alterar integrante) (H - incluir hospedagem) (T - excluir hospedagem) (M - modifica hospedagem)
                objReservaListagemIntegranteVO.intEmissor = cmbIntEmissor.Text
                objReservaListagemIntegranteVO.acmIdCobranca = cmbAcomodacaoCobranca.Text
                objReservaListagemIntegranteVO.estId = cmbIntEstId.Text
                If txtIntCidade.Text.ToUpper Like "GOI?NIA" Then
                    objReservaListagemIntegranteVO.intCapitalGoias = "S"
                Else
                    objReservaListagemIntegranteVO.intCapitalGoias = "N"
                End If
                objReservaListagemIntegranteVO.intCidade = txtIntCidade.Text.Trim.Replace("'", "")
                '@Transferencia char(1), -- (S)im ou (N)ão
                If ckbRefeicao.Items.Item(0).Selected = True And
                    ckbRefeicao.Items.Item(2).Selected = False Then
                    objReservaListagemIntegranteVO.intAlmoco = "I0"
                ElseIf ckbRefeicao.Items.Item(0).Selected = False And
                    ckbRefeicao.Items.Item(2).Selected = True Then
                    objReservaListagemIntegranteVO.intAlmoco = "O0"
                ElseIf ckbRefeicao.Items.Item(0).Selected = True And
                    ckbRefeicao.Items.Item(2).Selected = True Then
                    objReservaListagemIntegranteVO.intAlmoco = "I1"
                Else
                    objReservaListagemIntegranteVO.intAlmoco = ""
                End If
                If ckbRefeicao.Items.Item(1).Selected = True And
                    ckbRefeicao.Items.Item(3).Selected = False Then
                    objReservaListagemIntegranteVO.intJantar = "I0"
                ElseIf ckbRefeicao.Items.Item(1).Selected = False And
                    ckbRefeicao.Items.Item(3).Selected = True Then
                    objReservaListagemIntegranteVO.intJantar = "O0"
                ElseIf ckbRefeicao.Items.Item(1).Selected = True And
                    ckbRefeicao.Items.Item(3).Selected = True Then
                    objReservaListagemIntegranteVO.intJantar = "I1"
                Else
                    objReservaListagemIntegranteVO.intJantar = ""
                End If
                objReservaListagemIntegranteVO.intUsuario = Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")
                objReservaListagemIntegranteVO.intSalario = cmbIntSalario.Text
                objReservaListagemIntegranteVO.intEscolaridade = cmbIntEscolaridade.Text
                objReservaListagemIntegranteVO.intEstadoCivil = cmbIntEstadoCivil.Text

                Try
                    objReservaListagemIntegranteVO.intVinculoId = cmbIntVinculoId.Text
                Catch ex As Exception
                    objReservaListagemIntegranteVO.intVinculoId = Nothing
                End Try
                objReservaListagemIntegranteVO.intFoneResponsavelExc = txtIntFoneResponsavelExc.Text.Replace("(", "").Replace(") ", "").Replace(" ", "").Trim
                objReservaListagemIntegranteVO.intLocalTrabalhoResponsavelExc = txtIntLocalTrabalhoResponsavelExc.Text
                objReservaListagemIntegranteVO.intEnderecoResponsavelExc = txtIntEnderecoResponsavelExc.Text
                objReservaListagemIntegranteVO.intBairroResponsavelExc = txtIntBairroResponsavelExc.Text
                Try
                    If hddMultiValorado.Value = "N" Then
                        If InStr("CF", cmbIntFormaPagamento.Text.Trim) = 0 Then 'Cortesia ou Free
                            If CDec(hddIdade.Value.Replace(".", ",")) >= CInt(hddIdadeAdulto.Value) Then
                                If cmbIntCatCobranca.Text = "1" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblComAdultoVlr.Text
                                ElseIf cmbIntCatCobranca.Text = "3" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblConvAdultoVlr.Text
                                Else
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblUsuAdultoVlr.Text
                                End If
                            ElseIf CDec(hddIdade.Value.Replace(".", ",")) >= CInt(hddIdadeCrianca.Value) Then
                                If cmbIntCatCobranca.Text = "1" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblComCriancaVlr.Text
                                ElseIf cmbIntCatCobranca.Text = "3" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblConvCriancaVlr.Text
                                Else
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblUsuCriancaVlr.Text
                                End If
                            Else
                                If cmbIntCatCobranca.Text = "1" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblComIsentoVlr.Text
                                ElseIf cmbIntCatCobranca.Text = "3" Then
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblConvIsentoVlr.Text
                                Else
                                    objReservaListagemIntegranteVO.intValorUnitarioExc = lblUsuIsentoVlr.Text
                                End If
                            End If
                        Else
                            objReservaListagemIntegranteVO.intValorUnitarioExc = "0,00"
                        End If
                    Else
                        objReservaListagemIntegranteVO.intValorUnitarioExc = txtIntValorUnitarioExc.Text
                    End If
                Catch ex As Exception
                    objReservaListagemIntegranteVO.intValorUnitarioExc = 0
                End Try
                objReservaListagemIntegranteVO.intPoltronaExc = txtIntPoltronaExc.Text
                objReservaListagemIntegranteVO.intApartamentoExc = txtIntApartamentoExc.Text
                objReservaListagemIntegranteVO.intPratoRefeicao = cmbPratoRapido.Text
                If hddIdadeColo.Value = "1000" Then
                    If ckbColo.Checked Then
                        objReservaListagemIntegranteVO.intCriancaColoExc = "S"
                    Else
                        objReservaListagemIntegranteVO.intCriancaColoExc = "N"
                    End If
                Else
                    If CDec(hddIdade.Value.Replace(".", ",")) < CInt(hddIdadeColo.Value) Then
                        objReservaListagemIntegranteVO.intCriancaColoExc = "S"
                    Else
                        objReservaListagemIntegranteVO.intCriancaColoExc = "N"
                    End If
                End If

                Dim strResultado As New Turismo.ReservaListagemIntegranteDAO.Resultado
                If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                    If sender Is btnIntegranteGravar Then
                        strResultado = objReservaListagemIntegranteDAO.AcaoEmissivo(objReservaListagemIntegranteVO, "N")
                    Else ' btnIntegranteExcluir / só poderá excluir um integrante quem estiver devidamente inserido no grupo abaixo
                        Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
                        Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
                        If Grupos.Contains("Turismo Social Excluir Integrante Passeio") Then
                            strResultado = objReservaListagemIntegranteDAO.AcaoEmissivo(objReservaListagemIntegranteVO, "S")
                        Else
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro não apagado!\n\nVocê não possui permissão para excluir integrantes de grupo de passeio.');", True)
                            Return
                        End If
                    End If
                    If strResultado.mensagem.Trim = "OK" Then
                        hddIntId.Value = strResultado.intId.ToString
                        'If sender Is btnIntegranteExcluir Then
                        imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registrado com sucesso.');", True)
                        CType(Page.Master.FindControl("scpMngTurismoSocial"), ScriptManager).SetFocus(txtIntMatricula)
                        'End If
                    Else
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                    End If

                Else 'Hospedagem
                    If objReservaListagemIntegranteVO.intId = 0 Then
                        strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "I")
                        If strResultado.mensagem.Trim = "" Then
                            CopiaDadosIntegrante("C") 'Copia o básico do integrante anterior para o proximo registro.
                            imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registrado com sucesso.');", True)
                            CType(Page.Master.FindControl("scpMngTurismoSocial"), ScriptManager).SetFocus(txtIntMatricula)
                        Else
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                        End If
                    Else
                        If sender Is btnIntegranteGravar Then
                            If hddIntStatus.Value.Trim = "" Then
                                strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "A")
                            Else
                                strResultado.mensagem = ""
                            End If

                            'Copia o básico do integrante anterior para o proximo registro.
                            CopiaDadosIntegrante("C")

                            If strResultado.mensagem.Trim = "" Then
                                If hddSolId.Value = hddSolIdNovo.Value Then
                                    If hddHosDataIniSol.Value <> txtHosDataIniSol.Text Or
                                       hddHosDataFimSol.Value <> txtHosDataFimSol.Text Or
                                       hddHosHoraIniSol.Value <> txtHosHoraIniSol.Text Or
                                       hddHosHoraFimSol.Value <> txtHosHoraFimSol.Text Or
                                       hddIntCPFAntes.Value <> txtIntCPF.Text.Trim.Replace(" ", "") Or
                                       btnIntegranteGravar.Attributes.Item("NomeAntes").ToString <> txtIntNome.Text.Trim Or
                                       btnIntegranteGravar.Attributes.Item("NascimentoAntes").ToString <> txtIntNascimento.Text Or
                                       btnIntegranteGravar.Attributes.Item("SexoAntes").ToString <> cmbIntSexo.SelectedValue Or
                                       btnIntegranteGravar.Attributes.Item("intAlmoco").ToString <> objReservaListagemIntegranteVO.intAlmoco Or
                                       btnIntegranteGravar.Attributes.Item("intJantar").ToString <> objReservaListagemIntegranteVO.intJantar Or
                                       hddAcomodacaoCobranca.Value <> cmbAcomodacaoCobranca.Text Then
                                        strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "M")
                                    End If
                                Else
                                    strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "H")
                                End If
                                If strResultado.mensagem.Trim = "" Then
                                    'Ocultando o grid de Integrante
                                    pnlEdicaoIntegrante.Visible = False
                                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registrado com sucesso.');", True)
                                    imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                                    CType(Page.Master.FindControl("scpMngTurismoSocial"), ScriptManager).SetFocus(txtIntMatricula)
                                Else
                                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                                End If
                            Else
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                            End If
                        ElseIf sender Is btnIntegranteExcluir Then
                            If gdvReserva12.Rows.Count > 1 Then
                                strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "T")
                            Else
                                strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "D")
                            End If

                            If strResultado.mensagem.Trim = "" Then
                                imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                            Else
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                            End If
                        ElseIf sender Is radAcomodacao Then
                            strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "T")
                            If strResultado.mensagem.Trim = "" Then
                                objReservaListagemIntegranteVO.solId = Mid(radAcomodacao.SelectedValue, 1, radAcomodacao.SelectedValue.ToString.IndexOf("#"))
                                objReservaListagemIntegranteVO.acmIdCobranca = Mid(radAcomodacao.SelectedValue, radAcomodacao.SelectedValue.ToString.IndexOf("#") + 2)
                                strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "H")
                                If strResultado.mensagem.Trim = "" Then
                                    imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                                End If
                            Else
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                            End If
                        ElseIf sender Is imgBtnAlterarPagamento Then
                            objReservaListagemIntegranteDAO.AtualizaIntegranteFormaPagamento(
                              objReservaListagemIntegranteVO.resId,
                              objReservaListagemIntegranteVO.intUsuario,
                              objReservaListagemIntegranteVO.intFormaPagamento)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Forma de Pagamento registrado.');", True)
                        ElseIf sender Is imgBtnAlterarMemorando Then
                            objReservaListagemIntegranteDAO.AtualizaIntegranteMemorando(
                              objReservaListagemIntegranteVO.resId,
                              objReservaListagemIntegranteVO.intUsuario,
                              objReservaListagemIntegranteVO.intMemorando,
                              objReservaListagemIntegranteVO.intEmissor)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Memorando e Emissor registrado.');", True)
                        ElseIf sender Is imgBtnAlterarCategoria Then
                            objReservaListagemIntegranteDAO.AtualizaIntegranteCategoriaCobranca(
                              objReservaListagemIntegranteVO.resId,
                              objReservaListagemIntegranteVO.intUsuario,
                              objReservaListagemIntegranteVO.intCatCobranca)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria de Cobrança registrado.');", True)
                        ElseIf sender Is imgBtnAlterarRefeicao Then
                            objReservaListagemIntegranteDAO.AtualizaIntegranteRefeicao(
                              objReservaListagemIntegranteVO.resId,
                              objReservaListagemIntegranteVO.intUsuario,
                              objReservaListagemIntegranteVO.intAlmoco,
                              objReservaListagemIntegranteVO.intJantar)
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Refeições registrado.');", True)
                        End If
                    End If
                    'Atualizando a lista de acomodações e seus integrantes
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
                    Else
                        objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
                    End If
                    lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
                    gdvReserva8.DataSource = lista
                    gdvReserva8.DataBind()
                End If
                objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                CarregaCmbRefeicaoPrato()
                CarregaDadosReserva()
                ListaIntegranteViaResId()
                Me.pnlIntegranteTitulo_CollapsiblePanelExtender.ClientState = "False"

            ElseIf (Not sender Is btnIntegranteExcluir) And ((cmbIntCatId.Text > cmbIntCatCobranca.Text) And (txtIntMemorando.Text.Trim = "" And (hddResCaracteristica.Value <> "E" And hddResCaracteristica.Value <> "T"))) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Memorando não foi preenchido.');", True)
                txtIntMemorando.Focus()
            ElseIf (Not sender Is btnIntegranteExcluir) And ((cmbIntCatId.Text > cmbIntCatCobranca.Text) And (cmbIntEmissor.Text = "0") And (hddResCaracteristica.Value <> "E" And hddResCaracteristica.Value <> "T")) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Emissor não foi preenchido.');", True)
                cmbIntEmissor.Focus()
            ElseIf hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria de Cobrança inválida.');", True)
                'Quando for Grupo Deixará salvar mesmo sem matricula
            Else
                If (Not sender Is btnIntegranteExcluir) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Categoria de Cobrança inválida.');", True)
                    cmbIntCatCobranca.Focus()
                End If
            End If

            'Carregando o grid com as informações financeiras, Ressarcimento.
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
            Else
                objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
            End If
            If hddResId.Value > 0 Then
                gdvRessarcimento.DataSource = objReservaListagemFinanceiroDAO.ConsultaRessarcimento(hddResId.Value)
                gdvRessarcimento.DataBind()
            End If
            'Atualizando lista de Boletos gerados
            ListaFinanceiroViaResId()
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
            "&Erro=Erro ao salvar o integrante" & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnIntegranteExcluir Intid:" & hddIntId.Value & " ResId: " & hddResId.Value & " ")
        End Try
    End Sub

    Protected Sub txtIntNome_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If txtIntMatricula.Text.Trim = "" And txtIntNascimento.Text <> "__/__/____" Then
                txtIntMatricula.Enabled = False
                txtIntNascimento.Enabled = True
                cmbIntSexo.Enabled = True
                cmbIntFormaPagamento.Enabled = True
                txtIntMemorando.Enabled = True
                cmbIntEmissor.Enabled = True
                cmbIntCatId.Enabled = True
                cmbIntCatId.Items.Clear()
                Try
                    Dim varIdade As Integer = calculaIdade(CDate(txtIntNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
                    If CDate(hddResDataIni.Value).Month < CDate(txtIntNascimento.Text).Month Or (CDate(hddResDataIni.Value).Month = CDate(txtIntNascimento.Text).Month And CDate(hddResDataIni.Value).Day < CDate(txtIntNascimento.Text).Day) Then
                        varIdade = varIdade - 1
                    End If
                    If varIdade >= hddIdadeColo.Value Then
                        cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                        cmbIntCatId.Text = 4
                    Else
                        'cmbIntCatId.Items.Insert(0, New ListItem("Dependente", "2"))
                        'cmbIntCatId.Items.Insert(1, New ListItem("Conveniado", "3"))
                        cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                        cmbIntCatId.Text = 4
                    End If
                Catch ex As Exception
                    cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                    cmbIntCatId.Text = 4
                End Try

                cmbIntCatCobranca.Enabled = True

                ckbRefeicao.Enabled = True

                If cmbIntEstId.Items.Count <> cmbEstId.Items.Count Then
                    cmbIntEstId.Items.Clear()
                    Dim It As ListItem
                    For Each It In cmbEstId.Items
                        cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                    Next
                End If
                cmbIntEstId.Enabled = True
                cmbIntEstId_SelectedIndexChanged(sender, e)
                cmbIntSalario.Enabled = True
                cmbIntEscolaridade.Enabled = True
                cmbIntEstadoCivil.Enabled = True
                cmbAcomodacaoCobranca.Enabled = True
                txtIntCPF.Enabled = True
                txtIntRG.Enabled = True
                cmbIntRG.Enabled = True
                cmbIntVinculoId.Enabled = True
                txtIntFoneResponsavelExc.Enabled = True
                txtIntLocalTrabalhoResponsavelExc.Enabled = True
                txtIntEnderecoResponsavelExc.Enabled = True
                txtIntBairroResponsavelExc.Enabled = True
                txtIntValorUnitarioExc.Enabled = (hddMultiValorado.Value = "S")
                txtIntPoltronaExc.Enabled = True
                ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                txtIntApartamentoExc.Enabled = True
                cmbPratoRapido.Enabled = True
                btnIntegranteGravar.Visible = True
                btnIntegranteExcluir.Visible = False
                imgBtnAlterarCategoria.Visible = False
                imgBtnAlterarMemorando.Visible = False
                imgBtnAlterarPagamento.Visible = False
                imgBtnAlterarRefeicao.Visible = False
            End If
            If txtIntNascimento.Text = "__/__/____" Or txtIntNascimento.Text = "" Then
                If txtIntNome.Text.Trim.Length > 0 Then
                    txtIntNascimento.Focus()
                End If
            Else
                cmbIntSexo.Focus()
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao carregar informações do Integrante. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: txtIntNome")
        End Try
    End Sub

    Protected Sub gdvReserva11_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva11.SelectedIndexChanged
        hddSolId.Value = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(1).ToString()

        'Atualizando a data de base para alterar o período dareserva
        imgBtnDtCheck_InMais.Attributes.Add("DataIniSolOld", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(3).ToString())
        imgBtnDtCheck_InMais.Attributes.Add("DataFimSolOld", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(4).ToString())

        lblAcomodacaoEscolhida.Text = "Acomodação " & gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(7).ToString()
        txtHosDataIniSol.Text = Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(3).ToString()), "dd/MM/yyyy")
        hddHosDataIniSol.Value = Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(3).ToString()), "dd/MM/yyyy")
        txtHosDataFimSol.Text = Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(4).ToString()), "dd/MM/yyyy")
        hddHosDataFimSol.Value = Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(4).ToString()), "dd/MM/yyyy")
        txtHosHoraIniSol.Text = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(5).ToString()
        hddHosHoraIniSol.Value = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(5).ToString()
        txtHosHoraFimSol.Text = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(6).ToString()
        hddHosHoraFimSol.Value = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(6).ToString()
        cmbAcomodacaoCobranca.Text = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(0).ToString()
        hddAcomodacaoCobranca.Value = gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(0).ToString()

        radAcomodacao.Attributes.Add("solId", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item("solId").ToString())
        ValidaTotaldeIntegrantesPorAcomodacao()
        radAcomodacao.Attributes.Remove("solId")


        imgBtnDtCheck_InMenos.Visible = False
        imgBtnDtCheck_InMais.Visible = False
        imgBtnDtCheck_OutMenos.Visible = False
        imgBtnDtCheck_OutMais.Visible = False

        If txtIntMatricula.Enabled Then
            txtIntMatricula.Focus()
        Else
            txtIntNome.Focus()
        End If

        'Armazenando os mesmos dados em variáveis para posterior descarrego.
        With lblAcomodacaoEscolhida.Attributes
            .Add("lblAcomodacaoEscolhida", "Acomodação " & gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(7).ToString())
            .Add("txtHosDataIniSol", Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(3).ToString()), "dd/MM/yyyy"))
            .Add("hddHosDataIniSol", Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(3).ToString()), "dd/MM/yyyy"))
            .Add("txtHosDataFimSol", Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(4).ToString()), "dd/MM/yyyy"))
            .Add("hddHosDataFimSol", Format(CDate(gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(4).ToString()), "dd/MM/yyyy"))
            .Add("txtHosHoraIniSol", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(5).ToString())
            .Add("hddHosHoraIniSol", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(5).ToString())
            .Add("txtHosHoraFimSol", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(6).ToString())
            .Add("hddHosHoraFimSol", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(6).ToString())
            .Add("cmbAcomodacaoCobranca", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(0).ToString())
            .Add("hddAcomodacaoCobranca", gdvReserva11.DataKeys(gdvReserva11.SelectedIndex).Item(0).ToString())
        End With
    End Sub

    Protected Sub radAcomodacao_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radAcomodacao.SelectedIndexChanged

        radAcomodacao.Attributes.Add("solId", Mid(radAcomodacao.SelectedValue, 1, radAcomodacao.SelectedValue.IndexOf("#")))
        ValidaTotaldeIntegrantesPorAcomodacao()

        'Irá verificar se a inserção será permitida, respeitando o limite máximo de leitos por acomodação
        'Passeio e Grupos não entram na questão - excessão: Presidencia e Dr Hddintid=0 quer dizer inserção
        If (hddResCaracteristica.Value = "I" And cmbOrgId.SelectedValue <> "37") Then ' And cmbOrgId.SelectedValue <> "49")) Then 'Passeio e Grupos não entram na questão - exceção: Presidencia
            If imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" Then
                Mensagem("Esgotado o número de vagas para essa acomodação.")
                Return
            ElseIf (imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And hddIdade.Value >= CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                If hddIntId.Value > 0 Then
                    Mensagem("Vagas esgotadas para essa acomodação. \nObs.: Será possível inserir apenas integrantes com idade de berço, que tenha menos de " & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos.\n\n Inserção não permitida.")
                    imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                    Return
                End If
            ElseIf ((imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "L" And
                    imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N") And
                    hddIdade.Value > CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                Mensagem("A idade do integrante não poderá ser superior a idade de criança de berço (" & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos). \n\n Inserção não permitida.")
                imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                Return
            End If
        End If
        btnIntegranteGravar_Click(sender, e)
        radAcomodacao.Attributes.Remove("solId")
    End Sub

    Protected Sub ckbHeadIntegrante_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each linha As GridViewRow In gdvReserva9.Rows
            If (sender.Checked And CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Visible And
                sender.Checked And CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True) Then
                CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
            Else
                CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked = False
            End If
        Next

        CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Visible = False

        If gdvReserva9.HeaderRow.FindControl("imgBoleto50").Visible Then
            gdvReserva9.HeaderRow.FindControl("imgBoleto50").Focus()
        ElseIf gdvReserva9.HeaderRow.FindControl("imgBoleto").Visible Then
            gdvReserva9.HeaderRow.FindControl("imgBoleto").Focus()
        ElseIf gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual").Visible Then
            gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual").Focus()
        ElseIf gdvReserva9.HeaderRow.FindControl("imgCupom").Visible Then
            gdvReserva9.HeaderRow.FindControl("imgCupom").Focus()
        ElseIf gdvReserva9.HeaderRow.FindControl("imgCupomIndividual").Visible Then
            gdvReserva9.HeaderRow.FindControl("imgCupomIndividual").Focus()
        End If
    End Sub
    Protected Sub imgBoleto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) '100%
        Try
            'Preenchimento obrigatório para impressão dos boletos'
            If ((txtResNumero.Text = "0" Or txtResNumero.Text.Trim.Length = 0) _
                And txtResQuadra.Text.Trim.Length = 0 _
                And txtResLote.Text.Trim.Length = 0 _
                And txtResComplemento.Text.Trim.Length = 0) Then
                Mensagem("Um dos campos (Numero, Quadra, Lote ou complemento) do responsável terá que ser informado.")
                Exit Try
            End If

            If txtResBairro.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o Bairro do responsável.")
                txtResBairro.Focus()
                Exit Try
            End If
            If txtResCep.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o CEP do responsável.")
                txtResCep.Focus()
                Exit Try
            End If
            If txtResCidade.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar a Cidade do responsável.")
                txtResCidade.Focus()
                Exit Try
            End If

            If txtResCPF.Text = "00.000.000/0000-00" Or txtResCPF.Text = "000.000.000-00" Then
                Mensagem("CPF ou CNPJ inválido!")
                txtResCPF.Focus()
                Exit Try
            End If

            If CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = "" Or
            CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text.Trim.Length < 10 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor inserir a data para geração do boleto.');", True)
                gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Focus()
                Exit Try
            End If

            If DateDiff(DateInterval.Day, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text), CDate(hddResDataIni.Value)) < 5 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Geração de boleto não permitida.\n\nFalta menos de cinco dias para iniciar a reserva.');", True)
                gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Focus()
                Exit Try
            End If

            'Se for Grupo Emissivo com destino a Caldas Novas e não promovido pelo Sesc Não irá permitir emitir boleto individual
            If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" And
                btnHospedagemNova.Attributes.Item("resPasseioPromovidoCEREC") = "N" And
                btnHospedagemNova.Attributes.Item("resColoniaFeriasDes") = "S" Then

                CType(gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante"), CheckBox).Visible = False
                For Each Linha As GridViewRow In gdvReserva9.Rows
                    CType(Linha.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                Next
            End If

            'Reserva individual, imprimir somente boletos da reserva toda
            If hddResCaracteristica.Value = "I" Then
                For Each Linha As GridViewRow In gdvReserva9.Rows
                    If CType(Linha.FindControl("ckbItemIntegrante"), CheckBox).Enabled = True And CType(Linha.FindControl("ckbItemIntegrante"), CheckBox).Visible = True And CType(Linha.FindControl("ckbItemIntegrante"), CheckBox).Checked = False Then
                        Mensagem("Para reservas individuais, selecione todos os integrantes.\n\nNão será permitido a emissão de boletos individuais.")
                        Exit Try
                    End If
                Next
            End If

            Dim Integrante As String = ""
            Dim BolPrimeiraParcela = "", BolSegundaParcela = "", BolParcelaUnica = "", BolSolicitacaoRegistro = ""
            Dim Destino As String
            If gdvReserva9.Rows.Count = 1 Then
                For Each linha As GridViewRow In gdvReserva9.Rows
                    Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
                Next
            Else
                For Each linha As GridViewRow In gdvReserva9.Rows
                    If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked And
                        InStr(Integrante, CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass) = 0 Then
                        Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
                    End If
                Next
            End If

            'PREPARADO PARA BOLETOS COM REGISTRO .... A PARTIR DAQUI
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Destino = "C"
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                Destino = "P"
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If

            'Essa lista é importante pois estarei trabalhando com os dados sempre atualizados.
            lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", btnCaixa.CommandName)

            For Each LinhaAtualizada As ReservaListagemIntegranteVO In lista
                If Integrante.Contains(LinhaAtualizada.intId) Then
                    BolPrimeiraParcela += LinhaAtualizada.BolPrimeiraStatusRegistro.ToString
                    BolSegundaParcela += LinhaAtualizada.BolSegundaStatusRegistro.ToString
                    BolParcelaUnica += LinhaAtualizada.BolUnicaStatusRegistro.ToString
                    BolSolicitacaoRegistro = BolPrimeiraParcela & BolSegundaParcela & BolParcelaUnica
                End If
            Next

            objBoletoNetDAO = New BoletoNetDAO
            objBoleto1NetVO = New BoletoNetVO

            If (BolSolicitacaoRegistro.Contains("90")) Then 'Enviado para registro
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAVistaRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = False
                Mensagem("Atenção!\n\nJá existe solicitação de registro de boleto para um dos integrantes selecionados.")
                Exit Try
            ElseIf BolSolicitacaoRegistro.Contains("00") Then 'Irei apagar os boletosimp, reimpressão e integrante boletos ainda não registrados
                For Each linha As GridViewRow In gdvReserva9.Rows
                    If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked Then
                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolPrimeiraParcela").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolPrimeiraParcela").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select

                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolSegundaParcela").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolSegundaParcela").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select

                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select
                    End If
                Next
            ElseIf BolSolicitacaoRegistro.Contains("02") Then 'Registrado
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                Mensagem("Atenção!\n\nNessa reserva já existe integrante com boleto gerado impossibilitando a geração de um novo boleto.")
                Exit Try
            ElseIf BolSolicitacaoRegistro.Contains("06") Then 'Pago
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                Mensagem("Atenção!\n\nNessa reserva já existe integrante com boleto Pago impossibilitando a geração de um novo boleto.")
                Exit Try
            End If
            'Exit Try
            Dim ResCaracteristica As String = ""
            Dim Horario = "*Entrada às " & objBoleto1NetVO.Entrada & " e Saída às " & objBoleto1NetVO.Saida
            Dim Demonstrativo As String = ""
            Dim Refeicoes As String = ""
            Dim MensagemSescBoletoSacado As String = ""
            Dim MensagemSescBoletoCedente As String = ""
            Dim CodigoCedenteNovo = "", TipoDeParcelaBoleto As String = "U"

            Dim StringConexao As String = ""
            If Integrante = "" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Selecione os integrantes para gerar a cobrança.');", True)
                gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante").Focus()
            Else
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialCaldas")
                    StringConexao = "TurismoSocialCaldas"
                    Destino = "C"
                Else
                    objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialPiri")
                    StringConexao = "TurismoSocialPiri"
                    Destino = "P"
                End If

                hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))
                CodigoCedenteNovo = objBoletoNetDAO.RetornaCodCedenteNovo(Destino)

                'Definindo se será a segunda parcela do boleto ou a única
                Dim TipoParcela As String = ""
                Dim TipoParcelaDescricaoBoleto = ""
                'If (CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/boleto2parcela.png" = True  Or btnHospedagemNova.Attributes.Item("PagoPrimeira") = "S") Then
                If Mid(BolParcelaUnica, 1, 2) = "02" Or Mid(BolParcelaUnica, 1, 2) = "06" Then
                    TipoParcela = "U"
                ElseIf Mid(BolSegundaParcela, 1, 2) = "02" Or Mid(BolSegundaParcela, 1, 2) = "06" Then
                    TipoParcela = "S"
                Else
                    TipoParcela = "U"
                End If

                'If BolPrimeiraParcela = "06" Or BolPrimeiraParcela = "06" Then 'Se for registrada ou paga será a segunda
                '    TipoParcela = "S"
                '    TipoParcelaDescricaoBoleto = "2ª Parcela" 'Será usado no campo de observação do boleto
                'Else
                '    TipoParcela = "U"
                '    TipoParcelaDescricaoBoleto = "Parcela única"
                'End If

                'Os processo a seguir irão definir se serado o boleto novo ou haverá uma reimpressão.
                'Observação .. Irei atualizar somente o valor e nunca a data de vencimento dos boletos, pois esse casos serão gerados somente na central de reservas
                Dim ResultadoConsulta = objBoletoNetDAO.SobrepontoBoletoReimpressao(Destino,
                                                       hddResId.Value, TipoParcela, Integrante)

                'Definindo a data em que o cliente deverá procurar o banco para efeturar o pagamento
                Dim PagarAposDia As String = Format(DateAdd(DateInterval.Hour, 1, Date.Now), "dd/MM/yyyy HH:mm:ss")
                'If Date.Today.DayOfWeek = 6 Then 'Sabado
                '    PagarAposDia = Format(DateAdd(DateInterval.Day, 4, Date.Today), "dd/MM/yyyy")
                'ElseIf Date.Today.DayOfWeek = 0 Then 'Domingo
                '    PagarAposDia = Format(DateAdd(DateInterval.Day, 3, Date.Today), "dd/MM/yyyy")
                'ElseIf Date.Today.DayOfWeek = 1 Then 'Segunda
                '    PagarAposDia = Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy")
                'ElseIf (Date.Today.DayOfWeek = 2 Or Date.Today.DayOfWeek = 3 Or Date.Today.DayOfWeek = 4 Or Date.Today.DayOfWeek = 5) Then 'De terça em diante
                '    PagarAposDia = Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy")
                'End If

                Dim Arquivo = ""
                Dim path = ""
                Dim NossoNumero As String = Mid(ResultadoConsulta, 2, 10).ToString.Trim
                Select Case ResultadoConsulta
                    Case Is > 10
                        'Se existir o arquivo gerado irá apenas abrir.
                        Arquivo = Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero
                        'Verificando a existência do arquivo gerado.
                        If File.Exists(Server.MapPath(".") & "\BoletosTemp\" & NossoNumero & ".pdf") Then
                            path = ("BoletosTemp/" & NossoNumero & ".pdf")
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
                            Exit Try
                        ElseIf File.Exists(Server.MapPath(".") & "\BoletosTemp\" & NossoNumero & ".html") Then
                            path = ("BoletosTemp/" & NossoNumero & ".html")
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
                            Exit Try
                        Else
                            'Recriar o e-mail'
                            'Irei recriar aqui o boleto para reimpressão
                            Dim objBoleto1NetDAO = New BoletoNetDAO()
                            Dim Unidade As String = ""
                            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                                Unidade = "C"
                            Else
                                Unidade = "P"
                            End If
                            objBoleto1NetVO = objBoleto1NetDAO.PreencheDadosBoletoReimpressao(Unidade, hddResId.Value, "B", TipoParcela, NossoNumero)
                            'Se for emissivo ou passeio sempre será o Cendente de Caldas Novas

                            'Individuais
                            If hddResCaracteristica.Value = "I" Then
                                MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                    MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                    MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>As diárias do Sesc Caldas Novas incluem check-in com almoço e jantar e check-out com café da manhã.<font size=1px;font style='font-weight:normal;'>"
                                ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                    MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                    MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h e saída às 10h." & "<br/>"
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>No valor das diárias estão inclusos hospedagem com café da manhã e jantar(buffet de sopas, caldos e cremes).<font size=1px;font style='font-weight:normal;'>"
                                End If
                                MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                                MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."
                                If TipoParcela = "S" Then
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Segunda Parcela</font>"
                                Else
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Parcela única</font>"
                                End If

                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"


                                If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                                Else
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                                End If

                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                            Else
                                'Passeio
                                If txtDataInicialSolicitacao.Text = txtDataFinalSolicitacao.Text Then
                                    If ckbOrganizadoSESC.Checked Or hddResCaracteristica.Value = "I" Then
                                        MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                    Else
                                        MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " Passeio no dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                    End If
                                    'Textos para Meia diária sem pernoite
                                    If cmbDestino.SelectedIndex > 0 Then
                                        If Destino = "C" Then
                                            If cmbDestino.SelectedValue <> "0" Then 'Outro
                                                MensagemSescBoletoSacado += "<br/>Destino: Caldas Novas "
                                                MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                            Else
                                                MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                            End If
                                        ElseIf Destino = "P" Then
                                            If cmbDestino.SelectedValue <> "0" Then 'Outro
                                                MensagemSescBoletoSacado += "<br/>Destino: Sesc Pirenópolis "
                                                MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                            Else
                                                MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                            End If
                                        End If
                                    End If
                                    MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>Saída: " & txtResLocalSaida.Text & " - Chegada: " & cmbResHoraSaida.SelectedValue & " h" & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passante(s)"
                                    'Texto para o Cedente
                                    MensagemSescBoletoCedente = "Uso do Sesc: "
                                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                    If ResCaracteristica <> "I" Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                                Else
                                    'Emissivo Eduardo - Excursão
                                    ResCaracteristica = hddResCaracteristica.Value
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " - Excursão de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"

                                    If cmbDestino.SelectedIndex > 0 Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;>Destino: " & txtResNome.Text.Trim
                                        MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: Pacote"
                                    End If
                                    If hddResCaracteristica.Value <> "I" Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;>Saída(s): " & txtResLocalSaida.Text & " - Chegada: " & cmbResHoraSaida.SelectedItem.ToString
                                    End If
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passageiro(s)"

                                    MensagemSescBoletoCedente = "Uso do Sesc: "
                                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "Excursão de Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                    If ResCaracteristica <> "I" Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                                End If

                                'Texto para o Cedente
                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If

                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                            End If
                        End If
                    Case 0
                        '- Antes do registro --> Existe boletos de reimpressão já nesse valor, então não irá gerá, apenas irei solicitar ao cliente que reimprima o boleto
                        '- Agora com o Resitro --> Existe boleto gerado mais o status esta diferente de 02 que é o registro então o usuário deverá esperar o registro acontecer.

                        Mensagem("Impressão de boleto ainda não autorizada... Por favor, aguarde!")
                        Exit Try
                    Case 2
                        If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                            Dim LocalDestino As String = ""
                            If cmbDestino.SelectedItem.Text = "0" Then
                                LocalDestino = cmbDestinoCidade.SelectedItem.Text & " " & cmbDestinoEstado.SelectedItem.Text
                            Else
                                LocalDestino = cmbDestino.SelectedItem.Text
                            End If
                            ResCaracteristica = hddResCaracteristica.Value
                            objBoleto1NetVO = objBoletoNetDAO.GeraBoletoEmissivo(hddResId.Value, "B", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString, Destino)


                            'Sobrepondo as informações de bolImp após a geração'
                            ResultadoConsulta = objBoletoNetDAO.SobrepontoBoletoReimpressao(Destino,
                                                       hddResId.Value, TipoParcela, Integrante)

                            'Se for emissivo ou passeio sempre será o Cendente de Caldas Novas
                            If txtDataInicialSolicitacao.Text = txtDataFinalSolicitacao.Text Then
                                If ckbOrganizadoSESC.Checked Then
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                Else
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " Passeio no dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                End If
                                'Textos para Meia diária sem pernoite
                                If cmbDestino.SelectedIndex > 0 Then
                                    'MensagemSescBoletoSacado += "<br/>Destino: " & Mid(cmbDestino.SelectedItem.ToString.Trim, 1, cmbDestino.SelectedItem.ToString.IndexOf("-")).Replace("-", "")
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & Mid(cmbDestino.SelectedItem.ToString.Trim, cmbDestino.SelectedItem.ToString.IndexOf("-") + 1, Len(cmbDestino.SelectedItem.ToString) - cmbDestino.SelectedItem.ToString.IndexOf("-") + 1)
                                    If Destino = "C" Then
                                        If cmbDestino.SelectedValue <> "0" Then 'Outro
                                            MensagemSescBoletoSacado += "<br/>Destino: Caldas Novas "
                                            MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                        Else
                                            MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                        End If
                                    ElseIf Destino = "P" Then
                                        If cmbDestino.SelectedValue <> "0" Then 'Outro
                                            MensagemSescBoletoSacado += "<br/>Destino: Sesc Pirenópolis "
                                            MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                        Else
                                            MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                        End If
                                    End If
                                End If

                                MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>Saída: " & txtResLocalSaida.Text & " - Chegada: " & cmbResHoraSaida.SelectedValue & " h" & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passante(s)"

                                'Texto para o Cedente
                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If

                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                            Else
                                'Emissivo Eduardo - Excursão
                                ResCaracteristica = hddResCaracteristica.Value
                                MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " - Excursão de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                'MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                'MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                If cmbDestino.SelectedIndex > 0 Then
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>Destino: " & objBoleto1NetVO.Demonstrativo.Trim.Replace("-", "")
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Destino: " & txtResNome.Text.Trim
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: Pacote"
                                End If
                                MensagemSescBoletoSacado += "<br/><font size=2px;>Saída(s): " & txtResLocalSaida.Text & " - Chegada: " & cmbResHoraSaida.SelectedItem.ToString
                                MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passageiro(s)"

                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "Excursão de Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If
                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                            End If
                        Else

                            'Gerando boleto Parcelado
                            If DateDiff(DateInterval.Day, CDate(Format(Date.Now, "dd/MM/yyyy")),
                        CDate(Format(CDate(hddResDataIni.Value), "dd/MM/yyyy"))) > 30 Then
                                If sender.CommandArgument.ToString = "50" Then
                                    objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante,
                                        Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"),
                                        User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "50", Destino)
                                    'Dim varVlrDoc As String = objBoleto1NetVO.VlrDoc.ToString.Replace(",", "").Replace(".", "")
                                    ResCaracteristica = hddResCaracteristica.Value
                                    'Informando que se trata de um boleto parcelado
                                    TipoDeParcelaBoleto = "P"
                                    'Essa parte deixarei desabilitada, que o segundo boleto do parcelado.
                                    'Dim objBoleto2NetVO As New BoletoNetVO
                                    'objBoleto2NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante, _
                                    '    Format(DateAdd(DateInterval.Day, -30, CDate(hddResDataIni.Value)), "dd/MM/yyyy"), _
                                    '    User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "50", Destino)
                                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/BoletoParcelado" & _

                                    'Mensagem do Boleto
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>As diárias do Sesc Caldas Novas incluem check-in com almoço e jantar e check-out com café da manhã.<font size=1px;font style='font-weight:normal;'>"
                                    ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s)-Entrada às 14h e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>No valor das diárias estão inclusos hospedagem com café da manhã e jantar(buffet de sopas, caldos e cremes).<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                                    MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>2ª Parcela</font>"

                                    MensagemSescBoletoCedente = "Uso do Sesc: "
                                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"

                                    If ResCaracteristica <> "I" Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                                    Else
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                                    End If
                                    'MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"

                                    'Gerando tão somente a segunda parcela de uma reserva
                                ElseIf TipoParcela = "S" Then
                                    objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante,
                                        Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"),
                                        User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "100", Destino)
                                    'Dim varVlrDoc As String = objBoleto1NetVO.VlrDoc.ToString.Replace(",", "").Replace(".", "")
                                    ResCaracteristica = hddResCaracteristica.Value
                                    'Informando que se trata de um boleto parcelado
                                    TipoDeParcelaBoleto = "S"
                                    'Essa parte deixarei desabilitada, que o segundo boleto do parcelado.
                                    'Dim objBoleto2NetVO As New BoletoNetVO
                                    'objBoleto2NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante, _
                                    '    Format(DateAdd(DateInterval.Day, -30, CDate(hddResDataIni.Value)), "dd/MM/yyyy"), _
                                    '    User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "50", Destino)
                                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/BoletoParcelado" & _

                                    'Mensagem do Boleto
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>As diárias do Sesc Caldas Novas incluem check-in com almoço e jantar e check-out com café da manhã.<font size=1px;font style='font-weight:normal;'>"
                                    ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s)-Entrada às 14h e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>No valor das diárias estão inclusos hospedagem com café da manhã e jantar(buffet de sopas, caldos e cremes).<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                                    MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>2ª Parcela</font>"

                                    MensagemSescBoletoCedente = "Uso do Sesc: "
                                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"

                                    If ResCaracteristica <> "I" Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                                    Else
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                                    End If
                                    'MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                                Else
                                    ResCaracteristica = hddResCaracteristica.Value
                                    objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"),
                                       User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "100", Destino)
                                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/BoletoCobranca" & _
                                    'Mensagem do Boleto
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>As diárias do Sesc Caldas Novas incluem check-in com almoço e jantar e check-out com café da manhã.<font size=1px;font style='font-weight:normal;'>"
                                    ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                        MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                        MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h e saída às 10h." & "<br/>"
                                        'MensagemSescBoletoSacado += "<br/><font size=2px;>No valor das diárias estão inclusos hospedagem com café da manhã e jantar(buffet de sopas, caldos e cremes).<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                                    MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Parcela única</font>"

                                    MensagemSescBoletoCedente = "Uso do Sesc: "
                                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"

                                    If ResCaracteristica <> "I" Then
                                        MensagemSescBoletoCedente += "<br/> " & ResCaracteristica.ToString & "<font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                    End If
                                    If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                                    Else
                                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                                    End If
                                    'MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                                End If
                            Else
                                ResCaracteristica = hddResCaracteristica.Value

                                If Format(CDate(txtResDtLimiteRetorno.Text), "yyyy-MM-dd") < Format(Date.Now, "yyyy-MM-dd") Then
                                    Mensagem("Atenção, o processo será abortado!\n\nMotivo: Altere a data ""Bloquear até"" para no mínimo a data de hoje.")
                                    Exit Try
                                End If

                                If Format(DateAdd(DateInterval.Day, 7, CDate(txtResDtLimiteRetorno.Text)), "yyyy-MM-dd") >= Format(CDate(txtDataInicialSolicitacao.Text), "yyyy-MM-dd") Then
                                    Mensagem("Atenção! O boleto não poderá ser gerado.\n\nMotivo: Vencimento não poderá ser igual ou superior ao início da reserva.")
                                    Exit Try
                                End If

                                objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), "100", Destino)
                                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/BoletoCobranca" & _

                                'Mensagem do Boleto
                                MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                    MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                    MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>As diárias do Sesc Caldas Novas incluem check-in com almoço e jantar e check-out com café da manhã.<font size=1px;font style='font-weight:normal;'>"
                                ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                    MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                    MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h e saída às 10h." & "<br/>"
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>No valor das diárias estão inclusos hospedagem com café da manhã e jantar(buffet de sopas, caldos e cremes).<font size=1px;font style='font-weight:normal;'>"
                                End If
                                MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                                MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."
                                'MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"

                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"

                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If
                                If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                                Else
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                                End If
                                'MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"
                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após: " & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"

                            End If
                        End If
                End Select
                'End If
                'Se for federação irá preencher o endereço com os dados do Sesc
                If cmbOrgId.SelectedValue = "37" Then
                    ResCaracteristica = "F"
                End If

                'CodigoCedenteNovo = "0012535475"
                NossoNumero = Mid(objBoleto1NetVO.NossoNum, 1, 10)
                Dim NossoNumeroCompleto = objBoleto1NetVO.NossoNum

                Dim LinhaDigitavel = ""
                'AQUI NESSE LUGAR VOU PASSAR UM ObjBoletoVo com os dados de endereço completo do bolimpid
                Dim ObjBoletosUteis As New Uteis.BoletoVO
                With ObjBoletosUteis
                    .ResNome = objBoleto1NetVO.ResNome
                    .ResCPFCGC = objBoleto1NetVO.ResCPFCGC
                    .ResLogradouro = objBoleto1NetVO.ResLogradouro
                    .ResQuadra = objBoleto1NetVO.ResQuadra
                    .ResLote = objBoleto1NetVO.ResLote
                    .ResNumero = objBoleto1NetVO.ResNumero
                    .ResCidade = objBoleto1NetVO.ResCidade
                    .ResBairro = objBoleto1NetVO.ResBairro
                    .ResCep = objBoleto1NetVO.ResCep
                    .UF = objBoleto1NetVO.UF
                End With

                'Gerando o arquivo do boleto1 - 

                Dim s = Uteis.GeraBoletoComRegistro.GeraBoletoHtmlPdf(ObjBoletosUteis, objBoleto1NetVO.Cedente.Trim, Mid(objBoleto1NetVO.CodBanco, 1, 3), "SESC ADM REGIONAL GO", objBoleto1NetVO.Vencimento, Mid(CodigoCedenteNovo, 1, 4), Mid(CodigoCedenteNovo, 5, 6), "4", NossoNumero, objBoleto1NetVO.VlrDoc,
                           objBoleto1NetVO.Banco, objBoleto1NetVO.DataDocumento, objBoleto1NetVO.DataProces, MensagemSescBoletoSacado, MensagemSescBoletoCedente, Destino, TipoDeParcelaBoleto, hddResId.Value, 0, StringConexao, hddResId.Value)


                'PRIMEIRO Boleto - No Final do Boleto original havia Espaço de 06 linhas inseridas, a rotina abaixo Retira a inserção de 4 linhas.
                Dim TInicio = Mid(s, 1, s.Length - 170)
                Dim TFim = Mid(s, s.Length - 29, 29)
                Dim BoletoPronto = TInicio & TFim
                'Devolvendo o boleto já sem as linhas no final
                s = BoletoPronto

                'Separando a linha digitavel
                LinhaDigitavel = Mid(s, 1, InStr(s, "§") - 1)
                'Retorna somente o código HTML  
                s = Mid(s, InStr(s, "§") + 1, s.Length)

                Arquivo = Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero

                Dim pp = New System.Diagnostics.Process()
                pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
                pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                File.Create(Arquivo & ".html").Dispose()
                File.AppendAllText(Arquivo & ".html", s, Encoding.UTF8)

                pp.StartInfo.Arguments = "--zoom 2 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"
                pp.Start()
                pp.WaitForExit()
                pp.Close()
                pp.Dispose()
                'Teste na máquina local: Comentar o Delete e mudar no Path como .pdf
                File.Delete(Server.MapPath(".") & "..\BoletosTemp\ " & Arquivo & ".html")

                path = ("BoletosTemp/" & NossoNumero & ".pdf")

                ListaFinanceiroViaResId()

                'Atualiza as informações
                ListaIntegranteViaResId()

            End If
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Sub

    Protected Sub imgCupom_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Obriga digital a data para geração do boleto
        If CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = "" Or
            CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text.Trim.Length < 10 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor inserir a data para geração do boleto.');", True)
            gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Focus()
            Return
        End If
        Dim Integrante As String = ""
        Dim Destino As String

        If gdvReserva9.Rows.Count = 1 Then
            For Each linha As GridViewRow In gdvReserva9.Rows
                Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
            Next
        Else
            For Each linha As GridViewRow In gdvReserva9.Rows
                If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked And
                    InStr(Integrante, CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass) = 0 Then
                    Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
                End If
            Next
        End If

        If Integrante = "" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Selecione os integrantes para gerar a cobrança.');", True)
            gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante").Focus()
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialCaldas")
                Destino = "C"
            Else
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialPiri")
                Destino = "P"
            End If
            'objBoletoVO = New BoletoVO

            hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))

            If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                Dim LocalDestino As String = ""
                If cmbDestino.SelectedItem.Text = "0" Then
                    LocalDestino = cmbDestinoCidade.SelectedItem.Text & " " & cmbDestinoEstado.SelectedItem.Text
                Else
                    LocalDestino = cmbDestino.SelectedItem.Text
                End If
                Dim path = ("Cupons/" & "CupomPasseio" & Destino)

                objBoletoVO = objBoletoDAO.GeraBoletoEmissivo(hddResId.Value, "C", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)
                If txtDataInicialSolicitacao.Text = txtDataFinalSolicitacao.Text Then
                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/CupomPasseio" & Destino & ".asp?Banco=" & objBoletoVO.Banco & _
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & path & ".asp?Banco=" & objBoletoVO.Banco &
                    "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" &
                    "&LocalSaida=" & Mid(txtResLocalSaida.Text.Trim, 1, 200) &
                    "&HoraSaida=" & cmbResHoraSaida.Text &
                    "&NomeResp=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&Destino=" & LocalDestino &
                    "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
                Else
                    path = ("Cupons/" & "CupomExcursao" & Destino)
                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/CupomExcursao" & Destino & ".asp?Banco=" & objBoletoVO.Banco & _
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & path & ".asp?Banco=" & objBoletoVO.Banco &
                    "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" &
                    "&LocalSaida=" & Mid(txtResLocalSaida.Text.Trim, 1, 200) &
                    "&HoraSaida=" & cmbResHoraSaida.Text &
                    "&NomeResp=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&Destino=" & LocalDestino &
                    "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
                End If
            Else
                If DateDiff(DateInterval.Day, CDate(Format(Date.Now, "dd/MM/yyyy")),
                    CDate(Format(CDate(hddResDataIni.Value), "dd/MM/yyyy"))) > 30 Then
                    objBoletoVO = objBoletoDAO.GeraBoleto(hddResId.Value, "C", Integrante,
                    Format(DateAdd(DateInterval.Day, -30, CDate(hddResDataIni.Value)), "dd/MM/yyyy"),
                    User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)
                Else
                    objBoletoVO = objBoletoDAO.GeraBoleto(hddResId.Value, "C", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)
                End If
                Dim Path = ("Cupons/" & "CupomCobranca" & Destino)
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/CupomCobranca" & Destino & ".asp?Banco=" & objBoletoVO.Banco & _
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & Path & ".asp?Banco=" & objBoletoVO.Banco &
                "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & objBoletoVO.Sacado &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" & "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
            End If
            ListaFinanceiroViaResId()
        End If
    End Sub
    Protected Sub imgBoletoIndividual_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            For Each linha As GridViewRow In gdvReserva9.Rows
                'Verifica se possui mais de um integrante selecionado, individual é para apenas uma pessoa.
                If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked = True Then
                    Dim CPFCNPJ As String
                    If IsNothing(gdvReserva9.DataKeys(linha.RowIndex()).Item("IntCPF").ToString) Then
                        CPFCNPJ = ""
                    Else
                        CPFCNPJ = gdvReserva9.DataKeys(linha.RowIndex()).Item("IntCPF").ToString.Replace(" ", "").Replace("-", " ").Replace("/", "").Replace(".", "")
                    End If
                    btnCaixa.Attributes.Add("IntCPF", CPFCNPJ)
                    btnCaixa.Attributes.Add("Sacado", CType(linha.FindControl("lnkIntNome"), LinkButton).Text.ToString)
                    cont += 1
                    ''Checa se existe mais de um integrante selecionado.
                    'If cont > 1 Then
                    '    Mensagem("Você possui mais de um integrante selecionado.\n\nProcesso abortado!")
                    '    Exit Try
                    'End If
                End If
            Next

            hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))

            'A data de vencimento nunca poderá ser inferior a Hoje + 6 dias, que é a data de vencimento do boleto
            If (CDate(Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "yyyy-MM-dd")) < CDate(Format(DateAdd(DateInterval.Day, 1, Date.Now), "yyyy-MM-dd"))) Then
                Mensagem("A data de vencimento não poderá ser inferior ao dia " & CDate(Format(DateAdd(DateInterval.Day, 1, Date.Now), "dd/MM/yyyy")))
                Exit Try
            End If

            'Preenchimento obrigatório para impressão dos boletos'
            If ((txtResNumero.Text = "0" Or txtResNumero.Text.Trim.Length = 0) _
                And txtResQuadra.Text.Trim.Length = 0 _
                And txtResLote.Text.Trim.Length = 0 _
                And txtResComplemento.Text.Trim.Length = 0) Then
                Mensagem("Um dos campos (Numero, Quadra, Lote ou complemento) do responsável terá que ser informado.")
                Exit Try
            End If

            If txtResBairro.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o Bairro do responsável.")
                txtResBairro.Focus()
                Exit Try
            End If
            If txtResCep.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o CEP do responsável.")
                txtResCep.Focus()
                Exit Try
            End If
            If txtResCidade.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar a Cidade do responsável.")
                txtResCidade.Focus()
                Exit Try
            End If
            'Obriga digital a data para geração do boleto
            If Not CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip.Contains("Boleto Registrado") Then
                If CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = "" Or
                    CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text.Trim.Length < 10 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor inserir a data para geração do boleto.');", True)
                    gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Focus()
                    Return
                End If
            Else
                CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = Format(Date.Now, "dd/MM/yyyy")
            End If

            Dim BolPrimeiraParcela = "", BolSegundaParcela = "", BolParcelaUnica = "", BolSolicitacaoRegistro = ""
            Dim Integrante As String = ""
            Dim Destino As String
            For Each linha As GridViewRow In gdvReserva9.Rows
                If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked And
                    InStr(Integrante, CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass) = 0 Then
                    Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
                End If
            Next

            'PREPARADO PARA BOLETOS COM REGISTRO .... A PARTIR DAQUI
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Destino = "C"
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                Destino = "P"
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If

            'Essa lista é importante pois estarei trabalhando com os dados sempre atualizados.
            lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", btnCaixa.CommandName)

            For Each LinhaAtualizada As ReservaListagemIntegranteVO In lista
                If Integrante.Contains(LinhaAtualizada.intId) Then
                    BolPrimeiraParcela += LinhaAtualizada.BolPrimeiraStatusRegistro.ToString
                    BolSegundaParcela += LinhaAtualizada.BolSegundaStatusRegistro.ToString
                    BolParcelaUnica += LinhaAtualizada.BolUnicaStatusRegistro.ToString
                    BolSolicitacaoRegistro = BolPrimeiraParcela & BolSegundaParcela & BolParcelaUnica
                End If
            Next

            objBoletoNetDAO = New BoletoNetDAO
            objBoleto1NetVO = New BoletoNetVO

            If (BolSolicitacaoRegistro.Contains("90")) Then 'Enviado para registro
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAVistaRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = False
                Mensagem("Atenção!\n\nJá existe solicitação de registro de boleto para um dos integrantes selecionados.")
                'Exit Try
            ElseIf BolSolicitacaoRegistro.Contains("00") Then 'Irei apagar os boletosimp, reimpressão e integrante boletos ainda não registrados
                For Each linha As GridViewRow In gdvReserva9.Rows
                    If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked Then
                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolPrimeiraParcela").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolPrimeiraParcela").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select

                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolSegundaParcela").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolSegundaParcela").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select

                        Select Case gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim.Length
                            Case Is > 8
                                Select Case objBoletoNetDAO.ApagaBoletosSemRegistros(gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim, Destino)
                                    Case 0
                                        Mensagem("Atenção!\n\nHouve falha no processo de exclusão dos boletos já gerados e não enviados para registro.\n\nBoleto: " & gdvReserva9.DataKeys(linha.RowIndex).Item("BolParcelaUnica").ToString.Trim)
                                        Exit Try
                                End Select
                        End Select
                    End If
                Next
            ElseIf BolSolicitacaoRegistro.Contains("02") Then 'Registrado
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
            End If
            'Exit Try
            Dim ResCaracteristica As String = ""
            Dim Horario = "*Entrada às " & objBoleto1NetVO.Entrada & " e Saída às " & objBoleto1NetVO.Saida
            Dim Demonstrativo As String = ""
            Dim Refeicoes As String = ""
            Dim MensagemSescBoletoSacado As String = ""
            Dim MensagemSescBoletoCedente As String = ""
            Dim CodigoCedenteNovo = "", TipoDeParcelaBoleto As String = "U" 'Individual

            Dim Arquivo = ""
            Dim path = ""
            Dim NossoNumero As String = ""
            Dim StringConexao As String = ""
            If Integrante = "" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Selecione os integrantes para gerar a cobrança.');", True)
                gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante").Focus()
            Else
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialCaldas")
                    StringConexao = "TurismoSocialCaldas"
                    Destino = "C"
                Else
                    objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialPiri")
                    StringConexao = "TurismoSocialPiri"
                    Destino = "P"
                End If

                'hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))
                CodigoCedenteNovo = objBoletoNetDAO.RetornaCodCedenteNovo(Destino)

                'Definindo se será a segunda parcela ou a única
                Dim TipoParcelaDescricaoBoleto = ""
                Dim TipoParcela As String = "U" 'Boleto individual será sempre parcela única
                TipoParcelaDescricaoBoleto = "Parcela Individual"
                'If CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/boleto2parcela.png" = True Then
                '    TipoParcela = "S"
                '    TipoParcelaDescricaoBoleto = "2ª Parcela" 'Será usado no campo de observação do boleto
                'Else
                '    TipoParcela = "U" 'Indiviual
                '    TipoParcelaDescricaoBoleto = "Parcela Individual"
                'End If

                'Os processo a seguir irão definir se sera gerado o boleto novo ou haverá uma reimpressão.
                'Observação .. Irei atualizar somente o valor e nunca a data de vencimento dos boletos, pois esse casos serão gerados somente na central de reservas
                Dim ResultadoConsulta = objBoletoNetDAO.SobrepontoBoletoReimpressao(Destino,
                                                       hddResId.Value, TipoParcela, Integrante)

                NossoNumero = Mid(ResultadoConsulta, 2, 10).ToString.Trim
                Select Case ResultadoConsulta
                    Case Is > 10
                        'Se existir o arquivo gerado irá apenas abrir, testa qualque um que existir ele abre
                        Arquivo = Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero
                        'Verificando a existência do arquivo gerado.
                        If File.Exists(Server.MapPath(".") & "\BoletosTemp\" & NossoNumero & ".html") Then
                            path = ("BoletosTemp/" & NossoNumero & ".html")
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
                            Exit Try
                        ElseIf File.Exists(Server.MapPath(".") & "\BoletosTemp\" & NossoNumero & ".pdf") Then
                            path = ("BoletosTemp/" & NossoNumero & ".pdf")
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
                            Exit Try
                        Else
                            Mensagem("O boleto registrado não foi encontrado.")
                            Exit Try
                        End If
                    Case 0
                        '- Antes do registro --> Existe boletos de reimpressão já nesse valor, então não irá gerá, apenas irei solicitar ao cliente que reimprima o boleto
                        '- Agora com o Resitro --> Existe boleto gerado mais o status esta diferente de 02 que é o registro então o usuário deverá esperar o registro acontecer.
                        'Me.imgReimpressao_Click(CType(row.FindControl("imgReimpressao"), ImageButton), e)
                        'Mensagem("Escolha o boleto que você deseja reimprimir.")
                        Mensagem("Impressão de boleto ainda não autorizada... Por favor, aguarde!")
                        Exit Try
                    Case 2
                        If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                            Mensagem("Opção indisponível no momento. Informe o centro de informática.")
                            Exit Try 'Não permitir gerar por enquanto ... deixarei gerar por enquanto apenas grupo

                            Dim LocalDestino As String = ""
                            If cmbDestino.SelectedItem.Text = "0" Then
                                LocalDestino = cmbDestinoCidade.SelectedItem.Text & " " & cmbDestinoEstado.SelectedItem.Text
                            Else
                                LocalDestino = cmbDestino.SelectedItem.Text
                            End If
                            objBoleto1NetVO = objBoletoNetDAO.GeraBoletoEmissivo(hddResId.Value, "B", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString, Destino)
                            ResCaracteristica = hddResCaracteristica.Value
                            'Se for emissivo ou passeio sempre será o Cendente de Caldas Novas
                            If txtDataInicialSolicitacao.Text = txtDataFinalSolicitacao.Text Then
                                If ckbOrganizadoSESC.Checked Then
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                Else
                                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " Passeio no dia <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                                End If

                                'Sacado para meia diária sem pernoite
                                If cmbDestino.SelectedIndex > 0 Then
                                    'MensagemSescBoletoSacado += "<br/>Destino: " & Mid(cmbDestino.SelectedItem.ToString.Trim, 1, cmbDestino.SelectedItem.ToString.IndexOf("-")).Replace("-", "")
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & Mid(cmbDestino.SelectedItem.ToString.Trim, cmbDestino.SelectedItem.ToString.IndexOf("-") + 1, Len(cmbDestino.SelectedItem.ToString) - cmbDestino.SelectedItem.ToString.IndexOf("-") + 1)
                                    If Destino = "C" Then
                                        If cmbDestino.SelectedValue <> "0" Then 'Outro
                                            MensagemSescBoletoSacado += "<br/>Destino: Caldas Novas "
                                            MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                        Else
                                            MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                        End If
                                    ElseIf Destino = "P" Then
                                        If cmbDestino.SelectedValue <> "0" Then 'Outro
                                            MensagemSescBoletoSacado += "<br/>Destino: Sesc Pirenópolis "
                                            MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: " & cmbDestino.SelectedItem.ToString.Trim
                                        Else
                                            MensagemSescBoletoSacado += "<br/>Destino: " & txtResNome.Text.Trim
                                        End If
                                    End If
                                End If

                                MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>Saída: " & txtResLocalSaida.Text & " - Chegada: " & cmbResHoraSaida.SelectedValue & " h" & "<font size=1px;font style='font-weight:normal;'>"
                                MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passante(s)"

                                'Texto para o Cedente
                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"


                            Else
                                'Emissivo Eduardo - Excursão
                                ResCaracteristica = hddResCaracteristica.Value
                                MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " - Excursão de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"

                                If cmbDestino.SelectedIndex > 0 Then
                                    'MensagemSescBoletoSacado += "<br/><font size=2px;>Destino: " & objBoleto1NetVO.Demonstrativo.Trim.Replace("-", "")
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Destino: " & txtResNome.Text.Trim
                                    MensagemSescBoletoSacado += "<br/><font size=2px;>Serviços: Pacote"
                                End If
                                MensagemSescBoletoSacado += "<br/><font size=2px;>Saída(s): " & txtResLocalSaida.Text & " - Chegada:" & cmbResHoraSaida.SelectedItem.ToString
                                MensagemSescBoletoSacado += "<br/><font size=2px;>" & objBoleto1NetVO.QtdePess & " Passageiro(s)"


                                MensagemSescBoletoCedente = "Uso do Sesc: "
                                MensagemSescBoletoCedente += "<br/><font size=2px;>" & "Excursão de Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                                If ResCaracteristica <> "I" Then
                                    MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                                End If
                                MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"

                            End If
                        Else
                            If DateDiff(DateInterval.Day, CDate(Format(Date.Now, "yyyy-MM-dd")),
                            CDate(Format(CDate(hddResDataIni.Value), "yyyy-MM-dd"))) > 30 Then
                                'objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "B", Integrante, _
                                objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "I", Integrante,
                                Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"),
                                User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString, Destino)
                                'Format(DateAdd(DateInterval.Day, -30, CDate(hddResDataIni.Value)), "dd/MM/yyyy"), _
                            Else
                                objBoleto1NetVO = objBoletoNetDAO.GeraBoleto(hddResId.Value, "I", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString, Destino)
                            End If
                            'irá verificar se o boleto foi mesmo gerado e caso tenha tido algum tipo na geração, não irá permitir criar e exibir o boleto
                            'Se houve erro na geração, então irá setar o nosso número como zero
                            Dim Erro = 0
                            Try
                                If IsNothing(objBoleto1NetVO.NossoNum) Then
                                    objBoleto1NetVO.NossoNum = 0
                                End If
                                If objBoletoNetDAO.ConfirmaGeracaoBoletoIndividual(Integrante, Mid(objBoleto1NetVO.NossoNum, 1, 10), Destino, hddResId.Value) = 0 Then
                                    Mensagem("O Boleto não pode ser gerado, verifique a data de vencimento e tente novamente.")
                                    Exit Try
                                End If
                            Catch ex As Exception
                                Erro = 1
                                Mensagem("O Boleto não pode ser gerado, verifique a data de vencimento e tente novamente.")
                                Exit Try
                            End Try
                            If Erro = 1 Then
                                Exit Try
                            End If

                            ResCaracteristica = hddResCaracteristica.Value
                            'Mensagem do Boleto
                            MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"
                            If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS-</font><font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s) - Entrada às 14h (com almoço) e saída às 10h." & "<br/>"

                            ElseIf objBoleto1NetVO.Demonstrativo.Contains("Pirenópolis") Then
                                MensagemSescBoletoSacado += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS<font size=1px;>" & objBoleto1NetVO.Acomodacao.Replace("<BR>", " ")
                                MensagemSescBoletoSacado += "<br/>" & objBoleto1NetVO.QtdePess & " Hóspede(s)-Entrada às 14h e saída às 10h." & "<br/>"

                            End If
                            MensagemSescBoletoSacado += "Obrigatória a apresentação deste boleto e das carteiras do Sesc atualizadas, na chegada." & "<br/>"
                            MensagemSescBoletoSacado += "A constatação de discrepância gerará alteração do valor e cobrança da diferença."

                            MensagemSescBoletoCedente = "Uso do Sesc: "
                            MensagemSescBoletoCedente += "<br/><font size=2px;>" & "RESERVA Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"

                            If ResCaracteristica <> "I" Then
                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>Excursão-" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                            End If
                            If objBoleto1NetVO.Demonstrativo.Contains("Caldas Novas") Then
                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>SESC CALDAS NOVAS</font>"
                            Else
                                MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>POUSADA SESC PIRENÓPOLIS</font>"
                            End If
                            'MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>1º Parcela</font>"
                            MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"

                        End If
                End Select
            End If

            'Se for federação irá preencher o endereço com os dados do Sesc
            If cmbOrgId.SelectedValue = "37" Then
                ResCaracteristica = "F"
            End If

            'CodigoCedenteNovo = "0012535475"
            NossoNumero = Mid(objBoleto1NetVO.NossoNum, 1, 10)
            Dim NossoNumeroCompleto = objBoleto1NetVO.NossoNum
            'Usado para na hora de gerar o boleto, informar que o click aconteceu no botão de boleto individual
            btnCaixa.Attributes.Add("BoletoIndividual", "BoletoIndividual")
            'Gerando o arquivo do boleto1 - 

            'AQUI NESSE LUGAR VOU PASSAR UM ObjBoletoVo com os dados de endereço completo do bolimpid
            Dim LinhaDigitavel = ""
            Dim ObjBoletosUteis As New Uteis.BoletoVO
            With ObjBoletosUteis
                .ResNome = objBoleto1NetVO.ResNome
                .ResCPFCGC = objBoleto1NetVO.ResCPFCGC
                .ResLogradouro = objBoleto1NetVO.ResLogradouro
                .ResQuadra = objBoleto1NetVO.ResQuadra
                .ResLote = objBoleto1NetVO.ResLote
                .ResNumero = objBoleto1NetVO.ResNumero
                .ResCidade = objBoleto1NetVO.ResCidade
                .ResBairro = objBoleto1NetVO.ResBairro
                .ResCep = objBoleto1NetVO.ResCep
                .UF = objBoleto1NetVO.UF
            End With

            Dim s = Uteis.GeraBoletoComRegistro.GeraBoletoHtmlPdf(ObjBoletosUteis, objBoleto1NetVO.Cedente.Trim, Mid(objBoleto1NetVO.CodBanco, 1, 3), "SESC ADM REGIONAL GO", objBoleto1NetVO.Vencimento, Mid(CodigoCedenteNovo, 1, 4), Mid(CodigoCedenteNovo, 5, 6), "4", NossoNumero, objBoleto1NetVO.VlrDoc,
           objBoleto1NetVO.Banco, objBoleto1NetVO.DataDocumento, objBoleto1NetVO.DataProces, MensagemSescBoletoSacado, MensagemSescBoletoCedente, Destino, TipoDeParcelaBoleto, hddResId.Value, 0, StringConexao, hddResId.Value)


            'Gerando o Boleto2
            'Dim s2 = GeraBoletoNet(objBoleto1NetVO.Cedente.Trim, Mid(objBoleto1NetVO.CodBanco, 1, 3), "SESC ADM REGIONAL GO-" & objBoleto1NetVO.Cedente.Trim & ",Rua19 Nº260 S.Central-Goiânia-GO-CEP:74030-090", objBoleto2NetVO.Vencimento, Mid(CodigoCedenteNovo, 1, 4), Mid(CodigoCedenteNovo, 5, 6), "4", NossoNumero2, objBoleto2NetVO.VlrDoc, _
            '              objBoleto2NetVO.Banco, objBoleto1NetVO.DataDocumento, objBoleto1NetVO.DataProces, MensagemSescBoletoSacado & "<p><font size=2px;font style='font-weight:bold;'></font><p/>", MensagemSescBoletoCedente, Destino)

            Session.Remove("LinhaDigitavel")

            'PRIMEIRO Boleto - No Final do Boleto original havia Espaço de 06 linhas inseridas, a rotina abaixo Retira a inserção de 4 linhas.
            Dim TInicio = Mid(s, 1, s.Length - 170)
            Dim TFim = Mid(s, s.Length - 29, 29)
            Dim BoletoPronto = TInicio & TFim
            'Devolvendo o boleto já sem as linhas no final
            s = BoletoPronto

            'Separando a linha digitavel
            LinhaDigitavel = Mid(s, 1, InStr(s, "§") - 1)
            'Retorna somente o código HTML  
            s = Mid(s, InStr(s, "§") + 1, s.Length)


            'Juntando os dois boletos para impressão com quebra de página no meio
            s = s & "<p style=" & "page-break-before:always" & "></p>" '& s2

            Arquivo = Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero

            Dim pp = New System.Diagnostics.Process()
            pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
            pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            File.Create(Arquivo & ".html").Dispose()
            File.AppendAllText(Arquivo & ".html", s, Encoding.UTF8)

            pp.StartInfo.Arguments = "--zoom 2 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"
            pp.Start()
            pp.WaitForExit()
            pp.Close()
            pp.Dispose()
            'Teste na máquina local: Comentar o Delete e mudar no Path o .pdf para .html
            File.Delete(Arquivo & ".html")
            'path = ("BoletosTemp/" & NossoNumero & ".html")
            path = ("BoletosTemp/" & NossoNumero & ".pdf")
            ListaFinanceiroViaResId()
            ListaIntegranteViaResId()
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))

        End Try
    End Sub

    Protected Sub imgCupomIndividual_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Obriga digital a data para geração do boleto
        If CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = "" Or
            CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text.Trim.Length < 10 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor inserir a data para geração do boleto.');", True)
            gdvReserva9.HeaderRow.FindControl("txtDiasPrazo").Focus()
            Return
        End If
        Dim Integrante As String = ""
        Dim Destino As String
        For Each linha As GridViewRow In gdvReserva9.Rows
            If CType(linha.FindControl("ckbItemIntegrante"), CheckBox).Checked And
                InStr(Integrante, CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass) = 0 Then
                Integrante += CType(linha.FindControl("ckbItemIntegrante"), CheckBox).CssClass & "."
            End If
        Next

        If Integrante = "" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Selecione os integrantes para gerar a cobrança.');", True)
            gdvReserva9.HeaderRow.FindControl("ckbHeadIntegrante").Focus()
            Return
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialCaldas")
                Destino = "C"
            Else
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialPiri")
                Destino = "P"
            End If
            'objBoletoVO = New BoletoVO

            hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))
            Dim path = ("Cupons/" & "CupomPasseio" & Destino)

            If InStr("IET", hddResCaracteristica.Value) = 0 Then 'Emissivo
                Dim LocalDestino As String = ""
                If cmbDestino.SelectedItem.Text = "0" Then
                    LocalDestino = cmbDestinoCidade.SelectedItem.Text & " " & cmbDestinoEstado.SelectedItem.Text
                Else
                    LocalDestino = cmbDestino.SelectedItem.Text
                End If

                objBoletoVO = objBoletoDAO.GeraBoletoEmissivo(hddResId.Value, "C", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)

                If txtDataInicialSolicitacao.Text = txtDataFinalSolicitacao.Text Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & path & ".asp?Banco=" & objBoletoVO.Banco &
                    "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & objBoletoVO.Sacado &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" &
                    "&LocalSaida=" & Mid(txtResLocalSaida.Text.Trim, 1, 200) &
                    "&HoraSaida=" & cmbResHoraSaida.Text &
                    "&NomeResp=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&Destino=" & LocalDestino &
                    "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
                Else
                    path = ("Cupons/" & "CupomExcursao" & Destino)
                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/CupomExcursao" & Destino & ".asp?Banco=" & objBoletoVO.Banco & _
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & path & ".asp?Banco=" & objBoletoVO.Banco &
                    "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & objBoletoVO.Sacado &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" &
                    "&LocalSaida=" & Mid(txtResLocalSaida.Text.Trim, 1, 200) &
                    "&HoraSaida=" & cmbResHoraSaida.Text &
                    "&NomeResp=" & Mid(txtResNome.Text.Trim, 1, 80) &
                    "&Destino=" & LocalDestino &
                    "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
                End If
            Else

                If DateDiff(DateInterval.Day, CDate(Format(Date.Now, "dd/MM/yyyy")),
                    CDate(Format(CDate(hddResDataIni.Value), "dd/MM/yyyy"))) > 30 Then
                    objBoletoVO = objBoletoDAO.GeraBoleto(hddResId.Value, "C", Integrante,
                    Format(DateAdd(DateInterval.Day, -30, CDate(hddResDataIni.Value)), "dd/MM/yyyy"),
                    User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)
                Else
                    objBoletoVO = objBoletoDAO.GeraBoleto(hddResId.Value, "C", Integrante, Format(DateAdd(DateInterval.Day, CInt(hddDiasPrazo.Value), Date.Now), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), sender.CommandArgument.ToString)
                End If
                path = ("Cupons/" & "CupomCobranca" & Destino)
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('http://turismosocialweb.sescgo.com.br/CupomCobranca" & Destino & ".asp?Banco=" & objBoletoVO.Banco & _
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "javascript:window.open('" & path & ".asp?Banco=" & objBoletoVO.Banco &
                    "&Boleto=" & objBoletoVO.Boleto &
                    "&CodBanco=" & Mid(objBoletoVO.CodBanco, 1, 3) & "-" &
                        Mid(objBoletoVO.CodBanco, 4, 1) &
                    "&Cedente=" & objBoletoVO.Cedente &
                    "&Reserva=" & objBoletoVO.Reserva &
                    "&Vencimento=" & Format(CDate(objBoletoVO.Vencimento), "dd/MM/yyyy") &
                    "&Sacado=" & objBoletoVO.Sacado &
                    "&AgeCodCed=" & objBoletoVO.AgeCodCed &
                    "&NossoNum=" & objBoletoVO.NossoNum &
                    "&Demonstrativo=" & objBoletoVO.Demonstrativo &
                    "&VlrDoc=" & objBoletoVO.VlrDoc &
                    "&DataDocumento=" & Format(CDate(objBoletoVO.DataDocumento), "dd/MM/yyyy") &
                    "&DataProces=" & Format(CDate(objBoletoVO.DataProces), "dd/MM/yyyy") &
                    "&Codigoipte=" & objBoletoVO.CodigoIPTE &
                    "&CodigoBarra=" & objBoletoVO.CodigoBarra &
                    "&End1=" & objBoletoVO.End1 &
                    "&Comando=" &
                    "&Periodo=" & objBoletoVO.Periodo &
                    "&Entrada=" & objBoletoVO.Entrada &
                    "&Saida=" & objBoletoVO.Saida &
                    "&QdtePess=" & objBoletoVO.QtdePess &
                    "&Acomodacao=" & objBoletoVO.Acomodacao &
                    "&Percentual=100" & "', '_blank', " & "'menubar=1, resizable=1, fullscreen=yes')", True)
            End If
            ListaFinanceiroViaResId()
        End If
    End Sub

    Protected Sub gdvReserva10_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva10.SelectedIndexChanged
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
        End If

        Dim objTestaGrupo As New Uteis.TestaUsuario
        Dim objListaGrupo As New Uteis.TestaUsuario
        Dim varGrupos = objListaGrupo.listaGrupos(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")).ToUpper


        If gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(3) = "M" Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" And Not Grupos.Contains("Turismo Social Excluir Conciliacao Manual") Then
                Mensagem("Permissão negada para essa operação.")
                Return
            ElseIf Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Not Grupos.Contains("Turismo Social Piri Excluir Conciliacao Manual") Then
                Mensagem("Permissão negada para essa operação.")
                Return
            End If
            objReservaListagemFinanceiroDAO.Acao(
            gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(1).ToString(),
            gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(0).ToString(),
            "01/01/2000",
            "0",
            User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
            "",
            "M",
            "")
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" And Not Grupos.Contains("Turismo Social Fazer Conciliacao Manual") Then
                Mensagem("Permissão negada para essa operação.")
                Return
            ElseIf Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Not Grupos.Contains("Turismo Social Piri Fazer Conciliacao Manual") Then
                Mensagem("Permissão negada para essa operação.")
                Return
            End If
            objReservaListagemFinanceiroDAO.Acao(
            "0",
            gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(0).ToString(),
            Format(Date.Now, "dd/MM/yyyy"),
            gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(9).Replace(".", "").Replace(",", "."),
            User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
            gdvReserva10.DataKeys(gdvReserva10.SelectedIndex).Item(4).ToString(),
            "M",
            "")
        End If
        ListaFinanceiroViaResId()
        gdvReserva10.SelectedIndex = -1
        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
        CarregaCmbRefeicaoPrato()
        CarregaDadosReserva()
        ListaIntegranteViaResId()
        gdvReserva10.DataBind()
        'Carregando o grid com as informações financeiras, Ressarcimento.
        If hddResId.Value > 0 Then
            gdvRessarcimento.DataSource = objReservaListagemFinanceiroDAO.ConsultaRessarcimento(hddResId.Value)
            gdvRessarcimento.DataBind()
        End If
    End Sub

    Protected Sub btnEmissivoNova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmissivoNova.Click
        Try
            btnReservaGravar.Attributes.Add("RT", "") 'Limpando o RT - Na reserva que possue apto RT ele nunca pederá ser HospedeJá
            pnlSolicitacaoSelecionada_RoundedCornersExtender.Enabled = False
            pnlIntegranteGeral_RoundedCornersExtender.Enabled = False
            If CDate(txtDataInicialSolicitacao.Text) = CDate(txtDataFinalSolicitacao.Text) Then
                hddResCaracteristica.Value = "P"
                pnlDestinoGrupo.Visible = True
                pnlGrupo.Visible = False
                lblResHoraSaida.Visible = False
            Else
                hddResCaracteristica.Value = "E"
                pnlDestinoGrupo.Visible = False

                pnlGrupo.Visible = True
                pnlDestinoGrupo.Visible = False
                lblResHoraSaida.Visible = True
                cmbReservaHoraSaida.Visible = True
                gdvReserva9.Columns(3).Visible = True
                gdvReserva9.Columns(4).Visible = True

            End If
            txtResMatricula.Text = ""
            pnlResponsavelTitulo.Visible = True
            pnlResponsavelAcao.Visible = True

            cmbReservaHoraSaida.Visible = False
            btnHospedagemNova.Enabled = False
            btnReservaGravar.Visible = True
            btnReservaGravar.Enabled = True
            btnReservaCalculo.Visible = True
            btnReservaCancelar.Enabled = (hddResId.Value <> "0")
            btnReservaCancelar.Visible = (hddResId.Value <> "0")
            btnReservaReativar.Enabled = False
            btnReservaReativar.Visible = False
            btnInformarRestituicao.Enabled = False
            btnInformarRestituicao.Visible = False
            txtResDtLimiteRetorno.Text = Format(Date.Now, "dd/MM/yyyy")
            cmbResHrLimiteRetorno.SelectedValue = Format(CLng(Now.Hour), "00")
            'Esses campos para individual não aparece somente para grupo e passeios
            lblResCatCobranca.Visible = True
            cmbResCatCobranca.Visible = True
            btnReservaCalculo.Visible = False
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
       "&Erro=Erro ao gerar um novo emissivo. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: BtnEmissivoNova")
        End Try
    End Sub

    Protected Sub imgBtnDtCheck_InMenos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnDtCheck_InMenos.Click, imgBtnDtCheck_InMais.Click, imgBtnDtCheck_OutMenos.Click, imgBtnDtCheck_OutMais.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
        End If
        Dim Msg As String = "Houve uma falha na operação"
        If sender Is imgBtnDtCheck_InMenos Then
            'Retirar um dia da data Inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, -1, CDate(txtHosDataIniSol.Text)), txtHosDataFimSol.Text)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(hddSolId.Value, hddIntId.Value, -1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
            txtHosDataIniSol.Text = Mid(DateAdd(DateInterval.Day, -1, CDate(txtHosDataIniSol.Text)).ToString, 1, 10)
        ElseIf sender Is imgBtnDtCheck_InMais Then
            'Validação do HospedeJá - Somei um dia na data inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, 1, CDate(txtHosDataIniSol.Text)), txtHosDataFimSol.Text)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(hddSolId.Value, hddIntId.Value, 1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
            txtHosDataIniSol.Text = Mid(DateAdd(DateInterval.Day, 1, CDate(txtHosDataIniSol.Text)).ToString, 1, 10)
        ElseIf sender Is imgBtnDtCheck_OutMenos Then
            'Validação do HospedeJá - Retirei um dia na data final
            ValidaDataHospedeJa(txtHosDataIniSol.Text, DateAdd(DateInterval.Day, -1, CDate(txtHosDataFimSol.Text)))

            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(hddSolId.Value, hddIntId.Value, 0, -1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
            txtHosDataFimSol.Text = Mid(DateAdd(DateInterval.Day, -1, CDate(txtHosDataFimSol.Text)).ToString, 1, 10)
        ElseIf sender Is imgBtnDtCheck_OutMais Then
            'Validação do HospedeJá - Somei um dia na data Final
            ValidaDataHospedeJa(txtHosDataIniSol.Text, DateAdd(DateInterval.Day, 1, CDate(txtHosDataFimSol.Text)))
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(hddSolId.Value, hddIntId.Value, 0, 1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
            txtHosDataFimSol.Text = Mid(DateAdd(DateInterval.Day, 1, CDate(txtHosDataFimSol.Text)).ToString, 1, 10)
        End If

        lnkIntNome_Click(sender:=Nothing, e:=Nothing)
        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaHosId(hddHosId.Value)

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" & Msg & "');", True)

        CarregaDadosIntegrante()

        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
        gdvReserva8.DataSource = lista
        gdvReserva8.DataBind()
        gdvReserva8.SelectedIndex = -1
        If (CDate(txtHosDataIniSol.Text) > Now.Date) Then 'And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value)) Then
            imgBtnDtCheck_InMenos.Visible = True And (InStr("CFE", hddResStatus.Value) = 0) And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value))
        Else
            imgBtnDtCheck_InMenos.Visible = False
        End If
        If (DateDiff(DateInterval.Day, CDate(txtHosDataIniSol.Text), CDate(txtHosDataFimSol.Text)) > 1) Then
            imgBtnDtCheck_InMais.Visible = True And (InStr("CFE", hddResStatus.Value) = 0) And (CDate(txtHosDataIniSol.Text) = CDate(hddResDataIni.Value))
            imgBtnDtCheck_OutMenos.Visible = True And (InStr("CF", hddResStatus.Value) = 0) And (CDate(txtHosDataFimSol.Text) = CDate(hddResDataFim.Value))
        Else
            imgBtnDtCheck_InMais.Visible = False
            imgBtnDtCheck_OutMenos.Visible = False
        End If
        imgBtnDtCheck_OutMais.Visible = True And (InStr("CF", hddResStatus.Value) = 0) And (CDate(txtHosDataFimSol.Text) = CDate(hddResDataFim.Value))
    End Sub

    Protected Sub imgBtnDtCheckInMenos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
        End If

        Dim varAux As Char = sender.AccessKey
        Dim Msg As String = "Houve uma falha na operação"
        If sender.AccessKey = "1" Then
            'Validação do HospedeJá - Retirei um dia na data inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, -1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1))), CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2)))
            'Processo de inserção do dia
            hddDataInAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")), 0, -1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "2" Then ' + do dia - 1ª Data do Grid
            'Validação do HospedeJá - Somei um dia na data inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, 1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1))), CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2)))
            'Processo de inserção do dia
            hddDataInAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")), 0, 1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "3" Then
            'Validação do HospedeJá - Retirei um dia na data final
            ValidaDataHospedeJa(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1), DateAdd(DateInterval.Day, -1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2))))
            'Processo de inserção do dia
            hddDataOutAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")), 0, 0, -1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "4" Then ' + do dia - 2ª Data do Grid
            'Validação do HospedeJá - Somei um dia na data Final
            ValidaDataHospedeJa(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1), DateAdd(DateInterval.Day, 1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2))))
            'Processo de inserção do dia
            hddDataOutAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")), 0, 0, 1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        End If
        hddHosId.Value = Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#"))
        lnkAcomodacao_Click(sender:=Nothing, e:=Nothing)
        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "") 'consultarReservaViaResId
        gdvReserva8.DataSource = lista
        gdvReserva8.DataBind()
        gdvReserva8.SelectedIndex = -1

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" & Msg & "');", True)
        'End If
    End Sub

    Protected Sub imgBtnDtCheckInMais_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
        End If


        'se apertar + :Vericando se poderá adicionar um dia a mais na reserva - Motivo: Estouro da capacidade de leitos
        If sender.accesskey = "4" Then
            Dim ImagemSolId As ImageButton = sender
            Dim row As GridViewRow = ImagemSolId.NamingContainer 'Dim index As Integer = row.RowIndex
            radAcomodacao.Attributes.Add("solId", gdvReserva9.DataKeys(row.RowIndex()).Item("solId").ToString)
            hddIdade.Value = calculaIdade(Format(CDate(gdvReserva9.DataKeys(row.RowIndex()).Item("IntDtNascimento").ToString), "yyyy/MM/dd"), CDate(gdvReserva9.DataKeys(row.RowIndex()).Item("hosDataIniSol").ToString))

            ValidaTotaldeIntegrantesPorAcomodacao()
            radAcomodacao.Attributes.Remove("solId")
            'Irá verificar se a inserção será permitida, respeitando o limite máximo de leitos por acomodação
            If (hddResCaracteristica.Value = "I" And cmbOrgId.SelectedValue <> "37") Then ' And cmbOrgId.SelectedValue <> "49")) Then 'Passeio e Grupos não entram na questão - exceção: Presidencia e Dr
                If imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" Then
                    Mensagem("Esgotado o número de vagas para essa acomodação.")
                    Return
                ElseIf (imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And hddIdade.Value >= CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                    Mensagem("Vagas esgotadas para essa acomodação. \nObs.: Será possível inserir apenas integrantes com idade de berço, que tenha menos de " & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos.\n\n Inserção não permitida.")
                    Return
                ElseIf ((imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "L" And
                        imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N") And
                        hddIdade.Value > CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                    Mensagem("A idade do integrante não poderá ser superior a idade de criança de berço (" & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos). \n\n Inserção não permitida.")
                    Return
                End If
            End If
        End If


        Dim varAux As Char = sender.AccessKey
        Dim Msg As String = "Houve uma falha na operação"
        If sender.AccessKey = "1" Then
            'Validação do HospedeJá - Retirei um dia na data inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, -1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1))), CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2)))
            'Processo de inserção do dia
            hddDataInAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")),
            Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1),
            -1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "2" Then
            'Validação do HospedeJá - Somei um dia na data inicial
            ValidaDataHospedeJa(DateAdd(DateInterval.Day, 1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1))), CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2)))
            'Processo de inserção do dia
            hddDataInAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")),
            Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1),
            1, 0, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "3" Then
            'Validação do HospedeJá - Retirei um dia na data final
            ValidaDataHospedeJa(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1), DateAdd(DateInterval.Day, -1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2))))
            'Processo de inserção do dia
            hddDataOutAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")),
            Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1),
            0, -1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        ElseIf sender.AccessKey = "4" Then
            'Validação do HospedeJá - Somei um dia na data Final
            ValidaDataHospedeJa(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("$") + 2, (sender.CommandArgument.ToString.IndexOf("æ")) - (sender.CommandArgument.ToString.IndexOf("$")) - 1), DateAdd(DateInterval.Day, 1, CDate(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2))))
            'Processo de inserção do dia
            hddDataOutAntes.Value = Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("æ") + 2)
            Msg = objReservaListagemIntegranteDAO.EspacarPeriodo(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")),
            Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("$")) - (sender.CommandArgument.ToString.IndexOf("#")) - 1),
            0, 1, Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
        End If
        hddHosId.Value = Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#"))
        lnkIntNome_Click(sender:=Nothing, e:=Nothing)
        lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
        gdvReserva8.DataSource = lista
        gdvReserva8.DataBind()
        gdvReserva8.SelectedIndex = -1
        'If ((varAux = "1" Or varAux = "2") And hddDataInAntes.Value = hddDataInDepois.Value) Or _
        '   ((varAux = "3" Or varAux = "4") And hddDataOutAntes.Value = hddDataOutDepois.Value) Then
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" & Msg & "');", True)
        'End If
    End Sub

    Protected Sub btnResEmailGrupo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResEmailGrupo.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""
        atualizarUsuarioGrupo()
        btnReservaGravar_Click(sender, e)
        enviarEmail()
    End Sub

    Protected Sub atualizarUsuarioGrupo()
        Try

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
            Else
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialPiri")
            End If

            ObjReservaVO.resId = hddResId.Value
            ObjReservaVO.resIdWeb = cmbResIdWeb.SelectedValue

            ObjReservaDAO.AtualizaUsuarioGrupo(ObjReservaVO)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCaixa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCaixa.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""
        Dim Receber = "" 'Onde será recebido o valor
        Dim BancoDestino = ""
        Dim Origem = "" 'Origem da reserva
        Dim BancoOrigem = ""
        Try
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
                ObjEnviaPagamentoCaixaDAO = New EnviaPagamentoCaixaDAO("TurismoSocialCaldas")
                Origem = "C"
                BancoOrigem = "TurismoSocialCaldas"
            Else
                objReservaListagemFinanceiroDAO = New Turismo.ReservaListagemFinanceiroDAO("TurismoSocialPiri")
                ObjEnviaPagamentoCaixaDAO = New EnviaPagamentoCaixaDAO("TurismoSocialPiri")
                Origem = "P"
                BancoOrigem = "TurismoSocialPiri"
            End If
            objEnviaPagamentoCaixaVO = New EnviaPagamentoCaixaVO
            With objEnviaPagamentoCaixaVO
                .ResId = hddResId.Value
                .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            End With

            'Pesquisando dados da origem da reserva, essas informações serão repassadas na hora da inserção dos dados logo abaixo
            objEnviaPagamentoCaixaVO = ObjEnviaPagamentoCaixaDAO.ConsultaValoresParaEnvioCaixa(objEnviaPagamentoCaixaVO, 100, BancoOrigem)
            With objEnviaPagamentoCaixaVO
                .ResId = hddResId.Value
                .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            End With

            'Esse procedimento irá consultar no AD a qual unidade o usuário pertence e logo 
            'em seguida irá gravar o nome da unidade em uma session
            Dim ping As New System.Net.NetworkInformation.Ping()
            Dim objTestaGrupo As New Uteis.TestaUsuario
            Dim varInitials As String, varOffice As String = ""
            varInitials = objTestaGrupo.listaInitials(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
            If IsNothing(varInitials) Then
            Else
                varOffice = objTestaGrupo.listaOffice(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))
                If (Not IsNothing(varOffice)) Or varOffice <> "" Then
                    'Session.Add("UnidadeOperacional", varOffice)
                Else
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Não foi encontrado no Escritório do Active Directory a unidade do usuário atual, informe o Centro de Informática');", True)
                    Exit Try
                End If
            End If
            If varOffice = "Caldas Novas" Then
                Receber = "C" 'REceber no caixa de Caldas Novas
            ElseIf varOffice = "Pirenópolis" Then
                Receber = "P" 'REceber no caixa de Pirenópolis
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não foi possível detectar a localização do usuário atual. Informe o centro de informática.(CEIN: Verificar o Escritório No AD, veja se esta preenchido devidamente.)');", True)
                Exit Try
            End If

            If (Receber = "C") Then 'Reserva de Caldas Novas será enviada para o caixa de Caldas Novas
                ObjEnviaPagamentoCaixaDAO = New EnviaPagamentoCaixaDAO("TurismoSocialCaldas")
                BancoDestino = "TurismoSocialCaldas"
            ElseIf (Receber = "P") Then 'Reserva de Pirenópolis será enviada para o caixa de Pirenopolis
                ObjEnviaPagamentoCaixaDAO = New EnviaPagamentoCaixaDAO("TurismoSocialPiri")
                BancoDestino = "TurismoSocialPiri"
            End If

            With objEnviaPagamentoCaixaVO
                .ProId = Origem
            End With
            ObjEnviaPagamentoCaixaDAO.EnviaPagtoCaixa(objEnviaPagamentoCaixaVO, 100, BancoDestino)
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Valores enviados com sucesso para pagamento no caixa!');", True)
            'objReservaListagemFinanceiroDAO.Caixa(hddResId.Value, User.Identity.Name.ToString)
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro ao enviar o recebimento para o caixa.  &excecao=" & ex.StackTrace.ToString &
         "&Erro=Erro ao enviar recebimento para o caixa. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnCaixa Resid:" & hddResId.Value & " ")
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro de comunicação com o servidor ou você possui um caixa aberto.');", True)
            'Return
        End Try
    End Sub

    Protected Sub imgBtnReservaAcaoVoltarRecepcao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnReservaAcaoVoltarRecepcao.Click
        Server.Transfer("~/Recepcao.aspx")
    End Sub

    Protected Sub imgResponsavel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdrResponsavel_Click(sender, e)
    End Sub

    Protected Sub lnkhdrResponsavel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdrResponsavel" Or sender.ID = "imgResponsavel" Then
            If btnReserva.CommandName = "resNomeComplemento" Then
                btnReserva.CommandName = "resNomeComplemento desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "resNomeComplemento"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrReserva" Or sender.ID = "imgReserva" Then
            If btnReserva.CommandName = "resStatusDesc" Then
                btnReserva.CommandName = "resStatusDesc desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgReserva"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "resStatusDesc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgReserva"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrPeriodo" Or sender.ID = "imgPeriodo" Then
            If btnReserva.CommandName = "resDataIni, resDataFim" Then
                btnReserva.CommandName = "resDataIni desc, resDataFim desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "resDataIni, resDataFim"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante" Then
            If btnReserva.CommandName = "Hospede" Then
                btnReserva.CommandName = "Hospede desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "Hospede"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrApto" Or sender.ID = "imgApto" Then
            If btnReserva.CommandName = "qtdAcomodacao" Then
                btnReserva.CommandName = "qtdAcomodacao desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgApto"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "qtdAcomodacao"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgApto"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrValores" Or sender.ID = "imgValores" Then
            If btnReserva.CommandName = "ValorPago" Then
                btnReserva.CommandName = "ValorPago desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgValores"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "ValorPago"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgValores"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor" Then
            If btnReserva.CommandName = "ResUsuario" Then
                btnReserva.CommandName = "ResUsuario desc"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnReserva.CommandName = "ResUsuario"
                btnReserva_Click(sender, e)
                CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If
        CType(gdvReserva1.HeaderRow.FindControl("imgResponsavel"), ImageButton).Visible = (sender.ID = "lnkhdrResponsavel" Or sender.ID = "imgResponsavel")
        CType(gdvReserva1.HeaderRow.FindControl("imgReserva"), ImageButton).Visible = (sender.ID = "lnkhdrReserva" Or sender.ID = "imgReserva")
        CType(gdvReserva1.HeaderRow.FindControl("imgPeriodo"), ImageButton).Visible = (sender.ID = "lnkhdrPeriodo" Or sender.ID = "imgPeriodo")
        CType(gdvReserva1.HeaderRow.FindControl("imgIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante")
        CType(gdvReserva1.HeaderRow.FindControl("imgApto"), ImageButton).Visible = (sender.ID = "lnkhdrApto" Or sender.ID = "imgApto")
        CType(gdvReserva1.HeaderRow.FindControl("imgValores"), ImageButton).Visible = (sender.ID = "lnkhdrValores" Or sender.ID = "imgValores")
        CType(gdvReserva1.HeaderRow.FindControl("imgServidor"), ImageButton).Visible = (sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor")
    End Sub

    Protected Sub lnkhdrAcomodacao_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdrAcomodacao" Or sender.ID = "imgAcomodacao" Then

            If btnAcomodacao.CommandName = "AcmDescricao" Then
                btnAcomodacao.CommandName = "AcmDescricao desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "AcmDescricao"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrDiaCheckInAcomodacao" Or sender.ID = "imgDiaCheckInAcomodacao" Then

            If btnAcomodacao.CommandName = "datepart(weekday, SolDataIni), datepart(weekday, SolDataFim)" Then
                btnAcomodacao.CommandName = "datepart(weekday, SolDataIni) desc, datepart(weekday, SolDataFim) desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckInAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "datepart(weekday, SolDataIni), datepart(weekday, SolDataFim)"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckInAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrDataCheckInAcomodacao" Or sender.ID = "imgDataCheckInAcomodacao" Then

            If btnAcomodacao.CommandName = "SolDataIni, SolDataFim" Then
                btnAcomodacao.CommandName = "SolDataIni desc, SolDataFim desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckInAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "SolDataIni, SolDataFim"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckInAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrDiaCheckOutAcomodacao" Or sender.ID = "imgDiaCheckOutAcomodacao" Then

            If btnAcomodacao.CommandName = "datepart(weekday, SolDataFim), datepart(weekday, SolDataIni)" Then
                btnAcomodacao.CommandName = "datepart(weekday, SolDataFim) desc, datepart(weekday, SolDataIni) desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckOutAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "datepart(weekday, SolDataFim), datepart(weekday, SolDataIni)"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckOutAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrDataCheckOutAcomodacao" Or sender.ID = "imgDataCheckOutAcomodacao" Then

            If btnAcomodacao.CommandName = "SolDataFim, SolDataIni" Then
                btnAcomodacao.CommandName = "SolDataFim desc, SolDataIni desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckOutAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "SolDataFim, SolDataIni"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckOutAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor" Then
            If btnAcomodacao.CommandName = "solUsuario" Then
                btnAcomodacao.CommandName = "solUsuario desc"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnAcomodacao.CommandName = "solUsuario"
                btnAcomodacao_Click(sender, e)
                CType(gdvReserva2.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If

        CType(gdvReserva2.HeaderRow.FindControl("imgAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdrAcomodacao" Or sender.ID = "imgAcomodacao")
        CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckInAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdrDiaCheckInAcomodacao" Or sender.ID = "imgDiaCheckInAcomodacao")
        CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckInAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdrDataCheckInAcomodacao" Or sender.ID = "imgDataCheckInAcomodacao")
        CType(gdvReserva2.HeaderRow.FindControl("imgDiaCheckOutAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdrDiaCheckOutAcomodacao" Or sender.ID = "imgDiaCheckOutAcomodacao")
        CType(gdvReserva2.HeaderRow.FindControl("imgDataCheckOutAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdrDataCheckOutAcomodacao" Or sender.ID = "imgDataCheckOutAcomodacao")
        CType(gdvReserva2.HeaderRow.FindControl("imgServidor"), ImageButton).Visible = (sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor")
    End Sub

    Protected Sub imgAcomodacao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdrAcomodacao_Click(sender, e)
    End Sub

    Protected Sub lnkhdrIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante" Then
            'If btnIntegrante.CommandName = "intNomeTitular, i.IntDtNascimento, i.intNome, i.intDataIni, i.intDataFim" Then
            If btnIntegrante.CommandName = "intNomeTitular, i.IntDtNascimento, intNome, i.intDataIni, i.intDataFim" Then

                btnIntegrante.CommandName = "IntNome desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "intNomeTitular, i.IntDtNascimento, intNome, i.intDataIni, i.intDataFim"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrCategoria" Or sender.ID = "imgCategoria" Then
            If btnIntegrante.CommandName = "CatDescricao" Then
                btnIntegrante.CommandName = "CatDescricao desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCategoria"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "CatDescricao"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCategoria"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrCheckInIntegrante" Or sender.ID = "imgCheckInIntegrante" Then
            If btnIntegrante.CommandName = "i.IntDataIni, i.IntDataFim" Then
                btnIntegrante.CommandName = "i.IntDataIni desc, i.IntDataFim desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCheckInIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "i.IntDataIni, i.IntDataFim"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCheckInIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrCheckOutIntegrante" Or sender.ID = "imgCheckOutIntegrante" Then
            If btnIntegrante.CommandName = "i.IntDataFim, i.IntDataIni" Then
                btnIntegrante.CommandName = "i.IntDataFim desc, i.IntDataIni desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCheckOutIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "i.IntDataFim, i.IntDataIni"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgCheckOutIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrVlrDevidoIntegrante" Or sender.ID = "imgVlrDevidoIntegrante" Then
            If btnIntegrante.CommandName = "hosValorDevido" Then
                btnIntegrante.CommandName = "hosValorDevido desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgVlrDevidoIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "hosValorDevido"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgVlrDevidoIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrVlrPagoIntegrante" Or sender.ID = "imgVlrPagoIntegrante" Then
            If btnIntegrante.CommandName = "hosValorPago" Then
                btnIntegrante.CommandName = "hosValorPago desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgVlrPagoIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "hosValorPago"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgVlrPagoIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor" Then
            If btnIntegrante.CommandName = "i.intUsuario" Then
                btnIntegrante.CommandName = "i.intUsuario desc"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnIntegrante.CommandName = "i.intUsuario"
                btnIntegrante_Click(sender, e)
                CType(gdvReserva3.HeaderRow.FindControl("imgServidor"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If
        CType(gdvReserva3.HeaderRow.FindControl("imgIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrIntegrante" Or sender.ID = "imgIntegrante")
        CType(gdvReserva3.HeaderRow.FindControl("imgCategoria"), ImageButton).Visible = (sender.ID = "lnkhdrCategoria" Or sender.ID = "imgCategoria")
        CType(gdvReserva3.HeaderRow.FindControl("imgCheckInIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrCheckInIntegrante" Or sender.ID = "imgCheckInIntegrante")
        CType(gdvReserva3.HeaderRow.FindControl("imgCheckOutIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrCheckOutIntegrante" Or sender.ID = "imgCheckOutIntegrante")
        CType(gdvReserva3.HeaderRow.FindControl("imgVlrDevidoIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrVlrDevidoIntegrante" Or sender.ID = "imgVlrDevidoIntegrante")
        CType(gdvReserva3.HeaderRow.FindControl("imgVlrPagoIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdrVlrPagoIntegrante" Or sender.ID = "imgVlrPagoIntegrante")
        CType(gdvReserva3.HeaderRow.FindControl("imgServidor"), ImageButton).Visible = (sender.ID = "lnkhdrServidor" Or sender.ID = "imgServidor")
    End Sub

    Protected Sub imgIntegrante_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdrIntegrante_Click(sender, e)
    End Sub

    Protected Sub lnkhdrCobranca_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdrCobranca" Or sender.ID = "imgCobranca" Then
            If btnFinanceiro.CommandName = "BolImpNossoNumero" Then
                btnFinanceiro.CommandName = "BolImpNossoNumero desc"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgCobranca"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnFinanceiro.CommandName = "BolImpNossoNumero"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgCobranca"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrVencimento" Or sender.ID = "imgVencimento" Then
            If btnFinanceiro.CommandName = "BolImpDtVencimento" Then
                btnFinanceiro.CommandName = "BolImpDtVencimento desc"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgVencimento"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnFinanceiro.CommandName = "BolImpDtVencimento"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgVencimento"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrPagamento" Or sender.ID = "imgPagamento" Then
            If btnFinanceiro.CommandName = "VenData" Then
                btnFinanceiro.CommandName = "VenData desc"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgPagamento"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnFinanceiro.CommandName = "VenData"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgPagamento"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrVlrPago" Or sender.ID = "imgVlrPago" Then
            If btnFinanceiro.CommandName = "VenValor" Then
                btnFinanceiro.CommandName = "VenValor desc"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgVlrPago"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnFinanceiro.CommandName = "VenValor"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgVlrPago"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If

        ElseIf sender.ID = "lnkhdrForma" Or sender.ID = "imgVlrPago" Then
            If btnFinanceiro.CommandName = "VenStatus" Then
                btnFinanceiro.CommandName = "VenStatus desc"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgForma"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnFinanceiro.CommandName = "VenStatus"
                btnFinanceiro_Click(sender, e)
                CType(gdvReserva4.HeaderRow.FindControl("imgForma"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If
        CType(gdvReserva4.HeaderRow.FindControl("imgCobranca"), ImageButton).Visible = (sender.ID = "lnkhdrCobranca" Or sender.ID = "imgCobranca")
        CType(gdvReserva4.HeaderRow.FindControl("imgVencimento"), ImageButton).Visible = (sender.ID = "lnkhdrVencimento" Or sender.ID = "imgVencimento")
        CType(gdvReserva4.HeaderRow.FindControl("imgPagamento"), ImageButton).Visible = (sender.ID = "lnkhdrPagamento" Or sender.ID = "imgPagamento")
        CType(gdvReserva4.HeaderRow.FindControl("imgVlrPago"), ImageButton).Visible = (sender.ID = "lnkhdrVlrPago" Or sender.ID = "imgVlrPago")
        CType(gdvReserva4.HeaderRow.FindControl("imgForma"), ImageButton).Visible = (sender.ID = "lnkhdrForma" Or sender.ID = "imgForma")
    End Sub

    Protected Sub imgCobranca_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdrCobranca_Click(sender, e)
    End Sub

    Protected Sub rblDiasPrazo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hddDiasPrazo.Value = sender.Text
    End Sub

    Protected Sub btnReservaCalculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReservaCalculo.Click

        'Só pertencentes a esse grupo poderão ter acesso à Planilha de custos
        'Com pagamento no boleto e vencimento no dia da reserva
        Dim objTestaGrupo As New Uteis.TestaUsuario
        Dim objListaGrupo As New Uteis.TestaUsuario
        Dim varGrupos = objListaGrupo.listaGrupos(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")).ToUpper
        If varGrupos.Contains("TURISMO SOCIAL PLANILHA DE CUSTO PASSEIOS") Then
            pnlPlanilha.Visible = False
            pnlPlanilhaCusto.Visible = True
        Else
            pnlPlanilha.Visible = False
            pnlPlanilhaCusto.Visible = False
            Mensagem("Você não possui permissão de acesso à Planilha de Custos.")
            Return
        End If

        If hddResId.Value = "0" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "if (confirm('Salvar dados da reserva?')){document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$hddFinalizaPor.value='C';document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$btnFinaliza.click();}else{document.forms['aspnetForm'].ctl00$conPlaHolTurismoSocial$txtConsulta.focus();}", True)
        Else
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
                objPlanilhaCustoItemDAO = New Turismo.PlanilhaCustoItemDAO("TurismoSocialCaldas")
            Else
                objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
                objPlanilhaCustoItemDAO = New Turismo.PlanilhaCustoItemDAO("TurismoSocialPiri")
            End If
            objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
            If objPlanilhaCustoVO.plcQtde = "0" And objPlanilhaCustoVO.resId = "0" Then
                txtPlcQtde.Text = "30"
            Else
                txtPlcQtde.Text = objPlanilhaCustoVO.plcQtde
            End If
            txtPlcCapacidade.Text = objPlanilhaCustoVO.plcCapacidade
            txtPlcGuia.Text = objPlanilhaCustoVO.plcGuia
            txtPlcMotorista.Text = objPlanilhaCustoVO.plcMotorista
            If objPlanilhaCustoVO.plcPercentualConveniado = "0" And objPlanilhaCustoVO.resId = "0" Then
                txtPlcPercentualConveniado.Text = "20"
            Else
                txtPlcPercentualConveniado.Text = objPlanilhaCustoVO.plcPercentualConveniado
            End If
            If objPlanilhaCustoVO.plcPercentualUsuario = "0" And objPlanilhaCustoVO.resId = "0" Then
                txtPlcPercentualUsuario.Text = "30"
            Else
                txtPlcPercentualUsuario.Text = objPlanilhaCustoVO.plcPercentualUsuario
            End If
            If objPlanilhaCustoVO.plcMargem = "0" And objPlanilhaCustoVO.resId = "0" Then
                txtPlcMargem.Text = "5"
            Else
                txtPlcMargem.Text = objPlanilhaCustoVO.plcMargem
            End If
            txtPlcColo.Text = objPlanilhaCustoVO.plcColo
            TxtPlcIsento.Text = objPlanilhaCustoVO.PlcIdadeIsento
            TxtPlcCrianca.Text = objPlanilhaCustoVO.PlcIdadeCrianca
            ckbPlcValorado.Checked = (objPlanilhaCustoVO.plcValorado = "S")
            ckbPlcAutorizaConveniado.Checked = (objPlanilhaCustoVO.plcAutorizaConveniado = "S")
            ckbPlcAutorizaUsuario.Checked = (objPlanilhaCustoVO.plcAutorizaUsuario = "S")
            lista = objPlanilhaCustoItemDAO.consultar(objPlanilhaCustoVO.resId)
            gdvReserva13.DataSource = lista
            gdvReserva13.DataBind()
            gdvReserva13.SelectedIndex = -1
            pnlReservaAcao.Visible = False
            pnlPlanilhaCusto.Visible = True
            imgBtnCalcularCusto_Click(sender, e)
        End If
    End Sub

    Protected Sub imgBtnPlanilhaCustoVoltar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnPlanilhaItemVoltar.Click
        pnlPlanilhaItem.Visible = False
        pnlPlanilhaCusto.Visible = True
    End Sub

    Protected Sub btnPlanilhaCustoItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlanilhaCustoItem.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialCaldas")
        Else
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialPiri")
        End If
        lista = objPlanilhaItemDAO.consultar
        gdvReserva5.DataSource = lista
        gdvReserva5.DataBind()
        gdvReserva5.SelectedIndex = -1
        pnlPlanilhaCusto.Visible = False
        pnlPlanilhaItem.Visible = True
        txtPliDescricao.Focus()
        btnPlanilhaItemNovo.Enabled = False
        btnPlanilhaItemExcluir.Enabled = False
        txtPliId.Text = 0
    End Sub

    Protected Sub imgBtnPlanilhaCustoVoltar_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnPlanilhaCustoVoltar.Click
        pnlPlanilhaCusto.Visible = False
        pnlReservaAcao.Visible = True
    End Sub

    Protected Sub gdvReserva13_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvReserva13.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgBtnCalcular"), ImageButton).Visible = (CInt(gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciId").ToString) >= 0)
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciId") = -1 Or
               gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciId") = -2 Or
               gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciId") = -3 Then
                CType(e.Row.FindControl("imgBtnCalcularCusto"), ImageButton).Visible = (CInt(gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciId").ToString) = -1)
                CType(e.Row.FindControl("txtPciValorTotal"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorTotal"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorComAdulto"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorComAdulto"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorComCrianca"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorComCrianca"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorComIsento"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorComIsento"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorConvAdulto"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorConvAdulto"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorConvCrianca"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorConvCrianca"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorConvIsento"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorConvIsento"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorUsuAdulto"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorUsuAdulto"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorUsuCrianca"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorUsuCrianca"), Label).Visible = True
                CType(e.Row.FindControl("txtPciValorUsuIsento"), TextBox).Visible = False
                CType(e.Row.FindControl("lblPciValorUsuIsento"), Label).Visible = True
            Else
                CType(e.Row.FindControl("txtPciValorTotal"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorComAdulto"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorComCrianca"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorComIsento"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorConvAdulto"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorConvCrianca"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorConvIsento"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorUsuAdulto"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorUsuCrianca"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                CType(e.Row.FindControl("txtPciValorUsuIsento"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorTotal") = "0" Then
                CType(e.Row.FindControl("txtPciValorTotal"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorComAdulto") = "0" Then
                CType(e.Row.FindControl("txtPciValorComAdulto"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorComCrianca") = "0" Then
                CType(e.Row.FindControl("txtPciValorComCrianca"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorComIsento") = "0" Then
                CType(e.Row.FindControl("txtPciValorComIsento"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorConvAdulto") = "0" Then
                CType(e.Row.FindControl("txtPciValorConvAdulto"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorConvCrianca") = "0" Then
                CType(e.Row.FindControl("txtPciValorConvCrianca"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorConvIsento") = "0" Then
                CType(e.Row.FindControl("txtPciValorConvIsento"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorUsuAdulto") = "0" Then
                CType(e.Row.FindControl("txtPciValorUsuAdulto"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorUsuCrianca") = "0" Then
                CType(e.Row.FindControl("txtPciValorUsuCrianca"), TextBox).Text = ""
            End If
            If gdvReserva13.DataKeys(e.Row.RowIndex).Item("PciValorUsuIsento") = "0" Then
                CType(e.Row.FindControl("txtPciValorUsuIsento"), TextBox).Text = ""
            End If
        End If
    End Sub

    Protected Sub btnPlanilhaItemGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlanilhaItemGravar.Click
        'Limpando o hddProcessando(Proteção contra duplicação de registro)
        hddProcessando.Value = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialCaldas")
        Else
            objPlanilhaItemDAO = New Turismo.PlanilhaItemDAO("TurismoSocialPiri")
        End If

        objPlanilhaItemVO.pliId = txtPliId.Text
        objPlanilhaItemVO.pliDescricao = txtPliDescricao.Text
        'objPlanilhaCustoVO.modA = cmbModeloA.SelectedValue
        objPlanilhaItemDAO.Acao(objPlanilhaItemVO)

        lista = objPlanilhaItemDAO.consultar
        gdvReserva5.DataSource = lista
        gdvReserva5.DataBind()
        gdvReserva5.SelectedIndex = -1
        txtPliDescricao.Focus()
        btnPlanilhaItemNovo.Enabled = False
        btnPlanilhaItemExcluir.Enabled = False
        txtPliId.Text = 0
        txtPliDescricao.Text = ""
    End Sub

    Protected Sub btnPlanilhaCustoGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlanilhaCustoGravar.Click
        Try
            objPlanilhaCustoVO.resId = hddResId.Value
            objPlanilhaCustoVO.plcQtde = txtPlcQtde.Text
            objPlanilhaCustoVO.plcCapacidade = txtPlcCapacidade.Text
            objPlanilhaCustoVO.plcGuia = txtPlcGuia.Text
            objPlanilhaCustoVO.plcMotorista = txtPlcMotorista.Text
            objPlanilhaCustoVO.plcPercentualConveniado = txtPlcPercentualConveniado.Text
            objPlanilhaCustoVO.plcPercentualUsuario = txtPlcPercentualUsuario.Text
            objPlanilhaCustoVO.plcMargem = txtPlcMargem.Text
            objPlanilhaCustoVO.plcColo = txtPlcColo.Text
            objPlanilhaCustoVO.PlcIdadeIsento = TxtPlcIsento.Text
            objPlanilhaCustoVO.PlcIdadeCrianca = TxtPlcCrianca.Text
            If ckbPlcValorado.Checked Then
                objPlanilhaCustoVO.plcValorado = "S"
            Else
                objPlanilhaCustoVO.plcValorado = "N"
            End If
            If ckbPlcAutorizaConveniado.Checked Then
                objPlanilhaCustoVO.plcAutorizaConveniado = "S"
            Else
                objPlanilhaCustoVO.plcAutorizaConveniado = "N"
            End If
            If ckbPlcAutorizaUsuario.Checked Then
                objPlanilhaCustoVO.plcAutorizaUsuario = "S"
            Else
                objPlanilhaCustoVO.plcAutorizaUsuario = "N"
            End If
            objPlanilhaCustoVO.plcUsuario = User.Identity.Name.Replace("SESC-GO.COM.BR\", "")
            For Each linha As GridViewRow In gdvReserva13.Rows
                objPlanilhaCustoItemVO = New Turismo.PlanilhaCustoItemVO
                objPlanilhaCustoItemVO.resId = hddResId.Value
                objPlanilhaCustoItemVO.pliId = gdvReserva13.DataKeys(linha.RowIndex).Item("PliId").ToString
                objPlanilhaCustoItemVO.pciId = gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString
                objPlanilhaCustoItemVO.pciUsuario = User.Identity.Name.Replace("SESC-GO.COM.BR\", "")
                objPlanilhaCustoItemVO.pciUsuarioData = Format(CDate(Now.ToString), "dd/MM/yyyy")
                If (CType(linha.FindControl("txtPciValorTotal"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorTotal"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorTotal = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorTotal = CType(linha.FindControl("txtPciValorTotal"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorComAdulto = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorComAdulto = CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorComCrianca = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorComCrianca = CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorComIsento = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorComIsento = CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorConvAdulto = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorConvAdulto = CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorConvCrianca = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorConvCrianca = CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorConvIsento = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorConvIsento = CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorUsuAdulto = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorUsuAdulto = CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorUsuCrianca = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorUsuCrianca = CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text
                End If
                If (CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text = "0,00") Then
                    objPlanilhaCustoItemVO.pciValorUsuIsento = "0"
                Else
                    objPlanilhaCustoItemVO.pciValorUsuIsento = CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text
                End If

                If objPlanilhaCustoItemVO.pciId = -1 Then
                    objPlanilhaCustoVO.plcCustoComAdulto = CType(linha.FindControl("lblPciValorComAdulto"), Label).Text
                    objPlanilhaCustoVO.plcCustoComCrianca = CType(linha.FindControl("lblPciValorComCrianca"), Label).Text
                    objPlanilhaCustoVO.plcCustoComIsento = CType(linha.FindControl("lblPciValorComIsento"), Label).Text
                    objPlanilhaCustoVO.plcCustoConvAdulto = CType(linha.FindControl("lblPciValorConvAdulto"), Label).Text
                    objPlanilhaCustoVO.plcCustoConvCrianca = CType(linha.FindControl("lblPciValorConvCrianca"), Label).Text
                    objPlanilhaCustoVO.plcCustoConvIsento = CType(linha.FindControl("lblPciValorConvIsento"), Label).Text
                    objPlanilhaCustoVO.plcCustoUsuAdulto = CType(linha.FindControl("lblPciValorUsuAdulto"), Label).Text
                    objPlanilhaCustoVO.plcCustoUsuCrianca = CType(linha.FindControl("lblPciValorUsuCrianca"), Label).Text
                    objPlanilhaCustoVO.plcCustoUsuIsento = CType(linha.FindControl("lblPciValorUsuIsento"), Label).Text
                End If

                If objPlanilhaCustoItemVO.pciId = -4 Then
                    If (CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaComAdulto = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaComAdulto = CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaComCrianca = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaComCrianca = CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaComIsento = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaComIsento = CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaConvAdulto = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaConvAdulto = CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaConvCrianca = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaConvCrianca = CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaConvIsento = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaConvIsento = CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaUsuAdulto = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaUsuAdulto = CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaUsuCrianca = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaUsuCrianca = CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text
                    End If
                    If (CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text = "") Or (CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text = "0,00") Then
                        objPlanilhaCustoVO.plcVendaUsuIsento = "0"
                    Else
                        objPlanilhaCustoVO.plcVendaUsuIsento = CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text
                    End If
                End If
                If objPlanilhaCustoItemVO.pciId <> -1 And objPlanilhaCustoItemVO.pciId <> -2 And
                   objPlanilhaCustoItemVO.pciId <> -3 And objPlanilhaCustoItemVO.pciId <> -4 Then
                    lista.Add(objPlanilhaCustoItemVO)
                End If
            Next
            If sender Is btnPlanilhaCustoGravar Then
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
                    objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialCaldas")
                Else
                    objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
                    objDefaultDAO = New Turismo.DefaultDAO("TurismoSocialPiri")
                End If
                objPlanilhaCustoDAO.Acao(lista, objPlanilhaCustoVO)
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Planilha salva com sucesso!" + "');", True)
                btnReservaCalculo_Click(sender, e)
                imgBtnCalcularCusto_Click(sender, e)

                'Atualizando os valores dos componentes após o salvamento
                objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
                If objPlanilhaCustoVO.resId > "0" Then
                    hddCapacidade.Value = objPlanilhaCustoVO.plcCapacidade.ToString
                    hddMultiValorado.Value = objPlanilhaCustoVO.plcValorado.ToString
                    hddIdadeColo.Value = objPlanilhaCustoVO.plcColo.ToString
                    hddAutorizaConveniado.Value = objPlanilhaCustoVO.plcAutorizaConveniado.ToString
                    hddAutorizaUsuario.Value = objPlanilhaCustoVO.plcAutorizaUsuario.ToString
                    lblComAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
                    lblComCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
                    lblComIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
                    lblConvAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
                    lblConvCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
                    lblConvIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
                    lblUsuAdulto.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
                    lblUsuCrianca.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
                    lblUsuIsento.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)

                    lblComAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComAdulto.ToString)
                    lblComCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComCrianca.ToString)
                    lblComIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaComIsento.ToString)
                    lblConvAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvAdulto.ToString)
                    lblConvCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvCrianca.ToString)
                    lblConvIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaConvIsento.ToString)
                    lblUsuAdultoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuAdulto.ToString)
                    lblUsuCriancaVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuCrianca.ToString)
                    lblUsuIsentoVlr.Text = FormatNumber(objPlanilhaCustoVO.plcVendaUsuIsento.ToString)

                    'pnlAcomodacao.Visible = False
                    'pnlPlanilha.Visible = True And pnlReserva.Visible
                    'Se for emissivo irá pegar o valor da tabela
                    If hddResCaracteristica.Value = "P" Then
                        hddIdadeColo.Value = objPlanilhaCustoVO.plcColo
                        hddIdadeAdulto.Value = objPlanilhaCustoVO.PlcIdadeCrianca
                        hddIdadeCrianca.Value = objPlanilhaCustoVO.PlcIdadeIsento
                    End If
                End If
                pnlEdicaoIntegrante.Visible = False

            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao gravar dados da planilha de Custo de valores dos integrantes. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnPlanilhaCustoGravar")
        End Try
    End Sub

    Protected Sub imgBtnCalcularCusto_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim varValorTotal As Decimal = 0
        Dim varValorComAdulto As Decimal = 0
        Dim varValorComCrianca As Decimal = 0
        Dim varValorComIsento As Decimal = 0
        Dim varValorConvAdulto As Decimal = 0
        Dim varValorConvCrianca As Decimal = 0
        Dim varValorConvIsento As Decimal = 0
        Dim varValorUsuAdulto As Decimal = 0
        Dim varValorUsuCrianca As Decimal = 0
        Dim varValorUsuIsento As Decimal = 0
        For Each linha As GridViewRow In gdvReserva13.Rows
            If gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString >= "0" Then
                If CType(linha.FindControl("txtPciValorTotal"), TextBox).Text > "0,00" Then
                    varValorTotal += CType(linha.FindControl("txtPciValorTotal"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text > "0,00" Then
                    varValorComAdulto += CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text > "0,00" Then
                    varValorComCrianca += CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text > "0,00" Then
                    varValorComIsento += CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text > "0,00" Then
                    varValorConvAdulto += CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text > "0,00" Then
                    varValorConvCrianca += CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text > "0,00" Then
                    varValorConvIsento += CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text > "0,00" Then
                    varValorUsuAdulto += CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text > "0,00" Then
                    varValorUsuCrianca += CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text
                End If
                If CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text > "0,00" Then
                    varValorUsuIsento += CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text
                End If
            End If
        Next
        For Each linha As GridViewRow In gdvReserva13.Rows
            If gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString = "-1" Then
                CType(linha.FindControl("lblPciValorTotal"), Label).Text = Format(varValorTotal, "###,##0.00")
                CType(linha.FindControl("lblPciValorComAdulto"), Label).Text = Format(varValorComAdulto, "###,##0.00")
                CType(linha.FindControl("lblPciValorComCrianca"), Label).Text = Format(varValorComCrianca, "###,##0.00")
                CType(linha.FindControl("lblPciValorComIsento"), Label).Text = Format(varValorComIsento, "###,##0.00")

                CType(linha.FindControl("lblPciValorConvAdulto"), Label).Text = Format(varValorConvAdulto, "###,##0.00")
                CType(linha.FindControl("lblPciValorConvCrianca"), Label).Text = Format(varValorConvCrianca, "###,##0.00")
                CType(linha.FindControl("lblPciValorConvIsento"), Label).Text = Format(varValorConvIsento, "###,##0.00")

                CType(linha.FindControl("lblPciValorUsuAdulto"), Label).Text = Format(varValorUsuAdulto, "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuCrianca"), Label).Text = Format(varValorUsuCrianca, "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuIsento"), Label).Text = Format(varValorUsuIsento, "###,##0.00")
            End If

            If gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString = "-2" Then
                CType(linha.FindControl("lblPciValorTotal"), Label).Text = Format(varValorTotal * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComAdulto"), Label).Text = Format(varValorComAdulto * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComCrianca"), Label).Text = Format(varValorComCrianca * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComIsento"), Label).Text = Format(varValorComIsento * (txtPlcMargem.Text / 100), "###,##0.00")

                CType(linha.FindControl("lblPciValorConvAdulto"), Label).Text = Format(varValorConvAdulto * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorConvCrianca"), Label).Text = Format(varValorConvCrianca * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorConvIsento"), Label).Text = Format(varValorConvIsento * (txtPlcMargem.Text / 100), "###,##0.00")

                CType(linha.FindControl("lblPciValorUsuAdulto"), Label).Text = Format(varValorUsuAdulto * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuCrianca"), Label).Text = Format(varValorUsuCrianca * (txtPlcMargem.Text / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuIsento"), Label).Text = Format(varValorUsuIsento * (txtPlcMargem.Text / 100), "###,##0.00")
            End If

            If gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString = "-3" Then
                CType(linha.FindControl("lblPciValorTotal"), Label).Text = Format(varValorTotal * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComAdulto"), Label).Text = Format(varValorComAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComCrianca"), Label).Text = Format(varValorComCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorComIsento"), Label).Text = Format(varValorComIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")

                CType(linha.FindControl("lblPciValorConvAdulto"), Label).Text = Format(varValorConvAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorConvCrianca"), Label).Text = Format(varValorConvCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorConvIsento"), Label).Text = Format(varValorConvIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")

                CType(linha.FindControl("lblPciValorUsuAdulto"), Label).Text = Format(varValorUsuAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuCrianca"), Label).Text = Format(varValorUsuCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                CType(linha.FindControl("lblPciValorUsuIsento"), Label).Text = Format(varValorUsuIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
            End If

            If gdvReserva13.DataKeys(linha.RowIndex).Item("PciId").ToString = "-4" Then
                CType(linha.FindControl("txtPciValorTotal"), TextBox).Text = Format(varValorTotal * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                If Not CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text > "0,00" Then
                    CType(linha.FindControl("txtPciValorComAdulto"), TextBox).Text = Format(varValorComAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorComCrianca"), TextBox).Text = Format(varValorComCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorComIsento"), TextBox).Text = Format(varValorComIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")

                    CType(linha.FindControl("txtPciValorConvAdulto"), TextBox).Text = Format(varValorConvAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorConvCrianca"), TextBox).Text = Format(varValorConvCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorConvIsento"), TextBox).Text = Format(varValorConvIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")

                    CType(linha.FindControl("txtPciValorUsuAdulto"), TextBox).Text = Format(varValorUsuAdulto * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorUsuCrianca"), TextBox).Text = Format(varValorUsuCrianca * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                    CType(linha.FindControl("txtPciValorUsuIsento"), TextBox).Text = Format(varValorUsuIsento * ((txtPlcMargem.Text + 100) / 100), "###,##0.00")
                End If
            End If
        Next
    End Sub

    Protected Sub gdvReserva13_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva13.SelectedIndexChanged
        If Not txtPlcQtde.Text > "0" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Quantidade precisa ser maior que 0." + "');", True)
        ElseIf Not CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text > "0,00" And
           Not CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text > "0,00" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Valor total ou do comerciário adulto precisa ser maior que 0,00." + "');", True)
        Else
            If CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text > "0,00" Then
                CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text =
                   Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                CType(sender.SelectedRow.FindControl("txtPciValorComCrianca"), TextBox).Text =
                   Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                CType(sender.SelectedRow.FindControl("txtPciValorComIsento"), TextBox).Text =
                   Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")

                If txtPlcPercentualConveniado.Text > "0" Then
                    CType(sender.SelectedRow.FindControl("txtPciValorConvAdulto"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvCrianca"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvIsento"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                Else
                    CType(sender.SelectedRow.FindControl("txtPciValorConvAdulto"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvCrianca"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvIsento"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                End If
                If txtPlcPercentualUsuario.Text > "0" Then
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuAdulto"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuCrianca"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuIsento"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                Else
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuAdulto"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuCrianca"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuIsento"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorTotal"), TextBox).Text / txtPlcQtde.Text, "###,##0.00")
                End If
            Else
                If txtPlcPercentualConveniado.Text > "0" Then
                    CType(sender.SelectedRow.FindControl("txtPciValorConvAdulto"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvCrianca"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComCrianca"), TextBox).Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvIsento"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComIsento"), TextBox).Text) * ((txtPlcPercentualConveniado.Text + 100) / 100), "###,##0.00")
                Else
                    CType(sender.SelectedRow.FindControl("txtPciValorConvAdulto"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvCrianca"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComCrianca"), TextBox).Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorConvIsento"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComIsento"), TextBox).Text, "###,##0.00")
                End If
                If txtPlcPercentualUsuario.Text > "0" Then
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuAdulto"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuCrianca"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComCrianca"), TextBox).Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuIsento"), TextBox).Text =
                       Format((CType(sender.SelectedRow.FindControl("txtPciValorComIsento"), TextBox).Text) * ((txtPlcPercentualUsuario.Text + 100) / 100), "###,##0.00")
                Else
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuAdulto"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComAdulto"), TextBox).Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuCrianca"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComCrianca"), TextBox).Text, "###,##0.00")
                    CType(sender.SelectedRow.FindControl("txtPciValorUsuIsento"), TextBox).Text =
                       Format(CType(sender.SelectedRow.FindControl("txtPciValorComIsento"), TextBox).Text, "###,##0.00")
                End If
            End If
            imgBtnCalcularCusto_Click(sender, e)
        End If
    End Sub

    Protected Sub lnkhdriIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.ID = "lnkhdriIntegrante" Or sender.ID = "imgiIntegrante" Then
            If btnCaixa.CommandName = "intNomeTitular, i.IntDtNascimento, intNome, i.intDataIni, i.intDataFim" Then
                btnCaixa.CommandName = "i.IntNome desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "intNomeTitular, i.IntDtNascimento, intNome, i.intDataIni, i.intDataFim"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriCategoria" Or sender.ID = "imgiCategoria" Then
            If btnCaixa.CommandName = "CatDescricao" Then
                btnCaixa.CommandName = "CatDescricao desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCategoria"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "CatDescricao"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCategoria"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriAcomodacao" Or sender.ID = "imgiAcomodacao" Then
            If btnCaixa.CommandName = "AcmDescricao" Then
                btnCaixa.CommandName = "AcmDescricao desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiAcomodacao"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "AcmDescricao"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiAcomodacao"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriCheckInIntegrante" Or sender.ID = "imgiCheckInIntegrante" Then
            If btnCaixa.CommandName = "h.HosDataIniSol, h.HosDataFimSol" Then
                btnCaixa.CommandName = "h.HosDataIniSol desc, h.HosDataFimSol desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCheckInIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "h.HosDataIniSol, h.HosDataFimSol"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCheckInIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriCheckOutIntegrante" Or sender.ID = "imgiCheckOutIntegrante" Then
            If btnCaixa.CommandName = "h.HosDataFimSol, h.HosDataIniSol" Then
                btnCaixa.CommandName = "h.HosDataFimSol desc, h.HosDataIniSol desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCheckOutIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "h.HosDataFimSol, h.HosDataIniSol"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiCheckOutIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriVlrDevidoIntegrante" Or sender.ID = "imgiVlrDevidoIntegrante" Then
            If btnCaixa.CommandName = "hosValorDevido" Then
                btnCaixa.CommandName = "hosValorDevido desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiVlrDevidoIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "hosValorDevido"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiVlrDevidoIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriVlrPagoIntegrante" Or sender.ID = "imgiVlrPagoIntegrante" Then
            If btnCaixa.CommandName = "hosValorPago" Then
                btnCaixa.CommandName = "hosValorPago desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiVlrPagoIntegrante"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "hosValorPago"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiVlrPagoIntegrante"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        ElseIf sender.ID = "lnkhdriServidor" Or sender.ID = "imgiServidor" Then
            If btnCaixa.CommandName = "i.intUsuario" Then
                btnCaixa.CommandName = "i.intUsuario desc"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiServidor"), ImageButton).ImageUrl = "~/images/ZA.png"
            Else
                btnCaixa.CommandName = "i.intUsuario"
                ListaIntegranteViaResId()
                CType(gdvReserva9.HeaderRow.FindControl("imgiServidor"), ImageButton).ImageUrl = "~/images/AZ.png"
            End If
        End If
        CType(gdvReserva9.HeaderRow.FindControl("imgiIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdriIntegrante" Or sender.ID = "imgiIntegrante")
        CType(gdvReserva9.HeaderRow.FindControl("imgiCategoria"), ImageButton).Visible = (sender.ID = "lnkhdriCategoria" Or sender.ID = "imgiCategoria")
        CType(gdvReserva9.HeaderRow.FindControl("imgiAcomodacao"), ImageButton).Visible = (sender.ID = "lnkhdriAcomodacao" Or sender.ID = "imgiAcomodacao")
        CType(gdvReserva9.HeaderRow.FindControl("imgiCheckInIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdriCheckInIntegrante" Or sender.ID = "imgiCheckInIntegrante")
        CType(gdvReserva9.HeaderRow.FindControl("imgiCheckOutIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdriCheckOutIntegrante" Or sender.ID = "imgiCheckOutIntegrante")
        CType(gdvReserva9.HeaderRow.FindControl("imgiVlrDevidoIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdriVlrDevidoIntegrante" Or sender.ID = "imgiVlrDevidoIntegrante")
        CType(gdvReserva9.HeaderRow.FindControl("imgiVlrPagoIntegrante"), ImageButton).Visible = (sender.ID = "lnkhdriVlrPagoIntegrante" Or sender.ID = "imgiVlrPagoIntegrante")
        CType(gdvReserva9.HeaderRow.FindControl("imgiServidor"), ImageButton).Visible = (sender.ID = "lnkhdriServidor" Or sender.ID = "imgiServidor")
    End Sub

    Protected Sub imgiIntegrante_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        lnkhdriIntegrante_Click(sender, e)
    End Sub

    Protected Sub gdvReserva4_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva4.DataBinding
        lblFinanceiro.Text = "Pagamentos"
    End Sub

    Protected Sub gdvReserva2_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva2.DataBinding
        lblAcomodacao.Text = "Acomodações"
    End Sub

    Protected Sub gdvReserva3_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva3.DataBinding
        lblIntegrante.Text = "Integrantes"
    End Sub

    Protected Sub gdvReserva1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva1.DataBinding
        lblReserva.Text = "Reservas"
    End Sub

    Protected Sub imgBtnAlterarPagamento_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnAlterarPagamento.Click, imgBtnAlterarCategoria.Click, imgBtnAlterarMemorando.Click, imgBtnAlterarRefeicao.Click
        btnIntegranteGravar_Click(sender, e)
    End Sub

    Protected Sub gdvRessarcimento_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvRessarcimento.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = FormatNumber(CDec(e.Row.Cells(0).Text), 2)
        End If
    End Sub

    Protected Sub cmbOrgId_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrgId.SelectedIndexChanged, cmbResCatId.SelectedIndexChanged
        If cmbOrgId.SelectedItem.Text.Trim = "Presidência" Then
            cmbReservaHoraSaida.Text = "12"
            ckbRefeicao.Items.Item(0).Selected = False
            ckbRefeicao.Items.Item(1).Selected = True
            ckbRefeicao.Items.Item(2).Selected = True
            ckbRefeicao.Items.Item(3).Selected = False
            'Se for presidencia e se não tiver nenhum número de telefone cadastrado... define valores padrão
            If txtResFax.Text.Trim.Length <= 6 _
               And txtResFoneComercial.Text.Trim.Length < 6 _
               And txtResFoneResidencial.Text.Trim.Length < 6 _
               And txtResCelular.Text.Trim.Length < 6 Then
                txtResContato.Text = "0"
                cmbResCatId.SelectedValue = 4
                cmbResCatCobranca.SelectedValue = 4
                txtResFoneComercial.Text = "(62)00000-0000"
                cmbResEmissor.SelectedValue = 1
            End If
            If txtResLogradouro.Text.Trim.Length = 0 Then
                txtResLogradouro.Text = "P"
            End If
            If txtResNumero.Text.Trim.Length = 0 Then
                txtResNumero.Text = "0"
            End If
            If txtResQuadra.Text.Trim.Length = 0 Then
                txtResQuadra.Text = "0"
            End If
            If txtResLote.Text.Trim.Length = 0 Then
                txtResLote.Text = "0"
            End If
            If txtResComplemento.Text.Trim.Length = 0 Then
                txtResComplemento.Text = "P"
            End If
            If txtResBairro.Text.Trim.Length = 0 Then
                txtResBairro.Text = "P"
            End If
            If txtResCep.Text.Trim.Length = 0 Then
                txtResCep.Text = "74000000"
            End If
            If txtResCidade.Text.Trim.Length = 0 Then
                txtResCidade.Text = "P"
            End If
            If txtResEmail.Text.Trim.Length = 0 Then
                txtResEmail.Text = "federacao@federacao.com.br"
            End If
        Else
            cmbReservaHoraSaida.Text = "12"
            ckbRefeicao.Items.Item(0).Selected = True
            ckbRefeicao.Items.Item(1).Selected = True
            ckbRefeicao.Items.Item(2).Selected = False
            ckbRefeicao.Items.Item(3).Selected = False
        End If
        cmbOrgId.Focus()
    End Sub

    Protected Sub imgBtnResMatricula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgBtnResMatricula.Click
        Try
            Dim ConexaoBanco As String = ""
            'Verificando se o valores de matrícula e cpf são númericos
            txtResMatricula.Text = txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Replace("/", "").Replace("\", "")
            If txtResMatricula.Text.Trim.Length > 0 Then
                If Not IsNumeric(txtResMatricula.Text.ToString.Replace(" ", "")) Then
                    ScriptManager.RegisterStartupScript(updPnlReserva, updPnlReserva.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela deve ser composta de apenas números.');", True)
                    txtResMatricula.Text = ""
                    txtResMatricula.Focus()
                    Exit Try
                ElseIf txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length < 11 Then
                    ScriptManager.RegisterStartupScript(updPnlReserva, updPnlReserva.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela precisa conter no mínimo 11 caracteres.');", True)
                    txtResMatricula.Text = ""
                    txtResMatricula.Focus()
                    Exit Try
                End If
            End If

            'Se limpar a matrícula o campo estado será carregado novamente com todos os estados.
            If txtResMatricula.Text.Length = 0 Then
                cmbEstId.Items.Clear()
                cmbEstId.Enabled = True
                'CarregaEstado()
                cmbEstId.Items.Clear()
                cmbEstId.DataValueField = ("estId")
                cmbEstId.DataTextField = ("estDescricao")
                CarregaTodosEstadosIntegrante("R")  'Session.Item("Estados") 'listaEstadoAuxiliar
                cmbEstId.DataBind()
                Exit Try
            End If

            If txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length > 0 Then
                txtResMatricula.Text = Format(CLng(txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "")), "0000 000000 0")
            End If

            If (Len(txtResMatricula.Text.Trim) = 13) Or (Len(txtResMatricula.Text.Trim) = 14) Then
                'Passara apenas o valor número da carteirinha, caso copia e cole com os caracteres abaixo
                If Len(txtResMatricula.Text.Trim) = 14 Then
                    txtResMatricula.Text = Format(CLng(Mid(txtResMatricula.Text.Trim.Replace(" ", ""), 2, 11)), "0000 000000 0")
                Else
                    txtResMatricula.Text = Format(CLng(txtResMatricula.Text.Trim.Replace(" ", "")), "0000 000000 0")
                End If

                objUopVO = New Turismo.UopVO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objUopDAO = New Turismo.UopDAO("TurismoSocialCaldas")
                    ConexaoBanco = "TurismoSocialCaldas"
                Else
                    objUopDAO = New Turismo.UopDAO("TurismoSocialPiri")
                    ConexaoBanco = "TurismoSocialPiri"
                End If
                txtResContato.Text = ""
                'Irá buscar no TbLeReserva o Contato de emergencia com base na matrícula e inserir no campo devido.
                objClienteVO = New CentralAtendimento.ClienteVO
                objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)

                objClienteVO = objClienteDAO.ConsultaContatoEmergencia(Mid((txtResMatricula.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Trim), 1, 10), ConexaoBanco)
                txtResContato.Text = objClienteVO.ContatoEmergencia
                Dim ContatoEmergencia = objClienteVO.ContatoEmergencia

                'Primeira consula pela matricula
                objUopVO = objUopDAO.consultar(Mid(txtResMatricula.Text.Trim, 1, 4))
                objClienteVO = New CentralAtendimento.ClienteVO
                objClienteVO.matricula = txtResMatricula.Text.Trim
                objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)
                'objClienteVO = objClienteDAO.consultarClientelaeEndereco()

                'Inserido por Washington 22-01-2014=======================================
                If Not IsNothing(objUopVO) Then ' Localizou o estado de origem da matricula
                    If objUopVO.estSigla = "GO" Then ' Se for Goiás, busca na Central de Atendimento
                        objClienteVO = objClienteDAO.consultarClientelaeEndereco()
                    Else
                        'objClienteVO = Nothing
                        'objClienteVO.matricula = ""
                    End If
                Else ' Não localizou o estado de origem da matricula
                    objClienteVO = Nothing
                End If

                'Desabilitar(pnlEdicaoIntegrante)
                'Desabilitar(pnlIntegranteEmissivo)

                If IsNothing(objClienteVO) Then ' Matrícula sem estado de origem localizado
                    If cmbEstId.Items.Count <> cmbIntEstIdSemCA.Items.Count Then
                        cmbEstId.Items.Clear()
                        Dim It As ListItem
                        For Each It In cmbIntEstIdSemCA.Items
                            cmbEstId.Items.Add(New ListItem(It.Text, It.Value))
                        Next
                        cmbResCidade.Items.Clear()
                        cmbEstId_SelectedIndexChanged(sender, e)
                        txtResCPF.Focus()
                    End If
                Else
                    cmbEstId.Items.Clear()
                    CarregaEstado()
                End If
                '=================================================================

                'If objClienteVO.matricula = "VAZIO" Then ' Não existe a matricula na Central de Atendimento
                If IsNothing(objClienteVO) Then ' Não existe a matricula na Central de Atendimento
                    'Limpando todos os campos
                    'LimpaCamposResponsavel()
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula inexistente na Central de Atendimentos.\n\nConfira o número digitado e continue com a inserção dos dados para efetivar a reserva." + "');", True)
                    txtResMatricula.Enabled = True
                    txtResContato.Text = ContatoEmergencia
                    'Não encontrou na CA irá ativar os campos Estado e Cidade
                    cmbEstId.Enabled = True

                    cmbResCidade.Enabled = True
                    txtResCidade.ReadOnly = False
                    'txtResNome.Focus()
                    txtResCPF.Focus()
                ElseIf Not IsNothing(objClienteVO.nome) Then ' Matrícula encontrada na Central de Atendimento
                    If CDate(objClienteVO.dtVencimento) < Now.Date Then
                        LimpaCamposResponsavel()
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula vencida. Dirija-se a Central de Atendimento." + "');", True)
                        txtResMatricula.Enabled = True
                        txtResMatricula.Text = ""
                        'Se a encontrar na CA irá desativar os campos Estado e Cidade
                        cmbEstId.Enabled = False
                        cmbResCidade.Enabled = False
                        txtResCidade.ReadOnly = False
                        txtResMatricula.Focus()
                    Else
                        txtResNome.Text = Mid(objClienteVO.nome.Trim, 1, 100)
                        'txtResCPF.Text = objClienteVO.cpf
                        txtResCPF.Text = Mid(objClienteVO.cpf, 1, 3) & " " & Mid(objClienteVO.cpf, 4, 3) & " " & Mid(objClienteVO.cpf, 7, 3) & " " & Mid(objClienteVO.cpf, 10, 2)

                        'Carregando RG do cliente
                        cmbResDocIdentificacao.SelectedValue = "RG"

                        'Se a encontrar na CA irá desativar os campos Estado e Cidade
                        cmbEstId.Enabled = False
                        cmbResCidade.Enabled = False
                        txtResCidade.ReadOnly = True

                        If objClienteVO.rg Is Nothing Then
                            txtResDocIdentificacao.Text = ""
                        Else
                            txtResDocIdentificacao.Text = objClienteVO.rg.Trim + "/" + objClienteVO.emissorRg
                        End If

                        txtResDtNascimento.Text = Format(CDate(objClienteVO.dtNascimento), "dd/MM/yyyy")
                        If objClienteVO.sexo = "0" Then
                            cmbResSexo.Text = "M"
                        Else
                            cmbResSexo.Text = "F"
                        End If
                        Select Case objClienteVO.categoria
                            Case 1, 3, 4, 5, 9, 11, 18, 22, 28
                                cmbResCatId.Text = "1"
                            Case 2, 10, 14, 16, 19, 23, 26
                                cmbResCatId.Text = "2"
                            Case 7, 8, 12, 13, 17, 20, 25, 27
                                cmbResCatId.Text = "3"
                            Case Else
                                cmbResCatId.Text = "4"
                        End Select
                        Try
                            cmbResSalario.Text = CInt(objClienteVO.cdClassif.Trim)
                        Catch ex As Exception
                            cmbResSalario.Text = 1
                        End Try
                        Try
                            cmbResEscolaridade.Text = objClienteVO.cdNivel
                        Catch ex As Exception
                            cmbResEscolaridade.Text = 1
                        End Try
                        Try
                            cmbResEstadoCivil.Text = objClienteVO.estCivil
                        Catch ex As Exception
                            cmbResEstadoCivil.Text = 0
                        End Try

                        If txtResContato.Text.Trim = "" Then
                            txtResContato.Text = objClienteVO.telefone.Trim
                        End If

                        If objClienteVO.logradouro Is Nothing Then
                            txtResLogradouro.Text = ""
                        Else
                            txtResLogradouro.Text = Mid(objClienteVO.logradouro.Trim, 1, 40)
                        End If

                        txtResNumero.Text = objClienteVO.numero

                        If objClienteVO.complemento Is Nothing Then
                            txtResComplemento.Text = ""
                        Else
                            txtResComplemento.Text = Mid(objClienteVO.complemento.Trim, 1, 40)
                        End If

                        txtResBairro.Text = objClienteVO.bairro
                        txtResCep.Text = objClienteVO.cep
                        cmbEstId.Text = objUopVO.estId

                        If objClienteVO.municipio.Trim = "GOIANIA" Then
                            cmbResCidade.SelectedValue = "GOIÂNIA"
                            cmbResCidade.Enabled = False
                        Else
                            Try
                                cmbResCidade.SelectedValue = objClienteVO.municipio.Trim
                                cmbResCidade.Enabled = False
                            Catch ex As Exception
                                txtResCidade.Visible = False
                                cmbResCidade.Visible = True
                                cmbResCidade.Enabled = True
                                cmbResCidade.SelectedIndex = 0
                                Mensagem("Selecione o nome da cidade antes de salvar.")
                            End Try
                        End If
                        'txtResCidade.Visible = True
                        'txtResCidade.Text = Mid(objClienteVO.municipio.Trim, 1, 40)
                        'cmbResCidade.Visible = False
                        txtResEmail.Text = Mid(objClienteVO.email.Trim, 1, 40)
                        txtResCPF.Focus()
                    End If
                Else
                    'LimpaCamposResponsavel()
                    'Quando for comerciário de outro estado, irá excluir a opção de Goiás do combo de seleção
                    cmbEstId.Items.Remove(cmbIntEstId.Items.FindByText(" GOIÁS"))
                    cmbEstId.Text = objUopVO.estId
                    cmbEstId.Enabled = True
                    cmbResCidade.Enabled = True
                    cmbEstId_SelectedIndexChanged(sender, e)
                    cmbResCatId.SelectedValue = 1
                    txtResCPF.Focus()
                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula inválida." + "');", True)
                txtResMatricula.Text = ""
                txtResMatricula.Focus()
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
                     "&Erro=Erro ao carregar os dados do responsável baseado na matrícula digitada. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: imgBtnResMatricula - Matricula:" & txtResMatricula.Text & "")
        End Try
    End Sub

    Protected Sub btnIntMatricula_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIntMatricula.Click
        Try
            txtIntMatricula.Text = txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Replace("/", "").Replace("\", "")
            'Verificando se a matrícula e o cpf são númericos
            If txtIntMatricula.Text.Trim.Length > 0 Then
                If Not IsNumeric(txtIntMatricula.Text.ToString.Replace(" ", "")) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela deve ser composta de apenas números.');", True)
                    txtIntMatricula.Text = ""
                    txtIntMatricula.Focus()
                    Exit Try
                End If
            End If

            If txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length < 11 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! A Matrícula apresentada não é válida, ela precisa conter no mínimo 11 caracteres.');", True)
                txtIntMatricula.Text = ""
                txtIntMatricula.Focus()
                Exit Try
            End If

            If txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "").Length > 0 Then
                txtIntMatricula.Text = Format(CLng(txtIntMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "")), "0000 000000 0")
            End If

            '(Linha Original - ver com o Haas e depois apagá-la - Lembrar de voltar no DBT a SPManutençãoIntegranteAux para o SQL-CTL)
            'If hddIntId.Value = 0 And Len(txtIntCPF.Text.Replace(" ", "").Trim) = 0 And _ 
            'Situação: Insere o CPF e busca os dados, altera a categoria e as linhas a seguir obriga validar a matricula após a inserção.
            Dim CPFAnt As String = ""
            If txtIntCPF.Text.Replace(" ", "").Trim.Length > 0 Then
                CPFAnt = txtIntCPF.Text.Replace(" ", "")
                txtIntCPF.Text = ""
            End If

            'Inicio do processo de validação da mantrícula
            If (Len(txtIntCPF.Text.Replace(" ", "").Trim) = 0 Or hddIntId.Value <> 0) And
              ((Len(txtIntMatricula.Text.Trim.Replace(" ", "")) = 11) Or (Len(txtIntMatricula.Text.Trim.Replace(" ", "")) = 12)) Then
                If Len(txtIntMatricula.Text.Trim.Replace(" ", "")) = 12 Then
                    txtIntMatricula.Text = Format(CLng(Mid(txtIntMatricula.Text.Trim.Replace(" ", ""), 2, 11)), "0000 000000 0")
                ElseIf Len(txtIntMatricula.Text.Trim.Replace(" ", "")) = 11 Then
                    txtIntMatricula.Text = Format(CLng(Mid(txtIntMatricula.Text.Trim.Replace(" ", ""), 1, 11)), "0000 000000 0")
                End If

                'Formatando o cpf
                txtIntCPF.Text = Mid(CPFAnt, 1, 3) & " " & Mid(CPFAnt, 4, 3) & " " & Mid(CPFAnt, 7, 3) & " " & Mid(CPFAnt, 10, 2)
                objUopVO = New Turismo.UopVO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objUopDAO = New Turismo.UopDAO("TurismoSocialCaldas")
                Else
                    objUopDAO = New Turismo.UopDAO("TurismoSocialPiri")
                End If
                objUopVO = objUopDAO.consultar(Mid(txtIntMatricula.Text.Trim, 1, 4))

                objClienteVO = New CentralAtendimento.ClienteVO
                objClienteVO.matricula = txtIntMatricula.Text.Trim
                objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)

                If Not IsNothing(objUopVO) Then ' Localizou o estado de origem da matricula
                    If objUopVO.estSigla = "GO" Then ' Se for Goiás, busca na Central de Atendimento
                        objClienteVO = objClienteDAO.consultarClientelaeEndereco()
                        lblIntVencMatricula.Text = "Venc.: " & Format(CDate(objClienteVO.dtVencimento), "MM/yyyy") 'DTVENCTO
                    Else
                        lblIntVencMatricula.Text = ""
                        'objClienteVO = Nothing
                    End If
                Else ' Não localizou o estado de origem da matricula
                    objClienteVO = Nothing
                End If

                Desabilitar(pnlEdicaoIntegrante)
                'Desabilitar(pnlIntegranteEmissivo)

                If IsNothing(objClienteVO) Then ' Matrícula sem estado de origem localizado
                    If cmbIntEstId.Items.Count <> cmbIntEstIdSemCA.Items.Count Then
                        cmbIntEstId.Items.Clear()
                        Dim It As ListItem
                        For Each It In cmbIntEstIdSemCA.Items
                            cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                        Next
                    End If

                    txtIntCPF.Enabled = True
                    txtIntNome.Enabled = True
                    txtIntNascimento.Enabled = True
                    cmbIntSexo.Enabled = True
                    cmbIntFormaPagamento.Enabled = True
                    txtIntMemorando.Enabled = True
                    cmbIntEmissor.Enabled = True
                    cmbIntCatId.Enabled = True
                    cmbIntCatId.Items.Clear()
                    cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                    cmbIntCatId.Items.Insert(1, New ListItem("Dependente", "2"))
                    'cmbIntCatId.Items.Insert(0, New ListItem("Conveniado", "3"))
                    cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                    cmbIntEstId.DataBind()
                    cmbIntCatId.SelectedValue = "1"
                    cmbIntCatCobranca.SelectedValue = "1"

                    cmbIntCatCobranca.Enabled = True
                    ckbRefeicao.Enabled = True
                    cmbIntEstId.Enabled = True
                    cmbIntEstId_SelectedIndexChanged(sender, e)
                    cmbIntSalario.Enabled = True
                    cmbIntEscolaridade.Enabled = True
                    cmbIntEstadoCivil.Enabled = True
                    cmbAcomodacaoCobranca.Enabled = True
                    txtIntCPF.Enabled = True
                    txtIntRG.Enabled = True
                    cmbIntRG.Enabled = True
                    cmbIntVinculoId.Enabled = True
                    txtIntFoneResponsavelExc.Enabled = True
                    txtIntLocalTrabalhoResponsavelExc.Enabled = True
                    txtIntEnderecoResponsavelExc.Enabled = True
                    txtIntBairroResponsavelExc.Enabled = True
                    txtIntPoltronaExc.Enabled = True
                    ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                    txtIntApartamentoExc.Enabled = True
                    cmbPratoRapido.Enabled = True
                    txtIntCPF.Focus()
                    ScriptManager.RegisterStartupScript(txtIntCPF, txtIntCPF.GetType(), Guid.NewGuid().ToString(), "$get('" + txtIntCPF.ClientID + "').focus();", True)
                    btnIntegranteGravar.Visible = True
                    btnIntegranteExcluir.Visible = False
                    imgBtnAlterarCategoria.Visible = False
                    imgBtnAlterarMemorando.Visible = False
                    imgBtnAlterarPagamento.Visible = False
                    imgBtnAlterarRefeicao.Visible = False
                ElseIf objClienteVO.matricula = "VAZIO" Then ' Não existe a matricula na Central de Atendimento
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula inexistente na Central de Atendimentos." + "');", True)
                    txtIntMatricula.Enabled = True
                    'txtIntMatricula.Text = ""
                    'txtIntMatricula.Focus()

                    cmbIntEstId.DataSource = Nothing
                    cmbIntEstId.DataBind()
                    If cmbIntEstId.Items.Count <> cmbIntEstIdSemCA.Items.Count Then
                        cmbIntEstId.Items.Clear()
                        Dim It As ListItem
                        For Each It In cmbIntEstIdSemCA.Items
                            cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                        Next
                    End If

                    cmbIntEstId_SelectedIndexChanged(sender, e)
                    btnIntegranteGravar.Visible = True
                    btnIntegranteExcluir.Visible = True
                    imgBtnAlterarCategoria.Visible = True
                    imgBtnAlterarMemorando.Visible = True
                    imgBtnAlterarPagamento.Visible = True
                    imgBtnAlterarRefeicao.Visible = True
                    Habilitar(pnlEdicaoIntegrante)
                    'ScriptManager.RegisterStartupScript(txtIntMatricula, txtIntMatricula.GetType(), Guid.NewGuid().ToString(), "$get('" + txtIntMatricula.ClientID + "').focus();", True)
                    txtIntCPF.Focus()
                ElseIf Not IsNothing(objClienteVO.nome) Then ' Matrícula encontrada na Central de Atendimento
                    If CDate(objClienteVO.dtVencimento) < Now.Date Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula vencida. Dirija-se a Central de Atendimento." + "');", True)
                        txtIntMatricula.Enabled = True
                        txtIntMatricula.Text = ""
                        'txtIntMatricula.Focus()
                        ScriptManager.RegisterStartupScript(txtIntMatricula, txtIntMatricula.GetType(), Guid.NewGuid().ToString(), "$get('" + txtIntMatricula.ClientID + "').focus();", True)
                    Else
                        txtIntNome.Text = objClienteVO.nome.Trim
                        txtIntNascimento.Text = Format(CDate(objClienteVO.dtNascimento), "dd/MM/yyyy")
                        If objClienteVO.sexo = "0" Then
                            cmbIntSexo.Text = "M"
                        Else
                            cmbIntSexo.Text = "F"
                        End If
                        'If cmbIntEstId.Items.Count <> cmbEstId.Items.Count Then
                        '    cmbIntEstId.Items.Clear()
                        '    Dim It As ListItem
                        '    For Each It In cmbEstId.Items
                        '        cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                        '    Next
                        'End If
                        cmbIntFormaPagamento.Enabled = True
                        txtIntMemorando.Enabled = True
                        cmbIntEmissor.Enabled = True
                        cmbIntCatId.Items.Clear()
                        cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                        cmbIntCatId.Items.Insert(1, New ListItem("Dependente", "2"))
                        cmbIntCatId.Items.Insert(2, New ListItem("Conveniado", "3"))
                        cmbIntCatId.Items.Insert(3, New ListItem("Usuário", "4"))

                        Select Case objClienteVO.categoria
                            Case 1, 3, 4, 5, 9, 11, 18, 22, 28
                                cmbIntCatId.Text = "1"
                            Case 2, 10, 14, 16, 19, 23, 26
                                cmbIntCatId.Text = "2"
                            Case 7, 8, 12, 13, 17, 20, 25, 27
                                cmbIntCatId.Text = "3"
                            Case Else
                                cmbIntCatId.Text = "4"
                        End Select
                        Select Case objClienteVO.categoria
                            Case 1, 3, 4, 5, 9, 11, 18, 22, 28
                                cmbIntCatCobranca.Text = "1"
                            Case 2, 10, 14, 16, 19, 23, 26
                                cmbIntCatCobranca.Text = "1"
                            Case 7, 8, 12, 13, 17, 20, 25, 27
                                cmbIntCatCobranca.Text = "3"
                            Case Else
                                cmbIntCatCobranca.Text = "4"
                        End Select
                        cmbIntCatCobranca.Enabled = True
                        'cmbIntEstId.Text = objUopVO.estId
                        cmbIntEstId.SelectedValue = objUopVO.estId
                        cmbIntEstId_SelectedIndexChanged(sender, e)
                        txtIntCidade.Visible = True
                        If objClienteVO.municipio.Trim = "GOIANIA" Then
                            cmbIntCidade.SelectedValue = "GOIÂNIA"
                            cmbIntCidade.Visible = False
                        Else
                            Try
                                cmbIntCidade.SelectedValue = objClienteVO.municipio.Trim
                                cmbIntCidade.Visible = False
                            Catch ex As Exception
                                txtIntCidade.Visible = False
                                cmbIntCidade.Visible = True
                                cmbIntCidade.Enabled = True
                                cmbIntCidade.SelectedIndex = 0
                                Mensagem("Selecione o nome da cidade antes de salvar.")
                            End Try
                        End If
                        txtIntCidade.Text = objClienteVO.municipio.Trim
                        cmbAcomodacaoCobranca.Enabled = True
                        txtIntRG.Enabled = True
                        cmbIntRG.Enabled = True
                        cmbIntVinculoId.Enabled = True
                        txtIntFoneResponsavelExc.Enabled = True
                        txtIntLocalTrabalhoResponsavelExc.Enabled = True
                        txtIntEnderecoResponsavelExc.Enabled = True
                        txtIntBairroResponsavelExc.Enabled = True
                        txtIntPoltronaExc.Enabled = True
                        ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                        txtIntApartamentoExc.Enabled = True
                        cmbPratoRapido.Enabled = True
                        ckbRefeicao.Enabled = True
                        Try
                            cmbIntSalario.Text = CInt(objClienteVO.cdClassif.Trim)
                        Catch ex As Exception
                            cmbIntSalario.Text = 1
                        End Try
                        Try
                            cmbIntEscolaridade.Text = objClienteVO.cdNivel
                        Catch ex As Exception
                            cmbIntEscolaridade.Text = 1
                        End Try
                        Try
                            cmbIntEstadoCivil.Text = objClienteVO.estCivil
                        Catch ex As Exception
                            cmbIntEstadoCivil.Text = 0
                        End Try
                        Try
                            'Mascarando o CPF quando encontrado
                            If objClienteVO.cpf.Trim.Length > 0 Then
                                txtIntCPF.Text = Format(CLng(objClienteVO.cpf.Trim.Replace(" ", "").Replace("-", "").Replace("_", "").Replace(".", "")), "000 000 000 00")
                            End If
                            'txtIntCPF.Text = objClienteVO.cpf.Trim
                            txtIntCPF.Enabled = (Len(txtIntCPF.Text.Replace(" ", "")) = 0)
                        Catch ex As Exception
                            txtIntCPF.Enabled = True
                            txtIntCPF.Text = ""
                        End Try
                        Try
                            txtIntRG.Text = objClienteVO.rg.Trim + "/" + objClienteVO.emissorRg
                        Catch ex As Exception
                            txtIntRG.Text = ""
                        End Try
                        cmbIntRG.Text = "RG"
                        btnIntegranteGravar.Visible = True
                        btnIntegranteExcluir.Visible = False
                        imgBtnAlterarCategoria.Visible = False
                        imgBtnAlterarMemorando.Visible = False
                        imgBtnAlterarPagamento.Visible = False
                        imgBtnAlterarRefeicao.Visible = False
                    End If
                Else ' Matrícula com estado de origem localizada
                    'If cmbIntEstId.Items.Count <> cmbEstId.Items.Count Then
                    '    cmbIntEstId.Items.Clear()
                    '    Dim It As ListItem
                    '    For Each It In cmbEstId.Items
                    '        cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                    '    Next
                    'End If
                    'Quando for comerciário de outro estado, irá excluir a opção de Goiás do combo de seleção
                    cmbIntEstId.Items.Remove(cmbIntEstId.Items.FindByText(" GOIÁS"))
                    txtIntCPF.Enabled = True
                    txtIntNome.Enabled = True
                    txtIntNascimento.Enabled = True
                    cmbIntSexo.Enabled = True
                    cmbIntFormaPagamento.Enabled = True
                    txtIntMemorando.Enabled = True
                    cmbIntEmissor.Enabled = True
                    cmbIntCatId.Enabled = True
                    cmbIntCatId.Items.Clear()
                    cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                    cmbIntCatId.Items.Insert(1, New ListItem("Dependente", "2"))
                    'cmbIntCatId.Items.Insert(0, New ListItem("Conveniado", "3"))
                    cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                    cmbIntCatId.SelectedValue = 1 'Irá setar o combo como comerciário automaticamente
                    cmbIntCatCobranca.Enabled = True
                    ckbRefeicao.Enabled = True
                    cmbIntEstId.Text = objUopVO.estId
                    cmbIntEstId_SelectedIndexChanged(sender, e)
                    'Campo abilitado por Washington em 07-12-2016 (Os números de matrículas de um estado começaram a entram em outro estado, então a pedido da Pollyana o campo foi liberado para seleção)
                    cmbIntEstId.Enabled = True
                    cmbIntCidade.Enabled = True
                    cmbIntSalario.Enabled = True
                    cmbIntEscolaridade.Enabled = True
                    cmbIntEstadoCivil.Enabled = True
                    cmbAcomodacaoCobranca.Enabled = True
                    txtIntCPF.Enabled = True
                    txtIntRG.Enabled = True
                    cmbIntRG.Enabled = True
                    cmbIntVinculoId.Enabled = True
                    txtIntFoneResponsavelExc.Enabled = True
                    txtIntLocalTrabalhoResponsavelExc.Enabled = True
                    txtIntEnderecoResponsavelExc.Enabled = True
                    txtIntBairroResponsavelExc.Enabled = True
                    txtIntPoltronaExc.Enabled = True
                    ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                    txtIntApartamentoExc.Enabled = True
                    cmbPratoRapido.Enabled = True
                    'txtIntNome.Focus()
                    ScriptManager.RegisterStartupScript(txtIntNome, txtIntNome.GetType(), Guid.NewGuid().ToString(), "$get('" + txtIntNome.ClientID + "').focus();", True)
                    btnIntegranteGravar.Visible = True
                    btnIntegranteExcluir.Visible = False
                    imgBtnAlterarCategoria.Visible = False
                    imgBtnAlterarMemorando.Visible = False
                    imgBtnAlterarPagamento.Visible = False
                    imgBtnAlterarRefeicao.Visible = False
                    txtIntCPF.Focus()
                End If
            ElseIf hddIntId.Value = 0 And Len(txtIntCPF.Text.Replace(" ", "").Trim) = 0 And Len(txtIntMatricula.Text.Trim) > 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula inválida." + "');", True)
                txtIntMatricula.Attributes.Add("Matricula", "Inválida")
                txtIntMatricula.Text = ""
                'txtIntMatricula.Focus()
                ScriptManager.RegisterStartupScript(txtIntMatricula, txtIntMatricula.GetType(), Guid.NewGuid().ToString(), "$get('" + txtIntMatricula.ClientID + "').focus();", True)
            End If
            txtIntValorUnitarioExc.Enabled = (hddMultiValorado.Value = "S")
            'Quando for usuários das central de atendimento não poderão trocar ou adicionar a refeição e nem trocar a categoria de cobrança dos aptos
            If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Or Not Grupos.Contains("") Then
                lblAcomodacaoCobranca.Visible = False
                cmbAcomodacaoCobranca.Visible = False
                ckbRefeicao.Enabled = False
                imgBtnAlterarRefeicao.Visible = False
                HospedeJaControleCamposIntegrantes()
            End If
            'Inserir aqui, quando pertencer ao grupo de apenas recepcionistas mesmo, e a reserva for do fecomercio e federação, permitir trocar o almoço caso contrario não.

        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao carregar os dados do integrante baseado na matrícula digitada. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnIntMatricula")
        End Try
    End Sub

    Protected Sub btnMoveFocoResNome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMoveFocoResNome.Click
        If pnlResponsavelTitulo_CollapsiblePanelExtender.Collapsed = False Then
            CType(Page.Master.FindControl("scpMngTurismoSocial"), ScriptManager).SetFocus(txtResMatricula)
        End If
    End Sub

    Protected Sub btnIntCPF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIntCPF.Click
        Try
            'Limpando do nome para que a busca aconteça
            txtIntNome.Text = ""
            txtIntMatricula.Text = ""

            If hddIntId.Value = 0 And Len(txtIntMatricula.Text.Replace(" ", "").Trim) = 0 And Len(txtIntCPF.Text.Replace(" ", "").Trim) = 11 Then
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                Else
                    objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
                End If

                If txtIntCPF.Text.Trim.Length > 0 Then
                    If Not IsNumeric(txtIntCPF.Text.ToString.Replace(" ", "")) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O CPF informado não é válido, ele deve ser composto de apenas números.');", True)
                        txtIntCPF.Focus()
                        Exit Try
                    End If
                End If

                objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaCPF(txtIntCPF.Text.Replace(" ", ""))

                cmbIntCatId.Items.Clear()
                cmbIntCatId.Items.Insert(0, New ListItem("Comerciário", "1"))
                cmbIntCatId.Items.Insert(1, New ListItem("Dependente", "2"))
                cmbIntCatId.Items.Insert(2, New ListItem("Conveniado", "3"))
                cmbIntCatId.Items.Insert(3, New ListItem("Usuário", "4"))

                If objReservaListagemIntegranteVO.IntNome = "" Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "O portador do CPF informado ainda não esteve hospedado na unidade.\n\nEntre com os dados manualmente." + "');", True)
                    txtIntCPF.Focus()
                Else
                    txtIntNome.Text = objReservaListagemIntegranteVO.IntNome
                    'Checando a matrícula se esta vencida ou não.
                    If objReservaListagemIntegranteVO.intMatricula.Trim.Length > 0 Then
                        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                            objUopDAO = New Turismo.UopDAO("TurismoSocialCaldas")
                        Else
                            objUopDAO = New Turismo.UopDAO("TurismoSocialPiri")
                        End If

                        If IsNumeric(Mid(objReservaListagemIntegranteVO.intMatricula.Trim, 1, 4)) Then
                            objUopVO = objUopDAO.consultar(Mid(objReservaListagemIntegranteVO.intMatricula.Trim, 1, 4))
                        Else
                            objReservaListagemIntegranteVO.intMatricula = ""
                        End If

                        objClienteVO = New CentralAtendimento.ClienteVO
                        objClienteVO.matricula = objReservaListagemIntegranteVO.intMatricula.Trim
                        objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)

                        'Se carregar matrícula irá checar se ela esta vencida ou não
                        If objClienteVO.dtVencimento.ToString.Length > 1 Then
                            If Not IsNothing(objUopVO) Then ' Localizou o estado de origem da matricula
                                If objUopVO.estSigla = "GO" Then ' Se for Goiás, busca na Central de Atendimento
                                    objClienteVO = objClienteDAO.consultarClientelaeEndereco()
                                Else
                                    'objClienteVO = Nothing
                                End If
                            Else ' Não localizou o estado de origem da matricula
                                objClienteVO = Nothing
                            End If

                            If Not IsNothing(objClienteVO) Then
                                If IsDate(objClienteVO.dtVencimento) And objUopVO.estSigla = "GO" Then
                                    If DateDiff(DateInterval.Day, CDate(Mid(Date.Now, 1, 10)), CDate(objClienteVO.dtVencimento)) < 0 Then
                                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula vencida.Dirija-se a Central de Atendimento.\n\nIntegrante: " & objClienteVO.nome.Trim & "\nVencida em " & Format(CDate(objClienteVO.dtVencimento), "dd/MM/yyyy") & "" + "');", True)
                                        imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                                        Exit Try
                                    End If
                                End If
                            End If
                        End If
                    End If

                    txtIntNascimento.Text = Format(CDate(objReservaListagemIntegranteVO.intDtNascimento), "dd/MM/yyyy")
                    'txtIntMatricula.Text = objReservaListagemIntegranteVO.intMatricula

                    If objReservaListagemIntegranteVO.intMatricula.Trim.Length > 0 Then
                        txtIntMatricula.Text = CLng(objReservaListagemIntegranteVO.intMatricula.Trim.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("\", "").Replace("_", "")).ToString("0000 000000 0")
                    Else
                        txtIntMatricula.Text = objReservaListagemIntegranteVO.intMatricula.Trim
                    End If

                    cmbIntSexo.Text = objReservaListagemIntegranteVO.intSexo
                    'cmbIntCatId.Text = objReservaListagemIntegranteVO.catId
                    objCategoriaDAO = New Turismo.CategoriaDAO("TurismoSocialCaldas")
                    cmbIntCatId.Items.Clear()
                    lista = objCategoriaDAO.consultar("Reserva")
                    cmbIntCatId.DataSource = lista
                    cmbIntCatId.DataValueField = ("catId")
                    cmbIntCatId.DataTextField = ("catDescricao")
                    cmbIntCatId.DataBind()
                    If objReservaListagemIntegranteVO.intMatricula.Trim.Length > 0 Then
                        'Ira sempre carregar a ultima categoria na busca pelo CPF 
                        cmbIntCatId.SelectedValue = objReservaListagemIntegranteVO.catId
                        If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then 'Emissivo irá pegar sempre o da reserva
                            cmbIntCatCobranca.SelectedValue = cmbResCatCobranca.SelectedValue
                        Else
                            cmbIntCatCobranca.SelectedValue = objReservaListagemIntegranteVO.intCatCobranca
                        End If

                    Else

                        If hddResCaracteristica.Value = "E" Then 'Emissivo irá pegar sempre o da reserva
                            cmbIntCatCobranca.SelectedValue = cmbResCatCobranca.SelectedValue
                        Else
                            cmbIntCatId.SelectedValue = 4
                            cmbIntCatCobranca.SelectedValue = 4
                        End If
                    End If

                    'Carregando a lista de todos os estados'
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                        objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                    Else
                        objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                        objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                    End If

                    lista = objEstadoDAO.consultarEstadoPais
                    cmbIntEstId.DataSource = lista
                    cmbIntEstId.DataValueField = ("estId")
                    cmbIntEstId.DataTextField = ("estDescricao")
                    cmbIntEstId.DataBind()

                    'cmbIntCatCobranca.Text = objReservaListagemIntegranteVO.intCatCobranca
                    cmbIntEstId.Text = objReservaListagemIntegranteVO.estId
                    txtIntCidade.Text = objReservaListagemIntegranteVO.intCidade
                    cmbIntSalario.Text = objReservaListagemIntegranteVO.intSalario
                    txtIntRG.Text = objReservaListagemIntegranteVO.intRG.Replace("RG - ", "").Replace("CN - ", "")
                    Try
                        cmbIntEscolaridade.Text = objReservaListagemIntegranteVO.intEscolaridade
                    Catch ex As Exception
                        cmbIntEscolaridade.Text = "1"
                    End Try
                    cmbIntEstadoCivil.Text = objReservaListagemIntegranteVO.intEstadoCivil
                    hddIdade.Value = objReservaListagemIntegranteVO.intIdade
                End If

                'Desabilitar(pnlEdicaoIntegrante)
                'Desabilitar(pnlIntegranteEmissivo)
                txtIntCPF.Enabled = True
                txtIntNome.Enabled = True
                txtIntNascimento.Enabled = True
                cmbIntSexo.Enabled = True
                cmbIntFormaPagamento.Enabled = True
                txtIntMemorando.Enabled = True
                cmbIntEmissor.Enabled = True
                cmbIntCatId.Enabled = True
                cmbIntCatCobranca.Enabled = True
                ckbRefeicao.Enabled = True
                cmbIntEstId.Enabled = True
                cmbIntEstId_SelectedIndexChanged(sender, e)
                cmbIntSalario.Enabled = True
                cmbIntEscolaridade.Enabled = True
                cmbIntEstadoCivil.Enabled = True
                cmbAcomodacaoCobranca.Enabled = True
                txtIntCPF.Enabled = True
                txtIntRG.Enabled = True
                cmbIntRG.Enabled = True
                cmbIntVinculoId.Enabled = True
                txtIntFoneResponsavelExc.Enabled = True
                txtIntLocalTrabalhoResponsavelExc.Enabled = True
                txtIntEnderecoResponsavelExc.Enabled = True
                txtIntBairroResponsavelExc.Enabled = True
                txtIntPoltronaExc.Enabled = True
                ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                txtIntApartamentoExc.Enabled = True
                cmbPratoRapido.Enabled = True
                txtIntNome.Focus()
                btnIntegranteGravar.Visible = True
                btnIntegranteExcluir.Visible = False
                imgBtnAlterarCategoria.Visible = False
                imgBtnAlterarMemorando.Visible = False
                imgBtnAlterarPagamento.Visible = False
                imgBtnAlterarRefeicao.Visible = False
            ElseIf hddIntId.Value = 0 And Len(txtIntMatricula.Text.Replace(" ", "").Trim) > 0 And Len(txtIntCPF.Text.Replace(" ", "").Trim) = 11 Then
                'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "CPF inválido." + "');", True)
                'txtIntCPF.Text = ""
                'txtIntCPF.Focus()
                'Se o comércio veio da central de atendimentos 
                If txtIntNome.Enabled Then
                    txtIntNome.Focus()
                Else
                    cmbIntFormaPagamento.Focus()
                End If
            ElseIf hddIntId.Value = 0 And Len(txtIntMatricula.Text.Replace(" ", "").Trim) = 0 And Len(txtIntCPF.Text.Replace(" ", "").Trim) = 0 Then
                If txtIntMatricula.Attributes("Matricula") = "Inválida" Then
                    txtIntMatricula.Focus()
                Else
                    txtIntNome.Focus()
                End If
                'Se for encontrada na central de atendimentos e importado os dados
            ElseIf hddIntId.Value = 0 And Len(txtIntMatricula.Text.Replace(" ", "").Trim) > 0 Then
                If txtIntCPF.Enabled Then
                    txtIntCPF.Focus()
                Else
                    cmbIntFormaPagamento.Focus()
                End If
            Else
                txtIntCPF.Focus()
            End If
            'Quando for usuários das central de atendimento não poderão trocar ou adicionar a refeição e nem trocar a categoria de cobrança dos aptos
            If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Then
                lblAcomodacaoCobranca.Visible = False
                cmbAcomodacaoCobranca.Visible = False
                ckbRefeicao.Enabled = False
                imgBtnAlterarRefeicao.Visible = False
                HospedeJaControleCamposIntegrantes()
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao carregar os dados do integrante baseado no CPF digitado. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: btnIntCPF")
        End Try
    End Sub

    Protected Sub txtIntNascimento_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIntNascimento.TextChanged
        Try
            Dim Idade As Integer = 0
            If IsDate(txtIntNascimento.Text.Trim) Then
                'Essa idade terá como base o inicio da reserva.
                hddIdade.Value = calculaIdade(CDate(txtIntNascimento.Text), CDate(txtDataInicialSolicitacao.Text))
                'Essa idade esta comparando com o dia de hoje para verificar se é de maior
                Idade = calculaIdade(CDate(txtIntNascimento.Text), Date.Today)
                'Quando digitar uma data de nascimento maior que o dia de hoje.
                If Idade < 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de nascimento não pode ser maior que a data de hoje.');", True)
                    txtIntNascimento.Text = ""
                    txtIntNascimento.Focus()
                    Return
                End If
            Else
                txtIntNascimento.Text = ""
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de nascimento informada não é válida.');", True)
                txtIntNascimento.Focus()
                Return
            End If

            'Irá verificar se a inserção será permitida, respeitando o limite máximo de leitos por acomodação

            If (hddResCaracteristica.Value = "I" And cmbOrgId.SelectedValue <> "37") Then
                If imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" Then
                    Mensagem("Esgotado o número de vagas para essa acomodação.")
                    Exit Try
                ElseIf (imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And hddIdade.Value >= CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                    txtIntNascimento.Text = ""
                    Mensagem("Vagas esgotadas para essa acomodação. \nObs.: Será possível inserir apenas integrantes com idade de berço, que tenha menos de " & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos.\n\n Inserção não permitida.")
                    txtIntNascimento.Focus()
                    Exit Try
                ElseIf ((imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "L" And
                            imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N") And
                            hddIdade.Value > CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco"))) Then
                    txtIntNascimento.Text = ""
                    Mensagem("A idade do integrante não poderá ser superior a idade de criança de berço (" & CLng(imgBtnIntegranteNovoAcao.Attributes.Item("IdadeBerco")) & " anos). \n\n Inserção não permitida.")
                    txtIntNascimento.Focus()
                    Exit Try
                End If
            End If

            'Só ira alterar esses dados se for um novo integrante
            If hddIntId.Value = 0 Then
                If txtIntMatricula.Text.Trim = "" And txtIntNascimento.Text <> "__/__/____" Then
                    txtIntMatricula.Enabled = False
                    txtIntNascimento.Enabled = True
                    cmbIntSexo.Enabled = True
                    cmbIntFormaPagamento.Enabled = True
                    txtIntMemorando.Enabled = True
                    cmbIntEmissor.Enabled = True
                    cmbIntCatId.Enabled = True
                    cmbIntCatId.Items.Clear()
                    Try
                        Dim varIdade As Integer = CDate(hddResDataIni.Value).Year - CDate(txtIntNascimento.Text).Year
                        If CDate(hddResDataIni.Value).Month < CDate(txtIntNascimento.Text).Month Or (CDate(hddResDataIni.Value).Month = CDate(txtIntNascimento.Text).Month And CDate(hddResDataIni.Value).Day < CDate(txtIntNascimento.Text).Day) Then
                            varIdade = varIdade - 1
                        End If
                        If varIdade >= hddIdadeColo.Value Then
                            cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                            cmbIntCatId.Text = 4
                        Else

                            cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                            cmbIntCatId.Text = 4
                        End If
                    Catch ex As Exception
                        cmbIntCatId.Items.Insert(0, New ListItem("Usuário", "4"))
                        cmbIntCatId.Text = 4
                    End Try

                    cmbIntCatCobranca.Enabled = True

                    If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then 'Emissivo
                        cmbIntCatCobranca.SelectedValue = cmbResCatCobranca.SelectedValue
                    ElseIf imgBtnIntegranteNovoAcao.Attributes.Item("IntCopiado") <> "S" Then
                        cmbIntCatCobranca.SelectedValue = 4
                    End If
                    'Se estiver copiando e colando os dados do integrante, não irá alterar a categoria de cobrança

                    ckbRefeicao.Enabled = True

                    If cmbIntEstId.Items.Count <> cmbEstId.Items.Count Then
                        cmbIntEstId.Items.Clear()
                        Dim It As ListItem
                        For Each It In cmbEstId.Items
                            cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
                        Next
                    End If
                    cmbIntEstId.Enabled = True
                    If imgBtnIntegranteNovoAcao.Attributes.Item("IntCopiado") <> "S" Then
                        cmbIntEstId_SelectedIndexChanged(sender, e)
                    Else
                        cmbIntCidade.Enabled = True
                    End If
                    cmbIntSalario.Enabled = True
                    cmbIntEscolaridade.Enabled = True
                    cmbIntEstadoCivil.Enabled = True
                    cmbAcomodacaoCobranca.Enabled = True
                    txtIntCPF.Enabled = True
                    txtIntRG.Enabled = True
                    cmbIntRG.Enabled = True
                    cmbIntVinculoId.Enabled = True
                    txtIntFoneResponsavelExc.Enabled = True
                    txtIntLocalTrabalhoResponsavelExc.Enabled = True
                    txtIntEnderecoResponsavelExc.Enabled = True
                    txtIntBairroResponsavelExc.Enabled = True
                    txtIntValorUnitarioExc.Enabled = (hddMultiValorado.Value = "S")
                    txtIntPoltronaExc.Enabled = True
                    ckbColo.Enabled = (hddIdadeColo.Value = "1000")
                    txtIntApartamentoExc.Enabled = True
                    cmbPratoRapido.Enabled = True
                    btnIntegranteGravar.Visible = True
                    btnIntegranteExcluir.Visible = False
                    imgBtnAlterarCategoria.Visible = False
                    imgBtnAlterarMemorando.Visible = False
                    imgBtnAlterarPagamento.Visible = False
                    imgBtnAlterarRefeicao.Visible = False
                End If

                If txtIntNascimento.Text = "__/__/____" Or txtIntNascimento.Text = "" Then
                    txtIntNascimento.Focus()
                Else
                    cmbIntSexo.Focus()
                End If
            End If
            'Quando for usuários das central de atendimento não poderão trocar ou adicionar a refeição e nem trocar a categoria de cobrança dos aptos
            If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Then
                lblAcomodacaoCobranca.Visible = False
                cmbAcomodacaoCobranca.Visible = False
                ckbRefeicao.Enabled = False
                imgBtnAlterarRefeicao.Visible = False
                HospedeJaControleCamposIntegrantes()
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
           "&Erro=Erro ao executar o changed do txtintNascimento. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: txtIntNascimento")
        End Try
    End Sub

    Protected Sub LimpaCamposResponsavel()
        txtResNome.Text = ""
        txtResCPF.Text = ""
        txtResContato.Text = ""
        txtResEmail.Text = ""
        txtResDtNascimento.Text = "01/01/1900"
        txtResFoneComercial.Text = ""
        txtResFoneResidencial.Text = ""
        txtResCelular.Text = ""
        txtResFax.Text = ""
        txtResLogradouro.Text = ""
        txtResNumero.Text = ""
        txtResQuadra.Text = ""
        txtResLote.Text = ""
        txtResComplemento.Text = ""
        txtResBairro.Text = ""
        txtResCep.Text = ""
        cmbResSalario.SelectedIndex = 0
        cmbResEscolaridade.SelectedIndex = 0
        cmbResEstadoCivil.SelectedIndex = 0
        cmbResDocIdentificacao.SelectedIndex = 0
        txtResDocIdentificacao.Text = ""
        txtResMemorando.Text = ""
        cmbResEmissor.SelectedIndex = 0
        txtResObs.Text = ""
    End Sub

    Protected Sub cmbHospedagem_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHospedagem.Init
        Dim Cont As Integer = 0
        Dim Total As Integer = cmbHospedagem.Items.Count
        While Cont < Total
            Select Case Me.cmbHospedagem.Items(Cont).ToString
                Case "Todos"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:White;")
                Case "Especiais"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:#FF69B4; color:White;")
                Case "Fecomércio"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:#FF0000;color:White;")
                Case "Normal"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:Black;color:White;")
                Case "RT"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:#008B00;color:White;")
                Case "RTM"
                    Me.cmbHospedagem.Items(Cont).Attributes.Add("style", "background-color:#3333FF;color:White;")
            End Select
            Cont = Cont + 1
        End While
    End Sub

    Protected Sub txtResDtNascimento_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtResDtNascimento.TextChanged
        If IsDate(txtResDtNascimento.Text.Trim) Then
            'Quando digitar uma data de nascimento maior que o dia de hoje.
            Dim Idade As Integer = 0
            'Idade com base no dia de hoje
            Idade = calculaIdade(CDate(txtResDtNascimento.Text), Date.Today)
            'Idade com base no dia inicial da reserva
            hddIdade.Value = calculaIdade(CDate(txtResDtNascimento.Text), CDate(txtDataInicialSolicitacao.Text))

            If Idade < 18 Then
                Mensagem("Idade inválida!\n\n O responsável pela reserva tem que ser maior de idade.")
                txtResDtNascimento.Focus()
                Return
            End If

            If Idade < 0 Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de nascimento não pode ser maior que a data de hoje.');", True)
                txtResDtNascimento.Text = ""
                txtResDtNascimento.Focus()
                Return
            End If

            btnReservaGravar.Attributes.Remove("CPF")
            'Se for de maior, Sem CPF e <> 37 = Presidência.

            cmbResSexo.Focus()
        Else
            txtResDtNascimento.Text = ""
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data de nascimento informada não é válida.');", True)
            txtResDtNascimento.Focus()
            Return
        End If
    End Sub

    Protected Sub imgBtnResCPF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgBtnResCPF.Click
        Try
            Dim ConexaoBanco As String
            If Len(txtResCPF.Text.Replace(" ", "").Replace(".", "").Replace("-", "")) = 11 Then
                objReservaListagemSolicitacaoVO = New Turismo.ReservaListagemSolicitacaoVO
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                    ConexaoBanco = "TurismoSocialCaldas"
                Else
                    objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                    ConexaoBanco = "TurismoSocialCaldas"
                End If

                If txtResCPF.Text.Trim.Length > 0 Then
                    If Not IsNumeric(txtResCPF.Text.ToString.Replace(" ", "").Replace(".", "").Replace("-", "")) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O CPF informado não é válido, ele deve ser composto de apenas números.');", True)
                        txtResCPF.Focus()
                        Exit Try
                    End If
                End If

                txtResContato.Text = ""
                'Irá buscar no TbLeReserva o Contato de emergencia com base na matrícula e inserir no campo devido.
                objClienteVO = New CentralAtendimento.ClienteVO
                objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)
                objClienteVO = objClienteDAO.ConsultaContatoEmergencia(Mid((txtResCPF.Text.Replace(" ", "").Replace(".", "").Replace("-", "").Trim), 1, 11), ConexaoBanco)
                txtResContato.Text = objClienteVO.ContatoEmergencia
                Dim ContatoEmergencia = objClienteVO.ContatoEmergencia

                objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaCPF(txtResCPF.Text.Trim.Replace(" ", "").Replace("-", "").Replace(".", "").Replace("_", ""))

                If objReservaListagemSolicitacaoVO.resNome = "" Then
                    LimpaCamposResponsavel()
                    txtResMatricula.Text = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "O portador do CPF informado ainda não esteve hospedado na unidade.\n\nEntre com os dados manualmente." + "');", True)
                    txtResCPF.Focus()
                Else
                    txtResNome.Text = Mid(objReservaListagemSolicitacaoVO.resNome.Trim, 1, 100)

                    ''Checando a matrícula se esta vencida ou não. tenho que terminar os teste e ver se esta ok 19-10-2014
                    'If objReservaListagemSolicitacaoVO.resMatricula.Trim.Length > 0 Then
                    '    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    '        objUopDAO = New Turismo.UopDAO("TurismoSocialCaldas")
                    '    Else
                    '        objUopDAO = New Turismo.UopDAO( "TurismoSocialPiri")
                    '    End If

                    '    objUopVO = objUopDAO.consultar(Mid(objReservaListagemSolicitacaoVO.resMatricula.Trim, 1, 4))
                    '    objClienteVO = New CentralAtendimento.ClienteVO
                    '    objClienteVO.matricula = objReservaListagemSolicitacaoVO.resMatricula.Trim
                    '    objClienteDAO = New CentralAtendimento.ClienteDAO(objClienteVO)

                    '    'Se carregar matrícula irá checar se ela esta vencida ou não
                    '    If objClienteVO.dtVencimento.ToString.Length > 1 Then
                    '        If Not IsNothing(objUopVO) Then ' Localizou o estado de origem da matricula
                    '            If objUopVO.estSigla = "GO" Then ' Se for Goiás, busca na Central de Atendimento
                    '                objClienteVO = objClienteDAO.consultarClientelaeEndereco()
                    '            Else
                    '                'objClienteVO = Nothing
                    '            End If
                    '        Else ' Não localizou o estado de origem da matricula
                    '            objClienteVO = Nothing
                    '        End If

                    '        If Not IsNothing(objClienteVO) Then
                    '            If IsDate(objClienteVO.dtVencimento) Then
                    '                If DateDiff(DateInterval.Day, CDate(Mid(Date.Now, 1, 10)), CDate(objClienteVO.dtVencimento)) < 0 Then
                    '                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Matrícula vencida.Dirija-se a Central de Atendimento.\n\nIntegrante: " & objClienteVO.nome.Trim & "\nVencida em " & Format(CDate(objClienteVO.dtVencimento), "dd/MM/yyyy") & "" + "');", True)
                    '                    imgBtnIntegranteNovoAcao_Click(sender:=Nothing, e:=Nothing)
                    '                    Exit Try
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'End If
                    'Carregando a lista de todos os estados'
                    If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                        objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralCaldas")
                        objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralCaldas")
                    Else
                        objEstadoDAO = New Geral.BdProdEstadoDAO("DbGeralPiri")
                        objMunicipioDAO = New Geral.BdProdMunicipioDAO("DbGeralPiri")
                    End If

                    lista = objEstadoDAO.consultarEstadoPais
                    cmbEstId.DataSource = lista
                    cmbEstId.DataValueField = ("estId")
                    cmbEstId.DataTextField = ("estDescricao")
                    cmbEstId.DataBind()

                    txtResDtNascimento.Text = Format(CDate(objReservaListagemSolicitacaoVO.resDtNascimento), "dd/MM/yyyy")
                    'If objReservaListagemSolicitacaoVO.resMatricula.Trim.Length > 0 Then

                    If IsNumeric(objReservaListagemSolicitacaoVO.resMatricula) Then
                        txtResMatricula.Text = Format(CLng(objReservaListagemSolicitacaoVO.resMatricula.Replace("-", "").Replace("_", "").Replace(".", "").Replace(" ", "")), "0000 000000 0")
                    Else
                        txtResMatricula.Text = ""
                    End If

                    cmbResSexo.Text = objReservaListagemSolicitacaoVO.resSexo
                    cmbResCatId.Text = objReservaListagemSolicitacaoVO.catId
                    If ContatoEmergencia = "" Then
                        txtResContato.Text = objReservaListagemSolicitacaoVO.resContato.Trim
                    Else
                        txtResContato.Text = ContatoEmergencia
                    End If
                    txtResLogradouro.Text = Mid(objReservaListagemSolicitacaoVO.resLogradouro.Trim, 1, 40)
                    txtResNumero.Text = Mid(objReservaListagemSolicitacaoVO.resNumero.Trim, 1, 10)
                    txtResComplemento.Text = Mid(objReservaListagemSolicitacaoVO.resComplemento.Trim, 1, 40)
                    txtResBairro.Text = objReservaListagemSolicitacaoVO.resBairro
                    txtResCep.Text = objReservaListagemSolicitacaoVO.resCep.Trim.Replace("'", "").Insert(5, " ")
                    cmbEstId.Text = objReservaListagemSolicitacaoVO.estId
                    txtResCidade.Text = Mid(objReservaListagemSolicitacaoVO.resCidade.Trim, 1, 40)
                    cmbResSalario.Text = objReservaListagemSolicitacaoVO.resSalario
                    cmbResEscolaridade.Text = objReservaListagemSolicitacaoVO.resEscolaridade
                    cmbResEstadoCivil.Text = objReservaListagemSolicitacaoVO.resEstadoCivil
                    txtResEmail.Text = Mid(objReservaListagemSolicitacaoVO.resEmail.Trim, 1, 40)
                    'Carregando a RG
                    Select Case Mid(objReservaListagemSolicitacaoVO.resRG, 1, 3).Trim
                        Case "RG"
                            cmbResDocIdentificacao.SelectedValue = "RG"
                            txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                        Case "CC"
                            cmbResDocIdentificacao.SelectedValue = "CC"
                            txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                        Case "CN"
                            cmbResDocIdentificacao.SelectedValue = "CN"
                            txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                        Case "OU"
                            cmbResDocIdentificacao.SelectedValue = "OU"
                            txtResDocIdentificacao.Text = Mid(objReservaListagemSolicitacaoVO.resRG, 5, 25).Trim
                        Case Else
                            cmbResDocIdentificacao.SelectedValue = "RG"
                            txtResDocIdentificacao.Text = objReservaListagemSolicitacaoVO.resRG.Trim
                    End Select
                End If
            End If
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
          "&Erro=Erro na consulta de um Responsável pela reserva com base no CPF informado. " & "&sistema=Sistema de Reservas" & "&acao=Formulário: Reserva.vb " & "&Local=Objeto: imgBtnResCPF - CPF: " & txtResCPF.Text & "")
        End Try
    End Sub

    Protected Sub CarregaTodosEstadosIntegrante(ByVal ResponsavelIntegrante As String)
        If ResponsavelIntegrante = "R" Then 'Responsável
            cmbEstId.Items.Clear()
            Dim Item As ListItem
            For Each Item In cmbEstadoAux.Items
                cmbEstId.Items.Add(New ListItem(Item.Text, Item.Value))
            Next
        Else
            cmbIntEstId.Items.Clear()
            Dim It As ListItem
            For Each It In cmbEstadoAux.Items
                cmbIntEstId.Items.Add(New ListItem(It.Text, It.Value))
            Next
        End If
    End Sub
    Protected Sub AlteraDadosEstada()
        If (hddResStatus.Value = "E" Or hddResStatus.Value = "P") Then
            txtIntMatricula.Enabled = False
            If cmbIntFormaPagamento.SelectedValue = "F" Then
                txtIntNome.Enabled = True
                txtIntCPF.Enabled = True
            Else
                txtIntNome.Enabled = False
                txtIntCPF.Enabled = False
            End If
            txtIntNascimento.Enabled = False
            cmbIntSexo.Enabled = False
            cmbIntFormaPagamento.Enabled = False
            txtIntMemorando.Enabled = False
            cmbIntEmissor.Enabled = False
            cmbIntCatId.Enabled = False
            cmbIntCatCobranca.Enabled = False
            cmbIntEstId.Enabled = False
            cmbIntCidade.Enabled = False
            cmbIntSalario.Enabled = False
            cmbIntEscolaridade.Enabled = False
            cmbIntEstadoCivil.Enabled = False
            cmbIntRG.Enabled = False
            txtIntRG.Enabled = False
        End If
    End Sub

    Protected Sub imgTiraAcomodacao_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            hddProcessando.Value = ""
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                objReservaListagemIntegranteDAO = New Turismo.ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If

            Dim Lista As String = ""
            Dim strResultado As New Turismo.ReservaListagemIntegranteDAO.Resultado
            'Passando pelo grid 
            For Each linha As GridViewRow In gdvReserva9.Rows
                'Ira passar somente nos apartamentos checados
                If CType(linha.FindControl("chkTiraAcomodacao"), CheckBox).Checked = True Then
                    If gdvReserva9.Columns(3).Visible And CInt(CType(linha.FindControl("lnkIntNome"), LinkButton).CommandArgument) > 0 Then
                        hddHosId.Value = CType(linha.FindControl("lnkIntNome"), LinkButton).CommandArgument
                        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaHosId(hddHosId.Value)
                        hddIntId.Value = objReservaListagemIntegranteVO.intId
                    Else
                        hddIntId.Value = Math.Abs(CInt(sender.CommandArgument.ToString)).ToString
                        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.consultarViaIntId(hddIntId.Value)
                    End If
                    Lista = Lista.Trim & objReservaListagemIntegranteVO.IntNome.Trim & "\n"

                    'Retirando as acomodações dos integrantes selecionados

                    strResultado = objReservaListagemIntegranteDAO.AcaoHospedagem(objReservaListagemIntegranteVO, "T")

                    If Not String.IsNullOrEmpty(strResultado.mensagem.Trim) Then
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + strResultado.mensagem.Trim + "');", True)
                    End If

                    CType(linha.FindControl("chkTiraAcomodacao"), CheckBox).Checked = False
                End If
            Next
            ListaIntegranteViaResId()
            If String.IsNullOrEmpty(strResultado.mensagem.Trim) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Acomodação desvinculada com sucesso. Integrante(s):\n\n " & Lista & " ');", True)
            End If

        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Sub

    Protected Sub txtResDtLimiteRetorno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtResDtLimiteRetorno.TextChanged
        'If txtResDtLimiteRetorno.Text.Length > 0 And txtResDtLimiteRetorno.Text.Length >= 10 Then
        '    If IsDate(txtResDtLimiteRetorno.Text) Then
        '        If ((DateDiff(DateInterval.Day, CDate(txtDataInicialSolicitacao.Text), DateAdd(DateInterval.Day, 3, CDate(txtResDtLimiteRetorno.Text))) > 0) _
        '            And txtResDtLimiteRetorno.Text.Length > 0) Then
        '            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Com a data informada, o boleto de pagamento será gerado com a data vencida.\n\nVerifique por favor.\n\nVencimento do Boleto: " & Format(CDate(DateAdd(DateInterval.Day, 3, CDate(txtResDtLimiteRetorno.Text))), "dd/MM/yyyy") & "\nData Inicial: " & Format(CDate(txtDataInicialSolicitacao.Text), "dd/MM/yyyy") & "');", True)
        '            'txtResDtLimiteRetorno.Focus()
        '            Return
        '        End If
        '    End If
        'End If
    End Sub

    Protected Sub gdvReserva9_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvReserva9.SelectedIndexChanged
        Dim Linha As Integer = 0
        btnIntegranteGravar.Attributes.Add("Linha", gdvReserva9.SelectedRow.RowIndex)
        'Pegando o Idade para calculo das crianças.
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialCaldas")
        Else
            objPlanilhaCustoDAO = New Turismo.PlanilhaCustoDAO("TurismoSocialPiri")
        End If
        objPlanilhaCustoVO = objPlanilhaCustoDAO.consultar(hddResId.Value)
        'Se for emissivo irá pegar o valor da tabela
        If hddResCaracteristica.Value = "P" Then
            hddIdadeColo.Value = objPlanilhaCustoVO.plcColo
            hddIdadeAdulto.Value = objPlanilhaCustoVO.PlcIdadeCrianca
            hddIdadeCrianca.Value = objPlanilhaCustoVO.PlcIdadeIsento
        End If
    End Sub

    Protected Sub btnIntUltimoRegistro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIntUltimoRegistro.Click
        'Se tiver algo gravado em Linha, irá direcionar caso contrário irá apontar o usuário par ao primeiro registro do grid.
        If btnIntegranteGravar.Attributes.Item("Linha") <> Nothing And
           btnIntegranteGravar.Attributes.Item("Linha") <> "" Then
            CType(gdvReserva9.Rows.Item(btnIntegranteGravar.Attributes.Item("Linha")).FindControl("lnkIntNome"), LinkButton).Focus()
        Else
            CType(gdvReserva9.Rows.Item(0).FindControl("lnkIntNome"), LinkButton).Focus()
        End If
    End Sub

    Protected Sub CopiaDadosIntegrante(ByVal Acao As String)
        Dim sender As Object = Nothing
        Dim e As System.EventArgs = Nothing
        If Acao = "C" Then 'Copiar
            With imgBtnIntegranteNovoAcao.Attributes
                .Add("FormaPagamento", cmbIntFormaPagamento.SelectedValue)
                .Add("Memorando", txtIntMemorando.Text.Trim)
                .Add("Emissor", cmbIntEmissor.SelectedValue)
                .Add("Categoria", cmbIntCatId.SelectedValue)
                .Add("Cobranca", cmbIntCatCobranca.SelectedValue)
                .Add("Estado", cmbIntEstId.SelectedValue)
                'cmbIntEstId_SelectedIndexChanged(sender, e)
                .Add("Cidade", cmbIntCidade.SelectedValue)
                If ckbRefeicao.Items(0).Selected = True Then .Add("AlmocoIn", True) Else .Add("AlmocoIn", False)
                If ckbRefeicao.Items(1).Selected = True Then .Add("AlmocoOut", True) Else .Add("AlmocoOut", False)
                If ckbRefeicao.Items(2).Selected = True Then .Add("JantarIn", True) Else .Add("JantarIn", False)
                If ckbRefeicao.Items(3).Selected = True Then .Add("JantarOut", True) Else .Add("JantarOut", False)
                imgBtnIntegranteNovoAcao.Attributes.Add("IntCopiado", "S")
            End With
        ElseIf Acao = "D" Then 'Descarregando'
            With imgBtnIntegranteNovoAcao.Attributes
                cmbIntFormaPagamento.SelectedValue = .Item("FormaPagamento")
                txtIntMemorando.Text = .Item("Memorando")
                cmbIntEmissor.SelectedValue = .Item("Emissor")
                cmbIntCatId.SelectedValue = .Item("Categoria")
                cmbIntCatCobranca.SelectedValue = .Item("Cobranca")
                cmbIntEstId.SelectedValue = .Item("Estado")
                cmbIntEstId_SelectedIndexChanged(sender, e)
                cmbIntCidade.SelectedValue = .Item("Cidade")
                If .Item("AlmocoIn") = True Then ckbRefeicao.Items(0).Selected = True Else ckbRefeicao.Items(0).Selected = False
                If .Item("AlmocoOut") = True Then ckbRefeicao.Items(1).Selected = True Else ckbRefeicao.Items(1).Selected = False
                If .Item("JantarIn") = True Then ckbRefeicao.Items(2).Selected = True Else ckbRefeicao.Items(2).Selected = False
                If .Item("JantarOut") = True Then ckbRefeicao.Items(3).Selected = True Else ckbRefeicao.Items(3).Selected = False
            End With
        Else
            With imgBtnIntegranteNovoAcao.Attributes
                .Remove("FormaPagamento")
                .Remove("Memorando")
                .Remove("Emissor")
                .Remove("Categoria")
                .Remove("Cobranca")
                .Remove("Estado")
                .Remove("Cidade")
                .Remove("AlmocoIn")
                .Remove("AlmocoOut")
                .Remove("JantarIn")
                .Remove("JantarOut")
                'Saindo do modo de copia dos dados dos integrantes
                .Remove("IntCopiado")
            End With
        End If
    End Sub
    Public Function Func_Ultimo_Dia_Mes(paramDataX As Date) As Date
        Func_Ultimo_Dia_Mes = DateAdd("m", 1, DateSerial(Year(paramDataX), Month(paramDataX), 1))
        Func_Ultimo_Dia_Mes = DateAdd("d", -1, Func_Ultimo_Dia_Mes)
    End Function

    Protected Sub Mensagem(Texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + Texto + "');", True)
    End Sub

    Public Function ValidaDataHospedeJa(DataInicialCa As DateTime, DataFinalCa As DateTime) As Boolean
        Dim Resultado As Boolean = True
        Dim objTestaGrupo As New Uteis.TestaUsuario
        Dim objListaGrupo As New Uteis.TestaUsuario
        Dim varGrupos = objListaGrupo.listaGrupos(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")).ToUpper
        'Pertencentes a esse grupo poderão fazer reserva dentro do Mês corrente no balcão 
        'Com pagamento no boleto e vencimento no dia da reserva
        Dim DataLimiteFinalCa As DateTime
        'DataInicialCa = CDate(txtDataInicialSolicitacao.Text)
        'DataFinalCa = CDate(txtDataFinalSolicitacao.Text)
        DataLimiteFinalCa = Func_Ultimo_Dia_Mes(DateAdd(DateInterval.Month, 1, Date.Now))

        If Session("GrupoCentralAtendimento") Then
            btnHospedagemNova.Attributes.Add("UsuariosCA", "S")
            If DateDiff(DateInterval.Day, DataInicialCa, DataFinalCa) > 7 Then
                Mensagem("A quantidade de diárias não poderá ser superior a 7.")
                txtDataInicialSolicitacao.Focus()
                Resultado = False
            End If

            'If (Month(DataInicialCa) <> Month(Date.Today)) Then
            'Dilatei a data do hospede já para o mês atual mais um.
            If (Month(DataInicialCa) > Month(Date.Today) + 1) Then
                Mensagem("Período não autorizado! \n\nO mês " & Month(DataInicialCa) & " esta fora do limite permitido para as reservas do HospedeJá.")
                txtDataInicialSolicitacao.Focus()
                Resultado = False
            End If

            'Limita a data da saída: ser apenas um dia a mais caso o final da reserva caia no sábado
            If (DataFinalCa > DataLimiteFinalCa) Then 'se a data final for maior que o último dia do mês ira testar se a saida é diferente de sabado
                If DataFinalCa.DayOfWeek <> DayOfWeek.Saturday Then
                    Mensagem("Período não autorizado! \n\nA data final não pode ser maior que: " & Format(DataLimiteFinalCa, "dd/MM/yyyy") & " .")
                    Resultado = False
                    txtDataFinalSolicitacao.Focus()
                Else
                    If DataFinalCa > DateAdd(DateInterval.Day, 1, DataLimiteFinalCa) Then 'como é igual irá testar se a saída é maior que o limite mais um que é o tolerável.
                        Mensagem("Período não autorizado! \n\nA data final não pode ser maior que: " & Format(DateAdd(DateInterval.Day, 1, DataLimiteFinalCa), "dd/MM/yyyy") & " .")
                        Resultado = False
                        txtDataFinalSolicitacao.Focus()
                    End If
                End If
            End If
        End If
        Return Resultado
    End Function

    Private Function VerificaIntegranteAcomodacao() As String
        Dim ResultadoAdulto As String = ""
        Dim ResultadoCrianca As String = ""

        'Pegando o Idade para calculo das crianças.
        objReservaListagemIntegranteVO = New ReservaListagemIntegranteVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
        Else
            objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
        End If
        objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.VerificaIntegranteAcomodacao(hddResId.Value, hddSolId.Value, 0)

        With objReservaListagemIntegranteVO
            If .QtdMaior >= .MaximoPermitido Then
                ResultadoAdulto = "N" 'Negar nova inserção
            Else
                ResultadoAdulto = "L" 'Liberar nova inserção
            End If

            'Maior e menor completado
            If .QtdMenor >= 2 And ResultadoAdulto = "N" Then
                ResultadoAdulto = "N" 'Negar nova inserção
                ResultadoCrianca = "N" 'Neghar noova inserção
            ElseIf .QtdMenor < 2 Then
                ResultadoCrianca = "A" 'Autorizar nova inserção
            End If
        End With
        Return ResultadoAdulto
    End Function

    Protected Sub ValidaTotaldeIntegrantesPorAcomodacao()
        'Verifica se o número máximo de leitos por acomodação já foi completado (N - Negar inserção e L-Liberar Inserção)
        If (hddResCaracteristica.Value = "I") Then 'Passeio e Grupos não entram na questão - excessão: Presidencia
            imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoAdulto", "")
            imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoCrianca", "")
            objReservaListagemIntegranteVO = New ReservaListagemIntegranteVO
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
            Else
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
            End If
            'Efetivando a consulta e carregando os objetos
            objReservaListagemIntegranteVO = objReservaListagemIntegranteDAO.VerificaIntegranteAcomodacao(hddResId.Value, CLng(radAcomodacao.Attributes.Item("solId")), 0) 'hddSolId.Value)
            imgBtnIntegranteNovoAcao.Attributes.Add("IdadeBerco", objReservaListagemIntegranteVO.IdadeBerco)

            With objReservaListagemIntegranteVO
                If (.QtdMaior - .SaidaAntecipada) >= .MaximoPermitido Then 'Se sair uma pessoa antecipada, irá liberar a vaga para uma outra pessoa
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoAdulto", "N")
                Else
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoAdulto", "L")
                End If

                'Maior e menor completados
                If .QtdMenor >= 2 And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" Then
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoAdulto", "N")
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoCrianca", "N")
                    '- 2 - Limite de crianças na idade de berço (Se for inserindo clianças Acima de 2 irá tirando a reserva dos adultos) 
                ElseIf ((.QtdMenor + .QtdMaior) - (2 + .SaidaAntecipada)) >= .MaximoPermitido Then
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoAdulto", "N")
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoCrianca", "N")
                ElseIf .QtdMenor < 2 Then
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoCrianca", "L")
                ElseIf .QtdMenor >= 2 Then
                    imgBtnIntegranteNovoAcao.Attributes.Add("ResultadoCrianca", "N")
                End If

                If imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "N" Then
                    'Mensagem("Esgotado o número de vagas para essa acomodação.")
                    pnlEdicaoIntegrante.Visible = False
                ElseIf imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoAdulto") = "N" And imgBtnIntegranteNovoAcao.Attributes.Item("ResultadoCrianca") = "L" Then
                    'Mensagem("O número leitos para adultos foi completado para essa acomodação!\n\n Você poderá inserir no máximo 2 crianças com idade de colo.")
                End If
            End With
        End If
        'Na hora que inserir a data de nascimento do novo integrante, irei pegar a idade e ver se é adulto ou criança e fazer o tratamento
        'Final da verificação do máximo de pessoas no apto
    End Sub

    Protected Sub imgSobeTodos_Click(sender As Object, e As ImageClickEventArgs)
        'Dim ListaAuxiliar As New ArrayList
        Dim objLotacaoDAO As New FPW.LotacaoDAO
        Dim objLotacaoVO As New FPW.LotacaoVO
        Dim objTestaGrupo As New Uteis.TestaUsuario

        Dim objInsereSolicitacaoHospedagemDAO As Turismo.InsereSolicitacaoHospedagemDAO
        Dim Banco As String = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            Banco = "DbGeralCaldas"
            'objReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
        Else
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            Banco = "DbGeralPiri"
            'objReservaDAO = New Turismo.ReservaDAO( "TurismoSocialPiri")
        End If

        'Inicia o processo de bloqueio dos apartamentos selecionados
        For Each linha As GridViewRow In gdvReserva6.Rows
            If CType(linha.FindControl("chkSobeApto"), CheckBox).Checked = True Then
                If hddOrgGrupo.Value = "F" Then
                    objLotacaoVO.loCodLot = hddOrgLotacao.Value
                Else
                    lista = objLotacaoDAO.consultarReserva(Banco, objTestaGrupo.listaInitials(User.Identity.Name.Replace("SESC-GO.COM.BR\", "")))
                    objLotacaoVO = lista.Item(0)
                End If

                Dim varResId As String
                'Antes estava SelectedValue = 'P' só que presidencia agora é "S" de fecomércio
                If (hddOrgGrupo.Value = "F") Or cmbHospedagem.SelectedValue = "S" Or
                    (((Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or
                    (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Session("GrupoRecepcaoPiri"))) And
                    (CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtInicial").ToString) = Now.Date)) Then
                    '(CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString) = Now.Date)) Then

                    varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                      hddResId.Value.ToString, gdvReserva6.DataKeys(linha.RowIndex).Item("acmId").ToString,
                      gdvReserva6.DataKeys(linha.RowIndex).Item("apaId").ToString,
                      Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                      Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                      "A",
                      User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                      objLotacaoVO.loCodLot.ToString)
                Else
                    varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                      hddResId.Value.ToString, gdvReserva6.DataKeys(linha.RowIndex).Item("acmId").ToString,
                      gdvReserva6.DataKeys(linha.RowIndex).Item("apaId").ToString,
                      Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                      Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                      "C",
                      User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                      objLotacaoVO.loCodLot.ToString)
                End If

                If varResId = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar a operação." + "');", True)
                Else
                    If hddResId.Value <> varResId Then
                        hddResId.Value = varResId
                        hddIntId.Value = "0"
                        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                        CarregaCmbRefeicaoPrato()
                        CarregaDadosReserva()
                        pnlResponsavelTitulo.Visible = True
                        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
                        pnlResponsavelAcao.Visible = True
                    ElseIf hddResId.Value = varResId Then
                        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                        CarregaCmbRefeicaoPrato()
                        CarregaDadosReserva()
                    End If
                    While CType(linha.FindControl("txtQtdeAcomodacao"), TextBox).Text > 1
                        varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                          hddResId.Value.ToString, gdvReserva6.DataKeys(linha.RowIndex).Item("acmId").ToString, "",
                          Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                          Format(CDate(gdvReserva6.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                          "C",
                          User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                          objLotacaoVO.loCodLot.ToString)
                        CType(linha.FindControl("txtQtdeAcomodacao"), TextBox).Text -= 1
                    End While
                End If

                lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
                gdvReserva8.DataSource = lista
                gdvReserva8.DataBind()
                gdvReserva8.SelectedIndex = -1
                btnHospedagemNova_Click(sender, e)
                pnlSolicitacaoSelecionada.Visible = True
                pnlAcomodacaoTitulo.Visible = True
                lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, 0) 'hddIntId.Value)
                gdvReserva11.DataSource = lista
                gdvReserva11.DataBind()
                gdvReserva11.SelectedIndex = -1
            End If
        Next
    End Sub

    Protected Sub imgSobeTodos_Click1(sender As Object, e As ImageClickEventArgs)
        'Dim ListaAuxiliar As New ArrayList
        Dim objLotacaoDAO As New FPW.LotacaoDAO
        Dim objLotacaoVO As New FPW.LotacaoVO
        Dim objTestaGrupo As New Uteis.TestaUsuario

        Dim objInsereSolicitacaoHospedagemDAO As Turismo.InsereSolicitacaoHospedagemDAO
        Dim Banco As String = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialCaldas")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
            Banco = "DbGeralCaldas"
            'objReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
        Else
            objInsereSolicitacaoHospedagemDAO = New Turismo.InsereSolicitacaoHospedagemDAO("TurismoSocialPiri")
            objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
            objReservaListagemAcomodacaoDAO = New Turismo.ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
            Banco = "DbGeralPiri"
            'objReservaDAO = New Turismo.ReservaDAO( "TurismoSocialPiri")
        End If

        'Inicia o processo de bloqueio dos apartamentos selecionados
        For Each linha As GridViewRow In gdvReserva7.Rows
            If CType(linha.FindControl("chkSobeApto"), CheckBox).Checked = True Then
                If hddOrgGrupo.Value = "F" Then
                    objLotacaoVO.loCodLot = hddOrgLotacao.Value
                Else
                    lista = objLotacaoDAO.consultarReserva(Banco, objTestaGrupo.listaInitials(User.Identity.Name.Replace("SESC-GO.COM.BR\", "")))
                    objLotacaoVO = lista.Item(0)
                End If

                Dim varResId As String
                'Antes estava SelectedValue = 'P' só que presidencia agora é "S" de fecomércio
                If (hddOrgGrupo.Value = "F") Or cmbHospedagem.SelectedValue = "S" Or
                    (((Session("MasterPage").ToString = "~/TurismoSocial.Master" And Session("GrupoRecepcao")) Or
                    (Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" And Session("GrupoRecepcaoPiri"))) And
                    (CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtInicial").ToString) = Now.Date)) Then
                    '(CDate(sender.DataKeys(sender.SelectedIndex).Item(1).ToString) = Now.Date)) Then

                    varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                      hddResId.Value.ToString, gdvReserva7.DataKeys(linha.RowIndex).Item("acmId").ToString,
                      gdvReserva7.DataKeys(linha.RowIndex).Item("apaId").ToString,
                      Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                      Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                      "A",
                      User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                      objLotacaoVO.loCodLot.ToString)
                Else
                    varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                      hddResId.Value.ToString, gdvReserva7.DataKeys(linha.RowIndex).Item("acmId").ToString,
                      gdvReserva7.DataKeys(linha.RowIndex).Item("apaId").ToString,
                      Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                      Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                      "C",
                      User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                      objLotacaoVO.loCodLot.ToString)
                End If

                If varResId = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi possível executar a operação." + "');", True)
                Else
                    If hddResId.Value <> varResId Then
                        hddResId.Value = varResId
                        hddIntId.Value = "0"
                        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                        CarregaCmbRefeicaoPrato()
                        CarregaDadosReserva()
                        pnlResponsavelTitulo.Visible = True
                        pnlResponsavelTitulo_CollapsiblePanelExtender.ClientState = "True"
                        pnlResponsavelAcao.Visible = True
                    ElseIf hddResId.Value = varResId Then
                        objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
                        CarregaCmbRefeicaoPrato()
                        CarregaDadosReserva()
                    End If
                    While CType(linha.FindControl("txtQtdeAcomodacao"), TextBox).Text > 1
                        varResId = objInsereSolicitacaoHospedagemDAO.insereSolicitacaoHospedagem(
                          hddResId.Value.ToString, gdvReserva7.DataKeys(linha.RowIndex).Item("acmId").ToString, "",
                          Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtInicial").ToString), "dd/MM/yyyy"),
                          Format(CDate(gdvReserva7.DataKeys(linha.RowIndex).Item("dtFinal").ToString), "dd/MM/yyyy"), "12", "12",
                          "C",
                          User.Identity.Name.Replace("SESC-GO.COM.BR\", ""),
                          objLotacaoVO.loCodLot.ToString)
                        CType(linha.FindControl("txtQtdeAcomodacao"), TextBox).Text -= 1
                    End While
                End If

                lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
                gdvReserva8.DataSource = lista
                gdvReserva8.DataBind()
                gdvReserva8.SelectedIndex = -1
                btnHospedagemNova_Click(sender, e)
                pnlSolicitacaoSelecionada.Visible = True
                pnlAcomodacaoTitulo.Visible = True
                lista = objReservaListagemAcomodacaoDAO.consultarHospedagemDisponivel(hddResId.Value, 0) 'hddIntId.Value)
                gdvReserva11.DataSource = lista
                gdvReserva11.DataBind()
                gdvReserva11.SelectedIndex = -1
            End If
        Next
    End Sub
    Protected Sub MascaraTelefone()
        Dim Base_txtResCelular As String = txtResCelular.Text.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
        Dim Base_txtResFax As String = txtResFax.Text.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
        Dim Base_txtResFoneComercial As String = txtResFoneComercial.Text.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
        Dim Base_txtResFoneResidencial As String = txtResFoneResidencial.Text.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")

        If Base_txtResCelular.Trim.Length = 10 Then
            txtResCelular.Text = "(" & Mid(Base_txtResCelular, 1, 2) & ")" & Mid(Base_txtResCelular, 3, 4) + "-" + Mid(Base_txtResCelular, 7, 4)
        ElseIf Base_txtResCelular.Trim.Length = 11 Then
            txtResCelular.Text = "(" & Mid(Base_txtResCelular, 1, 2) & ")" & Mid(Base_txtResCelular, 3, 5) + "-" + Mid(Base_txtResCelular, 8, 4)
        End If

        If Base_txtResFax.Trim.Length = 10 Then
            txtResFax.Text = "(" & Mid(Base_txtResFax, 1, 2) & ")" & Mid(Base_txtResFax, 3, 4) + "-" + Mid(Base_txtResFax, 7, 4)
        ElseIf Base_txtResFax.Trim.Length = 11 Then
            txtResFax.Text = "(" & Mid(Base_txtResFax, 1, 2) & ")" & Mid(Base_txtResFax, 3, 5) + "-" + Mid(Base_txtResFax, 8, 4)
        End If

        If Base_txtResFoneComercial.Trim.Length = 10 Then
            txtResFoneComercial.Text = "(" & Mid(Base_txtResFoneComercial, 1, 2) & ")" & Mid(Base_txtResFoneComercial, 3, 4) + "-" + Mid(Base_txtResFoneComercial, 7, 4)
        ElseIf Base_txtResFoneComercial.Trim.Length = 11 Then
            txtResFoneComercial.Text = "(" & Mid(Base_txtResFoneComercial, 1, 2) & ")" & Mid(Base_txtResFoneComercial, 3, 5) + "-" + Mid(Base_txtResFoneComercial, 8, 4)
        End If

        If Base_txtResFoneResidencial.Trim.Length = 10 Then
            txtResFoneResidencial.Text = "(" & Mid(Base_txtResFoneResidencial, 1, 2) & ")" & Mid(Base_txtResFoneResidencial, 3, 4) + "-" + Mid(Base_txtResFoneResidencial, 7, 4)
        ElseIf Base_txtResFoneResidencial.Trim.Length = 11 Then
            txtResFoneResidencial.Text = "(" & Mid(Base_txtResFoneResidencial, 1, 2) & ")" & Mid(Base_txtResFoneResidencial, 3, 5) + "-" + Mid(Base_txtResFoneResidencial, 8, 4)
        End If
    End Sub

    Public Function GeraBoletoNet(CNPJ As String, CodBanco As String, Cedente As String, Vencimento As String, AgeCodCed As String,
                                CodCedente As String, DigAgencia As String, NossoNumero As String,
                                VlrDoc As String, DetalheValor As String, DataDocumento As String, DataProces As String,
                                MensagemSescBoletoSacado As String, MensagemSescBoletoCedente As String, Destino As String,
                                ResCaracteristica As String, TipoParcela As String, ResId As String, NossoNumeroCompleto As String) As String

        'Informar os dados do cedente
        Dim c = New BoletoNet.Cedente(CNPJ, Cedente, AgeCodCed, DigAgencia, "000000") 'Não estou passando o número da conta, pois no boleto não usa.


        'Dependendo da carteira, é necessário informar o código do cedente (o banco que fornece)
        c.Codigo = CodCedente ' (535475) cedente de caldas novas passado pelo Morandi (535488) é o de Pirenópolis

        'Carteria estava 14 - Com Registro e foi pedido para passar para 24- Sem Registro
        Dim b = New BoletoNet.Boleto(Format(CDate(Vencimento), "dd/MM/yyyy"), CDec(VlrDoc), "24", NossoNumero.Trim, c) '24' - SR sem registro

        b.Aceite = "A"
        b.QuantidadeMoeda = 0

        '//Dependendo da carteira, é necessário o número do documento
        b.NumeroDocumento = Format(CDec(NossoNumeroCompleto.Trim), "00000000000") 'NossoNumeroCompleto.Trim  

        b.ValorCobrado = 0

        'Sacado

        If (ResCaracteristica = "E" Or ResCaracteristica = "T" Or ResCaracteristica = "P" Or ResCaracteristica = "F") Then
            If btnCaixa.Attributes.Item("BoletoIndividual") = "BoletoIndividual" Then
                'Se não possuir CPF cadastrado, irá pegar o CPFCNPJ do responsável pela reserva
                If btnCaixa.Attributes.Item("IntCPF") = "" Then
                    b.Sacado = New BoletoNet.Sacado(txtResCPF.Text.ToString.Replace(" ", "").Replace("/", "").Replace("-", "").Replace(".", ""), btnCaixa.Attributes.Item("sacado").ToUpper)
                Else
                    b.Sacado = New BoletoNet.Sacado(btnCaixa.Attributes.Item("IntCPF"), btnCaixa.Attributes.Item("sacado").ToUpper)
                End If
                btnCaixa.Attributes.Remove("BoletoIndividual")
                btnCaixa.Attributes.Remove("IntCPF")
            Else
                'b.Sacado = New BoletoNet.Sacado("03671444000147", txtResNome.Text.ToString.ToUpper)
                b.Sacado = New BoletoNet.Sacado(txtResCPF.Text.Trim.Replace(" ", ""), txtResNome.Text.ToString.ToUpper)
            End If
            Dim ComplementoEndereco As String = ""
            If txtResNumero.Text.Trim.Length > 0 And txtResNumero.Text.Trim <> "0" Then
                ComplementoEndereco += ",Nº " & txtResNumero.Text
            End If
            If txtResQuadra.Text.Trim.Length > 0 Then
                ComplementoEndereco += ",Qd-" & txtResQuadra.Text
            End If
            If txtResLote.Text.Trim.Length > 0 Then
                ComplementoEndereco += ",Lt-" & txtResLote.Text
            End If
            If txtResComplemento.Text.Trim.Length > 0 Then
                ComplementoEndereco += ",Complemento: " & txtResComplemento.Text
            End If
            b.Sacado.Endereco.End = txtResLogradouro.Text.Trim.ToUpper & ComplementoEndereco
            b.Sacado.Endereco.Bairro = txtResBairro.Text.Trim.ToUpper
            b.Sacado.Endereco.Cidade = txtResCidade.Text.Trim.ToUpper
            b.Sacado.Endereco.CEP = txtResCep.Text.Trim
            b.Sacado.Endereco.UF = cmbEstId.SelectedItem.ToString.ToUpper
            'b.Sacado.Endereco.End = "RUA19 Nº260".ToUpper
            'b.Sacado.Endereco.Bairro = "S.CENTRAL".ToUpper
            'b.Sacado.Endereco.Cidade = "GOIÂNIA"
            'b.Sacado.Endereco.CEP = "74030-090"
            'b.Sacado.Endereco.UF = "GO"
        Else
            b.Sacado = New BoletoNet.Sacado(txtResCPF.Text.Trim.Replace(" ", ""), txtResNome.Text.ToString.ToUpper)
            b.Sacado.Endereco.End = txtResLogradouro.Text.Trim.ToUpper
            b.Sacado.Endereco.Bairro = txtResBairro.Text.Trim.ToUpper
            b.Sacado.Endereco.Cidade = txtResCidade.Text.Trim.ToUpper
            b.Sacado.Endereco.CEP = txtResCep.Text.Trim
            b.Sacado.Endereco.UF = cmbEstId.SelectedItem.ToString.ToUpper
        End If

        '//Instrução.
        Dim i1 = New BoletoNet.Instrucao_Caixa(CInt(CodBanco))
        i1.Descricao = MensagemSescBoletoSacado

        i1.Descricao = i1.Descricao + "<br/><br/>SAC CAIXA: 0800 726 0101 (informações, reclamações, sugestões e elogios)"
        i1.Descricao = i1.Descricao + "<br/>Para pessoas com deficiência auditiva ou de fala: 0800 726 2492"
        i1.Descricao = i1.Descricao + "<br/>Ouvidoria: 0800 725 7474"
        i1.Descricao = i1.Descricao + "<br/>caixa.gov.br"

        b.Sacado.Instrucoes.Add(i1)

        Dim i2 = New BoletoNet.Instrucao_Caixa(CInt(CodBanco))
        i2.Descricao = MensagemSescBoletoCedente
        b.Cedente.Instrucoes.Add(i2)
        b.EspecieDocumento = New BoletoNet.EspecieDocumento_Caixa("3") 'Ds = Duplicata  de  Prestação  de  Serviços

        Dim bb = New BoletoNet.BoletoBancario()

        'Esse comando irá ocultar a parte de cima do boleto
        'bb.OcultarReciboSacado = True

        bb.CodigoBanco = CInt(CodBanco)
        bb.Boleto = b
        bb.MostrarCodigoCarteira = True
        bb.Boleto.Valida()
        Dim teste = b.DigitoNossoNumero

        'Calculo do Digito verificador - nosso número
        'Explicação do cálculo com detalhes esta logo abaixo no cálculo do dv do código de barras
        Dim BaseCalculoNNumero = b.Carteira & Format(CDec(NossoNumero.Trim), "000000000000000") '17Posições
        Dim DigitoVerificadorNossoNumero = CalculaDigitoVerificadorModulo11CEF(BaseCalculoNNumero, False)

        'Essa linha seria o caso de mudar o nosso número informando o DV do nosso número junto (Cesar Leonardi)
        'NossoNumero = NossoNumero & DigitoVerificadorNossoNumero

        'Calculo Digito Verificador Código do Cedente
        Dim BaseCalculoCedente = c.Codigo   '6 Posições
        Dim DigitoVerificadorCedente = CalculaDigitoVerificadorModulo11CEF(BaseCalculoCedente, False)

        b.NossoNumero = b.Carteira & "/" & Format(CDec(NossoNumero.Trim), "000000000000000") & "-" & DigitoVerificadorNossoNumero

        'Codigo do Cedente
        c.Codigo = c.Codigo & "-" & DigitoVerificadorCedente

        '//false -> Oculta o comprovante de entrega
        'bb.MostrarComprovanteEntrega = True

        'Dim Codigobarrateste = b.CodigoBarra.Codigo
        Dim CalculoVencimento = DateDiff(DateInterval.Day, CDate("1997-10-07"), CDate(Format(CDate(Vencimento), "yyyy-MM-dd"))) 'Calculo da da data de vencimento que vai no código de barras
        Dim NNumeroPart1 = Mid(Format(CDec(NossoNumero.Trim), "000000000000000"), 1, 3) & Mid(b.Carteira, 1, 1)
        Dim NNumeroPart2 = Mid(Format(CDec(NossoNumero.Trim), "000000000000000"), 4, 3) & Mid(b.Carteira, 2, 1)
        Dim NNumeroPart3 = Mid(Format(CDec(NossoNumero.Trim), "000000000000000"), 7, 10)
        Dim MontaCodBarras As String = b.Banco.Codigo & b.Moeda.ToString & CalculoVencimento & Format(CDec(b.ValorBoleto.ToString.Replace(",", "")), "0000000000") _
                                       & c.Codigo.Replace("-", "") & NNumeroPart1 & NNumeroPart2 & NNumeroPart3

        'Calculando o Digito Verificador Livre do Código de Barras (Ultimo valor do código de barras a ser impresso)
        'CampoLivre = CodigoLivre (CEF) Só Observação pessoal
        Dim BaseDvCampoLivre = Mid(MontaCodBarras, 19, 24)
        Dim DigitoVerificadorDvCampoLivre = CalculaDigitoVerificadorModulo11CEF(BaseDvCampoLivre, False)

        'Adicionando o dígito verificar do campo livre (Esse valor fica na ultima posição do codigo de barras)
        MontaCodBarras = MontaCodBarras & DigitoVerificadorDvCampoLivre

        'Calculando o Digito Verificador GERAL do Código de barras que ficará na posição 5
        Dim BaseCalculoDvGeralCodBarra = MontaCodBarras
        Dim DvGeralBarra = CalculaDigitoVerificadorModulo11CEF(BaseCalculoDvGeralCodBarra, True)

        'Esse é o código de barras completo já para impressão
        MontaCodBarras = Mid(MontaCodBarras, 1, 4) & DvGeralBarra & Mid(MontaCodBarras, 5, 40)
        b.CodigoBarra.Codigo = MontaCodBarras

        '========================== Gerando o número digitável do código de barras==============================
        'Calculando o Digitos Verificador de cada um dos 3 campos acima
        Dim BaseLinhaDigitavel As String = MontaCodBarras.Trim
        Dim Campo1LinhaDigitavel = Mid(BaseLinhaDigitavel, 1, 4) & Mid(BaseLinhaDigitavel, 20, 5)
        Dim Campo2LinhaDigitavel = Mid(BaseLinhaDigitavel, 25, 10)
        Dim Campo3LinhaDigitavel = Mid(BaseLinhaDigitavel, 35, 10)

        Campo1LinhaDigitavel = Campo1LinhaDigitavel & CalculaDVerificadorLinhaDigitavel(Campo1LinhaDigitavel.ToString)
        Campo2LinhaDigitavel = Campo2LinhaDigitavel & CalculaDVerificadorLinhaDigitavel(Campo2LinhaDigitavel.ToString)
        Campo3LinhaDigitavel = Campo3LinhaDigitavel & CalculaDVerificadorLinhaDigitavel(Campo3LinhaDigitavel.ToString)

        Campo1LinhaDigitavel = Mid(Campo1LinhaDigitavel, 1, 5) & "." & Mid(Campo1LinhaDigitavel, 6, 5)
        Campo2LinhaDigitavel = Mid(Campo2LinhaDigitavel, 1, 5) & "." & Mid(Campo2LinhaDigitavel, 6, 6)
        Campo3LinhaDigitavel = Mid(Campo3LinhaDigitavel, 1, 5) & "." & Mid(Campo3LinhaDigitavel, 6, 6)

        Dim LinhaDigitavelCompleta = Campo1LinhaDigitavel & " " & Campo2LinhaDigitavel & " " & Campo3LinhaDigitavel & " " & DvGeralBarra & " " & CalculoVencimento & Format(CDec(b.ValorBoleto.ToString.Replace(",", "")), "0000000000")
        'Linha digitavel completa para impressão
        b.CodigoBarra.LinhaDigitavel = LinhaDigitavelCompleta

        'Montando o Html já pronto para impressão
        'Dim s As String = bb.MontaHtml.Replace("0012/535475-7-8", AgeCodCed & "/" & c.Codigo)

        '"0012/535475-7-8" Caldas Novas
        '"0012/535488-9-1" Pirenopolis
        Dim s As String = bb.MontaHtmlEmbedded.Replace("0012/535475-7-8", AgeCodCed & "/" & c.Codigo).Replace _
                          ("EcdBar Al pL19""" & "><img", "EcdBar Al pL19""" & "> <img class=""EcdBar Al""").Replace("24 -", "SR") _
                          .Replace("0012/535488-9-1", AgeCodCed & "/" & c.Codigo)


        If File.Exists("\\SQL-CTL\c$\BoletosTemp\" & NossoNumero & ".html") Then
            File.Delete("\\SQL-CTL\c$\BoletosTemp\" & NossoNumero & ".html")
        End If


        Select Case Destino
            Case Is = "C"
                Destino = "C" 'Caldas
            Case 1
                Destino = "P" 'Pirenopolis
        End Select

        If Session.Item("LinhaDigitavel") > "" Then
            Session.Add("LinhaDigitavel", Session.Item("LinhaDigitavel") & "<br/>" & bb.Boleto.CodigoBarra.LinhaDigitavel)
        Else
            Session.Add("LinhaDigitavel", bb.Boleto.CodigoBarra.LinhaDigitavel)
        End If

        'Gravando o CodBarra,CodDigitavel e o Cedente na base de dados para futuras consultas (Somente para reservas individuais)
        objBoletoNetDAO = New BoletoNetDAO
        Select Case objBoletoNetDAO.AtualizaCodBarraNovoBoleto(NossoNumero.ToString, bb.Boleto.CodigoBarra.Codigo, bb.Boleto.CodigoBarra.LinhaDigitavel, AgeCodCed & CodCedente, Destino, TipoParcela, s, ResId)
            Case Is = 0
                enviarEmailGenerico("Houve um erro ao salvar os dados dos boleto da reserva " & ResId & "Mensagem referente à função ObjBoletoNetDAO.AtualizaCodBarraNovoBoleto responsável por substituir o arquivo htlm e gravar os logs no banco.")
        End Select
        'End If

        'Obs.: a Geração em Pdf do Boleto esta no clique de cada imagem
        Return s
    End Function
    Public Function CalculaDigitoVerificadorModulo11CEF(BaseCalculo As String, CodigoLivre As Boolean) As Long
        'O Módulo 11 - faz um calculo de 2 a 9 (2,3,4,5...) - Mariores informações no arquivo da caixa.
        Dim TotPassada = BaseCalculo.Trim.Length
        Dim BaseCopia = TotPassada
        Dim ValorAtualMutiplicacao = 0
        Dim SomaBaseCalculo = 0
        Dim FatorMultiplicacao = 2

        While BaseCopia > 0
            ValorAtualMutiplicacao = Mid(BaseCalculo, BaseCopia, 1) 'Separando o último caractere do valor passado para que haja a multiplicacao
            SomaBaseCalculo = SomaBaseCalculo + (ValorAtualMutiplicacao * FatorMultiplicacao) 'Multiplica o valor acima pelo fator de multiplicação que começa com 2 (vai até nove)
            If FatorMultiplicacao = 9 Then 'Se o fator for 9 retorna para 2 se não for soma 1 até chegar aos 9
                FatorMultiplicacao = 2
            Else
                FatorMultiplicacao += 1
            End If
            BaseCopia -= 1 'Diminui 1 caracter na base para pegar o prossimo caractere da direita para a esquerad.
        End While

        Dim DivisaoBaseCalculo = SomaBaseCalculo / 11 'Fazendo a divisão da soma por 11 (Modulo 11)
        Dim Resto = SomaBaseCalculo Mod 11 'Retirando o resto da divisão para análise
        Dim DigitoVerificador = 0
        If SomaBaseCalculo < 11 Then 'Regra: Quando a soma da multiplicação for menor que 11, retiro a soma diretamente 11-Soma = Digito Verificador
            DigitoVerificador = 11 - SomaBaseCalculo
        Else
            If CodigoLivre = False Then 'Se não for calculo do código verificador do código livre, faremos o passo a seguir para chegar ao digito verificador
                If 11 - Resto > 9 Then
                    DigitoVerificador = 0
                Else
                    DigitoVerificador = 11 - Resto
                End If
            Else
                If (11 - Resto > 9) Or (11 - Resto = 0) Then 'Exclusivo para calculo do código livre que não pode ser igual a 0
                    DigitoVerificador = 1
                Else
                    DigitoVerificador = 11 - Resto
                End If
            End If
        End If
        Return DigitoVerificador
    End Function

    Public Function CalculaDVerificadorLinhaDigitavel(BaseCalculo As String) As Long
        'Calculando o Digitos Verificador de cada um dos 3 campos acima
        'Aplica-se a esse cálculo o Módulo 10 da CEF - Multiplicação por 2,1,2,1,2..... sucessivamente
        'Manual em pdf será guardado dentro da pasta do turweb.
        Dim BaseLinhaDigitavel = BaseCalculo 'String com os valores do código de barras para geração da linha digitável

        Dim TotPassadaCampo = BaseCalculo.ToString.Length
        Dim BaseCopiaCampo = TotPassadaCampo
        Dim ValorMultiplicacaoCampo = 0
        Dim SomaValorCampo = 0
        Dim FatorMultiplicacaoCampo = 2

        While BaseCopiaCampo > 0
            ValorMultiplicacaoCampo = Mid(BaseCalculo, BaseCopiaCampo, 1)
            'Quando o resultado da multiplicação possuir mais de dois algarismos, o mesmo deve ser somado ex.: 14  (1 + 4 = 5)
            Dim ResultadoMultiplicacao = (ValorMultiplicacaoCampo * FatorMultiplicacaoCampo)
            If (ResultadoMultiplicacao) > 9 Then
                Dim Auxiliar = CInt(Mid(ResultadoMultiplicacao, 1, 1)) + CInt(Mid(ResultadoMultiplicacao, 2, 1))
                SomaValorCampo = SomaValorCampo + Auxiliar
            Else
                SomaValorCampo = SomaValorCampo + ResultadoMultiplicacao
            End If

            If FatorMultiplicacaoCampo = 2 Then
                FatorMultiplicacaoCampo = 1
            Else
                FatorMultiplicacaoCampo += 1
            End If
            BaseCopiaCampo -= 1
        End While

        'Dim DivisaoCampo1 = SomaValorCampo / 10
        Dim RestoDvCampo = SomaValorCampo Mod 10
        Dim DvCampoLinhaDigitavel = 0

        If SomaValorCampo < 10 Then
            DvCampoLinhaDigitavel = 10 - SomaValorCampo
        ElseIf RestoDvCampo = 0 Then
            DvCampoLinhaDigitavel = 0
        Else
            DvCampoLinhaDigitavel = 10 - RestoDvCampo
        End If

        Return DvCampoLinhaDigitavel
    End Function

    Protected Sub enviarEmailGenerico(Mensagem As String)
        Try
            Dim objEmail As New System.Net.Mail.MailMessage()
            objEmail.Subject = "Sistema de Reserva (Mensagem de erro) "
            objEmail.To.Add(New System.Net.Mail.MailAddress("elvis.irineu@sescgo.com.br"))
            objEmail.CC.Add(New System.Net.Mail.MailAddress("gustavo.cesar@sescgo.com.br"))
            'objEmail.To.Add(New MailAddress(objUsuarioRedeVO.mail))
            objEmail.IsBodyHtml = True

            Dim objSmtp As SmtpClient

            '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
            'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
            objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
            objSmtp.EnableSsl = True
            objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
            objSmtp.UseDefaultCredentials = False
            objSmtp.Timeout = 5000


            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    objEmail.From = New System.Net.Mail.MailAddress("reservas.caldasnovas@sescgo.com.br ")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    objEmail.From = New System.Net.Mail.MailAddress("reservas.pirenopolis@sescgo.com.br ")
            End Select


            Dim sEmail As New StringBuilder
            sEmail.Append("<p />" & Mensagem)
            objEmail.IsBodyHtml = True
            objEmail.Body = sEmail.ToString


            objSmtp.Send(objEmail)
        Catch ex As Exception
            Response.Redirect("erroEmail.aspx?erro=Ocorreu um erro em sua solicitação&excecao=" & ex.StackTrace.ToString, False)
        End Try
    End Sub
    Protected Sub GravaLog(ByVal msg As String)
        If User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "") = "flavia.costa" Then
            Dim dt As DateTime = Now
            Dim arquivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "LOG" + Format("{0:yyyyMMdd}", dt) + ".TXT"
            Dim objStream As New FileStream(arquivo, FileMode.Append)
            Dim arq As New StreamWriter(objStream)
            arq.Write(Format("{0:HH:mm:ss}", dt) + " " + msg + vbCrLf)
            arq.Close()
        End If
    End Sub

    Protected Sub imgBoletoSinal_Click(sender As Object, e As ImageClickEventArgs) Handles imgBoletoSinal.Click
        Try
            'Preenchimento obrigatório para impressão dos boletos'
            If ((txtResNumero.Text = "0" Or txtResNumero.Text.Trim.Length = 0) _
                And txtResQuadra.Text.Trim.Length = 0 _
                And txtResLote.Text.Trim.Length = 0 _
                And txtResComplemento.Text.Trim.Length = 0) Then
                Mensagem("Um dos campos (Numero, Quadra, Lote ou complemento) do responsável terá que ser informado.")
                Exit Try
            End If

            If txtResBairro.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o Bairro do responsável.")
                txtResBairro.Focus()
                Exit Try
            End If
            If txtResCep.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar o CEP do responsável.")
                txtResCep.Focus()
                Exit Try
            End If
            If txtResCidade.Text.Trim.Length = 0 Then
                Mensagem("Faltou infomar a Cidade do responsável.")
                txtResCidade.Focus()
                Exit Try
            End If

            If txtResDtGrupoPgtoSinal.Text.Trim.Length = 0 Then
                Mensagem("Faltou informar a data do sinal.")
                txtResDtGrupoPgtoSinal.Focus()
                Exit Try
                'Verifica a data de vencimento do boleto, se dará prazo de fazer o registro e alertar o usuário sobre...
            ElseIf Date.Today.DayOfWeek = 6 And DateDiff(DateInterval.Day, Date.Today, CDate(txtResDtGrupoPgtoSinal.Text)) < 5 Then 'Sabado
                Mensagem("A data do campo: Pagar sinal até não poderá ser inferior a: " & Format(DateAdd(DateInterval.Day, 4, Date.Today), "dd/MM/yyyy"))
                txtResDtGrupoPgtoSinal.Focus()
                Exit Try
            ElseIf Date.Today.DayOfWeek = 0 And DateDiff(DateInterval.Day, Date.Today, CDate(txtResDtGrupoPgtoSinal.Text)) < 4 Then 'Domingo
                Mensagem("A data do campo: Pagar sinal até não poderá ser inferior a: " & Format(DateAdd(DateInterval.Day, 3, Date.Today), "dd/MM/yyyy"))
                txtResDtGrupoPgtoSinal.Focus()
                Exit Try
            ElseIf Date.Today.DayOfWeek = 1 And DateDiff(DateInterval.Day, Date.Today, CDate(txtResDtGrupoPgtoSinal.Text)) < 3 Then 'Segunda
                Mensagem("A data do campo: Pagar sinal até não poderá ser inferior a: " & Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy"))
                txtResDtGrupoPgtoSinal.Focus()
                Exit Try
            ElseIf (Date.Today.DayOfWeek = 2 Or Date.Today.DayOfWeek = 3 Or Date.Today.DayOfWeek = 4 Or Date.Today.DayOfWeek = 5) And DateDiff(DateInterval.Day, Date.Today, CDate(txtResDtGrupoPgtoSinal.Text)) < 3 Then
                Mensagem("A data do campo: Pagar sinal até não poderá ser inferior a: " & Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy"))
                txtResDtGrupoPgtoSinal.Focus()
                Exit Try
            End If

            Dim PagarAposDia As String = ""
            If Date.Today.DayOfWeek = 6 Then 'Sabado
                PagarAposDia = Format(DateAdd(DateInterval.Day, 4, Date.Today), "dd/MM/yyyy")
            ElseIf Date.Today.DayOfWeek = 0 Then 'Domingo
                PagarAposDia = Format(DateAdd(DateInterval.Day, 3, Date.Today), "dd/MM/yyyy")
            ElseIf Date.Today.DayOfWeek = 1 Then 'Segunda
                PagarAposDia = Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy")
            ElseIf (Date.Today.DayOfWeek = 2 Or Date.Today.DayOfWeek = 3 Or Date.Today.DayOfWeek = 4 Or Date.Today.DayOfWeek = 5) Then 'De terça em diante
                PagarAposDia = Format(DateAdd(DateInterval.Day, 2, Date.Today), "dd/MM/yyyy")
            End If

            If txtResCPF.Text = "00.000.000/0000-00" Or txtResCPF.Text = "000.000.000-00" Then
                Mensagem("CPF ou CNPJ inválido!")
                txtResCPF.Focus()
                Exit Try
            End If

            ''▓ ================== DAQUI PARA BAIXO ERA ONDE EU GERAVA O BOLETO SEM REGISTRO =======================▓
            'Dim Integrante As String = ""
            Dim Destino As String
            Dim NomeConexao As String
            objBoletoNetDAO = New BoletoNetDAO
            objBoleto1NetVO = New BoletoNetVO
            Dim ResCaracteristica As String = ""
            Dim Horario = "*Entrada às " & objBoleto1NetVO.Entrada & " e Saída às " & objBoleto1NetVO.Saida
            Dim Demonstrativo As String = ""
            Dim Refeicoes As String = ""
            Dim MensagemSescBoletoSacado As String = ""
            Dim MensagemSescBoletoCedente As String = ""
            Dim CodigoCedenteNovo = "", TipoDeParcelaBoleto As String = "U"

            'Atualiza o valor do boleto em TbBoletosImp
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialCaldas")
                Destino = "C"
                NomeConexao = "TurismoSocialCaldas"
            Else
                objBoletoDAO = New Turismo.BoletoDAO("TurismoSocialPiri")
                Destino = "P"
                NomeConexao = "TurismoSocialPiri"
            End If

            'Verifica se já existe um boleto do tipo P - Primeira parcela criado para esse grupo com CNPJ
            Dim BoletoGeradoGrupo As String = objBoletoNetDAO.VerificaExistenciaBoletoGrupo(hddResId.Value, Destino)
            If BoletoGeradoGrupo.Length > 9 Then
                Mensagem("Existe um boleto registrado para essa reserva de grupo.\n\nO mesmo será aberto para impressão.")
                Dim pathGrupo = (Request.PhysicalApplicationPath & "BoletosTemp\" & BoletoGeradoGrupo & ".pdf")

                Dim pathGrupoOpen = ("BoletosTemp/" & BoletoGeradoGrupo & ".pdf")
                'Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero
                'Dim pathgrupoOpen = ("BoletosTemp/" & BoletoGeradoGrupo & ".pdf")
                If File.Exists(pathGrupo) Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & pathGrupoOpen & "');", True)
                End If
                Exit Try
            End If

            'hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))
            CodigoCedenteNovo = objBoletoNetDAO.RetornaCodCedenteNovo(Destino)

            If InStr("IET", hddResCaracteristica.Value) = 2 Then 'Grupo
                Dim LocalDestino As String = ""
                If cmbDestino.SelectedItem.Text = "0" Then
                    LocalDestino = cmbDestinoCidade.SelectedItem.Text & " " & cmbDestinoEstado.SelectedItem.Text
                Else
                    LocalDestino = cmbDestino.SelectedItem.Text
                End If
                ResCaracteristica = hddResCaracteristica.Value

                'Irei capturar o valor dos 50% do grupo
                objBoletoNetDAO = New BoletoNetDAO
                objBoleto1NetVO = New BoletoNetVO
                objBoleto1NetVO = objBoletoNetDAO.GeraSinalGrupo(Destino, hddResId.Value)
                Dim ValorSinal = objBoleto1NetVO.VlrDoc

                objBoleto1NetVO = New BoletoNetVO
                'Buscando os dados para a geração do boleto (Observei que o Percentual aqui não faz diferença na hora de garar o boleto, para definir o tipo de parcela na SP enviei 50%)
                objBoleto1NetVO = objBoletoNetDAO.GeraBoletoGrupo(hddResId.Value, "B", Format(CDate(txtResDtGrupoPgtoSinal.Text), "dd/MM/yyyy"), User.Identity.Name.Replace("SESC-GO.COM.BR\", ""), ValorSinal.ToString.Replace(".", "").Replace(",", "."), 50, Destino)

                objBoletoNetDAO = New BoletoNetDAO
                'Emissivo Eduardo - Excursão
                If txtDataInicialSolicitacao.Text <> txtDataFinalSolicitacao.Text Then
                    ResCaracteristica = hddResCaracteristica.Value
                    MensagemSescBoletoSacado = "<p vertical-align: bottom;><font size=2px;>Reserva de Nº " & objBoleto1NetVO.Reserva & " - Excursão de <font size=4px;font style='font-weight:bold;'>" & objBoleto1NetVO.Periodo & "</font>"


                    MensagemSescBoletoCedente = "Uso do Sesc: "
                    MensagemSescBoletoCedente += "<br/><font size=2px;>" & "Excursão de Nº " & objBoleto1NetVO.Reserva & " de <font size=3px;><b> " & objBoleto1NetVO.Periodo & "<b/></font>"
                    If ResCaracteristica <> "I" Then
                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & txtResNome.Text.Trim & "<font size=1px;font style='font-weight:normal;'>"
                        MensagemSescBoletoCedente += "<br/><font size=2px;font style='font-weight:bold;'>" & "Obs.: Pagar após o dia:" & PagarAposDia & "<font size=1px;font style='font-weight:normal;'>"
                    End If
                    MensagemSescBoletoCedente += "<br/><br/><p vertical-align: bottom;><font size=3px;><b>Sr.Caixa, NÃO RECEBER APÓS O VENCIMENTO</font><b/>"
                End If
            End If

            'Se for federação irá preencher o endereço com os dados do Sesc
            If cmbOrgId.SelectedValue = "37" Then
                ResCaracteristica = "F"
            End If

            'CodigoCedenteNovo = "0012535475"
            Dim NossoNumero = Mid(objBoleto1NetVO.NossoNum, 1, 10)
            Dim NossoNumeroCompleto = objBoleto1NetVO.NossoNum

            'AQUI NESSE LUGAR VOU PASSAR UM ObjBoletoVo com os dados de endereço completo do bolimpid
            Dim ObjBoletosUteis As New Uteis.BoletoVO
            With ObjBoletosUteis
                .ResNome = objBoleto1NetVO.ResNome
                .ResCPFCGC = objBoleto1NetVO.ResCPFCGC
                .ResLogradouro = objBoleto1NetVO.ResLogradouro
                .ResQuadra = objBoleto1NetVO.ResQuadra
                .ResLote = objBoleto1NetVO.ResLote
                .ResNumero = objBoleto1NetVO.ResNumero
                .ResCidade = objBoleto1NetVO.ResCidade
                .ResBairro = objBoleto1NetVO.ResBairro
                .ResCep = objBoleto1NetVO.ResCep
                .UF = objBoleto1NetVO.UF
            End With

            Dim s = GeraBoletoComRegistro.GeraBoletoHtmlPdf(ObjBoletosUteis, objBoleto1NetVO.Cedente.Trim, Mid(objBoleto1NetVO.CodBanco, 1, 3), "SESC ADM REGIONAL GO", objBoleto1NetVO.Vencimento, Mid(CodigoCedenteNovo, 1, 4), Mid(CodigoCedenteNovo, 5, 6), "4", NossoNumero, objBoleto1NetVO.VlrDoc,
                       objBoleto1NetVO.Banco, objBoleto1NetVO.DataDocumento, objBoleto1NetVO.DataProces, MensagemSescBoletoSacado, MensagemSescBoletoCedente, Destino, TipoDeParcelaBoleto, hddResId.Value, 0, NomeConexao, hddResId.Value)

            Session.Remove("LinhaDigitavel")

            'PRIMEIRO Boleto - No Final do Boleto original havia Espaço de 06 linhas inseridas, a rotina abaixo Retira a inserção de 4 linhas.
            Dim TInicio = Mid(s, 1, s.Length - 170)
            Dim TFim = Mid(s, s.Length - 29, 29)
            Dim BoletoPronto = TInicio & TFim
            'Devolvendo o boleto já sem as linhas no final
            s = BoletoPronto


            'Separando a linha digitavel
            Dim LinhaDigitavel = Mid(s, 1, InStr(s, "§") - 1)
            'Retorna somente o código HTML  
            s = Mid(s, InStr(s, "§") + 1, s.Length)
            Dim Arquivo = Request.PhysicalApplicationPath & "BoletosTemp\" & NossoNumero
            Dim pp = New System.Diagnostics.Process()
            pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
            pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            File.Create(Arquivo & ".html").Dispose()
            File.AppendAllText(Arquivo & ".html", s, Encoding.UTF8)

            pp.StartInfo.Arguments = "--zoom 2 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"
            pp.Start()
            pp.WaitForExit()
            pp.Close()
            pp.Dispose()
            'Teste na máquina local: Comentar o Delete e mudar no Path o .pdf para .html
            File.Delete(Arquivo & ".html")
            'Dim path = ("BoletosTemp/" & NossoNumero & ".html")
            Dim path = ("BoletosTemp/" & NossoNumero & ".pdf")
            ListaFinanceiroViaResId()
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
            'GravaLog("Turweb - Erro na geração de boleto, ResId ='" & objBoleto1NetVO.Reserva & "' Erro:'" & ex.StackTrace.ToString.Replace(Chr(10), "") & "'  ")
            'enviarEmailGenerico("Turweb - Erro na geração de boleto, ResId ='" & objBoleto1VO.Reserva & "' Erro:'" & ex.StackTrace.ToString.Replace(Chr(10), "") & "'  ")
        End Try
    End Sub
    Protected Sub HospedeJaControleCamposIntegrantes()
        If btnHospedagemNova.Attributes.Item("HospedeJa") = "S" Then
            'Quando a categoria for usuário, a cobrança sempre será usuário
            If cmbIntCatId.SelectedValue = 4 Then
                cmbIntCatCobranca.SelectedValue = 4
                cmbIntCatCobranca.Enabled = False
                'Comerciário, forçara a cobrança ser comerciário.
            ElseIf cmbIntCatId.SelectedValue = 1 Then
                cmbIntCatCobranca.SelectedValue = 1
                cmbIntCatCobranca.Enabled = False
            Else
                cmbIntCatCobranca.Enabled = True
            End If
            txtIntMemorando.Text = ""
            cmbIntEmissor.SelectedIndex = 0
            txtIntMemorando.Enabled = False
            cmbIntEmissor.Enabled = False
            imgBtnAlterarMemorando.Enabled = False
            cmbIntFormaPagamento.Enabled = False
            cmbIntCatId.Enabled = False
        End If
    End Sub
    Private Function TrocarAlmocoPelaJanta() As Boolean
        'Com pagamento no boleto e vencimento no dia da reserva
        Dim objTestaGrupo As New Uteis.TestaUsuario
        Dim objListaGrupo As New Uteis.TestaUsuario
        Dim varGrupos = objListaGrupo.listaGrupos(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")).ToUpper

        If hddResTipo.Value = "F" Then
            Return True 'Deixa mudar
        ElseIf ((ckbRefeicao.Items.Item(0).Selected = False And ckbRefeicao.Items.Item(2).Selected = True) And
               Not (varGrupos.Contains("TURISMO SOCIAL RESERVA TROCAR REFEICOES") Or varGrupos.Contains("TURISMO SOCIAL PIRI RESERVA TROCAR REFEICOES"))) Then
            Return False 'Não deixa mudar o almoço
        ElseIf ((ckbRefeicao.Items.Item(1).Selected = False And ckbRefeicao.Items.Item(3).Selected = True) And
            Not (varGrupos.Contains("TURISMO SOCIAL RESERVA TROCAR REFEICOES") Or varGrupos.Contains("TURISMO SOCIAL PIRI RESERVA TROCAR REFEICOES"))) Then
            Return False 'Não deixa mudar a janta
        Else
            Return True 'Deixa mudar ou adicionar Serviço Extra de Diária
        End If
    End Function

    Protected Sub ckbRefeicao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ckbRefeicao.SelectedIndexChanged
        If TrocarAlmocoPelaJanta() = False Then
            ckbRefeicao.Items.Item(0).Selected = True
            ckbRefeicao.Items.Item(1).Selected = True
            ckbRefeicao.Items.Item(2).Selected = False
            ckbRefeicao.Items.Item(3).Selected = False
            Mensagem("Usuário sem permissão para efetuar a troca de refeições.")
        End If
    End Sub

    Protected Sub txtResEmail_TextChanged(sender As Object, e As EventArgs) Handles txtResEmail.TextChanged
        If txtResEmail.Text > "" Then
            If EmailAddressCheck(txtResEmail.Text.Trim) = False Then
                Mensagem("E-mail inválido!")
                txtResEmail.Text = ""
                txtResEmail.Focus()
            Else
                txtResDtNascimento.Focus()
            End If
        Else
            Mensagem("O preenchimento do e-mail é obrigatório!")
            txtResEmail.Focus()
        End If
    End Sub

    Protected Sub btnRelFinanceiro_Click(sender As Object, e As ImageClickEventArgs) Handles btnRelFinanceiro.Click
        Response.Redirect("~/Financeiro/frmPagametosCartoesCredito.aspx")
    End Sub
    Public Sub ComprovantePagamento(Tipo As String)
        Try
            Dim Destino As String = ""
            'Atualiza o valor do boleto em TbBoletosImp
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Destino = "C"
            Else
                Destino = "P"
            End If

            Dim SituacaoReserva As String = ""
            Select Case ObjReservaConsultasVO.ResStatus
                Case "C"
                    SituacaoReserva = "Cancelada"
                Case "E"
                    SituacaoReserva = "Estada"
                Case "F"
                    SituacaoReserva = "Finalizada"
                Case "I"
                    SituacaoReserva = "Pendente de Integrante"
                Case "P"
                    SituacaoReserva = "Pendente de Pagamento"
                Case "R"
                    SituacaoReserva = "Confirmada"
                Case "S"
                    SituacaoReserva = "Solicitada"
            End Select

            Dim sEmail As New StringBuilder
            With ObjReservaConsultasVO
                sEmail.Clear()
                sEmail.AppendLine("<!DOCTYPE html> ")
                sEmail.AppendLine("<html xmlns=" & "http://www.w3.org/1999/xhtml" & "> ")
                sEmail.AppendLine("<head> ")
                sEmail.AppendLine("    <title>SESC - Confirmação de Pagamento</title> ")
                'sEmail.AppendLine("    <link href=" & "EnvioEmail.css" & " rel=" & "stylesheet />  ")
                sEmail.AppendLine("    <meta http-equiv=" & "content-type" & " content=" & "text/html;charset=ISO-8859-1 /> ")
                sEmail.AppendLine("<style type=" & "text/css> ")
                sEmail.AppendLine("	.texto {color:#B000FF;} ")
                sEmail.AppendLine("        .FormataTitulo{ ")
                sEmail.AppendLine("           background-color:#7EC0EE; ")
                sEmail.AppendLine("           font-size:15px; ")
                sEmail.AppendLine("           font-family:'Times New Roman'; ")
                sEmail.AppendLine("           color:black; ")
                sEmail.AppendLine("           height:20px; ")
                sEmail.AppendLine("           text-align:center; ")
                sEmail.AppendLine("           vertical-align:central; ")
                sEmail.AppendLine("           font-weight:bold; ")
                sEmail.AppendLine("           border-color: Background; ")
                sEmail.AppendLine("           border-width: 1px; ")
                sEmail.AppendLine("           -moz-border-radius: 20px 20px; /* Para Firefox */ ")
                sEmail.AppendLine("           -webkit-border-radius: 20px 20px; /*Para Safari e Chrome */ ")
                sEmail.AppendLine("           border-radius: 20px 20px; ")
                sEmail.AppendLine("         } ")
                sEmail.AppendLine("	.FormataTituloMenor{ ")
                sEmail.AppendLine("    	      background-color:#7EC0EE; ")
                sEmail.AppendLine("	          font-size:15px; ")
                sEmail.AppendLine("    	      font-family:'Times New Roman'; ")
                sEmail.AppendLine("           color:black; ")
                sEmail.AppendLine("           height:20px; ")
                sEmail.AppendLine("           text-align:center; ")
                sEmail.AppendLine("           vertical-align:central; ")
                sEmail.AppendLine("           font-weight:bold; ")
                sEmail.AppendLine("           border-color: Background; ")
                sEmail.AppendLine("           border-width: 1px; ")
                sEmail.AppendLine("           -moz-border-radius: 20px 20px; /* Para Firefox */ ")
                sEmail.AppendLine("           -webkit-border-radius: 20px 20px; /*Para Safari e Chrome */ ")
                sEmail.AppendLine("           border-radius: 20px 20px; ")
                sEmail.AppendLine("           } ")
                sEmail.AppendLine("        .ELinhas{ ")
                sEmail.AppendLine("    	      font-family:'Times New Roman'; ")
                sEmail.AppendLine("    	      font-size:10px; ")
                sEmail.AppendLine("           text-align:center; ")
                sEmail.AppendLine("           } ")
                sEmail.AppendLine("        .ELinhasLeft{ ")
                sEmail.AppendLine("    	      font-family:'Times New Roman'; ")
                sEmail.AppendLine("    	      font-size:10px; ")
                sEmail.AppendLine("           text-align:left; ")
                sEmail.AppendLine("           } ")
                sEmail.AppendLine("</style> ")
                sEmail.AppendLine("</head> ")
                sEmail.AppendLine("<body style=" & "text-align: center;" & " > ")

                ObjReservaConsultasVO.StatusPagamento = "CON"

                sEmail.AppendLine("    <div class=" & "FormataTitulo" & ">COMPROVANTE DE RESERVA</div> ")
                If Tipo = "" And hddResCaracteristica.Value = "I" Then
                    sEmail.AppendLine("    <p class=" & "ELinhas" & " style=" & "font-weight:bold" & ">Olá, " & .BolImpSacado.ToUpper & "!<p/> ")
                End If
                sEmail.AppendLine("    <p class=" & "ELinhas" & ">Informamos que a solicitação de reserva nº <b>" & .ResId & "</b>, está confirmada. <p/> ")
                If Tipo = "" Then
                    sEmail.AppendLine("    <p class=" & "ELinhas" & ">O status atual da reserva é:<b> " & SituacaoReserva & "</b><p/> ")
                End If

                sEmail.AppendLine("    <p class=" & "ELinhas" & ">Acesse o site Https://turweb.sescgo.com.br</a> para consultar suas solicitações de reserva. <p/> ")

                sEmail.AppendLine("    <div class=" & "FormataTituloMenor" & ">&nbsp;&nbsp;DADOS DA RESERVA</div> ")
                sEmail.AppendLine("    <p class=" & "ELinhas" & ">Reserva nº <b>" & .ResId & "</b> Período de: <font size=2px; font style='font-weight:bold;'> " & .BolImpDtPagamento & " </font>  <p/> ")
                If Destino = "C" Then 'Caldas Novas
                    sEmail.AppendLine("    <p class=" & "ELinhas" & ">Bloco: " & "<font size=2px; font style='font-weight:bold;'> " & .BloId & " </font>  <p/> ")
                    sEmail.AppendLine("    <p class=" & "ELinhas" & " style=" & "font-weight:bold;font-size:25px" & ">Sesc de Caldas Novas<p/> ")
                    sEmail.AppendLine("    <p class=" & "ELinhas" & ">" & .QtdPessoas & " pessoas - Entrada às 14h e saída às 12h.<p/> ")
                    If Tipo <> "" Then
                        sEmail.AppendLine("    <p class=" & "ELinhas" & "><b>Integrante(s): </b> <p/> ")
                        sEmail.AppendLine("    <p class=" & "ELinhas" & "> " & .Integrantes & " <p/> ")
                    End If
                Else
                    sEmail.AppendLine("    <p class=" & "ELinhas" & " style=" & "font-weight:bold;font-size:25px" & ">Pousada Sesc Pirenópolis<p/> ")
                    sEmail.AppendLine("    <p class=" & "ELinhas" & "><b>" & .QtdPessoas & " pessoas </b> - Entrada às 14h e saída às 12h.<p/> ")
                    If Tipo <> "" Then
                        sEmail.AppendLine("    <p class=" & "ELinhas" & "><b>Integrante(s): </b><p/> ")
                        sEmail.AppendLine("    <p class=" & "ELinhas" & "> " & .Integrantes & " <p/> ")
                    End If
                End If

                sEmail.AppendLine("<p class=" & "ELinhasLeft" & "><b>Obrigatória a apresentação de: </b><br/> ")
                sEmail.AppendLine("&nbsp&nbsp&nbsp•&nbsp Documento de identificação pessoal (com foto a partir de 12 anos) de todos os integrantes da reserva; <br/> ")
                sEmail.AppendLine("&nbsp&nbsp&nbsp•&nbsp Carteira Sesc atualizada (para reservas na categoria COMERCIÁRIO ou CONVENIADO Goiás); <br/> ")
                sEmail.AppendLine("&nbsp&nbsp&nbsp•&nbsp Documento do veículo (se tiver interesse em estacionar algum veículo no estacionamento<br/>&nbsp&nbsp&nbsp&nbsp&nbsp interno da unidade). <p/> ")

                sEmail.AppendLine("    <p class=" & "ELinhas" & ">A constatação de discrepância gerará alteração do valor e cobrança da diferença.<p/> ")
                sEmail.AppendLine("    <p class=" & "ELinhas" & " style=" & "font-weight:bold" & "><i>Importante!Este documento poderá ser exigido no ato do check-in para prováveis confirmações.</i><p/> ")
                sEmail.AppendLine("    <p class=" & "ELinhas" & " style=" & "font-weight:bold" & "><i>Tenha este comprovante impresso, gravado no celular, tablet ou demais dispositivos eletrônicos que permitam sua visualização.</i><p/> ")
                sEmail.AppendLine("      <hr /> ")
                sEmail.AppendLine("</body> ")
                sEmail.AppendLine("</html> ")
                'End Select

                '<> = Voucher
                Dim Arquivo = ""
                If Tipo <> "" Then
                    Arquivo = Request.PhysicalApplicationPath & "ComprovanteTemp\Comprovante-" & Tipo
                Else
                    Arquivo = Request.PhysicalApplicationPath & "ComprovanteTemp\Comprovante-" & .ResId
                End If

                'Convertendo o boleto para PDF e abrindo em uma nova aba
                Dim pp = New System.Diagnostics.Process()
                pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
                pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                File.Create(Arquivo & ".html").Dispose()
                File.AppendAllText(Arquivo & ".html", sEmail.ToString, Encoding.UTF8)

                pp.StartInfo.Arguments = "--zoom 2 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"

                pp.Start()
                pp.WaitForExit()
                pp.Close()
                pp.Dispose()
                Dim path = ""
                If Tipo <> "" Then
                    File.Delete(Server.MapPath(".") & "..\ComprovanteTemp\Comprovante-" & Tipo & ".html")
                    path = ("../ComprovanteTemp/Comprovante-" & Tipo & ".pdf")
                Else
                    File.Delete(Server.MapPath(".") & "..\ComprovanteTemp\Comprovante-" & .ResId & ".html")
                    path = ("../ComprovanteTemp/Comprovante-" & .ResId & ".pdf")
                End If

                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)

            End With

        Catch ex As Exception
        End Try
    End Sub
    Public Sub ComprovantePagamentoTemplate()
        Try

            Dim sUnidade, sPensao, sAlmocoChegada, sAlmocoSaida As String

            objNotificacaoTemplateVO = New NotificacaoTemplateVO

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objNotificacaoTemplateDAO = New NotificacaoTemplateDAO("TurismoSocialCaldas")
                ObjReservaDAO = New ReservaDAO("TurismoSocialCaldas")
                objReservaListagemSolicitacaoDAO = New ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                objReservaListagemAcomodacaoDAO = New ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
                'ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Sesc Caldas Novas"

                sUnidade = "Caldas Novas"
                sPensao = "Pensão Completa – café da manhã, almoço e jantar"

            Else
                objNotificacaoTemplateDAO = New NotificacaoTemplateDAO("TurismoSocialPiri")
                objReservaListagemSolicitacaoDAO = New ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
                ObjReservaDAO = New ReservaDAO("TurismoSocialPiri")
                objReservaListagemAcomodacaoDAO = New ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
                objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialPiri")
                'ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Pousada Sesc Pirenópolis"

                sUnidade = "Pirenópolis"
                sPensao = "Meia Pensão – café da manhã e buffet de caldo, sopa e creme à noite"

            End If

            Dim SituacaoReserva As String = ""
            Select Case ObjReservaConsultasVO.ResStatus
                Case "C"
                    SituacaoReserva = "Cancelada"
                Case "E"
                    SituacaoReserva = "Estada"
                Case "F"
                    SituacaoReserva = "Finalizada"
                Case "I"
                    SituacaoReserva = "Pendente de Integrante"
                Case "P"
                    SituacaoReserva = "Pendente de Pagamento"
                Case "R"
                    SituacaoReserva = "Confirmada"
                Case "S"
                    SituacaoReserva = "Solicitada"
            End Select

            ObjReservaVO = ObjReservaDAO.consultar(hddResId.Value)
            objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
            lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
            If lista.Count > 0 Then
                objReservaListagemAcomodacaoVO = lista.Item(0)
            End If

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-logo")
            Dim sLogo As New StringBuilder
            sLogo.Clear()
            sLogo.Append(objNotificacaoTemplateVO.TextoModelo)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-identificacao")
            Dim sIdentificacao As New StringBuilder
            sIdentificacao.Clear()
            sIdentificacao.Append(objNotificacaoTemplateVO.TextoModelo)
            sIdentificacao.Replace("{{Nome}}", ObjReservaVO.resNome)
            sIdentificacao.Replace("{{NumReserva}}", ObjReservaVO.resId)
            sIdentificacao.Replace("{{Status}}", SituacaoReserva)


            Dim valorTotal As Decimal = 0
            lista = objReservaListagemIntegranteDAO.consultarViaResId(hddResId.Value, "", "i.IntNome desc")
            For Each item As ReservaListagemIntegranteVO In lista
                If item.intFormaPagamento = "ER" Then
                    valorTotal = valorTotal + item.hosValorPago
                End If

                If String.IsNullOrEmpty(sAlmocoChegada) And String.IsNullOrEmpty(sAlmocoSaida) Then
                    If ObjReservaVO.resSistema = "portal-turismo" Then
                        If sUnidade = "Pirenópolis" Then
                            sAlmocoChegada = "Não"
                        Else
                            sAlmocoChegada = "Sim"
                        End If
                        sAlmocoSaida = "Não"
                    Else 'Balcão
                        If item.intAlmoco = "I" Or item.intAlmoco = "S" Then
                            sAlmocoChegada = "Sim"
                        Else
                            sAlmocoChegada = "Não"
                        End If
                        If item.intAlmoco = "O" Or item.intAlmoco = "S" Then
                            sAlmocoSaida = "Sim"
                        Else
                            sAlmocoSaida = "Não"
                        End If
                    End If
                End If
            Next

            Dim solid As String


            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-pagamento")
            Dim sPagamento As New StringBuilder
            sPagamento.Clear()
            If valorTotal > 0 Then
                sPagamento.Append(objNotificacaoTemplateVO.TextoModelo)

                sPagamento.Replace("{{ValorTotal}}", Format(CDec(valorTotal), "#,##0.00"))

                Dim sParcelamentos As String = ""
                lista = objReservaListagemFinanceiroDAO.consultarViaResId(hddResId.Value, "")

                For Each item As ReservaListagemFinanceiroVO In lista
                    If Not String.IsNullOrEmpty(item.venData) Then
                        Dim sFormaPagamento As String = ""
                        Dim TotalParcelas As Integer = 1

                        Select Case item.BolTipo
                            Case "B"
                                sFormaPagamento = "Boleto"
                            Case "T"
                                sFormaPagamento = "Cartão de Crédito"
                            Case "C"
                                sFormaPagamento = "Central de Atendimento"
                        End Select

                        If Not String.IsNullOrEmpty(item.BolParcelasCartaoCredito) Then
                            TotalParcelas = Int32.Parse(item.BolParcelasCartaoCredito)
                        End If

                        sParcelamentos += sFormaPagamento & " " & TotalParcelas & "x de R$ " & Format(CDec(CDec(item.bolImpValor) / TotalParcelas), "#,##0.00") & "<br>"
                    End If
                Next
                sPagamento.Replace("{{Parcelamentos}}", sParcelamentos)
            End If

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-dados-reserva")
            Dim sDadosReserva As New StringBuilder
            sDadosReserva.Clear()
            sDadosReserva.Append(objNotificacaoTemplateVO.TextoModelo)
            sDadosReserva.Replace("{{DataInicio}}", Format(CDate(ObjReservaVO.resDataIni), "dd/MM/yyyy").Substring(0, 10))
            sDadosReserva.Replace("{{DataTermino}}", Format(CDate(ObjReservaVO.resDataFim), "dd/MM/yyyy").Substring(0, 10))
            sDadosReserva.Replace("{{UnidadeHotel}}", sUnidade)
            sDadosReserva.Replace("{{TipoPensao}}", sPensao)
            sDadosReserva.Replace("{{AlmocoChegada}}", sAlmocoChegada)
            sDadosReserva.Replace("{{AlmocoSaida}}", sAlmocoSaida)
            If lista.Count > 0 Then
                sDadosReserva.Replace("{{DadosAcomodacao}}", objReservaListagemAcomodacaoVO.acmDescricao)
            Else
                sDadosReserva.Replace("{{DadosAcomodacao}}", "Não Identificado")
            End If

            sDadosReserva.Replace("{{QtdPessoas}}", ObjReservaConsultasVO.QtdPessoas)
            sDadosReserva.Replace("{{ListaIntegrantes}}", ObjReservaConsultasVO.Integrantes)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-informacoes")
            Dim sInformacoes As New StringBuilder
            sInformacoes.Clear()
            sInformacoes.Append(objNotificacaoTemplateVO.TextoModelo)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-rodape")
            Dim sRodape As New StringBuilder
            sRodape.Clear()
            sRodape.Append(objNotificacaoTemplateVO.TextoModelo)
            sRodape.Replace("{{Mensagem}}", "")
            sRodape.Replace("{{UnidadeHotel}}", sUnidade)


            Dim sEmail As New StringBuilder
            With ObjReservaConsultasVO
                sEmail.Clear()

                sEmail.AppendLine("<!DOCTYPE html> ")
                sEmail.AppendLine("<html xmlns=" & "http://www.w3.org/1999/xhtml" & "> ")
                sEmail.AppendLine("<head> ")
                sEmail.AppendLine("    <title>SESC - Confirmação de Pagamento</title> ")
                sEmail.AppendLine("    <meta http-equiv=" & "content-type" & " content=" & "text/html;charset=ISO-8859-1 /> ")

                sEmail.Append(sLogo)
                sEmail.Append(sIdentificacao)
                sEmail.Append(sPagamento)
                sEmail.Append(sDadosReserva)
                sEmail.Append(sInformacoes)
                sEmail.Append(sRodape)

                sEmail.AppendLine("</body> ")
                sEmail.AppendLine("</html> ")
                Dim Arquivo = ""
                Arquivo = Request.PhysicalApplicationPath & "ComprovanteTemp\Comprovante-" & .ResId

                'Convertendo o boleto para PDF e abrindo em uma nova aba
                Dim pp = New System.Diagnostics.Process()
                pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
                pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                File.Create(Arquivo & ".html").Dispose()
                File.AppendAllText(Arquivo & ".html", sEmail.ToString, Encoding.UTF8)

                pp.StartInfo.Arguments = "--zoom 1 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"

                pp.Start()
                pp.WaitForExit()
                pp.Close()
                pp.Dispose()
                Dim path = ""

                File.Delete(Server.MapPath(".") & "..\ComprovanteTemp\Comprovante-" & .ResId & ".html")
                path = ("../ComprovanteTemp/Comprovante-" & .ResId & ".pdf")


                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)

            End With

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.alert('" & ex.Message & "');", True)
        End Try
    End Sub
    Public Sub ComprovantePagamentoTemplateEmissivo(ByVal SolId As String)
        Try

            Dim sUnidade, sPensao, sAlmocoChegada, sAlmocoSaida As String

            objNotificacaoTemplateVO = New NotificacaoTemplateVO

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objNotificacaoTemplateDAO = New NotificacaoTemplateDAO("TurismoSocialCaldas")
                ObjReservaDAO = New ReservaDAO("TurismoSocialCaldas")
                objReservaListagemSolicitacaoDAO = New ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                objReservaListagemAcomodacaoDAO = New ReservaListagemAcomodacaoDAO("TurismoSocialCaldas")
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialCaldas")
                objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialCaldas")
                'ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Sesc Caldas Novas"

                sUnidade = "Caldas Novas"
                sPensao = "Pensão Completa – café da manhã, almoço e jantar"

            Else
                objNotificacaoTemplateDAO = New NotificacaoTemplateDAO("TurismoSocialPiri")
                objReservaListagemSolicitacaoDAO = New ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                objReservaListagemIntegranteDAO = New ReservaListagemIntegranteDAO("TurismoSocialPiri")
                ObjReservaDAO = New ReservaDAO("TurismoSocialPiri")
                objReservaListagemAcomodacaoDAO = New ReservaListagemAcomodacaoDAO("TurismoSocialPiri")
                objReservaListagemFinanceiroDAO = New ReservaListagemFinanceiroDAO("TurismoSocialPiri")
                'ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Pousada Sesc Pirenópolis"

                sUnidade = "Pirenópolis"
                sPensao = "Meia Pensão – café da manhã e buffet de caldo, sopa e creme à noite"

            End If

            Dim SituacaoReserva As String = ""
            Select Case ObjReservaConsultasVO.ResStatus
                Case "C"
                    SituacaoReserva = "Cancelada"
                Case "E"
                    SituacaoReserva = "Estada"
                Case "F"
                    SituacaoReserva = "Finalizada"
                Case "I"
                    SituacaoReserva = "Pendente de Integrante"
                Case "P"
                    SituacaoReserva = "Pendente de Pagamento"
                Case "R"
                    SituacaoReserva = "Confirmada"
                Case "S"
                    SituacaoReserva = "Solicitada"
            End Select

            ObjReservaVO = ObjReservaDAO.consultar(hddResId.Value)
            objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)
            lista = objReservaListagemAcomodacaoDAO.consultarViaResId(hddResId.Value, "")
            For Each item As ReservaListagemAcomodacaoVO In lista
                If item.solId = SolId Then
                    objReservaListagemAcomodacaoVO = item
                End If
            Next

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-reserva-logo")
            Dim sLogo As New StringBuilder
            sLogo.Clear()
            sLogo.Append(objNotificacaoTemplateVO.TextoModelo)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-emissivo-identificacao")
            Dim sIdentificacao As New StringBuilder
            sIdentificacao.Clear()
            sIdentificacao.Append(objNotificacaoTemplateVO.TextoModelo)
            sIdentificacao.Replace("{{NumReserva}}", ObjReservaVO.resId)

            Dim valorTotal As Decimal = 0
            lista = objReservaListagemIntegranteDAO.consultarViaResIdSolId(hddResId.Value, SolId, "i.IntNome desc")
            For Each item As ReservaListagemIntegranteVO In lista

                If item.intFormaPagamento = "ER" Then
                    valorTotal = valorTotal + item.hosValorPago
                End If

                If String.IsNullOrEmpty(sAlmocoChegada) And String.IsNullOrEmpty(sAlmocoSaida) Then
                    If ObjReservaVO.resSistema = "portal-turismo" Then
                        If sUnidade = "Pirenópolis" Then
                            sAlmocoChegada = "Não"
                        Else
                            sAlmocoChegada = "Sim"
                        End If
                        sAlmocoSaida = "Não"
                    Else 'Balcão
                        If item.intAlmoco = "I" Or item.intAlmoco = "S" Then
                            sAlmocoChegada = "Sim"
                        Else
                            sAlmocoChegada = "Não"
                        End If
                        If item.intAlmoco = "O" Or item.intAlmoco = "S" Then
                            sAlmocoSaida = "Sim"
                        Else
                            sAlmocoSaida = "Não"
                        End If
                    End If
                End If
            Next

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-emissivo-pagamento")
            Dim sPagamento As New StringBuilder
            sPagamento.Clear()
            If valorTotal > 0 Then
                sPagamento.Append(objNotificacaoTemplateVO.TextoModelo)

                sPagamento.Replace("{{ValorTotal}}", Format(CDec(valorTotal), "#,##0.00"))

                Dim sParcelamentos As String = ""
                lista = objReservaListagemFinanceiroDAO.consultarViaResIdSolId(hddResId.Value, SolId)

                For Each item As ReservaListagemFinanceiroVO In lista
                    If Not String.IsNullOrEmpty(item.venData) Then
                        Dim sFormaPagamento As String = ""
                        Dim TotalParcelas As Integer = 1

                        Select Case item.BolTipo
                            Case "B"
                                sFormaPagamento = "Boleto"
                            Case "T"
                                sFormaPagamento = "Cartão de Crédito"
                            Case "C"
                                sFormaPagamento = "Central de Atendimento"
                        End Select

                        If Not String.IsNullOrEmpty(item.BolParcelasCartaoCredito) Then
                            TotalParcelas = Int32.Parse(item.BolParcelasCartaoCredito)
                        End If

                        sParcelamentos += sFormaPagamento & " " & TotalParcelas & "x de R$ " & Format(CDec(CDec(item.bolImpValor) / TotalParcelas), "#,##0.00") & "<br>"
                    End If
                Next
                sPagamento.Replace("{{Parcelamentos}}", sParcelamentos)
            End If

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-emissivo-dados-reserva")
            Dim sDadosReserva As New StringBuilder
            sDadosReserva.Clear()
            sDadosReserva.Append(objNotificacaoTemplateVO.TextoModelo)
            sDadosReserva.Replace("{{DataInicio}}", Format(CDate(ObjReservaVO.resDataIni), "dd/MM/yyyy").Substring(0, 10))
            sDadosReserva.Replace("{{DataTermino}}", Format(CDate(ObjReservaVO.resDataFim), "dd/MM/yyyy").Substring(0, 10))
            sDadosReserva.Replace("{{UnidadeHotel}}", sUnidade)
            sDadosReserva.Replace("{{TipoPensao}}", sPensao)
            sDadosReserva.Replace("{{AlmocoChegada}}", sAlmocoChegada)
            sDadosReserva.Replace("{{AlmocoSaida}}", sAlmocoSaida)
            If lista.Count > 0 Then
                sDadosReserva.Replace("{{DadosAcomodacao}}", objReservaListagemAcomodacaoVO.acmDescricao)
            Else
                sDadosReserva.Replace("{{DadosAcomodacao}}", "Não Identificado")
            End If

            sDadosReserva.Replace("{{QtdPessoas}}", ObjReservaConsultasVO.QtdPessoas)
            sDadosReserva.Replace("{{ListaIntegrantes}}", ObjReservaConsultasVO.Integrantes)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-emissivo-informacoes")
            Dim sInformacoes As New StringBuilder
            sInformacoes.Clear()
            sInformacoes.Append(objNotificacaoTemplateVO.TextoModelo)

            objNotificacaoTemplateVO = objNotificacaoTemplateDAO.consultarbyIdentificador("comprovante-emissivo-rodape")
            Dim sRodape As New StringBuilder
            sRodape.Clear()
            sRodape.Append(objNotificacaoTemplateVO.TextoModelo)
            sRodape.Replace("{{Mensagem}}", "")
            sRodape.Replace("{{UnidadeHotel}}", sUnidade)


            Dim sEmail As New StringBuilder
            With ObjReservaConsultasVO
                sEmail.Clear()

                sEmail.AppendLine("<!DOCTYPE html> ")
                sEmail.AppendLine("<html xmlns=" & "http://www.w3.org/1999/xhtml" & "> ")
                sEmail.AppendLine("<head> ")
                sEmail.AppendLine("    <title>SESC - Confirmação de Pagamento</title> ")
                sEmail.AppendLine("    <meta http-equiv=" & "content-type" & " content=" & "text/html;charset=ISO-8859-1 /> ")

                sEmail.Append(sLogo)
                sEmail.Append(sIdentificacao)
                sEmail.Append(sPagamento)
                sEmail.Append(sDadosReserva)
                sEmail.Append(sInformacoes)
                sEmail.Append(sRodape)

                sEmail.AppendLine("</body> ")
                sEmail.AppendLine("</html> ")
                Dim Arquivo = ""
                Arquivo = Request.PhysicalApplicationPath & "ComprovanteTemp\Comprovante-" & .ResId & "-" & SolId

                'Convertendo o boleto para PDF e abrindo em uma nova aba
                Dim pp = New System.Diagnostics.Process()
                pp.StartInfo.FileName = "C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
                pp.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                File.Create(Arquivo & ".html").Dispose()
                File.AppendAllText(Arquivo & ".html", sEmail.ToString, Encoding.UTF8)

                pp.StartInfo.Arguments = "--zoom 1 --page-size A4 --margin-top 4mm --margin-bottom 4mm --margin-left 10mm --margin-right 10mm " & Arquivo & ".html" & " " & Arquivo & ".pdf"

                pp.Start()
                pp.WaitForExit()
                pp.Close()
                pp.Dispose()
                Dim path = ""

                File.Delete(Server.MapPath(".") & "..\ComprovanteTemp\Comprovante-" & .ResId & "-" & SolId & ".html")
                path = ("../ComprovanteTemp/Comprovante-" & .ResId & "-" & SolId & ".pdf")


                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & path & "');", True)

            End With

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.alert('" & ex.Message & "');", True)
        End Try
    End Sub

    Protected Sub btnImprimeComprovante_Click(sender As Object, e As EventArgs) Handles btnImprimeComprovante.Click
        'Dim ImgPrinter As ImageButton = sender
        'Dim row As GridViewRow = ImgPrinter.NamingContainer 'Dim index As Integer = row.RowIndex
        'Dim ResId = gdvListaSolicitacao.DataKeys(row.RowIndex()).Item("resIdTS").ToString
        'hddDestino.Value = gdvListaSolicitacao.DataKeys(row.RowIndex()).Item("destino").ToString
        Dim Destino As String = ""
        'Atualiza o valor do boleto em TbBoletosImp
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Destino = "C"
        Else
            Destino = "P"
        End If

        If Destino = "C" Then
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialCaldas")
        Else
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialPiri")
        End If
        ObjReservaConsultasVO = New ReservaConsultasVO
        ObjReservaConsultasVO.ResId = hddResId.Value
        If ObjReservaConsultasVO.ResId = 0 Then
            Return
        End If
        If Destino = "C" Then
            ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Sesc Caldas Novas"
        Else
            ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Pousada Sesc Pirenópolis"
        End If
        ObjReservaConsultasVO = ObjReservaConsultasDAO.ConsultaDadosComprovanteReserva(ObjReservaConsultasVO)
        ComprovantePagamentoTemplate()
        hddProcessando.Value = ""
    End Sub

    Protected Sub imgRelatosObs_Click(sender As Object, e As ImageClickEventArgs) Handles imgRelatosObs.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialCaldas")
        Else
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialPiri")
        End If
        ObjReservaConsultasVO = New ReservaConsultasVO
        Dim ListaAux As IList
        Dim ListaToolTipo As String = ""

        Dim Contador As Integer = 0
        imgRelatosObs.ToolTip.Remove(0)

        ListaAux = ObjReservaConsultasDAO.ConsultaObservacoes(txtResCPF.Text.Trim.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(".", ""), txtResMatricula.Text.Trim.Replace(" ", "").Replace("-", "").Replace("/", "").Replace(".", ""))
        For Each item As ReservaConsultasVO In ListaAux
            If Contador = 0 Then
                ListaToolTipo = "PERÍODO".PadRight(42, " ") & "OBSERVAÇÕES".PadRight(58, " ")
                Contador += 1
                ListaToolTipo += Chr(13) & "=".PadRight(80, "=")
            End If
            Contador += 1
            ListaToolTipo += Chr(13) & item.Periodo.PadRight(30, " ") & " " & item.ResObs.PadRight(70, " ")
        Next
        imgRelatosObs.ToolTip = ListaToolTipo
    End Sub

    Protected Sub ckbItemIntegrante_CheckedChanged(sender As Object, e As EventArgs)
        'CType(e.Row.FindControl("ckbItemIntegrante"), CheckBox).Attributes.Add("BoletoReimpressao", "BoletosTemp\" & gdvReserva9.DataKeys(e.Row.RowIndex).Item("BolParcelaUnica").ToString & ".html")

        Dim checkbox As CheckBox = sender
        Dim LinhaAtual As GridViewRow = checkbox.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim IntId = gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("intId").ToString

        Dim bolImpPrimeiraParcela = "",
        bolImpIdSegudaParcela = "",
        bolImpUnicaParcela = "",
        bolImpIndividual = "",
        EnvRegPrimeira = False,
        EnvRegSegunda = False,
        EnvRegUnica = False,
        EnvIndividual = False,
        PagoPrimeira = False,
        PagoSegunda = False,
        PagoUnica = False,
        PagoIndividual = False,
        RegPrimeira = False,
        RegSegunda = False,
        RegUnica = False,
        RegIndividual = False

        If checkbox.Checked = True Then
            bolImpPrimeiraParcela = gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("BolPrimeiraParcela").ToString
            bolImpIdSegudaParcela = gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("BolSegundaParcela").ToString
            bolImpUnicaParcela = gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("BolParcelaUnica").ToString
            bolImpIndividual = gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("BolParcelaUnica").ToString
            btnHospedagemNova.Attributes.Remove("PagoPrimeira")

            For Each Item As GridViewRow In gdvReserva9.Rows
                If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolPrimeiraParcela").ToString = bolImpPrimeiraParcela And bolImpPrimeiraParcela.Trim.Length > 0 Then
                    If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolPrimeiraStatusRegistro").ToString = "90" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        EnvRegPrimeira = True
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolPrimeiraStatusRegistro").ToString = "06" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        PagoPrimeira = True
                        btnHospedagemNova.Attributes.Add("PagoPrimeira", "S")
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolPrimeiraStatusRegistro").ToString = "02" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        RegPrimeira = True
                    End If
                End If

                If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolSegundaParcela").ToString = bolImpIdSegudaParcela And bolImpIdSegudaParcela.Trim.Length > 0 Then
                    If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolSegundaStatusRegistro").ToString = "90" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        EnvRegSegunda = True
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolSegundaStatusRegistro").ToString = "06" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        PagoSegunda = True
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolSegundaStatusRegistro").ToString = "02" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        RegSegunda = True
                    End If
                End If

                'A individual 
                If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolParcelaUnica").ToString = bolImpUnicaParcela And bolImpUnicaParcela.Trim.Length > 0 Then
                    If gdvReserva9.DataKeys(Item.RowIndex()).Item("BolUnicaStatusRegistro").ToString = "90" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        EnvRegUnica = True
                        EnvIndividual = True
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolUnicaStatusRegistro").ToString = "06" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        PagoUnica = True
                        PagoIndividual = True
                    ElseIf gdvReserva9.DataKeys(Item.RowIndex()).Item("BolUnicaStatusRegistro").ToString = "02" Then
                        CType(Item.FindControl("ckbItemIntegrante"), CheckBox).Checked = True
                        RegUnica = True
                        RegIndividual = True
                    End If
                End If
            Next
            Dim Arquivo = ""
            Dim path = ""

            'Boletos Gerados, Enviados e Registrados serão exibidos para reimpressão
            Select Case gdvReserva9.DataKeys(LinhaAtual.RowIndex()).Item("BolUnicaStatusRegistro").ToString
                Case Is = "00", "90"
                    CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Visible = False
                Case "02"
                    CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Visible = True
                    CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text = Format(DateAdd(DateInterval.Day, 6, Now.Date), "dd/MM/yyyy")
                Case Else
                    CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Visible = DateDiff(DateInterval.Day, CDate(Format(Date.Now, "yyyy-MM-dd")), CDate(Format(CDate(hddResDataIni.Value), "yyyy-MM-dd"))) > 9
            End Select

            '=== Pagamento da primeira parcela
            If EnvRegPrimeira = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParceladoRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto enviado para registro"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParcelado.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If PagoPrimeira = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParcelado.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = False
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto único já pago."
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParcelado.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If RegPrimeira = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParcelado.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto Registrado. Clique para imprimir"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoParcelado.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            '=== Pagamento da segunda parcela
            If EnvRegSegunda = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAVistaRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto enviado para registro"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If PagoSegunda = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = False
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto único já pago."
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If RegSegunda = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto Registrado. Clique para imprimir"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If


            '=== Pagamento em parcela única no Boleto
            If EnvRegUnica = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAVistaRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto enviado para registro"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If PagoUnica = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = False
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto único já pago."
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If RegUnica = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto Registrado. Clique para imprimir"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ImageUrl = "images/BoletoAvista.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoleto"), ImageButton).ToolTip = "Boleto à vista"
            End If

            '=== Pagamento em parcela Individual
            If EnvIndividual = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividualRegistrando.jpg"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto enviado para registro"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividual.png"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If PagoIndividual = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividual.png"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Enabled = False
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto único já pago."
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividual.png"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto à vista"
            End If

            If RegIndividual = True Then
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividual.png"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto Registrado. Clique para imprimir"
            Else
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ImageUrl = "images/BoletoIndividual.png"
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Enabled = True
                CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).ToolTip = "Boleto à vista"
            End If
        Else
            CType(gdvReserva9.HeaderRow.FindControl("imgBoletoIndividual"), ImageButton).Visible = False
        End If

    End Sub

    Protected Sub BtnNovaTabela_Click(sender As Object, e As EventArgs) Handles BtnNovaTabela.Click
        ObjReservaConsultasVO = New ReservaConsultasVO

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialCaldas")
        Else
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialPiri")
        End If

        If ObjReservaConsultasDAO.MudaDataInsercao(hddResId.Value, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")) = 1 Then
            Mensagem("Data de inserção alterada com sucesso!\n\nClique em um integrante e salve para que os novos valores entrem em vigor.")
        End If

    End Sub

    Protected Sub txtDiasPrazo_TextChanged(sender As Object, e As EventArgs)
        hddDiasPrazo.Value = DateDiff(DateInterval.Day, Date.Today, CDate(CType(gdvReserva9.HeaderRow.FindControl("txtDiasPrazo"), TextBox).Text))
    End Sub

    Protected Sub imgRegistro_Click(sender As Object, e As ImageClickEventArgs)
        Dim ImageBall As ImageButton = sender
        Dim row As GridViewRow = ImageBall.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim BolImpId = gdvReserva10.DataKeys(row.RowIndex()).Item("bolImpNossoNumero").ToString
        BolImpId = Mid(BolImpId, 1, 10)
        'BolImpId = "8218170303"

        If CType(row.FindControl("imgRegistro"), ImageButton).ImageUrl = "images\BoletoVerde.png" Then
            If BolImpId.Length > 9 Then
                'Mensagem("Existe um boleto registrado para essa reserva de grupo.\n\nO mesmo será aberto para impressão.")
                'Dim pathGrupo = "BoletosTemp\" & BolImpId & ".pdf"
                Dim pathGrupo = (Request.PhysicalApplicationPath & "BoletosTemp\" & BolImpId & ".pdf")
                Dim pathGrupoTurWeb = ("\\srv-adm-web\BoletosTemp\" & BolImpId & ".pdf")

                Dim pathGrupoOpen = ""
                If File.Exists(pathGrupo) Then
                    pathGrupoOpen = "BoletosTemp/" & BolImpId & ".pdf"
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & pathGrupoOpen & "');", True)
                ElseIf File.Exists(pathGrupo.Replace("pdf", "html")) Then
                    pathGrupoOpen = "BoletosTemp/" & BolImpId & ".html"
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & pathGrupoOpen & "');", True)
                    'Abrindo boletos que estão no turWeb
                ElseIf File.Exists(pathGrupoTurWeb) Then
                    pathGrupoOpen = "file://srv-adm-web/BoletosTemp/" & BolImpId & ".pdf"
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & pathGrupoOpen & "');", True)
                ElseIf File.Exists(pathGrupoTurWeb.Replace("pdf", "html")) Then
                    pathGrupoOpen = "file://srv-adm-web/BoletosTemp/" & BolImpId & ".html"
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "window.open('" & pathGrupoOpen & "');", True)
                Else
                    Mensagem("O boleto não pode ser encontrado.")
                End If
            End If
        End If
    End Sub

    Protected Sub imgApagaBoleto_Click(sender As Object, e As ImageClickEventArgs)
        Dim ImageBall As ImageButton = sender
        Dim row As GridViewRow = ImageBall.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim BolImpId = gdvReserva10.DataKeys(row.RowIndex()).Item("bolImpNossoNumero").ToString
        BolImpId = Mid(BolImpId, 1, 10)
        Dim Destino As String = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Destino = "C"
            If Session("ApagaBoleto") Then
                objBoletoNetVO = New BoletoNetVO
                objBoletoNetDAO = New BoletoNetDAO()
                If objBoletoNetDAO.ApagaBoletoLogicamente(BolImpId, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), Destino) = 1 Then
                    Mensagem("O Boleto foi apagado com sucesso!")
                    ListaFinanceiroViaResId()
                    ListaIntegranteViaResId()
                    hddProcessando.Value = ""
                Else
                    Mensagem("Esse boleto não pode ser apagado. Verifique por favor.")
                    hddProcessando.Value = ""
                End If
            Else
                Mensagem("Você não possui autorização para essa operação.")
                hddProcessando.Value = ""
            End If
        Else
            Destino = "P"
            If Session("ApagaBoletoPiri") Then
                objBoletoNetVO = New BoletoNetVO
                objBoletoNetDAO = New BoletoNetDAO()
                If objBoletoNetDAO.ApagaBoletoLogicamente(BolImpId, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), Destino) = 1 Then
                    Mensagem("O Boleto foi apagado com sucesso!")
                    ListaFinanceiroViaResId()
                    hddProcessando.Value = ""
                Else
                    Mensagem("Esse boleto não pode ser apagado. Verifique por favor.")
                    hddProcessando.Value = ""
                End If
            Else
                Mensagem("Você não possui autorização para essa operação.")
                hddProcessando.Value = ""
            End If
        End If
    End Sub

    ''' Verifica se o e-mail é válido
    ''' <param name="emailAddress">Endereço de e-mail</param>
    Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
        ' Pattern ou mascara de verificação
        '"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
        Dim pattern As String = "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + "\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + ".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"

        ' Verifica se o email corresponde a pattern/mascara
        Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)

        ' Caso corresponda
        If emailAddressMatch.Success Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub imbVoucher_Click(sender As Object, e As ImageClickEventArgs)
        Dim Voucher As ImageButton = sender
        Dim row As GridViewRow = Voucher.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim solId = gdvReserva9.DataKeys(row.RowIndex()).Item("solId").ToString
        Dim hosid = gdvReserva9.DataKeys(row.RowIndex()).Item("hosId").ToString

        Dim Destino As String = ""
        'Atualiza o valor do boleto em TbBoletosImp
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Destino = "C"
        Else
            Destino = "P"
        End If

        If Destino = "C" Then
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialCaldas")
        Else
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialPiri")
        End If
        ObjReservaConsultasVO = New ReservaConsultasVO
        ObjReservaConsultasVO.ResId = hddResId.Value
        If ObjReservaConsultasVO.ResId = 0 Then
            Return
        End If
        If Destino = "C" Then
            ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Sesc Caldas Novas"
        Else
            ObjReservaConsultasVO.BolLocalidadeCartaoCredito = "Pousada Sesc Pirenópolis"
        End If
        ObjReservaConsultasVO = ObjReservaConsultasDAO.ConsultaDadosVoucherReserva(hosid, solId)

        If ObjReservaConsultasVO Is Nothing Then
            Mensagem("Verifique o pagamento dos integrantes dessa hospedagem.")
            Exit Sub
        End If
        '(V) = Voucher        
        If hddResCaracteristica.Value = "E" Or hddResCaracteristica.Value = "T" Then
            ComprovantePagamentoTemplateEmissivo(solId)
        Else
            ComprovantePagamentoTemplate()
        End If

        hddProcessando.Value = ""
    End Sub
    Protected Sub imgConsultaNascimento_Click(sender As Object, e As ImageClickEventArgs) Handles imgConsultaNascimento.Click
        'Atualiza o valor do boleto em TbBoletosImp
        ObjReservaConsultasVO = New ReservaConsultasVO
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialCaldas")
        Else
            ObjReservaConsultasDAO = New ReservaConsultasDAO("TurismoSocialPiri")
        End If
        ObjReservaConsultasVO = ObjReservaConsultasDAO.ConsultaDataNascimento(Replace(txtConCPF.Text, " ", "").Replace("-", "").Replace("/", "").Replace(".", ""))

        'Retornei o nascimento nesse campo
        If ObjReservaConsultasVO.BolImpDtDocumento = "" Then
            Mensagem("Não foi possível localizar o responsável pelo CPF informado.")
        Else
            If IsDate(CDate(Format(CDate(ObjReservaConsultasVO.BolImpDtDocumento), "dd/MM/yyyy"))) Then
                Mensagem("Data encontrada.\n\nNome: " & ObjReservaConsultasVO.BolImpSacado & "\nNascimento: " & CDate(Format(CDate(ObjReservaConsultasVO.BolImpDtDocumento), "dd/MM/yyyy")))
            End If
        End If
    End Sub

    Private Sub btnInformarRestituicao_Click(sender As Object, e As EventArgs) Handles btnInformarRestituicao.Click

        Try
            hddProcessando.Value = ""
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialCaldas")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialCaldas")
            Else
                objReservaListagemSolicitacaoDAO = New Turismo.ReservaListagemSolicitacaoDAO("TurismoSocialPiri")
                ObjReservaDAO = New Turismo.ReservaDAO("TurismoSocialPiri")
            End If
            If (sender Is btnInformarRestituicao) Then
                objReservaListagemSolicitacaoVO.resId = hddResId.Value
            Else
                objReservaListagemSolicitacaoVO.resId = -hddResId.Value
            End If

            objReservaListagemSolicitacaoVO = objReservaListagemSolicitacaoDAO.consultarReservaViaResId(hddResId.Value)

            If ObjReservaDAO.InformaEstornoReserva("Reserva já teve o valor estornado, favor não reativar. " & objReservaListagemSolicitacaoVO.resObs, objReservaListagemSolicitacaoVO.resId) Then
                Mensagem("Reserva Atualizada. ")
            End If

            hddProcessando.Value = ""
            txtResMatricula.Focus()

        Catch ex As Exception

        End Try

    End Sub

End Class
