# CaeruleaHelper
This is a helper containing random stuff.
Created using a template, so it might contain useless code.

## Content

Caerulea Helper contains various features, including backdrops, entities,
triggers and special flags.

### Entities
Bronze Berry: Acts like a golden/silver berry, except you can use different
sprites on different bronze berries in one map.

Static Space Block: Ignore this one. This is a floaty space block without
natural wobbling.

Toggle Reverse Booster: Acts like reverse boosters in Glass Helper, except it
toggles whenever being used. There are two modes, global mode and non-global mode.
Global mode makes all the boosters toggle globally when used. Non-global mode makes
the booster only toggles itself.
The texture of the arrows indicators are from [GlassHelper](https://github.com/raine-storm/GlassHelper).

### Triggers

Always Fail Super/Hyper Trigger: This trigger lets player perform failed supers/hypers
instead of normal ones. An additional option controls whether a "reverse failed super/hyper"
(which is not a vanilla tech) can be performed.

Bronze Collect Trigger: This trigger lets player collect all their bronze berries.

Disable Super/Hyper Trigger: This trigger disables supers/hypers.

No Dash Speed Reset Trigger: This trigger makes player preserve speed after horizontal
and up-diagnal dashes.

### Backdrops

Custom Ascending Stars: Provides random moving stars, like the ones in Final Goodbye
(Sunrise Running, specifically).

Pure Color: One color.

### Special Flags

caeruleaJumpSwitchFlag: This flag toggles whenever player jumps. This includes
normal jumping, super/hyper, wall kick, wall bounce, climb jump and hiccups.

caeruleaHelperToggleReverseBoosterFlag: This flag controls global mode Toggle Reverse
Boosters.
