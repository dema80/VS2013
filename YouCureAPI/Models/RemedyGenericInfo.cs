using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class RemedyGenericInfo {
        public int RGId { get; set; }
        public string RAbbrev { get; set; }
        public string RTextInfo { get; set; }
    }
}
