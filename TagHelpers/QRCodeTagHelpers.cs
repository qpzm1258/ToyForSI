using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using ZXing.QrCode;
using ZXing.Rendering;
using System.Text;

namespace ToyForSI.TagHelpers
{
    [HtmlTargetElement("qrcode")]
    public class QRCodeTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var QrcodeContent = context.AllAttributes["content"].Value.ToString();
            var alt = context.AllAttributes["alt"].Value.ToString();
            int width = 150;
            int height = 30;
            if(context.AllAttributes.ContainsName("width"))
            {
                int.TryParse(context.AllAttributes["width"].Value.ToString(),out width );
                width=width==0?150:width;
            }
            if(context.AllAttributes.ContainsName("height"))
            {
                int.TryParse(context.AllAttributes["height"].Value.ToString(),out height );
                height=height==0?30:height;
            }
            
            
            //var width = 150; // width of the Qr Code   
            //var height = 30; // height of the Qr Code   
            //var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                //Format = ZXing.BarcodeFormat.QR_CODE,
                //一维码
				Format = ZXing.BarcodeFormat.CODE_128,
                //Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    PureBarcode = true,
                    //Margin = margin,
                    CharacterSet= "UTF-8"
                }
            };
            byte[] bytes = Encoding.Default.GetBytes(QrcodeContent);
            string QrcodeContentUTF8 = Encoding.UTF8.GetString(bytes);
            var pixelData = qrCodeWriter.Write(QrcodeContentUTF8);
            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference   
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB   
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image   
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG  
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                output.TagName = "img";
                output.Attributes.Clear();
                output.Attributes.Add("width", width);
                output.Attributes.Add("height", height);
                output.Attributes.Add("alt", alt);
                output.Attributes.Add("src", String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
                //output.TagName="div";
                //output.Attributes.Clear();
                //output.Content.Append("<span style='position: absolute; top: 0; left: 0;'>"+context.AllAttributes["content"].ToString()+"</span>");
                //output.Content.Append("<img width='"+width+"' height='"+height+"' alt='"+context.AllAttributes["content"].ToString()+"' src='"+String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()))+"'>");
            }
        }
    }
}