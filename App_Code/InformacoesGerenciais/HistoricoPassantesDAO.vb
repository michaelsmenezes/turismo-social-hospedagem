Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class HistoricoPassantesDAO
    Inherits HistoricoHospedesVO
    Public Function ConsultaPassantes(ByVal HistoricoHospedesVO As HistoricoHospedesVO, ByVal AliasBanco As String, ByVal Dia1 As String, ByVal Dia2 As String, ByVal Nome As String, ByVal Categoria As Integer, ByVal Servidores As String, ByVal Idade As String, ByVal Ordenador As String, ByVal FuncCaldasNovas As String) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                '.Append("Declare ")
                '.Append("@Dia1 char(10), @Dia2 char(10), @Nome varchar(100), @Placa char(7), @Categoria integer, @Caldas char(1) ")

                '.Append("set nocount on ")
                '.Append("declare @Data1 datetime ")
                '.Append("declare @Data2 datetime ")

                '.Append("set @Dia1 = '" & Dia1 & "' ")
                '.Append("set @Dia2 = '" & Dia2 & "' ")
                '.Append("set @Nome = '" & Nome & "' ")
                '.Append("set @Placa = '" & Placa & "' ")
                '.Append("set @Categoria = " & Categoria & " ")
                '.Append("set @Caldas = '" & Servidores.Trim & "' ")

                '.Append("set @Data1 = convert(datetime, @Dia1, 103) ")
                '.Append("set @Data2 = convert(datetime, @Dia2 + ' 23:59:59', 103) ")
                '.Append("set @Data2 = @Data2 + 1 ")


                '.Append("select ")
                '.Append("p.IntId, p.IntNome, ")
                '.Append("  case p.ResId ")
                '.Append("    when 0 then '' ")
                '.Append("    else r.ResNome ")
                '.Append("  end as Observacao, ")
                '.Append("  c.CatDescricao as Categoria, ")
                '.Append("  p.IntPlacaVeiculo as Placa, ")
                '.Append("  convert(char(10), IntDataIniReal, 103) DataIniReal, ")
                '.Append("  substring(convert(char(25), p.IntDataIniReal, 120), 12, 5) HoraIniReal, ")
                '.Append("  case ")
                '.Append("    when p.IntDataFimReal is null then convert(char(10), p.IntDataFim, 103) ")
                '.Append("    else convert(char(10), p.IntDataFimReal, 103) ")
                '.Append("  end as DataFimReal, ")
                '.Append("  case ")
                '.Append("    when p.IntDataFimReal is null then substring(convert(char(25), p.IntDataFim, 120), 12, 5) ")
                '.Append("    else substring(convert(char(25), p.IntDataFimReal, 120), 12, 5) ")
                '.Append("  end as HoraFimReal, ")
                '.Append("  case p.IntStatus ")
                '.Append("    when  'F' then 'Finalizado' ")
                '.Append("    else 'Parque Aquático' ")
                '.Append("  end as Situacao, p.IntDtNascimento, ")
                '.Append("  case ")
                '.Append("    when p.ResId > 0 then 'Excursão' ")
                '.Append("    when p.PreIntId > 0 then (select replace(IntUsuario, 'SESC-GO.COM.BR\', '') ")
                '.Append("      from TbPreIntegrante ii ")
                '.Append("      where ii.PreIntId = p.PreIntId) ")
                '.Append("    when p.ResId = 0 then replace(p.IntUsuario, 'SESC-GO.COM.BR\', '') ")
                '.Append("  end as Atendente ")
                '.Append("  from VwPassante p with (nolock) ")
                '.Append("  left join TbReserva r with (nolock) ")
                '.Append("  on p.ResId = r.ResId ")
                '.Append("  inner join TbCategoria c with (nolock) ")
                '.Append("  on p.CatId = c.CatId ")
                '.Append("  and c.CatLinkCat =  ")
                '.Append("  case ")
                '.Append("    when @Categoria = 0 then c.CatLinkCat ")
                '.Append("    else @Categoria ")
                '.Append("  end ")
                '.Append("  where p.IntStatus in ('P','F') ")
                '.Append("  and ((p.IntNome like '%' + @Nome + '%') ")
                '.Append("    or (r.ResNome like '%' + @Nome + '%')) ")
                '.Append("  and (p.IntDataIni >=  @Data1 and p.IntDataIni < @Data2) ")
                '.Append("  and (p.IntPlacaVeiculo = @Placa) ")
                '.Append("  and p.IntPasComCld = case ")
                '.Append("    when @Caldas = 'S' then 'S' ")
                '.Append("    else p.IntPasComCld ")
                '.Append("  end ")

                .Append("select ")
                .Append("i.IntId,i.IntNome, ")
                .Append("case CONVERT(nvarchar(10),i.IntDtNascimento,103) ")
                .Append("   when  '01/01/1900'  then ' ' ")
                .Append("else ")
                .Append("   CONVERT(nvarchar(10),i.IntDtNascimento,103) end as IntDtNascimento, ")
                .Append("Floor(datediff(Day,i.IntDtNascimento,convert(datetime,i.intdataIniReal,101))/365.25) as Idade, ")
                .Append("c.CatDescricao as Categoria, ")
                .Append("case i.ResId ")
                .Append("when 0 then '' ")
                .Append("else ")
                .Append("r.ResNome end as Observacao,i.IntPlacaVeiculo as Placa,convert(nvarchar(10),i.IntDataIniReal,103) as DataIniReal,CONVERT(nvarchar(5),i.IntDataIniReal,108) as HoraIniReal, ")
                .Append("convert(nvarchar(10),i.IntDataFimReal,103) as DataFimReal,CONVERT(nvarchar(5),i.IntDataFimReal,108) as HoraFimReal, ")
                .Append("case i.IntStatus ")
                .Append("when  'F' then 'Finalizado' ")
                .Append("else 'Parque Aquático' end as Situacao, i.IntUsuario as Atendente, ")
                .Append("case ")
                .Append("when g.IntPasAntAlm = 0 and g.IntVinculoId = 0  then '' ")
                .Append("when g.IntPasAntAlm > 0 then g.IntNome ")
                .Append("when g.IntPasAntAlm = 0 and g.IntVinculoId > 0  then isnull((Select CAST(IntPasAntAlm as varchar) +' Refeições contidas no cartão: '+ intNome from TbIntegrante ii where ii.IntId = g.IntVinculoId and ii.IntPasAntAlm > 0),'') ")
                .Append("else ")
                .Append("(Select intNome from TbIntegrante ii where ii.IntId = g.IntVinculoId) ")
                .Append("end as VinculoRefeicao ")

                '.Append("case ")
                '.Append("when g.IntPasAntAlm = 0  then '' ")
                '.Append("when g.IntPasAntAlm > 0 and g.IntVinculoId = 0  then g.IntNome  ")
                '.Append("else ")
                '.Append("(Select intNome from TbIntegrante ii where ii.IntId = g.IntVinculoId)  ")
                '.Append("end as VinculoRefeicao ")


                .Append("from VwPassante i ")
                .Append("left join TbReserva r on r.resid = i.ResId ")
                .Append("inner join TbCategoria c on c.CatId = i.CatId ")
                .Append("inner join tbIntegrante g on g.intId = i.intId ")
                If FuncCaldasNovas = "S" Then
                    .Append("inner join [dbgeral].[dbo].TbFuncionarios f on f.funCPF = REPLACE(g.IntCPF,'/','') and f.funCodLotacao between '102000000' and '102000009' ")
                End If
                .Append("where i.IntDataIni >= CONVERT(DateTime,'" & Dia1 & " 00:00:00',103) and i.IntDataFim <= CONVERT(DateTime,'" & Dia2 & " 23:59:59',103) ")
                If Nome.Trim.ToString.Length > 0 Then
                    .Append("and (i.IntNome like '%" & Nome & "%' or r.ResNome like '%" & Nome & "%') ")
                End If
                If Servidores = "S" Then
                    .Append("and i.IntPasComCld = 'S' ")
                End If
                If Idade = "4" Then
                    .Append("and Floor(datediff(Day,i.IntDtNascimento,convert(datetime,GetDate(),101))/365.25)  < 5 ")
                ElseIf Idade = "9" Then
                    .Append("and Floor(datediff(Day,i.IntDtNascimento,convert(datetime,GetDate(),101))/365.25)  > 4 ")
                    .Append("and Floor(datediff(Day,i.IntDtNascimento,convert(datetime,GetDate(),101))/365.25)  < 10 ")
                End If
                If Categoria > 0 Then
                    .Append("and i.CatId = " & Categoria & " ")
                End If

                ''Listar crianças entre 0 e 10 anos
                'Select Case Idade
                '    Case "4" 'Mostrar apenas os menores de 5 anos'
                '        .Append("and (select DATEDIFF(year,p.IntDtNascimento,GETDATE())) < 5 ")
                '    Case "9" 'Irá mostra os que possuirem de 5 a 9 anos
                '        .Append("and ((select DATEDIFF(year,p.IntDtNascimento,GETDATE())) > 4 and (select DATEDIFF(year,p.IntDtNascimento,GETDATE())) < 10)")
                'End Select
                .Append("  order by " & Ordenador & " ")
            End With
            Return PreencheListaPassantes(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaPassantes(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim objHistoricoPassantesVO As New HistoricoPassantesVO
                With objHistoricoPassantesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntId")) Then
                        .IntId = 0
                    Else
                        .IntId = ResultadoConsulta.Item("IntId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntNome")) Then
                        .IntNome = ""
                    Else
                        .IntNome = ResultadoConsulta.Item("IntNome")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntDtNascimento")) Then
                        .IntDtNascimento = ""
                    ElseIf ResultadoConsulta.Item("IntDtNascimento") = "01/01/1900" Then
                        .IntDtNascimento = ""
                    Else
                        .IntDtNascimento = ResultadoConsulta.Item("IntDtNascimento")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("Categoria")) Then
                        .Categoria = ""
                    Else
                        .Categoria = ResultadoConsulta.Item("Categoria")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Observacao")) Then
                        .Observacao = ""
                    Else
                        .Observacao = ResultadoConsulta.Item("Observacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Placa")) Then
                        .Placa = ""
                    Else
                        .Placa = ResultadoConsulta.Item("Placa")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("DataIniReal")) Then
                        .DataIniReal = ""
                    Else
                        .DataIniReal = ResultadoConsulta.Item("DataIniReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraIniReal")) Then
                        .HoraIniReal = ""
                    Else
                        .HoraIniReal = ResultadoConsulta.Item("HoraIniReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataFimReal")) Then
                        .DataFimReal = ""
                    Else
                        .DataFimReal = ResultadoConsulta.Item("DataFimReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraFimReal")) Then
                        .HoraFimReal = ""
                    Else
                        .HoraFimReal = ResultadoConsulta.Item("HoraFimReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Situacao")) Then
                        .Situacao = ""
                    Else
                        .Situacao = ResultadoConsulta.Item("Situacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Atendente")) Then
                        .Atendente = ""
                    Else
                        .Atendente = ResultadoConsulta.Item("Atendente")
                    End If
                    If (Convert.IsDBNull(ResultadoConsulta.Item("Idade")) Or .IntDtNascimento = " ") Then
                        .Idade = ""
                    Else
                        .Idade = ResultadoConsulta.Item("Idade")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("VinculoRefeicao")) Then
                        .VinculoRefeicao = ""
                    Else
                        .VinculoRefeicao = ResultadoConsulta.Item("VinculoRefeicao")
                    End If
                End With
                Lista.Add(objHistoricoPassantesVO)

            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
