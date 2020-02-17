using EWF.Data;
using EWF.Data.Repository;
using EWF.Util.Data;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Repository
{
	public class DefaultRepository
	{
		public string Default_Schema = EWFConsts.Default_SchemaName;
		public string File_Schema = EWFConsts.File_SchemaName;
		public string RTDB_Schema = EWFConsts.RTDB_SchemaName;
		protected IDatabase database;
		public DefaultRepository(IOptionsSnapshot<DbOption> options)
		{
			var dbOption = options.Get("Default_Option");
			if (dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
		}
	}
	
	public class DefaultRepository<TEntity> : RepositoryBase<TEntity> where TEntity : class, new()
	{
		public string Default_Schema = EWFConsts.Default_SchemaName;
		public string File_Schema = EWFConsts.File_SchemaName;
		public string RTDB_Schema = EWFConsts.RTDB_SchemaName;

		public DefaultRepository(IOptionsSnapshot<DbOption> options)
		{
			var dbOption = options.Get("Default_Option");
			if (dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
		}
	}

}
