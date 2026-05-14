using System;
using System.Collections.Generic;
using System.Text;

namespace SportSys.ConsoleApp.Model.Config;

public interface IMapyCom: Apollo.HttpService.Interface.IClientConfig
{
  string ApiKey { get; }
  string BaseUrl { get; }
  string GeocodeRoute { get;  }
}

//public record class MapyCom(string ApiKey, string BaseUrl, string GeocodeRoute) : IMapyCom;

public record class MapyCom : IMapyCom
{
  public string ApiKey { get; init; } = "";
  public string BaseUrl { get; init; } = "";
  public string GeocodeRoute { get; init; } = "";
}
  