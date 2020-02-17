/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_SYSLOG                                     
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
	[Table("TBL_SYS_SYSLOG")]
	public class SYS_SYSLOG:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String SYSLOGID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String OBJECTID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String LOGTYPE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String IPADDRESS {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String IPADDRESSNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UNITID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String DEPARTMENTID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CREATEDATE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String CREATEUSERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String CREATEUSERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MODULEID {get;set;}

		/// <summary>
		///  
		/// </summary>
		public String REMARK {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String STATUS {get;set;}


	}
}
