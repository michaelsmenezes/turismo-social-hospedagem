function ValidaIdade(source, arguments) {
    dia1 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtDataInicialSolicitacao.value.substr(0, 2);
    mes1 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtDataInicialSolicitacao.value.substr(3, 2);
    ano1 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtDataInicialSolicitacao.value.substr(6, 4);
    dia2 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtIntNascimento.value.substr(0, 2);
    mes2 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtIntNascimento.value.substr(3, 2);
    ano2 = document.aspnetForm.ctl00$conPlaHolTurismoSocial$txtIntNascimento.value.substr(6, 4);
    periodo1 = new Date(ano1, mes1 - 1, dia1);
    periodo2 = new Date(ano2, mes2 - 1, dia2);
    timeDifference = (periodo1 - periodo2);
    document.aspnetForm.ctl00$conPlaHolTurismoSocial$hddIdade.value = (timeDifference / (1000 * 60 * 60 * 24 * 365.26));
    //se o hddRescaracteristica for igual P(passeio) pega o if de baixo
    //        if (document.aspnetForm.ctl00$conPlaHolTurismoSocial$hddResCaracteristica.value == 'P')
    //       {
    //          if (((timeDifference / (1000 * 60 * 60 * 24 * 365.26)) < 5) && (document.aspnetForm.ctl00$conPlaHolTurismoSocial$cmbIntVinculoId.value == "0")) { arguments.IsValid = false; return false; } else { arguments.IsValid = true; return true; }
    //       }
}

function validaCPF(source, arguments) {
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    var cpf = arguments.Value;
    cpf = cpf.split(" ").join("");
    digitos_iguais = 1;
    //    alert(cpf);

    if (cpf.length < 11) {
        arguments.IsValid = false;
        return false;
    }
    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) {
            arguments.IsValid = false;
            return false;
        }
        numeros = cpf.substring(0, 10);
        soma = 0;
        for (i = 11; i > 1; i--)
            soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1)) {
            arguments.IsValid = false;
            return false;
        }
        arguments.IsValid = true;
        return true;
    }
    else {
        arguments.IsValid = false;
        return false;
    }
}

//Diversas formatações em uma só função
function formatarGeral(mascara, valor) {
    var i = valor.value.length;
    var saida = mascara.substring(0, 1);
    var texto = mascara.substring(i)

    if (texto.substring(0, 1) != saida) {
        valor.value += texto.substring(0, 1);
    }
}

function desabilitaEnter() {
    if (window.event.keyCode == 13) { window.event.keyCode = 0; event.returnValue = false; };
}

function abrir(URL, Topo, Esquerda, Tamanho, Altura) {
    window.open(URL, '',
	'top=' + Topo +
	',left=' + Esquerda +
	',width=' + Tamanho +
	',height=' + Altura +
	',resizable=yes,toolbar=no,status=yes,menubar=no,location=no,scrollbars=yes,dependet=no');
}
//function SoNumero(formfield) {
function SoNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

//var number = formfield.replace('.', '').replace('-', '').replace('/', '').replace(' ', '');
//var output = '';
//for (i = number.length; i > 0; i--) {
//    if ((number.substring(i, i - 1) == '0') ||
//        (number.substring(i, i - 1) == '1') ||
//        (number.substring(i, i - 1) == '2') ||
//        (number.substring(i, i - 1) == '3') ||
//        (number.substring(i, i - 1) == '4') ||
//        (number.substring(i, i - 1) == '5') ||
//        (number.substring(i, i - 1) == '6') ||
//        (number.substring(i, i - 1) == '7') ||
//        (number.substring(i, i - 1) == '8') ||
//        (number.substring(i, i - 1) == '9'))
//        output = number.substring(i, i - 1) + output;
//}
//formfield.value = output;
//return output;
//}


function FormataData(inputData, e) {
    var tecla;
    if (document.all) // Internet Explorer
        tecla = event.keyCode;
    else //Outros Browsers
        tecla = e.which;

    if (tecla >= 47 && tecla < 58) { // numeros de 0 a 9 e '/'
        var data = inputData.value;

        //se for um numero coloca no input
        if (tecla > 47 && tecla < 58) {
            if (data.length == 2 || data.length == 5) {
                data += '/';

            }
        } else if (tecla == 47) { //se for a barra, so deixa colocar se estiver na posicao certa
            if (data.length != 2 && data.length != 5) {
                return false;
            }
        }
        //atualiza o input da data
        inputData.value = data;
        return true;

    } else if (tecla == 8 || tecla == 0) // Backspace, Delete e setas direcionais(para mover o cursor, apenas para FF)
        return true;
    else
        return false;
}

//como chamar: txtResMatricula.Attributes.Add("onKeyUp", "javascript:this.value=FormataMatricula(this.value,event);")
function FormataMatricula(campo, e) {//Esta função formata Matricula do SESC, esta preparada para inserção manual e leitor de código de barras  
    //SoNumero(event);
    vr = campo.replace('.', '').replace('-', '').replace('/', '').replace(' ', '');
    tam = vr.length;
    if ((e.keyCode || e.which) != 9) {
        if (tam <= 4)
        { valorcampo = vr.substr(0, 4); }
        else if (tam > 4 && tam < 10)
        { valorcampo = vr.substr(0, 4) + ' ' + vr.substr(4, 6); }
        else if (tam == 10)
        { valorcampo = vr.substr(0, 4) + ' ' + vr.substr(4, 6) + ' ' + vr.substr(10, 1); }
        else if (tam > 11) {
            key = 0;
            valorcampo = vr.substr(1, 4) + ' ' + vr.replace(' ', "").substr(5, 6) + ' ' + vr.replace(' ', "").substr(11, 1);
        }
    }
    //Tive que adicionar essa parte, pois no firefox da erro se não tiver esse código.
    if (e.keyCode == 9) {
        return campo;
    }
    else
        return valorcampo;
}

function FormataCPF(campo, e) {
    //SoNumero(event);
    vr = campo.replace('.', '').replace('-', '').replace('/', '').replace(' ', '');
    tam = vr.length;
    if ((e.keyCode || e.which) != 9) {
        if (tam <= 3)
        { valorcampo = vr.substr(0, 3); }
        if (tam > 3 && tam < 7)
        { valorcampo = vr.substr(0, 3) + ' ' + vr.replace(' ', '').substr(3, 3); }
        else if (tam > 6 && tam < 10)
        { valorcampo = vr.substr(0, 3) + ' ' + vr.replace(' ', '').substr(3, 3) + ' ' + vr.replace(' ', '').substr(6, 3); }
        else if (tam > 9 && tam < 13)
        { valorcampo = vr.substr(0, 3) + ' ' + vr.replace(' ', '').substr(3, 3) + ' ' + vr.replace(' ', '').substr(6, 3) + ' ' + vr.replace(' ', '').substr(10, 2); }
        //return valorcampo;
    }
    if (e.keyCode == 9) {
        return campo;
    }
    else
        return valorcampo;
}

function mascaracep(campo, e) {
    vr = campo.replace('.', '').replace('-', '').replace('/', '').replace(' ', '');
    var valorcampo = campo;
    tam = vr.length;
    if ((e.keyCode || e.which) != 9) {
        if (tam < 6)
        { valorcampo = vr.substr(0, 5) + ' '; }
        else if (tam > 6 && tam < 10)
        { valorcampo = vr.substr(0, 5) + ' ' + vr.substr(5, 4); }
    }
    if (e.keyCode == 9) {
        return campo.substr(0,8);
    }
    else
        return valorcampo.substr(0,8);
}

function ValidaData(objName, branco) {
    var strDate;
    var strDateArray;
    var strDay;
    var strMonth;
    var strYear;
    var intday;
    var intMonth;
    var intYear;
    var booFound = false;
    var datefield = objName;
    var strSeparatorArray = new Array("-", " ", "/", ".");
    var intElementNr;
    strDate = datefield.value;
    if ((branco == "S") && (strDate.length < 1)) {
        return true;
    }
    if ((strDate.length < 10) || (strDate.length > 10)) {
        return false
    }
    for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) {
        if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) {
            strDateArray = strDate.split(strSeparatorArray[intElementNr]);
            if (strDateArray.length != 3) {
                return false;
            }
            else {
                strDay = strDateArray[0];
                strMonth = strDateArray[1];
                strYear = strDateArray[2];
            }
            booFound = true;
        }
    }
    if (booFound == false) {
        if (strDate.length > 5) {
            strDay = strDate.substr(0, 2);
            strMonth = strDate.substr(2, 2);
            strYear = strDate.substr(4);
        }
    }
    if (strYear.length == 2) {
        strYear = '20' + strYear;
    }
    if (strYear.length == 3) {
        strYear = '2' + strYear;
    }
    if (strMonth.length == 1) {
        strMonth = '0' + strMonth;
    }
    if (strDay.length == 1) {
        strDay = '0' + strDay;
    }
    intday = parseInt(strDay, 10);
    if (isNaN(strDay)) {
        return false;
    }
    intMonth = parseInt(strMonth, 10);
    if (isNaN(strMonth)) {
        return false;
    }
    intYear = parseInt(strYear, 10);
    if (isNaN(strYear)) {
        return false;
    }
    if (intMonth > 12 || intMonth < 1) {
        return false;
    }
    if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
        return false;
    }
    if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)) {
        return false;
    }
    if (intMonth == 2) {
        if (intday < 1) {
            return false;
        }
        if (LeapYear(intYear) == true) {
            if (intday > 29) {
                return false;
            }
        }
        else {
            if (intday > 28) {
                return false;
            }
        }
    }
    datefield.value = strDay + "/" + strMonth + "/" + strYear;
    return true;
}

function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) {
            return true;
        }
    }
    else {
        if ((intYear % 4) == 0) {
            return true;
        }
    }
    return false;
}

function SomarData(txtData, DiasAdd) {
    // Tratamento das Variaveis.
    // var txtData = "01/01/2007"; //poder ser qualquer outra
    // var DiasAdd = 10 // Aqui vem quantos dias você quer adicionar a data
    var d = new Date();
    // Aqui eu "mudo" a configuração de datas.
    // Crio um obj Date e pego o campo txtData e 
    // "recorto" ela com o split("/") e depois dou um
    // reverse() para deixar ela em padrão americanos YYYY/MM/DD
    // e logo em seguida eu coloco as barras "/" com o join("/")
    // depois, em milisegundos, eu multiplico um dia (86400000 milisegundos)
    // pelo número de dias que quero somar a txtData.
    d.setTime(Date.parse(txtData.split("/").reverse().join("/")) + (86400000 * (DiasAdd)))

    // Crio a var da DataFinal 
    var DataFinal;

    // Aqui comparo o dia no objeto d.getDate() e vejo se é menor que dia 10. 
    if (d.getDate() < 10) {
        // Se o dia for menor que 10 eu coloca o zero no inicio
        // e depois transformo em string com o toString()
        // para o zero ser reconhecido como uma string e não
        // como um número.
        DataFinal = "0" + d.getDate().toString();
    }
    else {
        // Aqui a mesma coisa, porém se a data for maior do que 10
        // não tenho necessidade de colocar um zero na frente.
        DataFinal = d.getDate().toString();
    }

    // Aqui, já com a soma do mês, vejo se é menor do que 10
    // se for coloco o zero ou não.
    if ((d.getMonth() + 1) < 10) {
        DataFinal += "/0" + (d.getMonth() + 1).toString() + "/" + d.getFullYear().toString();
    }
    else {
        DataFinal += "/" + ((d.getMonth() + 1).toString()) + "/" + d.getFullYear().toString();
    }
    return (DataFinal)
}

function ComparaData(data1, data2) {
    dia1 = data1.substr(0, 2);
    mes1 = data1.substr(3, 2);
    ano1 = data1.substr(6, 4);
    dia2 = data2.substr(0, 2);
    mes2 = data2.substr(3, 2);
    ano2 = data2.substr(6, 4);
    periodo1 = new Date(ano1, mes1 - 1, dia1);
    periodo2 = new Date(ano2, mes2 - 1, dia2);
    timeDifference = (periodo1 - periodo2);
    return (timeDifference / 86400000);
}

function ColocaVirgula(formfield) {
    var number = formfield.value;
    var output = '';
    for (i = number.length; i > 0; i--) {
        if ((number.substring(i, i - 1) == '0') ||
            (number.substring(i, i - 1) == '1') ||
            (number.substring(i, i - 1) == '2') ||
            (number.substring(i, i - 1) == '3') ||
            (number.substring(i, i - 1) == '4') ||
            (number.substring(i, i - 1) == '5') ||
            (number.substring(i, i - 1) == '6') ||
            (number.substring(i, i - 1) == '7') ||
            (number.substring(i, i - 1) == '8') ||
            (number.substring(i, i - 1) == '9'))
            output = number.substring(i, i - 1) + output;
    }

    while (output.length < 3)
        output = '0' + output;

    var decimal = output.substring(output.length - 2, output.length);
    var aux = output.substring(0, output.length - 2);
    for (i = 0; i < aux.length; i++) {
        if ((aux.substring(i, i + 1) == '0') && (aux.length != 1))
            aux = aux.substring(i + 1, aux.length);
        else
            i = aux.length;
    }
    var inteiro = aux.toString();
    if (inteiro.length > 3) {
        var mod = inteiro.length % 3;
        output = (mod > 0 ? (inteiro.substring(0, mod)) : '');
        for (i = 0; i < Math.floor(inteiro.length / 3) ; i++) {
            if ((mod == 0) && (i == 0))
                output += inteiro.substring(mod + 3 * i, mod + 3 * i + 3);
            else
                output += '.' + inteiro.substring(mod + 3 * i, mod + 3 * i + 3);
        }
        output = output + ',' + decimal;
    }
    else
        output = inteiro + ',' + decimal;
    return output;
}
function limitaTamanhoCampo(campo, tamanho) {
    if (campo.value.length >= tamanho) {
        alert('Este campo nao pode ultrapassar ' + tamanho + ' caracteres.');
        window.event.keyCode = 0;
    }
    else
        return true;
}
function SomenteNumeros() {
    if ((window.event.keyCode >= 48 && window.event.keyCode <= 57) || window.event.keyCode == 13)
        return true
    else
        window.event.keyCode = 0;
}

function verificaCPF(passaCPF) {
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    var cpf = passaCPF;

    //retirando os espaços em brando do CPF
    var CPFLimpo = passaCPF;
    for (i = 0; i < 3; i++) {
        CPFLimpo = CPFLimpo.replace(" ", "");
        CPFLimpo = CPFLimpo.replace("-", "");
        CPFLimpo = CPFLimpo.replace(".", "");
    }

    cpf = CPFLimpo;
    digitos_iguais = 1;
    //    alert(cpf);

    if (cpf.length < 11) {
        arguments.IsValid = false;
        return false;
    }
    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) {
            arguments.IsValid = false;
            alert('CPF Inválido!');
            return false;
        }
        numeros = cpf.substring(0, 10);
        soma = 0;
        for (i = 11; i > 1; i--)
            soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1)) {
            arguments.IsValid = false;
            alert('CPF Inválido!');
            return false;
        }
        arguments.IsValid = true;
        return true;
    }
    else {
        arguments.IsValid = false;
        alert('CPF Inválido!');
        return false;
    }
}
/////////////////////////////////////////////////////////////////////////
function ConverteMaiusculo(formfield) {
    formfield.value = formfield.value.toUpperCase()
}

function naoColarTexto() {
    var Texto = 'Não é permitido colar a matrícula!';
    alert(unescape(Texto));
    return false;
}
//////////////////Mascara para telefone
/* Máscaras ER --> chamando txtDadosTelCelular.Attributes.Add("onkeyup", "javascript:mascaratelefone(this,mtel)") */
function mascaratelefone(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value)
}
function mtel(v) {
    v = v.replace(/\D/g, "");             //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2"); //Coloca parênteses em volta dos dois primeiros dígitos
    v = v.replace(/(\d)(\d{4})$/, "$1-$2");    //Coloca hífen entre o quarto e o quinto dígitos
    return v;
}
function id(el) {
    return document.getElementById(el);
}


//////////////////VALIDADOR DE CPF E CNPJ NUMA MESMA FUNÇÃO///////////////////
/*
 verifica_cpf_cnpj
 
 Verifica se é CPF ou CNPJ
 
 @see http://www.todoespacoonline.com/w/
*/
function verifica_cpf_cnpj(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');

    // Verifica CPF
    if (valor.length === 11) {
        return 'CPF';
    }

        // Verifica CNPJ
    else if (valor.length === 14) {
        return 'CNPJ';
    }
 
        // Não retorna nada
    else {
        return false;
    }

} // verifica_cpf_cnpj

/*
 calc_digitos_posicoes
 
 Multiplica dígitos vezes posições
 
 @param string digitos Os digitos desejados
 @param string posicoes A posição que vai iniciar a regressão
 @param string soma_digitos A soma das multiplicações entre posições e dígitos
 @return string Os dígitos enviados concatenados com o último dígito
*/
function calc_digitos_posicoes(digitos, posicoes, soma_digitos) {
    soma_digitos = 0;
    // Garante que o valor é uma string
    digitos = digitos.toString();

    // Faz a soma dos dígitos com a posição
    // Ex. para 10 posições:
    //   7    7    4    1    9    2    0    0   1   4
    //  x2  x10   x9   x8   x7   x6   x5   x4   x3  x2
    //  14 + 70 + 36 +  8 +  63 +12 +  0 +  0 +  3 +  8 = 214
    for (var i = 0; i < digitos.length; i++) {
        // Preenche a soma com o dígito vezes a posição
        soma_digitos = soma_digitos + (digitos[i] * posicoes);

        // Subtrai 1 da posição
        posicoes--;

        // Parte específica para CNPJ
        // Ex.: 5-4-3-2-9-8-7-6-5-4-3-2
        if (posicoes < 2) {
            // Retorno a posição para 9
            posicoes = 9;
        }
    }

    // Captura o resto da divisão entre soma_digitos dividido por 11
    // Ex.: 196 % 11 = 9
    soma_digitos = soma_digitos % 11;

    // Verifica se soma_digitos é menor que 2
    if (soma_digitos < 2) {
        // soma_digitos agora será zero
        soma_digitos = 0;
    } else {
        // Se for maior que 2, o resultado é 11 menos soma_digitos
        // Ex.: 11 - 9 = 2
        // Nosso dígito procurado é 2
        soma_digitos = 11 - soma_digitos;
    }

    // Concatena mais um dígito aos primeiro nove dígitos
    // Ex.: 025462884 + 2 = 0254628842
    var cpf = digitos + soma_digitos;

    // Retorna
    return cpf;

} // calc_digitos_posicoes

/*
 Valida CPF
 
 Valida se for CPF
 
 @param  string cpf O CPF com ou sem pontos e traço
 @return bool True para CPF correto - False para CPF incorreto
*/
function valida_cpf(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // Captura os 9 primeiros dígitos do CPF
    // Ex.: 02546288423 = 025462884
    var digitos = valor.substr(0, 9);

    // Faz o cálculo dos 9 primeiros dígitos do CPF para obter o primeiro dígito
    var novo_cpf = calc_digitos_posicoes(digitos, 10);

    // Faz o cálculo dos 10 dígitos do CPF para obter o último dígito
    var novo_cpf = calc_digitos_posicoes(novo_cpf, 11);

    // Verifica se o novo CPF gerado é idêntico ao CPF enviado
    if (novo_cpf === valor) {
        // CPF válido
        return true;
    } else {
        // CPF inválido
        return false;
    }

} // valida_cpf

/*
 valida_cnpj
 
 Valida se for um CNPJ
 
 @param string cnpj
 @return bool true para CNPJ correto
*/
function valida_cnpj(valor) {

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // O valor original
    var cnpj_original = valor;

    // Captura os primeiros 12 números do CNPJ
    var primeiros_numeros_cnpj = valor.substr(0, 12);

    // Faz o primeiro cálculo
    var primeiro_calculo = calc_digitos_posicoes(primeiros_numeros_cnpj, 5);

    // O segundo cálculo é a mesma coisa do primeiro, porém, começa na posição 6
    var segundo_calculo = calc_digitos_posicoes(primeiro_calculo, 6);

    // Concatena o segundo dígito ao CNPJ
    var cnpj = segundo_calculo;

    // Verifica se o CNPJ gerado é idêntico ao enviado
    if (cnpj === cnpj_original) {
        return true;
    }

    // Retorna falso por padrão
    return false;

} // valida_cnpj

/*
 valida_cpf_cnpj
 
 Valida o CPF ou CNPJ
 
 @access public
 @return bool true para válido, false para inválido
*/
function valida_cpf_cnpj(valor) {

    // Verifica se é CPF ou CNPJ
    var valida = verifica_cpf_cnpj(valor);

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // Valida CPF
    if (valida === 'CPF') {
        // Retorna true para cpf válido
        return valida_cpf(valor);
    }

        // Valida CNPJ
    else if (valida === 'CNPJ') {
        // Retorna true para CNPJ válido
        return valida_cnpj(valor);
    }

        // Não retorna nada
    else {
        return false;
    }

} // valida_cpf_cnpj

/*
 formata_cpf_cnpj
 
 Formata um CPF ou CNPJ
 
 @access public
 @return string CPF ou CNPJ formatado
*/
function formata_cpf_cnpj(valor) {

    // O valor formatado
    var formatado = false;

    // Verifica se é CPF ou CNPJ
    var valida = verifica_cpf_cnpj(valor);

    // Garante que o valor é uma string
    valor = valor.toString();

    // Remove caracteres inválidos do valor
    valor = valor.replace(/[^0-9]/g, '');


    // Valida CPF
    if (valida === 'CPF') {

        // Verifica se o CPF é válido
        if (valida_cpf(valor)) {

            // Formata o CPF ###.###.###-##
            formatado = valor.substr(0, 3) + '.';
            formatado += valor.substr(3, 3) + '.';
            formatado += valor.substr(6, 3) + '-';
            formatado += valor.substr(9, 2) + '';

        }

    }

        // Valida CNPJ
    else if (valida === 'CNPJ') {

        // Verifica se o CNPJ é válido
        if (valida_cnpj(valor)) {

            // Formata o CNPJ ##.###.###/####-##
            formatado = valor.substr(0, 2) + '.';
            formatado += valor.substr(2, 3) + '.';
            formatado += valor.substr(5, 3) + '/';
            formatado += valor.substr(8, 4) + '-';
            formatado += valor.substr(12, 14) + '';
        }

    }

    // Retorna o valor 
    return formatado;

} // formata_cpf_cnpj
