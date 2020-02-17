/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-06-03 11:30:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：sys_role                                     
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
	[Table("TBL_SYS_ROLE")]
	public class SYS_ROLE:IEntity
	{
        [Key]
        public int ID { get; set; }
		/// <summary>
		///  
		/// </summary>
		
		public String RoleCode {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public String RoleSeq {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String RoleName {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(2048)]
		public String Description {get;set;}

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
        /// <summary>
		///  
		/// </summary>
		[MaxLength(6)]
        public String ADDVCD { get; set; }
        /// <summary>
		///  
		/// </summary>
		[MaxLength(20)]
        public int? TYPE { get; set; }


    }
}
