using System;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Framework.Client;
using Microsoft.LightSwitch.Presentation;
using Microsoft.LightSwitch.Presentation.Extensions;
namespace LightSwitchApplication
{
    public partial class TradersHome
    {
        partial void TradersHome_InitializeDataWorkspace(List<IDataService> saveChangesTo)
        {
            if (this.Application.LoggedInUser == null || 
                this.Application.TraderTokenCode == null || 
                this.Application.LoggedInTrader == null)
            {
                //If the logged in user cannot be found in the employee table then we can't display the My Appointments grid.
                //(Note: The CurrentAppointmentsByEmployee query is supplying the Employee value in CurrentAppointmentsByEmployee_PreprocessQuery)
                //Message("Cannot display your appointments. Your user name " + this.Application.User.Name +
                //        " was not found. Please have an administrator add you to the user table.");
                //this.FindControl("CurrentAppointments").IsEnabled = false;
                this.ShowMessageBox("Your user name " + this.Application.User.FullName + " could not be recognized by AviTrade!!");
            }
            else
            {
                // Initialize the UserName property
                //UserName = "You are logged in as " + this.Application.User.FullName;// +" in group " + this.Application.LoggedInUser.Group.Description + "!";
                UserName = "Your AviTrade Account # is " + this.Application.LoggedInTrader.Account;

                // Initialize the UserName property
                TraderAccount = this.Application.LoggedInTrader.Account;
            }
        }

        partial void ViewProfile_Execute()
        {
            // Navigate to the Profile Screen (via the application)
        }

        partial void ConductOrdersAdvancedSearch_Execute()
        {
            // Navigate to the Advanced Search My Orders Screen (via the application)
            this.Application.ShowOrdersAdvancedSearch(""); // TODO: I am not sure why this requires an input!!
        }
    }
}
