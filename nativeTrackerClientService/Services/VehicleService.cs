using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace nativeTrackerClientService.Services;

public class VehicleService : nativeTrackerClientService.VehicleService.VehicleServiceBase
{
    [Authorize]
    public override Task GetVehicles(
        GetVehiclesRequest request,
        IServerStreamWriter<GetVehiclesResponse> responseStream,
        ServerCallContext context)
    {
        return Task.FromResult(() =>
        {
            using Entities.nativeContext db = new();

            foreach (var vehicle in db.Vehicles)
            {
                responseStream.WriteAsync(new GetVehiclesResponse()
                {
                    VehicleHandle = vehicle.ID,
                    Name = vehicle.Name,
                    Photo = ByteString.CopyFrom(vehicle.Photo)
                });
            }
        });
    }
}