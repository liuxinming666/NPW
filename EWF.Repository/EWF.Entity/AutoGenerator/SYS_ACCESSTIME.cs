/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_ACCESSTIME                                     
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
	[Table("TBL_SYS_ACCESSTIME")]
	public class SYS_ACCESSTIME:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String AID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String UID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime LoginTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime LoginOutTime {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(1)]
		public String ATYPE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String APPIP {get;set;}


	}
}
