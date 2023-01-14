using Riptide;

namespace DiceGuardiansClient.Source.Networking; 

public static class NetworkManager {
    private const string HostAddress = "127.0.0.1:7777";
    private static Client _client;

    public static void Start() {
        _client = new Client();
        _client.Connect(HostAddress);
    }

    public static void Update() {
        _client.Update();
    }

    public static Client GetClient() {
        return _client;
    }
}