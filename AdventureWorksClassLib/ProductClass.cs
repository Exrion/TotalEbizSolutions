using AdventureWorksClassLib.Services;
using AdventureWorksClassLib.Models;
using AdventureWorksClassLib.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace AdventureWorksClassLib
{
    public class ProductClass
    {
        #region DB Context
        private readonly AppDbContext _dbCtx;

        public ProductClass(AppDbContext dbCtx)
        {
            _dbCtx = dbCtx;
        }
        #endregion

        #region Main
        public List<Product> getAllProducts(int range, int iterationSize)
        {
            //Page indicates which iteration of the set of 100 entries are to be retrieved
            List<Product> data = _dbCtx.Product
                .Skip(range)
                .Take(iterationSize)
                .ToList();

            return data;
        }
        public List<Product> getAllProducts()
        {
            //Page indicates which iteration of the set of 100 entries are to be retrieved
            List<Product> data = _dbCtx.Product
                .ToList();

            return data;
        }
        public int addProduct(ProductDTO product)
        {
            DbSet<Product> dbs = _dbCtx.Product;

            //Assign from DTO
            Product newProd = convProduct(product);

            //Add and save product to dbset
            dbs.Add(newProd);

            switch (_dbCtx.SaveChanges())
            {
                case 1:
                    return 1;
                case 0:
                    return 0;
            }
            return 0;
        }
        public int updateProduct(int id, ProductDTO newProduct)
        {
            DbSet<Product> dbs = _dbCtx.Product;

            Product? oldProduct = dbs
                .Where(p => p.ProductId == newProduct.ProductId)
                .FirstOrDefault();

            //Check if product is found
            if (oldProduct != null)
            {
                //Assign from DTO
                Product newProd = convProduct(newProduct);

                //Update product
                dbs.Update(newProd);

                //Save changes, return results
                switch (_dbCtx.SaveChanges())
                {
                    case 1:
                        return 1;
                    case 0:
                        return 0;
                }
            }
            return 0;
        }
        public int deleteProduct(int id)
        {
            DbSet<Product> dbs = _dbCtx.Product;

            //Find product
            Product? product = dbs
                .Where(p => p.ProductId == id)
                .FirstOrDefault();

            //Check if product is found
            if (product != null)
            {
                dbs.Remove(product);
            }
            else
            {
                return 0;
            }

            //Save changes, return results
            switch (_dbCtx.SaveChanges())
            {
                case 1:
                    return 1;
                case 0:
                    return 0;
            }
            return 0;
        }
        private Product convProduct(ProductDTO product)
        {
            var newProd = new Product()
            {
                Name = product.Name,
                ProductNumber = product.ProductNumber,
                MakeFlag = product.MakeFlag,
                FinishedGoodsFlag = product.FinishedGoodsFlag,
                SafetyStockLevel = product.SafetyStockLevel,
                ReorderPoint = product.ReorderPoint,
                StandardCost = product.StandardCost,
                ListPrice = product.ListPrice,
                SizeUnitMeasureCode = product.SizeUnitMeasureCode,
                WeightUnitMeasureCode = product.WeightUnitMeasureCode,
                DaysToManufacture = product.DaysToManufacture,
                ProductSubcategoryId = product.ProductSubcategoryId,
                ProductModelId = product.ProductModelId,
                SellStartDate = product.SellStartDate,
                Rowguid = product.Rowguid,
                ModifiedDate = product.ModifiedDate
            };

            return newProd;
        }
        #endregion
    }
}