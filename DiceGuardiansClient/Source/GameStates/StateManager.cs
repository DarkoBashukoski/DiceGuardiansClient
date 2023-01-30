using DiceGuardiansClient.Source.Networking;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.World;
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
    
    [MessageHandler((ushort)ServerToClientId.SuccessfulGetCollectionResponse)]
    private static void SuccessfulGetCollectionResponse(Message m) {
        if (_currentState is Collection screen) {screen.TriggerSuccessfulGetCollection(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.StartGame)]
    private static void StartGame(Message m) {
        if (_currentState is MainMenu screen) {screen.TriggerStartGame(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.BeginStandby)]
    private static void BeginStandby(Message m) {
        if (_currentState is GameInstance screen) {screen.TriggerBeginStandby(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.BeginDiceSelect)]
    private static void BeginDiceSelect(Message m) {
        if (_currentState is GameInstance screen) {screen.TriggerBeginDiceSelect(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.BeginMain)]
    private static void BeginMain(Message m) {
        if (_currentState is GameInstance screen) {screen.TriggerBeginMain(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.BeginEnd)]
    private static void BeginEnd(Message m) {
        if (_currentState is GameInstance screen) {screen.TriggerBeginEnd(m);}
    }
    
    [MessageHandler((ushort)ServerToClientId.DiceRollResult)]
    private static void DiceRollResult(Message m) {
        if (_currentState is GameInstance screen) {screen.TriggerDiceRollResult(m);}
    }
}