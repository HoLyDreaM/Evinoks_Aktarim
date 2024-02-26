namespace Evinoks_Aktarim
{
    partial class Sevkislemleri
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sevkislemleri));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAktar = new System.Windows.Forms.Button();
            this.btnArama = new System.Windows.Forms.Button();
            this.grdSevkislemi = new DevExpress.XtraGrid.GridControl();
            this.sevkislemiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ds = new Evinoks_Aktarim.ds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSiparisID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHesapKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMalKodu = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEvrakNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSevkTarih = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEklenmeTarihi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDepo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBirim = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMiktar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFiyat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colToplam = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colKdv = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sevkislemiTableAdapter = new Evinoks_Aktarim.dsTableAdapters.SevkislemiTableAdapter();
            this.colOtvTutari = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSevkislemi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkislemiBindingSource)).BeginInit();
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
            this.btnAktar.Image = ((System.Drawing.Image)(resources.GetObject("btnAktar.Image")));
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
            this.btnArama.Image = ((System.Drawing.Image)(resources.GetObject("btnArama.Image")));
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
            // grdSevkislemi
            // 
            this.grdSevkislemi.DataSource = this.sevkislemiBindingSource;
            this.grdSevkislemi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSevkislemi.Location = new System.Drawing.Point(0, 64);
            this.grdSevkislemi.MainView = this.gridView1;
            this.grdSevkislemi.Name = "grdSevkislemi";
            this.grdSevkislemi.Size = new System.Drawing.Size(972, 354);
            this.grdSevkislemi.TabIndex = 23;
            this.grdSevkislemi.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // sevkislemiBindingSource
            // 
            this.sevkislemiBindingSource.DataMember = "Sevkislemi";
            this.sevkislemiBindingSource.DataSource = this.ds;
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
            this.colSiparisID,
            this.colHesapKodu,
            this.colMalKodu,
            this.colEvrakNo,
            this.colSevkTarih,
            this.colEklenmeTarihi,
            this.colDepo,
            this.colBirim,
            this.colMiktar,
            this.colFiyat,
            this.colToplam,
            this.colKdv,
            this.colOtvTutari});
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
            this.gridView1.GridControl = this.grdSevkislemi;
            this.gridView1.GroupPanelText = "  ";
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colSiparisID
            // 
            this.colSiparisID.FieldName = "SiparisID";
            this.colSiparisID.Name = "colSiparisID";
            this.colSiparisID.Visible = true;
            this.colSiparisID.VisibleIndex = 0;
            // 
            // colHesapKodu
            // 
            this.colHesapKodu.FieldName = "HesapKodu";
            this.colHesapKodu.Name = "colHesapKodu";
            this.colHesapKodu.OptionsColumn.ReadOnly = true;
            this.colHesapKodu.Visible = true;
            this.colHesapKodu.VisibleIndex = 1;
            // 
            // colMalKodu
            // 
            this.colMalKodu.FieldName = "MalKodu";
            this.colMalKodu.Name = "colMalKodu";
            this.colMalKodu.Visible = true;
            this.colMalKodu.VisibleIndex = 2;
            // 
            // colEvrakNo
            // 
            this.colEvrakNo.FieldName = "EvrakNo";
            this.colEvrakNo.Name = "colEvrakNo";
            this.colEvrakNo.Visible = true;
            this.colEvrakNo.VisibleIndex = 3;
            // 
            // colSevkTarih
            // 
            this.colSevkTarih.FieldName = "SevkTarih";
            this.colSevkTarih.Name = "colSevkTarih";
            this.colSevkTarih.Visible = true;
            this.colSevkTarih.VisibleIndex = 4;
            // 
            // colEklenmeTarihi
            // 
            this.colEklenmeTarihi.FieldName = "EklenmeTarihi";
            this.colEklenmeTarihi.Name = "colEklenmeTarihi";
            this.colEklenmeTarihi.Visible = true;
            this.colEklenmeTarihi.VisibleIndex = 5;
            this.colEklenmeTarihi.Width = 110;
            // 
            // colDepo
            // 
            this.colDepo.FieldName = "Depo";
            this.colDepo.Name = "colDepo";
            this.colDepo.Visible = true;
            this.colDepo.VisibleIndex = 6;
            // 
            // colBirim
            // 
            this.colBirim.FieldName = "Birim";
            this.colBirim.Name = "colBirim";
            this.colBirim.OptionsColumn.ReadOnly = true;
            this.colBirim.Visible = true;
            this.colBirim.VisibleIndex = 7;
            // 
            // colMiktar
            // 
            this.colMiktar.FieldName = "Miktar";
            this.colMiktar.Name = "colMiktar";
            this.colMiktar.OptionsColumn.ReadOnly = true;
            this.colMiktar.Visible = true;
            this.colMiktar.VisibleIndex = 8;
            // 
            // colFiyat
            // 
            this.colFiyat.FieldName = "Fiyat";
            this.colFiyat.Name = "colFiyat";
            this.colFiyat.Visible = true;
            this.colFiyat.VisibleIndex = 9;
            // 
            // colToplam
            // 
            this.colToplam.FieldName = "Toplam";
            this.colToplam.Name = "colToplam";
            this.colToplam.OptionsColumn.ReadOnly = true;
            this.colToplam.Visible = true;
            this.colToplam.VisibleIndex = 10;
            // 
            // colKdv
            // 
            this.colKdv.Caption = "Kdv";
            this.colKdv.DisplayFormat.FormatString = "N2";
            this.colKdv.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colKdv.FieldName = "Kdv";
            this.colKdv.Name = "colKdv";
            this.colKdv.Visible = true;
            this.colKdv.VisibleIndex = 11;
            // 
            // sevkislemiTableAdapter
            // 
            this.sevkislemiTableAdapter.ClearBeforeFill = true;
            // 
            // colOtvTutari
            // 
            this.colOtvTutari.Caption = "Ötv Tutarı";
            this.colOtvTutari.DisplayFormat.FormatString = "N2";
            this.colOtvTutari.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colOtvTutari.FieldName = "OtvTutari";
            this.colOtvTutari.Name = "colOtvTutari";
            this.colOtvTutari.Visible = true;
            this.colOtvTutari.VisibleIndex = 12;
            // 
            // Sevkislemleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 418);
            this.Controls.Add(this.grdSevkislemi);
            this.Controls.Add(this.panel1);
            this.Name = "Sevkislemleri";
            this.Text = "Sevk İşlemleri Sorgulama";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Sevkislemleri_FormClosing);
            this.Load += new System.EventHandler(this.Sevkislemleri_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSevkislemi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkislemiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAktar;
        private System.Windows.Forms.Button btnArama;
        private DevExpress.XtraGrid.GridControl grdSevkislemi;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private ds ds;
        private System.Windows.Forms.BindingSource sevkislemiBindingSource;
        private dsTableAdapters.SevkislemiTableAdapter sevkislemiTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colSiparisID;
        private DevExpress.XtraGrid.Columns.GridColumn colHesapKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colMalKodu;
        private DevExpress.XtraGrid.Columns.GridColumn colEvrakNo;
        private DevExpress.XtraGrid.Columns.GridColumn colSevkTarih;
        private DevExpress.XtraGrid.Columns.GridColumn colEklenmeTarihi;
        private DevExpress.XtraGrid.Columns.GridColumn colDepo;
        private DevExpress.XtraGrid.Columns.GridColumn colBirim;
        private DevExpress.XtraGrid.Columns.GridColumn colMiktar;
        private DevExpress.XtraGrid.Columns.GridColumn colFiyat;
        private DevExpress.XtraGrid.Columns.GridColumn colToplam;
        private DevExpress.XtraGrid.Columns.GridColumn colKdv;
        private DevExpress.XtraGrid.Columns.GridColumn colOtvTutari;
    }
}