using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AdventureWorksClassLib;
using AdventureWorksClassLib.Models;
using AdventureWorksClassLib.ViewModels;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace AdventureWorksAPI.Controllers
{
    [Authorize]
    [Route("api/products")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        #region Context
        private readonly ProductClass prodClass;

        public ProductApiController(ProductClass prodClass)
        {
            this.prodClass = prodClass;
        }
        #endregion

        #region Main

        #region GET Products
        [HttpGet]
        [Route("list/{page}")]
        public IActionResult getAll(int page)
        {
            //Sets iteration size 
            int iterationSize = 50;

            //Calculate range 
            int range = 0;
            if (page != 1)
            {
                return BadRequest("0 is not an acceptable value");
            }
            else if (page != 1)
            {
                range = (page * iterationSize);
            }

            //Page indicates which iteration of the set of 100 entries are to be retrieved
            /*List<Product> data = _dbCtx.Product
                .Skip(range)
                .Take(iterationSize)
                .ToList();*/

            List<Product> data = prodClass.getAllProducts(range, iterationSize);

            return Ok(data);
        }

        [HttpGet]
        [Route("list")]
        public IActionResult getAll()
        {
            //Page indicates which iteration of the set of 100 entries are to be retrieved
            List<Product> data = prodClass.getAllProducts();

            return Ok(data);
        }
        #endregion

        #region POST Products
        /*{
              "productId": 0,
              "name": "a",
              "productNumber": "a",
              "makeFlag": false,
              "finishedGoodsFlag": false,
              "safetyStockLevel": 1000,
              "reorderPoint": 750,
              "standardCost": 0,
              "listPrice": 0,
              "sizeUnitMeasureCode": null,
              "weightUnitMeasureCode": null,
              "daysToManufacture": 0,
              "productSubcategoryId": null,
              "productModelId": null,
              "sellStartDate": "2022-09-21T08:23:04.350Z",
              "rowguid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
              "modifiedDate": "2022-09-21T08:23:04.350Z"
            }*/

        [HttpPost]
        [Route("add")]
        public IActionResult add(ProductDTO product)
        {
            if (ModelState.IsValid && product != null)
            {
                switch (prodClass.addProduct(product))
                {
                    case 1:
                        return Ok("Product Added");
                    case 0:
                        return BadRequest("Product could not be added");
                }
            }
            return BadRequest(HttpStatusCode.BadRequest);
        }
        #endregion

        #region PUT Products
        [HttpPost]
        [Route("update/{id}")]
        public IActionResult update(int id, ProductDTO newProduct)
        {
            if (ModelState.IsValid && newProduct != null)
            {
                //Save changes, return results
                switch (prodClass.updateProduct(id, newProduct))
                {
                    case 1:
                        return Ok("Product Updated");
                    case 0:
                        return BadRequest("Product could not be updated");
                }
            }
            else
            {
                return BadRequest("Product not found");
            }
            return BadRequest(HttpStatusCode.BadRequest);
        }
        #endregion

        #region DELETE Products
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult delete(int id)
        {
            //Save changes, return results
            switch (prodClass.deleteProduct(id))
            {
                case 1:
                    return Ok("Product Deleted");
                case 0:
                    return BadRequest("Product could not be deleted");
            }
            return BadRequest(HttpStatusCode.BadRequest);
        }
        #endregion

        #endregion
    }
}
