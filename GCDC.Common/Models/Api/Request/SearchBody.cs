
using Azure;
using GCDC.Common.Models.CMS;
using Org.BouncyCastle.Asn1.Ocsp;
using static Umbraco.Cms.Core.Constants.HttpContext;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GCDC.Common.Models.Api.Request
{
    public class SearchBody
    {
        private int? pageSize;
        private int? pageNo;
        private List<Filter> searchQuery;
        private string? searchType;
        private string? category;

        public List<Filter>? SearchQuery { get { return searchQuery ?? new List<Filter>(); } set { searchQuery = value; } }
        public int Skip => (PageSize.Value) * (PageNo.Value - 1);
        public int? PageNo { get { return pageNo ?? 1; } set { pageNo = value; } }
        public string? Category { get; set; }

        public int? PageSize { get { return pageSize ?? 6; } set { pageSize = value; } }
        public bool? IsActive { get; set; }
        public string? Culture { get; set; }
        public bool SkipPagination { get; set; }
        public string? ContentType { get; set; }
        public string? CurrentItemId { get; set; }
        public List<Filter>? Filters { get; set; }
    }

    public class Filter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}


