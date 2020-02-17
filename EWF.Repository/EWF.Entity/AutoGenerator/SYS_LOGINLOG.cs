/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_LOGINLOG                                     
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
	[Table("TBL_SYS_LOGINLOG")]
	public class SYS_LOGINLOG:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public String SLID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(60)]
		public String SLSYSTEMURL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String SLURL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(60)]
		public String SLMODULAR {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(60)]
		public String SLTYPE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String SLMESSAGE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(10)]
		public Int32 SLRESULT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SLOPERATOR {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(60)]
		public String SLOPERATORName {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? SLDATETIME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SLCREATEBY {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? SLCREATEON {get;set;}

		/// <summary>
		///  
		/// </summary>
		[Required]
		[MaxLength(10)]
		public Int32 SLDELETECODE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(60)]
		public String SLUSERIP {get;set;}


	}
}
