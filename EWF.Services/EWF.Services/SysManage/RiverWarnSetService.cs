using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Services
{
    public class RiverWarnSetService : IRiverWarnSetService
    {
        private readonly IRiverWarnSetRepository repository;
        public RiverWarnSetService(IRiverWarnSetRepository _repository)
        {
            repository = _repository;
        }
        public Page<dynamic> GetRiverWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd)
        {
            return repository.GetRiverWarnData(pageIndex, pageSize, stnm, type, addvcd);
        }
        public string UpdateData(ST_RVFCCH_B model)
        {
            return repository.UpdateData(model);
        }
    }
}
