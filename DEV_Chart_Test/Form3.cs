using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Udp_Agreement.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Status;

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
            dd();
            Init();
        }
        double num = 0.01;
        private DataTable GetData()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("width");
            dt.Columns.Add("vibration");
           // double num = 0.01;
            int cs = 1000;
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 2;
                }
                else
                {
                    b = -2;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 5;
                }
                else
                {
                    b = -5;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 1;
                }
                else
                {
                    b = 1;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 6;
                }
                else
                {
                    b = -6;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 2;
                }
                else
                {
                    b = -2;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            return dt;
        }

        private void dd()
        {
            Line line1 = new Line();
            Line line2 = new Line();
            tChart1.Aspect.View3D = false;
            tChart1.Panel.Gradient.Visible = true;
            tChart1.Header.Text = "TeeChart Multiple Axes";
            tChart1.Series.Add(line1); tChart1.Series.Add(line2);
            for (int t = 0; t <= 10; ++t)
            {
                line1.Add(Convert.ToDouble(t), Convert.ToDouble(10 + t), Color.Red);
                if (t > 1)
                {
                    line2.Add(Convert.ToDouble(t), Convert.ToDouble(t), Color.Green);
                }
            }

            Steema.TeeChart.Axis leftAxis = tChart1.Axes.Right;
            leftAxis.StartPosition = 0;
            leftAxis.EndPosition = 50;
            leftAxis.AxisPen.Color = Color.Red;
            leftAxis.Title.Font.Color = Color.Red;
            leftAxis.Title.Font.Bold = true;
            leftAxis.Title.Text = "1st Left Axis";

            Steema.TeeChart.Axis axis1 = new Steema.TeeChart.Axis(false, false, tChart1.Chart);

            tChart1.Axes.Custom.Add(axis1);

            line2.CustomVertAxis = axis1;
            axis1.StartPosition = 50;
            axis1.EndPosition = 100;
            axis1.AxisPen.Color = Color.Green;
            axis1.Title.Font.Color = Color.Green;
            axis1.Title.Font.Bold = true;
            axis1.Title.Text = "Extra Axis";
            axis1.PositionUnits = PositionUnits.Percent;
            axis1.RelativePosition = 20;


            //Steema.TeeChart.Axis axis12 = new Steema.TeeChart.Axis(false, false, tChart1.Chart);

            //tChart1.Axes.Custom.Add(axis12);

            //line2.CustomVertAxis = axis12;
            //axis12.StartPosition = 50;
            //axis12.EndPosition = 100;
            //axis12.AxisPen.Color = Color.Green;
            //axis12.Title.Font.Color = Color.Green;
            //axis12.Title.Font.Bold = true;
            //axis12.Title.Text = "Extra Axis";
            //axis12.PositionUnits = PositionUnits.Percent;
            //axis12.RelativePosition = 20;

            DataTable dt = GetData();
            line1.YValues.DataMember = "vibration";
            line1.XValues.DataMember = "width";
            line1.DataSource = dt;


            line2.YValues.DataMember = "vibration";
            line2.XValues.DataMember = "width";
            line2.DataSource = dt;
        }

        public void dd2()
        {
            Steema.TeeChart.Styles.Line line1 = new Steema.TeeChart.Styles.Line(tChart1.Chart[0].Chart);

            //Steema.TeeChart.Styles.Line line2 = new Steema.TeeChart.Styles.Line(tChart1.Chart.Chart[1].Chart);

            line1.Stairs = true; //Set line to Stairs
            line1.LinePen.Color = Color.Blue; //LineSeries bounding lines colour
            //line2.Stairs = true; //Set line to Stairs
            //line2.LinePen.Color = Color.Yellow; //LineSeries bounding lines colour



            DataTable dt = GetData();


            List<DataModel> list = new List<DataModel>() {
                new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="1",
                           Vibration2="-1",
                           Vibration3="1",
                       }
                   }
                },
                new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                              Vibration1="1",
                           Vibration2="-1",
                           Vibration3="1",
                       }
                   }
                },
                new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="1",
                           Vibration2="-1",
                           Vibration3="1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                                       Vibration1="1",
                           Vibration2="-1",
                           Vibration3="1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.2",
                           Vibration2="1.2",
                           Vibration3="-1.5",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                               Vibration1="1",
                           Vibration2="-1",
                           Vibration3="1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="2.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="2.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="2.1",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                },
                 new DataModel(){
                   new_data=new List<Vibration_Current>(){
                       new Vibration_Current(){
                           Vibration1="-1.1",
                           Vibration2="1.2",
                           Vibration3="-1.1",
                       }
                   }
                }
            };

            if (list.Count > 0)
            {
                double num = 0.1;
                foreach (DataModel item in list)
                {
                    for (int i = 0; i < item.new_data.Count; i++)
                    {
                        Vibration v = new Vibration();
                        DataRow dr = dt.NewRow();

                        double d1 = (dt.Rows.Count * num);

                        dr["width"] = d1 + num;
                        dr["vibration"] = item.new_data[i].Vibration1;
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        d1 = (dt.Rows.Count * num);
                        dr["width"] = d1 + num;
                        dr["vibration"] = item.new_data[i].Vibration2;
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        d1 = (dt.Rows.Count * num);
                        dr["width"] = d1 + num;
                        dr["vibration"] = item.new_data[i].Vibration3;
                        dt.Rows.Add(dr);
                    }
                }
            }

            line1.YValues.DataMember = "vibration";
            line1.XValues.DataMember = "width";
            line1.DataSource = dt;


            //line2.YValues.DataMember = "vibration";
            //line2.XValues.DataMember = "width";
            //line2.DataSource = dt;
        }

        #region  多图标代码
        private TChart tChart = new TChart();
        private int space = 3;
        /// <summary>
        /// 添加若干个自定义坐标轴
        /// </summary>
        /// <param name="count"></param>
        private void AddCustomAxis(int count)
        {
            List<BaseLine> listBaseLine = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                listBaseLine.Add((BaseLine)tChart.Series[i]);
            }

            double single = (100 - space * (count + 2)) / (count + 1);//单个坐标轴的百分比
            tChart.Axes.Left.StartPosition = space;
            tChart.Axes.Left.EndPosition = tChart.Axes.Left.EndPosition = tChart.Axes.Left.StartPosition + single;
            tChart.Axes.Left.StartEndPositionUnits = PositionUnits.Percent;
            //tChart.Axes.Bottom.Minimum = 0;
            //tChart.Axes.Bottom.Maximum = 10;

            //tChart.Axes.Bottom.StartPosition = 0;
            //tChart.Axes.Bottom.EndPosition = 10;




            listBaseLine[0].CustomVertAxis = tChart.Axes.Left;

            double startPosition = tChart.Axes.Left.StartPosition;
            double endPosition = tChart.Axes.Left.EndPosition;
            Steema.TeeChart.Axis axis;
            for (int i = 0; i < count; i++)
            {
                axis = new Steema.TeeChart.Axis();
                startPosition = endPosition + space;
                endPosition = startPosition + single;
                axis.StartPosition = startPosition;
                axis.EndPosition = endPosition;
                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }
        DataTable dt;
        private void Init()
        {
            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            //tChart.Panel.Gradient.Visible = true;
            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
            //tChart.Axes.Bottom.Labels.ExactDateTime = true;
            tChart.Axes.Bottom.Labels.Angle = 100;
            //tChart.Axes.Bottom.StartPosition = 0.00;
            //tChart.Axes.Bottom.EndPosition = 10.00;
            //tChart.Axes.Bottom.Maximum = 10.00;
            //tChart.Axes.Bottom.Minimum = 0.00;

            panel1.Controls.Add(tChart);

             dt = GetData();

           // DateTime time = DateTime.Now;

            for (int i = 0; i < 3; i++)
            {
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = string.Format("曲线{0}", i + 1);
               // line.XValues.DateTime = true;
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    line.Add(time.AddSeconds(i).ToOADate(), (double)dt.Rows[i][1]);
                //}

                line.YValues.DataMember = "vibration";
                line.XValues.DataMember = "width";
                line.DataSource = dt;
            }

            AddCustomAxis(3);
        }

        private List<BaseLine> GetVisibleSeries()
        {
            List<BaseLine> visibleSeries = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                tChart.Series[i].CustomVertAxis = null;
                if (tChart.Series[i].Visible)
                {
                    visibleSeries.Add((BaseLine)tChart.Series[i]);
                }
            }
            return visibleSeries;
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt1 =GetData();
            foreach(DataRow dr in dt1.Rows)
            {
                DataRow dr1 = dt.NewRow();
                dr1["id"] = dt.Rows.Count + 1;
                dr1["vibration"] = dr["vibration"];
                dr1["width"] = (dt.Rows.Count * num) + num;
                dt.Rows.Add(dr1);
            }
            tChart.Series.Clear();
            for (int i = 0;i< 3;i++)
            {
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = string.Format("曲线{0}", i + 1);
                line.YValues.DataMember = "vibration";
                line.XValues.DataMember = "width";
                line.DataSource = dt;
            }

            AddCustomAxis(3);
            tChart.Refresh();
        }
        #endregion
    }
}
