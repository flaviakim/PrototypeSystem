using System.Collections.Generic;
using System.IO;

namespace TypeObjectSystem.TypeLoader {
    public abstract class FileTypeLoader<TType> : ITypeLoader<TType> where TType : IType {
        private readonly string _rootFolder = TypeLoadingDefaultValues.RootFolder; 
        private readonly string _relativeFolder;
        protected string FullPath => Path.Combine(_rootFolder, _relativeFolder);

        protected FileTypeLoader(string relativeFolder, string rootFolder = null) {
            _relativeFolder = relativeFolder;
            if (rootFolder != null) {
                _rootFolder = rootFolder;
            }
        }

        public abstract Dictionary<string, TType> LoadAll();
    }
}