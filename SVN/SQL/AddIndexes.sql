CREATE INDEX VDMS.INDEX_DATA_ITEMINSTANCE1 ON VDMS.DATA_ITEMINSTANCE (ITEMCODE)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_ORDERDETAIL1 ON VDMS.SALE_ORDERDETAIL (ITEMCODE)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_INVENTORYDAY1 ON VDMS.SALE_INVENTORYDAY (ITEMCODE)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_INVENTORY1 ON VDMS.SALE_INVENTORY (ITEMCODE)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_SHIPPINGDETAIL1 ON VDMS.SALE_SHIPPINGDETAIL (ITEMCODE)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_ORDERDETAIL2 ON VDMS.SALE_ORDERDETAIL (ORDERID)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);

CREATE INDEX VDMS.INDEX_SALE_ORDER_PAYMENT1 ON VDMS.SALE_ORDER_PAYMENT (ORDER_HEADER_ID)
TABLESPACE SYSTEM
STORAGE (
  INITIAL 64K
  MAXEXTENTS UNLIMITED
);