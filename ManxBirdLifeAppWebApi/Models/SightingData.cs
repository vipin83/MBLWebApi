using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManxBirdLifeAppWebApi.Models
{
    public class SightingData
    {
        public int sightReportUserID { get; set; }             
        public object locationSelected { get; set; }
        public string gridReference { get; set; }
        public string dt { get; set; }
        public string myTime { get; set; }       
    }

    public class Species
    {
        public int SightDetailsID { get; set; }
        public object speciesSelected { get; set; }
        public int speciesnumber { get; set; }
        public string approximation { get; set; }
        public string details { get; set; }
        public bool chkSensitiveDoNotDisplay { get; set; }
    }

    public class EditSightingData
    {
        public int SightDetailsID { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string Details { get; set; }
        public bool boolOverrideSensitiveSpeciesRecord { get; set; }
        public string GridRef { get; set; }
		public string SightDate { get; set; }
		public string SightTime { get; set; }
	}
}