Imports Microsoft.VisualBasic

Public Class EmprestimosRecepcaoVO
    Private _proId As String
    Private _proDescricao As String
    Private _proValor As String
    Private _proUsuario As String
    Private _proUsuarioData As String

    Public Property proId As String
        Get
            Return _proId
        End Get
        Set(value As String)
            _proId = value
        End Set
    End Property
    Public Property proDescricao As String
        Get
            Return _proDescricao
        End Get
        Set(value As String)
            _proDescricao = value
        End Set
    End Property
    Public Property proValor As String
        Get
            Return _proValor
        End Get
        Set(value As String)
            _proValor = value
        End Set
    End Property
    Public Property proUsuario As String
        Get
            Return _proUsuario
        End Get
        Set(value As String)
            _proUsuario = value
        End Set
    End Property
    Public Property proUsuarioData As String
        Get
            Return _proUsuarioData
        End Get
        Set(value As String)
            _proUsuarioData = value
        End Set
    End Property
End Class
