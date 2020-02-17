/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/2/25 18:28:27
 * 描  述：CodeGenerateOption
 * *********************************************/
using EWF.Util.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.CodeGenerator.Options
{
    public class CodeGenerateOption : DbOption
    {
        /// <summary>
        /// 数据库连接名称
        /// </summary>
        public string ConnName { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 代码生成时间
        /// </summary>
        public string GeneratorTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string ModelsNamespace { get; set; }
        /// <summary>
        /// 实体映射类命名空间
        /// </summary>
        public string MappingNamespace { get; set; }
        /// <summary>
        /// 仓储接口命名空间
        /// </summary>
        public string IRepositoryNamespace { get; set; }
        /// <summary>
        /// 仓储命名空间
        /// </summary>
        public string RepositoryNamespace { get; set; }
        /// <summary>
        /// 服务命名接口空间
        /// </summary>
        public string IServicesNamespace { get; set; }
        /// <summary>
        /// 服务命名空间
        /// </summary>
        public string ServicesNamespace { get; set; }
    }
}
