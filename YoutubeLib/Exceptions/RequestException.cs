using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace YoutubeLib.ErrorHandling
{
    /// <summary>
    /// Represents a YouTube request exception.
    /// </summary>
    [Serializable]
    public class RequestException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/> class.
        /// </summary>
        public RequestException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/> class with the specified exception message.
        /// </summary>
        /// <param name="message">The message.</param>
        public RequestException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/> class with the specified message and inner exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public RequestException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestException"/> class with the specified serialization info and context.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The serialization context.</param>
        protected RequestException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
