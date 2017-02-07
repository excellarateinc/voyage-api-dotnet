namespace Voyage.Web.Auth_Stuff
{
    public static class Clients
    {
        public static readonly Client Client1 = new Client
        {
            Id = "123456",
            Secret = "abcdef",
            RedirectUrl = Paths.AuthorizeCodeCallBackPath
        };

        public static readonly Client Client2 = new Client
        {
            Id = "7890ab",
            Secret = "7890ab",
            RedirectUrl = Paths.ImplicitGrantCallBackPath
        };
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class Client
#pragma warning restore SA1402 // File may only contain a single class
    {
        public string Id { get; set; }

        public string Secret { get; set; }

        public string RedirectUrl { get; set; }
    }
}