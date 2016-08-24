using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class SessionDuration {
        public string Token { get; set; }
        public DateTime Deadline { get; set; }
    }
}
