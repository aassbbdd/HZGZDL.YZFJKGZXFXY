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
    }
}
