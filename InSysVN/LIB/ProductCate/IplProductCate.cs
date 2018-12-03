using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ProductCate
{
    public class IplProductCate : BaseService<ProductCateEntity, long>, IProductCate
    {
        public ProductCateEntity Insert(ProductCateEntity entity,  ref string message)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", entity.ProductId);
                param.Add("@CateId", entity.CateId);
              
                return unitOfWork.Procedure<ProductCateEntity>("sp_ProductCate_Insert", param).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public List<ProductCateEntity> GetCateProuctId(long ProductId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", ProductId);
                List<ProductCateEntity> data = unitOfWork.Procedure<ProductCateEntity>("sp_ProductCate_GetByProduct", param).ToList();
                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
    }
    public interface IProductCate : IBaseServices<ProductCateEntity, long>
    {
        ProductCateEntity Insert(ProductCateEntity entity, ref string message);
        List<ProductCateEntity> GetCateProuctId(long ProductId);
    }
}
