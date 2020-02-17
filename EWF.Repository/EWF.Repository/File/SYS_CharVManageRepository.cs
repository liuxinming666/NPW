using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Util;
using System.Dynamic;
using EWF.IRepository.SysManage;
using System.Data;
using EWF.Util.Data;

namespace EWF.Repository.SysManage
{
    public partial class SYS_CharVManageRepository :DefaultRepository, ISYS_CharVManageRepository
    {
		public string File_DBName = EWFConsts.File_DBName;
		public SYS_CharVManageRepository(IOptionsSnapshot<DbOption> options):base(options) {}

        #region 历史特征值
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd">站码</param>
        public dynamic GetData(string stcd,string type)
        {
            
            var tableName = "";
            switch (type)
            {
                case "1":
                    //最大水位
                    tableName =  "ST_ZSTAT_R";
                    break;
                case "2":
                    //最大流量
                    tableName =  "ST_QSTAT_R";
                    break;
                case "3":
                    //最大含沙量
                    tableName = "ST_SSTAT_R";
                    break;
                case "4":
                    //径流量
                    tableName =  "ST_JLSTAT_R";
                    break;
                case "5":
                    //输沙量
                    tableName =  "ST_SEDRF_R";
                    break;
                case "6":
                    //降雨量
                    tableName =  "ST_PPSTAT_R";
                    break;
                default:
                    tableName = "";
                    break;
            }
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            sqlParams.Add("tableName", tableName);
            //第一条
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT B.name AS COLUMN_NAME,C.value AS COMMENTS,D.type as DATA_TYPE, D.length as DATA_LENGTH, D.NULLABLE as NULLABLE ");
            strSql.Append($" FROM {File_DBName}sys.tables A");
            strSql.Append($" left join  {File_DBName}sys.columns B ON B.object_id = A.object_id");
            strSql.Append($" LEFT JOIN {File_DBName}sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id");
            strSql.Append(" INNER JOIN");
            strSql.Append($" (select a.id, a.colid, a.length, a.name, b.name as type, (case a.isnullable  when 1 then 'Y' else 'N' end) as NULLABLE from {File_Schema}syscolumns a, {File_Schema}sysTypes b where a.xtype = b.xtype) D");
            strSql.Append(" on D.id = B.object_id and d.colid = B.column_id");
            strSql.Append(" WHERE A.name =@tableName order by  B.column_id  asc ");




            //第二条语句
            strSql.Append("select * from "+ File_Schema + tableName + "  where STCD=@stcd and STTDRCD = '6'  order by IDTM desc");
            //switch (type)
            //{
            //    case "1":
            //        //最大水位
            //        strSql.Append("select STCD,IDTM,MAXZ,MINZ,AVZ,STTDRCD from ST_ZSTAT_R  where STCD=@stcd and STTDRCD = '6'  order by IDTM desc");
            //        break;
            //    case "2":
            //        //最大流量
            //        strSql.Append("select STCD,IDTM,MAXQ,MINQ,AVQ,STTDRCD from ST_QSTAT_R where STCD=@stcd and STTDRCD = '6' order by IDTM desc");
            //        break;
            //    case "3":
            //        //最大含沙量
            //        strSql.Append("select STCD,IDTM,MAXS,MINS,STTDRCD from ST_SSTAT_R where STCD=@stcd and STTDRCD = '6' order by IDTM desc");
            //        break;
            //    case "4":
            //        //径流量
            //        strSql.Append("select STCD,IDTM,round(ACCJL,2) as ACCJL,RUMO,RUDE,STTDRCD from ST_JLSTAT_R where STCD=@stcd and STTDRCD = '6' order by IDTM desc");
            //        break;
            //    case "5":
            //        //输沙量
            //        strSql.Append("select STCD,IDTM,round(STW,2) as STW,STMO,STTDRCD from ST_SEDRF_R where STCD=@stcd and STTDRCD = '6' order by IDTM desc");
            //        break;
            //    case "6":
            //        //降雨量
            //        strSql.Append(" select STCD,IDTM,round(ACCP,1) as ACCP,STTDRCD from ST_PPSTAT_R where STCD =@stcd  and STTDRCD = '6' order by IDTM desc");
            //        break;
            //    default:
            //        strSql.Append("");
            //        break;
            //}
            dynamic data = new ExpandoObject();
            using (var multi = database.Connection.QueryMultiple(strSql.ToString(), sqlParams))
            {
                data.Table = multi.Read<dynamic>().AsList();
                data.Table1 = multi.Read<dynamic>().AsList();
            }

            return data;
        }
        /// <summary>
        /// 根据站码和表名获取数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable SearchData(string stcd, string tableName)
        {
            string sqlInnerText = string.Format("select * from {0}  where STCD ={1}", tableName, stcd);
            return new RepositoryBase(database).FindTable(sqlInnerText);
         }
        /// <summary>
        /// 根据测站和表名删除数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> DeleteFacEqData(string stcd, string tableName)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = "delete from " + tableName + " where STCD =@stcd";

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 保存编辑后的历史特佂值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SaveData(string stcd, string type, string field, string fieldType, string fieldContent)
        {
            var db = new RepositoryBase(database);
            var tableName = "";
            switch (type)
            {
                case "1":
                    //最大水位
                    tableName = File_Schema + "ST_ZSTAT_R";
                    break;
                case "2":
                    //最大流量
                    tableName = File_Schema + "ST_QSTAT_R";
                    break;
                case "3":
                    //最大含沙量
                    tableName = File_Schema + "ST_SSTAT_R";
                    break;
                case "4":
                    //径流量
                    tableName = File_Schema + "ST_JLSTAT_R";
                    break;
                case "5":
                    //输沙量
                    tableName = File_Schema + "ST_SEDRF_R";
                    break;
                case "6":
                    //降雨量
                    tableName = File_Schema + "ST_PPSTAT_R";
                    break;
                default:
                    tableName = "";
                    break;
            }
            var result = "";
            //获取原数据
            DataTable oldtable = SearchData(stcd, tableName);
            DeleteFacEqData(stcd, tableName);//删除数据
            if (!string.IsNullOrWhiteSpace(fieldContent))
            {
                int cnt = 0;
                string[] nameAry = field.Split(new string[] { "," }, StringSplitOptions.None);
                string[] typeAry = fieldType.Split(new string[] { "," }, StringSplitOptions.None);
                string[] contentAry = fieldContent.Split(new string[] { "$" }, StringSplitOptions.None);
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO " + tableName + " (");
                for (int i = 0; i < nameAry.Length; i++)
                {
                    strSql.Append(nameAry[i].ToString() + ",");
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);

                strSql.Append(")VALUES");
                for (int j = 0; j < contentAry.Length; j++)
                {
                    strSql.Append("(");
                    string[] contentArychildren = contentAry[j].Split(new string[] { "," }, StringSplitOptions.None);
                    for (int k = 0; k < contentArychildren.Length - 1; k++)
                    {
                        if (k == contentArychildren.Length - 2)
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(contentArychildren[k].ToString().Replace("undefined", "")))
                                    strSql.Append("" + contentArychildren[k].ToString().Replace("undefined", "") + "");
                                else
                                    strSql.Append("null");
                            }
                            else
                                strSql.Append("'" + contentArychildren[k].ToString().Replace("undefined", "") + "'");
                        }
                        else
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(contentArychildren[k].ToString().Replace("undefined", "")))
                                    strSql.Append("" + contentArychildren[k].ToString().Replace("undefined", "") + ", ");
                                else
                                    strSql.Append("null,");
                            }
                            else
                            {
                                strSql.Append("'" + contentArychildren[k].ToString().Replace("undefined", "") + "', ");
                            }
                        }

                    }
                    strSql.Append("),");
                }
                try
                {
                    cnt = db.ExecuteBySql(strSql.ToString().Substring(0, strSql.ToString().Length - 1));
                    //删除成功
                    if (cnt > 0)
                    {
                        result ="true";
                    }
                }
                catch (Exception ex)
                {
                    //删除失败时重加加入之前的数据
                    StringBuilder strSql2 = new StringBuilder();
                    strSql2.Append("INSERT INTO " + tableName + " (");
                    for (int i = 0; i < nameAry.Length; i++)
                    {
                        strSql2.Append(nameAry[i].ToString() + ",");
                    }
                    strSql2 = strSql2.Remove(strSql2.Length - 1, 1);

                    strSql2.Append(")VALUES");
                    for (int j = 0; j < oldtable.Rows.Count; j++)
                    {
                        strSql2.Append("(");
                        for (int k = 0; k < typeAry.Length; k++)
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "")))
                                    strSql2.Append("" + oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "") + ", ");
                                else
                                    strSql2.Append("null,");
                            }
                            else
                            {
                                strSql2.Append("'" + oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "") + "', ");
                            }
                           
                        }
                        strSql2.Remove(strSql2.Length-2, 1);
                        strSql2.Append("),");
                    }
                    cnt = db.ExecuteBySql(strSql2.ToString().Substring(0, strSql2.ToString().Length - 1));
                    result = "false";
                   
                }
                return result;
            }
            else
            {
                return "true";
            }
        }
        #endregion

        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetZQLineData(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);

            string sqlInnerText = $"select a.STNM,b.BGTM,b.LNNM,b.PTNO,b.Z,b.Q,b.ZMEA,b.QMEA from {RTDB_Schema}ST_STBPRP_B a, {File_Schema}TBL_ZQRL b where"
                + " b.STCD = @stcd and a.STCD = b.STCD ";
            sqlInnerText += " order by b.BGTM desc,b.PTNO asc"; 
            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="field"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldContent"></param>
        /// <returns></returns>
        public string SaveZQLineData(string stcd, string field, string fieldType, string fieldContent)
        {
            var db = new RepositoryBase(database);
            var result = "";
            //获取原数据
            DataTable oldtable = SearchZQLineData(stcd);
            DeleteZQLineData(stcd);//删除数据
            if (!string.IsNullOrWhiteSpace(fieldContent))
            {
                int cnt = 0;
                string[] nameAry = field.Split(new string[] { "," }, StringSplitOptions.None);
                string[] typeAry = fieldType.Split(new string[] { "," }, StringSplitOptions.None);
                string[] contentAry = fieldContent.Split(new string[] { "$" }, StringSplitOptions.None);
                StringBuilder strSql = new StringBuilder();
                strSql.Append($"INSERT INTO {File_Schema}tbl_zqrl (");
                for (int i = 0; i < nameAry.Length; i++)
                {
                    strSql.Append(nameAry[i].ToString() + ",");
                }
                strSql = strSql.Remove(strSql.Length - 1, 1);

                strSql.Append(")VALUES");
                for (int j = 0; j < contentAry.Length; j++)
                {
                    strSql.Append("(");
                    string[] contentArychildren = contentAry[j].Split(new string[] { "," }, StringSplitOptions.None);
                    for (int k = 0; k < contentArychildren.Length - 1; k++)
                    {
                        if (k == contentArychildren.Length - 2)
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(contentArychildren[k].ToString().Replace("undefined", "")))
                                    strSql.Append("" + contentArychildren[k].ToString().Replace("undefined", "") + "");
                                else
                                    strSql.Append("null");
                            }
                            else
                                strSql.Append("'" + contentArychildren[k].ToString().Replace("undefined", "") + "'");
                        }
                        else
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(contentArychildren[k].ToString().Replace("undefined", "")))
                                    strSql.Append("" + contentArychildren[k].ToString().Replace("undefined", "") + ", ");
                                else
                                    strSql.Append("null,");
                            }
                            else
                            {
                                strSql.Append("'" + contentArychildren[k].ToString().Replace("undefined", "") + "', ");
                            }
                        }

                    }
                    strSql.Append("),");
                }
                try
                {
                    cnt = db.ExecuteBySql(strSql.ToString().Substring(0, strSql.ToString().Length - 1));
                    //删除成功
                    if (cnt > 0)
                    {
                        result = "true";
                    }
                }
                catch (Exception ex)
                {
                    //删除失败时重加加入之前的数据
                    StringBuilder strSql2 = new StringBuilder();
                    strSql2.Append($"INSERT INTO {File_Schema}tbl_zqrl (");
                    for (int i = 0; i < nameAry.Length; i++)
                    {
                        strSql2.Append(nameAry[i].ToString() + ",");
                    }
                    strSql2 = strSql2.Remove(strSql2.Length - 1, 1);

                    strSql2.Append(")VALUES");
                    for (int j = 0; j < oldtable.Rows.Count; j++)
                    {
                        strSql2.Append("(");
                        for (int k = 0; k < typeAry.Length; k++)
                        {
                            if (typeAry[k] == "number" || typeAry[k] == "numeric")
                            {
                                if (!string.IsNullOrWhiteSpace(oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "")))
                                    strSql2.Append("" + oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "") + ", ");
                                else
                                    strSql2.Append("null,");
                            }
                            else
                            {
                                strSql2.Append("'" + oldtable.Rows[j][nameAry[k]].ToString().Replace("undefined", "") + "', ");
                            }

                        }
                        strSql2.Remove(strSql2.Length - 2, 1);
                        strSql2.Append("),");
                    }
                    cnt = db.ExecuteBySql(strSql2.ToString().Substring(0, strSql2.ToString().Length - 1));
                    result = "false";

                }
                return result;
            }
            else
            {
                return "true";
            }
        }
        /// <summary>
        /// 根据站码获取数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public DataTable SearchZQLineData(string stcd)
        {
            string sqlInnerText = string.Format($"select * from {File_Schema}tbl_zqrl  where STCD ={0}", stcd);
            return new RepositoryBase(database).FindTable(sqlInnerText);
        }
        /// <summary>
        /// 根据测站删除数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> DeleteZQLineData(string stcd)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            string sqlInnerText = $"delete from {File_Schema}tbl_zqrl where STCD =@stcd";

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        #endregion

    }
}
