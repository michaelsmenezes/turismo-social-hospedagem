Partial Class Alimentacao
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDataPrevisao.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataManualIni.Text = Format("01/" + DatePart(DateInterval.Month, Date.Today).ToString + "/" + DatePart(DateInterval.Year, Date.Today).ToString)
            txtDataManualFim.Text = DateAdd(DateInterval.Day, -1, CDate(Format("01/" + DatePart(DateInterval.Month, Date.Today.AddMonths(1)).ToString + "/" + DatePart(DateInterval.Year, Date.Today).ToString)))
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub lnkPrevisao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrevisao.Click, lnkMeiaDiaria.Click, lnkRefeicaoManual.Click, lnkConsumo.Click, lnkHistorico.Click, lnkHistoricoTipo.Click
        lnkPrevisao.Font.Bold = (sender.Text = "Previsão do Restaurante")
        pnlPrevisao.Visible = (sender.Text = "Previsão do Restaurante")
        lnkMeiaDiaria.Font.Bold = (sender.Text = "Meia Diária sem Pernoite")
        pnlMeiaDiaria.Visible = (sender.Text = "Meia Diária sem Pernoite")
        lnkRefeicaoManual.Font.Bold = (sender.Text = "Registro Manual de Refeição")
        pnlRefeicaoManual.Visible = (sender.Text = "Registro Manual de Refeição")
        lnkConsumo.Font.Bold = (sender.Text = "Consumo Restaurante")
        pnlConsumo.Visible = (sender.Text = "Consumo Restaurante")
        lnkHistorico.Font.Bold = (sender.Text = "Histórico de Refeição")
        pnlHistorico.Visible = (sender.Text = "Histórico de Refeição")
        lnkHistoricoTipo.Font.Bold = (sender.Text = "Histórico Refeição por Tipo")
        pnlHistoricoTipo.Visible = (sender.Text = "Histórico Refeição por Tipo")
        If pnlPrevisao.Visible Then
            txtDataPrevisao.Focus()
        End If
    End Sub

    Protected Sub bntConsultarPrevisao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntConsultarPrevisao.Click
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objRestauranteDAO = New Turismo.RestauranteDAO(bd)
        Dim lista As New ArrayList
        lista = objRestauranteDAO.previsao(Mid(txtDataPrevisao.Text, 7, 4) & "-" & Mid(txtDataPrevisao.Text, 4, 2) & "-" & Mid(txtDataPrevisao.Text, 1, 2))
        gdvPrevisao.DataSource = lista
        gdvPrevisao.DataBind()
        lista = objRestauranteDAO.previsaoPratoRapido(txtDataPrevisao.Text)
        gdvPrevisaoPratoRapido.DataSource = lista
        gdvPrevisaoPratoRapido.DataBind()
        lista = objRestauranteDAO.previsaoPassanteRestaurante(txtDataPrevisao.Text)
        Dim objRestauranteVO As New Turismo.RestauranteVO
        objRestauranteVO = lista(0)
        If objRestauranteVO.confirmado <> "0" Or objRestauranteVO.consumir <> "0" Or objRestauranteVO.consumido <> "0" Then
            gdvPrevisaoPassanteRestaurante.DataSource = lista
        Else
            gdvPrevisaoPassanteRestaurante.DataSource = Nothing
        End If
        gdvPrevisaoPassanteRestaurante.DataBind()
    End Sub

    Protected Sub gdvPrevisao_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrevisao.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvPrevisao.DataKeys(e.Row.RowIndex).Item("RefeicaoTipo").ToString = "D" Then
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Desjejum"
            ElseIf gdvPrevisao.DataKeys(e.Row.RowIndex).Item("RefeicaoTipo").ToString = "A" Then
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Almoço"
            Else
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Jantar"
            End If
            CType(e.Row.FindControl("lnkPago"), LinkButton).Text = _
              CInt(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("DesjejumRefeicaoConfirmado")) - CInt(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("Estada"))
            CType(e.Row.FindControl("lnkAPagar"), LinkButton).Text = _
              CInt(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("DesjejumRefeicao")) - CInt(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("DesjejumRefeicaoConfirmado"))
        End If
    End Sub

    Protected Sub bntConsultarManual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntConsultarManual.Click
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objRestauranteDAO = New Turismo.RestauranteDAO(bd)
        Dim lista As New ArrayList
        lista = objRestauranteDAO.consultaRefeicaoManual(txtDataManualIni.Text, txtDataManualFim.Text)
        gdvManual.DataSource = lista
        gdvManual.DataBind()
    End Sub

    Protected Sub gdvManual_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvManual.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvManual.DataKeys(e.Row.RowIndex).Item("refTipo").ToString = "D" Then
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Desjejum"
            ElseIf gdvManual.DataKeys(e.Row.RowIndex).Item("refTipo").ToString = "A" Then
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Almoço"
            Else
                CType(e.Row.FindControl("lnkRefeicao"), LinkButton).Text = "Jantar"
            End If
        End If
    End Sub

    Protected Sub gdvManual_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvManual.SelectedIndexChanged
        hddRefId.Value = gdvManual.DataKeys(gdvManual.SelectedIndex).Item(0).ToString
    End Sub
End Class
