namespace Launchpad.Core
{
    public static class Constants
    {
        public static class ErrorCodes
        {
            public const string MissingField = "missing.required.field";
            public const string InvalidEmail = "invalid.email";
            public const string InvalidLength = "invalid.length";
            public const string InvalidDependentRule = "invalid.dependent.rule";
            public const string EntityNotFound = "notfound.entity";
            public const string Unauthorized = "error.unauthorized";
        }
    }
}
