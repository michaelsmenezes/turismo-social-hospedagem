Imports Microsoft.Reporting.WebForms
Imports dtsRelacaoIntegrantesTableAdapters
Imports Turismo

Partial Class frmAnaliseDemandaEmissivo
    Inherits System.Web.UI.Page

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        If txtDataInicial.Text.Trim.Length = 0 Then
            Mensagem("A data inicial não foi preenchida.")
            txtDataInicial.Focus()
            Return
        End If
        If txtDataFinal.Text.Trim.Length = 0 Then
            Mensagem("A data final não foi preenchida.")
            txtDataFinal.Focus()
            Return
        End If

        Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
        With VarSql
            '.AppendLine(") ")
            .AppendLine("Declare @DataIni as datetime ")
            .AppendLine("Declare @DataFim as DateTime ")
            .AppendLine("Declare @Contador as integer ")

            .AppendLine("SEt @Contador = 0 ")

            .AppendLine("Set @DataIni = '" & Format(CDate(txtDataInicial.Text), "yyyy-MM-dd") & " 00:00:00' ")
            .AppendLine("Set @DataFim = ' " & Format(CDate(txtDataFinal.Text), "yyyy-MM-dd") & " 23:59:59' ")

            .AppendLine("SET NOCOUNT ON Declare @TbBase table( ")
            .AppendLine("    Descricao varchar(60), ")
            .AppendLine("	Idade integer, ")
            .AppendLine("	Salario Dec(18,2) ")
            .AppendLine(") ")

            .AppendLine("SET NOCOUNT ON Declare @TbFinal table( ")
            .AppendLine("    Descricao varchar(60), ")
            .AppendLine("    Porcentagem dec(18,2), ")
            .AppendLine("    Quantidade integer ")
            .AppendLine(") ")

            .AppendLine("SEt @Contador = 0 ")
            .AppendLine("Declare @TotalGeral as decimal(18,2) ")
            .AppendLine("insert @TbBase(Idade,Salario ) ")
            If Format(CDate(txtDataInicial.Text), "yyyy-MM-dd") < Format(Date.Now, "yyyy-MM-dd") Then
                .AppendLine("    (select FLOOR(DATEDIFF (day,i.IntDtNascimento,i.IntDataIniReal)/365.25),i.IntSalario from TbIntegrante i ")
            Else
                .AppendLine("    (select FLOOR(DATEDIFF (day,i.IntDtNascimento,i.IntDataIni)/365.25),i.IntSalario from TbIntegrante i ")
            End If

            If drpTipoDemanda.SelectedValue = "E" Then
                .AppendLine("    inner join TbReserva r on r.ResId = i.ResId ")
                .AppendLine("    where r.ResCaracteristica in ('P','E','T') ")
                .AppendLine("    and i.IntDataIni >= @DataIni ")
                .AppendLine("    and i.IntDataIni <= @DataFim ")
            ElseIf drpTipoDemanda.SelectedValue = "H" Then
                .AppendLine("    inner join TbReserva r on r.ResId = i.ResId ")
                .AppendLine("    where r.ResCaracteristica <> ('P') ")
                .AppendLine("    and i.IntDataIni >= @DataIni ")
                .AppendLine("    and i.IntDataIni <= @DataFim ")
            ElseIf drpTipoDemanda.SelectedValue = "N" Then 'Estarão na unidade independente do check in, não contará o check out do dia
                .AppendLine("inner join TbReserva r on r.ResId = i.ResId ")
                .AppendLine("where @DataIni between i.IntDataIni and i.IntDataFim ")
                .AppendLine("and r.ResStatus not in ('C','F') ")
                .AppendLine("and i.IntDataFimReal is null ")
                .AppendLine("and r.ResCaracteristica <> 'P' ")
                '.AppendLine("    inner join TbReserva r on r.ResId = i.ResId ")
                '.AppendLine("    where r.ResCaracteristica <> ('P') ")
                '.AppendLine("    and @DataIni > i.IntDataIni and  @DataIni + '12:00:00' < i.IntDataFim")
            End If
            .AppendLine("    and r.resStatus <> 'C' ")

            If Format(CDate(txtDataInicial.Text), "yyyy-MM-dd") < Format(Date.Now, "yyyy-MM-dd") And drpTipoDemanda.SelectedValue <> "N" Then
                .AppendLine(" and i.IntDataIniReal is not null ")
            End If
            .AppendLine(" )    --and FLOOR(DATEDIFF (day,i.IntDtNascimento,i.IntDataIni)/365.25) between @Variacao1  and @Variacao2) ")
            If drpTipoDemanda.SelectedValue = "N" Then
                .AppendLine("OPTION (OPTIMIZE FOR(@DataIni='2012-01-01')) ")
            Else
                .AppendLine("OPTION (OPTIMIZE FOR(@DataIni='2012-01-01',@DataFim='2012-01-01')) ")
            End If

            .AppendLine("Set @TotalGeral = (select count(1) from @TbBase) ")

            .AppendLine("insert @TbFinal(Descricao) ")
            .AppendLine("Select 'FAIXA ETÁRIA DA CLIENTELA' ")

            .AppendLine("While @Contador < 7 ")
            .AppendLine("Begin ")
            .AppendLine("   Set @Contador = @Contador + 1 ")
            .AppendLine("   if @Contador = 1 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select 'Até 11 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade >= 0 and idade < 12 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 2 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select '12 a 17 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 11 and idade < 18 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 3 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select '18 a 24 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 17 and idade < 25 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 4 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select '25 a 39 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 24 and idade < 40 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 5 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select '40 a 54 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 39 and idade < 55 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 6 ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select '55 a 64 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 54 and idade < 65 ")
            .AppendLine("	  end ")
            .AppendLine("   if @Contador = 7  ")
            .AppendLine("      begin ")
            .AppendLine("        insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("		select 'A partir de 65 anos:',(count(1)*100/@TotalGeral),count(1) from @TbBase where Idade > 64 ")
            .AppendLine("	  end ")
            .AppendLine("   end ")

            .AppendLine("insert @TbFinal(Descricao) ")
            .AppendLine("Select 'FAIXA SALARIAL DA CLIENTELA' ")

            .AppendLine("Set @Contador = 0 ")
            .AppendLine("While @Contador < 4 ")
            .AppendLine("Begin ")
            .AppendLine(" Set @Contador = @Contador + 1 ")
            .AppendLine(" if @Contador = 1 ")
            .AppendLine("  begin ")
            .AppendLine("    insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("	select '1/3 Salários Mínimos',(count(1)*100/@TotalGeral),count(1) from @TbBase where Salario > 0 and Salario <= 3 ")
            .AppendLine("  end ")
            .AppendLine("if @Contador = 2 ")
            .AppendLine("  begin ")
            .AppendLine("    insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("	select '4/6 Salários Mínimos',(count(1)*100/@TotalGeral),count(1) from @TbBase where Salario > 3 and Salario <= 6 ")
            .AppendLine("  end ")
            .AppendLine("if @Contador = 3 ")
            .AppendLine("  begin ")
            .AppendLine("	insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("	select '7/10 Salários Mínimos',(count(1)*100/@TotalGeral),count(1) from @TbBase where Salario > 6 and Salario <= 10 ")
            .AppendLine("  end ")
            .AppendLine("if @Contador = 4 ")
            .AppendLine("  begin ")
            .AppendLine("    insert @TbFinal(Descricao,Porcentagem,Quantidade) ")
            .AppendLine("	select 'Mais de 10 Salários Mínimos',(count(1)*100/@TotalGeral),count(1) from @TbBase where Salario > 10 and Salario <= 500000 ")
            .AppendLine("  end ")
            .AppendLine("end ")

            .AppendLine("Insert @TbFinal(Descricao) values ('') ")
            .AppendLine("Insert @TbFinal(Descricao,Quantidade) values ('TOTAL GERAL DE REGISTROS:',@TotalGeral) ")

            .AppendLine("select * from @TbFinal ")

        End With
        Dim OrdersRelacaoIntegrantes As RelacaoIntegrantesTableAdapter = New RelacaoIntegrantesTableAdapter

        Dim Unidade As String = ""
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            OrdersRelacaoIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            Unidade = "SESC de Caldas Novas"
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            OrdersRelacaoIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            Unidade = "SESC de Pirenópolis"
        End If

        'Passando os parâmetros'
        Dim ParamsSaidaProdutos(2) As ReportParameter
        If drpTipoDemanda.SelectedValue = "N" Then
            ParamsSaidaProdutos(0) = New ReportParameter("periodo", "Relação do dia " & txtDataInicial.Text & " - " & Unidade & " - " & drpTipoDemanda.SelectedItem.ToString)
        Else
            ParamsSaidaProdutos(0) = New ReportParameter("periodo", "Período de " & txtDataInicial.Text & " até " & txtDataFinal.Text & " - " & Unidade)
        End If
        If drpTipoDemanda.SelectedValue = "E" Then
            ParamsSaidaProdutos(1) = New ReportParameter("unidade", "ANÁLISE DE DEMANDA - TURISMO EMISSIVO")
        Else
            ParamsSaidaProdutos(1) = New ReportParameter("unidade", "ANÁLISE DE DEMANDA - HOSPEDAGEM")
        End If
        ParamsSaidaProdutos(2) = New ReportParameter("usuario", "  Usuário: " & User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))

        Dim ordersRelRelacaoIntegrantes As System.Data.DataTable = OrdersRelacaoIntegrantes.GetData(VarSql.ToString)
        Dim rdsRelRelatorioIntegrantes As New ReportDataSource("dtsAnaliseDemanaEmissivo", ordersRelRelacaoIntegrantes)

        'Chamando o relatório'
        rptAnaliseDemanda.Visible = True
        rptAnaliseDemanda.LocalReport.DataSources.Clear()
        rptAnaliseDemanda.LocalReport.DataSources.Add(rdsRelRelatorioIntegrantes)
        rptAnaliseDemanda.LocalReport.SetParameters(ParamsSaidaProdutos)

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDataInicial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataInicial.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataInicial.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataFinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDataFinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
            txtDataFinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';}")

            txtDataInicial.Text = Format(CDate("01/" & Now.Month & "/" & Now.Year), "dd/MM/yyyy")
            txtDataFinal.Text = Func_Ultimo_Dia_Mes(Today.Date)
            drpTipoDemanda.Focus()
        End If
    End Sub
    Protected Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    'Função que retorna o ultimo dia do mes
    Public Function Func_Ultimo_Dia_Mes(paramDataX As Date) As Date
        Func_Ultimo_Dia_Mes = DateAdd("m", 1, DateSerial(Year(paramDataX), Month(paramDataX), 1))
        Func_Ultimo_Dia_Mes = DateAdd("d", -1, Func_Ultimo_Dia_Mes)
    End Function

    Protected Sub txtDataInicial_TextChanged(sender As Object, e As EventArgs) Handles txtDataInicial.TextChanged
        If IsDate(txtDataInicial.Text) Then
            txtDataFinal.Text = Func_Ultimo_Dia_Mes(CDate(CDate(txtDataInicial.Text)))
            rptAnaliseDemanda.Visible = False
            txtDataFinal.Focus()
        End If
    End Sub

    Protected Sub Mensagem(msn As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + msn + "');", True)
    End Sub

    Protected Sub drpTipoDemanda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpTipoDemanda.SelectedIndexChanged
        If drpTipoDemanda.SelectedValue = "N" Then
            txtDataFinal.Visible = False
            lblDataFinal.Visible = False
        Else
            txtDataFinal.Visible = True
            lblDataFinal.Visible = True
        End If
    End Sub
End Class
