using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using Riptide;

namespace DiceGuardiansClient.Source.GameStates; 

public static class StateManager {
    private static State _currentState;

    public static void Start(DisplayManager displayManager) {
        _currentState = new LoginScreen(displayManager);
    }

    public static State GetState() {
        return _currentState;
    }

    public static void SetState(State state) {
        _currentState = state;
    }

    [MessageHandler((ushort) ServerToClientId.InvalidLoginUserDoesNotExist)]
    private static void InvalidLoginUserDoesNotExist(Message m) {
        if (_currentState is LoginScreen screen) {screen.TriggerUserDoesNotExistError();}
    }

    [MessageHandler((ushort)ServerToClientId.InvalidLoginWrongPassword)]
    private static void InvalidLoginWrongPassword(Message m) {
        if (_currentState is LoginScreen screen) {screen.TriggerInvalidPasswordError();}
    }

    [MessageHandler((ushort)ServerToClientId.SuccessfulLogin)]
    private static void SuccessfulLogin(Message m) {
        if (_currentState is LoginScreen screen) {screen.TriggerSuccessfulLogin(m);}
    }

    [MessageHandler((ushort)ServerToClientId.InvalidRegisterUsernameAlreadyTaken)]
    private static void InvalidRegisterUsernameAlreadyTaken(Message m) {
        if (_currentState is LoginScreen screen) {screen.TriggerUsernameAlreadyTakenError();}
    }

    [MessageHandler((ushort)ServerToClientId.SuccessfulRegister)]
    private static void SuccessfulRegister(Message m) {
        if (_currentState is LoginScreen screen) {screen.TriggerSuccessfulRegister(m);}
    }
}