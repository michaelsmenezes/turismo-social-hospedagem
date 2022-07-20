Imports System
Imports System.Collections
Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic
Public Class SolicitacaoDAO
    Dim ObjSolicitacaoVO As SolicitacaoVO
    Dim ObjSolicitacaoDAO As SolicitacaoDAO
    'Dim DB As String = "HDManutencaoTst"
    Dim ODBC As String = "BDPROD"
    Public Function Consultar(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal DataIni As String, ByVal DataFim As String, ByVal Formulario As String, ByVal GrupoNutricao As String, ByVal AliasBanco As String) As IList
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("SELECT solId,solCentroCusto,solLocDescricao,solArea,solDescSubLoc,solAssunto,solDescricao,solUsuarioSolicitante,solDisplayNameSolicitante,solDataHoraSolicitacao, ")
            VarSql.AppendLine(" case solSituacao ")
            VarSql.AppendLine("When 'E' then 'Em andamento' ")
            VarSql.AppendLine("When 'C' then 'Encerrada' ")
            VarSql.AppendLine("when 'P' then 'Pendente' ")
            VarSql.AppendLine("End as solSituacao,")
            VarSql.AppendLine("solPrevisaoAtendimento,solDataAtendimento,solMatriculaAtendimento,solNomeAtendimento,solDataLog,solUsuarioLog,solIPUnidade,solPatrimonio,solPatrimonio + ' - ' + solDescBem as solDescBem,solObsManutencao,solCentroCustoSolicitante, ")
            VarSql.AppendLine("solGrauPrioridade,solAvaliacao,solDevolucao,solSetorExecutante,solJustificativaAvaliacao,solBloqueioApartamento ")
            VarSql.AppendLine("FROM TbSolicitacao ")
            'As solicitações feitas pelos centros de custos 144,158,146,154 foram agrupadas para que a Adrieny veja tanto as das lanchonetes quanto do restaurante'
            If GrupoNutricao = "NutricaoCaldasNovas" Then
                VarSql.AppendLine(" WHERE SolCentroCustoSolicitante in (144,146,154,158) ")
            Else
                VarSql.AppendLine(" WHERE SolCentroCustoSolicitante = '" & ObjSolicitacaoVO.CentroCustoSolicitante & "' ")
            End If
            VarSql.AppendLine("AND solAssunto LIKE '%" & ObjSolicitacaoVO.Assunto & "%' ")
            'Se o usuario escolher a opção de filtrar pelo Usuário em "SOLICITAÇÃO, o filtro ira acontecer"'
            If (ObjSolicitacaoVO.UsuarioSolicitante <> "") Then
                VarSql.AppendLine("AND solUsuarioSolicitante = '" & ObjSolicitacaoVO.UsuarioSolicitante & "' ")
            End If
            If (ObjSolicitacaoVO.Situacao <> "") Then 'Caso contrário pega todas as situações, Em Andamento e concluida sem avaliação'
                'Nesse caso esta filtrando apenas as solicitações pendentes'
                If (ObjSolicitacaoVO.Situacao = "P") Then
                    VarSql.AppendLine("AND SolSituacao = 'P' ")
                ElseIf (ObjSolicitacaoVO.Situacao = "E") Then
                    'Nesse caso irá mostrar todas as solicitações em andamento, pendente e encerradas mais em baixo valida as encerradas que ainda não foi avaliada.'
                    VarSql.AppendLine("AND solSituacao IN ('" & ObjSolicitacaoVO.Situacao & "','P','C') ")
                ElseIf (ObjSolicitacaoVO.Situacao = "C") Then
                    VarSql.AppendLine("AND solSituacao IN ('" & ObjSolicitacaoVO.Situacao & "') ")
                Else
                    'VarSql.AppendLine("AND solSituacao IN ('" & ObjSolicitacaoVO.Situacao & "') ")
                End If
            End If
            'NO CASO DA SITUAÇÃO EM ANDAMENTO, IRÁ MOSTRAR TODAS INDEPENDENTE DE DATA'
            If (DataIni <> "" And DataFim <> "" And ObjSolicitacaoVO.Situacao <> "E") Then
                VarSql.AppendLine("AND Convert(DateTime,solDataHoraSolicitacao,103) >= Convert(DateTime,'" & DataIni & " 00:00:00',103) ")
                VarSql.AppendLine("AND Convert(DateTime,solDataHoraSolicitacao,103) <= Convert(DateTime,'" & DataFim & " 23:59:59',103) ")
            End If
            'Se for as solicitação em aberto, vai filtrar as avaliações em branco'
            If (ObjSolicitacaoVO.Situacao = "E") Then
                VarSql.AppendLine("AND solAvaliacao = '0' ")
            ElseIf (ObjSolicitacaoVO.Situacao = "C") Then
                VarSql.AppendLine("AND solAvaliacao <> '0' ")
            End If
            If (ObjSolicitacaoVO.CentroCusto <> "0") Then
                VarSql.AppendLine("AND solCentroCusto = '" & ObjSolicitacaoVO.CentroCusto & "' ")
            End If
            VarSql.AppendLine("Order by solDataHoraSolicitacao Desc ")
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Public Function ConsultarAcompanhamento(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal DataIni As String, ByVal DataFim As String, ByVal Formulario As String, ByVal AliasBanco As String) As IList
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("SELECT solId,solCentroCusto,solLocDescricao,solArea,solDescSubLoc,solAssunto,solDescricao,solUsuarioSolicitante,solDisplayNameSolicitante,solDataHoraSolicitacao, ")
            VarSql.AppendLine(" case solSituacao ")
            VarSql.AppendLine("When 'E' then 'Em andamento' ")
            VarSql.AppendLine("When 'C' then 'Encerrada' ")
            VarSql.AppendLine("when 'P' then 'Pendente' ")
            VarSql.AppendLine("End as solSituacao,")
            VarSql.AppendLine("solPrevisaoAtendimento,solDataAtendimento,solMatriculaAtendimento,solNomeAtendimento,solDataLog,solUsuarioLog,solIPUnidade,solPatrimonio,solPatrimonio + ' - ' + solDescBem as solDescBem,solObsManutencao,solCentroCustoSolicitante, ")
            VarSql.AppendLine("solGrauPrioridade,solAvaliacao,solDevolucao,solSetorExecutante,solJustificativaAvaliacao,SolbloqueioApartamento ")
            VarSql.AppendLine("FROM TbSolicitacao ")
            'Quando for acompanhamento o Funcionário da Manutenção irá ver todas as solicitações abertas'
            VarSql.AppendLine(" WHERE solAssunto LIKE '%" & ObjSolicitacaoVO.Assunto & "%' ")
            If (ObjSolicitacaoVO.Situacao <> "") Then
                'Em andamento deve mostrar as que estão em andamento mais as que estão paralizadas'
                Select Case ObjSolicitacaoVO.Situacao
                    Case "E"
                        VarSql.AppendLine("AND solSituacao = 'E' ")
                    Case "P"
                        VarSql.AppendLine("AND solSituacao = 'P' ")
                    Case "C" 'Pendente e Em Andamento
                        VarSql.AppendLine("AND solSituacao = 'C' ")
                    Case "PE" 'Pendente e Em Andamento
                        VarSql.AppendLine("AND solSituacao IN ('E','P') ")
                    Case Else
                        VarSql.AppendLine("AND solSituacao IN ('E','P','C') ")
                End Select

            End If
            'Se desejar ver apenas as solicitações de um determinado técnico'
            If (ObjSolicitacaoVO.MatriculaAtendimento <> "0") Then
                VarSql.AppendLine("AND solMatriculaAtendimento = '" & ObjSolicitacaoVO.MatriculaAtendimento & "' ")
            End If
            If (DataIni <> "" And DataFim <> "") Then
                VarSql.AppendLine("AND Convert(DateTime,solDataHoraSolicitacao,103) >= Convert(DateTime,'" & DataIni & " 00:00:00',103) ")
                VarSql.AppendLine("AND Convert(DateTime,solDataHoraSolicitacao,103) <= Convert(DateTime,'" & DataFim & " 23:59:59',103) ")
            End If
            VarSql.AppendLine("AND Substring(solIPUnidade,5,2) = '" & ObjSolicitacaoVO.IpUnidade & "' ")
            VarSql.AppendLine("AND solSetorExecutante = '" & ObjSolicitacaoVO.SetorExecutante & "' ")
            VarSql.AppendLine("Order by solDataHoraSolicitacao Desc ")
            Return PreencheLista(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If (ResultadoConsulta.HasRows) Then
            While (ResultadoConsulta.Read)
                ObjSolicitacaoVO = New SolicitacaoVO
                ObjSolicitacaoVO.solId = ResultadoConsulta.Item("solId")
                ObjSolicitacaoVO.CentroCusto = ResultadoConsulta.Item("solCentroCusto")
                ObjSolicitacaoVO.LocDescricao = ResultadoConsulta.Item("solLocDescricao")
                ObjSolicitacaoVO.Area = ResultadoConsulta.Item("solArea")
                ObjSolicitacaoVO.DescSubLoc = ResultadoConsulta.Item("solDescSubLoc")
                ObjSolicitacaoVO.Assunto = ResultadoConsulta.Item("solAssunto")
                ObjSolicitacaoVO.Descricao = ResultadoConsulta.Item("solDescricao")
                ObjSolicitacaoVO.UsuarioSolicitante = ResultadoConsulta.Item("solUsuarioSolicitante")
                ObjSolicitacaoVO.DisplayNameSolicitante = ResultadoConsulta.Item("solDisplayNameSolicitante")
                ObjSolicitacaoVO.DataHoraSolicitacao = ResultadoConsulta.Item("solDataHoraSolicitacao")
                ObjSolicitacaoVO.Situacao = ResultadoConsulta.Item("solSituacao")
                If Not Convert.IsDBNull(ResultadoConsulta.Item("solPrevisaoAtendimento")) Then
                    ObjSolicitacaoVO.PrevisaoAtendimento = ResultadoConsulta.Item("solPrevisaoAtendimento")
                Else
                    ObjSolicitacaoVO.PrevisaoAtendimento = ""
                End If

                If Not Convert.IsDBNull(ResultadoConsulta.Item("solDataAtendimento")) Then
                    ObjSolicitacaoVO.DataAtendimento = ResultadoConsulta.Item("solDataAtendimento")
                Else
                    ObjSolicitacaoVO.DataAtendimento = ""
                End If
                ObjSolicitacaoVO.MatriculaAtendimento = ResultadoConsulta.Item("solMatriculaAtendimento")
                ObjSolicitacaoVO.NomeFuncAtendimento = ResultadoConsulta.Item("solNomeAtendimento")
                ObjSolicitacaoVO.DataLog = ResultadoConsulta.Item("solDataLog")
                ObjSolicitacaoVO.UsuarioLog = ResultadoConsulta.Item("solUsuarioLog")
                ObjSolicitacaoVO.Patrimonio = ResultadoConsulta.Item("solPatrimonio")
                ObjSolicitacaoVO.DesBem = ResultadoConsulta.Item("solDescBem")
                ObjSolicitacaoVO.ObsManutencao = ResultadoConsulta.Item("solObsManutencao")
                ObjSolicitacaoVO.CentroCustoSolicitante = ResultadoConsulta.Item("solCentroCustoSolicitante")
                ObjSolicitacaoVO.GrauPrioridade = ResultadoConsulta.Item("solGrauPrioridade")
                ObjSolicitacaoVO.Avaliacao = ResultadoConsulta.Item("solAvaliacao")
                ObjSolicitacaoVO.Devolucao = ResultadoConsulta.Item("solDevolucao")
                ObjSolicitacaoVO.SetorExecutante = ResultadoConsulta.Item("solSetorExecutante")
                ObjSolicitacaoVO.JustificativaAvaliacao = ResultadoConsulta.Item("solJustificativaAvaliacao")
                If Convert.IsDBNull(ResultadoConsulta.Item("SolbloqueioApartamento")) Then
                    ObjSolicitacaoVO.BloqueioApartamento = "N"
                Else
                    ObjSolicitacaoVO.BloqueioApartamento = ResultadoConsulta.Item("SolbloqueioApartamento")
                End If
                Lista.Add(ObjSolicitacaoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return (Lista)
    End Function
    Public Function ConsultarCodigo(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal AliasBanco As String) As SolicitacaoVO
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("SELECT solId,solCentroCusto,solLocDescricao,solArea,solDescSubLoc,solAssunto,solDescricao,solUsuarioSolicitante,solDisplayNameSolicitante,solDataHoraSolicitacao, ")
            VarSql.AppendLine(" case solSituacao ")
            VarSql.AppendLine("When 'E' then 'Em andamento' ")
            VarSql.AppendLine("When 'C' then 'Encerrada' ")
            VarSql.AppendLine("when 'P' then 'Pendente' ")
            VarSql.AppendLine("End as solSituacao,")
            VarSql.AppendLine("solPrevisaoAtendimento,solNomeAtendimento,solDataAtendimento,solMatriculaAtendimento,solDataLog,solUsuarioLog,solIPUnidade,solPatrimonio,solPatrimonio + ' - ' + solDescBem as solDescBem,solObsManutencao,solCentroCustoSolicitante, ")
            VarSql.AppendLine("solGrauPrioridade, solAvaliacao,solDevolucao,solSetorExecutante,solJustificativaAvaliacao,solBloqueioApartamento ")
            VarSql.AppendLine("FROM TbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & " ")
            Return PreencheObjeto(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheObjeto(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As SolicitacaoVO
        If (ResultadoConsulta.HasRows) Then
            ResultadoConsulta.Read()
            ObjSolicitacaoVO = New SolicitacaoVO
            ObjSolicitacaoVO.solId = ResultadoConsulta.Item("solId")
            ObjSolicitacaoVO.CentroCusto = ResultadoConsulta.Item("solCentroCusto")
            ObjSolicitacaoVO.LocDescricao = ResultadoConsulta.Item("solLocDescricao")
            ObjSolicitacaoVO.Area = ResultadoConsulta.Item("solArea")
            ObjSolicitacaoVO.DescSubLoc = ResultadoConsulta.Item("solDescSubLoc")
            ObjSolicitacaoVO.Assunto = ResultadoConsulta.Item("solAssunto")
            ObjSolicitacaoVO.Descricao = ResultadoConsulta.Item("solDescricao")
            ObjSolicitacaoVO.UsuarioSolicitante = ResultadoConsulta.Item("solUsuarioSolicitante")
            ObjSolicitacaoVO.DisplayNameSolicitante = ResultadoConsulta.Item("solDisplayNameSolicitante")
            ObjSolicitacaoVO.DataHoraSolicitacao = ResultadoConsulta.Item("solDataHoraSolicitacao")
            ObjSolicitacaoVO.Situacao = ResultadoConsulta.Item("solSituacao")
            If Not Convert.IsDBNull(ResultadoConsulta.Item("solPrevisaoAtendimento")) Then
                ObjSolicitacaoVO.PrevisaoAtendimento = ResultadoConsulta.Item("solPrevisaoAtendimento")
            Else
                ObjSolicitacaoVO.PrevisaoAtendimento = ""
            End If

            If Not Convert.IsDBNull(ResultadoConsulta.Item("solDataAtendimento")) Then
                ObjSolicitacaoVO.DataAtendimento = ResultadoConsulta.Item("solDataAtendimento")
            Else
                ObjSolicitacaoVO.DataAtendimento = ""
            End If

            ObjSolicitacaoVO.MatriculaAtendimento = ResultadoConsulta.Item("solMatriculaAtendimento")
            ObjSolicitacaoVO.NomeFuncAtendimento = ResultadoConsulta.Item("solNomeAtendimento")
            ObjSolicitacaoVO.DataLog = ResultadoConsulta.Item("solDataLog")
            ObjSolicitacaoVO.UsuarioLog = ResultadoConsulta.Item("solUsuarioLog")
            ObjSolicitacaoVO.Patrimonio = ResultadoConsulta.Item("solPatrimonio")
            ObjSolicitacaoVO.DesBem = ResultadoConsulta.Item("solDescBem")
            ObjSolicitacaoVO.ObsManutencao = ResultadoConsulta.Item("solObsManutencao")
            ObjSolicitacaoVO.CentroCustoSolicitante = ResultadoConsulta.Item("solCentroCustoSolicitante")
            ObjSolicitacaoVO.GrauPrioridade = ResultadoConsulta.Item("solGrauPrioridade")
            ObjSolicitacaoVO.Avaliacao = ResultadoConsulta.Item("solAvaliacao")
            ObjSolicitacaoVO.Devolucao = ResultadoConsulta.Item("solDevolucao")
            ObjSolicitacaoVO.SetorExecutante = ResultadoConsulta.Item("solSetorExecutante")
            ObjSolicitacaoVO.JustificativaAvaliacao = ResultadoConsulta.Item("solJustificativaAvaliacao")
            If Convert.IsDBNull(ResultadoConsulta.Item("solBloqueioApartamento")) = True Or IsNothing(ResultadoConsulta.Item("solBloqueioApartamento")) = True Then
                ObjSolicitacaoVO.BloqueioApartamento = "N"
            ElseIf ResultadoConsulta.Item("solBloqueioApartamento") = "N" Then
                ObjSolicitacaoVO.BloqueioApartamento = "N"
            Else
                ObjSolicitacaoVO.BloqueioApartamento = "S"
            End If
        End If
        ResultadoConsulta.Close()
        Return ObjSolicitacaoVO
    End Function
    Public Function Inserir(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal AliasBanco As String, ByVal BloqueioApartamento As String, ByVal ObjBloqueioAptoVO As BloqueioAptoVO, ByVal BancoTurismo As String) As Long
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder

            Dim Servidor As String = ""
            Dim Banco As String = ""

            If BancoTurismo = "TurismoSocialCaldas" Then
                Servidor = "SQL-CTL"
                Banco = "DbTurismoSocial"
            ElseIf BancoTurismo = "TurismoSocialPiri" Then
                Servidor = "SQL-PSP"
                Banco = "dbTurismoSocialPiri"
            End If

            'If BancoTurismo = "TurismoSocialCaldas" Then
            '    Servidor = "HOM_TURISMO_SOCIAL_CALD"
            '    Banco = "DbTurismoSocial"
            'ElseIf BancoTurismo = "TurismoSocialPiri" Then
            '    Servidor = "HOM_TURISMO_SOCIAL_PIRI"
            '    Banco = "dbTurismoSocialPiri"
            'End If

            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("SET XACT_ABORT ON ") 'Permite fazer transações distribuidas via linked server
            VarSql.AppendLine("IF NOT EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("Declare @SolIdManutencao as Numeric(18) ")
            VarSql.AppendLine("Declare @SolIdTurismo as Numeric(18) ")

            VarSql.AppendLine("INSERT INTO TbSolicitacao(solCentroCusto,solLocDescricao,solArea,solDescSubLoc,solAssunto,solDescricao,solUsuarioSolicitante,solDataHoraSolicitacao, ")
            VarSql.AppendLine("solSituacao, solPrevisaoAtendimento, solDataAtendimento, solMatriculaAtendimento, solDataLog, solUsuarioLog,solIPUnidade,solDisplayNameSolicitante,solPatrimonio,solDescBem,solObsManutencao,solNomeAtendimento,solCentroCustoSolicitante,solGrauPrioridade,solAvaliacao,solDevolucao,solSetorExecutante,solJustificativaAvaliacao,solBloqueioApartamento,solIdUnidade,solBloqueioApartamentoDataInicial) VALUES ")
            VarSql.AppendLine("('" & ObjSolicitacaoVO.CentroCusto.Trim & "','" & ObjSolicitacaoVO.LocDescricao.Trim & "','" & ObjSolicitacaoVO.Area.Trim & "','" & ObjSolicitacaoVO.DescSubLoc.Trim & "','" & ObjSolicitacaoVO.Assunto.Trim & "','" & ObjSolicitacaoVO.Descricao & "','" & ObjSolicitacaoVO.UsuarioSolicitante & "','" & String.Format(CDate(ObjSolicitacaoVO.DataHoraSolicitacao), "yyyy-MM-dd ") & TimeOfDay & "', ")
            VarSql.AppendLine("'" & ObjSolicitacaoVO.Situacao & "','" & ObjSolicitacaoVO.PrevisaoAtendimento & "','" & ObjSolicitacaoVO.DataAtendimento & "'  ,'" & ObjSolicitacaoVO.MatriculaAtendimento & "','" & String.Format(CDate(ObjSolicitacaoVO.DataLog), "yyyy-MM-dd ") & TimeOfDay & "','" & ObjSolicitacaoVO.UsuarioLog & "','" & ObjSolicitacaoVO.IpUnidade & "','" & ObjSolicitacaoVO.DisplayNameSolicitante & "', ")
            VarSql.AppendLine("'" & ObjSolicitacaoVO.Patrimonio & "','" & ObjSolicitacaoVO.DesBem & "','" & ObjSolicitacaoVO.ObsManutencao & "','" & ObjSolicitacaoVO.NomeFuncAtendimento & "','" & ObjSolicitacaoVO.CentroCustoSolicitante & "','" & ObjSolicitacaoVO.GrauPrioridade & "','" & ObjSolicitacaoVO.Avaliacao & "','" & ObjSolicitacaoVO.Devolucao & "','" & ObjSolicitacaoVO.SetorExecutante & "','" & ObjSolicitacaoVO.JustificativaAvaliacao & "','" & ObjSolicitacaoVO.BloqueioApartamento & "','" & ObjSolicitacaoVO.IdUnidade & "','" & String.Format(CDate(ObjSolicitacaoVO.DataHoraSolicitacao), "yyyy-MM-dd 12:00:00") & "') ")

            VarSql.AppendLine("Select top 1 @SolIdManutencao = SolId from TbSolicitacao Where solCentroCusto = '" & ObjSolicitacaoVO.CentroCusto & " ' and solArea = '" & ObjSolicitacaoVO.Area & "' order by solid desc ")

            If BloqueioApartamento = "S" Then
                'Botando o apartamento em manutenção se estiver Limpo ou em Arrumação
                VarSql.AppendLine("DECLARE @ERRO NUMERIC,@Dia smallint, @ROW NUMERIC, @ACM NUMERIC ")
                VarSql.AppendLine("SET @Dia = " & ObjBloqueioAptoVO.Dia & " ")
                VarSql.AppendLine("IF ('" & ObjBloqueioAptoVO.Acao & "' = 'M') AND ('" & ObjBloqueioAptoVO.AlteraManutencao & "' = '0') ")
                VarSql.AppendLine("BEGIN ")
                VarSql.AppendLine("  UPDATE [" & Servidor & "].[" & Banco & "].[DBO].TBAPARTAMENTO ")
                'VarSql.AppendLine("  UPDATE [" & Banco & "].[DBO].TBAPARTAMENTO ")
                VarSql.AppendLine("  SET APASTATUS = ")
                VarSql.AppendLine("    CASE ")
                VarSql.AppendLine("      WHEN APASTATUS IN ('L','A') THEN 'M' ")
                VarSql.AppendLine("      ELSE APASTATUS ")
                VarSql.AppendLine("    END, ")
                VarSql.AppendLine("   APAUSUARIO ='" & ObjBloqueioAptoVO.Usuario & "', APAUSUARIODATA = GETDATE() ")
                VarSql.AppendLine("   WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " ")
                VarSql.AppendLine("   SET @ERRO = @@ERROR ")
                VarSql.AppendLine("   SET @ROW = @@ROWCOUNT ")
                VarSql.AppendLine(" IF (@ERRO = 0) AND (@ROW > 0) ")
                VarSql.AppendLine("   BEGIN ")
                '.AppendLine("INSERT TBMANUTENCAO (MANDATAABERTURA, APAID, MANREQUISITANTE, MANDESCRICAOREQUIS)")
                '.AppendLine("VALUES (GETDATE()," & ObjBloqueioAptoVO.ApaId & ",'" & ObjBloqueioAptoVO.Usuario & "','" & ObjBloqueioAptoVO.ManDescricaoResquisitante & "') ")
                VarSql.AppendLine("     SET @ERRO = @@ERROR ")
                VarSql.AppendLine("     SET @ROW = @@ROWCOUNT ")
                VarSql.AppendLine("  IF (@ERRO = 0) AND (@ROW > 0) AND (@DIA >= 1) ")
                VarSql.AppendLine("    BEGIN ")
                'Inserindo a solicitação em TurismoSocial
                'VarSql.AppendLine("       SET @ACM = ISNULL((SELECT TOP 1 ACMID FROM [" & Servidor & "].[" & Banco & "].[DBO].TBAPARTAMENTO WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " AND APASTATUS <> 'O'), 0) ")
                VarSql.AppendLine("       SET @ACM = ISNULL((SELECT TOP 1 ACMID FROM [" & Servidor & "].[" & Banco & "].[DBO].TBAPARTAMENTO WHERE APAID = " & ObjBloqueioAptoVO.ApaId & " ), 0) ")
                VarSql.AppendLine("          IF @ACM <> 0 AND @Dia > 0")
                VarSql.AppendLine("            BEGIN ")
                VarSql.AppendLine("               IF NOT EXISTS (SELECT 1 FROM [" & Servidor & "].[" & Banco & "].[DBO].TBSOLICITACAO WHERE RESID IS NULL AND APAID = " & ObjBloqueioAptoVO.ApaId & ") ")
                VarSql.AppendLine("                  BEGIN ")
                VarSql.AppendLine("                    INSERT [" & Servidor & "].[" & Banco & "].[DBO].TBSOLICITACAO (ACMID, ACMIDCOBRANCA, APAID, SOLDATAINI, SOLHORAINI, SOLDATAFIM, SOLHORAFIM, ")
                VarSql.AppendLine("                     SOLDATAINIAUX, SOLDATAFIMAUX, SOLUSUARIO, SOLUSUARIODATA) VALUES(")
                VarSql.AppendLine("                     @ACM,@ACM," & ObjBloqueioAptoVO.ApaId & " ,Convert(char(10),GetDate(),120 ) + ' 12:00:00','" & 12 & "',")
                VarSql.AppendLine("                     cast(Convert(char(10),GetDate(),120 ) + ' 12:00:00' as DateTime) + " & ObjBloqueioAptoVO.Dia & " ,'" & 12 & "',")
                VarSql.AppendLine("                     Convert(char(10),GetDate(),120 ) + ' 12:00:00', cast(Convert(char(10),GetDate(),120 ) + ' 12:00:00' as DateTime) + " & ObjBloqueioAptoVO.Dia & ",'" & ObjBloqueioAptoVO.Usuario & "',GETDATE()) ")
                VarSql.AppendLine("Select top 1 @SolIdTurismo = solId from [" & Servidor & "].[" & Banco & "].[DBO].TBSOLICITACAO WHERE ApaId = " & ObjBloqueioAptoVO.ApaId & " and resId is Null order by solId Desc ")
                'Pegando o Id da Solicitação em Turismo Social e Inserindo no tbSolicitação da Manutenção para update futuro
                VarSql.AppendLine("                    Update tbSolicitacao set SolIdTurismo = @SolIdTurismo where solId = @SolIdManutencao ")
                'VarSql.AppendLine("                     Update tbSolicitacao set SolIdTurismo = (Select top 1 solId from [" & Banco & "].[DBO].TbSolicitacao order by solid desc) where solId = (select top 1 solId from tbsolicitacao order by solid desc) and solCentroCusto = '" & ObjSolicitacaoVO.CentroCusto & " ' and solArea = '" & ObjSolicitacaoVO.Area & "' ")
                VarSql.AppendLine("                     SET @ERRO = @@ERROR ")
                VarSql.AppendLine("                     SET @ROW = @@ROWCOUNT ")
                VarSql.AppendLine("                  END ")
                VarSql.AppendLine("            END ")
                VarSql.AppendLine("    END ")
                VarSql.AppendLine("   END ")
                VarSql.AppendLine("END ")
            End If

            VarSql.AppendLine("SELECT 1 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("Update TbSolicitacao SET solCentroCusto='" & ObjSolicitacaoVO.CentroCusto & "',solLocDescricao='" & ObjSolicitacaoVO.LocDescricao & "',solArea='" & ObjSolicitacaoVO.Area & "',solDescSubLoc='" & ObjSolicitacaoVO.DescSubLoc & "',solAssunto='" & ObjSolicitacaoVO.Assunto & "',solDescricao='" & ObjSolicitacaoVO.Descricao & "',solUsuarioSolicitante='" & ObjSolicitacaoVO.UsuarioSolicitante & "',solDataHoraSolicitacao='" & String.Format(CDate(ObjSolicitacaoVO.DataHoraSolicitacao), "yyyy-MM-dd ") & TimeOfDay & "',solCentroCustoSolicitante =' " & ObjSolicitacaoVO.CentroCustoSolicitante & "',")
            VarSql.AppendLine("solSituacao='" & ObjSolicitacaoVO.Situacao & "', solPrevisaoAtendimento='" & ObjSolicitacaoVO.PrevisaoAtendimento & "', solDataAtendimento='" & ObjSolicitacaoVO.DataAtendimento & "', solMatriculaAtendimento='" & ObjSolicitacaoVO.MatriculaAtendimento & "',solIPUnidade='" & ObjSolicitacaoVO.IpUnidade & "',solDisplayNameSolicitante = '" & ObjSolicitacaoVO.DisplayNameSolicitante & "',solPatrimonio='" & ObjSolicitacaoVO.Patrimonio & "',solDescBem='" & ObjSolicitacaoVO.DesBem & "',solObsManutencao='" & ObjSolicitacaoVO.ObsManutencao & "',solNomeAtendimento='" & ObjSolicitacaoVO.NomeFuncAtendimento & "', ")
            VarSql.AppendLine("solGrauPrioridade ='" & ObjSolicitacaoVO.GrauPrioridade & "',solDevolucao='" & ObjSolicitacaoVO.Devolucao & "',solSetorExecutante='" & ObjSolicitacaoVO.SetorExecutante & "',solJustificativaAvaliacao='" & ObjSolicitacaoVO.JustificativaAvaliacao & "',solBloqueioApartamento='" & ObjSolicitacaoVO.BloqueioApartamento & "',solIdUnidade='" & ObjSolicitacaoVO.IdUnidade & "'  ")
            VarSql.AppendLine("WHERE solId = " & ObjSolicitacaoVO.solId & " ")
            VarSql.AppendLine("SELECT 2 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF @@Error > 0 GOTO ERRO ")
            VarSql.AppendLine("SELECT 3 GOTO SAIDA ")
            VarSql.AppendLine("ERRO: SELECT 0 GOTO SAIDA ")
            VarSql.AppendLine("ERRO2: SELECT -1 GOTO SAIDA ")
            VarSql.AppendLine("SAIDA: ")

            'GravaLog("Setença: " & VarSql.ToString)

            Dim Resultado As Long = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            GravaLog("Deu erro na função inserir: " & ex.StackTrace.ToString)
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function InserirAcompanhamento(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal UnidadeOperacional As String, ByVal AliasBanco As String) As Long
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("IF NOT EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("INSERT INTO TbSolicitacao(solPrevisaoAtendimento,solMatriculaAtendimento,solSituacao,solObsManutencao,solNomeAtendimento,solCentroCustoSolicitante,solGrauPrioridade) values ")
            VarSql.AppendLine("('" & String.Format(CDate(ObjSolicitacaoVO.PrevisaoAtendimento), "yyyy-MM-dd ") & TimeOfDay & "','" & ObjSolicitacaoVO.MatriculaAtendimento & "','" & ObjSolicitacaoVO.Situacao & "','" & ObjSolicitacaoVO.ObsManutencao & "','" & ObjSolicitacaoVO.NomeFuncAtendimento & "','" & ObjSolicitacaoVO.CentroCustoSolicitante & "','" & ObjSolicitacaoVO.GrauPrioridade & "') ")
            VarSql.AppendLine("SELECT 1 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("Update TbSolicitacao SET solPrevisaoAtendimento='" & String.Format(CDate(ObjSolicitacaoVO.PrevisaoAtendimento), "yyyy-MM-dd HH:mm:ss") & "',solMatriculaAtendimento='" & ObjSolicitacaoVO.MatriculaAtendimento & "',solSituacao='" & ObjSolicitacaoVO.Situacao & "',solObsManutencao='" & ObjSolicitacaoVO.ObsManutencao & "',solDataAtendimento='" & String.Format(CDate(ObjSolicitacaoVO.DataAtendimento), "yyyy-MM-dd " & TimeOfDay) & "', ")
            VarSql.AppendLine("SolNomeAtendimento='" & ObjSolicitacaoVO.NomeFuncAtendimento & "',solCentroCustoSolicitante='" & Trim(ObjSolicitacaoVO.CentroCustoSolicitante) & "',solGrauPrioridade = '" & ObjSolicitacaoVO.GrauPrioridade & "' ")
            'If estiver concluindo, irá retirar o bloqueio
            If ObjSolicitacaoVO.Situacao = "C" Then
                VarSql.AppendLine(",SolBloqueioApartamento = 'N' ")
            End If
            VarSql.AppendLine("WHERE solId = " & ObjSolicitacaoVO.solId & " ")
            'Se não Existir mais nenhum bloqueio para o apto em questão, irá voltá-lo ao Status de Arrumação e sairá da manutenção
            VarSql.AppendLine("IF NOT EXISTS(SELECT * FROM TBSOLICITACAO WHERE SOLCENTROCUSTO = '" & ObjSolicitacaoVO.CentroCusto & "' AND SOLAREA = " & ObjSolicitacaoVO.Area & " AND SOLBLOQUEIOAPARTAMENTO = 'S') ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("UPDATE [" & UnidadeOperacional & "].[DBO].TBAPARTAMENTO SET APASTATUS = 'A' WHERE APACUSTO = '" & ObjSolicitacaoVO.CentroCusto & "' AND APAAREA= " & ObjSolicitacaoVO.Area & " ")
            VarSql.AppendLine("End ")
            VarSql.AppendLine("SELECT 2 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF @@Error > 0 GOTO ERRO ")
            VarSql.AppendLine("SELECT 3 GOTO SAIDA ")
            VarSql.AppendLine("ERRO: SELECT 0 GOTO SAIDA ")
            VarSql.AppendLine("ERRO2: SELECT -1 GOTO SAIDA ")
            VarSql.AppendLine("SAIDA: ")
            Dim Resultado As Long = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function InserirAvaliacao(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal AliasBanco As String) As Long
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("IF EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("Update TbSolicitacao SET solAvaliacao ='" & ObjSolicitacaoVO.Avaliacao & "',solJustificativaAvaliacao=' " & ObjSolicitacaoVO.JustificativaAvaliacao & "' ")
            VarSql.AppendLine("WHERE solId = " & ObjSolicitacaoVO.solId & " ")
            VarSql.AppendLine("SELECT 2 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF @@Error > 0 GOTO ERRO ")
            VarSql.AppendLine("SELECT 3 GOTO SAIDA ")
            VarSql.AppendLine("ERRO: SELECT 0 GOTO SAIDA ")
            VarSql.AppendLine("ERRO2: SELECT -1 GOTO SAIDA ")
            VarSql.AppendLine("SAIDA: ")
            Dim Resultado As Long = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function InserirDevolucao(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal AliasBanco As String) As Long
        Try
            'Conn = New Conexao(Db, LocalWebService)
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("IF EXISTS (SELECT * FROM tbSolicitacao WHERE solId = " & ObjSolicitacaoVO.solId & ") ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("Update TbSolicitacao SET solDevolucao ='" & ObjSolicitacaoVO.Devolucao & "',SolSituacao ='" & ObjSolicitacaoVO.Situacao & "' ")
            VarSql.AppendLine("WHERE solId = " & ObjSolicitacaoVO.solId & " ")
            VarSql.AppendLine("SELECT 2 GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("IF @@Error > 0 GOTO ERRO ")
            VarSql.AppendLine("SELECT 3 GOTO SAIDA ")
            VarSql.AppendLine("ERRO: SELECT 0 GOTO SAIDA ")
            VarSql.AppendLine("ERRO2: SELECT -1 GOTO SAIDA ")
            VarSql.AppendLine("SAIDA: ")
            Dim Resultado As Long = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Public Function ConsultaPendeciaAvaliacao(ByVal ObjSolicitacaoVO As SolicitacaoVO, ByVal AliasBanco As String) As SolicitacaoVO
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql As Text.StringBuilder
            VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            VarSql.AppendLine("IF EXISTS (SELECT SOLID,SOLDESCRICAO FROM tbsolicitacao WHERE solUsuarioSolicitante ='" & ObjSolicitacaoVO.UsuarioSolicitante & "' and solSituacao = 'C' and solAvaliacao = '0') ")
            VarSql.AppendLine("BEGIN ")
            'BAIXANDO AS SOLICITAÇÕES ENCERRADAS E NÃO AVALIADAS POR MAIS DE TRES DIAS - 'S'--> SISTEMA
            VarSql.AppendLine("UPDATE TBSOLICITACAO SET SOLAVALIACAO = 'S',SOLJUSTIFICATIVAAVALIACAO ='ENCERRADA SEM AVALIAÇÃO DO USUÁRIO' ")
            VarSql.AppendLine("from [DbRestauranteServidores].[dbo].TbFuncionarios F ")
            VarSql.AppendLine("INNER JOIN TbSolicitacao S on s.solCentroCustoSolicitante  = f.funCentroCusto ")
            VarSql.AppendLine("Where(S.solCentroCustoSolicitante = '" & ObjSolicitacaoVO.CentroCustoSolicitante & "') ")
            VarSql.AppendLine("and S.solSituacao = 'C' and S.solAvaliacao = '0' ")
            VarSql.AppendLine("AND DATEDIFF(DAY,S.solDataAtendimento,GETDATE()) > 3 ")
            'VarSql.AppendLine("UPDATE TBSOLICITACAO SET SOLAVALIACAO = 'S',SOLJUSTIFICATIVAAVALIACAO ='ENCERRADA SEM AVALIAÇÃO DO USUÁRIO' WHERE DateDiff(DAY,SOLDATAATENDIMENTO,GETDATE()) > 3 ")
            'VarSql.AppendLine("AND SOLUSUARIOSOLICITANTE  ='" & ObjSolicitacaoVO.UsuarioSolicitante & "' AND SOLSITUACAO = 'C' and solAvaliacao = '0'")
            'VERIFICANDO SE FICOU ALGUMA SOLICITACAO ENCERRADA SEM AVALIAÇÃO
            VarSql.AppendLine("SELECT Count(*) as NaoAvaliadas FROM tbsolicitacao WHERE solUsuarioSolicitante  ='" & ObjSolicitacaoVO.UsuarioSolicitante & "' and solSituacao = 'C' and solAvaliacao = '0' GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("ELSE ")
            VarSql.AppendLine("BEGIN ")
            VarSql.AppendLine("SELECT 0 as NaoAvaliadas GOTO SAIDA ")
            VarSql.AppendLine("END ")
            VarSql.AppendLine("SAIDA: ")
            Return PreencheTotal(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheTotal(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As SolicitacaoVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjSolicitacaoVO = New SolicitacaoVO
            ObjSolicitacaoVO.TotalSolSemAvaliacao = ResultadoConsulta.Item("NaoAvaliadas")
        End If
        ResultadoConsulta.Close()
        Return ObjSolicitacaoVO
    End Function

    Public Function BuscaInformacoesAptoBDProd(ByVal ApaCCusto As String, ByVal ApaArea As Long) As SolicitacaoVO
        Try
            Dim Conn = New Banco.Conexao(ODBC)
            Dim VarSql = New Text.StringBuilder
            With VarSql
                .AppendLine("SELECT A.CDCCUSTO,A.CDAREA,A.NMAREA,C.SGCCUSTO,C.NMCCUSTO ")
                .AppendLine("FROM AREA A ")
                .AppendLine("INNER JOIN CCUSTO C ON C.CDCCUSTO = A.CDCCUSTO ")
                .AppendLine("WHERE A.CDCCUSTO = '" & ApaCCusto & "' ")
                .AppendLine("AND A.CDAREA = " & ApaArea & " ")
                .AppendLine("ORDER BY NMAREA ")
            End With
            Return PreencheAptoPorId(Conn.executaOdbc(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheAptoPorId(ByVal Dt As System.Data.DataTable) As SolicitacaoVO
        If Dt.Rows.Count <> 0 Then
            For Each DtRow As System.Data.DataRow In Dt.Rows
                ObjSolicitacaoVO = New SolicitacaoVO
                If Dt.Columns.Contains("CDCCUSTO") Then
                    If Not Convert.IsDBNull(DtRow.Item("CDCCUSTO")) Then
                        ObjSolicitacaoVO.CentroCusto = DtRow.Item("CDCCUSTO")
                    Else
                        ObjSolicitacaoVO.CentroCusto = ""
                    End If
                End If

                If Dt.Columns.Contains("CDAREA") Then
                    If Not Convert.IsDBNull(DtRow.Item("CDAREA")) Then
                        ObjSolicitacaoVO.Area = DtRow.Item("CDAREA")
                    Else
                        ObjSolicitacaoVO.Area = ""
                    End If
                End If

                If Dt.Columns.Contains("NMCCUSTO") Then
                    If Not Convert.IsDBNull(DtRow.Item("NMCCUSTO")) Then
                        ObjSolicitacaoVO.LocDescricao = DtRow.Item("NMCCUSTO")
                    Else
                        ObjSolicitacaoVO.LocDescricao = ""
                    End If
                End If

                If Dt.Columns.Contains("NMAREA") Then
                    If Not Convert.IsDBNull(DtRow.Item("NMAREA")) Then
                        ObjSolicitacaoVO.DescSubLoc = DtRow.Item("NMAREA")
                    Else
                        ObjSolicitacaoVO.DescSubLoc = ""
                    End If
                End If
            Next
        End If
        Return ObjSolicitacaoVO
    End Function
    Public Shared Sub GravaLog(ByVal msg As String)
        Dim dt As DateTime = Now
        Dim arquivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\Log\" & "LOG" + Format("{0:yyyyMMdd}", dt) + ".TXT"
        Dim objStream As New FileStream(arquivo, FileMode.Append)
        Dim arq As New StreamWriter(objStream)
        arq.Write(Format("{0:HH:mm:ss}", dt) + " " + msg + vbCrLf)
        arq.Close()
    End Sub
End Class
