using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NHibernate.Mapping;


namespace YouCureAPI.Models
{
    public class TranslatedPathology
    {
        public TranslatedPathology() { }
        public TranslatedPathology(object[] value)
        {
            Id = value[0] as int?;
            CommonName = value[1] as string;
            KeySymptoms = value[2] as string;
            Icd10Code = value[3] as string;
            QIdFirstQuestion = value[4] as int?;
            Warning = value[5] as string;
            ScientificName = value[6] as string;
            Localisation = value[7] as string;
        }
        public virtual int? Id { get; set; }
        public virtual string CommonName { get; set; }
        public virtual string KeySymptoms { get; set; }
        public virtual string Icd10Code { get; set; }
        [Required]
        public virtual int? QIdFirstQuestion { get; set; }
        public virtual string Warning { get; set; }
        public virtual string ScientificName { get; set; }
        public virtual string Localisation { get; set; }
        public static explicit operator TranslatedPathology(object[] value)
        {
            TranslatedPathology tp = new TranslatedPathology(value);
            return tp;
        }
    }
}
