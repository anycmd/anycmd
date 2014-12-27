
namespace Anycmd.Engine.Hecp
{
    using System;

    /// <summary>A helper class for retrieving and comparing standard Hecp actionCodes.</summary>
    public class Verb : IEquatable<Verb>
    {
        private readonly string _actionCode;
        private static readonly Verb GetAction = new Verb("GET");
        private static readonly Verb UpdateAction = new Verb("UPDATE");
        private static readonly Verb CreateAction = new Verb("CREATE");
        private static readonly Verb DeleteAction = new Verb("DELETE");
        private static readonly Verb HeadAction = new Verb("HEAD");

        /// <summary>
        /// Represents an Hecp GET protocol actionCode.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Verb" />.
        /// </returns>
        public static Verb Get
        {
            get
            {
                return Verb.GetAction;
            }
        }

        /// <summary>
        /// Represents an Hecp PUT protocol actionCode that is used to replace an entity identified by a URI.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Verb" />.
        /// </returns>
        public static Verb Update
        {
            get
            {
                return Verb.UpdateAction;
            }
        }

        /// <summary>
        /// Represents an Hecp POST protocol actionCode that is used to post a new entity as an addition to a URI.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Verb" />.
        /// </returns>
        public static Verb Create
        {
            get
            {
                return Verb.CreateAction;
            }
        }

        /// <summary>
        /// Represents an Hecp DELETE protocol actionCode.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Verb" />.
        /// </returns>
        public static Verb Delete
        {
            get
            {
                return Verb.DeleteAction;
            }
        }

        /// <summary>
        /// Represents an Hecp HEAD protocol actionCode. The HEAD actionCode is identical to GET except that the server only returns message-headers in the response, without a message-body.
        /// </summary>
        /// <returns>
        /// Returns <see cref="Verb" />.
        /// </returns>
        public static Verb Head
        {
            get
            {
                return Verb.HeadAction;
            }
        }

        /// <summary>
        /// An Hecp actionCode. 
        /// </summary>
        /// <returns>
        /// Returns <see cref="T:System.String" />.An Hecp actionCode represented as a <see cref="T:System.String" />.
        /// </returns>
        public string Code
        {
            get
            {
                return this._actionCode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Verb" /> class with a specific Hecp actionCode.
        /// </summary>
        /// <param name="actionCode">The Hecp actionCode.</param>
        public Verb(string actionCode)
        {
            this._actionCode = actionCode ?? string.Empty;
        }

        /// <returns>
        /// Returns <see cref="T:System.Boolean" />.
        /// </returns>
        public bool Equals(Verb other)
        {
            return other != null && (ReferenceEquals(this._actionCode, other._actionCode) || String.Compare(this._actionCode, other._actionCode, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <returns>
        /// Returns <see cref="T:System.Boolean" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Verb);
        }

        /// <returns>
        /// Returns <see cref="T:System.Int32" />.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Code.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// Returns <see cref="T:System.String" />.A string representing the current object.
        /// </returns>
        public override string ToString()
        {
            return this._actionCode;
        }

        /// <returns>
        /// Returns <see cref="T:System.Boolean" />.
        /// </returns>
        public static bool operator ==(Verb left, Verb right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Code.Equals(right.Code, StringComparison.OrdinalIgnoreCase);
        }

        /// <returns>
        /// Returns <see cref="T:System.Boolean" />.
        /// </returns>
        public static bool operator !=(Verb left, Verb right)
        {
            return !(left == right);
        }
    }
}
