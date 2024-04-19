using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using iTextSharp.text.pdf;

/// <summary>
/// Summary description for AETToPdf
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class AETToPdf : System.Web.Services.WebService
{

    public AETToPdf()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    /// <summary>
    /// Täytetään 'Avioliiton esteiden tutkinta' -lomake
    /// </summary>
    [WebMethod]
    public byte[] MuodostaPdf(HenkilonTiedot henA, HenkilonTiedot henB, UlkomaisenLisatiedot ultMies, UlkomaisenLisatiedot ultNainen, SukunimivalinnanTiedot st, VirkailijanTiedot vt, bool MiesOnUlkomaalainen, bool NainenOnUlkomaalainen, string tulostuskieli, bool miehenUskonto, bool miehenRippi, bool naisenUskonto, bool naisenRippi, string miehenHetu, string naisenHetu)
    {
        //pdf-lomake
        string pdfPath = System.Configuration.ConfigurationManager.AppSettings["AET_tutkintatodistus"] + tulostuskieli + ".pdf";

        var output = new MemoryStream();
        var reader = new PdfReader(pdfPath);
        var stamper = new PdfStamper(reader, output);
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


        //foreach (var fieldName in formFieldMap.Keys)
        //    formFields.SetField(fieldName, formFieldMap[fieldName]);



        //formFields.SetField("viranomainen", vt.Allekirjoitus_ABtayttanytViranomainen);

        //miehen tiedot 
        formFields.SetField("mnimi", henA.NykyinenSukunimi);
        formFields.SetField("metun", henA.NykyinenEtunimi);
        formFields.SetField("mhetu", henA.Hetu);
        formFields.SetField("mnimiv", ((!string.IsNullOrEmpty(henA.ViimNaimattomanaSukunimi)) ? henA.ViimNaimattomanaSukunimi : henA.NykyinenSukunimi));


        formFields.SetField("mkans", henA.Kansalaisuus);
//        formFields.SetField("mkans", henA.Kansalaisuuskoodi + " " + henA.Kansalaisuus);
//        formFields.SetField("masv", ultMies.AsuinmaaKoodi + " " + ultMies.AsuinmaaNimi);
        formFields.SetField("masv", henA.Asuinvaltio);
        
        //VTJKYS-914 Pakallinen rekisteriviranomainen poistettu
        //formFields.SetField("mprv", henA.Maistraatti);

//        formFields.SetField("mkans", (henA.Kansalaisuuskoodi != "246" ? henA.Kansalaisuus : ""));

        formFields.SetField("Anaim", (henA.Siviilisaatykoodi == "1" ? "0" : 
                                     (henA.Siviilisaatykoodi == "4" || henA.Siviilisaatykoodi == "7" ? "1" : 
                                     (henA.Siviilisaatykoodi == "5" || henA.Siviilisaatykoodi == "8" ? "2" : "Off"))));
        /*
        formFields.SetField("mnton", (henA.Siviilisaatykoodi == "1" ? "x" : "")); //mies: naimaton 
        formFields.SetField("meron", (henA.Siviilisaatykoodi == "4" || henA.Siviilisaatykoodi == "7" ? "x" : "")); //mies: eronnut, eronnut rekpa 
        formFields.SetField("mleski", (henA.Siviilisaatykoodi == "5" || henA.Siviilisaatykoodi == "8" ? "x" : "")); //mies: leski, leski rekpan jälkeen 
        */

        int tmpmonesko = ((henA.Moneskoliitto != "") ? int.Parse(henA.Moneskoliitto) : 1);
        formFields.SetField("avnr 1", tmpmonesko.ToString());

//        formFields.SetField("mhkirj", henA.Kotikunta);

        formFields.SetField("Aks", ("200".Equals(henA.Kotikuntakoodi) || "198".Equals(henA.Kotikuntakoodi) || (string.IsNullOrEmpty(miehenHetu)) ? "1" : "Off"));
        //formFields.SetField("ej1", ("200".Equals(henA.Kotikuntakoodi) || "198".Equals(henA.Kotikuntakoodi) || (string.IsNullOrEmpty(miehenHetu)) ? "x" : ""));

        //formFields.SetField("Aevl", (miehenRippi && ("Kyllä".Equals(henA.Rippikoulu) || "Ja".Equals(henA.Rippikoulu)) ? "1" : 
        //                            (miehenRippi && ("Ei".Equals(henA.Rippikoulu) || "Nej".Equals(henA.Rippikoulu)) ? "2" : "Off")));

        //VTJKYS-2236 muutoksen jälkeen 'Ei' tarkkoittaan 'Tietoja ei voida saada' - value "3"
        formFields.SetField("Aevl", (miehenRippi && ("Kyllä".Equals(henA.Rippikoulu) || "Ja".Equals(henA.Rippikoulu)) ? "0" :
                            (miehenRippi && ("Ei".Equals(henA.Rippikoulu) || "Nej".Equals(henA.Rippikoulu)) ? "3" :
                            (("Ei haettu".Equals(henA.Rippikoulu) || "Ej sökt".Equals(henA.Rippikoulu)) ? "2" : "Off"))));

        //formFields.SetField("mKrk", ((miehenRippi && "Kyllä".Equals(henA.Rippikoulu)) ? "x" : "") + ((miehenRippi && "Ja".Equals(henA.Rippikoulu)) ? "x" : ""));
        //formFields.SetField("mErk", ((miehenRippi && "Ei".Equals(henA.Rippikoulu)) ? "x" : "") + ((miehenRippi && "Nej".Equals(henA.Rippikoulu)) ? "x" : ""));
        formFields.SetField("mkirk", (miehenUskonto ? henA.Uskontokunta : ""));

        //naisen tiedot 
        formFields.SetField("nnimi", henB.NykyinenSukunimi);
        formFields.SetField("netun", henB.NykyinenEtunimi);
        formFields.SetField("nhetu", henB.Hetu);
        formFields.SetField("nnimiv", ((!string.IsNullOrEmpty(henB.ViimNaimattomanaSukunimi)) ? henB.ViimNaimattomanaSukunimi : henB.NykyinenSukunimi));

        formFields.SetField("nkans", henB.Kansalaisuus);
//        formFields.SetField("nkans", henB.Kansalaisuuskoodi + " " + henB.Kansalaisuus);
//        formFields.SetField("nasv", ultNainen.AsuinmaaKoodi + " " + ultNainen.AsuinmaaNimi);
        formFields.SetField("nasv", henB.Asuinvaltio);

        //VTJKYS-914 Pakallinen rekisteriviranomainen poistettu
        //formFields.SetField("nprv", henB.Maistraatti);

//        formFields.SetField("nkans", (henB.Kansalaisuuskoodi != "246" ? henB.Kansalaisuus : ""));

        formFields.SetField("Bnaim", (henB.Siviilisaatykoodi == "1" ? "0" :
                                     (henB.Siviilisaatykoodi == "4" || henB.Siviilisaatykoodi == "7" ? "1" :
                                     (henB.Siviilisaatykoodi == "5" || henB.Siviilisaatykoodi == "8" ? "2" : "Off"))));
        /*
        formFields.SetField("nnton", (henB.Siviilisaatykoodi == "1" ? "x" : "")); //nainen: naimaton 
        formFields.SetField("neron", (henB.Siviilisaatykoodi == "4" || henB.Siviilisaatykoodi == "7" ? "x" : "")); //nainen: eronnut, eronnut rekpa 
        formFields.SetField("nleski", (henB.Siviilisaatykoodi == "5" || henB.Siviilisaatykoodi == "8" ? "x" : "")); //nainen: leski, leski rekpan jälkeen 
        */

        tmpmonesko = ((henB.Moneskoliitto != "") ? int.Parse(henB.Moneskoliitto) : 1);
        formFields.SetField("avnr 2", tmpmonesko.ToString());

//        formFields.SetField("nhkirj", henB.Kotikunta);

//        formFields.SetField("Bks", ("200".Equals(henB.Kotikuntakoodi) || "198".Equals(henB.Kotikuntakoodi) || (string.IsNullOrEmpty(naisenHetu)) ? "1" : "Off"));
        //formFields.SetField("ej2", ("200".Equals(henB.Kotikuntakoodi) || "198".Equals(henB.Kotikuntakoodi) || (string.IsNullOrEmpty(naisenHetu)) ? "x" : ""));

        //VTJKYS-2236 muutoksen jälkeen 'Ei' tarkkoittaan 'Tietoja ei voida saada' - value "3"
        formFields.SetField("Bevl", (naisenRippi && ("Kyllä".Equals(henB.Rippikoulu) || "Ja".Equals(henB.Rippikoulu)) ? "0" :
                                    (naisenRippi && ("Ei".Equals(henB.Rippikoulu) || "Nej".Equals(henB.Rippikoulu)) ? "3"  :
                                    (("Ei haettu".Equals(henB.Rippikoulu) || "Ej sökt".Equals(henB.Rippikoulu)) ? "2" : "Off"))));

        //formFields.SetField("nKrk", ((naisenRippi && "Kyllä".Equals(henB.Rippikoulu)) ? "x" : "") + ((naisenRippi && "Ja".Equals(henB.Rippikoulu)) ? "x" : ""));
        //formFields.SetField("nErk", ((naisenRippi && "Ei".Equals(henB.Rippikoulu)) ? "x" : "") + ((naisenRippi && "Nej".Equals(henB.Rippikoulu)) ? "x" : ""));
        formFields.SetField("nkirk", (naisenUskonto ? henB.Uskontokunta : ""));



        ////ulkomaisen lisätiedot 
        ////molemmat ulk
        //if (!string.IsNullOrEmpty(ultMies.AidinkieliKoodi) && !string.IsNullOrEmpty(ultNainen.AidinkieliKoodi))
        //{
        //    string tmp1 = "";
        //    if (henA.Kansalaisuuskoodi != "246" && MiesOnUlkomaalainen == true)
        //        //tmp1 = henA.Kansalaisuus + ", ";
        //        //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä ja ota kommentti pois ylemmästä
        //        tmp1 = henA.Kansalaisuuskoodi + " " + henA.Kansalaisuus + ", ";


        //    if (henB.Kansalaisuuskoodi != "246" && NainenOnUlkomaalainen == true)
        //        // tmp1 += henB.Kansalaisuus;
        //        //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä ja ota kommentti pois ylemmästä
        //        tmp1 += henB.Kansalaisuuskoodi + " " + henB.Kansalaisuus;


        //    formFields.SetField("kansx1", tmp1);

        //    tmp1 = ultMies.AidinkieliNimi + ", " + ultNainen.AidinkieliNimi;

        //    //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä
        //    tmp1 = ultMies.AidinkieliKoodi + " " + ultMies.AidinkieliNimi + ", " + ultNainen.AidinkieliKoodi + " " + ultNainen.AidinkieliNimi;


        //    formFields.SetField("kielix1", tmp1);
        //    tmp1 = ultMies.AsuinmaaNimi + ", " + ultNainen.AsuinmaaNimi;

        //    //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä
        //    tmp1 = ultMies.AsuinmaaKoodi + " " + ultMies.AsuinmaaNimi + ", " + ultNainen.AsuinmaaKoodi + " " + ultNainen.AsuinmaaNimi;


        //    formFields.SetField("asuinmaax1", tmp1);
        //}
        //else if (!string.IsNullOrEmpty(ultMies.AidinkieliKoodi))
        //{
        //    //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä
        //    if (henA.Kansalaisuuskoodi != "246" && MiesOnUlkomaalainen == true)
        //        formFields.SetField("kansx1", henA.Kansalaisuuskoodi + " " + henA.Kansalaisuus);
        //    formFields.SetField("kielix1", ultMies.AidinkieliKoodi + " " + ultMies.AidinkieliNimi);
        //    formFields.SetField("asuinmaax1", ultMies.AsuinmaaKoodi + " " + ultMies.AsuinmaaNimi);
        //    //tähän saakka ja ota alempi lohko pois kommenteista
        //    /*
        //    if (henA.Kansalaisuuskoodi != "246" && MiesOnUlkomaalainen == true)
        //        formFields.SetField("kansx1", henA.Kansalaisuus);
        //    formFields.SetField("kielix1", ultMies.AidinkieliNimi);
        //    formFields.SetField("asuinmaax1", ultMies.AsuinmaaNimi);
        //    */
        //}
        //else if (!string.IsNullOrEmpty(ultNainen.AidinkieliKoodi))
        //{
        //    //Jos kansalaisuuskoodit halutaan taas pois niin kommentoi tämä
        //    if (henB.Kansalaisuuskoodi != "246" && NainenOnUlkomaalainen == true)
        //        formFields.SetField("kansx1", henB.Kansalaisuuskoodi + " " + henB.Kansalaisuus);

        //    formFields.SetField("kielix1", ultNainen.AidinkieliKoodi + " " + ultNainen.AidinkieliNimi);
        //    formFields.SetField("asuinmaax1", ultNainen.AsuinmaaKoodi + " " + ultNainen.AsuinmaaNimi);
        //    //tähän saakka  ja ota alempi lohko pois kommenteista
        //    /*
        //    if (henB.Kansalaisuuskoodi != "246" && NainenOnUlkomaalainen == true)
        //        formFields.SetField("kansx1", henB.Kansalaisuus);

        //    formFields.SetField("kielix1", ultNainen.AidinkieliNimi);
        //    formFields.SetField("asuinmaax1", ultNainen.AsuinmaaNimi);
        //    */
        //}

        //Sukunimivalinta
        formFields.SetField("animi", st.PuolisonASukunimi);
        formFields.SetField("bnimi", st.PuolisonBSukunimi);

        formFields.SetField("AsukunKyll", (st.LainmukainenAK == true ? "On" : "OFF"));
        formFields.SetField("AsukunEi", (st.LainmukainenAE == true ? "On" : "OFF"));
        formFields.SetField("BsukunKyll", (st.LainmukainenBK == true ? "On" : "OFF"));
        formFields.SetField("BsukunEi", (st.LainmukainenBE == true ? "On" : "OFF"));
        formFields.SetField("AeiToim", (st.EiValtaaTutkiaA == true ? "On" : "OFF"));
        formFields.SetField("BeiToim", (st.EiValtaaTutkiaB == true ? "On" : "OFF"));

//        formFields.SetField("Lainmuk_K", (st.LainmukainenK == true ? "x" : ""));
//        formFields.SetField("Lainmuk_E", (st.LainmukainenE == true ? "x" : ""));

        //Esteiden tutkinta 
        formFields.SetField("pv_pyyd", vt.TutkintaPaiva);
        formFields.SetField("pv_ann", vt.TutkintaAnnettu);
        formFields.SetField("Lisatiet 1", vt.Allekirjoitus_Lisatiedot);
      
//        formFields.SetField("numero2", vt.TutkinnanNumero);

        formFields.SetField("Astunn", vt.TutkinnanNumero);
//        formFields.SetField("tutk_valt", vt.UlkomKihlEsteetTutkValtio);
        //formFields.SetField("datum1", vt.Allekirjoitus_Paikka + "  " + vt.Allekirjoitus_Aika);
        //formFields.SetField("tunnistautuminen1", vt.Allekirjoitus_Nimi + ", " + vt.Allekirjoitus_VirkaAsema);
        formFields.SetField("datum1", vt.Allekirjoitus_Aika + ", " + vt.Allekirjoitus_Nimi + ", " + vt.Allekirjoitus_VirkaAsema);
        formFields.SetField("tunnistautuminen1", vt.Allekirjoitus_ABtayttanytViranomainen);



        stamper.FormFlattening = false; //Form fields should no longer be editable
        stamper.Close();
        reader.Close();

        return output.ToArray();

        /* Once the PdfStamper has been closed the stream specified when instantiating the PdfStamper object contains the generated PDF. 
         * If you used a FileStream object then that means the generated PDF now exists on disk. 
         * This case we use a MemoryStream, which means the generated PDF now resides in memory. 
         * At this point we're ready to send it back to the browser for display. 
         * output.ToArray() returns the contents of the MemoryStream 
         * - namely, the binary contents of the generated PDF document - as a byte array, which is then sent down to the client. 
        */
         
    }


    /// <summary>
    /// Täytetään 'Kelpoisuus solmia avioliitto ulkomailla' -lomake
    /// </summary>
    [WebMethod]
    public byte[] MuodostaPdfUlkom(HenkilonTiedot henA, HenkilonTiedot henB, VirkailijanTiedot vt, string tulostuskieli)
    {
        try
        {
            //pdf-lomake
            string pdfPath = System.Configuration.ConfigurationManager.AppSettings["AET_tutkintatodistusUlkom"] + tulostuskieli + ".pdf";

            var output = new MemoryStream();
            var reader = new PdfReader(pdfPath);
            var stamper = new PdfStamper(reader, output);
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

            // Henkilön A tiedot ----------------------------------------------------------------
            // Nimet
            formFields.SetField("suknim1", henA.NykyinenSukunimi);
            formFields.SetField("etunim1", henA.NykyinenEtunimi);

            // Syntymäaika
            formFields.SetField("syntaika1", henA.Syntymaaika);

            // Syntymäpaikka ja -valtio
            string syntymapaikkaJavaltio = string.Empty;
            syntymapaikkaJavaltio = (string.IsNullOrEmpty(henA.Syntymapaikka) ? "" : henA.Syntymapaikka + " ")
                                  + (string.IsNullOrEmpty(henA.Syntymavaltio) ? "" : henA.Syntymavaltio);
            formFields.SetField("syntpaikka1", (string.IsNullOrEmpty(syntymapaikkaJavaltio) ? "" : syntymapaikkaJavaltio));

            // Kansalaisuus
            formFields.SetField("kans1", henA.Kansalaisuus);

            // Siviilisääty
            formFields.SetField("sivsaat1", (henA.Siviilisaatykoodi == "1" ? "0" :                                              // Naimaton
                                            (henA.Siviilisaatykoodi == "4" || henA.Siviilisaatykoodi == "7" ? "1" :             // Eronnut, Eronnut rekisteröidystä parisuhteesta
                                            (henA.Siviilisaatykoodi == "5" || henA.Siviilisaatykoodi == "8" ? "2" : "Off"))));  // Leski, Leski rekisteröidystä parisuhteesta

            // Asuinvaltio
            formFields.SetField("asuinvalt1", henA.Asuinvaltio);

            // Avioliittojen lkm
            formFields.SetField("avioliit1", (string.IsNullOrEmpty(henA.Moneskoliitto) ? "" : henA.Moneskoliitto));

            // Henkilön B tiedot ----------------------------------------------------------------
            // Nimet
            formFields.SetField("suknim2", henB.NykyinenSukunimi);
            formFields.SetField("etunim2", henB.NykyinenEtunimi);

            // Syntymäaika
            formFields.SetField("syntaika2", henB.Hetu);

            // Syntymäpaikka ja -valtio
            syntymapaikkaJavaltio = (string.IsNullOrEmpty(henB.Syntymapaikka) ? "" : henB.Syntymapaikka + " ")
                                  + (string.IsNullOrEmpty(henB.Syntymavaltio) ? "" : henB.Syntymavaltio);
            formFields.SetField("syntpaikka2", (string.IsNullOrEmpty(syntymapaikkaJavaltio) ? "" : syntymapaikkaJavaltio));

            // Kansalaisuus
            formFields.SetField("kans2", henB.Kansalaisuus);

            // Siviilisääty
            formFields.SetField("sivsaat2", (henB.Siviilisaatykoodi == "1" ? "0" :                                              // Naimaton
                                            (henB.Siviilisaatykoodi == "4" || henB.Siviilisaatykoodi == "7" ? "1" :             // Eonnut, Eronnut rekisteröidystä parisuhteesta
                                            (henB.Siviilisaatykoodi == "5" || henB.Siviilisaatykoodi == "8" ? "2" : "Off"))));  // Leski, Leski rekisteröidystä parisuhteesta

            // Asuinvaltio
            formFields.SetField("asuinvalt2", henB.Asuinvaltio);

            // Avioliittojen lkm
            formFields.SetField("avioliit2", (string.IsNullOrEmpty(henB.Moneskoliitto) ? "" : henB.Moneskoliitto));


            // Allekirjoitustiedot ----------------------------------------------------------- 

            formFields.SetField("todAnt", vt.Allekirjoitus_ABtayttanytViranomainen);
            formFields.SetField("asiat", vt.TutkinnanNumero);
            formFields.SetField("paikkapvm", vt.Allekirjoitus_Paikka + " " + vt.Allekirjoitus_Aika);
            formFields.SetField("nimensel", vt.Allekirjoitus_Nimi + ", " + vt.Allekirjoitus_VirkaAsema);


            stamper.FormFlattening = false; //Form fields should no longer be editable
            stamper.Close();
            reader.Close();

            return output.ToArray();
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    /// <summary>
    /// Täytetään 'Kelpoisuus solmia avioliitto ulkomailla' EU-vakiolomake
    /// </summary>
    [WebMethod]
    public byte[] MuodostaPdfUlkomVakiolomake(HenkilonTiedot henA, HenkilonTiedot henB, VirkailijanTiedot vt, string tulostuskieli, string kohdemaa)
    {
        try
        {
            string chkboxValittu = "Yes";
            char[] merkit = new char[] { '.', '/' };
            string tyhja = "-";

            // EU-vakiolomakepohja 
            string pdfPath = System.Configuration.ConfigurationManager.AppSettings["EU_vakiolomake"];
            pdfPath = pdfPath + "FI_CapacityToMarry_" + (tulostuskieli.Equals("_SE") ? "SV" : "FI") + "_" + kohdemaa + ".pdf";

            var output = new MemoryStream();
            var reader = new PdfReader(pdfPath);
            var stamper = new PdfStamper(reader, output);
            var formFields = stamper.AcroFields;

            // Alustetaan lomakkeen tekstikentät "-" merkillä
            var fieldKeys = formFields.Fields.Keys;
            foreach (string fieldKey in fieldKeys)
            {
                if (formFields.GetFieldType(fieldKey) == AcroFields.FIELD_TYPE_TEXT)
                {
                    formFields.SetField(fieldKey, tyhja);
                }
            }

            // -- OTTEEN ANTAJAN TIEDOT --------------------------------------------
            formFields.SetField("text_63", (string.IsNullOrEmpty(vt.Allekirjoitus_ABtayttanytViranomainen) ? tyhja : vt.Allekirjoitus_ABtayttanytViranomainen));
            formFields.SetField("text_65", (string.IsNullOrEmpty(vt.Allekirjoitus_ABtayttanytViranomainen) ? tyhja : vt.Allekirjoitus_ABtayttanytViranomainen));

            // Hallinnollinen asiakirja, Todistus
            formFields.SetField("checkbox_81", chkboxValittu);
            formFields.SetField("checkbox_82", chkboxValittu);

            formFields.SetField("text_date_94", (string.IsNullOrEmpty(vt.Allekirjoitus_Aika) ? tyhja : vt.Allekirjoitus_Aika.Replace(merkit[0], merkit[1])));
            formFields.SetField("text_97", (string.IsNullOrEmpty(vt.Allekirjoitus_Paikka) ? tyhja : vt.Allekirjoitus_Paikka));

            string allekirjoittaja = string.Empty;
            allekirjoittaja = (string.IsNullOrEmpty(vt.Allekirjoitus_Nimi) ? "" : vt.Allekirjoitus_Nimi)
                            + (string.IsNullOrEmpty(vt.Allekirjoitus_VirkaAsema) ? "" : ", " + vt.Allekirjoitus_VirkaAsema);
            formFields.SetField("text_100", (string.IsNullOrEmpty(allekirjoittaja) ? tyhja : allekirjoittaja));

            // Yleisen asiakirjan viitenumero
            formFields.SetField("text_107", (string.IsNullOrEmpty(vt.TutkinnanNumero) ? tyhja : vt.TutkinnanNumero));

            // -- ASIANOMAISEN HENKILÖN TIEDOT -------------------------------------
            // Nimet
            formFields.SetField("text_125", (string.IsNullOrEmpty(henA.NykyinenSukunimi) ? tyhja : henA.NykyinenSukunimi));
            formFields.SetField("text_130", (string.IsNullOrEmpty(henA.NykyinenEtunimi) ? tyhja : henA.NykyinenEtunimi));

            // Syntymäaika
            formFields.SetField("text_date_137", (string.IsNullOrEmpty(henA.Syntymaaika) ? tyhja : henA.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Syntymäpaikka ja -maa
            string syntymapaikka = string.Empty;
            syntymapaikka = (string.IsNullOrEmpty(henA.Syntymapaikka) ? "" : henA.Syntymapaikka + " ")
                          + (string.IsNullOrEmpty(henA.Syntymavaltio) ? "" : henA.Syntymavaltio);
            formFields.SetField("text_145", (string.IsNullOrEmpty(syntymapaikka) ? tyhja : syntymapaikka));

            // Kansalaisuus
            switch (henA.Kansalaisuus)
            {
                case "Ei selvit":
                case "Ei selvit (99)":
                case "Ej uträtt":
                case "Ej uträtt (99)":
                case "Not yet determined":
                case "Not yet determined (99)":
                    formFields.SetField("checkbox_163", chkboxValittu);
                    break;
                case "Tuntematon":
                case "Tuntematon (99)":
                case "Okänt":
                case "Okänt (99)":
                case "Unknown":
                case "Unknown (99)":
                    formFields.SetField("checkbox_164", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_159", (string.IsNullOrEmpty(henA.Kansalaisuus) ? tyhja : henA.Kansalaisuus));
                    break;
            }

            // Siviilisääty
            switch (henA.Siviilisaatykoodi)
            {
                case "1":
                    formFields.SetField("checkbox_279", chkboxValittu);
                    break;
                case "4":
                case "7":
                    formFields.SetField("checkbox_280", chkboxValittu);
                    break;
                case "5":
                case "8":
                    formFields.SetField("checkbox_281", chkboxValittu);
                    break;
            }

            // Asuinvaltio ?? puuttuu lomakkeelta
            //formFields.SetField("text_???", (string.IsNullOrEmpty(henA.Asuinvaltio) ? tyhja : henA.Asuinvaltio));

            // Avioliittojen lkm
            formFields.SetField("text_267", (string.IsNullOrEmpty(henA.Moneskoliitto) ? tyhja : henA.Moneskoliitto));

            // SEN YLEISEN ASIAKIRJAN MUKAAN, JOHON TÄMÄ ASIAKIRJA LIITETÄÄN, kohta 5.3 -checkbox
            formFields.SetField("checkbox_292", chkboxValittu);

            // -- ASIANOMAISEN HENKILÖN TULEVAN PUOLISON TIEDOT -----------------------------------------
            // Huom. Alla olevat ulkomaalaisen kihlakumppanin tiedot perustuvat henkilön omaan ilmoitukseen -checkbox
            formFields.SetField("checkbox_309", chkboxValittu); 

            // Nimet
            formFields.SetField("text_310", (string.IsNullOrEmpty(henB.NykyinenSukunimi) ? tyhja : henB.NykyinenSukunimi));
            formFields.SetField("text_315", (string.IsNullOrEmpty(henB.NykyinenEtunimi) ? tyhja : henB.NykyinenEtunimi));

            // Syntymäaika
            formFields.SetField("text_date_320", (string.IsNullOrEmpty(henB.Syntymaaika) ? tyhja : henB.Syntymaaika.Replace(merkit[0], merkit[1])));

            // Syntymäpaikka ja -maa
            syntymapaikka = string.Empty;
            syntymapaikka = (string.IsNullOrEmpty(henB.Syntymapaikka) ? "" : henB.Syntymapaikka + " ")
                          + (string.IsNullOrEmpty(henB.Syntymavaltio) ? "" : henB.Syntymavaltio);
            formFields.SetField("text_325", (string.IsNullOrEmpty(syntymapaikka) ? tyhja : syntymapaikka));

            // Kansalaisuus
            switch (henB.Kansalaisuus)
            {
                case "Ei selvit":
                case "Ei selvit (99)":
                case "Ej uträtt":
                case "Ej uträtt (99)":
                case "Not yet determined":
                case "Not yet determined (99)":
                    formFields.SetField("checkbox_340", chkboxValittu);
                    break;
                case "Tuntematon":
                case "Tuntematon (99)":
                case "Okänt":
                case "Okänt (99)":
                case "Unknown":
                case "Unknown (99)":
                    formFields.SetField("checkbox_342", chkboxValittu);
                    break;
                default:
                    formFields.SetField("text_336", (string.IsNullOrEmpty(henB.Kansalaisuus) ? tyhja : henB.Kansalaisuus));
                    break;
            }

            // Siviilisääty
            switch (henB.Siviilisaatykoodi)
            {
                case "1":
                    formFields.SetField("checkbox_411", chkboxValittu);
                    break;
                case "4":
                case "7":
                    formFields.SetField("checkbox_412", chkboxValittu);
                    break;
                case "5":
                case "8":
                    formFields.SetField("checkbox_413", chkboxValittu);
                    break;
            }

            // Asuinvaltio
            formFields.SetField("text_419", (string.IsNullOrEmpty(henB.Asuinvaltio) ? tyhja : henB.Asuinvaltio));

            // Avioliittojen lkm
            formFields.SetField("text_400", (string.IsNullOrEmpty(henB.Moneskoliitto) ? tyhja : henB.Moneskoliitto));


            // -- ALLEKIRJOITTAJAN TIEDOT --------------------------------
            formFields.SetField("text_454", (string.IsNullOrEmpty(vt.Allekirjoitus_Nimi) ? tyhja : vt.Allekirjoitus_Nimi));
            formFields.SetField("text_455", (string.IsNullOrEmpty(vt.Allekirjoitus_VirkaAsema) ? tyhja : vt.Allekirjoitus_VirkaAsema));
            formFields.SetField("text_date_456", (string.IsNullOrEmpty(vt.Allekirjoitus_Aika) ? tyhja : vt.Allekirjoitus_Aika.Replace(merkit[0], merkit[1])));


            stamper.FormFlattening = false; //Form fields should no longer be editable
            stamper.Close();
            reader.Close();

            return output.ToArray();
        }
        catch (Exception ex)
        {
            throw;
        }

    }

}

