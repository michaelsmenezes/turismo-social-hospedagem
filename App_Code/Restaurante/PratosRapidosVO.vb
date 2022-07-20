Imports Microsoft.VisualBasic

Public Class PratosRapidosVO
    Private _praId As Long
    Private _Opcao As Long
    Private _Descricao As String
    Private _Solicitado As Long
    Private _NaoPago As Long
    Private _Confirmado As Long
    Private _Consumido As Long
    Private _Consumir As Long
    Public Property Opcao() As Long
        Get
            Return Me._Opcao
        End Get
        Set(ByVal value As Long)
            If (_Opcao <> value) Then
                Me._Opcao = value
            End If
        End Set
    End Property
    Public Property praId() As Long
        Get
            Return Me._praId
        End Get
        Set(ByVal value As Long)
            If (_praId <> value) Then
                Me._praId = value
            End If
        End Set
    End Property
    Public Property Descricao() As String
        Get
            Return Me._Descricao
        End Get
        Set(ByVal value As String)
            If (_Descricao <> value) Then
                Me._Descricao = value
            End If
        End Set
    End Property
    Public Property Solicitado() As Long
        Get
            Return Me._Solicitado
        End Get
        Set(ByVal value As Long)
            If (_Solicitado <> value) Then
                Me._Solicitado = value
            End If
        End Set
    End Property
    Public Property NaoPago() As Long
        Get
            Return Me._NaoPago
        End Get
        Set(ByVal value As Long)
            If (_NaoPago <> value) Then
                Me._NaoPago = value
            End If
        End Set
    End Property
    Public Property Confirmado() As Long
        Get
            Return Me._Confirmado
        End Get
        Set(ByVal value As Long)
            If (_Confirmado <> value) Then
                Me._Confirmado = value
            End If
        End Set
    End Property
    Public Property Consumido() As Long
        Get
            Return Me._Consumido
        End Get
        Set(ByVal value As Long)
            If (_Consumido <> value) Then
                Me._Consumido = value
            End If
        End Set
    End Property
    Public Property Consumir() As Long
        Get
            Return Me._Consumir
        End Get
        Set(ByVal value As Long)
            If (_Consumir <> value) Then
                Me._Consumir = value
            End If
        End Set
    End Property
End Class
