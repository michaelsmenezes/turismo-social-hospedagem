'INFORMAÇÃO:
' 0 = ISENTO --> Os isentos são somados junto com os dependentes e já são somados nos totais
'-1 = SERVIDOR
'-2 = HOSPEDES --> Servidores e Hospedes são somados como comerciários e já são somados nos totais

Imports Turismo

Partial Class frmHistoricoRefeicoes
    Inherits System.Web.UI.Page
    Dim ObjHistoricoRefeicoesVO As HistoricoRefeicaoVO
    Dim ObjHistoricoRefecoesDAO As HistoricoRefeicaoDAO

    Private DesTotCom As Integer, DesTotDep As Integer, DesTotUsu As Integer, DesTotalGeral As Integer, DesIsento As Integer
    Private AlmTotCom As Integer, AlmTotDep As Integer, AlmTotUsu As Integer, AlmTotalGeral As Integer, AlmIsento As Integer
    Private JanTotCom As Integer, JanTotDep As Integer, JanTotUsu As Integer, JanTotalGeral As Integer, JanIsento As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Caldas Novas")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Usado na pagina de relatórios
                    btnConsultar.Attributes.Add("UOP", "Pirenopolis")
            End Select

            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            'Pegando o Nome Completo do usuário logado no momento'
            Dim search As System.DirectoryServices.DirectorySearcher = New System.DirectoryServices.DirectorySearcher("LDAP://sesc-go.com.br/DC=sesc-go,DC=com,DC=br")
            search.Filter = "(SAMAccountName=" + Replace(User.Identity.Name.ToString, "SESC-GO.COM.BR\", "").ToUpper + ")"
            Dim result As System.DirectoryServices.SearchResult = search.FindOne()
            search.PropertiesToLoad.Add("displayName")
            'Armazenando nome do Usuario no botão para usar na passagem de parametro do relatorio'
            btnConsultar.Attributes.Add("NomeUsuario", result.Properties("displayName").Item(0).ToString)


            'pnlPesquisa_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlPesquisa_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlGridHistorico_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlGridHistorico_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            btnEstatistica.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnGeraEstatistica.click();")
            'Inserindo os anos no combo'
            Dim AnoBase As Integer
            Dim Cont As Integer = 0
            AnoBase = Now.Year + 1
            AnoBase = AnoBase
            While (Cont <= 20)
                drpAno.Items.Add(AnoBase)
                Cont = Cont + 1
                AnoBase = AnoBase - 1
            End While
            'Definindo o mes atual'
            drpMes.SelectedValue = Now.Month
            drpAno.SelectedValue = Now.Year
            'Executando a consulta
            'btnConsultar_Click(sender, e)
            pnlGridHistorico.Visible = False
        End If
    End Sub
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        If rptEstatistica.Visible = True Then
            rptEstatistica.Visible = False
        ElseIf rptEstatisticaPiri.Visible = True Then
            rptEstatisticaPiri.Visible = False
        End If
        pnlGridHistorico.Visible = True
        ObjHistoricoRefeicoesVO = New HistoricoRefeicaoVO
        ObjHistoricoRefecoesDAO = New HistoricoRefeicaoDAO
        gdvHistoricoRef.DataSource = ObjHistoricoRefecoesDAO.ConsultaHistorico(ObjHistoricoRefeicoesVO, btnConsultar.Attributes.Item("AliasBancoTurismo"), drpMes.SelectedValue, drpAno.SelectedValue, drpTipoRefeicao.SelectedValue, drpOrigemDados.SelectedValue)

        gdvHistoricoRef.DataBind()
        'ObjHistoricoRefeicoesVO = ObjHistoricoRefecoesDAO.ConsultaRefFuncionarios(ObjHistoricoRefeicoesVO, "DbRestauranteServidores", drpMes.SelectedValue, drpAno.SelectedValue, drpTipoRefeicao.SelectedValue, drpOrigemDados.SelectedIndex)
        ''Mostrando os valores da refeições dos funcionários nos label's'
        'lblDesjejumServ.Text = ObjHistoricoRefeicoesVO.DesjejumFunc
        'lblAlmocoServ.Text = ObjHistoricoRefeicoesVO.AlmocoFunc
        'lblJantarServ.Text = ObjHistoricoRefeicoesVO.JantarFunc
        lblDataImpressao.Text = Format(Date.Now, "dd/MM/yyyy")
        If gdvHistoricoRef.Rows.Count = 0 Then
            lblTotRefeicao.Text = "00"
        End If
    End Sub

    Protected Sub gdvHistoricoRef_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvHistoricoRef.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DesTotCom = 0
            DesTotDep = 0
            DesTotUsu = 0
            DesTotalGeral = 0
            AlmTotCom = 0
            AlmTotDep = 0
            AlmTotUsu = 0
            AlmTotalGeral = 0
            JanTotCom = 0
            JanTotDep = 0
            JanTotUsu = 0
            JanTotalGeral = 0
            DesIsento = 0
            AlmIsento = 0
            JanIsento = 0
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Cells(5).Text = Format(CInt(e.Row.Cells(1).Text) + CInt(e.Row.Cells(2).Text) + CInt(e.Row.Cells(3).Text) + CInt(e.Row.Cells(4).Text), "###,###,#00")
            e.Row.Cells(4).Text = Format(CInt(e.Row.Cells(1).Text) + CInt(e.Row.Cells(2).Text) + CInt(e.Row.Cells(3).Text), "###,###,#00")
            e.Row.Cells(4).Font.Bold = True
            'Somando as refeições Desjejum
            DesTotCom = DesTotCom + CInt(e.Row.Cells(1).Text)
            DesTotDep = DesTotDep + CInt(e.Row.Cells(2).Text)
            DesTotUsu = DesTotUsu + CInt(e.Row.Cells(3).Text)
            'Somando as refeições Almoço
            AlmTotCom = AlmTotCom + CInt(e.Row.Cells(8).Text)
            AlmTotDep = AlmTotDep + CInt(e.Row.Cells(9).Text)
            AlmTotUsu = AlmTotUsu + CInt(e.Row.Cells(10).Text)
            'Somando as refeições Jantar
            JanTotCom = JanTotCom + CInt(e.Row.Cells(15).Text)
            JanTotDep = JanTotDep + CInt(e.Row.Cells(16).Text)
            JanTotUsu = JanTotUsu + CInt(e.Row.Cells(17).Text)
            'Somando os isentos
            DesIsento = DesIsento + CInt(e.Row.Cells(5).Text)
            AlmIsento = AlmIsento + CInt(e.Row.Cells(12).Text)
            JanIsento = JanIsento + CInt(e.Row.Cells(19).Text)

            'Adicionando o Total Almoço
            e.Row.Cells(11).Text = Format(CInt(e.Row.Cells(8).Text) + CInt(e.Row.Cells(9).Text) + CInt(e.Row.Cells(10).Text), "###,###,#00")
            e.Row.Cells(11).Font.Bold = True

            'Adicionando o Total no Jantar
            e.Row.Cells(18).Text = Format(CInt(e.Row.Cells(15).Text) + CInt(e.Row.Cells(16).Text) + CInt(e.Row.Cells(17).Text), "###,###,#00")
            e.Row.Cells(18).Font.Bold = True


            DesTotalGeral = DesTotalGeral + CInt(e.Row.Cells(4).Text)
            AlmTotalGeral = AlmTotalGeral + CInt(e.Row.Cells(11).Text)
            JanTotalGeral = JanTotalGeral + CInt(e.Row.Cells(18).Text)
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(0).Font.Bold = True
            e.Row.Cells(1).Font.Bold = True
            e.Row.Cells(1).Text = Format(DesTotCom, "###,###,#00")
            e.Row.Cells(1).HorizontalAlign = 2
            e.Row.Cells(2).Font.Bold = True
            e.Row.Cells(2).Text = Format(DesTotDep, "###,###,#00")
            e.Row.Cells(2).HorizontalAlign = 2
            e.Row.Cells(3).Font.Bold = True
            e.Row.Cells(3).Text = Format(DesTotUsu, "###,###,#00")
            e.Row.Cells(3).HorizontalAlign = 2

            e.Row.Cells(7).Text = "Total"
            e.Row.Cells(7).Font.Bold = True
            e.Row.Cells(8).Text = Format(AlmTotCom, "###,###,#00")
            e.Row.Cells(8).Font.Bold = True
            e.Row.Cells(8).HorizontalAlign = 2
            e.Row.Cells(9).Text = Format(AlmTotDep, "###,###,#00")
            e.Row.Cells(9).Font.Bold = True
            e.Row.Cells(9).HorizontalAlign = 2
            e.Row.Cells(10).Text = Format(AlmTotUsu, "###,###,#00")
            e.Row.Cells(10).Font.Bold = True
            e.Row.Cells(10).HorizontalAlign = 2

            e.Row.Cells(14).Text = "Total"
            e.Row.Cells(14).Font.Bold = True
            e.Row.Cells(15).Text = Format(JanTotCom, "###,###,#00")
            e.Row.Cells(15).Font.Bold = True
            e.Row.Cells(15).HorizontalAlign = 2
            e.Row.Cells(16).Text = Format(JanTotDep, "###,###,#00")
            e.Row.Cells(16).Font.Bold = True
            e.Row.Cells(16).HorizontalAlign = 2
            e.Row.Cells(17).Text = Format(JanTotUsu, "###,###,#00")
            e.Row.Cells(17).Font.Bold = True
            e.Row.Cells(17).HorizontalAlign = 2

            e.Row.Cells(4).Text = Format(DesTotalGeral, "###,###,#00")
            e.Row.Cells(4).Font.Bold = True
            e.Row.Cells(4).HorizontalAlign = 2
            e.Row.Cells(11).Text = Format(AlmTotalGeral, "###,###,#00")
            e.Row.Cells(11).Font.Bold = True
            e.Row.Cells(11).HorizontalAlign = 2
            e.Row.Cells(18).Text = Format(JanTotalGeral, "###,###,#00")
            e.Row.Cells(18).Font.Bold = True
            e.Row.Cells(18).HorizontalAlign = 2

            e.Row.Cells(5).Text = Format(DesIsento, "###,###,#00")
            e.Row.Cells(5).Font.Bold = True
            e.Row.Cells(5).HorizontalAlign = 2
            e.Row.Cells(12).Text = Format(AlmIsento, "###,###,#00")
            e.Row.Cells(12).Font.Bold = True
            e.Row.Cells(12).HorizontalAlign = 2
            e.Row.Cells(19).Text = Format(JanIsento, "###,###,#00")
            e.Row.Cells(19).Font.Bold = True
            e.Row.Cells(19).HorizontalAlign = 2
            lblTotRefeicao.Text = Format(DesTotalGeral + AlmTotalGeral + JanTotalGeral, "###,###,#00")
            lblTotRefeicao.Font.Bold = True
            lblTotRefeicao.Font.Size = 20

        End If
    End Sub


    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub

    Protected Sub btnEstatistica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEstatistica.Click
        'Ira executar o evento btnGeraEstatistica_Click que esta logo abaixo'
    End Sub

    Protected Sub btnGeraEstatistica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeraEstatistica.Click
        'Informação: IntId= -2: Inserção Manual; -1: Servidores; 0:Menor de 5 Anos
        'Obs.: os servidores deixaram de ser computados estatisticamente em 2017

        Dim MesExtenso As String = ""
        pnlGridHistorico.Visible = False
        Select Case drpMes.SelectedValue
            Case 1 : MesExtenso = "JANEIRO"
            Case 2 : MesExtenso = "FEVEREIRO"
            Case 3 : MesExtenso = "MARÇO"
            Case 4 : MesExtenso = "ABRIL"
            Case 5 : MesExtenso = "MAIO"
            Case 6 : MesExtenso = "JUNHO"
            Case 7 : MesExtenso = "JULHO"
            Case 8 : MesExtenso = "AGOSTO"
            Case 9 : MesExtenso = "SETEMBRO"
            Case 10 : MesExtenso = "OUTUBRO"
            Case 11 : MesExtenso = "NOVEMBRO"
            Case 12 : MesExtenso = "DEZEMBRO"
        End Select

        Dim Data1 As String, TipoRelatorio As String
        Data1 = Format(CDate(drpAno.SelectedValue & "-" & drpMes.SelectedValue & "-01"), "yyyy-MM-dd")
        TipoRelatorio = drpOrigemDados.SelectedValue

        Dim UnidadeOperacional As String = ""
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            UnidadeOperacional = "SESC CALDAS NOVAS"
        Else
            UnidadeOperacional = "POUSADA SESC PIRINÓPOLIS"
        End If

        Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
        With VarSql
            .AppendLine("Declare @QtdeRefeicoes int, @Data1 DateTime,@Data2 Datetime, @TotalPassantes int,@TipoRefeicao char(1),@TipoRelatorio char(1), ")
            .AppendLine("@Com Int,@Dep Int,@Usu Int,@Ise Int ")
            .AppendLine("Set @Data1 = '" & Data1 & "' ")
            .AppendLine("Set @TipoRelatorio = '" & TipoRelatorio & "' --C:Completa P:Passante ")
            .AppendLine("Set @Data2 = (Select Cast(DATEADD(MONTH,DATEDIFF(MONTH,0,@Data1) +1,0)-1 + '23:59:59' as datetime)) ")

            'AUXILIAR PARA UNION 
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

            'COMPLETA
            '.AppendLine("if (@TipoRelatorio = 'C') ")
            '.AppendLine("   Begin ")
            '.AppendLine("		 Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            '.AppendLine("		   SELECT        r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
            '.AppendLine("		   FROM            dbo.TbRefeicao r ")
            '.AppendLine("		   Where  r.RefData between @Data1 and @Data2 ")
            ''.AppendLine("          and r.intid not in(select rr.intId from VwTbRefeicao rr ")
            ''.AppendLine("                             Inner Join VwPassante p On rr.IntId = p.IntId ")
            ''.AppendLine("                             Where rr.RefData >= Convert(Varchar(20),@Data1,120) And rr.RefData < Convert(Varchar(20),@Data2,120) ")
            ''.AppendLine("                             and rr.RefTipo not in ('A') )    ")
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
            .AppendLine("   and r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("	OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

            ''Pegando os passantes com refeições no restaurante
            '.AppendLine("   Insert @TbAuxi(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            '.AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
            '.AppendLine("   from TbRefeicao r ")
            '.AppendLine("   inner join VwPassante i on i.IntId = r.IntId ")
            '.AppendLine("   where RefData between  @Data1 and @Data2 ")
            '.AppendLine("   and (i.IntId not in (select pr.IntId from TbRefeicaoPratoIntegrante pr where pr.RefPratoIntData = r.RefData and pr.RefPratoIntQtdEntr > 0)) ")
            '.AppendLine("   and r.RefTipo = 'A' ") 'Aqui estou pegando somente o almoço até que a Erika resolva junto a Lucimar se o Desjejum e Jantar irão ser computados aqui também.
            '.AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

            'Pegando as inserções manuais cujo IntId = -2
            .AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            .AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
            .AppendLine("   FROM dbo.TbRefeicao r ")
            '.AppendLine("   Where r.RefData between @Data1 and @Data2 ")
            .AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("   and intid = -2 ")
            .AppendLine("	OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")

            'Pegando os menores de 5 anos cujo IntId = 0
            .AppendLine("   Insert @tbHospedes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            .AppendLine("   SELECT r.RefId, r.IntId, r.RefData, r.RefTipo, r.RefQtde, r.RefCortesia, DATEPART(dd, r.RefData) AS Dia ")
            .AppendLine("   FROM dbo.TbRefeicao r ")
            '.AppendLine("   Where r.RefData between @Data1 and @Data2 ")
            .AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("   and intid = -0 ")
            .AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            .AppendLine("  End ")

            .AppendLine(" if (@TipoRelatorio = 'P' or @TipoRelatorio = 'C') ")
            .AppendLine("Begin ")

            '.AppendLine("   --Desmembrando os passantes ")
            '.AppendLine("   Select @QtdeRefeicoes = isnull(sum(r.RefQtde),0) from VwTbRefeicao r ")
            '.AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
            '.AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")

            .AppendLine("   --Desmembrando os passantes ")
            .AppendLine("   Select @QtdeRefeicoes = isnull(sum(r.RefQtde),0) from VwTbRefeicao r ")
            .AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
            .AppendLine("   Where r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("   and r.RefTipo = 'A' ") 'Pegando somente almoço até que a Pollyana resolva com a Lucimar se irá mostrar o Desejum e Jantar
            .AppendLine("	OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")

            '.AppendLine("   OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            .AppendLine("   Insert @tbPassantes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            .AppendLine("   select R.RefId,p.IntId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia  from VwTbRefeicao r ")
            .AppendLine("   Inner Join VwPassante p On r.IntId = p.IntId ")
            .AppendLine("   Where r.RefData >=  Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("   and r.RefTipo = 'A' ") 'Pegando somente almoço até que a Pollyana resolva com a Lucimar se irá mostrar o Desejum e Jantar
            .AppendLine("	OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")

            .AppendLine("   set @TotalPassantes = @@ROWCOUNT ")

            .AppendLine("   Insert @tbPassantes(RefId,IntId,RefData,RefTipo,RefQtde,IntCortesiaRestaurante,Dia) ")
            .AppendLine("   select top (ISNULL(@QtdeRefeicoes,0) - isNull(@TotalPassantes,0)) r.refId,P.IntVinculoId,r.RefData,r.RefTipo,1 as RefQtde,p.IntCortesiaRestaurante,DATEPART(dd, r.RefData) as Dia from VwPassante p ")
            .AppendLine("   left join VwTbRefeicao r on r.IntId = p.IntVinculoId  --p.PreIntId ")
            .AppendLine("   Where r.IntId > 0 ")
            '.AppendLine("   and not exists(select 1 from TbReserva rr where rr.resid = p.resid and rr.ResColoniaFeriasDes in ('1','2','3','4','5','6','R')) ")
            .AppendLine("   and r.RefTipo = 'A' ") 'Pegando somente almoço até que a Pollyana resolva com a Lucimar se irá mostrar o Desejum e Jantar
            .AppendLine("   and r.RefData >= Convert(Varchar(20),@Data1,120) And r.RefData < Convert(Varchar(20),@Data2,120) ")
            .AppendLine("   and r.RefQtde > 1 ")
            .AppendLine("   and p.IntVinculoId in (select rr.IntId  from VwTbRefeicao rr ")
            .AppendLine("   Inner Join VwPassante pp On pp.IntId = rr.IntId ")
            .AppendLine("   inner Join TbCategoria c on c.CatId = pp.CatId ")
            .AppendLine("   Where rr.RefData >= Convert(Varchar(20),@Data1,120) And rr.RefData < Convert(Varchar(20),@Data2,120)) ")
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

        End With

        Dim ordersPrioridadeLinha As dtsEstatisticaRefeicoesTableAdapters.DataTableRefeicoesTableAdapter = New dtsEstatisticaRefeicoesTableAdapters.DataTableRefeicoesTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelPrioridadeLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarSql.ToString)
        Dim rdsRelPrioridadeLinha As New Microsoft.Reporting.WebForms.ReportDataSource("dtsEstatisticaRefeicoes_DataTableRefeicoes", ordersRelPrioridadeLinha)

        'Somanto o total de refeições e passando como parametro para o relatório
        Dim soma As Integer = 0
        For Each item As System.Data.DataRow In ordersRelPrioridadeLinha.Rows
            'If item("Dia") = 100 Then
            soma += item("DesCom") + item("DesDep") + item("DesUsu") _
                + item("AlmCom") + item("AlmDep") + item("AlmUsu") _
                + item("JanCom") + item("JanDep") + item("JanUsu")
            'End If
        Next

        Dim VarSqlManual As Text.StringBuilder = New Text.StringBuilder("SET NOCOUNT ON ")
        If drpOrigemDados.SelectedIndex <> 2 Then
            'Passando as refeições inseridas manualmente, no caso de queda de energia ou perda de sistema
            With VarSqlManual
                .AppendLine("SELECT INTID,REFDATA, ")
                .AppendLine("CASE ")
                .AppendLine("WHEN REFTIPO = 'A' THEN 'ALMOÇO' ")
                .AppendLine("WHEN REFTIPO = 'J' THEN 'JANTAR' ")
                .AppendLine("WHEN REFTIPO = 'D' THEN 'DESJEJUM' ")
                .AppendLine("END AS REFTIPO ")
                .AppendLine(",SUM(REFQTDE) AS REFQTDE,REFCORTESIA,REFMOTIVOINSMANUAL ")
                .AppendLine("FROM TBREFEICAO WHERE YEAR(REFDATA) = " & drpAno.SelectedValue & "  ")
                .AppendLine("AND MONTH(REFDATA) = " & drpMes.SelectedValue & " ")
                .AppendLine("AND INTID = -2 ")
                .AppendLine("GROUP BY INTID,REFDATA,REFTIPO,REFCORTESIA,REFMOTIVOINSMANUAL ")
            End With
        End If
        Dim ordersPrioridadeLinhaRefManual As dtsEstatisticaRefeicoesTableAdapters.DataTableRefManualTableAdapter = New dtsEstatisticaRefeicoesTableAdapters.DataTableRefManualTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersPrioridadeLinhaRefManual.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersPrioridadeLinhaRefManual.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelPrioridadeLinhaRefManual As System.Data.DataTable = ordersPrioridadeLinhaRefManual.GetData(VarSqlManual.ToString)
        Dim rdsRelPrioridadeLinhaRefManual As New Microsoft.Reporting.WebForms.ReportDataSource("dtsEstatisticaRefeicoes_DataTableRefManual", ordersRelPrioridadeLinhaRefManual)

        'Passando os parametros'
        Dim ParamsEstatistica(5) As Microsoft.Reporting.WebForms.ReportParameter
        ParamsEstatistica(0) = New Microsoft.Reporting.WebForms.ReportParameter("ano", drpAno.SelectedValue)
        ParamsEstatistica(1) = New Microsoft.Reporting.WebForms.ReportParameter("mes", MesExtenso)
        ParamsEstatistica(2) = New Microsoft.Reporting.WebForms.ReportParameter("unidade", UnidadeOperacional)
        ParamsEstatistica(3) = New Microsoft.Reporting.WebForms.ReportParameter("responsavel", btnConsultar.Attributes.Item("NomeUsuario").ToString)
        ParamsEstatistica(4) = New Microsoft.Reporting.WebForms.ReportParameter("somageral", Format(soma, "###,###,###"))
        Select Case drpOrigemDados.SelectedIndex
            Case 0
                ParamsEstatistica(5) = New Microsoft.Reporting.WebForms.ReportParameter("titulo", "NUTRIÇÃO-Hóspedes e passantes")
            Case 1
                ParamsEstatistica(5) = New Microsoft.Reporting.WebForms.ReportParameter("titulo", "NUTRIÇÃO-Hóspedes")
            Case 2
                ParamsEstatistica(5) = New Microsoft.Reporting.WebForms.ReportParameter("titulo", "NUTRIÇÃO-Passantes")
        End Select

        If UnidadeOperacional = "POUSADA SESC PIRINÓPOLIS" Then
            ParamsEstatistica(5) = New Microsoft.Reporting.WebForms.ReportParameter("titulo", "NUTRIÇÃO")
        End If

        'Chamando o relatório'
        If btnConsultar.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            rptEstatistica.Visible = True
            rptEstatisticaPiri.Visible = False
            rptEstatistica.LocalReport.DataSources.Clear()
            rptEstatistica.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
            'rptEstatistica.LocalReport.DataSources.Add(rdsRelPrioridadeLinhaFunc)
            rptEstatistica.LocalReport.DataSources.Add(rdsRelPrioridadeLinhaRefManual)
            rptEstatistica.LocalReport.SetParameters(ParamsEstatistica)
        Else
            rptEstatistica.Visible = False
            rptEstatisticaPiri.Visible = True
            rptEstatisticaPiri.LocalReport.DataSources.Clear()
            rptEstatisticaPiri.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
            rptEstatisticaPiri.LocalReport.DataSources.Add(rdsRelPrioridadeLinhaRefManual)
            rptEstatisticaPiri.LocalReport.SetParameters(ParamsEstatistica)
        End If
    End Sub

    Protected Sub drpMes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpMes.SelectedIndexChanged
        pnlGridHistorico.Visible = False
    End Sub

    Protected Sub drpAno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpAno.SelectedIndexChanged
        pnlGridHistorico.Visible = False
    End Sub

    Protected Sub drpTipoRefeicao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpTipoRefeicao.SelectedIndexChanged
        pnlGridHistorico.Visible = False
    End Sub

    Protected Sub drpOrigemDados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpOrigemDados.SelectedIndexChanged
        pnlGridHistorico.Visible = False
    End Sub
End Class
