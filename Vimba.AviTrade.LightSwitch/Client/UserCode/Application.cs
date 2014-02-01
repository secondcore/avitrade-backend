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
    public partial class Application
    {
        public User LoggedInUser { get; private set; }
        public string TraderTokenCode { get; private set; } 
        public Trader LoggedInTrader { get; private set; }

        partial void Application_LoggedIn()
        {
            //TODO: the user name is hard-coded!!
            LoggedInUser = this.CreateDataWorkspace().AviTradeOLTPData.UserByLoginName("at0001"/*this.User.Name*/).FirstOrDefault();
            if (LoggedInUser != null)
            {
                var userConfigItem = this.CreateDataWorkspace().AviTradeOLTPData.TraderCodeByUser(LoggedInUser.Id).FirstOrDefault();
                if (userConfigItem != null)
                {
                    TraderTokenCode = userConfigItem.Value;
                    if (TraderTokenCode != null)
                    {
                        LoggedInTrader = this.CreateDataWorkspace().AviTradeOLTPData.TraderByToken(TraderTokenCode).FirstOrDefault().Trader;
                    }
                    else
                    {
                        //TODO: I am not sure what to do! Perhaps we will let the screens complain
                    }
                }
                else
                {
                    //TODO: I am not sure what to do! Perhaps we will let the screens complain
                }
            }
            else
            {
                //TODO: I am not sure what to do! Perhaps we will let the screens complain
            }
        }

        partial void TradersHome_CanRun(ref bool result)
        {
            // Set result to the desired field value
            result = true;

        }
    }
}
