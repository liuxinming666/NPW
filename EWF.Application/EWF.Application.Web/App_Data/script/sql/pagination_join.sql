
/****** Object:  StoredProcedure [dbo].[Pagination_Join]    Script Date: 2019/8/7 14:25:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Pagination_Join]
(
	 @SqlWhere varchar(8000) = Null,--条件语句(不用加where)
	 @PageSize int,                    --每页多少条记录
	 @PageIndex int = 1 ,            --指定当前为第几页
	 @SqlTable varchar(8000),        --表名
	 @SqlField varchar(8000) = '*',    --字段名(全部字段为*)
	 @SqlOrder varchar(200),        --排序字段(必须!支持多字段)
	 @Count int=0 output					--返回总记录数
)
AS
BEGIN
    
     Begin Tran --开始事务

    Declare @sql nvarchar(4000); 

 	declare @TotalPage int;            --返回总页数

        --计算总记录数
             
        if (@SqlWhere='' or @SqlWhere=NULL)
            set @sql = 'select @Count = count(*) from ' + @SqlTable
        else
            set @sql = 'select @Count = count(*) from ' + @SqlTable + ' where ' + @SqlWhere

       EXEC sp_executesql @sql,N'@Count int OUTPUT',@Count OUTPUT--计算总记录数       
		 --set @Count=100


    --计算总页数
    select @TotalPage=CEILING((@Count+0.0)/@PageSize)

    if (@SqlWhere='' or @SqlWhere=NULL)
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @SqlOrder + ') as rowId,' + @SqlField + ' from ' + @SqlTable 
    else
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(order by ' + @SqlOrder + ') as rowId,' + @SqlField + ' from ' + @SqlTable + ' where ' + @SqlWhere    
        

    --处理页数超出范围情况
    if @PageIndex<=0 
        Set @PageIndex = 1
    
    if @PageIndex>@TotalPage
        Set @PageIndex = @TotalPage

     --处理开始点和结束点
    Declare @StartRecord int
    Declare @EndRecord int
    
    set @StartRecord = (@PageIndex-1)*@PageSize + 1
    set @EndRecord = @StartRecord + @PageSize - 1

    --继续合成sql语句
    set @Sql = @Sql + ') as t where rowId between ' + Convert(varchar(50),@StartRecord) + ' and ' +  Convert(varchar(50),@EndRecord)
     print @sql   
    Exec(@Sql)
    ---------------------------------------------------
    If @@Error <> 0
      Begin
        RollBack Tran
        Return -1
      End
     Else
      Begin
        Commit Tran
        Return @Count ---返回记录总数
      End   
END


GO


