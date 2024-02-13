using Microsoft.AspNetCore.Authorization;

namespace Stushbr.AdminUtilsWeb.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AuthorizeAdminAttribute() : AuthorizeAttribute("Admin");