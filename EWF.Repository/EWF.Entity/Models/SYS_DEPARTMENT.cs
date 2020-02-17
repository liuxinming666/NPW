/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 16:58:39                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_DEPARTMENT                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity.Models
{
	/// <summary>
	/// zhujun
	/// 2019-06-03 16:58:39
	/// 
	/// </summary>
	[Table("TBL_SYS_DEPARTMENT")]
	public class SYS_DEPARTMENT
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String DEPARTMENTID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UNITID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PARENTID {get;set;}

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
		[MaxLength(50)]
		public String SHORTNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String NATURE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MANAGER {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PHONE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String FAX {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String EMAIL {get;set;}

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
