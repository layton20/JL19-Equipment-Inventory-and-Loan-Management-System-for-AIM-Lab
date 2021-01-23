using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Equipment
{
    internal static class EquipmentAdapter
    {
        internal static CreateEquipmentRequest ToRequest(this CreateEquipmentViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            CreateEquipmentRequest _Request = new CreateEquipmentRequest
            {
                Name = model.Name,
                WarrantyExpirationDate = model.WarrantyExpirationDate,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                Status = (Status)model.Status,
                PurchaseDate = model.PurchaseDate,
                PurchasePrice = model.PurchasePrice,
                Quantity = model.Quantity,
            };

            if (Guid.TryParse(model.OwnerUID, out Guid ownerUID))
            {
                _Request.OwnerUID = ownerUID;
            }

            return _Request;
        }

        internal static EquipmentEntity ToEntity(this CreateEquipmentRequest request)
        {
            return request == null ? null : new EquipmentEntity
            {
                Name = request.Name,
                Description = request.Description,
                SerialNumber = request.SerialNumber,
                WarrantyExpirationDate = request.WarrantyExpirationDate,
                Status = request.Status,
                PurchasePrice = request.PurchasePrice,
                PurchaseDate = request.PurchaseDate,
                OwnerUID = request.OwnerUID
            };
        }

        internal static EquipmentEntity ToEntity(this EquipmentViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            EquipmentEntity _Entity = new EquipmentEntity
            {
                UID = model.UID,
                Name = model.Name,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                WarrantyExpirationDate = model.WarrantyExpirationDate,
                Status = model.Status,
                PurchasePrice = model.PurchasePrice,
                PurchaseDate = model.PurchaseDate
            };

            if (Guid.TryParse(model.OwnerUID, out Guid ownerUID))
            {
                _Entity.OwnerUID = ownerUID;
            }

            return _Entity;
        }

        internal static EquipmentResponse ToResponse(this EquipmentEntity entity)
        {
            return entity == null ? null : new EquipmentResponse
            {
                UID = entity.UID,
                Name = entity.Name,
                Description = entity.Description,
                SerialNumber = entity.SerialNumber,
                PurchasePrice = entity.PurchasePrice,
                PurchaseDate = entity.PurchaseDate,
                Status = entity.Status,
                WarrantyExpirationDate = entity.WarrantyExpirationDate,
                CreatedTimestamp = entity.CreatedTimestamp,
                AmendedTimestamp = entity.AmendedTimestamp,
                OwnerUID = entity.OwnerUID
            };
        }

        internal static IList<EquipmentResponse> ToResponse(this IList<EquipmentEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(x => ToResponse(x)).ToList() : Enumerable.Empty<EquipmentResponse>().ToList();
        }

        internal static EquipmentViewModel ToViewModel(this EquipmentResponse response)
        {
            return response == null ? null : new EquipmentViewModel
            {
                UID = response.UID,
                Name = response.Name,
                Description = response.Description,
                SerialNumber = response.SerialNumber,
                Status = response.Status,
                WarrantyExpirationDate = response.WarrantyExpirationDate,
                PurchaseDate = response.PurchaseDate,
                PurchasePrice = response.PurchasePrice,
                OwnerUID = response.OwnerUID.ToString()
            };
        }

        internal static IList<EquipmentViewModel> ToViewModel(this IList<EquipmentResponse> responses)
        {
            return responses != null && responses.Count > 0 ? responses.Select(x => ToViewModel(x)).ToList() : Enumerable.Empty<EquipmentViewModel>().ToList();
        }

        internal static UpdateEquipmentViewModel ToUpdateViewModel(this EquipmentResponse response)
        {
            return response == null ? null : new UpdateEquipmentViewModel
            {
                UID = response.UID,
                Name = response.Name,
                Description = response.Description,
                WarrantyExpirationDate = response.WarrantyExpirationDate,
                Status = response.Status,
                PurchaseDate = response.PurchaseDate,
                PurchasePrice = response.PurchasePrice,
                SerialNumber = response.SerialNumber,
                OwnerUID = response.OwnerUID
            };
        }
    }
}
