Imports Microsoft.VisualBasic

Public Class ConsultasHospedeJaVO
    Private _resDtInsercao As String
    Private _ResDtLimiteRetorno As String
    Private _resDataIni As String
    Private _resDataFim As String
    Private _resTipo As String
    Private _resId As String

    Public Property resDtInsercao() As String
        Get
            Return _resDtInsercao
        End Get
        Set(value As String)
            _resDtInsercao = value
        End Set
    End Property
    Public Property ResDtLimiteRetorno As String
        Get
            Return _ResDtLimiteRetorno
        End Get
        Set(value As String)
            _ResDtLimiteRetorno = value
        End Set
    End Property
    Public Property resDataIni As String
        Get
            Return _resDataIni
        End Get
        Set(value As String)
            _resDataIni = value
        End Set
    End Property
    Public Property resDataFim As String
        Get
            Return _resDataFim
        End Get
        Set(value As String)
            _resDataFim = value
        End Set
    End Property
    Public Property resTipo As String
        Get
            Return _resTipo
        End Get
        Set(value As String)
            _resTipo = value
        End Set
    End Property
    Public Property resId As String
        Get
            Return _resId
        End Get
        Set(value As String)
            _resId = value
        End Set
    End Property



End Class
