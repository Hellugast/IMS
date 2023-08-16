using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Products
{
    public class ViewProductByNameUseCase : IViewProductByNameUseCase
    {
        private readonly IProductRepository _productRepository;

        public ViewProductByNameUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
        {
            return await _productRepository.GetProductsByNameAsync(name);
        }
    }
}
