using DocDecrypt.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Data.Common;

namespace DbHelper
{
    /// <summary>
    /// SQLite 数据库帮助类
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// 所有成员函数都是静态的，构造函数定义为私有
        /// </summary>
        private SQLiteHelper()
        {
        }
        /// <summary>
        /// 链接路径
        /// </summary>
        private static string connectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString.ToString();
        /// <summary>
        /// 数据库名字
        /// </summary>
        private static string dbName = ConfigurationManager.ConnectionStrings["path_sqlite"].ConnectionString.ToString();

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static SQLiteHelper instance = null;
        public static SQLiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLiteHelper();
                }
                return instance;
            }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
        private static SQLiteConnection _Conn = null;
        /// <summary>
        /// 连接对象
        /// </summary>
        public static SQLiteConnection Conn
        {
            get
            {
                if (_Conn == null) _Conn = new SQLiteConnection(ConnectionString);
                return SQLiteHelper._Conn;
            }
            set { SQLiteHelper._Conn = value; }
        }

        public static object Configuration { get; private set; }

        /// <summary>
        /// 新建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns>新建成功，返回true，否则返回false</returns>
        static public Boolean NewDbFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\DB\\";
            try
            {
                FileHelper.CreateDirectoy(path);//创建文件夹
                path = path + dbName;//拼接文件路径
                if (!FileHelper.IsFileExist(path))//验证文件是否存在
                {
                    SQLiteConnection.CreateFile(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("新建数据库文件" + path + "失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="tableName">表名称</param>
        static public void NewTable(string tbStr)
        {
            SQLiteConnection sqliteConn = new SQLiteConnection(Conn);
            if (sqliteConn.State != System.Data.ConnectionState.Open)
            {
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = tbStr;
                cmd.ExecuteNonQuery();
            }
            sqliteConn.Close();
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="tableName">表名称</param>
        static public void NewTable1(string tableName)
        {
            SQLiteConnection sqliteConn = new SQLiteConnection(Conn);
            if (sqliteConn.State != System.Data.ConnectionState.Open)
            {
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = "CREATE TABLE " + tableName + "(Name varchar,Team varchar, Number varchar)";
                cmd.ExecuteNonQuery();
            }
            sqliteConn.Close();
        }
        #region CreateCommand(commandText,SQLiteParameter[])
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="commandText">语句</param>
        /// <param name="commandParameters">语句参数.</param>
        /// <returns>SQLite Command</returns>
        public static SQLiteCommand CreateCommand(string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, Conn);
            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            return cmd;
        }

        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <returns> true 存在 false 不存在</returns>
        public bool IsTableExist(string tableName)
        {
            SQLiteCommand cmd = Conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + tableName + "';";
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            int IsTableExist = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();
            Conn.Close();
            if (0 == IsTableExist)// 1 有, 0 无
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion



        #region 增删改查

        #region 执行数据库操作(新增、更新或删除)，返回影响行数
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(SQLiteCommand cmd)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, cmd.CommandType, cmd.CommandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, params SQLiteParameter[] cmdParms)
        {
            int result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText, cmdParms);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(SQLiteCommand cmd)
        {
            object result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, cmd.CommandType, cmd.CommandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型（默认语句）</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text)
        {
            object result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, params SQLiteParameter[] cmdParms)
        {
            object result = 0;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText, cmdParms);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region 执行数据库查询，返回SqlDataReader对象
        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>SqlDataReader对象</returns>
        public static DbDataReader ExecuteReader(SQLiteCommand cmd)
        {
            DbDataReader reader = null;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");

            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, cmd.CommandType, cmd.CommandText);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>SqlDataReader对象</returns>
        public static DbDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.Text)
        {
            DbDataReader reader = null;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型（默认语句）</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>SqlDataReader对象</returns>
        public static DbDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.Text, params SQLiteParameter[] cmdParms)
        {
            DbDataReader reader = null;
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText, cmdParms);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }
        #endregion

        #region 执行数据库查询，返回DataSet对象
        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(SQLiteCommand cmd)
        {
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, cmd.CommandType, cmd.CommandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string commandText, CommandType commandType = CommandType.Text)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string commandText, CommandType commandType = CommandType.Text, params SQLiteParameter[] cmdParms)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText, cmdParms);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }
        #endregion

        #region 执行数据库查询，返回DataTable对象
        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型(默认语句)</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandType = CommandType.Text)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataTable dt = new DataTable();
            SQLiteConnection con = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return dt;
        }

        #endregion

        #region 通用分页查询方法
        /// <summary>
        /// 通用分页查询方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strColumns">查询字段名</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <param name="pageSize">每页数据数量</param>
        /// <param name="currentIndex">当前页数</param>
        /// <param name="recordOut">数据总量</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectPaging(string tableName, string strColumns, string strWhere, string strOrder, int pageSize, int currentIndex, out int recordOut)
        {
            DataTable dt = new DataTable();
            recordOut = Convert.ToInt32(ExecuteScalar("select count(*) from " + tableName, CommandType.Text));
            string pagingTemplate = "select {0} from {1} where {2} order by {3} limit {4} offset {5} ";
            int offsetCount = (currentIndex - 1) * pageSize;
            string commandText = String.Format(pagingTemplate, strColumns, tableName, strWhere, strOrder, pageSize.ToString(), offsetCount.ToString());
            using (DbDataReader reader = ExecuteReader(commandText, CommandType.Text))
            {
                if (reader != null)
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        #endregion

        #region 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// <summary>
        /// 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// </summary>
        /// <param name="cmd">Command对象</param>
        /// <param name="conn">Connection对象</param>
        /// <param name="trans">Transcation对象</param>
        /// <param name="useTrans">是否使用事务</param>
        /// <param name="cmdType">SQL字符串执行类型</param>
        /// <param name="cmdText">SQL Text</param>
        /// <param name="cmdParms">SQLiteParameters to use in the command</param>
        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, ref SQLiteTransaction trans, bool useTrans, CommandType cmdType, string cmdText, params SQLiteParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (useTrans)
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
            }


            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #endregion

    }
}
