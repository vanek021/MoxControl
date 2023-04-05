using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoxControl.Services;
using MoxControl.ViewModels.ImageViewModels;
using MoxControl.ViewModels.TemplateViewModels;

namespace MoxControl.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        private readonly TemplateService _templateService;

        public TemplateController(TemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task<IActionResult> Index()
        {
            var templateIndexVm = await _templateService.GetTemplateIndexViewModelAsync();
            return View(templateIndexVm);
        }

        public async Task<IActionResult> Create()
        {
            var imageCreateVm = await _templateService.GetTemplateViewModelForCreateAsync();
            return View(imageCreateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TemplateCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _templateService.CreateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            viewModel.Images = await _templateService.GetImagesSelectListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var imageViewModel = await _templateService.GetTemplateViewModelForEditAsync(id);

            if (imageViewModel is null)
                return NotFound();

            return View(imageViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TemplateCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _templateService.UpdateAsync(viewModel);

                if (result)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }

            ModelState.AddModelError(string.Empty, "Что-то пошло не так, проверьте правильность введенных данных");
            viewModel.Images = await _templateService.GetImagesSelectListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(long id)
        {
            await _templateService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
