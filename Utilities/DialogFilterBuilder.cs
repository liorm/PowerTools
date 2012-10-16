using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// Holds various options for the filters construction.
    /// </summary>
    public enum DialogFilterOptions
    {
        /// <summary>
        /// Generate an "All" entry at the end of the list?
        /// </summary>
        IncludeAllEntry
    }

    /// <summary>
    /// Class to encapsulate several dialog filters with convinence operator overloading.
    /// </summary>
    /// <example>
    ///        openFileDialog.Filter =
    ///            new DialogFilter("Word Document", "*.doc", "*.docx") +
    ///            new DialogFilter("Excel Document", "*.xls", "*.xlsx") +
    ///            new DialogFilter("PowerPoint Document", "*.ppt", "*.pptx") + 
    ///            DialogFilterOptions.IncludeAllEntry;
    /// </example>
    public class DialogFilters
    {
        /// <summary>
        /// An empty filters list - useful for single line construction.
        /// </summary>
        public static readonly DialogFilters Empty = new DialogFilters(Enumerable.Empty<DialogFilter>());

        /// <summary>
        /// Construct a new instance with the list of filters.
        /// </summary>
        public DialogFilters(IEnumerable<DialogFilter> a_filters) : 
            this(a_filters, Enumerable.Empty<DialogFilterOptions>())
        {
            
        }

        /// <summary>
        /// Construct a new instance with the list of filters and options.
        /// </summary>
        public DialogFilters(IEnumerable<DialogFilter> a_filters, IEnumerable<DialogFilterOptions> a_options)
        {
            m_filters = a_filters;
            m_options = new HashSet<DialogFilterOptions>(a_options);
        }

        private readonly IEnumerable<DialogFilter> m_filters;
        private readonly HashSet<DialogFilterOptions> m_options;

        /// <summary>
        /// Implicit conversion from a single filter.
        /// </summary>
        public static implicit operator DialogFilters(DialogFilter a_firstFilter)
        {
            return new DialogFilters(new[] { a_firstFilter });
        }

        /// <summary>
        /// Append a filter to the list.
        /// </summary>
        public static DialogFilters operator + (DialogFilters a_filtersList, DialogFilter a_newFilter)
        {
            List<DialogFilter> filters = a_filtersList.m_filters.ToList();
            filters.Add(a_newFilter);

            return new DialogFilters(filters, a_filtersList.Options);
        }

        /// <summary>
        /// Append an option to the filters class.
        /// </summary>
        public static DialogFilters operator + (DialogFilters a_filtersList, DialogFilterOptions a_option)
        {
            return new DialogFilters(a_filtersList.Filters, a_filtersList.Options.Concat(new[] { a_option }));
        }

        /// <summary>
        /// Return the filters list.
        /// </summary>
        public IEnumerable<DialogFilter> Filters { get { return m_filters; } }

        /// <summary>
        /// Return list of associated options.
        /// </summary>
        public IEnumerable<DialogFilterOptions> Options { get { return m_options ?? Enumerable.Empty<DialogFilterOptions>(); } }

        /// <summary>
        /// Implicit conversion to string (construct the filter list).
        /// </summary>
        public static implicit operator string(DialogFilters a_filters)
        {
            return a_filters.ToString();
        }

        public override string ToString()
        {
            return ConstructFilter(this);
        }

        #region ConstructFilter 

        /// <summary>
        /// Construct a filter string from the given entries
        /// </summary>
        /// <param name="a_filters">List of entries</param>
        /// <returns>Formatted common dialog filter string</returns>
        public static string ConstructFilter(DialogFilters a_filters)
        {
            if (!a_filters.Filters.Any())
                return string.Empty;

            StringBuilder resultString = new StringBuilder();

            // Make the "all" option first (to be autoselected).
            if (a_filters.Options.Contains(DialogFilterOptions.IncludeAllEntry))
                resultString.AppendFormat(
                    "{0} ({1})|{1}",
                    "All",
                    ConstructExtensionsString(a_filters.Filters.SelectMany(a_entry => a_entry.Extensions)));

            foreach (var entry in a_filters.Filters)
            {
                if (resultString.Length > 0)
                    resultString.Append('|');

                resultString.AppendFormat(
                    "{0} ({1})|{1}", 
                    entry.Title, 
                    ConstructExtensionsString(entry.Extensions));
            }

            return resultString.ToString();
        }

        private static string ConstructExtensionsString(IEnumerable<string> a_extensions)
        {
            return 
                a_extensions.Skip(1).
                Aggregate(
                    a_extensions.First(),
                    (a_prev, a_new) => a_prev + ";" + a_new);
        }

        #endregion
    }

    /// <summary>
    /// Holds a single filter entry with a title and list of filter strings
    /// </summary>
    /// <example>
    ///        openFileDialog.Filter =
    ///            new DialogFilter("Word Document", "*.doc", "*.docx") +
    ///            new DialogFilter("Excel Document", "*.xls", "*.xlsx") +
    ///            new DialogFilter("PowerPoint Document", "*.ppt", "*.pptx");
    /// </example>
    public class DialogFilter
    {
        /// <summary>
        /// Construct a new filter with a title
        /// </summary>
        public DialogFilter(string a_title)
        {
            m_title = a_title;
            m_extensions = Enumerable.Empty<string>();
        }

        /// <summary>
        /// Construct a new filter with a title and list of filters.
        /// </summary>
        public DialogFilter(string a_title, IEnumerable<string> a_extensions)
        {
            m_title = a_title;
            m_extensions = a_extensions;
        }

        /// <summary>
        /// Construct a new filter with a title and list of filters.
        /// </summary>
        public DialogFilter(string a_title, params string[] a_extensions)
        {
            m_title = a_title;
            m_extensions = a_extensions;
        }

        /// <summary>
        /// Construct a new filter from the given string as a title.
        /// </summary>
        public static implicit operator DialogFilter(string a_title)
        {
            return new DialogFilter(a_title);
        }

        /// <summary>
        /// Append filter string to this filter entry.
        /// </summary>
        public static DialogFilter operator %(DialogFilter a_left, string a_right)
        {
            List<string> filters = a_left.m_extensions.ToList();
            filters.Add(a_right);

            return new DialogFilter(a_left.m_title, filters);
        }

        /// <summary>
        /// Concat 2 filter entries and return a filters list
        /// </summary>
        public static DialogFilters operator + (DialogFilter a_left, DialogFilter a_right)
        {
            return new DialogFilters(new[] { a_left, a_right });
        }

        /// <summary>
        /// Concat 2 filter entries and return a filters list
        /// </summary>
        public static DialogFilters operator + (DialogFilter a_left, DialogFilters a_right)
        {
            return new DialogFilters(new[] { a_left }.Concat(a_right.Filters), a_right.Options);
        }


        private readonly string m_title;

        /// <summary>
        /// Return the entry title.
        /// </summary>
        public string Title { get { return m_title; } }

        private readonly IEnumerable<string> m_extensions;

        /// <summary>
        /// Return the list of filter strings.
        /// </summary>
        public IEnumerable<string> Extensions { get { return m_extensions; } }

        /// <summary>
        /// Implicit conversion to string (construct the filter list).
        /// </summary>
        public static implicit operator string(DialogFilter a_filter)
        {
            return a_filter.ToString();
        }

        public override string ToString()
        {
            return DialogFilters.ConstructFilter(this);
        }

    }
}
