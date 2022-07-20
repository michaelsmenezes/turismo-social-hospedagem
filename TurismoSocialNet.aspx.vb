Partial Class TurismoSocialNet
    Inherits System.Web.UI.Page

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If IsNothing(Session("MasterPage")) Then
            ScriptManager.RegisterStartupScript(Page, Page.Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi encontrado a sua matrícula nas informações da conta de rede. Por favor, abra um chamado técnico solicitando a inserção desta informação." + "');", True)
        ElseIf Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
End Class
