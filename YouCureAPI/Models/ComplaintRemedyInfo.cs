using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class ComplaintRemedyInfo {
        public int CrId { get; set; }
        public string RAbbrev { get; set; }
        public string PIcd10Code { get; set; }
        public string TextInfo { get; set; }
    }
}
