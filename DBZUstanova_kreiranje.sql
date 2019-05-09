/*use master
drop database DBZUstanovaBeta*/

/*---Kreiranje baze DBZUstanovaBeta---*/
/*create database DBZUstanovaBeta*/

/*---Kreiranje tabela---*/
use DBZUstanovaBeta

/*---Kreiranje tabele Korisnik---*/
create table Korisnik(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	Ime nvarchar(30) not null,
	Prezime nvarchar(30) not null,
	KorisnickoIme nvarchar(30) unique not null, /*kod lekara i admina je korisnicko ime, a kod pacijenta broj zdravstvene knjizice*/
	Lozinka varchar(30) not null,
	DatumRodjenja date not null
);

/*---Kreiranje tabele Pacijent---*/
create table Pacijent(
	ID int not null, /*primarni/strani kljuc*/
	JMBG varchar(13) unique not null,
	NosilacOsiguranja nvarchar(60),
	SrodstvoSaNosiocem nvarchar(50),
	IDOdeljenja int, /*strani kljuc*/
	IDUstanove int, /*strani kljuc*/
	KrvnaGrupa varchar(5),
	Pol nchar,
	Adresa nvarchar(30) not null, /*Ulica i broj*/
	Telefon varchar(20) not null,
	Email varchar(60) unique not null,
	IstekOsiguranja date not null,
	Odobren int not null
);

/*---Kreiranje tabele Lekar---*/
create table Lekar(
	ID int not null, /*primarni/strani kljuc*/
	TipLekara nvarchar(50) not null,
	IDOdeljenja int not null, /*strani kljuc*/
	Licenca nvarchar(50),
	Specijalizacija nvarchar(60),
	Slika nvarchar(50)
);

/*---Kreiranje tabele Administrator---*/
create table Administrator(
	ID int not null /*primarni/strani kljuc*/
);

/*---Kreiranje tabele Ustanova---*/

create table Ustanova(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	Naziv nvarchar(50) not null,
	Adresa nvarchar(50) not null,
	Telefon varchar(20) not null,
	Email varchar(60) not null,
	Slika nvarchar(50)
);

/*---Kreiranje tabele Odeljenje---*/
create table Odeljenje(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDUstanove int not null, /*strani kljuc*/
	Naziv nvarchar(50) not null,
	SlobodnihMesta int not null
);

/*---Kreiranje tabele Karton---*/
create table Karton(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDPacijenta int not null, /*strani kljuc - pacijent ciji je karton*/
	IDLekara int not null, /*strani kljuc - Lekar koji je menjao nalaze*/
	DatumVremeNalaza date not null,
	Disanje int not null,
	Puls int not null,
	TelesnaTemperatura float not null,
	KrvniPritisak varchar(20) not null,
	Mokraca varchar(20) not null,
	Stolica varchar(20) not null,
	KrvaSlika varchar(20) not null,
	SpecificniPregled text
);

/*---Kreiranje tabele Izvestaj---*/
create table Izvestaj(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDPacijenta int not null, /*strani kljuc - pacijent ciji je izvestaj*/
	IDLekara int not null, /*strani kljuc - Lekar koji je napisao izvestaj*/
	DatumPregleda date not null,
	Dijagnoza text not null
);

/*---Kreiranje tabele Uput---*/
create table Uput(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDPacijenta int not null, /*strani kljuc - pacijent ciji je upu*/
	IDLekaraOd int not null, /*strani kljuc - Lekar koji je napisao uput*/
	IDLekaraKome int not null, /*strani kljuc - Lekar kojem se upucuje pacijent*/
	DatumPregleda date not null,
	IDOdeljenja int not null /*strani kljuc*/
);
alter table uput add ZavrsenPregled int

/*---Kreiranje tabele RadnoVremeLekara---
create table RadnoVremeLekara(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDLekara int not null, /*strani kljuc*/
	Pocetak time not null,
	Kraj time not null
);*/

/*---Kreiranje tabele ZakazivanjePregleda---*/
create table ZakazivanjePregleda(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	IDPacijenta int not null, /*strani kljuc*/
	IDLekara int not null, /*strani kljuc*/
	DatumPregleda date not null,
	VremePregleda time not null,
	DatumZakazivanja date not null
);
alter table zakazivanjepregleda add ZavrsenPregled int

create table Vesti(
	ID int identity(1, 1) not null, /*primarni kljuc*/
	Naslov nvarchar(50) not null,
	Tekst text not null,
	DatumObjave date not null,
	Slika nvarchar(50)
);

create table ONamaPodaci(
	ID int identity (1, 1) not null,
	ONama text,
	Adresa nvarchar(50),
	Telefon varchar(20),
	Email nvarchar(50)
);

/*---Dodavanje Primarnih i stranih kljuceva---*/
ALTER TABLE Korisnik
ADD CONSTRAINT PK_Korisnik PRIMARY KEY (ID);

ALTER TABLE Pacijent
ADD CONSTRAINT PK_Pacijent PRIMARY KEY (ID);

ALTER TABLE Pacijent
ADD CONSTRAINT FK_PacijentKorisnik
FOREIGN KEY (ID) REFERENCES Korisnik(ID);

ALTER TABLE Lekar
ADD CONSTRAINT PK_Lekar PRIMARY KEY (ID);

ALTER TABLE Lekar
ADD CONSTRAINT FK_LekarKorisnik
FOREIGN KEY (ID) REFERENCES Korisnik(ID);

--ALTER TABLE TipLekara
--ADD CONSTRAINT PK_TipLekar PRIMARY KEY (ID);

--ALTER TABLE Lekar
--ADD CONSTRAINT FK_LekarTipLekara
--FOREIGN KEY (TipLekara) REFERENCES TipLekara(ID);

ALTER TABLE Administrator
ADD CONSTRAINT PK_Administrator PRIMARY KEY (ID);

ALTER TABLE Administrator
ADD CONSTRAINT FK_AdministratorKorisnik
FOREIGN KEY (ID) REFERENCES Korisnik(ID);

ALTER TABLE Ustanova
ADD CONSTRAINT PK_Ustanova PRIMARY KEY (ID);

ALTER TABLE Odeljenje
ADD CONSTRAINT PK_Odeljenje PRIMARY KEY (ID);

ALTER TABLE Odeljenje
ADD CONSTRAINT FK_OdeljenjeUstanova
FOREIGN KEY (IDUstanove) REFERENCES Ustanova(ID);

ALTER TABLE Pacijent
ADD CONSTRAINT FK_PacijentOdeljenje
FOREIGN KEY (IDOdeljenja) REFERENCES Odeljenje(ID);

ALTER TABLE Pacijent
ADD CONSTRAINT FK_PacijentUstanova
FOREIGN KEY (IDUstanove) REFERENCES Ustanova(ID);

ALTER TABLE Lekar
ADD CONSTRAINT FK_LekarOdeljenje
FOREIGN KEY (IDOdeljenja) REFERENCES Odeljenje(ID);

ALTER TABLE Karton
ADD CONSTRAINT PK_Karton PRIMARY KEY (ID);

ALTER TABLE Karton
ADD CONSTRAINT FK_KartonPacijent
FOREIGN KEY (IDPacijenta) REFERENCES Korisnik(ID);

ALTER TABLE Karton
ADD CONSTRAINT FK_KartonLekar
FOREIGN KEY (IDLekara) REFERENCES Korisnik(ID);

ALTER TABLE Izvestaj
ADD CONSTRAINT PK_Izvestaj PRIMARY KEY (ID);

ALTER TABLE Izvestaj
ADD CONSTRAINT FK_IzvestajPacijent
FOREIGN KEY (IDPacijenta) REFERENCES Korisnik(ID);

ALTER TABLE Izvestaj
ADD CONSTRAINT FK_IzvestajLekar
FOREIGN KEY (IDLekara) REFERENCES Korisnik(ID);

ALTER TABLE Uput
ADD CONSTRAINT PK_Uput PRIMARY KEY (ID);

ALTER TABLE Uput
ADD CONSTRAINT FK_UputPacijent
FOREIGN KEY (IDPacijenta) REFERENCES Korisnik(ID);

ALTER TABLE Uput
ADD CONSTRAINT FK_UputLekarOd
FOREIGN KEY (IDLekaraOd) REFERENCES Korisnik(ID);

ALTER TABLE Uput
ADD CONSTRAINT FK_UputLekarKome
FOREIGN KEY (IDLekaraKome) REFERENCES Korisnik(ID);

ALTER TABLE Uput
ADD CONSTRAINT FK_UputOdeljenje
FOREIGN KEY (IDOdeljenja) REFERENCES Odeljenje(ID);

--ALTER TABLE RadnoVremeLekara
--ADD CONSTRAINT PK_RadnoVremeLekara PRIMARY KEY (ID);

--ALTER TABLE RadnoVremeLekara
--ADD CONSTRAINT FK_RadnoVremeLekaraLekar
--FOREIGN KEY (IDLekara) REFERENCES Korisnik(ID);

ALTER TABLE ZakazivanjePregleda
ADD CONSTRAINT PK_ZakazivanjePregleda PRIMARY KEY (ID);

ALTER TABLE ZakazivanjePregleda
ADD CONSTRAINT FK_ZakazivanjePregledaPacijent
FOREIGN KEY (IDPacijenta) REFERENCES Korisnik(ID);

ALTER TABLE ZakazivanjePregleda
ADD CONSTRAINT FK_ZakazivanjePregledaLekar
FOREIGN KEY (IDLekara) REFERENCES Korisnik(ID);

ALTER TABLE ONamaPodaci
ADD CONSTRAINT PK_ONamaPodaci PRIMARY KEY (ID);

ALTER TABLE Vesti
ADD CONSTRAINT PK_Vesti PRIMARY KEY (ID);