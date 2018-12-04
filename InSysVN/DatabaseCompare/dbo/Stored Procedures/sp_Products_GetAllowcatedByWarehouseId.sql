CREATE proc [dbo].[sp_Products_GetAllowcatedByWarehouseId]
@WarehouseId bigint
as
select	Id
		,Allowcated
		,WarehouseId
from Products 
where WarehouseId = @WarehouseId