using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Evinoks_Aktarim
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        #region Bağlantı ve Tanımlar

        string yol;
        string yol2;
        SqlConnection ConLink = new SqlConnection();
        SqlConnection ConMrp = new SqlConnection();
        iniOku.iniOku iniOku = new iniOku.iniOku(Application.StartupPath + "\\ayar.ini");
        AnaForm anafrm;
        public Boolean kontrol = true;

        #endregion

        #region Şifreli Veriyi Çözüyoruz

        private static string Coz(string cozVeri)
        {
            byte[] cozByteDizi = System.Convert.FromBase64String(cozVeri);
            string orjinalVeri = System.Text.ASCIIEncoding.ASCII.GetString(cozByteDizi);
            return orjinalVeri;
        }

        #endregion

        #region veriyi Şifreliyoruz

        private static string Sifrele(string veri)
        {
            byte[] veriByteDizisi = System.Text.ASCIIEncoding.ASCII.GetBytes(veri);
            string sifrelenmisVeri = System.Convert.ToBase64String(veriByteDizisi);
            return sifrelenmisVeri;
        }

        #endregion

        #region Bağlantı Yapıyoruz

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            if (cmGirisTuru.SelectedIndex == 0)
            {
                Properties.Settings.Default["Cs1"] = "Data Source=" + txtLinkServer.Text + ";Initial Catalog=YNS" + txtLinkSirket.Text + ";Integrated Security=True";
                Properties.Settings.Default["Cs2"] = "Data Source=" + txtMrpServer.Text + ";Initial Catalog=" + txtMrpSirket.Text + ";Integrated Security=True";
            }
            else
            {
                Properties.Settings.Default["Cs1"] = "Data Source=" + txtLinkServer.Text + ";Initial Catalog=YNS" + txtLinkSirket.Text + ";User ID=" + txtLinkKullaniciAdi.Text + ";Password=" + txtLinkSifre.Text + "";
                Properties.Settings.Default["Cs2"] = "Data Source=" + txtMrpServer.Text + ";Initial Catalog=" + txtMrpSirket.Text + ";User ID=" + txtMrpKullaniciAdi.Text + ";Password=" + txtMrpSifre.Text + "";
            }

            yol = Properties.Settings.Default.Cs1;
            yol2 = Properties.Settings.Default.Cs2;

            ConLink.ConnectionString = yol;
            ConMrp.ConnectionString = yol2;
            try
            {
                ConLink.Open();
                ConMrp.Open();
                ConLink.Close();
                ConMrp.Close();

                anafrm = new AnaForm();

                if (kontrol) Program.ac.MainForm = anafrm;
                iniOku.IniWriteValue("Ayar", "LinkServer", txtLinkServer.Text);
                iniOku.IniWriteValue("Ayar", "LinkSirket", txtLinkSirket.Text);
                iniOku.IniWriteValue("Ayar", "LinkSifre", Sifrele(txtLinkSifre.Text));
                iniOku.IniWriteValue("Ayar", "LinkKullanici", txtLinkKullaniciAdi.Text);
                iniOku.IniWriteValue("Ayar", "MrpServer", txtMrpServer.Text);
                iniOku.IniWriteValue("Ayar", "MrpSirket", txtMrpSirket.Text);
                iniOku.IniWriteValue("Ayar", "MrpSifre", Sifrele(txtMrpSifre.Text));
                iniOku.IniWriteValue("Ayar", "MrpKullanici", txtMrpKullaniciAdi.Text);
                iniOku.IniWriteValue("Ayar", "oto", oto.Checked.ToString());
                iniOku.IniWriteValue("Ayar", "GirisTuru", cmGirisTuru.SelectedIndex.ToString());
                ConLink.Close();
                ConMrp.Close();

                if (kontrol)
                {
                    anafrm.tsLinkSirket.Text = txtLinkSirket.Text;
                    anafrm.sirketAdi = txtLinkSirket.Text;
                    anafrm.MrpSirket = txtMrpSirket.Text;
                    anafrm.Show();
                }
                else
                {
                    Application.Restart();
                }
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Bağlantı Sağlanamadı");
            }
        }

        #endregion

        #region Giriş Türü Seçimi

        private void cmGirisTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmGirisTuru.SelectedIndex == 0)
            {
                txtLinkKullaniciAdi.Enabled = false;
                txtLinkSifre.Enabled = false;
                txtMrpKullaniciAdi.Enabled = false;
                txtMrpSifre.Enabled = false;
            }
            else
            {
                txtLinkKullaniciAdi.Enabled = true;
                txtLinkSifre.Enabled = true;
                txtMrpKullaniciAdi.Enabled = true;
                txtMrpSifre.Enabled = true;
            }
        }

        #endregion

        #region Form Load İşlemleri

        private void Login_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            cmGirisTuru.SelectedIndex = 0;
            try
            {
                txtLinkServer.Text = iniOku.IniReadValue("Ayar", "LinkServer");
                txtLinkSirket.Text = iniOku.IniReadValue("Ayar", "LinkSirket");
                txtLinkSifre.Text = Coz(iniOku.IniReadValue("Ayar", "LinkSifre"));
                txtLinkKullaniciAdi.Text = iniOku.IniReadValue("Ayar", "LinkKUllanici");
                txtMrpServer.Text = iniOku.IniReadValue("Ayar", "MrpServer");
                txtMrpSirket.Text = iniOku.IniReadValue("Ayar", "MrpSirket");
                txtMrpSifre.Text = Coz(iniOku.IniReadValue("Ayar", "MrpSifre"));
                txtMrpKullaniciAdi.Text = iniOku.IniReadValue("Ayar", "MrpKullanici");

                cmGirisTuru.SelectedIndex = Convert.ToInt32(iniOku.IniReadValue("Ayar", "GirisTuru"));
                oto.Checked = Convert.ToBoolean(iniOku.IniReadValue("Ayar", "oto"));

                if (oto.Checked && kontrol)
                    btnBaglan_Click(sender, e);
            }
            catch { }
        }

        #endregion

    }
}
