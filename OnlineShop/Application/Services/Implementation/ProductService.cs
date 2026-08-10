using Application.Enums.Product;
using Application.Services.Interfaces;
using Azure;
using Domain.Interfaces;
using Domain.Model;
using Domain.ViewModel.ProductViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Application.Services.Implementation
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        #region CreateProductAsync
        public async Task<CreateProductResult> CreateProductAsync(CreateProductViewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string Tags)
        {
            var res = await _productRepository.ProductExistsAsync(model.GroupId, model.Title);
            if (res)
            {
                return CreateProductResult.DuplicateTitle;
            }
            #region save productImg
            string image = "nophoto.png";
            if (ImgUpload != null && ImgUpload.Length > 0)
            {
                image = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(ImgUpload.FileName);
                string path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Product/ProductImg", image);
                using (var stream = new FileStream(path1, FileMode.Create))
                {
                    await ImgUpload.CopyToAsync(stream);
                }
            }
            else
            {
                return CreateProductResult.imageformatnotvalid;
            }
            #endregion

            var product = new Product()
            {
                GroupId = model.GroupId,
                Title = model.Title,
                CreateDate = DateTime.Now,
                Description = model.Description,
                isDelete = false,
                Price = model.Price,
                Summery = model.Summery,
                ImageName = image,
            };

            #region save tags
            if (!string.IsNullOrEmpty(Tags))
            {
                string[] tags = Tags.Split('-',StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in tags)
                {
                    product.Tags.Add(new Tags
                    {
                        TagName = item.Trim(),
                        CreateDate = DateTime.Now,
                    });
                }
            }
            #endregion

            #region saveGallary
            if (Gimgupload != null && Gimgupload.Length > 0)
            {
                foreach (var item in Gimgupload)
                {
                    string galleryImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.FileName);
                    string path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Product/ProductGallary", galleryImageName);
                    using (var stream = new FileStream(path2, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    product.ProductGallery.Add(new ProductGallery
                    {
                        CreateDate = DateTime.Now,
                        isDelete=false,
                        ImageName=galleryImageName,
                    });
                }
            }

            #endregion
            await _productRepository.AddProductAsync(product);
            await _productRepository.SavechangeAsync();
            return CreateProductResult.Success;
        }
        #endregion


        #region GetAllProductsAsync
        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            var product = await _productRepository.GetAllProductsAsync();

            if (product != null && product.Any())
            {
                return product.Select(product => new ProductViewModel()
                {
                    Title = product.Title,
                    Description = product.Description,
                    Summery = product.Summery,
                    ProductGroup = product.ProductGroup,
                    CreateDate = product.CreateDate,
                    GroupId = product.GroupId,
                    ImageName = product.ImageName,
                    isDelete = product.isDelete,
                    ProductId = product.ProductId
                }).ToList();
            }
            return null;
        }
        #endregion






    }
}
