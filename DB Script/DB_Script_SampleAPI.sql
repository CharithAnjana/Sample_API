CREATE DATABASE SampleAPI

GO
USE SampleAPI

GO
CREATE TABLE Customer
(
	UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Username VARCHAR(30) NOT NULL,
	Email VARCHAR(20) NOT NULL,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20) NOT NULL,
	CreatedOn DATETIME NOT NULL,
	IsActive BIT NOT NULL
);
GO
INSERT INTO Customer(UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive) 
VALUES ('cee43d8f-2eef-4e09-84e9-8a9a15dc3c8e', 'charith123', 'charith@gmail.com', 'Charith', 'Bandara', 2023-05-06, 1),
('461b64dd-1d80-4792-9e07-c898b7f47bbb', 'kasun123', 'kasun@gmail.com', 'Kasun', 'Perera', 2023-05-06, 1)


GO
CREATE TABLE Supplier
(
	SupplierId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	SupplierName VARCHAR(50) NOT NULL,
	CreatedOn DATETIME NOT NULL,
	IsActive BIT NOT NULL
);
GO
INSERT INTO Supplier (SupplierId, SupplierName, CreatedOn, IsActive) 
VALUES ('9a335b18-4669-43fa-a8e8-4993527e358a', 'Saman Perera',2023-05-02, 1),
('63c1e037-3632-4b08-87ac-5ec06922d812', 'Anil Fernando',2023-05-04, 1)

GO
CREATE TABLE Product
(
	ProductId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	ProductName VARCHAR(50) NOT NULL,
	UnitPrice DECIMAL(10,2) NOT NULL,
	SupplierId UNIQUEIDENTIFIER NOT NULL,
	CreatedOn DATETIME NOT NULL,
	IsActive BIT NOT NULL,
	FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);
GO
INSERT INTO Product (ProductId, ProductName, UnitPrice, SupplierId, CreatedOn, IsActive) VALUES 
('1aa019f1-aba3-4632-be9d-a32dbb7f807e', 'Book', CAST(200.00 AS Decimal(10, 2)), '9a335b18-4669-43fa-a8e8-4993527e358a', 2023-05-06, 1),
('717c60c0-cf2e-4d96-812d-e8628e06b4fd', 'Pen', CAST(50.00 AS Decimal(10, 2)), '63c1e037-3632-4b08-87ac-5ec06922d812', 2023-05-05, 1)

GO
CREATE TABLE [Order]
(
	OrderId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	ProductId UNIQUEIDENTIFIER NOT NULL,
	OrderStatus TINYINT NOT NULL,
	OrderType TINYINT NOT NULL,
	OrderBy UNIQUEIDENTIFIER NOT NULL,
	OrderedOn DATETIME NOT NULL,
	ShippedOn DATETIME NOT NULL,
	IsActive BIT NOT NULL,
	FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
	FOREIGN KEY (OrderBy) REFERENCES Customer(UserId)
);
GO
INSERT INTO [Order] (OrderId, ProductId, OrderStatus, OrderType, OrderBy, OrderedOn, ShippedOn, IsActive) 
VALUES ('60c91d4e-c5ab-4ae5-a23a-a0d150c58931', '717c60c0-cf2e-4d96-812d-e8628e06b4fd', 1, 1, 'cee43d8f-2eef-4e09-84e9-8a9a15dc3c8e', 2023-05-06, 2023-05-06, 1),
('1276efd4-4391-4009-9c47-bf37e051280f', '1aa019f1-aba3-4632-be9d-a32dbb7f807e', 1, 1, 'cee43d8f-2eef-4e09-84e9-8a9a15dc3c8e', 2023-05-05, 2023-05-06, 1),
('63b6b304-2933-4b4f-b64f-e538144d2b02', '1aa019f1-aba3-4632-be9d-a32dbb7f807e', 1, 1, '461b64dd-1d80-4792-9e07-c898b7f47bbb', 2023-05-05, 2023-05-06, 1)

GO
CREATE PROCEDURE getActiveOrdersByCustomer(@CustomerId UNIQUEIDENTIFIER)
AS
BEGIN
	SELECT odr.OrderId, odr.OrderStatus, odr.OrderType, odr.OrderBy, odr.OrderedOn, odr.ShippedOn, 
	odr.IsActive AS OrderIsActive, prd.ProductId, prd.ProductName, prd.UnitPrice, prd.CreatedOn AS PrdCreatedOn, 
	prd.IsActive AS PrdIsActive, sup.SupplierId, sup.SupplierName, sup.CreatedOn AS SupCreatedOn, sup.IsActive AS SupIsAcive
	FROM [Order] odr 
	INNER JOIN Product prd ON prd.ProductId = odr.ProductId
	INNER JOIN Supplier sup ON sup.SupplierId = prd.SupplierId
	WHERE odr.OrderBy = @CustomerId AND odr.IsActive = 1;
END
GO