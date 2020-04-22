Create Database FinalProject_FoodPort
use FinalProject_FoodPort
Create Table Admins
(
 AdminID int primary key identity(1234001,1),
 AdminName varchar(30) not null,
 AdminPhoneNumber varchar(30) unique not null,
 AdminEmailID varchar(30),
)
Create Table Customer
(
  CustomerID int primary key identity(1001,1),
  CustomerName varchar(50) not null,
  CustomerEmail varchar(50) not null,
  CustomerPhoneNumber varchar(10) unique not null,
  CustomerAddress varchar(50) not null,
  CustomerImage varchar(50)
)
Create Table Restaurant
(
  RestaurantID int primary key identity(10101,1),
  RestaurantName varchar(30) not null,
  RestaurantCity varchar(30) not null,
  RestaurantLocality varchar(30) not null,
  RestaurantState varchar(30) not null,
  RestaurantPhoneNumber varchar(30) not null,
  RestaurantEmail varchar(30),
  RestaurantOpenTime varchar(30),
  RestaurantCloseTime varchar(30),
  RestaurantStatus varchar(30) not null,
  RestaurantImage varchar(30),
)
Create Table Menu
(
  ItemID int identity(1,1) not null,
  RestaurantID int foreign key references Restaurant(RestaurantID) not null,
  ItemCategory varchar(30) not null,
  ItemVegNonVeg varchar(30) not null,
  ItemName varchar(30) not null,
  ItemPrice decimal(10,2) not null,
  ItemDetails varchar(50),
  ItemTaste varchar(30),
  ItemImage varchar(30),
  primary key(RestaurantID,ItemName)
)
Select * from AccountsCustomer
Create Table Cart
(
  CustomerID int,
  RestaurantID int,
  itemID int,
  itemName varchar(30),
  itemQuantity int,
  itemPrice decimal(10,2),
  primary key(CustomerID,RestaurantID,itemID)
)
Create Table Address
(
 CustomerID int foreign key references Customer(CustomerID) not null,
 CustomerAddress varchar(100) 
)
Create Table AccountsCustomer
(
  AccountNumber varchar(30) primary key,
  AccountHolderName varchar(30),
  AccountBalance decimal,
  AccountCardNumber varchar(30),
  Validfrom varchar(30),
  ValidTo varchar(30),
  CVV varchar(30)
)
select * from BankTransaction
insert AccountsCustomer values('1234567890',ae varchar(30),
  AccountBalance decimal,
)
insert AccountsRestaurant values('8877665544','Wrapsto',0)
Create Table BankTransaction
(
  TransactionID int identity(1011101,1),
  CustomerAccountNumber varchar(30) foreign key references AccountsCustomer(AccountNumber),
  RestaurantAccountNumber varchar(30) foreign key references AccountsRestaurant(AccountNumber),
  TransactionType varchar(30) not null,
  TransactionAmount decimal(10,2) not null,
  TransactionDate datetime not null,
)
Create Table OrderDetails
(
  OrderID int primary key identity(101,1),
  CustomerID int foreign key references Customer(CustomerID) not null,
  RestaurantID int foreign key references Restaurant(RestaurantID) not null,
  TransactionID int,
  OrderAmount decimal(10,2) not null,
  OrderDate datetime not null,
  OrderAddress varchar(50) not null,
  OrderStatus varchar(30) not null,
  unique(OrderID,CustomerID,RestaurantID)
)
Select * from Restaurant
insert OrderDetails values(1003,10106,110010,1000,GetDate(),'Bhubaneswar','Order Placed')
Create Table OrderItems
(
  OrderID int foreign key references OrderDetails(OrderID),
  ItemID int not null,
  ItemName varchar(30) not null,
  ItemQuantity int not null,
  ItemPrice decimal(10,4) not null,
  primary key(OrderID,ItemID)
)










Select * from Customer
insert FoodPortAdmin values('Souradeep@iNautix','pass@123')
Select Count(*) from Customer where CustomerPhoneNumber=1234567890
update Customer set CustomerImage='' where CustomerID=1004
update Customer set CustomerImage='' where CustomerID=1004
update Customer set CustomerImage='' where CustomerID=1006
update Customer set CustomerImage='' where CustomerID=1007