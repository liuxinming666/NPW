using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IRepository.SysManage
{
    public interface  ISYS_FACEQRepository : IDependency
    {
        /// <summary>
        /// 保存编辑后的数据到相应表格
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <param name="newdata"></param>
        /// <returns></returns>
        string SaveFacEqData(string stcd, string tableName, string fieldName,string  fieldType,string fieldContent);
    }
}
