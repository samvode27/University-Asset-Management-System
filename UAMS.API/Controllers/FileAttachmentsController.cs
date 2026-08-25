using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UAMS.Application.DTOs.FileAttachments.Requests;
using UAMS.Application.DTOs.FileAttachments.Responses;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Enums;

namespace UAMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class FileAttachmentsController : ControllerBase
{
    private readonly IFileAttachmentService _fileAttachmentService;

    public FileAttachmentsController(
        IFileAttachmentService fileAttachmentService)
    {
        _fileAttachmentService = fileAttachmentService;
    }


    // ================================================================
    // GET: api/FileAttachments/{id}
    // Get File Attachment By ID
    // ================================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileAttachmentResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .GetByIdAsync(
                    id,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/{id}/details
    // Get File Attachment Details
    // ================================================================

    [HttpGet("{id:guid}/details")]
    [ProducesResponseType(
        typeof(FileAttachmentDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileAttachmentDetailResponseDto>>
        GetDetails(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .GetDetailsAsync(
                    id,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments
    // Get Files With Filtering / Pagination
    // ================================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(FileAttachmentListResponseDto),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<FileAttachmentListResponseDto>>
        GetAll(
            [FromQuery] FileAttachmentFilterRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var result = await _fileAttachmentService
                .GetAllAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/name/{fileName}
    // Get File By Original File Name
    // ================================================================

    [HttpGet("name/{fileName}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        GetByFileName(
            string fileName,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .GetByFileNameAsync(
                    fileName,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/stored/{storedFileName}
    // Get File By Stored File Name
    // ================================================================

    [HttpGet("stored/{storedFileName}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        GetByStoredFileName(
            string storedFileName,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .GetByStoredFileNameAsync(
                    storedFileName,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/entity/{entityName}/{entityId}
    // Get Files By Entity
    // ================================================================

    [HttpGet("entity/{entityName}/{entityId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<FileAttachmentResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<FileAttachmentResponseDto>>>
        GetByEntity(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken)
    {
        try
        {
            var files = await _fileAttachmentService
                .GetByEntityAsync(
                    entityName,
                    entityId,
                    cancellationToken);

            return Ok(files);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/entity/{entityName}/{entityId}/active
    // Get Active Files By Entity
    // ================================================================

    [HttpGet("entity/{entityName}/{entityId:guid}/active")]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<FileAttachmentResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<FileAttachmentResponseDto>>>
        GetActiveByEntity(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken)
    {
        try
        {
            var files = await _fileAttachmentService
                .GetActiveByEntityAsync(
                    entityName,
                    entityId,
                    cancellationToken);

            return Ok(files);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/user/{userId}
    // Get Files Uploaded By User
    // ================================================================

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<FileAttachmentResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<FileAttachmentResponseDto>>>
        GetByUploadedById(
            Guid userId,
            CancellationToken cancellationToken)
    {
        try
        {
            var files = await _fileAttachmentService
                .GetByUploadedByIdAsync(
                    userId,
                    cancellationToken);

            return Ok(files);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/user/{userId}/active
    // Get Active Files Uploaded By User
    // ================================================================

    [HttpGet("user/{userId:guid}/active")]
    [ProducesResponseType(
        typeof(IReadOnlyCollection<FileAttachmentResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<
        ActionResult<IReadOnlyCollection<FileAttachmentResponseDto>>>
        GetActiveByUploadedById(
            Guid userId,
            CancellationToken cancellationToken)
    {
        try
        {
            var files = await _fileAttachmentService
                .GetActiveByUploadedByIdAsync(
                    userId,
                    cancellationToken);

            return Ok(files);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/checksum/{checksum}
    // Get File By Checksum
    // ================================================================

    [HttpGet("checksum/{checksum}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        GetByChecksum(
            string checksum,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .GetByChecksumAsync(
                    checksum,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/FileAttachments/upload
    // Upload File
    // ================================================================

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Upload(
            [FromForm] UploadFileAttachmentRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var uploadedById = GetCurrentUserId();

            var file = await _fileAttachmentService
                .UploadAsync(
                    request,
                    uploadedById,
                    cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = file.Id
                },
                file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // PUT: api/FileAttachments/{id}
    // Update File Metadata
    // ================================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Update(
            Guid id,
            [FromBody] UpdateFileAttachmentRequestDto request,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .UpdateAsync(
                    id,
                    request,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/FileAttachments/{id}/archive
    // Archive File
    // ================================================================

    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Archive(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .ArchiveAsync(
                    id,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/FileAttachments/{id}/restore
    // Restore File
    // ================================================================

    [HttpPost("{id:guid}/restore")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Restore(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var restoredById = GetCurrentUserId();

            var file = await _fileAttachmentService
                .RestoreAsync(
                    id,
                    restoredById,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // DELETE: api/FileAttachments/{id}
    // Soft Delete File
    // ================================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Delete(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var deletedById = GetCurrentUserId();

            var file = await _fileAttachmentService
                .DeleteAsync(
                    id,
                    deletedById,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/FileAttachments/{id}/activate
    // Activate File
    // ================================================================

    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Activate(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .ActivateAsync(
                    id,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // POST: api/FileAttachments/{id}/deactivate
    // Deactivate File
    // ================================================================

    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(
        typeof(FileAttachmentResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FileAttachmentResponseDto>>
        Deactivate(
            Guid id,
            CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileAttachmentService
                .DeactivateAsync(
                    id,
                    cancellationToken);

            if (file is null)
            {
                return NotFound(new
                {
                    message = "File attachment was not found."
                });
            }

            return Ok(file);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/checksum/{checksum}/exists
    // Checksum Exists
    // ================================================================

    [HttpGet("checksum/{checksum}/exists")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExistsByChecksum(
        string checksum,
        CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _fileAttachmentService
                .ExistsByChecksumAsync(
                    checksum,
                    cancellationToken);

            return Ok(new
            {
                exists
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // GET: api/FileAttachments/user/{userId}/total-size
    // Get Total File Size
    // ================================================================

    [HttpGet("user/{userId:guid}/total-size")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalFileSize(
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            var totalSize =
                await _fileAttachmentService
                    .GetTotalFileSizeByUploadedByIdAsync(
                        userId,
                        cancellationToken);

            return Ok(new
            {
                userId,
                totalFileSize = totalSize
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // ================================================================
    // Current Authenticated User
    // ================================================================

    private Guid GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? User.FindFirstValue("userId");

        if (!Guid.TryParse(
                userIdValue,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "The authenticated user identifier is missing or invalid.");
        }

        return userId;
    }
}