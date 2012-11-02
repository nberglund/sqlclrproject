select o1.name as table_name, c1.name as column_name

, case d.referenced_minor_id when 0 then o2.name end as referenced_fn

, case d.referenced_minor_id when 0 then null else o2.name end as
referenced_tbl

, c2.name as referenced_column

from sys.sql_dependencies as d

join sys.objects as o1

on o1.object_id = d.object_id

join sys.columns as c1

on c1.object_id = d.object_id

and c1.column_id = d.column_id

join sys.objects as o2

on o2.object_id = d.referenced_major_id

left join sys.columns as c2

on c2.object_id = d.referenced_major_id

and c2.column_id = d.referenced_minor_id


select * from sys.assembly_modules