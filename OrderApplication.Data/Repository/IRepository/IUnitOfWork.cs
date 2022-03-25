using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        //IDiposable, iunitofworkun işlemi bittiği zaman kendisi otomatik olarak silinmesini sağlar.
        //Tüm Repositoryler burada toplanır, toplu olarak save changes() yapılır.

        IAppUserRepository AppUser { get; }
        ICartRepository Cart { get; }
        ICategoryRepository Category { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderProductRepository OrderProduct { get; }
        IProductRepository Product { get; }

        void Save();





    }
}
