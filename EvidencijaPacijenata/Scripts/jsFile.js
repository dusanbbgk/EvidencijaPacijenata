function proveriLoginPacijent() {
    var broj_zk = document.forms.forma.KorisnickoIme.value;
    var lozinka_pacijent = document.forms.forma.Lozinka.value;
    var uzorakBrojZK = /^\d{11}$/;
    if (!uzorakBrojZK.test(broj_zk)) {
        alert("Broj zdravstvene knjižice nije u dobrom formatu!");
        document.forms.forma.KorisnickoIme.focus();
        document.forms.forma.KorisnickoIme.style.backgroundColor = "#ff9999";
        return false;
    }
    if (lozinka_pacijent.length < 8 || !/[A-Z]/.test(lozinka_pacijent) || !/[a-z]/.test(lozinka_pacijent) || !/[0-9]/.test(lozinka_pacijent) || /[^a-zA-Z0-9]/.test(lozinka_pacijent)) {
        alert("Lozinka nije u dobrom formatu!");
        document.forms.forma.Lozinka.focus();
        document.forms.forma.Lozinka.style.backgroundColor = "#ff9999";
        return false;
    }
    return true;
}
function proveriLoginLekar() {
    var korisnicko_ime = document.forms.forma2.KorisnickoIme.value;
    var lozinka_lekar = document.forms.forma2.Lozinka.value;
    var uzorakKorisnicko_ime = /^[a-zA-Z]\w+([\.-]\w)*\w{6,}$/;
    if (!uzorakKorisnicko_ime.test(korisnicko_ime)) {
        alert("Korisničko ime nije u dobrom formatu!");
        document.forms.forma2.KorisnickoIme.focus();
        document.forms.forma2.KorisnickoIme.style.backgroundColor = "#ff9999";
        return false;
    }
    if (lozinka_lekar.length < 8 || !/[A-Z]/.test(lozinka_lekar) || !/[a-z]/.test(lozinka_lekar) || !/[0-9]/.test(lozinka_lekar) || /[^a-zA-Z0-9]/.test(lozinka_lekar)) {
        alert("Lozinka nije u dobrom formatu!");
        document.forms.forma2.Lozinka.focus();
        document.forms.forma2.Lozinka.style.backgroundColor = "#ff9999";
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

function resetPassword() {
    
    var KorisnickoIme = $("#korisnickoIme").val();
    var email = $("#email").val();
    var Lozinka = $("#lozinka").val();
    var Lozinka2 = document.getElementById("lozinka2").value;
    if (KorisnickoIme == "" || email == "" || Lozinka == "" || Lozinka2 == "") {
        alert("Morate popuniti sva polja!");
        return false;
    }
    if (Lozinka != Lozinka2) {
        alert("Ponovljena lozinka nije ista!");
        return false;
    }
    return true;
}