using FlipProof.Image.Matrices;

namespace FlipProof.Image.IO;
internal class MatrixReader
{

   public static DenseMatrix<double>[] ReadAsciiMatrix(string outputFolder, int expectedRows, int expectedColumns)
   {
      string[] matFiles = (from a in Directory.GetFiles(outputFolder)
                           orderby a
                           select a).ToArray();
      DenseMatrix<double>[] matrices = new DenseMatrix<double>[matFiles.Length];
      for (int f = 0; f < matFiles.Length; f++)
      {
         string cur = matFiles[f];
         DenseMatrix<double> mat = ReadAsciiMatrix(expectedRows, expectedColumns, cur);
         matrices[f] = mat;
      }
      return matrices;
   }

   public static DenseMatrix<double> ReadAsciiMatrix(int expectedRows, int expectedColumns, string loc)
   {
      string[] lines = File.ReadAllLines(loc);
      return ParseAsciiMatrix(expectedRows, expectedColumns, lines, loc);
   }

   public static DenseMatrix<double> ParseAsciiMatrix(int expectedRows, int expectedColumns, string asciiMatrix)
   {
      string[] s = asciiMatrix.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
      return ParseAsciiMatrix(expectedRows, expectedColumns, s);
   }

   public static DenseMatrix<double> ParseAsciiMatrix(int expectedRows, int expectedColumns, string[] lines, string descriptionForError = "")
   {
      if (lines.Length != expectedRows)
      {
         throw new Exception("Matrix was not " + expectedRows + " * " + expectedColumns + ": " + descriptionForError);
      }
      DenseMatrix<double> mat = new DenseMatrix<double>(expectedRows, expectedColumns);
      double[][] entries = lines.Select((string a) => (from b in a.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                       select double.Parse(b)).ToArray()).ToArray();
      for (int i = 0; i < expectedColumns; i++)
      {
         double[] row = entries[i];
         if (row.Length != expectedRows)
         {
            throw new Exception("Matrix was not " + expectedRows + " * " + expectedColumns + ": " + descriptionForError);
         }
         for (int j = 0; j < expectedRows; j++)
         {
            mat[i, j] = row[j];
         }
      }
      return mat;
   }
}
