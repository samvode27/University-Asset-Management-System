using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using UAMS.Application.DTOs.FileAttachments.Requests;
using UAMS.Application.DTOs.FileAttachments.Responses;
using UAMS.Application.Interfaces.Persistence;
using UAMS.Application.Interfaces.Services;
using UAMS.Domain.Entities.FileAttachments;
using UAMS.Domain.Enums;

namespace UAMS.Application.Services.FileAttachments;

public class FileAttachmentService : IFileAttachmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _environment;

    private const string UploadDirectory =
        "uploads/fileattachments";

    public FileAttachmentService(
        IUnitOfWork unitOfWork,
        IWebHostEnvironment environment)
    {
        _unitOfWork = unitOfWork;
        _environment = environment;
    }


    // ================================================================
    // Get By ID
    // ================================================================

    public async Task<FileAttachmentResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(id, cancellationToken);

        return file is null
            ? null
            : MapToResponse(file);
    }


    // ================================================================
    // Get Details
    // ================================================================

    public async Task<FileAttachmentDetailResponseDto?> GetDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(id, cancellationToken);

        if (file is null)
            return null;

        string? uploadedByName = null;

        var user = await _unitOfWork.Users
            .GetByIdAsync(file.UploadedById, cancellationToken);

        if (user is not null)
        {
            uploadedByName = user.FullName;
        }

        return new FileAttachmentDetailResponseDto
        {
            Id = file.Id,

            EntityName = file.EntityName,
            EntityId = file.EntityId,

            FileName = file.FileName,
            StoredFileName = file.StoredFileName,
            FilePath = file.FilePath,
            ContentType = file.ContentType,
            FileExtension = file.FileExtension,
            FileSize = file.FileSize,

            FileType = file.FileType,
            Status = file.Status,

            Description = file.Description,

            UploadedById = file.UploadedById,
            UploadedByName = uploadedByName,

            UploadedAt = file.UploadedAt,

            Checksum = file.Checksum,

            IsActive = file.IsActive,

            CreatedAt = file.CreatedAt,
            CreatedBy = file.CreatedBy,
            UpdatedAt = file.UpdatedAt,
            UpdatedBy = file.UpdatedBy
        };
    }


    // ================================================================
    // Get By File Name
    // ================================================================

    public async Task<FileAttachmentResponseDto?> GetByFileNameAsync(
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException(
                "File name is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByFileNameAsync(
                fileName.Trim(),
                cancellationToken);

        return file is null
            ? null
            : MapToResponse(file);
    }


    // ================================================================
    // Get By Stored File Name
    // ================================================================

    public async Task<FileAttachmentResponseDto?>
        GetByStoredFileNameAsync(
            string storedFileName,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storedFileName))
            throw new ArgumentException(
                "Stored file name is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByStoredFileNameAsync(
                storedFileName.Trim(),
                cancellationToken);

        return file is null
            ? null
            : MapToResponse(file);
    }


    // ================================================================
    // Get By Entity
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetByEntityAsync(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken = default)
    {
        ValidateEntity(entityName, entityId);

        var files = await _unitOfWork.FileAttachments
            .GetByEntityAsync(
                entityName.Trim(),
                entityId,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Active By Entity
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetActiveByEntityAsync(
            string entityName,
            Guid entityId,
            CancellationToken cancellationToken = default)
    {
        ValidateEntity(entityName, entityId);

        var files = await _unitOfWork.FileAttachments
            .GetActiveByEntityAsync(
                entityName.Trim(),
                entityId,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Uploaded User
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default)
    {
        if (uploadedById == Guid.Empty)
            throw new ArgumentException(
                "Uploaded by user ID is required.");

        var files = await _unitOfWork.FileAttachments
            .GetByUploadedByIdAsync(
                uploadedById,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get Active By Uploaded User
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetActiveByUploadedByIdAsync(
            Guid uploadedById,
            CancellationToken cancellationToken = default)
    {
        if (uploadedById == Guid.Empty)
            throw new ArgumentException(
                "Uploaded by user ID is required.");

        var files = await _unitOfWork.FileAttachments
            .GetActiveByUploadedByIdAsync(
                uploadedById,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By File Type
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetByFileTypeAsync(
            FileAttachmentType fileType,
            CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(fileType))
            throw new ArgumentException(
                "Invalid file attachment type.");

        var files = await _unitOfWork.FileAttachments
            .GetByFileTypeAsync(
                fileType,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Status
    // ================================================================

    public async Task<IReadOnlyCollection<FileAttachmentResponseDto>>
        GetByStatusAsync(
            FileAttachmentStatus status,
            CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentException(
                "Invalid file attachment status.");

        var files = await _unitOfWork.FileAttachments
            .GetByStatusAsync(
                status,
                cancellationToken);

        return files
            .Select(MapToResponse)
            .ToList();
    }


    // ================================================================
    // Get By Checksum
    // ================================================================

    public async Task<FileAttachmentResponseDto?>
        GetByChecksumAsync(
            string checksum,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checksum))
            throw new ArgumentException(
                "Checksum is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByChecksumAsync(
                checksum.Trim(),
                cancellationToken);

        return file is null
            ? null
            : MapToResponse(file);
    }


    // ================================================================
    // Get All / Filter / Pagination
    // ================================================================

    public async Task<FileAttachmentListResponseDto> GetAllAsync(
        FileAttachmentFilterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.PageNumber < 1)
            throw new ArgumentException(
                "Page number must be greater than zero.");

        if (request.PageSize < 1 || request.PageSize > 100)
            throw new ArgumentException(
                "Page size must be between 1 and 100.");

        var files = await _unitOfWork.FileAttachments
            .GetAllAsync(cancellationToken);

        IEnumerable<FileAttachment> query = files;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim();

            query = query.Where(file =>
                file.FileName.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase) ||
                file.StoredFileName.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase) ||
                file.EntityName.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase) ||
                (file.Description != null &&
                 file.Description.Contains(
                     search,
                     StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(request.EntityName))
        {
            var entityName = request.EntityName.Trim();

            query = query.Where(file =>
                file.EntityName.Equals(
                    entityName,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (request.EntityId.HasValue)
        {
            query = query.Where(file =>
                file.EntityId == request.EntityId.Value);
        }

        if (request.UploadedById.HasValue)
        {
            query = query.Where(file =>
                file.UploadedById == request.UploadedById.Value);
        }

        if (request.FileType.HasValue)
        {
            query = query.Where(file =>
                file.FileType == request.FileType.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(file =>
                file.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(file =>
                file.UploadedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(file =>
                file.UploadedAt <= request.ToDate.Value);
        }

        if (request.MinimumFileSize.HasValue)
        {
            query = query.Where(file =>
                file.FileSize >= request.MinimumFileSize.Value);
        }

        if (request.MaximumFileSize.HasValue)
        {
            query = query.Where(file =>
                file.FileSize <= request.MaximumFileSize.Value);
        }

        var ordered = query
            .OrderByDescending(file => file.UploadedAt)
            .ToList();

        var totalCount = ordered.Count;

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)request.PageSize);

        var items = ordered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToResponse)
            .ToList();

        return new FileAttachmentListResponseDto
        {
            Items = items,

            PageNumber = request.PageNumber,
            PageSize = request.PageSize,

            TotalCount = totalCount,
            TotalPages = totalPages,

            HasPreviousPage = request.PageNumber > 1,
            HasNextPage = request.PageNumber < totalPages
        };
    }


    // ================================================================
    // Upload
    // ================================================================

    public async Task<FileAttachmentResponseDto> UploadAsync(
        UploadFileAttachmentRequestDto request,
        Guid uploadedById,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (uploadedById == Guid.Empty)
            throw new ArgumentException(
                "Uploaded by user ID is required.");

        ValidateEntity(
            request.EntityName,
            request.EntityId);

        if (request.File is null)
            throw new ArgumentException(
                "File is required.");

        if (request.File.Length <= 0)
            throw new ArgumentException(
                "Uploaded file cannot be empty.");

        var originalFileName =
            Path.GetFileName(request.File.FileName);

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ArgumentException(
                "File name is required.");

        var extension =
            Path.GetExtension(originalFileName);

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException(
                "Uploaded file must have a valid extension.");

        extension = extension.ToLowerInvariant();

        var storedFileName =
            $"{Guid.NewGuid():N}{extension}";

        var uploadRoot = Path.Combine(
            _environment.ContentRootPath,
            UploadDirectory);

        Directory.CreateDirectory(uploadRoot);

        var physicalPath = Path.Combine(
            uploadRoot,
            storedFileName);

        string checksum;

        await using (var inputStream = request.File.OpenReadStream())
        await using (var outputStream =
                     new FileStream(
                         physicalPath,
                         FileMode.CreateNew,
                         FileAccess.Write,
                         FileShare.None,
                         81920,
                         FileOptions.Asynchronous))
        {
            using var sha256 = SHA256.Create();

            var hash = await sha256.ComputeHashAsync(
                inputStream,
                cancellationToken);

            checksum = Convert.ToHexString(hash);

            inputStream.Position = 0;

            await inputStream.CopyToAsync(
                outputStream,
                cancellationToken);
        }

        try
        {
            var duplicate =
                await _unitOfWork.FileAttachments
                    .ExistsByChecksumAsync(
                        checksum,
                        cancellationToken);

            if (duplicate)
            {
                DeletePhysicalFile(physicalPath);

                throw new InvalidOperationException(
                    "A file with the same content already exists.");
            }

            var relativePath =
                Path.Combine(
                    UploadDirectory,
                    storedFileName)
                .Replace(
                    Path.DirectorySeparatorChar,
                    '/');

            var file = FileAttachment.Create(
                uploadedById,
                request.EntityName.Trim(),
                request.EntityId,
                originalFileName,
                storedFileName,
                relativePath,
                request.File.ContentType,
                extension,
                request.File.Length,
                request.FileType,
                request.Description?.Trim(),
                checksum);

            await _unitOfWork.FileAttachments.AddAsync(
                file,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return MapToResponse(file);
        }
        catch
        {
            DeletePhysicalFile(physicalPath);
            throw;
        }
    }


    // ================================================================
    // Update
    // ================================================================

    public async Task<FileAttachmentResponseDto?> UpdateAsync(
        Guid id,
        UpdateFileAttachmentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        ArgumentNullException.ThrowIfNull(request);

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "Deleted file attachments cannot be updated.");

        file.Update(
            request.Description?.Trim(),
            request.FileType);

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Archive
    // ================================================================

    public async Task<FileAttachmentResponseDto?> ArchiveAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "Deleted file attachments cannot be archived.");

        if (file.Status == FileAttachmentStatus.Archived)
            throw new InvalidOperationException(
                "File attachment is already archived.");

        file.Archive();

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Restore
    // ================================================================

    public async Task<FileAttachmentResponseDto?> RestoreAsync(
        Guid id,
        Guid restoredById,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        if (restoredById == Guid.Empty)
            throw new ArgumentException(
                "Restored by user ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "Deleted file attachments cannot be restored.");

        if (file.Status == FileAttachmentStatus.Active &&
            file.IsActive)
        {
            throw new InvalidOperationException(
                "File attachment is already active.");
        }

        file.Restore();

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Delete
    // ================================================================

    public async Task<FileAttachmentResponseDto?> DeleteAsync(
        Guid id,
        Guid deletedById,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        if (deletedById == Guid.Empty)
            throw new ArgumentException(
                "Deleted by user ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "File attachment is already deleted.");

        file.MarkDeleted(deletedById);

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Activate
    // ================================================================

    public async Task<FileAttachmentResponseDto?> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "Deleted file attachments cannot be activated.");

        if (file.IsActive)
            throw new InvalidOperationException(
                "File attachment is already active.");

        file.Activate();

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Deactivate
    // ================================================================

    public async Task<FileAttachmentResponseDto?> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(
                "File attachment ID is required.");

        var file = await _unitOfWork.FileAttachments
            .GetByIdAsync(
                id,
                cancellationToken);

        if (file is null)
            return null;

        if (file.IsDeleted)
            throw new InvalidOperationException(
                "Deleted file attachments cannot be deactivated.");

        if (!file.IsActive)
            throw new InvalidOperationException(
                "File attachment is already inactive.");

        file.Deactivate();

        _unitOfWork.FileAttachments.Update(file);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(file);
    }


    // ================================================================
    // Checksum Exists
    // ================================================================

    public Task<bool> ExistsByChecksumAsync(
        string checksum,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(checksum))
            throw new ArgumentException(
                "Checksum is required.");

        return _unitOfWork.FileAttachments
            .ExistsByChecksumAsync(
                checksum.Trim(),
                cancellationToken);
    }


    // ================================================================
    // Total File Size
    // ================================================================

    public Task<long> GetTotalFileSizeByUploadedByIdAsync(
        Guid uploadedById,
        CancellationToken cancellationToken = default)
    {
        if (uploadedById == Guid.Empty)
            throw new ArgumentException(
                "Uploaded by user ID is required.");

        return _unitOfWork.FileAttachments
            .GetTotalFileSizeByUploadedByIdAsync(
                uploadedById,
                cancellationToken);
    }


    // ================================================================
    // Validation
    // ================================================================

    private static void ValidateEntity(
        string entityName,
        Guid entityId)
    {
        if (string.IsNullOrWhiteSpace(entityName))
            throw new ArgumentException(
                "Entity name is required.");

        if (entityId == Guid.Empty)
            throw new ArgumentException(
                "Entity ID is required.");
    }


    // ================================================================
    // Physical File Cleanup
    // ================================================================

    private static void DeletePhysicalFile(
        string physicalPath)
    {
        try
        {
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }
        catch
        {
            // Do not hide the original operation exception.
        }
    }


    // ================================================================
    // Mapping
    // ================================================================

    private static FileAttachmentResponseDto MapToResponse(
        FileAttachment file)
    {
        return new FileAttachmentResponseDto
        {
            Id = file.Id,

            EntityName = file.EntityName,
            EntityId = file.EntityId,

            FileName = file.FileName,
            StoredFileName = file.StoredFileName,
            FilePath = file.FilePath,
            ContentType = file.ContentType,
            FileExtension = file.FileExtension,
            FileSize = file.FileSize,

            FileType = file.FileType,
            Status = file.Status,

            Description = file.Description,

            UploadedById = file.UploadedById,
            UploadedAt = file.UploadedAt,

            IsActive = file.IsActive,

            CreatedAt = file.CreatedAt
        };
    }
}