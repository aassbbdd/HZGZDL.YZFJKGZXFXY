using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEV_Chart_Test
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void buttonPen1_Click(object sender, EventArgs e)
        {
            

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Area area1 = new Steema.TeeChart.Styles.Area(tChart1.Chart);
            Steema.TeeChart.Styles.Bar bar1 = new Steema.TeeChart.Styles.Bar(tChart1.Chart);
            Steema.TeeChart.Styles.Line line1 = new Steema.TeeChart.Styles.Line(tChart1.Chart);
            //Use Series common properties
            tChart1.Series[0].FillSampleValues(10);
            tChart1.Series[1].FillSampleValues(10);
            tChart1.Series[2].FillSampleValues(10);
            tChart1.Series[1].Marks.Visible = false;
            tChart1.Series[2].Marks.Visible = false;


            //Modify Bar specific properties
            bar1.BarStyle = Steema.TeeChart.Styles.BarStyles.Pyramid; //Change Bar type
            bar1.Pen.Color = Color.Yellow; //Bar bounding lines colour               //Modify Line specific properties
            line1.Stairs = true; //Set line to Stairs
            line1.LinePen.Color = Color.Blue; //LineSeries bounding lines colour
            //Modify Area specific properties
            area1.AreaBrush.Style = System.Drawing.Drawing2D.HatchStyle.Cross; //Area fill pattern  
        }
    }
}
