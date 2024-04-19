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
/// Summary description for OteToPdf
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class OteToPdf : System.Web.Services.WebService
{
    private BaseColor c_1d1d1b = new BaseColor(29, 29, 27);
    private BaseColor c_3c3c3b = new BaseColor(60, 60, 59);
    private BaseColor c_575756 = new BaseColor(87, 87, 86);
    private BaseColor c_6f6f6e = new BaseColor(111, 111, 110);
    private BaseColor c_e9e9f2 = new BaseColor(233, 233, 242);
    private BaseColor c_ffffff = new BaseColor(255, 255, 255);
    private BaseColor c_0052cc = new BaseColor(0, 82, 204);
    private BaseColor c_00265A = new BaseColor(0, 38, 90);

    private static string fontit = System.Configuration.ConfigurationManager.AppSettings["OTE_fontit"];

    private BaseFont f_fira_medium = BaseFont.CreateFont(fontit + "FiraMono-Medium.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    private BaseFont f_fira_regular = BaseFont.CreateFont(fontit + "FiraMono-Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

    private BaseFont f_ssans_regular = BaseFont.CreateFont(fontit + "SourceSansPro-Regular.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    private BaseFont f_ssans_bold = BaseFont.CreateFont(fontit + "SourceSansPro-Bold.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
    private BaseFont f_ssans_light = BaseFont.CreateFont(fontit + "SourceSansPro-Light.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

    private float[] padding_0_0_0 = new float[] { 0, 0, 0 };
    private float[] padding_0_0_10 = new float[] { 0, 0, 10 };
    private float[] padding_0_0_20 = new float[] { 0, 0, 20 };
    private float[] padding_0_20_0 = new float[] { 0, 20, 0 };
    private float[] padding_0_40_10 = new float[] { 0, 40, 10 };
    private float[] padding_20_0_0 = new float[] { 20, 0, 3 };
    private float[] paddingt_20_0_0 = new float[] { 20, 0, 0 };
    private float[] padding_20_0_10 = new float[] { 20, 0, 10 };
    private float[] padding_20_20_0 = new float[] { 20, 20, 3 };
    private float[] padding_20_10_0 = new float[] { 20, 10, 3 };
    private float[] padding_20_10_20 = new float[] { 20, 10, 20 };
    private float[] padding_20_20_10 = new float[] { 20, 20, 10 };
    private float[] padding_70_0_0 = new float[] { 70, 0, 3 };
    private float[] padding_70_40_0 = new float[] { 70, 40, 3 };

    // Tietojen asemointi pystysuunnassa, alkuarvo
    private float YPositio = 720;
    private float YPositioAlkuarvo = 720;
    private float YPositioVastaanottaja = 740;

    // Logot
    private static string DVV_logo_FIN = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_FIN"];
    private static string DVV_logo_SWE = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_SWE"];
    private static string DVV_logo_ENG = System.Configuration.ConfigurationManager.AppSettings["DVV_logo_ENG"];

    private static string DVV_logo2_FIN = System.Configuration.ConfigurationManager.AppSettings["DVV_logo2_FIN"];
    private static string DVV_logo2_SWE = System.Configuration.ConfigurationManager.AppSettings["DVV_logo2_SWE"];
    private static string DVV_logo2_ENG = System.Configuration.ConfigurationManager.AppSettings["DVV_logo2_ENG"];

    private static string DVV_alatunniste_FIN = System.Configuration.ConfigurationManager.AppSettings["DVV_alatunniste_FIN"];
    private static string DVV_alatunniste_SWE = System.Configuration.ConfigurationManager.AppSettings["DVV_alatunniste_SWE"];
    private static string DVV_alatunniste_ENG = System.Configuration.ConfigurationManager.AppSettings["DVV_alatunniste_ENG"];

    // Linkit
    private static string Palautekysely = System.Configuration.ConfigurationManager.AppSettings["Palautekysely"];
    private static string Rekisterointi_FIN = System.Configuration.ConfigurationManager.AppSettings["Rekisterointi_FIN"];
    private static string Rekisterointi_SWE = System.Configuration.ConfigurationManager.AppSettings["Rekisterointi_SWE"];
    private static string Rekisterointi_ENG = System.Configuration.ConfigurationManager.AppSettings["Rekisterointi_ENG"];
    private static string Muuttoilmoitus_FIN = System.Configuration.ConfigurationManager.AppSettings["Muuttoilmoitus_FIN"];
    private static string Muuttoilmoitus_SWE = System.Configuration.ConfigurationManager.AppSettings["Muuttoilmoitus_SWE"];
    private static string Muuttoilmoitus_ENG = System.Configuration.ConfigurationManager.AppSettings["Muuttoilmoitus_ENG"];
    private static string Verohallinto_FIN = System.Configuration.ConfigurationManager.AppSettings["Verohallinto_FIN"];
    private static string Verohallinto_SWE = System.Configuration.ConfigurationManager.AppSettings["Verohallinto_SWE"];
    private static string Verohallinto_ENG = System.Configuration.ConfigurationManager.AppSettings["Verohallinto_ENG"];

    // Onko kyseessä ote, joka tulostetaan kirjeen muodossa
    private bool bKirje = false;

    private bool bKirjeKELA = false;
    private bool bKirjeETK = false;
    private static string pdfPathKELAjaETK = "";

    private bool bKirjeVERO = false;

    private bool bMuuttoilmoitusPaatos = false;
    private bool bMuuttoilmoitusPaatosSaatteella = false;

    private bool bKotikuntaPaatos = false;
    private bool bKotikuntaPaatosSaatteella = false;


    public OteToPdf()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public byte[] LuoOtePdfmuodossa(Ote ote)
    {
        byte[] pdfByte = null;

        // Onko kyse kirjeestä
        if (ote.KyseltyTuote.Equals(new Guid(OteTuote.REKISTEROINTITIEDOT)))
        {
            bKirje = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.REKISTEROINTITIEDOT_KELA)))
        {
            bKirjeKELA = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.REKISTEROINTITIEDOT_ETK)))
        {
            bKirjeETK = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.REKISTEROINTITIEDOT_VERO)))
        {
            bKirjeVERO = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.MUUTTOILMOITUSPAATOS)))
        {
            bMuuttoilmoitusPaatos = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.MUUTTOILMOITUSPAATOS_SAATE)))
        {
            bMuuttoilmoitusPaatosSaatteella = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS)) || ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS2)) || ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS3)))
        {
            bKotikuntaPaatos = true;
        }
        else if (ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS_SAATE)) || ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS2_SAATE)) || ote.KyseltyTuote.Equals(new Guid(OteTuote.KOTIKUNTAPAATOS3_SAATE)))
        {
            bKotikuntaPaatosSaatteella = true;
        }

        pdfByte = MuodostaPdf(ote);

        if (bKotikuntaPaatos || bKotikuntaPaatosSaatteella || bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella)
        {
            return pdfByte;
        }

        pdfByte = TaydennaYlatunniste(pdfByte, ote.allekirjoitustiedot.Aika, ote.Tulostuskieli, ote.Diaarinumero);

        return pdfByte;

    }

    private byte[] MuodostaPdf(Ote ote)
    {

        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            PdfContentByte cb = writer.DirectContent;

            cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

            // Vastaanottaja
            bool eiOsoitetta = true;
            PdfPTable table00 = new PdfPTable(1);
            table00.SetTotalWidth(new float[] { 505 });

            table00.AddCell(CreateCell(ote.NykyinenSukunimi + " " + ote.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));

            // Verottajan info-otteelle ei tulosteta vastaanottajan osoitetta
            if (!bKirjeVERO)
            {
                if (ote.vakinaisetOsoitteet.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(ote.vakinaisetOsoitteet[0].Postitoimipaikka))
                    {
                        eiOsoitetta = false;
                        table00.AddCell(CreateCell(ote.vakinaisetOsoitteet[0].Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                        table00.AddCell(CreateCell(ote.vakinaisetOsoitteet[0].Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_20));
                    }
                }
                if (eiOsoitetta && ote.tilapaisetOsoitteet.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(ote.tilapaisetOsoitteet[0].Postitoimipaikka))
                    {
                        eiOsoitetta = false;
                        table00.AddCell(CreateCell(ote.tilapaisetOsoitteet[0].Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                        table00.AddCell(CreateCell(ote.tilapaisetOsoitteet[0].Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_20));
                    }
                }
                if (eiOsoitetta && ote.postiOsoitteet.Count > 0)
                {
                    eiOsoitetta = false;
                    table00.AddCell(CreateCell(ote.postiOsoitteet[0].Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                    table00.AddCell(CreateCell(ote.postiOsoitteet[0].Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_20));
                }
            }
            if (eiOsoitetta)
            {
                table00.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                table00.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_20));
            }

            table00.WriteSelectedRows(0, -1, 55, YPositioVastaanottaja, cb);
            YPositio = YPositioVastaanottaja - table00.TotalHeight;

            // Otteen nimi, saatetekstit, käyttötarkoitus
            PdfPTable table01 = new PdfPTable(1);
            table01.SetTotalWidth(new float[] { 505 });

            if (bKirje)
            {
                table01.AddCell(CreateCell(ote.Nimi, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_40_10));
                table01.AddCell(CreateCell(ote.Saateteksti.Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));
                table01.AddCell(CreateLinkCell(ote.Saateteksti2.Substring(0, ote.Saateteksti2.IndexOf("<br />")), f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_10_0));
                table01.AddCell(CreateLinkCell3(ote.Saateteksti2.Substring(ote.Saateteksti2.IndexOf("<br />")), (ote.Tulostuskieli == "1" ? Muuttoilmoitus_FIN : (ote.Tulostuskieli == "2" ? Muuttoilmoitus_SWE : Muuttoilmoitus_ENG)), f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_10_20));
            }
            else if (bKirjeKELA || bKirjeETK)
            {
                table01.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "BÄSTA KUND" : (ote.Tulostuskieli == "3" ? "DEAR CUSTOMER" : "")), f_ssans_regular, 14, c_00265A, c_ffffff, padding_0_40_10));
                table01.AddCell(CreateCell(ote.Saateteksti.Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_10_20));
                table01.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "INFORMATION SOM REGISTRATS" : (ote.Tulostuskieli == "3" ? "REGISTER ENTRY" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_0_10));
            }
            else if (bKirjeVERO)
            {
                table01.AddCell(CreateCell(ote.Nimi, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_40_10));
                table01.AddCell(CreateCell(ote.Saateteksti.Substring(0, ote.Saateteksti.IndexOf("<br />")).Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));
                table01.AddCell(CreateLinkCell(ote.Saateteksti.Substring(ote.Saateteksti.IndexOf("<br />")).Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_10_20));
            }

            else if (bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella)
            {
                if (bMuuttoilmoitusPaatosSaatteella)
                {
                    // Eka sivun saateteksti
                    PdfPTable table01a = new PdfPTable(1);
                    table01a.SetTotalWidth(new float[] { 505 });

                    string[] saateteksti = ote.Saateteksti2.Split(new char[] { '|' });

                    table01a.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Ilmoitus päätöksestä" : (ote.Tulostuskieli == "2" ? "Meddelande om beslut" : "")), f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));
                    table01a.AddCell(CreateCell(saateteksti[0].Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                    table01a.AddCell(CreateCell("Notification of decision", f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));
                    table01a.AddCell(CreateCell(saateteksti[1].Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                    table01a.WriteSelectedRows(0, -1, 55, YPositio, cb);

                    cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                    document.NewPage();
                    cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

                    table00.WriteSelectedRows(0, -1, 55, YPositioVastaanottaja, cb);
                    YPositio = YPositioVastaanottaja - table00.TotalHeight;
                }

                string nimi = ote.Nimi;
                if (ote.PaatoksenTyyppi == "1")
                {
                    nimi = ote.Tulostuskieli == "1" ? "Päätös" : ote.Tulostuskieli == "2" ? "Beslut" : ote.Nimi;
                }

                table01.AddCell(CreateCell(nimi, f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));
                table01.AddCell(CreateCell(ote.Saateteksti.Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));
            }
            else if (bKotikuntaPaatos || bKotikuntaPaatosSaatteella)
            {
                if (bKotikuntaPaatosSaatteella)
                {
                    // Eka sivun saate
                    PdfPTable table01a = new PdfPTable(1);
                    table01a.SetTotalWidth(new float[] { 505 });

                    string[] saateteksti = ote.Saateteksti2.Split(new char[] { '|' });

                    table01a.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Ilmoitus päätöksestä" : (ote.Tulostuskieli == "2" ? "Meddelande om beslut" : "")), f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));
                    table01a.AddCell(CreateCell(saateteksti[0].Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                    table01a.AddCell(CreateCell("Notification of decision", f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));
                    table01a.AddCell(CreateCell(saateteksti[1].Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                    table01a.WriteSelectedRows(0, -1, 55, YPositio, cb);

                    cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                    document.NewPage();
                    cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

                    table00.WriteSelectedRows(0, -1, 55, YPositioVastaanottaja, cb);
                    YPositio = YPositioVastaanottaja - table00.TotalHeight;
                }

                table01.AddCell(CreateCell(ote.Nimi, f_ssans_regular, 16, c_00265A, c_ffffff, padding_0_40_10));

                // Asia
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Asia" : (ote.Tulostuskieli == "2" ? "Ärende" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Kotikunnan rekisteröinti" : (ote.Tulostuskieli == "2" ? "Registrering av hemkommun" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Hakija
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Hakija" : (ote.Tulostuskieli == "2" ? "Sökande" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("{0}, {1} (synt. {2})", ote.NykyinenSukunimi, ote.NykyisetEtunimet, ote.Syntymaaika) : (ote.Tulostuskieli == "2" ? string.Format("{0}, {1} (född {2})", ote.NykyinenSukunimi, ote.NykyisetEtunimet, ote.Syntymaaika) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Pyyntö
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Pyyntö" : (ote.Tulostuskieli == "2" ? "Ansökan" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("Olet {0} pyytänyt Digi- ja väestötietovirastoa rekisteröimään sinulle kotikunnan väestötietojärjestelmään.", ote.HakemuksenSaapumispaiva) : (ote.Tulostuskieli == "2" ? string.Format("Du har den {0} ansökt om att Myndigheten för digitalisering och befolkningsdata ska registrera din hemkommun i befolkningsdatasystemet.", ote.HakemuksenSaapumispaiva) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Digi- ja väestötietoviraston ratkaisu
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Digi- ja väestötietoviraston ratkaisu" : (ote.Tulostuskieli == "2" ? "Avgörande av Myndigheten för digitalisering och befolkningsdata" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Digi- ja väestötietovirasto ei ole tehnyt pyydettyä rekisteröintiä väestötietojärjestelmään." : (ote.Tulostuskieli == "2" ? "Myndigheten för digitalisering och befolkningsdata har inte gjort den begärda registreringen." : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Ratkaisun perustelut
                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Ratkaisun perustelut" : (ote.Tulostuskieli == "2" ? "Motivering för avgörandet" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table01.AddCell(CreateCell(ote.Saateteksti.Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                table01.AddCell(CreateCell((ote.Tulostuskieli == "1" ? (ote.RekisteroityOsoite == "1" ? "Osoitteesi on rekisteröity tilapäisenä osoitteena." : (ote.RekisteroityOsoite == "2" ? "Osoitteesi on rekisteröity postiosoitteena." : "")) : (ote.Tulostuskieli == "2" ? (ote.RekisteroityOsoite == "1" ? "Din adress har registrerats som tillfällig adress." : (ote.RekisteroityOsoite == "2" ? "Din adress har registrerats som postadress." : "")) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));
            }
            else
            {
                table01.AddCell(CreateCell(ote.Nimi, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_0_0));
                if (!string.IsNullOrEmpty(ote.Kayttotarkoitus))
                {
                    table01.AddCell(CreateCell(ote.Kayttotarkoitus_otsikko + ": " + ote.Kayttotarkoitus, f_ssans_regular, 16, c_3c3c3b, c_ffffff, padding_0_0_10));
                }
            }

            table01.WriteSelectedRows(0, -1, 55, YPositio, cb);
            YPositio = YPositio - table01.TotalHeight;

            // -----------------------------------------------------
            // HENKILÖTIEDOT
            // -----------------------------------------------------

            if (!bMuuttoilmoitusPaatos && !bMuuttoilmoitusPaatosSaatteella && !bKotikuntaPaatos && !bKotikuntaPaatosSaatteella) //Päätöksissä ei henkilötietojatietoja
            {

                PdfPTable table1 = new PdfPTable(2);
                table1.SetTotalWidth(new float[] { 145, 360 });

                //// Elossaolotieto
                //table1.AddCell(CreateCell("", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_20_0));
                //table1.AddCell(CreateCell(ote.Elossaolotieto, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));

                // Sukunimi
                table1.AddCell(CreateCell(ote.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_20_0));
                table1.AddCell(CreateCell(ote.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));

                // Etunimet
                table1.AddCell(CreateCell(ote.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Nimen lisätieto
                table1.AddCell(CreateCell(ote.NimenLisatieto_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.NimenLisatieto, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Kutsumanimi
                if (!string.IsNullOrWhiteSpace(ote.Kutsumanimi))
                {
                    table1.AddCell(CreateCell(ote.Kutsumanimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Kutsumanimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                // Valinimi
                if (!string.IsNullOrWhiteSpace(ote.Valinimi))
                {
                    table1.AddCell(CreateCell(ote.Valinimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Valinimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                // Patronyymi
                if (!string.IsNullOrWhiteSpace(ote.Patronyymi))
                {
                    table1.AddCell(CreateCell(ote.Patronyymi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Patronyymi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Henkilötunnus
                table1.AddCell(CreateCell(ote.Henkilotunnus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Henkilotunnus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Kuolinpv
                if (!string.IsNullOrWhiteSpace(ote.Kuolinpv))
                {
                    table1.AddCell(CreateCell(ote.Kuolinpv_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Kuolinpv, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Syntymäaika
                table1.AddCell(CreateCell(ote.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Sukupuoli
                table1.AddCell(CreateCell(ote.Sukupuoli_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Sukupuoli, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Syntymäkotikunta
                if (!string.IsNullOrWhiteSpace(ote.Syntymakotikunta))
                {
                    table1.AddCell(CreateCell(ote.Syntymakotikunta_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Syntymakotikunta, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                // Syntymäpaikka
                if (!string.IsNullOrWhiteSpace(ote.Syntymapaikka))
                {
                    table1.AddCell(CreateCell(ote.Syntymapaikka_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Syntymapaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }
                // Syntymävaltio
                if (!string.IsNullOrWhiteSpace(ote.Syntymavaltio))
                {
                    table1.AddCell(CreateCell(ote.Syntymavaltio_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Syntymavaltio, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Kansalaisuus
                table1.AddCell(CreateCell(ote.Kansalaisuus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Kansalaisuus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Siviilisääty
                table1.AddCell(CreateCell(ote.Siviilisaaty_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Siviilisaaty, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Siviilisääty + pvm
                table1.AddCell(CreateCell(ote.SiviilisaatyPvm_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.SiviilisaatyPvm, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Äidinkieli
                if (!string.IsNullOrWhiteSpace(ote.Aidinkieli))
                {
                    table1.AddCell(CreateCell(ote.Aidinkieli_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Aidinkieli, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Asiointikieli
                if (!string.IsNullOrWhiteSpace(ote.Asiointikieli))
                {
                    table1.AddCell(CreateCell(ote.Asiointikieli_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Asiointikieli, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Ammatti
                if (!string.IsNullOrWhiteSpace(ote.Ammatti))
                {
                    table1.AddCell(CreateCell(ote.Ammatti_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Ammatti, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                // Kotikunta
                table1.AddCell(CreateCell(ote.Kotikunta_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Kotikunta, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Rekisteriviranomainen
                table1.AddCell(CreateCell(ote.Rekisteriviranomainen_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                table1.AddCell(CreateCell(ote.Rekisteriviranomainen, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                // Tietojenluovutuskiellot
                if (!string.IsNullOrWhiteSpace(ote.Tietojenluovutuskielto))
                {
                    table1.AddCell(CreateCell(ote.Tietojenluovutuskielto_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table1.AddCell(CreateCell(ote.Tietojenluovutuskielto, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                }

                table1.WriteSelectedRows(0, -1, 55, YPositio, cb);

                YPositio = YPositio - table1.TotalHeight;


                // ---------------------------------------------------------
                // TAULUKKOTIEDOT
                // ---------------------------------------------------------

                bool ekarivi = true;

                // Ulkomainen henkilönumero
                foreach (Ote.Taulukkotieto ulkomainenHenkilonumero in ote.ulkomaisetHenkilonumerot)
                {
                    if (!string.IsNullOrWhiteSpace(ulkomainenHenkilonumero.Sarake1))
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(ulkomainenHenkilonumero.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(ulkomainenHenkilonumero.Sarake1 + " " + ulkomainenHenkilonumero.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }

                // Passiivi henkilötunnus
                ekarivi = true;
                foreach (Ote.Taulukkotieto passiiviHenkilotunnus in ote.passivoidutHenkilotunnukset)
                {
                    if (!string.IsNullOrWhiteSpace(passiiviHenkilotunnus.Sarake1))
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(passiiviHenkilotunnus.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(passiiviHenkilotunnus.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }

                // Sähköpostiosoitteet
                ekarivi = true;
                foreach (Ote.Taulukkotieto sposoite in ote.sahkopostiOsoitteet)
                {
                    if (!string.IsNullOrWhiteSpace(sposoite.Sarake1))
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(sposoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(sposoite.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }


                if (!bKirjeKELA && !bKirjeETK && !bMuuttoilmoitusPaatos && !bMuuttoilmoitusPaatosSaatteella) //Kela ja ETK kirjeeseen sekä muuttoilmoitusasia osoite vain vastaanottajan tietoihin
                {
                    // Vakinainen osoite
                    ekarivi = true;
                    foreach (Ote.TaulukkotietoOsoite vakinainenOsoite in ote.vakinaisetOsoitteet)
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(vakinainenOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        else
                        {
                            table.AddCell(tyhja());
                            table.AddCell(tyhja());
                        }
                        table.AddCell(CreateCell(vakinainenOsoite.Lahiosoite_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(vakinainenOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(vakinainenOsoite.Postitoimipaikka_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(vakinainenOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(vakinainenOsoite.Alkupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(vakinainenOsoite.Alkupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }

                    // Tilapäiset osoitteet
                    ekarivi = true;
                    foreach (Ote.TaulukkotietoOsoite tilapOsoite in ote.tilapaisetOsoitteet)
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(tilapOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        else
                        {
                            table.AddCell(tyhja());
                            table.AddCell(tyhja());
                        }
                        table.AddCell(CreateCell(tilapOsoite.Lahiosoite_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(tilapOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(tilapOsoite.Postitoimipaikka_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(tilapOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(tilapOsoite.Voimassaoloaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(tilapOsoite.Voimassaoloaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }

                    // Postiosoitteet
                    ekarivi = true;
                    foreach (Ote.TaulukkotietoOsoite postiOsoite in ote.postiOsoitteet)
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(postiOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        else
                        {
                            table.AddCell(tyhja());
                            table.AddCell(tyhja());
                        }
                        table.AddCell(CreateCell(postiOsoite.Lahiosoite_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(postiOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(postiOsoite.Postitoimipaikka_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(postiOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(postiOsoite.Voimassaoloaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(postiOsoite.Voimassaoloaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }

                //// Turvakiellon ilmoitusteksti
                //PdfPTable table12 = new PdfPTable(2);
                //table12.SetTotalWidth(new float[] { 145, 360 });

                //table12.AddCell(CreateCell("", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_20_0));
                //table12.AddCell(CreateCell(ote.Turvakieltoteksti, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));

                //cb = LisaaTaulu(ref document, cb, ote, table12);


                // Entiset kotikunnat
                ekarivi = true;
                foreach (Ote.Taulukkotieto kotikunta in ote.entisetKotikunnat)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.SetTotalWidth(new float[] { 145, 160, 200 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(kotikunta.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(kotikunta.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(kotikunta.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Entiset vakinaiset osoitteet
                if (bKirje)
                {
                    ekarivi = true;
                    foreach (Ote.TaulukkotietoOsoite entVakOsoite in ote.entisetVakinaisetOsoitteet)
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.SetTotalWidth(new float[] { 145, 360 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(entVakOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        else
                        {
                            table.AddCell(tyhja());
                            table.AddCell(tyhja());
                        }
                        table.AddCell(CreateCell(entVakOsoite.Lahiosoite_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Postitoimipaikka_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Voimassaoloaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Voimassaoloaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }
                else
                {
                    ekarivi = true;
                    foreach (Ote.TaulukkotietoOsoite entVakOsoite in ote.entisetVakinaisetOsoitteet)
                    {
                        PdfPTable table = new PdfPTable(3);
                        table.SetTotalWidth(new float[] { 145, 160, 200 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(entVakOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }

                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Voimassaoloaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(entVakOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }


                // Entiset tilapaiset osoitteet
                ekarivi = true;
                foreach (Ote.TaulukkotietoOsoite entTilOsoite in ote.entisetTilapaisetOsoitteet)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.SetTotalWidth(new float[] { 145, 160, 200 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(entTilOsoite.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(entTilOsoite.Voimassaoloaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(entTilOsoite.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(entTilOsoite.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Entiset sukunimet
                ekarivi = true;
                foreach (Ote.Taulukkotieto sukunimi in ote.entisetSukunimet)
                {
                    if (!string.IsNullOrEmpty(sukunimi.Sarake1) || !string.IsNullOrEmpty(sukunimi.Sarake2))
                    {
                        PdfPTable table = new PdfPTable(3);
                        table.SetTotalWidth(new float[] { 145, 160, 200 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(sukunimi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(sukunimi.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(sukunimi.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }

                //Entiset etunimet
                ekarivi = true;
                foreach (Ote.Taulukkotieto etunimi in ote.entisetEtunimet)
                {
                    if (!string.IsNullOrEmpty(etunimi.Sarake1) || !string.IsNullOrEmpty(etunimi.Sarake2))
                    {
                        PdfPTable table = new PdfPTable(3);
                        table.SetTotalWidth(new float[] { 145, 160, 200 });

                        if (ekarivi)
                        {
                            table.AddCell(CreateCell(etunimi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                            ekarivi = false;
                        }
                        table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(etunimi.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(etunimi.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                        cb = LisaaTaulu(ref document, cb, ote, table);
                    }
                }

                //Korjatut sukunimet
                ekarivi = true;
                foreach (Ote.Taulukkotieto korjattuSukunimi in ote.korjatutSukunimet)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.SetTotalWidth(new float[] { 145, 160, 200 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(korjattuSukunimi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(korjattuSukunimi.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(korjattuSukunimi.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }

                //Korjatut etunimet
                ekarivi = true;
                foreach (Ote.Taulukkotieto korjattuEtunimi in ote.korjatutEtunimet)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.SetTotalWidth(new float[] { 145, 160, 200 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(korjattuEtunimi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(korjattuEtunimi.Sarake1, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(korjattuEtunimi.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Avioliitto
                ekarivi = true;
                foreach (Ote.TaulukkotietoAvioliitto avioliitto in ote.avioliitot)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(avioliitto.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(avioliitto.Jarjestysnro_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.Jarjestysnro, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.Alkupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.Alkupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(avioliitto.Loppupaiva))
                    {
                        table.AddCell(CreateCell(avioliitto.Loppupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(avioliitto.Loppupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(avioliitto.Paattymistapa_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(avioliitto.Paattymistapa, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    table.AddCell(CreateCell(avioliitto.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.Kansalaisuus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.Kansalaisuus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(avioliitto.Kuolinpv))
                    {
                        table.AddCell(CreateCell(avioliitto.Kuolinpv_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(avioliitto.Kuolinpv, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    table.AddCell(CreateCell(avioliitto.EntinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.EntinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.EntisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.EntisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.KorjattuSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.KorjattuSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(avioliitto.KorjatutEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.KorjatutEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Rekisteröity parisuhde
                ekarivi = true;
                foreach (Ote.TaulukkotietoAvioliitto rekpa in ote.rekpat)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(rekpa.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(rekpa.Jarjestysnro_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.Jarjestysnro, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.Alkupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.Alkupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(rekpa.Loppupaiva))
                    {
                        table.AddCell(CreateCell(rekpa.Loppupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(rekpa.Loppupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                        table.AddCell(CreateCell(rekpa.Paattymistapa_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(rekpa.Paattymistapa, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    table.AddCell(CreateCell(rekpa.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.Kansalaisuus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.Kansalaisuus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(rekpa.Kuolinpv))
                    {
                        table.AddCell(CreateCell(rekpa.Kuolinpv_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(rekpa.Kuolinpv, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    table.AddCell(CreateCell(rekpa.EntinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.EntinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.EntisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.EntisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.KorjattuSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.KorjattuSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(rekpa.KorjatutEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(rekpa.KorjatutEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Entiset avioliitot
                ekarivi = true;
                foreach (Ote.Taulukkotieto avioliitto in ote.entisetAvioliitot)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(avioliitto.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(avioliitto.Sarake1 + " - " + avioliitto.Sarake2, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Lapset
                ekarivi = true;
                foreach (Ote.TaulukkotietoViitehenkilo lapsi in ote.lapset)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(lapsi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(lapsi.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(lapsi.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(lapsi.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(lapsi.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(lapsi.Henkilotunnus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(lapsi.Henkilotunnus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(lapsi.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(lapsi.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(lapsi.Kansalaisuus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(lapsi.Kansalaisuus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(lapsi.Kuolinpv))
                    {
                        table.AddCell(CreateCell(lapsi.Kuolinpv_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(lapsi.Kuolinpv, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Huollettavat
                ekarivi = true;
                foreach (Ote.TaulukkotietoViitehenkilo huollettava in ote.huollettavat)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(huollettava.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(huollettava.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huollettava.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huollettava.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huollettava.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huollettava.Henkilotunnus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huollettava.Henkilotunnus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huollettava.UlkohloSyntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huollettava.UlkohloSyntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huollettava.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huollettava.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Vanhemmat
                ekarivi = true;
                foreach (Ote.TaulukkotietoViitehenkilo vanhempi in ote.vanhemmat)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(vanhempi.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(vanhempi.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(vanhempi.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(vanhempi.Henkilotunnus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.Henkilotunnus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(vanhempi.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(vanhempi.EntinenSukunimi))
                    {
                        table.AddCell(CreateCell(vanhempi.EntinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(vanhempi.EntinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }
                    table.AddCell(CreateCell(vanhempi.Syntymakotikunta_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.Syntymakotikunta, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(vanhempi.Syntymavaltio_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.Syntymavaltio, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(vanhempi.Kansalaisuus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(vanhempi.Kansalaisuus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    if (!string.IsNullOrWhiteSpace(vanhempi.Kuolinpv))
                    {
                        table.AddCell(CreateCell(vanhempi.Kuolinpv_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                        table.AddCell(CreateCell(vanhempi.Kuolinpv, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    }

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Huoltajat

                ekarivi = true;
                foreach (Ote.TaulukkotietoViitehenkilo huoltaja in ote.huoltajat)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(huoltaja.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(huoltaja.NykyinenSukunimi_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huoltaja.NykyinenSukunimi, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huoltaja.NykyisetEtunimet_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huoltaja.NykyisetEtunimet, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huoltaja.Henkilotunnus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huoltaja.Henkilotunnus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huoltaja.UlkohloSyntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huoltaja.UlkohloSyntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(huoltaja.Syntymaaika_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(huoltaja.Syntymaaika, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }



                // Edunvalvonnat
                ekarivi = true;
                foreach (Ote.TaulukkotietoEdunvalvonta edunvalvonta in ote.edunvalvonnat)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(edunvalvonta.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(edunvalvonta.Rajoitus_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(edunvalvonta.Rajoitus, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(edunvalvonta.Alkupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(edunvalvonta.Alkupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(edunvalvonta.TehtavienJako_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(edunvalvonta.TehtavienJako, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }


                // Edunvalvontavaltuutus
                ekarivi = true;
                foreach (Ote.TaulukkotietoEdunvalvontavaltuutus edunvalvontavaltuutus in ote.edunvalvontavaltuutukset)
                {
                    PdfPTable table = new PdfPTable(2);
                    table.SetTotalWidth(new float[] { 145, 360 });

                    if (ekarivi)
                    {
                        table.AddCell(CreateCell(edunvalvontavaltuutus.Otsikko, f_ssans_bold, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true));
                        table.AddCell(CreateCell(" ", f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_20_0));
                        ekarivi = false;
                    }
                    else
                    {
                        table.AddCell(tyhja());
                        table.AddCell(tyhja());
                    }
                    table.AddCell(CreateCell(edunvalvontavaltuutus.Alkupaiva_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(edunvalvontavaltuutus.Alkupaiva, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));
                    table.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako_otsikko, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0));
                    table.AddCell(CreateCell(edunvalvontavaltuutus.TehtavienJako, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0));

                    cb = LisaaTaulu(ref document, cb, ote, table);
                }

                cb = LisaaTyhjarivi(cb);

            }

            // -------------------------------------------------------------------------------------------
            // TODISTUKSEN LOPPUTEKSTIT
            // -------------------------------------------------------------------------------------------

            // Allekirjoitustiedot
            if (bKirje)
            {
                if (YPositio < 320)
                {
                    // kirjoitetaan uudelle sivulle
                    cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                    document.NewPage();
                    cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                }

                // Lopputervehdys ja yhteystiedot
                PdfPTable table03 = new PdfPTable(1);
                table03.SetTotalWidth(new float[] { 485 });

                table03.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_0_0));
                table03.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateCell(ote.allekirjoitustiedot.Sahkoposti_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_20_0)); //Yhteystiedot
                table03.AddCell(CreateLinkCell2(ote.allekirjoitustiedot.Sahkoposti, f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin, f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateCell(" ", f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateLinkCell(ote.Tietosuojalausunto, f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateCell(" ", f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));


                //Palautekyselyyn liittyvä teksti ja linkit
                table03.AddCell(CreateCell(ote.Palautekysely, f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                Anchor klikkaa = new Anchor(new Phrase(new Chunk((ote.Tulostuskieli == "1" ? "Klikkaa tästä " : (ote.Tulostuskieli == "2" ? "Klicka här " : (ote.Tulostuskieli == "3" ? "Click here " : ""))), new Font(f_ssans_regular, 12, Font.NORMAL, c_0052cc))));
                klikkaa.Reference = Palautekysely;
                //klikkaa.Reference = "https://response.questback.com/dynamic/dvv/y6ogn0ic6u/answer?sid=QwEJl6aJYk&Palvelu=Kansainvälisten elämäntapahtumien rekisteröinti=dummyText&Palveluluokka=VTJtietopalvelut Vireillepano";

                Anchor klikkaa2 = new Anchor(new Phrase(new Chunk((ote.Tulostuskieli == "1" ? Rekisterointi_FIN : (ote.Tulostuskieli == "2" ? Rekisterointi_SWE : (ote.Tulostuskieli == "3" ? Rekisterointi_ENG : ""))), new Font(f_ssans_regular, 12, Font.NORMAL, c_0052cc))));
                klikkaa2.Reference = (ote.Tulostuskieli == "1" ? Rekisterointi_FIN : (ote.Tulostuskieli == "2" ? Rekisterointi_SWE : (ote.Tulostuskieli == "3" ? Rekisterointi_ENG : "")));
                //klikkaa2.Reference = "https://dvv.fi/ulkomaalaisen-rekisterointi";

                Phrase tai = new Phrase();
                tai.Add(new Chunk((ote.Tulostuskieli == "1" ? "tai " : (ote.Tulostuskieli == "2" ? "eller " : (ote.Tulostuskieli == "3" ? "or " : ""))), new Font(f_ssans_regular, 12, Font.NORMAL, c_3c3c3b)));

                Paragraph paragraph = new Paragraph();
                paragraph.Add(klikkaa);
                paragraph.Add(tai);
                paragraph.Add(klikkaa2);

                PdfPCell c = new PdfPCell();
                c.Border = 0;
                c.BackgroundColor = c_ffffff;
                c.PaddingLeft = 20;
                c.AddElement(paragraph);
                table03.AddCell(c);

                table03.WriteSelectedRows(0, -1, 55, 295, cb);

                YPositio = YPositio - table03.TotalHeight;

            }
            else if (bKirjeVERO)
            {
                if (YPositio < 140)
                {
                    // kirjoitetaan uudelle sivulle
                    document.NewPage();
                    cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                }

                // Lopputervehdys ja yhteystiedot
                PdfPTable table03 = new PdfPTable(1);
                table03.SetTotalWidth(new float[] { 485 });

                table03.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_ffffff, padding_20_0_0));
                table03.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_ffffff, padding_20_0_0));
                table03.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_0_0));
                table03.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_20_0_0));

                table03.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_20_0)); //Yhteystiedot -otsikko
                table03.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin, f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                Phrase lueLisaa = new Phrase();
                lueLisaa.Add(new Chunk((ote.Tulostuskieli == "1" ? "Lue lisää " : (ote.Tulostuskieli == "2" ? "Läs mera " : (ote.Tulostuskieli == "3" ? "Read more " : ""))), new Font(f_ssans_regular, 12, Font.NORMAL, c_3c3c3b)));

                Anchor vero = new Anchor(new Phrase(new Chunk("www.vero.fi", new Font(f_ssans_regular, 12, Font.NORMAL, c_0052cc))));
                vero.Reference = (ote.Tulostuskieli == "1" ? Verohallinto_FIN : (ote.Tulostuskieli == "2" ? Verohallinto_SWE : (ote.Tulostuskieli == "3" ? Verohallinto_ENG : "")));

                Paragraph paragraph = new Paragraph();
                paragraph.Add(lueLisaa);
                paragraph.Add(vero);

                PdfPCell c = new PdfPCell();
                c.Border = 0;
                c.BackgroundColor = c_ffffff;
                c.PaddingLeft = 20;
                c.AddElement(paragraph);
                table03.AddCell(c);

                table03.WriteSelectedRows(0, -1, 55, YPositio, cb);

                YPositio = YPositio - table03.TotalHeight;

            }
            else if (bKirjeKELA || bKirjeETK)
            {
                PdfPTable table03 = new PdfPTable(1);
                table03.SetTotalWidth(new float[] { 485 });

                if (bKirjeKELA)
                {
                    //Yhteydenottokehoitus KELA
                    table03.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "ANVISNINGAR FÖR FÖRFRÅGNINGAR" : (ote.Tulostuskieli == "3" ? "INSTRUCTIONS FOR INQUIRIES" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                    table03.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "Vänligen kontakta:" + Chunk.NEWLINE + "Folkpensionsanstalten FPA" + Chunk.NEWLINE + "Centret för internationella ärenden" + Chunk.NEWLINE + "via telefon: +358 20 634 0200." + Chunk.NEWLINE + "Andra kontaktuppgifter finns på deras webbplats https://www.kela.fi/web/sv." : (ote.Tulostuskieli == "3" ? "Please contact:" + Chunk.NEWLINE + "KELA, the Social Insurance Institution of Finland" + Chunk.NEWLINE + "Centre for International Affairs" + Chunk.NEWLINE + "by phone: +358 20 634 0200." + Chunk.NEWLINE + "Other contact information can be found on their website https://www.kela.fi/web/en." : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));
                }
                else
                {
                    //Yhteydenottokehoitus ETK
                    table03.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "ANVISNINGAR FÖR FÖRFRÅGNINGAR" : (ote.Tulostuskieli == "3" ? "INSTRUCTIONS FOR INQUIRIES" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                    table03.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "Vänligen kontakta: Pensionsskyddcentralen," + Chunk.NEWLINE + "via e-post på ulkomaanasiat@etk.fi eller via telefon på +358 29 411 2110" : (ote.Tulostuskieli == "3" ? "Please contact: The Finnish Centre for Pensions," + Chunk.NEWLINE + "by e-mail ulkomaanasiat@etk.fi or by phone +358 29 411 2110" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));
                }

                cb = LisaaTaulu(ref document, cb, ote, table03);

                cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                document.NewPage();
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

                PdfPTable table04 = new PdfPTable(1);
                table04.SetTotalWidth(new float[] { 485 });

                //Yhteydenottokehoitus DVV
                table04.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "ANVISNINGAR FÖR BEGÄRAN OM RÄTTELSE" : (ote.Tulostuskieli == "3" ? "INSTRUCTIONS FOR REQUESTING RECTIFICATION" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table04.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "Om du anser att din registerpost är felaktig eller har gjorts under falska förevändningar har du rätt att göra en skriftlig begäran om rättelse till Myndigheten för digitalisering och befolkningsdata." : (ote.Tulostuskieli == "3" ? "If you consider your register entry to be incorrect or made under false pretenses, you have the right to make a written rectification request to the Digital and Population Data Services Agency." : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_10));

                table04.AddCell(CreateCell(string.Format("{0}\n{1}, FI-{2}, FINLAND", ote.allekirjoitustiedot.ViranomainenNimi, ote.allekirjoitustiedot.Lahiosoite, ote.allekirjoitustiedot.Postitoimipaikka), f_ssans_regular, 12, c_1d1d1b, c_ffffff, padding_20_0_0));

                table04.AddCell(CreateCell(ote.Saateteksti2.Replace("<br />", ""), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_10_20));

                // Lakipykälät
                table04.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "BESTÄMMELSER ENLIGT FINLANDS LAG" : (ote.Tulostuskieli == "3" ? "STATUTES ACCORDING TO FINNISH LAW" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_0_10));
                table04.AddCell(CreateCell((ote.Tulostuskieli == "2" ? "Lag om befolkningsdatasystemet och de certifikattjänster som tillhandahålls av Myndigheten för digitalisering och befolkningsdata, paragraf 76" + Chunk.NEWLINE + "Förvaltningsprocesslagen, paragraf 46" + Chunk.NEWLINE + "Allmänna dataskyddsförordningen, artikel 6.1" : (ote.Tulostuskieli == "3" ? "The Population Information Act, Section 76" + Chunk.NEWLINE + "Administrative Procedure Act, Section 46" + Chunk.NEWLINE + "General Data Protection Regulation Article 6 (1)" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                // Lopputervehdys
                table04.AddCell(CreateCell(" ", f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_20_0));
                table04.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_20_0));
                table04.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimiEiYksikkoa, f_ssans_regular, 12, c_1d1d1b, c_ffffff, padding_20_0_0));
                table04.AddCell(CreateCell(ote.allekirjoitustiedot.Sahkoposti_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_20_20_0)); //Yhteystiedot
                table04.AddCell(CreateLinkCell2(ote.allekirjoitustiedot.Sahkoposti, f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_0_0));
                table04.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin, f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                table04.AddCell(CreateCell(" ", f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));
                table04.AddCell(CreateLinkCell(ote.Tietosuojalausunto, f_ssans_regular, 12, c_3c3c3b, c_0052cc, c_ffffff, padding_20_0_0));
                table04.AddCell(CreateCell(" ", f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_20_0_0));

                cb = LisaaTaulu(ref document, cb, ote, table04);
            }
            else if (bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella)
            {
                PdfPTable table06 = new PdfPTable(1);
                table06.SetTotalWidth(new float[] { 485 });

                // Lakipykälät
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Lainkohdat" : (ote.Tulostuskieli == "2" ? "Lagrum" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Kotikuntalaki 2 §, 7 §, 7a §, 9 §, 10 §, 11 § ja 16 §" : (ote.Tulostuskieli == "2" ? "Lagen om hemkommun 2 §, 7 §, 7a §, 9 §, 10 §, 11 § och 16 §" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Muutoksenhaku
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Muutoksenhaku" : (ote.Tulostuskieli == "2" ? "Begäran av omprövning" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Tähän päätökseen saa hakea oikaisua siten kuin hallintolaissa (434/2003) säädetään. Oikaisuvaatimusohje on liitteenä." : (ote.Tulostuskieli == "2" ? "Omprövning i detta förvaltningsbeslut får sökas enlighet med förvaltningslagen (434/2003). Anvisning för begäran av omprövning har bifogats." : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Virkailijan tiedot
                table06.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi + Chunk.NEWLINE + ote.allekirjoitustiedot.Virkanimike + Chunk.NEWLINE + (ote.Tulostuskieli == "1" ? "Digi- ja väestötietovirasto" : (ote.Tulostuskieli == "2" ? "Myndigheten för digitalisering och befolkningsdata" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_40_0));
                //table06.AddCell(CreateCell(ote.allekirjoitustiedot.Virkanimike, f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_60_0_0));
                //table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Digi- ja väestötietovirasto" : (ote.Tulostuskieli == "2" ? "Myndigheten för digitalisering och befolkningsdata" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_60_0_0));

                // Lisätiedot
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Lisätiedot" : (ote.Tulostuskieli == "2" ? "Mer information" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("Lisätietoja tästä päätöksestä antaa {0} {1} puh. {2}, {3}", ote.allekirjoitustiedot.Virkanimike, ote.allekirjoitustiedot.VirkailijaNimi, ote.allekirjoitustiedot.Puhelin, ote.allekirjoitustiedot.Sahkoposti) : (ote.Tulostuskieli == "2" ? string.Format("Mer information om detta beslut ges av {0} {1} tfn. {2}, {3}", ote.allekirjoitustiedot.Virkanimike, ote.allekirjoitustiedot.VirkailijaNimi, ote.allekirjoitustiedot.Puhelin, ote.allekirjoitustiedot.Sahkoposti) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Jakelu
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Jakelu" : (ote.Tulostuskieli == "2" ? "Delgivning" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("{0} {1}", (ote.Tiedoksiantotapa == "1" ? "Sähköpostitse" : (ote.Tiedoksiantotapa == "2" ? "Postitse" : "")), ote.allekirjoitustiedot.Aika) : (ote.Tulostuskieli == "2" ? string.Format("{0} {1}", (ote.Tiedoksiantotapa == "1" ? "Per e-post" : (ote.Tiedoksiantotapa == "2" ? "Per post" : "")), ote.allekirjoitustiedot.Aika) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                cb = LisaaTaulu(ref document, cb, ote, table06);

                cb = TaydennaSivunYlatunniste(cb, ote.Tulostuskieli, 1, 1, ote.allekirjoitustiedot.Aika, ote.Diaarinumero);
            }
            else if (bKotikuntaPaatos || bKotikuntaPaatosSaatteella)
            {
                // kirjoitetaan uudelle sivulle
                cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                cb = TaydennaSivunYlatunniste(cb, ote.Tulostuskieli, 1, 2, ote.allekirjoitustiedot.Aika, ote.Diaarinumero);
                document.NewPage();
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);

                PdfPTable table06 = new PdfPTable(1);
                table06.SetTotalWidth(new float[] { 485 });

                // Lakipykälät
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Sovelletut lainkohdat" : (ote.Tulostuskieli == "2" ? "Lagrum" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Kotikuntalaki (201/1994) 4 §" : (ote.Tulostuskieli == "2" ? "Lag om hemkommun (201/1994) 4 §" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Muutoksenhaku
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Oikaisuvaatimus" : (ote.Tulostuskieli == "2" ? "Begäran av omprövning" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Tähän päätökseen saa hakea oikaisua siten kuin hallintolaissa (434/2003) säädetään. Oikaisuvaatimusohje on liitteenä." : (ote.Tulostuskieli == "2" ? "Omprövning i detta förvaltningsbeslut får sökas enlighet med förvaltningslagen (434/2003). Anvisning för begäran av omprövning har bifogats." : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Lisätiedot
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Lisätietoja" : (ote.Tulostuskieli == "2" ? "Mer information" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("Lisätietoja tästä päätöksestä antaa {0} {1} puh. {2}, {3}", ote.allekirjoitustiedot.Virkanimike, ote.allekirjoitustiedot.VirkailijaNimi, ote.allekirjoitustiedot.Puhelin, ote.allekirjoitustiedot.Sahkoposti) : (ote.Tulostuskieli == "2" ? string.Format("Mer information om detta beslut ges av {0} {1} tfn. {2}, {3}", ote.allekirjoitustiedot.Virkanimike, ote.allekirjoitustiedot.VirkailijaNimi, ote.allekirjoitustiedot.Puhelin, ote.allekirjoitustiedot.Sahkoposti) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                // Virkailijan tiedot
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Allekirjoitus" : (ote.Tulostuskieli == "2" ? "Underskrift" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi + Chunk.NEWLINE + ote.allekirjoitustiedot.Virkanimike + Chunk.NEWLINE + (ote.Tulostuskieli == "1" ? "Digi- ja väestötietovirasto" : (ote.Tulostuskieli == "2" ? "Myndigheten för digitalisering och befolkningsdata" : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_40_0));

                // Jakelu
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? "Tiedoksianto" : (ote.Tulostuskieli == "2" ? "Delgivning" : "")), f_ssans_regular, 12, c_00265A, c_ffffff, padding_20_20_10));
                table06.AddCell(CreateCell((ote.Tulostuskieli == "1" ? string.Format("{0} {1}", (ote.Tiedoksiantotapa == "1" ? "Sähköpostitse" : (ote.Tiedoksiantotapa == "2" ? "Postitse" : "")), ote.allekirjoitustiedot.Aika) : (ote.Tulostuskieli == "2" ? string.Format("{0} {1}", (ote.Tiedoksiantotapa == "1" ? "Per e-post" : (ote.Tiedoksiantotapa == "2" ? "Per post" : "")), ote.allekirjoitustiedot.Aika) : "")), f_ssans_regular, 12, c_3c3c3b, c_ffffff, padding_70_0_0));

                cb = LisaaTaulu(ref document, cb, ote, table06);

                cb = TaydennaSivunYlatunniste(cb, ote.Tulostuskieli, 2, 2, ote.allekirjoitustiedot.Aika, ote.Diaarinumero);
            }
            else
            {
                if (YPositio < 320)
                {
                    // kirjoitetaan uudelle sivulle
                    cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                    document.NewPage();
                    cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                }

                // Allekirjoittava käyttäjä ja viranomainen
                PdfPTable table05 = new PdfPTable(2);
                table05.SetTotalWidth(new float[] { 145, 360 });

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimi_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.ViranomainenNimi, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                //table05.AddCell(CreateCell(ote.allekirjoitustiedot.Lahiosoite_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                //table05.AddCell(CreateCell(ote.allekirjoitustiedot.Lahiosoite, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postiosoite_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postiosoite, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postitoimipaikka_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Postitoimipaikka, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Puhelin, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Aika_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Aika, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Paikka_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Paikka, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.VirkailijaNimi, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Virkanimike_otsikko, f_ssans_regular, 12, c_575756, c_ffffff, padding_0_0_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Virkanimike, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_0_0));

                table05.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_ffffff, padding_0_20_0));
                table05.AddCell(CreateCell(ote.allekirjoitustiedot.Hinta, f_fira_medium, 10, c_1d1d1b, c_ffffff, padding_0_20_0));

                table05.WriteSelectedRows(0, -1, 55, 245, cb);

                YPositio = YPositio - table05.TotalHeight;

            }

            cb = LisaaAlatunniste(cb, ote.Tulostuskieli);

            if (bKirjeKELA || bKirjeETK)
            {
                //KELA ja ETK kirjeisiin ohje asiakkaalle, sivu 3
                string pdfPath = System.Configuration.ConfigurationManager.AppSettings["KELAjaETK"] + (ote.Tulostuskieli == "2" ? "_SE" : (ote.Tulostuskieli == "3" ? "_EN" : "")) + ".pdf";
                PdfReader reader = new PdfReader(pdfPath);
                int pages = reader.NumberOfPages;
                for (int pagenum = 1; pagenum <= pages; pagenum++)
                {
                    document.NewPage();
                    PdfImportedPage page = writer.GetImportedPage(reader, pagenum);
                    cb.AddTemplate(page, 0, 0);
                }
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
            }

            if (bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella || bKotikuntaPaatos || bKotikuntaPaatosSaatteella)
            {
                //Muuttoilmoitusasia ja Kotikunta päätökseen oikaisuvaatimusohje asiakkaalle, kaksi viimeistä sivua
                string pdfPath = System.Configuration.ConfigurationManager.AppSettings["Oikaisuvaatimusohje"] + (ote.PaatoksenTyyppi == "1" ? "_kv" : (ote.PaatoksenTyyppi == "2" ? "_mu" : "")) + (ote.Tulostuskieli == "1" ? "_FI" : (ote.Tulostuskieli == "2" ? "_SE" : "")) + ".pdf";

                PdfReader reader = new PdfReader(pdfPath);
                int pages = reader.NumberOfPages;
                for (int pagenum = 1; pagenum <= pages; pagenum++)
                {
                    document.NewPage();
                    PdfImportedPage page = writer.GetImportedPage(reader, pagenum);
                    cb.AddTemplate(page, 0, 0);

                    if (ote.PaatoksenTyyppi == "2")
                    {
                        cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                        cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                    }
                }

                // Jos joutuu muokkaamaan oikaisuvaatimusohjetta linkkien osalta ...
                /*
                PdfPTable table06a = new PdfPTable(1);
                table06a.SetTotalWidth(new float[] { 485 });

                Anchor turvaposti = new Anchor(new Phrase(new Chunk((ote.Tulostuskieli == "1" ? "DVV:n turvapostia" : (ote.Tulostuskieli == "2" ? "krypterad e-post" : "")), new Font(f_ssans_regular, 10, Font.NORMAL, c_0052cc))));
                turvaposti.Reference = "https://turvaviesti.dvv.fi/";
                Anchor international = new Anchor(new Phrase(new Chunk("international@dvv.fi", new Font(f_ssans_regular, 10, Font.NORMAL, c_0052cc))));
                international.Reference = "international@dvv.fi";
                Anchor muuttoneuvonta = new Anchor(new Phrase(new Chunk("muuttoneuvonta@dvv.fi", new Font(f_ssans_regular, 10, Font.NORMAL, c_0052cc))));
                muuttoneuvonta.Reference = "muuttoneuvonta@dvv.fi";
                Anchor dvv = new Anchor(new Phrase(new Chunk("www.dvv.fi", new Font(f_ssans_regular, 10, Font.NORMAL, c_0052cc))));
                dvv.Reference = "http://www.dvv.fi/";

                Phrase ohje_a = new Phrase();
                ohje_a.Add(new Chunk((ote.Tulostuskieli == "1" ? "Voit ottaa yhteyttä myös sähköpostitse." + Chunk.NEWLINE + "Käytä sähköpostin lähettämiseen " : (ote.Tulostuskieli == "2" ? "Du kan även ta kontakt med oss per e-post." + Chunk.NEWLINE + "Använd " : "")), new Font(f_ssans_regular, 10, Font.NORMAL, c_3c3c3b)));
                ohje_a.Add(turvaposti);
                ohje_a.Add(new Chunk((ote.Tulostuskieli == "1" ? ", jotta tietosi liikkuvat salatusti. Valitse turvapostissa vastaanottajaksi " : (ote.Tulostuskieli == "2" ? " för att överföra dina uppgifter säkert. Välj " : "")), new Font(f_ssans_regular, 10, Font.NORMAL, c_3c3c3b)));
                ohje_a.Add(ote.PaatoksenTyyppi == "1" ? international : (ote.PaatoksenTyyppi == "2" ? muuttoneuvonta : international));
                ohje_a.Add(new Chunk((ote.Tulostuskieli == "1" ? "" : (ote.Tulostuskieli == "2" ? " som mottagare." : "")), new Font(f_ssans_regular, 10, Font.NORMAL, c_3c3c3b)));

                Paragraph paragraph = new Paragraph();
                paragraph.Add(ohje_a);

                PdfPCell c = new PdfPCell();
                c.Border = 0;
                c.BackgroundColor = c_ffffff;
                c.PaddingLeft = 100;
                c.AddElement(paragraph);
                table06a.AddCell(c);

                table06a.WriteSelectedRows(0, -1, 55, 195, cb);
                YPositio = YPositio - table06a.TotalHeight;
                */
            }

            document.Close();
            writer.Close();

            return ms.ToArray();
        }


    }


    private PdfContentByte LisaaTaulu(ref Document document, PdfContentByte cb, Ote ote, PdfPTable table)
    {
        if (table.Rows.Count > 0)
        {
            float tableHeight = CalculatePdfPTableHeight(table);

            if ((YPositio - tableHeight) < 80)
            {
                // kirjoitetaan uudelle sivulle
                cb = LisaaTyhjarivi(cb);
                cb = LisaaAlatunniste(cb, ote.Tulostuskieli);
                document.NewPage();
                cb = LisaaYlatunniste(cb, ote.Tulostuskieli);
                //cb = LisaaTyhjarivi(cb);
                table.WriteSelectedRows(0, -1, 55, YPositio, cb);
            }
            else
            {
                table.WriteSelectedRows(0, -1, 55, YPositio, cb);
            }

            YPositio = YPositio - table.TotalHeight;
        }

        return cb;
    }

    private PdfContentByte LisaaTyhjarivi(PdfContentByte cb)
    {
        PdfPTable tableTyhja = new PdfPTable(1);
        tableTyhja.SetTotalWidth(new float[] { 505 });

        tableTyhja.AddCell(CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, padding_0_0_0));
        tableTyhja.WriteSelectedRows(0, -1, 55, YPositio, cb);
        YPositio = YPositio - tableTyhja.TotalHeight;

        return cb;
    }

    private PdfContentByte LisaaYlatunniste(PdfContentByte cb, string tulostuskieli)
    {
        if (!bKirjeVERO)
        {
            // DVV:n logo
            string logo_tiedosto;
            int skaalaus = 100;

            if (tulostuskieli == "1")
            {
                logo_tiedosto = DVV_logo2_FIN;
                skaalaus = 32;
            }
            else if (tulostuskieli == "2")
            {
                logo_tiedosto = DVV_logo2_SWE;
                skaalaus = 32;
            }
            else
            {
                logo_tiedosto = DVV_logo2_ENG;
                skaalaus = 32;
            }

            Image iDvv = Image.GetInstance(logo_tiedosto);
            iDvv.SetAbsolutePosition(45, 735);
            iDvv.ScalePercent(skaalaus);
            cb.AddImage(iDvv);
        }

        // Tietojen asemointi pystysuunnassa, alkuarvon asetus
        YPositio = YPositioAlkuarvo;

        return cb;
    }

    private PdfContentByte TaydennaSivunYlatunniste(PdfContentByte cb, string tulostuskieli, int sivunro, int sivuja, string aika, string diaarinumero)
    {
        PdfPTable table1 = new PdfPTable(1);

        // lisätään Päätös/Beslut
        table1.SetTotalWidth(new float[] { 200 });
        table1.AddCell(CreateCell((tulostuskieli == "1" ? "PÄÄTÖS" : (tulostuskieli == "2" ? "BESLUT" : "")), f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

        table1.WriteSelectedRows(0, -1, 315, 800, cb);

        // lisätään päiväys ko. sivulle
        PdfPTable table2 = new PdfPTable(1);
        table2.SetTotalWidth(new float[] { 140 });
        table2.AddCell(CreateCell(aika, f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

        table2.WriteSelectedRows(0, -1, 315, 755, cb);

        // lisätään sivunumero ko. sivulle
        PdfPTable table3 = new PdfPTable(1);
        table3.SetTotalWidth(new float[] { 140 });
        table3.AddCell(CreateCell(sivunro + " (" + sivuja + ")", f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));
        table3.WriteSelectedRows(0, -1, 515, 800, cb);

        // lisätään diaarinumero ko. sivulle
        PdfPTable table4 = new PdfPTable(1);
        table4.SetTotalWidth(new float[] { 140 });
        table4.AddCell(CreateCell(diaarinumero, f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

        table4.WriteSelectedRows(0, -1, 415, 780, cb);

        return cb;
    }

    private byte[] TaydennaYlatunniste(byte[] pdfByte, string aika, string tulostuskieli, string diaarinumero)
    {

        var output = new MemoryStream();
        var reader = new PdfReader(pdfByte);
        var stamper = new PdfStamper(reader, output);

        int numberOfPages = reader.NumberOfPages;

        for (int i = 1; i <= numberOfPages; ++i)
        {
            PdfContentByte cb = stamper.GetOverContent(i);

            PdfPTable table1 = new PdfPTable(1);
            if (bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella)
            {
                // lisätään Päätös/Beslut

                table1.SetTotalWidth(new float[] { 200 });
                table1.AddCell(CreateCell((tulostuskieli == "1" ? "PÄÄTÖS" : (tulostuskieli == "2" ? "BESLUT" : "")), f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));
            }
            else
            {
                // lisätään Kirje/Brev/Letter

                table1.SetTotalWidth(new float[] { 200 });
                table1.AddCell(CreateCell((tulostuskieli == "1" ? "KIRJE" : (tulostuskieli == "2" ? "BREV" : (tulostuskieli == "3" ? "LETTER" : ""))), f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));
            }

            table1.WriteSelectedRows(0, -1, 315, 800, cb);

            // lisätään päiväys ko. sivulle
            PdfPTable table2 = new PdfPTable(1);
            table2.SetTotalWidth(new float[] { 140 });
            table2.AddCell(CreateCell(aika, f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

            table2.WriteSelectedRows(0, -1, 315, 755, cb);

            // lisätään sivunumero ko. sivulle
            PdfPTable table3 = new PdfPTable(1);
            table3.SetTotalWidth(new float[] { 140 });
            table3.AddCell(CreateCell(i + " (" + numberOfPages + ")", f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

            table3.WriteSelectedRows(0, -1, 515, 800, cb);

            // lisätään diaarinumero ko. sivulle
            PdfPTable table4 = new PdfPTable(1);
            table4.SetTotalWidth(new float[] { 140 });
            table4.AddCell(CreateCell(diaarinumero, f_ssans_regular, 12, c_6f6f6e, c_ffffff, padding_0_0_0));

            table4.WriteSelectedRows(0, -1, 415, 780, cb);

        }

        stamper.FormFlattening = false;
        stamper.Close();
        reader.Close();

        return output.ToArray();
    }

    private PdfContentByte LisaaAlatunniste(PdfContentByte cb, string tulostuskieli)
    {
        if (bKirje || bKirjeKELA || bKirjeETK || bMuuttoilmoitusPaatos || bMuuttoilmoitusPaatosSaatteella || bKotikuntaPaatos || bKotikuntaPaatosSaatteella)
        {
            // DVV:n alatunniste
            string alatunniste_tiedosto;
            int skaalaus = 100;

            if (tulostuskieli == "1")
            {
                alatunniste_tiedosto = DVV_alatunniste_FIN;
                skaalaus = 22;
            }
            else if (tulostuskieli == "2")
            {
                alatunniste_tiedosto = DVV_alatunniste_SWE;
                skaalaus = 22;
            }
            else
            {
                alatunniste_tiedosto = DVV_alatunniste_ENG;
                skaalaus = 22;
            }

            Image iDvv = Image.GetInstance(alatunniste_tiedosto);
            iDvv.SetAbsolutePosition(33, 4);
            iDvv.ScalePercent(skaalaus);
            cb.AddImage(iDvv);
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

    private PdfPCell tauluOtsikko(string text)
    {
        return CreateCell(text, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_20_0, Element.ALIGN_LEFT, true);
    }

    private PdfPCell riviOtsikko(string text)
    {
        return CreateCell(text, f_ssans_regular, 12, c_575756, c_e9e9f2, padding_20_0_0);
    }

    private PdfPCell tyhja()
    {
        return CreateCell(" ", f_ssans_regular, 12, c_575756, c_e9e9f2, paddingt_20_0_0);
    }

    private PdfPCell riviData(string text)
    {
        return CreateCell(text, f_fira_medium, 10, c_1d1d1b, c_e9e9f2, padding_0_0_0);
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

    private PdfPCell CreateLinkCell(string text, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor linkcolor, BaseColor bcolor, float[] padding)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);
        Font linkfont = new Font(bfont, fsize, Font.NORMAL, linkcolor);

        string teksti = text;
        string linkki = "";

        int indx = text.IndexOf(":");
        if (indx > -1)
        {
            teksti = text.Substring(0, indx + 2);
            linkki = text.Substring(indx + 2).Trim();
        }

        Anchor ankkuri = new Anchor(new Phrase(new Chunk(linkki, linkfont)));
        ankkuri.Reference = linkki;

        Phrase phrase = new Phrase();
        phrase.Add(new Chunk(teksti, font));
        phrase.Add(ankkuri);

        PdfPCell cell = new PdfPCell(phrase);

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

    private PdfPCell CreateLinkCell2(string text, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor linkcolor, BaseColor bcolor, float[] padding)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);
        Font linkfont = new Font(bfont, fsize, Font.NORMAL, linkcolor);

        string teksti = text;
        string linkki = "";

        int indx = text.IndexOf(":");
        if (indx > -1)
        {
            teksti = text.Substring(0, indx + 2);
            linkki = text.Substring(indx + 2).Trim();
        }

        Phrase phrase = new Phrase();
        phrase.Add(new Chunk(teksti, font));
        phrase.Add(new Chunk(linkki, linkfont));

        PdfPCell cell = new PdfPCell(phrase);

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

    private PdfPCell CreateLinkCell3(string text, string linkki, BaseFont bfont, float fsize, BaseColor fcolor, BaseColor linkcolor, BaseColor bcolor, float[] padding)
    {
        Font font = new Font(bfont, fsize, Font.NORMAL, fcolor);
        Font linkfont = new Font(bfont, fsize, Font.NORMAL, linkcolor);

        string teksti = text.Replace("<br />", "").Trim();
        string teksti1 = "";
        string teksti2 = "";
        string linkkiteksti1 = "";
        string linkkiteksti2 = "";

        int indx = teksti.IndexOf(". ");
        if (indx > -1)
        {
            teksti1 = teksti.Substring(0, indx);
            teksti2 = teksti.Substring(indx);
        }

        indx = teksti1.LastIndexOf(" ");
        if (indx > -1)
        {
            linkkiteksti1 = teksti1.Substring(indx + 1);
            teksti1 = teksti1.Substring(0, indx + 1);
        }

        indx = teksti2.LastIndexOf(" ");
        if (indx > -1)
        {
            linkkiteksti2 = teksti2.Substring(indx + 1);
            teksti2 = teksti2.Substring(0, indx + 1);
        }

        Anchor ankkuri1 = new Anchor(new Phrase(new Chunk(linkkiteksti1, linkfont)));
        ankkuri1.Reference = linkki;

        Phrase phrase = new Phrase();
        phrase.Add(new Chunk(teksti1, font));
        phrase.Add(ankkuri1);
        phrase.Add(new Chunk(teksti2, font));
        phrase.Add(new Chunk(linkkiteksti2, linkfont));

        PdfPCell cell = new PdfPCell(phrase);

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


}
