namespace TennisKata
{
	public interface ITennisGame
	{
		void WonPoint(string playerName);
		string GetScore();
	}
}