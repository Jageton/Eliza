using System;
using System.ComponentModel;

namespace OGESolver
{
    [Serializable]
    public enum TimeUnit: int
    {
        [Description("сек.")]
        Second = 1,
        [Description("мин.")]
        Minute = 60,
        [Description("ч.")]
        Hour = 3600
    }
}
