using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models {

    public interface IRepository {

        IEnumerable<Product> Products { get; }

        void AddProduct(Product product);
    }
}
