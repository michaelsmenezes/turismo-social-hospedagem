Partial Class PortariaRestaurante
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Me.txtConsulta.Attributes.Add("OnMouseUp", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='MouseUp';")
            'Me.txtConsulta.Attributes.Add("OnMouseDown", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='MouseDown';")
            Me.txtConsulta.Attributes.Add("OnDrop", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='Drop';")
            Me.txtConsulta.Attributes.Add("OnPaste", "javascript:ctl00_conPlaHolTurismoSocial_hddOperacao.value='Paste';")
            Me.txtConsulta.Attributes.Add("OnChange", "javascript:if (this.value.length==1) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo1.value=agora.getTime();} else {if (this.value.length==6) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo2.value=agora.getTime(); ctl00_conPlaHolTurismoSocial_hddOperacao.value = ctl00_conPlaHolTurismoSocial_hddPeriodo2.value - ctl00_conPlaHolTurismoSocial_hddPeriodo1.value > 200; if (ctl00_conPlaHolTurismoSocial_hddOperacao.value=='true') {alert('Utilize a leitora de cartão.');this.value='';}}};")
            Me.txtConsulta.Attributes.Add("OnKeyPress", "javascript:if (this.value.length==1) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo1.value=agora.getTime();} else {if (this.value.length==6) {agora = new Date(); ctl00_conPlaHolTurismoSocial_hddPeriodo2.value=agora.getTime(); ctl00_conPlaHolTurismoSocial_hddOperacao.value = ctl00_conPlaHolTurismoSocial_hddPeriodo2.value - ctl00_conPlaHolTurismoSocial_hddPeriodo1.value > 200; if (ctl00_conPlaHolTurismoSocial_hddOperacao.value=='true') {alert('Utilize a leitora de cartão.');this.value='';}}};")
            lblIsento.Text = ""
        End If
        lblNome.Text = ""
        lblSituacao.Text = ""
        txtConsulta.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub txtConsulta_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConsulta.TextChanged, btnIsentoMais.Click, btnIsentoMenos.Click
        If hddOperacao.Value = "false" Then
            Dim bd As String
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                bd = "TurismoSocialCaldas"
            Else
                bd = "TurismoSocialPiri"
            End If
            Dim objRestauranteDAO = New Turismo.RestauranteDAO(bd)
            Dim lista As New ArrayList
            If sender Is txtConsulta Then
                lista = objRestauranteDAO.entrada(txtConsulta.Text)
            ElseIf sender Is btnIsentoMais Then
                lista = objRestauranteDAO.entrada("SOMA")
            ElseIf sender Is btnIsentoMenos Then
                lista = objRestauranteDAO.entrada("SUBTRA")
            End If
            Dim objRestauranteVO As New Turismo.RestauranteVO
            objRestauranteVO = lista.Item(0)
            lblNome.Text = objRestauranteVO.intNome
            lblSituacao.Text = objRestauranteVO.situacao
            lblIsento.Text = objRestauranteVO.isento
            'Else
            'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Utilize a leitora de cartão.');", True)
        End If
        txtConsulta.Text = ""
        hddOperacao.Value = "false"
    End Sub

    Protected Sub btnSituacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSituacao.Click
        Dim bd As String
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            bd = "TurismoSocialCaldas"
        Else
            bd = "TurismoSocialPiri"
        End If
        Dim objRestauranteDAO = New Turismo.RestauranteDAO(bd)
        Dim lista As New ArrayList
        lista = objRestauranteDAO.situacao
        If lista.Count > 0 Then
            Dim objRestauranteVO As New Turismo.RestauranteVO
            objRestauranteVO = lista.Item(0)
            lblPrevisao.Visible = True
            lblIngressaram.Visible = True
            lblHospede.Visible = True
            lblMeia.Visible = True
            lblPrevisaoHospede.Text = objRestauranteVO.totalHospede
            lblIngressaramHospede.Text = objRestauranteVO.hospede
            lblPrevisaoMeia.Text = objRestauranteVO.totalPassante
            lblIngressaramMeia.Text = objRestauranteVO.passante
        End If
    End Sub
End Class
