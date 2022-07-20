Imports Microsoft.Reporting.WebForms
'Imports dtsRelatorioGerencialTableAdapters
Imports dtsTransferenciasTableAdapters


Partial Class InformacoesGerenciais_frmConsultaTranferencias
    Inherits System.Web.UI.Page

    Dim objPrevisaoEstadaVO As PrevisaoEstadaVO
    Dim objPrevisaoEstadaDAO As PrevisaoEstadaDAO

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtDataIni.Value = Format(Date.Now, "dd/MM/yyyy")
            txtDataFim.Value = Format(Date.Now, "dd/MM/yyyy")

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                lblTituloPrincipal.Text = "Consulta Transferências(Caldas Novas)"
            Else
                lblTituloPrincipal.Text = "Consulta Transferências(Pirenópolis)"
            End If
        End If
    End Sub
    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        If (txtDataIni.Value = "__/__/____" Or txtDataFim.Value = "__/__/____" Or txtDataIni.Value = "" Or txtDataFim.Value = "") Then
            'divReportview.Visible = False
            Return
        End If


        Dim Unidade As String = ""
        Dim OrdersOverBook As DataTableTransferenciaNewTableAdapter = New DataTableTransferenciaNewTableAdapter
        Dim VarSqlRateio As New Text.StringBuilder("")
        Dim conexao As String = ""
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            conexao = "TurismoSocialCaldas"
        Else
            conexao = "TurismoSocialPiri"
        End If

        objPrevisaoEstadaVO = New PrevisaoEstadaVO
        objPrevisaoEstadaDAO = New PrevisaoEstadaDAO()

        objPrevisaoEstadaVO = New PrevisaoEstadaVO
        objPrevisaoEstadaDAO = New PrevisaoEstadaDAO()
        gdvTransferencia.DataSource = ""
        gdvTransferencia.DataBind()
        'gdvTransferencia.DataSource = objPrevisaoEstadaDAO.ConsultarTransferencia(conexao, txtDataIni.Value, txtDataFim.Value)
        If gdvTransferencia.DataSource.Count > 0 Then
            divTransferencia.Visible = True
            gdvTransferencia.Visible = True
            gdvTransferencia.DataBind()
        Else
            divTransferencia.Visible = False
        End If

    End Sub
    Protected Sub btnVoltar_Click(sender As Object, e As EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmMenuGerencial.aspx")
    End Sub


End Class
