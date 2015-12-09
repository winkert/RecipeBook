create table Measurements(
	mes_ID int identity primary key
	, mes_Name varchar(30)
	, mes_Description varchar(200)
	, mes_Type int
	)
create table Ingredients(
	ing_ID int identity primary key
	, ing_Name varchar(30)
	, ing_Description varchar(200)
	)
create table Recipes (
	rec_ID int identity primary key
	, rec_Name varchar(100)
	, rec_Description varchar(200)
	, rec_Source varchar(60)
	, rec_PreparationInstructions varchar(500)
	, rec_CookingInstructions varchar(500)
	, rec_EntryDate date
	)
create table RecipeIngredients(
	ri_ID int identity primary key
	, rec_ID int foreign key references Recipes(rec_ID)
	, ing_ID int foreign key references Ingredients(ing_ID)
	, mes_ID int foreign key references Measurements(mes_ID)
	, ri_Amount float
	)
go
--Recipe View
create view Full_Recipes as (
	select r.rec_Name, r.rec_Description
		, ri.ri_Amount, m.mes_Name, i.ing_Name
	from Recipes r
	join RecipeIngredients ri on r.rec_ID = ri.rec_ID
	join Ingredients i on ri.ing_ID = i.ing_ID
	join Measurements m on ri.mes_ID = m.mes_ID
	)
go