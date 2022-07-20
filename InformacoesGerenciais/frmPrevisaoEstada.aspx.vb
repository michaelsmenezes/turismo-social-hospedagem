Partial Class InformacoesGerenciais_frmPrevisaoEstada
    Inherits System.Web.UI.Page
    Private PrevisaoLeitosOcupados As Decimal
    Private TotalLeitos As Decimal
    Private PrevisaoAptosOcupados As Decimal
    Private PrevisaoEstada As Decimal
    Private PrevisaoConfirmado As Decimal
    Private PrevisaoPagar As Decimal
    Private SomaApartamentos As Decimal
    Private AptosBloqueados As Decimal
    Private AptosManutencao As Decimal
    Private AptosLivres As Decimal
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
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheet.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
                    drpAcomodacoes.Items.Clear()
                    drpAcomodacoes.Items.Add(New ListItem("Todos", "T"))
                    drpAcomodacoes.Items.Add(New ListItem("Central de Reservas", "N"))
                    drpAcomodacoes.Items.Add(New ListItem("Federação", "S"))
                    drpAcomodacoes.Items.Add(New ListItem("PNE", "E"))
                    drpAcomodacoes.Items.Add(New ListItem("DR", "R"))
                    drpAcomodacoes.Items.Add(New ListItem("Flutuante", "F"))
                    drpBloco.Enabled = True
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
                    drpAcomodacoes.Items.Clear()
                    drpAcomodacoes.Items.Add(New ListItem("Todos", "T"))
                    drpAcomodacoes.Items.Add(New ListItem("Central de Reservas", "N"))
                    drpAcomodacoes.Items.Add(New ListItem("Especiais", "E"))
                    drpAcomodacoes.Items.Add(New ListItem("Federação", "S"))
                    drpAcomodacoes.Items.Add(New ListItem("Flutuante", "F"))
                    drpAcomodacoes.Items.Add(New ListItem("RTM", "R"))
                    drpBloco.Enabled = False
            End Select
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            'Formatando datas
            txtDataIni.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataIni.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtDataIni.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtDataFim.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataFim.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtDataFim.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            'Data de hoje padrão
            txtDataIni.Text = Format(Date.Today, "dd/MM/yyyy")
            txtDataFim.Text = Format(Date.Today, "dd/MM/yyyy")
            'btnConsultar_Click(sender, e)
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Dim ObjPrevisaoEstadaVO As New PrevisaoEstadaVO
        Dim ObjPrevisaoEstadaDAO As New PrevisaoEstadaDAO
        gdvPrevisao.DataSource = ObjPrevisaoEstadaDAO.ConsultaPrevisao(ObjPrevisaoEstadaVO, btnConsultar.Attributes.Item("AliasBancoTurismo").ToString, txtDataIni.Text, txtDataFim.Text, drpBloco.SelectedValue, drpAcomodacoes.SelectedValue)
        gdvPrevisao.DataBind()
    End Sub

    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmMenuGerencial.aspx")
    End Sub

    Protected Sub gdvPrevisao_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvPrevisao.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            PrevisaoLeitosOcupados = 0
            PrevisaoAptosOcupados = 0
            TotalLeitos = 0
            PrevisaoEstada = 0
            AptosBloqueados = 0
            AptosManutencao = 0
            AptosLivres = 0
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            TotalLeitos = TotalLeitos + CDec(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvTotalLeitos"))
            PrevisaoAptosOcupados = PrevisaoAptosOcupados + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvTotalPrevisao"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvTotalPrevisao").ToString.Replace(" ", "").IndexOf("=")))
            PrevisaoEstada = PrevisaoEstada + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoE"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoE").ToString.Replace(" ", "").IndexOf("=")))
            PrevisaoConfirmado = PrevisaoConfirmado + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoR"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoR").ToString.Replace(" ", "").IndexOf("=")))
            PrevisaoPagar = PrevisaoPagar + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoP"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoP").ToString.Replace(" ", "").IndexOf("=")))
            SomaApartamentos = SomaApartamentos + CDec(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvQtdeTotalAptos"))
            AptosBloqueados = AptosBloqueados + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvBloqueados"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("prvBloqueados").ToString.Replace(" ", "").IndexOf("=")))
            AptosManutencao = AptosManutencao + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoM"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("AptoM").ToString.Replace(" ", "").IndexOf("=")))
            AptosLivres = AptosLivres + CDec(Mid(gdvPrevisao.DataKeys(e.Row.RowIndex).Item("Livre"), 1, gdvPrevisao.DataKeys(e.Row.RowIndex).Item("Livre").ToString.Replace(" ", "").IndexOf("=")))
        End If

        If e.Row.RowType = DataControlRowType.Footer And gdvPrevisao.Rows.Count > 0 Then
            'If gdvPrevisao.Rows.Count > 0 Then
            e.Row.Cells(0).Text = "TOTAIS"
            e.Row.Cells(1).Text = FormatNumber(TotalLeitos, 0).ToString
            e.Row.Cells(2).Text = PrevisaoAptosOcupados.ToString + " = " + FormatNumber((PrevisaoAptosOcupados * 100 / TotalLeitos), 2).ToString + "%"
            e.Row.Cells(3).Text = PrevisaoEstada.ToString + " = " + FormatNumber((PrevisaoEstada * 100 / TotalLeitos), 2).ToString + "%"
            e.Row.Cells(4).Text = PrevisaoConfirmado.ToString + " = " + FormatNumber((PrevisaoConfirmado * 100 / TotalLeitos), 2).ToString + "%"
            e.Row.Cells(5).Text = PrevisaoPagar.ToString + " = " + FormatNumber((PrevisaoPagar * 100 / TotalLeitos), 2).ToString + "%"
            e.Row.Cells(6).Text = SomaApartamentos.ToString
            e.Row.Cells(7).Text = AptosBloqueados.ToString + " = " + FormatNumber((AptosBloqueados * 100 / SomaApartamentos), 2).ToString + "%"
            e.Row.Cells(8).Text = AptosManutencao.ToString + " = " + FormatNumber((AptosManutencao * 100 / SomaApartamentos), 2).ToString + "%"
            e.Row.Cells(9).Text = AptosLivres.ToString + " = " + FormatNumber((AptosLivres * 100 / SomaApartamentos), 2).ToString + "%"
        End If
    End Sub
End Class
