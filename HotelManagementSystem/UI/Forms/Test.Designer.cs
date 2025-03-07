using System.ComponentModel;

namespace HotelManagementSystem.UI.Forms
{
    partial class Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
    
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
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(160, 54);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(366, 216);
            label1.TabIndex = 0;
            label1.Text = "Test";
            // 
            // Test
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(802, 458);
            Controls.Add(label1);
            Text = "Test";
            ResumeLayout(false);
        }
    
        private System.Windows.Forms.Label label1;
    
        #endregion
    }
}

