Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting

Namespace HeatGainApp
    Public Class MainForm
        Inherits Form

        Private ReadOnly cityCombo As ComboBox
        Private ReadOnly grid As DataGridView
        Private ReadOnly calcButton As Button
        Private ReadOnly chart As Chart
        Private cities As List(Of CitySolarData)

        Public Sub New()
            Me.Text = "Solar Heat Gain Calculator"
            Me.Width = 1000
            Me.Height = 700

            cityCombo = New ComboBox() With {
                .Location = New Drawing.Point(10, 10),
                .Width = 200,
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            Me.Controls.Add(cityCombo)

            grid = New DataGridView() With {
                .Location = New Drawing.Point(10, 50),
                .Size = New Drawing.Size(450, 250),
                .AllowUserToAddRows = False,
                .RowHeadersVisible = False
            }
            grid.Columns.Add("Name", "Name")
            grid.Columns.Add("Area", "Area (m²)")
            Dim orientCol As New DataGridViewComboBoxColumn() With {
                .Name = "Orientation",
                .HeaderText = "Orientation",
                .Items = {"N", "E", "S", "W"}
            }
            grid.Columns.Add(orientCol)
            grid.Columns.Add("Transmittance", "Transmittance")
            grid.Columns.Add("Shading", "Shading")
            grid.RowCount = 10
            Me.Controls.Add(grid)

            calcButton = New Button() With {
                .Text = "Calculate",
                .Location = New Drawing.Point(10, 310)
            }
            AddHandler calcButton.Click, AddressOf OnCalculate
            Me.Controls.Add(calcButton)

            chart = New Chart() With {
                .Location = New Drawing.Point(470, 50),
                .Size = New Drawing.Size(500, 400)
            }
            chart.ChartAreas.Add(New ChartArea("main"))
            Me.Controls.Add(chart)

            LoadCityData()
        End Sub

        Private Sub LoadCityData()
            cities = New List(Of CitySolarData)()
            cities.Add(New CitySolarData With {
                .Name = "Moscow",
                .Radiation = CreateDefaultRadiation(0.75)
            })
            cities.Add(New CitySolarData With {
                .Name = "Saint Petersburg",
                .Radiation = CreateDefaultRadiation(0.6)
            })
            cities.Add(New CitySolarData With {
                .Name = "Novosibirsk",
                .Radiation = CreateDefaultRadiation(0.8)
            })
            cityCombo.DataSource = cities
            cityCombo.DisplayMember = "Name"
        End Sub

        Private Function CreateDefaultRadiation(baseVal As Double) As Dictionary(Of String, Double())
            Dim dict As New Dictionary(Of String, Double())()
            Dim orientations = New String() {"N", "E", "S", "W"}
            For Each o In orientations
                Dim arr(11) As Double
                For i As Integer = 0 To 11
                    arr(i) = baseVal * Math.Max(0, Math.Sin((i + 6) / 24.0 * Math.PI)) * OrientationFactor(o)
                Next
                dict(o) = arr
            Next
            Return dict
        End Function

        Private Function OrientationFactor(o As String) As Double
            Select Case o
                Case "S"
                    Return 1.0
                Case "E", "W"
                    Return 0.7
                Case Else
                    Return 0.3
            End Select
        End Function

        Private Sub OnCalculate(sender As Object, e As EventArgs)
            chart.Series.Clear()
            Dim city = CType(cityCombo.SelectedItem, CitySolarData)
            Dim total(11) As Double

            For Each row As DataGridViewRow In grid.Rows
                If row.IsNewRow Then Continue For
                Dim name = CStr(row.Cells("Name").Value)
                Dim area = Convert.ToDouble(row.Cells("Area").Value)
                Dim orient = CStr(row.Cells("Orientation").Value)
                Dim trans = Convert.ToDouble(row.Cells("Transmittance").Value)
                Dim shade = Convert.ToDouble(row.Cells("Shading").Value)
                If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(orient) Then Continue For

                Dim gains = ComputeGains(city, orient, area, trans, shade)
                Dim series = chart.Series.Add(name)
                series.ChartType = SeriesChartType.Line
                For i As Integer = 0 To gains.Length - 1
                    series.Points.AddXY(i + 6, gains(i))
                    total(i) += gains(i)
                Next
            Next

            Dim totalSeries = chart.Series.Add("Total")
            totalSeries.ChartType = SeriesChartType.Line
            totalSeries.BorderWidth = 2
            For i As Integer = 0 To total.Length - 1
                totalSeries.Points.AddXY(i + 6, total(i))
            Next
        End Sub

        Private Function ComputeGains(city As CitySolarData, orient As String, area As Double, trans As Double, shade As Double) As Double()
            Dim rad = city.Radiation(orient)
            Dim gains(rad.Length - 1) As Double
            For i As Integer = 0 To rad.Length - 1
                gains(i) = area * trans * shade * rad(i)
            Next
            Return gains
        End Function
    End Class
End Namespace
