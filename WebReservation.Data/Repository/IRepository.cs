using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebReservation.Data.Repository
{
    public interface IRepository <TEntity> where TEntity: class
    {
        //IEnumerable<TEntity> All();
        // void Add(TEntity entity);
        // void Delete(TEntity entity);
        // void Update(TEntity entity);
        // TEntity FindById(int Id);
    }
}