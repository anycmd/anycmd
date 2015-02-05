
namespace Anycmd.Document
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represent a document schemeless to use in collections.
    /// </summary>
    public class BsonDocument : BsonObject
    {
        public const int MaxDocumentSize = 1 * 1024 * 1024; // limits in 1MB max document size to avoid large documents, memory usage and slow performance

        public BsonDocument()
            : base()
        {
        }

        public BsonDocument(BsonValue value)
            : base(value.AsObject.RawValue)
        {
            if (!this.HasKey("_id")) throw new ArgumentException("BsonDocument must have an _id key");

            this.Id = this["_id"].RawValue;
            this.RemoveKey("_id");
        }

        public object Id { get; set; }

        internal BsonDocument(Dictionary<string, object> obj)
            : base(obj)
        {
        }
    }
}
