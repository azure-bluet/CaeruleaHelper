namespace Celeste.Mod.CaeruleaHelper;

public class CaeruleaHelperModuleSession : EverestModuleSession
{
    public bool DisableSuper { get; set; } = false;
    public bool AlwaysFailSuper { get; set; } = false;
    public bool AllowReverseFailSuper { get; set; } = true;
    public bool NoDashSpeedReset { get; set; } = false;
}