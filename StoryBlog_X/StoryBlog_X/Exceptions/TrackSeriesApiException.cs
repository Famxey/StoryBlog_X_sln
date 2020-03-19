using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StoryBlog_X.Exceptions
{
    public class TrackSeriesApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Connection { get; set; }

        public TrackSeriesApiException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
            Connection = true;
        }

        public TrackSeriesApiException(string message, bool connection, Exception inner)
            : base(message, inner)
        {
            Connection = connection;
            StatusCode = HttpStatusCode.ServiceUnavailable;
        }
    }

    public class TrackSeriesApiError
    {
        public string Message { get; set; }
    }
}
