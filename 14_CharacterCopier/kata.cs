using Xunit;

namespace CharacterCopierKata;

public class CharacterCopierShould 
{
    [Fact]
    public void CopyCharacterFromSourceToDestination()
    {
        Assert.True(true);
    }
}

public class CharacterCopier
{
    public void Copy() {}
}

public interface ISource
{
    char GetChar();
}

public interface IDestination
{
    void GetChar(char character);
}