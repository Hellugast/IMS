using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Reports
{
    public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
    {
        private readonly IProductTransactionRepository _ProductTransactionRepository;

        public SearchProductTransactionsUseCase(IProductTransactionRepository ProductTransactionRepository)
        {
            _ProductTransactionRepository = ProductTransactionRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(string ProductName,
            DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? ProductTransactionType)
        {

            if (dateTo.HasValue)
                dateTo = dateTo.Value.AddDays(1);

            return await _ProductTransactionRepository.GetProductTransactionsAsync(ProductName, dateFrom, dateTo, ProductTransactionType);

        }
    }
}
