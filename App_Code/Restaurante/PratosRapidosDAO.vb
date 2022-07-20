Imports Microsoft.VisualBasic
Imports Banco
Imports Uteis
Imports System.Data.SqlClient
Imports System
Imports System.Collections
Imports System.Text
Public Class PratosRapidosDAO
    Dim VarSql As StringBuilder
    Dim Conn As Conexao
    Dim ObjPratosRapidosVO As PratosRapidosVO
    Dim ObjPratosRapidosDOA As PratosRapidosDAO

    Public Function Consultar(ByVal ObjPratosRapidosVO As PratosRapidosVO, ByVal Dia As String, ByVal AliasBanco As String) As IList
        Try
            Conn = New Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("SET NOCOUNT ON ")

                .AppendLine("DECLARE @DATAINI AS DATETIME ,@DATAFIM AS DATETIME ")

                .AppendLine("SET @DATAINI = '" & Dia & "'  ")
                .AppendLine("SET @DATAFIM = '" & Format(DateAdd(DateInterval.Day, 1, (CDate(Dia))), "yyyy-MM-dd") & "' ")

                .AppendLine("SELECT ")
                .AppendLine("CASE GROUPING(RPI.REFPRATOCOD) ")
                .AppendLine("WHEN 1 THEN 0 ")
                .AppendLine("ELSE MAX(RPI.REFPRATOCOD) ")
                .AppendLine("END AS OPCAO, ")
                .AppendLine("CASE GROUPING(RPI.REFPRATOCOD) ")
                .AppendLine("WHEN 1 THEN 'TOTAL GERAL' ")
                .AppendLine("ELSE MAX(RP.REFPRATODESC) ")
                .AppendLine("END AS DESCRICAO, SUM(RPI.REFPRATOINTQTDSOL) AS SOLICITADO, ")
                .AppendLine("0 AS NAOPAGO, ")
                .AppendLine("0 AS CONFIRMADO, ")
                .AppendLine("SUM(RPI.REFPRATOINTQTDSOL - RPI.REFPRATOINTQTD) AS CONSUMIDO, ")
                .AppendLine("SUM(RPI.REFPRATOINTQTDSOL - (RPI.REFPRATOINTQTDSOL - RPI.REFPRATOINTQTD)) AS CONSUMIR ")
                .AppendLine("INTO #REFEICAOPRATOINTEGRANTE ")
                .AppendLine("FROM TBREFEICAOPRATOINTEGRANTE RPI WITH(NOLOCK) ")
                .AppendLine("INNER JOIN ")
                .AppendLine("TBREFEICAOPRATO RP WITH(NOLOCK) ON RPI.REFPRATOCOD = RP.REFPRATOCOD ")
                .AppendLine("INNER JOIN TBINTEGRANTE I WITH(NOLOCK) ON RPI.INTID = I.INTID ")
                .AppendLine("AND (I.INTSTATUS IN('','P','F') AND I.INTDATAINI >= @DATAINI AND I.INTDATAINI < @DATAFIM) ")
                .AppendLine("WHERE RPI.REFPRATOINTDATA >= @DATAINI AND RPI.REFPRATOINTDATA < @DATAFIM ")
                .AppendLine("AND NOT EXISTS(SELECT 1 FROM TbReserva rr WHERE rr.ResId = I.ResId AND rr.ResCaracteristica IN ('E','P','T') AND rr.ResStatus = 'C') ")
                .AppendLine("GROUP BY RPI.REFPRATOCOD WITH CUBE ")
                .AppendLine("OPTION (OPTIMIZE FOR(@DATAINI='2012-01-01',@DATAFIM='2012-01-01')) ")


                .AppendLine("SELECT ")
                .AppendLine("CASE GROUPING(RPI.REFPRATOCOD) ")
                .AppendLine("WHEN 1 THEN 0 ")
                .AppendLine("ELSE MAX(RPI.REFPRATOCOD) ")
                .AppendLine("END AS OPCAO, ")
                .AppendLine("SUM(RPI.REFPRATOINTQTDSOL) AS CONFIRMADO ")
                .AppendLine("INTO #CONFIRMADO ")
                .AppendLine("FROM TBREFEICAOPRATOINTEGRANTE RPI WITH(NOLOCK) ")
                .AppendLine("INNER JOIN ")
                .AppendLine("TBREFEICAOPRATO RP WITH(NOLOCK) ON RPI.REFPRATOCOD = RP.REFPRATOCOD ")
                .AppendLine("INNER JOIN TBINTEGRANTE I WITH(NOLOCK) ON RPI.INTID = I.INTID ")
                .AppendLine("AND ((I.INTSTATUS IN('P','F') OR (I.INTSTATUS = '' AND I.RESID > 0 AND (SELECT R.RESSTATUS FROM TBRESERVA R WHERE R.RESID = I.RESID ) = 'R' )) ")
                .AppendLine("AND I.INTDATAINI >= @DATAINI AND I.INTDATAINI < @DATAFIM) ")
                .AppendLine("WHERE RPI.REFPRATOINTDATA >= @DATAINI AND RPI.REFPRATOINTDATA < @DATAFIM ")
                .AppendLine("AND NOT EXISTS(SELECT 1 FROM TbReserva rr WHERE rr.ResId = I.ResId AND rr.ResCaracteristica IN ('E','P','T') AND rr.ResStatus = 'C') ")
                .AppendLine("GROUP BY RPI.REFPRATOCOD WITH CUBE ")
                .AppendLine("OPTION (OPTIMIZE FOR(@DATAINI='2012-01-01',@DATAFIM='2012-01-01')) ")

                .AppendLine("UPDATE #REFEICAOPRATOINTEGRANTE ")
                .AppendLine("SET CONFIRMADO = C.CONFIRMADO, NAOPAGO = SOLICITADO - C.CONFIRMADO ")
                .AppendLine("FROM #REFEICAOPRATOINTEGRANTE R INNER JOIN #CONFIRMADO C ON R.OPCAO = C.OPCAO ")

                .AppendLine("UPDATE #REFEICAOPRATOINTEGRANTE ")
                .AppendLine("SET NAOPAGO = SOLICITADO ")
                .AppendLine("WHERE NAOPAGO = 0 AND CONFIRMADO = 0 ")

                .AppendLine("SELECT * FROM #REFEICAOPRATOINTEGRANTE ")

                .AppendLine("DROP TABLE #REFEICAOPRATOINTEGRANTE ")
                .AppendLine("DROP TABLE #CONFIRMADO ")
                Return PreencheLista(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjPratosRapidosVO = New PratosRapidosVO
                With ObjPratosRapidosVO
                    .Opcao = ResultadoConsulta.Item("Opcao")
                    .Descricao = ResultadoConsulta.Item("Descricao")
                    .Confirmado = ResultadoConsulta.Item("Confirmado")
                    .Consumido = ResultadoConsulta.Item("Consumido")
                    .Consumir = ResultadoConsulta.Item("Consumir")
                    .NaoPago = ResultadoConsulta.Item("NaoPago")
                    .Solicitado = ResultadoConsulta.Item("Solicitado")
                End With
                Lista.Add(ObjPratosRapidosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaPratosRestaurante(ByVal ObjPratosRapidosVO As PratosRapidosVO, ByVal Dia As String, ByVal AliasBanco As String) As IList
        Try
            Conn = New Conexao(AliasBanco)
            VarSql = New StringBuilder("set nocount on ")
            With VarSql
                .AppendLine("SELECT SUM(I.INTPASANTALM) AS ADQUIRIDOS,(SUM(I.INTPASANTALM) - SUM(I.INTPASTOTALM )) AS ENTRARAMRESTAURANTE,SUM(I.INTPASTOTALM) AS ACONSUMIR ")
                .AppendLine("FROM TBREFEICAOPRATOINTEGRANTE R ")
                .AppendLine("RIGHT JOIN TBINTEGRANTE I ON R.INTID = I.INTID ")
                .AppendLine("WHERE I.INTDATAINI BETWEEN '" & Dia & " 00:00:00' AND '" & Dia & " 23:59:59' ")
                .AppendLine("AND I.INTPASANTALM > 0 ")
                .AppendLine("AND R.REFPRATOCOD IS NULL ")
            End With
            Return PreencheListaPratosRestaurante(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaPratosRestaurante(ByVal ResultadoConsulta As SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjPratosRapidosVO = New PratosRapidosVO
                With ObjPratosRapidosVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Adquiridos")) Then
                        .Confirmado = 0
                    Else
                        .Confirmado = ResultadoConsulta.Item("Adquiridos")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("EntraramRestaurante")) Then
                        .Consumido = 0
                    Else
                        .Consumido = ResultadoConsulta.Item("EntraramRestaurante")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("AConsumir")) Then
                        .Consumir = 0
                    Else
                        .Consumir = ResultadoConsulta.Item("AConsumir")
                    End If
                End With
                Lista.Add(ObjPratosRapidosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function ConsultaPratosParaInsercao(ByVal ObjPratosRapidosVO As PratosRapidosVO, ByVal Dia As String, ByVal AliasBanco As String) As IList
        Try
            Conn = New Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT REFPRATOCOD,REFPRATODESC,REFPRATODISPONIBILIDADE FROM DBO.TBREFEICAOPRATO WHERE REFPRATOSTATUS = 'A' ")
            End With
            Return PreencheListaPratosInsercao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaPratosInsercao(ByVal ResultadoConsulta As SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjPratosRapidosVO = New PratosRapidosVO
                With ObjPratosRapidosVO
                    .praId = ResultadoConsulta.Item("RefPratoCod")
                    .Descricao = ResultadoConsulta.Item("RefPratoDesc")
                    .Consumir = ResultadoConsulta.Item("RefPratoDisponibilidade")
                End With
                Lista.Add(ObjPratosRapidosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function InserePratosRapido(ByVal ObjPratosRapidosVO As PratosRapidosVO, ByVal CodPrato As String, ByVal AliasBanco As String, Usuario As String) As Long
        Try
            Conn = New Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("IF EXISTS (SELECT 1 FROM TBREFEICAOPRATO WHERE REFPRATOCOD = " & CodPrato & ") ")
                .AppendLine("BEGIN ")
                If ObjPratosRapidosVO.Confirmado > 0 Then
                    .AppendLine("   Insert TbRefeicaoPratoLog(RefPratoCod,RefPratoDisponibilidade,RefUsuarioLog,RefUsuarioDataLog) ")
                    .AppendLine("   Values  (" & CodPrato & ",'" & ObjPratosRapidosVO.Confirmado & "','" & Usuario & "',GetDate()) ")
                End If
                .AppendLine("   UPDATE TBREFEICAOPRATO SET REFPRATODISPONIBILIDADE = " & ObjPratosRapidosVO.Confirmado & " WHERE REFPRATOCOD =  " & CodPrato & " ")
                .AppendLine("SELECT 2 GOTO SAIDA ")
                .AppendLine("END ")

                .AppendLine("IF @@ERROR = 0 ")
                .AppendLine("BEGIN ")
                .AppendLine("SELECT 1 GOTO SAIDA ")
                .AppendLine("END ")
                .AppendLine("ELSE ")
                .AppendLine("BEGIN ")
                .AppendLine("SELECT 0 GOTO SAIDA ")
                .AppendLine("END ")
                .AppendLine("SAIDA: ")
            End With
            Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

End Class
