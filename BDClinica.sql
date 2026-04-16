-- Servidor de Datos: BDClinica
-- Autor: Basado en Grupo D.E.V.
-- Fecha: 15 de abril del 2026

use master
go

if DB_ID('BDClinica') is not null
   drop database BDClinica
go

create database BDClinica
go

use BDClinica
go

--------------------------------------------------------------------------------------------
-- TABLA PACIENTE
--------------------------------------------------------------------------------------------
if OBJECT_ID('TPaciente','U') is not null
	drop table TPaciente
go 
create table TPaciente
(
	CodPaciente 	char(5) not null,
	APaterno 	    varchar(50) not null,
	AMaterno 	    varchar(50) not null,
	Nombres 	    varchar(50) not null,
	DNI             char(8) unique not null,
	primary key (CodPaciente)
)  
go

--------------------------------------------------------------------------------------------
-- TABLA MEDICO
--------------------------------------------------------------------------------------------
if OBJECT_ID('TMedico','U') is not null
	drop table TMedico
go 
create table TMedico
(
	CodMedico 	    char(3) not null,
	APaterno 	    varchar(50) not null,
	AMaterno 	    varchar(50) not null,
	Nombres 	    varchar(50) not null,
	Especialidad    varchar(50) not null,
	primary key (CodMedico)
)  
go

--------------------------------------------------------------------------------------------
-- TABLA CITA
--------------------------------------------------------------------------------------------
if OBJECT_ID('TCita','U') is not null
	drop table TCita
go 
create table TCita
(
	IdCita          int identity(1,1),
	CodPaciente     char(5) not null,
	CodMedico       char(3) not null,
	FechaCita       datetime not null,
	Motivo          varchar(200),
	primary key (IdCita),
	foreign key (CodPaciente) references TPaciente,
	foreign key (CodMedico) references TMedico
)
go

use BDClinica
go
-- INSERTAMOS DATOS A LAS TABLAS
--------------------------------------------------------------------------------------------
-- DATOS PARA LA TABLA: TPaciente
--------------------------------------------------------------------------------------------
insert into TPaciente values ('P0001', 'García', 'Mendoza', 'Juan Carlos', '45678912')
insert into TPaciente values ('P0002', 'Rodríguez', 'Pérez', 'María Elena', '70895623')
insert into TPaciente values ('P0003', 'Sánchez', 'López', 'Ricardo', '10234567')
insert into TPaciente values ('P0004', 'Quispe', 'Mamani', 'Lucía', '44556677')
insert into TPaciente values ('P0005', 'Villanueva', 'Castro', 'Roberto', '09876543')
insert into TPaciente values ('P0006', 'Torres', 'Arias', 'Andrea', '12341234')
insert into TPaciente values ('P0007', 'Flores', 'Hidalgo', 'Luis Alberto', '88776655')
insert into TPaciente values ('P0008', 'Morales', 'Ramos', 'Carmen', '55443322')
insert into TPaciente values ('P0009', 'Ortiz', 'Guzmán', 'Fernando', '33221100')
insert into TPaciente values ('P0010', 'Vargas', 'Blanco', 'Patricia', '99887766')
go

--------------------------------------------------------------------------------------------
-- DATOS PARA LA TABLA: TMedico
--------------------------------------------------------------------------------------------
insert into TMedico values ('M01', 'Zevallos', 'Prado', 'Héctor', 'Cardiología')
insert into TMedico values ('M02', 'Dávila', 'Reyes', 'Mónica', 'Pediatría')
insert into TMedico values ('M03', 'Chávez', 'Luna', 'Gustavo', 'Dermatología')
insert into TMedico values ('M04', 'Maldonado', 'Solís', 'Rosa', 'Ginecología')
insert into TMedico values ('M05', 'Benítez', 'Valle', 'Julio', 'Oftalmología')
insert into TMedico values ('M06', 'Paredes', 'Soto', 'Sofía', 'Neurología')
insert into TMedico values ('M07', 'Campos', 'Riva', 'Andrés', 'Traumatología')
insert into TMedico values ('M08', 'Espinosa', 'León', 'Elena', 'Endocrinología')
go

--------------------------------------------------------------------------------------------
-- DATOS PARA LA TABLA: TCita
-- Nota: IdCita es identity, no se incluye en el insert
--------------------------------------------------------------------------------------------
-- Citas para Cardiología (M01)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0001', 'M01', '2026-05-10 09:00', 'Chequeo preventivo de presión arterial')
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0005', 'M01', '2026-05-10 10:30', 'Arritmia detectada en examen previo')

-- Citas para Pediatría (M02)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0002', 'M02', '2026-05-11 15:00', 'Control de crecimiento y vacunas')
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0004', 'M02', '2026-05-11 16:00', 'Cuadro febril persistente')

-- Citas para Dermatología (M03)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0003', 'M03', '2026-05-12 08:30', 'Alergia cutánea en extremidades')
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0006', 'M03', '2026-05-12 09:30', 'Evaluación de lunares')

-- Citas para Ginecología (M04)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0008', 'M04', '2026-05-13 11:00', 'Control prenatal primer trimestre')

-- Citas para Traumatología (M07)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0007', 'M07', '2026-05-14 14:00', 'Dolor lumbar crónico')
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0010', 'M07', '2026-05-14 15:30', 'Posible esguince de tobillo')

-- Cita para Neurología (M06)
insert into TCita (CodPaciente, CodMedico, FechaCita, Motivo) 
values ('P0009', 'M06', '2026-05-15 17:00', 'Migrañas frecuentes')
go

-- PROCEDIMIENTOS ALMACENADOS
-- LISTAR
if OBJECT_ID('spListarPaciente') is not null drop proc spListarPaciente
go
create proc spListarPaciente
as
begin
	select * from TPaciente
end
go

--AGREGAR
if OBJECT_ID('spAgregarPaciente') is not null drop proc spAgregarPaciente
go
create proc spAgregarPaciente
@CodPaciente char(5), @APaterno varchar(50), @AMaterno varchar(50), @Nombres varchar(50), @DNI char(8)
as
begin
	if not exists(select CodPaciente from TPaciente where CodPaciente = @CodPaciente)
		if not exists(select DNI from TPaciente where DNI = @DNI)
		begin
			insert into TPaciente values(@CodPaciente, @APaterno, @AMaterno, @Nombres, @DNI)
			select CodError = 0, Mensaje = 'Paciente registrado correctamente'
		end
		else select CodError = 1, Mensaje = 'Error: El DNI ya está registrado'
	else select CodError = 1, Mensaje = 'Error: El CodPaciente ya existe'
end
go

--ACTUALIZAR
if OBJECT_ID('spActualizarPaciente') is not null drop proc spActualizarPaciente
go
create proc spActualizarPaciente
@CodPaciente char(5), @APaterno varchar(50), @AMaterno varchar(50), @Nombres varchar(50), @DNI char(8)
as
begin
    if exists (select CodPaciente from TPaciente where CodPaciente = @CodPaciente)
    begin
        update TPaciente 
        set APaterno = @APaterno, AMaterno = @AMaterno, Nombres = @Nombres, DNI = @DNI
        where CodPaciente = @CodPaciente
        select CodError = 0, Mensaje = 'Datos del paciente actualizados'
	end
	else select CodError = 1, Mensaje = 'Error: CodPaciente no existe'
end
go

--ELIMINAR
if OBJECT_ID('spEliminarPaciente') is not null drop proc spEliminarPaciente
go
create proc spEliminarPaciente
@CodPaciente char(5)
as
begin
	if exists (select CodPaciente from TPaciente where CodPaciente = @CodPaciente)
		-- Validar que no tenga citas registradas antes de borrar
		if not exists(select CodPaciente from TCita where CodPaciente = @CodPaciente)
		begin
			delete from TPaciente where CodPaciente = @CodPaciente
			select CodError = 0, Mensaje = 'Paciente eliminado correctamente'
		end
		else select CodError = 1, Mensaje = 'Error: No se puede eliminar, el paciente tiene citas pendientes'
	else select CodError = 1, Mensaje = 'Error: CodPaciente no existe'
end
go

--if OBJECT_ID('spBuscarPaciente') is not null drop proc spBuscarPaciente
go
create proc spBuscarPaciente
@CodPaciente char(5)
as
begin
    if exists (select CodPaciente from TPaciente where CodPaciente = @CodPaciente)
        select * from TPaciente where CodPaciente = @CodPaciente
    else 
        select CodError = 1, Mensaje = 'Error: El paciente no existe'
end
go


--PRUEBAS DE PROCEDIMINETOS ALMACENADOS

-- Caso A: Insertar un paciente nuevo (Éxito)
exec spAgregarPaciente 'P0011', 'Soto', 'Meza', 'Carlos', '11223344'

-- Caso B: Intentar insertar con un CodPaciente que ya existe (Error)
-- Debería retornar: "Error: El CodPaciente ya existe"
exec spAgregarPaciente 'P0011', 'Luna', 'Paz', 'Diego', '99999999'

-- Caso C: Intentar insertar con un DNI que ya pertenece a otro paciente (Error)
-- Debería retornar: "Error: El DNI ya está registrado"
exec spAgregarPaciente 'P0012', 'Ríos', 'Casas', 'Ana', '11223344'
go

-- AGREGAR PACIENTES

-- Caso A: Actualizar datos de un paciente existente (Éxito)
-- Vamos a cambiar el apellido del paciente P0011
exec spActualizarPaciente 'P0011', 'Soto', 'Zegarra', 'Carlos Alberto', '11223344'

-- Caso B: Intentar actualizar un código que no existe (Error)
-- Debería retornar: "Error: CodPaciente no existe"
exec spActualizarPaciente 'P9999', 'Error', 'Error', 'Error', '00000000'
go

--LISTAR Y BUSCAR PACIENTES

-- Caso A: Buscar un paciente existente
exec spBuscarPaciente 'P0001'

-- Caso B: Buscar un paciente que no existe (Error controlado)
exec spBuscarPaciente 'P5000'

-- Caso C: Listar todos los pacientes
exec spListarPaciente
go

--ELIMINAR PACIENTES

-- Caso A: Intentar eliminar un paciente que TIENE citas (Error)
-- El paciente P0001 tiene una cita en la data que te pasé anteriormente.
-- Debería retornar: "Error: No se puede eliminar, el paciente tiene citas pendientes"
exec spEliminarPaciente 'P0001'

-- Caso B: Eliminar un paciente que NO tiene citas (Éxito)
-- Usaremos el paciente P0011 que creamos en el paso 1 de esta lista.
exec spEliminarPaciente 'P0011'

-- Caso C: Intentar eliminar un código que no existe (Error)
exec spEliminarPaciente 'PXXXX'
go

--VERIFICAR LAS TABLA PACIENTE
select * from TPaciente;
-- Nota que P0011 ya no debería estar si el Caso 4-B fue exitoso.