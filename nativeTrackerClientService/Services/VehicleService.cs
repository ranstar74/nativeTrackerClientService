using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using nativeTrackerClientService.Entities;
using nativeTrackerClientService.Extensions;

namespace nativeTrackerClientService.Services;

[Authorize]
public class VehicleService : nativeTrackerClientService.VehicleService.VehicleServiceBase
{
    public override async Task GetVehicles(
        GetVehiclesRequest request,
        IServerStreamWriter<GetVehiclesResponse> responseStream,
        ServerCallContext context)
    {
        await Task.Run(async () =>
        {
            await using nativeContext db = new();

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

    public override async Task<AddVehicleResponse> AddVehicle(
        AddVehicleRequest request, 
        ServerCallContext context)
    {
        return await Task.Run(async () =>
        {
            await using nativeContext db = new();

            var user = await db.ClientUsers.FindAsync(context.GetUserName());

            user!.Vehicles.Add(new Vehicle()
            {
                Name = request.Name,
                Photo = request.Photo.ToByteArray(),
                ClientLogin = user.Login
            });
            await db.SaveChangesAsync();

            return new AddVehicleResponse();
        });
    }
}