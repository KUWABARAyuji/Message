using System.Collections.Generic;
using System.Threading.Tasks;
using WebChat.Domain.Models.DTOs;
using WebChat.Domain.Models;

namespace WebChat.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<List<ApplicationUser>> ListUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<ApplicationUser> GetUserByUserName(string userName);
        Task<int> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string userId);
        Task<bool> SignUp(SignUpDTO signUpDTO);
        Task<SsoDTO> SignIn(SignInDTO signInDTO);
        Task<ApplicationUser> GetCurrentUser();

    }
}
