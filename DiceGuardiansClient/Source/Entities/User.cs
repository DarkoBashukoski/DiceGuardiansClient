namespace DiceGuardiansClient.Source.Entities; 

public class User {
    private long _userId;
    private string _userName;
    private int _mmr;
    private int _gamesPlayed;
    private int _gamesWon;

    public User(string userName) {
        _userName = userName;
        _mmr = 0;
        _gamesPlayed = 0;
        _gamesWon = 0;
    }

    public User(long userId, string userName, int mmr, int gamesPlayed, int gamesWon) {
        _userId = userId;
        _userName = userName;
        _mmr = mmr;
        _gamesPlayed = gamesPlayed;
        _gamesWon = gamesWon;
    }

    public long GetUserId() {return _userId;}
    public string GetUserName() {return _userName;}
    public int GetMmr() {return _mmr;}
    public int GetGamesPlayed() {return _gamesPlayed;}
    public int GetGamesWon() {return _gamesWon;}
}