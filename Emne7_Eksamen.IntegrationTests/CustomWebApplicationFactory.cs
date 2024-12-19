using Emne7_Eksamen.Features.Members.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Emne7_Eksamen.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IMemberRepository> MemberRepositoryMock { get; set; }
    public Mock<IMemberService> MemberServiceMock { get; set; }
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    
    public CustomWebApplicationFactory()
    {
        MemberRepositoryMock = new Mock<IMemberRepository>();    
        MemberServiceMock = new Mock<IMemberService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserId"] = "1";
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }
    

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(MemberRepositoryMock.Object);
            
            services.AddSingleton(_httpContextAccessorMock.Object);
        });
    }
}