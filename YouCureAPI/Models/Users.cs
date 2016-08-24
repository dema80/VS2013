using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Users {
        public string Login { get; set; }
        public string Pwd { get; set; }
    }
}
