/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 11:30:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：sys_userRoleMap                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWF.Entity
{
	/// <summary>
	/// zhujun
	/// 2019-06-03 11:30:42
	/// 
	/// </summary>
	[Table("tBL_sys_userRoleMap")]
	public class SYS_USERROLEMAP:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 ID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		public String UserCode {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(100)]
		public String RoleCode {get;set;}


	}
}
