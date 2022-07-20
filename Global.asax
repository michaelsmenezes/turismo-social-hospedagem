<%@ Application Language="VB" %>


<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        Dim ping As New System.Net.NetworkInformation.Ping()
        Dim objTestaGrupo As New Uteis.TestaUsuario
        Dim objListaGrupo As New Uteis.TestaUsuario

        Dim varOffice As String
        varOffice = objTestaGrupo.listaOffice(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", ""))

        If varOffice = "Pirenópolis" Then
            Session.Add("MasterPage", "~/TurismoSocialPiri.Master")
        Else
            Session.Add("MasterPage", "~/TurismoSocial.Master")
        End If

        Dim varGrupos = objListaGrupo.listaGrupos(Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "")).ToUpper

        Session.Add("GrupoCEREC", varGrupos.Contains("TURISMO SOCIAL CEREC") Or varGrupos.Contains("TURISMO SOCIAL RESERVA"))
        Session.Add("GrupoGP", varGrupos.Contains("TURISMO SOCIAL DR") Or varGrupos.Contains("TURISMO SOCIAL FEDERACAO"))
        Session.Add("GrupoDR", varGrupos.Contains("TURISMO SOCIAL DR"))
        Session.Add("GrupoEmissivo", varGrupos.Contains("TURISMO SOCIAL EMISSIVO"))
        Session.Add("GrupoRecepcao", varGrupos.Contains("TURISMO SOCIAL RECEPCAO"))
        Session.Add("GrupoGovernanca", varGrupos.Contains("TURISMO SOCIAL GOVERNANCA"))
        Session.Add("GrupoPortariaRestaurante", varGrupos.Contains("TURISMO SOCIAL PORTARIA RESTAURANTE"))
        Session.Add("GrupoAlimentacao", varGrupos.Contains("TURISMO SOCIAL ALIMENTACAO"))
        Session.Add("GrupoManutencao", varGrupos.Contains("TURISMO SOCIAL MANUTENCAO"))
        Session.Add("GrupoGerencial", varGrupos.Contains("TURISMO SOCIAL GERENCIAL"))
        Session.Add("GrupoRanking", varGrupos.Contains("TURISMO SOCIAL RANKING"))
        Session.Add("GrupoValorReserva", varGrupos.Contains("TURISMO SOCIAL CADASTRA VALOR RESERVA"))
        Session.Add("GrupoCentralAtendimento", varGrupos.Contains("TURISMO SOCIAL RESERVA CENTRAL DE ATENDIMENTOS"))
        Session.Add("GrupoMudarDataInsercao", varGrupos.Contains("TURISMO SOCIAL RESERVA MUDA DATA INSERCAO"))
        Session.Add("ApagaBoleto", varGrupos.Contains("TURISMO SOCIAL APAGA BOLETO"))

        Session.Add("GrupoRecepcaoPiri", varGrupos.Contains("TURISMO SOCIAL PIRI RECEPCAO"))
        Session.Add("GrupoGovernancaPiri", varGrupos.Contains("TURISMO SOCIAL PIRI GOVERNANCA"))
        Session.Add("GrupoPortariaPiri", varGrupos.Contains("TURISMO SOCIAL PIRI PORTARIA"))
        Session.Add("GrupoPortariaRestaurantePiri", varGrupos.Contains("TURISMO SOCIAL PIRI PORTARIA RESTAURANTE"))
        Session.Add("GrupoAlimentacaoPiri", varGrupos.Contains("TURISMO SOCIAL PIRI ALIMENTACAO"))
        Session.Add("GrupoManutencaoPiri", varGrupos.Contains("TURISMO SOCIAL PIRI MANUTENCAO"))
        Session.Add("GrupoGerencialPiri", varGrupos.Contains("TURISMO SOCIAL PIRI GERENCIAL"))
        Session.Add("ApagaBoletoPiri", varGrupos.Contains("TURISMO SOCIAL PIRI APAGA BOLETO"))

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
        Session.Abandon()
    End Sub
</script>

