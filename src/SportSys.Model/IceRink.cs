namespace SportSys.Model;

public record class IceRink(
  string Name,
  string Street,
  string ZipCode,
  string City,
  double Lat,
  double Lon
);
