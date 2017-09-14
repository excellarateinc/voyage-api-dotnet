using System.IdentityModel.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Chat;

namespace Voyage.Api.API.v1
{
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class ChatsController : ApiController
    {
        private readonly IChatService _chatService;
        private readonly IAuthenticationManager _authenticationManager;

        public ChatsController(IChatService chatService, IAuthenticationManager authenticationManager)
        {
            _chatService = chatService.ThrowIfNull(nameof(chatService));
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        [ClaimAuthorize(ClaimValue = AppClaims.ListChannels)]
        [HttpGet]
        [Route("chats/channels")]
        public IHttpActionResult GetChannels()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var channels = _chatService.GetChannels(userId);
            return Ok(channels);
        }

        [ClaimAuthorize(ClaimValue = AppClaims.CreateChannel)]
        [HttpPost]
        [Route("chats/channels")]
        public async Task<IHttpActionResult> CreateChannel()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var channel = await _chatService.CreateChannel(userId);
            return Ok(channel);
        }

        [ClaimAuthorize(ClaimValue = AppClaims.ListMessages)]
        [HttpGet]
        [Route("chats/channels/{channelId}/messages")]
        public IHttpActionResult GetMessagesByChannel(int channelId)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var messages = _chatService.GetMessagesByChannel(userId, channelId);
            return Ok(messages);
        }

        [ClaimAuthorize(ClaimValue = AppClaims.CreateMessage)]
        [HttpPost]
        [Route("chats/channels/{channelId}/messages")]
        public async Task<IHttpActionResult> CreateMessage([FromUri]int channelId, [FromBody]ChatMessageModel model)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            model.ChannelId = channelId;
            var createdMessage = await _chatService.CreateMessage(userId, model);
            return Ok(createdMessage);
        }
    }
}
