select r.Id as Id, r.Ime as Ime,r.Prezime as Prezime, r.Adresa as Adresa ,z.Id as IdZarade,z.neto as Neto from Radnici as r join Zarade as z on r.Id=z.Id_radnika

insert into Zarade(Id,Id_radnika,neto)
values (2,2,40500)