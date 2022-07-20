Imports Microsoft.VisualBasic

Public Class FuncionarioVO
    Private _Matricula As String
    Private _Nome As String
    Private _CentroCusto As String
    Public Property Matricula() As String
        Get
            Return Me._Matricula
        End Get
        Set(ByVal value As String)
            If (_Matricula <> value) Then
                Me._Matricula = value
            End If
        End Set
    End Property
    Public Property Nome() As String
        Get
            Return (Me._Nome)
        End Get
        Set(ByVal value As String)
            If (_Nome <> value) Then
                Me._Nome = value
            End If
        End Set
    End Property
    Public Property CentroCusto() As String
        Get
            Return Me._CentroCusto
        End Get
        Set(ByVal value As String)
            If (_CentroCusto <> value) Then
                Me._CentroCusto = value
            End If
        End Set
    End Property
End Class
