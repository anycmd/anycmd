using System.Reflection;

[assembly: AssemblyVersion(RevisionClass.Major + "." + RevisionClass.Minor + "." + RevisionClass.Build + "." + RevisionClass.Revision)]
[assembly: AssemblyInformationalVersion(RevisionClass.FullVersion)]

internal static class RevisionClass
{
    public const string Major = "0";
    public const string Minor = "9";
    public const string Build = "0";
    public const string Revision = "*";
    public const string VersionName = "Beta1"; // "" is not valid for no version name, you have to use null if you don't want a version name (eg "Beta 1")

    public const string FullVersion = Major + "." + Minor + "." + Build + "." + VersionName;
}
