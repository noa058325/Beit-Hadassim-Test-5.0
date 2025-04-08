
CREATE DATABASE FamilyTreeDB;
USE FamilyTreeDB;

-- ����� ���� Person
CREATE TABLE Person (
    Person_Id INT PRIMARY KEY,
    Personal_Name NVARCHAR(50),
    Family_Name NVARCHAR(50),
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- ���� ��� / ����
    Father_Id INT NULL,
    Mother_Id INT NULL,
    Spouse_Id INT NULL,
    FOREIGN KEY (Father_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Mother_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Spouse_Id) REFERENCES Person(Person_Id)
);

-- ����� ���� ConnectionType
CREATE TABLE ConnectionType (
    Type_Id INT PRIMARY KEY,
    Type_Name NVARCHAR(20) NOT NULL
);

-- ����� ���� FamilyTree
CREATE TABLE FamilyTree (
    Person_Id INT NOT NULL,
    Relative_Id INT NOT NULL,
    Connection_Type INT NOT NULL,
    PRIMARY KEY (Person_Id, Relative_Id, Connection_Type),
    FOREIGN KEY (Person_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Relative_Id) REFERENCES Person(Person_Id),
    FOREIGN KEY (Connection_Type) REFERENCES ConnectionType(Type_Id)
);

-- ����� �� ������� �������
DELETE FROM FamilyTree;
DELETE FROM ConnectionType;
DELETE FROM Person;


INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(1, N'����', N'���', 'F', NULL, NULL, 2),  -- ����: ���, �� ��� �� ���� (2)
(2, N'����', N'���', 'M', NULL, NULL, 1), -- ����: ��, �� ��� �� ���� (1)
(3, N'����', N'���', 'M', 2, 3, 4), -- ����: ��, �� ��� �� ���� (4)
(4, N'����', N'���', 'F', 2, 3, 3), -- ����: �� ��� �� ���� (3)
(5, N'���', N'���', 'F', 2, 3, 6), -- ���: ����, �� ��� �� ����� (6)
(6, N'�����', N'���', 'M', NULL, NULL, 5), -- �����: �� ��� �� ��� (5)
(7, N'����', N'���', 'F', 2, 3, NULL), -- ����: ���� (��� �� ���)
(8, N'���', N'���', 'F', 2, 3, 9), -- ���: ����, �� ��� �� ��� (9)
(9, N'���', N'���', 'M', NULL, NULL, 8), -- ���: �� ��� �� ��� (8)
(10, N'����', N'���', 'M', 2, 3, NULL), -- ����: �� (��� �� ���)
(11, N'�����', N'���', 'F', 2, 3, 12), -- �����: ����, �� ��� �� ���� (12)
(12, N'����', N'���', 'M', NULL, NULL, 11); -- ����: �� ��� �� ����� (11)


INSERT INTO ConnectionType (Type_Id, Type_Name)
VALUES 
(1, N'��'),        -- ��
(2, N'��'),        -- ��
(3, N'��'),        -- ��
(4, N'����'),      -- ����
(5, N'��'),        -- ��
(6, N'��'),        -- ��
(7, N'�� ���'),    -- �� ���
(8, N'�� ���');    -- �� ���


-- ����� ����� ����� FamilyTree (�� �����)
INSERT INTO FamilyTree (Person_Id, Relative_Id, Connection_Type)
VALUES
(1, 2, 7),  -- ���� => �� ��� => ����
(2, 1, 8),  -- ���� => �� ��� => ����
(3, 2, 5),  -- ���� => �� => ����
(3, 1, 5),  -- ���� => �� => ����
(4, 3, 8),  -- ���� => �� ��� => ����
(5, 3, 6),  -- ��� => ���� => ����
(6, 5, 7),  -- ����� => �� ��� => ���
(7, 5, 6),  -- ���� => ���� => ���
(8, 3, 6),  -- ��� => ���� => ����
(9, 8, 7),  -- ��� => �� ��� => ���
(10, 3, 5), -- ���� => �� => ����
(11, 3, 4), -- ����� => ���� => ����
(12, 3, 7); -- ���� => �� ��� => �����

SELECT * FROM Person;

SELECT * FROM ConnectionType;

SELECT * FROM FamilyTree;


----����� 2 

CREATE TRIGGER trg_MatchSpouses
ON Person
AFTER INSERT, UPDATE
AS
BEGIN
    -- ����� ��� ����� �� ����� �� ��� (Spouse_Id)
    UPDATE p2
    SET Spouse_Id = i.Person_Id
    FROM Person p2
    INNER JOIN inserted i ON p2.Person_Id = i.Spouse_Id
    WHERE (p2.Spouse_Id IS NULL OR p2.Spouse_Id <> i.Person_Id)
      AND i.Spouse_Id IS NOT NULL;
END;

---����� �������� ��� ��
SELECT * FROM sys.triggers;

--����� ��� ��� ������ ������

INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(222, N'����', N'���', 'F', NULL, NULL, NULL);

INSERT INTO Person (Person_Id, Personal_Name, Family_Name, Gender, Father_Id, Mother_Id, Spouse_Id)
VALUES 
(111, N'���', N'���', 'M', 2, 1, 222);


SELECT * FROM Person WHERE Person_Id IN (111, 222);


