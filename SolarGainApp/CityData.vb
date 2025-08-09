Imports System.Collections.Generic

Namespace SolarGainApp
    Public Class CityData
        Public Property Name As String
        Public Property Radiation As Dictionary(Of String, Double())

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Shared Function CreateDefault() As List(Of CityData)
            Dim list As New List(Of CityData)()

            Dim moscow As New CityData()
            moscow.Name = "Москва"
            moscow.Radiation = New Dictionary(Of String, Double()) From {
                {"North", New Double() {15, 20, 35, 45, 55, 60, 50, 45, 30, 20, 10, 5}},
                {"East",  New Double() {60, 80, 110, 130, 140, 135, 120, 115, 100, 80, 60, 50}},
                {"South", New Double() {90, 100, 120, 140, 150, 160, 150, 140, 130, 110, 100, 90}},
                {"West",  New Double() {60, 80, 110, 130, 140, 135, 120, 115, 100, 80, 60, 50}}
            }
            list.Add(moscow)

            Dim petersburg As New CityData()
            petersburg.Name = "Санкт-Петербург"
            petersburg.Radiation = New Dictionary(Of String, Double()) From {
                {"North", New Double() {10, 15, 25, 35, 45, 50, 45, 40, 25, 15, 8, 5}},
                {"East",  New Double() {50, 70, 95, 110, 120, 115, 100, 95, 80, 60, 45, 35}},
                {"South", New Double() {80, 90, 110, 130, 140, 150, 140, 130, 120, 100, 90, 80}},
                {"West",  New Double() {50, 70, 95, 110, 120, 115, 100, 95, 80, 60, 45, 35}}
            }
            list.Add(petersburg)

            Dim sochi As New CityData()
            sochi.Name = "Сочи"
            sochi.Radiation = New Dictionary(Of String, Double()) From {
                {"North", New Double() {30, 40, 60, 80, 100, 110, 100, 90, 70, 50, 35, 30}},
                {"East",  New Double() {80, 100, 135, 160, 170, 160, 150, 145, 130, 110, 90, 80}},
                {"South", New Double() {110, 120, 150, 170, 180, 190, 185, 175, 165, 140, 120, 110}},
                {"West",  New Double() {80, 100, 135, 160, 170, 160, 150, 145, 130, 110, 90, 80}}
            }
            list.Add(sochi)

            Dim novosibirsk As New CityData()
            novosibirsk.Name = "Новосибирск"
            novosibirsk.Radiation = New Dictionary(Of String, Double()) From {
                {"North", New Double() {10, 20, 30, 40, 50, 55, 45, 40, 25, 15, 8, 5}},
                {"East",  New Double() {55, 75, 100, 120, 130, 125, 110, 105, 90, 70, 55, 40}},
                {"South", New Double() {85, 95, 115, 135, 145, 155, 150, 140, 125, 105, 90, 80}},
                {"West",  New Double() {55, 75, 100, 120, 130, 125, 110, 105, 90, 70, 55, 40}}
            }
            list.Add(novosibirsk)

            Return list
        End Function
    End Class
End Namespace
