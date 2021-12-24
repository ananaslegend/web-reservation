create table guests
(
    id           serial,
    name         varchar(50),
    phone_number varchar(50),
    comment      text,
    number       integer
);
create table reservations
(
    id              serial
        primary key,
    guest_id        integer   not null,
    date_start_time timestamp not null,
    date_end_time   timestamp not null,
    hall            integer   not null,
    num_table       integer   not null,
    hours           integer default 3
);