using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

    public class Position
    {
        public Position(double xPos, double yPos, double rotationDegrees)
        {
            XPos = xPos;
            YPos = yPos;
            RotationDegrees = rotationDegrees;
        }

        public double XPos { get; }
        public double YPos { get; }
        public double RotationDegrees { get; }
    }

    public static class BackendConnector
    {
        private const string LocalUrl = "http://localhost:9876";
        private const string RemoteUrl = "";
        private const string BackendUrl = LocalUrl;
        private static readonly HttpClient Client = new HttpClient();
        private static Position previous { get; set; }
        
        public static async Task<bool> Move(double amount, MovementType type)
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
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> Turn(double amount, TurnType type)
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
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> Blink()
        {
            try
            {
                return (await Client.PostAsync($"{BackendUrl}/RaspPi/Blink", null)).IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<Position> GetPosition()
        {
            try
            {
                previous = new Position(
                    Convert.ToDouble(await (await Client.GetAsync($"{BackendUrl}/RaspPi/Position/XPos")).Content
                        .ReadAsStringAsync()),
                    Convert.ToDouble(await (await Client.GetAsync($"{BackendUrl}/RaspPi/Position/YPos")).Content
                        .ReadAsStringAsync()),
                    Convert.ToDouble(await (await Client.GetAsync($"{BackendUrl}/RaspPi/Position/Rotation")).Content
                        .ReadAsStringAsync()));
                return previous;
            }
            catch (Exception)
            {
                return previous;
            }
        }
    }
}