CREATE TABLE items (
    id int IDENTITY(1,1) PRIMARY KEY,
    firstname nvarchar(60),
    lastname nvarchar(60),
    phonenumber varchar(40),
    email varchar(50),
    advtext text,
    age int,
    priceusd numeric(5,2),
    course numeric(5,2),
    priceuah numeric(11,4)
);

CREATE TABLE feedback (
    id int IDENTITY(1,1) PRIMARY KEY,
    email varchar(50),
    msgtext text
);