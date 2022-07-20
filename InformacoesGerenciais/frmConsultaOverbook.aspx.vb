Imports Microsoft.Reporting.WebForms
Imports dtsRelatorioGerencialTableAdapters


Partial Class InformacoesGerenciais_frmConsultaOverbook
    Inherits System.Web.UI.Page

    Dim objPrevisaoEstadaVO As PrevisaoEstadaVO
    Dim objPrevisaoEstadaDAO As PrevisaoEstadaDAO

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDataIni.Value = Format(Date.Now.Date, "dd/MM/yyyy")
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                lblTituloPrincipal.Text = "Consulta situação de hospedagem e overbooking(Caldas Novas)"
            Else
                lblTituloPrincipal.Text = "Consulta situação de hospedagem e overbooking(Pirenópolis)"
            End If
        End If
    End Sub
    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim Unidade As String = ""
        Dim OrdersOverBook As DataTableOverBookTableAdapter = New DataTableOverBookTableAdapter
        Dim VarSqlRateio As New Text.StringBuilder("")
        Dim conexao As String = ""
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        'If Local.SelectedIndex = 0 Then
        '    conexao = "TurismoSocialCaldas"
        'Else
        '    conexao = "TurismoSocialPiri"
        'End If
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            conexao = "TurismoSocialCaldas"
        Else
            conexao = "TurismoSocialPiri"
        End If

        objPrevisaoEstadaVO = New PrevisaoEstadaVO
        objPrevisaoEstadaDAO = New PrevisaoEstadaDAO()
        gdvRelSolicitacao.DataSource = ""
        gdvRelSolicitacao.DataBind()
        gdvRelSolicitacao.DataSource = objPrevisaoEstadaDAO.RelacaodeSolicitacoesPorStatus(conexao, txtDataIni.Value, txtDias.Value)
        If gdvRelSolicitacao.DataSource.Count > 0 Then
            DivRelatorio.Visible = True
            gdvRelSolicitacao.Visible = True
            gdvRelSolicitacao.DataBind()
        Else
            DivRelatorio.Visible = False
        End If


        objPrevisaoEstadaVO = New PrevisaoEstadaVO
        objPrevisaoEstadaDAO = New PrevisaoEstadaDAO()
        gdvOver.DataSource = ""
        gdvOver.DataBind()
        gdvOver.DataSource = objPrevisaoEstadaDAO.ConsultaOverBook(conexao, txtDataIni.Value, txtDias.Value)
        If gdvOver.DataSource.Count > 0 Then
            divOver.Visible = True
            gdvOver.Visible = True
            gdvOver.DataBind()
        Else
            divOver.Visible = False
        End If

    End Sub
    Protected Sub btnVoltar_Click(sender As Object, e As EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmMenuGerencial.aspx")
    End Sub
End Class
