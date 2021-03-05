using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.General.Models.Media;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;
using ELMS.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
                string uri = await __BlobService.UploadFormFile(file);
                if (!string.IsNullOrWhiteSpace(uri))
                {
                    BlobResponse _BlobResponse = await __BlobManager.CreateAsync(new CreateBlobRequest
                    {
                        Name = file.FileName,
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
    }
}
