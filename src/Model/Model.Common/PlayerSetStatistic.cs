namespace App.Models;

public class PlayerSetStatistic
{
    public int OneEighties { get; set; }
    public int Ninedarters { get; set; }

    public int LegsPlayed { get; set; }
    public int LegsWon { get; set; }
    public int LegsLost => LegsPlayed - LegsWon;
}