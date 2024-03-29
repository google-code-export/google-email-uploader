// Copyright 2007 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
//
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Text;

namespace Google.MailClientInterfaces {
  /// <summary>
  /// Interface for creating the email client objects
  /// </summary>
  public interface IClientFactory {
    /// <summary>
    /// Returns the list of process names that are associated with the client
    /// </summary>
    IEnumerable ClientProcessNames {
      get;
    }

    /// <summary>
    /// Creates the client object.
    /// </summary>
    IClient CreateClient();
  }

  /// <summary>
  /// Interface representing the eMail client. For example: Outlook, Outlook
  /// Express, Thunderbird etc.
  /// </summary>
  public interface IClient : IDisposable {
    /// <summary>
    /// Name of the client.
    /// </summary>
    string Name {
      get;
    }

    /// <summary>
    /// Client stores currently opened by the client.
    /// </summary>
    IEnumerable Stores {
      get;
    }

    /// <summary>
    /// Identifies if the client supports contacts.
    /// </summary>
    bool SupportsContacts {
      get;
    }

    /// <summary>
    /// For opening a new store using the client. For example: opening mbox 
    /// file in Thunderbird or pst in Outlook. If successful the opened store
    /// is added to the enumeration returned by Stores.
    /// </summary>
    /// <param name="filename">Name of the file containing the store.</param>
    /// <returns>Store if successful, null otherwise.</returns>
    IStore OpenStore(string filename);

    /// <summary>
    /// Retuns the list of file names of the stores loaded.
    /// </summary>
    IEnumerable LoadedStoreFileNames {
      get;
    }

    /// <summary>
    /// Identifies whether the client supports a loaded store.
    /// </summary>
    bool SupportsLoadingStore {
      get;
    }
  }

  /// <summary>
  /// The store containing the client email.
  /// </summary>
  public interface IStore {
    /// <summary>
    /// The client that opened this store.
    /// </summary>
    IClient Client {
      get;
    }

    /// <summary>
    /// Persisted name of the store.
    /// </summary>
    string PersistName {
      get;
    }

    /// <summary>
    /// Display name of the store.
    /// </summary>
    string DisplayName {
      get;
    }

    /// <summary>
    /// Folders in the store.
    /// </summary>
    IEnumerable Folders {
      get;
    }

    /// <summary>
    /// Count of number of contacts in the store.
    /// </summary>
    uint ContactCount {
      get;
    }
  
    /// <summary>
    /// Returns the list of contacts available with the client
    /// </summary>
    IEnumerable Contacts {
      get;
    }
  }

  /// <summary>
  /// Enumberation indicating if the folder is special.
  /// </summary>
  public enum FolderKind {
    /// <summary>
    /// The folder is inbox for the client.
    /// </summary>
    Inbox,
    /// <summary>
    /// The folder is sent items for the client.
    /// </summary>
    Sent,
    /// <summary>
    /// The folder stores draft (yet to be sent) mails.
    /// </summary>
    Draft,
    /// <summary>
    /// The folder stores deleted items.
    /// </summary>
    Trash,
    /// <summary>
    /// The folder is user created.
    /// </summary>
    Other,
  }

  /// <summary>
  /// Represents the folder in the client.
  /// </summary>
  public interface IFolder {
    /// <summary>
    /// Kind of the folder.
    /// </summary>
    FolderKind Kind {
      get;
    }

    /// <summary>
    /// The parent folder of this folder. Can be null if this is root folder
    /// in the store.
    /// </summary>
    IFolder ParentFolder {
      get;
    }

    /// <summary>
    /// The store that contains this folder.
    /// </summary>
    IStore Store {
      get;
    }

    /// <summary>
    /// Name of the folder.
    /// </summary>
    string Name {
      get;
    }

    /// <summary>
    /// Enumeration of all the direct subfolders of this folder.
    /// </summary>
    IEnumerable SubFolders {
      get;
    }

    /// <summary>
    /// Count of number of mails in the folder.
    /// </summary>
    uint MailCount {
      get;
    }

    /// <summary>
    /// Enumeration of the email in this folder.
    /// </summary>
    /// <remarks>
    /// To implementers: It is recomended that the mails are not stored in
    /// array lists as this would make them unclaimable by the garbage
    /// collector leading to blowup of memory usage. This property is best
    /// delay evaluated and not cached. Mails are to be read as the
    /// enumeration is done.
    /// </remarks>
    IEnumerable Mails {
      get;
    }
  }

  /// <summary>
  /// Represents the email.
  /// </summary>
  public interface IMail : IDisposable {
    /// <summary>
    /// Folder that contains the mail.
    /// </summary>
    IFolder Folder {
      get;
    }

    /// <summary>
    /// Identifier representing the mail. This should be same across
    /// Multiple instantiations of the client and should be persistable.
    /// </summary>
    string MailId {
      get;
    }

    /// <summary>
    /// Flag indicating that mail is marked in read state.
    /// </summary>
    bool IsRead {
      get;
    }

    /// <summary>
    /// Flag indicating that mail is marked/flagged in the client
    /// </summary>
    bool IsStarred {
      get;
    }

    /// <summary>
    /// Size of the message. Could be approximation. The exact size is given
    /// by the size of array returned by Rfc822Buffer. This is meant to
    /// filter out really huge mails.
    /// </summary>
    uint MessageSize {
      get;
    }

    /// <summary>
    /// Represents the rfc822 encoding of the contents of email. This includes
    /// the body as well as the attachments. In case of failure to read the
    /// message this returns an empty array.
    /// </summary>
    byte[] Rfc822Buffer {
      get;
    }
  }

  /// <summary>
  /// Represents the contact.
  /// </summary>
  public interface IContact : IDisposable {
    string ContactId {
      get;
    }

    string Title {
      get;
    }

    string OrganizationName {
      get;
    }

    string OrganizationTitle {
      get;
    }

    string HomePageUrl {
      get;
    }

    string Notes {
      get;
    }

    IEnumerable EmailAddresses {
      get;
    }

    IEnumerable IMIdentities {
      get;
    }

    IEnumerable PhoneNumbers {
      get;
    }

    IEnumerable PostalAddresses {
      get;
    }
  }

  public enum ContactRelation {
    Home,
    Mobile,
    Pager,
    Work,
    HomeFax,
    WorkFax,
    Other,
    Label,
  }

  public class ContactElement {
    string label;
    ContactRelation relation;

    public ContactElement(string label,
                          ContactRelation relation) {
      this.label = label;
      this.relation = relation;
    }

    public string Label {
      get {
        return this.label;
      }
    }

    public ContactRelation Relation {
      get {
        return this.relation;
      }
    }
  }

  public class EmailContact : ContactElement {
    string emailAddress;
    bool isPrimary;

    public EmailContact(string emailAddress,
                        string label,
                        ContactRelation relation,
                        bool isPrimary)
      : base(label, relation) {
      this.emailAddress = emailAddress;
      this.isPrimary = isPrimary;
    }

    public string EmailAddress {
      get {
        return this.emailAddress;
      }
    }

    public bool IsPrimary {
      get {
        return this.isPrimary;
      }
    }
  }

  public class IMContact : ContactElement {
    string imAddress;
    string protocol;

    public IMContact(string imAddress,
                     string protocol,
                     string label,
                     ContactRelation relation)
      : base(label, relation) {
      this.imAddress = imAddress;
      this.protocol = protocol;
    }

    public string IMAddress {
      get {
        return this.imAddress;
      }
    }

    public string Protocol {
      get {
        return this.protocol;
      }
    }
  }

  public class PhoneContact : ContactElement {
    string phoneNumber;

    public PhoneContact(string phoneNumber,
                        string label,
                        ContactRelation relation)
      : base(label, relation) {
      this.phoneNumber = phoneNumber;
    }

    public string PhoneNumber {
      get {
        return this.phoneNumber;
      }
    }
  }

  public class PostalContact : ContactElement {
    string postalAddress;

    public PostalContact(string postalAddress,
                         string label,
                         ContactRelation relation)
      : base(label, relation) {
      this.postalAddress = postalAddress;
    }

    public string PostalAddress {
      get {
        return this.postalAddress;
      }
    }
  }

  /// <summary>
  /// Assembly level attribute used to identify the type implementing
  /// IClientFactory in the assembly which implements mail reading from
  /// client.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public class ClientFactoryAttribute : Attribute {
    Type clientFactoryType;

    /// <summary>
    /// Constructor for the MailClientAttrbute.
    /// </summary>
    /// <param name="clientFactoryType">
    /// The type implementing IClientFactory
    /// </param>
    public ClientFactoryAttribute(Type clientFactoryType) {
      this.clientFactoryType = clientFactoryType;
    }

    /// <summary>
    /// Returns the mail client type that implements IClientFactory.
    /// </summary>
    public Type ClientFactoryType {
      get {
        return this.clientFactoryType;
      }
    }
  }
}
