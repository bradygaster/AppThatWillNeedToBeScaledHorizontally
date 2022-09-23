using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using Orleans.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace AppThatWillNeedToBeScaledHorizontally
{
    public static class OrleansDistributedStateExtensions
    {
        public static IServiceCollection AddDefaultDistributedProviders(this IServiceCollection services,
            Action<DistributedServiceEnlistmentOptions, ISiloBuilder> options)
        {
            return services;
        }

        public static IServiceCollection AddDefaultDistributedProviders(this IServiceCollection services, 
            Action<DistributedServiceEnlistmentOptions> options)
        {
            return services;
        }

        public static IServiceCollection AddDefaultDistributedProviders(this IServiceCollection services)
        {
            // distributed caching and distributed session
            services.AddSingleton<IDistributedCache, OrleansDistributedCacheProvider>();
            services.AddDistributedMemoryCache();

            // session object storage in orleans 
            services.AddSession();
            services.TryAddSingleton<ISessionStore, OrleansSessionStore>();

            // backplaning SignalR Hubs
            services.TryAddSingleton(typeof(HubLifetimeManager<>), typeof(OrleansHubLifetimeManager<>));

            // output caching
            services.TryAddSingleton<IOutputCacheStore, OrleansOutputCacheStore>();

            // identity store services
            services.TryAddSingleton(typeof(RoleStoreBase<,,,>), typeof(OrleansRoleStore<,,,>));
            services.TryAddSingleton(typeof(UserStoreBase<,,,,>), typeof(OrleansUserStore<,,,,>));

            // feature management
            services
                .AddSingleton<ISessionManager, OrleansFeatureFlagSessionManager>()
                .AddSingleton<IFeatureDefinitionProvider, OrleansFeatureFlagDefinitionProvider>()
                    .AddFeatureManagement();

            return services;
        }
    }

    public class DistributedServiceEnlistmentOptions
    {
        public bool DistributedCache { get; set; }
        public bool FeatureManagement { get; set; }
        public bool Identity { get; set; }
        public bool OutputCaching { get; set; }
        public bool HubLifetimeManager { get; set; }
    }

    public class OrleansDistributedCacheProvider : IDistributedCache
    {
        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }

    public class OrleansSessionStore : ISessionStore
    {
        public ISession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            throw new NotImplementedException();
        }
    }

    public class OrleansHubLifetimeManager<T> : HubLifetimeManager<T> where T : Hub
    {
        public override Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task OnConnectedAsync(HubConnectionContext connection)
        {
            throw new NotImplementedException();
        }

        public override Task OnDisconnectedAsync(HubConnectionContext connection)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendAllAsync(string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendAllExceptAsync(string methodName, object?[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendConnectionAsync(string connectionId, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendConnectionsAsync(IReadOnlyList<string> connectionIds, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendGroupAsync(string groupName, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendGroupExceptAsync(string groupName, string methodName, object?[] args, IReadOnlyList<string> excludedConnectionIds, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendGroupsAsync(IReadOnlyList<string> groupNames, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendUserAsync(string userId, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SendUsersAsync(IReadOnlyList<string> userIds, string methodName, object?[] args, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    public class OrleansOutputCacheStore : IOutputCacheStore
    {
        public ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class OrleansRoleStore<TRole, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TKey, TUserRole, TRoleClaim> :
        RoleStoreBase<TRole, TKey, TUserRole, TRoleClaim>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where TUserRole : IdentityUserRole<TKey>, new()
            where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        protected OrleansRoleStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override IQueryable<TRole> Roles => throw new NotImplementedException();

        public override Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override TKey? ConvertIdFromString(string? id)
        {
            return base.ConvertIdFromString(id);
        }

        public override string? ConvertIdToString(TKey id)
        {
            return base.ConvertIdToString(id);
        }

        public override Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override Task<TRole?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<TRole?> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Task<string?> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
        {
            return base.GetNormalizedRoleNameAsync(role, cancellationToken);
        }

        public override Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
        {
            return base.GetRoleIdAsync(role, cancellationToken);
        }

        public override Task<string?> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
        {
            return base.GetRoleNameAsync(role, cancellationToken);
        }

        public override Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task SetNormalizedRoleNameAsync(TRole role, string? normalizedName, CancellationToken cancellationToken = default)
        {
            return base.SetNormalizedRoleNameAsync(role, normalizedName, cancellationToken);
        }

        public override Task SetRoleNameAsync(TRole role, string? roleName, CancellationToken cancellationToken = default)
        {
            return base.SetRoleNameAsync(role, roleName, cancellationToken);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override TRoleClaim CreateRoleClaim(TRole role, Claim claim)
        {
            return base.CreateRoleClaim(role, claim);
        }
    }

    public abstract class OrleansUserStore<TUser, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TKey, TUserClaim, TUserLogin, TUserToken> :
        UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
            where TUserClaim : IdentityUserClaim<TKey>, new()
            where TUserLogin : IdentityUserLogin<TKey>, new()
            where TUserToken : IdentityUserToken<TKey>, new()
    {
        protected OrleansUserStore(IdentityErrorDescriber describer) : base(describer)
        {
        }
    }

    public class OrleansFeatureFlagSessionManager : ISessionManager
    {
        public Task<bool?> GetAsync(string featureName)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string featureName, bool enabled)
        {
            throw new NotImplementedException();
        }
    }

    public class OrleansFeatureFlagDefinitionProvider : IFeatureDefinitionProvider
    {
        public IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            throw new NotImplementedException();
        }
    }
}
