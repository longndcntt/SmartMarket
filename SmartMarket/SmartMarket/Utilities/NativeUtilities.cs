using System;
using SmartMarket.Interfaces;
using Xamarin.Forms;

namespace SmartMarket.Utilities
{
    public class NativeUtilities
    {
        private static readonly Lazy<INativeUtilities> Implementation = new Lazy<INativeUtilities>(CreateMedia,
            System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static INativeUtilities Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
#if DEBUG
                    throw NotImplementedInReferenceAssembly();
#endif
                }
                return ret;
            }
        }

        private static INativeUtilities CreateMedia()
        {
            return DependencyService.Get<INativeUtilities>();
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
