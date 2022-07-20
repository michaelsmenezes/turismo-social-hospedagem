Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class HistoricoHospedesDAO
    Dim objHistoricoHospedesVO As HistoricoHospedesVO
    Dim ojbHistoricoHospedesDAO As HistoricoHospedesDAO
    Public Function Consultar(ByVal HistoricoHospedesVO As HistoricoHospedesVO, ByVal AliasBanco As String, ByVal Dia1 As String, ByVal Dia2 As String, ByVal Nome As String, ByVal ApaId As String, ByVal Dia1Tipo As String, ByVal Dia2Tipo As String, ByVal Categoria As Long, ByVal Servidores As String, ByVal Ordenador As String) As IList
        Try
            Dim Conn As Banco.Conexao = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("select i.IntId,i.ResId,i.IntNome,r.ResNome,h.ApaId,a.ApaDesc,cast(H.HosDataIniSol as date) as DataIni,h.HosHoraIniSol as Horaini, ")
                .AppendLine("h.HosDataIniReal as DataIniReal,h.HosHoraIniReal as HoraIniReal,cast(h.HosDataFimSol as Date) as DataFim, ")
                .AppendLine("h.HosHoraFimSol as HoraFim,h.HosDataFimReal as DataFimReal,h.HosHoraFimReal as HoraFimReal  from VwIntegrante i ")
                .AppendLine("inner join TbReserva r on r.ResId = i.ResId ")
                .AppendLine("inner join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("left join TbApartamento a on a.ApaId = h.ApaId ")
                .AppendLine("Where i.resid > 0 ")
                .AppendLine("and i.IntTipo = 'H' ")
                .AppendLine("and r.ResStatus in ('F','E','R','P') ")
                '.AppendLine("And ((convert(char(10),i.IntDataIni,120) >= '" & Format(CDate(Dia1), "yyyy-MM-dd") & "' and convert(char(10),i.IntDataIni,120) <= '" & Format(CDate(Dia2), "yyyy-MM-dd") & "') or")
                .AppendLine("And (convert(char(10),i.IntDataIni,120) <= '" & Format(CDate(Dia2), "yyyy-MM-dd") & "' and convert(char(10),i.IntDataFim,120) >= '" & Format(CDate(Dia1), "yyyy-MM-dd") & "') ")
                .AppendLine("And r.ResDataIni > '" & Format(DateAdd(DateInterval.Day, -30, CDate(Dia1)), "yyyy-MM-dd") & "' ") 'Só para otimizar e não buscar o banco inteiro
                If Nome.Trim.Length > 0 Then
                    If Servidores = "True" Then
                        .AppendLine("and ((i.IntNome in ( ")
                        .AppendLine("(SELECT [FUNomFunc] ")
                        .AppendLine(" FROM [sql-adm].[FPw].[dbo].[Funciona]))) or ")
                        .AppendLine("(r.ResNome in ( ")
                        .AppendLine("(SELECT [FUNomFunc] ")
                        .AppendLine(" FROM [sql-adm].[FPw].[dbo].[Funciona])))) ")
                        .AppendLine("  and ((i.IntNome like '%" & Nome & "%') ")
                        .AppendLine("    or (r.ResNome like '%" & Nome & "%')) ")
                    Else
                        .AppendLine("and (i.IntNome like '%" & Nome & "%' OR r.ResNome like '%" & Nome & "%')  ")
                    End If
                End If
                If (ApaId.Trim.Length > 0) Then
                    .AppendLine("and (a.ApaDesc like '%" & ApaId & "%' or i.IntPlacaVeiculo like '%" & ApaId & "%') ")
                End If

                .AppendLine("ORDER BY " & Ordenador & " ")
                '.AppendLine("OPTION (OPTIMIZE FOR(@Data1 ='2012-01-01',@Data2 ='2012-01-01')) ")

                '.AppendLine("Declare ")
                '.AppendLine("@Dia1 char(10), @Dia2 char(10), @Nome varchar(100), @ApaId varchar(7), ")
                '.AppendLine("@Dia1Tipo char(1), @Dia2Tipo char(1), @Categoria char(1) ")

                '.AppendLine("declare @Data1 datetime ")
                '.AppendLine("declare @Data2 datetime ")

                '.AppendLine("set @Dia1 = '" & Dia1 & "' ")
                '.AppendLine("set @Dia2 = '" & Dia2 & "' ")
                '.AppendLine("set @Nome = '" & Nome & "' ")
                '.AppendLine("set @ApaId = '" & ApaId & "' ")

                '.AppendLine("set @Data1 = convert(datetime, @Dia1, 103) ")
                '.AppendLine("set @Data2 = convert(datetime, @Dia2 + ' 23:59:59', 103) ")
                '.AppendLine("set @Dia1Tipo = '" & Dia1Tipo & "' ")
                '.AppendLine("set @Dia2Tipo = '" & Dia2Tipo & "' ")

                '.AppendLine("Set @Categoria = 0 ")

                '.AppendLine("set nocount on ")
                '.AppendLine("select  ")
                '.AppendLine("i.IntId, r.ResId, i.IntNome, r.ResNome, ")
                '.AppendLine("case when i.IntDataIniReal is null then Null else h.ApaId end as ApaId,")
                '.AppendLine("case when i.IntDataIniReal is null then Null else a.ApaDesc end as ApaDesc,")

                '.AppendLine("convert(char(10), i.IntDataIni, 103) DataIni, ")
                '.AppendLine("datepart(hh, i.IntDataIni) HoraIni,  ")
                '.AppendLine("convert(char(10), IntDataIniReal, 103) DataIniReal, ")
                '.AppendLine("substring(convert(char(25), i.IntDataIniReal, 120), 12, 5) HoraIniReal, ")
                '.AppendLine("convert(char(10), i.IntDataFim, 103) DataFim, ")
                '.AppendLine("datepart(hh, i.IntDataFim) HoraFim, ")
                ''.AppendLine("convert(char(10), i.IntDataFimReal, 103) DataFimReal, ")
                '.AppendLine("case when i.IntDataIniReal is null then Null else convert(char(10), i.IntDataFimReal, 103) end as DataFimReal, ")
                '.AppendLine("case when i.IntDataIniReal is null then Null else substring(convert(char(25), i.IntDataFimReal, 120), 12, 5) end as HoraFimReal")
                ''.AppendLine("substring(convert(char(25), i.IntDataFimReal, 120), 12, 5) HoraFimReal ")
                '.AppendLine("  from TbIntegrante i with (nolock) ")
                '.AppendLine("  inner join TbReserva r with (nolock) on i.ResId = r.ResId ")
                '.AppendLine("  inner join TbHospedagem h with (nolock) on i.IntId = h.IntId  ")
                '.AppendLine("  and h.HosStatus = case when (i.IntDataIniReal is null and h.HosStatus = 'T') then 'T' else 'A' end ") 'Oginal = A
                '.AppendLine("  inner join TbCategoria c on c.CatId = i.CatId  ")
                '.AppendLine("  left outer join TbApartamento a on a.ApaId = h.ApaId ")
                '.AppendLine("  where r.ResStatus in ('F','E','R') ")
                ''Busca apenas servidores, caso contrário busca os hóspedes normalmente
                'If Servidores = "True" Then
                '    .AppendLine("and ((i.IntNome in ( ")
                '    .AppendLine("(SELECT [FUNomFunc] ")
                '    .AppendLine(" FROM [FPw].[FPw].[dbo].[Funciona]))) or ")
                '    .AppendLine("(r.ResNome in ( ")
                '    .AppendLine("(SELECT [FUNomFunc] ")
                '    .AppendLine(" FROM [FPw].[FPw].[dbo].[Funciona])))) ")
                '    .AppendLine("  and ((i.IntNome like '%' + @Nome + '%') ")
                '    .AppendLine("    or (r.ResNome like '%' + @Nome + '%')) ")
                'Else
                '    .AppendLine("  and ((i.IntNome like '%' + @Nome + '%') ")
                '    .AppendLine("or (r.ResNome like '%' + @Nome + '%')) ")
                'End If
                '.AppendLine("  and ( ")
                '.AppendLine("((r.ResDataIni between @Data1 and @Data2) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((r.ResDataFim between @Data1 and @Data2) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((@Data1 between r.ResDataIni and r.ResDataFim) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((@Data2 between r.ResDataIni and r.ResDataFim) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((i.IntDataIni between @Data1 and @Data2) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((i.IntDataFim between @Data1 and @Data2) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((isNull(i.IntDataIniReal,i.IntDataIni) between @Data1 and @Data2) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((i.IntDataFimReal between @Data1 and @Data2) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((@Data1 between i.IntDataIni and i.IntDataFim) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((@Data2 between i.IntDataIni and i.IntDataFim) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((@Data1 between i.IntDataIniReal and i.IntDataFimReal) and (@Dia1Tipo = 'I') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((@Data2 between i.IntDataIniReal and i.IntDataFimReal) and (@Dia2Tipo = 'F') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((convert(char(10), h.HosDataIniSol, 103) = convert(char(10), @Data1, 103)) and (@Dia1Tipo = 'C') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((convert(char(10), h.HosDataFimSol, 103) = convert(char(10), @Data2, 103)) and (@Dia2Tipo = 'C') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("((convert(char(10), isnull(h.HosDataIniReal, h.HosDataIniSol), 103) = convert(char(10), @Data1, 103)) and (@Dia1Tipo = 'C') and (@Dia2Tipo <> 'C')) or ")
                '.AppendLine("((convert(char(10), isnull(h.HosDataFimReal, h.HosDataFimSol), 103) = convert(char(10), @Data2, 103)) and (@Dia2Tipo = 'C') and (@Dia1Tipo <> 'C')) or ")
                '.AppendLine("(((convert(char(10), h.HosDataIniSol, 103) = convert(char(10), @Data1, 103)) and (@Dia1Tipo = 'C')) and ")
                '.AppendLine("((convert(char(10), h.HosDataFimSol, 103) = convert(char(10), @Data2, 103)) and (@Dia2Tipo = 'C'))) or ")
                '.AppendLine("(((convert(char(10), isnull(h.HosDataIniReal, h.HosDataIniSol), 103) = convert(char(10), @Data1, 103)) and (@Dia1Tipo = 'C')) and ")
                '.AppendLine("((convert(char(10), isnull(h.HosDataFimReal, h.HosDataFimSol), 103) = convert(char(10), @Data2, 103)) and (@Dia2Tipo = 'C')))) ")

                '.AppendLine("  and ((a.ApaDesc like '%' + ")
                '.AppendLine("case ")
                '.AppendLine("  when @ApaId <> '' then @ApaId ")
                '.AppendLine("  else a.ApaDesc ")
                '.AppendLine("end + '%') ")

                '.AppendLine("  or (i.IntPlacaVeiculo = ")
                '.AppendLine("case ")
                '.AppendLine("  when @ApaId = '' then i.IntPlacaVeiculo ")
                '.AppendLine("  else @ApaId ")
                '.AppendLine("end)) ")
                '.AppendLine("  and c.CatLinkCat =  ")
                '.AppendLine("  case ")
                '.AppendLine("when @Categoria = '0' then c.CatLinkCat ")
                '.AppendLine("else @Categoria ")
                '.AppendLine("  end ")
                ''.AppendLine("  order by i.IntNome, i.IntDataIni, i.IntDataFim ")
                '.AppendLine("  order by " & Ordenador & " ")
                '.AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            End With
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function Consultar(ByVal AliasBanco As String, ByVal ResId As Integer) As IList
        Try
            Dim Conn As Banco.Conexao = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("select i.IntId,i.ResId,i.IntNome,r.ResNome,h.ApaId,a.ApaDesc,cast(H.HosDataIniSol as date) as DataIni,h.HosHoraIniSol as Horaini, ")
                .AppendLine("h.HosDataIniReal as DataIniReal,h.HosHoraIniReal as HoraIniReal,cast(h.HosDataFimSol as Date) as DataFim, ")
                .AppendLine("h.HosHoraFimSol as HoraFim,h.HosDataFimReal as DataFimReal,h.HosHoraFimReal as HoraFimReal  from VwIntegrante i ")
                .AppendLine("inner join TbReserva r on r.ResId = i.ResId ")
                .AppendLine("inner join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("left join TbApartamento a on a.ApaId = h.ApaId ")
                .AppendLine("Where i.resid = " & ResId)
                .AppendLine("ORDER BY i.IntNome ")
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
                objHistoricoHospedesVO = New HistoricoHospedesVO
                With objHistoricoHospedesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaId")) Then
                        .ApaId = 0
                    Else
                        .ApaId = ResultadoConsulta.Item("ApaId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaDesc")) Then
                        .ApaDesc = ""
                    Else
                        .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataFim")) Then
                        .DataFim = ""
                    Else
                        .DataFim = ResultadoConsulta.Item("DataFim")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataFimReal")) Then
                        .DataFimReal = ""
                    Else
                        .DataFimReal = ResultadoConsulta.Item("DataFimReal")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("DataIni")) Then
                        .DataIni = ""
                    Else
                        .DataIni = ResultadoConsulta.Item("DataIni")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataIniReal")) Then
                        .DataIniReal = ""
                    Else
                        .DataIniReal = ResultadoConsulta.Item("DataIniReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraFim")) Then
                        .HoraFim = ""
                    Else
                        .HoraFim = ResultadoConsulta.Item("HoraFim")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraFimReal")) Then
                        .HoraFimReal = ""
                    Else
                        .HoraFimReal = ResultadoConsulta.Item("HoraFimReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraIni")) Then
                        .HoraIni = ""
                    Else
                        .HoraIni = ResultadoConsulta.Item("HoraIni")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraIniReal")) Then
                        .HoraIniReal = ""
                    Else
                        .HoraIniReal = ResultadoConsulta.Item("HoraIniReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntId")) Then
                        .IntId = 0
                    Else
                        .IntId = ResultadoConsulta.Item("IntId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntNome").ToString.ToUpper) Then
                        .IntNome = ""
                    Else
                        .IntNome = ResultadoConsulta.Item("IntNome").ToString.ToUpper
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ResId")) Then
                        .ResId = 0
                    Else
                        .ResId = ResultadoConsulta.Item("ResId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ResNome").ToString.ToUpper) Then
                        .ResNome = ""
                    Else
                        .ResNome = ResultadoConsulta.Item("ResNome").ToString.ToUpper
                    End If
                End With
                Lista.Add(objHistoricoHospedesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function



End Class
