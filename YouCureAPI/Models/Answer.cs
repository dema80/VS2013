using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Answer {
        public Answer() {
            //Session = new List<Session>();
        }
        public int AId { get; set; }
        public int? NextQuestionId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int SymptomId { get; set; }
        public string AText { get; set; }
        //public IList<Session> Session { get; set; }
    }
}
