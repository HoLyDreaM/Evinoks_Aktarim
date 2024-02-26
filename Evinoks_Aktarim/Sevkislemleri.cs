using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using DevExpress.Utils;
using System.Xml;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;

namespace Evinoks_Aktarim
{
    public partial class Sevkislemleri : Form
    {
        public Sevkislemleri()
        {
            InitializeComponent();
        }

        #region Bağlantılar

        public AnaForm anaFrm
        {
            get;
            set;
        }

        Thread Sevk;
        DataTable dt;

        SqlConnection ConStk005;
        SqlCommand CmdStk005;
        SqlDataReader Stk005Dr;

        SqlConnection ConUpdateMrpStokKart;
        SqlCommand CmdUpdateMrpStokKart;
        SqlDataReader DrUpdateMrpStokKart;

        SqlConnection ConUpdateMrpCariKart;
        SqlCommand CmdUpdateMrpCariKart;
        SqlDataReader DrUpdateMrpCariKart;

        SqlConnection ConUpdateCariKart;
        SqlCommand CmdUpdateCariKart;
        SqlDataReader DrUpdateCariKart;

        SqlConnection ConUpdateSevkDet;
        SqlCommand CmdUpdateSevkDet;
        SqlDataReader DrUpdateSevkDet;

        SqlConnection ConUpdateSevkMst;
        SqlCommand CmdUpdateSevkMst;
        SqlDataReader DrUpdateSevkMst;

        SqlConnection ConSevkislemi;
        SqlCommand CmdSevkislemi;
        SqlDataReader SevkislemiDr;

        SqlConnection ConSevkislemi1;
        SqlCommand CmdSevkislemi1;
        SqlDataReader SevkislemiDr1;

        SqlConnection ConSCar;
        SqlCommand CmdSCar;
        SqlDataReader SCarDr;

        SqlConnection ConStk02Update;
        SqlCommand CmdStk02Update;
        SqlDataReader Stk02UpdateDr;

        SqlConnection ConStk002Kontrol;
        SqlCommand CmdStk002Kontrol;
        SqlDataReader Stk002KontrolDr;

        SqlConnection ConCariKontrol;
        SqlCommand CmdCariKontrol;
        SqlDataReader CariKontrolDr;

        SqlConnection ConLinkKontrol;
        SqlCommand CmdLinkKontrol;
        SqlDataReader LinkKontrolDr;

        SqlConnection ConCariAktar;
        SqlCommand CmdCariAktar;
        SqlDataReader CariAktarDr;

        SqlConnection ConSEQNo;
        SqlCommand cmdSEQNo;
        SqlDataReader SEQNoDr;

        SqlConnection ConStkUpdate;
        SqlCommand CmdStkUpdate;
        SqlDataReader StkUpdateDr;

        SqlConnection ConStk004;
        SqlCommand CmdStk004;
        SqlDataReader Stk004Dr;

        SqlConnection ConMrpStk;
        SqlCommand CmdMrpStk;
        SqlDataReader MrpStkDr;

        SqlConnection ConStkEkle;
        SqlCommand CmdStkEkle;
        SqlDataReader StkEkleDr;

        string Cs1 = Properties.Settings.Default.Cs1;
        string Cs2 = Properties.Settings.Default.Cs2;

        #endregion

        #region Tanımlar

        int siralamaMak = 1;
        int siralamaMak2;

        string ID;
        string MrpCariID;
        string LinkCariID;
        string LinkStokID;
        string SiparisID;
        string Sorgu;
        string Sirket;
        string MrpSirket;
        double Tarih;
        int TarihRakam;
        double SPTarih;
        int SPTarihRakam;
        double STTarih;
        int STTarihRakam;
        double CarFaturaTarih;
        int CarFaturaTarihRakam;
        int SEQNo;
        string MrpStokKodu;
        string MrpStokAciklama;
        int Stk004Durum;
        string Durum2;
        string PCAdi;
        string SiraID;
        string Urunler;
        string MalKodu;
        string EvrakNo;
        string DepoKodu;
        string HesapKodu;
        DateTime SevkTarihi;
        DateTime EklenmeTarihi;
        string Model;
        string Birim;
        int Miktar;
        decimal Fiyat;
        decimal Toplam;
        string CariKontrolDurum;
        int LinkCariDurumu;
        string Durum;
        string CariHesapAdi;
        string CariHesapKodu;
        string Kur;
        string Adres;
        int UlkeKodu;
        string UlkeKodu2;
        int BolgeKodu;
        string VergiDairesi;
        string VergiNo;
        string Telefon;

        #endregion

        #region Form Load İşlemleri

        private void Sevkislemleri_Load(object sender, EventArgs e)
        {
            ConSevkislemi = new SqlConnection(Cs1);
            ConSevkislemi1 = new SqlConnection(Cs2);
            Sirket = anaFrm.tsLinkSirket.Text;
            MrpSirket = anaFrm.MrpSirket.ToString();
        }

        #endregion

        #region Sevk Arama İşlemleri

        private void SevkArama()
        {
            ConSevkislemi = new SqlConnection(Cs1);
            ConSevkislemi1 = new SqlConnection(Cs2);

            btnAktar.Enabled = false;
            btnArama.Enabled = false;

            grdSevkislemi.DataSource = null;
            grdSevkislemi.DataSource = ds.Sevkislemi;

            Sorgu = "SELECT tbl.SiparisID,tbl.HesapKodu,tbl.MalKodu,tbl.EvrakNo,tbl.SevkTarih,tbl.EklenmeTarihi, " +
                    "tbl.Depo,tbl.Birim,SUM(tbl.Miktar) AS Miktar,tbl.Fiyat,ROUND(CONVERT(DECIMAL(21,2),SUM(tbl.Miktar)*tbl.Fiyat),2) AS Toplam, " +
                    "CONVERT(DECIMAL(21,2),SUM(tbl.Miktar)*tbl.Fiyat+ " +
                    "CONVERT(DECIMAL(21,2),SUM(tbl.Miktar)*tbl.Fiyat*tbl.Gumruk/100))*tbl.Kdv/100 AS Kdv, " +
                    "ROUND(CONVERT(DECIMAL(21,2),SUM(tbl.Miktar)*tbl.Fiyat*tbl.Gumruk/100),2) AS OtvTutari " +
                    "FROM(SELECT sevkdetay.sn AS SiparisID,(CASE  SUBSTRING(cus_code, 1, 1)  " +
                    "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1)  " +
                    "ELSE cus_code END) AS HesapKodu,STK002_MalKodu AS MalKodu, " +
                    "bill_number AS EvrakNo,sevk_tar AS SevkTarih, cre_date AS EklenmeTarihi,wh_code AS Depo, " +
                    "SUBSTRING(sevkdetay.um_code,1,LEN(sevkdetay.um_code)-1) AS Birim,sevkdetay.um_qty AS Miktar, " +
                    "STK002_BirimFiyati AS Fiyat,ISNULL(Sipler.tax_type,18) AS Kdv,StokKarti.STK004_GumrukOrani AS Gumruk  " +
                    "From " + MrpSirket.ToString() + ".dbo.ab_sevk_mst AS SevkMst  " +
                    "INNER JOIN " + MrpSirket.ToString() + ".dbo.ab_sevk_det AS SevkDetay On status='VAL' AND SevkMst.sevk_no=SevkDetay.sevk_no  " +
                    "INNER JOIN " + MrpSirket.ToString() + ".dbo.siparis_det_y AS Sipler On Sipler.sn=SevkDetay.sn " +
                    "INNER JOIN YNS" + Sirket.ToString() + ".STK004 AS StokKarti ON STK004_MalKodu=SevkDetay.sku " +
                    "INNER JOIN YNS" + Sirket.ToString() + ".STK002 AS STK ON STK002_EvrakSeriNo2=SevkDetay.sn AND  " +
                    "RIGHT(STK.STK002_EvrakSeriNo,LEN(bill_number))=bill_number  " +
                    "WHERE SevkDetay.checkok=0 AND SevkMst.checkok=0  AND SevkDetay.sku=STK002_MalKodu AND SevkDetay.onay=1 AND sevkdetay.sku IN  " +
                    "(SELECT STK002.STK002_MalKodu FROM YNS" + Sirket.ToString() + ".STK002 WHERE STK002.STK002_SipDurumu=0  " +
                    "AND STK.STK002_EvrakSeriNo2=SevkDetay.sn AND RIGHT(STK002_EvrakSeriNo,LEN(bill_number))=bill_number)  " +
                    "GROUP BY sevkdetay.sn,cus_code,sevkdetay.sku,bill_number,sevk_tar,cre_date, " +
                    "wh_code,sevkdetay.um_code,sevkdetay.um_qty,STK002_BirimFiyati,Sipler.tax_type,StokKarti.STK004_GumrukOrani,STK002_MalKodu) AS tbl  " +
                    "GROUP BY tbl.SiparisID,tbl.HesapKodu,tbl.MalKodu,tbl.EvrakNo,tbl.SevkTarih,tbl.EklenmeTarihi,tbl.Depo, " +
                    "tbl.Birim,tbl.Fiyat,tbl.Kdv,tbl.Gumruk";

            if (ConSevkislemi.State == ConnectionState.Closed)
                ConSevkislemi.Open();

            if (ConSevkislemi1.State == ConnectionState.Closed)
                ConSevkislemi1.Open();

            CmdSevkislemi = new SqlCommand(Sorgu, ConSevkislemi);
            CmdSevkislemi.CommandTimeout = 120;
            SevkislemiDr = CmdSevkislemi.ExecuteReader(CommandBehavior.CloseConnection);
            ds.Sevkislemi.TableName.Replace("YNS00033", "YNS" + Sirket.ToString());

            while (SevkislemiDr.Read())
            {
                Thread.Sleep(50);

                SiparisID = SevkislemiDr["SiparisID"].ToString();
                HesapKodu = SevkislemiDr["HesapKodu"].ToString();
                MalKodu = SevkislemiDr["MalKodu"].ToString();
                EvrakNo = SevkislemiDr["EvrakNo"].ToString();
                SevkTarihi = Convert.ToDateTime(SevkislemiDr["SevkTarih"].ToString());
                EklenmeTarihi = Convert.ToDateTime(SevkislemiDr["EklenmeTarihi"].ToString());
                DepoKodu = SevkislemiDr["Depo"].ToString();
                Birim = SevkislemiDr["Birim"].ToString();
                decimal Miktarsevk = Convert.ToDecimal(SevkislemiDr["Miktar"].ToString());
                Miktar = Convert.ToInt32(Miktarsevk);
                decimal StkFiyat = Convert.ToDecimal(SevkislemiDr["Fiyat"].ToString());
                decimal StkTotal = Convert.ToDecimal(SevkislemiDr["Toplam"].ToString());
                decimal StkKdv = Convert.ToDecimal(SevkislemiDr["Kdv"].ToString());
                decimal OtvTutari = Convert.ToDecimal(SevkislemiDr["OtvTutari"].ToString());

                ds.Sevkislemi.AddSevkislemiRow(Convert.ToInt32(SiparisID), HesapKodu, MalKodu, EvrakNo, SevkTarihi, EklenmeTarihi, DepoKodu, Birim, Miktar, StkFiyat, StkTotal, StkKdv, OtvTutari);

            }

            ConSevkislemi.Dispose();
            ConSevkislemi1.Dispose();
            ConSevkislemi.Close();
            ConSevkislemi1.Close();
            SevkislemiDr.Dispose();
            SevkislemiDr.Close();
            CmdSevkislemi.Dispose();

            MessageBox.Show("Arama İşlemi Tamamlanmıştır.\nToplam Bulunan Kayıt Sipariş Sayısı : " + Convert.ToString(ds.Sevkislemi.Rows.Count));

            btnArama.Enabled = true;
            btnAktar.Enabled = true;
        }

        #endregion

        #region Sevk Arama Butonu

        private void btnArama_Click(object sender, EventArgs e)
        {
            ds.Sevkislemi.Clear();

            Sevk = new Thread(new ThreadStart(SevkArama));
            Sevk.Priority = ThreadPriority.Highest;
            Sevk.Start();
        }

        #endregion

        #region Aktarım İşlemi

        private void Aktarim()
        {
            if (MessageBox.Show("Bulunan Sevk İşlemleri Kayıtlarını Linke Aktarmak İstediğinize Emin Misiniz?", "Uyarı...",
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                ConStk002Kontrol = new SqlConnection(Cs1);
                ConSCar = new SqlConnection(Cs1);
                ConSEQNo = new SqlConnection(Cs1);
                ConStk005 = new SqlConnection(Cs1);
                ConStk02Update = new SqlConnection(Cs1);
                ConStk004 = new SqlConnection(Cs1);
                ConMrpStk = new SqlConnection(Cs2);
                ConStkEkle = new SqlConnection(Cs1);
                ConStkUpdate = new SqlConnection(Cs1);
                ConLinkKontrol = new SqlConnection(Cs1);
                ConCariAktar = new SqlConnection(Cs1);
                ConCariKontrol = new SqlConnection(Cs2);
                ConUpdateMrpCariKart = new SqlConnection(Cs2);
                ConUpdateCariKart = new SqlConnection(Cs1);
                ConUpdateMrpStokKart = new SqlConnection(Cs2);
                ConUpdateSevkDet = new SqlConnection(Cs2);
                ConUpdateSevkMst = new SqlConnection(Cs2);

                #region Bağlantıları Açıyoruz

                if (ConUpdateSevkDet.State == ConnectionState.Closed)
                    ConUpdateSevkDet.Open();

                if (ConUpdateSevkMst.State == ConnectionState.Closed)
                    ConUpdateSevkMst.Open();

                if (ConUpdateMrpStokKart.State == ConnectionState.Closed)
                    ConUpdateMrpStokKart.Open();

                if (ConUpdateCariKart.State == ConnectionState.Closed)
                    ConUpdateCariKart.Open();

                if (ConUpdateMrpCariKart.State == ConnectionState.Closed)
                    ConUpdateMrpCariKart.Open();

                if (ConSCar.State == ConnectionState.Closed)
                    ConSCar.Open();

                if (ConStk002Kontrol.State == ConnectionState.Closed)
                    ConStk002Kontrol.Open();

                if (ConSEQNo.State == ConnectionState.Closed)
                    ConSEQNo.Open();

                if (ConStk005.State == ConnectionState.Closed)
                    ConStk005.Open();

                if (ConStk02Update.State == ConnectionState.Closed)
                    ConStk02Update.Open();

                if (ConStk004.State == ConnectionState.Closed)
                    ConStk004.Open();

                if (ConMrpStk.State == ConnectionState.Closed)
                    ConMrpStk.Open();

                if (ConStkEkle.State == ConnectionState.Closed)
                    ConStkEkle.Open();

                if (ConStkUpdate.State == ConnectionState.Closed)
                    ConStkUpdate.Open();

                if (ConLinkKontrol.State == ConnectionState.Closed)
                    ConLinkKontrol.Open();

                if (ConCariAktar.State == ConnectionState.Closed)
                    ConCariAktar.Open();

                if (ConCariKontrol.State == ConnectionState.Closed)
                    ConCariKontrol.Open();

                #endregion

                #region Datetime Now Linke Göre Ayarlıyoruz

                DateTime obj = new DateTime();
                obj = DateTime.Now;
                string str;
                Tarih = 0;
                Tarih = obj.ToOADate();
                str = Tarih.ToString().Substring(0, 5);
                TarihRakam = Convert.ToInt32(str);

                #endregion

                #region PC Adını Alıyoruz

                PCAdi = Environment.UserName.ToString();

                if (PCAdi.ToString().Length > 10)
                {
                    PCAdi = PCAdi.ToString().Substring(0, 10);
                }

                #endregion

                #region SEQNo yu Alıyoruz

                string SeqNoSorgu = "Select MAX(STK005_SEQNo) AS SEQNo From YNS" + Sirket.ToString() + ".STK005";
                cmdSEQNo = new SqlCommand(SeqNoSorgu, ConSEQNo);
                cmdSEQNo.CommandTimeout = 120;
                SEQNoDr = cmdSEQNo.ExecuteReader(CommandBehavior.CloseConnection);

                if (SEQNoDr.Read())
                {
                    string sekno = SEQNoDr["SEQNo"].ToString();
                    if (string.IsNullOrEmpty(sekno))
                    {
                        sekno = "0";
                    }
                    SEQNo = Convert.ToInt32(sekno);
                }

                SEQNoDr.Dispose();
                SEQNoDr.Close();

                #endregion

                #region CAR005 Sorgu

                string Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                      "VALUES (@Secenek,@FaturaTarihi,@FaturaNo,@BA,@CariIslemTipi,@SatirTipi,@SatirNo,@SatirKodu,@Filler,@SatirAciklama,@CHKodu,@Tutar,@Oran)";

                #endregion

                #region STK005 İnsert Sorgu

                string STK005insert2 = "INSERT INTO  YNS" + Sirket.ToString() + ".STK005(STK005_MalKodu, STK005_IslemTarihi, STK005_GC, STK005_CariHesapKodu, STK005_EvrakSeriNo, STK005_Miktari, STK005_BirimFiyati, " +
                      "STK005_Tutari, STK005_Iskonto, STK005_KDVTutari, STK005_IslemTipi, STK005_Kod1, STK005_Kod2, STK005_IrsaliyeNo, STK005_FaturaDurumu,  " +
                      "STK005_MuhasebelesmeDurumu, STK005_KDVDurumu, STK005_ParaBirimi, STK005_SEQNo, STK005_GirenKaynak, STK005_GirenTarih, STK005_GirenSaat,  " +
                      "STK005_GirenKodu, STK005_GirenSurum, STK005_DegistirenKaynak, STK005_DegistirenTarih, STK005_DegistirenSaat, STK005_DegistirenKodu,  " +
                      "STK005_DegistirenSurum, STK005_IptalDurumu, STK005_AsilEvrakTarihi, STK005_SevkTarihi, STK005_OTV, STK005_Miktar2, STK005_Tutar2,  " +
                      "STK005_KalemIskontoOrani1, STK005_KalemIskontoOrani2, STK005_KalemIskontoOrani3, STK005_KalemIskontoOrani4, STK005_KalemIskontoOrani5,  " +
                      "STK005_KalemIskontoTutari1, STK005_KalemIskontoTutari2, STK005_KalemIskontoTutari3, STK005_KalemIskontoTutari4, STK005_KalemIskontoTutari5,  " +
                      "STK005_DovizCinsi, STK005_DovizTutari, STK005_DovizKuru, STK005_Aciklama1, STK005_Aciklama2, STK005_Depo, STK005_Kod3, STK005_Kod4, STK005_Kod5,  " +
                      "STK005_Kod6, STK005_Kod7, STK005_Kod8, STK005_Kod9, STK005_Kod10, STK005_Kod11, STK005_Kod12, STK005_Vasita, STK005_MalSeriNo,  " +
                      "STK005_EvrakSeriNo2, STK005_SiparisNo, STK005_VadeTarihi, STK005_IrsaliyeFaturaTarihi, STK005_SiparisTarihi, STK005_IadeFaturaNo, STK005_Masraf,  " +
                      "STK005_MaliyetTutari, STK005_MaliyetMuhasebelesmeSekli, STK005_MaliyetMuhasebelesmeDurumu, STK005_MasrafMerkezi, STK005_MuhasebeFisKodu,  " +
                      "STK005_MuhasebeHesapNo, STK005_MuhasebeKarsiHesapNo, STK005_Kod9Flag, STK005_Kod10Flag, STK005_StokTrFinansmanGider, STK005_StokTrVadeFarki,  " +
                      "STK005_StokTrSozFaizOrani, STK005_StokTrStokDuzHesapKodu, STK005_StokTrSmmDuzHesapKodu, STK005_StokTrNonReelFinansGidSpk,  " +
                      "STK005_StokTrNonReelFinansGidMly, STK005_StokTrDuzKatsayiSpk, STK005_StokTrDuzKatsayiMly, STK005_StokTrDuzTutarSpk, STK005_StokTrDuzTutarMly,  " +
                      "STK005_StokTrDuzSatSpk, STK005_StokTrDuzSatMly, STK005_StokTrSatMaliyeti, STK005_StokTrKrediTutari, STK005_StokTrIlgiliEvrak, STK005_KDVOranFlag,  " +
                      "STK005_EvrakTipi, STK005_TeslimCHKodu, STK005_KarsiMuhasebeKodu, STK005_ExtFldTutar1, STK005_FaturalasanMiktar) " +
                      "VALUES (@STK005_MalKodu,@STK005_IslemTarihi, 1,@STK005_CariHesapKodu,@STK005_EvrakSeriNo,@STK005_Miktari,@STK005_BirimFiyati,@STK005_Tutari, 0.00, " +
                      "@STK005_KdvTutari, 2, N'', N'', N'', @STK005_FaturaDurumu, 0, 0, 1,@STK005_SEQNo, N'Y6019',@STK005_GirenTarih, N'100401',@STK005_GirenKodu,N'5.1.10', N'Y6019',@STK005_DegistirenTarih, " +
                      "N'100401',@STK005_DegistirenKodu,N'5.1.10', 1,@STK005_AsilEvrakTarihi,@STK005_SevkTarihi, 0.00, 0.00, 0.000, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, " +
                      "N'', 0.00, 0.000000, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.00, 0.00, N'', N'', @STK005_EvrakSeriNo2, @STK005_SiparisNo,@STK005_VadeTarihi,@STK005_IrsaliyeFaturaTarihi, @STK005_SiparisTarihi, N'', 0.00, 0.00, 1, " +
                      "0, N'', N'', N'', N'', 0, 0, 0.00, 0.00, 0.00, N'', N'', 0.00, 0.00, 0.00000, 0.00000, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, N'', 4, @STK005_EvrakTipi,@STK005_TeslimCHKodu, N'', 0.00, 0.0000)";

                #endregion

                #region STK004 Insert Sorgu

                string STK004inset = "INSERT INTO YNS" + Sirket.ToString() + ".STK004(STK004_MalKodu, STK004_Aciklama, STK004_GrupKodu, STK004_Iskonto, STK004_KDV, STK004_Birim1, STK004_Birim2, STK004_Birim3, " +
                      "STK004_BorcluHesap, STK004_AlacakliHesap, STK004_AlisFiyati1, STK004_AlisKDV1, STK004_AlisBirim1, STK004_AlisFiyati2, STK004_AlisKDV2, " +
                      "STK004_AlisBirim2, STK004_AlisFiyati3, STK004_AlisKDV3, STK004_AlisBirim3, STK004_SatisFiyati1, STK004_SatisKDV1, STK004_SatisBirim1, " +
                      "STK004_SatisFiyati2, STK004_SatisKDV2, STK004_SatisBirim2, STK004_SatisFiyati3, STK004_SatisKDV3, STK004_SatisBirim3, STK004_DevirMiktari, " +
                      "STK004_DevirTutari, STK004_GirisMiktari, STK004_GirisTutari, STK004_GirisIskonto, STK004_CikisMiktari, STK004_CikisTutari, STK004_CikisIskonto, " +
                      "STK004_KritikSeviye, STK004_DevirTarihi, STK004_KafileBuyuklugu, STK004_GirenKaynak, STK004_GirenTarih, STK004_GirenSaat, STK004_GirenKodu, " +
                      "STK004_GirenSurum, STK004_DegistirenKaynak, STK004_DegistirenTarih, STK004_DegistirenSaat, STK004_DegistirenKodu, STK004_DegistirenSurum, " +
                      "STK004_OzelKodu, STK004_TipKodu, STK004_Kod4, STK004_Kod5, STK004_Kod6, STK004_Kod7, STK004_Kod8, STK004_Kod9, STK004_Kod10, STK004_Kod11, " +
                      "STK004_Kod12, STK004_UreticiKodu1, STK004_UreticiKodu2, STK004_UreticiKodu3, STK004_MusterekMalKodu, STK004_MaliyetSekli, STK004_FireOrani, " +
                      "STK004_TeminYeri, STK004_TeminSuresi, STK004_Mensei, STK004_GTIPN, STK004_GumrukOrani, STK004_Fon, STK004_DovizAlis1, STK004_DovizAlisCinsi1, " +
                      "STK004_DovizAlis2, STK004_DovizAlisCinsi2, STK004_DovizAlis3, STK004_DovizAlisCinsi3, STK004_DovizSatis1, STK004_DovizSatisCinsi1, STK004_DovizSatis2, " +
                      "STK004_DovizSatisCinsi2, STK004_DovizSatis3, STK004_DovizSatisCinsi3, STK004_AlisSiparisi, STK004_SatisSiparisi, STK004_AzamiSeviye, " +
                      "STK004_SonGirisTarihi, STK004_SonCikisTarihi, STK004_BirimAgirligi, STK004_ValorGun, STK004_Barkod1, STK004_Barkod2, STK004_Barkod3, STK004_Aciklama2, " +
                      "STK004_Aciklama3, STK004_UygMaliyetTipi, STK004_SonMaliyetBirimFiyati, STK004_SonMaliyetTarihi, STK004_SonMaliyetTipi, STK004_SonMaliyetParaBirimi, " +
                      "STK004_Rezervasyon, STK004_AlimdanIade, STK004_SatistanIade, STK004_MalKodu2, STK004_DovizCinsi, STK004_SonAlimFaturasiTarihi, " +
                      "STK004_SonAlimFaturasiNo, STK004_SonAlimFaturasiBirimFiyati, STK004_SonAlimFaturasiCariHesapKodu, STK004_SonAlimFaturasiDovizBirimFiyati, " +
                      "STK004_SonSatisFaturasiTarihi, STK004_SonSatisFaturasiNo, STK004_SonSatisFaturasiBirimFiyati, STK004_SonSatisFaturasiCariHesapKodu, " +
                      "STK004_SonSatisFaturasiDovizBirimFiyati, STK004_MasrafMerkezi, STK004_ResimDosyasi, STK004_FiyatListesindeCikart, STK004_SatisFiyati1ValorGun, " +
                      "STK004_SatisFiyati2ValorGun, STK004_SatisFiyati3ValorGun, STK004_DovizSatisFiyati1ValorGun, STK004_DovizSatisFiyati2ValorGun, " +
                      "STK004_DovizSatisFiyati3ValorGun, STK004_DovizDevirTutari, STK004_DovizGirisTutari, STK004_DovizGirisIskontoTutari, STK004_DovizCikisTutari, " +
                      "STK004_DovizCikisIskontoTutari, STK004_BarkodBirim1, STK004_BarkodBirim2, STK004_BarkodBirim3, STK004_BarkodCarpan1, STK004_BarkodCarpan2, " +
                      "STK004_BarkodCarpan3, STK004_DevirMiktari2, STK004_GirisMiktari2, STK004_GirisTutari2, STK004_CikisMiktari2, STK004_CikisTutari2, STK004_DepoKodu1, " +
                      "STK004_DepoDevirMiktari21, STK004_DepoDevirTutari21, STK004_DepoGirisMiktari1, STK004_DepoGirisTutari1, STK004_DepoGirisIskonto1, " +
                      "STK004_DepoCikisMiktari1, STK004_DepoCikisTutari1, STK004_DepoCikisIskonto1, STK004_DepoSonMaliyetBf1, STK004_DepoSonDevirTarihi1, " +
                      "STK004_DepoSonGirisTarihi1, STK004_DepoSonCikisTarihi1, STK004_DepoKodu2, STK004_DepoDevirMiktari22, STK004_DepoDevirTutari22, " +
                      "STK004_DepoGirisMiktari2, STK004_DepoGirisTutari2, STK004_DepoGirisIskonto2, STK004_DepoCikisMiktari2, STK004_DepoCikisTutari2, " +
                      "STK004_DepoCikisIskonto2, STK004_DepoSonMaliyetBf2, STK004_DepoKodu3, STK004_DepoDevirMiktari23, STK004_DepoDevirTutari23, " +
                      "STK004_DepoGirisMiktari3, STK004_DepoGirisTutari3, STK004_DepoGirisIskonto3, STK004_DepoCikisMiktari3, STK004_DepoCikisTutari3, " +
                      "STK004_DepoCikisIskonto3, STK004_DepoSonMaliyetBf3, STK004_DepoKodu4, STK004_DepoDevirMiktari24, STK004_DepoDevirTutari24, " +
                      "STK004_DepoGirisMiktari4, STK004_DepoGirisTutari4, STK004_DepoGirisIskonto4, STK004_DepoCikisMiktari4, STK004_DepoCikisTutari4, " +
                      "STK004_DepoCikisIskonto4, STK004_DepoSonMaliyetBf4, STK004_DepoKodu5, STK004_DepoDevirMiktari25, STK004_DepoDevirTutari25, " +
                      "STK004_DepoGirisMiktari5, STK004_DepoGirisTutari5, STK004_DepoGirisIskonto5, STK004_DepoCikisMiktari5, STK004_DepoCikisTutari5, " +
                      "STK004_DepoCikisIskonto5, STK004_DepoSonMaliyetBf5, STK004_DepoKodu6, STK004_DepoDevirMiktari26, STK004_DepoDevirTutari26, " +
                      "STK004_DepoGirisMiktari6, STK004_DepoGirisTutari6, STK004_DepoGirisIskonto6, STK004_DepoCikisMiktari6, STK004_DepoCikisTutari6, " +
                      "STK004_DepoCikisIskonto6, STK004_DepoSonMaliyetBf6, STK004_DepoKodu7, STK004_DepoDevirMiktari27, STK004_DepoDevirTutari27, " +
                      "STK004_DepoGirisMiktari7, STK004_DepoGirisTutari7, STK004_DepoGirisIskonto7, STK004_DepoCikisMiktari7, STK004_DepoCikisTutari7, " +
                      "STK004_DepoCikisIskonto7, STK004_DepoSonMaliyetBf7, STK004_DepoKodu8, STK004_DepoDevirMiktari28, STK004_DepoDevirTutari28, " +
                      "STK004_DepoGirisMiktari8, STK004_DepoGirisTutari8, STK004_DepoGirisIskonto8, STK004_DepoCikisMiktari8, STK004_DepoCikisTutari8, " +
                      "STK004_DepoCikisIskonto8, STK004_DepoSonMaliyetBf8, STK004_DepoKodu9, STK004_DepoDevirMiktari29, STK004_DepoDevirTutari29, " +
                      "STK004_DepoGirisMiktari9, STK004_DepoGirisTutari9, STK004_DepoGirisIskonto9, STK004_DepoCikisMiktari9, STK004_DepoCikisTutari9, " +
                      "STK004_DepoCikisIskonto9, STK004_DepoSonMaliyetBf9, STK004_DepoKodu10, STK004_DepoDevirMiktari210, STK004_DepoDevirTutari210, " +
                      "STK004_DepoGirisMiktari10, STK004_DepoGirisTutari10, STK004_DepoGirisIskonto10, STK004_DepoCikisMiktari10, STK004_DepoCikisTutari10, " +
                      "STK004_DepoCikisIskonto10, STK004_DepoSonMaliyetBf10, STK004_StokMiktar2Br, STK004_StokMCH1, STK004_MOP1, STK004_StokMCH2, STK004_MOP2, " +
                      "STK004_StokMCH3, STK004_MOP3, STK004_StokMCH4, STK004_MOP4, STK004_MOP5, STK004_StokMSOP, STK004_StokDevirTutar2, STK004_Not1, STK004_Not2, " +
                      "STK004_Not3, STK004_Not4, STK004_Not5, STK004_Not6, STK004_Not7, STK004_StokDBMiktari, STK004_StokDBTutari, STK004_StokDBDuzTutari, " +
                      "STK004_StokDBDuzDate, STK004_StokDBDuzYil, STK004_StokDBDuzDonem, STK004_StokDBRofm, STK004_StokDBEntHesapKodu, STK004_YASatisFiati1, " +
                      "STK004_YSatisKDV1, STK004_YSatisBirim1, STK004_YASatisFiati2, STK004_YSatisKDV2, STK004_YSatisBirim2, STK004_YASatisFiati3, STK004_YSatisKDV3, " +
                      "STK004_YSatisBirim3, STK004_YSatisFiyati1ValorGun, STK004_YSatisFiyati2ValorGun, STK004_YSatisFiyati3ValorGun, STK004_AktifFlag, STK004_SayimTarihi, " +
                      "STK004_SayimMiktari, STK004_Dokuman1, STK004_Dokuman2, STK004_Dokuman3, STK004_SMMHesapKodu) " +
                      "VALUES (@STK004_MalKodu,@STK004_Aciklama, N'', 0.000, 4, N'', N'', N'', N'', N'', 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, " +
                      "0, 0.0000, 0.00,0.0000,0.00, 0.00, @STK004_CikisMiktari, @STK004_CikisTutari, 0.00, 0.0000, 0, 0.0000, N'Y4004',@STK004_GirenTarih, " +
                      "N'100323',@STK004_GirenKodu, N'5.1.10', N'Y6019',@STK004_DegistirenTarih, N'100401',@STK004_DegistirenKodu, N'5.1.10', N'', N'', N'', N'', N'', N'', N'', N'', N'', " +
                      "0.000, 0.000, N'', N'', N'', N'', 1, 0.00000, N'', 0, N'', N'', 0.00, 0.00, 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.0000, " +
                      "0.0000, 0.0000,0, @STK004_SonCikisTarihi, 0.0000, 0, N'', N'', N'', N'', N'', 0, 0.000000, 0, 0, 1, 0.0000, N'', N'', N'', N'', 0, " +
                      "N'', 0.000000, N'', 0.000000, 0, N'', " +
                      "0.000000, N'', 0.000000, N'', N'', 0, 0, 0, 0, 0, 0, 0, 0.00, 0.00, 0.00, 0.00, 0.00, N'', N'', N'', 0.00, 0.00, 0.00, 0.0000, 0.0000, 0.000, 0.0000, 0.000, N'', 0.0000, 0.00, " +
                      "0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, 0, 0, 0, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                      "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, " +
                      "0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                      "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0, N'', 0, " +
                      "N'', 0, N'', 0, N'', N'', 0, 0.000, N'', N'', N'', N'', N'', N'', N'', 0.0000, 0.00, 0.00, 0, 0, 0, 0.00, N'', 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0, 0, 0, 1, 0, 0.0000, N'', " +
                      "N'', N'', N'')";

                #endregion

                #region Aktarımı Başlatıyoruz

                MessageBox.Show("Aktarım İşlemi Başlamıştır.Lütfen İşlem Bitmeden Programı Kapatmayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string OtvDurumEvrak1 = "";
                string OtvDurumEvrak2 = "";
                string OtvMalKodu = "";
                decimal OtvliOran = 0;
                decimal BuldugumuzOtvOrani = 0;
                string[] OtvDurumEvrak3 = new string[siralamaMak];
                string[] BuldugumuzOtvOrani1 = new string[siralamaMak];
                string[] OtvliMalKodu = new string[siralamaMak];
                int[] ForRowSayilari = new int[siralamaMak];
                bool OtvDurumu = false;
                int DonguSayisi = 0;

                for (int i = 0; i <= ds.Sevkislemi.Rows.Count - 1; i++)
                {
                    OtvDurumEvrak1 = ds.Sevkislemi.Rows[i]["EvrakNo"].ToString();

                    for (int a = 0; a <= ds.Sevkislemi.Rows.Count - 1; a++)
                    {
                        OtvDurumEvrak2 = ds.Sevkislemi.Rows[a]["EvrakNo"].ToString();
                        OtvMalKodu = ds.Sevkislemi.Rows[a]["MalKodu"].ToString();

                        #region Ötv Oranımızı Buluyoruz ve Oranımızı Diziye Atıyoruz

                        if (OtvDurumEvrak1 == OtvDurumEvrak2)
                        {
                            BuldugumuzOtvOrani = Convert.ToDecimal(ds.Sevkislemi.Rows[a]["OtvTutari"].ToString());

                            siralamaMak2 = siralamaMak;
                            siralamaMak2 = siralamaMak - 1;

                            OtvDurumEvrak3[siralamaMak2] = OtvDurumEvrak2;
                            BuldugumuzOtvOrani1[siralamaMak2] = Convert.ToString(BuldugumuzOtvOrani).Replace(".", ",");
                            OtvliMalKodu[siralamaMak2] = OtvMalKodu;
                            ForRowSayilari[siralamaMak2] = a;

                            siralamaMak++;
                            siralamaMak2 = siralamaMak;

                            Array.Resize(ref OtvDurumEvrak3, siralamaMak2);
                            Array.Resize(ref BuldugumuzOtvOrani1, siralamaMak2);
                            Array.Resize(ref OtvliMalKodu, siralamaMak2);
                            Array.Resize(ref ForRowSayilari, siralamaMak2);

                            i = a;
                            DonguSayisi = a;
                        }

                        #endregion
                    }

                    #region Dizilerden Null Değerleri Siliyoruz

                    for (int b = 0; b < OtvDurumEvrak3.Length; b++)
                    {
                        string kontrolevrk1 = OtvDurumEvrak3[b];

                        if (kontrolevrk1 == null)
                        {
                            Array.Resize(ref OtvDurumEvrak3, OtvDurumEvrak3.Length - 1);
                            Array.Resize(ref BuldugumuzOtvOrani1, BuldugumuzOtvOrani1.Length - 1);
                            Array.Resize(ref OtvliMalKodu, OtvliMalKodu.Length - 1);
                            Array.Resize(ref ForRowSayilari, ForRowSayilari.Length - 1);
                        }
                    }

                    #endregion

                    #region Sevkimiz Ötvli Irsaliye mi Yoksa Normal mi Belirliyoruz

                    decimal OtvTopluyoruz = 0;
                    int rowsatir = 0;

                    for (int c = 0; c <= OtvDurumEvrak3.Length - 1; c++)
                    {
                        rowsatir = ForRowSayilari[c];

                        OtvTopluyoruz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["OtvTutari"].ToString()) + OtvTopluyoruz;
                    }

                    rowsatir = 0;

                    if (OtvTopluyoruz > 0)
                    {
                        OtvDurumu = true;
                    }
                    else
                    {
                        OtvDurumu = false;
                    }

                    #endregion

                    for (int d = 0; d <= OtvDurumEvrak3.Length - 1; d++)
                    {
                        if (OtvDurumu == true)
                        {
                            rowsatir = ForRowSayilari[d];

                            #region Değişkenler

                            string MalKodumuz = ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString();
                            string CariHesapKodumuz = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();
                            HesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            string EvrakNumaramiz = ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString().TrimStart();
                            string EvrakNo2 = EvrakNumaramiz.ToString();

                            if (EvrakNumaramiz.ToString().Length == 6)
                            {
                                EvrakNumaramiz = "  " + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvrakNumaramiz.ToString().Length == 5)
                            {
                                EvrakNumaramiz = "  0" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvrakNumaramiz.ToString().Length == 4)
                            {
                                EvrakNumaramiz = "  00" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }

                            decimal Stk004Total = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Toplam"].ToString());
                            decimal Stk005Kdv = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Kdv"].ToString());
                            string Stk004BirimMiktar = ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString();

                            decimal Miktarimiz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString());
                            decimal BirimFiyatimiz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Fiyat"].ToString());
                            decimal Tutarimiz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Toplam"].ToString());
                            decimal Kdvmiz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Kdv"].ToString());
                            decimal Otvmiz = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["OtvTutari"].ToString());
                            string SiparisIDmiz = ds.Sevkislemi.Rows[rowsatir]["SiparisID"].ToString();

                            #endregion

                            #region Stk005 Ötvli Kayıt Atıyoruz

                            string STK005insert = "INSERT INTO  YNS" + Sirket.ToString() + ".STK005(STK005_MalKodu, STK005_IslemTarihi, STK005_GC, STK005_CariHesapKodu, STK005_EvrakSeriNo, STK005_Miktari, STK005_BirimFiyati, "+
                      "STK005_Tutari, STK005_Iskonto, STK005_KDVTutari, STK005_IslemTipi, STK005_Kod1, STK005_Kod2, STK005_IrsaliyeNo, STK005_FaturaDurumu,  "+
                      "STK005_MuhasebelesmeDurumu, STK005_KDVDurumu, STK005_ParaBirimi, STK005_SEQNo, STK005_GirenKaynak, STK005_GirenTarih, STK005_GirenSaat,  "+
                      "STK005_GirenKodu, STK005_GirenSurum, STK005_DegistirenKaynak, STK005_DegistirenTarih, STK005_DegistirenSaat, STK005_DegistirenKodu,  "+
                      "STK005_DegistirenSurum, STK005_IptalDurumu, STK005_AsilEvrakTarihi, STK005_SevkTarihi, STK005_OTVDahilHaric, STK005_OTV, STK005_Miktar2,  "+
                      "STK005_Tutar2, STK005_KalemIskontoOrani1, STK005_KalemIskontoOrani2, STK005_KalemIskontoOrani3, STK005_KalemIskontoOrani4,  "+
                      "STK005_KalemIskontoOrani5, STK005_KalemIskontoTutari1, STK005_KalemIskontoTutari2, STK005_KalemIskontoTutari3, STK005_KalemIskontoTutari4,  "+
                      "STK005_KalemIskontoTutari5, STK005_DovizCinsi, STK005_DovizTutari, STK005_DovizKuru, STK005_Aciklama1, STK005_Aciklama2, STK005_Depo, STK005_Kod3,  "+
                      "STK005_Kod4, STK005_Kod5, STK005_Kod6, STK005_Kod7, STK005_Kod8, STK005_Kod9, STK005_Kod10, STK005_Kod11, STK005_Kod12, STK005_Vasita,  "+
                      "STK005_MalSeriNo, STK005_EvrakSeriNo2, STK005_SiparisNo, STK005_VadeTarihi, STK005_IrsaliyeFaturaTarihi, STK005_SiparisTarihi, STK005_IadeFaturaNo,  "+
                      "STK005_Masraf, STK005_MaliyetTutari, STK005_MaliyetMuhasebelesmeSekli, STK005_MaliyetMuhasebelesmeDurumu, STK005_MasrafMerkezi,  "+
                      "STK005_MuhasebeFisKodu, STK005_MuhasebeHesapNo, STK005_MuhasebeKarsiHesapNo, STK005_Kod9Flag, STK005_Kod10Flag, STK005_StokTrFinansmanGider, "+
                      "STK005_StokTrVadeFarki, STK005_StokTrSozFaizOrani, STK005_StokTrStokDuzHesapKodu, STK005_StokTrSmmDuzHesapKodu,  "+
                      "STK005_StokTrNonReelFinansGidSpk, STK005_StokTrNonReelFinansGidMly, STK005_StokTrDuzKatsayiSpk, STK005_StokTrDuzKatsayiMly,  "+
                      "STK005_StokTrDuzTutarSpk, STK005_StokTrDuzTutarMly, STK005_StokTrDuzSatSpk, STK005_StokTrDuzSatMly, STK005_StokTrSatMaliyeti,  "+
                      "STK005_StokTrKrediTutari, STK005_StokTrIlgiliEvrak, STK005_KDVOranFlag, STK005_EvrakTipi, STK005_TeslimCHKodu, STK005_KarsiMuhasebeKodu,  "+
                      "STK005_ExtFldTutar1, STK005_FaturalasanMiktar) " +
                      "VALUES     ('" + MalKodumuz.ToString() + "'," + TarihRakam + ", 1,'" + CariHesapKodumuz.ToString() + "','" + EvrakNumaramiz.ToString() + "'," + Miktarimiz.ToString().Replace(",", ".") + ",'" + BirimFiyatimiz.ToString().Replace(",", ".") + "','" + Tutarimiz.ToString().Replace(",", ".") + "', '0.00','" + Kdvmiz.ToString().Replace(",", ".") + "', 2, N'', N'', N'', 0, 0, 0, 1, 1,  " +
                      "N'Y6019'," + TarihRakam + ", N'100401','" + PCAdi.ToString() + "', N'5.1.10', N'Y6019'," + TarihRakam + ", N'100401','" + PCAdi.ToString() + "', N'5.1.10', 1," + TarihRakam + ", 0, 0,'" + Otvmiz.ToString().Replace(",", ".") + "', '0.000', '0.000', '0.00',  " +
                      "'0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', '0.00', N'', '0.00', '0.000000', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', '0.00', '0.00', N'',  "+
                      "N'','" + EvrakNo2.ToString() + "','" + SiparisIDmiz.ToString() + "'," + TarihRakam + "," + TarihRakam + ", 0, N'', '0.00', '0.00', 1, 0, N'', N'', N'', N'', 0, 0, '0.00', '0.00', '0.00', N'', N'', '0.00', '0.00', '0.00000', '0.00000',  " +
                      "'0.00', '0.00', '0.00', '0.00', '0.00', '0.00', N'', 4, 13,'" + CariHesapKodumuz.ToString() + "', N'', '0.00', '0.0000')";

                            CmdStk005 = new SqlCommand(STK005insert, ConStk005);
                            CmdStk005.CommandTimeout = 120;
                            CmdStk005.ExecuteNonQuery();

                            #endregion

                            #region Cari Kontrol Ediyoruz

                            string CariKontrolSorgu = "Select cus_num AS CariID, " +
                                        "(CASE  SUBSTRING(cus_code, 1, 1) " +
                                        "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                                        "ELSE cus_code END) AS HesapKodu, " +
                                        "(Select CASE cusven " +
                                        "WHEN 'V' THEN 'SATICI' " +
                                        "WHEN 'C' THEN 'MÜŞTERİ' " +
                                        "END) AS Durum,cus_name AS HesapAdi, " +
                                        "(Select CASE sprice_type " +
                                        "WHEN 'STANDART' THEN 'EUR' " +
                                        "END) AS Kur,adress1 AS Adres, " +
                                        "(Select CASE country_code " +
                                        "WHEN 'TR' THEN '52' " +
                                        "WHEN 'CY' THEN '601' " +
                                        "END) AS UlkeKodu, " +
                                        "(Select CASE isnumeric(city) " +
                                        "WHEN 0 THEN 0 " +
                                        "WHEN 1 THEN city " +
                                        "END) AS BolgeKodu,tax_office AS Vergi_Dairesi,tax_number AS VergiNo,tel1 AS Telefon From " + MrpSirket.ToString() + ".dbo.ab_cus_def AS Musteri " +
                                        "Where (CASE  SUBSTRING(cus_code, 1, 1) " +
                                        "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                                        "ELSE cus_code END)='" + HesapKodu.ToString() + "'";

                            CmdCariKontrol = new SqlCommand(CariKontrolSorgu, ConCariKontrol);
                            CmdCariKontrol.CommandTimeout = 120;
                            CariKontrolDr = CmdCariKontrol.ExecuteReader();

                            if (CariKontrolDr.Read())
                            {
                                MrpCariID = CariKontrolDr["CariID"].ToString();
                                CariKontrolDurum = CariKontrolDr["HesapKodu"].ToString();
                                CariHesapKodu = CariKontrolDr["HesapKodu"].ToString();
                                Durum = CariKontrolDr["Durum"].ToString();
                                CariHesapAdi = CariKontrolDr["HesapAdi"].ToString();
                                Kur = CariKontrolDr["Kur"].ToString();
                                Adres = CariKontrolDr["Adres"].ToString();
                                UlkeKodu2 = CariKontrolDr["UlkeKodu"].ToString();
                                BolgeKodu = Convert.ToInt32(CariKontrolDr["BolgeKodu"].ToString());
                                VergiDairesi = CariKontrolDr["Vergi_Dairesi"].ToString();
                                VergiNo = CariKontrolDr["VergiNo"].ToString();
                                Telefon = CariKontrolDr["Telefon"].ToString();
                            }

                            if (!string.IsNullOrEmpty(UlkeKodu2))
                            {
                                UlkeKodu = Convert.ToInt32(UlkeKodu2);
                            }
                            else
                            {
                                UlkeKodu = 52;
                            }

                            CariKontrolDr.Dispose();
                            CariKontrolDr.Close();

                            if (!string.IsNullOrEmpty(Adres))
                            {
                                if (Adres.ToString().Length > 40)
                                {
                                    Adres = Adres.ToString().Substring(0, 40);
                                }
                            }

                            if (!string.IsNullOrEmpty(VergiDairesi))
                            {
                                if (VergiDairesi.ToString().Length > 32)
                                {
                                    VergiDairesi = VergiDairesi.ToString().Substring(0, 32);
                                }
                            }

                            if (!string.IsNullOrEmpty(CariHesapAdi))
                            {
                                if (CariHesapAdi.ToString().Length > 40)
                                {
                                    CariHesapAdi = CariHesapAdi.ToString().Substring(0, 40);
                                }
                            }

                            #endregion

                            #region Cari Kartı Kontrol Edip Aktarıyoruz

                            //string LinkKontrolSorgu = "Select Count(CAR002_HesapKodu) Durum From YNS" + Sirket.ToString() + ".CAR002 " +
                            //        "Where CAR002_HesapKodu='" + AktarimHesapKodu.ToString() + "'";
                            string LinkKontrolSorgu = "SELECT CAR002_Row_ID AS LinkCariID, Count(CAR002_HesapKodu) Durum FROM YNS" + Sirket.ToString() + ".CAR002 " +
                                             "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_cus_def ON checkok = 0 AND " +
                                             "CASE cusven  " +
                                             "WHEN 'V' THEN SUBSTRING(REPLACE(cus_code,'M',''),1,LEN(cus_code)) " +
                                             "WHEN 'C' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1)  " +
                                             "END = SUBSTRING(CAR002_HesapKodu,1,LEN(CAR002_HesapKodu)) " +
                                             "WHERE CAR002_HesapKodu='" + HesapKodu.ToString() + "' " +
                                             " GROUP BY CAR002_Row_ID,CAR002_HesapKodu";

                            CmdLinkKontrol = new SqlCommand(LinkKontrolSorgu, ConLinkKontrol);
                            CmdLinkKontrol.CommandTimeout = 120;
                            LinkKontrolDr = CmdLinkKontrol.ExecuteReader();

                            if (LinkKontrolDr.Read())
                            {
                                LinkCariDurumu = Convert.ToInt32(LinkKontrolDr["Durum"].ToString());
                                LinkCariID = LinkKontrolDr["LinkCariID"].ToString();
                            }

                            CmdLinkKontrol.Dispose();
                            LinkKontrolDr.Dispose();
                            LinkKontrolDr.Close();

                            if (LinkCariDurumu == 0)
                            {
                                string CariKartAktarım = "INSERT INTO YNS" + Sirket.ToString() + ".CAR002(CAR002_HesapKodu, CAR002_Unvan1, CAR002_Unvan2, CAR002_Adres1, CAR002_Adres2, CAR002_Adres3, CAR002_VergiDairesi, " +
                                "CAR002_VergiHesapNo, CAR002_Telefon1, CAR002_BolgeKodu, CAR002_GrupKodu, CAR002_MuhasebeKodu, CAR002_IskontoOrani, CAR002_OpsiyonGunu,  " +
                                "CAR002_DevirTarihi, CAR002_DevirBorc, CAR002_DevirAlacak, CAR002_DigerBorc, CAR002_DigerAlacak, CAR002_OdemeBorc, CAR002_OdemeAlacak,  " +
                                "CAR002_CiroBorc, CAR002_CiroAlacak, CAR002_IadeBorc, CAR002_IadeAlacak, CAR002_KDVBorc, CAR002_KDVAlacak, CAR002_otvborc, CAR002_otvalacak,  " +
                                "CAR002_GirenKaynak, CAR002_GirenTarih, CAR002_GirenSaat, CAR002_GirenKodu, CAR002_GirenSurum, CAR002_DegistirenKaynak, CAR002_DegistirenTarih,  " +
                                "CAR002_DegistirenSaat, CAR002_DegistirenKodu, CAR002_DegistirenSurum, CAR002_Telefon2, CAR002_Telefon3, CAR002_Telefon4, CAR002_Fax,  " +
                                "CAR002_EMailAdresi, CAR002_InternetAdresi, CAR002_OzelKodu, CAR002_TipKodu, CAR002_Kod1, CAR002_Kod2, CAR002_Kod3, CAR002_Kod4, CAR002_Kod5,  " +
                                "CAR002_Kod6, CAR002_Kod7, CAR002_Kod8, CAR002_Kod9, CAR002_UygulanacakFiyat, CAR002_UygulanacakBankaKodu, CAR002_UygulanacakFiyatTipi,  " +
                                "CAR002_UygulananFiyat, CAR002_UygulananBankaKodu, CAR002_UygulananFiyatTipi, CAR002_Yetkili1, CAR002_Yetkili1Gorevi, CAR002_Yetkili1DahiliNo,  " +
                                "CAR002_Yetkili1EMail, CAR002_Yetkili2, CAR002_Yetkili2Gorevi, CAR002_Yetkili2DahiliNo, CAR002_Yetkili2EMail, CAR002_BankaHesapKodu, CAR002_BankaAdi,  " +
                                "CAR002_BankaSubeKodu, CAR002_BankaHesapNo, CAR002_KrediKartNo, CAR002_OdemeGunu, CAR002_ParaBirimi, CAR002_MutabakatTarihi,  " +
                                "CAR002_MutabakatBakiyesi, CAR002_TeslimYeri1, CAR002_TeslimYeri2, CAR002_TeslimAdresi1, CAR002_TeslimAdresi2, CAR002_KrediLimiti, " +
                                "CAR002_DevirSenetRiskiBorc, CAR002_DevirSenetRiskiAlacak, CAR002_DevirCekRiskiBorc, CAR002_DevirCekRiskiAlacak, CAR002_DevirTeminat1Borc,  " +
                                "CAR002_DevirTeminat1Alacak, CAR002_DevirTeminat2Borc, CAR002_DevirTeminat2Alacak, CAR002_DevirProtestoSenet, CAR002_DevirKarsiliksizCek,  " +
                                "CAR002_SenetRiskiBorc, CAR002_SenetRiskiAlacak, CAR002_CekRiskiBorc, CAR002_CekRiskiAlacak, CAR002_Teminat1Borc, CAR002_Teminat1Alacak,  " +
                                "CAR002_Teminat2Borc, CAR002_Teminat2Alacak, CAR002_ProtestoSenet, CAR002_KarsiliksizCek, CAR002_SonBorcTarihi, CAR002_SonAlacakTarihi,  " +
                                "CAR002_SonRiskBorcTarihi, CAR002_SonRiskAlacakTarihi, CAR002_Notlar1, CAR002_Notlar2, CAR002_Notlar3, CAR002_Notlar4, CAR002_Notlar5, CAR002_Notlar6,  " +
                                "CAR002_Notlar7, CAR002_MasrafMerkezi, CAR002_Sifre, CAR002_Resim, CAR002_YaslandirmaBakiye, CAR002_YaslandirmaTarihi, CAR002_YaslandirmaGunu,  " +
                                "CAR002_OdemeTarihi, CAR002_DovizMutabakatBakiyesi, CAR002_DovizDevirBorc, CAR002_DovizDevirAlacak, CAR002_DovizDigerBorc, CAR002_DovizDigerAlacak,  " +
                                "CAR002_DovizOdemeBorc, CAR002_DovizOdemeAlacak, CAR002_DovizCiroBorc, CAR002_DovizCiroAlacak, CAR002_DovizIadeBorc, CAR002_DovizIadeAlacak,  " +
                                "CAR002_DovizKDVBorc, CAR002_DovizKDVAlacak, CAR002_Dovizotvborc, CAR002_Dovizotvalacak, CAR002_DovizKrediLimiti, CAR002_DovizDevirSenetRiskiBorc,  " +
                                "CAR002_DovizDevirSenetRiskiAlacak, CAR002_DovizDevirCekRiskiBorc, CAR002_DovizDevirCekRiskiAlacak, CAR002_DovizDevirTeminat1Borc,  " +
                                "CAR002_DovizDevirTeminat1Alacak, CAR002_DovizDevirTeminat2Borc, CAR002_DovizDevirTeminat2Alacak, CAR002_DovizDevirProtestoSenet,  " +
                                "CAR002_DovizDevirKarsiliksizCek, CAR002_DovizSenetRiskiBorc, CAR002_DovizSenetRiskiAlacak, CAR002_DovizCekRiskiBorc, CAR002_DovizCekRiskiAlacak,  " +
                                "CAR002_DovizTeminat1Borc, CAR002_DovizTeminat1Alacak, CAR002_DovizTeminat2Borc, CAR002_DovizTeminat2Alacak, CAR002_DovizProtestoSenet,  " +
                                "CAR002_DovizKarsiliksizCek, CAR002_Ulke, CAR002_CariFormB, CAR002_FormBUnvanFlag, CAR002_VergiDairesiKodu, CAR002_BankaIBANNo, CAR002_AktifFlag,  " +
                                "CAR002_HesapTipi, CAR002_YetkiliCep, CAR002_YetkiliCep2, CAR002_TeslimAdresi3, CAR002_Dokuman1, CAR002_Dokuman2, CAR002_Dokuman3,  " +
                                "CAR002_BankaKodu1, CAR002_BankaAdi1, CAR002_SubeKodu1, CAR002_IBAN1, CAR002_BankaKodu2, CAR002_BankaAdi2, CAR002_SubeKodu2, CAR002_IBAN2,  " +
                                "CAR002_BankaKodu3, CAR002_BankaAdi3, CAR002_SubeKodu3, CAR002_IBAN3, CAR002_BankaKodu4, CAR002_BankaAdi4, CAR002_SubeKodu4, CAR002_IBAN4,  " +
                                "CAR002_FiyatListeNoAlis, CAR002_FiyatListeNoSatis, CAR002_IrsaliyeFormNo, CAR002_FaturaFormNo, CAR002_ILKodu, CAR002_ILCEKodu, CAR002_PostaKodu,  " +
                                "CAR002_IrsaliyeRGNFormName, CAR002_FaturaRGNFormName) " +
                               "VALUES (@HesapKodu,@Unvan1,N'',N'',N'',N'',@VergiDairesi,@VergiHesapNo,@Telefon1,@BolgeKodu,N'',N'', " +
                               "0.00,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,'Y5004',@GirenTarih,N'093717',@GirenKodu,'6.1.00','Y5004',@DegistirenTarih, " +
                               "N'093717',@DegistirenKodu,'6.1.00',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0.00,0.00,6,0,2,1,0, " +
                               "1,N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0,N'',0,0.00,N'',N'',N'',N'',0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0,0,0,0,N'',N'',N'',N'',N'',N'',N'', " +
                               "N'',N'',N'',0.00,0,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,@Ulke,2,1,0,N'',1,1,N'',N'',N'',N'',N'',N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,0,0,0,0,0,N'',N'',N'')";

                                CmdCariAktar = new SqlCommand(CariKartAktarım, ConCariAktar);
                                CmdCariAktar.Parameters.Add("@HesapKodu", SqlDbType.NVarChar).Value = HesapKodu.ToString();
                                CmdCariAktar.Parameters.Add("@Unvan1", SqlDbType.NVarChar).Value = CariHesapAdi.ToString();
                                CmdCariAktar.Parameters.Add("@Telefon1", SqlDbType.NVarChar).Value = Telefon.ToString();
                                CmdCariAktar.Parameters.Add("@BolgeKodu", SqlDbType.NVarChar).Value = BolgeKodu;
                                CmdCariAktar.Parameters.Add("@VergiDairesi", SqlDbType.NVarChar).Value = VergiDairesi.ToString();
                                CmdCariAktar.Parameters.Add("@VergiHesapNo", SqlDbType.NVarChar).Value = VergiNo.ToString();
                                CmdCariAktar.Parameters.Add("@GirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdCariAktar.Parameters.Add("@GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdCariAktar.Parameters.Add("@DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdCariAktar.Parameters.Add("@DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdCariAktar.Parameters.Add("@Ulke", SqlDbType.Int).Value = UlkeKodu;


                                CmdCariAktar.CommandTimeout = 120;
                                CmdCariAktar.ExecuteNonQuery();

                                #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                                string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                                                           "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                                CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                                CmdUpdateMrpCariKart.CommandTimeout = 120;
                                CmdUpdateMrpCariKart.ExecuteNonQuery();

                                #endregion

                            }
                            else if (LinkCariDurumu > 0)
                            {
                                string UpdateCariKart = "UPDATE YNS" + Sirket.ToString() + ".CAR002 SET CAR002_Unvan1='" + CariHesapAdi.ToString() + "',CAR002_Telefon1='" + Telefon.ToString() + "', " +
                                "CAR002_VergiDairesi='" + VergiDairesi.ToString() + "',CAR002_VergiHesapNo='" + VergiNo.ToString() + "' " +
                                "WHERE CAR002_Row_ID=" + Convert.ToInt32(LinkCariID.ToString()) + "";

                                CmdUpdateCariKart = new SqlCommand(UpdateCariKart, ConUpdateCariKart);

                                CmdUpdateCariKart.CommandTimeout = 120;
                                CmdUpdateCariKart.ExecuteNonQuery();

                                #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                                string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                                                           "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                                CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                                CmdUpdateMrpCariKart.CommandTimeout = 120;
                                CmdUpdateMrpCariKart.ExecuteNonQuery();

                                #endregion
                            }

                            #endregion

                            #region STK002 Tarafı

                            SqlConnection constksorgula = new SqlConnection(Cs1);

                            if (constksorgula.State == ConnectionState.Closed)
                                constksorgula.Open();

                            string stksorguluyoruz = "SELECT STK002_Row_ID AS ID,STK002_Miktari AS StkMiktar,STK002_TeslimMiktari AS TeslimMiktari " +
                                                    "FROM YNS" + Sirket.ToString() + ".STK002 " +
                                                    "WHERE RIGHT(STK002_EvrakSeriNo,LEN('" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString() + "'))='" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString() + "' AND STK002_EvrakSeriNo2='" + ds.Sevkislemi.Rows[rowsatir]["SiparisID"].ToString() + "'";

                            SqlCommand cmdStkSorgula = new SqlCommand(stksorguluyoruz, constksorgula);
                            SqlDataReader drStkSorgu = cmdStkSorgula.ExecuteReader(CommandBehavior.CloseConnection);

                            decimal SevkMiktar = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString());
                            decimal stkMiktar = 0;
                            decimal stkTMiktar = 0;
                            int Stk2ID = 0;

                            if (drStkSorgu.Read())
                            {
                                Stk2ID = Convert.ToInt32(drStkSorgu["ID"].ToString());
                                stkMiktar = Convert.ToDecimal(drStkSorgu["StkMiktar"].ToString());
                                stkTMiktar = Convert.ToDecimal(drStkSorgu["TeslimMiktari"].ToString());
                            }

                            int sMiktar = Convert.ToInt32(SevkMiktar);
                            int kMiktar = Convert.ToInt32(stkMiktar);
                            int TesMiktar = Convert.ToInt32(stkTMiktar) + sMiktar;

                            drStkSorgu.Dispose();
                            drStkSorgu.Close();

                            if (TesMiktar >= kMiktar)
                            {
                                string STK002Update = "UPDATE YNS" + Sirket.ToString() + ".STK002 " +
                                "SET  STK002_TeslimMiktari = @STK002_TeslimMiktari, STK002_IrsaliyeNo = @STK002_IrsaliyeNo, STK002_SipDurumu = @STK002_SipDurumu " +
                                "Where STK002_Row_ID=@STK002_Row_ID";

                                CmdStk02Update = new SqlCommand(STK002Update, ConStk02Update);

                                CmdStk02Update.Parameters.Add("@STK002_TeslimMiktari", SqlDbType.Float).Value = Convert.ToDecimal(TesMiktar);
                                CmdStk02Update.Parameters.Add("@STK002_SipDurumu", SqlDbType.TinyInt).Value = 1;
                                CmdStk02Update.Parameters.Add("@STK002_IrsaliyeNo", SqlDbType.NVarChar).Value = EvrakNumaramiz.ToString();
                                CmdStk02Update.Parameters.Add("@STK002_Row_ID", SqlDbType.Int).Value = Stk2ID;

                                CmdStk02Update.CommandTimeout = 120;
                                CmdStk02Update.ExecuteNonQuery();
                            }
                            else if (TesMiktar < kMiktar)
                            {
                                string STK002Update = "UPDATE YNS" + Sirket.ToString() + ".STK002 " +
                                    "SET  STK002_TeslimMiktari += @STK002_TeslimMiktari, STK002_IrsaliyeNo = @STK002_IrsaliyeNo " +
                                    "Where STK002_Row_ID=@STK002_Row_ID";

                                CmdStk02Update = new SqlCommand(STK002Update, ConStk02Update);

                                CmdStk02Update.Parameters.Add("@STK002_TeslimMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(TesMiktar.ToString().Replace(",", "."));
                                //CmdStk02Update.Parameters.Add("@STK002_SipDurumu", SqlDbType.TinyInt).Value = 0;
                                CmdStk02Update.Parameters.Add("@STK002_IrsaliyeNo", SqlDbType.NVarChar).Value = EvrakNumaramiz.ToString();
                                CmdStk02Update.Parameters.Add("@STK002_Row_ID", SqlDbType.Int).Value = Stk2ID;

                                CmdStk02Update.CommandTimeout = 120;
                                CmdStk02Update.ExecuteNonQuery();
                            }

                            #endregion

                            #region Stok Kartını Kontrol Ediyoruz

                            string Stk004Sorgu = "SELECT COUNT(STK004_MalKodu) AS Durum  " +
                                   "FROM YNS" + Sirket.ToString() + ".STK004 " +
                                   "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_sku_def AS STOK ON checkok = 0 AND " +
                                   "STOK.sku=STK004_MalKodu " +
                                   "Where STK004_MalKodu='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "' ";

                            CmdStk004 = new SqlCommand(Stk004Sorgu, ConStk004);
                            CmdStk004.CommandTimeout = 120;
                            Stk004Dr = CmdStk004.ExecuteReader();

                            if (Stk004Dr.Read())
                            {
                                Stk004Durum = Convert.ToInt32(Stk004Dr["Durum"].ToString());
                            }

                            CmdStk004.Dispose();
                            Stk004Dr.Dispose();
                            Stk004Dr.Close();

                            if (Stk004Durum == 0)
                            {
                                string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                                                     "Where sku='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "'";

                                CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                                CmdMrpStk.CommandTimeout = 120;
                                MrpStkDr = CmdMrpStk.ExecuteReader();

                                if (MrpStkDr.Read())
                                {
                                    MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                    MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                                }

                                if (MrpStokAciklama.ToString().Length > 50)
                                {
                                    MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                                }

                                CmdMrpStk.Dispose();
                                MrpStkDr.Dispose();
                                MrpStkDr.Close();

                                CmdStkEkle = new SqlCommand(STK004inset, ConStkEkle);

                                CmdStkEkle.Parameters.Add("@STK004_MalKodu", SqlDbType.NVarChar).Value = MrpStokKodu.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_CikisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                                CmdStkEkle.Parameters.Add("@STK004_CikisTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(".", ","));
                                CmdStkEkle.Parameters.Add("@STK004_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkEkle.Parameters.Add("@STK004_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkEkle.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_SonCikisTarihi", SqlDbType.Int).Value = TarihRakam;

                                CmdStkEkle.CommandTimeout = 120;
                                CmdStkEkle.ExecuteNonQuery();

                                #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                                string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                                                           "WHERE sku='" + MrpStokKodu.ToString() + "'";

                                CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                                CmdUpdateMrpStokKart.CommandTimeout = 120;
                                CmdUpdateMrpStokKart.ExecuteNonQuery();

                                #endregion

                            }
                            else if (Stk004Durum > 0)
                            {
                                string PcAdi2 = PCAdi.ToString();

                                if (PcAdi2.ToString().Length > 5)
                                {
                                    PcAdi2 = PcAdi2.ToString().Substring(0, 5);
                                }

                                string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                                "Where sku='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "'";

                                CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                                CmdMrpStk.CommandTimeout = 120;
                                MrpStkDr = CmdMrpStk.ExecuteReader();

                                if (MrpStkDr.Read())
                                {
                                    MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                    MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                                }

                                if (!string.IsNullOrEmpty(MrpStokAciklama))
                                {
                                    if (MrpStokAciklama.ToString().Length > 50)
                                    {
                                        MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                                    }
                                }

                                CmdMrpStk.Dispose();
                                MrpStkDr.Dispose();
                                MrpStkDr.Close();

                                string Stk004Update = "UPDATE YNS" + Sirket.ToString() + ".STK004 SET STK004_CikisMiktari += @STK004_CikisMiktari, " +
                                "STK004_CikisTutari += @STK004_CikisTutari, STK004_DegistirenTarih = @STK004_DegistirenTarih, STK004_DegistirenKodu = @STK004_DegistirenKodu " +
                                "WHERE (STK004_MalKodu = '" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "')";

                                CmdStkUpdate = new SqlCommand(Stk004Update, ConStkUpdate);

                                CmdStkUpdate.Parameters.Add("@STK004_CikisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar.ToString().Replace(",", "."));
                                CmdStkUpdate.Parameters.Add("@STK004_CikisTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(",", "."));
                                CmdStkUpdate.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkUpdate.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PcAdi2.ToString();

                                CmdStkUpdate.CommandTimeout = 120;
                                CmdStkUpdate.ExecuteNonQuery();

                                #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                                if (!string.IsNullOrEmpty(MrpStokKodu))
                                {

                                    string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                                                               "WHERE sku='" + MrpStokKodu.ToString() + "'";

                                    CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                                    CmdUpdateMrpStokKart.CommandTimeout = 120;
                                    CmdUpdateMrpStokKart.ExecuteNonQuery();
                                }
                                #endregion
                            }

                            #endregion

                            LinkCariDurumu = 0;
                            LinkCariID = "0";
                            Stk004Durum = 0;
                        }
                        else if (OtvDurumu == false)
                        {
                            rowsatir = ForRowSayilari[d];

                            string EvrakNostk2 = ds.Sevkislemi.Rows[rowsatir]["SiparisID"].ToString();
                            decimal BirimFiyatstk02 = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Fiyat"].ToString());

                            decimal Stk004Total = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Toplam"].ToString());
                            decimal Stk005Kdv = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Kdv"].ToString());
                            string Stk004BirimMiktar = ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString();

                            #region değişkenler

                            HesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            string EvNo = ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString().TrimStart();
                            string EvrakNo2 = EvNo.ToString();

                            if (EvNo.ToString().Length == 6)
                            {
                                EvNo = "  " + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 5)
                            {
                                EvNo = "  0" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 4)
                            {
                                EvNo = "  00" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }

                            decimal Total = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString()) * BirimFiyatstk02;

                            #endregion

                            #region STK005 Normal İrsaliye Oluşturuyoruz

                            CmdStk005 = new SqlCommand(STK005insert2, ConStk005);

                            double STK005Birim;
                            bool resultBirimStk005 = double.TryParse(BirimFiyatstk02.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out STK005Birim);

                            double STK005Total;
                            bool resultTotalStk005 = double.TryParse(Stk004Total.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out STK005Total);

                            CmdStk005.Parameters.Add("@STK005_MalKodu", SqlDbType.NVarChar).Value = ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString();
                            CmdStk005.Parameters.Add("@STK005_IslemTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_CariHesapKodu", SqlDbType.NVarChar).Value = HesapKodu.ToString();
                            CmdStk005.Parameters.Add("@STK005_EvrakSeriNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                            CmdStk005.Parameters.Add("@STK005_Miktari", SqlDbType.Decimal).Value = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString());
                            CmdStk005.Parameters.Add("@STK005_BirimFiyati", SqlDbType.Decimal).Value = Convert.ToDecimal(BirimFiyatstk02.ToString().Replace(".", ","));
                            CmdStk005.Parameters.Add("@STK005_Tutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(".", ","));
                            CmdStk005.Parameters.Add("@STK005_KdvTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk005Kdv.ToString().Replace(".", ","));
                            CmdStk005.Parameters.Add("@STK005_FaturaDurumu", SqlDbType.TinyInt).Value = 0;
                            CmdStk005.Parameters.Add("@STK005_SEQNo", SqlDbType.Int).Value = 1;
                            CmdStk005.Parameters.Add("@STK005_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStk005.Parameters.Add("@STK005_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStk005.Parameters.Add("@STK005_AsilEvrakTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_SevkTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_EvrakSeriNo2", SqlDbType.NVarChar).Value = EvrakNo2.ToString();
                            CmdStk005.Parameters.Add("@STK005_SiparisNo", SqlDbType.NVarChar).Value = EvrakNostk2.ToString();
                            CmdStk005.Parameters.Add("@STK005_VadeTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_IrsaliyeFaturaTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_SiparisTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStk005.Parameters.Add("@STK005_EvrakTipi", SqlDbType.SmallInt).Value = 13;
                            CmdStk005.Parameters.Add("@STK005_TeslimCHKodu", SqlDbType.NVarChar).Value = HesapKodu.ToString();

                            CmdStk005.CommandTimeout = 120;
                            CmdStk005.ExecuteNonQuery();

                            #endregion

                            #region Cari Kontrol Ediyoruz

                            string CariKontrolSorgu = "Select cus_num AS CariID, " +
                                        "(CASE  SUBSTRING(cus_code, 1, 1) " +
                                        "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                                        "ELSE cus_code END) AS HesapKodu, " +
                                        "(Select CASE cusven " +
                                        "WHEN 'V' THEN 'SATICI' " +
                                        "WHEN 'C' THEN 'MÜŞTERİ' " +
                                        "END) AS Durum,cus_name AS HesapAdi, " +
                                        "(Select CASE sprice_type " +
                                        "WHEN 'STANDART' THEN 'EUR' " +
                                        "END) AS Kur,adress1 AS Adres, " +
                                        "(Select CASE country_code " +
                                        "WHEN 'TR' THEN '52' " +
                                        "WHEN 'CY' THEN '601' " +
                                        "END) AS UlkeKodu, " +
                                        "(Select CASE isnumeric(city) " +
                                        "WHEN 0 THEN 0 " +
                                        "WHEN 1 THEN city " +
                                        "END) AS BolgeKodu,tax_office AS Vergi_Dairesi,tax_number AS VergiNo,tel1 AS Telefon From " + MrpSirket.ToString() + ".dbo.ab_cus_def AS Musteri " +
                                        "Where (CASE  SUBSTRING(cus_code, 1, 1) " +
                                        "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                                        "ELSE cus_code END)='" + HesapKodu.ToString() + "'";

                            CmdCariKontrol = new SqlCommand(CariKontrolSorgu, ConCariKontrol);
                            CmdCariKontrol.CommandTimeout = 120;
                            CariKontrolDr = CmdCariKontrol.ExecuteReader();

                            if (CariKontrolDr.Read())
                            {
                                MrpCariID = CariKontrolDr["CariID"].ToString();
                                CariKontrolDurum = CariKontrolDr["HesapKodu"].ToString();
                                CariHesapKodu = CariKontrolDr["HesapKodu"].ToString();
                                Durum = CariKontrolDr["Durum"].ToString();
                                CariHesapAdi = CariKontrolDr["HesapAdi"].ToString();
                                Kur = CariKontrolDr["Kur"].ToString();
                                Adres = CariKontrolDr["Adres"].ToString();
                                UlkeKodu2 = CariKontrolDr["UlkeKodu"].ToString();
                                BolgeKodu = Convert.ToInt32(CariKontrolDr["BolgeKodu"].ToString());
                                VergiDairesi = CariKontrolDr["Vergi_Dairesi"].ToString();
                                VergiNo = CariKontrolDr["VergiNo"].ToString();
                                Telefon = CariKontrolDr["Telefon"].ToString();
                            }

                            if (!string.IsNullOrEmpty(UlkeKodu2))
                            {
                                UlkeKodu = Convert.ToInt32(UlkeKodu2);
                            }
                            else
                            {
                                UlkeKodu = 52;
                            }

                            CariKontrolDr.Dispose();
                            CariKontrolDr.Close();

                            if (!string.IsNullOrEmpty(Adres))
                            {
                                if (Adres.ToString().Length > 40)
                                {
                                    Adres = Adres.ToString().Substring(0, 40);
                                }
                            }

                            if (!string.IsNullOrEmpty(VergiDairesi))
                            {
                                if (VergiDairesi.ToString().Length > 32)
                                {
                                    VergiDairesi = VergiDairesi.ToString().Substring(0, 32);
                                }
                            }

                            if (!string.IsNullOrEmpty(CariHesapAdi))
                            {
                                if (CariHesapAdi.ToString().Length > 40)
                                {
                                    CariHesapAdi = CariHesapAdi.ToString().Substring(0, 40);
                                }
                            }

                            #endregion

                            #region Cari Kartı Kontrol Edip Aktarıyoruz

                            //string LinkKontrolSorgu = "Select Count(CAR002_HesapKodu) Durum From YNS" + Sirket.ToString() + ".CAR002 " +
                            //        "Where CAR002_HesapKodu='" + AktarimHesapKodu.ToString() + "'";
                            string LinkKontrolSorgu = "SELECT CAR002_Row_ID AS LinkCariID, Count(CAR002_HesapKodu) Durum FROM YNS" + Sirket.ToString() + ".CAR002 " +
                                             "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_cus_def ON checkok = 0 AND " +
                                             "CASE cusven  " +
                                             "WHEN 'V' THEN SUBSTRING(REPLACE(cus_code,'M',''),1,LEN(cus_code)) " +
                                             "WHEN 'C' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1)  " +
                                             "END = SUBSTRING(CAR002_HesapKodu,1,LEN(CAR002_HesapKodu)) " +
                                             "WHERE CAR002_HesapKodu='" + HesapKodu.ToString() + "' " +
                                             " GROUP BY CAR002_Row_ID,CAR002_HesapKodu";

                            CmdLinkKontrol = new SqlCommand(LinkKontrolSorgu, ConLinkKontrol);
                            CmdLinkKontrol.CommandTimeout = 120;
                            LinkKontrolDr = CmdLinkKontrol.ExecuteReader();

                            if (LinkKontrolDr.Read())
                            {
                                LinkCariDurumu = Convert.ToInt32(LinkKontrolDr["Durum"].ToString());
                                LinkCariID = LinkKontrolDr["LinkCariID"].ToString();
                            }

                            CmdLinkKontrol.Dispose();
                            LinkKontrolDr.Dispose();
                            LinkKontrolDr.Close();

                            if (LinkCariDurumu == 0)
                            {
                                string CariKartAktarım = "INSERT INTO YNS" + Sirket.ToString() + ".CAR002(CAR002_HesapKodu, CAR002_Unvan1, CAR002_Unvan2, CAR002_Adres1, CAR002_Adres2, CAR002_Adres3, CAR002_VergiDairesi, " +
                                "CAR002_VergiHesapNo, CAR002_Telefon1, CAR002_BolgeKodu, CAR002_GrupKodu, CAR002_MuhasebeKodu, CAR002_IskontoOrani, CAR002_OpsiyonGunu,  " +
                                "CAR002_DevirTarihi, CAR002_DevirBorc, CAR002_DevirAlacak, CAR002_DigerBorc, CAR002_DigerAlacak, CAR002_OdemeBorc, CAR002_OdemeAlacak,  " +
                                "CAR002_CiroBorc, CAR002_CiroAlacak, CAR002_IadeBorc, CAR002_IadeAlacak, CAR002_KDVBorc, CAR002_KDVAlacak, CAR002_otvborc, CAR002_otvalacak,  " +
                                "CAR002_GirenKaynak, CAR002_GirenTarih, CAR002_GirenSaat, CAR002_GirenKodu, CAR002_GirenSurum, CAR002_DegistirenKaynak, CAR002_DegistirenTarih,  " +
                                "CAR002_DegistirenSaat, CAR002_DegistirenKodu, CAR002_DegistirenSurum, CAR002_Telefon2, CAR002_Telefon3, CAR002_Telefon4, CAR002_Fax,  " +
                                "CAR002_EMailAdresi, CAR002_InternetAdresi, CAR002_OzelKodu, CAR002_TipKodu, CAR002_Kod1, CAR002_Kod2, CAR002_Kod3, CAR002_Kod4, CAR002_Kod5,  " +
                                "CAR002_Kod6, CAR002_Kod7, CAR002_Kod8, CAR002_Kod9, CAR002_UygulanacakFiyat, CAR002_UygulanacakBankaKodu, CAR002_UygulanacakFiyatTipi,  " +
                                "CAR002_UygulananFiyat, CAR002_UygulananBankaKodu, CAR002_UygulananFiyatTipi, CAR002_Yetkili1, CAR002_Yetkili1Gorevi, CAR002_Yetkili1DahiliNo,  " +
                                "CAR002_Yetkili1EMail, CAR002_Yetkili2, CAR002_Yetkili2Gorevi, CAR002_Yetkili2DahiliNo, CAR002_Yetkili2EMail, CAR002_BankaHesapKodu, CAR002_BankaAdi,  " +
                                "CAR002_BankaSubeKodu, CAR002_BankaHesapNo, CAR002_KrediKartNo, CAR002_OdemeGunu, CAR002_ParaBirimi, CAR002_MutabakatTarihi,  " +
                                "CAR002_MutabakatBakiyesi, CAR002_TeslimYeri1, CAR002_TeslimYeri2, CAR002_TeslimAdresi1, CAR002_TeslimAdresi2, CAR002_KrediLimiti, " +
                                "CAR002_DevirSenetRiskiBorc, CAR002_DevirSenetRiskiAlacak, CAR002_DevirCekRiskiBorc, CAR002_DevirCekRiskiAlacak, CAR002_DevirTeminat1Borc,  " +
                                "CAR002_DevirTeminat1Alacak, CAR002_DevirTeminat2Borc, CAR002_DevirTeminat2Alacak, CAR002_DevirProtestoSenet, CAR002_DevirKarsiliksizCek,  " +
                                "CAR002_SenetRiskiBorc, CAR002_SenetRiskiAlacak, CAR002_CekRiskiBorc, CAR002_CekRiskiAlacak, CAR002_Teminat1Borc, CAR002_Teminat1Alacak,  " +
                                "CAR002_Teminat2Borc, CAR002_Teminat2Alacak, CAR002_ProtestoSenet, CAR002_KarsiliksizCek, CAR002_SonBorcTarihi, CAR002_SonAlacakTarihi,  " +
                                "CAR002_SonRiskBorcTarihi, CAR002_SonRiskAlacakTarihi, CAR002_Notlar1, CAR002_Notlar2, CAR002_Notlar3, CAR002_Notlar4, CAR002_Notlar5, CAR002_Notlar6,  " +
                                "CAR002_Notlar7, CAR002_MasrafMerkezi, CAR002_Sifre, CAR002_Resim, CAR002_YaslandirmaBakiye, CAR002_YaslandirmaTarihi, CAR002_YaslandirmaGunu,  " +
                                "CAR002_OdemeTarihi, CAR002_DovizMutabakatBakiyesi, CAR002_DovizDevirBorc, CAR002_DovizDevirAlacak, CAR002_DovizDigerBorc, CAR002_DovizDigerAlacak,  " +
                                "CAR002_DovizOdemeBorc, CAR002_DovizOdemeAlacak, CAR002_DovizCiroBorc, CAR002_DovizCiroAlacak, CAR002_DovizIadeBorc, CAR002_DovizIadeAlacak,  " +
                                "CAR002_DovizKDVBorc, CAR002_DovizKDVAlacak, CAR002_Dovizotvborc, CAR002_Dovizotvalacak, CAR002_DovizKrediLimiti, CAR002_DovizDevirSenetRiskiBorc,  " +
                                "CAR002_DovizDevirSenetRiskiAlacak, CAR002_DovizDevirCekRiskiBorc, CAR002_DovizDevirCekRiskiAlacak, CAR002_DovizDevirTeminat1Borc,  " +
                                "CAR002_DovizDevirTeminat1Alacak, CAR002_DovizDevirTeminat2Borc, CAR002_DovizDevirTeminat2Alacak, CAR002_DovizDevirProtestoSenet,  " +
                                "CAR002_DovizDevirKarsiliksizCek, CAR002_DovizSenetRiskiBorc, CAR002_DovizSenetRiskiAlacak, CAR002_DovizCekRiskiBorc, CAR002_DovizCekRiskiAlacak,  " +
                                "CAR002_DovizTeminat1Borc, CAR002_DovizTeminat1Alacak, CAR002_DovizTeminat2Borc, CAR002_DovizTeminat2Alacak, CAR002_DovizProtestoSenet,  " +
                                "CAR002_DovizKarsiliksizCek, CAR002_Ulke, CAR002_CariFormB, CAR002_FormBUnvanFlag, CAR002_VergiDairesiKodu, CAR002_BankaIBANNo, CAR002_AktifFlag,  " +
                                "CAR002_HesapTipi, CAR002_YetkiliCep, CAR002_YetkiliCep2, CAR002_TeslimAdresi3, CAR002_Dokuman1, CAR002_Dokuman2, CAR002_Dokuman3,  " +
                                "CAR002_BankaKodu1, CAR002_BankaAdi1, CAR002_SubeKodu1, CAR002_IBAN1, CAR002_BankaKodu2, CAR002_BankaAdi2, CAR002_SubeKodu2, CAR002_IBAN2,  " +
                                "CAR002_BankaKodu3, CAR002_BankaAdi3, CAR002_SubeKodu3, CAR002_IBAN3, CAR002_BankaKodu4, CAR002_BankaAdi4, CAR002_SubeKodu4, CAR002_IBAN4,  " +
                                "CAR002_FiyatListeNoAlis, CAR002_FiyatListeNoSatis, CAR002_IrsaliyeFormNo, CAR002_FaturaFormNo, CAR002_ILKodu, CAR002_ILCEKodu, CAR002_PostaKodu,  " +
                                "CAR002_IrsaliyeRGNFormName, CAR002_FaturaRGNFormName) " +
                               "VALUES (@HesapKodu,@Unvan1,N'',N'',N'',N'',@VergiDairesi,@VergiHesapNo,@Telefon1,@BolgeKodu,N'',N'', " +
                               "0.00,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,'Y5004',@GirenTarih,N'093717',@GirenKodu,'6.1.00','Y5004',@DegistirenTarih, " +
                               "N'093717',@DegistirenKodu,'6.1.00',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0.00,0.00,6,0,2,1,0, " +
                               "1,N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0,N'',0,0.00,N'',N'',N'',N'',0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0,0,0,0,N'',N'',N'',N'',N'',N'',N'', " +
                               "N'',N'',N'',0.00,0,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                               "0.00,0.00,@Ulke,2,1,0,N'',1,1,N'',N'',N'',N'',N'',N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,0,0,0,0,0,N'',N'',N'')";

                                CmdCariAktar = new SqlCommand(CariKartAktarım, ConCariAktar);
                                CmdCariAktar.Parameters.Add("@HesapKodu", SqlDbType.NVarChar).Value = HesapKodu.ToString();
                                CmdCariAktar.Parameters.Add("@Unvan1", SqlDbType.NVarChar).Value = CariHesapAdi.ToString();
                                CmdCariAktar.Parameters.Add("@Telefon1", SqlDbType.NVarChar).Value = Telefon.ToString();
                                CmdCariAktar.Parameters.Add("@BolgeKodu", SqlDbType.NVarChar).Value = BolgeKodu;
                                CmdCariAktar.Parameters.Add("@VergiDairesi", SqlDbType.NVarChar).Value = VergiDairesi.ToString();
                                CmdCariAktar.Parameters.Add("@VergiHesapNo", SqlDbType.NVarChar).Value = VergiNo.ToString();
                                CmdCariAktar.Parameters.Add("@GirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdCariAktar.Parameters.Add("@GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdCariAktar.Parameters.Add("@DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdCariAktar.Parameters.Add("@DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdCariAktar.Parameters.Add("@Ulke", SqlDbType.Int).Value = UlkeKodu;


                                CmdCariAktar.CommandTimeout = 120;
                                CmdCariAktar.ExecuteNonQuery();

                                #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                                string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                                                           "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                                CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                                CmdUpdateMrpCariKart.CommandTimeout = 120;
                                CmdUpdateMrpCariKart.ExecuteNonQuery();

                                #endregion

                            }
                            else if (LinkCariDurumu > 0)
                            {
                                string UpdateCariKart = "UPDATE YNS" + Sirket.ToString() + ".CAR002 SET CAR002_Unvan1='" + CariHesapAdi.ToString() + "',CAR002_Telefon1='" + Telefon.ToString() + "', " +
                                "CAR002_VergiDairesi='" + VergiDairesi.ToString() + "',CAR002_VergiHesapNo='" + VergiNo.ToString() + "' " +
                                "WHERE CAR002_Row_ID=" + Convert.ToInt32(LinkCariID.ToString()) + "";

                                CmdUpdateCariKart = new SqlCommand(UpdateCariKart, ConUpdateCariKart);

                                CmdUpdateCariKart.CommandTimeout = 120;
                                CmdUpdateCariKart.ExecuteNonQuery();

                                #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                                string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                                                           "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                                CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                                CmdUpdateMrpCariKart.CommandTimeout = 120;
                                CmdUpdateMrpCariKart.ExecuteNonQuery();

                                #endregion
                            }

                            #endregion

                            #region STK002 Tarafı

                            SqlConnection constksorgula = new SqlConnection(Cs1);

                            if (constksorgula.State == ConnectionState.Closed)
                                constksorgula.Open();

                            string stksorguluyoruz = "SELECT STK002_Row_ID AS ID,STK002_Miktari AS StkMiktar,STK002_TeslimMiktari AS TeslimMiktari " +
                                                    "FROM YNS" + Sirket.ToString() + ".STK002 " +
                                                    "WHERE RIGHT(STK002_EvrakSeriNo,LEN('" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString() + "'))='" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString() + "' "+
                                                    "AND STK002_MalKodu='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "' AND STK002_EvrakSeriNo2='" + ds.Sevkislemi.Rows[rowsatir]["SiparisID"].ToString() + "'";

                            SqlCommand cmdStkSorgula = new SqlCommand(stksorguluyoruz, constksorgula);
                            SqlDataReader drStkSorgu = cmdStkSorgula.ExecuteReader(CommandBehavior.CloseConnection);

                            decimal SevkMiktar = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString());
                            decimal stkMiktar = 0;
                            decimal stkTMiktar = 0;
                            int Stk2ID = 0;

                            if (drStkSorgu.Read())
                            {
                                Stk2ID = Convert.ToInt32(drStkSorgu["ID"].ToString());
                                stkMiktar = Convert.ToDecimal(drStkSorgu["StkMiktar"].ToString());
                                stkTMiktar = Convert.ToDecimal(drStkSorgu["TeslimMiktari"].ToString());
                            }

                            int sMiktar = Convert.ToInt32(SevkMiktar);
                            int kMiktar = Convert.ToInt32(stkMiktar);
                            int TesMiktar = Convert.ToInt32(stkTMiktar) + sMiktar;

                            drStkSorgu.Dispose();
                            drStkSorgu.Close();

                            if (TesMiktar >= kMiktar)
                            {
                                string STK002Update = "UPDATE YNS" + Sirket.ToString() + ".STK002 " +
                                "SET  STK002_TeslimMiktari = @STK002_TeslimMiktari, STK002_IrsaliyeNo = @STK002_IrsaliyeNo, STK002_SipDurumu = @STK002_SipDurumu " +
                                "Where STK002_Row_ID=@STK002_Row_ID";

                                CmdStk02Update = new SqlCommand(STK002Update, ConStk02Update);

                                CmdStk02Update.Parameters.Add("@STK002_TeslimMiktari", SqlDbType.Float).Value = Convert.ToDecimal(TesMiktar);
                                CmdStk02Update.Parameters.Add("@STK002_SipDurumu", SqlDbType.TinyInt).Value = 1;
                                CmdStk02Update.Parameters.Add("@STK002_IrsaliyeNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                                CmdStk02Update.Parameters.Add("@STK002_Row_ID", SqlDbType.Int).Value = Stk2ID;

                                CmdStk02Update.CommandTimeout = 120;
                                CmdStk02Update.ExecuteNonQuery();
                            }
                            else if (TesMiktar < kMiktar)
                            {
                                string STK002Update = "UPDATE YNS" + Sirket.ToString() + ".STK002 " +
                                    "SET  STK002_TeslimMiktari += @STK002_TeslimMiktari, STK002_IrsaliyeNo = @STK002_IrsaliyeNo " +
                                    "Where STK002_Row_ID=@STK002_Row_ID";

                                CmdStk02Update = new SqlCommand(STK002Update, ConStk02Update);

                                CmdStk02Update.Parameters.Add("@STK002_TeslimMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(TesMiktar.ToString().Replace(",", "."));
                                //CmdStk02Update.Parameters.Add("@STK002_SipDurumu", SqlDbType.TinyInt).Value = 0;
                                CmdStk02Update.Parameters.Add("@STK002_IrsaliyeNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                                CmdStk02Update.Parameters.Add("@STK002_Row_ID", SqlDbType.Int).Value = Stk2ID;

                                CmdStk02Update.CommandTimeout = 120;
                                CmdStk02Update.ExecuteNonQuery();
                            }

                            #endregion

                            #region Stok Kartını Kontrol Ediyoruz

                            string Stk004Sorgu = "SELECT COUNT(STK004_MalKodu) AS Durum  " +
                                   "FROM YNS" + Sirket.ToString() + ".STK004 " +
                                   "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_sku_def AS STOK ON checkok = 0 AND " +
                                   "STOK.sku=STK004_MalKodu " +
                                   "Where STK004_MalKodu='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "' ";

                            CmdStk004 = new SqlCommand(Stk004Sorgu, ConStk004);
                            CmdStk004.CommandTimeout = 120;
                            Stk004Dr = CmdStk004.ExecuteReader();

                            if (Stk004Dr.Read())
                            {
                                Stk004Durum = Convert.ToInt32(Stk004Dr["Durum"].ToString());
                            }

                            CmdStk004.Dispose();
                            Stk004Dr.Dispose();
                            Stk004Dr.Close();

                            if (Stk004Durum == 0)
                            {
                                string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                                                     "Where sku='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "'";

                                CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                                CmdMrpStk.CommandTimeout = 120;
                                MrpStkDr = CmdMrpStk.ExecuteReader();

                                if (MrpStkDr.Read())
                                {
                                    MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                    MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                                }

                                if (MrpStokAciklama.ToString().Length > 50)
                                {
                                    MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                                }

                                CmdMrpStk.Dispose();
                                MrpStkDr.Dispose();
                                MrpStkDr.Close();

                                CmdStkEkle = new SqlCommand(STK004inset, ConStkEkle);

                                CmdStkEkle.Parameters.Add("@STK004_MalKodu", SqlDbType.NVarChar).Value = MrpStokKodu.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_CikisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                                CmdStkEkle.Parameters.Add("@STK004_CikisTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(".", ","));
                                CmdStkEkle.Parameters.Add("@STK004_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkEkle.Parameters.Add("@STK004_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkEkle.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                                CmdStkEkle.Parameters.Add("@STK004_SonCikisTarihi", SqlDbType.Int).Value = TarihRakam;

                                CmdStkEkle.CommandTimeout = 120;
                                CmdStkEkle.ExecuteNonQuery();

                                #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                                string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                                                           "WHERE sku='" + MrpStokKodu.ToString() + "'";

                                CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                                CmdUpdateMrpStokKart.CommandTimeout = 120;
                                CmdUpdateMrpStokKart.ExecuteNonQuery();

                                #endregion

                            }
                            else if (Stk004Durum > 0)
                            {
                                string PcAdi2 = PCAdi.ToString();

                                if (PcAdi2.ToString().Length > 5)
                                {
                                    PcAdi2 = PcAdi2.ToString().Substring(0, 5);
                                }

                                string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                                    "Where sku='" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "'";

                                CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                                CmdMrpStk.CommandTimeout = 120;
                                MrpStkDr = CmdMrpStk.ExecuteReader();

                                if (MrpStkDr.Read())
                                {
                                    MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                    MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                                }

                                if (MrpStokAciklama.ToString().Length > 50)
                                {
                                    MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                                }

                                CmdMrpStk.Dispose();
                                MrpStkDr.Dispose();
                                MrpStkDr.Close();

                                string Stk004Update = "UPDATE YNS" + Sirket.ToString() + ".STK004 SET STK004_CikisMiktari += @STK004_CikisMiktari, " +
                                "STK004_CikisTutari += @STK004_CikisTutari, STK004_DegistirenTarih = @STK004_DegistirenTarih, STK004_DegistirenKodu = @STK004_DegistirenKodu " +
                                "WHERE (STK004_MalKodu = '" + ds.Sevkislemi.Rows[rowsatir]["MalKodu"].ToString() + "')";

                                CmdStkUpdate = new SqlCommand(Stk004Update, ConStkUpdate);

                                CmdStkUpdate.Parameters.Add("@STK004_CikisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar.ToString().Replace(",", "."));
                                CmdStkUpdate.Parameters.Add("@STK004_CikisTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(",", "."));
                                CmdStkUpdate.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                                CmdStkUpdate.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PcAdi2.ToString();

                                CmdStkUpdate.CommandTimeout = 120;
                                CmdStkUpdate.ExecuteNonQuery();

                                #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                                string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                                                           "WHERE sku='" + MrpStokKodu.ToString() + "'";

                                CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                                CmdUpdateMrpStokKart.CommandTimeout = 120;
                                CmdUpdateMrpStokKart.ExecuteNonQuery();

                                #endregion
                            }

                            LinkCariDurumu = 0;
                            LinkCariID = "0";
                            Stk004Durum = 0;

                            #endregion
                        }
                    }

                    #region Car005 İnsert işlemleri

                    string CarEvrakNo = "";
                    string CarEvrakNo2 = "";
                    decimal Car005Toplami = 0;
                    decimal Car005KdvTutari = 0;
                    decimal Car005OtvToplami = 0;

                    for (int e = 0; e <= OtvDurumEvrak3.Length - 1; e++)
                    {
                        if (OtvDurumu == true)
                        {
                            rowsatir = ForRowSayilari[e];

                            CarEvrakNo = ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            Car005Toplami = Convert.ToDecimal(String.Format("{0:0.##}",ds.Sevkislemi.Rows[rowsatir]["Toplam"].ToString())) + Car005Toplami;
                            Car005KdvTutari = Convert.ToDecimal(String.Format("{0:0.##}",ds.Sevkislemi.Rows[rowsatir]["Kdv"].ToString())) + Car005KdvTutari;
                            Car005OtvToplami = Convert.ToDecimal(String.Format("{0:0.##}", ds.Sevkislemi.Rows[rowsatir]["OtvTutari"].ToString())) + Car005OtvToplami;

                        }
                        else if (OtvDurumu == false)
                        {
                            rowsatir = ForRowSayilari[e];

                            Car005Toplami = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Toplam"].ToString()) + Car005Toplami;
                            Car005KdvTutari = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Kdv"].ToString()) + Car005KdvTutari;
                        }
                    }

                    if (OtvDurumu == true)
                    {
                        for (int h = 0; h < 1; h++)
                        {
                            rowsatir = ForRowSayilari[h];
                            string Car005Toplam = Convert.ToString(Car005Toplami);
                            string Car005KdvTut = Convert.ToString(Car005KdvTutari);
                            decimal BirimFiyatstk02 = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Fiyat"].ToString());
                            decimal KdvMatrahi = Car005Toplami + Car005OtvToplami;

                            #region değişkenler

                            HesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            string EvNo = ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString().TrimStart();
                            string EvrakNo2 = EvNo.ToString();

                            if (EvNo.ToString().Length == 6)
                            {
                                EvNo = "  " + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 5)
                            {
                                EvNo = "  0" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 4)
                            {
                                EvNo = "  00" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }

                            decimal Total = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString()) * BirimFiyatstk02;

                            #endregion

                            #region Car005 İnsert Parametreleri

                            string SatirTipi = "";
                            string SatirAciklama = "";
                            string YeniTutar = "";
                            string YeniOran = "";
                            string OtvTutari = "";
                            int SatirNo = 5;
                            string CariHesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            for (int j = 0; j < 6; j++)
                            {
                                if (j == 0)
                                {
                                    SatirTipi = "Z";
                                    SatirAciklama = "";
                                    YeniTutar = "0.00";
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }

                                if (j == 1)
                                {
                                    SatirTipi = "T";
                                    SatirAciklama = "MAL BEDELI";
                                    YeniTutar = Convert.ToString(Car005Toplami.ToString().Replace(",", "."));
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                     "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 2)
                                {
                                    SatirTipi = "O";
                                    SatirAciklama = "ÖZEL TÜKETİM VERG.";
                                    YeniTutar = Convert.ToString(Car005OtvToplami.ToString().Replace(",", "."));
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                     "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 3)
                                {
                                    SatirTipi = "U";
                                    SatirAciklama = "KDV MATRAHI";
                                    YeniTutar = Convert.ToString(KdvMatrahi.ToString().Replace(",", "."));
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                     "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 4)
                                {
                                    SatirTipi = "K";
                                    SatirAciklama = "KATMA DEGER VERGISI";
                                    YeniTutar = Convert.ToString(Car005KdvTutari).Replace(",", ".");
                                    YeniOran = Convert.ToString(Car005KdvTutari * 100 / (Car005Toplami + Car005OtvToplami)).ToString().Replace(",", ".");

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 5)
                                {
                                    SatirTipi = "G";
                                    SatirAciklama = "GENEL TOPLAM";

                                    double KdvyiAliyoruz;
                                    bool CariKdvResult = double.TryParse(Car005KdvTut.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out KdvyiAliyoruz);

                                    double ToplamiAliyoruz;
                                    bool CariToplamResult = double.TryParse(Car005Toplam, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out ToplamiAliyoruz);

                                    decimal KdvTutarToplami = Convert.ToDecimal(Car005KdvTut) + Convert.ToDecimal(Car005Toplam) + Convert.ToDecimal(Car005OtvToplami);
                                    YeniTutar = Convert.ToString(KdvTutarToplami).Replace(",", ".");
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (17," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }

                                CmdSCar = new SqlCommand(Car005Sorgu, ConSCar);
                                CmdSCar.CommandTimeout = 120;
                                CmdSCar.ExecuteNonQuery();
                                SatirNo++;
                            }

                            Car005Toplami = 0;
                            Car005KdvTutari = 0;
                        }

                            #endregion

                    }
                    else if (OtvDurumu == false)
                    {
                        for (int k = 0; k < 1; k++)
                        {
                            rowsatir = ForRowSayilari[k];
                            
                            string Car005Toplam = Convert.ToString(Car005Toplami);
                            string Car005KdvTut = Convert.ToString(Car005KdvTutari);

                            string EvrakNostk2 = ds.Sevkislemi.Rows[rowsatir]["SiparisID"].ToString();
                            decimal BirimFiyatstk02 = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Fiyat"].ToString());

                            string SatirTipi = "";
                            string SatirAciklama = "";
                            string YeniTutar = "";
                            string YeniOran = "";
                            int SatirNo = 3;
                            string CariHesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            #region değişkenler

                            HesapKodu = ds.Sevkislemi.Rows[rowsatir]["HesapKodu"].ToString();

                            string EvNo = ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString().TrimStart();
                            string EvrakNo2 = EvNo.ToString();

                            if (EvNo.ToString().Length == 6)
                            {
                                EvNo = "  " + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 5)
                            {
                                EvNo = "  0" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }
                            else if (EvNo.ToString().Length == 4)
                            {
                                EvNo = "  00" + ds.Sevkislemi.Rows[rowsatir]["EvrakNo"].ToString();
                            }

                            decimal Total = Convert.ToDecimal(ds.Sevkislemi.Rows[rowsatir]["Miktar"].ToString()) * BirimFiyatstk02;

                            #endregion

                            #region Car005 İnsert Parametreleri

                            for (int j = 0; j < 6; j++)
                            {

                                if (j == 0)
                                {
                                    SatirTipi = "Z";
                                    SatirAciklama = "";
                                    YeniTutar = "0.00";
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 1)
                                {
                                    SatirTipi = "T";
                                    SatirAciklama = "MAL BEDELI";
                                    YeniTutar = Convert.ToString(Car005Toplami.ToString().Replace(",", "."));
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                     "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 2)
                                {
                                    SatirTipi = "K";
                                    SatirAciklama = "KATMA DEGER VERGISI";
                                    YeniTutar = Convert.ToString(Car005KdvTutari).Replace(",", ".");
                                    YeniOran = Convert.ToString(Car005KdvTutari * 100 / Car005Toplami).ToString().Replace(",", ".");

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 3)
                                {
                                    SatirTipi = "Z";
                                    SatirAciklama = "";
                                    YeniTutar = "0.00";
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 4)
                                {
                                    SatirTipi = "G";
                                    SatirAciklama = "GENEL TOPLAM";

                                    double KdvyiAliyoruz;
                                    bool CariKdvResult = double.TryParse(Car005KdvTut.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out KdvyiAliyoruz);

                                    double ToplamiAliyoruz;
                                    bool CariToplamResult = double.TryParse(Car005Toplam, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out ToplamiAliyoruz);

                                    decimal KdvTutarToplami = Convert.ToDecimal(Car005KdvTut) + Convert.ToDecimal(Car005Toplam);
                                    YeniTutar = Convert.ToString(KdvTutarToplami).Replace(",", ".");
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }
                                else if (j == 5)
                                {
                                    SatirTipi = "Y";
                                    SatirAciklama = "";
                                    YeniTutar = "0.00";
                                    YeniOran = "0.00";

                                    Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                      "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                      "VALUES (2," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + CariHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                                }

                                CmdSCar = new SqlCommand(Car005Sorgu, ConSCar);
                                CmdSCar.CommandTimeout = 120;
                                CmdSCar.ExecuteNonQuery();
                                SatirNo++;
                            }

                            #endregion
                        }
                    }

                    #endregion

                    for (int b = 0; b < OtvDurumEvrak3.Length; b++)
                    {
                        Array.Resize(ref OtvDurumEvrak3, OtvDurumEvrak3.Length - 1);
                        Array.Resize(ref BuldugumuzOtvOrani1, BuldugumuzOtvOrani1.Length - 1);
                        Array.Resize(ref OtvliMalKodu, OtvliMalKodu.Length - 1);
                        Array.Resize(ref ForRowSayilari, ForRowSayilari.Length - 1);
                        siralamaMak = 1;
                    }
                    Array.Resize(ref OtvDurumEvrak3, siralamaMak);
                    Array.Resize(ref BuldugumuzOtvOrani1, siralamaMak);
                    Array.Resize(ref OtvliMalKodu, siralamaMak);
                    Array.Resize(ref ForRowSayilari, siralamaMak);
                }

                #endregion

                #region MRP SEVK TABLOLARINDA CHECKOK GÜNCELLİYORUZ

                foreach (DataRow drUpdateSevkDet in ds.Sevkislemi.Rows)
                {

                    string SevkDetUpdate = "UPDATE DET  "+
                                           "SET DET.checkok = 1 "+
                                           "FROM zynp.dbo.ab_sevk_det AS DET "+
                                           "INNER JOIN zynp.dbo.ab_sevk_mst AS MST ON DET.sevk_no=MST.sevk_no "+
                                           "WHERE  DET.sku='" + drUpdateSevkDet["MalKodu"].ToString() + "' AND "+
                                           "DET.sn=" + Convert.ToInt32(drUpdateSevkDet["SiparisID"].ToString()) + " "+
                                           "AND MST.bill_number='" + drUpdateSevkDet["EvrakNo"].ToString() + "'";

                    CmdUpdateSevkDet = new SqlCommand(SevkDetUpdate, ConUpdateSevkDet);
                    CmdUpdateSevkDet.ExecuteNonQuery();

                }

                foreach (DataRow drUpdateSevkMst in ds.Sevkislemi.Rows)
                {
                    string SevkMstUpdate = "UPDATE ab_sevk_mst SET checkok=1 "+
                                            "WHERE bill_number='" + drUpdateSevkMst["EvrakNo"].ToString() + "' AND (CASE  SUBSTRING(cus_code, 1, 1) " +
                                            "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) "+
                                            "ELSE cus_code END) =(CASE  SUBSTRING(cus_code, 1, 1) "+
                                            "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) "+
                                            "ELSE cus_code END)";

                    CmdUpdateSevkMst = new SqlCommand(SevkMstUpdate, ConUpdateSevkMst);
                    CmdUpdateSevkMst.ExecuteNonQuery();
                }

                #endregion

                if (CmdStkUpdate != null)
                {
                    CmdStkUpdate.Dispose();
                }
                if (CmdStkEkle != null)
                {
                    CmdStkEkle.Dispose();
                }
                CmdCariKontrol.Dispose();

                if (CmdCariAktar != null)
                {
                    CmdCariAktar.Dispose();
                }

                CariKontrolDr.Dispose();
                CariKontrolDr.Close();
                CmdSCar.Dispose();
                cmdSEQNo.Dispose();
                SEQNoDr.Dispose();
                SEQNoDr.Close();

                ConStkEkle.Dispose();
                ConSEQNo.Dispose();
                ConSCar.Dispose();
                ConCariKontrol.Dispose();
                ConCariAktar.Dispose();
                ConLinkKontrol.Dispose();
                ConStk004.Dispose();
                ConMrpStk.Dispose();
                ConStkUpdate.Dispose();

                ConStkEkle.Close();
                ConSEQNo.Close();
                ConSCar.Close();
                ConCariKontrol.Close();
                ConCariAktar.Close();
                ConLinkKontrol.Close();
                ConStk004.Close();
                ConMrpStk.Close();
                ConStkUpdate.Close();

                MessageBox.Show("Aktarımı Tamamlanan Toplam Kayıt Sayısı : " + ds.Sevkislemi.Rows.Count);

                btnAktar.Enabled = true;
                btnArama.Enabled = true;
            }
        }

        #endregion

        #region Aktarım Butonu

        private void btnAktar_Click(object sender, EventArgs e)
        {
            Sevk = new Thread(new ThreadStart(Aktarim));
            Sevk.Priority = ThreadPriority.Highest;
            Sevk.Start();
        }

        #endregion

        #region Formu Kapatıyoruz

        private void Sevkislemleri_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sevkislemleri sevkislemi = new Sevkislemleri();

            if (Sevk != null)
            {
                Sevk.Abort();
            }

            sevkislemi.Close();
        }

        #endregion
    }
}
