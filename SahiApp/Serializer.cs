using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SahiApp
{
    // דרישה 7: מנגנון Serialization לשמירה ושחזור
    public static class Serializer
    {
        private static string filePath = "sahi_data.dat";

        public static void SaveData(DataManager data)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("שגיאה בשמירת הנתונים: " + ex.Message);
            }
        }

        public static DataManager LoadData()
        {
            if (!File.Exists(filePath)) return null;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (DataManager)formatter.Deserialize(fs);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}