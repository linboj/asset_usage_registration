using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

/// <summary>
/// Hub used in updating usages of assets
/// </summary>
public class UsagesOfAssetHub : Hub
{

    /// <summary>
    /// Join to group based on group name (assetId)
    /// </summary>
    /// <param name="assetId">The group name (assetId)</param>
    public async Task JoinToGroup(string assetId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, assetId);
        await Clients.Caller.SendAsync("JoinToGroup", $"You have joined the group: {assetId}");
    }


    /// <summary>
    /// Remove from group based on group name (assetId)
    /// </summary>
    /// <param name="assetId">The group name (assetId)</param>
    public async Task RemoveFromGroup(string assetId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, assetId);
    }
}