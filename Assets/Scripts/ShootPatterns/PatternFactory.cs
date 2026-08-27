using UnityEngine;

namespace ShootPatterns
{
    public static class PatternFactory
    {
        // Example method to create a pattern based on ShootPatternInfo
        public static Pattern CreatePattern(ShootPatternInfo patternInfo)
        {
            switch (patternInfo.PatternType)
            {
                case ShootPatternType.Radial:
                    return new RadialPattern((RadialPatternConfig)patternInfo.Config);
                case ShootPatternType.Spiral:
                    return new SpiralPattern((SpiralPatternConfig)patternInfo.Config);
                default:
                    Debug.LogError("Unknown pattern type: " + patternInfo.PatternType);
                    return null;
            }
        }
    }

    [System.Serializable]
    public class ShootPatternInfo
    {
        public ShootPatternType PatternType;
        public ShootPatternConfig Config;
    }
}
