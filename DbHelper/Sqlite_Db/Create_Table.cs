using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelper.Sqlite_Db
{
    /// <summary>
    /// 生成数据库表
    /// </summary>
    public class Create_Table
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static Create_Table instance = null;
        public static Create_Table Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Create_Table();
                }
                return instance;
            }
        }

        /// <summary>
        /// 测试生成表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Create_AA(string tableName)
        {
            string tablestring = @"CREATE TABLE " + tableName + "(" +
                "ID integer not null  primary key autoincrement " +
                ",NAME  varchar(20) not null" +
                ",TEAM varchar(10)" +
                ", NUMBER varchar(10))";

            return tablestring;
        }


        /// <summary>
        /// 生成测试配置表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Create_TEST_COFIGE()
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("drop table if exists TEST_COFIGE; ");

            sbsql.Append(@"   create table TEST_COFIGE
                                (
                                   ID                  integer       not null          primary key autoincrement,

                                   PARENTID               varchar(20)                   null,
                                   DVNAME               varchar(200)                   null,
                                   DVPOSITION           varchar(200)                   null,
                                   DVID                 varchar(20)                    null,
                                   TESTER               varchar(50)                    null,
                                   OLTC_TS              varchar(20)                    null,
                                   CONTACT_NUM          integer                        null,
                                   TEST_NUM             integer                        null,
                                   SPLACE               varchar(50)                    null,
                                   OILTEMP              varchar(50)                    null,
                                   TEST_TIME            varchar(50)                       null,
                                   TEST_TYPE            varchar(20)                    null,
                                   GETINFO              varchar(20)                    null,
                                   TESTSTAGE            varchar(20)                    null,
                                   DJUST                varchar(20)                    null,
                                   DESCRIBE             varchar(2000)                  null,
                                   SCURRENT             varchar(20)                    null,
                                   ECURRENT              varchar(20)                    null,
                                   TIME_UNIT            varchar(20)                    null,
                                   V1                   varchar(20)                    null,
                                   V2                   varchar(20)                    null,
                                   V3                   varchar(20)                    null,
                                   C1                   varchar(20)                    null,
                                   C2                   varchar(20)                    null,
                                   C3                   varchar(20)                    null
                                )");

            return sbsql.ToString();
        }


        /// <summary>
        /// 生成测试配置表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string Create_TEST_DATA()
        {
            StringBuilder sbsql = new StringBuilder();
            sbsql.Append("drop table if exists TEST_DATA; ");

            sbsql.Append(@"   create table TEST_DATA
                                (
                                   ID                  integer       not null          primary key autoincrement,
                                   DATAID               varchar(50)                    null,
                                   DATAIDX16            varchar(50)                    null,
                                   DATACONTENT          varchar(2000)                  null
                                )");

            return sbsql.ToString();
        }
    }
}
