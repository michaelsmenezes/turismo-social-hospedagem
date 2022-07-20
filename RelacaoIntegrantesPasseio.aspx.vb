Imports Microsoft.Reporting.WebForms

Partial Class RelacaoIntegrantesPasseio
    Inherits System.Web.UI.Page

    Dim objPasseioDAO As Turismo.PasseioDAO
    Dim objPasseioVO As New Turismo.PasseioVO

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dataPasseio As String
        'Busca o nome e a data da reserva 
        objPasseioDAO = New Turismo.PasseioDAO
        objPasseioVO = objPasseioDAO.consultar(Request.QueryString("resId"))
        dataPasseio = Format(CDate(objPasseioVO.data), "dd/MM/yyyy")

        rptVwrRelacaoIntegrantesPasseio.Visible = True

        'USO DO REPORTING SERVICES
        Dim params(2) As ReportParameter
        params(0) = New ReportParameter("ResId", Request.QueryString("resId"))
        params(1) = New ReportParameter("ResNome", objPasseioVO.resNome)
        params(2) = New ReportParameter("ResData", dataPasseio)

        rptVwrRelacaoIntegrantesPasseio.ServerReport.SetParameters(params)
        rptVwrRelacaoIntegrantesPasseio.ServerReport.Refresh()
    End Sub
End Class
