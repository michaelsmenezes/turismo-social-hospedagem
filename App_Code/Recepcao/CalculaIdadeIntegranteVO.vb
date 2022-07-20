Imports Microsoft.VisualBasic
Public Class CalculaIdadeIntegranteVO
    Private _IntNome As String
    Private _ApaDesc As String
    Public Property IntNome As String
        Get
            Return _IntNome
        End Get
        Set(value As String)
            _IntNome = value
        End Set
    End Property
    Public Property ApaDesc As String
        Get
            Return _ApaDesc
        End Get
        Set(value As String)
            _ApaDesc = value
        End Set
    End Property
End Class
