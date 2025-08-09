Namespace SolarGainApp
    Public Module SolarGainCalculator
        Public Function Calculate(structure As SolarStructure, city As CityData) As Double()
            Dim values(11) As Double
            Dim radiation As Double()
            If Not city.Radiation.TryGetValue(structure.Orientation, radiation) Then
                Return values
            End If
            For i As Integer = 0 To 11
                values(i) = structure.Area * structure.Transmittance * structure.Shading * radiation(i)
            Next
            Return values
        End Function
    End Module
End Namespace
