namespace DiceGuardiansClient.Source.Networking; 

public enum ClientToServerId : ushort {
    Login = 1,
    Register = 2,
    GetCollection = 3,
    StartMatchmaking = 4,
    CancelMatchmaking = 5,
    SelectDiceRoll = 6
}