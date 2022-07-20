Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class ConferenciaDAO
    Dim ObjConferenciaVO As ConferenciaVO
    Dim ObjConferenciaDAO As ConferenciaDAO
    Public Function Consultar(ByVal ObjConferenciaVO As ConferenciaVO, ByVal Federacao As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                If Federacao <> "" Then
                    'Criando a tabela temporária na memória 
                    .Append("DECLARE @TEMP AS TABLE( ")
                    .Append("APAID SMALLINT, ")
                    .Append("APADESC VARCHAR(10), ")
                    .Append("INTID NUMERIC(18), ")
                    .Append("APACCUSTO VARCHAR(10), ")
                    .Append("APAAREA SMALLINT, ")
                    .Append("EMPRESTIMO CHAR(1) ) ")
                    'Inserindo os dados com um select 
                    .Append("INSERT @TEMP ")
                    .Append("SELECT A.APAID, A.APADESC,H.INTID,A.APACUSTO,A.APAAREA,'' ")
                    .Append("FROM TBAPARTAMENTO A  ")
                    .Append("INNER JOIN TBHOSPEDAGEM H ON H.APAID = A.APAID ")
                    .Append("INNER JOIN TbAptoConferencia C ON A.ApaId = C.ApaId ")
                    .Append("WHERE(Convert(VARCHAR(10), ApaConDtSolicitacao, 103) <= Convert(VARCHAR(10), GETDATE(), 103)) ")
                    .Append("AND c.ApaConDtConferencia IS NULL AND a.ApaDesc LIKE '%" & ObjConferenciaVO.ApaId & "%' ")
                    .Append("AND a.ApaFederacao = '" & Federacao & "' ")
                    .Append("AND H.HOSDATAFIMREAL IS NULL ")
                    .Append("ORDER BY ApaConDtSolicitacao DESC ")
                    'Fazendo a busca final para exibição 
                    .Append("SELECT MAX(P.APAID)AS APAID,MAX(P.APADESC) AS APADESC,MAX(APACCUSTO) AS APACCUSTO,MAX(APAAREA) AS APAAREA, ")
                    .Append("CASE WHEN MAX(C.CSEVALOR) IS NOT NULL THEN 'S' ELSE 'N'END AS EMPRESTIMOS,MAX(C.CSEDESCRICAO) as DESCEMPRESTIMO ")
                    .Append("FROM @TEMP P ")
                    .Append("LEFT JOIN TBCONSUMOSERVICO C ON C.INTID = P.INTID ")
                    .Append("WHERE P.APADESC LIKE '%%' ")
                    .Append("GROUP BY APAID ")
                    .Append("ORDER BY APAID ")
                Else
                    'Criando a tabela temporária na memória 
                    .Append("DECLARE @TEMP AS TABLE( ")
                    .Append("APAID SMALLINT, ")
                    .Append("APADESC VARCHAR(10), ")
                    .Append("INTID NUMERIC(18), ")
                    .Append("APACCUSTO VARCHAR(10), ")
                    .Append("APAAREA SMALLINT, ")
                    .Append("EMPRESTIMO CHAR(1) ) ")
                    'Inserindo os dados com um select 
                    .Append("INSERT @TEMP ")
                    .Append("SELECT A.APAID, A.APADESC,H.INTID,A.APACUSTO,A.APAAREA,'' ")
                    .Append("FROM TBAPARTAMENTO A  ")
                    .Append("INNER JOIN TBHOSPEDAGEM H ON H.APAID = A.APAID ")
                    .Append("INNER JOIN TbAptoConferencia C ON A.ApaId = C.ApaId ")
                    .Append("WHERE(Convert(VARCHAR(10), ApaConDtSolicitacao, 120) <= Convert(VARCHAR(10), GETDATE(), 120)) ")
                    .Append("AND c.ApaConDtConferencia IS NULL AND a.ApaDesc LIKE '%" & ObjConferenciaVO.ApaId & "%' ")
                    '.Append("AND a.ApaFederacao = '" & Federacao & "' ")
                    .Append("AND H.HOSDATAFIMREAL IS NULL ")
                    .Append("ORDER BY ApaConDtSolicitacao DESC ")
                    'Fazendo a busca final para exibição 
                    .Append("SELECT MAX(P.APAID)AS APAID,MAX(P.APADESC) AS APADESC,MAX(APACCUSTO) AS APACCUSTO,MAX(APAAREA) AS APAAREA, ")
                    .Append("CASE WHEN MAX(C.CSEVALOR) IS NOT NULL THEN 'S' ELSE 'N'END AS EMPRESTIMOS,MAX(C.CSEDESCRICAO) as DESCEMPRESTIMO ")
                    .Append("FROM @TEMP P ")
                    .Append("LEFT JOIN TBCONSUMOSERVICO C ON C.INTID = P.INTID ")
                    .Append("WHERE P.APADESC LIKE '%%' ")
                    .Append("GROUP BY APAID ")
                    If AliasBanco = "TurismoSocialPiri" Then
                        .Append("order by APADESC ")
                    Else
                        .Append("order by APAID ")
                    End If
                    '.Append("ORDER BY APAID ")
                End If

                '.Append("IF '" & Federacao & "' <> '' ")
                '.Append("BEGIN ")
                '.Append("SELECT a.ApaId,a.ApaDesc FROM tbaptoconferencia c ")
                '.Append("INNER JOIN TbApartamento a ON A.ApaId = C.ApaId ")
                '.Append("WHERE(Convert(VARCHAR(10), ApaConDtSolicitacao, 103) = Convert(VARCHAR(10), GETDATE(), 103)) ")
                '.Append("AND c.ApaConDtConferencia IS NULL ")
                '.Append("AND a.ApaDesc LIKE '%" & ObjConferenciaVO.ApaId & "%' ")
                '.Append("AND a.ApaFederacao = '" & Federacao & "' ")
                '.Append("ORDER BY ApaConDtSolicitacao DESC ")
                '.Append("End ")
                '.Append("ELSE ")
                '.Append("BEGIN ")
                '.Append("SELECT a.ApaId,a.ApaDesc FROM tbaptoconferencia c ")
                '.Append("INNER JOIN TbApartamento a ON A.ApaId = C.ApaId ")
                '.Append("WHERE(Convert(VARCHAR(10), ApaConDtSolicitacao, 103) = Convert(VARCHAR(10), GETDATE(), 103)) ")
                '.Append("AND c.ApaConDtConferencia IS NULL ")
                '.Append("AND a.ApaDesc LIKE '%" & ObjConferenciaVO.ApaId & "%'  ")
                '.Append("ORDER BY ApaConDtSolicitacao DESC ")
                '.Append("End ")
            End With
            Return Me.PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjConferenciaVO = New ConferenciaVO
                With ObjConferenciaVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .Emprestimos = ResultadoConsulta.Item("Emprestimos")
                    If Convert.IsDBNull(ResultadoConsulta.Item("DescEmprestimo")) Then
                        .DescEmprestimos = " "
                    Else
                        .DescEmprestimos = ResultadoConsulta.Item("DescEmprestimo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaCCusto")) Then
                        .ApaArea = " "
                    Else
                        .ApaCCusto = ResultadoConsulta.Item("ApaCCusto")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaArea")) Then
                        .ApaArea = " "
                    Else
                        .ApaArea = ResultadoConsulta.Item("ApaArea")
                    End If


                    '.TipoAcomodacao = ResultadoConsulta.Item("TipoAcomodacao")
                    '.ApaHTML = ResultadoConsulta.Item("")
                    '.ApaStatus = ResultadoConsulta.Item("")
                End With
                Lista.Add(ObjConferenciaVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function EfetuaConferenciaApto(ByVal ObjConferenciaVO As ConferenciaVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS(SELECT APAID FROM TBAPTOCONFERENCIA WHERE APAID = " & ObjConferenciaVO.ApaId & " AND APACONDTCONFERENCIA IS NULL) ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPTOCONFERENCIA ")
                .Append("SET APACONDTCONFERENCIA = GETDATE() ")
                .Append("WHERE APAID = " & ObjConferenciaVO.ApaId & " ")
                .Append("AND APACONDTCONFERENCIA IS NULL ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
