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
            double[] xNew = new double[y.Length - 2];
            double[] yNew = new double[y.Length - 2];
            int fcount = 0;

            xNew = Diff(y, x, out fcount);
            xNew = Sign(y, out fcount);
            xNew = Diff(y, x, out fcount);

            Out_x = new double[fcount];
            Out_y = new double[fcount];
            int j = 1;//计算峰值个数

            Out_x[0] = 0;
            Out_y[0] = 0;

            for (int i = 0; i < xNew.Length; i++)
            {
                if (xNew[i] < 0)//峰值
                {
                    Out_x[j] = (i + 1) * spacing;
                    Out_y[j] = y[i + 1];
                    j++;
                }
                else//谷值
                {

                }
            }

            Out_x[Out_x.Length - 1] = x.Length * spacing;
            Out_y[Out_x.Length - 1] = 0;
        }
        #endregion

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
            for (int i = 0; i < newY.Length; i++)
            {
                var n1 = yArray[i + 1];
                var n2 = yArray[i];
                var num = (yArray[i + 1] - yArray[i]);
                newY[i] = (yArray[i + 1] - yArray[i]);
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
    }
}
