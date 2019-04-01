using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udp_Agreement
{
    /// <summary>
    /// 包络线算法
    /// </summary>
    public class Envelope_Algorithm
    {
        /// 获取唯一实例
        /// </summary>
        private static Envelope_Algorithm instance = null;
        public static Envelope_Algorithm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Envelope_Algorithm();
                }
                return instance;
            }
        }

        #region 包络线算法

        /// <summary>
        /// 包络线算法
        /// </summary>
        /// <param name="x">传入x轴值</param>
        /// <param name="y">传入y轴值</param>
        /// <param name="Out_x">返回包络 x轴值</param>
        /// <param name="Out_y">返回包络 y轴值</param>
        /// <param name="spacing">点间距 </param>
        /// <param name="rc_dn">下降 rc值</param>
        /// <param name="rc_up">上升 rc值</param>
        public void Envelope(double spacing, double[] x, double[] y
            , out double[] Out_x, out double[] Out_y
             , double rc_dn = 0.01, double rc_up = 0.0001)
        {

            double[] yNew = (double[])y.Clone();
            double[] xNew = (double[])x.Clone();

            double[] outY = new double[yNew.Length];
            double[] outX = new double[xNew.Length];
            env_3(yNew, outY, rc_dn, rc_up);

            Out_x = x;
            Out_y = outY;
        }

        /// <summary>
        /// 包络线算法
        /// </summary>
        /// <param name="x">传入x轴值</param>
        /// <param name="y">传入y轴值</param>
        /// <param name="Out_x">返回包络 x轴值</param>
        /// <param name="Out_y">返回包络 y轴值</param>
        /// <param name="spot">点间距 </param>
        public void Envelope_01(int spot, double[] x, double[] y
            , out double[] Out_x, out double[] Out_y)
        {
            //int HZ = 5;
            double[] yNew = (double[])y.Clone();
            double[] xNew = (double[])x.Clone();
            int count = yNew.Length / spot;

            double[] outY = new double[count];
            double[] outX = new double[count];
            env_2(yNew, xNew, count, out outY, out outX, spot);

            Out_x = outX;
            Out_y = outY;
        }
        #endregion

        /// <summary>
        /// 返回包络线数据
        /// </summary>
        /// <param name="In_y">完整y轴</param>
        /// <param name="In_x">完整x轴</param>
        /// <param name="count">总长度</param>
        /// <param name="Out_y">返回y轴</param>
        /// <param name="Out_x">返回x轴</param>
        /// <param name="spot">间隔点位</param>
        public void env_2(double[] In_y, double[] In_x, int count, out double[] Out_y, out double[] Out_x, int spot = 5)
        {
            double[] Out_y_01 = new double[count];
            double[] Out_x_01 = new double[count];
            double y = 0.0;
            double x = 0.0;
            int j = 1;

            for (int i = 1; i < In_y.Length; i++)
            {
                if (y < In_y[i])
                {
                    y = In_y[i];
                    x = In_x[i];
                }
                if (i % spot == 0)
                {
                    if (count > j && y != 0)
                    {
                        Out_y_01[j] = y;
                        Out_x_01[j] = x;
                        y = 0;
                        j++;
                    }
                }
            }
            Out_y = new double[j];
            Out_x = new double[j];

            for (int i = 0; i < j; i++)
            {
                Out_y[i] = Out_y_01[i];
                Out_x[i] = Out_x_01[i];
            }

            Out_y[j - 1] = 0;
            Out_x[j - 1] = In_x[In_x.Length - 1];

        }
        /// <summary>
        /// 返回包络线数据
        /// </summary>
        /// <param name="In_y">入参 振动值数组</param>
        /// <param name="Out_y">出参 振动值数组</param>
        /// <param name="rc_dn">下降 rc值</param>
        /// <param name="rc_up">上升 rc值</param>
        public void env_3(double[] In_y, double[] Out_y, double rc_dn = 0.01, double rc_up = 0.0001)
        {
            double xx = 0.0;
            int i;
            double t = 0.000001;

            Out_y[0] = Math.Abs(In_y[0]);
            for (i = 1; i < In_y.Length; i++)
            {
                xx = In_y[i];
                if ((xx) > Out_y[i - 1])
                {
                    Out_y[i] = Math.Round(Out_y[i - 1] + ((xx - Out_y[i - 1]) * (1 - Math.Exp(-t / rc_up))), 2);
                }
                else
                {
                    Out_y[i] = Math.Round(Out_y[i - 1] - ((Out_y[i - 1] - xx) * (1 - Math.Exp(-t / rc_dn))), 2);
                }


            }
        }

        public void Average_100(double spacing, double[] y, out double[] newys, out double[] newxs)
        {
            int num = 1;
            newys = new double[y.Length / num];
            newxs = new double[y.Length / num];

            if (y.Length > 0)
            {
                double newy = new double();
                int j = 0;
                for (int i = 0; i < y.Length; i++)
                {
                    newy += y[i];
                    if (((i + 1) % num) == 0)
                    {
                        int dd = ((i + 1) % num);
                        newys[j] = newy / num;

                        newxs[j] = j * spacing * num;

                        j++;
                        newy = 0;
                    }
                    //else if (i == y.Length / num)
                    //{
                    //    newys[j] = newy / num;
                    //}

                }

                newys[newys.Length - 1] = 0;

                newxs[newxs.Length - 1] = 5;
            }
        }

    }
}
