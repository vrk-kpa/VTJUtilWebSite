using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

/// <summary>
/// Summary description for TodistusToPdf
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class TodistusToPdf : System.Web.Services.WebService {

    private BaseColor c_1d1d1b = new BaseColor(29, 29, 27);
    private BaseColor c_3c3c3b = new BaseColor(60, 60, 59);
    private BaseColor c_575756 = new BaseColor(87, 87, 86);
    private BaseColor c_6f6f6e = new BaseColor(111, 111, 110);
    private BaseColor c_e9e9f2 = new BaseColor(233, 233, 242);
    private BaseColor c_ffffff = new BaseColor(255, 255, 255);

    private static string fontit = System.Configuration.ConfigurationManager.AppSettings["OTE_fontit"];

    private BaseFont f_fira_medium = BaseFont.CreateFont(fontit + "FiraMono-Medium.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    private BaseFont f_fira_regular = BaseFont.CreateFont(fontit + "FiraMono-Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    
    private BaseFont f_ssans_regular = BaseFont.CreateFont(fontit + "SourceSansPro-Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    private BaseFont f_ssans_light = BaseFont.CreateFont(fontit + "SourceSansPro-Light.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

    private float[] padding_0_0_0 = new float[] { 0, 0, 0 };
    private float[] padding_0_0_10 = new float[] { 0, 0, 10 };
    private float[] padding_0_0_20 = new float[] { 0, 0, 20 };
    private float[] padding_0_20_0 = new float[] { 0, 20, 0 };
    private float[] padding_20_0_0 = new float[] { 20, 0, 3 };
    private float[] padding_20_0_10 = new float[] { 20, 0, 10 };
    private float[] padding_20_20_0 = new float[] { 20, 20, 3 };

    // Tietojen asemointi pystysuunnassa, alkuarvo
    private float YPositio = 720;
    private float YPositioAlkuarvo = 720;

    // Logot
    private static string DVV_logo_FIN = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_FIN"];
    private static string DVV_logo_SWE = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_SWE"];
    private static string DVV_logo_ENG = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_ENG"];

    //private static string VRK_logo = System.Configuration.ConfigurationManager.AppSettings["VRK_logo"];
    //private static string MTI_logo = System.Configuration.ConfigurationManager.AppSettings["MTI_logo"];

    // Onko kyseessä todistus, jonka yhteydessä mahdollista tulostaa EU-vakiolomake
    private bool bTodistusJaVakiolomake = false;

    // Onko kyseessä todistus avioliittokelpoisuudesta, jolloin olemassaoleva pdf-lomakepohja täytetään tiedoilla
    private bool bTodistusAvioliittokelpoisuudesta = false;

    public TodistusToPdf () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public XmlDocument OtePdfmuodossa(OtteenTiedot oteData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        
        string pdfString = string.Empty;
        byte[] pdfByte = null;

        string pdfString_vakiolomake = string.Empty;
        byte[] pdfByte_vakiolomake = null;

        string xmlString = string.Empty;


        // Onko kyse todistus ja vakiolomake -tuotteesta
        if ( oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.SYNTYMATODISTUS_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.SYNTYMATODISTUS2_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.ELOSSAOLOTODISTUS_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.ASUINPAIKKATODISTUS_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.AVIOLIITTOTODISTUS_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.AVIOLIITTOTODISTUS2_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.SIVIILISAATYTODISTUS_VAKIOLOMAKE))
          || oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.KUOLINTODISTUS_VAKIOLOMAKE))
            )
        {
            bTodistusJaVakiolomake = true;
        }

        if (oteData.KyseltyTuote.Equals(new Guid(TodistusTuote.AVIOLIITTOKELPOISUUSTODISTUS_VAKIOLOMAKE)))
        {
            bTodistusAvioliittokelpoisuudesta = true;
            bTodistusJaVakiolomake = true;
        }

        // Todistus
        if (bTodistusAvioliittokelpoisuudesta)
            pdfByte = TaytaPdf(oteData);
        else
            pdfByte = MuodostaPdf(oteData);

        pdfString = Convert.ToBase64String(pdfByte); //Base64 Encoding


        if (!bTodistusJaVakiolomake)
        {
            // Todistus + tiiviste
            string tiiviste = LaskeTiiviste(pdfString);
            pdfByte = LisaaTiiviste(pdfByte, tiiviste, oteData);
            pdfString = Convert.ToBase64String(pdfByte); //Base64 Encoding

            // Tiivisteen tallennus SQL-server kantaan
            DateTime luovutusaika = Convert.ToDateTime(oteData.VtjPvm + " " + oteData.VtjKlo);
            TallennaTiiviste(tiiviste, luovutusaika, oteData.KyseltyHenkilotunnus, oteData.KyseltyTuote);

            // XML-vastauksen kasaus
            xmlString = "<Todistus>"
                      + "<Tiiviste>" + tiiviste + "</Tiiviste>"
                      + "<Pdf>" + pdfString + "</Pdf>"
                      + "</Todistus>";
        }
        else
        {
            // Eu-vakiolomake
            if (!string.IsNullOrWhiteSpace(oteData.Kohdemaa) && oteData.Tulostuskieli != "3")
            {
                TodistusToVakiolomake euVakiolomake = new TodistusToVakiolomake();
                pdfByte_vakiolomake = euVakiolomake.MuodostaVakiolomakePdf(oteData);
                pdfString_vakiolomake = Convert.ToBase64String(pdfByte_vakiolomake);
            }

            // XML-vastauksen kasaus
            xmlString = "<Todistus>"
                      + "<Pdf>" + pdfString + "</Pdf>"
                      + "<VakiolomakePdf>" + pdfString_vakiolomake + "</VakiolomakePdf>"
                      + "</Todistus>";
        }


        xmlDoc.LoadXml(xmlString);



        //// **************************************************************************************
        //// Testausta varten, Pdf kirjoitus tiedostoon
        ////
        //byte[] data = Convert.FromBase64String(pdfString);  // Decoding
        //string pdfFile = @"C:\Temp\VTJkysely\PDF\Ote_FromBase64String.pdf";
        //using (BinaryWriter writer = new BinaryWriter(File.Open(pdfFile, FileMode.Create)))
        //{
        //    writer.Write(data);
        //}
        //// **************************************************************************************



        return xmlDoc;
    }


    private byte[] MuodostaPdf(OtteenTiedot ote)
    {

        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            // PDF-tagit
            writer.SetTagged();

            // Dokumentin kielisyys
            string lang = ote.Tulostuskieli == "1" ? "fi-FI" : ote.Tulostuskieli == "2" ? "sv-SE" : "en-US";
            writer.SetLanguage(lang);
            document.AddLanguage(lang);

            // Dokumentin otsikko
            document.AddTitle(ote.Ote);

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

        // Otteen nimi ja käyttötarkoitus
            PdfPTable table01 = new PdfPTable(1);
            table01.SetTotalWidth(new float[] { 400 });

            if (!string.IsNullOrEmpty(ote.Kayttotarkoitus))
            {
                table01.AddCell(CreateCell(ote.Ote, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_0_0));
                table01.AddCell(CreateCell(ote.Kayttotarkoitus_otsikko + ": " + ote.Kayttotarkoitus, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_0_10));
            }
            else
            {
                table01.AddCell(CreateCell(ote.Ote, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_0_10));
            }
            table01.WriteSelectedRows(0, -1, 37, YPositio, cb);

            YPositio = YPositio - table01.TotalHeight;


        // -----------------------------------------------------
        // HENKILÖTIEDOT
        // -----------------------------------------------------
        
            PdfPTable table1 = new PdfPTable(2); 
            table1.SetTotalWidth(new float[] { 145, 380 });

        // Elossaolotieto
            table1.AddCell(CreateCell("", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
            table1.AddCell(CreateCell(ote.Elossaolotieto, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));

        // Nimet
            table1.AddCell(CreateCell(ote.NykyinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
            table1.AddCell(CreateCell(ote.NykyinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));

            table1.AddCell(CreateCell(ote.NykyisetEtunimet_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.NykyisetEtunimet, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Kutsumanimi
            if (!string.IsNullOrWhiteSpace(ote.Kutsumanimi))
            {
                table1.AddCell(CreateCell(ote.Kutsumanimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Kutsumanimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

        // Henkilötunnus
            table1.AddCell(CreateCell(ote.Henkilotunnus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Henkilotunnus, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Syntymäaika
            table1.AddCell(CreateCell(ote.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Kuolinpäivä
            table1.AddCell(CreateCell(ote.Kuolinpaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Kuolinpaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Sukupuoli
            table1.AddCell(CreateCell(ote.Sukupuoli_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Sukupuoli, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Siviilisääty
            table1.AddCell(CreateCell(ote.Siviilisaaty_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Siviilisaaty, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Siviilisääty + pvm
            table1.AddCell(CreateCell(ote.SiviilisaatyPvm_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.SiviilisaatyPvm, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Syntymäkotikunta
            table1.AddCell(CreateCell(ote.Syntymakotikunta_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Syntymakotikunta, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Syntymävaltio
            table1.AddCell(CreateCell(ote.Syntymavaltio_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Syntymavaltio, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Ulkomainen syntymäpaikka
        if (!string.IsNullOrWhiteSpace(ote.Syntymapaikka))
        {
            table1.AddCell(CreateCell(ote.Syntymapaikka_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Syntymapaikka, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
        }

        // Kansalaisuus
            table1.AddCell(CreateCell(ote.Kansalaisuus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Kansalaisuus, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

        // Vakinainen osoite
            foreach (TaulukkotietoOsoite vakinainenOsoite in ote.vakinaisetOsoitteet)
            {
                table1.AddCell(CreateCell(vakinainenOsoite.Lahiosoite_otsikko , f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
                table1.AddCell(CreateCell(vakinainenOsoite.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                table1.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(vakinainenOsoite.Postitoimipaikka, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table1.AddCell(CreateCell(vakinainenOsoite.Alkupaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(vakinainenOsoite.Alkupaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                break;
            }

        // Kotikunta
            table1.AddCell(CreateCell(ote.Kotikunta_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
            table1.AddCell(CreateCell(ote.Kotikunta, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

            table1.WriteSelectedRows(0, -1, 37, YPositio, cb);

            YPositio = YPositio - table1.TotalHeight;


        // ---------------------------------------------------------
        // TAULUKKOTIEDOT
        // ---------------------------------------------------------

            bool ekarivi = true;
            int rivi = 0;

        // Tilapäiset osoitteet
            PdfPTable table11 = new PdfPTable(2);
            table11.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoOsoite tilapOsoite in ote.tilapaisetOsoitteet)
            {
                rivi++;
                if (rivi == 1)
                {
                    table11.AddCell(CreateCell(tilapOsoite.Lahiosoite_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
                    table11.AddCell(CreateCell(tilapOsoite.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                }
                else
                {
                    table11.AddCell(CreateCell(tilapOsoite.Lahiosoite_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table11.AddCell(CreateCell(tilapOsoite.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                table11.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table11.AddCell(CreateCell(tilapOsoite.Postitoimipaikka, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                if (rivi == ote.tilapaisetOsoitteet.Count)
                {
                    table11.AddCell(CreateCell(tilapOsoite.Voimassaoloaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table11.AddCell(CreateCell(tilapOsoite.Voimassaoloaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table11.AddCell(CreateCell(tilapOsoite.Voimassaoloaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table11.AddCell(CreateCell(tilapOsoite.Voimassaoloaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table11);


        // Turvakiellon ilmoitusteksti
            PdfPTable table12 = new PdfPTable(2);
            table12.SetTotalWidth(new float[] { 145, 380 });

            table12.AddCell(CreateCell("", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
            table12.AddCell(CreateCell(ote.Turvakieltoteksti, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));

            cb = LisaaTaulu(ref document, cb, ote, table12);


        // Entiset nimet
            PdfPTable table2 = new PdfPTable(3);
            table2.SetTotalWidth(new float[] { 145, 170, 210 });

            ekarivi = true;
            foreach (Taulukkotieto sukunimi in ote.entisetSukunimet)
            {
                if (!string.IsNullOrEmpty(sukunimi.Sarake1) || !string.IsNullOrEmpty(sukunimi.Sarake2))
                {
                    if (ekarivi)
                    {
                        table2.AddCell(CreateCell(sukunimi.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table2.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table2.AddCell(CreateCell(sukunimi.Sarake1, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table2.AddCell(CreateCell(sukunimi.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
            }

            ekarivi = true;
            foreach (Taulukkotieto etunimi in ote.entisetEtunimet)
            {
                if (!string.IsNullOrEmpty(etunimi.Sarake1) || !string.IsNullOrEmpty(etunimi.Sarake2))
                {
                    if (ekarivi)
                    {
                        table2.AddCell(CreateCell(etunimi.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table2.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table2.AddCell(CreateCell(etunimi.Sarake1, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table2.AddCell(CreateCell(etunimi.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
            }

            ekarivi = true;
            foreach (Taulukkotieto korjattuSukunimi in ote.korjatutSukunimet)
            {
                if (ekarivi)
                {
                    table2.AddCell(CreateCell(korjattuSukunimi.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table2.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table2.AddCell(CreateCell(korjattuSukunimi.Sarake1, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table2.AddCell(CreateCell(korjattuSukunimi.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            ekarivi = true;
            foreach (Taulukkotieto korjattuEtunimi in ote.korjatutEtunimet)
            {
                if (ekarivi)
                {
                    table2.AddCell(CreateCell(korjattuEtunimi.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    table2.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table2.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table2.AddCell(CreateCell(korjattuEtunimi.Sarake1, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table2.AddCell(CreateCell(korjattuEtunimi.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            cb = LisaaTaulu(ref document, cb, ote, table2);



        // Entiset kotikunnat
            PdfPTable table3 = new PdfPTable(3);
            table3.SetTotalWidth(new float[] { 145, 170, 210 });

            ekarivi = true;
            foreach (Taulukkotieto kotikunta in ote.entisetKotikunnat)
            {
                if (ekarivi)
                {
                    table3.AddCell(CreateCell(kotikunta.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table3.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    table3.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table3.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table3.AddCell(CreateCell(kotikunta.Sarake1, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table3.AddCell(CreateCell(kotikunta.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            cb = LisaaTaulu(ref document, cb, ote, table3);
            


        // Entiset vakinaiset osoitteet
            PdfPTable table4 = new PdfPTable(3);
            table4.SetTotalWidth(new float[] { 145, 170, 210 });

            ekarivi = true;
            foreach (TaulukkotietoOsoite entVakOsoite in ote.entisetVakinaisetOsoitteet)
            {
                if (ekarivi)
                {
                    table4.AddCell(CreateCell(entVakOsoite.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table4.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    table4.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table4.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table4.AddCell(CreateCell(entVakOsoite.Voimassaoloaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table4.AddCell(CreateCell(entVakOsoite.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                table4.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table4.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table4.AddCell(CreateCell(entVakOsoite.Postitoimipaikka, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            cb = LisaaTaulu(ref document, cb, ote, table4);



        // Entiset tilapaiset osoitteet
            PdfPTable table5 = new PdfPTable(3);
            table5.SetTotalWidth(new float[] { 145, 170, 210 });

            ekarivi = true;
            foreach (TaulukkotietoOsoite entTilOsoite in ote.entisetTilapaisetOsoitteet)
            {
                if (ekarivi)
                {
                    table5.AddCell(CreateCell(entTilOsoite.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table5.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    table5.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table5.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table5.AddCell(CreateCell(entTilOsoite.Voimassaoloaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table5.AddCell(CreateCell(entTilOsoite.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                table5.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table5.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table5.AddCell(CreateCell(entTilOsoite.Postitoimipaikka, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            cb = LisaaTaulu(ref document, cb, ote, table5);


        // Avioliitto
            PdfPTable table10 = new PdfPTable(2);
            table10.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoAvioliitto avioliitto in ote.avioliitot)
            {
                rivi++;
                if (rivi == 1)
                {
                    table10.AddCell(CreateCell(avioliitto.Alkupaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0));
                    table10.AddCell(CreateCell(avioliitto.Alkupaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                }
                else
                {
                    table10.AddCell(CreateCell(avioliitto.Alkupaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.Alkupaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                if (rivi == ote.avioliitot.Count)
                {
                    table10.AddCell(CreateCell(avioliitto.Jarjestysnumero_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.Jarjestysnumero, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyisetEtunimet_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyisetEtunimet, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.EntinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.EntinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table10.AddCell(CreateCell(avioliitto.Jarjestysnumero_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.Jarjestysnumero, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyisetEtunimet_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.NykyisetEtunimet, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table10.AddCell(CreateCell(avioliitto.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table10.AddCell(CreateCell(avioliitto.EntinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table10.AddCell(CreateCell(avioliitto.EntinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table10);


        // Entiset avioliitot
            PdfPTable table6 = new PdfPTable(2);
            table6.SetTotalWidth(new float[] { 145, 380 });

            ekarivi = true;
            foreach (Taulukkotieto avioliitto in ote.entisetAvioliitot)
            {
                if (ekarivi)
                {
                    table6.AddCell(CreateCell(avioliitto.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table6.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table6.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table6.AddCell(CreateCell(avioliitto.Sarake1 + " - " + avioliitto.Sarake2, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
            }

            cb = LisaaTaulu(ref document, cb, ote, table6);



        // Vanhemmat
            PdfPTable table7 = new PdfPTable(2);
            table7.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoVanhempi vanhempi in ote.vanhemmat)
            {
                rivi++;
                if (rivi == 1)
                {
                    table7.AddCell(CreateCell(vanhempi.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table7.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table7.AddCell(CreateCell(vanhempi.NykyinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.NykyinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table7.AddCell(CreateCell(vanhempi.NykyisetEtunimet_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.NykyisetEtunimet, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table7.AddCell(CreateCell(vanhempi.Henkilotunnus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.Henkilotunnus, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table7.AddCell(CreateCell(vanhempi.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                if (!string.IsNullOrWhiteSpace(vanhempi.EntinenSukunimi))
                {
                    table7.AddCell(CreateCell(vanhempi.EntinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table7.AddCell(CreateCell(vanhempi.EntinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                table7.AddCell(CreateCell(vanhempi.Syntymakotikunta_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.Syntymakotikunta, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table7.AddCell(CreateCell(vanhempi.Syntymavaltio_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table7.AddCell(CreateCell(vanhempi.Syntymavaltio, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                if (rivi == ote.vanhemmat.Count)
                {
                    table7.AddCell(CreateCell(vanhempi.Kansalaisuus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table7.AddCell(CreateCell(vanhempi.Kansalaisuus.Replace(";", ", "), f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table7.AddCell(CreateCell(vanhempi.Kansalaisuus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table7.AddCell(CreateCell(vanhempi.Kansalaisuus.Replace(";", ", "), f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table7);


            // Huoltajat
            PdfPTable table71 = new PdfPTable(2);
            table71.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoHuoltaja huoltaja in ote.huoltajat)
            {
                rivi++;
                if (rivi == 1)
                {
                    table71.AddCell(CreateCell(huoltaja.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table71.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                    ekarivi = false;
                }
                table71.AddCell(CreateCell(huoltaja.NykyinenSukunimi_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table71.AddCell(CreateCell(huoltaja.NykyinenSukunimi, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table71.AddCell(CreateCell(huoltaja.NykyisetEtunimet_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table71.AddCell(CreateCell(huoltaja.NykyisetEtunimet, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table71.AddCell(CreateCell(huoltaja.Henkilotunnus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table71.AddCell(CreateCell(huoltaja.Henkilotunnus, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                if (rivi == ote.huoltajat.Count)
                {
                    table71.AddCell(CreateCell(huoltaja.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table71.AddCell(CreateCell(huoltaja.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table71.AddCell(CreateCell(huoltaja.Syntymaaika_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table71.AddCell(CreateCell(huoltaja.Syntymaaika, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table71);


            // Edunvalvonnat
            PdfPTable table8 = new PdfPTable(2);
            table8.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoEdunvalvonta edunvalvonta in ote.edunvalvonnat)
            {
                rivi++;
                if (rivi == 1)
                {
                    table8.AddCell(CreateCell(edunvalvonta.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table8.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                }
                table8.AddCell(CreateCell(edunvalvonta.Rajoitus_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table8.AddCell(CreateCell(edunvalvonta.Rajoitus, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                table8.AddCell(CreateCell(edunvalvonta.Alkupaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table8.AddCell(CreateCell(edunvalvonta.Alkupaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                if (rivi == ote.edunvalvonnat.Count)
                {
                    table8.AddCell(CreateCell(edunvalvonta.TehtavienJako_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table8.AddCell(CreateCell(edunvalvonta.TehtavienJako, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table8.AddCell(CreateCell(edunvalvonta.TehtavienJako_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table8.AddCell(CreateCell(edunvalvonta.TehtavienJako, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table8);


            // Edunvalvontavaltuutus
            PdfPTable table9 = new PdfPTable(2);
            table9.SetTotalWidth(new float[] { 145, 380 });

            rivi = 0;
            foreach (TaulukkotietoEdunvalvontavaltuutus edunvalvontavaltuutus in ote.edunvalvontavaltuutukset)
            {
                rivi++;
                if (rivi == 1)
                {
                    table9.AddCell(CreateCell(edunvalvontavaltuutus.Otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                    table9.AddCell(CreateCell(" ", f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                }
                table9.AddCell(CreateCell(edunvalvontavaltuutus.Alkupaiva_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                table9.AddCell(CreateCell(edunvalvontavaltuutus.Alkupaiva, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                if (rivi == ote.edunvalvonnat.Count)
                {
                    table9.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_0));
                    table9.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                else
                {
                    table9.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako_otsikko, f_ssans_regular, 13, c_575756, c_e9e9f2, padding_20_0_10));
                    table9.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako, f_fira_medium, 11, c_1d1d1b, c_e9e9f2, padding_0_0_10));
                }
            }

            cb = LisaaTaulu(ref document, cb, ote, table9);

        // -------------------------------------------------------------------------------------------
        // TODISTUKSEN LOPPUTEKSTIT
        // -------------------------------------------------------------------------------------------

            cb = LisaaTyhjarivi(cb);
            if (YPositio < 320)
            {
                // kirjoitetaan uudelle sivulle
                cb = LisaaAlatunniste(cb, ote);
                document.NewPage();
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
            }

        // Todistusteksti, jakautuu kahdelle riville
            //cb.BeginText();
            //ColumnText ct = new ColumnText(cb);
            //ct.SetSimpleColumn(new Phrase(new Chunk(ote.Todistusteksti, new Font(f_ssans_regular, 13))), 37, 260, 380, 300, 15, Element.ALIGN_LEFT | Element.ALIGN_TOP);
            //ct.Go();
            //cb.EndText();

            PdfPTable table02 = new PdfPTable(1);
            table02.SetTotalWidth(new float[] { 350 });

            Paragraph p = new Paragraph(ote.Todistusteksti);
            p.Font = new Font(f_ssans_regular, 13, Font.NORMAL, c_3c3c3b);
            p.Leading = 15;
            
            PdfPCell c = new PdfPCell();
            c.Border = 0;
            c.BackgroundColor = c_ffffff;
            c.AddElement(p);
            table02.AddCell(c);

            //table02.AddCell(CreateCell(ote.Todistusteksti, f_ssans_regular, 13, c_3c3c3b, c_ffffff, padding_0_0_0));
            table02.WriteSelectedRows(0, -1, 37, 300, cb);


        // Allekirjoitustiedot
            if (!bTodistusJaVakiolomake)
            {
                // Rekisterinpitäjä + aika
                PdfPTable table03 = new PdfPTable(2);
                table03.SetTotalWidth(new float[] { 145, 380 });

                table03.AddCell(CreateCell(ote.Rekisterinpitaja_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table03.AddCell(CreateCell(ote.Rekisterinpitaja, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));

                table03.AddCell(CreateCell(ote.Aika_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table03.AddCell(CreateCell(ote.Aika, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));

                table03.WriteSelectedRows(0, -1, 37, 245, cb);

                YPositio = YPositio - table03.TotalHeight;

            }
            else
            {
                // Allekirjoittava käyttäjä ja viranomainen
                PdfPTable table05 = new PdfPTable(2);
                table05.SetTotalWidth(new float[] { 145, 380 });

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimi_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimi, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));
                //table05.AddCell(CreateCell(ote.allekirjoitustiedot.Lahiosoite_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                //table05.AddCell(CreateCell(ote.allekirjoitustiedot.Lahiosoite, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postiosoite_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postiosoite, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postitoimipaikka_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postitoimipaikka, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Aika_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Aika, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Paikka_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Paikka, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Virkanimike_otsikko, f_ssans_regular, 13, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Virkanimike, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Hinta, f_fira_medium, 11, c_1d1d1b, c_ffffff, padding_0_20_0));

                table05.WriteSelectedRows(0, -1, 37, 245, cb);

                YPositio = YPositio - table05.TotalHeight;

            }

            cb = LisaaAlatunniste(cb, ote);

            document.Close();
            writer.Close();

            return ms.ToArray();
        }


    }

    private PdfContentByte LisaaTaulu(ref Document document, PdfContentByte cb, OtteenTiedot ote, PdfPTable table)
    {

        //int pageRows = 0;
        //while (pageRows < table.Rows.Count + 19)
        //{
        //    table.WriteSelectedRows(pageRows, pageRows + 20, 37, YPositio, cb);
        //    pageRows = pageRows + 20;
        //    cb = LisaaAlatunniste(cb, ote);
        //    document.NewPage();
        //    cb = LisaaYlatunniste(cb);
        //}

        if (table.Rows.Count > 0)
        {
            float tableHeight = CalculatePdfPTableHeight(table);

            if ((YPositio - tableHeight) < 110)
            {
                // kirjoitetaan uudelle sivulle
                cb = LisaaTyhjarivi(cb);
                cb = LisaaAlatunniste(cb, ote);
                document.NewPage();
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                table.WriteSelectedRows(0, -1, 37, YPositio, cb);
            }
            else
            {
                table.WriteSelectedRows(0, -1, 37, YPositio, cb);
            }

            YPositio = YPositio - table.TotalHeight;
        }

        return cb;
    }

    private PdfContentByte LisaaTyhjarivi(PdfContentByte cb)
    {
        PdfPTable tableTyhja = new PdfPTable(1);
        tableTyhja.SetTotalWidth(new float[] { 525 });

        tableTyhja.AddCell(CreateCell(" ", f_ssans_regular, 13, c_575756, c_e9e9f2, padding_0_0_0));
        tableTyhja.WriteSelectedRows(0, -1, 37, YPositio, cb);
        YPositio = YPositio - tableTyhja.TotalHeight;

        return cb;
    }

    private PdfContentByte LisaaYlatunniste(PdfContentByte cb, string tulostuskieli)
    {
        // DVV:n logo
        string logo_tiedosto;
        int skaalaus = 100;

        if (tulostuskieli == "1")
        {
            logo_tiedosto = DVV_logo_FIN;
            skaalaus = 14;
        }
        else if (tulostuskieli == "2")
        {
            logo_tiedosto = DVV_logo_SWE;
            skaalaus = 8;
        }
        else
        {
            logo_tiedosto = DVV_logo_ENG;
            skaalaus = 8;
        }

        Image iDvv = Image.GetInstance(logo_tiedosto);
        iDvv.SetAbsolutePosition(25, 750);
        iDvv.ScalePercent(skaalaus);
        cb.AddImage(iDvv);

/*
        // VRK:n logo
        Image iVrk = Image.GetInstance(VRK_logo);
        iVrk.SetAbsolutePosition(45, 750);
        iVrk.ScalePercent(25);
        cb.AddImage(iVrk);

        // Maistraatin logo
        Image iMti = Image.GetInstance(MTI_logo);
        iMti.SetAbsolutePosition(175, 765);
        iMti.ScalePercent(25);
        cb.AddImage(iMti);
*/ 

        // Tietojen asemointi pystysuunnassa, alkuarvon asetus
        YPositio = YPositioAlkuarvo;

        return cb;
    }

    private PdfContentByte LisaaAlatunniste(PdfContentByte cb, OtteenTiedot ote)
    {
        if (!bTodistusJaVakiolomake)
        {
            // Luovutettu teksti + pvm ja klo
            PdfPTable table = new PdfPTable(1);
            table.SetTotalWidth(new float[] { 540 });

            table.AddCell(CreateCell(string.Format("{0} {1} {2}", ote.Luovutusteksti, ote.VtjPvm, ote.VtjKlo), f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

            if (ote.Tulostuskieli == "3")
                table.WriteSelectedRows(0, -1, 37, 108, cb);
            else
                table.WriteSelectedRows(0, -1, 37, 100, cb);
        }

        return cb;
    }

    private static float CalculatePdfPTableHeight(PdfPTable table)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (Document doc = new Document(PageSize.A4))
            {
                using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                {
                    doc.Open();
                    table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);
                    doc.Close();

                    return table.TotalHeight;
                }
            }
        }
    }

    private PdfPCell CreateCell(string text, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor bcolor, float[] padding)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk(text, font)));

        cell.Border = 0;
        cell.BackgroundColor = bcolor;

        if (padding[0] > 0)
            cell.PaddingLeft = padding[0];
        if (padding[1] > 0)
            cell.PaddingTop = padding[1];
        if (padding[2] > 0)
            cell.PaddingBottom = padding[2];

        return cell;
    }

    private PdfPCell CreateCell(string text, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor bcolor, float[] padding, int align)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk(text, font)));

        cell.Border = 0;
        cell.BackgroundColor = bcolor;

        if (padding[0] > 0)
            cell.PaddingLeft = padding[0];
        if (padding[1] > 0)
            cell.PaddingTop = padding[1];
        if (padding[2] > 0)
            cell.PaddingBottom = padding[2];

        cell.HorizontalAlignment = align;

        return cell;
    }

    private PdfPCell CreateCell(string text, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor bcolor, float[] padding, int aling, bool noWrap)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);

        PdfPCell cell = new PdfPCell(new Phrase(new Chunk(text, font)));

        cell.Border = 0;
        cell.BackgroundColor = bcolor;

        if (padding[0] > 0)
            cell.PaddingLeft = padding[0];
        if (padding[1] > 0)
            cell.PaddingTop = padding[1];
        if (padding[2] > 0)
            cell.PaddingBottom = padding[2];

        cell.NoWrap = noWrap;

        return cell;
    }


    private string LaskeTiiviste(string pdfString)
    {
        string tiiviste = string.Empty;
        
        byte[] inputBytes = Encoding.Unicode.GetBytes(pdfString);

        // Compute hash code for pdf
        MD5 myMD5 = MD5.Create();
        byte[] Md5Bytes = myMD5.ComputeHash(inputBytes);
        string Md5String = BitConverter.ToString(Md5Bytes).Replace("-", string.Empty);

        //SHA1 mySHa1 = SHA1.Create();
        //byte[] Sha1Bytes = mySHa1.ComputeHash(inputBytes);
        //string Sha1String = BitConverter.ToString(Sha1Bytes).Replace("-", string.Empty);

        //SHA256 mySHa256 = SHA256Managed.Create();
        //byte[] Sha256Bytes = mySHa256.ComputeHash(inputBytes);
        //string Sha256String = BitConverter.ToString(Sha256Bytes).Replace("-", string.Empty);

        tiiviste = Md5String;
        return tiiviste;
    }

    private byte[] LisaaTiiviste(byte[] pdfByte, string tiiviste, OtteenTiedot oteData)
    {

        var output = new MemoryStream();
        var reader = new PdfReader(pdfByte);
        var stamper = new PdfStamper(reader, output);

        int numberOfPages = reader.NumberOfPages;

        // tiivisteen otsikko + tiiviste
        PdfPTable table = new PdfPTable(1);
        table.SetTotalWidth(new float[] { 540 });
        table.AddCell(CreateCell(oteData.Tiiviste, f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));
        table.AddCell(CreateCell(tiiviste, f_fira_regular, 11, c_6f6f6e, c_ffffff, padding_0_0_0));

        for (int i = 1; i <= numberOfPages; ++i)
        {
            PdfContentByte cb = stamper.GetOverContent(i);

            // lisätään tiiviste ko. sivulle
            table.WriteSelectedRows(0, -1, 37, 80, cb);
        }


        //// Viivakoodi
        //Barcode39 bc39 = new Barcode39();
        //bc39.Code = tiiviste;
        //Image img = bc39.CreateImageWithBarcode(cb, null, null);
        //img.SetAbsolutePosition(100, 100);
        //cb.AddImage(img);

        //// QR-koodi
        //BarcodeQRCode bcQR = new BarcodeQRCode(tiiviste, 1, 1, null);
        //Image img2 = bcQR.GetImage();
        //img2.SetAbsolutePosition(415, 50);
        //cb.AddImage(img2);


        stamper.FormFlattening = false;
        stamper.Close();
        reader.Close();

        return output.ToArray();

    }

    private void TallennaTiiviste(string tiiviste, DateTime luovutusaika, string henkilotunnus, Guid tuote_id)
    {
        
        SQLServiceTA.SQLServiceTA _client = new SQLServiceTA.SQLServiceTA();
        SQLServiceTA.TodistuksenTiedot _tt = new SQLServiceTA.TodistuksenTiedot();

        string tallentaja = System.Configuration.ConfigurationManager.AppSettings["TodistusTallentaja"];

        _tt.Todistus_id = Guid.NewGuid();
        _tt.Tuote_id = tuote_id;
        _tt.Tiiviste = tiiviste;
        _tt.Luovutusaika = luovutusaika;
        _tt.Henkilotunnus = henkilotunnus;
        _tt.Tallentaja = tallentaja;

        _client.LisaaTodistuksenTiedot(_tt);

    }

    private byte[] TaytaPdf(OtteenTiedot tiedot)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // Tällä hetkellä käytössä vain 'Kelpoisuus solmia avioliitto ulkomailla' -todistus
            string pdflomake = Pdflomake.AVIOLIITTOKELPOISUUS;
            char[] merkit = new char[] { '.', '/' };
            string tyhja = " ";


            // Pdf-lomakepohja 
            string pdfPath = System.Configuration.ConfigurationManager.AppSettings["AET_tutkintatodistusUlkom"];
            pdfPath = pdfPath + "_" + (tiedot.Tulostuskieli.Equals("3") ? "EN" : tiedot.Tulostuskieli.Equals("2") ? "SE" : "FI") + ".pdf";

            var reader = new PdfReader(pdfPath);
            var stamper = new PdfStamper(reader, ms);
            var formFields = stamper.AcroFields;

            string fontit = System.Configuration.ConfigurationManager.AppSettings["OTE_fontit"];
            BaseFont bs = BaseFont.CreateFont(fontit + "arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            formFields.AddSubstitutionFont(bs);

            var fieldKeys = formFields.Fields.Keys;
            foreach (string fieldKey in fieldKeys)
            {
                if (formFields.GetFieldType(fieldKey) == AcroFields.FIELD_TYPE_TEXT)
                {
                    formFields.SetFieldProperty(fieldKey, "textfont", bs, null);
                    formFields.SetField(fieldKey, " ");
                }
            }

            #region Avioliittokelpoisuus

            // Kelpoisuus solmia avioliitto ulkomailla
            if (pdflomake == Pdflomake.AVIOLIITTOKELPOISUUS)
            {
                // ASIANOMAISEN HENKILÖN TIEDOT
                // Nimet
                formFields.SetField("suknim1", (string.IsNullOrEmpty(tiedot.NykyinenSukunimi) ? tyhja : tiedot.NykyinenSukunimi));
                formFields.SetField("etunim1", (string.IsNullOrEmpty(tiedot.NykyisetEtunimet) ? tyhja : tiedot.NykyisetEtunimet));

                // Syntymäaika
                formFields.SetField("syntaika1", (string.IsNullOrEmpty(tiedot.Syntymaaika) ? tyhja : tiedot.Syntymaaika));

                // Syntymäpaikka ja -valtio
                string syntymapaikkaJavaltio = string.Empty;
                syntymapaikkaJavaltio = (string.IsNullOrEmpty(tiedot.Syntymapaikka) ? "" : tiedot.Syntymapaikka + " ")
                                      + (string.IsNullOrEmpty(tiedot.Syntymavaltio) ? "" : tiedot.Syntymavaltio);
                formFields.SetField("syntpaikka1", (string.IsNullOrEmpty(syntymapaikkaJavaltio) ? tyhja : syntymapaikkaJavaltio));

                // Kansalaisuus
                formFields.SetField("kans1", (string.IsNullOrEmpty(tiedot.Kansalaisuus) ? tyhja : tiedot.Kansalaisuus));

                // Siviilisääty
                formFields.SetField("sivsaat1", (tiedot.Siviilisaaty == "1" ? "0" :                                           // Naimaton
                                                (tiedot.Siviilisaaty == "4" || tiedot.Siviilisaaty == "7" ? "1" :             // Eronnut, Eronnut rekisteröidystä parisuhteesta
                                                (tiedot.Siviilisaaty == "5" || tiedot.Siviilisaaty == "8" ? "2" : "Off"))));  // Leski, Leski rekisteröidystä parisuhteesta

                // Asuinvaltio
                formFields.SetField("asuinvalt1", (string.IsNullOrEmpty(tiedot.Asuinvaltio) ? tyhja : tiedot.Asuinvaltio));

                // Avioliittojen lkm
                formFields.SetField("avioliit1", (string.IsNullOrEmpty(tiedot.Jarjestysnumero) ? "1" : tiedot.Jarjestysnumero));

                
                // HENKILÖ B:n TIEDOT ----------------------------------------------------------------
                // Nimet
                formFields.SetField("suknim2", (string.IsNullOrEmpty(tiedot.HenkiloB.NykyinenSukunimi) ? tyhja : tiedot.HenkiloB.NykyinenSukunimi));
                formFields.SetField("etunim2", (string.IsNullOrEmpty(tiedot.HenkiloB.NykyisetEtunimet) ? tyhja : tiedot.HenkiloB.NykyisetEtunimet));

                // Syntymäaika - rajapinnasta muodossa vvvvkkpp, muotoillaan
                formFields.SetField("syntaika2", (string.IsNullOrEmpty(tiedot.HenkiloB.Syntymaaika) ? tyhja : muotoilePaivamaara(tiedot.HenkiloB.Syntymaaika, tiedot.Tulostuskieli)));

                // Syntymäpaikka ja -valtio
                syntymapaikkaJavaltio = (string.IsNullOrEmpty(tiedot.HenkiloB.Syntymapaikka) ? "" : tiedot.HenkiloB.Syntymapaikka + " ")
                                      + (string.IsNullOrEmpty(tiedot.HenkiloB.Syntymavaltio) ? "" : tiedot.HenkiloB.Syntymavaltio);
                formFields.SetField("syntpaikka2", (string.IsNullOrEmpty(syntymapaikkaJavaltio) ? tyhja : syntymapaikkaJavaltio));

                // Kansalaisuus
                formFields.SetField("kans2", (string.IsNullOrEmpty(tiedot.HenkiloB.Kansalaisuus) ? tyhja : tiedot.HenkiloB.Kansalaisuus));

                // Siviilisääty
                formFields.SetField("sivsaat2", (tiedot.HenkiloB.Siviilisaaty == "1" ? "0" :                                                    // Naimaton
                                                (tiedot.HenkiloB.Siviilisaaty == "4" || tiedot.HenkiloB.Siviilisaaty == "7" ? "1" :             // Eronnut, Eronnut rekisteröidystä parisuhteesta
                                                (tiedot.HenkiloB.Siviilisaaty == "5" || tiedot.HenkiloB.Siviilisaaty == "8" ? "2" : "Off"))));  // Leski, Leski rekisteröidystä parisuhteesta

                // TODO: Asuinvaltio
                formFields.SetField("asuinvalt2", (string.IsNullOrEmpty(tiedot.HenkiloB.Asuinvaltio) ? tyhja : tiedot.HenkiloB.Asuinvaltio));

                // Avioliittojen lkm
                formFields.SetField("avioliit2", (string.IsNullOrEmpty(tiedot.HenkiloB.Jarjestysnumero) ? "" : tiedot.HenkiloB.Jarjestysnumero));


                // Allekirjoitustiedot ----------------------------------------------------------- 
                formFields.SetField("todAnt", tiedot.allekirjoitustiedot.ViranomainenNimi);
                formFields.SetField("asiat", tiedot.allekirjoitustiedot.Asiatunnus);
                formFields.SetField("paikkapvm", tiedot.allekirjoitustiedot.Paikka + " " + tiedot.allekirjoitustiedot.Aika);
                formFields.SetField("nimensel", tiedot.allekirjoitustiedot.VirkailijaNimi + ", " + tiedot.allekirjoitustiedot.Virkanimike);

            }
            #endregion


            stamper.FormFlattening = false; //Form fields should no longer be editable
            stamper.Close();
            reader.Close();

            return ms.ToArray();

        }
    }

    private string muotoilePaivamaara(string pvm, string tulostuskieli)
    {
        string muotoiltuPvm = pvm;

        DateTime dtPvm;
        if (DateTime.TryParseExact(pvm, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtPvm))
        {
            if (tulostuskieli.Equals("3"))
                muotoiltuPvm = dtPvm.ToString("dd MMM yyyy");
            else
                muotoiltuPvm = dtPvm.ToString("dd.MM.yyyy");
        }
            
        return muotoiltuPvm;
    }

}
