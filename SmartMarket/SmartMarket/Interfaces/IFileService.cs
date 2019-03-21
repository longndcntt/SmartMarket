using System.Threading.Tasks;
using FotoScan.Tablet.Enums;
using FotoScan.Tablet.Models.FileModels;
using Xamarin.Forms;

namespace FotoScan.Tablet.Interfaces
{
    public interface IFileService
    {
        string FilePath { get; }

        bool DeleteFile(string filePath);

        byte[] GetCompressedBitmap(string imagePath);

        ImageSource SaveImageCompressed(string localPrivateFilePath, byte[] compressedImage, LeadType type = LeadType.Form,
            string name = "");

        Task<FileModel> SaveFile(string localPrivateFilePath, string eventName , LeadType type = LeadType.Form,  string name = null, string content = null);

        Task<FileModel> SaveNotesFile(string eventName , LeadType type = LeadType.Form,  string name = null, string content = null, string exitPath = null);

        Task<FileModel> SaveXmlFile(string eventName, LeadType type = LeadType.Form, string name = null, string content = null, string exitPath = null);

        Task<FileModel> SaveSketch(byte[] byteImage, string eventName , LeadType type = LeadType.Form,  string name = null, string exitPath = null);

        Task<FileModel> SaveFilePdf(string localPrivateFilePath, string eventName, LeadType type = LeadType.Form, string name = null, string content = null);
    }
}