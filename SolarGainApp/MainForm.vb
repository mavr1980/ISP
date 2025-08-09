Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting

Namespace SolarGainApp
    Public Class MainForm
        Inherits Form

        Private ReadOnly comboCity As ComboBox
        Private ReadOnly dataGrid As DataGridView
        Private ReadOnly btnCalculate As Button
        Private ReadOnly chart As Chart
        Private ReadOnly cities As List(Of CityData)

        Public Sub New()
            Me.Text = "Теплопоступления от солнечной радиации"
            Me.Width = 1000
            Me.Height = 700

            comboCity = New ComboBox() With {
                .Left = 20,
                .Top = 20,
                .Width = 200,
                .DropDownStyle = ComboBoxStyle.DropDownList
            }
            cities = CityData.CreateDefault()
            comboCity.DataSource = cities
            comboCity.DisplayMember = "Name"
            Me.Controls.Add(comboCity)

            dataGrid = New DataGridView() With {
                .Left = 20,
                .Top = 60,
                .Width = 600,
                .Height = 250,
                .AllowUserToAddRows = False,
                .RowHeadersVisible = False
            }
            dataGrid.Columns.Add("Name", "Название")
            dataGrid.Columns.Add("Area", "Площадь, м²")
            Dim orientCol As New DataGridViewComboBoxColumn()
            orientCol.Name = "Orientation"
            orientCol.HeaderText = "Ориентация"
            orientCol.Items.AddRange("North", "East", "South", "West")
            dataGrid.Columns.Add(orientCol)
            dataGrid.Columns.Add("Transmittance", "τ")
            dataGrid.Columns.Add("Shading", "η")
            For i As Integer = 1 To 10
                dataGrid.Rows.Add()
            Next
            Me.Controls.Add(dataGrid)

            btnCalculate = New Button() With {
                .Text = "Расчет",
                .Left = 20,
                .Top = 330
            }
            AddHandler btnCalculate.Click, AddressOf OnCalculate
            Me.Controls.Add(btnCalculate)

            chart = New Chart() With {
                .Left = 20,
                .Top = 370,
                .Width = 950,
                .Height = 300
            }
            Dim ca As New ChartArea()
            ca.AxisX.Title = "Месяц"
            ca.AxisY.Title = "Q, кВт·ч"
            chart.ChartAreas.Add(ca)
            Me.Controls.Add(chart)
        End Sub

        Private Sub OnCalculate(sender As Object, e As EventArgs)
            Dim city As CityData = TryCast(comboCity.SelectedItem, CityData)
            If city Is Nothing Then
                MessageBox.Show("Выберите город", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            chart.Series.Clear()
            Dim totals(11) As Double
            For Each row As DataGridViewRow In dataGrid.Rows
                If row.Cells("Area").Value IsNot Nothing AndAlso row.Cells("Orientation").Value IsNot Nothing Then
                    Dim area As Double
                    If Not Double.TryParse(Convert.ToString(row.Cells("Area").Value), area) Then Continue For
                    Dim trans As Double = 1.0
                    Double.TryParse(Convert.ToString(row.Cells("Transmittance").Value), trans)
                    Dim shade As Double = 1.0
                    Double.TryParse(Convert.ToString(row.Cells("Shading").Value), shade)

                    Dim structure As New SolarStructure With {
                        .Name = Convert.ToString(row.Cells("Name").Value),
                        .Area = area,
                        .Orientation = Convert.ToString(row.Cells("Orientation").Value),
                        .Transmittance = trans,
                        .Shading = shade
                    }
                    Dim values = SolarGainCalculator.Calculate(structure, city)

                    Dim seriesName As String
                    If String.IsNullOrWhiteSpace(structure.Name) Then
                        seriesName = $"Конструкция {row.Index + 1}"
                    Else
                        seriesName = structure.Name
                    End If

                    Dim s = chart.Series.Add(seriesName)
                    s.ChartType = SeriesChartType.Line
                    For i As Integer = 0 To 11
                        s.Points.AddXY(i + 1, values(i))
                        totals(i) += values(i)
                    Next
                End If
            Next

            Dim totalSeries = chart.Series.Add("Итого")
            totalSeries.ChartType = SeriesChartType.Line
            totalSeries.BorderWidth = 2
            totalSeries.Color = Drawing.Color.Red
            For i As Integer = 0 To 11
                totalSeries.Points.AddXY(i + 1, totals(i))
            Next
        End Sub
    End Class
End Namespace
