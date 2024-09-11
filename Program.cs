using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IT_Hardware.Infra;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

var initialScopes =new string[] { Constants1.ScopeUserRead, Constants1.ScopeGroupMemberRead };

CacheSettings cacheSettings = new CacheSettings
{
    SlidingExpirationInSeconds = builder.Configuration
                                .GetValue<string>("CacheSettings:SlidingExpirationInSeconds"),
    AbsoluteExpirationInSeconds = builder.Configuration
                                .GetValue<string>("CacheSettings:AbsoluteExpirationInSeconds")
};

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite
    options.HandleSameSiteCookieCompatibility();

});

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
        {
            // Ensure default token validation is carried out
            builder.Configuration.Bind("AzureAd", options);

            // The following lines code instruct the asp.net core middleware to use the data in the "roles" claim in the [Authorize] attribute, policy.RequireRole() and User.IsInRole()
            // See https://docs.microsoft.com/aspnet/core/security/authorization/roles for more info.
            options.TokenValidationParameters.RoleClaimType = "groups";

            /// <summary>
            /// Below you can do extended token validation and check for additional claims, such as:
            ///
            /// - check if the caller's tenant is in the allowed tenants list via the 'tid' claim (for multi-tenant applications)
            /// - check if the caller's account is homed or guest via the 'acct' optional claim
            /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
            ///
            /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
            /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
            /// </summary>

            options.Events.OnTokenValidated = async context =>
            {
                if (context != null)
                {
                    List<string> requiredGroupsIds = builder.Configuration.GetSection("AzureAd:Groups")
                                    .AsEnumerable().Select(x => x.Value).Where(x => x != null).ToList();

                    // Calls method to process groups overage claim (before policy checks kick-in)
                    await GraphHelper.ProcessAnyGroupsOverage(context, requiredGroupsIds, cacheSettings);
                }

                await Task.CompletedTask;
            };
        })
    .EnableTokenAcquisitionToCallDownstreamApi(options => builder.Configuration.Bind("AzureAd", options), initialScopes)
    .AddMicrosoftGraph(builder.Configuration.GetSection("GraphAPI"))
    .AddInMemoryTokenCaches();

// Adding authorization policies that enforce authorization using Azure AD security groups.
builder.Services.AddAuthorization(options =>
{
    // this policy stipulates that users in Chapter Office can access resources
    options.AddPolicy(AuthorizationPolicies.Chapter, policy => policy.RequireRole(builder.Configuration["Groups:Chapter"], builder.Configuration["Groups:ITHardwareManager"]));

    // this policy stipulates that users in ROs Office can access resources
    options.AddPolicy(AuthorizationPolicies.ROsGroup, policy => policy.RequireRole( builder.Configuration["Groups:ROs"], builder.Configuration["Groups:ITHardwareManager"]));

    // this policy stipulates that users in IT Staffs can access resources
    options.AddPolicy(AuthorizationPolicies.ITStaff, policy => policy.RequireRole( builder.Configuration["Groups:ITStaff"]));

    // this policy stipulates that users in IT Hardware Staffs can access resources
    options.AddPolicy(AuthorizationPolicies.ITSupportEngineer, policy => policy.RequireRole(builder.Configuration["Groups:ITSupportEngineer"], builder.Configuration["Groups:ITHardwareManager"]));

    // this policy stipulates that users in IT Hardware Staffs can access resources
    options.AddPolicy(AuthorizationPolicies.ITHardwareManager, policy => policy.RequireRole( builder.Configuration["Groups:ITHardwareManager"]));

});

builder.Services.AddMvcCore();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// The following flag can be used to get more descriptive errors in development environments
// Enable diagnostic logging to help with troubleshooting. For more details, see https://aka.ms/IdentityModel/PII.
// You might not want to keep this following flag on for production
IdentityModelEventSource.ShowPII = true;

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddRazorPages();

// Add the UI support to handle claims challenges
builder.Services.AddServerSideBlazor()
   .AddMicrosoftIdentityConsentHandler();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Chapter_Hardware",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Log_In}/{action=Log_In}/{id?}");
app.MapRazorPages();

app.Run();
