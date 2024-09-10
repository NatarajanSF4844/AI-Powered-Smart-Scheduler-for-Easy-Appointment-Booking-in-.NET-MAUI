using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSchedulerAIAssistant
{
    public class ResourceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceViewModel"/> class.
        /// </summary>
        public ResourceViewModel()
        {
            this.Name = string.Empty;
            this.Id = string.Empty;
            this.Background = Brush.Transparent;
            this.Foreground = Brush.Black;
            this.ImageName = string.Empty;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///  Gets or sets the Background.
        /// </summary>
        public Brush Background { get; set; }

        /// <summary>
        ///  Gets or sets the Foreground.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        ///  Gets or sets the ImageName.
        /// </summary>
        public string ImageName { get; set; }

        #endregion
    }
}
