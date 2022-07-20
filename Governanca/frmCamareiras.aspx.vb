
Partial Class frmCamareiras
    Inherits System.Web.UI.Page
    Dim objCamareiraVO As CamareiraVO
    Dim objCamareiraDAO As CamareiraDAO
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
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("CNV_GOVERNANCA") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Governanca")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            pnlPesquisa_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlPesquisa_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            pnlCadastro_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            pnlCadastro_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
        End If
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmConfiguracoes.aspx")
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        'Buscando a relação de camareiras
        objCamareiraVO = New CamareiraVO
        objCamareiraVO.CamNome = txtNome.Text
        objCamareiraDAO = New CamareiraDAO()
        gdvCamareiras.DataSource = objCamareiraDAO.ConsultarCamareira(objCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvCamareiras.DataBind()
        pnlGridCamareira.Visible = True
    End Sub

    Protected Sub imgExcluir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        objCamareiraVO = New CamareiraVO
        objCamareiraVO.CamId = CLng(sender.CommandArgument.ToString)
        objCamareiraDAO = New CamareiraDAO()
        Select Case objCamareiraDAO.Exclui(objCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Excluído com sucesso!');", True)
            Case 3
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro! Informe o centro de informática');", True)
                hddProcessando.Value = ""
                Return
        End Select
        hddProcessando.Value = ""
        btnConsultar_Click(sender, e)
    End Sub

    Protected Sub gdvCamareiras_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvCamareiras.SelectedIndexChanged
        btnNovo.Attributes.Add("CamId", gdvCamareiras.DataKeys(gdvCamareiras.SelectedIndex).Item(0).ToString())
        txtNomeCad.Text = gdvCamareiras.DataKeys(gdvCamareiras.SelectedIndex).Item(1).ToString()
        pnlGridCamareira.Visible = False
        pnlCadastro.Visible = True
        txtNomeCad.Focus()
    End Sub

    Protected Sub gdvCamareiras_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCamareiras.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Adicionando o AlaId na imagem'
            CType(e.Row.FindControl("imgExcluir"), ImageButton).CommandArgument = gdvCamareiras.DataKeys(e.Row.RowIndex).Item(0).ToString
        End If
    End Sub

    Protected Sub btnSalvarCad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvarCad.Click
        'Cadastrando uma camareira'
        objCamareiraVO = New CamareiraVO
        'Carregando o Objeto'
        objCamareiraVO.CamId = CLng(btnNovo.Attributes.Item("CamId").ToString)   'CLng(sender.CommandArgument.ToString)
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
        pnlCadastro.Visible = False
        btnConsultar_Click(sender, e)
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnVoltarCad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarCad.Click
        pnlCadastro.Visible = False
        pnlGridCamareira.Visible = True
    End Sub

    Protected Sub btnNovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNovo.Click
        'Limpando o componente antes de iniciar
        txtNomeCad.Text = ""
        btnNovo.Attributes.Add("CamId", -1)
        'Controle de paineis
        pnlGridCamareira.Visible = False
        pnlCadastro.Visible = True
        txtNomeCad.Focus()
    End Sub
End Class
