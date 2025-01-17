CREATE OR ALTER FUNCTION [dbo].[fGetVehSpecSchemaIdFromYMM]
(	
	@year int,
    @makeId int,
    @modelId int
)
RETURNS TABLE  
AS  
RETURN  
    SELECT VSS.Id FROM dbo.VehicleSpecSchema VSS
    INNER JOIN dbo.VehicleSpecSchema_Year VSY ON VSS.Id = VSY.VehicleSpecSchemaId AND VSY.[Year] = @year
    INNER JOIN dbo.VehicleSpecSchema_Model VSMD ON VSMD.VehicleSpecSchemaId = VSS.Id AND VSMD.ModelId = @modelId
    WHERE VSS.MakeId = @makeId;