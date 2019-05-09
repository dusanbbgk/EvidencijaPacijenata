use DBZUstanovaBeta

CREATE PROCEDURE slobodniTermini @lekarId INT, @datum DATE, @vreme TIME
AS
SELECT * FROM ZakazivanjePregleda
WHERE IDLekara=@lekarId AND VremePregleda=@vreme AND DatumPregleda=@datum 
GO

CREATE PROCEDURE pretragaPacijenata @pretraga NVARCHAR(30), @IDLekara INT, @Uslov INT
AS
BEGIN
IF @Uslov = 1
	BEGIN
		SELECT DISTINCT Korisnik.ID, Korisnik.Ime, Korisnik.Prezime, Pacijent.JMBG FROM Pacijent
		INNER JOIN Korisnik ON Pacijent.ID = Korisnik.ID
		INNER JOIN Lekar ON Pacijent.IDOdeljenja = Lekar.IDOdeljenja
		WHERE Lekar.ID = @IDLekara AND (Korisnik.Ime Like @pretraga+'%' OR Korisnik.Prezime Like @pretraga+'%' OR Pacijent.JMBG Like @pretraga+'%')
	END
ELSE IF @Uslov = 0
	BEGIN
		SELECT DISTINCT Korisnik.ID, Korisnik.Ime, Korisnik.Prezime, Pacijent.JMBG FROM Pacijent
		INNER JOIN Korisnik ON Pacijent.ID = Korisnik.ID
		INNER JOIN Ustanova ON Pacijent.IDUstanove = Ustanova.ID
		INNER JOIN Odeljenje ON Ustanova.ID = Odeljenje.IDUstanove
		INNER JOIN Lekar ON Odeljenje.ID = Lekar.IDOdeljenja
		WHERE Lekar.ID = @IDLekara AND (Korisnik.Ime Like @pretraga+'%' OR Korisnik.Prezime Like @pretraga+'%' OR Pacijent.JMBG Like @pretraga+'%')
	END
END