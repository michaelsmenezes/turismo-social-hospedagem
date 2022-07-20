Imports BoletimOcupacaoTableAdapters
Imports Microsoft.Reporting.WebForms
Imports Turismo
'Imports System.Configuration
'Imports System.Web.Configuration
Partial Class InformacoesGerenciais_frmMinisterioTurismo
    Inherits System.Web.UI.Page
    Dim ObjMinisterioTurismoVO As MinisterioTurismoVO
    Dim ObjMinisterioTurismoDAO As MinisterioTurismoDAO
    Dim MediaOcupacaoUH As Decimal = 0
    Dim MediaOcupacaoLE As Decimal = 0
    Dim TotalEntradas As Decimal = 0
    Dim TotalSaidas As Decimal = 0
    Dim TotalSaidasUH As Decimal = 0
    Dim TotalEntradasUH As Decimal = 0
    Dim TotalHospedados As Decimal = 0
    Dim Data1 As String = ""
    Dim Data2 As String = ""

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        ObjMinisterioTurismoVO = New MinisterioTurismoVO
        ObjMinisterioTurismoDAO = New MinisterioTurismoDAO(btnConsultar.Attributes.Item("Banco"))
        rptBHO.Visible = False
        'Montando as datas de acordo com o mês selecionado'
        CalculaData()
        gdvMinisterio.DataSource = ObjMinisterioTurismoDAO.Consultar(Format(CDate(btnConsultar.Attributes.Item("Data1")), "yyyy-MM-dd"), Format(CDate(btnConsultar.Attributes.Item("Data2")), "yyyy-MM-dd"))
        gdvMinisterio.DataBind()
        gdvMinisterio.Visible = True
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Page.Master.FindControl("scpMngTurismoSocial"), ScriptManager).RegisterPostBackControl(btnImprimir)
        If Not IsPostBack Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                btnConsultar.Attributes.Add("Banco", "TurismoSocialGerencialCaldas")
                'Alterando dinamicamente as cores da pagina
                Dim myHtmlLink As New HtmlLink()
                Page.Header.Controls.Remove(myHtmlLink)
                myHtmlLink.Href = "~/stylesheet.css"
                myHtmlLink.Attributes.Add("rel", "stylesheet")
                myHtmlLink.Attributes.Add("type", "text/css")
                'Adicionando o Css na Sessão Head da Página
                Page.Header.Controls.Add(myHtmlLink)
            Else
                btnConsultar.Attributes.Add("Banco", "TurismoSocialGerencialPiri")
                'Alterando dinamicamente as cores da pagina
                Dim myHtmlLink As New HtmlLink()
                Page.Header.Controls.Remove(myHtmlLink)
                myHtmlLink.Href = "~/stylesheetverde.css"
                myHtmlLink.Attributes.Add("rel", "stylesheet")
                myHtmlLink.Attributes.Add("type", "text/css")
                'Adicionando o Css na Sessão Head da Página
                Page.Header.Controls.Add(myHtmlLink)
            End If

            drpAno.DataSource = Nothing
            drpAno.DataBind()
            Dim Contador As Integer = 0
            'Será decrementado 8 anos
            While Contador < 8
                drpAno.Items.Insert(Contador, New ListItem(Year(Now) - Contador, Contador))
                Contador = Contador + 1
            End While
            drpMes.SelectedValue = Month(Now) - 1
        End If
        AplicaCss()
    End Sub

    Protected Sub gdvMinisterio_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvMinisterio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Calculando média ocupacional para pirenópolis ou Caldas Novas
            If btnConsultar.Attributes.Item("Banco") = "TurismoSocialGerencialCaldas" Then
                e.Row.Cells(8).Text = FormatNumber(((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Apartamentos").ToString) * 100) / 309), 2) & " %"
                e.Row.Cells(9).Text = FormatNumber(((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Hospedados").ToString) * 100) / 1177), 2) & " %"
                MediaOcupacaoUH = MediaOcupacaoUH + ((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Apartamentos").ToString) * 100) / 309)
                MediaOcupacaoLE = MediaOcupacaoLE + ((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Hospedados").ToString) * 100) / 1177)
            Else
                e.Row.Cells(8).Text = FormatNumber(((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Apartamentos").ToString) * 100) / 45), 2) & " %"
                e.Row.Cells(9).Text = FormatNumber(((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Hospedados").ToString) * 100) / 134), 2) & " %"
                MediaOcupacaoUH = MediaOcupacaoUH + ((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Apartamentos").ToString) * 100) / 45)
                MediaOcupacaoLE = MediaOcupacaoLE + ((CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Hospedados").ToString) * 100) / 134)
            End If
            TotalEntradas = TotalEntradas + CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Entradas").ToString)
            TotalEntradasUH = TotalEntradasUH + CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("EntradasUH").ToString)
            TotalHospedados = TotalHospedados + CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Hospedados").ToString)
            TotalSaidas = TotalSaidas + CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("Saidas").ToString)
            TotalSaidasUH = TotalSaidasUH + CLng(gdvMinisterio.DataKeys(e.Row.RowIndex()).Item("SaidasUH").ToString)
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Totais"
            e.Row.Cells(1).Text = FormatNumber((TotalEntradas), 0)
            e.Row.Cells(2).Text = FormatNumber((TotalSaidas), 0)
            e.Row.Cells(3).Text = FormatNumber((TotalEntradasUH), 0)
            e.Row.Cells(4).Text = FormatNumber((TotalSaidasUH), 0)
            e.Row.Cells(7).Text = FormatNumber((TotalHospedados), 0)
            e.Row.Cells(8).Text = FormatNumber((MediaOcupacaoUH / gdvMinisterio.Rows.Count), 2) & " %"
            e.Row.Cells(9).Text = FormatNumber((MediaOcupacaoLE / gdvMinisterio.Rows.Count), 2) & " %"
        End If

    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If gdvMinisterio.Visible Then gdvMinisterio.Visible = False
        CalculaData()
        Dim Ministerio As StringBuilder
        Ministerio = New Text.StringBuilder("")

        With Ministerio
            .AppendLine("Declare @DataIni DateTime ")
            .AppendLine("Declare @DataFim DateTime ")

            .AppendLine("Set @DataIni = '" & Format(CDate(btnConsultar.Attributes.Item("Data1")), "yyyy-MM-dd") & "' ")
            .AppendLine("Set @DataFim = '" & Format(CDate(btnConsultar.Attributes.Item("Data2")), "yyyy-MM-dd") & "' ")

            'Tabela Auxiliar que irá receber os valores sem agrupamento para depois serem agrupados 
            .AppendLine("                Create Table #Auxiliar( ")
            .AppendLine("                    Data DateTime, ")
            .AppendLine("                    DataFinal DateTime, ")
            .AppendLine("                    Entradas integer, ")
            .AppendLine("                    Saidas integer, ")
            .AppendLine("                    QtdePessoasDiaMesAnterior integer, ")
            .AppendLine("                    Apartamentos integer, ")
            .AppendLine("                    Leitos integer, ")
            .AppendLine("                    TotLeitos integer, ")
            .AppendLine("                    Hospedados integer ) ")

            'Tabela auxiliar para agrupamento e consulta final par ao relatório 
            .AppendLine("                Create Table #Agrupada( ")
            .AppendLine("                    Data DateTime, ")
            .AppendLine("                    Entradas integer, ")
            .AppendLine("                    Saidas integer, ")
            .AppendLine("                    QtdePessoasDiaMesAnterior integer, ")
            .AppendLine("                    Apartamentos integer, ")
            .AppendLine("                    Leitos integer, ")
            .AppendLine("                    TotLeitos integer, ")
            .AppendLine("                    Hospedados integer ) ")

            'Gerando as Entradas com base no TbMapaHospedagem ainda desagrupadas 
            .AppendLine("                Insert #Auxiliar(Data,Entradas) ")
            .AppendLine("                select CONVERT(varchar(10), h.data ,120), 1 as Entradas ")
            .AppendLine("                from TbMapaHospedagem h ")
            .AppendLine("                where h.data between @DataIni and @DataFim ")
            .AppendLine("                and h.TIPODIARIA = 'I' ")
            .AppendLine("                and exists(select top 1 1 from TbMapaHospedagem hh where hh.INTID = h.INTID and hh.TIPODIARIA = 'A') ")

            'Agrupando a entradas e finalizando as entradas 
            .AppendLine("                Insert #Agrupada(Data,Entradas) ")
            .AppendLine("                select MAX(Data) as Data,COUNT(Entradas) as Entradas from #Auxiliar ")
            .AppendLine("                group by Data ")
            .AppendLine("                order by Data ")

            'Inicio do processo de geração das Saídaas  
            .AppendLine("                Declare @DataAuxiliar as DateTime ")
            .AppendLine("                Declare @Maximo as integer ")
            .AppendLine("                Declare @Contador as integer ")
            .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
            .AppendLine("                Set @Contador = 0 ")
            .AppendLine("                Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")
            'Inserindo as Saidas de forma agrupada e prontas para o relatório 
            .AppendLine("                while @Contador <= @Maximo ")
            .AppendLine("                  BEGIN ")
            .AppendLine("                    update #Agrupada set Saidas = ")
            .AppendLine("                    (select COUNT(1) ")
            .AppendLine("                    from TbMapaHospedagem m ")
            .AppendLine("                    where m.DATA = @DataAuxiliar ")
            .AppendLine("                    and m.TIPODIARIA = 'A' ")
            .AppendLine("                    and not exists (select 1 from TbMapaHospedagem hh where hh.INTID = m.INTID and hh.DATA > m.DATA) ")
            .AppendLine("                    group by m.data) ")
            .AppendLine("                    where Data = @DataAuxiliar ")

            .AppendLine("                    Set @Contador = @Contador + 1 ")
            .AppendLine("                    Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
            .AppendLine("                END  ")

            'Inserindo o total de pessoas Hospedadas com base na tabela TbMapaHospedagem 
            .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
            .AppendLine("                Set @Contador = 0 ")
            .AppendLine("                Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")
            .AppendLine("                while @Contador <= @Maximo ")
            .AppendLine("                     BEGIN ")
            .AppendLine("                        update #Agrupada set Hospedados = ")
            .AppendLine("                       (select COUNT(1) ")
            .AppendLine("                        from TbMapaHospedagem m ")
            .AppendLine("                        where m.DATA =  @DataAuxiliar + 1 ")
            .AppendLine("                        and m.TIPODIARIA = 'A' ")
            .AppendLine("                        group by m.data) ")
            .AppendLine("                        where data = @DataAuxiliar ")
            .AppendLine("                        Set @Contador = @Contador + 1 ")
            .AppendLine("                        Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
            .AppendLine("                     End ")

            'Inserindo a Quantidade o último dia do Mês Anterior
            .AppendLine("                Set @DataAuxiliar = (select min(DATA) from #Agrupada) ")
            .AppendLine("                update #Agrupada set QtdePessoasDiaMesAnterior =  ")
            .AppendLine("                (select COUNT(1) ")
            .AppendLine("                from TbMapaHospedagem h ")
            .AppendLine("                where h.DATA = @DataAuxiliar  ")
            .AppendLine("                and h.TIPODIARIA = 'A' ")
            .AppendLine("                group by h.data) ")

            'Inserindo Apartamentos e Leitos Ocupados em um determinado dia 
            .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
            .AppendLine("                 Set @Contador = 0 ")
            .AppendLine("                 Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")

            .AppendLine("                 while @Contador <= @Maximo ")
            .AppendLine("                     BEGIN ")
            .AppendLine("                        Update #Agrupada Set Apartamentos =  ")
            .AppendLine("                        (select COUNT(ApaId)as Apartamentos from TbLeitosOcupados ")
            .AppendLine("                        where LoDATA = @DataAuxiliar ")
            .AppendLine("                        group by LoData) ")
            .AppendLine("                        where data = @DataAuxiliar ")

            .AppendLine("                        Update #Agrupada Set Leitos = ")
            .AppendLine("                        (select SUM(LoQtde) as Leitos from TbLeitosOcupados ")
            .AppendLine("                        where LoDATA = @DataAuxiliar ")
            .AppendLine("                        group by LoData) ")
            .AppendLine("                        where data = @DataAuxiliar ")

            .AppendLine("                        Set @Contador = @Contador + 1 ")
            .AppendLine("                        Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
            .AppendLine("                 END ")

            'Inserindo o Total Geral de Leitos Ocupados  
            .AppendLine("                 Update #Agrupada set TotLeitos =  ")
            .AppendLine("                    (select sum(((AcmCC * 2) + AcmCS) * AcmQtdeApto)  as TotLeitos  from TbAcomodacao ) ")

            .AppendLine("                 select Data,IsNull((Entradas),0) as Entradas,isNull((Saidas),0) as Saidas,isNull((QtdePessoasDiaMesAnterior),0) as QtdePessoasDiaMesAnterior,isNull((Apartamentos),0) as Apartamentos,isNull((Leitos),0) as Leitos,isNull((TotLeitos),0) as TotLeitos,isNull((Hospedados),0) as Hospedados from  #Agrupada ")
            .AppendLine("                 order by Data ")

            .AppendLine("                 Drop Table #Auxiliar ")
            .AppendLine("                 Drop Table #Agrupada ")
        End With

        Dim ordersBoletimLinha As dtsBoletimTableAdapter = New dtsBoletimTableAdapter
        Dim NrEmbratur As String = ""
        Dim TotLeitos As Long = 0
        Dim CNPJ As String = ""
        Dim Municipio As String = ""
        Dim TotUH As Long = 0
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("Banco").ToString = "TurismoSocialGerencialCaldas" Then
            Dim oTurismo = New ConexaoDAO("TurismoSocialGerencialCaldas")
            ordersBoletimLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            NrEmbratur = "09.047882.20.0008-7"
            TotLeitos = 1177
            CNPJ = "03.671.444/0008-13"
            Municipio = "CALDAS NOVAS"
            TotUH = 309
        Else
            Dim oTurismo = New ConexaoDAO("TurismoSocialGerencialPiri")
            ordersBoletimLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            NrEmbratur = "09.047881.20.0010-0"
            TotLeitos = 134
            CNPJ = "03.671.444/0010-38"
            Municipio = "PIRENÓPOLIS"
            TotUH = 45
        End If

        'Passando os parametros'
        Dim ParamsSugestoes(7) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("nrembratur", NrEmbratur)
        ParamsSugestoes(1) = New ReportParameter("totleitos", TotLeitos)
        ParamsSugestoes(2) = New ReportParameter("diafinal", Day(CDate(btnConsultar.Attributes.Item("Data2"))))
        ParamsSugestoes(3) = New ReportParameter("mes", CStr(Format(CDate(btnConsultar.Attributes.Item("Data2")), "MMMM")))
        ParamsSugestoes(4) = New ReportParameter("ano", Year(CDate(btnConsultar.Attributes.Item("Data2"))))
        ParamsSugestoes(5) = New ReportParameter("cnpj", CNPJ)
        ParamsSugestoes(6) = New ReportParameter("municipio", Municipio)
        ParamsSugestoes(7) = New ReportParameter("totuh", TotUH)

        Dim ordersRelBoletimLinha As System.Data.DataTable = ordersBoletimLinha.GetData(Ministerio.ToString)
        Dim rdsRelBoletimLinha As New ReportDataSource("BoletimOcupacao_DataTableBoletim", ordersRelBoletimLinha)
        If ordersBoletimLinha.GetData(Ministerio.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            rptBHO.Visible = True
            rptBHO.LocalReport.DataSources.Clear()
            rptBHO.LocalReport.DataSources.Add(rdsRelBoletimLinha)
            rptBHO.LocalReport.SetParameters(ParamsSugestoes)
        End If
    End Sub

    Protected Sub CalculaData()
        'Montando as datas de acordo com o mês selecionado'
        Dim Data1 As String = ""
        Dim Data2 As String = ""
        Select Case drpMes.SelectedValue
            Case 1
                Data1 = "01/01/" & drpAno.SelectedItem.ToString
                Data2 = "31/01/" & drpAno.SelectedItem.ToString
            Case 2
                'Se true o ano é Bissexto
                If DateTime.IsLeapYear(drpAno.SelectedItem.ToString) = True Then
                    Data1 = "01/02/" & drpAno.SelectedItem.ToString
                    Data2 = "29/02/" & drpAno.SelectedItem.ToString
                Else
                    Data1 = "01/02/" & drpAno.SelectedItem.ToString
                    Data2 = "28/02/" & drpAno.SelectedItem.ToString
                End If
            Case 3
                Data1 = "01/03/" & drpAno.SelectedItem.ToString
                Data2 = "31/03/" & drpAno.SelectedItem.ToString
            Case 4
                Data1 = "01/04/" & drpAno.SelectedItem.ToString
                Data2 = "30/04/" & drpAno.SelectedItem.ToString
            Case 5
                Data1 = "01/05/" & drpAno.SelectedItem.ToString
                Data2 = "31/05/" & drpAno.SelectedItem.ToString
            Case 6
                Data1 = "01/06/" & drpAno.SelectedItem.ToString
                Data2 = "30/06/" & drpAno.SelectedItem.ToString
            Case 7
                Data1 = "01/07/" & drpAno.SelectedItem.ToString
                Data2 = "31/07/" & drpAno.SelectedItem.ToString
            Case 8
                Data1 = "01/08/" & drpAno.SelectedItem.ToString
                Data2 = "31/08/" & drpAno.SelectedItem.ToString
            Case 9
                Data1 = "01/09/" & drpAno.SelectedItem.ToString
                Data2 = "30/09/" & drpAno.SelectedItem.ToString
            Case 10
                Data1 = "01/10/" & drpAno.SelectedItem.ToString
                Data2 = "31/10/" & drpAno.SelectedItem.ToString
            Case 11
                Data1 = "01/11/" & drpAno.SelectedItem.ToString
                Data2 = "30/11/" & drpAno.SelectedItem.ToString
            Case 12
                Data1 = "01/12/" & drpAno.SelectedItem.ToString
                Data2 = "31/12/" & drpAno.SelectedItem.ToString
        End Select
        btnConsultar.Attributes.Add("Data1", Data1)
        btnConsultar.Attributes.Add("Data2", Data2)
    End Sub

    Protected Sub AplicaCss()
        'Só ira voltar para a primeira página se não tiver dados no grid resultado de uma consulta anterior
        If Session.Item("MasterPage").ToString = "~/TurismoSocial.Master" Then
            'Mudando o CSS
            Dim myHtmlLink As New HtmlLink()
            Page.Header.Controls.Remove(myHtmlLink)
            myHtmlLink.Href = "~/stylesheet.css"
            myHtmlLink.Attributes.Add("rel", "stylesheet")
            myHtmlLink.Attributes.Add("type", "text/css")

            CType(Page.Master.FindControl("lnkPirenopolis"), LinkButton).Text = "Pousada SESC Pirenópolis"
            CType(Page.Master.FindControl("lnkPirenopolis"), LinkButton).ToolTip = "Ir para o SESC de Pirenópolis"
            CType(Page.Master.FindControl("imgHome"), ImageButton).ToolTip = "Ir para o SESC de Pirenópolis"
            CType(Page.Master.FindControl("imgHome"), ImageButton).ImageUrl = "~/images/home_green.png"

            'Adicionando o Css na Sessão Head da Página
            Page.Header.Controls.Add(myHtmlLink)

            'Adicionar o título na página
            Page.Header.Title = "Pré-Passante Caldas Novas"
        Else
            'Mudando o CSS
            Dim myHtmlLink As New HtmlLink()
            Page.Header.Controls.Remove(myHtmlLink)
            myHtmlLink.Href = "~/stylesheetverde.css"
            myHtmlLink.Attributes.Add("rel", "stylesheet")
            myHtmlLink.Attributes.Add("type", "text/css")

            CType(Page.Master.FindControl("lnkPirenopolis"), LinkButton).Text = "Pousada SESC Caldas Novas"
            CType(Page.Master.FindControl("lnkPirenopolis"), LinkButton).ToolTip = "Ir para o SESC de Caldas Novas"
            CType(Page.Master.FindControl("imgHome"), ImageButton).ToolTip = "Ir para o SESC de Caldas Novas"
            CType(Page.Master.FindControl("imgHome"), ImageButton).ImageUrl = "~/images/home_blue.png"

            'Adicionando o Css na Sessão Head da Página
            Page.Header.Controls.Add(myHtmlLink)

            'Adicionar o título na página
            Page.Header.Title = "Pré-Passante Pirenópolis"
        End If
    End Sub
End Class
