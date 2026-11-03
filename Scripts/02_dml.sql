-- =============================================================
-- personapi-dotnet - Script DML (datos de prueba)
-- Idempotente: cada tabla solo se llena si está vacía.
-- Orden: primero las tablas padre (persona, profesion) y luego
-- las que tienen llaves foráneas (estudios, telefono).
-- =============================================================
USE persona_db;
GO

IF NOT EXISTS (SELECT 1 FROM persona)
INSERT INTO persona (cc, nombre, apellido, genero, edad) VALUES
    (1012345678, 'Laura',  'Gomez',    'F', 21),
    (1023456789, 'Andres', 'Martinez', 'M', 23),
    (1034567890, 'Camila', 'Rodriguez','F', 22),
    (79123456,   'Carlos', 'Perez',    'M', 45);
GO

IF NOT EXISTS (SELECT 1 FROM profesion)
INSERT INTO profesion (id, nom, des) VALUES
    (1, 'Ingenieria de Sistemas', 'Diseno y desarrollo de sistemas de informacion y software'),
    (2, 'Medicina',               'Diagnostico, tratamiento y prevencion de enfermedades'),
    (3, 'Derecho',                'Estudio y aplicacion del ordenamiento juridico'),
    (4, 'Administracion',         'Gestion de organizaciones y recursos');
GO

IF NOT EXISTS (SELECT 1 FROM estudios)
INSERT INTO estudios (id_prof, cc_per, fecha, univer) VALUES
    (1, 1012345678, '2026-06-15', 'Pontificia Universidad Javeriana'),
    (1, 1023456789, '2025-12-10', 'Universidad Nacional de Colombia'),
    (2, 1034567890, '2026-06-20', 'Universidad del Rosario'),
    (3, 79123456,   '2004-11-30', 'Universidad Externado'),
    (4, 79123456,   '2010-07-01', 'Universidad de los Andes');
GO

IF NOT EXISTS (SELECT 1 FROM telefono)
INSERT INTO telefono (num, oper, duenio) VALUES
    ('3001234567', 'Claro',    1012345678),
    ('3109876543', 'Movistar', 1023456789),
    ('3205556677', 'Tigo',     1034567890),
    ('3157778899', 'Claro',    79123456),
    ('6015551234', 'ETB',      79123456);
GO
