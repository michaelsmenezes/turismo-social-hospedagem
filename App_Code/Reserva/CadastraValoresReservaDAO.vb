Imports Microsoft.VisualBasic
Imports Banco
Imports System

Public Class CadastraValoresReservaDAO
    Dim Conn As Conexao
    Dim ObjCadastraNovosValoresDAO As CadastraValoresReservaDAO
    Public Sub New(Banco As String)
        Conn = New Conexao(Banco)
    End Sub

    Public Function CadastraNovoValores(Comando As String) As Long
        Dim Resultado As Long
        Try
            Resultado = Conn.executaTransacionalTestaRetorno(Comando.ToString)
        Catch ex As Exception
            Resultado = 0
        End Try
        Return Resultado
    End Function
End Class
