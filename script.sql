
CREATE DATABASE santaclauscrm; 
create table "Elves" (
    "Id" serial primary key,
    "NickName" text not null,
    "Speed" int not null
);

insert into "Elves" ("NickName", "Speed") values ('Ciuffo', 100);
insert into "Elves" ("NickName", "Speed") values ('Giuffo', 120);
insert into "Elves" ("NickName", "Speed") values ('Puffo', 80);
insert into "Elves" ("NickName", "Speed") values ('Ugo', 90);

create table "Gifts" (
    "Id" serial primary key,
    "Product" text not null
);

create table "Operations" (
    "Id" serial primary key,
    "Name" text not null
);

insert into "Operations" ("Name") values ('Costruzione');
insert into "Operations" ("Name") values ('Impacchettamento');
insert into "Operations" ("Name") values ('MessaInSlitta');


create table "GiftOperations" (
    "Id" serial primary key,
    "GiftId" int not null references "Gifts" ("Id"),
    "OperationId" int not null references "Operations" ("Id"),
    "ElfId" int not null references "Elves" ("Id"),
    "UncleChristmasId" int REFERENCES "UnclesChristmas" ("Id")
);



CREATE TABLE "UnclesChristmas" (
    "Id" serial PRIMARY KEY,
    "Name" text not null,
    "CarriedGifts" int DEFAULT 0 
);
INSERT into "UncleChristmas" ("Id","Name") VALUES (-1,'none');

// USE IN CASE OF THE CREATOR IS NOT THE CORRECT {user}
{ 
alter table "Operations" owner to {user};
alter table "GiftOperations" owner to {user};
alter table "UnclesChristmas" owner to {user};
alter table "Gifts" owner to {user};
}
