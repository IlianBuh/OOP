using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.IO.Pipes;

namespace GraphicEditor.intern.serializer
{
    class Serializer
    {

        public Serializer (){
            
        }
        public void LoadFile(Canvas canvas) {
            string path = this.selectFileName(new OpenFileDialog());
            if (path == "") {
                return;
            }

            this.loadPngImageToCanvas(path, canvas);
        }
        public void SaveCanvas(Canvas canvas) {
            string path = this.selectFileName(new SaveFileDialog());
            if (path == "") {
                return;
            }

            this.saveCanvasToPngImage(path, canvas, new PngBitmapEncoder());

        }
        private void loadPngImageToCanvas(string imagePath, Canvas canvas){
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

                BitmapDecoder decoder = BitmapDecoder.Create(
                    fileStream,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.OnLoad
                );

                BitmapFrame frame = decoder.Frames[0];

                Image imageControl = new Image
                {
                    Source = frame,
                    Width = frame.PixelWidth,
                    Height = frame.PixelHeight,
                    Stretch = Stretch.None
                };

                Canvas.SetLeft(imageControl, 0);
                Canvas.SetTop(imageControl, 0);

                canvas.Children.Add(imageControl);

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Invalid path");
            }
            catch (IOException)
            {
                MessageBox.Show($"Failed to load file");
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error");
            }
            finally {
                try {
                    fileStream?.Close();
                } 
                catch {
                    MessageBox.Show("Failed to close file");
                } 
            }
        }
        private void saveCanvasToPngImage(string filePath, Canvas canvas, BitmapEncoder encoder)
        {
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)width, (int)height,
                96d, 96d, 
                PixelFormats.Pbgra32);

            renderBitmap.Render(canvas);

            FileStream fileStream = null; 
            try { 
                fileStream = new FileStream(filePath, FileMode.Create);
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fileStream);
                fileStream.Close();
                MessageBox.Show("File saved");
            } catch(Exception e) {
                MessageBox.Show("Failed to save: " + e.Message);
            }
            finally {
                try {
                    fileStream?.Close();
                }
                catch {
                    MessageBox.Show("Failed to close file");
                }
            }
        }
        private string selectFileName(FileDialog dialog) {
            var res = "";
            dialog.Filter = "PNG Image|*.png";
            if (dialog.ShowDialog() == true) {
                res = dialog.FileName;
            }

            return res;
        }
    }
}
