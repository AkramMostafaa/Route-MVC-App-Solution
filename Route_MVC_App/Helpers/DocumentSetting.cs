using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Route_MVC_App.PL.Helpers
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            // 1.Get Located Folder Path  
            //var folderPath = Directory.GetCurrentDirectory()+ @"\wwwroot\files\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2. Get File Name And Make It UNIQUE
            string fileName =$"{Guid.NewGuid()}{file.FileName}" ;

            //3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File AS Streams[Data Per Time ]

         using var fileStream = new FileStream(filePath, FileMode.Create); 

            file.CopyTo(fileStream);
            return fileName;


        }

        public static void DeleteFile(string fileName,string folderName) 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
            
        }
    }
}
