using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Ote
/// </summary>

public static class OteTuote
{
    // Otetuotteiden Tuote_id:t
    public const string REKISTEROINTITIEDOT = "ba755a70-ec02-4d56-b520-617b9f3769b3";
    public const string REKISTEROINTITIEDOT_ETK = "87c5239f-9b3d-4713-98b7-2a2c38b4e9ec";
    public const string REKISTEROINTITIEDOT_KELA = "c85f57d3-f856-4457-bdd7-0effe88fa536";
    public const string REKISTEROINTITIEDOT_VERO = "7a63009a-850d-4f19-8b99-f3840d8d8479";
    public const string MUUTTOILMOITUSPAATOS = "fd7e488e-a9cd-42a0-83b5-bbdf01925abd"; // Päätös muuttoilmoitusasiassa
    public const string MUUTTOILMOITUSPAATOS_SAATE = "5aaf624f-a29b-46dc-aae7-c47de6a81901";
    public const string KOTIKUNTAPAATOS = "0dcedc50-3c13-4336-b4a3-2391e77afb0a";   // Kotikuntapäätös
    public const string KOTIKUNTAPAATOS_SAATE = "9ebebbd0-3823-4cd9-ad8d-4307b6b573ae";
    public const string KOTIKUNTAPAATOS2 = "e8ceea4b-3ab6-4b5e-a456-21af243f12fe";  // Kotikuntapäätös alle vuoden B-lupa
    public const string KOTIKUNTAPAATOS2_SAATE = "d991fa1f-f015-42ee-9511-e3bdc5bf5b07";
    public const string KOTIKUNTAPAATOS3 = "13f427cd-03eb-41ec-a584-8c10465cad21";  // Kotikuntapäätös ei EU-rekisteröity
    public const string KOTIKUNTAPAATOS3_SAATE = "7fbb0ed4-6766-4a64-af6e-7d80db831ed2";
}

public class Ote
{

    private Guid kyseltyTuote;
    public Guid KyseltyTuote
    {
        get { return kyseltyTuote; }
        set { kyseltyTuote = value; }
    }

    private string nimi;
    public string Nimi
    {
        get { return nimi; }
        set { nimi = value; }
    }

    private string tulostuskieli;
    public string Tulostuskieli
    {
        get { return tulostuskieli; }
        set { tulostuskieli = value; }
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

    private string diaarinumero;
    public string Diaarinumero
    {
        get { return diaarinumero; }
        set { diaarinumero = value; }
    }

    private string hakemuksenSaapumispaiva;
    public string HakemuksenSaapumispaiva
    {
        get { return hakemuksenSaapumispaiva; }
        set { hakemuksenSaapumispaiva = value; }
    }

    private string ilmotettuMuuttopaiva;
    public string IlmotettuMuuttopaiva
    {
        get { return ilmotettuMuuttopaiva; }
        set { ilmotettuMuuttopaiva = value; }
    }

    private string tiedoksiantotapa;
    public string Tiedoksiantotapa
    {
        get { return tiedoksiantotapa; }
        set { tiedoksiantotapa = value; }
    }

    private string rekisteroityOsoite;
    public string RekisteroityOsoite
    {
        get { return rekisteroityOsoite; }
        set { rekisteroityOsoite = value; }
    }

    private string paatoksenTyyppi;
    public string PaatoksenTyyppi
    {
        get { return paatoksenTyyppi; }
        set { paatoksenTyyppi = value; }
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

    private string ulkom_henkilotunnus;
    private string ulkom_henkilotunnus_otsikko;
    public string Ulkom_henkilotunnus
    {
        get { return ulkom_henkilotunnus; }
        set { ulkom_henkilotunnus = value; }
    }
    public string Ulkom_henkilotunnus_otsikko
    {
        get { return ulkom_henkilotunnus_otsikko; }
        set { ulkom_henkilotunnus_otsikko = value; }
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

    private string kuolinpv;
    private string kuolinpv_otsikko;
    public string Kuolinpv
    {
        get { return kuolinpv; }
        set { kuolinpv = value; }
    }
    public string Kuolinpv_otsikko
    {
        get { return kuolinpv_otsikko; }
        set { kuolinpv_otsikko = value; }
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

    private string valinimi;
    private string valinimi_otsikko;
    public string Valinimi
    {
        get { return valinimi; }
        set { valinimi = value; }
    }
    public string Valinimi_otsikko
    {
        get { return valinimi_otsikko; }
        set { valinimi_otsikko = value; }
    }

    private string patronyymi;
    private string patronyymi_otsikko;
    public string Patronyymi
    {
        get { return patronyymi; }
        set { patronyymi = value; }
    }
    public string Patronyymi_otsikko
    {
        get { return patronyymi_otsikko; }
        set { patronyymi_otsikko = value; }
    }

    private string nimenLisatieto;
    private string nimenLisatieto_otsikko;
    public string NimenLisatieto
    {
        get { return nimenLisatieto; }
        set { nimenLisatieto = value; }
    }
    public string NimenLisatieto_otsikko
    {
        get { return nimenLisatieto_otsikko; }
        set { nimenLisatieto_otsikko = value; }
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

    private string tietojenluovutuskielto;
    private string tietojenluovutuskielto_otsikko;
    public string Tietojenluovutuskielto
    {
        get { return tietojenluovutuskielto; }
        set { tietojenluovutuskielto = value; }
    }
    public string Tietojenluovutuskielto_otsikko
    {
        get { return tietojenluovutuskielto_otsikko; }
        set { tietojenluovutuskielto_otsikko = value; }
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

    private string aidinkieli;
    private string aidinkieli_otsikko;
    public string Aidinkieli
    {
        get { return aidinkieli; }
        set { aidinkieli = value; }
    }
    public string Aidinkieli_otsikko
    {
        get { return aidinkieli_otsikko; }
        set { aidinkieli_otsikko = value; }
    }

    private string asiointikieli;
    private string asiointikieli_otsikko;
    public string Asiointikieli
    {
        get { return asiointikieli; }
        set { asiointikieli = value; }
    }
    public string Asiointikieli_otsikko
    {
        get { return asiointikieli_otsikko; }
        set { asiointikieli_otsikko = value; }
    }

    private string ammatti;
    private string ammatti_otsikko;
    public string Ammatti
    {
        get { return ammatti; }
        set { ammatti = value; }
    }
    public string Ammatti_otsikko
    {
        get { return ammatti_otsikko; }
        set { ammatti_otsikko = value; }
    }

    private string rekisteriviranomainen;
    private string rekisteriviranomainen_otsikko;
    public string Rekisteriviranomainen
    {
        get { return rekisteriviranomainen; }
        set { rekisteriviranomainen = value; }
    }
    public string Rekisteriviranomainen_otsikko
    {
        get { return rekisteriviranomainen_otsikko; }
        set { rekisteriviranomainen_otsikko = value; }
    }

    private string sahkoposti;
    private string sahkoposti_otsikko;
    public string Sahkoposti
    {
        get { return sahkoposti; }
        set { sahkoposti = value; }
    }
    public string Sahkoposti_otsikko
    {
        get { return sahkoposti_otsikko; }
        set { sahkoposti_otsikko = value; }
    }


    private string saateteksti;
    public string Saateteksti
    {
        get { return saateteksti; }
        set { saateteksti = value; }
    }

    private string saateteksti2;
    public string Saateteksti2
    {
        get { return saateteksti2; }
        set { saateteksti2 = value; }
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

    private string tietosuojalausunto;
    private string tietosuojalausunto_otsikko;
    public string Tietosuojalausunto
    {
        get { return tietosuojalausunto; }
        set { tietosuojalausunto = value; }
    }
    public string Tietosuojalausunto_otsikko
    {
        get { return tietosuojalausunto_otsikko; }
        set { tietosuojalausunto_otsikko = value; }
    }

    private string palautekysely;
    public string Palautekysely
    {
        get { return palautekysely; }
        set { palautekysely = value; }
    }

    public List<Taulukkotieto> ulkomaisetHenkilonumerot { get; set; }
    public List<Taulukkotieto> passivoidutHenkilotunnukset { get; set; }
    public List<TaulukkotietoOsoite> vakinaisetOsoitteet { get; set; }
    public List<TaulukkotietoOsoite> tilapaisetOsoitteet { get; set; }
    public List<TaulukkotietoOsoite> postiOsoitteet { get; set; }
    public List<Taulukkotieto> sahkopostiOsoitteet { get; set; }
    public List<Taulukkotieto> entisetSukunimet { get; set; }
    public List<Taulukkotieto> entisetEtunimet { get; set; }
    public List<Taulukkotieto> korjatutSukunimet { get; set; }
    public List<Taulukkotieto> korjatutEtunimet { get; set; }
    public List<TaulukkotietoOsoite> entisetVakinaisetOsoitteet { get; set; }
    public List<TaulukkotietoOsoite> entisetTilapaisetOsoitteet { get; set; }
    public List<Taulukkotieto> entisetKotikunnat { get; set; }
    public List<Taulukkotieto> entisetAvioliitot { get; set; }
    public List<TaulukkotietoViitehenkilo> vanhemmat { get; set; }
    public List<TaulukkotietoViitehenkilo> huoltajat { get; set; }
    public List<TaulukkotietoViitehenkilo> lapset { get; set; }
    public List<TaulukkotietoViitehenkilo> huollettavat { get; set; }
    public List<TaulukkotietoAvioliitto> avioliitot { get; set; }
    public List<TaulukkotietoAvioliitto> rekpat { get; set; }
    public List<TaulukkotietoEdunvalvonta> edunvalvonnat { get; set; }
    public List<TaulukkotietoEdunvalvontavaltuutus> edunvalvontavaltuutukset { get; set; }
    public Allekirjoitustiedot allekirjoitustiedot { get; set; }


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

        private string jarjestysnro;
        private string jarjestysnro_otsikko;
        public string Jarjestysnro
        {
            get { return jarjestysnro; }
            set { jarjestysnro = value; }
        }
        public string Jarjestysnro_otsikko
        {
            get { return jarjestysnro_otsikko; }
            set { jarjestysnro_otsikko = value; }
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

        private string loppupaiva;
        private string loppupaiva_otsikko;
        public string Loppupaiva
        {
            get { return loppupaiva; }
            set { loppupaiva = value; }
        }
        public string Loppupaiva_otsikko
        {
            get { return loppupaiva_otsikko; }
            set { loppupaiva_otsikko = value; }
        }

        private string paattymistapa;
        private string paattymistapa_otsikko;
        public string Paattymistapa
        {
            get { return paattymistapa; }
            set { paattymistapa = value; }
        }
        public string Paattymistapa_otsikko
        {
            get { return paattymistapa_otsikko; }
            set { paattymistapa_otsikko = value; }
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

        private string entisetEtunimet;
        private string entisetEtunimet_otsikko;
        public string EntisetEtunimet
        {
            get { return entisetEtunimet; }
            set { entisetEtunimet = value; }
        }
        public string EntisetEtunimet_otsikko
        {
            get { return entisetEtunimet_otsikko; }
            set { entisetEtunimet_otsikko = value; }
        }

        private string korjattuSukunimi;
        private string korjattuSukunimi_otsikko;
        public string KorjattuSukunimi
        {
            get { return korjattuSukunimi; }
            set { korjattuSukunimi = value; }
        }
        public string KorjattuSukunimi_otsikko
        {
            get { return korjattuSukunimi_otsikko; }
            set { korjattuSukunimi_otsikko = value; }
        }

        private string korjatutEtunimet;
        private string korjatutEtunimet_otsikko;
        public string KorjatutEtunimet
        {
            get { return korjatutEtunimet; }
            set { korjatutEtunimet = value; }
        }
        public string KorjatutEtunimet_otsikko
        {
            get { return korjatutEtunimet_otsikko; }
            set { korjatutEtunimet_otsikko = value; }
        }

        private string kuolinpv;
        private string kuolinpv_otsikko;
        public string Kuolinpv
        {
            get { return kuolinpv; }
            set { kuolinpv = value; }
        }
        public string Kuolinpv_otsikko
        {
            get { return kuolinpv_otsikko; }
            set { kuolinpv_otsikko = value; }
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

    public class TaulukkotietoViitehenkilo
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

        private string ulkohloSyntymaaika;
        private string ulkohloSyntymaaika_otsikko;
        public string UlkohloSyntymaaika
        {
            get { return ulkohloSyntymaaika; }
            set { ulkohloSyntymaaika = value; }
        }
        public string UlkohloSyntymaaika_otsikko
        {
            get { return ulkohloSyntymaaika_otsikko; }
            set { ulkohloSyntymaaika_otsikko = value; }
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

        private string kuolinpv;
        private string kuolinpv_otsikko;
        public string Kuolinpv
        {
            get { return kuolinpv; }
            set { kuolinpv = value; }
        }
        public string Kuolinpv_otsikko
        {
            get { return kuolinpv_otsikko; }
            set { kuolinpv_otsikko = value; }
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

        private string viranomainenNimiEiYksikkoa;
        private string viranomainenNimiEiYksikkoa_otsikko;
        public string ViranomainenNimiEiYksikkoa
        {
            get { return viranomainenNimiEiYksikkoa; }
            set { viranomainenNimiEiYksikkoa = value; }
        }
        public string ViranomainenNimiEiYksikkoa_otsikko
        {
            get { return viranomainenNimiEiYksikkoa_otsikko; }
            set { viranomainenNimiEiYksikkoa_otsikko = value; }
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

        private string sahkoposti;
        private string sahkoposti_otsikko;
        public string Sahkoposti
        {
            get { return sahkoposti; }
            set { sahkoposti = value; }
        }
        public string Sahkoposti_otsikko
        {
            get { return sahkoposti_otsikko; }
            set { sahkoposti_otsikko = value; }
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
    }


}