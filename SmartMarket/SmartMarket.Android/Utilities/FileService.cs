using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;
using SmartMarket.Droid.Utilities;
using SmartMarket.Files;
using Xamarin.Forms;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;

[assembly: Dependency(typeof(FileService))]
namespace SmartMarket.Droid.Utilities
{
    public class FileService : IFileService
    {

        string IFileService.FilePath => Environment.ExternalStorageDirectory.ToString();

        public bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath)) return false;

            File.Delete(filePath);
            return true;
        }

        public byte[] GetCompressedBitmap(string imagePath)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> ResizeImage(byte[] imageData, string imagePath, int ratioResized)
        {
            // Load the bitmap 
            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
            options.InSampleSize = ratioResized;
            //Bitmap originalImages = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);
            Bitmap originalImage = BitmapFactory.DecodeFile(imagePath, options);

            //float newHeight = 0;
            //float newWidth = 0;

            //var originalHeight = originalImage.Height;
            //var originalWidth = originalImage.Width;

            //newHeight = originalHeight * 1f;
            //newWidth = originalWidth * 1f;
            //Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            //originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                originalImage.Compress(format: Bitmap.CompressFormat.Jpeg, quality: 80, stream: ms);

                originalImage.Recycle();

                return ms.ToArray();
            }

        }
        public string GetWidthHeight(byte[] imageData, string imagePath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
            //Bitmap originalImages = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);
            Bitmap originalImage = BitmapFactory.DecodeFile(imagePath, options);

            return originalImage.Width.ToString() + "*" + originalImage.Height.ToString();

        }
    }
}