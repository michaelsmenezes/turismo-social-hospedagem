<%@ Page Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="AcessoNegado.aspx.vb" Inherits="AcessoNegado" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
        <table width="1000px">
            <tr>
                <td align="center" class="formLabelWeb" height="100px">
                </td>
            </tr>
            <tr>
                <td align="center" class="formLabelWeb" height="100px">
                    Acesso Negado
                </td>
            </tr>
            <tr>
                <td height="100px" align="center">
                    <img alt="" src="images/barricada.gif" />
                </td>
            </tr>
            <tr>
                <td align="center" class="formLabelWeb" height="100px">
                    Você não possui permissão de acesso a esta página.
                </td>
            </tr>
        </table>
</asp:Content>
