--
-- ER/Studio 8.0 SQL Code Generation
-- Company :      Thang Long Software
-- Project :      VDMS-II.DM1
-- Author :       Quang Minh
--
-- Date Created : Saturday, June 27, 2009 16:31:42
-- Target DBMS : Oracle 9i
--

DROP TABLE v2_app_Role_In_Path CASCADE CONSTRAINTS
;
DROP TABLE v2_app_Role_In_Task CASCADE CONSTRAINTS
;
DROP TABLE v2_app_Site_Map CASCADE CONSTRAINTS
;
DROP TABLE v2_app_Task CASCADE CONSTRAINTS
;
DROP TABLE v2_app_User_Profile CASCADE CONSTRAINTS
;
DROP TABLE v2_data_File CASCADE CONSTRAINTS
;
DROP TABLE v2_data_Message CASCADE CONSTRAINTS
;
DROP TABLE v2_data_Message_Box CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Accessory CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Accessory_Type CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Category CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Contact CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Customer CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Cycle_Count_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Cycle_Count_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Dealer CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Favorite CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Inventory CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Inventory_Lock CASCADE CONSTRAINTS
;
DROP TABLE v2_p_N_G_Form_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_N_G_Form_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Order_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Order_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Part_Info CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Part_Safety CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Receive_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Receive_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Sales_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Sales_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_System_Data CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Transaction_History CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Transfer_Detail CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Transfer_Header CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Vendor CASCADE CONSTRAINTS
;
DROP TABLE v2_p_Warehouse CASCADE CONSTRAINTS
;
DROP SEQUENCE v2_seq_RoleInPath;
DROP SEQUENCE v2_seq_RoleInTask;
DROP SEQUENCE v2_seq_SiteMap;
DROP SEQUENCE v2_seq_Task;
DROP SEQUENCE v2_seq_File;
DROP SEQUENCE v2_seq_Message;
DROP SEQUENCE v2_seq_MessageBox;
DROP SEQUENCE v2_seq_Accessory;
DROP SEQUENCE v2_seq_Category;
DROP SEQUENCE v2_seq_Contact;
DROP SEQUENCE v2_seq_Customer;
DROP SEQUENCE v2_seq_CycleCountDetail;
DROP SEQUENCE v2_seq_CycleCountHeader;
DROP SEQUENCE v2_seq_Favorite;
DROP SEQUENCE v2_seq_Inventory;
DROP SEQUENCE v2_seq_InventoryLock;
DROP SEQUENCE v2_seq_NGFormDetail;
DROP SEQUENCE v2_seq_NGFormHeader;
DROP SEQUENCE v2_seq_OrderDetail;
DROP SEQUENCE v2_seq_OrderHeader;
DROP SEQUENCE v2_seq_PartInfo;
DROP SEQUENCE v2_seq_ReceiveDetail;
DROP SEQUENCE v2_seq_ReceiveHeader;
DROP SEQUENCE v2_seq_SalesDetail;
DROP SEQUENCE v2_seq_SalesHeader;
DROP SEQUENCE v2_seq_TransactionHistory;
DROP SEQUENCE v2_seq_TransferDetail;
DROP SEQUENCE v2_seq_TransferHeader;
DROP SEQUENCE v2_seq_Vendor;
DROP SEQUENCE v2_seq_Warehouse;
-- 
-- TABLE: v2_app_Role_In_Path 
--

CREATE TABLE v2_app_Role_In_Path(
    Role_Path_Id        NUMBER(10, 0)    NOT NULL,
    Role_Name           VARCHAR2(255)    NOT NULL,
    Application_Name    VARCHAR2(255)    NOT NULL,
    Path_Id             NUMBER(10, 0)    NOT NULL,
    CONSTRAINT v2_PK02 PRIMARY KEY (Role_Path_Id)
)
;



-- 
-- TABLE: v2_app_Role_In_Task 
--

CREATE TABLE v2_app_Role_In_Task(
    Role_Task_Id        NUMBER(38, 0)    NOT NULL,
    Task_Id             NUMBER(38, 0)    NOT NULL,
    Role_Name           VARCHAR2(255)    NOT NULL,
    Application_Name    VARCHAR2(255)    NOT NULL,
    CONSTRAINT v2_PK04 PRIMARY KEY (Role_Task_Id)
)
;



-- 
-- TABLE: v2_app_Site_Map 
--

CREATE TABLE v2_app_Site_Map(
    Path_Id    NUMBER(10, 0)     NOT NULL,
    URL        VARCHAR2(4000)    NOT NULL,
    CONSTRAINT v2_PK10 PRIMARY KEY (Path_Id)
)
;



Insert into v2_app_Site_Map (Path_Id, URL) values (1, '/Vehicle/Report/PrintOrder.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (2, '/Vehicle/Inventory/Orderform.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (3, '/Vehicle/Inventory/EditOrder.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (4, '/Vehicle/Inventory/Business.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (5, '/Vehicle/Inventory/ProcessOrder.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (6, '/Vehicle/Inventory/Check.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (7, '/Vehicle/Inventory/Import.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (8, '/Vehicle/Inventory/SaleReject.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (9, '/Vehicle/Inventory/Reject.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (10, '/Vehicle/Inventory/adjustment.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (11, '/Vehicle/Inventory/History.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (12, '/Vehicle/Inventory/Detail.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (13, '/Vehicle/Inventory/reformation.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (14, '/Vehicle/Inventory/Summary.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (15, '/Vehicle/Sale/Customer.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (16, '/Vehicle/Sale/Store.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (17, '/Vehicle/Sale/DataReformation.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (18, '/Vehicle/Sale/Datacheck.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (19, '/Vehicle/Sale/Money.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (20, '/Vehicle/Sale/Receipt.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (21, '/Vehicle/Report/ForAgency.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (22, '/Vehicle/Report/SaleDetail.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (23, '/Vehicle/Report/ReceiptDetail.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (24, '/Vehicle/Report/DailySaleReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (25, '/Service/WarrantyContent.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (26, '/Service/RepairHistory.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (27, '/Service/RepairList.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (28, '/Service/Report/PrintPartChange.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (29, '/Service/tempSRS.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (30, '/Service/ExchangeVoucher.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (31, '/Service/ExchangeReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (32, '/Service/LookupExchange.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (33, '/Service/confirmation.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (34, '/Service/verifyExchangeVoucher.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (35, '/Service/CustomerInfor.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (36, '/Vehicle/Report/ForBusiness.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (37, '/Service/WarrantyInfo.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (38, '/Part/Inventory/Order.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (39, '/Part/Inventory/Receive.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (40, '/Part/Inventory/NotGood.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (41, '/Part/Inventory/NotGoodApprove.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (42, '/Part/Inventory/UndoConfirm.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (43, '/Part/Inventory/InShort.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (44, '/Part/SavedSP.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (45, '/Part/Sale.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (46, '/Part/Inventory/SpecialIE.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (47, '/Part/Inventory/StockTransfer.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (48, '/Part/Inventory/BinCard.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (49, '/Part/Inventory/MonthlyClose.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (50, '/Admin/Part/CCA.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (51, '/Part/Report/InShortsupplier.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (52, '/Part/Report/ReceiptDetail.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (53, '/Part/Report/WarrantyReturn.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (54, '/Part/Report/ShippingSpan.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (55, '/Part/Report/PartOrderServiceSummary.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (56, '/Part/Report/saleReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (57, '/Part/Report/inventory.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (58, '/Part/Report/IOSReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (59, '/Part/Report/PartInputReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (60, '/Part/Report/SaleDetailReport.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (61, '/Admin/Database/Broken.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (62, '/Admin/Database/Branch.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (63, '/Admin/Database/WarrantyTime.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (64, '/Admin/Database/Agency.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (65, '/Admin/Database/ListMotorSale.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (66, '/Admin/Part/FindPart.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (67, '/Admin/Part/Part.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (68, '/Admin/Part/Category.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (69, '/Admin/Part/Customer.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (70, '/Admin/Database/Vendor.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (71, '/Admin/Security/ManageUsers.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (72, '/Admin/Security/Role.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (73, '/Admin/Security/Permission.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (74, '/Admin/Database/Dealer.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (75, '/Admin/Database/Article.aspx');
Insert into v2_app_Site_Map (Path_Id, URL) values (76, '/Admin/Security/Setting.aspx');
-- 
-- TABLE: v2_app_Task 
--

CREATE TABLE v2_app_Task(
    Task_Id         NUMBER(38, 0)    NOT NULL,
    Path_Id         NUMBER(10, 0)    NOT NULL,
    Task_Name       NVARCHAR2(50)    NOT NULL,
    Command_Name    VARCHAR2(30),
    CONSTRAINT v2_PK13 PRIMARY KEY (Task_Id)
)
;



-- 
-- TABLE: v2_app_User_Profile 
--

CREATE TABLE v2_app_User_Profile(
    User_Name         NVARCHAR2(256)    NOT NULL,
    Dealer_Code       VARCHAR2(30)      NOT NULL,
    Database_Code     CHAR(10),
    Area_Code         CHAR(25),
    Dept              CHAR(2),
    Position          VARCHAR2(1),
    N_G_Level         NUMBER(3, 0),
    Full_Name         NVARCHAR2(50),
    V_Warehouse_Id    NUMBER(10, 0),
    Warehouse_Id      NUMBER(10, 0),
    CONSTRAINT v2_PK29 PRIMARY KEY (User_Name, Dealer_Code)
)
;



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
-- TABLE: v2_p_Accessory 
--

CREATE TABLE v2_p_Accessory(
    Accessory_Id           NUMBER(10, 0)     NOT NULL,
    Accessory_Code         VARCHAR2(30)      NOT NULL,
    English_Name           NVARCHAR2(256),
    Vietnam_Name           NVARCHAR2(256),
    Dealer_Code            VARCHAR2(30)      NOT NULL,
    Accessory_Type_Code    CHAR(2)           NOT NULL,
    CONSTRAINT v2_PK32 PRIMARY KEY (Accessory_Id)
)
;



-- 
-- TABLE: v2_p_Accessory_Type 
--

CREATE TABLE v2_p_Accessory_Type(
    Accessory_Type_Code    CHAR(2)          NOT NULL,
    Accessory_Type_Name    NVARCHAR2(50)    NOT NULL,
    CONSTRAINT v2_PK33 PRIMARY KEY (Accessory_Type_Code)
)
;



Insert into v2_p_Accessory_Type (Accessory_Type_Code, Accessory_Type_Name) values ('SY', 'SYM');
Insert into v2_p_Accessory_Type (Accessory_Type_Code, Accessory_Type_Name) values ('HD', 'Honda');
Insert into v2_p_Accessory_Type (Accessory_Type_Code, Accessory_Type_Name) values ('YA', 'Yamaha');
Insert into v2_p_Accessory_Type (Accessory_Type_Code, Accessory_Type_Name) values ('CN', 'China');
Insert into v2_p_Accessory_Type (Accessory_Type_Code, Accessory_Type_Name) values ('OT', 'Other');
-- 
-- TABLE: v2_p_Category 
--

CREATE TABLE v2_p_Category(
    Category_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code    VARCHAR2(30)      NOT NULL,
    Code           CHAR(5)           NOT NULL,
    Name           NVARCHAR2(256)    NOT NULL,
    CONSTRAINT v2_PK03 PRIMARY KEY (Category_Id)
)
;



-- 
-- TABLE: v2_p_Contact 
--

CREATE TABLE v2_p_Contact(
    Contact_Id         NUMBER(10, 0)     NOT NULL,
    Full_Name          NVARCHAR2(50),
    Address            NVARCHAR2(255),
    Phone              VARCHAR2(20),
    Fax                VARCHAR2(20),
    Mobile             VARCHAR2(20),
    Email              VARCHAR2(255),
    Additional_Info    NVARCHAR2(255),
    CONSTRAINT v2_PK12 PRIMARY KEY (Contact_Id)
)
;



-- 
-- TABLE: v2_p_Customer 
--

CREATE TABLE v2_p_Customer(
    Customer_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code    VARCHAR2(30)      NOT NULL,
    Contact_Id     NUMBER(10, 0),
    Code           VARCHAR2(30)      NOT NULL,
    V_A_T_Code     VARCHAR2(30),
    Name           NVARCHAR2(255)    NOT NULL,
    CONSTRAINT v2_PK15 PRIMARY KEY (Customer_Id)
)
;



-- 
-- TABLE: v2_p_Cycle_Count_Detail 
--

CREATE TABLE v2_p_Cycle_Count_Detail(
    Cycle_Count_Detail_Id    NUMBER(10, 0)     NOT NULL,
    Cycle_Count_Header_Id    NUMBER(10, 0)     NOT NULL,
    Part_Code                VARCHAR2(30)      NOT NULL,
    Part_Type                CHAR(1)           NOT NULL,
    Quantity                 NUMBER(38, 0)     NOT NULL,
    Item_Comment             NVARCHAR2(256),
    CONSTRAINT PK166 PRIMARY KEY (Cycle_Count_Detail_Id)
)
;



-- 
-- TABLE: v2_p_Cycle_Count_Header 
--

CREATE TABLE v2_p_Cycle_Count_Header(
    Cycle_Count_Header_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code              VARCHAR2(30)      NOT NULL,
    Warehouse_Id             NUMBER(10, 0)     NOT NULL,
    Created_Time             TIMESTAMP(6)      NOT NULL,
    Created_By               NVARCHAR2(256)    NOT NULL,
    Last_Edited_Date         TIMESTAMP(6)      NOT NULL,
    Cycle_Date               TIMESTAMP(6)      NOT NULL,
    Status                   CHAR(1)           NOT NULL,
    Transaction_Comment      NVARCHAR2(256),
    CONSTRAINT PK165 PRIMARY KEY (Cycle_Count_Header_Id)
)
;



-- 
-- TABLE: v2_p_Dealer 
--

CREATE TABLE v2_p_Dealer(
    Dealer_Code               VARCHAR2(30)      NOT NULL,
    Parent_Code               VARCHAR2(30),
    Contact_Id                NUMBER(10, 0),
    Dealer_Name               NVARCHAR2(256)    NOT NULL,
    Dealer_Type               CHAR(2),
    Address                   NVARCHAR2(256),
    Database_Code             VARCHAR2(10)      NOT NULL,
    Area_Code                 VARCHAR2(25)      NOT NULL,
    Receive_Span              NUMBER(3, 0)      NOT NULL,
    Default_Warehouse_Id      NUMBER(38, 0)     NOT NULL,
    Default_V_Warehouse_Id    NUMBER(38, 0)     NOT NULL,
    Order_Date_Control        NUMBER(38, 0)     NOT NULL,
    CONSTRAINT v2_PK05 PRIMARY KEY (Dealer_Code)
)
;



INSERT INTO v2_p_Dealer (Dealer_Code, Parent_Code, Contact_Id, Dealer_Name, Database_Code, Area_Code, Receive_Span, Default_Warehouse_Id, Default_V_Warehouse_Id, Order_Date_Control)
VALUES ('/', null, null, 'VMEP', 'DNF', 'HCM', 0, 0, 0, 0);
-- 
-- TABLE: v2_p_Favorite 
--

CREATE TABLE v2_p_Favorite(
    Favorite_Id    NUMBER(10, 0)    NOT NULL,
    Part_Code      VARCHAR2(40)     NOT NULL,
    Dealer_Code    VARCHAR2(30)     NOT NULL,
    Part_Type      CHAR(1)          NOT NULL,
    Rank           NUMBER(5, 0)     NOT NULL,
    Type           CHAR(2)          NOT NULL,
    CONSTRAINT v2_PK07 PRIMARY KEY (Favorite_Id)
)
;



-- 
-- TABLE: v2_p_Inventory 
--

CREATE TABLE v2_p_Inventory(
    Inventory_Id    NUMBER(10, 0)    NOT NULL,
    Dealer_Code     VARCHAR2(30)     NOT NULL,
    Warehouse_Id    NUMBER(10, 0),
    Part_Info_Id    NUMBER(10, 0)    NOT NULL,
    Quantity        NUMBER(38, 0)    NOT NULL,
    Month           NUMBER(3, 0)     NOT NULL,
    Year            NUMBER(5, 0)     NOT NULL,
    CONSTRAINT v2_PK20 PRIMARY KEY (Inventory_Id)
)
;



-- 
-- TABLE: v2_p_Inventory_Lock 
--

CREATE TABLE v2_p_Inventory_Lock(
    Inventory_Lock_Id    NUMBER(10, 0)    NOT NULL,
    Month                NUMBER(38, 0)    NOT NULL,
    Year                 NUMBER(38, 0)    NOT NULL,
    Dealer_Code          VARCHAR2(30)     NOT NULL,
    Warehouse_Id         NUMBER(10, 0),
    CONSTRAINT PK159 PRIMARY KEY (Inventory_Lock_Id)
)
;



-- 
-- TABLE: v2_p_N_G_Form_Detail 
--

CREATE TABLE v2_p_N_G_Form_Detail(
    N_G_Form_Detail_Id        NUMBER(10, 0)     NOT NULL,
    N_G_Form_Header_Id        NUMBER(10, 0)     NOT NULL,
    Part_Code                 VARCHAR2(40)      NOT NULL,
    Part_Status               CHAR(1),
    Request_Quantity          NUMBER(38, 0)     NOT NULL,
    Approved_Quantity         NUMBER(38, 0)     NOT NULL,
    Problem_Again_Quantity    NUMBER(38, 0)     NOT NULL,
    Passed                    NUMBER(1, 0)      NOT NULL,
    Broken_Code               VARCHAR2(30),
    Dealer_Comment            NVARCHAR2(250)    NOT NULL,
    L1_Comment                NVARCHAR2(250),
    L2_Comment                NVARCHAR2(250),
    L3_Comment                NVARCHAR2(250),
    Transaction_Comment       NVARCHAR2(250),
    CONSTRAINT v2_PK25 PRIMARY KEY (N_G_Form_Detail_Id)
)
;



-- 
-- TABLE: v2_p_N_G_Form_Header 
--

CREATE TABLE v2_p_N_G_Form_Header(
    N_G_Form_Header_Id    NUMBER(10, 0)    NOT NULL,
    Dealer_Code           VARCHAR2(30)     NOT NULL,
    Receive_Header_Id     NUMBER(10, 0),
    Not_Good_Number       VARCHAR2(30),
    Reward_Number         VARCHAR2(30),
    Created_Date          TIMESTAMP(6)     NOT NULL,
    Approve_Date          TIMESTAMP(6),
    Approve_Level         NUMBER(3, 0)     NOT NULL,
    Status                CHAR(2)          NOT NULL,
    N_G_Type              CHAR(1)          NOT NULL,
    CONSTRAINT v2_PK26 PRIMARY KEY (N_G_Form_Header_Id)
)
;



-- 
-- TABLE: v2_p_Order_Detail 
--

CREATE TABLE v2_p_Order_Detail(
    Order_Detail_Id       NUMBER(10, 0)     NOT NULL,
    Order_Header_Id       NUMBER(10, 0)     NOT NULL,
    Line_Number           NUMBER(38, 0)     NOT NULL,
    Part_Code             VARCHAR2(40)      NOT NULL,
    Order_Quantity        NUMBER(38, 0)     NOT NULL,
    Quotation_Quantity    NUMBER(38, 0)     NOT NULL,
    Unit_Price            NUMBER(18, 0)     NOT NULL,
    Modify_Flag           CHAR(1)           NOT NULL,
    Status                CHAR(1),
    Note                  VARCHAR2(250),
    Part_Code_History     VARCHAR2(1000),
    CONSTRAINT v2_PK09 PRIMARY KEY (Order_Detail_Id)
)
;



-- 
-- TABLE: v2_p_Order_Header 
--

CREATE TABLE v2_p_Order_Header(
    Order_Header_Id                NUMBER(10, 0)     NOT NULL,
    Reference_Id                   NUMBER(10, 0),
    Created_Date                   TIMESTAMP(6)      NOT NULL,
    Created_By                     NVARCHAR2(256)    NOT NULL,
    Status                         CHAR(2)           NOT NULL,
    Order_Type                     CHAR(1)           NOT NULL,
    Order_Source                   CHAR(1)           NOT NULL,
    To_Dealer                      VARCHAR2(30)      NOT NULL,
    Dealer_Code                    VARCHAR2(30)      NOT NULL,
    To_Location                    NUMBER(10, 0)     NOT NULL,
    Order_Date                     TIMESTAMP(6)      NOT NULL,
    Confirm_Date                   TIMESTAMP(6),
    Quotation_Date                 TIMESTAMP(6),
    Payment_Date                   TIMESTAMP(6),
    Delivery_Date                  TIMESTAMP(6),
    Shipping_Date                  TIMESTAMP(6),
    Auto_In_Stock_Date             TIMESTAMP(6),
    Already_In_Stock               NUMBER(1, 0)      NOT NULL,
    Can_Undo_Auto_Receive          NUMBER(1, 0)      NOT NULL,
    Change_Remark                  CHAR(1)           NOT NULL,
    Tip_Top_Number                 VARCHAR2(10),
    Tip_Top_Processed              CHAR(1)           NOT NULL,
    Amount                         NUMBER(19, 0)     DEFAULT 0 NOT NULL,
    Sent_Warning_Over_Quotation    NUMBER(1, 0)      NOT NULL,
    Sent_Warning_Over_Shipping     NUMBER(1, 0),
    CONSTRAINT v2_PK08 PRIMARY KEY (Order_Header_Id)
)
;



-- 
-- TABLE: v2_p_Part_Info 
--

CREATE TABLE v2_p_Part_Info(
    Part_Info_Id    NUMBER(10, 0)    NOT NULL,
    Dealer_Code     VARCHAR2(30)     NOT NULL,
    Category_Id     NUMBER(10, 0),
    Part_Code       VARCHAR2(40)     NOT NULL,
    Accessory_Id    NUMBER(10, 0),
    Part_Type       CHAR(1)          NOT NULL,
    Price           NUMBER(19, 0),
    Deleted         NUMBER(1, 0),
    CONSTRAINT v2_PK14 PRIMARY KEY (Part_Info_Id)
)
;



-- 
-- TABLE: v2_p_Part_Safety 
--

CREATE TABLE v2_p_Part_Safety(
    Part_Info_Id       NUMBER(10, 0)    NOT NULL,
    Warehouse_Id       NUMBER(10, 0)    NOT NULL,
    Safety_Quantity    NUMBER(38, 0)    NOT NULL,
    Current_Stock      NUMBER(38, 0)    NOT NULL,
    Deleted            NUMBER(1, 0)     NOT NULL,
    CONSTRAINT v2_PK27 PRIMARY KEY (Part_Info_Id, Warehouse_Id)
)
;



-- 
-- TABLE: v2_p_Receive_Detail 
--

CREATE TABLE v2_p_Receive_Detail(
    Receive_Detail_Id    NUMBER(10, 0)     NOT NULL,
    Receive_Header_Id    NUMBER(10, 0)     NOT NULL,
    Order_Header_Id      NUMBER(10, 0)     NOT NULL,
    Part_Code            VARCHAR2(40)      NOT NULL,
    Shipping_Quantity    NUMBER(38, 0)     NOT NULL,
    Good_Quantity        NUMBER(38, 0)     NOT NULL,
    Broken_Quantity      NUMBER(38, 0)     NOT NULL,
    Wrong_Quantity       NUMBER(38, 0)     NOT NULL,
    Lack_Quantity        NUMBER(38, 0)     NOT NULL,
    Status               CHAR(1),
    Dealer_Comment       NVARCHAR2(250),
    CONSTRAINT v2_PK24 PRIMARY KEY (Receive_Detail_Id)
)
;



-- 
-- TABLE: v2_p_Receive_Header 
--

CREATE TABLE v2_p_Receive_Header(
    Receive_Header_Id    NUMBER(10, 0)    NOT NULL,
    Order_Header_Id      NUMBER(10, 0)    NOT NULL,
    Dealer_Code          VARCHAR2(30)     NOT NULL,
    Warehouse_Id         NUMBER(10, 0)    NOT NULL,
    Issue_Number         VARCHAR2(30)     NOT NULL,
    Receive_Date         TIMESTAMP(6)     NOT NULL,
    Is_Locked            NUMBER(1, 0)     NOT NULL,
    Is_Automatic         NUMBER(1, 0)     NOT NULL,
    Has_Undo             NUMBER(1, 0)     NOT NULL,
    Has_N_G_Form         NUMBER(1, 0)     NOT NULL,
    Not_Good_Number      VARCHAR2(30),
    CONSTRAINT v2_PK23 PRIMARY KEY (Receive_Header_Id)
)
;



-- 
-- TABLE: v2_p_Sales_Detail 
--

CREATE TABLE v2_p_Sales_Detail(
    Sales_Detail_Id     NUMBER(10, 0)     NOT NULL,
    Sales_Header_Id     NUMBER(10, 0)     NOT NULL,
    Part_Info_Id        NUMBER(10, 0)     NOT NULL,
    Part_Code           VARCHAR2(40),
    Part_Name           NVARCHAR2(256)    NOT NULL,
    Part_Type           CHAR(1)           NOT NULL,
    Order_Quantity      NUMBER(38, 0)     NOT NULL,
    Unit_Price          NUMBER(38, 0)     NOT NULL,
    Percent_Discount    NUMBER(38, 0)     NOT NULL,
    Line_Total          NUMBER(38, 0)     NOT NULL,
    Modified_Date       TIMESTAMP(6)      NOT NULL,
    CONSTRAINT v2_PK17 PRIMARY KEY (Sales_Detail_Id)
)
;



-- 
-- TABLE: v2_p_Sales_Header 
--

CREATE TABLE v2_p_Sales_Header(
    Sales_Header_Id          NUMBER(10, 0)     NOT NULL,
    Dealer_Code              VARCHAR2(30)      NOT NULL,
    Customer_Id              NUMBER(10, 0),
    Warehouse_Id             NUMBER(10, 0)     NOT NULL,
    Order_Date               TIMESTAMP(6)      NOT NULL,
    Sales_Date               TIMESTAMP(6)      NOT NULL,
    Status                   CHAR(2)           NOT NULL,
    Sales_Order_Number       VARCHAR2(50)      NOT NULL,
    Manual_Voucher_Number    VARCHAR2(50),
    Customer_Name            NVARCHAR2(50),
    Sales_Person             NVARCHAR2(255)    NOT NULL,
    Sub_Total                NUMBER(19, 0)     NOT NULL,
    Tax_Amount               NUMBER(19, 0)     NOT NULL,
    Discount                 NUMBER(19, 0)     NOT NULL,
    Modified_Date            TIMESTAMP(6)      NOT NULL,
    Sales_Comment            NVARCHAR2(256),
    CONSTRAINT v2_PK18 PRIMARY KEY (Sales_Header_Id)
)
;



-- 
-- TABLE: v2_p_System_Data 
--

CREATE TABLE v2_p_System_Data(
    Code    CHAR(2)          NOT NULL,
    Type    CHAR(2)          NOT NULL,
    Term    NVARCHAR2(50)    NOT NULL,
    CONSTRAINT v2_PK22 PRIMARY KEY (Code)
)
;



Insert into v2_p_System_Data (Code, Type, Term) values ('OP', 'OS', 'Order Open');
Insert into v2_p_System_Data (Code, Type, Term) values ('SN', 'OS', 'Order Sent');
Insert into v2_p_System_Data (Code, Type, Term) values ('CF', 'OS', 'Order Confirmed');
Insert into v2_p_System_Data (Code, Type, Term) values ('NC', 'OS', 'Order Closed (normal)');
Insert into v2_p_System_Data (Code, Type, Term) values ('RO', 'OS', 'Order Re-Open');
Insert into v2_p_System_Data (Code, Type, Term) values ('AC', 'OS', 'Order Closed (abnormal)');
Insert into v2_p_System_Data (Code, Type, Term) values ('VD', 'OS', 'Order Void');
Insert into v2_p_System_Data (Code, Type, Term) values ('SE', 'IA', 'Special Export');
Insert into v2_p_System_Data (Code, Type, Term) values ('SI', 'IA', 'Special Import');
Insert into v2_p_System_Data (Code, Type, Term) values ('NI', 'IA', 'Normal Import');
Insert into v2_p_System_Data (Code, Type, Term) values ('CC', 'IA', 'Cycle count');
Insert into v2_p_System_Data (Code, Type, Term) values ('RE', 'OS', 'Order Rejected');
Insert into v2_p_System_Data (Code, Type, Term) values ('SL', 'TS', 'Sale Transaction');
Insert into v2_p_System_Data (Code, Type, Term) values ('ST', 'IA', 'Stock transfer');
Insert into v2_p_System_Data (Code, Type, Term) values ('AI', 'IA', 'Auto Import');
Insert into v2_p_System_Data (Code, Type, Term) values ('UI', 'IA', 'Undo Auto Import');
-- 
-- TABLE: v2_p_Transaction_History 
--

CREATE TABLE v2_p_Transaction_History(
    Transaction_History_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code               VARCHAR2(30)      NOT NULL,
    Invoice_Number            VARCHAR2(50),
    Transaction_Date          TIMESTAMP(6)      NOT NULL,
    Transaction_Code          CHAR(2)           NOT NULL,
    Vendor_Id                 NUMBER(10, 0),
    Quantity                  NUMBER(38, 0)     NOT NULL,
    Actual_Cost               NUMBER(19, 0)     NOT NULL,
    Created_By                NVARCHAR2(256)    NOT NULL,
    Created_Date              TIMESTAMP(6)      NOT NULL,
    Transaction_Comment       NVARCHAR2(250),
    Part_Info_Id              NUMBER(10, 0)     NOT NULL,
    Warehouse_Id              NUMBER(10, 0)     NOT NULL,
    Secondary_Warehouse_Id    NUMBER(10, 0),
    CONSTRAINT v2_PK19 PRIMARY KEY (Transaction_History_Id)
)
;



-- 
-- TABLE: v2_p_Transfer_Detail 
--

CREATE TABLE v2_p_Transfer_Detail(
    Transfer_Detail_Id    NUMBER(10, 0)     NOT NULL,
    Transfer_Header_Id    NUMBER(10, 0)     NOT NULL,
    Part_Info_Id          NUMBER(10, 0)     NOT NULL,
    Part_Code             VARCHAR2(40),
    Quantity              NUMBER(38, 0)     NOT NULL,
    Part_Comment          NVARCHAR2(256),
    CONSTRAINT PK156 PRIMARY KEY (Transfer_Detail_Id)
)
;



-- 
-- TABLE: v2_p_Transfer_Header 
--

CREATE TABLE v2_p_Transfer_Header(
    Transfer_Header_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code           VARCHAR2(30)      NOT NULL,
    Created_Date          TIMESTAMP(6)      NOT NULL,
    Created_By            NVARCHAR2(256)    NOT NULL,
    Transfer_Date         TIMESTAMP(6),
    From_Warehouse_Id     NUMBER(10, 0)     NOT NULL,
    To_Warehouse_Id       NUMBER(10, 0)     NOT NULL,
    Transfer_Comment      NVARCHAR2(256),
    Status                CHAR(2)           NOT NULL,
    CONSTRAINT PK155 PRIMARY KEY (Transfer_Header_Id)
)
;



-- 
-- TABLE: v2_p_Vendor 
--

CREATE TABLE v2_p_Vendor(
    Vendor_Id      NUMBER(10, 0)     NOT NULL,
    Dealer_Code    VARCHAR2(30)      NOT NULL,
    Contact_Id     NUMBER(10, 0),
    Code           CHAR(30)          NOT NULL,
    Name           NVARCHAR2(250)    NOT NULL,
    CONSTRAINT v2_PK11 PRIMARY KEY (Vendor_Id)
)
;



-- 
-- TABLE: v2_p_Warehouse 
--

CREATE TABLE v2_p_Warehouse(
    Warehouse_Id    NUMBER(10, 0)     NOT NULL,
    Dealer_Code     VARCHAR2(30)      NOT NULL,
    Code            VARCHAR2(30)      NOT NULL,
    Address         NVARCHAR2(255)    NOT NULL,
    Type            CHAR(1)           NOT NULL,
    CONSTRAINT v2_PK06 PRIMARY KEY (Warehouse_Id)
)
;



-- 
-- TABLE: v2_app_Role_In_Path 
--

ALTER TABLE v2_app_Role_In_Path ADD CONSTRAINT Refv2_app_Site_Map249 
    FOREIGN KEY (Path_Id)
    REFERENCES v2_app_Site_Map(Path_Id)
;


-- 
-- TABLE: v2_app_Role_In_Task 
--

ALTER TABLE v2_app_Role_In_Task ADD CONSTRAINT Refv2_app_Task250 
    FOREIGN KEY (Task_Id)
    REFERENCES v2_app_Task(Task_Id)
;


-- 
-- TABLE: v2_app_Task 
--

ALTER TABLE v2_app_Task ADD CONSTRAINT Refv2_app_Site_Map243 
    FOREIGN KEY (Path_Id)
    REFERENCES v2_app_Site_Map(Path_Id)
;


-- 
-- TABLE: v2_app_User_Profile 
--

ALTER TABLE v2_app_User_Profile ADD CONSTRAINT Refv2_p_Dealer201 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_app_User_Profile ADD CONSTRAINT Refv2_p_Warehouse272 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_app_User_Profile ADD CONSTRAINT Refv2_p_Warehouse314 
    FOREIGN KEY (V_Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;


-- 
-- TABLE: v2_data_File 
--

ALTER TABLE v2_data_File ADD CONSTRAINT Refv2_data_Message326 
    FOREIGN KEY (Message_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TABLE: v2_data_Message 
--

ALTER TABLE v2_data_Message ADD CONSTRAINT Refv2_data_Message327 
    FOREIGN KEY (Parent_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TABLE: v2_data_Message_Box 
--

ALTER TABLE v2_data_Message_Box ADD CONSTRAINT Refv2_data_Message328 
    FOREIGN KEY (Message_Id)
    REFERENCES v2_data_Message(Message_Id)
;


-- 
-- TABLE: v2_p_Accessory 
--

ALTER TABLE v2_p_Accessory ADD CONSTRAINT Refv2_p_Dealer226 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Accessory ADD CONSTRAINT Refv2_p_Accessory_Type227 
    FOREIGN KEY (Accessory_Type_Code)
    REFERENCES v2_p_Accessory_Type(Accessory_Type_Code)
;


-- 
-- TABLE: v2_p_Category 
--

ALTER TABLE v2_p_Category ADD CONSTRAINT Refv2_p_Dealer37 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Customer 
--

ALTER TABLE v2_p_Customer ADD CONSTRAINT Refv2_p_Contact60 
    FOREIGN KEY (Contact_Id)
    REFERENCES v2_p_Contact(Contact_Id)
;

ALTER TABLE v2_p_Customer ADD CONSTRAINT Refv2_p_Dealer105 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Cycle_Count_Detail 
--

ALTER TABLE v2_p_Cycle_Count_Detail ADD CONSTRAINT Refv2_p_Cycle_Count_Header318 
    FOREIGN KEY (Cycle_Count_Header_Id)
    REFERENCES v2_p_Cycle_Count_Header(Cycle_Count_Header_Id)
;


-- 
-- TABLE: v2_p_Cycle_Count_Header 
--

ALTER TABLE v2_p_Cycle_Count_Header ADD CONSTRAINT Refv2_p_Warehouse319 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Cycle_Count_Header ADD CONSTRAINT Refv2_p_Dealer320 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Dealer 
--

ALTER TABLE v2_p_Dealer ADD CONSTRAINT Refv2_p_Contact40 
    FOREIGN KEY (Contact_Id)
    REFERENCES v2_p_Contact(Contact_Id)
;

ALTER TABLE v2_p_Dealer ADD CONSTRAINT Refv2_p_Dealer41 
    FOREIGN KEY (Parent_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Favorite 
--

ALTER TABLE v2_p_Favorite ADD CONSTRAINT Refv2_p_Dealer191 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Inventory 
--

ALTER TABLE v2_p_Inventory ADD CONSTRAINT Refv2_p_Dealer273 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Inventory ADD CONSTRAINT Refv2_p_Part_Info274 
    FOREIGN KEY (Part_Info_Id)
    REFERENCES v2_p_Part_Info(Part_Info_Id)
;

ALTER TABLE v2_p_Inventory ADD CONSTRAINT Refv2_p_Warehouse275 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;


-- 
-- TABLE: v2_p_Inventory_Lock 
--

ALTER TABLE v2_p_Inventory_Lock ADD CONSTRAINT Refv2_p_Warehouse295 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Inventory_Lock ADD CONSTRAINT Refv2_p_Dealer296 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_N_G_Form_Detail 
--

ALTER TABLE v2_p_N_G_Form_Detail ADD CONSTRAINT Refv2_p_N_G_Form_Header130 
    FOREIGN KEY (N_G_Form_Header_Id)
    REFERENCES v2_p_N_G_Form_Header(N_G_Form_Header_Id)
;


-- 
-- TABLE: v2_p_N_G_Form_Header 
--

ALTER TABLE v2_p_N_G_Form_Header ADD CONSTRAINT Refv2_p_Dealer131 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_N_G_Form_Header ADD CONSTRAINT Refv2_p_Receive_Header168 
    FOREIGN KEY (Receive_Header_Id)
    REFERENCES v2_p_Receive_Header(Receive_Header_Id)
;


-- 
-- TABLE: v2_p_Order_Detail 
--

ALTER TABLE v2_p_Order_Detail ADD CONSTRAINT Refv2_p_Order_Header148 
    FOREIGN KEY (Order_Header_Id)
    REFERENCES v2_p_Order_Header(Order_Header_Id)
;


-- 
-- TABLE: v2_p_Order_Header 
--

ALTER TABLE v2_p_Order_Header ADD CONSTRAINT Refv2_p_Dealer149 
    FOREIGN KEY (To_Dealer)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Order_Header ADD CONSTRAINT Refv2_p_Order_Header150 
    FOREIGN KEY (Reference_Id)
    REFERENCES v2_p_Order_Header(Order_Header_Id)
;

ALTER TABLE v2_p_Order_Header ADD CONSTRAINT Refv2_p_Warehouse184 
    FOREIGN KEY (To_Location)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Order_Header ADD CONSTRAINT Refv2_p_Dealer185 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Order_Header ADD CONSTRAINT Refv2_p_System_Data215 
    FOREIGN KEY (Status)
    REFERENCES v2_p_System_Data(Code)
;


-- 
-- TABLE: v2_p_Part_Info 
--

ALTER TABLE v2_p_Part_Info ADD CONSTRAINT Refv2_p_Category169 
    FOREIGN KEY (Category_Id)
    REFERENCES v2_p_Category(Category_Id)
;

ALTER TABLE v2_p_Part_Info ADD CONSTRAINT Refv2_p_Dealer171 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Part_Info ADD CONSTRAINT Refv2_p_Accessory224 
    FOREIGN KEY (Accessory_Id)
    REFERENCES v2_p_Accessory(Accessory_Id)
;


-- 
-- TABLE: v2_p_Part_Safety 
--

ALTER TABLE v2_p_Part_Safety ADD CONSTRAINT Refv2_p_Warehouse197 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Part_Safety ADD CONSTRAINT Refv2_p_Part_Info231 
    FOREIGN KEY (Part_Info_Id)
    REFERENCES v2_p_Part_Info(Part_Info_Id)
;


-- 
-- TABLE: v2_p_Receive_Detail 
--

ALTER TABLE v2_p_Receive_Detail ADD CONSTRAINT Refv2_p_Receive_Header152 
    FOREIGN KEY (Receive_Header_Id)
    REFERENCES v2_p_Receive_Header(Receive_Header_Id)
;

ALTER TABLE v2_p_Receive_Detail ADD CONSTRAINT Refv2_p_Order_Header208 
    FOREIGN KEY (Order_Header_Id)
    REFERENCES v2_p_Order_Header(Order_Header_Id)
;


-- 
-- TABLE: v2_p_Receive_Header 
--

ALTER TABLE v2_p_Receive_Header ADD CONSTRAINT Refv2_p_Dealer153 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Receive_Header ADD CONSTRAINT Refv2_p_Order_Header154 
    FOREIGN KEY (Order_Header_Id)
    REFERENCES v2_p_Order_Header(Order_Header_Id)
;

ALTER TABLE v2_p_Receive_Header ADD CONSTRAINT Refv2_p_Warehouse225 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;


-- 
-- TABLE: v2_p_Sales_Detail 
--

ALTER TABLE v2_p_Sales_Detail ADD CONSTRAINT Refv2_p_Sales_Header156 
    FOREIGN KEY (Sales_Header_Id)
    REFERENCES v2_p_Sales_Header(Sales_Header_Id)
;

ALTER TABLE v2_p_Sales_Detail ADD CONSTRAINT Refv2_p_Part_Info247 
    FOREIGN KEY (Part_Info_Id)
    REFERENCES v2_p_Part_Info(Part_Info_Id)
;


-- 
-- TABLE: v2_p_Sales_Header 
--

ALTER TABLE v2_p_Sales_Header ADD CONSTRAINT Refv2_p_Dealer157 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Sales_Header ADD CONSTRAINT Refv2_p_Customer158 
    FOREIGN KEY (Customer_Id)
    REFERENCES v2_p_Customer(Customer_Id)
;

ALTER TABLE v2_p_Sales_Header ADD CONSTRAINT Refv2_p_Warehouse281 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;


-- 
-- TABLE: v2_p_Transaction_History 
--

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_Vendor187 
    FOREIGN KEY (Vendor_Id)
    REFERENCES v2_p_Vendor(Vendor_Id)
;

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_Dealer188 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_System_Data216 
    FOREIGN KEY (Transaction_Code)
    REFERENCES v2_p_System_Data(Code)
;

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_Part_Info248 
    FOREIGN KEY (Part_Info_Id)
    REFERENCES v2_p_Part_Info(Part_Info_Id)
;

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_Warehouse278 
    FOREIGN KEY (Secondary_Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Transaction_History ADD CONSTRAINT Refv2_p_Warehouse279 
    FOREIGN KEY (Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;


-- 
-- TABLE: v2_p_Transfer_Detail 
--

ALTER TABLE v2_p_Transfer_Detail ADD CONSTRAINT Refv2_p_Part_Info288 
    FOREIGN KEY (Part_Info_Id)
    REFERENCES v2_p_Part_Info(Part_Info_Id)
;

ALTER TABLE v2_p_Transfer_Detail ADD CONSTRAINT Refv2_p_Transfer_Header289 
    FOREIGN KEY (Transfer_Header_Id)
    REFERENCES v2_p_Transfer_Header(Transfer_Header_Id)
;


-- 
-- TABLE: v2_p_Transfer_Header 
--

ALTER TABLE v2_p_Transfer_Header ADD CONSTRAINT Refv2_p_Warehouse290 
    FOREIGN KEY (From_Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Transfer_Header ADD CONSTRAINT Refv2_p_Warehouse291 
    FOREIGN KEY (To_Warehouse_Id)
    REFERENCES v2_p_Warehouse(Warehouse_Id)
;

ALTER TABLE v2_p_Transfer_Header ADD CONSTRAINT Refv2_p_Dealer292 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Vendor 
--

ALTER TABLE v2_p_Vendor ADD CONSTRAINT Refv2_p_Contact44 
    FOREIGN KEY (Contact_Id)
    REFERENCES v2_p_Contact(Contact_Id)
;

ALTER TABLE v2_p_Vendor ADD CONSTRAINT Refv2_p_Dealer45 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TABLE: v2_p_Warehouse 
--

ALTER TABLE v2_p_Warehouse ADD CONSTRAINT Refv2_p_Dealer46 
    FOREIGN KEY (Dealer_Code)
    REFERENCES v2_p_Dealer(Dealer_Code)
;


-- 
-- TRIGGER: v2_app_Role_In_Path 
--

CREATE SEQUENCE v2_seq_RoleInPath MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_app_Role_In_Path_U_PK BEFORE INSERT ON v2_app_Role_In_Path
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_RoleInPath.NEXTVAL INTO :NEW.Role_Path_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_app_Role_In_Task_U_PK 
--

CREATE SEQUENCE v2_seq_RoleInTask MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_app_Role_In_Task_U_PK BEFORE INSERT ON v2_app_Role_In_Task
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_RoleInTask.NEXTVAL INTO :NEW.Role_Task_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_app_Site_Map_U_PK 
--

CREATE SEQUENCE v2_seq_SiteMap MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_app_Site_Map_U_PK BEFORE INSERT ON v2_app_Site_Map
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_SiteMap.NEXTVAL INTO :NEW.Path_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_app_Task_U_PK 
--

CREATE SEQUENCE v2_seq_Task MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_app_Task_U_PK BEFORE INSERT ON v2_app_Task
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Task.NEXTVAL INTO :NEW.Task_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_data_File_U_PK 
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
-- TRIGGER: v2_data_Message_U_PK 
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
-- TRIGGER: v2_data_Message_Box_U_PK 
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

-- 
-- TRIGGER: v2_p_Accessory_U_PK 
--

CREATE SEQUENCE v2_seq_Accessory MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Accessory_U_PK BEFORE INSERT ON v2_p_Accessory
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Accessory.NEXTVAL INTO :NEW.Accessory_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Category_U_PK 
--

CREATE SEQUENCE v2_seq_Category MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Category_U_PK BEFORE INSERT ON v2_p_Category
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Category.NEXTVAL INTO :NEW.Category_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Contact_U_PK 
--

CREATE SEQUENCE v2_seq_Contact MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Contact_U_PK BEFORE INSERT ON v2_p_Contact
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Contact.NEXTVAL INTO :NEW.Contact_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Customer_U_PK 
--

CREATE SEQUENCE v2_seq_Customer MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Customer_U_PK BEFORE INSERT ON v2_p_Customer
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Customer.NEXTVAL INTO :NEW.Customer_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Cycle_Count_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_CycleCountDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Cycle_Count_Detail_U_PK BEFORE INSERT ON v2_p_Cycle_Count_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_CycleCountDetail.NEXTVAL INTO :NEW.Cycle_Count_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Cycle_Count_Header_P_UK 
--

CREATE SEQUENCE v2_seq_CycleCountHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Cycle_Count_Header_U_PK BEFORE INSERT ON v2_p_Cycle_Count_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_CycleCountHeader.NEXTVAL INTO :NEW.Cycle_Count_Header_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Favorite_U_PK 
--

CREATE SEQUENCE v2_seq_Favorite MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Favorite_U_PK BEFORE INSERT ON v2_p_Favorite
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Favorite.NEXTVAL INTO :NEW.Favorite_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Inventory_U_PK 
--

CREATE SEQUENCE v2_seq_Inventory MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Inventory_U_PK BEFORE INSERT ON v2_p_Inventory
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Inventory.NEXTVAL INTO :NEW.Inventory_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Inventory_Lock_U_PK 
--

CREATE SEQUENCE v2_seq_InventoryLock MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Inventory_Lock_U_PK BEFORE INSERT ON v2_p_Inventory_Lock
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_InventoryLock.NEXTVAL INTO :NEW.Inventory_Lock_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_N_G_Form_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_NGFormDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_N_G_Form_Detail_U_PK BEFORE INSERT ON v2_p_N_G_Form_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_NGFormDetail.NEXTVAL INTO :NEW.N_G_Form_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_N_G_Form_Header_U_PK 
--

CREATE SEQUENCE v2_seq_NGFormHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_N_G_Form_Header_U_PK BEFORE INSERT ON v2_p_N_G_Form_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_NGFormHeader.NEXTVAL INTO :NEW.N_G_Form_Header_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Order_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_OrderDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Order_Detail_U_PK BEFORE INSERT ON v2_p_Order_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_OrderDetail.NEXTVAL INTO :NEW.Order_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Order_Header_U_PK 
--

CREATE SEQUENCE v2_seq_OrderHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Order_Header_U_PK BEFORE INSERT ON v2_p_Order_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
IF :NEW.ORDER_SOURCE = 'V' THEN
SELECT v2_seq_OrderHeader.NEXTVAL INTO :NEW.Order_Header_Id FROM DUAL;
END IF;
END;
/

-- 
-- TRIGGER: v2_p_Part_Info_U_PK 
--

CREATE SEQUENCE v2_seq_PartInfo MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Part_Info_U_PK BEFORE INSERT ON v2_p_Part_Info
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_PartInfo.NEXTVAL INTO :NEW.Part_Info_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Receive_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_ReceiveDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Receive_Detail_U_PK BEFORE INSERT ON v2_p_Receive_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_ReceiveDetail.NEXTVAL INTO :NEW.Receive_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Receive_Header_U_PK 
--

CREATE SEQUENCE v2_seq_ReceiveHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Receive_Header_U_PK BEFORE INSERT ON v2_p_Receive_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_ReceiveHeader.NEXTVAL INTO :NEW.Receive_Header_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Sales_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_SalesDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Sales_Detail_U_PK BEFORE INSERT ON v2_p_Sales_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_SalesDetail.NEXTVAL INTO :NEW.Sales_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Sales_Header_U_PK 
--

CREATE SEQUENCE v2_seq_SalesHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Sales_Header_U_PK BEFORE INSERT ON v2_p_Sales_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_SalesHeader.NEXTVAL INTO :NEW.Sales_Header_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Transaction_History_U_PK 
--

CREATE SEQUENCE v2_seq_TransactionHistory MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Transaction_History_U_PK BEFORE INSERT ON v2_p_Transaction_History
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_TransactionHistory.NEXTVAL INTO :NEW.Transaction_History_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Transfer_Detail_U_PK 
--

CREATE SEQUENCE v2_seq_TransferDetail MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Transfer_Detail_U_PK BEFORE INSERT ON v2_p_Transfer_Detail
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_TransferDetail.NEXTVAL INTO :NEW.Transfer_Detail_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Transfer_Header_U_PK 
--

CREATE SEQUENCE v2_seq_TransferHeader MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Transfer_Header_U_PK BEFORE INSERT ON v2_p_Transfer_Header
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_TransferHeader.NEXTVAL INTO :NEW.Transfer_Header_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Vendor_U_PK 
--

CREATE SEQUENCE v2_seq_Vendor MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Vendor_U_PK BEFORE INSERT ON v2_p_Vendor
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Vendor.NEXTVAL INTO :NEW.Vendor_Id FROM DUAL;
END;
/

-- 
-- TRIGGER: v2_p_Warehouse_U_PK 
--

CREATE SEQUENCE v2_seq_Warehouse MINVALUE 1 MAXVALUE 
999999999999999999999999999 START WITH 1 INCREMENT BY 1 CACHE 20;

CREATE OR REPLACE TRIGGER v2_p_Warehouse_U_PK BEFORE INSERT ON v2_p_Warehouse
REFERENCING OLD AS OLD NEW AS NEW
FOR EACH ROW
BEGIN
SELECT v2_seq_Warehouse.NEXTVAL INTO :NEW.Warehouse_Id FROM DUAL;
END;
/

