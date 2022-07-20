Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Imports Banco
Imports System.Net.Mail

Public Class ReservaEmissivoDAO
    Dim Conn As Conexao
    Dim VarSql As Text.StringBuilder
    Dim objReservaEmissivaVO As ReservaEmissivoVO
    Dim objReservaEmissivaDAO As ReservaEmissivoDAO
    Public Sub New(Banco As String)
        Conn = New Conexao(Banco)
    End Sub

    Public Function InsereEmissivo(objReservasEmissivoVO As ReservaEmissivoVO, resTipo As String) As Long
        Try
            VarSql = New Text.StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("if not exists(Select 1 from TbReserva where ResId = " & objReservasEmissivoVO.resId & ") ")
                .AppendLine("   Begin ")
                .AppendLine("           insert TbReserva(ResCaracteristica,ResNome,ResDataIni,ResDataFim,ResPasseioPromovidoCEREC,ResContato,ResFoneComercial,ResFoneResidencial, ")
                .AppendLine("          ResCelular,CatId,ResCatCobranca,ResMemorando,ResEmissor,ResObs,ResAlmoco,ResAlmocoRestaurante,EstIdDes,ResCidadeDes,ResHotelExcursao, ")
                .AppendLine("          ResLocalSaida,ResHoraSaida,ResTipo, ")
                .AppendLine("          resStatus,catRefeicaoId,refPratoCod,resCafe,resJantar,resFormaPagamento,resFormaPagtoCafe,resFormaPagtoAlmoco,resFormaPagtoJantar,resCapitalGoias,resUsuario,resUsuarioData ")
                'Grupo da Luzia
                If resTipo = "E" Then
                    .AppendLine(",resIdWeb,resDtGrupoConfirmacao,resDtGrupoListagem,resDtGrupoPgtoSinal,resDtGrupoPgtoTotal)")
                Else
                    .AppendLine(") ")
                End If

                .AppendLine("     values ")
                .AppendLine("('" & objReservasEmissivoVO.resCaracteristica & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resNome & "'")
                .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDataIni), "yyyy-MM-dd") & "'")
                .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDataFim), "yyyy-MM-dd") & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resPasseioPromovidoCEREC & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resContato & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFoneComercial & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFoneResidencial & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFoneResidencial & "'")
                .AppendLine(",'" & objReservasEmissivoVO.catId & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resCatCobranca & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resMemorando & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resEmissor & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resObs & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resAlmoco & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resAlmocoRestaurante & "'")
                .AppendLine(",'" & objReservasEmissivoVO.estIdDes & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resCidadeDes & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resHotelExcursao & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resLocalSaida & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resHoraSaida & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resTipo & "'")
                'Daqui para baixo havia faltado
                .AppendLine(",'" & objReservasEmissivoVO.resStatus & "'")
                .AppendLine(",'" & objReservasEmissivoVO.catRefeicaoId & "'")
                .AppendLine(",'" & objReservasEmissivoVO.refPratoCod & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resCafe & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resJantar & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFormaPagamento & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFormaPagtoCafe & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFormaPagtoAlmoco & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resFormaPagtoJantar & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resCapitalGoias & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resUsuario & "'")
                .AppendLine(",'" & objReservasEmissivoVO.resUsuarioData & "' ")
                'Grupos da Luzia
                If resTipo = "E" Then
                    .AppendLine(",'" & objReservasEmissivoVO.resIdWeb & "' ")
                    .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDtGrupoConfirmacao), "yyyy-MM-dd") & "' ")
                    .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDtGrupoPgtoSinal), "yyyy-MM-dd") & "' ")
                    .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDtGrupoPgtoSinal), "yyyy-MM-dd") & "' ")
                    .AppendLine(",'" & Format(CDate(objReservasEmissivoVO.resDtGrupoPgtoTotal), "yyyy-MM-dd") & "') ")
                Else
                    .AppendLine(") ")
                End If
                .AppendLine("        select 1 goto saida ")
                .AppendLine("    end ")
                .AppendLine("  else ")
                .AppendLine("    Begin ")
                .AppendLine("Update TbReserva set ")
                .AppendLine("ResCaracteristica='" & objReservasEmissivoVO.resCaracteristica & "'")
                .AppendLine(",ResNome='" & objReservasEmissivoVO.resNome & "' ")
                .AppendLine(",ResDataIni='" & Format(CDate(objReservasEmissivoVO.resDataIni), "yyyy-MM-dd") & "' ")
                .AppendLine(",ResDataFim='" & Format(CDate(objReservasEmissivoVO.resDataFim), "yyyy-MM-dd") & "' ")
                .AppendLine(",ResPasseioPromovidoCEREC='" & objReservasEmissivoVO.resPasseioPromovidoCEREC & "' ")
                .AppendLine(",ResContato='" & objReservasEmissivoVO.resContato & "' ")
                .AppendLine(",ResFoneComercial='" & objReservasEmissivoVO.resFoneComercial & "' ")
                .AppendLine(",ResFoneResidencial='" & objReservasEmissivoVO.resFoneResidencial & "' ")
                .AppendLine(",ResCelular='" & objReservasEmissivoVO.resCelular & "' ")
                .AppendLine(",CatId='" & objReservasEmissivoVO.catId & "' ")
                .AppendLine(",ResCatCobranca='" & objReservasEmissivoVO.resCatCobranca & "' ")
                .AppendLine(",ResMemorando='" & objReservasEmissivoVO.resMemorando & "' ")
                .AppendLine(",ResEmissor='" & objReservasEmissivoVO.resEmissor & "' ")
                .AppendLine(",ResObs='" & objReservasEmissivoVO.resObs & "' ")
                .AppendLine(",ResAlmoco='" & objReservasEmissivoVO.resAlmoco & "' ")
                .AppendLine(",ResAlmocoRestaurante='" & objReservasEmissivoVO.resAlmocoRestaurante & "' ")
                .AppendLine(",EstIdDes='" & objReservasEmissivoVO.estIdDes & "' ")
                .AppendLine(",ResCidadeDes='" & objReservasEmissivoVO.resCidadeDes & "' ")
                .AppendLine(",ResHotelExcursao='" & objReservasEmissivoVO.resHotelExcursao & "' ")
                .AppendLine(",ResLocalSaida='" & objReservasEmissivoVO.resLocalSaida & "' ")
                .AppendLine(",ResHoraSaida='" & objReservasEmissivoVO.resHoraSaida & "' ")
                .AppendLine(",ResTipo='" & objReservasEmissivoVO.resTipo & "' ")
                'Havia faltado daqui para baixo
                .AppendLine(",resStatus='" & objReservasEmissivoVO.resStatus & "' ")
                .AppendLine(",catRefeicaoId='" & objReservasEmissivoVO.catRefeicaoId & "' ")
                .AppendLine(",refPratoCod='" & objReservasEmissivoVO.refPratoCod & "' ")
                .AppendLine(",resCafe='" & objReservasEmissivoVO.resCafe & "' ")
                .AppendLine(",resJantar='" & objReservasEmissivoVO.resJantar & "' ")
                .AppendLine(",resFormaPagamento='" & objReservasEmissivoVO.resFormaPagamento & "' ")
                .AppendLine(",resFormaPagtoCafe='" & objReservasEmissivoVO.resFormaPagtoCafe & "' ")
                .AppendLine(",resFormaPagtoAlmoco='" & objReservasEmissivoVO.resFormaPagtoAlmoco & "' ")
                .AppendLine(",resFormaPagtoJantar='" & objReservasEmissivoVO.resFormaPagtoJantar & "' ")
                .AppendLine(",resCapitalGoias='" & objReservasEmissivoVO.resCapitalGoias & "' ")
                .AppendLine(",resUsuario='" & objReservasEmissivoVO.resUsuario & "' ")
                .AppendLine(",resUsuarioData='" & objReservasEmissivoVO.resUsuarioData & "' ")
                'Grupos da Luzia
                If resTipo = "E" Then
                    .AppendLine(",resIdWeb='" & objReservasEmissivoVO.resIdWeb & "' ")
                    .AppendLine(",resDtGrupoConfirmacao='" & Format(CDate(objReservasEmissivoVO.resDtGrupoConfirmacao), "yyyy-MM-dd") & "' ")
                    .AppendLine(",resDtGrupoListagem='" & Format(CDate(objReservasEmissivoVO.resDtGrupoListagem), "yyyy-MM-dd") & "' ")
                    .AppendLine(",resDtGrupoPgtoSinal='" & Format(CDate(objReservasEmissivoVO.resDtGrupoPgtoSinal), "yyyy-MM-dd") & "' ")
                    .AppendLine(",resDtGrupoPgtoTotal='" & Format(CDate(objReservasEmissivoVO.resDtGrupoPgtoTotal), "yyyy-MM-dd") & "' ")
                End If
                .AppendLine("       Where resid = '" & objReservasEmissivoVO.resId & "'")
                .AppendLine("       select 2 goto saida")
                .AppendLine("    end ")
                .AppendLine("IF @@ERROR > 0 ")
                .AppendLine("Select 0 goto saida ")
                .AppendLine("saida: ")
                Dim resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return resultado
            End With
        Catch ex As Exception
            enviarEmail("Erro ao inserir ou fazer o update na função InsereEmissivo." & ex.StackTrace.ToString)
            Throw New System.Exception("Erro ao inserir ou fazer o update na função InsereEmissivo.", ex)
        End Try
    End Function

    Public Function CancelaPasseio(ResId As String) As Long
        Try
            VarSql = New Text.StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("if exists(Select 1 from TbReserva where ResId = " & ResId & ") ")
                .AppendLine("   Begin ")
                .AppendLine("          Update tbreserva set ResStatus = 'C' where resId = " & ResId & " ")
                .AppendLine("          Select 1 goto Saida ")
                .AppendLine("    end ")
                .AppendLine("IF @@ERROR > 0 ")
                .AppendLine("Select 0 goto saida ")
                .AppendLine("saida: ")
                Dim resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return resultado
            End With
        Catch ex As Exception
            enviarEmail("Erro ao Cancelar o passeio cujo resId = " & ResId & vbNewLine & "  Update na função Cancela Passeio." & ex.StackTrace.ToString)
            Throw New System.Exception("Erro ao fazer cancelar um passeio:  função CancelaPasseio.", ex)
        End Try
    End Function
    Public Function ReativaPasseio(ResId As String) As Long
        Try
            VarSql = New Text.StringBuilder("Set nocount on ")
            With VarSql
                .AppendLine("if exists(Select 1 from TbReserva where ResId = " & ResId & ") ")
                .AppendLine(" Declare @Devido as decimal ")
                .AppendLine(" Declare @Pago as Decimal ")
                .AppendLine(" Set @Devido = (select SUM(VenValor) from TbVencimento where venstatus = 'V' and ResId = " & ResId & ") ")
                .AppendLine(" Set @Pago = (select SUM(VenValor) from TbVencimento where venstatus in ('M','C','B','T','A') and ResId = " & ResId & ") ")
                .AppendLine(" if @Pago < @Devido ")
                .AppendLine("    Begin ")
                .AppendLine("       Update tbreserva set ResStatus = 'P' where resId = " & ResId & " and ResStatus = 'C' and CONVERT(char(10), ResDataIni,120) >= CONVERT(char(10),GETDATE(),120) ")
                .AppendLine("       select 1 goto saida ")
                .AppendLine("    end ")
                .AppendLine(" if @Pago >= @Devido  ")
                .AppendLine("    Begin ")
                .AppendLine("       if not exists(select top 1 1 from TbIntegrante where ResId = " & ResId & " )")
                .AppendLine("          Begin ")
                .AppendLine("             Update tbreserva set ResStatus = 'I' where resId = " & ResId & " and ResStatus = 'C' and CONVERT(char(10), ResDataIni,120) >= CONVERT(char(10),GETDATE(),120) ")
                .AppendLine("          end ")
                .AppendLine("       else ")
                .AppendLine("          Begin ")
                .AppendLine("             Update tbreserva set ResStatus = 'R' where resId = " & ResId & " and ResStatus = 'C' and CONVERT(char(10), ResDataIni,120) >= CONVERT(char(10),GETDATE(),120) ")
                .AppendLine("             select 2 goto saida ")
                .AppendLine("          end ")
                .AppendLine("    end ")
                .AppendLine("IF @@ERROR > 0 ")
                .AppendLine("Select 0 goto saida ")
                .AppendLine("    saida: ")
                Dim resultado As String = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return resultado
            End With
        Catch ex As Exception
            enviarEmail("Erro ao reativar o passeio cujo resId = " & ResId & vbNewLine & "  Update na função ReativaPasseio Passeio." & ex.StackTrace.ToString)
            Throw New System.Exception("Erro ao reativar um passeio:  função ReativaPasseio.", ex)
        End Try
    End Function

    Protected Sub enviarEmail(Mensagem As String)
        Try
            Dim objEmail As New System.Net.Mail.MailMessage()
            objEmail.Subject = "SESC Goiás - Turismo Social"
            objEmail.To.Add(New System.Net.Mail.MailAddress("reservas.caldasnovas@sescgo.com.br "))
            'objEmail.To.Add(New MailAddress(objUsuarioRedeVO.mail))
            objEmail.IsBodyHtml = True

            Dim objSmtp As SmtpClient

            '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
            'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
            objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
            objSmtp.EnableSsl = True
            objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
            objSmtp.UseDefaultCredentials = False
            objSmtp.Timeout = 5000

            objEmail.From = New System.Net.Mail.MailAddress("reservas.caldasnovas@sescgo.com.br ")
            'objEmail.Bcc.Add("haas@sescgo.com.br")
            Dim sEmail As New StringBuilder
            sEmail.Append("<p />'" & Mensagem & "'.")
            objEmail.Body = sEmail.ToString
            objSmtp.Send(objEmail)
        Catch ex As Exception
            Throw New System.Exception("Erro ao enviar email: Função EnviarEmail em ReservaEmissivoDAO.VB ", ex)
        End Try
    End Sub
End Class
