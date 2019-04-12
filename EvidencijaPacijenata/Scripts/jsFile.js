function proveriLogin() {
    var korisnicko_ime = document.forms.forma.KorisnickoIme.value;
    var lozinka = document.forms.forma.Lozinka.value;
    var uzorakKorisnicko_ime = /^[a-zA-Z]\w+([\.-]\w)*\w{6,}$/;
    var uzorakBrojZK = /^\d{11}$/;
    if (!uzorakBrojZK.test(korisnicko_ime) && !uzorakKorisnicko_ime.test(korisnicko_ime)) {
        alert("Korisničko ime (Broj zdravstvene knjižice) nije u dobrom formatu!");
        document.forms.forma.KorisnickoIme.focus();
        document.forms.forma.KorisnickoIme.style.backgroundColor = "#ff9999";
        return false;
    }
    if (lozinka.length < 8 || !/[A-Z]/.test(lozinka) || !/[a-z]/.test(lozinka) || !/[0-9]/.test(lozinka) || /[^a-zA-Z0-9]/.test(lozinka)) {
        alert("Lozinka nije u dobrom formatu!");
        document.forms.forma.Lozinka.focus();
        document.forms.forma.Lozinka.style.backgroundColor = "#ff9999";
        return false;
    }
    return true;
}

function proveriRegistracijuPacijenta() {
    /*var Ime = $("#Ime").text();
    var Prezime = $("#Prezime").text();
    var KorisnickoIme = $("#KorisnickoIme").text();*/
    var Lozinka = $("#lozinka1").text();
    var Lozinka2 = $("#lozinka2").text();
    /*var JMBG = $("#JMBG").text();
    var NosilacOsiguranja = $("#NosilacOsiguranja").text();
    var SrodstvoSaNosiocem = $("#SrodstvoSaNosiocem").text();
    var KrvnaGrupa = $("#KrvnaGrupa").text();
    var Pol = $("#Pol").text();
    var Adresa = $("#Adresa").text();
    var Telefon = $("#Telefon").text();
    var Email = $("#Email").text();*/

    /*if (!(Ime != "" && Prezime != "" && KorisnickoIme != "" && Lozinka != "" && Lozinka2 != "" && JMBG != "" && NosilacOsiguranja != ""
        && SrodstvoSaNosiocem != "" && KrvnaGrupa != "" && Pol != "" && Adresa != "" && Telefon != "" && Email != "")) {
        alert("Sva polja moraju biti popunjena!");
        return false;
    }*/
    if (Lozinka != Lozinka2) {
        alert("Ponovljena lozinka nije ista!");
        return false;
    }
    return true;
}