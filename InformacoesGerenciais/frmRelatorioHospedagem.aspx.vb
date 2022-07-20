
Imports Turismo

Partial Class InformacoesGerenciais_frmRelatorioHospedagem
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Caldas Novas")
                    drpBlocos.Items.Insert(0, New ListItem("Todos", "0"))
                    drpBlocos.Items.Insert(1, New ListItem("Rio Tocantins", "1"))
                    drpBlocos.Items.Insert(2, New ListItem("Rio Araguaia", "2"))
                    drpBlocos.Items.Insert(3, New ListItem("Rio Vermelho", "4"))
                    drpBlocos.Items.Insert(4, New ListItem("Rio Paranaiba", "3"))
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
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Pirenopolis")
                    'Pirenopolis não terá a seleção de blocos
                    tdBlocos.Visible = False
                    drpBlocos.Visible = False
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
            End Select

            'Inserindo os anos no combo, irá incrementar a partir de 2016
            Dim AnoBase As Integer
            Dim Cont As Integer = 0
            Dim AnoMinimo = 2016
            AnoMinimo = Now.Year - AnoMinimo
            AnoBase = Now.Year
            AnoBase = AnoBase
            While (Cont <= AnoMinimo)
                drpAno.Items.Add(AnoBase)
                Cont = Cont + 1
                AnoBase = AnoBase - 1
            End While

            'Definindo o mes atual'
            drpMes.SelectedValue = Now.Month
            drpAno.SelectedValue = Now.Year
            drpBlocos.SelectedValue = 0

            drpMes.Items.Insert(0, New ListItem("Janeiro", "1"))
            drpMes.Items.Insert(1, New ListItem("Fevereiro", "2"))
            drpMes.Items.Insert(2, New ListItem("Março", "3"))
            drpMes.Items.Insert(3, New ListItem("Abril", "4"))
            drpMes.Items.Insert(4, New ListItem("Maio", "5"))
            drpMes.Items.Insert(5, New ListItem("Junho", "6"))
            drpMes.Items.Insert(6, New ListItem("Julho", "7"))
            drpMes.Items.Insert(7, New ListItem("Agosto", "8"))
            drpMes.Items.Insert(8, New ListItem("Setembro", "9"))
            drpMes.Items.Insert(9, New ListItem("Outubro", "10"))
            drpMes.Items.Insert(10, New ListItem("Novembro", "11"))
            drpMes.Items.Insert(11, New ListItem("Dezembro", "12"))
        End If
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim UnidadeOperacional As String = ""
        Dim BlocoSelecionado As String = ""
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            If drpBlocos.SelectedValue = 0 Then
                BlocoSelecionado = "Sesc Caldas Novas"
            Else
                BlocoSelecionado = "Bloco " & drpBlocos.SelectedItem.ToString
            End If

        Else
            BlocoSelecionado = "Pousada Sesc Pirinópolis"
        End If

        Dim Periodo As String = ""
        Dim InicioMes As String = ""
        Select Case drpMes.SelectedValue
            Case 1
                InicioMes = "01-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 2
                InicioMes = "02-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 3
                InicioMes = "03-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 4
                InicioMes = "04-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 5
                InicioMes = "05-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 6
                InicioMes = "06-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 7
                InicioMes = "07-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 8
                InicioMes = "08-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 9
                InicioMes = "09-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 10
                InicioMes = "10-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 11
                InicioMes = "11-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
            Case 12
                InicioMes = "12-01-" & drpAno.SelectedItem.ToString
                Periodo = Format(CDate(InicioMes), "MM/dd/yyyy") & " Até " & Format(CDate(Func_Ultimo_Dia_Mes(Format(CDate(InicioMes), "MM/dd/yyyy"))), "dd/MM/yyyy")
        End Select

        Dim VarSql = New StringBuilder("SET NOCOUNT ON ")

        With VarSql
            .AppendLine("Declare @InicioMes char(10) ")
            .AppendLine("Declare @FinalMes char(10) ")
            .AppendLine("Declare @UltimoDiaMes Smallint,@Contador Smallint,@BloId SmallInt; ")

            .AppendLine("set @InicioMes = '" & InicioMes & "' ")
            .AppendLine("set @FinalMes =  convert(char(10), (SELECT DATEADD(DD, -DAY(DATEADD(M, 1, @InicioMes)), DATEADD(M, 1, @InicioMes))),120) ")
            .AppendLine("set @UltimoDiaMes = day(@FinalMes) ")


            .AppendLine("select ")
            .AppendLine("DATEPART(DAY,m.DATA) as Dia, ")
            .AppendLine("Sum(case when m.CATID = 1 and M.TIPODIARIA = 'I' then 1 else 0 end) as Com, ")
            .AppendLine("Sum(case when m.CATID IN (2,8,9) and M.TIPODIARIA = 'I' then 1 else 0 end) as Dep, ")
            .AppendLine("Sum(case when m.CATID IN (4,7,10,3,11,12) and M.TIPODIARIA = 'I' then 1 else 0 end) as Usu, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'A' then 1 else 0 end) as Diarias, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'D' then 1 else 0 end) as DayUser, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'I' and m.CAPITAL = 'S' and m.ESTID = 9 then 1 else 0 end) as Capital, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'I' and m.CAPITAL = 'N' and m.ESTID = 9 then 1 else 0 end) as Interior, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'I' and m.CAPITAL = 'S' and m.ESTID <> 9 and m.ESTID <= 27 then 1 else 0 end) as OutEstCapital, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'I' and m.CAPITAL = 'N' and m.ESTID <> 9 and m.ESTID <= 27 then 1 else 0 end) as OutEstInterior, ")
            .AppendLine("Sum(case when M.TIPODIARIA = 'I' and m.ESTID > 27 then 1 else 0 end) as OutrosPaises, ")

            .AppendLine("IsNull((select  sum(LoIntegrante) from TbLeitosOcupados lo ")
            .AppendLine("inner join TbApartamento ap on ap.ApaId = lo.ApaId ")
            .AppendLine("inner join TbAcomodacao ad on ad.AcmId = ap.AcmId ")
            .AppendLine("where lo.LoData = m.DATA ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and ad.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("),0) as LeitosOcupados, ")

            .AppendLine("(select  count (distinct l.Apaid) ")
            .AppendLine("from TbLeitosOcupados l ")
            .AppendLine("inner join TbMapaHospedagem mm on mm.APAID = l.ApaId and mm.DATA = l.LoData ")
            .AppendLine("inner join TbAcomodacao ad on ad.AcmId = mm.ACMID ")
            .AppendLine("where l.LoData = m.DATA ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and ad.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("and mm.TIPODIARIA = 'A')as UhsOcupadas ")

            .AppendLine("from TbMapaHospedagem m ")
            .AppendLine("inner join TbAcomodacao aa on aa.AcmId = m.ACMID ")
            .AppendLine("where M.Data between @InicioMes and @FinalMes ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and aa.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("group by m.DATA ")
            .AppendLine("order by m.DATA ")
        End With

        'Somatório de categorias por Estado - Segunda parte do relatório
        Dim VarSqlEstado = New StringBuilder("SET NOCOUNT ON ")
        With VarSqlEstado
            .AppendLine("Declare @InicioMes char(10) ")
            .AppendLine("Declare @FinalMes char(10) ")
            .AppendLine("Declare @UltimoDiaMes Smallint,@Contador Smallint,@BloId SmallInt; ")

            'Pirenópolis não irá pegar por bloco.
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                .AppendLine("Set @Bloid = " & drpBlocos.SelectedValue & " ")
            Else
                .AppendLine("Set @Bloid = " & 0 & " ")
            End If

            .AppendLine("set @InicioMes = '" & InicioMes & "' ")
            .AppendLine("set @FinalMes =  convert(char(10), (SELECT DATEADD(DD, -DAY(DATEADD(M, 1, @InicioMes)), DATEADD(M, 1, @InicioMes))),120) ")
            .AppendLine("set @UltimoDiaMes = day(@FinalMes) ")
            .AppendLine("Set @Contador = 0 ")

            .AppendLine("select ")
            .AppendLine("e.EstDescricao, ")
            .AppendLine("Sum(case when m.CATID = 1 and M.TIPODIARIA = 'I' then 1 else 0 end) as Com, ")
            .AppendLine("Sum(case when m.CATID IN (2,8,9) and M.TIPODIARIA = 'I' then 1 else 0 end) as Dep, ")
            .AppendLine("Sum(case when m.CATID IN (4,7,10,3,11,12) and M.TIPODIARIA = 'I' then 1 else 0 end) as Usua ")
            .AppendLine("from TbMapaHospedagem m ")
            .AppendLine("inner join TbAcomodacao aa on aa.AcmId = m.ACMID ")
            .AppendLine("inner join TbEstadoPais e on e.EstId = m.ESTID ")
            .AppendLine("where M.Data between @InicioMes and @FinalMes ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and aa.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("group by e.EstDescricao ")
            .AppendLine("order by e.EstDescricao  ")
        End With


        'Apenas DAYUSE
        Dim VarSqlDayUser As New Text.StringBuilder("Set Nocount on ")
        With VarSqlDayUser
            .AppendLine("Declare @InicioMes char(10) ")
            .AppendLine("Declare @FinalMes char(10) ")
            .AppendLine("Declare @UltimoDiaMes Smallint,@Contador Smallint,@BloId SmallInt; ")

            'Pirenópolis não irá pegar por bloco.ç
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                .AppendLine("Set @Bloid = " & drpBlocos.SelectedValue & " ")
            Else
                .AppendLine("Set @Bloid = " & 0 & " ")
            End If

            .AppendLine("set @InicioMes = '" & InicioMes & "' ")
            .AppendLine("set @FinalMes =  convert(char(10), (SELECT DATEADD(DD, -DAY(DATEADD(M, 1, @InicioMes)), DATEADD(M, 1, @InicioMes))),120) ")
            .AppendLine("set @UltimoDiaMes = day(@FinalMes) ")

            .AppendLine("select ")
            .AppendLine("DATEPART(DAY,m.DATA) as Dia, ")
            .AppendLine("Sum(case when m.CATID = 1 and M.TIPODIARIA = 'D' then 1 else 0 end) as DayCom, ")
            .AppendLine("Sum(case when m.CATID IN (2,8,9) and M.TIPODIARIA = 'D' then 1 else 0 end) as DayDep, ")
            .AppendLine("Sum(case when m.CATID IN (4,7,10,11,12) and M.TIPODIARIA = 'D' then 1 else 0 end) as DayUsu ")
            .AppendLine("from TbMapaHospedagem m ")
            .AppendLine("inner join TbAcomodacao aa on aa.AcmId = m.ACMID ")
            .AppendLine("where M.Data between @InicioMes and @FinalMes ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and aa.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("group by m.DATA ")
            .AppendLine("order by m.DATA ")
        End With

        'Somatório de categorias por Estado - Segunda parte do relatório DAYUSE
        Dim VarSqlEstadoDayUser = New StringBuilder("SET NOCOUNT ON ")
        With VarSqlEstadoDayUser
            .AppendLine("Declare @InicioMes char(10) ")
            .AppendLine("Declare @FinalMes char(10) ")
            .AppendLine("Declare @UltimoDiaMes Smallint,@Contador Smallint,@BloId SmallInt; ")

            'Pirenópolis não irá pegar por bloco.
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                .AppendLine("Set @Bloid = " & drpBlocos.SelectedValue & " ")
            Else
                .AppendLine("Set @Bloid = " & 0 & " ")
            End If

            .AppendLine("set @InicioMes = '" & InicioMes & "' ")
            .AppendLine("set @FinalMes =  convert(char(10), (SELECT DATEADD(DD, -DAY(DATEADD(M, 1, @InicioMes)), DATEADD(M, 1, @InicioMes))),120) ")
            .AppendLine("set @UltimoDiaMes = day(@FinalMes) ")

            .AppendLine("select ")
            .AppendLine("e.EstDescricao, ")
            .AppendLine("Sum(case when m.CATID = 1 and M.TIPODIARIA = 'D' then 1 else 0 end) as comDayUser, ")
            .AppendLine("Sum(case when m.CATID IN (2,8,9) and M.TIPODIARIA = 'D' then 1 else 0 end) as DepDayUser, ")
            .AppendLine("Sum(case when m.CATID IN (4,7,10,3,11,12) and M.TIPODIARIA = 'D' then 1 else 0 end) as UsuaDayUser ")
            .AppendLine("from TbMapaHospedagem m ")
            .AppendLine("inner join TbEstadoPais e on e.EstId = m.ESTID ")
            .AppendLine("inner join TbAcomodacao aa on aa.AcmId = m.ACMID ")
            .AppendLine("where M.Data between @InicioMes and @FinalMes ")
            If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
                If drpBlocos.SelectedValue > 0 Then
                    .AppendLine("and aa.BloId = '" & drpBlocos.SelectedValue & "' ")
                End If
            End If
            .AppendLine("group by e.EstDescricao ")
            .AppendLine("order by e.EstDescricao  ")
        End With


        Dim ordersPrioridadeLinha As dtsRelatorioGerencialTableAdapters.DataTableRelHospedagemTableAdapter = New dtsRelatorioGerencialTableAdapters.DataTableRelHospedagemTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("TurismoSocialGerencialCaldas")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("TurismoSocialGerencialPiri")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        'Linhas do relatório do Mapa Principal
        Dim ordersRelPrioridadeLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSql.ToString)
        Dim rdsRelPrioridadeLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsRelatorioHospedagem", ordersRelPrioridadeLinha)
        'Linhas do Rélatório do somatório de clientes por Estado
        Dim ordersRelEstadosLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSqlEstado.ToString)
        Dim rdsRelEstadoLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsTotalEstado", ordersRelEstadosLinha)

        'Linhas do Rélatório DAYUSER
        Dim ordersRelDayUserLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSqlDayUser.ToString)
        Dim rdsRelDayUserLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsDayUser", ordersRelDayUserLinha)

        Dim ordersRelEstadosDayUserLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSqlEstadoDayUser.ToString)
        Dim rdsRelEstadoDayUserLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsRelEstadoDayUser", ordersRelEstadosDayUserLinha)

        'Passando os parametros'
        Dim ParamsEstatistica(2) As Microsoft.Reporting.WebForms.ReportParameter
        ParamsEstatistica(0) = New Microsoft.Reporting.WebForms.ReportParameter("usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsEstatistica(1) = New Microsoft.Reporting.WebForms.ReportParameter("periodo", Periodo)
        ParamsEstatistica(2) = New Microsoft.Reporting.WebForms.ReportParameter("unidade", BlocoSelecionado)

        'Chamando o relatório'
        rptRelatorioHospedagem.Visible = True
        rptRelatorioHospedagem.LocalReport.DataSources.Clear()
        rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
        rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelEstadoLinha)
        rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelDayUserLinha)
        rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelEstadoDayUserLinha)

        'rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelPrioridadeLinhaFunc)
        'rptRelatorioHospedagem.LocalReport.DataSources.Add(rdsRelPrioridadeLinhaRefManual)
        rptRelatorioHospedagem.LocalReport.SetParameters(ParamsEstatistica)
    End Sub
    'Função que retorna o ultimo dia do mes
    Public Function Func_Ultimo_Dia_Mes(paramDataX As Date) As Date
        Func_Ultimo_Dia_Mes = DateAdd("m", 1, DateSerial(Year(paramDataX), Month(paramDataX), 1))
        Func_Ultimo_Dia_Mes = DateAdd("d", -1, Func_Ultimo_Dia_Mes)
    End Function

    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub drpAno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpAno.SelectedIndexChanged
        '    btnConsultar.Enabled = True
        '    drpMes.ToolTip = ""
        '    drpMes.Items.Clear()
        '    Dim Cont = 0
        '    If drpAno.SelectedValue <= CInt(2016) Then
        '        Dim UltimoMesPermito = Now.Month
        '        Cont = 9
        '        While Cont <= UltimoMesPermito
        '            Select Case Cont
        '                Case 9
        '                    drpMes.Items.Insert(0, New ListItem("Setembro", "9"))
        '                Case 10
        '                    drpMes.Items.Insert(1, New ListItem("Outubro", "10"))
        '                Case 11
        '                    drpMes.Items.Insert(2, New ListItem("Novembro", "11"))
        '                Case 12
        '                    drpMes.Items.Insert(3, New ListItem("Dezembro", "12"))
        '            End Select
        '            Cont += 1
        '        End While
        '    ElseIf drpAno.SelectedValue < CInt(2016) Then
        '        drpMes.Items.Clear()
        '        drpMes.Items.Insert(0, New ListItem("Não permitido", "100"))
        '        drpMes.ToolTip = "Escolha um ano acima de 2015"
        '        btnConsultar.Enabled = False
        '    Else
        '        Dim UltimoMesPermito = Now.Month
        '        Cont = 1
        '        While Cont <= UltimoMesPermito
        '            Select Case Cont
        '                Case 1
        '                    drpMes.Items.Insert(0, New ListItem("Janeiro", "1"))
        '                Case 2
        '                    drpMes.Items.Insert(1, New ListItem("Fevereiro", "2"))
        '                Case 3
        '                    drpMes.Items.Insert(2, New ListItem("Março", "3"))
        '                Case 4
        '                    drpMes.Items.Insert(3, New ListItem("Abril", "4"))
        '                Case 5
        '                    drpMes.Items.Insert(4, New ListItem("Maio", "5"))
        '                Case 6
        '                    drpMes.Items.Insert(5, New ListItem("Junho", "6"))
        '                Case 7
        '                    drpMes.Items.Insert(6, New ListItem("Julho", "7"))
        '                Case 8
        '                    drpMes.Items.Insert(7, New ListItem("Agosto", "8"))
        '                Case 9
        '                    drpMes.Items.Insert(8, New ListItem("Setembro", "9"))
        '                Case 10
        '                    drpMes.Items.Insert(9, New ListItem("Outubro", "10"))
        '                Case 11
        '                    drpMes.Items.Insert(10, New ListItem("Novembro", "11"))
        '                Case 12
        '                    drpMes.Items.Insert(11, New ListItem("Dezembro", "12"))
        '            End Select
        '            Cont += 1
        '        End While
        '    End If
    End Sub
End Class
