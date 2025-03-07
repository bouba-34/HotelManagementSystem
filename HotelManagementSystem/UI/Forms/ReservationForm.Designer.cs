using System.ComponentModel;

namespace HotelManagementSystem.UI.Forms
{
    partial class ReservationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboRoom = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboGuest = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpCheckInDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpCheckOutDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.numGuests = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.chkIsPaid = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numDeposit = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAvailability = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeposit)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Room:";
            // 
            // cboRoom
            // 
            this.cboRoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoom.FormattingEnabled = true;
            this.cboRoom.Location = new System.Drawing.Point(106, 12);
            this.cboRoom.Name = "cboRoom";
            this.cboRoom.Size = new System.Drawing.Size(200, 21);
            this.cboRoom.TabIndex = 1;
            this.cboRoom.SelectedIndexChanged += new System.EventHandler(this.cboRoom_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Guest:";
            // 
            // cboGuest
            // 
            this.cboGuest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGuest.FormattingEnabled = true;
            this.cboGuest.Location = new System.Drawing.Point(106, 39);
            this.cboGuest.Name = "cboGuest";
            this.cboGuest.Size = new System.Drawing.Size(200, 21);
            this.cboGuest.TabIndex = 3;
            this.cboGuest.SelectedIndexChanged += new System.EventHandler(this.cboGuest_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Check-In Date:";
            // 
            // dtpCheckInDate
            // 
            this.dtpCheckInDate.Location = new System.Drawing.Point(106, 69);
            this.dtpCheckInDate.Name = "dtpCheckInDate";
            this.dtpCheckInDate.Size = new System.Drawing.Size(200, 20);
            this.dtpCheckInDate.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Check-Out Date:";
            // 
            // dtpCheckOutDate
            // 
            this.dtpCheckOutDate.Location = new System.Drawing.Point(106, 96);
            this.dtpCheckOutDate.Name = "dtpCheckOutDate";
            this.dtpCheckOutDate.Size = new System.Drawing.Size(200, 20);
            this.dtpCheckOutDate.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Number of Guests:";
            // 
            // numGuests
            // 
            this.numGuests.Location = new System.Drawing.Point(106, 125);
            this.numGuests.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numGuests.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numGuests.Name = "numGuests";
            this.numGuests.Size = new System.Drawing.Size(60, 20);
            this.numGuests.TabIndex = 9;
            this.numGuests.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(106, 154);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(305, 60);
            this.txtNotes.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Total Price:";
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.AutoSize = true;
            this.lblTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrice.Location = new System.Drawing.Point(103, 227);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(44, 13);
            this.lblTotalPrice.TabIndex = 13;
            this.lblTotalPrice.Text = "$0.00";
            // 
            // chkIsPaid
            // 
            this.chkIsPaid.AutoSize = true;
            this.chkIsPaid.Location = new System.Drawing.Point(106, 250);
            this.chkIsPaid.Name = "chkIsPaid";
            this.chkIsPaid.Size = new System.Drawing.Size(61, 17);
            this.chkIsPaid.TabIndex = 14;
            this.chkIsPaid.Text = "Is Paid";
            this.chkIsPaid.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 275);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Deposit:";
            // 
            // numDeposit
            // 
            this.numDeposit.DecimalPlaces = 2;
            this.numDeposit.Location = new System.Drawing.Point(106, 273);
            this.numDeposit.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numDeposit.Name = "numDeposit";
            this.numDeposit.Size = new System.Drawing.Size(120, 20);
            this.numDeposit.TabIndex = 16;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(255, 319);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 319);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblAvailability
            // 
            this.lblAvailability.AutoSize = true;
            this.lblAvailability.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailability.Location = new System.Drawing.Point(317, 99);
            this.lblAvailability.Name = "lblAvailability";
            this.lblAvailability.Size = new System.Drawing.Size(94, 13);
            this.lblAvailability.TabIndex = 19;
            this.lblAvailability.Text = "Check availability";
            // 
            // ReservationForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(424, 354);
            this.Controls.Add(this.lblAvailability);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.numDeposit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkIsPaid);
            this.Controls.Add(this.lblTotalPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numGuests);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpCheckOutDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpCheckInDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboGuest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboRoom);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReservationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reservation";
            this.Load += new System.EventHandler(this.ReservationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numGuests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeposit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRoom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboGuest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpCheckInDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpCheckOutDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numGuests;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalPrice;
        private System.Windows.Forms.CheckBox chkIsPaid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numDeposit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAvailability;
    }
}