namespace DiceGuardiansClient.Source.Networking; 

public enum ServerToClientId : ushort {
    InvalidLoginUserDoesNotExist = 1,
    InvalidLoginWrongPassword = 2,
    SuccessfulLogin = 3,
    InvalidRegisterUsernameAlreadyTaken = 4,
    SuccessfulRegister = 5
}