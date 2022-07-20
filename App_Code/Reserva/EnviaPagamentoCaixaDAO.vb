
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Public Class EnviaPagamentoCaixaDAO
    Dim ObjEnviaPagamentoCaixaVO As EnviaPagamentoCaixaVO
    Dim ObjEnviaPagamentoCaixaDAO As EnviaPagamentoCaixaDAO
    Dim VarSql As Text.StringBuilder
    Dim Conn As Banco.Conexao

    Public Sub New(ByVal NomeConexao As String)
        Conn = New Banco.Conexao(NomeConexao)
    End Sub

    Public Function ConsultaValoresParaEnvioCaixa(ByVal ObjEnviaPagamentoCaixaVO As EnviaPagamentoCaixaVO, Percentual As Integer, Banco As String) As EnviaPagamentoCaixaVO
        Try
            VarSql = New Text.StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("Declare @Reserva numeric, @Usuario varchar(60), @Percentual numeric ")
                .AppendLine("set @Usuario = '" & ObjEnviaPagamentoCaixaVO.Usuario & "' ")
                .AppendLine("set @Reserva = '" & ObjEnviaPagamentoCaixaVO.ResId & "' ")
                .AppendLine("set @Percentual = " & Percentual & " ")
                .AppendLine("SET XACT_ABORT ON ")
                .AppendLine("begin tran ")
                .AppendLine("Create table #TbAux( ")
                .AppendLine("   Operador varchar(30), ")
                .AppendLine("   Data varchar(30), ")
                .AppendLine("   Usuario varchar(30), ")
                .AppendLine("   Reserva varchar(10), ")
                .AppendLine("   Percentual decimal(18,2), ")
                .AppendLine("   ResCaracteristica varchar(10), ")
                .AppendLine("   ResNome varchar(100), ")
                .AppendLine("   ResDataIni varchar(20), ")
                .AppendLine("   ResDataFim varchar(20), ")
                .AppendLine("   VlrVen Decimal(18,2), ")
                .AppendLine("   VlrVenPg Decimal(18,2), ")
                .AppendLine("   VlrH Decimal(18,2), ")
                .AppendLine("   VlrD decimal(18,2), ")
                .AppendLine("   VlrR decimal(18,2), ")
                .AppendLine("   VlrHPago decimal(18,2), ")
                .AppendLine("   VlrDPago decimal(18,2), ")
                .AppendLine("   VlrRPago decimal(18,2)) ")

                .AppendLine("declare ")
                .AppendLine("  @VlrVen numeric (18,2), ")
                .AppendLine("  @VlrVenPg numeric (18,2), ")
                .AppendLine("  @VlrEncaminhado numeric (18,2), ")
                .AppendLine("  @Erro integer, ")
                .AppendLine("  @VlrD numeric(18,2), ")
                .AppendLine("  @VlrH numeric(18,2), ")
                .AppendLine("  @VlrR numeric(18,2), ")
                .AppendLine("  @VlrDPago numeric(18,2), ")
                .AppendLine("  @VlrHPago numeric(18,2), ")
                .AppendLine("  @VlrRPago numeric(18,2), ")
                .AppendLine("  @Data DateTime, ")
                .AppendLine("  @ResCaracteristica char(1), ")
                .AppendLine("  @Operador char(60), ")
                .AppendLine("  @ProId char(1), ")
                .AppendLine("  @ProDescricao varchar(150), ")
                .AppendLine("  @Banco varchar(50), ")
                .AppendLine("  @Origem char(1), ")
                .AppendLine("  @VlrAux varchar(20), ")
                .AppendLine("  @ResNome varchar(100), ")
                .AppendLine("  @ResDataIni char(15), ")
                .AppendLine("  @ResDataFim char(15) ")

                .AppendLine("  set @Banco = '' ")
                .AppendLine("set @Operador = replace(@Usuario,'SESC-GO.COM.BR\','') ")
                .AppendLine("set @Erro = 0 ")
                .AppendLine("set @Data = getdate() ")
                .AppendLine("if exists(select top 1 1 from tbreserva r ")
                .AppendLine("  inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("  left join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("  where convert(char(10),r.ResdataIni) = convert(char(10),r.ResDatafim) and r.resid =  @Reserva ) ")
                .AppendLine("    Begin ")
                .AppendLine("       select top 1 ")
                .AppendLine("          @ResDataIni = convert(char(10),isnull(h.HosDataIniSol,r.ResDataIni), 103), ")
                .AppendLine("          @ResDataFim = convert(char(10), r.ResDataFim, 103) ")
                .AppendLine("       from tbreserva r ")
                .AppendLine("       inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("       left join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("       where r.resid =  @Reserva ")
                .AppendLine("    end ")
                .AppendLine("  else ")
                .AppendLine("    begin ")
                .AppendLine("      select top 1 ")
                .AppendLine("          @ResDataIni = convert(char(10),isnull(h.HosDataIniSol,r.ResDataIni), 103) + ' 14 h', ")
                .AppendLine("          @ResDataFim = convert(char(10), r.ResDataFim, 103) + ' 10 h' ")
                .AppendLine("       from tbreserva r ")
                .AppendLine("       inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("       left join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("       where r.resid =  @Reserva ")
                .AppendLine("end ")
                .AppendLine("select top 1 ")
                .AppendLine("  @ResCaracteristica = r.ResCaracteristica, ")
                .AppendLine("  @ResNome = r.ResNome ")
                .AppendLine("  from TbReserva r ")
                .AppendLine("  inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("  left join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("  where r.ResId = @Reserva ")
                .AppendLine("  order by CONVERT(datetime,h.HosUsuarioData,103) desc ")

                .AppendLine("select @VlrVen = isnull(sum(VenValor),0) / 100 * @Percentual from TbVencimento where ResId = @Reserva ")
                '.AppendLine("  and VenStatus = 'V' and VenFormaPagto in ('ER','EC') ")
                'Adicionei a forma de pagamento C devido a reservas de cortesia
                .AppendLine("  and VenStatus = 'V' and VenFormaPagto in ('ER','EC','C') ")
                .AppendLine("set @VlrVenPg = isnull((select sum(VenValor) from TbVencimento where ResId = @Reserva ")
                .AppendLine("  and VenStatus in ('M','C','B','T','A')),0) ")
                .AppendLine("set @VlrEncaminhado = isnull((select sum(VenValor) from TbVencimento where ResId = @Reserva ")
                .AppendLine(" and VenStatus = 'E'),0) ")
                .AppendLine(" set @VlrVenPg = @VlrVenPg - @VlrEncaminhado ")

                'Quando houver cortesia considerar o venstatus V e forma de pagamento C como pago
                .AppendLine("set @VlrVenPg = @VlrVenPg + isnull((select sum(VenValor) from TbVencimento where ResId = @Reserva ")
                .AppendLine(" and VenStatus in ('V') and VenFormaPagto = 'C'),0) ")

                .AppendLine("begin ")

                .AppendLine("  --Calcula percentagem Desjejum por Washington 18/12/2017 ")
                .AppendLine("   Declare @PercenteD Decimal(18,2), @PercenteR Decimal(18,2), @PercenteH Decimal(18,2), ")
                .AppendLine("           @VlrAReceber Decimal (18,2), @vlrRecebido Decimal(18,2) ")

                .AppendLine("   set @VlrAReceber = @VlrVen - @VlrVenPg ")
                .AppendLine("   set @vlrRecebido = @VlrVenPg ")

                .AppendLine("   Set @PercenteD = isnull((select sum(r.RatValor) ")
                .AppendLine("   from TbRateio r ")
                .AppendLine("   inner join TbVencimento v on r.VenId = v.VenId ")
                .AppendLine("   and v.ResId = @Reserva and v.VenStatus = 'V' ")
                .AppendLine("   and v.VenFormaPagto in ('ER','EC') and r.RatTipo = 'D'), 0) / @VlrVen--Pg ")

                .AppendLine("	Set @PercenteR = isnull((select sum(r.RatValor) ")
                .AppendLine("    from TbRateio r ")
                .AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                .AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                .AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo in ('A','J')), 0) / @VlrVen--Pg ")

                .AppendLine("	Set @PercenteH = isnull((select sum(r.RatValor) ")
                .AppendLine("    from TbRateio r ")
                .AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                .AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                .AppendLine("    and r.RatTipo in ('H','P')), 0) / @VlrVen--Pg ")

                '.AppendLine("   --Calculando o rateio do valor que estou Recebendo ")
                .AppendLine("   set @VlrD = isnull(@VlrAReceber * @PercenteD,0) ")
                .AppendLine("   set @VlrR = isnull(@VlrAReceber * @PercenteR, 0) ")
                .AppendLine("   Set @VlrH = isnull(@VlrAReceber - (@VlrD + @VlrR),0) --isnull(@VlrAReceber * @PercenteH, 0) ")

                .AppendLine("   --Calculo o rateio do valor já pago ")
                .AppendLine("   set @VlrDPago  = isnull(@vlrRecebido * @PercenteD,0) ")
                .AppendLine("   set @VlrRPago = isnull(@vlrRecebido * @PercenteR, 0) ")
                .AppendLine("   Set @VlrHPago = isnull(@vlrRecebido - (@VlrDPago + @VlrRPago),0)--isnull(@vlrRecebido * @PercenteH, 0) ")

                'Encontrando as Quantidade de rateio para calcular quando for meia diária sem pernoite
                .AppendLine("   Declare @RateioQtdeD integer, @RateioQtdeA integer, @RateioQtdeP integer,@SomaRefeicoesExtras Decimal(18,2) ")
                .AppendLine("Set @RateioQtdeD = isnull((select sum(r.RatQtde) from TbRateio r inner join TbVencimento v on r.VenId = v.VenId where v.VenStatus = 'V' and r.RatTipo = 'D' and v.ResId = @Reserva),0) ")
                .AppendLine("Set @RateioQtdeA = isnull((select sum(r.RatQtde) from TbRateio r inner join TbVencimento v on r.VenId = v.VenId where v.VenStatus = 'V' and r.RatTipo = 'A' and v.ResId = @Reserva),0) ")
                .AppendLine("Set @RateioQtdeP = isnull((select sum(r.RatQtde) from TbRateio r inner join TbVencimento v on r.VenId = v.VenId where v.VenStatus = 'V' and r.RatTipo = 'P' and v.ResId = @Reserva),0) ")
                .AppendLine("Set @SomaRefeicoesExtras = isnull((select sum(1) from TbHospedagem h inner join TbIntegrante i on h.IntId = i.IntId where i.ResId = @Reserva ")
                .AppendLine("    and i.IntPasAntAlm > 0) * (select vv.ValValor  from TbValor vv where vv.ValTipo = 'AG' AND vv.CatId = 1),0) ")

                'Se não existir valor pago e existir valor devido, já busco direto o valor rateado pelo sistema em tbrateio, caso contrario farei o calculo em % 
                .AppendLine("if (@VlrDPago = 0 and @VlrD > 0) ")
                .AppendLine("	Begin ")
                .AppendLine("		Set @VlrD = (select SUM(r.RatValor) from TbRateio r inner join TbVencimento v on v.VenId = r.VenId and r.RatTipo = 'D' and v.ResId = @Reserva) ")
                .AppendLine("	End ")
                .AppendLine("else ")
                .AppendLine("	Begin ")
                .AppendLine("		Set @VlrD = isnull(@VlrAReceber * @PercenteD,0) ")
                .AppendLine("   End ")

                .AppendLine("if (@VlrRPago = 0 and @VlrR > 0) ")
                .AppendLine("	Begin ")
                .AppendLine("		Set @VlrR = (select SUM(r.RatValor) from TbRateio r inner join TbVencimento v on v.VenId = r.VenId and r.RatTipo in ('A','J') and v.ResId = @Reserva) ")
                .AppendLine("   end ")
                .AppendLine("else ")
                .AppendLine("	Begin ")
                .AppendLine("		Set @VlrR = isnull(@VlrAReceber * @PercenteR, 0) ")
                .AppendLine("End ")

                'Para evitar erros de rateio, a Hospedagem sempre será o valor a receber - a soma de Desjejum e Refeição
                .AppendLine("Set @VlrH = isnull(@VlrAReceber - (@VlrD + @VlrR),0) --isnull(@VlrAReceber * @PercenteH, 0) ")

                'Essa foi uma formula encontrada para que a refeição extra não seja rateada, ou seja, ela será inserida por inteiro como Refeição.
                'Problema encontrado, insere um novo integrante e já marca a refeição, então o sistema não reconhece a ação e irá ratear a até a refeição extra, quando fo só a refeição extra o rateio será feito diretamente para Refeição(A refeição não terá rateio)
                .AppendLine("   if ((@RateioQtdeA > @RateioQtdeD) ")
                .AppendLine("      And (@RateioQtdeA - @RateioQtdeP) = @RateioQtdeD  --Teve venda de meia diária sem pernoite e que ainda não foi Rateada ")
                .AppendLine("      And (@VlrVen - @VlrVenPg) <= @SomaRefeicoesExtras)-- Fiz uma média, quantidade de pessoas que adquiriram refeição extra na hospedagem * valor da refeição ")
                .AppendLine("         Begin ")
                .AppendLine("            set @VlrD = 0 ")
                .AppendLine("            set @VlrR = (@VlrVen - @VlrVenPg) ")
                .AppendLine("            set @VlrH = 0 ")
                .AppendLine("         End ")
                .AppendLine("end ")

                'De onde sairá o resultado final para envio ao caixa
                .AppendLine("insert #TbAux ")
                .AppendLine("Select @Operador as Operador ,@Data as Data,@Usuario as Usuario,@Reserva as Reserva, ")
                .AppendLine("@Percentual as Percentual,@ResCaracteristica as ResCaracteristica,@ResNome as ResNome,@ResDataIni as ResDataIni,@ResDataFim as ResDataFim, ")
                .AppendLine("@VlrVen as VlrVen,@VlrVenPg as VlrVenPg,@VlrH as VlrH,@VlrD as VlrD,@VlrR as VlrR,@VlrHPago as VlrHPago,@VlrDPago as VlrDPago,@VlrRPago as VlrRPago ")
                .AppendLine("select * from #TbAux ")
                .AppendLine("drop Table #TbAux ")

                '.AppendLine("Declare ")
                '.AppendLine("@Reserva numeric, @Usuario varchar(60), @Percentual numeric ")

                '.AppendLine("set @Usuario = '" & ObjEnviaPagamentoCaixaVO.Usuario & "' ")
                '.AppendLine("set @Reserva = '" & ObjEnviaPagamentoCaixaVO.ResId & "' ")
                '.AppendLine("set @Percentual = " & Percentual & " ")
                ''executa transações distribuídas ")
                '.AppendLine("SET XACT_ABORT ON  ")

                '.AppendLine("begin tran ")

                ''Tabela  auxiliar, iremos usar os valores dela na hora de salvar os dados no caixa.
                '.AppendLine("Create table #TbAux( ")
                '.AppendLine("   Operador varchar(30), ")
                '.AppendLine("   Data varchar(30), ")
                '.AppendLine("   Usuario varchar(30), ")
                '.AppendLine("   Reserva varchar(10), ")
                '.AppendLine("   Percentual decimal(18,2), ")
                '.AppendLine("   ResCaracteristica varchar(10), ")
                '.AppendLine("   ResNome varchar(100), ")
                '.AppendLine("   ResDataIni varchar(20), ")
                '.AppendLine("   ResDataFim varchar(20), ")
                '.AppendLine("   VlrVen Decimal(18,2), ")
                '.AppendLine("   VlrVenPg Decimal(18,2), ")
                '.AppendLine("   VlrH Decimal(18,2), ")
                '.AppendLine("   VlrD decimal(18,2), ")
                '.AppendLine("   VlrR decimal(18,2), ")
                '.AppendLine("   VlrHPago decimal(18,2), ")
                '.AppendLine("   VlrDPago decimal(18,2), ")
                '.AppendLine("   VlrRPago decimal(18,2) ")
                '.AppendLine(") ")

                '.AppendLine("declare  ")
                '.AppendLine("  @VlrVen numeric (18,2), ")
                '.AppendLine("  @VlrVenPg numeric (18,2), ")
                '.AppendLine("  @Erro integer, ")
                '.AppendLine("  @VlrD numeric(18,2), ")
                '.AppendLine("  @VlrH numeric(18,2), ")
                '.AppendLine("  @VlrR numeric(18,2), ")
                ''@VlrP	numeric(18,2), ")
                '.AppendLine("  @VlrDPago numeric(18,2), ")
                '.AppendLine("  @VlrHPago numeric(18,2), ")
                '.AppendLine("  @VlrRPago numeric(18,2), ")
                ''@VlrPPago numeric(18,2), ")
                '.AppendLine("  @Data DateTime, ")
                '.AppendLine("  @ResCaracteristica char(1), ")
                '.AppendLine("  @Operador char(60), ")
                '.AppendLine("  @ProId char(1), ")
                '.AppendLine("  @ProDescricao varchar(150), ")
                '.AppendLine("  @Banco varchar(50), ")
                '.AppendLine("  @Origem char(1), ")
                '.AppendLine("  @VlrAux varchar(20), ")
                '.AppendLine("  @ResNome varchar(100), ")
                '.AppendLine("  @ResDataIni char(15), ")
                '.AppendLine("  @ResDataFim char(15) ")

                '.AppendLine("set @Banco = '' ")

                '.AppendLine("set @Operador = replace(@Usuario,'SESC-GO.COM.BR\','') ")
                '.AppendLine("set @Erro = 0 ")
                '.AppendLine("set @Data = getdate() ")

                ''Setano as variaveis, se a data inicial da reserva for diferente da data final será adicionado as horas 
                '.AppendLine("if exists(select top 1 1 from tbreserva r ")
                '.AppendLine("  inner join TbIntegrante i on i.ResId = r.ResId ")
                '.AppendLine("  left join TbHospedagem h on h.IntId = i.IntId ")
                '.AppendLine("  where convert(char(10),r.ResdataIni) = convert(char(10),r.ResDatafim) and r.resid =  @Reserva ) ")
                '.AppendLine("    Begin ")
                '.AppendLine("       select top 1 ")
                '.AppendLine("          @ResDataIni = convert(char(10),isnull(h.HosDataIniSol,r.ResDataIni), 103), ")
                '.AppendLine("          @ResDataFim = convert(char(10), r.ResDataFim, 103) ")
                '.AppendLine("       from tbreserva r ")
                '.AppendLine("       inner join TbIntegrante i on i.ResId = r.ResId ")
                '.AppendLine("       left join TbHospedagem h on h.IntId = i.IntId ")
                '.AppendLine("       where r.resid =  @Reserva ")
                '.AppendLine("    end ")
                '.AppendLine("  else ")
                '.AppendLine("    begin ")
                '.AppendLine("      select top 1 ")
                '.AppendLine("          @ResDataIni = convert(char(10),isnull(h.HosDataIniSol,r.ResDataIni), 103) + ' 14 h', ")
                '.AppendLine("          @ResDataFim = convert(char(10), r.ResDataFim, 103) + ' 10 h' ")
                '.AppendLine("       from tbreserva r ")
                '.AppendLine("       inner join TbIntegrante i on i.ResId = r.ResId ")
                '.AppendLine("       left join TbHospedagem h on h.IntId = i.IntId ")
                '.AppendLine("       where r.resid =  @Reserva ")
                '.AppendLine("end ")

                ''Alimentando as variáveis
                '.AppendLine("select top 1 ")
                '.AppendLine("  @ResCaracteristica = r.ResCaracteristica, ")
                '.AppendLine("  @ResNome = r.ResNome ")
                ''Essas variáveis foram alimentadas logo acima
                ''.AppendLine("  @ResDataIni = convert(char(10), h.HosDataIniSol, 103) + ' 14 h', ")
                ''.AppendLine("  @ResDataFim = convert(char(10), r.ResDataFim, 103) + ' 10 h' ")
                '.AppendLine("  from TbReserva r ")
                '.AppendLine("  inner join TbIntegrante i on i.ResId = r.ResId ")
                '.AppendLine("  left join TbHospedagem h on h.IntId = i.IntId ")
                '.AppendLine("  where r.ResId = @Reserva ")
                '.AppendLine("  order by CONVERT(datetime,h.HosUsuarioData,103) desc ")

                '.AppendLine("select @VlrVen = isnull(sum(VenValor),0) / 100 * @Percentual from TbVencimento where ResId = @Reserva ")
                '.AppendLine("  and VenStatus = 'V' and VenFormaPagto in ('ER','EC') ")
                '.AppendLine("set @VlrVenPg = isnull((select sum(VenValor) from TbVencimento where ResId = @Reserva ")
                '.AppendLine("  and VenStatus in ('M','C','B','T','A')),0) ")
                '.AppendLine("begin ")

                ''Alterado por Washington em 10/11/2017
                '.AppendLine("  set @VlrD = isnull((select sum(r.RatValor) ")
                '.AppendLine("    from TbRateio r ")
                '.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                '.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                '.AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo = 'D'), 0)  * (@VlrVenPg / @VlrVen) ")

                '.AppendLine("  set @VlrR = isnull((select sum(r.RatValor) ")
                '.AppendLine("    from TbRateio r ")
                '.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                '.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                '.AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo in ('A','J')), 0) * (@VlrVenPg / @VlrVen) ")

                '.AppendLine("set @VlrH = (@VlrVenPg - (@VlrD + @VlrR))  ")

                ''.AppendLine("set @VlrH = ((@VlrVen - @VlrVenPg) - (@VlrD + @VlrR))  ")

                ''.AppendLine("  set @VlrH = isnull((select sum(r.RatValor) ")
                ''.AppendLine("    from TbRateio r ")
                ''.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                ''.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                ''.AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo in ('H','P')), 0) / 100 * @Percentual ")

                ''.AppendLine("  set @VlrD = isnull((select sum(r.RatValor) ")
                ''.AppendLine("    from TbRateio r ")
                ''.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                ''.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                ''.AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo = 'D'), 0) / 100 * @Percentual ")

                ''.AppendLine("  set @VlrR = isnull((select sum(r.RatValor) ")
                ''.AppendLine("    from TbRateio r ")
                ''.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                ''.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'V' ")
                ''.AppendLine("    and v.VenFormaPagto in ('ER','EC') and r.RatTipo in ('A','J')), 0) / 100 * @Percentual ")

                '.AppendLine("  set @VlrHPago = isnull((select sum(r.RatValor) ")
                '.AppendLine("    from TbRateio r ")
                '.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                '.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'C' ")
                '.AppendLine("    and r.RatTipo in ('H','P')), 0) ")

                '.AppendLine("  set @VlrDPago = isnull((select sum(r.RatValor) ")
                '.AppendLine("    from TbRateio r  ")
                '.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                '.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'C' ")
                '.AppendLine("    and r.RatTipo = 'D'), 0) ")

                '.AppendLine("  set @VlrRPago = isnull((select sum(r.RatValor) ")
                '.AppendLine("    from TbRateio r  ")
                '.AppendLine("    inner join TbVencimento v on r.VenId = v.VenId ")
                '.AppendLine("    and v.ResId = @Reserva and v.VenStatus = 'C' ")
                '.AppendLine("    and r.RatTipo = 'R'), 0) ")



                ' '' Incluido Haas 30/08/2013
                ''.AppendLine("if (exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'B') and ")
                ''.AppendLine("  exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'C')) or  ")
                ''.AppendLine("  (exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'M') and  ")
                ''.AppendLine("  exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'C')) or ")
                ''.AppendLine("  (exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'T') and  ")
                ''.AppendLine("  exists (select 1 from TbVencimento where ResId = @Reserva and VenStatus = 'C')) ")


                ''.AppendLine("begin ")
                ''.AppendLine("  set @VlrH = @VlrH + @VlrHPago ")
                ''.AppendLine("  set @VlrD = @VlrD + @VlrDPago ")
                ''.AppendLine("  set @VlrR = @VlrR + @VlrRPago ")
                ''.AppendLine("end ")
                ''.AppendLine("else ")
                ''.AppendLine("begin ")
                ''.AppendLine("  set @VlrH = @VlrH ") '@VlrHPago
                ''.AppendLine("  set @VlrD = @VlrD ") ' @VlrDPago
                ''.AppendLine("  set @VlrR = @VlrR ") ' @VlrRPago
                ''.AppendLine("end ")

                ' '' Incluido Haas 30/08/2013
                ''.AppendLine("  if @VlrVenPg > (@VlrHPago + @VlrDPago + @VlrRPago) ") '+ @VlrPPago)
                ''.AppendLine("  begin ")
                ''.AppendLine("    set @VlrHPago = @VlrHPago + isnull((select sum(VenValor) from TbVencimento where ")
                ''.AppendLine("      ResId = @Reserva and VenStatus in ('M','B','C','T')),0) ")
                ''.AppendLine("    if @VlrHPago > @VlrH ")
                ''.AppendLine("    begin ")
                ''.AppendLine("      set @VlrDPago = @VlrDPago + (@VlrHPago - @VlrH) ")
                ''.AppendLine("      set @VlrHPago = @VlrH ")
                ''.AppendLine("      if @VlrDPago > @VlrD ")
                ''.AppendLine("      begin ")
                ''.AppendLine("        set @VlrRPago = @VlrRPago + (@VlrDPago - @VlrD) ")
                ''.AppendLine("        set @VlrDPago = @VlrD ")
                ''.AppendLine("      end ")
                ''.AppendLine("      if @VlrRPago > @VlrR ")
                ''.AppendLine("      begin ")
                ''.AppendLine("        set @VlrRPago = @VlrR ")
                ''.AppendLine("      end ")
                ''.AppendLine("    end ")
                ''.AppendLine("  end ")

                ''.AppendLine("  if @VlrD < @VlrDPago ")
                ''.AppendLine("  begin ")
                ''.AppendLine("    set @VlrH = @VlrH + (@VlrDPago - @VlrD) ")
                ''.AppendLine("    set @VlrD = 0 ")
                ''.AppendLine("  end ")
                ''.AppendLine("  if @VlrR < @VlrRPago  ")
                ''.AppendLine("  begin ")
                ''.AppendLine("    set @VlrH = @VlrH + (@VlrRPago - @VlrR) ")
                ''.AppendLine("    set @VlrR = 0 ")
                ''.AppendLine("  end ")

                '.AppendLine("end ")
                ''.AppendLine("select top 1 @Banco = proDescricao,@Origem = ProId from TbProcedencia  ")

                '.AppendLine("insert #TbAux ")
                '.AppendLine("Select @Operador as Operador ,@Data as Data,@Usuario as Usuario,@Reserva as Reserva, ")
                '.AppendLine("@Percentual as Percentual,@ResCaracteristica as ResCaracteristica,@ResNome as ResNome,@ResDataIni as ResDataIni,@ResDataFim as ResDataFim,  ")
                '.AppendLine("@VlrVen as VlrVen,@VlrVenPg as VlrVenPg,@VlrH as VlrH,@VlrD as VlrD,@VlrR as VlrR,@VlrHPago as VlrHPago,@VlrDPago as VlrDPago,@VlrRPago as VlrRPago  ")
                '.AppendLine("select * from #TbAux ")
                '.AppendLine("drop Table #TbAux ")
            End With
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ResultadoConsulta As SqlDataReader) As EnviaPagamentoCaixaVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjEnviaPagamentoCaixaVO = New EnviaPagamentoCaixaVO
            With ObjEnviaPagamentoCaixaVO
                .Operador = ResultadoConsulta.Item("Operador")
                .Data = Format(CDate(ResultadoConsulta.Item("Data")), "yyy-MM-dd hh:mm:ss")
                .ResCaracteristica = ResultadoConsulta.Item("ResCaracteristica")
                .ResNome = ResultadoConsulta.Item("ResNome")
                .ResDataIni = ResultadoConsulta.Item("ResDataIni").ToString.Trim
                .ResDataFim = ResultadoConsulta.Item("ResDataFim").ToString.Trim
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrVen")) Then
                    .VlrVen = 0
                Else
                    .VlrVen = ResultadoConsulta.Item("VlrVen")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrVenPg")) Then
                    .VlrVenPg = 0
                Else
                    .VlrVenPg = ResultadoConsulta.Item("VlrVenPg")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrH")) Then
                    .VlrH = 0
                Else
                    .VlrH = ResultadoConsulta.Item("VlrH")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrD")) Then
                    .VlrD = 0
                Else
                    .VlrD = ResultadoConsulta.Item("VlrD")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrR")) Then
                    .VlrR = 0
                Else
                    .VlrR = ResultadoConsulta.Item("VlrR")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrHPago")) Then
                    .VlrHPago = 0
                Else
                    .VlrHPago = ResultadoConsulta.Item("VlrHPago")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrDPago")) Then
                    .VlrDPago = 0
                Else
                    .VlrDPago = ResultadoConsulta.Item("VlrDPago")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("VlrRPago")) Then
                    .VlrRPago = 0
                Else
                    .VlrRPago = ResultadoConsulta.Item("VlrRPago")
                End If
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjEnviaPagamentoCaixaVO
    End Function



    Public Function EnviaPagtoCaixa(ByVal ObjEnviaPagamentoCaixaVO As EnviaPagamentoCaixaVO, Percentual As Integer, Banco As String) As Long
        Try
            VarSql = New Text.StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("Declare ")
                .AppendLine("@Reserva numeric, @Usuario varchar(60), @Percentual numeric ")

                .AppendLine("set @Usuario = '" & ObjEnviaPagamentoCaixaVO.Usuario & "' ")
                .AppendLine("set @Reserva = '" & ObjEnviaPagamentoCaixaVO.ResId & "' ")
                .AppendLine("set @Percentual = " & Percentual & " ")
                'executa transações distribuídas ")
                .AppendLine("SET XACT_ABORT ON  ")
                .AppendLine("begin tran ")
                .AppendLine("declare  ")
                .AppendLine("  @VlrVen numeric (18,2), ")
                .AppendLine("  @VlrVenPg numeric (18,2), ")
                .AppendLine("  @Erro integer, ")
                .AppendLine("  @VlrD numeric(18,2), ")
                .AppendLine("  @VlrH numeric(18,2), ")
                .AppendLine("  @VlrR numeric(18,2), ")
                '@VlrP	numeric(18,2), ")
                .AppendLine("  @VlrDPago numeric(18,2), ")
                .AppendLine("  @VlrHPago numeric(18,2), ")
                .AppendLine("  @VlrRPago numeric(18,2), ")
                '@VlrPPago numeric(18,2), ")
                .AppendLine("  @Data DateTime, ")
                .AppendLine("  @ResCaracteristica char(1), ")
                .AppendLine("  @Operador char(60), ")
                .AppendLine("  @ProId char(1), ")
                .AppendLine("  @ProDescricao varchar(50), ")
                .AppendLine("  @Banco varchar(50), ")
                .AppendLine("  @Origem char(1), ")
                .AppendLine("  @VlrAux varchar(20), ")
                .AppendLine("  @ResNome varchar(100), ")
                .AppendLine("  @ResDataIni char(15), ")
                .AppendLine("  @ResDataFim char(15) ")

                'Setando os variáveis
                .AppendLine(" set @VlrVen = " & ObjEnviaPagamentoCaixaVO.VlrVen.Replace(",", ".") & " ")
                .AppendLine(" set @VlrVenPg = " & ObjEnviaPagamentoCaixaVO.VlrVenPg.Replace(",", ".") & " ")
                .AppendLine(" set @VlrD = " & ObjEnviaPagamentoCaixaVO.VlrD.Replace(",", ".") & " ")
                .AppendLine(" set @VlrH = " & ObjEnviaPagamentoCaixaVO.VlrH.Replace(",", ".") & " ")
                .AppendLine(" set @VlrR = " & ObjEnviaPagamentoCaixaVO.VlrR.Replace(",", ".") & " ")
                '@VlrP	numeric(18,2), ")
                .AppendLine(" set @VlrDPago = " & ObjEnviaPagamentoCaixaVO.VlrDPago.Replace(",", ".") & " ")
                .AppendLine(" set @VlrHPago = " & ObjEnviaPagamentoCaixaVO.VlrHPago.Replace(",", ".") & " ")
                .AppendLine(" set @VlrRPago = " & ObjEnviaPagamentoCaixaVO.VlrRPago.Replace(",", ".") & " ")
                '@VlrPPago numeric(18,2), ")
                .AppendLine(" set @Data = '" & ObjEnviaPagamentoCaixaVO.Data & "' ")
                .AppendLine(" set @Erro = 0 ")
                .AppendLine(" set @ResCaracteristica = '" & ObjEnviaPagamentoCaixaVO.ResCaracteristica & "' ")
                .AppendLine(" set @Operador = '" & ObjEnviaPagamentoCaixaVO.Operador & "' ")
                .AppendLine(" set @ProId = '" & ObjEnviaPagamentoCaixaVO.ProId & "' ")
                .AppendLine(" set @ProDescricao = '" & ObjEnviaPagamentoCaixaVO.PpcDescricao & "' ")
                .AppendLine(" set @Origem = '" & ObjEnviaPagamentoCaixaVO.ProId & "' ")
                '.AppendLine(" set @VlrAux varchar(20), ")
                .AppendLine(" set @ResNome = '" & ObjEnviaPagamentoCaixaVO.ResNome & "' ")
                .AppendLine(" set @ResDataIni = '" & ObjEnviaPagamentoCaixaVO.ResDataIni & "' ")
                .AppendLine(" set @ResDataFim = '" & ObjEnviaPagamentoCaixaVO.ResDataFim & "' ")

                .AppendLine("set @Banco = '' ")

                .AppendLine("set @banco = '" & Banco & "'  ") 'O banco onde será gravado os dados no caixa

                .AppendLine("if '" & Banco & "' > '' ")
                .AppendLine("begin ")
                .AppendLine("  exec ('delete TbPgtoPendenteCaixa where ResId = ' + @Reserva + ' and PpcOrigem = ''VND''') ")
                .AppendLine("  begin ")
                '.AppendLine("    if ((@VlrD <> @VlrDPago) and @VlrD > 0) ")
                .AppendLine("    if (@VlrD > 0) ")
                .AppendLine("    begin ")
                .AppendLine("      if @VlrD > 0 ")
                .AppendLine("      begin ")
                '.AppendLine("        set @VlrAux = cast(@VlrD - @VlrDPago as varchar(20)) ")
                .AppendLine("        set @VlrAux = cast(@VlrD as varchar(20)) ")
                .AppendLine("        exec ( ")
                .AppendLine("          'insert into TbPgtoPendenteCaixa ' + ")
                .AppendLine("          '(ResId, IntId, PpcData, TipOprCod, PpcValor, PpcTipo, PpcDescricao, PpcStatus, PpcOrigem, ProId, PpcSacado, PpcCheckin, PpcCheckout) ' + ")
                .AppendLine("          'select ' + @Reserva + ', 0, ''' + @Data + ''', 13,''' + ")
                .AppendLine("            @VlrAux + ''', ''I'' as Tipo, case when ''' + @ResCaracteristica +  ")
                .AppendLine("            ''' <> ''P'' then ''Pagamento de Estadia' + ")
                .AppendLine("            ''' else ''Pagamento Passeio'' end, ''P'', ''VND'', ''' + @Origem + ''', ''' + @ResNome + ")
                .AppendLine("            ''', ''' + @ResDataIni + ''', ''' + @ResDataFim + '''') ")
                .AppendLine("        set @Erro = @Erro + @@Error ")
                .AppendLine("      end ")
                .AppendLine("      else ")
                .AppendLine("      begin ")
                .AppendLine("        set @VlrH = @VlrH + @VlrD ")
                .AppendLine("        set @VlrHPago = @VlrHPago + @VlrDPago ")
                .AppendLine("      end ")
                .AppendLine("    end ")

                '.AppendLine("    if ((@VlrR <> @VlrRPago) and @VlrR > 0) ")
                .AppendLine("    if (@VlrR > 0) ")
                .AppendLine("    begin ")
                .AppendLine("      if @VlrR > 0 ")
                .AppendLine("      begin ")
                '.AppendLine("        set @VlrAux = cast(@VlrR - @VlrRPago as varchar(20)) ")
                .AppendLine("        set @VlrAux = cast(@VlrR as varchar(20)) ")
                .AppendLine("        exec ( ")
                .AppendLine("          'insert into TbPgtoPendenteCaixa ' + ")
                .AppendLine("          '(ResId, IntId, PpcData, TipOprCod, PpcValor, PpcTipo, PpcDescricao, PpcStatus, PpcOrigem, ProId, PpcSacado, PpcCheckin, PpcCheckout) ' + ")
                .AppendLine("          'select ' + @Reserva + ', 0, ''' + @Data + ''', case when ''' + @ResCaracteristica + '''<> ''P'' then 14 else 7 end, ''' + ")
                .AppendLine("            @VlrAux + ''', ''I'' as Tipo, case when ''' + @ResCaracteristica + '''<> ''P'' then ''Pagamento de Estadia' + ")
                .AppendLine("            ''' else ''Pagamento Passeio'' end, ''P'', ''VND'', ''' + @Origem + ''', ''' + @ResNome + ")
                .AppendLine("            ''', ''' + @ResDataIni + ''', ''' + @ResDataFim + '''') ")
                .AppendLine("        set @Erro = @Erro + @@Error ")
                .AppendLine("      end ")
                .AppendLine("      else ")
                .AppendLine("      begin ")
                .AppendLine("        set @VlrH = @VlrH + @VlrR ")
                .AppendLine("        set @VlrHPago = @VlrHPago + @VlrRPago ")
                .AppendLine("      end  ")
                .AppendLine("    end ")

                '.AppendLine("    if ((@VlrH <> @VlrHPago) and @VlrH > 0) ")
                .AppendLine("    if (@VlrH > 0) ")
                .AppendLine("    begin ")
                '.AppendLine("      set @VlrAux = cast(@VlrH - @VlrHPago as varchar(20)) ")
                .AppendLine("      set @VlrAux = cast(@VlrH as varchar(20)) ")
                .AppendLine("      exec ( ")
                .AppendLine("        'insert into TbPgtoPendenteCaixa' +  ")
                .AppendLine("        '(ResId, IntId, PpcData, TipOprCod, PpcValor, PpcTipo, PpcDescricao, PpcStatus, PpcOrigem, ProId, PpcSacado, PpcCheckin, PpcCheckout) ' + ")
                .AppendLine("        'select ' + @Reserva + ', 0, ''' + @Data + ''', 12, ''' + ")
                .AppendLine("          @VlrAux + ''', ''I'' as Tipo, case when ''' + @ResCaracteristica + '''<> ''P'' then ''Pagamento de Estadia' + ")
                .AppendLine("          ''' else ''Pagamento Passeio'' end, ''P'', ''VND'', ''' + @Origem + ''', ''' + @ResNome + ")
                .AppendLine("          ''', ''' + @ResDataIni + ''', ''' + @ResDataFim + '''') ")
                .AppendLine("      set @Erro = @Erro + @@Error ")
                .AppendLine("    end ")

                .AppendLine("    set @VlrAux = cast(@VlrVen - @VlrVenPg as varchar(20)) ")
                .AppendLine("    if (cast(@VlrAux as numeric(18,2)) > 0) ")
                .AppendLine("    begin ")
                .AppendLine("      exec ( ")
                .AppendLine("        'insert into TbPgtoPendenteCaixa ' + ")
                .AppendLine("        '(ResId, IntId, PpcData, TipOprCod, PpcValor, PpcTipo, PpcDescricao, PpcStatus, PpcOrigem, ProId, PpcSacado, PpcCheckin, PpcCheckout) ' + ")
                .AppendLine("        'select ' + @Reserva + ', 0, ''' + @Data + ''', 0, ''' + ")
                .AppendLine("        @VlrAux + ''', ''C'' as Tipo, case when ''' + @ResCaracteristica + '''<> ''P'' then ''Pagamento de Estadia' +  ")
                .AppendLine("        ''' else ''Pagamento Passeio'' end, ''P'', ''VND'', ''' + @Origem + ''', ''' + @ResNome + ")
                .AppendLine("          ''', ''' + @ResDataIni + ''', ''' + @ResDataFim + '''') ")
                .AppendLine("      set @Erro = @Erro + @@Error ")
                .AppendLine("    end ")
                .AppendLine("  end ")
                .AppendLine("end ")

                .AppendLine("exec ('select * from TbPgtoPendenteCaixa where ResId = ' + @Reserva + ' and PpcOrigem = ''VND''')  ")

                .AppendLine("if @Erro = 0 ")
                .AppendLine("  commit tran ")
                .AppendLine("else ")
                .AppendLine("  rollback tran ")
            End With
            Return Conn.executaTransacionalTestaRetorno(VarSql.ToString)
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
