using Common.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Services.Implementation
{
    public class IndependentReserveClient : IIndependentReserveClient
    {
        private readonly IHttpClientFactory httpClientFactory;

        public IndependentReserveClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<RecentTradesResponse> GetRecentTradesAsync(CancellationToken cancellationToken)
        {
            const string url = "https://api.independentreserve.com/Public/GetRecentTrades?primaryCurrencyCode=xbt&secondaryCurrencyCode=aud&numberOfRecentTradesToRetrieve=10";
            using var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.GetFromJsonAsync<RecentTradesResponse>(url, cancellationToken);
            if (response is null)
            {
                throw new InvalidOperationException($"Response from URI {url} is null");
            }

            return response;
        }

        public async IAsyncEnumerable<TradeEvent> SubscribeAndGetTradesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            const string uri = "wss://websockets.independentreserve.com?subscribe=ticker-xbt";
            using var wsClient = new ClientWebSocket();
            await wsClient.ConnectAsync(new Uri(uri), cancellationToken);

            byte[] buf = new byte[1056];

            while (wsClient.State == WebSocketState.Open)
            {
                var result = await wsClient.ReceiveAsync(buf, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken);
                    yield break;
                }
                else
                {
                    var str = Encoding.ASCII.GetString(buf, 0, result.Count);
                    var baseEvent = JsonConvert.DeserializeObject<EventBase>(str);

                    if (baseEvent?.Event == "Trade")
                    {
                        var trade = JsonConvert.DeserializeObject<TradeEvent>(str, new JsonSerializerSettings
                        {
                            DateTimeZoneHandling = DateTimeZoneHandling.Utc
                        });

                        if (trade is null)
                        {
                            throw new InvalidOperationException($"Cannot deserialize JSON into {nameof(TradeEvent)}, json: {str}");
                        }

                        yield return trade;
                    }
                }
            }
        }
    }
}
