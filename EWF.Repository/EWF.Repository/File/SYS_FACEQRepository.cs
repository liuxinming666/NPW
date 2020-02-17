using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository.SysManage;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Repository.SysManage
{
    public partial  class SYS_FACEQRepository:DefaultRepository, ISYS_FACEQRepository
    {
        public SYS_FACEQRepository(IOptionsSnapshot<DbOption> options) : base(options) { }
		/// <summary>
		/// 根据站码和表名获取数据
		/// </summary>
		/// <param name="stcd"></param>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public DataTable SearchFacEqData(string stcd, string tableName)
        {
            string sqlInnerText =string.Format("select * from {0}  where STCD ={1}", File_Schema+tableName, stcd);
            return new RepositoryBase(database).FindTable(sqlInnerText);
            //using (var db = fileDataBase.Connection)
            //{
            //    return db.Query<dynamic>(sqlInnerText, sqlParams);
            //}
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
            string sqlInnerText = "delete from " + File_Schema + tableName + " where STCD =@stcd";

            using (var db = database.Connection)
            {
                return db.Query<dynamic>(sqlInnerText, sqlParams);
            }
        }
        /// <summary>
        /// 保存编辑后的设施设备数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SaveFacEqData(string stcd, string tableName, string fieldName,string fieldType, string fieldContent)
        {
            var sqlParams = new DynamicParameters();
            sqlParams.Add("stcd", stcd);
            var db = new RepositoryBase(database);
            var result = "";
            //获取原数据
            DataTable oldtable = SearchFacEqData(stcd, tableName);
            DeleteFacEqData(stcd, tableName);//删除数据
            if (!string.IsNullOrWhiteSpace(fieldContent))
            {
                int cnt = 0;
                string[] nameAry = fieldName.Split(new string[] { "," }, StringSplitOptions.None);
                string[] typeAry = fieldType.Split(new string[] { "," }, StringSplitOptions.None);
                string[] contentAry = fieldContent.Split(new string[] { "$" }, StringSplitOptions.None);
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO " + File_Schema + tableName + " (");
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
                    strSql2.Append("INSERT INTO " + File_Schema + tableName + " (");
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
    }
}
