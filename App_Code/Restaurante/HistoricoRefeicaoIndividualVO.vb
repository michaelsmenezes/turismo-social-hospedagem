Imports Microsoft.VisualBasic

Public Class HistoricoRefeicaoIndividualVO
    Private _Dia As Long
    Private _Comerciario As Long
    Private _Dependente As Long
    Private _Conveniado As Long
    Private _Usuario As Long
    Private _PasComerciario As Long
    Private _PasDependente As Long
    Private _PasUsuario As Long
    Private _Cortesias As Long
    Private _InsercaoManual As Long
    Private _Menor5 As Long
    Private _Servidores As Long
    Private _TotalLinha As Long

    Public Property Dia() As Long
        Get
            Return Me._Dia
        End Get
        Set(ByVal value As Long)
            If (_Dia <> value) Then
                Me._Dia = value
            End If
        End Set
    End Property
    Public Property Comerciario() As Long
        Get
            Return Me._Comerciario
        End Get
        Set(ByVal value As Long)
            If (_Comerciario <> value) Then
                Me._Comerciario = value
            End If
        End Set
    End Property
    Public Property Dependente() As Long
        Get
            Return Me._Dependente
        End Get
        Set(ByVal value As Long)
            If (_Dependente <> value) Then
                Me._Dependente = value
            End If
        End Set
    End Property
    Public Property Conveniado() As Long
        Get
            Return Me._Conveniado
        End Get
        Set(ByVal value As Long)
            If (_Conveniado <> value) Then
                Me._Conveniado = value
            End If
        End Set
    End Property
    Public Property Usuario() As Long
        Get
            Return Me._Usuario
        End Get
        Set(ByVal value As Long)
            If (_Usuario <> value) Then
                Me._Usuario = value
            End If
        End Set
    End Property
    Public Property PasComerciario() As Long
        Get
            Return Me._PasComerciario
        End Get
        Set(ByVal value As Long)
            If (_PasComerciario <> value) Then
                Me._PasComerciario = value
            End If
        End Set
    End Property
    Public Property PasDependente() As Long
        Get
            Return Me._PasDependente
        End Get
        Set(ByVal value As Long)
            If (_PasDependente <> value) Then
                Me._PasDependente = value
            End If
        End Set
    End Property
    Public Property PasUsuario() As Long
        Get
            Return Me._PasUsuario
        End Get
        Set(ByVal value As Long)
            If (_PasUsuario <> value) Then
                Me._PasUsuario = value
            End If
        End Set
    End Property
    Public Property Cortesias() As Long
        Get
            Return Me._Cortesias
        End Get
        Set(ByVal value As Long)
            If (_Cortesias <> value) Then
                Me._Cortesias = value
            End If
        End Set
    End Property
    Public Property InsercaoManual() As Long
        Get
            Return Me._InsercaoManual
        End Get
        Set(ByVal value As Long)
            If (_InsercaoManual <> value) Then
                Me._InsercaoManual = value
            End If
        End Set
    End Property
    Public Property Menor5() As Long
        Get
            Return Me._Menor5
        End Get
        Set(ByVal value As Long)
            If (_Menor5 <> value) Then
                Me._Menor5 = value
            End If
        End Set
    End Property
    Public Property Servidores() As Long
        Get
            Return Me._Servidores
        End Get
        Set(ByVal value As Long)
            If (_Servidores <> value) Then
                Me._Servidores = value
            End If
        End Set
    End Property
    Public Property TotalLinha() As Long
        Get
            Return Me._TotalLinha
        End Get
        Set(ByVal value As Long)
            If (_TotalLinha <> value) Then
                Me._TotalLinha = value
            End If
        End Set
    End Property


End Class
