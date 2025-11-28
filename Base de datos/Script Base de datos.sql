-- Versión del servidor 
SELECT version(); -- Se creo en PostgreSQL 9.5.25, compiled by Visual C++ build 1800, 64-bit

-- Schema
CREATE SCHEMA test_sch AUTHORIZATION postgres;

-- Secuencia
CREATE SEQUENCE test_sch.seq_usuarios
    INCREMENT BY 1
    START WITH 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
	
-- Tabla
CREATE TABLE test_sch.usuarios (
    id               integer      NOT NULL
        DEFAULT nextval('test_sch.seq_usuarios')
        CONSTRAINT pk_usuarios PRIMARY KEY,

    nombres          varchar(50)  NOT NULL,
    apellidos        varchar(50)  NOT NULL,
    fechanacimiento  date         NOT NULL,
    direccion        varchar(150) NOT NULL,
    password         varchar(120) NOT NULL,
    telefono         varchar(8)   NOT NULL,
    email            varchar(100) NOT NULL,

    estado           char(1)      NULL
        CONSTRAINT ck_usuarios_estado CHECK (estado IN ('A','I')),

    fechacreacion    timestamp(0) NOT NULL,
    fechamodificacion timestamp(0) NULL
);

-- Para que la secuencia quede “propietaria” de la columna
ALTER SEQUENCE test_sch.seq_usuarios OWNED BY test_sch.usuarios.id;

-- Funciones para fecha de creación/modificación
CREATE OR REPLACE FUNCTION test_sch.fn_usuarios_set_fechas()
RETURNS trigger
LANGUAGE plpgsql
AS
$$
BEGIN
    IF TG_OP = 'INSERT' THEN
        IF NEW.fechacreacion IS NULL THEN
            NEW.fechacreacion := now();
        END IF;

        IF NEW.fechamodificacion IS NULL THEN
            NEW.fechamodificacion := now();
        END IF;

    ELSIF TG_OP = 'UPDATE' THEN
        NEW.fechamodificacion := now();
    END IF;

    RETURN NEW;
END;
$$;

-- Trigger para la tabla Usuarios
CREATE TRIGGER tr_usuarios_fechas
BEFORE INSERT OR UPDATE ON test_sch.usuarios
FOR EACH ROW
EXECUTE PROCEDURE test_sch.fn_usuarios_set_fechas();