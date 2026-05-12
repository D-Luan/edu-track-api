using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace EduTrack.Api.Extensions;

public class HealthCheckExtensions
{
    public static async Task WriteJsonResponseAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            Status = report.Status.ToString(),
            TotalDuration = $"{report.TotalDuration.TotalMilliseconds} ms",
            Dependencies = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description ?? "No issues detected"
            })
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, response);
    }
}
