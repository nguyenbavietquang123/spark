using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spark.Web.Models;
using Spark.Web.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public class AuthorizeFhirAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _requiredPermission;

    public AuthorizeFhirAttribute(string requiredPermission)
    {
        _requiredPermission = requiredPermission;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var introspectSettings = httpContext.RequestServices.GetService<IOptions<IntrospectSettings>>()?.Value;
        Console.WriteLine("Go to OnAuthorization");
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authHeader.Split(' ').Last();

        string verifyError = FhirAuth.verifyAccessToken(token, introspectSettings);
        if (!string.IsNullOrEmpty(verifyError))
        {
            var outcome = FhirFileImport.ImportData(verifyError).First();
            context.Result = new ObjectResult(outcome) { StatusCode = (int)HttpStatusCode.Unauthorized };
            return;
        }

        string permissionError = FhirAuth.checkPermission(token, _requiredPermission);
        if (!string.IsNullOrEmpty(permissionError))
        {
            var outcome = FhirFileImport.ImportData(permissionError).First();
            context.Result = new ObjectResult(outcome) { StatusCode = (int)HttpStatusCode.Unauthorized };
            return;
        }

        await Task.CompletedTask;
    }

    // Task IAsyncAuthorizationFilter.OnAuthorizationAsync(AuthorizationFilterContext context) => throw new NotImplementedException();
}
