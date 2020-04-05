using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.requests;
using api.Servers.ProductServer.Impl;
using api.Servers.ProductServer.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        ///// <summary>
        ///// @xis 添加产品 2020-3-29 09:06:08
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost("addproduct")]
        //[Privilege("add_product")]
        //public async Task<IActionResult> AddProduct([FromBody]AddProductModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return GetModelErrorCode();
        //    }

        //    reqmodel<AddProductModel> reqmodel = await RequestPackingAsync(model);
        //    IProductServer productServer = new ProductServerImpl();

        //    return await productServer.AddProduct(reqmodel);
        //}

        /// <summary>
        /// @xis 删除产品 2020-3-29 09:06:08
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("deleteproduct")]
        [Privilege("delete_product")]
        public async Task<IActionResult> DeleteProduct([FromBody]int id)
        {
            reqmodel<int> reqmodel = await RequestPackingAsync(id);
            IProductServer productServer = new ProductServerImpl();

            return await productServer.DeleteProduct(reqmodel);
        }
    }
}
