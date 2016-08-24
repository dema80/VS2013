using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace YouCureAPI.Models {
    
    public class PathologyLocalisation {
        private int? _pId;
        private int? _lId;
        public int PId {
            get {
                return this._pId;
            }
            set {
                this._pId = value;
            }
        }
        public int LId {
            get {
                return this._lId;
            }
            set {
                this._lId = value;
            }
        }
        #region NHibernate Composite Key Requirements
        public override bool Equals(object obj) {
			if (obj == null) return false;
			var t = obj as PathologyLocalisation;
			if (t == null) return false;
			if (PId == t.PId
			 && LId == t.LId)
				return true;

			return false;
        }
        public override int GetHashCode() {
			int hash = GetType().GetHashCode();
			hash = (hash * 397) ^ PId.GetHashCode();
			hash = (hash * 397) ^ LId.GetHashCode();

			return hash;
        }
        #endregion
    }
}
