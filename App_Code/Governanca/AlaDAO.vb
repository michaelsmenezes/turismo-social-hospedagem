Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

Public Class AlaDAO
    Dim ObjAlaVO As AlaVO
    Dim ObjAlaDAO As AlaDAO
    Dim ObjApartamentoVO As ApartamentosVO
    Dim ObjApartamentoDAO As ApartamentosDAO
    Public Function Inserir(ByVal ObjAlaVO As AlaVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                .Append("SET NOCOUNT ON ")
                'Inserindo
                .Append("IF NOT EXISTS (SELECT ALAID FROM TBAPARTAMENTOALA WHERE ALAID = " & ObjAlaVO.AlaId & " )")
                .Append("BEGIN ")
                'Se ja existir uma ala com o nome que esta tentando ser inserido
                .Append("IF EXISTS(SELECT ALANOME FROM TBAPARTAMENTOALA WHERE ALANOME = '" & ObjAlaVO.AlaNome & "') ")
                .Append("BEGIN ")
                .Append("SELECT 4 GOTO SAIDA ")
                .Append("END ")
                'Se a ala ainda não existir insere
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("INSERT INTO TBAPARTAMENTOALA(ALANOME,ALADESCRICAO,CAMID)VALUES ")
                .Append("('" & ObjAlaVO.AlaNome & "','" & ObjAlaVO.AlaDescricao & "'," & ObjAlaVO.CamId & ") ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("END  ")
                'Se não for inserção.. será um update  
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("IF EXISTS (SELECT ALAID FROM TBAPARTAMENTOALA WHERE ALAID =  " & ObjAlaVO.AlaId & " )")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTOALA SET ALANOME='" & ObjAlaVO.AlaNome & "' ,ALADESCRICAO='" & ObjAlaVO.AlaDescricao & "',CAMID=" & ObjAlaVO.CamId & " ")
                .Append("WHERE ALAID = " & ObjAlaVO.AlaId & " ")
                .Append("SELECT 2 GOTO SAIDA ")
                .Append("END ")
                .Append("END ")
                .Append("IF @@ERROR > 0 GOTO ERRO ")
                .Append("SELECT 3 GOTO SAIDA ")
                .Append("ERRO: SELECT 0 GOTO SAIDA ")
                .Append("ERRO2: SELECT -1 GOTO SAIDA ")
                .Append("SAIDA: ")
                Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaAla(ByVal ObjAlaVO As AlaVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT A.ALAID,A.ALANOME,A.ALADESCRICAO,A.CAMID,C.CAMNOME FROM TBAPARTAMENTOALA A ")
                .Append("INNER JOIN TbCamareira C ON C.CamId = A.camId ")
                .Append("WHERE ALANOME LIKE '%" & ObjAlaVO.AlaNome & "%' ")
            End With
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        Try
            If ResultadoConsulta.HasRows Then
                While ResultadoConsulta.Read()
                    ObjAlaVO = New AlaVO
                    With ObjAlaVO
                        .AlaId = ResultadoConsulta.Item("alaId")
                        .AlaNome = ResultadoConsulta.Item("AlaNome")
                        .AlaDescricao = ResultadoConsulta.Item("AlaDescricao")
                        .CamId = ResultadoConsulta.Item("CamId")
                        .CamNome = ResultadoConsulta.Item("CamNome").ToString.ToUpper
                    End With
                    Lista.Add(ObjAlaVO)
                End While
            End If
            ResultadoConsulta.Close()
            Return Lista
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaAlaCodigo(ByVal ObjAlaVO As AlaVO, ByVal AliasBanco As String) As AlaVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT * FROM TBAPARTAMENTOALA ")
                .Append("WHERE ALAID = " & ObjAlaVO.AlaId & " ")
            End With
            Return PreencheListaCodigo(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaCodigo(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As AlaVO
        Try
            If ResultadoConsulta.HasRows Then
                ResultadoConsulta.Read()
                ObjAlaVO = New AlaVO
                With ObjAlaVO
                    .AlaId = ResultadoConsulta.Item("alaId")
                    .AlaNome = ResultadoConsulta.Item("AlaNome")
                    .AlaDescricao = ResultadoConsulta.Item("AlaDescricao")
                    .CamId = ResultadoConsulta.Item("CamId")
                End With
            End If
            ResultadoConsulta.Close()
            Return ObjAlaVO
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function AtualizaCamareiraAla(ByVal ObjAlaVO As AlaVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS (SELECT ALAID FROM TBAPARTAMENTOALA WHERE ALAID =  " & ObjAlaVO.AlaId & " )")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTOALA SET CAMID=" & ObjAlaVO.CamId & " ")
                .Append("WHERE ALAID = " & ObjAlaVO.AlaId & " ")
                .Append("SELECT 2 GOTO SAIDA ")
                .Append("END ")
                .Append("IF @@ERROR > 0 GOTO ERRO ")
                .Append("SELECT 3 GOTO SAIDA ")
                .Append("ERRO: SELECT 0 GOTO SAIDA ")
                .Append("ERRO2: SELECT -1 GOTO SAIDA ")
                .Append("SAIDA: ")
                Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
