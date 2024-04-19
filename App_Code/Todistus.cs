using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Todistus
/// </summary>

public static class TodistusTuote
{
    // Rajapintatuotteiden Tuote_id:t
    public const string SYNTYMATODISTUS_VAKIOLOMAKE = "1353a410-eb73-4f57-8a2f-ae01040ddfec";
    public const string SYNTYMATODISTUS2_VAKIOLOMAKE = "64c36d58-5ea8-47a3-852f-a1dd71d2879b";
    public const string ELOSSAOLOTODISTUS_VAKIOLOMAKE = "da88cae1-1107-427c-a41d-5203fc9c9db9";
    public const string AVIOLIITTOTODISTUS_VAKIOLOMAKE = "f99f6373-2c08-48c2-8361-b88f471bebc8";
    public const string AVIOLIITTOTODISTUS2_VAKIOLOMAKE = "58669bfd-9c33-43ff-acdf-da63536332cc";
    public const string SIVIILISAATYTODISTUS_VAKIOLOMAKE = "c512f699-652f-46f5-8921-0d1a04f6076f";
    public const string ASUINPAIKKATODISTUS_VAKIOLOMAKE = "3e54b001-2f1b-48a5-868c-fea04726bc72";
    public const string KUOLINTODISTUS_VAKIOLOMAKE = "a1c93bbc-8c19-4d2f-a666-86fa9231850b";
    public const string AVIOLIITTOKELPOISUUSTODISTUS_VAKIOLOMAKE = "a1c6e880-75c1-4e51-abf6-5afe8dbaa3f8";
}

public static class Vakiolomake
{
    /// <summary>
    /// "FI_Birth"
    /// </summary>
    public const string SYNTYMATODISTUS = "FI_Birth";

    /// "FI_Birth2"
    /// </summary>
    public const string SYNTYMATODISTUS2 = "FI_Birth2";

    /// <summary>
    /// "FI_Life"
    /// </summary>    
    public const string ELOSSAOLOTODISTUS = "FI_Life";

    /// <summary>
    /// "FI_Marriage"
    /// </summary>
    public const string AVIOLIITTOTODISTUS = "FI_Marriage";

    /// <summary>
    /// "FI_MaritalStatus"
    /// </summary>
    public const string SIVIILISAATYTODISTUS_AVIOLIITTO = "FI_MaritalStatus";

    /// <summary>
    /// "FI_RegisteredPartnershipStatus"
    /// </summary>
    public const string SIVIILISAATYTODISTUS_REKPA = "FI_RegisteredPartnershipStatus";

    /// <summary>
    /// "FI_DomicileAndOrResidence"
    /// </summary>
    public const string ASUINPAIKKATODISTUS = "FI_DomicileAndOrResidence";

    /// <summary>
    /// "FI_Death"
    /// </summary>    
    public const string KUOLINTODISTUS = "FI_Death";

    /// <summary>
    /// "FI_CapacityToMarry"
    /// </summary>    
    public const string AVIOLIITTOKELPOISUUSTODISTUS = "FI_CapacityToMarry";
}

public static class Pdflomake
{
    /// <summary>
    /// "Avioliittokelpoisuus"
    /// </summary>    
    public const string AVIOLIITTOKELPOISUUS = "AET_tutkintatodistusUlkom";
}

public class OtteenTiedot
{
    private string ote;
    public string Ote
    {
        get { return ote; }
        set { ote = value; }
    }

    private string tulostuskieli;
    public string Tulostuskieli
    {
        get { return tulostuskieli; }
        set { tulostuskieli = value; }
    }

    private string kohdemaa;
    public string Kohdemaa
    {
        get { return kohdemaa; }
        set { kohdemaa = value; }
    }

    private string kayttotarkoitus;
    private string kayttotarkoitus_otsikko;
    public string Kayttotarkoitus
    {
        get { return kayttotarkoitus; }
        set { kayttotarkoitus = value; }
    }
    public string Kayttotarkoitus_otsikko
    {
        get { return kayttotarkoitus_otsikko; }
        set { kayttotarkoitus_otsikko = value; }
    }

    private Guid kyseltyTuote;
    public Guid KyseltyTuote
    {
        get { return kyseltyTuote; }
        set { kyseltyTuote = value; }
    }

    private string kyseltyHenkilotunnus;
    public string KyseltyHenkilotunnus
    {
        get { return kyseltyHenkilotunnus; }
        set { kyseltyHenkilotunnus = value; }
    }

    private string henkilotunnus;
    private string henkilotunnus_otsikko;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }
    public string Henkilotunnus_otsikko
    {
        get { return henkilotunnus_otsikko; }
        set { henkilotunnus_otsikko = value; }
    }

    private string syntymaaika;
    private string syntymaaika_otsikko;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }
    public string Syntymaaika_otsikko
    {
        get { return syntymaaika_otsikko; }
        set { syntymaaika_otsikko = value; }
    }

    private string kuolinpaiva;
    private string kuolinpaiva_otsikko;
    public string Kuolinpaiva
    {
        get { return kuolinpaiva; }
        set { kuolinpaiva = value; }
    }
    public string Kuolinpaiva_otsikko
    {
        get { return kuolinpaiva_otsikko; }
        set { kuolinpaiva_otsikko = value; }
    }

    private string sukupuoli;
    private string sukupuoli_otsikko;
    public string Sukupuoli
    {
        get { return sukupuoli; }
        set { sukupuoli = value; }
    }
    public string Sukupuoli_otsikko
    {
        get { return sukupuoli_otsikko; }
        set { sukupuoli_otsikko = value; }
    }

    private string nykyinenSukunimi;
    private string nykyinenSukunimi_otsikko;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    public string NykyinenSukunimi_otsikko
    {
        get { return nykyinenSukunimi_otsikko; }
        set { nykyinenSukunimi_otsikko = value; }
    }

    private string nykyisetEtunimet;
    private string nykyisetEtunimet_otsikko;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }
    public string NykyisetEtunimet_otsikko
    {
        get { return nykyisetEtunimet_otsikko; }
        set { nykyisetEtunimet_otsikko = value; }
    }

    private string kutsumanimi;
    private string kutsumanimi_otsikko;
    public string Kutsumanimi
    {
        get { return kutsumanimi; }
        set { kutsumanimi = value; }
    }
    public string Kutsumanimi_otsikko
    {
        get { return kutsumanimi_otsikko; }
        set { kutsumanimi_otsikko = value; }
    }

    private string siviilisaaty;
    private string siviilisaaty_otsikko;
    public string Siviilisaaty
    {
        get { return siviilisaaty; }
        set { siviilisaaty = value; }
    }
    public string Siviilisaaty_otsikko
    {
        get { return siviilisaaty_otsikko; }
        set { siviilisaaty_otsikko = value; }
    }

    private string siviilisaatyPvm;
    private string siviilisaatyPvm_otsikko;
    public string SiviilisaatyPvm
    {
        get { return siviilisaatyPvm; }
        set { siviilisaatyPvm = value; }
    }
    public string SiviilisaatyPvm_otsikko
    {
        get { return siviilisaatyPvm_otsikko; }
        set { siviilisaatyPvm_otsikko = value; }
    }

    private string jarjestysnumero;
    private string jarjestysnumero_otsikko;
    public string Jarjestysnumero
    {
        get { return jarjestysnumero; }
        set { jarjestysnumero = value; }
    }
    public string Jarjestysnumero_otsikko
    {
        get { return jarjestysnumero_otsikko; }
        set { jarjestysnumero_otsikko = value; }
    }

    private string syntymakotikunta;
    private string syntymakotikunta_otsikko;
    public string Syntymakotikunta
    {
        get { return syntymakotikunta; }
        set { syntymakotikunta = value; }
    }
    public string Syntymakotikunta_otsikko
    {
        get { return syntymakotikunta_otsikko; }
        set { syntymakotikunta_otsikko = value; }
    }

    private string syntymapaikka;
    private string syntymapaikka_otsikko;
    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }
    public string Syntymapaikka_otsikko
    {
        get { return syntymapaikka_otsikko; }
        set { syntymapaikka_otsikko = value; }
    }

    private string syntymavaltio;
    private string syntymavaltio_otsikko;
    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }
    public string Syntymavaltio_otsikko
    {
        get { return syntymavaltio_otsikko; }
        set { syntymavaltio_otsikko = value; }
    }

    private string asuinvaltio;
    private string asuinvaltio_otsikko;
    public string Asuinvaltio
    {
        get { return asuinvaltio; }
        set { asuinvaltio = value; }
    }
    public string Asuinvaltio_otsikko
    {
        get { return asuinvaltio_otsikko; }
        set { asuinvaltio_otsikko = value; }
    }

    private string kansalaisuus;
    private string kansalaisuus_otsikko;
    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }
    public string Kansalaisuus_otsikko
    {
        get { return kansalaisuus_otsikko; }
        set { kansalaisuus_otsikko = value; }
    }

    private string kotikunta;
    private string kotikunta_otsikko;
    public string Kotikunta
    {
        get { return kotikunta; }
        set { kotikunta = value; }
    }
    public string Kotikunta_otsikko
    {
        get { return kotikunta_otsikko; }
        set { kotikunta_otsikko = value; }
    }

    private string turvakieltoteksti;
    public string Turvakieltoteksti
    {
        get { return turvakieltoteksti; }
        set { turvakieltoteksti = value; }
    }

    private string elossaolotieto;
    private string elossaolotieto_otsikko;
    public string Elossaolotieto
    {
        get { return elossaolotieto; }
        set { elossaolotieto = value; }
    }
    public string Elossaolotieto_otsikko
    {
        get { return elossaolotieto_otsikko; }
        set { elossaolotieto_otsikko = value; }
    }

    private string rekisterinpitaja;
    private string rekisterinpitaja_otsikko;
    public string Rekisterinpitaja
    {
        get { return rekisterinpitaja; }
        set { rekisterinpitaja = value; }
    }

    public string Rekisterinpitaja_otsikko
    {
        get { return rekisterinpitaja_otsikko; }
        set { rekisterinpitaja_otsikko = value; }
    }

    private string allekirjoitus;
    private string allekirjoitus_otsikko;
    public string Allekirjoitus
    {
        get { return allekirjoitus; }
        set { allekirjoitus = value; }
    }
    public string Allekirjoitus_otsikko
    {
        get { return allekirjoitus_otsikko; }
        set { allekirjoitus_otsikko = value; }
    }

    private string nimenselvennys;
    private string nimenselvennys_otsikko;
    public string Nimenselvennys
    {
        get { return nimenselvennys; }
        set { nimenselvennys = value; }
    }
    public string Nimenselvennys_otsikko
    {
        get { return nimenselvennys_otsikko; }
        set { nimenselvennys_otsikko = value; }
    }

    private string aika;
    private string aika_otsikko;
    public string Aika
    {
        get { return aika; }
        set { aika = value; }
    }
    public string Aika_otsikko
    {
        get { return aika_otsikko; }
        set { aika_otsikko = value; }
    }

    private string vtjPvm;
    private string vtjPvm_otsikko;
    public string VtjPvm
    {
        get { return vtjPvm; }
        set { vtjPvm = value; }
    }
    public string VtjPvm_otsikko
    {
        get { return vtjPvm_otsikko; }
        set { vtjPvm_otsikko = value; }
    }

    private string vtjKlo;
    private string vtjKlo_otsikko;
    public string VtjKlo
    {
        get { return vtjKlo; }
        set { vtjKlo = value; }
    }
    public string VtjKlo_otsikko
    {
        get { return vtjKlo_otsikko; }
        set { vtjKlo_otsikko = value; }
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

    private string tiiviste;
    private string tiiviste_otsikko;
    public string Tiiviste
    {
        get { return tiiviste; }
        set { tiiviste = value; }
    }
    public string Tiiviste_otsikko
    {
        get { return tiiviste_otsikko; }
        set { tiiviste_otsikko = value; }
    }


    public List<TaulukkotietoOsoite> vakinaisetOsoitteet { get; set; }
    public List<TaulukkotietoOsoite> tilapaisetOsoitteet { get; set; }
    public List<Taulukkotieto> entisetSukunimet { get; set; }
    public List<Taulukkotieto> entisetEtunimet { get; set; }
    public List<Taulukkotieto> korjatutSukunimet { get; set; }
    public List<Taulukkotieto> korjatutEtunimet { get; set; }
    public List<TaulukkotietoOsoite> entisetVakinaisetOsoitteet { get; set; }
    public List<TaulukkotietoOsoite> entisetTilapaisetOsoitteet { get; set; }
    public List<Taulukkotieto> entisetKotikunnat { get; set; }
    public List<Taulukkotieto> entisetAvioliitot { get; set; }
    public List<TaulukkotietoVanhempi> vanhemmat { get; set; }
    public List<TaulukkotietoHuoltaja> huoltajat { get; set; }
    public List<TaulukkotietoAvioliitto> avioliitot { get; set; }
    public List<TaulukkotietoEdunvalvonta> edunvalvonnat { get; set; }
    public List<TaulukkotietoEdunvalvontavaltuutus> edunvalvontavaltuutukset { get; set; }

    public Henkilotiedot HenkiloB { get; set; }

    public Allekirjoitustiedot allekirjoitustiedot { get; set; }

}

public class Taulukkotieto
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

public class TaulukkotietoOsoite
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string lahiosoite;
    private string lahiosoite_otsikko;
    public string Lahiosoite
    {
        get { return lahiosoite; }
        set { lahiosoite = value; }
    }
    public string Lahiosoite_otsikko
    {
        get { return lahiosoite_otsikko; }
        set { lahiosoite_otsikko = value; }
    }

    private string postitoimipaikka;
    private string postitoimipaikka_otsikko;
    public string Postitoimipaikka
    {
        get { return postitoimipaikka; }
        set { postitoimipaikka = value; }
    }
    public string Postitoimipaikka_otsikko
    {
        get { return postitoimipaikka_otsikko; }
        set { postitoimipaikka_otsikko = value; }
    }

    private string voimassaoloaika;
    private string voimassaoloaika_otsikko;
    public string Voimassaoloaika
    {
        get { return voimassaoloaika; }
        set { voimassaoloaika = value; }
    }
    public string Voimassaoloaika_otsikko
    {
        get { return voimassaoloaika_otsikko; }
        set { voimassaoloaika_otsikko = value; }
    }

    private string alkupaiva;
    private string alkupaiva_otsikko;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }
    public string Alkupaiva_otsikko
    {
        get { return alkupaiva_otsikko; }
        set { alkupaiva_otsikko = value; }
    }

}

public class TaulukkotietoAvioliitto
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string alkupaiva;
    private string alkupaiva_otsikko;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }
    public string Alkupaiva_otsikko
    {
        get { return alkupaiva_otsikko; }
        set { alkupaiva_otsikko = value; }
    }

    private string jarjestysnumero;
    private string jarjestysnumero_otsikko;
    public string Jarjestysnumero
    {
        get { return jarjestysnumero; }
        set { jarjestysnumero = value; }
    }
    public string Jarjestysnumero_otsikko
    {
        get { return jarjestysnumero_otsikko; }
        set { jarjestysnumero_otsikko = value; }
    }

    private string henkilotunnus;
    private string henkilotunnus_otsikko;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }
    public string Henkilotunnus_otsikko
    {
        get { return henkilotunnus_otsikko; }
        set { henkilotunnus_otsikko = value; }
    }

    private string syntymaaika;
    private string syntymaaika_otsikko;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }
    public string Syntymaaika_otsikko
    {
        get { return syntymaaika_otsikko; }
        set { syntymaaika_otsikko = value; }
    }

    private string nykyinenSukunimi;
    private string nykyinenSukunimi_otsikko;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    public string NykyinenSukunimi_otsikko
    {
        get { return nykyinenSukunimi_otsikko; }
        set { nykyinenSukunimi_otsikko = value; }
    }

    private string nykyisetEtunimet;
    private string nykyisetEtunimet_otsikko;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }
    public string NykyisetEtunimet_otsikko
    {
        get { return nykyisetEtunimet_otsikko; }
        set { nykyisetEtunimet_otsikko = value; }
    }

    private string entinenSukunimi;
    private string entinenSukunimi_otsikko;
    public string EntinenSukunimi
    {
        get { return entinenSukunimi; }
        set { entinenSukunimi = value; }
    }
    public string EntinenSukunimi_otsikko
    {
        get { return entinenSukunimi_otsikko; }
        set { entinenSukunimi_otsikko = value; }
    }
}

public class TaulukkotietoVanhempi
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string isaaiti;
    private string isaaiti_otsikko;
    public string IsaAiti
    {
        get { return isaaiti; }
        set { isaaiti = value; }
    }
    public string IsaAiti_otsikko
    {
        get { return isaaiti_otsikko; }
        set { isaaiti_otsikko = value; }
    }

    private string henkilotunnus;
    private string henkilotunnus_otsikko;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }
    public string Henkilotunnus_otsikko
    {
        get { return henkilotunnus_otsikko; }
        set { henkilotunnus_otsikko = value; }
    }

    private string syntymaaika;
    private string syntymaaika_otsikko;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }
    public string Syntymaaika_otsikko
    {
        get { return syntymaaika_otsikko; }
        set { syntymaaika_otsikko = value; }
    }

    private string nykyinenSukunimi;
    private string nykyinenSukunimi_otsikko;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    public string NykyinenSukunimi_otsikko
    {
        get { return nykyinenSukunimi_otsikko; }
        set { nykyinenSukunimi_otsikko = value; }
    }

    private string nykyisetEtunimet;
    private string nykyisetEtunimet_otsikko;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }
    public string NykyisetEtunimet_otsikko
    {
        get { return nykyisetEtunimet_otsikko; }
        set { nykyisetEtunimet_otsikko = value; }
    }

    private string entinenSukunimi;
    private string entinenSukunimi_otsikko;
    public string EntinenSukunimi
    {
        get { return entinenSukunimi; }
        set { entinenSukunimi = value; }
    }
    public string EntinenSukunimi_otsikko
    {
        get { return entinenSukunimi_otsikko; }
        set { entinenSukunimi_otsikko = value; }
    }

    private string syntymakotikunta;
    private string syntymakotikunta_otsikko;
    public string Syntymakotikunta
    {
        get { return syntymakotikunta; }
        set { syntymakotikunta = value; }
    }
    public string Syntymakotikunta_otsikko
    {
        get { return syntymakotikunta_otsikko; }
        set { syntymakotikunta_otsikko = value; }
    }

    private string syntymapaikka;
    private string syntymapaikka_otsikko;
    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }
    public string Syntymapaikka_otsikko
    {
        get { return syntymapaikka_otsikko; }
        set { syntymapaikka_otsikko = value; }
    }

    private string syntymavaltio;
    private string syntymavaltio_otsikko;
    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }
    public string Syntymavaltio_otsikko
    {
        get { return syntymavaltio_otsikko; }
        set { syntymavaltio_otsikko = value; }
    }

    private string kansalaisuus;
    private string kansalaisuus_otsikko;
    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }
    public string Kansalaisuus_otsikko
    {
        get { return kansalaisuus_otsikko; }
        set { kansalaisuus_otsikko = value; }
    }

}

public class TaulukkotietoHuoltaja
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string isaaiti;
    private string isaaiti_otsikko;
    public string IsaAiti
    {
        get { return isaaiti; }
        set { isaaiti = value; }
    }
    public string IsaAiti_otsikko
    {
        get { return isaaiti_otsikko; }
        set { isaaiti_otsikko = value; }
    }

    private string henkilotunnus;
    private string henkilotunnus_otsikko;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }
    public string Henkilotunnus_otsikko
    {
        get { return henkilotunnus_otsikko; }
        set { henkilotunnus_otsikko = value; }
    }

    private string syntymaaika;
    private string syntymaaika_otsikko;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }
    public string Syntymaaika_otsikko
    {
        get { return syntymaaika_otsikko; }
        set { syntymaaika_otsikko = value; }
    }

    private string nykyinenSukunimi;
    private string nykyinenSukunimi_otsikko;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    public string NykyinenSukunimi_otsikko
    {
        get { return nykyinenSukunimi_otsikko; }
        set { nykyinenSukunimi_otsikko = value; }
    }

    private string nykyisetEtunimet;
    private string nykyisetEtunimet_otsikko;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }
    public string NykyisetEtunimet_otsikko
    {
        get { return nykyisetEtunimet_otsikko; }
        set { nykyisetEtunimet_otsikko = value; }
    }

}

public class TaulukkotietoEdunvalvonta
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string alkupaiva;
    private string alkupaiva_otsikko;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }
    public string Alkupaiva_otsikko
    {
        get { return alkupaiva_otsikko; }
        set { alkupaiva_otsikko = value; }
    }

    private string rajoitus;
    private string rajoitus_otsikko;
    public string Rajoitus
    {
        get { return rajoitus; }
        set { rajoitus = value; }
    }
    public string Rajoitus_otsikko
    {
        get { return rajoitus_otsikko; }
        set { rajoitus_otsikko = value; }
    }

    private string tehtavienJako;
    private string tehtavienJako_otsikko;
    public string TehtavienJako
    {
        get { return tehtavienJako; }
        set { tehtavienJako = value; }
    }
    public string TehtavienJako_otsikko
    {
        get { return tehtavienJako_otsikko; }
        set { tehtavienJako_otsikko = value; }
    }
}

public class TaulukkotietoEdunvalvontavaltuutus
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string alkupaiva;
    private string alkupaiva_otsikko;
    public string Alkupaiva
    {
        get { return alkupaiva; }
        set { alkupaiva = value; }
    }
    public string Alkupaiva_otsikko
    {
        get { return alkupaiva_otsikko; }
        set { alkupaiva_otsikko = value; }
    }

    private string tehtavienJako;
    private string tehtavienJako_otsikko;
    public string TehtavienJako
    {
        get { return tehtavienJako; }
        set { tehtavienJako = value; }
    }
    public string TehtavienJako_otsikko
    {
        get { return tehtavienJako_otsikko; }
        set { tehtavienJako_otsikko = value; }
    }
}

public class Henkilotiedot
{
    private string otsikko;
    public string Otsikko
    {
        get { return otsikko; }
        set { otsikko = value; }
    }

    private string henkilotunnus;
    private string henkilotunnus_otsikko;
    public string Henkilotunnus
    {
        get { return henkilotunnus; }
        set { henkilotunnus = value; }
    }
    public string Henkilotunnus_otsikko
    {
        get { return henkilotunnus_otsikko; }
        set { henkilotunnus_otsikko = value; }
    }

    private string syntymaaika;
    private string syntymaaika_otsikko;
    public string Syntymaaika
    {
        get { return syntymaaika; }
        set { syntymaaika = value; }
    }
    public string Syntymaaika_otsikko
    {
        get { return syntymaaika_otsikko; }
        set { syntymaaika_otsikko = value; }
    }

    private string nykyinenSukunimi;
    private string nykyinenSukunimi_otsikko;
    public string NykyinenSukunimi
    {
        get { return nykyinenSukunimi; }
        set { nykyinenSukunimi = value; }
    }
    public string NykyinenSukunimi_otsikko
    {
        get { return nykyinenSukunimi_otsikko; }
        set { nykyinenSukunimi_otsikko = value; }
    }

    private string nykyisetEtunimet;
    private string nykyisetEtunimet_otsikko;
    public string NykyisetEtunimet
    {
        get { return nykyisetEtunimet; }
        set { nykyisetEtunimet = value; }
    }
    public string NykyisetEtunimet_otsikko
    {
        get { return nykyisetEtunimet_otsikko; }
        set { nykyisetEtunimet_otsikko = value; }
    }

    private string syntymapaikka;
    private string syntymapaikka_otsikko;
    public string Syntymapaikka
    {
        get { return syntymapaikka; }
        set { syntymapaikka = value; }
    }
    public string Syntymapaikka_otsikko
    {
        get { return syntymapaikka_otsikko; }
        set { syntymapaikka_otsikko = value; }
    }

    private string syntymavaltio;
    private string syntymavaltio_otsikko;
    public string Syntymavaltio
    {
        get { return syntymavaltio; }
        set { syntymavaltio = value; }
    }
    public string Syntymavaltio_otsikko
    {
        get { return syntymavaltio_otsikko; }
        set { syntymavaltio_otsikko = value; }
    }

    private string asuinvaltio;
    private string asuinvaltio_otsikko;
    public string Asuinvaltio
    {
        get { return asuinvaltio; }
        set { asuinvaltio = value; }
    }
    public string Asuinvaltio_otsikko
    {
        get { return asuinvaltio_otsikko; }
        set { asuinvaltio_otsikko = value; }
    }

    private string kansalaisuus;
    private string kansalaisuus_otsikko;
    public string Kansalaisuus
    {
        get { return kansalaisuus; }
        set { kansalaisuus = value; }
    }
    public string Kansalaisuus_otsikko
    {
        get { return kansalaisuus_otsikko; }
        set { kansalaisuus_otsikko = value; }
    }

    private string siviilisaaty;
    private string siviilisaaty_otsikko;
    public string Siviilisaaty
    {
        get { return siviilisaaty; }
        set { siviilisaaty = value; }
    }
    public string Siviilisaaty_otsikko
    {
        get { return siviilisaaty_otsikko; }
        set { siviilisaaty_otsikko = value; }
    }

    private string jarjestysnumero;
    private string jarjestysnumero_otsikko;
    public string Jarjestysnumero
    {
        get { return jarjestysnumero; }
        set { jarjestysnumero = value; }
    }
    public string Jarjestysnumero_otsikko
    {
        get { return jarjestysnumero_otsikko; }
        set { jarjestysnumero_otsikko = value; }
    }
}

public class Allekirjoitustiedot
{
    private string aika;
    private string aika_otsikko;
    public string Aika
    {
        get { return aika; }
        set { aika = value; }
    }
    public string Aika_otsikko
    {
        get { return aika_otsikko; }
        set { aika_otsikko = value; }
    }

    private string paikka;
    private string paikka_otsikko;
    public string Paikka
    {
        get { return paikka; }
        set { paikka = value; }
    }
    public string Paikka_otsikko
    {
        get { return paikka_otsikko; }
        set { paikka_otsikko = value; }
    }

    private string viranomainenNimi;
    private string viranomainenNimi_otsikko;
    public string ViranomainenNimi
    {
        get { return viranomainenNimi; }
        set { viranomainenNimi = value; }
    }
    public string ViranomainenNimi_otsikko
    {
        get { return viranomainenNimi_otsikko; }
        set { viranomainenNimi_otsikko = value; }
    }

    private string virkailijaNimi;
    private string virkailijaNimi_otsikko;
    public string VirkailijaNimi
    {
        get { return virkailijaNimi; }
        set { virkailijaNimi = value; }
    }
    public string VirkailijaNimi_otsikko
    {
        get { return virkailijaNimi_otsikko; }
        set { virkailijaNimi_otsikko = value; }
    }

    private string virkanimike;
    private string virkanimike_otsikko;
    public string Virkanimike
    {
        get { return virkanimike; }
        set { virkanimike = value; }
    }
    public string Virkanimike_otsikko
    {
        get { return virkanimike_otsikko; }
        set { virkanimike_otsikko = value; }
    }

    private string lahiosoite;
    private string lahiosoite_otsikko;
    public string Lahiosoite
    {
        get { return lahiosoite; }
        set { lahiosoite = value; }
    }
    public string Lahiosoite_otsikko
    {
        get { return lahiosoite_otsikko; }
        set { lahiosoite_otsikko = value; }
    }

    private string postiosoite;
    private string postiosoite_otsikko;
    public string Postiosoite
    {
        get { return postiosoite; }
        set { postiosoite = value; }
    }
    public string Postiosoite_otsikko
    {
        get { return postiosoite_otsikko; }
        set { postiosoite_otsikko = value; }
    }

    private string postitoimipaikka;
    private string postitoimipaikka_otsikko;
    public string Postitoimipaikka
    {
        get { return postitoimipaikka; }
        set { postitoimipaikka = value; }
    }
    public string Postitoimipaikka_otsikko
    {
        get { return postitoimipaikka_otsikko; }
        set { postitoimipaikka_otsikko = value; }
    }

    private string puhelin;
    private string puhelin_otsikko;
    public string Puhelin
    {
        get { return puhelin; }
        set { puhelin = value; }
    }
    public string Puhelin_otsikko
    {
        get { return puhelin_otsikko; }
        set { puhelin_otsikko = value; }
    }

    private string hinta;
    private string hinta_otsikko;
    public string Hinta
    {
        get { return hinta; }
        set { hinta = value; }
    }
    public string Hinta_otsikko
    {
        get { return hinta_otsikko; }
        set { hinta_otsikko = value; }
    }

    private string asiatunnus;
    private string asiatunnus_otsikko;
    public string Asiatunnus
    {
        get { return asiatunnus; }
        set { asiatunnus = value; }
    }
    public string Asiatunnus_otsikko
    {
        get { return asiatunnus_otsikko; }
        set { asiatunnus_otsikko = value; }
    }
}

