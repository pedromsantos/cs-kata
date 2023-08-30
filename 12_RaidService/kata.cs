namespace RaidServiceKata;

public class RaidService
{
    public List<Raid> GetRaidsByGuildMember(GuildMember guildMember)
    {
        List<Raid> raidList = new List<Raid>();
        GuildMember loggedGuildMember = GuildMemberSession.GetLoggedGuildMember();
        bool isFriend = false;

        if (loggedGuildMember != null)
        {
            foreach (GuildMember fellowMember in guildMember.GetFriends())
            {
                if (fellowMember == loggedGuildMember)
                {
                    isFriend = true;
                    break;
                }
            }

            if (isFriend)
            {
                raidList = RaidDAO.FindRaidsByGuildMember(guildMember);
            }

            return raidList;
        }
        else
        {
            throw new GuildMemberNotLoggedInException();
        }
    }
}

public class GuildMember
{
    private readonly List<Raid> _raids = new List<Raid>();
    private readonly List<GuildMember> _friends = new List<GuildMember>();

    public List<GuildMember> GetFriends()
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

    public List<Raid> GetRaids()
    {
        return _raids;
    }
}

public class GuildMemberSession
{
    public static GuildMember GetLoggedGuildMember()
    {
        throw new CollaboratorCallException();
    }
}

public class RaidDAO
{
    public static List<Raid> FindRaidsByGuildMember(GuildMember guildMember)
    {
        throw new CollaboratorCallException();
    }
}

public class Raid { }

public class GuildMemberNotLoggedInException : Exception { }

public class CollaboratorCallException : Exception { }