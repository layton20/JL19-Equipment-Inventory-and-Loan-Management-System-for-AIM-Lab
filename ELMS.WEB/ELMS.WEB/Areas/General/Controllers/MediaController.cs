using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.General.Models.Media;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;
using ELMS.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.General.Controllers
{
    [Authorize]
    [Area("General")]
    public class MediaController : Controller
    {
        private readonly IMapper __Mapper;
        // BlobManager manages the DB records that track each stored blob (uploaded onto Azure)
        private readonly IBlobManager __BlobManager;
        // BlobService manages the Azure storage blob container
        private readonly IBlobService __BlobService;
        private readonly IEquipmentBlobManager __EquipmentBlobManager;
        private readonly string ENTITY_NAME = "Media";

        public MediaController(IMapper mapper, IBlobManager blobManager, IBlobService blobService, IEquipmentBlobManager equipmentBlobManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __BlobManager = blobManager ?? throw new ArgumentNullException(nameof(blobManager));
            __BlobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            __EquipmentBlobManager = equipmentBlobManager ?? throw new ArgumentNullException(nameof(equipmentBlobManager));
        }

        [HttpPost]
        public async Task<IActionResult> UploadEquipmentMediaAsync(DetailsViewModel model)
        {
            foreach (IFormFile file in model.UploadMedia.MediaFiles)
            {
                string blobName = $"{Guid.NewGuid()}_{file.FileName}";
                string uri = await __BlobService.UploadFormFile(file, blobName);

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    BlobResponse _BlobResponse = await __BlobManager.CreateAsync(new CreateBlobRequest
                    {
                        Name = blobName,
                        Path = uri
                    });

                    await __EquipmentBlobManager.CreateAsync(new CreateEquipmentBlobRequest
                    {
                        EquipmentUID = model.UploadMedia.EquipmentUID,
                        BlobUID = _BlobResponse.UID
                    });
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEquipmentMediaModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            EquipmentBlobResponse _Response = await __EquipmentBlobManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return PartialView("_DeleteEquipmentMediaModal", new DeleteEquipmentMediaViewModel
            {
                UID = uid
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEquipmentMediaAsync(DeleteEquipmentMediaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            EquipmentBlobResponse _RetrieveResponse = await __EquipmentBlobManager.GetByUIDAsync(model.UID);

            if (!_RetrieveResponse.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            await __BlobService.DeleteBlobAsync(_RetrieveResponse.Blob.Name);
            await __EquipmentBlobManager.DeleteAsync(_RetrieveResponse.UID);
            await __BlobManager.DeleteAsync(_RetrieveResponse.BlobUID);

            return RedirectToAction("DetailsView", "Equipment", new { Area = "Equipment", equipmentUID = _RetrieveResponse.EquipmentUID, successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} removed {ENTITY_NAME}." });
        }
    }
}
