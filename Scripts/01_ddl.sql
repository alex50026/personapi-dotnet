IF DB_ID('persona_db') IS NULL
    CREATE DATABASE persona_db;
GO


ALTER AUTHORIZATION ON DATABASE::persona_db TO sa;
GO

USE persona_db;
GO

IF OBJECT_ID(N'dbo.persona', N'U') IS NULL
CREATE TABLE persona (
    cc       BIGINT      NOT NULL,
    nombre   VARCHAR(45) NOT NULL,
    apellido VARCHAR(45) NOT NULL,
    genero   CHAR(1)     NOT NULL,
    edad     INT         NULL,
    CONSTRAINT PK_persona        PRIMARY KEY (cc),
    CONSTRAINT CK_persona_genero CHECK (genero IN ('M','F'))
);
GO

IF OBJECT_ID(N'dbo.profesion', N'U') IS NULL
CREATE TABLE profesion (
    id   INT          NOT NULL,
    nom  VARCHAR(90)  NOT NULL,
    des  VARCHAR(MAX) NULL,
    CONSTRAINT PK_profesion PRIMARY KEY (id)
);
GO

IF OBJECT_ID(N'dbo.estudios', N'U') IS NULL
CREATE TABLE estudios (
    id_prof INT         NOT NULL,
    cc_per  BIGINT      NOT NULL,
    fecha   DATE        NULL,
    univer  VARCHAR(50) NULL,
    CONSTRAINT PK_estudios PRIMARY KEY (id_prof, cc_per),
    CONSTRAINT FK_estudios_profesion FOREIGN KEY (id_prof) REFERENCES profesion(id),
    CONSTRAINT FK_estudios_persona   FOREIGN KEY (cc_per)  REFERENCES persona(cc)
);
GO

IF OBJECT_ID(N'dbo.telefono', N'U') IS NULL
CREATE TABLE telefono (
    num    VARCHAR(15) NOT NULL,
    oper   VARCHAR(45) NOT NULL,
    duenio BIGINT      NOT NULL,
    CONSTRAINT PK_telefono         PRIMARY KEY (num),
    CONSTRAINT FK_telefono_persona FOREIGN KEY (duenio) REFERENCES persona(cc)
);
GO