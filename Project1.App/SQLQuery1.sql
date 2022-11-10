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

SELECT * FROM Users