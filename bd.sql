use base;
use RepositorioUA;
drop database RepositorioUA;
create database RepositorioUA;
use RepositorioUA;


create table usuario(
idusuario int primary key identity(1,1),
usuario varchar(50),
contrasena varchar(500),
fkrol int,
);

create table rol(
idrol int primary key IDENTITY(1,1) ,
nombre varchar(50),
fkestadorol int
);
create table estadorol(
idestadorol int primary key IDENTITY(1,1) ,
nombre varchar(50)
);


create table privilegio(
idprivilegio int primary key IDENTITY(1,1) ,
fkestadoprivilegio int,
fkrol int,
fkmenu int,
);


create table estadoprivilegio(
idestadoprivilegio int primary key identity(1,1),
nombre varchar(50)
);

create table menu(
idmenu int primary key identity(1,1),
nombre varchar(50),
ruta varchar(50),
extension varchar(50),
mid varchar(50),
fkmenu int
);


alter table usuario add constraint fkroll2 foreign key(fkrol) references rol(idrol);
alter table privilegio add constraint fkroll foreign key(fkrol) references rol(idrol);
alter table privilegio add constraint fkmenu foreign key(fkmenu) references menu(idmenu);
alter table menu add constraint fksm foreign key(fkmenu) references menu(idmenu);
alter table privilegio add constraint fkestadop foreign key(fkestadoprivilegio) references estadoprivilegio(idestadoprivilegio)
alter table rol add constraint fkrolesta  foreign key (fkestadorol) references estadorol(idestadorol);


insert into estadorol values('Activo');
insert into estadorol values('Inactivo');
insert into estadoprivilegio values('Activo');
insert into estadoprivilegio values('Inactivo');
insert into rol values('Administrador',1);
insert into rol values('Secretario',1);
insert into usuario values('usuario1','123',1);
insert into usuario values('usuario2','123',2);

insert into menu values('Directorios','','Directorio','mid',null);
insert into menu values('Directorio 1','','Directorio','mid',1);
insert into menu values('Archivo 1','','PDF','62ee849902809544acbb9032',2);
insert into menu values('Directorio 2','','Directorio','mid',2);
insert into menu values('Directorio 3','','Directorio','mid',2);
insert into menu values('Archivo 2','','PDF','62ee849902809544acbb9032',5);
insert into menu values('Directorio 4','','Directorio','mid',1);

insert into privilegio values(1,1,1);
insert into privilegio values(1,1,2);
insert into privilegio values(1,1,3);
insert into privilegio values(1,1,4);
insert into privilegio values(1,1,5);
insert into privilegio values(1,1,6);
insert into privilegio values(1,1,7);

select * from menu;
select * from rol;
select * from privilegio;
