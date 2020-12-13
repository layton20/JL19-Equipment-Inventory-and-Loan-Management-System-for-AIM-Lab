using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Entities.Equipment;
using ELMS.WEB.Models.Equipment.Response;
using System.Collections.Generic;
using System.Linq;

namespace ELMS.WEB.Adapters.Equipment
{
    internal static class EquipmentAdapter
    {
        internal static EquipmentEntity ToEntity(this CreateEquipmentViewModel model)
        {
            return model == null ? null : new EquipmentEntity
            {
                Name = model.Name,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                WarrantyExpirationDate = model.WarrantyExpirationDate,
                Status = model.Status,
                PurchasePrice = model.PurchasePrice,
                PurchaseDate = model.PurchaseDate,
            };
        }

        internal static EquipmentEntity ToEntity(this UpdateEquipmentViewModel model)
        {
            return model == null ? null : new EquipmentEntity
            {
                UID = model.UID,
                Name = model.Name,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                WarrantyExpirationDate = model.WarrantyExpirationDate,
                Status = model.Status,
                PurchasePrice = model.PurchasePrice,
                PurchaseDate = model.PurchaseDate,
            };
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
                AmendedTimestamp = entity.AmendedTimestamp
            };
        }

        internal static IList<EquipmentResponse> ToResponse(this IList<EquipmentEntity> entities)
        {
            return entities != null && entities.Count > 0 ? entities.Select(x => ToResponse(x)).ToList() : Enumerable.Empty<EquipmentResponse>().ToList();
        }
    }
}
