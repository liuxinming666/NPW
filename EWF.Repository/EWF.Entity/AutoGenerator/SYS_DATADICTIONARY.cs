/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_DATADICTIONARY                                     
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
	[Table("TBL_SYS_DATADICTIONARY")]
	public class SYS_DATADICTIONARY:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String DATADICTIONARYID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UNITID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PARENTID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? ISTREE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String CATEGORY {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String DCODE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String FULLNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? ISENABLED {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? SORTCODE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? DELETEMARK {get;set;}

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
		[MaxLength(23)]
		public DateTime? MODIFYDATE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MODIFYUSERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MODIFYUSERNAME {get;set;}


	}
}
