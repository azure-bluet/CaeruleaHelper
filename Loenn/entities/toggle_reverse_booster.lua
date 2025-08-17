local toggleReverseBooster = {}

toggleReverseBooster.name = "CaeruleaHelper/ToggleReverseBooster"
toggleReverseBooster.depth = -100
toggleReverseBooster.placements = {
    {
        name = "toggle_reverse_booster",
        data = {
            red = false,
            global = false,
            initial = false,
            twistSprite = "objects/caerulea/revbooster/twist/twist"
        }
    }
}

function toggleReverseBooster.texture(room, entity)
    local red = entity.red

    if red then
        return "objects/booster/boosterRed00"

    else
        return "objects/booster/booster00"
    end
end

return toggleReverseBooster
