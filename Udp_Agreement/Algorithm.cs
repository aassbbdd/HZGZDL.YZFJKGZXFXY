using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udp_Agreement
{
    /// <summary>
    /// 在线有载测试仪通讯协议 算法
    /// </summary>
    public class Algorithm
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static Algorithm instance = null;
        public static Algorithm Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Algorithm();
                }
                return instance;
            }
        }
        #region Vibration 振动
        /// <summary>
        /// 振动计算公式 算法
        /// </summary>
        /// <returns></returns>
        public string Vibration_Algorithm(double AD)
        {
            /// <summary>
            /// 振动传感器量程：G 默认50
            /// </summary>
            int G = 50;
            /// <summary>
            /// 振动校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认10V（ -10V到+10V）
            /// </summary>
            int V = 10;


            //振动采集到的AD值 0-32767（正）,32768-65535（负）
            //参考电压：2.5V固定  衰减倍数：5固定
            //计算加速度：x
            double x = (AD * 2.5 * 5 * G * k) / (32768 * V);
            return x.ToString();
        }
        /// <summary>
        /// 振动计算公式 算法
        /// </summary>
        /// <returns></returns>
        public string Vibration_Algorithm(string AD)
        {
            /// <summary>
            /// 振动传感器量程：G 默认50
            /// </summary>
            int G = 50;
            /// <summary>
            /// 振动校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认10V（ -10V到+10V）
            /// </summary>
            int V = 10;
            //int ad = Convert.ToInt32(AD, 16);

            //if (ad >= 32768)
            //{
            //    ad = -(65535 - ad);
            //}
            double ad = Convert.ToInt16(AD, 16);

            //振动采集到的AD值 0-32767（正）,32768-65535（负）
            //参考电压：2.5V固定  衰减倍数：5固定
            //计算加速度：x
            double x = (ad * 2.5 * 5 * G * k) / (32768 * V);
            return x.ToString();
        }

        /// <summary>
        /// 振动计算公式 算法 返回 Double 
        /// </summary>
        /// <returns></returns>
        public double Vibration_Algorithm_Double(string AD)
        {
            /// <summary>
            /// 振动传感器量程：G 默认50
            /// </summary>
            int G = 50;
            /// <summary>
            /// 振动校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认10V（ -10V到+10V）
            /// </summary>
            int V = 10;
            int ad = Convert.ToInt32(AD, 16);

            if (ad >= 32768)
            {
                ad = -(65535 - ad);
            }

            //double ad = Convert.ToInt16(AD, 16);


            //振动采集到的AD值 0-32767（正）,32768-65535（负）
            //参考电压：2.5V固定  衰减倍数：5固定
            //计算加速度：x
            double x = (ad * 2.5 * 5 * G * k) / (32768 * V);
            return x;
        }
        #endregion
        #region Current 电流
        /// <summary>
        /// 电流计算公式 算法
        /// </summary>
        /// <returns></returns>
        public string Current_Algorithm(int AD)
        {
            /// <summary>
            /// 电流传感器量程：I默认10A（或者100A）
            /// </summary>
            int I = 10;
            /// <summary>
            /// 电流校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认1V（ -1V到+1V）
            /// </summary>
            int V = 1;
            //电流采集到的AD值 0 - 32767（正）,32768 - 65535（负）
            //参考电压：2.5V固定 衰减倍数：1固定
            //计算电流：x

            double x = (AD * 2.5 * 1 * I * k) / (32768 * V);
            return x.ToString();
        }

        /// <summary>
        /// 电流计算公式 算法
        /// </summary>
        /// <returns></returns>
        public string Current_Algorithm(string AD)
        {
            /// <summary>
            /// 电流传感器量程：I默认10A（或者100A）
            /// </summary>
            int I = 10;
            /// <summary>
            /// 电流校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认1V（ -1V到+1V）
            /// </summary>
            int V = 1;
            //电流采集到的AD值 0 - 32767（正）,32768 - 65535（负）
            //参考电压：2.5V固定 衰减倍数：1固定
            //计算电流：x
            // double ad = Convert.ToInt32(AD, 16);
            double ad = Convert.ToInt16(AD, 16);
            //if (ad >= 32768)
            //{
            //    ad = -(65535 - ad);
            //}

            double x = (ad * 2.5 * 1 * I * k) / (32768 * V);
            return x.ToString();
        }

        /// <summary>
        /// 电流计算公式 算法 返回 Double 
        /// </summary>
        /// <returns></returns>
        public double Current_Algorithm_Double(string AD)
        {
            /// <summary>
            /// 电流传感器量程：I默认10A（或者100A）
            /// </summary>
            int I = 10;
            /// <summary>
            /// 电流校准系数：k 默认1.000
            /// </summary>
            double k = 1.000;
            /// <summary>
            /// 振动传感器输出V 默认1V（ -1V到+1V）
            /// </summary>
            int V = 1;
            //电流采集到的AD值 0 - 32767（正）,32768 - 65535（负）
            //参考电压：2.5V固定 衰减倍数：1固定
            //计算电流：x
            double ad = Convert.ToInt16(AD, 16);

            //if (ad >= 32768)
            //{
            //    ad = -(65535 - ad);
            //}
            double x = (ad * 2.5 * 1 * I * k) / (32768 * V);
            return x;
        }
        #endregion

        #region 偷点算法

        /// <summary>
        /// 偷点 算法
        /// </summary>
        /// <returns></returns>
        public string Less_Porint_Algorithm(double num, double width, double lessnum, int allnum)
        {
            string l = ((((num * width) + width) * lessnum) / allnum).ToString();
            return l;
        }

        /// <summary>
        /// 偷点 算法
        /// </summary>
        /// <returns></returns>
        public double Less_Porint_Algorithm_d(double num, double width, double lessnum, int allnum)
        {
            double l = ((((num * width) + width) * lessnum) / allnum);
            return l;
        }
        #endregion
    }
}
