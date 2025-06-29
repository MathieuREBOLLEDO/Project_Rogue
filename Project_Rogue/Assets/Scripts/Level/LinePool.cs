using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LinePool", menuName = "BrickLevel/Line Pool", order = 1)]
public class LinePool : ScriptableObject
{
    public List<WeightedLine> lines = new List<WeightedLine>();
}
