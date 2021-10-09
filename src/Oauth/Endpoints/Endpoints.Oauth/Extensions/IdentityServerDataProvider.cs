using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using ApiResource = IdentityServer4.Models.ApiResource;
using ApiScope = IdentityServer4.Models.ApiScope;
using Client = IdentityServer4.Models.Client;
using Secret = IdentityServer4.Models.Secret;

namespace BookStore.Endpoints.Oauth.Extensions
{
    public static class IdentityServerDataProvider
    {
        public static List<ApiResource> GetApiResources()
        {
            return new()
            {
                new ApiResource("BookStore", "Book Store", new[] { JwtClaimTypes.Name, JwtClaimTypes.FamilyName })
                {
                    ApiSecrets = new List<Secret> { new("resource-secret".Sha256()) },
                    Scopes = new List<string> { "seller", "guest" }
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new()
            {
                new ApiScope("seller", "E-Shop Book Seller"),
                new ApiScope("guest", "E-Shop Book Seller")
            };
        }

        public static List<Client> GetClients()
        {
            return new()
            {
                new Client
                {
                    ClientId = "angular-client",
                    ClientSecrets = new List<Secret> { new("angular-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        "guest",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    IncludeJwtId = false,
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Jwt
                },
                new Client
                {
                    ClientId = "seller-client",
                    ClientSecrets = new List<Secret> { new("seller-secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new List<string>
                    {
                        "seller",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    IncludeJwtId = false,
                    AccessTokenType = AccessTokenType.Jwt
                },
            };
        }
    }
}
