using System;
using System.Drawing;
using System.IO;

namespace sort_images_by_dimensions.App
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Path to picture folder is expected.");
      }

      var folderPath = args[0];
      if (!Directory.Exists(folderPath))
      {
        Console.WriteLine("Path not found");
      }

      var processor = new ImageManipulator(folderPath);
      processor.SortCopyByDimension();
    }
  }

  public class ImageManipulator
  {
    private string _srcPath { get; set; }
    private string _destVerticallySortedPath { get; set; }
    private string _destHorizontallySortedPath { get; set; }
    private string _destSquaredSortedPath { get; set; }
    private const double _ratioBound = (double)4 / (double)3;
    private const bool _overwriteAtDestination = true;
    public ImageManipulator(string path)
    {
      _srcPath = path;
      _destHorizontallySortedPath = $"{path}/Horizontal";
      _destVerticallySortedPath = $"{path}/Vertical";
      _destSquaredSortedPath = $"{path}/Squared";
      CheckFolderReady(_destHorizontallySortedPath);
      CheckFolderReady(_destVerticallySortedPath);
      CheckFolderReady(_destSquaredSortedPath);
    }

    public void SortCopyByDimension()
    {
      foreach (var file in Directory.GetFiles(_srcPath))
      {
        ProcessImage(file);
      }
    }

    private void ProcessImage(string imagePath)
    {
      double ratio = default(double);
      using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
      {
        try
        {

          using (Image original = Image.FromStream(fs))
          {
            ratio = (double)original.Width / (double)original.Height;
          }
        }
        catch
        {
          Console.WriteLine($"Error while reading file: {imagePath}");
        }
      }

      try
      {
        if (ratio == 0)
        {
          Console.WriteLine($"Error while reading file: {imagePath}");
        }
        else if (ratio > 1)
        {
          File.Copy(imagePath, _destHorizontallySortedPath, _overwriteAtDestination);
        }
        else if (ratio < 1)
        {
          File.Copy(imagePath, _destVerticallySortedPath, _overwriteAtDestination);
        }
        else
        {
          File.Copy(imagePath, _destSquaredSortedPath, _overwriteAtDestination);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void CheckFolderReady(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }
  }
}
