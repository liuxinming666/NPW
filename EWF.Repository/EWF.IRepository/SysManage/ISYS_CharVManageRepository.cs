using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository.SysManage
{
    public interface ISYS_CharVManageRepository : IDependency
    {
        #region 历史特征值
        /// <summary>
        /// 根据站码和类型查询数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        dynamic GetData(string stcd, string type);
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <param name="type">类型</param>
        /// <param name="field">字段组</param>
        /// <param name="fieldType">字段类型组</param>
        /// <param name="fieldContent">内容组</param>
        /// <returns></returns>
        string SaveData(string stcd, string type, string field, string fieldType, string fieldContent);
        #endregion
        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetZQLineData(string stcd);
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="field"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldContent"></param>
        /// <returns></returns>
        string SaveZQLineData(string stcd, string field, string fieldType, string fieldContent);
       #endregion
    }
}
