Imports Microsoft.VisualBasic

Public Class ConferenciaVO
    Private _ApaId As String
    Private _ApaDesc As String
    Private _ApaStatus As String
    Private _ApaHTML As String
    Private _TipoAcomodacao As String
    Private _Emprestimos As String
    Private _DescEmprestimos As String
    Private _ApaCCusto As String
    Private _ApaArea As String

    Public Property ApaId() As String
        Get
            Return Me._ApaId
        End Get
        Set(ByVal value As String)
            If (_ApaId <> value) Then
                Me._ApaId = value
            End If
        End Set
    End Property
    Public Property ApaDesc() As String
        Get
            Return Me._ApaDesc
        End Get
        Set(ByVal value As String)
            If (_ApaDesc <> value) Then
                Me._ApaDesc = value
            End If
        End Set
    End Property
    Public Property ApaStatus() As String
        Get
            Return Me._ApaStatus
        End Get
        Set(ByVal value As String)
            If (_ApaStatus <> value) Then
                Me._ApaStatus = value
            End If
        End Set
    End Property
    Public Property ApaHTML() As String
        Get
            Return Me._ApaHTML
        End Get
        Set(ByVal value As String)
            If (_ApaHTML <> value) Then
                Me._ApaHTML = value
            End If
        End Set
    End Property
    Public Property TipoAcomodacao() As String
        Get
            Return Me._TipoAcomodacao
        End Get
        Set(ByVal value As String)
            If (_TipoAcomodacao <> value) Then
                Me._TipoAcomodacao = value
            End If
        End Set
    End Property
    Public Property Emprestimos() As String
        Get
            Return Me._Emprestimos
        End Get
        Set(ByVal value As String)
            If (_Emprestimos <> value) Then
                Me._Emprestimos = value
            End If
        End Set
    End Property
    Public Property DescEmprestimos() As String
        Get
            Return Me._DescEmprestimos
        End Get
        Set(ByVal value As String)
            If (_DescEmprestimos <> value) Then
                Me._DescEmprestimos = value
            End If
        End Set
    End Property
    Public Property ApaCCusto() As String
        Get
            Return Me._ApaCCusto
        End Get
        Set(ByVal value As String)
            If (_ApaCCusto <> value) Then
                Me._ApaCCusto = value
            End If
        End Set
    End Property
    Public Property ApaArea() As String
        Get
            Return Me._ApaArea
        End Get
        Set(ByVal value As String)
            If (_ApaArea <> value) Then
                Me._ApaArea = value
            End If
        End Set
    End Property
End Class
