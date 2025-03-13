using backend.DTO;
using backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services;

/// <summary>
/// Service to handle asset operations.
/// </summary>
public class InfoUpateService
{
    private readonly IHubContext<UsagesOfAssetHub> _hubContext;

    public InfoUpateService(IHubContext<UsagesOfAssetHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    /// Send Updated Usages Info to Clients in the Group
    /// </summary>
    /// <param name="groupName">The group Name to send updated Info </param>
    /// <param name="updatedInfo">The updated usage detail info</param>
    public async Task SendUsageUpdateInfo(string groupName, UsageDetailDTO updatedInfo)
    {
        await _hubContext.Clients.Group(groupName).SendAsync("ReceiveUpdatedInfo", updatedInfo);
    }
}