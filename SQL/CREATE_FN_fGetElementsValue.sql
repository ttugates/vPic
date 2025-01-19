SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER FUNCTION [dbo].[fGetElementsValue]
(	
	@elementId int,
    @attributeId varchar(500)
)
RETURNS VARCHAR(500)
AS
BEGIN
    RETURN
        CASE 
            WHEN @elementId = 2 THEN (SELECT TOP 1 Name FROM BatteryType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 3 THEN (SELECT TOP 1 Name FROM BedType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 4 THEN (SELECT TOP 1 Name FROM BodyCab WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 5 THEN (SELECT TOP 1 Name FROM BodyStyle WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 8 THEN (SELECT TOP 1 Name FROM Country WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 10 THEN (SELECT TOP 1 Name FROM DestinationMarket WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 15 THEN (SELECT TOP 1 Name FROM DriveType WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 16 THEN (SELECT TOP 1 Name FROM DriverAssist WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 23 THEN (SELECT TOP 1 Name FROM EntertainmentSystem WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 24 THEN (SELECT TOP 1 Name FROM FuelType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 25 THEN (SELECT TOP 1 Name FROM GrossVehicleWeightRating WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 26 THEN (SELECT TOP 1 Name FROM Make WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 27 THEN (SELECT TOP 1 Name FROM Manufacturer WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 28 THEN (SELECT TOP 1 Name FROM Model WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 36 THEN (SELECT TOP 1 Name FROM Steering WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 37 THEN (SELECT TOP 1 Name FROM Transmission WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 39 THEN (SELECT TOP 1 Name FROM VehicleType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 42 THEN (SELECT TOP 1 Name FROM BrakeSystem WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 45 THEN (SELECT TOP 1 Name FROM NCICCode WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 55 THEN (SELECT TOP 1 Name FROM AirBagLocations WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 56 THEN (SELECT TOP 1 Name FROM AirBagLocations WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 60 THEN (SELECT TOP 1 Name FROM WheelBaseType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 62 THEN (SELECT TOP 1 Name FROM ValvetrainDesign WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 64 THEN (SELECT TOP 1 Name FROM EngineConfiguration WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 65 THEN (SELECT TOP 1 Name FROM AirBagLocFront WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 66 THEN (SELECT TOP 1 Name FROM FuelType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 67 THEN (SELECT TOP 1 Name FROM FuelDeliveryType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 69 THEN (SELECT TOP 1 Name FROM AirBagLocKnee WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 72 THEN (SELECT TOP 1 Name FROM EVDriveUnit WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 75 THEN (SELECT TOP 1 Name FROM Country WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 78 THEN (SELECT TOP 1 Name FROM Pretensioner WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 79 THEN (SELECT TOP 1 Name FROM SeatBeltsAll WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 81 THEN (SELECT TOP 1 Name FROM AdaptiveCruiseControl WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 82 THEN (SELECT TOP 1 Name FROM AdaptiveHeadlights WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 86 THEN (SELECT TOP 1 Name FROM ABS WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 87 THEN (SELECT TOP 1 Name FROM AutoBrake WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 88 THEN (SELECT TOP 1 Name FROM BlindSpotMonitoring WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 89 THEN (SELECT TOP 1 Name FROM CAFEBodyType WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 90 THEN (SELECT TOP 1 Name FROM CAFEMake WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 91 THEN (SELECT TOP 1 Name FROM CAFEModel WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 96 THEN (SELECT TOP 1 Name FROM vNCSABodyType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 97 THEN (SELECT TOP 1 Name FROM vNCSAMake WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 98 THEN (SELECT TOP 1 Name FROM vNCSAModel WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 99 THEN (SELECT TOP 1 Name FROM ECS WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 100 THEN (SELECT TOP 1 Name FROM TractionControl WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 101 THEN (SELECT TOP 1 Name FROM ForwardCollisionWarning WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 102 THEN (SELECT TOP 1 Name FROM LaneDepartureWarning WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 103 THEN (SELECT TOP 1 Name FROM LaneKeepSystem WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 104 THEN (SELECT TOP 1 Name FROM RearVisibilityCamera WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 105 THEN (SELECT TOP 1 Name FROM ParkAssist WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 107 THEN (SELECT TOP 1 Name FROM AirBagLocations WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 116 THEN (SELECT TOP 1 Name FROM TrailerType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 117 THEN (SELECT TOP 1 Name FROM TrailerBodyType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 122 THEN (SELECT TOP 1 Name FROM CoolingType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 126 THEN (SELECT TOP 1 Name FROM ElectrificationLevel WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 127 THEN (SELECT TOP 1 Name FROM ChargerLevel WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 135 THEN (SELECT TOP 1 Name FROM Turbo WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 140 THEN (SELECT TOP 1 Name FROM EquipmentType WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 141 THEN (SELECT TOP 1 Name FROM ManufacturerType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 143 THEN (SELECT TOP 1 Name FROM ErrorCode WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 145 THEN (SELECT TOP 1 Name FROM AxleConfiguration WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 148 THEN (SELECT TOP 1 Name FROM BusFloorConfigType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 149 THEN (SELECT TOP 1 Name FROM BusType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 151 THEN (SELECT TOP 1 Name FROM CustomMotorcycleType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 152 THEN (SELECT TOP 1 Name FROM MotorcycleSuspensionType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 153 THEN (SELECT TOP 1 Name FROM MotorcycleChassisType WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 168 THEN (SELECT TOP 1 Name FROM TPMS WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 170 THEN (SELECT TOP 1 Name FROM DynamicBrakeSupport WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 171 THEN (SELECT TOP 1 Name FROM PedestrianAutomaticEmergencyBraking WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 172 THEN (SELECT TOP 1 Name FROM AutoReverseSystem WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 173 THEN (SELECT TOP 1 Name FROM AutomaticPedestrainAlertingSound WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 174 THEN (SELECT TOP 1 Name FROM CAN_AACN WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 175 THEN (SELECT TOP 1 Name FROM EDR WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 176 THEN (SELECT TOP 1 Name FROM KeylessIgnition WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 177 THEN (SELECT TOP 1 Name FROM DaytimeRunningLight WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 178 THEN (SELECT TOP 1 Name FROM LowerBeamHeadlampLightSource WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 179 THEN (SELECT TOP 1 Name FROM SemiautomaticHeadlampBeamSwitching WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 180 THEN (SELECT TOP 1 Name FROM AdaptiveDrivingBeam WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 183 THEN (SELECT TOP 1 Name FROM RearCrossTrafficAlert WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 184 THEN (SELECT TOP 1 Name FROM GrossVehicleWeightRating WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 185 THEN (SELECT TOP 1 Name FROM GrossVehicleWeightRating WHERE Id = TRY_CAST(@attributeId AS INT))
            -- WHEN @elementId = 187 THEN (SELECT TOP 1 Name FROM NCSAMappingException WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 190 THEN (SELECT TOP 1 Name FROM GrossVehicleWeightRating WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 192 THEN (SELECT TOP 1 Name FROM RearAutomaticEmergencyBraking WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 193 THEN (SELECT TOP 1 Name FROM BlindSpotIntervention WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 194 THEN (SELECT TOP 1 Name FROM LaneCenteringAssistance WHERE Id = TRY_CAST(@attributeId AS INT))
            WHEN @elementId = 195 THEN (SELECT TOP 1 Name FROM NonLandUse WHERE Id = TRY_CAST(@attributeId AS INT))
            ELSE @attributeId
        END 
END
GO
