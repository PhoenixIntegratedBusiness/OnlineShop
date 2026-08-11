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
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services.Implementation
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #region UpdateProductAsync
        public async Task<UpdateProductResult> UpdateProductAsync(EditProductviewModel model, IFormFile ImgUpload, IFormFile[] Gimgupload, string TagsText)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            if (product == null)
            {
                return UpdateProductResult.Failure;
            }
            var duplicateTitle = await _productRepository.IsTitleExistAsync(model.Title, model.ProductId);
            if (duplicateTitle)
            {
                return UpdateProductResult.DuplicateTitle;
            }

            #region image  
            string Imagename = product.ImageName;
            if (ImgUpload != null && ImgUpload.Length > 0)
            {
                // عکس قبلی
                if (!string.IsNullOrEmpty(product.ImageName) &&
                    product.ImageName != "nophoto.png")
                {
                    string oldImagePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/Images/Product/ProductImg",
                        product.ImageName);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // عکس جدید
                string newImagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/Images/Product/ProductImg",
                    Imagename);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await ImgUpload.CopyToAsync(stream);
                }
            }
            #endregion

            #region ImageGallary
            if (Gimgupload != null && Gimgupload.Length > 0)
            {
                foreach (var item in Gimgupload)
                {
                    string imagegallary = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.FileName);
                    string path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product/ProductGallary/", imagegallary);
                    using (var stream = new FileStream(path2, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    product.ProductGallery.Add(new ProductGallery
                    {
                        CreateDate = DateTime.Now,
                        isDelete = false,
                        ImageName = imagegallary,
                    });
                }
            }
            #endregion

            #region Tags
            await _productRepository.GetAllTagsByIdAsync(product.ProductId);

            if (!string.IsNullOrWhiteSpace(TagsText))
            {
                var keywords = TagsText.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var keyword in keywords)
                {
                    product.Tags.Add(new Tags
                    {
                        TagName = keyword.Trim(),
                        CreateDate = DateTime.Now,
                        isDelete = false
                    });
                }
            }
            #endregion

            product.Title = model.Title;
            product.GroupId = model.GroupId;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Summery = model.Summery;
            product.isDelete = model.isDelete;
            product.CreateDate = model.CreateDate;

            //_productRepository.UpdateProduct(product);
            await _productRepository.SavechangeAsync();
            return UpdateProductResult.Success;
        }
        #endregion

        #region DeleteGallaryImgByIdAsync
        public async Task DeleteGallaryImgByIdAsync(int id)
        {
            var image = await _productRepository.FindImgGallaryAsync(id);
            if (image != null)
            {

                string deletePath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/Images/Product/ProductGallary", image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                await _productRepository.DeleteGallaryImgByIdAsync(id);
                await _productRepository.SavechangeAsync();

            }
        }
        #endregion

        #region GetProductByIdAsync    
        public async Task<EditProductviewModel> GetProductByIdAsync(int id)
        {
            var res = await _productRepository.GetProductByIdAsync(id);

            return new EditProductviewModel()
            {
                ImageName = res.ImageName,
                ProductId = res.ProductId,
                CreateDate = res.CreateDate,
                Description = res.Description,
                GroupId = res.GroupId,
                isDelete = res.isDelete,
                Price = res.Price,
                ProductGallery = res.ProductGallery,
                Summery = res.Summery,
                Tags = res.Tags,
                Title = res.Title
            };

        }

        #endregion

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
                string[] tags = Tags.Split('-', StringSplitOptions.RemoveEmptyEntries);
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
                        isDelete = false,
                        ImageName = galleryImageName,
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
