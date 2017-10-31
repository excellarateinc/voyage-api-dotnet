using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;
using Swashbuckle.Swagger.Annotations;
using Voyage.Api.Filters;
using Voyage.Core;
using Voyage.Models;
using Voyage.Services.Chat;

namespace Voyage.Api.API.v1
{
    /// <summary>
    /// Controller that handles Chats.
    /// </summary>
    [RoutePrefix(Constants.RoutePrefixes.V1)]
    public class ChatsController : ApiController
    {
        private readonly IChatService _chatService;
        private readonly IAuthenticationManager _authenticationManager;

        /// <summary>
        /// Constructor for the Chats Controller.
        /// </summary>
        public ChatsController(IChatService chatService, IAuthenticationManager authenticationManager)
        {
            _chatService = chatService.ThrowIfNull(nameof(chatService));
            _authenticationManager = authenticationManager.ThrowIfNull(nameof(authenticationManager));
        }

        /// <summary>
        /// Get all channels for the current user.
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ListChannels)]
        [HttpGet]
        [Route("chats/channels")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ChatChannelModel>))]
        public IHttpActionResult GetChannels()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var channels = _chatService.GetChannels(userId);
            return Ok(channels);
        }

        /// <summary>
        /// Creates a new channel.
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.CreateChannel)]
        [HttpPost]
        [Route("chats/channels")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ChatChannelModel))]
        public async Task<IHttpActionResult> CreateChannel()
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var channel = await _chatService.CreateChannel(userId);
            return Ok(channel);
        }

        /// <summary>
        /// Gets messages for the specified channel.
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.ListMessages)]
        [HttpGet]
        [Route("chats/channels/{channelId}/messages")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ChatMessageModel>))]
        public IHttpActionResult GetMessagesByChannel(int channelId)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            var messages = _chatService.GetMessagesByChannel(userId, channelId);
            return Ok(messages);
        }

        /// <summary>
        /// Creates a new message for a channel.
        /// </summary>
        [ClaimAuthorize(ClaimValue = AppClaims.CreateMessage)]
        [HttpPost]
        [Route("chats/channels/{channelId}/messages")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ChatMessageModel))]
        public async Task<IHttpActionResult> CreateMessage([FromUri]int channelId, [FromBody]ChatMessageModel model)
        {
            var userId = _authenticationManager.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value;
            model.ChannelId = channelId;
            var createdMessage = await _chatService.CreateMessage(userId, model);
            return Ok(createdMessage);
        }
    }
}
