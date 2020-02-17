using EWF.IRepository.SysManage;
using EWF.IServices.SysManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EWF.Services.SysManage
{
    public class SYS_FACEQService: ISYS_FACEQService
    {
        private ISYS_FACEQRepository repository;
        public SYS_FACEQService(ISYS_FACEQRepository _epository)
        {
            repository = _epository;
        }
        /// <summary>
        /// 保存编辑后的数据到相应表格
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <param name="newdata"></param>
        /// <returns></returns>
        public string SaveFacEqData(string stcd, string tableName, string fieldName, string fieldType, string fieldContent)
        {
            var list = repository.SaveFacEqData(stcd, tableName, fieldName, fieldType, fieldContent);
            return list;
        }
    }
}
