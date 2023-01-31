namespace DiceGuardiansClient.Source.Networking; 

public enum ServerToClientId : ushort {
    InvalidLoginUserDoesNotExist = 1,
    InvalidLoginWrongPassword = 2,
    SuccessfulLogin = 3,
    InvalidRegisterUsernameAlreadyTaken = 4,
    SuccessfulRegister = 5,
    SuccessfulGetCollectionResponse = 6,
    StartGame = 7,
    BeginStandby = 101,
    BeginDiceSelect = 102,
    BeginMain = 103,
    BeginEnd = 104,
    DiceRollResult = 201,
    PlaceTile = 202,
    MoveMinion = 203
}