local quarterRotateBooster = {}

quarterRotateBooster.name = "CaeruleaHelper/QuarterRotateBooster"
quarterRotateBooster.depth = -100
quarterRotateBooster.placements = {
    {
        name = "quarter_rotate_booster",
        data = {
            red = false,
            degrees = 90,
            twistSprite = "objects/caerulea/revbooster/twist/twist"
        }
    }
}

function quarterRotateBooster.texture(room, entity)
    local red = entity.red

    if red then
        return "objects/booster/boosterRed00"

    else
        return "objects/booster/booster00"
    end
end

return quarterRotateBooster
