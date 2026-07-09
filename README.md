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
Options:
 - Red: Controls whether this booster is a red booster
 - Global: Controls whether this booster is in global mode
 - Initial Value: Controls whether this booster is a reverse booster at the beginning
 - Toggle Reverse: Controls whether this booster toggles reversal
 - Toggle Type: Controls whether this booster toggles type (red or green)

The texture of the arrows indicators are from [GlassHelper](https://github.com/raine-storm/GlassHelper).

Quarter Rotate Booster: Rotates player any degrees you want.

Custom Star Jump Block: Star Jump Block, but you can customize it. Two options control
whether there are edges or railing. Additionally, you can specify the textures of the
edges and railing. A default edge texture `objects/caerulea/starjumpblock/` is provided
as an almost blank texture.

### Controllers

Invisible Spinner Controller: Place this so all your (vanilla) spinners in this room 
become invisible. This will (unexpectedly) affect spinners in the following room when
they're being first loaded, so be careful. Note: I recommend using Set Spinner Invisible
Trigger instead.

Jelly Freezing Fix Controller: This fixes the vanilla bug for jellyfish (aka glider). In
vanilla when you do holdable freezing to jellyfish and die (not retry), the game would encounter
a critical error. If you place this controller, the bug is fixed.

### Triggers

Always Fail Super/Hyper Trigger: This trigger lets player perform failed supers/hypers
instead of normal ones. An additional option controls whether a "reverse failed super/hyper"
(which is not a vanilla tech) can be performed.

Always Fail Wallbounce Trigger: This trigger lets player always perform wall kicks or climb
jumps instead of wallbounces. Note that this does not change the original 4px leniency for
wallbounces, so to avoid wall kicking / climb jumping 4px from the wall, you need to use
Extended Variants to change Wall Bounce Distance.

Downdash Wallbounce Trigger: This trigger allows player to perform wallbounces by down
dashing.

Set Spinner Invisible Trigger: This trigger has similar effects with Invisible Spinner
Controller. Note: This does not work when the level is being first loaded. Use the controller
for this case.

Spike Correction Leniency Trigger: This trigger adds leniency to corner correction when
the wall has spikes on it. More specifically, if the player is dashing exactly towards
spikes, there will be a single invincibility frame, making corner corrections stable.
This is useful for wall bounces, air supers, etc. and is especially helpful for downdash
corner corrections on top of spikes.

Bronze Collect Trigger: This trigger lets player collect all their bronze berries.

Disable Super/Hyper Trigger: This trigger disables supers/hypers.

No Dash Speed Reset Trigger: This trigger makes player preserve speed after horizontal
and up-diagnal dashes.

Backdrop Blur Effect Trigger: This trigger adds (or removes, of course) blur effects to
specific stylegrounds. To use this trigger, fill in some tag, than add `caeruleaBlurEffect`
and the tag you wrote to the styleground (in case you don't know, use comma to seperate).

### Backdrops

Custom Ascending Stars: Provides random moving stars, like the ones in Final Goodbye
(Sunrise Running, specifically).

Pure Color: One color.

### Special Flags

caeruleaJumpSwitchFlag: This flag toggles whenever player jumps. This includes
normal jumping, super/hyper, wall kick, wall bounce, climb jump and hiccups.

caeruleaHelperToggleReverseBoosterFlag: This flag controls global mode Toggle Reverse
Boosters.
