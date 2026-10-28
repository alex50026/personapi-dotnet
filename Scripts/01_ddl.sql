IF DB_ID('persona_db') IS NULL
    CREATE DATABASE persona_db;
GO

USE persona_db;
GO

CREATE TABLE persona (
    cc       BIGINT      NOT NULL PRIMARY KEY,
    nombre   VARCHAR(45) NOT NULL,
    apellido VARCHAR(45) NOT NULL,
    genero   CHAR(1)     NOT NULL CHECK (genero IN ('M','F')),
    edad     INT         NULL
);

CREATE TABLE profesion (
    id   INT          NOT NULL PRIMARY KEY,
    nom  VARCHAR(90)  NOT NULL,
    des  VARCHAR(MAX) NULL
);

CREATE TABLE estudios (
    id_prof INT         NOT NULL,
    cc_per  BIGINT      NOT NULL,
    fecha   DATE        NULL,
    univer  VARCHAR(50) NULL,
    CONSTRAINT PK_estudios PRIMARY KEY (id_prof, cc_per),
    CONSTRAINT FK_estudios_profesion FOREIGN KEY (id_prof) REFERENCES profesion(id),
    CONSTRAINT FK_estudios_persona   FOREIGN KEY (cc_per)  REFERENCES persona(cc)
);

CREATE TABLE telefono (
    num    VARCHAR(15) NOT NULL PRIMARY KEY,
    oper   VARCHAR(45) NOT NULL,
    duenio BIGINT      NOT NULL,
    CONSTRAINT FK_telefono_persona FOREIGN KEY (duenio) REFERENCES persona(cc)
);
GO