
using System;

namespace Udp_Agreement
{
    /// <summary>
    /// 包络线 算法
    /// </summary>
    public class Hilbert
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static Hilbert instance = null;
        public static Hilbert Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Hilbert();
                }
                return instance;
            }
        }

        //================================================
        //fr为双精度一维数组，长度为n，输入信号的实部；
        //fi为双精度一维数组，长度为n，输入信号的虚部；
        //返回双精度二维数组，为原信号的解析信号；
        //其中hilbert[0]为原信号，hilbert[1]为信号的hilbert变换；
        //================================================
        public double[][] hilbert(double[] fr, double[] fi, int n)
        {
            double[][] fft = new double[2][];
            fft[0] = new double[n];
            fft[1] = new double[n];
            double[][] z1 = new double[2][];
            z1[0] = new double[n];
            z1[1] = new double[n];
            double[][] hil = new double[2][];
            hil[0] = new double[n];
            hil[1] = new double[n];
            fft = fft_dsa(fr, fi, n, 1);
            z1[0][0] = 0;
            z1[1][0] = 0;
            for (int i = 1; i < n / 2; i++)
            {
                z1[0][i] = 2 * fft[0][i];
                z1[1][i] = 2 * fft[1][i];
            }
            for (int i = n / 2; i < n; i++)
            {
                z1[0][i] = 0.0;
                z1[1][i] = 0.0;
            }
            hil = fft_dsa(z1[0], z1[1], n, -1);
            return hil;
        }

        //*----------------------------------------------------*
        //*   Fast Fourier Trasform Using Time Decomposition   *
        //*   With Input Bit Reversal.                         *
        //*   Data is in FR (real) and FI (imaginary) arrays   *
        //*   Computation is in place,output replaces input    *
        //*   Number of points must be N = 2**K.               *
        //*   If T=1,compute DFT.T=-1,IDFT                     *
        //*   FR(N) and FI(N) must be dimensioned in main.     *
        //*----------------------------------------------------*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="fi"></param>
        /// <param name="n"></param>
        /// <param name="t"></param>
        /// /*利用时间分解的快速傅立叶变换

        //        输入位反转。
        //数据在FR（实数）和FI（虚数）数组中。
        //计算到位，输出替代输入
        //点的数量必须是n＝2×*k。
        //如果t＝1，则计算DFT.T＝-1，IDFT
        //FR（n）和FI（n）必须在主尺度上确定。
        //            */
        /// <returns></returns>

        public double[][] fft_dsa(double[] fr, double[] fi, int n, int t)
        {
            double tr, ti, wr, wi;
            double[][] result;
            result = new double[2][];
            result[0] = new double[n];
            result[1] = new double[n];
            int nn, l, istep, el;
            int i, j, m, mr;
            if (t < 0)
                for (i = 0; i < n; i++)
                {
                    fr[i] = fr[i] / n;
                    fi[i] = fi[i] / n;
                }
            mr = 0;
            nn = n - 1;
            for (m = 1; m <= nn; m++)
            {
                l = n;
                do { l /= 2; } while ((mr + l) > nn);
                mr = mr % l + l;
                if (mr > m)
                {
                    tr = fr[m];
                    fr[m] = fr[mr];
                    fr[mr] = tr;
                    ti = fi[m];
                    fi[m] = fi[mr];
                    fi[mr] = ti;
                }
            }
            l = 1;
            while (l < n)
            {
                istep = 2 * l;
                el = l;
                for (m = 1; m <= l; m++)
                {
                    wr = (double)Math.Cos((double)(Math.PI * (1 - m) / el));
                    wi = (double)Math.Cos((double)(Math.PI / 2.0 - Math.PI * (1 - m) / el)) * t;
                    for (i = m; i <= n; i += istep)
                    {
                        j = i + l;
                        tr = wr * fr[j - 1] - wi * fi[j - 1];
                        ti = wr * fi[j - 1] + wi * fr[j - 1];
                        fr[j - 1] = fr[i - 1] - tr;
                        fi[j - 1] = fi[i - 1] - ti;
                        fr[i - 1] = fr[i - 1] + tr;
                        fi[i - 1] = fi[i - 1] + ti;
                    }
                }
                l = istep;
            }
            for (i = 0; i < n; i++)
            {
                result[0][i] = fr[i];
                result[1][i] = fi[i];
            }
            return result;
        }
    }

    




}