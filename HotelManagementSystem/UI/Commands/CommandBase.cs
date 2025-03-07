using System;
using System.Threading.Tasks;

namespace HotelManagementSystem.UI.Commands
{
    public abstract class CommandBase
    {
        protected readonly string _executedBy;
        
        protected CommandBase(string executedBy)
        {
            _executedBy = executedBy ?? "Unknown";
        }
        
        public abstract Task<bool> ExecuteAsync();
    }
}