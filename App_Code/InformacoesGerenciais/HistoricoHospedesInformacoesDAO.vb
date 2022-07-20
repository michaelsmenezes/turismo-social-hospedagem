Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class HistoricoHospedesInformacoesDAO
    Public Function ConsultaSituacaoCortesia(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("declare ")
                .Append("@IntId numeric ")

                .Append("set @IntId = " & IntId & " ")

                .Append("select ")
                .Append("IntId, IntMatricula, IntNome, IntRG, IntCPF, ")
                .Append("  convert(char(10), IntDataIni, 103) as DataIni, ")
                .Append("  substring(convert(char(25), IntDataIni, 120), 12, 5) HoraIni, ")
                .Append("  convert(char(10), IntDataIniReal, 103) as DataIniReal, ")
                .Append("  substring(convert(char(25), IntDataIniReal, 120), 12, 5) HoraIniReal, ")
                .Append("  convert(char(10), IntDataFim, 103) as DataFim, ")
                .Append("  substring(convert(char(25), IntDataFim, 120), 12, 5) HoraFim, ")
                .Append("  convert(char(10), IntDataFimReal, 103) as DataFimReal, ")
                .Append("  substring(convert(char(25), IntDataFimReal, 120), 12, 5) HoraFimReal, ")
                .Append("  IntDtNascimento, IntSexo, ")
                .Append("  case ")
                .Append("    when IntStatus = 'P' then 'Parque Aquático' ")
                .Append("    when IntStatus = 'E' then 'Estada' ")
                .Append("    when IntStatus = 'F' then 'Finalizado' ")
                .Append("    else ' ' ")
                .Append("  end as Status, IntMemorando, ")
                .Append("  case ")
                .Append("    when IntEmissor = '1' then 'FECOMÉRCIO' ")
                .Append("    when IntEmissor = '2' then 'DR' ")
                .Append("    when IntEmissor = '3' then 'DOS' ")
                .Append("    when IntEmissor = '4' then 'DA' ")
                .Append("    when IntEmissor = '5' then 'DIFIN' ")
                .Append("    else ' ' ")
                .Append("  end as Emissor, IntCartao, abs(i.IntSaldo) as IntSaldo, ")
                .Append("  IntCidade, ")
                .Append("  case when IntCortesiaCaucao = 'N' then 'Não' else 'Sim' end as IntCortesiaCaucao, ")
                .Append("  case when IntCortesiaConsumo = 'N' then 'Não' else 'Sim' end as IntCortesiaConsumo, ")
                .Append("  case when IntCortesiaRestaurante = 'N' then 'Não' else 'sim' end as IntCortesiaRestaurante, ")
                .Append("  IntCortesiaResponsavel, ")
                .Append("  IntPlacaVeiculo, IntAlmoco, IntJantar, ")
                .Append("  IntTotalAlmoco, IntTotalJantar, e.EstDescricao, f.FormaPagto, ")
                .Append("  c.CatDescricao, cc.CatDescricao ")
                .Append("    from  ")
                .Append("    TbIntegrante i inner join TbEstadoPais e ")
                .Append("    on i.EstId = e.EstId ")
                .Append("    inner join VwFormaPagto f ")
                .Append("    on i.IntFormaPagamento = f.FormaPagtoId ")
                .Append("    inner join TbCategoria c ")
                .Append("    on i.CatId = c.Catid ")
                .Append("    inner join TbCategoria cc ")
                .Append("    on i.IntCatCobranca = cc.CatId ")
                .Append("    where IntId = @IntId ")
                Return PreencheListaSituacaoCortesia(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaSituacaoCortesia(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjHistHspInformacoesVO = New HistoricoHospedesInformacoesVO
                With ObjHistHspInformacoesVO
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
                    If Convert.IsDBNull(ResultadoConsulta.Item("Status")) Then
                        .Status = ""
                    Else
                        .Status = ResultadoConsulta.Item("Status")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Status")) Then
                        .Status = ""
                    Else
                        .Status = ResultadoConsulta.Item("Status")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntSaldo")) Then
                        .IntSaldo = ""
                    Else
                        .IntSaldo = ResultadoConsulta.Item("IntSaldo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaCaucao")) Then
                        .IntCortesiaCaucao = ""
                    Else
                        .IntCortesiaCaucao = ResultadoConsulta.Item("IntCortesiaCaucao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaConsumo")) Then
                        .IntCortesiaConsumo = ""
                    Else
                        .IntCortesiaConsumo = ResultadoConsulta.Item("IntCortesiaConsumo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaRestaurante")) Then
                        .IntCortesiaRestaurante = ""
                    Else
                        .IntCortesiaRestaurante = ResultadoConsulta.Item("IntCortesiaRestaurante")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("FormaPagto")) Then
                        .FormaPagto = ""
                    Else
                        .FormaPagto = ResultadoConsulta.Item("FormaPagto")
                    End If
                End With
                Lista.Add(ObjHistHspInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaInformacoesHospedagem(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Declare ")
                .Append("@IntId numeric ")

                .Append("set @IntId = " & IntId & " ")

                .Append("select a.ApaId, a.ApaDesc, ")
                .Append("c.AcmDescricao Acomodacao, ")
                .Append("cc.AcmDescricao AcomodacaoCobranca, ")
                .Append("convert(char(5), HosDataIniSol, 103) HosDataIniSol, ")
                .Append("HosHoraIniSol, ")
                .Append("convert(char(5), HosDataIniReal, 103) HosDataIniReal, ")
                .Append("substring(convert(char(25), HosDataIniReal, 120), 12, 5) as HosHoraIniReal, ")
                .Append("convert(char(5), HosDataFimSol, 103) HosDataFimSol, ")
                .Append("HosHoraFimSol, ")
                .Append("convert(char(5), HosDataFimReal, 103) HosDataFimReal, ")
                .Append("substring(convert(char(25), HosDataFimReal, 120), 12, 5) as HosHoraFimReal, ")
                .Append("HosValorDevido, ")
                .Append("HosValorPago, ")
                .Append("case ")
                .Append("when HosStatus = 'T' then 'Transferido' ")
                .Append("  when HosStatus = 'A' then ")
                .Append("    case ")
                .Append("      when isdate(HosDataFimReal) = 1 then 'Finalizado' ")
                .Append("      else 'Atual' ")
                .Append("    end ")
                .Append("  else 'A transferir' ")
                .Append("end HosStatus ")
                .Append("  from TbHospedagem h ")
                .Append("  inner join TbAcomodacao c ")
                .Append("  on h.AcmId = c.AcmId ")
                .Append("  inner join TbAcomodacao cc ")
                .Append("  on h.AcmIdCobranca = cc.AcmId ")
                .Append("  left outer join TbApartamento a ")
                .Append("  on a.ApaId = h.ApaId ")
                .Append("  where h.IntId = @IntId ")
                .Append("  order by HosDataIniSol ")
            End With
            Return PreencheListaInformacoesHospedagem(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaInformacoesHospedagem(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjHistHspInformacoesVO = New HistoricoHospedesInformacoesVO
                With ObjHistHspInformacoesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaDesc")) Then
                        .ApaDesc = ""
                    Else
                        .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Acomodacao")) Then
                        .Acomodacao = ""
                    Else
                        .Acomodacao = ResultadoConsulta.Item("Acomodacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("AcomodacaoCobranca")) Then
                        .AcomodacaoCobranca = ""
                    Else
                        .AcomodacaoCobranca = ResultadoConsulta.Item("AcomodacaoCobranca")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosDataIniSol")) Then
                        .HosDataIniSol = ""
                    Else
                        .HosDataIniSol = ResultadoConsulta.Item("HosDataIniSol")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosHoraIniSol")) Then
                        .HosHoraIniSol = ""
                    Else
                        .HosHoraIniSol = ResultadoConsulta.Item("HosHoraIniSol")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("HosDataIniReal")) Then
                        .DataIniReal = ""
                    Else
                        .DataIniReal = ResultadoConsulta.Item("HosDataIniReal")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("HosHoraIniReal")) Then
                        .HoraIniReal = ""
                    Else
                        .HoraIniReal = ResultadoConsulta.Item("HosHoraIniReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosDataFimSol")) Then
                        .HosDataFimSol = ""
                    Else
                        .HosDataFimSol = ResultadoConsulta.Item("HosDataFimSol")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosHoraFimSol")) Then
                        .HosHoraFimSol = ""
                    Else
                        .HosHoraFimSol = ResultadoConsulta.Item("HosHoraFimSol")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosDataFimReal")) Then
                        .DataFimReal = ""
                    Else
                        .DataFimReal = ResultadoConsulta.Item("HosDataFimReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosHoraFimReal")) Then
                        .HoraFimReal = ""
                    Else
                        .HoraFimReal = ResultadoConsulta.Item("HosHoraFimReal")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosValorDevido")) Then
                        .HosValorDevido = ""
                    Else
                        .HosValorDevido = ResultadoConsulta.Item("HosValorDevido")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosValorPago")) Then
                        .HosValorPago = ""
                    Else
                        .HosValorPago = ResultadoConsulta.Item("HosValorPago")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HosStatus")) Then
                        .HosStatus = ""
                    Else
                        .HosStatus = ResultadoConsulta.Item("HosStatus")
                    End If
                End With
                Lista.Add(ObjHistHspInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaConsumoHospedes(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("declare @IntId numeric ")
                .Append("set @IntId = " & IntId & " ")
                .Append("select ")
                .Append("convert(char(5), CIntData, 103) as Dia, ")
                .Append("substring(convert(char(25), CIntData, 120), 12, 5) as Hora, ")
                .Append("CIIQuantidade, LncPreVnd, ")
                .Append("case ")
                .Append("  when PrdPes = 'S' then round((CIIQuantidade * LncPreVnd), 2) ")
                .Append("  else round(CIIQuantidade * LncPreVnd, 2) ")
                .Append("end as Valor, ")
                .Append("CIICortesia, CIIFormaConsumo, CxaPdvNom, OprCod, PrdCupDes, ")
                .Append("PrdPes, UniCodSai ")
                .Append("from TbConsumoIntegrante c ")
                .Append("  inner join TbConsumoIntegranteItem i ")
                .Append("  on c.CIntId = i.CIntId ")
                .Append("  inner join TbCxaAbe ca ")
                .Append("  on c.CxaAbeCod = ca.CxaAbeCod ")
                .Append("  inner join TbEstoqueProduto p ")
                .Append("  on i.PrdCod = p.PrdCod ")
                .Append("  where IntId = @IntId and CIIQuantidade > 0 ")
                .Append("  order by CIntData ")
            End With
            Return PreencheListaConsumoHospede(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaConsumoHospede(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read()
                Dim ObjHistHspInformacoesVO As New HistoricoHospedesInformacoesVO
                With ObjHistHspInformacoesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Dia")) Then
                        .Dia = ""
                    Else
                        .Dia = ResultadoConsulta.Item("Dia")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Hora")) Then
                        .Hora = ""
                    Else
                        .Hora = ResultadoConsulta.Item("Hora")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("PrdCupDes")) Then
                        .Descricao = ""
                    Else
                        .Descricao = ResultadoConsulta.Item("PrdCupDes")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("LncPreVnd")) Then
                        .PrecoVenda = 0
                    Else
                        .PrecoVenda = ResultadoConsulta.Item("LncPreVnd")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CIIQuantidade")) Then
                        .Quantidade = ""
                    Else
                        .Quantidade = ResultadoConsulta.Item("CIIQuantidade")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("UniCodSai")) Then
                        .UnidadeMedida = ""
                    Else
                        .UnidadeMedida = ResultadoConsulta.Item("UniCodSai")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Valor")) Then
                        .ValorTotal = 0
                    Else
                        .ValorTotal = ResultadoConsulta.Item("Valor")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CIIFormaConsumo")) Then
                        .FormaConsumo = ""
                    Else
                        .FormaConsumo = ResultadoConsulta.Item("CIIFormaConsumo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CxaPdvNom")) Then
                        .LocalConsumo = ""
                    Else
                        .LocalConsumo = ResultadoConsulta.Item("CxaPdvNom")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("OprCod")) Then
                        .Operador = ""
                    Else
                        .Operador = ResultadoConsulta.Item("OprCod")
                    End If
                End With
                Lista.Add(ObjHistHspInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaFinanceiraHospedes(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Declare ")
                .Append("@IntId numeric(18) ")
                .Append("set @IntId = " & IntId & " ")
                'Esta procedure levanta toda a movimentação financeira desempenhada por um integrante
                .Append("select ")
                .Append("o.CxaCrtCod as Caixa, ")
                .Append("o.OprNum as Operacao, ")
                .Append("convert(char(10), OprDat, 103) as Dia, ")
                .Append(" substring(o.OprHor,1,5) as Hora, ")
                .Append("op.TipOprDes as Descricao, ")
                .Append("o.OprVal as Valor, ")
                .Append("case o.OprTipPag ")
                .Append("when 'DH' then 'Dinheiro' ")
                .Append("when 'CH' then 'Cheque' ")
                .Append("when 'CC' then 'Cartão de Crédito' ")
                .Append("when 'VN' then 'VisaNet' ")
                .Append("when 'RC' then 'RedeCard' ")
                .Append("when 'RS' then 'RedeShop' ")
                .Append("when 'VE' then 'Visa Electron' ")
                .Append("     else 'Erro - Informe ao Cein' ")
                .Append("end as Forma, ")
                .Append("case o.OprEstorno ")
                .Append("when 'S' then 'Operação Estornada - ' + o.OprObsEstorno ")
                .Append("     else '' ")
                .Append("end as Observacao, ")
                .Append("c.CxaCrtOPr as Operador ")
                .Append("from TbCxaOpr o with(nolock) inner join TbCxaTop op with(nolock) ")
                .Append("     on o.TipOprCod = op.TipOprCod ")
                .Append("     inner join TbCxaCxa c with(nolock) ")
                .Append("     on c.CxaCrtCod = o.CxaCrtCod ")
                .Append("where o.IntId = @IntId ")
            End With
            Return PreencheListaFinanceiraHospede(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaFinanceiraHospede(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjHistHspInformacoesVO As New HistoricoHospedesInformacoesVO
                With ObjHistHspInformacoesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Caixa")) Then
                        .Caixa = ""
                    Else
                        .Caixa = ResultadoConsulta.Item("Caixa")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Operacao")) Then
                        .Operacao = ""
                    Else
                        .Operacao = ResultadoConsulta.Item("Operacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Dia")) Then
                        .Dia = ""
                    Else
                        .Dia = ResultadoConsulta.Item("Dia")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Hora")) Then
                        .Hora = ""
                    Else
                        .Hora = ResultadoConsulta.Item("Hora")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Descricao")) Then
                        .Descricao = ""
                    Else
                        .Descricao = ResultadoConsulta.Item("Descricao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Valor")) Then
                        .ValorTotal = 0
                    Else
                        .ValorTotal = ResultadoConsulta.Item("Valor")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Forma")) Then
                        .FormaPagto = ""
                    Else
                        .FormaPagto = ResultadoConsulta.Item("Forma")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Observacao")) Then
                        .Observacao = ""
                    Else
                        .Observacao = ResultadoConsulta.Item("Observacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Operador")) Then
                        .Operador = ""
                    Else
                        .Operador = ResultadoConsulta.Item("Operador")
                    End If
                End With
                Lista.Add(ObjHistHspInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaLogEventosHospedes(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("declare ")
                .Append("@IntId numeric ")
                .Append("set @IntId = " & IntId & " ")

                .Append("set nocount on ")
                .Append("declare Integrante_cursor cursor for ")
                .Append("select  ")
                .Append("  IntId, 'U' IntAcao, ResId, IntMatricula, IntDataIni, IntDataIniReal, ")
                .Append("    IntDataFim, IntDataFimReal, CatId, IntNome, IntDtNascimento, ")
                .Append("    IntStatus, IntFormaPagamento, IntCatCobranca, IntMemorando, ")
                .Append("    IntEmissor, IntCartao, IntCortesiaConsumo, IntCortesiaResponsavel, ")
                .Append("    IntDesjejum, IntAlmoco, IntJantar, IntFechamComSaldoResponsavel, ")
                .Append("    replace(IntUsuario,'SESC-GO.COM.BR\',''), IntUsuarioData ")
                .Append("  from TbIntegrante i with (nolock) ")
                .Append("  where IntId = @IntId ")
                .Append("union ")
                .Append("select ")
                .Append("  IntId, IntAcao, ResId, IntMatricula, IntDataIni, IntDataIniReal, ")
                .Append("    IntDataFim, IntDataFimReal, CatId, IntNome, IntDtNascimento, ")
                .Append("    IntStatus, IntFormaPagamento, IntCatCobranca, IntMemorando, ")
                .Append("    IntEmissor, IntCartao, IntCortesiaConsumo, IntCortesiaResponsavel, ")
                .Append("    IntDesjejum, IntAlmoco, IntJantar, IntFechamComSaldoResponsavel, ")
                .Append("    replace(IntUsuario,'SESC-GO.COM.BR\',''), IntUsuarioData ")
                .Append("  from TbIntegranteLog il with (nolock) ")
                .Append("  where IntId = @IntId ")
                .Append("  order by IntUsuarioData ")

                .Append("create table #Log ( ")
                .Append("  Evento [varchar] (1000), ")
                .Append("  Usuario [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AI NULL , ")
                .Append("  Data [char] (10) NULL, ")
                .Append("  Horario [char] (5)) ")

                .Append("declare ")
                .Append("  @IntId1 [numeric](18, 0), ")
                .Append("  @IntAcao1 [char] (1), ")
                .Append("  @ResId1 [numeric](18, 0), ")
                .Append("  @IntMatricula1 [varchar] (15), ")
                .Append("  @IntDataIni1 [datetime], ")
                .Append("  @IntDataIniReal1 [datetime], ")
                .Append("  @IntDataFim1 [datetime], ")
                .Append("  @IntDataFimReal1 [datetime], ")
                .Append("  @CatId1 [numeric](18, 0), ")
                .Append("  @IntNome1 [varchar] (80), ")
                .Append("  @IntDtNascimento1 [datetime], ")
                .Append("  @IntStatus1 [char] (1), ")
                .Append("  @IntFormaPagamento1 [char] (2), ")
                .Append("  @IntCatCobranca1 [numeric](18, 0), ")
                .Append("  @IntMemorando1 [varchar] (100), ")
                .Append("  @IntEmissor1 [smallint], ")
                .Append("  @IntCartao1 [varchar] (11), ")
                .Append("  @IntCortesiaConsumo1 [char] (1), ")
                .Append("  @IntCortesiaResponsavel1 [varchar] (60), ")
                .Append("  @IntDesjejum1 [char] (1), ")
                .Append("  @IntAlmoco1 [char] (1), ")
                .Append("  @IntJantar1 [char] (1), ")
                .Append("  @IntFechamComSaldoResponsavel1 [varchar] (60), ")
                .Append("  @IntUsuario1 [varchar] (60), ")
                .Append("  @IntUsuarioData1 [datetime], ")
                .Append("  @IntId2 [numeric](18, 0),  ")
                .Append("  @IntAcao2 [char] (1), ")
                .Append("  @ResId2 [numeric](18, 0), ")
                .Append("  @IntMatricula2 [varchar] (15), ")
                .Append("  @IntDataIni2 [datetime], ")
                .Append("  @IntDataIniReal2 [datetime], ")
                .Append("  @IntDataFim2 [datetime], ")
                .Append("  @IntDataFimReal2 [datetime], ")
                .Append("  @CatId2 [numeric](18, 0), ")
                .Append("  @IntNome2 [varchar] (80), ")
                .Append("  @IntDtNascimento2 [datetime], ")
                .Append("  @IntStatus2 [char] (1), ")
                .Append("  @IntFormaPagamento2 [char] (2), ")
                .Append("  @IntCatCobranca2 [numeric](18, 0), ")
                .Append("  @IntMemorando2 [varchar] (100), ")
                .Append("  @IntEmissor2 [smallint], ")
                .Append("  @IntCartao2 [varchar] (11), ")
                .Append("  @IntCortesiaConsumo2 [char] (1), ")
                .Append("  @IntCortesiaResponsavel2 [varchar] (60), ")
                .Append("  @IntDesjejum2 [char] (1), ")
                .Append("  @IntAlmoco2 [char] (1), ")
                .Append("  @IntJantar2 [char] (1), ")
                .Append("  @IntFechamComSaldoResponsavel2 [varchar] (60), ")
                .Append("  @IntUsuario2 [varchar] (60), ")
                .Append("  @IntUsuarioData2 [datetime] ")

                .Append("open Integrante_cursor ")
                .Append("fetch next from Integrante_cursor ")
                .Append("  into @IntId1, @IntAcao1, @ResId1, ")
                .Append("  @IntMatricula1, @IntDataIni1, @IntDataIniReal1, @IntDataFim1, ")
                .Append("  @IntDataFimReal1, @CatId1, @IntNome1, @IntDtNascimento1, ")
                .Append("  @IntStatus1, @IntFormaPagamento1, @IntCatCobranca1, ")
                .Append("  @IntMemorando1, @IntEmissor1, @IntCartao1, ")
                .Append("  @IntCortesiaConsumo1, @IntCortesiaResponsavel1, ")
                .Append("  @IntDesjejum1, @IntAlmoco1, @IntJantar1, ")
                .Append("  @IntFechamComSaldoResponsavel1, @IntUsuario1, @IntUsuarioData1 ")
                .Append("insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("  values ('Inserção do Hóspede', @IntUsuario1, ")
                .Append("    convert(char(10), @IntUsuarioData1, 103), ")
                .Append("    convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("while @@fetch_status = 0 ")
                .Append("begin ")
                .Append("  set @IntId2 = @IntId1 ")
                .Append("  set @IntAcao2 = @IntAcao1 ")
                .Append("  set @ResId2 = @ResId1 ")
                .Append("  set @IntMatricula2 = @IntMatricula1 ")
                .Append("  set @IntDataIni2 = @IntDataIni1 ")
                .Append("  set @IntDataIniReal2 = @IntDataIniReal1 ")
                .Append("  set @IntDataFim2 = @IntDataFim1 ")
                .Append("  set @IntDataFimReal2 = @IntDataFimReal1 ")
                .Append("  set @CatId2 = @CatId1 ")
                .Append("  set @IntNome2 = @IntNome1 ")
                .Append("  set @IntDtNascimento2 = @IntDtNascimento1 ")
                .Append("  set @IntStatus2 = @IntStatus1 ")
                .Append("  set @IntFormaPagamento2 = @IntFormaPagamento1 ")
                .Append("  set @IntCatCobranca2 = @IntCatCobranca1 ")
                .Append("  set @IntMemorando2 = @IntMemorando1 ")
                .Append("  set @IntEmissor2 = @IntEmissor1 ")
                .Append("  set @IntCartao2 = @IntCartao1 ")
                .Append("  set @IntCortesiaConsumo2 = @IntCortesiaConsumo1 ")
                .Append("  set @IntCortesiaResponsavel2 = @IntCortesiaResponsavel1 ")
                .Append("  set @IntDesjejum2 = @IntDesjejum1 ")
                .Append("  set @IntAlmoco2 = @IntAlmoco1 ")
                .Append("  set @IntJantar2 = @IntJantar1 ")
                .Append("  set @IntFechamComSaldoResponsavel2 = @IntFechamComSaldoResponsavel1 ")
                .Append("  set @IntUsuario2 = @IntUsuario1 ")
                .Append("  set @IntUsuarioData2 = @IntUsuarioData1 ")

                .Append("  fetch next from Integrante_cursor ")
                .Append("    into @IntId1, @IntAcao1, @ResId1, ")
                .Append("    @IntMatricula1, @IntDataIni1, @IntDataIniReal1, @IntDataFim1, ")
                .Append("    @IntDataFimReal1, @CatId1, @IntNome1, @IntDtNascimento1, ")
                .Append("    @IntStatus1, @IntFormaPagamento1, @IntCatCobranca1, ")
                .Append("    @IntMemorando1, @IntEmissor1, @IntCartao1, ")
                .Append("    @IntCortesiaConsumo1, @IntCortesiaResponsavel1, ")
                .Append("    @IntDesjejum1, @IntAlmoco1, @IntJantar1, ")
                .Append("    @IntFechamComSaldoResponsavel1, @IntUsuario1, @IntUsuarioData1 ")


                .Append("  if @IntStatus1 <> @IntStatus2 ")
                .Append("  begin ")
                .Append("    if @IntStatus1 = 'P' and @IntDataIniReal2 is null ")
                .Append("      insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("        values ('Entrada Parque Aquático', @IntUsuario1, ")
                .Append("          convert(char(10), @IntUsuarioData1, 103), ")
                .Append("          convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("    if @IntStatus1 = 'E' and @IntDataIniReal2 is not null ")
                .Append("      insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("        values ('Check-in', @IntUsuario1, ")
                .Append("          convert(char(10), @IntUsuarioData1, 103), ")
                .Append("          convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("    if @IntStatus1 = 'P' and @IntDataIniReal2 is not null ")
                .Append("    begin ")
                .Append("      insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("        values ('Check-out', @IntUsuario1, ")
                .Append("          convert(char(10), @IntUsuarioData1, 103), ")
                .Append("          convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("      insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("        select top 1 'Conferência', 'Governança',  ")
                .Append("          convert(char(10), ApaConDtConferencia, 103), ")
                .Append("          convert(char(5), ApaConDtConferencia, 108) ")
                .Append("          from TbAptoConferencia a ")
                .Append("          inner join TbHospedagem h ")
                .Append("          on a.ApaId = h.ApaId  ")
                .Append("          and h.IntId = @IntId  ")
                .Append("          and h.HosStatus = 'A' ")
                .Append("          and a.ApaConDtSolicitacao > h.HosDataFimReal ")
                .Append("          order by a.ApaConDtSolicitacao ")
                .Append("    end ")
                .Append("    if @IntStatus1 = 'F' ")
                .Append("      insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("        values ('Hóspede Finalizado', @IntUsuario1, ")
                .Append("          convert(char(10), @IntUsuarioData1, 103), ")
                .Append("          convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("  end ")
                .Append("  if @IntCartao1 <> @IntCartao2 ")
                .Append("  begin ")
                .Append("  if @IntStatus1 = '' and @IntCartao1 > '' ")
                .Append("    insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("      values ('Atribuído Cartão', @IntUsuario1, ")
                .Append("          convert(char(10), @IntUsuarioData1, 103), ")
                .Append("          convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("  end ")
                .Append("  if @IntDtNascimento1 <> @IntDtNascimento2 ")
                .Append("insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("      values ('Alteração de Data de Nascimento (' +  ")
                .Append("      convert(char(10), @IntDtNascimento2, 103) + ' para ' + ")
                .Append("      convert(char(10), @IntDtNascimento1, 103) + ')', @IntUsuario1, ")
                .Append("        convert(char(10), @IntUsuarioData1, 103), ")
                .Append("        convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("  if @IntNome1 <> @IntNome2 ")
                .Append("    insert #Log (Evento, Usuario, Data, Horario) ")
                .Append("      values ('Alteração de Nome (' +  ")
                .Append("      rtrim(@IntNome2) + ' para ' + ")
                .Append("      rtrim(@IntNome1) + ')', @IntUsuario1, ")
                .Append("        convert(char(10), @IntUsuarioData1, 103), ")
                .Append("        convert(char(5), @IntUsuarioData1, 108)) ")
                .Append("end ")
                .Append("close Integrante_cursor ")
                .Append("deallocate Integrante_cursor ")

                .Append("select * from #Log order by convert(datetime, Data, 103), convert(datetime, Horario, 108) ")
                .Append("drop table #Log ")
            End With
            Return PreencheListaLogEventosHospedes(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaLogEventosHospedes(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjHistHspInformacoesVO As New HistoricoHospedesInformacoesVO
                With ObjHistHspInformacoesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Evento")) Then
                        .Descricao = ""
                    Else
                        .Descricao = ResultadoConsulta.Item("Evento")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("Usuario")) Then
                        .Operador = ""
                    Else
                        .Operador = ResultadoConsulta.Item("Usuario")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("Data")) Then
                        .Dia = ""
                    Else
                        .Dia = ResultadoConsulta.Item("Data")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("Horario")) Then
                        .Hora = ""
                    Else
                        .Hora = ResultadoConsulta.Item("Horario")
                    End If
                End With
                Lista.Add(ObjHistHspInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ConsultaInformacoesPassante(ByVal ObjHistHspInformacoesVO As HistoricoHospedesInformacoesVO, ByVal AliasBanco As String, ByVal IntId As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Declare ")
                .Append("@IntId numeric(18) ")
                .Append("set @intId = " & IntId & " ")
                .Append("set nocount on ")
                .Append("select  ")
                .Append("  i.IntId, i.IntMatricula, i.IntNome, i.IntRG, i.IntCPF, ")
                .Append("  convert(char(10), i.IntDataIniReal, 103) as DataIni, ")
                .Append("  substring(convert(char(25), i.IntDataIniReal, 120), 12, 5) HoraIni, ")
                .Append("  case IntStatus ")
                .Append("    when 'P' then convert(char(10), i.IntDataFim, 103) ")
                .Append("                  else  convert(char(10), i.IntDataFimReal, 103) ")
                .Append("  end as DataFim, ")
                .Append("  case i.IntStatus ")
                .Append("    when 'P' then substring(convert(char(25), i.IntDataFim, 120), 12, 5) ")
                .Append("                  else  substring(convert(char(25), i.IntDataFimReal, 120), 12, 5) ")
                .Append("  end as HoraFim, ")
                .Append("  i.IntDtNascimento, i.IntSexo, ")
                .Append("  case ")
                .Append("    when i.IntStatus = 'P' then 'Parque Aquático' ")
                .Append("    when i.IntStatus = 'E' then 'Estada' ")
                .Append("    when i.IntStatus = 'F' then 'Finalizado' ")
                .Append("    else ' ' ")
                .Append("  end as Status, i.IntMemorando, ")
                .Append("  case  ")
                .Append("    when i.IntEmissor = '1' then 'FECOMÉRCIO' ")
                .Append("    when i.IntEmissor = '2' then 'DR' ")
                .Append("    when i.IntEmissor = '3' then 'DOS' ")
                .Append("    when i.IntEmissor = '4' then 'DA' ")
                .Append("    when i.IntEmissor = '5' then 'DIFIN' ")
                .Append("    else ' ' ")
                .Append("  end as Emissor, i.IntCartao, abs(i.IntSaldo) as IntSaldo, ")
                .Append("  i.IntCidade, i.IntCortesiaPqAquatico, i.IntCortesiaCaucao, i.IntCortesiaConsumo, ")
                .Append("  i.IntCortesiaRestaurante, i.IntCortesiaPassPermanencia, ")
                .Append("  case ")
                .Append("    when isnull(i.PreIntid,0) = 0 then  i.IntCortesiaResponsavel ")
                '-- P.IntUsuario Trata-se de um passante pré-cadastrado
                .Append("                                               else p.IntUsuario ")
                .Append("  end as IntCortesiaResponsavel, ")
                .Append("  i.IntPlacaVeiculo, i.IntAlmoco, i.IntJantar, ")
                .Append("  e.EstDescricao as UF, c.CatDescricao as Categoria, cc.CatDescricao as CategCobr, ")
                .Append("  i.IntPasComCld as PasComCld, i.IntPasIse as PasIse, co.TEpDes as MotivoCortesia, ")
                .Append("  M.EstSigla,M.MunDescricao, ")
                'Exibe o nome do integrante cuja refeição esta vinculada.
                .Append("case ")
                .Append("when i.IntPasAntAlm = 0 and i.IntVinculoId = 0  then '' ")
                .Append("when i.IntPasAntAlm > 0 then i.IntNome ")
                .Append("when i.IntPasAntAlm = 0 and i.IntVinculoId > 0  then isnull((Select CAST(IntPasAntAlm as varchar) +' Refeições contidas no cartão: '+ intNome from TbIntegrante ii where ii.IntId = i.IntVinculoId and ii.IntPasAntAlm > 0),'') ")
                .Append("else ")
                .Append("(Select intNome from TbIntegrante ii where ii.IntId = i.IntVinculoId) ")
                .Append("end as VinculoRefeicao ")


                '.Append(" case ")
                '.Append("when i.IntPasAntAlm = 0 then '' ") 'Não possui refeição vinculada
                '.Append("when i.IntPasAntAlm > 0 and i.IntVinculoId = 0  then i.IntNome ") 'O proprio integrante possui a refeição no seu cartão
                '.Append("else ")
                '.Append("(Select intNome from TbIntegrante ii where ii.IntId = i.IntVinculoId) ") 'Refeição vinculada a outro integrante
                '.Append("end as VinculoRefeicao")
                .Append("    from ")
                .Append("    TbIntegrante i inner join TbEstadoPais e ")
                .Append("    on i.EstId = e.EstId ")
                .Append("    inner join TbCategoria c ")
                .Append("    on i.CatId = c.Catid ")
                .Append("    inner join TbCategoria cc ")
                .Append("    on i.IntCatCobranca = cc.CatId ")
                .Append("    left join TbMotivoCortesia co ")
                .Append("    on i.TepCod = co.TepCod ")
                .Append("    left join TbPreIntegrante p ")
                .Append("    on i.PreIntid = p.PreIntid ")
                .Append("    left Join [dbgeral].[dbo].TbBdProdMunicipio m ")
                .Append("    on m.MunId = i.MunId ")
                .Append("    where i.IntId = @IntId ")
            End With
            Return PreencheInformacoesPassante(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheInformacoesPassante(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                Dim ObjHistPstInformacoesVO As New HistoricoPassantesInformacoesVO
                With ObjHistPstInformacoesVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntNome")) Then
                        .IntNome = ""
                    Else
                        .IntNome = ResultadoConsulta.Item("IntNome")
                    End If
                    If (ResultadoConsulta.Item("IntMatricula")) = Nothing Then
                        .Matricula = " "
                    Else
                        .Matricula = ResultadoConsulta.Item("IntMatricula")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntRG")) Or ResultadoConsulta.Item("IntRG") = Nothing Then
                        .RG = " "
                    Else
                        .RG = ResultadoConsulta.Item("IntRG")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCPF")) Then
                        .CPF = ""
                    Else
                        .CPF = ResultadoConsulta.Item("IntCPF")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataIni")) Then
                        .DataIniReal = ""
                    Else
                        .DataIniReal = ResultadoConsulta.Item("DataIni")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraIni")) Then
                        .HoraIniReal = ""
                    Else
                        .HoraIniReal = ResultadoConsulta.Item("HoraIni")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DataFim")) Then
                        .DataFimReal = ""
                    Else
                        .DataFimReal = ResultadoConsulta.Item("DataFim")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("HoraFim")) Then
                        .HoraFimReal = ""
                    Else
                        .HoraFimReal = ResultadoConsulta.Item("HoraFim")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Categoria")) Then
                        .Categoria = ""
                    Else
                        .Categoria = ResultadoConsulta.Item("Categoria")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("PasComCld")) Then
                        .ComerciarioCaldasNovas = ""
                    Else
                        .ComerciarioCaldasNovas = ResultadoConsulta.Item("PasComCld")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("PasIse")) Then
                        .Isento = ""
                    Else
                        .Isento = ResultadoConsulta.Item("PasIse")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaCaucao")) Then
                        .CortesiaCaucao = ""
                    Else
                        .CortesiaCaucao = ResultadoConsulta.Item("IntCortesiaCaucao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaPqAquatico")) Then
                        .CortesiaPqAquatico = ""
                    Else
                        .CortesiaPqAquatico = ResultadoConsulta.Item("IntCortesiaPqAquatico")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaConsumo")) Then
                        .CortesiaLanchonetes = ""
                    Else
                        .CortesiaLanchonetes = ResultadoConsulta.Item("IntCortesiaConsumo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaPassPermanencia")) Then
                        .CortesiaPermanenciaPQ = ""
                    Else
                        .CortesiaPermanenciaPQ = ResultadoConsulta.Item("IntCortesiaPassPermanencia")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaRestaurante")) Then
                        .CortesiaRestaurante = ""
                    Else
                        .CortesiaRestaurante = ResultadoConsulta.Item("IntCortesiaRestaurante")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntCortesiaResponsavel")) Or ResultadoConsulta.Item("IntCortesiaResponsavel") = Nothing Then
                        .ResponsavelCortesia = " "
                    Else
                        .ResponsavelCortesia = ResultadoConsulta.Item("IntCortesiaResponsavel")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CategCobr")) Then
                        .CategoriaCobranca = ""
                    Else
                        .CategoriaCobranca = ResultadoConsulta.Item("CategCobr")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntMemorando")) Or ResultadoConsulta.Item("IntMemorando") = Nothing Then
                        .DocMemorando = " "
                    Else
                        .DocMemorando = ResultadoConsulta.Item("IntMemorando")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("MotivoCortesia")) Then
                        .MotivoEmissor = " "
                    Else
                        .MotivoEmissor = ResultadoConsulta.Item("MotivoCortesia")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Status")) Then
                        .Situacao = " "
                    Else
                        .Situacao = ResultadoConsulta.Item("Status")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("IntPlacaVeiculo")) Then
                        .Placa = " "
                    Else
                        .Placa = ResultadoConsulta.Item("IntPlacaVeiculo")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("EstSigla")) Then
                        .UF = " "
                    Else
                        .UF = ResultadoConsulta.Item("EstSigla")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("MunDescricao")) Then
                        .Cidade = " "
                    Else
                        .Cidade = ResultadoConsulta.Item("MunDescricao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("VinculoRefeicao")) Then
                        .VinculoRefeicao = " "
                    Else
                        .VinculoRefeicao = ResultadoConsulta.Item("VinculoRefeicao")
                    End If
                End With
                Lista.Add(ObjHistPstInformacoesVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
