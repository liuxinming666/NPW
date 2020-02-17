using EWF.Data;
using EWF.Data.Repository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Repository._Database
{
	public class RTDBRepository
	{
		private IDatabase database;
		public RTDBRepository(IOptionsSnapshot<DbOption> options)
		{
			var dbOption = options.Get("RTDB_Opion");
			if (dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
		}
	}
}
