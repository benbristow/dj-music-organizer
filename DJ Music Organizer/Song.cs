using System.IO;
using TagLib;

namespace DJ_Music_Organizer
{
    class Song
    {
        private FileInfo file;
        
        public Song(FileInfo file)
        {
            this.file = file;
        }        

        public string getGenre()
        {
            return string.Join(" , ", fileAsTaggable().Tag.Genres);
        }

        public void setGenre()
        {
            TagLib.File taggableFile = fileAsTaggable();
            if (getGenre() != getDirectoryName())
            {
                taggableFile.Tag.Genres = new string[] { getDirectoryName() };
                taggableFile.Save();
            }
        }

        public string getFilePath()
        {
            return file.FullName;
        }

        public string getDirectoryName()
        {
            return file.Directory.Name;
        }

        private TagLib.File fileAsTaggable()
        {
            return TagLib.File.Create(getFilePath());
        }
    }
}
