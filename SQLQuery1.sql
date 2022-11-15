CREATE TABLE Users
(
	UserId INT PRIMARY KEY IDENTITY,
	Username VARCHAR(255) NOT NULL,
	Password VARCHAR(255) NOT NULL,
	Title VARCHAR(255) DEFAULT 'Employee'
);

INSERT INTO Users (Username, Password, Title)
	VALUES
	('Kate','Ward','Manager'),
	('Edel', 'Idle','Employee');

SELECT * FROM Project1.Users;
SELECT * FROM Project1.Tickets;

DELETE FROM Users WHERE Username = 'Register';

INSERT INTO Project1.Tickets (Amount, Description, EmployeeId)
VALUE
	(35, 'For fun',2);

SELECT TicketNum, Amount, Description, isPending, EmployeeId, ApprovedBy FROM Project1.Tickets
WHERE ApprovedBy IS NULL