local trigger = {}

trigger.name = "CaeruleaHelper/DowndashWallbounceTrigger"
trigger.placements = {
    name = "trigger",
    data = {
        enable = true,
        revertOnLeave = true,
        horizontalSpeedMultiplier = 1,
        verticalSpeedMultiplier = 1,
        jumpTimerMultiplier = 1
    }
}

return trigger