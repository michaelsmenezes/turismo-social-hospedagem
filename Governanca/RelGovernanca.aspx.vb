Imports dtsRelGovernancaTableAdapters
Imports Microsoft.Reporting.WebForms
Imports System.Configuration
Imports System.Web.Configuration
Imports Turismo

Partial Class RelGovernanca
    Inherits System.Web.UI.Page
    Dim ObjRelGovernancaVO As RelGovernacaVO
    Dim ObjRelGovernancaDAO As RelGovernacaDAO
    Dim ObjAlaVO As AlaVO
    Dim ObjAlaDAO As AlaDAO
    Dim ObjCamareiraVO As CamareiraVO
    Dim ObjCamareiraDAO As CamareiraDAO
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnUnidade.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Direcionando para o help desk de Caldas Novas
                    btnUnidade.Attributes.Add("AliasBancoHdManutencao", "HDManutencao")
                    'Usado na pagina de relatórios
                    btnUnidade.Attributes.Add("UOP", "Caldas Novas")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnUnidade.Attributes.Add("BancoTurismoSocial", "dbTurismoSocial")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheet.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnUnidade.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Direcionando para o help desk 
                    btnUnidade.Attributes.Add("AliasBancoHdManutencao", "HDManutencaoPiri")
                    'Usado na pagina de relatórios
                    btnUnidade.Attributes.Add("UOP", "Pirenopolis")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnUnidade.Attributes.Add("BancoTurismoSocial", "dbTurismoSocialPiri")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
            End Select
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("CNV_GOVERNANCA") Or Grupos.Contains("Turismo Social Total")) Then
                Response.Redirect("AcessoNegado.aspx")
            End If
            'Montando os blocos, Caldas e Pirenopolis'
            drpBloco.Items.Clear()
            Select Case btnUnidade.Attributes.Item("UOP").ToString
                Case "Caldas Novas"
                    drpBloco.Items.Insert(0, New ListItem("Todos...", "0"))
                    drpBloco.Items.Insert(1, New ListItem("Rio Tocantins", "1"))
                    drpBloco.Items.Insert(2, New ListItem("Rio Araguaia", "2"))
                    drpBloco.Items.Insert(3, New ListItem("Rio Paranaiba", "3"))
                    drpBloco.Items.Insert(4, New ListItem("Rio Vermelho", "33"))
                    drpBloco.SelectedValue = 0
                Case "Pirenopolis"
                    Response.Cookies("UOP").Value = "Pirenopolis"
                    drpBloco.Items.Insert(0, New ListItem("Selecione...", "0"))
                    drpBloco.Items.Insert(1, New ListItem("Bloco A", "1"))
                    drpBloco.Items.Insert(2, New ListItem("Bloco B/C/D", "2"))
                    drpBloco.Items.Insert(3, New ListItem("Bloco E/F/G", "3"))
                    drpBloco.SelectedValue = 0
            End Select
            pnlConsulta_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlConsulta_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            ''pnlCadastroCam_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            ''pnlCadastroCam_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            txtData.Text = Format(Date.Today, "dd/MM/yyyy")
            txtData.Attributes.Add("OnkeyPress", "javascript:FormataData(this,event)")
            txtData.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtData.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtData.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            btnCadCamareira.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtNomeCad.value=''")
            'USADO PARA EXECUTAR A CONSULTA'
            btnConsultar.Attributes.Add("OnClick", "Javascript:ctl00_conPlaHolTurismoSocial_btnExecRelatorio.click()")
            drpBloco.Attributes.Add("OnChange", "Javascript:ctl00_conPlaHolTurismoSocial_btnEscondeRel.click()")
            drpAla.Attributes.Add("OnChange", "Javascript:ctl00_conPlaHolTurismoSocial_btnEscondeRel.click()")
            drpCamareira.Attributes.Add("OnChange", "Javascript:ctl00_conPlaHolTurismoSocial_btnEscondeRel.click()")
            drpStatus.Attributes.Add("OnChange", "Javascript:ctl00_conPlaHolTurismoSocial_btnEscondeRel.click()")
            'CARREGANDO LISTA DE CAMAREIRAS'
            ObjCamareiraVO = New CamareiraVO
            ObjCamareiraDAO = New CamareiraDAO()
            drpCamareira.DataValueField = "CamId"
            drpCamareira.DataTextField = "CamNome"
            drpCamareira.DataSource = ObjCamareiraDAO.ConsultarCamareira(ObjCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            drpCamareira.DataBind()
            drpCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            drpCamareira.SelectedValue = 0

            Select Case btnUnidade.Attributes.Item("UOP").ToString
                Case "Caldas Novas"
                    'CARREGANDO ALA INICIALMENTE
                    drpAla.Items.Insert(0, New ListItem("Todas", "0"))
                    drpAla.SelectedValue = 0
                Case "Pirenopolis"
                    drpCamareira.Enabled = False
                    lblTituloBloco.Visible = False
                    drpBloco.Visible = False
                    drpAla.Items.Clear()
                    ObjAlaVO = New AlaVO
                    ObjAlaDAO = New AlaDAO()
                    drpAla.DataValueField = "AlaId"
                    drpAla.DataTextField = "AlaDescricao"
                    ObjAlaVO.AlaNome = ""
                    drpAla.DataSource = ObjAlaDAO.ConsultaAla(ObjAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    drpAla.DataBind()
                    drpAla.Items.Insert(0, New ListItem("Todas", "0"))
                    drpAla.SelectedValue = 0
            End Select
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        'VEJA O LOAD DA PÁGINA, ESTA DANDO UM CLICK NO EVENTO BTNEXECRELATORIO'
    End Sub

    Protected Sub btnExecRelatorio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExecRelatorio.Click
        Select Case drpStatus.SelectedValue
            Case 0
                TodosStatus(btnUnidade.Attributes.Item("UOP").ToString)
            Case "P"
                Prioridade(btnUnidade.Attributes.Item("UOP").ToString)
            Case "A"
                Arrumacao(btnUnidade.Attributes.Item("UOP").ToString)
            Case "R"
                Revisao(btnUnidade.Attributes.Item("UOP").ToString)
            Case "T"
                Atendimento(btnUnidade.Attributes.Item("UOP").ToString)
            Case "X" 'total de pax
                TotalPax(btnUnidade.Attributes.Item("UOP").ToString)
        End Select
    End Sub

    Public Sub Prioridade(ByVal UnidadeOperacional As String)
        Dim Ala As Integer = drpAla.SelectedValue
        Dim Bloco As String = ""
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = "0"     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Passando os parametros'
        Dim ParamsSugestoes(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("Dia", txtData.Text)
        ParamsSugestoes(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsSugestoes(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsSugestoes(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsSugestoes(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)
        Dim VarPrioridade As StringBuilder
        VarPrioridade = New StringBuilder("")
        With VarPrioridade
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            .AppendLine("declare @ApaStatus char(1), @Qtde smallint, @DataIni datetime, @SolId numeric, @Integrante smallint, ")
            .AppendLine("@SolBerco smallint, @CC smallint, @CS smallint, @CE smallint, @BE smallint, @JT smallint, @FR smallint, ")
            .AppendLine("@ApaCamaExtra smallint, @ApaBerco smallint, @ApaId smallint,@AlaId smallint, @ApaDesc varchar(10), ")
            .AppendLine("@AcmBicama char(1), @AcmSofacama char(1) ")
            .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
            .AppendLine("from TbParametro with (nolock) ")
            '.AppendLine("-- ")

            '.AppendLine("create table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")

            .AppendLine("declare @Cont integer, @Aux integer ")
            .AppendLine("set @Cont = 1 ")
            .AppendLine("while Len(@ListaCaract) > @Cont begin ")
            .AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")

            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")

            .AppendLine("set @Cont = @Aux + 1 ")
            .AppendLine("end ")
            '.AppendLine("-- ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            '.AppendLine("-- ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ")
            'washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("create index ISApaId on @Apto ( ApaId ASC ) ")
            'Washington
            '.AppendLine("create index ISAlaId on @Apto ( AlaId ASC ) ")
            '.AppendLine("create index ISAptoCC on @Apto ( AptoCC ASC ) ")
            '.AppendLine("create index ISAptoCS on @Apto ( AptoCS ASC ) ")
            '.AppendLine("create index ISAptoCE on @Apto ( AptoCE ASC ) ")
            '.AppendLine("create index ISAptoBE on @Apto ( AptoBE ASC ) ")
            '.AppendLine("create index ISAptoFR on @Apto ( AptoFR ASC ) ")
            '.AppendLine("create index ISAptoJT on @Apto ( AptoJT ASC ) ")
            '.AppendLine("create index ISAptoCEAtual on @Apto ( AptoCEAtual ASC ) ")
            '.AppendLine("create index ISAptoBEAtual on @Apto ( AptoBEAtual ASC ) ")
            '.AppendLine("--declare @Dia char(10) ")
            .AppendLine("declare @DiaDate datetime, ")
            .AppendLine("@LimiteIDadeBerco integer ")
            '.AppendLine("--set @Dia = convert(char(10), getdate(), 103) ")
            .AppendLine("set @DiaDate = convert(datetime, @Dia, 103) ")
            .AppendLine("select @LimiteIdadeBerco = LimiteIdadeBerco from TbParametro ")

            .AppendLine("declare Prioridade_cursor cursor for ")
            .AppendLine("select h.SolId, h.ApaId, min(h.HosDataIniSol) as HosDataIniSol, ")
            .AppendLine("count(h.HosId) ")
            .AppendLine("as Integrante, ")
            .AppendLine("sum(case when DbFuncao.Dbo.FuIdade(IntDtNascimento, IntDataIni) < @LimiteIdadeBerco then 1 else 0 end) SolBerco ")
            .AppendLine("from TbHospedagem h with (nolock) inner join TbSolicitacao s with (nolock) ")
            .AppendLine("on h.SolId = s.SolId ")
            .AppendLine("inner join TbIntegrante i with (nolock) on i.IntId = h.IntId ")

            '.AppendLine("where @DiaDate between dateadd(hh, - datepart(hh, h.HosDataIniSol), ")
            '.AppendLine("h.HosDataIniSol) and dateadd(hh, - datepart(hh, h.HosDataFimSol), h.HosDataFimSol) ")

            .AppendLine("  where (((convert(datetime,convert(char(11), @DiaDate)) ")
            .AppendLine("  between convert(datetime, convert(char(11), h.HosDataIniSol))")
            .AppendLine("  and convert(datetime, convert(char(11), h.HosDataFimSol))))) ")
            .AppendLine("  and convert(datetime,CONVERT(char(11), h.HosDataFimSol)) > CONVERT(datetime, CONVERT(CHAR(11), getdate())) ")
            .AppendLine("  and h.HosDataFimReal is null ")

            .AppendLine("group by h.SolId, h.ApaId ")
            '.AppendLine("order by h.HosDataIniSol, h.ApaId desc, ")
            .AppendLine("order by HosDataIniSol, ApaId desc, ")
            .AppendLine("Integrante desc ")
            .AppendLine("OPTION (OPTIMIZE FOR(@DiaDate='2012-01-01')) ")
            '.AppendLine("-- ")
            .AppendLine("open Prioridade_cursor ")
            '.AppendLine("-- ")
            .AppendLine("fetch next from Prioridade_cursor ")
            .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            .AppendLine("while @@fetch_status = 0 ")
            .AppendLine("begin ")
            .AppendLine("if @ApaId is null ")
            .AppendLine("begin ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--      and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--      and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '1' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '2' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT =  ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '3' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '4' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("select top 1 @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco, @ApaDesc = a.ApaDesc ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            .AppendLine("and d.ApaId = @ApaId ")
            .AppendLine("and d.Status = 'A' ")
            '.AppendLine("where (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--  if (@ApaStatus not in ('2','4')) or ((@ApaCamaExtra <> @CE) or (@ApaBerco <> @BE)) ")
            .AppendLine("begin ")
            .AppendLine("set @ApaDesc = (select ApaDesc from TbApartamento where ApaId = @ApaId) ")
            'Washington
            .AppendLine("set @AlaId = (select AlaId from TbApartamento where ApaId = @ApaId) ")
            .AppendLine("if @ApaStatus in ('1','3') ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaCamaExtra <> @CE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaCamaExtra > @CE ")
            .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            .AppendLine("else ")
            .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("set @ApaCamaExtra = 0 ")
            .AppendLine("set @CE = 0 ")
            .AppendLine("end ")
            .AppendLine("if (@ApaBerco <> @BE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaBerco > @BE ")
            .AppendLine("set @BE = @BE - @ApaBerco ")
            .AppendLine("else ")
            .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("set @ApaBerco = 0 ")
            .AppendLine("set @BE = 0 ")
            .AppendLine("end ")
            .AppendLine("set @FR = @CE + @BE ")
            .AppendLine("set @JT = @CE + @BE ")
            .AppendLine("set @CC = 0 ")
            .AppendLine("set @CS = 0 ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaCamaExtra <> @CE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaCamaExtra > @CE ")
            .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            .AppendLine("else ")
            .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("set @CE = 0 ")
            .AppendLine("if (@ApaBerco <> @BE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaBerco > @BE ")
            .AppendLine("set @BE = @BE - @ApaBerco ")
            .AppendLine("else ")
            .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("set @BE = 0 ")
            .AppendLine("end ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is not null) ")
            '.AppendLine(" and ((select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and c.ApaId = @ApaId) = @Qtde) ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaStatus = 3) and not exists (select 1 from TbHospedagem with (nolock) where SolId = @SolId and HosDataIniReal is not null) ")
            .AppendLine("set @ApaStatus = 5 ")
            'Washington
            .AppendLine("insert @Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT, AptoStatus, AptoCEAtual, AptoBEAtual, Hora) ")
            .AppendLine("values (@ApaId, @AlaId, @ApaDesc, @CC, @CS, @CE, @BE, @FR, @JT, @ApaStatus, @ApaCamaExtra, @ApaBerco, datepart(hh, @DataIni)) ")
            .AppendLine("end ")
            .AppendLine("end ")
            .AppendLine("fetch next from Prioridade_cursor ")
            .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            .AppendLine("end ")
            '.AppendLine("--")
            .AppendLine("close Prioridade_cursor ")
            .AppendLine("deallocate Prioridade_cursor ")
            '.AppendLine("--")
            .AppendLine("update @Apto set ")
            .AppendLine("AptoFR = AptoFR - AptoCE - AptoBE, ")
            .AppendLine("AptoJT = AptoJT - AptoCE - AptoBE, ")
            .AppendLine("AptoCE = 0, ")
            .AppendLine("AptoBE = 0 ")
            '.AppendLine("--")

            .AppendLine("select a.* from @Apto a ")
            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId  ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")

            .AppendLine("where ((AptoCC > 0) or (AptoCS > 0) or (AptoCE > 0) ")
            .AppendLine("or (AptoBE > 0) or (AptoFR> 0) or (AptoJT> 0)) ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("and a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.apaid ")
            '.AppendLine("--    or (AptoCEAtual > 0) or (AptoBEAtual > 0)) -- or (AptoStatus = 5)) ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table #Caracteristica ")

            'With VarPrioridade
            '    .AppendLine("declare ")
            '    .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            '    .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            '    .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            '    .AppendLine("set nocount on ")
            '    .AppendLine("declare @ApaStatus char(1), @Qtde smallint, @DataIni datetime, @SolId numeric, @Integrante smallint, ")
            '    .AppendLine("@SolBerco smallint, @CC smallint, @CS smallint, @CE smallint, @BE smallint, @JT smallint, @FR smallint, ")
            '    .AppendLine("@ApaCamaExtra smallint, @ApaBerco smallint, @ApaId smallint,@AlaId smallint, @ApaDesc varchar(10), ")
            '    .AppendLine("@AcmBicama char(1), @AcmSofacama char(1) ")
            '    .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
            '    .AppendLine("from TbParametro with (nolock) ")
            '    '.AppendLine("-- ")

            '    '.AppendLine("create table #Caracteristica( ")
            '    '.AppendLine("CId numeric primary Key) ")

            '    .AppendLine("declare @Cont integer, @Aux integer ")
            '    .AppendLine("set @Cont = 1 ")
            '    .AppendLine("while Len(@ListaCaract) > @Cont begin ")
            '    .AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")

            '    '.AppendLine("insert #Caracteristica(CId) ")
            '    '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")

            '    .AppendLine("set @Cont = @Aux + 1 ")
            '    .AppendLine("end ")
            '    '.AppendLine("-- ")
            '    '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '    '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '    '.AppendLine("on cc.CId = c.CarId ")
            '    '.AppendLine("-- ")
            '    .AppendLine("create table #Apto( ")
            '    .AppendLine("ApaId smallint, ")
            '    'washington
            '    .AppendLine("AlaId smallint, ")
            '    .AppendLine("ApaDesc varchar(10), ")
            '    .AppendLine("AptoStatus char(1), ")
            '    .AppendLine("AptoCC smallint, ")
            '    .AppendLine("AptoCS smallint, ")
            '    .AppendLine("AptoCE smallint, ")
            '    .AppendLine("AptoCEAtual smallint, ")
            '    .AppendLine("AptoBE smallint, ")
            '    .AppendLine("AptoBEAtual smallint, ")
            '    .AppendLine("AptoFR smallint, ")
            '    .AppendLine("AptoJT smallint, ")
            '    .AppendLine("Hora smallint) ")
            '    '.AppendLine("-- ")
            '    .AppendLine("create index ISApaId on #Apto ( ApaId ASC ) ")
            '    'Washington
            '    .AppendLine("create index ISAlaId on #Apto ( AlaId ASC ) ")
            '    .AppendLine("create index ISAptoCC on #Apto ( AptoCC ASC ) ")
            '    .AppendLine("create index ISAptoCS on #Apto ( AptoCS ASC ) ")
            '    .AppendLine("create index ISAptoCE on #Apto ( AptoCE ASC ) ")
            '    .AppendLine("create index ISAptoBE on #Apto ( AptoBE ASC ) ")
            '    .AppendLine("create index ISAptoFR on #Apto ( AptoFR ASC ) ")
            '    .AppendLine("create index ISAptoJT on #Apto ( AptoJT ASC ) ")
            '    .AppendLine("create index ISAptoCEAtual on #Apto ( AptoCEAtual ASC ) ")
            '    .AppendLine("create index ISAptoBEAtual on #Apto ( AptoBEAtual ASC ) ")
            '    '.AppendLine("--declare @Dia char(10) ")
            '    .AppendLine("declare @DiaDate datetime, ")
            '    .AppendLine("@LimiteIDadeBerco integer ")
            '    '.AppendLine("--set @Dia = convert(char(10), getdate(), 103) ")
            '    .AppendLine("set @DiaDate = convert(datetime, @Dia, 103) ")
            '    .AppendLine("select @LimiteIdadeBerco = LimiteIdadeBerco from TbParametro ")

            '    .AppendLine("declare Prioridade_cursor cursor for ")
            '    .AppendLine("select h.SolId, h.ApaId, min(h.HosDataIniSol) as HosDataIniSol, ")
            '    .AppendLine("count(h.HosId) ")
            '    .AppendLine("as Integrante, ")
            '    .AppendLine("sum(case when DbFuncao.Dbo.FuIdade(IntDtNascimento, IntDataIni) < @LimiteIdadeBerco then 1 else 0 end) SolBerco ")
            '    .AppendLine("from TbHospedagem h with (nolock) inner join TbSolicitacao s with (nolock) ")
            '    .AppendLine("on h.SolId = s.SolId ")
            '    .AppendLine("inner join TbIntegrante i with (nolock) on i.IntId = h.IntId ")

            '    '.AppendLine("where @DiaDate between dateadd(hh, - datepart(hh, h.HosDataIniSol), ")
            '    '.AppendLine("h.HosDataIniSol) and dateadd(hh, - datepart(hh, h.HosDataFimSol), h.HosDataFimSol) ")

            '    .AppendLine("  where (((convert(datetime,convert(char(11), @DiaDate)) ")
            '    .AppendLine("  between convert(datetime, convert(char(11), h.HosDataIniSol))")
            '    .AppendLine("  and convert(datetime, convert(char(11), h.HosDataFimSol))))) ")
            '    .AppendLine("  and convert(datetime,CONVERT(char(11), h.HosDataFimSol)) > CONVERT(datetime, CONVERT(CHAR(11), getdate())) ")
            '    .AppendLine("  and h.HosDataFimReal is null ")

            '    .AppendLine("group by h.SolId, h.ApaId ")
            '    '.AppendLine("order by h.HosDataIniSol, h.ApaId desc, ")
            '    .AppendLine("order by HosDataIniSol, ApaId desc, ")
            '    .AppendLine("Integrante desc ")
            '    .AppendLine("OPTION (OPTIMIZE FOR(@DiaDate='2012-01-01')) ")
            '    '.AppendLine("-- ")
            '    .AppendLine("open Prioridade_cursor ")
            '    '.AppendLine("-- ")
            '    .AppendLine("fetch next from Prioridade_cursor ")
            '    .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            '    .AppendLine("while @@fetch_status = 0 ")
            '    .AppendLine("begin ")
            '    .AppendLine("if @ApaId is null ")
            '    .AppendLine("begin ")
            '    .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            '    .AppendLine("@CS = a.AcmCS - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@CE = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@BE = @SolBerco, ")
            '    .AppendLine("@JT = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@FR = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            '    .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            '    .AppendLine("on d.ApaId = a.ApaId ")
            '    .AppendLine("and d.SolId = @SolId ")
            '    '.AppendLine("--      and a.ApaBerco = @SolBerco ")
            '    .AppendLine("and d.Status = 'D' ")
            '    '.AppendLine("--      and a.Integrante = @Integrante ")
            '    .AppendLine("and a.Ordem = '1' ")
            '    .AppendLine("where not exists (select 1 from #Apto p where p.ApaId = d.ApaId) ")
            '    '.AppendLine("and (case ")
            '    '.AppendLine("when @Qtde > 0 then ")
            '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and a.ApaId = c.ApaId) ")
            '    '.AppendLine("else 0 end) = @Qtde ")
            '    .AppendLine("order by a.Ordem ")
            '    '.AppendLine("--")
            '    .AppendLine("if (@ApaId is null) ")
            '    .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            '    .AppendLine("@CS = a.AcmCS - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@CE = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@BE = @SolBerco, ")
            '    .AppendLine("@JT = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@FR = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            '    .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            '    .AppendLine("on d.ApaId = a.ApaId ")
            '    .AppendLine("and d.SolId = @SolId ")
            '    '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            '    .AppendLine("and d.Status = 'D' ")
            '    '.AppendLine("--        and a.Integrante = @Integrante ")
            '    .AppendLine("and a.Ordem = '2' ")
            '    .AppendLine("where not exists (select 1 from #Apto p where p.ApaId = d.ApaId) ")
            '    '.AppendLine("and (case ")
            '    '.AppendLine("when @Qtde > 0 then ")
            '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and a.ApaId = c.ApaId) ")
            '    '.AppendLine("else 0 end) = @Qtde ")
            '    .AppendLine("order by a.Ordem ")
            '    '.AppendLine("--")
            '    .AppendLine("if (@ApaId is null) ")
            '    .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            '    .AppendLine("@CS = a.AcmCS - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@CE = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@BE = @SolBerco, ")
            '    .AppendLine("@JT =  ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@FR = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            '    .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            '    .AppendLine("on d.ApaId = a.ApaId ")
            '    .AppendLine("and d.SolId = @SolId ")
            '    '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            '    .AppendLine("and d.Status = 'D' ")
            '    '.AppendLine("--        and a.Integrante = @Integrante ")
            '    .AppendLine("and a.Ordem = '3' ")
            '    .AppendLine("where not exists (select 1 from #Apto p where p.ApaId = d.ApaId) ")
            '    '.AppendLine("and (case ")
            '    '.AppendLine("when @Qtde > 0 then ")
            '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and a.ApaId = c.ApaId) ")
            '    '.AppendLine("else 0 end) = @Qtde ")
            '    .AppendLine("order by a.Ordem ")
            '    '.AppendLine("--")
            '    .AppendLine("if (@ApaId is null) ")
            '    .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            '    .AppendLine("@CS = a.AcmCS - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@CE = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@BE = @SolBerco, ")
            '    .AppendLine("@JT = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@FR = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            '    .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            '    .AppendLine("on d.ApaId = a.ApaId ")
            '    .AppendLine("and d.SolId = @SolId ")
            '    '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            '    .AppendLine("and d.Status = 'D' ")
            '    '.AppendLine("--        and a.Integrante = @Integrante ")
            '    .AppendLine("and a.Ordem = '4' ")
            '    .AppendLine("where not exists (select 1 from #Apto p where p.ApaId = d.ApaId) ")
            '    '.AppendLine("and (case ")
            '    '.AppendLine("when @Qtde > 0 then ")
            '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and a.ApaId = c.ApaId) ")
            '    '.AppendLine("else 0 end) = @Qtde ")
            '    .AppendLine("order by a.Ordem ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("select top 1 @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            '    .AppendLine("@CS = a.AcmCS - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@CE = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end, ")
            '    .AppendLine("@BE = @SolBerco, ")
            '    .AppendLine("@JT = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@FR = ")
            '    .AppendLine("case ")
            '    .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            '    .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end - ")
            '    .AppendLine("case ")
            '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            '    .AppendLine("else 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("end, ")
            '    .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco, @ApaDesc = a.ApaDesc ")
            '    .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            '    .AppendLine("on d.ApaId = a.ApaId ")
            '    .AppendLine("and d.SolId = @SolId ")
            '    .AppendLine("and d.ApaId = @ApaId ")
            '    .AppendLine("and d.Status = 'A' ")
            '    '.AppendLine("where (case ")
            '    '.AppendLine("when @Qtde > 0 then ")
            '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and a.ApaId = c.ApaId) ")
            '    '.AppendLine("else 0 end) = @Qtde ")
            '    .AppendLine("order by a.Ordem ")
            '    '.AppendLine("--  if (@ApaStatus not in ('2','4')) or ((@ApaCamaExtra <> @CE) or (@ApaBerco <> @BE)) ")
            '    .AppendLine("begin ")
            '    .AppendLine("set @ApaDesc = (select ApaDesc from TbApartamento where ApaId = @ApaId) ")
            '    'Washington
            '    .AppendLine("set @AlaId = (select AlaId from TbApartamento where ApaId = @ApaId) ")
            '    .AppendLine("if @ApaStatus in ('1','3') ")
            '    .AppendLine("begin ")
            '    .AppendLine("if (@ApaCamaExtra <> @CE) ")
            '    .AppendLine("begin ")
            '    .AppendLine("if @ApaCamaExtra > @CE ")
            '    .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            '    .AppendLine("else ")
            '    .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("begin ")
            '    .AppendLine("set @ApaCamaExtra = 0 ")
            '    .AppendLine("set @CE = 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("if (@ApaBerco <> @BE) ")
            '    .AppendLine("begin ")
            '    .AppendLine("if @ApaBerco > @BE ")
            '    .AppendLine("set @BE = @BE - @ApaBerco ")
            '    .AppendLine("else ")
            '    .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("begin ")
            '    .AppendLine("set @ApaBerco = 0 ")
            '    .AppendLine("set @BE = 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("set @FR = @CE + @BE ")
            '    .AppendLine("set @JT = @CE + @BE ")
            '    .AppendLine("set @CC = 0 ")
            '    .AppendLine("set @CS = 0 ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("begin ")
            '    .AppendLine("if (@ApaCamaExtra <> @CE) ")
            '    .AppendLine("begin ")
            '    .AppendLine("if @ApaCamaExtra > @CE ")
            '    .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            '    .AppendLine("else ")
            '    .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("set @CE = 0 ")
            '    .AppendLine("if (@ApaBerco <> @BE) ")
            '    .AppendLine("begin ")
            '    .AppendLine("if @ApaBerco > @BE ")
            '    .AppendLine("set @BE = @BE - @ApaBerco ")
            '    .AppendLine("else ")
            '    .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            '    .AppendLine("end ")
            '    .AppendLine("else ")
            '    .AppendLine("set @BE = 0 ")
            '    .AppendLine("end ")
            '    '.AppendLine("--")
            '    .AppendLine("if (@ApaId is not null) ")
            '    '.AppendLine(" and ((select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '    '.AppendLine("(select CId from #Caracteristica) ")
            '    '.AppendLine("and c.ApaId = @ApaId) = @Qtde) ")
            '    .AppendLine("begin ")
            '    .AppendLine("if (@ApaStatus = 3) and not exists (select 1 from TbHospedagem with (nolock) where SolId = @SolId and HosDataIniReal is not null) ")
            '    .AppendLine("set @ApaStatus = 5 ")
            '    'Washington
            '    .AppendLine("insert #Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT, AptoStatus, AptoCEAtual, AptoBEAtual, Hora) ")
            '    .AppendLine("values (@ApaId, @AlaId, @ApaDesc, @CC, @CS, @CE, @BE, @FR, @JT, @ApaStatus, @ApaCamaExtra, @ApaBerco, datepart(hh, @DataIni)) ")
            '    .AppendLine("end ")
            '    .AppendLine("end ")
            '    .AppendLine("fetch next from Prioridade_cursor ")
            '    .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            '    .AppendLine("end ")
            '    '.AppendLine("--")
            '    .AppendLine("close Prioridade_cursor ")
            '    .AppendLine("deallocate Prioridade_cursor ")
            '    '.AppendLine("--")
            '    .AppendLine("update #Apto set ")
            '    .AppendLine("AptoFR = AptoFR - AptoCE - AptoBE, ")
            '    .AppendLine("AptoJT = AptoJT - AptoCE - AptoBE, ")
            '    .AppendLine("AptoCE = 0, ")
            '    .AppendLine("AptoBE = 0 ")
            '    '.AppendLine("--")

            '    .AppendLine("select a.* from #Apto a ")
            '    .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId  ")
            '    .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")

            '    .AppendLine("where ((AptoCC > 0) or (AptoCS > 0) or (AptoCE > 0) ")
            '    .AppendLine("or (AptoBE > 0) or (AptoFR> 0) or (AptoJT> 0)) ")
            '    If Bloco > 0 Then
            '        .AppendLine("and ac.BloId = '" & Bloco & "' ")
            '    End If
            '    'FILTRANDO POR ALA
            '    If Ala <> 0 Then
            '        .AppendLine("and a.AlaId = " & Ala & " ")
            '    End If
            '    .AppendLine("order by a.apaid ")
            '    '.AppendLine("--    or (AptoCEAtual > 0) or (AptoBEAtual > 0)) -- or (AptoStatus = 5)) ")
            '    .AppendLine("drop table #Apto ")
            '    '.AppendLine("drop table #Caracteristica ")

        End With

        Dim ordersPrioridadeLinha As DataTablePrioridadeTableAdapter = New DataTablePrioridadeTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelPrioridadeLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarPrioridade.ToString)
        Dim rdsRelPrioridadeLinha As New ReportDataSource("dtsRelGovernanca_DataTablePrioridade", ordersRelPrioridadeLinha)
        '
        If ordersPrioridadeLinha.GetData(VarPrioridade.ToString).Rows.Count <> 0 Then
            'Chamando o relatório'
            rptArrumacao.Visible = False
            rptRevisao.Visible = False
            rptAtendimento.Visible = False
            rptTodos.Visible = False
            rptPrioridade.Visible = True
            rptPrioridade.LocalReport.DataSources.Clear()
            rptPrioridade.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
            rptPrioridade.LocalReport.SetParameters(ParamsSugestoes)
        Else
            rptArrumacao.Visible = False
            rptRevisao.Visible = False
            rptAtendimento.Visible = False
            rptTodos.Visible = False
            rptPrioridade.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
    End Sub
    Public Sub Revisao(ByVal UnidadeOperacional As String)
        Dim ala As Long = drpAla.SelectedValue
        Dim Bloco As String = ""
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = "0"     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Dim Andar As String = ""
        'Select Case drpAndar.SelectedValue
        '    Case 0 : Andar = ""
        '    Case 5 : Andar = "5."   '1º andar
        '    Case 6 : Andar = "6."   '2º andar
        '    Case 11 : Andar = "11." 'Adaptado para pessoas especiais
        '    Case 28 : Andar = "28." '3º andar
        '    Case 29 : Andar = "29." '4º andar
        '    Case 30 : Andar = "30." '5º andar
        '    Case 31 : Andar = "31." '6º andar
        '    Case 32 : Andar = "32." '7º andar
        'End Select
        'Passando os parametros'
        Dim ParamsSugestoes(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("Dia", Format(CDate(txtData.Text), "dd/MM/yyyy"))
        ParamsSugestoes(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsSugestoes(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsSugestoes(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsSugestoes(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)
        'REVISÃO'
        Dim VarRevisao As StringBuilder
        VarRevisao = New StringBuilder("")
        With VarRevisao
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            '.AppendLine("-- ")
            .AppendLine("set nocount on ")
            .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @RevisaoGovernancaHoras smallint ")
            '.AppendLine("-- ")
            '.AppendLine("create table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")
            .AppendLine("set @Cont = 1 ")
            '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
            '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
            '.AppendLine("set @Cont = @Aux + 1 ")
            '.AppendLine("end ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            '.AppendLine("-- ")
            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AlaId smallint, ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("create index ISApaId on @AptoPrioridade ( ApaId ASC ) ")
            '.AppendLine("-- ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ApaDesc varchar(10), AlaId smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("create index ISApaId1 on @Apto ( ApaId ASC ) ")
            '.AppendLine("-- ")
            .AppendLine("select @RevisaoGovernancaHoras = RevisaoGovernancaHoras from TbParametro with (nolock) ")
            '.AppendLine("-- ")
            .AppendLine("insert @Apto (ApaId, ApaDesc, AlaId) ")
            .AppendLine("select a.ApaId, a.ApaDesc,a.AlaId from TbApartamento a with (nolock) ")
            .AppendLine("where a.ApaStatus = 'L' ")
            .AppendLine("and not exists ")
            .AppendLine("(select 1 from TbAtendimentoGov ag with (nolock) where ag.ApaId = a.ApaId ")
            .AppendLine("and AGoData between ")
            .AppendLine("dateadd(hh, -@RevisaoGovernancaHoras, dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
            .AppendLine("and dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
            .AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            '.AppendLine("-- ")
            .AppendLine("OPTION (OPTIMIZE FOR(@Dia='2012-01-01')) ")

            .AppendLine("select * from @Apto a ")
            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If
            'FILTRANDO POR ALA
            If ala <> 0 Then
                .AppendLine("where a.AlaId = " & ala & " ")
            End If
            .AppendLine("order by a.ApaId  ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With


        'With VarRevisao
        '    .AppendLine("declare ")
        '    .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
        '    .AppendLine("set @ListaCaract = '" & Bloco & "' ")
        '    .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
        '    '.AppendLine("-- ")
        '    .AppendLine("set nocount on ")
        '    .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @RevisaoGovernancaHoras smallint ")
        '    '.AppendLine("-- ")
        '    '.AppendLine("create table #Caracteristica( ")
        '    '.AppendLine("CId numeric primary Key) ")
        '    .AppendLine("set @Cont = 1 ")
        '    '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
        '    '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
        '    '.AppendLine("insert #Caracteristica(CId) ")
        '    '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
        '    '.AppendLine("set @Cont = @Aux + 1 ")
        '    '.AppendLine("end ")
        '    '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
        '    '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
        '    '.AppendLine("on cc.CId = c.CarId ")
        '    '.AppendLine("-- ")
        '    .AppendLine("create table #AptoPrioridade( ")
        '    .AppendLine("ApaId smallint, ")
        '    .AppendLine("ApaDesc varchar(10), ")
        '    .AppendLine("AlaId smallint, ")
        '    .AppendLine("AptoStatus char(1), ")
        '    .AppendLine("AptoCC smallint, ")
        '    .AppendLine("AptoCS smallint, ")
        '    .AppendLine("AptoCE smallint, ")
        '    .AppendLine("AptoCEAtual smallint, ")
        '    .AppendLine("AptoBE smallint, ")
        '    .AppendLine("AptoBEAtual smallint, ")
        '    .AppendLine("AptoFR smallint, ")
        '    .AppendLine("AptoJT smallint, ")
        '    .AppendLine("Hora smallint) ")
        '    '.AppendLine("-- ")
        '    .AppendLine("create index ISApaId on #AptoPrioridade ( ApaId ASC ) ")
        '    '.AppendLine("-- ")
        '    .AppendLine("create table #Apto( ")
        '    .AppendLine("ApaId smallint, ApaDesc varchar(10), AlaId smallint) ")
        '    '.AppendLine("-- ")
        '    .AppendLine("create index ISApaId1 on #Apto ( ApaId ASC ) ")
        '    '.AppendLine("-- ")
        '    .AppendLine("select @RevisaoGovernancaHoras = RevisaoGovernancaHoras from TbParametro with (nolock) ")
        '    '.AppendLine("-- ")
        '    .AppendLine("insert #Apto (ApaId, ApaDesc, AlaId) ")
        '    .AppendLine("select a.ApaId, a.ApaDesc,a.AlaId from TbApartamento a with (nolock) ")
        '    .AppendLine("where a.ApaStatus = 'L' ")
        '    .AppendLine("and not exists ")
        '    .AppendLine("(select 1 from TbAtendimentoGov ag with (nolock) where ag.ApaId = a.ApaId ")
        '    .AppendLine("and AGoData between ")
        '    .AppendLine("dateadd(hh, -@RevisaoGovernancaHoras, dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
        '    .AppendLine("and dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
        '    .AppendLine("and not exists (select 1 from #AptoPrioridade p where p.ApaId = a.ApaId) ")
        '    '.AppendLine("and (case ")
        '    '.AppendLine("when @Qtde > 0 then ")
        '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
        '    '.AppendLine("(select CId from #Caracteristica) ")
        '    '.AppendLine("and a.ApaId = c.ApaId) ")
        '    '.AppendLine("else 0 end) = @Qtde ")
        '    '.AppendLine("-- ")
        '    .AppendLine("OPTION (OPTIMIZE FOR(@Dia='2012-01-01')) ")

        '    .AppendLine("select * from #Apto a ")
        '    .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
        '    .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
        '    If Bloco > 0 Then
        '        .AppendLine("and ac.BloId = '" & Bloco & "' ")
        '    End If
        '    'FILTRANDO POR ALA
        '    If ala <> 0 Then
        '        .AppendLine("where a.AlaId = " & ala & " ")
        '    End If
        '    .AppendLine("order by a.ApaId  ")
        '    .AppendLine("drop table #Apto ")
        '    .AppendLine("drop table #AptoPrioridade ")
        '    '.AppendLine("drop table #Caracteristica ")
        'End With

        Dim ordersRevisaoLinha As DataTableRevisaoTableAdapter = New DataTableRevisaoTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersRevisaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersRevisaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelRevisaoLinha As System.Data.DataTable = ordersRevisaoLinha.GetData(VarRevisao.ToString)
        Dim rdsRelRevisaoLinha As New ReportDataSource("dtsRelGovernanca_DataTableRevisao", ordersRelRevisaoLinha)
        If ordersRevisaoLinha.GetData(VarRevisao.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            rptRevisao.Visible = True
            rptAtendimento.Visible = False
            rptPrioridade.Visible = False
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            rptRevisao.LocalReport.DataSources.Clear()
            rptRevisao.LocalReport.DataSources.Add(rdsRelRevisaoLinha)
            rptRevisao.LocalReport.SetParameters(ParamsSugestoes)
        Else
            rptRevisao.Visible = False
            rptAtendimento.Visible = False
            rptPrioridade.Visible = False
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
    End Sub
    Public Sub Atendimento(ByVal UnidadeOperacional As String)
        Dim Bloco As String = ""

        Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
        ConfigurationManager.AppSettings.Set("DbTurismoSocialConnectionString", oTurismo.strConnection.ConnectionString)
        Dim newstring = ConfigurationManager.AppSettings.Get("DbTurismoSocialConnectionString")

        Dim Ala As Integer = drpAla.SelectedValue
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = "0"     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Dim Andar As String = ""
        'Select Case drpAndar.SelectedValue
        '    Case 0 : Andar = ""
        '    Case 5 : Andar = "5."   '1º andar
        '    Case 6 : Andar = "6."   '2º andar
        '    Case 11 : Andar = "11." 'Adaptado para pessoas especiais
        '    Case 28 : Andar = "28." '3º andar
        '    Case 29 : Andar = "29." '4º andar
        '    Case 30 : Andar = "30." '5º andar
        '    Case 31 : Andar = "31." '6º andar
        '    Case 32 : Andar = "32." '7º andar
        'End Select
        'Passando os parametros'
        Dim ParamsSugestoes(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("Dia", Format(CDate(txtData.Text), "dd/MM/yyyy"))
        ParamsSugestoes(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsSugestoes(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsSugestoes(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsSugestoes(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)
        'ATENDIMENTO'
        Dim VarAtendimento As StringBuilder
        VarAtendimento = New StringBuilder("")

        With VarAtendimento
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer ")
            '.AppendLine("create table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")
            .AppendLine("set @Cont = 1 ")
            .AppendLine("while Len(@ListaCaract) > @Cont begin ")
            .AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
            .AppendLine("set @Cont = @Aux + 1 ")
            .AppendLine("end ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("create index ISApaId on @AptoPrioridade ( ApaId ASC ) ")
            .AppendLine("Declare @Integrante table( ")
            .AppendLine("ApaId smallint, ")
            .AppendLine("Integrante smallint) ")
            '.AppendLine("create index ISApaId1 on @Integrante ( ApaId ASC ) ")
            .AppendLine("insert @Integrante (ApaId, Integrante) ")
            .AppendLine("select ApaId, Integrante from VwQuantidadeHospedeApartamento with (nolock) ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("NomeTitular varchar(200), ")
            .AppendLine("TotIntegrante smallint, ")
            .AppendLine("DataInicial DateTime, ")
            .AppendLine("DataFinal Datetime, ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint) ")
            '.AppendLine("create index ISApaId2 on @Apto ( ApaId ASC ) ")
            .AppendLine("declare @DiaDate datetime, @DiaAux datetime ")
            .AppendLine("set @DiaDate = convert(datetime, convert(varchar(10), convert(datetime, @Dia, 103), 103) + ' 23:59', 103) ")
            .AppendLine("set @DiaAux = convert(datetime, convert(varchar(10), convert(datetime, @Dia, 103), 103) + ' 00:00', 103) ")
            .AppendLine("set @DiaAux = dateadd(hh, datepart(hh, getdate()), @DiaAux) ")
            '.AppendLine("insert @Apto (ApaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("insert @Apto (ApaId, AlaId, ApaDesc,NomeTitular,TotIntegrante,DataInicial,DataFinal,AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("select ")
            .AppendLine("a.ApaId, a.AlaId, a.ApaDesc, ")
            'Pegando o nome de um integrante da reserva de maior idade
            .AppendLine("Upper((Select top 1 IntNome from tbintegrante II where II.ResId = Max(R.ResId) group by IntNome order by Max(IntDtNascimento))) as NomeTitular,  ")
            'Somando o total de integrantes de um apartamento
            .AppendLine("(SELECT COUNT(*) FROM TBINTEGRANTE IT ")
            .AppendLine("Inner join TbHospedagem MM on MM.intid = IT.inTId ")
            .AppendLine("WHERE IT.RESID = MAX(R.RESID) ")
            .AppendLine("AND MM.APAID = A.APAID) AS TotIntegrante, ")
            '
            .AppendLine("MAX(IntdataIniReal) as DataInicial, ")
            .AppendLine("MAX(INTDATAFIM)as DataFinal, ")
            .AppendLine("MAX(c.AcmCC) as AptoCC, ")
            .AppendLine("case ")
            .AppendLine("when (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - max(c.AcmCC) > Max(c.AcmCS) then max(c.AcmCS) ")
            .AppendLine("else (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - max(c.AcmCC) ")
            .AppendLine("end as AptoCS, ")
            .AppendLine("max(a.ApaCamaExtra) as AptoCE,MAX(a.ApaBerco) as AptoBE, ")
            .AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoFR, ")
            .AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoJT ")
            .AppendLine("from TbApartamento a with (nolock) ")
            .AppendLine("inner join TbAcomodacao c with (nolock) on a.AcmId = c.AcmId ")
            .AppendLine("inner join TbHospedagem H on A.ApaId = H.ApaId ")
            .AppendLine("inner join TbIntegrante I on I.IntId = H.IntId ")
            .AppendLine("inner join tbReserva R on R.ResId = I.ResId ")
            .AppendLine("where ApaStatus = 'O' ")
            .AppendLine("and I.IntStatus in ('E','P') ")
            .AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("  (select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("Group by a.ApaId,a.ApaDesc,a.AlaId  ")

            'Antiga Select ainda sem o Nome to titular da reserva + Data Ini e data Fim
            '.AppendLine("select a.ApaId, a.ApaDesc, c.AcmCC as AptoCC, ")
            '.AppendLine("case ")
            '.AppendLine("when (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - c.AcmCC > c.AcmCS then c.AcmCS ")
            '.AppendLine("else (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - c.AcmCC ")
            '.AppendLine("end as AptoCS, ")
            '.AppendLine("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
            '.AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoFR, ")
            '.AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoJT ")
            '.AppendLine("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
            '.AppendLine("on a.AcmId = c.AcmId ")
            '.AppendLine("where ApaStatus = 'O' ")
            '.AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("  (select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")

            .AppendLine("select distinct a. *, ")
            .AppendLine("case ")
            .AppendLine("when  ")
            .AppendLine("exists (select top 1 1 from TbHospedagem hh with (nolock) ")
            .AppendLine("where hh.ApaId = a.ApaId ")
            .AppendLine("and hh.HosDataIniSol = h.HosDataFimSol ")
            .AppendLine("and hh.HosStatus = 'F') ")
            .AppendLine("then 'T' ")
            .AppendLine("when ")
            .AppendLine("not exists ")
            .AppendLine("(select top 1 1 from TbHospedagem with (nolock) where ApaId = a.ApaId ")
            .AppendLine("and HosDataIniReal is not null ")
            .AppendLine("and HosDataFimReal is null ")
            .AppendLine("and HosStatus = 'A' ")
            .AppendLine("and HosDataFimSol > @DiaDate) then 'S' ")
            .AppendLine("when ")
            .AppendLine("not exists (select top 1 1 from TbAtendimentoGov a with (nolock) ")
            .AppendLine("where h.ApaId = a.ApaId ")
            .AppendLine("and (abs(datediff(dd, a.AGoData, @DiaAux)) < 2) ")
            .AppendLine("order by a.AGoData desc) ")
            .AppendLine("then 'A' ")
            .AppendLine("else 'N' ")
            .AppendLine("end as CheckOut ")
            .AppendLine("from @Apto a ")
            .AppendLine("inner join TbHospedagem h ")
            .AppendLine("on a.ApaId = h.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = h.acmid ")
            .AppendLine("Where h.HosDataIniReal is not null ")
            .AppendLine("and h.HosDataFimReal is null ")
            .AppendLine("and h.HosStatus = 'A' ")
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("and a.AlaId = " & Ala & " ")
            End If
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If

            .AppendLine("order by a.ApaId ")
            .AppendLine("OPTION (OPTIMIZE FOR(@DiaAux='2012-01-01',@DiaDate='2012-01-01')) ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @Integrante ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With


        Dim ordersAtendimetoLinha As DataTableAtendimentoTableAdapter = New DataTableAtendimentoTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersAtendimetoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersAtendimetoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelAtendimentoLinha As System.Data.DataTable = ordersAtendimetoLinha.GetData(VarAtendimento.ToString)
        Dim rdsRelAtendimentoLinha As New ReportDataSource("dtsRelGovernanca_DataTableAtendimento", ordersRelAtendimentoLinha)
        If ordersAtendimetoLinha.GetData(VarAtendimento.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            rptAtendimento.Visible = True
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            rptPrioridade.Visible = False
            rptRevisao.Visible = False
            rptAtendimento.LocalReport.DataSources.Clear()
            rptAtendimento.LocalReport.DataSources.Add(rdsRelAtendimentoLinha)
            rptAtendimento.LocalReport.SetParameters(ParamsSugestoes)
        Else
            rptAtendimento.Visible = False
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            rptPrioridade.Visible = False
            rptRevisao.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If

    End Sub
    Public Sub TodosStatus(ByVal UnidadeOperacional As String)
        Dim Ala As String = drpAla.SelectedValue
        Dim Bloco As String = ""
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = "0"     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Dim Andar As String = ""
        'Select Case drpAndar.SelectedValue
        '    Case 0 : Andar = ""
        '    Case 5 : Andar = "5."   '1º andar
        '    Case 6 : Andar = "6."   '2º andar
        '    Case 11 : Andar = "11." 'Adaptado para pessoas especiais
        '    Case 28 : Andar = "28." '3º andar
        '    Case 29 : Andar = "29." '4º andar
        '    Case 30 : Andar = "30." '5º andar
        '    Case 31 : Andar = "31." '6º andar
        '    Case 32 : Andar = "32." '7º andar
        'End Select
        ObjRelGovernancaVO = New RelGovernacaVO
        ObjRelGovernancaDAO = New RelGovernacaDAO()
        'Montando Select Apartamento em ARRUMAÇÃO'
        rptArrumacao.Visible = True
        Dim VarArrumacao As StringBuilder
        VarArrumacao = New StringBuilder("SET NOCOUNT ON ")
        With VarArrumacao
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @AcmBicama char(1), @AcmSofacama char(1) ")
            .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
            .AppendLine("from TbParametro with (nolock) ")

            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")

            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")

            '.AppendLine("Declare index ISApaId on @AptoPrioridade ( ApaId ASC ) ")

            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint) ")
            '.AppendLine("Declare index ISApaId1 on @Apto ( ApaId ASC ) ")
            .AppendLine("insert @Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("select a.ApaId,a.AlaId, a.ApaDesc, c.AcmCC as AptoCC, ")
            .AppendLine("c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoCS, ")
            .AppendLine("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
            .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoFR, ")
            .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoJT ")
            .AppendLine("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
            .AppendLine("on a.AcmId = c.AcmId and ApaStatus = 'A' ")
            .AppendLine("where not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            .AppendLine("select * from @Apto a ")
            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("Where a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.ApaId ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With
        Dim ordersArrumacaoLinha As DataTableArrumacaoAdapter = New DataTableArrumacaoAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersArrumacaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersArrumacaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelArrumacaoLinha As System.Data.DataTable = ordersArrumacaoLinha.GetData(VarArrumacao.ToString)
        Dim rdsRelArrumacaoLinha As New ReportDataSource("dtsRelGovernanca_DataTableArrumacao", ordersRelArrumacaoLinha)
        'Passando os parametros'
        Dim ParamsSugestoes(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("Dia", Format(CDate(txtData.Text), "dd/MM/yyyy"))
        ParamsSugestoes(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsSugestoes(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsSugestoes(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsSugestoes(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)


        'PRIORIDADE
        Dim VarPrioridade As StringBuilder
        VarPrioridade = New StringBuilder("")
        With VarPrioridade
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            .AppendLine("declare @ApaStatus char(1), @Qtde smallint, @DataIni datetime, @SolId numeric, @Integrante smallint, ")
            .AppendLine("@SolBerco smallint, @CC smallint, @CS smallint, @CE smallint, @BE smallint, @JT smallint, @FR smallint, ")
            .AppendLine("@ApaCamaExtra smallint, @ApaBerco smallint, @ApaId smallint,@AlaId smallint, @ApaDesc varchar(10), ")
            .AppendLine("@AcmBicama char(1), @AcmSofacama char(1) ")
            .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
            .AppendLine("from TbParametro with (nolock) ")
            '.AppendLine("-- ")

            '.AppendLine("Declare table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")

            .AppendLine("declare @Cont integer, @Aux integer ")
            .AppendLine("set @Cont = 1 ")
            .AppendLine("while Len(@ListaCaract) > @Cont begin ")
            .AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")

            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")

            .AppendLine("set @Cont = @Aux + 1 ")
            .AppendLine("end ")
            '.AppendLine("-- ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            '.AppendLine("-- ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ")
            'washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("Declare index ISApaId on @Apto ( ApaId ASC ) ")
            '.AppendLine("Declare index ISAlaId on @Apto ( AlaId ASC ) ")
            '.AppendLine("Declare index ISAptoCC on @Apto ( AptoCC ASC ) ")
            '.AppendLine("Declare index ISAptoCS on @Apto ( AptoCS ASC ) ")
            '.AppendLine("Declare index ISAptoCE on @Apto ( AptoCE ASC ) ")
            '.AppendLine("Declare index ISAptoBE on @Apto ( AptoBE ASC ) ")
            '.AppendLine("Declare index ISAptoFR on @Apto ( AptoFR ASC ) ")
            '.AppendLine("Declare index ISAptoJT on @Apto ( AptoJT ASC ) ")
            '.AppendLine("Declare index ISAptoCEAtual on @Apto ( AptoCEAtual ASC ) ")
            '.AppendLine("Declare index ISAptoBEAtual on @Apto ( AptoBEAtual ASC ) ")
            '.AppendLine("--declare @Dia char(10) ")
            .AppendLine("declare @DiaDate datetime, ")
            .AppendLine("@LimiteIDadeBerco integer ")
            '.AppendLine("--set @Dia = convert(char(10), getdate(), 103) ")
            .AppendLine("set @DiaDate = convert(datetime, @Dia, 103) ")
            .AppendLine("select @LimiteIdadeBerco = LimiteIdadeBerco from TbParametro ")
            .AppendLine("declare Prioridade_cursor cursor for ")
            .AppendLine("select h.SolId, h.ApaId, min(h.HosDataIniSol) as HosDataIniSol, ")
            .AppendLine("count(h.HosId) ")
            .AppendLine("as Integrante, ")
            .AppendLine("sum(case when DbFuncao.Dbo.FuIdade(IntDtNascimento, IntDataIni) < @LimiteIdadeBerco then 1 else 0 end) SolBerco ")
            .AppendLine("from TbHospedagem h with (nolock) inner join TbSolicitacao s with (nolock) ")
            .AppendLine("on h.SolId = s.SolId ")
            .AppendLine("inner join TbIntegrante i with (nolock) on i.IntId = h.IntId ")

            .AppendLine("  where (((convert(datetime,convert(char(11), @DiaDate)) ")
            .AppendLine("  between convert(datetime, convert(char(11), h.HosDataIniSol))")
            .AppendLine("  and convert(datetime, convert(char(11), h.HosDataFimSol))))) ")
            .AppendLine("  and convert(datetime,CONVERT(char(11), h.HosDataFimSol)) > CONVERT(datetime, CONVERT(CHAR(11), getdate())) ")
            .AppendLine("  and h.HosDataFimReal is null ")

            'Consulta antiga, estava retornando os check out do dia sem check in previsto.
            '.AppendLine("where @DiaDate between dateadd(hh, - datepart(hh, h.HosDataIniSol), ")
            '.AppendLine("h.HosDataIniSol) and dateadd(hh, - datepart(hh, h.HosDataFimSol), h.HosDataFimSol) ")

            .AppendLine("group by h.SolId, h.ApaId ")
            '.AppendLine("order by h.HosDataIniSol, h.ApaId desc, ")
            .AppendLine("order by HosDataIniSol, ApaId desc, ")
            .AppendLine("Integrante desc ")

            .AppendLine("OPTION (OPTIMIZE FOR(@DiaDate='2012-01-01')) ")

            '.AppendLine("-- ")
            .AppendLine("open Prioridade_cursor ")
            '.AppendLine("-- ")
            .AppendLine("fetch next from Prioridade_cursor ")
            .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            .AppendLine("while @@fetch_status = 0 ")
            .AppendLine("begin ")
            .AppendLine("if @ApaId is null ")
            .AppendLine("begin ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--      and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--      and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '1' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '2' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT =  ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '3' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is null) ")
            .AppendLine("select top 1 @ApaId = a.ApaId, @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            '.AppendLine("--        and a.ApaBerco = @SolBerco ")
            .AppendLine("and d.Status = 'D' ")
            '.AppendLine("--        and a.Integrante = @Integrante ")
            .AppendLine("and a.Ordem = '4' ")
            .AppendLine("where not exists (select 1 from @Apto p where p.ApaId = d.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("select top 1 @ApaDesc = a.ApaDesc, @ApaStatus = a.Ordem, @CC = a.AcmCC, ")
            .AppendLine("@CS = a.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@CE = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante - a.AcmCC * 2 - a.AcmCS ")
            .AppendLine("else 0 ")
            .AppendLine("end, ")
            .AppendLine("@BE = @SolBerco, ")
            .AppendLine("@JT = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@FR = ")
            .AppendLine("case ")
            .AppendLine("when @Integrante > a.AcmCC * 2 + a.AcmCS then @Integrante + @SolBerco ")
            .AppendLine("else a.AcmCC * 2 + a.AcmCS + @SolBerco - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end ")
            .AppendLine("end, ")
            .AppendLine("@ApaCamaExtra = ApaCamaExtra, @ApaBerco = ApaBerco, @ApaDesc = a.ApaDesc ")
            .AppendLine("from VwDisponibilidadeAtualSemLock d with (nolock) inner join VwListaOrdemPrioridade a with (nolock) ")
            .AppendLine("on d.ApaId = a.ApaId ")
            .AppendLine("and d.SolId = @SolId ")
            .AppendLine("and d.ApaId = @ApaId ")
            .AppendLine("and d.Status = 'A' ")
            '.AppendLine("where (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("order by a.Ordem ")
            '.AppendLine("--  if (@ApaStatus not in ('2','4')) or ((@ApaCamaExtra <> @CE) or (@ApaBerco <> @BE)) ")
            .AppendLine("begin ")
            .AppendLine("set @ApaDesc = (select ApaDesc from TbApartamento where ApaId = @ApaId) ")
            'Washington
            .AppendLine("set @AlaId = (select AlaId from TbApartamento where ApaId = @ApaId) ")
            .AppendLine("if @ApaStatus in ('1','3') ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaCamaExtra <> @CE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaCamaExtra > @CE ")
            .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            .AppendLine("else ")
            .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("set @ApaCamaExtra = 0 ")
            .AppendLine("set @CE = 0 ")
            .AppendLine("end ")
            .AppendLine("if (@ApaBerco <> @BE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaBerco > @BE ")
            .AppendLine("set @BE = @BE - @ApaBerco ")
            .AppendLine("else ")
            .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("set @ApaBerco = 0 ")
            .AppendLine("set @BE = 0 ")
            .AppendLine("end ")
            .AppendLine("set @FR = @CE + @BE ")
            .AppendLine("set @JT = @CE + @BE ")
            .AppendLine("set @CC = 0 ")
            .AppendLine("set @CS = 0 ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaCamaExtra <> @CE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaCamaExtra > @CE ")
            .AppendLine("set @CE = @CE - @ApaCamaExtra ")
            .AppendLine("else ")
            .AppendLine("set @CE = abs(@CE - @ApaCamaExtra) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("set @CE = 0 ")
            .AppendLine("if (@ApaBerco <> @BE) ")
            .AppendLine("begin ")
            .AppendLine("if @ApaBerco > @BE ")
            .AppendLine("set @BE = @BE - @ApaBerco ")
            .AppendLine("else ")
            .AppendLine("set @BE = abs(@BE - @ApaBerco) ")
            .AppendLine("end ")
            .AppendLine("else ")
            .AppendLine("set @BE = 0 ")
            .AppendLine("end ")
            '.AppendLine("--")
            .AppendLine("if (@ApaId is not null)  ")
            '.AppendLine("and ((select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in  ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and c.ApaId = @ApaId) = @Qtde) ")
            .AppendLine("begin ")
            .AppendLine("if (@ApaStatus = 3) and not exists (select 1 from TbHospedagem with (nolock) where SolId = @SolId and HosDataIniReal is not null) ")
            .AppendLine("set @ApaStatus = 5 ")
            'Washington
            .AppendLine("insert @Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT, AptoStatus, AptoCEAtual, AptoBEAtual, Hora) ")
            .AppendLine("values (@ApaId, @AlaId, @ApaDesc, @CC, @CS, @CE, @BE, @FR, @JT, @ApaStatus, @ApaCamaExtra, @ApaBerco, datepart(hh, @DataIni)) ")
            .AppendLine("end ")
            .AppendLine("end ")
            .AppendLine("fetch next from Prioridade_cursor ")
            .AppendLine("into @SolId, @ApaId, @DataIni, @Integrante, @SolBerco ")
            .AppendLine("end ")
            '.AppendLine("--")
            .AppendLine("close Prioridade_cursor ")
            .AppendLine("deallocate Prioridade_cursor ")
            '.AppendLine("--")
            .AppendLine("update @Apto set ")
            .AppendLine("AptoFR = AptoFR - AptoCE - AptoBE, ")
            .AppendLine("AptoJT = AptoJT - AptoCE - AptoBE, ")
            .AppendLine("AptoCE = 0, ")
            .AppendLine("AptoBE = 0 ")
            '.AppendLine("--")
            .AppendLine("select a.* from @Apto a ")

            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId  ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")

            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If

            .AppendLine("where ((AptoCC > 0) or (AptoCS > 0) or (AptoCE > 0) ")
            .AppendLine("or (AptoBE > 0) or (AptoFR> 0) or (AptoJT> 0)) ")
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("and a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.apaid ")
            '.AppendLine("--    or (AptoCEAtual > 0) or (AptoBEAtual > 0)) -- or (AptoStatus = 5)) ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table #Caracteristica ")
        End With
        Dim ordersPrioridadeLinha As DataTablePrioridadeTableAdapter = New DataTablePrioridadeTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersPrioridadeLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelPrioridadeLinha As System.Data.DataTable = ordersPrioridadeLinha.GetData(VarPrioridade.ToString)
        Dim rdsRelPrioridadeLinha As New ReportDataSource("dtsRelGovernanca_DataTablePrioridade", ordersRelPrioridadeLinha)


        'REVISÃO'
        Dim VarRevisao As StringBuilder
        VarRevisao = New StringBuilder("")
        With VarRevisao
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            '.AppendLine("-- ")
            .AppendLine("set nocount on ")
            .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @RevisaoGovernancaHoras smallint ")
            '.AppendLine("-- ")
            '.AppendLine("Declare table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")
            .AppendLine("set @Cont = 1 ")
            '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
            '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
            '.AppendLine("set @Cont = @Aux + 1 ")
            '.AppendLine("end ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            '.AppendLine("-- ")
            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AlaId smallint, ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("Declare index ISApaId on @AptoPrioridade ( ApaId ASC ) ")
            '.AppendLine("-- ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ApaDesc varchar(10), AlaId smallint) ")
            '.AppendLine("-- ")
            '.AppendLine("Declare index ISApaId1 on @Apto ( ApaId ASC ) ")
            '.AppendLine("-- ")
            .AppendLine("select @RevisaoGovernancaHoras = RevisaoGovernancaHoras from TbParametro with (nolock) ")
            '.AppendLine("-- ")
            .AppendLine("insert @Apto (ApaId, ApaDesc, AlaId) ")
            .AppendLine("select a.ApaId, a.ApaDesc,a.AlaId from TbApartamento a with (nolock) ")
            .AppendLine("where a.ApaStatus = 'L' ")
            .AppendLine("and not exists ")
            .AppendLine("(select 1 from TbAtendimentoGov ag with (nolock) where ag.ApaId = a.ApaId ")
            .AppendLine("and AGoData between ")
            .AppendLine("dateadd(hh, -@RevisaoGovernancaHoras, dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
            .AppendLine("and dateadd(hh, datepart(hh, getdate()), convert(datetime, @Dia, 103))) ")
            .AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            .AppendLine("OPTION (OPTIMIZE FOR(@Dia='2012-01-01')) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            '.AppendLine("-- ")
            .AppendLine("select * from @Apto a ")
            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "'")
            End If

            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("where a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.ApaId ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With
        Dim ordersRevisaoLinha As DataTableRevisaoTableAdapter = New DataTableRevisaoTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersRevisaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersRevisaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelRevisaoLinha As System.Data.DataTable = ordersRevisaoLinha.GetData(VarRevisao.ToString)
        Dim rdsRelRevisaoLinha As New ReportDataSource("dtsRelGovernanca_DataTableRevisao", ordersRelRevisaoLinha)


        'ATENDIMENTO'
        Dim VarAtendimento As StringBuilder
        VarAtendimento = New StringBuilder("")
        With VarAtendimento
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco.Replace(".", "") & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            '.AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer ")
            '.AppendLine("Declare table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")
            '.AppendLine("set @Cont = 1 ")
            '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
            '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
            '.AppendLine("set @Cont = @Aux + 1 ")
            '.AppendLine("end ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("Declare index ISApaId on @AptoPrioridade ( ApaId ASC ) ")
            .AppendLine("Declare @Integrante table( ")
            .AppendLine("ApaId smallint, ")
            .AppendLine("Integrante smallint) ")
            '.AppendLine("Declare index ISApaId1 on @Integrante ( ApaId ASC ) ")
            .AppendLine("insert @Integrante (ApaId, Integrante) ")
            .AppendLine("select ApaId, Integrante from VwQuantidadeHospedeApartamento with (nolock) ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("NomeTitular varchar(200), ")
            .AppendLine("TotIntegrante smallint, ")
            .AppendLine("DataInicial DateTime, ")
            .AppendLine("DataFinal Datetime, ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint) ")
            '.AppendLine("Declare index ISApaId2 on @Apto ( ApaId ASC ) ")
            .AppendLine("declare @DiaDate datetime, @DiaAux datetime ")
            .AppendLine("set @DiaDate = convert(datetime, convert(varchar(10), convert(datetime, @Dia, 103), 103) + ' 23:59', 103) ")
            .AppendLine("set @DiaAux = convert(datetime, convert(varchar(10), convert(datetime, @Dia, 103), 103) + ' 00:00', 103) ")
            .AppendLine("set @DiaAux = dateadd(hh, datepart(hh, getdate()), @DiaAux) ")
            '.AppendLine("insert @Apto (ApaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("insert @Apto (ApaId, AlaId, ApaDesc,NomeTitular,TotIntegrante,DataInicial,DataFinal,AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("select ")
            .AppendLine("a.ApaId, a.AlaId, a.ApaDesc, ")
            'Pegando o nome de um integrante da reserva de maior idade
            .AppendLine("Upper((Select top 1 IntNome from tbintegrante II where II.ResId = Max(R.ResId) group by IntNome order by Max(IntDtNascimento))) as NomeTitular,  ")
            'Somando o total de integrantes de um apartamento
            .AppendLine("(SELECT COUNT(*) FROM TBINTEGRANTE IT ")
            .AppendLine("Inner join TbHospedagem MM on MM.intid = IT.inTId ")
            .AppendLine("WHERE IT.RESID = MAX(R.RESID) ")
            .AppendLine("AND MM.APAID = A.APAID) AS TotIntegrante, ")
            '
            .AppendLine("MAX(IntdataIniReal) as DataInicial, ")
            .AppendLine("MAX(INTDATAFIM)as DataFinal, ")
            .AppendLine("MAX(c.AcmCC) as AptoCC, ")
            .AppendLine("case ")
            .AppendLine("when (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - max(c.AcmCC) > Max(c.AcmCS) then max(c.AcmCS) ")
            .AppendLine("else (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - max(c.AcmCC) ")
            .AppendLine("end as AptoCS, ")
            .AppendLine("max(a.ApaCamaExtra) as AptoCE,MAX(a.ApaBerco) as AptoBE, ")
            .AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoFR, ")
            .AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoJT ")
            .AppendLine("from TbApartamento a with (nolock) ")
            .AppendLine("inner join TbAcomodacao c with (nolock) on a.AcmId = c.AcmId ")
            .AppendLine("inner join TbHospedagem H on A.ApaId = H.ApaId ")
            .AppendLine("inner join TbIntegrante I on I.IntId = H.IntId ")
            .AppendLine("inner join tbReserva R on R.ResId = I.ResId ")
            .AppendLine("where ApaStatus = 'O' ")
            .AppendLine("and I.IntStatus in ('E','P') ")
            .AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("  (select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("Group by a.ApaId,a.ApaDesc,a.AlaId  ")
            'Antiga Select ainda sem o Nome to titular da reserva + Data Ini e data Fim
            '.AppendLine("select a.ApaId, a.ApaDesc, c.AcmCC as AptoCC, ")
            '.AppendLine("case ")
            '.AppendLine("when (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - c.AcmCC > c.AcmCS then c.AcmCS ")
            '.AppendLine("else (select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) - c.AcmCC ")
            '.AppendLine("end as AptoCS, ")
            '.AppendLine("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
            '.AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoFR, ")
            '.AppendLine("(select top 1 i.Integrante from @Integrante i where i.ApaId = a.ApaId) as AptoJT ")
            '.AppendLine("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
            '.AppendLine("on a.AcmId = c.AcmId ")
            '.AppendLine("where ApaStatus = 'O' ")
            '.AppendLine("and not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("  (select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("select distinct a. *, ")
            .AppendLine("case ")
            .AppendLine("when  ")
            .AppendLine("exists (select top 1 1 from TbHospedagem hh with (nolock) ")
            .AppendLine("where hh.ApaId = a.ApaId ")
            .AppendLine("and hh.HosDataIniSol = h.HosDataFimSol ")
            .AppendLine("and hh.HosStatus = 'F') ")
            .AppendLine("then 'T' ")
            .AppendLine("when ")
            .AppendLine("not exists ")
            .AppendLine("(select top 1 1 from TbHospedagem with (nolock) where ApaId = a.ApaId ")
            .AppendLine("and HosDataIniReal is not null ")
            .AppendLine("and HosDataFimReal is null ")
            .AppendLine("and HosStatus = 'A' ")
            .AppendLine("and HosDataFimSol > @DiaDate) then 'S' ")
            .AppendLine("when ")
            .AppendLine("not exists (select top 1 1 from TbAtendimentoGov a with (nolock) ")
            .AppendLine("where h.ApaId = a.ApaId ")
            .AppendLine("and (abs(datediff(dd, a.AGoData, @DiaAux)) < 2) ")
            .AppendLine("order by a.AGoData desc) ")
            .AppendLine("then 'A' ")
            .AppendLine("else 'N' ")
            .AppendLine("end as CheckOut ")
            .AppendLine("from @Apto a ")
            .AppendLine("inner join TbHospedagem h ")
            .AppendLine("on a.ApaId = h.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = h.acmid ")
            .AppendLine("Where h.HosDataIniReal is not null ")
            .AppendLine("and h.HosDataFimReal is null ")
            .AppendLine("and h.HosStatus = 'A' ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco.Replace(".", "") & "' ")
            End If
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("and a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.ApaId ")
            .AppendLine("OPTION (OPTIMIZE FOR(@DiaDate='2012-01-01',@DiaAux ='2012-01-01')) ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @Integrante ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With
        Dim ordersAtendimetoLinha As DataTableAtendimentoTableAdapter = New DataTableAtendimentoTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersAtendimetoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersAtendimetoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelAtendimentoLinha As System.Data.DataTable = ordersAtendimetoLinha.GetData(VarAtendimento.ToString)
        Dim rdsRelAtendimentoLinha As New ReportDataSource("dtsRelGovernanca_DataTableAtendimento", ordersRelAtendimentoLinha)
        'Chamando o relatório'
        rptTodos.Visible = True
        rptArrumacao.Visible = False
        rptAtendimento.Visible = False
        rptPrioridade.Visible = False
        rptRevisao.Visible = False
        rptTodos.LocalReport.DataSources.Clear()
        rptTodos.LocalReport.DataSources.Add(rdsRelArrumacaoLinha)
        rptTodos.LocalReport.DataSources.Add(rdsRelPrioridadeLinha)
        rptTodos.LocalReport.DataSources.Add(rdsRelRevisaoLinha)
        rptTodos.LocalReport.DataSources.Add(rdsRelAtendimentoLinha)
        rptTodos.LocalReport.SetParameters(ParamsSugestoes)

    End Sub
    Public Sub Arrumacao(ByVal UnidadeOperacional As String)
        Dim Ala As Integer = drpAla.SelectedValue
        Dim Bloco As String = ""
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = "0"     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Dim Andar As String = ""
        'Select Case drpAndar.SelectedValue
        '    Case 0 : Andar = ""
        '    Case 5 : Andar = "5."   '1º andar
        '    Case 6 : Andar = "6."   '2º andar
        '    Case 11 : Andar = "11." 'Adaptado para pessoas especiais
        '    Case 28 : Andar = "28." '3º andar
        '    Case 29 : Andar = "29." '4º andar
        '    Case 30 : Andar = "30." '5º andar
        '    Case 31 : Andar = "31." '6º andar
        '    Case 32 : Andar = "32." '7º andar
        'End Select

        ObjRelGovernancaVO = New RelGovernacaVO
        ObjRelGovernancaDAO = New RelGovernacaDAO()
        'Montando Select Apartamento em ARRUMAÇÃO'
        rptArrumacao.Visible = True
        Dim VarArrumacao As StringBuilder
        VarArrumacao = New StringBuilder("SET NOCOUNT ON ")
        With VarArrumacao
            .AppendLine("declare ")
            .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
            .AppendLine("set @ListaCaract = '" & Bloco & "' ")
            .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
            .AppendLine("set nocount on ")
            .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @AcmBicama char(1), @AcmSofacama char(1) ")
            .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
            .AppendLine("from TbParametro with (nolock) ")
            '.AppendLine("create table #Caracteristica( ")
            '.AppendLine("CId numeric primary Key) ")
            .AppendLine("set @Cont = 1 ")
            '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
            '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
            '.AppendLine("insert #Caracteristica(CId) ")
            '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
            '.AppendLine("set @Cont = @Aux + 1 ")
            '.AppendLine("end ")
            '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
            '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
            '.AppendLine("on cc.CId = c.CarId ")
            .AppendLine("Declare @AptoPrioridade table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")

            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoStatus char(1), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoCEAtual smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoBEAtual smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint, ")
            .AppendLine("Hora smallint) ")
            '.AppendLine("create index ISApaId on @AptoPrioridade ( ApaId ASC ) ")
            .AppendLine("Declare @Apto table( ")
            .AppendLine("ApaId smallint, ")
            'Washington
            .AppendLine("AlaId smallint, ")
            .AppendLine("ApaDesc varchar(10), ")
            .AppendLine("AptoCC smallint, ")
            .AppendLine("AptoCS smallint, ")
            .AppendLine("AptoCE smallint, ")
            .AppendLine("AptoBE smallint, ")
            .AppendLine("AptoFR smallint, ")
            .AppendLine("AptoJT smallint) ")
            '.AppendLine("create index ISApaId1 on @Apto ( ApaId ASC ) ")
            .AppendLine("insert @Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
            .AppendLine("select a.ApaId,a.AlaId, a.ApaDesc, c.AcmCC as AptoCC, ")
            .AppendLine("c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoCS, ")
            .AppendLine("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
            .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoFR, ")
            .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
            .AppendLine("case ")
            .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
            .AppendLine("else 0 ")
            .AppendLine("end - ")
            .AppendLine("case ")
            .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
            .AppendLine("else 0 ")
            .AppendLine("end as AptoJT ")
            .AppendLine("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
            .AppendLine("on a.AcmId = c.AcmId and ApaStatus = 'A' ")
            .AppendLine("where not exists (select 1 from @AptoPrioridade p where p.ApaId = a.ApaId) ")
            '.AppendLine("and (case ")
            '.AppendLine("when @Qtde > 0 then ")
            '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
            '.AppendLine("(select CId from #Caracteristica) ")
            '.AppendLine("and a.ApaId = c.ApaId) ")
            '.AppendLine("else 0 end) = @Qtde ")
            .AppendLine("select * from @Apto a ")
            .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
            .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
            If Bloco > 0 Then
                .AppendLine("and ac.BloId = '" & Bloco & "' ")
            End If
            'FILTRANDO POR ALA
            If Ala <> 0 Then
                .AppendLine("Where a.AlaId = " & Ala & " ")
            End If
            .AppendLine("order by a.ApaId ")
            '.AppendLine("drop table @Apto ")
            '.AppendLine("drop table @AptoPrioridade ")
            '.AppendLine("drop table #Caracteristica ")
        End With






        'With VarArrumacao
        '    .AppendLine("declare ")
        '    .AppendLine("@ListaCaract varchar(1000), @Dia char(10) ")
        '    .AppendLine("set @ListaCaract = '" & Bloco & "' ")
        '    .AppendLine("set @Dia = '" & Format(CDate(txtData.Text), "dd/MM/yyyy") & "'")
        '    .AppendLine("set nocount on ")
        '    .AppendLine("declare @Qtde smallint, @Cont integer, @Aux integer, @AcmBicama char(1), @AcmSofacama char(1) ")
        '    .AppendLine("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
        '    .AppendLine("from TbParametro with (nolock) ")
        '    '.AppendLine("create table #Caracteristica( ")
        '    '.AppendLine("CId numeric primary Key) ")
        '    .AppendLine("set @Cont = 1 ")
        '    '.AppendLine("while Len(@ListaCaract) > @Cont begin ")
        '    '.AppendLine("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
        '    '.AppendLine("insert #Caracteristica(CId) ")
        '    '.AppendLine("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
        '    '.AppendLine("set @Cont = @Aux + 1 ")
        '    '.AppendLine("end ")
        '    '.AppendLine("select @Qtde = count(distinct CarGrupo) from ")
        '    '.AppendLine("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
        '    '.AppendLine("on cc.CId = c.CarId ")
        '    .AppendLine("create table #AptoPrioridade( ")
        '    .AppendLine("ApaId smallint, ")
        '    'Washington
        '    .AppendLine("AlaId smallint, ")

        '    .AppendLine("ApaDesc varchar(10), ")
        '    .AppendLine("AptoStatus char(1), ")
        '    .AppendLine("AptoCC smallint, ")
        '    .AppendLine("AptoCS smallint, ")
        '    .AppendLine("AptoCE smallint, ")
        '    .AppendLine("AptoCEAtual smallint, ")
        '    .AppendLine("AptoBE smallint, ")
        '    .AppendLine("AptoBEAtual smallint, ")
        '    .AppendLine("AptoFR smallint, ")
        '    .AppendLine("AptoJT smallint, ")
        '    .AppendLine("Hora smallint) ")
        '    .AppendLine("create index ISApaId on #AptoPrioridade ( ApaId ASC ) ")
        '    .AppendLine("create table #Apto( ")
        '    .AppendLine("ApaId smallint, ")
        '    'Washington
        '    .AppendLine("AlaId smallint, ")
        '    .AppendLine("ApaDesc varchar(10), ")
        '    .AppendLine("AptoCC smallint, ")
        '    .AppendLine("AptoCS smallint, ")
        '    .AppendLine("AptoCE smallint, ")
        '    .AppendLine("AptoBE smallint, ")
        '    .AppendLine("AptoFR smallint, ")
        '    .AppendLine("AptoJT smallint) ")
        '    .AppendLine("create index ISApaId1 on #Apto ( ApaId ASC ) ")
        '    .AppendLine("insert #Apto (ApaId,AlaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
        '    .AppendLine("select a.ApaId,a.AlaId, a.ApaDesc, c.AcmCC as AptoCC, ")
        '    .AppendLine("c.AcmCS - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end as AptoCS, ")
        '    .AppendLine("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
        '    .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end as AptoFR, ")
        '    .AppendLine("c.AcmCC * 2 + c.AcmCS - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmBicama = 'N' then AcmBicama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end - ")
        '    .AppendLine("case ")
        '    .AppendLine("when @AcmSofacama = 'N' then AcmSofacama ")
        '    .AppendLine("else 0 ")
        '    .AppendLine("end as AptoJT ")
        '    .AppendLine("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
        '    .AppendLine("on a.AcmId = c.AcmId and ApaStatus = 'A' ")
        '    .AppendLine("where not exists (select 1 from #AptoPrioridade p where p.ApaId = a.ApaId) ")
        '    '.AppendLine("and (case ")
        '    '.AppendLine("when @Qtde > 0 then ")
        '    '.AppendLine("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
        '    '.AppendLine("(select CId from #Caracteristica) ")
        '    '.AppendLine("and a.ApaId = c.ApaId) ")
        '    '.AppendLine("else 0 end) = @Qtde ")
        '    .AppendLine("select * from #Apto a ")
        '    .AppendLine("inner join TbApartamento aa on aa.ApaId = a.ApaId ")
        '    .AppendLine("inner join TbAcomodacao ac on ac.AcmId = aa.acmid ")
        '    If Bloco > 0 Then
        '        .AppendLine("and ac.BloId = '" & Bloco & "' ")
        '    End If
        '    'FILTRANDO POR ALA
        '    If Ala <> 0 Then
        '        .AppendLine("Where a.AlaId = " & Ala & " ")
        '    End If
        '    .AppendLine("order by a.ApaId ")
        '    .AppendLine("drop table #Apto ")
        '    .AppendLine("drop table #AptoPrioridade ")
        '    .AppendLine("drop table #Caracteristica ")
        'End With
        Dim ordersArrumacaoLinha As DataTableArrumacaoAdapter = New DataTableArrumacaoAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersArrumacaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersArrumacaoLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        'Dim ordersRelArrumacaoLinha As dtsRelGovernanca.DataTableArrumacaoPiriDataTable = ordersArrumacaoLinha.GetData(VarArrumacao.ToString)
        Dim ordersRelArrumacaoLinha As System.Data.DataTable = ordersArrumacaoLinha.GetData(VarArrumacao.ToString)
        Dim rdsRelArrumacaoLinha As New ReportDataSource("dtsRelGovernanca_DataTableArrumacao", ordersRelArrumacaoLinha)

        'Passando os parametros'                          
        Dim ParamsSugestoes(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsSugestoes(0) = New ReportParameter("Dia", txtData.Text)
        ParamsSugestoes(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsSugestoes(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsSugestoes(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsSugestoes(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)
        'Chamando o relatório'
        rptArrumacao.Visible = True
        rptTodos.Visible = False
        rptAtendimento.Visible = False
        rptPrioridade.Visible = False
        rptRevisao.Visible = False
        rptArrumacao.LocalReport.DataSources.Clear()
        rptArrumacao.LocalReport.DataSources.Add(rdsRelArrumacaoLinha)
        rptArrumacao.LocalReport.SetParameters(ParamsSugestoes)
    End Sub

    Public Sub TotalPax(UnidadeOperacional As String)
        Dim ala As Long = drpAla.SelectedValue
        Dim Bloco As String = ""
        Select Case drpBloco.SelectedValue
            Case 0 : Bloco = ""     'Todos
            Case 1 : Bloco = "1"   'Anhanguera
            Case 2 : Bloco = "2"   'Bambuí
            Case 3 : Bloco = "3"   'Oswaldo Kilzer
            Case 33 : Bloco = "4" 'Wilton Honorato
        End Select
        'Passando os parametros'
        Dim ParamsTotPax(4) As ReportParameter
        'Dim params(3) As ReportParameter
        ParamsTotPax(0) = New ReportParameter("Dia", Format(CDate(txtData.Text), "dd/MM/yyyy"))
        ParamsTotPax(1) = New ReportParameter("Bloco", drpBloco.SelectedItem.ToString)
        ParamsTotPax(2) = New ReportParameter("Andar", drpAla.SelectedItem.ToString)
        ParamsTotPax(3) = New ReportParameter("Usuario", User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        ParamsTotPax(4) = New ReportParameter("Camareira", drpCamareira.SelectedItem.ToString)
        Dim VarTotalPax As StringBuilder
        VarTotalPax = New StringBuilder("")
        With VarTotalPax
            .AppendLine("SET NOCOUNT ON ")
            .AppendLine("select m.BloId,a.ApaDesc,COUNT(a.ApaId) as TotIntegrantes ")
            .AppendLine("from tbApartamento a ")
            .AppendLine("inner join TbHospedagem h on h.ApaId = a.ApaId ")
            .AppendLine("inner join TbIntegrante i on i.IntId = h.IntId ")
            .AppendLine("inner join TbReserva r on r.ResId = i.ResId ")
            .AppendLine("inner join TbAcomodacao m on m.AcmId = h.AcmId ")
            .AppendLine("where i.IntDataFimReal is null ")
            .AppendLine("and i.IntStatus = 'E' ")
            .AppendLine("and r.ResStatus in ('E') ")
            .AppendLine("and h.HosStatus = 'A' ")
            .AppendLine("and CONVERT(char(10),h.HosDataFimSol,120) = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & "'")
            If Bloco <> "" Then
                .AppendLine("and BloId = " & Bloco & " ")
            End If
            'If drpAla.SelectedIndex > 0 Then
            '    .AppendLine("and a.AlaId = '" & drpAla.SelectedValue & "' ")
            'End If
            .AppendLine("group by m.BloId,a.ApaDesc ")
            .AppendLine("order by m.BloId,a.Apadesc ")
        End With
        Dim ordersTotPaxLinha As DataTableTotPaxTableAdapter = New DataTableTotPaxTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnUnidade.Attributes.Item("UOP").ToString = "Caldas Novas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersTotPaxLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersTotPaxLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelTotPaxLinha As System.Data.DataTable = ordersTotPaxLinha.GetData(VarTotalPax.ToString)
        Dim rdsRelTotPaxLinha As New ReportDataSource("DtsTotPax", ordersRelTotPaxLinha)
        If ordersTotPaxLinha.GetData(VarTotalPax.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            rptTotalPax.Visible = True
            rptRevisao.Visible = False
            rptAtendimento.Visible = False
            rptPrioridade.Visible = False
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            rptTotalPax.LocalReport.DataSources.Clear()
            rptTotalPax.LocalReport.DataSources.Add(rdsRelTotPaxLinha)
            rptTotalPax.LocalReport.SetParameters(ParamsTotPax)
        Else
            rptTotalPax.Visible = False
            rptRevisao.Visible = False
            rptAtendimento.Visible = False
            rptPrioridade.Visible = False
            rptArrumacao.Visible = False
            rptTodos.Visible = False
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("Governanca.aspx")
    End Sub

    Protected Sub drpBloco_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpBloco.SelectedIndexChanged
        drpCamareira.Enabled = False
        drpAla.Items.Clear()
        ObjAlaVO = New AlaVO
        ObjAlaDAO = New AlaDAO()
        drpAla.DataValueField = "AlaId"
        drpAla.DataTextField = "AlaNome"
        ObjAlaVO.AlaNome = Mid(drpBloco.SelectedItem.ToString, 1, 1) & Mid(drpBloco.SelectedItem.ToString, 5, 1)
        drpAla.DataSource = ObjAlaDAO.ConsultaAla(ObjAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpAla.DataBind()
        drpAla.Items.Insert(0, New ListItem("Todas", "0"))
        drpAla.SelectedValue = 0


    End Sub

    Protected Sub drpAla_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAla.SelectedIndexChanged
        If drpAla.SelectedValue <> 0 Then
            drpCamareira.Enabled = True
        Else
            drpCamareira.Enabled = False
        End If
        'Indo na base de dados e buscando qual a camareira é reponsavel por essa ala.'
        If drpAla.SelectedValue <> 0 Then
            ObjAlaVO = New AlaVO
            ObjAlaVO.AlaId = drpAla.SelectedValue
            ObjAlaDAO = New AlaDAO()
            ObjAlaVO = ObjAlaDAO.ConsultaAlaCodigo(ObjAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            'Setando a camareira responsável pela ala
            drpCamareira.SelectedValue = ObjAlaVO.CamId
            'Estou gravando o CamId para Fazer um comparativo no drpCamareira
            btnCadCamareira.Attributes.Add("CamId", ObjAlaVO.CamId)
        Else
            drpCamareira.SelectedValue = 0
        End If
    End Sub

    Protected Sub btnSalvarCad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvarCad.Click
        'Cadastrando uma camareira'
        ObjCamareiraVO = New CamareiraVO
        'Carregando o Objeto'
        ObjCamareiraVO.CamId = -1 'Forçar sempre a inserção'
        ObjCamareiraVO.CamNome = Trim(txtNomeCad.Text)
        ObjCamareiraVO.CamSituacao = "A" 'Ativa'
        ObjCamareiraDAO = New CamareiraDAO()
        'Inserindo
        Select Case ObjCamareiraDAO.Inserir(ObjCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Salvo com sucesso!');", True)
            Case 2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atualizado com sucesso!');", True)
            Case 3
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro!Informe ao centro de informática!');", True)
                hddProcessando.Value = ""
                Return
        End Select
        hddProcessando.Value = ""
        drpCamareira.Items.Clear()
        'CARREGANDO LISTA DE CAMAREIRAS PAINEL DE ATENDIMENTO'
        ObjCamareiraVO = New CamareiraVO
        ObjCamareiraDAO = New CamareiraDAO()
        drpCamareira.DataValueField = "CamId"
        drpCamareira.DataTextField = "CamNome"
        drpCamareira.DataSource = ObjCamareiraDAO.ConsultarCamareira(ObjCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpCamareira.DataBind()
        drpCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
    End Sub

    Protected Sub drpCamareira_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCamareira.SelectedIndexChanged
        'Se mudarem a camareira, irá fazer a alteração diretamente no cadastro de alas idenficando a nova camareira
        If drpCamareira.SelectedValue <> CLng(btnCadCamareira.Attributes.Item("CamId").ToString) Then
            ObjAlaVO = New AlaVO
            ObjAlaVO.AlaId = drpAla.SelectedValue
            ObjAlaVO.CamId = drpCamareira.SelectedValue
            ObjAlaDAO = New AlaDAO()
            'Atualizando a camareira na ala'
            Select Case ObjAlaDAO.AtualizaCamareiraAla(ObjAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                Case 3
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro!Informe ao centro de informática!');", True)
                    Return
            End Select
        End If
    End Sub

    Protected Sub btnCadCamareira_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCadCamareira.Click
        txtNomeCad.Text = ""
    End Sub

    Protected Sub btnEscondeRel_Click(sender As Object, e As EventArgs) Handles btnEscondeRel.Click
        'Escondendo os relatório da tela
        rptArrumacao.Visible = False
        rptAtendimento.Visible = False
        rptPrioridade.Visible = False
        rptRevisao.Visible = False
        rptTodos.Visible = False
        rptTotalPax.Visible = False
    End Sub
End Class
