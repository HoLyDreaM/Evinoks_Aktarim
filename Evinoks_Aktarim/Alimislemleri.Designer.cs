namespace Evinoks_Aktarim
{
    partial class Alimislemleri
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAktar = new System.Windows.Forms.Button();
            this.btnArama = new System.Windows.Forms.Button();
            this.grdMalKabul = new DevExpress.XtraGrid.GridControl();
            this.malKabulBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ds = new Evinoks_Aktarim.ds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSiraNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHesap_Kodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMalKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIrsaliyeNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIrsaliyeTarih = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDurum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGirisTarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDoviz = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKayitTarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKayitEden = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMalDurum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBirim = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMiktar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colToplamFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSevkDurum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.malKabulTableAdapter = new Evinoks_Aktarim.dsTableAdapters.MalKabulTableAdapter();
            this.colKdvTutari = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKdvOrani = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGenelToplam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMalKabul)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.malKabulBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAktar);
            this.panel1.Controls.Add(this.btnArama);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 64);
            this.panel1.TabIndex = 22;
            // 
            // btnAktar
            // 
            this.btnAktar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAktar.Image = global::Evinoks_Aktarim.Properties.Resources.GorevEkle;
            this.btnAktar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAktar.Location = new System.Drawing.Point(97, 6);
            this.btnAktar.Name = "btnAktar";
            this.btnAktar.Size = new System.Drawing.Size(114, 49);
            this.btnAktar.TabIndex = 5;
            this.btnAktar.Text = "Linke Aktar";
            this.btnAktar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAktar.UseVisualStyleBackColor = true;
            this.btnAktar.Click += new System.EventHandler(this.btnAktar_Click);
            // 
            // btnArama
            // 
            this.btnArama.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnArama.Image = global::Evinoks_Aktarim.Properties.Resources.ara;
            this.btnArama.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArama.Location = new System.Drawing.Point(10, 6);
            this.btnArama.Name = "btnArama";
            this.btnArama.Size = new System.Drawing.Size(81, 49);
            this.btnArama.TabIndex = 5;
            this.btnArama.Text = "Arama";
            this.btnArama.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnArama.UseVisualStyleBackColor = true;
            this.btnArama.Click += new System.EventHandler(this.btnArama_Click);
            // 
            // grdMalKabul
            // 
            this.grdMalKabul.DataSource = this.malKabulBindingSource;
            this.grdMalKabul.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMalKabul.Location = new System.Drawing.Point(0, 64);
            this.grdMalKabul.MainView = this.gridView1;
            this.grdMalKabul.Name = "grdMalKabul";
            this.grdMalKabul.Size = new System.Drawing.Size(972, 354);
            this.grdMalKabul.TabIndex = 23;
            this.grdMalKabul.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // malKabulBindingSource
            // 
            this.malKabulBindingSource.DataMember = "MalKabul";
            this.malKabulBindingSource.DataSource = this.ds;
            // 
            // ds
            // 
            this.ds.DataSetName = "ds";
            this.ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colSiraNo,
            this.colHesap_Kodu,
            this.colMalKodu,
            this.colIrsaliyeNo,
            this.colIrsaliyeTarih,
            this.colDurum,
            this.colGirisTarihi,
            this.colDoviz,
            this.colStatu,
            this.colKayitTarihi,
            this.colKayitEden,
            this.colMalDurum,
            this.colBirim,
            this.colMiktar,
            this.colFiyat,
            this.colToplamFiyat,
            this.colKdvTutari,
            this.colKdvOrani,
            this.colGenelToplam,
            this.colSevkDurum});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.White;
            styleFormatCondition1.Appearance.BackColor2 = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition1.Expression = "IsNullOrEmpty([cha_kod])";
            styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.White;
            styleFormatCondition2.Appearance.BackColor2 = System.Drawing.Color.Green;
            styleFormatCondition2.Appearance.Options.UseBackColor = true;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition2.Expression = " Not  IsNullOrEmpty([cha_kod])";
            styleFormatCondition3.Appearance.BackColor = System.Drawing.Color.White;
            styleFormatCondition3.Appearance.BackColor2 = System.Drawing.Color.Red;
            styleFormatCondition3.Appearance.Options.UseBackColor = true;
            styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition3.Expression = "IsNullOrEmpty([cha_kasa_hizkod])";
            styleFormatCondition4.Appearance.BackColor = System.Drawing.Color.White;
            styleFormatCondition4.Appearance.BackColor2 = System.Drawing.Color.Green;
            styleFormatCondition4.Appearance.Options.UseBackColor = true;
            styleFormatCondition4.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition4.Expression = " Not  IsNullOrEmpty([cha_kasa_hizkod])";
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2,
            styleFormatCondition3,
            styleFormatCondition4});
            this.gridView1.GridControl = this.grdMalKabul;
            this.gridView1.GroupPanelText = "  ";
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colID.Width = 35;
            // 
            // colSiraNo
            // 
            this.colSiraNo.FieldName = "SiraNo";
            this.colSiraNo.Name = "colSiraNo";
            this.colSiraNo.Visible = true;
            this.colSiraNo.VisibleIndex = 1;
            this.colSiraNo.Width = 45;
            // 
            // colHesap_Kodu
            // 
            this.colHesap_Kodu.FieldName = "Hesap_Kodu";
            this.colHesap_Kodu.Name = "colHesap_Kodu";
            this.colHesap_Kodu.Visible = true;
            this.colHesap_Kodu.VisibleIndex = 2;
            // 
            // colMalKodu
            // 
            this.colMalKodu.FieldName = "MalKodu";
            this.colMalKodu.Name = "colMalKodu";
            this.colMalKodu.Visible = true;
            this.colMalKodu.VisibleIndex = 3;
            this.colMalKodu.Width = 114;
            // 
            // colIrsaliyeNo
            // 
            this.colIrsaliyeNo.FieldName = "IrsaliyeNo";
            this.colIrsaliyeNo.Name = "colIrsaliyeNo";
            this.colIrsaliyeNo.Visible = true;
            this.colIrsaliyeNo.VisibleIndex = 4;
            this.colIrsaliyeNo.Width = 65;
            // 
            // colIrsaliyeTarih
            // 
            this.colIrsaliyeTarih.FieldName = "IrsaliyeTarih";
            this.colIrsaliyeTarih.Name = "colIrsaliyeTarih";
            this.colIrsaliyeTarih.Visible = true;
            this.colIrsaliyeTarih.VisibleIndex = 5;
            // 
            // colDurum
            // 
            this.colDurum.FieldName = "Durum";
            this.colDurum.Name = "colDurum";
            this.colDurum.OptionsColumn.ReadOnly = true;
            this.colDurum.Visible = true;
            this.colDurum.VisibleIndex = 6;
            this.colDurum.Width = 55;
            // 
            // colGirisTarihi
            // 
            this.colGirisTarihi.FieldName = "GirisTarihi";
            this.colGirisTarihi.Name = "colGirisTarihi";
            this.colGirisTarihi.Visible = true;
            this.colGirisTarihi.VisibleIndex = 7;
            // 
            // colDoviz
            // 
            this.colDoviz.FieldName = "Doviz";
            this.colDoviz.Name = "colDoviz";
            this.colDoviz.Visible = true;
            this.colDoviz.VisibleIndex = 8;
            this.colDoviz.Width = 50;
            // 
            // colStatu
            // 
            this.colStatu.FieldName = "Statu";
            this.colStatu.Name = "colStatu";
            this.colStatu.Visible = true;
            this.colStatu.VisibleIndex = 9;
            this.colStatu.Width = 55;
            // 
            // colKayitTarihi
            // 
            this.colKayitTarihi.FieldName = "KayitTarihi";
            this.colKayitTarihi.Name = "colKayitTarihi";
            this.colKayitTarihi.Visible = true;
            this.colKayitTarihi.VisibleIndex = 10;
            // 
            // colKayitEden
            // 
            this.colKayitEden.FieldName = "KayitEden";
            this.colKayitEden.Name = "colKayitEden";
            this.colKayitEden.Visible = true;
            this.colKayitEden.VisibleIndex = 11;
            // 
            // colMalDurum
            // 
            this.colMalDurum.FieldName = "MalDurum";
            this.colMalDurum.Name = "colMalDurum";
            this.colMalDurum.OptionsColumn.ReadOnly = true;
            this.colMalDurum.Visible = true;
            this.colMalDurum.VisibleIndex = 12;
            this.colMalDurum.Width = 65;
            // 
            // colBirim
            // 
            this.colBirim.FieldName = "Birim";
            this.colBirim.Name = "colBirim";
            this.colBirim.Visible = true;
            this.colBirim.VisibleIndex = 13;
            this.colBirim.Width = 45;
            // 
            // colMiktar
            // 
            this.colMiktar.FieldName = "Miktar";
            this.colMiktar.Name = "colMiktar";
            this.colMiktar.Visible = true;
            this.colMiktar.VisibleIndex = 14;
            this.colMiktar.Width = 50;
            // 
            // colFiyat
            // 
            this.colFiyat.DisplayFormat.FormatString = "n2";
            this.colFiyat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colFiyat.FieldName = "Fiyat";
            this.colFiyat.Name = "colFiyat";
            this.colFiyat.Visible = true;
            this.colFiyat.VisibleIndex = 15;
            this.colFiyat.Width = 60;
            // 
            // colToplamFiyat
            // 
            this.colToplamFiyat.DisplayFormat.FormatString = "n2";
            this.colToplamFiyat.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colToplamFiyat.FieldName = "ToplamFiyat";
            this.colToplamFiyat.Name = "colToplamFiyat";
            this.colToplamFiyat.Visible = true;
            this.colToplamFiyat.VisibleIndex = 16;
            // 
            // colSevkDurum
            // 
            this.colSevkDurum.FieldName = "SevkDurum";
            this.colSevkDurum.Name = "colSevkDurum";
            this.colSevkDurum.OptionsColumn.ReadOnly = true;
            this.colSevkDurum.Visible = true;
            this.colSevkDurum.VisibleIndex = 20;
            this.colSevkDurum.Width = 70;
            // 
            // malKabulTableAdapter
            // 
            this.malKabulTableAdapter.ClearBeforeFill = true;
            // 
            // colKdvTutari
            // 
            this.colKdvTutari.Caption = "Kdv Tutarı";
            this.colKdvTutari.FieldName = "KdvTutari";
            this.colKdvTutari.Name = "colKdvTutari";
            this.colKdvTutari.Visible = true;
            this.colKdvTutari.VisibleIndex = 17;
            // 
            // colKdvOrani
            // 
            this.colKdvOrani.Caption = "Kdv Oranı";
            this.colKdvOrani.FieldName = "KdvOrani";
            this.colKdvOrani.Name = "colKdvOrani";
            this.colKdvOrani.Visible = true;
            this.colKdvOrani.VisibleIndex = 18;
            // 
            // colGenelToplam
            // 
            this.colGenelToplam.Caption = "Genel Toplam";
            this.colGenelToplam.FieldName = "GenelToplam";
            this.colGenelToplam.Name = "colGenelToplam";
            this.colGenelToplam.Visible = true;
            this.colGenelToplam.VisibleIndex = 19;
            // 
            // Alimislemleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 418);
            this.Controls.Add(this.grdMalKabul);
            this.Controls.Add(this.panel1);
            this.Name = "Alimislemleri";
            this.Text = "Alım İşlemleri Sorgulama";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Alimislemleri_FormClosing);
            this.Load += new System.EventHandler(this.Alimislemleri_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMalKabul)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.malKabulBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAktar;
        private System.Windows.Forms.Button btnArama;
        private DevExpress.XtraGrid.GridControl grdMalKabul;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private ds ds;
        private System.Windows.Forms.BindingSource malKabulBindingSource;
        private dsTableAdapters.MalKabulTableAdapter malKabulTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colSiraNo;
        private DevExpress.XtraGrid.Columns.GridColumn colHesap_Kodu;
        private DevExpress.XtraGrid.Columns.GridColumn colMalKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colIrsaliyeNo;
        private DevExpress.XtraGrid.Columns.GridColumn colIrsaliyeTarih;
        private DevExpress.XtraGrid.Columns.GridColumn colDurum;
        private DevExpress.XtraGrid.Columns.GridColumn colGirisTarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colDoviz;
        private DevExpress.XtraGrid.Columns.GridColumn colStatu;
        private DevExpress.XtraGrid.Columns.GridColumn colKayitTarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colKayitEden;
        private DevExpress.XtraGrid.Columns.GridColumn colMalDurum;
        private DevExpress.XtraGrid.Columns.GridColumn colBirim;
        private DevExpress.XtraGrid.Columns.GridColumn colMiktar;
        private DevExpress.XtraGrid.Columns.GridColumn colFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colToplamFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colSevkDurum;
        private DevExpress.XtraGrid.Columns.GridColumn colKdvTutari;
        private DevExpress.XtraGrid.Columns.GridColumn colKdvOrani;
        private DevExpress.XtraGrid.Columns.GridColumn colGenelToplam;
    }
}