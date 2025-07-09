local effect = {}

effect.name = "CaeruleaHelper/PureColor"
effect.canBackground = true
effect.canForeground = true -- um why are you considering this

effect.fieldInformation = {
    color = {
        fieldType = "color",
        allowEmpty = false
    }
}

effect.defaultData = {
    color = "ffffff",
}

return effect
