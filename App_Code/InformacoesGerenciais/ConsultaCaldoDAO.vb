Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class ConsultaCaldoDAO
    Public Function ConsultaCaldo(ByVal Conexao As String, ByVal data1 As String, ByVal data2 As String) As IList
        Try
            Dim Conn As New Banco.Conexao(Conexao)
            Dim VarSql As New Text.StringBuilder("Set nocount on ")
            With VarSql
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
                .Append("WHERE (ci.CIntData >= '" & Format(CDate(data1), "yyyy-MM-dd") & "' AND ci.CIntData < '" & Format(CDate(data2), "yyyy-MM-dd") & "') AND cii.PrdCod IN(select PrdCod from Tbestoqueproduto where grucod = 600) AND cii.CiiQuantidade > 0 ")
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
            Return PreencheListaCaldos(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaCaldos(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjConsultaCaldoVO As New ConsultaCaldoVO
                With ObjConsultaCaldoVO
                    .Categoria = ResultadoConsulta.Item("Categoria")
                    .Data = ResultadoConsulta.Item("Data")
                    .Qtde = ResultadoConsulta.Item("Qtde")
                    .Valor = FormatNumber(ResultadoConsulta.Item("Valor"), 2)
                End With
                Lista.Add(ObjConsultaCaldoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
