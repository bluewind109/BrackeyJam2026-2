using UnityEngine;

namespace ShootPatterns
{
    public static class PatternFactory
    {
        public static Pattern CreatePattern(ShootPatternInfo patternInfo)
        {
            switch (patternInfo.PatternType)
            {
                case ShootPatternType.Radial:
                    return new RadialPattern((RadialPatternConfig)patternInfo.Config);
                case ShootPatternType.Spiral:
                    return new SpiralPattern((SpiralPatternConfig)patternInfo.Config);
                case ShootPatternType.Sideway:
                    return new SidewayPattern((SidewayPatternConfig)patternInfo.Config);
                case ShootPatternType.Fan:
                    return new FanPattern((FanPatternConfig)patternInfo.Config);
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
