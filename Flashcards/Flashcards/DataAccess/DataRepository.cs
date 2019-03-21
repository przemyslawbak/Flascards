using Flashcards.Models;
using LumenWorks.Framework.IO.Csv;
using Plugin.FilePicker;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Flashcards.DataAccess
{
    /// <summary>
    /// Repository for data access from SQLite and CSV
    /// </summary>
    public class DataRepository : IDataRepository
    {
        SQLiteConnection database;
        static object locker = new object();
        public DataRepository()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Phrase>();
        }
        public void Dispose()
        {
            // do something
        }
        public IEnumerable<Phrase> GetPhrases()
        {
            lock (locker)
            {
                return (from p in database.Table<Phrase>()
                        select p).ToList();
            }
        }
        public List<string> GetGroups()
        {
            lock (locker)
            {
                List<string> groups = new List<string>();
                var context = database.Table<Phrase>();
                foreach (Phrase phrase in context)
                {
                    groups.Add(phrase.Group);
                }
                return groups.ToList();
            }
        }
        public void SavePhrase(Phrase phrase)
        {
            lock (locker)
            {
                if (phrase.Id != 0)
                {
                    database.Update(phrase);
                }
                else
                {
                    database.Insert(phrase);
                }
            }
        }
        public Phrase GetPhraseById(int phraseId)
        {
            lock (locker)
            {
                var context = database.Table<Phrase>();
                return context.Single(f => f.Id == phraseId);
            }
        }
        public string GetStreamFromCSV(string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    string readContents;
                    using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
                    {
                        readContents = streamReader.ReadToEnd();
                    }
                    return readContents;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception choosing file: " + ex.ToString());
            }
            return null;
        }
        public async Task<string> PickUpFile()
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile();
                if (file != null)
                {
                    return file.FilePath;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception choosing file: " + ex.ToString());
                return "";
            }
        }
    }
}
