using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LIB.Model;

namespace LIB
{
    public class IplModule : BaseService<ModuleEntity, int>, IModule
    {

        public IplModule() { }

        public List<ModuleTreeViewModel> ModuleGetListTreeView(string spaceTab = "====/")
        {
            var list = this.Raw_Query<ModuleTreeViewModel>(@"
                ;WITH temp(Id, Name, Parent, Sorting, SortingNew, iLevel, SortingOrder)
                as (
                        Select
			                Id				=	Id,
			                Name			=	Name,
			                Parent			=	isnull(Parent, 0),
			                Sorting			=	isnull(Sorting,0),
			                SortingNew		=	ROW_NUMBER() OVER(ORDER BY Sorting),

			                iLevel			=	1,
			                SortingOrder	=	convert(nvarchar, format(isnull(Sorting,0), 'd6'))
                        From Module
                        Where isnull(Parent, 0) = 0

                        Union All
                        Select
			                Id				=	temp1.Id,
			                Name			=	temp1.Name,
			                Parent			=	isnull(temp1.Parent, 0),
			                Sorting			=	isnull(temp1.Sorting,0),
			                SortingNew		=	ROW_NUMBER() OVER(ORDER BY temp1.Sorting),

			                iLevel			=	t.iLevel + 1,
			                SortingOrder	=	cast((t.SortingOrder + '/' + convert(nvarchar, format(isnull(temp1.Sorting,0), 'd6'))) as nvarchar)
                        From Module temp1
		                inner join temp t on t.Id = temp1.Parent
                )
                select
	                *,
	                NameTreeView	    =	case when temp.ParentNew <> 0 then REPLICATE(@spaceTab, temp.iLevel-1) else '' end  + temp.DisplayName,
                    Parent              =   ParentNew
                from(
	                Select 
                        Id              =   m.Id,
		                Name			=	m.Name,
		                DisplayName		=	m.DisplayName,
		                Parent			=	m.Parent,
		                Sorting			=	m.Sorting,
		                isShow			=	m.isShow,
		                iLevel			=	isnull(temp.iLevel, 0),
		                SortingNew		=	isnull(temp.SortingNew, 1),
		                ParentNew		=	case when temp.SortingNew is null then 0 when m.Parent is null then 0 else m.Parent end,
		                SortingOrder	=	temp.SortingOrder
	                From temp
	                right join Module m on m.Id = temp.Id
                ) temp
                order by SortingOrder
            ", new Dictionary<string, object>() {
                { "spaceTab", spaceTab}
            });
            return list.ToList();
        }

        public bool UpdateModuleIdAndSort(string xml)
        {
            var result = this.unitOfWork.Procedure<int>("sp_Module_UpdateParentIdAndSort", new
            {
                xml = xml
            });
            return true;
        }
        public List<ModuleEntity> GetDataModule()
        {
            DynamicParameters param = new DynamicParameters();
            return unitOfWork.Procedure<ModuleEntity>("sp_Module_GetData", param).ToList();
        }
        public List<ModuleEntity> GetListModuleByRoleId(int RoleId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("RoleId", RoleId);
                return unitOfWork.Procedure<ModuleEntity>("sp_Modules_GetByRoleId", param).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
    }
}
