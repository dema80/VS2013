using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models
{

    public class HomRemedy
    {
        public int RId { get; set; }
        public int? RPictureid { get; set; }
        public int? RFrequence { get; set; }
        public int? RFrequenceorder { get; set; }
        public string RAbbrev2 { get; set; }
        public string RComment { get; set; }
        public string RInternalnote { get; set; }
        public string RResearchinfo { get; set; }
        public string RNotactivereason { get; set; }
        public DateTime? RDateintroduction { get; set; }
        public string RScientificname { get; set; }
        public bool? RNosode { get; set; }
        public string RAbbrev { get; set; }
        public int? RContenttypevalueid { get; set; }
        public bool? RActive { get; set; }
        public string RHmaname { get; set; }
        public int? RExternalid { get; set; }
        public string ROrigin { get; set; }
        public string RName { get; set; }
        public short? RTypesize { get; set; }
        public string RAbbrev3 { get; set; }
    }
}
