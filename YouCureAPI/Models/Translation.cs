using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class Translation {
        public string Original { get; set; }
        public short LanguageId { get; set; }
        public string Translationval { get; set; }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as Translation;
			if (t == null) return false;
			if (Original == t.Original
			 && LanguageId == t.LanguageId)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ Original.GetHashCode();
			hash = (hash * 397) ^ LanguageId.GetHashCode();

			return hash;
        }
        #endregion
    }
}
