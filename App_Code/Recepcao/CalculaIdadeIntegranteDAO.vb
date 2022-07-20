Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Imports Banco
Public Class CalculaIdadeIntegranteDAO
    Dim Conn As Conexao
    Dim VarSql As Text.StringBuilder
    Dim objCalculaIdadeIntegranteVO As CalculaIdadeIntegranteVO
    Public Sub New(banco As String)
        Conn = New Conexao(banco)
    End Sub

    Public Function CalculaIdadeIntegrante(IntId As String) As Long
        VarSql = New Text.StringBuilder("Set nocount on ")
        With VarSql
            'As faixas de idades sofream alteração em 2019, tbFaixaEtaria é o divisor
            .AppendLine("Declare @IntId integer, @IntDataIni DateTime ")
            .AppendLine("Set @IntId = " & IntId)
            .AppendLine(" Set @IntDataIni = (Select convert(Char(10),intDataIni,120) from TbIntegrante where IntId = @IntId) ")

            .AppendLine("if @IntDataIni <= (select d.FaixaEtariaData from TbDefault d) ")
            .AppendLine("Begin ")
            '1-Adulto/2-Criança de 2 a 5/3-Colo com menos de 2 anos
            .AppendLine("   select case ")
            .AppendLine("    when (SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) >= (select FaixaEtariaAdulto from TbDefault) then '1'")
            .AppendLine("    when ((SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) < (select FaixaEtariaAdulto from TbDefault))  ")
            .AppendLine("    and ((SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25))>= (select LimiteIdadeColo from TbParametro)) then '2'")
            .AppendLine("    when (SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) < (select LimiteIdadeColo from TbParametro) then '3'")
            .AppendLine("   End ")
            .AppendLine("End ")
            .AppendLine("else if  @IntDataIni > (select d.FaixaEtariaData from TbDefault d) ")
            .AppendLine("Begin ")
            '1-Adulto/2-Criança de 2 a 5/3-Colo com menos de 2 anos
            .AppendLine("   select case ")
            .AppendLine("    when (SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) >= (select FaixaEtariaAdulto1 from TbDefault) then '1'")
            .AppendLine("    when ((SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) < (select FaixaEtariaAdulto1 from TbDefault))  ")
            .AppendLine("    and ((SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25))>= (select LimiteIdadeColo from TbParametro)) then '2'")
            .AppendLine("    when (SELECT FLOOR(DATEDIFF(DAY, (select IntDtNascimento from TbIntegrante i where i.intid = @IntId), GETDATE()) / 365.25)) < (select LimiteIdadeColo from TbParametro) then '3'")
            .AppendLine("  End ")
            .AppendLine("End ")
            Dim Resultado = CLng(Conn.executaTransacional(VarSql.ToString))
            Return Resultado
        End With
    End Function
    Public Function LocalizaCheckInSemEntregaChave() As IList
        VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
        With VarSql
            .AppendLine("SELECT i.IntNome,a.ApaDesc FROM TbIntegrante i ")
            .AppendLine("INNER JOIN TbHospedagem h ON h.IntId = i.IntId ")
            .AppendLine("INNER JOIN TbApartamento a ON a.ApaId = h.Apaid ")
            .AppendLine("WHERE CONVERT(CHAR(10),i.intDataIniReal,103) <> CONVERT(CHAR(10),(SELECT TOP 1 hh.HosDataIniReal FROM tbhospedagem hh WHERE hh.IntId = i.intid AND hh.hosDataIniReal IS NOT NULL ORDER BY hh.HosDataIniReal),103) ")
            .AppendLine("AND h.HosDataIniReal IS NOT NULL ")
            .AppendLine("AND EXISTS (SELECT 1 FROM TbReserva WHERE ResId = i.resid AND resId> 0 AND ResCaracteristica = 'I' AND resStatus IN ('E','P','T')) ")
            '.AppendLine("and i.IntDataFimReal > '2016-06-01' ")
            .AppendLine("ORDER BY a.ApaDesc ")
        End With
        Return PreecheLista(Conn.consulta(VarSql.ToString))
    End Function

    Private Function PreecheLista(ResultadoConsulta As Data.SqlClient.SqlDataReader) As IList
        Dim Lista As New ArrayList
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read()
                objCalculaIdadeIntegranteVO = New CalculaIdadeIntegranteVO
                With objCalculaIdadeIntegranteVO
                    .IntNome = ResultadoConsulta.Item("IntNome")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                End With
                Lista.Add(objCalculaIdadeIntegranteVO)
            End While
        End If
        Return Lista
    End Function
End Class
