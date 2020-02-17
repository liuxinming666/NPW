using EWF.Entity;
using EWF.IRepository.SysManage;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.Services
{
    public class SYS__RsvrWarnService : ISYS__RsvrWarnService
    {
        private readonly ISYS__RsvrWarnRepository repository;

        public SYS__RsvrWarnService(ISYS__RsvrWarnRepository _repository)
        {
            repository = _repository;
        }
        public Page<dynamic> GetRsvrWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd)
        {
            var list = repository.GetRsvrWarnData(pageIndex, pageSize, stnm, type, addvcd);
            return list;
        }
        public string UpdateData(SYS_ST_RSVRFSR_B model)
        {
            return repository.UpdateData(model);
        }
    }
}
