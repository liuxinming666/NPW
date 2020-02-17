/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-05 11:39:58                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：STATIONINFO_SURVEY                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWF.Entity
{
	/// <summary>
	/// zhujun
	/// 2019-06-05 11:39:58
	/// 
	/// </summary>
	[Table("TBL_STATIONINFO_SCHEME")]
	public class STATIONINFO_SCHEME : IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 ID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(8)]
		public String STCD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String STNM {get;set;}

		/// <summary>
		///  测报方案上传URL
		/// </summary>
		[MaxLength(200)]
		public String FILE_URL1 {get;set;}

        /// <summary>
		///  任务书上传URL
		/// </summary>
		[MaxLength(200)]
        public String FILE_URL2 { get; set; }
    }
}