using System.Text;
using System.Text.RegularExpressions;
using Emne7_Eksamen.Configurations;
using Emne7_Eksamen.Features.Members.Interfaces;
using Microsoft.Extensions.Options;

namespace Emne7_Eksamen.Middleware;

public class GokstadAthleticsBasicAuthentication : IMiddleware
{
    private readonly ILogger<GokstadAthleticsBasicAuthentication> _logger;
    private readonly IMemberService _memberService;
    private readonly List<Regex> _excludePatterns;

    public GokstadAthleticsBasicAuthentication(
        ILogger<GokstadAthleticsBasicAuthentication> logger,
        IMemberService memberService,
        IOptions<BasicAuthenticationOptions> options)
    {
        _logger = logger;
        _memberService = memberService;
        
        _excludePatterns = options.Value.ExcludePatterns
            .Select(p => new Regex(p)).ToList();
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var regex in _excludePatterns)
        {
            if (regex.IsMatch(context.Request.Path))
            {
                await next(context);
                return;
            }   
        }
        
        string authHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authHeader))
        {
            _logger.LogWarning("Authentication header is empty");
            throw new UnauthorizedAccessException("Authentication header is empty");
        }
        
        // sjekk om authorization header starter med "Basic"
        if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Authentication header is incorrect");
            throw new UnauthorizedAccessException("Authentication header is incorrect");
        }
        
        SplitString(authHeader, " ", out string basic, out string base64String);
        if (string.IsNullOrWhiteSpace(base64String) || string.IsNullOrWhiteSpace(basic))
        {
            _logger.LogWarning("Authentication header is empty, log in.");
            throw new UnauthorizedAccessException("Authentication header is empty, log in.");
        }
        
        // Decode base64-string -> memberId og passord
        int memberId;
        string password;
        try
        {
            // username:password
            string memberIdPassword = ExtractBase64String(base64String);
            SplitString(memberIdPassword, 
                ":", 
                out var memberIdString, 
                out password);

            if (string.IsNullOrWhiteSpace(memberIdString) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Missing memberId and/or password");
                throw new UnauthorizedAccessException("Missing memberId and/or password");
            }

            if (!int.TryParse(memberIdString, out memberId))
            {
                _logger.LogWarning("MemberId is not a valid integer");
                throw new UnauthorizedAccessException("MemberId must be a valid integer");
            }
            
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Authentication header is incorrect");
            throw new UnauthorizedAccessException("Authentication header is incorrect", e);
        }
        
        // nå har vi username og passord !!
        int? userId = await _memberService.AuthenticateMemberAsync(memberId, password);
        if (userId == null)
        {
            _logger.LogWarning("memberId or password is incorrect");
            throw new UnauthorizedAccessException("memberId or password is incorrect");
        }
        
        // Kommer vi hit har vi en gyldig bruker! Vi får bruk for denne id når vi authorize
        context.Items["UserId"] = userId.ToString();
        
        
        // Går videre til neste middleware !!
        await next(context);
    }
    
    private string ExtractBase64String(string base64String)
    {
        var base64Bytes = Convert.FromBase64String(base64String);
        var userNamePassword = Encoding.UTF8.GetString(base64Bytes);
        return userNamePassword;
    }

    private void SplitString(string authHeader, string separator, out string left, out string right)
    {
        left = right = string.Empty; 
        var arr = authHeader.Split(separator);
        if (arr is [var a, var b])
        {
            left = a;
            right = b;
        }
    }
}