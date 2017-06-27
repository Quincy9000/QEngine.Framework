using System;

namespace QPhysics.Shared.Contracts
{
    public class RequiredException : Exception
    {
        public RequiredException(string message) : base(message) { }
    }
}