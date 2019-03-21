using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;
using FotoScan.Tablet.Constants;
using FotoScan.Tablet.Droid.Utilities;
using FotoScan.Tablet.Enums;
using FotoScan.Tablet.Interfaces;
using FotoScan.Tablet.Models.FileModels;
using Xamarin.Forms;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;

[assembly: Dependency(typeof(FileService))]
namespace FotoScan.Tablet.Droid.Utilities
{
    public class FileService : IFileService
    {
        public string FilePath { get; } = Environment.ExternalStorageDirectory.ToString();

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

        public byte[] GetCompressedBitmap(string imagePath)
        {
            float maxHeight = 1920.0f;
            float maxWidth = 1080.0f;
            Bitmap scaledBitmap = null;
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            Bitmap bmp = BitmapFactory.DecodeFile(imagePath, options);

            int actualHeight = options.OutHeight;
            int actualWidth = options.OutWidth;
            float imgRatio = (float)actualWidth / (float)actualHeight;
            float maxRatio = maxWidth / maxHeight;

            if (actualHeight > maxHeight || actualWidth > maxWidth)
            {
                if (imgRatio < maxRatio)
                {
                    imgRatio = maxHeight / actualHeight;
                    actualWidth = (int)(imgRatio * actualWidth);
                    actualHeight = (int)maxHeight;
                }
                else if (imgRatio > maxRatio)
                {
                    imgRatio = maxWidth / actualWidth;
                    actualHeight = (int)(imgRatio * actualHeight);
                    actualWidth = (int)maxWidth;
                }
                else
                {
                    actualHeight = (int)maxHeight;
                    actualWidth = (int)maxWidth;
                }
            }

            options.InSampleSize = CalculateInSampleSize(options, actualWidth, actualHeight);
            options.InJustDecodeBounds = false;
            options.InDither = false;
            options.InPurgeable = true;
            options.InInputShareable = true;
            options.InTempStorage = new byte[16 * 1024];

            try
            {
                bmp = BitmapFactory.DecodeFile(imagePath, options);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }
            try
            {
                scaledBitmap = Bitmap.CreateBitmap(actualWidth, actualHeight, Bitmap.Config.Argb8888);
            }
            catch (Java.Lang.OutOfMemoryError exception)
            {
                exception.PrintStackTrace();
            }

            float ratioX = actualWidth / (float)options.OutWidth;
            float ratioY = actualHeight / (float)options.OutHeight;
            float middleX = actualWidth / 2.0f;
            float middleY = actualHeight / 2.0f;

            Matrix scaleMatrix = new Matrix();
            scaleMatrix.SetScale(ratioX, ratioY, middleX, middleY);

            Canvas canvas = new Canvas(scaledBitmap);
            canvas.Matrix = scaleMatrix;
            canvas.DrawBitmap(bmp, middleX - bmp.Width / 2, middleY - bmp.Height / 2, new Paint(PaintFlags.FilterBitmap));

            ExifInterface exif = null;
            try
            {
                exif = new ExifInterface(imagePath);
                int orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 0);
                Matrix matrix = new Matrix();
                if (orientation == 6)
                {
                    matrix.PostRotate(90);
                }
                else if (orientation == 3)
                {
                    matrix.PostRotate(180);
                }
                else if (orientation == 8)
                {
                    matrix.PostRotate(270);
                }
                scaledBitmap = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height, matrix, true);
            }
            catch (IOException e)
            {
                e.StackTrace.ToString();
            }

            MemoryStream output = new MemoryStream();
            scaledBitmap.Compress(Bitmap.CompressFormat.Jpeg, 85, output);

            byte[] byteArray = output.ToArray();

            return byteArray;
        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                int heightRatio = Java.Lang.Math.Round((float)height / (float)reqHeight);
                int widthRatio = Java.Lang.Math.Round((float)width / (float)reqWidth);
                inSampleSize = heightRatio < widthRatio ? heightRatio : widthRatio;
            }
            float totalPixels = width * height;
            float totalReqPixelsCap = reqWidth * reqHeight * 2;

            while (totalPixels / (inSampleSize * inSampleSize) > totalReqPixelsCap)
            {
                inSampleSize++;
            }
            return inSampleSize;
        }

        public ImageSource SaveImageCompressed(string localPrivateFilePath, byte[] compressedImage, LeadType type = LeadType.Form, string name = "")
        {
            //Read all the PDF data from the LOCAL PRIVATE STORAGE where you saved it to
            // create fileService directory
            //Copy the private fileService's data to the EXTERNAL PUBLIC location
            File.WriteAllBytes(localPrivateFilePath + "compress", compressedImage);

            var image = ImageSource.FromFile(localPrivateFilePath + "compress");

            return image;
        }

        //public async Task<FileModel> SaveFile(string localPrivateFilePath, string leadName, LeadType type = LeadType.Form,string name = "")
        //{
        //    //Read all the PDF data from the LOCAL PRIVATE STORAGE where you saved it to
        //    var bytes = File.ReadAllBytes(localPrivateFilePath);
        //    var date = DateTime.Now;
        //    var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";

        //    // create fileService directory
        //    var filePath = CreateFilePath(type);
        //    InitStorageUpload(filePath);

        //    var localPublicFilePath = Path.Combine(filePath, filename);

        //    //Copy the private fileService's data to the EXTERNAL PUBLIC location
        //    File.WriteAllBytes(localPublicFilePath, bytes);

        //    return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        //}

        public async Task<FileModel> SaveFile(string localPrivateFilePath, string eventName, LeadType type = LeadType.Form, string name = "", string content = "")
        {
            var date = DateTime.Now;
            string filename = $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}";
            //var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";
            filename += !(type == LeadType.Note) ? ".pdf" : ".txt";

            // create fileService directory
            var filePath = CreateFilePath(eventName, type);
            InitStorageUpload(filePath);

            var localPublicFilePath = Path.Combine(filePath, filename);
            if (type == LeadType.Note)
            {
                File.WriteAllText(localPublicFilePath, content);
            }
            else
            {
                // Read all the PDF data from the LOCAL PRIVATE STORAGE where you saved it to
                var bytes = File.ReadAllBytes(localPrivateFilePath);
                //Copy the private fileService's data to the EXTERNAL PUBLIC location
                File.WriteAllBytes(localPublicFilePath, bytes);
            }
            return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        }

        public async Task<FileModel> SaveNotesFile(string eventName, LeadType type = LeadType.Form, string name = "", string content = "", string existPath = "")
        {
            var date = DateTime.Now;
            string filename = $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}";
            //var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";
            filename += !(type == LeadType.Note) ? ".pdf" : ".txt";

            // create fileService directory
            string localPublicFilePath = string.Empty;
            if (string.IsNullOrEmpty(existPath))
            {
                string filePath = CreateFilePath(eventName, type);
                InitStorageUpload(filePath);
                localPublicFilePath = Path.Combine(filePath, filename);
            }
            else
            {
                localPublicFilePath = existPath;
            }

            File.WriteAllText(localPublicFilePath, content);

            return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        }
        public async Task<FileModel> SaveXmlFile(string eventName, LeadType type = LeadType.Form, string name = "", string content = "", string existPath = "")
        {
            var date = DateTime.Now;
            string filename = $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}";
            //var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";
            filename += !(type == LeadType.Xml) ? ".pdf" : ".xml";

            // create fileService directory
            string localPublicFilePath = string.Empty;
            if (string.IsNullOrEmpty(existPath))
            {
                string filePath = CreateFilePath(eventName, type);
                InitStorageUpload(filePath);
                localPublicFilePath = Path.Combine(filePath, filename);
            }
            else
            {
                localPublicFilePath = existPath;
            }

            File.WriteAllText(localPublicFilePath, content);

            return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        }

        public async Task<FileModel> SaveSketch(byte[] byteImage, string eventName, LeadType type = LeadType.Form, string name = "", string existPath = "")
        {
            var date = DateTime.Now;
            string filename = $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.jpeg";
            //var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";

            // create fileService directory
            string localPublicFilePath = string.Empty;
            if (string.IsNullOrEmpty(existPath))
            {
                string filePath = CreateFilePath(eventName, type);
                InitStorageUpload(filePath);
                localPublicFilePath = Path.Combine(filePath, filename);
            }
            else
            {
                localPublicFilePath = existPath;
            }
            File.WriteAllBytes(localPublicFilePath, byteImage);
            return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        }

        public async Task<FileModel> SaveFilePdf(string localPrivateFilePath, string leadName, LeadType type = LeadType.Form, string name = "", string content = "")
        {
            //Read all the PDF data from the LOCAL PRIVATE STORAGE where you saved it to
            var bytes = File.ReadAllBytes(localPrivateFilePath);
            var date = DateTime.Now;
            var filename = string.IsNullOrWhiteSpace(name) ? $"{type.ToString()} {date:yyyy-MM-dd HH.mm.ss}.pdf" : $"{name}.pdf";

            // create fileService directory
            var filePath = CreateFilePath(leadName, type);
            InitStorageUpload(filePath);

            var localPublicFilePath = Path.Combine(filePath, "Compress" + filename);

            //Copy the private fileService's data to the EXTERNAL PUBLIC location
            File.WriteAllBytes(localPublicFilePath, bytes);

            return new FileModel() { FileName = filename, LocalPath = localPublicFilePath, IsSaved = true, Type = type, CreateDate = date };
        }

        private string CreateFilePath(string eventName, LeadType type)
        {
            return LeadConstant.FotoScanPath(eventName, type);
        }

        // Check if the fileService has the same name or not 
        private bool CheckFileNameExists()
        {
            return false;
        }
    }
}