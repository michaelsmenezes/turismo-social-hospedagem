Imports Microsoft.VisualBasic

Public Class IntegrantesVO
    Private _IntId As String
    Private _ApaId As String
    Private _Nome As String
    Private _CheckOut As Date
    Private _Emprestimos As String
    Private _ValorEmprestimos As String

    Public Property IntId() As String
        Get
            Return Me._IntId
        End Get
        Set(ByVal value As String)
            If (_IntId <> value) Then
                Me._IntId = value
            End If
        End Set
    End Property
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
    Public Property Nome() As String
        Get
            Return Me._Nome
        End Get
        Set(ByVal value As String)
            If (_Nome <> value) Then
                Me._Nome = value
            End If
        End Set
    End Property
    Public Property CheckOut() As Date
        Get
            Return Me._CheckOut
        End Get
        Set(ByVal value As Date)
            If (_CheckOut <> value) Then
                Me._CheckOut = value
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
    Public Property ValorEmprestimos() As String
        Get
            Return Me._ValorEmprestimos
        End Get
        Set(ByVal value As String)
            If (_ValorEmprestimos <> value) Then
                Me._ValorEmprestimos = value
            End If
        End Set
    End Property
End Class
