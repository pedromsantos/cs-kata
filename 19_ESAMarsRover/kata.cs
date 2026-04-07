using Xunit;

namespace ESAMarsRoverKata;

public class MarsRoverShould 
{
    [Fact]
    public void Test1()
    {
    }
}

public interface IRover {
    void Execute();
}

public interface IRadio {
    void Send(string message);
    string Receive();
}
