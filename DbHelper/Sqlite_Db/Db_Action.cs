﻿using DbHelper.Db_Model;
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
        //public void Test_Data_insert(DataModel model)
        //{
        //    try
        //    {
        //        string strsql = @"

        //                        insert into TEST_DATA(
        //                        DATAID
        //                        ,DATAIDX16   
        //                        ,DATACONTENT                   

        //                        )values(
        //                         @DATAID
        //                        ,@DATAIDX16   
        //                        ,@DATACONTENT    
        //                        )
        //                        ";

        //        SQLiteParameter[] parameters = {
        //            new SQLiteParameter("DATAID",DbType.String)
        //            ,new SQLiteParameter("DATAIDX16",DbType.String)
        //            ,new SQLiteParameter("DATACONTENT",DbType.String,2000)


        //        };

        //        parameters[0].Value = model.id;
        //        parameters[1].Value = model.head;
        //        parameters[2].Value = model.text;

        //        int count = SQLiteHelper.ExecuteNonQuery(strsql, CommandType.Text, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        #region 测试计划数据操作

        /// <summary>
        /// 插入 测试计划 
        /// </summary>
        public void Test_Cofige_Insert(Test_Plan model)
        {
            try
            {
                string strsql = @"
                                insert into TEST_COFIGE(
      
                                                     DVNAME    
                                                    ,DVPOSITION
                                                    ,DVID      
                                                    ,TESTER    
                                                    ,OLTC_TS   

                                                    ,CONTACT_NUM
                                                    ,TEST_NUM  
                                                    ,SPLACE    
                                                    ,OILTEMP   
                                                    ,TEST_TIME 

                                                    ,TEST_TYPE 
                                                    ,GETINFO   
                                                    ,TESTSTAGE 
                                                    ,DJUST     
                                                    ,DESCRIBE 

                                                    ,SCURRENT  
                                                    ,ECURRENT  
                                                    ,TIME_UNIT 
                                                    ,V1    
                                                    ,V2        

                                                    ,V3        
                                                    ,C1        
                                                    ,C2        
                                                    ,C3        

                                )values(
                                                     @DVNAME    
                                                    ,@DVPOSITION
                                                    ,@DVID      
                                                    ,@TESTER    
                                                    ,@OLTC_TS   

                                                    ,@CONTACT_NUM
                                                    ,@TEST_NUM  
                                                    ,@SPLACE    
                                                    ,@OILTEMP   
                                                    ,@TEST_TIME 

                                                    ,@TEST_TYPE 
                                                    ,@GETINFO   
                                                    ,@TESTSTAGE 
                                                    ,@DJUST     
                                                    ,@DESCRIBE 

                                                    ,@SCURRENT  
                                                    ,@ECURRENT  
                                                    ,@TIME_UNIT 
                                                    ,@V1        
                                                    ,@V2   

                                                    ,@V3        
                                                    ,@C1        
                                                    ,@C2        
                                                    ,@C3      
                                )
                                ";

                SQLiteParameter[] parameters = {
                    new SQLiteParameter("DVNAME",DbType.String)
                    ,new SQLiteParameter("DVPOSITION",DbType.String)
                    ,new SQLiteParameter("DVID",DbType.String)
                    ,new SQLiteParameter("TESTER",DbType.String)
                    ,new SQLiteParameter("OLTC_TS",DbType.String)

                    ,new SQLiteParameter("CONTACT_NUM",DbType.String)
                     ,new SQLiteParameter("TEST_NUM",DbType.String)
                     ,new SQLiteParameter("SPLACE",DbType.String)
                     ,new SQLiteParameter("OILTEMP",DbType.String)
                     ,new SQLiteParameter("TEST_TIME",DbType.String)

                    ,new SQLiteParameter("TEST_TYPE",DbType.String)
                     ,new SQLiteParameter("GETINFO",DbType.String)
                     ,new SQLiteParameter("TESTSTAGE",DbType.String)
                     ,new SQLiteParameter("DJUST",DbType.String)
                     ,new SQLiteParameter("DESCRIBE",DbType.String)

                    ,new SQLiteParameter("SCURRENT",DbType.String)
                     ,new SQLiteParameter("ECURRENT",DbType.String)
                     ,new SQLiteParameter("TIME_UNIT",DbType.String)
                     ,new SQLiteParameter("V1",DbType.String)
                     ,new SQLiteParameter("V2",DbType.String)

                    ,new SQLiteParameter("V3",DbType.String)
                     ,new SQLiteParameter("C1",DbType.String)
                     ,new SQLiteParameter("C2",DbType.String)
                     ,new SQLiteParameter("C3",DbType.String)
                };

                parameters[0].Value = model.DVNAME;
                parameters[1].Value = model.DVPOSITION;
                parameters[2].Value = model.DVID;
                parameters[3].Value = model.TESTER;
                parameters[4].Value = model.OLTC_TS;

                parameters[5].Value = model.CONTACT_NUM;
                parameters[6].Value = model.TEST_NUM;
                parameters[7].Value = model.SPLACE;
                parameters[8].Value = model.OILTEMP;
                parameters[9].Value = model.TEST_TIME;

                parameters[10].Value = model.TEST_TYPE;
                parameters[11].Value = model.GETINFO;
                parameters[12].Value = model.TESTSTAGE;
                parameters[13].Value = string.IsNullOrEmpty(model.DJUST) ? "" : model.DJUST;
                parameters[14].Value = model.DESCRIBE;

                parameters[15].Value = model.SCURRENT;
                parameters[16].Value = model.ECURRENT;
                parameters[17].Value = model.TIME_UNIT;
                parameters[18].Value = string.IsNullOrEmpty(model.V1) ? "" : model.V1;
                parameters[19].Value = string.IsNullOrEmpty(model.V2) ? "" : model.V2;

                parameters[20].Value = string.IsNullOrEmpty(model.V3) ? "" : model.V3;
                parameters[21].Value = string.IsNullOrEmpty(model.C1) ? "" : model.C1;
                parameters[22].Value = string.IsNullOrEmpty(model.C2) ? "" : model.C2;
                parameters[23].Value = string.IsNullOrEmpty(model.C3) ? "" : model.C3;


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
