Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

Public Class OcupadoDAO
    Dim ObjOcupadoVO As OcupadoVO
    Dim ObjOcupadoDAO As OcupadoDAO
    Public Function Consultar(ByVal ObjOcupadoVO As OcupadoVO, ByVal Federacao As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                'CRIANDO UMA VARIAVEL TABLE PARA INSERÇÃO DOS DADOS 
                .Append("DECLARE @TEMP AS TABLE( ")
                .Append("APAID SMALLINT, ")
                .Append("APADESC VARCHAR(10), ")
                .Append("HOSDATAFIMSOL DATETIME, ")
                .Append("HOSDATAFIMREAL DATETIME, ")
                .Append("INTID NUMERIC(18), ")
                .Append("FEDERACAO CHAR(1), ")
                .Append("APACCUSTO VARCHAR(10), ")
                .Append("APAAREA SMALLINT ")
                .Append(") ")
                'INSERINDO OS DADOS NA TABELA TEMP
                .Append("INSERT @TEMP ")
                .Append("SELECT DISTINCT A.APAID, A.APADESC,H.HOSDATAFIMSOL,H.HOSDATAFIMREAL,H.INTID,A.APAFEDERACAO,A.APACUSTO,A.APAAREA  ")
                .Append("FROM TBAPARTAMENTO A INNER JOIN TBHOSPEDAGEM H ON H.APAID = A.APAID ")
                .Append("WHERE A.APASTATUS = 'O' ")
                .Append("AND A.APADESC LIKE '%%' ")
                .Append("AND H.HOSDATAFIMREAL IS NULL ")
                .Append("AND H.HOSDATAINIREAL IS NOT NULL ")
                'AGRUPANDO O RESULTADO PARA APLICAÇÃO 
                .Append("SELECT MAX(P.APAID)AS APAID,MAX(P.APADESC) AS APADESC,MAX(P.HOSDATAFIMSOL) AS HOSDATAFIMSOL,MAX(P.HOSDATAFIMREAL) AS HOSDATAFIMREAL,MAX(P.FEDERACAO)AS APAFEDERACAO, ")
                .Append("MAX(C.CSEORIGEM) AS CSEORIGEM,MAX(C.CSEMANUAL)AS CSEMANUAL,MAX(C.CSEDESCRICAO) AS CSEDESCRICAO,MAX(APACCUSTO) AS APACCUSTO,MAX(APAAREA) AS APAAREA, ")
                .Append("CASE WHEN MAX(C.CSEVALOR) IS NOT NULL THEN 'S' ELSE 'N' END AS EMPRESTIMOS ")
                .Append("FROM @TEMP P ")
                .Append("LEFT JOIN TBCONSUMOSERVICO C ON C.INTID = P.INTID ")
                If ObjOcupadoVO.ApaDesc <> " " Then
                    .Append("WHERE P.APADESC LIKE '%" & ObjOcupadoVO.ApaDesc & "%' ")
                End If
                If Federacao <> "" Then
                    .Append("AND P.FEDERACAO = '" & Federacao & "' ")
                End If
                .Append("GROUP BY APAID ")
                If AliasBanco = "TurismoSocialPiri" Then
                    .Append("order by APADESC ")
                Else
                    .Append("order by APAID ")
                End If
                '.Append("ORDER BY APADESC ")
                Return Me.PreencheLista(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjOcupadoVO = New OcupadoVO
                With ObjOcupadoVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .HosDataFimSol = ResultadoConsulta.Item("HosDataFimSol")
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosDataFimReal")) Then
                        .HosDataFimReal = " "
                    Else
                        .HosDataFimReal = ResultadoConsulta.Item("HosDataFimReal")
                    End If

                    .Emprestimos = ResultadoConsulta.Item("Emprestimos")
                    If Convert.IsDBNull(ResultadoConsulta.Item("CSeManual")) Then
                        .CSeManual = " "
                    Else
                        .CSeManual = ResultadoConsulta.Item("CSeManual")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CSeOrigem")) Then
                        .CSeOrigem = " "
                    Else
                        .CSeOrigem = ResultadoConsulta.Item("CSeOrigem")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CSeDescricao")) Then
                        .CSeDescricao = " "
                    Else
                        If ResultadoConsulta.Item("CSeDescricao") = "" Then
                            .CSeDescricao = " "
                        Else
                            .CSeDescricao = ResultadoConsulta.Item("CSeDescricao")
                        End If
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaCCusto")) Then
                        .ApaCCusto = " "
                    Else
                        .ApaCCusto = ResultadoConsulta.Item("ApaCCusto")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaArea")) Then
                        .ApaArea = " "
                    Else
                        .ApaArea = ResultadoConsulta.Item("ApaArea")
                    End If

                    '.TipoAcomodacao = ResultadoConsulta.Item("TipoAcomodacao")
                    '.ApaStatus = ResultadoConsulta.Item("")
                    '.ApaHTML = ResultadoConsulta.Item("")
                End With
                Lista.Add(ObjOcupadoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function SelecionaAptosComCheckout(ByVal ObjOcupadoVO As OcupadoVO, ByVal DataFinalSolicitacao As String, ByVal AliasBanco As String) As OcupadoVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT DISTINCT H.APAID FROM TBHOSPEDAGEM H ")
                .Append("INNER JOIN TBINTEGRANTE I ON I.INTID = H.INTID ")
                .Append("WHERE H.HOSDATAFIMSOL <= '" & DataFinalSolicitacao & "' ")
                .Append("AND H.APAID = " & ObjOcupadoVO.ApaId & " ")
                .Append("AND I.INTSTATUS IN ('E','P') ")
                .Append("AND H.HOSDATAINIREAL IS NOT NULL ")
                .Append("AND H.HOSDATAFIMREAL IS NULL ")
                .Append("GROUP BY H.APAID ")
            End With
            Return PreecheAptoCheckOut(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreecheAptoCheckOut(ByVal ResultadaConsulta As System.Data.SqlClient.SqlDataReader) As OcupadoVO
        If ResultadaConsulta.HasRows Then
            ResultadaConsulta.Read()
            ObjOcupadoVO = New OcupadoVO
            ObjOcupadoVO.ApaId = ResultadaConsulta.Item("ApaId")
        Else
            ObjOcupadoVO = New OcupadoVO
            ObjOcupadoVO.ApaId = 0
        End If
        ResultadaConsulta.Close()
        Return ObjOcupadoVO
    End Function
End Class
