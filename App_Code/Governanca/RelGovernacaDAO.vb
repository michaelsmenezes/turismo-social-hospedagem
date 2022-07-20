Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class RelGovernacaDAO
    Dim ObjRelGovernancaVO As RelGovernacaVO
    Dim ObjRelGovernancaDAO As RelGovernacaDAO
    Public Function ConsultaArrumacao(ByVal ObjRelGovernancaVO As RelGovernacaVO, ByVal Lista As String, ByVal Dia As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                .Append("declare ")
                .Append("@ListaCaract varchar(1000), @Dia char(10) ")
                .Append("set @ListaCaract = '" & Lista & "' ")
                .Append("set @Dia = '" & Dia & "'")
                .Append("set nocount on ")
                .Append("declare @Qtde smallint, @Cont integer, @Aux integer, @AcmBicama char(1), @AcmSofacama char(1) ")
                .Append("select @AcmBicama = ArrumaAcmBicama, @AcmSofacama = ArrumaAcmSofacama ")
                .Append("from TbParametro with (nolock) ")
                .Append("create table #Caracteristica( ")
                .Append("CId numeric primary Key) ")
                .Append("set @Cont = 1 ")
                .Append("while Len(@ListaCaract) > @Cont begin ")
                .Append("set @Aux = (select charindex('.', @ListaCaract, @Cont)) ")
                .Append("insert #Caracteristica(CId) ")
                .Append("values(substring(@ListaCaract, @Cont, @Aux - @Cont)) ")
                .Append("set @Cont = @Aux + 1 ")
                .Append("end ")
                .Append("select @Qtde = count(distinct CarGrupo) from ")
                .Append("#Caracteristica cc inner join TbCaracteristica c with (nolock) ")
                .Append("on cc.CId = c.CarId ")
                .Append("create table #AptoPrioridade( ")
                .Append("ApaId smallint, ")
                .Append("ApaDesc varchar(10), ")
                .Append("AptoStatus char(1), ")
                .Append("AptoCC smallint, ")
                .Append("AptoCS smallint, ")
                .Append("AptoCE smallint, ")
                .Append("AptoCEAtual smallint, ")
                .Append("AptoBE smallint, ")
                .Append("AptoBEAtual smallint, ")
                .Append("AptoFR smallint, ")
                .Append("AptoJT smallint, ")
                .Append("Hora smallint) ")
                .Append("create index ISApaId on #AptoPrioridade ( ApaId ASC ) ")
                .Append("create table #Apto( ")
                .Append("ApaId smallint, ")
                .Append("ApaDesc varchar(10), ")
                .Append("AptoCC smallint, ")
                .Append("AptoCS smallint, ")
                .Append("AptoCE smallint, ")
                .Append("AptoBE smallint, ")
                .Append("AptoFR smallint, ")
                .Append("AptoJT smallint) ")
                .Append("create index ISApaId1 on #Apto ( ApaId ASC ) ")
                .Append("insert #Apto (ApaId, ApaDesc, AptoCC, AptoCS, AptoCE, AptoBE, AptoFR, AptoJT) ")
                .Append("select a.ApaId, a.ApaDesc, c.AcmCC as AptoCC, ")
                .Append("c.AcmCS - ")
                .Append("case ")
                .Append("when @AcmBicama = 'N' then AcmBicama ")
                .Append("else 0 ")
                .Append("end - ")
                .Append("case ")
                .Append("when @AcmSofacama = 'N' then AcmSofacama ")
                .Append("else 0 ")
                .Append("end as AptoCS, ")
                .Append("a.ApaCamaExtra as AptoCE, a.ApaBerco as AptoBE, ")
                .Append("c.AcmCC * 2 + c.AcmCS - ")
                .Append("case ")
                .Append("when @AcmBicama = 'N' then AcmBicama ")
                .Append("else 0 ")
                .Append("end - ")
                .Append("case ")
                .Append("when @AcmSofacama = 'N' then AcmSofacama ")
                .Append("else 0 ")
                .Append("end as AptoFR, ")
                .Append("c.AcmCC * 2 + c.AcmCS - ")
                .Append("case ")
                .Append("when @AcmBicama = 'N' then AcmBicama ")
                .Append("else 0 ")
                .Append("end - ")
                .Append("case ")
                .Append("when @AcmSofacama = 'N' then AcmSofacama ")
                .Append("else 0 ")
                .Append("end as AptoJT ")
                .Append("from TbApartamento a with (nolock) inner join TbAcomodacao c with (nolock) ")
                .Append("on a.AcmId = c.AcmId and ApaStatus = 'A' ")
                .Append("where not exists (select 1 from #AptoPrioridade p where p.ApaId = a.ApaId) ")
                .Append("and (case ")
                .Append("when @Qtde > 0 then ")
                .Append("(select count(1) from VwCaracteristicaAcmApa c with (nolock) where c.CarId in ")
                .Append("(select CId from #Caracteristica) ")
                .Append("and a.ApaId = c.ApaId) ")
                .Append("else 0 end) = @Qtde ")
                .Append("select * from #Apto order by ApaId ")
                .Append("drop table #Apto ")
                .Append("drop table #AptoPrioridade ")
                .Append("drop table #Caracteristica ")
                Return PreencheListaArrumacao(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaArrumacao(ByVal ResultadoArrumacao As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoArrumacao.HasRows Then
            While ResultadoArrumacao.Read
                ObjRelGovernancaVO = New RelGovernacaVO
                With ObjRelGovernancaVO
                    .ApaDesc = ResultadoArrumacao.Item("ApaDesc")
                    .ApaId = ResultadoArrumacao.Item("ApaId")
                    .AptoBE = ResultadoArrumacao.Item("AptoBE")
                    .AptoCC = ResultadoArrumacao.Item("AptoCC")
                    .AptoCE = ResultadoArrumacao.Item("AptoCE")
                    .AptoCS = ResultadoArrumacao.Item("AptoCS")
                    .AptoFR = ResultadoArrumacao.Item("AptoFR")
                    .AptoJT = ResultadoArrumacao.Item("AptoJT")
                End With
                Lista.Add(ObjRelGovernancaVO)
            End While
        End If
        ResultadoArrumacao.Close()
        Return Lista
    End Function
End Class
