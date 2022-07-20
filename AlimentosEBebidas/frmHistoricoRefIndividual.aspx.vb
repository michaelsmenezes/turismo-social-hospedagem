
Partial Class frmHistoricoRefIndividual
    Inherits System.Web.UI.Page
    Dim ObjHRIndividualVO As HistoricoRefeicaoIndividualVO
    Dim ObjHRIndividualDAO As HistoricoRefeicaoIndividualDAO

    Private Comerciario As Integer
    Private Dependente As Integer
    Private Conveniado As Integer
    Private Usuario As Integer
    Private PasComerciario As Integer
    Private PasDependente As Integer
    Private PasUsuario As Integer
    Private Cortesias As Integer
    Private InsercaoManual As Integer
    Private Menor5 As Integer
    Private Servidores As Integer
    Private TotalRefeicao As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo",  "TurismoSocialPiri")
            End Select

            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Alimentacao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Alimentacao")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If

            'pnlPesquisa_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlPesquisa_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'pnlGridHistorico_RoundedCornersExtender.Color = Drawing.Color.LightSteelBlue
            'pnlGridHistorico_RoundedCornersExtender.BorderColor = Drawing.Color.LightSteelBlue
            'Inserindo os anos no combo'
            Dim AnoBase As Integer
            Dim Cont As Integer = 0
            AnoBase = Now.Year + 1
            AnoBase = AnoBase
            While (Cont <= 20)
                drpAno.Items.Add(AnoBase)
                Cont = Cont + 1
                AnoBase = AnoBase - 1
            End While
            'Definindo o mes atual'
            drpMes.SelectedValue = Now.Month
            drpAno.SelectedValue = Now.Year
            'Executando a consulta
            btnConsultar_Click(sender, e)
        End If
    End Sub
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        ObjHRIndividualVO = New HistoricoRefeicaoIndividualVO
        ObjHRIndividualDAO = New HistoricoRefeicaoIndividualDAO
        gdvHistoricoRef.DataSource = ObjHRIndividualDAO.Consultar(ObjHRIndividualVO, btnConsultar.Attributes.Item("AliasBancoTurismo"), drpMes.SelectedValue, drpAno.SelectedValue, drpTipo.SelectedValue)
        gdvHistoricoRef.DataBind()
    End Sub

    Protected Sub gdvHistoricoRef_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvHistoricoRef.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Comerciario = 0
            Dependente = 0
            Conveniado = 0
            Usuario = 0
            PasComerciario = 0
            PasDependente = 0
            PasUsuario = 0
            Cortesias = 0
            InsercaoManual = 0
            Menor5 = 0
            Servidores = 0
            TotalRefeicao = 0
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Comerciario = Comerciario + CInt(e.Row.Cells(1).Text)
            PasComerciario = PasComerciario + CInt(e.Row.Cells(2).Text)
            InsercaoManual = InsercaoManual + CInt(e.Row.Cells(3).Text)
            Servidores = Servidores + CInt(e.Row.Cells(4).Text)
            Dependente = Dependente + CInt(e.Row.Cells(5).Text)
            PasDependente = PasDependente + CInt(e.Row.Cells(6).Text)
            Menor5 = Menor5 + CInt(e.Row.Cells(7).Text)
            Usuario = Usuario + CInt(e.Row.Cells(8).Text)
            PasUsuario = PasUsuario + CInt(e.Row.Cells(9).Text)
            Conveniado = Conveniado + CInt(e.Row.Cells(10).Text)
            Cortesias = Cortesias + CInt(e.Row.Cells(12).Text)
            TotalRefeicao = TotalRefeicao + CInt(e.Row.Cells(11).Text)
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Font.Bold = True
            e.Row.HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).Text = "TOTAL"
            e.Row.Cells(1).Text = FormatNumber(Comerciario, 0)
            e.Row.Cells(2).Text = FormatNumber(PasComerciario, 0)
            e.Row.Cells(3).Text = FormatNumber(InsercaoManual, 0)
            e.Row.Cells(4).Text = FormatNumber(Servidores, 0)
            e.Row.Cells(5).Text = FormatNumber(Dependente, 0)
            e.Row.Cells(6).Text = FormatNumber(PasDependente, 0)
            e.Row.Cells(7).Text = FormatNumber(Menor5, 0)
            e.Row.Cells(8).Text = FormatNumber(Usuario, 0)
            e.Row.Cells(9).Text = FormatNumber(PasUsuario, 0)
            e.Row.Cells(10).Text = FormatNumber(Conveniado, 0)
            e.Row.Cells(11).Text = FormatNumber(TotalRefeicao, 0)
            e.Row.Cells(12).Text = FormatNumber(Cortesias, 0)
        End If
    End Sub


    Protected Sub btnVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltar.Click
        Response.Redirect("frmAlimentosBebidas.aspx")
    End Sub
End Class
