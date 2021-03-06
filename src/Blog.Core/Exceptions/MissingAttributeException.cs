using System;

namespace Blog.Core
{
    public class MissingAttributeException : Exception
    {
        public Type AttributeTypeExpected { get; private set; }

        public MissingAttributeException(Type expected)
            : base($"Expected {expected.Name} was not found")
        {
            AttributeTypeExpected = expected;
        }
    }
}