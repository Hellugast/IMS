﻿using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelsValidations
{
    public class ProduceEnsureEnoughInventoryQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;
            if (produceViewModel != null)
            {
                if (produceViewModel.Product != null && produceViewModel.Product.ProductInventories != null)
                {
                    foreach (var pi in produceViewModel.Product.ProductInventories)
                    {
                        if (pi.Inventory != null &&
                            (pi.InventoryQuantity * produceViewModel.QuantityToProduce) > pi.Inventory.Quantity)
                        {
                            return new ValidationResult($"Inventory {pi.Inventory.InventoryName} is not enough to produce " +
                                $"{produceViewModel.QuantityToProduce} products", new[] { validationContext.MemberName });

                        }
                        //pi.Inventory.Quantity -= (pi.InventoryQuantity * produceViewModel.QuantityToProduce);
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
