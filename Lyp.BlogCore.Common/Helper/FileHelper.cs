using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyp.BlogCore.Common.Helper
{
    public class FileHelper
    {
        public string FileName { get; set; }
        public long Length { get; set; }
        public string Extension { get; set; }
        public string FileType { get; set; }

        private readonly static string[] Filters = { ".jpg", ".png", ".bmp" };
        public bool IsValid => !string.IsNullOrEmpty(this.Extension);// 
        
        private IFormFile file;
        public IFormFile File
        {
            get { return file; }
            set
            {
                if (value != null)
                {
                    this.file = value;

                    this.FileType = this.file.ContentType;
                    this.Length = this.file.Length;
                    this.Extension = this.file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    if (string.IsNullOrEmpty(this.FileName))
                        this.FileName = this.FileName;
                }
            }
        }

        public async Task<string> SaveAs(string destinationDir = null)
        {
            try
            {
                if (this.file == null)
                    throw new ArgumentNullException("没有需要保存的文件");

                if (destinationDir != null)
                    Directory.CreateDirectory(destinationDir);

                var newName = DateTime.Now.Ticks;
                var newFile = Path.Combine(destinationDir ?? "", $"{newName}{this.Extension}");
                using (FileStream fs = new FileStream(newFile, FileMode.CreateNew))
                {
                    await this.file.CopyToAsync(fs);
                    fs.Flush();
                }
                return newFile;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            
        }
    }
}
