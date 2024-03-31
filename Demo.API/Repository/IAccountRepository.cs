using Demo.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.API.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel data);
        Task<string> SignInAsync(SignInModel data);
    }
}