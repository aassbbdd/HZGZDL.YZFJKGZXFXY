using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

      
    }
}
