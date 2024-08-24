namespace BookApp.Helper
{
    public static class ImageHelper
    {
        public static string SaveImage(IFormFile imageFile, string webRootPath)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            // Generate a unique file name
            var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            var extension = Path.GetExtension(imageFile.FileName);
            fileName = fileName + "_" + Guid.NewGuid().ToString() + extension;

            // Define the path to save the image
            var path = Path.Combine(webRootPath, "Images", fileName);

            // Save the image file to the specified path
            using (var stream = new FileStream(path, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            // Return the URL to the saved image
            return "/Images/" + fileName;
        }
    }
}
