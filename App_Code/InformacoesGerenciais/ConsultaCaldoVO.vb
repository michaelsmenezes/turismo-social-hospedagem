Imports Microsoft.VisualBasic

Public Class ConsultaCaldoVO
    Private _Data As String
    Private _Categoria As String
    Private _Qtde As String
    Private _Valor As String
    Public Property Data() As String
        Get
            Return Me._Data
        End Get
        Set(ByVal value As String)
            If (_Data <> value) Then
                Me._Data = value
            End If
        End Set
    End Property
    Public Property Categoria() As String
        Get
            Return Me._Categoria
        End Get
        Set(ByVal value As String)
            If (_Categoria <> value) Then
                Me._Categoria = value
            End If
        End Set
    End Property
    Public Property Qtde() As String
        Get
            Return Me._Qtde
        End Get
        Set(ByVal value As String)
            If (_Qtde <> value) Then
                Me._Qtde = value
            End If
        End Set
    End Property
    Public Property Valor() As String
        Get
            Return Me._Valor
        End Get
        Set(ByVal value As String)
            If (_Valor <> value) Then
                Me._Valor = value
            End If
        End Set
    End Property
End Class
