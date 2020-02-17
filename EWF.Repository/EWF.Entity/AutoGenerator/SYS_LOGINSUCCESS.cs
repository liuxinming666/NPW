/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_LOGINSUCCESS                                     
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
	[Table("TBL_SYS_LOGINSUCCESS")]
	public class SYS_LOGINSUCCESS:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String LSID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String USERIP {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String UID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String USERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime LOGINTIME {get;set;}


	}
}