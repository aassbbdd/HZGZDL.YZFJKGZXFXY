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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create a line series.

            // Add points to it.
            series1.Points.Add(new SeriesPoint(1.1, 2.2));
            series1.Points.Add(new SeriesPoint(2, 12));
            series1.Points.Add(new SeriesPoint(3, 14));
            series1.Points.Add(new SeriesPoint(4, 17));

            series2.Points.Add(new SeriesPoint(1, 22));
            series2.Points.Add(new SeriesPoint(2, -11.22));
            series2.Points.Add(new SeriesPoint(3.3, 64.5));
            series2.Points.Add(new SeriesPoint(4, -17));
            Test_Chart(series1, series2);

        }

        Series series1 = new Series("Series 1", ViewType.Line);

        Series series2 = new Series("Series 2", ViewType.Line);
        ChartControl lineChart = new ChartControl();
        private void Test_Chart(Series series1, Series series2)
        {
            // Create a new chart.
           // ChartControl lineChart = new ChartControl();


            // Add the series to the chart.
            lineChart.Series.Add(series1);
            lineChart.Series.Add(series2);

            // Set the numerical argument scale types for the series,
            // as it is qualitative, by default.
            series1.ArgumentScaleType = ScaleType.Numerical;

            // Access the view-type-specific options of the series.
            ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Triangle;
            ((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Dash;

            // Access the type-specific options of the diagram.
            ((XYDiagram)lineChart.Diagram).EnableAxisXZooming = true;

            // Hide the legend (if necessary).
            lineChart.Legend.Visible = false;

            // Add a title to the chart (if necessary).
            lineChart.Titles.Add(new ChartTitle());
            lineChart.Titles[0].Text = "A Line Chart";

            // Add the chart to the form.
            lineChart.Dock = DockStyle.Fill;
            // this.Controls.Add(lineChart);

            this.panelControl1.Controls.Add(lineChart);
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // Create a line series.
            //Series series1 = new Series("Series 1", ViewType.Line);

            //Series series2 = new Series("Series 2", ViewType.Line);
            lineChart.Series.Remove(series1);
            lineChart.Series.Remove(series2);
            series1.Points.Add(new SeriesPoint(5, 2.2));
            series1.Points.Add(new SeriesPoint(6, 12));
            series1.Points.Add(new SeriesPoint(7, 14));
            series1.Points.Add(new SeriesPoint(8, 17));

            series2.Points.Add(new SeriesPoint(5, 22));
            series2.Points.Add(new SeriesPoint(6, -11.22));
            series2.Points.Add(new SeriesPoint(7, 64.5));
            series2.Points.Add(new SeriesPoint(8, -17));



            lineChart.Series.Add(series1);
            lineChart.Series.Add(series2);

            this.panelControl1.Refresh();
        }
    }
}
