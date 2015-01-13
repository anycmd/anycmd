
namespace Anycmd.MongoDb.Conventions
{
    using Model;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson.Serialization.IdGenerators;

    public class GuidIdGeneratorConvention : IPostProcessingConvention
    {
        #region IPostProcessingConvention Members
        /// <summary>
        /// Post process the class map.
        /// </summary>
        /// <param name="classMap">The class map to be processed.</param>
        public void PostProcess(BsonClassMap classMap)
        {
            if (typeof(IEntity).IsAssignableFrom(classMap.ClassType) && classMap.IdMemberMap != null)
                classMap.IdMemberMap.SetIdGenerator(new GuidGenerator());
        }

        #endregion

        #region IConvention Members
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return this.GetType().Name; }
        }

        #endregion
    }
}
