using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;
using MoxControl.ViewModels.ImageViewModels;

namespace MoxControl.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _imageService.GetImageIndexViewModelAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(long id)
        {
            var viewModel = await _imageService.GetImageDetailsViewModelAsync(id);
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ImageViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _imageService.CreateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var imageViewModel = await _imageService.GetImageViewModelAsync(id);

            if (imageViewModel is null)
                return NotFound();

            return View(imageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ImageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _imageService.UpdateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(long id)
        {
            await _imageService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
