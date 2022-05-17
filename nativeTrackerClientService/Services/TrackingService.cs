using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using nativeTrackerClientService.Entities;

namespace nativeTrackerClientService.Services;

[Authorize]
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

            for (int i = 0; i < rand.Next(4000, 5000); i++)
            {
                await Task.Delay(rand.Next(5, 100));
                await responseStream.WriteAsync(new VehicleTrackUpdate()
                {
                    Longitude = (float)rand.NextDouble() * 360 - 180,
                    Latitude = (float)rand.NextDouble() * 170 - 85,
                    Time = Timestamp.FromDateTime(DateTime.UtcNow)
                });
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to send update: {e.Message}");
        }
    }

    public override async Task<GetTrackModelFeaturesResponse> GetTrackModelsFeatures(
        GetTrackModelFeaturesRequest request,
        ServerCallContext context)
    {
        return await Task.Run(() =>
        {
            using nativeContext db = new();

            return new GetTrackModelFeaturesResponse()
            {
                Features =
                {
                    db.Features.Select(x => x.Name)
                }
            };
        });
    }

    public override async Task<GetTrackModelManufacturersResponse> GetTrackModelsManufacturers(
        GetTrackModelManufacturersRequest request,
        ServerCallContext context)
    {
        return await Task.Run(() =>
        {
            using nativeContext db = new();

            return new GetTrackModelManufacturersResponse()
            {
                Manufacturers =
                {
                    db.Manufacturers.Select(x => x.Name)
                }
            };
        });
    }

    public override async Task GetTrackModels(GetTrackModelsRequest request,
        IServerStreamWriter<GetTrackModelResponse> responseStream, ServerCallContext context)
    {
        await Task.Run(async () =>
        {
            await using nativeContext db = new();

            IEnumerable<Model> models = db.Models;

            // Filters

            if (!string.IsNullOrEmpty(request.Search))
            {
                models = models.Where(x =>
                    x.Name.Contains(request.Search) ||
                    x.Description.Contains(request.Search));
            }

            if (request.Features.Count > 0)
            {
                models = models.Where(x =>
                    x.Features.Select(f => f.Name).Intersect(request.Features).Any());
            }

            if (request.Manufacturers.Count > 0)
            {
                models = models.Where(x => request.Manufacturers.Contains(x.Manufacturer.Name));
            }

            models = models.Where(
                x => (double)x.Price >= request.MinPrice && (double)x.Price <= request.MaxPrice);

            // Pagination

            models = models
                .Skip(request.Page * request.ElementsOnPage)
                .Take(request.ElementsOnPage);

            foreach (var model in models)
            {
                await responseStream.WriteAsync(new GetTrackModelResponse()
                {
                    Name = model.Name,
                    Price = (double)model.Price,
                    Description = model.Description,
                    Manufacturer = model.Manufacturer.Name,
                });
            }
        });
    }

    public override async Task<GetTrackModelPriceRangeResponse> GetTrackModelPriceRange(
        GetTrackModelPriceRangeRequest request, ServerCallContext context)
    {
        return await Task.Run(() =>
        {
            using nativeContext db = new();

            return new GetTrackModelPriceRangeResponse()
            {
                MinPrice = (double)db.Models.Min(x => x.Price),
                MaxPrice = (double)db.Models.Max(x => x.Price),
            };
        });
    }
}