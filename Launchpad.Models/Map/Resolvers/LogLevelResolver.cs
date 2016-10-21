using AutoMapper;
using Launchpad.Models.Enum;

namespace Launchpad.Models.Map.Resolvers
{
    public class LogLevelResolver : IMemberValueResolver<object, object, string, StatusCode>
    {
       
        public StatusCode Resolve(object source, object destination, string sourceMember, StatusCode destMember, ResolutionContext context)
        {
            StatusCode result;
            switch (sourceMember)
            {
                case "Information":
                case "Debug":
                case "Verbose":
                    result = StatusCode.OK;
                    break;
                default:
                    result = StatusCode.Critical;
                    break;

            }
            return result;
        }
    }
}
