Imports System.Linq

Public Module SolarData
    Public ReadOnly Cities As Dictionary(Of String, Double()) = New Dictionary(Of String, Double()) From {
        {"Moscow", New Double() {20,30,50,80,100,120,110,90,60,40,25,15}},
        {"Saint Petersburg", New Double() {15,25,45,70,90,110,100,80,55,35,20,10}}
    }

    Public ReadOnly OrientationFactors As Dictionary(Of String, Double) = New Dictionary(Of String, Double) From {
        {"North", 0.5},
        {"East", 0.75},
        {"South", 1.0},
        {"West", 0.75}
    }

    Public ReadOnly Property CityNames As String()
        Get
            Return Cities.Keys.ToArray()
        End Get
    End Property

    Public Function MonthlyRadiation(city As String) As Double()
        Return Cities(city)
    End Function

    Public Function OrientationFactor(orientation As String) As Double
        If orientation Is Nothing OrElse Not OrientationFactors.ContainsKey(orientation) Then
            Return 1.0
        End If
        Return OrientationFactors(orientation)
    End Function
End Module
