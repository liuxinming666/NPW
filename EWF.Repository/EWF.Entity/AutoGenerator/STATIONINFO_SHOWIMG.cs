/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-05 11:39:58                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：STATIONINFO_SHOWIMG                                     
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
	[Table("TBL_STATIONINFO_SHOWIMG")]
	public class STATIONINFO_SHOWIMG : IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 id {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(8)]
		public int show_Id { get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String file_Name { get;set;}

		/// <summary>
		///  图片URL
		/// </summary>
		[MaxLength(200)]
		public String file_Path { get;set;}

        /// <summary>
		///  扩展名
		/// </summary>
		[MaxLength(10)]
        public String file_Ext { get; set; }
        /// <summary>
		///  添加时间
		/// </summary>
		[MaxLength(50)]
        public DateTime add_Time { get; set; }
    }
}
