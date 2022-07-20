
Partial Class frmMeiaDiariaSemPernoite
    Inherits System.Web.UI.Page
    Dim ObjRestaurantePrevisaoVO As RestaurantePrevisoesVO
    Dim ObjRestaurantePrevisaoDAO As RestaurantePrevisoesDAO
    Dim ObjPratosRapidosVO As PratosRapidosVO
    Dim ObjPratosRapidosDAO As PratosRapidosDAO
    Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
    Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
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
                    btnGravar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnGravar.Attributes.Add("BancoTurismoSocial", "dbTurismoSocial")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnGravar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnGravar.Attributes.Add("BancoTurismoSocial", "dbTurismoSocialPiri")
            End Select
           
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If

            ObjRestaurantePrevisaoVO = New RestaurantePrevisoesVO
            ObjRestaurantePrevisaoDAO = New RestaurantePrevisoesDAO()
            ObjPratosRapidosVO = New PratosRapidosVO
            ObjPratosRapidosDAO = New PratosRapidosDAO
            'Inserção de refeições
            gdvInsereMeiaDiaria.DataSource = ObjPratosRapidosDAO.ConsultaPratosParaInsercao(ObjPratosRapidosVO, Format((Date.Today), "yyyy-MM-dd"), btnGravar.Attributes.Item("AliasBancoTurismo"))
            gdvInsereMeiaDiaria.DataBind()
            'Previsão de Pratos Rápidos
            gdvPratosRapidos.DataSource = ObjPratosRapidosDAO.Consultar(ObjPratosRapidosVO, Format((Date.Today), "yyyy-MM-dd"), btnGravar.Attributes.Item("AliasBancoTurismo"))
            gdvPratosRapidos.DataBind()
            'Previsão de pratos rápidos no restaurante
            gdvPassantesRestaurante.DataSource = ObjPratosRapidosDAO.ConsultaPratosRestaurante(ObjPratosRapidosVO, Format((Date.Today), "yyyy-MM-dd"), btnGravar.Attributes.Item("AliasBancoTurismo"))
            gdvPassantesRestaurante.DataBind()
            'pnlConsulta_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlConsulta_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlAlmocoRst_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlAlmocoRst_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlOpcoesPratos_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlOpcoesPratos_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
        End If
    End Sub

    Protected Sub gdvPratosRapidos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPratosRapidos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(0).Text = "TOTAL GERAL" Then
                e.Row.ForeColor = Drawing.Color.Black
                e.Row.Font.Bold = True
            End If
        End If
    End Sub

    Protected Sub gdvInsereMeiaDiaria_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvInsereMeiaDiaria.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Não deixando digitar string no textbox
            CType(e.Row.FindControl("txtQuantidade"), TextBox).Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            'Pegando o código do prato
            CType(e.Row.FindControl("txtQuantidade"), TextBox).Attributes.Add("CodPrato", gdvInsereMeiaDiaria.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("txtQuantidade"), TextBox).Attributes.Add("Quantidade", CType(e.Row.FindControl("txtQuantidade"), TextBox).Text)
            'Inserindo a quantidade de refeições buscada no banco
            CType(e.Row.FindControl("txtQuantidade"), TextBox).Text = gdvInsereMeiaDiaria.DataKeys(e.Row.RowIndex).Item(2).ToString
        End If
    End Sub

    Protected Sub btnGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGravar.Click
        For Each linha As GridViewRow In gdvInsereMeiaDiaria.Rows
            'IRA Passar em todas as linhas do grid inserindo
            If CLng(CType(linha.FindControl("txtQuantidade"), TextBox).Text) >= 0 Then
                ObjPratosRapidosVO = New PratosRapidosVO
                ObjPratosRapidosVO.Confirmado = CLng(CType(linha.FindControl("txtQuantidade"), TextBox).Text)
                ObjPratosRapidosDAO = New PratosRapidosDAO
                Select Case ObjPratosRapidosDAO.InserePratosRapido(ObjPratosRapidosVO, CLng(CType(linha.FindControl("txtQuantidade"), TextBox).Attributes.Item("CodPrato")), btnGravar.Attributes.Item("AliasBancoTurismo"), User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
                    Case 0
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao inserir o prato rápido, informe o Centro de Informática.');", True)
                        hddProcessando.Value = ""
                        Return
                End Select
            End If
        Next
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Refeições inseridas com sucesso.');", True)
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub
End Class
