using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using ShopAppWebUI.Controllers;
using ShopAppWebUI.Models;
using ShopAppWebUI.Models.DTOs;
using ShopAppWebUI.Repositories;
using ShopAppWebUI.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShopAppWebTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task AddProduct_Should_AddProduct_And_Return_RedirectToAction()
        {
            // Arrange
            var mockProductRepo = new Mock<IProductRepository>();
            var mockCollectionRepo = new Mock<ICollectionRepository>();
            var mockFileService = new Mock<IFileService>();

            // Створюємо список колекцій для мока
            var collections = new List<Collection>
            {
                new Collection { Id = 1, CollectionName = "Collection 1" },
                new Collection { Id = 2, CollectionName = "Collection 2" }
            };
            mockCollectionRepo.Setup(repo => repo.GetCollections())
                .ReturnsAsync(collections);

            // Імітуємо збереження файлу
            mockFileService.Setup(fs => fs.SaveFile(It.IsAny<IFormFile>(), It.IsAny<string[]>()))
                .ReturnsAsync("image.png");

            // Створюємо дані для тесту
            var productDto = new ProductDTO
            {
                ProductName = "Test Product",
                Price = 10.0,
                CollectionId = 1,
                ImageFile = null // Можна додати мок для IFormFile, якщо потрібно
            };

            var controller = new ProductController(mockProductRepo.Object, mockCollectionRepo.Object, mockFileService.Object);

            // Додаємо TempData до контролера
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            var result = await controller.AddProduct(productDto) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AddProduct", result.ActionName);

            // Перевіряємо, чи викликано метод AddProduct у репозиторії
            mockProductRepo.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_Should_UpdateProduct_And_Return_RedirectToAction()
        {
            // Arrange
            var mockProductRepo = new Mock<IProductRepository>();
            var mockCollectionRepo = new Mock<ICollectionRepository>();
            var mockFileService = new Mock<IFileService>();

            // Створюємо список колекцій для мока
            var collections = new List<Collection>
            {
                new Collection { Id = 1, CollectionName = "Collection 1" },
                new Collection { Id = 2, CollectionName = "Collection 2" }
            };
            mockCollectionRepo.Setup(repo => repo.GetCollections())
                .ReturnsAsync(collections);

            // Існуючий продукт для оновлення
            var existingProduct = new Product
            {
                Id = 1,
                ProductName = "Old Product",
                Price = 15.0,
                CollectionId = 1,
                Image = "oldImage.png"
            };
            mockProductRepo.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync(existingProduct);

            // Імітуємо збереження нового файлу
            mockFileService.Setup(fs => fs.SaveFile(It.IsAny<IFormFile>(), It.IsAny<string[]>()))
                .ReturnsAsync("newImage.png");

            // Створюємо DTO для оновлення
            var productDto = new ProductDTO
            {
                Id = 1,
                ProductName = "Updated Product",
                Price = 20.0,
                CollectionId = 2,
                ImageFile = new Mock<IFormFile>().Object, // Імітація нового файлу
                Image = "oldImage.png"
            };


            var controller = new ProductController(mockProductRepo.Object, mockCollectionRepo.Object, mockFileService.Object);

            // Додаємо TempData до контролера
            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            var result = await controller.UpdateProduct(productDto) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            // Перевіряємо, чи викликано метод UpdateProduct у репозиторії
            mockProductRepo.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);

            // Якщо зображення оновлено, перевіряємо виклик DeleteFile
            if (productDto.ImageFile != null && !string.IsNullOrWhiteSpace(existingProduct.Image))
            {
                mockFileService.Verify(fs => fs.DeleteFile(existingProduct.Image), Times.Once);
            }
            else
            {
                mockFileService.Verify(fs => fs.DeleteFile(It.IsAny<string>()), Times.Never);
            }

        }

        [Fact]
        public async Task DeleteProduct_Should_DeleteProduct_And_Return_RedirectToAction()
        {
            // Arrange
            var mockProductRepo = new Mock<IProductRepository>();
            var mockFileService = new Mock<IFileService>();

            var product = new Product
            {
                Id = 1,
                ProductName = "Test Product",
                Image = "testImage.png"
            };

            mockProductRepo.Setup(repo => repo.GetProductById(1))
                .ReturnsAsync(product);

            mockProductRepo.Setup(repo => repo.DeleteProduct(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Налаштовуємо метод DeleteFile як void
            mockFileService.Setup(fs => fs.DeleteFile("testImage.png"))
                .Callback(() => { });

            var controller = new ProductController(mockProductRepo.Object, null, mockFileService.Object);

            var tempDataMock = new Mock<ITempDataDictionary>();
            controller.TempData = tempDataMock.Object;

            // Act
            var result = await controller.DeleteProduct(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(controller.Index), result.ActionName);

            mockProductRepo.Verify(repo => repo.GetProductById(1), Times.Once);
            mockProductRepo.Verify(repo => repo.DeleteProduct(It.IsAny<Product>()), Times.Once);
            mockFileService.Verify(fs => fs.DeleteFile("testImage.png"), Times.Once);
        }
    }
}
