create database usuario;
use usuario;

create table usuarios (
idUsuario int primary key auto_increment not null,
nome varchar(45) not null
);

alter table usuarios add column (username varchar(45) not null);
alter table usuarios add column (senha varchar(45) not null);

select * from usuarios where username = '{username}' and senha = '{senha}';

insert into usuarios values
(null, 'Alexandre', 'alecosta', '123');

