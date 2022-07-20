Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Imports Banco
Public Class EmprestimosRecepcaoDAO
    Dim ObjEmprestimosRecepcaoVO As EmprestimosRecepcaoVO
    Dim Conn As Conexao
    Dim VarSql As Text.StringBuilder
    Public Sub New(banco As String)
        Conn = New Conexao(banco)
    End Sub

    Public Function Inserir(ObjEmprestimosRecepcaoVO As EmprestimosRecepcaoVO) As Long
        ObjEmprestimosRecepcaoVO.proValor = ObjEmprestimosRecepcaoVO.proValor.Replace(",", ".")
        VarSql = New Text.StringBuilder("Set nocount on ")
        With VarSql
            .AppendLine("If not exists(select 1 from tbProdutosEmprestimo where proId = " & ObjEmprestimosRecepcaoVO.proId & ") ")
            .AppendLine("  Begin ")
            .AppendLine("    Insert TbProdutosEmprestimo(proDescricao,proValor,proUsuario,proUsuarioData) values ")
            .AppendLine("         ('" & ObjEmprestimosRecepcaoVO.proDescricao & "','" & ObjEmprestimosRecepcaoVO.proValor.Replace(",", ".") & "','" & ObjEmprestimosRecepcaoVO.proUsuario & "',GetDate()) ")
            .AppendLine("          select 1 goto saida ")
            .AppendLine("  End ")
            .AppendLine("else ")
            .AppendLine("  Begin ")
            .AppendLine("     if exists(Select 1 from tbEmprestimo where proId = " & ObjEmprestimosRecepcaoVO.proId & ")  ")
            .AppendLine("          Begin ")
            .AppendLine("             Update TbProdutosEmprestimo set proValor = '" & ObjEmprestimosRecepcaoVO.proValor & "' where proId = " & ObjEmprestimosRecepcaoVO.proId & " ")
            .AppendLine("             Select 2 goto saida ")
            .AppendLine("          End ")
            .AppendLine("     else ")
            .AppendLine("          Begin  ")
            .AppendLine("             Update TbProdutosEmprestimo set proDescricao = '" & ObjEmprestimosRecepcaoVO.proDescricao & "' ,proValor = '" & ObjEmprestimosRecepcaoVO.proValor & "' where proId = " & ObjEmprestimosRecepcaoVO.proId & " ")
            .AppendLine("             Select 2 goto saida ")
            .AppendLine("          End ")
            .AppendLine("  End ")
            .AppendLine("saida:")
            Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        End With
    End Function

    Public Function ConsultarProdutos(proDescricao As String) As IList
        VarSql = New Text.StringBuilder("Set nocount on ")
        With VarSql
            .AppendLine("Select * from tbProdutosEmprestimo where proDescricao like '%" & proDescricao.Replace(" ", "%") & "%' ")
        End With
        Return PreencheLista(Conn.consulta(VarSql.ToString))
    End Function

    Private Function PreencheLista(ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjEmprestimosRecepcaoVO = New EmprestimosRecepcaoVO
                With ObjEmprestimosRecepcaoVO
                    .proDescricao = ResultadoConsulta.Item("proDescricao")
                    .proValor = ResultadoConsulta.Item("proValor")
                    .proUsuario = ResultadoConsulta.Item("proUsuario")
                    .proUsuarioData = ResultadoConsulta.Item("proUsuarioData")
                End With
                Lista.Add(ObjEmprestimosRecepcaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
