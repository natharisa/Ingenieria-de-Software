-- ============================================================
-- Base de Datos: Tecni Salud
-- Script de creacion de tablas
-- Motor sugerido: SQL Server
-- ============================================================

-- Crear base de datos opcional
CREATE DATABASE TecniSalud;
 GO
 USE TecniSalud;
 GO

-- ============================================================
-- TABLAS DE SEGURIDAD, ROLES, PERMISOS E IDIOMA
-- ============================================================

CREATE TABLE Idioma (
    id_idioma INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(10) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    estado_idioma VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Idioma_Codigo UNIQUE (codigo),
    CONSTRAINT CK_Idioma_Estado CHECK (estado_idioma IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE Usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    id_idioma INT NULL,
    nombre_usuario VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    estado_usuario VARCHAR(20) NOT NULL DEFAULT 'Activo',
    fecha_alta DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Usuario_Nombre UNIQUE (nombre_usuario),
    CONSTRAINT UQ_Usuario_Email UNIQUE (email),
    CONSTRAINT CK_Usuario_Estado CHECK (estado_usuario IN ('Activo', 'Inactivo', 'Bloqueado')),
    CONSTRAINT FK_Usuario_Idioma FOREIGN KEY (id_idioma) REFERENCES Idioma(id_idioma)
);
GO

CREATE TABLE Rol (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    estado_rol VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Rol_Nombre UNIQUE (nombre),
    CONSTRAINT CK_Rol_Estado CHECK (estado_rol IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE Permiso (
    id_permiso INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(100) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    modulo VARCHAR(100) NOT NULL,
    accion VARCHAR(100) NOT NULL,
    estado_permiso VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Permiso_Codigo UNIQUE (codigo),
    CONSTRAINT CK_Permiso_Estado CHECK (estado_permiso IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE UsuarioRol (
    id_usuario_rol INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_rol INT NOT NULL,
    fecha_asignacion DATETIME NOT NULL DEFAULT GETDATE(),
    estado_usuario_rol VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_UsuarioRol UNIQUE (id_usuario, id_rol),
    CONSTRAINT CK_UsuarioRol_Estado CHECK (estado_usuario_rol IN ('Activo', 'Inactivo')),
    CONSTRAINT FK_UsuarioRol_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    CONSTRAINT FK_UsuarioRol_Rol FOREIGN KEY (id_rol) REFERENCES Rol(id_rol)
);
GO

CREATE TABLE RolPermiso (
    id_rol_permiso INT IDENTITY(1,1) PRIMARY KEY,
    id_rol INT NOT NULL,
    id_permiso INT NOT NULL,
    CONSTRAINT UQ_RolPermiso UNIQUE (id_rol, id_permiso),
    CONSTRAINT FK_RolPermiso_Rol FOREIGN KEY (id_rol) REFERENCES Rol(id_rol),
    CONSTRAINT FK_RolPermiso_Permiso FOREIGN KEY (id_permiso) REFERENCES Permiso(id_permiso)
);
GO

CREATE TABLE Etiqueta (
    id_etiqueta INT IDENTITY(1,1) PRIMARY KEY,
    clave VARCHAR(150) NOT NULL,
    descripcion VARCHAR(255) NULL,
    CONSTRAINT UQ_Etiqueta_Clave UNIQUE (clave)
);
GO

CREATE TABLE Traduccion (
    id_traduccion INT IDENTITY(1,1) PRIMARY KEY,
    id_etiqueta INT NOT NULL,
    id_idioma INT NOT NULL,
    texto NVARCHAR(500) NOT NULL,
    CONSTRAINT UQ_Traduccion UNIQUE (id_etiqueta, id_idioma),
    CONSTRAINT FK_Traduccion_Etiqueta FOREIGN KEY (id_etiqueta) REFERENCES Etiqueta(id_etiqueta),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (id_idioma) REFERENCES Idioma(id_idioma)
);
GO

-- ============================================================
-- PACIENTES, PROFESIONALES, CONSULTORIOS Y TURNOS
-- ============================================================

CREATE TABLE Paciente (
    id_paciente INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NULL,
    dni VARCHAR(20) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    fecha_nacimiento DATE NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(150) NULL,
    direccion VARCHAR(255) NULL,
    estado_paciente VARCHAR(20) NOT NULL DEFAULT 'Activo',
    fecha_alta DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Paciente_DNI UNIQUE (dni),
    CONSTRAINT UQ_Paciente_Usuario UNIQUE (id_usuario),
    CONSTRAINT CK_Paciente_Estado CHECK (estado_paciente IN ('Activo', 'Inactivo')),
    CONSTRAINT FK_Paciente_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);
GO

CREATE TABLE ObraSocial (
    id_obra_social INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    cuit VARCHAR(20) NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(150) NULL,
    estado_obra_social VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_ObraSocial_Nombre UNIQUE (nombre),
    CONSTRAINT CK_ObraSocial_Estado CHECK (estado_obra_social IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE PacienteObraSocial (
    id_paciente_obra_social INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_obra_social INT NOT NULL,
    numero_afiliado VARCHAR(100) NOT NULL,
    plan_os VARCHAR(100) NULL,
    fecha_desde DATE NOT NULL,
    fecha_hasta DATE NULL,
    estado_paciente_obra_social VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT CK_PacienteObraSocial_Estado CHECK (estado_paciente_obra_social IN ('Activo', 'Inactivo')),
    CONSTRAINT CK_PacienteObraSocial_Fechas CHECK (fecha_hasta IS NULL OR fecha_hasta >= fecha_desde),
    CONSTRAINT FK_PacienteObraSocial_Paciente FOREIGN KEY (id_paciente) REFERENCES Paciente(id_paciente),
    CONSTRAINT FK_PacienteObraSocial_ObraSocial FOREIGN KEY (id_obra_social) REFERENCES ObraSocial(id_obra_social)
);
GO

CREATE TABLE Especialidad (
    id_especialidad INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    CONSTRAINT UQ_Especialidad_Nombre UNIQUE (nombre)
);
GO

CREATE TABLE Profesional (
    id_profesional INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_especialidad INT NOT NULL,
    matricula VARCHAR(50) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(150) NULL,
    estado_profesional VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Profesional_Usuario UNIQUE (id_usuario),
    CONSTRAINT UQ_Profesional_Matricula UNIQUE (matricula),
    CONSTRAINT CK_Profesional_Estado CHECK (estado_profesional IN ('Activo', 'Inactivo')),
    CONSTRAINT FK_Profesional_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    CONSTRAINT FK_Profesional_Especialidad FOREIGN KEY (id_especialidad) REFERENCES Especialidad(id_especialidad)
);
GO

CREATE TABLE Consultorio (
    id_consultorio INT IDENTITY(1,1) PRIMARY KEY,
    nombre_o_numero VARCHAR(50) NOT NULL,
    ubicacion VARCHAR(150) NULL,
    estado_consultorio VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Consultorio_Nombre UNIQUE (nombre_o_numero),
    CONSTRAINT CK_Consultorio_Estado CHECK (estado_consultorio IN ('Activo', 'Inactivo', 'Mantenimiento'))
);
GO

CREATE TABLE JornadaProfesional (
    id_jornada INT IDENTITY(1,1) PRIMARY KEY,
    id_profesional INT NOT NULL,
    id_consultorio INT NOT NULL,
    fecha DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    duracion_turno_min INT NOT NULL,
    estado_jornada VARCHAR(20) NOT NULL DEFAULT 'Activa',
    CONSTRAINT CK_Jornada_Horario CHECK (hora_fin > hora_inicio),
    CONSTRAINT CK_Jornada_Duracion CHECK (duracion_turno_min > 0),
    CONSTRAINT CK_Jornada_Estado CHECK (estado_jornada IN ('Activa', 'Cancelada', 'Finalizada')),
    CONSTRAINT FK_Jornada_Profesional FOREIGN KEY (id_profesional) REFERENCES Profesional(id_profesional),
    CONSTRAINT FK_Jornada_Consultorio FOREIGN KEY (id_consultorio) REFERENCES Consultorio(id_consultorio)
);
GO

CREATE TABLE Turno (
    id_turno INT IDENTITY(1,1) PRIMARY KEY,
    id_jornada INT NOT NULL,
    id_paciente INT NOT NULL,
    id_paciente_obra_social INT NULL,
    fecha_hora_inicio DATETIME NOT NULL,
    fecha_hora_fin DATETIME NOT NULL,
    estado_turno VARCHAR(30) NOT NULL DEFAULT 'Reservado',
    origen_turno VARCHAR(30) NOT NULL,
    motivo_consulta VARCHAR(255) NULL,
    fecha_reserva DATETIME NOT NULL DEFAULT GETDATE(),
    id_usuario_creador INT NOT NULL,
    CONSTRAINT CK_Turno_Horario CHECK (fecha_hora_fin > fecha_hora_inicio),
    CONSTRAINT CK_Turno_Estado CHECK (estado_turno IN ('Reservado', 'Confirmado', 'Cancelado', 'Atendido', 'Ausente', 'Reprogramado')),
    CONSTRAINT CK_Turno_Origen CHECK (origen_turno IN ('Autogestionado', 'Recepcion', 'Administracion')),
    CONSTRAINT UQ_Turno_Jornada_Inicio UNIQUE (id_jornada, fecha_hora_inicio),
    CONSTRAINT FK_Turno_Jornada FOREIGN KEY (id_jornada) REFERENCES JornadaProfesional(id_jornada),
    CONSTRAINT FK_Turno_Paciente FOREIGN KEY (id_paciente) REFERENCES Paciente(id_paciente),
    CONSTRAINT FK_Turno_PacienteObraSocial FOREIGN KEY (id_paciente_obra_social) REFERENCES PacienteObraSocial(id_paciente_obra_social),
    CONSTRAINT FK_Turno_UsuarioCreador FOREIGN KEY (id_usuario_creador) REFERENCES Usuario(id_usuario)
);
GO

CREATE TABLE HistorialEstadoTurno (
    id_historial INT IDENTITY(1,1) PRIMARY KEY,
    id_turno INT NOT NULL,
    estado_anterior VARCHAR(30) NULL,
    estado_nuevo VARCHAR(30) NOT NULL,
    fecha_cambio DATETIME NOT NULL DEFAULT GETDATE(),
    motivo VARCHAR(255) NULL,
    id_usuario_responsable INT NOT NULL,
    CONSTRAINT FK_HistorialTurno_Turno FOREIGN KEY (id_turno) REFERENCES Turno(id_turno),
    CONSTRAINT FK_HistorialTurno_Usuario FOREIGN KEY (id_usuario_responsable) REFERENCES Usuario(id_usuario)
);
GO

CREATE TABLE Atencion (
    id_atencion INT IDENTITY(1,1) PRIMARY KEY,
    id_turno INT NOT NULL,
    fecha_hora_llegada DATETIME NULL,
    fecha_hora_inicio_atencion DATETIME NULL,
    fecha_hora_fin_atencion DATETIME NULL,
    observaciones_operativas VARCHAR(500) NULL,
    id_usuario_responsable INT NOT NULL,
    CONSTRAINT UQ_Atencion_Turno UNIQUE (id_turno),
    CONSTRAINT CK_Atencion_Horario CHECK (
        fecha_hora_fin_atencion IS NULL OR fecha_hora_inicio_atencion IS NULL OR fecha_hora_fin_atencion >= fecha_hora_inicio_atencion
    ),
    CONSTRAINT FK_Atencion_Turno FOREIGN KEY (id_turno) REFERENCES Turno(id_turno),
    CONSTRAINT FK_Atencion_Usuario FOREIGN KEY (id_usuario_responsable) REFERENCES Usuario(id_usuario)
);
GO

-- ============================================================
-- PAGOS DE PACIENTES
-- ============================================================

CREATE TABLE MedioPago (
    id_medio_pago INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    estado_medio_pago VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_MedioPago_Nombre UNIQUE (nombre),
    CONSTRAINT CK_MedioPago_Estado CHECK (estado_medio_pago IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE Pago (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_turno INT NOT NULL,
    id_atencion INT NULL,
    id_paciente INT NOT NULL,
    id_medio_pago INT NOT NULL,
    monto DECIMAL(12,2) NOT NULL,
    fecha_pago DATETIME NULL,
    estado_pago VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    origen_pago VARCHAR(30) NOT NULL,
    id_usuario_registro INT NOT NULL,
    observaciones VARCHAR(255) NULL,
    CONSTRAINT CK_Pago_Monto CHECK (monto >= 0),
    CONSTRAINT CK_Pago_Estado CHECK (estado_pago IN ('Pendiente', 'Pagado', 'Rechazado', 'Anulado', 'Devuelto')),
    CONSTRAINT CK_Pago_Origen CHECK (origen_pago IN ('Autogestionado', 'Recepcion', 'Administracion')),
    CONSTRAINT FK_Pago_Turno FOREIGN KEY (id_turno) REFERENCES Turno(id_turno),
    CONSTRAINT FK_Pago_Atencion FOREIGN KEY (id_atencion) REFERENCES Atencion(id_atencion),
    CONSTRAINT FK_Pago_Paciente FOREIGN KEY (id_paciente) REFERENCES Paciente(id_paciente),
    CONSTRAINT FK_Pago_MedioPago FOREIGN KEY (id_medio_pago) REFERENCES MedioPago(id_medio_pago),
    CONSTRAINT FK_Pago_Usuario FOREIGN KEY (id_usuario_registro) REFERENCES Usuario(id_usuario)
);
GO

-- ============================================================
-- INVENTARIO E INSUMOS
-- ============================================================

CREATE TABLE CategoriaInsumo (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NULL,
    CONSTRAINT UQ_CategoriaInsumo_Nombre UNIQUE (nombre)
);
GO

CREATE TABLE Insumo (
    id_insumo INT IDENTITY(1,1) PRIMARY KEY,
    id_categoria INT NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(255) NULL,
    unidad_medida VARCHAR(50) NOT NULL,
    stock_minimo DECIMAL(12,2) NOT NULL DEFAULT 0,
    punto_reposicion DECIMAL(12,2) NOT NULL DEFAULT 0,
    estado_insumo VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT CK_Insumo_StockMinimo CHECK (stock_minimo >= 0),
    CONSTRAINT CK_Insumo_PuntoReposicion CHECK (punto_reposicion >= 0),
    CONSTRAINT CK_Insumo_Estado CHECK (estado_insumo IN ('Activo', 'Inactivo')),
    CONSTRAINT FK_Insumo_Categoria FOREIGN KEY (id_categoria) REFERENCES CategoriaInsumo(id_categoria)
);
GO

CREATE TABLE LoteInsumo (
    id_lote INT IDENTITY(1,1) PRIMARY KEY,
    id_insumo INT NOT NULL,
    numero_lote VARCHAR(100) NOT NULL,
    fecha_ingreso DATE NOT NULL,
    fecha_vencimiento DATE NULL,
    cantidad_inicial DECIMAL(12,2) NOT NULL,
    cantidad_disponible DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_Lote_CantidadInicial CHECK (cantidad_inicial >= 0),
    CONSTRAINT CK_Lote_CantidadDisponible CHECK (cantidad_disponible >= 0),
    CONSTRAINT CK_Lote_DisponibleInicial CHECK (cantidad_disponible <= cantidad_inicial),
    CONSTRAINT UQ_Lote_Insumo_Numero UNIQUE (id_insumo, numero_lote),
    CONSTRAINT FK_Lote_Insumo FOREIGN KEY (id_insumo) REFERENCES Insumo(id_insumo)
);
GO

CREATE TABLE DetalleConsumoAtencion (
    id_detalle_consumo INT IDENTITY(1,1) PRIMARY KEY,
    id_atencion INT NOT NULL,
    id_insumo INT NOT NULL,
    id_lote INT NOT NULL,
    cantidad_consumida DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_DetalleConsumo_Cantidad CHECK (cantidad_consumida > 0),
    CONSTRAINT FK_DetalleConsumo_Atencion FOREIGN KEY (id_atencion) REFERENCES Atencion(id_atencion),
    CONSTRAINT FK_DetalleConsumo_Insumo FOREIGN KEY (id_insumo) REFERENCES Insumo(id_insumo),
    CONSTRAINT FK_DetalleConsumo_Lote FOREIGN KEY (id_lote) REFERENCES LoteInsumo(id_lote)
);
GO

CREATE TABLE MovimientoStock (
    id_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    id_insumo INT NOT NULL,
    id_lote INT NOT NULL,
    tipo_movimiento VARCHAR(30) NOT NULL,
    cantidad DECIMAL(12,2) NOT NULL,
    fecha_movimiento DATETIME NOT NULL DEFAULT GETDATE(),
    motivo VARCHAR(255) NULL,
    id_atencion INT NULL,
    id_usuario_responsable INT NOT NULL,
    CONSTRAINT CK_MovimientoStock_Cantidad CHECK (cantidad > 0),
    CONSTRAINT CK_MovimientoStock_Tipo CHECK (tipo_movimiento IN ('Ingreso', 'EgresoPorAtencion', 'AjusteEntrada', 'AjusteSalida', 'Vencimiento', 'Baja', 'Reposicion')),
    CONSTRAINT FK_MovimientoStock_Insumo FOREIGN KEY (id_insumo) REFERENCES Insumo(id_insumo),
    CONSTRAINT FK_MovimientoStock_Lote FOREIGN KEY (id_lote) REFERENCES LoteInsumo(id_lote),
    CONSTRAINT FK_MovimientoStock_Atencion FOREIGN KEY (id_atencion) REFERENCES Atencion(id_atencion),
    CONSTRAINT FK_MovimientoStock_Usuario FOREIGN KEY (id_usuario_responsable) REFERENCES Usuario(id_usuario)
);
GO

-- ============================================================
-- PROVEEDORES, COMPRAS Y PAGOS A PROVEEDORES
-- ============================================================

CREATE TABLE Proveedor (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    razon_social VARCHAR(150) NOT NULL,
    cuit VARCHAR(20) NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(150) NULL,
    direccion VARCHAR(255) NULL,
    estado_proveedor VARCHAR(20) NOT NULL DEFAULT 'Activo',
    CONSTRAINT UQ_Proveedor_CUIT UNIQUE (cuit),
    CONSTRAINT CK_Proveedor_Estado CHECK (estado_proveedor IN ('Activo', 'Inactivo'))
);
GO

CREATE TABLE OrdenCompra (
    id_orden_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    fecha_orden DATETIME NOT NULL DEFAULT GETDATE(),
    estado_orden VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    observaciones VARCHAR(255) NULL,
    id_usuario_creador INT NOT NULL,
    CONSTRAINT CK_OrdenCompra_Estado CHECK (estado_orden IN ('Pendiente', 'Aprobada', 'Recibida', 'Cancelada')),
    CONSTRAINT FK_OrdenCompra_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor),
    CONSTRAINT FK_OrdenCompra_Usuario FOREIGN KEY (id_usuario_creador) REFERENCES Usuario(id_usuario)
);
GO

CREATE TABLE DetalleOrdenCompra (
    id_detalle_orden INT IDENTITY(1,1) PRIMARY KEY,
    id_orden_compra INT NOT NULL,
    id_insumo INT NOT NULL,
    cantidad DECIMAL(12,2) NOT NULL,
    precio_unitario DECIMAL(12,2) NOT NULL,
    CONSTRAINT CK_DetalleOrden_Cantidad CHECK (cantidad > 0),
    CONSTRAINT CK_DetalleOrden_Precio CHECK (precio_unitario >= 0),
    CONSTRAINT FK_DetalleOrden_Orden FOREIGN KEY (id_orden_compra) REFERENCES OrdenCompra(id_orden_compra),
    CONSTRAINT FK_DetalleOrden_Insumo FOREIGN KEY (id_insumo) REFERENCES Insumo(id_insumo)
);
GO

CREATE TABLE FacturaProveedor (
    id_factura_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    id_proveedor INT NOT NULL,
    id_orden_compra INT NULL,
    numero_factura VARCHAR(100) NOT NULL,
    fecha_emision DATE NOT NULL,
    fecha_vencimiento DATE NULL,
    importe_total DECIMAL(12,2) NOT NULL,
    estado_factura VARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    observaciones VARCHAR(255) NULL,
    CONSTRAINT CK_FacturaProveedor_Importe CHECK (importe_total >= 0),
    CONSTRAINT CK_FacturaProveedor_Estado CHECK (estado_factura IN ('Pendiente', 'Pagada', 'PagadaParcial', 'Anulada', 'Vencida')),
    CONSTRAINT UQ_FacturaProveedor UNIQUE (id_proveedor, numero_factura),
    CONSTRAINT FK_FacturaProveedor_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedor(id_proveedor),
    CONSTRAINT FK_FacturaProveedor_Orden FOREIGN KEY (id_orden_compra) REFERENCES OrdenCompra(id_orden_compra)
);
GO

CREATE TABLE PagoProveedor (
    id_pago_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    id_factura_proveedor INT NOT NULL,
    id_medio_pago INT NOT NULL,
    fecha_pago DATETIME NOT NULL DEFAULT GETDATE(),
    importe_pagado DECIMAL(12,2) NOT NULL,
    nro_comprobante VARCHAR(100) NULL,
    observaciones VARCHAR(255) NULL,
    id_usuario_registro INT NOT NULL,
    CONSTRAINT CK_PagoProveedor_Importe CHECK (importe_pagado > 0),
    CONSTRAINT FK_PagoProveedor_Factura FOREIGN KEY (id_factura_proveedor) REFERENCES FacturaProveedor(id_factura_proveedor),
    CONSTRAINT FK_PagoProveedor_MedioPago FOREIGN KEY (id_medio_pago) REFERENCES MedioPago(id_medio_pago),
    CONSTRAINT FK_PagoProveedor_Usuario FOREIGN KEY (id_usuario_registro) REFERENCES Usuario(id_usuario)
);
GO

-- ============================================================
-- INDICES RECOMENDADOS
-- ============================================================

CREATE INDEX IX_Turno_Paciente ON Turno(id_paciente);
CREATE INDEX IX_Turno_Jornada ON Turno(id_jornada);
CREATE INDEX IX_Turno_FechaHora ON Turno(fecha_hora_inicio, fecha_hora_fin);
CREATE INDEX IX_Pago_Turno ON Pago(id_turno);
CREATE INDEX IX_Pago_Paciente ON Pago(id_paciente);
CREATE INDEX IX_MovimientoStock_Insumo ON MovimientoStock(id_insumo);
CREATE INDEX IX_MovimientoStock_Lote ON MovimientoStock(id_lote);
CREATE INDEX IX_LoteInsumo_Vencimiento ON LoteInsumo(fecha_vencimiento);
CREATE INDEX IX_FacturaProveedor_Proveedor ON FacturaProveedor(id_proveedor);
GO

-- ============================================================
-- DATOS INICIALES SUGERIDOS
-- ============================================================

INSERT INTO Idioma (codigo, nombre) VALUES
('es-AR', 'Español Argentina'),
('en-US', 'Inglés Estados Unidos');
GO

INSERT INTO MedioPago (nombre, descripcion) VALUES
('Efectivo', 'Pago en efectivo'),
('Tarjeta de débito', 'Pago con tarjeta de débito'),
('Tarjeta de crédito', 'Pago con tarjeta de crédito'),
('Transferencia', 'Pago por transferencia bancaria'),
('Mercado Pago', 'Pago mediante billetera virtual'),
('Obra social', 'Cobertura mediante obra social');
GO

INSERT INTO Rol (nombre, descripcion) VALUES
('Administrador', 'Acceso total al sistema'),
('Paciente', 'Usuario paciente con autogestión de turnos'),
('Recepcionista', 'Gestión de turnos y pacientes'),
('Profesional', 'Gestión de atenciones'),
('EncargadoStock', 'Gestión de inventario e insumos'),
('Direccion', 'Consulta de reportes e indicadores');
GO

-- ============================================================
-- FIN DEL SCRIPT
-- ============================================================
