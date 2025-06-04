using System.Collections.Generic;

namespace ConvertFactory.Media
{
    /// <summary>
    /// Represents data for a specific media conversion type, including supported extensions and conversion targets.
    /// </summary>
    public class ConversionData
    {
        /// <summary>
        /// Gets the file extension associated with this conversion type.
        /// </summary>
        public string Ext { get; }

        /// <summary>
        /// Gets the list of target formats that this file type can be converted to.
        /// </summary>
        public List<string> To { get; }

        /// <summary>
        /// Initializes a new instance of the ConversionData class.
        /// </summary>
        /// <param name="ext">The file extension for this conversion type.</param>
        /// <param name="to">The list of target formats this file type can be converted to.</param>
        public ConversionData(string ext, List<string> to)
        {
            Ext = ext;
            To = to;
        }
    }
}