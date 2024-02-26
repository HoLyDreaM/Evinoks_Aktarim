namespace Evinoks_Server_Kurulum
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblSiparis = new System.Windows.Forms.Label();
            this.lblSevk = new System.Windows.Forms.Label();
            this.lblAlimislemleri = new System.Windows.Forms.Label();
            this.ds = new Evinoks_Server_Kurulum.ds();
            this.malKabulBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.malKabulTableAdapter = new Evinoks_Server_Kurulum.dsTableAdapters.MalKabulTableAdapter();
            this.satisSiparisiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.satisSiparisiTableAdapter = new Evinoks_Server_Kurulum.dsTableAdapters.SatisSiparisiTableAdapter();
            this.sevkislemiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sevkislemiTableAdapter = new Evinoks_Server_Kurulum.dsTableAdapters.SevkislemiTableAdapter();
            this.satisSiparisiOlanlarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.satisSiparisiOlanlarTableAdapter = new Evinoks_Server_Kurulum.dsTableAdapters.SatisSiparisiOlanlarTableAdapter();
            this.lblProgram = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.malKabulBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkislemiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiOlanlarBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSiparis
            // 
            this.lblSiparis.AutoSize = true;
            this.lblSiparis.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSiparis.Location = new System.Drawing.Point(29, 11);
            this.lblSiparis.Name = "lblSiparis";
            this.lblSiparis.Size = new System.Drawing.Size(49, 19);
            this.lblSiparis.TabIndex = 0;
            this.lblSiparis.Text = "label1";
            // 
            // lblSevk
            // 
            this.lblSevk.AutoSize = true;
            this.lblSevk.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSevk.Location = new System.Drawing.Point(29, 35);
            this.lblSevk.Name = "lblSevk";
            this.lblSevk.Size = new System.Drawing.Size(49, 19);
            this.lblSevk.TabIndex = 0;
            this.lblSevk.Text = "label1";
            // 
            // lblAlimislemleri
            // 
            this.lblAlimislemleri.AutoSize = true;
            this.lblAlimislemleri.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAlimislemleri.Location = new System.Drawing.Point(29, 59);
            this.lblAlimislemleri.Name = "lblAlimislemleri";
            this.lblAlimislemleri.Size = new System.Drawing.Size(49, 19);
            this.lblAlimislemleri.TabIndex = 0;
            this.lblAlimislemleri.Text = "label1";
            // 
            // ds
            // 
            this.ds.DataSetName = "ds";
            this.ds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // malKabulBindingSource
            // 
            this.malKabulBindingSource.DataMember = "MalKabul";
            this.malKabulBindingSource.DataSource = this.ds;
            // 
            // malKabulTableAdapter
            // 
            this.malKabulTableAdapter.ClearBeforeFill = true;
            // 
            // satisSiparisiBindingSource
            // 
            this.satisSiparisiBindingSource.DataMember = "SatisSiparisi";
            this.satisSiparisiBindingSource.DataSource = this.ds;
            // 
            // satisSiparisiTableAdapter
            // 
            this.satisSiparisiTableAdapter.ClearBeforeFill = true;
            // 
            // sevkislemiBindingSource
            // 
            this.sevkislemiBindingSource.DataMember = "Sevkislemi";
            this.sevkislemiBindingSource.DataSource = this.ds;
            // 
            // sevkislemiTableAdapter
            // 
            this.sevkislemiTableAdapter.ClearBeforeFill = true;
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
            // lblProgram
            // 
            this.lblProgram.AutoSize = true;
            this.lblProgram.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProgram.Location = new System.Drawing.Point(29, 83);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(49, 19);
            this.lblProgram.TabIndex = 0;
            this.lblProgram.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 121);
            this.Controls.Add(this.lblProgram);
            this.Controls.Add(this.lblAlimislemleri);
            this.Controls.Add(this.lblSevk);
            this.Controls.Add(this.lblSiparis);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Evinoks Mrp - Link Entegre Programı";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.malKabulBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkislemiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satisSiparisiOlanlarBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSiparis;
        private System.Windows.Forms.Label lblSevk;
        private System.Windows.Forms.Label lblAlimislemleri;
        private ds ds;
        private System.Windows.Forms.BindingSource malKabulBindingSource;
        private dsTableAdapters.MalKabulTableAdapter malKabulTableAdapter;
        private System.Windows.Forms.BindingSource satisSiparisiBindingSource;
        private dsTableAdapters.SatisSiparisiTableAdapter satisSiparisiTableAdapter;
        private System.Windows.Forms.BindingSource sevkislemiBindingSource;
        private dsTableAdapters.SevkislemiTableAdapter sevkislemiTableAdapter;
        private System.Windows.Forms.BindingSource satisSiparisiOlanlarBindingSource;
        private dsTableAdapters.SatisSiparisiOlanlarTableAdapter satisSiparisiOlanlarTableAdapter;
        private System.Windows.Forms.Label lblProgram;
    }
}

