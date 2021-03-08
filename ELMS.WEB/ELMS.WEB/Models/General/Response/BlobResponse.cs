using ELMS.WEB.Models.Base.Response;

namespace ELMS.WEB.Models.General.Response
{
    public class BlobResponse : BaseEntityResponse
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}