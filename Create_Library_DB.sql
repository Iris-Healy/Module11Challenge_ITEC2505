IF NOT EXISTS (
    SELECT name
        FROM sys.databases
        WHERE name = N'Library'
)
CREATE DATABASE Library;
GO

USE Library;

--Drop Tables From Database if they exist
DROP TABLE IF EXISTS Book_Genres;
DROP TABLE IF EXISTS Book_Authors;
DROP TABLE IF EXISTS Books;
DROP TABLE IF EXISTS Publishers;
DROP TABLE IF EXISTS Genres;
DROP TABLE IF EXISTS Authors;

--Create Authors Table
CREATE TABLE Authors
(
    Author_ID INT IDENTITY PRIMARY KEY NOT NULL,
    A_Last_Name NVARCHAR(256) NOT NULL,
    A_First_Name NVARCHAR(256) NOT NULL
);

--Create Genres Table
CREATE TABLE Genres
(
    Genre_ID INT IDENTITY PRIMARY KEY NOT NULL,
    Genre NVARCHAR(256) NOT NULL
);

--Create Publishers Table
CREATE TABLE Publishers 
(
    Publisher_ID INT IDENTITY PRIMARY KEY NOT NULL,
    P_Name NVARCHAR(256) NOT NULL
);

--Create Books table with Publishers FK
CREATE TABLE Books
(
    Book_ID INT IDENTITY PRIMARY KEY NOT NULL,
    BK_Title NVARCHAR(256) NOT NULL,
    ISBN NVARCHAR(256) NOT NULL,
    BK_Publication_Year CHAR(4),
    Publisher_ID INT NOT NULL,
    CONSTRAINT Books_FK_Publishers FOREIGN KEY (Publisher_ID)
    REFERENCES Publishers (Publisher_ID)
);

--Create Many-Many Relationship Table Book_Authors
CREATE TABLE Book_Authors
(
    BA_ID INT IDENTITY PRIMARY KEY NOT NULL,
    Book_ID INT NOT NULL,
    Author_ID INT NOT NULL,
    CONSTRAINT Book_Authors_FK_Books FOREIGN KEY (Book_ID)
    REFERENCES Books (Book_ID),
    CONSTRAINT Book_Authors_FK_Authors FOREIGN KEY (Author_ID)
    REFERENCES Authors (Author_ID)
);

--Create Many-Many Relationship Table Book_Genres
CREATE TABLE Book_Genres
(
    BG_ID INT IDENTITY PRIMARY KEY NOT NULL,
    Book_ID INT NOT NULL,
    Genre_ID INT NOT NULL,
    CONSTRAINT Book_Genres_FK_Books FOREIGN KEY (Book_ID)
    REFERENCES Books (Book_ID),
    CONSTRAINT Book_Genres_FK_Genres FOREIGN KEY (Genre_ID)
    REFERENCES Genres (Genre_ID)
);

--Populate Authors Table
INSERT INTO Authors (A_Last_Name, A_First_Name)
VALUES ('Corey', 'James S.A'),
       ('Chambers', 'Becky'),
       ('Leguin', 'Ursala K.');

--Populate Genres Taale
INSERT INTO Genres (Genre)
VALUES ('Horror'),
       ('Sci-Fi'),
       ('Space Opera'),
       ('LGBT'),
       ('Adult'),
       ('Political'),
       ('Fantasy'),
       ('Fiction');

--Populate Publishers
INSERT INTO Publishers (P_Name)
VALUES ('Orbit'),
       ('Create Space'),
       ('Parnassus Press'),
       ('Bantam Books'),
       ('Puffin Books'),
       ('Gateway');

--Populate Books
INSERT INTO Books (BK_Title, ISBN, BK_Publication_Year, Publisher_ID)
VALUES ('Leviathan Wakes', '9780316129084', '2011', 1),
       ('Caliban''s War', '9780316129060', '2012', 1),
       ('Abaddon''s Gate', '9780316129077', '2013', 1),
       ('Cibola Burn', '9780316217620', '2014', 1),
       ('Nemesis Games', '9780316334716', '2016', 1),
       ('Babylon''s Ashes', '9780316217644', '2017', 1),
       ('Persepolis Rising', '9780316332859', '2019', 1),
       ('Tiamat''s Wrath' ,'9780356510361', '2020', 1),
       ('Leviathan Falls', '9780316332941', '2024', 1),
       ('The Long Way to a Small Angry Planet', ' 9780062444134' , '2016', 2),
       ('A Closed and Common Orbit', '9780062569400', '2017', 2),
       ('Record of a Spaceborn Few', '9780062699220',  '2018', 2),
       ('The Galaxy and the Ground Within', '9780062936042', '2021', 2),
       ('The Wizard of Earthsea', '9780547773742', '1968', 3),
       ('The Tombs of Atuan', '9781442459915', '1975', 4),
       ('The Farthest Shore', '9780140306941', '1976', 5),
       ('The Dispossessed', '9781857988826', '1999', 6),
       ('The Left Hand of Darkness', '9780441007318', '2000', 6);

--Populate Book_Authors
INSERT INTO Book_Authors (Book_ID, Author_ID)
VALUES (1, 1),
       (2, 1),
       (3, 1),
       (4, 1),
       (5, 1),
       (6, 1),
       (7, 1),
       (8, 1),
       (9, 1),
       (10, 2),
       (11, 2),
       (12, 2),
       (13, 2),
       (14, 3),
       (15, 3),
       (16, 3),
       (17, 3),
       (18, 3);

--Populate Book_genres
INSERT INTO Book_Genres (Book_ID, Genre_ID)
VALUES (1, 1), (1, 2), (1, 5), (1, 8),
       (2, 2), (2, 3), (2, 5), (2, 8),
       (3, 2), (3, 3), (3, 5), (3, 8),
       (4, 2), (4, 3), (4, 5), (4, 8),
       (5, 2), (5, 3), (5, 5), (5, 8),
       (6, 2), (6, 3), (6, 5), (6, 8),
       (7, 2), (7, 3), (7, 5), (7, 8),
       (8, 2), (8, 3), (8, 5), (8, 8),
       (9, 2), (9, 3), (9, 5), (9, 8),
       (10, 2), (10, 4), (10, 7), (10, 8),
       (11, 2), (11, 4), (11, 7), (11, 8),
       (12, 2), (12, 4), (12, 7), (12, 8),
       (13, 2), (13, 4), (13, 7), (13, 8),
       (14, 7), (14, 8),
       (15, 7), (15, 8),
       (16, 7), (16, 8),
       (17, 2), (17, 5), (17, 6), (17, 8),
       (18, 2), (18, 5), (18, 6), (18, 8);



