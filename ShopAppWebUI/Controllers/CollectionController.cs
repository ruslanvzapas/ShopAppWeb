using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Humanizer.Localisation;
using ShopAppWebUI.Models.DTOs;

namespace ShopAppWebUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CollectionController : Controller
    {
        private readonly ICollectionRepository _collectionRepo;

        public CollectionController(ICollectionRepository collectionRepo)
        {
            _collectionRepo = collectionRepo;
        }

        public async Task<IActionResult> Index()
        {
            var collections = await _collectionRepo.GetCollections();
            return View(collections);
        }

        public IActionResult AddCollection()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCollection(CollectionDTO collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }
            try
            {
                var collectionToAdd = new Collection { CollectionName = collection.CollectionName, Id = collection.Id };
                await _collectionRepo.AddCollection(collectionToAdd);
                TempData["successMessage"] = "Collection added successfully";
                return RedirectToAction(nameof(AddCollection));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Collection could not added!";
                return View(collection);
            }

        }
        //
        public async Task<IActionResult> UpdateCollection(int id)
        {
            var collection = await _collectionRepo.GetCollectionById(id);
            if (collection is null)
                throw new InvalidOperationException($"Collection with id: {id} does not found");
            var collectionToUpdate = new CollectionDTO
            {
                Id = collection.Id,
                CollectionName = collection.CollectionName
            };
            return View(collectionToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCollection(CollectionDTO collectionToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(collectionToUpdate);
            }
            try
            {
                var collection = new Collection { CollectionName = collectionToUpdate.CollectionName, Id = collectionToUpdate.Id };
                await _collectionRepo.UpdateCollection(collection);
                TempData["successMessage"] = "Collection is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Collection could not updated!";
                return View(collectionToUpdate);
            }

        }

        public async Task<IActionResult> DeleteCollection(int id)
        {
            var collection = await _collectionRepo.GetCollectionById(id);
            if (collection is null)
                throw new InvalidOperationException($"Collection with id: {id} does not found");
            await _collectionRepo.DeleteCollection(collection);
            return RedirectToAction(nameof(Index));
        }


    }
}
