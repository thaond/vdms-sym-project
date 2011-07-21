--
-- ER/Studio 8.0 SQL Code Generation
-- Company :      Thang Long Software
-- Project :      VDMS-II.DM1
-- Author :       Quang Minh
--
-- Date Created : Thursday, February 19, 2009 10:14:51
-- Target DBMS : Oracle 9i
--

DROP TABLE v2_data_File CASCADE CONSTRAINTS
;
DROP TABLE v2_data_Message CASCADE CONSTRAINTS
;
DROP TABLE v2_data_Message_Box CASCADE CONSTRAINTS
;
DROP SEQUENCE v2_seq_File;
DROP SEQUENCE v2_seq_Message;
DROP SEQUENCE v2_seq_MessageBox;
-- 
-- TABLE: v2_data_File 
--

CREATE TABLE v2_data_File(
    File_Id       NUMBER(10, 0)     NOT NULL,
    Message_Id    NUMBER(10, 0)     NOT NULL,
    File_Name     NVARCHAR2(128)    NOT NULL,
    Body          LONG RAW          NOT NULL,
    CONSTRAINT v2_PK28 PRIMARY KEY (File_Id)
)
;



-- 
-- TABLE: v2_data_Message 
--

CREATE TABLE v2_data_Message(
    Message_Id      NUMBER(10, 0)     NOT NULL,
    Parent_Id       NUMBER(10, 0),
    Body            NVARCHAR2(512)    NOT NULL,
    Created_Date    TIMESTAMP(6)      NOT NULL,
    Created_By      NVARCHAR2(256)    NOT NULL,
    Flag            CHAR(1)           NOT NULL,
    CONSTRAINT v2_PK01 PRIMARY KEY (Message_Id)
)
;



-- 
-- TABLE: v2_data_Message_Box 
--

CREATE TABLE v2_data_Message_Box(
    Message_Box_Id    NUMBER(10, 0)    NOT NULL,
    Message_Id        NUMBER(10, 0)    NOT NULL,
    From_User         VARCHAR2(30)     NOT NULL,
    To_User           VARCHAR2(30)     NOT NULL,
    Flag              CHAR(1)          NOT NULL,
    Position          CHAR(1)          NOT NULL,
    CONSTRAINT v2_PK31 PRIMARY KEY (Message_Box_Id)
)
;



-- 
-- TABLE: v2_data_File 
--

ALTER TABLE v2_data_File ADD CONSTRAINT Refv2_data_Message189 
    FOREIGN KEY (Message_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TABLE: v2_data_Message 
--

ALTER TABLE v2_data_Message ADD CONSTRAINT Refv2_data_Message180 
    FOREIGN KEY (Parent_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TABLE: v2_data_Message_Box 
--

ALTER TABLE v2_data_Message_Box ADD CONSTRAINT Refv2_data_Message212 
    FOREIGN KEY (Message_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TRIGGER: t2 
--

CREATE SEQUENCE v2_seq_File MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_data_File_U_PK BEFORE INSERT ON v2_data_File
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_File.NEXTVAL INTO :NEW.File_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: t1 
--

CREATE SEQUENCE v2_seq_Message MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_data_Message_U_PK BEFORE INSERT ON v2_data_Message
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Message.NEXTVAL INTO :NEW.Message_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: t3 
--

CREATE SEQUENCE v2_seq_MessageBox MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_data_Message_Box_U_PK BEFORE INSERT ON v2_data_Message_Box
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_MessageBox.NEXTVAL INTO :NEW.Message_Box_Id FROM DUAL;
END;
/

