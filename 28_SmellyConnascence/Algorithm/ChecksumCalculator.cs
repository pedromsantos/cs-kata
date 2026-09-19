namespace SmellyConnascenceKata.Algorithm;

// Connascence of Algorithm: the same checksum computation (sum of char
// codes mod 10) is duplicated in both methods instead of extracted once
// -- if the algorithm ever changes, both call sites must be updated in
// lockstep or they silently disagree.
public class ChecksumCalculator
{
    public string AddChecksum(string inputData)
    {
        var sum = 0;
        foreach (var ch in inputData)
        {
            sum += ch;
        }

        var checksum = sum % 10;
        return inputData + checksum;
    }

    public bool Check(string inputDataWithChecksum)
    {
        var inputData = inputDataWithChecksum[..^1];
        var expected = int.Parse(inputDataWithChecksum[^1..]);
        var sum = 0;
        foreach (var ch in inputData)
        {
            sum += ch;
        }

        return sum % 10 == expected;
    }
}
