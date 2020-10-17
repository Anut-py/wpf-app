using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyFirstWpfApp
{
    public enum MovementType
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    }

    public class BackendConnector
    {
        private const string LOCAL_URL = "http://localhost:9876";
        private const string REMOTE_URL = "";
        private const string BACKEND_URL = LOCAL_URL;
        private static readonly HttpClient client = new HttpClient();

        public static async Task<bool> move(int degrees, MovementType type)
        {
            var values = new Dictionary<string, string>
            {
                {"degrees", degrees.ToString()},
                {"type", type.ToString()}
            };
            var content = new FormUrlEncodedContent(values);
            return (await client.PostAsync($"{BACKEND_URL}/RaspPi/MoveWheels", content)).IsSuccessStatusCode;
        }

        public static async Task<bool> blink()
        {
            return (await client.GetAsync($"{BACKEND_URL}/RaspPi/Blink")).IsSuccessStatusCode;
        }
    }
}