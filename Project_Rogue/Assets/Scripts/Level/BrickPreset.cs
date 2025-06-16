using System;
using System.Collections.Generic;

[Serializable]
public class BrickPreset
{
    public int rows;
    public int columns;
    public List<string> grid = new List<string>(); // chaque string est une ligne
}
