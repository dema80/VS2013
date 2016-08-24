using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Question {
        public Question() {
			Answers = new List<Answer>();
            //Pathology = new List<Pathology>();
        }
        public virtual int QId { get; set; }
        public virtual string QText { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        //public virtual IList<Pathology> Pathology { get; set; }
    }
}
