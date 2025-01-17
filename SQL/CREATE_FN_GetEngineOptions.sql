CREATE OR ALTER FUNCTION [dbo].[fGetEngineOptions]
(	
	@VinSchemaId int
)
RETURNS TABLE  
AS  
RETURN  

WITH T AS (
SELECT P.Keys, P.ElementId,
    ( SELECT
        CASE
            WHEN P.ElementId = 24 THEN (SELECT TOP 1 Name FROM dbo.FuelType WHERE Id = P.AttributeId)
            WHEN P.ElementId = 62 THEN (SELECT TOP 1 Name FROM dbo.ValvetrainDesign WHERE Id = P.AttributeId)
            WHEN P.ElementId = 64 THEN (SELECT TOP 1 Name FROM dbo.EngineConfiguration WHERE Id = P.AttributeId)
            WHEN P.ElementId = 135 THEN (SELECT TOP 1 Name FROM dbo.Turbo WHERE Id = P.AttributeId)
            ELSE P.AttributeId
        END
    ) AS Val
FROM dbo.Pattern P  
WHERE VinSchemaId = @VinSchemaId 
    AND P.ElementId IN (13, 64, 18, 9, 24, 129, 135, 62)
)
SELECT DISTINCT(T.Keys), 
EngModel.Val AS EngineModel,
FuelType.Val AS FuelType,
Displacement.Val AS DisplacementL,
EngCfg.Val AS EngineConfiguration,
NoCyls.Val AS NoCylinders,
VlvTrnDesign.Val AS ValveTrainDesign,
Turbo.Val AS Turbo,
OtherEngInfo.Val AS OtherEngineInfo
FROM T
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 18
) EngModel
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 13
) Displacement
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 64
) EngCfg
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 62
) VlvTrnDesign
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 9
) NoCyls
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 24
) FuelType
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 129
) OtherEngInfo
OUTER APPLY (
    SELECT TOP 1 Val
    FROM T TI 
    WHERE T.Keys = TI.Keys 
     AND TI.ElementId  = 135
) Turbo


GO




