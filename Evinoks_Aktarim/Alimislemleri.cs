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
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;

namespace Evinoks_Aktarim
{
    public partial class Alimislemleri : Form
    {
        public Alimislemleri()
        {
            InitializeComponent();

            System.Globalization.CultureInfo yenikultur = new System.Globalization.CultureInfo("tr-TR");
            yenikultur.NumberFormat.CurrencyDecimalSeparator = ",";
            yenikultur.NumberFormat.CurrencyGroupSeparator = ",";
            yenikultur.NumberFormat.NumberDecimalSeparator = ",";
            yenikultur.NumberFormat.NumberGroupSeparator = ",";
            yenikultur.NumberFormat.PercentDecimalSeparator = ",";
            yenikultur.NumberFormat.PercentGroupSeparator = ",";
            Application.CurrentCulture = yenikultur; 
        }

        #region Bağlantı Ayarları

        public AnaForm anaFrm
        {
            get;
            set;
        }

        Thread MalKabul;

        SqlConnection ConMalKabul;
        SqlCommand CmdMalKabul;
        SqlDataReader MalKabulDr;

        SqlConnection ConUpdateMrpCariKart;
        SqlCommand CmdUpdateMrpCariKart;
        SqlDataReader DrUpdateMrpCariKart;

        SqlConnection ConUpdateMrpStokKart;
        SqlCommand CmdUpdateMrpStokKart;
        SqlDataReader DrUpdateMrpStokKart;

        SqlConnection ConUpdateCariKart;
        SqlCommand CmdUpdateCariKart;
        SqlDataReader DrUpdateCariKart;

        SqlConnection ConMalKabul2;
        SqlCommand CmdMalKabul2;
        SqlDataReader MalKabulDr2;

        SqlConnection ConMalKabulAkar;
        SqlCommand CmdMalKabulAktar;
        SqlDataReader MalKabulAktardr;

        SqlConnection ConEvrakNo;
        SqlCommand CmdEvrakNo;
        SqlDataReader EvrakNoDr;

        SqlConnection ConSEQNo;
        SqlCommand cmdSEQNo;
        SqlDataReader SEQNoDr;

        SqlConnection ConStk005;
        SqlCommand CmdStk005;
        SqlDataReader Stk005Dr;

        SqlConnection ConStk004;
        SqlCommand CmdStk004;
        SqlDataReader Stk004Dr;

        SqlConnection ConStkEkle;
        SqlCommand CmdStkEkle;
        SqlDataReader StkEkleDr;

        SqlConnection ConStkUpdate;
        SqlCommand CmdStkUpdate;
        SqlDataReader StkUpdateDr;

        SqlConnection ConCariUpdate;
        SqlCommand CmdCariUpdate;
        SqlDataReader CariUpdateDr;

        SqlConnection ConMrpStk;
        SqlCommand CmdMrpStk;
        SqlDataReader MrpStkDr;

        SqlConnection ConCar005;
        SqlCommand CmdCar005;
        SqlDataReader Car005Dr;

        SqlConnection ConCar003;
        SqlCommand CmdCar003;
        SqlDataReader Car003Dr;

        SqlConnection ConCariKontrol;
        SqlCommand CmdCariKontrol;
        SqlDataReader CariKontrolDr;

        SqlConnection ConLinkKontrol;
        SqlCommand CmdLinkKontrol;
        SqlDataReader LinkKontrolDr;

        SqlConnection ConCariAktar;
        SqlCommand CmdCariAktar;
        SqlDataReader CariAktarDr;

        string Cs1 = Properties.Settings.Default.Cs1;
        string Cs2 = Properties.Settings.Default.Cs2;

        #endregion

        #region Değişkenler

        string ID;
        string MrpCariID;
        string LinkCariID;
        string LinkStokID;
        string MalKabulSorgu;
        string Sirket;
        string MrpSirket;
        string SiraID;
        string HesapKodu;
        string Malkodu;
        double Tarih;
        int TarihRakam;
        int Stk004Durum;
        string MrpStokKodu;
        string MrpStokAciklama;
        string IrsaliyeNo;
        string EvrakNo2;
        DateTime IrsaliyeTarih;
        string Durum;
        string Durum2;
        string PCAdi;
        DateTime GirisTarih;
        string Doviz;
        string Statu;
        DateTime KayitTarih;
        string KayitEden;
        string MalDurum;
        string Birim;
        double Miktar;
        string SevkDurum;
        decimal Fiyat;
        decimal Toplam;
        int SEQNo;

        string CariKontrolDurum;
        int LinkCariDurumu;
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

        private void Alimislemleri_Load(object sender, EventArgs e)
        {
            ConMalKabul = new SqlConnection(Cs1);
            ConMalKabul2 = new SqlConnection(Cs2);
            Sirket = anaFrm.tsLinkSirket.Text;
            MrpSirket = anaFrm.MrpSirket.ToString();
        }

        #endregion

        #region Mal Kabul Arama Butonu

        private void btnArama_Click(object sender, EventArgs e)
        {
            ds.MalKabul.Clear();

            MalKabul = new Thread(new ThreadStart(MalKabulArama));
            MalKabul.Priority = ThreadPriority.Highest;
            MalKabul.Start();
        }

        #endregion

        #region Mal Kabul Arama Sorgumuz

        private void MalKabulArama()
        {
            ConMalKabul = new SqlConnection(Cs1);
            ConMalKabul2 = new SqlConnection(Cs2);

            btnAktar.Enabled = false;
            btnArama.Enabled = false;

            grdMalKabul.DataSource = null;
            grdMalKabul.DataSource = ds.MalKabul;

            MalKabulSorgu = "SELECT MalKabul.inv_seq AS ID,MalKabulDetay.inv_seq AS SiraNo, " +
                            "(CASE  SUBSTRING(cus_code, 1, 1) " +
                            "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                            "ELSE cus_code END) AS Hesap_Kodu,MalKabulDetay.sku AS MalKodu, " +
                            "bill_number AS IrsaliyeNo,bill_date AS IrsaliyeTarih, " +
                            "(SELECT CASE inv_type  " +
                            "WHEN 'ALI' Then 'MalKabul' " +
                            "WHEN 'IAD' Then 'MusteriIade' " +
                            "END )AS Durum " +
                            ",entry_date AS GirisTarihi,MalKabul.cur_code AS Doviz,MalKabul.status AS Statu,MalKabul.cre_date AS KayitTarihi, " +
                            "MalKabul.cre_user AS KayitEden, " +
                            "(SELECT CASE malkabul_type " +
                            "WHEN '0' THEN 'Irsaliye' " +
                            "WHEN '1' THEN 'Fatura' " +
                            "WHEN '2' THEN 'Diger' " +
                            "END) AS MalDurum,MalKabulDetay.um_code AS Birim,um_qty AS Miktar, " +
                            "CONVERT(DECIMAL(21,2),MalKabulFiyat.un_price) AS Fiyat, " +
                            "CONVERT(DECIMAL(21,2),MalKabulFiyat.un_price*um_qty) AS ToplamFiyat, " +
                            "CONVERT(DECIMAL(21,2),MalKabulFiyat.un_price*um_qty*StokKarti.stax_type/100) AS KdvTutari, " +
                            "CONVERT(DECIMAL(21,2),StokKarti.stax_type) AS KdvOrani, " +
                            "CONVERT(DECIMAL(21,2),MalKabulFiyat.un_price*um_qty+MalKabulFiyat.un_price*um_qty*StokKarti.stax_type/100) AS GenelToplam, " +
                            "(Select CASE sevk_onay " +
                            "WHEN '0' Then 'Onaysız' " +
                            "WHEN '1' Then 'Onaylı' " +
                            "END) AS SevkDurum FROM  " + MrpSirket.ToString() + ".dbo.ab_sas_inv_mst AS MalKabul " +
                            "INNER JOIN  " + MrpSirket.ToString() + ".dbo.ab_sas_inv_det AS MalKabulDetay " +
                            "On MalKabul.inv_seq=MalKabulDetay.inv_seq " +
                            "INNER JOIN " + MrpSirket.ToString() + ".dbo.ab_sku_def AS StokKarti ON MalKabulDetay.sku=StokKarti.sku " +
                            "INNER JOIN " + MrpSirket.ToString() + ".dbo.ab_sas_po_det AS MalKabulFiyat ON ISNULL(MalKabulDetay.po_no,0)=ISNULL(MalKabulFiyat.po_no,0) " +
                            "AND ISNULL(MalKabulFiyat.item_no,0)=ISNULL(MalKabulDetay.po_item_no,0) " +
                            "WHERE sevk_onay='1' AND MalKabul.checkok=0 AND MalKabulDetay.checkok=0 AND bill_number NOT IN  " +
                            "(SELECT STK005_EvrakSeriNo2 FROM YNS" + Sirket.ToString() + ".STK005)";

            if (ConMalKabul.State == ConnectionState.Closed)
                ConMalKabul.Open();

            if (ConMalKabul2.State == ConnectionState.Closed)
                ConMalKabul2.Open();

            CmdMalKabul = new SqlCommand(MalKabulSorgu, ConMalKabul);
            CmdMalKabul.CommandTimeout = 120;
            MalKabulDr = CmdMalKabul.ExecuteReader(CommandBehavior.CloseConnection);
            ds.MalKabul.TableName.Replace("YNS00033", "YNS" + Sirket.ToString());

            while (MalKabulDr.Read())
            {
                Thread.Sleep(50);

                ID = MalKabulDr["ID"].ToString();
                SiraID = MalKabulDr["SiraNo"].ToString();
                HesapKodu = MalKabulDr["Hesap_Kodu"].ToString();
                Malkodu = MalKabulDr["MalKodu"].ToString();
                IrsaliyeNo = MalKabulDr["IrsaliyeNo"].ToString();
                IrsaliyeTarih = Convert.ToDateTime(MalKabulDr["IrsaliyeTarih"].ToString());
                Durum = MalKabulDr["Durum"].ToString();
                GirisTarih = Convert.ToDateTime(MalKabulDr["GirisTarihi"].ToString());
                Doviz = MalKabulDr["Doviz"].ToString();
                Statu = MalKabulDr["Statu"].ToString();
                KayitTarih = Convert.ToDateTime(MalKabulDr["KayitTarihi"].ToString());
                KayitEden = MalKabulDr["KayitEden"].ToString();
                MalDurum = MalKabulDr["MalDurum"].ToString();
                Birim = MalKabulDr["Birim"].ToString();
                Miktar = Convert.ToDouble(MalKabulDr["Miktar"].ToString());
                SevkDurum = MalKabulDr["SevkDurum"].ToString();
                string AktarimFiyat = MalKabulDr["Fiyat"].ToString().Replace(".", ",");
                string AktarimToplam = MalKabulDr["ToplamFiyat"].ToString().Replace(".", ",");
                string AktarimKdv = MalKabulDr["KdvTutari"].ToString().Replace(".", ",");
                string AktarimKdvOrani = MalKabulDr["KdvOrani"].ToString().Replace(".", ",");
                string AktarimGenelToplam = MalKabulDr["GenelToplam"].ToString().Replace(".", ",");
                string AktarimHesapKodu = MalKabulDr["Hesap_Kodu"].ToString();

                int YeniMiktar = Convert.ToInt32(Math.Round(Miktar, 0));

                ds.MalKabul.AddMalKabulRow(Convert.ToInt16(ID), Convert.ToInt32(SiraID), HesapKodu, Malkodu, IrsaliyeNo,
                IrsaliyeTarih, Durum, GirisTarih, Doviz, Statu, KayitTarih, KayitEden, MalDurum, Birim,
                Convert.ToInt32(Miktar), Convert.ToDecimal(AktarimFiyat), Convert.ToDecimal(AktarimToplam), Convert.ToDecimal(AktarimKdv),
                Convert.ToDecimal(AktarimKdvOrani), Convert.ToDecimal(AktarimGenelToplam), SevkDurum);

            }

            MalKabulDr.Dispose();
            MalKabulDr.Close();
            CmdMalKabul.Dispose();
            ConMalKabul.Dispose();
            ConMalKabul.Close();

            MessageBox.Show("Arama İşlemi Tamamlanmıştır.\nToplam Bulunan Kayıt Sayısı : " + ds.MalKabul.Rows.Count);

            btnArama.Enabled = true;
            btnAktar.Enabled = true;
        }

        #endregion

        #region Aktarım Butonu

        private void btnAktar_Click(object sender, EventArgs e)
        {
            MalKabul = new Thread(new ThreadStart(MalKabulAktar));
            MalKabul.Priority = ThreadPriority.Highest;
            MalKabul.Start();
        }

        #endregion

        #region Linke Aktarıyoruz

        private void MalKabulAktar()
        {
            if (MessageBox.Show("Bulunan Mal Kabulü Kayıtlarını Linke Aktarmak İstediğinize Emin Misiniz?", "Uyarı...",
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnAktar.Enabled = false;
                btnArama.Enabled = false;

                ConMalKabulAkar = new SqlConnection(Cs1);
                ConSEQNo = new SqlConnection(Cs1);
                ConStk005 = new SqlConnection(Cs1);
                ConCar005 = new SqlConnection(Cs1);
                ConCar003 = new SqlConnection(Cs1);
                ConCariKontrol = new SqlConnection(Cs2);
                ConCariAktar = new SqlConnection(Cs1);
                ConLinkKontrol = new SqlConnection(Cs1);
                ConStk004 = new SqlConnection(Cs1);
                ConMrpStk = new SqlConnection(Cs2);
                ConStkEkle = new SqlConnection(Cs1);
                ConStkUpdate = new SqlConnection(Cs1);
                ConCariUpdate = new SqlConnection(Cs1);
                ConEvrakNo = new SqlConnection(Cs1);
                ConUpdateMrpCariKart = new SqlConnection(Cs2);
                ConUpdateCariKart = new SqlConnection(Cs1);
                ConUpdateMrpStokKart = new SqlConnection(Cs2);

                #region Bağlantıları Açıyoruz

                if (ConUpdateMrpStokKart.State == ConnectionState.Closed)
                    ConUpdateMrpStokKart.Open();

                if (ConUpdateCariKart.State == ConnectionState.Closed)
                    ConUpdateCariKart.Open();

                if (ConUpdateMrpCariKart.State == ConnectionState.Closed)
                    ConUpdateMrpCariKart.Open();

                if (ConMalKabulAkar.State == ConnectionState.Closed)
                    ConMalKabulAkar.Open();

                if (ConSEQNo.State == ConnectionState.Closed)
                    ConSEQNo.Open();

                if (ConStk005.State == ConnectionState.Closed)
                    ConStk005.Open();

                if (ConCar005.State == ConnectionState.Closed)
                    ConCar005.Open();

                if (ConCar003.State == ConnectionState.Closed)
                    ConCar003.Open();

                if (ConCariKontrol.State == ConnectionState.Closed)
                    ConCariKontrol.Open();

                if (ConCariAktar.State == ConnectionState.Closed)
                    ConCariAktar.Open();

                if (ConLinkKontrol.State == ConnectionState.Closed)
                    ConLinkKontrol.Open();

                if (ConStk004.State == ConnectionState.Closed)
                    ConStk004.Open();

                if (ConMrpStk.State == ConnectionState.Closed)
                    ConMrpStk.Open();

                if (ConStkEkle.State == ConnectionState.Closed)
                    ConStkEkle.Open();

                if (ConStkUpdate.State == ConnectionState.Closed)
                    ConStkUpdate.Open();

                if (ConCariUpdate.State == ConnectionState.Closed)
                    ConCariUpdate.Open();

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

                #region PC Adını Alıyoruz

                PCAdi = Environment.UserName.ToString();

                if (PCAdi.ToString().Length > 10)
                {
                    PCAdi = PCAdi.ToString().Substring(0, 10);
                }

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

                #region STK005 İnsert Sorgu

                string STK005insert = "INSERT INTO  YNS" + Sirket.ToString() + ".STK005(STK005_MalKodu, STK005_IslemTarihi, STK005_GC, STK005_CariHesapKodu, STK005_EvrakSeriNo, STK005_Miktari, STK005_BirimFiyati, " +
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
                      "VALUES (@STK005_MalKodu,@STK005_IslemTarihi, 0,@STK005_CariHesapKodu,@STK005_EvrakSeriNo,@STK005_Miktari,@STK005_BirimFiyati,@STK005_Tutari, 0.00, " +
                      "@STK005_KDVTutari, 2, N'', N'', N'', @STK005_FaturaDurumu, 0, 0, 1,@STK005_SEQNo, N'Y6019',@STK005_GirenTarih, N'100401',@STK005_GirenKodu,N'5.1.10', N'Y6019',@STK005_DegistirenTarih, " + 
                      "N'100401',@STK005_DegistirenKodu,N'5.1.10', 1,@STK005_AsilEvrakTarihi,@STK005_SevkTarihi, 0.00, 0.00, 0.000, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, " +
                      "N'', 0.00, 0.000000, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.00, 0.00, N'', N'', @STK005_EvrakSeriNo2, N'',@STK005_VadeTarihi,@STK005_IrsaliyeFaturaTarihi, 0, N'', 0.00, 0.00, 1, " +
                      "0, N'', N'', N'', N'', 0, 0, 0.00, 0.00, 0.00, N'', N'', 0.00, 0.00, 0.00000, 0.00000, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, N'', 4, @STK005_EvrakTipi,@STK005_TeslimCHKodu, N'', 0.00, 0.0000)";

                #endregion

                #region CAR005 Insert Sorgu

                string CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, "+ 
                "CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, "+
                "CAR005_Tutar, CAR005_Oran) " +
                "VALUES (@CAR005_Secenek,@CAR005_FaturaTarihi,@CAR005_FaturaNo, N'A', 4, N'G', 0, N'', N'', N'',@CAR005_CHKodu,@CAR005_Tutar, 0.00)";

                #endregion

                #region CAR003 Insert Sorgu

                string CAR003insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR003(CAR003_HesapKodu, CAR003_Tarih, CAR003_IslemTipi, CAR003_EvrakSeriNo, CAR003_Aciklama, CAR003_BA, CAR003_Tutar, " +
                      "CAR003_VadeTarihi, CAR003_KarsiEvrakSeriNo, CAR003_Kod1, CAR003_Kod2, CAR003_KDVOrani, CAR003_KDVDahilHaric, CAR003_MuhasebelesmeDurumu, " +
                      "CAR003_ParaBirimi, CAR003_SEQNo, CAR003_GirenKaynak, CAR003_GirenTarih, CAR003_GirenSaat, CAR003_GirenKodu, CAR003_GirenSurum, " +
                      "CAR003_DegistirenKaynak, CAR003_DegistirenTarih, CAR003_DegistirenSaat, CAR003_DegistirenKodu, CAR003_DegistirenSurum, CAR003_IptalDurumu, " +
                      "CAR003_AsilEvrakTarihi, CAR003_otvdahilharic, CAR003_otvtutari, CAR003_KarsiHesapKodu, CAR003_Kod3, CAR003_Kod4, CAR003_Kod5, CAR003_Kod6, " +
                      "CAR003_Kod7, CAR003_Kod8, CAR003_Kod9, CAR003_Kod10, CAR003_Kod11, CAR003_Kod12, CAR003_Tutar2, CAR003_Tarih2, CAR003_Aciklama2, " +
                      "CAR003_EvrakSeriNo2, CAR003_DovizCinsi, CAR003_DovizTutari, CAR003_DovizKuru, CAR003_SenetCekBordroNo, CAR003_SenetCekPozisyonTipi, " +
                      "CAR003_MuhasebelesmeSekli, CAR003_MuhasebeFisTarihi, CAR003_MuhasebeTipi, CAR003_MuhasebeFisNumarasi, CAR003_MuhasebeFisKodu, " +
                      "CAR003_MuhasebeSiraNo, CAR003_MuhasebeHesapNo, CAR003_MuhasebeKarsiHeaspNo, CAR003_MuhasebeYevmiyeSekli, CAR003_IskontoTuru, " +
                      "CAR003_EvrakSeriNo3, CAR003_MasrafMerkezi, CAR003_VadeFarkiTarihi, CAR003_VadeFarkiTutari, CAR003_FaizOrani, CAR003_Ulke, CAR003_VergiHesapNo, " +
                      "CAR003_Unvani, CAR003_EvrakSayisi, CAR003_EvrakTipi, CAR003_MHSMaddeNo, CAR003_IBAN, CAR003_KKPOSTableRowID, CAR003_KKTaksitSayisi, " +
                      "CAR003_KKTaksitNo, CAR003_KKKomisyonID) " +
                      "VALUES (@CAR003_HesapKodu,@CAR003_Tarih, 4,@CAR003_EvrakSeriNo, N'Faturası', 1,@CAR003_Tutar,@CAR003_VadeTarihi, N'', N'', N'', 4, 0, 0, 1,@CAR003_SEQNo, " +
                      "N'Y6023',@GirenTarih, N'143807',@CAR003_GirenKodu, N'5.1.10', N'Y6023',@CAR003_DegistirenTarih, N'143807',@CAR003_DegistirenKodu, N'5.1.10', " +
                      "1,@CAR003_AsilEvrakTarihi, 0, 0.00, N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.00, 0.00, 0.00, 0, N'', @CAR003_EvrakSeriNo2, N'', 0.00, 0.000000, N'', 0, 1, 0, 0, 0, N'', 0, N'', N'', 0, N'', N'', N'', " +
                      "0, 0.00, 0.00, 0, N'', N'', 1, 21, 0, N'', 0, 0, 0, 0)";

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
                      "0, 0.0000, 0.00, @STK004_GirisMiktari,@STK004_GirisTutari, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0, 0.0000, N'Y4004',@STK004_GirenTarih, " +
                      "N'100323',@STK004_GirenKodu, N'5.1.10', N'Y6019',@STK004_DegistirenTarih, N'100401',@STK004_DegistirenKodu, N'5.1.10', N'', N'', N'', N'', N'', N'', N'', N'', N'', " +
                      "0.000, 0.000, N'', N'', N'', N'', 1, 0.00000, N'', 0, N'', N'', 0.00, 0.00, 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.0000, " +
                      "0.0000, 0.0000, @STK004_SonGirisTarihi, 0, 0.0000, 0, N'', N'', N'', N'', N'', 0, 0.000000, 0, 0, 1, 0.0000, N'', N'', N'', N'', @STK004_SonAlimFaturasiTarihi, " +
                      "@STK004_SonAlimFaturasiNo, @STK004_SonAlimFaturasiBirimFiyati, @STK004_SonAlimFaturasiCariHesapKodu, 0.000000, 0, N'', " +
                      "0.000000, N'', 0.000000, N'', N'', 0, 0, 0, 0, 0, 0, 0, 0.00, 0.00, 0.00, 0.00, 0.00, N'', N'', N'', 0.00, 0.00, 0.00, 0.0000, 0.0000, 0.000, 0.0000, 0.000, N'', 0.0000, 0.00, " +
                      "0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, 0, 0, 0, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                      "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, " +
                      "0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                      "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0, N'', 0, " +
                      "N'', 0, N'', 0, N'', N'', 0, 0.000, N'', N'', N'', N'', N'', N'', N'', 0.0000, 0.00, 0.00, 0, 0, 0, 0.00, N'', 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0, 0, 0, 1, 0, 0.0000, N'', " +
                      "N'', N'', N'')";

                #endregion

                MessageBox.Show("Aktarım İşlemi Başlamıştır.Lütfen İşlem Bitmeden Programı Kapatmayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                #region Aktarıma Başlıyoruz

                foreach (DataRow dr in ds.MalKabul.Rows)
                {
                    Thread.Sleep(50);
                    SEQNo++;

                    #region Evrak Numarasını Kontrol Edip Ayarlıyoruz

                    string EvNo = dr["IrsaliyeNo"].ToString().TrimStart();
                    EvrakNo2 = dr["IrsaliyeNo"].ToString().TrimStart();

                    if (EvNo.ToString().Length == 6)
                    {
                        EvNo = "  " + dr["IrsaliyeNo"].ToString();
                    }
                    else if (EvNo.ToString().Length == 5)
                    {
                        EvNo = "  0" + dr["IrsaliyeNo"].ToString();
                    }
                    else if (EvNo.ToString().Length == 4)
                    {
                        EvNo = "  00" + dr["IrsaliyeNo"].ToString();
                    }
                    else if (EvNo.ToString().Length == 3)
                    {
                        EvNo = "  000" + dr["IrsaliyeNo"].ToString();
                    }

                    #endregion

                    string AktarimFiyat = dr["Fiyat"].ToString();
                    string AktarimToplam = dr["ToplamFiyat"].ToString();
                    string AktarimHesapKodu = dr["Hesap_Kodu"].ToString();
                    string MalDurumSevk = dr["MalDurum"].ToString();
                    string Price = dr["ToplamFiyat"].ToString();
                    string CarTotal = dr["ToplamFiyat"].ToString();
                    string Stk002Total = dr["ToplamFiyat"].ToString();
                    string Stk002BirimFiyat = dr["Fiyat"].ToString();
                    string Stk002Miktar = dr["Miktar"].ToString();
                    string Stk004Total = dr["ToplamFiyat"].ToString();
                    string Stk004BirimMiktar = dr["Miktar"].ToString();
                    string StkSonAlimFiyat = dr["Fiyat"].ToString();
                    string AktarimKdvTutari = dr["KdvTutari"].ToString();

                    #region Fiyatları Ayarlıyoruz

                    Price = Price.Replace(".", ",");
                    double PFiyat = double.Parse(Price);
                    Price = PFiyat.ToString("0.0000");
                    //Price = Convert.ToString(PFiyat);
                    Price = Price.Replace(",", ".");

                    Stk002BirimFiyat = Stk002BirimFiyat.Replace(".", ",");
                    double Stk002BirimFiyat1 = double.Parse(Stk002BirimFiyat);
                    //Stk002BirimFiyat = Stk002BirimFiyat1.ToString("0.000000");
                    //Price = Convert.ToString(PFiyat);
                    Stk002BirimFiyat = Stk002BirimFiyat.Replace(",", ".");

                    Stk002Miktar = Stk002Miktar.Replace(".", ",");
                    double Stk002Miktar1 = double.Parse(Stk002Miktar);
                    //Stk002Miktar = Stk002Miktar1.ToString("0.0000");
                    //Price = Convert.ToString(PFiyat);
                    Stk002Miktar = Stk002Miktar.Replace(",", ".");

                    Stk002Total = Stk002Total.Replace(".", ",");
                    double Stk002Fiyat = double.Parse(Stk002Total);
                    Stk002Total = Stk002Fiyat.ToString("0.00");
                    //Price = Convert.ToString(PFiyat);
                    Stk002Total = Stk002Total.Replace(",", ".");

                    StkSonAlimFiyat = StkSonAlimFiyat.Replace(".", ",");
                    double StkAlimFiyat = double.Parse(StkSonAlimFiyat);
                    StkSonAlimFiyat = StkAlimFiyat.ToString("0.000000");
                    //Price = Convert.ToString(PFiyat);
                    StkSonAlimFiyat = StkSonAlimFiyat.Replace(",", ".");

                    CarTotal = CarTotal.Replace(".", ",");
                    double CarFiyat = double.Parse(CarTotal);
                    CarTotal = CarFiyat.ToString("0.00");
                    //Price = Convert.ToString(PFiyat);
                    CarTotal = CarTotal.Replace(",", ".");

                    Stk004Total = Stk004Total.Replace(".", ",");
                    double Stk004Fiyat = double.Parse(Stk004Total);
                    Stk004Total = Stk004Fiyat.ToString("0.00");
                    //Price = Convert.ToString(PFiyat);
                    Stk004Total = Stk004Total.Replace(",", ".");

                    Stk004BirimMiktar = Stk004BirimMiktar.Replace(".", ",");
                    double Stk004BirimMiktar1 = double.Parse(Stk004BirimMiktar);
                    //Stk004BirimMiktar = Stk004BirimMiktar1.ToString("0.0000");
                    //Price = Convert.ToString(PFiyat);
                    Stk004BirimMiktar = Stk004BirimMiktar.Replace(",", ".");

                    #endregion

                    if (MalDurumSevk.ToString() == "Irsaliye")
                    {
                        CmdStk005 = new SqlCommand(STK005insert, ConStk005);


                        #region Stk005 İnsert Parametreleri

                        CmdStk005.Parameters.Add("@STK005_MalKodu", SqlDbType.NVarChar).Value = dr["MalKodu"].ToString();
                        CmdStk005.Parameters.Add("@STK005_IslemTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_CariHesapKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();
                        CmdStk005.Parameters.Add("@STK005_EvrakSeriNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                        CmdStk005.Parameters.Add("@STK005_Miktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk002Miktar);
                        CmdStk005.Parameters.Add("@STK005_BirimFiyati", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk002BirimFiyat.ToString().Replace(".", ","));
                        CmdStk005.Parameters.Add("@STK005_Tutari", SqlDbType.Decimal).Value = Convert.ToDecimal(CarTotal.ToString().Replace(".", ","));
                        CmdStk005.Parameters.Add("@STK005_KDVTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(AktarimKdvTutari.ToString().Replace(".", ","));
                        CmdStk005.Parameters.Add("@STK005_FaturaDurumu", SqlDbType.TinyInt).Value = 0;
                        CmdStk005.Parameters.Add("@STK005_SEQNo", SqlDbType.Int).Value = 1;
                        CmdStk005.Parameters.Add("@STK005_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                        CmdStk005.Parameters.Add("@STK005_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                        CmdStk005.Parameters.Add("@STK005_AsilEvrakTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_SevkTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_EvrakSeriNo2", SqlDbType.NVarChar).Value = EvrakNo2.ToString();
                        CmdStk005.Parameters.Add("@STK005_VadeTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_IrsaliyeFaturaTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdStk005.Parameters.Add("@STK005_EvrakTipi", SqlDbType.SmallInt).Value = 23;
                        CmdStk005.Parameters.Add("@STK005_TeslimCHKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();

                        CmdStk005.CommandTimeout = 120;
                        CmdStk005.ExecuteNonQuery();

                        #endregion

                        CmdCar005.Parameters.Add("@CAR005_Secenek", SqlDbType.NVarChar).Value = "4";
                        CmdCar005.Parameters.Add("@CAR005_FaturaTarihi", SqlDbType.Int).Value = TarihRakam;
                        CmdCar005.Parameters.Add("@CAR005_FaturaNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                        CmdCar005.Parameters.Add("@CAR005_CHKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();
                        CmdCar005.Parameters.Add("@CAR005_Tutar", SqlDbType.Decimal).Value = Convert.ToDecimal(CarTotal.ToString().Replace(".", ","));

                        CmdCar005.CommandTimeout = 120;
                        CmdCar005.ExecuteNonQuery();

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
                                    "ELSE cus_code END)='" + AktarimHesapKodu.ToString() + "'";

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
                                         "WHERE CAR002_HesapKodu='" + AktarimHesapKodu.ToString() + "' " +
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
                            CmdCariAktar.Parameters.Add("@HesapKodu", SqlDbType.NVarChar).Value = AktarimHesapKodu.ToString();
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

                        #region Stok Kartını Kontrol Ediyoruz

                        string Stk004Sorgu = "SELECT COUNT(STK004_MalKodu) AS Durum  " +
                               "FROM YNS" + Sirket.ToString() + ".STK004 " +
                               "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_sku_def AS STOK ON checkok = 0 AND " +
                               "STOK.sku=STK004_MalKodu " +
                               "Where STK004_MalKodu='" + dr["MalKodu"].ToString() + "' ";

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
                                                 "Where sku='" + dr["MalKodu"].ToString() + "'";

                            CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                            CmdMrpStk.CommandTimeout = 120;
                            MrpStkDr = CmdMrpStk.ExecuteReader();

                            if (MrpStkDr.Read())
                            {
                                MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                            }

                            CmdMrpStk.Dispose();
                            MrpStkDr.Dispose();
                            MrpStkDr.Close();

                            if (MrpStokAciklama.ToString().Length > 50)
                            {
                                MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                            }

                            CmdStkEkle = new SqlCommand(STK004inset, ConStkEkle);

                            CmdStkEkle.Parameters.Add("@STK004_MalKodu", SqlDbType.NVarChar).Value = MrpStokKodu.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_GirisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                            CmdStkEkle.Parameters.Add("@STK004_GirisTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004Total.ToString().Replace(".", ","));
                            CmdStkEkle.Parameters.Add("@STK004_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_SonGirisTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiTarihi", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiNo", SqlDbType.NVarChar).Value = "";
                            CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiBirimFiyati", SqlDbType.Float).Value = "0.000000";
                            CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiCariHesapKodu", SqlDbType.NVarChar).Value = "";

                            CmdStkEkle.CommandTimeout = 120;
                            CmdStkEkle.ExecuteNonQuery();

                            #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                            string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                                                       "WHERE sku='" + MrpStokKodu.ToString() + "'";

                            CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                            CmdUpdateMrpStokKart.CommandTimeout = 120;
                            CmdUpdateMrpStokKart.ExecuteNonQuery();

                            #endregion

                            #region MRP MAL KABUL CHECKOK GÜNCELLİYORUZ

                            SqlConnection ConMalKabulUpdateMST = new SqlConnection(Cs2);
                            SqlConnection ConMalKabulUpdateSas = new SqlConnection(Cs2);

                            if (ConMalKabulUpdateMST.State == ConnectionState.Closed)
                                ConMalKabulUpdateMST.Open();

                            if (ConMalKabulUpdateSas.State == ConnectionState.Closed)
                                ConMalKabulUpdateSas.Open();

                            string MalKabulUpdateMST = "UPDATE ab_sas_inv_mst SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                            SqlCommand CmdMalKabulUpsMST = new SqlCommand(MalKabulUpdateMST, ConMalKabulUpdateMST);
                            CmdMalKabulUpsMST.ExecuteNonQuery();

                            string MalKabulUpdatesas = "UPDATE ab_sas_inv_det SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                            SqlCommand CmdMalKabulUpsSas = new SqlCommand(MalKabulUpdatesas, ConMalKabulUpdateSas);
                            CmdMalKabulUpsSas.ExecuteNonQuery();

                            CmdMalKabulUpsMST.Dispose();
                            ConMalKabulUpdateMST.Dispose();
                            ConMalKabulUpdateMST.Close();

                            CmdMalKabulUpsSas.Dispose();
                            ConMalKabulUpdateSas.Dispose();
                            ConMalKabulUpdateSas.Close();

                            #endregion
                        }
                        else if (Stk004Durum > 0)
                        {
                            string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                            "Where sku='" + dr["MalKodu"].ToString() + "'";

                            CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                            CmdMrpStk.CommandTimeout = 120;
                            MrpStkDr = CmdMrpStk.ExecuteReader();

                            if (MrpStkDr.Read())
                            {
                                MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                                MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                            }

                            CmdMrpStk.Dispose();
                            MrpStkDr.Dispose();
                            MrpStkDr.Close();

                            string PcAdi2 = PCAdi.ToString();

                            if (PcAdi2.ToString().Length > 5)
                            {
                                PcAdi2 = PcAdi2.ToString().Substring(0, 5);
                            }

                            double STK004number;
                            bool result = double.TryParse(Stk004Total.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out STK004number);

                            string Stk004Update = "UPDATE YNS" + Sirket.ToString() + ".STK004 SET STK004_GirisMiktari += @STK004_GirisMiktari, " +
                            "STK004_GirisTutari += @STK004_GirisTutari, STK004_DegistirenTarih = @STK004_DegistirenTarih, STK004_DegistirenKodu = @STK004_DegistirenKodu " +
                            "WHERE (STK004_MalKodu = '" + dr["MalKodu"].ToString() + "')";

                            CmdStkUpdate = new SqlCommand(Stk004Update, ConStkUpdate);

                            CmdStkUpdate.Parameters.Add("@STK004_GirisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                            CmdStkUpdate.Parameters.Add("@STK004_GirisTutari", SqlDbType.Decimal).Value = STK004number.ToString().Replace(",", ".");
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

                            #region MRP MAL KABUL CHECKOK GÜNCELLİYORUZ

                            SqlConnection ConMalKabulUpdateMST = new SqlConnection(Cs2);
                            SqlConnection ConMalKabulUpdateSas = new SqlConnection(Cs2);

                            if (ConMalKabulUpdateMST.State == ConnectionState.Closed)
                                ConMalKabulUpdateMST.Open();

                            if (ConMalKabulUpdateSas.State == ConnectionState.Closed)
                                ConMalKabulUpdateSas.Open();

                            string MalKabulUpdateMST = "UPDATE ab_sas_inv_mst SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                            SqlCommand CmdMalKabulUpsMST = new SqlCommand(MalKabulUpdateMST, ConMalKabulUpdateMST);
                            CmdMalKabulUpsMST.ExecuteNonQuery();

                            string MalKabulUpdatesas = "UPDATE ab_sas_inv_det SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                            SqlCommand CmdMalKabulUpsSas = new SqlCommand(MalKabulUpdatesas, ConMalKabulUpdateSas);
                            CmdMalKabulUpsSas.ExecuteNonQuery();

                            CmdMalKabulUpsMST.Dispose();
                            ConMalKabulUpdateMST.Dispose();
                            ConMalKabulUpdateMST.Close();

                            CmdMalKabulUpsSas.Dispose();
                            ConMalKabulUpdateSas.Dispose();
                            ConMalKabulUpdateSas.Close();

                            #endregion
                        }

                        #endregion
                    }

                    #region Fatura Tarafı

                    //else if (MalDurumSevk.ToString() == "Fatura")
                    //{
                    //    CmdStk005 = new SqlCommand(STK005insert, ConStk005);

                    //    #region Stk005 İnsert Parametreleri

                    //    CmdStk005.Parameters.Add("@STK005_MalKodu", SqlDbType.NVarChar).Value = dr["MalKodu"].ToString();
                    //    CmdStk005.Parameters.Add("@STK005_IslemTarihi", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_CariHesapKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();
                    //    CmdStk005.Parameters.Add("@STK005_EvrakSeriNo", SqlDbType.NVarChar).Value = EvNo.ToString();
                    //    CmdStk005.Parameters.Add("@STK005_Miktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                    //    CmdStk005.Parameters.Add("@STK005_BirimFiyati", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk002BirimFiyat.ToString().Replace(".", ","));
                    //    CmdStk005.Parameters.Add("@STK005_Tutari", SqlDbType.Decimal).Value = Convert.ToDecimal(CarTotal.ToString().Replace(".", ","));
                    //    CmdStk005.Parameters.Add("@STK005_KDVTutari", SqlDbType.Decimal).Value = Convert.ToDecimal(AktarimKdvTutari.ToString().Replace(".", ","));
                    //    CmdStk005.Parameters.Add("@STK005_FaturaDurumu", SqlDbType.TinyInt).Value = 1;
                    //    CmdStk005.Parameters.Add("@STK005_SEQNo", SqlDbType.Int).Value = 1;
                    //    CmdStk005.Parameters.Add("@STK005_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //    CmdStk005.Parameters.Add("@STK005_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //    CmdStk005.Parameters.Add("@STK005_AsilEvrakTarihi", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_SevkTarihi", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_EvrakSeriNo2", SqlDbType.NVarChar).Value = EvrakNo2.ToString();
                    //    CmdStk005.Parameters.Add("@STK005_VadeTarihi", SqlDbType.Int).Value = TarihRakam;
                    //    CmdStk005.Parameters.Add("@STK005_IrsaliyeFaturaTarihi", SqlDbType.Int).Value = 0;
                    //    CmdStk005.Parameters.Add("@STK005_EvrakTipi", SqlDbType.SmallInt).Value = 21;
                    //    CmdStk005.Parameters.Add("@STK005_TeslimCHKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();

                    //    CmdStk005.CommandTimeout = 120;
                    //    CmdStk005.ExecuteNonQuery();

                    //    #endregion

                    //    #region Cari Kontrol Ediyoruz

                    //    string CariKontrolSorgu = "Select cus_num AS CariID, " +
                    //                "(CASE  SUBSTRING(cus_code, 1, 1) " +
                    //                "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                    //                "ELSE cus_code END) AS HesapKodu, " +
                    //                "(Select CASE cusven " +
                    //                "WHEN 'V' THEN 'SATICI' " +
                    //                "WHEN 'C' THEN 'MÜŞTERİ' " +
                    //                "END) AS Durum,cus_name AS HesapAdi, " +
                    //                "(Select CASE sprice_type " +
                    //                "WHEN 'STANDART' THEN 'EUR' " +
                    //                "END) AS Kur,adress1 AS Adres, " +
                    //                "(Select CASE country_code " +
                    //                "WHEN 'TR' THEN '52' " +
                    //                "WHEN 'CY' THEN '601' " +
                    //                "END) AS UlkeKodu, " +
                    //                "(Select CASE isnumeric(city) " +
                    //                "WHEN 0 THEN 0 " +
                    //                "WHEN 1 THEN city " +
                    //                "END) AS BolgeKodu,tax_office AS Vergi_Dairesi,tax_number AS VergiNo,tel1 AS Telefon From " + MrpSirket.ToString() + ".dbo.ab_cus_def AS Musteri " +
                    //                "Where (CASE  SUBSTRING(cus_code, 1, 1) " +
                    //                "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                    //                "ELSE cus_code END)='" + AktarimHesapKodu.ToString() + "'";

                    //    CmdCariKontrol = new SqlCommand(CariKontrolSorgu, ConCariKontrol);
                    //    CmdCariKontrol.CommandTimeout = 120;
                    //    CariKontrolDr = CmdCariKontrol.ExecuteReader();

                    //    if (CariKontrolDr.Read())
                    //    {
                    //        MrpCariID = CariKontrolDr["CariID"].ToString();
                    //        CariKontrolDurum = CariKontrolDr["HesapKodu"].ToString();
                    //        CariHesapKodu = CariKontrolDr["HesapKodu"].ToString();
                    //        Durum = CariKontrolDr["Durum"].ToString();
                    //        CariHesapAdi = CariKontrolDr["HesapAdi"].ToString();
                    //        Kur = CariKontrolDr["Kur"].ToString();
                    //        Adres = CariKontrolDr["Adres"].ToString();
                    //        UlkeKodu2 = CariKontrolDr["UlkeKodu"].ToString();
                    //        BolgeKodu = Convert.ToInt32(CariKontrolDr["BolgeKodu"].ToString());
                    //        VergiDairesi = CariKontrolDr["Vergi_Dairesi"].ToString();
                    //        VergiNo = CariKontrolDr["VergiNo"].ToString();
                    //        Telefon = CariKontrolDr["Telefon"].ToString();
                    //    }

                    //    if (!string.IsNullOrEmpty(UlkeKodu2))
                    //    {
                    //        UlkeKodu = Convert.ToInt32(UlkeKodu2);
                    //    }
                    //    else
                    //    {
                    //        UlkeKodu = 52;
                    //    }

                    //    CariKontrolDr.Dispose();
                    //    CariKontrolDr.Close();

                    //    if (!string.IsNullOrEmpty(Adres))
                    //    {
                    //        if (Adres.ToString().Length > 40)
                    //        {
                    //            Adres = Adres.ToString().Substring(0, 40);
                    //        }
                    //    }

                    //    if (!string.IsNullOrEmpty(VergiDairesi))
                    //    {
                    //        if (VergiDairesi.ToString().Length > 32)
                    //        {
                    //            VergiDairesi = VergiDairesi.ToString().Substring(0, 32);
                    //        }
                    //    }

                    //    if (!string.IsNullOrEmpty(CariHesapAdi))
                    //    {
                    //        if (CariHesapAdi.ToString().Length > 40)
                    //        {
                    //            CariHesapAdi = CariHesapAdi.ToString().Substring(0, 40);
                    //        }
                    //    }

                    //    #endregion

                    //    #region Cari Kartı Kontrol Edip Aktarıyoruz

                    //    //string LinkKontrolSorgu = "Select Count(CAR002_HesapKodu) Durum From YNS" + Sirket.ToString() + ".CAR002 " +
                    //    //        "Where CAR002_HesapKodu='" + AktarimHesapKodu.ToString() + "'";
                    //    string LinkKontrolSorgu = "SELECT CAR002_Row_ID AS LinkCariID, Count(CAR002_HesapKodu) Durum FROM YNS" + Sirket.ToString() + ".CAR002 " +
                    //                     "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_cus_def ON checkok = 0 AND " +
                    //                     "CASE cusven  " +
                    //                     "WHEN 'V' THEN SUBSTRING(REPLACE(cus_code,'M',''),1,LEN(cus_code)) " +
                    //                     "WHEN 'C' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1)  " +
                    //                     "END = SUBSTRING(CAR002_HesapKodu,1,LEN(CAR002_HesapKodu)) " +
                    //                     "WHERE CAR002_HesapKodu='" + AktarimHesapKodu.ToString() + "' " +
                    //                     " GROUP BY CAR002_Row_ID,CAR002_HesapKodu";

                    //    CmdLinkKontrol = new SqlCommand(LinkKontrolSorgu, ConLinkKontrol);
                    //    CmdLinkKontrol.CommandTimeout = 120;
                    //    LinkKontrolDr = CmdLinkKontrol.ExecuteReader();

                    //    if (LinkKontrolDr.Read())
                    //    {
                    //        LinkCariDurumu = Convert.ToInt32(LinkKontrolDr["Durum"].ToString());
                    //        LinkCariID = LinkKontrolDr["LinkCariID"].ToString();
                    //    }

                    //    CmdLinkKontrol.Dispose();
                    //    LinkKontrolDr.Dispose();
                    //    LinkKontrolDr.Close();

                    //    if (LinkCariDurumu == 0)
                    //    {
                    //        string CariKartAktarım = "INSERT INTO YNS" + Sirket.ToString() + ".CAR002(CAR002_HesapKodu, CAR002_Unvan1, CAR002_Unvan2, CAR002_Adres1, CAR002_Adres2, CAR002_Adres3, CAR002_VergiDairesi, " +
                    //        "CAR002_VergiHesapNo, CAR002_Telefon1, CAR002_BolgeKodu, CAR002_GrupKodu, CAR002_MuhasebeKodu, CAR002_IskontoOrani, CAR002_OpsiyonGunu,  " +
                    //        "CAR002_DevirTarihi, CAR002_DevirBorc, CAR002_DevirAlacak, CAR002_DigerBorc, CAR002_DigerAlacak, CAR002_OdemeBorc, CAR002_OdemeAlacak,  " +
                    //        "CAR002_CiroBorc, CAR002_CiroAlacak, CAR002_IadeBorc, CAR002_IadeAlacak, CAR002_KDVBorc, CAR002_KDVAlacak, CAR002_otvborc, CAR002_otvalacak,  " +
                    //        "CAR002_GirenKaynak, CAR002_GirenTarih, CAR002_GirenSaat, CAR002_GirenKodu, CAR002_GirenSurum, CAR002_DegistirenKaynak, CAR002_DegistirenTarih,  " +
                    //        "CAR002_DegistirenSaat, CAR002_DegistirenKodu, CAR002_DegistirenSurum, CAR002_Telefon2, CAR002_Telefon3, CAR002_Telefon4, CAR002_Fax,  " +
                    //        "CAR002_EMailAdresi, CAR002_InternetAdresi, CAR002_OzelKodu, CAR002_TipKodu, CAR002_Kod1, CAR002_Kod2, CAR002_Kod3, CAR002_Kod4, CAR002_Kod5,  " +
                    //        "CAR002_Kod6, CAR002_Kod7, CAR002_Kod8, CAR002_Kod9, CAR002_UygulanacakFiyat, CAR002_UygulanacakBankaKodu, CAR002_UygulanacakFiyatTipi,  " +
                    //        "CAR002_UygulananFiyat, CAR002_UygulananBankaKodu, CAR002_UygulananFiyatTipi, CAR002_Yetkili1, CAR002_Yetkili1Gorevi, CAR002_Yetkili1DahiliNo,  " +
                    //        "CAR002_Yetkili1EMail, CAR002_Yetkili2, CAR002_Yetkili2Gorevi, CAR002_Yetkili2DahiliNo, CAR002_Yetkili2EMail, CAR002_BankaHesapKodu, CAR002_BankaAdi,  " +
                    //        "CAR002_BankaSubeKodu, CAR002_BankaHesapNo, CAR002_KrediKartNo, CAR002_OdemeGunu, CAR002_ParaBirimi, CAR002_MutabakatTarihi,  " +
                    //        "CAR002_MutabakatBakiyesi, CAR002_TeslimYeri1, CAR002_TeslimYeri2, CAR002_TeslimAdresi1, CAR002_TeslimAdresi2, CAR002_KrediLimiti, " +
                    //        "CAR002_DevirSenetRiskiBorc, CAR002_DevirSenetRiskiAlacak, CAR002_DevirCekRiskiBorc, CAR002_DevirCekRiskiAlacak, CAR002_DevirTeminat1Borc,  " +
                    //        "CAR002_DevirTeminat1Alacak, CAR002_DevirTeminat2Borc, CAR002_DevirTeminat2Alacak, CAR002_DevirProtestoSenet, CAR002_DevirKarsiliksizCek,  " +
                    //        "CAR002_SenetRiskiBorc, CAR002_SenetRiskiAlacak, CAR002_CekRiskiBorc, CAR002_CekRiskiAlacak, CAR002_Teminat1Borc, CAR002_Teminat1Alacak,  " +
                    //        "CAR002_Teminat2Borc, CAR002_Teminat2Alacak, CAR002_ProtestoSenet, CAR002_KarsiliksizCek, CAR002_SonBorcTarihi, CAR002_SonAlacakTarihi,  " +
                    //        "CAR002_SonRiskBorcTarihi, CAR002_SonRiskAlacakTarihi, CAR002_Notlar1, CAR002_Notlar2, CAR002_Notlar3, CAR002_Notlar4, CAR002_Notlar5, CAR002_Notlar6,  " +
                    //        "CAR002_Notlar7, CAR002_MasrafMerkezi, CAR002_Sifre, CAR002_Resim, CAR002_YaslandirmaBakiye, CAR002_YaslandirmaTarihi, CAR002_YaslandirmaGunu,  " +
                    //        "CAR002_OdemeTarihi, CAR002_DovizMutabakatBakiyesi, CAR002_DovizDevirBorc, CAR002_DovizDevirAlacak, CAR002_DovizDigerBorc, CAR002_DovizDigerAlacak,  " +
                    //        "CAR002_DovizOdemeBorc, CAR002_DovizOdemeAlacak, CAR002_DovizCiroBorc, CAR002_DovizCiroAlacak, CAR002_DovizIadeBorc, CAR002_DovizIadeAlacak,  " +
                    //        "CAR002_DovizKDVBorc, CAR002_DovizKDVAlacak, CAR002_Dovizotvborc, CAR002_Dovizotvalacak, CAR002_DovizKrediLimiti, CAR002_DovizDevirSenetRiskiBorc,  " +
                    //        "CAR002_DovizDevirSenetRiskiAlacak, CAR002_DovizDevirCekRiskiBorc, CAR002_DovizDevirCekRiskiAlacak, CAR002_DovizDevirTeminat1Borc,  " +
                    //        "CAR002_DovizDevirTeminat1Alacak, CAR002_DovizDevirTeminat2Borc, CAR002_DovizDevirTeminat2Alacak, CAR002_DovizDevirProtestoSenet,  " +
                    //        "CAR002_DovizDevirKarsiliksizCek, CAR002_DovizSenetRiskiBorc, CAR002_DovizSenetRiskiAlacak, CAR002_DovizCekRiskiBorc, CAR002_DovizCekRiskiAlacak,  " +
                    //        "CAR002_DovizTeminat1Borc, CAR002_DovizTeminat1Alacak, CAR002_DovizTeminat2Borc, CAR002_DovizTeminat2Alacak, CAR002_DovizProtestoSenet,  " +
                    //        "CAR002_DovizKarsiliksizCek, CAR002_Ulke, CAR002_CariFormB, CAR002_FormBUnvanFlag, CAR002_VergiDairesiKodu, CAR002_BankaIBANNo, CAR002_AktifFlag,  " +
                    //        "CAR002_HesapTipi, CAR002_YetkiliCep, CAR002_YetkiliCep2, CAR002_TeslimAdresi3, CAR002_Dokuman1, CAR002_Dokuman2, CAR002_Dokuman3,  " +
                    //        "CAR002_BankaKodu1, CAR002_BankaAdi1, CAR002_SubeKodu1, CAR002_IBAN1, CAR002_BankaKodu2, CAR002_BankaAdi2, CAR002_SubeKodu2, CAR002_IBAN2,  " +
                    //        "CAR002_BankaKodu3, CAR002_BankaAdi3, CAR002_SubeKodu3, CAR002_IBAN3, CAR002_BankaKodu4, CAR002_BankaAdi4, CAR002_SubeKodu4, CAR002_IBAN4,  " +
                    //        "CAR002_FiyatListeNoAlis, CAR002_FiyatListeNoSatis, CAR002_IrsaliyeFormNo, CAR002_FaturaFormNo, CAR002_ILKodu, CAR002_ILCEKodu, CAR002_PostaKodu,  " +
                    //        "CAR002_IrsaliyeRGNFormName, CAR002_FaturaRGNFormName) " +
                    //       "VALUES (@HesapKodu,@Unvan1,N'',N'',N'',N'',@VergiDairesi,@VergiHesapNo,@Telefon1,@BolgeKodu,N'',N'', " +
                    //       "0.00,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,@CAR002_CiroAlacak,0.00, " +
                    //       "0.00,0.00,@CAR002_KDVAlacak,0.00,0.00,'Y5004',@GirenTarih,N'093717',@GirenKodu,'6.1.00','Y5004',@DegistirenTarih, " +
                    //       "N'093717',@DegistirenKodu,'6.1.00',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0.00,0.00,6,0,2,1,0, " +
                    //       "1,N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',N'',0,N'',0,0.00,N'',N'',N'',N'',0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                    //       "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0,@CAR002_SonAlacakTarihi,0,0,N'',N'',N'',N'',N'',N'',N'', " +
                    //       "N'',N'',N'',0.00,0,0,0,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                    //       "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00, " +
                    //       "0.00,0.00,@Ulke,2,1,0,N'',1,1,N'',N'',N'',N'',N'',N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,N'',0,0,0,0,0,0,N'',N'',N'')";

                    //        CmdCariAktar = new SqlCommand(CariKartAktarım, ConCariAktar);
                    //        CmdCariAktar.Parameters.Add("@HesapKodu", SqlDbType.NVarChar).Value = AktarimHesapKodu.ToString();
                    //        CmdCariAktar.Parameters.Add("@Unvan1", SqlDbType.NVarChar).Value = CariHesapAdi.ToString();
                    //        CmdCariAktar.Parameters.Add("@Telefon1", SqlDbType.NVarChar).Value = Telefon.ToString();
                    //        CmdCariAktar.Parameters.Add("@BolgeKodu", SqlDbType.NVarChar).Value = BolgeKodu;
                    //        CmdCariAktar.Parameters.Add("@VergiDairesi", SqlDbType.NVarChar).Value = VergiDairesi.ToString();
                    //        CmdCariAktar.Parameters.Add("@VergiHesapNo", SqlDbType.NVarChar).Value = VergiNo.ToString();
                    //        CmdCariAktar.Parameters.Add("@GirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //        CmdCariAktar.Parameters.Add("@GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //        CmdCariAktar.Parameters.Add("@DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //        CmdCariAktar.Parameters.Add("@DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //        CmdCariAktar.Parameters.Add("@Ulke", SqlDbType.Int).Value = UlkeKodu;
                    //        CmdCariAktar.Parameters.Add("@CAR002_CiroAlacak", SqlDbType.Decimal).Value = Convert.ToDecimal(CarTotal.ToString().Replace(".", ","));
                    //        CmdCariAktar.Parameters.Add("@CAR002_KDVAlacak", SqlDbType.Decimal).Value = Convert.ToDecimal(AktarimKdvTutari.ToString().Replace(".", ","));
                    //        CmdCariAktar.Parameters.Add("@CAR002_SonAlacakTarihi", SqlDbType.Int).Value = TarihRakam;

                    //        CmdCariAktar.CommandTimeout = 120;
                    //        CmdCariAktar.ExecuteNonQuery();

                    //        #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                    //        string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                    //                                   "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                    //        CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                    //        CmdUpdateMrpCariKart.CommandTimeout = 120;
                    //        CmdUpdateMrpCariKart.ExecuteNonQuery();

                    //        #endregion

                    //    }
                    //    else if (LinkCariDurumu > 0)
                    //    {
                    //        double number;
                    //        bool CariAlimresult = double.TryParse(CarTotal.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number);

                    //        AktarimKdvTutari = AktarimKdvTutari.Replace(".", ",");
                    //        decimal numberKdv = Convert.ToDecimal(AktarimKdvTutari);
                    //        //double numberKdv = Convert.ToDouble(AktarimKdvTutari.ToString(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat .CurrencyDecimalSeparator);
                    //        //CariAlimresult = double.TryParse(AktarimKdvTutari.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out numberKdv);

                    //        string UpdateCariKart = "UPDATE YNS" + Sirket.ToString() + ".CAR002 SET CAR002_Unvan1='" + CariHesapAdi.ToString() + "',CAR002_Telefon1='" + Telefon.ToString() + "', CAR002_SonAlacakTarihi = " + TarihRakam + ", " +
                    //        "CAR002_VergiDairesi='" + VergiDairesi.ToString() + "',CAR002_VergiHesapNo='" + VergiNo.ToString() + "', CAR002_KDVAlacak += " + numberKdv.ToString().Replace(",", ".") + ", CAR002_CiroAlacak += " + number.ToString().Replace(",", ".") + " " +
                    //        "WHERE CAR002_Row_ID=" + Convert.ToInt32(LinkCariID.ToString()) + "";

                    //        CmdUpdateCariKart = new SqlCommand(UpdateCariKart, ConUpdateCariKart);

                    //        CmdUpdateCariKart.CommandTimeout = 120;
                    //        CmdUpdateCariKart.ExecuteNonQuery();

                    //        #region MRP CARİ KART CHECKOK GÜNCELLENİYOR

                    //        string MrpCariKartUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_cus_def SET checkok=1 " +
                    //                                   "WHERE cus_num=" + Convert.ToInt32(MrpCariID) + "";

                    //        CmdUpdateMrpCariKart = new SqlCommand(MrpCariKartUpdate, ConUpdateMrpCariKart);
                    //        CmdUpdateMrpCariKart.CommandTimeout = 120;
                    //        CmdUpdateMrpCariKart.ExecuteNonQuery();

                    //        #endregion
                    //    }

                    //    #endregion

                    //    #region Stok Kartını Kontrol Ediyoruz

                    //    string Stk004Sorgu = "SELECT COUNT(STK004_MalKodu) AS Durum  " +
                    //           "FROM YNS" + Sirket.ToString() + ".STK004 " +
                    //           "LEFT JOIN " + MrpSirket.ToString() + ".dbo.ab_sku_def AS STOK ON checkok = 0 AND " +
                    //           "STOK.sku=STK004_MalKodu " +
                    //           "Where STK004_MalKodu='" + dr["MalKodu"].ToString() + "' ";

                    //    CmdStk004 = new SqlCommand(Stk004Sorgu, ConStk004);
                    //    CmdStk004.CommandTimeout = 120;
                    //    Stk004Dr = CmdStk004.ExecuteReader();

                    //    if (Stk004Dr.Read())
                    //    {
                    //        Stk004Durum = Convert.ToInt32(Stk004Dr["Durum"].ToString());
                    //    }

                    //    CmdStk004.Dispose();
                    //    Stk004Dr.Dispose();
                    //    Stk004Dr.Close();

                    //    if (Stk004Durum == 0)
                    //    {
                    //        string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                    //        "Where sku='" + dr["MalKodu"].ToString() + "'";

                    //        CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                    //        CmdMrpStk.CommandTimeout = 120;
                    //        MrpStkDr = CmdMrpStk.ExecuteReader();

                    //        if (MrpStkDr.Read())
                    //        {
                    //            MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                    //            MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                    //        }

                    //        CmdMrpStk.Dispose();
                    //        MrpStkDr.Dispose();
                    //        MrpStkDr.Close();

                    //        CmdStkEkle = new SqlCommand(STK004inset, ConStkEkle);

                    //        double stk04number;
                    //        bool resultstk04 = double.TryParse(Stk004Total.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out stk04number);

                    //        double stk04Birim;
                    //        bool resultstk04Birim = double.TryParse(Stk002BirimFiyat.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out stk04Birim);

                    //        CmdStkEkle.Parameters.Add("@STK004_MalKodu", SqlDbType.NVarChar).Value = MrpStokKodu.ToString();
                    //        CmdStkEkle.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                    //        CmdStkEkle.Parameters.Add("@STK004_GirisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar);
                    //        CmdStkEkle.Parameters.Add("@STK004_GirisTutari", SqlDbType.Decimal).Value = stk04number.ToString().Replace(",", ".");
                    //        CmdStkEkle.Parameters.Add("@STK004_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkEkle.Parameters.Add("@STK004_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //        CmdStkEkle.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkEkle.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                    //        CmdStkEkle.Parameters.Add("@STK004_SonGirisTarihi", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiTarihi", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiNo", SqlDbType.NVarChar).Value = EvrakNo2.ToString();
                    //        CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiBirimFiyati", SqlDbType.Decimal).Value = stk04Birim.ToString().Replace(",", ".");
                    //        CmdStkEkle.Parameters.Add("@STK004_SonAlimFaturasiCariHesapKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();

                    //        CmdStkEkle.CommandTimeout = 120;
                    //        CmdStkEkle.ExecuteNonQuery();

                    //        #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                    //        string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                    //                                   "WHERE sku='" + MrpStokKodu.ToString() + "'";

                    //        CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                    //        CmdUpdateMrpStokKart.CommandTimeout = 120;
                    //        CmdUpdateMrpStokKart.ExecuteNonQuery();

                    //        #endregion
                    //    }
                    //    else if (Stk004Durum > 0)
                    //    {
                    //        string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                    //        "Where sku='" + dr["MalKodu"].ToString() + "'";

                    //        CmdMrpStk = new SqlCommand(MrpStkSorgu, ConMrpStk);
                    //        CmdMrpStk.CommandTimeout = 120;
                    //        MrpStkDr = CmdMrpStk.ExecuteReader();

                    //        if (MrpStkDr.Read())
                    //        {
                    //            MrpStokKodu = MrpStkDr["MalKodu"].ToString();
                    //            MrpStokAciklama = MrpStkDr["MalAdi"].ToString();
                    //        }

                    //        CmdMrpStk.Dispose();
                    //        MrpStkDr.Dispose();
                    //        MrpStkDr.Close();

                    //        string PcAdi2 = PCAdi.ToString();

                    //        if (PcAdi2.ToString().Length > 5)
                    //        {
                    //            PcAdi2 = PcAdi2.ToString().Substring(0, 5);
                    //        }

                    //        string Stk004Update = "UPDATE YNS" + Sirket.ToString() + ".STK004 SET STK004_GirisMiktari += @STK004_GirisMiktari, " +
                    //        "STK004_GirisTutari += @STK004_GirisTutari, STK004_DegistirenTarih = @STK004_DegistirenTarih, STK004_DegistirenKodu = @STK004_DegistirenKodu, " +
                    //        "STK004_SonAlimFaturasiTarihi = @STK004_SonAlimFaturasiTarihi, STK004_SonAlimFaturasiNo = @STK004_SonAlimFaturasiNo, " +
                    //        "STK004_SonAlimFaturasiBirimFiyati = @STK004_SonAlimFaturasiBirimFiyati, STK004_SonAlimFaturasiCariHesapKodu = @STK004_SonAlimFaturasiCariHesapKodu " +
                    //        "WHERE (STK004_MalKodu = '" + dr["MalKodu"].ToString() + "')";

                    //        CmdStkUpdate = new SqlCommand(Stk004Update, ConStkUpdate);

                    //        double stk04number;
                    //        bool resultstk04 = double.TryParse(Stk004Total.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out stk04number);

                    //        double stk04Birim;
                    //        bool resultstk04Birim = double.TryParse(Stk002BirimFiyat.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out stk04Birim);

                    //        CmdStkUpdate.Parameters.Add("@STK004_GirisMiktari", SqlDbType.Decimal).Value = Convert.ToDecimal(Stk004BirimMiktar.Replace(",", "."));
                    //        CmdStkUpdate.Parameters.Add("@STK004_GirisTutari", SqlDbType.Decimal).Value = stk04number.ToString().Replace(",", ".");
                    //        CmdStkUpdate.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkUpdate.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PcAdi2.ToString();
                    //        CmdStkUpdate.Parameters.Add("@STK004_SonAlimFaturasiTarihi", SqlDbType.Int).Value = TarihRakam;
                    //        CmdStkUpdate.Parameters.Add("@STK004_SonAlimFaturasiNo", SqlDbType.NVarChar).Value = EvrakNo2.ToString();
                    //        CmdStkUpdate.Parameters.Add("@STK004_SonAlimFaturasiBirimFiyati", SqlDbType.Decimal).Value = stk04Birim.ToString().Replace(",", ".");
                    //        CmdStkUpdate.Parameters.Add("@STK004_SonAlimFaturasiCariHesapKodu", SqlDbType.NVarChar).Value = dr["Hesap_Kodu"].ToString();

                    //        CmdStkUpdate.CommandTimeout = 120;
                    //        CmdStkUpdate.ExecuteNonQuery();

                    //        #region MRP STOK KART CHECKOK GÜNCELLİYORUZ

                    //        string UpdateMrpStokKart = "UPDATE " + MrpSirket.ToString() + ".dbo.ab_sku_def SET checkok=1 " +
                    //                                   "WHERE sku='" + MrpStokKodu.ToString() + "'";

                    //        CmdUpdateMrpStokKart = new SqlCommand(UpdateMrpStokKart, ConUpdateMrpStokKart);
                    //        CmdUpdateMrpStokKart.CommandTimeout = 120;
                    //        CmdUpdateMrpStokKart.ExecuteNonQuery();

                    //        #endregion

                    //        #region MRP MAL KABUL CHECKOK GÜNCELLİYORUZ

                    //        SqlConnection ConMalKabulUpdateMST = new SqlConnection(Cs2);
                    //        SqlConnection ConMalKabulUpdateSas = new SqlConnection(Cs2);

                    //        if (ConMalKabulUpdateMST.State == ConnectionState.Closed)
                    //            ConMalKabulUpdateMST.Open();

                    //        if (ConMalKabulUpdateSas.State == ConnectionState.Closed)
                    //            ConMalKabulUpdateSas.Open();

                    //        string MalKabulUpdateMST = "UPDATE ab_sas_inv_mst SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                    //        SqlCommand CmdMalKabulUpsMST = new SqlCommand(MalKabulUpdateMST, ConMalKabulUpdateMST);
                    //        CmdMalKabulUpsMST.ExecuteNonQuery();

                    //        string MalKabulUpdatesas = "UPDATE ab_sas_inv_det SET checkok=1 WHERE inv_seq=" + Convert.ToInt32(dr["ID"].ToString()) + "";

                    //        SqlCommand CmdMalKabulUpsSas = new SqlCommand(MalKabulUpdatesas, ConMalKabulUpdateSas);
                    //        CmdMalKabulUpsSas.ExecuteNonQuery();

                    //        CmdMalKabulUpsMST.Dispose();
                    //        ConMalKabulUpdateMST.Dispose();
                    //        ConMalKabulUpdateMST.Close();

                    //        CmdMalKabulUpsSas.Dispose();
                    //        ConMalKabulUpdateSas.Dispose();
                    //        ConMalKabulUpdateSas.Close();

                    //        #endregion
                    //    }

                    //    #endregion
                    //}

                    #endregion

                    LinkCariDurumu = 0;
                    LinkCariID = "0";
                    Stk004Durum = 0;
                }

                #endregion

                #region Fatura Kayıtlarını Car005 ve Car003 e Aktarıyoruz

                string CarEvrakNo = "";
                string CarEvrakNo2 = "";
                decimal Car005Toplami = 0;
                decimal Car005KdvTutari = 0;

                //for (int i = 0; i <= ds.MalKabul.Rows.Count-1; i++)
                //{
                //    CarEvrakNo = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                //    string MalDurumSevk = ds.MalKabul.Rows[i]["MalDurum"].ToString();

                //    for (int f = 0; f <= ds.MalKabul.Rows.Count - 1; f++)
                //    {
                //        CarEvrakNo2 = ds.MalKabul.Rows[f]["IrsaliyeNo"].ToString();

                //        if (CarEvrakNo == CarEvrakNo2 && MalDurumSevk.ToString() == "Fatura")
                //        {
                //            Car005Toplami = Convert.ToDecimal(ds.MalKabul.Rows[f]["ToplamFiyat"].ToString()) + Car005Toplami;
                //            Car005KdvTutari = Convert.ToDecimal(ds.MalKabul.Rows[f]["KdvTutari"].ToString()) + Car005KdvTutari;
                //            i = f;
                //        }
                //    }

                //    #region Evrak Numarasını Kontrol Edip Ayarlıyoruz

                //    string EvNo = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString().TrimStart();
                //    EvrakNo2 = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString().TrimStart();

                //    if (EvNo.ToString().Length == 6)
                //    {
                //        EvNo = "  " + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                //    }
                //    else if (EvNo.ToString().Length == 5)
                //    {
                //        EvNo = "  0" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                //    }
                //    else if (EvNo.ToString().Length == 4)
                //    {
                //        EvNo = "  00" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                //    }
                //    else if (EvNo.ToString().Length == 3)
                //    {
                //        EvNo = "  000" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                //    }

                //    #endregion

                //    DateTime DtSaat = DateTime.Now;
                //    string Saatimiz = Convert.ToString(DtSaat);
                //    Saatimiz = Saatimiz.ToString().Substring(11, 8);
                //    Saatimiz = Saatimiz.ToString().Replace(":", "");

                //    if (Saatimiz.ToString().Length > 8)
                //    {
                //        Saatimiz = Saatimiz.ToString().Substring(0, 8);
                //    }

                //    string Car005Toplam = Convert.ToString(Car005Toplami);
                //    string Car005KdvTut = Convert.ToString(Car005KdvTutari);

                //    string AktarimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();
                //    string AktarimToplam = Convert.ToString(Car005Toplami).Replace(",", ".");
                //    string AktarimHesapKodu = ds.MalKabul.Rows[i]["Hesap_Kodu"].ToString();
                //    string Price = Convert.ToString(Car005Toplami).Replace(",", ".");
                //    string CarTotal = Convert.ToString(Car005Toplami).Replace(",", ".");
                //    string Stk002Total = Convert.ToString(Car005Toplami).Replace(",", ".");
                //    string Stk002BirimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();
                //    string Stk002Miktar = ds.MalKabul.Rows[i]["Miktar"].ToString();
                //    string Stk004Total = Convert.ToString(Car005Toplami).Replace(",", ".");
                //    string Stk004BirimMiktar = ds.MalKabul.Rows[i]["Miktar"].ToString();
                //    string StkSonAlimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();

                //    #region Fiyatları Ayarlıyoruz

                //    Price = Price.Replace(".", ",");
                //    double PFiyat = double.Parse(Price);
                //    Price = PFiyat.ToString("0.0000");
                //    //Price = Convert.ToString(PFiyat);
                //    Price = Price.Replace(",", ".");

                //    Stk002BirimFiyat = Stk002BirimFiyat.Replace(".", ",");
                //    double Stk002BirimFiyat1 = double.Parse(Stk002BirimFiyat);
                //    //Stk002BirimFiyat = Stk002BirimFiyat1.ToString("0.000000");
                //    //Price = Convert.ToString(PFiyat);
                //    Stk002BirimFiyat = Stk002BirimFiyat.Replace(",", ".");

                //    Stk002Miktar = Stk002Miktar.Replace(".", ",");
                //    double Stk002Miktar1 = double.Parse(Stk002Miktar);
                //    //Stk002Miktar = Stk002Miktar1.ToString("0.0000");
                //    //Price = Convert.ToString(PFiyat);
                //    Stk002Miktar = Stk002Miktar.Replace(",", ".");

                //    Stk002Total = Stk002Total.Replace(".", ",");
                //    double Stk002Fiyat = double.Parse(Stk002Total);
                //    Stk002Total = Stk002Fiyat.ToString("0.00");
                //    //Price = Convert.ToString(PFiyat);
                //    Stk002Total = Stk002Total.Replace(",", ".");

                //    StkSonAlimFiyat = StkSonAlimFiyat.Replace(".", ",");
                //    double StkAlimFiyat = double.Parse(StkSonAlimFiyat);
                //    StkSonAlimFiyat = StkAlimFiyat.ToString("0.000000");
                //    //Price = Convert.ToString(PFiyat);
                //    StkSonAlimFiyat = StkSonAlimFiyat.Replace(",", ".");

                //    CarTotal = CarTotal.Replace(".", ",");
                //    double CarFiyat = double.Parse(CarTotal);
                //    CarTotal = CarFiyat.ToString("0.00");
                //    //Price = Convert.ToString(PFiyat);
                //    CarTotal = CarTotal.Replace(",", ".");

                //    Stk004Total = Stk004Total.Replace(".", ",");
                //    double Stk004Fiyat = double.Parse(Stk004Total);
                //    Stk004Total = Stk004Fiyat.ToString("0.00");
                //    //Price = Convert.ToString(PFiyat);
                //    Stk004Total = Stk004Total.Replace(",", ".");

                //    Stk004BirimMiktar = Stk004BirimMiktar.Replace(".", ",");
                //    double Stk004BirimMiktar1 = double.Parse(Stk004BirimMiktar);
                //    //Stk004BirimMiktar = Stk004BirimMiktar1.ToString("0.0000");
                //    //Price = Convert.ToString(PFiyat);
                //    Stk004BirimMiktar = Stk004BirimMiktar.Replace(",", ".");

                //    #endregion

                //    if (CarEvrakNo == CarEvrakNo2 && MalDurumSevk.ToString() == "Fatura")
                //    {

                //        #region Car005 İnsert Parametreleri

                //        string SatirTipi = "";
                //        string SatirAciklama = "";
                //        string YeniTutar = "";
                //        string YeniOran = "";
                //        int SatirNo = 3;

                //        for (int j = 0; j < 6; j++)
                //        {

                //            if (j == 0)
                //            {
                //                SatirTipi = "Z";
                //                SatirAciklama = "";
                //                YeniTutar = "0.00";
                //                YeniOran = "0.00";

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                  "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }
                //            else if (j == 1)
                //            {
                //                SatirTipi = "T";
                //                SatirAciklama = "MAL BEDELI";
                //                YeniTutar = Stk002Total;
                //                YeniOran = "0.00";

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                 "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }
                //            else if (j == 2)
                //            {
                //                SatirTipi = "K";
                //                SatirAciklama = "KATMA DEGER VERGISI";
                //                YeniTutar = Convert.ToString(Car005KdvTutari).Replace(",", ".");
                //                YeniOran = ds.MalKabul.Rows[i]["KdvOrani"].ToString().Replace(",", ".");

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                  "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }
                //            else if (j == 3)
                //            {
                //                SatirTipi = "Z";
                //                SatirAciklama = "";
                //                YeniTutar = "0.00";
                //                YeniOran = "0.00";

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                  "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }
                //            else if (j == 4)
                //            {
                //                SatirTipi = "G";
                //                SatirAciklama = "GENEL TOPLAM";

                //                double KdvyiAliyoruz;
                //                bool CariKdvResult = double.TryParse(Car005KdvTut.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out KdvyiAliyoruz);

                //                double ToplamiAliyoruz;
                //                bool CariToplamResult = double.TryParse(Car005Toplam, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out ToplamiAliyoruz);

                //                decimal KdvTutarToplami = Convert.ToDecimal(Car005KdvTut) + Convert.ToDecimal(Car005Toplam);
                //                YeniTutar = Convert.ToString(KdvTutarToplami).Replace(",", ".");
                //                YeniOran = "0.00";

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                  "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }
                //            else if (j == 5)
                //            {
                //                SatirTipi = "Y";
                //                SatirAciklama = "";
                //                YeniTutar = "0.00";
                //                YeniOran = "0.00";

                //                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                //                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                //                  "VALUES (3," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                //            }

                //            CmdCar005 = new SqlCommand(CAR005insert, ConCar005);
                //            CmdCar005.CommandTimeout = 120;
                //            CmdCar005.ExecuteNonQuery();
                //            SatirNo++;

                //        }

                //        #endregion

                //    }
                //    if (CarEvrakNo == CarEvrakNo2 && MalDurumSevk.ToString() == "Fatura")
                //    {
                //        string YeniTutar = Car005Toplam.Replace(",", ".");
                //        string YeniOran = Car005KdvTut.Replace(",", ".");

                //        #region Car003 İnsert Parametreleri

                //        for (int a = 0; a < 2; a++)
                //        {
                //            if (a == 0)
                //            {
                //                CAR003insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR003(CAR003_HesapKodu, CAR003_Tarih, CAR003_IslemTipi, CAR003_EvrakSeriNo, CAR003_Aciklama, CAR003_BA, CAR003_Tutar, " +
                //      "CAR003_VadeTarihi, CAR003_KarsiEvrakSeriNo, CAR003_Kod1, CAR003_Kod2, CAR003_KDVOrani, CAR003_KDVDahilHaric, CAR003_MuhasebelesmeDurumu, " +
                //      "CAR003_ParaBirimi, CAR003_SEQNo, CAR003_GirenKaynak, CAR003_GirenTarih, CAR003_GirenSaat, CAR003_GirenKodu, CAR003_GirenSurum, " +
                //      "CAR003_DegistirenKaynak, CAR003_DegistirenTarih, CAR003_DegistirenSaat, CAR003_DegistirenKodu, CAR003_DegistirenSurum, CAR003_IptalDurumu, " +
                //      "CAR003_AsilEvrakTarihi, CAR003_otvdahilharic, CAR003_otvtutari, CAR003_KarsiHesapKodu, CAR003_Kod3, CAR003_Kod4, CAR003_Kod5, CAR003_Kod6, " +
                //      "CAR003_Kod7, CAR003_Kod8, CAR003_Kod9, CAR003_Kod10, CAR003_Kod11, CAR003_Kod12, CAR003_Tutar2, CAR003_Tarih2, CAR003_Aciklama2, " +
                //      "CAR003_EvrakSeriNo2, CAR003_DovizCinsi, CAR003_DovizTutari, CAR003_DovizKuru, CAR003_SenetCekBordroNo, CAR003_SenetCekPozisyonTipi, " +
                //      "CAR003_MuhasebelesmeSekli, CAR003_MuhasebeFisTarihi, CAR003_MuhasebeTipi, CAR003_MuhasebeFisNumarasi, CAR003_MuhasebeFisKodu, " +
                //      "CAR003_MuhasebeSiraNo, CAR003_MuhasebeHesapNo, CAR003_MuhasebeKarsiHeaspNo, CAR003_MuhasebeYevmiyeSekli, CAR003_IskontoTuru, " +
                //      "CAR003_EvrakSeriNo3, CAR003_MasrafMerkezi, CAR003_VadeFarkiTarihi, CAR003_VadeFarkiTutari, CAR003_FaizOrani, CAR003_Ulke, CAR003_VergiHesapNo, " +
                //      "CAR003_Unvani, CAR003_EvrakSayisi, CAR003_EvrakTipi, CAR003_MHSMaddeNo, CAR003_IBAN, CAR003_KKPOSTableRowID, CAR003_KKTaksitSayisi, " +
                //      "CAR003_KKTaksitNo, CAR003_KKKomisyonID) " +
                //      "VALUES ('" + AktarimHesapKodu.ToString() + "'," + TarihRakam + ", 4,'" + EvNo.ToString() + "', N'Faturası', 1,'" + YeniTutar + "'," + TarihRakam + ", N'', N'', N'', 4, 0, 0, 1,1, " +
                //      "N'Y6023'," + TarihRakam + ", '" + Saatimiz + "','" + PCAdi.ToString() + "', N'5.1.10', N'Y6023'," + TarihRakam + ", '" + Saatimiz + "','" + PCAdi.ToString() + "', N'5.1.10', " +
                //      "1," + TarihRakam + ", 0, 0.00, N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.00, 0.00, 0.00, 0, N'', '" + EvrakNo2.ToString() + "', N'', 0.00, 0.000000, N'', 0, 1, 0, 0, 0, N'', 0, N'', N'', 0, N'', N'', N'', " +
                //      "0, 0.00, 0.00, 0, N'', N'', 1, 21, 0, N'', 0, 0, 0, 0)";
                //            }
                //            else if (a == 1)
                //            {
                //                CAR003insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR003(CAR003_HesapKodu, CAR003_Tarih, CAR003_IslemTipi, CAR003_EvrakSeriNo, CAR003_Aciklama, CAR003_BA, CAR003_Tutar, " +
                //      "CAR003_VadeTarihi, CAR003_KarsiEvrakSeriNo, CAR003_Kod1, CAR003_Kod2, CAR003_KDVOrani, CAR003_KDVDahilHaric, CAR003_MuhasebelesmeDurumu, " +
                //      "CAR003_ParaBirimi, CAR003_SEQNo, CAR003_GirenKaynak, CAR003_GirenTarih, CAR003_GirenSaat, CAR003_GirenKodu, CAR003_GirenSurum, " +
                //      "CAR003_DegistirenKaynak, CAR003_DegistirenTarih, CAR003_DegistirenSaat, CAR003_DegistirenKodu, CAR003_DegistirenSurum, CAR003_IptalDurumu, " +
                //      "CAR003_AsilEvrakTarihi, CAR003_otvdahilharic, CAR003_otvtutari, CAR003_KarsiHesapKodu, CAR003_Kod3, CAR003_Kod4, CAR003_Kod5, CAR003_Kod6, " +
                //      "CAR003_Kod7, CAR003_Kod8, CAR003_Kod9, CAR003_Kod10, CAR003_Kod11, CAR003_Kod12, CAR003_Tutar2, CAR003_Tarih2, CAR003_Aciklama2, " +
                //      "CAR003_EvrakSeriNo2, CAR003_DovizCinsi, CAR003_DovizTutari, CAR003_DovizKuru, CAR003_SenetCekBordroNo, CAR003_SenetCekPozisyonTipi, " +
                //      "CAR003_MuhasebelesmeSekli, CAR003_MuhasebeFisTarihi, CAR003_MuhasebeTipi, CAR003_MuhasebeFisNumarasi, CAR003_MuhasebeFisKodu, " +
                //      "CAR003_MuhasebeSiraNo, CAR003_MuhasebeHesapNo, CAR003_MuhasebeKarsiHeaspNo, CAR003_MuhasebeYevmiyeSekli, CAR003_IskontoTuru, " +
                //      "CAR003_EvrakSeriNo3, CAR003_MasrafMerkezi, CAR003_VadeFarkiTarihi, CAR003_VadeFarkiTutari, CAR003_FaizOrani, CAR003_Ulke, CAR003_VergiHesapNo, " +
                //      "CAR003_Unvani, CAR003_EvrakSayisi, CAR003_EvrakTipi, CAR003_MHSMaddeNo, CAR003_IBAN, CAR003_KKPOSTableRowID, CAR003_KKTaksitSayisi, " +
                //      "CAR003_KKTaksitNo, CAR003_KKKomisyonID) " +
                //      "VALUES ('" + AktarimHesapKodu.ToString() + "'," + TarihRakam + ", 6,'" + EvNo.ToString() + "', N'Faturası', 1,'" + YeniOran + "'," + TarihRakam + ", N'', N'', N'', 4, 0, 0, 1,1, " +
                //      "N'Y6023'," + TarihRakam + ", '" + Saatimiz + "','" + PCAdi.ToString() + "', N'5.1.10', N'Y6023'," + TarihRakam + ", '" + Saatimiz + "','" + PCAdi.ToString() + "', N'5.1.10', " +
                //      "1," + TarihRakam + ", 0, 0.00, N'', N'', N'', N'', N'', N'', N'', N'', N'', 0.00, 0.00, 0.00, 0, N'', '" + EvrakNo2.ToString() + "', N'', 0.00, 0.000000, N'', 0, 1, 0, 0, 0, N'', 0, N'', N'', 0, N'', N'', N'', " +
                //      "0, 0.00, 0.00, 0, N'', N'', 0, 21, 0, N'', 0, 0, 0, 0)";
                //            }

                //            CmdCar003 = new SqlCommand(CAR003insert, ConCar003);
                //            CmdCar003.CommandTimeout = 120;
                //            CmdCar003.ExecuteNonQuery();
                //        }

                //        #endregion
                //    }
                //    Car005Toplami = 0;
                //    Car005KdvTutari = 0;
                //}

                #endregion

                #region Irsaliye Kayıtlarını Car005 e Aktarıyoruz

                for (int i = 0; i <= ds.MalKabul.Rows.Count - 1; i++)
                {
                    CarEvrakNo = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                    string MalDurumSevk = ds.MalKabul.Rows[i]["MalDurum"].ToString();

                    for (int f = 0; f <= ds.MalKabul.Rows.Count - 1; f++)
                    {
                        CarEvrakNo2 = ds.MalKabul.Rows[f]["IrsaliyeNo"].ToString();

                        if (CarEvrakNo == CarEvrakNo2 && MalDurumSevk.ToString() == "Irsaliye")
                        {
                            Car005Toplami = Convert.ToDecimal(ds.MalKabul.Rows[f]["ToplamFiyat"].ToString()) + Car005Toplami;
                            Car005KdvTutari = Convert.ToDecimal(ds.MalKabul.Rows[f]["KdvTutari"].ToString()) + Car005KdvTutari;
                            i = f;
                        }
                    }

                    if (CarEvrakNo == CarEvrakNo2 && MalDurumSevk.ToString() == "Irsaliye")
                    {

                        #region Evrak Numarasını Kontrol Edip Ayarlıyoruz

                        string EvNo = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString().TrimStart();
                        EvrakNo2 = ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString().TrimStart();

                        if (EvNo.ToString().Length == 6)
                        {
                            EvNo = "  " + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                        }
                        else if (EvNo.ToString().Length == 5)
                        {
                            EvNo = "  0" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                        }
                        else if (EvNo.ToString().Length == 4)
                        {
                            EvNo = "  00" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                        }
                        else if (EvNo.ToString().Length == 3)
                        {
                            EvNo = "  000" + ds.MalKabul.Rows[i]["IrsaliyeNo"].ToString();
                        }

                        #endregion

                        string Car005Toplam = Convert.ToString(Car005Toplami);
                        string Car005KdvTut = Convert.ToString(Car005KdvTutari);

                        string AktarimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();
                        string AktarimToplam = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string AktarimHesapKodu = ds.MalKabul.Rows[i]["Hesap_Kodu"].ToString();
                        string Price = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string CarTotal = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string Stk002Total = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string Stk002BirimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();
                        string Stk002Miktar = ds.MalKabul.Rows[i]["Miktar"].ToString();
                        string Stk004Total = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string Stk004BirimMiktar = ds.MalKabul.Rows[i]["Miktar"].ToString();
                        string StkSonAlimFiyat = ds.MalKabul.Rows[i]["Fiyat"].ToString();

                        #region Fiyatları Ayarlıyoruz

                        Price = Price.Replace(".", ",");
                        double PFiyat = double.Parse(Price);
                        Price = PFiyat.ToString("0.0000");
                        //Price = Convert.ToString(PFiyat);
                        Price = Price.Replace(",", ".");

                        Stk002BirimFiyat = Stk002BirimFiyat.Replace(".", ",");
                        double Stk002BirimFiyat1 = double.Parse(Stk002BirimFiyat);
                        //Stk002BirimFiyat = Stk002BirimFiyat1.ToString("0.000000");
                        //Price = Convert.ToString(PFiyat);
                        Stk002BirimFiyat = Stk002BirimFiyat.Replace(",", ".");

                        Stk002Miktar = Stk002Miktar.Replace(".", ",");
                        double Stk002Miktar1 = double.Parse(Stk002Miktar);
                        //Stk002Miktar = Stk002Miktar1.ToString("0.0000");
                        //Price = Convert.ToString(PFiyat);
                        Stk002Miktar = Stk002Miktar.Replace(",", ".");

                        Stk002Total = Stk002Total.Replace(".", ",");
                        double Stk002Fiyat = double.Parse(Stk002Total);
                        Stk002Total = Stk002Fiyat.ToString("0.00");
                        //Price = Convert.ToString(PFiyat);
                        Stk002Total = Stk002Total.Replace(",", ".");

                        StkSonAlimFiyat = StkSonAlimFiyat.Replace(".", ",");
                        double StkAlimFiyat = double.Parse(StkSonAlimFiyat);
                        StkSonAlimFiyat = StkAlimFiyat.ToString("0.000000");
                        //Price = Convert.ToString(PFiyat);
                        StkSonAlimFiyat = StkSonAlimFiyat.Replace(",", ".");

                        CarTotal = CarTotal.Replace(".", ",");
                        double CarFiyat = double.Parse(CarTotal);
                        CarTotal = CarFiyat.ToString("0.00");
                        //Price = Convert.ToString(PFiyat);
                        CarTotal = CarTotal.Replace(",", ".");

                        Stk004Total = Stk004Total.Replace(".", ",");
                        double Stk004Fiyat = double.Parse(Stk004Total);
                        Stk004Total = Stk004Fiyat.ToString("0.00");
                        //Price = Convert.ToString(PFiyat);
                        Stk004Total = Stk004Total.Replace(",", ".");

                        Stk004BirimMiktar = Stk004BirimMiktar.Replace(".", ",");
                        double Stk004BirimMiktar1 = double.Parse(Stk004BirimMiktar);
                        //Stk004BirimMiktar = Stk004BirimMiktar1.ToString("0.0000");
                        //Price = Convert.ToString(PFiyat);
                        Stk004BirimMiktar = Stk004BirimMiktar.Replace(",", ".");

                        #endregion

                        #region Car005 İnsert Parametreleri

                        string SatirTipi = "";
                        string SatirAciklama = "";
                        string YeniTutar = "";
                        string YeniOran = "";
                        int SatirNo = 2;

                        for (int j = 0; j < 6; j++)
                        {

                            if (j == 0)
                            {
                                SatirTipi = "Z";
                                SatirAciklama = "";
                                YeniTutar = "0.00";
                                YeniOran = "0.00";

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 1)
                            {
                                SatirTipi = "T";
                                SatirAciklama = "MAL BEDELI";
                                YeniTutar = Stk002Total;
                                YeniOran = "0.00";

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                 "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 2)
                            {
                                SatirTipi = "K";
                                SatirAciklama = "KATMA DEGER VERGISI";
                                YeniTutar = Convert.ToString(Car005KdvTutari).Replace(",", ".");
                                YeniOran = ds.MalKabul.Rows[i]["KdvOrani"].ToString().Replace(",", ".");

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 3)
                            {
                                SatirTipi = "Z";
                                SatirAciklama = "";
                                YeniTutar = "0.00";
                                YeniOran = "0.00";

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
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

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 5)
                            {
                                SatirTipi = "Y";
                                SatirAciklama = "";
                                YeniTutar = "0.00";
                                YeniOran = "0.00";

                                CAR005insert = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (4," + TarihRakam + ",'" + EvNo.ToString() + "','A',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }

                            CmdCar005 = new SqlCommand(CAR005insert, ConCar005);
                            CmdCar005.CommandTimeout = 120;
                            CmdCar005.ExecuteNonQuery();
                            SatirNo++;

                        }

                        #endregion

                    }
                    Car005Toplami = 0;
                    Car005KdvTutari = 0;
                }

                #endregion

                CariKontrolDr.Dispose();
                CariKontrolDr.Close();

                SEQNoDr.Dispose();
                SEQNoDr.Close();

                ConMalKabulAkar.Dispose();
                ConSEQNo.Dispose();
                ConStk005.Dispose();
                ConCar005.Dispose();
                ConCar003.Dispose();
                ConCariKontrol.Dispose();
                ConCariAktar.Dispose();
                ConLinkKontrol.Dispose();
                ConStk004.Dispose();
                ConMrpStk.Dispose();
                ConStkEkle.Dispose();
                ConStkUpdate.Dispose();
                ConCariUpdate.Dispose();

                ConMalKabulAkar.Close();
                ConSEQNo.Close();
                ConStk005.Close();
                ConCar005.Close();
                ConCar003.Close();
                ConCariKontrol.Close();
                ConCariAktar.Close();
                ConLinkKontrol.Close();
                ConStk004.Close();
                ConMrpStk.Close();
                ConStkEkle.Close();
                ConStkUpdate.Close();
                ConCariUpdate.Close();

                MessageBox.Show("Aktarımı Tamamlanan Toplam Kayıt Sayısı : " + ds.MalKabul.Rows.Count);

                btnAktar.Enabled = true;
                btnArama.Enabled = true;
            }
        }

        #endregion

        #region Formu Kapatıyoruz

        private void Alimislemleri_FormClosing(object sender, FormClosingEventArgs e)
        {
            Alimislemleri alimislemi = new Alimislemleri();

            if (MalKabul != null)
            {
                MalKabul.Abort();
            }

            alimislemi.Close();
        }

        #endregion

    }
}
