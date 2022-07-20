Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Text

Public Class HistoricoRefeicaoDAO
    Dim ObjHistoricoRefeicaoVO As HistoricoRefeicaoVO
    Dim ObjHistoricoRefeicaoDAO As HistoricoRefeicaoDAO
    Public Function ConsultaHistorico(ByVal ObjHistoricoRefeicaoVO As HistoricoRefeicaoVO, ByVal AliasBanco As String, ByVal Mes As String, ByVal Ano As String, ByVal TipoInsercao As String, Origem As String) As IList
        Try
            'Informação: IntId -2=Inserção Manual; -1: Servidores; 0: Menor de 5 anos
            Dim Conn = New Banco.Conexao(AliasBanco)

            Dim Data1 As String, TipoRelatorio As String
            Data1 = Format(CDate(Ano & "-" & Mes & "-01"), "yyyy-MM-dd")
            TipoRelatorio = Origem

            'Dim UnidadeOperacional As String = ""
            'If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            '    UnidadeOperacional = "SESC CALDAS NOVAS"
            'Else
            '    UnidadeOperacional = "POUSADA SESC PIRINÓPOLIS"
            'End If

            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("Declare @QtdeRefeicoes int, @Data1 DateTime,@Data2 Datetime, @TotalPassantes int,@TipoRefeicao char(1),@TipoRelatorio char(1), ")
                .AppendLine("@Com Int,@Dep Int,@Usu Int,@Ise Int ")
                .AppendLine("Set @Data1 = '" & Data1 & "' ")
                .AppendLine("Set @TipoRelatorio = '" & TipoRelatorio & "' --C:Completa P:Passante ")
                .AppendLine("Set @Data2 = (Select Cast(DATEADD(MONTH,DATEDIFF(MONTH,0,@Data1) +1,0)-1 + '23:59:59' as datetime)) ")
                'TABELA AUXILIAR PARA EXECUTAR O UNION E FECHAMENTO DOS DADOS
                .AppendLine("Declare @TbAuxi Table( ")
                .AppendLine("   RefId int, ")
                .AppendLine("   IntId int, ")
                .AppendLine("   RefData Datetime, ")
                .AppendLine("   RefTipo char(1), ")
                .AppendLine("   RefQtde int, ")
                .AppendLine("   IntCortesiaRestaurante char(1), ")
                .AppendLine("   Dia int ")
                .AppendLine(") ")

                'CARREGAMENTO DE HOSPEDES
                .AppendLine("Declare @tbHospedes Table( ")
                .AppendLine("   RefId int, ")
                .AppendLine("   IntId int, ")
                .AppendLine("   RefData Datetime, ")
                .AppendLine("   RefTipo char(1), ")
                .AppendLine("   RefQtde int, ")
                .AppendLine("   IntCortesiaRestaurante char(1), ")
                .AppendLine("   Dia int ")
                .AppendLine(") ")
                'CARREGAMENTO DE PASSANTES
                .AppendLine("Declare @tbPassantes Table( ")
                .AppendLine("   RefId int, ")
                .AppendLine("   IntId int, ")
                .AppendLine("   RefData Datetime, ")
                .AppendLine("   RefTipo char(1), ")
                .AppendLine("   RefQtde int, ")
                .AppendLine("   IntCortesiaRestaurante char(1), ")
                .AppendLine("   Dia int ")
                .AppendLine(") ")

                '.AppendLine("if (@TipoRelatorio = 'C') ")
                '.AppendLine("   Begin ")
                '.AppendLine("		 Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                '.AppendLine("		   SELECT        r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                '.AppendLine("		   FROM            dbo.TbRefeicao r ")
                '.AppendLine("		   Where  r.RefData between @Data1 and @Data2 ")
                '.AppendLine("		   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                '.AppendLine("   End ")

                .AppendLine(" if (@TipoRelatorio = 'H' or @TipoRelatorio = 'C') ")
                .AppendLine("  Begin ")
                .AppendLine("   --Pegando todas as refeições + Servidores - Passantes ")
                'Inserindo todas as refeições cujo tipo seja almoço - Desjejum e Jantar serão adicionadas logo abaixo
                .AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                .AppendLine("   FROM dbo.TbRefeicao r ")
                .AppendLine("   inner join VwIntegrante i on i.IntId = r.IntId and i.IntTipo = 'H' ")
                '.AppendLine("	and r.RefData between @Data1 and @Data2 ")
                .AppendLine("   and r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                ''Pegando os passantes com refeições no restaurante
                '.AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                '.AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                '.AppendLine("   from TbRefeicao r ")
                '.AppendLine("	inner join VwIntegrante i on i.IntId = r.IntId  ")
                '.AppendLine("	inner join TbReserva rr on rr.ResId = i.ResId and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R') ")
                '.AppendLine("	where RefData between  @Data1 and @Data2 ")

                'Pegando as inserções manuais cujo IntId = -2
                .AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                .AppendLine("   FROM dbo.TbRefeicao r ")
                '.AppendLine("   Where r.RefData between @Data1 and @Data2 ")
                .AppendLine("   Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   and intid = -2 ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                'Pegando os menores de 5 anos cujo IntId = 0
                .AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                .AppendLine("   FROM dbo.TbRefeicao r ")
                '.AppendLine("   Where r.RefData between @Data1 and @Data2 ")
                .AppendLine("   Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   and intid = -0 ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                .AppendLine("  End ")


                .AppendLine(" if (@TipoRelatorio = 'P' or @TipoRelatorio = 'C') ")
                .AppendLine("Begin ")
                .AppendLine("   --Desmembrando os passantes ")
                .AppendLine("   Select @QtdeRefeicoes = isnull(sum(r.RefQtde),0) from VwTbRefeicao r ")
                .AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
                .AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                '.AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                .AppendLine("   Insert @tbPassantes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("   select R.RefId,p.IntId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia  from VwTbRefeicao r ")
                .AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
                .AppendLine("   Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   and r.RefTipo = 'A' ") 'Pegando somente almoço até que a Pollyana resolva com a Lucimar se irá mostrar o Desejum e Jantar
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                .AppendLine("   set @TotalPassantes = @@ROWCOUNT ")

                .AppendLine("   Insert @tbPassantes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("   select top (ISNULL(@QtdeRefeicoes,0) - isNull(@TotalPassantes,0)) r.refId,P.IntVinculoId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia from VwPassante p ")
                .AppendLine("   left join VwTbRefeicao r on r.IntId = p.IntVinculoId  --p.PreIntId ")
                .AppendLine("   Where r.IntId > 0 ")
                '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                .AppendLine("   and r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                .AppendLine("   and r.RefQtde > 1 ")
                .AppendLine("   and p.IntVinculoId in (select rr.IntId  from VwTbRefeicao rr ")
                .AppendLine("   Inner Join VwPassante pp On pp.IntId = rr.IntId ")
                .AppendLine("   inner Join TbCategoria c on c.CatId = pp.CatId ")
                .AppendLine("   Where rr.RefData >= Convert(Varchar(20),@Data1,120) And rr.RefData < Convert(Varchar(20),@Data2,120)) ")
                .AppendLine("   and r.RefTipo = 'A' ") 'Pegando somente almoço até que a Pollyana resolva com a Lucimar se irá mostrar o Desejum e Jantar
                '.AppendLine("   and (p.IntPasAntAlm > 0 or p.IntCortesiaRestaurante = 'S' or p.IntTotalAlmoco > 0) ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                .AppendLine("End")

                'UNINDO AS TABELAS DE PASSANTES COM HÓSPEDES PARA GERAR O VALOR TOTAL
                .AppendLine(" if (@TipoRelatorio = 'C') ")
                .AppendLine("  Begin ")
                .AppendLine("    insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia)  ")
                .AppendLine("      Select RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia from @tbHospedes  ")
                '.AppendLine("    UNION ")
                .AppendLine("    insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia)  ")
                .AppendLine("      Select RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia from @tbPassantes ")
                .AppendLine("  End ")
                'SOMENTE HÓSPEDES
                .AppendLine(" if (@TipoRelatorio = 'H') ")
                .AppendLine("  Begin ")
                .AppendLine("    insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("      Select RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia from @tbHospedes  ")
                .AppendLine("  End ")
                'SOMENTE PASSANTES
                .AppendLine(" if (@TipoRelatorio = 'P') ")
                .AppendLine("  Begin ")
                .AppendLine("    insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                .AppendLine("      Select RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia from @tbPassantes ")
                .AppendLine("  End ")

                .AppendLine("--Inserindo os dados na tabela auxiliar separados por categoria ")
                .AppendLine(" Declare @MapaEstatistico as Table( ")
                .AppendLine("                 Dia Integer, ")
                .AppendLine("            	 DesCom Integer, ")
                .AppendLine("            	 DesDep integer, ")
                .AppendLine("            	 DesCon integer, ")
                .AppendLine("            	 DesUsu integer, ")
                .AppendLine("            	 DesIse integer, ")
                .AppendLine("            	 DesMen5 integer, ")
                .AppendLine("            	 DesSer integer, ")


                .AppendLine("            	 AlmCom Integer, ")
                .AppendLine("            	 AlmDep integer, ")
                .AppendLine("            	 AlmCon integer, ")
                .AppendLine("            	 AlmUsu integer, ")
                .AppendLine("            	 AlmIse integer, ")
                .AppendLine("            	 AlmMen5 integer, ")
                .AppendLine("            	 AlmSer integer, ")

                .AppendLine("            	 JanCom Integer, ")
                .AppendLine("            	 JanDep integer, ")
                .AppendLine("            	 JanCon integer, ")
                .AppendLine("            	 JanUsu integer, ")
                .AppendLine("            	 JanIse integer, ")
                .AppendLine("            	 JanMen5 integer, ")
                .AppendLine("            	 JanSer integer ")
                .AppendLine("            ) ")


                .AppendLine("Declare @Contador Int, @VezesPassada int = 1 ")
                .AppendLine("While @VezesPassada <= 3 ")
                .AppendLine("Begin ")
                .AppendLine("   Set @TipoRefeicao = ")
                .AppendLine("   (Select Case ")
                .AppendLine("      When @VezesPassada = 1 then 'D' ")
                .AppendLine("	  When @VezesPassada = 2 then 'A' ")
                .AppendLine("	  When @VezesPassada = 3 then 'J' ")
                .AppendLine("    end as TipoRefeicao) ")
                .AppendLine("		Set @Contador = 1 ")
                .AppendLine("		--Iniciando o preenchimento das refeicoes ")
                .AppendLine("		while @contador <= DATEPART(dd,@data2) ")
                .AppendLine("			Begin ")
                .AppendLine("				--select sum(RefQtde) from @TbAuxi where IntId <> -1 ")
                .AppendLine("				Set @Com = 0 ")
                .AppendLine("				Select @Com = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                .AppendLine("				where c.CatLinkCat = '1' ")
                .AppendLine("				and r.intId > 0 ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				Select @Com = @Com + IsNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				where r.IntId = '-2' ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				--DEPENDENTE ")
                .AppendLine("				Set @Dep = 0")
                .AppendLine("				Select @Dep = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                .AppendLine("				where c.CatLinkCat = '2' ")
                .AppendLine("				and r.intId > 0 ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				Select @Dep = @Dep + IsNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				where r.IntId = '0' ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				--USUARIO ")
                .AppendLine("				Set @Usu = 0 ")
                .AppendLine("				Select @Usu = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                .AppendLine("				where c.CatLinkCat = 4 ")
                .AppendLine("				and r.intId > 0 ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				Select @Usu = @Usu + isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                .AppendLine("				where c.CatLinkCat = 3 ")
                .AppendLine("				and r.intId > 0 ")
                .AppendLine("				and r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				--ISENTOS ")
                .AppendLine("				Set @Ise = 0")
                .AppendLine("				Select @Ise = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                .AppendLine("				Where r.refTipo = @TipoRefeicao ")
                .AppendLine("				and r.IntCortesiaRestaurante = 'S' ")
                .AppendLine("				and r.Dia = @Contador),0) ")

                .AppendLine("				--SELECT @Usu ")
                .AppendLine("				if @TipoRefeicao = 'D' ")
                .AppendLine("					insert @MapaEstatistico(Dia,DesCom,DesDep,DesUsu,DesIse) Values (@Contador ,@Com,@Dep,@Usu,@Ise) ")
                .AppendLine("				If @TipoRefeicao = 'A' ")
                .AppendLine("					Update @MapaEstatistico Set AlmCom=@Com ,AlmDep=@Dep,AlmUsu=@Usu,AlmIse=@Ise Where Dia = @Contador ")
                .AppendLine("				If @TipoRefeicao = 'J' ")
                .AppendLine("					Update @MapaEstatistico Set JanCom=@Com ,JanDep=@Dep,JanUsu=@Usu,JanIse=@Ise Where Dia = @Contador ")
                .AppendLine("				Set @Contador += 1 ")
                .AppendLine("			End ")
                .AppendLine("	Set @VezesPassada += 1 ")
                .AppendLine("End ")

                .AppendLine("Select isnull(Dia,0) as Dia,isNull(DesCom,0) as DesCom,isNull(DesDep,0) as DesDep,isNull(DesCon,0) as DesCon,isNull(DesUsu,0) as DesUsu,isNull(DesIse,0) as DesIse, ")
                .AppendLine("isNull(DesMen5,0) as DesMen5,isNull(DesSer,0) as DesSer,isNull(AlmCom,0) as AlmCom,isNull(AlmDep,0) as AlmDep,isNull(AlmCon,0) as AlmCon,isNull(AlmUsu,0) as AlmUsu, ")
                .AppendLine("isNull(AlmIse,0) as AlmIse,isNull(AlmMen5,0) as AlmMen5,isNull(AlmSer,0) as AlmSer,isNull(JanCom,0) as JanCom,isNull(JanDep,0)as JanDep,isNull(JanCon,0) as JanCon, ")
                .AppendLine("isNull(JanUsu,0)as JanUsu,isNull(JanIse,0) as JanIse,isNull(JanMen5,0) as JanMen5,isNull(JanSer,0) as JanSer from @MapaEstatistico ")


                '.AppendLine("Declare @QtdeRefeicoes int, @Data1 DateTime,@Data2 Datetime, @TotalPassantes int,@TipoRefeicao char(1),@TipoRelatorio char(1), ")
                '.AppendLine("@Com Int,@Dep Int,@Usu Int,@Ise Int ")
                '.AppendLine("Set @Data1 = '" & Data1 & "' ")
                '.AppendLine("Set @TipoRelatorio = '" & TipoRelatorio & "' --C:Completa P:Passante ")
                '.AppendLine("Set @Data2 = (Select Cast(DATEADD(MONTH,DATEDIFF(MONTH,0,@Data1) +1,0)-1 + '23:59:59' as datetime)) ")
                '.AppendLine("Declare @TbAuxi Table( ")
                '.AppendLine("   RefId int, ")
                '.AppendLine("   IntId int, ")
                '.AppendLine("   RefData Datetime, ")
                '.AppendLine("   RefTipo char(1), ")
                '.AppendLine("   RefQtde int, ")
                '.AppendLine("   IntCortesiaRestaurante char(1), ")
                '.AppendLine("   Dia int ")
                '.AppendLine(") ")

                '.AppendLine("if (@TipoRelatorio = 'C' or @TipoRelatorio = 'H') ")
                '.AppendLine("Begin ")
                '.AppendLine("   --Pegando todas as refeições + Servidores - Passantes ")
                '.AppendLine("   Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                '.AppendLine("   SELECT        r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
                '.AppendLine("   FROM            dbo.TbRefeicao r ")
                ''--Quando as meias diarias de grupos de passeios forem feitas no Restaurante, essas serão somandas nas refeições do restaurante
                '.AppendLine("   where not exists(select 1 from VwPassante p inner join TbReserva rr on rr.ResId = p.ResId  where p.IntId = r.IntId and rr.ResColoniaFeriasDes in ('S','0') ) ")
                ''.AppendLine("   where not exists(select 1 from VwPassante p where p.IntId = r.IntId ) ")
                '.AppendLine("   and r.RefData between @Data1 and @Data2 ")
                ''Só irei nessa tabela se for Caldas
                'If AliasBanco = "TurismoSocialCaldas" Then
                '    .AppendLine("   UNION ")
                '    .AppendLine("   SELECT        0 AS RefId, - 1 AS IntId, conData AS RefData, conTipoRefeicao AS RefTipo, 1 AS RefQtde, 'N' AS RefCortesia, datepart(dd, conData) AS Dia ")
                '    .AppendLine("   FROM            DbRestauranteServidores.dbo.TbConsumo ")
                '    .AppendLine("   where ConData Between @Data1 and @Data2 ")
                'End If
                '.AppendLine("End ")

                '.AppendLine(" if (@TipoRelatorio = 'C' or @TipoRelatorio = 'P') ")
                '.AppendLine("Begin ")
                '.AppendLine("   --Buscando as refeições dos passantes ")
                '.AppendLine("   Select @QtdeRefeicoes = isnull(sum(r.RefQtde),0) from VwTbRefeicao r ")
                '.AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
                '.AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                ''Quando for grupo de passeio irei pegar somente as refeições feitas nas lanchontes as demais são computadas com refeições no restaurante (Hóspedes)
                'If TipoRelatorio = "P" Then
                '    .AppendLine("and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                'End If
                '.AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                '.AppendLine("   Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                '.AppendLine("   select R.RefId,p.IntId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia  from VwTbRefeicao r ")
                '.AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
                '.AppendLine("   Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                ''Quando for grupo de passeio irei pegar somente as refeições feitas nas lanchontes as demais são computadas com refeições no restaurante (Hóspedes)
                'If TipoRelatorio = "P" Then
                '    .AppendLine("and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                'End If
                '.AppendLine("   set @TotalPassantes = @@ROWCOUNT ")

                '.AppendLine("   Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
                '.AppendLine("   select top (ISNULL(@QtdeRefeicoes,0) - isNull(@TotalPassantes,0)) r.refId,P.IntVinculoId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia from VwPassante p ")
                '.AppendLine("   left join VwTbRefeicao r on r.IntId = p.IntVinculoId  --p.PreIntId ")
                '.AppendLine("   Where r.IntId > 0 ")
                ''Quando for grupo de passeio irei pegar somente as refeições feitas nas lanchontes as demais são computadas com refeições no restaurante (Hóspedes)
                'If TipoRelatorio = "P" Then
                '    .AppendLine("and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
                'End If
                '.AppendLine("   and r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
                '.AppendLine("   and r.RefQtde > 1 ")
                '.AppendLine("   and p.IntVinculoId in (select rr.IntId  from VwTbRefeicao rr ")
                '.AppendLine("   Inner Join VwPassante pp On pp.IntId = rr.IntId ")
                '.AppendLine("   inner Join TbCategoria c on c.CatId = pp.CatId ")
                '.AppendLine("   Where rr.RefData >= Convert(Varchar(20),@Data1,120) And rr.RefData < Convert(Varchar(20),@Data2,120)) ")
                '.AppendLine("   and (p.IntPasAntAlm > 0 or p.IntCortesiaRestaurante = 'S' or p.IntTotalAlmoco > 0) ")
                '.AppendLine("End ")

                '.AppendLine("--Inserindo os dados na tabela auxiliar separados por categoria ")
                '.AppendLine(" Declare @MapaEstatistico as Table( ")
                '.AppendLine("                 Dia Integer, ")
                '.AppendLine("            	 DesCom Integer, ")
                '.AppendLine("            	 DesDep integer, ")
                '.AppendLine("            	 DesCon integer, ")
                '.AppendLine("            	 DesUsu integer, ")
                '.AppendLine("            	 DesIse integer, ")
                '.AppendLine("            	 DesMen5 integer, ")
                '.AppendLine("            	 DesSer integer, ")


                '.AppendLine("            	 AlmCom Integer, ")
                '.AppendLine("            	 AlmDep integer, ")
                '.AppendLine("            	 AlmCon integer, ")
                '.AppendLine("            	 AlmUsu integer, ")
                '.AppendLine("            	 AlmIse integer, ")
                '.AppendLine("            	 AlmMen5 integer, ")
                '.AppendLine("            	 AlmSer integer, ")

                '.AppendLine("            	 JanCom Integer, ")
                '.AppendLine("            	 JanDep integer, ")
                '.AppendLine("            	 JanCon integer, ")
                '.AppendLine("            	 JanUsu integer, ")
                '.AppendLine("            	 JanIse integer, ")
                '.AppendLine("            	 JanMen5 integer, ")
                '.AppendLine("            	 JanSer integer ")
                '.AppendLine("            ) ")

                '.AppendLine("Declare @Contador Int, @VezesPassada int = 1 ")
                '.AppendLine("While @VezesPassada <= 3 ")
                '.AppendLine("Begin ")
                '.AppendLine("   Set @TipoRefeicao = ")
                '.AppendLine("   (Select Case ")
                '.AppendLine("      When @VezesPassada = 1 then 'D' ")
                '.AppendLine("	  When @VezesPassada = 2 then 'A' ")
                '.AppendLine("	  When @VezesPassada = 3 then 'J' ")
                '.AppendLine("    end as TipoRefeicao) ")
                '.AppendLine("		Set @Contador = 1 ")
                '.AppendLine("		--Iniciando o preenchimento das refeicoes ")
                '.AppendLine("		while @contador <= DATEPART(dd,@data2) ")
                '.AppendLine("			Begin ")
                '.AppendLine("				--select sum(RefQtde) from @TbAuxi where IntId <> -1 ")
                '.AppendLine("				Set @Com = 0 ")
                '.AppendLine("				Select @Com = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                '.AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                '.AppendLine("				where c.CatLinkCat = '1' ")
                '.AppendLine("				and r.intId > 0 ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				Select @Com = @Com + IsNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				where r.IntId = '-2' ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				--DEPENDENTE ")
                '.AppendLine("				Set @Dep = 0")
                '.AppendLine("				Select @Dep = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                '.AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                '.AppendLine("				where c.CatLinkCat = '2' ")
                '.AppendLine("				and r.intId > 0 ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				Select @Dep = @Dep + IsNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				where r.IntId = '0' ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				--USUARIO ")
                '.AppendLine("				Set @Usu = 0 ")
                '.AppendLine("				Select @Usu = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                '.AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                '.AppendLine("				where c.CatLinkCat = 4 ")
                '.AppendLine("				and r.intId > 0 ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				Select @Usu = @Usu + isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId ")
                '.AppendLine("				inner join TbCategoria c on c.CatId = i.CatId ")
                '.AppendLine("				where c.CatLinkCat = 3 ")
                '.AppendLine("				and r.intId > 0 ")
                '.AppendLine("				and r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				--ISENTOS ")
                '.AppendLine("				Set @Ise = 0")
                '.AppendLine("				Select @Ise = isNull((select sum(r.RefQtde) from  @TbAuxi r ")
                '.AppendLine("				Where r.refTipo = @TipoRefeicao ")
                '.AppendLine("				and r.IntCortesiaRestaurante = 'S' ")
                '.AppendLine("				and r.Dia = @Contador),0) ")

                '.AppendLine("				--SELECT @Usu ")
                '.AppendLine("				if @TipoRefeicao = 'D' ")
                '.AppendLine("					insert @MapaEstatistico(Dia,DesCom,DesDep,DesUsu,DesIse) Values (@Contador ,@Com,@Dep,@Usu,@Ise) ")
                '.AppendLine("				If @TipoRefeicao = 'A' ")
                '.AppendLine("					Update @MapaEstatistico Set AlmCom=@Com ,AlmDep=@Dep,AlmUsu=@Usu,AlmIse=@Ise Where Dia = @Contador ")
                '.AppendLine("				If @TipoRefeicao = 'J' ")
                '.AppendLine("					Update @MapaEstatistico Set JanCom=@Com ,JanDep=@Dep,JanUsu=@Usu,JanIse=@Ise Where Dia = @Contador ")
                '.AppendLine("				Set @Contador += 1 ")
                '.AppendLine("			End ")
                '.AppendLine("	Set @VezesPassada += 1 ")
                '.AppendLine("End ")

                '.AppendLine("Select isnull(Dia,0) as Dia,isNull(DesCom,0) as DesCom,isNull(DesDep,0) as DesDep,isNull(DesCon,0) as DesCon,isNull(DesUsu,0) as DesUsu,isNull(DesIse,0) as DesIse, ")
                '.AppendLine("isNull(DesMen5,0) as DesMen5,isNull(DesSer,0) as DesSer,isNull(AlmCom,0) as AlmCom,isNull(AlmDep,0) as AlmDep,isNull(AlmCon,0) as AlmCon,isNull(AlmUsu,0) as AlmUsu, ")
                '.AppendLine("isNull(AlmIse,0) as AlmIse,isNull(AlmMen5,0) as AlmMen5,isNull(AlmSer,0) as AlmSer,isNull(JanCom,0) as JanCom,isNull(JanDep,0)as JanDep,isNull(JanCon,0) as JanCon, ")
                '.AppendLine("isNull(JanUsu,0)as JanUsu,isNull(JanIse,0) as JanIse,isNull(JanMen5,0) as JanMen5,isNull(JanSer,0) as JanSer from @MapaEstatistico ")

                Return PreencheLista(Conn.consulta(VarSql.ToString))
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
                ObjHistoricoRefeicaoVO = New HistoricoRefeicaoVO
                With ObjHistoricoRefeicaoVO
                    .AlmCom = ResultadoConsulta.Item("AlmCom")
                    .AlmCon = ResultadoConsulta.Item("AlmCon")
                    .AlmDep = ResultadoConsulta.Item("AlmDep")
                    .AlmIse = ResultadoConsulta.Item("AlmIse")
                    .AlmMen5 = ResultadoConsulta.Item("AlmMen5")
                    .AlmSer = ResultadoConsulta.Item("AlmSer")
                    .AlmUsu = ResultadoConsulta.Item("AlmUsu")
                    .DesCom = ResultadoConsulta.Item("DesCom")
                    .DesCon = ResultadoConsulta.Item("DesCon")
                    .DesDep = ResultadoConsulta.Item("DesDep")
                    .DesIse = ResultadoConsulta.Item("DesIse")
                    .DesMen5 = ResultadoConsulta.Item("DesMen5")
                    .DesSer = ResultadoConsulta.Item("DesSer")
                    .DesUsu = ResultadoConsulta.Item("DesUsu")
                    .Dia = ResultadoConsulta.Item("Dia")
                    .JanCom = ResultadoConsulta.Item("JanCom")
                    .JanCon = ResultadoConsulta.Item("JanCon")
                    .JanDep = ResultadoConsulta.Item("JanDep")
                    .JanIse = ResultadoConsulta.Item("JanIse")
                    .JanMen5 = ResultadoConsulta.Item("JanMen5")
                    .JanSer = ResultadoConsulta.Item("JanSer")
                    .JanUsu = ResultadoConsulta.Item("JanUsu")
                End With
                Lista.Add(ObjHistoricoRefeicaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function ConsultaRefFuncionarios(ByVal ObjHistoricoRefeicaoVO As HistoricoRefeicaoVO, ByVal AliasBanco As String, ByVal Mes As String, ByVal Ano As String, ByVal TipoInsercao As String, Origem As String) As HistoricoRefeicaoVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("DECLARE @MES INT ")
                .Append("DECLARE @ANO INT ")

                .Append("SET @MES = " & Mes & " ")
                .Append("SET @ANO = " & Ano & " ")

                .Append("DECLARE @AUX TABLE ")
                .Append("( ")
                .Append("DESJEJUM INT, ")
                .Append("ALMOCO INT, ")
                .Append("JANTAR INT ")
                .Append(") ")

                'Se for passante não irá somar as refeiçõe dos servidores
                If Origem = 2 Then
                    .Append("INSERT INTO @AUX(DESJEJUM,ALMOCO,JANTAR) VALUES (0,0,0) ")
                Else
                    'Desjejum
                    .Append("INSERT INTO @AUX(DESJEJUM) ")
                    .Append("SELECT SUM(C.CONQUANTIDADE) ")
                    .Append("FROM TBCONSUMO C ")
                    .Append("WHERE MONTH(C.CONDATA) = @MES ")
                    .Append("AND YEAR(C.CONDATA) = @ANO ")
                    .Append("AND C.CONTIPOREFEICAO = 'D' ")
                    'insere condição de inserção manual
                    If TipoInsercao = "M" Then
                        .Append("AND C.ConTipoInsercao = 'M' ")
                    End If

                    'Almoço
                    .Append("UPDATE @AUX SET ALMOCO = ")
                    .Append("(SELECT SUM(C.CONQUANTIDADE) ")
                    .Append("FROM TBCONSUMO C ")
                    .Append("WHERE MONTH(C.CONDATA) = @MES ")
                    .Append("AND YEAR(C.CONDATA) = @ANO ")
                    'insere condição de inserção manual
                    If TipoInsercao = "M" Then
                        .Append("AND C.CONTIPOREFEICAO = 'A' ")
                        .Append("AND C.ConTipoInsercao = 'M') ")
                    Else
                        .Append("AND C.CONTIPOREFEICAO = 'A') ")
                    End If

                    'Jantar
                    .Append("UPDATE @AUX SET JANTAR = ")
                    .Append("(SELECT SUM(C.CONQUANTIDADE) ")
                    .Append("FROM TBCONSUMO C ")
                    .Append("WHERE MONTH(C.CONDATA) = @MES ")
                    .Append("AND YEAR(C.CONDATA) = @ANO ")
                    'insere condição de inserção manual
                    If TipoInsercao = "M" Then
                        .Append("AND C.CONTIPOREFEICAO = 'J' ")
                        .Append("AND C.ConTipoInsercao = 'M') ")
                    Else
                        .Append("AND C.CONTIPOREFEICAO = 'J') ")
                    End If
                End If

                .Append("SELECT * FROM @AUX ")
                Return PreencheListaRefFuncionario(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaRefFuncionario(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As HistoricoRefeicaoVO
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjHistoricoRefeicaoVO = New HistoricoRefeicaoVO
                With ObjHistoricoRefeicaoVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Desjejum")) Then
                        .DesjejumFunc = 0
                    Else
                        .DesjejumFunc = ResultadoConsulta.Item("Desjejum")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Almoco")) Then
                        .AlmocoFunc = 0
                    Else

                        .AlmocoFunc = ResultadoConsulta.Item("Almoco")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Jantar")) Then
                        .JantarFunc = 0
                    Else
                        .JantarFunc = ResultadoConsulta.Item("Jantar")
                    End If
                End With
            End While
        End If
        ResultadoConsulta.Close()
        Return ObjHistoricoRefeicaoVO
    End Function
    Public Function HistoricoComplementoDiaria(AliasBanco As String, Dia As String, Mes As String, Ano As String, Caracteristica As String, HoraAquisicao As Integer) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("Declare @Mes varchar(2), @Ano char(4) ")
                .AppendLine("set @Mes = '" & Mes & "' ")
                .AppendLine("set @Ano = '" & Ano & "' ")
                .AppendLine("if (cast(@Mes as integer) < 4) and (@Ano = '2003') ")
                .AppendLine("begin ")
                .AppendLine("set @Mes = '04' ")
                .AppendLine("set @Ano = '2003' ")
                .AppendLine("     End ")

                .AppendLine("declare ")
                .AppendLine("@Data1 datetime, ")
                .AppendLine("@Data2 datetime ")
                .AppendLine("set @Data1 = convert(datetime, '01/' + @Mes + '/' + @Ano + ' 00:00:00', 103) ")
                .AppendLine("set @Data2 = dateadd(ss, -1, dateadd(m, 1, @Data1)) ")
                .AppendLine("SELECT SUM(1) AS Quantidade,CAST(i.IntDataFim AS Date) as Data, ")
                .AppendLine("(select sum(1) from TbIntegrante ii where CAST(ii.IntDataFim AS DATE) = CAST(i.IntDataFim AS DATE) AND II.ResId > 0 ")
                .AppendLine("and ii.IntDataIniReal is not null ")
                .AppendLine("and Not Exists(Select 1 from tbReserva rr where rr.resId = ii.ResId and rr.ResStatus = 'C') ) as CheckOut, ")
                .AppendLine("case datepart(dw,i.IntDataFim) ")
                .AppendLine("when 1 then 'Domingo' ")
                .AppendLine("when 2 then 'Segunda feira' ")
                .AppendLine("when 3 then 'Terça feira' ")
                .AppendLine("when 4 then 'Quarta feira' ")
                .AppendLine("when 5 then 'Quinta feira' ")
                .AppendLine("when 6 then 'Sexta feria' ")
                .AppendLine("when 7 then 'Sábado' ")
                .AppendLine("end as DiaSemana, ")
                .AppendLine("sum(i.IntTotalAlmoco) as TotalAlmoco ")
                .AppendLine("FROM TbIntegrante I ")
                .AppendLine("INNER JOIN TbReserva R ON R.ResId = I.ResId ")
                .AppendLine("WHERE CAST(I.IntDataFim AS DATE) BETWEEN  @Data1 AND @Data2 ")
                .AppendLine("AND I.RESID > 0  ")
                .AppendLine("AND I.IntAlmoco = 'S' ") 'S=Quem tem almoço na entra e saida'
                If Dia <> "T" Then
                    .AppendLine("AND  datepart(dw,i.IntDataFim) = '" & Dia & "' ")
                End If
                If Caracteristica = "I" Then
                    .AppendLine("AND R.ResCaracteristica IN ('I') ")
                ElseIf Caracteristica = "E" Then
                    .AppendLine("AND R.ResCaracteristica IN ('P','E','T') ")
                End If
                If HoraAquisicao.ToString > 0 Then
                    .AppendLine("And (DATEPART(Hour,isNull((select top 1 t.IntUsuarioData from tbIntegrantelog t where t.IntId = i.intId and t.IntAlmoco = 'S' order by t.intUsuarioData asc),i.IntUsuarioData)) Between 0 and " & HoraAquisicao - 1 & " ")
                    .AppendLine("OR CAST(isNull((select top 1 t.IntUsuarioData from tbIntegrantelog t where t.IntId = i.intId and t.IntAlmoco = 'S' order by t.intUsuarioData asc),i.IntUsuarioData) AS DATE) < CAST(i.IntDataFim AS DATE)) ") 'Aconteceu casos da venda ser antes do dia da saída

                End If
                .AppendLine("GROUP BY CAST(i.IntDataFim AS DATE),datepart(dw,i.IntDataFim) ")
                .AppendLine("ORDER BY CAST(i.IntDataFim AS DATE) ")
                .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            End With
            Return PreecheComplementoDiaria(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreecheComplementoDiaria(ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjHistoricoRefeicaoVO = New HistoricoRefeicaoVO
                With ObjHistoricoRefeicaoVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("Quantidade")) Then
                        .Quantidade = 0
                    Else
                        .Quantidade = ResultadoConsulta.Item("Quantidade")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("Data")) Then
                        .Data = ""
                    Else
                        .Data = ResultadoConsulta.Item("Data")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("DiaSemana")) Then
                        .DiaSemana = ""
                    Else
                        .DiaSemana = ResultadoConsulta.Item("DiaSemana")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("TotalAlmoco")) Then
                        .TotalAlmoco = 0
                    Else
                        .TotalAlmoco = ResultadoConsulta.Item("TotalAlmoco")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CheckOut")) Then
                        .CheckOut = 0
                    Else
                        .CheckOut = ResultadoConsulta.Item("CheckOut")
                    End If
                End With
                Lista.Add(ObjHistoricoRefeicaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function




    'Public Function ConsultaHistorico(ByVal ObjHistoricoRefeicaoVO As HistoricoRefeicaoVO, ByVal AliasBanco As String, ByVal Mes As String, ByVal Ano As String, ByVal TipoInsercao As String, Origem As String) As IList


End Class
