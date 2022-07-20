Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Text
Public Class HistoricoRefeicaoIndividualDAO
    Dim ObjHRIndividualVO As HistoricoRefeicaoIndividualVO
    Dim ObjHRManualDAO As HistoricoRefeicaoIndividualDAO

    Public Function Consultar(ByVal ObjHRManualDAO As HistoricoRefeicaoIndividualVO, ByVal AliasBanco As String, ByVal Mes As String, ByVal Ano As String, ByVal Tipo As String) As IList
        Try

            Dim Data1 As String
            Data1 = Format(CDate(Ano & "-" & Mes & "-01"), "yyyy-MM-dd")

            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .AppendLine("SET NOCOUNT ON Declare @QtdeRefeicoes int, @Data1 DateTime,@Data2 Datetime, @TotalPassantes int,@TipoRefeicao char(1), ")
                .AppendLine("@Com Int,@Dep Int,@Usu Int,@Ise Int  ")

                .AppendLine("Declare @Comerciario int,@Dependente int,@Usuario int,@PasComerciario int,@PasDependente int, ")
                .AppendLine("@PasUsuario int,@Cortesias int,@Menor5 int,@InsercaoManual int,@Conveniado int,@Servidores int ")

                .AppendLine("Set @Data1 = '" & Data1 & "'  ")
                .AppendLine("Set @Data2 = (Select Cast(DATEADD(MONTH,DATEDIFF(MONTH,0,@Data1) +1,0)-1 + '23:59:59' as datetime))  ")
                .AppendLine("Declare @TbAuxi Table(  ")
                .AppendLine("   RefId int,  ")
                .AppendLine("   IntId int,  ")
                .AppendLine("   RefData Datetime,  ")
                .AppendLine("   RefTipo char(1),  ")
                .AppendLine("   RefQtde int,  ")
                .AppendLine("   IntCortesiaRestaurante char(1),  ")
                .AppendLine("   Dia int ")
                .AppendLine(")  ")
                .AppendLine("   --Pegando todas as refeições + Servidores - Passantes  ")
                .AppendLine("   Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia)  ")
                .AppendLine("   SELECT        r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia  ")
                .AppendLine("   FROM            dbo.TbRefeicao r  ")
                .AppendLine("   where not exists(select 1 from VwPassante p where p.IntId = r.IntId )  ")
                .AppendLine("   and r.RefData between @Data1 and @Data2  ")
                'Só irá consultar a base para servidores quando for de Caldas Novas
                If AliasBanco = "TurismoSocialCaldas" Then
                    .AppendLine("   UNION  ")
                    .AppendLine("   SELECT        0 AS RefId, - 1 AS IntId, conData AS RefData, conTipoRefeicao AS RefTipo, 1 AS RefQtde, 'N' AS RefCortesia, datepart(dd, conData) AS Dia  ")
                    .AppendLine("   FROM            DbRestauranteServidores.dbo.TbConsumo  ")
                    .AppendLine("   where ConData Between @Data1 and @Data2  ")
                    .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
                End If

                .AppendLine("--Desmembrando os passantes  ")
                .AppendLine("Select @QtdeRefeicoes = isnull(sum(r.RefQtde),0) from VwTbRefeicao r  ")
                .AppendLine("Inner Join VwPassante p On r.IntId = p.IntId  ")
                .AppendLine("Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120)  ")
                .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01'))  ")
                .AppendLine("Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia)  ")
                .AppendLine("select R.RefId,p.IntId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia  from VwTbRefeicao r  ")
                .AppendLine("Inner Join VwPassante p On r.IntId = p.IntId  ")
                .AppendLine("Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120)  ")
                .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                .AppendLine("set @TotalPassantes = @@ROWCOUNT  ")

                .AppendLine("Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia)  ")
                .AppendLine("select top (ISNULL(@QtdeRefeicoes,0) - isNull(@TotalPassantes,0)) r.refId,P.IntVinculoId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia from VwPassante p  ")
                .AppendLine("left join VwTbRefeicao r on r.IntId = p.IntVinculoId   ")
                .AppendLine("Where r.IntId > 0  ")
                .AppendLine("and r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120)  ")
                .AppendLine("and r.RefQtde > 1  ")
                .AppendLine("and p.IntVinculoId in (select rr.IntId  from VwTbRefeicao rr  ")
                .AppendLine("Inner Join VwPassante pp On pp.IntId = rr.IntId  ")
                .AppendLine("inner Join TbCategoria c on c.CatId = pp.CatId  ")
                .AppendLine("Where rr.RefData >= Convert(Varchar(20),@Data1,120) And rr.RefData < Convert(Varchar(20),@Data2,120))  ")
                .AppendLine("and (p.IntPasAntAlm > 0 or p.IntCortesiaRestaurante = 'S' or p.IntTotalAlmoco > 0)  ")
                .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

                .AppendLine("--Inserindo os dados na tabela auxiliar separados por categoria  ")
                .AppendLine(" Declare @MapaEstatistico as Table(   ")
                .AppendLine("                     Dia Integer,   ")
                .AppendLine("                	 Comerciario Integer,   ")
                .AppendLine("                	 Dependente integer,   ")
                .AppendLine("                	 Conveniado integer,   ")
                .AppendLine("                	 Usuario integer,   ")
                .AppendLine("                	 PasComerciario Integer,   ")
                .AppendLine("                	 PasDependente integer,   ")
                .AppendLine("                	 PasUsuario integer,   ")
                .AppendLine("                	 Cortesias integer,   ")
                .AppendLine("                	 InsercaoManual Integer,  ")
                .AppendLine("                	 Menor5 integer,  ")
                .AppendLine("                	 Servidores integer,  ")
                .AppendLine("                	 TotalLinha integer   ")
                .AppendLine(") ")

                .AppendLine("Declare @Contador Int  ")
                .AppendLine("   Set @TipoRefeicao = '" & Tipo & "' ")
                .AppendLine("		Set @Contador = 1  ")
                .AppendLine("		--Iniciando o preenchimento das refeicoes  ")
                .AppendLine("		while @contador <= DATEPART(dd,@data2)  ")
                .AppendLine("			Begin  ")
                .AppendLine("				--select sum(RefQtde) from @TbAuxi where IntId <> -1  ")
                .AppendLine("				Set @Comerciario = 0  ")
                .AppendLine("				Select @Comerciario = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = '1'  ")
                .AppendLine("				and I.Inttipo <> 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @PasComerciario = 0  ")
                .AppendLine("				Select @PasComerciario = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = '1'  ")
                .AppendLine("				and I.Inttipo = 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @InsercaoManual = 0 ")
                .AppendLine("				Select @InsercaoManual = IsNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				where r.IntId = '-2'  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")
                .AppendLine("				--DEPENDENTE  ")
                .AppendLine("				Set @Dependente = 0 ")
                .AppendLine("				Select @Dependente = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = '2'  ")
                .AppendLine("				and i.IntTipo <> 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @PasDependente = 0 ")
                .AppendLine("				Select @PasDependente = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = '2'  ")
                .AppendLine("				and i.IntTipo = 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @Menor5 = 0 ")
                .AppendLine("				Select @Menor5 = IsNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				where r.IntId = '0'  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				--USUARIO  ")
                .AppendLine("				Set @Usuario = 0  ")
                .AppendLine("				Select @Usuario = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = 4  ")
                .AppendLine("				and i.IntTipo <> 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @PasUsuario = 0  ")
                .AppendLine("				Select @PasUsuario = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = 4  ")
                .AppendLine("				and i.IntTipo = 'P' ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("               Set @Conveniado = 0 ")
                .AppendLine("				Select @Conveniado = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				inner join VwIntegrante i on i.IntId = r.IntId  ")
                .AppendLine("				inner join TbCategoria c on c.CatId = i.CatId  ")
                .AppendLine("				where c.CatLinkCat = 3  ")
                .AppendLine("				and r.intId > 0  ")
                .AppendLine("				and r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")
                .AppendLine("				--ISENTOS  ")
                .AppendLine("				Set @Cortesias = 0 ")
                .AppendLine("				Select @Cortesias = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				Where r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.IntCortesiaRestaurante = 'S'  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				Set @Servidores= 0 ")
                .AppendLine("				Select @Servidores = isNull((select sum(r.RefQtde) from  @TbAuxi r  ")
                .AppendLine("				Where r.refTipo = @TipoRefeicao  ")
                .AppendLine("				and r.IntId  = -1  ")
                .AppendLine("				and r.Dia = @Contador),0)  ")

                .AppendLine("				insert @MapaEstatistico(Dia,Comerciario,Dependente,Usuario,PasComerciario,PasDependente,PasUsuario,Menor5,InsercaoManual,Cortesias,Servidores,Conveniado,TotalLinha) Values  ")
                .AppendLine("				(@Contador ,@Comerciario,@Dependente,@Usuario,@PasComerciario,@PasDependente,@PasUsuario,@Menor5,@InsercaoManual,@Cortesias,@Servidores, @Conveniado, ")
                .AppendLine("				@Comerciario + @Dependente + @Usuario + @PasComerciario + @PasDependente + @PasUsuario + @Menor5 + @InsercaoManual + @Servidores + @Conveniado)  ")
                .AppendLine("				Set @Contador += 1  ")
                .AppendLine("			End  ")

                .AppendLine("Select Dia,ISNULL(Comerciario,0) Comerciario,ISNULL(PasComerciario,0)PasComerciario,ISNULL(InsercaoManual,0)InsercaoManual,ISNULL(Servidores,0)Servidores, ")
                .AppendLine("ISNULL(Dependente,0)Dependente,ISNULL(PasDependente,0)PasDependente,ISNULL(Menor5,0)Menor5, ")
                .AppendLine("ISNULL(Usuario,0)Usuario,ISNULL(PasUsuario,0)PasUsuario,  ")
                .AppendLine("ISNULL(Conveniado ,0)Conveniado,ISNULL(TotalLinha,0)TotalLinha,ISNULL(Cortesias,0)Cortesias from @MapaEstatistico  ")

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
                ObjHRIndividualVO = New HistoricoRefeicaoIndividualVO
                With ObjHRIndividualVO
                    .Dia = ResultadoConsulta.Item("Dia")
                    .Comerciario = ResultadoConsulta.Item("Comerciario")
                    .Dependente = ResultadoConsulta.Item("Dependente")
                    .Conveniado = ResultadoConsulta.Item("Conveniado")
                    .Usuario = ResultadoConsulta.Item("Usuario")
                    .PasComerciario = ResultadoConsulta.Item("PasComerciario")
                    .PasDependente = ResultadoConsulta.Item("PasDependente")
                    .PasUsuario = ResultadoConsulta.Item("PasUsuario")
                    .Cortesias = ResultadoConsulta.Item("Cortesias")
                    .InsercaoManual = ResultadoConsulta.Item("InsercaoManual")
                    .Menor5 = ResultadoConsulta.Item("Menor5")
                    .Servidores = ResultadoConsulta.Item("Servidores")
                    .TotalLinha = ResultadoConsulta.Item("TotalLinha")
                End With
                Lista.Add(ObjHRIndividualVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
