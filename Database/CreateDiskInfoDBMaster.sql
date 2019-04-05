Drop table IF EXISTS  FSEntities;
Create table FSEntities
(
    ID INT PRIMARY KEY IDENTITY (1, 1),
    EnumId TinyInt Unique NOT NULL,
    DisplayName VARCHAR (255) NOT NULL
); 
Select * from FSEntities;
insert into FSEntities 
(EnumId,DisplayName)
Values 
(0,'Directory')
,(1,'Archive')
,(2,'File')
;
Select * from FSEntities;

Drop table IF EXISTS  FSEntityInfo;
Create table FSEntityInfo
(
    id INT PRIMARY KEY IDENTITY (1, 1),
    DisplayName VARCHAR (255) NOT NULL,
	ByteLength BigInt ,
	MD5Hash BigInt,
	FK_FSEntitiesID INT FOREIGN KEY REFERENCES  FSEntities(ID)
) AS NODE; 
Select * from FSEntityInfo;

Drop table IF EXISTS  DiskMfgrInfo;
Create table DiskMfgrInfo
(
    id INT PRIMARY KEY IDENTITY (1, 1),
    DisplayName VARCHAR (255) NOT NULL,
);
insert into DiskMfgrInfo 
(DisplayName)
Values 
('Western Digital')
,('Seagate')
,('SanDisk')
;
Select * from DiskMfgrInfo;

Drop table IF EXISTS  DiskInfo;
Create table DiskInfo
(
    id INT PRIMARY KEY IDENTITY (1, 1),
    DisplayName VARCHAR (255) NOT NULL,
    SerialNumber VARCHAR (255) NOT NULL,
    FK_DiskMfgrInfoID INT FOREIGN KEY REFERENCES  DiskMfgrInfo(ID),
) AS NODE; 
Select * from DiskInfo;

Drop table IF EXISTS  DiskPartitionInfo;
Create table DiskPartitionInfo
(
    id INT PRIMARY KEY IDENTITY (1, 1),
    DisplayName VARCHAR (255) NOT NULL,
    PartitionType VARCHAR (255) NOT NULL,
    PartitionSize BigInt NOT NULL,
    FK_DiskInfoID INT FOREIGN KEY REFERENCES  DiskInfo(ID),
) AS NODE; 
Select * from DiskPartitionInfo;
