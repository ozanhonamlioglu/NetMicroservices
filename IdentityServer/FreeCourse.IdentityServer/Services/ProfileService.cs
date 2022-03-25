using FreeCourse.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
  public class ProfileService : IProfileService
  {

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      string sub = context.Subject.GetSubjectId();
      ApplicationUser user = await _userManager.FindByIdAsync(sub);
      ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

      List<Claim> claims = userClaims.Claims.ToList();
      claims = claims.Where(c => context.RequestedClaimTypes.Contains(c.Type)).ToList();

      if (_userManager.SupportsUserRole)
      {
        IList<string> roles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
          claims.Add(new Claim(JwtClaimTypes.Role, roleName));
          if (_roleManager.SupportsRoleClaims)
          {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
              claims.AddRange(await _roleManager.GetClaimsAsync(role));
            }
          }
        }
      }

      context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
      string sub = context.Subject.GetSubjectId();
      ApplicationUser user = await _userManager.FindByIdAsync(sub);
      context.IsActive = user != null;
    }
  }
}
