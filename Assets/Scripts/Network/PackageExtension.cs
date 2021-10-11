using Network;
using UnityEngine;

public static class PackageExtension
{
    public static void WriteVector3(this Package package, Vector3 vector)
    {
        package.WriteFloat(vector.x);
        package.WriteFloat(vector.y);
        package.WriteFloat(vector.z);
    }

    public static Vector3 ReadVector3(this Package package)
    {
        return new Vector3(package.ReadFloat(), package.ReadFloat(), package.ReadFloat());
    }
}
