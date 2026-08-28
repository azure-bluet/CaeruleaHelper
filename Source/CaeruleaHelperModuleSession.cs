using System.Collections.Generic;

namespace Celeste.Mod.CaeruleaHelper;

public class CaeruleaHelperModuleSession : EverestModuleSession
{
    public bool DisableSuper { get; set; } = false;
    public bool AlwaysFailSuper { get; set; } = false;
    public bool AlwaysFailWallbounce { get; set; } = false;
    public bool DowndashWallbounce { get; set; } = false;
    public float DowndashWallbounceHorizontalSpeedMultiplier { get; set; } = 1f;
    public float DowndashWallbounceVerticalSpeedMultiplier { get; set; } = 1f;
    public float DowndashWallbounceJumpTimerMultiplier { get; set; } = 1f;
    public bool AllowReverseFailSuper { get; set; } = true;
    public bool NoDashSpeedReset { get; set; } = false;
    public bool SetSpinnerInvisible { get; set; } = false;
    public bool SpikeCorrectionLeniency { get; set; } = false;
    public Dictionary<string, float> BlurEffectValues { get; set; } = [];
}