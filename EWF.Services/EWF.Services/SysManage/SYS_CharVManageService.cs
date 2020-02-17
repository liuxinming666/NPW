using EWF.IRepository.SysManage;
using EWF.IServices.SysManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWF.Services.SysManage
{
    public class SYS_CharVManageService : ISYS_CharVManageService
    {
        private ISYS_CharVManageRepository repository;
        public SYS_CharVManageService(ISYS_CharVManageRepository _epository)
        {
            repository = _epository;
        }
        #region  历史特征值
        /// <summary>
        /// 根据站码和类型查询数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public dynamic GetData(string stcd, string type)
        {
            var list = repository.GetData(stcd,type);
            return list;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <param name="type">类型</param>
        /// <param name="field">字段组</param>
        /// <param name="fieldType">字段类型组</param>
        /// <param name="fieldContent">内容组</param>
        /// <returns></returns>
        public string SaveData(string stcd, string type, string field, string fieldType, string fieldContent)
        {
            var list = repository.SaveData(stcd, type, field, fieldType, fieldContent);
            return list;
        }
        #endregion
        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetZQLineData(string stcd)
        {
            var list = repository.GetZQLineData(stcd);
            return list.ToList();
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
            var list = repository.SaveZQLineData(stcd,field, fieldType, fieldContent);
            return list;
        }
        #endregion
    }
}
