using System;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A AnyUri data type definition. This class contains two static helper methods that help
    /// to hierarchically compare two Uri instances.
    /// </summary>
    public class AnyUri : IDataType
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal AnyUri()
        {
        }

        #endregion

        #region IDataType members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new AnyUriEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new AnyUriIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new AnyUriSubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XsdAnyUri; }
        }

        /// <summary>
        /// Return an instance of an AnyUri form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            try
            {
                return new Uri(value);
            }
            catch (Exception e)
            {
                throw new EvaluationException(string.Format(Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeName), e);
            }
        }

        #endregion

        #region Static members

        /// <summary>
        /// Verifies whether the first Uri specified is an indirect parent of the second Uri specified.
        /// </summary>
        /// <param name="parent">The parent Uri specified.</param>
        /// <param name="descendant">The descendant Uri specified.</param>
        /// <returns>True if the sencond Uri is a descendant of the first Uri, otherwise is False.</returns>
        public static bool IsDescendantOf(Uri parent, Uri descendant)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (descendant == null) throw new ArgumentNullException("descendant");
            if (parent.Scheme == "urn" && descendant.Scheme == "urn")
            {
                string[] parentParts = parent.AbsolutePath.Split(':');
                string[] descendantParts = descendant.AbsolutePath.Split(':');
                if (parentParts.Length > descendantParts.Length)
                {
                    //Parent path is larger than descendant path so there's not a descendant
                    return false;
                }
                for (int i = 0; i < parentParts.Length; i++)
                {
                    if (descendantParts[i] != parentParts[i])
                    {
                        //Some parent path is not equal to descendant path so there's no relationship between them
                        return false;
                    }
                }
                return true;
            }
            else
            {
                throw new EvaluationException(Resource.exc_invalid_uri_schema);
            }
        }

        /// <summary>
        /// Verifies whether the first Uri specified is a direct parent of the second Uri specified.
        /// </summary>
        /// <param name="parent">The parent Uri specified.</param>
        /// <param name="children">The children Uri specified.</param>
        /// <returns>True if the sencond Uri is a chldrend of the first Uri, otherwise is False.</returns>
        public static bool IsChildrenOf(Uri parent, Uri children)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (children == null) throw new ArgumentNullException("children");
            if (parent.Scheme == "urn" && children.Scheme == "urn")
            {
                string[] parentParts = parent.AbsolutePath.Split(':');
                string[] childrenParts = children.AbsolutePath.Split(':');
                if (parentParts.Length > childrenParts.Length)
                {
                    //Parent path is larger than children path so there's not a children
                    return false;
                }
                if (parentParts.Length + 1 != childrenParts.Length)
                {
                    //Parent path length +1 is not equals path length, so there's not a children is a descendant
                    return false;
                }
                for (int i = 0; i < parentParts.Length; i++)
                {
                    if (childrenParts[i] != parentParts[i])
                    {
                        //Some parent path is not equal to children path so there's no relationship between them
                        return false;
                    }
                }
                return true;
            }
            else
            {
                throw new EvaluationException(Resource.exc_invalid_uri_schema);
            }
        }

        #endregion
    }
}
