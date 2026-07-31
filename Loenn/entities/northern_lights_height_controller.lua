local northernLightsHeightController = {}

northernLightsHeightController.name = "CaeruleaHelper/NorthernLightsHeightController"
northernLightsHeightController.fieldInformation = {
    minHeight = {
        fieldType = "integer",
    },
    maxHeight = {
        fieldType = "integer",
    }
}
northernLightsHeightController.placements = {
    {
        name = "northern_lights_height_controller",
        data = {
            minHeight = 40,
            maxHeight = 90
        }
    }
}
northernLightsHeightController.texture = "@Internal@/northern_lights"

return northernLightsHeightController
