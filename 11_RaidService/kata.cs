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
    private List<Raid> raids = new List<Raid>();
    private List<GuildMember> friends = new List<GuildMember>();

    public List<GuildMember> GetFriends()
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

    public List<Raid> GetRaids()
    {
        return raids;
    }
}

public class GuildMemberSession
{
    public static GuildMember GetLoggedGuildMember()
    {
        throw new CollaboratorCallException(
            "GuildMemberSession.GetLoggedGuildMember() should not be called in a unit test"
        );
    }
}

public class RaidDAO
{
    public static List<Raid> FindRaidsByGuildMember(GuildMember guildMember)
    {
        throw new CollaboratorCallException("RaidDAO should not be invoked in a unit test.");
    }
}

public class Raid { }

public class GuildMemberNotLoggedInException : Exception { }

public class CollaboratorCallException : Exception { }