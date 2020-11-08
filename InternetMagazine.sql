use InternetMagazine

create table Customer
(
   Id_customer int primary key identity(1,1),
   FIO nvarchar(50) not null,
   Gender char(7) not null
   Check (Gender in ('М', 'Ж')),
   DateOfBirth date not null,
   Email nvarchar(50) not null unique,
   Phone nvarchar(50) not null unique
   Check (Phone between +79000000000 and +79999999999)
)

create table AccountCustomer
(
   Id_account int primary key identity(1,1),
   Id_customer int foreign key references Customer unique,
   [Login] nvarchar(50) not null unique,
   [Password] nvarchar(50) not null unique
)

create table Category
(
   Id_category int primary key identity(1,1),
   Name_category nvarchar(50) not null
)
create table Product
(
  Id_product int primary key identity(1,1),
  [Name] nvarchar(50) not null unique,
  Price int not null Default(1) Check (Price > 0),
  [Count] int not null Default(0) Check ([Count] > 0),
  [Description] nvarchar(200),
  [Image] nvarchar(50),
  Id_category int foreign key references Category
)

create table Basket
(
  Id_order int primary key identity(1,1),
  Id_customer int foreign key references Customer,
  Id_product int foreign key references Product,
  [Count] int not null Default(1) Check ([Count] > 1),
  SummOrder int not null,
)

create table [Order]
(
  Id_order int primary key identity(1,1),
  Id_customer int foreign key references Customer,
  Id_product int foreign key references Product,
  DateOfOrder datetime not null,
  [Address] nvarchar(50),
  [Count] int not null Default(1) Check ([Count] > 1),
  SummOrder int not null,
  StateOrder nvarchar(50) not null
  Check (StateOrder in ('В пути', 'Выполнен')),
  TypeOfPay nvarchar(50) not null
  Check (TypeOfPay in ('Банковская карта', 'Наличные')),
  TypeOfDelivery nvarchar(50) not null
  Check (TypeOfDelivery in ('Самовывоз', 'Курьерская доставка', 'Почта')),
  AdditionalCharges int
)

create table Review
(
  Id_Review int primary key identity(1,1),
  Id_customer int foreign key references Customer,
  Id_product int foreign key references Product,
  Comment nvarchar(300),
  DateOfWrite datetime not null,
  Mark int not null check (Mark between 2 and 5),
)
