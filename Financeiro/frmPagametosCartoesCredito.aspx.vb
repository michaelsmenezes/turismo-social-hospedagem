Imports Microsoft.Reporting.WebForms
Imports dtsPagtoCartaoCreditoTableAdapters
Imports Turismo

Partial Class Financeiro_frmPagametosCartoesCredito
    Inherits System.Web.UI.Page

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        divRelatorio.Visible = False
        Try
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")

            Dim Unidade As String = ""
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                oTurismo = New ConexaoDAO("DbTurismoSocial")
                Unidade = "Sesc de Caldas Novas"
            ElseIf Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" Then
                oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
                Unidade = "Pousada Sesc Pirenópolis"
            End If


            Dim OrdersRelacaoRateio As DataTableRateioTableAdapter = New DataTableRateioTableAdapter
            Dim VarSqlRateio As New Text.StringBuilder("")
            If hddRelatorio.Value = "RelatorioEdu" Then
                'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    OrdersRelacaoRateio.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
                    Unidade = "Sesc de Caldas Novas"

                    With VarSqlRateio
                        .AppendLine("SET NOCOUNT ON ")
                        .AppendLine("Declare @Data1 Date ")
                        .AppendLine("Set @Data1 = '" & Format(CDate(txtDataIni.Value), "yyyy-MM-dd") & "'")

                        .AppendLine("DECLARE @Aux TABLE( ")
                        .AppendLine("ResId int, ")
                        .AppendLine("BolImpNossoNumero Numeric(12,0), ")
                        .AppendLine("BolImpValor Numeric(18,2), ")
                        .AppendLine("resDataIni integer, ")
                        .AppendLine("bolImpDtVencimento datetime, ")
                        .AppendLine("ratD Numeric(18,2),	")
                        .AppendLine("ratA Numeric(18,2),	")
                        .AppendLine("ratJ Numeric(18,2),	")
                        .AppendLine("ratH Numeric(18,2), ")
                        .AppendLine("BolDataSitefCartaoCredito datetime ")
                        .AppendLine(") ")

                        .AppendLine("INSERT @Aux ")
                        .AppendLine("SELECT distinct B.ResId,B.BolImpNossoNumero,B.BolImpValor,MONTH(RE.ResDataIni) AS resDataIni,CAST(B.bolImpDtVencimento as Date)bolImpDtVencimento, ")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'D' ),0)*100.00 AS ratD,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'A' AND NOT EXISTS(SELECT TOP 1 1 FROM TbRateio R1 WHERE R1.venId = V.venId AND R1.ratTipo = 'P')),0)* 100 AS ratA,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'J'),0)*100.00 AS ratJ,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'H'),0)*100.00 AS ratH, ")
                        .AppendLine("CAST(b.BolDataSitefCartaoCredito as DATE)BolDataSitefCartaoCredito ")
                        .AppendLine("FROM TbBoletosImp B ")
                        .AppendLine("INNER JOIN TbReserva RE ON RE.resId = B.resId ")
                        .AppendLine("INNER JOIN TbVencimento V ON RE.resId = V.resId AND V.VenFormaPagto IN ('ER','EC') ")

                        .AppendLine("WHERE CAST(B.BolDataSitefCartaoCredito AS DATE) = @Data1 AND ((V.venStatus IN('V','T') ")
                        .AppendLine("AND BolStatusPgtoCartaoCredito = 'CON'")
                        '.AppendLine("AND BolStatusARisco = 'APR'")
                        .AppendLine("AND V.venFormaPagto IN ('ER','EC') ) OR ")
                        .AppendLine("(V.venStatus = 'T' AND NOT EXISTS(SELECT 1 FROM TbVencimento VE ")
                        .AppendLine("WHERE VE.resId = V.resId AND VE.VenStatus = 'V' ))) ")
                        .AppendLine("ORDER BY MONTH(RE.ResDataIni) Asc ")
                        .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01')) ")

                        .AppendLine("SELECT DISTINCT ResId,BolImpNossoNumero,BolImpValor,resDataIni,bolImpDtVencimento, ")
                        .AppendLine("MAX(ratD) ratD,MAX(ratA) ratA,MAX(ratJ) ratJ, BolImpValor - (MAX(ratD) + MAX(ratA) + MAX(ratJ))ratH,BolDataSitefCartaoCredito FROM @Aux  ")
                        .AppendLine("GROUP BY resid,BolImpNossoNumero,BolImpValor,resDataIni,bolImpDtVencimento,BolDataSitefCartaoCredito ")
                        .AppendLine("ORDER BY resDataIni ")
                    End With
                Else
                    OrdersRelacaoRateio.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
                    Unidade = "Pousada Sesc Pirenópolis"
                    With VarSqlRateio
                        .AppendLine("SET NOCOUNT ON ")
                        .AppendLine("Declare @Data1 Date ")
                        .AppendLine("Set @Data1 = '" & Format(CDate(txtDataIni.Value), "yyyy-MM-dd") & "'")

                        .AppendLine("DECLARE @Aux TABLE( ")
                        .AppendLine("ResId int, ")
                        .AppendLine("BolImpNossoNumero Numeric(12,0), ")
                        .AppendLine("BolImpValor Numeric(18,2), ")
                        .AppendLine("resDataIni integer, ")
                        .AppendLine("bolImpDtVencimento datetime, ")
                        .AppendLine("ratD Numeric(18,2),	")
                        .AppendLine("ratA Numeric(18,2),	")
                        .AppendLine("ratJ Numeric(18,2),	")
                        .AppendLine("ratH Numeric(18,2), ")
                        .AppendLine("BolDataSitefCartaoCredito datetime ")
                        .AppendLine(") ")

                        .AppendLine("INSERT @Aux ")
                        .AppendLine("SELECT distinct B.ResId,B.BolImpNossoNumero,B.BolImpValor,MONTH(RE.ResDataIni) AS resDataIni,CAST(B.bolImpDtVencimento as Date)bolImpDtVencimento, ")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'D' ),0)*100.00 AS ratD,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'A' AND NOT EXISTS(SELECT TOP 1 1 FROM TbRateio R1 WHERE R1.venId = V.venId AND R1.ratTipo = 'P')),0)* 100 AS ratA,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'J'),0)*100.00 AS ratJ,")
                        .AppendLine("ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto IN ('ER','EC') AND RT.ratTipo = 'H'),0)*100.00 AS ratH, ")
                        .AppendLine("CAST(b.BolDataSitefCartaoCredito as DATE)BolDataSitefCartaoCredito ")
                        .AppendLine("FROM TbBoletosImp B ")
                        .AppendLine("INNER JOIN TbReserva RE ON RE.resId = B.resId ")
                        .AppendLine("INNER JOIN TbVencimento V ON RE.resId = V.resId AND V.VenFormaPagto in ('ER','EC') ")

                        .AppendLine("WHERE CAST(B.BolDataSitefCartaoCredito AS DATE) = @Data1 AND ((V.venStatus IN('V','T') ")
                        .AppendLine("AND BolStatusPgtoCartaoCredito = 'CON'")
                        '.AppendLine("AND BolStatusARisco = 'APR'")
                        .AppendLine("AND V.venFormaPagto IN ('ER','EC'))) OR ")
                        .AppendLine("(V.venStatus = 'T' AND NOT EXISTS(SELECT 1 FROM TbVencimento VE ")
                        .AppendLine("WHERE VE.resId = V.resId AND VE.VenStatus = 'V' )) ")
                        .AppendLine("ORDER BY MONTH(RE.ResDataIni) Asc ")
                        .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01')) ")

                        .AppendLine("SELECT DISTINCT ResId,BolImpNossoNumero,BolImpValor,resDataIni,bolImpDtVencimento, ")
                        .AppendLine("MAX(ratD) ratD,MAX(ratA) ratA,MAX(ratJ) ratJ, BolImpValor - (MAX(ratD) + MAX(ratA) + MAX(ratJ))ratH,BolDataSitefCartaoCredito FROM @Aux  ")
                        .AppendLine("GROUP BY resid,BolImpNossoNumero,BolImpValor,resDataIni,bolImpDtVencimento,BolDataSitefCartaoCredito ")
                        .AppendLine("ORDER BY resDataIni ")
                    End With
                End If

                Dim nomeUsuario As String = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")

                'Passando os parâmetros geração do relatório'
                Dim ParamsSaidaProdutos(2) As ReportParameter
                ParamsSaidaProdutos(0) = New ReportParameter("unidade", Unidade)
                ParamsSaidaProdutos(1) = New ReportParameter("periodo", "Movimento do dia " & Format(CDate(txtDataIni.Value), "dd/MM/yyyy")) ' & " até " & Format(CDate(txtDataFim .Value), "dd/MM/yyyy"))
                ParamsSaidaProdutos(2) = New ReportParameter("usuario", nomeUsuario)
                Dim ordersRelacaoDosRateios As System.Data.DataTable = OrdersRelacaoRateio.GetData(VarSqlRateio.ToString)
                Dim rdsRelRateios As New ReportDataSource("DtsRateios", ordersRelacaoDosRateios)

                'Chamando o relatório' 
                divRelatorio.Visible = True
                rptVendas.Visible = True
                rptVendas.LocalReport.DataSources.Clear()
                rptVendas.LocalReport.DataSources.Add(rdsRelRateios)
                rptVendas.LocalReport.SetParameters(ParamsSaidaProdutos)
            End If

            '============================== RELATÓRIO DE RATEIO========================================
            If hddRelatorio.Value = "Rateio" Then
                divRelatorio.Visible = False
                Dim OrdersRelacaoRateioResumido As DataTableRateioTableAdapter = New DataTableRateioTableAdapter
                'Dim Unidade As String = ""
                'Dim VarSqlRateio As New Text.StringBuilder("")
                'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    oTurismo = New ConexaoDAO("DbTurismoSocial")
                    OrdersRelacaoRateioResumido.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
                    Unidade = "Sesc de Caldas Novas"
                ElseIf Session("MasterPage").ToString = "~/TurismoSocialPiri.Master" Then
                    oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
                    OrdersRelacaoRateioResumido.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
                    Unidade = "Pousada Sesc Pirenópolis"
                Else
                    Mensagem("Pirenópolis ainda em desenvolvimento.")
                    'Por enquanto não estou fazendo Pirenópolis por isso irei abortar aqui
                    Exit Try
                End If

                With VarSqlRateio
                    .AppendLine("SET NOCOUNT ON ")

                    .AppendLine("--Vou gerar o processo de inserção para os tipos de reserva ")
                    .AppendLine("--I - Individual ")
                    .AppendLine("--P - Passeio ")
                    .AppendLine("--E - Excursão ")

                    .AppendLine("SET NOCOUNT ON ")
                    .AppendLine("Declare @tbBase Table( ")
                    .AppendLine("   BolImpId Decimal(18,0), ")
                    .AppendLine("   BolImpValor Decimal(18,2), ")
                    .AppendLine("   ResDataIni integer, ")
                    .AppendLine("   Desjejum decimal(18,2), ")
                    .AppendLine("   Almoco decimal(18,2), ")
                    .AppendLine("   Jantar decimal(18,2), ")
                    .AppendLine("   Hospedagem decimal(18,2), ")
                    .AppendLine("   ResCaracteristica varchar(3), ")
                    .AppendLine("   DescricaoPasseio varchar(200), ")
                    .AppendLine("   CaracteristicaOrigem varchar(3), ")
                    .AppendLine("   AlmocoPasseio decimal(18,2), ")
                    .AppendLine("   PAquaticoPasseio decimal(18,2), ")
                    .AppendLine("   PTurismoEmissivo decimal(18,2) ")
                    .AppendLine(") ")
                    .AppendLine("Declare @tbFinal Table( ")
                    .AppendLine("   BolImpValor Decimal(18,2), ")
                    .AppendLine("   ResDataIni integer, ")
                    .AppendLine("   TipoRefeicao Varchar(3), ")
                    .AppendLine("   Valor decimal(18,2), ")
                    .AppendLine("   DescricaoPasseio varchar(300), ")
                    .AppendLine("   ResCaracteristica varchar(3), ")
                    .AppendLine("   CaracteristicaOrigem varchar(3), ")
                    .AppendLine("   Ordem Numeric(2,0) ")
                    .AppendLine(") ")

                    .AppendLine("Declare @Resultado Table( ")
                    .AppendLine("    conConta varchar(10), ")
                    .AppendLine("    modCodigo integer, ")
                    .AppendLine("    modModeloA integer, ")
                    .AppendLine("    conDescricao varchar(300), ")
                    .AppendLine("    bolImpValor Decimal(18,2), ")
                    .AppendLine("    ResDataIni Decimal(18,2), --integer, ")
                    .AppendLine("    TipoRefeicao varchar(3), ")
                    .AppendLine("    Valor Decimal(18,2), ")
                    .AppendLine("	ResCaracteristica varchar(3), ")
                    .AppendLine("	CaracteristicaOrigem varchar(3), ")
                    .AppendLine("	Ordem Numeric(2,0), ")
                    .AppendLine("	TipoMes Numeric(1,0) ")
                    .AppendLine(") ")

                    .AppendLine("Declare @Data1 Date, @Data2 Date,@Tipo char(1), @Contador int,@MaxContador Int,@ResCaracteristica char(10) ")
                    .AppendLine("--Vou passar 3 vezes no processo para pegar todos os tipos I,P,E ")
                    .AppendLine("Set @Contador = 1 ")
                    .AppendLine("Set @MaxContador = 9 ")
                    .AppendLine("--Set @ResCaracteristica = '' ")

                    .AppendLine("Set @Tipo = '" & drpTipo.SelectedValue & "' ")
                    .AppendLine("--Set @Data1 = '" & Format(txtDataIni.Value, "yyyy-MM-dd") & "' '2018-01-17' --12 teve EE, 15 teve EE, 22 teve EE -- 24 teve PC 31 teve EE ")
                    .AppendLine("Set @Data1 = '" & Format(CDate(txtDataIni.Value), "yyyy-MM-dd") & "' ")

                    .AppendLine("	Declare @D Numeric(18,2), @A Numeric(18,2),@J Numeric(18,2),@H Decimal(18,2) ")
                    .AppendLine("	Insert @tbBase(BolImpId,BolImpValor,ResDataIni,Desjejum,Almoco,Jantar,Hospedagem,AlmocoPasseio,PAquaticoPasseio,PTurismoEmissivo,ResCaracteristica,DescricaoPasseio,CaracteristicaOrigem) ")
                    .AppendLine("		SELECT Distinct B.BolImpId,B.BolImpValor, ")
                    .AppendLine("		case when MONTH(RE.ResDataIni) = MONTH(GETDATE()) then '0' else MONTH(RE.ResDataIni) end resDataIni, ")
                    .AppendLine("		--Desjejum ")
                    .AppendLine("		  ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'D' ),0)*100.00 as ratD, ")
                    .AppendLine("		--Almoço ")
                    .AppendLine("		  ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'A'),0)* 100 as ratA, ")
                    .AppendLine("		--Jantar ")
                    .AppendLine("		  ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'J'),0)*100.00 as ratJ, ")
                    .AppendLine("		--Hospedagem ")
                    .AppendLine("		  0.00, ")
                    .AppendLine("		--Almoço Passeio ")

                    .AppendLine("       ISNULL((select top 1 rf.ValorPrato from TbRefeicaoPratoValor rf where CatId = 1),0) AS ratAP, ")
                    .AppendLine("		--Parque Aquatico ")
                    .AppendLine("		ISNULL((SELECT top 1 rf.ValorPqAquatico from TbRefeicaoPratoValor rf where rf.CatId = 1),0) as RefPasseio, ")
                    .AppendLine("		--Turismo Emissivo (Que sobra da refeição + Parque Aquatico 'Iremar') ")
                    .AppendLine("		case ")
                    .AppendLine("		   --Excursão Caldas Novas ")

                    .AppendLine("          WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and (re.ResColoniaFeriasDes = 'S' or re.ResCidadeDes = 'CALDAS NOVAS') and re.ResPasseioPromovidoCEREC = 'S' then ")
                    .AppendLine("		       ISNULL(B.BolImpValor - ((ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'A'),0)* 100) + ISNULL((SELECT top 1 rf.ValorPqAquatico from TbRefeicaoPratoValor rf where rf.CatId = re.Catid),0)),0) ")
                    .AppendLine("		   --Excursão outras cidades ")
                    .AppendLine("		   WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and re.ResColoniaFeriasDes <> 'S' and re.ResPasseioPromovidoCEREC = 'S' then ")
                    .AppendLine("			   --ISNULL(B.BolImpValor - ((ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'A'),0)* 100) + ISNULL((SELECT top 1 rf.ValorPqAquatico from TbRefeicaoPratoValor rf where rf.CatId = re.Catid),0)),0) ")
                    .AppendLine("			   ISNULL(B.BolImpValor,0) ")
                    .AppendLine("           --Passeio Caldas Novas ")
                    .AppendLine("		   WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) = cast(re.ResDataFim as date) and re.ResColoniaFeriasDes <> '0' and re.ResPasseioPromovidoCEREC = 'S' then  ")
                    .AppendLine("		       ISNULL(B.BolImpValor - ((ISNULL((SELECT  TOP 1 RT.ratValor * (((CAST(B.BolImpValor AS NUMERIC(18,2))/100.00))/V.VenValor) FROM TbRateio RT WHERE RT.venId = V.venId AND V.venStatus = 'V' AND V.venFormaPagto in ('ER','EC') AND RT.ratTipo = 'A'),0)* 100) + ISNULL((SELECT top 1 rf.ValorPqAquatico from TbRefeicaoPratoValor rf where rf.CatId = re.Catid),0)),0) ")
                    .AppendLine("		end	AS ratP, ")
                    .AppendLine("		Re.ResCaracteristica, ")
                    .AppendLine("		LTRIM(RTRIM(re.ResNome)) + ' NO DIA ' + CONVERT(CHAR(10),re.ResDataIni,103) DescricaoPasseio, ")
                    .AppendLine("		CASE WHEN RE.ResCaracteristica = 'I' then 'I' --Individual ")
                    .AppendLine("		     --Excursão Outras Cidades = EC ")
                    .AppendLine("			 WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and re.ResColoniaFeriasDes <> 'S' and re.ResCidadeDes <> 'CALDAS NOVAS' and re.ResPasseioPromovidoCEREC = 'S' then 'EC' ")
                    .AppendLine("			 --Excursão outros estados = EE ")
                    .AppendLine("			 WHEN ((RE.ResTipo = 'P' AND RE.EstIdDes <> '9') or ((re.ResTipo = 'E' or re.ResTipo = 'T') and re.EstId <> 9)) THEN 'EE' ")
                    .AppendLine("			 --Passeio Outras Cidades = PC ")
                    .AppendLine("			 WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) = cast(re.ResDataFim as date) and re.ResColoniaFeriasDes = '0' and re.ResPasseioPromovidoCEREC = 'S' then 'PC' ")
                    .AppendLine("		     --*****Excursão Caldas Novas ")
                    .AppendLine("			 WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and re.ResColoniaFeriasDes = 'S' and re.ResPasseioPromovidoCEREC = 'S' then 'EN' ")
                    .AppendLine("------------Quando for excursões organizadas pelo Sesc, Entrará nos elementos de excursões")
                    .AppendLine("            WHEN RE.ResTipo = 'E' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and re.ResColoniaFeriasDes = 'S' and re.ResPasseioPromovidoCEREC = 'S' then 'EN' ")
                    .AppendLine("------------Quando não for excursões organizadas pelo Sesc, será contabilizado como reservas normais")
                    .AppendLine("            WHEN RE.ResTipo = 'E' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) < cast(re.ResDataFim as date) and re.ResColoniaFeriasDes = 'S' and re.ResPasseioPromovidoCEREC = 'N' then 'I' ")
                    .AppendLine("			 --****Passeio Caldas Novas ")
                    .AppendLine("			 WHEN RE.ResTipo = 'P' and re.EstIdDes = 9 and cast(re.ResDataIni as date ) = cast(re.ResDataFim as date) and re.ResColoniaFeriasDes <> '0' and re.ResPasseioPromovidoCEREC = 'S' then 'PN' ")
                    .AppendLine("		ELSE ")
                    .AppendLine("		     'CC' ")
                    .AppendLine("		END ")

                    .AppendLine("		FROM TbBoletosImp B ")
                    .AppendLine("		INNER JOIN TbReserva RE ON RE.resId = B.resId ")
                    .AppendLine("		INNER JOIN TbVencimento V ON RE.resId = V.resId AND V.VenFormaPagto in ('ER','EC') ")
                    .AppendLine("		--Se for exibir pgto no boleto ")
                    If drpTipo.SelectedValue = "B" Then
                        .AppendLine("		where CAST(B.BolImpDataCredito AS DATE) = @Data1 ")
                    ElseIf drpTipo.SelectedValue = "T" Then
                        .AppendLine("		where CAST(B.BolImpDtPagamento AS DATE) = @Data1 ")
                    End If
                    .AppendLine("		AND (V.venStatus IN('V') ")
                    .AppendLine("		AND V.venFormaPagto in ('ER','EC')) ")
                    .AppendLine("		and B.BolTipo = @Tipo ")
                    .AppendLine("		OPTION (OPTIMIZE FOR(@Data1='2012-01-01')) ")

                    .AppendLine("		--Atualizando o valor da hospedagem ")
                    .AppendLine("		UPDATE @tbBase SET Hospedagem = BolImpValor - (Desjejum + Almoco + Jantar) ")
                    .AppendLine("       Update @tbBase SET PTurismoEmissivo = BolImpValor - (AlmocoPasseio + PAquaticoPasseio) where CaracteristicaOrigem = 'PN' ")
                    .AppendLine("		--select * from @tbBase order by ResDataIni ")

                    'Quando ocorre de definir o pagamento no check in e o cliente vai e paga no cartão esta duplicando o bolImpId
                    'Esse delete irá deixar somente o maior valor de refeição no rateio.
                    .AppendLine("delete from @tbBase ")
                    .AppendLine("where BolImpId in ")
                    .AppendLine("  (select BolImpId from @tbBase ")
                    .AppendLine("   group by BolImpId ")
                    .AppendLine("   having Count(BolImpId)>1) ")
                    .AppendLine("and not Almoco  in ")
                    .AppendLine("  (select Min(Almoco) from @tbBase ")
                    .AppendLine("   group by BolImpId ")
                    .AppendLine("   having Count(BolImpId)>1) ")



                    .AppendLine("While @Contador <= @MaxContador ")
                    .AppendLine("  Begin ")
                    .AppendLine("		--Definindo qua o tipo de reseva irei pegar em cada laço ")
                    .AppendLine("        Set @ResCaracteristica = ")
                    .AppendLine("		(Select case ")
                    .AppendLine("		     when @Contador = 1 then 'I' --Reservas Individuais -- Tem Refeições ")
                    .AppendLine("			 when @Contador = 2 then 'EC' --Excursão outras cidades ")
                    .AppendLine("			 when @Contador = 3 then 'EE' --Excursão outros Estados ")
                    .AppendLine("			 when @Contador = 4 then 'PC' --Passeio outras cidades ")
                    .AppendLine("			 when @Contador = 5 then 'PN' --Passeio Caldas Novas - Pode haver refeições ")
                    .AppendLine("			 when @Contador = 6 then 'EN' --Excursão Caldas Novas - Tem Refeições ")
                    .AppendLine("			 when @Contador = 7 then 'EP' --Excursão Pirenopolis ")
                    .AppendLine("			 when @Contador = 8 then 'PP' --Passeio Pirenopolis ")
                    .AppendLine("			 when @Contador = 9 then 'RC' --Recreação Pirenopolis ")
                    .AppendLine("          end) ")

                    .AppendLine("     --*********Somente reservas individuas ********** ")
                    .AppendLine("	 if @ResCaracteristica in ('I','EN','TN','PN') ")
                    .AppendLine("	   BEGIN ")
                    .AppendLine("	       IF @ResCaracteristica IN ('I','EN','TN') ")
                    .AppendLine("		     BEGIN ")
                    .AppendLine("	   				--Botando o resultado das refeição na horizontal ")
                    .AppendLine("				insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem, Ordem,DescricaoPasseio) ")
                    .AppendLine("				SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'D',sum(b.Desjejum)D,@ResCaracteristica,b.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end,max(b.DescricaoPasseio)   ")
                    .AppendLine("				FROM @tbBase b ")
                    .AppendLine("				where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("				group by b.ResDataIni,B.CaracteristicaOrigem ")

                    .AppendLine("				insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,DescricaoPasseio) ")
                    .AppendLine("				SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'R',sum(b.Almoco) + sum(b.Jantar)R,@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end,max(b.DescricaoPasseio) ")
                    .AppendLine("				FROM @tbBase b ")
                    .AppendLine("				where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("				group by b.ResDataIni,B.CaracteristicaOrigem ")

                    .AppendLine("				insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,DescricaoPasseio) ")
                    .AppendLine("				SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'H',sum(b.Hospedagem)H,@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end,max(b.DescricaoPasseio) ")
                    .AppendLine("				FROM @tbBase b ")
                    .AppendLine("				where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("				group by b.ResDataIni,B.CaracteristicaOrigem ")
                    .AppendLine("            END ")

                    .AppendLine("           IF @ResCaracteristica = 'PN' ")
                    .AppendLine("		      BEGIN ")
                    .AppendLine("			        --REFEIÇÃO ")
                    .AppendLine("					insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,DescricaoPasseio,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem) ")
                    .AppendLine("					SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'R',B.DescricaoPasseio,ISNULL(sum(b.AlmocoPasseio),0)A,@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end ")
                    .AppendLine("					FROM @tbBase b ")
                    .AppendLine("					where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("					group by b.ResDataIni,B.CaracteristicaOrigem,B.DescricaoPasseio ")

                    .AppendLine("					--RECREACAO ")
                    .AppendLine("					insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,DescricaoPasseio,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem) ")
                    .AppendLine("					SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'REC',B.DescricaoPasseio,ISNULL(sum(b.PAquaticoPasseio),0)REC,@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end ")
                    .AppendLine("					FROM @tbBase b ")
                    .AppendLine("					where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("					group by b.ResDataIni,B.CaracteristicaOrigem,B.DescricaoPasseio ")

                    .AppendLine("					--TURISMO EMISSIVO ")
                    .AppendLine("					insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,DescricaoPasseio,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem) ")
                    .AppendLine("					SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'TEM',B.DescricaoPasseio,ISNULL(sum(PTurismoEmissivo),0)TEM,@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '1' end ")
                    .AppendLine("					FROM @tbBase b ")
                    .AppendLine("					where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("					group by b.ResDataIni,B.CaracteristicaOrigem,B.DescricaoPasseio ")
                    .AppendLine("			  END ")
                    .AppendLine("		END ")
                    .AppendLine("	   ELSE IF @ResCaracteristica in ('PC') ")
                    .AppendLine("	    BEGIN ")
                    .AppendLine("   		  insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,DescricaoPasseio) ")
                    .AppendLine("		  SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'TEM',sum(b.BolImpValor),@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '2' end,b.DescricaoPasseio ")
                    .AppendLine("		    FROM @tbBase b ")
                    .AppendLine("		    where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("		    group by b.ResDataIni,B.CaracteristicaOrigem,b.DescricaoPasseio ")
                    .AppendLine("		END ")
                    .AppendLine("      ELSE IF @ResCaracteristica in ('EE') ")
                    .AppendLine("	    BEGIN ")
                    .AppendLine("   		  insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,DescricaoPasseio) ")
                    .AppendLine("		  SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'TEM',sum(b.BolImpValor),@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '3' end,b.DescricaoPasseio ")
                    .AppendLine("		    FROM @tbBase b ")
                    .AppendLine("		    where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("		    group by b.ResDataIni,B.CaracteristicaOrigem,b.DescricaoPasseio ")
                    .AppendLine("		END ")
                    .AppendLine("	  ELSE IF @ResCaracteristica in ('EC') ")
                    .AppendLine("	    BEGIN ")
                    .AppendLine("   		  insert @tbFinal(BolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,DescricaoPasseio) ")
                    .AppendLine("		  SELECT Sum(b.BolImpValor)BolImpValor,b.ResDataIni,'TEM',sum(b.BolImpValor),@ResCaracteristica,B.CaracteristicaOrigem,case when ResDataIni = 0 then '0' else '3' end,b.DescricaoPasseio ")
                    .AppendLine("		    FROM @tbBase b ")
                    .AppendLine("		    where b.CaracteristicaOrigem = @ResCaracteristica ")
                    .AppendLine("		    group by b.ResDataIni,B.CaracteristicaOrigem,b.DescricaoPasseio ")
                    .AppendLine("		END ")
                    .AppendLine("	Set @Contador += 1 ")
                    .AppendLine("End ")

                    .AppendLine("        --select * from @tbFinal order by ResDataIni ")

                    .AppendLine(" 		insert @Resultado(conConta,modCodigo,modModeloA,conDescricao,bolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,TipoMes) ")
                    .AppendLine("		Select c.conConta,m.modCodigo,m.ModModeloA, ")
                    .AppendLine("       CASE WHEN F.ResCaracteristica = 'I' THEN c.conDescricao ")

                    .AppendLine("           ELSE f.DescricaoPasseio END, ")
                    .AppendLine("		f.BolImpValor,f.ResDataIni,f.TipoRefeicao,f.Valor,f.ResCaracteristica,f.CaracteristicaOrigem,f.Ordem,c.ConTipoMes ")
                    .AppendLine("		from @tbFinal f ")
                    .AppendLine("		inner join tbContaContabilFinanceiro c on c.conAtividade = f.TipoRefeicao --and SUBSTRING(c.conCaracteristicaOrigem,3,1) <> '0' --ok EE = EE ")
                    .AppendLine("		inner join tbModalidadeReceitaApropriar m on m.ModMes = f.ResDataIni ")
                    .AppendLine("		and f.CaracteristicaOrigem = m.ModCaracteristicaOrigem ")
                    .AppendLine("		and f.TipoRefeicao = m.modElemento ")
                    .AppendLine("		and f.ResCaracteristica = c.conCaracteristicaOrigem ")
                    .AppendLine("		and c.ConTipoMes = m.ModTipoMes ")
                    .AppendLine("		and f.ResCaracteristica = 'I' ")

                    .AppendLine("       if exists(select 1 from @tbFinal where Ordem > 0) ")
                    .AppendLine("          BEGIN ")
                    .AppendLine("             insert @Resultado(Ordem,ResCaracteristica,ResDataIni,TipoRefeicao,conDescricao) valueS (0,'W',0.5,'','CONTABILIZAÇÃO DAS RECEITAS SICOBS A APROPRIAR') ")
                    .AppendLine("          END ")

                    .AppendLine("		insert @Resultado(conConta,modCodigo,modModeloA,conDescricao,bolImpValor,ResDataIni,TipoRefeicao,Valor,ResCaracteristica,CaracteristicaOrigem,Ordem,TipoMes) ")
                    .AppendLine("		Select c.conConta,m.modCodigo,m.ModModeloA,CASE WHEN F.ResCaracteristica = 'I' THEN c.conDescricao ELSE f.DescricaoPasseio END, ")
                    .AppendLine("		f.BolImpValor,f.ResDataIni,f.TipoRefeicao,f.Valor,f.ResCaracteristica,f.CaracteristicaOrigem,f.Ordem,c.ConTipoMes ")
                    .AppendLine("		from @tbFinal f ")
                    .AppendLine("		inner join tbContaContabilFinanceiro c on c.conAtividade = f.TipoRefeicao --and SUBSTRING(c.conCaracteristicaOrigem,3,1) <> '0' --ok EE = EE ")
                    .AppendLine("		inner join tbModalidadeReceitaApropriar m on m.ModMes = f.ResDataIni ")
                    .AppendLine("		and f.CaracteristicaOrigem = m.ModCaracteristicaOrigem ")
                    .AppendLine("		and f.TipoRefeicao = m.modElemento ")
                    .AppendLine("		and f.ResCaracteristica = c.conCaracteristicaOrigem ")
                    .AppendLine("		and c.ConTipoMes = m.ModTipoMes ")
                    .AppendLine("		and f.ResCaracteristica <> 'I' ")

                    .AppendLine("       select * from @Resultado order by ordem, ResCaracteristica,ResDataIni,TipoRefeicao ")
                End With

                Dim nomeUsuario As String = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")

                'Passando os parâmetros'
                Dim ParamsRateios(3) As ReportParameter
                ParamsRateios(0) = New ReportParameter("unidade", Unidade)
                If drpTipo.SelectedValue = "T" Then
                    ParamsRateios(1) = New ReportParameter("periodo", "Movimento do dia " & Format(CDate(txtDataIni.Value), "dd/MM/yyyy")) ' & " até " & Format(CDate(txtDataFim .Value), "dd/MM/yyyy"))
                Else
                    ParamsRateios(1) = New ReportParameter("periodo", "Créditado no dia " & Format(CDate(txtDataIni.Value), "dd/MM/yyyy")) ' & " até " & Format(CDate(txtDataFim .Value), "dd/MM/yyyy"))
                End If
                'ParamsRateios(1) = New ReportParameter("periodo", "Movimento do dia " & Format(CDate(txtDataIni.Value), "dd/MM/yyyy")) ' & " até " & Format(CDate(txtDataFim .Value), "dd/MM/yyyy"))
                ParamsRateios(2) = New ReportParameter("usuario", nomeUsuario)
                If drpTipo.SelectedValue = "B" Then
                    ParamsRateios(3) = New ReportParameter("Titulo", "RELATÓRIO COM RECEBIMENTO EM BOLETOS")
                Else
                    ParamsRateios(3) = New ReportParameter("Titulo", "RELATÓRIO COM RECEBIMENTO EM CARTÕES DE CRÉDITO")
                End If

                Dim ordersRelacaoDosRateiosResumidos As System.Data.DataTable = OrdersRelacaoRateioResumido.GetData(VarSqlRateio.ToString)
                Dim rdsRelRateiosResumido As New ReportDataSource("DtsRateioResumido", ordersRelacaoDosRateiosResumidos)

                'Chamando o relatório' 
                divRelatorio.Visible = False
                divRelRateio.Visible = True
                rptVendas.Visible = False
                rptRateios.Visible = True
                rptRateios.LocalReport.DataSources.Clear()
                rptRateios.LocalReport.DataSources.Add(rdsRelRateiosResumido)
                rptRateios.LocalReport.SetParameters(ParamsRateios)
            End If

        Catch ex As Exception
            'Mensagem("Não será possível exibir as informações solicitadas, pois não foi encontrado na base de dados informações sobre esse período.\n\nTente outro período.")
        End Try

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                lblTituloPrincipal.Text = "Relatório Financeiros - Caldas Novas"
            Else
                lblTituloPrincipal.Text = "Relatório Financeiros - Pirenópolis"
            End If
            txtDataIni.Value = Format(Now, "dd/MM/yyyy")
            'txtDataFim.Value = Format(Now, "dd/MM/yyyy")
            txtDataIni.Focus()
        End If

    End Sub
    Protected Sub btnVoltar_Click(sender As Object, e As EventArgs) Handles btnVoltar.Click
        Response.Redirect("~/Reserva.aspx")
    End Sub

    Protected Sub mnuRelatorios_MenuItemClick(sender As Object, e As MenuEventArgs) Handles mnuRelatorios.MenuItemClick
        If mnuRelatorios.SelectedValue = "RelatorioEdu" Then
            rptRateios.Visible = False
            rptVendas.Visible = False
            lblTitulo.Text = "Relatório para conferência - Central de Reservas"
            lblDataCreditoRecebimento.Text = "Data de Recebimento"
            divPagamentosCartoes.Visible = True
            divTipoRecebimento.Visible = False
            hddRelatorio.Value = "RelatorioEdu"
        ElseIf mnuRelatorios.SelectedValue = "Rateio" Then
            rptRateios.Visible = False
            rptVendas.Visible = False
            divPagamentosCartoes.Visible = True
            lblTitulo.Text = "Relatório de Rateio - Contabilidade"
            lblDataCreditoRecebimento.Text = "Data do Crédito"
            divTipoRecebimento.Visible = True
            hddRelatorio.Value = "Rateio"
        End If
    End Sub

    Protected Sub drpTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpTipo.SelectedIndexChanged
        Select Case drpTipo.SelectedValue
            Case "B"
                lblDataCreditoRecebimento.Text = "Data do Crédito"
            Case "T"
                lblDataCreditoRecebimento.Text = "Data do Recebimento"
        End Select
    End Sub
    Protected Sub Mensagem(Texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + Texto + "');", True)
    End Sub
End Class
