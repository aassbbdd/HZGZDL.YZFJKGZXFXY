using DbHelper.Db_Model;
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
                sbsql.Append("select * from TEST_DATA  order by  DATAID  ;");
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
        public List<Test_Plan> All_Test_Cofig_Get()
        {
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select tc.* ,( select DVNAME from TEST_CONFIGE where tc.PARENTID=id) as PARENTNAME  from TEST_CONFIGE tc  order by ID desc");
                dt = SQLiteHelper.ExecuteDataTable(sb.ToString());
                if (dt.Rows.Count > 0)
                {
                    return Test_Plan_Bind(dt);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取 单条数据
        /// </summary>
        /// <returns></returns>
        public Test_Plan Single_Test_Cofig_Get(string dvname)
        {
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.Append(" select tc.* ,( select DVNAME from TEST_CONFIGE where tc.PARENTID=id) as PARENTNAME  from TEST_CONFIGE tc where " +
                    "tc.DVNAME='" + dvname + "' order by ID desc");
                dt = SQLiteHelper.ExecuteDataTable(sb.ToString());
                if (dt.Rows.Count > 0)
                {
                    return Test_Plan_Bind(dt)[0];
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private List<Test_Plan> Test_Plan_Bind(DataTable dt)
        {
            try
            {
                List<Test_Plan> list = new List<Test_Plan>();

                foreach (DataRow dr in dt.Rows)
                {
                    Test_Plan item = new Test_Plan();
                    item.ID = dr["ID"].ToString();

                    item.DVNAME = dr["DVNAME"].ToString();
                    item.PARENTNAME = dr["PARENTNAME"].ToString();
                    item.DVPOSITION = dr["DVPOSITION"].ToString();
                    item.DVID = dr["DVID"].ToString();
                    item.TESTER = dr["TESTER"].ToString();
                    item.OLTC_TS = dr["OLTC_TS"].ToString();

                    item.CONTACT_NUM = dr["CONTACT_NUM"].ToString();
                    item.TEST_NUM = dr["TEST_NUM"].ToString();
                    item.SPLACE = dr["SPLACE"].ToString();
                    item.OILTEMP = dr["OILTEMP"].ToString();
                    item.TEST_TIME = dr["TEST_TIME"].ToString();

                    item.TEST_TYPE = dr["TEST_TYPE"].ToString();
                    item.GETINFO = dr["GETINFO"].ToString();
                    item.TESTSTAGE = dr["TESTSTAGE"].ToString();
                    item.DJUST = dr["DJUST"].ToString();
                    item.DESCRIBE = dr["DESCRIBE"].ToString();

                    item.SCURRENT = dr["SCURRENT"].ToString();
                    item.ECURRENT = dr["ECURRENT"].ToString();
                    item.TIME_UNIT = dr["TIME_UNIT"].ToString();
                    item.V1 = dr["V1"].ToString();
                    item.V2 = dr["V2"].ToString();

                    item.V3 = dr["V3"].ToString();
                    item.C1 = dr["C1"].ToString();
                    item.C2 = dr["C2"].ToString();
                    item.C3 = dr["C3"].ToString();

                    item.PARENTID = dr["PARENTID"].ToString();
                    item.TEST_BASE_C = dr["TEST_BASE_C"].ToString();
                    item.TEST_SINGLE_DOUBLE = dr["TEST_SINGLE_DOUBLE"].ToString();
                    item.DOUBLE_SP = dr["DOUBLE_SP"].ToString();
                    item.DOUBLE_EP = dr["DOUBLE_EP"].ToString();

                    item.SINGLE_P = dr["SINGLE_P"].ToString();
                    item.TEST_ORDER = dr["TEST_ORDER"].ToString();
                    item.COUNT_BASE_C = dr["COUNT_BASE_C"].ToString();
                    item.VOLTAGE = dr["VOLTAGE"].ToString();


                    list.Add(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
