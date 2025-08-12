using System;
using Celeste.Mod.CaeruleaHelper.Effects;
using Celeste.Mod.CaeruleaHelper.Entities;
using Celeste.Mod.CaeruleaHelper.Triggers;
using Monocle;

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
        SuperJumpHook.Load();
        DashSpeedHook.Load();
        BerryHook.Load();
        BackdropLoader.Load();
        JumpSwitchFlag.Load();
    }

    public override void Unload()
    {
        // TODO: unapply any hooks applied in Load()
        SuperJumpHook.Unload();
        DashSpeedHook.Unload();
        BerryHook.Unload();
        BackdropLoader.Unload();
        JumpSwitchFlag.Unload();
    }
}