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

namespace Evinoks_Aktarim
{
    public partial class AnaForm : Form
    {
        public AnaForm()
        {
            InitializeComponent();
        }

        #region Bağlantı Ayarları ve Tanımlar

        public AnaForm anaFrm
        {
            get;
            set;
        }
        
        string Cs1 = Properties.Settings.Default.Cs1;
        string Cs2 = Properties.Settings.Default.Cs2;
        public string sirketAdi;
        public string MrpSirket;
        public Login lgn;
        public SatisSiparisi satisSiparisi;
        public Alimislemleri AliMislemleri;
        public Sevkislemleri Sevkislemleri;


        string veriCek;
        string sitedenVeriCek;

        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        #endregion

        #region Tanımlar

        DateTime SevkTarihi;
        DateTime EklenmeTarihi;
        string DepoKodu;
        int timerzaman;

        string ID;
        string SiparisID;
        string Sorgu;
        string Sirket;
        //string MrbSirket;
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

        #region Alım İşlemşleri

        string MalKabulSorgu;
        string IrsaliyeNo;
        string EvrakNo2;
        DateTime IrsaliyeTarih;
        string Durum2;
        DateTime GirisTarih;
        string Doviz;
        string Statu;
        DateTime KayitTarih;
        string KayitEden;
        string MalDurum;
        string SevkDurum;
        double MiktarKabul;

        #endregion

        #endregion

        #region Form Load İşlemleri

        private void AnaForm_Load(object sender, EventArgs e)
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            CheckForIllegalCrossThreadCalls = false;

            Acilis frmAcilis = new Acilis();
            frmAcilis.MdiParent = this;
            frmAcilis.Show();

            Sirket = tsLinkSirket.Text;

            dosyaSurumunuCek();
            YeniSurumBilgisiniCek();
            surumKarsilastir();
            tTarih.Start();
        }

        #endregion

        #region Tarihi Alıyoruz

        private void tTarih_Tick(object sender, EventArgs e)
        {
            tsTarih.Text = Convert.ToString(DateTime.Now);
        }

        #endregion

        #region Login Giriş Ayarları

        private void btnGirisAyari_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (lgn == null || lgn.IsDisposed)
            {
                lgn = new Login();
                lgn.kontrol = false;
                lgn.Show();
            }
            else
            {
                lgn.Activate();
            }
        }

        #endregion

        #region Formu Kapatıyoruz

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnaForm anfrm = new AnaForm();
            anfrm.Close();
        }

        #endregion

        #region Yeni Sürüm Çekiyoruz

        private void YeniSurumBilgisiniCek()
        {
            try
            {
                XmlTextReader xmlDocument = new XmlTextReader("http://www.editorgroup.net/Programlar/Evinoks/MrpAktarim/guncelleme.xml");
                while (xmlDocument.Read())
                {
                    if (xmlDocument.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlDocument.Name)
                        {
                            case "surum":
                                sitedenVeriCek = Convert.ToString(xmlDocument.ReadString());
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sürüm Bilgisi Çekilirken Hata Meydana Geldi" + Environment.NewLine + "Hata Açıklaması : " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Dosya Sürümü Çekiyoruz

        private void dosyaSurumunuCek()
        {
            try
            {
                XmlTextReader xmlDocument = new XmlTextReader("guncelleme.xml");
                while (xmlDocument.Read())
                {
                    if (xmlDocument.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlDocument.Name)
                        {
                            case "surum":
                                veriCek = Convert.ToString(xmlDocument.ReadString());
                                tsProgramVersiyon.Text = veriCek;
                                break;

                        }
                    }
                }
                xmlDocument.Close();
            }
            catch (Exception ex)
            {
                tsProgramVersiyon.Text = veriCek;
                this.Close();
                this.Dispose();
            }
        }

        #endregion

        #region Sürüm Karşılaştırıyoruz

        private void surumKarsilastir()
        {
            if (veriCek != sitedenVeriCek)
            {
                tsProgramVersiyon.ForeColor = Color.Red;
                tsProgramVersiyon.Text = sitedenVeriCek + " Versiyonu Çıkmıştır.Lütfen Güncelleme Yapınız.";
                //surumMbCek();
                btnUpdate.Enabled = true;
            }
            else
            {
                tsProgramVersiyon.Text = veriCek + " Güncel Sürüm";
                btnUpdate.Enabled = false;
            }
        }

        #endregion

        #region Sql Sorgulayıcı KısaYol Tuşu

        private void AnaForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                Process.Start("Sorgulayici.exe");
            }
        }

        #endregion

        #region Güncelleme Butonu

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("ProgramGuncelleme.exe");
            Application.Exit();
        }

        #endregion

        #region Satış Siparişi Butonu

        private void btnSatisSiparisi_Click(object sender, EventArgs e)
        {
            if (satisSiparisi == null || satisSiparisi.IsDisposed)
            {
                satisSiparisi = new SatisSiparisi();
                satisSiparisi.Owner = this;
                satisSiparisi.MdiParent = this;
                satisSiparisi.anaFrm = this;
                satisSiparisi.Show();
            }
            else
            {
                satisSiparisi.Activate();
            }
        }

        #endregion

        #region Alım İşlemleri Butonu

        private void btnAlimislemleri_Click(object sender, EventArgs e)
        {
            if (AliMislemleri == null || AliMislemleri.IsDisposed)
            {
                AliMislemleri = new Alimislemleri();
                AliMislemleri.Owner = this;
                AliMislemleri.MdiParent = this;
                AliMislemleri.anaFrm = this;
                AliMislemleri.Show();
            }
            else
            {
                AliMislemleri.Activate();
            }
        }

        #endregion

        #region Sevk İşlemleri Butonu

        private void btnSevkSorgula_Click(object sender, EventArgs e)
        {
            if (Sevkislemleri == null || Sevkislemleri.IsDisposed)
            {
                Sevkislemleri = new Sevkislemleri();
                Sevkislemleri.Owner = this;
                Sevkislemleri.MdiParent = this;
                Sevkislemleri.anaFrm = this;
                Sevkislemleri.Show();
            }
            else
            {
                Sevkislemleri.Activate();
            }
        }

        #endregion

        #region Ana Formu Boyutlandırıyoruz

        private void AnaForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                Koseicon.Visible = true;

                Thread.Sleep(3000);
                Koseicon.ShowBalloonTip(1000, "Evinok Aktarım Programı", "Evinok Aktarım Programı", ToolTipIcon.Info);
            }
        }

        #endregion

        #region Saat Yanına Alıyoruz Formu

        private void Koseicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Maximized;
            Koseicon.Visible = false;
        }

        #endregion

        #region Regedit Kayıt İşlemleri

        private void btnRegEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            key.GetValue("Evinok Aktarim");
            key.SetValue("Evinoks Aktarım", "\"" + Application.ExecutablePath + "\"");
            key.Close();
            MessageBox.Show("İlk Açılışa Program Kaydedilmiştir.", "Açılış İşlemleri", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        #endregion
    }
}
