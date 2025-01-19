-- Detail using VehicleSpecPatern as FK

DECLARE @year int = 2024;
DECLARE @MakeId int = 449;
DECLARE @ModelId int = 1703;


SELECT               
    VSX.Id AS VSpecSchemaPatternId, VSX.SchemaId, e.GroupName, e.Name, ([dbo].[fGetElementsValue](VSP.ElementId, VSP.AttributeId)) AS Value, *
FROM 
    dbo.VehicleSpecPattern AS VSP WITH (NOLOCK)     
    INNER JOIN dbo.VSpecSchemaPattern VSX ON VSX.Id = VSP.VSpecSchemaPatternId
    INNER JOIN dbo.VehicleSpecSchema VSS ON VSS.Id = VSX.SchemaId AND VSS.MakeId = @MakeId 
    INNER JOIN dbo.VehicleSpecSchema_Model VSM ON VSM.VehicleSpecSchemaId = VSS.Id AND VSM.ModelId = @ModelId
    INNER JOIN dbo.VehicleSpecSchema_Year VSY ON VSY.VehicleSpecSchemaId = VSS.Id AND VSY.[Year] = @year
    INNER JOIN Element E ON VSP.ElementId = E.Id
    ORDER BY E.GroupName, E.Name