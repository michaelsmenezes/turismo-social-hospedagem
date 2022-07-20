Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class BloqueiAptoDAO
    Dim ObjBloqueioAptoVO As BloqueioAptoVO
    Dim ObjBloqueioAptoDAO As BloqueiAptoDAO
    Public Function AtualizaAptoGovernanca(ByVal ObjBloqueioAptoVO As BloqueioAptoVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim Varsql = New Text.StringBuilder("SET NOCOUNT ON ")
            With Varsql
                'Essa procedure pega apartamento limpo e passa para arrumação desde que os apartamentos estejam limpos ou em arrumação
                .Append("DECLARE @ERRO NUMERIC,@Dia smallint, @ROW NUMERIC, @ACM NUMERIC ")
                .Append("SET @DIA = " & ObjBloqueioAptoVO.Dia & " ")
                .Append("IF '" & ObjBloqueioAptoVO.Acao & "' = 'G' ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO ")
                .Append("SET APASTATUS = 'A', APAUSUARIO = '" & ObjBloqueioAptoVO.Usuario & "', APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " ")
                .Append("AND APASTATUS = 'L' ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("IF (@ERRO = 0) AND (@ROW > 0) AND ")
                .Append("NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV WHERE (APAID = " & ObjBloqueioAptoVO.ApaId & " ) AND (ISDATE(AGODATA) = 0)) ")
                .Append("BEGIN ")
                .Append("INSERT TBATENDIMENTOGOV (APAID, AGOORIGEM) VALUES (" & ObjBloqueioAptoVO.ApaId & " ,'" & ObjBloqueioAptoVO.Acao & "') ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")
                .Append("END ")
                .Append("ELSE IF ('" & ObjBloqueioAptoVO.Acao & "' = 'M') AND ('" & ObjBloqueioAptoVO.AlteraManutencao & "' = '0') ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO ")
                .Append("SET APASTATUS = ")
                .Append("CASE ")
                .Append("WHEN APASTATUS IN ('L','A') THEN 'M' ")
                .Append("ELSE APASTATUS ")
                .Append("END, ")
                .Append("APAUSUARIO ='" & ObjBloqueioAptoVO.Usuario & "', APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                'A geração do movimento da manutenção não será mais gerado devido o Help Desk ter assumido essa função
                '.Append("IF (@ERRO = 0) AND (@ROW > 0) ")
                '.Append("BEGIN ")
                '.Append("INSERT TBMANUTENCAO (MANDATAABERTURA, APAID, MANREQUISITANTE, MANDESCRICAOREQUIS)")
                '.Append("VALUES (GETDATE()," & ObjBloqueioAptoVO.ApaId & ",'" & ObjBloqueioAptoVO.Usuario & "','" & ObjBloqueioAptoVO.ManDescricaoResquisitante & "') ")
                '.Append("SET @ERRO = @@ERROR ")
                '.Append("SET @ROW = @@ROWCOUNT ")
                .Append("IF (@ERRO = 0) AND (@ROW > 0) AND (@DIA >= 1) ")
                .Append("BEGIN ")
                'ORIGINAL
                '.Append("SET @ACM = ISNULL((SELECT TOP 1 ACMID FROM TBAPARTAMENTO WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " AND APASTATUS <> 'O'), 0) ")
                '.Append("IF @ACM <> 0 ")
                '.Append("BEGIN ")
                '.Append("IF EXISTS (SELECT 1 FROM TBSOLICITACAO WHERE RESID IS NULL AND APAID = " & ObjBloqueioAptoVO.ApaId & ") ")
                '.Append("UPDATE TBSOLICITACAO SET ")
                '.Append("SOLDATAFIM = GETDATE() + " & ObjBloqueioAptoVO.Dia & ", ")
                '.Append("SOLDATAFIMAUX = GETDATE() + " & ObjBloqueioAptoVO.Dia & ", ")
                '.Append("SOLUSUARIO = '" & ObjBloqueioAptoVO.Usuario & "', ")
                '.Append("SOLUSUARIODATA = GETDATE() ")
                '.Append("WHERE RESID IS NULL AND APAID = " & ObjBloqueioAptoVO.ApaId & " AND SOLDATAFIM < GETDATE() + " & ObjBloqueioAptoVO.Dia & " ")
                '.Append("ELSE ")
                '.Append("INSERT TBSOLICITACAO (ACMID, ACMIDCOBRANCA, APAID, SOLDATAINI, SOLHORAINI, SOLDATAFIM, SOLHORAFIM, ")
                '.Append("SOLDATAINIAUX, SOLDATAFIMAUX, SOLUSUARIO, SOLUSUARIODATA) VALUES(")
                '.Append("@ACM,@ACM," & ObjBloqueioAptoVO.ApaId & " ,GETDATE(),DATEPART(HH, GETDATE()),")
                '.Append("GETDATE() + " & ObjBloqueioAptoVO.Dia & " ,DATEPART(HH, GETDATE() + " & ObjBloqueioAptoVO.Dia & "),")
                '.Append("GETDATE(),GETDATE() + " & ObjBloqueioAptoVO.Dia & ",'" & ObjBloqueioAptoVO.Usuario & "',GETDATE()) ")
                '.Append("SET @ERRO = @@ERROR ")
                '.Append("SET @ROW = @@ROWCOUNT ")
                '.Append("END ")
                'ALTERADO
                .Append("SET @ACM = ISNULL((SELECT TOP 1 ACMID FROM TBAPARTAMENTO WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " AND APASTATUS <> 'O'), 0) ")
                .Append("IF @ACM <> 0 AND @Dia > 0")
                .Append("BEGIN ")
                .Append("IF NOT EXISTS (SELECT 1 FROM TBSOLICITACAO WHERE RESID IS NULL AND APAID = " & ObjBloqueioAptoVO.ApaId & ") ")
                '.Append("UPDATE TBSOLICITACAO SET ")
                '.Append("SOLDATAFIM = GETDATE() + " & ObjBloqueioAptoVO.Dia & ", ")
                '.Append("SOLDATAFIMAUX = GETDATE() + " & ObjBloqueioAptoVO.Dia & ", ")
                '.Append("SOLUSUARIO = '" & ObjBloqueioAptoVO.Usuario & "', ")
                '.Append("SOLUSUARIODATA = GETDATE() ")
                '.Append("WHERE RESID IS NULL AND APAID = " & ObjBloqueioAptoVO.ApaId & " AND SOLDATAFIM < GETDATE() + " & ObjBloqueioAptoVO.Dia & " ")
                '.Append("ELSE ")
                .Append("INSERT TBSOLICITACAO (ACMID, ACMIDCOBRANCA, APAID, SOLDATAINI, SOLHORAINI, SOLDATAFIM, SOLHORAFIM, ")
                .Append("SOLDATAINIAUX, SOLDATAFIMAUX, SOLUSUARIO, SOLUSUARIODATA) VALUES(")
                '.Append("@ACM,@ACM," & ObjBloqueioAptoVO.ApaId & " , GETDATE(),DATEPART(HH, GETDATE()),")
                .Append("@ACM,@ACM," & ObjBloqueioAptoVO.ApaId & " ,Convert(datetime,convert(char(10),GETDATE(),101) + ' 14:00:00',101),DATEPART(HH, convert(char(10),GETDATE(),101)+' 14:00:00'), ")
                'CONVERT(datetime,convert(char(10),getdate(),101)+' 12:00:00',101) '
                '.Append("GETDATE() + " & ObjBloqueioAptoVO.Dia & " ,DATEPART(HH, GETDATE() + " & ObjBloqueioAptoVO.Dia & "),")
                .Append("convert(datetime,convert(char(10),GETDATE() + " & ObjBloqueioAptoVO.Dia & ",101) + ' 10:00:00',101) ,DATEPART(HH, convert(char(10),GETDATE() + " & ObjBloqueioAptoVO.Dia & ",101) + ' 10:00:00'), ")
                '.Append("GETDATE(),GETDATE() + " & ObjBloqueioAptoVO.Dia & ",'" & ObjBloqueioAptoVO.Usuario & "',GETDATE()) ")
                .Append("convert(datetime,convert(char(10),GETDATE(),101)+' 14:00:00',101),convert(DateTime,convert(char(10),GETDATE() + " & ObjBloqueioAptoVO.Dia & ",101) + ' 10:00:00',101),'" & ObjBloqueioAptoVO.Usuario & "',GETDATE()) ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")

                .Append("END ")
                .Append("END ")
                '.Append("END ")
                .Append("ELSE IF ('" & ObjBloqueioAptoVO.Acao & "' = 'M') AND ('" & ObjBloqueioAptoVO.AlteraManutencao & "' <> '0') ")
                .Append("BEGIN ")
                .Append("UPDATE TBMANUTENCAO  ")
                .Append("SET MANDESCRICAOREQUIS = '" & ObjBloqueioAptoVO.Manutencao & "' ")
                .Append("WHERE MANID = '" & ObjBloqueioAptoVO.AlteraManutencao & "' ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")
                .Append("IF @ERRO = 0 AND (@ROW > 0) ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("SAIDA: ")
                Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(Varsql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaApaIdApartamento(ByVal ObjBloqueioAptoVO As BloqueioAptoVO, ByVal AliasBanco As String) As BloqueioAptoVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim Varsql = New Text.StringBuilder("SET NOCOUNT ON ")
            With Varsql
                Varsql.Append("SELECT APAID FROM TBAPARTAMENTO WHERE APACUSTO = '" & Trim(ObjBloqueioAptoVO.CCusto) & "'  AND APAAREA = " & ObjBloqueioAptoVO.Area & " ")
            End With
            Return PreencheObjeto(Conn.consulta(Varsql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheObjeto(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As BloqueioAptoVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjBloqueioAptoVO = New BloqueioAptoVO
            ObjBloqueioAptoVO.ApaId = ResultadoConsulta.Item("ApaId")
        End If
        ResultadoConsulta.Close()
        Return ObjBloqueioAptoVO
    End Function
End Class
