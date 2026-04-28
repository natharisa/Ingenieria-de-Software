USE [TecniSalud];
GO

IF OBJECT_ID('dbo.sp_Usuario_Registrar', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_Usuario_Registrar;
END
GO

CREATE PROCEDURE dbo.sp_Usuario_Registrar
    @nombre_usuario NVARCHAR(100),
    @email NVARCHAR(255),
    @password_hash NVARCHAR(255),
    @id_usuario_nuevo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @id_idioma_default INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (
            SELECT 1
            FROM dbo.Usuario
            WHERE nombre_usuario = @nombre_usuario
        )
        BEGIN
            RAISERROR('Ya existe un usuario con ese nombre de usuario.', 16, 1);
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.Usuario
            WHERE email = @email
        )
        BEGIN
            RAISERROR('Ya existe un usuario con ese email.', 16, 1);
        END;

        SELECT @id_idioma_default = id_idioma
        FROM dbo.Idioma
        WHERE codigo = 'es-AR'
          AND estado_idioma = 'ACTIVO';

        IF @id_idioma_default IS NULL
        BEGIN
            RAISERROR('No existe un idioma activo con codigo es-AR.', 16, 1);
        END;

        INSERT INTO dbo.Usuario
        (
            id_idioma,
            nombre_usuario,
            email,
            password_hash,
            estado_usuario,
            fecha_alta
        )
        VALUES
        (
            @id_idioma_default,
            @nombre_usuario,
            @email,
            @password_hash,
            'ACTIVO',
            GETDATE()
        );

        SET @id_usuario_nuevo = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT
            u.id_usuario,
            u.id_idioma,
            u.nombre_usuario,
            u.email,
            u.estado_usuario,
            u.fecha_alta
        FROM dbo.Usuario u
        WHERE u.id_usuario = @id_usuario_nuevo;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END
GO

IF OBJECT_ID('dbo.sp_Usuario_Login', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_Usuario_Login;
END
GO

CREATE PROCEDURE dbo.sp_Usuario_Login
    @identificador NVARCHAR(255),
    @password_hash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        u.id_usuario,
        u.id_idioma,
        u.nombre_usuario,
        u.email,
        u.estado_usuario,
        u.fecha_alta
    FROM dbo.Usuario u
    WHERE (u.nombre_usuario = @identificador OR u.email = @identificador)
      AND u.password_hash = @password_hash
      AND u.estado_usuario = 'ACTIVO';

    SELECT
        ur.id_usuario,
        ur.id_rol,
        r.nombre AS nombre_rol
    FROM dbo.UsuarioRol ur
    INNER JOIN dbo.Rol r
        ON r.id_rol = ur.id_rol
    INNER JOIN dbo.Usuario u
        ON u.id_usuario = ur.id_usuario
    WHERE (u.nombre_usuario = @identificador OR u.email = @identificador)
      AND u.password_hash = @password_hash
      AND u.estado_usuario = 'ACTIVO'
      AND ur.estado_usuario_rol = 'ACTIVO'
      AND r.estado_rol = 'ACTIVO';
END
GO
