create table Zarade(
	Id int primary key,
	Id_radnika int references Radnici(Id),
	neto decimal(18,2)
)