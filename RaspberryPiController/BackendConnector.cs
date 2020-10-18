using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaspberryPiController
{
    public enum MovementType
    {
        Forward,
        Backward
    }

    public enum TurnType
    {
        Left,
        Right
    }

    public class BackendConnector
    {
        private const string LocalUrl = "http://localhost:9876";
        private const string RemoteUrl = "";
        private const string BackendUrl = LocalUrl;
        private static readonly HttpClient Client = new HttpClient();

        public static async Task<bool> Move(int amount, MovementType type)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    {"amount", amount.ToString()},
                    {"type", type.ToString()}
                };
                var content = new FormUrlEncodedContent(values);
                return (await Client.PostAsync($"{BackendUrl}/RaspPi/Move", content)).IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Turn(int amount, TurnType type)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    {"amount", amount.ToString()},
                    {"type", type.ToString()}
                };
                var content = new FormUrlEncodedContent(values);
                return (await Client.PostAsync($"{BackendUrl}/RaspPi/Turn", content)).IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Blink()
        {
            try
            {
                return (await Client.GetAsync($"{BackendUrl}/RaspPi/Blink")).IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
