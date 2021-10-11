using System;

namespace Network.Exceptions
{
    /// <summary>
    /// Describes an error that occurred while processing a package.
    /// </summary>
    public class PackageException : Exception
    {
        internal PackageException(string message) : base(message)
        {
        }
    }
}
