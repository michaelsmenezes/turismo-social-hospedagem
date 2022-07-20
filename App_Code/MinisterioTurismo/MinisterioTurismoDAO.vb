Imports System
Imports System.Collections
Imports Microsoft.VisualBasic

Public Class MinisterioTurismoDAO
    Dim ObjMinisterioTurismoVO As MinisterioTurismoVO
    Dim ObjMinisterioTurismoDAO As MinisterioTurismoDAO
    Dim DB As String
    Dim Conn As Banco.Conexao
    Dim VarSql As Text.StringBuilder

    Public Sub New(ByVal Banco As String)
        DB = Banco
    End Sub

    Public Function Consultar(ByVal DataIni As String, ByVal DataFim As String) As IList
        Try
            Conn = New Banco.Conexao(DB)
            VarSql = New Text.StringBuilder("Set Nocount on ")
            With VarSql
                .Append("Declare @DataIni DateTime ")
                .Append("Declare @DataFim DateTime ")

                .Append("Set @DataIni = '" & DataIni & "' ")
                .Append("Set @DataFim = '" & DataFim & "' ")

                'Tabela Auxiliar que irá receber os valores sem agrupamento para depois serem agrupados 
                .AppendLine("                Create Table #Auxiliar( ")
                .AppendLine("                    Data DateTime, ")
                .AppendLine("                    DataFinal DateTime, ")
                .AppendLine("                    Entradas integer, ")
                .AppendLine("                    Saidas integer, ")
                .AppendLine("                    QtdePessoasDiaMesAnterior integer, ")
                .AppendLine("                    Apartamentos integer, ")
                .AppendLine("                    Leitos integer, ")
                .AppendLine("                    TotLeitos integer, ")
                .AppendLine("                    Hospedados integer, ")
                .AppendLine("                    EntradasUH integer, ")
                .AppendLine("                    SaidasUH integer) ")

                'Tabela auxiliar para agrupamento e consulta final par ao relatório 
                .AppendLine("                Create Table #Agrupada( ")
                .AppendLine("                    Data DateTime, ")
                .AppendLine("                    Entradas integer, ")
                .AppendLine("                    Saidas integer, ")
                .AppendLine("                    QtdePessoasDiaMesAnterior integer, ")
                .AppendLine("                    Apartamentos integer, ")
                .AppendLine("                    Leitos integer, ")
                .AppendLine("                    TotLeitos integer, ")
                .AppendLine("                    Hospedados integer, ")
                .AppendLine("                    EntradasUH integer, ")
                .AppendLine("                    SaidasUH integer) ")

                'Gerando as Entradas com base no TbMapaHospedagem ainda desagrupadas 
                .AppendLine("                Insert #Auxiliar(Data,Entradas) ")
                .AppendLine("                select CONVERT(varchar(10), h.data ,120), 1 as Entradas ")
                .AppendLine("                from TbMapaHospedagem h ")
                .AppendLine("                where h.data between @DataIni and @DataFim ")
                .AppendLine("                and h.TIPODIARIA = 'I' ")
                .AppendLine("                and exists(select top 1 1 from TbMapaHospedagem hh where hh.INTID = h.INTID and hh.TIPODIARIA = 'A') ")

                'Agrupando a entradas e finalizando as entradas 
                .AppendLine(" Insert #Agrupada(Data,Entradas,EntradasUH )  ")
                .AppendLine("select MAX(a.Data) as Data,COUNT(a.Entradas) as Entradas, ")
                .AppendLine("(select  count (distinct CAST(mm.RESID as varchar(20)) + 'a' + cast(mm.APAID as varchar(20))) from TbMapaHospedagem mm where mm.DATA = a.Data and mm.TIPODIARIA = 'I') ")
                .AppendLine("from #Auxiliar a  ")
                .AppendLine("group by a.Data  ")
                .AppendLine("order by a.Data  ")

                '.AppendLine("                Insert #Agrupada(Data,Entradas) ")
                '.AppendLine("                select MAX(Data) as Data,COUNT(Entradas) as Entradas from #Auxiliar ")
                '.AppendLine("                group by Data ")
                '.AppendLine("                order by Data ")

                'Inicio do processo de geração das Saídaas  
                .AppendLine("                Declare @DataAuxiliar as DateTime ")
                .AppendLine("                Declare @Maximo as integer ")
                .AppendLine("                Declare @Contador as integer ")
                .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
                .AppendLine("                Set @Contador = 0 ")
                .AppendLine("                Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")
                'Inserindo as Saidas de forma agrupada e prontas para o relatório 
                .AppendLine("                while @Contador <= @Maximo  ")
                .AppendLine("                   BEGIN ")
                .AppendLine("                      update #Agrupada set Saidas =  ")
                .AppendLine("                      (select COUNT(1) ")
                .AppendLine("                      from TbMapaHospedagem m ")
                .AppendLine("                      where m.DATA = @DataAuxiliar ")
                .AppendLine("                      and m.TIPODIARIA = 'A' ")
                .AppendLine("                      and not exists (select 1 from TbMapaHospedagem hh where hh.INTID = m.INTID and hh.DATA > m.DATA) ")
                .AppendLine("                      group by m.data), ")

                .AppendLine("                      SaidasUH = ")
                .AppendLine("                      (select  count (distinct CAST(mm.RESID as varchar(20)) + 'a' + cast(mm.APAID as varchar(20))) ")
                .AppendLine("                      from TbMapaHospedagem mm where mm.DATA = @DataAuxiliar ")
                .AppendLine("                      and mm.TIPODIARIA = 'A' ")
                .AppendLine("                      and not exists (select 1 from TbMapaHospedagem hh where hh.INTID = mm.INTID and hh.DATA > mm.DATA) ")
                .AppendLine("                      group by mm.DATA) ")

                .AppendLine("                      where Data = @DataAuxiliar  ")
                .AppendLine("                      Set @Contador = @Contador + 1  ")
                .AppendLine("                      Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
                .AppendLine("                  END   ")

                '.AppendLine("                while @Contador <= @Maximo ")
                '.AppendLine("                  BEGIN ")
                '.AppendLine("                    update #Agrupada set Saidas = ")
                '.AppendLine("                    (select COUNT(1) ")
                '.AppendLine("                    from TbMapaHospedagem m ")
                '.AppendLine("                    where m.DATA = @DataAuxiliar ")
                '.AppendLine("                    and m.TIPODIARIA = 'A' ")
                '.AppendLine("                    and not exists (select 1 from TbMapaHospedagem hh where hh.INTID = m.INTID and hh.DATA > m.DATA) ")
                '.AppendLine("                    group by m.data) ")
                '.AppendLine("                    where Data = @DataAuxiliar ")

                '.AppendLine("                    Set @Contador = @Contador + 1 ")
                '.AppendLine("                    Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
                '.AppendLine("                END  ")

                'Inserindo o total de pessoas Hospedadas com base na tabela TbMapaHospedagem 
                .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
                .AppendLine("                Set @Contador = 0 ")
                .AppendLine("                Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")
                .AppendLine("                while @Contador <= @Maximo ")
                .AppendLine("                     BEGIN ")
                .AppendLine("                        update #Agrupada set Hospedados = ")
                .AppendLine("                       (select COUNT(1) ")
                .AppendLine("                        from TbMapaHospedagem m ")
                .AppendLine("                        where m.DATA =  @DataAuxiliar + 1 ")
                .AppendLine("                        and m.TIPODIARIA = 'A' ")
                .AppendLine("                        group by m.data) ")
                .AppendLine("                        where data = @DataAuxiliar ")
                .AppendLine("                        Set @Contador = @Contador + 1 ")
                .AppendLine("                        Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
                .AppendLine("                     End ")

                'Inserindo a Quantidade o último dia do Mês Anterior
                .AppendLine("                Set @DataAuxiliar = (select min(DATA) from #Agrupada) ")
                .AppendLine("                update #Agrupada set QtdePessoasDiaMesAnterior =  ")
                .AppendLine("                (select COUNT(1) ")
                .AppendLine("                from TbMapaHospedagem h ")
                .AppendLine("                where h.DATA = @DataAuxiliar  ")
                .AppendLine("                and h.TIPODIARIA = 'A' ")
                .AppendLine("                group by h.data) ")

                'Inserindo Apartamentos e Leitos Ocupados em um determinado dia 
                .AppendLine("                Set @Maximo = (select MAX(day(Data)) from #Agrupada) ")
                .AppendLine("                 Set @Contador = 0 ")
                .AppendLine("                 Set @DataAuxiliar = (select MIN(data) from #Agrupada) ")

                .AppendLine("                 while @Contador <= @Maximo ")
                .AppendLine("                     BEGIN ")
                .AppendLine("                        Update #Agrupada Set Apartamentos =  ")
                .AppendLine("                        (select COUNT(ApaId)as Apartamentos from TbLeitosOcupados ")
                .AppendLine("                        where LoDATA = @DataAuxiliar ")
                .AppendLine("                        group by LoData) ")
                .AppendLine("                        where data = @DataAuxiliar ")

                .AppendLine("                        Update #Agrupada Set Leitos = ")
                .AppendLine("                        (select SUM(LoQtde) as Leitos from TbLeitosOcupados ")
                .AppendLine("                        where LoDATA = @DataAuxiliar ")
                .AppendLine("                        group by LoData) ")
                .AppendLine("                        where data = @DataAuxiliar ")

                .AppendLine("                        Set @Contador = @Contador + 1 ")
                .AppendLine("                        Set @DataAuxiliar = DATEADD(day,1,@DataAuxiliar) ")
                .AppendLine("                 END ")

                'Inserindo o Total Geral de Leitos Ocupados  
                .AppendLine("                 Update #Agrupada set TotLeitos =  ")
                .AppendLine("                    (select sum(((AcmCC * 2) + AcmCS) * AcmQtdeApto)  as TotLeitos  from TbAcomodacao ) ")

                .AppendLine("                 select Data,IsNull((Entradas),0) as Entradas,isNull((Saidas),0) as Saidas,ISNULL((EntradasUH),0) as EntradasUH,ISNULL((SaidasUH),0) as SaidasUH,isNull((QtdePessoasDiaMesAnterior),0) as QtdePessoasDiaMesAnterior,isNull((Apartamentos),0) as Apartamentos,isNull((Leitos),0) as Leitos,isNull((TotLeitos),0) as TotLeitos,isNull((Hospedados),0) as Hospedados from  #Agrupada  ")
                .AppendLine("                 Order by Data ")

                .AppendLine("                 Drop Table #Auxiliar ")
                .AppendLine("                 Drop Table #Agrupada ")

                Return PreencheLista(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function

    Private Function PreencheLista(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjMinisterioTurismoVO = New MinisterioTurismoVO
                With ObjMinisterioTurismoVO
                    .Data = ResultadoConsulta.Item("Data")
                    .Entradas = ResultadoConsulta.Item("Entradas")
                    .Saidas = ResultadoConsulta.Item("Saidas")
                    .EntradasUH = ResultadoConsulta.Item("EntradasUH")
                    .SaidasUH = ResultadoConsulta.Item("SaidasUH")
                    .QtdePessoasDiaMesAnterior = ResultadoConsulta.Item("QtdePessoasDiaMesAnterior")
                    .Apartamentos = ResultadoConsulta.Item("Apartamentos")
                    .Leitos = ResultadoConsulta.Item("Leitos")
                    .TotLeitos = ResultadoConsulta.Item("TotLeitos")
                    .Hospedados = ResultadoConsulta.Item("Hospedados")
                End With
                Lista.Add(ObjMinisterioTurismoVO)
            End While
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
