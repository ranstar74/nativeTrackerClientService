using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using nativeTrackerClientService.Extensions;

namespace nativeTrackerClientService.Services;

public class VehicleService : nativeTrackerClientService.VehicleService.VehicleServiceBase
{
    [Authorize]
    public override Task GetVehicles(
        GetVehiclesRequest request,
        IServerStreamWriter<GetVehiclesResponse> responseStream,
        ServerCallContext context)
    {
        return Task.FromResult(async () =>
        {
            await using Entities.nativeContext db = new();

            var user = await db.ClientUsers.FindAsync(context.GetUserName());
            foreach (var vehicle in user!.Vehicles)
            {
                await responseStream.WriteAsync(new GetVehiclesResponse()
                {
                    VehicleHandle = vehicle.ID,
                    Name = vehicle.Name,
                    Photo = ByteString.CopyFrom(vehicle.Photo)
                });
            }
        });
    }
}