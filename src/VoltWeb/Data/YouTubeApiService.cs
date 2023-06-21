using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace VoltWeb.Data;

public sealed class YouTubeApiService
{
    private readonly YouTubeService youTubeService;
    
    public YouTubeApiService(YouTubeService youTubeService)
    {
        this.youTubeService = youTubeService;
    }

    public async Task<YouTubeVideo?> GetChannelLatestVideo(string channelId)
    {
        ChannelsResource.ListRequest? channelRequest = youTubeService.Channels.List("contentDetails");
        channelRequest.Id = channelId;

        ChannelListResponse? result = await channelRequest.ExecuteAsync();
        if (result == null || result.Items.Count == 0)
            return null;
        
        string? uploads = result.Items[0].ContentDetails.RelatedPlaylists.Uploads;
        if (uploads == null)
            return null;

        PlaylistItemsResource.ListRequest? playlistRequest = youTubeService.PlaylistItems.List("snippet");
        playlistRequest.PlaylistId = uploads;
        playlistRequest.MaxResults = 1;

        PlaylistItemListResponse? playlistResult = await playlistRequest.ExecuteAsync();
        if (playlistResult == null || playlistResult.Items.Count == 0)
            return null;

        return new YouTubeVideo()
        {
            VideoId = playlistResult.Items[0].Snippet.ResourceId.VideoId
        };
    }
}