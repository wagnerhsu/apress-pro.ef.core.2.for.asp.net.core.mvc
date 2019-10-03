using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models {

    public interface IRepository {

        IQueryable<Product> Products { get; }

        void AddProduct(Product product);
    }
}
