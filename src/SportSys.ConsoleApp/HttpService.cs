using Apollo.HttpService.Interface;
using Apollo.HttpService.Model.Response;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SportSys.ConsoleApp.Model;
using SportSys.ConsoleApp.Model.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SportSys.ConsoleApp
{
  public class HttpService : Apollo.HttpService.HttpJsonService
  {
    private readonly IMapyCom config;

    public HttpService(ILoggerFactory loggerFactory, IOptions<MapyCom> config, JsonSerializerOptions? jsonOptions = null) :   base(loggerFactory, config.Value, jsonOptions)
    {
      this.config = config.Value;
    }

    protected override HttpClient CreateHttpClient(string baseUrl)
    {
      throw new NotImplementedException();
    }

    protected override HttpClient CreateHttpClient(IClientConfig clientConfig)
    {
      IMapyCom mapyCfg = (IMapyCom)clientConfig;
      return new HttpClient()
      {
        BaseAddress = new Uri(mapyCfg.BaseUrl)
      };
    }

    protected override ApiResponse ParseErrorResponse(HttpStatusCode httpStatusCode, string response)
    {
      throw new NotImplementedException();
    }

    public async Task<IceRink> Search(string query)
    {
      var qs = new Dictionary<string, string>()
      {
          { "query", query },
          { "lang", "cs" },
          { "limit", "10" },
          { "apikey", config.ApiKey }
      };
      var response = await GetAsync<Root>(config.GeocodeRoute, qs);
      Console.WriteLine(response);
      if (!response.IsValid || response.Response is null)
        throw new InvalidOperationException($"Search request failed: {response}");
      var item = response.Response.Items.FirstOrDefault();
      if (item == null)
        throw new InvalidOperationException($"No results found for query: {query}");
      return new IceRink(
        item.Name,
        item.Label,
        item.Location,
        item.RegionalStructure.FirstOrDefault()?.Name ?? "",
        item.Position.Lat,
        item.Position.Lon
      );
    }
  }
}
