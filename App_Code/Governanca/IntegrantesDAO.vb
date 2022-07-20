Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

Public Class IntegrantesDAO
    Dim ObjIntegrantesVO As IntegrantesVO
    Dim ObjIntegrantesDAO As IntegrantesDAO
    Public Function ConsultaIntegrante(ByVal ObjIntegrantesVO As IntegrantesVO, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT intstatus, I.INTID,H.APAID, UPPER(I.INTNOME)AS NOMEINTEGRANTE,I.INTDATAFIM, ")
                .Append("isnull((SELECT SUM(II.CSEVALOR*II.CSEQUANTIDADE) FROM TBCONSUMOSERVICO II WHERE II.INTID = I.INTID),0) VALOR ")
                .Append("FROM TBHOSPEDAGEM H ")
                .Append("INNER JOIN TBINTEGRANTE I ")
                .Append("ON I.INTID = H.INTID ")
                .Append("INNER JOIN TBAPARTAMENTO P ON H.APAID = P.APAID ")
                .Append("AND H.HOSDATAFIMREAL IS NULL ")
                .Append("AND H.HOSDATAINIREAL IS NOT NULL ")
                .Append("AND H.APAID = " & ObjIntegrantesVO.ApaId & " ")
                .Append("and intstatus = 'E' ")
                .Append("ORDER BY H.APAID, I.INTNOME ")
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
                ObjIntegrantesVO = New IntegrantesVO
                With ObjIntegrantesVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .Nome = ResultadoConsulta.Item("NomeIntegrante")
                    .CheckOut = ResultadoConsulta.Item("IntDataFim")
                    '.Emprestimos = ResultadoConsulta.Item("Emprestimos")
                    .IntId = ResultadoConsulta.Item("IntId")
                    If Convert.IsDBNull(ResultadoConsulta.Item("Valor")) Then
                        .ValorEmprestimos = FormatCurrency(0, 2)
                    Else
                        .ValorEmprestimos = FormatCurrency(ResultadoConsulta.Item("Valor"), 2)
                    End If
                End With
                Lista.Add(ObjIntegrantesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
