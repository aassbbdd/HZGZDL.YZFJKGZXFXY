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


        //public void Envelope(double spacing, double[] x, double[] y
        //    , out double[] Out_x, out double[] Out_y
        //    , double rc_dn = 0.01, double rc_up = 0.0001)
        //{
        //    double[] yNew = (double[])y.Clone();
        //    double[] xNew = (double[])x.Clone();

        //    //Average_100(spacing, y, out yNew, out xNew);

        //    double[] outY = new double[yNew.Length];
        //    double[] outX = new double[xNew.Length];
        //    // env_3(yNew, xNew, yNew.Length, 0);
        //    env_3(yNew, outY, rc_dn, rc_up);

        //    Out_x = x;
        //    Out_y = outY;


        //    //double[] bfy = bf(y);

        //    //double[] xNew = new double[bfy.Length];
        //    //double[] yNew = new double[bfy.Length];
        //    //int fcount = 0;

        //    //xNew = Diff(bfy, x, out fcount);
        //    //xNew = Sign(bfy, out fcount);
        //    //xNew = Diff(bfy, x, out fcount);

        //    //Out_x = new double[fcount];
        //    //Out_y = new double[fcount];
        //    //int j = 1;//计算峰值个数

        //    //Out_x[0] = 0;
        //    //Out_y[0] = 0;

        //    //for (int i = 0; i < xNew.Length; i++)
        //    //{
        //    //    if (xNew[i] < 0)//峰值
        //    //    {
        //    //        Out_x[j] = (i + 1) * spacing;
        //    //        Out_y[j] = bfy[i + 1];
        //    //        j++;
        //    //    }
        //    //    else//谷值
        //    //    {

        //    //    }
        //    //}

        //    //Out_x[Out_x.Length - 1] = x.Length * spacing;
        //    //Out_y[Out_x.Length - 1] = 0;
        //}
        #endregion

        //private double[] bf(double[] f)
        //{
        //    double[] fNew = new double[f.Length];
        //    for (int i = 0; i < f.Length - 1; i++)
        //    {
        //        if (f[i] > 0)
        //        {
        //            fNew[i] = f[i];
        //        }
        //        else
        //        {
        //            fNew[i] = -1;
        //        }
        //    }
        //    return fNew;
        //}

        ///// <summary>
        ///// （求斜率函数）：对数组y0相邻元素求斜率，比原始信号少一位数据。结果是个数组。
        ///// </summary>
        ///// <param name="yArray">Y轴数组</param>
        ///// <param name="yArray">峰数量</param>
        ///// <returns></returns>
        //public double[] Diff(double[] yArray, double[] xArray, out int fcount)
        //{
        //    fcount = 2;
        //    double[] newY = new double[yArray.Length - 2];
        //    for (int i = 0; i < newY.Length - 10; i++)
        //    {
        //        var n1 = yArray[i + 10];
        //        var n2 = yArray[i];
        //        var num = (yArray[i + 10] - yArray[i]);
        //        newY[i] = (yArray[i + 10] - yArray[i]);
        //        if (newY[i] < 0)
        //        {
        //            fcount++;
        //        }
        //    }
        //    return newY;
        //}

        ///// <summary>
        ///// （求符号函数）：判断数组yy1各元素符号，大于0结果为1，等于0结果为0，小于0结果为-1。结果是个数组。
        ///// </summary>
        ///// <param name="yArray">Y轴数组</param>
        ///// <returns></returns>
        //public double[] Sign(double[] yArray, out int fcount)
        //{
        //    double[] newX = new double[yArray.Length];
        //    fcount = 1;
        //    for (int i = 0; i < yArray.Length; i++)
        //    {
        //        double newY = yArray[i];

        //        if (newY > 0)
        //        {
        //            newX[i] = 1;
        //            fcount++;
        //        }
        //        else if (newY == 0)
        //        {
        //            newX[i] = 0;
        //            fcount++;
        //        }
        //        else if (newY <= 0)
        //        {
        //            newX[i] = -1;
        //        }
        //    }
        //    return newX;
        //}

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
        public void env_2(double[] x, double[] y, double[] outX, int N, double rct = 100)
        {
            double xx = 0.0;
            int i;

            y[0] = Math.Abs(x[0]);
            for (i = 1; i < x.Length; i++)
            {

                xx = x[i];
                if ((xx) > y[i - 1])
                {
                    y[i] = xx;
                }
                else
                {
                    y[i] = y[i - 1] * rct / (rct + 1);
                }
            }
        }

        /// <summary>
        /// 返回包络线数据
        /// </summary>
        /// <param name="In_y">入参 震动值数组</param>
        /// <param name="Out_y">出参 震动值数组</param>
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
                    Out_y[i] = Out_y[i - 1] + ((xx - Out_y[i - 1]) * (1 - Math.Exp(-t / rc_up)));
                }
                else
                {
                    Out_y[i] = Out_y[i - 1] - ((Out_y[i - 1] - xx) * (1 - Math.Exp(-t / rc_dn)));
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
