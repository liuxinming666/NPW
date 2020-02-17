using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.CodeGenerator.Models
{
    /// <summary>
    /// yilezhu
    /// 2018.12.12
    /// 数据库中表属性
    /// </summary>
    [Serializable]
    public class DbTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表对应实体的名称
        /// </summary>
        public string ModelName
        {
            get
            {
                return TableName.StartsWith("TBL_") ? TableName.Replace("TBL_", "") : TableName;
            }
        }

        /// <summary>
        /// 表说明
        /// </summary>
        public string TableComment { get; set; }
        /// <summary>
        /// 字段集合
        /// </summary>
        public virtual List<DbTableColumn> Columns { get; set; } = new List<DbTableColumn>();
    }
}
