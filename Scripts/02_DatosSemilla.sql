/* =====================================================================
   OCN_WebApp_DB · Datos semilla (Actividad 8 - RegistrosOCN)
   Mínimo pedido: 3 zonas, 3 problemáticas y 10 registros.

   - Datos ANÓNIMOS y categorizados: no hay nombres de personas,
     teléfonos ni direcciones exactas (regla ética del OCN).
   - Se puede ejecutar varias veces: primero borra lo que haya y
     vuelve a insertar todo.
   - Requisito: haber aplicado la migración (dotnet ef database update).
   ===================================================================== */

USE OCN_WebApp_DB;
GO

SET NOCOUNT ON;
BEGIN TRANSACTION;

-- 1) Limpiar. Primero el "hijo" (RegistrosOCN), luego los "papás",
--    porque la FK no deja borrar una zona que todavía tiene registros.
DELETE FROM RegistrosOCN;
DELETE FROM Zonas;
DELETE FROM Problematicas;

-- Reiniciar los contadores (IDENTITY) para que los Id vuelvan a empezar en 1.
-- Solo se hace si la tabla ya tuvo filas alguna vez (last_value no es NULL);
-- en una tabla recién creada no hace falta y daría Id = 0.
IF (SELECT last_value FROM sys.identity_columns WHERE object_id = OBJECT_ID('RegistrosOCN')) IS NOT NULL
    DBCC CHECKIDENT ('RegistrosOCN', RESEED, 0) WITH NO_INFOMSGS;
IF (SELECT last_value FROM sys.identity_columns WHERE object_id = OBJECT_ID('Zonas')) IS NOT NULL
    DBCC CHECKIDENT ('Zonas', RESEED, 0) WITH NO_INFOMSGS;
IF (SELECT last_value FROM sys.identity_columns WHERE object_id = OBJECT_ID('Problematicas')) IS NOT NULL
    DBCC CHECKIDENT ('Problematicas', RESEED, 0) WITH NO_INFOMSGS;

-- 2) Catálogo de zonas (los "papás"). El Id lo pone SQL Server solo (IDENTITY).
INSERT INTO Zonas (NombreZona) VALUES
    (N'Sector Centro'),
    (N'Sector Norte'),
    (N'Sector Sur');

-- 3) Catálogo de problemáticas.
INSERT INTO Problematicas (NombreProblematica) VALUES
    (N'Deserción escolar'),
    (N'Consumo de alcohol en jóvenes'),
    (N'Falta de espacios recreativos');

-- 4) Registros (los "hijos"). En vez de escribir el número de la zona a mano,
--    se busca por su nombre: así no importa qué Id le tocó a cada una.
--    Intensidad y Frecuencia siempre entre 1 y 5.
INSERT INTO RegistrosOCN (ZonaId, ProblematicaId, Intensidad, Frecuencia)
SELECT z.ZonaId, p.ProblematicaId, d.Intensidad, d.Frecuencia
FROM (VALUES
    (N'Sector Centro', N'Deserción escolar',             4, 3),
    (N'Sector Centro', N'Consumo de alcohol en jóvenes', 3, 4),
    (N'Sector Centro', N'Falta de espacios recreativos', 2, 2),
    (N'Sector Norte',  N'Deserción escolar',             5, 4),
    (N'Sector Norte',  N'Consumo de alcohol en jóvenes', 4, 5),
    (N'Sector Norte',  N'Falta de espacios recreativos', 3, 3),
    (N'Sector Sur',    N'Deserción escolar',             2, 1),
    (N'Sector Sur',    N'Consumo de alcohol en jóvenes', 5, 5),
    (N'Sector Sur',    N'Falta de espacios recreativos', 4, 4),
    (N'Sector Norte',  N'Deserción escolar',             1, 2)
) AS d (NombreZona, NombreProblematica, Intensidad, Frecuencia)
JOIN Zonas         AS z ON z.NombreZona         = d.NombreZona
JOIN Problematicas AS p ON p.NombreProblematica = d.NombreProblematica;

COMMIT TRANSACTION;
GO

-- 5) Comprobación: cuántos hay de cada uno (esperado: 3, 3 y 10).
SELECT 'Zonas' AS Tabla, COUNT(*) AS Total FROM Zonas
UNION ALL SELECT 'Problematicas', COUNT(*) FROM Problematicas
UNION ALL SELECT 'RegistrosOCN',  COUNT(*) FROM RegistrosOCN;

-- 6) Los registros con el NOMBRE de su zona y problemática (usando las FK).
SELECT r.RegistroOCNId, z.NombreZona, p.NombreProblematica, r.Intensidad, r.Frecuencia
FROM RegistrosOCN AS r
JOIN Zonas         AS z ON z.ZonaId         = r.ZonaId
JOIN Problematicas AS p ON p.ProblematicaId = r.ProblematicaId
ORDER BY r.RegistroOCNId;
