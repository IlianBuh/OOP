using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using GraphicEditor;
using GraphicEditor.intern.serializer;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WpfProject
{
     public class JsonSerializerImpl : ISerializer
     {
         private string _filePath = "shapes.json"; 

         public JsonSerializerImpl()
         {
         }

         public void Serialize(List<AShape> shapes)
         {
             try
             {
                 if (!this.fetchFilePath(new SaveFileDialog()))
                 {
                     return ;
                 }
                 string jsonString = JsonConvert.SerializeObject(shapes, Formatting.Indented, new JsonSerializerSettings
                 {
                     TypeNameHandling = TypeNameHandling.Auto, 
                 });
                 File.WriteAllText(_filePath, jsonString);
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error during serialization: {ex.Message}");
                 throw; 
             }
         }

         public List<AShape>? Deserialise()
         {
             try
             {
                 if (!this.fetchFilePath(new OpenFileDialog()))
                 {
                     return null;
                 }
                 string jsonString = File.ReadAllText(_filePath);
                 List<AShape>? shapes = JsonConvert.DeserializeObject<List<AShape>>(jsonString,
                     new JsonSerializerSettings
                     {
                         TypeNameHandling = TypeNameHandling.Auto,
                     });
                 return shapes;
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error during deserialization: {ex.Message}");
                 return null;
             }
         }

         private bool fetchFilePath(FileDialog fileDialog)
         {
             fileDialog.Filter = "JSON Files (*.json)|*.json";
             fileDialog.DefaultExt = ".json";
             fileDialog.FileName = _filePath;
             if (fileDialog.ShowDialog() == true)
             {
                 _filePath = fileDialog.FileName;
                return true;
             }
             else
             {
                 _filePath = "shapes.json";
                 return false;
             }

         }
     }
}
