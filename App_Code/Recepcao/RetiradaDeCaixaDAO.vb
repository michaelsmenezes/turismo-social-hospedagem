Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class RetiradaDeCaixaDAO
    Dim ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO
    Dim ObjRetiradaDeCaixaDAO As RetiradaDeCaixaDAO
    Public Function ConsultarCaixaAberto(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String) As RetiradaDeCaixaVO
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS (SELECT 1 FROM TBCXACXA WHERE CXACRTSTS = 'A' AND CXACRTOPR ='" & Usuario & "') ")
                .Append("  BEGIN ")
                .Append("     if exists(select 1 from tbRetiradaDeCaixaCabecalho where cbrUsuario = '" & Usuario & "' and cbrStatus = 'A' )")
                .Append("       BEGIN ")
                .Append("          SELECT C.CXACRTCOD,C.CXACRTOPR,C.CXACRTDABE,C.CXACRTHABE,MAX(CX.cbrId) AS CBRID FROM TBCXACXA C ")
                '.Append("          INNER JOIN tbRetiradaDeCaixa R ON R.cxaCrtCod = C.CxaCrtCod ")
                .Append("          INNER JOIN TBRETIRADADECAIXACABECALHO CX ON CX.CXACRTCOD = C.CXACRTCOD ")
                .Append("          WHERE C.CXACRTSTS = 'A' ")
                .Append("          AND C.CXACRTOPR = '" & Usuario & "' ")
                .Append("          GROUP BY C.CXACRTCOD,C.CXACRTOPR,C.CXACRTDABE,C.CXACRTHABE ")
                .Append("       END ")
                .Append("     ELSE ")
                .Append("       BEGIN ")
                .Append("         SELECT CXACRTCOD,CXACRTOPR,CXACRTDABE,CXACRTHABE,'0' AS CBRID FROM TBCXACXA WHERE CXACRTSTS = 'A' AND CXACRTOPR ='" & Usuario & "' ")
                .Append("       END ")
                .Append("    END ")
            End With
            Return PreencheObjeto(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheObjeto(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As RetiradaDeCaixaVO
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    .cxaCrtCod = ResultadoConsulta.Item("cxaCrtCod")
                    .CxaCrtOpr = ResultadoConsulta.Item("cxaCrtOpr")
                    .cxacrthabe = ResultadoConsulta.Item("cxacrthabe")
                    .CxaCrtDAbe = ResultadoConsulta.Item("CxaCrtDAbe")
                    .cbrId = ResultadoConsulta.Item("cbrId")
                End With
            End While
        Else
            ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
            With ObjRetiradaDeCaixaVO
                .cxaCrtCod = 0
                .CxaCrtOpr = ""
                .CxaCrtDAbe = ""
                .cxacrthabe = ""
                .cbrId = 0
            End With
        End If
        Return ObjRetiradaDeCaixaVO
    End Function
    Public Function Inserir(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String, ByVal rcxId As Long) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("BEGIN TRY ")
                .Append("IF EXISTS (SELECT 1 FROM TBCXACXA WHERE CXACRTSTS = 'A' AND CXACRTOPR ='" & Usuario & "') ")
                .Append("BEGIN ")
                .Append(" IF NOT EXISTS(SELECT 1 FROM tbRetiradaDeCaixa WHERE rcxId = " & rcxId & " ) ")
                .Append("   BEGIN ")
                .Append("     INSERT INTO tbRetiradaDeCaixa(rcxData,cxaCrtCod,cxaCrtOpr,cxaTipoOperacao,cxaQuantidade,cxaValor,cxaNumeroBanco,cxaNumeroCheque,cxaDescricaoCartao,cxaNumeroCupom,cbrId) ")
                .Append("     VALUES ")
                .Append("     ('" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "','" & ObjRetiradaDeCaixaVO.cxaCrtCod & "','" & ObjRetiradaDeCaixaVO.CxaCrtOpr & "','" & ObjRetiradaDeCaixaVO.cxaTipoOperacao & "'," & ObjRetiradaDeCaixaVO.cxaQuantidade & "," & ObjRetiradaDeCaixaVO.cxaValor.Replace(",", ".") & " ,'" & ObjRetiradaDeCaixaVO.cxaNumeroBanco & "','" & ObjRetiradaDeCaixaVO.cxaNumeroCheque & "','" & ObjRetiradaDeCaixaVO.cxaDescricaoCartao & "','" & ObjRetiradaDeCaixaVO.cxaNumeroCupom & "'," & ObjRetiradaDeCaixaVO.cbrId & ") ")
                .Append("     SELECT 1 GOTO SAIDA ")
                .Append("   END ")
                .Append("  ELSE ")
                .Append("   BEGIN ")
                .Append("     UPDATE tbRetiradaDeCaixa SET rcxData = '" & Format(Now, "yyyy-MM-dd HH:mm:ss") & "',cxaCrtCod='" & ObjRetiradaDeCaixaVO.cxaCrtCod & "' ")
                .Append("     ,cxaCrtOpr='" & ObjRetiradaDeCaixaVO.CxaCrtOpr & "',cxaTipoOperacao='" & ObjRetiradaDeCaixaVO.cxaTipoOperacao & "' ")
                .Append("     ,cxaQuantidade=" & ObjRetiradaDeCaixaVO.cxaQuantidade & ",cxaValor=" & ObjRetiradaDeCaixaVO.cxaValor.Replace(",", ".") & ",cxaNumeroBanco='" & ObjRetiradaDeCaixaVO.cxaNumeroBanco & "' ")
                .Append("     ,cxaNumeroCheque='" & ObjRetiradaDeCaixaVO.cxaNumeroCheque & "',cxaDescricaoCartao='" & ObjRetiradaDeCaixaVO.cxaDescricaoCartao & "',cxaNumeroCupom='" & ObjRetiradaDeCaixaVO.cxaNumeroCupom & "',cbrId=" & ObjRetiradaDeCaixaVO.cbrId & " ")
                .Append("     where rcxId = " & rcxId & " ")
                .Append("     SELECT 2 GOTO SAIDA ")
                .Append("   END ")
                .Append("END ")
                .Append("ELSE ")
                .Append(" BEGIN ")
                .Append("   SELECT -1 GOTO SAIDA ")
                .Append(" END ")
                .Append("END TRY ")
                .Append("BEGIN CATCH ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("END CATCH ")
                .Append("SAIDA: ")
                Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaCedulas(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String, ByVal rcxId As Long, ByVal Gerencial As String, ByVal IdRetirada As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Declare @Cedulas Table( ")
                .Append("cxaQuantidade Varchar(30), ")
                .Append("rcxId int, ")
                .Append("cxaValor Dec(18,2), ")
                .Append("cbrId int, ")
                .Append("cxaTotal Dec(18,2)) ")

                .Append("Insert into @Cedulas(rcxId,cxaQuantidade,cxaValor,cxaTotal,cbrId) ")
                .Append("SELECT R.RCXID,R.CXAQUANTIDADE,R.CXAVALOR,R.CXAQUANTIDADE * R.CXAVALOR as TOTAL,R.CBRID as CABECALHO ")
                .Append("FROM TBRETIRADADECAIXA R ")
                .Append("INNER JOIN TBRETIRADADECAIXACABECALHO X ON R.CBRID = X.CBRID ")
                .Append("WHERE R.CXACRTOPR = '" & Usuario & "' ")
                If Gerencial = "N" Then
                    .Append("AND X.CBRSTATUS = 'A' ")
                End If
                .Append("AND R.cxaCrtCod = " & rcxId & " ")
                .Append("AND R.CBRID = " & IdRetirada & " ")
                .Append("and R.cxaTipoOperacao = 'CE' ")

                .Append("Insert into @Cedulas(cxaQuantidade,cxaTotal,cbrId) ")
                .Append("SELECT 'Total Dinheiro:',SUM(CC.cxaTotal),cbrId From @Cedulas CC Group By cbrId ")

                .Append("select * from @cedulas order by cxaValor desc ")
            End With
            Return PreencheListaCedulasMoedas(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function ConsultaMoedas(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String, ByVal rcxId As Long, ByVal Gerencial As String, ByVal IdRetirada As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                'CRIANDO O MOVIMENTO EM MOEDAS
                .Append("Declare @Moedas Table( ")
                .Append("cxaQuantidade Varchar(30), ")
                .Append("rcxId int, ")
                .Append("cbrId int, ")
                .Append("cxaValor Dec(18,2), ")
                .Append("cxaTotal Dec(18,2)) ")

                .Append("Insert into @Moedas(rcxId,cxaQuantidade,cxaValor,cxaTotal,cbrId) ")
                .Append("SELECT R.RCXID,R.CXAQUANTIDADE,R.CXAVALOR,R.CXAQUANTIDADE * R.CXAVALOR as TOTAL,R.CbrId ")
                .Append("FROM TBRETIRADADECAIXA R ")
                .Append("INNER JOIN TBRETIRADADECAIXACABECALHO X ON R.CBRID = X.CBRID ")
                .Append("WHERE R.CXACRTOPR = '" & Usuario & "' ")
                If Gerencial = "N" Then
                    .Append("AND X.CBRSTATUS = 'A' ")
                End If
                .Append("AND R.cxaCrtCod = " & rcxId & " ")
                .Append("AND R.CBRID = " & IdRetirada & " ")
                .Append("and R.cxaTipoOperacao = 'MO' ")
                .Append("Insert into @Moedas(cxaQuantidade,cxaTotal,cbrId) ")
                .Append("SELECT 'Total Moedas:',SUM(CC.cxaTotal),cbrId From @Moedas CC Group By cc.cbrId ")

                .Append("select * from @Moedas order by cxaValor desc ")
            End With
            Return PreencheListaCedulasMoedas(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaCedulasMoedas(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    .cxaQuantidade = ResultadoConsulta.Item("cxaQuantidade")
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaValor")) Then
                        .cxaValor = ""
                    Else
                        .cxaValor = ResultadoConsulta.Item("cxaValor")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaTotal")) Then
                        .cxaTotal = 0
                    Else
                        .cxaTotal = FormatNumber(ResultadoConsulta.Item("cxaTotal"), 2)
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("rcxId")) Then
                        .rcxId = 0
                    Else
                        .rcxId = ResultadoConsulta.Item("rcxId")
                    End If
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        Else
            ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
            With ObjRetiradaDeCaixaVO
                .cxaQuantidade = ""
                .cxaValor = ""
                .cxaTotal = 0
                .rcxId = 0
            End With
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function ConsultaCheques(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String, ByVal rcxId As Long, ByVal Gerencial As String, ByVal IdRetirada As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                'CRIANDO O MOVIMENTO EM CHEQUES ")
                .Append("Declare @Cheques Table( ")
                .Append("cxaNumeroBanco Varchar(20), ")
                .Append("rcxId int, ")
                .Append("cbrId int, ")
                .Append("cxaNumeroCheque Varchar(10), ")
                .Append("CxaValor Dec(18,2)) ")

                .Append("Insert into @Cheques(rcxId,cxaNumerocheque,cxaNumeroBanco,cxavalor,cbrId) ")
                .Append("SELECT R.rcxId,R.cxaNumeroCheque,R.cxaNumeroBanco,R.CXAVALOR as TOTAL,R.cbrId ")
                .Append("FROM TBRETIRADADECAIXA R ")
                .Append("INNER JOIN TBRETIRADADECAIXACABECALHO X ON R.CBRID = X.CBRID  ")
                .Append("WHERE R.CXACRTOPR = '" & Usuario & "' ")
                If Gerencial = "N" Then
                    .Append("AND X.CBRSTATUS = 'A' ")
                End If
                .Append("AND R.cxaCrtCod = " & rcxId & " ")
                .Append("AND R.CBRID = " & IdRetirada & " ")
                .Append("and R.cxaTipoOperacao = 'CH' ")

                .Append("Insert into @Cheques(cxaNumeroBanco,cxaValor,cbrId) ")
                .Append("SELECT 'Total Cheques:',SUM(CC.cxaValor),cbrId From @Cheques CC group by cc.cbrid ")

                .Append("select * from @Cheques order by cxaValor ASC ")
            End With
            Return PreencheListaCheques(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaCheques(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    .cxaNumeroBanco = ResultadoConsulta.Item("cxaNumeroBanco")
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCheque")) Then
                        .cxaNumeroCheque = ""
                    Else
                        .cxaNumeroCheque = ResultadoConsulta.Item("cxaNumeroCheque")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaValor")) Then
                        .cxaValor = 0
                    Else
                        .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("rcxId")) Then
                        .rcxId = 0
                    Else
                        .rcxId = ResultadoConsulta.Item("rcxId")
                    End If
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        Else
            With ObjRetiradaDeCaixaVO
                .cxaNumeroBanco = 0
                .cxaNumeroCheque = 0
                .cxaValor = 0
                .rcxId = 0
            End With
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function ConsultaCartoes(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal AliasBanco As String, ByVal Usuario As String, ByVal rcxId As Long, ByVal Gerencial As String, ByVal IdRetirada As Long) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                'CRIANDO O MOVIMENTO EM CARTOES 
                .Append("Declare @Cartao Table( ")
                .Append("cxaDescricaoCartao Varchar(30), ")
                .Append("rcxId int, ")
                .Append("cbrId int, ")
                .Append("cxaNumeroCupom varchar(15), ")
                .Append("cxaValor Dec(18,2)) ")

                .Append("Insert into @Cartao(rcxId,cxaDescricaoCartao,cxaValor,cxaNumeroCupom,cbrId) ")
                .Append("SELECT R.rcxId,R.cxaDescricaoCartao,R.CXAVALOR,R.CXANUMEROCUPOM as TOTAL,R.cbrId ")
                .Append("FROM TBRETIRADADECAIXA R ")
                .Append("INNER JOIN TBRETIRADADECAIXACABECALHO X ON R.cbrId = X.CBRID ")
                .Append("WHERE R.CXACRTOPR = '" & Usuario & "' ")
                If Gerencial = "N" Then
                    .Append("AND X.CBRSTATUS = 'A' ")
                End If
                .Append("AND R.cxaCrtCod = " & rcxId & " ")
                .Append("AND R.CBRID = " & IdRetirada & " ")
                .Append("and R.cxaTipoOperacao = 'CA' ")

                .Append("Insert into @Cartao(cxaDescricaoCartao,cxaValor,cbrId) ")
                .Append("SELECT 'Total Cartões:',SUM(CC.cxaValor),cbrId From @Cartao CC Group by cc.cbrId ")

                .Append("select rcxId, ")
                .Append("CASE ")
                .Append("  WHEN CR.cxaDescricaoCartao = 'VN' then 'Visa Net' ")
                .Append("  WHEN CR.cxaDescricaoCartao = 'VE' then 'Visa Eletro' ")
                .Append("  When CR.cxaDescricaoCartao = 'Total Cartões:' then 'Total Cartões:' ")
                .Append("END as cxaDescricaoCartao, cxaValor,cxaNumeroCupom,cbrId ")
                .Append("  from @Cartao CR order by cxaValor ASC ")
            End With
            Return PreencheListaCartoes(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaCartoes(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaDescricaoCartao")) Then
                        .cxaDescricaoCartao = ""
                    Else
                        .cxaDescricaoCartao = ResultadoConsulta.Item("cxaDescricaoCartao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaValor")) Then
                        .cxaValor = 0
                    Else
                        .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("rcxId")) Then
                        .rcxId = 0
                    Else
                        .rcxId = ResultadoConsulta.Item("rcxId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCupom")) Then
                        .cxaNumeroCupom = ""
                    Else
                        .cxaNumeroCupom = ResultadoConsulta.Item("cxaNumeroCupom")
                    End If
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        Else
            ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
            With ObjRetiradaDeCaixaVO
                .cxaDescricaoCartao = ""
                .cxaValor = 0
                .rcxId = 0
                .cxaNumeroCupom = ""
            End With
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Private Function PreencheListaValores(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    If Convert.IsDBNull(ResultadoConsulta.Item("rcxId")) Then
                        .rcxId = 0
                    Else
                        .rcxId = ResultadoConsulta.Item("rcxId")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaCrtCod")) Then
                        .cxaCrtCod = 0
                    Else
                        .cxaCrtCod = ResultadoConsulta.Item("cxaCrtCod")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("CxaCrtOpr")) Then
                        .CxaCrtOpr = ""
                    Else
                        .CxaCrtOpr = ResultadoConsulta.Item("CxaCrtOpr")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaDescricaoCartao")) Then
                        .cxaDescricaoCartao = ""
                    Else
                        .cxaDescricaoCartao = ResultadoConsulta.Item("cxaDescricaoCartao")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroBanco")) Then
                        .cxaNumeroBanco = ""
                    Else
                        .cxaNumeroBanco = ResultadoConsulta.Item("cxaNumeroBanco")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCheque")) Then
                        .cxaNumeroCheque = ""
                    Else
                        .cxaNumeroCheque = ResultadoConsulta.Item("cxaNumeroCheque")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaQuantidade")) Then
                        .cxaQuantidade = 0
                    Else
                        .cxaQuantidade = ResultadoConsulta.Item("cxaQuantidade")
                    End If

                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaTipoOperacao")) Then
                        .cxaTipoOperacao = ""
                    Else
                        .cxaTipoOperacao = ResultadoConsulta.Item("cxaTipoOperacao")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaValor")) Then
                        .cxaValor = 0
                    Else
                        .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                    End If

                    'If Convert.IsDBNull(ResultadoConsulta.Item("cxaTotal")) Then
                    '    .cxaTotal = 0
                    'Else
                    '    .cxaTotal = ResultadoConsulta.Item("cxaTotal")
                    'End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("rcxData")) Then
                        .rcxData = ""
                    Else
                        .rcxData = ResultadoConsulta.Item("rcxData")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCupom")) Then
                        .cxaNumeroCupom = ""
                    Else
                        .cxaNumeroCupom = ResultadoConsulta.Item("cxaNumeroCupom")
                    End If
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function ConsultaRetiradaCodigo(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal crxId As Long, ByVal AliasBanco As String) As RetiradaDeCaixaVO
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT ")
                .Append("RCXID, RCXDATA, CXACRTCOD, CXATIPOOPERACAO, CXAQUANTIDADE, CXAVALOR, CXANUMEROBANCO, CXANUMEROCHEQUE, CXADESCRICAOCARTAO, CXACRTOPR,CXANUMEROCUPOM ")
                .Append("FROM TBRETIRADADECAIXA WHERE RCXID = " & crxId & " ")
            End With
            Return PreencheListaPorCodigo(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaPorCodigo(ByVal ResultadoConsulta As Data.SqlClient.SqlDataReader) As RetiradaDeCaixaVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
            With ObjRetiradaDeCaixaVO
                If Convert.IsDBNull(ResultadoConsulta.Item("rcxId")) Then
                    .rcxId = 0
                Else
                    .rcxId = ResultadoConsulta.Item("rcxId")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("cxaCrtCod")) Then
                    .cxaCrtCod = 0
                Else
                    .cxaCrtCod = ResultadoConsulta.Item("cxaCrtCod")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("CxaCrtOpr")) Then
                    .CxaCrtOpr = ""
                Else
                    .CxaCrtOpr = ResultadoConsulta.Item("CxaCrtOpr")
                End If

                If Convert.IsDBNull(ResultadoConsulta.Item("cxaDescricaoCartao")) Then
                    .cxaDescricaoCartao = ""
                Else
                    .cxaDescricaoCartao = ResultadoConsulta.Item("cxaDescricaoCartao")
                End If

                If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroBanco")) Then
                    .cxaNumeroBanco = ""
                Else
                    .cxaNumeroBanco = ResultadoConsulta.Item("cxaNumeroBanco")
                End If

                If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCheque")) Then
                    .cxaNumeroCheque = ""
                Else
                    .cxaNumeroCheque = ResultadoConsulta.Item("cxaNumeroCheque")
                End If

                If Convert.IsDBNull(ResultadoConsulta.Item("cxaQuantidade")) Then
                    .cxaQuantidade = 0
                Else
                    .cxaQuantidade = ResultadoConsulta.Item("cxaQuantidade")
                End If

                If Convert.IsDBNull(ResultadoConsulta.Item("cxaTipoOperacao")) Then
                    .cxaTipoOperacao = ""
                Else
                    .cxaTipoOperacao = ResultadoConsulta.Item("cxaTipoOperacao")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("cxaValor")) Then
                    .cxaValor = 0
                Else
                    .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                End If

                'If Convert.IsDBNull(ResultadoConsulta.Item("cxaTotal")) Then
                '    .cxaTotal = 0
                'Else
                '    .cxaTotal = ResultadoConsulta.Item("cxaTotal")
                'End If
                If Convert.IsDBNull(ResultadoConsulta.Item("rcxData")) Then
                    .rcxData = ""
                Else
                    .rcxData = ResultadoConsulta.Item("rcxData")
                End If
                If Convert.IsDBNull(ResultadoConsulta.Item("cxaNumeroCupom")) Then
                    .cxaNumeroCupom = ""
                Else
                    .cxaNumeroCupom = ResultadoConsulta.Item("cxaNumeroCupom")
                End If

            End With
        End If
        ResultadoConsulta.Close()
        Return ObjRetiradaDeCaixaVO
    End Function
    Public Function Excluir(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal NrCaixa As Long, ByVal crxId As Long, ByVal AliasBanco As String) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Begin try ")
                'Verifica se existe caixa aberto para poder apagar
                .Append("if exists (select 1 from TbCxaCxa X where x.CxaCrtCod = " & NrCaixa & " and X.CxaCrtSts = 'A') ")
                .Append("Begin ")
                'Caixa aberto, vamos ver se existe o item pra ser apagado
                .Append(" if exists(select 1 from tbRetiradaDeCaixa where rcxId = " & crxId & ") ")
                .Append("     Begin ")
                .Append("       delete from tbRetiradaDeCaixa where rcxId = " & crxId & " ")
                .Append("       Select 1 goto saida ")
                .Append("     end ")
                .Append("  else ")
                'Se não existir o item a ser apagado
                .Append("     Begin ")
                .Append("       select -1 goto saida")
                .Append("     End ")
                .Append(" End ")
                .Append("else ")
                .Append("  Begin ")
                .Append("    Select -2 goto saida ")
                .Append("  End ")
                .Append("End try ")
                'Se não existir nem o caixa aberto
                .Append("begin catch ")
                .Append("  Select 0 goto Saida ")
                .Append("end catch ")
                .Append("Saida: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function VerificaOpercaoChequeCartao(ByVal AliasBanco As String, ByVal Usuario As String, ByVal Caixa As String, ByVal Valor As String, ByVal TipoOperacao As String) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("BEGIN TRY ")
                .Append("IF EXISTS (SELECT 1 FROM TBCXACXA WHERE CXACRTSTS = 'A' AND CXACRTOPR ='" & Usuario & "') ")
                .Append(" BEGIN ")
                If TipoOperacao = "CA" Then
                    .Append("IF EXISTS(SELECT 1 FROM TBCXAOPR WHERE CXACRTCOD = " & Caixa & " AND OPRVAL = " & Valor & " and OprTipPag IN ('VE','VN','AE','CC','RC')) ")
                Else
                    .Append("IF EXISTS(SELECT 1 FROM TBCXAOPR WHERE CXACRTCOD = " & Caixa & " AND OPRVAL = " & Valor & " and OprTipPag = 'CH') ")
                End If
                .Append("           BEGIN ")
                .Append("             SELECT 1 GOTO SAIDA ")
                .Append("           END ")
                .Append("         ELSE ")
                .Append("           BEGIN ")
                .Append("             SELECT -2 GOTO SAIDA ")
                .Append("           END ")
                .Append(" END ")
                .Append("ELSE ")
                .Append("  SELECT -1 GOTO SAIDA ")
                .Append("END TRY ")
                .Append("BEGIN CATCH ")
                .Append("   SELECT 0 GOTO SAIDA ")
                .Append("END CATCH ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function VisualizaRetiradas(ByVal DataIni As String, ByVal DataFim As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT MIN(RCXDATA)AS CXADATA,CX.CXACRTOPR,CX.CXACRTCOD,SUM(CX.CXAQUANTIDADE * CX.CXAVALOR) AS CXAVALOR,")
                .Append("(SELECT SUM(O.OprVal)FROM TbCxaOpr O WHERE O.CxaCrtCod = CX.cxaCrtCod) AS CxaValorOperacoes ")
                .Append("FROM TBRETIRADADECAIXA CX ")
                .Append("WHERE CX.RCXDATA BETWEEN '" & Format(CDate(DataIni), "yyyy-MM-dd 00:00:00") & "' AND '" & Format(CDate(DataFim), "yyyy-MM-dd 23:59:59") & "' ")
                .Append("GROUP BY CX.CXACRTOPR,CX.CXACRTCOD ")
            End With
            Return PreencheListaRetiradas(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaRetiradas(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    .rcxData = Format(CDate(ResultadoConsulta.Item("cxaData")), "dd/MM/yyyy")
                    .CxaCrtOpr = ResultadoConsulta.Item("CxaCrtOpr")
                    .cxaCrtCod = ResultadoConsulta.Item("cxaCrtCod")
                    .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                    .CxaValorOperacoes = FormatNumber(ResultadoConsulta.Item("cxaValorOperacoes"), 2)
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function InserirCabecalho(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal caixa As Long, ByVal Operador As String, ByVal AliasBanco As String, ByVal AbrirFechar As String, ByVal ValorTotal As String) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("Declare @Status char(1) ")
                .Append("set @Status = '" & AbrirFechar & "' ")
                .Append("BEGIN TRY ")
                .Append("  IF NOT EXISTS(SELECT 1 FROM tbRetiradaDeCaixaCabecalho where cxacrtCod = " & caixa & " and cbrstatus = 'A') ")
                .Append("    BEGIN ")
                .Append("       INSERT INTO TBRETIRADADECAIXACABECALHO(CBRDATA,CBRUSUARIO,CXACRTCOD,CBRTOTAL,CBRSTATUS) ")
                .Append("       SELECT GETDATE(),C.CXACRTOPR,C.CXACRTCOD,0,'A'  FROM TBCXACXA C WHERE C.CXACRTOPR = '" & Operador & "' AND C.CXACRTSTS = 'A' ")
                .Append("       SELECT CBRID FROM TBRETIRADADECAIXACABECALHO WHERE CBRUSUARIO = '" & Operador & "' AND CBRSTATUS = 'A' GOTO SAIDA ")
                .Append("     END ")
                .Append("   ELSE ")
                .Append("     if @Status = 'F' ")
                .Append("        Begin")
                .Append("          UPDATE tbRetiradaDeCaixaCabecalho set CbrTotal = '" & ValorTotal & "',CbrStatus = 'F' WHERE cxacrtCod = " & caixa & " and cbrstatus = 'A' ")
                .Append("          SELECT 2 GOTO SAIDA ")
                .Append("        End ")
                .Append("    ELSE ")
                .Append("       SELECT 3 GOTO SAIDA ")
                .Append("END TRY ")
                .Append("BEGIN CATCH ")
                .Append("  SELECT 0 GOTO SAIDA ")
                .Append("END CATCH ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaCabecalhoAtivo(ByVal Operador As String, ByVal AliasBanco As String) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("IF EXISTS (SELECT TOP 1 CBRID FROM TBRETIRADADECAIXACABECALHO WHERE CBRUSUARIO = '" & Operador & "' AND CBRSTATUS = 'A' ORDER BY CBRID DESC) ")
                .Append("  BEGIN")
                .Append("    SELECT (SELECT TOP 1 CBRID FROM TBRETIRADADECAIXACABECALHO WHERE CBRUSUARIO = '" & Operador & "' AND CBRSTATUS = 'A' ORDER BY CBRID DESC) GOTO SAIDA ")
                .Append("    END ")
                .Append(" ELSE ")
                .Append("BEGIN ")
                .Append("    SELECT 0 GOTO SAIDA ")
                .Append("END ")
                .Append("SAIDA: ")
            End With
            Dim Resultadao As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultadao
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaRetiradasPorCaixa(ByVal ObjRetiradaDeCaixaVO As RetiradaDeCaixaVO, ByVal caixa As Long, ByVal AliasBanco As String, ByVal DataIni As String, ByVal DataFim As String) As IList
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder
            With VarSql
                .Append("SELECT MIN(RT.CBRDATA) AS RCXDATA ,CX.CBRID,CX.CXACRTOPR,CX.CXACRTCOD,SUM(CX.CXAQUANTIDADE  * CX.CXAVALOR) AS CXAVALOR ")
                .Append("FROM TBRETIRADADECAIXA CX ")
                .Append("INNER JOIN TBRETIRADADECAIXACABECALHO RT ON RT.CBRID = CX.CBRID ")
                .Append("WHERE CX.RCXDATA BETWEEN '" & Format(CDate(DataIni), "yyyy-MM-dd 00:00:00") & "' AND '" & Format(CDate(DataFim), "yyyy-MM-dd 23:59:59") & "' ")
                .Append("AND CX. CXACRTCOD = " & caixa & " ")
                .Append("GROUP BY CONVERT(CHAR(10),CX.RCXDATA,103),CX.CBRID,CX.CXACRTOPR,CX.CXACRTCOD ")
            End With
            Return PreencheListaRetiradasPorCaixa(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaRetiradasPorCaixa(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjRetiradaDeCaixaVO = New RetiradaDeCaixaVO
                With ObjRetiradaDeCaixaVO
                    .rcxData = Format(CDate(ResultadoConsulta.Item("RCXDATA")), "dd/MM/yyyy HH:mm:ss")
                    .cbrId = ResultadoConsulta.Item("cbrId")
                    .CxaCrtOpr = ResultadoConsulta.Item("CxaCrtOpr")
                    .cxaCrtCod = ResultadoConsulta.Item("cxaCrtCod")
                    .cxaValor = FormatNumber(ResultadoConsulta.Item("cxaValor"), 2)
                End With
                Lista.Add(ObjRetiradaDeCaixaVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function ForcaFechamentoRetirada(ByVal Operador As String, ByVal AliasBanco As String) As Long
        Try
            Dim Conn As New Banco.Conexao(AliasBanco)
            Dim VarSql As New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("BEGIN TRY ")
                'Se a diferença de data for maior que 0, irá fechar automaticamente a retirada
                .Append("IF EXISTS(SELECT 1 FROM TBRETIRADADECAIXACABECALHO WHERE CBRUSUARIO = '" & Operador & "' AND DATEDIFF(day,CBRDATA,GETDATE()) > 0 AND CBRSTATUS = 'A') ")
                .Append("   BEGIN ")
                .Append("       UPDATE TBRETIRADADECAIXACABECALHO SET CBRSTATUS = 'F' WHERE CBRUSUARIO = '" & Operador & "' AND GETDATE() > CBRDATA AND CBRSTATUS = 'A' ")
                .Append("       SELECT 1 GOTO SAIDA ")
                .Append("   END ")
                .Append("  ELSE ")
                .Append("   BEGIN ")
                .Append("      SELECT 2 GOTO SAIDA ")
                .Append("   END ")
                .Append("END TRY ")
                .Append("BEGIN CATCH ")
                .Append("  SELECT 0 GOTO SAIDA ")
                .Append("END CATCH ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
End Class
