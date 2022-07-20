Partial Class frmRestaurantePrevisao
    Inherits System.Web.UI.Page
    Dim ObjRestaurantePrevisaoVO As RestaurantePrevisoesVO
    Dim ObjRestaurantePrevisaoDAO As RestaurantePrevisoesDAO
    Dim ObjPratosRapidosVO As PratosRapidosVO
    Dim ObjPratosRapidosDAO As PratosRapidosDAO
    Dim V_Previsao As Long = 0
    Dim V_Estada As Long = 0
    Dim V_Pago As Long = 0
    Dim V_Apagar As Long = 0

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        ObjRestaurantePrevisaoVO = New RestaurantePrevisoesVO
        ObjRestaurantePrevisaoDAO = New RestaurantePrevisoesDAO()
        ObjPratosRapidosVO = New PratosRapidosVO
        ObjPratosRapidosDAO = New PratosRapidosDAO
        'Previsão de refeições
        gdvPrevisoesRef.DataSource = ObjRestaurantePrevisaoDAO.Consultar(btnConsultar.Attributes.Item("AliasBancoTurismo"), Format(CDate(txtDia.Text), "yyyy-MM-dd"), "A")
        gdvPrevisoesRef.DataBind()
        'Previsão de Pratos Rápidos
        gdvPratosRapidos.DataSource = ObjPratosRapidosDAO.Consultar(ObjPratosRapidosVO, Format(CDate(txtDia.Text), "yyyy-MM-dd"), btnConsultar.Attributes.Item("AliasBancoTurismo"))
        gdvPratosRapidos.DataBind()
        'Previsão de pratos rápidos no restaurante
        gdvPassantesRestaurante.DataSource = ObjPratosRapidosDAO.ConsultaPratosRestaurante(ObjPratosRapidosVO, Format(CDate(txtDia.Text), "yyyy-MM-dd"), btnConsultar.Attributes.Item("AliasBancoTurismo"))
        gdvPassantesRestaurante.DataBind()
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
                    btnConsultar.Attributes.Add("AliasBancoTurismo",  "TurismoSocialPiri")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnConsultar.Attributes.Add("BancoTurismoSocial", "dbTurismoSocialPiri")
            End Select

            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If

            txtDia.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDia.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDia.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            'gdvPrevisoesRef_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'gdvPrevisoesRef_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'gdvPratosRapidos_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'gdvPratosRapidos_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlConsulta_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlConsulta_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlAutorizadoRest_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlAutorizadoRest_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlPratosRapidos_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlPratosRapidos_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            btnConsultar_Click(sender, e)
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

    Protected Sub gdvPrevisoesRef_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrevisoesRef.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            V_Previsao = V_Previsao + gdvPrevisoesRef.DataKeys(e.Row.RowIndex).Item(1).ToString
            V_Estada = V_Estada + gdvPrevisoesRef.DataKeys(e.Row.RowIndex).Item(2).ToString
            V_Pago = V_Pago + gdvPrevisoesRef.DataKeys(e.Row.RowIndex).Item(3).ToString
            V_Apagar = V_Apagar + gdvPrevisoesRef.DataKeys(e.Row.RowIndex).Item(4).ToString
        End If
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(0).Text = "TOTAL GERAL"
            e.Row.Cells(1).Text = V_Previsao
            e.Row.Cells(2).Text = V_Estada
            e.Row.Cells(3).Text = V_Pago
            e.Row.Cells(4).Text = V_Apagar
            e.Row.ForeColor = Drawing.Color.Black
            e.Row.Font.Bold = True
        End If

    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub
End Class
