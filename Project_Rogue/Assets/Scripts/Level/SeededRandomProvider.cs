public class SeededRandomProvider : IRandomProvider
{
    private System.Random rng;

    public SeededRandomProvider(int seed)
    {
        rng = new System.Random(seed);
    }

    public int Next(int min, int max)
    {
        return rng.Next(min, max);
    }
}
