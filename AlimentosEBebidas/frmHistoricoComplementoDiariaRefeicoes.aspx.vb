'INFORMAÇÃO:
'SERVIÇO EXTRA DE DIÁRIA - Refeição extra servida na saída do hóspede 

Imports Turismo

Partial Class frmHistoricoComplementoDiariaRefeicoes
    Inherits System.Web.UI.Page
    Dim ObjHistoricoRefeicoesVO As HistoricoRefeicaoVO
    Dim ObjHistoricoRefecoesDAO As HistoricoRefeicaoDAO
    Dim Quantidade As Long = 0, Total As Decimal = 0, Linhas As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Caldas Novas")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Pirenopolis")
            End Select

            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            'Pegando o Nome Completo do usuário logado no momento'
            Dim search As System.DirectoryServices.DirectorySearcher = New System.DirectoryServices.DirectorySearcher("LDAP://sesc-go.com.br/DC=sesc-go,DC=com,DC=br")
            search.Filter = "(SAMAccountName=" + Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", "").ToUpper + ")"
            Dim result As System.DirectoryServices.SearchResult = search.FindOne()
            search.PropertiesToLoad.Add("displayName")
            'Armazenando nome do Usuario no botão para usar na passagem de parametro do relatorio'
            btnConsultar.Attributes.Add("NomeUsuario", result.Properties("displayName").Item(0).ToString)

            btnImprimir.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnGeraEstatistica.click();")
            'Inserindo os anos no combo'
            Dim AnoBase As Integer
            Dim Cont As Integer = 0
            AnoBase = Now.Year + 1
            AnoBase = AnoBase
            While (Cont <= 20)
                drpAno.Items.Add(AnoBase)
                Cont = Cont + 1
                AnoBase = AnoBase - 1
            End While
            'Definindo o mes atual'
            drpMes.SelectedValue = Now.Month
            drpAno.SelectedValue = Now.Year

            'Setando o dia da semana com o dia atual
            Select Case Weekday(Now)
                Case 1 : drpDiaSemana.SelectedValue = 1
                Case 2 : drpDiaSemana.SelectedValue = 2
                Case 3 : drpDiaSemana.SelectedValue = 3
                Case 4 : drpDiaSemana.SelectedValue = 4
                Case 5 : drpDiaSemana.SelectedValue = 5
                Case 6 : drpDiaSemana.SelectedValue = 6
                Case 7 : drpDiaSemana.SelectedValue = 7
            End Select
            'Executando a consulta
            btnConsultar_Click(sender, e)
        End If
    End Sub
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        If rptEstatistica.Visible = True Then
            rptEstatistica.Visible = False
        End If
        pnlGridHistorico.Visible = True
        ObjHistoricoRefeicoesVO = New HistoricoRefeicaoVO
        ObjHistoricoRefecoesDAO = New HistoricoRefeicaoDAO
        gdvHistoricoRef.DataSource = ObjHistoricoRefecoesDAO.HistoricoComplementoDiaria(btnConsultar.Attributes.Item("AliasBancoTurismo"), drpDiaSemana.SelectedValue, drpMes.SelectedValue, drpAno.SelectedValue, drpCaracteristica.SelectedValue, drpHoraAquisicao.SelectedValue)
        gdvHistoricoRef.DataBind()
        If gdvHistoricoRef.Rows.Count = 0 Then
            lblTotRefeicao.Text = "00"
        End If
    End Sub

    Protected Sub gdvHistoricoRef_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvHistoricoRef.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Quantidade = 0
            Total = 0
            Linhas = 0
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Quantidade += CInt(e.Row.Cells(2).Text)
            e.Row.Cells(4).Text = FormatNumber(gdvHistoricoRef.DataKeys(e.Row.RowIndex).Item("Quantidade") * 100 / gdvHistoricoRef.DataKeys(e.Row.RowIndex).Item("CheckOut"), 2)
            e.Row.Cells(4).ToolTip = "Percentual de SED vendidos, em relação ao número de check-out do dia"
            Linhas += 1
            'gdvReserva4.DataKeys(e.Row.RowIndex).Item("venUsuario")
            'Total += CDec(e.Row.Cells(3).Text)
            'e.Row.Cells(3).Text = FormatNumber(e.Row.Cells(3).Text, 2)
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "TOTAL"
            e.Row.Cells(2).Text = "Média " & FormatNumber((Quantidade / Linhas), 2)
            'e.Row.Cells(3).Text = FormatNumber(Total, 2)
            lblTotRefeicao.Text = FormatNumber(Quantidade, 0)
        End If
    End Sub


    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub

    Protected Sub btnEstatistica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'Ira executar o evento btnGeraEstatistica_Click que esta logo abaixo'
    End Sub

    Protected Sub btnGeraEstatistica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeraEstatistica.Click
        Dim MesExtenso As String = ""
        pnlGridHistorico.Visible = False
        Select Case drpMes.SelectedValue
            Case 1 : MesExtenso = "Janeiro"
            Case 2 : MesExtenso = "Fevereiro"
            Case 3 : MesExtenso = "Março"
            Case 4 : MesExtenso = "Abril"
            Case 5 : MesExtenso = "Maio"
            Case 6 : MesExtenso = "Junho"
            Case 7 : MesExtenso = "Julho"
            Case 8 : MesExtenso = "Agosto"
            Case 9 : MesExtenso = "Setembro"
            Case 10 : MesExtenso = "Outubro"
            Case 11 : MesExtenso = "Novembro"
            Case 12 : MesExtenso = "Dezembro"
        End Select

        Dim DiaSemana As String = ""
        Select Case drpDiaSemana.SelectedValue
            Case "T" : DiaSemana = "Todos os dias"
            Case "1" : DiaSemana = "Domingo"
            Case "2" : DiaSemana = "Segunda"
            Case "3" : DiaSemana = "Terça"
            Case "4" : DiaSemana = "Quarta"
            Case "5" : DiaSemana = "Quinta"
            Case "6" : DiaSemana = "Sexta"
            Case "7" : DiaSemana = "Sábado"
        End Select

        Dim Caracteristica As String = ""
        Select Case drpCaracteristica.SelectedValue
            Case "T" : Caracteristica = "Excursões/Passeios/Individuas"
            Case "E" : Caracteristica = "Excursões e passeios"
            Case "I" : Caracteristica = "Reserva individual"
        End Select


        Dim UnidadeOperacional As String = ""
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            UnidadeOperacional = "SESC CALDAS NOVAS"
        Else
            UnidadeOperacional = "POUSADA SESC PIRINÓPOLIS"
        End If

        Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
        With VarSql
            .AppendLine("Declare @Mes varchar(2), @Ano char(4) ")
            .AppendLine("    set @Mes = '" & drpMes.SelectedValue & "' ")
            .AppendLine("    set @Ano = '" & drpAno.SelectedValue & "' ")
            .AppendLine("if (cast(@Mes as integer) < 4) and (@Ano = '2003') ")
            .AppendLine("begin ")
            .AppendLine("   set @Mes = '04' ")
            .AppendLine("   set @Ano = '2003' ")
            .AppendLine("End ")

            .AppendLine("declare ")
            .AppendLine("@Data1 datetime, ")
            .AppendLine("@Data2 datetime ")
            .AppendLine("set @Data1 = convert(datetime, '01/' + @Mes + '/' + @Ano + ' 00:00:00', 103) ")
            .AppendLine("set @Data2 = dateadd(ss, -1, dateadd(m, 1, @Data1)) ")
            .AppendLine("SELECT SUM(1) AS Quantidade,CAST(i.IntDataFim AS Date) as Data, ")
            .AppendLine("(select sum(1) from TbIntegrante ii where CAST(ii.IntDataFim AS DATE) = CAST(i.IntDataFim AS DATE) AND II.ResId > 0 ")
            .AppendLine("and ii.IntDataIniReal is not null ")
            .AppendLine("and Not Exists(Select 1 from tbReserva rr where rr.resId = ii.ResId and rr.ResStatus = 'C') ) as CheckOut, ")
            .AppendLine("case datepart(dw,i.IntDataFim) ")
            .AppendLine("when 1 then 'Domingo' ")
            .AppendLine("when 2 then 'Segunda feira' ")
            .AppendLine("when 3 then 'Terça feira' ")
            .AppendLine("when 4 then 'Quarta feira' ")
            .AppendLine("when 5 then 'Quinta feira' ")
            .AppendLine("when 6 then 'Sexta feria' ")
            .AppendLine("when 7 then 'Sábado' ")
            .AppendLine("end as DiaSemana, ")
            .AppendLine("sum(i.IntTotalAlmoco) as TotalAlmoco ")
            .AppendLine("FROM TbIntegrante I ")
            .AppendLine("INNER JOIN TbReserva R ON R.ResId = I.ResId ")
            .AppendLine("WHERE CAST(I.IntDataFim AS DATE) BETWEEN  @Data1 AND @Data2 ")
            .AppendLine("AND I.RESID > 0  ")
            .AppendLine("AND I.IntAlmoco = 'S' ") 'S=Quem tem almoço na entra e saida'
            If drpDiaSemana.SelectedValue <> "T" Then
                .AppendLine("AND  datepart(dw,i.IntDataFim) = '" & drpDiaSemana.SelectedValue & "' ")
            End If
            If drpCaracteristica.SelectedValue = "I" Then
                .AppendLine("AND R.ResCaracteristica IN ('I') ")
            ElseIf drpCaracteristica.SelectedValue = "E" Then
                .AppendLine("AND R.ResCaracteristica IN ('P','E','T') ")
            End If
            If drpHoraAquisicao.SelectedValue > 0 Then
                .AppendLine("And (DATEPART(Hour,isNull((select top 1 t.IntUsuarioData from tbIntegrantelog t where t.IntId = i.intId and t.IntAlmoco = 'S' order by t.intUsuarioData asc),i.IntUsuarioData)) Between 0 and " & drpHoraAquisicao.SelectedValue - 1 & " ")
                .AppendLine("OR CAST(isNull((select top 1 t.IntUsuarioData from tbIntegrantelog t where t.IntId = i.intId and t.IntAlmoco = 'S' order by t.intUsuarioData asc),i.IntUsuarioData) AS DATE) < CAST(i.IntDataFim AS DATE)) ") 'Aconteceu casos da venda ser antes do dia da saída
            End If
            .AppendLine("GROUP BY CAST(i.IntDataFim AS DATE),datepart(dw,i.IntDataFim) ")
            .AppendLine("ORDER BY CAST(i.IntDataFim AS DATE) ")
            .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
        End With

        Dim ordersPrioridadeLinha As dtsEstatisticaRefeicoesTableAdapters.DataTableRefeicoesTableAdapter = New dtsEstatisticaRefeicoesTableAdapters.DataTableRefeicoesTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelPrioridadeLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSql.ToString)
        Dim rdsRelPrioridadeLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsComplementoDiaria", ordersRelPrioridadeLinha)

        'Passando os parametros'
        Dim ParamsEstatistica(1) As Microsoft.Reporting.WebForms.ReportParameter
        If drpHoraAquisicao.SelectedValue > 0 Then
            ParamsEstatistica(0) = New Microsoft.Reporting.WebForms.ReportParameter("filtro", "Mês de " & MesExtenso & " de " & drpAno.SelectedValue & " | Dia da Semana: " & DiaSemana & " | Tipos de reservas: " & Caracteristica & " | Adquiridas até às " & drpHoraAquisicao.SelectedValue & " Horas ")
        Else
            ParamsEstatistica(0) = New Microsoft.Reporting.WebForms.ReportParameter("filtro", "Mês de " & MesExtenso & " de " & drpAno.SelectedValue & " | Dia da Semana: " & DiaSemana & " | Tipos de reservas: " & Caracteristica & " ")
        End If

        ParamsEstatistica(1) = New Microsoft.Reporting.WebForms.ReportParameter("usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))

        'Chamando o relatório'
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            rptEstatistica.Visible = True
            rptEstatistica.LocalReport.DataSources.Clear()
            rptEstatistica.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
            rptEstatistica.LocalReport.SetParameters(ParamsEstatistica)
        End If
    End Sub
End Class
