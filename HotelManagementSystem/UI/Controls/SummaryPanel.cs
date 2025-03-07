using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HotelManagementSystem.Core.Enums;

namespace HotelManagementSystem.UI.Controls
{
    public partial class SummaryPanel : UserControl
    {
        private Dictionary<RoomStatusType, int> _roomStatusSummary;
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<RoomStatusType, int> RoomStatusSummary
        {
            get => _roomStatusSummary;
            set
            {
                _roomStatusSummary = value;
                UpdateSummaryDisplay();
            }
        }
        
        public SummaryPanel()
        {
            InitializeComponent();
            _roomStatusSummary = new Dictionary<RoomStatusType, int>();
        }
        
        private void UpdateSummaryDisplay()
        {
            // Si RoomStatusSummary est null, initialisez-le pour éviter l'exception
            if (_roomStatusSummary == null)
            {
                _roomStatusSummary = new Dictionary<RoomStatusType, int>();
            }
            // Clear existing labels
            panelSummary.Controls.Clear();
            
            // Calculate total rooms
            int totalRooms = 0;
            foreach (var count in _roomStatusSummary.Values)
            {
                totalRooms += count;
            }
            
            // Add total rooms label
            AddSummaryRow("Total Rooms", totalRooms.ToString(), Color.White, 0);
            
            // Add status-specific counts
            int yPos = 30;
            
            // Available rooms
            int availableCount = _roomStatusSummary.ContainsKey(RoomStatusType.Available) 
                ? _roomStatusSummary[RoomStatusType.Available] 
                : 0;
            AddSummaryRow("Available", availableCount.ToString(), Color.LightGreen, yPos);
            yPos += 25;
            
            // Occupied rooms
            int occupiedCount = _roomStatusSummary.ContainsKey(RoomStatusType.Occupied) 
                ? _roomStatusSummary[RoomStatusType.Occupied] 
                : 0;
            AddSummaryRow("Occupied", occupiedCount.ToString(), Color.LightCoral, yPos);
            yPos += 25;
            
            // Reserved rooms
            int reservedCount = _roomStatusSummary.ContainsKey(RoomStatusType.Reserved) 
                ? _roomStatusSummary[RoomStatusType.Reserved] 
                : 0;
            AddSummaryRow("Reserved", reservedCount.ToString(), Color.LightBlue, yPos);
            yPos += 25;
            
            // Under maintenance rooms
            int maintenanceCount = _roomStatusSummary.ContainsKey(RoomStatusType.UnderMaintenance) 
                ? _roomStatusSummary[RoomStatusType.UnderMaintenance] 
                : 0;
            AddSummaryRow("Maintenance", maintenanceCount.ToString(), Color.Orange, yPos);
            yPos += 25;
            
            // Cleaning in progress rooms
            int cleaningCount = _roomStatusSummary.ContainsKey(RoomStatusType.CleaningInProgress) 
                ? _roomStatusSummary[RoomStatusType.CleaningInProgress] 
                : 0;
            AddSummaryRow("Cleaning", cleaningCount.ToString(), Color.LightYellow, yPos);
            
            // Calculate occupancy rate
            if (totalRooms > 0)
            {
                double occupancyRate = (double)occupiedCount / totalRooms * 100;
                lblOccupancyRate.Text = $"Occupancy Rate: {occupancyRate:F1}%";
            }
            else
            {
                lblOccupancyRate.Text = "Occupancy Rate: 0.0%";
            }
        }
        
        private void AddSummaryRow(string label, string value, Color color, int yPosition)
        {
            // Create status indicator
            var indicatorPanel = new Panel
            {
                BackColor = color,
                Size = new Size(15, 15),
                Location = new Point(5, yPosition + 2),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Create label
            var lblStatus = new Label
            {
                Text = label,
                AutoSize = true,
                Location = new Point(25, yPosition),
                Font = new Font("Microsoft Sans Serif", 9F)
            };
            
            // Create value
            var lblValue = new Label
            {
                Text = value,
                AutoSize = true,
                Location = new Point(panelSummary.Width - 50, yPosition),
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            // Add to panel
            panelSummary.Controls.Add(indicatorPanel);
            panelSummary.Controls.Add(lblStatus);
            panelSummary.Controls.Add(lblValue);
        }
    }
}