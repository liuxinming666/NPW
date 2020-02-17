/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_SYSLOGDETAIL                                     
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
	[Table("TBL_SYS_SYSLOGDETAIL")]
	public class SYS_SYSLOGDETAIL:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String SYSLOGDETAILID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String SYSLOGID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PROPERTYNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PROPERTYFIELD {get;set;}

		/// <summary>
		///  
		/// </summary>
		public String NEWVALUE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String OLDVALUE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CREATEDATE {get;set;}


	}
}
