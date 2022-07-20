<%@ Page Title="Manutenções do sistema" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmConfiguracoes.aspx.vb" Inherits="frmConfiguracoes" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <p>
                <br />
            </p>
            <br />
            <br />
            <br />
            <br />
            <br />
            <div align="left">
                <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
            </div>
            <br />
            <br />
            <asp:TreeView ID="mnuConfiguracao" runat="server" ImageSet="Arrows">
                <ParentNodeStyle Font-Bold="False" />
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
                    HorizontalPadding="0px" VerticalPadding="0px" />
                <Nodes>
                    <asp:TreeNode NavigateUrl="~/Governanca/frmCamareiras.aspx" Text="Camareiras" 
                        Value="Camareiras"></asp:TreeNode>
                    <asp:TreeNode NavigateUrl="~/Governanca/frmCadastroAla.aspx" Text="Manutenção de Alas" 
                        Value="Alas"></asp:TreeNode>
                </Nodes>
                <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
                    HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
            </asp:TreeView>
            <asp:RoundedCornersExtender ID="mnuConfiguracao_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="mnuConfiguracao">
            </asp:RoundedCornersExtender>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
    <p>
    </p>
</asp:Content>

