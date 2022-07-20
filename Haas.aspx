<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Haas.aspx.vb" Inherits="Haas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <!DOCTYPE HTML>
        <html>
        <head>
            <style type="text/css">
                #div1, #div2
                {
                    float: left;
                    width: 100px;
                    height: 35px;
                    margin: 10px;
                    padding: 10px;
                    border: 1px solid #aaaaaa;
                }
            </style>

            <script>
                function allowDrop(ev) {
                    ev.preventDefault();
                }

                function drag(ev) {
                    ev.dataTransfer.setData("Text", ev.target.id);
                }

                function drop(ev) {
                    ev.preventDefault();
                    var data = ev.dataTransfer.getData("Text");
                    alert(data);
                    if (ev.target.id == 'div1') 
                    {
                    alert('div1');
                    }
                    else
                    {
                    alert('div2');
                    }
                    
                    ev.target.appendChild(document.getElementById(data));
                    alert(ev.target.id);
                }
</script>
</head>
        <body>
            <div id="div1" ondrop="drop(event)" ondragover="allowDrop(event)" align="center">
                <img src="images/HospedeAzul.png" draggable="true" ondragstart="drag(event)" id="Azul"
                    height="31" align="absmiddle" title="Sérgio">
                <img src="images/HospedeVerde.png" draggable="true" ondragstart="drag(event)" id="Verde"
                    height="31" align="absmiddle">
                <asp:LinkButton ondragstart="drag(event)" ID="LinkButton1" runat="server"><img src="images/HospedeAzul.png" />Haas</asp:LinkButton>
            </div>
            <div id="div2" ondrop="drop(event)" ondragover="allowDrop(event)" align="center">
                <asp:LinkButton ID="LinkButton3" runat="server" draggable="true" ondragstart="drag(event)" CommandName="HAAS">LinkButton</asp:LinkButton>
            </div>
        </body>
        </html>
    </div>
    </form>
</body>
</html>
