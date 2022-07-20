<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RelacaoIntegrantesPasseio.aspx.vb" Inherits="RelacaoIntegrantesPasseio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="rptVwrRelacaoIntegrantesPasseio" runat="server" DocumentMapWidth="100%"
            Height="450px" ProcessingMode="Remote" Width="100%" Font-Names="Verdana" Font-Size="8pt">
            <ServerReport ReportPath="/RptPasseio/rptRelacaoIntegrantes" 
                ReportServerUrl="http://server-bd-bkp/ReportServer_SQL2008" />
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
