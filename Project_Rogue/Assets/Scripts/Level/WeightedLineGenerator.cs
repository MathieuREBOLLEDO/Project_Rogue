using System.Collections.Generic;

public class WeightedLineGenerator : ILineGenerator
{
    private readonly List<WeightedLine> lines;
    private readonly IRandomProvider random;

    public WeightedLineGenerator(List<WeightedLine> lines, IRandomProvider random)
    {
        this.lines = lines;
        this.random = random;
    }

    public string GenerateLine()
    {
        int totalWeight = 0;
        foreach (var line in lines)
            totalWeight += line.weight;

        int rnd = random.Next(0, totalWeight);
        int sum = 0;
        foreach (var line in lines)
        {
            sum += line.weight;
            if (rnd < sum)
                return line.line;
        }

        return new string('1', lines[0].line.Length);
    }
}