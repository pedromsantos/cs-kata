namespace RaidServiceKata;

public class RaidService
{
    public List<Raid> GetRaidsByGuildMember(GuildMember guildMember)
    {
        var loggedGuildMember = Session.GetLoggedGuildMember();

        if (loggedGuildMember == null) throw new NullReferenceException();
        
        if (guildMember.GetFriends().Any(member => member == loggedGuildMember))
        {
            return RaidDao.FindRaidsByGuildMember(guildMember);
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

public class Session
{
    public static GuildMember GetLoggedGuildMember()
    {
        throw new NotImplementedException();
    }
}

public static class RaidDao
{
    public static List<Raid> FindRaidsByGuildMember(GuildMember guildMember)
    {
        throw new NotImplementedException();
    }
}

public class Raid { }