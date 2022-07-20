Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class RefeicaoManualDAO
    Dim ObjRefeicaoManualVO As RefeicaoManualVO
    Dim ObjRefeicaoManualDAO As RefeicaoManualDAO
    Public Function ConsultaRefeicoes(ByVal ObjRefeicaoManualVO As RefeicaoManualVO, ByVal DataInicial As String, ByVal DataFinal As String, ByVal AliasBanco As String, ByVal TipoInsercao As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("SELECT REFID,INTID,REFDATA, ")
                .AppendLine("CASE REFTIPO ")
                .AppendLine("WHEN 'D' THEN 'DESJEJUM' ")
                .AppendLine("WHEN 'A' THEN 'ALMOÇO' ")
                .AppendLine("WHEN 'J' THEN 'JANTAR' ")
                .AppendLine("END REFTIPO, ")
                .AppendLine("REFQTDE, ")
                .AppendLine("CASE REFCORTESIA ")
                .AppendLine("WHEN 'S' THEN 'SIM' ")
                .AppendLine("WHEN 'N' THEN 'NÃO' ")
                .AppendLine("END AS REFCORTESIA ")
                .AppendLine("FROM TBREFEICAO ")
                .AppendLine("WHERE REFDATA BETWEEN '" & DataInicial & "'  AND '" & DataFinal & "' ")
                If ObjRefeicaoManualVO.RefTipo <> "T" Then
                    .AppendLine("AND REFTIPO LIKE '%" & ObjRefeicaoManualVO.RefTipo & "%' ")
                End If
                'Se foi inserção manual ou não: -2 = Hóspedes'
                If (TipoInsercao = "M") Then
                    .AppendLine("AND INTID = -2 ")
                End If
                .AppendLine("ORDER BY RefData DESC ")
            End With
            Return PreecheListaRefeicao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreecheListaRefeicao(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRefeicaoManualVO = New RefeicaoManualVO
                With ObjRefeicaoManualVO
                    .IntId = ResultadoConsulta.Item("IntId")
                    .RefCortesia = ResultadoConsulta.Item("RefCortesia")
                    .RefData = ResultadoConsulta.Item("RefData")
                    .RefId = ResultadoConsulta.Item("RefId")
                    .RefQtde = ResultadoConsulta.Item("RefQtde")
                    .RefTipo = ResultadoConsulta.Item("RefTipo")
                End With
                Lista.Add(ObjRefeicaoManualVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ApagaRefeicao(ByVal ObjRefeicaoManualVO As RefeicaoManualVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS(SELECT 1 FROM TBREFEICAO WHERE REFID = " & ObjRefeicaoManualVO.RefId & ")")
                .Append("BEGIN ")
                .Append("DELETE FROM TBREFEICAO ")
                .Append("WHERE REFID = " & ObjRefeicaoManualVO.RefId & " AND INTID = -2 ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("IF @@ERROR > 0 ")
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
    Public Function InserindoRefeicoes(ByVal ObjRefeicaoManualVO As RefeicaoManualVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("BEGIN TRY ")
                .AppendLine("IF NOT EXISTS (SELECT 1 FROM TBREFEICAO WHERE REFID = 0) ")
                .AppendLine("BEGIN ")
                .AppendLine("  INSERT INTO TBREFEICAO(INTID,REFDATA,REFTIPO,REFQTDE,REFCORTESIA,REFMOTIVOINSMANUAL,REFUSUARIO,REFUSUARIODATA) VALUES ")
                .AppendLine("  (" & ObjRefeicaoManualVO.IntId & ",'" & Format(CDate(ObjRefeicaoManualVO.RefData), "yyyy-MM-dd ") & TimeOfDay & "' ,'" & ObjRefeicaoManualVO.RefTipo & "', ")
                .AppendLine("  " & ObjRefeicaoManualVO.RefQtde & ",'" & ObjRefeicaoManualVO.RefCortesia & "','" & ObjRefeicaoManualVO.RefMotivoInsManual & "', ")
                .AppendLine("  '" & ObjRefeicaoManualVO.RefUsuario & "','" & Format(CDate(ObjRefeicaoManualVO.RefUsuarioData), "yyyy-MM-dd") & "') ")
                .AppendLine("  SELECT 1 GOTO SAIDA ")
                .AppendLine("END ")
                .AppendLine("END TRY ")
                .AppendLine("BEGIN CATCH ")
                .AppendLine("  SELECT 0 GOTO SAIDA ")
                .AppendLine("END CATCH ")
                .AppendLine("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

End Class
