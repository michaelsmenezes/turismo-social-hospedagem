Imports Turismo

Partial Public Class TurismoSocial
    Inherits System.Web.UI.MasterPage
    Dim objTestaGrupo As New Uteis.TestaUsuario
    Dim oTurismo = New ConexaoDAO("TurismoSocialCaldas")


    Protected Sub updPnlTurismoSocial_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles updPnlTurismoSocial.Load
        If Not IsPostBack Then

            lblUsuario.Text = Context.User.Identity.Name.Replace("SESC-GO.COM.BR\", "")

            If ConfigurationManager.AppSettings("Environment") = "HOMOL" Then
                lblUsuario.Text = lblUsuario.Text & " - Datasource: " & oTurismo.strConnection.DataSource & "- Database: " & oTurismo.strConnection.Database
                lblUsuario.ForeColor = Drawing.Color.Red
            Else
                lblUsuario.Text = lblUsuario.Text & " - " & oTurismo.strConnection.DataSource
            End If


            'mnuTurismoSocial.Items(1).Text = 0 : Reserva
            '                                 1 : Recepção
            '                                 2 : Emissivo
            '                                 3 : Governança
            '                                 4 : Restaurante
            '                                 5 : Alimentação
            '                                 6 : Manutenção
            '                                 7 : Ranking
            '                                 8 : Gerencial
            '                                 9 : Simulador
            '                                 10: Receptivo
            '                                 11: Valor

            'Menu Reserva e Emissivo
            mnuTurismoSocial.Items(0).Selectable = Session("GrupoCEREC") Or Session("GrupoDR") Or Session("GrupoGP") Or Session("GrupoEmissivo")
            If Not mnuTurismoSocial.Items(0).Selectable Then
                mnuTurismoSocial.Items(0).Text = "" 'Reserva
            End If

            mnuTurismoSocial.Items(1).Selectable = Session("GrupoRecepcao")
            If Not mnuTurismoSocial.Items(1).Selectable Then
                mnuTurismoSocial.Items(1).Text = "" 'Recepção
                mnuTurismoSocial.Items(10).Text = "" 'Receptivo
                mnuTurismoSocial.Items(2).Text = "" 'Emissivo
            End If

            mnuTurismoSocial.Items(3).Selectable = Session("GrupoGovernanca")
            If Not mnuTurismoSocial.Items(3).Selectable Then
                mnuTurismoSocial.Items(3).Text = "" 'Governança
            End If

            mnuTurismoSocial.Items(4).Selectable = Session("GrupoPortariaRestaurante")
            If Not mnuTurismoSocial.Items(4).Selectable Then
                mnuTurismoSocial.Items(4).Text = "" 'Restaurante
            End If

            mnuTurismoSocial.Items(5).Selectable = Session("GrupoAlimentacao")
            If Not mnuTurismoSocial.Items(5).Selectable Then
                mnuTurismoSocial.Items(5).Text = "" 'Alimentação
            End If

            mnuTurismoSocial.Items(6).Selectable = Session("GrupoManutencao")
            If Not mnuTurismoSocial.Items(6).Selectable Then
                mnuTurismoSocial.Items(6).Text = "" 'Manutenção
            End If

            mnuTurismoSocial.Items(7).Selectable = Session("GrupoRanking")
            If Not mnuTurismoSocial.Items(7).Selectable Then
                mnuTurismoSocial.Items(7).Text = "" 'Ranking
            End If

            mnuTurismoSocial.Items(8).Selectable = Session("GrupoGerencial")
            If Not mnuTurismoSocial.Items(8).Selectable Then
                mnuTurismoSocial.Items(8).Text = "" 'Gerencial
            End If

            mnuTurismoSocial.Items(9).Selectable = Session("GrupoGerencial")
            If Not mnuTurismoSocial.Items(9).Selectable Then
                mnuTurismoSocial.Items(9).Text = "" 'Financeiro
            End If

            mnuTurismoSocial.Items(11).Selectable = Session("GrupoValorReserva")
            If Not mnuTurismoSocial.Items(11).Selectable Then
                mnuTurismoSocial.Items(11).Text = "" 'Simulador de valor
            End If

            lnkPirenopolis.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        End If
    End Sub

    Protected Sub lnkPirenopolis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPirenopolis.Click
        oTurismo = New ConexaoDAO("TurismoSocialPiri")
        Session("MasterPage") = "~/TurismoSocialPiri.Master"
        Response.Redirect("~/TurismoSocialNet.aspx")
    End Sub

    Protected Sub imgHome_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgHome.Click
        lnkPirenopolis_Click(sender, e)
    End Sub
End Class

