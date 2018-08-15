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
    }
}
