using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udp_Agreement.Model;

namespace DbHelper.Sqlite_Db
{
    /// <summary>
    /// 数据库增删改
    /// </summary>
    public class Db_Action
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static Db_Action instance = null;
        public static Db_Action Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Db_Action();
                }
                return instance;
            }
        }


        #region 数据插入
        /// <summary>
        /// 插入 insert into 
        /// </summary>
        public void Test_Data_insert(DataModel model)
        {
            try
            {
                string strsql = @"

                                insert into TEST_DATA(
                                DATAID
                                ,DATAIDX16   
                                ,DATACONTENT                   

                                )values(
                                 @DATAID
                                ,@DATAIDX16   
                                ,@DATACONTENT    
                                )
                                ";

                SQLiteParameter[] parameters = {
                    new SQLiteParameter("DATAID",DbType.String)
                    ,new SQLiteParameter("DATAIDX16",DbType.String)
                    ,new SQLiteParameter("DATACONTENT",DbType.String,2000)


                };

                parameters[0].Value = model.id;
                parameters[1].Value = model.head;
                parameters[2].Value = model.text;

                int count = SQLiteHelper.ExecuteNonQuery(strsql, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}
