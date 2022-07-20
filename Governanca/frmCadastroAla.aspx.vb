Partial Class frmCadastroAla
    Inherits System.Web.UI.Page
    Dim objAlaVO As AlaVO
    Dim objAlaDAO As AlaDAO
    Dim objCamareiraVO As CamareiraVO
    Dim objCamareiraDAO As CamareiraDAO
    Dim ObjApartamentosVO As ApartamentosVO
    Dim ObjApartamentosDAO As ApartamentosDAO
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
                    btnUnidade.Attributes.Add("AliasBancoTurismo",  "TurismoSocialPiri")
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
            ''Limpando os cookies
            'If Request.Cookies("BancoTurismoSocial") Is Nothing Then
            '    Response.Cookies("BancoTurismoSocial").Expires = DateTime.Now.AddDays(-9)
            'End If
            'If Request.Cookies("AliasBancoTurismo") Is Nothing Then
            '    Response.Cookies("AliasBancoTurismo").Expires = DateTime.Now.AddDays(-9)
            'End If
            'If Request.Cookies("AliasBancoHdManutencao") Is Nothing Then
            '    Response.Cookies("AliasBancoHdManutencao").Expires = DateTime.Now.AddDays(-9)
            'End If
            'If Request.Cookies("UOP") Is Nothing Then
            '    Response.Cookies("UOP").Expires = DateTime.Now.AddDays(-10)
            'End If

            ''Definindo o alias para o banco de dados
            'If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            '    'Direcionando a aplicação banco o banco DbTurismoSocial
            '    'Response.Cookies("AliasBancoTurismo").Value = "TurismoSocialTst"
            '    Response.Cookies.Add(New HttpCookie("AliasBancoTurismo", "TurismoSocialTst"))
            '    'Direcionando a aplicação para o banco hdmanutenção de Caldas Novas
            '    Session("AliasBancoHdManutencao").ToString = "HDManutencao"dbTurismoSocialPiri"
            '    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
            '    Response.Cookies("BancoTurismoSocial").Value = "dbTurismoSocial"
            '    'Usado no pagina de relatórios
            '    Response.Cookies("UOP").Value = "Caldas Novas"
            'Else
            '    'Direcionando a aplicação banco o banco DbTurismoSocialPiri
            '    'Response.Cookies("AliasBancoTurismo").Value =  "TurismoSocialPiri"
            '    Response.Cookies.Add(New HttpCookie("AliasBancoTurismo",  "TurismoSocialPiri"))
            '    'Direcionando a aplicação para o banco hdmanutenção de Pirenopolis
            '    Session("AliasBancoHdManutencao").ToString = "HdManutencaoTst"
            '    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
            '    Response.Cookies("BancoTurismoSocial").Value = "dbTurismoSocialPiri"
            '    'Usado no pagina de relatórios
            '    Response.Cookies("UOP").Value = "Pirenopolis"
            'End If
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("CNV_GOVERNANCA") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Governanca")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            pnlConsultaAla_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlConsultaAla_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            pnlCadastroAla_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlCadastroAla_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            pnlInsereAptoAla_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlInsereAptoAla_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlCadastroCam_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlCadastroCam_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'CARREGANDO LISTA DE CAMAREIRAS PAINEL DE ATENDIMENTO'
            objCamareiraVO = New CamareiraVO
            objCamareiraDAO = New CamareiraDAO()
            drpCadCamareira.DataValueField = "CamId"
            drpCadCamareira.DataTextField = "CamNome"
            drpCadCamareira.DataSource = objCamareiraDAO.ConsultarCamareira(objCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            drpCadCamareira.DataBind()
            drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            btnCamareira.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtNomeCad.value=''")
        End If
    End Sub

    Protected Sub btnPesquisarAla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPesquisarAla.Click
        pnlAptoAlas.Visible = False
        objAlaVO = New AlaVO
        objAlaVO.AlaNome = Trim(txtNomeAla.Text)
        objAlaDAO = New AlaDAO()
        pnlGridAlas.Visible = True
        GdvListaAla.DataSource = objAlaDAO.ConsultaAla(objAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        GdvListaAla.DataBind()
        pnlCadastroAla.Visible = False
    End Sub

    Protected Sub btnSair_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSair.Click
        pnlCadastroAla.Visible = False
        pnlGridAlas.Visible = True
    End Sub

    Protected Sub btnInserirAla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInserirAla.Click
        'Ativando apenas a inserção sempre'
        hddCodigo.Value = 0
        pnlAptoAlas.Visible = False
        'Limpando os campos antes de inserir'
        txtCadNome.Text = ""
        txtCadDescricao.Text = ""
        drpCadCamareira.SelectedValue = 0
        'Iniciando processo de inserção
        pnlGridAlas.Visible = False
        pnlCadastroAla.Visible = True
        txtCadNome.Focus()
    End Sub

    Protected Sub btnSalvarAla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvarAla.Click
        'Obriga a digitar as informações'
        If (txtCadNome.Text.Length = 0) Or (txtCadDescricao.Text.Length = 0) Or (drpCadCamareira.SelectedValue = 0) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('As informações sobre a ala são de preenchimento obrigatório!');", True)
            Return
        End If

        'Preenchendo o objeto antes de inserir'
        objAlaVO = New AlaVO
        objAlaDAO = New AlaDAO()
        With objAlaVO
            .AlaId = hddCodigo.Value
            .AlaNome = Replace(Trim(txtCadNome.Text), "'", "").ToUpper
            .AlaDescricao = Replace(Trim(txtCadDescricao.Text), "'", "").ToUpper
            .CamId = drpCadCamareira.SelectedValue
        End With
        Select Case objAlaDAO.Inserir(objAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao salvar! Informe o Centro de Informática!');", True)
            Case 1 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Salvo com sucesso!');", True)
            Case 2 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atualizado com sucesso!');", True)
            Case 4 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Ala já existente, inserção não autorizada!');", True)
        End Select
        pnlCadastroAla.Visible = False
    End Sub

    Protected Sub GdvListaAla_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GdvListaAla.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Adicionando o AlaId na imagem'
            CType(e.Row.FindControl("imgAptoAla"), ImageButton).CommandArgument = GdvListaAla.DataKeys(e.Row.RowIndex).Item(0).ToString
        End If
    End Sub
    Protected Sub GdvListaAla_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GdvListaAla.SelectedIndexChanged
        hddCodigo.Value = GdvListaAla.DataKeys(GdvListaAla.SelectedIndex).Item(0).ToString()
        'Carregando os dados da consulta na tela de alteração'
        txtCadNome.Text = GdvListaAla.DataKeys(GdvListaAla.SelectedIndex).Item(1).ToString()
        txtCadDescricao.Text = GdvListaAla.DataKeys(GdvListaAla.SelectedIndex).Item(2).ToString
        drpCadCamareira.SelectedValue = GdvListaAla.DataKeys(GdvListaAla.SelectedIndex).Item(3).ToString
        pnlGridAlas.Visible = False
        pnlCadastroAla.Visible = True
    End Sub

    Protected Sub imgAptoAla_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        pnlAptoAlas.Visible = True
        gdvAptosAla.Visible = False
        'Carregando os apartamentos que ainda não pertecem a ala dentro do combobox'
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosVO.AlaId = CLng(sender.CommandArgument.ToString)
        btnInserirAla.Attributes.Add("AlaId", CLng(sender.CommandArgument.ToString))
        'CONSULTAR O CADASTRO DE ALAS, CARREGAR O OBJETO E ALIMENTAR O TXTMOSTRAALA --> MUDAR PARA LABEL'
        objAlaVO = New AlaVO
        objAlaVO.AlaId = CLng(sender.CommandArgument.ToString)
        objAlaDAO = New AlaDAO()
        objAlaVO = objAlaDAO.ConsultaAlaCodigo(objAlaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        lblMostraAla.Text = objAlaVO.AlaNome
        'CARREGANDO APARTAMENTOS'
        ObjApartamentosDAO = New ApartamentosDAO()
        drpAptoAla.DataTextField = "ApaDesc"
        drpAptoAla.DataValueField = "ApaId"
        'PEGANDO APENAS A LETRA INICIAL DA ALA PARA APLICAR O FILTRO NO APARTAMENTO'
        ObjApartamentosVO.ApaDesc = Mid(lblMostraAla.Text, 1, 1)
        drpAptoAla.DataSource = ObjApartamentosDAO.ApartamentoAlas(ObjApartamentosVO, "N", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpAptoAla.DataBind()
        'BUSCANDO OS APARTAMENTOS REFERENTE A ALA E MOSTRANDO NA TELA
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosDAO = New ApartamentosDAO()
        ObjApartamentosVO.AlaId = CLng(btnInserirAla.Attributes.Item("AlaId").ToString)
        gdvAptosAla.DataSource = ObjApartamentosDAO.ApartamentoAlas(ObjApartamentosVO, "S", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvAptosAla.DataBind()
        pnlGridAlas.Visible = False
        gdvAptosAla.Visible = True
    End Sub

    Protected Sub btnSairAptoAla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSairAptoAla.Click
        pnlAptoAlas.Visible = False
        pnlGridAlas.Visible = True
    End Sub

    Protected Sub btnAptoAlaNovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAptoAlaNovo.Click
        'Carregando os apartamentos que ainda não pertecem a ala dentro do combobox'
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosDAO = New ApartamentosDAO()
        ObjApartamentosVO.AlaId = CLng(btnInserirAla.Attributes.Item("AlaId").ToString)
        'Dar um update no tbapartamento setando apenas o alaid'
        ObjApartamentosVO.ApaId = drpAptoAla.SelectedValue
        Select Case ObjApartamentosDAO.AtribuiAla(ObjApartamentosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 2 'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atribuido com sucesso!');", True)
            Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Ocorreu um erro ao atribuir, informe ao centro de informática!');", True)
        End Select
        'Fazendo a consulta novamente dos apartamentos restantes que não pertencem a esta ala
        drpAptoAla.DataTextField = "ApaDesc"
        drpAptoAla.DataValueField = "ApaId"
        'PEGANDO APENAS O VALOR INICIAL DA ALA PARA BUSCA NO BANCO
        ObjApartamentosVO.ApaDesc = Mid(lblMostraAla.Text, 1, 1)
        drpAptoAla.DataSource = ObjApartamentosDAO.ApartamentoAlas(ObjApartamentosVO, "N", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpAptoAla.DataBind()
        drpAptoAla.Items.Insert(0, New ListItem("Selecione...", "0"))
        'Fazer a consulta dos aptos que pertencem a essa ala e exibir'
        ObjApartamentosVO.AlaId = CLng(btnInserirAla.Attributes.Item("AlaId").ToString)
        gdvAptosAla.DataSource = ObjApartamentosDAO.ApartamentoAlas(ObjApartamentosVO, "S", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvAptosAla.DataBind()
        gdvAptosAla.Visible = True
        drpAptoAla.Focus()
    End Sub

    Protected Sub btnConsultaAptoAla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultaAptoAla.Click
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosDAO = New ApartamentosDAO()
        ObjApartamentosVO.AlaId = CLng(btnInserirAla.Attributes.Item("AlaId").ToString)
        gdvAptosAla.DataSource = ObjApartamentosDAO.ApartamentoAlas(ObjApartamentosVO, "S", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvAptosAla.DataBind()
        gdvAptosAla.Visible = True
    End Sub

    Protected Sub btnVoltarGov_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarGov.Click
        Response.Redirect("frmConfiguracoes.aspx")
    End Sub

    Protected Sub btnSalvarCad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvarCad.Click
        'Cadastrando uma camareira'
        objCamareiraVO = New CamareiraVO
        'Carregando o Objeto'
        objCamareiraVO.CamId = -1 'Forçar sempre a inserção'
        objCamareiraVO.CamNome = Trim(txtNomeCad.Text)
        objCamareiraVO.CamSituacao = "A" 'Ativa'
        objCamareiraDAO = New CamareiraDAO()
        'Inserindo
        Select Case objCamareiraDAO.Inserir(objCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
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
        drpCadCamareira.Items.Clear()
        'CARREGANDO LISTA DE CAMAREIRAS PAINEL DE ATENDIMENTO'
        objCamareiraVO = New CamareiraVO
        objCamareiraDAO = New CamareiraDAO()
        drpCadCamareira.DataValueField = "CamId"
        drpCadCamareira.DataTextField = "CamNome"
        drpCadCamareira.DataSource = objCamareiraDAO.ConsultarCamareira(objCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpCadCamareira.DataBind()
        drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
    End Sub

    Protected Sub btnCamareira_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCamareira.Click
        txtNomeCad.Text = ""
    End Sub
End Class
