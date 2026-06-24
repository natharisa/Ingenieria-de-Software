USE [TecniSalud];
GO

DECLARE @idiomaId INT;

SELECT @idiomaId = id_idioma
FROM dbo.Idioma
WHERE codigo = 'en-US';

IF @idiomaId IS NULL
BEGIN
    INSERT INTO dbo.Idioma (codigo, nombre, estado_idioma)
    VALUES ('en-US', 'English United States', 'ACTIVO');

    SET @idiomaId = SCOPE_IDENTITY();
END;

MERGE dbo.Traduccion AS target
USING (
    SELECT e.id_etiqueta, @idiomaId AS id_idioma, v.texto
    FROM dbo.Etiqueta e
    INNER JOIN (VALUES
        ('BTN_NEW', 'New Language'),
        ('BTN_REFRESH', 'Refresh'),
        ('BTN_SAVE', 'Save'),
        ('COMPONENT_SELECTED', 'Selected Component'),
        ('COMPONENT_TREE', 'Labels'),
        ('FIELD_NAME', 'Name'),
        ('GRID_DESCRIPTION', 'Description'),
        ('GRID_ID', 'ID'),
        ('LABEL_TAG', 'Label'),
        ('LANGUAGE_ACTIVE', 'Active'),
        ('LANGUAGE_CODE', 'Language Code'),
        ('LANGUAGE_DETAIL', 'Language Details'),
        ('LANGUAGE_SELECTOR', 'Selected Language'),
        ('LANGUAGES_DESCRIPTION', 'Manage languages in the following panel:'),
        ('LANGUAGES_TITLE', 'Languages'),
        ('MENU_AUDIT', 'Audit Log'),
        ('MENU_LANGUAGES', 'Languages'),
        ('MENU_LOGOUT', 'Log out'),
        ('MENU_ROLES', 'Roles'),
        ('MENU_USERS', 'Users'),
        ('SECURITY_LANGUAGE_CREATE_DENIED', 'You do not have permission to create languages.'),
        ('SECURITY_LANGUAGE_EDIT_DENIED', 'You do not have permission to modify languages.'),
        ('SECURITY_TRANSLATION_EDIT_DENIED', 'You do not have permission to modify translations.'),
        ('TRANSLATION_DETAIL', 'Translation Details'),
        ('TRANSLATION_TEXT', 'Translation')
    ) v(clave, texto) ON v.clave = e.clave
) AS source
    ON target.id_etiqueta = source.id_etiqueta
   AND target.id_idioma = source.id_idioma
WHEN MATCHED THEN
    UPDATE SET texto = source.texto
WHEN NOT MATCHED THEN
    INSERT (id_etiqueta, id_idioma, texto)
    VALUES (source.id_etiqueta, source.id_idioma, source.texto);
GO
