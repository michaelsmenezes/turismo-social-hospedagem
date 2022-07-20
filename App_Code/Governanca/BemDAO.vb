Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class BemDAO
    Dim objBemVo As BemVO
    Dim ObjBemDAO As BemDAO
    Dim Db As String = "BDPROD"
    Public Sub New()

    End Sub
    Public Function PesquisaBem(ByVal Area As String, ByVal CentroCusto As String) As IList
        Try
            Dim Conn = New Banco.Conexao(Db)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("Select NUREG,SUBSTR(char(NUREG),1,6) || ' - ' || SUBSTR(NMBEM,1,48) as NMBEM,CDCCUSTO,CDAREA,STBEM from bem ")
            VarSql.Append("WHERE CDAREA =" & Area & " ")
            VarSql.Append("AND CDCCUSTO = '" & CentroCusto & "' ")
            VarSql.Append("AND CDSUBCLAS <> 1001 ") '1001 são equipamentos de informática' 
            VarSql.Append("ORDER BY NMBEM")
            Return PreencheBem(Conn.executaOdbc(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        Finally
            Dim Conn = Nothing
        End Try
    End Function
    Private Function PreencheBem(ByVal Dt As System.Data.DataTable) As IList
        Dim Lista As New ArrayList
        If Dt.Rows.Count <> 0 Then
            For Each DtRow As System.Data.DataRow In Dt.Rows
                objBemVo = New BemVO
                If Dt.Columns.Contains("NUREG") Then
                    If Not Convert.IsDBNull(DtRow.Item("NUREG")) Then
                        objBemVo.Patrimonio = DtRow.Item("NUREG")
                    Else
                        objBemVo.Patrimonio = ""
                    End If
                End If

                If Dt.Columns.Contains("CDCCUSTO") Then
                    If Not Convert.IsDBNull(DtRow.Item("CDCCUSTO")) Then
                        objBemVo.CentroCusto = DtRow.Item("CDCCUSTO")
                    Else
                        objBemVo.CentroCusto = ""
                    End If
                End If

                If Dt.Columns.Contains("CDAREA") Then
                    If Not Convert.IsDBNull(DtRow.Item("CDAREA")) Then
                        objBemVo.Area = DtRow.Item("CDAREA")
                    Else
                        objBemVo.Area = ""
                    End If
                End If

                If Dt.Columns.Contains("NMBEM") Then
                    If Not Convert.IsDBNull(DtRow.Item("NMBEM")) Then
                        objBemVo.NomeBem = DtRow.Item("NMBEM")
                    Else
                        objBemVo.NomeBem = ""
                    End If
                End If

                If Dt.Columns.Contains("STBEM") Then
                    If Not Convert.IsDBNull(DtRow.Item("STBEM")) Then
                        objBemVo.SituacaoBem = DtRow.Item("STBEM")
                    Else
                        objBemVo.SituacaoBem = ""
                    End If
                End If
                Lista.Add(objBemVo)
            Next
        End If
        Return Lista
    End Function
End Class
