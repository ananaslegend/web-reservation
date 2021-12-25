using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebReservation.Data.Repository
{
    public interface IRepository <TEntity> where TEntity: class
    {
        // IEnumerable<TEntity> All();
        // int Add(TEntity entity);
        // void Delete(int Id);
        // TEntity FindById(int Id);
    }
}