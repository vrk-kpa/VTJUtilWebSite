using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Luokat
/// </summary>
public class HenkilonTiedot
{
    private int henkiloNro;

    public int HenkiloNro
    {
        get { return henkiloNro; }
        set { henkiloNro = value; }
    }
    private string nykyinenSukunimi;

    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    private string viimNaimattomanaSukunimi;

    public string ViimNaimattomanaSukunimi
    {
        get { return viimNaimattomanaSukunimi; }
        set { viimNaimattomanaSukunimi = value; }
    }
    private string nykyinenEtunimi;

    public string NykyinenEtunimi
    {
        get { return nykyinenEtunimi; }
        set { nykyinenEtunimi = value; }
    }
    private string moneskoliitto;

    public string Moneskoliitto
    {
        get { return moneskoliitto; }
        set { moneskoliitto = value; }
    }

    private string hetu;

    public string Hetu
    {
        get { return hetu; }
        set { hetu = value; }
    }
    private string siviilisaatykoodi;

    public string Siviilisaatykoodi
    {
        get { return siviilisaatykoodi; }
        set { siviilisaatykoodi = value; }
    }
    private string siviilisaaty;

    public string Siviilisaaty
    {
        get { return siviilisaaty; }
        set { siviilisaaty = value; }
    }
    private string kansalaisuuskoodi;

    public string Kansalaisuuskoodi
    {
        get { return kansalaisuuskoodi; }
        set { kansalaisuuskoodi = value; }
    }
    private string kansalaisuus;

    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }
    private string maistraatti;

    public string Maistraatti
    {
        get { return maistraatti; }
        set { maistraatti = value; }
    }
    private string kotikunta;

    public string Kotikunta
    {
        get { return kotikunta; }
        set { kotikunta = value; }
    }
    private string kotikuntakoodi;

    public string Kotikuntakoodi
    {
        get { return kotikuntakoodi; }
        set { kotikuntakoodi = value; }
    }
    private string rippikoulu;

    public string Rippikoulu
    {
        get { return rippikoulu; }
        set { rippikoulu = value; }
    }
    private string uskontokunta;

    public string Uskontokunta
    {
        get { return uskontokunta; }
        set { uskontokunta = value; }
    }

    private string asuinvaltio = "";

    public string Asuinvaltio
    {
        get { return asuinvaltio; }
        set { asuinvaltio = value; }
    }

    private string syntymaaika = "";

    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }

    private string syntymapaikka = "";

    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }

    private string syntymavaltio = "";

    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }

}


public class VirkailijanTiedot
{
    private string tutkintaAnnettu;

    public string TutkintaAnnettu
    {
        get { return tutkintaAnnettu; }
        set { tutkintaAnnettu = value; }
    }

    private string tutkintaPaiva;

    public string TutkintaPaiva
    {
        get { return tutkintaPaiva; }
        set { tutkintaPaiva = value; }
    }
    private string tutkinnanNumero;

    public string TutkinnanNumero
    {
        get { return tutkinnanNumero; }
        set { tutkinnanNumero = value; }
    }
    private string ulkomKihlEsteetTutkValtio;

    public string UlkomKihlEsteetTutkValtio
    {
        get { return ulkomKihlEsteetTutkValtio; }
        set { ulkomKihlEsteetTutkValtio = value; }
    }
    private string allekirjoitus_Paikka;

    public string Allekirjoitus_Paikka
    {
        get { return allekirjoitus_Paikka; }
        set { allekirjoitus_Paikka = value; }
    }
    private string allekirjoitus_Aika;

    public string Allekirjoitus_Aika
    {
        get { return allekirjoitus_Aika; }
        set { allekirjoitus_Aika = value; }
    }
    private string allekirjoitus_Nimi;

    public string Allekirjoitus_Nimi
    {
        get { return allekirjoitus_Nimi; }
        set { allekirjoitus_Nimi = value; }
    }
    private string allekirjoitus_VirkaAsema;

    public string Allekirjoitus_VirkaAsema
    {
        get { return allekirjoitus_VirkaAsema; }
        set { allekirjoitus_VirkaAsema = value; }
    }
    private string allekirjoitus_ABtayttanytViranomainen;

    public string Allekirjoitus_ABtayttanytViranomainen
    {
        get { return allekirjoitus_ABtayttanytViranomainen; }
        set { allekirjoitus_ABtayttanytViranomainen = value; }
    }

    private string allekirjoitus_lisatiedot;

    public string Allekirjoitus_Lisatiedot
    {
        get { return allekirjoitus_lisatiedot; }
        set { allekirjoitus_lisatiedot = value; }
    }

}

[Serializable]
public class UlkomaisenLisatiedot
{
    //public string KansalaisuusKoodi="";
    //public string KansalaisuusNimi = "";
    private string aidinkieliKoodi = "";

    public string AidinkieliKoodi
    {
        get { return aidinkieliKoodi; }
        set { aidinkieliKoodi = value; }
    }
    private string aidinkieliNimi = "";

    public string AidinkieliNimi
    {
        get { return aidinkieliNimi; }
        set { aidinkieliNimi = value; }
    }
    private string asuinmaaKoodi = "";

    public string AsuinmaaKoodi
    {
        get { return asuinmaaKoodi; }
        set { asuinmaaKoodi = value; }
    }
    private string asuinmaaNimi = "";

    public string AsuinmaaNimi
    {
        get { return asuinmaaNimi; }
        set { asuinmaaNimi = value; }
    }
}

[Serializable]
public class SukunimivalinnanTiedot
{
    private bool lainmukainenAK;
    public bool LainmukainenAK
    {
        get { return lainmukainenAK; }
        set { lainmukainenAK = value; }
    }

    private bool lainmukainenAE;
    public bool LainmukainenAE
    {
        get { return lainmukainenAE; }
        set { lainmukainenAE = value; }
    }

    private bool eiValtaaTutkiaA;
    public bool EiValtaaTutkiaA
    {
        get { return eiValtaaTutkiaA; }
        set { eiValtaaTutkiaA = value; }
    }

    private string puolisonASukunimi = "";
    public string PuolisonASukunimi
    {
        get { return puolisonASukunimi; }
        set { puolisonASukunimi = value; }
    }

    private bool lainmukainenBK;
    public bool LainmukainenBK
    {
        get { return lainmukainenBK; }
        set { lainmukainenBK = value; }
    }

    private bool lainmukainenBE;
    public bool LainmukainenBE
    {
        get { return lainmukainenBE; }
        set { lainmukainenBE = value; }
    }

    private bool eiValtaaTutkiaB;
    public bool EiValtaaTutkiaB
    {
        get { return eiValtaaTutkiaB; }
        set { eiValtaaTutkiaB = value; }
    }

    private string puolisonBSukunimi = "";
    public string PuolisonBSukunimi
    {
        get { return puolisonBSukunimi; }
        set { puolisonBSukunimi = value; }
    }
}
