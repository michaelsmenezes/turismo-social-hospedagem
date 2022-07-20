<%@ Page Language="VB"  MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True" CodeFile="TurismoSocialNet.aspx.vb" Inherits="TurismoSocialNet" %>

<%--<%@ Page Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="TurismoSocialNet.aspx.vb" Inherits="TurismoSocialNet" %>--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updPnlTurismoSocial" runat="server">
        <ContentTemplate>
            <div style="top: 0px; left: 0px; z-index: 10; width: 100%">
                <table width="100%">
                    <tr>
                        <td align="center" class="formLabelWeb" height="100px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="formLabelWeb" height="100px">
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td height="100px" align="center">
                            <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/Sesc 70 anos Grande.png" Height="180px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="formLabelWeb" height="100px">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
