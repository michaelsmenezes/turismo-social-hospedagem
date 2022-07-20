Imports Microsoft.VisualBasic

Public Class RestaurantePrevisoesVO
    Private _DescricaoRefeicao As String
    Private _Previsao As Long
    Private _EmEstada As Long
    Private _Pago As Long
    Private _APagar As Long

    Public Property DescricaoRefeicao() As String
        Get
            Return Me._DescricaoRefeicao
        End Get
        Set(ByVal value As String)
            If (_DescricaoRefeicao <> value) Then
                Me._DescricaoRefeicao = value
            End If
        End Set
    End Property
    Public Property Previsao() As Long
        Get
            Return Me._Previsao
        End Get
        Set(ByVal value As Long)
            If (_Previsao <> value) Then
                Me._Previsao = value
            End If
        End Set
    End Property
    Public Property EmEstada() As Long
        Get
            Return Me._EmEstada
        End Get
        Set(ByVal value As Long)
            If (_EmEstada <> value) Then
                Me._EmEstada = value
            End If
        End Set
    End Property
    Public Property Pago() As Long
        Get
            Return Me._Pago
        End Get
        Set(ByVal value As Long)
            If (_Pago <> value) Then
                Me._Pago = value
            End If
        End Set
    End Property
    Public Property APagar() As Long
        Get
            Return Me._APagar
        End Get
        Set(ByVal value As Long)
            If (_APagar <> value) Then
                Me._APagar = value
            End If
        End Set
    End Property

End Class
