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
        public KodiConnection()
        {
            Id = Guid.NewGuid();

            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            Name = resourceLoader.GetString("NewConnection");
            Status = ConnectionStatus.Pending;
        }

        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Version { get; set; }

        public bool IsOnline { get; set; }

        public ConnectionStatus Status { get; set; }

        [JsonProperty]
        public bool IsDefault { get; set; }

        [JsonProperty]
        public Connection Kodi { get; set; }

        private bool _askStop;

        private async void StartTestingLoop()
        {
            if (Kodi == null) return;

            if (Kodi.IsMocked)
            {
                IsOnline = true;
                Status = ConnectionStatus.Online;
                Name = "Kodi server";
                Version = "Version 15";
                return;
            }

            do
            {
                Status = ConnectionStatus.Pending;
                try
                {
                    IsOnline = await Kodi.JsonRpc.PingAsync();
                    Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;

                    await Task.Delay(4000);
                    continue;
                }
                catch { }

                await Task.Delay(4000);

            } while (!_askStop);
        }

        private void StopTestingLoop()
        {
            _askStop = true;
        }

        public async Task<bool> TestConnectionAsync()
        {

            Status = ConnectionStatus.Pending;

            const string labelFriendlyName = "System.FriendlyName";
            const string labelBuildVersion = "System.BuildVersion";

            IsOnline = await Kodi.JsonRpc.PingAsync();
            Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;
            if (!IsOnline) return false;

            var labels = await Kodi.Server.GetInfoLabelsAsync(labelFriendlyName, labelBuildVersion);
            Name = labels[labelFriendlyName];
            Version = labels[labelBuildVersion];

            return true;
        }

        //#region INotifyPropertyChanged Members

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion
    }
}
