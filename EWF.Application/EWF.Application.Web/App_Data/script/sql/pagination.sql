

/****** Object:  StoredProcedure [dbo].[Pagination]    Script Date: 2019/8/7 9:57:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Pagination]
(
	 @SqlWhere varchar(8000) = Null,--�������(���ü�where)
	 @PageSize int,                    --ÿҳ��������¼
	 @PageIndex int = 1 ,            --ָ����ǰΪ�ڼ�ҳ
	 @SqlTable varchar(8000),        --����
	 @SqlField varchar(8000) = '*',    --�ֶ���(ȫ���ֶ�Ϊ*)
	 @SqlPK varchar(50),--���� 
	 @SqlOrder varchar(200),        --�����ֶ�(����!֧�ֶ��ֶ�)
	 @Count int=0 output					--�����ܼ�¼��
)
AS
BEGIN
    
     Begin Tran --��ʼ����

    Declare @sql nvarchar(4000); 

 	declare @TotalPage int;            --������ҳ��

	if(@SqlOrder='' or @SqlOrder=null)
		begin
			set @SqlOrder='Order by '+@SqlPK
		end

        --�����ܼ�¼��
             
        if (@SqlWhere='' or @SqlWhere=NULL)
            set @sql = 'select @Count = count('+@SqlPK+') from ' + @SqlTable
        else
            set @sql = 'select @Count = count('+@SqlPK+') from ' + @SqlTable + ' with(nolock) where 1=1 ' + @SqlWhere

		print @sql
        EXEC sp_executesql @sql,N'@Count int OUTPUT',@Count OUTPUT--�����ܼ�¼��       



    --������ҳ��
    select @TotalPage=CEILING((@Count+0.0)/@PageSize)

    if (@SqlWhere='' or @SqlWhere=NULL)
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(' + @SqlOrder + ') as rowId,' + @SqlField + ' from ' + @SqlTable 
    else
        set @sql = 'Select * FROM (select ROW_NUMBER() Over(' + @SqlOrder + ') as rowId,' + @SqlField + ' from ' + @SqlTable + ' with(nolock) where 1=1 ' + @SqlWhere    
        

    --����ҳ��������Χ���
    if @PageIndex<=0 
        Set @PageIndex = 1
    
    if @PageIndex>@TotalPage
        Set @PageIndex = @TotalPage

     --����ʼ��ͽ�����
    Declare @StartRecord int
    Declare @EndRecord int
    
    set @StartRecord = (@PageIndex-1)*@PageSize + 1
    set @EndRecord = @StartRecord + @PageSize - 1

    --�����ϳ�sql���
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
        Return @Count ---���ؼ�¼����
      End   
END

GO


