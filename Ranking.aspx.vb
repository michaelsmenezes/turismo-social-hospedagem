Imports System.Net.Mail

Partial Class Ranking
    Inherits System.Web.UI.Page
    Dim objRankingDAO As Turismo.RankingDAO
    Dim objRankingVO As Turismo.RankingVO
    Dim objResponsavelDAO As Turismo.ResponsavelDAO
    Dim objResponsavelVO As Turismo.ResponsavelVO
    Dim objPrevisaoEstadaDAO As Turismo.PrevisaoEstadaDAO
    Dim lista As IList
    Dim qtdeDiarias As Integer
    Dim qtdeIntegrantes As Integer
    Dim Idade As Integer

    Protected Sub enviarEmail(ByVal Email As String, ByVal Texto As String, Destino As String)
        Try
            Dim objEmail As New System.Net.Mail.MailMessage()
            objEmail.Subject = "SESC Goiás - Turismo Social"
            objEmail.To.Add(New System.Net.Mail.MailAddress(Email))
            objEmail.IsBodyHtml = True

            Dim objSmtp As SmtpClient

            '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
            'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
            objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
            objSmtp.EnableSsl = True
            objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
            objSmtp.UseDefaultCredentials = False
            objSmtp.Timeout = 5000

            If Destino = "C" Then
                objEmail.From = New System.Net.Mail.MailAddress("reservas.caldasnovas@sescgo.com.br")
            Else
                objEmail.From = New System.Net.Mail.MailAddress("reservas.pirenopolis@sescgo.com.br")
            End If


            objEmail.IsBodyHtml = True
            objEmail.Body = Texto

            objSmtp.Send(objEmail)
        Catch ex As Exception
            Response.Redirect("erro.aspx?erro=Ocorreu um erro em sua solicitação.  &excecao=" & ex.StackTrace.ToString &
              "&Erro=Erro ao enviar e-mail. " & "&sistema=TurismoNet" & "&acao=enviarEmail(): Ranking.vb " & "&Local=Objeto: funcao enviarEmail() Email: " & Email)
        End Try
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
            If Not Session("GrupoRanking") Then
                Response.Redirect("AcessoNegado.aspx")
            Else
                calChegada.SelectedDate = Date.Now
                calSaida.SelectedDate = Date.Now
            End If
        End If
        If Not My.Computer.Network.Ping("sql-ctl", 1000) Then
            'rblRankingDestino.Items(0).Enabled = False
            rblRankingDestino.Items(0).Text = "Caldas Novas momentaneamente indisponível"
            'rblRankingDestino.Items(1).Selected = True
        Else
            rblRankingDestino.Items(0).Enabled = True
            rblRankingDestino.Items(0).Text = "SESC Caldas Novas"
            rblRankingDestino.Items(0).Selected = Not rblRankingDestino.Items(1).Selected
        End If

        If Not My.Computer.Network.Ping("sql-psp", 1000) Then
            'rblRankingDestino.Items(1).Enabled = False
            rblRankingDestino.Items(1).Text = "Pirenópolis momentaneamente indisponível"
            'rblRankingDestino.Items(0).Selected = True
        Else
            rblRankingDestino.Items(1).Enabled = True
            rblRankingDestino.Items(1).Text = "Pousada SESC Pirenópolis"
            rblRankingDestino.Items(1).Selected = Not rblRankingDestino.Items(0).Selected
        End If
    End Sub

    Protected Sub calChegada_SelectionChanged(sender As Object, e As EventArgs) Handles calChegada.SelectionChanged, calSaida.SelectionChanged
        If sender.Equals(calChegada) Then
            If CDate(calChegada.SelectedDate) > CDate(calSaida.SelectedDate) Then
                calSaida.SelectedDate = calChegada.SelectedDate
            End If
            calSaida.Focus()
        ElseIf sender.Equals(calSaida) Then
            If CDate(calChegada.SelectedDate) > CDate(calSaida.SelectedDate) Then
                calChegada.SelectedDate = calSaida.SelectedDate
            End If
            btnConsulta.Focus()
        End If
    End Sub

    Protected Sub gdvRankingAcao_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvRankingAcao.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            qtdeDiarias = "0"
            qtdeIntegrantes = "0"

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ckbItemResponsavel"), CheckBox).BorderColor = If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("SolDtListaEspera").ToString > "", Drawing.Color.Orange, If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("SolDtSemDisponibilidade").ToString > "", Drawing.Color.Red, Drawing.Color.White))
            CType(e.Row.FindControl("ckbItemResponsavel"), CheckBox).Attributes.Add("SolId", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("solId"))
            CType(e.Row.FindControl("ckbItemResponsavel"), CheckBox).Attributes.Add("ResSexo", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resSexo"))
            CType(e.Row.FindControl("ckbItemResponsavel"), CheckBox).Attributes.Add("ResNome", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resNome"))

            CType(e.Row.FindControl("imgBtnResponsavel"), ImageButton).Attributes.Add("resId", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("imgBtnResponsavel"), ImageButton).Attributes.Add("solId", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("solId"))

            'Deu erro para a Rosamar ao fazer o atendimento do ranking
            Try
                If gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("SolDtEnviado").ToString.Replace(" ", "").Replace("/", "").Replace("-", "") > "" Then
                    Dim ListaTool As New StringBuilder
                    ListaTool.AppendLine("Enviada em: " & Format(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("SolDtEnviado").ToString), "dd/MM/yyyy"))
                    ListaTool.AppendLine("Bloqueio: " & Format(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDtLimiteRetorno").ToString), "dd/MM/yyyy"))
                    ListaTool.AppendLine("Telefone: " & "(" & gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("ResFonePrefixo").ToString & ")" & gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("ResCelular").ToString.Insert(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("ResCelular").ToString.Length - 4, "-"))
                    'CType(e.Row.FindControl("imgBtnResponsavel"), ImageButton).ToolTip = "Enviada em: " & Format(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("SolDtEnviado").ToString), "dd/MM/yyyy")
                    CType(e.Row.FindControl("imgBtnResponsavel"), ImageButton).ToolTip = ListaTool.ToString
                End If
            Catch ex As Exception
            End Try

            CType(e.Row.FindControl("lblPeriodo"), Label).Text = Mid(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni").ToString, 1, 5) + " - " + Mid(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtFim").ToString, 1, 5) + " (" +
                DateDiff(DateInterval.Day, CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtFim"))).ToString + "d)"
            qtdeDiarias += DateDiff(DateInterval.Day, CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtFim")))
            CType(e.Row.FindControl("lblIntegrantes"), Label).Text =
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                   gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome1").ToString + " (" +
                calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento1")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")" +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido1").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome2").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome2").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento2")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido2").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome3").ToString > " ", "<br><font cor=chocolate>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome3").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento3")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido3").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome4").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome4").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento4")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido4").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome5").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome5").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento5")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido5").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome6").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome6").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento6")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido6").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "1", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome7").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome7").ToString + " (" +
                  calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento7")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetido7").ToString()
            'DateDiff(DateInterval.Year, CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento7")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") + _

            CType(e.Row.FindControl("lblIntegrantesAtendidos"), Label).Text =
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId1Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId1Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome1Atendido").ToString > " ", gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome1Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento1Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido1").ToString +
               If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId2Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId2Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome2Atendido").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome2Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento2Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido2").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId3Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId3Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome3Atendido").ToString > " ", "<br><font cor=chocolate>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome3Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento3Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido3").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId4Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId4Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome4Atendido").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome4Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento4Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido4").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId5Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId5Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome5Atendido").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome5Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento5Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido5").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId6Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId6Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome6Atendido").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome6Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento6Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                 gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido6").ToString +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId7Atendido").ToString <= "2", "<font color=green>", If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intCatId7Atendido").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome7Atendido").ToString > " ", "<br>" + gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome7Atendido").ToString + " (" +
                   calculaIdade(CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intDtNascimento7Atendido")), CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
            gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intRepetidoAtendido7").ToString()

            CType(e.Row.FindControl("lblReserva"), Label).Text += If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDataIni").ToString() > " ", "<br>" + Mid(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDataIni").ToString(), 1, 5) + " - " +
                Mid(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDataFim").ToString(), 1, 5) + " (" +
                DateDiff(DateInterval.Day, gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDataIni"), gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("resDataFim")).ToString + "d)", "")

            CType(e.Row.FindControl("lblhistorico"), Label).Text = If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("situacao").ToString() = "0", "Nunca Hospedou",
                                                                            If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("situacao").ToString() = "1", "Hospedou há menos de 2 anos", "Não hospeda há mais de 2 anos"))
            qtdeIntegrantes += If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome7").ToString > " ", 7, + _
                                  If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome6").ToString > " ", 6, + _
                                  If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome5").ToString > " ", 5, + _
                                  If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome4").ToString > " ", 4, + _
                                  If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome3").ToString > " ", 3, + _
                                  If(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("intNome2").ToString > " ", 2, 1))))))

            'Irá identificar com uma cor diferente as solicitações que estão próximas de vencer.
            If (DateDiff(DateInterval.Day, Now.Date, CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))) <= 5) Then
                e.Row.BackColor = Drawing.Color.Silver
                e.Row.ToolTip = DateDiff(DateInterval.Day, Now.Date, CDate(gdvRankingAcao.DataKeys(e.Row.RowIndex).Item("dtIni"))) & " Dia(s) para o início da reserva."
            End If


        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = gdvRankingAcao.Rows.Count.ToString
            e.Row.Cells(4).Text = qtdeDiarias.ToString + " d"
            e.Row.Cells(5).Text = qtdeIntegrantes
        End If

    End Sub

    Protected Sub ckbHeadResponsavel_CheckedChanged(sender As Object, e As EventArgs)
        For Each linha As GridViewRow In gdvRankingAcao.Rows
            CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked = sender.Checked
        Next
        pnlAcao.Visible = sender.Checked
    End Sub

    Protected Sub ckbItemResponsavel_CheckedChanged(sender As Object, e As EventArgs)
        pnlAcao.Visible = False
        If gdvRankingAcao.Rows.Count > 0 Then
            CType(gdvRankingAcao.HeaderRow.FindControl("ckbHeadResponsavel"), CheckBox).Checked = True
            For Each linha As GridViewRow In gdvRankingAcao.Rows
                pnlAcao.Visible += CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked
                CType(gdvRankingAcao.HeaderRow.FindControl("ckbHeadResponsavel"), CheckBox).Checked = CType(gdvRankingAcao.HeaderRow.FindControl("ckbHeadResponsavel"), CheckBox).Checked AndAlso CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked
            Next
        End If
    End Sub

    Protected Sub btnConsulta_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        Try
            If rblRankingDestino.SelectedValue = "C" Then
                objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
            Else
                objRankingDAO = New Turismo.RankingDAO("TurismoSocialPiri")
            End If
            gdvRankingAcao.DataSource = objRankingDAO.consultar(rblRankingOpcao.SelectedValue, txtRankingPercentual.Text, FormatDateTime(calChegada.SelectedDate, DateFormat.ShortDate), FormatDateTime(calSaida.SelectedDate, DateFormat.ShortDate))
            gdvRankingAcao.EmptyDataText = "Atenção! Não foi encontrada nenhuma solicitação."
            gdvRankingAcao.DataBind()
            pnlAcao.Visible = False
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub btnAtender_Click(sender As Object, e As EventArgs) Handles btnAtender.Click
        Dim Texto As String = ""
        Dim listaRanking As String = ""
        Dim destino As String = rblRankingDestino.SelectedValue
        For Each linha As GridViewRow In gdvRankingAcao.Rows
            If CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked Then
                listaRanking += CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Attributes("solId") + "."
            End If
        Next
        If destino = "C" Then
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
        Else
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialPiri")
        End If
        lista = objRankingDAO.acaoRanking(listaRanking, Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))

        For Each item As Turismo.RankingVO In lista
            If item.resSexo = "M" Then
                Texto = "Prezado Sr. " & item.resNome
            Else
                Texto = "Prezada Sra. " & item.resNome
            End If
            Texto += "<p />Obrigado por utilizar o Sistema Online de Turismo Social do SESC Goiás."
            If destino = "C" Then
                Texto += "<p>Informamos que sua solicitação de reserva para o Sesc de Caldas Novas no período de "
            ElseIf destino = "P" Then
                Texto += "<p>Informamos que sua solicitação de reserva para o Sesc Pirenópolis no período de "
            End If
            Texto += item.dtIni & " com entrada a partir das 14h00 "
            If destino = "C" Then
                Texto += "(incluso almoço servido no horário de 12h00 às 15h00) "
            End If
            Texto += "a " & item.dtFim & " com saída até 10h, foi confirmada."
            Texto += "<p>Para garanti-la, acesse o site <a href='http://www.sescgo.com.br'>www.sescgo.com.br</a> e clique no ícone 'Solicitação de Reserva', em seguida 'Desejo acessar minhas Solicitações de Reserva' e siga os procedimentos indicados."
            Texto += "<p>O valor da hospedagem poderá ser pago nas seguintes condições:"
            Texto += "<p>1. À vista, por meio de cartão de débito, cartão de crédito e boleto bancário, ou parcelado, em até 2X sem juros por meio de boleto e até 10X sem juros com cartão de crédito – conforme opções disponibilizadas no momento da confirmação;"
            Texto += "<p>2. Se à vista, por cartão de débito, o pagamento poderá ser feito somente nas Unidades Executivas do Sesc em Goiânia, Anápolis, Pirenópolis, Itumbiara, Jataí e Caldas Novas e as bandeiras aceitas são: VISA ELECTRON, MASTERCARD MAESTRO, DINERS e ELO;"
            Texto += "<p>3. Os cartões de crédito aceitos são: VISA, MASTERCARD, DINERS e ELO e o pagamento poderá ser feito diretamente no site ou em qualquer Unidade Executiva do Sesc em Goiânia, Anápolis, Pirenópolis, Itumbiara, Jataí e Caldas Novas;"
            Texto += "<p>4. O pagamento deverá ser feito até a data do vencimento apresentada pelo sistema. Se em duas parcelas, por boleto bancário, a segunda não poderá vencer em prazo inferior a 10 dias do início da estada;"
            Texto += "<p>5. Após o vencimento do primeiro boleto, não havendo o pagamento, a reserva será automaticamente cancelada;"
            Texto += "<p>6. Após o vencimento do segundo boleto, não havendo o pagamento, a reserva será automaticamente cancelada e o valor pago ficará sujeito às normas de restituição;"
            Texto += "<p>7. A reserva só estará garantida após a confirmação do pagamento integral;"
            Texto += "<p>Observações:"
            Texto += "<p>ATENÇÃO!!! É necessário permitir pop-up para visualizar o boleto gerado."
            Texto += "<p>Verifique se o código de barras gerado na tela é compatível com o impresso. Uma confirmação será apresentada na tela do computador após a impressão do boleto, para visualização e conferência. Existem situações em que o computador pode estar infectado por um vírus que altera dados do boleto gerado."
            Texto += "<p>Sesc Central de Reservas - Turismo Social"
            Texto += "<p>Não é preciso responder este email."
            Texto += "<p>Goiânia, " & Format(Now, "HH:mm") & " horas do dia " & Now.Day.ToString & " de " & MonthName(Now.Month) & " de " & Now.Year.ToString
            Texto += "<p>Esta mensagem pode conter informações confidenciais e/ou privilegiadas."
            Texto += "<br>Se você não for o destinatário ou a pessoa autorizada a recebê-la, não pode usar, copiar ou divulgar as informações nela contidas ou tomar qualquer ação baseada nelas."
            Texto += "<br>Se você recebeu esta mensagem por engano, por favor, avise imediatamente o remetente, e em seguida, apague-a."
            Texto += "<br>Comunicações pela Internet não podem ser garantidas quanto à segurança ou inexistência de erros ou de vírus. O remetente, por esta razão, não aceita responsabilidade por qualquer erro ou omissão no contexto da mensagem decorrente da transmissão via Internet."
            enviarEmail(item.resEmail, Texto, destino)
        Next
        btnConsulta_Click(sender, e)
    End Sub

    Protected Sub btnFila_Click(sender As Object, e As EventArgs) Handles btnFila.Click
        Dim Texto As String = ""
        Dim listaRanking As String = ""
        Dim destino As String = rblRankingDestino.SelectedValue
        Dim DestinoEmail As String = rblRankingDestino.SelectedValue
        For Each linha As GridViewRow In gdvRankingAcao.Rows
            If CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked Then
                listaRanking += CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Attributes("solId") + "."
            End If
        Next
        If destino = "C" Then
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
            destino = "o SESC Caldas Novas"
        Else
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialPiri")
            destino = "a Pousada SESC Pirenópolis"
        End If
        lista = objRankingDAO.acaoListaEspera(listaRanking)

        For Each item As Turismo.RankingVO In lista
            'Se por acaso a lista ficou sem ação, enviarei um e-mail ao cliente somente quando a data inicial for maior ou igual a hoje
            'Por Washington em 25-01-2019 10h
            If Format(CDate(item.dtIni), "yyyy-MM-dd") > Format(Date.Now, "yyyy-MM-dd") Then
                If item.resSexo = "M" Then
                    Texto = "Prezado Sr. " & item.resNome
                Else
                    Texto = "Prezada Sra. " & item.resNome
                End If
                Texto += "<p>Obrigado por utilizar o Sistema Online de Turismo Social do SESC Goiás."
                Texto += "<p>No momento não há disponibilidade para o período solicitado de " & item.dtIni & " a " & item.dtFim & " para " & destino & "."
                Texto += "<p>Sua solicitação de reserva foi incluída na Lista de Espera e será mantida dentro dos prazos informados para o recebimento da resposta definitiva."
                Texto += "<p>Comerciários / Dependentes: até 30 dias antes do início do mês de interesse."
                Texto += "<br>Conveniados / Usuários: após este prazo, até o início do mês de interesse."
                Texto += "<p>Para melhor compreensão, enquanto seu pedido estiver em lista de espera, não há disponibilidade imediata para a sua acomodação, somente possibilidades de confirmação diante de cancelamentos e/ou desistências."
                Texto += "<p>À oportunidade agradecemos a preferência."
                Texto += "<p>Atenciosamente"
                Texto += "<p>Sesc Turismo Social"
                Texto += "<p>Não é preciso responder este email."
                Texto += "<p>Goiânia, " & Format(Now, "HH:mm") & " horas do dia " & Now.Day.ToString & " de " & MonthName(Now.Month) & " de " & Now.Year.ToString
                Texto += "<p>Esta mensagem pode conter informações confidenciais e/ou privilegiadas."
                Texto += "<br>Se você não for o destinatário ou a pessoa autorizada a recebê-la, não pode usar, copiar ou divulgar as informações nela contidas ou tomar qualquer ação baseada nelas."
                Texto += "<br>Se você recebeu esta mensagem por engano, por favor, avise imediatamente o remetente, e em seguida, apague-a."
                Texto += "<br>Comunicações pela Internet não podem ser garantidas quanto à segurança ou inexistência de erros ou de vírus. O remetente, por esta razão, não aceita responsabilidade por qualquer erro ou omissão no contexto da mensagem decorrente da transmissão via Internet."
                enviarEmail(item.resEmail, Texto, DestinoEmail)
            End If
        Next
        btnConsulta_Click(sender, e)
    End Sub

    Protected Sub btnPostergar_Click(sender As Object, e As EventArgs) Handles btnPostergar.Click
        Dim Texto As String = ""
        Dim listaRanking As String = ""
        Dim destino As String = rblRankingDestino.SelectedValue
        Dim DestinoEmail As String = rblRankingDestino.SelectedValue
        For Each linha As GridViewRow In gdvRankingAcao.Rows
            If CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Checked Then
                listaRanking += CType(linha.FindControl("ckbItemResponsavel"), CheckBox).Attributes("solId") + "."
            End If
        Next
        If destino = "C" Then
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
            destino = "o SESC Caldas Novas"
        Else
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialPiri")
            destino = "a Pousada SESC Pirenópolis"
        End If
        lista = objRankingDAO.acaoIndisponibilidade(listaRanking)


        For Each item As Turismo.RankingVO In lista
            'Se por acaso a lista ficou sem ação, enviarei um e-mail ao cliente somente quando a data inicial for maior ou igual a hoje
            'Por Washington em 25-01-2019 10h
            If Format(CDate(item.dtIni), "yyyy-MM-dd") > Format(Date.Now, "yyyy-MM-dd") Then
                If item.resSexo = "M" Then
                    Texto = "Prezado Sr. " & item.resNome
                Else
                    Texto = "Prezada Sra. " & item.resNome
                End If
                Texto += "<p>Obrigado por utilizar o Sistema Online de Turismo Social do SESC Goiás."
                Texto += "<p>Informarmos que não há disponibilidade para atender a sua solicitação de reserva para " & destino & " no período de " & item.dtIni & " a " & item.dtFim & "."
                Texto += "<p>Considerando que o Sesc Goiás dispõe de unidades de hospedagem em Caldas Novas e Pirenópolis, sugerimos a solicitação para outra unidade ou a mudança de período como nova tentativa."
                Texto += "<p>Dica: antes de fazer o seu pedido verifique as datas com menor procura através do link: "
                'Texto += "<p><a href='http://www.sescgo.com.br/pt-br/site.php?secao=turismosocialprincipal&pub=1136&area=turismosocialprincipal'>www.sescgo.com.br/pt-br/site.php?secao=turismosocialprincipal&pub=1136&area=turismosocialprincipal</a>"
                Texto += "<p><a href='http://www.sescgo.com.br/turismo/aumenteSuasChances/index'>www.sescgo.com.br/turismo/aumenteSuasChances/index</a>"
                Texto += "<p>Caso haja interesse, favor solicitar pelo site!"
                Texto += "<p>À oportunidade agradecemos a preferência."
                Texto += "<p>Atenciosamente"
                Texto += "<p>Sesc Turismo Social"
                Texto += "<p>Não é preciso responder este email."
                Texto += "<p>Goiânia, " & Format(Now, "HH:mm") & " horas do dia " & Now.Day.ToString & " de " & MonthName(Now.Month) & " de " & Now.Year.ToString
                Texto += "<p>Esta mensagem pode conter informações confidenciais e/ou privilegiadas."
                Texto += "<br>Se você não for o destinatário ou a pessoa autorizada a recebê-la, não pode usar, copiar ou divulgar as informações nela contidas ou tomar qualquer ação baseada nelas."
                Texto += "<br>Se você recebeu esta mensagem por engano, por favor, avise imediatamente o remetente, e em seguida, apague-a."
                Texto += "<br>Comunicações pela Internet não podem ser garantidas quanto à segurança ou inexistência de erros ou de vírus. O remetente, por esta razão, não aceita responsabilidade por qualquer erro ou omissão no contexto da mensagem decorrente da transmissão via Internet."
                enviarEmail(item.resEmail, Texto, DestinoEmail)
            End If
        Next
        btnConsulta_Click(sender, e)
    End Sub

    Protected Sub rblRankingOpcao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblRankingOpcao.SelectedIndexChanged, rblRankingDestino.SelectedIndexChanged
        gdvRankingAcao.DataSource = Nothing
        gdvRankingAcao.EmptyDataText = ""
        btnAtender.Visible = (rblRankingOpcao.SelectedValue = "S")
        btnFila.Visible = (rblRankingOpcao.SelectedValue = "S" OrElse rblRankingOpcao.SelectedValue = "N")
        btnPostergar.Visible = (rblRankingOpcao.SelectedValue <> "T")
        gdvRankingAcao.Columns(0).Visible = (rblRankingOpcao.SelectedValue <> "T")
        gdvRankingAcao.Columns(3).Visible = (rblRankingOpcao.SelectedValue = "S")
        gdvRankingAcao.DataBind()
        pnlAcao.Visible = False
    End Sub

    Protected Sub imgBtnResponsavel_Click(sender As Object, e As ImageClickEventArgs)
        lnkBtnNome_Click(sender, e)
    End Sub

    Protected Sub btnVoltarHistorico_Click(sender As Object, e As EventArgs) Handles btnVoltarHistorico.Click
        pnlHistorico.Visible = False
        'pnlHistoricoPesquisa.Visible = False
        If btnVoltarHistorico.CommandArgument = "imgBtnResponsavel" Then
            pnlRanking.Visible = True
            pnlHistoricoPesquisa.Visible = False
            pnlVoltarHistorico.Visible = False
            pnlPercentual.Visible = False
            pnlConfiguracao.Visible = False
            ckbItemResponsavel_CheckedChanged(sender, e)
        ElseIf btnVoltarHistorico.CommandArgument = "lnkBtnNome" Then
            pnlHistoricoPesquisa.Visible = True
            btnVoltarHistorico.CommandArgument = "imgBtnResponsavel"
        End If
        'ckbItemResponsavel_CheckedChanged(sender, e)
    End Sub

    Protected Sub gdvRankingHistoricoCaldas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvRankingHistoricoCaldas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblPeriodoC"), Label).Text = Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni").ToString, 1, 5) + " - " + Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtFim").ToString, 1, 10) + " (" +
                DateDiff(DateInterval.Day, CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtFim"))).ToString + "d)"
            CType(e.Row.FindControl("lblIntegrantesC"), Label).Text =
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome1").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento1")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")" +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido1").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome2").ToString > " ", "<br>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome2").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento2")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido2").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome3").ToString > " ", "<br><font cor=chocolate>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome3").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento3")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido3").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome4").ToString > " ", "<br>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome4").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento4")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido4").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome5").ToString > " ", "<br>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome5").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento5")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido5").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome6").ToString > " ", "<br>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome6").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento6")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido6").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "1", "<font color=green>", If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome7").ToString > " ", "<br>" + gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intNome7").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intDtNascimento7")), CDate(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("intRepetido7").ToString
            CType(e.Row.FindControl("lblDtEnviadaC"), Label).Text = gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtEnviado").ToString
            CType(e.Row.FindControl("lblDtReservadoC"), Label).Text = gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtReservado").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtReservado").ToString().Trim > "", " (" +
                calculaIdade(CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtReservado"), 1, 10))).ToString + "d)", "")
            CType(e.Row.FindControl("lblEsperaC"), Label).Text = gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtListaEspera").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtListaEspera").ToString().Trim > "", " (" +
                DateDiff(DateInterval.Day, CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtListaEspera"), 1, 10))).ToString + "d)", "")
            CType(e.Row.FindControl("lblTrabalhoC"), Label).Text = gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade").ToString +
                If(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade").ToString().Trim > "", " (" +
                DateDiff(DateInterval.Day, CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoCaldas.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade"), 1, 10))).ToString + "d)", "")
        End If
    End Sub

    Protected Sub gdvRankingHistoricoPiri_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvRankingHistoricoPiri.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblPeriodoP"), Label).Text = Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni").ToString, 1, 5) + " - " + Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtFim").ToString, 1, 10) + " (" +
                DateDiff(DateInterval.Day, CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtFim"))).ToString + "d)"
            CType(e.Row.FindControl("lblIntegrantesP"), Label).Text =
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId1").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome1").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento1")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")" +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido1").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId2").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome2").ToString > " ", "<br>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome2").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento2")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido2").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId3").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome3").ToString > " ", "<br><font cor=chocolate>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome3").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento3")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido3").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId4").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome4").ToString > " ", "<br>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome4").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento4")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido4").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId5").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome5").ToString > " ", "<br>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome5").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento5")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido5").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId6").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome6").ToString > " ", "<br>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome6").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento6")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido6").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "1", "<font color=green>", If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("catId7").ToString = "3", "<font color=chocolate>", "<font color=red>")) +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome7").ToString > " ", "<br>" + gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intNome7").ToString + " (" +
                  calculaIdade(CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intDtNascimento7")), CDate(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("dtIni"))).ToString + ")", "") +
                  gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("intRepetido7").ToString
            CType(e.Row.FindControl("lblDtEnviadaP"), Label).Text = gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtEnviado").ToString
            CType(e.Row.FindControl("lblDtReservadoP"), Label).Text = gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtReservado").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtReservado").ToString() > " ", " (" +
                DateDiff(DateInterval.Day, CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtReservado"), 1, 10))).ToString + "d)", "")
            CType(e.Row.FindControl("lblEsperaP"), Label).Text = gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtListaEspera").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtListaEspera").ToString() > " ", " (" +
                DateDiff(DateInterval.Day, CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtListaEspera"), 1, 10))).ToString + "d)", "")
            CType(e.Row.FindControl("lblTrabalhoP"), Label).Text = gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade").ToString +
                If(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade").ToString() > " ", " (" +
                DateDiff(DateInterval.Day, CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtEnviado"), 1, 10)),
                         CDate(Mid(gdvRankingHistoricoPiri.DataKeys(e.Row.RowIndex).Item("solDtSemDisponibilidade"), 1, 10))).ToString + "d)", "")
        End If
    End Sub

    Protected Sub imgBtnGrupo_Click(sender As Object, e As ImageClickEventArgs) Handles imgBtnGrupo.Click
        objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
        If objRankingDAO.atualizaGrupo(hddResId.Value, rblGrupo.SelectedValue) > 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atualizado com sucesso.')", True)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não foi possível atualizar o dado.')", True)
        End If
    End Sub

    Protected Sub imgHistorico_Click(sender As Object, e As ImageClickEventArgs) Handles imgHistorico.Click
        pnlRanking.Visible = False
        pnlHistorico.Visible = False
        pnlAcao.Visible = False
        'txtConsulta.Text = ""
        pnlHistoricoPesquisa.Visible = True
        pnlVoltarHistorico.Visible = True
        txtConsulta.Focus()
        btnVoltarHistorico.CommandArgument = "imgBtnResponsavel"
    End Sub

    Protected Sub btnConsultaHistorico_Click(sender As Object, e As EventArgs) Handles btnConsultaHistorico.Click
        If Len(txtConsulta.Text) > 2 Then
            objResponsavelDAO = New Turismo.ResponsavelDAO("TurismoSocialCaldas")
            gdvRankingUsuario.DataSource = objResponsavelDAO.consultarViaNome(txtConsulta.Text)
            gdvRankingUsuario.DataBind()
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Informe 3 caracteres no mínimo.')", True)
            txtConsulta.Focus()
        End If
    End Sub

    Protected Sub gdvRankingUsuario_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdvRankingUsuario.PageIndexChanging
        gdvRankingUsuario.PageIndex = e.NewPageIndex
        objResponsavelDAO = New Turismo.ResponsavelDAO("TurismoSocialCaldas")
        gdvRankingUsuario.DataSource = objResponsavelDAO.consultarViaNome(txtConsulta.Text)
        gdvRankingUsuario.DataBind()
    End Sub

    Protected Sub gdvRankingUsuario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvRankingUsuario.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkBtnNome"), LinkButton).Attributes.Add("resId", gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("lnkBtnReset"), LinkButton).Attributes.Add("resId", gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resId"))
            CType(e.Row.FindControl("lnkBtnReset"), LinkButton).Visible = (gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resGrupo") = "S")
            CType(e.Row.FindControl("lnkBtnReset"), LinkButton).Attributes.Add("resNome", gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resNome"))
            CType(e.Row.FindControl("lnkBtnReset"), LinkButton).Attributes.Add("resSexo", gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resSexo"))
            CType(e.Row.FindControl("lnkBtnReset"), LinkButton).Attributes.Add("resEmail", gdvRankingUsuario.DataKeys(e.Row.RowIndex).Item("resEmail"))
        End If
    End Sub
    Protected Sub lnkBtnNome_Click(sender As Object, e As EventArgs)
        btnVoltarHistorico.CommandArgument = sender.ID
        pnlRanking.Visible = False
        pnlAcao.Visible = False
        'If rblRankingDestino.SelectedValue = "C" Then
        objResponsavelDAO = New Turismo.ResponsavelDAO("TurismoSocialCaldas")
        'Else
        'objResponsavelDAO = New Turismo.ResponsavelDAO( "TurismoSocialPiri")
        'End If

        objResponsavelVO = objResponsavelDAO.consultar(CInt(sender.Attributes("resId")))
        hddResId.Value = sender.Attributes("resId")
        lbltxtNome.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objResponsavelVO.resNome.ToLower)
        lbltxtNascimento.Text = objResponsavelVO.resDtNascimento.ToString
        lbltxtCPF.Text = objResponsavelVO.resCPFCGC
        If IsNumeric(objResponsavelVO.resCep) Then
            objResponsavelVO.resCep = objResponsavelVO.resCep.Insert(objResponsavelVO.resCep.Length - 3, "-")
        End If

        lbltxtEndereco.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objResponsavelVO.resLogradouro.ToLower.Trim) & ", Qd " &
            objResponsavelVO.resQuadra.Trim & ", Lt " &
            objResponsavelVO.resLote.Trim & ", Nº " &
            objResponsavelVO.resNumero.Trim & ", Bairro " &
            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objResponsavelVO.resBairro.ToLower.Trim) & ", Cidade " &
            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objResponsavelVO.resCidade.ToLower.Trim) & " - " & objResponsavelVO.estDescricao.ToLower.Trim & ", CEP " &
            objResponsavelVO.resCep
        lbltxtPrefixo.Text = "(" & objResponsavelVO.resFonePrefixo & ")"
        lbltxtFone1.Text = "" 'Limpando
        If IsNumeric(objResponsavelVO.resFoneComercial) Then
            objResponsavelVO.resFoneComercial = objResponsavelVO.resFoneComercial.Insert(objResponsavelVO.resFoneComercial.Length - 4, "-")
            lbltxtFone1.Text = lbltxtFone1.Text & "Comercial: " & objResponsavelVO.resFoneComercial
        End If
        If IsNumeric(objResponsavelVO.resFoneResidencial) Then
            objResponsavelVO.resFoneResidencial = objResponsavelVO.resFoneResidencial.Insert(objResponsavelVO.resFoneResidencial.Length - 4, "-")
            lbltxtFone1.Text = lbltxtFone1.Text & " Residencial: " & objResponsavelVO.resFoneResidencial
        End If
        If IsNumeric(objResponsavelVO.resCelular) Then
            objResponsavelVO.resCelular = objResponsavelVO.resCelular.Insert(objResponsavelVO.resCelular.Length - 4, "-")
            lbltxtFone1.Text = lbltxtFone1.Text & " Celular: " & objResponsavelVO.resCelular
        End If
        If IsNumeric(objResponsavelVO.ResContato) Then
            objResponsavelVO.ResContato = objResponsavelVO.ResContato.Insert(objResponsavelVO.ResContato.Length - 4, "-")
            lbltxtFone1.Text = lbltxtFone1.Text & " Emergência: " & objResponsavelVO.ResContato
        Else
            lbltxtFone1.Text = lbltxtFone1.Text & " Emergência: " & objResponsavelVO.ResContato
        End If
        lbltxtFone1.Text = lbltxtFone1.Text
        'lbltxtFone2.Text = "Residencial: " & objResponsavelVO.resFoneResidencial & " Celular: " & objResponsavelVO.resCelular & " Emergência: " & objResponsavelVO.ResContato
        lbltxtEmail.Text = objResponsavelVO.resEmail.ToLower
        lbltxtCadastro.Text = objResponsavelVO.resDtInsercao.ToString
        rblGrupo.SelectedValue = objResponsavelVO.resGrupo

        If rblRankingDestino.Items(0).Enabled Then
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
            gdvRankingHistoricoCaldas.DataSource = objRankingDAO.consultarHistorico(sender.Attributes("resId"))
            gdvRankingHistoricoCaldas.DataBind()
        End If

        If rblRankingDestino.Items(1).Enabled Then
            objRankingDAO = New Turismo.RankingDAO("TurismoSocialPiri")
            gdvRankingHistoricoPiri.DataSource = objRankingDAO.consultarHistorico(sender.Attributes("resId"))
            gdvRankingHistoricoPiri.DataBind()
        End If
        pnlHistorico.Visible = True
        pnlHistoricoPesquisa.Visible = False
        pnlVoltarHistorico.Visible = True
    End Sub

    Protected Sub lnkBtnReset_Click(sender As Object, e As EventArgs)
        objRankingDAO = New Turismo.RankingDAO("TurismoSocialCaldas")
        If objRankingDAO.resetarSenhaGrupo(sender.Attributes("resId")) > 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atualizado com sucesso.')", True)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não foi possível atualizar o dado.')", True)
        End If

        Dim Texto As String = ""
        Texto = IIf(sender.Attributes("resSexo") = "M", "Prezado Sr. ", "Prezada Sra. ") & sender.Attributes("resNome")
        Texto += "<p>Obrigado por utilizar o Sistema Online de Turismo Social do SESC Goiás."
        Texto += "<p>Seu Usuário é: " & sender.Attributes("resEmail")
        Texto += "<p>Sua nova Senha é: senha123"
        Texto += "<p>Acesse o site http://turismosocialweb.sescgo.com.br para poder ter acesso ao seu cadastro."
        Texto += "<p>Em seguida, altere a senha de acordo com sua preferência."
        Texto += "<p>À oportunidade agradecemos a preferência."
        Texto += "<p>Atenciosamente"
        Texto += "<p>Sesc Turismo Social"
        Texto += "<p>Não é preciso responder este email."
        Texto += "<p>Goiânia, " & Format(Now, "HH:mm") & " horas do dia " & Now.Day.ToString & " de " & MonthName(Now.Month) & " de " & Now.Year.ToString
        Texto += "<p>Esta mensagem pode conter informações confidenciais e/ou privilegiadas."
        Texto += "<br>Se você não for o destinatário ou a pessoa autorizada a recebê-la, não pode usar, copiar ou divulgar as informações nela contidas ou tomar qualquer ação baseada nelas."
        Texto += "<br>Se você recebeu esta mensagem por engano, por favor, avise imediatamente o remetente, e em seguida, apague-a."
        Texto += "<br>Comunicações pela Internet não podem ser garantidas quanto à segurança ou inexistência de erros ou de vírus. O remetente, por esta razão, não aceita responsabilidade por qualquer erro ou omissão no contexto da mensagem decorrente da transmissão via Internet."
        enviarEmail(sender.Attributes("resEmail"), Texto, "C")
    End Sub

    Protected Sub imgPercentual_Click(sender As Object, e As ImageClickEventArgs) Handles imgPercentual.Click
        lblPercentual.Text = rblRankingDestino.SelectedItem.Text
        btnVoltarHistorico.CommandArgument = "imgBtnResponsavel"
        pnlRanking.Visible = False
        pnlPercentual.Visible = True
        pnlAcao.Visible = False
        pnlVoltarHistorico.Visible = True
        If rblRankingDestino.Items(0).Selected Then
            objPrevisaoEstadaDAO = New Turismo.PrevisaoEstadaDAO("TurismoSocialCaldas")
        Else
            objPrevisaoEstadaDAO = New Turismo.PrevisaoEstadaDAO("TurismoSocialPiri")
        End If
        gdvPercentual.DataSource = objPrevisaoEstadaDAO.consultar(FormatDateTime(calChegada.SelectedDate, DateFormat.ShortDate), FormatDateTime(calSaida.SelectedDate, DateFormat.ShortDate), "S", "S", "S", "S", "Todos", "N")
        gdvPercentual.DataBind()
    End Sub

    Protected Sub imgConfiguracao_Click(sender As Object, e As ImageClickEventArgs) Handles imgConfiguracao.Click
        'lblConfiguracao.Text = rblRankingDestino.SelectedItem.Text
        'btnVoltarHistorico.CommandArgument = "imgBtnResponsavel"
        'pnlRanking.Visible = False
        'pnlConfiguracao.Visible = True
        'pnlVoltarHistorico.Visible = True
    End Sub

    Protected Sub gdvPercentual_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvPercentual.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblPercentual"), Label).Text =
                Format(CDec(((gdvPercentual.DataKeys(e.Row.RowIndex).Item("aptoE") +
                gdvPercentual.DataKeys(e.Row.RowIndex).Item("aptoI") +
                gdvPercentual.DataKeys(e.Row.RowIndex).Item("aptoM") +
                gdvPercentual.DataKeys(e.Row.RowIndex).Item("aptoP") +
                gdvPercentual.DataKeys(e.Row.RowIndex).Item("aptoR")) /
                gdvPercentual.DataKeys(e.Row.RowIndex).Item("acmQtde"))) * 100, "0.0").ToString
        End If
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
End Class

