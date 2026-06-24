USE [TecniSalud];
GO

DECLARE @es INT = (SELECT id_idioma FROM dbo.Idioma WHERE codigo = 'es-AR');
DECLARE @en INT = (SELECT id_idioma FROM dbo.Idioma WHERE codigo = 'en-US');

MERGE dbo.Traduccion AS target
USING (
    SELECT e.id_etiqueta, @es AS id_idioma, v.texto
    FROM dbo.Etiqueta e
    INNER JOIN (VALUES
        ('MAIN_TITLE', 'Panel principal'),
        ('MAIN_USER', 'Usuario: {0}'),
        ('MAIN_NO_SESSION', 'Usuario: sin sesion'),
        ('MENU_AUDIT', 'Bitacora'),
        ('MENU_USERS', 'Usuarios'),
        ('MENU_ROLES', 'Roles'),
        ('MENU_LANGUAGES', 'Idiomas'),
        ('MENU_LOGOUT', 'Salir'),
        ('LANGUAGE_SELECTOR', 'Idioma seleccionado'),
        ('SECURITY_ACCESS_DENIED', 'No tenes permisos para acceder a esta seccion.'),
        ('NO_PERMISSIONS_ASSIGNED', 'No tenes permisos asignados.'),
        ('USERS_TITLE', 'Usuarios'),
        ('USERS_DESCRIPTION', 'Alta, modificacion e inhabilitacion de usuarios del sistema.'),
        ('USERS_DETAIL', 'Detalle de usuario'),
        ('USERS_CREATE_MODE', 'Crear usuario'),
        ('USERS_EDIT_MODE', 'Modificar usuario'),
        ('FIELD_USER', 'Usuario'),
        ('FIELD_EMAIL', 'Email'),
        ('FIELD_NAME', 'Nombre'),
        ('FIELD_LASTNAME', 'Apellido'),
        ('FIELD_NEW_PASSWORD', 'Contrasena nueva'),
        ('FIELD_STATUS', 'Estado'),
        ('GRID_ID', 'ID'),
        ('GRID_USER', 'Usuario'),
        ('GRID_EMAIL', 'Email'),
        ('GRID_STATUS', 'Estado'),
        ('BTN_NEW', 'Nuevo'),
        ('BTN_CREATE', 'Crear'),
        ('BTN_SAVE', 'Guardar'),
        ('BTN_DISABLE', 'Inhabilitar'),
        ('USER_ROLES', 'Roles de usuario'),
        ('USER_ROLE_SELECT_HELP', 'Selecciona un usuario para asignarle un rol.'),
        ('USER_ROLE_SELECT_ONE', 'Selecciona el rol que queres asignar.'),
        ('USER_ROLE_EMPTY', 'No hay roles disponibles.'),
        ('USER_ROLE_EDIT_DENIED', 'No tenes permisos para modificar roles de usuarios.'),
        ('ROLES_TITLE', 'Roles'),
        ('ROLES_DESCRIPTION', 'Administracion de roles y permisos del sistema.'),
        ('ROLES_STRUCTURE', 'Estructura de roles'),
        ('ROLES_ADMIN', 'Administracion de roles'),
        ('ROLE_CODE', 'Codigo'),
        ('ROLE_SELECTED_FAMILY', 'Familia seleccionada en el arbol'),
        ('ROLE_CHILD_COMPONENT', 'Permiso o familia a agregar'),
        ('ROLE_SELECT_FAMILY', 'Selecciona una familia del arbol.'),
        ('ROLE_SELECT_CHILD', 'Selecciona un permiso o familia para quitar.'),
        ('ROLE_SELECT_FAMILY_AND_COMPONENT', 'Selecciona una familia y un componente.'),
        ('ROLE_CREATED', 'Rol creado correctamente.'),
        ('ROLE_CREATE_ERROR', 'No se pudo crear el rol.'),
        ('ROLE_RELATION_ADDED', 'Relacion agregada correctamente.'),
        ('ROLE_RELATION_ADD_ERROR', 'No se pudo agregar la relacion.'),
        ('ROLE_RELATION_REMOVED', 'Relacion quitada correctamente.'),
        ('ROLE_RELATION_REMOVE_ERROR', 'No se pudo quitar la relacion.'),
        ('ROLE_RELATION_IDENTIFY_ERROR', 'No se pudo identificar la relacion.'),
        ('ROLE_SELF_REFERENCE_ERROR', 'Un rol no puede contenerse a si mismo.'),
        ('ROLE_INVALID_PARENT', 'La familia seleccionada no es valida.'),
        ('ROLE_INVALID_CHILD', 'El componente seleccionado no es valido.'),
        ('ROLE_CYCLE_ERROR', 'La relacion genera un ciclo.'),
        ('BTN_CREATE_ROLE', 'Crear rol'),
        ('BTN_ADD', 'Agregar'),
        ('BTN_REMOVE_SELECTED', 'Quitar seleccionado'),
        ('BTN_REMOVE_FROM', 'Quitar de {0}'),
        ('SECURITY_ROLE_CREATE_DENIED', 'No tenes permisos para crear roles.'),
        ('SECURITY_ROLE_EDIT_DENIED', 'No tenes permisos para modificar roles.')
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
        ('MAIN_TITLE', 'Main panel'),
        ('MAIN_USER', 'User: {0}'),
        ('MAIN_NO_SESSION', 'User: no session'),
        ('MENU_AUDIT', 'Audit log'),
        ('MENU_USERS', 'Users'),
        ('MENU_ROLES', 'Roles'),
        ('MENU_LANGUAGES', 'Languages'),
        ('MENU_LOGOUT', 'Log out'),
        ('LANGUAGE_SELECTOR', 'Selected language'),
        ('SECURITY_ACCESS_DENIED', 'You do not have permission to access this section.'),
        ('NO_PERMISSIONS_ASSIGNED', 'You do not have assigned permissions.'),
        ('USERS_TITLE', 'Users'),
        ('USERS_DESCRIPTION', 'Create, edit and disable system users.'),
        ('USERS_DETAIL', 'User details'),
        ('USERS_CREATE_MODE', 'Create user'),
        ('USERS_EDIT_MODE', 'Edit user'),
        ('FIELD_USER', 'User'),
        ('FIELD_EMAIL', 'Email'),
        ('FIELD_NAME', 'Name'),
        ('FIELD_LASTNAME', 'Last name'),
        ('FIELD_NEW_PASSWORD', 'New password'),
        ('FIELD_STATUS', 'Status'),
        ('GRID_ID', 'ID'),
        ('GRID_USER', 'User'),
        ('GRID_EMAIL', 'Email'),
        ('GRID_STATUS', 'Status'),
        ('BTN_NEW', 'New'),
        ('BTN_CREATE', 'Create'),
        ('BTN_SAVE', 'Save'),
        ('BTN_DISABLE', 'Disable'),
        ('USER_ROLES', 'User roles'),
        ('USER_ROLE_SELECT_HELP', 'Select a user to assign a role.'),
        ('USER_ROLE_SELECT_ONE', 'Select the role you want to assign.'),
        ('USER_ROLE_EMPTY', 'There are no roles available.'),
        ('USER_ROLE_EDIT_DENIED', 'You do not have permission to modify user roles.'),
        ('ROLES_TITLE', 'Roles'),
        ('ROLES_DESCRIPTION', 'Manage system roles and permissions.'),
        ('ROLES_STRUCTURE', 'Role structure'),
        ('ROLES_ADMIN', 'Role administration'),
        ('ROLE_CODE', 'Code'),
        ('ROLE_SELECTED_FAMILY', 'Family selected in the tree'),
        ('ROLE_CHILD_COMPONENT', 'Permission or family to add'),
        ('ROLE_SELECT_FAMILY', 'Select a family from the tree.'),
        ('ROLE_SELECT_CHILD', 'Select a permission or family to remove.'),
        ('ROLE_SELECT_FAMILY_AND_COMPONENT', 'Select a family and a component.'),
        ('ROLE_CREATED', 'Role created successfully.'),
        ('ROLE_CREATE_ERROR', 'Could not create the role.'),
        ('ROLE_RELATION_ADDED', 'Relation added successfully.'),
        ('ROLE_RELATION_ADD_ERROR', 'Could not add the relation.'),
        ('ROLE_RELATION_REMOVED', 'Relation removed successfully.'),
        ('ROLE_RELATION_REMOVE_ERROR', 'Could not remove the relation.'),
        ('ROLE_RELATION_IDENTIFY_ERROR', 'Could not identify the relation.'),
        ('ROLE_SELF_REFERENCE_ERROR', 'A role cannot contain itself.'),
        ('ROLE_INVALID_PARENT', 'The selected family is invalid.'),
        ('ROLE_INVALID_CHILD', 'The selected component is invalid.'),
        ('ROLE_CYCLE_ERROR', 'The relation creates a cycle.'),
        ('BTN_CREATE_ROLE', 'Create role'),
        ('BTN_ADD', 'Add'),
        ('BTN_REMOVE_SELECTED', 'Remove selected'),
        ('BTN_REMOVE_FROM', 'Remove from {0}'),
        ('SECURITY_ROLE_CREATE_DENIED', 'You do not have permission to create roles.'),
        ('SECURITY_ROLE_EDIT_DENIED', 'You do not have permission to modify roles.')
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
