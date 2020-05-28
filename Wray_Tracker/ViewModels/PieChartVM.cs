using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wray_Tracker.ViewModels
{
    public class PieChartVM
    {
        public List<KeyValuePair<int, string>> Data { get; set; }
        public List<string> BackgroundColors { get; set; }
        public List<string> Lables { get; set; }

        public PieChartVM()
        {

        }
    }

    
}