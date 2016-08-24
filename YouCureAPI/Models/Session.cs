using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Session {
        public int SId { get; set; }
        public int SAnswerId { get; set; }
        public string SToken { get; set; }
    }
}
