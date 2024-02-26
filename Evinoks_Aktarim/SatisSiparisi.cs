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
    public partial class SatisSiparisi : Form
    {
        public SatisSiparisi()
        {
            InitializeComponent();
        }

        #region Bağlantı ve Tanımlar

        public AnaForm anaFrm
        {
            get;
            set;
        }

        Thread SSiparis;

        SqlConnection ConSiparis;
        SqlCommand CmdSiparis;
        SqlDataReader SiparisDr;
        //Devamı
        SqlConnection ConSiparis2;
        SqlCommand CmdSiparis2;
        SqlDataReader SiparisDr2;

        SqlConnection ConOlanSiparis;
        SqlCommand CmdOlanSiparis;
        SqlDataReader DrOlanSiparis;

        SqlConnection ConOlanSiparislerDeleteStk;
        SqlCommand CmdOlanSiparislerDeleteStk;

        SqlConnection ConOlanSiparislerDeleteCar;
        SqlCommand CmdOlanSiparislerDeleteCar;
        
        SqlConnection ConUpdateMrpSiparisDetY;
        SqlCommand cmdUpdateMrpSiparisDetY;
        SqlDataReader drUpdateMrpSiparisDetY;

        SqlConnection ConUpdateMrpSiparismstY;
        SqlCommand cmdUpdateMrpSiparismstY;
        SqlDataReader drUpdateMrpSiparismstY;

        SqlConnection ConUpdateMrpCariKart;
        SqlCommand CmdUpdateMrpCariKart;
        SqlDataReader DrUpdateMrpCariKart;

        SqlConnection ConUpdateCariKart;
        SqlCommand CmdUpdateCariKart;
        SqlDataReader DrUpdateCariKart;

        SqlConnection ConUpdateMrpStokKart;
        SqlCommand CmdUpdateMrpStokKart;
        SqlDataReader DrUpdateMrpStokKart;

        SqlConnection ConStkEkle;
        SqlCommand CmdStkEkle;
        SqlDataReader StkEkleDr;

        SqlConnection ConSAktar;
        SqlCommand CmdSAktar;
        SqlDataReader SAktarDr;

        SqlConnection ConSCar;
        SqlCommand CmdSCar;
        SqlDataReader SCarDr;

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

        string Cs1 = Properties.Settings.Default.Cs1;
        string Cs2 = Properties.Settings.Default.Cs2;

        #endregion

        #region Tanımlar

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
        string PCAdi;
        string SiraID;
        string Urunler;
        string MalKodu;
        string EvrakNo;
        string HesapKodu;
        DateTime SiparisTarihi;
        DateTime OnayTarihi;
        DateTime UretimTarihi;
        DateTime TeslimTarihi;
        string Model;
        string Birim;
        int Miktar;
        decimal Fiyat;
        decimal Toplam;
        decimal Kdv;
        decimal KdvOrani;
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
        DateTime SipUretimTarihi;
        DateTime SipTeslimTarihi;

        #endregion

        #region Form Load İşlemleri

        private void SatisSiparisi_Load(object sender, EventArgs e)
        {
            ConSiparis = new SqlConnection(Cs1);
            ConSiparis2 = new SqlConnection(Cs2);
            Sirket = anaFrm.tsLinkSirket.Text;
            MrpSirket = anaFrm.MrpSirket.ToString();
        }

        #endregion

        #region Arama Butonu

        private void btnArama_Click(object sender, EventArgs e)
        {
            ds.SatisSiparisi.Clear();

            SSiparis = new Thread(new ThreadStart(SSiparisSorgula));
            SSiparis.Priority = ThreadPriority.Highest;
            SSiparis.Start();
        }

        #endregion

        #region Satış Siparişi Arama Sorgumuz

        private void SSiparisSorgula()
        {
            ConSiparis = new SqlConnection(Cs1);
            ConSiparis2 = new SqlConnection(Cs2);
            ConOlanSiparislerDeleteStk = new SqlConnection(Cs1);
            ConOlanSiparislerDeleteCar = new SqlConnection(Cs1);

            if (ConOlanSiparislerDeleteCar.State == ConnectionState.Closed)
                ConOlanSiparislerDeleteCar.Open();

            if (ConOlanSiparislerDeleteStk.State == ConnectionState.Closed)
                ConOlanSiparislerDeleteStk.Open();

            btnArama.Enabled = false;
            btnAktar.Enabled = false;

            #region Düzenlenecek Siparişleri Alıyoruz

            ConOlanSiparis = new SqlConnection(Cs1);

            if (ConOlanSiparis.State == ConnectionState.Closed)
                ConOlanSiparis.Open();

            string olanSiparisler = "SELECT Siparis.sn AS SiparisID ,DETAY.sira AS SiraID,oper_urn AS Urunler,cusven_sku AS Mal_Kodu, " +
                                    "STK002_EvrakSeriNo AS Evrak_No,  " +
                                    "(CASE  SUBSTRING(cus_code, 1, 1) "+
                                    "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) "+
                                    "ELSE cus_code END) AS Hesap_Kodu,sprs_trh AS Siparis_Tarihi,  " +
                                    "onay_trh AS Onay_Tarihi,uret_trh AS Uretim_Tarihi,  " +
                                    "Siparis.teslim_trh AS Teslim_Tarihi,model_type AS Model,LEFT(um_code,2) AS Birim,um_qty AS Miktar, " +
                                    "Convert(DECIMAL(23,2),fiyat) AS Fiyat, "+
                                    "Convert(DECIMAL(19,2),um_qty)*Convert(DECIMAL(23,2),fiyat) AS Toplam,  " +
                                    "Convert(DECIMAL(21,2),Convert(DECIMAL(19,2),um_qty)*Convert(DECIMAL(23,2),fiyat)*ISNULL(tax_type,18)/100) AS Kdv,  " +
                                    "uret_date AS Sip_Uretim_Tarihi,DETAY.teslim_trh AS Sip_Teslim_Tarihi  " +
                                    "FROM " + MrpSirket.ToString() + ".dbo.siparis_det_y AS DETAY " +
                                    "INNER JOIN " + MrpSirket.ToString() + ".dbo.siparis_mst_y AS Siparis  ON Siparis.sn=DETAY.sn  " +
                                    "INNER JOIN YNS" + Sirket.ToString() + ".STK002 ON STK002_EvrakSeriNo2=Siparis.sn " +
                                    "WHERE DETAY.checkok = 0 AND Siparis.checkok = 0 AND STK002_EvrakSeriNo2=Siparis.sn AND " +
                                    "CONVERT(INT,STK002_TeslimMiktari) = 0 AND cusven_sku=STK002_MalKodu AND  " +
                                    "REPLACE(REPLACE(parti_no,'-',''),' ','') IN  " +
                                    "(SELECT RIGHT(STK002_EvrakSeriNo,LEN(REPLACE(REPLACE(parti_no,'-',''),' ','')))  " +
                                    "FROM YNS" + Sirket.ToString() + ".STK002  " +
                                    "GROUP BY STK002_EvrakSeriNo)";

            CmdOlanSiparis = new SqlCommand(olanSiparisler, ConOlanSiparis);
            CmdOlanSiparis.CommandTimeout = 120;
            DrOlanSiparis = CmdOlanSiparis.ExecuteReader(CommandBehavior.CloseConnection);
            ds.SatisSiparisiOlanlar.TableName.Replace("YNS00033", "YNS" + Sirket.ToString());

            while (DrOlanSiparis.Read())
            {
                Thread.Sleep(50);

                ID = DrOlanSiparis["SiparisID"].ToString();
                SiraID = DrOlanSiparis["SiraID"].ToString();
                Urunler = DrOlanSiparis["Urunler"].ToString();
                MalKodu = DrOlanSiparis["Mal_Kodu"].ToString();
                EvrakNo = DrOlanSiparis["Evrak_No"].ToString();
                EvrakNo = EvrakNo.Replace(" ", "");
                HesapKodu = DrOlanSiparis["Hesap_Kodu"].ToString();
                SiparisTarihi = Convert.ToDateTime(DrOlanSiparis["Siparis_Tarihi"].ToString());
                OnayTarihi = Convert.ToDateTime(DrOlanSiparis["Onay_Tarihi"].ToString());
                UretimTarihi = Convert.ToDateTime(DrOlanSiparis["Uretim_Tarihi"].ToString());
                TeslimTarihi = Convert.ToDateTime(DrOlanSiparis["Teslim_Tarihi"].ToString());

                Model = DrOlanSiparis["Model"].ToString();
                Birim = DrOlanSiparis["Birim"].ToString();
                Miktar = Convert.ToInt32(DrOlanSiparis["Miktar"].ToString());

                string Price = DrOlanSiparis["Fiyat"].ToString();
                string PToplam = DrOlanSiparis["Toplam"].ToString();
                string PKdv = DrOlanSiparis["Kdv"].ToString();

                Price = Price.Replace(".", ",");
                double PFiyat = double.Parse(Price);
                Price = PFiyat.ToString("0.000000");
                //Price = Convert.ToString(PFiyat);
                Price = Price.Replace(".", ",");

                PToplam = PToplam.Replace(".", ",");
                double PFiyatToplam = double.Parse(PToplam);
                PToplam = PFiyatToplam.ToString("0.00");
                //Price = Convert.ToString(PFiyat);
                PToplam = PToplam.Replace(".", ",");

                PKdv = PKdv.Replace(".", ",");
                double PKdvGelen = double.Parse(PKdv);
                PKdv = PKdvGelen.ToString("0.00");
                //Price = Convert.ToString(PFiyat);
                PKdv = PKdv.Replace(".", ",");

                Fiyat = Convert.ToDecimal(DrOlanSiparis["Fiyat"].ToString());
                Toplam = Convert.ToDecimal(DrOlanSiparis["Toplam"].ToString());
                Kdv = Convert.ToDecimal(DrOlanSiparis["Kdv"].ToString());
                SipUretimTarihi = Convert.ToDateTime(DrOlanSiparis["Sip_Uretim_Tarihi"].ToString());
                SipTeslimTarihi = Convert.ToDateTime(DrOlanSiparis["Sip_Teslim_Tarihi"].ToString());

                //ds.SatisSiparisiOlanlar.AddSatisSiparisiOlanlarRow(Convert.ToInt16(ID), Convert.ToInt32(SiraID), Urunler, MalKodu, EvrakNo, HesapKodu, SiparisTarihi, OnayTarihi, UretimTarihi, TeslimTarihi, Model, Birim, Convert.ToInt16(Miktar), Convert.ToDecimal(Price), Convert.ToDecimal(PToplam), SipUretimTarihi, SipTeslimTarihi);
                ds.SatisSiparisiOlanlar.AddSatisSiparisiOlanlarRow(Convert.ToInt16(ID), Convert.ToInt32(SiraID), Urunler, MalKodu, EvrakNo, HesapKodu, SiparisTarihi, OnayTarihi, UretimTarihi, TeslimTarihi, Model, Birim, Convert.ToInt16(Miktar), Convert.ToDecimal(Price),Convert.ToDecimal(PToplam), Convert.ToDecimal(PKdv), SipUretimTarihi, SipTeslimTarihi);
            }

            ConOlanSiparis.Dispose();
            ConOlanSiparis.Close();
            CmdOlanSiparis.Dispose();
            DrOlanSiparis.Dispose();
            DrOlanSiparis.Close();

            #endregion

            #region Olan Siparişleri Önce Siliyoruz

            foreach (DataRow drOlanSiparislerSil in ds.SatisSiparisiOlanlar.Rows)
            {
                string DeleteSorguStk = "DELETE FROM YNS" + Sirket.ToString() + ".STK002 " +
                    "WHERE RIGHT(STK002_EvrakSeriNo,LEN('" + drOlanSiparislerSil["Evrak_No"].ToString() + "'))='" + drOlanSiparislerSil["Evrak_No"].ToString() + "'" +
                    " AND STK002_EvrakSeriNo2='" + drOlanSiparislerSil["SiparisID"].ToString() + "'";

                CmdOlanSiparislerDeleteStk = new SqlCommand(DeleteSorguStk, ConOlanSiparislerDeleteStk);
                CmdOlanSiparislerDeleteStk.ExecuteNonQuery();

                string DeleteSorguCar = "DELETE  FROM YNS" + Sirket.ToString() + ".CAR005 WHERE RIGHT(CAR005_FaturaNo,LEN('" + drOlanSiparislerSil["Evrak_No"].ToString() + "')) = '" + drOlanSiparislerSil["Evrak_No"].ToString() + "'";

                CmdOlanSiparislerDeleteCar = new SqlCommand(DeleteSorguCar, ConOlanSiparislerDeleteCar);
                CmdOlanSiparislerDeleteCar.ExecuteNonQuery();
            }

            #endregion

            grdSatisSiparisi.DataSource = null;
            grdSatisSiparisi.DataSource = ds.SatisSiparisi;

            Sorgu = "Select Siparis.sn AS SiparisID ,DETAY.sira AS SiraID,oper_urn AS Urunler,cusven_sku AS Mal_Kodu, " +
                    "Replace(Replace(parti_no,'-',''),' ','') AS Evrak_No, " +
                    "(CASE  SUBSTRING(cus_code, 1, 1) " +
                    "WHEN 'M' THEN SUBSTRING(cus_code,2,LEN(cus_code)-1) " +
                    "ELSE cus_code END) AS Hesap_Kodu,sprs_trh AS Siparis_Tarihi, " +
                    "onay_trh AS Onay_Tarihi,uret_trh AS Uretim_Tarihi, " +
                    "Siparis.teslim_trh AS Teslim_Tarihi,model_type AS Model,LEFT(um_code,2) AS Birim,um_qty AS Miktar, " +
                    "Convert(DECIMAL(23,2),fiyat) AS Fiyat, " +
                    "Convert(DECIMAL(19,2),um_qty)*Convert(DECIMAL(23,2),fiyat) AS Toplam,  " +
                    "Convert(DECIMAL(21,2),Convert(DECIMAL(19,2),um_qty)*Convert(DECIMAL(23,2),fiyat)*ISNULL(tax_type,18)/100) AS Kdv, " +
                    "Convert(DECIMAL(21,2),ISNULL(tax_type,18)) AS Oran, " +
                    "uret_date AS Sip_Uretim_Tarihi,DETAY.teslim_trh AS Sip_Teslim_Tarihi " +
                    "From " + MrpSirket.ToString() + ".dbo.siparis_det_y AS DETAY INNER JOIN " + MrpSirket.ToString() + ".dbo.siparis_mst_y AS Siparis  " +
                    "ON Siparis.sn=DETAY.sn " +
                    "Where DETAY.checkok = 0 AND Siparis.checkok = 0 AND Replace(Replace(parti_no,'-',''),' ','') Not In " +
                    "(Select RIGHT(STK002_EvrakSeriNo,len(Replace(Replace(parti_no,'-',''),' ',''))) From YNS" + Sirket.ToString() + ".STK002 " +
                    "WHERE STK002_MalKodu=cusven_sku AND " +
                    "RIGHT(STK002_EvrakSeriNo,len(Replace(Replace(parti_no,'-',''),' ','')))=Replace(Replace(parti_no,'-',''),' ','') " +
                    "Group By STK002_EvrakSeriNo)";

            if (ConSiparis.State == ConnectionState.Closed)
                ConSiparis.Open();

            if (ConSiparis2.State == ConnectionState.Closed)
                ConSiparis2.Open();

            CmdSiparis = new SqlCommand(Sorgu, ConSiparis);
            CmdSiparis.CommandTimeout = 120;
            SiparisDr = CmdSiparis.ExecuteReader(CommandBehavior.CloseConnection);
            ds.SatisSiparisi.TableName.Replace("YNS00033", "YNS" + Sirket.ToString());

            while (SiparisDr.Read())
            {
                Thread.Sleep(50);

                ID = SiparisDr["SiparisID"].ToString();
                SiraID = SiparisDr["SiraID"].ToString();
                Urunler = SiparisDr["Urunler"].ToString();
                MalKodu = SiparisDr["Mal_Kodu"].ToString();
                EvrakNo = SiparisDr["Evrak_No"].ToString();
                EvrakNo = EvrakNo.Replace(" ", "");
                HesapKodu = SiparisDr["Hesap_Kodu"].ToString();
                SiparisTarihi = Convert.ToDateTime(SiparisDr["Siparis_Tarihi"].ToString());
                OnayTarihi = Convert.ToDateTime(SiparisDr["Onay_Tarihi"].ToString());
                UretimTarihi = Convert.ToDateTime(SiparisDr["Uretim_Tarihi"].ToString());
                TeslimTarihi = Convert.ToDateTime(SiparisDr["Teslim_Tarihi"].ToString());
                
                Model = SiparisDr["Model"].ToString();
                Birim = SiparisDr["Birim"].ToString();
                Miktar = Convert.ToInt32(SiparisDr["Miktar"].ToString());

                string Price = SiparisDr["Fiyat"].ToString();
                string PToplam = SiparisDr["Toplam"].ToString();
                string PKdv = SiparisDr["kdv"].ToString();

                Price = Price.Replace(".", ",");
                double PFiyat = double.Parse(Price);
                Price = PFiyat.ToString("0.000000");
                //Price = Convert.ToString(PFiyat);
                Price = Price.Replace(".", ",");

                PToplam = PToplam.Replace(".", ",");
                double PFiyatToplam = double.Parse(PToplam);
                PToplam = PFiyatToplam.ToString("0.00");
                //Price = Convert.ToString(PFiyat);
                PToplam = PToplam.Replace(".", ",");

                PKdv = PKdv.Replace(".", ",");
                double PKdvGelen = double.Parse(PKdv);
                PKdv = PKdvGelen.ToString("0.00");
                //Price = Convert.ToString(PFiyat);
                PKdv = PKdv.Replace(".", ",");

                Fiyat = Convert.ToDecimal(SiparisDr["Fiyat"].ToString());
                Toplam = Convert.ToDecimal(SiparisDr["Toplam"].ToString());
                Kdv = Convert.ToDecimal(SiparisDr["Kdv"].ToString());
                KdvOrani = Convert.ToDecimal(SiparisDr["Oran"].ToString());

                SipUretimTarihi = Convert.ToDateTime(SiparisDr["Sip_Uretim_Tarihi"].ToString());
                SipTeslimTarihi = Convert.ToDateTime(SiparisDr["Sip_Teslim_Tarihi"].ToString());

                ds.SatisSiparisi.AddSatisSiparisiRow(Convert.ToInt16(ID), Convert.ToInt32(SiraID), Urunler, MalKodu, EvrakNo, HesapKodu, SiparisTarihi, OnayTarihi, UretimTarihi, TeslimTarihi, Model, Birim, Convert.ToInt16(Miktar), Convert.ToDecimal(Price), Convert.ToDecimal(PToplam),Convert.ToDecimal(PKdv),KdvOrani, SipUretimTarihi, SipTeslimTarihi);
            }

            ConSiparis.Dispose();
            ConSiparis.Close();
            ConSiparis2.Dispose();
            ConSiparis2.Close();
            CmdSiparis.Dispose();
            SiparisDr.Dispose();
            SiparisDr.Close();

            MessageBox.Show("Arama İşlemi Tamamlanmıştır.\nToplam Bulunan Kayıt Sipariş Sayısı : " + Convert.ToString(ds.SatisSiparisi.Rows.Count));

            btnArama.Enabled = true;
            btnAktar.Enabled = true;
        }

        #endregion

        #region Formu Kapatıyoruz

        private void SatisSiparisi_FormClosing(object sender, FormClosingEventArgs e)
        {
            SatisSiparisi sp = new SatisSiparisi();

            if (SSiparis != null)
            {
                SSiparis.Abort();
                sp.Close();
            }
        }

        #endregion

        #region Aktarım Butonu

        private void btnAktar_Click(object sender, EventArgs e)
        {
            SSiparis = new Thread(new ThreadStart(SSiparisAktar));
            SSiparis.Priority = ThreadPriority.Highest;
            SSiparis.Start();
        }

        #endregion

        #region Satış Siparişini Aktarıyoruz

        private void SSiparisAktar()
        {
            //try
            //{
                if (MessageBox.Show("Bulunan Satış Siparişi Kayıtlarını Linke Aktarmak İstediğinize Emin Misiniz?", "Uyarı...",
     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnAktar.Enabled = false;
                    btnArama.Enabled = false;

                    ConSAktar = new SqlConnection(Cs1);
                    ConSEQNo = new SqlConnection(Cs1);
                    ConSCar = new SqlConnection(Cs1);
                    ConCariKontrol = new SqlConnection(Cs2);
                    ConCariAktar = new SqlConnection(Cs1);
                    ConLinkKontrol = new SqlConnection(Cs1);
                    ConStk004 = new SqlConnection(Cs1);
                    ConMrpStk = new SqlConnection(Cs2);
                    ConStkUpdate = new SqlConnection(Cs1);
                    ConStkEkle = new SqlConnection(Cs1);
                    ConUpdateMrpSiparisDetY = new SqlConnection(Cs2);
                    ConUpdateMrpSiparismstY = new SqlConnection(Cs2);
                    ConUpdateMrpCariKart = new SqlConnection(Cs2);
                    ConUpdateMrpStokKart = new SqlConnection(Cs2);
                    ConUpdateCariKart = new SqlConnection(Cs1);


                    #region Bağlantıları Açıyoruz

                    if (ConUpdateCariKart.State == ConnectionState.Closed)
                        ConUpdateCariKart.Open();

                    if (ConUpdateMrpStokKart.State == ConnectionState.Closed)
                        ConUpdateMrpStokKart.Open();

                    if (ConUpdateMrpCariKart.State == ConnectionState.Closed)
                        ConUpdateMrpCariKart.Open();

                    if (ConUpdateMrpSiparismstY.State == ConnectionState.Closed)
                        ConUpdateMrpSiparismstY.Open();

                    if (ConUpdateMrpSiparisDetY.State == ConnectionState.Closed)
                        ConUpdateMrpSiparisDetY.Open();

                    if (ConStkUpdate.State == ConnectionState.Closed)
                        ConStkUpdate.Open();

                    if (ConSAktar.State == ConnectionState.Closed)
                        ConSAktar.Open();

                    if (ConSEQNo.State == ConnectionState.Closed)
                        ConSEQNo.Open();

                    if (ConSCar.State == ConnectionState.Closed)
                        ConSCar.Open();

                    if (ConStkEkle.State == ConnectionState.Closed)
                        ConStkEkle.Open();

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

                    #endregion

                    #region SEQNo yu Alıyoruz

                    string SeqNoSorgu = "SELECT (CASE WHEN MAX(STK002_SEQNo)<5000000 THEN 5000000 "+
                                        "ELSE MAX(STK002_SEQNo)+1 END)AS SEQNo FROM YNS" + Sirket.ToString() + ".STK002";
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

                    #region STK002 Sorgu

                    string Stk02Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".STK002(STK002_MalKodu, STK002_IslemTarihi, STK002_GC, STK002_CariHesapKodu, STK002_EvrakSeriNo, STK002_Miktari, STK002_BirimFiyati, " +
                              "STK002_Tutari, STK002_Iskonto, STK002_KDVTutari, STK002_IslemTipi, STK002_Kod1, STK002_Kod2, STK002_IrsaliyeNo, STK002_TeslimMiktari,  " +
                              "STK002_SipDurumu, STK002_Bos, STK002_KDVDurumu, STK002_TeslimTarihi, STK002_ParaBirimi, STK002_SEQNo, STK002_GirenKaynak, STK002_GirenTarih,  " +
                              "STK002_GirenSaat, STK002_GirenKodu, STK002_GirenSurum, STK002_DegistirenKaynak, STK002_DegistirenTarih, STK002_DegistirenSaat, STK002_DegistirenKodu,  " +
                              "STK002_DegistirenSurum, STK002_IptalDurumu, STK002_AsilEvrakTarihi, STK002_Miktar2, STK002_Tutar2, STK002_KalemIskontoOrani1,  " +
                              "STK002_KalemIskontoOrani2, STK002_KalemIskontoOrani3, STK002_KalemIskontoOrani4, STK002_KalemIskontoOrani5, STK002_KalemIskontoTutari1,  " +
                              "STK002_KalemIskontoTutari2, STK002_KalemIskontoTutari3, STK002_KalemIskontoTutari4, STK002_KalemIskontoTutari5, STK002_DovizCinsi, STK002_DovizTutari,  " +
                              "STK002_DovizKuru, STK002_Aciklama1, STK002_Aciklama2, STK002_Depo, STK002_Kod3, STK002_Kod4, STK002_Kod5, STK002_Kod6, STK002_Kod7,  " +
                              "STK002_Kod8, STK002_Kod9, STK002_Kod10, STK002_Kod11, STK002_Kod12, STK002_Vasita, STK002_MalSeriNo, STK002_VadeTarihi, STK002_Masraf,  " +
                              "STK002_EvrakSeriNo2, STK002_Kod9Flag, STK002_Kod10Flag, STK002_KDVOranFlag, STK002_TeslimCHKodu) " +
                              "VALUES     (@MalKodu,@IslemTarihi,@GC,@CariHesapKodu,@EvrakSeriNo,@Miktari,@BirimFiyati,@Tutari,@Iskonto,@KDVTutari,@IslemTipi,@Kod1,@Kod2,@IrsaliyeNo, " +
                              "@TeslimMiktari,@SipDurumu,@Bos,@KdvDurumu,@TeslimTarihi,@ParaBirimi,@SEQNo,@GirenKaynak,@GirenTarih,@GirenSaat,@GirenKodu,@GirenSurum,@DegistirenKaynak, " +
                              "@DegistirenTarih,@DegistirenSaat,@DegistirenKodu,@DegistirenSurum,@IptalDurumu,@AsilEvrakTarihi,@Miktar2,@Tutar2,@KalemIskontoOrani1,@KalemIskontoOrani2, " +
                              "@KalemIskontoOrani3,@KalemIskontoOrani4,@KalemIskontoOrani5,@KalemIskontoTutari1,@KalemIskontoTutari2,@KalemIskontoTutari3,@KalemIskontoTutari4,@KalemIskontoTutari5, " +
                              "@DovizCinsi,@DovizTutari,@DovizKuru,@Aciklama1,@Aciklama2,@Depo,@Kod3,@Kod4,@Kod5,@Kod6,@Kod7,@Kod8,@Kod9,@Kod10,@Kod11,@Kod12,@Vasita,@MalSeriNo, " +
                              "@VadeTarihi,@Masraf,@EvrakSeriNo2,@Kod9Flag,@Kod10Flag,@KDVOranFlag,@TeslimCHKodu)";

                    #endregion

                    #region CAR005 Sorgu

                    string Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                          "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                          "VALUES (@Secenek,@FaturaTarihi,@FaturaNo,@BA,@CariIslemTipi,@SatirTipi,@SatirNo,@SatirKodu,@Filler,@SatirAciklama,@CHKodu,@Tutar,@Oran)";

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
                          "0, 0.0000, 0.00,@STK004_GirisMiktari,@STK004_GirisTutari, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0, 0.0000, N'Y4004',@STK004_GirenTarih, " +
                          "N'100323',@STK004_GirenKodu, N'5.1.10', N'Y6019',@STK004_DegistirenTarih, N'100401',@STK004_DegistirenKodu, N'5.1.10', N'', N'', N'', N'', N'', N'', N'', N'', N'', " +
                          "0.000, 0.000, N'', N'', N'', N'', 1, 0.00000, N'', 0, N'', N'', 0.00, 0.00, 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.000000, N'', 0.0000, " +
                          "@STK004_SatisSiparisi, 0.0000, 0, 0, 0.0000, 0, N'', N'', N'', N'', N'', 0, 0.000000, 0, 0, 1, 0.0000, N'', N'', N'', N'', 0, " +
                          "N'', 0.000000, N'', 0.000000, 0, N'', " +
                          "0.000000, N'', 0.000000, N'', N'', 0, 0, 0, 0, 0, 0, 0, 0.00, 0.00, 0.00, 0.00, 0.00, N'', N'', N'', 0.00, 0.00, 0.00, 0.0000, 0.0000, 0.000, 0.0000, 0.000, N'', 0.0000, 0.00, " +
                          "0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, 0, 0, 0, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                          "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, " +
                          "0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, " +
                          "0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0.0000, 0.00, 0.0000, 0.00, 0.00, 0.0000, 0.00, 0.00, 0.000000, N'', 0, N'', 0, " +
                          "N'', 0, N'', 0, N'', N'', 0, 0.000, N'', N'', N'', N'', N'', N'', N'', 0.0000, 0.00, 0.00, 0, 0, 0, 0.00, N'', 0.000000, 0, 0, 0.000000, 0, 0, 0.000000, 0, 0, 0, 0, 0, 1, 0, 0.0000, N'', " +
                          "N'', N'', N'')";

                    #endregion

                    #region Aktarımı Yapıyoruz

                    MessageBox.Show("Aktarım İşlemi Başlamıştır.Lütfen İşlem Bitmeden Programı Kapatmayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (DataRow dr in ds.SatisSiparisi.Rows)
                    {
                        Thread.Sleep(50);

                        SEQNo++;

                        #region Sipariş Tarihini Convert Ediyoruz

                        DateTime objSiparisTarihi = new DateTime();
                        objSiparisTarihi = Convert.ToDateTime(dr["Siparis_Tarihi"].ToString());
                        string strSiparisTarihi;
                        SPTarih = 0;
                        SPTarih = objSiparisTarihi.ToOADate();
                        strSiparisTarihi = SPTarih.ToString().Substring(0, 5);
                        SPTarihRakam = Convert.ToInt32(strSiparisTarihi);

                        #endregion

                        #region Teslim Tarihini Convert Ediyoruz

                        DateTime objSTTarihi = new DateTime();
                        objSTTarihi = Convert.ToDateTime(dr["Teslim_Tarihi"].ToString());
                        string strSTTarihi;
                        STTarih = 0;
                        STTarih = objSTTarihi.ToOADate();
                        strSTTarihi = STTarih.ToString().Substring(0, 5);
                        STTarihRakam = Convert.ToInt32(strSTTarihi);

                        #endregion

                        #region Fatura Tarihini Convert Ediyoruz

                        DateTime objFaturaTarihi = new DateTime();
                        objFaturaTarihi = Convert.ToDateTime(dr["Onay_Tarihi"].ToString());
                        string strFaturaTarihi;
                        CarFaturaTarih = 0;
                        CarFaturaTarih = objFaturaTarihi.ToOADate();
                        strFaturaTarihi = CarFaturaTarih.ToString().Substring(0, 5);
                        CarFaturaTarihRakam = Convert.ToInt32(strFaturaTarihi);

                        #endregion

                        #region Evrak Numarasını Kontrol Edip Ayarlıyoruz

                        string EvNo = dr["Evrak_No"].ToString().TrimStart();

                        if (EvNo.ToString().Length == 6)
                        {
                            EvNo = "  " + dr["Evrak_No"].ToString();
                        }
                        else if (EvNo.ToString().Length == 5)
                        {
                            EvNo = "  0" + dr["Evrak_No"].ToString();
                        }
                        else if (EvNo.ToString().Length == 4)
                        {
                            EvNo = "  00" + dr["Evrak_No"].ToString();
                        }

                        #endregion

                        string AktarimFiyat = dr["Fiyat"].ToString();
                        string AktarimToplam = dr["Toplam"].ToString();
                        string AktarimHesapKodu = dr["Hesap_Kodu"].ToString();
                        string Price = dr["Toplam"].ToString();
                        string CarTotal = dr["Toplam"].ToString();
                        string Stk002Total = dr["Toplam"].ToString();
                        string Stk002BirimFiyat = dr["Fiyat"].ToString();
                        string Stk002Miktar = dr["Miktar"].ToString();
                        string KdvStkTutari = dr["Kdv"].ToString().Replace(",", ".");
                        SiparisID = dr["SiparisID"].ToString();

                        #region Fiyatları Ayarlarıyoruz

                        Price = Price.Replace(".", ",");
                        double PFiyat = double.Parse(Price);
                        Price = PFiyat.ToString("0.0000");
                        //Price = Convert.ToString(PFiyat);
                        Price = Price.Replace(",", ".");

                        Stk002BirimFiyat = Stk002BirimFiyat.Replace(".", ",");
                        double Stk002BirimFiyat1 = double.Parse(Stk002BirimFiyat);
                        Stk002BirimFiyat = Stk002BirimFiyat1.ToString("0.000000");
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

                        CarTotal = CarTotal.Replace(".", ",");
                        double CarFiyat = double.Parse(CarTotal);
                        CarTotal = CarFiyat.ToString("0.00");
                        //Price = Convert.ToString(PFiyat);
                        CarTotal = CarTotal.Replace(",", ".");

                        #endregion

                        #region Stk002 İnsert Parametreleri

                        Stk02Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".STK002(STK002_MalKodu, STK002_IslemTarihi, STK002_GC, STK002_CariHesapKodu, STK002_EvrakSeriNo, STK002_Miktari, STK002_BirimFiyati, " +
                              "STK002_Tutari, STK002_Iskonto, STK002_KDVTutari, STK002_IslemTipi, STK002_Kod1, STK002_Kod2, STK002_IrsaliyeNo, STK002_TeslimMiktari,  " +
                              "STK002_SipDurumu, STK002_Bos, STK002_KDVDurumu, STK002_TeslimTarihi, STK002_ParaBirimi, STK002_SEQNo, STK002_GirenKaynak, STK002_GirenTarih,  " +
                              "STK002_GirenSaat, STK002_GirenKodu, STK002_GirenSurum, STK002_DegistirenKaynak, STK002_DegistirenTarih, STK002_DegistirenSaat, STK002_DegistirenKodu,  " +
                              "STK002_DegistirenSurum, STK002_IptalDurumu, STK002_AsilEvrakTarihi, STK002_Miktar2, STK002_Tutar2, STK002_KalemIskontoOrani1,  " +
                              "STK002_KalemIskontoOrani2, STK002_KalemIskontoOrani3, STK002_KalemIskontoOrani4, STK002_KalemIskontoOrani5, STK002_KalemIskontoTutari1,  " +
                              "STK002_KalemIskontoTutari2, STK002_KalemIskontoTutari3, STK002_KalemIskontoTutari4, STK002_KalemIskontoTutari5, STK002_DovizCinsi, STK002_DovizTutari,  " +
                              "STK002_DovizKuru, STK002_Aciklama1, STK002_Aciklama2, STK002_Depo, STK002_Kod3, STK002_Kod4, STK002_Kod5, STK002_Kod6, STK002_Kod7,  " +
                              "STK002_Kod8, STK002_Kod9, STK002_Kod10, STK002_Kod11, STK002_Kod12, STK002_Vasita, STK002_MalSeriNo, STK002_VadeTarihi, STK002_Masraf,  " +
                              "STK002_EvrakSeriNo2, STK002_Kod9Flag, STK002_Kod10Flag, STK002_KDVOranFlag, STK002_TeslimCHKodu) " +
                              "VALUES     ('" + dr["Mal_Kodu"].ToString() + "'," + SPTarihRakam + ",1,'" + AktarimHesapKodu.ToString() + "','" + EvNo.ToString() + "','" + Stk002Miktar + "','" + Stk002BirimFiyat + "','" + Stk002Total + "','0.00','" + KdvStkTutari.ToString() + "',2,'','','', " +
                              "'0.0000',0,'',0," + STTarihRakam + ",1,1,'Y6015'," + TarihRakam + ",'095916','" + PCAdi.ToString() + "','6.1.00','Y6015', " +
                              "" + TarihRakam + ",'100431','" + PCAdi.ToString() + "','6.1.00',1," + TarihRakam + ",'0.000','0.000','0.00','0.00', " +
                              "'0.00','0.00','0.00','0.00','0.00','0.00','0.00','0.00', " +
                              "'','0.00','0.00000','','','','','','','','','','','','0.00','0.00','','', " +
                              "" + TarihRakam + ",'0.00','" + SiparisID.ToString() + "',0,0,4,'" + dr["Hesap_Kodu"].ToString() + "')";

                        CmdSAktar = new SqlCommand(Stk02Sorgu, ConSAktar);
                        CmdSAktar.CommandTimeout = 120;
                        CmdSAktar.ExecuteNonQuery();

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

                        if (!string.IsNullOrEmpty(Telefon))
                        {
                            Telefon = Telefon.Replace(" ", "");

                            if (Telefon.ToString().Length > 12)
                            {
                                Telefon = Telefon.ToString().Substring(0, 12);
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
                               "Where STK004_MalKodu='" + dr["Mal_Kodu"].ToString() + "' ";

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
                                                 "Where sku='" + dr["Mal_Kodu"].ToString() + "'";

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

                            CmdStkEkle = new SqlCommand(STK004inset, ConStkEkle);

                            if (MrpStokAciklama.ToString().Length > 50)
                            {
                                MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                            }

                            CmdStkEkle.Parameters.Add("@STK004_MalKodu", SqlDbType.NVarChar).Value = MrpStokKodu.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_GirisMiktari", SqlDbType.Float).Value = "0,0000";
                            CmdStkEkle.Parameters.Add("@STK004_GirisTutari", SqlDbType.Float).Value = "0,00";
                            CmdStkEkle.Parameters.Add("@STK004_GirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_GirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStkEkle.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PCAdi.ToString();
                            CmdStkEkle.Parameters.Add("@STK004_SatisSiparisi", SqlDbType.Float).Value = Convert.ToDouble(Stk002Miktar);

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
                            string MrpStkSorgu = "Select sku AS MalKodu,sku_name AS MalAdi From " + MrpSirket.ToString() + ".dbo.ab_sku_def " +
                            "Where sku='" + dr["Mal_Kodu"].ToString() + "'";

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

                            if (MrpStokAciklama.ToString().Length > 50)
                            {
                                MrpStokAciklama = MrpStokAciklama.ToString().Substring(0, 50);
                            }

                            double numberSatisSiparisi;
                            bool Stk004SatisSİparisi = double.TryParse(Stk002Miktar.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out numberSatisSiparisi);

                            string Stk004Update = "UPDATE YNS" + Sirket.ToString() + ".STK004 SET STK004_Aciklama = @STK004_Aciklama, " +
                            "STK004_DegistirenTarih = @STK004_DegistirenTarih, " +
                            "STK004_DegistirenKodu = @STK004_DegistirenKodu, STK004_SatisSiparisi += @STK004_SatisSiparisi " +
                            "WHERE (STK004_MalKodu = '" + dr["Mal_Kodu"].ToString() + "')";

                            CmdStkUpdate = new SqlCommand(Stk004Update, ConStkUpdate);

                            CmdStkUpdate.Parameters.Add("@STK004_Aciklama", SqlDbType.NVarChar).Value = MrpStokAciklama.ToString();
                            CmdStkUpdate.Parameters.Add("@STK004_DegistirenTarih", SqlDbType.Int).Value = TarihRakam;
                            CmdStkUpdate.Parameters.Add("@STK004_DegistirenKodu", SqlDbType.NVarChar).Value = PcAdi2.ToString();
                            CmdStkUpdate.Parameters.Add("@STK004_SatisSiparisi", SqlDbType.Decimal).Value = numberSatisSiparisi.ToString().Replace(",", ".");

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

                        #endregion

                        LinkCariDurumu = 0;
                        LinkCariID = "0";
                        Stk004Durum = 0;
                    }

                    #endregion

                    #region Car005 e Aktarım Yapıyoruz

                    string CarEvrakNo = "";
                    string CarEvrakNo2 = "";
                    decimal Car005Toplami = 0;
                    decimal Car005KdvTutari = 0;
                    
                    for (int i = 0; i <= ds.SatisSiparisi.Rows.Count-1; i++)
                    {
                        CarEvrakNo = ds.SatisSiparisi.Rows[i]["Evrak_No"].ToString();

                        for (int f = 0; f <= ds.SatisSiparisi.Rows.Count-1; f++)
                        {
                            CarEvrakNo2 = ds.SatisSiparisi.Rows[f]["Evrak_No"].ToString();

                            if (CarEvrakNo == CarEvrakNo2)
                            {
                                Car005Toplami = Convert.ToDecimal(ds.SatisSiparisi.Rows[f]["Toplam"].ToString()) + Car005Toplami;
                                Car005KdvTutari = Convert.ToDecimal(ds.SatisSiparisi.Rows[f]["Kdv"].ToString()) + Car005KdvTutari;
                                i = f;
                            }
                        }

                        #region Sipariş Tarihini Convert Ediyoruz

                        DateTime objSiparisTarihi = new DateTime();
                        objSiparisTarihi = Convert.ToDateTime(ds.SatisSiparisi.Rows[i]["Siparis_Tarihi"].ToString());
                        string strSiparisTarihi;
                        SPTarih = 0;
                        SPTarih = objSiparisTarihi.ToOADate();
                        strSiparisTarihi = SPTarih.ToString().Substring(0, 5);
                        SPTarihRakam = Convert.ToInt32(strSiparisTarihi);

                        #endregion

                        #region Teslim Tarihini Convert Ediyoruz

                        DateTime objSTTarihi = new DateTime();
                        objSTTarihi = Convert.ToDateTime(ds.SatisSiparisi.Rows[i]["Teslim_Tarihi"].ToString());
                        string strSTTarihi;
                        STTarih = 0;
                        STTarih = objSTTarihi.ToOADate();
                        strSTTarihi = SPTarih.ToString().Substring(0, 5);
                        STTarihRakam = Convert.ToInt32(strSTTarihi);

                        #endregion

                        #region Fatura Tarihini Convert Ediyoruz

                        DateTime objFaturaTarihi = new DateTime();
                        objFaturaTarihi = Convert.ToDateTime(ds.SatisSiparisi.Rows[i]["Onay_Tarihi"].ToString());
                        string strFaturaTarihi;
                        CarFaturaTarih = 0;
                        CarFaturaTarih = objFaturaTarihi.ToOADate();
                        strFaturaTarihi = CarFaturaTarih.ToString().Substring(0, 5);
                        CarFaturaTarihRakam = Convert.ToInt32(strFaturaTarihi);

                        #endregion

                        #region Evrak Numarasını Kontrol Edip Ayarlıyoruz

                        string EvNo = ds.SatisSiparisi.Rows[i]["Evrak_No"].ToString().TrimStart();

                        if (EvNo.ToString().Length == 6)
                        {
                            EvNo = "  " + ds.SatisSiparisi.Rows[i]["Evrak_No"].ToString();
                        }
                        else if (EvNo.ToString().Length == 5)
                        {
                            EvNo = "  0" + ds.SatisSiparisi.Rows[i]["Evrak_No"].ToString();
                        }
                        else if (EvNo.ToString().Length == 4)
                        {
                            EvNo = "  00" + ds.SatisSiparisi.Rows[i]["Evrak_No"].ToString();
                        }

                        #endregion

                        string AktarimFiyat = ds.SatisSiparisi.Rows[i]["Fiyat"].ToString();
                        string AktarimToplam = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string AktarimHesapKodu = ds.SatisSiparisi.Rows[i]["Hesap_Kodu"].ToString();
                        string Price = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string CarTotal = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string Stk002Total = Convert.ToString(Car005Toplami).Replace(",", ".");
                        string Stk002BirimFiyat = ds.SatisSiparisi.Rows[i]["Fiyat"].ToString();
                        string Stk002Miktar = ds.SatisSiparisi.Rows[i]["Miktar"].ToString();
                        string KdvStkTutari = Convert.ToString(Car005KdvTutari).Replace(",", ".");

                        string Car005Toplam = Convert.ToString(Car005Toplami);
                        string Car005KdvTut = Convert.ToString(Car005KdvTutari);

                        SiparisID = ds.SatisSiparisi.Rows[i]["SiparisID"].ToString();

                        #region Fiyatları Ayarlarıyoruz

                        Price = Price.Replace(".", ",");
                        double PFiyat = double.Parse(Price);
                        Price = PFiyat.ToString("0.0000");
                        //Price = Convert.ToString(PFiyat);
                        Price = Price.Replace(",", ".");

                        Stk002BirimFiyat = Stk002BirimFiyat.Replace(".", ",");
                        double Stk002BirimFiyat1 = double.Parse(Stk002BirimFiyat);
                        Stk002BirimFiyat = Stk002BirimFiyat1.ToString("0.000000");
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

                        CarTotal = CarTotal.Replace(".", ",");
                        double CarFiyat = double.Parse(CarTotal);
                        CarTotal = CarFiyat.ToString("0.00");
                        //Price = Convert.ToString(PFiyat);
                        CarTotal = CarTotal.Replace(",", ".");

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

                                Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 1)
                            {
                                SatirTipi = "T";
                                SatirAciklama = "MAL BEDELI";
                                YeniTutar = Stk002Total;
                                YeniOran = "0.00";

                                Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                 "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 2)
                            {
                                SatirTipi = "K";
                                SatirAciklama = "KATMA DEGER VERGISI";
                                YeniTutar = Convert.ToString(Car005KdvTutari).Replace(",", ".");
                                YeniOran = ds.SatisSiparisi.Rows[i]["Oran"].ToString().Replace(",", ".");

                                Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 3)
                            {
                                SatirTipi = "Z";
                                SatirAciklama = "";
                                YeniTutar = "0.00";
                                YeniOran = "0.00";

                                Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
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
                                  "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }
                            else if (j == 5)
                            {
                                SatirTipi = "Y";
                                SatirAciklama = "";
                                YeniTutar = "0.00";
                                YeniOran = "0.00";

                                Car005Sorgu = "INSERT INTO YNS" + Sirket.ToString() + ".CAR005(CAR005_Secenek, CAR005_FaturaTarihi, CAR005_FaturaNo, CAR005_BA, CAR005_CariIslemTipi, CAR005_SatirTipi, CAR005_SatirNo, " +
                                  "CAR005_SatirKodu, CAR005_Filler, CAR005_SatirAciklama, CAR005_CHKodu, CAR005_Tutar, CAR005_Oran) " +
                                  "VALUES (5," + TarihRakam + ",'" + EvNo.ToString() + "','B',4,'" + SatirTipi.ToString() + "'," + SatirNo + ",'','1','" + SatirAciklama.ToString() + "','" + AktarimHesapKodu.ToString() + "','" + YeniTutar + "','" + YeniOran + "')";
                            }

                            CmdSCar = new SqlCommand(Car005Sorgu, ConSCar);
                            CmdSCar.CommandTimeout = 120;
                            CmdSCar.ExecuteNonQuery();
                            SatirNo++;

                        }

                        #endregion

                        Car005Toplami = 0;
                        Car005KdvTutari = 0;
                    }

                    #endregion

                    #region Mrp Sipariş Mst-y ve Det-y Tablolarınde Güncelleme Yapıyoruz

                    foreach (DataRow drupdatey in ds.SatisSiparisi.Rows)
                    {

                        string SiparisDetYUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.siparis_det_y SET checkok = 1 " +
                                                   "WHERE sn=" + Convert.ToInt32(drupdatey["SiparisID"].ToString()) + "" +
                                                   "AND sira=" + Convert.ToInt32(drupdatey["SiraID"].ToString()) + "";

                        cmdUpdateMrpSiparisDetY = new SqlCommand(SiparisDetYUpdate, ConUpdateMrpSiparisDetY);
                        cmdUpdateMrpSiparisDetY.ExecuteNonQuery();

                    }

                    foreach (DataRow drUpdatemstY in ds.SatisSiparisi.Rows)
                    {
                        string SiparisMstyUpdate = "UPDATE " + MrpSirket.ToString() + ".dbo.siparis_mst_y SET checkok = 1 " +
                                                   "WHERE sn=" + Convert.ToInt32(drUpdatemstY["SiparisID"].ToString()) + "";

                        cmdUpdateMrpSiparismstY = new SqlCommand(SiparisMstyUpdate, ConUpdateMrpSiparismstY);
                        cmdUpdateMrpSiparismstY.ExecuteNonQuery();
                    }

                    MessageBox.Show("Aktarımı Tamamlanan Toplam Kayıt Sayısı : " + ds.SatisSiparisi.Rows.Count);

                    btnAktar.Enabled = true;
                    btnArama.Enabled = true;

                    #region Bağlantıları Kapatıyoruz

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
                    CmdSAktar.Dispose();
                    cmdSEQNo.Dispose();
                    SEQNoDr.Dispose();
                    SEQNoDr.Close();

                    ConStkEkle.Dispose();
                    ConSAktar.Dispose();
                    ConSEQNo.Dispose();
                    ConSCar.Dispose();
                    ConCariKontrol.Dispose();
                    ConCariAktar.Dispose();
                    ConLinkKontrol.Dispose();
                    ConStk004.Dispose();
                    ConMrpStk.Dispose();
                    ConStkUpdate.Dispose();

                    ConStkEkle.Close();
                    ConSAktar.Close();
                    ConSEQNo.Close();
                    ConSCar.Close();
                    ConCariKontrol.Close();
                    ConCariAktar.Close();
                    ConLinkKontrol.Close();
                    ConStk004.Close();
                    ConMrpStk.Close();
                    ConStkUpdate.Close();

                    ConUpdateMrpSiparismstY.Dispose();
                    ConUpdateMrpSiparismstY.Close();

                    ConUpdateMrpSiparismstY.Dispose();
                    ConUpdateMrpSiparismstY.Close();

                    #endregion

                    #endregion

                }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}

        }

        #endregion

    }
}
