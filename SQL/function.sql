CREATE OR REPLACE TYPE t_record as object (
  id       NUMBER,
  key       NUMBER,
  value   VARCHAR2(16)
);
create or replace type t_table as table of t_record;

CREATE OR REPLACE FUNCTION GetTestData ( id IN NUMBER ) return t_table PIPELINED
IS cursor result IS SELECT
    "id",
	"key",
	"value"
FROM
	yinyang."test"
WHERE
	"id" = id;
BEGIN
		FOR ROW IN result
		loop
		PIPE ROW ( t_record ( ROW."id", ROW."key", ROW."value" ) );

END loop;
return;

END;
