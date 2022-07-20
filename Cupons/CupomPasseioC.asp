<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<%
Dim Banco,       CodBanco,      Cedente,    Reserva,    Vencimento ,   Sacado,     AgeCodCed     
Dim NossoNum,    Demonstrativo, VlrDoc,     CodigoPgto, DataDocumento, DataProces, Codigoipte    
Dim CodigoBarra, Periodo,       Entrada,    Saida,      Comando ,      QdtePess,   End1         
Dim Boleto,      Acomodacao,    Destino,    Hospedagem, UFDestino,     NomeResp,   EnderecoResp 
Dim BairroResp,  CidadeResp,    EstadoResp, LocalSaida, HoraSaida
       
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
Destino       = Request("Destino")
Hospedagem    = Request("Hospedagem")
UFDestino     = Request("UFDestino")
NomeResp      = Request("NomeResp")
EnderecoResp  = Request("EnderecoResp")
BairroResp    = Request("BairroResp")
CidadeResp    = Request("CidadeResp")
EstadoResp    = Request("EstadoResp")
LocalSaida    = Request("LocalSaida")
HoraSaida     = Request("HoraSaida")
%>

<script language="JavaScript">
    function printPage() {
        if (confirm('Clique em OK para imprimir este cupom...')) {
            window.print();
        }
        window.opener = 'X';
        window.open('', '_parent', '');
        window.close();
    }
</script>

<html>
<head>
    <title>Cobrança</title>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
    <meta content="Microsoft FrontPage 4.0" name="GENERATOR">
</head>
<form name="Boleto" action="">
<body onload="printPage()">
    <table cellspacing="0" cellpadding="0" width="640" border="0" height="23">
        <tbody>
            <tr>
                <td width="86" rowspan="2" height="9">
                    <p align="center">
                        <img border="0" src="../images/GIFSESC.gif">
                </td>
                <td width="411" height="29">
                    SERVIÇO SOCIAL DO COMÉRCIO
                </td>
                <td valign="bottom" align="right" width="143" rowspan="2" height="9">
                    <p align="center">
                    Via Caixa SESC
                </td>
            </tr>
            <tr>
                <td width="411" height="1">
                    Departamento Regional em Goiás
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="2" width="640" border="1" height="1">
        <tbody>
            <tr>
                <td align="left" width="625" height="2" colspan="3">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp; SESC Central de Reservas&nbsp;- </font><b>Não receber
                            após o vencimento</b>
                </td>
                <td align="left" width="57" height="2">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
                </td>
            </tr>
            <tr>
                <td align="left" width="130" height="1">
                    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
                </td>
                <td align="left" width="264" height="1">
                    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
                </td>
                <td align="left" width="170" height="1">
                    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
                </td>
                <td align="left" width="185" height="1">
                    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$
                    <%response.write VlrDoc%>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    &nbsp;Período&nbsp;<%response.write Periodo%>
                    <br>
                    &nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%>
                    <br>
                    &nbsp;CA:&nbsp;<%response.write Sacado%>&nbsp;<%response.write NossoNum%>
                </td>
            </tr>
            <tr>
                <td align="left" width="632" height="1" colspan="4">
                    <p align="right">
                        <font size="1">Cortar aqui...</font></p>
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="0" width="640" border="0" height="23">
        <tbody>
            <tr>
                <td width="86" rowspan="2" height="9">
                    <p align="center">
                        <img border="0" src="../images/GIFSESC.gif">
                </td>
                <td width="411" height="29">
                    SERVIÇO SOCIAL DO COMÉRCIO
                </td>
                <td valign="bottom" align="right" width="143" rowspan="2" height="9">
                    <p align="center">
                    Via Central de Reservas
                </td>
            </tr>
            <tr>
                <td width="411" height="1">
                    Departamento Regional em Goiás
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="2" width="640" border="1" height="1">
        <tbody>
            <tr>
                <td align="left" width="625" height="2" colspan="3">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp; SESC Central de Reservas&nbsp;- </font><b>Não receber
                            após o vencimento</b>
                </td>
                <td align="left" width="57" height="2">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
                </td>
            </tr>
            <tr>
                <td align="left" width="130" height="1">
                    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
                </td>
                <td align="left" width="264" height="1">
                    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
                </td>
                <td align="left" width="170" height="1">
                    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
                </td>
                <td align="left" width="185" height="1">
                    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$
                    <%response.write VlrDoc%>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" width="477" height="1" colspan="4">
                    &nbsp;Período&nbsp;<%response.write Periodo%>
                    <br>
                    &nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%>
                </td>
            </tr>
            <tr>
                <td align="left" width="632" height="1" colspan="4">
                    <p align="right">
                        <font size="1">Cortar aqui...</font></p>
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="0" width="640" border="0" height="53">
        <tbody>
            <tr>
                <td width="86" rowspan="2" height="23">
                    <p align="center">
                        <img border="0" src="../images/GIFSESC.gif">
                </td>
                <td width="411" height="29">
                    SERVIÇO SOCIAL DO COMÉRCIO
                </td>
                <td valign="bottom" align="right" width="143" rowspan="2" height="23">
                    <p align="center">
                    Via Sacado
                </td>
            </tr>
            <tr>
                <td width="411" height="1">
                    Departamento Regional em Goiás
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="2" width="640" border="1" height="1">
        <tbody>
            <tr>
                <td align="left" width="612" height="18" colspan="3">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Cedente</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp; SESC Central de Reservas</font>
                </td>
                <td align="left" width="70" height="18">
                    <sup><font face="Arial" size="2">&nbsp;</font></sup><font face="Arial"><sup>Vencimento</sup></font><font
                        face="arial" size="2">&nbsp;&nbsp;<%response.write Vencimento%></font>
                </td>
            </tr>
            <tr>
                <td align="left" width="130" height="9">
                    &nbsp;<sup>Data</sup>&nbsp;&nbsp;&nbsp;<%response.write DataDocumento%>
                </td>
                <td align="left" width="272" height="9">
                    <sup>&nbsp;Nº de Controle</sup>&nbsp;&nbsp;&nbsp;<%response.write NossoNum%>
                </td>
                <td align="left" width="149" height="9">
                    <sup>&nbsp;Nº de Pessoas</sup>&nbsp;&nbsp;&nbsp;<%response.write QdtePess%>
                </td>
                <td align="left" width="198" height="9">
                    <sup>&nbsp;Valor</sup>&nbsp;&nbsp;&nbsp;R$
                    <%response.write VlrDoc%>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    &nbsp;Período&nbsp;<%response.write Periodo%>
                    <br>
                    &nbsp;Sacado&nbsp;&nbsp;<%response.write Sacado%>
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="2" width="640" border="1" height="295">
        <tbody>
            <tr>
                <td align="left" width="100%" height="88" colspan="2">
                    <font face="Arial" size="2">Demonstrativo Turismo Social - Passeio Nº:
                        <%response.write Reserva%>&nbsp;/&nbsp;<%response.write QdtePess%>
                        Integrante(s) <font face="arial" size="2">
                            <br>
                            Passeio:
                            <%response.write NomeResp%>
                            <br>
                            Destino:
                            <%response.write Destino%></font>
                        <br>
                        <p>
                            <font face="arial" size="2">Saída:
                                <%response.write LocalSaida%>&nbsp;&nbsp;Chegada:<%response.write HoraSaida%>
                                h</font>
                        </p>
                    </font>
                </td>
            </tr>
            <tr>
                <td align="left" width="100%" height="56" colspan="2">
                    <font face="arial" size="2">
                        <center>
                            <b>ATENÇÃO - Valores e serviços sujeitos a alteração.</b></center>
                        <br>
                        Caso não ocorra o pagamento deste cupom até o vencimento, a inscrição no PASSEIO
                        será <b>cancelada automaticamente</b>.
                        <br />
                        <span><span lang="pt-br">No </span>momento&nbsp; do&nbsp; embarque é obrigatório apresentação
                            dos sequintes documentos: Identidade para adulto e certidão de nascimento para criança.<span
                                lang="pt-br"> Não será&nbsp; permitido o embarque sem&nbsp;a documentação.</span></span><br>
                        O passageiro poderá ser substituído para fins de reembolso completo, com até 5 dias
                        de antecedência ao embarque, desde que o novo passageiro indicado seja enquadrado
                        na mesma categoria.
                        <br>
                        No prazo inferior a 5 dias do embarque o Passeio passa a ser intransferível, vigorando
                        as normas de cancelamento abaixo.
                        <br>
                        Quando do não comparecimento ao embarque, não haverá reembolso.
                        <br>
                        O cancelamento do Passeio, encaminhado por escrito à Central de Reservas, obedecerá
                        o seguinte critério de reembolso: </font>
                </td>
            </tr>
            <tr>
                <td align="left" width="50%" height="56">
                    <font face="arial" size="1">
                        <center>
                            CANCELAMENTO EFETUADO:</center>
                        <br>
                        Até 10 dias antes do embarque
                        <br>
                        De 9 a 5 dias antes do embarque
                        <br>
                        Menos de 5 dias antes do embarque </font>
                </td>
                <td align="left" width="50%" height="56">
                    <font face="arial" size="1">
                        <center>
                            PORCENTAGEM DE REEMBOLSO</center>
                        <br>
                        <center>
                            80 %
                            <br>
                            50 %
                            <br>
                            sem reembolso</center>
                    </font>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%" height="6" colspan="2">
                    <p align="center">
                        <sup>Contato Central de Reservas: reservas.caldasnovas@sescgo.com.br </sup>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</form>
</html>
