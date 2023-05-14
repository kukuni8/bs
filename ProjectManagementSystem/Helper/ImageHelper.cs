namespace ProjectManagementSystem.Helper
{
    public interface IImageService
    {
        string SaveCoverImage(IFormFile file);
    }

    public class ImageHelper : IImageService
    {
        private readonly IWebHostEnvironment env;

        public ImageHelper(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public string SaveCoverImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(env.WebRootPath, "document", fileName);

                // 检查文件是否已经存在
                if (System.IO.File.Exists(filePath))
                {
                    return $"/document/{fileName}";
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return $"/document/{fileName}";
            }
            else
            {
                return null;
            }
        }
    }
}
