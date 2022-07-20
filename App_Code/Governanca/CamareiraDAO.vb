Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class CamareiraDAO
    Dim ObjCamareiraVO As CamareiraVO
    Dim ObjCamareiraDAO As CamareiraDAO
    Public Function ConsultarCamareira(ByVal ObjCamareiraVO As CamareiraVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT * FROM TBCAMAREIRA ")
                .Append("WHERE CAMNOME LIKE '%" & ObjCamareiraVO.CamNome & "%'")
                .Append("AND CAMSITUACAO = 'A' ")
                .Append("ORDER BY CAMNOME ASC ")
            End With
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjCamareiraVO = New CamareiraVO
                With ObjCamareiraVO
                    .CamId = ResultadoConsulta.Item("CamId")
                    .CamNome = ResultadoConsulta.Item("CamNome")
                End With
                Lista.Add(ObjCamareiraVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function Exclui(ByVal ObjCamareiraVO As CamareiraVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS(SELECT CAMID FROM TBCAMAREIRA WHERE CAMID = " & ObjCamareiraVO.CamId & ") ")
                .Append("BEGIN ")
                .Append("UPDATE TBCAMAREIRA SET CAMSITUACAO = 'E' ")
                .Append("WHERE CAMID = " & ObjCamareiraVO.CamId & " ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("IF @@ERROR > 0 GOTO ERRO ")
                .Append("SELECT 3 GOTO SAIDA ")
                .Append("ERRO: SELECT 0 GOTO SAIDA ")
                .Append("ERRO2: SELECT -1 GOTO SAIDA ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function Inserir(ByVal ObjCamareiraVO As CamareiraVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF NOT EXISTS(SELECT CAMID FROM TBCAMAREIRA WHERE CAMID =" & ObjCamareiraVO.CamId & ") ")
                .Append("BEGIN ")
                .Append("INSERT INTO TBCAMAREIRA(CAMNOME,CAMSITUACAO) VALUES ")
                .Append("('" & ObjCamareiraVO.CamNome & "','" & ObjCamareiraVO.CamSituacao & "') ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("IF EXISTS(SELECT CAMID FROM TBCAMAREIRA WHERE CAMID =" & ObjCamareiraVO.CamId & ") ")
                .Append("BEGIN ")
                .Append("UPDATE TBCAMAREIRA SET CAMNOME='" & ObjCamareiraVO.CamNome & "',CAMSITUACAO='" & ObjCamareiraVO.CamSituacao & "' ")
                .Append("WHERE CAMID = " & ObjCamareiraVO.CamId & " ")
                .Append("SELECT 2 GOTO SAIDA ")
                .Append("END ")
                .Append("IF @@ERROR > 0 GOTO ERRO ")
                .Append("SELECT 3 GOTO SAIDA ")
                .Append("ERRO: SELECT 0 GOTO SAIDA ")
                .Append("ERRO2: SELECT -1 GOTO SAIDA ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
