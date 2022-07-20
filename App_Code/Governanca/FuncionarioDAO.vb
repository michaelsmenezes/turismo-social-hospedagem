Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class FuncionarioDAO
    Dim DBFuncLocal As String = "DbRestauranteServidores" 'Esse procedimento vai buscar o Centro de Custo do funcionario na base local - SQL-CTL'
    Dim ObjFuncionarioVO As FuncionarioVO
    Dim ObjFuncionarioDAO As FuncionarioDAO
    Public Function Consultar(ByVal ObjFuncionarioVO As FuncionarioVO) As IList
        Try
            Dim Conn = New Banco.Conexao(DBFuncLocal)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.Append("select * from tbFuncionarios F ")
            VarSql.Append("Where F.funCodLotacao = '102000000'")
            VarSql.Append("order by F.funNome")
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
                ObjFuncionarioVO = New FuncionarioVO
                ObjFuncionarioVO.CentroCusto = ResultadoConsulta.Item("funCentroCusto")
                ObjFuncionarioVO.Matricula = ResultadoConsulta.Item("funMatricula")
                ObjFuncionarioVO.Nome = ResultadoConsulta.Item("funNome")
                Lista.Add(ObjFuncionarioVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return (Lista)
    End Function
    Public Function ConsultarFuncMatricula(ByVal ObjFuncionarioVO As FuncionarioVO, ByVal UnidadeOperacional As String) As FuncionarioVO
        Try
            Dim Conn = New Banco.Conexao(DBFuncLocal)
            Dim VarSql As Text.StringBuilder
            'VarSql = New Text.StringBuilder("SELECT FUNNOME,FUNMATRICULA,FUNCENTROCUSTO FROM TBFUNCIONARIOS WHERE FUNMATRICULA = '" & ObjFuncionarioVO.Matricula & "' ")
            'Se  não encontrar o funcionario aqui em Caldas procura no FPW'
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            If UnidadeOperacional = "Caldas Novas" Then
                VarSql.Append("if exists (SELECT FUNNOME,FUNMATRICULA,FUNCENTROCUSTO FROM TBFUNCIONARIOS WHERE FUNMATRICULA = '" & ObjFuncionarioVO.Matricula & "') ")
                VarSql.Append("BEGIN ")
                VarSql.Append("SELECT FUNNOME,FUNMATRICULA,FUNCENTROCUSTO FROM TBFUNCIONARIOS WHERE FUNMATRICULA = '" & ObjFuncionarioVO.Matricula & "' ")
                VarSql.Append("End ")
            Else
                VarSql.Append("Select top 1 FUCentrCus as FunCentroCusto,FUMatFunc as FunMatriCula,FuNomFunc as FunNome from [SQL-ADM].[fpw].[dbo].[Funciona] where FUMatFunc ='" & ObjFuncionarioVO.Matricula & "' ")
            End If
            Return PreencheObjeto(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheObjeto(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As FuncionarioVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjFuncionarioVO = New FuncionarioVO
            ObjFuncionarioVO.CentroCusto = ResultadoConsulta.Item("FUNCENTROCUSTO")
            ObjFuncionarioVO.Matricula = ResultadoConsulta.Item("FUNMATRICULA")
            ObjFuncionarioVO.Nome = ResultadoConsulta.Item("FUNNOME")
        Else
            ObjFuncionarioVO = New FuncionarioVO
            ObjFuncionarioVO.CentroCusto = ""
            ObjFuncionarioVO.Matricula = ""
            ObjFuncionarioVO.Nome = ""
        End If
        ResultadoConsulta.Close()
        Return ObjFuncionarioVO
    End Function
End Class
