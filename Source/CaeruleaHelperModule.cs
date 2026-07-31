using System;
using Celeste.Mod.CaeruleaHelper.Effects;
using Celeste.Mod.CaeruleaHelper.Entities;
using Celeste.Mod.CaeruleaHelper.Hooks;

namespace Celeste.Mod.CaeruleaHelper;

public class CaeruleaHelperModule : EverestModule {
    public static CaeruleaHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(CaeruleaHelperModuleSettings);
    public static CaeruleaHelperModuleSettings Settings => (CaeruleaHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(CaeruleaHelperModuleSession);
    public static CaeruleaHelperModuleSession Session => (CaeruleaHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(CaeruleaHelperModuleSaveData);
    public static CaeruleaHelperModuleSaveData SaveData => (CaeruleaHelperModuleSaveData) Instance._SaveData;

    public CaeruleaHelperModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(CaeruleaHelperModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(CaeruleaHelperModule), LogLevel.Info);
#endif
    }

    public override void Load()
    {
        // TODO: apply any hooks that should always be active
        ActorHook.Load();
        BackdropLoader.Load();
        BackdropRenderHook.Load();
        BerryHook.Load();
        CustomStarJumpBlock.Load();
        DashCorrectionProtection.Load();
        DashSpeedHook.Load();
        InvisibleSpinnerController.Load();
        JumpSwitchFlag.Load();
        NorthernLightsHook.Load();
        QuarterRotateBooster.Load();
        SuperJumpHook.Load();
        ToggleReverseBooster.Load();
    }

    public override void Unload()
    {
        // TODO: unapply any hooks applied in Load()
        ActorHook.Unload();
        BackdropLoader.Unload();
        BackdropRenderHook.Unload();
        BerryHook.Unload();
        CustomStarJumpBlock.Unload();
        DashCorrectionProtection.Unload();
        DashSpeedHook.Unload();
        InvisibleSpinnerController.Unload();
        JumpSwitchFlag.Unload();
        NorthernLightsHook.Unload();
        QuarterRotateBooster.Unload();
        SuperJumpHook.Unload();
        ToggleReverseBooster.Unload();
    }
}