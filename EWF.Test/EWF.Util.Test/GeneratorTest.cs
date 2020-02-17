

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

//using Czar.Cms.Models;
//using Czar.Cms.IRepository;
//using Czar.Cms.Repository.SqlServer;
//using Czar.Cms.Core.Models;
//using Czar.Cms.Core.CodeGenerator;
//using Czar.Cms.Core.Options;


namespace EWF.Util.Test
{
    /// <summary>
    /// yilezhu
    /// 2018.12.12
    /// 测试代码生成器
    /// 暂时只实现了SqlServer的实体代码生成
    /// </summary>
    public class GeneratorTest
    {
        public static void GeneratorModelForSqlServer()
        {
            var serviceProvider = Common.BuildServiceForSqlServer();
            var codeGenerator = serviceProvider.GetRequiredService<EWF.Util.CodeGenerator.CodeGeneratorBuilder>();
            codeGenerator.GenerateTemplateCodesFromDatabase(true);

        }

      
    }
}
