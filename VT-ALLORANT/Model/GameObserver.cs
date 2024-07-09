using VT_ALLORANT.Model;
public class GameObserver
{
    public int GameId { get; set; }
    public Game? Game { get; set; }
    public int ObserverId { get; set; }
    public Player? Observer { get; set; }
}