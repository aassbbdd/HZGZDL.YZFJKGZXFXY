using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace DEV_Chart_Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Create a new chart.
            ChartControl splineChart = new ChartControl();

            // Create a spline series.
            Series series1 = new Series("Series 1", ViewType.Spline);

            // Add points to it.
            series1.Points.Add(new SeriesPoint(0.5, 3));
            series1.Points.Add(new SeriesPoint(10, 12));
            series1.Points.Add(new SeriesPoint(3, 4));
            series1.Points.Add(new SeriesPoint(4, 17));
            series1.Points.Add(new SeriesPoint(5, 3));
            series1.Points.Add(new SeriesPoint(6, 12));
            series1.Points.Add(new SeriesPoint(7, 4));
            series1.Points.Add(new SeriesPoint(8, 17));

            // Add the series to the chart.
            splineChart.Series.Add(series1);

            // Set the numerical argument scale types for the series,
            // as it is qualitative, by default.
            series1.ArgumentScaleType = ScaleType.Numerical;

            // Access the view-type-specific options of the series.
            ((SplineSeriesView)series1.View).LineTensionPercent = 90;

            // Access the type-specific options of the diagram.
            ((XYDiagram)splineChart.Diagram).EnableAxisXZooming = true;

            // Hide the legend (if necessary).
            splineChart.Legend.Visible = false;

            // Add a title to the chart (if necessary).
            splineChart.Titles.Add(new ChartTitle());
            splineChart.Titles[0].Text = "A Spline Chart";

            // Add the chart to the form.
            splineChart.Dock = DockStyle.Fill;
            panelControl1.Controls.Add(splineChart);

        }
    }
}
