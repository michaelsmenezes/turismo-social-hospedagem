Imports Microsoft.Reporting.WebForms
Imports dtsRelatorioGerencialTableAdapters
Imports Turismo

Partial Class InformacoesGerenciais_frmCortesiasHospedagem
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDataInicial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataInicial.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataInicial.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataFinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataFinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataFinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataInicial.Text = Format(Date.Now, "dd/MM/yyyy")
            txtDataFinal.Text = Format(DateAdd(DateInterval.Day, 30, Date.Now), "dd/MM/yyyy")

            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnImprimir.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnImprimir.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
            End Select
        End If
    End Sub

    Protected Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If txtDataInicial.Text.Trim.Length = 10 Then
            If Not IsDate(txtDataInicial.Text) Then
                txtDataInicial.Focus()
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar uma data válida.');", True)
                Return
            End If
        Else
            txtDataInicial.Focus()
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar uma data válida.');", True)
            Return
        End If

        If txtDataFinal.Text.Trim.Length = 10 Then
            If Not IsDate(txtDataFinal.Text) Then
                txtDataFinal.Focus()
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar uma data válida.');", True)
                Return
            End If
        Else
            txtDataFinal.Focus()
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor informar uma data válida.');", True)
            Return
        End If

        'Impressão de Caldos'
        Dim VarCortesias As StringBuilder
        VarCortesias = New StringBuilder("")
        With VarCortesias
            .AppendLine("SELECT CONVERT(VARCHAR(10), r.ResId) + ' - ' + r.ResNome + ' ' + ' de ' + CONVERT(CHAR(10),r.ResDataIni,103) + ' até ' + CONVERT(CHAR(10),r.ResDataFim,103)Reserva , ")
            .AppendLine("i.IntNome, ")
            .AppendLine("CONVERT(CHAR(10),i.IntDataIniReal,103)IntDataIniReal, ")
            .AppendLine("CONVERT(CHAR(10),i.IntDataFimReal,103)IntDataFimReal, ")
            .AppendLine("h.HosValorDevido,r.ResObs , ")
            .AppendLine("CASE WHEN i.IntFormaPagamento = 'C' THEN 'Cortesia' ")
            .AppendLine("     WHEN i.IntFormaPagamento = 'F' THEN 'Free' ELSE i.IntFormaPagamento END intFormaPagamento, ")
            .AppendLine("a.ApaDesc ,r.ResUsuario,r.ResStatus,r.ResMemorando ")
            .AppendLine("FROM TbReserva r ")
            .AppendLine("INNER JOIN TbIntegrante i ON i.ResId = r.ResId  ")
            .AppendLine("INNER JOIN TbHospedagem h ON h.IntId = i.IntId  ")
            .AppendLine("INNER JOIN TbApartamento a ON a.ApaId = h.ApaId  ")
            'W = Todas reservas menos as canceladas
            If drpStatus.SelectedValue = "W" Then
                .AppendLine("WHERE r.ResStatus <> 'C' ")
            ElseIf drpStatus.SelectedValue = "T" Then
                .AppendLine("WHERE r.ResStatus <> '" & drpStatus.SelectedValue & "' ")
            Else
                .AppendLine("WHERE r.ResStatus = '" & drpStatus.SelectedValue & "' ")
            End If
            If drpFormaPagto.SelectedValue = "FC" Then
                .AppendLine("AND i.IntFormaPagamento IN ('C','F') ")
            Else
                .AppendLine("AND i.IntFormaPagamento = ('" & drpFormaPagto.SelectedValue & "') ")
            End If
            .AppendLine("AND h.HosValorDevido > 0 ")
            .AppendLine("AND r.ResDataIni BETWEEN '" & Format(CDate(txtDataInicial.Text), "yyyy/MM/dd") & "' AND '" & Format(CDate(txtDataFinal.Text), "yyyy/MM/dd") & "' ")
            .AppendLine("AND r.ResCaracteristica = '" & drpCaracteristica.SelectedValue & "' ")
            .AppendLine("AND r.ResId > 0 ")
            .AppendLine("ORDER BY r.ResNome,r.ResDataIni,i.IntNome  ")
        End With

        'Passando os parametros'
        Dim Params(6) As ReportParameter
        'Dim params(3) As ReportParameter
        If btnImprimir.Attributes.Item("AliasBancoTurismo") = "TurismoSocialCaldas" Then
            Params(0) = New ReportParameter("unidade", "SESC - Caldas Novas")
        Else
            Params(0) = New ReportParameter("unidade", "SESC - Pousada Pirenópolis")
        End If
        Params(1) = New ReportParameter("data1", Format(CDate(txtDataInicial.Text), "dd/MM/yyyy"))
        Params(2) = New ReportParameter("data2", Format(CDate(txtDataFinal.Text), "dd/MM/yyyy"))
        Params(3) = New ReportParameter("usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        Params(4) = New ReportParameter("formapagamento", drpFormaPagto.SelectedItem.ToString & "|")
        Params(5) = New ReportParameter("status", drpStatus.SelectedItem.ToString & "|")
        Params(6) = New ReportParameter("caracteristica", drpCaracteristica.SelectedItem.ToString)

        Dim ordersCaldosLinha As DataTableCortesiaHospedagemTableAdapter = New DataTableCortesiaHospedagemTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnImprimir.Attributes.Item("AliasBancoTurismo") = "TurismoSocialCaldas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersCaldosLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersCaldosLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelCortesiaLinha As System.Data.DataTable = ordersCaldosLinha.GetData(VarCortesias.ToString)
        Dim rdsRelCortesiaLinha As New ReportDataSource("dtsCortesiasHospedagem", ordersRelCortesiaLinha)
        If ordersCaldosLinha.GetData(VarCortesias.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            divReport.Visible = True
            rptCortesias.Visible = True
            rptCortesias.LocalReport.DataSources.Clear()
            rptCortesias.LocalReport.DataSources.Add(rdsRelCortesiaLinha)
            rptCortesias.LocalReport.SetParameters(Params)
        Else
            divReport.Visible = False
            rptCortesias.LocalReport.DataSources.Clear()
            rptCortesias.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
    End Sub
End Class
