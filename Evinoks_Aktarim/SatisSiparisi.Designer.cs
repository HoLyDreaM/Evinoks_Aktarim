namespace Evinoks_Aktarim
{
    partial class SatisSiparisi
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAktar = new System.Windows.Forms.Button();
            this.btnArama = new System.Windows.Forms.Button();
            this.grdSatisSiparisi = new DevExpress.XtraGrid.GridControl();
            this.satisSiparisiBindingSource = new System.Windows.Forms.BindingSource();
            this.ds = new Evinoks_Aktarim.ds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSiraID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUrunler = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMal_Kodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEvrak_No = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHesap_Kodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSiparis_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOnay_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUretim_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTeslim_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSip_Uretim_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSip_Teslim_Tarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBirim = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMiktar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colToplam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.satisSiparisiTableAdapter = new Evinoks_Aktarim.dsTableAdapters.SatisSiparisiTableAdapter();
            this.satisSiparisiOlanlarBindingSource = new System.Windows.Forms.BindingSource();
            this.satisSiparisiOlanlarTableAdapter = new Evinoks_Aktarim.dsTableAdapters.SatisSiparisiOlanlarTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSatisSiparisi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiOlanlarBindingSource)).BeginInit();
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
            this.panel1.TabIndex = 21;
            // 
            // btnAktar
            // 
            this.btnAktar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAktar.Image = global::Evinoks_Aktarim.Properties.Resources.GorevEkle;
            this.btnAktar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAktar.Location = new System.Drawing.Point(97, 6);
            this.btnAktar.Name = "btnAktar";
            this.btnAktar.Size = new System.Drawing.Size(112, 49);
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
            // grdSatisSiparisi
            // 
            this.grdSatisSiparisi.DataSource = this.satisSiparisiBindingSource;
            this.grdSatisSiparisi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSatisSiparisi.Location = new System.Drawing.Point(0, 64);
            this.grdSatisSiparisi.MainView = this.gridView1;
            this.grdSatisSiparisi.Name = "grdSatisSiparisi";
            this.grdSatisSiparisi.Size = new System.Drawing.Size(972, 354);
            this.grdSatisSiparisi.TabIndex = 22;
            this.grdSatisSiparisi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // satisSiparisiBindingSource
            // 
            this.satisSiparisiBindingSource.DataMember = "SatisSiparisi";
            this.satisSiparisiBindingSource.DataSource = this.ds;
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
            this.colSiraID,
            this.colUrunler,
            this.colMal_Kodu,
            this.colEvrak_No,
            this.colHesap_Kodu,
            this.colSiparis_Tarihi,
            this.colOnay_Tarihi,
            this.colUretim_Tarihi,
            this.colTeslim_Tarihi,
            this.colSip_Uretim_Tarihi,
            this.colSip_Teslim_Tarihi,
            this.colModel,
            this.colBirim,
            this.colMiktar,
            this.colFiyat,
            this.colToplam,
            this.gridColumn1,
            this.gridColumn2});
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
            this.gridView1.GridControl = this.grdSatisSiparisi;
            this.gridView1.GroupPanelText = "  ";
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.Caption = "SiparisID";
            this.colID.FieldName = "SiparisID";
            this.colID.Name = "colID";
            this.colID.OptionsColumn.ReadOnly = true;
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colID.Width = 60;
            // 
            // colSiraID
            // 
            this.colSiraID.Caption = "Sıra No";
            this.colSiraID.FieldName = "SiraID";
            this.colSiraID.Name = "colSiraID";
            this.colSiraID.OptionsColumn.ReadOnly = true;
            this.colSiraID.Visible = true;
            this.colSiraID.VisibleIndex = 1;
            this.colSiraID.Width = 50;
            // 
            // colUrunler
            // 
            this.colUrunler.FieldName = "Urunler";
            this.colUrunler.Name = "colUrunler";
            this.colUrunler.OptionsColumn.ReadOnly = true;
            this.colUrunler.Visible = true;
            this.colUrunler.VisibleIndex = 2;
            // 
            // colMal_Kodu
            // 
            this.colMal_Kodu.Caption = "Mal Kodu";
            this.colMal_Kodu.FieldName = "Mal_Kodu";
            this.colMal_Kodu.Name = "colMal_Kodu";
            this.colMal_Kodu.OptionsColumn.ReadOnly = true;
            this.colMal_Kodu.Visible = true;
            this.colMal_Kodu.VisibleIndex = 3;
            // 
            // colEvrak_No
            // 
            this.colEvrak_No.Caption = "Evrak No";
            this.colEvrak_No.FieldName = "Evrak_No";
            this.colEvrak_No.Name = "colEvrak_No";
            this.colEvrak_No.OptionsColumn.ReadOnly = true;
            this.colEvrak_No.Visible = true;
            this.colEvrak_No.VisibleIndex = 4;
            this.colEvrak_No.Width = 55;
            // 
            // colHesap_Kodu
            // 
            this.colHesap_Kodu.Caption = "Cari Kodu";
            this.colHesap_Kodu.FieldName = "Hesap_Kodu";
            this.colHesap_Kodu.Name = "colHesap_Kodu";
            this.colHesap_Kodu.OptionsColumn.ReadOnly = true;
            this.colHesap_Kodu.Visible = true;
            this.colHesap_Kodu.VisibleIndex = 5;
            this.colHesap_Kodu.Width = 68;
            // 
            // colSiparis_Tarihi
            // 
            this.colSiparis_Tarihi.Caption = "Sipariş Tarihi";
            this.colSiparis_Tarihi.DisplayFormat.FormatString = "d";
            this.colSiparis_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colSiparis_Tarihi.FieldName = "Siparis_Tarihi";
            this.colSiparis_Tarihi.Name = "colSiparis_Tarihi";
            this.colSiparis_Tarihi.OptionsColumn.ReadOnly = true;
            this.colSiparis_Tarihi.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.colSiparis_Tarihi.Visible = true;
            this.colSiparis_Tarihi.VisibleIndex = 6;
            // 
            // colOnay_Tarihi
            // 
            this.colOnay_Tarihi.Caption = "Onay Tarihi";
            this.colOnay_Tarihi.DisplayFormat.FormatString = "d";
            this.colOnay_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colOnay_Tarihi.FieldName = "Onay_Tarihi";
            this.colOnay_Tarihi.Name = "colOnay_Tarihi";
            this.colOnay_Tarihi.OptionsColumn.ReadOnly = true;
            this.colOnay_Tarihi.Visible = true;
            this.colOnay_Tarihi.VisibleIndex = 7;
            // 
            // colUretim_Tarihi
            // 
            this.colUretim_Tarihi.Caption = "Üretim Tarihi";
            this.colUretim_Tarihi.DisplayFormat.FormatString = "d";
            this.colUretim_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colUretim_Tarihi.FieldName = "Uretim_Tarihi";
            this.colUretim_Tarihi.Name = "colUretim_Tarihi";
            this.colUretim_Tarihi.OptionsColumn.ReadOnly = true;
            this.colUretim_Tarihi.Visible = true;
            this.colUretim_Tarihi.VisibleIndex = 8;
            // 
            // colTeslim_Tarihi
            // 
            this.colTeslim_Tarihi.Caption = "Teslim Tarihi";
            this.colTeslim_Tarihi.DisplayFormat.FormatString = "d";
            this.colTeslim_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTeslim_Tarihi.FieldName = "Teslim_Tarihi";
            this.colTeslim_Tarihi.Name = "colTeslim_Tarihi";
            this.colTeslim_Tarihi.OptionsColumn.ReadOnly = true;
            this.colTeslim_Tarihi.Visible = true;
            this.colTeslim_Tarihi.VisibleIndex = 9;
            this.colTeslim_Tarihi.Width = 78;
            // 
            // colSip_Uretim_Tarihi
            // 
            this.colSip_Uretim_Tarihi.Caption = "Sipariş Üretim Tarihi";
            this.colSip_Uretim_Tarihi.DisplayFormat.FormatString = "d";
            this.colSip_Uretim_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colSip_Uretim_Tarihi.FieldName = "Sip_Uretim_Tarihi";
            this.colSip_Uretim_Tarihi.Name = "colSip_Uretim_Tarihi";
            this.colSip_Uretim_Tarihi.OptionsColumn.ReadOnly = true;
            this.colSip_Uretim_Tarihi.Visible = true;
            this.colSip_Uretim_Tarihi.VisibleIndex = 10;
            this.colSip_Uretim_Tarihi.Width = 112;
            // 
            // colSip_Teslim_Tarihi
            // 
            this.colSip_Teslim_Tarihi.Caption = "Sipariş Teslim Tarihi";
            this.colSip_Teslim_Tarihi.DisplayFormat.FormatString = "d";
            this.colSip_Teslim_Tarihi.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colSip_Teslim_Tarihi.FieldName = "Sip_Teslim_Tarihi";
            this.colSip_Teslim_Tarihi.Name = "colSip_Teslim_Tarihi";
            this.colSip_Teslim_Tarihi.OptionsColumn.ReadOnly = true;
            this.colSip_Teslim_Tarihi.Visible = true;
            this.colSip_Teslim_Tarihi.VisibleIndex = 11;
            this.colSip_Teslim_Tarihi.Width = 114;
            // 
            // colModel
            // 
            this.colModel.FieldName = "Model";
            this.colModel.Name = "colModel";
            this.colModel.OptionsColumn.ReadOnly = true;
            this.colModel.Visible = true;
            this.colModel.VisibleIndex = 12;
            this.colModel.Width = 50;
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
            this.colMiktar.OptionsColumn.ReadOnly = true;
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
            this.colFiyat.OptionsColumn.ReadOnly = true;
            this.colFiyat.Visible = true;
            this.colFiyat.VisibleIndex = 15;
            this.colFiyat.Width = 60;
            // 
            // colToplam
            // 
            this.colToplam.DisplayFormat.FormatString = "N2";
            this.colToplam.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colToplam.FieldName = "Toplam";
            this.colToplam.Name = "colToplam";
            this.colToplam.OptionsColumn.ReadOnly = true;
            this.colToplam.Visible = true;
            this.colToplam.VisibleIndex = 16;
            this.colToplam.Width = 60;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Kdv";
            this.gridColumn1.DisplayFormat.FormatString = "N2";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumn1.FieldName = "Kdv";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 17;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Kdv Oranı";
            this.gridColumn2.DisplayFormat.FormatString = "N2";
            this.gridColumn2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumn2.FieldName = "Oran";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 18;
            // 
            // satisSiparisiTableAdapter
            // 
            this.satisSiparisiTableAdapter.ClearBeforeFill = true;
            // 
            // satisSiparisiOlanlarBindingSource
            // 
            this.satisSiparisiOlanlarBindingSource.DataMember = "SatisSiparisiOlanlar";
            this.satisSiparisiOlanlarBindingSource.DataSource = this.ds;
            // 
            // satisSiparisiOlanlarTableAdapter
            // 
            this.satisSiparisiOlanlarTableAdapter.ClearBeforeFill = true;
            // 
            // SatisSiparisi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 418);
            this.Controls.Add(this.grdSatisSiparisi);
            this.Controls.Add(this.panel1);
            this.Name = "SatisSiparisi";
            this.Text = "Satış Siparişi Sorgulama";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SatisSiparisi_FormClosing);
            this.Load += new System.EventHandler(this.SatisSiparisi_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSatisSiparisi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiOlanlarBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAktar;
        private System.Windows.Forms.Button btnArama;
        private DevExpress.XtraGrid.GridControl grdSatisSiparisi;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private ds ds;
        private System.Windows.Forms.BindingSource satisSiparisiBindingSource;
        private dsTableAdapters.SatisSiparisiTableAdapter satisSiparisiTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colSiraID;
        private DevExpress.XtraGrid.Columns.GridColumn colUrunler;
        private DevExpress.XtraGrid.Columns.GridColumn colMal_Kodu;
        private DevExpress.XtraGrid.Columns.GridColumn colEvrak_No;
        private DevExpress.XtraGrid.Columns.GridColumn colHesap_Kodu;
        private DevExpress.XtraGrid.Columns.GridColumn colSiparis_Tarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colOnay_Tarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colUretim_Tarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colTeslim_Tarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colModel;
        private DevExpress.XtraGrid.Columns.GridColumn colBirim;
        private DevExpress.XtraGrid.Columns.GridColumn colMiktar;
        private DevExpress.XtraGrid.Columns.GridColumn colFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colToplam;
        private DevExpress.XtraGrid.Columns.GridColumn colSip_Uretim_Tarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colSip_Teslim_Tarihi;
        private System.Windows.Forms.BindingSource satisSiparisiOlanlarBindingSource;
        private dsTableAdapters.SatisSiparisiOlanlarTableAdapter satisSiparisiOlanlarTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;

    }
}