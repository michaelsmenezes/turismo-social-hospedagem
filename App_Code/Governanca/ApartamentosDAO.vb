Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

Public Class ApartamentosDAO
    Dim Conn As Banco.Conexao
    Dim VarSql As Text.StringBuilder
    Dim ObjApartamentosVO As ApartamentosVO
    Public Sub New()

    End Sub

    Public Function ConsultaApartamento(ByVal ObjApartamentosVO As ApartamentosVO, ByVal Bloco As Long, ByVal AptoIni As Long, ByVal AptoFinal As Long, ByVal Status As String, ByVal AliasBanco As String) As IList
        Try
            Conn = New Banco.Conexao(AliasBanco)
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT A.APAID,A.APADESC,A.ACMID,A.APAFEDERACAO,A.APASTATUS,A.ACMID, ")
                .Append("C.BLOID, C.ACMCC, ACMCS, C.ACMBICAMA, C.ACMSOFACAMA ")
                '.Append("D.AGOCCASAL,D.AGOCSOLTEIRO,D.AGOBERCO,D.AGOTRAVESSEIRO,D.AGOJOGOTOALHAS,D.AGOROLOPAPEL,D.AGOSACOLIXO,D.AGOSABONETE,D.AGOTAPETE,D.AGOORIGEM ")
                .Append("FROM TBAPARTAMENTO A ")
                .Append("INNER JOIN TBACOMODACAO C ON A.ACMID = C.ACMID ")
                Select Case ObjApartamentosVO.ApaFederacao
                    Case "S" : .Append("WHERE A.APAFEDERACAO = 'S' ")
                    Case "N" : .Append("WHERE A.APAFEDERACAO = 'N' ")
                    Case Else
                End Select
                'SE FOR TODOS (0) NÃO FILTRA POR BLOCO
                If Bloco <> 0 Then
                    .Append("AND C.BLOID = " & Bloco)
                End If
                'VALOR SERÁ PEGO PELO SESSÃO DA TELA GOVERNANCA.ASPX
                If ObjApartamentosVO.ApaId <> 0 Then
                    .Append("AND A.APAID = " & ObjApartamentosVO.ApaId & " ")
                End If
                If AptoIni > 0 And AptoFinal > 0 Then
                    .Append("AND A.APAID BETWEEN " & AptoIni & " AND " & AptoFinal & " ")
                End If
                If Status <> "" Then
                    .Append("AND A.ApaStatus = '" & Status & "' ")
                End If
                '.Append("where A.APASTATUS = '" & Status & "' ")
                .Append("ORDER BY A.APAID,A.APADESC ASC ")
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
                ObjApartamentosVO = New ApartamentosVO
                With ObjApartamentosVO
                    .AcmBicama = ResultadoConsulta.Item("AcmBicama")
                    .AcmCC = ResultadoConsulta.Item("AcmCC")
                    .AcmCS = ResultadoConsulta.Item("AcmCS")
                    .AcmId = ResultadoConsulta.Item("AcmId")
                    .AcmSofacama = ResultadoConsulta.Item("AcmSofacama")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .ApaFederacao = ResultadoConsulta.Item("ApaFederacao")
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaStatus = ResultadoConsulta.Item("ApaStatus")
                    .BloId = ResultadoConsulta.Item("BloId")
                End With
                Lista.Add(ObjApartamentosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ListaGeralApartamento(ByVal ObjApartamentosVO As ApartamentosVO, ByVal AliasBanco As String) As IList
        Try
            Conn = New Banco.Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT APAID,APADESC FROM TBAPARTAMENTO ORDER BY APADESC ASC ")
            End With
            Return PreencheListaAptoGeral(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaAptoGeral(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjApartamentosVO = New ApartamentosVO
                With ObjApartamentosVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                End With
                Lista.Add(ObjApartamentosVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function VerificaAptoSaiuManutencao(ByVal ObjApartamentosVO As ApartamentosVO, ByVal AliasBanco As String) As ApartamentosVO
        Try
            Conn = New Banco.Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT DISTINCT A.APAID FROM ")
                .Append("TBAPARTAMENTO A WITH (NOLOCK) INNER JOIN TBMANUTENCAO M WITH (NOLOCK) ")
                .Append("ON M.APAID = A.APAID ")
                .Append("WHERE NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV G WITH (NOLOCK) WHERE G.APAID = A.APAID ")
                .Append("AND G.AGODATA > M.MANDATACONCLUSAO) ")
                .Append("AND A.APASTATUS IN ('A','O') ")
                .Append("AND A.APAID = " & ObjApartamentosVO.ApaId & " ")
            End With
            Return PreencheAptoSaiuManutencao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheAptoSaiuManutencao(ByVal resultadoconsulta As System.Data.SqlClient.SqlDataReader) As ApartamentosVO
        If resultadoconsulta.HasRows Then
            resultadoconsulta.Read()
            ObjApartamentosVO = New ApartamentosVO
            'ObjApartamentosVO.ApaId = resultadoconsulta.Item("ApaId")
            ObjApartamentosVO.ApaId = 1
        Else
            ObjApartamentosVO = New ApartamentosVO
            ObjApartamentosVO.ApaId = 0
        End If
        resultadoconsulta.Close()
        Return ObjApartamentosVO
    End Function
    Public Function ApartamentoAlas(ByVal objApartamentosVO As ApartamentosVO, ByVal AptoContidoAla As String, ByVal AliasBanco As String) As IList
        Try
            Conn = New Banco.Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT APAID,APADESC FROM TBAPARTAMENTO  ")
                If AptoContidoAla = "S" Then
                    .Append("WHERE ALAID = " & objApartamentosVO.AlaId & " ")
                Else
                    .Append("WHERE (ALAID <> " & objApartamentosVO.AlaId & " ")
                    .Append("OR ALAID IS NULL) ")
                End If
                .Append("AND APADESC LIKE '%" & objApartamentosVO.ApaDesc & "%'")
                .Append("ORDER BY APADESC ASC ")
            End With
            Return PreencheListaAptoAla(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaAptoAla(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        Try
            If ResultadoConsulta.HasRows Then
                While ResultadoConsulta.Read
                    ObjApartamentosVO = New ApartamentosVO
                    With ObjApartamentosVO
                        .ApaId = ResultadoConsulta.Item("ApaId")
                        .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    End With
                    Lista.Add(ObjApartamentosVO)
                End While
            End If
            ResultadoConsulta.Close()
            Return Lista
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function AtribuiAla(ByVal ObjApartamentosVO As ApartamentosVO, ByVal AliasBanco As String) As Long
        Try
            Conn = New Banco.Conexao(AliasBanco)
            VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS(SELECT APAID FROM TBAPARTAMENTO WHERE APAID = " & ObjApartamentosVO.ApaId & " )")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO SET ALAID = " & ObjApartamentosVO.AlaId & " ")
                .Append("WHERE APAID = " & ObjApartamentosVO.ApaId & " ")
                .Append("SELECT 2 GOTO SAIDA ")
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
                Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function


End Class
