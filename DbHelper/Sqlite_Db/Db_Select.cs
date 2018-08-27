using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelper.Sqlite_Db
{
    /// <summary>
    /// 数据库查询
    /// </summary>
    public class Db_Select
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static Db_Select instance = null;
        public static Db_Select Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Db_Select();
                }
                return instance;
            }
        }

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <returns></returns>
        public DataSet Get_All_Table()
        {
            //查询.db库中所有的表用以下的sql语句

            try
            {
                string tablenames = "select name as'NAME' from sqlite_master where type='table' order by name;";
                DataSet ds = SQLiteHelper.ExecuteDataSet(tablenames);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 获取测试数据
        /// </summary>
        /// <returns></returns>
        public DataTable Test_Data_Get()
        {
            try
            {

                StringBuilder sbsql = new StringBuilder();
                sbsql.Append("select * from TEST_DATA  order by  DATAID ;");
                DataSet ds = SQLiteHelper.ExecuteDataSet(sbsql.ToString());

                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region 测试计划

        /// <summary>
        /// 获取所有测试计划
        /// </summary>
        /// <returns></returns>
        public DataTable All_Test_Cofig_Get()
        {
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from TEST_COFIGE");

                dt = SQLiteHelper.ExecuteDataTable(sb.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
