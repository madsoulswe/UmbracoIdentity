using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Umbraco.Web.Models;

namespace UmbracoIdentity.Models
{
    public class UmbracoIdentityMember : IdentityMember<int, IdentityMemberLogin<int>, IdentityMemberRole<int>, IdentityMemberClaim<int>>
    {
        /// <summary>
        /// Gets/sets the members real name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The member type alias
        /// </summary>
        /// <remarks>
        /// Setting this will only have an affect when creating a new member
        /// </remarks>
        public string MemberTypeAlias { get; set; }

        /// <summary>
        /// Overridden to make the retrieval lazy
        /// </summary>
        public override ICollection<IdentityMemberRole<int>> Roles
        {
            get
            {
                if (_getRoles != null && !_getRoles.IsValueCreated)
                {
                    _roles = new ObservableCollection<IdentityMemberRole<int>>();
                    foreach (var l in _getRoles.Value)
                    {
                        _roles.Add(l);
                    }
                    //now assign events
                    _roles.CollectionChanged += Roles_CollectionChanged;
                }
                return _roles;
            }
        }

        /// <summary>
        /// Overridden to make the retrieval lazy
        /// </summary>
        public override ICollection<IdentityMemberLogin<int>> Logins
        {
            get
            {
                if (_getLogins != null && !_getLogins.IsValueCreated)
                {
                    _logins = new ObservableCollection<IdentityMemberLogin<int>>();
                    foreach (var l in _getLogins.Value)
                    {
                        _logins.Add(l);
                    }
                    //now assign events
                    _logins.CollectionChanged += Logins_CollectionChanged;
                }
                return _logins;
            }
        }

        public bool LoginsChanged { get; private set; }
        public bool RolesChanged { get; private set; }

        void Logins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LoginsChanged = true;
        }
        void Roles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RolesChanged = true;
        }

        private ObservableCollection<IdentityMemberLogin<int>> _logins;
        private Lazy<IEnumerable<IdentityMemberLogin<int>>> _getLogins;

        private ObservableCollection<IdentityMemberRole<int>> _roles;
        private Lazy<IEnumerable<IdentityMemberRole<int>>> _getRoles;

        /// <summary>
        /// Used to set a lazy call back to populate the user's Login list
        /// </summary>
        /// <param name="callback"></param>
        public void SetLoginsCallback(Lazy<IEnumerable<IdentityMemberLogin<int>>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            _getLogins = callback;
        }

        /// <summary>
        /// Used to set a lazy call back to populate the user's Role list
        /// </summary>
        /// <param name="callback"></param>
        public void SetRolesCallback(Lazy<IEnumerable<IdentityMemberRole<int>>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            _getRoles = callback;
        }

        /// <summary>
        /// Returns the member properties
        /// </summary>
        public List<UmbracoProperty> MemberProperties { get; set; }
    }
}