<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<%
Dim             Banco,        CodBanco,      Cedente,    Reserva,       Vencimento
Dim             Sacado,       AgeCodCed,     NossoNum,   Demonstrativo, VlrDoc     
Dim             CodigoPgto,   DataDocumento, DataProces, Codigoipte,    CodigoBarra
Dim             Periodo,      Entrada,       Saida,      Comando,       Texto  
Dim             QdtePess,     End1,          Boleto,     Acomodacao,    Percentual
Dim             ValorReserva, ValorAux 
       
Banco         = Request("Banco")
CodBanco      = Request("CodBanco")
Cedente       = Request("Cedente")
Reserva       = Request("Reserva")
Vencimento    = Request("Vencimento")
Sacado        = Request("Sacado")
AgeCodCed     = Request("AgeCodCed")
NossoNum      = Request("NossoNum")
Demonstrativo = Request("Demonstrativo") 
VlrDoc        = Request("VlrDoc")
DataDocumento = Request("DataDocumento")
DataProces    = Request("DataProces")
Codigoipte    = Request("Codigoipte")
CodigoBarra   = Request("CodigoBarra")
End1          = Request("End1")
Periodo       = Request("Periodo")
Entrada       = Request("Entrada")
Saida         = Request("Saida")
Comando       = Request("Comando")
QdtePess      = Request("QdtePess")
Boleto        = Request("Boleto")
Acomodacao    = Request("Acomodacao")
Percentual    = Request("Percentual")
Percentual    = Request("Percentual")

ValorAux     = replace(VlrDoc, "." , "")

%>

<SCRIPT LANGUAGE="JavaScript">
function printPage() {
  if (confirm('Clique em OK para imprimir este cupom...'))
  {
    window.print();
  }
  window.opener='X';
  window.open('','_parent','');
  window.close();
}
</script>

<HTML><HEAD>
<TITLE>Cupom de Cobran�a</TITLE>
<META http-equiv=Content-Type content="text/html; charset=ISO-8859-1">
<META content="Microsoft FrontPage 4.0" name=GENERATOR></HEAD>
<form name=Boleto action="">
<BODY OnLoad=printPage()>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=0 bordercolor="#C0C0C0" height="23">
  <TBODY>
  <TR>
    <TD width=86 rowspan="2" height="9">
      <p align="center"><img border="0" src="../images/GIFSESC.gif"></TD>
    <TD width=411 height="29">SERVI�O SOCIAL DO COM�RCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="9">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goi�s</TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 bordercolor="#C0C0C0" height="1">
  <TBODY>
  <TR>
    <TD align=left width="625" height="2" colspan="3">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font face="arial" size="2">&nbsp;&nbsp;
      SESC Central de Reservas&nbsp;- </font><b>N�o receber ap�s o vencimento</b>
    </TD>
    <TD align=left width="57" height="2">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
    </TD>
  </TR>
  <TR>
    <TD align=left width="130" height="1">
    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
    </TD>
    <TD align=left width="264" height="1">
    <sup>&nbsp;N� de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="170" height="1">
    <sup>&nbsp;N� de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="185" height="1">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="477" height="1" colspan="3">
    &nbsp;Per�odo&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Sa�da&nbsp;<%response.write Saida%>
    <BR>&nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%></TD>
    <TD align=left width="217" height="1">
    <sup>Rateio (Uso SESC)&nbsp;&nbsp;<BR><center><%response.write Banco%></center></sup></TD>
  </TR>
  <TR>
    <TD align=left width="740" height="1" colspan="4"><font size="2"><%response.write Acomodacao%></font></TD>
  </TR>
  <TR>
    <TD align=left width="632" height="1" colspan="4">
    <font size="1"><sup>&nbsp;1� Via - CEREC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Autentica��o Mec�nica</sup></font>
    <p align="right"><font size="1">Cortar aqui...</font></p>
    </TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=0 bordercolor="#C0C0C0" height="41">
  <TBODY>
  <TR>
    <TD width=86 rowspan="2" height="11">
      <p align="center"><img border="0" src="../images/GIFSESC.gif"></TD>
    <TD width=411 height="29">SERVI�O SOCIAL DO COM�RCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="11">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goi�s</TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 bordercolor="#C0C0C0" height="75">
  <TBODY>
  <TR>
    <TD align=left width="618" height="21" colspan="3">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font face="arial" size="2">&nbsp;&nbsp;
      SESC Central de Reservas</font>
    </TD>
    <TD align=left width="64" height="21">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
    </TD>
  </TR>
  <TR>
    <TD align=left width="130" height="20">
    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
    </TD>
    <TD align=left width="268" height="20">
    <sup>&nbsp;N� de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="159" height="20">
    <sup>&nbsp;N� de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="192" height="20">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="470" height="1" colspan="3">
    &nbsp;Per�odo&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Sa�da&nbsp;<%response.write Saida%>
    <BR>&nbsp;Sacado&nbsp;&nbsp<%response.write Sacado%>
    <br>
    &nbsp;CA:&nbsp;<%response.write Sacado%>&nbsp;<%response.write NossoNum%></TD>
    <TD align=left width="224" height="1">
    <sup>Rateio (Uso SESC)&nbsp;&nbsp;<BR><center><%response.write Banco%></center></sup></TD>
  </TR>
  <TR>
    <TD align=left width="740" height="1" colspan="4"><font size="2"><%response.write Acomodacao%></font></TD>
  </TR>
  <TR>
    <TD align=left width="632" height="30" colspan="4">
    <font size="1"><sup>&nbsp;2� Via - Caixa&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Autentica��o Mec�nica</sup></font>
    <p align="right"><font size="1">Cortar aqui...</font></p>
    </TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=0 bordercolor="#C0C0C0" height="41">
  <TBODY>
  <TR>
    <TD width=86 rowspan="2" height="11">
      <p align="center"><img border="0" src="../images/GIFSESC.gif"></TD>
    <TD width=411 height="29">SERVI�O SOCIAL DO COM�RCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="11">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goi�s</TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 bordercolor="#C0C0C0">
  <TBODY>
  <TR>
    <TD align=left width="618" height="21" colspan="3">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font face="arial" size="2">&nbsp;&nbsp;
      SESC Central de Reservas</font>
    </TD>
    <TD align=left width="64" height="21">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
    </TD>
  </TR>
  <TR>
    <TD align=left width="130" height="20">
    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
    </TD>
    <TD align=left width="268" height="20">
    <sup>&nbsp;N� de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="159" height="20">
    <sup>&nbsp;N� de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="192" height="20">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="470" height="1" colspan="3">
    &nbsp;Per�odo&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Sa�da&nbsp;<%response.write Saida%>
    <BR>&nbsp;Sacado&nbsp;&nbsp<%response.write Sacado%></TD>
    <TD align=left width="224" height="1">
    <sup>Rateio (Uso SESC)&nbsp;&nbsp;<BR><center><%response.write Banco%></center></sup></TD>
  </TR>
  <TR>
    <TD align=left width="640" height="1" colspan="4"><font size="2"><%response.write Acomodacao%></font></TD>
  </TR>

  <TR>
    <TD align=left width="640" height="28" colspan="4">
      <p align="center">
      <FONT face=arial size=2><b><i>As di�rias de hospedagem na Pousada SESC Piren�polis incluem apenas caf� da manh�.</center></i></b>
      </FONT>
    </TD>
  </TR>
  <TR>
    <TD align=left width="640" height="1" colspan="4">
      <center><b><font face="arial" size="3">ATEN��O</font></b></center><b><font face="arial" size="2"></font></b>
  	  <br><font face="arial" size="2">
    &nbsp;RESERVA N� <%response.write Reserva%> de <%response.write Periodo%>                       
    <BR>&nbsp;<b>SESC Piren�polis</b>-<%response.write Acomodacao%>
    <BR>&nbsp;<%response.write QdtePess%> Hospede(s) - Entrada �s 14h e sa�da �s 10h.
    <BR>&nbsp;Obrigat�ria a apresenta��o deste boleto e das carteiras do Sesc atualizadas, na chegada.
    <BR>&nbsp;A constata��o de discrep�ncia gerar� altera��o do valor e cobran�a da diferen�a.
    </font>    
    <BR>
    <BR>
  </TR>
</TABLE>
<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 colspan="4">         
  <TR>
    <TD align=center width="100%" colspan="4">
      <p align="center"><sup>Contato Central de Reservas: reservas.pirenopolis@sescgo.com.br</sup>
    </TD>
  </TR>
  </TBODY>
</TABLE>
<font size="1">
3� Via - Sacado&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    Autentica��o Mec�nica</font>
</BODY>
</form>
</HTML>