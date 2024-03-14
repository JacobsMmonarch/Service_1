using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Service_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NetworkController : ControllerBase
    {
        [HttpGet("Info")] // Измененный путь
        public IActionResult GetNetworkInfo()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var networkInfo = new List<NetworkInfo>();

            foreach (var networkInterface in networkInterfaces)
            {
                var ipProps = networkInterface.GetIPProperties();
                var networkAddresses = ipProps.UnicastAddresses.Select(a => a.Address.ToString()).ToList();

                var info = new NetworkInfo
                {
                    InterfaceName = networkInterface.Name,
                    IsUp = networkInterface.OperationalStatus == OperationalStatus.Up,
                    NetworkAddresses = networkAddresses,
                    Speed = networkInterface.Speed
                };

                networkInfo.Add(info);
            }

            return Ok(networkInfo);
        }
    }

    public class NetworkInfo
    {
        public string InterfaceName { get; set; }
        public bool IsUp { get; set; }
        public List<string> NetworkAddresses { get; set; }
        public long Speed { get; set; }
    }
}
