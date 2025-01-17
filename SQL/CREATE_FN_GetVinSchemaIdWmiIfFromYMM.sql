CREATE OR ALTER FUNCTION [dbo].[fGetVinSchemaIdWmiIdFromYMM]
(	
	@year int,
    @makeId int,
    @modelId int
)
RETURNS TABLE  
AS  
RETURN  
    SELECT               
        UPPER(P.Keys) AS PatternKeys, 
        P.VinSchemaId, 
        WVS.WmiId    
    FROM 
        dbo.Pattern AS P WITH (NOLOCK) 
        INNER JOIN dbo.Element E WITH (NOLOCK) ON P.ElementId = E.Id
        INNER JOIN dbo.VinSchema VS WITH (NOLOCK) ON P.VinSchemaId = vs.Id
        INNER JOIN dbo.Wmi_VinSchema AS WVS WITH (NOLOCK) ON vs.Id = WVS.VinSchemaId 
            AND @year BETWEEN WVS.YearFrom 
            AND isnull(WVS.YearTo, 2999)
    WHERE   
        (P.ElementId = 28 AND P.AttributeId = @ModelId)        
    AND NOT E.Decode IS NULL;

