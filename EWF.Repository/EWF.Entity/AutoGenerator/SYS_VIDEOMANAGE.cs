/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_USER                                     
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
	[Table("TBL_SYS_VIDEOMANAGE")]
	public class SYS_VIDEOMANAGE : IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String STCD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(80)]
		public String SNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String SIP {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(30)]
		public String SPORT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String USERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PASSWORD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String PASSWAY {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(30)]
		public String LINETYPE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(30)]
		public String SEBLONG {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String REMARKS { get;set;}
        /// <summary>
        ///  是否在首页显示
        /// </summary>
        [MaxLength(1)]
        public bool MAINPAGE { get; set; }

    }
}
