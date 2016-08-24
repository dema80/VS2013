using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouCureAPI
{
    [Serializable]
    public class SerializedError
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public SerializedError()
        {
            this.TimeStamp = DateTime.Now;
        }

        public SerializedError(string Message)
            : this()
        {
            this.Message = Message;
        }

        public SerializedError(System.Exception ex)
            : this(ex.Message)
        {
            this.StackTrace = ex.StackTrace;
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}