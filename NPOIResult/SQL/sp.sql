USE [Practice]
GO

/****** Object:  StoredProcedure [dbo].[spInsertUser]    Script Date: 2020/8/24 ¤W¤È 10:30:51 ******/
DROP PROCEDURE [dbo].[spSearch]
GO

/****** Object:  StoredProcedure [dbo].[spInsertUser]    Script Date: 2020/8/24 ¤W¤È 10:30:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE spSearch
@Account nvarchar(100)


/*
exec spSearch 'T0001'
*/
AS
BEGIN
select*  from Member
where Account=@Account
end