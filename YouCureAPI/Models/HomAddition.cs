using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class HomAddition {
        public int RaId { get; set; }
        public short RaDegree { get; set; }
        public int? RaRubricid { get; set; }
        public short? RaType { get; set; }
        public int RaObjectid { get; set; }
        public float? RaConfidence { get; set; }
        public HomRemedy Remedy { get; set; }

    }
}
