/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：sys_menu                                     
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
	[Table("SYS_MENU")]
	public class SYS_MENU:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String MenuCode {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		public String ParentCode {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String MenuName {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String URL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String IconClass {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String IconURL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? MenuSeq {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(2048)]
		public String Description {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(1)]
		public Boolean? IsVisible {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(1)]
		public Boolean? IsEnable {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(20)]
		public String CreatePerson {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CreateDate {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(20)]
		public String UpdatePerson {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? UpdateDate {get;set;}


	}
}
