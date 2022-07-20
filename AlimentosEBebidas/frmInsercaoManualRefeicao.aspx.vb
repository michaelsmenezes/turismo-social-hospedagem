
Partial Class frmInsercaoManualRefeicao
    Inherits System.Web.UI.Page
    Dim ObjRefeicaoManualVO As RefeicaoManualVO
    Dim ObjRefeicaoManualDAO As RefeicaoManualDAO
    Dim ContaRefeicao As Long = 0
    Dim ContaRefeicaoGeral As Long = 0

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        ObjRefeicaoManualVO = New RefeicaoManualVO
        ObjRefeicaoManualVO.RefTipo = drpRefeicao.SelectedValue
        ObjRefeicaoManualDAO = New RefeicaoManualDAO()
        'Previsão de refeições
        PnlInserir.Visible = False
        gdvRefeicaoManual.Visible = True
        gdvRefeicaoManual.DataSource = ObjRefeicaoManualDAO.ConsultaRefeicoes(ObjRefeicaoManualVO, Format(CDate(txtDataInicial.Text), "yyyy-MM-dd") & " 00:00:00", Format(CDate(txtDataFinal.Text), "yyyy-MM-dd") & " 23:59:59", btnConsultar.Attributes.Item("AliasBancoTurismo"), drpInsercao.SelectedValue)
        gdvRefeicaoManual.DataBind()

        If drpInsercao.SelectedValue = "M" Then
            gdvRefeicaoManual.Columns(4).Visible = True
        Else
            gdvRefeicaoManual.Columns(4).Visible = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnConsultar.Attributes.Add("BancoTurismoSocial", "dbTurismoSocial")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnConsultar.Attributes.Add("BancoTurismoSocial", "dbTurismoSocialPiri")
            End Select
            'Definindo cor das bordas'
            'pnlPesquisa_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlPesquisa_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlGridRefeicao_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlGridRefeicao_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'PnlInserir_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'PnlInserir_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            ''SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            'If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
            If Not (Grupos.Contains("Turismo Social Alimentacao Insercao Manual de Refeicoes") Or Grupos.Contains("Turismo Social Piri Alimentacao Insercao Manual de Refeicoes")) Then
                Response.Redirect("..\AcessoNegado.aspx")
                Return
            End If

            txtDataInicial.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataInicial.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataInicial.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtDataFinal.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataFinal.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataFinal.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtQuantidade.Attributes.Add("OnKeyPress", "javascript:SomenteNumeros(this.value);")
            txtDataRefeicao.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataRefeicao.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtDataRefeicao.Attributes.Add("OnKeyPress", "javascript:SomenteNumeros(this.value);")
            drpInsercao.SelectedValue = "M"
            btnConsultar_Click(sender, e)
        End If
    End Sub

    'Protected Sub gdvPratosRapidos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPratosRapidos.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        If e.Row.Cells(0).Text = "TOTAL GERAL" Then
    '            e.Row.ForeColor = Drawing.Color.Black
    '            e.Row.Font.Bold = True
    '        End If
    '    End If
    'End Sub

    Protected Sub gdvRefeicaoManual_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvRefeicaoManual.RowDataBound
        
        If e.Row.RowType = DataControlRowType.DataRow Then
            ContaRefeicao = ContaRefeicao + gdvRefeicaoManual.DataKeys(e.Row.RowIndex).Item(4).ToString
            CType(e.Row.FindControl("imgApagar"), ImageButton).CommandArgument = gdvRefeicaoManual.DataKeys(e.Row.RowIndex).Item(3).ToString
            lblTotRefeicao.Text = Format((ContaRefeicao), "###,###,#00")
        End If

        'If e.Row.RowType = DataControlRowType.Footer Then
        '    'lblTotRefeicao.Text = Format((ContaRefeicao), "###,###,#00")
        '    'lblTotRefeicao.ForeColor = Drawing.Color.Black
        '    'lblTotRefeicao.Font.Bold = True
        'Else
        '    'lblTotRefeicao.Text = 0
        '    'lblTotRefeicao.ForeColor = Drawing.Color.Black
        '    'lblTotRefeicao.Font.Bold = True
        'End If

    End Sub

    Protected Sub btnInserir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInserir.Click
        PnlInserir.Visible = True
        gdvRefeicaoManual.Visible = False
        txtDataRefeicao.Text = Format(Date.Today, "dd/MM/yyyy")
        txtDataRefeicao.Focus()
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        PnlInserir.Visible = False
    End Sub

    Protected Sub btnGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGravar.Click
        If txtDataFinal.Text.Length = 0 Or txtQuantidade.Text.Length = 0 Or txtMotivo.Text.Length = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Favor preencher todos os campos para inserir!');", True)
            txtQuantidade.Focus()
            Return
        End If

        ObjRefeicaoManualVO = New RefeicaoManualVO
        'Carregando objeto
        With ObjRefeicaoManualVO
            .IntId = -2
            .RefCortesia = "N"
            .RefData = txtDataRefeicao.Text
            .RefQtde = txtQuantidade.Text
            .RefTipo = drpTipoRefeicao.SelectedValue
            .RefMotivoInsManual = txtMotivo.Text
            .RefUsuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            .RefUsuarioData = Date.Now
        End With
        ObjRefeicaoManualDAO = New RefeicaoManualDAO()
        'Inserindo no banco
        Select Case ObjRefeicaoManualDAO.InserindoRefeicoes(ObjRefeicaoManualVO, btnConsultar.Attributes.Item("AliasBancoTurismo"))
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Inserido com sucesso!');", True)
                lblTotRefeicao.Text = ""
                PnlInserir.Visible = False
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao inserir registro.Informe o centro de informática.');", True)
                hddProcessando.Value = ""
                Return
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub imgApagar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ObjRefeicaoManualVO = New RefeicaoManualVO
        ObjRefeicaoManualVO.RefId = CLng(sender.commandargument.ToString)
        ObjRefeicaoManualDAO = New RefeicaoManualDAO()
        Select Case ObjRefeicaoManualDAO.ApagaRefeicao(ObjRefeicaoManualVO, btnConsultar.Attributes.Item("AliasBancoTurismo"))
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                btnConsultar_Click(sender, e)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao apagar.Informe o centro de informática.');", True)
                hddProcessando.Value = ""
                Return
        End Select
        'btnGravar.Attributes.Add("entId", CLng(sender.commandargument.ToString))
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnVoltarPrincipal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarPrincipal.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub

    Protected Sub gdvRefeicaoManual_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdvRefeicaoManual.PageIndexChanging
        gdvRefeicaoManual.PageIndex = e.NewPageIndex
        btnConsultar_Click(sender, e)
    End Sub
End Class
