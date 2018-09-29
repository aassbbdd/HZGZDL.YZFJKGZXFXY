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
        /// <param name="xNew">返回包络 x轴值</param>
        /// <param name="yNew">返回包络 y轴值</param>

        public void Envelope(double spacing, double[] x, double[] y, out double[] Out_x, out double[] Out_y)
        {
            double[] xNew = (double[])x.Clone();
            double[] yNew = (double[])y.Clone();

            env_3(yNew, xNew, yNew.Length, 0);

            Out_x = x;
            Out_y = xNew;


            //double[] bfy = bf(y);

            //double[] xNew = new double[bfy.Length];
            //double[] yNew = new double[bfy.Length];
            //int fcount = 0;

            //xNew = Diff(bfy, x, out fcount);
            //xNew = Sign(bfy, out fcount);
            //xNew = Diff(bfy, x, out fcount);

            //Out_x = new double[fcount];
            //Out_y = new double[fcount];
            //int j = 1;//计算峰值个数

            //Out_x[0] = 0;
            //Out_y[0] = 0;

            //for (int i = 0; i < xNew.Length; i++)
            //{
            //    if (xNew[i] < 0)//峰值
            //    {
            //        Out_x[j] = (i + 1) * spacing;
            //        Out_y[j] = bfy[i + 1];
            //        j++;
            //    }
            //    else//谷值
            //    {

            //    }
            //}

            //Out_x[Out_x.Length - 1] = x.Length * spacing;
            //Out_y[Out_x.Length - 1] = 0;
        }
        #endregion

        private double[] bf(double[] f)
        {
            double[] fNew = new double[f.Length];
            for (int i = 0; i < f.Length - 1; i++)
            {
                if (f[i] > 0)
                {
                    fNew[i] = f[i];
                }
                else
                {
                    fNew[i] = -1;
                }
            }
            return fNew;
        }

        /// <summary>
        /// （求斜率函数）：对数组y0相邻元素求斜率，比原始信号少一位数据。结果是个数组。
        /// </summary>
        /// <param name="yArray">Y轴数组</param>
        /// <param name="yArray">峰数量</param>
        /// <returns></returns>
        public double[] Diff(double[] yArray, double[] xArray, out int fcount)
        {
            fcount = 2;
            double[] newY = new double[yArray.Length - 2];
            for (int i = 0; i < newY.Length - 10; i++)
            {
                var n1 = yArray[i + 10];
                var n2 = yArray[i];
                var num = (yArray[i + 10] - yArray[i]);
                newY[i] = (yArray[i + 10] - yArray[i]);
                if (newY[i] < 0)
                {
                    fcount++;
                }
            }
            return newY;
        }

        /// <summary>
        /// （求符号函数）：判断数组yy1各元素符号，大于0结果为1，等于0结果为0，小于0结果为-1。结果是个数组。
        /// </summary>
        /// <param name="yArray">Y轴数组</param>
        /// <returns></returns>
        public double[] Sign(double[] yArray, out int fcount)
        {
            double[] newX = new double[yArray.Length];
            fcount = 1;
            for (int i = 0; i < yArray.Length; i++)
            {
                double newY = yArray[i];

                if (newY > 0)
                {
                    newX[i] = 1;
                    fcount++;
                }
                else if (newY == 0)
                {
                    newX[i] = 0;
                    fcount++;
                }
                else if (newY <= 0)
                {
                    newX[i] = -1;
                }
            }
            return newX;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="rct"></param>
        /// <returns></returns>
        public double env_1(double x, double rct)
        {
            double old_y = 0.0;
            if (rct == 0.0)
            {
                old_y = 0.0;
            }
            else
            {
                if (x > old_y)
                {
                    old_y = x;
                }
                else
                {
                    old_y *= rct / (rct + 1);
                }
            }
            return old_y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="N"></param>
        /// <param name="rct"></param>
        public void env_2(double[] x, double[] y, int N, double rct = 2)
        {
            double xx = 0.0;
            int i;
            y[0] = Math.Abs(x[0]);
            for (i = 1; i < N; i++)
            {
                xx = x[i];

                if (xx > 0)
                {
                    if ((xx + 20.00) > y[i - 1])
                    {
                        y[i] = xx;
                    }
                    else
                    {
                        y[i] = y[i - 1] * rct / (rct + 1);
                    }
                }
                else
                {
                    y[i] = 0;
                }

            }
        }

        public void env_3(double[] iny, double[] outy, int N, double rct = 0.01)
        {
            double m_old = 0;
            for (int i = 0; i < N; i++)
            {
                if ((iny[i]) > m_old)
                {
                    m_old = iny[i];
                    outy[i] = m_old;
                }
                else
                {
                    m_old *= rct / (rct + 1);
                    outy[i] = m_old;
                }
            }

        }

    }
}
