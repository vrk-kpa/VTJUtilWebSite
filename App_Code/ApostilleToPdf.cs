using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Xml;
using System.Text;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

/// <summary>
/// Summary description for ApostilleToPdf
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ApostilleToPdf : System.Web.Services.WebService {

    public ApostilleToPdf () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public byte[] LuoApostille(ApostilleTiedot tiedot) 
    {
        string chkboxValittu = "Yes";
        char[] merkit = new char[] { '.', '/' };
        string tyhja = "-";

        // Jos kyseessä siviilisäätytodistus, niin 
        // päätellään täytetäänkö MaritalStatus vaiko RegisteredPartnershipStatus -lomake
        if (tiedot.Todistus.Equals("FI_MaritalStatus"))
        {
            if (tiedot.SiviilisaatyPvm.Contains("Rekisteröidyssä parisuhteessa") || 
                tiedot.SiviilisaatyPvm.Contains("Registrerad partner") || 
                tiedot.SiviilisaatyPvm.Contains("Eronnut rekisteröidystä parisuhteesta") || 
                tiedot.SiviilisaatyPvm.Contains("Skild från registrerad partner") || 
                tiedot.SiviilisaatyPvm.Contains("Leski rekisteröidystä parisuhteesta") || 
                tiedot.SiviilisaatyPvm.Contains("Efterlevande partner (registrerat partnerskap)"))
            {
                tiedot.Todistus = "FI_RegisteredPartnershipStatus";
            }
            else
            {
                tiedot.Todistus = "FI_MaritalStatus";
            }
        }

        // EU-vakiolomakepohja 
        string pdfPath = System.Configuration.ConfigurationManager.AppSettings["EU_vakiolomake"];
        pdfPath = pdfPath + tiedot.Todistus + "_" + tiedot.Tulostuskieli + "_" + tiedot.Kohdemaa + ".pdf"; 

        var output = new MemoryStream();
        var reader = new PdfReader(pdfPath);
        var stamper = new PdfStamper(reader, output);
        var formFields = stamper.AcroFields;

//        BaseFont bs = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

//        Font font = new Font(bs, 8);
        //formFields.AddSubstitutionFont(font.BaseFont);
//        formFields.AddSubstitutionFont(bs);

        string fontit = System.Configuration.ConfigurationManager.AppSettings["OTE_fontit"];

        BaseFont bs = BaseFont.CreateFont(fontit + "FreeSerif.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        formFields.AddSubstitutionFont(bs);

        //var dr = reader.AcroForm.GetAsDict(PdfName.DR);
        //var fontDict = dr.GetAsDict(PdfName.FONT);
        //foreach (var fk in fontDict.Keys)
        //{
        //    var data = fontDict.GetAsDict(fk);
        //    var desc = data.GetAsDict(PdfName.FONTDESCRIPTOR);
        //    if (desc != null)
        //    {
        //        var family = desc.GetAsDict(PdfName.FONTFAMILY);

        //    }
        //}

        // Alustetaan lomakkeen tekstikentät "-" merkillä
        var fieldKeys = formFields.Fields.Keys;
        foreach (string fieldKey in fieldKeys)
        {
            if (fieldKey.Contains("text_"))
            {
                formFields.SetFieldProperty(fieldKey, "textfont", bs, null);
//                formFields.SetFieldProperty(fieldKey, "textsize", (float)10, null);
                formFields.SetField(fieldKey, tyhja);
            }
        }


        #region Syntymätodistus
        // SYNTYMÄTODISTUS
        if (tiedot.Todistus == "FI_Birth")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_73", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            //formFields.SetField("text_79", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
            formFields.SetField("text_82", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_85", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_94", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_107", chkboxValittu);
            formFields.SetField("checkbox_125", chkboxValittu); 

            formFields.SetField("text_134", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_140", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // SYNTYNEEN HENKILÖN TIEDOT //
            // Nimet
            formFields.SetField("text_198", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_206", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_210", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Entiset sukunimet
            string entSukunimi = string.Empty;
            foreach (Taulukko sukunimi in tiedot.entisetSukunimet)
            {
                if (entSukunimi == string.Empty)
                    entSukunimi = sukunimi.Sarake2;
                else
                    entSukunimi = entSukunimi + ", " + sukunimi.Sarake2;
            }
            formFields.SetField("text_200", (string.IsNullOrEmpty(entSukunimi) ? tyhja : entSukunimi));

            // Entiset etunimet
            string entEtunimi = string.Empty;
            foreach (Taulukko etunimi in tiedot.entisetEtunimet)
            {
                if (entEtunimi == string.Empty)
                    entEtunimi = etunimi.Sarake2;
                else
                    entEtunimi = entEtunimi + ", " + etunimi.Sarake2;
            }
            // - 30.8.2023 Ei vielä paikkaa entisille etunimille 
            //formFields.SetField("text_???", (string.IsNullOrEmpty(entEtunimi) ? tyhja : entEtunimi));

            // Syntymäaika
            formFields.SetField("text_230", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Syntymäpaikka ja -maa
            string syntymapaikka = string.Empty;
            syntymapaikka = (string.IsNullOrEmpty(tiedot.Syntymapaikka) ? "" : tiedot.Syntymapaikka + " ") 
                          + (string.IsNullOrEmpty(tiedot.Syntymavaltio) ? "" : tiedot.Syntymavaltio);
            formFields.SetField("text_253", (string.IsNullOrEmpty(syntymapaikka) ? tyhja : syntymapaikka));

            // Syntymäkotikunta
            switch (tiedot.Syntymakotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_260", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänd":
                    formFields.SetField("checkbox_261", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_257", (string.IsNullOrEmpty(tiedot.Syntymakotikunta) ? tyhja : tiedot.Syntymakotikunta));
                    break;
            }

            // Sukupuoli
            switch (tiedot.Sukupuoli)
            {
                case "Nainen":
                case "Kvinna":
                    formFields.SetField("checkbox_270", chkboxValittu);
                    break;
                case "Mies":
                case "Man":
                    formFields.SetField("checkbox_271", chkboxValittu);
                    break;
                default:
                    formFields.SetField("checkbox_272", chkboxValittu);
                    break;
            }
            
            // Henkilötunnus
            formFields.SetField("text_380", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Kansalaisuus
            switch (tiedot.Kansalaisuus)
            {
                case "Ei selvit":
                case "Ei selvit (99)":
                case "Ej uträtt":
                case "Ej uträtt (99)":
                    formFields.SetField("checkbox_518", chkboxValittu);
                    break;
                case "Tuntematon":
                case "Tuntematon (99)":
                case "Okänt":
                case "Okänt (99)":
                    formFields.SetField("checkbox_527", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_481", (string.IsNullOrEmpty(tiedot.Kansalaisuus) ? tyhja : tiedot.Kansalaisuus));
                    break;
            }

            // Kotikunta (valinnainen tieto otteella)
            if (!string.IsNullOrEmpty(tiedot.Kotikunta))
            {
                switch (tiedot.Kotikunta)
                {
                    case "Ei kotikuntaa Suomessa":
                    case "Ej hemkommun i Finland":
                        formFields.SetField("checkbox_567", chkboxValittu);
                        break;
                    case "Ulkomaat":
                    case "Utlandet":
                        formFields.SetField("checkbox_572", chkboxValittu);
                        break;
                    case "TUNTEMATON":
                    case "Okänt":
                        formFields.SetField("checkbox_585", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("text_549", tiedot.Kotikunta);
                        break;
                }
            }

            // VANHEMMAT
            foreach (Vanhempi vanhempi in tiedot.vanhemmat)
            {
                switch (vanhempi.IsaAiti)
                { 
                    case "ISÄ":
                    case "Isä":
                    case "FAR":
                    case "Far":
                        // Nimet
                        formFields.SetField("text_661", (string.IsNullOrEmpty(vanhempi.NykyinenSukunimi) ? tyhja : vanhempi.NykyinenSukunimi));
                        formFields.SetField("text_662", (string.IsNullOrEmpty(vanhempi.EntinenSukunimiYli15v) ? tyhja : vanhempi.EntinenSukunimiYli15v));
                        formFields.SetField("text_664", (string.IsNullOrEmpty(vanhempi.NykyisetEtunimet) ? tyhja : vanhempi.NykyisetEtunimet));

                        // Syntymäaika
                        formFields.SetField("text_671", (string.IsNullOrEmpty(vanhempi.Syntymaaika) ? tyhja : vanhempi.Syntymaaika.Replace(merkit[0], merkit[1])));

                        // Syntymäpaikka ja -maa
                        string isaSyntymapaikka = string.Empty;
                        isaSyntymapaikka = (string.IsNullOrEmpty(vanhempi.Syntymapaikka) ? "" : vanhempi.Syntymapaikka + " ")
                                         + (string.IsNullOrEmpty(vanhempi.Syntymavaltio) ? "" : vanhempi.Syntymavaltio);
                        formFields.SetField("text_674", (string.IsNullOrEmpty(isaSyntymapaikka) ? tyhja : isaSyntymapaikka));

                        // Syntymäkotikunta
                        switch (vanhempi.Syntymakotikunta)
                        {
                            case "Ulkomaat":
                            case "Utlandet":
                                formFields.SetField("checkbox_677", chkboxValittu);
                                break;
                            case "TUNTEMATON":
                            case "Okänd":
                                formFields.SetField("checkbox_679", chkboxValittu);
                                break;
                            default:
                                formFields.SetField("text_676", (string.IsNullOrEmpty(vanhempi.Syntymakotikunta) ? tyhja : vanhempi.Syntymakotikunta));
                                break;
                        }

                        // Henkilötunnus
                        formFields.SetField("text_680", (string.IsNullOrEmpty(vanhempi.Henkilotunnus) ? tyhja : vanhempi.Henkilotunnus));

                        // Kansalaisuus
                        switch (vanhempi.Kansalaisuus)
                        {
                            case "Ei selvit":
                            case "Ei selvit (99)":
                            case "Ej uträtt":
                            case "Ej uträtt (99)":
                                formFields.SetField("checkbox_683", chkboxValittu);
                                break;
                            case "Tuntematon":
                            case "Tuntematon (99)":
                            case "Okänt":
                            case "Okänt (99)":
                                formFields.SetField("checkbox_684", chkboxValittu);
                                break;
                            default:
                                formFields.SetField("text_681", (string.IsNullOrEmpty(vanhempi.Kansalaisuus) ? tyhja : vanhempi.Kansalaisuus));
                                break;
                        }

                        break;

                    case "ÄITI":
                    case "Äiti":
                    case "MOR":
                    case "Mor":
                        // Nimet
                        formFields.SetField("text_691", (string.IsNullOrEmpty(vanhempi.NykyinenSukunimi) ? tyhja : vanhempi.NykyinenSukunimi));
                        formFields.SetField("text_693", (string.IsNullOrEmpty(vanhempi.EntinenSukunimiYli15v) ? tyhja : vanhempi.EntinenSukunimiYli15v));
                        formFields.SetField("text_696", (string.IsNullOrEmpty(vanhempi.NykyisetEtunimet) ? tyhja : vanhempi.NykyisetEtunimet));

                        // Syntymäaika
                        formFields.SetField("text_701", (string.IsNullOrEmpty(vanhempi.Syntymaaika) ? tyhja : vanhempi.Syntymaaika.Replace(merkit[0], merkit[1])));

                        // Syntymäpaikka ja -maa
                        string aitiSyntymapaikka = string.Empty;
                        aitiSyntymapaikka = (string.IsNullOrEmpty(vanhempi.Syntymapaikka) ? "" : vanhempi.Syntymapaikka + " ")
                                          + (string.IsNullOrEmpty(vanhempi.Syntymavaltio) ? "" : vanhempi.Syntymavaltio);
                        formFields.SetField("text_704", (string.IsNullOrEmpty(aitiSyntymapaikka) ? tyhja : aitiSyntymapaikka));

                        // Syntymäkotikunta
                        switch (vanhempi.Syntymakotikunta)
                        {
                            case "Ulkomaat":
                            case "Utlandet":
                                formFields.SetField("checkbox_709", chkboxValittu);
                                break;
                            case "TUNTEMATON":
                            case "Okänd":
                                formFields.SetField("checkbox_710", chkboxValittu);
                                break;
                            default:
                                formFields.SetField("text_706", (string.IsNullOrEmpty(vanhempi.Syntymakotikunta) ? tyhja : vanhempi.Syntymakotikunta));
                                break;
                        }

                        // Henkilötunnus
                        formFields.SetField("text_711", (string.IsNullOrEmpty(vanhempi.Henkilotunnus) ? tyhja : vanhempi.Henkilotunnus));

                        // Kansalaisuus
                        switch (vanhempi.Kansalaisuus)
                        {
                            case "Ei selvit":
                            case "Ei selvit (99)":
                            case "Ej uträtt":
                            case "Ej uträtt (99)":
                                formFields.SetField("checkbox_715", chkboxValittu);
                                break;
                            case "Tuntematon":
                            case "Tuntematon (99)":
                            case "Okänt":
                            case "Okänt (99)":
                                formFields.SetField("checkbox_716", chkboxValittu);
                                break;
                            default:
                                formFields.SetField("text_713", (string.IsNullOrEmpty(vanhempi.Kansalaisuus) ? tyhja : vanhempi.Kansalaisuus));
                                break;
                        }

                        break;
                }
            }

            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_831", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi)); 
            formFields.SetField("text_832", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_833", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));

        }
        #endregion

        #region Syntymätodistus2
        // SYNTYMÄTODISTUS 2 - VTJKYS-3924
        else if (tiedot.Todistus == "FI_Birth2")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_73", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_75", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_79", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_86", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_90", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_103", chkboxValittu);
            formFields.SetField("checkbox_121", chkboxValittu);

            formFields.SetField("text_date_131", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_141", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // SYNTYNEEN HENKILÖN TIEDOT //
            // Nimet
            formFields.SetField("text_206", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_214", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_217", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Entiset sukunimet
            string entSukunimi = string.Empty;
            foreach (Taulukko sukunimi in tiedot.entisetSukunimet)
            {
                if (entSukunimi == string.Empty)
                    entSukunimi = sukunimi.Sarake2;
                else
                    entSukunimi = entSukunimi + ", " + sukunimi.Sarake2;
            }
            formFields.SetField("text_207", (string.IsNullOrEmpty(entSukunimi) ? tyhja : entSukunimi));

            // Entiset etunimet
            string entEtunimi = string.Empty;
            foreach (Taulukko etunimi in tiedot.entisetEtunimet)
            {
                if (entEtunimi == string.Empty)
                    entEtunimi = etunimi.Sarake2;
                else
                    entEtunimi = entEtunimi + ", " + etunimi.Sarake2;
            }
            // - 30.8.2023 Ei vielä paikkaa entisille etunimille 
            //formFields.SetField("text_???", (string.IsNullOrEmpty(entEtunimi) ? tyhja : entEtunimi));

            // Syntymäaika
            formFields.SetField("text_date_238", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Syntymäpaikka ja -maa
            string syntymapaikka = string.Empty;
            syntymapaikka = (string.IsNullOrEmpty(tiedot.Syntymapaikka) ? "" : tiedot.Syntymapaikka + " ")
                          + (string.IsNullOrEmpty(tiedot.Syntymavaltio) ? "" : tiedot.Syntymavaltio);
            formFields.SetField("text_259", (string.IsNullOrEmpty(syntymapaikka) ? tyhja : syntymapaikka));

            // Syntymäkotikunta
            switch (tiedot.Syntymakotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_266", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänd":
                    formFields.SetField("checkbox_267", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_265", (string.IsNullOrEmpty(tiedot.Syntymakotikunta) ? tyhja : tiedot.Syntymakotikunta));
                    break;
            }

            // Sukupuoli
            switch (tiedot.Sukupuoli)
            {
                case "Nainen":
                case "Kvinna":
                    formFields.SetField("checkbox_274", chkboxValittu);
                    break;
                case "Mies":
                case "Man":
                    formFields.SetField("checkbox_275", chkboxValittu);
                    break;
                default:
                    formFields.SetField("checkbox_276", chkboxValittu);
                    break;
            }

            // Henkilötunnus
            formFields.SetField("text_394", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Kansalaisuus
            switch (tiedot.Kansalaisuus)
            {
                case "Ei selvit":
                case "Ei selvit (99)":
                case "Ej uträtt":
                case "Ej uträtt (99)":
                    formFields.SetField("checkbox_523", chkboxValittu);
                    break;
                case "Tuntematon":
                case "Tuntematon (99)":
                case "Okänt":
                case "Okänt (99)":
                    formFields.SetField("checkbox_537", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_501", (string.IsNullOrEmpty(tiedot.Kansalaisuus) ? tyhja : tiedot.Kansalaisuus));
                    break;
            }

            // Kotikunta (valinnainen tieto otteella)
            if (!string.IsNullOrEmpty(tiedot.Kotikunta))
            {
                switch (tiedot.Kotikunta)
                {
                    case "Ei kotikuntaa Suomessa":
                    case "Ej hemkommun i Finland":
                        formFields.SetField("checkbox_585", chkboxValittu);
                        break;
                    case "Ulkomaat":
                    case "Utlandet":
                        formFields.SetField("checkbox_602", chkboxValittu);
                        break;
                    case "TUNTEMATON":
                    case "Okänt":
                        formFields.SetField("checkbox_603", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("text_576", tiedot.Kotikunta);
                        break;
                }
            }

            // HENKILÖN VANHEMMAT //
            int vanhempiNro = 0;
            foreach (Vanhempi vanhempi in tiedot.vanhemmat)
            {
                vanhempiNro += 1;

                // vanhempi 1
                if (vanhempiNro == 1)
                {
                    // Isä/Äiti
                    switch (vanhempi.IsaAiti)
                    {
                        case "ISÄ":
                        case "Isä":
                        case "FAR":
                        case "Far":
                            formFields.SetField("checkbox_740", chkboxValittu);
                            break;
                        case "ÄITI":
                        case "Äiti":
                        case "MOR":
                        case "Mor":
                            formFields.SetField("checkbox_743", chkboxValittu);
                            break;
                    }

                    // Nimet
                    formFields.SetField("text_746", (string.IsNullOrEmpty(vanhempi.NykyinenSukunimi) ? tyhja : vanhempi.NykyinenSukunimi));
                    formFields.SetField("text_747", (string.IsNullOrEmpty(vanhempi.EntinenSukunimiYli15v) ? tyhja : vanhempi.EntinenSukunimiYli15v));
                    formFields.SetField("text_749", (string.IsNullOrEmpty(vanhempi.NykyisetEtunimet) ? tyhja : vanhempi.NykyisetEtunimet));

                    // Syntymäaika
                    formFields.SetField("text_date_755", (string.IsNullOrEmpty(vanhempi.Syntymaaika) ? tyhja : vanhempi.Syntymaaika.Replace(merkit[0], merkit[1])));

                    // Syntymäpaikka ja -maa
                    string vanh1Syntymapaikka = string.Empty;
                    vanh1Syntymapaikka = (string.IsNullOrEmpty(vanhempi.Syntymapaikka) ? "" : vanhempi.Syntymapaikka + " ")
                                     + (string.IsNullOrEmpty(vanhempi.Syntymavaltio) ? "" : vanhempi.Syntymavaltio);
                    formFields.SetField("text_757", (string.IsNullOrEmpty(vanh1Syntymapaikka) ? tyhja : vanh1Syntymapaikka));

                    // Syntymäkotikunta
                    switch (vanhempi.Syntymakotikunta)
                    {
                        case "Ulkomaat":
                        case "Utlandet":
                            formFields.SetField("checkbox_762", chkboxValittu);
                            break;
                        case "TUNTEMATON":
                        case "Okänd":
                            formFields.SetField("checkbox_763", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_759", (string.IsNullOrEmpty(vanhempi.Syntymakotikunta) ? tyhja : vanhempi.Syntymakotikunta));
                            break;
                    }

                    // Henkilötunnus
                    formFields.SetField("text_764", (string.IsNullOrEmpty(vanhempi.Henkilotunnus) ? tyhja : vanhempi.Henkilotunnus));

                    // Kansalaisuus
                    switch (vanhempi.Kansalaisuus)
                    {
                        case "Ei selvit":
                        case "Ei selvit (99)":
                        case "Ej uträtt":
                        case "Ej uträtt (99)":
                            formFields.SetField("checkbox_767", chkboxValittu);
                            break;
                        case "Tuntematon":
                        case "Tuntematon (99)":
                        case "Okänt":
                        case "Okänt (99)":
                            formFields.SetField("checkbox_768", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_765", (string.IsNullOrEmpty(vanhempi.Kansalaisuus) ? tyhja : vanhempi.Kansalaisuus));
                            break;
                    }
                }

                // Vanhempi 2
                else if (vanhempiNro == 2)
                {
                    // Isä/Äiti
                    switch (vanhempi.IsaAiti)
                    {
                        case "ISÄ":
                        case "Isä":
                        case "FAR":
                        case "Far":
                            formFields.SetField("checkbox_774", chkboxValittu);
                            break;
                        case "ÄITI":
                        case "Äiti":
                        case "MOR":
                        case "Mor":
                            formFields.SetField("checkbox_776", chkboxValittu);
                            break;
                    }

                    // Nimet
                    formFields.SetField("text_779", (string.IsNullOrEmpty(vanhempi.NykyinenSukunimi) ? tyhja : vanhempi.NykyinenSukunimi));
                    formFields.SetField("text_780", (string.IsNullOrEmpty(vanhempi.EntinenSukunimiYli15v) ? tyhja : vanhempi.EntinenSukunimiYli15v));
                    formFields.SetField("text_782", (string.IsNullOrEmpty(vanhempi.NykyisetEtunimet) ? tyhja : vanhempi.NykyisetEtunimet));

                    // Syntymäaika
                    formFields.SetField("text_date_786", (string.IsNullOrEmpty(vanhempi.Syntymaaika) ? tyhja : vanhempi.Syntymaaika.Replace(merkit[0], merkit[1])));

                    // Syntymäpaikka ja -maa
                    string vanh2Syntymapaikka = string.Empty;
                    vanh2Syntymapaikka = (string.IsNullOrEmpty(vanhempi.Syntymapaikka) ? "" : vanhempi.Syntymapaikka + " ")
                                      + (string.IsNullOrEmpty(vanhempi.Syntymavaltio) ? "" : vanhempi.Syntymavaltio);
                    formFields.SetField("text_789", (string.IsNullOrEmpty(vanh2Syntymapaikka) ? tyhja : vanh2Syntymapaikka));

                    // Syntymäkotikunta
                    switch (vanhempi.Syntymakotikunta)
                    {
                        case "Ulkomaat":
                        case "Utlandet":
                            formFields.SetField("checkbox_793", chkboxValittu);
                            break;
                        case "TUNTEMATON":
                        case "Okänd":
                            formFields.SetField("checkbox_795", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_792", (string.IsNullOrEmpty(vanhempi.Syntymakotikunta) ? tyhja : vanhempi.Syntymakotikunta));
                            break;
                    }

                    // Henkilötunnus
                    formFields.SetField("text_796", (string.IsNullOrEmpty(vanhempi.Henkilotunnus) ? tyhja : vanhempi.Henkilotunnus));

                    // Kansalaisuus
                    switch (vanhempi.Kansalaisuus)
                    {
                        case "Ei selvit":
                        case "Ei selvit (99)":
                        case "Ej uträtt":
                        case "Ej uträtt (99)":
                            formFields.SetField("checkbox_800", chkboxValittu);
                            break;
                        case "Tuntematon":
                        case "Tuntematon (99)":
                        case "Okänt":
                        case "Okänt (99)":
                            formFields.SetField("checkbox_801", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_798", (string.IsNullOrEmpty(vanhempi.Kansalaisuus) ? tyhja : vanhempi.Kansalaisuus));
                            break;
                    }
                }
            }

            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_916", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
            formFields.SetField("text_917", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_date_918", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));

        }
        #endregion

        #region Siviilisäätytodistus
        // SIVIILISÄÄTYTODISTUS / Avioliitto
        else if (tiedot.Todistus == "FI_MaritalStatus")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_58", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_60", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            //formFields.SetField("text_63", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
            formFields.SetField("text_65", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_69", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_81", chkboxValittu);
            formFields.SetField("checkbox_85", chkboxValittu);

            formFields.SetField("text_92", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_94", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // ASIANOMAISEN HENKILÖN TIEDOT
            // Nimet
            formFields.SetField("text_124", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_131", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_140", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Syntymäaika
            formFields.SetField("text_141", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Henkilötunnus
            formFields.SetField("text_161", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Kotikunta
            switch (tiedot.Kotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_211", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänt":
                    formFields.SetField("checkbox_212", chkboxValittu);
                    break;
                case "Ei kotikuntaa Suomessa":
                case "Ej hemkommun i Finland":
                    formFields.SetField("checkbox_213", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_210", (string.IsNullOrEmpty(tiedot.Kotikunta) ? tyhja : tiedot.Kotikunta.Replace(merkit[0], merkit[1])));
                    break;
            }

            // Siviilisääty
            if (tiedot.SiviilisaatyPvm.Contains("Avioliitossa") || tiedot.SiviilisaatyPvm.Contains("Gift"))
            {
                formFields.SetField("checkbox_338", chkboxValittu);
                formFields.SetField("text_338", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Naimaton") || tiedot.SiviilisaatyPvm.Contains("Ogift"))
            {
                formFields.SetField("checkbox_349", chkboxValittu);
                formFields.SetField("checkbox_350", chkboxValittu);
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Eronnut") || tiedot.SiviilisaatyPvm.Contains("Frånskild"))
            {
                formFields.SetField("checkbox_349", chkboxValittu);
                formFields.SetField("checkbox_351", chkboxValittu);
                formFields.SetField("text_351", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Leski") || tiedot.SiviilisaatyPvm.Contains("Änka/änkling"))
            {
                formFields.SetField("checkbox_349", chkboxValittu);
                formFields.SetField("checkbox_357", chkboxValittu);
                formFields.SetField("text_357", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Ei tietoa") || tiedot.SiviilisaatyPvm.Contains("Okänt"))
            {
                formFields.SetField("checkbox_375", chkboxValittu);
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Asumuserossa") || tiedot.SiviilisaatyPvm.Contains("Hemskild"))
            {
                formFields.SetField("checkbox_379", chkboxValittu);
            }
            
            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_482", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
            formFields.SetField("text_483", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_484", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
        }    

        // SIVIILISÄÄTYTODISTUS / Rekisteröity parisuhde
        else if (tiedot.Todistus == "FI_RegisteredPartnershipStatus")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_48", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_50", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            //formFields.SetField("text_51", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
            formFields.SetField("text_52", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_53", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_54", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_62", chkboxValittu);
            formFields.SetField("checkbox_65", chkboxValittu);

            formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_73", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // ASIANOMAISEN HENKILÖN TIEDOT
            // Nimet
            formFields.SetField("text_79", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_85", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_89", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Syntymäaika
            formFields.SetField("text_90", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Henkilötunnus
            formFields.SetField("text_105", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Kotikunta
            switch (tiedot.Kotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_130", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänt":
                    formFields.SetField("checkbox_131", chkboxValittu);
                    break;
                case "Ei kotikuntaa Suomessa":
                case "Ej hemkommun i Finland":
                    formFields.SetField("checkbox_132", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_129", (string.IsNullOrEmpty(tiedot.Kotikunta) ? tyhja : tiedot.Kotikunta));
                    break;
            }

            // Siviilisääty
            if (tiedot.SiviilisaatyPvm.Contains("Rekisteröidyssä parisuhteessa") || tiedot.SiviilisaatyPvm.Contains("Registrerad partner"))
            {
                formFields.SetField("checkbox_157", chkboxValittu);
                formFields.SetField("text_157", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Eronnut rekisteröidystä parisuhteesta") || tiedot.SiviilisaatyPvm.Contains("Skild från registrerad partner"))
            {
                formFields.SetField("checkbox_159", chkboxValittu);
                formFields.SetField("checkbox_161", chkboxValittu);
                formFields.SetField("text_161", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }
            else if (tiedot.SiviilisaatyPvm.Contains("Leski rekisteröidystä parisuhteesta") || tiedot.SiviilisaatyPvm.Contains("Efterlevande partner (registrerat partnerskap)"))
            {
                formFields.SetField("checkbox_159", chkboxValittu);
                formFields.SetField("checkbox_167", chkboxValittu);
                formFields.SetField("text_167", vainPaivamaara(tiedot.SiviilisaatyPvm));
            }

            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_201", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
            formFields.SetField("text_202", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_203", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));

        }
    
        #endregion

        #region Elossaolotodistus
        // ELOSSAOLOTODISTUS
        else if (tiedot.Todistus == "FI_Life")
        {
            // tulostetaan täytetty pdf vain jos henkilö on elossa
            if (tiedot.Elossaolotieto.Contains("Henkilö on elossa") || tiedot.Elossaolotieto.Contains("Personen lever"))
            {

                // OTTEEN ANTAJAN TIEDOT
                formFields.SetField("text_56", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
                formFields.SetField("text_58", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
                //formFields.SetField("text_59", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
                formFields.SetField("text_62", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
                formFields.SetField("text_64", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
                formFields.SetField("text_66", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

                // Hallinnollinen asiakirja, Väestörekisteriote
                formFields.SetField("checkbox_74", chkboxValittu);
                formFields.SetField("checkbox_78", chkboxValittu);

                formFields.SetField("text_84", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
                formFields.SetField("text_89", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

                // ASIANOMAISEN HENKILÖN TIEDOT
                // Nimet
                formFields.SetField("text_105", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
                formFields.SetField("text_109", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
                formFields.SetField("text_117", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

                // Entiset sukunimet
                string entSukunimi = string.Empty;
                foreach (Taulukko sukunimi in tiedot.entisetSukunimet)
                {
                    if (entSukunimi == string.Empty)
                        entSukunimi = sukunimi.Sarake2;
                    else
                        entSukunimi = entSukunimi + ", " + sukunimi.Sarake2;
                }
                formFields.SetField("text_106", (string.IsNullOrEmpty(entSukunimi) ? tyhja : entSukunimi));

                // Entiset etunimet
                string entEtunimi = string.Empty;
                foreach (Taulukko etunimi in tiedot.entisetEtunimet)
                {
                    if (entEtunimi == string.Empty)
                        entEtunimi = etunimi.Sarake2;
                    else
                        entEtunimi = entEtunimi + ", " + etunimi.Sarake2;
                }
                formFields.SetField("text_113", (string.IsNullOrEmpty(entEtunimi) ? tyhja : entEtunimi));

                // Syntymäaika
                formFields.SetField("text_118", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

                // Osoite
                foreach (Osoite osoite in tiedot.vakinaisetOsoitteet)
                {
                    formFields.SetField("text_134", (string.IsNullOrEmpty(osoite.Lahiosoite) ? tyhja : osoite.Lahiosoite));
                    formFields.SetField("text_135", (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? tyhja : osoite.Postitoimipaikka));
                    formFields.SetField("text_136", tyhja); //osoite.Maa ???
                    formFields.SetField("text_137", (string.IsNullOrEmpty(osoite.Alkupaiva) ? tyhja : osoite.Alkupaiva.Replace(merkit[0], merkit[1])));
                    break;
                }

                // Kotikunta
                switch (tiedot.Kotikunta)
                {
                    case "Ulkomaat":
                    case "Utlandet":
                        formFields.SetField("checkbox_142", chkboxValittu);
                        break;
                    case "TUNTEMATON":
                    case "Okänt":
                        formFields.SetField("checkbox_143", chkboxValittu);
                        break;
                    case "Ei kotikuntaa Suomessa":
                    case "Ej hemkommun i Finland":
                        formFields.SetField("checkbox_144", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("text_141", (string.IsNullOrEmpty(tiedot.Kotikunta) ? tyhja : tiedot.Kotikunta.Replace(merkit[0], merkit[1])));
                        break;
                }

                // Henkilötunnus
                formFields.SetField("text_150", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

                // ALLEKIRJOITUSTIEDOT
                formFields.SetField("text_240", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
                formFields.SetField("text_241", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

                formFields.SetField("text_242", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            }
        }
        #endregion

        #region Avioliittotodistus
        // AVIOLIITTOTODISTUS
        else if (tiedot.Todistus == "FI_Marriage")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_73", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            //formFields.SetField("text_77", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
            formFields.SetField("text_81", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_90", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_93", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_106", chkboxValittu);
            formFields.SetField("checkbox_121", chkboxValittu);

            formFields.SetField("text_129", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_137", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // AVIOLIITON TIEDOT //
            foreach (Avioliitto avioliitto in tiedot.avioliitot)
            {
                // Avioliiton solmimispäivä
                formFields.SetField("text_183", (string.IsNullOrEmpty(avioliitto.Alkupaiva) ? tyhja : avioliitto.Alkupaiva.Replace(merkit[0], merkit[1])));
                // Avioliiton järjestysnumero
                formFields.SetField("text_468", (string.IsNullOrEmpty(avioliitto.Jarjestysnumero) ? tyhja : avioliitto.Jarjestysnumero));

                // PUOLISON B TIEDOT, Syntymäaika
                formFields.SetField("text_514", (string.IsNullOrEmpty(avioliitto.Syntymaaika) ? tyhja : avioliitto.Syntymaaika.Replace(merkit[0], merkit[1])));
                formFields.SetField("text_502", (string.IsNullOrEmpty(avioliitto.EntinenSukunimiYli15v) ? tyhja : avioliitto.EntinenSukunimiYli15v));
                formFields.SetField("text_503", (string.IsNullOrEmpty(avioliitto.NykyinenSukunimi) ? tyhja : avioliitto.NykyinenSukunimi));
                formFields.SetField("text_504", (string.IsNullOrEmpty(avioliitto.NykyisetEtunimet) ? tyhja : avioliitto.NykyisetEtunimet));

                break;
            }

            // PUOLISON A TIEDOT
            // Nimet
            formFields.SetField("text_292", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_293", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_302", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Entiset sukunimet yli 15v
            string entSukunimi = string.Empty;
            foreach (Taulukko sukunimi in tiedot.entisetSukunimimetYli15v)
            {
                if (entSukunimi == string.Empty)
                    entSukunimi = sukunimi.Sarake2;
                else
                    entSukunimi = entSukunimi + ", " + sukunimi.Sarake2;
            }
            formFields.SetField("text_291", (string.IsNullOrEmpty(entSukunimi) ? tyhja : entSukunimi));

            // Entiset etunimet yli 15v
            string entEtunimi = string.Empty;
            foreach (Taulukko etunimi in tiedot.entisetEtunimetYli15v)
            {
                if (entEtunimi == string.Empty)
                    entEtunimi = etunimi.Sarake2;
                else
                    entEtunimi = entEtunimi + ", " + etunimi.Sarake2;
            }
            formFields.SetField("text_300", (string.IsNullOrEmpty(entEtunimi) ? tyhja : entEtunimi));

            // Syntymäaika
            formFields.SetField("text_303", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Henkilötunnus
            formFields.SetField("text_354", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Sukupuoli
            switch (tiedot.Sukupuoli)
            {
                case "Nainen":
                case "Kvinna":
                    formFields.SetField("checkbox_335", chkboxValittu);
                    break;
                case "Mies":
                case "Man":
                    formFields.SetField("checkbox_336", chkboxValittu);
                    break;
                default:
                    formFields.SetField("checkbox_337", chkboxValittu);
                    break;
            }

            // Siviilisääty
            switch (tiedot.Siviilisaaty)
            {
                case "Naimaton":
                case "Ogift":
                    formFields.SetField("checkbox_428", chkboxValittu);
                    break;
                case "Avioliitossa":
                case "Gift":
                    formFields.SetField("checkbox_431", chkboxValittu);
                    break;
                case "Asumuserossa":
                case "Hemskild":
                    formFields.SetField("checkbox_432", chkboxValittu);
                    break;
                case "Eronnut":
                case "Frånskild":
                    formFields.SetField("checkbox_433", chkboxValittu);
                    break;
                case "Leski":
                case "Änka/änkling":
                    formFields.SetField("checkbox_434", chkboxValittu);
                    break;
                case "Rekisteröidyssä parisuhteessa":
                case "Registrerad partner":
                    formFields.SetField("checkbox_435", chkboxValittu);
                    break;
                case "Eronnut rekisteröidystä parisuhteesta":
                case "Skild från registrerad partner":
                    formFields.SetField("checkbox_436", chkboxValittu);
                    break;
                case "Leski rekisteröidystä parisuhteesta":
                case "Efterlevande partner (registrerat partnerskap)":
                    formFields.SetField("checkbox_437", chkboxValittu);
                    break;
                case "Ei tietoa":
                case "Okänt":
                    formFields.SetField("checkbox_438", chkboxValittu);
                    break;
            }

            // Kotikunta
            switch (tiedot.Kotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_462", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänt":
                    formFields.SetField("checkbox_465", chkboxValittu);
                    break;
                case "Ei kotikuntaa Suomessa":
                case "Ej hemkommun i Finland":
                    formFields.SetField("checkbox_466", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_458", (string.IsNullOrEmpty(tiedot.Kotikunta) ? tyhja : tiedot.Kotikunta.Replace(merkit[0], merkit[1])));
                    break;
            }

            // Osoite
            foreach (Osoite osoite in tiedot.vakinaisetOsoitteet)
            {
                formFields.SetField("text_477", (string.IsNullOrEmpty(osoite.Lahiosoite) ? "" : osoite.Lahiosoite + " ")
                                              + (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? "" : osoite.Postitoimipaikka));
                formFields.SetField("text_485", (string.IsNullOrEmpty(osoite.Alkupaiva) ? tyhja : osoite.Alkupaiva.Replace(merkit[0], merkit[1])));
                break;
            }

            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_824", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
            formFields.SetField("text_825", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_826", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
        }
        #endregion

        #region Asuinpaikkatodistus
        // ASUINPAIKKATODISTUS
        else if (tiedot.Todistus == "FI_DomicileAndOrResidence")
        {
            // OTTEEN ANTAJAN TIEDOT
            formFields.SetField("text_63", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            formFields.SetField("text_65", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
            //formFields.SetField("text_67", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
            formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
            formFields.SetField("text_72", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
            formFields.SetField("text_74", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

            // Hallinnollinen asiakirja, Väestörekisteriote
            formFields.SetField("checkbox_83", chkboxValittu);
            formFields.SetField("checkbox_91", chkboxValittu);

            formFields.SetField("text_98", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_102", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

            // ASIANOMAISEN HENKILÖN TIEDOT
            // Nimet
            formFields.SetField("text_136", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
            formFields.SetField("text_142", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
            formFields.SetField("text_152", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

            // Entiset sukunimet yli 15v
            string entSukunimiYli15v = string.Empty;
            foreach (Taulukko sukunimi in tiedot.entisetSukunimimetYli15v)
            {
                if (entSukunimiYli15v == string.Empty)
                    entSukunimiYli15v = sukunimi.Sarake2;
                else
                    entSukunimiYli15v = entSukunimiYli15v + ", " + sukunimi.Sarake2;
            }
            formFields.SetField("text_141", (string.IsNullOrEmpty(entSukunimiYli15v) ? tyhja : entSukunimiYli15v));

            // Entiset etunimet yli 15v
            string entEtunimiYli15v = string.Empty;
            foreach (Taulukko etunimi in tiedot.entisetEtunimetYli15v)
            {
                if (entEtunimiYli15v == string.Empty)
                    entEtunimiYli15v = etunimi.Sarake2;
                else
                    entEtunimiYli15v = entEtunimiYli15v + ", " + etunimi.Sarake2;
            }
            formFields.SetField("text_150", (string.IsNullOrEmpty(entEtunimiYli15v) ? tyhja : entEtunimiYli15v));

            // Syntymäaika
            formFields.SetField("text_153", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Henkilötunnus
            formFields.SetField("text_169", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

            // Siviilisääty
            switch (tiedot.Siviilisaaty)
            {
                case "Naimaton":
                case "Ogift":
                    formFields.SetField("checkbox_207", chkboxValittu);
                    break;
                case "Avioliitossa":
                case "Gift":
                    formFields.SetField("checkbox_210", chkboxValittu);
                    break;
                case "Asumuserossa":
                case "Hemskild":
                    formFields.SetField("checkbox_212", chkboxValittu);
                    break;
                case "Eronnut":
                case "Frånskild":
                    formFields.SetField("checkbox_213", chkboxValittu);
                    break;
                case "Leski":
                case "Änka/änkling":
                    formFields.SetField("checkbox_214", chkboxValittu);
                    break;
                case "Rekisteröidyssä parisuhteessa":
                case "Registrerad partner":
                    formFields.SetField("checkbox_215", chkboxValittu);
                    break;
                case "Eronnut rekisteröidystä parisuhteesta":
                case "Skild från registrerad partner":
                    formFields.SetField("checkbox_216", chkboxValittu);
                    break;
                case "Leski rekisteröidystä parisuhteesta":
                case "Efterlevande partner (registrerat partnerskap)":
                    formFields.SetField("checkbox_217", chkboxValittu);
                    break;
                case "Ei tietoa":
                case "Okänt":
                    formFields.SetField("checkbox_218", chkboxValittu);
                    break;
            }

            // Vakinainen osoite
            foreach (Osoite osoite in tiedot.vakinaisetOsoitteet)
            {
                formFields.SetField("checkbox_269", chkboxValittu);
                formFields.SetField("text_274", (string.IsNullOrEmpty(osoite.Lahiosoite) ? tyhja : osoite.Lahiosoite)) ;
                formFields.SetField("text_277", (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? tyhja : osoite.Postitoimipaikka));
                formFields.SetField("text_283", tyhja); //osoite.Maa ???
                formFields.SetField("text_284", (string.IsNullOrEmpty(osoite.Alkupaiva) ? tyhja : osoite.Alkupaiva.Replace(merkit[0], merkit[1])));
                break;
            }

            // Kotikunta
            switch (tiedot.Kotikunta)
            {
                case "Ulkomaat":
                case "Utlandet":
                    formFields.SetField("checkbox_287", chkboxValittu);
                    break;
                case "TUNTEMATON":
                case "Okänt":
                    formFields.SetField("checkbox_288", chkboxValittu);
                    break;
                case "Ei kotikuntaa Suomessa":
                case "Ej hemkommun i Finland":
                    formFields.SetField("checkbox_289", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_286", (string.IsNullOrEmpty(tiedot.Kotikunta) ? tyhja : tiedot.Kotikunta.Replace(merkit[0], merkit[1])));
                    break;
            }

            // Edelliset vakinaiset osoitteet
            string entVakOsoite = string.Empty;
            foreach (Osoite osoite in tiedot.entisetVakinaisetOsoitteet)
            {
                if (entVakOsoite != string.Empty)
                    entVakOsoite = entVakOsoite + ", ";

                entVakOsoite = entVakOsoite
                    + (string.IsNullOrEmpty(osoite.Lahiosoite) ? "" : osoite.Lahiosoite + " ") 
                    + (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? "" : osoite.Postitoimipaikka + " ") 
                    + (string.IsNullOrEmpty(osoite.Voimassaoloaika) ? "" : osoite.Voimassaoloaika.Replace(merkit[0], merkit[1]));
            }
            formFields.SetField("text_294", (string.IsNullOrEmpty(entVakOsoite) ? tyhja : entVakOsoite));

            // Entiset kotikunnat
            string entKotikunta = string.Empty;
            foreach (Taulukko kotikunta in tiedot.entisetKotikunnat)
            {
                if (entKotikunta != string.Empty)
                    entKotikunta = entKotikunta + ", ";
                
                entKotikunta = entKotikunta 
                    + (string.IsNullOrEmpty(kotikunta.Sarake2) ? "" : kotikunta.Sarake2 + " ") 
                    + (string.IsNullOrEmpty(kotikunta.Sarake1) ? "" : kotikunta.Sarake1.Replace(merkit[0], merkit[1]));
            }
            formFields.SetField("text_318", (string.IsNullOrEmpty(entKotikunta) ? tyhja : entKotikunta));

            // Tilapäinen osoite
            string tilOsoite = string.Empty;
            foreach (Osoite osoite in tiedot.tilapaisetOsoitteet)
            {
                if (tilOsoite != string.Empty)
                    tilOsoite = tilOsoite + ", ";

                tilOsoite = tilOsoite
                    + (string.IsNullOrEmpty(osoite.Lahiosoite) ? "" : osoite.Lahiosoite + " ")
                    + (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? "" : osoite.Postitoimipaikka + " ")
                    + (string.IsNullOrEmpty(osoite.Voimassaoloaika) ? "" : osoite.Voimassaoloaika.Replace(merkit[0], merkit[1]));
            }
            formFields.SetField("text_338", (string.IsNullOrEmpty(tilOsoite) ? tyhja : tilOsoite));

            // Entiset tilapäiset osoitteet
            string entTilOsoite = string.Empty;
            foreach (Osoite osoite in tiedot.entisetTilapaisetOsoitteet)
            {
                if (entTilOsoite != string.Empty)
                    entTilOsoite = entTilOsoite + ", ";
                
                entTilOsoite = entTilOsoite
                    + (string.IsNullOrEmpty(osoite.Lahiosoite) ? "" : osoite.Lahiosoite + " ")
                    + (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? "" : osoite.Postitoimipaikka + " ")
                    + (string.IsNullOrEmpty(osoite.Voimassaoloaika) ? "" : osoite.Voimassaoloaika.Replace(merkit[0], merkit[1]));
            }
            formFields.SetField("text_347", (string.IsNullOrEmpty(entTilOsoite) ? tyhja : entTilOsoite));
            
            // Postiosoite
            string postiosoite = string.Empty;
            foreach (Osoite osoite in tiedot.postiosoitteet)
            {
                if (postiosoite != string.Empty)
                    postiosoite = postiosoite + ", ";
                
                postiosoite = postiosoite
                    + (string.IsNullOrEmpty(osoite.Lahiosoite) ? "" : osoite.Lahiosoite + " ")
                    + (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? "" : osoite.Postitoimipaikka + " ")
                    + (string.IsNullOrEmpty(osoite.Voimassaoloaika) ? "" : osoite.Voimassaoloaika.Replace(merkit[0], merkit[1]));                    
            }
            formFields.SetField("text_352", (string.IsNullOrEmpty(postiosoite) ? tyhja : postiosoite));            

            // ALLEKIRJOITUSTIEDOT
            formFields.SetField("text_430", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
            formFields.SetField("text_431", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

            formFields.SetField("text_432", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));

        }
        #endregion


        #region Kuolintodistus
        // KUOLINTODISTUS
        if (tiedot.Todistus == "FI_Death")
        {
            // tulostetaan täytetty pdf vain jos henkilö on kuollut 
            if (tiedot.Elossaolotieto == string.Empty)
            {
                // OTTEEN ANTAJAN TIEDOT
                formFields.SetField("text_71", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
                formFields.SetField("text_73", (string.IsNullOrEmpty(tiedot.ViranomainenNimi) ? tyhja : tiedot.ViranomainenNimi));
                //formFields.SetField("text_76", (string.IsNullOrEmpty(tiedot.ViranomainenLahiosoite) ? tyhja : tiedot.ViranomainenLahiosoite));
                formFields.SetField("text_75", (string.IsNullOrEmpty(tiedot.ViranomainenPostiosoite) ? tyhja : tiedot.ViranomainenPostiosoite));
                formFields.SetField("text_92", (string.IsNullOrEmpty(tiedot.ViranomainenPostitmp) ? tyhja : tiedot.ViranomainenPostitmp));
                formFields.SetField("text_97", (string.IsNullOrEmpty(tiedot.ViranomainenPuhelin) ? tyhja : tiedot.ViranomainenPuhelin));

                // Hallinnollinen asiakirja, Väestörekisteriote
                formFields.SetField("checkbox_111", chkboxValittu);
                formFields.SetField("checkbox_126", chkboxValittu);

                formFields.SetField("text_date_135", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
                formFields.SetField("text_139", (string.IsNullOrEmpty(tiedot.Antamispaikka) ? tyhja : tiedot.Antamispaikka));

                // KUOLLEEN HENKILÖN TIEDOT //
                // Nimet
                formFields.SetField("text_211", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
                formFields.SetField("text_221", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));
                formFields.SetField("text_232", (string.IsNullOrEmpty(tiedot.Kutsumanimi) ? tyhja : tiedot.Kutsumanimi));

                // Entiset sukunimet
                string entSukunimi = string.Empty;
                foreach (Taulukko sukunimi in tiedot.entisetSukunimet)
                {
                    if (entSukunimi == string.Empty)
                        entSukunimi = sukunimi.Sarake2;
                    else
                        entSukunimi = entSukunimi + ", " + sukunimi.Sarake2;
                }
                formFields.SetField("text_214", (string.IsNullOrEmpty(entSukunimi) ? tyhja : entSukunimi));

                // Entiset etunimet
                string entEtunimet = string.Empty;
                foreach (Taulukko etunimet in tiedot.entisetEtunimet)
                {
                    if (entEtunimet == string.Empty)
                        entEtunimet = etunimet.Sarake2;
                    else
                        entEtunimet = entEtunimet + ", " + etunimet.Sarake2;
                }
                formFields.SetField("text_224", (string.IsNullOrEmpty(entEtunimet) ? tyhja : entEtunimet));

                // Kuolinpäivä
                formFields.SetField("text_date_233", (string.IsNullOrEmpty(tiedot.Kuolinpaiva) ? tyhja : tiedot.Kuolinpaiva.Replace(merkit[0], merkit[1])));

                // Syntymäaika
                formFields.SetField("text_date_255", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika.Replace(merkit[0], merkit[1])));

                // Syntymäpaikka
                formFields.SetField("text_269", (string.IsNullOrEmpty(tiedot.Syntymapaikka) ? tyhja : tiedot.Syntymapaikka));

                // Syntymävaltio
                formFields.SetField("text_282", (string.IsNullOrEmpty(tiedot.Syntymavaltio) ? tyhja : tiedot.Syntymavaltio));

                // Syntymäkotikunta
                switch (tiedot.Syntymakotikunta)
                {
                    case "Ulkomaat":
                    case "Utlandet":
                        formFields.SetField("checkbox_286", chkboxValittu);
                        break;
                    case "TUNTEMATON":
                    case "Okänd":
                        formFields.SetField("checkbox_287", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("text_285", (string.IsNullOrEmpty(tiedot.Syntymakotikunta) ? tyhja : tiedot.Syntymakotikunta));
                        break;
                }

                // Sukupuoli
                switch (tiedot.Sukupuoli)
                {
                    case "Nainen":
                    case "Kvinna":
                        formFields.SetField("checkbox_289", chkboxValittu);
                        break;
                    case "Mies":
                    case "Man":
                        formFields.SetField("checkbox_290", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("checkbox_291", chkboxValittu);
                        break;
                }

                // Henkilötunnus 
                formFields.SetField("text_319", (string.IsNullOrEmpty(tiedot.Henkilotunnus) ? tyhja : tiedot.Henkilotunnus));

                // Vakinainen osoite
                foreach (Osoite osoite in tiedot.vakinaisetOsoitteet)
                {
                    formFields.SetField("text_371", (string.IsNullOrEmpty(osoite.Lahiosoite) ? tyhja : osoite.Lahiosoite));
                    formFields.SetField("text_375", (string.IsNullOrEmpty(osoite.Postitoimipaikka) ? tyhja : osoite.Postitoimipaikka));
                    formFields.SetField("text_date_383", (string.IsNullOrEmpty(osoite.Alkupaiva) ? tyhja : osoite.Alkupaiva.Replace(merkit[0], merkit[1])));
                    break;
                }

                // Rekisteriviranomainen
                if (!string.IsNullOrEmpty(tiedot.Rekisterinpitaja))
                {
                    switch (tiedot.Kotikunta)
                    {
                        case "Ei väestörekisterinpitäjää":
                        case "Ej befolkningsregisterförare":
                            formFields.SetField("checkbox_389", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_388", tiedot.Rekisterinpitaja.Replace(merkit[0], merkit[1]));
                            break;
                    }
                }

                // Kotikunta
                if (!string.IsNullOrEmpty(tiedot.Kotikunta))
                {
                    switch (tiedot.Kotikunta)
                    {
                        case "Ei kotikuntaa Suomessa":
                        case "Ej hemkommun i Finland":
                            formFields.SetField("checkbox_394", chkboxValittu);
                            break;
                        case "Ulkomaat":
                        case "Utlandet":
                            formFields.SetField("checkbox_392", chkboxValittu);
                            break;
                        case "TUNTEMATON":
                        case "Okänt":
                            formFields.SetField("checkbox_393", chkboxValittu);
                            break;
                        default:
                            formFields.SetField("text_390", tiedot.Kotikunta.Replace(merkit[0], merkit[1]));
                            break;
                    }
                }

                // Kansalaisuus
                switch (tiedot.Kansalaisuus)
                {
                    case "Ei selvit":
                    case "Ei selvit (99)":
                    case "Ej uträtt":
                    case "Ej uträtt (99)":
                        formFields.SetField("checkbox_425", chkboxValittu);
                        break;
                    case "Tuntematon":
                    case "Tuntematon (99)":
                    case "Okänt":
                    case "Okänt (99)":
                        formFields.SetField("checkbox_428", chkboxValittu);
                        break;
                    default:
                        formFields.SetField("text_408", (string.IsNullOrEmpty(tiedot.Kansalaisuus) ? tyhja : tiedot.Kansalaisuus));
                        break;
                }

                // Siviilisääty
                switch (tiedot.Siviilisaaty)
                {
                    case "Naimaton":
                    case "Ogift":
                        formFields.SetField("checkbox_452", chkboxValittu);
                        break;
                    case "Avioliitossa":
                    case "Gift":
                        formFields.SetField("checkbox_457", chkboxValittu);
                        break;
                    case "Asumuserossa":
                    case "Hemskild":
                        formFields.SetField("checkbox_470", chkboxValittu);
                        break;
                    case "Eronnut":
                    case "Frånskild":
                        formFields.SetField("checkbox_473", chkboxValittu);
                        break;
                    case "Leski":
                    case "Änka/änkling":
                        formFields.SetField("checkbox_476", chkboxValittu);
                        break;
                    case "Rekisteröidyssä parisuhteessa":
                    case "Registrerad partner":
                        formFields.SetField("checkbox_478", chkboxValittu);
                        break;
                    case "Eronnut rekisteröidystä parisuhteesta":
                    case "Skild från registrerad partner":
                        formFields.SetField("checkbox_480", chkboxValittu);
                        break;
                    case "Leski rekisteröidystä parisuhteesta":
                    case "Efterlevande partner (registrerat partnerskap)":
                        formFields.SetField("checkbox_481", chkboxValittu);
                        break;
                    case "Ei tietoa":
                    case "Okänt":
                        formFields.SetField("checkbox_482", chkboxValittu);
                        break;
                }

                // ALLEKIRJOITUSTIEDOT
                formFields.SetField("text_616", (string.IsNullOrEmpty(tiedot.VirkamiehenNimi) ? tyhja : tiedot.VirkamiehenNimi));
                formFields.SetField("text_617", (string.IsNullOrEmpty(tiedot.Virkanimike) ? tyhja : tiedot.Virkanimike));

                formFields.SetField("text_date_618", (string.IsNullOrEmpty(tiedot.Antamispaiva) ? tyhja : tiedot.Antamispaiva.Replace(merkit[0], merkit[1])));
            }
        }
        #endregion


        stamper.FormFlattening = false; //Form fields should no longer be editable
        stamper.Close();
        reader.Close();

        return output.ToArray();
    }

    private string vainPaivamaara(string teksti)
    {
        char[] merkit = new char[] { '.', '/' };
        string pvm = "-";

        if (!string.IsNullOrEmpty(teksti))
        {
            int indx = teksti.IndexOf(merkit[0]) - 2;
            if (indx > -1)
            {
                pvm = teksti.Substring(indx, 10);
                pvm = pvm.Replace(merkit[0], merkit[1]);
            }
        }

        return pvm;
    }


}
