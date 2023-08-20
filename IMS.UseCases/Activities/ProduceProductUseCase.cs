using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {

        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IProductRepository _productRepository;

        public ProduceProductUseCase(IProductTransactionRepository productTransactionRepository, IProductRepository productRepository)
        {
            _productTransactionRepository = productTransactionRepository;
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            await _productTransactionRepository.ProduceAsync(productionNumber, product, quantity, doneBy);

            product.Quantity += quantity;

            await _productRepository.EditProductAsync(product);
        }
    }
}
