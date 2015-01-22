
namespace Anycmd.Xacml
{
    public partial class Consts
    {
        public partial class Schema1
        {
            /// <summary>The name of the element/attribute in the XSD schema.</summary>
            public static class PolicyCombiningAlgorithms
            {
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string DenyOverrides = "urn:oasis:names:tc:xacml:1.0:policy-combining-algorithm:deny-overrides";
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string PermitOverrides = "urn:oasis:names:tc:xacml:1.0:policy-combining-algorithm:permit-overrides";
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string FirstApplicable = "urn:oasis:names:tc:xacml:1.0:policy-combining-algorithm:first-applicable";
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string OnlyOneApplicable = "urn:oasis:names:tc:xacml:1.0:policy-combining-algorithm:only-one-applicable";
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string OrderedDenyOverrides = "urn:oasis:names:tc:xacml:1.1:policy-combining-algorithm:ordered-deny-overrides";
                /// <summary>The name of the element/attribute in the XSD schema.</summary>
                public const string OrderedPermitOverrides = "urn:oasis:names:tc:xacml:1.1:policy-combining-algorithm:ordered-permit-overrides";
            }
        }
    }
}