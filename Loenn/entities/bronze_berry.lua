local bronzeBerry = {}

bronzeBerry.name = "CaeruleaHelper/BronzeBerry"
bronzeBerry.depth = -100
bronzeBerry.placements = {
    {
        name = "bronze_berry",
        data = {
            bronze = "ffa332",
            altbronze = "de6a1d",
            ghost = "ffa332",
            altghost = "de6a1d",
            sprite = "Caerulea_BronzeBerry",
            ghostsprite = "Caerulea_BronzeBerryGhost"
        }
    }
}
bronzeBerry.fieldInformation = {
    bronze = {
        fieldType = "color",
        allowEmpty = false
    },
    altbronze = {
        fieldType = "color",
        allowEmpty = false
    },
    ghost = {
        fieldType = "color",
        allowEmpty = false
    },
    altghost = {
        fieldType = "color",
        allowEmpty = false
    }
}

bronzeBerry.texture = "collectables/caerulea/bronze/idle00"

return bronzeBerry
