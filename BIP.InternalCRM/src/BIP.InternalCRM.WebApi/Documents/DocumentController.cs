using BIP.InternalCRM.Application;
using BIP.InternalCRM.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BIP.InternalCRM.WebApi.Documents;

[Route("documents")]
public class DocumentController : ApiControllerBase
{
    [HttpGet("images/{imageFilename}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> OnGetProductIconAsync(
        [FromRoute] string imageFilename,
        [FromServices] IImageRepository imageRepository,
        CancellationToken cancellationToken)
    {
        var result = await imageRepository.GetImageByFilenameAsync(
            imageFilename,
            cancellationToken);

        return result.Match<IActionResult>(
            value => File(value.Data, value.MediaType),
            NotFound);
    }
}