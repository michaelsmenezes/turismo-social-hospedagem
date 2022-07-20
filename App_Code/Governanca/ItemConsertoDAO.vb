Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class ItemConsertoDAO
    Dim ObjItemConsertoVO As ItemConsertoVO
    Dim ObjItemConsertoDAO As ItemConsertoDAO

    Public Function Consultar(ByVal ObjItemConsertoVO As ItemConsertoVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.Append("SELECT iteId,Ltrim(str(iteId)) + ' - ' + iteDescricao as iteDescricao,iteUsuario,iteDataHora ")
            VarSql.Append("FROM TbItemConserto ")
            VarSql.Append("WHERE iteSetorExecutante = '" & ObjItemConsertoVO.SetorExecutante & "' ")
            VarSql.Append("AND iteIdUnidade= '" & ObjItemConsertoVO.IdUnidade & "' ")
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If (ResultadoConsulta.HasRows) Then
            While ResultadoConsulta.Read
                ObjItemConsertoVO = New ItemConsertoVO
                ObjItemConsertoVO.Codigo = ResultadoConsulta.Item("iteId")
                ObjItemConsertoVO.DataHora = ResultadoConsulta.Item("iteDataHora")
                ObjItemConsertoVO.Descricao = ResultadoConsulta.Item("iteDescricao")
                ObjItemConsertoVO.Usuario = ResultadoConsulta.Item("iteUsuario")
                Lista.Add(ObjItemConsertoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultarCodigo(ByVal ObjItemConsertoVO As ItemConsertoVO, ByVal AliasBanco As String) As ItemConsertoVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.Append("SELECT * FROM TbItemConserto ")
            VarSql.Append("WHERE iteId = " & ObjItemConsertoVO.Codigo & " ")
            Return PreencheObjeto(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheObjeto(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As ItemConsertoVO
        If (ResultadoConsulta.HasRows) Then
            ResultadoConsulta.Read()
            ObjItemConsertoVO = New ItemConsertoVO
            ObjItemConsertoVO.Codigo = ResultadoConsulta.Item("iteId")
            ObjItemConsertoVO.DataHora = ResultadoConsulta.Item("iteDataHora")
            ObjItemConsertoVO.Descricao = Trim(ResultadoConsulta.Item("iteDescricao"))
            ObjItemConsertoVO.Usuario = ResultadoConsulta.Item("iteUsuario")
            ObjItemConsertoVO.SetorExecutante = ResultadoConsulta.Item("iteSetorExecutante")
        End If
        ResultadoConsulta.Close()
        Return ObjItemConsertoVO
    End Function
    Public Function Inserir(ByVal ObjItemConsertoVO As ItemConsertoVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("IF NOT EXISTS(SELECT * FROM TbItemConserto WHERE iteId =" & ObjItemConsertoVO.Codigo & ") ")
            VarSql.Append("BEGIN ")
            VarSql.Append("INSERT INTO TbItemConserto(iteDataHora,iteDescricao,iteUsuario,iteSetorExecutante,iteIdUnidade) VALUES ")
            VarSql.Append("(GetDate(),'" & ObjItemConsertoVO.Descricao & "','" & ObjItemConsertoVO.Usuario & "','" & ObjItemConsertoVO.SetorExecutante & "','" & ObjItemConsertoVO.IdUnidade & "') ")
            VarSql.Append("SELECT 1 GOTO SAIDA ")
            VarSql.Append("END ")
            VarSql.Append("IF EXISTS(SELECT * FROM TbItemConserto WHERE iteId =" & ObjItemConsertoVO.Codigo & ") ")
            VarSql.Append("BEGIN ")
            VarSql.Append("UPDATE TbItemConserto SET iteDescricao='" & ObjItemConsertoVO.Descricao & "',iteUsuario='" & ObjItemConsertoVO.Usuario & "',iteSetorExecutante='" & ObjItemConsertoVO.SetorExecutante & "',iteIdUnidade='" & ObjItemConsertoVO.IdUnidade & "' ")
            VarSql.Append("SELECT 2 GOTO SAIDA ")
            VarSql.Append("END ")
            VarSql.Append("IF @@Error > 0 GOTO ERRO ")
            VarSql.Append("SELECT 0 GOTO SAIDA ")
            VarSql.Append("ERRO: SELECT 0 GOTO SAIDA ")
            VarSql.Append("ERRO2: SELECT -1 GOTO SAIDA ")
            VarSql.Append("SAIDA: ")
            Dim resultado As Long = CLng(Conn.executaTransacional(VarSql.ToString))
            Return resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
