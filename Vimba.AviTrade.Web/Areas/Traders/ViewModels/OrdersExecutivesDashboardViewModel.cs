using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vimba.AviTrade.Models;
using Vimba.AviTrade.Models.Stats;

namespace Vimba.AviTrade.Web.Areas.Traders.ViewModels
{
    public class OrdersExecutivesDashboardViewModel
    {
        public List<GenericIntegerIdStringName> TimeSlots { get; set; }
        public GenericIntegerIdStringName SelectedTimeSlot { get; set; }
        public Trader Trader { get; set; }
        public TraderExecutivesDashboard Dashboard { get; set; }

        public OrdersExecutivesDashboardViewModel()
        {
            TimeSlots = new List<GenericIntegerIdStringName>();
            TimeSlots.Add(new GenericIntegerIdStringName()
                {
                    Id = TimeSlotsEquates.TimeSlotLast7Days,
                    Name = "Last 7 Days"
                });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast30Days,
                Name = "Last 30 Days"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast60Days,
                Name = "Last 60 Days"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast90Days,
                Name = "Last 90 Days"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast180Days,
                Name = "Last 180 Days"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast12Months,
                Name = "Last 12 Months"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotYearToDate,
                Name = "Year To Date"
            });
            TimeSlots.Add(new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotMonthToDate,
                Name = "Month To Date"
            });
            
            // Selected default value
            SelectedTimeSlot = new GenericIntegerIdStringName()
            {
                Id = TimeSlotsEquates.TimeSlotLast12Months,
                Name = "Last 12 Months"
            };
        }
    }
}