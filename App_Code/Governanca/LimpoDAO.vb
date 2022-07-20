Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class LimpoDAO
    Dim ObjLimpoDAO As LimpoDAO
    Dim ObjLimpoVO As LimpoVO
    Public Function Consultar(ByVal ObjLimpoVO As LimpoVO, ByVal Federacao As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                .Append("IF '" & Federacao & "' <> '' ")
                .Append("BEGIN ")
                .Append("SELECT a.ApaId,a.ApaDesc,ApaUsuario, ApaUsuarioData,ApaCusto,ApaArea ")
                .Append("FROM tbApartamento a ")
                .Append("WHERE a.ApaStatus = 'L' ")
                .Append("AND a.ApaDesc LIKE '%" & ObjLimpoVO.ApaId & "%'  ")
                .Append("AND a.ApaFederacao = '" & Federacao & "' ")
                .Append("End ")
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("SELECT a.ApaId,a.ApaDesc,ApaUsuario, ApaUsuarioData,ApaCusto,ApaArea ")
                .Append("FROM tbApartamento a ")
                .Append("WHERE ApaStatus = 'L' ")
                .Append("AND a.ApaDesc LIKE '%" & ObjLimpoVO.ApaId & "%' ")
                If AliasBanco = "TurismoSocialPiri" Then
                    .Append("order by a.APADESC ")
                Else
                    .Append("order by a.APAID ")
                End If
                '.Append("ORDER BY A.APADESC ")
                .Append("End ")
                Return Me.PreencheLista(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        'Faz a leitura nas linhas da consulta e preenche o objeto'
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjLimpoVO = New LimpoVO
                With ObjLimpoVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    '.TipoAcomodacao = ResultadoConsulta.Item("TipoAcomodacao")
                    '.ApaStatus = ResultadoConsulta.Item("")
                    .ApaHTML = ResultadoConsulta.Item("ApaDesc")
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaUsuarioData")) Then
                        .DataLimpoLog = " "
                    Else
                        .DataLimpoLog = ResultadoConsulta.Item("ApaUsuarioData")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaUsuario")) Or (ResultadoConsulta.Item("ApaUsuario") = "") Then
                        .UsuarioLimpoLog = " "
                    Else
                        .UsuarioLimpoLog = ResultadoConsulta.Item("ApaUsuario")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaCusto")) Then
                        .ApaCCusto = " "
                    Else
                        .ApaCCusto = ResultadoConsulta.Item("ApaCusto")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaArea")) Then
                        .ApaArea = " "
                    Else
                        .ApaArea = ResultadoConsulta.Item("ApaArea")
                    End If
                End With
                Lista.Add(ObjLimpoVO)
            End While
        Else
            ''Se não tiver resultado posivito. Ou seja, nenhuma linha'
            'ObjLimpoVO = New LimpoVO
            'With ObjLimpoVO
            '    .ApaId = " "
            '    .ApaDesc = " "
            '    '.ApaStatus = " "
            '    '.ApaHTML = " "
            'End With
            'Lista.Add(ObjLimpoVO)
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function QuemLimpouApto(ByVal ApaId As Integer, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                .Append("SET NOCOUNT ON ")
                .Append("SELECT TOP 3 APAID,APAUSUARIO,APAUSUARIODATA ")
                .Append("FROM TbApartamentoLog  ")
                .Append("WHERE ApaId = " & ApaId & "  ")
                .Append("AND ApaStatus = 'L' ")
                .Append("ORDER BY ApaUsuarioDataLog DESC ")
                Return Me.PreencheListaQuemLimpou(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaQuemLimpou(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjLimpoVO = New LimpoVO
                With ObjLimpoVO
                    .ApaId = ResultadoConsulta.Item("APAID")
                    .UsuarioLimpoLog = ResultadoConsulta.Item("APAUSUARIO")
                    .DataLimpoLog = ResultadoConsulta.Item("APAUSUARIODATA")
                End With
                Lista.Add(ObjLimpoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
