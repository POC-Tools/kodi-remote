using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KodiRemote.Core;
using Newtonsoft.Json;
using Windows.ApplicationModel.Resources;

namespace KodiRemote.Uwp.Core
{
    [JsonObject]
    public class KodiConnection //: INotifyPropertyChanged
    {
        public KodiConnection() => Status = ConnectionStatus.Pending;


        public KodiConnection(string login, string password, string address, string port, string macAddress)
        {
            Kodi = new Connection(address, port, login, password, macAddress);
        }

        //[JsonProperty]
        //public Guid Id { get; set; }

        //[JsonProperty]
        //public string Name { get; set; }

        [JsonProperty]
        public string Version { get; set; }

        [JsonProperty]
        public bool IsDefault { get; set; }

        [JsonProperty]
        public Connection Kodi { get; set; }

        public bool IsOnline { get; private set; }

        public ConnectionStatus Status { get; private set; }

        public async Task<string> GetName()
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            string name = resourceLoader.GetString("NewConnection");
            try
            {
                const string labelBuildVersion = "System.BuildVersion";
                const string labelFriendlyName = "System.FriendlyName";
                var labels = await Kodi.Server.GetInfoLabelsAsync(labelFriendlyName, labelBuildVersion);
                name = string.IsNullOrEmpty(labels[labelFriendlyName]) ? name : labels[labelFriendlyName];
                Version = labels[labelBuildVersion];
            }
            catch { }
            return name;
        }


        public async Task<bool> TestConnectionAsync()
        {
            Status = ConnectionStatus.Pending;
            try
            {
                IsOnline = await Kodi.JsonRpc.PingAsync();
                Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;
                if (!IsOnline) return false;
            }
            finally { }
            return true;
        }


    }
}
