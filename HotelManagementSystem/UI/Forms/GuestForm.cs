// HotelManagementSystem.UI/Forms/GuestForm.cs
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.UI.Utilities;

namespace HotelManagementSystem.UI.Forms
{
    public partial class GuestForm : Form
    {
        private readonly IGuestService _guestService;
        private Guest _guest;
        private bool _isNewGuest;

        public GuestForm(IGuestService guestService, int? guestId = null)
        {
            InitializeComponent();
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            _isNewGuest = !guestId.HasValue;

            if (_isNewGuest)
            {
                Text = "Add New Guest";
                _guest = new Guest();
            }
            else
            {
                Text = "Edit Guest";
                Tag = guestId; // Store ID for later loading
            }
        }

        private async void GuestForm_Load(object sender, EventArgs e)
        {
            // Only need to load data if editing an existing guest
            if (!_isNewGuest && Tag is int guestId)
            {
                await this.ExecuteWithUIFeedbackAsync(async () =>
                {
                    try
                    {
                        _guest = await _guestService.GetGuestByIdAsync(guestId);
                        
                        if (_guest == null)
                        {
                            this.ShowError("Guest not found.");
                            this.InvokeIfRequired(() => Close());
                            return;
                        }

                        // Populate form fields
                        PopulateFormFields();
                    }
                    catch (Exception ex)
                    {
                        this.ShowError($"Error loading guest: {ex.Message}");
                    }
                });
            }
        }

        private void PopulateFormFields()
        {
            txtFirstName.Text = _guest.FirstName;
            txtLastName.Text = _guest.LastName;
            txtEmail.Text = _guest.Email;
            txtPhone.Text = _guest.Phone;
            txtAddress.Text = _guest.Address;
            txtCity.Text = _guest.City;
            txtState.Text = _guest.State;
            txtZipCode.Text = _guest.ZipCode;
            txtCountry.Text = _guest.Country;
            cboIdentificationType.Text = _guest.IdentificationType;
            txtIdentificationNumber.Text = _guest.IdentificationNumber;
            
            if (_guest.DateOfBirth.HasValue)
            {
                dtpDateOfBirth.Value = _guest.DateOfBirth.Value;
                chkHasDOB.Checked = true;
            }
            else
            {
                chkHasDOB.Checked = false;
                dtpDateOfBirth.Enabled = false;
            }
            
            txtNotes.Text = _guest.Notes;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            await this.ExecuteWithUIFeedbackAsync(async () =>
            {
                try
                {
                    // Update guest object with form values
                    _guest.FirstName = txtFirstName.Text.Trim();
                    _guest.LastName = txtLastName.Text.Trim();
                    _guest.Email = txtEmail.Text.Trim();
                    _guest.Phone = txtPhone.Text.Trim();
                    _guest.Address = txtAddress.Text.Trim();
                    _guest.City = txtCity.Text.Trim();
                    _guest.State = txtState.Text.Trim();
                    _guest.ZipCode = txtZipCode.Text.Trim();
                    _guest.Country = txtCountry.Text.Trim();
                    _guest.IdentificationType = cboIdentificationType.Text;
                    _guest.IdentificationNumber = txtIdentificationNumber.Text.Trim();
                    _guest.DateOfBirth = chkHasDOB.Checked ? dtpDateOfBirth.Value : (DateTime?)null;
                    _guest.Notes = txtNotes.Text.Trim();

                    if (_isNewGuest)
                    {
                        // Create new guest
                        await _guestService.CreateGuestAsync(_guest);
                        this.ShowInfo("Guest added successfully.", "Success");
                    }
                    else
                    {
                        // Update existing guest
                        bool success = await _guestService.UpdateGuestAsync(_guest);
                        if (success)
                        {
                            this.ShowInfo("Guest updated successfully.", "Success");
                        }
                        else
                        {
                            this.ShowError("Failed to update guest.");
                            return;
                        }
                    }

                    DialogResult = DialogResult.OK;
                    this.InvokeIfRequired(() => Close());
                }
                catch (InvalidOperationException ex)
                {
                    this.ShowError(ex.Message, "Validation Error");
                }
                catch (Exception ex)
                {
                    this.ShowError($"Error saving guest: {ex.Message}");
                }
            });
        }

        private bool ValidateForm()
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                this.ShowError("First name is required.", "Validation Error");
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                this.ShowError("Last name is required.", "Validation Error");
                txtLastName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                // Simple email validation
                if (!IsValidEmail(txtEmail.Text))
                {
                    this.ShowError("Please enter a valid email address.", "Validation Error");
                    txtEmail.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(cboIdentificationType.Text))
            {
                this.ShowError("Identification type is required.", "Validation Error");
                cboIdentificationType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtIdentificationNumber.Text))
            {
                this.ShowError("Identification number is required.", "Validation Error");
                txtIdentificationNumber.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkHasDOB_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/disable date of birth picker based on checkbox
            dtpDateOfBirth.Enabled = chkHasDOB.Checked;
        }
    }
}