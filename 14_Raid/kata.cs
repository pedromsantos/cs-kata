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

        return [];
    }
}

public class GuildMember
{
    private readonly List<Raid> raids = [];
    private readonly List<GuildMember> friends = new();

    public IEnumerable<GuildMember> GetFriends()
    {
        return friends;
    }

    public void AddFriend(GuildMember member)
    {
        friends.Add(member);
    }

    public void AddRaid(Raid raid)
    {
        raids.Add(raid);
    }

    public IEnumerable<Raid> GetRaids()
    {
        return raids;
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