Imports ConsultaCaldosTableAdapters
Imports Microsoft.Reporting.WebForms
Imports Turismo

Partial Class InformacoesGerenciais_frmConsultaCaldo
    Inherits System.Web.UI.Page
    Dim Soma As New Decimal
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
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
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
            If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            DateAdd(DateInterval.Day, 1, Date.Today)
            Data1.Text = Format(DateAdd(DateInterval.Day, -1, Date.Today), "dd/MM/yyyy")   'Format((Date.Today), "dd/MM/yyyy")
            Data2.Text = Format((Date.Today), "dd/MM/yyyy")
            Data1.Focus()
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        If DateDiff(DateInterval.Day, CDate(Data1.Text), CDate(Data2.Text)) > 30 Then
            pnlGridCaldos.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('O número de dias não pode ser superior a 31.');", True)
            Return
        End If

        pnlGridCaldos.Visible = True
        rptCaldos.Visible = False
        Dim ObjConsultaCaldosVO As New ConsultaCaldoVO
        Dim ObjConsultaCaldosDAO As New ConsultaCaldoDAO
        gdvGridCaldos.DataSource = ObjConsultaCaldosDAO.ConsultaCaldo(btnConsultar.Attributes.Item("AliasBancoTurismo"), Data1.Text, Data2.Text)
        gdvGridCaldos.DataBind()
    End Sub

    Protected Sub gdvGridCaldos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvGridCaldos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(1).Text = "TOTAL DO DIA" Or e.Row.Cells(1).Text = "TOTAL GERAL" Then
                e.Row.Font.Bold = True
            End If
            Soma = Soma + gdvGridCaldos.DataKeys(e.Row.RowIndex).Item(3).ToString
        End If
    End Sub

    Protected Sub btnImprimirCima_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimirCima.Click
        'Impressão de Caldos'
        Dim VarCaldos As StringBuilder
        VarCaldos = New StringBuilder("")
        With VarCaldos
            .Append("SET NOCOUNT ON ")
            .Append("DECLARE @Qtde INTEGER, @QtdeDia INTEGER, @QtdeTotal INTEGER, @Valor Numeric(18,2), @ValorDia Numeric(18,2), @ValorTotal Numeric(18,2), @Id_CM NUMERIC(18) ")
            .Append("DECLARE @Data DATETIME, @DataAux Datetime ")

            .Append("SET @Data = '1900-01-01' ")
            .Append("SET @DataAux = '1900-01-01' ")
            .Append("SET @Qtde = 0 ")
            .Append("SET @QtdeDia = 0 ")
            .Append("SET @QtdeTotal = 0 ")
            .Append("SET @Valor = 0 ")
            .Append("SET @ValorDia = 0 ")
            .Append("SET @ValorTotal = 0 ")

            .Append("CREATE TABLE #TempConsumo(Id_CM INTEGER IDENTITY(1,1), Data Datetime, Categoria VARCHAR(20), Qtde SMALLINT, Valor NUMERIC(18,2)) ")
            .Append("CREATE TABLE #Consumo(Data Varchar(30), Categoria VARCHAR(20), Qtde SMALLINT, Valor NUMERIC(18,2)) ")

            .Append("SELECT CONVERT(DATETIME,CONVERT(VARCHAR(10),ci.CIntData,120)) AS 'Data', ")
            .Append("       CASE c.CatLink ")
            .Append("         WHEN 1 THEN 'COMERCIÁRIO' ")
            .Append("         WHEN 3 THEN 'CONVENIADO' ")
            .Append("                ELSE 'USUÁRIO' ")
            .Append("       END AS 'Categoria', ")
            .Append("       cii.CiiQuantidade AS 'Qtde', cii.CiiQuantidade * cii.LncPreVnd AS 'Valor' ")
            .Append(" INTO #Temp ")
            .Append(" FROM TbConsumoIntegranteItem cii ")
            .Append("       INNER JOIN ")
            .Append("      TbConsumoIntegrante ci ON ci.CIntId = cii.CintId ")
            .Append("       INNER JOIN ")
            .Append("      TbIntegrante i ON ci.IntId = i.IntId ")
            .Append("       INNER JOIN ")
            .Append("      TbCategoria c ON i.CatId = c.CatId ")
            .Append("WHERE (ci.CIntData >= '" & Format(CDate(Data1.Text), "yyyy-MM-dd") & "' AND ci.CIntData < '" & Format(CDate(Data2.Text), "yyyy-MM-dd") & "') AND cii.PrdCod IN(select PrdCod from Tbestoqueproduto where grucod = 600) AND cii.CiiQuantidade > 0 ")
            .Append("ORDER BY ci.CIntData ")

            .Append("INSERT #TempConsumo ")
            .Append("SELECT MAX(Data) AS 'Data', Max(Categoria) as 'Categoria', SUM(Qtde) as 'Qtde', SUM(Valor) AS 'Valor' ")
            .Append("From #Temp ")
            .Append("GROUP BY Data, Categoria ")
            .Append("ORDER BY DATA ")

            .Append("WHILE EXISTS(SELECT TOP 1 1 FROM #TempConsumo) ")
            .Append("BEGIN ")
            .Append("  SELECT TOP 1 @Id_CM = Id_CM, @DataAux = Data, @Qtde = Qtde, @Valor = Valor FROM #TempConsumo ")

            .Append("  IF @DataAux <> '1900-01-01' AND @Data <> @DataAux ")
            .Append("    BEGIN ")
            .Append("      SET @Data = @DataAux ")
            .Append("      INSERT #Consumo ")
            .Append("        SELECT '','TOTAL DO DIA',@QtdeDia, @ValorDia ")
            .Append("      SET @QtdeDia = 0 ")
            .Append("      SET @ValorDia = 0 ")
            .Append("    END ")

            .Append("  SET @QtdeDia = @QtdeDia + @Qtde ")
            .Append("  SET @ValorDia = @ValorDia + @Valor ")
            .Append("  SET @QtdeTotal = @QtdeTotal + @Qtde ")
            .Append("  SET @ValorTotal = @ValorTotal + @Valor ")

            .Append("  INSERT #Consumo ")
            .Append("   SELECT CONVERT(VARCHAR(20),Data,103), Categoria, Qtde, Valor FROM #TempConsumo WHERE Id_CM = @Id_CM ")

            .Append("  DELETE FROM #TempConsumo WHERE Id_CM = @Id_CM ")
            .Append("END ")

            .Append("INSERT #Consumo ")
            .Append(" SELECT '','TOTAL DO DIA',@QtdeDia, @ValorDia ")

            .Append("INSERT #Consumo ")
            .Append(" SELECT '','TOTAL GERAL',@QtdeTotal, @ValorTotal ")

            .Append("SELECT Data, Categoria, Qtde, Replace(Valor,'.',',') as Valor from #Consumo ")

            .Append("DROP TABLE #Temp ")
            .Append("DROP TABLE #TempConsumo ")
            .Append("DROP TABLE #Consumo ")
        End With
        'Passando os parametros'
        Dim Params(3) As ReportParameter
        'Dim params(3) As ReportParameter
        If btnConsultar.Attributes.Item("AliasBancoTurismo") = "TurismoSocial" Then
            Params(0) = New ReportParameter("Unidade", "SESC - Caldas Novas")
        Else
            Params(0) = New ReportParameter("Unidade", "SESC - Pousada Pirenópolis")
        End If
        Params(1) = New ReportParameter("Data1", Format(CDate(Data1.Text), "dd/MM/yyyy"))
        Params(2) = New ReportParameter("Data2", Format(CDate(Data2.Text), "dd/MM/yyyy"))
        Params(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))

        Dim ordersCaldosLinha As DataTableConsultaCaldosTableAdapter = New DataTableConsultaCaldosTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("AliasBancoTurismo") = "TurismoSocial" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersCaldosLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersCaldosLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelCaldosLinha As System.Data.DataTable = ordersCaldosLinha.GetData(VarCaldos.ToString)
        Dim rdsRelCaldosLinha As New ReportDataSource("ConsultaCaldos_DataTableConsultaCaldos", ordersRelCaldosLinha)
        If ordersCaldosLinha.GetData(VarCaldos.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            pnlGridCaldos.Visible = False
            rptCaldos.Visible = True
            rptCaldos.LocalReport.DataSources.Clear()
            rptCaldos.LocalReport.DataSources.Add(rdsRelCaldosLinha)
            rptCaldos.LocalReport.SetParameters(Params)
        Else
            pnlGridCaldos.Visible = True
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
    End Sub

    Protected Sub btnImprimirBaixo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimirBaixo.Click
        btnImprimirCima_Click(sender, e)
    End Sub
End Class
