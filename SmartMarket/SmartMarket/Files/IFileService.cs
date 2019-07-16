using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMarket.Files
{
    public interface IFileService
    {
        string FilePath { get; }

        bool DeleteFile(string filePath);

        Task<byte[]> ResizeImage(byte[] imageData, string imagePath, int ratioResized);

        byte[] GetCompressedBitmap(string imagePath);

        //Task<string> CreateFilePath(LeadType type, string imageName);

        //Task<ImageSource> SaveImage(string localPrivateFilePath, byte[] compressedImage, LeadType type = LeadType.Form,
        //    string name = "");

        //Task<ImageSource> SaveImageCompressed(string localPrivateFilePath, byte[] compressedImage, LeadType type = LeadType.Form,
        //    string name = "");

        //Task<FileModel> SaveFile(string localPrivateFilePath, LeadType type = LeadType.Form, string name = null);

        //Task<FileModel> SaveFilePdf(string localPrivateFilePath, LeadType type = LeadType.Form, string name = null);


        //Task<byte[]> ApplyFilterAutomation(ImageSource originalSource, float brightNess, float color, float contrast);
        //Task<byte[]> ApplyBrightnessFilterAutomation(ImageSource originalSource, float brightNess);
        //Task<byte[]> ApplyColorFilterAutomation(ImageSource originalSource, float color);
        //Task<byte[]> ApplyContrastFilterAutomation(ImageSource originalSource, float contrast);

        //Task<byte[]> GetByteFromImageSource(ImageSource originalSource);

        //ImageSource OverrideSaveFileImageSource(string localPrivateFilePath, byte[] byteImage);

        //string GetWidthHeight(byte[] imageData, string imagePath);

    }
}