namespace InfiniteScrolling
{
    partial class InfiniteScrollingDialog
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
            Janus.Windows.GridEX.GridEXLayout gridEXLayout1 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfiniteScrollingDialog));
            this.gridNote = new InfiniteScrolling();
            ((System.ComponentModel.ISupportInitialize)(this.gridNote)).BeginInit();
            this.SuspendLayout();
            // 
            // gridNote
            // 
            gridEXLayout1.LayoutString = resources.GetString("gridEXLayout1.LayoutString");
            this.gridNote.DesignTimeLayout = gridEXLayout1;
            this.gridNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNote.Location = new System.Drawing.Point(0, 0);
            this.gridNote.Name = "gridNote";
            this.gridNote.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridNote.Size = new System.Drawing.Size(803, 322);
            this.gridNote.TabIndex = 0;
            // 
            // InfiniteScrollingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 322);
            this.Controls.Add(this.gridNote);
            this.Name = "InfiniteScrollingDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Infinite Scrolling";
            this.Load += new System.EventHandler(this.InfiniteScrollingDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridNote)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private InfiniteScrolling gridNote;
    }
}

