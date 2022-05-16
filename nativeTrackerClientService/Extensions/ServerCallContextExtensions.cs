using Grpc.Core;

namespace nativeTrackerClientService.Extensions;

public static class ServerCallContextExtensions
{
    public static string GetUserName(this ServerCallContext context)
    {
        return context.GetHttpContext().User.Identity!.Name!;
    }
}