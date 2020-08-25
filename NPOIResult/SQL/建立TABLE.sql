Create Table dbo.Member(
Account  nvarchar(50)   not null,
Password nvarchar(50)   not null,
Name     nvarchar(10)   not null,
Phone    nvarchar(10)   not null,
Tel      varchar (10)   null,
Gender   char(1)        not null,
Birthday datetime       not null
)