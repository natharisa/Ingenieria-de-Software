USE [TecniSalud];
GO

MERGE dbo.Etiqueta AS target
USING (VALUES
    ('AUDIT_TITLE', 'Titulo de bitacora'),
    ('AUDIT_DESCRIPTION', 'Descripcion de bitacora'),
    ('AUDIT_EMPTY', 'Mensaje sin eventos'),
    ('AUDIT_COUNT', 'Cantidad de eventos registrados'),
    ('BTN_REFRESH', 'Boton actualizar'),
    ('GRID_ID', 'Columna id'),
    ('GRID_DATE', 'Columna fecha'),
    ('GRID_USER_ID', 'Columna id usuario'),
    ('GRID_USER', 'Columna usuario'),
    ('GRID_MODULE', 'Columna modulo'),
    ('GRID_ACTION', 'Columna accion'),
    ('GRID_LEVEL', 'Columna nivel'),
    ('GRID_DESCRIPTION', 'Columna descripcion'),
    ('GRID_DEVICE', 'Columna equipo')
) AS source (clave, descripcion)
    ON target.clave = source.clave
WHEN NOT MATCHED THEN
    INSERT (clave, descripcion)
    VALUES (source.clave, source.descripcion);
GO

DECLARE @es INT = (SELECT id_idioma FROM dbo.Idioma WHERE codigo = 'es-AR');
DECLARE @en INT = (SELECT id_idioma FROM dbo.Idioma WHERE codigo = 'en-US');

MERGE dbo.Traduccion AS target
USING (
    SELECT e.id_etiqueta, @es AS id_idioma, v.texto
    FROM dbo.Etiqueta e
    INNER JOIN (VALUES
        ('AUDIT_TITLE', 'Bitacora'),
        ('AUDIT_DESCRIPTION', 'Aca vas a poder consultar los eventos del sistema.'),
        ('AUDIT_EMPTY', 'No hay eventos registrados.'),
        ('AUDIT_COUNT', '{0} evento(s) registrados.'),
        ('BTN_REFRESH', 'Actualizar'),
        ('GRID_ID', 'ID'),
        ('GRID_DATE', 'Fecha'),
        ('GRID_USER_ID', 'ID usuario'),
        ('GRID_USER', 'Usuario'),
        ('GRID_MODULE', 'Modulo'),
        ('GRID_ACTION', 'Accion'),
        ('GRID_LEVEL', 'Nivel'),
        ('GRID_DESCRIPTION', 'Descripcion'),
        ('GRID_DEVICE', 'Equipo')
    ) v(clave, texto) ON v.clave = e.clave
    WHERE @es IS NOT NULL
) AS source
    ON target.id_etiqueta = source.id_etiqueta
   AND target.id_idioma = source.id_idioma
WHEN MATCHED THEN
    UPDATE SET texto = source.texto
WHEN NOT MATCHED THEN
    INSERT (id_etiqueta, id_idioma, texto)
    VALUES (source.id_etiqueta, source.id_idioma, source.texto);

MERGE dbo.Traduccion AS target
USING (
    SELECT e.id_etiqueta, @en AS id_idioma, v.texto
    FROM dbo.Etiqueta e
    INNER JOIN (VALUES
        ('AUDIT_TITLE', 'Audit log'),
        ('AUDIT_DESCRIPTION', 'Here you can review system events.'),
        ('AUDIT_EMPTY', 'No events registered.'),
        ('AUDIT_COUNT', '{0} event(s) registered.'),
        ('BTN_REFRESH', 'Refresh'),
        ('GRID_ID', 'ID'),
        ('GRID_DATE', 'Date'),
        ('GRID_USER_ID', 'User ID'),
        ('GRID_USER', 'User'),
        ('GRID_MODULE', 'Module'),
        ('GRID_ACTION', 'Action'),
        ('GRID_LEVEL', 'Level'),
        ('GRID_DESCRIPTION', 'Description'),
        ('GRID_DEVICE', 'Device')
    ) v(clave, texto) ON v.clave = e.clave
    WHERE @en IS NOT NULL
) AS source
    ON target.id_etiqueta = source.id_etiqueta
   AND target.id_idioma = source.id_idioma
WHEN MATCHED THEN
    UPDATE SET texto = source.texto
WHEN NOT MATCHED THEN
    INSERT (id_etiqueta, id_idioma, texto)
    VALUES (source.id_etiqueta, source.id_idioma, source.texto);
GO
