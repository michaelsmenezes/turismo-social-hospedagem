Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic


Public Class ManutencaoDAO
    Dim ObjManutencaoVO As ManutencaoVO
    Dim ObjManutencaoDAO As ManutencaoDAO
    Public Function Consultar(ByVal ObjManutencaoVO As ManutencaoVO, ByVal Federacao As String, ByVal UnidadeOperacional As String, ByVal AliasBanco As String) As IList
        Try
            'Verificar o alas se passa hdmanutencao ou turismosocialpiri
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder
            With VarSql
                .AppendLine("SET XACT_ABORT ON ") 'Permite fazer transações distribuidas via linked server
                .AppendLine("SELECT DISTINCT ")
                .AppendLine("A.ApaId as ApaId, ")
                .AppendLine("MAX(A.ApaDesc) as ApaDesc,MAX(a.ApaCusto) as ApaCCusto,MAX(a.ApaArea) as ApaArea, ")
                .AppendLine("(select TS.SolDescricao from [SQL-ADM].[DbHDManutencao].[Dbo].TbSolicitacao TS where TS.solDataHoraSolicitacao = MIN(S.solDataHoraSolicitacao)AND TS.solArea = A.ApaArea) AS ManDescricaoManut, ")
                .AppendLine("(select TS.solDataHoraSolicitacao from [SQL-ADM].[DbHDManutencao].[Dbo].TbSolicitacao TS where TS.solDataHoraSolicitacao = MIN(S.solDataHoraSolicitacao)AND TS.solArea = A.ApaArea) as ManDataAbertura, ")
                .AppendLine("(select TS.SolPrevisaoAtendimento from [SQL-ADM].[DbHDManutencao].[Dbo].TbSolicitacao TS where TS.solDataHoraSolicitacao = MIN(S.solDataHoraSolicitacao)AND TS.solArea = A.ApaArea) as SolDataFim ")

                Dim Servidor As String = ""

                'If UnidadeOperacional = "dbTurismoSocial" Then
                '    Servidor = "HOM_TURISMO_SOCIAL_CALD"
                'ElseIf UnidadeOperacional = "dbTurismoSocialPiri" Then
                '    Servidor = "HOM_TURISMO_SOCIAL_PIRI"
                'ElseIf UnidadeOperacional = "TurismoSocialCaldasxxx" Then
                '    Servidor = "SERVER-CTL-BKP"
                'End If

                If UnidadeOperacional = "dbTurismoSocial" Then
                    Servidor = "SQL-CTL"
                ElseIf UnidadeOperacional = "dbTurismoSocialPiri" Then
                    Servidor = "SQL-PSP"
                ElseIf UnidadeOperacional = "TurismoSocialCaldasxxx" Then
                    Servidor = "SERVER-CTL-BKP"
                End If

                '.AppendLine("from [" & Servidor & "].[" & UnidadeOperacional & "].[Dbo].TbApartamento A ")
                .AppendLine("from TbApartamento A ")
                .AppendLine("INNER JOIN [SQL-ADM].[DbHDManutencao].[Dbo].TbSolicitacao S ON S.solCentroCusto = A.ApaCusto ")
                .AppendLine("Where S.solArea = A.ApaArea ")
                .AppendLine("and S.solBloqueioApartamento = 'S' ")
                .AppendLine("and A.ApaFederacao like '%" & Federacao & "%' ")
                .AppendLine("and A.Apastatus = 'M' ")
                .AppendLine("GROUP BY A.APAID,A.ApaArea ")
                If AliasBanco = "TurismoSocialPiri" Then
                    .AppendLine("order by MAX(a.APADESC) ")
                Else
                    .AppendLine("order by a.APAID ")
                End If
            End With
            Return Me.PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjManutencaoVO = New ManutencaoVO
                With ObjManutencaoVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .ManDescricaoManut = ResultadoConsulta.Item("ManDescricaoManut")
                    .ManDataAbertura = ResultadoConsulta.Item("ManDataAbertura")
                    .SolDataFim = ResultadoConsulta.Item("SolDataFim")
                    .ApaCCusto = ResultadoConsulta.Item("ApaCCusto")
                    .ApaArea = ResultadoConsulta.Item("ApaArea")
                End With
                Lista.Add(ObjManutencaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function InsereManutencao(ByVal ObjManutencaoVO As ManutencaoVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("EXEC SpAtualizaManutencao ")
            'VarSql.Append(" " & ObjManutencaoVO.ApaId & ",'" & ObjManutencaoVO.ManDescricaoManut & "','" & ObjManutencaoVO.Usuario & "' ")
            VarSql.Append("0,'" & ObjManutencaoVO.ManDescricaoManut & "','" & ObjManutencaoVO.Usuario & "' ")
            Dim Resultado = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaHistoricoManutencao(ByVal ObjManutencaoVO As ManutencaoVO, ByVal Data1 As String, ByVal Data2 As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SET NOCOUNT ON ")
                .Append("SELECT A.APAID, A.APADESC, CONVERT(CHAR(10), S.SOLDATAHORASOLICITACAO, 103) DIAABERTURA, ")
                .Append("CAST(DATEPART(HH, S.SOLDATAHORASOLICITACAO) AS VARCHAR(2)) HORAABERTURA, ")
                .Append("CONVERT(CHAR(10), S.SOLDATAATENDIMENTO, 103) DIACONCLUSAO, ")
                .Append("CAST(DATEPART(HH, S.SOLDATAATENDIMENTO) AS VARCHAR(2)) HORACONCLUSAO, ")
                .Append("DATEDIFF(DD, S.SOLDATAHORASOLICITACAO, ISNULL(S.SOLDATAATENDIMENTO, GETDATE())) AS DIA, ")
                .Append("(DATEDIFF(HH, S.SOLDATAHORASOLICITACAO, ISNULL(S.SOLDATAATENDIMENTO, GETDATE())) % 24) AS HORA, ")
                .Append("S.SOLUSUARIOSOLICITANTE AS MANREQUISITANTE,S.SOLDESCRICAO AS MANDESCRICAOREQUIS, S.SOLOBSMANUTENCAO AS MANDESCRICAOMANUT ")
                'Tabela de solicição do Help desk que agora esta no srv-adm-bd em Goiânia
                .Append("FROM [SQL-ADM].[DBHDMANUTENCAO].[DBO].Tbsolicitacao S ")
                .Append("INNER JOIN TBAPARTAMENTO A ON A.APAAREA = S.SOLAREA ")
                .Append("WHERE S.SOLDATAATENDIMENTO BETWEEN '" & Format(CDate(Data1), "yyyy-MM-dd 00:00:00") & "' AND '" & Format(CDate(Data2), "yyyy-MM-dd 23:59:59") & "' ")
                .Append("AND S.SOLCENTROCUSTO = A.APACUSTO ")
                If ObjManutencaoVO.ApaId > 0 Then
                    .Append("AND A.APAID = " & ObjManutencaoVO.ApaId & " ")
                End If
                .Append("ORDER BY S.SOLDATAATENDIMENTO ")
            End With
            Return PreencheListaHistoricoManutencao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaHistoricoManutencao(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjManutencaoVO = New ManutencaoVO
                With ObjManutencaoVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    If Convert.IsDBNull(ResultadoConsulta.Item("DiaAbertura")) Then
                        .ManAbertura = ""
                    Else
                        .ManAbertura = ResultadoConsulta.Item("DiaAbertura")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraAbertura")) Then
                        .ManHorasAbertura = ""
                    Else
                        .ManHorasAbertura = ResultadoConsulta.Item("HoraAbertura")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DiaConclusao")) Then
                        .ManConclusao = ""
                    Else
                        .ManConclusao = ResultadoConsulta.Item("DiaConclusao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraConclusao")) Then
                        .ManHorasConclusao = ""
                    Else
                        .ManHorasConclusao = ResultadoConsulta.Item("HoraConclusao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Dia")) Then
                        .TempoDiaConserto = ""
                    Else
                        .TempoDiaConserto = ResultadoConsulta.Item("Dia")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Hora")) Then
                        .TempoHoraConserto = ""
                    Else
                        .TempoHoraConserto = ResultadoConsulta.Item("Hora")
                    End If
                    'JUNTANDO O DIA A E AS HORAS EM UM SÓ OBJETO - ESCOHIDO PARA O GRID "TEMPODIACONSERTO"'
                    Select Case .TempoDiaConserto
                        Case 0 : .TempoDiaConserto = ""
                        Case 1 : .TempoDiaConserto = .TempoDiaConserto & " Dia"
                        Case Is > 1 : .TempoDiaConserto = .TempoDiaConserto & " Dias"
                    End Select
                    Select Case .TempoHoraConserto
                        Case 0 : .TempoHoraConserto = ""
                        Case 1 : .TempoHoraConserto = .TempoHoraConserto & " Hora"
                        Case Is > 1 : .TempoHoraConserto = .TempoHoraConserto & " Horas"
                    End Select
                    'ESSE VALOR QUE SERÁ EXIBIDO NO GRID NO CAMPO TEMPO'
                    .TempoDiaConserto = .TempoDiaConserto & " " & .TempoHoraConserto

                    If Convert.IsDBNull(ResultadoConsulta.Item("ManDescricaoRequis")) Then
                        .ManDescricaoManut = ""
                    Else
                        .ManDescricaoManut = ResultadoConsulta.Item("ManDescricaoRequis")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ManRequisitante")) Then
                        .Usuario = ""
                    Else
                        .Usuario = Replace(ResultadoConsulta.Item("ManRequisitante"), "SESC-GO.COM.BR\", "")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ManDescricaoManut")) Then
                        .ManRealizado = ""
                    Else
                        .ManRealizado = ResultadoConsulta.Item("ManDescricaoManut")
                    End If
                End With
                Lista.Add(ObjManutencaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

End Class
