using System.ComponentModel;

namespace HotelManagementSystem.UI.Controls
{
    partial class RoomControl
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
            this.lblRoomNumber = new System.Windows.Forms.Label();
            this.lblRoomType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.picWifi = new System.Windows.Forms.PictureBox();
            this.picMinibar = new System.Windows.Forms.PictureBox();
            this.picBalcony = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWifi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinibar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBalcony)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRoomNumber
            // 
            this.lblRoomNumber.AutoSize = true;
            this.lblRoomNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoomNumber.Location = new System.Drawing.Point(3, 4);
            this.lblRoomNumber.Name = "lblRoomNumber";
            this.lblRoomNumber.Size = new System.Drawing.Size(57, 20);
            this.lblRoomNumber.TabIndex = 0;
            this.lblRoomNumber.Text = "101";
            // 
            // lblRoomType
            // 
            this.lblRoomType.AutoSize = true;
            this.lblRoomType.Location = new System.Drawing.Point(4, 30);
            this.lblRoomType.Name = "lblRoomType";
            this.lblRoomType.Size = new System.Drawing.Size(38, 13);
            this.lblRoomType.TabIndex = 1;
            this.lblRoomType.Text = "Single";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom |
                                                                          System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(4, 83);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 13);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Available";
            // 
            // lblPrice
            // 
            this.lblPrice.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.Location = new System.Drawing.Point(90, 6);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(49, 15);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "$99.99";
            // 
            // picWifi
            // 
            this.picWifi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom |
                                                                        System.Windows.Forms.AnchorStyles.Right)));
            this.picWifi.Image = null; // Wi-Fi icon would be set here
            this.picWifi.Location = new System.Drawing.Point(93, 50);
            this.picWifi.Name = "picWifi";
            this.picWifi.Size = new System.Drawing.Size(16, 16);
            this.picWifi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWifi.TabIndex = 4;
            this.picWifi.TabStop = false;
            // 
            // picMinibar
            // 
            this.picMinibar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom |
                                                                           System.Windows.Forms.AnchorStyles.Right)));
            this.picMinibar.Image = null; // Minibar icon would be set here
            this.picMinibar.Location = new System.Drawing.Point(115, 50);
            this.picMinibar.Name = "picMinibar";
            this.picMinibar.Size = new System.Drawing.Size(16, 16);
            this.picMinibar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMinibar.TabIndex = 5;
            this.picMinibar.TabStop = false;
            // 
            // picBalcony
            // 
            this.picBalcony.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom |
                                                                           System.Windows.Forms.AnchorStyles.Right)));
            this.picBalcony.Image = null; // Balcony icon would be set here
            this.picBalcony.Location = new System.Drawing.Point(93, 72);
            this.picBalcony.Name = "picBalcony";
            this.picBalcony.Size = new System.Drawing.Size(16, 16);
            this.picBalcony.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBalcony.TabIndex = 6;
            this.picBalcony.TabStop = false;
            // 
            // RoomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.picBalcony);
            this.Controls.Add(this.picMinibar);
            this.Controls.Add(this.picWifi);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblRoomType);
            this.Controls.Add(this.lblRoomNumber);
            this.Name = "RoomControl";
            this.Size = new System.Drawing.Size(150, 100);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RoomControl_MouseClick);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RoomControl_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.picWifi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinibar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBalcony)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblRoomNumber;
        private System.Windows.Forms.Label lblRoomType;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.PictureBox picWifi;
        private System.Windows.Forms.PictureBox picMinibar;
        private System.Windows.Forms.PictureBox picBalcony;
    }
}