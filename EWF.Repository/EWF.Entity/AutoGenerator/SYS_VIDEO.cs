/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_VIDEO                              
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWF.Entity
{
	/// <summary>
	/// zhujun
	/// 2019-05-23 16:35:45
	/// 
	/// </summary>
	[Table("TBL_SYS_VIDEO")]
	public class SYS_VIDEO : IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String STCD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		public String NAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public decimal LGTD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public decimal LTTD { get;set;}    
	}
}
