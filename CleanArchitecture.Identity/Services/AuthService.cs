using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Identity.Services
{
	public class AuthService : IAuthService
	{
		// Permite consultar data de usuarios, data de roles y crear data de estas entidades
		private readonly UserManager<ApplicationUser> _userManager;
		// Permite hacer validación de credenciales
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly JwtSettings _jwtSettings;

		public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtSettings = jwtSettings.Value;
		}

		public async Task<AuthResponse> Login(AuthRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if(user is null)
			{
				throw new Exception($"El usuario con email {request.Email} no existe");
			}

			var resultado = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!resultado.Succeeded)
            {
				throw new Exception($"Las credenciales son incorrectas");
            }


			var authResponse = new AuthResponse
			{
				Id = user.Id,
				Token = await GenerateToken(user),
				Email = user.Email,
				UserName = user.UserName,
			};

			return authResponse;
        }

		public async Task<RegistrationResponse> Register(RegistrationRequest request)
		{
			var existingUser = await _userManager.FindByNameAsync(request.Username);
			if(existingUser is not null)
			{
				throw new Exception($"El username ya fue tomado por otra cuenta");
			}

			var existingEmail = await _userManager.FindByEmailAsync(request.Email);
			if(existingEmail is not null)
			{
				throw new Exception($"El email ya fue tomado por otra cuenta");
			}

			var user = new ApplicationUser
			{
				Email = request.Email,
				Nombre = request.Nombre,
				Apellidos = request.Apellidos,
				UserName = request.Username,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "Operator");
				return new RegistrationResponse
				{
					Email = user.Email,
					Token = await GenerateToken(user),
					UserId = user.Id,
					UserName = user.UserName
				};
			}

			throw new Exception($"{result.Errors}");
		}

		private async Task<string> GenerateToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);

			var roleClaims = new List<Claim>();

			foreach (var role in roles)
			{
				roleClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(CustomClaimTypes.Uid, user.Id),
			}.Union(userClaims).Union(roleClaims);

			// Llave de seguridad para acceder a la data del token
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			// Instanciamos el algoritmo que va ejecutar la validación del token
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
				signingCredentials: signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
		}
	}
}
