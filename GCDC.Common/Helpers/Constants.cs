using System.Globalization;
using Umbraco.Cms.Web.Common.AspNetCore;

namespace GCDC.Common.Helpers
{
    public class Constants
    {
        public const string cms_component_name = "cms_component_name";
        public const string cms_component_view_name = "cms_component_view_name";
        public const string cms_page_name = "cms_page_name";
        public const string cms_page_view_name = "cms_page_view_name";

        public const string LayoutViewLocation = "~/Views/Shared/_Layout.cshtml";
        public const string PageViewLocation = "~/Views/Pages/";
        public const string ComponentViewLocation = "~/Views/Partials/Components/";

        public static class Languages
        {
            public const string Default = "en";
            public const string ArabicLanguageCode = "ar";
            public static string Current = string.Empty;
        }

        public static CultureInfo currentCulture = new System.Globalization.CultureInfo("en");


        public static class RootAlias
        {
            public const string DataSources = "dataSources";
            public const string Site = "site";
            public const string FormSubmissionNode = "formSubmissions";
            public const string FormCollectionsNode = "formCollections";
        }

        #region Filters
        public static class Filters
        {
            public const string NewsCategory = "news";
            public const string MediaCategory = "mediaGallery";
            public const string Year = "year";
            public const string TagsItem = "tagsItem";
            public const string SearchQuery = "SearchQuery";

        }

        public static class QueryFilters
        {
            public const string NewsCategoryType = "type";
            public const string FaqsCategoryType = "type";
            public const string MediaType = "type";
            public const string Year = "text";
            public const string Search = "text";
        }

        #endregion

        #region Content Types
        public static class ContentTypes
        {
            public const string News = "newsAndMedia";
            public const string FreqAskedQuestions = "freqAskedQuestions";
            public const string ImagesVideos = "imagesAndVideos";
            public const string SearchText = "Media-Centre";

        }

        #endregion
        public static class DictionaryKeys
        {
            public const string Back = "Back";
            public const string EmailPlaceholder = "EmailPlaceholder";
            public const string FirstNamePlaceholder = "FirstNamePlaceholder";
            public const string LastNamePlaceholder = "LastNamePlaceholder";
            public const string MessagePlaceholder = "MessagePlaceholder";
            public const string PhonePlaceholder = "PhonePlaceholder";
            public const string SubmitButton = "SubmitButton";
            public const string SuccessMessage = "SuccessMessage";
            public const string MessageSent = "MessageSent";
            public const string InquirySuccess = "InquirySuccess";
            public const string EmailValidation = "EmailValidation";
            public const string RecaptchaValidationFailed = "Recaptcha Validation Failed";

        }

        #region 
        public class _2FAConfiguration
        {
            public const string ApplicationName = "GCDC App Authenticator";
        }
        #endregion

        #region Labels
        public static class Labels
		{
			public const string GB = "GB Label";
			public const string MB = "MB Label";
			public const string KB = "KB Label";
			public const string Bytes = "Bytes Label";
		}
		#endregion
	}
}