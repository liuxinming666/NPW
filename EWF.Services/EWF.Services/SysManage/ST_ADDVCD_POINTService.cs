using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Services
{
    public class ST_ADDVCD_POINTService : IST_ADDVCD_POINTService
    {
        private readonly IST_ADDVCD_POINTRepository repository;

        public ST_ADDVCD_POINTService(IST_ADDVCD_POINTRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 获取单位分页信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Stnm"></param>
        /// <param name="Addvcd"></param>
        /// <returns></returns>
        public Page<ST_ADDVCD_POINT> GetAddvcdPointData(int pageIndex, int pageSize, string Addvcd, string Stnm)
        {
            return repository.GetAddvcdPointData(pageIndex, pageSize, Addvcd, Stnm);

        }

        public Page<dynamic> GetAddPointData(int pageIndex, int pageSize, string Addvcd, string Stnm)
        {
            return repository.GetAddPointData(pageIndex,pageSize,Addvcd,Stnm);
        }

        public Page<dynamic> GetStcdData(int pageIndex, int pageSize, string Stcd, string Stnm)
        {
            return repository.GetStcdData(pageIndex, pageSize, Stcd, Stnm);
        }

        public string Insert(ST_ADDVCD_POINT entity)
        {
            var result = "";
            try
            {
                result = repository.Insert<string>(entity);
                if (!result.IsEmpty())
                {
                    result= "录入成功";
                }
            }
            catch {
                result = "录入失败";
            }
            return result;
        }
        public string Update(ST_ADDVCD_POINT entity)
        {
            var result = repository.Update(entity);
            if (result)
            {
                return "编辑成功";
            }
            return "编辑失败";
        }
        public string Delete(string Addvcd, string Stcd)
        {
            int result = repository.DeletePoint(Addvcd,Stcd);
            if (result > 0)
            {
                return "删除成功";
            }
            return "删除失败";
        }
        public ST_ADDVCD_POINT GetAddvcdPointByID(string ID)
        {
            var result = repository.Get(ID);
            return result;
        }
    }
}
