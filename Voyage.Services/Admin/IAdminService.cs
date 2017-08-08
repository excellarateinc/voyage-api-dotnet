using System.Threading.Tasks;
using Voyage.Models;

namespace Voyage.Services.Admin
{
    public interface IAdminService
    {
        Task<UserModel> ToggleAccountStatus(string userId, ChangeAccountStatusModel changeAccountStatusModel);
    }
}
