using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Pathology {
        public Pathology()
        {
            Localisations = new List<Localisation>();
        }
        public int PId { get; set; }
        public string PCommonName { get; set; }
        public string PKeySymptoms { get; set; }
        public string PIcd10Code { get; set; }
        [Required]
        public int QIdFirstQuestion { get; set; }
        public string PWarning { get; set; }
        public string PScientificName { get; set; }
        public IList<Localisation> Localisations { get; set; }
    }
}
