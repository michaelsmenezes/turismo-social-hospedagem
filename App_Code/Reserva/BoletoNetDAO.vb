Imports Microsoft.VisualBasic, Banco, System.Data.SqlClient
Imports System
Imports System.Collections
Imports System.Text

Public Class BoletoNetDAO

    Dim varConn As Conexao
    Dim objBoletoNetVO As BoletoNetVO
    Dim varBd As String

    Public Function GeraBoleto(ByVal resId As String, _
                              ByVal BoletoCupom As String, _
                              ByVal Integrante As String, _
                              ByVal Vencimento As String, _
                              ByVal Usuario As String, _
                              ByVal Percentual As String, _
                              Destino As String) As BoletoNetVO
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            'varConn = New Banco.Conexao(varBd)
            Dim sSql As Text.StringBuilder
            sSql = New Text.StringBuilder("exec SpDadosBoletoWeb " & resId & ", '" & BoletoCupom & "', '" & Integrante & "', '" & _
                    Vencimento & "', '" & Usuario & "', " & Percentual)
            Return preencheObjeto(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Public Function GeraBoleto(ByVal resId As String, _
                               ByVal BoletoCupom As String, _
                               ByVal Integrante As String, _
                               ByVal Vencimento As String, _
                               ByVal Usuario As String, _
                               ByVal Percentual As String, _
                               ByVal ValorDoc As String, _
                               Destino As String) As BoletoNetVO
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If

            'varConn = New Banco.Conexao(varBd)
            Dim sSql As Text.StringBuilder
            sSql = New Text.StringBuilder("exec SpDadosBoletoParceladoWeb " & resId & ", '" & BoletoCupom & "', '" & Integrante & "', '" & _
                    Vencimento & "', '" & Usuario & "', " & Percentual & ", '" & ValorDoc & "'")
            Return preencheObjeto(varConn.consulta(sSql.ToString))


        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Public Function GeraBoletoEmissivo(ByVal resId As String,
                             ByVal BoletoCupom As String,
                             ByVal Integrante As String,
                             ByVal Vencimento As String,
                             ByVal Usuario As String,
                             ByVal Percentual As String,
                             ByVal Destino As String) As BoletoNetVO
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            'varConn = New Banco.Conexao(varBd)
            Dim sSql As Text.StringBuilder
            sSql = New Text.StringBuilder("exec SpDadosBoletoPasseioNet " & resId & ", '" & BoletoCupom & "', '" & Integrante & "', '" &
                    Vencimento & "', '" & Usuario & "', " & Percentual)
            Return preencheObjeto(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Public Function GeraBoletoGrupo(ByVal resId As String, _
                             ByVal BoletoCupom As String, _
                             ByVal Vencimento As String, _
                             ByVal Usuario As String, _
                             ByVal Valor As String, _
                             ByVal Percentual As String, Destino As String) As BoletoNetVO

        'exec SpDadosBoletoGrupo 483013, 'B','26/12/2016','wborges', '150.00', 100

        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            'varConn = New Banco.Conexao(varBd)
            Dim sSql As Text.StringBuilder
            sSql = New Text.StringBuilder("exec SpDadosBoletoGrupo " & resId & ", '" & BoletoCupom & "', '" & _
                    Vencimento & "', '" & Usuario & "', " & Valor & ", " & Percentual & " ")
            Return preencheObjeto(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function


    Private Function preencheObjeto(ByVal resultadoConsulta As SqlDataReader) As BoletoNetVO
        If (resultadoConsulta.HasRows) Then 'Se existe algum registro
            resultadoConsulta.Read()
            objBoletoNetVO = New BoletoNetVO
            With objBoletoNetVO
                .Boleto = resultadoConsulta.Item("Boleto")
                .Cedente = resultadoConsulta.Item("Cedente")
                .AgeCodCed = resultadoConsulta.Item("AgeCodCed")
                .DataDocumento = resultadoConsulta.Item("DataDocumento")
                .DataProces = resultadoConsulta.Item("DataProces")
                .Vencimento = resultadoConsulta.Item("Vencimento")
                .Sacado = resultadoConsulta.Item("Sacado")
                .NossoNum = resultadoConsulta.Item("NossoNum")
                .VlrDoc = resultadoConsulta.Item("VlrDoc")
                .Demonstrativo = resultadoConsulta.Item("Demonstrativo")
                .Reserva = resultadoConsulta.Item("Reserva")
                .QtdePess = resultadoConsulta.Item("QtdePess")
                .Periodo = resultadoConsulta.Item("Periodo")
                .Entrada = resultadoConsulta.Item("Entrada")
                .Saida = resultadoConsulta.Item("Saida")
                If Convert.IsDBNull(resultadoConsulta.Item("Banco")) Then
                    .Banco = ""
                Else
                    .Banco = resultadoConsulta.Item("Banco")
                End If
                .CodBanco = resultadoConsulta.Item("CodBanco")
                If Convert.IsDBNull(resultadoConsulta.Item("End1")) Then
                    .End1 = ""
                Else
                    .End1 = resultadoConsulta.Item("End1")
                End If
                .CodigoIPTE = resultadoConsulta.Item("CodigoIPTE")
                .CodigoBarra = resultadoConsulta.Item("CodigoBarra")
                .Acomodacao = resultadoConsulta.Item("Acomodacao")
                .DetalhePessoa = resultadoConsulta.Item("DetalhePessoa")
                .DetalheValor = resultadoConsulta.Item("DetalheValor")
                'Campos complementares para geração de boleto com registro (centralizando as informações necessárias em TbBoletosImp)
                .ResNome = resultadoConsulta.Item("Sacado")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpCGC_CPF")) Then .ResCPFCGC = " " Else .ResCPFCGC = resultadoConsulta.Item("BolImpCGC_CPF")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpLogradouro")) Then .ResLogradouro = " " Else .ResLogradouro = resultadoConsulta.Item("BolImpLogradouro")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpBairro")) Then .ResBairro = " " Else .ResBairro = resultadoConsulta.Item("BolImpBairro")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpNumero")) Then .ResNumero = " " Else .ResNumero = resultadoConsulta.Item("BolImpNumero")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpQuadra")) Then .ResQuadra = " " Else .ResQuadra = resultadoConsulta.Item("BolImpQuadra")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpLote")) Then .ResLote = " " Else .ResLote = resultadoConsulta.Item("BolImpLote")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpComplemento")) Then .ResComplemento = " " Else .ResComplemento = resultadoConsulta.Item("BolImpComplemento")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpCEP")) Then .ResCep = " " Else .ResCep = resultadoConsulta.Item("BolImpCEP")
                If Convert.IsDBNull(resultadoConsulta.Item("EstId")) Then .UF = " " Else .UF = resultadoConsulta.Item("EstId")
                If Convert.IsDBNull(resultadoConsulta.Item("BolImpCidade")) Then .ResCidade = " " Else .ResCidade = resultadoConsulta.Item("BolImpCidade")
            End With
        End If
        resultadoConsulta.Close()
        Return objBoletoNetVO
    End Function
    'Essa função ficou diferente do TurWeb, pois aqui sempre irei salvar um novo registro e nunca sobrepor
    Public Function GravaDadosReimpressaoBoletos(Destino As String, objBoletoVO As BoletoNetVO, Acao As String, TipoParcela As String) As Long
        Try
            objBoletoVO.NossoNum = Mid(objBoletoVO.NossoNum, 1, 10)
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As New StringBuilder("Set Nocount on ")
            With sSql
                .AppendLine("IF EXISTS(Select 1 from TbReserva where Resid = " & objBoletoVO.Reserva & ")")
                .AppendLine("  BEGIN ")
                .AppendLine("      INSERT TbBoletosReimpressao(ResId,BolImpId,BolHTML,BolTipoParcela,BolDataUltimaReimpressao)")
                .AppendLine("      VALUES ")
                'Obs.: Quando restaurar o html, onde tiver o caractere % converter para '
                .AppendLine("('" & objBoletoVO.Reserva & "','" & objBoletoVO.NossoNum & "','" & objBoletoVO.BolHTML.Replace("'", "%") & "','" & TipoParcela & "',GetDate())") '
                .AppendLine("  END ")
                .AppendLine("If @@ERROR > 0")
                .AppendLine("    BEGIN ")
                .AppendLine("      SELECT 0 GOTO saida ")
                .AppendLine("   END")
                .AppendLine("ELSE ")
                .AppendLine("   SELECT 1 GOTO saida ")
                .AppendLine("saida: ")
                'End If
            End With
            Dim Resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function
    Public Function ConsultaBoletosReimpressao(Destino As String, objBoletoVO As BoletoNetVO) As IList
        Try
            objBoletoVO.NossoNum = Mid(objBoletoVO.NossoNum, 1, 10)
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As New StringBuilder("Set Nocount on ")
            With sSql
                .AppendLine("select r.Resid, r.BolImpId,r.bolHtml,b.BolImpDtDocumento,b.BolImpDtVencimento,b.BolImpValor, ")
                .AppendLine("case r.BolTipoParcela ")
                .AppendLine(" When 'P' then 'Primeira Parcela' ")
                .AppendLine(" When 'S' then 'Segunda Parcela' ")
                .AppendLine(" When 'U' then 'Parcela Única' ")
                .AppendLine("End as BolTipoParcela ")
                .AppendLine(" from TbBoletosReimpressao r ")
                .AppendLine("inner join TbBoletosImp b on b.BolImpId = r.BolImpId ")
                .AppendLine("Where r.ResId = '" & objBoletoVO.Reserva & "' ")
                .AppendLine("and CONVERT(char(10), GETDATE(),120) <= CONVERT(char(10), b.BolImpDtVencimento,120)")
            End With
            Return PreencheListaReimpressao(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Private Function PreencheListaReimpressao(ResultadoConsulta As SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                objBoletoNetVO = New BoletoNetVO
                With objBoletoNetVO
                    .Reserva = ResultadoConsulta.Item("Resid")
                    .NossoNum = ResultadoConsulta.Item("BolImpId")
                    .BolHTML = ResultadoConsulta.Item("bolHtml")
                    .DataDocumento = Format(CDate(ResultadoConsulta.Item("BolImpDtDocumento")), "dd/MM/yyyy")
                    .Vencimento = Format(CDate(ResultadoConsulta.Item("BolImpDtVencimento")), "dd/MM/yyyy")
                    .VlrDoc = ResultadoConsulta.Item("BolImpValor")
                    .BolTipoParcela = ResultadoConsulta.Item("BolTipoParcela")
                End With
                Lista.Add(objBoletoNetVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    '    Public Function SobrepontoBoletoReimpressao(Destino As String, BolImpId As Long, NovoValor As String, Resid As Long, BolTipoParcela As String, DataDocumento As String, DataProcessamento As String, DataVencimento As String) As Long
    Public Function SobrepontoBoletoReimpressao(Destino As String, BolImpId As Long, NovoValor As String, Resid As Long, BolTipoParcela As String, DataVencimento As String) As Long

        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If

            Dim sSql As New Text.StringBuilder("Set Nocount on ")
            With sSql
                .AppendLine("Declare @BolImpId varchar(10) ")
                '.AppendLine("Declare @BolImpValor Numeric(18,2) ")
                .AppendLine("Declare @NovoValor Numeric(18,2) ")
                .AppendLine("Declare @ResId Numeric(10) ")
                .AppendLine("Declare @BolTipoParcela char(1) ")
                '.AppendLine("Set @BolImpValor = '" & BolImpValor.Replace(",", ".") & "' ")
                .AppendLine("Set @NovoValor = '" & NovoValor.Replace(",", "") & "' ")
                .AppendLine("Set @ResId = " & Resid & " ")
                .AppendLine("set @BolTipoParcela= '" & BolTipoParcela & "' ")
                .AppendLine("if exists(select 1 from TbBoletosReimpressao where ResId = @ResId and BolTipoParcela = @BolTipoParcela ) ")
                .AppendLine("Begin")
                .AppendLine("  if exists( ")
                .AppendLine("  select top 1 1 from TbBoletosReimpressao r ")
                .AppendLine("  inner join TbBoletosImp b on b.BolImpId = r.BolImpId ")
                .AppendLine("  where r.ResId  = @ResId ")
                .AppendLine("  And r.bolTipoParcela = (@BolTipoParcela) ")
                .AppendLine("  and b.BolImpPagou = 'N' ")
                .AppendLine("  and not exists(select 1 from tbVencimento v where v.VenNossoNumero = B.BolImpNossoNumero)) ")
                .AppendLine("     Begin ")
                .AppendLine("       Set @BolImpId = (select top 1 b.BolImpId from TbBoletosReimpressao r ")
                .AppendLine("                       inner join TbBoletosImp b on b.BolImpId = r.BolImpId ")
                .AppendLine("                       where r.ResId  = @ResId ")
                .AppendLine("                       And r.bolTipoParcela = (@BolTipoParcela) ")
                .AppendLine("                       and b.BolImpPagou = 'N' ")
                .AppendLine("                       and not exists(select 1 from tbVencimento v where v.VenNossoNumero = B.BolImpNossoNumero) order by b.BolImpId) ")
                'Faz o log antes de fazer update 
                .AppendLine("       Insert TbBoletosImpLog(BolImpId,BolImpNossoNumero,ResId,BolTipo,BolImpValor,BolImpDtVencimento,BolImpDtDocumento, ")
                .AppendLine("                          BolImpDtProcessamento,BolImpPagou,BolImpImpresso,BolImpDtPagamento,BolImpEndereco,BolImpSacado, ")
                .AppendLine("                          BolImpCGC_CPF,BolImpAgCodCedente,BolImpCodIpte,BolImpCodBarra,BolImpUsuario,BolAntecipacao, ")
                .AppendLine("                          BolNitCartaoCredito,BolNsuCartaoCredito,BolLocalidadeCartaoCredito,BolConfirmacaoPagtoCartaoCredito, ")
                .AppendLine("                          BolCodBarraNovoBoleto,BolCodDigitavelNovoBoleto,BolCodCedenteNovo,BolStatusPgtoCartaoCredito, ")
                .AppendLine("                          BolValorPagoCartaoCredito,BolParcelasCartaoCredito,BolRedeCartaoCredito,BolNumeroAutorizacaoCartaoCredito,BolDataSitefCartaoCredito) ")

                .AppendLine("                          Select BolImpId,BolImpNossoNumero,ResId,BolTipo,BolImpValor,BolImpDtVencimento,BolImpDtDocumento, ")
                .AppendLine("                          BolImpDtProcessamento,BolImpPagou,BolImpImpresso,BolImpDtPagamento,BolImpEndereco,BolImpSacado, ")
                .AppendLine("                          BolImpCGC_CPF,BolImpAgCodCedente,BolImpCodIpte,BolImpCodBarra,BolImpUsuario,BolAntecipacao, ")
                .AppendLine("                          BolNitCartaoCredito,BolNsuCartaoCredito,BolLocalidadeCartaoCredito,BolConfirmacaoPagtoCartaoCredito, ")
                .AppendLine("                          BolCodBarraNovoBoleto,BolCodDigitavelNovoBoleto,BolCodCedenteNovo,BolStatusPgtoCartaoCredito, ")
                .AppendLine("                          BolValorPagoCartaoCredito,BolParcelasCartaoCredito,BolRedeCartaoCredito,BolNumeroAutorizacaoCartaoCredito,BolDataSitefCartaoCredito ")
                .AppendLine("                          from TbBoletosImp where BolImpid = @BolImpId ")
                'Faz o Update em seguida 
                '.AppendLine("       Update TbBoletosImp set BolImpDtDocumento=Getdate(),BolImpDtVencimento = '" & Format(CDate(DataVencimento), "yyyy-MM-dd") & "',BolImpDtProcessamento =GetDate(), BolImpValor = @NovoValor ")
                .AppendLine("       Update TbBoletosImp set BolImpValor = @NovoValor, BolImpDtVencimento = '" & Format(CDate(DataVencimento), "yyyy-MM-dd") & "' ")
                .AppendLine("       where BolImpId = @BolImpId ")
                .AppendLine("       Select '1' +  @BolImpId  goto saida ") 'Select 1  goto saida ")
                .AppendLine("     end ")
                .AppendLine("   else ")
                .AppendLine("     Begin ")
                .AppendLine("       Select 2 goto saida ")
                .AppendLine("     end ")
                .AppendLine("end ")
                .AppendLine("else ")
                .AppendLine("  Select 2 goto saida") 'Nunca foi gerado boleto, será gerado agora
                .AppendLine("saida: ")
            End With
            Dim resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    Public Function PreencheDadosBoletoReimpressao(Destino As String, ResId As Long, Boltipo As String, BolTipoParcela As String, BolImpId As String) As BoletoNetVO
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If

            Dim sSql As New Text.StringBuilder("Set Nocount On ")
            With sSql
                .AppendLine("Declare @BolImpImpresso varchar(10) ")
                .AppendLine("declare @Detalhamento varchar(8000) ")
                .AppendLine("Declare @ResId numeric ")
                .AppendLine("Declare @ResDataIni Datetime ")
                .AppendLine("Declare @BolTipo char(1) ")
                .AppendLine("Declare @BolTipoParcela char(1) ")
                .AppendLine("Declare @Banco Varchar(50) ")

                .AppendLine("Set @ResId = " & ResId & " ")
                .AppendLine("set @BolTipo = '" & Boltipo & "' ")
                .AppendLine("set @BolTipoParcela = '" & BolTipoParcela & "' ")
                .AppendLine("Set @BolImpImpresso = '" & BolImpId.Trim & "' ")
                .AppendLine("Set @ResDataIni = (Select cast(ResDataIni as Date) from tbReserva where resid = @ResId ) ")

                .AppendLine(" create table #Acomodacao ( ")
                .AppendLine("	[AcmId] [numeric](18, 0), ")
                .AppendLine("	[Qtde] [int], ")
                .AppendLine("	[Descricao] [varchar] (5000), ")
                .AppendLine("             [HosDataIniSol] [datetime], ")
                .AppendLine("             [HosDataFimSol] [datetime]) ")
                .AppendLine("  insert #Acomodacao ")
                .AppendLine("    select ")
                .AppendLine("      h.AcmId, count(distinct h.SolId) Qtde, ")
                .AppendLine("        (Select a.AcmDescricaoSucinto ")
                .AppendLine("        from TbAcomodacao a where a.AcmId = h.AcmId), ")
                .AppendLine("        h.HosDataIniSol, h.HosDataFimSol ")
                .AppendLine("      from TbHospedagem h ")
                .AppendLine("      where h.IntId in ")
                .AppendLine("      (select i.IntId from TbIntegranteBoleto i ")
                .AppendLine("        where i.BolImpId = @BolImpImpresso) ")
                .AppendLine("--      and h.HosStatus = 'A' ")
                .AppendLine("      group by h.AcmId, h.HosDataIniSol, h.HosDataFimSol ")
                .AppendLine("      order by h.HosDataIniSol, h.HosDataFimSol ")

                .AppendLine("  declare Acomodacao_cursor cursor for ")
                .AppendLine("    select a.AcmId, CarDescricaoSucinto ")
                .AppendLine("      from #Acomodacao a ")
                .AppendLine("      inner join TbCaracteristicaAcm ca ")
                .AppendLine("      on a.AcmId = ca.AcmId")
                .AppendLine("      inner join TbCaracteristica c ")
                .AppendLine("      on ca.CarId = c.CarId")
                .AppendLine("      where CarDescricaoSucinto > '' ")

                .AppendLine("  declare @AcmId numeric, ")
                .AppendLine("          @Descricao varchar(20) ")

                .AppendLine("  open Acomodacao_cursor ")
                .AppendLine("  fetch next from Acomodacao_cursor ")
                .AppendLine("    into @AcmId, @Descricao ")

                .AppendLine("  while @@fetch_status = 0 ")
                .AppendLine("  begin ")
                .AppendLine("      update #Acomodacao ")
                .AppendLine("      set Descricao = Descricao + ', ' + @Descricao ")
                .AppendLine("      where AcmId = @AcmId ")
                .AppendLine("    fetch next from Acomodacao_cursor ")
                .AppendLine("      into @AcmId, @Descricao ")
                .AppendLine("    end ")

                .AppendLine("  close Acomodacao_cursor ")
                .AppendLine("  deallocate Acomodacao_cursor ")

                .AppendLine("  Declare @Unidade Varchar(20) ")
                .AppendLine("  set @Unidade = (select CGCCerec  from TbDefault) ")
                .AppendLine("  Declare @Bloco varchar(20) ")

                .AppendLine("    if @Unidade  = '03671444000490' --Caldas Novas ")
                .AppendLine("        Begin ")
                .AppendLine("            set @Bloco = (select top 1 case when BloId = 1 then 'Bloco Rio Tocantins-' ")
                .AppendLine("                                      when BloId = 2 then 'Bloco Rio Araguaia-' ")
                .AppendLine("                                      When BloId = 3 then 'Bloco Rio Paranaíba-' ")
                .AppendLine("                                      When BloId = 4 then 'Bloco Rio Vermelho-' end from TbAcomodacao where AcmId = @AcmId) ")
                .AppendLine("       end ")
                .AppendLine("   Else ")
                .AppendLine("       Begin")
                .AppendLine("          Set @Bloco = ISNULL((select top 1 case when BloId = 1 and  AcmDescricaoEmail like '%ar%condicionado%' then 'com ar condicionado'")
                .AppendLine("                                                 when AcmDescricaoEmail like '%Ventilador%' then 'com ventilador' ")
                .AppendLine("                                               else ''")
                .AppendLine("                                                 end from TbAcomodacao where AcmId = @AcmId),'Pirenópolis') ")
                .AppendLine("       End")
                '.AppendLine("     else ")
                '.AppendLine("       Begin")
                '.AppendLine("            Set @Bloco  = '' ")
                '.AppendLine("       end  ")

                .AppendLine("  declare @Acomodacao varchar(8000) ")
                .AppendLine("  set @Acomodacao = '' ")
                .AppendLine("  if (select count(1) from #Acomodacao) = 1 ")
                .AppendLine("    --set @Acomodacao = 'Acomodação:'")
                .AppendLine("    set @Acomodacao = @Bloco ")
                .AppendLine("  else")
                .AppendLine("    --set @Acomodacao = 'Acomodações:' ")
                .AppendLine("    set @Acomodacao = @Bloco ")
                .AppendLine("  select @Acomodacao =  ")
                .AppendLine("    --@Acomodacao + '<BR>' + cast(Qtde as varchar(2)) + case")
                .AppendLine("    @Acomodacao + cast(Qtde as varchar(2)) + case ")

                .AppendLine("      when Qtde = 1 then ' Apto ' ")
                .AppendLine("      else ' Aptos ' ")
                .AppendLine("    end + ' (' + Descricao + ')' -- de ' + ")
                .AppendLine("    --substring(convert(char(10), HosDataIniSol, 103),1,5) + ")
                .AppendLine("    --' a ' + substring(convert(char(10), HosDataFimSol, 103),1,10) ")
                .AppendLine("    from #Acomodacao ")
                .AppendLine("  drop table #Acomodacao ")

                .AppendLine("Set @Banco = (Select DescricaoBancoBoleto from TbDefault) ")

                .AppendLine("  set @Detalhamento = 'Detalhamento<br>Adultos pagantes: ' + ")
                .AppendLine("  cast((select count(1) from tbintegranteboleto ib ")
                .AppendLine("    inner join tbintegrante i ")
                .AppendLine("    on i.IntId = ib.Intid ")
                .AppendLine("    inner join tbcategoria c ")
                .AppendLine("    on i.CatId = c.CatId ")
                .AppendLine("    where BolImpId = @BolImpImpresso ")
                .AppendLine("    and i.IntFormaPagamento not in ('C', 'F') ")
                .AppendLine("    and CatFaixaEtaria = (select case when @ResDataIni <= FaixaEtariaData then FaixaEtariaAdulto else FaixaEtariaAdulto1 end from tbdefault)) as varchar(3)) + ")
                .AppendLine("    '<br>Crianças pagantes: ' + ")
                .AppendLine("  cast((select count(1) from tbintegranteboleto ib ")
                .AppendLine("    inner join tbintegrante i ")
                .AppendLine("    on i.IntId = ib.Intid ")
                .AppendLine("    inner join tbcategoria c ")
                .AppendLine("    on i.CatId = c.CatId ")
                .AppendLine("    where BolImpId = @BolImpImpresso ")
                .AppendLine("    and CatFaixaEtaria = (select case when @ResDataIni <= FaixaEtariaData then FaixaEtariaCrianca else FaixaEtariaCrianca1 end from tbdefault)) as varchar(3)) + ")
                .AppendLine("    '<br>Crianças isentas: ' + ")
                .AppendLine("  cast((select count(1) from tbintegranteboleto ib ")
                .AppendLine("    inner join tbintegrante i ")
                .AppendLine("    on i.IntId = ib.Intid ")
                .AppendLine("    inner join tbcategoria c ")
                .AppendLine("    on i.CatId = c.CatId ")
                .AppendLine("    where BolImpId = @BolImpImpresso ")
                .AppendLine("    and CatFaixaEtaria = (select case when @ResDataIni <= FaixaEtariaData then FaixaEtariaIsento else FaixaEtariaIsento1 end from tbdefault)) as varchar(3)) + ")
                .AppendLine("    '<br>Motorista/Guia: ' + ")
                .AppendLine("  cast((select count(1) ")
                .AppendLine("    from tbintegranteboleto ib ")
                .AppendLine("    inner join tbintegrante i ")
                .AppendLine("    on i.IntId = ib.Intid ")
                .AppendLine("    inner join tbcategoria c ")
                .AppendLine("    on i.CatId = c.CatId ")
                .AppendLine("    where BolImpId = @BolImpImpresso ")
                .AppendLine("    and i.IntFormaPagamento in ('C', 'F') ")
                .AppendLine("    and CatFaixaEtaria = (select case when @ResDataIni <= FaixaEtariaData then FaixaEtariaAdulto else FaixaEtariaAdulto1 end from tbdefault)) as varchar(3)) ")

                .AppendLine(" declare @Valores varchar(100) ")
                .AppendLine("  set @Valores = 'Valor Total: R$ ' +  ")
                .AppendLine("    isnull(cast((select sum(VenValor) from TbVencimento where ResId = @ResId ")
                .AppendLine("    and VenStatus = 'V' and VenFormaPagto not in ('F', 'C')) as varchar(20)), '0.00') + ")
                .AppendLine("    ' Valor Pago: R$ ' + ")
                .AppendLine("    isnull(cast((select sum(VenValor) from TbVencimento where ResId = @ResId ")
                .AppendLine("    and VenStatus in ('M','C','B','T','A') and VenFormaPagto not in ('F', 'C')) as varchar(20)), '0.00') + ")
                .AppendLine("    ' Valor Devido: R$ ' + ")
                .AppendLine("    cast(isnull((select sum(VenValor) from TbVencimento where ResId = @ResId ")
                .AppendLine("    and VenStatus = 'V' and VenFormaPagto not in ('F', 'C')),0.00) - ")
                .AppendLine("    isnull((select sum(VenValor) from tbvencimento where resid = @ResId ")
                .AppendLine("      and VenStatus in ('M','C','B','T','A') and VenFormaPagto not in ('F', 'C')),0.00) as varchar(20)) ")
                .AppendLine("  set @Valores = replace(@Valores,'.',',') ")

                .AppendLine("select b.BolImpId as Boleto,'03671444000147' as Cedente,SUBSTRING(b.BolCodCedenteNovo,1,4) as AgeCodCed, ")
                .AppendLine("convert(char(10), b.BolImpDtDocumento, 103) as DataDocumento, ")
                .AppendLine("convert(char(10), b.BolImpDtProcessamento, 103) as DataProces, ")
                .AppendLine("convert(char(10), b.BolImpDtVencimento, 103) as Vencimento, ")
                .AppendLine("b.BolImpSacado as Sacado, b.BolImpNossoNumero as NossoNum, b.BolImpValor as VlrDoc, ")
                If Destino = "C" Then
                    .AppendLine("' - Sesc Caldas Novas' as Demonstrativo ")
                Else
                    .AppendLine("' - Pousada Sesc Pirenópolis' as Demonstrativo ")
                End If
                .AppendLine(",b.ResId as Reserva, ")
                .AppendLine("(select count(ib.IntId) from TbIntegranteBoleto ib where ib.BolImpId = b.BolImpId) as QtdePess, ")
                .AppendLine("convert(char(10), r.ResDataIni, 103) + ' a ' + convert(char(10), r.ResDataFim, 103) as Periodo, ")
                .AppendLine("r.ResDataIni as Entrada,r.ResDataFim as Saida,@Banco as Banco, (Select NumeroBancoBoleto from TbDefault) as CodBanco, ")
                .AppendLine("isnull(b.BolImpEndereco,'') as End1, b.BolCodDigitavelNovoBoleto as CodigoIPTE, b.BolCodBarraNovoBoleto as CodigoBarra, ")
                .AppendLine("isNull(@Acomodacao,'') as Acomodacao, @Detalhamento as DetalhePessoa,@Valores as DetalheValor,b.BolImpCGC_CPF ")
                .AppendLine(",ISNULL(b.BolImpLogradouro,'')BolImpLogradouro,ISNULL(b.BolImpBairro,'')BolImpBairro,ISNULL(b.BolImpNumero,'')BolImpNumero,ISNULL(b.BolImpQuadra,'')BolImpQuadra ")
                .AppendLine(",ISNULL(b.BolImpLote,'')BolImpLote,ISNULL(b.BolImpComplemento,'')BolImpComplemento,ISNULL(b.BolImpCEP,'')BolImpCEP,ISNULL(b.EstId,'')EstId,ISNULL(b.BolImpCidade,'')BolImpCidade ")
                .AppendLine("from TbBoletosImp b ")
                .AppendLine("inner join TbReserva r on r.ResId = b.ResId ")
                .AppendLine("inner join TbBoletosImp i on i.BolImpId = b.BolImpId  ")
                .AppendLine("where r.ResId = @ResId ")
                .AppendLine("and i.BolTipo = @BolTipo ")
                .AppendLine("and i.BolImpImpresso = 'N' ")
                .AppendLine("and i.BolImpTipoParcela = @BolTipoParcela ")
                .AppendLine("and b.bolimpid = @BolImpImpresso ")
            End With
            Return preencheObjeto(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    'Public Function GeraBoleto(ByVal resId As String, _
    'ByVal Vencimento As String, _
    'ByVal Percentual As String, _
    'ByVal Destino As String) As BoletoNetVO
    '    Try
    '        If Destino = "C" Then
    '            varConn = New Conexao("TurismoSocialCaldas")
    '        Else
    '            varConn = New Conexao("TurismoSocialPiri")
    '        End If
    '        Dim sSql As StringBuilder
    '        sSql = New StringBuilder("exec SpDadosBoletoWeb " & resId & ", 'B', '.', '" & _
    '                Vencimento & "', 'Internet', " & Percentual)
    '        Return preencheObjeto(varConn.consulta(sSql.ToString))
    '    Catch ex As Exception
    '        Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
    '    End Try
    'End Function

    'Public Function GeraBoleto(ByVal resId As String, _
    '    ByVal Vencimento As String, _
    '    ByVal Percentual As String, _
    '    ByVal Destino As String, _
    '    ByVal ValorDoc As String) As BoletoNetVO
    '    Try
    '        If Destino = "C" Then
    '            varConn = New Conexao("TurismoSocialCaldas")
    '        Else
    '            varConn = New Conexao("TurismoSocialPiri")
    '        End If
    '        Dim sSql As StringBuilder
    '        sSql = New StringBuilder("exec SpDadosBoletoParceladoWeb " & resId & ", 'B', '.', '" & _
    '                Vencimento & "', 'Internet', " & Percentual & ", '" & ValorDoc & "'")
    '        Return preencheObjeto(varConn.consulta(sSql.ToString))
    '    Catch ex As Exception
    '        Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
    '    End Try
    'End Function

    'Private Function preencheObjeto(ByVal resultadoConsulta As SqlDataReader) As BoletoNetVO
    '    If (resultadoConsulta.HasRows) Then 'Se existe algum registro
    '        While (resultadoConsulta.Read)
    '            objBoletoNetVO = New BoletoNetVO
    '            objBoletoNetVO.Boleto = resultadoConsulta.Item("Boleto")
    '            objBoletoNetVO.Cedente = resultadoConsulta.Item("Cedente")
    '            objBoletoNetVO.AgeCodCed = resultadoConsulta.Item("AgeCodCed")
    '            objBoletoNetVO.DataDocumento = resultadoConsulta.Item("DataDocumento")
    '            objBoletoNetVO.DataProces = resultadoConsulta.Item("DataProces")
    '            objBoletoNetVO.Vencimento = resultadoConsulta.Item("Vencimento")
    '            objBoletoNetVO.Sacado = resultadoConsulta.Item("Sacado")
    '            objBoletoNetVO.NossoNum = resultadoConsulta.Item("NossoNum")
    '            objBoletoNetVO.VlrDoc = resultadoConsulta.Item("VlrDoc")
    '            objBoletoNetVO.Demonstrativo = resultadoConsulta.Item("Demonstrativo")
    '            objBoletoNetVO.Reserva = resultadoConsulta.Item("Reserva")
    '            objBoletoNetVO.QtdePess = resultadoConsulta.Item("QtdePess")
    '            objBoletoNetVO.Periodo = resultadoConsulta.Item("Periodo")
    '            objBoletoNetVO.Entrada = resultadoConsulta.Item("Entrada")
    '            objBoletoNetVO.Saida = resultadoConsulta.Item("Saida")
    '            objBoletoNetVO.Banco = resultadoConsulta.Item("Banco")
    '            objBoletoNetVO.CodBanco = resultadoConsulta.Item("CodBanco")
    '            objBoletoNetVO.End1 = resultadoConsulta.Item("End1")
    '            objBoletoNetVO.CodigoIPTE = resultadoConsulta.Item("CodigoIPTE")
    '            objBoletoNetVO.CodigoBarra = resultadoConsulta.Item("CodigoBarra")
    '            objBoletoNetVO.Acomodacao = resultadoConsulta.Item("Acomodacao")
    '            objBoletoNetVO.DetalhePessoa = resultadoConsulta.Item("DetalhePessoa")
    '            objBoletoNetVO.DetalheValor = resultadoConsulta.Item("DetalheValor")
    '        End While
    '    End If
    '    resultadoConsulta.Close()
    '    Return objBoletoNetVO
    'End Function

    'Public Function AtualizaCodBarraNovoBoleto(ByVal BolImpId As String, CodBarra As String, CodDigitavel As String, CodCedente As String, Destino As String) As Long
    '    Try
    '        If Destino = "C" Then
    '            varConn = New Conexao("TurismoSocialCaldas")
    '        Else
    '            varConn = New Conexao("TurismoSocialPiri")
    '        End If
    '        Dim sSql As StringBuilder = New Text.StringBuilder("Set Nocount On ")
    '        With sSql
    '            .Append("if exists(select 1 from TbBoletosImp where BolImpId = '" & BolImpId & "' ) ")
    '            .Append("    Begin ")
    '            .Append("       Update TbBoletosImp set BolCodBarraNovoBoleto = '" & CodBarra & "',BolCodDigitavelNovoBoleto='" & CodDigitavel & "',BolCodCedenteNovo='" & CodCedente & "' where BolImpId = '" & BolImpId & "' ")
    '            .Append("       Select 1 Goto Saida ")
    '            .Append("    end ")
    '            .Append("  else ")
    '            .Append("    Begin ")
    '            .Append("       Select 0 Goto Saida ")
    '            .Append("    end ")
    '            .Append("  Saida: ")
    '        End With
    '        Dim Resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
    '        Return Resultado
    '    Catch ex As Exception
    '        Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
    '    End Try
    'End Function
    Public Function AtualizaCodBarraNovoBoleto(ByVal BolImpId As String, CodBarra As String, CodDigitavel As String, CodCedente As String, Destino As String, TipoParcela As String, BoletoHtml As String, ResdId As String) As Long
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As StringBuilder = New Text.StringBuilder("Set Nocount On ")
            With sSql
                .AppendLine("If exists(select 1 from TbBoletosImp where BolImpId = '" & BolImpId & "' ) ")
                .AppendLine(" Begin ")
                .AppendLine("    Update TbBoletosImp set BolCodBarraNovoBoleto = '" & CodBarra & "',BolCodDigitavelNovoBoleto='" & CodDigitavel & "',BolCodCedenteNovo='" & CodCedente & "' where BolImpId = '" & BolImpId & "' ")
                .AppendLine("    if Exists(select 1 from TbBoletosReimpressao where bolImpId = '" & BolImpId & "' )")
                .AppendLine("      Begin ")
                .AppendLine("        Update TbBoletosReimpressao set BolHTML='" & BoletoHtml.Replace("'", "%") & "',BolDataUltimaReimpressao=GetDate() ,BolQtdeVezes=(select ISNULL(BolQtdeVezes,0) + 1 from TbBoletosReimpressao where BolImpid = '" & BolImpId & "' ) where BolImpId ='" & BolImpId & "'  ")
                .AppendLine("        insert TbBoletosReimpressaoLog(bolImpId,bolDataReimpressao) values (" & BolImpId & ",GetDate()) ")
                .AppendLine("      End")
                .AppendLine("    else ")
                .AppendLine("      Begin ")
                .AppendLine("         insert TbBoletosReimpressao(ResId,BolImpId,BolHTML,BolTipoParcela)")
                .AppendLine("         values ")
                'Obs.: Quando restaurar o html, onde tiver o caractere % converter para '
                'TipoParcela = 'T' = Turismo Social (P-Primeira S-Segunda U-Unica) - T- Nunca irá sobrepor.
                .AppendLine("         ('" & ResdId & "','" & BolImpId & "','" & BoletoHtml.Replace("'", "%") & "','T')") '
                '.AppendLine("         ('" & ResdId & "','" & BolImpId & "','" & BoletoHtml.Replace("'", "%") & "','" & TipoParcela & "')") '
                .AppendLine("       End ")
                .AppendLine("    Select 1 Goto Saida ")
                .AppendLine(" End ")
                .AppendLine("if @@ERROR > 0  ")
                .AppendLine("    Begin ")
                .AppendLine("       Select 0 Goto Saida ")
                .AppendLine("    end ")
                .AppendLine("  Saida: ")
            End With
            Dim Resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Public Function ReimpressaoBoleto(BolImpId As String, Destino As String) As IList
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As StringBuilder = New Text.StringBuilder("Set Nocount On ")
            With sSql
                .Append("if exists(select 1 from TbBoletosImp where BolImpId = '" & BolImpId & "' ) ")
                .Append("    Begin ")
                .Append("       select BolImpId,ResId,BolImpValor,BolImpDtVencimento,BolImpDtDocumento,BolImpDtProcessamento, ")
                .Append("       BolImpEndereco,BolImpSacado,BolImpCGC_CPF,bolcodBarraNovoBoleto,BolCodDigitavelNovoBoleto,BolCodCedenteNovo ")
                .Append("       from TbBoletosImp where BolImpId = '" & BolImpId & "' ")
                .Append("    end ")
            End With
            Return preencheObjetoReimpressao(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function
    Private Function preencheObjetoReimpressao(ByVal resultadoConsulta As SqlDataReader) As IList
        Dim Lista = New ArrayList
        If (resultadoConsulta.HasRows) Then 'Se existe algum registro
            While (resultadoConsulta.Read)
                objBoletoNetVO = New BoletoNetVO
                objBoletoNetVO.Boleto = resultadoConsulta.Item("BolImpId")
                objBoletoNetVO.Reserva = resultadoConsulta.Item("ResId")
                objBoletoNetVO.VlrDoc = resultadoConsulta.Item("BolImpValor")
                objBoletoNetVO.Vencimento = resultadoConsulta.Item("BolImpDtVencimento")
                objBoletoNetVO.DataDocumento = resultadoConsulta.Item("BolImpDtDocumento")
                objBoletoNetVO.DataProces = resultadoConsulta.Item("BolImpDtProcessamento")
                objBoletoNetVO.End1 = resultadoConsulta.Item("BolImpEndereco")
                objBoletoNetVO.Sacado = resultadoConsulta.Item("BolImpSacado")
                objBoletoNetVO.BolImpCGC_CPF = resultadoConsulta.Item("BolImpCGC_CPF")
                objBoletoNetVO.BolCodBarraNovoBoleto = resultadoConsulta.Item("bolcodBarraNovoBoleto")
                objBoletoNetVO.BolCodDigitavelNovoBoleto = resultadoConsulta.Item("BolCodDigitavelNovoBoleto")
                objBoletoNetVO.BolCodCedenteNovo = resultadoConsulta.Item("BolCodCedenteNovo")
            End While
        End If
        resultadoConsulta.Close()
        Return Lista
    End Function

    Public Function RetornaCodCedenteNovo(Destino As String) As String
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As StringBuilder = New Text.StringBuilder("Set Nocount On ")
            With sSql
                .Append("if exists(select d.BancoCedenteBoletoNovo from TbDefault d) ")
                .Append("    Begin ")
                .Append("       select d.BancoCedenteBoletoNovo from TbDefault d goto saida ")
                .Append("    end ")
                .Append("  else ")
                .Append("    Begin ")
                .Append("        select 0 goto saida ")
                .Append("    end ")
                .Append("    saida: ")
            End With
            Dim Resultado = CStr(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function

    Public Function GeraSinalGrupo(Destino As String, ResId As String) As BoletoNetVO
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            'varConn = New Banco.Conexao(varBd)
            Dim sSql As Text.StringBuilder
            sSql = New Text.StringBuilder("exec SpLEListaDadosSolicitacao " & ResId)
            Return PreencheListaSinalGrupo(varConn.consulta(sSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try

    End Function

    Private Function PreencheListaSinalGrupo(ResultadoConsulta As SqlDataReader) As BoletoNetVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            objBoletoNetVO = New BoletoNetVO
            With objBoletoNetVO
                .Reserva = ResultadoConsulta.Item("Reserva")
                .VlrDoc = ResultadoConsulta.Item("VlrSinal")
            End With
        End If
        ResultadoConsulta.Close()
        Return objBoletoNetVO
    End Function

    Public Function SobrepontoBoletoReimpressao(Destino As String, Resid As Long, BolTipoParcela As String, Integrante As String) As Long
        Try

            Integrante = Mid(Integrante, 1, Integrante.Length - 1).Replace(".", ",")

            Dim sSql As New Text.StringBuilder("Set Nocount on ")
            With sSql
                .AppendLine("Declare @BolImpId varchar(10) ")
                .AppendLine("Declare @ResId Numeric(10) ")
                .AppendLine("Declare @BolTipoParcela char(1) ")
                .AppendLine("Set @ResId = " & Resid & " ")
                .AppendLine("set @BolTipoParcela= '" & BolTipoParcela & "' ")

                .AppendLine("if exists(select 1 from TbBoletosImp r where r.ResId = @ResId and r.BolTipo = 'B' and r.BolImpTipoParcela = @BolTipoParcela and exists(select 1 from TbIntegranteBoleto ib where ib.IntId in(" & Integrante & ") and ib.BolImpId = r.BolImpId)) ")
                .AppendLine("Begin")
                .AppendLine("  if exists( ")
                .AppendLine("  select top 1 1 from TbBoletosImp b  ")
                .AppendLine("  where b.ResId  = @ResId ")
                .AppendLine("  And b.BolTipo = 'B' ")
                .AppendLine("  And b.BolImpTipoParcela = @BolTipoParcela  ")
                .AppendLine("  and b.BolImpPagou = 'N' order by b.BolImpId desc ) ")
                .AppendLine("     Begin ")
                .AppendLine("       Set @BolImpId = (select top 1 b.BolImpId from TbBoletosImp b  ")
                .AppendLine("                       inner join TbIntegranteBoleto i on i.BolImpId = b.BolImpId ")
                .AppendLine("                       where b.ResId  = @ResId ")
                .AppendLine("                       and i.IntId  in(" & Integrante & ") ")
                .AppendLine("                       And b.BolImpTipoParcela = @BolTipoParcela ")
                .AppendLine("                       and b.BolImpPagou = 'N' order by b.BolImpId desc ) ")

                .AppendLine("       Update TbBoletosImp set BolStatusRemessaCaixa = '00', BolImpTipoParcela = @BolTipoParcela ")
                .AppendLine("       where bolImpId = @BolImpId and (BolStatusRemessaCaixa is null or BolStatusRemessaCaixa = '') ")
                .AppendLine("       And BolTipo = 'B' ")

                .AppendLine("       Select '1' +  @BolImpId  goto saida ") 'Select 1  goto saida ")
                .AppendLine("     end ")
                .AppendLine("End ")
                .AppendLine("else ")
                .AppendLine("  Select 2 goto saida") 'Nunca foi gerado boleto, será gerado agora
                .AppendLine("  saida: ")

            End With
            Dim resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    Public Function ApagaBoletosSemRegistros(bolImpId As String, Destino As String) As Long
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If

            Dim sSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With sSql
                .AppendLine("IF EXISTS(SELECT 1 FROM TbBoletosImp WHERE BolImpId = '" & bolImpId & "' AND BolStatusRemessaCaixa = '00') ")
                .AppendLine("BEGIN ")
                .AppendLine("    DELETE FROM TbIntegranteBoleto WHERE BolImpId = '" & bolImpId & "' ")
                .AppendLine("    DELETE FROM TbBoletosImp WHERE BolImpId = '" & bolImpId & "'  AND BolStatusRemessaCaixa = '00' ")
                .AppendLine("    DELETE FROM tbBoletosReimpressao WHERE BolImpId = '" & bolImpId & "' ")
                .AppendLine("    SELECT 1 GOTO SAIDA ")
                .AppendLine("END ")
                .AppendLine("IF @@ERROR > 0 ")
                .AppendLine("  BEGIN ")
                .AppendLine("   SELECT 0 GOTO SAIDA ")
                .AppendLine("  END ")
                .AppendLine("ELSE ")
                .AppendLine("   SELECT 1 GOTO SAIDA ")
                .AppendLine("SAIDA: ")
            End With
            Dim Resultado = CLng(varConn.executaTransacional(sSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString)
        End Try
    End Function

    Public Function VerificaExistenciaBoletoGrupo(ResId As Integer, Destino As String) As Long
        If Destino = "C" Then
            varConn = New Conexao("TurismoSocialCaldas")
        Else
            varConn = New Conexao("TurismoSocialPiri")
        End If
        Dim sSql As New Text.StringBuilder("SET NOCOUNT ON ")
        With sSql
            .AppendLine("IF EXISTS(SELECT TOP 1 1 FROM TbBoletosImp WHERE BolImpTipoParcela = 'P' AND ResId = " & ResId & " and LEN(BolImpCGC_CPF) = 14 and BolStatusRemessaCaixa in ('02','90')) ")
            .AppendLine("  BEGIN ")
            .AppendLine("      SELECT TOP 1 bolImpId FROM TbBoletosImp WHERE BolImpTipoParcela = 'P' AND ResId = " & ResId & " and LEN(BolImpCGC_CPF) = 14 and BolStatusRemessaCaixa in ('02','90') GOTO SAIDA ")
            .AppendLine("  END ")
            .AppendLine("ELSE IF EXISTS(SELECT 1 FROM TbBoletosImp WHERE BolImpTipoParcela = 'P' AND ResId = " & ResId & "  and LEN(BolImpCGC_CPF) = 14 and BolStatusRemessaCaixa = '00') ")
            .AppendLine("   BEGIN  ")
            .AppendLine("      DELETE FROM TbBoletosImp WHERE BolImpTipoParcela = 'P' AND ResId = " & ResId & " and LEN(BolImpCGC_CPF) = 14 and BolStatusRemessaCaixa = '00' ")
            .AppendLine("      SELECT 0 GOTO SAIDA ")
            .AppendLine("   END ")
            .AppendLine("ELSE ")
            .AppendLine("      SELECT 0 GOTO SAIDA ")
            .AppendLine("  IF @@ERROR > 0 ")
            .AppendLine("      SELECT 0 GOTO SAIDA ")
            .AppendLine("SAIDA: ")
        End With
        Dim Resultado = CLng(varConn.executaTransacional(sSql.ToString))
        Return Resultado
    End Function

    Public Function ConfirmaGeracaoBoletoIndividual(Integrante As String, Boleto As String, Destino As String, ResId As Integer) As Long
        If Destino = "C" Then
            varConn = New Conexao("TurismoSocialCaldas")
        Else
            varConn = New Conexao("TurismoSocialPiri")
        End If
        Dim sSql As New Text.StringBuilder("SET NOCOUNT ON ")
        With sSql
            '.AppendLine("IF EXISTS(SELECT 1 FROM TbIntegranteBoleto WHERE IntId = " & Integrante & " and BolImpId = " & Boleto & ") ")
            '.AppendLine("  BEGIN ")
            '.AppendLine("      SELECT 1 GOTO SAIDA ")
            '.AppendLine("  END ")
            '.AppendLine("ELSE  ")
            '.AppendLine("   BEGIN  ")
            '.AppendLine("      SELECT 0 GOTO SAIDA ")
            '.AppendLine("   END ")
            .AppendLine("Declare @TbIntegrante table(IId Numeric) ")


            .AppendLine("  declare @Cont integer,@Aux integer,@Integrantes varchar(3000) ")
            .AppendLine("  set @Integrantes = '" & Integrante & "' ")
            .AppendLine("  set @Cont = 1 ")
            .AppendLine("  if (@Integrantes > ' ') and (@Integrantes <> '.') ")
            .AppendLine("  begin ")
            .AppendLine("    while Len(@Integrantes) > @Cont  ")
            .AppendLine("    begin ")
            .AppendLine("      set @Aux = (select charindex('.', @Integrantes, @Cont)) ")
            .AppendLine("      insert @TbIntegrante(IId) ")
            .AppendLine("        values(substring(@Integrantes, @Cont, @Aux - @Cont)) ")

            .AppendLine("      set @Cont = @Aux + 1 ")
            .AppendLine("    end ")
            .AppendLine("  end ")
            .AppendLine("  else if @Integrantes = '.' ")
            .AppendLine("  begin ")
            .AppendLine("    insert @TbIntegrante(IId) ")
            .AppendLine("      (select IntId from TbIntegrante where ResId = " & ResId & " )")
            .AppendLine("  end ")

            .AppendLine("  if exists(SELECT top 1 1 FROM TbIntegranteBoleto WHERE IntId in (select IntId from @TbIntegrante)) ")
            .AppendLine("     Begin ")
            .AppendLine("       select 1 goto saida  ")
            .AppendLine("	  end ")
            .AppendLine("ELSE  ")
            .AppendLine("   Begin  ")
            .AppendLine("      select 0 goto saida ")
            .AppendLine("   End ")
            .AppendLine("saida: ")
        End With
        Dim Resultado = CLng(varConn.executaTransacional(sSql.ToString))
        Return Resultado
    End Function

    Public Function ApagaBoletoLogicamente(BolImpId As String, Usuario As String, Destino As String) As Long
        Try
            If Destino = "C" Then
                varConn = New Conexao("TurismoSocialCaldas")
            Else
                varConn = New Conexao("TurismoSocialPiri")
            End If
            Dim sSql As New StringBuilder("Set Nocount on ")
            With sSql
                .AppendLine("Declare @Boleto as varchar(20), @Usuario varchar(30) ")
                .AppendLine("Set @Boleto = '" & BolImpId & "' ")
                .AppendLine("set @Usuario = '" & Usuario & "' ")

                .AppendLine("If Exists (select 1 from TbBoletosImp b where b.BolImpId = @Boleto and (b.BolStatusRemessaCaixa <> '06' or b.BolStatusRemessaCaixa is null) and BolTipo = 'B' ")
                .AppendLine(" and not exists(select 1 from TbVencimento v where v.VenNossoNumero = b.BolImpNossoNumero)) ")
                .AppendLine("   Begin ")
                .AppendLine("     Delete from TbIntegranteBoleto where BolImpId = @Boleto ")
                .AppendLine("	 --Criando Log de quem apagou, gravarei os dados no logradouro ")
                .AppendLine("	 Insert TbBoletosImpLog(BolImpId,BolImpNossoNumero,ResId,BolTipo,BolImpValor,BolImpDtVencimento,BolImpDtDocumento,BolImpDtProcessamento,BolImpPagou,BolImpImpresso,BolImpDtPagamento,BolImpEndereco,BolImpSacado,BolImpCGC_CPF,BolImpAgCodCedente,BolImpCodIpte,BolImpCodBarra,BolImpUsuario,BolAntecipacao,BolNitCartaoCredito,BolNsuCartaoCredito,BolLocalidadeCartaoCredito,BolConfirmacaoPagtoCartaoCredito,BolCodBarraNovoBoleto,BolCodDigitavelNovoBoleto,BolCodCedenteNovo,BolStatusPgtoCartaoCredito,BolValorPagoCartaoCredito,BolParcelasCartaoCredito,BolRedeCartaoCredito,BolNumeroAutorizacaoCartaoCredito,BolDataSitefCartaoCredito,BolStatusARisco,BolEnvioARisco,BolRetornoARisco,SeqId,BolStatusRemessaCaixa,BolDataRetornoRemessaCaixa,BolIdRemessaCaixa,BolImpDataCredito,BolImpLogradouro,BolImpBairro,BolImpNumero,BolImpQuadra,BolImpLote,BolImpComplemento,EstId,BolImpCEP,BolImpCidade,BolImpTipoParcela) ")
                .AppendLine("	 select BolImpId,BolImpNossoNumero,ResId,BolTipo,BolImpValor,BolImpDtVencimento,BolImpDtDocumento,BolImpDtProcessamento,BolImpPagou,BolImpImpresso,BolImpDtPagamento,BolImpEndereco,BolImpSacado,BolImpCGC_CPF,BolImpAgCodCedente,BolImpCodIpte,BolImpCodBarra,BolImpUsuario,BolAntecipacao,BolNitCartaoCredito,BolNsuCartaoCredito,BolLocalidadeCartaoCredito,BolConfirmacaoPagtoCartaoCredito,BolCodBarraNovoBoleto,BolCodDigitavelNovoBoleto,BolCodCedenteNovo,BolStatusPgtoCartaoCredito,BolValorPagoCartaoCredito,BolParcelasCartaoCredito,BolRedeCartaoCredito,BolNumeroAutorizacaoCartaoCredito,BolDataSitefCartaoCredito,BolStatusARisco,BolEnvioARisco,BolRetornoARisco,SeqId,BolStatusRemessaCaixa,BolDataRetornoRemessaCaixa,BolIdRemessaCaixa,BolImpDataCredito,@Usuario +'/' + convert(varchar(20), GetDate(),120),BolImpBairro,BolImpNumero,BolImpQuadra,BolImpLote,BolImpComplemento,EstId,BolImpCEP,BolImpCidade,BolImpTipoParcela from TbBoletosImp where BolImpId = @Boleto  ")
                .AppendLine("	 --Mudo o status da tabela original para que não apareça e que possa inserir um novo boleto ")
                .AppendLine("	 Update TbBoletosImp set BolImpTipoParcela = 'W', BolTipo = 'W' where BolImpId = @Boleto ")
                .AppendLine("    Select 1 goto saida ")
                .AppendLine("   End ")
                .AppendLine("else ")
                .AppendLine("   Begin ")
                .AppendLine("     Select 0 goto saida ")
                .AppendLine("   End ")
                .AppendLine("saida: ")
            End With
            Dim Resultado = CLng(varConn.executaTransacionalTestaRetorno(sSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), " "))
        End Try
    End Function
End Class
