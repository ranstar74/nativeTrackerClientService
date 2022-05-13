using Grpc.Core;

namespace nativeTrackerClientService.Services;

public class TrackingService : VehicleTrackService.VehicleTrackServiceBase
{
    private readonly ILogger<TrackingService> _logger;
    public TrackingService(ILogger<TrackingService> logger)
    {
        _logger = logger;
    }
    
    public override async Task Subscribe(
        VehicleTrackRequest request, 
        IServerStreamWriter<VehicleTrackUpdate> responseStream, 
        ServerCallContext context)
    {
        _logger.LogInformation($"{request.VehicleHandle} subscribed.");

        try
        {
            Random rand = new();

            for (int i = 0; i < rand.Next(10, 20); i++)
            {
                await Task.Delay(rand.Next(500, 5000));
                await responseStream.WriteAsync(new VehicleTrackUpdate()
                {
                    Longitude = (float)rand.NextDouble() * 360 - 180,
                    Latitude = (float)rand.NextDouble() * 170 - 85,
                });
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to send update: {e.Message}");
        }
    }
}