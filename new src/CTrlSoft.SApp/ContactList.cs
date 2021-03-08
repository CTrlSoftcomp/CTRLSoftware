using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTrlSoft.SApp
{
    /// <summary>
    /// This class provides the data for the contact module.
    /// </summary>
    class ContactList
    {

        #region Private variables
        private string fullname, company, file_as, country_region;
        private string business_phone, business_fax, homephone, mobilephone;
        private string email, categories;
        private bool journal;
        #endregion

        #region Constructor
      
        public ContactList(string fullname, string company, string country_region, string business_phone,
                            bool journal)
        {
            this.fullname = fullname;
            this.company = company;
            this.country_region = country_region;
            this.business_phone = business_phone;
            this.journal = journal;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FULLNAME
        {
            get { return fullname; }
            set { fullname = value; }
        }

        /// <summary>
        /// Gets or sets the files for the contact.
        /// </summary>
        public string FILE_AS
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string COMPANY
        {
            get { return company; }
            set { company = value; }
        }

        /// <summary>
        /// Gets or sets the country region.
        /// </summary>
        public string COUNTRY_REGION
        {
            get { return country_region; }
            set { country_region = value; }
        }

        /// <summary>
        /// Gets or sets the business contact numbers.
        /// </summary>
        public string BUSINESS_PHONE
        {
            get { return business_phone; }
            set { business_phone = value; }
        }

        /// <summary>
        /// Gets or sets the business FAX number.
        /// </summary>
        public string BUSINESS_FAX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the home contact number.
        /// </summary>
        public string HOMEPHONE
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mobile contact number.
        /// </summary>
        public string MOBILEPHONE
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the EMAIL address.
        /// </summary>
        public string EMAIL
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the journal.
        /// </summary>
        public bool JOURNAL
        {
            get { return journal; }
            set { journal = value; }
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public string CATEGORIES
        {
            get;
            set;
        }

        #endregion

    }
}
