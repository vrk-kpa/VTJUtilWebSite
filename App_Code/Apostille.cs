using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Apostille
/// EU-lomakkeen tiedot
/// Todistuksen yhteyteen tulostettavan käännöslomakkeen tiedot
/// </summary>
public class ApostilleTiedot
{
    private string todistus;
    public string Todistus
    {
        get { return todistus; }
        set { todistus = value; }
    }

    private string kohdemaa;
    public string Kohdemaa
    {
        get { return kohdemaa; }
        set { kohdemaa = value; }
    }

    private string tulostuskieli;
    public string Tulostuskieli
    {
        get { return tulostuskieli; }
        set { tulostuskieli = value; }
    }

    private string henkilotunnus;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }

    private string syntymaaika;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }

    private string kuolinpaiva;
    public string Kuolinpaiva
    {
        get { return kuolinpaiva; }
        set { kuolinpaiva = value; }
    }
    
    private string sukupuoli;
    public string Sukupuoli
    {
        get { return sukupuoli; }
        set { sukupuoli = value; }
    }

    private string nykyinenSukunimi;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }

    private string nykyisetEtunimet;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }

    private string kutsumanimi;
    public string Kutsumanimi
    {
        get { return kutsumanimi; }
        set { kutsumanimi = value; }
    }

    private string siviilisaaty;
    public string Siviilisaaty
    {
        get { return siviilisaaty; }
        set { siviilisaaty = value; }
    }

    private string siviilisaatyPvm;
    public string SiviilisaatyPvm
    {
        get { return siviilisaatyPvm; }
        set { siviilisaatyPvm = value; }
    }

    private string syntymakotikunta;
    public string Syntymakotikunta
    {
        get { return syntymakotikunta; }
        set { syntymakotikunta = value; }
    }

    private string syntymapaikka;
    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }

    private string syntymavaltio;
    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }

    private string kansalaisuus;
    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }

    private string kotikunta;
    public string Kotikunta
    {
        get { return kotikunta; }
        set { kotikunta = value; }
    }

    private string elossaolotieto;
    public string Elossaolotieto
    {
        get { return elossaolotieto; }
        set { elossaolotieto = value; }
    }

    private string rekisterinpitaja;
    public string Rekisterinpitaja
    {
        get { return rekisterinpitaja; }
        set { rekisterinpitaja = value; }
    }

    private string allekirjoitus;
    public string Allekirjoitus
    {
        get { return allekirjoitus; }
        set { allekirjoitus = value; }
    }

    private string virkamiehenNimi;
    public string VirkamiehenNimi
    {
        get { return virkamiehenNimi; }
        set { virkamiehenNimi = value; }
    }

    private string virkanimike;
    public string Virkanimike
    {
        get { return virkanimike; }
        set { virkanimike = value; }
    }

    private string viranomainenNimi;
    public string ViranomainenNimi
    {
        get { return viranomainenNimi; }
        set { viranomainenNimi = value; }
    }

    private string viranomainenLahiosoite;
    public string ViranomainenLahiosoite
    {
        get { return viranomainenLahiosoite; }
        set { viranomainenLahiosoite = value; }
    }

    private string viranomainenPostiosoite;
    public string ViranomainenPostiosoite
    {
        get { return viranomainenPostiosoite; }
        set { viranomainenPostiosoite = value; }
    }

    private string viranomainenPostitmp;
    public string ViranomainenPostitmp
    {
        get { return viranomainenPostitmp; }
        set { viranomainenPostitmp = value; }
    }

    private string viranomainenPuhelin;
    public string ViranomainenPuhelin
    {
        get { return viranomainenPuhelin; }
        set { viranomainenPuhelin = value; }
    }

    private string antamispaiva;
    public string Antamispaiva
    {
        get { return antamispaiva; }
        set { antamispaiva = value; }
    }

    private string antamispaikka;
    public string Antamispaikka
    {
        get { return antamispaikka; }
        set { antamispaikka = value; }
    }

    private string todistusteksti;
    public string Todistusteksti
    {
        get { return todistusteksti; }
        set { todistusteksti = value; }
    }

    private string luovutusteksti;
    public string Luovutusteksti
    {
        get { return luovutusteksti; }
        set { luovutusteksti = value; }
    }


    public List<Osoite> vakinaisetOsoitteet { get; set; }
    public List<Osoite> tilapaisetOsoitteet { get; set; }
    public List<Taulukko> entisetSukunimet { get; set; }
    public List<Taulukko> entisetSukunimimetYli15v { get; set; }
    public List<Taulukko> entisetEtunimet { get; set; }
    public List<Taulukko> entisetEtunimetYli15v { get; set; }
    public List<Taulukko> korjatutSukunimet { get; set; }
    public List<Taulukko> korjatutEtunimet { get; set; }
    public List<Osoite> entisetVakinaisetOsoitteet { get; set; }
    public List<Osoite> entisetTilapaisetOsoitteet { get; set; }
    public List<Osoite> postiosoitteet { get; set; }
    public List<Taulukko> entisetKotikunnat { get; set; }
    public List<Taulukko> entisetAvioliitot { get; set; }
    public List<Vanhempi> vanhemmat { get; set; }
    public List<Avioliitto> avioliitot { get; set; }

}

public class Taulukko
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string sarake1;
    public string Sarake1
    {
        get { return sarake1; }
        set { sarake1 = value; }
    }

    private string sarake2;
    public string Sarake2
    {
        get { return sarake2; }
        set { sarake2 = value; }
    }

}

public class Osoite
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string lahiosoite;
    public string Lahiosoite
    {
        get { return lahiosoite; }
        set { lahiosoite = value; }
    }

    private string postitoimipaikka;
    public string Postitoimipaikka
    {
        get { return postitoimipaikka; }
        set { postitoimipaikka = value; }
    }

    private string voimassaoloaika;
    public string Voimassaoloaika
    {
        get { return voimassaoloaika; }
        set { voimassaoloaika = value; }
    }

    private string alkupaiva;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }
}

public class Avioliitto
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string alkupaiva;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }

    private string jarjestysnumero;
    public string Jarjestysnumero
    {
        get { return jarjestysnumero; }
        set { jarjestysnumero = value; }
    }

    private string henkilotunnus;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }

    private string syntymaaika;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }

    private string nykyinenSukunimi;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }

    private string nykyisetEtunimet;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }

    private string entinenSukunimiYli15v;
    public string EntinenSukunimiYli15v
    {
        get { return entinenSukunimiYli15v; }
        set { entinenSukunimiYli15v = value; }
    }
}

public class Vanhempi
{
    private string isaaiti;
    public string IsaAiti
    {
        get { return isaaiti; }
        set { isaaiti = value; }
    }

    private string henkilotunnus;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }

    private string syntymaaika;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }

    private string nykyinenSukunimi;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }

    private string entinenSukunimiYli15v;
    public string EntinenSukunimiYli15v
    {
        get { return entinenSukunimiYli15v; }
        set { entinenSukunimiYli15v = value; }
    }

    private string nykyisetEtunimet;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }

    private string kutsumanimi;
    public string Kutsumanimi
    {
        get { return kutsumanimi; }
        set { kutsumanimi = value; }
    }

    private string syntymakotikunta;
    public string Syntymakotikunta
    {
        get { return syntymakotikunta; }
        set { syntymakotikunta = value; }
    }

    private string syntymapaikka;
    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }

    private string syntymavaltio;
    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }

    private string kansalaisuus;
    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }

}
