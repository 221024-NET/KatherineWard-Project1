CREATE TABLE Project1.Users
(
	UserId INT PRIMARY KEY IDENTITY,
	Username VARCHAR(255) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	--Title VARCHAR(255) DEFAULT 'Employee'
	isManager BIT NOT NULL DEFAULT 0
);

-- DROP TABLE Project1.Users;

INSERT INTO Project1.Users (Username, Password)--, Title)
	VALUES
	('Kate','Ward'),--,'Manager'),
	('Edel', 'Idle')--,'Employee');

SELECT * FROM Project1.Users

GO

CREATE SCHEMA Project1;

GO

ALTER SCHEMA Project1
	TRANSFER dbo.Users;

GO

CREATE TABLE Project1.Tickets
(
	TicketNum INT PRIMARY KEY IDENTITY,
	Amount INT NOT NULL,
	Description VARCHAR(MAX) NOT NULL,
	isPending BIT NOT NULL DEFAULT 1,
	EmployeeId INT NOT NULL,
	ApprovedBy VARCHAR(255),
	FOREIGN KEY (EmployeeId) REFERENCES Project1.Users(UserId)
);

-- DROP TABLE Project1.Tickets;

-- tickets need id, pending status, 

GO

SELECT * FROM Project1.Tickets;

ALTER TABLE Project1.Users
ADD isManager BIT NOT NULL DEFAULT 0;

ALTER TABLE Project1.Users
DROP COLUMN Title;

ALTER TABLE Project1.Users
DROP CONSTRAINT DF__Users__Title__5CD6CB2B;

UPDATE Project1.Users SET isManager = 1
WHERE UserName = 'Kate';


INSERT INTO Project1.Tickets (Amount, Description, EmployeeId)
VALUES
	(35, 'For fun', 2);

INSERT INTO Project1.Tickets (Amount, Description, EmployeeId)
VALUES
	(150, 'Help me embezzle this', 11);

INSERT INTO Project1.Tickets (Amount, Description, EmployeeId, ApprovedBy)
VALUES
	(25, 'Summit Parking', 2, 'Katherine Ward');

--------------------------
	SELECT TicketNum, Amount, Description, isPending, Name FROM Project1.Tickets
	JOIN Project1.Users ON EmployeeId = UserId
	WHERE ApprovedBy IS NULL
--------------------------
	SELECT TicketNum, Amount, Description, isPending, Name FROM Project1.Tickets
	JOIN Project1.Users ON EmployeeId = UserId
	WHERE ApprovedBy IS NOT NULL AND Project1.Users.Username = 'Edel';
--------------------------

ALTER TABLE  Project1.Users
ADD Name VARCHAR(MAX)

UPDATE Project1.Users SET Name = 'Chaundra Ward'
WHERE UserName = 'Chaundra';
