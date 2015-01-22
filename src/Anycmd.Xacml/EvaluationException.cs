using System;
using System.Runtime.Serialization;

namespace Anycmd.Xacml
{
	/// <summary>
	/// The exception thrown by the evaluation engine when there is a controlled exception found.
	/// </summary>
	[Serializable]
	public class EvaluationException : Exception
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public EvaluationException()
		{
		}

		/// <summary>
		/// Creates an exception with a given message.
		/// </summary>
		/// <param name="message">The exception message.</param>
		public EvaluationException( string message ) 
			: base( message )
		{
		}

		/// <summary>
		/// Creates an exception with a given message.
		/// </summary>
		/// <param name="message">The exception message.</param>
		/// <param name="inner">The inner exception chain.</param>
		public EvaluationException( string message, Exception inner ) 
			: base( message, inner )
		{
		}

		/// <summary>
		/// Required by the serialization engine.
		/// </summary>
		/// <param name="serializationInfo">The serialization information.</param>
		/// <param name="streamingContext">The serialization context.</param>
		protected EvaluationException(SerializationInfo serializationInfo, StreamingContext streamingContext )
			: base( serializationInfo, streamingContext )
		{
		}
	}
}
