using System;
using Foundation;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using CoreImage;
using System.Drawing;
using SmartMarket.Files;
using SmartMarket.iOS.Utilities;

[assembly: Dependency(typeof(FileService))]
namespace SmartMarket.iOS.Utilities
{
    public class FileService : IFileService
    {
        public string FilePath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) return false;

            File.Delete(filePath);
            return true;
        }

        public bool InitStorageUpload(string filePath)
        {
            if (File.Exists(filePath)) return false;

            Directory.CreateDirectory(filePath);
            return true;
        }

       

        // Check if the fileService has the same name or not 
        private bool CheckFileNameExists()
        {
            return false;
        }

        public byte[] GetCompressedBitmap(string imagePath)
        {
            UIImage image = new UIImage(imagePath);
            byte[] imageBytes;
            // AsJPEG compression argument can be 0 to 1
            // 0 is max compression (lowest quality), 1 is best quality
            using (NSData jpgImage = image.AsJPEG(0.8f))
            {
                imageBytes = jpgImage.ToArray();
                // upload your image data, write to a file, etc.
            }
            return imageBytes;
        }

        //public ImageSource SaveImageCompressed(string localPrivateFilePath, byte[] compressedImage, LeadType type = LeadType.Form, string name = "")
        //{
        //    //Read all the PDF data from the LOCAL PRIVATE STORAGE where you saved it to
        //    // create fileService directory
        //    //Copy the private fileService's data to the EXTERNAL PUBLIC location
        //    File.WriteAllBytes(localPrivateFilePath + "compress", compressedImage);

        //    var image = ImageSource.FromFile(localPrivateFilePath + "compress");

        //    return image;
        //}

        public async Task<byte[]> ResizeImage(byte[] bytes, string v, int ratioResized)
        {
            UIImage image = new UIImage(v);
            byte[] imageBytes;
            // AsJPEG compression argument can be 0 to 1
            // 0 is max compression (lowest quality), 1 is best quality
            var sourceSize = image.Size;

            var width = sourceSize.Width / ratioResized;
            var height = sourceSize.Height / ratioResized;

            UIGraphics.BeginImageContext(new SizeF((int)width, (int)height));
            image.Draw(new RectangleF(0, 0, (int)width, (int)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            using (NSData jpgImage = resultImage.AsJPEG(1f))
            {
                imageBytes = jpgImage.ToArray();
                // upload your image data, write to a file, etc.
            }
            return imageBytes;
        }
    }
    //public static class ImageSourceExtensions
    //{
    //    static ImageLoaderSourceHandler s_imageLoaderSourceHandler;

    //    static ImageSourceExtensions()
    //    {
    //        s_imageLoaderSourceHandler = new ImageLoaderSourceHandler();
    //    }
    //    public static Task<UIImage> ToUIImage(this ImageSource imageSource)
    //    {
    //        return s_imageLoaderSourceHandler.LoadImageAsync(imageSource);
    //    }
    //}
}