using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Images
{
    public interface IImages: IBaseServices<ImagesEntity, long>
    {
        bool InsertOrUpdate(ImagesEntity images);
    }
}
