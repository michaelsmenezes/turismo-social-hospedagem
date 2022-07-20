Imports Microsoft.VisualBasic

Public Class EmprestimosVO
    Private _CseId As String
    Private _TMoId As String
    Private _EmpManual As String
    Private _EmpOrigem As String
    Private _IntId As String
    Private _EmpData As String
    Private _EmpDescricao As String
    Private _EmpValor As String
    Private _EmpQuantidade As String
    Private _EmpOperacao As String
    Private _EmpSituacao As String
    Private _EmpUsuario As String
    Private _EmpDataUsuario As String
    Private _IntNome As String
    Private _SomaEmprestimo As String
    'Operação
    Private _TMoDescricao As String


    Public Property CseId() As String
        Get
            Return Me._CseId
        End Get
        Set(ByVal value As String)
            If (_CseId <> value) Then
                Me._CseId = value
            End If
        End Set
    End Property
    Public Property TMoId() As String
        Get
            Return Me._TMoId
        End Get
        Set(ByVal value As String)
            If (_TMoId <> value) Then
                Me._TMoId = value
            End If
        End Set
    End Property
    Public Property EmpManual() As String
        Get
            Return Me._EmpManual
        End Get
        Set(ByVal value As String)
            If (_EmpManual <> value) Then
                Me._EmpManual = value
            End If
        End Set
    End Property
    Public Property EmpOrigem() As String
        Get
            Return Me._EmpOrigem
        End Get
        Set(ByVal value As String)
            If (_EmpOrigem <> value) Then
                Me._EmpOrigem = value
            End If
        End Set
    End Property

    Public Property EmpData() As String
        Get
            Return Me._EmpData
        End Get
        Set(ByVal value As String)
            If (_EmpData <> value) Then
                Me._EmpData = value
            End If
        End Set
    End Property
    Public Property EmpDescricao() As String
        Get
            Return Me._EmpDescricao
        End Get
        Set(ByVal value As String)
            If (_EmpDescricao <> value) Then
                Me._EmpDescricao = value
            End If
        End Set
    End Property
    Public Property EmpValor() As String
        Get
            Return Me._EmpValor
        End Get
        Set(ByVal value As String)
            If (_EmpValor <> value) Then
                Me._EmpValor = value
            End If
        End Set
    End Property
    Public Property EmpQuantidade() As String
        Get
            Return Me._EmpQuantidade
        End Get
        Set(ByVal value As String)
            If (_EmpQuantidade <> value) Then
                Me._EmpQuantidade = value
            End If
        End Set
    End Property
    Public Property EmpOperacao() As String
        Get
            Return Me._EmpOperacao
        End Get
        Set(ByVal value As String)
            If (_EmpOperacao <> value) Then
                Me._EmpOperacao = value
            End If
        End Set
    End Property
    Public Property EmpSituacao() As String
        Get
            Return Me._EmpSituacao
        End Get
        Set(ByVal value As String)
            If (_EmpSituacao <> value) Then
                Me._EmpSituacao = value
            End If
        End Set
    End Property
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
    Public Property EmpUsuario() As String
        Get
            Return Me._EmpUsuario
        End Get
        Set(ByVal value As String)
            If (_EmpUsuario <> value) Then
                Me._EmpUsuario = value
            End If
        End Set
    End Property
    Public Property EmpDataUsuario() As String
        Get
            Return Me._EmpDataUsuario
        End Get
        Set(ByVal value As String)
            If (_EmpDataUsuario <> value) Then
                Me._EmpDataUsuario = value
            End If
        End Set
    End Property
    Public Property IntNome() As String
        Get
            Return Me._IntNome
        End Get
        Set(ByVal value As String)
            If (_IntNome <> value) Then
                Me._IntNome = value
            End If
        End Set
    End Property
    Public Property SomaEmprestimo() As String
        Get
            Return Me._SomaEmprestimo
        End Get
        Set(ByVal value As String)
            If (_SomaEmprestimo <> value) Then
                Me._SomaEmprestimo = value
            End If
        End Set
    End Property
    Public Property TMoDescricao() As String
        Get
            Return Me._TMoDescricao
        End Get
        Set(ByVal value As String)
            If (_TMoDescricao <> value) Then
                Me._TMoDescricao = value
            End If
        End Set
    End Property
End Class
