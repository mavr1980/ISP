Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting

Public Class MainForm
    Inherits Form

    Private cityCombo As ComboBox
    Private surfaceGrid As DataGridView
    Private calcButton As Button
    Private resultChart As Chart

    Public Sub New()
        Me.Text = "Solar Heat Gain Calculator"
        Me.Width = 1000
        Me.Height = 700

        cityCombo = New ComboBox() With {
            .Left = 10,
            .Top = 10,
            .Width = 150,
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cityCombo.Items.AddRange(SolarData.CityNames)
        cityCombo.SelectedIndex = 0
        Me.Controls.Add(cityCombo)

        surfaceGrid = New DataGridView() With {
            .Left = 10,
            .Top = 50,
            .Width = 600,
            .Height = 300,
            .AllowUserToAddRows = False,
            .RowCount = 10
        }
        surfaceGrid.Columns.Add("Type", "Type")
        Dim orientCol As New DataGridViewComboBoxColumn()
        orientCol.HeaderText = "Orientation"
        orientCol.Items.AddRange("North", "East", "South", "West")
        surfaceGrid.Columns.Add(orientCol)
        surfaceGrid.Columns.Add("Area", "Area (m²)")
        surfaceGrid.Columns.Add("SHGC", "SHGC")
        Me.Controls.Add(surfaceGrid)

        calcButton = New Button() With {
            .Text = "Calculate",
            .Left = 10,
            .Top = 360
        }
        AddHandler calcButton.Click, AddressOf OnCalculate
        Me.Controls.Add(calcButton)

        resultChart = New Chart() With {
            .Left = 620,
            .Top = 50,
            .Width = 350,
            .Height = 300
        }
        Dim area As New ChartArea("Main")
        area.AxisX.Interval = 1
        area.AxisX.Title = "Month"
        area.AxisY.Title = "Heat Gain (kWh)"
        resultChart.ChartAreas.Add(area)
        Me.Controls.Add(resultChart)
    End Sub

    Private Sub OnCalculate(sender As Object, e As EventArgs)
        resultChart.Series.Clear()

        Dim city As String = cityCombo.SelectedItem.ToString()
        Dim radiation = SolarData.MonthlyRadiation(city)

        Dim total(11) As Double

        For row As Integer = 0 To surfaceGrid.RowCount - 1
            Dim type = Convert.ToString(surfaceGrid.Rows(row).Cells(0).Value)
            Dim orientation = Convert.ToString(surfaceGrid.Rows(row).Cells(1).Value)
            Dim areaValue As Double
            Double.TryParse(Convert.ToString(surfaceGrid.Rows(row).Cells(2).Value), areaValue)
            Dim shgc As Double
            Double.TryParse(Convert.ToString(surfaceGrid.Rows(row).Cells(3).Value), shgc)

            If String.IsNullOrEmpty(type) OrElse areaValue <= 0 OrElse shgc <= 0 Then
                Continue For
            End If

            Dim orientationFactor = SolarData.OrientationFactor(orientation)
            Dim gains(11) As Double
            For m As Integer = 0 To 11
                gains(m) = radiation(m) * areaValue * shgc * orientationFactor
                total(m) += gains(m)
            Next

            Dim series = New Series($"Surface {row + 1}: {type}") With {
                .ChartType = SeriesChartType.Line
            }
            For m As Integer = 0 To 11
                series.Points.AddXY(m + 1, gains(m))
            Next
            resultChart.Series.Add(series)
        Next

        Dim totalSeries = New Series("Total") With {
            .ChartType = SeriesChartType.Line,
            .BorderWidth = 3
        }
        For m As Integer = 0 To 11
            totalSeries.Points.AddXY(m + 1, total(m))
        Next
        resultChart.Series.Add(totalSeries)
    End Sub
End Class
