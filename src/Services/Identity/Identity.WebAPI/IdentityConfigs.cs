using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;
using System.Text.Json;

namespace Identity.WebAPI;

public static class IdentityConfigs
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "Street",
                locality = "Samsun",
                postal_code = 55400,
                country = "Türkiye"
            };

            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "285561",
                    Username = "admin_usr",
                    Password = "admin_pwd",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Enes Özcan"),
                        new Claim(JwtClaimTypes.GivenName, "Enes"),
                        new Claim(JwtClaimTypes.FamilyName, "Özcan"),
                        new Claim(JwtClaimTypes.Email, "enes.ozcan.55@gmail.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.WebSite, "https://localhost"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }

    public static IEnumerable<IdentityResource> IdentityResources =>
       new[]
       {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
       };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("weather.api.read"),
            new ApiScope("weather.api.write"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("weather.api")
            {
                Scopes = new List<string> {"weather.api.read", "weather.api.write"},
                ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                UserClaims = new List<string> {"role"}
            }
        };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            // weather.api.client
            new Client
            {
                ClientId = "weather.api.client",
                ClientName = "Weather API Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
                AllowedScopes = {"weather.api.read", "weather.api.write"}
            },
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:7040/signin-oidc"},
                FrontChannelLogoutUri = "https://localhost:7040/signout-oidc",
                PostLogoutRedirectUris = {"https://localhost:7040/signout-callback-oidc"},
                AllowOfflineAccess = true,
                AllowedScopes = {"openid", "profile", "weather.api.read"},
                RequirePkce = true,
                RequireConsent = true,
                AllowPlainTextPkce = false
            },
        };
}