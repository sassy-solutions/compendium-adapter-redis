// -----------------------------------------------------------------------
// <copyright file="EnvironmentConfigurationHelper.cs" company="Sassy Solutions">
//     Copyright (c) 2026 Sassy Solutions. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Compendium.IntegrationTests.Infrastructure;

/// <summary>
/// Helper for Redis test infrastructure configuration with fallback strategy:
/// 1. Environment variable (CI/CD or manual override)
/// 2. Docker Compose local (localhost:6379)
/// 3. TestContainers (automatic, used when above are unavailable)
/// </summary>
public static class EnvironmentConfigurationHelper
{
    /// <summary>
    /// Gets the Redis connection string using fallback strategy. Returns empty
    /// when no external Redis is available — the caller should fall back to TestContainers.
    /// </summary>
    public static string GetRedisConnectionString()
    {
        var envConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(envConnectionString))
        {
            Console.WriteLine("✅ Using Redis from environment variable");
            return envConnectionString;
        }

        var dockerConnectionString = "localhost:6379";
        if (IsRedisAvailable(dockerConnectionString))
        {
            Console.WriteLine("✅ Using Redis from Docker Compose (localhost:6379)");
            return dockerConnectionString;
        }

        Console.WriteLine("⚠️ No local Redis found. TestContainers will be used.");
        return string.Empty;
    }

    private static bool IsRedisAvailable(string connectionString)
    {
        try
        {
            var redis = StackExchange.Redis.ConnectionMultiplexer.Connect(connectionString);
            var pingResult = redis.GetDatabase().Ping();
            redis.Dispose();
            return pingResult.TotalMilliseconds > 0;
        }
        catch
        {
            return false;
        }
    }
}
