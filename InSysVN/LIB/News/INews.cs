using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public interface INews : IBaseServices<NewsEntity, long>
    {
        List<NewsEntity> GetAllNews();
        NewsEntity GetNewsByID(int Id);
        bool CheckByTitle(string title);
        NewsEntity Insert_Update(NewsEntity newsentity);
        bool UpdateNewsImage(string base64Image, long NewsId, string PathServer, string PathFile);
        bool UpdateIsActive(int newsId);
        bool DeleteByNewsId(int id);
    }
}
