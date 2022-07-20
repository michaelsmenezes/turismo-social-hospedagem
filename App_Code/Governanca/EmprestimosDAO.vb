Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class EmprestimosDAO
    Dim ObjEmprestimosVO As EmprestimosVO
    Dim ObjEmprestimosDAO As EmprestimosDAO
    Public Function ConsultaEmprestimoIntegrante(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT CS.CSEID, CS.INTID, CS.CSEDATA, CS.CSEVALOR, ")
                .Append("CS.TMOID, CS.CSEQUANTIDADE, CS.CSEDESCRICAO, CS.CSEMANUAL, ")
                .Append("ISNULL((SELECT TOP 1 'PAGO   ' FROM TBCXAOPR C WHERE C.CSEID = CS.CSEID),'A PAGAR') AS SITUACAO, ")
                .Append("(SELECT TOP 1 TM.TMODESCRICAO FROM TBTIPOMOVIMENTO TM WHERE TM.TMOID = CS.TMOID) AS OPERACAO, CS.CSEUSUARIO, CS.CSEUSUARIODATA, CS.CSEORIGEM ")
                .Append("FROM TBCONSUMOSERVICO CS WHERE CS.INTID = " & ObjEmprestimosVO.IntId & " AND CS.CSEORIGEM IN ('CS', 'EG') ORDER BY CS.CSEDATA DESC ")
            End With
            Return ListaEmprestimos(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function ListaEmprestimos(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjEmprestimosVO = New EmprestimosVO
                With ObjEmprestimosVO
                    .EmpData = ResultadoConsulta.Item("CSEDATA")
                    .EmpDescricao = ResultadoConsulta.Item("CSEDESCRICAO")
                    .EmpValor = ResultadoConsulta.Item("CSEVALOR")
                    .EmpQuantidade = ResultadoConsulta.Item("CSEQUANTIDADE")
                    .EmpUsuario = ResultadoConsulta.Item("CSEUSUARIO")
                    .EmpDataUsuario = ResultadoConsulta.Item("CSEUSUARIODATA")
                    .CseId = ResultadoConsulta.Item("CSEID")
                    .IntId = ResultadoConsulta.Item("INTID")
                    .TMoId = ResultadoConsulta.Item("TMOID")
                    .EmpManual = ResultadoConsulta.Item("CSEMANUAL")
                    .EmpSituacao = ResultadoConsulta.Item("SITUACAO")
                    .EmpOperacao = ResultadoConsulta.Item("OPERACAO")
                    .EmpOrigem = ResultadoConsulta.Item("CSEORIGEM")

                    '.EmpOperacao = ResultadoConsulta.Item("")
                    '.EmpSituacao = ResultadoConsulta.Item("")
                End With
                Lista.Add(ObjEmprestimosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function SomaEmprestimo(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal AliasBanco As String) As EmprestimosVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT I.INTID, I.INTNOME, ")
                .Append("(SELECT SUM(II.CSEVALOR*II.CSEQUANTIDADE) FROM TBCONSUMOSERVICO II WHERE II.INTID = I.INTID) VALOR")
                .Append("FROM TBINTEGRANTE I WHERE I.INTID = " & ObjEmprestimosVO.IntId & " ")
                Return PrenncheSoma(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PrenncheSoma(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As EmprestimosVO
        If ResultadoConsulta.HasRows Then
            ObjEmprestimosVO = New EmprestimosVO
            With ObjEmprestimosVO
                .IntId = ResultadoConsulta.Item("IntId")
                .IntNome = ResultadoConsulta.Item("IntNome")
                .SomaEmprestimo = ResultadoConsulta.Item("Valor")
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjEmprestimosVO
    End Function
    Public Function PreencheOperacao(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT * FROM VWLISTATIPOOPERACAOCONSUMOSERVICO ")
            End With
            Return PreencheListaOperacao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaOperacao(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjEmprestimosVO = New EmprestimosVO
                With ObjEmprestimosVO
                    .TMoId = ResultadoConsulta.Item("TMoId")
                    .TMoDescricao = ResultadoConsulta.Item("TMoDescricao")
                End With
                Lista.Add(ObjEmprestimosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function InserirEmprestimo(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal HosId As Long, ByVal PRDCOD As Long, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                '.Append("EXEC SpAtualizaConsumoEmprestimo " & ObjEmprestimosVO.CseId & "," & ObjEmprestimosVO.IntId & "," & 0 & ",'" & ObjEmprestimosVO.EmpData & "'," & ObjEmprestimosVO.EmpValor & "," & ObjEmprestimosVO.TMoId & "," & 0 & "," & ObjEmprestimosVO.EmpQuantidade & ",'" & ObjEmprestimosVO.EmpDescricao & "','" & ObjEmprestimosVO.EmpManual & "','" & ObjEmprestimosVO.EmpOperacao & "','" & ObjEmprestimosVO.EmpUsuario & "','" & ObjEmprestimosVO.EmpOrigem & "' ")
                '.Append("IF " & ObjEmprestimosVO.EmpQuantidade & "  <= 0 ")
                '.Append("SET " & ObjEmprestimosVO.EmpQuantidade & " = 1 ")
                .Append("IF ('" & ObjEmprestimosVO.EmpOperacao & "' = 'I') AND EXISTS (SELECT 1 FROM TBINTEGRANTE WHERE INTID = " & ObjEmprestimosVO.IntId & " AND INTSTATUS IN ('E','P')) ")
                .Append("INSERT TBCONSUMOSERVICO(INTID, HOSID, CSEDATA, CSEVALOR, TMOID, PRDCOD, CSEQUANTIDADE, CSEDESCRICAO, CSEMANUAL, CSEUSUARIO, CSEUSUARIODATA, CSEORIGEM) ")
                .Append("VALUES(" & ObjEmprestimosVO.IntId & "," & HosId & ", CONVERT(DATETIME, '" & ObjEmprestimosVO.EmpData & "', 104), " & ObjEmprestimosVO.EmpValor & "," & ObjEmprestimosVO.TMoId & "," & PRDCOD & "," & ObjEmprestimosVO.EmpQuantidade & ",'" & ObjEmprestimosVO.EmpDescricao & "', 'S','" & ObjEmprestimosVO.EmpUsuario & "', GETDATE(),'" & ObjEmprestimosVO.EmpOrigem & "') ")
                .Append("ELSE ")
                .Append("IF ('" & ObjEmprestimosVO.EmpOperacao & "' = 'A') AND EXISTS (SELECT 1 FROM TBINTEGRANTE WHERE INTID = " & ObjEmprestimosVO.IntId & " AND INTSTATUS IN ('E','P')) ")
                .Append("UPDATE TBCONSUMOSERVICO ")
                .Append("SET CSEDATA = CONVERT(DATETIME,'" & ObjEmprestimosVO.EmpData & "', 104), ")
                .Append("CSEVALOR = " & ObjEmprestimosVO.EmpValor & ", ")
                .Append("TMOID = " & ObjEmprestimosVO.TMoId & ", ")
                .Append("CSEQUANTIDADE = " & ObjEmprestimosVO.EmpQuantidade & ", ")
                .Append("CSEDESCRICAO = '" & ObjEmprestimosVO.EmpDescricao & "', ")
                .Append("CSEUSUARIO = '" & ObjEmprestimosVO.EmpUsuario & "', ")
                .Append("CSEUSUARIODATA = GETDATE(), ")
                .Append("CSEORIGEM = '" & ObjEmprestimosVO.EmpOrigem & "' ")
                .Append("WHERE CSEID = " & ObjEmprestimosVO.CseId & " AND CSEMANUAL = 'S' ")
                .Append("AND NOT EXISTS (SELECT 1 FROM TBCXAOPR C WHERE C.CSEID = " & ObjEmprestimosVO.CseId & ") ")
                .Append("ELSE ")
                'ESSA PARTE NÃO SERÁ MAIS UTILIZADA, FOI CRIADA UMA PROCEDURE SEPARADA PARA A EXCLUSÃO
                .Append("IF ('" & ObjEmprestimosVO.EmpOperacao & "' = 'E') AND EXISTS (SELECT 1 FROM TBINTEGRANTE WHERE INTID = " & ObjEmprestimosVO.IntId & " AND INTSTATUS IN ('E','P')) ")
                .Append("BEGIN ")
                .Append("UPDATE TBDEFAULT SET USUARIOLOG = '" & ObjEmprestimosVO.EmpUsuario & "' ")
                .Append("DELETE TBCONSUMOSERVICO WHERE CSEID = " & ObjEmprestimosVO.CseId & "  AND CSEMANUAL = 'S' ")
                .Append("AND NOT EXISTS (SELECT 1 FROM TBCXAOPR C WHERE C.CSEID =" & ObjEmprestimosVO.CseId & ") ")
                .Append("END ")

                .Append("IF @@ERROR = 0 ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("END ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ApagaEmprestimo(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal HosId As Long, ByVal PRDCOD As Long, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                .Append("IF ('" & ObjEmprestimosVO.EmpOperacao & "' = 'E') AND EXISTS (SELECT 1 FROM TBINTEGRANTE WHERE INTID = " & ObjEmprestimosVO.IntId & " AND INTSTATUS IN ('E','P')) ")
                .Append("BEGIN ")
                .Append("UPDATE TBDEFAULT SET USUARIOLOG = '" & ObjEmprestimosVO.EmpUsuario & "' ")
                .Append("DELETE TBCONSUMOSERVICO WHERE CSEID = " & ObjEmprestimosVO.CseId & "  AND CSEMANUAL = 'S' ")
                .Append("AND NOT EXISTS (SELECT 1 FROM TBCXAOPR C WHERE C.CSEID =" & ObjEmprestimosVO.CseId & ") ")
                .Append("END ")
                .Append("IF @@ERROR = 0 ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("END ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaEmprestimoEspecifico(ByVal ObjEmprestimosVO As EmprestimosVO, ByVal AliasBanco As String) As EmprestimosVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("select top 10 CSeId,IntId,HosId,CSeData,CSeValor,TMoId,PrdCod,CSeQuantidade,CSeDescricao,CSeManual,")
                .Append("CSeUsuario,CSeUsuarioData,CSeOrigem,EmpId from tbconsumoservico ")
                .Append("where CSeId = " & ObjEmprestimosVO.CseId & " ")
            End With
            Return PreencheObjetoEmprestimo(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheObjetoEmprestimo(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As EmprestimosVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjEmprestimosVO = New EmprestimosVO
            With ObjEmprestimosVO
                .EmpData = ResultadoConsulta.Item("CSEDATA")
                'ACONTECEU UMA VEZ DE NÃO TER INFORMADO A DESCRIÇÃO E DEU ERRO
                If Convert.IsDBNull(ResultadoConsulta.Item("CSEDESCRICAO")) Then
                    .EmpDescricao = " "
                Else
                    If ResultadoConsulta.Item("CSEDESCRICAO") = "" Then
                        .EmpDescricao = " "
                    Else
                        .EmpDescricao = ResultadoConsulta.Item("CSEDESCRICAO")
                    End If
                End If
                .EmpDescricao = ResultadoConsulta.Item("CSEDESCRICAO")
                .EmpValor = ResultadoConsulta.Item("CSEVALOR")
                .EmpQuantidade = ResultadoConsulta.Item("CSEQUANTIDADE")
                .EmpUsuario = ResultadoConsulta.Item("CSEUSUARIO")
                .EmpDataUsuario = ResultadoConsulta.Item("CSEUSUARIODATA")
                .CseId = ResultadoConsulta.Item("CSEID")
                .IntId = ResultadoConsulta.Item("INTID")
                .TMoId = ResultadoConsulta.Item("TMOID")
                .EmpManual = ResultadoConsulta.Item("CSEMANUAL")
                '.EmpSituacao = ResultadoConsulta.Item("SITUACAO")
                '.EmpOperacao = ResultadoConsulta.Item("OPERACAO")
                .EmpOrigem = ResultadoConsulta.Item("CSEORIGEM")
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjEmprestimosVO
    End Function
End Class
