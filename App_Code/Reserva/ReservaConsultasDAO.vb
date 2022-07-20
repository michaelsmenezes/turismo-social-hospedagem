Imports Microsoft.VisualBasic
Imports Banco, System.Data.SqlClient
Imports System
Imports System.Collections
Imports System.Text

Public Class ReservaConsultasDAO
    Dim Conn As Conexao
    Dim ObjReservaConsultasVO As ReservaConsultasVO
    Dim ObjReservaConsultasDAO As ReservaConsultasDAO
    Public Sub New(Con As String)
        Conn = New Conexao(Con)
    End Sub

    Public Function VerificaAptoOcupado(ResId As String, SolId As String) As Long
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .Append("if exists (select top 1 1 from TbIntegrante i ")
                .Append("inner join TbHospedagem h on h.IntId  = i.intid ")
                .Append("where i.ResId = '" & ResId & "' ")
                .Append("and h.SolId = '" & SolId & "' ")
                .Append("and i.IntDataIniReal is not null) ")
                .Append("     Begin ")
                .Append("       select 1 as Estada goto saida ")
                .Append("     end ")
                .Append("   else ")
                .Append("     Begin ")
                .Append("       select 0 as CheckIn goto saida ")
                .Append("     end ")
                .Append("saida: ")
                Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    Public Function ConsultaDadosComprovanteReserva(ObjReservaConsultaVO As ReservaConsultasVO) As ReservaConsultasVO
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .AppendLine("select r.ResId,r.ResNome,ISNULL(r.ResEmail,'')ResEmail, ")
                .AppendLine("CONVERT(char(10),r.ResDataIni,103) + ' até ' + Convert(char(10),r.ResDataFim,103) as Periodo, ")
                .AppendLine("r.ResStatus, ")
                .AppendLine("Case When a.BloId = 1 then 'Rio Tocantins' ")
                .AppendLine("     When a.BloId = 2 then 'Rio Araguaia' ")
                .AppendLine("	 When a.BloId = 3 then 'Rio Paranaíba' ")
                .AppendLine("	 When a.BloId = 4 then 'Rio Vermelho' end as Bloco, ")
                .AppendLine("    (select COUNT(*)from TbIntegrante where resid = r.ResId) as Pessoas ")
                .AppendLine(", '' Integrantes ")
                .AppendLine("from TbReserva r ")
                .AppendLine("inner join TbSolicitacao s on s.ResId = r.ResId ")
                .AppendLine("inner join TbAcomodacao a on a.AcmId = s.AcmId ")
                .AppendLine("and r.resid = '" & ObjReservaConsultaVO.ResId & "' ")
            End With
            Return PreencheObjetoComprovanteReserva(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function
    Public Function ConsultaDadosVoucherReserva(hosId As String, SolId As String) As ReservaConsultasVO
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .AppendLine("Declare @ResId integer,@solId integer, @hosId integer ")

                .AppendLine("set @solId = " & SolId & " ")
                .AppendLine("set @hosId = " & hosId & " ")

                .AppendLine("Declare @Final Table( ")
                .AppendLine("ResId integer, ")
                .AppendLine("ResNome varchar(50), ")
                .AppendLine("ResEmail varchar(80), ")
                .AppendLine("Periodo varchar(50), ")
                .AppendLine("ResStatus char(1), ")
                .AppendLine("Bloco varchar(50), ")
                .AppendLine("Pessoas varchar(3), ")
                .AppendLine("Integrantes varchar(1000) ")
                .AppendLine(") ")

                .AppendLine("Declare @TodosIntegrante Table( ")
                .AppendLine("    IntId integer, ")
                .AppendLine("    IntNome varchar(100) ")
                .AppendLine(") ")

                .AppendLine("insert @TodosIntegrante(IntId,IntNome) ")
                .AppendLine("select ii.IntId,ii.intNome from TbIntegrante ii inner join TbHospedagem hh on hh.IntId = ii.IntId where hh.SolId = @solId ")

                .AppendLine("Declare @Integrantes varchar(1000)= '', @IntegranteId integer = 0 ")

                .AppendLine("While (select sum(1) from @TodosIntegrante) >= 0 ")
                .AppendLine("   Begin ")
                .AppendLine("      Set @IntegranteId = (select max(intid) from @TodosIntegrante) ")
                .AppendLine("      set @Integrantes = case when LEN(@Integrantes) > 0 then @Integrantes + '<br>' + LTRIM(RTRIM((select intNome from @TodosIntegrante where IntId = @IntegranteId))) else LTRIM(RTRIM((select intNome from @TodosIntegrante where IntId = @IntegranteId)))  end ")
                .AppendLine("	  delete from @TodosIntegrante where IntId = @IntegranteId ")
                .AppendLine("   end ")

                .AppendLine("Set Nocount On ")

                .AppendLine("insert @Final ")
                .AppendLine("select r.ResId,r.ResNome,ISNULL(r.ResEmail,'')ResEmail, ")
                .AppendLine("CONVERT(char(10),r.ResDataIni,103) + ' até ' + Convert(char(10),r.ResDataFim,103) as Periodo, ")
                .AppendLine("r.ResStatus, ")
                .AppendLine("Case When a.BloId = 1 then 'Rio Tocantins' ")
                .AppendLine("     When a.BloId = 2 then 'Rio Araguaia' ")
                .AppendLine("	 When a.BloId = 3 then 'Rio Paranaíba' ")
                .AppendLine("	 When a.BloId = 4 then 'Rio Vermelho' end as Bloco, ")
                .AppendLine("    (select COUNT(*)from TbIntegrante ie inner join TbHospedagem hp on ie.IntId = hp.IntId where hp.SolId = @solId ) as Pessoas,ISNULL(@Integrantes,'') Integrantes ")
                .AppendLine("from TbReserva r ")
                .AppendLine("inner join TbSolicitacao s on s.ResId = r.ResId ")
                .AppendLine("inner join TbAcomodacao a on a.AcmId = s.AcmId  ")
                .AppendLine("inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("inner join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("and s.solid = @solId ")
                .AppendLine("and h.HosId = @hosId ")
                'Todos os integrantes deverão estar com o valor pago para poder imprimir o voucher
                .AppendLine("and exists(select 1 from TbVencimento v ")
                .AppendLine("           inner join TbBoletosImp b on v.VenNossoNumero = b.BolImpNossoNumero ")
                .AppendLine("		   inner join TbIntegranteBoleto ib on ib.BolImpId = b.BolImpId ")
                .AppendLine("		   inner join TbHospedagem hm on hm.IntId = i.IntId ")
                .AppendLine("		   where ib.IntId = i.IntId and hm.SolId = @solId) ")
                'Quando for cortesia ou Free, irei permitir gerar o voucher
                .AppendLine("Union ")
                .AppendLine("select r.ResId,r.ResNome,ISNULL(r.ResEmail,'')ResEmail, ")
                .AppendLine("CONVERT(char(10),r.ResDataIni,103) + ' até ' + Convert(char(10),r.ResDataFim,103) as Periodo, ")
                .AppendLine("r.ResStatus, ")
                .AppendLine("Case When a.BloId = 1 then 'Rio Tocantins' ")
                .AppendLine("     When a.BloId = 2 then 'Rio Araguaia' ")
                .AppendLine("	 When a.BloId = 3 then 'Rio Paranaíba' ")
                .AppendLine("	 When a.BloId = 4 then 'Rio Vermelho' end as Bloco, ")
                .AppendLine("    (select COUNT(*)from TbIntegrante ie inner join TbHospedagem hp on ie.IntId = hp.IntId where hp.SolId = @solId ) as Pessoas,ISNULL(@Integrantes,'') Integrantes ")
                .AppendLine("from TbReserva r ")
                .AppendLine("inner join TbSolicitacao s on s.ResId = r.ResId ")
                .AppendLine("inner join TbAcomodacao a on a.AcmId = s.AcmId  ")
                .AppendLine("inner join TbIntegrante i on i.ResId = r.ResId ")
                .AppendLine("inner join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("and s.solid = @solId ")
                .AppendLine("and h.HosId = @hosId ")
                .AppendLine("where i.IntFormaPagamento in ('F','C') ")
                .AppendLine("select * from @Final ")
            End With
            Return PreencheObjetoComprovanteReserva(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function
    Private Function PreencheObjetoComprovanteReserva(ResultadoConsulta As SqlDataReader) As ReservaConsultasVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjReservaConsultasVO = New ReservaConsultasVO
            With ObjReservaConsultasVO
                .ResId = ResultadoConsulta.Item("ResId")
                .BolImpSacado = ResultadoConsulta.Item("ResNome")
                .BolImpDtPagamento = ResultadoConsulta.Item("Periodo")
                .resEmail = ResultadoConsulta.Item("ResEmail")
                .ResStatus = ResultadoConsulta.Item("ResStatus")
                .BloId = ResultadoConsulta.Item("Bloco")
                .QtdPessoas = ResultadoConsulta.Item("Pessoas")
                .Integrantes = ResultadoConsulta.Item("Integrantes").ToString.ToUpper
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjReservaConsultasVO
    End Function
    Public Function ConsultaObservacoes(CPF As String, Matricula As String) As IList
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .AppendLine("SELECT ResCPF_CGC,ResMatricula,ResObs,convert(char(10),ResDataIni,103) + ' Até ' + convert(char(10),ResDataFim,103) AS Periodo FROM TbReserva r  ")
                .AppendLine("WHERE resid > 0 ")
                .AppendLine("AND ResObs > '' ")
                .AppendLine("AND LEN(REPLACE(REPLACE(RTRIM(LTRIM(ResCPF_CGC)),' ',''),'-','')) > 10  ")
                .AppendLine("AND (ResCPF_CGC = '" & CPF & "' OR ResMatricula = '" & Matricula & "') ")
                .AppendLine("ORDER BY r.ResCPF_CGC,RESID DESC ")
            End With
            Return PreencheObjetolistaObservacao(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    Private Function PreencheObjetolistaObservacao(ResultadoConsulta As SqlDataReader) As IList
        Dim Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read()
                ObjReservaConsultasVO = New ReservaConsultasVO
                With ObjReservaConsultasVO
                    .ResCPF_CGC = ResultadoConsulta.Item("ResCPF_CGC")
                    .ResMatricula = ResultadoConsulta.Item("ResMatricula")
                    .ResObs = ResultadoConsulta.Item("ResObs")
                    .Periodo = ResultadoConsulta.Item("Periodo")
                End With
                Lista.Add(ObjReservaConsultasVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function

    Public Function MudaDataInsercao(ResId As String, Usuario As String) As Long
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .AppendLine("if exists (select 1 from TbReserva where resid = " & ResId & ") ")
                .AppendLine("     Begin ")
                .AppendLine("       Insert TbReservaLog(ResId,ResUsuario,ResUsuarioData,ResUsuarioLog,ResUsuarioDataLog,ResNome) (select ResId,ResUsuario,ResUsuarioData,'NovoUsuario',Getdate(),SUBSTRING(ResObs + ' ' + 'Antiga Inserção ' + convert(char(10), ResDtInsercao,103)  + ' " & Usuario & " ' + convert(varchar(20), GetDate(),126) ,0,200) from TbReserva where resid = " & ResId & ") ")
                .AppendLine("       Update TbReserva set ResDtInsercao = '2018-09-01 12:00:00', ResObs = SUBSTRING(ResObs + ' ' + 'Antiga Inserção ' + convert(char(10), ResDtInsercao,103)  + ' " & Usuario & " ' + convert(varchar(20), GetDate(),126) ,0,200) where resid = " & ResId & " ")
                .AppendLine("       Select 1 goto saida ")
                .AppendLine("     end ")
                .AppendLine("   else ")
                .AppendLine("     Begin ")
                .AppendLine("       select 0 goto saida ")
                .AppendLine("     end ")
                .AppendLine("saida: ")
                Dim Resultado = CLng(Conn.executaTransacionalTestaRetorno(VarSql.ToString))
                Return Resultado
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function

    Public Function ConsultaDataNascimento(CPF As String) As ReservaConsultasVO
        Try
            Dim VarSql = New Text.StringBuilder("Set Nocount On ")
            With VarSql
                .AppendLine("select top 1 isNull(ResDtNascimento,'')ResDtNascimento, IsNull(ResNome,'')ResNome from TbLEReserva where REPLACE(REPLACE(REPLACE(REPLACE(ResCPFCGC,' ',''),'/',''),'-',''),'.','') = '" & CPF & "' order by ResDtInsercao  ")
            End With
            Return PreencheNascimento(Conn.consulta(VarSql.ToString))
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Function
    Private Function PreencheNascimento(Resultado As SqlDataReader) As ReservaConsultasVO
        If Resultado.HasRows Then
            Resultado.Read()
            ObjReservaConsultasVO = New ReservaConsultasVO
            ObjReservaConsultasVO.BolImpDtDocumento = Resultado.Item("ResDtNascimento")
            ObjReservaConsultasVO.BolImpSacado = Resultado.Item("ResNome")
        End If
        Resultado.Close()
        Return ObjReservaConsultasVO
    End Function
End Class
