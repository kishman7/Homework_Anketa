create database Persons;
use Persons;
create table Person (
        Name nvarchar(50),
        Surname nvarchar(50),
        Age Date,
        Country int,
        Gender nvarchar(50),
        OtherHobby nvarchar(50),
        Hobby nvarchar(max),
    )