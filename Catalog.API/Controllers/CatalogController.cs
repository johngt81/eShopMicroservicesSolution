using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Infrastructure;
using Catalog.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext catalogContext;

        public CatalogController(CatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        // GET: api/Catalog
        [HttpGet]
        [Route("items/{id:int}", Name = nameof(GetItemByIdAsync))]
        public async Task<ActionResult<CatalogItem>> GetItemByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();

            var catalogItem = await catalogContext.CatalogItems.FirstOrDefaultAsync(x => x.Id == id);

            if (catalogItem != null)
                return catalogItem;
            return NotFound();
        }

        // POST: api/Catalog
        [HttpPost]
        [Route("items")]
        public async Task<ActionResult> CreateProductAsync([FromBody] CatalogItem catalogItem)
        {
            catalogContext.CatalogItems.Add(catalogItem);
            await catalogContext.SaveChangesAsync();
            return CreatedAtRoute(nameof(GetItemByIdAsync), new { id = catalogItem.Id }, catalogItem.Id);
        }

        // PUT: api/Catalog/5
        [HttpPut]
        [Route("items")]
        public async Task<ActionResult> UpdateProductAsync([FromBody] CatalogItem catalogItem)
        {
            var item = await catalogContext.CatalogItems.FirstOrDefaultAsync(x => x.Id == catalogItem.Id);

            if (item == null)
                return NotFound();

            if (item.Price != catalogItem.Price)
            {
                //Notify about price changed

            }
            //I had to manually map properties, assigning catalogItem did not work
            item.Name = catalogItem.Name;
            item.Description = catalogItem.Description;
            item.CatalogBrandId = catalogItem.CatalogBrandId;
            item.CatalogTypeId = catalogItem.CatalogTypeId;
            item.Price = catalogItem.Price;

            catalogContext.CatalogItems.Update(item);

            await catalogContext.SaveChangesAsync();

            return CreatedAtRoute(nameof(GetItemByIdAsync), new { id = catalogItem.Id }, catalogItem.Id);
        }
    }
}
