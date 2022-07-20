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

ValorAux     = replace(VlrDoc, "." , "")
ValorReserva = FormatCurrency(FormatCurrency(((FormatCurrency(ValorAux,2) * 100) / Percentual),2),2)
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
<TITLE>Cupom de Cobrança</TITLE>
<META http-equiv=Content-Type content="text/html; charset=ISO-8859-1">
<META content="Microsoft FrontPage 4.0" name=GENERATOR></HEAD>
<form name=Boleto action="">
<BODY OnLoad=printPage()>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=0 bordercolor="#C0C0C0" height="23">
  <TBODY>
  <TR>
    <TD width=86 rowspan="2" height="9">
      <p align="center"><img border="0" src="../images/GIFSESC.gif"></TD>
    <TD width=411 height="29">SERVIÇO SOCIAL DO COMÉRCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="9">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goiás</TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 bordercolor="#C0C0C0" height="1">
  <TBODY>
  <TR>
    <TD align=left width="625" height="2" colspan="3">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font face="arial" size="2">&nbsp;&nbsp;
      SESC Central de Reservas&nbsp;- </font><b>Não receber após o vencimento</b>
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
    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="170" height="1">
    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="185" height="1">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="477" height="1" colspan="3">
    &nbsp;Período&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Saída&nbsp;<%response.write Saida%>
    <BR>&nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%></TD>
    <TD align=left width="217" height="1">
    <sup>Rateio (Uso SESC)&nbsp;&nbsp;<BR><center><%response.write Banco%></center></sup></TD>
  </TR>
  <TR>
    <TD align=left width="740" height="1" colspan="4"><font size="2"><%response.write Acomodacao%></font></TD>
  </TR>
  <TR>
    <TD align=left width="632" height="1" colspan="4">
    <font size="1"><sup>&nbsp;1ª Via - CEREC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Autenticação Mecânica</sup></font>
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
    <TD width=411 height="29">SERVIÇO SOCIAL DO COMÉRCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="11">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goiás</TD>
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
    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="159" height="20">
    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="192" height="20">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="470" height="1" colspan="3">
    &nbsp;Período&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Saída&nbsp;<%response.write Saida%>
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
    <font size="1"><sup>&nbsp;2ª Via - Caixa&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Autenticação Mecânica</sup></font>
    <p align="right"><font size="1">Cortar aqui...</font></p>
    </TD>
  </TR>
  </TBODY>
</TABLE>
<TABLE cellSpacing=0 cellPadding=0 width=640 border=0 bordercolor="#C0C0C0" height="53">
  <TBODY>
  <TR>
    <TD width=86 rowspan="2" height="23">
      <p align="center"><img border="0" src="../images/GIFSESC.gif"></TD>
    <TD width=411 height="29">SERVIÇO SOCIAL DO COMÉRCIO</TD>
    <TD vAlign=bottom align=right width=143 rowspan="2" height="23">
      <p align="center">Cupom de Reserva</TD>
  </TR>
  <TR>
    <TD width=411 height="1">Departamento Regional em Goiás</TD>
  </TR>
  </TBODY>
</TABLE>

<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 bordercolor="#C0C0C0" height="1">
  <TBODY>
  <TR>
    <TD align=left width="612" height="18" colspan="3">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font face="arial" size="2">&nbsp;&nbsp;
      SESC Central de Reservas</font>
    </TD>
    <TD align=left width="70" height="18">
      <sup>
      <font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
    </TD>
  </TR>
  <TR>
    <TD align=left width="130" height="9">
    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
    </TD>
    <TD align=left width="272" height="9">
    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
    </TD>
    <TD align=left width="149" height="9">
    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
    </TD>
    <TD align=left width="198" height="9">
    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$ <%response.write VlrDoc%> *
    </TD>
  </TR>
  <TR>
    <TD align=left width="464" height="1" colspan="3">
    &nbsp;Período&nbsp;<%response.write Periodo%>
    &nbsp;Entrada&nbsp;<%response.write Entrada%>
    &nbsp;&nbsp;Saída&nbsp;<%response.write Saida%>
    <BR>&nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%></TD>
    <TD align=left width="230" height="1">
    <% if Percentual = 30 then 
      Texto = "<p>Atenção! O valor do cupom corresponde ao sinal de 30% do valor da diária pelo preço praticado no dia da reserva.</p>" 
      else 
      Texto = ""
    end if%>
    <font size="1"><%response.write Texto%></font>
	</TD>
  </TR>
  <TR>
    <TD align=left width="740" height="1" colspan="4"><font size="2"><%response.write Acomodacao%></font></TD>
  </TR>
  <TR>
    <TD align=left width="632" height="28" colspan="4">
      <p align="center">
      <FONT face=arial size=2><b><i>As diárias no SESC Caldas Novas incluem entrada com almoço e jantar e saída com café da manhã.</center></i></b>
      </FONT>
    </TD>
  </TR>
  <TR>
    <TD align=left width="632" height="1" colspan="4">
      <center><b><font face="arial" size="3">ATENÇÃO</font></b></center><b><font face="arial" size="2">(*) Valores e serviços sujeitos a alteração.</font></b>
  	  <br><font face="arial" size="1">
    O Valor deste Documento corresponde a <%response.write Percentual%>% do valor de <%response.write ValorReserva%> <sup>(*)</sup> da reserva de <%response.write QdtePess%> hóspede(s).
    <BR>O não pagamento do boleto na data do vencimento levará ao cancelamento automático da reserva.
    <BR>É obrigatório o pagamento total da reserva até 30 dias antes do início da estada.
    <BR>Na portaria do SESC Caldas Novas é necessária a apresentação de documento de identidade e Carteira do SESC atualizada.
    <BR>O SESC não se responsabiliza por objetos/valores deixados no apartamento e áreas comuns da unidade.
    <BR>Após 24 horas do início da estada não havendo comparecimento e sem comunicação oficial a reserva será cancelada e não haverá restituição do valor pago. 
    <BR>O cancelamento da reserva deverá ser feito por escrito, encaminhado  à Central de Reservas e obedecerá ao seguinte critério de reembolso, exceto em caso de falecimento comprovado do titular  ou de pai, mãe, cônjuge ou filhos, que será de 100% do valor pago:</font></TD>
  </TR>
</TABLE>
<TABLE cellSpacing=0 cellPadding=0 width=640 border=1 colspan="4">
    <tr>
                <td valign="top" width="38%">
                    <font face="arial" size="1">CANCELAMENTO EFETUADO
                        <br>
                        Até 20 dias antes do início da estada
                        <br>
                        Até 10 dias antes do início da estada
                        <br>
                        Até 05 dias antes do início da estada</font></td>
                <td valign="top" width="12%">
                    <font face="arial" size="1">REEMBOLSO
                        <br>
                        <span lang="pt-br">80</span> %
                        <br>
                        6<span lang="pt-br">0</span> %
                        <br>
                        50 %
                        </font>
                </td>
                    <td valign="top" width="25%">
                <font face="arial" size="1">
                        Sem restituição se o pedido de cancelamento for feito após (5) dias corridos antes
                        do início da estada.
                </font>
                    </td>
                    <td valign="top" width="25%">
                <font face="arial" size="1">
                        Sem restituição caso não haja comparecimento na Unidade de Hospedagem sem comunicação
                        prévia, por escrito e/ou interrupção da estada.
                
                    
                    </font>
                </td>
            </tr>
  <TR>
    <TD align=center width="100%" colspan="4">
      <p align="center"><sup>Contato Central de Reservas: reservas.caldasnovas@sescgo.com.br </sup>
    </TD>
  </TR>
  </TBODY>
</TABLE>
<font size="1">
3ª Via - Sacado&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    Autenticação Mecânica</font>
</BODY>
</form>
</HTML>