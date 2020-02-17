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
	

	public class FileRepository<TEntity> : RepositoryBase<TEntity> where TEntity : class, new()
	{
		public string Default_Schema = EWFConsts.Default_SchemaName;

		public FileRepository(IOptionsSnapshot<DbOption> options)
		{
			var dbOption = options.Get("File_Opion");
			if (dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
		}
	}

}
