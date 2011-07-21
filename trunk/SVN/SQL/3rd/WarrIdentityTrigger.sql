create or replace trigger DATA_WARRANTYCONDITION_U_PK
before insert on DATA_WARRANTYCONDITION
for each row
begin
if :new.WARRANTYCONDITIONID is null 
then
select SEQ_WARRANTYCONDITION.NEXTVAL into :new.WARRANTYCONDITIONID from dual;
end if;
end;