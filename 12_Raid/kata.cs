namespace RaidKata;

public class RaidService
{
    public List<Raid> GetRaidsByGuildMember(GuildMember other)
    {
        var player = GuildDao.FindActivePlayer();

        if (player == null) throw new NullReferenceException();

        if (other.GetFriends().Any(member => member == player))
        {
            return RaidDao.FindRaidsBy(other);
        }

        return new List<Raid>();
    }
}

public class GuildMember
{
    private readonly List<Raid> _raids = new();
    private readonly List<GuildMember> _friends = new();

    public IEnumerable<GuildMember> GetFriends()
    {
        return _friends;
    }

    public void AddFriend(GuildMember member)
    {
        _friends.Add(member);
    }

    public void AddRaid(Raid raid)
    {
        _raids.Add(raid);
    }

    public IEnumerable<Raid> GetRaids()
    {
        return _raids;
    }
}

public class GuildDao
{
    public static GuildMember FindActivePlayer()
    {
        throw new NotImplementedException();
    }
}

public static class RaidDao
{
    public static List<Raid> FindRaidsBy(GuildMember guildMember)
    {
        throw new NotImplementedException();
    }
}

public class Raid { }