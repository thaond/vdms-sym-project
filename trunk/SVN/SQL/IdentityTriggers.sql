create or replace trigger SYM_CUSTOMER_U_PK
before insert on SYM_CUSTOMER
for each row
begin
if :new.CUSTOMERID is null 
then
select SEQ_CUSTOMER.NEXTVAL into :new.CUSTOMERID from dual;
end if;
end;
/
create or replace trigger DATA_ITEMINSTANCE_U_PK
before insert on DATA_ITEMINSTANCE
for each row
begin
if :new.ITEMINSTANCEID is null 
then
select SEQ_ITEMINSTANCE.NEXTVAL into :new.ITEMINSTANCEID from dual;
end if;
end;
/
create or replace trigger SALE_INVENTORY_U_PK
before insert on SALE_INVENTORY
for each row
begin
if :new.INVENTORYID is null 
then
select SEQ_INVENTORY.NEXTVAL into :new.INVENTORYID from dual;
end if;
end;
/
create or replace trigger SALE_INVENTORYDAY_U_PK
before insert on SALE_INVENTORYDAY
for each row
begin
if :new.INVENTORYDAYID is null 
then
select SEQ_INVENTORYDAY.NEXTVAL into :new.INVENTORYDAYID from dual;
end if;
end;
/
create or replace trigger SALE_INVOICE_U_PK
before insert on SALE_INVOICE
for each row
begin
if :new.INVOICEID is null 
then
select SEQ_INVOICE.NEXTVAL into :new.INVOICEID from dual;
end if;
end;
/
create or replace trigger SALE_PAYMENT_U_PK
before insert on SALE_PAYMENT
for each row
begin
if :new.PAYMENTID is null 
then
select SEQ_PAYMENT.NEXTVAL into :new.PAYMENTID from dual;
end if;
end;
/
create or replace trigger SALE_SELLITEM_U_PK
before insert on SALE_SELLITEM
for each row
begin
if :new.SELLITEMID is null 
then
select SEQ_SELLITEM.NEXTVAL into :new.SELLITEMID from dual;
end if;
end;
/
create or replace trigger SALE_TRANSHIS_U_PK
before insert on SALE_TRANSHIS
for each row
begin
if :new.TRANSACTIONID is null 
then
select SEQ_TRANSHIS.NEXTVAL into :new.TRANSACTIONID from dual;
end if;
end;
/
create or replace trigger VDMS.SALE_ORDER_PAYMENT_PK
before insert on VDMS.SALE_ORDER_PAYMENT
for each row
begin
if :NEW.ORDER_PAYMENT_Id is null 
then
select VDMS.SEQ_SALE_ORDER_PAYMENT.NEXTVAL into :NEW.ORDER_PAYMENT_Id from dual;
end if;
end;
/