using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Domain.Contracts.Models.RequestModels;
using URLShortener.Domain.Exceptions;
using URLShortener.Manager;

namespace URLShortener.Controllers
{
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly IAppManager _manager;

        public AppController(IAppManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _manager.GetAllUsersAsync();

            return new JsonResult(result);
        }

        [HttpGet]
        [Route("User/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            try
            {
                var result = await _manager.GetUserByIdAsync(id);

                return new JsonResult(result);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create/User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel userModel)
        {
            try
            {
                await _manager.CreateUserAsync(userModel);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex) when (ex is NotFoundException || ex is DuplicateEmailException)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUrlById([FromRoute] string id)
        {
            try
            {
                var url = await _manager.GetUrlByIdAsync(id);
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("AllUrls")]
        public async Task<IActionResult> GetAllUrls()
        {
            var result = await _manager.GetAllUrlsAsync();

            return new JsonResult(result);
        }

        [HttpDelete]
        [Route("Delete/User/{id}/Url/{url}")]
        public async Task<IActionResult> DeleteUrl([FromRoute] string id, [FromRoute] string url)
        {
            try
            {
                await _manager.DeleteUrlAsync(id, url);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create/Url")]
        public async Task<IActionResult> CreateUrl([FromBody] CreateUrlRequestModel createUrlModel)
        {
            try
            {
                await _manager.CreateUrlAsync(createUrlModel);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex) when (ex is NotFoundException || ex is DuplicateUrlException || ex is UrlException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}