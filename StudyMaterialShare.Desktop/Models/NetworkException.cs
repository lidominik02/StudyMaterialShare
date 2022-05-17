using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Desktop.Models
{
    public class NetworkException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public NetworkException()
        {
        }

        public NetworkException(string? message) : base(message)
        {
        }

        public NetworkException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NetworkException(string? message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public NetworkException(string? message, HttpStatusCode statusCode ,Exception? innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected NetworkException(HttpStatusCode statusCode,SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = statusCode;
        }
    }
}
