using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DECATraining
{

    /*
     * General information about the categories, tests, and test sets available.
     */
    public static class Info
    {

        /*
         * Encapsulates a category name and display string.
         */
        public struct CategoryInfo
        {
            public CategoryName categoryName;
            public String categoryString;

            public CategoryInfo(CategoryName categoryName, String categoryString)
            {
                this.categoryName = categoryName;
                this.categoryString = categoryString;
            }
        }

        /*
         * Category names (identifiers).
         */
        public enum CategoryName
        {
            BusAdmin,
            BusLawEthics,
            BusMgmtAdmin,
            Finance,
            Hospitality,
            Marketing
        }

        /*
        * All available categories.
        */
        public static List<CategoryInfo> availableCategories =
            new List<CategoryInfo>()
                {
                    new CategoryInfo(CategoryName.BusAdmin,     "Business Admin"),
                    new CategoryInfo(CategoryName.BusLawEthics, "Business Law & Ethics"),
                    new CategoryInfo(CategoryName.BusMgmtAdmin, "Business Mgmt & Admin"),
                    new CategoryInfo(CategoryName.Finance,      "Finance"),
                    new CategoryInfo(CategoryName.Hospitality,  "Hospitality"),
                    new CategoryInfo(CategoryName.Marketing,    "Marketing")
                };








        /*
         * Test names (identifiers).
         * Must match the file name (without the extension).
         */
        public enum TestName
        {
            BusAdmin1027,
            BusAdmin1044,
            BusAdmin1061,
            BusAdmin1068,
            BusAdmin1075,

            BusLawEthics0872,
            BusLawEthics0902,
            BusLawEthics0943,
            BusLawEthics0964,
            BusLawEthics0976,
            BusLawEthics0997,
            BusLawEthicsICDC2009,

            BusMgmtAdmin1024,
            BusMgmtAdmin1031,
            BusMgmtAdmin1048,
            BusMgmtAdmin1065,
            BusMgmtAdmin1072,
            BusMgmtAdmin1079,
            BusMgmtAdminICDC2011,
            BusMgmtAdminICDC2012,
            BusMgmtAdminICDC2013,
            BusMgmtAdminSample2010,
            BusMgmtAdminSample2014,

            Finance0865,
            Finance0898,
            Finance0936,
            Finance0942,
            Finance0957,
            Finance0980,
            Finance1001,
            Finance1029,
            Finance1047,
            Finance1064,
            Finance1071,
            Finance1078,

            Hospitality0822,
            Hospitality0866,
            Hospitality0984A,
            Hospitality1013,
            Hospitality1030,
            Hospitality1046,
            Hospitality1063,
            Hospitality1070,
            Hospitality1077,
            HospitalityICDC2011,
            HospitalityICDC2012,
            HospitalityICDC2013,
            HospitalitySample2014,
            HospitalitySample2015,

            Marketing1021,
            Marketing1028,
            Marketing1038,
            Marketing1045,
            Marketing1055,
            Marketing1062,
            Marketing1069,
            Marketing1076,
            Marketing1086,
            Marketing1093,
            MarketingICDC2010,
            MarketingICDC2012,
            MarketingICDC2013,
            MarketingSample
        }








        /*
         * Encapsulates a test set name, display strings (short and long), and test names in the set.
         */
        public struct TestSetInfo
        {
            public TestSetName testSetName;
            public String testSetStringShort;
            public String testSetStringLong;
            public List<TestName> testNames;

            public TestSetInfo(TestSetName testSetName, String testSetStringShort, String testSetStringLong, List<TestName> testNames)
            {
                this.testSetName = testSetName;
                this.testSetStringShort = testSetStringShort;
                this.testSetStringLong = testSetStringLong;
                this.testNames = testNames;
            }
        }

        /*
         * Test set names (identifiers).
         */
        public enum TestSetName
        {
            BusAdminRegional,
            BusAdminProvincial,
            BusAdminOther,

            BusLawEthicsRegional,
            BusLawEthicsProvincial,
            BusLawEthicsICDC,
            BusLawEthicsOther,

            BusMgmtAdminRegional,
            BusMgmtAdminProvincial,
            BusMgmtAdminICDC,
            BusMgmtAdminOther,

            FinanceRegional,
            FinanceProvincial,
            FinanceOther,

            HospitalityRegional,
            HospitalityProvincial,
            HospitalityICDC,
            HospitalityOther,

            MarketingRegional,
            MarketingProvincial,
            MarketingICDC,
            MarketingOther
        }

        /*
         * All available test sets (grouped by category).
         */
        public static Dictionary<CategoryName, List<TestSetInfo>> availableTestSets = new Dictionary<CategoryName, List<TestSetInfo>>()
            {

                {CategoryName.BusAdmin, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.BusAdminRegional, "Regional", "Business Admin - Regional", new List<TestName>()
                            {TestName.BusAdmin1068} ),

                        new TestSetInfo(TestSetName.BusAdminProvincial, "Provincial", "Business Admin - Provincial", new List<TestName>()
                            {TestName.BusAdmin1061, TestName.BusAdmin1075} ),

                        new TestSetInfo(TestSetName.BusAdminOther, "Other", "Business Admin - Other", new List<TestName>()
                            {TestName.BusAdmin1027, TestName.BusAdmin1044} )
                    }
                },

                {CategoryName.BusLawEthics, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.BusLawEthicsRegional, "Regional", "Business Law & Ethics - Regional", new List<TestName>()
                            {TestName.BusLawEthics0943, TestName.BusLawEthics0976} ),

                        new TestSetInfo(TestSetName.BusLawEthicsProvincial, "Provincial", "Business Law & Ethics - Provincial", new List<TestName>()
                            {TestName.BusLawEthics0872, TestName.BusLawEthics0964, TestName.BusLawEthics0997} ),

                        new TestSetInfo(TestSetName.BusLawEthicsICDC, "ICDC", "Business Law & Ethics - ICDC", new List<TestName>()
                            {TestName.BusLawEthicsICDC2009} ),

                        new TestSetInfo(TestSetName.BusLawEthicsOther, "Other", "Business Law & Ethics - Other", new List<TestName>()
                            {TestName.BusLawEthics0902} )
                    }
                },

                {CategoryName.BusMgmtAdmin, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.BusMgmtAdminRegional, "Regional", "Business Mgmt & Admin - Regional", new List<TestName>()
                            {TestName.BusMgmtAdmin1024, TestName.BusMgmtAdmin1072} ),

                        new TestSetInfo(TestSetName.BusMgmtAdminProvincial, "Provincial", "Business Mgmt & Admin - Provincial", new List<TestName>()
                            {TestName.BusMgmtAdmin1065, TestName.BusMgmtAdmin1079} ),

                        new TestSetInfo(TestSetName.BusMgmtAdminICDC, "ICDC", "Business Mgmt & Admin - ICDC", new List<TestName>()
                            {TestName.BusMgmtAdminICDC2011, TestName.BusMgmtAdminICDC2012, TestName.BusMgmtAdminICDC2013} ),

                        new TestSetInfo(TestSetName.BusMgmtAdminOther, "Other", "Business Mgmt & Admin - Other", new List<TestName>()
                            {TestName.BusMgmtAdmin1031, TestName.BusMgmtAdmin1048, TestName.BusMgmtAdminSample2010, TestName.BusMgmtAdminSample2014} )
                    }
                },

                {CategoryName.Finance, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.FinanceRegional, "Regional", "Finance - Regional", new List<TestName>()
                            {TestName.Finance0980, TestName.Finance1071} ),

                        new TestSetInfo(TestSetName.FinanceProvincial, "Provincial", "Finance - Provincial", new List<TestName>()
                            {TestName.Finance0865, TestName.Finance0898, TestName.Finance0957, TestName.Finance1001, TestName.Finance1064, TestName.Finance1078} ),

                        new TestSetInfo(TestSetName.FinanceOther, "Other", "Finance - Other", new List<TestName>()
                            {TestName.Finance0936, TestName.Finance0942, TestName.Finance1029, TestName.Finance1047} )
                    }
                },

                {CategoryName.Hospitality, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.HospitalityRegional, "Regional", "Hospitality - Regional", new List<TestName>()
                            {TestName.Hospitality0984A, TestName.Hospitality1070} ),

                        new TestSetInfo(TestSetName.HospitalityProvincial, "Provincial", "Hospitality - Provincial", new List<TestName>()
                            {TestName.Hospitality0866, TestName.Hospitality1013, TestName.Hospitality1063, TestName.Hospitality1077} ),

                        new TestSetInfo(TestSetName.HospitalityICDC, "ICDC", "Hospitality - ICDC", new List<TestName>()
                            {TestName.HospitalityICDC2011, TestName.HospitalityICDC2012, TestName.HospitalityICDC2013} ),

                        new TestSetInfo(TestSetName.HospitalityOther, "Other", "Hospitality - Other", new List<TestName>()
                            {TestName.Hospitality0822, TestName.Hospitality1030, TestName.Hospitality1046, TestName.HospitalitySample2014, TestName.HospitalitySample2015} ),
                    }
                },

                {CategoryName.Marketing, new List<TestSetInfo>()
                    {
                        new TestSetInfo(TestSetName.MarketingRegional, "Regional", "Marketing - Regional", new List<TestName>()
                            {TestName.Marketing1055, TestName.Marketing1069, TestName.Marketing1086} ),

                        new TestSetInfo(TestSetName.MarketingProvincial, "Provincial", "Marketing - Provincial", new List<TestName>()
                            {TestName.Marketing1062, TestName.Marketing1076} ),

                        new TestSetInfo(TestSetName.MarketingICDC, "ICDC", "Marketing - ICDC", new List<TestName>()
                            {TestName.MarketingICDC2010, TestName.MarketingICDC2012, TestName.MarketingICDC2013} ),

                        new TestSetInfo(TestSetName.MarketingOther, "Other", "Marketing - Other", new List<TestName>()
                            {TestName.Marketing1021, TestName.Marketing1028, TestName.Marketing1038, TestName.Marketing1045, TestName.Marketing1093, TestName.MarketingSample} ),
                    }
                }

            };

    }

}
