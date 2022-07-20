Imports System
Imports Banco
Imports Microsoft.VisualBasic

Public Class ConsultasHospedeJaDAO
    Dim Conn As Conexao
    Dim ObjConsultasHospedeJaVO As ConsultasHospedeJaVO
    Dim varSql As Text.StringBuilder

    Public Sub New(banco As String)
        Conn = New Conexao(banco)
    End Sub

    Public Function ConsultaReservaPorResId(ResId As String) As ConsultasHospedeJaVO
        varSql = New Text.StringBuilder("SET NOCOUNT ON ")
        With varSql
            .Append("SELECT ResId,resDtInsercao,ResDtLimiteRetorno,resDataIni,resDataFim,resTipo ")
            .Append("FROM TBRESERVA where resid =  '" & ResId & "'")
        End With
        Return PreencheObjeto(Conn.consulta(varSql.ToString))
    End Function

    Private Function PreencheObjeto(ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As ConsultasHospedeJaVO
        If ResultadoConsulta.HasRows Then
            ResultadoConsulta.Read()
            ObjConsultasHospedeJaVO = New ConsultasHospedeJaVO
            With ObjConsultasHospedeJaVO
                .resDataFim = ResultadoConsulta.Item("resDataFim")
                .resDataIni = ResultadoConsulta.Item("resDataIni")
                .resDtInsercao = ResultadoConsulta.Item("resDtInsercao")
                .ResDtLimiteRetorno = ResultadoConsulta.Item("ResDtLimiteRetorno")
                .resId = ResultadoConsulta.Item("resId")
                If Convert.IsDBNull(ResultadoConsulta.Item("resTipo")) Then
                    .resTipo = "Null"
                Else
                    .resTipo = ResultadoConsulta.Item("resTipo")
                End If
            End With
        End If
        ResultadoConsulta.Close()
        Return ObjConsultasHospedeJaVO
    End Function

    Public Function TestaSeReservaFoiPelaInternet(ResId As String) As Long
        varSql = New Text.StringBuilder("Set Nocount on ")
        With varSql
            .Append("If exists (select 1 from tbreserva s with (nolock) where ressistema = 'portal-turismo' and s.resid = " & ResId & ") ")
            .Append("  Begin ")
            .Append("      Select 1 goto Saida ")
            .Append("  end ")
            .Append("else ")
            .Append("  begin ")
            .Append("      Select 0 goto Saida ")
            .Append("  end ")
            .Append(" Saida: ")
        End With
        Return Conn.executaTransacionalTestaRetorno(varSql.ToString)
    End Function



End Class
