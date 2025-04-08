
CREATE DATABASE FamilyTreeDB;
USE FamilyTreeDB;

-- יצירת טבלת Person
CREATE TABLE Person (
    Person_Id INT PRIMARY KEY,
    Personal_Name NVARCHAR(50),
    Family_Name NVARCHAR(50),
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- מגדר זכר / נקבה
    Father_Id INT NULL,
    Mother_Id INT NULL,
    Spouse_Id INT NULL,
    FOREIGN KEY (Father_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Mother_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Spouse_Id) REFERENCES Person(Person_Id)
);

-- יצירת טבלת ConnectionType
CREATE TABLE ConnectionType (
    Type_Id INT PRIMARY KEY,
    Type_Name NVARCHAR(20) NOT NULL
);

-- יצירת טבלת FamilyTree
CREATE TABLE FamilyTree (
    Person_Id INT NOT NULL,
    Relative_Id INT NOT NULL,
    Connection_Type INT NOT NULL,
    PRIMARY KEY (Person_Id, Relative_Id, Connection_Type),
    FOREIGN KEY (Person_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Relative_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Connection_Type) REFERENCES ConnectionType(Type_Id)
);

-- מחיקת כל הנתונים הקודמים
DELETE FROM FamilyTree;
DELETE FROM ConnectionType;
DELETE FROM Person;


INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(1, N'מירי', N'כהן', 'F', NULL, NULL, 2),  -- מירי: אמא, בת זוג של רפאל (2)
(2, N'רפאל', N'כהן', 'M', NULL, NULL, 1), -- רפאל: אב, בן זוג של מירי (1)
(3, N'יעקב', N'כהן', 'M', 2, 3, 4), -- יעקב: בן, בת זוג של נעמי (4)
(4, N'נעמי', N'כהן', 'F', 2, 3, 3), -- נעמי: בת זוג של יעקב (3)
(5, N'רות', N'כהן', 'F', 2, 3, 6), -- רות: אחות, בת זוג של יונתן (6)
(6, N'יונתן', N'כהן', 'M', NULL, NULL, 5), -- יונתן: בן זוג של רות (5)
(7, N'נועה', N'כהן', 'F', 2, 3, NULL), -- נועה: אחות (ללא בן זוג)
(8, N'עדי', N'כהן', 'F', 2, 3, 9), -- עדי: אחות, בת זוג של אבי (9)
(9, N'אבי', N'כהן', 'M', NULL, NULL, 8), -- אבי: בן זוג של עדי (8)
(10, N'יאיר', N'כהן', 'M', 2, 3, NULL), -- יאיר: אח (ללא בת זוג)
(11, N'תהילה', N'כהן', 'F', 2, 3, 12), -- תהילה: אחות, בת זוג של יוסי (12)
(12, N'יוסי', N'כהן', 'M', NULL, NULL, 11); -- יוסי: בן זוג של תהילה (11)


INSERT INTO ConnectionType (Type_Id, Type_Name)
VALUES 
(1, N'אב'),        -- אב
(2, N'אם'),        -- אם
(3, N'אח'),        -- אח
(4, N'אחות'),      -- אחות
(5, N'בן'),        -- בן
(6, N'בת'),        -- בת
(7, N'בן זוג'),    -- בן זוג
(8, N'בת זוג');    -- בת זוג


-- הכנסת קשרים לטבלת FamilyTree (עץ משפחה)
INSERT INTO FamilyTree (Person_Id, Relative_Id, Connection_Type)
VALUES
(1, 2, 7),  -- מירי => בן זוג => רפאל
(2, 1, 8),  -- רפאל => בת זוג => מירי
(3, 2, 5),  -- יעקב => בן => רפאל
(3, 1, 5),  -- יעקב => בן => מירי
(4, 3, 8),  -- נעמי => בת זוג => יעקב
(5, 3, 6),  -- רות => אחות => יעקב
(6, 5, 7),  -- יונתן => בן זוג => רות
(7, 5, 6),  -- נועה => אחות => רות
(8, 3, 6),  -- עדי => אחות => יעקב
(9, 8, 7),  -- אבי => בן זוג => עדי
(10, 3, 5), -- יאיר => אח => יעקב
(11, 3, 4), -- תהילה => אחות => יעקב
(12, 3, 7); -- יוסי => בן זוג => תהילה

SELECT * FROM Person;

SELECT * FROM ConnectionType;

SELECT * FROM FamilyTree;


----תרגיל 2 

CREATE TRIGGER trg_MatchSpouses
ON Person
AFTER INSERT, UPDATE
AS
BEGIN
    -- נבדוק האם הוכנס או עודכן בן זוג (Spouse_Id)
    UPDATE p2
    SET Spouse_Id = i.Person_Id
    FROM Person p2
    INNER JOIN inserted i ON p2.Person_Id = i.Spouse_Id
    WHERE (p2.Spouse_Id IS NULL OR p2.Spouse_Id <> i.Person_Id)
      AND i.Spouse_Id IS NOT NULL;
END;

---צפייה בטריגרים שיש לי
SELECT * FROM sys.triggers;

--הכנסת זוג חדש לשימוש בטריגר

INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(222, N'מירב', N'כהן', 'F', NULL, NULL, NULL);

INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(111, N'דני', N'כהן', 'M', 2, 1, 222);


SELECT * FROM Person WHERE Person_Id IN (111, 222);


