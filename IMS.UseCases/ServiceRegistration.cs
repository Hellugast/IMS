using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.Activities;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Reports.Interfaces;
using IMS.UseCases.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public static class ServiceRegistration
    {
        public static void AddUseCasesServices(this IServiceCollection services)
        {
            services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
            services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
            services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
            services.AddTransient<IViewInventoriesByIdUseCase, ViewInventoriesByIdUseCase>();
            services.AddTransient<IViewProductByNameUseCase, ViewProductByNameUseCase>();
            services.AddTransient<IAddProductUseCase, AddProductUseCase>();
            services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
            services.AddTransient<IEditProductUseCase, EditProductUseCase>();
            services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
            services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
            services.AddTransient<ISellProductUseCase, SellProductUseCase>();
            services.AddTransient<ISearchInventoryTransactionsUseCase, SearchInventoryTransactionsUseCase>();
            services.AddTransient<ISearchProductTransactionsUseCase, SearchProductTransactionsUseCase>();
        }
    }
}
