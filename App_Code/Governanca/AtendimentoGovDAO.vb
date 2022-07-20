Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic

Public Class AtendimentoGovDAO
    Dim ObjAtendimentoGovVO As AtendimentoGovVO
    Dim ObjAtendimentoGovDAO As AtendimentoGovDAO

    Public Function Inserir(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal AliasBanco As String) As Long
        Try
            'FUNÇÃO PARA FAZER O ATENDIMENTO DOS APARTAMENTOS NO SETOR DE GOVERNAÇA, LANÇAMENTO DE ITENS DO APARTAMENTO.
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                '.Append("EXEC SpAtendimentoGov ")
                ''Parametros da SP'
                '.Append(" " & ObjAtendimentoGovVO.AGoId & "," & ObjAtendimentoGovVO.ApaId & "," & ObjAtendimentoGovVO.CamId & "," & ObjAtendimentoGovVO.AGoCCasal & "," & ObjAtendimentoGovVO.AGoCSolteiro & ",")
                '.Append("" & ObjAtendimentoGovVO.AGoBerco & "," & ObjAtendimentoGovVO.AGoTravesseiro & "," & ObjAtendimentoGovVO.AGoJogoToalhas & "," & ObjAtendimentoGovVO.AGoRoloPapel & ",")
                '.Append("" & ObjAtendimentoGovVO.AGoSacoLixo & "," & ObjAtendimentoGovVO.AGoSabonete & ",'" & ObjAtendimentoGovVO.AGoTapete & "','" & ObjAtendimentoGovVO.AGoOrigem & "',")
                '.Append("'" & ObjAtendimentoGovVO.AGoObservacao & "','" & ObjAtendimentoGovVO.Acao & "'," & ObjAtendimentoGovVO.ApaCamaExtra & "," & ObjAtendimentoGovVO.ApaBerco & ",")
                '.Append("'" & ObjAtendimentoGovVO.Usuario & "'")

                .Append("DECLARE @ERRO SMALLINT ")
                .Append("SET @ERRO = 0 ")
                .Append("IF ('" & ObjAtendimentoGovVO.Acao & "' = 'A') AND ('" & ObjAtendimentoGovVO.AGoOrigem & "' IN ('R','A')) ")
                .Append("BEGIN ")
                .Append("IF (" & ObjAtendimentoGovVO.AGoCCasal & " <> 0) OR (" & ObjAtendimentoGovVO.AGoCSolteiro & "  <> 0) OR (" & ObjAtendimentoGovVO.AGoBerco & " <> 0) OR (" & ObjAtendimentoGovVO.AGoTravesseiro & " <> 0) OR ")
                .Append("(" & ObjAtendimentoGovVO.AGoJogoToalhas & " <> 0) OR (" & ObjAtendimentoGovVO.AGoRoloPapel & " <> 0) OR (" & ObjAtendimentoGovVO.AGoSacoLixo & "<> 0) OR (" & ObjAtendimentoGovVO.AGoSabonete & " <> 0) OR ")
                .Append("('" & ObjAtendimentoGovVO.AGoTapete & "' = 'S') OR ('" & ObjAtendimentoGovVO.AGoObservacao & "' > '') ")
                .Append("BEGIN ")
                .Append("INSERT TBATENDIMENTOGOV (APAID, CAMID, AGODATA, AGOCCASAL, AGOCSOLTEIRO, AGOBERCO, AGOTRAVESSEIRO, ")
                .Append("AGOJOGOTOALHAS, AGOROLOPAPEL, AGOSACOLIXO, AGOSABONETE, AGOTAPETE, AGOORIGEM, AGOOBSERVACAO,AGOUSUARIO,AGODATALOG) ")
                .Append("VALUES (" & ObjAtendimentoGovVO.ApaId & "," & ObjAtendimentoGovVO.CamId & ", GETDATE()," & ObjAtendimentoGovVO.AGoCCasal & "," & ObjAtendimentoGovVO.AGoCSolteiro & " ," & ObjAtendimentoGovVO.AGoBerco & " ," & ObjAtendimentoGovVO.AGoTravesseiro & ", ")
                .Append("" & ObjAtendimentoGovVO.AGoJogoToalhas & "," & ObjAtendimentoGovVO.AGoRoloPapel & "," & ObjAtendimentoGovVO.AGoSacoLixo & " ," & ObjAtendimentoGovVO.AGoSabonete & ",'" & ObjAtendimentoGovVO.AGoTapete & "','" & ObjAtendimentoGovVO.AGoOrigem & "',RTRIM('" & ObjAtendimentoGovVO.AGoObservacao & "'), ")
                .Append("'" & ObjAtendimentoGovVO.Usuario & "',GetDate()) ")
                '.Append("+ RTRIM('" & ObjAtendimentoGovVO.Usuario & "')) ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("END ")
                .Append("IF (@ERRO = 0) ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO SET ")
                .Append("APACAMAEXTRA = " & ObjAtendimentoGovVO.ApaCamaExtra & ", ")
                .Append("APABERCO = " & ObjAtendimentoGovVO.ApaBerco & ", ")
                .Append("APAUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("END ")
                .Append("END ")
                .Append("ELSE IF ('" & ObjAtendimentoGovVO.Acao & "' = 'A') ")
                .Append("BEGIN ")
                .Append("UPDATE TBATENDIMENTOGOV SET ")
                .Append("CAMID =  " & ObjAtendimentoGovVO.CamId & ", ")
                .Append("AGODATA = GETDATE(), ")
                .Append("AGOCCASAL = " & ObjAtendimentoGovVO.AGoCCasal & ", ")
                .Append("AGOCSOLTEIRO =  " & ObjAtendimentoGovVO.AGoCSolteiro & ", ")
                .Append("AGOBERCO = " & ObjAtendimentoGovVO.AGoBerco & ", ")
                .Append("AGOTRAVESSEIRO = " & ObjAtendimentoGovVO.AGoTravesseiro & " , ")
                .Append("AGOJOGOTOALHAS = " & ObjAtendimentoGovVO.AGoJogoToalhas & ", ")
                .Append("AGOROLOPAPEL = " & ObjAtendimentoGovVO.AGoRoloPapel & " , ")
                .Append("AGOSACOLIXO = " & ObjAtendimentoGovVO.AGoSacoLixo & " , ")
                .Append("AGOSABONETE = " & ObjAtendimentoGovVO.AGoSabonete & ", ")
                .Append("AGOTAPETE = '" & ObjAtendimentoGovVO.AGoTapete & "', ")
                .Append("AGOUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("AGODATALOG = GetDate(), ")
                .Append("AGOOBSERVACAO = RTRIM('" & ObjAtendimentoGovVO.AGoObservacao & "') ")
                .Append("WHERE AGOID = " & ObjAtendimentoGovVO.AGoId & " AND APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("IF (@ERRO = 0) AND (@@ROWCOUNT > 0) ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO SET  ")
                .Append("APASTATUS = CASE ")
                .Append("WHEN ('" & ObjAtendimentoGovVO.AGoOrigem & "' IN ('G','C')) AND APASTATUS = 'A' THEN 'L' ")
                .Append("ELSE APASTATUS ")
                .Append("END, ")
                .Append("APACAMAEXTRA = " & ObjAtendimentoGovVO.ApaCamaExtra & ", ")
                .Append("APABERCO = " & ObjAtendimentoGovVO.ApaBerco & ", ")
                .Append("APAUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("END ")
                .Append("END ")
                .Append("ELSE IF ('" & ObjAtendimentoGovVO.Acao & "'= 'E') ")
                .Append("BEGIN ")
                .Append("DELETE TBATENDIMENTOGOV WHERE AGOID = " & ObjAtendimentoGovVO.AGoId & " AND ISNULL(AGODATA, 0) = 0 ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("IF (@ERRO = 0) AND (@@ROWCOUNT > 0) AND ('" & ObjAtendimentoGovVO.AGoOrigem & "'IN ('G','C')) ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO SET ")
                .Append("APASTATUS = 'L', ")
                .Append("APAUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & "AND  ")
                .Append("NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " AND ISNULL(AGODATA, 0) = 0) ")
                .Append("AND APASTATUS = 'A' ")
                .Append("SET @ERRO = @ERRO + @@ERROR ")
                .Append("END ")
                .Append("END ")
                .Append("IF @ERRO = 0 ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("END ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function AtualizaAptoGovernanca(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                'ESSA PROCEDURE PEGA APARTAMENTO LIMPO E PASSA PARA ARRUMAÇÃO - CONFIRMAR COM HAAS O QUE MAIS ELA FAZ
                .Append("DECLARE @ERRO NUMERIC,@Dia smallint, @ROW NUMERIC, @ACM NUMERIC ")
                .Append("SET @DIA = " & ObjAtendimentoGovVO.Dia & "")
                .Append("IF '" & ObjAtendimentoGovVO.Acao & "' = 'G' ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO ")
                .Append("SET APASTATUS = 'A', APAUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("AND APASTATUS = 'L' ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("IF (@ERRO = 0) AND (@ROW > 0) AND ")
                .Append("NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV WHERE (APAID = " & ObjAtendimentoGovVO.ApaId & " ) AND (ISDATE(AGODATA) = 0)) ")
                .Append("BEGIN ")
                .Append("INSERT TBATENDIMENTOGOV (APAID, AGOORIGEM) VALUES (" & ObjAtendimentoGovVO.ApaId & " ,'" & ObjAtendimentoGovVO.Acao & "') ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")
                .Append("END ")
                .Append("ELSE IF ('" & ObjAtendimentoGovVO.Acao & "' = 'M') AND ('" & ObjAtendimentoGovVO.AlteraManutencao & "' = '0') ")
                .Append("BEGIN ")
                .Append("UPDATE TBAPARTAMENTO ")
                .Append("SET APASTATUS = ")
                .Append("CASE ")
                .Append("WHEN APASTATUS IN ('L','A') THEN 'M' ")
                .Append("ELSE APASTATUS ")
                .Append("END, ")
                .Append("APAUSUARIO ='" & ObjAtendimentoGovVO.Usuario & "', APAUSUARIODATA = GETDATE() ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("IF (@ERRO = 0) AND (@ROW > 0) ")
                .Append("BEGIN ")
                '.Append("INSERT TBMANUTENCAO (MANDATAABERTURA, APAID, MANREQUISITANTE, MANDESCRICAOREQUIS) ")
                '.Append("VALUES (GETDATE()," & ObjAtendimentoGovVO.ApaId & ",'" & ObjAtendimentoGovVO.Usuario & "','" & ObjAtendimentoGovVO.ManDescricaoResquisitante & "') ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("IF (@ERRO = 0) AND (@ROW > 0) AND (@DIA >= 1) ")
                .Append("BEGIN ")
                .Append("SET @ACM = ISNULL((SELECT TOP 1 ACMID FROM TBAPARTAMENTO WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " AND APASTATUS <> 'O'), 0) ")
                .Append("IF @ACM <> 0 ")
                .Append("BEGIN ")
                .Append("IF EXISTS (SELECT 1 FROM TBSOLICITACAO WHERE RESID IS NULL AND APAID = " & ObjAtendimentoGovVO.ApaId & ") ")
                .Append("UPDATE TBSOLICITACAO SET ")
                .Append("SOLDATAFIM = GETDATE() + " & ObjAtendimentoGovVO.Dia & ", ")
                .Append("SOLDATAFIMAUX = GETDATE() + " & ObjAtendimentoGovVO.Dia & ", ")
                .Append("SOLUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("SOLUSUARIODATA = GETDATE() ")
                .Append("WHERE RESID IS NULL AND APAID = " & ObjAtendimentoGovVO.ApaId & " AND SOLDATAFIM < GETDATE() + " & ObjAtendimentoGovVO.Dia & " ")
                .Append("ELSE ")
                .Append("INSERT TBSOLICITACAO (ACMID, ACMIDCOBRANCA, APAID, SOLDATAINI, SOLHORAINI, SOLDATAFIM, SOLHORAFIM, ")
                .Append("SOLDATAINIAUX, SOLDATAFIMAUX, SOLUSUARIO, SOLUSUARIODATA) VALUES(")
                .Append("@ACM,@ACM," & ObjAtendimentoGovVO.ApaId & " ,GETDATE(),DATEPART(HH, GETDATE()),")
                .Append("GETDATE() + " & ObjAtendimentoGovVO.Dia & " ,DATEPART(HH, GETDATE() + " & ObjAtendimentoGovVO.Dia & "),")
                .Append("GETDATE(),GETDATE() + " & ObjAtendimentoGovVO.Dia & ",'" & ObjAtendimentoGovVO.Usuario & "',GETDATE()) ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")
                .Append("END ")
                .Append("END ")
                .Append("END ")
                .Append("ELSE IF ('" & ObjAtendimentoGovVO.Acao & "' = 'M') AND ('" & ObjAtendimentoGovVO.AlteraManutencao & "' <> '0') ")
                .Append("BEGIN ")
                '.Append("UPDATE TBMANUTENCAO  ")
                '.Append("SET MANDESCRICAOREQUIS = '" & ObjAtendimentoGovVO.Manutencao & "' ")
                '.Append("WHERE MANID = '" & ObjAtendimentoGovVO.AlteraManutencao & "' ")
                .Append("SET @ERRO = @@ERROR ")
                .Append("SET @ROW = @@ROWCOUNT ")
                .Append("END ")
                .Append("IF @ERRO = 0 AND (@ROW > 0) ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("SAIDA: ")
                Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function ConsultaAtendimento(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal AliasBanco As String) As AtendimentoGovVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT * FROM  TBATENDIMENTOGOV ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " AND ISNULL(AGODATA, 0) = 0")

            End With
            Return PreencheAgoId(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function ConsultaAtendimentoArrumacao(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal AliasBanco As String) As AtendimentoGovVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT * FROM  TBATENDIMENTOGOV ")
                .Append("WHERE APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                .Append("AND AGOID = " & ObjAtendimentoGovVO.AGoId & "")
            End With
            Return PreencheAgoId(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheAgoId(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As AtendimentoGovVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjAtendimentoGovVO = New AtendimentoGovVO
            With ObjAtendimentoGovVO
                .AGoId = ResultadoConsulta.Item("AgoId")
                If Convert.IsDBNull(ResultadoConsulta.Item("CamId")) Then
                    .CamId = 0
                Else
                    .CamId = ResultadoConsulta.Item("CamId")
                End If
                .ApaId = ResultadoConsulta.Item("ApaId")
                .AGoCCasal = ResultadoConsulta.Item("AGoCCasal")
                .AGoCSolteiro = ResultadoConsulta.Item("AGoCSolteiro")
                .AGoBerco = ResultadoConsulta.Item("AGoBerco")
                .AGoTravesseiro = ResultadoConsulta.Item("AGoTravesseiro")
                .AGoJogoToalhas = ResultadoConsulta.Item("AGoJogoToalhas")
                .AGoRoloPapel = ResultadoConsulta.Item("AGoRoloPapel")
                .AGoSacoLixo = ResultadoConsulta.Item("AGoSacoLixo")
                .AGoSabonete = ResultadoConsulta.Item("AGoSabonete")
                .AGoTapete = ResultadoConsulta.Item("AGoTapete")
                'If (ResultadoConsulta.Item("AGoObservacao") = " ") Or (Convert.IsDBNull(ResultadoConsulta.Item("AGoObservacao"))) Then
                If Convert.IsDBNull(ResultadoConsulta.Item("AGoObservacao")) Then
                    .AGoObservacao = " "
                Else
                    .AGoObservacao = ResultadoConsulta.Item("AGoObservacao")
                End If
                '.CamId = ResultadoConsulta.Item("CamId")
                .AGoOrigem = ResultadoConsulta.Item("AGoOrigem")
            End With
        Else
            ObjAtendimentoGovVO = New AtendimentoGovVO
            ObjAtendimentoGovVO.AGoId = 0
        End If
        ResultadoConsulta.Close()
        Return ObjAtendimentoGovVO
    End Function

    Public Function ConsultaListaLnkArrumacao(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal Data1 As String, ByVal Data2 As String, ByVal CamId As Long, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("declare ")
                .Append("@Data1 datetime, ")
                .Append("@Data2 datetime, ")
                .Append("@Apto varchar(10), ")
                .Append("@CamId integer ")
                .Append("set @Data1 = convert(datetime, '" & Data1 & " 00:00:00', 103) ")
                .Append("set @Data2 = convert(datetime, '" & Data2 & " 23:59:59', 103) ")
                .Append("set @Apto = '" & ObjAtendimentoGovVO.ApaId & "' ")
                .Append("set @CamId = '" & CamId & "' ")
                .Append("select convert(char(10), a.AGoData, 103) as Data, ")
                .Append("cast(datepart(hh, a.AGoData) as varchar(2)) as Hora, ")
                .Append("a.AGoId, a.ApaId, a.CamId, a.AGoCCasal, a.AGoCSolteiro, a.AGoBerco, ")
                .Append("a.AGoTravesseiro, a.AGoJogoToalhas, a.AGoRoloPapel, a.AGoSacoLixo, ")
                .Append("a.AGoSabonete, a.AGoTapete, a.AGoOrigem, a.AGoObservacao,aa.ApaDesc ")
                .Append("from TbAtendimentoGov a inner join TbApartamento aa ")
                .Append("on a.ApaId = aa.ApaId ")
                .Append("where (a.AGoData between @Data1 and @Data2) ")
                .Append("and aa.ApaDesc like '%' + ")
                .Append("case ")
                .Append("when @Apto = '0' then aa.ApaDesc ")
                .Append("else @Apto ")
                .Append("end + '%' ")
                .Append("and a.CamId =  ")
                .Append("case ")
                .Append("when @CamId = 0 then a.CamId ")
                .Append("else @CamId ")
                .Append("end ")
                '.Append("order by a.AGoData,aa.ApaDesc ")
                .Append("order by aa.ApaDesc ")
                .AppendLine("OPTION (OPTIMIZE FOR(@Data1='2012-01-01',@Data2='2012-01-01')) ")
            End With
            Return PreencheListaLnkArrumacao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheListaLnkArrumacao(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjAtendimentoGovVO = New AtendimentoGovVO
                With ObjAtendimentoGovVO
                    .AGoId = ResultadoConsulta.Item("AgoId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .CheckOut = ResultadoConsulta.Item("Data")
                    .Hora = ResultadoConsulta.Item("Hora")
                    .AGoCCasal = ResultadoConsulta.Item("AGoCCasal")
                    .AGoCSolteiro = ResultadoConsulta.Item("AGoCSolteiro")
                    .AGoBerco = ResultadoConsulta.Item("AGoBerco")
                    .AGoTravesseiro = ResultadoConsulta.Item("AGoTravesseiro")
                    .AGoJogoToalhas = ResultadoConsulta.Item("AGoJogoToalhas")
                    .AGoRoloPapel = ResultadoConsulta.Item("AGoRoloPapel")
                    .AGoSacoLixo = ResultadoConsulta.Item("AGoSacoLixo")
                    .AGoSabonete = ResultadoConsulta.Item("AGoSabonete")
                    .AGoTapete = ResultadoConsulta.Item("AGoTapete")
                    'If ((ResultadoConsulta.Item("AGoObservacao") = "") Or Convert.IsDBNull(ResultadoConsulta.Item("AGoObservacao"))) Then
                    If Convert.IsDBNull(ResultadoConsulta.Item("AGoObservacao")) Then
                        .AGoObservacao = " "
                    Else
                        .AGoObservacao = ResultadoConsulta.Item("AGoObservacao")
                    End If
                    .CamId = ResultadoConsulta.Item("CamId")
                    .AGoOrigem = ResultadoConsulta.Item("AGoOrigem")
                End With
                Lista.Add(ObjAtendimentoGovVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
    Public Function AtualizaAtendimentoArrumacao(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("UPDATE TBATENDIMENTOGOV SET ")
                .Append("CAMID =  " & ObjAtendimentoGovVO.CamId & ", ")
                .Append("AGODATA = GETDATE(), ")
                .Append("AGOCCASAL = " & ObjAtendimentoGovVO.AGoCCasal & ", ")
                .Append("AGOCSOLTEIRO =  " & ObjAtendimentoGovVO.AGoCSolteiro & ", ")
                .Append("AGOBERCO = " & ObjAtendimentoGovVO.AGoBerco & ", ")
                .Append("AGOTRAVESSEIRO = " & ObjAtendimentoGovVO.AGoTravesseiro & " , ")
                .Append("AGOJOGOTOALHAS = " & ObjAtendimentoGovVO.AGoJogoToalhas & ", ")
                .Append("AGOROLOPAPEL = " & ObjAtendimentoGovVO.AGoRoloPapel & " , ")
                .Append("AGOSACOLIXO = " & ObjAtendimentoGovVO.AGoSacoLixo & " , ")
                .Append("AGOSABONETE = " & ObjAtendimentoGovVO.AGoSabonete & ", ")
                .Append("AGOTAPETE = '" & ObjAtendimentoGovVO.AGoTapete & "', ")
                .Append("AGODATALOG = GetDate(), ")
                .Append("AGOUSUARIO = '" & ObjAtendimentoGovVO.Usuario & "', ")
                .Append("AGOOBSERVACAO = RTRIM('" & ObjAtendimentoGovVO.AGoObservacao & "') ")
                .Append("WHERE AGOID = " & ObjAtendimentoGovVO.AGoId & " AND APAID = " & ObjAtendimentoGovVO.ApaId & " ")
                'Penso que aqui deveria mudar o status do apto de arrumação para limpo, ver com o Haas

                .Append("IF @@ERROR = 0 ")
                .Append("BEGIN ")
                .Append("SELECT 1 GOTO SAIDA ")
                .Append("END ")
                .Append("ELSE ")
                .Append("BEGIN ")
                .Append("SELECT 0 GOTO SAIDA ")
                .Append("END ")
                .Append("SAIDA: ")
            End With
            Dim Resultado As Long = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function BuscaAgoOrigem(ByVal ObjAtendimentoGovVO As AtendimentoGovVO, ByVal StatusApartamento As String, ByVal AliasBanco As String) As AtendimentoGovVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("SELECT A.APAID,A.APADESC,T.AGOORIGEM,T.AGOID FROM TBAPARTAMENTO A ")
                .Append("INNER JOIN TBATENDIMENTOGOV T ON A.APAID = T.APAID ")
                .Append("WHERE A.APASTATUS = '" & StatusApartamento & "' ")
                .Append("AND T.AGODATA IS NULL ")
                .Append("AND T.CAMID IS NULL ")
                .Append("AND A.APAID = " & ObjAtendimentoGovVO.ApaId & " ")
            End With
            Return PreencheAgoOrigem(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheAgoOrigem(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As AtendimentoGovVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjAtendimentoGovVO = New AtendimentoGovVO
            With ObjAtendimentoGovVO
                .ApaId = ResultadoConsulta.Item("ApaId")
                .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                .AGoOrigem = ResultadoConsulta.Item("AGoOrigem")
                .AGoId = ResultadoConsulta.Item("AGoId")
            End With
        Else
            ObjAtendimentoGovVO = New AtendimentoGovVO
            With ObjAtendimentoGovVO
                .ApaId = 0
                .ApaDesc = ""
                .AGoOrigem = "0"
                .AGoId = 0
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjAtendimentoGovVO
    End Function
End Class

