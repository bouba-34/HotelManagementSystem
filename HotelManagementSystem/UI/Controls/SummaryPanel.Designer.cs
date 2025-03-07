//using System.ComponentModel;

namespace HotelManagementSystem.UI.Controls
{
    partial class SummaryPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblOccupancyRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelSummary
            // 
            this.panelSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSummary.Location = new System.Drawing.Point(3, 32);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(194, 165);
            this.panelSummary.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(137, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Room Summary";
            // 
            // lblOccupancyRate
            // 
            this.lblOccupancyRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOccupancyRate.AutoSize = true;
            this.lblOccupancyRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOccupancyRate.Location = new System.Drawing.Point(4, 200);
            this.lblOccupancyRate.Name = "lblOccupancyRate";
            this.lblOccupancyRate.Size = new System.Drawing.Size(139, 16);
            this.lblOccupancyRate.TabIndex = 2;
            this.lblOccupancyRate.Text = "Occupancy Rate: 0%";
            // 
            // SummaryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOccupancyRate);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panelSummary);
            this.Name = "SummaryPanel";
            this.Size = new System.Drawing.Size(200, 220);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOccupancyRate;
    }
}