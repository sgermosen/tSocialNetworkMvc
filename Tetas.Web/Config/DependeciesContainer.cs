using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tetas.Repositories.Contracts;
using Tetas.Repositories.Implementations;
using Tetas.Web.Helpers;

public static class DependeciesContainer
{
    public static void AddMyDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {

        #region Current User
        //services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHttpContextAccessor();
        services.AddTransient<ICurrentUserFactory, CurrentUserFactory>();
        #endregion

        #region RepositoryScopes

        //services.AddScoped<IPostComment, PostCommentRepository>();
        services.AddScoped<IPost, PostRepository>();
        services.AddScoped<IGroup, GroupRepository>();

        #endregion
        #region My services
        services.AddScoped<IPsSelectList, PsSelectList>();
        services.AddScoped<IUserHelper, UserHelper>();
        services.AddScoped<IMailHelper, MailHelper>();
        #endregion
    }
}
